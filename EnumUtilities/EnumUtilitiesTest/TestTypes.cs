using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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

    public static readonly ImmutableArray<BitSet_Default> AllValues = ImmutableArray.CreateRange(new[]
    {
        BitSet_Default.Zero,

        BitSet_Default.One,
        BitSet_Default.Two,
        BitSet_Default.Four,

        BitSet_Default.One | BitSet_Default.Two,
        BitSet_Default.One | BitSet_Default.Four,
        BitSet_Default.Two | BitSet_Default.Four,

        BitSet_Default.One | BitSet_Default.Two | BitSet_Default.Four,
    });
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

internal enum NonBitSet : short
{
    Zero = 0,
    One = 1,
    Two = 2,
    Three = 3,
}
