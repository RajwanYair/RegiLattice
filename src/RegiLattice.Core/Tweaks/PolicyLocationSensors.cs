namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// ─────────────────────────────────────────────────────────────────────────────
// Sprint 644 — PolicyLocationSensors (Location & Sensors Group Policy)

[TweakModule]
internal static class PolicyLocationSensors
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LocationAndSensors";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "locsvc-disable-location-awareness",
            Label = "Disable Network Location Awareness",
            Category = "Privacy — Sensor",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents Windows Network Location Awareness (NLA) from uploading network topology data used to improve geolocation services. Stops SSID/BSSID mapping data from being sent to Microsoft.",
            Tags = ["location", "network", "nla", "privacy", "policy", "telemetry"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Network topology data not contributed to Microsoft location database.",
            ApplyOps = [RegOp.SetDword(Key, "DisableNetworkLocationAwareness", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableNetworkLocationAwareness")],
            DetectOps = [RegOp.CheckDword(Key, "DisableNetworkLocationAwareness", 1)],
        },
        new TweakDef
        {
            Id = "locsvc-disable-location-telemetry",
            Label = "Disable Location Service Telemetry Upload",
            Category = "Privacy — Sensor",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks Windows from uploading location service usage telemetry to Microsoft. Prevents location access frequency, accuracy levels, and app-level location events from being reported.",
            Tags = ["location", "telemetry", "privacy", "policy", "data-collection"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Location usage telemetry not uploaded; no location event data reaches Microsoft.",
            ApplyOps = [RegOp.SetDword(Key, "DisableLocationTelemetry", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableLocationTelemetry")],
            DetectOps = [RegOp.CheckDword(Key, "DisableLocationTelemetry", 1)],
        },
        new TweakDef
        {
            Id = "locsvc-disable-location-history",
            Label = "Disable Location History Storage",
            Category = "Privacy — Sensor",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents Windows from storing location history on the device. Location history is used by apps like Maps to show frequently visited places; disabling it keeps no local location log.",
            Tags = ["location", "history", "privacy", "policy", "data-storage"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "No location history stored on device; visited places not recorded.",
            ApplyOps = [RegOp.SetDword(Key, "DisableLocationHistory", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableLocationHistory")],
            DetectOps = [RegOp.CheckDword(Key, "DisableLocationHistory", 1)],
        },
        new TweakDef
        {
            Id = "locsvc-disable-geofencing",
            Label = "Disable Geofencing API",
            Category = "Privacy — Sensor",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks the Windows Geofencing API that allows applications to define geographic boundaries and trigger events when the device enters or exits them. Prevents background location monitoring by geofencing-aware apps.",
            Tags = ["location", "geofencing", "background", "privacy", "policy"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Geofencing triggers disabled; apps cannot monitor if device enters/exits geographic areas.",
            ApplyOps = [RegOp.SetDword(Key, "DisableGeofencing", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableGeofencing")],
            DetectOps = [RegOp.CheckDword(Key, "DisableGeofencing", 1)],
        },
        new TweakDef
        {
            Id = "locsvc-disable-sensor-data-service",
            Label = "Disable Sensor Data Service",
            Category = "Privacy — Sensor",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Windows Sensor Data Service that aggregates sensor readings from multiple physical sensors and exposes them to applications. Stopping this service prevents all sensor-based fingerprinting.",
            Tags = ["sensors", "service", "privacy", "policy", "fingerprinting"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Unified sensor aggregation service stopped; no sensor data exposed to apps.",
            ApplyOps = [RegOp.SetDword(Key, "DisableSensorDataService", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableSensorDataService")],
            DetectOps = [RegOp.CheckDword(Key, "DisableSensorDataService", 1)],
        },
        new TweakDef
        {
            Id = "locsvc-disable-light-sensor",
            Label = "Disable Ambient Light Sensor",
            Category = "Privacy — Sensor",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the ambient light sensor from being accessible to applications via the Windows Sensor API. Prevents light-level data from being used for screen brightness fingerprinting or environment profiling.",
            Tags = ["sensors", "ambient-light", "privacy", "policy", "fingerprinting"],
            RegistryKeys = [Key],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Ambient light sensor data not exposed to apps; environment-based profiling blocked.",
            ApplyOps = [RegOp.SetDword(Key, "DisableAmbientLightSensor", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableAmbientLightSensor")],
            DetectOps = [RegOp.CheckDword(Key, "DisableAmbientLightSensor", 1)],
        },
    ];
}
