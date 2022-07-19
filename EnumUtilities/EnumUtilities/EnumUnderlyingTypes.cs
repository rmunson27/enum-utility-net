using Rem.Core.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Rem.Core.Utilities;

using static EnumUnderlyingType;

/// <summary>
/// Static functionality for the <see cref="EnumUnderlyingType"/> enumeration.
/// </summary>
public static class EnumUnderlyingTypes
{
    private static readonly Dictionary<Type, EnumUnderlyingType> UnderlyingTypeMap;

    static EnumUnderlyingTypes()
    {
        UnderlyingTypeMap = new(8);
        UnderlyingTypeMap.Add(typeof(byte), Byte);
        UnderlyingTypeMap.Add(typeof(sbyte), SByte);
        UnderlyingTypeMap.Add(typeof(short), Short);
        UnderlyingTypeMap.Add(typeof(ushort), UShort);
        UnderlyingTypeMap.Add(typeof(int), Int);
        UnderlyingTypeMap.Add(typeof(uint), UInt);
        UnderlyingTypeMap.Add(typeof(long), Long);
        UnderlyingTypeMap.Add(typeof(ulong), ULong);
    }

    /// <summary>
    /// Gets the <see cref="Type"/> the current <see cref="EnumUnderlyingType"/> instance represents.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    /// <exception cref="InvalidEnumArgumentException">
    /// The current instance was unnamed.
    /// </exception>
    public static Type ToType([NamedEnum] this EnumUnderlyingType type) => type switch
    {
        Byte => typeof(byte),
        SByte => typeof(sbyte),
        Short => typeof(short),
        UShort => typeof(ushort),
        Int => typeof(int),
        UInt => typeof(uint),
        Long => typeof(long),
        ULong => typeof(ulong),
        _ => throw new InvalidEnumArgumentException($"Invalid unnamed {nameof(EnumUnderlyingType)} value."),
    };

    /// <summary>
    /// Attempts to get the <see cref="EnumUnderlyingType"/> instance associated with the supplied type, returning a
    /// boolean value indicating whether or not the operation succeeded.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="value">
    /// An <see langword="out"/> parameter to set to the instance associated with <paramref name="type"/>.
    /// </param>
    /// <returns>Whether or not the operation succeeded.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="type"/> was <see langword="null"/>.</exception>
    public static bool TryFromType(Type type, [NamedEnum] out EnumUnderlyingType value)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));

        return UnderlyingTypeMap.TryGetValue(type, out value);
    }

    /// <summary>
    /// Gets the <see cref="EnumUnderlyingType"/> instance associated with the supplied type.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"><paramref name="type"/> was <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="type"/> is not one of the underlying types representing an <see langword="enum"/>.
    /// </exception>
    [return: NamedEnum] public static EnumUnderlyingType FromType(Type type)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));

        return UnderlyingTypeMap.TryGetValue(type, out var value)
                ? value
                : throw new ArgumentException($"Invalid enum underlying type '{type}'.");
    }
}

/// <summary>
/// Represents the underlying type of an enum.
/// </summary>
public enum EnumUnderlyingType : byte
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
