using RegiLattice.Core;
using RegiLattice.Core.Models;
using RegiLattice.Core.Tweaks;
using System.Reflection;
using Xunit;

namespace RegiLattice.Core.Tests;

/// <summary>Tests for the B.2 <see cref="TweakModuleAttribute"/> attribute and attribute-based module discovery.</summary>
public sealed class TweakModuleAttributeTests
{
    // ── Attribute metadata ─────────────────────────────────────────────

    [Fact]
    public void TweakModuleAttribute_HasCorrectAttributeUsage()
    {
        var au = typeof(TweakModuleAttribute)
            .GetCustomAttribute<AttributeUsageAttribute>();
        Assert.NotNull(au);
        Assert.Equal(AttributeTargets.Class, au.ValidOn);
        Assert.False(au.Inherited);
        Assert.False(au.AllowMultiple);
    }

    [Fact]
    public void TweakModuleAttribute_IsSealed()
    {
        Assert.True(typeof(TweakModuleAttribute).IsSealed);
    }

    // ── Well-known modules carry the attribute ─────────────────────────

    [Theory]
    [InlineData(typeof(Accessibility))]
    [InlineData(typeof(Gaming))]
    [InlineData(typeof(Privacy))]
    [InlineData(typeof(Security))]
    [InlineData(typeof(Power))]
    public void KnownModule_HasTweakModuleAttribute(Type moduleType)
    {
        Assert.NotNull(moduleType.GetCustomAttribute<TweakModuleAttribute>());
    }

    // ── Attribute-based modules are reachable after RegisterBuiltins ───

    [Fact]
    public void RegisterBuiltins_AttributeDecoratedModules_AreIncludedInAllTweaks()
    {
        var bindingFlags = BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;

        // Collect IDs that come specifically from attribute-decorated modules.
        var attrModuleIds = typeof(TweakModuleAttribute).Assembly
            .GetTypes()
            .Where(t => t.GetCustomAttribute<TweakModuleAttribute>() != null)
            .SelectMany(t =>
            {
                var prop = t.GetProperty("Tweaks", bindingFlags);
                return prop?.GetValue(null) is IReadOnlyList<TweakDef> list
                    ? list.Select(td => td.Id)
                    : (IEnumerable<string>)[];
            })
            .ToHashSet(StringComparer.Ordinal);

        Assert.NotEmpty(attrModuleIds);

        var engine = new TweakEngine();
        engine.RegisterBuiltins();

        // Spot-check: every ID from an attribute-decorated module is accessible.
        foreach (var id in attrModuleIds.Take(10))
            Assert.NotNull(engine.GetTweak(id));
    }
}
