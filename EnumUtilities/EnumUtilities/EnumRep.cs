using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using Rem.Core.Attributes;

namespace Rem.Core.Utilities;

/// <summary>
/// Internally loads the details of the type <typeparamref name="TEnum"/> at static construction time so that the
/// library can perform efficient operations on the type.
/// </summary>
/// <typeparam name="TEnum"></typeparam>
internal static class EnumRep<TEnum> where TEnum : struct, Enum
{
    #region Properties And Fields
    private static readonly EnumOperations<TEnum> operations;

    internal static readonly EnumUnderlyingType underlyingType;

    private static readonly TEnum[] values = (TEnum[])Enum.GetValues(typeof(TEnum));

    /// <summary>
    /// An <see cref="ImmutableArray{T}"/> containing all named, defined values of type <typeparamref name="TEnum"/>.
    /// </summary>
    public static readonly ImmutableArray<TEnum> Values = values.ToImmutableArray();

    private static readonly HashSet<TEnum> valuesSet = new(values);

    internal static readonly bool HasFlagsAttribute;

    public static readonly ImmutableArray<TEnum> AtomicValues;
    private static readonly HashSet<TEnum> atomicValues;

    /// <summary>
    /// Stores whether or not the named <typeparamref name="TEnum"/> values form a continuous range if there are named
    /// values, otherwise is <see langword="false"/>.
    /// </summary>
    private static readonly bool IsContinuousRange = false;

    /// <summary>
    /// Stores the inclusive lower bound of the continuous range of values if <see cref="IsContinuousRange"/>
    /// is <see langword="true"/>, otherwise is the default.
    /// </summary>
    private static readonly TEnum ContinuousRangeStart = default;

    /// <summary>
    /// Stores the inclusive upper bound of the continuous range of values if <see cref="IsContinuousRange"/>
    /// is <see langword="true"/>, otherwise is the default.
    /// </summary>
    private static readonly TEnum ContinuousRangeEnd = default;
    #endregion

    #region Constructor
    static EnumRep()
    {
        underlyingType = EnumUnderlyingTypes.FromType(typeof(TEnum).GetEnumUnderlyingType());

#pragma warning disable CS8524 // The underlying types returned are always named (and if not it's a bug)
        operations = underlyingType switch
#pragma warning restore CS8524
        {
            EnumUnderlyingType.Byte => new ByteEnumOperations<TEnum>(),
            EnumUnderlyingType.SByte => new SByteEnumOperations<TEnum>(),
            EnumUnderlyingType.Short => new ShortEnumOperations<TEnum>(),
            EnumUnderlyingType.UShort => new UShortEnumOperations<TEnum>(),
            EnumUnderlyingType.Int => new IntEnumOperations<TEnum>(),
            EnumUnderlyingType.UInt => new UIntEnumOperations<TEnum>(),
            EnumUnderlyingType.Long => new LongEnumOperations<TEnum>(),
            EnumUnderlyingType.ULong => new ULongEnumOperations<TEnum>(),
        };

        HasFlagsAttribute = typeof(TEnum).GetCustomAttributes(typeof(FlagsAttribute), inherit: false).Length > 0;

        // Build atomic value set
        var atomicValuesBuilder = ImmutableArray.CreateBuilder<TEnum>();
        foreach (var v in values)
        {
            if (Equal(v, default)) continue; // The default is never considered an atomic value

            // Need to search for components that union together to create v
            // Only relevant values to search are ones that are direct bit subsets (equal values are excused from being
            // considered non-atomic just because another named value is equal)
            // If the union of all such elements equals the value, it is not atomic
            // Otherwise, it is.
            var possibleComponents = values.Where(v2 => !Equal(v2, default) && !Equal(v, v2) && HasFlag(v, v2));
            if (possibleComponents.Any())
            {
                var possibleComponentUnion = possibleComponents.Aggregate(Or);
                var b = Equal(possibleComponentUnion, v);
                if (!Equal(possibleComponentUnion, v)) atomicValuesBuilder.Add(v);
            }
            else atomicValuesBuilder.Add(v); // Must be atomic since it has no flags in the type
        }
        AtomicValues = atomicValuesBuilder.ToImmutable();
        atomicValues = new(atomicValuesBuilder);

        if (values.Length > 0)
        {
            // Determine if is continuous range
            SortedSet<BigInteger> sortedNumericValues = new(values.Select(operations.GetNumericValue));
            var firstValue = sortedNumericValues.First();
            var currentValue = firstValue;
            bool hasRange = true;
            foreach (var value in sortedNumericValues)
            {
                if (value == currentValue + 1) currentValue = value; // Increment the current value
                else if (value > currentValue) // Is a gap between `currentValue` and `value`
                {
                    hasRange = false;
                    break;
                }
            }
            if (hasRange)
            {
                var lastValue = currentValue;
                IsContinuousRange = true;
                ContinuousRangeStart = operations.FromNumericValue(firstValue);
                ContinuousRangeEnd = operations.FromNumericValue(lastValue);
            }
        }
    }
    #endregion

    #region Methods
    #region Bitwise Operations
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TEnum And(TEnum lhs, TEnum rhs) => operations.And(lhs, rhs);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TEnum Or(TEnum lhs, TEnum rhs) => operations.Or(lhs, rhs);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TEnum Or(IEnumerable<TEnum> values) => operations.Or(values);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TEnum Not(TEnum value) => operations.Not(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TEnum XOr(TEnum lhs, TEnum rhs) => operations.XOr(lhs, rhs);
    #endregion

    #region Comparisons
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Less(TEnum lhs, TEnum rhs) => operations.Less(lhs, rhs);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equal(TEnum lhs, TEnum rhs) => operations.Equal(lhs, rhs);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Greater(TEnum lhs, TEnum rhs) => operations.Greater(lhs, rhs);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool LessOrEqual(TEnum lhs, TEnum rhs) => operations.LessOrEqual(lhs, rhs);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool GreaterOrEqual(TEnum lhs, TEnum rhs) => operations.GreaterOrEqual(lhs, rhs);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int CompareTo(TEnum lhs, TEnum rhs) => operations.CompareTo(lhs, rhs);
    #endregion

    #region Enum Details
    /// <summary>
    /// Gets an array containing all values of type <typeparamref name="TEnum"/>.
    /// </summary>
    /// <returns></returns>
    [return: NameableEnum]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TEnum[] GetValues() => values.ToArray();

    /// <summary>
    /// Determines if the supplied <typeparamref name="TEnum"/> value is a named, defined value of its type, or a
    /// bit set of named, defined values of its type if <typeparamref name="TEnum"/> is decorated with an instance of
    /// <see cref="FlagsAttribute"/>.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsDefined([NamedWhen(true)] TEnum value)
        => HasFlagsAttribute ? IsDefinedFlags(value) : IsDefinedNoFlags(value);
    #endregion

    #region Flags
    /// <summary>
    /// Determines whether the specified <typeparamref name="TEnum"/> value is an atomic value (consisting of only
    /// 1 bit flag).
    /// </summary>
    /// <remarks>
    /// If <typeparamref name="TEnum"/> is not decorated with an instance of <see cref="FlagsAttribute"/>, this method
    /// will return whether or not <paramref name="value"/> is defined (as in that case all values are treated
    /// as atomic).
    /// <para />
    /// It should be noted that if <typeparamref name="TEnum"/> <i>is</i> decorated with an instance of
    /// <see cref="FlagsAttribute"/>, this method will return <see langword="false"/> when called on the default,
    /// even if the default is explicitly named.
    /// </remarks>
    /// <param name="value"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAtomic(TEnum value)
        => HasFlagsAttribute ? atomicValues.Contains(value) : IsDefinedNoFlags(value);

    /// <summary>
    /// Gets an array of all atomic values (consisting of only 1 bit flag) of type <typeparamref name="TEnum"/>.
    /// </summary>
    /// <remarks>
    /// If <typeparamref name="TEnum"/> is not decorated with an instance of <see cref="FlagsAttribute"/>, this method
    /// will return all values of the type (as in that case all values are treated as atomic).
    /// <para />
    /// It should be noted that if <typeparamref name="TEnum"/> <i>is</i> decorated with an instance of
    /// <see cref="FlagsAttribute"/>, the default will not be included in the list of atomic values, even if it
    /// is explicitly named.
    /// </remarks>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TEnum[] GetAtomicValues() => HasFlagsAttribute ? AtomicValues.ToArray() : GetValues();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasFlag(TEnum value, TEnum flag) => operations.HasFlag(value, flag);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasAnyFlag(TEnum value, IEnumerable<TEnum> flags) => operations.HasAnyFlag(value, flags);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<TEnum> PresentFlags(TEnum value, IEnumerable<TEnum> flags)
        => operations.PresentFlags(value, flags);

    /// <summary>
    /// Gets a collection of the flags contained in an instance of <typeparamref name="TEnum"/>.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">
    /// <typeparamref name="TEnum"/> is not decorated with an instance of <see cref="FlagsAttribute"/>, and therefore
    /// cannot reliably be treated as a bit set of atomic values of its type.
    /// </exception>
    public static IEnumerable<TEnum> GetFlags(TEnum value)
        => HasFlagsAttribute
            ? GetFlagsUnsafe(value)
            : throw new InvalidOperationException(
                $"Cannot get flags from enum type '{typeof(TEnum)}' that is not decorated with an instance"
                    + $" of '{nameof(FlagsAttribute)}'.");

    #endregion

    #region Helpers
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsDefinedNoFlags(TEnum value)
        => IsContinuousRange
            ? GreaterOrEqual(value, ContinuousRangeStart) && LessOrEqual(value, ContinuousRangeEnd)
            : valuesSet.Contains(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsDefinedFlags(TEnum value)
        => valuesSet.Contains(value)
            || (!Equal(value, default) // Don't allow the default to be compared for flags or it will always be defined
                    && Equal(value, Or(GetFlagsUnsafe(value))));

    /// <summary>
    /// Gets a collection of the flags contained in an instance of <typeparamref name="TEnum"/> without checking
    /// whether or not its type is decorated with an instance of <see cref="FlagsAttribute"/>.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static IEnumerable<TEnum> GetFlagsUnsafe(TEnum value)
    {
        foreach (var flag in atomicValues!)
        {
            if (operations.HasFlag(value, flag)) yield return flag;
        }
    }
    #endregion
    #endregion

    #region Classes
    public sealed class Comparer : IComparer<TEnum>
    {
        public int Compare(TEnum x, TEnum y) => CompareTo(x, y);
    }
    #endregion
}

