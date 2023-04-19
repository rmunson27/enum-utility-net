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
#pragma warning disable CS0618 // Still needs to be tested
        Assert.AreEqual(EnumUnderlyingType.Byte, Enums.UnderlyingType<ByteEnum>());
        Assert.AreEqual(EnumUnderlyingType.Byte, Enums<ByteEnum>.UnderlyingType);
        Assert.AreEqual(EnumUnderlyingType.SByte, Enums.UnderlyingType<SByteEnum>());
        Assert.AreEqual(EnumUnderlyingType.SByte, Enums<SByteEnum>.UnderlyingType);
        Assert.AreEqual(EnumUnderlyingType.Short, Enums.UnderlyingType<ShortEnum>());
        Assert.AreEqual(EnumUnderlyingType.Short, Enums<ShortEnum>.UnderlyingType);
        Assert.AreEqual(EnumUnderlyingType.UShort, Enums.UnderlyingType<UShortEnum>());
        Assert.AreEqual(EnumUnderlyingType.UShort, Enums<UShortEnum>.UnderlyingType);
        Assert.AreEqual(EnumUnderlyingType.Int, Enums.UnderlyingType<IntEnum>());
        Assert.AreEqual(EnumUnderlyingType.Int, Enums<IntEnum>.UnderlyingType);
        Assert.AreEqual(EnumUnderlyingType.UInt, Enums.UnderlyingType<UIntEnum>());
        Assert.AreEqual(EnumUnderlyingType.UInt, Enums<UIntEnum>.UnderlyingType);
        Assert.AreEqual(EnumUnderlyingType.Long, Enums.UnderlyingType<LongEnum>());
        Assert.AreEqual(EnumUnderlyingType.Long, Enums<LongEnum>.UnderlyingType);
        Assert.AreEqual(EnumUnderlyingType.ULong, Enums.UnderlyingType<ULongEnum>());
        Assert.AreEqual(EnumUnderlyingType.ULong, Enums<ULongEnum>.UnderlyingType);
#pragma warning restore CS0618
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

    /// <summary>
    /// Tests the value-getting methods of the <see cref="Enums"/> and <see cref="Enums{TEnum}"/> classes.
    /// </summary>
    [TestMethod]
    public void TestValues()
    {
        static void TestOfType<TEnum>() where TEnum : struct, Enum
        {
#pragma warning disable CS0618 // Still needs to be tested
            Assert.That.SequenceEqual(Enum.GetValues<TEnum>(), Enums.GetValues<TEnum>());
#pragma warning restore CS0618
            Assert.That.SequenceEqual(Enum.GetValues<TEnum>(), Enums<TEnum>.Values);
        }

        TestOfType<ByteEnum>();
        TestOfType<SByteEnum>();
        TestOfType<ShortEnum>();
        TestOfType<UShortEnum>();
        TestOfType<IntEnum>();
        TestOfType<UIntEnum>();
        TestOfType<LongEnum>();
        TestOfType<ULongEnum>();

        TestOfType<BitSet_Default>();
        TestOfType<BitSet_NoDefault>();
        TestOfType<NonBitSet>();
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

file static class AssertExtensions
{
    /// <summary>
    /// Asserts that the two sequences are equal.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="expected"></param>
    /// <param name="actual"></param>
    /// <exception cref="AssertFailedException">The assertion failed.</exception>
    public static void SequenceEqual<T>(this Assert _, IEnumerable<T> expected, IEnumerable<T> actual)
    {
        using IEnumerator<T> expectedEnumerator = expected.GetEnumerator(),
                             actualEnumerator = actual.GetEnumerator();

        bool expectedHasNext = expectedEnumerator.MoveNext(),
             actualHasNext = actualEnumerator.MoveNext();

        long i = 0;
        while (expectedHasNext && actualHasNext)
        {
            Assert.AreEqual(expectedEnumerator.Current, actualEnumerator.Current,
                            $"Elements at index {i} were not equal.");
            i++;
            expectedHasNext = expectedEnumerator.MoveNext();
            actualHasNext = actualEnumerator.MoveNext();
        }

        if (expectedHasNext != actualHasNext)
        {
            // Capitalize `shorter` because it will start sentence
            var (shorter, longer) = expectedHasNext ? ("Actual", "expected") : ("Expected", "actual");

            Assert.Fail($"{shorter} sequence ended after {i} elements, but {longer} sequence is longer.");
        }
    }
}
