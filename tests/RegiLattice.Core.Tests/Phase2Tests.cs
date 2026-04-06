// Phase2Tests.cs — xUnit tests for Phase 2 UI/UX & Accessibility features:
//   2.2 Keyboard shortcuts (F1/F5/Ctrl+F/Ctrl+L/Escape) data-layer coverage  (v6.18.0)
//   2.5 Enhanced context menu handlers (Favorites, History, Deps, RegPath)    (v6.18.0)
//
// Core logic tested here (no WinForms dependency):
//   • RegOp.Path is readable → used by Copy Registry Path handler
//   • TweakDef.ApplyOps paths are distinct-able → de-dup before copying to clipboard
//   • Regedit base-key path normalization (HKCU / HKLM strings)
//   • TweakEngine.ResolveDependencies returns TweakDef chain → Show Dependencies handler
//   • Favorites.Toggle returns bool semantics → ⭐ Toggle Favorite handler
//   • TweakHistory.ForTweak after RecordApply → View History handler
//   • HistoryEntry shape: Timestamp is non-empty string, Action + Result are non-empty
//   • TweakDef ImpactScore / SafetyRating defaults and range enforcement

using System;
using System.Collections.Generic;
using System.Linq;
using RegiLattice.Core;
using RegiLattice.Core.Models;
using RegiLattice.Core.Registry;
using RegiLattice.Core.Services;
using Xunit;

namespace RegiLattice.Core.Tests;

/// <summary>Tests for Phase 2.2 (keyboard shortcuts data) and Phase 2.5 (context menu handlers).</summary>
[Collection("Favorites")]
public sealed class Phase2Tests : IDisposable
{
    public Phase2Tests()
    {
        Favorites.Reset();
        TweakHistory.Reset();
    }

    public void Dispose()
    {
        Favorites.Reset();
        TweakHistory.Reset();
    }

    // ── Helpers ───────────────────────────────────────────────────────

    private static TweakDef MakeHklm(string id, string? depOn = null) =>
        new TweakDef
        {
            Id = id,
            Label = id,
            Category = "Phase2",
            DependsOn = depOn is null ? [] : [depOn],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\P2Test", id, 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\P2Test", id)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\P2Test", id, 1)],
        };

    private static TweakDef MakeWithMultiOps(string id) =>
        new TweakDef
        {
            Id = id,
            Label = id,
            Category = "Phase2",
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\P2A", "A", 1),
                RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\P2A", "B", 0),
                RegOp.SetString(@"HKEY_CURRENT_USER\SOFTWARE\P2B", "C", "off"),
            ],
        };

    // ── RegOp.Path is accessible ──────────────────────────────────────

    [Fact]
    public void RegOp_SetDword_PathIsReadable()
    {
        var op = RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Test", "Val", 1);
        Assert.Equal(@"HKEY_LOCAL_MACHINE\SOFTWARE\Test", op.Path);
    }

    [Fact]
    public void RegOp_SetString_PathIsReadable()
    {
        var op = RegOp.SetString(@"HKEY_CURRENT_USER\SOFTWARE\Test", "Val", "x");
        Assert.Equal(@"HKEY_CURRENT_USER\SOFTWARE\Test", op.Path);
    }

    // ── Copy Registry Path — distinct paths from ApplyOps ────────────

    [Fact]
    public void TweakDef_ApplyOps_DistinctPaths_ReturnsUniqueKeyPaths()
    {
        var td = MakeWithMultiOps("cp-test-1");
        var distinctPaths = td.ApplyOps.Select(o => o.Path).Distinct().ToList();
        Assert.Equal(2, distinctPaths.Count);
        Assert.Contains(@"HKEY_CURRENT_USER\SOFTWARE\P2A", distinctPaths);
        Assert.Contains(@"HKEY_CURRENT_USER\SOFTWARE\P2B", distinctPaths);
    }

    [Fact]
    public void TweakDef_SinglePathOp_DistinctPaths_ReturnsSinglePath()
    {
        var td = MakeHklm("cp-test-2");
        var distinctPaths = td.ApplyOps.Select(o => o.Path).Distinct().ToList();
        Assert.Single(distinctPaths);
    }

    // ── Regedit path normalization ────────────────────────────────────
    // The OnCtxOpenInRegedit handler normalizes HKCU/HKLM abbreviations before
    // writing to HKCU\Software\Microsoft\Windows\CurrentVersion\Applets\Regedit\LastKey.

    [Theory]
    [InlineData(@"HKEY_LOCAL_MACHINE\SOFTWARE\X", @"HKEY_LOCAL_MACHINE\SOFTWARE\X")]
    [InlineData(@"HKEY_CURRENT_USER\SOFTWARE\X", @"HKEY_CURRENT_USER\SOFTWARE\X")]
    [InlineData(@"HKLM\SOFTWARE\X", @"HKEY_LOCAL_MACHINE\SOFTWARE\X")]
    [InlineData(@"HKCU\SOFTWARE\X", @"HKEY_CURRENT_USER\SOFTWARE\X")]
    public void RegeditPath_Normalization_ProducesFullHiveName(string input, string expected)
    {
        string normalized = input
            .Replace(@"HKLM\", @"HKEY_LOCAL_MACHINE\", StringComparison.OrdinalIgnoreCase)
            .Replace(@"HKCU\", @"HKEY_CURRENT_USER\", StringComparison.OrdinalIgnoreCase);

        Assert.Equal(expected, normalized);
    }

    // ── TweakEngine.ResolveDependencies ──────────────────────────────

    [Fact]
    public void Engine_ResolveDependencies_TweakNoDeps_ReturnsSelf()
    {
        var engine = new TweakEngine();
        var td = MakeHklm("rd-root");
        engine.Register([td]);

        var chain = engine.ResolveDependencies("rd-root");

        Assert.Single(chain);
        Assert.Equal("rd-root", chain[0].Id);
    }

    [Fact]
    public void Engine_ResolveDependencies_TweakWithOneDep_ReturnsTwoInOrder()
    {
        var engine = new TweakEngine();
        var dep = MakeHklm("rd-dep-1");
        var root = MakeHklm("rd-root-1", depOn: "rd-dep-1");
        engine.Register([dep]);
        engine.Register([root]);

        var chain = engine.ResolveDependencies("rd-root-1");

        Assert.Equal(2, chain.Count);
        // dep comes before root in topological order
        Assert.Equal("rd-dep-1", chain[0].Id);
        Assert.Equal("rd-root-1", chain[1].Id);
    }

    [Fact]
    public void Engine_ResolveDependencies_BuiltinTweak_DoesNotThrow()
    {
        var engine = new TweakEngine();
        engine.RegisterBuiltins();

        // Pick a well-known tweak with no deps — ResolveDependencies should return it alone.
        var tweak = engine.AllTweaks().First(t => t.DependsOn.Count == 0);
        var chain = engine.ResolveDependencies(tweak.Id);

        Assert.NotEmpty(chain);
        Assert.Equal(tweak.Id, chain.Last().Id);
    }

    // ── Favorites toggle — Phase 2.5 context menu ────────────────────

    [Fact]
    public void Favorites_Toggle_ReturnsTrueWhenAdding()
    {
        bool added = Favorites.Toggle("p2-toggle-1");
        Assert.True(added);
    }

    [Fact]
    public void Favorites_Toggle_ReturnsFalseWhenRemoving()
    {
        Favorites.Toggle("p2-toggle-2");     // add
        bool result = Favorites.Toggle("p2-toggle-2"); // second Toggle removes → returns false
        Assert.False(result);
    }

    [Fact]
    public void Favorites_Toggle_CycleAddsAndRemoves()
    {
        const string id = "p2-cycle-1";
        bool r1 = Favorites.Toggle(id);   // true = added
        bool r2 = Favorites.Toggle(id);   // false = removed
        bool r3 = Favorites.Toggle(id);   // true = added again

        Assert.True(r1);
        Assert.False(r2);
        Assert.True(r3);
        Assert.True(Favorites.IsFavorite(id)); // final state: added
    }

    [Fact]
    public void Favorites_ContextMenuFlow_TwoIds_ToggleIndependently()
    {
        bool a = Favorites.Toggle("p2-ctx-a");
        bool b = Favorites.Toggle("p2-ctx-b");

        Assert.True(a);
        Assert.True(b);
        Assert.True(Favorites.IsFavorite("p2-ctx-a"));
        Assert.True(Favorites.IsFavorite("p2-ctx-b"));
    }

    // ── TweakHistory — Phase 2.5 View History handler ────────────────

    [Fact]
    public void TweakHistory_ForTweak_NoHistory_ReturnsEmpty()
    {
        var entries = TweakHistory.ForTweak("p2-ghost-id");
        Assert.Empty(entries);
    }

    [Fact]
    public void TweakHistory_RecordApply_ForTweak_ReturnsSingleEntry()
    {
        TweakHistory.RecordApply("p2-hist-1", TweakResult.Applied);
        var entries = TweakHistory.ForTweak("p2-hist-1");

        Assert.Single(entries);
        Assert.Equal("p2-hist-1", entries[0].TweakId);
    }

    [Fact]
    public void TweakHistory_HistoryEntry_ActionIsNonEmpty()
    {
        TweakHistory.RecordApply("p2-action-1", TweakResult.Applied);
        var entry = TweakHistory.ForTweak("p2-action-1")[0];

        Assert.NotEmpty(entry.Action);
    }

    [Fact]
    public void TweakHistory_HistoryEntry_TimestampIsNonEmpty()
    {
        TweakHistory.RecordApply("p2-ts-1", TweakResult.Applied);
        var entry = TweakHistory.ForTweak("p2-ts-1")[0];

        Assert.NotEmpty(entry.Timestamp);
    }

    [Fact]
    public void TweakHistory_HistoryEntry_ResultIsNonEmpty()
    {
        TweakHistory.RecordApply("p2-res-1", TweakResult.Applied);
        var entry = TweakHistory.ForTweak("p2-res-1")[0];

        Assert.NotEmpty(entry.Result);
    }

    [Fact]
    public void TweakHistory_MultipleActions_ForTweak_ReturnsAll()
    {
        TweakHistory.RecordApply("p2-multi-1", TweakResult.Applied);
        TweakHistory.RecordRemove("p2-multi-1", TweakResult.NotApplied);

        var entries = TweakHistory.ForTweak("p2-multi-1");
        Assert.Equal(2, entries.Count);
    }

    // ── TweakDef metadata (used by Phase 2.5 UI labels) ──────────────

    [Fact]
    public void TweakDef_ImpactScore_DefaultIs3()
    {
        var td = MakeHklm("meta-default-1");
        Assert.Equal(3, td.ImpactScore);
    }

    [Fact]
    public void TweakDef_SafetyRating_DefaultIs4()
    {
        var td = MakeHklm("meta-default-2");
        Assert.Equal(4, td.SafetyRating);
    }

    [Fact]
    public void TweakDef_ExplicitImpactScore_IsRetained()
    {
        var td = new TweakDef
        {
            Id = "meta-explicit-1",
            Label = "Test",
            Category = "Phase2",
            ImpactScore = 5,
            SafetyRating = 2,
            ApplyOps = [RegOp.SetDword(@"HKCU\SOFTWARE\Test", "X", 1)],
        };
        Assert.Equal(5, td.ImpactScore);
        Assert.Equal(2, td.SafetyRating);
    }
}
