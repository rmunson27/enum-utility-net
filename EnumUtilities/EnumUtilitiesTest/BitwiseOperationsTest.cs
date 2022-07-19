namespace Rem.CoreTest.Utilities;

/// <summary>
/// Tests of the <see cref="Enums"/> class bitwise operations.
/// </summary>
[TestClass]
public class BitwiseOperationsTest
{
    /// <summary>
    /// Tests the <see cref="Enums.And{TEnum}(TEnum, TEnum)"/> method.
    /// </summary>
    [TestMethod]
    public void TestAnd()
    {
        foreach (var value1 in BitSet_Defaults.AllPossibleCombinations)
        {
            foreach (var value2 in BitSet_Defaults.AllPossibleCombinations)
            {
                Assert.AreEqual(value1 & value2, Enums.And(value1, value2));
            }
        }
    }

    /// <summary>
    /// Tests the <see cref="Enums.Or{TEnum}(TEnum, TEnum)"/> method.
    /// </summary>
    [TestMethod]
    public void TestOr()
    {
        foreach (var value1 in BitSet_Defaults.AllPossibleCombinations)
        {
            foreach (var value2 in BitSet_Defaults.AllPossibleCombinations)
            {
                Assert.AreEqual(value1 | value2, Enums.Or(value1, value2));
            }
        }
    }

    /// <summary>
    /// Tests the <see cref="Enums.Or{TEnum}(TEnum[])"/> method.
    /// </summary>
    /// <remarks>
    /// The <see cref="Enums.Or{TEnum}(IEnumerable{TEnum})"/> method uses the same underlying code.
    /// </remarks>
    [TestMethod]
    public void TestOrRange()
    {
        foreach (var value1 in BitSet_Defaults.AllPossibleCombinations)
        {
            foreach (var value2 in BitSet_Defaults.AllPossibleCombinations)
            {
                Assert.AreEqual(value1 | value2, Enums.Or(new[] { value1, value2 }));
            }
        }

        Assert.AreEqual(
            BitSet_Default.One | BitSet_Default.Two | BitSet_Default.Four,
            Enums.Or(new[] { BitSet_Default.One, BitSet_Default.Two, BitSet_Default.Four }));
    }

    /// <summary>
    /// Tests the <see cref="Enums.XOr{TEnum}(TEnum, TEnum)"/> method.
    /// </summary>
    [TestMethod]
    public void TestXOr()
    {
        foreach (var value1 in BitSet_Defaults.AllPossibleCombinations)
        {
            foreach (var value2 in BitSet_Defaults.AllPossibleCombinations)
            {
                Assert.AreEqual(value1 ^ value2, Enums.XOr(value1, value2));
            }
        }
    }

    /// <summary>
    /// Tests the <see cref="Enums.Not{TEnum}(TEnum)"/> method.
    /// </summary>
    [TestMethod]
    public void TestNot()
    {
        foreach (var value in BitSet_Defaults.AllPossibleCombinations)
        {
            Assert.AreEqual(~value, Enums.Not(value));
        }
    }
}
