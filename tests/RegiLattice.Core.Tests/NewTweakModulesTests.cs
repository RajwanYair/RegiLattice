// tests/RegiLattice.Core.Tests/NewTweakModulesTests.cs
// Sprint 63 — Validate the 5 new tweak modules (XboxGameBar, WindowsHello,
// SmartAppControl, EnergySaver, CopilotPlus) are registered correctly.

using RegiLattice.Core;
using RegiLattice.Core.Models;
using Xunit;

namespace RegiLattice.Core.Tests;

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
    [InlineData("Xbox / Game Bar")]
    [InlineData("Windows Hello")]
    [InlineData("Smart App Control")]
    [InlineData("Energy Saver")]
    [InlineData("Copilot+ Features")]
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
    [InlineData("BitLocker Advanced")]
    [InlineData("Application Control Policy")]
    [InlineData("Hyper-V Advanced")]
    [InlineData("Windows Sandbox")]
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
