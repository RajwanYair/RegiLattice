// RegiLattice.Core — Tweaks/LocationSensors.cs
// Location services, sensor framework, and per-app location access policies (Sprint 136).
// Slug "loc" — LocationAndSensors, AppPrivacy, and HKCU CapabilityAccessManager paths.
// Complements priv-disable-location (Privacy.cs) and aperm-deny-location (AppPermissions.cs).

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class LocationSensors
{
    private const string LocPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LocationAndSensors";
    private const string AppPrivacy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy";
    private const string WinSearch = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search";
    private const string WifiConfig = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\WcmSvc\wifinetworkmanager\config";
    private const string UserLocation = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\location";
    private const string UserActivity = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\activity";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "loc-disable-location-scripting",
            Label = "Disable Location Scripting",
            Category = "Location & Sensors",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Blocks scripted applications (ActiveX, WSH) from querying device location "
                + "via the Windows Location scripting interface. DisableLocationScripting=1.",
            Tags = ["location", "privacy", "scripting", "activex"],
            RegistryKeys = [LocPolicy],
            ApplyOps = [RegOp.SetDword(LocPolicy, "DisableLocationScripting", 1)],
            RemoveOps = [RegOp.DeleteValue(LocPolicy, "DisableLocationScripting")],
            DetectOps = [RegOp.CheckDword(LocPolicy, "DisableLocationScripting", 1)],
        },
        new TweakDef
        {
            Id = "loc-disable-sensors",
            Label = "Disable Windows Sensor Platform",
            Category = "Location & Sensors",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Disables the Windows Sensor framework, blocking access to hardware sensors "
                + "such as ambient-light, accelerometer, and barometer. DisableSensors=1.",
            Tags = ["location", "sensors", "hardware", "privacy"],
            RegistryKeys = [LocPolicy],
            ApplyOps = [RegOp.SetDword(LocPolicy, "DisableSensors", 1)],
            RemoveOps = [RegOp.DeleteValue(LocPolicy, "DisableSensors")],
            DetectOps = [RegOp.CheckDword(LocPolicy, "DisableSensors", 1)],
        },
        new TweakDef
        {
            Id = "loc-disable-location-provider",
            Label = "Disable Windows Location Provider",
            Category = "Location & Sensors",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Disables the Windows Location Provider (WLP), which supplies geolocation "
                + "data to apps from GPS, Wi-Fi triangulation, and IP geolocation. "
                + "DisableWindowsLocationProvider=1.",
            Tags = ["location", "provider", "gps", "privacy"],
            RegistryKeys = [LocPolicy],
            ApplyOps = [RegOp.SetDword(LocPolicy, "DisableWindowsLocationProvider", 1)],
            RemoveOps = [RegOp.DeleteValue(LocPolicy, "DisableWindowsLocationProvider")],
            DetectOps = [RegOp.CheckDword(LocPolicy, "DisableWindowsLocationProvider", 1)],
        },
        new TweakDef
        {
            Id = "loc-policy-deny-app-location",
            Label = "Policy: Force-Deny All UWP Apps Location Access",
            Category = "Location & Sensors",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Machine-level group policy that forces all UWP apps to be denied location "
                + "access regardless of user consent. LetAppsAccessLocation=2 (Force Deny). "
                + "Complements priv-disable-location (which disables the OS service).",
            Tags = ["location", "policy", "app privacy", "uwp"],
            RegistryKeys = [AppPrivacy],
            ApplyOps = [RegOp.SetDword(AppPrivacy, "LetAppsAccessLocation", 2)],
            RemoveOps = [RegOp.DeleteValue(AppPrivacy, "LetAppsAccessLocation")],
            DetectOps = [RegOp.CheckDword(AppPrivacy, "LetAppsAccessLocation", 2)],
        },
        new TweakDef
        {
            Id = "loc-policy-deny-app-motion",
            Label = "Policy: Force-Deny All UWP Apps Motion Sensor Access",
            Category = "Location & Sensors",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Machine-wide policy preventing all UWP apps from reading motion and "
                + "activity sensors (step counter, gyroscope). LetAppsAccessMotion=2.",
            Tags = ["location", "motion", "sensors", "app privacy", "uwp"],
            RegistryKeys = [AppPrivacy],
            ApplyOps = [RegOp.SetDword(AppPrivacy, "LetAppsAccessMotion", 2)],
            RemoveOps = [RegOp.DeleteValue(AppPrivacy, "LetAppsAccessMotion")],
            DetectOps = [RegOp.CheckDword(AppPrivacy, "LetAppsAccessMotion", 2)],
        },
        new TweakDef
        {
            Id = "loc-disable-search-location",
            Label = "Disable Location-Based Windows Search Results",
            Category = "Location & Sensors",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Description =
                "Prevents Windows Search from refining results based on the device's geographic "
                + "location. AllowSearchToUseLocation=0 in the Windows Search policy.",
            Tags = ["location", "search", "cortana", "privacy"],
            RegistryKeys = [WinSearch],
            ApplyOps = [RegOp.SetDword(WinSearch, "AllowSearchToUseLocation", 0)],
            RemoveOps = [RegOp.DeleteValue(WinSearch, "AllowSearchToUseLocation")],
            DetectOps = [RegOp.CheckDword(WinSearch, "AllowSearchToUseLocation", 0)],
        },
        new TweakDef
        {
            Id = "loc-disable-wifi-location",
            Label = "Disable Wi-Fi Location Auto-Connect",
            Category = "Location & Sensors",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Description =
                "Disables Wi-Fi location-aware automatic connection feature. "
                + "AutoConnectAllowedOEM=0 prevents location-based Wi-Fi network switching.",
            Tags = ["location", "wifi", "auto-connect", "privacy", "network"],
            RegistryKeys = [WifiConfig],
            ApplyOps = [RegOp.SetDword(WifiConfig, "AutoConnectAllowedOEM", 0)],
            RemoveOps = [RegOp.DeleteValue(WifiConfig, "AutoConnectAllowedOEM")],
            DetectOps = [RegOp.CheckDword(WifiConfig, "AutoConnectAllowedOEM", 0)],
        },
        new TweakDef
        {
            Id = "loc-user-deny-location",
            Label = "Turn Off Location Access for Current User",
            Category = "Location & Sensors",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Sets the current user's location permission to Deny in the Windows "
                + "capability access store. All apps that request location receive a "
                + "denied response. Value=\"Deny\" in ConsentStore\\location.",
            Tags = ["location", "privacy", "user", "consent"],
            RegistryKeys = [UserLocation],
            ApplyOps = [RegOp.SetString(UserLocation, "Value", "Deny")],
            RemoveOps = [RegOp.SetString(UserLocation, "Value", "Allow")],
            DetectOps = [RegOp.CheckString(UserLocation, "Value", "Deny")],
        },
        new TweakDef
        {
            Id = "loc-user-deny-activity-sensors",
            Label = "Turn Off Activity Sensor Access for Current User",
            Category = "Location & Sensors",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Denies all apps access to the motion and activity sensors (pedometer, "
                + "accelerometer) for the current user via the ConsentStore. "
                + "Value=\"Deny\" in ConsentStore\\activity.",
            Tags = ["location", "activity", "motion", "sensors", "privacy"],
            RegistryKeys = [UserActivity],
            ApplyOps = [RegOp.SetString(UserActivity, "Value", "Deny")],
            RemoveOps = [RegOp.SetString(UserActivity, "Value", "Allow")],
            DetectOps = [RegOp.CheckString(UserActivity, "Value", "Deny")],
        },
        new TweakDef
        {
            Id = "loc-disable-ie-geolocation",
            Label = "Disable Internet Explorer Geolocation API",
            Category = "Location & Sensors",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Description =
                "Blocks Internet Explorer and legacy IE-based web applications from "
                + "requesting device geolocation via the HTML5 Geolocation API. "
                + "PolicyDisableGeolocation=1.",
            Tags = ["location", "ie", "legacy", "geolocation", "browser"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Internet Explorer\Geolocation", "PolicyDisableGeolocation", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Internet Explorer\Geolocation", "PolicyDisableGeolocation"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Internet Explorer\Geolocation", "PolicyDisableGeolocation", 1),
            ],
        },
    ];
}
