// RegiLattice.Core — Tweaks/FirewallProfileHardeningPolicy.cs
// Windows Defender Firewall per-profile stealth mode, unicast response, logging, and merge policy — Sprint 506.
// Category: "Firewall Profile Hardening Policy" | Slug: fwprof
// Registry: HKLM\SOFTWARE\Policies\Microsoft\WindowsFirewall\{Domain|Private|Public}Profile

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class FirewallProfileHardeningPolicy
{
    private const string DomainKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile";
    private const string PrivKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PrivateProfile";
    private const string PubKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PublicProfile";
    private const string DomLog = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile\Logging";
    private const string PubLog = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PublicProfile\Logging";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "fwprof-stealth-mode-private",
                Label = "Enable Firewall Stealth Mode on Private Networks",
                Category = "Firewall Profile Hardening Policy",
                Description =
                    "Enables stealth mode on the Private network profile, causing blocked inbound connection attempts to be silently dropped rather than returning RST/ICMP-unreachable, hiding this machine from port-scanner reconnaissance.",
                Tags = ["firewall", "stealth-mode", "private-profile", "port-scan", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Stealth mode on private networks; blocked ports silent. Machine harder to discover on home/office networks.",
                ApplyOps = [RegOp.SetDword(PrivKey, "DisableStealthMode", 0)],
                RemoveOps = [RegOp.DeleteValue(PrivKey, "DisableStealthMode")],
                DetectOps = [RegOp.CheckDword(PrivKey, "DisableStealthMode", 0)],
            },
            new TweakDef
            {
                Id = "fwprof-stealth-mode-domain",
                Label = "Enable Firewall Stealth Mode on Domain Networks",
                Category = "Firewall Profile Hardening Policy",
                Description =
                    "Enables stealth mode on the Domain network profile so that blocked inbound connection attempts are silently dropped on corporate networks, reducing noise and lateral-movement reconnaissance surface.",
                Tags = ["firewall", "stealth-mode", "domain-profile", "corporate", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Stealth mode on domain networks; blocked ports drop silently on corporate LAN.",
                ApplyOps = [RegOp.SetDword(DomainKey, "DisableStealthMode", 0)],
                RemoveOps = [RegOp.DeleteValue(DomainKey, "DisableStealthMode")],
                DetectOps = [RegOp.CheckDword(DomainKey, "DisableStealthMode", 0)],
            },
            new TweakDef
            {
                Id = "fwprof-block-local-merge-private",
                Label = "Block Local Firewall Rules from Overriding GPO on Private Profile",
                Category = "Firewall Profile Hardening Policy",
                Description =
                    "Prevents locally defined firewall rules from overriding Group Policy rules on the Private profile, ensuring GPO-deployed firewall rules cannot be undermined by applications or malware creating local exceptions.",
                Tags = ["firewall", "local-merge", "gpo", "private-profile", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Local rule merge blocked on private profile; only GPO rules apply. Local app exceptions cannot override.",
                ApplyOps = [RegOp.SetDword(PrivKey, "AllowLocalPolicyMerge", 0)],
                RemoveOps = [RegOp.DeleteValue(PrivKey, "AllowLocalPolicyMerge")],
                DetectOps = [RegOp.CheckDword(PrivKey, "AllowLocalPolicyMerge", 0)],
            },
            new TweakDef
            {
                Id = "fwprof-disable-notifications-private",
                Label = "Disable Firewall Blocked App Notifications on Private Profile",
                Category = "Firewall Profile Hardening Policy",
                Description =
                    "Suppresses the Windows Firewall notification that prompts users to approve newly blocked applications on Private networks, preventing non-admin users from weakening firewall policy via approval notifications.",
                Tags = ["firewall", "notification", "blocked-app", "private-profile", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Blocked app notifications suppressed on private profile; users cannot approve exceptions via notification.",
                ApplyOps = [RegOp.SetDword(PrivKey, "DisableNotifications", 1)],
                RemoveOps = [RegOp.DeleteValue(PrivKey, "DisableNotifications")],
                DetectOps = [RegOp.CheckDword(PrivKey, "DisableNotifications", 1)],
            },
            new TweakDef
            {
                Id = "fwprof-log-dropped-packets-public",
                Label = "Log Dropped Packets on Public Firewall Profile",
                Category = "Firewall Profile Hardening Policy",
                Description =
                    "Enables firewall logging of dropped packet events on the Public profile, recording all blocked inbound/outbound connection attempts on public networks for security incident investigation.",
                Tags = ["firewall", "log-dropped", "public-profile", "audit", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Dropped packets logged on public profile; blocked connections recorded in public profile firewall log.",
                ApplyOps = [RegOp.SetDword(PubLog, "LogDroppedPackets", 1)],
                RemoveOps = [RegOp.DeleteValue(PubLog, "LogDroppedPackets")],
                DetectOps = [RegOp.CheckDword(PubLog, "LogDroppedPackets", 1)],
            },
            new TweakDef
            {
                Id = "fwprof-log-allowed-packets-public",
                Label = "Log Allowed Connections on Public Firewall Profile",
                Category = "Firewall Profile Hardening Policy",
                Description =
                    "Enables firewall logging of successfully allowed connections on the Public profile, providing a record of all established connections on public networks for behavioural baselining and anomaly detection.",
                Tags = ["firewall", "log-allowed", "public-profile", "audit", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Allowed connections logged on public profile; all successful outbound traffic recorded in firewall log.",
                ApplyOps = [RegOp.SetDword(PubLog, "LogSuccessfulConnections", 1)],
                RemoveOps = [RegOp.DeleteValue(PubLog, "LogSuccessfulConnections")],
                DetectOps = [RegOp.CheckDword(PubLog, "LogSuccessfulConnections", 1)],
            },
            new TweakDef
            {
                Id = "fwprof-set-log-max-size-domain",
                Label = "Set Maximum Firewall Log Size to 32 MB on Domain Profile",
                Category = "Firewall Profile Hardening Policy",
                Description =
                    "Sets the maximum firewall log file size to 32768 KB (32 MB) on the Domain profile, ensuring sufficient log retention for forensic investigation without unbounded disk consumption.",
                Tags = ["firewall", "log-size", "domain-profile", "logging", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Domain profile firewall log capped at 32 MB; adequate retention for investigation without disk overflow.",
                ApplyOps = [RegOp.SetDword(DomLog, "LogFileSize", 32768)],
                RemoveOps = [RegOp.DeleteValue(DomLog, "LogFileSize")],
                DetectOps = [RegOp.CheckDword(DomLog, "LogFileSize", 32768)],
            },
            new TweakDef
            {
                Id = "fwprof-set-log-max-size-public",
                Label = "Set Maximum Firewall Log Size to 32 MB on Public Profile",
                Category = "Firewall Profile Hardening Policy",
                Description =
                    "Sets the maximum firewall log file size to 32768 KB (32 MB) on the Public profile, ensuring that firewall log entries on untrusted public networks are retained for post-incident forensic analysis.",
                Tags = ["firewall", "log-size", "public-profile", "logging", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Public profile firewall log capped at 32 MB; public network activity retained for post-incident review.",
                ApplyOps = [RegOp.SetDword(PubLog, "LogFileSize", 32768)],
                RemoveOps = [RegOp.DeleteValue(PubLog, "LogFileSize")],
                DetectOps = [RegOp.CheckDword(PubLog, "LogFileSize", 32768)],
            },
            new TweakDef
            {
                Id = "fwprof-unicast-no-response-private",
                Label = "Disable Unicast Responses to Multicast on Private Profile",
                Category = "Firewall Profile Hardening Policy",
                Description =
                    "Prevents the firewall from sending unicast replies to multicast and broadcast packets on the Private profile, closing a live-host detection technique used by network scanners that evade ICMP filtering.",
                Tags = ["firewall", "unicast-response", "multicast", "private-profile", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Unicast responses to multicast/broadcast disabled on private profile; host enumeration vector closed.",
                ApplyOps = [RegOp.SetDword(PrivKey, "DisableUnicastResponsesToMulticastBroadcast", 1)],
                RemoveOps = [RegOp.DeleteValue(PrivKey, "DisableUnicastResponsesToMulticastBroadcast")],
                DetectOps = [RegOp.CheckDword(PrivKey, "DisableUnicastResponsesToMulticastBroadcast", 1)],
            },
            new TweakDef
            {
                Id = "fwprof-block-ipsec-exempt-multicast",
                Label = "Block IPsec Exemption for Multicast and Broadcast Traffic",
                Category = "Firewall Profile Hardening Policy",
                Description =
                    "Removes the default IPsec exemption that allows multicast and broadcast traffic to bypass IPsec policy enforcement, ensuring all traffic — including multicast — is subject to IPsec rules on protected networks.",
                Tags = ["firewall", "ipsec", "multicast", "exemption", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "IPsec multicast/broadcast exemption removed; multicast traffic subject to IPsec enforcement. May break mDNS.",
                ApplyOps = [RegOp.SetDword(DomainKey, "IPsecExemptMulticast", 0)],
                RemoveOps = [RegOp.DeleteValue(DomainKey, "IPsecExemptMulticast")],
                DetectOps = [RegOp.CheckDword(DomainKey, "IPsecExemptMulticast", 0)],
            },
        ];
}
