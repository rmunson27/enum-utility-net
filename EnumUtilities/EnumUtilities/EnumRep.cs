using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Rem.Core.Utilities;

/// <summary>
/// Internally loads the details of the type <typeparamref name="TEnum"/> at static construction time so that the
/// library can perform efficient operations on the type.
/// </summary>
/// <typeparam name="TEnum"></typeparam>
internal static class EnumRep<TEnum> where TEnum : struct, Enum
{
    private static readonly EnumOperations<TEnum> operations;

    internal static readonly EnumUnderlyingType underlyingType;

    private static readonly TEnum[] values = (TEnum[])Enum.GetValues(typeof(TEnum));

    private static readonly HashSet<TEnum> valuesSet = new(values);

    [MemberNotNullWhen(true, nameof(atomicValues))] internal static bool HasFlagsAttribute { get; }

    private static readonly HashSet<TEnum>? atomicValues;

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
            var isAtomic = true;
            for (int j = 0; j < values.Length; j++)
            {
                if (j == i) continue; // Every enum value has itself as a flag

                if (HasFlag(values[i], values[j]))
                {
                    isAtomic = false;
                    break;
                }
            }

            if (isAtomic) atomicValues.Add(values[i]);
        }
    }

    public static TEnum And(TEnum lhs, TEnum rhs) => operations.And(lhs, rhs);

    public static bool HasFlag(TEnum value, TEnum flag) => operations.HasFlag(value, flag);

    public static TEnum Or(TEnum lhs, TEnum rhs) => operations.Or(lhs, rhs);

    public static TEnum Or(IEnumerable<TEnum> values) => operations.Or(values);

    public static TEnum Not(TEnum value) => operations.Not(value);

    public static TEnum XOr(TEnum lhs, TEnum rhs) => operations.XOr(lhs, rhs);

    public static bool Less(TEnum lhs, TEnum rhs) => operations.Less(lhs, rhs);

    public static bool Equal(TEnum lhs, TEnum rhs) => operations.Equal(lhs, rhs);

    public static bool Greater(TEnum lhs, TEnum rhs) => operations.Greater(lhs, rhs);

    public static bool LessOrEqual(TEnum lhs, TEnum rhs) => operations.LessOrEqual(lhs, rhs);

    public static bool GreaterOrEqual(TEnum lhs, TEnum rhs) => operations.GreaterOrEqual(lhs, rhs);

    public static int CompareTo(TEnum lhs, TEnum rhs) => operations.CompareTo(lhs, rhs);

    /// <summary>
    /// Determines whether the specified <typeparamref name="TEnum"/> value is an atomic value (consisting of only
    /// 1 bit flag).
    /// </summary>
    /// <remarks>
    /// If <typeparamref name="TEnum"/> is not decorated with an instance of <see cref="FlagsAttribute"/>, this method
    /// will return whether or not <paramref name="value"/> is defined (as in that case all values are treated
    /// as atomic).
    /// </remarks>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsAtomic(TEnum value) => HasFlagsAttribute ? atomicValues.Contains(value) : IsDefined(value);

    /// <summary>
    /// Gets an array of all atomic values (consisting of only 1 bit flag) of type <typeparamref name="TEnum"/>.
    /// </summary>
    /// <remarks>
    /// If <typeparamref name="TEnum"/> is not decorated with an instance of <see cref="FlagsAttribute"/>, this method
    /// will return all values of the type (as in that case all values are treated as atomic).
    /// </remarks>
    /// <returns></returns>
    public static TEnum[] GetAtomicValues() => HasFlagsAttribute ? atomicValues.ToArray() : GetValues();

    /// <summary>
    /// Gets an array containing all values of type <typeparamref name="TEnum"/>.
    /// </summary>
    /// <returns></returns>
    public static TEnum[] GetValues() => values.ToArray();

    /// <summary>
    /// Determines if the supplied <typeparamref name="TEnum"/> value is a named, defined value of its type, or a
    /// bit set of named, defined values of its type if <typeparamref name="TEnum"/> is decorated with an instance of
    /// <see cref="FlagsAttribute"/>.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsDefined(TEnum value)
        => HasFlagsAttribute ? Equals(value, Or(GetFlags(value))) : valuesSet.Contains(value);

    /// <summary>
    /// Gets a collection of the flags contained in an instance of <typeparamref name="TEnum"/>.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">
    /// <typeparamref name="TEnum"/> is not decorated with an instance of <see cref="FlagsAttribute"/>, and therefore
    /// cannot reliably be treated as a bit set of atomic values of its type.
    /// </exception>
    public static IEnumerable<TEnum> GetFlags(TEnum value)
    {
        if (HasFlagsAttribute)
        {
            foreach (var flag in atomicValues)
            {
                if (value.HasFlag(flag)) yield return flag;
            }
        }
        else throw new InvalidOperationException(
                $"Cannot get flags from enum type '{typeof(TEnum)}' that is not decorated with an instance"
                    + $" of '{nameof(FlagsAttribute)}'.");
    }
}

