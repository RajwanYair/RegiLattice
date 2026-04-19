#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Win32;
using RegiLattice.Core;
using RegiLattice.Core.Models;
using RegiLattice.Core.Plugins;
using RegiLattice.Core.Registry;
using RegiLattice.Core.Services;
using Xunit;

namespace RegiLattice.Core.Tests;
public sealed class ComplianceHistoryNullJsonBranchTests : IDisposable
{
    private readonly string _historyPath = ComplianceHistory.HistoryPath;
    private readonly string? _backup;

    public ComplianceHistoryNullJsonBranchTests()
    {
        if (File.Exists(_historyPath))
            _backup = File.ReadAllText(_historyPath);

        Directory.CreateDirectory(Path.GetDirectoryName(_historyPath)!);
        File.WriteAllText(_historyPath, "null");
    }

    public void Dispose()
    {
        if (_backup is not null)
            File.WriteAllText(_historyPath, _backup);
        else if (File.Exists(_historyPath))
            File.Delete(_historyPath);
    }

    [Fact]
    public void GetHistory_NullJsonFile_ReturnsEmpty()
    {
        var history = ComplianceHistory.GetHistory();
        Assert.Empty(history);
    }
}

// ── 4. Ratings — AllRatings when file exists (L41 F-branch) ──────────────────
//    Ratings.cs L41: `if (!File.Exists(FilePath))`
//    Rate() calls AllRatings() internally with no file (T-branch), then saves the file.
//    This test explicitly calls AllRatings() AFTER Rate() so the file exists → F-branch.

[Collection("Builtins")]
public sealed class ComplianceDriftAdditionalTests
{
    private readonly TweakEngine _engine;

    public ComplianceDriftAdditionalTests(BuiltinsFixture fixture) => _engine = fixture.Engine;

    // ── IsViolation when BaselineStatus != Applied (uncovered branch) ────

    [Fact]
    public void ComplianceDrift_IsViolation_FalseWhenBaselineIsNotApplied()
    {
        // BaselineStatus = NotApplied → IsViolation = false (first condition fails)
        var drift = new ComplianceDrift
        {
            TweakId = "x",
            Label = "X",
            Category = "C",
            BaselineStatus = TweakResult.NotApplied,
            CurrentStatus = TweakResult.NotApplied,
        };
        Assert.False(drift.IsViolation);
    }

    [Fact]
    public void ComplianceDrift_IsViolation_FalseWhenBaselineIsUnknown()
    {
        var drift = new ComplianceDrift
        {
            TweakId = "x",
            Label = "X",
            Category = "C",
            BaselineStatus = TweakResult.Unknown,
            CurrentStatus = TweakResult.NotApplied,
        };
        Assert.False(drift.IsViolation);
    }

    // ── ComplianceReport.ViolationCount = 0 when Drifted is empty ────────

    [Fact]
    public void ComplianceReport_ViolationCount_ZeroWhenNoDrift()
    {
        var report = new ComplianceReport
        {
            Drifted = [],
            TotalChecked = 0,
            CheckedAt = DateTime.UtcNow,
        };
        Assert.Equal(0, report.ViolationCount);
        Assert.True(report.IsCompliant);
    }

    // ── ComplianceService.Check: baseline id missing from engine ─────────

    [Fact]
    public void ComplianceService_Check_BaselineIdNotInEngine_SkipsGracefully()
    {
        // Build a small engine with no tweaks matching the baseline ids
        var engine = new TweakEngine();
        engine.Register([
            new TweakDef
            {
                Id = "dummy-tweak",
                Label = "Dummy",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\RegiLattice\Test", "Dummy", 1)],
            },
        ]);

        // Baseline contains an id that's NOT in the engine → `if (td is null) continue`
        var baseline = new Dictionary<string, string> { ["nonexistent-id"] = "applied" };
        var report = ComplianceService.Check(engine, baseline);

        // The non-existent id should be skipped, result should be empty or minimal
        Assert.NotNull(report);
    }

    // ── ComplianceService.Check: case-insensitive "APPLIED" recognition ──

    [Fact]
    public void ComplianceService_Check_BaselineWithUppercaseApplied_IsRecognized()
    {
        var engine = new TweakEngine();
        engine.Register([
            new TweakDef
            {
                Id = "ci-test-tweak",
                Label = "CI Test",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\RegiLattice\CITest", "Val", 1)],
            },
        ]);

        // "APPLIED" (uppercase) should be recognised as the applied state
        var baseline = new Dictionary<string, string> { ["ci-test-tweak"] = "APPLIED" };
        var report = ComplianceService.Check(engine, baseline);

        // TotalChecked = 1 (one baseline-applied tweak was found)
        Assert.Equal(1, report.TotalChecked);
    }

    // ── ComplianceService.CheckFromFile: non-existent file ───────────────

    [Fact]
    public void ComplianceService_CheckFromFile_FileNotFound_ReturnsSentinelReport()
    {
        var report = ComplianceService.CheckFromFile(_engine, @"C:\nonexistent\path\snap.json");
        // The method returns a report with TotalChecked = -1 when file can't be loaded
        Assert.Equal(-1, report.TotalChecked);
    }
}

// ═══════════════════════════════════════════════════════════════════════════
// 5.  ServiceManager async export / SetStartType uncovered paths
// ═══════════════════════════════════════════════════════════════════════════

