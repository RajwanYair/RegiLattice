using RegiLattice.Core.Models;
using RegiLattice.Core.Services;
using Xunit;

namespace RegiLattice.Core.Tests;

/// <summary>Tests for the TweakHistory service — record, retrieve, rolling limit.</summary>
public sealed class TweakHistoryTests : IDisposable
{
    public TweakHistoryTests()
    {
        TweakHistory.Reset();
    }

    public void Dispose()
    {
        TweakHistory.Reset();
    }

    [Fact]
    public void Record_SingleEntry_IncreasesCount()
    {
        TweakHistory.Record("perf-disable-animations", "apply", "Applied");
        Assert.Equal(1, TweakHistory.Count);
    }

    [Fact]
    public void RecordApply_CreatesEntry()
    {
        TweakHistory.RecordApply("perf-disable-animations", TweakResult.Applied);
        var all = TweakHistory.All();
        Assert.Single(all);
        Assert.Equal("perf-disable-animations", all[0].TweakId);
        Assert.Equal("apply", all[0].Action);
        Assert.Equal("Applied", all[0].Result);
    }

    [Fact]
    public void RecordRemove_CreatesEntry()
    {
        TweakHistory.RecordRemove("priv-disable-telemetry", TweakResult.NotApplied);
        var all = TweakHistory.All();
        Assert.Single(all);
        Assert.Equal("remove", all[0].Action);
    }

    [Fact]
    public void RecordUpdate_CreatesEntry()
    {
        TweakHistory.RecordUpdate("pkg-install-git", TweakResult.Applied);
        var all = TweakHistory.All();
        Assert.Single(all);
        Assert.Equal("update", all[0].Action);
    }

    [Fact]
    public void All_ReturnsOldestFirst()
    {
        TweakHistory.Record("first", "apply", "Applied");
        TweakHistory.Record("second", "apply", "Applied");
        TweakHistory.Record("third", "apply", "Applied");

        var all = TweakHistory.All();
        Assert.Equal("first", all[0].TweakId);
        Assert.Equal("third", all[2].TweakId);
    }

    [Fact]
    public void Recent_ReturnsNewestFirst()
    {
        TweakHistory.Record("first", "apply", "Applied");
        TweakHistory.Record("second", "apply", "Applied");
        TweakHistory.Record("third", "apply", "Applied");

        var recent = TweakHistory.Recent(2);
        Assert.Equal(2, recent.Count);
        Assert.Equal("third", recent[0].TweakId);
        Assert.Equal("second", recent[1].TweakId);
    }

    [Fact]
    public void ForTweak_FiltersCorrectly()
    {
        TweakHistory.Record("tweak-a", "apply", "Applied");
        TweakHistory.Record("tweak-b", "apply", "Applied");
        TweakHistory.Record("tweak-a", "remove", "NotApplied");

        var forA = TweakHistory.ForTweak("tweak-a");
        Assert.Equal(2, forA.Count);
    }

    [Fact]
    public void ForTweak_CaseInsensitive()
    {
        TweakHistory.Record("Tweak-A", "apply", "Applied");
        var result = TweakHistory.ForTweak("tweak-a");
        Assert.Single(result);
    }

    [Fact]
    public void MaxEntries_TrimsOldest()
    {
        for (int i = 0; i < TweakHistory.MaxEntries + 50; i++)
            TweakHistory.Record($"tweak-{i}", "apply", "Applied");

        Assert.Equal(TweakHistory.MaxEntries, TweakHistory.Count);
        // Oldest entries should have been trimmed
        var all = TweakHistory.All();
        Assert.Equal("tweak-50", all[0].TweakId);
    }

    [Fact]
    public void Clear_RemovesAllEntries()
    {
        TweakHistory.Record("a", "apply", "Applied");
        TweakHistory.Record("b", "apply", "Applied");
        TweakHistory.Clear();
        Assert.Equal(0, TweakHistory.Count);
    }

    [Fact]
    public void Timestamp_IsIso8601()
    {
        TweakHistory.Record("test", "apply", "Applied");
        var entry = TweakHistory.All()[0];
        Assert.True(DateTimeOffset.TryParse(entry.Timestamp, out _), $"Timestamp not ISO 8601: {entry.Timestamp}");
    }

    [Fact]
    public void FlushAndReload_PersistsToDisk()
    {
        TweakHistory.Record("persist-test", "apply", "Applied");
        TweakHistory.Flush();

        // Force reload from disk by resetting cache only
        var field = typeof(TweakHistory).GetField("_cache", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        field?.SetValue(null, null);
        var dirtyField = typeof(TweakHistory).GetField("_dirty", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        dirtyField?.SetValue(null, false);

        var all = TweakHistory.All();
        Assert.Single(all);
        Assert.Equal("persist-test", all[0].TweakId);
    }

    [Fact]
    public void Count_Initially_IsZero()
    {
        Assert.Equal(0, TweakHistory.Count);
    }

    [Fact]
    public void All_Initially_IsEmpty()
    {
        Assert.Empty(TweakHistory.All());
    }

    [Fact]
    public void Recent_WhenFewerRecordsThanRequested_ReturnsAll()
    {
        TweakHistory.Record("a", "apply", "Applied");
        TweakHistory.Record("b", "apply", "Applied");
        var recent = TweakHistory.Recent(100);
        Assert.Equal(2, recent.Count);
    }

    [Fact]
    public void Recent_ZeroCount_ReturnsEmpty()
    {
        TweakHistory.Record("a", "apply", "Applied");
        var recent = TweakHistory.Recent(0);
        Assert.Empty(recent);
    }

    [Fact]
    public void ForTweak_NoMatch_ReturnsEmpty()
    {
        TweakHistory.Record("tweak-x", "apply", "Applied");
        var result = TweakHistory.ForTweak("completely-different");
        Assert.Empty(result);
    }

    [Fact]
    public void Flush_WhenNotDirty_DoesNotThrow()
    {
        // Ensure flush when _dirty is false does not throw
        var dirty = typeof(TweakHistory).GetField("_dirty", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        dirty?.SetValue(null, false);
        TweakHistory.Flush(); // should be a no-op
        Assert.Equal(0, TweakHistory.Count);
    }

    [Fact]
    public void Record_HasNonEmptyTimestamp()
    {
        TweakHistory.Record("stamp-test", "apply", "Applied");
        var entry = TweakHistory.All()[0];
        Assert.False(string.IsNullOrEmpty(entry.Timestamp));
    }

    [Fact]
    public void MaxEntries_ExactlyAtLimit_NoTrimOccurs()
    {
        for (int i = 0; i < TweakHistory.MaxEntries; i++)
            TweakHistory.Record($"exact-{i}", "apply", "Applied");
        Assert.Equal(TweakHistory.MaxEntries, TweakHistory.Count);
    }

    [Fact]
    public void Clear_Then_Count_IsZero()
    {
        TweakHistory.Record("a", "apply", "Applied");
        TweakHistory.Record("b", "apply", "Applied");
        TweakHistory.Clear();
        Assert.Equal(0, TweakHistory.Count);
    }

    [Fact]
    public void Recent_DefaultCountTwenty_ReturnsAtMostTwenty()
    {
        for (int i = 0; i < 30; i++)
            TweakHistory.Record($"rec-{i}", "apply", "Applied");
        var recent = TweakHistory.Recent(); // default = 20
        Assert.Equal(20, recent.Count);
    }
}
