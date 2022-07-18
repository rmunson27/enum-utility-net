﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Rem.Core.Utilities;

/// <summary>
/// Constants and static functionality relating to <see langword="enum"/> types.
/// </summary>
/// <remarks>
/// This class offers methods for efficiently comparing and operating on generic <see langword="enum"/> values where
/// the type is not known.
/// </remarks>
public static class Enums
{
    /// <summary>
    /// Computes the bitwise AND (&) of the values passed in.
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static TEnum And<TEnum>(TEnum lhs, TEnum rhs) where TEnum : struct, Enum
        => EnumRep<TEnum>.And(lhs, rhs);

    /// <summary>
    /// Determines if the enum value passed in has the flag passed in.
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <param name="value"></param>
    /// <param name="flag"></param>
    /// <returns></returns>
    public static bool HasFlag<TEnum>(TEnum value, TEnum flag) where TEnum : struct, Enum
        => EnumRep<TEnum>.HasFlag(value, flag);

    /// <summary>
    /// Computes the bitwise OR (|) of the values passed in.
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static TEnum Or<TEnum>(TEnum lhs, TEnum rhs) where TEnum : struct, Enum => EnumRep<TEnum>.Or(lhs, rhs);

    /// <summary>
    /// Computes the bitwise NOT (!) of the value passed in.
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static TEnum Not<TEnum>(TEnum value) where TEnum : struct, Enum => EnumRep<TEnum>.Not(value);

    /// <summary>
    /// Computes the bitwise XOR (^) of the values passed in.
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static TEnum XOr<TEnum>(TEnum lhs, TEnum rhs) where TEnum : struct, Enum
        => EnumRep<TEnum>.XOr(lhs, rhs);

    /// <summary>
    /// Determines if the first value passed in is less than the second.
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool Less<TEnum>(TEnum lhs, TEnum rhs) where TEnum : struct, Enum
        => EnumRep<TEnum>.Less(lhs, rhs);

    /// <summary>
    /// Determines if the two values are equal.
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool Equal<TEnum>(TEnum lhs, TEnum rhs) where TEnum : struct, Enum
        => EnumRep<TEnum>.Equal(lhs, rhs);

    /// <summary>
    /// Determines if the first value passed in is greater than the second.
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool Greater<TEnum>(TEnum lhs, TEnum rhs) where TEnum : struct, Enum
        => EnumRep<TEnum>.Greater(lhs, rhs);

    /// <summary>
    /// Determines if the first value passed in is less than or equal to the second.
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool LessOrEqual<TEnum>(TEnum lhs, TEnum rhs) where TEnum : struct, Enum
        => EnumRep<TEnum>.LessOrEqual(lhs, rhs);

    /// <summary>
    /// Determines if the first value passed in is greater than or equal to the second.
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool GreaterOrEqual<TEnum>(TEnum lhs, TEnum rhs) where TEnum : struct, Enum
        => EnumRep<TEnum>.GreaterOrEqual(lhs, rhs);

    /// <summary>
    /// Compares the two values, returning an integer whose sign describes their relationship.
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns>
    /// An integer that is less than 0 if <paramref name="lhs"/> is less than <paramref name="rhs"/>,
    /// equal to 0 if <paramref name="lhs"/> equals <paramref name="rhs"/>,
    /// and greater than 0 if <paramref name="lhs"/> is greater than <paramref name="rhs"/>.
    /// </returns>
    public static int CompareTo<TEnum>(TEnum lhs, TEnum rhs) where TEnum : struct, Enum
        => EnumRep<TEnum>.CompareTo(lhs, rhs);

    /// <summary>
    /// Determines if the enum value passed in is a named, defined value of type <typeparamref name="TEnum"/>.
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsDefined<TEnum>(TEnum value) where TEnum : struct, Enum => EnumRep<TEnum>.IsDefined(value);

    /// <summary>
    /// Gets all named, defined values of the enum type <typeparamref name="TEnum"/>.
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <returns></returns>
    public static TEnum[] GetValues<TEnum>() where TEnum : struct, Enum => EnumRep<TEnum>.GetValues();
}