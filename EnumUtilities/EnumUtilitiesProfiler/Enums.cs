using System;

namespace EnumUtilitiesProfiler;

/**
 * Some test enums for the benchmarks.
 */

[Flags]
internal enum Bit : byte
{
    None = 0,
    One = 1,
    Two = 2,
    Four = 4,
    Eight = 8,
    Sixteen = 16,
    ThirtyTwo = 32,
    SixtyFour = 64,
    OneTwentyEight = 128,
}

internal enum Letter : int
{
    A,      B,      C,      D,      E,      F,      G,
    H,      I,      J,      K,      L,  M,  N,  O,  P,
    Q,      R,      S,              T,      U,      V,
    W,              X,              Y,   /* and */  Z
}

internal enum MusicalNoteLetter : int
{
    // Map notes to half-steps above C
    C = 0, D = 2, E = 4, F = 5, G = 7, A = 9, B = 11
}

// Oh why the hell not here's the song (only the letters themselves have notes attached, spaces and 'and' are skipped)
file static class AlphabetSongMapping
{
    const MusicalNoteLetter C = MusicalNoteLetter.C, D = MusicalNoteLetter.D, E = MusicalNoteLetter.E,
                            F = MusicalNoteLetter.F, G = MusicalNoteLetter.G, A = MusicalNoteLetter.A,
                            B = MusicalNoteLetter.B; // To make typing less of a hassle

    public static readonly IEnumerable<(Letter Letter, MusicalNoteLetter Note)> Pairs
        = Enums.GetValues<Letter>()
               .Zip(new[]
                    {
                        C,    C,    G,    G,    A,    A,    G,
                        F,    F,    E,    E,    D, D, D, D, C,
                        G,    G,    F,          E,    E,    D,
                        G,          F,          E,          D,
                    });
}

[Flags]
internal enum NumberType : long
{
    Empty = 0,

    NonEmpty = 1,
    NonZero = NonEmpty | 2,

    Positive = NonZero | 4,
    Negative = NonZero | 8,

    Fractional = NonZero | 16,
    Irrational = Fractional | 32,
    Transcendental = Irrational | 64,

    Prime = NonZero | 128,
    Composite = NonZero | 256,

    Infinity = NonZero | (1 << 31), // On high
}
