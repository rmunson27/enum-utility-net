using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

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

    private static readonly HashSet<TEnum> valuesSet = new(values);

    [MemberNotNullWhen(true, nameof(atomicValues))] internal static bool HasFlagsAttribute { get; }

    private static readonly HashSet<TEnum>? atomicValues;
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

        // Build the atomic values set
        atomicValues = new();

        // Go through every enum value to see if it is included in another
        for (int i = 0; i < values.Length; i++)
        {
            // Never include the default in the list of atomic values, even if it is named
            if (Equal(values[i], default)) continue;

            var isAtomic = true;
            for (int j = 0; j < values.Length; j++)
            {
                if (j == i) continue; // Every enum value has itself as a flag
                if (Equal(values[j], default)) continue; // Every enum value has the default as a flag

                if (HasFlag(values[i], values[j]))
                {
                    isAtomic = false;
                    break;
                }
            }

            if (isAtomic) atomicValues.Add(values[i]);
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
    public static bool IsDefined(TEnum value)
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
    public static TEnum[] GetAtomicValues() => HasFlagsAttribute ? atomicValues.ToArray() : GetValues();

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
    private static bool IsDefinedNoFlags(TEnum value) => valuesSet.Contains(value);

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
}

