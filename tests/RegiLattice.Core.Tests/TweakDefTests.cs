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

/// <summary>Tests for TweakDef.GetExpectedResult() auto-generation logic.</summary>
public sealed class ExpectedResultTests
{
    // ── ExpectedResult ─────────────────────────────────────────────────────

    [Fact]
    public void GetExpectedResult_ExplicitValue_ReturnsExplicit()
    {
        var td = new TweakDef
        {
            Id = "test-explicit",
            Label = "Disable Something",
            Category = "Test",
            ExpectedResult = "Custom expected result text.",
        };
        Assert.Equal("Custom expected result text.", td.GetExpectedResult());
    }

    [Fact]
    public void GetExpectedResult_DisableLabel_ContainsTurnedOff()
    {
        var td = new TweakDef
        {
            Id = "test-disable",
            Label = "Disable Windows Telemetry",
            Category = "Privacy",
        };
        string result = td.GetExpectedResult();
        Assert.Contains("turned off", result);
        Assert.Contains("Windows Telemetry", result);
        Assert.Contains("privacy", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetExpectedResult_EnableLabel_ContainsActivated()
    {
        var td = new TweakDef
        {
            Id = "test-enable",
            Label = "Enable Hardware-Accelerated GPU Scheduling",
            Category = "Gaming",
        };
        string result = td.GetExpectedResult();
        Assert.Contains("activated", result);
    }

    [Fact]
    public void GetExpectedResult_UnknownVerb_FallsBackToLabel()
    {
        var td = new TweakDef
        {
            Id = "test-custom",
            Label = "Tweak Some Setting",
            Category = "Test",
        };
        string result = td.GetExpectedResult();
        Assert.Contains("Tweak Some Setting", result);
        Assert.Contains("applied", result);
    }

    [Fact]
    public void GetExpectedResult_MachineTweak_MentionsRestart()
    {
        var td = new TweakDef
        {
            Id = "test-restart",
            Label = "Disable Service",
            Category = "Performance",
            NeedsAdmin = true,
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\Services\Test"],
        };
        string result = td.GetExpectedResult();
        Assert.Contains("restart", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ExpectedResult_DefaultsToEmpty()
    {
        var td = new TweakDef { Id = "test-default", Label = "Test", Category = "Test", RegistryKeys = [@"HKCU\Test"] };
        Assert.Equal("", td.ExpectedResult);
    }

    [Fact]
    public void GetExpectedResult_BlockLabel_ContainsBlocked()
    {
        var td = new TweakDef
        {
            Id = "t-block",
            Label = "Block Tracking Cookies",
            Category = "Privacy",
        };
        var result = td.GetExpectedResult();
        Assert.Contains("blocked", result);
    }

    [Fact]
    public void GetExpectedResult_HideLabel_ContainsHidden()
    {
        var td = new TweakDef
        {
            Id = "t-hide",
            Label = "Hide Desktop Icons",
            Category = "Explorer",
        };
        var result = td.GetExpectedResult();
        Assert.Contains("hidden", result);
    }

    [Fact]
    public void GetExpectedResult_ShowLabel_ContainsVisible()
    {
        var td = new TweakDef
        {
            Id = "t-show",
            Label = "Show File Extensions",
            Category = "Explorer",
        };
        var result = td.GetExpectedResult();
        Assert.Contains("visible", result);
    }

    [Fact]
    public void GetExpectedResult_SetLabel_ContainsApplied()
    {
        var td = new TweakDef
        {
            Id = "t-set",
            Label = "Set DNS Server",
            Category = "Network",
        };
        var result = td.GetExpectedResult();
        Assert.Contains("applied", result);
    }

    [Fact]
    public void GetExpectedResult_OptimizeLabel_ContainsOptimized()
    {
        var td = new TweakDef
        {
            Id = "t-opt",
            Label = "Optimize Memory Usage",
            Category = "Performance",
        };
        var result = td.GetExpectedResult();
        Assert.Contains("optimized", result);
    }

    [Fact]
    public void GetExpectedResult_ReduceLabel_ContainsReduced()
    {
        var td = new TweakDef
        {
            Id = "t-red",
            Label = "Reduce Animations",
            Category = "Performance",
        };
        var result = td.GetExpectedResult();
        Assert.Contains("reduced", result);
    }

    [Fact]
    public void GetExpectedResult_IncreaseLabel_ContainsIncreased()
    {
        var td = new TweakDef
        {
            Id = "t-inc",
            Label = "Increase Buffer Size",
            Category = "Network",
        };
        var result = td.GetExpectedResult();
        Assert.Contains("increased", result);
    }

    [Fact]
    public void GetExpectedResult_ConfigureLabel_ContainsConfigured()
    {
        var td = new TweakDef
        {
            Id = "t-conf",
            Label = "Configure Proxy Settings",
            Category = "Network",
        };
        var result = td.GetExpectedResult();
        Assert.Contains("configured", result);
    }

    [Fact]
    public void GetExpectedResult_ForceLabel_ContainsEnforced()
    {
        var td = new TweakDef
        {
            Id = "t-force",
            Label = "Force Dark Mode",
            Category = "Display",
        };
        var result = td.GetExpectedResult();
        Assert.Contains("enforced", result);
    }

    [Fact]
    public void GetExpectedResult_PreventLabel_ContainsPrevented()
    {
        var td = new TweakDef
        {
            Id = "t-prev",
            Label = "Prevent Auto-Restart",
            Category = "Windows Update",
        };
        var result = td.GetExpectedResult();
        Assert.Contains("prevented", result);
    }

    [Fact]
    public void GetExpectedResult_RestrictLabel_ContainsRestricted()
    {
        var td = new TweakDef
        {
            Id = "t-restrict",
            Label = "Restrict Background Apps",
            Category = "Performance",
        };
        var result = td.GetExpectedResult();
        Assert.Contains("restricted", result);
    }

    [Fact]
    public void GetExpectedResult_LimitLabel_ContainsLimited()
    {
        var td = new TweakDef
        {
            Id = "t-limit",
            Label = "Limit Bandwidth Usage",
            Category = "Network",
        };
        var result = td.GetExpectedResult();
        Assert.Contains("limited", result);
    }

    [Fact]
    public void GetExpectedResult_RemoveLabel_ContainsRemoved()
    {
        var td = new TweakDef
        {
            Id = "t-rm",
            Label = "Remove Bloatware",
            Category = "Notifications",
        };
        var result = td.GetExpectedResult();
        Assert.Contains("removed", result);
        Assert.Contains("notification", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetExpectedResult_SecurityCategory_MentionsSecurity()
    {
        var td = new TweakDef
        {
            Id = "t-sec",
            Label = "Enable BitLocker",
            Category = "Security",
        };
        var result = td.GetExpectedResult();
        Assert.Contains("security", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetExpectedResult_BootCategory_MentionsBoot()
    {
        var td = new TweakDef
        {
            Id = "t-boot",
            Label = "Disable Fast Startup",
            Category = "Boot",
        };
        var result = td.GetExpectedResult();
        Assert.Contains("boot", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetExpectedResult_AccessibilityCategory_MentionsAccessibility()
    {
        var td = new TweakDef
        {
            Id = "t-acc",
            Label = "Enable Narrator",
            Category = "Accessibility",
        };
        var result = td.GetExpectedResult();
        Assert.Contains("accessibility", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetExpectedResult_UserTweak_NoRestartMention()
    {
        var td = new TweakDef
        {
            Id = "t-user",
            Label = "Disable Animations",
            Category = "Test",
            NeedsAdmin = false,
            RegistryKeys = [@"HKCU\Software\Test"],
        };
        var result = td.GetExpectedResult();
        Assert.DoesNotContain("restart", result, StringComparison.OrdinalIgnoreCase);
    }
}

/// <summary>Tests for TweakDef.Kind auto-detection and HasOperations gate.</summary>
public sealed class TweakKindTests
{
    [Fact]
    public void Kind_WithKindHint_ReturnsHint()
    {
        var td = new TweakDef
        {
            Id = "t-hint",
            Label = "T",
            Category = "C",
            KindHint = TweakKind.ServiceControl,
            ApplyAction = _ => { },
        };
        Assert.Equal(TweakKind.ServiceControl, td.Kind);
    }

    [Fact]
    public void Kind_WithApplyAction_NoHint_ReturnsPowerShell()
    {
        var td = new TweakDef
        {
            Id = "t-action",
            Label = "T",
            Category = "C",
            ApplyAction = _ => { },
        };
        Assert.Equal(TweakKind.PowerShell, td.Kind);
    }

    [Fact]
    public void Kind_WithPoliciesPath_ReturnsGroupPolicy()
    {
        var td = new TweakDef
        {
            Id = "t-gpo",
            Label = "T",
            Category = "C",
            ApplyOps = [RegOp.SetDword(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\Test", "V", 0)],
        };
        Assert.Equal(TweakKind.GroupPolicy, td.Kind);
    }

    [Fact]
    public void Kind_RegularRegOps_ReturnsRegistry()
    {
        var td = new TweakDef
        {
            Id = "t-reg",
            Label = "T",
            Category = "C",
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\Test", "V", 1)],
        };
        Assert.Equal(TweakKind.Registry, td.Kind);
    }

    [Fact]
    public void HasOperations_WithApplyOps_True()
    {
        var td = new TweakDef
        {
            Id = "t",
            Label = "T",
            Category = "C",
            ApplyOps = [RegOp.SetDword(@"HKCU\Test", "V", 1)],
        };
        Assert.True(td.HasOperations);
    }

    [Fact]
    public void HasOperations_WithApplyAction_True()
    {
        var td = new TweakDef
        {
            Id = "t",
            Label = "T",
            Category = "C",
            ApplyAction = _ => { },
        };
        Assert.True(td.HasOperations);
    }

    [Fact]
    public void HasOperations_NoOpsNoAction_False()
    {
        var td = new TweakDef
        {
            Id = "t",
            Label = "T",
            Category = "C",
        };
        Assert.False(td.HasOperations);
    }

    [Fact]
    public void Scope_HkcrAbbreviated_ReturnsMachine()
    {
        var td = new TweakDef
        {
            Id = "t",
            Label = "T",
            Category = "C",
            RegistryKeys = [@"HKCR\*\shell\test"],
        };
        Assert.Equal(TweakScope.Machine, td.Scope);
    }

    [Fact]
    public void PackSource_DefaultsToNull()
    {
        var td = new TweakDef
        {
            Id = "t",
            Label = "T",
            Category = "C",
        };
        Assert.Null(td.PackSource);
    }

    [Fact]
    public void PackSource_FromPack_ReturnsPackName()
    {
        var td = new TweakDef
        {
            Id = "t",
            Label = "T",
            Category = "C",
            PackSource = "gaming-extra",
        };
        Assert.Equal("gaming-extra", td.PackSource);
    }
}

/// <summary>Tests for RegOp factory methods not covered by RegOpTests.</summary>
public sealed class RegOpExtendedTests
{
    [Fact]
    public void SetExpandString_CreatesCorrectOp()
    {
        var op = RegOp.SetExpandString(@"HKLM\Test", "ImagePath", @"%SystemRoot%\System32\svchost.exe");
        Assert.Equal(RegOpKind.SetValue, op.Kind);
        Assert.Equal(Microsoft.Win32.RegistryValueKind.ExpandString, op.ValueKind);
        Assert.Equal(@"%SystemRoot%\System32\svchost.exe", op.Value);
    }

    [Fact]
    public void SetQword_CreatesCorrectOp()
    {
        var op = RegOp.SetQword(@"HKLM\Test", "LargeVal", 0x1_0000_0000L);
        Assert.Equal(RegOpKind.SetValue, op.Kind);
        Assert.Equal(Microsoft.Win32.RegistryValueKind.QWord, op.ValueKind);
        Assert.Equal(0x1_0000_0000L, op.Value);
    }

    [Fact]
    public void SetBinary_CreatesCorrectOp()
    {
        byte[] data = [0xFF, 0x00, 0xAA];
        var op = RegOp.SetBinary(@"HKLM\Test", "BinVal", data);
        Assert.Equal(RegOpKind.SetValue, op.Kind);
        Assert.Equal(Microsoft.Win32.RegistryValueKind.Binary, op.ValueKind);
        Assert.Equal(data, op.Value);
    }

    [Fact]
    public void SetMultiSz_CreatesCorrectOp()
    {
        string[] vals = ["first", "second", "third"];
        var op = RegOp.SetMultiSz(@"HKLM\Test", "Multi", vals);
        Assert.Equal(RegOpKind.SetValue, op.Kind);
        Assert.Equal(Microsoft.Win32.RegistryValueKind.MultiString, op.ValueKind);
        Assert.Equal(vals, op.Value);
    }

    [Fact]
    public void CheckString_CreatesCorrectOp()
    {
        var op = RegOp.CheckString(@"HKCU\Test", "StringVal", "expected");
        Assert.Equal(RegOpKind.CheckValue, op.Kind);
        Assert.Equal("expected", op.Value);
        Assert.Equal(Microsoft.Win32.RegistryValueKind.String, op.ValueKind);
    }

    [Fact]
    public void CheckKeyMissing_HasNoName()
    {
        var op = RegOp.CheckKeyMissing(@"HKLM\Test\Sub");
        Assert.Equal("", op.Name);
    }
}
