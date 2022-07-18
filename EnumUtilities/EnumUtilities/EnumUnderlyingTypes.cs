using System;
using System.Collections.Generic;
using System.Text;

namespace Rem.Core.Utilities;

/// <summary>
/// Static functionality for the <see cref="EnumUnderlyingType"/> enumeration.
/// </summary>
internal static class EnumUnderlyingTypes
{
    private static readonly Dictionary<Type, EnumUnderlyingType> UnderlyingTypeMap;

    static EnumUnderlyingTypes()
    {
        UnderlyingTypeMap = new(8);
        UnderlyingTypeMap.Add(typeof(byte), EnumUnderlyingType.Byte);
        UnderlyingTypeMap.Add(typeof(sbyte), EnumUnderlyingType.SByte);
        UnderlyingTypeMap.Add(typeof(short), EnumUnderlyingType.Short);
        UnderlyingTypeMap.Add(typeof(ushort), EnumUnderlyingType.UShort);
        UnderlyingTypeMap.Add(typeof(int), EnumUnderlyingType.Int);
        UnderlyingTypeMap.Add(typeof(uint), EnumUnderlyingType.UInt);
        UnderlyingTypeMap.Add(typeof(long), EnumUnderlyingType.Long);
        UnderlyingTypeMap.Add(typeof(ulong), EnumUnderlyingType.ULong);
    }

    /// <summary>
    /// Gets the <see cref="EnumUnderlyingType"/> instance associated with the given type.
    /// </summary>
    /// <param name="underlyingType"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">
    /// The type is not one of the underlying types representing an <see langword="enum"/>.
    /// </exception>
    internal static EnumUnderlyingType Map(Type underlyingType)
        => UnderlyingTypeMap.TryGetValue(underlyingType, out var value)
            ? value
            : throw new InvalidOperationException($"Invalid underlying enum type '{underlyingType}'.");
}

/// <summary>
/// Represents the underlying type of an enum.
/// </summary>
internal enum EnumUnderlyingType : byte
{
    /// <summary>
    /// Indicates that an enum uses a <see cref="byte"/> as its underlying type.
    /// </summary>
    Byte,

    /// <summary>
    /// Indicates that an enum uses an <see cref="sbyte"/> as its underlying type.
    /// </summary>
    SByte,

    /// <summary>
    /// Indicates that an enum uses a <see cref="short"/> as its underlying type.
    /// </summary>
    Short,

    /// <summary>
    /// Indicates that an enum uses a <see cref="ushort"/> as its underlying type.
    /// </summary>
    UShort,

    /// <summary>
    /// Indicates that an enum uses an <see cref="int"/> as its underlying type.
    /// </summary>
    Int,

    /// <summary>
    /// Indicates that an enum uses a <see cref="uint"/> as its underlying type.
    /// </summary>
    UInt,

    /// <summary>
    /// Indicates that an enum uses a <see cref="long"/> as its underlying type.
    /// </summary>
    Long,

    /// <summary>
    /// Indicates that an enum uses a <see cref="ulong"/> as its underlying type.
    /// </summary>
    ULong,
}
