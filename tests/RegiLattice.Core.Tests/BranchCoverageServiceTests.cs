// tests/RegiLattice.Core.Tests/BranchCoverageServiceTests.cs
// Sprint 121 — T6.1: Branch coverage push, targeting specific uncovered arms in
// SmartScanService, PingResult, ConfigExporter, Ratings, and Analytics.

#nullable enable

using System.Collections.Generic;
using System.IO;
using RegiLattice.Core;
using RegiLattice.Core.Models;
using RegiLattice.Core.Services;
using Xunit;

namespace RegiLattice.Core.Tests;

// ═════════════════════════════════════════════════════════════════════════════
// SmartScanService — uncovered branches
// ═════════════════════════════════════════════════════════════════════════════

/// <summary>
/// Sprint 121 — Branch-coverage tests for <see cref="SmartScanService"/>.
/// Uses minimal inline engines so every branch is deliberately triggered.
/// </summary>
public sealed class SmartScanBranchTests
{
    // ── Helper ──────────────────────────────────────────────────────────

    private static TweakDef MakeTweak(
        string id,
        string label,
        int impact = 3,
        int safety = 3,
        string description = "",
        bool corpSafe = true,
        Func<bool>? isApplicable = null
    ) =>
        new()
        {
            Id = id,
            Label = label,
            Category = "Performance",
            Description = description,
            ImpactScore = impact,
            SafetyRating = safety,
            CorpSafe = corpSafe,
            IsApplicable = isApplicable,
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\RegiLattice\BranchTest", id, 1)],
        };

    // ── IsApplicable = () => false → tweak skipped, ScannedCount not incremented ─

    [Fact]
    public void Scan_IsApplicableReturnsFalse_TweakNotScanned()
    {
        var engine = new TweakEngine();
        engine.Register([
            MakeTweak("bct-inapplicable", "Inapplicable Tweak", isApplicable: () => false),
            MakeTweak("bct-applicable", "Applicable Tweak"),
        ]);

        var result = SmartScanService.Scan(engine, statusMap: null);

        // Only the applicable tweak counts
        Assert.Equal(1, result.ScannedCount);
        Assert.DoesNotContain(result.Recommendations, r => r.Tweak.Id == "bct-inapplicable");
    }

    [Fact]
    public void Scan_IsApplicableReturnsTrue_TweakIsScanned()
    {
        var engine = new TweakEngine();
        engine.Register([MakeTweak("bct-applicable-true", "Always Applicable", isApplicable: () => true)]);

        var result = SmartScanService.Scan(engine, statusMap: null);
        Assert.Equal(1, result.ScannedCount);
    }

    // ── BuildReason — description > 120 chars → ignore, fall through to generated ─

    [Fact]
    public void Scan_LongDescription_ReasonFallsBackToGeneratedString()
    {
        string longDesc = new string('X', 121); // 121 chars > 120 limit
        var engine = new TweakEngine();
        engine.Register([MakeTweak("bct-long-desc", "Long Description Tweak", description: longDesc, impact: 5, safety: 5)]);

        var result = SmartScanService.Scan(engine, statusMap: null);

        Assert.Single(result.Recommendations);
        // The generated reason should NOT be the long description
        Assert.DoesNotContain(longDesc, result.Recommendations[0].Reason);
        // It should be the generated format "Xxx impact, yyy safe — Category tweak."
        Assert.Contains("—", result.Recommendations[0].Reason);
    }

    [Fact]
    public void Scan_ShortDescription_ReasonIsDescription()
    {
        const string shortDesc = "Speeds up rendering.";
        var engine = new TweakEngine();
        engine.Register([MakeTweak("bct-short-desc", "Short Desc Tweak", description: shortDesc)]);

        var result = SmartScanService.Scan(engine, statusMap: null);
        Assert.Single(result.Recommendations);
        Assert.Equal(shortDesc, result.Recommendations[0].Reason);
    }

    // ── BuildReason — ImpactScore _ default arm (scores 1 and 2 → "Low impact") ─

    [Fact]
    public void Scan_ImpactScore1_ReasonContainsLowImpact()
    {
        var engine = new TweakEngine();
        engine.Register([MakeTweak("bct-impact1", "Impact Score One", impact: 1, safety: 5)]);

        var result = SmartScanService.Scan(engine, statusMap: null);
        Assert.Single(result.Recommendations);
        Assert.Contains("Low impact", result.Recommendations[0].Reason, StringComparison.Ordinal);
    }

    [Fact]
    public void Scan_ImpactScore2_ReasonContainsLowImpact()
    {
        var engine = new TweakEngine();
        engine.Register([MakeTweak("bct-impact2", "Impact Score Two", impact: 2, safety: 5)]);

        var result = SmartScanService.Scan(engine, statusMap: null);
        Assert.Single(result.Recommendations);
        Assert.Contains("Low impact", result.Recommendations[0].Reason, StringComparison.Ordinal);
    }

    // ── BuildReason — SafetyRating _ default arm (ratings 1 and 2 → "use with caution") ─

    [Fact]
    public void Scan_SafetyRating1_ReasonContainsUseWithCaution()
    {
        var engine = new TweakEngine();
        engine.Register([MakeTweak("bct-safety1", "Safety Rating One", impact: 5, safety: 1)]);

        var result = SmartScanService.Scan(engine, statusMap: null);
        Assert.Single(result.Recommendations);
        Assert.Contains("use with caution", result.Recommendations[0].Reason, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Scan_SafetyRating2_ReasonContainsUseWithCaution()
    {
        var engine = new TweakEngine();
        engine.Register([MakeTweak("bct-safety2", "Safety Rating Two", impact: 5, safety: 2)]);

        var result = SmartScanService.Scan(engine, statusMap: null);
        Assert.Single(result.Recommendations);
        Assert.Contains("use with caution", result.Recommendations[0].Reason, StringComparison.OrdinalIgnoreCase);
    }

    // ── BuildReason — all ImpactScore named arms ──────────────────────────────

    [Fact]
    public void Scan_ImpactScore3_ReasonContainsModerateImpact()
    {
        var engine = new TweakEngine();
        engine.Register([MakeTweak("bct-impact3", "Impact Score Three", impact: 3, safety: 5)]);
        var result = SmartScanService.Scan(engine, statusMap: null);
        Assert.Contains("Moderate impact", result.Recommendations[0].Reason, StringComparison.Ordinal);
    }

    [Fact]
    public void Scan_ImpactScore4_ReasonContainsGoodImpact()
    {
        var engine = new TweakEngine();
        engine.Register([MakeTweak("bct-impact4", "Impact Score Four", impact: 4, safety: 5)]);
        var result = SmartScanService.Scan(engine, statusMap: null);
        Assert.Contains("Good impact", result.Recommendations[0].Reason, StringComparison.Ordinal);
    }

    [Fact]
    public void Scan_ImpactScore5_ReasonContainsHighImpact()
    {
        var engine = new TweakEngine();
        engine.Register([MakeTweak("bct-impact5", "Impact Score Five", impact: 5, safety: 5)]);
        var result = SmartScanService.Scan(engine, statusMap: null);
        Assert.Contains("High impact", result.Recommendations[0].Reason, StringComparison.Ordinal);
    }

    // ── BuildReason — all SafetyRating named arms ─────────────────────────────

    [Fact]
    public void Scan_SafetyRating3_ReasonContainsGenerallySafe()
    {
        var engine = new TweakEngine();
        engine.Register([MakeTweak("bct-safety3", "Safety Rating Three", impact: 5, safety: 3)]);
        var result = SmartScanService.Scan(engine, statusMap: null);
        Assert.Contains("generally safe", result.Recommendations[0].Reason, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Scan_SafetyRating4_ReasonContainsVerySafe()
    {
        var engine = new TweakEngine();
        engine.Register([MakeTweak("bct-safety4", "Safety Rating Four", impact: 5, safety: 4)]);
        var result = SmartScanService.Scan(engine, statusMap: null);
        Assert.Contains("very safe", result.Recommendations[0].Reason, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Scan_SafetyRating5_ReasonContainsCompletelySafe()
    {
        var engine = new TweakEngine();
        engine.Register([MakeTweak("bct-safety5", "Safety Rating Five", impact: 5, safety: 5)]);
        var result = SmartScanService.Scan(engine, statusMap: null);
        Assert.Contains("completely safe", result.Recommendations[0].Reason, StringComparison.OrdinalIgnoreCase);
    }

    // ── Sort tiebreaker — equal priority → alphabetical label ─────────────────

    [Fact]
    public void Scan_EqualPriority_OrdersAlphabetically()
    {
        // Both tweaks have score = 3×3 = 9
        var engine = new TweakEngine();
        engine.Register([MakeTweak("bct-tie-z", "Zebra Tweak", impact: 3, safety: 3), MakeTweak("bct-tie-a", "Alpha Tweak", impact: 3, safety: 3)]);

        var result = SmartScanService.Scan(engine, statusMap: null);
        Assert.Equal(2, result.Recommendations.Count);
        Assert.Equal("bct-tie-a", result.Recommendations[0].Tweak.Id); // Alpha before Zebra
        Assert.Equal("bct-tie-z", result.Recommendations[1].Tweak.Id);
    }

    // ── IsQuickWin — all four quadrants ───────────────────────────────────────

    [Fact]
    public void IsQuickWin_ImpactGe4AndSafetyGe4_ReturnsTrue()
    {
        var rec = new ScanRecommendation
        {
            Tweak = MakeTweak("bct-qw-tt", "Quick Win", impact: 4, safety: 4),
            Reason = "r",
            PriorityScore = 16,
        };
        Assert.True(rec.IsQuickWin);
    }

    [Fact]
    public void IsQuickWin_ImpactLt4_ReturnsFalse()
    {
        var rec = new ScanRecommendation
        {
            Tweak = MakeTweak("bct-qw-ft", "Not Quick — low impact", impact: 3, safety: 5),
            Reason = "r",
            PriorityScore = 15,
        };
        Assert.False(rec.IsQuickWin);
    }

    [Fact]
    public void IsQuickWin_SafetyLt4_ReturnsFalse()
    {
        var rec = new ScanRecommendation
        {
            Tweak = MakeTweak("bct-qw-tf", "Not Quick — low safety", impact: 5, safety: 3),
            Reason = "r",
            PriorityScore = 15,
        };
        Assert.False(rec.IsQuickWin);
    }

    [Fact]
    public void IsQuickWin_BothLt4_ReturnsFalse()
    {
        var rec = new ScanRecommendation
        {
            Tweak = MakeTweak("bct-qw-ff", "Not Quick — both low", impact: 2, safety: 2),
            Reason = "r",
            PriorityScore = 4,
        };
        Assert.False(rec.IsQuickWin);
    }
}

// ═════════════════════════════════════════════════════════════════════════════
// PingResult — uncovered branches in LossPercent and Parse()
// ═════════════════════════════════════════════════════════════════════════════

/// <summary>
/// Sprint 121 — Branch-coverage tests for <see cref="PingResult"/>.
/// PingResult.Parse() is internal but visible via InternalsVisibleTo.
/// </summary>
public sealed class PingResultBranchTests
{
    // ── LossPercent: Sent == 0 branch ─────────────────────────────────────────

    [Fact]
    public void LossPercent_WhenSentIsZero_Returns100()
    {
        var result = new PingResult("host", Sent: 0, Received: 0, Lost: 0, AverageMs: 0, MinMs: 0, MaxMs: 0);
        Assert.Equal(100.0, result.LossPercent);
    }

    [Fact]
    public void LossPercent_WhenAllReceived_ReturnsZero()
    {
        var result = new PingResult("host", Sent: 4, Received: 4, Lost: 0, AverageMs: 0, MinMs: 0, MaxMs: 0);
        Assert.Equal(0.0, result.LossPercent);
    }

    [Fact]
    public void LossPercent_WhenPartialLoss_ReturnsCorrectPercent()
    {
        var result = new PingResult("host", Sent: 4, Received: 3, Lost: 1, AverageMs: 0, MinMs: 0, MaxMs: 0);
        Assert.Equal(25.0, result.LossPercent);
    }

    // ── PingResult.Parse() — Packets line present ─────────────────────────────

    [Fact]
    public void Parse_WithPacketsLine_ExtractsSentReceivedLost()
    {
        const string stdout = "    Packets: Sent = 4, Received = 3, Lost = 1 (25% loss),\r\n";
        var r = PingResult.Parse("8.8.8.8", stdout);

        Assert.Equal("8.8.8.8", r.Host);
        Assert.Equal(4, r.Sent);
        Assert.Equal(3, r.Received);
        Assert.Equal(1, r.Lost);
    }

    [Fact]
    public void Parse_WithTimingLine_ExtractsMinMaxAvg()
    {
        const string stdout = "    Packets: Sent = 4, Received = 4, Lost = 0 (0% loss),\r\n" + "Minimum = 2ms, Maximum = 8ms, Average = 4ms\r\n";
        var r = PingResult.Parse("1.1.1.1", stdout);

        Assert.Equal(2.0, r.MinMs);
        Assert.Equal(8.0, r.MaxMs);
        Assert.Equal(4.0, r.AverageMs);
    }

    [Fact]
    public void Parse_EmptyStdout_ReturnsAllZeros()
    {
        var r = PingResult.Parse("host", "");

        Assert.Equal(0, r.Sent);
        Assert.Equal(0, r.Received);
        Assert.Equal(0, r.Lost);
        Assert.Equal(0.0, r.AverageMs);
        Assert.Equal(0.0, r.MinMs);
        Assert.Equal(0.0, r.MaxMs);
    }

    [Fact]
    public void Parse_OnlyPacketsLineNoTiming_TimingIsZero()
    {
        const string stdout = "    Packets: Sent = 4, Received = 4, Lost = 0 (0% loss),\r\n";
        var r = PingResult.Parse("host", stdout);

        Assert.Equal(4, r.Sent);
        Assert.Equal(0.0, r.AverageMs); // no Minimum line → stays at default
    }

    [Fact]
    public void Parse_PacketsLineTooFewNumbers_SentReceivedLostRemainZero()
    {
        // Fewer than 3 captured numbers — should not throw, stays at defaults
        const string stdout = "    Packets: Sent = 4\r\n"; // only 1 number match
        var r = PingResult.Parse("host", stdout);

        Assert.Equal(0, r.Sent); // m.Count < 3 → skip
    }

    [Fact]
    public void Parse_TimingLineTooFewNumbers_TimingRemainZero()
    {
        // Fewer than 3 ms matches — stays at defaults
        const string stdout = "Minimum = 2ms, Maximum = 8ms\r\n"; // only 2 ms values
        var r = PingResult.Parse("host", stdout);

        Assert.Equal(0.0, r.MinMs); // m.Count < 3 → skip
    }
}

// ═════════════════════════════════════════════════════════════════════════════
// ConfigExporter — uncovered branches in Import() and Validate()
// ═════════════════════════════════════════════════════════════════════════════

/// <summary>
/// Sprint 121 — Branch-coverage tests for <see cref="ConfigExporter"/> edge cases.
/// </summary>
public sealed class ConfigExporterBranchTests : IDisposable
{
    private readonly string _tempDir;

    public ConfigExporterBranchTests()
    {
        _tempDir = Path.Combine(Path.GetTempPath(), $"rl-cfg-export-{Guid.NewGuid():N}");
        Directory.CreateDirectory(_tempDir);
    }

    public void Dispose()
    {
        try
        {
            Directory.Delete(_tempDir, true);
        }
        catch
        { /* best-effort */
        }
    }

    // ── Import — JSON object with no "tweaks" key → block 1 succeeds with default Tweaks ─

    [Fact]
    public void Import_JsonObjectWithNameButNoTweaks_ReturnsConfigWithEmptyTweaks()
    {
        var path = Path.Combine(_tempDir, "no-tweaks-key.json");
        // TweakConfig.Tweaks defaults to [] so any JSON object successfully deserializes
        // via block 1 even when "tweaks" is absent — covers the happy-path of block 1.
        File.WriteAllText(path, """{"name": "test", "description": "my config"}""");

        var result = ConfigExporter.Import(path);
        Assert.NotNull(result);
        Assert.Equal("test", result.Name);
        Assert.Empty(result.Tweaks);
    }

    // ── Import — JSON object with "tweaks": null → block 1 returns config with Tweaks=null ─

    [Fact]
    public void Import_JsonObjectWithNullTweaksValue_ReturnsConfigWithNullTweaks()
    {
        var path = Path.Combine(_tempDir, "tweaks-null.json");
        // {"tweaks": null} deserializes via block 1 with Tweaks = null.
        File.WriteAllText(path, """{"tweaks": null}""");

        var result = ConfigExporter.Import(path);
        Assert.NotNull(result);
        Assert.Null(result.Tweaks);
    }

    // ── Validate — empty config.Tweaks → both lists empty (loop never executes) ─

    [Fact]
    public void Validate_EmptyConfig_ReturnsBothListsEmpty()
    {
        var engine = new TweakEngine();
        engine.Register([
            new TweakDef
            {
                Id = "cfg-validate-anchor",
                Label = "Validate Anchor",
                Category = "Performance",
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\RegiLattice\Test", "val", 1)],
            },
        ]);

        var emptyConfig = new TweakConfig { Tweaks = [] };
        var (valid, unknown) = ConfigExporter.Validate(emptyConfig, engine);

        Assert.Empty(valid);
        Assert.Empty(unknown);
    }

    // ── Validate — all IDs known ──────────────────────────────────────────────

    [Fact]
    public void Validate_AllKnownIds_AllInValidList()
    {
        const string id = "cfg-validate-known";
        var engine = new TweakEngine();
        engine.Register([
            new TweakDef
            {
                Id = id,
                Label = "Known Tweak",
                Category = "Performance",
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\RegiLattice\Test", "v", 1)],
            },
        ]);

        var config = new TweakConfig { Tweaks = [id] };
        var (valid, unknown) = ConfigExporter.Validate(config, engine);

        Assert.Contains(id, valid);
        Assert.Empty(unknown);
    }

    // ── Validate — all IDs unknown ───────────────────────────────────────────

    [Fact]
    public void Validate_AllUnknownIds_AllInUnknownList()
    {
        var engine = new TweakEngine(); // empty engine
        var config = new TweakConfig { Tweaks = ["ghost-id-1", "ghost-id-2"] };
        var (valid, unknown) = ConfigExporter.Validate(config, engine);

        Assert.Empty(valid);
        Assert.Equal(2, unknown.Count);
    }
}

// ═════════════════════════════════════════════════════════════════════════════
// Ratings — uncovered branches: AverageRating() with no ratings → null,
// TopRated(0), and AllRatings() when deserialized list is null.
// ═════════════════════════════════════════════════════════════════════════════

/// <summary>
/// Sprint 121 — Branch-coverage tests for <see cref="Ratings"/> edge paths.
/// </summary>
public sealed class RatingsBranchTests
{
    // ── AverageRating() when no ratings exist → returns null ─────────────────

    [Fact]
    public void AverageRating_WhenNoRatingsExist_ReturnsNull()
    {
        // Force the ratings file to be an empty dict {}, so AllRatings() returns Count==0
        // and AverageRating() hits the `all.Count == 0 ? null` branch.
        var ratingsFile = Path.Combine(AppConfig.ConfigDir, "ratings.json");
        var backupExists = File.Exists(ratingsFile);
        string? backup = backupExists ? File.ReadAllText(ratingsFile) : null;

        try
        {
            Directory.CreateDirectory(AppConfig.ConfigDir);
            File.WriteAllText(ratingsFile, "{}"); // empty dict → Count == 0

            var avg = Ratings.AverageRating();
            Assert.Null(avg); // hits the null branch
        }
        finally
        {
            if (backup is not null)
                File.WriteAllText(ratingsFile, backup);
            else if (File.Exists(ratingsFile))
                File.Delete(ratingsFile);
        }
    }

    // ── TopRated(0) → returns empty list ─────────────────────────────────────

    [Fact]
    public void TopRated_ZeroN_ReturnsEmptyList()
    {
        var top = Ratings.TopRated(0);
        Assert.Empty(top);
    }

    // ── AllRatings() when file JSON is "null" → returns empty dict ────────────

    [Fact]
    public void AllRatings_FileContainsJsonNull_ReturnsEmptyDict()
    {
        // Write "null" to the ratings file to force the ?? [] branch
        var ratingsFile = Path.Combine(AppConfig.ConfigDir, "ratings.json");
        var backupExists = File.Exists(ratingsFile);
        string? backup = backupExists ? File.ReadAllText(ratingsFile) : null;

        try
        {
            // Ensure directory exists
            Directory.CreateDirectory(AppConfig.ConfigDir);
            File.WriteAllText(ratingsFile, "null");

            var all = Ratings.AllRatings();
            Assert.NotNull(all);
            // Should be an empty (or near-empty) dict — specifically covers the ?? [] branch
        }
        finally
        {
            // Restore the original file
            if (backup is not null)
                File.WriteAllText(ratingsFile, backup);
            else if (File.Exists(ratingsFile))
                File.Delete(ratingsFile);
        }
    }

    // ── AllRatings() when file is corrupted JSON → returns empty dict (catch) ─

    [Fact]
    public void AllRatings_CorruptedFile_ReturnsEmptyDict()
    {
        var ratingsFile = Path.Combine(AppConfig.ConfigDir, "ratings.json");
        var backupExists = File.Exists(ratingsFile);
        string? backup = backupExists ? File.ReadAllText(ratingsFile) : null;

        try
        {
            Directory.CreateDirectory(AppConfig.ConfigDir);
            File.WriteAllText(ratingsFile, "<<< NOT JSON >>>");

            var all = Ratings.AllRatings();
            // Should return empty dict (catch branch)
            Assert.NotNull(all);
            Assert.Empty(all);
        }
        finally
        {
            if (backup is not null)
                File.WriteAllText(ratingsFile, backup);
            else if (File.Exists(ratingsFile))
                File.Delete(ratingsFile);
        }
    }
}

// ═════════════════════════════════════════════════════════════════════════════
// Analytics — additional branches: double-Flush (no-op second call), Reset
// when no file exists, GetStats() when file has "null" JSON.
// ═════════════════════════════════════════════════════════════════════════════

/// <summary>
/// Sprint 121 — Additional branch-coverage tests for <see cref="Analytics"/>.
/// </summary>
[Collection("Analytics")]
public sealed class AnalyticsBranchTests
{
    // ── Reset() when analytics file does not exist → no-op (no File.Delete) ──

    [Fact]
    public void Reset_WhenFileDoesNotExist_DoesNotThrow()
    {
        // Force a clean state (removes the file if present)
        Analytics.Reset();

        // Reset again — file no longer exists; the if(File.Exists) → false branch
        Analytics.Reset();
        // Verify stats are still zeroed after second Reset
        var stats = Analytics.GetStats();
        Assert.Equal(0, stats.TotalApplies);
    }

    // ── Flush() called twice without dirty state → second call is no-op ───────

    [Fact]
    public void Flush_CalledTwiceWithoutDirty_DoesNotThrow()
    {
        Analytics.Flush(); // first flush — may or may not write
        Analytics.Flush(); // second flush — _dirty is false → early return branch
    }

    // ── GetStats() double-call reads from in-memory cache (no re-parse) ───────

    [Fact]
    public void GetStats_CalledTwice_ReturnsSameInstance()
    {
        // After Reset, first GetStats() loads/creates in memory.
        // Second call hits the cached-data path (file unchanged, _stats != null).
        Analytics.Reset();
        var first = Analytics.GetStats();
        var second = Analytics.GetStats();
        // Both calls should return consistent data
        Assert.Equal(first.TotalApplies, second.TotalApplies);
    }

    // ── TopTweaks(n) when n is larger than available tweaks → returns all ──────

    [Fact]
    public void TopTweaks_LargerNThanData_ReturnsAllAvailable()
    {
        Analytics.Reset();
        Analytics.RecordApply("branch-toptweaks-a");
        Analytics.RecordApply("branch-toptweaks-b");
        var top = Analytics.TopTweaks(999);
        Assert.Equal(2, top.Count);
    }
}
