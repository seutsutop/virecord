using Xunit;
using VIrecord.IndexerLib;

namespace VIrecord.Tests;

public class FolderInfoTests
{
    [Fact]
    public void Constructor_SetsFolderPath()
    {
        var folder = new FolderInfo("/home/user/Documents");
        Assert.Equal("/home/user/Documents", folder.FolderPath);
    }

    [Fact]
    public void Constructor_InitializesEmptyLists()
    {
        var folder = new FolderInfo("/test");
        Assert.NotNull(folder.Files);
        Assert.NotNull(folder.Folders);
        Assert.Empty(folder.Files);
        Assert.Empty(folder.Folders);
    }

    [Fact]
    public void FolderName_ReturnsLastSegment()
    {
        var folder = new FolderInfo("/home/user/Documents");
        Assert.Equal("Documents", folder.FolderName);
    }

    [Fact]
    public void FolderName_HandlesRootPath()
    {
        var folder = new FolderInfo("/");
        Assert.Equal("", folder.FolderName);
    }

    [Fact]
    public void IsEmpty_True_WhenNoFilesOrFolders()
    {
        var folder = new FolderInfo("/test");
        folder.Update();
        Assert.True(folder.IsEmpty);
    }

    [Fact]
    public void IsEmpty_False_WhenHasFiles()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "virecord_test_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);
        var tempFile = Path.Combine(tempDir, "test.txt");
        File.WriteAllText(tempFile, "content");

        try
        {
            var folder = new FolderInfo(tempDir);
            folder.Files.Add(new FileInfo(tempFile));
            folder.Update();
            Assert.False(folder.IsEmpty);
            Assert.Equal(1, folder.TotalFileCount);
        }
        finally
        {
            File.Delete(tempFile);
            Directory.Delete(tempDir);
        }
    }

    [Fact]
    public void Update_CalculatesSize_FromFiles()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "virecord_test_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);
        var tempFile = Path.Combine(tempDir, "test.txt");
        File.WriteAllText(tempFile, "Hello World");

        try
        {
            var folder = new FolderInfo(tempDir);
            folder.Files.Add(new FileInfo(tempFile));
            folder.Update();

            Assert.True(folder.Size > 0);
        }
        finally
        {
            File.Delete(tempFile);
            Directory.Delete(tempDir);
        }
    }

    [Fact]
    public void Update_CalculatesTotalFileCount_WithSubfolders()
    {
        var root = new FolderInfo("/root");
        var sub1 = new FolderInfo("/root/sub1");
        var sub2 = new FolderInfo("/root/sub2");

        var tempDir = Path.Combine(Path.GetTempPath(), "virecord_test_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);
        var f1 = Path.Combine(tempDir, "f1.txt");
        var f2 = Path.Combine(tempDir, "f2.txt");
        var f3 = Path.Combine(tempDir, "f3.txt");
        File.WriteAllText(f1, "a");
        File.WriteAllText(f2, "b");
        File.WriteAllText(f3, "c");

        try
        {
            root.Files.Add(new FileInfo(f1));
            sub1.Files.Add(new FileInfo(f2));
            sub2.Files.Add(new FileInfo(f3));

            root.Folders.Add(sub1);
            root.Folders.Add(sub2);

            root.Update();

            Assert.Equal(3, root.TotalFileCount);
            Assert.Equal(2, root.TotalFolderCount);
        }
        finally
        {
            File.Delete(f1);
            File.Delete(f2);
            File.Delete(f3);
            Directory.Delete(tempDir);
        }
    }

    [Fact]
    public void Update_CalculatesTotalFolderCount_WithNestedSubfolders()
    {
        var root = new FolderInfo("/root");
        var sub = new FolderInfo("/root/sub");
        var subsub = new FolderInfo("/root/sub/subsub");

        sub.Folders.Add(subsub);
        root.Folders.Add(sub);

        root.Update();

        Assert.Equal(2, root.TotalFolderCount);
        Assert.Equal(1, sub.TotalFolderCount);
        Assert.Equal(0, subsub.TotalFolderCount);
    }

    [Fact]
    public void Update_SortsFoldersByName()
    {
        var root = new FolderInfo("/root");
        root.Folders.Add(new FolderInfo("/root/zebra"));
        root.Folders.Add(new FolderInfo("/root/alpha"));
        root.Folders.Add(new FolderInfo("/root/middle"));

        root.Update();

        Assert.Equal("alpha", root.Folders[0].FolderName);
        Assert.Equal("middle", root.Folders[1].FolderName);
        Assert.Equal("zebra", root.Folders[2].FolderName);
    }

    [Fact]
    public void Update_AggregatesSize_FromSubfolders()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "virecord_test_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);
        var f1 = Path.Combine(tempDir, "f1.txt");
        var f2 = Path.Combine(tempDir, "f2.txt");
        File.WriteAllText(f1, "Hello");
        File.WriteAllText(f2, "World!!");

        try
        {
            var root = new FolderInfo("/root");
            var sub = new FolderInfo("/root/sub");

            root.Files.Add(new FileInfo(f1));
            sub.Files.Add(new FileInfo(f2));
            root.Folders.Add(sub);

            root.Update();

            long expectedSize = new FileInfo(f1).Length + new FileInfo(f2).Length;
            Assert.Equal(expectedSize, root.Size);
        }
        finally
        {
            File.Delete(f1);
            File.Delete(f2);
            Directory.Delete(tempDir);
        }
    }

    [Fact]
    public void Parent_CanBeSet()
    {
        var root = new FolderInfo("/root");
        var child = new FolderInfo("/root/child") { Parent = root };
        Assert.Same(root, child.Parent);
    }

    [Fact]
    public void EmptyFolder_HasZeroSizeAfterUpdate()
    {
        var folder = new FolderInfo("/empty");
        folder.Update();
        Assert.Equal(0, folder.Size);
        Assert.Equal(0, folder.TotalFileCount);
        Assert.Equal(0, folder.TotalFolderCount);
    }

    [Fact]
    public void DeeplyNested_Counts_AreCorrect()
    {
        var root = new FolderInfo("/r");
        var l1 = new FolderInfo("/r/l1");
        var l2 = new FolderInfo("/r/l1/l2");
        var l3 = new FolderInfo("/r/l1/l2/l3");

        l2.Folders.Add(l3);
        l1.Folders.Add(l2);
        root.Folders.Add(l1);

        root.Update();

        Assert.Equal(3, root.TotalFolderCount);
        Assert.Equal(2, l1.TotalFolderCount);
        Assert.Equal(1, l2.TotalFolderCount);
        Assert.Equal(0, l3.TotalFolderCount);
    }
}
