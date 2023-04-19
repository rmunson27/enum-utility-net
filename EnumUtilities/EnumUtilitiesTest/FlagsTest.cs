using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.CoreTest.Utilities;

/// <summary>
/// Tests of the flag-related methods of the <see cref="Enums"/> class.
/// </summary>
[TestClass]
public class FlagsTest
{
    private readonly Random Random = new();

    /// <summary>
    /// Tests the <see cref="Enums.GetAtomicValues{TEnum}"/> method.
    /// </summary>
    [TestMethod]
    public void TestGetAtomicValues()
    {
        // Values should just be the atomic values when dealing with a bit set
#pragma warning disable CS0618 // Still need to be tested
        Assert.That.AreSetEqual(BitSet_Defaults.AllAtomicValues, Enums.GetAtomicValues<BitSet_Default>());
        Assert.That.AreSetEqual(BitSet_Defaults.AllAtomicValues, Enums<BitSet_Default>.AtomicValues);

        // Otherwise values should be everything defined
        Assert.That.AreSetEqual(NonBitSets.AllValues, Enums.GetAtomicValues<NonBitSet>());
        Assert.That.AreSetEqual(NonBitSets.AllValues, Enums<NonBitSet>.AtomicValues);

        // This is complex - there is a defined value that has a non-strict subflag that is a defined value and another
        // non-strict subflag that is NOT a defined value
        // The value in question should be treated as atomic
        Assert.That.AreSetEqual(BitSet_Complexes.AllAtomicValues, Enums.GetAtomicValues<BitSet_Complex>());
        Assert.That.AreSetEqual(BitSet_Complexes.AllAtomicValues, Enums<BitSet_Complex>.AtomicValues);
#pragma warning restore CS0618
    }

    /// <summary>
    /// Tests the <see cref="Enums.IsAtomic{TEnum}(TEnum)"/> method.
    /// </summary>
    [TestMethod]
    public void TestIsAtomic()
    {
        // Atomic values of bit sets are determined based on defined bits set
        foreach (var atom in BitSet_Defaults.AllAtomicValues)
        {
            Assert.IsTrue(Enums.IsAtomic(atom), $"Value '{atom}' was not atomic as expected.");
        }
        foreach (var nonAtom in BitSet_Defaults.AllPossibleCombinations.Keys.Except(BitSet_Defaults.AllAtomicValues))
        {
            Assert.IsFalse(Enums.IsAtomic(nonAtom), $"Value '{nonAtom}' was atomic unexpectedly.");
        }
        Assert.IsFalse(Enums.IsAtomic(BitSet_Defaults.UnnamedAtomicFlag));
        Assert.IsFalse(Enums.IsAtomic(BitSet_Defaults.UnnamedNonAtomic));

        // Atomic values of non-bit sets are every defined value
        foreach (var value in NonBitSets.AllValues)
        {
            Assert.IsTrue(Enums.IsAtomic(value), $"Value '{value}' was not atomic as expected.");
        }
        Assert.IsFalse(Enums.IsAtomic(NonBitSets.Unnamed));
    }

    /// <summary>
    /// Tests the <see cref="Enums.HasFlag{TEnum}(TEnum, TEnum)"/> and
    /// <see cref="Enums.HasAnyFlag{TEnum}(TEnum, IEnumerable{TEnum})"/> methods.
    /// </summary>
    [TestMethod]
    public void TestHasFlag()
    {
        foreach (var (combo, atoms) in BitSet_Defaults.AllPossibleCombinations)
        {
            foreach (var atom in atoms)
            {
                Assert.IsTrue(Enums.HasFlag(combo, atom));
            }

            Assert.IsTrue(combo.HasFlag(combo));
            Assert.IsTrue(combo.HasFlag(default(BitSet_Default)));

            var notIncludedAtoms = BitSet_Defaults.AllAtomicValues.Except(atoms);
            foreach (var notIncludedAtom in notIncludedAtoms)
            {
                Assert.IsFalse(Enums.HasFlag(combo, notIncludedAtom));
            }

            Assert.IsFalse(Enums.HasAnyFlag(combo, notIncludedAtoms));
            if (atoms.Count > 0)
            {
                var randomAtom = atoms.ElementAt(Random.Next(atoms.Count));
                Assert.IsTrue(Enums.HasAnyFlag(combo, notIncludedAtoms.Append(randomAtom)));
            }
        }

        foreach (var v in BitSet_Defaults.AllAtomicValues)
        {
            Assert.IsTrue(Enums.HasFlag(v, default));
        }
    }

    /// <summary>
    /// Tests the <see cref="Enums.PresentFlags{TEnum}(TEnum, IEnumerable{TEnum})"/> method.
    /// </summary>
    [TestMethod]
    public void TestPresentFlags()
    {
        foreach (var (combo, atoms) in BitSet_Defaults.AllPossibleCombinations)
        {
            // Make sure the method picks out the specific flags that are present
            Assert.IsTrue(Enums.PresentFlags(combo, BitSet_Defaults.AllAtomicValues).ToHashSet().SetEquals(atoms));
        }
    }

    /// <summary>
    /// Tests the <see cref="Enums.IsFlagSetType{TEnum}"/> method.
    /// </summary>
    [TestMethod]
    public void TestIsFlagSetType()
    {
        Assert.IsTrue(Enums.IsFlagSetType<BitSet_Default>());
        Assert.IsTrue(Enums.IsFlagSetType<BitSet_NoDefault>());
        Assert.IsTrue(Enums.IsFlagSetType<BitSet_Complex>());
        Assert.IsFalse(Enums.IsFlagSetType<NonBitSet>());
    }

    /// <summary>
    /// Tests the <see cref="Enums.GetFlags{TEnum}(TEnum)"/> method.
    /// </summary>
    [TestMethod]
    public void TestGetFlags()
    {
        foreach (var (combo, atoms) in BitSet_Defaults.AllPossibleCombinations)
        {
            Assert.IsTrue(Enums.GetFlags(combo).ToHashSet().SetEquals(atoms));
        }

        // Can't do this with a non-bit set
        Assert.ThrowsException<InvalidOperationException>(() => Enums.GetFlags(NonBitSet.Three));
    }
}

file static class AssertExtensions
{
    public static void AreSetEqual<T>(this Assert _, IEnumerable<T> expected, IEnumerable<T> actual)
    {
        try
        {
            HashSet<T> expectedValues = new(expected), actualValues = new(actual);

            Assert.AreEqual(expectedValues.Count, actualValues.Count,
                            $"Numbers of unique values of expected sequence ({expectedValues.Count}) "
                                + $"and actual sequence ({actualValues.Count}) differ.");

            long i = 0;
            foreach (var e in expected)
            {
                Assert.IsTrue(actualValues.Contains(e),
                              $"Actual sequence does not contain expected value {e} at index {i}.");
                i++;
            }
            i = 0;
            foreach (var a in actual)
            {
                Assert.IsTrue(expectedValues.Contains(a),
                              $"Expected sequence does not contain actual value {a} at index {i}.");
                i++;
            }
        }
        catch (AssertFailedException e)
        {
            static string WriteSequence(IEnumerable<T> sequence) => $"{{ {string.Join(", ", sequence)} }}";

            throw new AssertFailedException(
                $"Sequences {WriteSequence(expected)} and {WriteSequence(actual)} were not sequence-equal.", e);
        }
    }
}
