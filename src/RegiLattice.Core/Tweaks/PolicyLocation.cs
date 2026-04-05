namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// RegiLattice.Core — Tweaks/PolicyLocation.cs
// Windows Maps application and Location & Sensors platform Group Policy tweaks.
// Category: "Privacy"
// Sprints 667–668 (v6.11.0)

internal static class PolicyLocation
{
    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            .. _MapsPolicy.Data,
            .. _LocationSensorsPolicy.Data,
        ];

    // ── Sprint 667 — Windows Maps Policy ──────────────────────────────────────
    private static class _MapsPolicy
    {
        private const string MapsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Maps";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "priv-maps-disable-location",
                    Label = "Disable Location in Maps Application",
                    Category = "Privacy",
                    Description =
                        "Prevents the Windows Maps app from accessing location services via Group Policy. The app can still display maps but will not use device location for routing or nearby search. Stronger than the per-user setting. Default: location enabled. Recommended: disabled for privacy.",
                    Tags = ["maps", "location", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ApplyOps = [RegOp.SetDword(MapsKey, "DisableLocationBasedMaps", 1)],
                    RemoveOps = [RegOp.DeleteValue(MapsKey, "DisableLocationBasedMaps")],
                    DetectOps = [RegOp.CheckDword(MapsKey, "DisableLocationBasedMaps", 1)],
                },
                new TweakDef
                {
                    Id = "priv-maps-disable-traffic",
                    Label = "Disable Live Traffic Data in Maps",
                    Category = "Privacy",
                    Description =
                        "Prevents the Maps app from downloading and displaying live traffic data. Eliminates background network calls used for route prediction and traffic inference. Default: traffic enabled. Recommended: disabled.",
                    Tags = ["maps", "traffic", "network", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ApplyOps = [RegOp.SetDword(MapsKey, "TrafficEnabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(MapsKey, "TrafficEnabled")],
                    DetectOps = [RegOp.CheckDword(MapsKey, "TrafficEnabled", 0)],
                },
                new TweakDef
                {
                    Id = "priv-maps-disable-auto-update",
                    Label = "Disable Automatic Offline Map Downloads",
                    Category = "Privacy",
                    Description =
                        "Stops Windows from automatically downloading and updating offline map tile data in the background. Eliminates background network traffic for map tile refreshes. Default: auto-updates enabled. Recommended: disabled.",
                    Tags = ["maps", "offline", "update", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ApplyOps = [RegOp.SetDword(MapsKey, "AutoUpdatesEnabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(MapsKey, "AutoUpdatesEnabled")],
                    DetectOps = [RegOp.CheckDword(MapsKey, "AutoUpdatesEnabled", 0)],
                },
                new TweakDef
                {
                    Id = "priv-maps-block-network-access",
                    Label = "Block Maps App Network Access for Tile Downloads",
                    Category = "Privacy",
                    Description =
                        "Blocks the Maps app from accessing the network to download map tile data. Companion to the auto-update disable policy; together they fully prevent background map network traffic. Default: network access allowed. Recommended: blocked.",
                    Tags = ["maps", "network", "offline", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ApplyOps = [RegOp.SetDword(MapsKey, "AllowNetworkAccess", 0)],
                    RemoveOps = [RegOp.DeleteValue(MapsKey, "AllowNetworkAccess")],
                    DetectOps = [RegOp.CheckDword(MapsKey, "AllowNetworkAccess", 0)],
                },
                new TweakDef
                {
                    Id = "priv-maps-disable-geofence-query",
                    Label = "Disable Geo-Fence Address Queries in Maps",
                    Category = "Privacy",
                    Description =
                        "Prevents the Maps app from making geo-fence address query requests to Microsoft servers. Geo-fence queries allow the service to track device entry and exit from geographic regions. Default: queries allowed. Recommended: disabled.",
                    Tags = ["maps", "geofence", "location", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ApplyOps = [RegOp.SetDword(MapsKey, "DisableGeoFenceAddressQuery", 1)],
                    RemoveOps = [RegOp.DeleteValue(MapsKey, "DisableGeoFenceAddressQuery")],
                    DetectOps = [RegOp.CheckDword(MapsKey, "DisableGeoFenceAddressQuery", 1)],
                },
            ];
    }

    // ── Sprint 668 — Location & Sensors Platform Policy ───────────────────────
    private static class _LocationSensorsPolicy
    {
        private const string LocKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LocationAndSensors";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "priv-loc-disable-location",
                    Label = "Disable Windows Location Platform (Policy)",
                    Category = "Privacy",
                    Description =
                        "Turns off the Windows Location platform via Group Policy. Prevents all apps and system components from obtaining device location. Stronger than individual per-app settings because this is a machine-wide policy that cannot be overridden by user settings. Default: location enabled. Recommended: disabled for privacy.",
                    Tags = ["location", "gps", "privacy", "policy", "sensors"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ApplyOps = [RegOp.SetDword(LocKey, "DisableLocation", 1)],
                    RemoveOps = [RegOp.DeleteValue(LocKey, "DisableLocation")],
                    DetectOps = [RegOp.CheckDword(LocKey, "DisableLocation", 1)],
                },
                new TweakDef
                {
                    Id = "priv-loc-disable-scripting",
                    Label = "Disable Scripted Access to Windows Location Provider",
                    Category = "Privacy",
                    Description =
                        "Prevents scripts (JavaScript, VBScript, WScript) from accessing the Windows Location Provider API. Blocks browser-based and embedded-application scripted location queries without affecting native app location access. Default: scripting allowed. Recommended: disabled.",
                    Tags = ["location", "scripting", "privacy", "api", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ApplyOps = [RegOp.SetDword(LocKey, "DisableLocationScripting", 1)],
                    RemoveOps = [RegOp.DeleteValue(LocKey, "DisableLocationScripting")],
                    DetectOps = [RegOp.CheckDword(LocKey, "DisableLocationScripting", 1)],
                },
                new TweakDef
                {
                    Id = "priv-loc-disable-win-location-provider",
                    Label = "Disable Windows Location Provider",
                    Category = "Privacy",
                    Description =
                        "Disables the Windows Location Provider component, which derives location from Wi-Fi triangulation, IP geolocation, and cell tower data (on mobile hardware). Prevents passive location tracking without GPS. Default: enabled. Recommended: disabled for privacy.",
                    Tags = ["location", "wifi", "triangulation", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ApplyOps = [RegOp.SetDword(LocKey, "DisableWindowsLocationProvider", 1)],
                    RemoveOps = [RegOp.DeleteValue(LocKey, "DisableWindowsLocationProvider")],
                    DetectOps = [RegOp.CheckDword(LocKey, "DisableWindowsLocationProvider", 1)],
                },
                new TweakDef
                {
                    Id = "priv-loc-disable-sensors",
                    Label = "Disable Windows Sensors Platform",
                    Category = "Privacy",
                    Description =
                        "Disables the Windows Sensors platform, which provides apps access to accelerometers, ambient light sensors, gyrometers, and other hardware sensors. Prevents passive motion and environment inference by apps. Default: enabled. Recommended: disabled on fixed workstations.",
                    Tags = ["sensors", "accelerometer", "ambient-light", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ApplyOps = [RegOp.SetDword(LocKey, "DisableSensors", 1)],
                    RemoveOps = [RegOp.DeleteValue(LocKey, "DisableSensors")],
                    DetectOps = [RegOp.CheckDword(LocKey, "DisableSensors", 1)],
                },
                new TweakDef
                {
                    Id = "priv-loc-disable-geolocation-api",
                    Label = "Disable Geolocation API for Applications",
                    Category = "Privacy",
                    Description =
                        "Disables the system Geolocation API used by UWP and Win32 apps to request precise geographic coordinates. Applied at the policy layer, blocking all apps from obtaining location data regardless of individual user grant settings. Default: API accessible. Recommended: disabled.",
                    Tags = ["geolocation", "api", "location", "privacy", "policy", "uwp"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ApplyOps = [RegOp.SetDword(LocKey, "DisableGeolocation", 1)],
                    RemoveOps = [RegOp.DeleteValue(LocKey, "DisableGeolocation")],
                    DetectOps = [RegOp.CheckDword(LocKey, "DisableGeolocation", 1)],
                },
            ];
    }
}
