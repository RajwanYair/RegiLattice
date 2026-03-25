// RegiLattice.Core — Tweaks/SensorServicePolicy.cs
// Sprint 294: Sensor Service Policy tweaks (10 tweaks)
// Category: "Sensor Service Policy" | Slug: sensor
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LocationAndSensors

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class SensorServicePolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LocationAndSensors";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "sensor-disable-location",
            Label = "Disable Location Services",
            Category = "Sensor Service Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "The Windows Location Service uses GPS, Wi-Fi positioning, and IP geolocation to determine the device's physical location for applications. Disabling location services through policy prevents all applications from accessing location data on corporate workstations. Enterprise productivity applications typically do not require location access, and allowing it creates unnecessary privacy risks. Location data can reveal workplace addresses, employee movement patterns, and organizational site information. Many compliance frameworks including GDPR and HIPAA require that location tracking be disabled on devices handling sensitive data. Disabling location services has no impact on standard enterprise application workflows.",
            Tags = ["location", "privacy", "sensors", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableLocation", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableLocation")],
            DetectOps = [RegOp.CheckDword(Key, "DisableLocation", 1)],
        },
        new TweakDef
        {
            Id = "sensor-disable-sensors",
            Label = "Disable Sensor Data Access",
            Category = "Sensor Service Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Windows sensors include ambient light sensors, accelerometers, gyroscopes, and other physical measurement devices integrated into modern hardware. Disabling sensor access through policy prevents applications from reading sensor data from integrated hardware platforms. Enterprise desktop and laptop applications rarely require access to environmental sensor data. Sensor data including accelerometer and orientation information can be used for device fingerprinting and motion inference attacks. The sensor framework represents an attack surface that can be eliminated on devices without legitimate sensor data use cases. Disabling sensors does not affect standard input devices, display adapters, or network hardware.",
            Tags = ["sensors", "privacy", "hardware", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableSensors", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableSensors")],
            DetectOps = [RegOp.CheckDword(Key, "DisableSensors", 1)],
        },
        new TweakDef
        {
            Id = "sensor-disable-location-scripting",
            Label = "Disable Location Scripting Access",
            Category = "Sensor Service Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Location scripting allows web applications and scripted environments to query the Windows Location Service through script-based APIs. Disabling location scripting prevents browser-based and script-based applications from requesting location data through the scripting interface. This policy blocks location access from Internet Explorer zones, web-based applications, and Windows Script Host environments. Web applications requesting location data can transmit this information to remote servers without user awareness in fully automated scripts. Enterprise security policies should deny location access to scripting environments as a default-deny principle. Standard desktop application location access is separately controlled by the DisableLocation policy.",
            Tags = ["location", "scripting", "browser", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableLocationScripting", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableLocationScripting")],
            DetectOps = [RegOp.CheckDword(Key, "DisableLocationScripting", 1)],
        },
        new TweakDef
        {
            Id = "sensor-block-location-provider-svc",
            Label = "Disable Windows Location Provider",
            Category = "Sensor Service Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "The Windows Location Provider supplies location information to the Location Service by combining GPS signals, Wi-Fi triangulation, and IP geolocation data. Disabling the Windows Location Provider removes the primary data source for the operating system's location awareness. Applications requesting location data receive a permission-denied response when the location provider is disabled at the policy level. This creates a defense-in-depth configuration where both the location service and its data provider are disabled. Wi-Fi positioning data transmitted to Microsoft's location database during queries is prevented when the provider is disabled. Disabling this provider is a recommended component of enterprise privacy hardening on managed endpoints.",
            Tags = ["location", "provider", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableWindowsLocationProvider", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableWindowsLocationProvider")],
            DetectOps = [RegOp.CheckDword(Key, "DisableWindowsLocationProvider", 1)],
        },
        new TweakDef
        {
            Id = "sensor-disable-location-telemetry",
            Label = "Disable Location Telemetry",
            Category = "Sensor Service Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Location service telemetry reports data about location requests, provider accuracy, and positioning events to Microsoft for service improvement. This telemetry can include coarse location information used to calibrate the positioning algorithms. Disabling location telemetry prevents any location-adjacent data from being transmitted through the telemetry pipeline. Organizations subject to strict privacy regulations cannot permit location data to leave the enterprise through any channel including diagnostic telemetry. Disabling location telemetry complements the DisableLocation policy to ensure comprehensive location data protection. The location service and sensor framework continue to behave identically regardless of telemetry status.",
            Tags = ["location", "telemetry", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableLocationTelemetry", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableLocationTelemetry")],
            DetectOps = [RegOp.CheckDword(Key, "DisableLocationTelemetry", 1)],
        },
        new TweakDef
        {
            Id = "sensor-disable-geocoder",
            Label = "Disable Geocoder Service",
            Category = "Sensor Service Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "The geocoder service converts raw location coordinates from GPS and Wi-Fi positioning into human-readable addresses and place names. Disabling the geocoder prevents applications from using reverse geocoding services that transmit coordinates to remote geocoding APIs. Geocoding requests send precise location coordinates to external services, representing a significant privacy concern for enterprise devices. Applications displaying maps or local amenity information depend on the geocoder for address resolution functionality. Enterprise productivity workstations do not require geocoding functionality for their core business workflows. Disabling the geocoder eliminates external API calls containing device location coordinates.",
            Tags = ["location", "geocoder", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableGeocoderService", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableGeocoderService")],
            DetectOps = [RegOp.CheckDword(Key, "DisableGeocoderService", 1)],
        },
        new TweakDef
        {
            Id = "sensor-disable-location-history",
            Label = "Disable Location History",
            Category = "Sensor Service Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Location history stores a record of positions visited by the device, enabling location-aware applications to provide context from past locations. Disabling location history prevents Windows from maintaining a persistent log of device movements over time. A location history log represents a comprehensive record of employee movements that creates significant privacy and legal risks. Data protection regulations including GDPR classify location history as personal data requiring explicit consent for collection. Disabling location history eliminates this data store from the device, reducing the potential impact of a security breach. No enterprise productivity application requires access to local location history data.",
            Tags = ["location", "history", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableLocationHistory", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableLocationHistory")],
            DetectOps = [RegOp.CheckDword(Key, "DisableLocationHistory", 1)],
        },
        new TweakDef
        {
            Id = "sensor-disable-permission-changes",
            Label = "Disable Location Permission Changes",
            Category = "Sensor Service Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Location permission changes allow applications and users to modify which apps have access to location data through the Privacy settings dialog. Disabling permission changes through policy prevents users from granting or revoking location access for individual applications. Enterprise devices with location services disabled by policy should not allow users to re-enable location access for individual applications. Preventing permission changes enforces the enterprise location policy without creating workarounds for non-compliant applications. This setting ensures the location access policy remains consistent and cannot be circumvented through user settings changes. Administrators retain full control over location access through Group Policy without being overridden by user-level permission modifications.",
            Tags = ["location", "permissions", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableLocationPermissionChanges", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableLocationPermissionChanges")],
            DetectOps = [RegOp.CheckDword(Key, "DisableLocationPermissionChanges", 1)],
        },
        new TweakDef
        {
            Id = "sensor-disable-network-location",
            Label = "Disable Network Location Service",
            Category = "Sensor Service Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "The network location service uses observed Wi-Fi access point information to infer device location through a remote positioning database query. Disabling the network location service prevents Windows from scanning for Wi-Fi networks to determine location even when the device is not connected to Wi-Fi. Wi-Fi scanning for positioning purposes can drain battery and disclose information about nearby wireless infrastructure. Corporate networks have specific RF environments that should not be disclosed through positioning queries to external services. Enterprises with RF security policies benefit from disabling the network location service to prevent uncontrolled Wi-Fi environment disclosure. GPS-based location remains available on devices with GPS hardware if location services are otherwise permitted.",
            Tags = ["location", "network", "wifi", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableNetworkLocationService", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableNetworkLocationService")],
            DetectOps = [RegOp.CheckDword(Key, "DisableNetworkLocationService", 1)],
        },
        new TweakDef
        {
            Id = "sensor-disable-location-broadcast",
            Label = "Disable Location Broadcast",
            Category = "Sensor Service Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Location broadcasting enables the device to share its current location with connected services and applications passively through background broadcasts. Disabling location broadcast prevents the operating system from proactively distributing location information to subscribed services. Passive location broadcasts can enable continuous device tracking by applications registered as location broadcast recipients. Enterprise devices should not broadcast location data to any service without an explicit, justified business need. Location broadcast functionality is relevant for consumer scenarios such as family location sharing that have no place on corporate devices. Disabling location broadcast eliminates a continuous background data egress channel from corporate endpoints.",
            Tags = ["location", "broadcast", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableLocationBroadcast", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableLocationBroadcast")],
            DetectOps = [RegOp.CheckDword(Key, "DisableLocationBroadcast", 1)],
        },
    ];
}
