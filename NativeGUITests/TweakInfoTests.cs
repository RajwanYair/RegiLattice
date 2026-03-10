using System.Collections.Generic;
using RegiLattice.Native.Models;
using Xunit;

namespace NativeGUITests;

/// <summary>Unit tests for TweakInfo model logic.</summary>
public sealed class TweakInfoTests
{
    private static TweakInfo Make(string status, params string[] keys) => new()
    {
        Id           = "test-id",
        Label        = "Test Tweak",
        Category     = "Test",
        Status       = status,
        RegistryKeys = keys,
    };

    // ── IsApplied ──────────────────────────────────────────────────────────
    [Fact]
    public void IsApplied_WhenStatusApplied_ReturnsTrue()
        => Assert.True(Make("applied").IsApplied);

    [Fact]
    public void IsApplied_WhenStatusNotApplied_ReturnsFalse()
        => Assert.False(Make("not_applied").IsApplied);

    [Fact]
    public void IsApplied_WhenStatusUnknown_ReturnsFalse()
        => Assert.False(Make("unknown").IsApplied);

    // ── ScopeBadge ────────────────────────────────────────────────────────
    [Fact]
    public void ScopeBadge_HkcuKey_ReturnsUser()
    {
        var tw = Make("unknown", @"HKEY_CURRENT_USER\Software\Test");
        Assert.Equal("USER", tw.ScopeBadge);
    }

    [Fact]
    public void ScopeBadge_HklmKey_ReturnsMachine()
    {
        var tw = Make("unknown", @"HKEY_LOCAL_MACHINE\SOFTWARE\Test");
        Assert.Equal("MACHINE", tw.ScopeBadge);
    }

    [Fact]
    public void ScopeBadge_BothKeys_ReturnsBoth()
    {
        var tw = Make("unknown",
            @"HKEY_CURRENT_USER\Software\Test",
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Test");
        Assert.Equal("BOTH", tw.ScopeBadge);
    }

    [Fact]
    public void ScopeBadge_HkcuAbbreviation_ReturnsUser()
    {
        var tw = Make("unknown", @"HKCU\Software\Test");
        Assert.Equal("USER", tw.ScopeBadge);
    }

    [Fact]
    public void ScopeBadge_HklmAbbreviation_ReturnsMachine()
    {
        var tw = Make("unknown", @"HKLM\SOFTWARE\Test");
        Assert.Equal("MACHINE", tw.ScopeBadge);
    }

    [Fact]
    public void ScopeBadge_HkcrKey_ReturnsMachine()
    {
        var tw = Make("unknown", @"HKEY_CLASSES_ROOT\*\shell\test");
        Assert.Equal("MACHINE", tw.ScopeBadge);
    }

    [Fact]
    public void ScopeBadge_NoKeys_ReturnsMachine()
    {
        var tw = Make("unknown");
        Assert.Equal("MACHINE", tw.ScopeBadge);
    }

    // ── Defaults ──────────────────────────────────────────────────────────
    [Fact]
    public void DefaultStatus_IsUnknown()
    {
        var tw = new TweakInfo();
        Assert.Equal("unknown", tw.Status);
    }

    [Fact]
    public void RegistryKeys_DefaultsToEmpty()
    {
        var tw = new TweakInfo();
        Assert.Empty(tw.RegistryKeys);
    }
}
