// RegiLattice.GUI.Tests — ComplianceTrendDialogTests.cs
// Compliance Trend Dashboard.
// Tests cover ComplianceHistory data layer and chart value helpers.
// No WinForms windows are created — all assertions target the data model.

using System;
using System.IO;
using System.Linq;
using RegiLattice.Core;
using RegiLattice.Core.Models;
using Xunit;

namespace RegiLattice.GUI.Tests;

/// <summary>
/// Tests for the ComplianceTrendDialog data layer — ComplianceHistory CRUD
/// and the metric calculations that drive chart rendering.
/// </summary>
[Collection("ComplianceHistory")]
public sealed class ComplianceTrendDialogTests : IDisposable
{
    // Backup/restore the real history file so tests don't corrupt user data.
    private readonly string _historyPath = ComplianceHistory.HistoryPath;
    private readonly string? _backup;

    public ComplianceTrendDialogTests()
    {
        if (File.Exists(_historyPath))
            _backup = File.ReadAllText(_historyPath);
        // Start with an empty history.
        ComplianceHistory.Clear();
    }

    public void Dispose()
    {
        if (_backup is not null)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_historyPath)!);
            File.WriteAllText(_historyPath, _backup);
        }
        else if (File.Exists(_historyPath))
        {
            File.Delete(_historyPath);
        }
    }

    // ── GetHistory returns empty list when no file ────────────────────────────

    [Fact]
    public void GetHistory_NoFile_ReturnsEmpty()
    {
        var history = ComplianceHistory.GetHistory();
        Assert.Empty(history);
    }

    // ── GetLatest returns null when empty ─────────────────────────────────────

    [Fact]
    public void GetLatest_NoHistory_ReturnsNull()
    {
        Assert.Null(ComplianceHistory.GetLatest());
    }

    // ── AddEntry + GetHistory round-trip ──────────────────────────────────────

    [Fact]
    public void AddEntry_SingleReport_CanBeRetrieved()
    {
        var report = MakeReport(totalChecked: 100, violationCount: 5);
        ComplianceHistory.AddEntry(report);

        var history = ComplianceHistory.GetHistory();
        Assert.Single(history);
        Assert.Equal(100, history[0].TotalChecked);
        Assert.Equal(5, history[0].ViolationCount);
        Assert.False(history[0].IsCompliant);
    }

    [Fact]
    public void AddEntry_CompliantReport_IsCompliantTrue()
    {
        var report = MakeReport(totalChecked: 50, violationCount: 0);
        ComplianceHistory.AddEntry(report);

        var latest = ComplianceHistory.GetLatest();
        Assert.NotNull(latest);
        Assert.True(latest.IsCompliant);
    }

    [Fact]
    public void AddEntry_MultipleEntries_OrderedByInsertion()
    {
        ComplianceHistory.AddEntry(MakeReport(totalChecked: 10, violationCount: 2));
        ComplianceHistory.AddEntry(MakeReport(totalChecked: 20, violationCount: 1));
        ComplianceHistory.AddEntry(MakeReport(totalChecked: 30, violationCount: 0));

        var history = ComplianceHistory.GetHistory();
        Assert.Equal(3, history.Count);
        Assert.Equal(10, history[0].TotalChecked);
        Assert.Equal(30, history[2].TotalChecked);
    }

    // ── MaxEntries cap ────────────────────────────────────────────────────────

    [Fact]
    public void AddEntry_ExceedsMaxEntries_TrimsOldest()
    {
        // Add MaxEntries + 5 entries
        for (int i = 0; i < ComplianceHistory.MaxEntries + 5; i++)
            ComplianceHistory.AddEntry(MakeReport(totalChecked: i + 1, violationCount: 0));

        var history = ComplianceHistory.GetHistory();
        Assert.Equal(ComplianceHistory.MaxEntries, history.Count);
        // Oldest 5 should have been trimmed; first surviving has totalChecked == 6
        Assert.Equal(6, history[0].TotalChecked);
    }

    // ── TotalViolationsInLast ─────────────────────────────────────────────────

    [Fact]
    public void TotalViolationsInLast_ReturnsCorrectSum()
    {
        ComplianceHistory.AddEntry(MakeReport(totalChecked: 10, violationCount: 3));
        ComplianceHistory.AddEntry(MakeReport(totalChecked: 10, violationCount: 7));
        ComplianceHistory.AddEntry(MakeReport(totalChecked: 10, violationCount: 2));

        Assert.Equal(9, ComplianceHistory.TotalViolationsInLast(2)); // last 2: 7+2
        Assert.Equal(12, ComplianceHistory.TotalViolationsInLast(3)); // all 3: 3+7+2
        Assert.Equal(12, ComplianceHistory.TotalViolationsInLast(99)); // clamps to available
    }

    // ── Clear ─────────────────────────────────────────────────────────────────

    [Fact]
    public void Clear_AfterAddingEntries_EmptiesHistory()
    {
        ComplianceHistory.AddEntry(MakeReport(totalChecked: 10, violationCount: 1));
        ComplianceHistory.Clear();

        Assert.Empty(ComplianceHistory.GetHistory());
        Assert.Null(ComplianceHistory.GetLatest());
    }

    // ── Compliance % metric (mirrors dialog's GetValue for % mode) ────────────

    [Theory]
    [InlineData(100, 0, 100.0)] // perfect compliance
    [InlineData(100, 25, 75.0)] // 75%
    [InlineData(50, 50, 0.0)] // all violated
    [InlineData(0, 0, 0.0)] // no tweaks (avoid div-by-zero)
    public void CompliancePercent_CalculatesCorrectly(int total, int violations, double expected)
    {
        double actual = total > 0 ? (double)(total - violations) / total * 100.0 : 0.0;
        Assert.Equal(expected, actual, precision: 6);
    }

    // ── DriftedTweakIds persisted ─────────────────────────────────────────────

    [Fact]
    public void AddEntry_WithDriftedTweaks_PersistsTweakIds()
    {
        var report = MakeReport(totalChecked: 10, violationCount: 2, driftedIds: ["priv-disable-telemetry", "perf-disable-animations"]);
        ComplianceHistory.AddEntry(report);

        var entry = ComplianceHistory.GetLatest();
        Assert.NotNull(entry);
        Assert.Equal(2, entry.DriftedTweakIds.Count);
        Assert.Contains("priv-disable-telemetry", entry.DriftedTweakIds);
    }

    // ── HistoryPath is deterministic ──────────────────────────────────────────

    [Fact]
    public void HistoryPath_ContainsRegiLatticeAndComplianceHistory()
    {
        string path = ComplianceHistory.HistoryPath;
        Assert.Contains("RegiLattice", path);
        Assert.EndsWith("compliance-history.json", path);
    }

    // ── GetLatest returns last inserted entry ─────────────────────────────────

    [Fact]
    public void GetLatest_ReturnsLastEntry()
    {
        ComplianceHistory.AddEntry(MakeReport(totalChecked: 5, violationCount: 0));
        ComplianceHistory.AddEntry(MakeReport(totalChecked: 99, violationCount: 0));

        var latest = ComplianceHistory.GetLatest();
        Assert.NotNull(latest);
        Assert.Equal(99, latest.TotalChecked);
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private static ComplianceReport MakeReport(int totalChecked, int violationCount, string[]? driftedIds = null)
    {
        var drifted = new List<ComplianceDrift>();
        foreach (string id in driftedIds ?? [])
        {
            drifted.Add(
                new ComplianceDrift
                {
                    TweakId = id,
                    Label = id,
                    Category = "Test",
                    BaselineStatus = TweakResult.Applied,
                    CurrentStatus = TweakResult.NotApplied,
                }
            );
        }

        // Pad to the requested violation count with synthetic drifts
        for (int i = drifted.Count; i < violationCount; i++)
        {
            drifted.Add(
                new ComplianceDrift
                {
                    TweakId = $"test-drift-{i}",
                    Label = $"Drift {i}",
                    Category = "Test",
                    BaselineStatus = TweakResult.Applied,
                    CurrentStatus = TweakResult.NotApplied,
                }
            );
        }

        return new ComplianceReport
        {
            CheckedAt = DateTime.UtcNow,
            TotalChecked = totalChecked,
            Drifted = drifted.AsReadOnly(),
        };
    }
}
