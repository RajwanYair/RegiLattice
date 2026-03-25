// RegiLattice.Core — Tweaks/DataSensePolicy.cs
// Sprint 288: Data Sense Policy tweaks (10 tweaks)
// Category: "Data Sense Policy" | Slug: dtsense
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataSense

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class DataSensePolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataSense";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "dtsense-disable-traffic-shaper",
            Label = "Disable Data Sense Traffic Shaper",
            Category = "Data Sense Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Data Sense traffic shaping throttles background network activity when the device approaches data usage limits on metered connections. Enterprise workstations typically operate on unmetered corporate networks where traffic shaping provides no benefit. Disabling the traffic shaper ensures consistent network throughput for all applications regardless of metered connection state. Background data transfers including Windows Update and application synchronization will proceed at full speed. This setting prevents unexpected performance degradation on networks incorrectly classified as metered. Administrators managing corporate networks should ensure connection profiles are correctly configured as unmetered.",
            Tags = ["data-sense", "network", "bandwidth", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "TrafficShaperEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "TrafficShaperEnabled")],
            DetectOps = [RegOp.CheckDword(Key, "TrafficShaperEnabled", 0)],
        },
        new TweakDef
        {
            Id = "dtsense-restrict-background-data",
            Label = "Restrict Background Data Usage",
            Category = "Data Sense Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Background data usage restriction limits which applications can transfer data while running in the background on metered connections. Restricting background data usage ensures that critical foreground applications receive priority over background synchronization tasks. This policy is particularly valuable on mobile workstations that connect to metered LTE or satellite networks. Applications performing background telemetry, update downloads, and cloud synchronization are subject to this restriction. Domain-joined machines on corporate networks may still benefit from this setting as a defense against uncontrolled background transfers. The restriction applies to applications respecting the metered connection API and can be supplemented with firewall rules.",
            Tags = ["data-sense", "background", "network", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "BackgroundDataUsageRestricted", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BackgroundDataUsageRestricted")],
            DetectOps = [RegOp.CheckDword(Key, "BackgroundDataUsageRestricted", 1)],
        },
        new TweakDef
        {
            Id = "dtsense-disable-usage-tracking",
            Label = "Disable Data Usage Tracking",
            Category = "Data Sense Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Data usage tracking monitors per-application and per-connection data consumption and stores this information in the Data Sense repository. Disabling tracking prevents Windows from maintaining a local data usage history for metered connections. Organizations with dedicated network monitoring solutions have no need for Windows to duplicate this tracking locally. The tracking data can be exposed to Microsoft through telemetry channels, representing a minor privacy consideration. Disabling this feature reduces the I/O overhead associated with maintaining usage logs per network session. Network monitoring requirements are better served by enterprise-grade infrastructure solutions rather than client-side tracking.",
            Tags = ["data-sense", "tracking", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DataUsageTrackingEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "DataUsageTrackingEnabled")],
            DetectOps = [RegOp.CheckDword(Key, "DataUsageTrackingEnabled", 0)],
        },
        new TweakDef
        {
            Id = "dtsense-disable-hotspot-throttle",
            Label = "Disable Mobile Hotspot Throttle",
            Category = "Data Sense Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "The mobile hotspot throttle reduces network buffer sizes and download rates when the device is connected to a mobile hotspot to conserve cellular data. Corporate devices connecting to approved mobile hotspots for remote work may experience degraded performance due to unnecessary throttling. Disabling hotspot throttle allows full use of available bandwidth when connecting through a mobile hotspot. This is particularly important for developers, analysts, and field personnel who rely on mobile connectivity for data-intensive work. The policy does not affect cellular data billing because billing is managed exclusively by the carrier. Network administrators should combine this setting with appropriate mobile device management policies.",
            Tags = ["data-sense", "hotspot", "network", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "MobileHotspotThrottleEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "MobileHotspotThrottleEnabled")],
            DetectOps = [RegOp.CheckDword(Key, "MobileHotspotThrottleEnabled", 0)],
        },
        new TweakDef
        {
            Id = "dtsense-disable-telemetry",
            Label = "Disable Data Sense Telemetry",
            Category = "Data Sense Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Data Sense telemetry transmits information about network usage patterns and connection types to Microsoft for product improvement purposes. This data can include information about which applications consume the most bandwidth on metered connections. Disabling Data Sense telemetry prevents this usage telemetry from leaving the enterprise boundary. Regulated industries handling sensitive data have obligations to minimize external telemetry data flows. The telemetry collection does not affect any Data Sense functionality or local network management capabilities. Administrators can achieve equivalent insights through internal network monitoring infrastructure.",
            Tags = ["data-sense", "telemetry", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DataSenseTelemetry", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "DataSenseTelemetry")],
            DetectOps = [RegOp.CheckDword(Key, "DataSenseTelemetry", 0)],
        },
        new TweakDef
        {
            Id = "dtsense-disable-auto-data-saving",
            Label = "Disable Auto Data Saving Mode",
            Category = "Data Sense Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Automatic data saving mode activates when the device approaches its configured data limit, restricting background transfers and reducing image quality. Enterprise devices operating within managed network environments typically do not require automatic data saving behavior. Disabling automatic data saving ensures consistent application performance that is not disrupted by data limit thresholds. Background services critical to business operations such as backup agents and security scanners continue running unrestricted. This setting provides more predictable behavior for services that require reliable network access. Administrators managing enterprise networks should implement quota policies at the network infrastructure level instead.",
            Tags = ["data-sense", "data-saving", "network", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableAutoDataSaving", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoDataSaving")],
            DetectOps = [RegOp.CheckDword(Key, "DisableAutoDataSaving", 1)],
        },
        new TweakDef
        {
            Id = "dtsense-zero-metered-threshold",
            Label = "Set Metered Connection Threshold to Zero",
            Category = "Data Sense Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Description =
                "The metered connection threshold determines the data usage percentage at which Data Sense begins applying stricter background transfer restrictions. Setting this threshold to zero effectively disables threshold-triggered restrictions without fully disabling the Data Sense feature. This prevents Data Sense from automatically engaging aggressive restrictions at any data usage level. Enterprise environments with unmetered connections benefit from ensuring no threshold-based restrictions are applied. Background services and synchronization tasks remain unaffected by threshold triggers. This setting complements other Data Sense policy configurations for comprehensive network behavior control.",
            Tags = ["data-sense", "metered", "network", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "MeteredConnectionThreshold", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "MeteredConnectionThreshold")],
            DetectOps = [RegOp.CheckDword(Key, "MeteredConnectionThreshold", 0)],
        },
        new TweakDef
        {
            Id = "dtsense-disable-connected-standby-data",
            Label = "Disable Connected Standby Data Usage",
            Category = "Data Sense Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Connected standby allows modern Windows devices to maintain network connectivity during sleep states to receive notifications and sync data. Data transferred during connected standby can consume metered data allowances without user awareness. Disabling connected standby data usage prevents background network activity while the device screen is off. Enterprise users on metered connections benefit from predictable data consumption that only occurs during active use sessions. Battery-powered devices additionally benefit from reduced power drain caused by background network activity during sleep. Applications requiring real-time push notifications should be evaluated for compatibility with this setting before deployment.",
            Tags = ["data-sense", "standby", "network", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableConnectedStandbyDataUsage", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableConnectedStandbyDataUsage")],
            DetectOps = [RegOp.CheckDword(Key, "DisableConnectedStandbyDataUsage", 1)],
        },
        new TweakDef
        {
            Id = "dtsense-disable-notifications",
            Label = "Disable Data Sense Notifications",
            Category = "Data Sense Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Description =
                "Data Sense generates notifications alerting users when they approach or exceed configured data usage thresholds on metered connections. These notifications are not relevant for enterprise devices operating on unmetered corporate networks. Disabling Data Sense notifications reduces notification noise and prevents user confusion about data limits on managed networks. The notifications cannot trigger any automatic remediation actions and are purely informational. Enterprise environments with dedicated bandwidth management solutions have superior alerting mechanisms. Suppressing these notifications has no effect on network connectivity or data transfer operations.",
            Tags = ["data-sense", "notifications", "ui", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableDataSenseNotifications", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableDataSenseNotifications")],
            DetectOps = [RegOp.CheckDword(Key, "DisableDataSenseNotifications", 1)],
        },
        new TweakDef
        {
            Id = "dtsense-disable-feature",
            Label = "Disable Data Sense Feature",
            Category = "Data Sense Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Data Sense is a Windows feature that monitors and manages network data usage to help users avoid exceeding metered connection limits. Enterprise devices on corporate networks typically operate outside the scope for which Data Sense was designed. Disabling the Data Sense feature removes the data usage monitoring overlay and its associated background services. Network resource management in enterprise environments is handled at the infrastructure level through switches, routers, and DLP solutions. Disabling the feature reduces the attack surface by removing a component that interacts with network traffic metadata. All network connectivity and data transfer capabilities remain fully functional when Data Sense is disabled.",
            Tags = ["data-sense", "feature", "network", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableDataSenseFeature", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableDataSenseFeature")],
            DetectOps = [RegOp.CheckDword(Key, "DisableDataSenseFeature", 1)],
        },
    ];
}
