using Xunit;
using VIrecord.HistoryLib;

namespace VIrecord.Tests;

public class HistoryItemTests
{
    [Fact]
    public void Favorite_ReturnsFalse_WhenTagsAreNull()
    {
        var item = new HistoryItem { Tags = null };
        Assert.False(item.Favorite);
    }

    [Fact]
    public void Favorite_ReturnsFalse_WhenNoFavoriteTag()
    {
        var item = new HistoryItem { Tags = new Dictionary<string, string>() };
        Assert.False(item.Favorite);
    }

    [Fact]
    public void Favorite_ReturnsTrue_WhenFavoriteTagExists()
    {
        var item = new HistoryItem
        {
            Tags = new Dictionary<string, string> { { "Favorite", null } }
        };
        Assert.True(item.Favorite);
    }

    [Fact]
    public void SetFavorite_True_AddsFavoriteTag()
    {
        var item = new HistoryItem();
        item.Favorite = true;
        Assert.NotNull(item.Tags);
        Assert.True(item.Tags.ContainsKey("Favorite"));
    }

    [Fact]
    public void SetFavorite_True_CreatesTagsDictionary_WhenNull()
    {
        var item = new HistoryItem { Tags = null };
        item.Favorite = true;
        Assert.NotNull(item.Tags);
        Assert.True(item.Favorite);
    }

    [Fact]
    public void SetFavorite_False_RemovesFavoriteTag()
    {
        var item = new HistoryItem();
        item.Favorite = true;
        Assert.True(item.Favorite);

        item.Favorite = false;
        Assert.False(item.Favorite);
        Assert.False(item.Tags.ContainsKey("Favorite"));
    }

    [Fact]
    public void TagsWindowTitle_ReturnsNull_WhenTagsAreNull()
    {
        var item = new HistoryItem { Tags = null };
        Assert.Null(item.TagsWindowTitle);
    }

    [Fact]
    public void TagsWindowTitle_ReturnsNull_WhenKeyMissing()
    {
        var item = new HistoryItem { Tags = new Dictionary<string, string>() };
        Assert.Null(item.TagsWindowTitle);
    }

    [Fact]
    public void TagsWindowTitle_ReturnsValue_WhenPresent()
    {
        var item = new HistoryItem
        {
            Tags = new Dictionary<string, string> { { "WindowTitle", "My App" } }
        };
        Assert.Equal("My App", item.TagsWindowTitle);
    }

    [Fact]
    public void TagsProcessName_ReturnsNull_WhenTagsAreNull()
    {
        var item = new HistoryItem { Tags = null };
        Assert.Null(item.TagsProcessName);
    }

    [Fact]
    public void TagsProcessName_ReturnsValue_WhenPresent()
    {
        var item = new HistoryItem
        {
            Tags = new Dictionary<string, string> { { "ProcessName", "notepad" } }
        };
        Assert.Equal("notepad", item.TagsProcessName);
    }

    [Fact]
    public void Tag_ReturnsNull_WhenTagsAreNull()
    {
        var item = new HistoryItem { Tags = null };
        Assert.Null(item.Tag);
    }

    [Fact]
    public void Tag_ReturnsNull_WhenKeyMissing()
    {
        var item = new HistoryItem { Tags = new Dictionary<string, string>() };
        Assert.Null(item.Tag);
    }

    [Fact]
    public void Tag_ReturnsValue_WhenPresent()
    {
        var item = new HistoryItem
        {
            Tags = new Dictionary<string, string> { { "Tag", "important" } }
        };
        Assert.Equal("important", item.Tag);
    }

    [Fact]
    public void SetTag_AddsTagKey()
    {
        var item = new HistoryItem();
        item.Tag = "screenshot";
        Assert.NotNull(item.Tags);
        Assert.Equal("screenshot", item.Tags["Tag"]);
    }

    [Fact]
    public void SetTag_CreatesTagsDictionary_WhenNull()
    {
        var item = new HistoryItem { Tags = null };
        item.Tag = "test";
        Assert.NotNull(item.Tags);
        Assert.Equal("test", item.Tag);
    }

    [Fact]
    public void SetTag_Null_RemovesTagKey()
    {
        var item = new HistoryItem();
        item.Tag = "something";
        Assert.Equal("something", item.Tag);

        item.Tag = null;
        Assert.False(item.Tags.ContainsKey("Tag"));
    }

    [Fact]
    public void SetTag_Empty_RemovesTagKey()
    {
        var item = new HistoryItem();
        item.Tag = "something";
        item.Tag = "";
        Assert.False(item.Tags.ContainsKey("Tag"));
    }

    [Fact]
    public void BasicProperties_CanBeSetAndRead()
    {
        var now = DateTime.UtcNow;
        var item = new HistoryItem
        {
            Id = 42,
            FileName = "screenshot.png",
            FilePath = @"C:\Screenshots\screenshot.png",
            DateTime = now,
            Type = "Image",
            Host = "imgur.com",
            URL = "https://imgur.com/abc123",
            ThumbnailURL = "https://imgur.com/abc123_thumb",
            DeletionURL = "https://imgur.com/delete/abc123",
            ShortenedURL = "https://is.gd/short"
        };

        Assert.Equal(42, item.Id);
        Assert.Equal("screenshot.png", item.FileName);
        Assert.Equal(@"C:\Screenshots\screenshot.png", item.FilePath);
        Assert.Equal(now, item.DateTime);
        Assert.Equal("Image", item.Type);
        Assert.Equal("imgur.com", item.Host);
        Assert.Equal("https://imgur.com/abc123", item.URL);
        Assert.Equal("https://imgur.com/abc123_thumb", item.ThumbnailURL);
        Assert.Equal("https://imgur.com/delete/abc123", item.DeletionURL);
        Assert.Equal("https://is.gd/short", item.ShortenedURL);
    }

    [Fact]
    public void MultipleTags_CanCoexist()
    {
        var item = new HistoryItem();
        item.Favorite = true;
        item.Tag = "work";

        Assert.True(item.Favorite);
        Assert.Equal("work", item.Tag);
        Assert.Equal(2, item.Tags.Count);
    }
}
