using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace EnumUtilitiesProfiler;

/// <summary>
/// Tests the <see cref="Enums"/> operations (and, or, xor, etc.).
/// </summary>
[SimpleJob(RuntimeMoniker.NetCoreApp31, baseline: true)]
[SimpleJob(RuntimeMoniker.Net50)]
[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[SuppressMessage("Performance", "CA1822:Mark members as static",
                 Justification = "BenchmarkDotNet needs public instance methods.")]
public class IsDefinedBenchmark
{
    [Params(1000, 10000, 100000)]
    public int N;

#if NET5_0_OR_GREATER
    [Benchmark]
    public void Generic_NET_ByteEnum()
    {
        Generic_NET(AllBits);
    }

    [Benchmark]
    public void Generic_NET_ContinuousRangeIntEnum()
    {
        Generic_NET(AllLetters);
    }

    [Benchmark]
    public void Generic_NET_NonContinuousRangeIntEnum()
    {
        Generic_NET(AllMusicalNoteLetters);
    }

    [Benchmark]
    public void Generic_NET_LongEnum()
    {
        Generic_NET(AllNumberTypes);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Generic_NET<TEnum>(TEnum[] allValues) where TEnum : struct, Enum
    {
        foreach (var value in allValues) Enum.IsDefined<TEnum>(value);
    }
#endif

    [Benchmark]
    public void NonGeneric_NET_ByteEnum()
    {
        NonGeneric_NET(AllBits);
    }

    [Benchmark]
    public void NonGeneric_NET_ContinuousRangeIntEnum()
    {
        NonGeneric_NET(AllLetters);
    }

    [Benchmark]
    public void NonGeneric_NET_NonContinuousRangeIntEnum()
    {
        NonGeneric_NET(AllMusicalNoteLetters);
    }

    [Benchmark]
    public void NonGeneric_NET_LongEnum()
    {
        NonGeneric_NET(AllNumberTypes);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void NonGeneric_NET<TEnum>(TEnum[] allValues) where TEnum : struct, Enum
    {
        foreach (var value in allValues) Enum.IsDefined(typeof(TEnum), value);
    }

    [Benchmark]
    public void ContinuousRangeOptimized()
    {
        foreach (var letter in AllLetters) letter.IsDefinedLetter();
    }

    [Benchmark]
    public void SwitchOptimized()
    {
        foreach (var note in AllMusicalNoteLetters) note.IsDefinedMusicalNoteLetter();
    }

    [Benchmark]
    public void Library_ByteEnum()
    {
        Library(AllBits);
    }

    [Benchmark]
    public void Library_ContinuousRangeIntEnum()
    {
        Library(AllLetters);
    }

    [Benchmark]
    public void Library_NonContinuousRangeIntEnum()
    {
        Library(AllMusicalNoteLetters);
    }

    [Benchmark]
    public void Library_LongEnum()
    {
        Library(AllNumberTypes);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Library<TEnum>(TEnum[] allValues) where TEnum : struct, Enum
    {
        foreach (var value in allValues) Enums.IsDefined(value);
    }
}
