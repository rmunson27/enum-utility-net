using System;
using System.Collections.Generic;
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

    static EnumRep()
    {
        underlyingType = EnumUnderlyingTypes.FromType(typeof(TEnum).GetEnumUnderlyingType());

#pragma warning disable CS8524 // The underlying types returned are always named
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

    public static TEnum[] GetValues() => values.ToArray();

    public static bool IsDefined(TEnum value) => valuesSet.Contains(value);
}

