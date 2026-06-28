using Xunit;
using VIrecord.HelpersLib;

namespace VIrecord.Tests;

public class StringExtensionsTests
{
    [Fact]
    public void Left_ReturnsLeftChars()
    {
        Assert.Equal("Hel", "Hello".Left(3));
    }

    [Fact]
    public void Left_ReturnsFull_WhenLengthExceedsString()
    {
        Assert.Equal("Hi", "Hi".Left(10));
    }

    [Fact]
    public void Left_ReturnsEmpty_WhenLengthIsZero()
    {
        Assert.Equal("", "Hello".Left(0));
    }

    [Fact]
    public void Right_ReturnsRightChars()
    {
        Assert.Equal("llo", "Hello".Right(3));
    }

    [Fact]
    public void Right_ReturnsFull_WhenLengthExceedsString()
    {
        Assert.Equal("Hi", "Hi".Right(10));
    }

    [Fact]
    public void Right_ReturnsEmpty_WhenLengthIsZero()
    {
        Assert.Equal("", "Hello".Right(0));
    }

    [Fact]
    public void RemoveLeft_RemovesFromStart()
    {
        Assert.Equal("lo", "Hello".RemoveLeft(3));
    }

    [Fact]
    public void RemoveLeft_ReturnsFullString_WhenLengthExceeds()
    {
        Assert.Equal("Hi", "Hi".RemoveLeft(10));
    }

    [Fact]
    public void RemoveRight_RemovesFromEnd()
    {
        Assert.Equal("He", "Hello".RemoveRight(3));
    }

    [Fact]
    public void RemoveRight_ReturnsFullString_WhenLengthExceeds()
    {
        Assert.Equal("Hi", "Hi".RemoveRight(10));
    }

    [Fact]
    public void Between_ExtractsTextBetweenDelimiters()
    {
        string result = "Hello [world] test".Between("[", "]");
        Assert.Equal("world", result);
    }

    [Fact]
    public void Between_ReturnsNull_WhenFirstNotFound()
    {
        string result = "Hello world".Between("[", "]");
        Assert.Null(result);
    }

    [Fact]
    public void Between_ReturnsNull_WhenLastNotFound()
    {
        string result = "Hello [world".Between("[", "]");
        Assert.Null(result);
    }

    [Fact]
    public void Between_IncludeFirstAndLast()
    {
        string result = "Hello [world] test".Between("[", "]", includeFirstAndLast: true);
        Assert.Equal("[world]", result);
    }

    [Fact]
    public void Between_IsFirstMatchForEnd_UsesFirstOccurrence()
    {
        string result = "a[b]c]d".Between("[", "]", isFirstMatchForEnd: true);
        Assert.Equal("b", result);
    }

    [Fact]
    public void Repeat_RepeatsString()
    {
        Assert.Equal("abcabcabc", "abc".Repeat(3));
    }

    [Fact]
    public void Repeat_ReturnsNull_WhenCountIsZero()
    {
        Assert.Null("abc".Repeat(0));
    }

    [Fact]
    public void Repeat_ReturnsNull_ForEmptyString()
    {
        Assert.Null("".Repeat(3));
    }

    [Fact]
    public void Replace_WithStringComparison_CaseInsensitive()
    {
        string result = "Hello HELLO hello".Replace("hello", "world", StringComparison.OrdinalIgnoreCase);
        Assert.Equal("world world world", result);
    }

    [Fact]
    public void Replace_WithStringComparison_CaseSensitive()
    {
        string result = "Hello HELLO hello".Replace("hello", "world", StringComparison.Ordinal);
        Assert.Equal("Hello HELLO world", result);
    }

    [Fact]
    public void Replace_ReturnsOriginal_WhenOldValueNotFound()
    {
        string result = "Hello".Replace("xyz", "abc", StringComparison.Ordinal);
        Assert.Equal("Hello", result);
    }

    [Fact]
    public void ReplaceWith_ReplacesAllOccurrences_ByDefault()
    {
        string result = "aXbXcX".ReplaceWith("X", "Y");
        Assert.Equal("aYbYcY", result);
    }

    [Fact]
    public void ReplaceWith_ReplacesLimitedOccurrences()
    {
        string result = "aXbXcX".ReplaceWith("X", "Y", 2);
        Assert.Equal("aYbYcX", result);
    }

    [Fact]
    public void ReplaceFirst_ReplacesFirstOccurrence()
    {
        bool found = "aXbXcX".ReplaceFirst("X", "Y", out string result);
        Assert.True(found);
        Assert.Equal("aYbXcX", result);
    }

    [Fact]
    public void ReplaceFirst_ReturnsFalse_WhenNotFound()
    {
        bool found = "abc".ReplaceFirst("X", "Y", out string result);
        Assert.False(found);
        Assert.Equal("abc", result);
    }

    [Fact]
    public void ReplaceAll_ReplacesAllWithFunction()
    {
        int counter = 0;
        string result = "a$b$c".ReplaceAll("$", () => (++counter).ToString());
        Assert.Equal("a1b2c", result);
    }

    [Fact]
    public void BatchReplace_ReplacesMultiplePatterns()
    {
        var replacements = new Dictionary<string, string>
        {
            { "a", "1" },
            { "b", "2" }
        };
        string result = "abc".BatchReplace(replacements);
        Assert.Equal("12c", result);
    }

    [Fact]
    public void RemoveWhiteSpaces_RemovesAllWhitespace()
    {
        Assert.Equal("Hello", " H e l l o ".RemoveWhiteSpaces());
    }

    [Fact]
    public void Reverse_ReversesString()
    {
        Assert.Equal("olleH", "Hello".Reverse());
    }

    [Fact]
    public void Truncate_TruncatesLongString()
    {
        Assert.Equal("Hel", "Hello".Truncate(3));
    }

    [Fact]
    public void Truncate_ReturnsOriginal_WhenShortEnough()
    {
        Assert.Equal("Hi", "Hi".Truncate(10));
    }

    [Fact]
    public void Truncate_ReturnsNull_ForNullInput()
    {
        Assert.Null(((string)null).Truncate(5));
    }

    [Fact]
    public void Truncate_WithEndings_AddsEllipsis()
    {
        Assert.Equal("Hel...", "Hello World".Truncate(6, "..."));
    }

    [Fact]
    public void Truncate_WithEndings_FromLeft()
    {
        Assert.Equal("...rld", "Hello World".Truncate(6, "...", truncateFromRight: false));
    }

    [Fact]
    public void HexToBytes_ConvertsCorrectly()
    {
        byte[] bytes = "FF00AB".HexToBytes();
        Assert.Equal(new byte[] { 0xFF, 0x00, 0xAB }, bytes);
    }

    [Fact]
    public void ParseQuoteString_ExtractsQuotedContent()
    {
        Assert.Equal("hello", "\"hello\"".ParseQuoteString());
    }

    [Fact]
    public void ParseQuoteString_HandlesNoQuotes()
    {
        Assert.Equal("hello", "hello".ParseQuoteString());
    }

    [Fact]
    public void ParseQuoteString_HandlesLeadingText()
    {
        Assert.Equal("world", "key=\"world\"".ParseQuoteString());
    }

    [Fact]
    public void IsNumber_ReturnsTrueForNumericStrings()
    {
        Assert.True("123".IsNumber());
        Assert.True("0".IsNumber());
        Assert.True("-42".IsNumber());
    }

    [Fact]
    public void IsNumber_ReturnsFalseForNonNumericStrings()
    {
        Assert.False("abc".IsNumber());
        Assert.False("12.5".IsNumber());
        Assert.False("".IsNumber());
    }

    [Fact]
    public void Lines_SplitsByNewlines()
    {
        string[] result = "a\nb\r\nc".Lines();
        Assert.Equal(new[] { "a", "b", "c" }, result);
    }

    [Fact]
    public void ReplaceNewLines_NormalizesNewlines()
    {
        string result = "a\nb\r\nc".ReplaceNewLines("\n");
        Assert.Equal("a\nb\nc", result);
    }

    [Fact]
    public void ForEachBetween_FindsAllOccurrences()
    {
        var results = "a{1}b{2}c".ForEachBetween("{", "}").ToList();
        Assert.Equal(2, results.Count);
        Assert.Equal("1", results[0].Item2);
        Assert.Equal("2", results[1].Item2);
    }

    [Fact]
    public void FromBase_ConvertsFromBinaryToDecimal()
    {
        Assert.Equal(10, "1010".FromBase(2, "01"));
    }

    [Fact]
    public void FromBase_ConvertsFromHex()
    {
        Assert.Equal(255, "FF".FromBase(16, "0123456789ABCDEF"));
    }

    [Fact]
    public void FromBase_ThrowsForNullDigits()
    {
        Assert.Throws<ArgumentNullException>(() => "10".FromBase(2, null));
    }

    [Fact]
    public void FromBase_ThrowsForInvalidRadix()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => "10".FromBase(1, "01"));
    }

    [Fact]
    public void PadCenter_CentersString()
    {
        string result = "hi".PadCenter(6);
        Assert.Equal(6, result.Length);
        Assert.Equal("  hi  ", result);
    }

    [Fact]
    public void PadCenter_WithCustomChar()
    {
        string result = "hi".PadCenter(6, '-');
        Assert.Equal("--hi--", result);
    }

    [Fact]
    public void Contains_WithComparison_CaseInsensitive()
    {
        Assert.True("Hello World".Contains("hello", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Contains_WithComparison_CaseSensitive()
    {
        Assert.False("Hello World".Contains("hello", StringComparison.Ordinal));
    }
}
