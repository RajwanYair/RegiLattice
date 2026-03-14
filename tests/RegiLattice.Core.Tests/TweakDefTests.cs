using RegiLattice.Core;
using RegiLattice.Core.Models;
using RegiLattice.Core.Registry;
using Xunit;

namespace RegiLattice.Core.Tests;

/// <summary>Tests for TweakDef model: scope computation, default values, factory methods.</summary>
public sealed class TweakDefTests
{
    private static TweakDef Make(params string[] keys) =>
        new()
        {
            Id = "test-id",
            Label = "Test Tweak",
            Category = "Test",
            RegistryKeys = keys,
        };

    // ── Scope computation ──────────────────────────────────────────────────
    [Fact]
    public void Scope_HkcuKey_ReturnsUser()
    {
        var td = Make(@"HKEY_CURRENT_USER\Software\Test");
        Assert.Equal(TweakScope.User, td.Scope);
    }

    [Fact]
    public void Scope_HklmKey_ReturnsMachine()
    {
        var td = Make(@"HKEY_LOCAL_MACHINE\SOFTWARE\Test");
        Assert.Equal(TweakScope.Machine, td.Scope);
    }

    [Fact]
    public void Scope_BothKeys_ReturnsBoth()
    {
        var td = Make(@"HKCU\Software\Test", @"HKLM\SOFTWARE\Test");
        Assert.Equal(TweakScope.Both, td.Scope);
    }

    [Fact]
    public void Scope_HkcrKey_ReturnsMachine()
    {
        var td = Make(@"HKEY_CLASSES_ROOT\*\shell\test");
        Assert.Equal(TweakScope.Machine, td.Scope);
    }

    [Fact]
    public void Scope_NoKeys_NotAdmin_ReturnsUser()
    {
        var td = new TweakDef
        {
            Id = "t",
            Label = "T",
            Category = "C",
            NeedsAdmin = false,
        };
        Assert.Equal(TweakScope.User, td.Scope);
    }

    [Fact]
    public void Scope_NoKeys_NeedsAdmin_ReturnsMachine()
    {
        var td = Make(); // NeedsAdmin defaults to true
        Assert.Equal(TweakScope.Machine, td.Scope);
    }

    // ── Default values ─────────────────────────────────────────────────────
    [Fact]
    public void Defaults_NeedsAdmin_True()
    {
        var td = Make();
        Assert.True(td.NeedsAdmin);
    }

    [Fact]
    public void Defaults_CorpSafe_False()
    {
        var td = Make();
        Assert.False(td.CorpSafe);
    }

    [Fact]
    public void Defaults_MinBuild_Zero()
    {
        var td = Make();
        Assert.Equal(0, td.MinBuild);
    }

    [Fact]
    public void Defaults_EmptyCollections()
    {
        var td = Make();
        Assert.Empty(td.Tags);
        Assert.Empty(td.DependsOn);
        Assert.Empty(td.ApplyOps);
        Assert.Empty(td.RemoveOps);
        Assert.Empty(td.DetectOps);
    }

    [Fact]
    public void ToString_ContainsIdAndCategory()
    {
        var td = Make();
        Assert.Contains("test-id", td.ToString());
        Assert.Contains("Test", td.ToString());
    }
}

/// <summary>Tests for RegOp factory methods.</summary>
public sealed class RegOpTests
{
    [Fact]
    public void SetDword_CreatesCorrectOp()
    {
        var op = RegOp.SetDword(@"HKLM\Test", "Value", 42);
        Assert.Equal(RegOpKind.SetValue, op.Kind);
        Assert.Equal(@"HKLM\Test", op.Path);
        Assert.Equal("Value", op.Name);
        Assert.Equal(42, op.Value);
        Assert.Equal(Microsoft.Win32.RegistryValueKind.DWord, op.ValueKind);
    }

    [Fact]
    public void SetString_CreatesCorrectOp()
    {
        var op = RegOp.SetString(@"HKCU\Test", "Name", "hello");
        Assert.Equal(RegOpKind.SetValue, op.Kind);
        Assert.Equal("hello", op.Value);
        Assert.Equal(Microsoft.Win32.RegistryValueKind.String, op.ValueKind);
    }

    [Fact]
    public void DeleteValue_CreatesCorrectOp()
    {
        var op = RegOp.DeleteValue(@"HKLM\Test", "Val");
        Assert.Equal(RegOpKind.DeleteValue, op.Kind);
        Assert.Equal("Val", op.Name);
    }

    [Fact]
    public void DeleteTree_CreatesCorrectOp()
    {
        var op = RegOp.DeleteTree(@"HKLM\Test\Sub");
        Assert.Equal(RegOpKind.DeleteTree, op.Kind);
        Assert.Equal(@"HKLM\Test\Sub", op.Path);
    }

    [Fact]
    public void CheckDword_CreatesCorrectOp()
    {
        var op = RegOp.CheckDword(@"HKLM\Test", "Val", 1);
        Assert.Equal(RegOpKind.CheckValue, op.Kind);
        Assert.Equal(1, op.Value);
    }

    [Fact]
    public void CheckMissing_CreatesCorrectOp()
    {
        var op = RegOp.CheckMissing(@"HKLM\Test", "Val");
        Assert.Equal(RegOpKind.CheckMissing, op.Kind);
    }

    [Fact]
    public void CheckKeyMissing_CreatesCorrectOp()
    {
        var op = RegOp.CheckKeyMissing(@"HKLM\Test\Sub");
        Assert.Equal(RegOpKind.CheckKeyMissing, op.Kind);
    }
}
