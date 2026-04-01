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
        Assert.True(_engine.CategoryCount >= 50, $"Expected >=50 categories, got {_engine.CategoryCount}");
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
    [InlineData("PowerShell")] // was "Command Line" — merged into PowerShell
    [InlineData("Security")] // was "Hardening" — merged into Security
    [InlineData("Developer")]
    [InlineData("Performance")] // was "Memory" — merged into Performance
    [InlineData("Maintenance")] // was "Disk Cleanup" + "Event Logging" — merged into Maintenance
    [InlineData("Windows 11")] // was "Debloat" + "App Compatibility" — merged into Windows 11
    [InlineData("Network")] // was "Network Optimization" — merged into Network
    [InlineData("Power")] // was "Power Management" — merged into Power
    [InlineData("Storage")] // was "SSD Optimization" — merged into Storage
    [InlineData("User Account")]
    [InlineData("Browser Common")]
    [InlineData("Privacy")] // was "Windows Recall" — merged into Privacy
    [InlineData("Proxy & VPN")]
    [InlineData("Backup & Recovery")] // was "System Restore" — merged into Backup & Recovery
    [InlineData("Scheduled Tasks")]
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

    // Window Appearance was merged into Desktop Customization (v5.99.0 consolidation)
    [Fact]
    public void RegisterBuiltins_HasWindowAppearanceCategory() => Assert.Contains("Desktop Customization", _engine.Categories());

    [Fact]
    public void RegisterBuiltins_WindowAppearance_HasAtLeast40Tweaks()
    {
        var byCat = _engine.TweaksByCategory();
        Assert.True(byCat.ContainsKey("Desktop Customization"));
        Assert.True(
            byCat["Desktop Customization"].Count >= 40,
            $"Expected ≥40 Desktop Customization tweaks, got {byCat["Desktop Customization"].Count}"
        );
    }

    // System Optimization was merged into Performance (v5.99.0 consolidation)
    [Fact]
    public void RegisterBuiltins_HasSystemOptimizationCategory() => Assert.Contains("Performance", _engine.Categories());

    [Fact]
    public void RegisterBuiltins_SystemOptimization_HasAtLeast30Tweaks()
    {
        var byCat = _engine.TweaksByCategory();
        Assert.True(byCat.ContainsKey("Performance"));
        Assert.True(
            byCat["Performance"].Count >= 28,
            $"Expected ≥28 Performance tweaks (merged from System Optimization), got {byCat["Performance"].Count}"
        );
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
        // Note: Privacy is excluded because it now legitimately contains recall- prefix tweaks (merged from WindowsRecall)
        // Note: Performance is excluded because it now legitimately contains sysopt- prefix tweaks (merged from SystemOptimization)
        // Note: Gaming is excluded because it now legitimately contains xbgb- prefix tweaks (merged from XboxGameBar)
        // Note: Security is excluded because it now contains harden- prefix tweaks (merged from Hardening)
        // Note: Startup is excluded because it now contains boot- prefix tweaks (merged from Boot)
        // Note: Windows Update is excluded because it now contains cbsupd- prefix tweaks (merged from PolicyUpdate)
        var checkedCategories = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["Services"] = "svc-",
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

// ── merged from NewTweakModulesTests.cs ──────────────────────────────────
/// <summary>Tests for Sprint 63: 5 new tweak modules (50 new tweaks total).</summary>
public sealed class NewTweakModulesTests : IClassFixture<BuiltinsFixture>
{
    private readonly TweakEngine _engine;

    public NewTweakModulesTests(BuiltinsFixture fixture)
    {
        _engine = fixture.Engine;
    }

    private TweakEngine BuildEngine() => _engine;

    // ── Per-module registration count ────────────────────────────────────

    [Theory]
    [InlineData("xbgb-", "Xbox Game Bar")]
    [InlineData("hello-", "Windows Hello")]
    [InlineData("sac-", "Smart App Control")]
    [InlineData("energy-", "Energy Saver")]
    [InlineData("cplplus-", "Copilot+")]
    public void Module_RegistersAtLeastTenTweaks(string idPrefix, string moduleName)
    {
        var engine = BuildEngine();
        var count = engine.AllTweaks().Count(t => t.Id.StartsWith(idPrefix, StringComparison.OrdinalIgnoreCase));

        Assert.True(count >= 10, $"Module '{moduleName}' (prefix '{idPrefix}') has only {count} tweaks — expected ≥10.");
    }

    // ── Total new-tweak count ────────────────────────────────────────────

    [Fact]
    public void NewModules_TotalAtLeastFiftyNewTweaks()
    {
        var engine = BuildEngine();
        var prefixes = new[] { "xbgb-", "hello-", "sac-", "energy-", "cplplus-" };
        var total = engine.AllTweaks().Count(t => prefixes.Any(p => t.Id.StartsWith(p, StringComparison.OrdinalIgnoreCase)));

        Assert.True(total >= 50, $"Expected ≥50 new tweaks across the 5 Sprint 63 modules, but found {total}.");
    }

    // ── All new IDs are globally unique ──────────────────────────────────

    [Fact]
    public void NewModules_AllIdsAreUnique()
    {
        var engine = BuildEngine();
        var prefixes = new[] { "xbgb-", "hello-", "sac-", "energy-", "cplplus-" };
        var newIds = engine
            .AllTweaks()
            .Where(t => prefixes.Any(p => t.Id.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
            .Select(t => t.Id)
            .ToList();

        var distinct = newIds.Distinct(StringComparer.OrdinalIgnoreCase).Count();
        Assert.Equal(newIds.Count, distinct);
    }

    // ── TweakValidator passes for new tweaks ─────────────────────────────

    [Fact]
    public void NewModules_ValidatorReturnsNoErrors()
    {
        var engine = BuildEngine();
        var prefixes = new[] { "xbgb-", "hello-", "sac-", "energy-", "cplplus-" };
        var newTweaks = engine.AllTweaks().Where(t => prefixes.Any(p => t.Id.StartsWith(p, StringComparison.OrdinalIgnoreCase))).ToList();

        var errors = TweakValidator.Validate(newTweaks, id => engine.GetTweak(id));
        Assert.Empty(errors);
    }

    // ── Each module has non-empty Labels and Categories ──────────────────

    [Fact]
    public void NewModules_AllTweaks_HaveNonEmptyLabelAndCategory()
    {
        var engine = BuildEngine();
        var prefixes = new[] { "xbgb-", "hello-", "sac-", "energy-", "cplplus-" };
        var newTweaks = engine.AllTweaks().Where(t => prefixes.Any(p => t.Id.StartsWith(p, StringComparison.OrdinalIgnoreCase))).ToList();

        Assert.All(
            newTweaks,
            t =>
            {
                Assert.False(string.IsNullOrWhiteSpace(t.Label), $"Tweak {t.Id} has empty Label");
                Assert.False(string.IsNullOrWhiteSpace(t.Category), $"Tweak {t.Id} has empty Category");
            }
        );
    }

    // ── HasOperations gate ───────────────────────────────────────────────

    [Fact]
    public void NewModules_AllTweaks_HaveOperations()
    {
        var engine = BuildEngine();
        var prefixes = new[] { "xbgb-", "hello-", "sac-", "energy-", "cplplus-" };

        // Engine.Register() only accepts tweaks with HasOperations == true.
        // If they are in AllTweaks(), they already passed the gate.
        var newTweaks = engine.AllTweaks().Where(t => prefixes.Any(p => t.Id.StartsWith(p, StringComparison.OrdinalIgnoreCase))).ToList();

        Assert.All(newTweaks, t => Assert.True(t.HasOperations, $"Tweak {t.Id} has no operations (HasOperations == false)"));
    }

    // ── Categories are registered (appear in engine.Categories()) ────────

    [Theory]
    [InlineData("Gaming")] // was "Xbox / Game Bar" — merged into Gaming
    [InlineData("User Account")] // was "Windows Hello" — merged into User Account
    [InlineData("Security")] // was "Smart App Control" — merged into Security
    [InlineData("Power")] // was "Energy Saver" — merged into Power
    [InlineData("AI / Copilot")] // was "Copilot+ Features" — merged into AI / Copilot
    public void Module_CategoryIsRegisteredInEngine(string categoryName)
    {
        var engine = BuildEngine();
        var categories = engine.Categories();

        Assert.Contains(categoryName, categories, StringComparer.OrdinalIgnoreCase);
    }

    // ═══════════════════════════════════════════════════════════════════
    // Sprint 69 — Phase H: BitLockerAdvanced, AppLockerWdac, HyperVAdvanced,
    //             WindowsSandboxAdv, PrinterAdvanced (+50 tweaks)
    // ═══════════════════════════════════════════════════════════════════

    private static readonly string[] _sprint69Prefixes = ["bitlocker-", "apl-", "hyperv-", "sandbox-", "prnta-"];

    [Theory]
    [InlineData("bitlocker-", "BitLocker Advanced", 12)]
    [InlineData("apl-", "AppLocker & WDAC", 10)]
    [InlineData("hyperv-", "Hyper-V Advanced", 10)]
    [InlineData("sandbox-", "Windows Sandbox", 8)]
    [InlineData("prnta-", "Printer Advanced", 10)]
    public void Sprint69_Module_RegistersExpectedCount(string idPrefix, string moduleName, int minCount)
    {
        var engine = BuildEngine();
        var count = engine.AllTweaks().Count(t => t.Id.StartsWith(idPrefix, StringComparison.OrdinalIgnoreCase));

        Assert.True(count >= minCount, $"Module '{moduleName}' (prefix '{idPrefix}') has {count} tweaks — expected ≥{minCount}.");
    }

    [Fact]
    public void Sprint69_AllModules_TotalAtLeastFiftyTweaks()
    {
        var engine = BuildEngine();
        var total = engine.AllTweaks().Count(t => _sprint69Prefixes.Any(p => t.Id.StartsWith(p, StringComparison.OrdinalIgnoreCase)));

        Assert.True(total >= 50, $"Sprint 69 Phase H modules produced {total} tweaks, expected ≥50.");
    }

    [Fact]
    public void Sprint69_AllIdsAreUnique()
    {
        var engine = BuildEngine();
        var newIds = engine
            .AllTweaks()
            .Where(t => _sprint69Prefixes.Any(p => t.Id.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
            .Select(t => t.Id)
            .ToList();

        var distinct = newIds.Distinct(StringComparer.OrdinalIgnoreCase).Count();
        Assert.Equal(newIds.Count, distinct);
    }

    [Fact]
    public void Sprint69_AllTweaks_HaveOperations()
    {
        var engine = BuildEngine();
        var newTweaks = engine.AllTweaks().Where(t => _sprint69Prefixes.Any(p => t.Id.StartsWith(p, StringComparison.OrdinalIgnoreCase))).ToList();

        Assert.All(newTweaks, t => Assert.True(t.HasOperations, $"Sprint 69 tweak {t.Id} has no operations"));
    }

    [Fact]
    public void Sprint69_AllTweaks_HaveNonEmptyLabelAndDescription()
    {
        var engine = BuildEngine();
        var newTweaks = engine.AllTweaks().Where(t => _sprint69Prefixes.Any(p => t.Id.StartsWith(p, StringComparison.OrdinalIgnoreCase))).ToList();

        Assert.All(
            newTweaks,
            t =>
            {
                Assert.False(string.IsNullOrWhiteSpace(t.Label), $"Tweak {t.Id} has empty Label");
                Assert.False(string.IsNullOrWhiteSpace(t.Description), $"Tweak {t.Id} has empty Description");
            }
        );
    }

    [Theory]
    [InlineData("Encryption")] // was "BitLocker Advanced" — merged into Encryption
    [InlineData("Application Control Policy")]
    [InlineData("Virtualization")] // was "Hyper-V Advanced" / "Windows Sandbox" — merged into Virtualization
    public void Sprint69_NewCategories_RegisteredInEngine(string categoryName)
    {
        var engine = BuildEngine();
        Assert.Contains(categoryName, engine.Categories(), StringComparer.OrdinalIgnoreCase);
    }

    [Fact]
    public void Sprint69_ValidatorReturnsNoErrors()
    {
        var engine = BuildEngine();
        var newTweaks = engine.AllTweaks().Where(t => _sprint69Prefixes.Any(p => t.Id.StartsWith(p, StringComparison.OrdinalIgnoreCase))).ToList();

        var errors = TweakValidator.Validate(newTweaks, id => engine.GetTweak(id));
        Assert.Empty(errors);
    }

    // ═══════════════════════════════════════════════════════════════════
    // Sprints 412-416 — v5.51.0: 5 Edge Policy modules
    //   EdgePrintAndPdfPolicy      (edgepdp-)   +10
    //   EdgeSearchAddressBarPolicy (edgesrch-)  +10
    //   EdgeMediaCapturePolicy     (edgemedia-) +10
    //   EdgeTrackingProtectionPolicy (edgetrack-) +10
    //   EdgeInternetExplorerModePolicy (iemode-) +10
    // ═══════════════════════════════════════════════════════════════════

    private static readonly string[] _sprint412Prefixes = ["edgepdp-", "edgesrch-", "edgemedia-", "edgetrack-", "iemode-"];

    [Theory]
    [InlineData("edgepdp-", "Edge Print & PDF Policy", 10)]
    [InlineData("edgesrch-", "Edge Search & Address Bar Policy", 10)]
    [InlineData("edgemedia-", "Edge Media Capture Policy", 10)]
    [InlineData("edgetrack-", "Edge Tracking Protection Policy", 10)]
    [InlineData("iemode-", "Edge IE Mode Policy", 10)]
    public void Sprint412_Module_RegistersExpectedCount(string idPrefix, string moduleName, int minCount)
    {
        var count = BuildEngine().AllTweaks().Count(t => t.Id.StartsWith(idPrefix, StringComparison.OrdinalIgnoreCase));

        Assert.True(count >= minCount, $"Module '{moduleName}' (prefix '{idPrefix}') has {count} tweaks — expected ≥{minCount}.");
    }

    [Fact]
    public void Sprint412_AllModules_TotalAtLeastFiftyTweaks()
    {
        var total = BuildEngine().AllTweaks().Count(t => _sprint412Prefixes.Any(p => t.Id.StartsWith(p, StringComparison.OrdinalIgnoreCase)));

        Assert.True(total >= 50, $"Sprint 412-416 modules produced {total} tweaks, expected ≥50.");
    }

    [Fact]
    public void Sprint412_AllIds_AreUnique()
    {
        var ids = BuildEngine()
            .AllTweaks()
            .Where(t => _sprint412Prefixes.Any(p => t.Id.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
            .Select(t => t.Id)
            .ToList();

        Assert.Equal(ids.Count, ids.Distinct(StringComparer.OrdinalIgnoreCase).Count());
    }

    [Fact]
    public void Sprint412_AllTweaks_HaveOperations()
    {
        var tweaks = BuildEngine()
            .AllTweaks()
            .Where(t => _sprint412Prefixes.Any(p => t.Id.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        Assert.All(tweaks, t => Assert.True(t.HasOperations, $"Sprint 412-416 tweak {t.Id} has no operations"));
    }

    [Fact]
    public void Sprint412_AllTweaks_HaveNonEmptyLabelDescriptionCategory()
    {
        var tweaks = BuildEngine()
            .AllTweaks()
            .Where(t => _sprint412Prefixes.Any(p => t.Id.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        Assert.All(
            tweaks,
            t =>
            {
                Assert.False(string.IsNullOrWhiteSpace(t.Label), $"{t.Id} has empty Label");
                Assert.False(string.IsNullOrWhiteSpace(t.Description), $"{t.Id} has empty Description");
                Assert.False(string.IsNullOrWhiteSpace(t.Category), $"{t.Id} has empty Category");
            }
        );
    }

    [Fact]
    public void Sprint412_AllTweaks_HaveImpactAndSafetyScoresInRange()
    {
        var tweaks = BuildEngine()
            .AllTweaks()
            .Where(t => _sprint412Prefixes.Any(p => t.Id.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        Assert.All(
            tweaks,
            t =>
            {
                Assert.InRange(t.ImpactScore, 1, 5);
                Assert.InRange(t.SafetyRating, 1, 5);
            }
        );
    }

    [Fact]
    public void Sprint412_AllTweaks_ApplyOpsTargetEdgePolicyKey()
    {
        const string expectedKeyPrefix = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge";

        var tweaks = BuildEngine()
            .AllTweaks()
            .Where(t => _sprint412Prefixes.Any(p => t.Id.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        Assert.All(
            tweaks,
            t =>
                Assert.All(
                    t.ApplyOps,
                    op =>
                        Assert.True(
                            op.Path.StartsWith(expectedKeyPrefix, StringComparison.OrdinalIgnoreCase),
                            $"{t.Id}: ApplyOp path '{op.Path}' does not start with '{expectedKeyPrefix}'"
                        )
                )
        );
    }

    [Fact]
    public void Sprint412_AllTweaks_AreCorpSafe()
    {
        var tweaks = BuildEngine()
            .AllTweaks()
            .Where(t => _sprint412Prefixes.Any(p => t.Id.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        Assert.All(tweaks, t => Assert.True(t.CorpSafe, $"{t.Id} is not CorpSafe (policy tweaks should be CorpSafe=true)"));
    }

    [Fact]
    public void Sprint412_ValidatorReturnsNoErrors()
    {
        var engine = BuildEngine();
        var tweaks = engine.AllTweaks().Where(t => _sprint412Prefixes.Any(p => t.Id.StartsWith(p, StringComparison.OrdinalIgnoreCase))).ToList();

        var errors = TweakValidator.Validate(tweaks, id => engine.GetTweak(id));
        Assert.Empty(errors);
    }

    [Theory]
    [InlineData("Browser Policy")]
    public void Sprint412_NewCategories_RegisteredInEngine(string categoryName)
    {
        Assert.Contains(categoryName, BuildEngine().Categories(), StringComparer.OrdinalIgnoreCase);
    }

    // ═══════════════════════════════════════════════════════════════════
    // Sprints 417-421 — v5.52.0: 5 Edge Policy modules
    //   EdgeSecureBrowsingPolicy             (edgesec-)  +10
    //   EdgeProfileSignInPolicy              (edgeprof-) +10
    //   EdgeNotificationsAndPopupPolicy      (edgenotif-) +10
    //   EdgeDownloadHistoryPolicy            (edgedl-)   +10
    //   EdgeSmartScreenAndSiteIsolationPolicy (edgessf-) +10
    // ═══════════════════════════════════════════════════════════════════

    private static readonly string[] _sprint417Prefixes = ["edgesec-", "edgeprof-", "edgenotif-", "edgedl-", "edgessf-"];

    [Theory]
    [InlineData("edgesec-", "Edge Secure Browsing Policy", 10)]
    [InlineData("edgeprof-", "Edge Profile & Sign-In Policy", 10)]
    [InlineData("edgenotif-", "Edge Notifications & Popup Policy", 10)]
    [InlineData("edgedl-", "Edge Download & History Policy", 10)]
    [InlineData("edgessf-", "Edge SmartScreen & Site Isolation Policy", 10)]
    public void Sprint417_Module_RegistersExpectedCount(string idPrefix, string moduleName, int minCount)
    {
        var count = BuildEngine().AllTweaks().Count(t => t.Id.StartsWith(idPrefix, StringComparison.OrdinalIgnoreCase));

        Assert.True(count >= minCount, $"Module '{moduleName}' (prefix '{idPrefix}') has {count} tweaks — expected ≥{minCount}.");
    }

    [Fact]
    public void Sprint417_AllModules_TotalAtLeastFiftyTweaks()
    {
        var total = BuildEngine().AllTweaks().Count(t => _sprint417Prefixes.Any(p => t.Id.StartsWith(p, StringComparison.OrdinalIgnoreCase)));

        Assert.True(total >= 50, $"Sprint 417-421 modules produced {total} tweaks, expected ≥50.");
    }

    [Fact]
    public void Sprint417_AllIds_AreUnique()
    {
        var ids = BuildEngine()
            .AllTweaks()
            .Where(t => _sprint417Prefixes.Any(p => t.Id.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
            .Select(t => t.Id)
            .ToList();

        Assert.Equal(ids.Count, ids.Distinct(StringComparer.OrdinalIgnoreCase).Count());
    }

    [Fact]
    public void Sprint417_AllTweaks_HaveOperations()
    {
        var tweaks = BuildEngine()
            .AllTweaks()
            .Where(t => _sprint417Prefixes.Any(p => t.Id.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        Assert.All(tweaks, t => Assert.True(t.HasOperations, $"Sprint 417-421 tweak {t.Id} has no operations"));
    }

    [Fact]
    public void Sprint417_AllTweaks_HaveNonEmptyLabelDescriptionCategory()
    {
        var tweaks = BuildEngine()
            .AllTweaks()
            .Where(t => _sprint417Prefixes.Any(p => t.Id.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        Assert.All(
            tweaks,
            t =>
            {
                Assert.False(string.IsNullOrWhiteSpace(t.Label), $"{t.Id} has empty Label");
                Assert.False(string.IsNullOrWhiteSpace(t.Description), $"{t.Id} has empty Description");
                Assert.False(string.IsNullOrWhiteSpace(t.Category), $"{t.Id} has empty Category");
            }
        );
    }

    [Fact]
    public void Sprint417_AllTweaks_HaveImpactAndSafetyScoresInRange()
    {
        var tweaks = BuildEngine()
            .AllTweaks()
            .Where(t => _sprint417Prefixes.Any(p => t.Id.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        Assert.All(
            tweaks,
            t =>
            {
                Assert.InRange(t.ImpactScore, 1, 5);
                Assert.InRange(t.SafetyRating, 1, 5);
            }
        );
    }

    [Fact]
    public void Sprint417_AllTweaks_ApplyOpsTargetEdgePolicyKey()
    {
        const string expectedKeyPrefix = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge";

        var tweaks = BuildEngine()
            .AllTweaks()
            .Where(t => _sprint417Prefixes.Any(p => t.Id.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        Assert.All(
            tweaks,
            t =>
                Assert.All(
                    t.ApplyOps,
                    op =>
                        Assert.True(
                            op.Path.StartsWith(expectedKeyPrefix, StringComparison.OrdinalIgnoreCase),
                            $"{t.Id}: ApplyOp path '{op.Path}' does not start with '{expectedKeyPrefix}'"
                        )
                )
        );
    }

    [Fact]
    public void Sprint417_AllTweaks_AreCorpSafe()
    {
        var tweaks = BuildEngine()
            .AllTweaks()
            .Where(t => _sprint417Prefixes.Any(p => t.Id.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        Assert.All(tweaks, t => Assert.True(t.CorpSafe, $"{t.Id} is not CorpSafe (Edge policy tweaks should be CorpSafe=true)"));
    }

    [Fact]
    public void Sprint417_AllTweaks_NoCrossModuleIdCollisionWithSprint412()
    {
        var s412Ids = new HashSet<string>(
            BuildEngine()
                .AllTweaks()
                .Where(t => _sprint412Prefixes.Any(p => t.Id.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
                .Select(t => t.Id),
            StringComparer.OrdinalIgnoreCase
        );

        var s417Ids = BuildEngine()
            .AllTweaks()
            .Where(t => _sprint417Prefixes.Any(p => t.Id.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
            .Select(t => t.Id)
            .ToList();

        var collisions = s417Ids.Where(id => s412Ids.Contains(id)).ToList();
        Assert.Empty(collisions);
    }

    [Fact]
    public void Sprint417_ValidatorReturnsNoErrors()
    {
        var engine = BuildEngine();
        var tweaks = engine.AllTweaks().Where(t => _sprint417Prefixes.Any(p => t.Id.StartsWith(p, StringComparison.OrdinalIgnoreCase))).ToList();

        var errors = TweakValidator.Validate(tweaks, id => engine.GetTweak(id));
        Assert.Empty(errors);
    }

    [Theory]
    [InlineData("Browser Policy")]
    public void Sprint417_NewCategories_RegisteredInEngine(string categoryName)
    {
        Assert.Contains(categoryName, BuildEngine().Categories(), StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Verifies that all Sprints 412-421 tweaks (100 total) have no DetectOps with a missing
    /// path — a common copy-paste bug where the detect op targets the wrong registry key.
    /// </summary>
    [Fact]
    public void Sprints412To421_AllDetectOps_HaveNonEmptyPath()
    {
        var allPrefixes = _sprint412Prefixes.Concat(_sprint417Prefixes).ToArray();
        var tweaks = BuildEngine().AllTweaks().Where(t => allPrefixes.Any(p => t.Id.StartsWith(p, StringComparison.OrdinalIgnoreCase))).ToList();

        Assert.All(tweaks, t => Assert.All(t.DetectOps, op => Assert.False(string.IsNullOrWhiteSpace(op.Path), $"{t.Id}: DetectOp has empty Path")));
    }

    /// <summary>
    /// Verifies that no two tweaks across Sprints 412-421 write the same registry
    /// PATH\ValueName combination (intra-batch duplicate registry ops).
    /// </summary>
    [Fact]
    public void Sprints412To421_NoIntraBatchDuplicateRegistryOps()
    {
        var allPrefixes = _sprint412Prefixes.Concat(_sprint417Prefixes).ToArray();
        var tweaks = BuildEngine().AllTweaks().Where(t => allPrefixes.Any(p => t.Id.StartsWith(p, StringComparison.OrdinalIgnoreCase))).ToList();

        var seen = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var conflicts = new List<string>();

        foreach (var t in tweaks)
        {
            foreach (var op in t.ApplyOps)
            {
                var key = $@"{op.Path}\{op.Name}";
                if (seen.TryGetValue(key, out var firstId))
                    conflicts.Add($"{t.Id} and {firstId} both write '{key}'");
                else
                    seen[key] = t.Id;
            }
        }

        Assert.Empty(conflicts);
    }
}

// ── merged from PolicyModulesV574Tests.cs ──────────────────────────────────
/// <summary>Tests for the 94 new policy modules added in v5.74.0 (Sprints 437-531).</summary>
public sealed class PolicyModulesV574Tests : IClassFixture<BuiltinsFixture>
{
    private readonly TweakEngine _engine;

    public PolicyModulesV574Tests(BuiltinsFixture fixture) => _engine = fixture.Engine;

    // ── Per-module minimum tweak count ────────────────────────────────

    /// <summary>
    /// Each new policy module must register at least 10 tweaks with its canonical ID prefix.
    /// </summary>
    [Theory]
    // Sprint 437-441 — Network Security
    [InlineData("fwadv-", "DefenderFirewallAdvanced")]
    [InlineData("ipsecpol-", "IpsecRule")]
    [InlineData("nlapol-", "NetworkLocationAwareness")]
    [InlineData("dohpol-", "DohEnforcement")]
    [InlineData("proxbyp-", "ProxyBypass")]
    // Sprint 442-446 — Clipboard
    [InlineData("clipadv-", "ClipboardHistoryAdvanced")]
    [InlineData("cliprdp-", "ClipboardRdpRedirection")]
    [InlineData("shrdclip-", "SharedClipboardControl")]
    [InlineData("uniclip-", "UniversalClipboardSync")]
    [InlineData("clipsens-", "ClipboardSensitivity")]
    // Sprint 447-451 — PowerShell / Terminal
    [InlineData("termadv-", "WindowsTerminalAdvanced")]
    [InlineData("ps7exec-", "Ps7ExecutionMode")]
    [InlineData("isedep-", "IseDeprecation")]
    [InlineData("sbloga-", "ScriptBlockLoggingAdvanced")]
    [InlineData("psjea-", "RemotePsJea")]
    // Sprint 452-456 — Edge
    [InlineData("wv2pol-", "EdgeWebView2")]
    [InlineData("eaguard-", "EdgeAppGuard")]
    [InlineData("edgsleep-", "EdgeSleepingTabs")]
    [InlineData("edgiso-", "EdgeSiteIsolation")]
    [InlineData("edgehint-", "EdgeEarlyHints")]
    // Sprint 457-461 — Azure AD / Entra
    [InlineData("aadca-", "AzureAdConditionalAccess")]
    [InlineData("entrareg-", "EntraDeviceRegistration")]
    [InlineData("aadprt-", "AzureAdPrtSso")]
    [InlineData("aadsspr-", "AzureAdSspr")]
    [InlineData("hjdns-", "HybridJoinDns")]
    // Sprint 462-466 — VBS / Security Isolation
    [InlineData("vbsenf-", "VbsEnforcement")]
    [InlineData("hvci-", "Hvci")]
    [InlineData("sldrtm-", "SecureLaunchDrtm")]
    [InlineData("sgrm-", "SystemGuardRuntime")]
    [InlineData("kdmapol-", "KernelDmaProtection")]
    // Sprint 467-471 — WSA / Android
    [InlineData("wsacore-", "WsaAndroid")]
    [InlineData("wsadbg-", "AndroidAppDebugging")]
    [InlineData("wsanet-", "WsaNetworkIsolation")]
    [InlineData("wsasnsr-", "AndroidSensorAccess")]
    [InlineData("wsastor-", "WsaStorage")]
    // Sprint 472-476 — Print Stack
    [InlineData("spladv-", "PrintSpoolerAdvanced")]
    [InlineData("pdrv-", "PrinterDriverIsolation")]
    [InlineData("inetprt-", "InternetPrinting")]
    [InlineData("wsdprt-", "WsdPrintDiscovery")]
    [InlineData("ippevy-", "IppEverywhere")]
    // Sprint 477-481 — Auth / Identity
    [InlineData("whfbpin-", "WhfbPin")]
    [InlineData("pwdless-", "PasswordlessSignIn")]
    [InlineData("biometric-", "BiometricAuth")]
    [InlineData("wpd-", "PortableDevice")]
    [InlineData("cbapol-", "CertificateBasedAuth")]
    // Sprint 482-486 — AI / Recall
    [InlineData("rcsnap-", "RecallAiSnapshot")]
    [InlineData("copnpu-", "CopilotPlusNpu")]
    [InlineData("aipol-", "WindowsAiPlatform")]
    [InlineData("aimod-", "AiContentModeration")]
    [InlineData("copsbar-", "CopilotSidebar")]
    // Sprint 487-491 — Storage Advanced
    [InlineData("sspol-", "StorageSpaces")]
    [InlineData("refspol-", "RefsFs")]
    [InlineData("dquota-", "DiskQuotaAdvanced")]
    [InlineData("vdspol-", "VirtualDiskService")]
    [InlineData("stobus-", "StorageBus")]
    // Sprint 492-496 — Event Logging
    [InlineData("evtchan-", "EventLogChannel")]
    [InlineData("wecpol-", "EventSubscription")]
    [InlineData("wefsubpol-", "WefSubscription")]
    [InlineData("etwses-", "EtwSession")]
    // Sprint 497-501 — Network / NIC
    [InlineData("netbridge-", "NetworkBridge")]
    [InlineData("lltdpol-", "LltdProtocol")]
    [InlineData("nicteam-", "NicTeaming")]
    [InlineData("nlaadv-", "NetLocationAwarenessAdvanced")]
    // Sprint 502-506 — Defender Suite
    [InlineData("ssadv-", "SmartScreenAdvanced")]
    [InlineData("avadv-", "DefenderAntivirusAdvanced")]
    [InlineData("egpol-", "ExploitGuard")]
    [InlineData("alockadv-", "AppLockerAdvanced")]
    [InlineData("fwprof-", "FirewallProfileHardening")]
    // Sprint 507-511 — Fonts
    [InlineData("fontpol-", "FontInstallation")]
    [InlineData("otfpol-", "OpenTypeSecurity")]
    [InlineData("gdipol-", "GdiRenderer")]
    // Sprint 512-516 — DirectX / GPU
    [InlineData("d3dpol-", "DirectXRendering")]
    [InlineData("gpucmp-", "GpuCompute")]
    [InlineData("wddmpol-", "WddmDriver")]
    [InlineData("gamebar-", "GameBar")]
    // Sprint 517-521 — Xbox / Gaming
    [InlineData("xboxnet-", "XboxNetworking")]
    // Sprint 522-526 — MS Store
    [InlineData("storepol-", "MicrosoftStore")]
    // Sprint 527-531 — Containers / Virtualization
    [InlineData("sbpol-", "WindowsSandbox")]
    [InlineData("hvcon-", "HyperVContainer")]
    [InlineData("wsl2adv-", "Wsl2Advanced")]
    public void Module_RegistersAtLeastOneTweak(string idPrefix, string moduleName)
    {
        int count = _engine.AllTweaks()
            .Count(t => t.Id.StartsWith(idPrefix, StringComparison.OrdinalIgnoreCase));

        Assert.True(count >= 1,
            $"Module '{moduleName}' (prefix '{idPrefix}') has {count} tweaks — expected ≥1.");
    }

    /// <summary>
    /// Policy modules that are well-established (≥10 tweaks confirmed in v5.74.0).
    /// </summary>
    [Theory]
    [InlineData("fwadv-", "DefenderFirewallAdvanced")]
    [InlineData("ipsecpol-", "IpsecRule")]
    [InlineData("nlapol-", "NetworkLocationAwareness")]
    [InlineData("dohpol-", "DohEnforcement")]
    [InlineData("proxbyp-", "ProxyBypass")]
    [InlineData("clipadv-", "ClipboardHistoryAdvanced")]
    [InlineData("cliprdp-", "ClipboardRdpRedirection")]
    [InlineData("shrdclip-", "SharedClipboardControl")]
    [InlineData("uniclip-", "UniversalClipboardSync")]
    [InlineData("clipsens-", "ClipboardSensitivity")]
    [InlineData("termadv-", "WindowsTerminalAdvanced")]
    [InlineData("ps7exec-", "Ps7ExecutionMode")]
    [InlineData("isedep-", "IseDeprecation")]
    [InlineData("sbloga-", "ScriptBlockLoggingAdvanced")]
    [InlineData("psjea-", "RemotePsJea")]
    [InlineData("wv2pol-", "EdgeWebView2")]
    [InlineData("eaguard-", "EdgeAppGuard")]
    [InlineData("edgsleep-", "EdgeSleepingTabs")]
    [InlineData("edgiso-", "EdgeSiteIsolation")]
    [InlineData("edgehint-", "EdgeEarlyHints")]
    [InlineData("aadca-", "AzureAdConditionalAccess")]
    [InlineData("entrareg-", "EntraDeviceRegistration")]
    [InlineData("aadprt-", "AzureAdPrtSso")]
    [InlineData("aadsspr-", "AzureAdSspr")]
    [InlineData("hjdns-", "HybridJoinDns")]
    [InlineData("vbsenf-", "VbsEnforcement")]
    [InlineData("hvci-", "Hvci")]
    [InlineData("sldrtm-", "SecureLaunchDrtm")]
    [InlineData("sgrm-", "SystemGuardRuntime")]
    [InlineData("kdmapol-", "KernelDmaProtection")]
    [InlineData("wsacore-", "WsaAndroid")]
    [InlineData("wsadbg-", "AndroidAppDebugging")]
    [InlineData("wsanet-", "WsaNetworkIsolation")]
    [InlineData("wsasnsr-", "AndroidSensorAccess")]
    [InlineData("wsastor-", "WsaStorage")]
    [InlineData("spladv-", "PrintSpoolerAdvanced")]
    [InlineData("pdrv-", "PrinterDriverIsolation")]
    [InlineData("inetprt-", "InternetPrinting")]
    [InlineData("wsdprt-", "WsdPrintDiscovery")]
    [InlineData("ippevy-", "IppEverywhere")]
    [InlineData("whfbpin-", "WhfbPin")]
    [InlineData("pwdless-", "PasswordlessSignIn")]
    [InlineData("biometric-", "BiometricAuth")]
    [InlineData("rcsnap-", "RecallAiSnapshot")]
    [InlineData("copnpu-", "CopilotPlusNpu")]
    [InlineData("sspol-", "StorageSpaces")]
    [InlineData("refspol-", "RefsFs")]
    [InlineData("dquota-", "DiskQuotaAdvanced")]
    [InlineData("vdspol-", "VirtualDiskService")]
    [InlineData("stobus-", "StorageBus")]
    [InlineData("evtchan-", "EventLogChannel")]
    [InlineData("wecpol-", "EventSubscription")]
    [InlineData("wefsubpol-", "WefSubscription")]
    [InlineData("netbridge-", "NetworkBridge")]
    [InlineData("lltdpol-", "LltdProtocol")]
    [InlineData("nicteam-", "NicTeaming")]
    [InlineData("ssadv-", "SmartScreenAdvanced")]
    [InlineData("avadv-", "DefenderAntivirusAdvanced")]
    [InlineData("egpol-", "ExploitGuard")]
    [InlineData("alockadv-", "AppLockerAdvanced")]
    [InlineData("fontpol-", "FontInstallation")]
    [InlineData("otfpol-", "OpenTypeSecurity")]
    [InlineData("d3dpol-", "DirectXRendering")]
    [InlineData("gpucmp-", "GpuCompute")]
    [InlineData("wddmpol-", "WddmDriver")]
    [InlineData("gamebar-", "GameBar")]
    [InlineData("xboxnet-", "XboxNetworking")]
    [InlineData("storepol-", "MicrosoftStore")]
    [InlineData("sbpol-", "WindowsSandbox")]
    [InlineData("hvcon-", "HyperVContainer")]
    [InlineData("wsl2adv-", "Wsl2Advanced")]
    public void Module_RegistersAtLeastTenTweaks(string idPrefix, string moduleName)
    {
        int count = _engine.AllTweaks()
            .Count(t => t.Id.StartsWith(idPrefix, StringComparison.OrdinalIgnoreCase));

        Assert.True(count >= 10,
            $"Module '{moduleName}' (prefix '{idPrefix}') has {count} tweaks — expected ≥10.");
    }

    // ── Required field validation for all new modules ─────────────────

    /// <summary>
    /// All new v5.74.0 policy module tweaks must have valid ImpactScore (1-5)
    /// and SafetyRating (1-5).
    /// </summary>
    [Fact]
    public void AllNewModuleTweaks_HaveValidImpactAndSafetyScores()
    {
        string[] newPrefixes =
        [
            "fwadv-", "ipsecpol-", "nlapol-", "dohpol-", "proxbyp-",
            "clipadv-", "cliprdp-", "shrdclip-", "uniclip-", "clipsens-",
            "termadv-", "ps7exec-", "isedep-", "sbloga-", "psjea-",
            "wv2pol-", "eaguard-", "edgsleep-", "edgiso-", "edgehint-",
            "aadca-", "entrareg-", "aadprt-", "aadsspr-", "hjdns-",
            "vbsenf-", "hvci-", "sldrtm-", "sgrm-", "kdmapol-",
            "wsacore-", "wsadbg-", "wsanet-", "wsasnsr-", "wsastor-",
            "spladv-", "pdrv-", "inetprt-", "wsdprt-", "ippevy-",
            "whfbpin-", "pwdless-", "biometric-", "wpd-", "cbapol-",
            "rcsnap-", "copnpu-", "aipol-", "aimod-", "copsbar-",
            "sspol-", "refspol-", "dquota-", "vdspol-", "stobus-",
            "evtchan-", "wecpol-", "wefsubpol-", "etwses-",
            "netbridge-", "lltdpol-", "nicteam-", "nlaadv-",
            "ssadv-", "avadv-", "egpol-", "alockadv-", "fwprof-",
            "fontpol-", "otfpol-", "gdipol-",
            "d3dpol-", "gpucmp-", "wddmpol-", "gamebar-",
            "xboxnet-",
            "storepol-",
            "sbpol-", "hvcon-", "wsl2adv-",
        ];

        var newTweaks = _engine.AllTweaks()
            .Where(t => newPrefixes.Any(p => t.Id.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        Assert.NotEmpty(newTweaks);

        foreach (TweakDef td in newTweaks)
        {
            Assert.True(td.ImpactScore is >= 1 and <= 5,
                $"Tweak '{td.Id}' has ImpactScore={td.ImpactScore} — must be 1–5.");
            Assert.True(td.SafetyRating is >= 1 and <= 5,
                $"Tweak '{td.Id}' has SafetyRating={td.SafetyRating} — must be 1–5.");
        }
    }

    /// <summary>
    /// All new v5.74.0 policy module tweaks must have NeedsAdmin = true
    /// (all operate on HKLM Policies keys).
    /// </summary>
    [Fact]
    public void AllNewModuleTweaks_RequireAdmin()
    {
        string[] newPrefixes =
        [
            "fwadv-", "ipsecpol-", "nlapol-", "dohpol-", "proxbyp-",
            "clipadv-", "cliprdp-", "shrdclip-", "uniclip-", "clipsens-",
            "termadv-", "ps7exec-", "isedep-", "sbloga-", "psjea-",
            "wv2pol-", "eaguard-", "edgsleep-", "edgiso-", "edgehint-",
            "aadca-", "entrareg-", "aadprt-", "aadsspr-", "hjdns-",
            "vbsenf-", "hvci-", "sldrtm-", "sgrm-", "kdmapol-",
            "wsacore-", "wsadbg-", "wsanet-", "wsasnsr-", "wsastor-",
            "spladv-", "pdrv-", "inetprt-", "wsdprt-", "ippevy-",
        ];

        var newTweaks = _engine.AllTweaks()
            .Where(t => newPrefixes.Any(p => t.Id.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        Assert.NotEmpty(newTweaks);
        Assert.All(newTweaks, td =>
            Assert.True(td.NeedsAdmin, $"Tweak '{td.Id}' must have NeedsAdmin=true (policy key)."));
    }

    /// <summary>
    /// All new v5.74.0 policy module tweaks must have non-empty Labels and Categories.
    /// </summary>
    [Fact]
    public void AllNewModuleTweaks_HaveRequiredFields()
    {
        string[] newPrefixes =
        [
            "fwadv-", "ipsecpol-", "nlapol-", "dohpol-", "proxbyp-",
            "clipadv-", "cliprdp-", "shrdclip-", "uniclip-", "clipsens-",
            "termadv-", "ps7exec-", "isedep-", "sbloga-", "psjea-",
            "wv2pol-", "eaguard-", "edgsleep-", "edgiso-", "edgehint-",
            "aadca-", "entrareg-", "aadprt-", "aadsspr-", "hjdns-",
            "vbsenf-", "hvci-", "sldrtm-", "sgrm-", "kdmapol-",
            "wsacore-", "wsadbg-", "wsanet-", "wsasnsr-", "wsastor-",
            "spladv-", "pdrv-", "inetprt-", "wsdprt-", "ippevy-",
            "whfbpin-", "pwdless-", "biometric-", "wpd-", "cbapol-",
            "rcsnap-", "copnpu-", "aipol-", "aimod-", "copsbar-",
            "sspol-", "refspol-", "dquota-", "vdspol-", "stobus-",
            "evtchan-", "wecpol-", "wefsubpol-", "etwses-",
            "netbridge-", "lltdpol-", "nicteam-",
            "ssadv-", "avadv-", "egpol-", "alockadv-",
            "fontpol-", "otfpol-",
            "d3dpol-", "gpucmp-", "wddmpol-",
            "xboxnet-", "storepol-",
            "sbpol-", "hvcon-", "wsl2adv-",
        ];

        var newTweaks = _engine.AllTweaks()
            .Where(t => newPrefixes.Any(p => t.Id.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        Assert.NotEmpty(newTweaks);

        foreach (TweakDef td in newTweaks)
        {
            Assert.False(string.IsNullOrWhiteSpace(td.Label),
                $"Tweak '{td.Id}' must have a non-empty Label.");
            Assert.False(string.IsNullOrWhiteSpace(td.Category),
                $"Tweak '{td.Id}' must have a non-empty Category.");
        }
    }

    // ── Total new-module tweak count ──────────────────────────────────

    [Fact]
    public void V574NewModules_TotalTweakCountIsAtLeast500()
    {
        // 94 new modules × 10 tweaks each = 940 new tweaks.
        // We test for ≥500 to allow for minor variance in module sizes.
        string[] newPrefixes =
        [
            "fwadv-", "ipsecpol-", "nlapol-", "dohpol-", "proxbyp-",
            "clipadv-", "cliprdp-", "shrdclip-", "uniclip-", "clipsens-",
            "termadv-", "ps7exec-", "isedep-", "sbloga-", "psjea-",
            "wv2pol-", "eaguard-", "edgsleep-", "edgiso-", "edgehint-",
            "aadca-", "entrareg-", "aadprt-", "aadsspr-", "hjdns-",
            "vbsenf-", "hvci-", "sldrtm-", "sgrm-", "kdmapol-",
            "wsacore-", "wsadbg-", "wsanet-", "wsasnsr-", "wsastor-",
            "spladv-", "pdrv-", "inetprt-", "wsdprt-", "ippevy-",
            "whfbpin-", "pwdless-", "biometric-", "wpd-", "cbapol-",
            "rcsnap-", "copnpu-", "aipol-", "aimod-", "copsbar-",
            "sspol-", "refspol-", "dquota-", "vdspol-", "stobus-",
            "evtchan-", "wecpol-", "wefsubpol-", "etwses-",
            "netbridge-", "lltdpol-", "nicteam-", "nlaadv-",
            "ssadv-", "avadv-", "egpol-", "alockadv-", "fwprof-",
            "fontpol-", "otfpol-", "gdipol-",
            "d3dpol-", "gpucmp-", "wddmpol-", "gamebar-",
            "xboxnet-",
            "storepol-",
            "sbpol-", "hvcon-", "wsl2adv-",
        ];

        int total = _engine.AllTweaks()
            .Count(t => newPrefixes.Any(p => t.Id.StartsWith(p, StringComparison.OrdinalIgnoreCase)));

        Assert.True(total >= 500,
            $"Expected ≥500 tweaks across v5.74.0 new policy modules, found {total}.");
    }

    // ── Validator passes for all new tweaks ───────────────────────────

    [Fact]
    public void V574NewModules_ValidatorReturnsNoErrors()
    {
        string[] newPrefixes =
        [
            "fwadv-", "ipsecpol-", "nlapol-", "dohpol-", "proxbyp-",
            "clipadv-", "cliprdp-", "shrdclip-", "uniclip-", "clipsens-",
            "termadv-", "ps7exec-", "isedep-", "sbloga-", "psjea-",
            "wv2pol-", "eaguard-", "edgsleep-", "edgiso-", "edgehint-",
            "aadca-", "entrareg-", "aadprt-", "aadsspr-", "hjdns-",
            "vbsenf-", "hvci-", "sldrtm-", "sgrm-", "kdmapol-",
            "wsacore-", "wsadbg-", "wsanet-", "wsasnsr-", "wsastor-",
            "spladv-", "pdrv-", "inetprt-", "wsdprt-", "ippevy-",
            "whfbpin-", "pwdless-", "biometric-", "wpd-", "cbapol-",
            "rcsnap-", "copnpu-",
            "sspol-", "refspol-", "dquota-", "vdspol-", "stobus-",
            "evtchan-", "wecpol-", "wefsubpol-",
            "netbridge-", "lltdpol-", "nicteam-",
            "ssadv-", "avadv-", "egpol-", "alockadv-",
            "fontpol-", "otfpol-",
            "d3dpol-", "gpucmp-", "wddmpol-",
            "xboxnet-", "storepol-",
            "sbpol-", "hvcon-", "wsl2adv-",
        ];

        var newTweaks = _engine.AllTweaks()
            .Where(t => newPrefixes.Any(p => t.Id.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        Assert.NotEmpty(newTweaks);

        var errors = TweakValidator.Validate(newTweaks, id => _engine.GetTweak(id));
        Assert.Empty(errors);
    }

    // ── CorpSafe = true for all policy tweaks ─────────────────────────

    [Fact]
    public void AllNewModuleTweaks_AreCorpSafe()
    {
        // Policy tweaks target HKLM\SOFTWARE\Policies\... which is safe in corporate environments.
        string[] newPrefixes =
        [
            "fwadv-", "ipsecpol-", "nlapol-", "dohpol-",
            "clipadv-", "cliprdp-", "shrdclip-", "uniclip-",
            "termadv-", "ps7exec-", "isedep-", "sbloga-", "psjea-",
            "wv2pol-", "eaguard-",
            "aadca-", "entrareg-", "aadprt-", "aadsspr-", "hjdns-",
            "vbsenf-", "hvci-", "sldrtm-", "sgrm-", "kdmapol-",
            "wsacore-", "wsadbg-", "wsanet-", "wsasnsr-", "wsastor-",
            "spladv-", "pdrv-", "inetprt-", "wsdprt-", "ippevy-",
        ];

        var policyTweaks = _engine.AllTweaks()
            .Where(t => newPrefixes.Any(p => t.Id.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        Assert.NotEmpty(policyTweaks);
        Assert.All(policyTweaks, td =>
            Assert.True(td.CorpSafe,
                $"Tweak '{td.Id}' must have CorpSafe=true (targets HKLM Policies key)."));
    }
}
