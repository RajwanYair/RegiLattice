using RegiLattice.Core.Services;
using Xunit;

namespace RegiLattice.Core.Tests;

/// <summary>Tests for the Favorites service — add, remove, toggle, persist.</summary>
public sealed class FavoritesTests : IDisposable
{
    public FavoritesTests()
    {
        Favorites.Reset();
    }

    public void Dispose()
    {
        Favorites.Reset();
    }

    [Fact]
    public void Add_SingleId_IncreasesCount()
    {
        Favorites.Add("perf-disable-animations");
        Assert.Equal(1, Favorites.Count);
    }

    [Fact]
    public void Add_DuplicateId_DoesNotDoubleCount()
    {
        Favorites.Add("perf-disable-animations");
        Favorites.Add("perf-disable-animations");
        Assert.Equal(1, Favorites.Count);
    }

    [Fact]
    public void Add_NullOrEmpty_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Favorites.Add(""));
        Assert.Throws<ArgumentException>(() => Favorites.Add("   "));
    }

    [Fact]
    public void Remove_ExistingId_DecreasesCount()
    {
        Favorites.Add("perf-disable-animations");
        Favorites.Add("priv-disable-telemetry");
        Favorites.Remove("perf-disable-animations");
        Assert.Equal(1, Favorites.Count);
        Assert.False(Favorites.IsFavorite("perf-disable-animations"));
    }

    [Fact]
    public void Remove_NonExistentId_NoOp()
    {
        Favorites.Remove("nonexistent");
        Assert.Equal(0, Favorites.Count);
    }

    [Fact]
    public void Toggle_AddsWhenAbsent_RemovesWhenPresent()
    {
        var added = Favorites.Toggle("perf-disable-animations");
        Assert.True(added);
        Assert.True(Favorites.IsFavorite("perf-disable-animations"));

        var removed = Favorites.Toggle("perf-disable-animations");
        Assert.False(removed);
        Assert.False(Favorites.IsFavorite("perf-disable-animations"));
    }

    [Fact]
    public void IsFavorite_ReturnsTrueForAdded()
    {
        Favorites.Add("priv-disable-telemetry");
        Assert.True(Favorites.IsFavorite("priv-disable-telemetry"));
    }

    [Fact]
    public void IsFavorite_CaseInsensitive()
    {
        Favorites.Add("perf-disable-animations");
        Assert.True(Favorites.IsFavorite("PERF-DISABLE-ANIMATIONS"));
    }

    [Fact]
    public void All_ReturnsSortedList()
    {
        Favorites.Add("zzz-test");
        Favorites.Add("aaa-test");
        Favorites.Add("mmm-test");

        var all = Favorites.All();
        Assert.Equal(3, all.Count);
        Assert.Equal("aaa-test", all[0]);
        Assert.Equal("mmm-test", all[1]);
        Assert.Equal("zzz-test", all[2]);
    }

    [Fact]
    public void Clear_RemovesAll()
    {
        Favorites.Add("a");
        Favorites.Add("b");
        Favorites.Clear();
        Assert.Equal(0, Favorites.Count);
    }

    [Fact]
    public void FlushAndReload_PersistsToDisk()
    {
        Favorites.Add("perf-disable-animations");
        Favorites.Add("priv-disable-telemetry");
        Favorites.Flush();

        // Reset cache (simulates new process)
        Favorites.Reset();

        // Note: Reset deletes file, so we re-add and flush to verify the cycle
        Favorites.Add("test-roundtrip");
        Favorites.Flush();

        // Force reload from disk
        var field = typeof(Favorites).GetField("_cache", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        field?.SetValue(null, null);

        Assert.True(Favorites.IsFavorite("test-roundtrip"));
    }

    [Fact]
    public void Count_Initially_IsZero()
    {
        Assert.Equal(0, Favorites.Count);
    }

    [Fact]
    public void All_Initially_IsEmpty()
    {
        Assert.Empty(Favorites.All());
    }

    [Fact]
    public void IsFavorite_Initially_ReturnsFalse()
    {
        Assert.False(Favorites.IsFavorite("anything"));
    }

    [Fact]
    public void Flush_WhenNotDirty_DoesNotThrow()
    {
        // Call flush on clean state — should not throw
        var dirty = typeof(Favorites).GetField("_dirty", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        dirty?.SetValue(null, false);
        Favorites.Flush(); // no-op
        Assert.Equal(0, Favorites.Count);
    }

    [Fact]
    public void Remove_Then_IsFavorite_ReturnsFalse()
    {
        Favorites.Add("to-remove");
        Favorites.Remove("to-remove");
        Assert.False(Favorites.IsFavorite("to-remove"));
    }

    [Fact]
    public void Add_Multiple_AllListedAndSorted()
    {
        Favorites.Add("z-last");
        Favorites.Add("a-first");
        Favorites.Add("m-middle");
        var all = Favorites.All();
        Assert.Equal(3, all.Count);
        Assert.Equal("a-first", all[0]);
        Assert.Equal("z-last", all[2]);
    }

    [Fact]
    public void Clear_Then_Add_Works()
    {
        Favorites.Add("before-clear");
        Favorites.Clear();
        Assert.Equal(0, Favorites.Count);
        Favorites.Add("after-clear");
        Assert.Equal(1, Favorites.Count);
        Assert.True(Favorites.IsFavorite("after-clear"));
    }

    [Fact]
    public void Toggle_CaseInsensitive_BypassesCaseDifference()
    {
        Favorites.Add("UPPER-ID");
        // Toggle with lowercase — should remove (because cache is case-insensitive)
        var result = Favorites.Toggle("upper-id");
        Assert.False(result);
        Assert.False(Favorites.IsFavorite("UPPER-ID"));
    }

    // ── Sprint 47 enhancement tests ───────────────────────────────

    [Fact]
    public async Task ExportToJsonAsync_WritesJsonArrayOfIds()
    {
        Favorites.Add("perf-test-1");
        Favorites.Add("priv-test-2");

        var path = Path.Combine(Path.GetTempPath(), $"rl-fav-{Guid.NewGuid()}.json");
        try
        {
            await Favorites.ExportToJsonAsync(path);
            Assert.True(File.Exists(path));
            var json = await File.ReadAllTextAsync(path);
            Assert.Contains("perf-test-1", json);
            Assert.Contains("priv-test-2", json);
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    [Fact]
    public async Task ImportFromJson_MergesNewIds_ReturnsAddedCount()
    {
        Favorites.Add("existing-fav");

        var path = Path.Combine(Path.GetTempPath(), $"rl-fav-imp-{Guid.NewGuid()}.json");
        try
        {
            await File.WriteAllTextAsync(path, "[\"existing-fav\",\"new-fav-1\",\"new-fav-2\"]");
            int added = Favorites.ImportFromJson(path);

            Assert.Equal(2, added); // existing-fav not counted
            Assert.True(Favorites.IsFavorite("new-fav-1"));
            Assert.True(Favorites.IsFavorite("new-fav-2"));
            Assert.Equal(3, Favorites.Count);
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    [Fact]
    public void ImportFromJson_MissingFile_ThrowsFileNotFound()
    {
        Assert.Throws<FileNotFoundException>(() => Favorites.ImportFromJson(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".json")));
    }
}
