// RegiLattice.Core.Tests — PropertyTests.cs
// Sprint 129 (T6.5): FsCheck property-based tests for TweakDef, TweakEngine,
// RegOp, and search/filter invariants.

using System;
using System.Collections.Generic;
using System.Linq;
using FsCheck;
using FsCheck.Xunit;
using RegiLattice.Core;
using RegiLattice.Core.Models;
using Xunit;

namespace RegiLattice.Core.Tests;

/// <summary>
/// Property-based tests using FsCheck. Each [Property] test verifies a behavioural
/// invariant that must hold for ALL generated inputs, not just hand-picked examples.
/// </summary>
public sealed class PropertyTests
{
    // ── RegOp factory invariants ──────────────────────────────────────────────

    [Property(MaxTest = 500, QuietOnSuccess = true)]
    public Property SetDword_PathAndName_ArePersisted(NonEmptyString rawPath, NonEmptyString rawName, int value)
    {
        var path = @"HKEY_CURRENT_USER\" + rawPath.Get.Replace("\0", "");
        var name = rawName.Get.Replace("\0", "");
        var op = RegOp.SetDword(path, name, value);
        return (op.Path == path && op.Name == name && (int)op.Value! == value).Label("SetDword stores path, name, and value exactly");
    }

    [Property(MaxTest = 500, QuietOnSuccess = true)]
    public Property SetString_PathAndName_ArePersisted(NonEmptyString rawPath, NonEmptyString rawName, NonNull<string> rawValue)
    {
        var path = @"HKEY_LOCAL_MACHINE\" + rawPath.Get.Replace("\0", "");
        var name = rawName.Get.Replace("\0", "");
        var value = rawValue.Get.Replace("\0", "");
        var op = RegOp.SetString(path, name, value);
        return (op.Path == path && op.Name == name && (string)op.Value! == value).Label("SetString stores path, name, and value exactly");
    }

    [Property(MaxTest = 500, QuietOnSuccess = true)]
    public Property DeleteValue_OpKindIsDeleteValue(NonEmptyString rawPath, NonEmptyString rawName)
    {
        var path = @"HKEY_CURRENT_USER\" + rawPath.Get.Replace("\0", "");
        var name = rawName.Get.Replace("\0", "");
        var op = RegOp.DeleteValue(path, name);
        return (op.Kind == RegOpKind.DeleteValue).Label("DeleteValue always produces Kind=DeleteValue");
    }

    [Property(MaxTest = 200, QuietOnSuccess = true)]
    public Property CheckDword_KindIsCheckDword(NonEmptyString rawPath, NonEmptyString rawName, int expected)
    {
        var path = @"HKEY_LOCAL_MACHINE\" + rawPath.Get.Replace("\0", "");
        var name = rawName.Get.Replace("\0", "");
        var op = RegOp.CheckDword(path, name, expected);
        return (op.Kind == RegOpKind.CheckValue && (int)op.Value! == expected).Label("CheckDword stores Kind=CheckValue and correct expected value");
    }

    // ── TweakDef scope invariants ─────────────────────────────────────────────

    [Fact]
    public void TweakDefScope_IsAlwaysOneOfThreeValues()
    {
        // Property: every registered tweak has Scope in {User, Machine, Both}.
        var engine = new TweakEngine();
        engine.RegisterBuiltins();

        var validScopes = new HashSet<TweakScope> { TweakScope.User, TweakScope.Machine, TweakScope.Both };
        foreach (var tweak in engine.AllTweaks())
            Assert.Contains(tweak.Scope, validScopes);
    }

    [Fact]
    public void TweakDefKind_IsAlwaysDefinedEnumValue()
    {
        var engine = new TweakEngine();
        engine.RegisterBuiltins();

        var validKinds = Enum.GetValues<TweakKind>().ToHashSet();
        foreach (var tweak in engine.AllTweaks())
            Assert.Contains(tweak.Kind, validKinds);
    }

    // ── TweakEngine search invariants ─────────────────────────────────────────

    [Fact]
    public void Search_CaseInsensitive_ReturnsIdenticalResults()
    {
        var engine = new TweakEngine();
        engine.RegisterBuiltins();

        // Use a fixed set of representative query terms.
        string[] queries = ["privacy", "telemetry", "GPU", "PERFORMANCE", "Storage"];
        foreach (string q in queries)
        {
            var upper = engine.Search(q.ToUpperInvariant()).Select(t => t.Id).ToList();
            var lower = engine.Search(q.ToLowerInvariant()).Select(t => t.Id).ToList();
            Assert.Equal(upper, lower);
        }
    }

    [Fact]
    public void Search_EmptyQuery_ReturnsAllHasOperationsTweaks()
    {
        var engine = new TweakEngine();
        engine.RegisterBuiltins();

        var searchAll = engine.Search("").Count;
        var allTweaks = engine.AllTweaks().Count;
        // Search("") should return every registered tweak (they all match empty string).
        Assert.Equal(allTweaks, searchAll);
    }

    [Fact]
    public void Search_SubstringMatch_ResultsContainQueryInLabelOrDescOrTags()
    {
        var engine = new TweakEngine();
        engine.RegisterBuiltins();

        var results = engine.Search("bluetooth");
        Assert.All(
            results,
            t =>
                Assert.True(
                    t.Label.Contains("bluetooth", StringComparison.OrdinalIgnoreCase)
                        || t.Description.Contains("bluetooth", StringComparison.OrdinalIgnoreCase)
                        || t.Category.Contains("bluetooth", StringComparison.OrdinalIgnoreCase)
                        || t.Id.Contains("bluetooth", StringComparison.OrdinalIgnoreCase)
                        || t.Tags.Any(tag => tag.Contains("bluetooth", StringComparison.OrdinalIgnoreCase))
                )
        );
    }

    // ── TweakEngine filter invariants ─────────────────────────────────────────

    [Fact]
    public void Filter_NoConstraints_ReturnsAllTweaks()
    {
        var engine = new TweakEngine();
        engine.RegisterBuiltins();

        var filtered = engine.Filter().Count;
        var all = engine.AllTweaks().Count;
        Assert.Equal(all, filtered);
    }

    [Fact]
    public void Filter_ByScopeMachine_AllResultsAreMachineOrBoth()
    {
        var engine = new TweakEngine();
        engine.RegisterBuiltins();

        var results = engine.Filter(scope: TweakScope.Machine);
        Assert.All(results, t => Assert.True(t.Scope == TweakScope.Machine || t.Scope == TweakScope.Both));
    }

    [Fact]
    public void Filter_ByScopeUser_AllResultsAreUserOrBoth()
    {
        var engine = new TweakEngine();
        engine.RegisterBuiltins();

        var results = engine.Filter(scope: TweakScope.User);
        Assert.All(results, t => Assert.True(t.Scope == TweakScope.User || t.Scope == TweakScope.Both));
    }

    [Fact]
    public void Filter_ByCategory_AllResultsHaveThatCategory()
    {
        var engine = new TweakEngine();
        engine.RegisterBuiltins();

        foreach (string category in engine.Categories().Take(10))
        {
            var results = engine.Filter(category: category);
            Assert.All(results, t => Assert.Equal(category, t.Category));
        }
    }

    // ── TweakEngine ID uniqueness invariant ────────────────────────────────────

    [Fact]
    public void AllTweaks_IdsAreGloballyUnique()
    {
        var engine = new TweakEngine();
        engine.RegisterBuiltins();

        var ids = engine.AllTweaks().Select(t => t.Id).ToList();
        var distinctIds = ids.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        Assert.Equal(distinctIds.Count, ids.Count);
    }

    // ── TweakDef GetExpectedResult invariant ─────────────────────────────────

    [Fact]
    public void AllTweaks_GetExpectedResult_IsNonEmpty()
    {
        var engine = new TweakEngine();
        engine.RegisterBuiltins();

        foreach (var tweak in engine.AllTweaks())
        {
            string result = tweak.GetExpectedResult();
            Assert.False(string.IsNullOrEmpty(result), $"Tweak '{tweak.Id}' has an empty GetExpectedResult()");
        }
    }

    // ── TweakEngine TweaksByCategory invariant ───────────────────────────────

    [Fact]
    public void TweaksByCategory_UnionEqualsAllTweaks()
    {
        var engine = new TweakEngine();
        engine.RegisterBuiltins();

        var byCategory = engine.TweaksByCategory();
        var allFromCategory = engine
            .Categories()
            .SelectMany(c => byCategory.TryGetValue(c, out var list) ? list : [])
            .Select(t => t.Id)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var allIds = engine.AllTweaks().Select(t => t.Id).ToHashSet(StringComparer.OrdinalIgnoreCase);

        Assert.Equal(allIds, allFromCategory);
    }

    // ── RegOp symmetry: SetDword vs CheckDword ────────────────────────────────

    [Property(MaxTest = 300, QuietOnSuccess = true)]
    public Property SetDwordAndCheckDword_SamePath_AreConsistent(NonEmptyString rawPath, NonEmptyString rawName, int value)
    {
        var path = @"HKEY_CURRENT_USER\" + rawPath.Get.Replace("\0", "");
        var name = rawName.Get.Replace("\0", "");
        var setOp = RegOp.SetDword(path, name, value);
        var checkOp = RegOp.CheckDword(path, name, value);
        return (setOp.Path == checkOp.Path && setOp.Name == checkOp.Name && (int)setOp.Value! == (int)checkOp.Value!).Label(
            "SetDword and CheckDword with same args produce ops with matching path/name/value"
        );
    }
}
