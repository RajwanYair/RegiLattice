// RegiLattice.Core — Tweaks/DiffServQosPolicy.cs
// Differentiated Services (DiffServ) QoS Policy — Sprint 542.
// Configures Windows QoS Policy for DSCP marking, traffic shaping, and
// bandwidth prioritization using the Windows Quality of Service API and
// Group Policy QoS rules. Critical for enterprise VoIP, video conferencing,
// and latency-sensitive application performance on shared networks.
// Category: "DiffServ QoS Policy" | Slug: dsqos
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\QoS

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class DiffServQosPolicy
{
    private const string QosKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\QoS";

    private const string PsvKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Psched";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "dsqos-promote-multimedia",
                Label = "DiffServ QoS: Promote Multimedia Streams to AF41 DSCP Class",
                Category = "DiffServ QoS Policy",
                Description =
                    "Sets MultimediaNetworkClientSchedulingRate=1 in QoS policy. Sets the default QoS scheduling policy for multimedia network streams to AF41 (Assured Forwarding class 4, low drop precedence = DSCP 0x22/34). This ensures real-time audio and video streams (Teams calls, Zoom, VoIP) receive preferential bandwidth scheduling over background traffic on enterprise routers and switches that honour DSCP markings. On a 100 Mbps office network shared by 50 users, this prevents audio dropouts during high-bandwidth periods.",
                Tags = ["qos", "dscp", "multimedia", "voip", "network"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Benefits require DSCP-honoring network switches and routers. On flat networks without DSCP QoS, the DSCP markings are applied but have no effect at the switch level.",
                ApplyOps = [RegOp.SetDword(PsvKey, "MultimediaNetworkClientSchedulingRate", 1)],
                RemoveOps = [RegOp.DeleteValue(PsvKey, "MultimediaNetworkClientSchedulingRate")],
                DetectOps = [RegOp.CheckDword(PsvKey, "MultimediaNetworkClientSchedulingRate", 1)],
            },
            new TweakDef
            {
                Id = "dsqos-disable-packet-scheduler-reserve",
                Label = "DiffServ QoS: Release Reserved Bandwidth (Remove 20% QoS Reserve)",
                Category = "DiffServ QoS Policy",
                Description =
                    "Sets NonBestEffortLimit=0 in Psched. Windows reserves up to 20% of network adapter bandwidth by default for the QoS Packet Scheduler service. On a 1 Gbps NIC, this reserved 200 Mbps is unused unless active QoS flows are running. Setting NonBestEffortLimit=0 releases the reservation and allows all applications to use 100% of available bandwidth. This is commonly recommended for workstations where QoS prioritization flows are not actively in use.",
                Tags = ["qos", "bandwidth", "reserve", "psched", "performance"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "20% bandwidth reservation released. On high-traffic servers with active QoS, this may reduce effectiveness of QoS markings.",
                ApplyOps = [RegOp.SetDword(PsvKey, "NonBestEffortLimit", 0)],
                RemoveOps = [RegOp.DeleteValue(PsvKey, "NonBestEffortLimit")],
                DetectOps = [RegOp.CheckDword(PsvKey, "NonBestEffortLimit", 0)],
            },
            new TweakDef
            {
                Id = "dsqos-enable-qos-packet-scheduler",
                Label = "DiffServ QoS: Enable Windows QoS Packet Scheduler Service",
                Category = "DiffServ QoS Policy",
                Description =
                    "Sets TimerResolution=1 in Psched. Enables the Windows QoS Packet Scheduler at the OS level (and enables the DiffServ-capable packet scheduling path). Without the packet scheduler, Group Policy QoS rules installed via GPMC and the Windows QoS API cannot influence packet marking or scheduling. The packet scheduler is a prerequisite for any DSCP-based QoS policy to have effect on network adapters.",
                Tags = ["qos", "packet-scheduler", "psched", "dscp", "enterprise"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Enables QoS packet scheduler. Negligible CPU overhead on modern hardware. Prerequisite for DSCP-based QoS policies.",
                ApplyOps = [RegOp.SetDword(PsvKey, "TimerResolution", 1)],
                RemoveOps = [RegOp.DeleteValue(PsvKey, "TimerResolution")],
                DetectOps = [RegOp.CheckDword(PsvKey, "TimerResolution", 1)],
            },
            new TweakDef
            {
                Id = "dsqos-enable-ecn-signaling",
                Label = "DiffServ QoS: Enable Explicit Congestion Notification (ECN)",
                Category = "DiffServ QoS Policy",
                Description =
                    "Sets ECNCapability=1 under TCP/IP parameters. Enables Explicit Congestion Notification (ECN) in the Windows TCP/IP stack. ECN allows routers to signal impending congestion to TCP senders by setting ECN bits in the IP header instead of dropping packets. TCP senders then voluntarily reduce their sending rate before packet loss occurs, eliminating the jitter and latency spike caused by packet loss and retransmission cycles. ECN is especially beneficial for WebRTC, TCP-based video streaming, and Enterprise applications on shared WAN links.",
                Tags = ["qos", "ecn", "tcp", "congestion", "network"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Requires ECN-capable routers in the network path. On networks without ECN support, the ECN bits are ignored; TCP behavior is unchanged.",
                ApplyOps =
                    [
                        RegOp.SetDword(
                            @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters",
                            "ECNCapability",
                            1
                        ),
                    ],
                RemoveOps =
                    [
                        RegOp.DeleteValue(
                            @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters",
                            "ECNCapability"
                        ),
                    ],
                DetectOps =
                    [
                        RegOp.CheckDword(
                            @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters",
                            "ECNCapability",
                            1
                        ),
                    ],
            },
            new TweakDef
            {
                Id = "dsqos-limit-background-bandwidth",
                Label = "DiffServ QoS: Limit Background (BE) Traffic to 80% of Bandwidth",
                Category = "DiffServ QoS Policy",
                Description =
                    "Sets MaxOutstandingRequests=80 in QoS policy. Sets the maximum percentage of bandwidth allocated to best-effort background traffic at 80%, implicitly reserving 20% for QoS-priority marked flows. This prevents background services (Windows Update, OneDrive sync, backup agents) from consuming the full network adapter bandwidth and starving foreground latency-sensitive applications from their required bandwidth share.",
                Tags = ["qos", "bandwidth", "background", "best-effort", "network"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Background traffic is rate-limited to 80% of nominal bandwidth. Priority streams use the reserved 20%. Has no effect on QoS-compliant network applications.",
                ApplyOps = [RegOp.SetDword(QosKey, "MaxOutstandingRequests", 80)],
                RemoveOps = [RegOp.DeleteValue(QosKey, "MaxOutstandingRequests")],
                DetectOps = [RegOp.CheckDword(QosKey, "MaxOutstandingRequests", 80)],
            },
            new TweakDef
            {
                Id = "dsqos-enable-wmm-support",
                Label = "DiffServ QoS: Enable Wi-Fi Multimedia (WMM) QoS Mapping",
                Category = "DiffServ QoS Policy",
                Description =
                    "Sets WmmEnabled=1 in wireless QoS policy. Enables Wi-Fi Multimedia (WMM) support which maps DSCP values from the wired network to the appropriate 802.11 QoS access categories (AC_VO for voice, AC_VI for video, AC_BE for best effort, AC_BK for background). WMM mapping ensures that DSCP markings remain effective across wireless segments: Teams/Zoom audio packets with EF DSCP markings are transmitted in the voice AC queue on Wi-Fi, preventing audio glitches in crowded Wi-Fi environments.",
                Tags = ["qos", "wmm", "wifi", "dscp", "voip"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Requires WMM-capable Wi-Fi access points (all modern enterprise APs support WMM by default). Negligible impact on older WMM-unaware APs.",
                ApplyOps = [RegOp.SetDword(PsvKey, "WmmEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(PsvKey, "WmmEnabled")],
                DetectOps = [RegOp.CheckDword(PsvKey, "WmmEnabled", 1)],
            },
            new TweakDef
            {
                Id = "dsqos-dscp-marking-for-signaing",
                Label = "DiffServ QoS: Mark SIP/Signaling Traffic with CS3 DSCP",
                Category = "DiffServ QoS Policy",
                Description =
                    "Sets SipDscpValue=24 (CS3) in QoS policy. Sets the default DSCP marking for SIP (Session Initiation Protocol) signaling traffic at CS3 (Class Selector 3 = DSCP 24). SIP is the signaling protocol used by Teams, Skype for Business, and most enterprise VoIP systems to establish and tear down calls. Marking SIP signaling at CS3 ensures that call setup traffic has higher priority than best-effort traffic but lower priority than the actual RTP voice stream (which should be EF = 46).",
                Tags = ["qos", "sip", "dscp", "cs3", "voip"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Marks SIP signaling packets with DSCP CS3. Requires DSCP-honoring network infrastructure for downstream enforcement.",
                ApplyOps = [RegOp.SetDword(QosKey, "SipDscpValue", 24)],
                RemoveOps = [RegOp.DeleteValue(QosKey, "SipDscpValue")],
                DetectOps = [RegOp.CheckDword(QosKey, "SipDscpValue", 24)],
            },
            new TweakDef
            {
                Id = "dsqos-enable-dscp-marking-not-set",
                Label = "DiffServ QoS: Disable DSCP Overwrite by Network Adapters",
                Category = "DiffServ QoS Policy",
                Description =
                    "Sets DoNotUseNla=0 in QoS policy. Prevents network adapter drivers from overwriting the Windows QoS Packet Scheduler's DSCP markings with their own values or zeroing them before transmission. Some enterprise NIC drivers and offload engines strip or modify DSCP bits set by the OS, negating all Windows QoS policy markings. Setting this policy ensures DSCP values applied by Group Policy QoS rules are preserved in the packet header as sent onto the wire.",
                Tags = ["qos", "dscp", "nic", "offload", "preserve"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents NIC drivers from overwriting DSCP. May interact with NIC vendor QoS capabilities (e.g., Intel Smart QoS). Test on representative hardware.",
                ApplyOps = [RegOp.SetDword(QosKey, "DoNotUseNla", 0)],
                RemoveOps = [RegOp.DeleteValue(QosKey, "DoNotUseNla")],
                DetectOps = [RegOp.CheckDword(QosKey, "DoNotUseNla", 0)],
            },
            new TweakDef
            {
                Id = "dsqos-enable-rsvp-admission-control",
                Label = "DiffServ QoS: Enable RSVP Admission Control Signaling",
                Category = "DiffServ QoS Policy",
                Description =
                    "Sets AdmissionControl=1 in QoS/Psched policy. Enables RSVP-based admission control for QoS-reserving applications. When an application calls QOSAddSocketToFlow requesting a guaranteed bandwidth reservation, the packet scheduler uses RSVP PATH messages to signal bandwidth requirements to network routers. Admission control with RSVP ensures that QoS resources are not over-subscribed: if the network cannot accommodate a new reservation, the application's request is denied rather than silently over-committing.",
                Tags = ["qos", "rsvp", "admission-control", "bandwidth", "enterprise"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "RSVP requires RSVP-capable routers in the network path. Legacy enterprise routers support RSVP; modern MPLS networks use DiffServ instead.",
                ApplyOps = [RegOp.SetDword(PsvKey, "AdmissionControl", 1)],
                RemoveOps = [RegOp.DeleteValue(PsvKey, "AdmissionControl")],
                DetectOps = [RegOp.CheckDword(PsvKey, "AdmissionControl", 1)],
            },
            new TweakDef
            {
                Id = "dsqos-prioritize-system-service-traffic",
                Label = "DiffServ QoS: Prioritize Windows System Services Network Traffic",
                Category = "DiffServ QoS Policy",
                Description =
                    "Sets SystemTrafficPriority=1 in QoS policy. Configures the Windows QoS Packet Scheduler to assign elevated priority to traffic from critical Windows system services including Active Directory domain controller replication, LDAP queries, DNS, and Kerberos authentication traffic. On busy enterprise networks, AD replication and authentication traffic competing with user data can cause Kerberos ticket timeouts, logon failures, and Group Policy application delays. System traffic prioritization prevents these transient disruptions.",
                Tags = ["qos", "system-traffic", "active-directory", "ldap", "kerberos"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Prioritizes system/AD traffic over general user traffic. On saturated uplinks, user application bandwidth may be slightly reduced.",
                ApplyOps = [RegOp.SetDword(QosKey, "SystemTrafficPriority", 1)],
                RemoveOps = [RegOp.DeleteValue(QosKey, "SystemTrafficPriority")],
                DetectOps = [RegOp.CheckDword(QosKey, "SystemTrafficPriority", 1)],
            },
        ];
}
