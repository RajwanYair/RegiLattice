// RegiLattice.Core — Tweaks/DefenderFirewallAdvancedPolicy.cs
// Windows Firewall domain/standard profile hardening via Group Policy — Sprint 437.
// Enforces firewall state, default inbound action, notifications, dropped-packet
// logging, log-size limits, and unicast-response policy on both profiles.
// Category: "Defender Firewall Advanced" | Slug: fwadv
// Registry: HKLM\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile
//           HKLM\SOFTWARE\Policies\Microsoft\WindowsFirewall\StandardProfile

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class DefenderFirewallAdvancedPolicy
{
    private const string Domain = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile";
    private const string Standard = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\StandardProfile";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "fwadv-enable-domain-firewall",
                Label = "Enable Windows Firewall — Domain Profile",
                Category = "Defender Firewall Advanced",
                Description =
                    "Enforces Windows Firewall enabled state on the domain network profile via Group Policy, preventing local override by users.",
                Tags = ["firewall", "domain", "policy", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Prevents domain users from disabling the firewall through local settings.",
                ApplyOps = [RegOp.SetDword(Domain, "EnableFirewall", 1)],
                RemoveOps = [RegOp.DeleteValue(Domain, "EnableFirewall")],
                DetectOps = [RegOp.CheckDword(Domain, "EnableFirewall", 1)],
            },
            new TweakDef
            {
                Id = "fwadv-enable-standard-firewall",
                Label = "Enable Windows Firewall — Standard Profile",
                Category = "Defender Firewall Advanced",
                Description = "Enforces Windows Firewall enabled on private and public network profiles via Group Policy.",
                Tags = ["firewall", "standard", "policy", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Enforces firewall-on for private and public profiles; blocks local override.",
                ApplyOps = [RegOp.SetDword(Standard, "EnableFirewall", 1)],
                RemoveOps = [RegOp.DeleteValue(Standard, "EnableFirewall")],
                DetectOps = [RegOp.CheckDword(Standard, "EnableFirewall", 1)],
            },
            new TweakDef
            {
                Id = "fwadv-block-inbound-domain",
                Label = "Block Inbound Connections by Default — Domain Profile",
                Category = "Defender Firewall Advanced",
                Description =
                    "Sets DefaultInboundAction=1 (Block) on the domain profile. All inbound traffic is blocked unless explicitly permitted by a firewall rule.",
                Tags = ["firewall", "inbound", "domain", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Blocks unsolicited inbound on domain networks; pre-configure required inbound rules.",
                ApplyOps = [RegOp.SetDword(Domain, "DefaultInboundAction", 1)],
                RemoveOps = [RegOp.DeleteValue(Domain, "DefaultInboundAction")],
                DetectOps = [RegOp.CheckDword(Domain, "DefaultInboundAction", 1)],
            },
            new TweakDef
            {
                Id = "fwadv-block-inbound-standard",
                Label = "Block Inbound Connections by Default — Standard Profile",
                Category = "Defender Firewall Advanced",
                Description = "Sets DefaultInboundAction=1 (Block) on the standard profile, protecting devices on private and public networks.",
                Tags = ["firewall", "inbound", "standard", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Blocks unsolicited inbound on private/public profiles.",
                ApplyOps = [RegOp.SetDword(Standard, "DefaultInboundAction", 1)],
                RemoveOps = [RegOp.DeleteValue(Standard, "DefaultInboundAction")],
                DetectOps = [RegOp.CheckDword(Standard, "DefaultInboundAction", 1)],
            },
            new TweakDef
            {
                Id = "fwadv-enable-notifications-domain",
                Label = "Enable Blocked-App Notifications — Domain Profile",
                Category = "Defender Firewall Advanced",
                Description = "Sets DisableNotifications=0 so users see a notification when the firewall blocks a new program on the domain profile.",
                Tags = ["firewall", "notifications", "domain", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Improves visibility of new blocked apps without weakening security.",
                ApplyOps = [RegOp.SetDword(Domain, "DisableNotifications", 0)],
                RemoveOps = [RegOp.DeleteValue(Domain, "DisableNotifications")],
                DetectOps = [RegOp.CheckDword(Domain, "DisableNotifications", 0)],
            },
            new TweakDef
            {
                Id = "fwadv-enable-notifications-standard",
                Label = "Enable Blocked-App Notifications — Standard Profile",
                Category = "Defender Firewall Advanced",
                Description =
                    "Sets DisableNotifications=0 on the standard profile so users see notifications when the firewall blocks a new application.",
                Tags = ["firewall", "notifications", "standard", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Notifies users of blocked apps on private/public networks.",
                ApplyOps = [RegOp.SetDword(Standard, "DisableNotifications", 0)],
                RemoveOps = [RegOp.DeleteValue(Standard, "DisableNotifications")],
                DetectOps = [RegOp.CheckDword(Standard, "DisableNotifications", 0)],
            },
            new TweakDef
            {
                Id = "fwadv-log-dropped-domain",
                Label = "Log Dropped Packets — Domain Profile",
                Category = "Defender Firewall Advanced",
                Description =
                    "Enables Windows Firewall logging of dropped packets on the domain profile for security auditing and incident response.",
                Tags = ["firewall", "logging", "domain", "audit"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Dropped-packet log aids forensic investigation of blocked domain traffic.",
                ApplyOps = [RegOp.SetDword(Domain, "EnableLogDroppedPackets", 1)],
                RemoveOps = [RegOp.DeleteValue(Domain, "EnableLogDroppedPackets")],
                DetectOps = [RegOp.CheckDword(Domain, "EnableLogDroppedPackets", 1)],
            },
            new TweakDef
            {
                Id = "fwadv-log-dropped-standard",
                Label = "Log Dropped Packets — Standard Profile",
                Category = "Defender Firewall Advanced",
                Description =
                    "Enables Windows Firewall dropped-packet logging on the standard profile for forensic auditing of private/public-network traffic.",
                Tags = ["firewall", "logging", "standard", "audit"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Provides packet-drop logs for incident analysis on private/public networks.",
                ApplyOps = [RegOp.SetDword(Standard, "EnableLogDroppedPackets", 1)],
                RemoveOps = [RegOp.DeleteValue(Standard, "EnableLogDroppedPackets")],
                DetectOps = [RegOp.CheckDword(Standard, "EnableLogDroppedPackets", 1)],
            },
            new TweakDef
            {
                Id = "fwadv-log-max-size-domain",
                Label = "Set Firewall Log Max Size 16 MB — Domain Profile",
                Category = "Defender Firewall Advanced",
                Description =
                    "Sets the Windows Firewall log maximum to 16384 KB (16 MB) on the domain profile, retaining substantially more history for incident analysis.",
                Tags = ["firewall", "logging", "domain", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Larger log file preserves more drop history; negligible disk usage.",
                ApplyOps = [RegOp.SetDword(Domain, "LogFileSize", 16384)],
                RemoveOps = [RegOp.DeleteValue(Domain, "LogFileSize")],
                DetectOps = [RegOp.CheckDword(Domain, "LogFileSize", 16384)],
            },
            new TweakDef
            {
                Id = "fwadv-disable-unicast-domain",
                Label = "Disable Unicast Response to Multicast/Broadcast — Domain",
                Category = "Defender Firewall Advanced",
                Description =
                    "Prevents unicast replies to multicast/broadcast frames on the domain profile, reducing exposure to network-scanning reconnaissance (DisableUnicastResponsesToMulticastBroadcast=1).",
                Tags = ["firewall", "multicast", "domain", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Suppresses unicast responses to broadcast probes; limits host discovery.",
                ApplyOps = [RegOp.SetDword(Domain, "DisableUnicastResponsesToMulticastBroadcast", 1)],
                RemoveOps = [RegOp.DeleteValue(Domain, "DisableUnicastResponsesToMulticastBroadcast")],
                DetectOps = [RegOp.CheckDword(Domain, "DisableUnicastResponsesToMulticastBroadcast", 1)],
            },
        ];
}
