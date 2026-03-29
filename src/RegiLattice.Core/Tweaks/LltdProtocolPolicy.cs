// RegiLattice.Core — Tweaks/LltdProtocolPolicy.cs
// Link Layer Topology Discovery (LLTD) protocol and network map responder policy — Sprint 498.
// Category: "LLTD Protocol Policy" | Slug: lltdpol
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\LLTD

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class LltdProtocolPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LLTD";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "lltdpol-disable-lltd-io",
                Label = "Disable LLTD I/O (Network Map Responder on Private Networks)",
                Category = "LLTD Protocol Policy",
                Description =
                    "Disables the LLTD I/O component that allows this machine to respond to Link Layer Topology Discovery queries on private networks (home/work), preventing its network adapters from appearing in the Windows Network Map.",
                Tags = ["lltd", "network-map", "topology-discovery", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "LLTD I/O on private networks disabled; machine removed from Windows Network Map on private networks.",
                ApplyOps = [RegOp.SetDword(Key, "EnableLLTDIO", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableLLTDIO")],
                DetectOps = [RegOp.CheckDword(Key, "EnableLLTDIO", 0)],
            },
            new TweakDef
            {
                Id = "lltdpol-disable-lltd-io-domain",
                Label = "Disable LLTD I/O on Domain Networks",
                Category = "LLTD Protocol Policy",
                Description =
                    "Disables the LLTD I/O component on domain-authenticated networks, preventing this machine from exposing its network topology to network discovery tools on corporate domain networks.",
                Tags = ["lltd", "domain-network", "topology-discovery", "corporate", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "LLTD I/O on domain networks disabled; machine does not respond to topology probes on domain networks.",
                ApplyOps = [RegOp.SetDword(Key, "AllowLLTDIOOnDomain", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowLLTDIOOnDomain")],
                DetectOps = [RegOp.CheckDword(Key, "AllowLLTDIOOnDomain", 0)],
            },
            new TweakDef
            {
                Id = "lltdpol-disable-lltd-io-public",
                Label = "Disable LLTD I/O on Public Networks",
                Category = "LLTD Protocol Policy",
                Description =
                    "Disables the LLTD I/O component on public networks (airports, hotels, coffee shops), preventing network enumeration of this machine by other devices on untrusted public Wi-Fi networks.",
                Tags = ["lltd", "public-network", "topology-discovery", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "LLTD I/O on public networks disabled; machine not discoverable on hotel/airport/coffee-shop Wi-Fi.",
                ApplyOps = [RegOp.SetDword(Key, "AllowLLTDIOOnPublicNet", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowLLTDIOOnPublicNet")],
                DetectOps = [RegOp.CheckDword(Key, "AllowLLTDIOOnPublicNet", 0)],
            },
            new TweakDef
            {
                Id = "lltdpol-disable-rspndr",
                Label = "Disable LLTD Responder Component on Private Networks",
                Category = "LLTD Protocol Policy",
                Description =
                    "Disables the LLTD Responder driver (rspndr) on private networks, preventing this machine from sending LLTD discovery responses that reveal its presence and IP/MAC mapping to network topology collectors.",
                Tags = ["lltd", "responder", "network-discovery", "mac-address", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "LLTD Responder disabled on private networks; MAC address and IP not revealed via discovery responses.",
                ApplyOps = [RegOp.SetDword(Key, "EnableRspndr", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableRspndr")],
                DetectOps = [RegOp.CheckDword(Key, "EnableRspndr", 0)],
            },
            new TweakDef
            {
                Id = "lltdpol-disable-rspndr-domain",
                Label = "Disable LLTD Responder on Domain Networks",
                Category = "LLTD Protocol Policy",
                Description =
                    "Disables the LLTD Responder driver on domain-authenticated networks, preventing topology discovery responses on corporate LANs where network mapping is managed exclusively by centralised network tools.",
                Tags = ["lltd", "responder", "domain-network", "corporate", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "LLTD Responder disabled on domain networks; machine not visible in Windows Network Map on domain.",
                ApplyOps = [RegOp.SetDword(Key, "AllowRspndrOnDomain", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowRspndrOnDomain")],
                DetectOps = [RegOp.CheckDword(Key, "AllowRspndrOnDomain", 0)],
            },
            new TweakDef
            {
                Id = "lltdpol-disable-rspndr-public",
                Label = "Disable LLTD Responder on Public Networks",
                Category = "LLTD Protocol Policy",
                Description =
                    "Disables the LLTD Responder driver on public networks, ensuring that this machine does not expose its presence, MAC address, or IP mapping to other hosts on untrusted public network segments.",
                Tags = ["lltd", "responder", "public-network", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "LLTD Responder disabled on public networks; presence not exposed on untrusted public Wi-Fi.",
                ApplyOps = [RegOp.SetDword(Key, "AllowRspndrOnPublicNet", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowRspndrOnPublicNet")],
                DetectOps = [RegOp.CheckDword(Key, "AllowRspndrOnPublicNet", 0)],
            },
            new TweakDef
            {
                Id = "lltdpol-log-lltd-probe-events",
                Label = "Enable Logging of LLTD Discovery Probe Events",
                Category = "LLTD Protocol Policy",
                Description =
                    "Enables event log entries when LLTD discovery probes are received, providing audit trail of which hosts on the network are conducting topology discovery scans against this machine.",
                Tags = ["lltd", "audit", "discovery-probe", "event-log", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "LLTD probe receipt events logged; topology scanning activity against this machine auditable.",
                ApplyOps = [RegOp.SetDword(Key, "LogLLTDProbeEvents", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "LogLLTDProbeEvents")],
                DetectOps = [RegOp.CheckDword(Key, "LogLLTDProbeEvents", 1)],
            },
            new TweakDef
            {
                Id = "lltdpol-disable-managed-network-qos",
                Label = "Disable LLTD Managed Network QoS Signalling",
                Category = "LLTD Protocol Policy",
                Description =
                    "Disables the LLTD managed network Quality of Service signalling extension, preventing this machine from participating in QoS scheduling signals broadcast over LLTD on Windows home network environments.",
                Tags = ["lltd", "qos", "network-management", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "LLTD QoS signalling disabled; machine does not participate in LLTD-based bandwidth scheduling.",
                ApplyOps = [RegOp.SetDword(Key, "DisableLLTDQoSSignaling", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableLLTDQoSSignaling")],
                DetectOps = [RegOp.CheckDword(Key, "DisableLLTDQoSSignaling", 1)],
            },
            new TweakDef
            {
                Id = "lltdpol-block-lltd-service-admin-change",
                Label = "Block Admin From Re-Enabling LLTD Without Policy Override",
                Category = "LLTD Protocol Policy",
                Description =
                    "Prevents local administrators from re-enabling LLTD I/O or Responder components without a Group Policy override, ensuring that the LLTD disable policy cannot be circumvented at the local machine level.",
                Tags = ["lltd", "admin-lockdown", "gpo", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "LLTD cannot be re-enabled locally without GPO change; admin cannot override the detection disable.",
                ApplyOps = [RegOp.SetDword(Key, "BlockLocalLLTDOverride", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockLocalLLTDOverride")],
                DetectOps = [RegOp.CheckDword(Key, "BlockLocalLLTDOverride", 1)],
            },
            new TweakDef
            {
                Id = "lltdpol-disable-lltd-multicast",
                Label = "Disable LLTD Multicast Discovery on All Segments",
                Category = "LLTD Protocol Policy",
                Description =
                    "Disables LLTD multicast discovery messages sent across all network segments, preventing bandwidth consumption from periodic LLTD multicast discovery packets on busy enterprise networks.",
                Tags = ["lltd", "multicast", "bandwidth", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "LLTD multicast discovery disabled; no periodic LLTD multicast traffic generated on any segment.",
                ApplyOps = [RegOp.SetDword(Key, "DisableLLTDMulticast", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableLLTDMulticast")],
                DetectOps = [RegOp.CheckDword(Key, "DisableLLTDMulticast", 1)],
            },
        ];
}
