using Xunit;
using VIrecord.HelpersLib;

namespace VIrecord.Tests;

public class NumberExtensionsTests
{
    [Theory]
    [InlineData(5, 10, 5)]
    [InlineData(15, 10, 10)]
    public void Min_Extension_Works(int num, int min, int expected)
    {
        Assert.Equal(expected, num.Min(min));
    }

    [Theory]
    [InlineData(5, 10, 10)]
    [InlineData(15, 10, 15)]
    public void Max_Extension_Works(int num, int max, int expected)
    {
        Assert.Equal(expected, num.Max(max));
    }

    [Theory]
    [InlineData(5, 0, 10, 5)]
    [InlineData(-5, 0, 10, 0)]
    [InlineData(15, 0, 10, 10)]
    public void Clamp_Extension_Works(int num, int min, int max, int expected)
    {
        Assert.Equal(expected, num.Clamp(min, max));
    }

    [Theory]
    [InlineData(5, 0, 10, true)]
    [InlineData(-1, 0, 10, false)]
    [InlineData(11, 0, 10, false)]
    public void IsBetween_Extension_Works(int num, int min, int max, bool expected)
    {
        Assert.Equal(expected, num.IsBetween(min, max));
    }

    [Fact]
    public void BetweenOrDefault_Extension_ReturnsValueInRange()
    {
        Assert.Equal(5, 5.BetweenOrDefault(0, 10));
    }

    [Fact]
    public void BetweenOrDefault_Extension_ReturnsDefault_WhenOutOfRange()
    {
        Assert.Equal(-1, 15.BetweenOrDefault(0, 10, -1));
    }

    [Theory]
    [InlineData(0, true)]
    [InlineData(2, true)]
    [InlineData(1, false)]
    [InlineData(3, false)]
    public void IsEvenNumber_Extension_Works(int num, bool expected)
    {
        Assert.Equal(expected, num.IsEvenNumber());
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(3, true)]
    [InlineData(0, false)]
    [InlineData(2, false)]
    public void IsOddNumber_Extension_Works(int num, bool expected)
    {
        Assert.Equal(expected, num.IsOddNumber());
    }

    [Fact]
    public void Remap_Extension_Works()
    {
        float result = 5f.Remap(0, 10, 0, 100);
        Assert.Equal(50f, result, 4);
    }

    [Theory]
    [InlineData(0L, "0 B")]
    [InlineData(500L, "500 B")]
    [InlineData(1000L, "1.00 KB")]
    [InlineData(1500L, "1.50 KB")]
    [InlineData(1000000L, "1.00 MB")]
    [InlineData(1500000L, "1.50 MB")]
    [InlineData(1000000000L, "1.00 GB")]
    [InlineData(1000000000000L, "1.00 TB")]
    public void ToSizeString_Decimal_FormatsCorrectly(long size, string expected)
    {
        Assert.Equal(expected, size.ToSizeString(binary: false));
    }

    [Theory]
    [InlineData(0L, "0 B")]
    [InlineData(1024L, "1.00 KiB")]
    [InlineData(1536L, "1.50 KiB")]
    [InlineData(1048576L, "1.00 MiB")]
    [InlineData(1073741824L, "1.00 GiB")]
    public void ToSizeString_Binary_FormatsCorrectly(long size, string expected)
    {
        Assert.Equal(expected, size.ToSizeString(binary: true));
    }

    [Fact]
    public void ToSizeString_NegativeSize_ReturnsZeroBytes()
    {
        Assert.Equal("0 B", (-100L).ToSizeString());
    }

    [Fact]
    public void ToSizeString_CustomDecimalPlaces()
    {
        Assert.Equal("1.5 KB", 1500L.ToSizeString(binary: false, decimalPlaces: 1));
    }

    [Theory]
    [InlineData(3.14159, 2, "3.14")]
    [InlineData(3.14159, 0, "3")]
    [InlineData(3.14159, 4, "3.1416")]
    [InlineData(0.0, 2, "0.00")]
    public void ToDecimalString_FormatsCorrectly(double number, int places, string expected)
    {
        Assert.Equal(expected, number.ToDecimalString(places));
    }

    [Theory]
    [InlineData(10, 2, "01", "1010")]
    [InlineData(255, 16, "0123456789ABCDEF", "FF")]
    [InlineData(7, 8, "01234567", "7")]
    public void ToBase_ConvertsCorrectly(int value, int radix, string digits, string expected)
    {
        Assert.Equal(expected, value.ToBase(radix, digits));
    }

    [Fact]
    public void ToBase_ThrowsForNullDigits()
    {
        Assert.Throws<ArgumentNullException>(() => 10.ToBase(2, null));
    }

    [Fact]
    public void ToBase_ThrowsForInvalidRadix()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => 10.ToBase(1, "01"));
    }

    [Fact]
    public void ToBase_AndFromBase_AreInverses()
    {
        string digits = "0123456789ABCDEF";
        int original = 12345;
        string hex = original.ToBase(16, digits);
        int back = hex.FromBase(16, digits);
        Assert.Equal(original, back);
    }
}
