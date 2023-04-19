using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Rem.Core.Utilities;

/// <summary>
/// Internally treats a value of type <typeparamref name="TEnum"/> as a value of the underlying type.
/// </summary>
/// <remarks>
/// Subtypes of this type will use a specified underlying type to perform unsafe operations on
/// <typeparamref name="TEnum"/> values to improve <see langword="enum"/> performance.
/// </remarks>
/// <typeparam name="TEnum"></typeparam>
internal abstract class EnumOperations<TEnum> where TEnum : struct, Enum
{
    /// <summary>
    /// Prevents this class from being extended outside of this assembly.
    /// </summary>
    /// <remarks>
    /// Even if this class is made <see langword="public"/> for some reason, it should never be extended outside of
    /// this assembly.
    /// </remarks>
    private protected EnumOperations() { }

    /// <summary>
    /// Performs the bitwise AND of the operands, using the underlying type to perform the operation.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public abstract TEnum And(TEnum lhs, TEnum rhs);

    /// <summary>
    /// Checks if the supplied <typeparamref name="TEnum"/> value has the supplied flag, using the underlying type
    /// to perform the operation.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="flag"></param>
    /// <returns></returns>
    public abstract bool HasFlag(TEnum value, TEnum flag);

    /// <summary>
    /// Checks if the supplied <typeparamref name="TEnum"/> value has any of the supplied flags, using the underlying
    /// type to perform the operation.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="flags"></param>
    /// <returns></returns>
    public abstract bool HasAnyFlag(TEnum value, IEnumerable<TEnum> flags);

    /// <summary>
    /// Filters the supplied collection of <typeparamref name="TEnum"/> flags through <paramref name="value"/>,
    /// returning a collection of the flags that were present.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="flags"></param>
    /// <returns></returns>
    public abstract IEnumerable<TEnum> PresentFlags(TEnum value, IEnumerable<TEnum> flags);

    /// <summary>
    /// Performs the bitwise OR of the operands, using the underlying type to perform the operation.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public abstract TEnum Or(TEnum lhs, TEnum rhs);

    /// <summary>
    /// Performs the bitwise OR of the listed <typeparamref name="TEnum"/> values, using the underlying type to
    /// perform the operation.
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public abstract TEnum Or(IEnumerable<TEnum> values);

    /// <summary>
    /// Performs the bitwise NOT of the operand, using the underlying type to perform the operation.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public abstract TEnum Not(TEnum value);

    /// <summary>
    /// Performs the bitwise XOR of the operands, using the underlying type to perform the operation.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public abstract TEnum XOr(TEnum lhs, TEnum rhs);

    /// <summary>
    /// Determines whether <paramref name="lhs"/> is greater than <paramref name="rhs"/>, using the underlying type
    /// to perform the operation.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public abstract bool Greater(TEnum lhs, TEnum rhs);

    /// <summary>
    /// Determines whether <paramref name="lhs"/> is equal to <paramref name="rhs"/>, using the underlying type
    /// to perform the operation.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public abstract bool Equal(TEnum lhs, TEnum rhs);

    /// <summary>
    /// Determines whether <paramref name="lhs"/> is less than <paramref name="rhs"/>, using the underlying type
    /// to perform the operation.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public abstract bool Less(TEnum lhs, TEnum rhs);

    /// <summary>
    /// Determines whether <paramref name="lhs"/> is greater than or equal to <paramref name="rhs"/>, using the
    /// underlying type to perform the operation.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public abstract bool GreaterOrEqual(TEnum lhs, TEnum rhs);

    /// <summary>
    /// Determines whether <paramref name="lhs"/> is less than or equal to <paramref name="rhs"/>, using the
    /// underlying type to perform the operation.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public abstract bool LessOrEqual(TEnum lhs, TEnum rhs);

    /// <summary>
    /// Compares the two operands, using the underlying type to perform the operation.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public abstract int CompareTo(TEnum lhs, TEnum rhs);

    /// <summary>
    /// Gets the numeric value of the <typeparamref name="TEnum"/> passed in.
    /// </summary>
    /// <remarks>
    /// This needs to be a <see cref="BigInteger"/> because <typeparamref name="TEnum"/> could be represented
    /// internally by a <see cref="long"/> or a <see cref="ulong"/>, neither of which contain the full value.
    /// </remarks>
    /// <param name="lhs"></param>
    /// <returns></returns>
    public abstract BigInteger GetNumericValue(TEnum value);

    /// <summary>
    /// Gets a <typeparamref name="TEnum"/> from the numeric value passed in.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public abstract TEnum FromNumericValue(BigInteger value);
}

internal sealed class ByteEnumOperations<TEnum> : EnumOperations<TEnum> where TEnum : struct, Enum
{
    public override TEnum And(TEnum lhs, TEnum rhs) => Enum(Byte(lhs) & Byte(rhs));

    public override IEnumerable<TEnum> PresentFlags(TEnum value, IEnumerable<TEnum> flags)
    {
        var valueUnderlying = Byte(value);
        foreach (var flag in flags)
        {
            var flagUnderlying = Byte(flag);
            if ((valueUnderlying & flagUnderlying) == flagUnderlying) yield return flag;
        }
    }

    public override bool HasAnyFlag(TEnum value, IEnumerable<TEnum> flags)
    {
        var valueUnderlying = Byte(value);
        foreach (var flag in flags)
        {
            var flagUnderlying = Byte(flag);
            if ((valueUnderlying & flagUnderlying) == flagUnderlying) return true;
        }
        return false;
    }

    public override bool HasFlag(TEnum value, TEnum flag)
    {
        var byteFlag = Byte(flag);
        return (Byte(value) & byteFlag) == byteFlag;
    }

    public override int CompareTo(TEnum lhs, TEnum rhs) => Byte(lhs).CompareTo(Byte(rhs));

    public override bool Equal(TEnum lhs, TEnum rhs) => Byte(lhs) == Byte(rhs);

    public override bool Greater(TEnum lhs, TEnum rhs) => Byte(lhs) > Byte(rhs);

    public override bool GreaterOrEqual(TEnum lhs, TEnum rhs) => Byte(lhs) >= Byte(rhs);

    public override bool Less(TEnum lhs, TEnum rhs) => Byte(lhs) < Byte(rhs);

    public override bool LessOrEqual(TEnum lhs, TEnum rhs) => Byte(lhs) <= Byte(rhs);

    public override TEnum Not(TEnum value) => Enum(~Byte(value));

    public override TEnum Or(TEnum lhs, TEnum rhs) => Enum(Byte(lhs) | Byte(rhs));

    public override TEnum Or(IEnumerable<TEnum> values)
    {
        byte byteResult = 0;
        foreach (var value in values) byteResult |= Byte(value);
        return Enum(byteResult);
    }

    public override TEnum XOr(TEnum lhs, TEnum rhs) => Enum(Byte(lhs) ^ Byte(rhs));

    public override BigInteger GetNumericValue(TEnum value) => Byte(value);

    public override TEnum FromNumericValue(BigInteger value) => Enum((byte)value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static byte Byte(TEnum value) => Unsafe.As<TEnum, byte>(ref value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static TEnum Enum(int value) => Unsafe.As<int, TEnum>(ref value);
}

internal sealed class SByteEnumOperations<TEnum> : EnumOperations<TEnum> where TEnum : struct, Enum
{
    public override TEnum And(TEnum lhs, TEnum rhs) => Enum(SByte(lhs) & SByte(rhs));

    public override IEnumerable<TEnum> PresentFlags(TEnum value, IEnumerable<TEnum> flags)
    {
        var valueUnderlying = SByte(value);
        foreach (var flag in flags)
        {
            var flagUnderlying = SByte(flag);
            if ((valueUnderlying & flagUnderlying) == flagUnderlying) yield return flag;
        }
    }

    public override bool HasAnyFlag(TEnum value, IEnumerable<TEnum> flags)
    {
        var valueUnderlying = SByte(value);
        foreach (var flag in flags)
        {
            var flagUnderlying = SByte(flag);
            if ((valueUnderlying & flagUnderlying) == flagUnderlying) return true;
        }
        return false;
    }

    public override bool HasFlag(TEnum value, TEnum flag)
    {
        var sbyteFlag = SByte(flag);
        return (SByte(value) & sbyteFlag) == sbyteFlag;
    }

    public override int CompareTo(TEnum lhs, TEnum rhs) => SByte(lhs).CompareTo(SByte(rhs));

    public override bool Equal(TEnum lhs, TEnum rhs) => SByte(lhs) == SByte(rhs);

    public override bool Greater(TEnum lhs, TEnum rhs) => SByte(lhs) > SByte(rhs);

    public override bool GreaterOrEqual(TEnum lhs, TEnum rhs) => SByte(lhs) >= SByte(rhs);

    public override bool Less(TEnum lhs, TEnum rhs) => SByte(lhs) < SByte(rhs);

    public override bool LessOrEqual(TEnum lhs, TEnum rhs) => SByte(lhs) <= SByte(rhs);

    public override TEnum Not(TEnum value) => Enum(~SByte(value));

    public override TEnum Or(TEnum lhs, TEnum rhs) => Enum(SByte(lhs) | SByte(rhs));

    public override TEnum Or(IEnumerable<TEnum> values)
    {
        sbyte sbyteResult = 0;
        foreach (var value in values) sbyteResult |= SByte(value);
        return Enum(sbyteResult);
    }

    public override TEnum XOr(TEnum lhs, TEnum rhs) => Enum(SByte(lhs) ^ SByte(rhs));

    public override BigInteger GetNumericValue(TEnum value) => SByte(value);

    public override TEnum FromNumericValue(BigInteger value) => Enum((sbyte)value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static sbyte SByte(TEnum value) => Unsafe.As<TEnum, sbyte>(ref value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static TEnum Enum(int value) => Unsafe.As<int, TEnum>(ref value);
}

internal sealed class ShortEnumOperations<TEnum> : EnumOperations<TEnum> where TEnum : struct, Enum
{
    public override TEnum And(TEnum lhs, TEnum rhs) => Enum(Short(lhs) & Short(rhs));

    public override IEnumerable<TEnum> PresentFlags(TEnum value, IEnumerable<TEnum> flags)
    {
        var valueUnderlying = Short(value);
        foreach (var flag in flags)
        {
            var flagUnderlying = Short(flag);
            if ((valueUnderlying & flagUnderlying) == flagUnderlying) yield return flag;
        }
    }

    public override bool HasAnyFlag(TEnum value, IEnumerable<TEnum> flags)
    {
        var valueUnderlying = Short(value);
        foreach (var flag in flags)
        {
            var flagUnderlying = Short(flag);
            if ((valueUnderlying & flagUnderlying) == flagUnderlying) return true;
        }
        return false;
    }

    public override bool HasFlag(TEnum value, TEnum flag)
    {
        var shortFlag = Short(flag);
        return (Short(value) & shortFlag) == shortFlag;
    }

    public override int CompareTo(TEnum lhs, TEnum rhs) => Short(lhs).CompareTo(Short(rhs));

    public override bool Equal(TEnum lhs, TEnum rhs) => Short(lhs) == Short(rhs);

    public override bool Greater(TEnum lhs, TEnum rhs) => Short(lhs) > Short(rhs);

    public override bool GreaterOrEqual(TEnum lhs, TEnum rhs) => Short(lhs) >= Short(rhs);

    public override bool Less(TEnum lhs, TEnum rhs) => Short(lhs) < Short(rhs);

    public override bool LessOrEqual(TEnum lhs, TEnum rhs) => Short(lhs) <= Short(rhs);

    public override TEnum Not(TEnum value) => Enum(~Short(value));

    public override TEnum Or(TEnum lhs, TEnum rhs) => Enum(Short(lhs) | Short(rhs));

    public override TEnum Or(IEnumerable<TEnum> values)
    {
        short shortResult = 0;
        foreach (var value in values) shortResult |= Short(value);
        return Enum(shortResult);
    }

    public override TEnum XOr(TEnum lhs, TEnum rhs) => Enum(Short(lhs) ^ Short(rhs));

    public override BigInteger GetNumericValue(TEnum value) => Short(value);

    public override TEnum FromNumericValue(BigInteger value) => Enum((short)value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static short Short(TEnum value) => Unsafe.As<TEnum, short>(ref value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static TEnum Enum(int value) => Unsafe.As<int, TEnum>(ref value);
}

internal sealed class UShortEnumOperations<TEnum> : EnumOperations<TEnum> where TEnum : struct, Enum
{
    public override TEnum And(TEnum lhs, TEnum rhs) => Enum(UShort(lhs) & UShort(rhs));

    public override IEnumerable<TEnum> PresentFlags(TEnum value, IEnumerable<TEnum> flags)
    {
        var valueUnderlying = UShort(value);
        foreach (var flag in flags)
        {
            var flagUnderlying = UShort(flag);
            if ((valueUnderlying & flagUnderlying) == flagUnderlying) yield return flag;
        }
    }

    public override bool HasAnyFlag(TEnum value, IEnumerable<TEnum> flags)
    {
        var valueUnderlying = UShort(value);
        foreach (var flag in flags)
        {
            var flagUnderlying = UShort(flag);
            if ((valueUnderlying & flagUnderlying) == flagUnderlying) return true;
        }
        return false;
    }

    public override bool HasFlag(TEnum value, TEnum flag)
    {
        var ushortFlag = UShort(flag);
        return (UShort(value) & ushortFlag) == ushortFlag;
    }

    public override int CompareTo(TEnum lhs, TEnum rhs) => UShort(lhs).CompareTo(UShort(rhs));

    public override bool Equal(TEnum lhs, TEnum rhs) => UShort(lhs) == UShort(rhs);

    public override bool Greater(TEnum lhs, TEnum rhs) => UShort(lhs) > UShort(rhs);

    public override bool GreaterOrEqual(TEnum lhs, TEnum rhs) => UShort(lhs) >= UShort(rhs);

    public override bool Less(TEnum lhs, TEnum rhs) => UShort(lhs) < UShort(rhs);

    public override bool LessOrEqual(TEnum lhs, TEnum rhs) => UShort(lhs) <= UShort(rhs);

    public override TEnum Not(TEnum value) => Enum(~UShort(value));

    public override TEnum Or(TEnum lhs, TEnum rhs) => Enum(UShort(lhs) | UShort(rhs));

    public override TEnum Or(IEnumerable<TEnum> values)
    {
        ushort ushortResult = 0;
        foreach (var value in values) ushortResult |= UShort(value);
        return Enum(ushortResult);
    }

    public override TEnum XOr(TEnum lhs, TEnum rhs) => Enum(UShort(lhs) ^ UShort(rhs));

    public override BigInteger GetNumericValue(TEnum value) => UShort(value);

    public override TEnum FromNumericValue(BigInteger value) => Enum((ushort)value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ushort UShort(TEnum value) => Unsafe.As<TEnum, ushort>(ref value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static TEnum Enum(int value) => Unsafe.As<int, TEnum>(ref value);
}

internal sealed class IntEnumOperations<TEnum> : EnumOperations<TEnum> where TEnum : struct, Enum
{
    public override TEnum And(TEnum lhs, TEnum rhs) => Enum(Int(lhs) & Int(rhs));

    public override IEnumerable<TEnum> PresentFlags(TEnum value, IEnumerable<TEnum> flags)
    {
        var valueUnderlying = Int(value);
        foreach (var flag in flags)
        {
            var flagUnderlying = Int(flag);
            if ((valueUnderlying & flagUnderlying) == flagUnderlying) yield return flag;
        }
    }

    public override bool HasAnyFlag(TEnum value, IEnumerable<TEnum> flags)
    {
        var valueUnderlying = Int(value);
        foreach (var flag in flags)
        {
            var flagUnderlying = Int(flag);
            if ((valueUnderlying & flagUnderlying) == flagUnderlying) return true;
        }
        return false;
    }

    public override bool HasFlag(TEnum value, TEnum flag)
    {
        var intFlag = Int(flag);
        return (Int(value) & intFlag) == intFlag;
    }

    public override int CompareTo(TEnum lhs, TEnum rhs) => Int(lhs).CompareTo(Int(rhs));

    public override bool Equal(TEnum lhs, TEnum rhs) => Int(lhs) == Int(rhs);

    public override bool Greater(TEnum lhs, TEnum rhs) => Int(lhs) > Int(rhs);

    public override bool GreaterOrEqual(TEnum lhs, TEnum rhs) => Int(lhs) >= Int(rhs);

    public override bool Less(TEnum lhs, TEnum rhs) => Int(lhs) < Int(rhs);

    public override bool LessOrEqual(TEnum lhs, TEnum rhs) => Int(lhs) <= Int(rhs);

    public override TEnum Not(TEnum value) => Enum(~Int(value));

    public override TEnum Or(TEnum lhs, TEnum rhs) => Enum(Int(lhs) | Int(rhs));

    public override TEnum Or(IEnumerable<TEnum> values)
    {
        int intResult = 0;
        foreach (var value in values) intResult |= Int(value);
        return Enum(intResult);
    }

    public override TEnum XOr(TEnum lhs, TEnum rhs) => Enum(Int(lhs) ^ Int(rhs));

    public override BigInteger GetNumericValue(TEnum value) => Int(value);

    public override TEnum FromNumericValue(BigInteger value) => Enum((int)value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int Int(TEnum value) => Unsafe.As<TEnum, int>(ref value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static TEnum Enum(int value) => Unsafe.As<int, TEnum>(ref value);
}

internal sealed class UIntEnumOperations<TEnum> : EnumOperations<TEnum> where TEnum : struct, Enum
{
    public override TEnum And(TEnum lhs, TEnum rhs) => Enum(UInt(lhs) & UInt(rhs));

    public override IEnumerable<TEnum> PresentFlags(TEnum value, IEnumerable<TEnum> flags)
    {
        var valueUnderlying = UInt(value);
        foreach (var flag in flags)
        {
            var flagUnderlying = UInt(flag);
            if ((valueUnderlying & flagUnderlying) == flagUnderlying) yield return flag;
        }
    }

    public override bool HasAnyFlag(TEnum value, IEnumerable<TEnum> flags)
    {
        var valueUnderlying = UInt(value);
        foreach (var flag in flags)
        {
            var flagUnderlying = UInt(flag);
            if ((valueUnderlying & flagUnderlying) == flagUnderlying) return true;
        }
        return false;
    }

    public override bool HasFlag(TEnum value, TEnum flag)
    {
        var uintFlag = UInt(flag);
        return (UInt(value) & uintFlag) == uintFlag;
    }

    public override int CompareTo(TEnum lhs, TEnum rhs) => UInt(lhs).CompareTo(UInt(rhs));

    public override bool Equal(TEnum lhs, TEnum rhs) => UInt(lhs) == UInt(rhs);

    public override bool Greater(TEnum lhs, TEnum rhs) => UInt(lhs) > UInt(rhs);

    public override bool GreaterOrEqual(TEnum lhs, TEnum rhs) => UInt(lhs) >= UInt(rhs);

    public override bool Less(TEnum lhs, TEnum rhs) => UInt(lhs) < UInt(rhs);

    public override bool LessOrEqual(TEnum lhs, TEnum rhs) => UInt(lhs) <= UInt(rhs);

    public override TEnum Not(TEnum value) => Enum(~UInt(value));

    public override TEnum Or(TEnum lhs, TEnum rhs) => Enum(UInt(lhs) | UInt(rhs));

    public override TEnum Or(IEnumerable<TEnum> values)
    {
        uint uintResult = 0;
        foreach (var value in values) uintResult |= UInt(value);
        return Enum(uintResult);
    }

    public override TEnum XOr(TEnum lhs, TEnum rhs) => Enum(UInt(lhs) ^ UInt(rhs));

    public override BigInteger GetNumericValue(TEnum value) => UInt(value);

    public override TEnum FromNumericValue(BigInteger value) => Enum((uint)value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint UInt(TEnum value) => Unsafe.As<TEnum, uint>(ref value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static TEnum Enum(uint value) => Unsafe.As<uint, TEnum>(ref value);
}

internal sealed class LongEnumOperations<TEnum> : EnumOperations<TEnum> where TEnum : struct, Enum
{
    public override TEnum And(TEnum lhs, TEnum rhs) => Enum(Long(lhs) & Long(rhs));

    public override IEnumerable<TEnum> PresentFlags(TEnum value, IEnumerable<TEnum> flags)
    {
        var valueUnderlying = Long(value);
        foreach (var flag in flags)
        {
            var flagUnderlying = Long(flag);
            if ((valueUnderlying & flagUnderlying) == flagUnderlying) yield return flag;
        }
    }

    public override bool HasAnyFlag(TEnum value, IEnumerable<TEnum> flags)
    {
        var valueUnderlying = Long(value);
        foreach (var flag in flags)
        {
            var flagUnderlying = Long(flag);
            if ((valueUnderlying & flagUnderlying) == flagUnderlying) return true;
        }
        return false;
    }

    public override bool HasFlag(TEnum value, TEnum flag)
    {
        var longFlag = Long(flag);
        return (Long(value) & longFlag) == longFlag;
    }

    public override int CompareTo(TEnum lhs, TEnum rhs) => Long(lhs).CompareTo(Long(rhs));

    public override bool Equal(TEnum lhs, TEnum rhs) => Long(lhs) == Long(rhs);

    public override bool Greater(TEnum lhs, TEnum rhs) => Long(lhs) > Long(rhs);

    public override bool GreaterOrEqual(TEnum lhs, TEnum rhs) => Long(lhs) >= Long(rhs);

    public override bool Less(TEnum lhs, TEnum rhs) => Long(lhs) < Long(rhs);

    public override bool LessOrEqual(TEnum lhs, TEnum rhs) => Long(lhs) <= Long(rhs);

    public override TEnum Not(TEnum value) => Enum(~Long(value));

    public override TEnum Or(TEnum lhs, TEnum rhs) => Enum(Long(lhs) | Long(rhs));

    public override TEnum Or(IEnumerable<TEnum> values)
    {
        long longResult = 0;
        foreach (var value in values) longResult |= Long(value);
        return Enum(longResult);
    }

    public override TEnum XOr(TEnum lhs, TEnum rhs) => Enum(Long(lhs) ^ Long(rhs));

    public override BigInteger GetNumericValue(TEnum value) => Long(value);

    public override TEnum FromNumericValue(BigInteger value) => Enum((long)value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static long Long(TEnum value) => Unsafe.As<TEnum, long>(ref value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static TEnum Enum(long value) => Unsafe.As<long, TEnum>(ref value);
}

internal sealed class ULongEnumOperations<TEnum> : EnumOperations<TEnum> where TEnum : struct, Enum
{
    public override TEnum And(TEnum lhs, TEnum rhs) => Enum(ULong(lhs) & ULong(rhs));

    public override IEnumerable<TEnum> PresentFlags(TEnum value, IEnumerable<TEnum> flags)
    {
        var valueUnderlying = ULong(value);
        foreach (var flag in flags)
        {
            var flagUnderlying = ULong(flag);
            if ((valueUnderlying & flagUnderlying) == flagUnderlying) yield return flag;
        }
    }

    public override bool HasAnyFlag(TEnum value, IEnumerable<TEnum> flags)
    {
        var valueUnderlying = ULong(value);
        foreach (var flag in flags)
        {
            var flagUnderlying = ULong(flag);
            if ((valueUnderlying & flagUnderlying) == flagUnderlying) return true;
        }
        return false;
    }

    public override bool HasFlag(TEnum value, TEnum flag)
    {
        var ulongFlag = ULong(flag);
        return (ULong(value) & ulongFlag) == ulongFlag;
    }

    public override int CompareTo(TEnum lhs, TEnum rhs) => ULong(lhs).CompareTo(ULong(rhs));

    public override bool Equal(TEnum lhs, TEnum rhs) => ULong(lhs) == ULong(rhs);

    public override bool Greater(TEnum lhs, TEnum rhs) => ULong(lhs) > ULong(rhs);

    public override bool GreaterOrEqual(TEnum lhs, TEnum rhs) => ULong(lhs) >= ULong(rhs);

    public override bool Less(TEnum lhs, TEnum rhs) => ULong(lhs) < ULong(rhs);

    public override bool LessOrEqual(TEnum lhs, TEnum rhs) => ULong(lhs) <= ULong(rhs);

    public override TEnum Not(TEnum value) => Enum(~ULong(value));

    public override TEnum Or(TEnum lhs, TEnum rhs) => Enum(ULong(lhs) | ULong(rhs));

    public override TEnum Or(IEnumerable<TEnum> values)
    {
        ulong ulongResult = 0;
        foreach (var value in values) ulongResult |= ULong(value);
        return Enum(ulongResult);
    }

    public override TEnum XOr(TEnum lhs, TEnum rhs) => Enum(ULong(lhs) ^ ULong(rhs));

    public override BigInteger GetNumericValue(TEnum value) => ULong(value);

    public override TEnum FromNumericValue(BigInteger value) => Enum((ulong)value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong ULong(TEnum value) => Unsafe.As<TEnum, ulong>(ref value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static TEnum Enum(ulong value) => Unsafe.As<ulong, TEnum>(ref value);
}
