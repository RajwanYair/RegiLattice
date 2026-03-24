// RegiLattice.Core — Tweaks/LocationSensorsPolicy.cs
// Location services and hardware sensors machine-scope GPO controls — Sprint 201.
// Governs device location data access, sensor drivers, Wi-Fi geo-location, and privacy controls.
// Category: "Location & Sensors Policy" | Slug: locsns
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LocationAndSensors

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class LocationSensorsPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LocationAndSensors";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "locsns-disable-location",
                Label = "Disable All Location Services",
                Category = "Location & Sensors Policy",
                Description =
                    "Disables the Windows location platform, preventing all applications and system components from accessing the device's geographic location. Default: enabled. Recommended: 1 (disabled) on workstations where geo-location is unnecessary.",
                Tags = ["location", "privacy", "sensors", "gps", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "No application can access location; maps, weather, and location-aware apps lose positioning.",
                ApplyOps = [RegOp.SetDword(Key, "DisableLocation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableLocation")],
                DetectOps = [RegOp.CheckDword(Key, "DisableLocation", 1)],
            },
            new TweakDef
            {
                Id = "locsns-disable-scripted-location",
                Label = "Disable Scripted Location Access",
                Category = "Location & Sensors Policy",
                Description =
                    "Prevents scripts (JScript, VBScript, PowerShell) and web content from accessing the Windows location platform. Closes browser-script and WSH location enumeration vectors. Default: enabled. Recommended: 1 (disabled).",
                Tags = ["location", "scripting", "privacy", "web", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Web and script-based location lookups are blocked; physical GPS and platform APIs are unaffected.",
                ApplyOps = [RegOp.SetDword(Key, "DisableLocationScripting", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableLocationScripting")],
                DetectOps = [RegOp.CheckDword(Key, "DisableLocationScripting", 1)],
            },
            new TweakDef
            {
                Id = "locsns-disable-sensors",
                Label = "Disable Hardware Sensor Driver Framework",
                Category = "Location & Sensors Policy",
                Description =
                    "Disables the Windows sensor platform, preventing ambient light, accelerometer, gyroscope, and other sensor drivers from reporting data to applications. Default: enabled. Recommended: 1 (disabled) on fixed workstations.",
                Tags = ["sensors", "hardware", "accelerometer", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "All hardware sensors (light, motion, barometer) are unavailable to applications.",
                ApplyOps = [RegOp.SetDword(Key, "DisableSensors", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableSensors")],
                DetectOps = [RegOp.CheckDword(Key, "DisableSensors", 1)],
            },
            new TweakDef
            {
                Id = "locsns-disable-windowed-location",
                Label = "Disable Windowed Location Mode",
                Category = "Location & Sensors Policy",
                Description =
                    "Disables the windowed (per-session) location mode where each window can independently request location access. Centralises location control. Default: enabled. Recommended: 1 (disabled).",
                Tags = ["location", "windowed", "session", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Windows cannot individually request location access; policy applies system-wide.",
                ApplyOps = [RegOp.SetDword(Key, "DisableWindowedLocation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableWindowedLocation")],
                DetectOps = [RegOp.CheckDword(Key, "DisableWindowedLocation", 1)],
            },
            new TweakDef
            {
                Id = "locsns-disable-wifi-scan",
                Label = "Disable Wi-Fi Scan for Geo-Location",
                Category = "Location & Sensors Policy",
                Description =
                    "Prevents the Windows location platform from using Wi-Fi access point scanning to determine the device's location. Eliminates WPS-based location leakage on enterprise Wi-Fi. Default: enabled. Recommended: 1 (disabled).",
                Tags = ["location", "wifi", "geolocation", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Location accuracy may decrease if GPS is unavailable; Wi-Fi AP MAC data is not used for positioning.",
                ApplyOps = [RegOp.SetDword(Key, "DisableWifiScanForLocation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableWifiScanForLocation")],
                DetectOps = [RegOp.CheckDword(Key, "DisableWifiScanForLocation", 1)],
            },
            new TweakDef
            {
                Id = "locsns-disable-fused-provider",
                Label = "Disable Fused Location Provider",
                Category = "Location & Sensors Policy",
                Description =
                    "Disables the Windows Fused Location Provider that combines GPS, cellular, Wi-Fi, and sensor data for high-accuracy positioning. Reduces background data aggregation. Default: enabled. Recommended: 1 (disabled) on desktop/server systems.",
                Tags = ["location", "fused", "gps", "accuracy", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Location accuracy is significantly reduced or unavailable when all sub-providers are also disabled.",
                ApplyOps = [RegOp.SetDword(Key, "DisableFusedLocationProvider", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableFusedLocationProvider")],
                DetectOps = [RegOp.CheckDword(Key, "DisableFusedLocationProvider", 1)],
            },
            new TweakDef
            {
                Id = "locsns-clear-location-history",
                Label = "Disable Location History Logging",
                Category = "Location & Sensors Policy",
                Description =
                    "Prevents Windows from storing a history of the device's geographic location. Location data is used for each query only and not retained locally. Default: enabled. Recommended: 1 (disabled) for regulatory compliance.",
                Tags = ["location", "history", "privacy", "data-retention", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "No location data is stored on-disk; each app request resolves live position only.",
                ApplyOps = [RegOp.SetDword(Key, "DisableLocationHistory", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableLocationHistory")],
                DetectOps = [RegOp.CheckDword(Key, "DisableLocationHistory", 1)],
            },
            new TweakDef
            {
                Id = "locsns-disable-cellular-location",
                Label = "Disable Cellular Data for Location",
                Category = "Location & Sensors Policy",
                Description =
                    "Prevents the Windows location service from using cellular tower triangulation to determine device position. Applicable on devices with mobile data or SIM cards. Default: enabled. Recommended: 1 (disabled).",
                Tags = ["location", "cellular", "triangulation", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Cellular-based location disabled; GPS or Wi-Fi are still available if also enabled.",
                ApplyOps = [RegOp.SetDword(Key, "DisableCellularDataForLocation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCellularDataForLocation")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCellularDataForLocation", 1)],
            },
            new TweakDef
            {
                Id = "locsns-geo-smoothing-disable",
                Label = "Disable Location Geo-Smoothing",
                Category = "Location & Sensors Policy",
                Description =
                    "Disables the geo-smoothing algorithm that averages location readings over time. Reduces background sensor polling frequency and associated battery/resource usage. Default: enabled. Recommended: 0 (off) on fixed machines.",
                Tags = ["location", "smoothing", "gps", "performance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Position updates are not averaged; minor location jitter may appear if GPS is active.",
                ApplyOps = [RegOp.SetDword(Key, "GeoSmoothingInterval", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "GeoSmoothingInterval")],
                DetectOps = [RegOp.CheckDword(Key, "GeoSmoothingInterval", 0)],
            },
            new TweakDef
            {
                Id = "locsns-deny-app-location-access",
                Label = "Deny All App Access to Location",
                Category = "Location & Sensors Policy",
                Description =
                    "Forces a system-wide policy that denies all apps access to the location platform, overriding per-app user consent. More restrictive than disabling the service; denies even system components. Default: not set. Recommended: 1.",
                Tags = ["location", "app-access", "privacy", "lockdown", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote = "All apps — including trusted Windows components — are denied location access. May affect time-zone syncing.",
                ApplyOps = [RegOp.SetDword(Key, "AllowAppsToAccessLocation", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowAppsToAccessLocation")],
                DetectOps = [RegOp.CheckDword(Key, "AllowAppsToAccessLocation", 0)],
            },
        ];
}
