using System;
using System.Collections.Immutable;

namespace EnumUtilitiesProfiler;

using static MusicalNoteLetter;

internal static class EnumValues
{
    public static Bit[] AllBits { get; } = (Bit[])Enum.GetValues(typeof(Bit));

    public static NumberType[] AllNumberTypes { get; } = (NumberType[])Enum.GetValues(typeof(NumberType));

    public static Letter[] AllLetters { get; } = (Letter[])Enum.GetValues(typeof(Letter));

    public static MusicalNoteLetter[] AllMusicalNoteLetters = new[] { C, D, E, F, G, A, B };

    /// <summary>
    /// We know that the defined <see cref="Letter"/> values form an all-inclusive integer range (default
    /// <see langword="enum"/> value constant assignment, increasing from <see cref="Letter.A"/> to
    /// <see cref="Letter.Z"/>), so the <c>IsDefined</c> operation can be optimized by simply checking if a given
    /// <see cref="Letter"/> is in the range.
    /// </summary>
    /// <param name="l"></param>
    /// <returns></returns>
    public static bool IsDefinedLetter(this Letter l) => l >= Letter.A && l <= Letter.Z;

    /// <summary>
    /// The defined <see cref="MusicalNoteLetter"/> instances do not form an all-inclusive integer range, so the
    /// IsDefined operation cannot be optimized as in <see cref="IsDefinedLetter(Letter)"/>, but we can still optimize
    /// with a <see langword="switch"/>.
    /// </summary>
    /// <param name="l"></param>
    /// <returns></returns>
    public static bool IsDefinedMusicalNoteLetter(this MusicalNoteLetter l) => l is C or D or E or F or G or A or B;
}
