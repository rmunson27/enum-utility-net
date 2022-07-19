using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.CoreTest.Utilities;

/// <summary>
/// Tests of the <see cref="EnumUnderlyingType"/> enum and accompanying <see cref="EnumUnderlyingTypes"/>
/// static class functionality.
/// </summary>
[TestClass]
public class EnumUnderlyingTypeTest
{
    private static readonly EnumUnderlyingType Unnamed = Enum.GetValues<EnumUnderlyingType>().Max() + 1;

    /// <summary>
    /// Tests the <see cref="EnumUnderlyingTypes.ToType(EnumUnderlyingType)"/> method.
    /// </summary>
    [TestMethod]
    public void TestToType()
    {
        Assert.AreEqual(typeof(byte), EnumUnderlyingType.Byte.ToType());
        Assert.AreEqual(typeof(sbyte), EnumUnderlyingType.SByte.ToType());
        Assert.AreEqual(typeof(short), EnumUnderlyingType.Short.ToType());
        Assert.AreEqual(typeof(ushort), EnumUnderlyingType.UShort.ToType());
        Assert.AreEqual(typeof(int), EnumUnderlyingType.Int.ToType());
        Assert.AreEqual(typeof(uint), EnumUnderlyingType.UInt.ToType());
        Assert.AreEqual(typeof(long), EnumUnderlyingType.Long.ToType());
        Assert.AreEqual(typeof(ulong), EnumUnderlyingType.ULong.ToType());

        Assert.ThrowsException<InvalidEnumArgumentException>(() => Unnamed.ToType());
    }

    /// <summary>
    /// Tests the <see cref="EnumUnderlyingTypes.FromType(Type)"/> method.
    /// </summary>
    [TestMethod]
    public void TestFromType()
    {
        Assert.AreEqual(EnumUnderlyingType.Byte, EnumUnderlyingTypes.FromType(typeof(byte)));
        Assert.AreEqual(EnumUnderlyingType.SByte, EnumUnderlyingTypes.FromType(typeof(sbyte)));
        Assert.AreEqual(EnumUnderlyingType.Short, EnumUnderlyingTypes.FromType(typeof(short)));
        Assert.AreEqual(EnumUnderlyingType.UShort, EnumUnderlyingTypes.FromType(typeof(ushort)));
        Assert.AreEqual(EnumUnderlyingType.Int, EnumUnderlyingTypes.FromType(typeof(int)));
        Assert.AreEqual(EnumUnderlyingType.UInt, EnumUnderlyingTypes.FromType(typeof(uint)));
        Assert.AreEqual(EnumUnderlyingType.Long, EnumUnderlyingTypes.FromType(typeof(long)));
        Assert.AreEqual(EnumUnderlyingType.ULong, EnumUnderlyingTypes.FromType(typeof(ulong)));

        Assert.ThrowsException<ArgumentException>(() => EnumUnderlyingTypes.FromType(typeof(char)));
    }
}
