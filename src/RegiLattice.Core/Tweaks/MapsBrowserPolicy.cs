// RegiLattice.Core — Tweaks/MapsBrowserPolicy.cs
// Windows Maps app and location-based browsing GPO controls — Sprint 218.
// Controls offline maps downloads, auto-update, and location access.
// Category: "Maps & Browser Policy" | Slug: mapsbr
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\MapsBrowser

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class MapsBrowserPolicy
{
    private const string Key =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\MapsBrowser";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "mapsbr-disable-auto-download",
                Label = "Disable Automatic Offline Maps Download",
                Category = "Maps & Browser Policy",
                Description =
                    "Prevents Windows from automatically downloading offline map data updates in the background. Reduces unnecessary network traffic on metered connections and removes a low-value background data transfer. Default: auto-download enabled. Recommended: 1.",
                Tags = ["maps", "offline", "download", "background", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Offline map data is not downloaded automatically; maps remain available but may be outdated.",
                ApplyOps = [RegOp.SetDword(Key, "AutoDownloadAndUpdateMapData", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AutoDownloadAndUpdateMapData")],
                DetectOps = [RegOp.CheckDword(Key, "AutoDownloadAndUpdateMapData", 0)],
            },
            new TweakDef
            {
                Id = "mapsbr-disable-untriggered-network-traffic",
                Label = "Disable Maps App Untriggered Network Traffic",
                Category = "Maps & Browser Policy",
                Description =
                    "Stops the Maps application from initiating network requests that are not triggered by explicit user interaction (such as background tile prefetching or POI data sync). Reduces bandwidth consumption and privacy exposure. Default: background network traffic allowed. Recommended: 1.",
                Tags = ["maps", "network", "background", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Maps app stops all unsolicited background network requests; only user-initiated map loads use network.",
                ApplyOps = [RegOp.SetDword(Key, "AllowUntriggeredNetworkTrafficOnSettingsPage", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowUntriggeredNetworkTrafficOnSettingsPage")],
                DetectOps = [RegOp.CheckDword(Key, "AllowUntriggeredNetworkTrafficOnSettingsPage", 0)],
            },
            new TweakDef
            {
                Id = "mapsbr-disable-location-for-maps",
                Label = "Disable Location Access for Windows Maps",
                Category = "Maps & Browser Policy",
                Description =
                    "Blocks the Windows Maps application from using the device's current location (GPS, Wi-Fi triangulation, IP geolocation) to centre the map or suggest nearby places. Prevents continuous location sampling by the app. Default: location allowed. Recommended: 1 on privacy-hardened endpoints.",
                Tags = ["maps", "location", "gps", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Windows Maps cannot access device location; map starts at a default location, not the user's current position.",
                ApplyOps = [RegOp.SetDword(Key, "DisableLocationForMaps", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableLocationForMaps")],
                DetectOps = [RegOp.CheckDword(Key, "DisableLocationForMaps", 1)],
            },
            new TweakDef
            {
                Id = "mapsbr-block-map-traffic-data",
                Label = "Disable Real-Time Traffic Data in Maps",
                Category = "Maps & Browser Policy",
                Description =
                    "Prevents Windows Maps from fetching real-time traffic data (congestion, incidents, road closures) from Microsoft's mapping service. Reduces background network calls and location telemetry inferences. Default: traffic data enabled. Recommended: 1 on privacy-hardened endpoints.",
                Tags = ["maps", "traffic", "realtime", "network", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Maps app does not show live traffic data; routes are calculated without congestion awareness.",
                ApplyOps = [RegOp.SetDword(Key, "DisableTrafficData", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableTrafficData")],
                DetectOps = [RegOp.CheckDword(Key, "DisableTrafficData", 1)],
            },
            new TweakDef
            {
                Id = "mapsbr-disable-map-tile-storage",
                Label = "Disable Offline Map Tile Storage",
                Category = "Maps & Browser Policy",
                Description =
                    "Prevents Windows Maps from caching map tiles on local disk for offline use. Removes the map data footprint from managed devices where the maps feature is not used. Default: tiles cached locally. Recommended: 1 on space-constrained or managed endpoints where maps is unused.",
                Tags = ["maps", "tile", "cache", "storage", "disk", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Map tile cache is not maintained on disk; offline map access is unavailable.",
                ApplyOps = [RegOp.SetDword(Key, "DisableOfflineTileStorage", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableOfflineTileStorage")],
                DetectOps = [RegOp.CheckDword(Key, "DisableOfflineTileStorage", 1)],
            },
            new TweakDef
            {
                Id = "mapsbr-disable-bing-search-integration",
                Label = "Disable Bing Search Integration in Maps",
                Category = "Maps & Browser Policy",
                Description =
                    "Prevents Windows Maps from sending search queries to Bing when a user searches for a place, address, or business. Stops search terms from being transmitted to Microsoft's servers. Default: Bing integration enabled. Recommended: 1 on privacy-hardened endpoints.",
                Tags = ["maps", "bing", "search", "privacy", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Map searches do not query Bing; only locally cached/offline map data is searched.",
                ApplyOps = [RegOp.SetDword(Key, "DisableBingSearchIntegration", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableBingSearchIntegration")],
                DetectOps = [RegOp.CheckDword(Key, "DisableBingSearchIntegration", 1)],
            },
            new TweakDef
            {
                Id = "mapsbr-disable-route-sharing",
                Label = "Disable Route/Directions Sharing from Maps",
                Category = "Maps & Browser Policy",
                Description =
                    "Removes the 'Share' button and functionality from Windows Maps so users cannot share routes, locations, or directions via mail, SMS, or other apps. Prevents incidental location data leakage through sharing. Default: sharing enabled. Recommended: 1.",
                Tags = ["maps", "sharing", "route", "privacy", "dlp", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Share functionality is removed from Maps; routes and places cannot be shared externally.",
                ApplyOps = [RegOp.SetDword(Key, "DisableRouteSharing", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableRouteSharing")],
                DetectOps = [RegOp.CheckDword(Key, "DisableRouteSharing", 1)],
            },
            new TweakDef
            {
                Id = "mapsbr-disable-personalized-maps",
                Label = "Disable Personalised Map Suggestions",
                Category = "Maps & Browser Policy",
                Description =
                    "Disables personalised place recommendations and 'frequent locations' features in Windows Maps that are based on past search history and route patterns. Prevents the accumulation of a location history profile. Default: personalisation enabled. Recommended: 1.",
                Tags = ["maps", "personalisation", "history", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Maps does not build or use a personal history; no frequent-place suggestions or route preferences are stored.",
                ApplyOps = [RegOp.SetDword(Key, "DisablePersonalisedMaps", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePersonalisedMaps")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePersonalisedMaps", 1)],
            },
            new TweakDef
            {
                Id = "mapsbr-disable-indoor-maps",
                Label = "Disable Indoor Maps Feature",
                Category = "Maps & Browser Policy",
                Description =
                    "Turns off the indoor floor-plan mapping feature in Windows Maps. Indoor maps require additional tile downloads and location data for floor-level positioning. On managed endpoints the feature is rarely needed and adds unnecessary resource usage. Default: indoor maps enabled. Recommended: 1.",
                Tags = ["maps", "indoor", "floorplan", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Indoor floor-plan maps are disabled; building interior layouts are not shown.",
                ApplyOps = [RegOp.SetDword(Key, "DisableIndoorMaps", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableIndoorMaps")],
                DetectOps = [RegOp.CheckDword(Key, "DisableIndoorMaps", 1)],
            },
            new TweakDef
            {
                Id = "mapsbr-disable-3d-maps",
                Label = "Disable 3D Maps View (Birds Eye / Air View)",
                Category = "Maps & Browser Policy",
                Description =
                    "Prevents Windows Maps from loading 3D aerial/birds-eye imagery tiles. 3D tiles are much larger than standard tiles and result in significant bandwidth consumption. On managed endpoints with limited bandwidth or no approved use of Maps, this reduces network overhead. Default: 3D imagery enabled. Recommended: 1.",
                Tags = ["maps", "3d", "aerial", "network", "bandwidth", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "3D birds-eye view tiles are not downloaded; Maps shows only flat 2D cartographic view.",
                ApplyOps = [RegOp.SetDword(Key, "Disable3DMaps", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "Disable3DMaps")],
                DetectOps = [RegOp.CheckDword(Key, "Disable3DMaps", 1)],
            },
        ];
}
