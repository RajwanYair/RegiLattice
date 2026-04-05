namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// RegiLattice.Core — Tweaks/PolicyProvisions.cs
// Consolidated policy tweaks: Location, DataCollection, WinRM, CredentialUI, MediaPlayer.
// Merged from 5 individual 10-tweak stubs into a single file (v6.12.0 consolidation).
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

internal static class PolicyDataCollection
{
    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            .. _DataCollectionPolicy.Data,
            .. _AppCompatPolicy.Data,
        ];

    // ── Sprint 669a — Data Collection (Telemetry) Advanced Policy ─────────────
    private static class _DataCollectionPolicy
    {
        private const string DcKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "telem-policy-disable-device-census",
                    Label = "Disable Device Census Telemetry Task (Policy)",
                    Category = "Privacy",
                    Description =
                        "Disables the Device Census scheduled task via Group Policy, which collects detailed hardware inventory, installed software, and system configuration data for Microsoft analytics. Default: enabled. Recommended: disabled.",
                    Tags = ["telemetry", "census", "inventory", "policy", "diagnostic"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ApplyOps = [RegOp.SetDword(DcKey, "DisableDeviceCensus", 1)],
                    RemoveOps = [RegOp.DeleteValue(DcKey, "DisableDeviceCensus")],
                    DetectOps = [RegOp.CheckDword(DcKey, "DisableDeviceCensus", 1)],
                },
                new TweakDef
                {
                    Id = "telem-policy-disable-onedrive-sync-telemetry",
                    Label = "Disable OneDrive Diagnostic Telemetry (Policy)",
                    Category = "Privacy",
                    Description =
                        "Disables OneDrive-specific diagnostic telemetry via the DataCollection Group Policy key. Prevents OneDrive from contributing sync diagnostic and usage pattern data to Microsoft telemetry pipelines. Default: enabled. Recommended: disabled.",
                    Tags = ["telemetry", "onedrive", "diagnostic", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ApplyOps = [RegOp.SetDword(DcKey, "DisableOneDriveSyncDiagnostics", 1)],
                    RemoveOps = [RegOp.DeleteValue(DcKey, "DisableOneDriveSyncDiagnostics")],
                    DetectOps = [RegOp.CheckDword(DcKey, "DisableOneDriveSyncDiagnostics", 1)],
                },
            ];
    }

    // ── Sprint 669b — Application Compatibility CEIP Policy ───────────────────
    private static class _AppCompatPolicy
    {
        private const string CompatKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "telem-policy-disable-uac-detection",
                    Label = "Disable Application Compatibility UAC Mitigation Detection",
                    Category = "Privacy",
                    Description =
                        "Disables the Application Compatibility layer from running UAC compatibility mitigations on applications. Reduces compatibility engine overhead and telemetry data about legacy application UAC patterns. Default: enabled. Recommended: disabled on modern application environments.",
                    Tags = ["appcompat", "uac", "compatibility", "policy", "telemetry"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ApplyOps = [RegOp.SetDword(CompatKey, "DisableUACompleteAutomation", 1)],
                    RemoveOps = [RegOp.DeleteValue(CompatKey, "DisableUACompleteAutomation")],
                    DetectOps = [RegOp.CheckDword(CompatKey, "DisableUACompleteAutomation", 1)],
                },
                new TweakDef
                {
                    Id = "telem-policy-disable-ceip-reporting",
                    Label = "Disable Application Compatibility CEIP Reporting",
                    Category = "Privacy",
                    Description =
                        "Disables the Customer Experience Improvement Program (CEIP) data collection within the Application Compatibility subsystem. Stops the engine from uploading crash and compatibility sentinel events to Microsoft telemetry servers. Default: CEIP data uploaded. Recommended: disabled.",
                    Tags = ["appcompat", "ceip", "telemetry", "reporting", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ApplyOps = [RegOp.SetDword(CompatKey, "DisablePropPageShim", 1)],
                    RemoveOps = [RegOp.DeleteValue(CompatKey, "DisablePropPageShim")],
                    DetectOps = [RegOp.CheckDword(CompatKey, "DisablePropPageShim", 1)],
                },
            ];
    }
}

internal static class PolicyWinRM
{
    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            .. _WinRMServicePolicy.Data,
            .. _WinRMClientPolicy.Data,
        ];

    // ── Sprint 670a — WinRM Service (Server Side) Hardening ───────────────────
    private static class _WinRMServicePolicy
    {
        private const string SvcKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinRM\Service";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "sec-winrm-disable-digest-auth",
                    Label = "Disable Digest Authentication on WinRM Service",
                    Category = "Security",
                    Description =
                        "Disables Digest authentication on the WinRM server. Digest auth is weak against offline dictionary and pass-the-hash attacks. Disabling forces more secure protocols (Kerberos, HTTPS + certificate). Default: Digest allowed. Recommended: disabled.",
                    Tags = ["winrm", "digest", "authentication", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ApplyOps = [RegOp.SetDword(SvcKey, "AllowDigest", 0)],
                    RemoveOps = [RegOp.DeleteValue(SvcKey, "AllowDigest")],
                    DetectOps = [RegOp.CheckDword(SvcKey, "AllowDigest", 0)],
                },
                new TweakDef
                {
                    Id = "sec-winrm-disable-auto-config",
                    Label = "Disable WinRM Service Auto-Configuration",
                    Category = "Security",
                    Description =
                        "Prevents the WinRM service from automatically configuring itself at startup. Auto-configuration can silently enable HTTP listeners and create firewall rules without explicit administrator action. Disabling requires explicit manual configuration. Default: auto-config allowed. Recommended: disabled.",
                    Tags = ["winrm", "autoconfig", "listener", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ApplyOps = [RegOp.SetDword(SvcKey, "AllowAutoConfig", 0)],
                    RemoveOps = [RegOp.DeleteValue(SvcKey, "AllowAutoConfig")],
                    DetectOps = [RegOp.CheckDword(SvcKey, "AllowAutoConfig", 0)],
                },
            ];
    }

    // ── Sprint 670b — WinRM Client Hardening ──────────────────────────────────
    private static class _WinRMClientPolicy
    {
        private const string CliKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinRM\Client";

        public static IReadOnlyList<TweakDef> Data =>
            [
            ];
    }
}

internal static class PolicyCredentialUI
{
    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            .. _CredUIPolicy.Data,
            .. _CredProviderPolicy.Data,
        ];

    // ── Sprint 671a — Credential UI (LogonUI) Policy ──────────────────────────
    private static class _CredUIPolicy
    {
        private const string CredUiKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CredUI";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "sec-credui-require-trusted-path",
                    Label = "Require Trusted Path for Credential Entry",
                    Category = "Security",
                    Description =
                        "Forces users to enter credentials through the Windows trusted path (Ctrl+Alt+Del secure desktop), rather than through possibly spoofed application dialogs. Prevents credential theft by fake login windows. Default: trusted path optional. Recommended: required.",
                    Tags = ["credential", "secure-desktop", "uac", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ApplyOps = [RegOp.SetDword(CredUiKey, "RequireTrustedPath", 1)],
                    RemoveOps = [RegOp.DeleteValue(CredUiKey, "RequireTrustedPath")],
                    DetectOps = [RegOp.CheckDword(CredUiKey, "RequireTrustedPath", 1)],
                },
                new TweakDef
                {
                    Id = "sec-credui-disable-web-creds-provider",
                    Label = "Disable Web Credential Provider in Logon UI",
                    Category = "Security",
                    Description =
                        "Blocks the Web Credentials provider tile (Microsoft Account, AAD web-auth) from appearing in the Windows Logon UI and UAC elevation dialogs. Reduces the attack surface by removing web-based authentication paths at the logon screen. Default: web credential tile shown. Recommended: disabled on enterprise domain machines.",
                    Tags = ["credential", "web", "msa", "logon", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ApplyOps = [RegOp.SetDword(CredUiKey, "DisableWebCredentialUI", 1)],
                    RemoveOps = [RegOp.DeleteValue(CredUiKey, "DisableWebCredentialUI")],
                    DetectOps = [RegOp.CheckDword(CredUiKey, "DisableWebCredentialUI", 1)],
                },
            ];
    }

    // ── Sprint 671b — Credential Provider (Winlogon) Policy ──────────────────
    private static class _CredProviderPolicy
    {
        private const string ProvKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "sec-credprov-enable-logon-legal-notice",
                    Label = "Enable Legal Notice at Logon (Compliance Banner)",
                    Category = "Security",
                    Description =
                        "Displays a legal notice (warning banner) to all users before they log on. Required by many compliance frameworks (NIST 800-53 AC-8, CIS L1, STIG) to establish authorized-use policy. Default: no legal notice. Recommended: enabled with organization-specific text.",
                    Tags = ["credential", "logon", "compliance", "legal", "banner", "cis", "nist", "stig", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ApplyOps = [RegOp.SetString(ProvKey, "legalnoticecaption", "Authorized Use Only")],
                    RemoveOps = [RegOp.DeleteValue(ProvKey, "legalnoticecaption")],
                    DetectOps = [RegOp.CheckMissing(ProvKey, "legalnoticecaption")],
                },
                new TweakDef
                {
                    Id = "sec-credprov-disable-shutdown-without-logon",
                    Label = "Disable Shutdown Button on Windows Logon Screen",
                    Category = "Security",
                    Description =
                        "Removes the Shutdown button from the Windows logon/lock screen. Prevents unauthenticated users from shutting down the machine, which could interrupt services, bypass auto-start security tools, or cause data loss. Default: Shutdown button visible. Recommended: disabled on servers and shared workstations.",
                    Tags = ["logon", "shutdown", "lockscreen", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ApplyOps = [RegOp.SetDword(ProvKey, "ShutdownWithoutLogon", 0)],
                    RemoveOps = [RegOp.DeleteValue(ProvKey, "ShutdownWithoutLogon")],
                    DetectOps = [RegOp.CheckDword(ProvKey, "ShutdownWithoutLogon", 0)],
                },
            ];
    }
}

internal static class PolicyMediaPlayer
{
    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            .. _WmpPolicy.Data,
        ];

    // ── Sprint 672 — Windows Media Player Policy ──────────────────────────────
    private static class _WmpPolicy
    {
        private const string WmpKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "media-policy-disable-first-run",
                    Label = "Disable Windows Media Player First-Run Setup Wizard",
                    Category = "Audio & Media",
                    Description =
                        "Prevents Windows Media Player from showing the first-run setup wizard that asks users to configure privacy, codec download, and CodecLink options. The wizard can silently enable online data sharing. Default: first-run wizard shown on launch. Recommended: disabled.",
                    Tags = ["wmp", "media-player", "setup", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ApplyOps = [RegOp.SetDword(WmpKey, "GroupPrivacyAcceptance", 1)],
                    RemoveOps = [RegOp.DeleteValue(WmpKey, "GroupPrivacyAcceptance")],
                    DetectOps = [RegOp.CheckDword(WmpKey, "GroupPrivacyAcceptance", 1)],
                },
                new TweakDef
                {
                    Id = "media-policy-disable-mms-protocol",
                    Label = "Disable MMS Streaming Protocol in Media Player",
                    Category = "Audio & Media",
                    Description =
                        "Blocks Windows Media Player from using the legacy MMS (Microsoft Media Server) streaming protocol. MMS uses unauthenticated UDP/TCP streams and is deprecated; blocking it reduces the network attack surface. Default: MMS protocol allowed. Recommended: disabled.",
                    Tags = ["wmp", "mms", "streaming", "network", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ApplyOps = [RegOp.SetDword(WmpKey, "PreventMMSProtocol", 1)],
                    RemoveOps = [RegOp.DeleteValue(WmpKey, "PreventMMSProtocol")],
                    DetectOps = [RegOp.CheckDword(WmpKey, "PreventMMSProtocol", 1)],
                },
                new TweakDef
                {
                    Id = "media-policy-disable-drm-online",
                    Label = "Disable Online DRM Licence Acquisition in Media Player",
                    Category = "Audio & Media",
                    Description =
                        "Prevents Windows Media Player from contacting Microsoft's DRM licensing servers to acquire playback licences for protected content. Online licence acquisition sends media metadata and hardware fingerprint data to Microsoft. Default: online licence acquisition enabled. Recommended: disabled.",
                    Tags = ["wmp", "drm", "licence", "privacy", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ApplyOps = [RegOp.SetDword(WmpKey, "PreventDRMLicenseAcquisition", 1)],
                    RemoveOps = [RegOp.DeleteValue(WmpKey, "PreventDRMLicenseAcquisition")],
                    DetectOps = [RegOp.CheckDword(WmpKey, "PreventDRMLicenseAcquisition", 1)],
                },
                new TweakDef
                {
                    Id = "media-policy-disable-radio-ui",
                    Label = "Hide Windows Media Player Radio UI",
                    Category = "Audio & Media",
                    Description =
                        "Hides the Radio feature in Windows Media Player, which streams internet radio content via Microsoft's WindowsMedia.com service. The Radio UI includes usage tracking and content recommendations. Default: Radio UI visible. Recommended: hidden.",
                    Tags = ["wmp", "radio", "streaming", "privacy", "ui", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ApplyOps = [RegOp.SetDword(WmpKey, "PreventRadioPresence", 1)],
                    RemoveOps = [RegOp.DeleteValue(WmpKey, "PreventRadioPresence")],
                    DetectOps = [RegOp.CheckDword(WmpKey, "PreventRadioPresence", 1)],
                },
                new TweakDef
                {
                    Id = "media-policy-disable-network-buffering",
                    Label = "Disable Predictive Network Buffering in Media Player",
                    Category = "Audio & Media",
                    Description =
                        "Disables the predictive network buffering feature that pre-fetches additional stream data based on playback patterns. Pre-fetch behaviour creates passive network chatter that can be used for traffic fingerprinting of media consumption. Default: enabled. Recommended: disabled for privacy.",
                    Tags = ["wmp", "buffering", "network", "privacy", "streaming", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ApplyOps = [RegOp.SetDword(WmpKey, "DisableNetworkSettings", 1)],
                    RemoveOps = [RegOp.DeleteValue(WmpKey, "DisableNetworkSettings")],
                    DetectOps = [RegOp.CheckDword(WmpKey, "DisableNetworkSettings", 1)],
                },
                new TweakDef
                {
                    Id = "media-policy-hide-privacy-tab",
                    Label = "Lock Media Player Privacy Settings via Policy",
                    Category = "Audio & Media",
                    Description =
                        "Hides the Privacy tab in Windows Media Player Options, preventing users from changing privacy settings (codec download, metadata retrieval, usage reporting). Used together with the other WMP policy tweaks to lock a hardened configuration in place. Default: Privacy tab accessible. Recommended: hidden after hardening.",
                    Tags = ["wmp", "privacy", "settings", "lockdown", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ApplyOps = [RegOp.SetDword(WmpKey, "HidePrivacyTab", 1)],
                    RemoveOps = [RegOp.DeleteValue(WmpKey, "HidePrivacyTab")],
                    DetectOps = [RegOp.CheckDword(WmpKey, "HidePrivacyTab", 1)],
                },
            ];
    }
}
