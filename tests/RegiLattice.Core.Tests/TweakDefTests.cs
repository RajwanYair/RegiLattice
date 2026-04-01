using RegiLattice.Core;
using RegiLattice.Core.Models;
using RegiLattice.Core.Registry;
using Xunit;
using System.Text.RegularExpressions;
using RegiLattice.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using FsCheck;
using FsCheck.Xunit;

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
        var td = new TweakDef
        {
            Id = "test-default",
            Label = "Test",
            Category = "Test",
            RegistryKeys = [@"HKCU\Test"],
        };
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

    // ── Sprint 21: Coverage boost — ComputeScope, GetExpectedResult, Kind detection ──

    [Fact]
    public void Scope_HKCR_ReturnsMachine()
    {
        var td = new TweakDef
        {
            Id = "scope-hkcr",
            Label = "Test",
            Category = "Test",
            RegistryKeys = [@"HKEY_CLASSES_ROOT\*\shell\open"],
            ApplyOps = [RegOp.SetString(@"HKEY_CLASSES_ROOT\*\shell\open", "V", "x")],
        };
        Assert.Equal(TweakScope.Machine, td.Scope);
    }

    [Fact]
    public void Scope_HKCU_ReturnsUser()
    {
        var td = new TweakDef
        {
            Id = "scope-hkcu",
            Label = "Test",
            Category = "Test",
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Test"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "V", 1)],
        };
        Assert.Equal(TweakScope.User, td.Scope);
    }

    [Fact]
    public void Scope_BothHives_ReturnsBoth()
    {
        var td = new TweakDef
        {
            Id = "scope-both",
            Label = "Test",
            Category = "Test",
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Test", @"HKEY_LOCAL_MACHINE\SOFTWARE\Test"],
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\Test", "V", 1)],
        };
        Assert.Equal(TweakScope.Both, td.Scope);
    }

    [Fact]
    public void Scope_NoKeys_NotAdmin_ReturnsUser()
    {
        var td = new TweakDef
        {
            Id = "scope-nokeys-noadmin",
            Label = "Test",
            Category = "Test",
            NeedsAdmin = false,
            ApplyAction = _ => { },
        };
        Assert.Equal(TweakScope.User, td.Scope);
    }

    [Fact]
    public void Scope_NoKeys_Admin_ReturnsMachine()
    {
        var td = new TweakDef
        {
            Id = "scope-nokeys-admin",
            Label = "Test",
            Category = "Test",
            NeedsAdmin = true,
            ApplyAction = _ => { },
        };
        Assert.Equal(TweakScope.Machine, td.Scope);
    }

    [Fact]
    public void Scope_HKLM_Abbreviation_ReturnsMachine()
    {
        var td = new TweakDef
        {
            Id = "scope-hklm-abbrev",
            Label = "Test",
            Category = "Test",
            RegistryKeys = [@"HKLM\SOFTWARE\Test"],
            ApplyOps = [RegOp.SetDword(@"HKLM\SOFTWARE\Test", "V", 1)],
        };
        Assert.Equal(TweakScope.Machine, td.Scope);
    }

    [Fact]
    public void Scope_HKCU_Abbreviation_ReturnsUser()
    {
        var td = new TweakDef
        {
            Id = "scope-hkcu-abbrev",
            Label = "Test",
            Category = "Test",
            RegistryKeys = [@"HKCU\Software\Test"],
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\Test", "V", 1)],
        };
        Assert.Equal(TweakScope.User, td.Scope);
    }

    [Fact]
    public void Kind_WithKindHint_ReturnsHint()
    {
        var td = new TweakDef
        {
            Id = "kind-hint",
            Label = "Test",
            Category = "Test",
            KindHint = TweakKind.ServiceControl,
            ApplyOps = [RegOp.SetDword(@"HKLM\Test", "V", 1)],
        };
        Assert.Equal(TweakKind.ServiceControl, td.Kind);
    }

    [Fact]
    public void Kind_WithApplyAction_ReturnsPowerShell()
    {
        var td = new TweakDef
        {
            Id = "kind-action",
            Label = "Test",
            Category = "Test",
            ApplyAction = _ => { },
        };
        Assert.Equal(TweakKind.PowerShell, td.Kind);
    }

    [Fact]
    public void Kind_PoliciesPath_ReturnsGroupPolicy()
    {
        var td = new TweakDef
        {
            Id = "kind-gpo",
            Label = "Test",
            Category = "Test",
            ApplyOps = [RegOp.SetDword(@"HKLM\SOFTWARE\Policies\Microsoft\Windows", "V", 0)],
        };
        Assert.Equal(TweakKind.GroupPolicy, td.Kind);
    }

    [Fact]
    public void Kind_NormalRegOps_ReturnsRegistry()
    {
        var td = new TweakDef
        {
            Id = "kind-registry",
            Label = "Test",
            Category = "Test",
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\Test", "V", 1)],
        };
        Assert.Equal(TweakKind.Registry, td.Kind);
    }

    [Fact]
    public void HasOperations_WithApplyOps_ReturnsTrue()
    {
        var td = new TweakDef
        {
            Id = "hasops-ops",
            Label = "Test",
            Category = "Test",
            ApplyOps = [RegOp.SetDword(@"HKCU\Test", "V", 1)],
        };
        Assert.True(td.HasOperations);
    }

    [Fact]
    public void HasOperations_WithApplyAction_ReturnsTrue()
    {
        var td = new TweakDef
        {
            Id = "hasops-action",
            Label = "Test",
            Category = "Test",
            ApplyAction = _ => { },
        };
        Assert.True(td.HasOperations);
    }

    [Fact]
    public void HasOperations_NoOpsNoAction_ReturnsFalse()
    {
        var td = new TweakDef
        {
            Id = "hasops-none",
            Label = "Test",
            Category = "Test",
        };
        Assert.False(td.HasOperations);
    }

    [Fact]
    public void GetExpectedResult_ExplicitValue_ReturnsIt()
    {
        var td = new TweakDef
        {
            Id = "expected-explicit",
            Label = "Disable X",
            Category = "Privacy",
            ExpectedResult = "Custom result text.",
            ApplyOps = [RegOp.SetDword(@"HKCU\Test", "V", 1)],
        };
        Assert.Equal("Custom result text.", td.GetExpectedResult());
    }

    [Fact]
    public void GetExpectedResult_Empty_AutoGenerates()
    {
        var td = new TweakDef
        {
            Id = "expected-auto",
            Label = "Disable Telemetry",
            Category = "Privacy",
            ApplyOps = [RegOp.SetDword(@"HKCU\Test", "V", 1)],
        };
        var result = td.GetExpectedResult();
        Assert.NotEmpty(result);
        Assert.NotEqual("", result);
    }

    [Fact]
    public void GetExpectedResult_EnableLabel_AutoGenerates()
    {
        var td = new TweakDef
        {
            Id = "expected-enable",
            Label = "Enable Dark Mode",
            Category = "Display",
            ApplyOps = [RegOp.SetDword(@"HKCU\Test", "V", 1)],
        };
        var result = td.GetExpectedResult();
        Assert.NotEmpty(result);
    }

    [Fact]
    public void GetExpectedResult_NeedsAdmin_IncludesRestartNote()
    {
        var td = new TweakDef
        {
            Id = "expected-restart",
            Label = "Disable Something",
            Category = "Performance",
            NeedsAdmin = true,
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Test"],
            ApplyOps = [RegOp.SetDword(@"HKLM\SOFTWARE\Test", "V", 1)],
        };
        var result = td.GetExpectedResult();
        Assert.Contains("restart", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void PackSource_Null_ByDefault()
    {
        var td = new TweakDef
        {
            Id = "pack-default",
            Label = "Test",
            Category = "Test",
            ApplyOps = [RegOp.SetDword(@"HKCU\Test", "V", 1)],
        };
        Assert.Null(td.PackSource);
    }

    [Fact]
    public void PackSource_SetFromPack_ReturnsPackName()
    {
        var td = new TweakDef
        {
            Id = "pack-set",
            Label = "Test",
            Category = "Test",
            PackSource = "my-custom-pack",
            ApplyOps = [RegOp.SetDword(@"HKCU\Test", "V", 1)],
        };
        Assert.Equal("my-custom-pack", td.PackSource);
    }

    [Fact]
    public void SetBinary_ValueKindAndData_AreCorrect()
    {
        byte[] data = [0x01, 0x02, 0x03];
        var op = RegOp.SetBinary(@"HKCU\Test", "BinVal", data);
        Assert.Equal(RegOpKind.SetValue, op.Kind);
        Assert.Equal(Microsoft.Win32.RegistryValueKind.Binary, op.ValueKind);
        Assert.Equal(data, op.Value);
    }

    [Fact]
    public void SetMultiSz_ValueKindAndData_AreCorrect()
    {
        string[] data = ["val1", "val2", "val3"];
        var op = RegOp.SetMultiSz(@"HKCU\Test", "MultiVal", data);
        Assert.Equal(RegOpKind.SetValue, op.Kind);
        Assert.Equal(Microsoft.Win32.RegistryValueKind.MultiString, op.ValueKind);
        Assert.Equal(data, op.Value);
    }

    [Fact]
    public void SetExpandString_ValueKindAndData_AreCorrect()
    {
        var op = RegOp.SetExpandString(@"HKLM\Test", "Path", @"%SystemRoot%\System32");
        Assert.Equal(RegOpKind.SetValue, op.Kind);
        Assert.Equal(Microsoft.Win32.RegistryValueKind.ExpandString, op.ValueKind);
        Assert.Equal(@"%SystemRoot%\System32", op.Value);
    }

    [Fact]
    public void SetQword_ValueKindAndData_AreCorrect()
    {
        var op = RegOp.SetQword(@"HKCU\Test", "Big", 999999999999L);
        Assert.Equal(RegOpKind.SetValue, op.Kind);
        Assert.Equal(Microsoft.Win32.RegistryValueKind.QWord, op.ValueKind);
        Assert.Equal(999999999999L, op.Value);
    }

    [Fact]
    public void DeleteTree_CreatesCorrectOp()
    {
        var op = RegOp.DeleteTree(@"HKLM\Software\Test\SubKey");
        Assert.Equal(RegOpKind.DeleteTree, op.Kind);
        Assert.Equal(@"HKLM\Software\Test\SubKey", op.Path);
    }

    [Fact]
    public void CheckMissing_CreatesCorrectOp()
    {
        var op = RegOp.CheckMissing(@"HKCU\Test", "ShouldNotExist");
        Assert.Equal(RegOpKind.CheckMissing, op.Kind);
        Assert.Equal("ShouldNotExist", op.Name);
    }
}

// ── merged from TweakDefMetadataTests.cs ──────────────────────────────────
/// <summary>Tests for Sprint 57: ImpactScore and SafetyRating metadata fields.</summary>
public sealed class TweakDefMetadataTests
{
    private static TweakDef MakeWithOps(string id = "meta-test") =>
        new()
        {
            Id = id,
            Label = "Metadata Test Tweak",
            Category = "Test",
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "V", 1)],
        };

    // ── Defaults ──────────────────────────────────────────────────────────

    [Fact]
    public void ImpactScore_Default_IsThree()
    {
        var td = MakeWithOps();
        Assert.Equal(3, td.ImpactScore);
    }

    [Fact]
    public void SafetyRating_Default_IsFour()
    {
        var td = MakeWithOps();
        Assert.Equal(4, td.SafetyRating);
    }

    // ── Explicit values ───────────────────────────────────────────────────

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void ImpactScore_ExplicitValue_Roundtrips(int score)
    {
        var td = new TweakDef
        {
            Id = "meta-test",
            Label = "Metadata Test Tweak",
            Category = "Test",
            ImpactScore = score,
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "V", 1)],
        };
        Assert.Equal(score, td.ImpactScore);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void SafetyRating_ExplicitValue_Roundtrips(int rating)
    {
        var td = new TweakDef
        {
            Id = "meta-test",
            Label = "Metadata Test Tweak",
            Category = "Test",
            SafetyRating = rating,
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "V", 1)],
        };
        Assert.Equal(rating, td.SafetyRating);
    }

    // ── Validator range checks ────────────────────────────────────────────

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    [InlineData(-1)]
    public void Validator_InvalidImpactScore_ReportsError(int score)
    {
        var td = new TweakDef
        {
            Id = "validator-impact",
            Label = "Impact Test",
            Category = "Test",
            ImpactScore = score,
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "V", 1)],
        };
        var errors = TweakValidator.Validate([td], _ => null);
        Assert.Contains(errors, e => e.Contains("ImpactScore") && e.Contains("validator-impact"));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    [InlineData(-1)]
    public void Validator_InvalidSafetyRating_ReportsError(int rating)
    {
        var td = new TweakDef
        {
            Id = "validator-safety",
            Label = "Safety Test",
            Category = "Test",
            SafetyRating = rating,
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "V", 1)],
        };
        var errors = TweakValidator.Validate([td], _ => null);
        Assert.Contains(errors, e => e.Contains("SafetyRating") && e.Contains("validator-safety"));
    }

    [Fact]
    public void Validator_ValidScores_NoErrors()
    {
        var td = new TweakDef
        {
            Id = "valid-scores",
            Label = "Valid Scores Test",
            Category = "Test",
            ImpactScore = 5,
            SafetyRating = 1,
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "V", 1)],
        };
        var errors = TweakValidator.Validate([td], _ => null);
        Assert.DoesNotContain(errors, e => e.Contains("ImpactScore") || e.Contains("SafetyRating"));
    }

    // ── Builtin tweaks carry default scores ───────────────────────────────

    [Fact]
    public void RegisterBuiltins_AllTweaks_HaveScoresInRange()
    {
        var engine = new TweakEngine();
        engine.RegisterBuiltins();
        Assert.All(
            engine.AllTweaks(),
            td =>
            {
                Assert.InRange(td.ImpactScore, 1, 5);
                Assert.InRange(td.SafetyRating, 1, 5);
            }
        );
    }
}
// ── merged from TweakDefPropertyTests.cs ──────────────────────────────────
/// <summary>
/// Sprint 73 — Universal invariant / property tests.
/// Each test verifies a structural property that must hold across ALL registered tweaks.
/// Failing here means a regression in the tweak corpus or engine, not just a single tweak.
/// </summary>
public sealed class TweakDefPropertyTests
{
    // ── Shared engine ─────────────────────────────────────────────────────────

    private static readonly TweakEngine _engine = BuildEngine();

    private static TweakEngine BuildEngine()
    {
        var e = new TweakEngine();
        e.RegisterBuiltins();
        return e;
    }

    // ── A: Universal TweakDef field invariants ────────────────────────────────

    /// <summary>
    /// Tags is declared with a default of []; verify it is never null at runtime
    /// for any of the registered tweaks.
    /// </summary>
    [Fact]
    public void AllTweaks_TagsNeverNull()
    {
        foreach (var td in _engine.AllTweaks())
            Assert.NotNull(td.Tags);
    }

    /// <summary>
    /// ImpactScore defaults to 3 and must always be in the 1–5 range.
    /// A value outside this range would corrupt HealthScoreService bucket weighting.
    /// </summary>
    [Fact]
    public void AllTweaks_ImpactScore_InRange_OneToFive()
    {
        foreach (var td in _engine.AllTweaks())
            Assert.InRange(td.ImpactScore, 1, 5);
    }

    /// <summary>Scope must resolve to one of the three defined enum values.</summary>
    [Fact]
    public void AllTweaks_Scope_IsDefinedEnum()
    {
        var validScopes = new[] { TweakScope.User, TweakScope.Machine, TweakScope.Both };
        foreach (var td in _engine.AllTweaks())
            Assert.Contains(td.Scope, validScopes);
    }

    /// <summary>
    /// Tweak IDs must follow the kebab-case naming convention:
    /// all lowercase letters, digits, and hyphens; must start with a letter.
    /// </summary>
    [Fact]
    public void AllTweaks_IdFollowsKebabCase()
    {
        var pattern = new Regex(@"^[a-z][a-z0-9\-]+$", RegexOptions.Compiled);
        foreach (var td in _engine.AllTweaks())
        {
            Assert.True(pattern.IsMatch(td.Id), $"Tweak id '{td.Id}' does not follow kebab-case convention (lowercase letters, digits, hyphens).");
        }
    }

    /// <summary>
    /// GetExpectedResult() must never return null for any registered tweak.
    /// It auto-generates a description when ExpectedResult is empty.
    /// </summary>
    [Fact]
    public void AllTweaks_GetExpectedResult_NeverNullOrEmpty()
    {
        foreach (var td in _engine.AllTweaks())
        {
            var result = td.GetExpectedResult();
            Assert.False(string.IsNullOrWhiteSpace(result), $"Tweak '{td.Id}' returned null/empty from GetExpectedResult().");
        }
    }

    /// <summary>
    /// Every registry path inside ApplyOps (if any) must start with a known hive prefix.
    /// This catches typos like "SOFTWARE\..." without a hive prefix.
    /// </summary>
    [Fact]
    public void AllTweaks_ApplyOps_RegistryPaths_StartWithKnownHive()
    {
        var validPrefixes = new[]
        {
            "HKEY_LOCAL_MACHINE",
            "HKEY_CURRENT_USER",
            "HKEY_CLASSES_ROOT",
            "HKEY_USERS",
            "HKEY_CURRENT_CONFIG",
            "HKLM",
            "HKCU",
            "HKCR",
        };

        foreach (var td in _engine.AllTweaks())
        {
            foreach (var op in td.ApplyOps)
            {
                Assert.True(
                    validPrefixes.Any(p => op.Path.StartsWith(p, StringComparison.OrdinalIgnoreCase)),
                    $"Tweak '{td.Id}' has ApplyOp with invalid hive path: '{op.Path}'"
                );
            }
        }
    }

    /// <summary>
    /// DependsOn entries must reference IDs that are actually registered in the engine.
    /// A broken dependency causes DependencyResolver to blow up at apply time.
    /// </summary>
    [Fact]
    public void AllTweaks_DependsOn_ReferencesRegisteredIds()
    {
        var allIds = _engine.AllTweaks().Select(t => t.Id).ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (var td in _engine.AllTweaks())
        {
            foreach (var dep in td.DependsOn)
            {
                Assert.True(allIds.Contains(dep), $"Tweak '{td.Id}' depends on '{dep}' which is not registered.");
            }
        }
    }

    // ── B: Engine structural invariants ──────────────────────────────────────

    /// <summary>
    /// TweakEngine.TweakCount must always equal AllTweaks().Count.
    /// These are two independent accessors for the same data.
    /// </summary>
    [Fact]
    public void TweakCount_MatchesAllTweaksCount()
    {
        Assert.Equal(_engine.AllTweaks().Count, _engine.TweakCount);
    }

    /// <summary>
    /// The sum of all per-category tweak counts must equal the total tweak count.
    /// This verifies that TweaksByCategory() covers every registered tweak exactly once.
    /// </summary>
    [Fact]
    public void TweaksByCategory_SumEqualsAllTweaksCount()
    {
        var total = _engine.TweaksByCategory().Values.Sum(list => list.Count);
        Assert.Equal(_engine.TweakCount, total);
    }

    /// <summary>
    /// Every tweak's Category must appear in Categories().
    /// An orphaned category (present on a tweak but absent from the index) would
    /// be invisible in the GUI category tree.
    /// </summary>
    [Fact]
    public void AllTweaks_Category_AppearsInCategoriesIndex()
    {
        var categories = _engine.Categories().ToHashSet(StringComparer.OrdinalIgnoreCase);
        foreach (var td in _engine.AllTweaks())
        {
            Assert.True(categories.Contains(td.Category), $"Tweak '{td.Id}' has Category '{td.Category}' not found in engine.Categories().");
        }
    }

    /// <summary>
    /// Every tweak's ID must appear in the TweaksByCategory() list for its own Category.
    /// Verifies bidirectional consistency between ID → Category and Category → [IDs].
    /// </summary>
    [Fact]
    public void AllTweaks_AreMemberOfTheirOwnCategory()
    {
        var byCategory = _engine.TweaksByCategory();
        foreach (var td in _engine.AllTweaks())
        {
            Assert.True(
                byCategory.TryGetValue(td.Category, out var list),
                $"Tweak '{td.Id}' category '{td.Category}' missing from TweaksByCategory() keys."
            );
            Assert.Contains(list, t => t.Id.Equals(td.Id, StringComparison.OrdinalIgnoreCase));
        }
    }

    // ── C: Search / Filter subset properties ─────────────────────────────────

    /// <summary>
    /// Filter(corpSafe: true) must always return a subset of AllTweaks().
    /// The filtered collection must never contain IDs not present in AllTweaks().
    /// </summary>
    [Fact]
    public void Filter_CorpSafeTrue_ReturnsSubsetOfAllTweaks()
    {
        var allIds = _engine.AllTweaks().Select(t => t.Id).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var filtered = _engine.Filter(corpSafe: true);

        Assert.All(filtered, t => Assert.Contains(t.Id, allIds));
        Assert.True(filtered.Count <= _engine.TweakCount);
    }

    /// <summary>
    /// Filter(needsAdmin: false) narrows the set — result count must be ≤ total count
    /// and every returned tweak must have NeedsAdmin == false.
    /// </summary>
    [Fact]
    public void Filter_NeedsAdminFalse_AllResultsHaveNeedsAdminFalse()
    {
        var filtered = _engine.Filter(needsAdmin: false);

        Assert.True(filtered.Count <= _engine.TweakCount);
        Assert.All(filtered, t => Assert.False(t.NeedsAdmin, $"Tweak '{t.Id}' has NeedsAdmin=true but passed Filter(needsAdmin:false)."));
    }

    /// <summary>
    /// Search() results must always be a strict subset of AllTweaks().
    /// Tests three representative query terms.
    /// </summary>
    [Theory]
    [InlineData("privacy")]
    [InlineData("gaming")]
    [InlineData("telemetry")]
    public void Search_ResultsAreSubsetOfAllTweaks(string query)
    {
        var allIds = _engine.AllTweaks().Select(t => t.Id).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var results = _engine.Search(query);

        Assert.NotEmpty(results);
        Assert.True(results.Count <= _engine.TweakCount);
        Assert.All(results, t => Assert.Contains(t.Id, allIds));
    }

    /// <summary>
    /// Filter(scope: User) must contain only tweaks whose Scope is User or Both —
    /// never Machine-only tweaks.
    /// </summary>
    [Fact]
    public void Filter_ScopeUser_NeverReturnsMachineTweaks()
    {
        var filtered = _engine.Filter(scope: TweakScope.User);
        Assert.All(
            filtered,
            t =>
                Assert.True(
                    t.Scope == TweakScope.User || t.Scope == TweakScope.Both,
                    $"Tweak '{t.Id}' has Scope={t.Scope} but was returned by Filter(scope:User)."
                )
        );
    }

    // ── D: HealthScore invariant ──────────────────────────────────────────────

    /// <summary>
    /// HealthScore.Overall is always the integer truncation of (P + E + S + St) / 4.
    /// Tests this with an all-applied map, all-notapplied, and a mixed map.
    /// </summary>
    [Fact]
    public void HealthScore_Overall_IsIntegerAverageOfFourDimensions()
    {
        var svc = new HealthScoreService(_engine);
        var allTweaks = _engine.AllTweaks();

        // Scenario 1: empty map → all zeros → Overall == 0
        var emptyScore = svc.Compute(new Dictionary<string, TweakResult>());
        Assert.Equal((emptyScore.Privacy + emptyScore.Performance + emptyScore.Security + emptyScore.Stability) / 4, emptyScore.Overall);

        // Scenario 2: all applied
        var allApplied = allTweaks.ToDictionary(t => t.Id, _ => TweakResult.Applied);
        var fullScore = svc.Compute(allApplied);
        Assert.Equal((fullScore.Privacy + fullScore.Performance + fullScore.Security + fullScore.Stability) / 4, fullScore.Overall);

        // Scenario 3: only privacy tweaks applied
        var privacyOnly = svc.PrivacyTweaks().ToDictionary(t => t.Id, _ => TweakResult.Applied);
        var partialScore = svc.Compute(privacyOnly);
        Assert.Equal((partialScore.Privacy + partialScore.Performance + partialScore.Security + partialScore.Stability) / 4, partialScore.Overall);
    }
}

// ── merged from TweakDefBranchCoverageTests.cs ──────────────────────────────────
/// <summary>
/// Sprint 111 — Targets the 20 uncovered category-note switch arms in
/// <c>TweakDef.GenerateExpectedResult()</c> and the 2 uncovered restart-note
/// branches (RegistryKeys-driven, NeedsAdmin=false).
/// Each test exercises exactly one previously untested branch.
/// </summary>
public sealed class TweakDefBranchCoverageTests
{
    // ── Helpers ──────────────────────────────────────────────────────────

    private static TweakDef Make(string label, string category, bool needsAdmin = false, string? regKey = null) =>
        new()
        {
            Id = $"br-{label.ToLowerInvariant().Replace(' ', '-').Replace('/', '-')}",
            Label = label,
            Category = category,
            NeedsAdmin = needsAdmin,
            RegistryKeys = regKey is null ? [] : [regKey],
        };

    // ── Category-note: Telemetry Advanced ────────────────────────────────

    [Fact]
    public void GetExpectedResult_TelemetryAdvancedCategory_MentionsPrivacy()
    {
        var td = Make("Disable Diagnostic Data", "Telemetry Advanced");
        Assert.Contains("privacy", td.GetExpectedResult(), StringComparison.OrdinalIgnoreCase);
    }

    // ── Category-note: Memory Optimization ───────────────────────────────

    [Fact]
    public void GetExpectedResult_MemoryOptimizationCategory_MentionsResponsiveness()
    {
        var td = Make("Disable Memory Compression", "Memory Optimization");
        Assert.Contains("responsiveness", td.GetExpectedResult(), StringComparison.OrdinalIgnoreCase);
    }

    // ── Category-note: SSD Optimization ──────────────────────────────────

    [Fact]
    public void GetExpectedResult_SsdOptimizationCategory_MentionsSpeed()
    {
        var td = Make("Enable TRIM", "SSD Optimization");
        string result = td.GetExpectedResult();
        Assert.True(
            result.Contains("speed", StringComparison.OrdinalIgnoreCase) || result.Contains("responsiveness", StringComparison.OrdinalIgnoreCase),
            $"Expected 'speed' or 'responsiveness' in: {result}"
        );
    }

    // ── Category-note: Gaming ────────────────────────────────────────────

    [Fact]
    public void GetExpectedResult_GamingCategory_MentionsGaming()
    {
        var td = Make("Enable Game Mode", "Gaming");
        Assert.Contains("gaming", td.GetExpectedResult(), StringComparison.OrdinalIgnoreCase);
    }

    // ── Category-note: GPU / Graphics ────────────────────────────────────

    [Fact]
    public void GetExpectedResult_GpuGraphicsCategory_MentionsGaming()
    {
        var td = Make("Enable Hardware Scheduling", "GPU / Graphics");
        Assert.Contains("gaming", td.GetExpectedResult(), StringComparison.OrdinalIgnoreCase);
    }

    // ── Category-note: Hardening ─────────────────────────────────────────

    [Fact]
    public void GetExpectedResult_HardeningCategory_MentionsSecurity()
    {
        var td = Make("Enable ASLR", "Hardening");
        Assert.Contains("security", td.GetExpectedResult(), StringComparison.OrdinalIgnoreCase);
    }

    // ── Category-note: Encryption ────────────────────────────────────────

    [Fact]
    public void GetExpectedResult_EncryptionCategory_MentionsSecurity()
    {
        var td = Make("Enable BitLocker Encryption", "Encryption");
        Assert.Contains("security", td.GetExpectedResult(), StringComparison.OrdinalIgnoreCase);
    }

    // ── Category-note: Firewall ───────────────────────────────────────────

    [Fact]
    public void GetExpectedResult_FirewallCategory_MentionsSecurity()
    {
        var td = Make("Enable Firewall Logging", "Firewall");
        Assert.Contains("security", td.GetExpectedResult(), StringComparison.OrdinalIgnoreCase);
    }

    // ── Category-note: Network Optimization ──────────────────────────────

    [Fact]
    public void GetExpectedResult_NetworkOptimizationCategory_MentionsNetwork()
    {
        var td = Make("Optimize TCP Window", "Network Optimization");
        Assert.Contains("network", td.GetExpectedResult(), StringComparison.OrdinalIgnoreCase);
    }

    // ── Category-note: DNS & Networking Advanced ─────────────────────────

    [Fact]
    public void GetExpectedResult_DnsNetworkingAdvancedCategory_MentionsNetwork()
    {
        var td = Make("Enable DNS over HTTPS", "DNS & Networking Advanced");
        Assert.Contains("network", td.GetExpectedResult(), StringComparison.OrdinalIgnoreCase);
    }

    // ── Category-note: Power ─────────────────────────────────────────────

    [Fact]
    public void GetExpectedResult_PowerCategory_MentionsPower()
    {
        var td = Make("Disable Sleep Mode", "Power");
        Assert.Contains("power", td.GetExpectedResult(), StringComparison.OrdinalIgnoreCase);
    }

    // ── Category-note: Power Management ─────────────────────────────────

    [Fact]
    public void GetExpectedResult_PowerManagementCategory_MentionsPower()
    {
        var td = Make("Set High Performance Plan", "Power Management");
        Assert.Contains("power", td.GetExpectedResult(), StringComparison.OrdinalIgnoreCase);
    }

    // ── Category-note: Startup ────────────────────────────────────────────

    [Fact]
    public void GetExpectedResult_StartupCategory_MentionsBoot()
    {
        var td = Make("Disable Startup Delay", "Startup");
        string result = td.GetExpectedResult();
        Assert.True(
            result.Contains("startup", StringComparison.OrdinalIgnoreCase) || result.Contains("boot", StringComparison.OrdinalIgnoreCase),
            $"Expected 'startup' or 'boot' in: {result}"
        );
    }

    // ── Category-note: Context Menu ───────────────────────────────────────

    [Fact]
    public void GetExpectedResult_ContextMenuCategory_MentionsShell()
    {
        var td = Make("Remove Open Terminal", "Context Menu");
        Assert.Contains("shell", td.GetExpectedResult(), StringComparison.OrdinalIgnoreCase);
    }

    // ── Category-note: Shell ──────────────────────────────────────────────

    [Fact]
    public void GetExpectedResult_ShellCategory_MentionsShell()
    {
        var td = Make("Enable Classic Shell", "Shell");
        Assert.Contains("shell", td.GetExpectedResult(), StringComparison.OrdinalIgnoreCase);
    }

    // ── Category-note: Taskbar ────────────────────────────────────────────

    [Fact]
    public void GetExpectedResult_TaskbarCategory_MentionsShell()
    {
        var td = Make("Hide Taskbar Labels", "Taskbar");
        Assert.Contains("shell", td.GetExpectedResult(), StringComparison.OrdinalIgnoreCase);
    }

    // ── Category-note: Bluetooth ──────────────────────────────────────────

    [Fact]
    public void GetExpectedResult_BluetoothCategory_MentionsPeripheral()
    {
        var td = Make("Disable Bluetooth Discovery", "Bluetooth");
        Assert.Contains("peripheral", td.GetExpectedResult(), StringComparison.OrdinalIgnoreCase);
    }

    // ── Category-note: USB & Peripherals ─────────────────────────────────

    [Fact]
    public void GetExpectedResult_UsbPeripheralsCategory_MentionsPeripheral()
    {
        var td = Make("Disable USB AutoPlay", "USB & Peripherals");
        Assert.Contains("peripheral", td.GetExpectedResult(), StringComparison.OrdinalIgnoreCase);
    }

    // ── Category-note: Printing ───────────────────────────────────────────

    [Fact]
    public void GetExpectedResult_PrintingCategory_MentionsPrinting()
    {
        var td = Make("Disable Print Spooler", "Printing");
        Assert.Contains("printing", td.GetExpectedResult(), StringComparison.OrdinalIgnoreCase);
    }

    // ── Category-note: default (unrecognised category) ────────────────────

    [Fact]
    public void GetExpectedResult_UnknownCategory_HasNoCategoryNote()
    {
        // "Custom Hardware" matches no arm → categoryNote = ""
        var td = Make("Disable Sensor Hub", "Custom Hardware", needsAdmin: false);
        string result = td.GetExpectedResult();
        // Result should contain only the verb action, with no extra category note text.
        // Verify by checking none of the category note substrings appear.
        Assert.DoesNotContain("privacy", result, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("responsiveness", result, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("gaming", result, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("security", result, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("peripheral", result, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("printing", result, StringComparison.OrdinalIgnoreCase);
    }

    // ── Restart note: NeedsAdmin=false + HKLM key triggers restart note ──

    [Fact]
    public void GetExpectedResult_NeedsAdminFalse_HklmKey_MentionsRestart()
    {
        // NeedsAdmin=false means the NeedsAdmin branch short-circuits to false,
        // but the RegistryKeys check must still fire and detect the HKLM path.
        var td = new TweakDef
        {
            Id = "br-restart-hklm",
            Label = "Disable Teredo Tunnelling",
            Category = "Network",
            NeedsAdmin = false,
            RegistryKeys = [@"HKLM\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters"],
        };
        Assert.Contains("restart", td.GetExpectedResult(), StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetExpectedResult_NeedsAdminFalse_HkeyLocalMachineFullName_MentionsRestart()
    {
        // Tests the second condition inside the RegistryKeys.Any() predicate:
        // k.Contains("HKEY_LOCAL_MACHINE", ...) rather than "HKLM".
        var td = new TweakDef
        {
            Id = "br-restart-hklm-full",
            Label = "Disable IPv6 Components",
            Category = "Network",
            NeedsAdmin = false,
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters"],
        };
        Assert.Contains("restart", td.GetExpectedResult(), StringComparison.OrdinalIgnoreCase);
    }

    // ── ComputeScope: HKCU abbreviated vs full name ───────────────────────

    [Fact]
    public void Scope_HkcuAbbreviated_ReturnsUser()
    {
        var td = new TweakDef
        {
            Id = "br-scope-hkcu",
            Label = "T",
            Category = "C",
            RegistryKeys = [@"HKCU\Software\Test"],
        };
        Assert.Equal(TweakScope.User, td.Scope);
    }

    [Fact]
    public void Scope_HklmAbbreviated_ReturnsMachine()
    {
        var td = new TweakDef
        {
            Id = "br-scope-hklm",
            Label = "T",
            Category = "C",
            RegistryKeys = [@"HKLM\SOFTWARE\Test"],
        };
        Assert.Equal(TweakScope.Machine, td.Scope);
    }

    [Fact]
    public void Scope_HkeyClassesRoot_FullName_ReturnsMachine()
    {
        var td = new TweakDef
        {
            Id = "br-scope-hkcr-full",
            Label = "T",
            Category = "C",
            RegistryKeys = [@"HKEY_CLASSES_ROOT\*\shell\open"],
        };
        Assert.Equal(TweakScope.Machine, td.Scope);
    }

    [Fact]
    public void Scope_BothHkcuAndHklm_FullNames_ReturnsBoth()
    {
        var td = new TweakDef
        {
            Id = "br-scope-both-full",
            Label = "T",
            Category = "C",
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Test", @"HKEY_LOCAL_MACHINE\SOFTWARE\Test"],
        };
        Assert.Equal(TweakScope.Both, td.Scope);
    }

    // ── Kind: ApplyAction with no KindHint → PowerShell ──────────────────

    [Fact]
    public void Kind_ApplyActionNoOps_NullKindHint_ReturnsPowerShell()
    {
        var td = new TweakDef
        {
            Id = "br-kind-ps",
            Label = "T",
            Category = "C",
            ApplyAction = _ => { },
        };
        Assert.Equal(TweakKind.PowerShell, td.Kind);
    }

    // ── Kind: no ApplyAction, no Policies path → Registry ────────────────

    [Fact]
    public void Kind_NoApplyAction_NoPoliciesPath_ReturnsRegistry()
    {
        var td = new TweakDef
        {
            Id = "br-kind-reg",
            Label = "T",
            Category = "C",
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\Test", "V", 1)],
        };
        Assert.Equal(TweakKind.Registry, td.Kind);
    }
}
// ── merged from PropertyTests.cs ──────────────────────────────────
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
