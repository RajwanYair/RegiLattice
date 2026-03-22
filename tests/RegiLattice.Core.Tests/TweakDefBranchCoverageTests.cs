// tests/RegiLattice.Core.Tests/TweakDefBranchCoverageTests.cs
// Sprint 111 — Branch coverage push: systematic tests for all uncovered arms
// of TweakDef.GenerateExpectedResult() (category-note switch + restart-note branches).

#nullable enable

using RegiLattice.Core.Models;
using Xunit;

namespace RegiLattice.Core.Tests;

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
            result.Contains("speed", StringComparison.OrdinalIgnoreCase)
                || result.Contains("responsiveness", StringComparison.OrdinalIgnoreCase),
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
            result.Contains("startup", StringComparison.OrdinalIgnoreCase)
                || result.Contains("boot", StringComparison.OrdinalIgnoreCase),
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
            RegistryKeys =
            [
                @"HKEY_CURRENT_USER\Software\Test",
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Test",
            ],
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
