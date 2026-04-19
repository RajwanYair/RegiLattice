// tests/RegiLattice.Core.Tests/ComplianceServiceTests.cs
// ComplianceService tests.

#nullable enable

using RegiLattice.Core;
using RegiLattice.Core.Models;
using Xunit;

namespace RegiLattice.Core.Tests;

/// <summary>ComplianceService check logic tests.</summary>
[Collection("Builtins")]
public sealed class ComplianceServiceTests
{
    private readonly TweakEngine _engine;

    public ComplianceServiceTests(BuiltinsFixture fixture)
    {
        _engine = fixture.Engine;
    }

    // helper so existing test bodies need no changes
    private TweakEngine BuildEngine() => _engine;

    // ── Argument validation ───────────────────────────────────────────────

    [Fact]
    public void Check_NullEngine_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => ComplianceService.Check(null!, new Dictionary<string, string>()));
    }

    [Fact]
    public void Check_NullBaseline_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => ComplianceService.Check(BuildEngine(), null!));
    }

    [Fact]
    public void CheckFromFile_NullEngine_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => ComplianceService.CheckFromFile(null!, "some-file.json"));
    }

    [Fact]
    public void CheckFromFile_NullPath_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => ComplianceService.CheckFromFile(BuildEngine(), null!));
    }

    [Fact]
    public void CheckFromFile_EmptyPath_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => ComplianceService.CheckFromFile(BuildEngine(), string.Empty));
    }

    // ── Empty baseline produces compliant report ──────────────────────────

    [Fact]
    public void Check_EmptyBaseline_IsCompliant()
    {
        var engine = BuildEngine();
        var report = ComplianceService.Check(engine, new Dictionary<string, string>());

        Assert.True(report.IsCompliant);
        Assert.Equal(0, report.TotalChecked);
        Assert.Empty(report.Drifted);
    }

    // ── Baseline with "notapplied" only produces compliant report ─────────

    [Fact]
    public void Check_AllBaselineNotApplied_IsCompliant()
    {
        var engine = BuildEngine();
        var tweaks = engine.AllTweaks();
        var baseline = tweaks.Take(5).ToDictionary(t => t.Id, _ => "notapplied");

        var report = ComplianceService.Check(engine, baseline);

        // "notapplied" items in baseline are not tracked as violations.
        Assert.True(report.IsCompliant);
        Assert.Equal(0, report.TotalChecked);
    }

    // ── ComplianceReport properties ───────────────────────────────────────

    [Fact]
    public void Check_CheckedAt_IsRecentUtcTimestamp()
    {
        var engine = BuildEngine();
        var before = DateTime.UtcNow.AddSeconds(-2);
        var report = ComplianceService.Check(engine, new Dictionary<string, string>());
        var after = DateTime.UtcNow.AddSeconds(2);

        Assert.True(report.CheckedAt >= before && report.CheckedAt <= after, "CheckedAt should be a recent UTC timestamp.");
    }

    // ── ComplianceDrift record ────────────────────────────────────────────

    [Fact]
    public void ComplianceDrift_IsViolation_TrueWhenBaselineAppliedCurrentIsNot()
    {
        var drift = new ComplianceDrift
        {
            TweakId = "test-id",
            Label = "Test",
            Category = "Test",
            BaselineStatus = TweakResult.Applied,
            CurrentStatus = TweakResult.NotApplied,
        };

        Assert.True(drift.IsViolation);
    }

    [Fact]
    public void ComplianceDrift_IsViolation_FalseWhenCurrentMatches()
    {
        var drift = new ComplianceDrift
        {
            TweakId = "test-id",
            Label = "Test",
            Category = "Test",
            BaselineStatus = TweakResult.Applied,
            CurrentStatus = TweakResult.Applied,
        };

        Assert.False(drift.IsViolation);
    }

    // ── ViolationCount ────────────────────────────────────────────────────

    [Fact]
    public void ComplianceReport_ViolationCount_CountsOnlyIsViolationDrift()
    {
        var drifted = new List<ComplianceDrift>
        {
            new()
            {
                TweakId = "a",
                Label = "A",
                Category = "C",
                BaselineStatus = TweakResult.Applied,
                CurrentStatus = TweakResult.NotApplied,
            },
            new()
            {
                TweakId = "b",
                Label = "B",
                Category = "C",
                BaselineStatus = TweakResult.Applied,
                CurrentStatus = TweakResult.Unknown,
            },
            new()
            {
                TweakId = "c",
                Label = "C",
                Category = "C",
                BaselineStatus = TweakResult.Applied,
                CurrentStatus = TweakResult.Applied,
            }, // not a violation
        };

        var report = new ComplianceReport
        {
            Drifted = drifted.AsReadOnly(),
            TotalChecked = 3,
            CheckedAt = DateTime.UtcNow,
        };

        Assert.Equal(2, report.ViolationCount);
    }

    // ── CheckFromFile with missing file ───────────────────────────────────

    [Fact]
    public void CheckFromFile_MissingFile_ReturnsTotalCheckedMinusOne()
    {
        var engine = BuildEngine();
        var report = ComplianceService.CheckFromFile(engine, @"C:\does-not-exist-12345\snap.json");

        Assert.Equal(-1, report.TotalChecked);
        Assert.Empty(report.Drifted);
    }

    // ── Round-trip via temp file ──────────────────────────────────────────

    [Fact]
    public void CheckFromFile_ValidSnapshotFile_ReturnsReport()
    {
        var engine = BuildEngine();
        var snapshotPath = Path.Combine(Path.GetTempPath(), $"rl-snap-{Guid.NewGuid():N}.json");

        try
        {
            // Create a snapshot with first 3 tweaks marked as "applied".
            var tweaks = engine.AllTweaks().Take(3).ToList();
            var dict = tweaks.ToDictionary(t => t.Id, _ => "applied");
            var json = System.Text.Json.JsonSerializer.Serialize(dict);
            File.WriteAllText(snapshotPath, json);

            var report = ComplianceService.CheckFromFile(engine, snapshotPath);

            // TotalChecked should equal the number of applied-in-baseline tweaks.
            Assert.Equal(3, report.TotalChecked);
            Assert.True(report.CheckedAt > DateTime.MinValue);
        }
        finally
        {
            if (File.Exists(snapshotPath))
                File.Delete(snapshotPath);
        }
    }
}
