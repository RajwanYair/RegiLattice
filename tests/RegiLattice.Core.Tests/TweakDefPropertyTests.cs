// RegiLattice.Core.Tests — TweakDefPropertyTests.cs
// Sprint 73: Universal property / invariant tests for TweakDef, TweakEngine, and HealthScore.
// These tests assert structural correctness properties that MUST hold for every registered tweak
// and for the engine's derived collections, without needing property-based test generators.

#nullable enable

using System.Text.RegularExpressions;
using RegiLattice.Core.Models;
using RegiLattice.Core.Services;
using Xunit;

namespace RegiLattice.Core.Tests;

/// <summary>
/// Sprint 73 — Universal invariant / property tests.
/// Each test verifies a structural property that must hold across ALL registered tweaks.
/// Failing here means a regression in the tweak corpus or engine, not just a single tweak.
/// </summary>
public sealed class TweakDefPropertyTests
{
    // ── Shared engine ─────────────────────────────────────────────────────────

    private static readonly TweakEngine _engine = BuildEngine();

    private static TweakEngine BuildEngine()
    {
        var e = new TweakEngine();
        e.RegisterBuiltins();
        return e;
    }

    // ── A: Universal TweakDef field invariants ────────────────────────────────

    /// <summary>
    /// Tags is declared with a default of []; verify it is never null at runtime
    /// for any of the registered tweaks.
    /// </summary>
    [Fact]
    public void AllTweaks_TagsNeverNull()
    {
        foreach (var td in _engine.AllTweaks())
            Assert.NotNull(td.Tags);
    }

    /// <summary>
    /// ImpactScore defaults to 3 and must always be in the 1–5 range.
    /// A value outside this range would corrupt HealthScoreService bucket weighting.
    /// </summary>
    [Fact]
    public void AllTweaks_ImpactScore_InRange_OneToFive()
    {
        foreach (var td in _engine.AllTweaks())
            Assert.InRange(td.ImpactScore, 1, 5);
    }

    /// <summary>Scope must resolve to one of the three defined enum values.</summary>
    [Fact]
    public void AllTweaks_Scope_IsDefinedEnum()
    {
        var validScopes = new[] { TweakScope.User, TweakScope.Machine, TweakScope.Both };
        foreach (var td in _engine.AllTweaks())
            Assert.Contains(td.Scope, validScopes);
    }

    /// <summary>
    /// Tweak IDs must follow the kebab-case naming convention:
    /// all lowercase letters, digits, and hyphens; must start with a letter.
    /// </summary>
    [Fact]
    public void AllTweaks_IdFollowsKebabCase()
    {
        var pattern = new Regex(@"^[a-z][a-z0-9\-]+$", RegexOptions.Compiled);
        foreach (var td in _engine.AllTweaks())
        {
            Assert.True(
                pattern.IsMatch(td.Id),
                $"Tweak id '{td.Id}' does not follow kebab-case convention (lowercase letters, digits, hyphens)."
            );
        }
    }

    /// <summary>
    /// GetExpectedResult() must never return null for any registered tweak.
    /// It auto-generates a description when ExpectedResult is empty.
    /// </summary>
    [Fact]
    public void AllTweaks_GetExpectedResult_NeverNullOrEmpty()
    {
        foreach (var td in _engine.AllTweaks())
        {
            var result = td.GetExpectedResult();
            Assert.False(
                string.IsNullOrWhiteSpace(result),
                $"Tweak '{td.Id}' returned null/empty from GetExpectedResult()."
            );
        }
    }

    /// <summary>
    /// Every registry path inside ApplyOps (if any) must start with a known hive prefix.
    /// This catches typos like "SOFTWARE\..." without a hive prefix.
    /// </summary>
    [Fact]
    public void AllTweaks_ApplyOps_RegistryPaths_StartWithKnownHive()
    {
        var validPrefixes = new[]
        {
            "HKEY_LOCAL_MACHINE",
            "HKEY_CURRENT_USER",
            "HKEY_CLASSES_ROOT",
            "HKEY_USERS",
            "HKEY_CURRENT_CONFIG",
            "HKLM",
            "HKCU",
            "HKCR",
        };

        foreach (var td in _engine.AllTweaks())
        {
            foreach (var op in td.ApplyOps)
            {
                Assert.True(
                    validPrefixes.Any(p => op.Path.StartsWith(p, StringComparison.OrdinalIgnoreCase)),
                    $"Tweak '{td.Id}' has ApplyOp with invalid hive path: '{op.Path}'"
                );
            }
        }
    }

    /// <summary>
    /// DependsOn entries must reference IDs that are actually registered in the engine.
    /// A broken dependency causes DependencyResolver to blow up at apply time.
    /// </summary>
    [Fact]
    public void AllTweaks_DependsOn_ReferencesRegisteredIds()
    {
        var allIds = _engine.AllTweaks()
                            .Select(t => t.Id)
                            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (var td in _engine.AllTweaks())
        {
            foreach (var dep in td.DependsOn)
            {
                Assert.True(
                    allIds.Contains(dep),
                    $"Tweak '{td.Id}' depends on '{dep}' which is not registered."
                );
            }
        }
    }

    // ── B: Engine structural invariants ──────────────────────────────────────

    /// <summary>
    /// TweakEngine.TweakCount must always equal AllTweaks().Count.
    /// These are two independent accessors for the same data.
    /// </summary>
    [Fact]
    public void TweakCount_MatchesAllTweaksCount()
    {
        Assert.Equal(_engine.AllTweaks().Count, _engine.TweakCount);
    }

    /// <summary>
    /// The sum of all per-category tweak counts must equal the total tweak count.
    /// This verifies that TweaksByCategory() covers every registered tweak exactly once.
    /// </summary>
    [Fact]
    public void TweaksByCategory_SumEqualsAllTweaksCount()
    {
        var total = _engine.TweaksByCategory().Values.Sum(list => list.Count);
        Assert.Equal(_engine.TweakCount, total);
    }

    /// <summary>
    /// Every tweak's Category must appear in Categories().
    /// An orphaned category (present on a tweak but absent from the index) would
    /// be invisible in the GUI category tree.
    /// </summary>
    [Fact]
    public void AllTweaks_Category_AppearsInCategoriesIndex()
    {
        var categories = _engine.Categories().ToHashSet(StringComparer.OrdinalIgnoreCase);
        foreach (var td in _engine.AllTweaks())
        {
            Assert.True(
                categories.Contains(td.Category),
                $"Tweak '{td.Id}' has Category '{td.Category}' not found in engine.Categories()."
            );
        }
    }

    /// <summary>
    /// Every tweak's ID must appear in the TweaksByCategory() list for its own Category.
    /// Verifies bidirectional consistency between ID → Category and Category → [IDs].
    /// </summary>
    [Fact]
    public void AllTweaks_AreMemberOfTheirOwnCategory()
    {
        var byCategory = _engine.TweaksByCategory();
        foreach (var td in _engine.AllTweaks())
        {
            Assert.True(
                byCategory.TryGetValue(td.Category, out var list),
                $"Tweak '{td.Id}' category '{td.Category}' missing from TweaksByCategory() keys."
            );
            Assert.Contains(list, t => t.Id.Equals(td.Id, StringComparison.OrdinalIgnoreCase));
        }
    }

    // ── C: Search / Filter subset properties ─────────────────────────────────

    /// <summary>
    /// Filter(corpSafe: true) must always return a subset of AllTweaks().
    /// The filtered collection must never contain IDs not present in AllTweaks().
    /// </summary>
    [Fact]
    public void Filter_CorpSafeTrue_ReturnsSubsetOfAllTweaks()
    {
        var allIds = _engine.AllTweaks().Select(t => t.Id).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var filtered = _engine.Filter(corpSafe: true);

        Assert.All(filtered, t => Assert.Contains(t.Id, allIds));
        Assert.True(filtered.Count <= _engine.TweakCount);
    }

    /// <summary>
    /// Filter(needsAdmin: false) narrows the set — result count must be ≤ total count
    /// and every returned tweak must have NeedsAdmin == false.
    /// </summary>
    [Fact]
    public void Filter_NeedsAdminFalse_AllResultsHaveNeedsAdminFalse()
    {
        var filtered = _engine.Filter(needsAdmin: false);

        Assert.True(filtered.Count <= _engine.TweakCount);
        Assert.All(filtered, t => Assert.False(t.NeedsAdmin, $"Tweak '{t.Id}' has NeedsAdmin=true but passed Filter(needsAdmin:false)."));
    }

    /// <summary>
    /// Search() results must always be a strict subset of AllTweaks().
    /// Tests three representative query terms.
    /// </summary>
    [Theory]
    [InlineData("privacy")]
    [InlineData("gaming")]
    [InlineData("telemetry")]
    public void Search_ResultsAreSubsetOfAllTweaks(string query)
    {
        var allIds = _engine.AllTweaks().Select(t => t.Id).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var results = _engine.Search(query);

        Assert.NotEmpty(results);
        Assert.True(results.Count <= _engine.TweakCount);
        Assert.All(results, t => Assert.Contains(t.Id, allIds));
    }

    /// <summary>
    /// Filter(scope: User) must contain only tweaks whose Scope is User or Both —
    /// never Machine-only tweaks.
    /// </summary>
    [Fact]
    public void Filter_ScopeUser_NeverReturnsMachineTweaks()
    {
        var filtered = _engine.Filter(scope: TweakScope.User);
        Assert.All(filtered, t =>
            Assert.True(
                t.Scope == TweakScope.User || t.Scope == TweakScope.Both,
                $"Tweak '{t.Id}' has Scope={t.Scope} but was returned by Filter(scope:User)."
            )
        );
    }

    // ── D: HealthScore invariant ──────────────────────────────────────────────

    /// <summary>
    /// HealthScore.Overall is always the integer truncation of (P + E + S + St) / 4.
    /// Tests this with an all-applied map, all-notapplied, and a mixed map.
    /// </summary>
    [Fact]
    public void HealthScore_Overall_IsIntegerAverageOfFourDimensions()
    {
        var svc = new HealthScoreService(_engine);
        var allTweaks = _engine.AllTweaks();

        // Scenario 1: empty map → all zeros → Overall == 0
        var emptyScore = svc.Compute(new Dictionary<string, TweakResult>());
        Assert.Equal((emptyScore.Privacy + emptyScore.Performance + emptyScore.Security + emptyScore.Stability) / 4,
                     emptyScore.Overall);

        // Scenario 2: all applied
        var allApplied = allTweaks.ToDictionary(t => t.Id, _ => TweakResult.Applied);
        var fullScore = svc.Compute(allApplied);
        Assert.Equal((fullScore.Privacy + fullScore.Performance + fullScore.Security + fullScore.Stability) / 4,
                     fullScore.Overall);

        // Scenario 3: only privacy tweaks applied
        var privacyOnly = svc.PrivacyTweaks().ToDictionary(t => t.Id, _ => TweakResult.Applied);
        var partialScore = svc.Compute(privacyOnly);
        Assert.Equal((partialScore.Privacy + partialScore.Performance + partialScore.Security + partialScore.Stability) / 4,
                     partialScore.Overall);
    }
}
