using System.Drawing;
using Xunit;
using VIrecord.HelpersLib;

namespace VIrecord.Tests;

public class MathHelpersTests
{
    [Theory]
    [InlineData(5, 10, 5)]
    [InlineData(15, 10, 10)]
    [InlineData(10, 10, 10)]
    public void Min_ReturnsSmaller(int num, int min, int expected)
    {
        Assert.Equal(expected, MathHelpers.Min(num, min));
    }

    [Theory]
    [InlineData(5, 10, 10)]
    [InlineData(15, 10, 15)]
    [InlineData(10, 10, 10)]
    public void Max_ReturnsLarger(int num, int max, int expected)
    {
        Assert.Equal(expected, MathHelpers.Max(num, max));
    }

    [Theory]
    [InlineData(5, 0, 10, 5)]
    [InlineData(-5, 0, 10, 0)]
    [InlineData(15, 0, 10, 10)]
    [InlineData(0, 0, 10, 0)]
    [InlineData(10, 0, 10, 10)]
    public void Clamp_ClampsToRange(int num, int min, int max, int expected)
    {
        Assert.Equal(expected, MathHelpers.Clamp(num, min, max));
    }

    [Theory]
    [InlineData(5, 0, 10, true)]
    [InlineData(-1, 0, 10, false)]
    [InlineData(11, 0, 10, false)]
    [InlineData(0, 0, 10, true)]
    [InlineData(10, 0, 10, true)]
    public void IsBetween_ChecksRange(int num, int min, int max, bool expected)
    {
        Assert.Equal(expected, MathHelpers.IsBetween(num, min, max));
    }

    [Theory]
    [InlineData(5, 0, 10, -1, 5)]
    [InlineData(-1, 0, 10, -1, -1)]
    [InlineData(11, 0, 10, -1, -1)]
    public void BetweenOrDefault_ReturnsDefaultWhenOutOfRange(int num, int min, int max, int defaultVal, int expected)
    {
        Assert.Equal(expected, MathHelpers.BetweenOrDefault(num, min, max, defaultVal));
    }

    [Theory]
    [InlineData(0, 0, 10, 0, 100, 0f)]
    [InlineData(5, 0, 10, 0, 100, 50f)]
    [InlineData(10, 0, 10, 0, 100, 100f)]
    public void Remap_RemapsValueCorrectly(float value, float from1, float to1, float from2, float to2, float expected)
    {
        Assert.Equal(expected, MathHelpers.Remap(value, from1, to1, from2, to2), 4);
    }

    [Theory]
    [InlineData(0, true)]
    [InlineData(2, true)]
    [InlineData(4, true)]
    [InlineData(1, false)]
    [InlineData(3, false)]
    [InlineData(-2, true)]
    public void IsEvenNumber_ChecksCorrectly(int num, bool expected)
    {
        Assert.Equal(expected, MathHelpers.IsEvenNumber(num));
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(3, true)]
    [InlineData(0, false)]
    [InlineData(2, false)]
    [InlineData(-1, true)]
    public void IsOddNumber_ChecksCorrectly(int num, bool expected)
    {
        Assert.Equal(expected, MathHelpers.IsOddNumber(num));
    }

    [Theory]
    [InlineData(0f, 10f, 0f, 0f)]
    [InlineData(0f, 10f, 1f, 10f)]
    [InlineData(0f, 10f, 0.5f, 5f)]
    [InlineData(10f, 20f, 0.25f, 12.5f)]
    public void Lerp_InterpolatesCorrectly(float v1, float v2, float amount, float expected)
    {
        Assert.Equal(expected, MathHelpers.Lerp(v1, v2, amount), 4);
    }

    [Fact]
    public void Lerp_Vector2_InterpolatesCorrectly()
    {
        var pos1 = new Vector2(0, 0);
        var pos2 = new Vector2(10, 20);
        var result = MathHelpers.Lerp(pos1, pos2, 0.5f);
        Assert.Equal(5f, result.X);
        Assert.Equal(10f, result.Y);
    }

    [Fact]
    public void RadianToDegree_ConvertsCorrectly()
    {
        float result = MathHelpers.RadianToDegree((float)Math.PI);
        Assert.Equal(180f, result, 1);
    }

    [Fact]
    public void DegreeToRadian_ConvertsCorrectly()
    {
        float result = MathHelpers.DegreeToRadian(180f);
        Assert.Equal((float)Math.PI, result, 1);
    }

    [Fact]
    public void RadianToDegree_And_DegreeToRadian_AreInverses()
    {
        float degrees = 45f;
        float radians = MathHelpers.DegreeToRadian(degrees);
        float backToDegrees = MathHelpers.RadianToDegree(radians);
        Assert.Equal(degrees, backToDegrees, 1);
    }

    [Fact]
    public void RadianToVector2_AtZeroRadian_PointsRight()
    {
        var v = MathHelpers.RadianToVector2(0f);
        Assert.Equal(1f, v.X, 4);
        Assert.Equal(0f, v.Y, 4);
    }

    [Fact]
    public void RadianToVector2_WithLength_ScalesCorrectly()
    {
        var v = MathHelpers.RadianToVector2(0f, 5f);
        Assert.Equal(5f, v.X, 4);
        Assert.Equal(0f, v.Y, 4);
    }

    [Fact]
    public void DegreeToVector2_At90Degrees_PointsUp()
    {
        var v = MathHelpers.DegreeToVector2(90f);
        Assert.Equal(0f, v.X, 1);
        Assert.Equal(1f, v.Y, 1);
    }

    [Fact]
    public void Vector2ToRadian_ReturnsCorrectAngle()
    {
        var direction = new Vector2(1, 0);
        float radian = MathHelpers.Vector2ToRadian(direction);
        Assert.Equal(0f, radian, 4);
    }

    [Fact]
    public void Vector2ToDegree_ReturnsCorrectAngle()
    {
        var direction = new Vector2(0, 1);
        float degree = MathHelpers.Vector2ToDegree(direction);
        Assert.Equal(90f, degree, 0);
    }

    [Fact]
    public void LookAtRadian_Vector2_ReturnsAngleToTarget()
    {
        var from = new Vector2(0, 0);
        var to = new Vector2(1, 0);
        float radian = MathHelpers.LookAtRadian(from, to);
        Assert.Equal(0f, radian, 4);
    }

    [Fact]
    public void LookAtRadian_PointF_ReturnsAngleToTarget()
    {
        var from = new PointF(0, 0);
        var to = new PointF(0, 1);
        float radian = MathHelpers.LookAtRadian(from, to);
        Assert.Equal((float)(Math.PI / 2), radian, 2);
    }

    [Fact]
    public void LookAtDegree_Vector2_ReturnsAngleInDegrees()
    {
        var from = new Vector2(0, 0);
        var to = new Vector2(1, 1);
        float degree = MathHelpers.LookAtDegree(from, to);
        Assert.Equal(45f, degree, 0);
    }

    [Fact]
    public void LookAtDegree_PointF_ReturnsAngleInDegrees()
    {
        var from = new PointF(0, 0);
        var to = new PointF(1, 1);
        float degree = MathHelpers.LookAtDegree(from, to);
        Assert.Equal(45f, degree, 0);
    }

    [Fact]
    public void Distance_Vector2_CalculatesCorrectly()
    {
        var a = new Vector2(0, 0);
        var b = new Vector2(3, 4);
        Assert.Equal(5f, MathHelpers.Distance(a, b), 4);
    }

    [Fact]
    public void Distance_PointF_CalculatesCorrectly()
    {
        var a = new PointF(0, 0);
        var b = new PointF(3, 4);
        Assert.Equal(5f, MathHelpers.Distance(a, b), 4);
    }

    [Fact]
    public void Distance_SamePoint_IsZero()
    {
        var p = new Vector2(5, 5);
        Assert.Equal(0f, MathHelpers.Distance(p, p), 4);
    }

    [Fact]
    public void Constants_AreCorrect()
    {
        Assert.Equal(57.29578f, MathHelpers.RadianPI, 3);
        Assert.Equal(0.01745329f, MathHelpers.DegreePI, 6);
        Assert.Equal(6.28319f, MathHelpers.TwoPI, 3);
    }
}

public class Vector2Tests
{
    [Fact]
    public void Constructor_SetsXAndY()
    {
        var v = new Vector2(3f, 4f);
        Assert.Equal(3f, v.X);
        Assert.Equal(4f, v.Y);
    }

    [Fact]
    public void Empty_HasZeroComponents()
    {
        Assert.Equal(0f, Vector2.Empty.X);
        Assert.Equal(0f, Vector2.Empty.Y);
    }

    [Fact]
    public void Addition_Works()
    {
        var a = new Vector2(1, 2);
        var b = new Vector2(3, 4);
        var result = a + b;
        Assert.Equal(4f, result.X);
        Assert.Equal(6f, result.Y);
    }

    [Fact]
    public void Subtraction_Works()
    {
        var a = new Vector2(5, 7);
        var b = new Vector2(2, 3);
        var result = a - b;
        Assert.Equal(3f, result.X);
        Assert.Equal(4f, result.Y);
    }

    [Fact]
    public void ScalarMultiplication_Works()
    {
        var v = new Vector2(2, 3);
        var result = v * 3f;
        Assert.Equal(6f, result.X);
        Assert.Equal(9f, result.Y);
    }

    [Fact]
    public void ScalarDivision_Works()
    {
        var v = new Vector2(6, 9);
        var result = v / 3f;
        Assert.Equal(2f, result.X);
        Assert.Equal(3f, result.Y);
    }

    [Fact]
    public void Negation_Works()
    {
        var v = new Vector2(3, -4);
        var result = -v;
        Assert.Equal(-3f, result.X);
        Assert.Equal(4f, result.Y);
    }

    [Fact]
    public void Equality_TrueForSameValues()
    {
        var a = new Vector2(1, 2);
        var b = new Vector2(1, 2);
        Assert.True(a == b);
        Assert.False(a != b);
    }

    [Fact]
    public void Equality_FalseForDifferentValues()
    {
        var a = new Vector2(1, 2);
        var b = new Vector2(3, 4);
        Assert.False(a == b);
        Assert.True(a != b);
    }

    [Fact]
    public void Equals_ReturnsTrueForSameValues()
    {
        var a = new Vector2(1, 2);
        var b = new Vector2(1, 2);
        Assert.True(a.Equals(b));
    }

    [Fact]
    public void Equals_ReturnsFalseForNonVector2()
    {
        var v = new Vector2(1, 2);
        Assert.False(v.Equals("not a vector"));
    }

    [Fact]
    public void ToString_FormatsCorrectly()
    {
        var v = new Vector2(3.5f, 4.5f);
        Assert.Equal("X=3.5, Y=4.5", v.ToString());
    }

    [Fact]
    public void ExplicitCast_ToPoint_Rounds()
    {
        var v = new Vector2(3.7f, 4.3f);
        var p = (Point)v;
        Assert.Equal(4, p.X);
        Assert.Equal(4, p.Y);
    }

    [Fact]
    public void ExplicitCast_ToPointF()
    {
        var v = new Vector2(3.5f, 4.5f);
        var p = (PointF)v;
        Assert.Equal(3.5f, p.X);
        Assert.Equal(4.5f, p.Y);
    }

    [Fact]
    public void ImplicitCast_FromPoint()
    {
        Vector2 v = new Point(3, 4);
        Assert.Equal(3f, v.X);
        Assert.Equal(4f, v.Y);
    }
}
