// RegiLattice.Core — Tweaks/NetLocationAwarenessAdvancedPolicy.cs
// Network Location Awareness (NLA) advanced profile, classification, and sensitivity policy — Sprint 501.
// Category: "NLA Advanced Policy" | Slug: nlaadv
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\NetworkList\Signatures

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class NetLocationAwarenessAdvancedPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkList\Signatures";
    private const string NlmKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkList";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "nlaadv-always-classify-as-public",
                Label = "Always Classify Unrecognised Networks as Public",
                Category = "NLA Advanced Policy",
                Description =
                    "Forces Windows to classify all new or unrecognised network connections as Public network profile (most restrictive firewall rules) until explicitly reclassified by an administrator, applying maximum firewall protection to unknown networks.",
                Tags = ["nla", "network-classification", "public-profile", "firewall", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Unknown networks classified as Public; most restrictive firewall rules apply to all unrecognised connections.",
                ApplyOps = [RegOp.SetDword(NlmKey, "DefaultClassification", 0)],
                RemoveOps = [RegOp.DeleteValue(NlmKey, "DefaultClassification")],
                DetectOps = [RegOp.CheckDword(NlmKey, "DefaultClassification", 0)],
            },
            new TweakDef
            {
                Id = "nlaadv-block-user-profile-reclassification",
                Label = "Block Standard Users from Reclassifying Network Profiles",
                Category = "NLA Advanced Policy",
                Description =
                    "Prevents standard users from changing a network's classification (Private/Public/Domain Work) in Windows, ensuring that firewall profile assignments can only be modified by administrators.",
                Tags = ["nla", "network-profile", "reclassification", "standard-user", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Network profile reclassification blocked for standard users; firewall profile changes require admin.",
                ApplyOps = [RegOp.SetDword(NlmKey, "AllowUserSetNetworkLocation", 0)],
                RemoveOps = [RegOp.DeleteValue(NlmKey, "AllowUserSetNetworkLocation")],
                DetectOps = [RegOp.CheckDword(NlmKey, "AllowUserSetNetworkLocation", 0)],
            },
            new TweakDef
            {
                Id = "nlaadv-disable-domain-network-upgrade",
                Label = "Disable Automatic Domain Network Upgrade from NLA",
                Category = "NLA Advanced Policy",
                Description =
                    "Prevents NLA from automatically reclassifying a network from Public/Private to Domain Work profile when domain controllers are reachable, keeping explicit admin-assigned firewall profiles even on domain member machines.",
                Tags = ["nla", "domain-profile", "auto-detect", "firewall", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "NLA domain auto-upgrade disabled; domain networks stay at assigned profile without auto-promotion.",
                ApplyOps = [RegOp.SetDword(NlmKey, "DisableDomainNetworkAutoDetect", 1)],
                RemoveOps = [RegOp.DeleteValue(NlmKey, "DisableDomainNetworkAutoDetect")],
                DetectOps = [RegOp.CheckDword(NlmKey, "DisableDomainNetworkAutoDetect", 1)],
            },
            new TweakDef
            {
                Id = "nlaadv-log-profile-change-events",
                Label = "Log Network Profile Change Events",
                Category = "NLA Advanced Policy",
                Description =
                    "Enables System event log entries when a network connection profile is changed (Private to Public, etc.), providing audit visibility into firewall profile transitions that could weaken security posture.",
                Tags = ["nla", "network-profile", "event-log", "audit", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Network profile changes logged; firewall profile transitions recorded in System event log.",
                ApplyOps = [RegOp.SetDword(NlmKey, "LogNetworkProfileChanges", 1)],
                RemoveOps = [RegOp.DeleteValue(NlmKey, "LogNetworkProfileChanges")],
                DetectOps = [RegOp.CheckDword(NlmKey, "LogNetworkProfileChanges", 1)],
            },
            new TweakDef
            {
                Id = "nlaadv-disable-nla-internet-probe",
                Label = "Disable NLA Internet Connectivity Probe (NCSI Bypass)",
                Category = "NLA Advanced Policy",
                Description =
                    "Disables the Network Connectivity Status Indicator (NCSI) probe that NLA sends to Microsoft servers (connectivity.microsoft.com) to determine internet connectivity status, preventing outbound probe traffic to cloud hosts.",
                Tags = ["nla", "ncsi", "connectivity-probe", "microsoft", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "NCSI internet probe disabled; no connectivity.microsoft.com probe. System tray may show 'No internet' falsely.",
                ApplyOps = [RegOp.SetDword(NlmKey, "DisableNCSI", 1)],
                RemoveOps = [RegOp.DeleteValue(NlmKey, "DisableNCSI")],
                DetectOps = [RegOp.CheckDword(NlmKey, "DisableNCSI", 1)],
            },
            new TweakDef
            {
                Id = "nlaadv-disable-captive-portal-detect",
                Label = "Disable Captive Portal Detection",
                Category = "NLA Advanced Policy",
                Description =
                    "Disables Windows captive portal detection that redirects a browser to a hotel/airport landing page, preventing unwanted browser launches in locked-down environments and avoiding false-positive network change alerts.",
                Tags = ["nla", "captive-portal", "hotspot", "browser", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Captive portal detection disabled; Windows does not auto-open browser when hotspot login required.",
                ApplyOps = [RegOp.SetDword(NlmKey, "DisableCaptivePortalDetection", 1)],
                RemoveOps = [RegOp.DeleteValue(NlmKey, "DisableCaptivePortalDetection")],
                DetectOps = [RegOp.CheckDword(NlmKey, "DisableCaptivePortalDetection", 1)],
            },
            new TweakDef
            {
                Id = "nlaadv-require-auth-for-private-upgrade",
                Label = "Require User Authentication Before Private Network Upgrade",
                Category = "NLA Advanced Policy",
                Description =
                    "Requires an administrator confirmation before NLA upgrades a network from Public to Private profile, preventing accidental loosening of firewall rules when a device connects to an unknown but trusted-seeming network.",
                Tags = ["nla", "private-profile", "authentication", "firewall", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Admin confirmation required before Private profile upgrade; prevents accidental firewall relaxation.",
                ApplyOps = [RegOp.SetDword(NlmKey, "RequireAuthForPrivateNetworkUpgrade", 1)],
                RemoveOps = [RegOp.DeleteValue(NlmKey, "RequireAuthForPrivateNetworkUpgrade")],
                DetectOps = [RegOp.CheckDword(NlmKey, "RequireAuthForPrivateNetworkUpgrade", 1)],
            },
            new TweakDef
            {
                Id = "nlaadv-block-network-name-ui",
                Label = "Hide Network Name and Location in System Tray",
                Category = "NLA Advanced Policy",
                Description =
                    "Removes the network name and location type from the system tray network flyout, preventing casual users from seeing and potentially modifying network profile names or locations.",
                Tags = ["nla", "system-tray", "network-name", "ui", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Network name hidden in system tray; network profile names and types not shown in flyout.",
                ApplyOps = [RegOp.SetDword(NlmKey, "HideNetworkLocationUI", 1)],
                RemoveOps = [RegOp.DeleteValue(NlmKey, "HideNetworkLocationUI")],
                DetectOps = [RegOp.CheckDword(NlmKey, "HideNetworkLocationUI", 1)],
            },
            new TweakDef
            {
                Id = "nlaadv-disable-network-telemetry",
                Label = "Disable NLA Network Profile Telemetry to Microsoft",
                Category = "NLA Advanced Policy",
                Description =
                    "Prevents Network Location Awareness from sending network profile assignment and classification telemetry to Microsoft, protecting information about this machine's network environment from cloud disclosure.",
                Tags = ["nla", "telemetry", "privacy", "microsoft", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "NLA telemetry to Microsoft disabled; network profile assignment and connectivity data not sent to cloud.",
                ApplyOps = [RegOp.SetDword(NlmKey, "DisableNLATelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(NlmKey, "DisableNLATelemetry")],
                DetectOps = [RegOp.CheckDword(NlmKey, "DisableNLATelemetry", 1)],
            },
            new TweakDef
            {
                Id = "nlaadv-set-profile-icon-classification",
                Label = "Set Network Profile Icon to Reflect Classification",
                Category = "NLA Advanced Policy",
                Description =
                    "Configures the network profile icon in the system tray to visually reflect the current classification (Public/Private/Domain) to ensure users have immediate visual awareness of the active firewall profile strength.",
                Tags = ["nla", "network-icon", "ui", "firewall-profile", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Network icon reflects profile classification; users see current Public/Private/Domain firewall level.",
                ApplyOps = [RegOp.SetDword(NlmKey, "ShowProfileClassificationIcon", 1)],
                RemoveOps = [RegOp.DeleteValue(NlmKey, "ShowProfileClassificationIcon")],
                DetectOps = [RegOp.CheckDword(NlmKey, "ShowProfileClassificationIcon", 1)],
            },
        ];
}
