// tests/RegiLattice.Core.Tests/BranchCoverage7Tests.cs
// Sprint 121 — BC7: Additional branch coverage to reach ≥75% (1738/2317).
// Targeting 12 uncovered branches identified from cov121f Cobertura XML analysis.

#nullable enable
using System.Reflection;
using RegiLattice.Core;
using RegiLattice.Core.Models;
using RegiLattice.Core.Services;
using Xunit;

namespace RegiLattice.Core.Tests;

// ── 1. Analytics — GetStats when file exists (L49 F-branch) ──────────────────
//    analytics.cs L49: `if (!File.Exists(FilePath))` — F-branch (file exists → try read)
//    Existing tests always call Reset() first (deletes file) → T-branch covered.
//    This test writes a file first so the F-branch is taken.

public sealed class AnalyticsFileExistsBranchTests : IDisposable
{
    public AnalyticsFileExistsBranchTests() => Analytics.Reset();

    public void Dispose() => Analytics.Reset();

    [Fact]
    public void GetStats_WhenFileExists_LoadsFromDisk()
    {
        var filePathField = typeof(Analytics).GetField("FilePath", BindingFlags.NonPublic | BindingFlags.Static)!;
        var fp = (string)filePathField.GetValue(null)!;

        // Write valid analytics JSON so File.Exists returns true.
        Directory.CreateDirectory(Path.GetDirectoryName(fp)!);
        File.WriteAllText(
            fp,
            """{"total_applies":7,"total_removes":2,"total_errors":0,"total_sessions":1,"most_applied":{},"most_removed":{},"error_counts":{},"last_session":0}"""
        );

        // Null the in-memory cache so GetStats() re-reads from disk (hits the F-branch).
        typeof(Analytics).GetField("_cache", BindingFlags.NonPublic | BindingFlags.Static)!.SetValue(null, null);

        var data = Analytics.GetStats();
        Assert.Equal(7, data.TotalApplies);
    }
}

// ── 2. SnapshotManager — null JSON → ?? [] (L32 T-branch) ────────────────────
//    SnapshotManager.cs L32:
//       `return JsonSerializer.Deserialize<Dictionary<string,string>>(json) ?? [];`
//    Existing tests always pass valid JSON → Deserialize returns non-null → F-branch.
//    This test writes "null" so Deserialize returns null → ?? [] fires (T-branch).

public sealed class SnapshotNullJsonBranchTests
{
    [Fact]
    public void Load_NullJsonContent_ReturnsEmptyDictionary()
    {
        var path = Path.GetTempFileName();
        try
        {
            File.WriteAllText(path, "null");
            var result = SnapshotManager.Load(path);
            Assert.Empty(result);
        }
        finally
        {
            File.Delete(path);
        }
    }
}

// ── 3. ComplianceHistory — null JSON → ?.AsReadOnly() → ?? [] (L57, +2 branches) ─
//    ComplianceHistory.cs L57:
//       `return JsonSerializer.Deserialize<List<ComplianceHistoryEntry>>(json, JsonOpts)?.AsReadOnly() ?? [];`
//    Covers 2 of 4 still-uncovered sub-branches:
//      · Deserialize returns null → `?.AsReadOnly()` returns null (null-conditional T)
//      · null?.AsReadOnly() == null → `?? []` fires (null-coalescing T)

[Collection("ComplianceHistory")]
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

[Collection("Ratings")]
public sealed class RatingsFileExistsBranchTests : IDisposable
{
    private readonly string _filePath;

    public RatingsFileExistsBranchTests()
    {
        _filePath = (string)typeof(Ratings).GetField("FilePath", BindingFlags.NonPublic | BindingFlags.Static)!.GetValue(null)!;

        if (File.Exists(_filePath))
            File.Delete(_filePath);
    }

    public void Dispose()
    {
        if (File.Exists(_filePath))
            File.Delete(_filePath);
    }

    [Fact]
    public void AllRatings_AfterRate_FileExistsBranchIsTaken()
    {
        // Rate() creates the ratings file.
        Ratings.Rate("bc7-test-tweak", 4);

        // Subsequent AllRatings() finds file exists → F-branch of if (!File.Exists(FilePath)).
        var all = Ratings.AllRatings();
        Assert.True(all.ContainsKey("bc7-test-tweak"));
    }
}

// ── 5. HealthScoreService — empty engine → totalWeight == 0 (L191 T-branch) ──
//    HealthScoreService.cs ScoreBucket L191: `if (totalWeight == 0) return 0;`
//    Existing tests use RegisterBuiltins() so every bucket has tweaks → totalWeight > 0 → F-branch.
//    Empty engine has no tweaks → foreach does nothing → totalWeight stays 0 → T-branch.

public sealed class HealthScoreEmptyEngineBranchTests
{
    [Fact]
    public void Compute_EmptyEngine_TotalWeightIsZeroPath()
    {
        // Deliberately no RegisterBuiltins → engine.AllTweaks() is empty.
        var engine = new TweakEngine();
        var service = new HealthScoreService(engine);
        var score = service.Compute(new Dictionary<string, TweakResult>());

        // ScoreBucket hits totalWeight == 0 → T-branch → return 0 for all buckets.
        Assert.Equal(0, score.Privacy);
        Assert.Equal(0, score.Performance);
        Assert.Equal(0, score.Security);
        Assert.Equal(0, score.Stability);
    }
}

// ── 6. AppConfig.Load — null-path arg (L285 T-branch of `path ??= DefaultConfigPath`) ─
//    AppConfig.cs L285: `path ??= DefaultConfigPath;`
//    All 35 existing test calls pass an explicit path → F-branch (no assignment).
//    This test calls Load() with no argument (null default) → T-branch fires once.

public sealed class AppConfigLoadNoArgBranchTests
{
    [Fact]
    public void Load_NullArgument_SetsPathFromDefaultConfigPath()
    {
        // No argument → path parameter is null → `path ??= DefaultConfigPath` T-branch.
        // Read-only call: does not write anything; returns default config if file absent.
        var config = AppConfig.Load();
        Assert.NotNull(config);
    }
}

// ── 7. ScheduledTweakService — null JSON (L82 T) and empty array (L82 F) ─────
//    ScheduledTweakService.cs L82:
//       `_schedules = JsonSerializer.Deserialize<List<TweakSchedule>>(json, JsonOptions) ?? [];`
//    Both branches currently at 0% (line never executed by any existing test).
//    · "null" JSON → Deserialize returns null → ?? [] fires (T-branch)
//    · "[]" JSON  → Deserialize returns empty List → ?? not needed (F-branch)

public sealed class ScheduledTweakServiceBranchTests : IDisposable
{
    private static readonly string SchedulesPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "RegiLattice",
        "scheduled-tweaks.json"
    );

    private readonly string? _backup;

    public ScheduledTweakServiceBranchTests()
    {
        if (File.Exists(SchedulesPath))
            _backup = File.ReadAllText(SchedulesPath);
    }

    public void Dispose()
    {
        if (_backup is not null)
            File.WriteAllText(SchedulesPath, _backup);
        else if (File.Exists(SchedulesPath))
            File.Delete(SchedulesPath);
    }

    [Fact]
    public void Load_NullJsonFile_SchedulesIsEmpty_TBranch()
    {
        // "null" → Deserialize<List<TweakSchedule>>("null") = null → ?? [] fires.
        Directory.CreateDirectory(Path.GetDirectoryName(SchedulesPath)!);
        File.WriteAllText(SchedulesPath, "null");

        var svc = new ScheduledTweakService();
        svc.Load();
        Assert.Empty(svc.Schedules);
    }

    [Fact]
    public void Load_EmptyArrayJson_SchedulesIsEmpty_FBranch()
    {
        // "[]" → Deserialize<List<TweakSchedule>>("[]") = empty List (not null) → ?? not triggered.
        Directory.CreateDirectory(Path.GetDirectoryName(SchedulesPath)!);
        File.WriteAllText(SchedulesPath, "[]");

        var svc = new ScheduledTweakService();
        svc.Load();
        Assert.Empty(svc.Schedules);
    }
}

// ── 8. Favorites — whitespace IDs (L116 short-circuit branch A) ──────────────
//    Favorites.cs L116:
//       `if (!string.IsNullOrWhiteSpace(id) && set.Add(id))`
//    Missing branch: `string.IsNullOrWhiteSpace(id)` == true → !true = false → short-circuit.
//    (All existing tests import valid non-whitespace IDs.)

[Collection("Favorites")]
public sealed class FavoritesWhitespaceBranchTests : IDisposable
{
    public FavoritesWhitespaceBranchTests() => Favorites.Reset();

    public void Dispose() => Favorites.Reset();

    [Fact]
    public void ImportFromJson_WhitespaceIds_AreSkipped()
    {
        var path = Path.GetTempFileName();
        try
        {
            // JSON array with empty string and whitespace-only string.
            // IsNullOrWhiteSpace("") = true, IsNullOrWhiteSpace("   ") = true
            // → short-circuit → added == 0 (L116 branch A covered).
            File.WriteAllText(path, """["", "   "]""");
            int added = Favorites.ImportFromJson(path);
            Assert.Equal(0, added);
        }
        finally
        {
            File.Delete(path);
        }
    }
}

// ── 9. AppConfig.Validate — brightness < 0 (L248) and minute > 59 (L280) ────
//    AppConfig.cs L248: `if (BrightnessDayPct < 0 || BrightnessDayPct > 100)`
//      Missing: `BrightnessDayPct < 0` T-branch (short-circuits `> 100` evaluation).
//    AppConfig.cs L280: `return int.TryParse(...) && ... && mm is >= 0 and <= 59;`
//      Missing: `mm > 59` (hh valid, minute out of range) sub-branch.

public sealed class AppConfigBrightnessBranchTests
{
    [Fact]
    public void Validate_BrightnessDayPctNegative_HasError_CoversBothBranches()
    {
        // BrightnessDayPct = -1 → left side of || is true → short-circuit → error added.
        // BrightnessDayTime = "12:99" → hh=12 (valid 0-23), mm=99 (> 59) → IsValidHhmm returns false.
        var config = new AppConfig { BrightnessDayPct = -1, BrightnessDayTime = "12:99" };
        var errors = config.Validate();
        Assert.Contains(errors, e => e.Contains("brightness_day_pct"));
        Assert.Contains(errors, e => e.Contains("brightness_day_time"));
    }
}
