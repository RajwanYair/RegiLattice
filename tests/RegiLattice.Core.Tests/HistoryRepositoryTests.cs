// RegiLattice.Core.Tests — HistoryRepositoryTests.cs
// Tests for IHistoryRepository / JsonHistoryRepository (B.3).

using RegiLattice.Core;
using RegiLattice.Core.Models;
using RegiLattice.Core.Services;
using Xunit;

namespace RegiLattice.Core.Tests;

public sealed class HistoryRepositoryTests : IDisposable
{
    private readonly string _tempFile;
    private readonly JsonHistoryRepository _repo;

    public HistoryRepositoryTests()
    {
        _tempFile = Path.GetTempFileName();
        File.Delete(_tempFile); // start fresh
        _repo = new JsonHistoryRepository(_tempFile);
    }

    public void Dispose()
    {
        if (File.Exists(_tempFile))
            File.Delete(_tempFile);
    }

    // ── Interface assignment ─────────────────────────────────────────────────

    [Fact]
    public void JsonHistoryRepository_ImplementsInterface()
    {
        Assert.IsAssignableFrom<IHistoryRepository>(_repo);
    }

    // ── Count on empty ───────────────────────────────────────────────────────

    [Fact]
    public void Count_NoEntries_ReturnsZero()
    {
        Assert.Equal(0, _repo.Count);
    }

    // ── RecordApply / RecordRemove / RecordUpdate ─────────────────────────────

    [Fact]
    public void RecordApply_IncrementsCount()
    {
        _repo.RecordApply("priv-foo", TweakResult.Applied);
        Assert.Equal(1, _repo.Count);
    }

    [Fact]
    public void RecordRemove_IncrementsCount()
    {
        _repo.RecordRemove("priv-bar", TweakResult.NotApplied);
        Assert.Equal(1, _repo.Count);
    }

    [Fact]
    public void RecordUpdate_IncrementsCount()
    {
        _repo.RecordUpdate("priv-baz", TweakResult.Applied);
        Assert.Equal(1, _repo.Count);
    }

    // ── Recent ───────────────────────────────────────────────────────────────

    [Fact]
    public void Recent_NoEntries_ReturnsEmpty()
    {
        Assert.Empty(_repo.Recent());
    }

    [Fact]
    public void Recent_ReturnsMostRecentFirst()
    {
        _repo.RecordApply("a", TweakResult.Applied);
        _repo.RecordApply("b", TweakResult.Applied);
        _repo.RecordApply("c", TweakResult.Applied);
        var recent = _repo.Recent(2);
        Assert.Equal(2, recent.Count);
        Assert.Equal("c", recent[0].TweakId);
        Assert.Equal("b", recent[1].TweakId);
    }

    // ── ForTweak ─────────────────────────────────────────────────────────────

    [Fact]
    public void ForTweak_ReturnsOnlyMatchingEntries()
    {
        _repo.RecordApply("priv-x", TweakResult.Applied);
        _repo.RecordApply("perf-y", TweakResult.Applied);
        _repo.RecordRemove("priv-x", TweakResult.NotApplied);
        var entries = _repo.ForTweak("priv-x");
        Assert.Equal(2, entries.Count);
        Assert.All(entries, e => Assert.Equal("priv-x", e.TweakId));
    }

    [Fact]
    public void ForTweak_CaseInsensitive()
    {
        _repo.RecordApply("Priv-X", TweakResult.Applied);
        Assert.Single(_repo.ForTweak("priv-x"));
    }

    // ── Clear ────────────────────────────────────────────────────────────────

    [Fact]
    public void Clear_RemovesAllEntries()
    {
        _repo.RecordApply("a", TweakResult.Applied);
        _repo.RecordApply("b", TweakResult.Applied);
        _repo.Clear();
        Assert.Equal(0, _repo.Count);
    }

    // ── Flush + persistence ──────────────────────────────────────────────────

    [Fact]
    public void Flush_WritesToDisk()
    {
        _repo.RecordApply("priv-persist", TweakResult.Applied);
        _repo.Flush();
        Assert.True(File.Exists(_tempFile));
    }

    // ── MaxEntries cap ───────────────────────────────────────────────────────

    [Fact]
    public void Record_ExceedsMaxEntries_TrimsOldest()
    {
        // Budget: cheap string inserts; MaxEntries=500 so add 502 entries.
        // Should trim to exactly MaxEntries (500).
        for (int i = 0; i <= JsonHistoryRepository.MaxEntries + 1; i++)
            _repo.Record($"t{i}", "apply", "Applied");
        Assert.Equal(JsonHistoryRepository.MaxEntries, _repo.Count);
    }

    // ── Record stores action name ─────────────────────────────────────────────

    [Fact]
    public void RecordApply_StoresApplyActionName()
    {
        _repo.RecordApply("priv-telemetry", TweakResult.Applied);
        var entry = _repo.Recent(1).Single();
        Assert.Equal("apply", entry.Action);
    }

    [Fact]
    public void RecordRemove_StoresRemoveActionName()
    {
        _repo.RecordRemove("priv-telemetry", TweakResult.NotApplied);
        var entry = _repo.Recent(1).Single();
        Assert.Equal("remove", entry.Action);
    }
}
