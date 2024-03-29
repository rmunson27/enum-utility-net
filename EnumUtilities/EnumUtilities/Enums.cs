﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

namespace Rem.Core.Utilities;

/// <summary>
/// Constants and static functionality relating to <see langword="enum"/> types.
/// </summary>
/// <remarks>
/// This class offers methods for efficiently comparing and operating on generic <see langword="enum"/> values where
/// the underlying type is not known, as well as adding some more useful set operations on <see langword="enum"/> types
/// that are not available in .NET.
/// </remarks>
public static class Enums
{
    #region Bitwise Operations
    /// <summary>
    /// Computes the bitwise AND (&amp;) of the values passed in.
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static TEnum And<TEnum>(TEnum lhs, TEnum rhs) where TEnum : struct, Enum
        => EnumRep<TEnum>.And(lhs, rhs);

    /// <summary>
    /// Computes the bitwise OR (|) of the values passed in.
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static TEnum Or<TEnum>(TEnum lhs, TEnum rhs) where TEnum : struct, Enum => EnumRep<TEnum>.Or(lhs, rhs);

    /// <summary>
    /// Computes the bitwise OR (|) of the values passed in.
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <param name="values"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"><paramref name="values"/> was <see langword="null"/>.</exception>
    public static TEnum Or<TEnum>(params TEnum[] values) where TEnum : struct, Enum
        => EnumRep<TEnum>.Or(ThrowIfArgNull(values, nameof(values)));

    /// <summary>
    /// Computes the bitwise OR (|) of the values passed in.
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <param name="values"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"><paramref name="values"/> was <see langword="null"/>.</exception>
    public static TEnum Or<TEnum>(IEnumerable<TEnum> values) where TEnum : struct, Enum
        => EnumRep<TEnum>.Or(ThrowIfArgNull(values, nameof(values)));

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
    #endregion

    #region Comparisons
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
    /// Gets an object that can be used to efficiently compare <typeparamref name="TEnum"/> values.
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <returns></returns>
    public static IComparer<TEnum> GetComparer<TEnum>() where TEnum : struct, Enum => new EnumRep<TEnum>.Comparer();
    #endregion

    #region Enum Details
    /// <summary>
    /// Determines if the supplied <typeparamref name="TEnum"/> value is a named, defined value of its type, or a
    /// bit set of named, defined values of its type if <typeparamref name="TEnum"/> is decorated with an instance of
    /// <see cref="FlagsAttribute"/>.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsDefined<TEnum>(TEnum value) where TEnum : struct, Enum => EnumRep<TEnum>.IsDefined(value);

    /// <summary>
    /// Gets all named, defined values of type <typeparamref name="TEnum"/>.
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <returns></returns>
    [Obsolete("Will be removed in an upcoming version. Use `Enums<TEnum>.AtomicValues` instead.")]
    public static TEnum[] GetValues<TEnum>() where TEnum : struct, Enum => EnumRep<TEnum>.GetValues();

    /// <summary>
    /// Gets a value describing the underlying <see langword="enum"/> representation type
    /// of <typeparamref name="TEnum"/>.
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <returns></returns>
    [Obsolete("Will be removed in an upcoming version. Use `Enums<TEnum>.UnderlyingType` instead.")]
    public static EnumUnderlyingType UnderlyingType<TEnum>() where TEnum : struct, Enum
        => EnumRep<TEnum>.underlyingType;
    #endregion

    #region Flags
    /// <summary>
    /// Determines if the <typeparamref name="TEnum"/> value passed in has the flag passed in.
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <param name="value"></param>
    /// <param name="flag"></param>
    /// <returns></returns>
    public static bool HasFlag<TEnum>(TEnum value, TEnum flag) where TEnum : struct, Enum
        => EnumRep<TEnum>.HasFlag(value, flag);

    /// <summary>
    /// Determines if the <typeparamref name="TEnum"/> value passed in has any of the specified flags.
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <param name="value"></param>
    /// <param name="flags"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"><paramref name="flags"/> was <see langword="null"/>.</exception>
    public static bool HasAnyFlag<TEnum>(TEnum value, params TEnum[] flags) where TEnum : struct, Enum
        => EnumRep<TEnum>.HasAnyFlag(value, flags is null ? throw new ArgumentNullException(nameof(flags)) : flags);

    /// <summary>
    /// Determines if the <typeparamref name="TEnum"/> value passed in has any of the specified flags.
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <param name="value"></param>
    /// <param name="flags"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"><paramref name="flags"/> was <see langword="null"/>.</exception>
    public static bool HasAnyFlag<TEnum>(TEnum value, IEnumerable<TEnum> flags) where TEnum : struct, Enum
        => EnumRep<TEnum>.HasAnyFlag(value, flags is null ? throw new ArgumentNullException(nameof(flags)) : flags);

    /// <summary>
    /// Returns a subset of the specified flags that are set in <paramref name="value"/>.
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <param name="value"></param>
    /// <param name="flags"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"><paramref name="flags"/> was <see langword="null"/>.</exception>
    public static IEnumerable<TEnum> PresentFlags<TEnum>(TEnum value, params TEnum[] flags)
        where TEnum : struct, Enum
        => EnumRep<TEnum>.PresentFlags(value, ThrowIfArgNull(flags, nameof(flags)));

    /// <summary>
    /// Returns a subset of the specified flags that are set in <paramref name="value"/>.
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <param name="value"></param>
    /// <param name="flags"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"><paramref name="flags"/> was <see langword="null"/>.</exception>
    public static IEnumerable<TEnum> PresentFlags<TEnum>(TEnum value, IEnumerable<TEnum> flags)
        where TEnum : struct, Enum
        => EnumRep<TEnum>.PresentFlags(value, ThrowIfArgNull(flags, nameof(flags)));

    /// <summary>
    /// Determines whether the type definition of <typeparamref name="TEnum"/> is decorated with an instance of
    /// <see cref="FlagsAttribute"/>, and can therefore be treated as a bit set of values of its type.
    /// </summary>
    /// <returns></returns>
    [Obsolete("Will be removed in an upcoming version. Use `Enums<TEnum>.IsFlagSet` instead.")]
    public static bool IsFlagSetType<TEnum>() where TEnum : struct, Enum => EnumRep<TEnum>.HasFlagsAttribute;

    /// <summary>
    /// Determines whether the specified <typeparamref name="TEnum"/> value is an atomic value (i.e. a value that is
    /// not equal to a union of other non-equal elements).
    /// </summary>
    /// <remarks>
    /// If <typeparamref name="TEnum"/> is not decorated with an instance of <see cref="FlagsAttribute"/>, this method
    /// will return whether or not <paramref name="value"/> is defined (as in that case all values are treated
    /// as atomic).
    /// </remarks>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsAtomic<TEnum>(TEnum value) where TEnum : struct, Enum => EnumRep<TEnum>.IsAtomic(value);

    /// <summary>
    /// Gets an array of all atomic values (values that are not equal to unions of other non-equal elements) of
    /// type <typeparamref name="TEnum"/>.
    /// </summary>
    /// <remarks>
    /// If <typeparamref name="TEnum"/> is not decorated with an instance of <see cref="FlagsAttribute"/>, this method
    /// will return all values of the type (as in that case all values are treated as atomic).
    /// </remarks>
    /// <returns></returns>
    [Obsolete("Will be removed in an upcoming version. Use `Enums<TEnum>.AtomicValues` instead.")]
    public static TEnum[] GetAtomicValues<TEnum>() where TEnum : struct, Enum => EnumRep<TEnum>.GetAtomicValues();

    /// <summary>
    /// Gets a collection of the flags contained in an instance of <typeparamref name="TEnum"/>.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">
    /// <typeparamref name="TEnum"/> is not decorated with an instance of <see cref="FlagsAttribute"/>, and therefore
    /// cannot reliably be treated as a bit set of atomic values of its type.
    /// </exception>
    public static IEnumerable<TEnum> GetFlags<TEnum>(TEnum value) where TEnum : struct, Enum
        => EnumRep<TEnum>.GetFlags(value);
    #endregion

    #region Helpers
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static T ThrowIfArgNull<T>([NotNull] T? arg, string argName)
        => arg is null ? throw new ArgumentNullException(argName) : arg;
    #endregion
}

/// <summary>
/// Constants and static functionality relating to the <typeparamref name="TEnum"/> <see langword="enum"/> type.
/// </summary>
/// <typeparam name="TEnum">The <see langword="enum"/> type this class contains functionality for.</typeparam>
public static class Enums<TEnum> where TEnum : struct, Enum
{
    /// <summary>
    /// Gets a value describing the underlying <see langword="enum"/> representation type
    /// of <typeparamref name="TEnum"/>.
    /// </summary>
    public static EnumUnderlyingType UnderlyingType => EnumRep<TEnum>.underlyingType;

    /// <summary>
    /// Gets whether or not <typeparamref name="TEnum"/> is decorated with an instance of <see cref="FlagsAttribute"/>,
    /// and can therefore be treated as a bit set of values of its type.
    /// </summary>
    public static bool IsFlagSet => EnumRep<TEnum>.HasFlagsAttribute;

    /// <summary>
    /// Gets an <see cref="ImmutableArray{T}"/> containing all named, defined values of
    /// type <typeparamref name="TEnum"/>.
    /// </summary>
    public static ImmutableArray<TEnum> Values => EnumRep<TEnum>.Values;

    /// <summary>
    /// Gets an <see cref="ImmutableArray{T}"/> containing all named, defined atomic values of type
    /// <typeparamref name="TEnum"/> (i.e. values that are not equal to unions of other non-equal elements).
    /// </summary>
    /// <remarks>
    /// If <typeparamref name="TEnum"/> is not decorated with an instance of <see cref="FlagsAttribute"/>, this array
    /// will contain all values of the type (as in that case all values are treated as atomic).
    /// <para/>
    /// If <typeparamref name="TEnum"/> <i>is</i> decorated with an instance of <see cref="FlagsAttribute"/>, this
    /// array will never contain <see langword="default"/>, as the default is never considered an atomic set of flags.
    /// </remarks>
    public static ImmutableArray<TEnum> AtomicValues => EnumRep<TEnum>.AtomicValues;
}
