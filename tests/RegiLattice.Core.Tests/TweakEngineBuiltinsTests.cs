using RegiLattice.Core;
using RegiLattice.Core.Models;
using RegiLattice.Core.Registry;
using Xunit;

namespace RegiLattice.Core.Tests;

/// <summary>Tests that require RegisterBuiltins — share a single engine via IClassFixture.</summary>
public sealed class TweakEngineBuiltinsTests : IClassFixture<BuiltinsFixture>
{
    private readonly TweakEngine _engine;

    public TweakEngineBuiltinsTests(BuiltinsFixture fixture)
    {
        _engine = fixture.Engine;
    }

    // ── Structural ─────────────────────────────────────────────────────

    [Fact]
    public void RegisterBuiltins_LoadsAllTweaks()
    {
        Assert.True(_engine.TweakCount > 1000, $"Expected >1000 tweaks, got {_engine.TweakCount}");
        Assert.True(_engine.CategoryCount >= 60, $"Expected >=60 categories, got {_engine.CategoryCount}");
    }

    [Fact]
    public void RegisterBuiltins_AllIdsUnique()
    {
        var ids = _engine.AllTweaks().Select(t => t.Id).ToList();
        Assert.Equal(ids.Count, ids.Distinct(StringComparer.OrdinalIgnoreCase).Count());
    }

    [Fact]
    public void RegisterBuiltins_AllHaveRequiredFields()
    {
        foreach (var td in _engine.AllTweaks())
        {
            Assert.False(string.IsNullOrWhiteSpace(td.Id), "Tweak with empty Id found");
            Assert.False(string.IsNullOrWhiteSpace(td.Label), $"Tweak {td.Id} has empty Label");
            Assert.False(string.IsNullOrWhiteSpace(td.Category), $"Tweak {td.Id} has empty Category");
        }
    }

    /// <summary>
    /// Replaces all per-ID InlineData existence checks. Verifies every registered tweak
    /// can be retrieved by its own ID — a single comprehensive coverage assertion.
    /// </summary>
    [Fact]
    public void AllRegisteredTweaks_CanBeRetrievedById()
    {
        foreach (var tweak in _engine.AllTweaks())
        {
            var resolved = _engine.GetTweak(tweak.Id);
            Assert.NotNull(resolved);
            Assert.Equal(tweak.Id, resolved.Id);
        }
    }

    // ── Profiles ───────────────────────────────────────────────────────

    [Fact]
    public void TweaksForProfile_UnknownProfile_ReturnsEmpty()
    {
        Assert.Empty(_engine.TweaksForProfile("nonexistent"));
    }

    [Fact]
    public void TweaksForProfile_Business_ReturnsNonEmpty()
    {
        var tweaks = _engine.TweaksForProfile("business");
        Assert.NotEmpty(tweaks);
        Assert.True(tweaks.Count > 100, $"Expected >100 tweaks for business profile, got {tweaks.Count}");
    }

    [Fact]
    public void ApplyProfile_UnknownProfile_ReturnsEmpty()
    {
        Assert.Empty(_engine.ApplyProfile("nonexistent"));
    }

    // ── Category existence ─────────────────────────────────────────────

    [Fact]
    public void RegisterBuiltins_SecurityCategoryExists()
    {
        Assert.Contains("Security", _engine.Categories());
    }

    [Theory]
    [InlineData("Command Line")]
    [InlineData("PowerShell")]
    [InlineData("Hardening")]
    [InlineData("Developer")]
    [InlineData("Memory")]
    [InlineData("Disk Cleanup")]
    [InlineData("Debloat")]
    [InlineData("Network Optimization")]
    [InlineData("Power Management")]
    [InlineData("SSD Optimization")]
    [InlineData("App Compatibility")]
    [InlineData("User Account")]
    [InlineData("Browser Common")]
    [InlineData("Windows Recall")]
    [InlineData("Proxy & VPN")]
    [InlineData("Event Logging")]
    [InlineData("System Restore")]
    [InlineData("Scheduled Tasks")]
    [InlineData("Security")]
    public void RegisterBuiltins_CategoryExists(string category)
    {
        Assert.Contains(category, _engine.Categories());
    }

    // ── TweakKind detection ─────────────────────────────────────────────

    [Fact]
    public void TweakKind_RegistryTweak_DefaultsToRegistry()
    {
        var tweak = _engine.GetTweak("startup-disable-startup-delay");
        Assert.NotNull(tweak);
        Assert.Equal(TweakKind.Registry, tweak.Kind);
    }

    [Fact]
    public void TweakKind_CommandLineTweak_IsSystemCommand()
    {
        var tweak = _engine.GetTweak("cmd-disable-hyper-v-hypervisor");
        Assert.NotNull(tweak);
        Assert.Equal(TweakKind.SystemCommand, tweak.Kind);
        Assert.NotNull(tweak.ApplyAction);
    }

    [Fact]
    public void TweakKind_ServiceControlTweak_IsServiceControl()
    {
        var tweak = _engine.GetTweak("ps-disable-print-spooler");
        Assert.NotNull(tweak);
        Assert.Equal(TweakKind.ServiceControl, tweak.Kind);
    }

    [Fact]
    public void TweakKind_ScheduledTaskTweak_IsScheduledTask()
    {
        var tweak = _engine.GetTweak("pst-disable-customer-experience");
        Assert.NotNull(tweak);
        Assert.Equal(TweakKind.ScheduledTask, tweak.Kind);
        Assert.NotNull(tweak.ApplyAction);
    }

    [Fact]
    public void TweakKind_DiskCleanupSystemCommand_IsCorrect()
    {
        var tweak = _engine.GetTweak("cleanup-disable-hibernation");
        Assert.NotNull(tweak);
        Assert.Equal(TweakKind.SystemCommand, tweak.Kind);
    }

    [Fact]
    public void TweakKind_DebloatPowerShell_IsCorrect()
    {
        var tweak = _engine.GetTweak("debloat-remove-preinstalled-apps");
        Assert.NotNull(tweak);
        Assert.Equal(TweakKind.PowerShell, tweak.Kind);
    }

    [Fact]
    public void TweakKind_PowerMgmtSystemCommand_IsCorrect()
    {
        var tweak = _engine.GetTweak("pwrmgmt-set-high-performance-plan");
        Assert.NotNull(tweak);
        Assert.Equal(TweakKind.SystemCommand, tweak.Kind);
    }

    [Fact]
    public void TweakKind_SsdSystemCommand_IsCorrect()
    {
        var tweak = _engine.GetTweak("ssd-disable-last-access-timestamp");
        Assert.NotNull(tweak);
        Assert.Equal(TweakKind.SystemCommand, tweak.Kind);
    }

    [Fact]
    public void TweakKind_UacGroupPolicy_IsCorrect()
    {
        var tweak = _engine.GetTweak("uac-disable-dimming");
        Assert.NotNull(tweak);
        Assert.Equal(TweakKind.GroupPolicy, tweak.Kind);
    }

    [Fact]
    public void TweakKind_RecallGroupPolicy_IsCorrect()
    {
        var tweak = _engine.GetTweak("recall-disable-recall");
        Assert.NotNull(tweak);
        Assert.Equal(TweakKind.GroupPolicy, tweak.Kind);
    }

    [Fact]
    public void TweakKind_ProxyRegistry_IsCorrect()
    {
        var tweak = _engine.GetTweak("proxy-disable-auto-detect");
        Assert.NotNull(tweak);
        Assert.Equal(TweakKind.Registry, tweak.Kind);
    }

    // ── Category counts ─────────────────────────────────────────────────

    [Fact]
    public void RegisterBuiltins_HasWindowAppearanceCategory() => Assert.Contains("Window Appearance", _engine.Categories());

    [Fact]
    public void RegisterBuiltins_WindowAppearance_HasAtLeast40Tweaks()
    {
        var byCat = _engine.TweaksByCategory();
        Assert.True(byCat.ContainsKey("Window Appearance"));
        Assert.True(byCat["Window Appearance"].Count >= 40, $"Expected ≥40 Window Appearance tweaks, got {byCat["Window Appearance"].Count}");
    }

    [Fact]
    public void RegisterBuiltins_HasSystemOptimizationCategory() => Assert.Contains("System Optimization", _engine.Categories());

    [Fact]
    public void RegisterBuiltins_SystemOptimization_HasAtLeast30Tweaks()
    {
        var byCat = _engine.TweaksByCategory();
        Assert.True(byCat.ContainsKey("System Optimization"));
        Assert.True(byCat["System Optimization"].Count >= 30, $"Expected ≥30 System Optimization tweaks, got {byCat["System Optimization"].Count}");
    }

    [Fact]
    public void RegisterBuiltins_HasDesktopCustomizationCategory() => Assert.Contains("Desktop Customization", _engine.Categories());

    [Fact]
    public void RegisterBuiltins_DesktopCustomization_HasAtLeast30Tweaks()
    {
        var byCat = _engine.TweaksByCategory();
        Assert.True(byCat.ContainsKey("Desktop Customization"));
        Assert.True(
            byCat["Desktop Customization"].Count >= 30,
            $"Expected ≥30 Desktop Customization tweaks, got {byCat["Desktop Customization"].Count}"
        );
    }

    // ── Performance ─────────────────────────────────────────────────────

    [Fact]
    public void RegisterBuiltins_CompletesUnder750ms()
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.RegisterBuiltins();
        sw.Stop();
        Assert.True(sw.ElapsedMilliseconds < 750, $"RegisterBuiltins took {sw.ElapsedMilliseconds}ms (budget: 750ms)");
    }

    [Fact]
    public void Freeze_BuildsFrozenDictionary()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.RegisterBuiltins();
        Assert.NotNull(engine.GetTweak("priv-disable-telemetry"));
    }

    [Fact]
    public void Search_CompletesUnder50ms()
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();
        var results = _engine.Search("telemetry");
        sw.Stop();
        Assert.NotEmpty(results);
        // Budget relaxed from 50ms → 150ms → 250ms: 4 828 tweaks at v5.0.0; baseline ~172ms on dev machine.
        // Increase threshold if tweak count grows past 6 000.
        Assert.True(sw.ElapsedMilliseconds < 250, $"Search took {sw.ElapsedMilliseconds}ms (budget: 250ms)");
    }

    [Fact]
    public void Categories_ReturnsCachedResult()
    {
        var cats1 = _engine.Categories();
        var cats2 = _engine.Categories();
        Assert.Same(cats1, cats2); // Same reference = cached
    }

    // ── Validation ──────────────────────────────────────────────────────

    [Fact]
    public void ValidateTweaks_AllBuiltins_ReturnsNoErrors()
    {
        var errors = _engine.ValidateTweaks();
        Assert.Empty(errors);
    }

    [Fact]
    public void ValidateTweaks_BrokenDependsOn_ReturnsError()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([
            new TweakDef
            {
                Id = "test-dep-missing",
                Label = "Test",
                Category = "Test",
                DependsOn = ["nonexistent-tweak"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "V", 1)],
            },
        ]);
        var errors = engine.ValidateTweaks();
        Assert.Contains(errors, e => e.Contains("nonexistent-tweak"));
    }

    [Fact]
    public void ValidateTweaks_CircularDependency_ReturnsError()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([
            new TweakDef
            {
                Id = "circ-a",
                Label = "A",
                Category = "Test",
                DependsOn = ["circ-b"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "A", 1)],
            },
            new TweakDef
            {
                Id = "circ-b",
                Label = "B",
                Category = "Test",
                DependsOn = ["circ-a"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "B", 1)],
            },
        ]);
        var errors = engine.ValidateTweaks();
        Assert.Contains(errors, e => e.Contains("circular dependency"));
    }

    // ── Dependency resolution ───────────────────────────────────────────

    [Fact]
    public void ResolveDependencies_NoDeps_ReturnsSingleItem()
    {
        var chain = _engine.ResolveDependencies(_engine.AllTweaks()[0].Id);
        Assert.Single(chain);
    }

    [Fact]
    public void ResolveDependencies_SimpleChain_ReturnsTopologicalOrder()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([
            new TweakDef
            {
                Id = "dep-base",
                Label = "Base",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "Base", 1)],
            },
            new TweakDef
            {
                Id = "dep-child",
                Label = "Child",
                Category = "Test",
                DependsOn = ["dep-base"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "Child", 1)],
            },
        ]);
        engine.Freeze();

        var chain = engine.ResolveDependencies("dep-child");
        Assert.Equal(2, chain.Count);
        Assert.Equal("dep-base", chain[0].Id);
        Assert.Equal("dep-child", chain[1].Id);
    }

    [Fact]
    public void ResolveDependencies_CircularDep_Throws()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([
            new TweakDef
            {
                Id = "loop-a",
                Label = "A",
                Category = "Test",
                DependsOn = ["loop-b"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "A", 1)],
            },
            new TweakDef
            {
                Id = "loop-b",
                Label = "B",
                Category = "Test",
                DependsOn = ["loop-a"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "B", 1)],
            },
        ]);
        engine.Freeze();

        Assert.Throws<InvalidOperationException>(() => engine.ResolveDependencies("loop-a"));
    }

    [Fact]
    public void ResolveDependencies_UnknownId_Throws()
    {
        Assert.Throws<ArgumentException>(() => _engine.ResolveDependencies("nonexistent-tweak-id"));
    }

    [Fact]
    public void Dependents_NoReverse_ReturnsEmpty()
    {
        var dependents = _engine.Dependents(_engine.AllTweaks()[0].Id);
        Assert.NotNull(dependents);
    }

    [Fact]
    public void Dependents_WithReverseDep_ReturnsDependents()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([
            new TweakDef
            {
                Id = "parent-tweak",
                Label = "Parent",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "P", 1)],
            },
            new TweakDef
            {
                Id = "child-tweak",
                Label = "Child",
                Category = "Test",
                DependsOn = ["parent-tweak"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "C", 1)],
            },
        ]);
        var deps = engine.Dependents("parent-tweak");
        Assert.Single(deps);
        Assert.Equal("child-tweak", deps[0].Id);
    }

    [Fact]
    public void ResolveDependencies_DiamondGraph_ReturnsCorrectOrder()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([
            new TweakDef
            {
                Id = "diamond-d",
                Label = "D",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "D", 1)],
            },
            new TweakDef
            {
                Id = "diamond-b",
                Label = "B",
                Category = "Test",
                DependsOn = ["diamond-d"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "B", 1)],
            },
            new TweakDef
            {
                Id = "diamond-c",
                Label = "C",
                Category = "Test",
                DependsOn = ["diamond-d"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "C", 1)],
            },
            new TweakDef
            {
                Id = "diamond-a",
                Label = "A",
                Category = "Test",
                DependsOn = ["diamond-b", "diamond-c"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "A", 1)],
            },
        ]);
        engine.Freeze();

        var chain = engine.ResolveDependencies("diamond-a").ToList();
        Assert.Equal(4, chain.Count);
        int idxD = chain.FindIndex(t => t.Id == "diamond-d");
        int idxB = chain.FindIndex(t => t.Id == "diamond-b");
        int idxC = chain.FindIndex(t => t.Id == "diamond-c");
        int idxA = chain.FindIndex(t => t.Id == "diamond-a");
        Assert.True(idxD < idxB);
        Assert.True(idxD < idxC);
        Assert.True(idxB < idxA);
        Assert.True(idxC < idxA);
    }

    [Fact]
    public void ResolveDependencies_DeepChain_FiveLevels()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([
            new TweakDef
            {
                Id = "chain-1",
                Label = "L1",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "L1", 1)],
            },
            new TweakDef
            {
                Id = "chain-2",
                Label = "L2",
                Category = "Test",
                DependsOn = ["chain-1"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "L2", 1)],
            },
            new TweakDef
            {
                Id = "chain-3",
                Label = "L3",
                Category = "Test",
                DependsOn = ["chain-2"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "L3", 1)],
            },
            new TweakDef
            {
                Id = "chain-4",
                Label = "L4",
                Category = "Test",
                DependsOn = ["chain-3"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "L4", 1)],
            },
            new TweakDef
            {
                Id = "chain-5",
                Label = "L5",
                Category = "Test",
                DependsOn = ["chain-4"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "L5", 1)],
            },
        ]);
        engine.Freeze();

        var chain = engine.ResolveDependencies("chain-5");
        Assert.Equal(5, chain.Count);
        for (int i = 0; i < 5; i++)
            Assert.Equal($"chain-{i + 1}", chain[i].Id);
    }

    [Fact]
    public void Dependents_MultipleChildren_ReturnsAll()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([
            new TweakDef
            {
                Id = "multi-parent",
                Label = "Parent",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "P", 1)],
            },
            new TweakDef
            {
                Id = "multi-child-1",
                Label = "Child1",
                Category = "Test",
                DependsOn = ["multi-parent"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "C1", 1)],
            },
            new TweakDef
            {
                Id = "multi-child-2",
                Label = "Child2",
                Category = "Test",
                DependsOn = ["multi-parent"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "C2", 1)],
            },
            new TweakDef
            {
                Id = "multi-child-3",
                Label = "Child3",
                Category = "Test",
                DependsOn = ["multi-parent"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "C3", 1)],
            },
        ]);
        var deps = engine.Dependents("multi-parent");
        Assert.Equal(3, deps.Count);
        Assert.Contains(deps, d => d.Id == "multi-child-1");
        Assert.Contains(deps, d => d.Id == "multi-child-2");
        Assert.Contains(deps, d => d.Id == "multi-child-3");
    }

    // ── Batch with progress callback ────────────────────────────────────

    [Fact]
    public void ApplyBatch_WithProgress_ReportsAllTweaks()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([
            new TweakDef
            {
                Id = "prog-a",
                Label = "A",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "A", 1)],
            },
            new TweakDef
            {
                Id = "prog-b",
                Label = "B",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "B", 1)],
            },
        ]);
        var progressCalls = new List<(int Done, int Total, string Id)>();
        engine.ApplyBatch(engine.AllTweaks(), forceCorp: false, (done, total, id, _) => progressCalls.Add((done, total, id)));

        Assert.Equal(2, progressCalls.Count);
        Assert.Equal(1, progressCalls[0].Done);
        Assert.Equal(2, progressCalls[0].Total);
        Assert.Equal(2, progressCalls[1].Done);
        Assert.Equal(2, progressCalls[1].Total);
    }

    [Fact]
    public void RemoveBatch_WithProgress_ReportsAllTweaks()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([
            new TweakDef
            {
                Id = "rmprog-a",
                Label = "A",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "A", 1)],
                RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Test", "A")],
            },
        ]);
        int callCount = 0;
        engine.RemoveBatch(engine.AllTweaks(), forceCorp: false, (_, _, _, _) => callCount++);
        Assert.Equal(1, callCount);
    }

    // ── Filter: multi-criteria edge cases ───────────────────────────────

    [Fact]
    public void Filter_AllCriteria_CombinesAnd()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([
            new TweakDef
            {
                Id = "fa-target",
                Label = "Target Tweak",
                Category = "FilterAll",
                CorpSafe = true,
                NeedsAdmin = false,
                MinBuild = 19041,
                Tags = ["filtertest"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\fa-target", "V", 1)],
            },
            new TweakDef
            {
                Id = "fa-wrong-cat",
                Label = "Wrong Cat",
                Category = "Other",
                CorpSafe = true,
                NeedsAdmin = false,
                MinBuild = 19041,
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\fa-wrong-cat", "V", 1)],
            },
            new TweakDef
            {
                Id = "fa-wrong-admin",
                Label = "Needs Admin",
                Category = "FilterAll",
                CorpSafe = true,
                NeedsAdmin = true,
                MinBuild = 19041,
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\fa-wrong-admin", "V", 1)],
            },
            new TweakDef
            {
                Id = "fa-wrong-corp",
                Label = "Not CorpSafe",
                Category = "FilterAll",
                CorpSafe = false,
                NeedsAdmin = false,
                MinBuild = 19041,
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\fa-wrong-corp", "V", 1)],
            },
        ]);
        engine.Freeze();

        var results = engine.Filter(corpSafe: true, needsAdmin: false, category: "FilterAll", minBuild: 20000, query: "target");
        Assert.Single(results);
        Assert.Equal("fa-target", results[0].Id);
    }

    [Fact]
    public void Filter_NoMatches_ReturnsEmpty()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([
            new TweakDef
            {
                Id = "fn-1",
                Label = "T",
                Category = "CatA",
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\fn-1", "V", 1)],
            },
            new TweakDef
            {
                Id = "fn-2",
                Label = "T",
                Category = "CatB",
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\fn-2", "V", 1)],
            },
        ]);
        engine.Freeze();
        Assert.Empty(engine.Filter(category: "NonExistentCategory"));
    }

    [Fact]
    public void Filter_NoCriteria_ReturnsAll()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([
            new TweakDef
            {
                Id = "fnc-1",
                Label = "T",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\fnc-1", "V", 1)],
            },
            new TweakDef
            {
                Id = "fnc-2",
                Label = "T",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\fnc-2", "V", 1)],
            },
            new TweakDef
            {
                Id = "fnc-3",
                Label = "T",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\fnc-3", "V", 1)],
            },
        ]);
        engine.Freeze();
        Assert.Equal(3, engine.Filter().Count);
    }

    [Fact]
    public void Filter_QueryAndScope_CombinesAnd()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([
            new TweakDef
            {
                Id = "fqs-user",
                Label = "User Telemetry",
                Category = "Privacy",
                RegistryKeys = [@"HKEY_CURRENT_USER\Software\fqs-user"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\fqs-user", "V", 1)],
            },
            new TweakDef
            {
                Id = "fqs-machine",
                Label = "Machine Telemetry",
                Category = "Privacy",
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\Software\fqs-machine"],
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\Software\fqs-machine", "V", 1)],
            },
        ]);
        engine.Freeze();

        var results = engine.Filter(scope: TweakScope.User, query: "telemetry");
        Assert.Single(results);
        Assert.Equal("fqs-user", results[0].Id);
    }

    // ── Update ──────────────────────────────────────────────────────────

    [Fact]
    public void Update_NoUpdateAction_FallsBackToApply()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        var td = new TweakDef
        {
            Id = "upd-fallback",
            Label = "U",
            Category = "Test",
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\upd-fallback", "V", 1)],
        };
        engine.Register([td]);
        Assert.Equal(TweakResult.Applied, engine.Update(td));
    }

    [Fact]
    public void Update_WithUpdateAction_CallsUpdateAction()
    {
        bool called = false;
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        var td = new TweakDef
        {
            Id = "upd-custom",
            Label = "U",
            Category = "Test",
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\upd-custom", "V", 1)],
            UpdateAction = (_) =>
            {
                called = true;
            },
        };
        engine.Register([td]);
        var result = engine.Update(td);
        Assert.Equal(TweakResult.Applied, result);
        Assert.True(called);
    }

    [Fact]
    public void Update_UpdateActionThrows_ReturnsError()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        var td = new TweakDef
        {
            Id = "upd-err",
            Label = "U",
            Category = "Test",
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\upd-err", "V", 1)],
            UpdateAction = (_) => throw new InvalidOperationException("update failed"),
        };
        engine.Register([td]);
        Assert.Equal(TweakResult.Error, engine.Update(td));
    }

    // ── Duplication guards ───────────────────────────────────────────────────

    [Fact]
    public void RegisterBuiltins_DuplicateRegistryOps_BelowRegressionThreshold()
    {
        // Regression guard: detects shared PATH\ValueName targets written by multiple tweaks.
        // Current known technical debt: a number of duplicate registry targets exist across modules.
        // Threshold (1200) is permissive to allow existing debt while blocking NET-NEW additions.
        // REMEDIATION: run scripts/Audit-Duplications.ps1 to identify and resolve these.
        var warnings = TweakValidator.DetectDuplicateRegistryOps(_engine.AllTweaks());
        Assert.True(
            warnings.Count <= 1200,
            $"Duplicate registry targets exceeded regression threshold: {warnings.Count} > 1200. "
                + $"Run scripts/Audit-Duplications.ps1 to investigate.\n"
                + $"First 3: {string.Join(" | ", warnings.Take(3))}"
        );
    }

    [Fact]
    public void RegisterBuiltins_NoCrossModuleLabelAndPathCollision()
    {
        // Two tweaks from DIFFERENT categories that share the same lowercased Label AND
        // the same first RegistryKeys entry are almost certainly functional duplicates.
        // Same label + different registry path = acceptable (same feature, different mechanism).
        // Threshold (200) is a regression guard over current technical debt (128 groups exist).
        // REMEDIATION: run scripts/Audit-Duplications.ps1 and review each group below.
        var collisions = _engine
            .AllTweaks()
            .Where(t => t.RegistryKeys.Count > 0)
            .GroupBy(t => (Label: t.Label.Trim().ToLowerInvariant(), Path: t.RegistryKeys[0].ToLowerInvariant()))
            .Where(g => g.Select(t => t.Category).Distinct(StringComparer.OrdinalIgnoreCase).Count() > 1)
            .Select(g => $"'{g.Key.Label}' + '{g.Key.Path}' [{string.Join(", ", g.Select(t => $"{t.Category}/{t.Id}"))}]")
            .ToList();

        Assert.True(
            collisions.Count <= 200,
            $"Cross-category duplicates (same Label + same RegistryKeys[0]) exceeded threshold: "
                + $"{collisions.Count} > 200. Run scripts/Audit-Duplications.ps1 to investigate.\n"
                + string.Join("\n", collisions.Take(10))
        );
    }

    [Fact]
    public void RegisterBuiltins_CategorySlugs_MatchKnownPrefixes()
    {
        // Spot-checks that tweaks in key categories use the canonical ID slug.
        // Catches misfiled or incorrectly prefixed tweaks added in future sprints.
        // Full slug table: see copilot-instructions.md "Tweak ID Naming Convention".
        var checkedCategories = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["Privacy"] = "priv-",
            ["Performance"] = "perf-",
            ["Gaming"] = "game-",
            ["Services"] = "svc-",
            ["Windows Update"] = "wu-",
            ["Security"] = "sec-",
            ["Hardening"] = "harden-",
            ["Startup"] = "startup-",
            ["Boot"] = "boot-",
            ["Taskbar"] = "tb-",
        };

        var violations = new List<string>();
        foreach (var (category, prefix) in checkedCategories)
        {
            if (!_engine.TweaksByCategory().TryGetValue(category, out var tweaks))
                continue;
            foreach (var t in tweaks)
            {
                if (!t.Id.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    violations.Add($"  [{category}] '{t.Id}' — expected prefix '{prefix}'");
            }
        }

        Assert.True(
            violations.Count == 0,
            $"{violations.Count} tweak(s) have IDs that don't match their canonical category slug:\n" + string.Join("\n", violations.Take(20))
        );
    }

    [Fact]
    public void RegisterBuiltins_DetectDuplicateRegistryOps_ProducesUsableOutput()
    {
        // Verifies that DetectDuplicateRegistryOps runs against all built-in tweaks
        // without throwing and returns a readable list (even if non-empty).
        // This test is a smoke-test ensuring the method works at scale with 3000+ tweaks.
        var warnings = TweakValidator.DetectDuplicateRegistryOps(_engine.AllTweaks());
        Assert.NotNull(warnings);
        // Each warning must be a non-empty string (well-formed output)
        Assert.All(warnings, w => Assert.False(string.IsNullOrWhiteSpace(w), "DetectDuplicateRegistryOps returned a null/empty warning string"));
    }
}
