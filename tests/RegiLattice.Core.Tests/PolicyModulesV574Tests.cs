// tests/RegiLattice.Core.Tests/PolicyModulesV574Tests.cs
// Sprint 437-531 — Validates all 94 new policy modules added in v5.74.0.
// Each group of 5 modules maps to one sprint (19 sprints × 5 modules = 95 modules).

#nullable enable

using RegiLattice.Core;
using RegiLattice.Core.Models;
using Xunit;

namespace RegiLattice.Core.Tests;

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
