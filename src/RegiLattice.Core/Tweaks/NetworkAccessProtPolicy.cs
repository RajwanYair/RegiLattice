// RegiLattice.Core — Tweaks/NetworkAccessProtPolicy.cs
// Network Access Protection Group Policy — Sprint 189.
// Controls NAP (Network Access Protection) client enforcement, DHCP/VPN/802.1X
// quarantine, and NAP user interface settings via Group Policy.
// Category: "Network Protection Policy" | Slug: nappol
// Registry path: HKLM\SOFTWARE\Policies\Microsoft\Windows\NetworkAccessProtection

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class NetworkAccessProtPolicy
{
    private const string NapKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkAccessProtection";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "nappol-disable-nap-client",
                Label = "Disable NAP Client Service",
                Category = "Network Protection Policy",
                Description =
                    "Sets Enabled=0 to disable the Network Access Protection client service. NAP was deprecated in Windows 10 but the client components remain; disabling prevents unnecessary service overhead.",
                Tags = ["nap", "network", "policy", "service"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "NAP client disabled; no impact on modern networks that do not use NAP infrastructure.",
                ApplyOps = [RegOp.SetDword(NapKey, "Enabled", 0)],
                RemoveOps = [RegOp.DeleteValue(NapKey, "Enabled")],
                DetectOps = [RegOp.CheckDword(NapKey, "Enabled", 0)],
            },
            new TweakDef
            {
                Id = "nappol-disable-dhcp-quarantine",
                Label = "Disable NAP DHCP Quarantine Enforcement",
                Category = "Network Protection Policy",
                Description =
                    "Sets EnableDhcpQuarantine=0 to disable NAP enforcement through DHCP. Prevents the client from being quarantined to a restricted network when DHCP health checks fail.",
                Tags = ["nap", "dhcp", "quarantine", "network"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "DHCP NAP quarantine disabled; client bypasses network health checks via DHCP.",
                ApplyOps = [RegOp.SetDword(NapKey, "EnableDhcpQuarantine", 0)],
                RemoveOps = [RegOp.DeleteValue(NapKey, "EnableDhcpQuarantine")],
                DetectOps = [RegOp.CheckDword(NapKey, "EnableDhcpQuarantine", 0)],
            },
            new TweakDef
            {
                Id = "nappol-disable-8021x-quarantine",
                Label = "Disable NAP 802.1X Quarantine Enforcement",
                Category = "Network Protection Policy",
                Description =
                    "Sets Enable8021xQuarantine=0 to disable NAP enforcement through 802.1X-authenticated network switches. Prevents 802.1X-based client quarantine on wired/wireless networks.",
                Tags = ["nap", "802.1x", "quarantine", "network"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "802.1X NAP quarantine disabled; wired/wireless enforcement bypassed for this client.",
                ApplyOps = [RegOp.SetDword(NapKey, "Enable8021xQuarantine", 0)],
                RemoveOps = [RegOp.DeleteValue(NapKey, "Enable8021xQuarantine")],
                DetectOps = [RegOp.CheckDword(NapKey, "Enable8021xQuarantine", 0)],
            },
            new TweakDef
            {
                Id = "nappol-disable-vpn-quarantine",
                Label = "Disable NAP VPN Quarantine Enforcement",
                Category = "Network Protection Policy",
                Description =
                    "Sets EnableVpnQuarantine=0 to disable VPN-based NAP enforcement. Prevents VPN connections from triggering NAP health evaluations and potential quarantine.",
                Tags = ["nap", "vpn", "quarantine", "network"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 3,
                ImpactNote = "VPN health checks via NAP bypassed; may reduce VPN connection setup time.",
                ApplyOps = [RegOp.SetDword(NapKey, "EnableVpnQuarantine", 0)],
                RemoveOps = [RegOp.DeleteValue(NapKey, "EnableVpnQuarantine")],
                DetectOps = [RegOp.CheckDword(NapKey, "EnableVpnQuarantine", 0)],
            },
            new TweakDef
            {
                Id = "nappol-disable-ipsec-quarantine",
                Label = "Disable NAP IPSec Quarantine Enforcement",
                Category = "Network Protection Policy",
                Description =
                    "Sets EnableIpsecQuarantine=0 to disable IPSec-based NAP health enforcement. Prevents IPSec connections from routing through NAP health validation and quarantine zones.",
                Tags = ["nap", "ipsec", "quarantine", "network", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 3,
                ImpactNote = "IPSec NAP health checks disabled; IPSec connections bypass health gating.",
                ApplyOps = [RegOp.SetDword(NapKey, "EnableIpsecQuarantine", 0)],
                RemoveOps = [RegOp.DeleteValue(NapKey, "EnableIpsecQuarantine")],
                DetectOps = [RegOp.CheckDword(NapKey, "EnableIpsecQuarantine", 0)],
            },
            new TweakDef
            {
                Id = "nappol-disable-dhcp-auto-remediation",
                Label = "Disable NAP DHCP Auto-Remediation",
                Category = "Network Protection Policy",
                Description =
                    "Sets DisableDhcpAutoRemediation=1 to prevent the NAP client from automatically attempting to remediate health failures during DHCP-based enforcement. Manual intervention is required.",
                Tags = ["nap", "dhcp", "remediation", "network"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Automatic DHCP remediation disabled; health policy failures require manual administrator action.",
                ApplyOps = [RegOp.SetDword(NapKey, "DisableDhcpAutoRemediation", 1)],
                RemoveOps = [RegOp.DeleteValue(NapKey, "DisableDhcpAutoRemediation")],
                DetectOps = [RegOp.CheckDword(NapKey, "DisableDhcpAutoRemediation", 1)],
            },
            new TweakDef
            {
                Id = "nappol-disable-nap-status-notifications",
                Label = "Disable NAP Status Notifications",
                Category = "Network Protection Policy",
                Description =
                    "Sets DisableStatusNotifications=1 to suppress NAP status change notifications from appearing to users. Network Access Protection events are logged but not displayed.",
                Tags = ["nap", "notifications", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "NAP status notifications hidden from users; NAP events still logged in Event Viewer.",
                ApplyOps = [RegOp.SetDword(NapKey, "DisableStatusNotifications", 1)],
                RemoveOps = [RegOp.DeleteValue(NapKey, "DisableStatusNotifications")],
                DetectOps = [RegOp.CheckDword(NapKey, "DisableStatusNotifications", 1)],
            },
            new TweakDef
            {
                Id = "nappol-disable-nap-ui",
                Label = "Disable NAP User Interface",
                Category = "Network Protection Policy",
                Description =
                    "Sets DisableUserUi=1 to completely disable the Network Access Protection user interface. NAP-related dialogs and status pages are inaccessible to users.",
                Tags = ["nap", "ui", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "NAP UI fully disabled; no user-facing NAP dialogs, status screens, or repair wizards.",
                ApplyOps = [RegOp.SetDword(NapKey, "DisableUserUi", 1)],
                RemoveOps = [RegOp.DeleteValue(NapKey, "DisableUserUi")],
                DetectOps = [RegOp.CheckDword(NapKey, "DisableUserUi", 1)],
            },
            new TweakDef
            {
                Id = "nappol-hide-nap-tray-icon",
                Label = "Hide NAP System Tray Icon",
                Category = "Network Protection Policy",
                Description =
                    "Sets HideSystemTrayIcon=1 to prevent the NAP system tray notification icon from appearing. Reduces status bar clutter when NAP components are otherwise disabled.",
                Tags = ["nap", "tray", "ui", "network"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "NAP tray icon hidden; no impact on networking, only removes the visual indicator.",
                ApplyOps = [RegOp.SetDword(NapKey, "HideSystemTrayIcon", 1)],
                RemoveOps = [RegOp.DeleteValue(NapKey, "HideSystemTrayIcon")],
                DetectOps = [RegOp.CheckDword(NapKey, "HideSystemTrayIcon", 1)],
            },
            new TweakDef
            {
                Id = "nappol-disable-nap-policy-autoupdate",
                Label = "Disable NAP Policy Auto-Update",
                Category = "Network Protection Policy",
                Description =
                    "Sets DisablePolicyAutoUpdate=1 to prevent the NAP client from automatically downloading updated health requirement policies from the network policy server (NPS).",
                Tags = ["nap", "policy", "update", "network"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "NAP policy updates disabled; client retains last-known health policy settings.",
                ApplyOps = [RegOp.SetDword(NapKey, "DisablePolicyAutoUpdate", 1)],
                RemoveOps = [RegOp.DeleteValue(NapKey, "DisablePolicyAutoUpdate")],
                DetectOps = [RegOp.CheckDword(NapKey, "DisablePolicyAutoUpdate", 1)],
            },
        ];
}
