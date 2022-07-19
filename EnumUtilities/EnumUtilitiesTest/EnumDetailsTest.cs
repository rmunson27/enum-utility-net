using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.CoreTest.Utilities;

/// <summary>
/// Tests of the enum details methods of the <see cref="Enums"/> class.
/// </summary>
[TestClass]
public class EnumDetailsTest
{
    /// <summary>
    /// Tests the <see cref="Enums.UnderlyingType{TEnum}"/> method.
    /// </summary>
    [TestMethod]
    public void TestUnderlyingType()
    {
        Assert.AreEqual(EnumUnderlyingType.Byte, Enums.UnderlyingType<ByteEnum>());
        Assert.AreEqual(EnumUnderlyingType.SByte, Enums.UnderlyingType<SByteEnum>());
        Assert.AreEqual(EnumUnderlyingType.Short, Enums.UnderlyingType<ShortEnum>());
        Assert.AreEqual(EnumUnderlyingType.UShort, Enums.UnderlyingType<UShortEnum>());
        Assert.AreEqual(EnumUnderlyingType.Int, Enums.UnderlyingType<IntEnum>());
        Assert.AreEqual(EnumUnderlyingType.UInt, Enums.UnderlyingType<UIntEnum>());
        Assert.AreEqual(EnumUnderlyingType.Long, Enums.UnderlyingType<LongEnum>());
        Assert.AreEqual(EnumUnderlyingType.ULong, Enums.UnderlyingType<ULongEnum>());
    }

    /// <summary>
    /// Tests the <see cref="Enums.IsDefined{TEnum}(TEnum)"/> method when called on a type that is not a bit set.
    /// </summary>
    [TestMethod]
    public void TestIsDefined_NonBitSet()
    {
        foreach (var value in NonBitSets.AllValues)
        {
            Assert.IsTrue(Enums.IsDefined(value), $"Value '{value}' was not defined.");
        }
        Assert.IsFalse(Enums.IsDefined(NonBitSets.Unnamed));
    }

    /// <summary>
    /// Tests the <see cref="Enums.IsDefined{TEnum}(TEnum)"/> method when called on a type that is a bit set.
    /// </summary>
    [TestMethod]
    public void TestIsDefined_BitSet()
    {
        foreach (var value in BitSet_Defaults.AllPossibleCombinations.Keys)
        {
            Assert.IsTrue(Enums.IsDefined(value), $"Value '{value}' was not defined.");
        }

        Assert.IsFalse(Enums.IsDefined(BitSet_Defaults.UnnamedAtomicFlag));
        Assert.IsFalse(Enums.IsDefined(BitSet_Defaults.UnnamedNonAtomic));

        // The default of this type is unnamed, so it should not be treated as defined
        Assert.IsFalse(Enums.IsDefined(default(BitSet_NoDefault)));
    }

    private enum ByteEnum : byte { }
    private enum SByteEnum : sbyte { }
    private enum ShortEnum : short { }
    private enum UShortEnum : ushort { }
    private enum IntEnum : int { }
    private enum UIntEnum : uint { }
    private enum LongEnum : long { }
    private enum ULongEnum : ulong { }
}
