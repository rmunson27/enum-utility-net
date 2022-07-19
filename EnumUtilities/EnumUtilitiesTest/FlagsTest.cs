﻿using System;
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
        Assert.IsTrue(BitSet_Defaults.AllAtomicValues.ToHashSet().SetEquals(Enums.GetAtomicValues<BitSet_Default>()));

        // Otherwise values should be everything defined
        Assert.IsTrue(NonBitSets.AllValues.ToHashSet().SetEquals(Enums.GetAtomicValues<NonBitSet>()));
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