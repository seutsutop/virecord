using Xunit;
using VIrecord.HistoryLib;

namespace VIrecord.Tests;

public class HistoryFilterTests
{
    private static List<HistoryItem> CreateSampleItems()
    {
        return new List<HistoryItem>
        {
            new HistoryItem
            {
                FileName = "screenshot.png",
                Type = "Image",
                Host = "imgur.com",
                URL = "https://imgur.com/abc123",
                DateTime = new DateTime(2025, 1, 15),
                Tags = new Dictionary<string, string> { { "Favorite", null }, { "Tag", "work" } }
            },
            new HistoryItem
            {
                FileName = "document.pdf",
                Type = "File",
                Host = "dropbox.com",
                URL = "https://dropbox.com/doc456",
                DateTime = new DateTime(2025, 3, 20),
                Tags = new Dictionary<string, string> { { "Tag", "personal" } }
            },
            new HistoryItem
            {
                FileName = "video.mp4",
                Type = "Video",
                Host = "youtube.com",
                URL = "https://youtube.com/watch?v=xyz",
                DateTime = new DateTime(2025, 6, 10),
                Tags = new Dictionary<string, string> { { "Favorite", null } }
            },
            new HistoryItem
            {
                FileName = "code_snippet.txt",
                Type = "Text",
                Host = "pastebin.com",
                URL = "https://pastebin.com/raw/abc",
                DateTime = new DateTime(2024, 12, 1),
                Tags = null
            }
        };
    }

    [Fact]
    public void NoFilters_ReturnsAllItems()
    {
        var items = CreateSampleItems();
        var filter = new HistoryFilter();
        var result = filter.ApplyFilter(items).ToList();
        Assert.Equal(4, result.Count);
    }

    [Fact]
    public void FilterFavorites_ReturnsOnlyFavorites()
    {
        var items = CreateSampleItems();
        var filter = new HistoryFilter { FilterFavorites = true };
        var result = filter.ApplyFilter(items).ToList();
        Assert.Equal(2, result.Count);
        Assert.All(result, item => Assert.True(item.Favorite));
    }

    [Fact]
    public void FilterType_FiltersCorrectly()
    {
        var items = CreateSampleItems();
        var filter = new HistoryFilter { FilterType = true, Type = "Image" };
        var result = filter.ApplyFilter(items).ToList();
        Assert.Single(result);
        Assert.Equal("screenshot.png", result[0].FileName);
    }

    [Fact]
    public void FilterType_CaseInsensitive()
    {
        var items = CreateSampleItems();
        var filter = new HistoryFilter { FilterType = true, Type = "image" };
        var result = filter.ApplyFilter(items).ToList();
        Assert.Single(result);
    }

    [Fact]
    public void FilterHost_FiltersCorrectly()
    {
        var items = CreateSampleItems();
        var filter = new HistoryFilter { FilterHost = true, Host = "imgur" };
        var result = filter.ApplyFilter(items).ToList();
        Assert.Single(result);
        Assert.Equal("imgur.com", result[0].Host);
    }

    [Fact]
    public void FilterHost_CaseInsensitive()
    {
        var items = CreateSampleItems();
        var filter = new HistoryFilter { FilterHost = true, Host = "IMGUR" };
        var result = filter.ApplyFilter(items).ToList();
        Assert.Single(result);
    }

    [Fact]
    public void FilterHost_PartialMatch()
    {
        var items = CreateSampleItems();
        var filter = new HistoryFilter { FilterHost = true, Host = ".com" };
        var result = filter.ApplyFilter(items).ToList();
        Assert.Equal(4, result.Count);
    }

    [Fact]
    public void FilterByFilename_ExactMatch()
    {
        var items = CreateSampleItems();
        var filter = new HistoryFilter { Filename = "screenshot.png" };
        var result = filter.ApplyFilter(items).ToList();
        Assert.Single(result);
        Assert.Equal("screenshot.png", result[0].FileName);
    }

    [Fact]
    public void FilterByFilename_WildcardMatch()
    {
        var items = CreateSampleItems();
        var filter = new HistoryFilter { Filename = "*.png" };
        var result = filter.ApplyFilter(items).ToList();
        Assert.Single(result);
        Assert.Equal("screenshot.png", result[0].FileName);
    }

    [Fact]
    public void FilterByFilename_QuestionMarkWildcard()
    {
        var items = CreateSampleItems();
        var filter = new HistoryFilter { Filename = "vide?.mp4" };
        var result = filter.ApplyFilter(items).ToList();
        Assert.Single(result);
        Assert.Equal("video.mp4", result[0].FileName);
    }

    [Fact]
    public void FilterByFilename_CaseInsensitive()
    {
        var items = CreateSampleItems();
        var filter = new HistoryFilter { Filename = "SCREENSHOT.PNG" };
        var result = filter.ApplyFilter(items).ToList();
        Assert.Single(result);
    }

    [Fact]
    public void FilterByFilename_MatchesInTags_WhenSearchInTagsEnabled()
    {
        var items = CreateSampleItems();
        var filter = new HistoryFilter { Filename = "work", SearchInTags = true };
        var result = filter.ApplyFilter(items).ToList();
        Assert.Single(result);
        Assert.Equal("screenshot.png", result[0].FileName);
    }

    [Fact]
    public void FilterByURL_FiltersByContains()
    {
        var items = CreateSampleItems();
        var filter = new HistoryFilter { URL = "imgur" };
        var result = filter.ApplyFilter(items).ToList();
        Assert.Single(result);
        Assert.Contains("imgur", result[0].URL);
    }

    [Fact]
    public void FilterByURL_CaseInsensitive()
    {
        var items = CreateSampleItems();
        var filter = new HistoryFilter { URL = "IMGUR" };
        var result = filter.ApplyFilter(items).ToList();
        Assert.Single(result);
    }

    [Fact]
    public void FilterDate_ReturnsItemsInRange()
    {
        var items = CreateSampleItems();
        var filter = new HistoryFilter
        {
            FilterDate = true,
            FromDate = new DateTime(2025, 1, 1),
            ToDate = new DateTime(2025, 3, 31)
        };
        var result = filter.ApplyFilter(items).ToList();
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void FilterDate_IncludesEdgeDates()
    {
        var items = CreateSampleItems();
        var filter = new HistoryFilter
        {
            FilterDate = true,
            FromDate = new DateTime(2025, 1, 15),
            ToDate = new DateTime(2025, 1, 15)
        };
        var result = filter.ApplyFilter(items).ToList();
        Assert.Single(result);
        Assert.Equal("screenshot.png", result[0].FileName);
    }

    [Fact]
    public void MaxItemCount_LimitsResults()
    {
        var items = CreateSampleItems();
        var filter = new HistoryFilter { MaxItemCount = 2 };
        var result = filter.ApplyFilter(items).ToList();
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void MaxItemCount_Zero_ReturnsAll()
    {
        var items = CreateSampleItems();
        var filter = new HistoryFilter { MaxItemCount = 0 };
        var result = filter.ApplyFilter(items).ToList();
        Assert.Equal(4, result.Count);
    }

    [Fact]
    public void CombinedFilters_TypeAndHost()
    {
        var items = CreateSampleItems();
        var filter = new HistoryFilter
        {
            FilterType = true,
            Type = "Image",
            FilterHost = true,
            Host = "imgur"
        };
        var result = filter.ApplyFilter(items).ToList();
        Assert.Single(result);
        Assert.Equal("screenshot.png", result[0].FileName);
    }

    [Fact]
    public void CombinedFilters_TypeAndDate()
    {
        var items = CreateSampleItems();
        var filter = new HistoryFilter
        {
            FilterType = true,
            Type = "Video",
            FilterDate = true,
            FromDate = new DateTime(2025, 1, 1),
            ToDate = new DateTime(2025, 12, 31)
        };
        var result = filter.ApplyFilter(items).ToList();
        Assert.Single(result);
        Assert.Equal("video.mp4", result[0].FileName);
    }

    [Fact]
    public void FilterFavorites_IgnoresOtherFilters()
    {
        var items = CreateSampleItems();
        var filter = new HistoryFilter
        {
            FilterFavorites = true,
            FilterType = true,
            Type = "NonExistent"
        };
        var result = filter.ApplyFilter(items).ToList();
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void EmptyInput_ReturnsEmpty()
    {
        var filter = new HistoryFilter { FilterType = true, Type = "Image" };
        var result = filter.ApplyFilter(Enumerable.Empty<HistoryItem>()).ToList();
        Assert.Empty(result);
    }

    [Fact]
    public void FilterType_EmptyType_NoFiltering()
    {
        var items = CreateSampleItems();
        var filter = new HistoryFilter { FilterType = true, Type = "" };
        var result = filter.ApplyFilter(items).ToList();
        Assert.Equal(4, result.Count);
    }

    [Fact]
    public void FilterHost_EmptyHost_NoFiltering()
    {
        var items = CreateSampleItems();
        var filter = new HistoryFilter { FilterHost = true, Host = "" };
        var result = filter.ApplyFilter(items).ToList();
        Assert.Equal(4, result.Count);
    }

    [Fact]
    public void DefaultSearchInTags_IsTrue()
    {
        var filter = new HistoryFilter();
        Assert.True(filter.SearchInTags);
    }

    [Fact]
    public void NullTags_DoNotCrashFilter()
    {
        var items = new List<HistoryItem>
        {
            new HistoryItem { FileName = "test.txt", Tags = null }
        };
        var filter = new HistoryFilter { Filename = "nomatch", SearchInTags = true };
        var result = filter.ApplyFilter(items).ToList();
        Assert.Empty(result);
    }

    [Fact]
    public void NullFileName_DoesNotCrashFilenameFilter()
    {
        var items = new List<HistoryItem>
        {
            new HistoryItem { FileName = null, Tags = null }
        };
        var filter = new HistoryFilter { Filename = "test" };
        var result = filter.ApplyFilter(items).ToList();
        Assert.Empty(result);
    }
}
