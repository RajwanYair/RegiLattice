// RegiLattice.Core.Tests — ComplianceHistoryTests.cs
// Unit tests for ComplianceHistory.
// ComplianceHistory persists ComplianceReport results to a rolling JSON log,
// capped at MaxEntries (90). Tests use file-system isolation: the history file
// is deleted before and after each test to prevent cross-test contamination.

using RegiLattice.Core;
using RegiLattice.Core.Models;
using Xunit;

namespace RegiLattice.Core.Tests;

[Collection("ComplianceHistory")]
public sealed class ComplianceHistoryTests : IDisposable
{
    // ── Isolation helpers ───────────────────────────────────────────────────

    public ComplianceHistoryTests()
    {
        DeleteFile();
    }

    public void Dispose()
    {
        DeleteFile();
    }

    private static void DeleteFile()
    {
        string path = ComplianceHistory.HistoryPath;
        if (File.Exists(path))
            File.Delete(path);
    }

    private static ComplianceReport MakeReport(int totalChecked, int violationCount = 0)
    {
        var drifted = Enumerable
            .Range(0, violationCount)
            .Select(i => new ComplianceDrift
            {
                TweakId = $"test-tweak-{i}",
                Label = $"Test Tweak {i}",
                Category = "Test",
                BaselineStatus = TweakResult.Applied,
                CurrentStatus = TweakResult.NotApplied,
            })
            .ToList()
            .AsReadOnly();

        return new ComplianceReport
        {
            Drifted = (IReadOnlyList<ComplianceDrift>)drifted,
            TotalChecked = totalChecked,
            CheckedAt = DateTime.UtcNow,
        };
    }

    // ── MaxEntries constant ─────────────────────────────────────────────────

    [Fact]
    public void MaxEntries_IsNinety()
    {
        Assert.Equal(90, ComplianceHistory.MaxEntries);
    }

    // ── HistoryPath ─────────────────────────────────────────────────────────

    [Fact]
    public void HistoryPath_ContainsComplianceHistoryJson()
    {
        Assert.EndsWith("compliance-history.json", ComplianceHistory.HistoryPath);
    }

    // ── GetHistory — empty / missing file ──────────────────────────────────

    [Fact]
    public void GetHistory_MissingFile_ReturnsEmptyList()
    {
        var history = ComplianceHistory.GetHistory();
        Assert.Empty(history);
    }

    [Fact]
    public void GetHistory_CorruptFile_ReturnsEmptyList()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(ComplianceHistory.HistoryPath)!);
        File.WriteAllText(ComplianceHistory.HistoryPath, "{ this is not valid json {{{{");
        var history = ComplianceHistory.GetHistory();
        Assert.Empty(history);
    }

    // ── GetLatest ───────────────────────────────────────────────────────────

    [Fact]
    public void GetLatest_EmptyHistory_ReturnsNull()
    {
        Assert.Null(ComplianceHistory.GetLatest());
    }

    [Fact]
    public void GetLatest_AfterAddEntry_ReturnsLastEntry()
    {
        var reportA = MakeReport(10, 0);
        var reportB = MakeReport(20, 2);
        ComplianceHistory.AddEntry(reportA);
        ComplianceHistory.AddEntry(reportB);

        var latest = ComplianceHistory.GetLatest();

        Assert.NotNull(latest);
        Assert.Equal(20, latest.TotalChecked);
        Assert.Equal(2, latest.ViolationCount);
    }

    // ── AddEntry ────────────────────────────────────────────────────────────

    [Fact]
    public void AddEntry_SingleEntry_PersistsFile()
    {
        ComplianceHistory.AddEntry(MakeReport(5));
        Assert.True(File.Exists(ComplianceHistory.HistoryPath));
    }

    [Fact]
    public void AddEntry_MultipleEntries_AllPersisted()
    {
        for (int i = 0; i < 5; i++)
            ComplianceHistory.AddEntry(MakeReport(i + 1));

        var history = ComplianceHistory.GetHistory();
        Assert.Equal(5, history.Count);
    }

    [Fact]
    public void AddEntry_SetsSnapshotPath()
    {
        const string snap = @"C:\snapshots\snap.json";
        ComplianceHistory.AddEntry(MakeReport(10), snapshotPath: snap);
        var latest = ComplianceHistory.GetLatest();
        Assert.NotNull(latest);
        Assert.Equal(snap, latest.SnapshotPath);
    }

    [Fact]
    public void AddEntry_NullSnapshotPath_IsNullInEntry()
    {
        ComplianceHistory.AddEntry(MakeReport(10), snapshotPath: null);
        var latest = ComplianceHistory.GetLatest();
        Assert.NotNull(latest);
        Assert.Null(latest.SnapshotPath);
    }

    [Fact]
    public void AddEntry_DriftedTweakIds_MappedFromReport()
    {
        var report = MakeReport(10, violationCount: 3);
        ComplianceHistory.AddEntry(report);

        var latest = ComplianceHistory.GetLatest();
        Assert.NotNull(latest);
        Assert.Equal(3, latest.DriftedTweakIds.Count);
        Assert.Contains("test-tweak-0", latest.DriftedTweakIds);
        Assert.Contains("test-tweak-2", latest.DriftedTweakIds);
    }

    [Fact]
    public void AddEntry_CompliantReport_IsCompliantTrue()
    {
        ComplianceHistory.AddEntry(MakeReport(10, violationCount: 0));
        var latest = ComplianceHistory.GetLatest();
        Assert.NotNull(latest);
        Assert.True(latest.IsCompliant);
        Assert.Equal(0, latest.ViolationCount);
    }

    [Fact]
    public void AddEntry_ExceedsMaxEntries_TrimsOldestEntries()
    {
        // Fill exactly MaxEntries + 5.
        int over = ComplianceHistory.MaxEntries + 5;
        for (int i = 0; i < over; i++)
            ComplianceHistory.AddEntry(MakeReport(i + 1));

        var history = ComplianceHistory.GetHistory();
        Assert.Equal(ComplianceHistory.MaxEntries, history.Count);
        // The retained entries should be the LAST MaxEntries ones.
        Assert.Equal(6, history[0].TotalChecked); // trimmed oldest 5 (1..5), kept from 6
    }

    [Fact]
    public void AddEntry_NullReport_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => ComplianceHistory.AddEntry(null!));
    }

    // ── Clear ───────────────────────────────────────────────────────────────

    [Fact]
    public void Clear_AfterAddEntries_DeletesFile()
    {
        ComplianceHistory.AddEntry(MakeReport(5));
        Assert.True(File.Exists(ComplianceHistory.HistoryPath));

        ComplianceHistory.Clear();
        Assert.False(File.Exists(ComplianceHistory.HistoryPath));
    }

    [Fact]
    public void Clear_AfterClear_GetHistoryReturnsEmpty()
    {
        ComplianceHistory.AddEntry(MakeReport(5));
        ComplianceHistory.Clear();
        Assert.Empty(ComplianceHistory.GetHistory());
    }

    [Fact]
    public void Clear_OnMissingFile_DoesNotThrow()
    {
        var ex = Record.Exception(ComplianceHistory.Clear);
        Assert.Null(ex);
    }

    // ── TotalViolationsInLast ───────────────────────────────────────────────

    [Fact]
    public void TotalViolationsInLast_ReturnsCorrectSum()
    {
        ComplianceHistory.AddEntry(MakeReport(10, 3));
        ComplianceHistory.AddEntry(MakeReport(10, 0));
        ComplianceHistory.AddEntry(MakeReport(10, 5));

        int total = ComplianceHistory.TotalViolationsInLast(3);
        Assert.Equal(8, total);
    }

    [Fact]
    public void TotalViolationsInLast_CountLargerThanHistory_SumsAll()
    {
        ComplianceHistory.AddEntry(MakeReport(10, 2));
        ComplianceHistory.AddEntry(MakeReport(10, 4));

        int total = ComplianceHistory.TotalViolationsInLast(100);
        Assert.Equal(6, total);
    }

    [Fact]
    public void TotalViolationsInLast_EmptyHistory_ReturnsZero()
    {
        Assert.Equal(0, ComplianceHistory.TotalViolationsInLast(10));
    }

    [Fact]
    public void TotalViolationsInLast_PartialCount_SumsOnlyLastN()
    {
        // 5 entries: violations = 1, 2, 3, 4, 5
        for (int i = 1; i <= 5; i++)
            ComplianceHistory.AddEntry(MakeReport(10, i));

        // Last 3 entries have 3+4+5 = 12 violations
        int last3 = ComplianceHistory.TotalViolationsInLast(3);
        Assert.Equal(12, last3);
    }
}
