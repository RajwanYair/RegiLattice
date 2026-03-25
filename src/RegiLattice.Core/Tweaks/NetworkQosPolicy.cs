// RegiLattice.Core — Tweaks/NetworkQosPolicy.cs
// Sprint 298: Network QoS Policy tweaks (10 tweaks)
// Category: "Network QoS Policy" | Slug: nqos
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\QoS

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class NetworkQosPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\QoS";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "nqos-disable-reservation",
            Label = "Disable Network QoS Bandwidth Reservation",
            Category = "Network QoS Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Windows QoS allows applications to reserve a portion of available network bandwidth for guaranteed delivery of real-time traffic like video and voice. The QoS bandwidth reservation mechanism historically allowed applications to reserve up to 20 percent of available link bandwidth. Disabling QoS bandwidth reservation ensures all available network bandwidth remains available for use by all applications equally. In networks with sufficient bandwidth this reservation provides no practical benefit and may limit throughput for bulk data transfers. Enterprise network QoS should be enforced at the network infrastructure level using DSCP markings rather than per-host reservations. Disabling host-side QoS reservation has no impact on network-enforced traffic prioritization.",
            Tags = ["network", "qos", "bandwidth", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableBestEffortReservation", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableBestEffortReservation")],
            DetectOps = [RegOp.CheckDword(Key, "DisableBestEffortReservation", 1)],
        },
        new TweakDef
        {
            Id = "nqos-disable-dscp-marking",
            Label = "Disable DSCP Marking Override",
            Category = "Network QoS Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "DSCP marking allows Windows to tag outbound network packets with Differentiated Services Code Point values for network infrastructure-based traffic prioritization. Windows can override application-requested DSCP values through Group Policy QoS rules. Disabling DSCP marking override prevents Windows from altering the DSCP values set by applications and network infrastructure devices. In properly configured enterprise networks, DSCP values should be set by trusted network devices rather than clienthosts. Host-based DSCP marking can conflict with network-enforced QoS policies deployed across the enterprise. Disabling this override ensures that network infrastructure QoS policies take precedence over host-side marking attempts.",
            Tags = ["network", "qos", "dscp", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableDscpMarkingOverride", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableDscpMarkingOverride")],
            DetectOps = [RegOp.CheckDword(Key, "DisableDscpMarkingOverride", 1)],
        },
        new TweakDef
        {
            Id = "nqos-disable-throttling",
            Label = "Disable Network QoS Throttling",
            Category = "Network QoS Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Windows QoS throttling limits outbound network traffic rates for applications that have been classified under specific QoS policies. Disabling QoS throttling removes artificially imposed rate limits on network applications that would otherwise be constrained. QoS throttling can inadvertently limit backup software, large file transfers, and other high-throughput legitimate workloads. Enterprise environments relying on network-level traffic shaping should not also apply host-side throttling that could conflict with infrastructure QoS. Removing throttling on trusted enterprise endpoints ensures full link utilization for bulk transfer operations. Network-level QoS enforced by switching and routing infrastructure remains unaffected by this setting.",
            Tags = ["network", "qos", "throttling", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableThrottling", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableThrottling")],
            DetectOps = [RegOp.CheckDword(Key, "DisableThrottling", 1)],
        },
        new TweakDef
        {
            Id = "nqos-disable-policy-application",
            Label = "Disable User-Level QoS Policy Application",
            Category = "Network QoS Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Windows QoS policies can be applied at both the computer level and user level, with user-level policies applied when specific users log on. Disabling user-level QoS policy application prevents per-user QoS configurations from being applied and enforced. User-level QoS policies are less predictable in shared environments because they vary based on which user is logged on. Computer-level QoS policies applied by Group Policy at the machine scope are unaffected and continue to be enforced. Consistent network behavior in shared computing environments is better achieved through machine-level QoS policies. Disabling user-level policy application simplifies QoS administration by eliminating user-specific network configuration variability.",
            Tags = ["network", "qos", "users", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableUserPolicyApplication", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableUserPolicyApplication")],
            DetectOps = [RegOp.CheckDword(Key, "DisableUserPolicyApplication", 1)],
        },
        new TweakDef
        {
            Id = "nqos-disable-pacer",
            Label = "Disable Network Packet Scheduler Pacer",
            Category = "Network QoS Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 3,
            Description =
                "The Windows packet scheduler pacer component enforces QoS policies by spacing out packet transmission to achieve desired traffic rates. Disabling the packet scheduler pacer removes the inter-packet scheduling enforcement for QoS flows. The pacer component adds CPU overhead and latency for every outbound network packet when QoS is in use. Enterprise endpoints not relying on Windows host-based QoS can benefit from removing this scheduling overhead. Removing the pacer reduces network stack latency and may improve throughput for applications sensitive to jitter and scheduling overhead. Network QoS enforcement at the infrastructure level through switch and router configurations is unaffected.",
            Tags = ["network", "qos", "pacer", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisablePacer", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisablePacer")],
            DetectOps = [RegOp.CheckDword(Key, "DisablePacer", 1)],
        },
        new TweakDef
        {
            Id = "nqos-disable-app-marking",
            Label = "Prevent Applications from Overriding QoS Settings",
            Category = "Network QoS Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Applications using QoS APIs can request specific DSCP markings and bandwidth priorities for their network traffic. Preventing applications from overriding QoS settings ensures that application-requested markings do not supersede Group Policy-defined QoS rules. Applications may abuse QoS APIs to mark all traffic with high priority values, degrading the effectiveness of traffic prioritization. Enterprise QoS policies should take strict precedence over application-level QoS requests to maintain consistent network behavior. Policy-locked QoS settings ensure the network infrastructure receives consistent DSCP markings regardless of application behavior. Critical real-time applications that require specific QoS treatment should be configured in Group Policy rather than self-assigning priorities.",
            Tags = ["network", "qos", "applications", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "PreventApplicationQosOverride", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "PreventApplicationQosOverride")],
            DetectOps = [RegOp.CheckDword(Key, "PreventApplicationQosOverride", 1)],
        },
        new TweakDef
        {
            Id = "nqos-disable-telemetry",
            Label = "Disable Network QoS Telemetry",
            Category = "Network QoS Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Description =
                "Network QoS telemetry collects and reports data about network quality of service events and policy application statistics. This telemetry helps Microsoft understand QoS usage patterns and improve network stack performance in future releases. Disabling QoS telemetry prevents network traffic classification and policy usage data from being reported externally. Network quality metrics represent sensitive operational data that may reveal enterprise network architecture details. Telemetry reporting from QoS components should be evaluated against data governance requirements before being permitted. All QoS policy enforcement and traffic prioritization functions continue to operate normally without telemetry.",
            Tags = ["network", "qos", "telemetry", "privacy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableQosTelemetry", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableQosTelemetry")],
            DetectOps = [RegOp.CheckDword(Key, "DisableQosTelemetry", 1)],
        },
        new TweakDef
        {
            Id = "nqos-disable-adaptive",
            Label = "Disable Adaptive QoS",
            Category = "Network QoS Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Adaptive QoS dynamically adjusts traffic classification and bandwidth allocation based on observed network conditions. Disabling adaptive QoS prevents automatic changes to traffic prioritization based on runtime network quality measurements. Adaptive QoS adjustments can cause unpredictable network behavior changes during production hours in enterprise environments. Static QoS configurations are preferred for enterprise deployments where consistent and auditable network behavior is required. Dynamic priority adjustments may interfere with time-sensitive applications relying on predictable network response characteristics. Disabling adaptive QoS ensures QoS policy settings remain stable and consistent with the Group Policy-defined configuration.",
            Tags = ["network", "qos", "adaptive", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableAdaptiveQos", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableAdaptiveQos")],
            DetectOps = [RegOp.CheckDword(Key, "DisableAdaptiveQos", 1)],
        },
        new TweakDef
        {
            Id = "nqos-disable-conformance",
            Label = "Disable QoS Traffic Conformance",
            Category = "Network QoS Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "QoS traffic conformance controls track whether applications are transmitting within their declared QoS profiles and can take action against non-conforming flows. Disabling traffic conformance removes enforcement of declared traffic profiles and allows flows to transmit outside their QoS specifications. Conformance enforcement adds complexity and potential false positives for legitimate burst traffic patterns in enterprise applications. Enterprise applications with variable traffic patterns may trigger conformance violations unnecessarily. Traffic shaping and rate enforcement should be handled at the network infrastructure level where more accurate visibility is available. Disabling conformance reduces QoS subsystem CPU overhead without affecting traffic marking behavior.",
            Tags = ["network", "qos", "conformance", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableTrafficConformance", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableTrafficConformance")],
            DetectOps = [RegOp.CheckDword(Key, "DisableTrafficConformance", 1)],
        },
        new TweakDef
        {
            Id = "nqos-disable-flow-inspection",
            Label = "Disable QoS Flow Inspection",
            Category = "Network QoS Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "QoS flow inspection examines network traffic characteristics to classify flows and apply appropriate QoS policies. Disabling flow inspection prevents the QoS subsystem from performing deep packet analysis for traffic classification. Flow inspection adds CPU and memory overhead proportional to the volume of classified network flows. Enterprise environments that do not rely on QoS flow-based classification for policy application can safely disable this component. Static QoS policy rules based on application name or destination port are unaffected and continue to operate without flow inspection. Disabling flow inspection reduces network stack processing overhead, particularly on high-throughput workloads.",
            Tags = ["network", "qos", "inspection", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableFlowInspection", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableFlowInspection")],
            DetectOps = [RegOp.CheckDword(Key, "DisableFlowInspection", 1)],
        },
    ];
}
