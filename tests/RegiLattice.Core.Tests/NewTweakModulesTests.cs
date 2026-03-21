// tests/RegiLattice.Core.Tests/NewTweakModulesTests.cs
// Sprint 63 — Validate the 5 new tweak modules (XboxGameBar, WindowsHello,
// SmartAppControl, EnergySaver, CopilotPlus) are registered correctly.

using RegiLattice.Core;
using RegiLattice.Core.Models;
using Xunit;

namespace RegiLattice.Core.Tests;

/// <summary>Tests for Sprint 63: 5 new tweak modules (50 new tweaks total).</summary>
public sealed class NewTweakModulesTests
{
    private static TweakEngine BuildEngine()
    {
        var engine = new TweakEngine();
        engine.RegisterBuiltins();
        return engine;
    }

    // ── Per-module registration count ────────────────────────────────────

    [Theory]
    [InlineData("xbgb-",   "Xbox Game Bar")]
    [InlineData("hello-",  "Windows Hello")]
    [InlineData("sac-",    "Smart App Control")]
    [InlineData("energy-", "Energy Saver")]
    [InlineData("cplplus-", "Copilot+")]
    public void Module_RegistersAtLeastTenTweaks(string idPrefix, string moduleName)
    {
        var engine = BuildEngine();
        var count  = engine.AllTweaks().Count(t => t.Id.StartsWith(idPrefix, StringComparison.OrdinalIgnoreCase));

        Assert.True(count >= 10,
            $"Module '{moduleName}' (prefix '{idPrefix}') has only {count} tweaks — expected ≥10.");
    }

    // ── Total new-tweak count ────────────────────────────────────────────

    [Fact]
    public void NewModules_TotalAtLeastFiftyNewTweaks()
    {
        var engine  = BuildEngine();
        var prefixes = new[] { "xbgb-", "hello-", "sac-", "energy-", "cplplus-" };
        var total   = engine.AllTweaks()
                            .Count(t => prefixes.Any(p => t.Id.StartsWith(p, StringComparison.OrdinalIgnoreCase)));

        Assert.True(total >= 50,
            $"Expected ≥50 new tweaks across the 5 Sprint 63 modules, but found {total}.");
    }

    // ── All new IDs are globally unique ──────────────────────────────────

    [Fact]
    public void NewModules_AllIdsAreUnique()
    {
        var engine   = BuildEngine();
        var prefixes = new[] { "xbgb-", "hello-", "sac-", "energy-", "cplplus-" };
        var newIds   = engine.AllTweaks()
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
        var engine   = BuildEngine();
        var prefixes = new[] { "xbgb-", "hello-", "sac-", "energy-", "cplplus-" };
        var newTweaks = engine.AllTweaks()
                              .Where(t => prefixes.Any(p => t.Id.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
                              .ToList();

        var errors = TweakValidator.Validate(newTweaks, id => engine.GetTweak(id));
        Assert.Empty(errors);
    }

    // ── Each module has non-empty Labels and Categories ──────────────────

    [Fact]
    public void NewModules_AllTweaks_HaveNonEmptyLabelAndCategory()
    {
        var engine   = BuildEngine();
        var prefixes = new[] { "xbgb-", "hello-", "sac-", "energy-", "cplplus-" };
        var newTweaks = engine.AllTweaks()
                              .Where(t => prefixes.Any(p => t.Id.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
                              .ToList();

        Assert.All(newTweaks, t =>
        {
            Assert.False(string.IsNullOrWhiteSpace(t.Label),    $"Tweak {t.Id} has empty Label");
            Assert.False(string.IsNullOrWhiteSpace(t.Category), $"Tweak {t.Id} has empty Category");
        });
    }

    // ── HasOperations gate ───────────────────────────────────────────────

    [Fact]
    public void NewModules_AllTweaks_HaveOperations()
    {
        var engine   = BuildEngine();
        var prefixes = new[] { "xbgb-", "hello-", "sac-", "energy-", "cplplus-" };

        // Engine.Register() only accepts tweaks with HasOperations == true.
        // If they are in AllTweaks(), they already passed the gate.
        var newTweaks = engine.AllTweaks()
                              .Where(t => prefixes.Any(p => t.Id.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
                              .ToList();

        Assert.All(newTweaks, t =>
            Assert.True(t.HasOperations, $"Tweak {t.Id} has no operations (HasOperations == false)"));
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
        var engine     = BuildEngine();
        var categories = engine.Categories();

        Assert.Contains(categoryName, categories,
            StringComparer.OrdinalIgnoreCase);
    }
}
