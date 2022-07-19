using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.CoreTest.Utilities;

/// <summary>
/// Tests the comparison methods of the <see cref="Enums"/> class.
/// </summary>
[TestClass]
public class ComparisonTest
{
    /// <summary>
    /// Tests all comparison methods.
    /// </summary>
    [TestMethod]
    public void TestComparisons()
    {
        foreach (var value1 in NonBitSets.AllValues)
        {
            foreach (var value2 in NonBitSets.AllValues)
            {
                Assert.AreEqual(value1 < value2, Enums.Less(value1, value2));
                Assert.AreEqual(value1 == value2, Enums.Equal(value1, value2));
                Assert.AreEqual(value1 > value2, Enums.Greater(value1, value2));
                Assert.AreEqual(value1 <= value2, Enums.LessOrEqual(value1, value2));
                Assert.AreEqual(value1 >= value2, Enums.GreaterOrEqual(value1, value2));
                Assert.AreEqual(value1.CompareTo(value2), Enums.CompareTo(value1, value2));
            }
        }
    }
}
