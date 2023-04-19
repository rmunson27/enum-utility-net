using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.CoreTest.Utilities;

internal static class BitSet_Defaults
{
    public static readonly ImmutableArray<BitSet_Default> AllAtomicValues = ImmutableArray.CreateRange(new[]
    {
        BitSet_Default.One,
        BitSet_Default.Two,
        BitSet_Default.Four,
    });

    /// <summary>
    /// A mapping of the defined values of the <see cref="BitSet_Default"/> type to the atomic values they contain.
    /// </summary>
    public static readonly ImmutableDictionary<BitSet_Default, ImmutableHashSet<BitSet_Default>> AllPossibleCombinations
        = ImmutableDictionary.CreateRange(new KeyValuePair<BitSet_Default, ImmutableHashSet<BitSet_Default>>[]
        {
            new(BitSet_Default.Zero, ImmutableHashSet<BitSet_Default>.Empty),

            new(BitSet_Default.One, ImmutableHashSet.Create(BitSet_Default.One)),
            new(BitSet_Default.Two, ImmutableHashSet.Create(BitSet_Default.Two)),
            new(BitSet_Default.Four, ImmutableHashSet.Create(BitSet_Default.Four)),

            new(
                BitSet_Default.One | BitSet_Default.Two,
                ImmutableHashSet.CreateRange(new[] { BitSet_Default.One, BitSet_Default.Two })),
            new(
                BitSet_Default.One | BitSet_Default.Four,
                ImmutableHashSet.CreateRange(new[] { BitSet_Default.One, BitSet_Default.Four })),
            new(
                BitSet_Default.Two | BitSet_Default.Four,
                ImmutableHashSet.CreateRange(new[] { BitSet_Default.Two, BitSet_Default.Four })),

            new(BitSet_Default.One | BitSet_Default.Two | BitSet_Default.Four, AllAtomicValues.ToImmutableHashSet()),
        });

    public const BitSet_Default UnnamedAtomicFlag = (BitSet_Default)8;
    public const BitSet_Default UnnamedNonAtomic = UnnamedAtomicFlag | BitSet_Default.Two;
}

[Flags]
internal enum BitSet_NoDefault : byte
{
    One = 1,
    Two = 2,
    Four = 4,

    Three = One | Two,
}

[Flags]
internal enum BitSet_Default : byte
{
    Zero = 0,
    One = 1,
    Two = 2,
    Four = 4,
}

internal static class BitSet_Complexes
{
    public static readonly ImmutableArray<BitSet_Complex> AllAtomicValues = new[] { 1, 2, 6, 2 }
                                                                                .Select(n => (BitSet_Complex)n)
                                                                                .ToImmutableArray();
}

[Flags]
[SuppressMessage("Design", "CA1069:Enums values should not be duplicated", Justification = "For testing.")]
internal enum BitSet_Complex : byte
{
    Zero = 0,
    One = 1,
    Two = 2,

    Three = One | Two, // Not atomic

    Six = Two | 4, // 4 is not equal to a value of this type, so this should be treated as atomic

    TwoAgain = 2, // This should be treated as atomic even though it's equal to 2
}

internal static class NonBitSets
{
    public static readonly ImmutableArray<NonBitSet> AllValues = Enum.GetValues<NonBitSet>().ToImmutableArray();

    public const NonBitSet Unnamed = (NonBitSet)4;
}

internal enum NonBitSet : short
{
    Zero = 0,
    One = 1,
    Two = 2,
    Three = 3,
}
