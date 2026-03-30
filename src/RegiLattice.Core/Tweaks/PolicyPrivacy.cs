// RegiLattice.Core — Tweaks/PolicyPrivacy.cs
// Advertising ID, data collection, feedback, location sensors, push notifications, Windows diagnostics, and privacy policy controls
// Category: "Privacy & Telemetry Policy"
// Consolidated from 18 modules.

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PolicyPrivacy
{
    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            .. _AdvertisingInfoPolicy.Data,
            .. _DataCollectionPolicy.Data,
            .. _DataIntegrityPolicy.Data,
            .. _FeedbackPolicy.Data,
            .. _LocationSensors.Data,
            .. _LocationSensorsPolicy.Data,
            .. _MicrosoftAccount.Data,
            .. _PhotoAcquisitionPolicy.Data,
            .. _PushNotificationsPolicy.Data,
            .. _SearchWebPolicy.Data,
            .. _SensorPolicy.Data,
            .. _SensorServicePolicy.Data,
            .. _SpellingAndTypingPolicy.Data,
            .. _WindowsDiagnostics.Data,
            .. _WindowsDiagnosticsInfraPolicy.Data,
            .. _WindowsDiagTrackPolicy.Data,
            .. _WindowsInfoProtectionPolicy.Data,
            .. _WindowsMapsPolicy.Data,
        ];

    // ── AdvertisingInfoPolicy ──
    private static class _AdvertisingInfoPolicy
    {    
        private const string Key =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AdvertisingInfo";
    
        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "advinfo-disable-advertising-id",
                    Label = "Disable Windows Advertising ID (RUID)",
                    Category = "Privacy & Telemetry Policy",
                    Description =
                        "Disables the Windows Advertising ID (also called Resettable Unique Identifier / RUID) that apps use to deliver targeted advertising across sessions. Prevents cross-app tracking of user behaviour. Default: enabled. Recommended: 1 for all privacy-conscious deployments.",
                    Tags = ["advertising", "adid", "tracking", "privacy", "ruid", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Advertising ID is disabled; apps cannot correlate ad impressions across sessions or users.",
                    ApplyOps = [RegOp.SetDword(Key, "DisabledByGroupPolicy", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisabledByGroupPolicy")],
                    DetectOps = [RegOp.CheckDword(Key, "DisabledByGroupPolicy", 1)],
                },
                new TweakDef
                {
                    Id = "advinfo-disable-personalised-ads",
                    Label = "Disable Personalised Ad Delivery",
                    Category = "Privacy & Telemetry Policy",
                    Description =
                        "Prevents Windows from delivering personalised ads based on browsing history, app usage, and interest profiles. Ads shown in apps and the OS are non-personalised. Default: personalised ads on. Recommended: 1.",
                    Tags = ["advertising", "personalised", "targeting", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Personalised ad targeting is disabled; only generic, non-profiled ads may appear.",
                    ApplyOps = [RegOp.SetDword(Key, "DisablePersonalisedAds", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisablePersonalisedAds")],
                    DetectOps = [RegOp.CheckDword(Key, "DisablePersonalisedAds", 1)],
                },
                new TweakDef
                {
                    Id = "advinfo-block-ad-id-reset",
                    Label = "Block User from Resetting / Re-Enabling Advertising ID",
                    Category = "Privacy & Telemetry Policy",
                    Description =
                        "Prevents users from navigating to Settings → Privacy → General and re-enabling or resetting the Advertising ID. Ensures the enterprise policy remains in effect. Default: users can change. Recommended: 1 on managed endpoints.",
                    Tags = ["advertising", "adid", "user-restriction", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Settings page advertising ID toggle is greyed out; users cannot re-enable or reset it.",
                    ApplyOps = [RegOp.SetDword(Key, "PreventUserOverride", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "PreventUserOverride")],
                    DetectOps = [RegOp.CheckDword(Key, "PreventUserOverride", 1)],
                },
                new TweakDef
                {
                    Id = "advinfo-disable-interest-profile",
                    Label = "Disable Interest Profile Building for Ads",
                    Category = "Privacy & Telemetry Policy",
                    Description =
                        "Prevents Windows from building an interest and behaviour profile based on usage patterns to improve ad targeting. Stops data collection that feeds the Microsoft ad platform. Default: profiling active. Recommended: 1.",
                    Tags = ["advertising", "profiling", "interests", "privacy", "telemetry", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Interest profile for ad purposes is not built; Microsoft's ad platform receives no usage pattern data.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableInterestProfile", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableInterestProfile")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableInterestProfile", 1)],
                },
                new TweakDef
                {
                    Id = "advinfo-disable-cross-device-ad-sync",
                    Label = "Disable Cross-Device Advertising ID Sync",
                    Category = "Privacy & Telemetry Policy",
                    Description =
                        "Prevents synchronisation of the Advertising ID across devices signed in with the same Microsoft Account. Eliminates cross-device ad targeting linking a user's phone, tablet, and PC. Default: sync enabled. Recommended: 1.",
                    Tags = ["advertising", "cross-device", "sync", "privacy", "account", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Advertising ID is not shared across devices; ad targeting cannot link behaviour across a user's device fleet.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableCrossDeviceSync", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableCrossDeviceSync")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableCrossDeviceSync", 1)],
                },
                new TweakDef
                {
                    Id = "advinfo-disable-location-for-ads",
                    Label = "Block Location Data for Advertising",
                    Category = "Privacy & Telemetry Policy",
                    Description =
                        "Prevents Windows from using geographic location data to serve location-targeted advertisements. Default: location can be used by ad platform. Recommended: 1 for privacy.",
                    Tags = ["advertising", "location", "privacy", "geotargeting", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Location data is not used by the advertising platform; geotargeted ads cannot be served.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableLocationForAds", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableLocationForAds")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableLocationForAds", 1)],
                },
                new TweakDef
                {
                    Id = "advinfo-disable-app-ad-consent-requests",
                    Label = "Block Apps from Requesting Ad Consent",
                    Category = "Privacy & Telemetry Policy",
                    Description =
                        "Prevents apps from displaying permission dialogs asking the user to consent to advertising-related data collection. Returns 'denied' to all such requests without prompting. Default: consent prompts allowed. Recommended: 1.",
                    Tags = ["advertising", "consent", "apps", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Ad consent dialogs from apps are suppressed; system silently denies all ad-data consent requests.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockAdConsentRequests", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockAdConsentRequests")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockAdConsentRequests", 1)],
                },
                new TweakDef
                {
                    Id = "advinfo-disable-ad-activity-history",
                    Label = "Disable Ad Activity History Collection",
                    Category = "Privacy & Telemetry Policy",
                    Description =
                        "Stops Windows from collecting and retaining a history of ad impression events (which ads were shown, clicked, or dismissed). Removes a persistent data trail used for attribution and retargeting. Default: history retained. Recommended: 1.",
                    Tags = ["advertising", "history", "impressions", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "No ad activity history is stored; retargeting and frequency-capping features in ad platforms are degraded.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableAdActivityHistory", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableAdActivityHistory")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableAdActivityHistory", 1)],
                },
                new TweakDef
                {
                    Id = "advinfo-hide-advertising-settings-page",
                    Label = "Hide Advertising Privacy Settings page",
                    Category = "Privacy & Telemetry Policy",
                    Description =
                        "Removes the Advertising ID sub-page from Settings → Privacy → General. Prevents users from discovering or interacting with advertising-related privacy controls. Default: page visible. Recommended: 1 on locked-down kiosk endpoints.",
                    Tags = ["advertising", "settings", "ui-restriction", "kiosk", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Advertising ID section is hidden from Settings; purely cosmetic — does not change underlying ID state.",
                    ApplyOps = [RegOp.SetDword(Key, "HideAdvertisingSettingsPage", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "HideAdvertisingSettingsPage")],
                    DetectOps = [RegOp.CheckDword(Key, "HideAdvertisingSettingsPage", 1)],
                },
                new TweakDef
                {
                    Id = "advinfo-disable-diagnostic-ad-feedback",
                    Label = "Disable Diagnostic Feedback for Ad Measurement",
                    Category = "Privacy & Telemetry Policy",
                    Description =
                        "Prevents Windows from sending diagnostic events (install, launch, purchase, uninstall) to Microsoft for use in advertising measurement and attribution. Reduces silently collected conversion data. Default: feedback sent. Recommended: 1.",
                    Tags = ["advertising", "diagnostics", "attribution", "telemetry", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "App install/launch/purchase conversion events are not sent to Microsoft's ad measurement service.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableDiagnosticAdFeedback", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableDiagnosticAdFeedback")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableDiagnosticAdFeedback", 1)],
                },
            ];
    
    }

    // ── DataCollectionPolicy ──
    private static class _DataCollectionPolicy
    {    
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection";
        private const string SqmKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient\Windows";
        private const string DastKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DAST";
    
        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "datacol-disable-opt-in-notification",
                Label = "Suppress Telemetry Opt-In Change Notification",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Prevents Windows from displaying a notification banner when the telemetry level changes. Eliminates user-visible popups during telemetry configuration.",
                Tags = ["telemetry", "notification", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Silences telemetry-change banners shown during deployments.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableTelemetryOptInChangeNotification", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableTelemetryOptInChangeNotification")],
                DetectOps = [RegOp.CheckDword(Key, "DisableTelemetryOptInChangeNotification", 1)],
            },
            new TweakDef
            {
                Id = "datacol-hide-telemetry-settings-ui",
                Label = "Hide Telemetry Controls from Settings",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Removes the Diagnostic & Feedback section from Windows Settings, preventing users from changing telemetry level or viewing diagnostic data.",
                Tags = ["telemetry", "settings", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Hides Diagnostic & Feedback settings from users.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableTelemetryOptInSettingsUx", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableTelemetryOptInSettingsUx")],
                DetectOps = [RegOp.CheckDword(Key, "DisableTelemetryOptInSettingsUx", 1)],
            },
            new TweakDef
            {
                Id = "datacol-disable-enterprise-auth-proxy",
                Label = "Disable Enterprise Auth Proxy for Telemetry",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Prevents Windows telemetry service from using authenticated proxy servers to send diagnostic data. Forces direct transmission or blocks it entirely.",
                Tags = ["telemetry", "proxy", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Prevents telemetry from routing through enterprise auth proxies.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableEnterpriseAuthProxy", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableEnterpriseAuthProxy")],
                DetectOps = [RegOp.CheckDword(Key, "DisableEnterpriseAuthProxy", 1)],
            },
            new TweakDef
            {
                Id = "datacol-disable-device-delete-button",
                Label = "Disable Delete Device Diagnostic Data Button",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Prevents users from deleting device diagnostic data via the 'Delete diagnostic data' button in Settings > Privacy > Diagnostics & Feedback.",
                Tags = ["telemetry", "diagnostic-data", "settings", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Prevents users from erasing diagnostic data via the Settings delete button.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableDeviceDelete", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDeviceDelete")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDeviceDelete", 1)],
            },
            new TweakDef
            {
                Id = "datacol-disable-feedback-notifications",
                Label = "Suppress Windows Feedback Reminder Pop-Ups",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Prevents Windows from displaying feedback reminder notifications that prompt users to rate experiences or submit feedback to Microsoft.",
                Tags = ["feedback", "notifications", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Removes Windows feedback notification pop-ups.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DoNotShowFeedbackNotifications", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DoNotShowFeedbackNotifications")],
                DetectOps = [RegOp.CheckDword(Key, "DoNotShowFeedbackNotifications", 1)],
            },
            new TweakDef
            {
                Id = "datacol-disable-device-name-in-telemetry",
                Label = "Block Device Name in Telemetry Submissions",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Prevents Windows from including the device's computer name in telemetry payloads sent to Microsoft. Reduces machine-identifying data in diagnostics.",
                Tags = ["telemetry", "device-name", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Stops the machine hostname from being sent in telemetry payloads.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowDeviceNameInTelemetry", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowDeviceNameInTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "AllowDeviceNameInTelemetry", 0)],
            },
            new TweakDef
            {
                Id = "datacol-disable-ceip",
                Label = "Disable Customer Experience Improvement Program",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Disables Microsoft's Customer Experience Improvement Program (CEIP) which collects anonymous usage statistics across Windows components. Policy-level disable.",
                Tags = ["ceip", "telemetry", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Disables Microsoft's Customer Experience Improvement Program.",
                RegistryKeys = [SqmKey],
                ApplyOps = [RegOp.SetDword(SqmKey, "CEIPEnable", 0)],
                RemoveOps = [RegOp.DeleteValue(SqmKey, "CEIPEnable")],
                DetectOps = [RegOp.CheckDword(SqmKey, "CEIPEnable", 0)],
            },
            new TweakDef
            {
                Id = "datacol-disable-sample-submission",
                Label = "Disable File Sample Submission to Microsoft",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Prevents Windows Diagnostic Analysis Service (DAST) and Windows Defender from submitting file samples to Microsoft's analysis cloud for threat intelligence.",
                Tags = ["sample-submission", "telemetry", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "Stops file samples going to Microsoft cloud; may reduce threat detection speed.",
                RegistryKeys = [DastKey],
                ApplyOps = [RegOp.SetDword(DastKey, "AllowSampleSubmission", 0)],
                RemoveOps = [RegOp.DeleteValue(DastKey, "AllowSampleSubmission")],
                DetectOps = [RegOp.CheckDword(DastKey, "AllowSampleSubmission", 0)],
            },
            new TweakDef
            {
                Id = "datacol-disable-onesettings-downloads",
                Label = "Block WindowsOneSettings Telemetry Overrides",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Prevents Windows from downloading one-time configuration overrides (OneSettings) that can dynamically change data collection settings without a Windows Update.",
                Tags = ["telemetry", "onesettings", "policy", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents Microsoft from remotely changing data collection settings via OneSettings.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableOneSettingsDownloads", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableOneSettingsDownloads")],
                DetectOps = [RegOp.CheckDword(Key, "DisableOneSettingsDownloads", 1)],
            },
            new TweakDef
            {
                Id = "datacol-disable-diagnostic-page",
                Label = "Hide Diagnostic Data Viewer Page in Settings",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Hides the Diagnostic Data Viewer page from Windows Settings > Privacy & Security > Diagnostics & Feedback, preventing users from reviewing diagnostic data submissions.",
                Tags = ["telemetry", "diagnostic-data", "settings", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Hides the Diagnostic Data Viewer page from Settings.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "HideDiagnosticPage", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "HideDiagnosticPage")],
                DetectOps = [RegOp.CheckDword(Key, "HideDiagnosticPage", 1)],
            },
        ];
    
    }

    // ── DataIntegrityPolicy ──
    private static class _DataIntegrityPolicy
    {    
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataIntegrity";
    
        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "dataintg-enable-integrity-checks",
                Label = "Enable System Data Integrity Checks",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                Description =
                    "System data integrity checks verify that critical Windows system files and data structures have not been modified from their expected state. Enabling integrity checks activates continuous monitoring of protected system components for unauthorized modification. Tampered system files are a hallmark of advanced persistent threat attacks seeking to establish persistent footholds. Integrity verification provides early detection of rootkits, bootkit infections, and file system tampering. Enterprise security operations should monitor integrity check violations as high-priority security events. Enabling integrity checks has a minor performance impact but provides substantial assurance against stealthy compromise scenarios.",
                Tags = ["integrity", "security", "system", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableIntegrityChecks", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableIntegrityChecks")],
                DetectOps = [RegOp.CheckDword(Key, "EnableIntegrityChecks", 1)],
            },
            new TweakDef
            {
                Id = "dataintg-enable-runtime-verification",
                Label = "Enable Runtime Data Integrity Verification",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Runtime data integrity verification continuously monitors in-memory data structures critical to system security against modification. Enabling runtime verification detects attempts to tamper with security-critical data while it resides in memory. In-memory attacks that modify security tokens, ACLs, or kernel data structures are a sophisticated evasion technique. Runtime integrity monitoring catches these attacks before they can cause persistent damage or expand privilege. Security logging of runtime integrity violations enables the SOC to identify attacker actions against memory structures. The performance overhead of runtime verification is acceptable given the significant detection capability it provides.",
                Tags = ["integrity", "runtime", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableRuntimeVerification", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableRuntimeVerification")],
                DetectOps = [RegOp.CheckDword(Key, "EnableRuntimeVerification", 1)],
            },
            new TweakDef
            {
                Id = "dataintg-disable-bypass",
                Label = "Disable Data Integrity Check Bypass",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                Description =
                    "Data integrity check bypass mechanisms allow exceptions to be granted for files or components that fail integrity verification. Disabling bypass mechanisms ensures that integrity failures are enforced without exception and cannot be overridden. Bypass mechanisms intended as temporary compatibility workarounds may be abused by attackers to suppress detection of tampering. A strict no-bypass enforcement model maximizes the effectiveness of integrity checking as a detective and preventive control. Legitimate software compatibility issues should be resolved through proper code signing and integrity manifest maintenance. Removing bypass paths eliminates a potential attacker technique for silencing integrity monitoring while maintaining a compromised system state.",
                Tags = ["integrity", "bypass", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableIntegrityBypass", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableIntegrityBypass")],
                DetectOps = [RegOp.CheckDword(Key, "DisableIntegrityBypass", 1)],
            },
            new TweakDef
            {
                Id = "dataintg-enable-audit-logging",
                Label = "Enable Data Integrity Audit Logging",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Data integrity audit logging records integrity check results and any violations to the Windows Security event log. Enabling audit logging ensures that all integrity check failures are captured as auditable events for SOC investigation. Integrity violation events contain information about the modified component, modification type, and associated process. Security operations can use these events to identify compromise scope and timeline during incident response. Audit log entries for integrity violations should trigger high-priority alerts in SIEM systems. Enabling audit logging has negligible performance impact and is essential for maintaining a complete security audit record.",
                Tags = ["integrity", "logging", "audit", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableIntegrityAuditLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableIntegrityAuditLogging")],
                DetectOps = [RegOp.CheckDword(Key, "EnableIntegrityAuditLogging", 1)],
            },
            new TweakDef
            {
                Id = "dataintg-enable-boot-verification",
                Label = "Enable Boot-Time Data Integrity Verification",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                Description =
                    "Boot-time data integrity verification checks the integrity of the Windows boot chain components before the operating system fully initializes. Enabling boot verification detects modifications to the bootloader, kernel, and early-launch drivers before they have a chance to execute. Bootkits that compromise the boot process are among the most difficult malware types to detect and remediate. Early detection during the boot phase enables recovery mechanisms to quarantine or remediate compromised components. Secure Boot and ELAM work together with boot-time integrity verification to create a chain of trust from firmware to OS. Boot-time verification results influence the security posture assessment used by attestation and device health services.",
                Tags = ["integrity", "boot", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableBootVerification", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableBootVerification")],
                DetectOps = [RegOp.CheckDword(Key, "EnableBootVerification", 1)],
            },
            new TweakDef
            {
                Id = "dataintg-enable-usermode-checks",
                Label = "Enable User-Mode Data Integrity Checks",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "User-mode integrity checks verify the integrity of user-space process images and libraries against expected hash values. Enabling user-mode integrity verification detects attempts to tamper with application binaries and shared libraries on the filesystem. In-memory patching and DLL replacement attacks target user-mode processes to hijack application execution. User-mode integrity checking provides a complementary layer to kernel-mode protections. Enterprise endpoint protection requires integrity verification across both kernel and user spaces for comprehensive coverage. Suspicious user-mode integrity failures should be logged and acted upon as potential signs of malicious persistence mechanisms.",
                Tags = ["integrity", "user-mode", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableUserModeIntegrityChecks", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableUserModeIntegrityChecks")],
                DetectOps = [RegOp.CheckDword(Key, "EnableUserModeIntegrityChecks", 1)],
            },
            new TweakDef
            {
                Id = "dataintg-disable-rollback",
                Label = "Disable Data Integrity Version Rollback",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Description =
                    "Version rollback allows integrity-checked components to be replaced with older versions of the same component under certain conditions. Disabling version rollback prevents downgrade attacks that replace current patched versions of protected components with older vulnerable versions. Downgrade attacks are used to reintroduce known vulnerabilities that have been patched in current software versions. Attackers can use rollback capabilities to exploit historic CVEs in components where newer versions are not vulnerable. Anti-rollback enforcement ensures that the security improvement trajectory of patch deployment cannot be reversed. Legitimate downgrade requirements should be addressed through vendor support rather than disabling rollback controls.",
                Tags = ["integrity", "rollback", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableVersionRollback", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableVersionRollback")],
                DetectOps = [RegOp.CheckDword(Key, "DisableVersionRollback", 1)],
            },
            new TweakDef
            {
                Id = "dataintg-enable-hash-validation",
                Label = "Enable File Hash Validation Before Execution",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                Description =
                    "File hash validation computes and verifies the cryptographic hash of executable files against a known-good baseline before allowing execution. Enabling hash validation before execution creates a substantial barrier against execution of unauthorized or modified binaries. Changed file hashes indicate either unauthorized modification or dynamic loading of code not present in the original deployment. Hash validation adds execution latency proportional to file sizes and the number of files validated on each invocation. Enterprise environments maintaining a strict software allowlist can use hash validation to enforce the approved software inventory. Combining hash validation with code signing creates defense-in-depth for executable integrity assurance.",
                Tags = ["integrity", "hash", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableHashValidation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableHashValidation")],
                DetectOps = [RegOp.CheckDword(Key, "EnableHashValidation", 1)],
            },
            new TweakDef
            {
                Id = "dataintg-disable-telemetry",
                Label = "Disable Data Integrity Telemetry",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Data integrity telemetry sends statistics about integrity check results, verification performance, and violation frequency to Microsoft. This data helps improve the data integrity infrastructure and identify systematic issues in the Windows ecosystem. Disabling integrity telemetry prevents check result data from being sent to Microsoft's analytics pipeline. Integrity check data revealing verification failures could expose sensitive information about the security state of enterprise systems. Security posture information should be shared through enterprise vulnerability management programs not consumer telemetry. Data integrity verification and enforcement continue to function normally with telemetry disabled.",
                Tags = ["integrity", "telemetry", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableIntegrityTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableIntegrityTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "DisableIntegrityTelemetry", 1)],
            },
            new TweakDef
            {
                Id = "dataintg-enforce-on-write",
                Label = "Enforce Data Integrity on File Write Operations",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                Description =
                    "Write-time integrity enforcement validates that files being written to protected locations meet integrity policy requirements before the write is committed. Enforcing integrity at write time prevents unauthorized modification of protected files from being persisted to disk. Post-write integrity scanning can miss write operations that occur between scan intervals allowing tampered files to remain. Write-time enforcement closes this window by blocking modification at write time rather than detecting it after the fact. Performance-sensitive workloads may experience write throughput reduction when write-time integrity checks are active. Critical system and security component directories are the highest priority targets for write-time integrity enforcement.",
                Tags = ["integrity", "write-protection", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceIntegrityOnWrite", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceIntegrityOnWrite")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceIntegrityOnWrite", 1)],
            },
        ];
    
    }

    // ── FeedbackPolicy ──
    private static class _FeedbackPolicy
    {    
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Feedback";
    
        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "fbk-disable-feedback-notifications",
                Label = "Disable Feedback Notifications",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
                Description =
                    "Sets DisableFeedbackNotifications=1 in the Feedback policy key. Prevents "
                    + "Windows from displaying in-product popups asking users to rate their "
                    + "experience, review new features, or complete satisfaction surveys. "
                    + "Feedback prompts appear on lock screens, Start, and Settings and "
                    + "are disruptive on shared kiosk or call-centre machines. "
                    + "Default: 0. Recommended: 1 on enterprise or kiosk deployments.",
                Tags = ["feedback", "notifications", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableFeedbackNotifications", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableFeedbackNotifications")],
                DetectOps = [RegOp.CheckDword(Key, "DisableFeedbackNotifications", 1)],
            },
            new TweakDef
            {
                Id = "fbk-disable-feedback-hub",
                Label = "Disable Feedback Hub",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
                Description =
                    "Sets DisableFeedbackHub=1 in the Feedback policy key. Blocks the "
                    + "Feedback Hub application from submitting problem reports, feature "
                    + "requests, or screenshots to Microsoft's developer pipeline. Feedback "
                    + "Hub submissions include attached device diagnostics and may capture "
                    + "window contents and audio, raising privacy concerns in regulated "
                    + "environments. Default: 0. Recommended: 1.",
                Tags = ["feedback", "hub", "privacy", "telemetry", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableFeedbackHub", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableFeedbackHub")],
                DetectOps = [RegOp.CheckDword(Key, "DisableFeedbackHub", 1)],
            },
            new TweakDef
            {
                Id = "fbk-disable-feedback-hub-nps",
                Label = "Disable Feedback Hub NPS Surveys",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
                Description =
                    "Sets DisableNpsSurveys=1 in the Feedback policy key. Suppresses "
                    + "Net Promoter Score (NPS) survey overlays that ask how likely the "
                    + "user is to recommend Windows. NPS surveys are triggered "
                    + "automatically after usage milestones and appear over full-screen "
                    + "applications, causing unexpected context loss and accessibility "
                    + "issues on touchscreen devices. Default: 0.",
                Tags = ["feedback", "nps", "survey", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableNpsSurveys", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableNpsSurveys")],
                DetectOps = [RegOp.CheckDword(Key, "DisableNpsSurveys", 1)],
            },
            new TweakDef
            {
                Id = "fbk-disable-feedback-telemetry-upload",
                Label = "Disable Feedback Telemetry Upload",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true, CorpSafe = true, ImpactScore = 4, SafetyRating = 5,
                Description =
                    "Sets DisableTelemetryUpload=1 in the Feedback policy key. Explicitly "
                    + "blocks the Feedback Hub service from uploading telemetry bundles "
                    + "that contain app crash data, device configuration snapshots, and "
                    + "event traces to Microsoft's backend. This is complementary to the "
                    + "global telemetry policy and specifically targets the Feedback Hub's "
                    + "own upload queue. Default: 0. Recommended: 1.",
                Tags = ["feedback", "telemetry", "upload", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableTelemetryUpload", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableTelemetryUpload")],
                DetectOps = [RegOp.CheckDword(Key, "DisableTelemetryUpload", 1)],
            },
            new TweakDef
            {
                Id = "fbk-disable-screen-capture-feedback",
                Label = "Disable Feedback Screen Capture",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true, CorpSafe = true, ImpactScore = 4, SafetyRating = 5,
                Description =
                    "Sets DisableScreenCapture=1 in the Feedback policy key. Prevents the "
                    + "Feedback Hub from automatically capturing a screenshot when the "
                    + "user initiates a problem report. Automatic screen capture collects "
                    + "potentially sensitive data visible on the screen at the moment of "
                    + "capture, including text, passwords, and documents. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["feedback", "screenshot", "screen-capture", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableScreenCapture", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableScreenCapture")],
                DetectOps = [RegOp.CheckDword(Key, "DisableScreenCapture", 1)],
            },
            new TweakDef
            {
                Id = "fbk-disable-recording-feedback",
                Label = "Disable Feedback Steps Recorder",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
                Description =
                    "Sets DisableStepsRecorder=1 in the Feedback policy key. Blocks the "
                    + "Steps Recorder component of the Feedback Hub from recording a "
                    + "sequence of UI interactions and screenshots to attach to a problem "
                    + "report. Steps Recorder captures all input events and window "
                    + "contents for the recording duration, representing a significant "
                    + "privacy and data-leakage risk on shared machines. Default: 0.",
                Tags = ["feedback", "steps-recorder", "recording", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableStepsRecorder", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableStepsRecorder")],
                DetectOps = [RegOp.CheckDword(Key, "DisableStepsRecorder", 1)],
            },
            new TweakDef
            {
                Id = "fbk-disable-feedback-app-prompts",
                Label = "Disable In-App Feedback Prompts",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
                Description =
                    "Sets DisableInAppFeedbackPrompts=1 in the Feedback policy key. Prevents "
                    + "Windows-inbox applications and UWP system apps from showing inline "
                    + "'Send feedback' or thumbs-up/down rating controls in their UI. These "
                    + "prompts appear in Calendar, Mail, Weather, and Edge and consume "
                    + "reachable screen space on constrained display sizes and kiosk shells. "
                    + "Default: 0. Recommended: 1 on locked-down deployments.",
                Tags = ["feedback", "in-app", "prompts", "ux", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableInAppFeedbackPrompts", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableInAppFeedbackPrompts")],
                DetectOps = [RegOp.CheckDword(Key, "DisableInAppFeedbackPrompts", 1)],
            },
            new TweakDef
            {
                Id = "fbk-set-feedback-frequency-never",
                Label = "Set Feedback Frequency to Never",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
                Description =
                    "Sets FeedbackFrequency=0 in the Feedback policy key. Sets the periodic "
                    + "feedback prompt interval to 0 (Never), so Windows never schedules "
                    + "a feedback dialog based on elapsed time or usage events. The "
                    + "FeedbackFrequency policy values are 0=Never, 1=Always, 2=Once. "
                    + "Setting this to Never is the most restrictive option and aligns with "
                    + "an enterprise no-feedback posture. Default: 1.",
                Tags = ["feedback", "frequency", "never", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "FeedbackFrequency", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "FeedbackFrequency")],
                DetectOps = [RegOp.CheckDword(Key, "FeedbackFrequency", 0)],
            },
            new TweakDef
            {
                Id = "fbk-disable-voluntary-data-collection",
                Label = "Disable Voluntary Data Collection via Feedback",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
                Description =
                    "Sets DisableVoluntaryDataCollection=1 in the Feedback policy key. Opts "
                    + "the device out of voluntary data collection programmes (such as the "
                    + "Customer Experience Improvement Program) that are triggered through "
                    + "Feedback Hub consent dialogs. Consent obtained via feedback prompts "
                    + "may be granted by users without full understanding of what diagnostic "
                    + "data is collected. Default: 0. Recommended: 1.",
                Tags = ["feedback", "ceip", "data-collection", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableVoluntaryDataCollection", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableVoluntaryDataCollection")],
                DetectOps = [RegOp.CheckDword(Key, "DisableVoluntaryDataCollection", 1)],
            },
            new TweakDef
            {
                Id = "fbk-disable-feedback-account-requirement",
                Label = "Disable Feedback Account Sign-In Requirement",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
                Description =
                    "Sets DisableAccountRequirement=1 in the Feedback policy key. Prevents "
                    + "the Feedback Hub from prompting users to sign in to a Microsoft "
                    + "Account as a prerequisite for submitting feedback. Removing the "
                    + "sign-in requirement limits correlation of feedback submissions with "
                    + "individual user identities in Microsoft's backend systems. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["feedback", "account", "msa", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAccountRequirement", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAccountRequirement")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAccountRequirement", 1)],
            },
        ];
    
    }

    // ── LocationSensors ──
    private static class _LocationSensors
    {    
        private const string LocPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LocationAndSensors";
        private const string AppPrivacy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy";
        private const string WinSearch = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search";
        private const string WifiConfig = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\WcmSvc\wifinetworkmanager\config";
        private const string UserLocation = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\location";
        private const string UserActivity = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\activity";
    
        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "loc-disable-location-scripting",
                Label = "Disable Location Scripting",
                Category = "Privacy & Telemetry Policy",
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
                Category = "Privacy & Telemetry Policy",
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
                Category = "Privacy & Telemetry Policy",
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
                Category = "Privacy & Telemetry Policy",
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
                Category = "Privacy & Telemetry Policy",
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
                Category = "Privacy & Telemetry Policy",
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
                Category = "Privacy & Telemetry Policy",
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
                Category = "Privacy & Telemetry Policy",
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
                Category = "Privacy & Telemetry Policy",
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
                Category = "Privacy & Telemetry Policy",
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

    // ── LocationSensorsPolicy ──
    private static class _LocationSensorsPolicy
    {    
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LocationAndSensors";
    
        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "locsns-disable-location",
                    Label = "Disable All Location Services",
                    Category = "Privacy & Telemetry Policy",
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
                    Category = "Privacy & Telemetry Policy",
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
                    Category = "Privacy & Telemetry Policy",
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
                    Category = "Privacy & Telemetry Policy",
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
                    Category = "Privacy & Telemetry Policy",
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
                    Category = "Privacy & Telemetry Policy",
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
                    Category = "Privacy & Telemetry Policy",
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
                    Category = "Privacy & Telemetry Policy",
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
                    Category = "Privacy & Telemetry Policy",
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
                    Category = "Privacy & Telemetry Policy",
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

    // ── MicrosoftAccount ──
    private static class _MicrosoftAccount
    {    
        private const string MsaPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PolicyManager\current\device\Accounts";
    
        private const string WinLogon = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon";
    
        private const string SyncPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SettingSync";
    
        private const string PassportPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PassportForWork";
    
        private const string SignIn = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System";
    
        private const string MsaUserPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Cloud";
    
        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "msa-disable-account-sync",
                Label = "Disable Microsoft Account Settings Sync",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true,
                CorpSafe = false,
                ImpactScore = 3,
                SafetyRating = 4,
                Tags = ["msa", "sync", "settings", "privacy"],
                Description =
                    "Disables the Sync Your Settings feature (DisableSettingSync=2). "
                    + "Prevents Windows settings like theme, passwords, and language preferences "
                    + "from being uploaded to and synced through your Microsoft Account.",
                ApplyOps = [RegOp.SetDword(SyncPolicy, "DisableSettingSync", 2)],
                RemoveOps = [RegOp.DeleteValue(SyncPolicy, "DisableSettingSync")],
                DetectOps = [RegOp.CheckDword(SyncPolicy, "DisableSettingSync", 2)],
            },
            new TweakDef
            {
                Id = "msa-disable-sync-override",
                Label = "Prevent Users from Overriding Settings Sync Policy",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true,
                CorpSafe = false,
                ImpactScore = 3,
                SafetyRating = 4,
                Tags = ["msa", "sync", "policy", "lock"],
                Description =
                    "Sets DisableSettingSyncUserOverride=1 to prevent users from re-enabling "
                    + "Settings Sync through the Settings app. Complements DisableSettingSync.",
                ApplyOps = [RegOp.SetDword(SyncPolicy, "DisableSettingSyncUserOverride", 1)],
                RemoveOps = [RegOp.DeleteValue(SyncPolicy, "DisableSettingSyncUserOverride")],
                DetectOps = [RegOp.CheckDword(SyncPolicy, "DisableSettingSyncUserOverride", 1)],
            },
            new TweakDef
            {
                Id = "msa-block-msa-signin",
                Label = "Block Microsoft Account Sign-In for Apps",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                Tags = ["msa", "sign-in", "block", "policy"],
                Description =
                    "Blocks apps and services from using Microsoft Account for authentication "
                    + "(NoConnectedUser=3). Suitable for managed corporate environments where "
                    + "all identity is managed by Azure AD or on-premises AD. "
                    + "WARNING: UWP app sign-in and Xbox will break.",
                ApplyOps = [RegOp.SetDword(SignIn, "NoConnectedUser", 3)],
                RemoveOps = [RegOp.DeleteValue(SignIn, "NoConnectedUser")],
                DetectOps = [RegOp.CheckDword(SignIn, "NoConnectedUser", 3)],
            },
            new TweakDef
            {
                Id = "msa-disable-windows-hello-provision",
                Label = "Disable Windows Hello Provisioning (Local Account)",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Tags = ["msa", "windows hello", "provisioning", "pin", "policy"],
                Description =
                    "Prevents Windows from requiring and prompting users to set up a Windows "
                    + "Hello PIN or biometric. Enabled=0 disables the PassportForWork policy. "
                    + "Suitable for shared PCs or environments using passwords only.",
                ApplyOps = [RegOp.SetDword(PassportPolicy, "Enabled", 0)],
                RemoveOps = [RegOp.DeleteValue(PassportPolicy, "Enabled")],
                DetectOps = [RegOp.CheckDword(PassportPolicy, "Enabled", 0)],
            },
            new TweakDef
            {
                Id = "msa-disable-optional-diagnostic-sync",
                Label = "Disable Optional Diagnostic Data Sync via MSA",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true,
                CorpSafe = false,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["msa", "diagnostic", "sync", "privacy", "telemetry"],
                Description =
                    "Disables the optional background upload of diagnostic/telemetry data "
                    + "associated with a signed-in Microsoft Account. Complements the main "
                    + "telemetry disable tweak with an MSA-scoped opt-out.",
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "AllowDeviceNameInTelemetry", 0)],
                RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "AllowDeviceNameInTelemetry")],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "AllowDeviceNameInTelemetry", 0)],
            },
            new TweakDef
            {
                Id = "msa-disable-app-access-account-info",
                Label = "Disable App Access to Account Information (Privacy)",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["msa", "account info", "privacy", "app access"],
                Description =
                    "Revokes app permission to read your account info (name, picture, username). "
                    + "Prevents UWP apps from accessing account details without explicit consent. "
                    + "UserConsent=Deny via the privacy capability setting.",
                ApplyOps =
                [
                    RegOp.SetString(
                        @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\DeviceAccess\Global\{C1D23ACC-752B-43E5-8448-8D0E519CD6D6}",
                        "Value",
                        "Deny"
                    ),
                ],
                RemoveOps =
                [
                    RegOp.SetString(
                        @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\DeviceAccess\Global\{C1D23ACC-752B-43E5-8448-8D0E519CD6D6}",
                        "Value",
                        "Allow"
                    ),
                ],
                DetectOps =
                [
                    RegOp.CheckString(
                        @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\DeviceAccess\Global\{C1D23ACC-752B-43E5-8448-8D0E519CD6D6}",
                        "Value",
                        "Deny"
                    ),
                ],
            },
            new TweakDef
            {
                Id = "msa-disable-suggested-apps-msa",
                Label = "Disable MSA-Based Suggested Apps and Content",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["msa", "suggested apps", "ads", "privacy"],
                Description =
                    "Disables personalised app and content suggestions delivered through "
                    + "the Microsoft Account integration (CloudContent\\DisableSoftLanding=1). "
                    + "Stops account-based promotional content in the Start menu and apps.",
                ApplyOps =
                [
                    RegOp.SetDword(
                        @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                        "SilentInstalledAppsEnabled",
                        0
                    ),
                ],
                RemoveOps =
                [
                    RegOp.SetDword(
                        @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                        "SilentInstalledAppsEnabled",
                        1
                    ),
                ],
                DetectOps =
                [
                    RegOp.CheckDword(
                        @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                        "SilentInstalledAppsEnabled",
                        0
                    ),
                ],
            },
            new TweakDef
            {
                Id = "msa-disable-theme-sync",
                Label = "Disable Theme Sync via Microsoft Account",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Tags = ["msa", "theme", "sync", "personalization"],
                Description =
                    "Stops Windows from syncing the desktop theme (wallpaper, colours, sounds) "
                    + "across devices linked to the same Microsoft Account. "
                    + "DisableThemeSettingSync=1.",
                ApplyOps = [RegOp.SetDword(SyncPolicy, "DisableThemeSettingSync", 1)],
                RemoveOps = [RegOp.DeleteValue(SyncPolicy, "DisableThemeSettingSync")],
                DetectOps = [RegOp.CheckDword(SyncPolicy, "DisableThemeSettingSync", 1)],
            },
            new TweakDef
            {
                Id = "msa-disable-password-sync",
                Label = "Disable Password Sync via Microsoft Account",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Tags = ["msa", "password", "sync", "credential", "security"],
                Description =
                    "Disables syncing of saved app and website passwords through the "
                    + "Microsoft Account sync channel. Prevents credentials from being "
                    + "transmitted to Microsoft servers. DisableCredentialsSettingSync=1.",
                ApplyOps = [RegOp.SetDword(SyncPolicy, "DisableCredentialsSettingSync", 1)],
                RemoveOps = [RegOp.DeleteValue(SyncPolicy, "DisableCredentialsSettingSync")],
                DetectOps = [RegOp.CheckDword(SyncPolicy, "DisableCredentialsSettingSync", 1)],
            },
            new TweakDef
            {
                Id = "msa-disable-app-settings-sync",
                Label = "Disable Per-App Settings Sync via Microsoft Account",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["msa", "app settings", "sync", "store apps"],
                Description =
                    "Disables per-app settings sync so individual app configurations (like "
                    + "UWP app preferences) are not uploaded and synced through the Microsoft "
                    + "Account. DisableApplicationSettingSync=1.",
                ApplyOps = [RegOp.SetDword(SyncPolicy, "DisableApplicationSettingSync", 1)],
                RemoveOps = [RegOp.DeleteValue(SyncPolicy, "DisableApplicationSettingSync")],
                DetectOps = [RegOp.CheckDword(SyncPolicy, "DisableApplicationSettingSync", 1)],
            },
        ];
    
    }

    // ── PhotoAcquisitionPolicy ──
    private static class _PhotoAcquisitionPolicy
    {    
        private const string PaLm = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PhotoAcquire";
        private const string PaCu = @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\PhotoAcquire";
        private const string DevMetaKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceMetadata";
    
        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "photo-disable-acquire-wizard",
                Label = "Disable Photo Acquisition Wizard",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Sets DisableAutoPlayForCamera=1 in the machine Photo Acquire policy key. "
                    + "Prevents the Windows Photo Acquisition Wizard from launching automatically when a camera "
                    + "or memory card is connected. Avoids the 'What do you want to do?' photo import prompt. "
                    + "Default: absent (wizard auto-launches). Recommended: 1 to reduce unwanted automatic import.",
                Tags = ["photo", "camera", "import", "wizard", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Photo import wizard no longer auto-launches on camera/memory card connect; manual import via Photos app still works.",
                ApplyOps = [RegOp.SetDword(PaLm, "DisableAutoPlayForCamera", 1)],
                RemoveOps = [RegOp.DeleteValue(PaLm, "DisableAutoPlayForCamera")],
                DetectOps = [RegOp.CheckDword(PaLm, "DisableAutoPlayForCamera", 1)],
            },
            new TweakDef
            {
                Id = "photo-disable-acquire-wizard-user",
                Label = "Disable Photo Acquisition Wizard (Current User)",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Sets DisableAutoPlayForCamera=1 in the per-user Photo Acquire policy key. "
                    + "Suppresses the photo import prompt for the current user when a camera is connected, "
                    + "without applying the restriction machine-wide. "
                    + "Default: absent. Recommended: 1 on user profiles where automated photo import is undesired.",
                Tags = ["photo", "camera", "import", "user", "policy"],
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Photo import wizard suppressed for this user only.",
                ApplyOps = [RegOp.SetDword(PaCu, "DisableAutoPlayForCamera", 1)],
                RemoveOps = [RegOp.DeleteValue(PaCu, "DisableAutoPlayForCamera")],
                DetectOps = [RegOp.CheckDword(PaCu, "DisableAutoPlayForCamera", 1)],
            },
            new TweakDef
            {
                Id = "photo-disable-delete-after-import",
                Label = "Prevent Photo Deletion After Import",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Sets NeverDeleteOriginalFiles=1 in the machine Photo Acquire policy key. "
                    + "Prevents the Photo Acquisition Wizard from offering or performing deletion of photos "
                    + "from the source camera/card after importing them to the PC. "
                    + "Default: absent (deletion allowed). Recommended: 1 to protect source media from accidental deletion.",
                Tags = ["photo", "import", "delete", "protection", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Source photos never deleted after import; the wizard's 'Delete from device' option is disabled.",
                ApplyOps = [RegOp.SetDword(PaLm, "NeverDeleteOriginalFiles", 1)],
                RemoveOps = [RegOp.DeleteValue(PaLm, "NeverDeleteOriginalFiles")],
                DetectOps = [RegOp.CheckDword(PaLm, "NeverDeleteOriginalFiles", 1)],
            },
            new TweakDef
            {
                Id = "photo-disable-tag-on-import",
                Label = "Disable Automatic Tagging During Photo Import",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Sets DisableTaggingOnAcquire=1 in the machine Photo Acquire policy key. "
                    + "Stops the Photo Acquisition Wizard from automatically adding metadata tags to photos "
                    + "during the import process. Useful when tagging must be done by a specific application. "
                    + "Default: absent (tagging allowed). Recommended: 1 when a DMS or DAM system handles metadata.",
                Tags = ["photo", "import", "tagging", "metadata", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Photos imported without auto-applied tags; metadata management left to the user or DMS.",
                ApplyOps = [RegOp.SetDword(PaLm, "DisableTaggingOnAcquire", 1)],
                RemoveOps = [RegOp.DeleteValue(PaLm, "DisableTaggingOnAcquire")],
                DetectOps = [RegOp.CheckDword(PaLm, "DisableTaggingOnAcquire", 1)],
            },
            new TweakDef
            {
                Id = "photo-disable-rotate-on-import",
                Label = "Disable Auto-Rotate During Photo Import",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Sets DisableRotateOnAcquire=1 in the machine Photo Acquire policy key. "
                    + "Prevents the acquisition wizard from auto-rotating photos based on EXIF orientation metadata. "
                    + "Useful when images must be preserved in their original orientation for processing pipelines. "
                    + "Default: absent (auto-rotate enabled). Recommended: 1 for raw capture workflows.",
                Tags = ["photo", "import", "rotate", "exif", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "EXIF auto-rotation skipped during import; images stored in their raw capture orientation.",
                ApplyOps = [RegOp.SetDword(PaLm, "DisableRotateOnAcquire", 1)],
                RemoveOps = [RegOp.DeleteValue(PaLm, "DisableRotateOnAcquire")],
                DetectOps = [RegOp.CheckDword(PaLm, "DisableRotateOnAcquire", 1)],
            },
            new TweakDef
            {
                Id = "photo-disable-title-on-import",
                Label = "Disable Title Prompt During Photo Import",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Sets DisableTitleOnAcquire=1 in the machine Photo Acquire policy key. "
                    + "Skips the title/description prompt during the photo acquisition wizard. "
                    + "Useful in automated or batch import scenarios where manual metadata entry is not desired. "
                    + "Default: absent (title prompt shown). Recommended: 1 in automated import pipelines.",
                Tags = ["photo", "import", "title", "metadata", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Title/description prompt skipped during photo import; wizard completes without metadata entry.",
                ApplyOps = [RegOp.SetDword(PaLm, "DisableTitleOnAcquire", 1)],
                RemoveOps = [RegOp.DeleteValue(PaLm, "DisableTitleOnAcquire")],
                DetectOps = [RegOp.CheckDword(PaLm, "DisableTitleOnAcquire", 1)],
            },
            new TweakDef
            {
                Id = "photo-disable-open-explorer-after",
                Label = "Disable 'Open Folder' After Photo Import",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Sets DisableOpenFilesystemAfterAcquire=1 in the machine Photo Acquire policy key. "
                    + "Prevents the Photo Acquisition Wizard from automatically opening the destination folder "
                    + "in Windows Explorer after importing photos. Reduces unnecessary window creation in workflows. "
                    + "Default: absent (folder opens after import). Recommended: 1 in scripted/automated deployments.",
                Tags = ["photo", "import", "explorer", "post-import", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Explorer does not open the import destination after the wizard completes; silent import.",
                ApplyOps = [RegOp.SetDword(PaLm, "DisableOpenFilesystemAfterAcquire", 1)],
                RemoveOps = [RegOp.DeleteValue(PaLm, "DisableOpenFilesystemAfterAcquire")],
                DetectOps = [RegOp.CheckDword(PaLm, "DisableOpenFilesystemAfterAcquire", 1)],
            },
            new TweakDef
            {
                Id = "photo-disable-wia-device-install",
                Label = "Disable WIA Device Metadata Internet Download",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Sets PreventDeviceMetadataFromNetwork=1 in the Device Metadata policy key. "
                    + "Prevents Windows from downloading WIA (Windows Image Acquisition) device metadata "
                    + "and drivers from the internet for cameras, scanners, and photo devices. "
                    + "Default: absent (online download allowed). Recommended: 1 on air-gapped or bandwidth-limited systems.",
                Tags = ["photo", "wia", "device-metadata", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "WIA photo device metadata not downloaded from the internet; local or WSUS-served drivers only.",
                ApplyOps = [RegOp.SetDword(DevMetaKey, "PreventDeviceMetadataFromNetwork", 1)],
                RemoveOps = [RegOp.DeleteValue(DevMetaKey, "PreventDeviceMetadataFromNetwork")],
                DetectOps = [RegOp.CheckDword(DevMetaKey, "PreventDeviceMetadataFromNetwork", 1)],
            },
            new TweakDef
            {
                Id = "photo-disable-scanner-events",
                Label = "Disable WIA Scanner Device Events",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Sets DisableScannerEvents=1 in the machine Photo Acquire policy key. "
                    + "Suppresses WIA scanner events that trigger the photo acquisition wizard or image scanning dialogs "
                    + "when a scanner button is pressed or paper is inserted, preventing interruptions. "
                    + "Default: absent (scanner events enabled). Recommended: 1 on machines without attached scanners.",
                Tags = ["photo", "scanner", "wia", "events", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Scanner hardware events suppressed; scanner still manually operable via its application.",
                ApplyOps = [RegOp.SetDword(PaLm, "DisableScannerEvents", 1)],
                RemoveOps = [RegOp.DeleteValue(PaLm, "DisableScannerEvents")],
                DetectOps = [RegOp.CheckDword(PaLm, "DisableScannerEvents", 1)],
            },
            new TweakDef
            {
                Id = "photo-disable-camera-events",
                Label = "Disable WIA Camera Device Events",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Sets DisableCameraEvents=1 in the machine Photo Acquire policy key. "
                    + "Suppresses WIA camera events that trigger the photo acquisition wizard when a digital camera "
                    + "is connected via USB and a camera-side button is pressed. Prevents unplanned import dialogs. "
                    + "Default: absent (camera events enabled). Recommended: 1 on machines without regular camera connections.",
                Tags = ["photo", "camera", "wia", "events", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "WIA camera button-press events suppressed; camera still functions normally when used manually.",
                ApplyOps = [RegOp.SetDword(PaLm, "DisableCameraEvents", 1)],
                RemoveOps = [RegOp.DeleteValue(PaLm, "DisableCameraEvents")],
                DetectOps = [RegOp.CheckDword(PaLm, "DisableCameraEvents", 1)],
            },
        ];
    
    }

    // ── PushNotificationsPolicy ──
    private static class _PushNotificationsPolicy
    {    
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\PushNotifications";
    
        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "pnp-disable-toast-notifications",
                    Label = "Disable All Toast Notifications via Policy",
                    Category = "Privacy & Telemetry Policy",
                    Description =
                        "Sets ToastEnabled=0 to block all toast (pop-up) notifications via Group Policy. "
                        + "No app toast alerts will appear on the desktop or in Action Center regardless of individual app notification settings. "
                        + "This policy-level control takes precedence over per-user and per-app notification preferences configured in Windows Settings.",
                    Tags = ["notifications", "toast", "policy", "gpo"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Silences all desktop toast alerts; users may miss time-critical application notifications.",
                    ApplyOps = [RegOp.SetDword(Key, "ToastEnabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ToastEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "ToastEnabled", 0)],
                },
                new TweakDef
                {
                    Id = "pnp-disable-lockscreen-toasts",
                    Label = "Disable Toast Notifications on Lock Screen",
                    Category = "Privacy & Telemetry Policy",
                    Description =
                        "Sets NoToastApplicationNotificationOnLockscreen=1 to prevent app toast notifications from appearing on the Windows lock screen. "
                        + "Protects notification content from physical shoulder-surfing on unattended machines in shared-space environments.",
                    Tags = ["notifications", "lock-screen", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Hides notification content on lock screen; privacy improvement for shared-space devices.",
                    ApplyOps = [RegOp.SetDword(Key, "NoToastApplicationNotificationOnLockscreen", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NoToastApplicationNotificationOnLockscreen")],
                    DetectOps = [RegOp.CheckDword(Key, "NoToastApplicationNotificationOnLockscreen", 1)],
                },
                new TweakDef
                {
                    Id = "pnp-disable-app-notifications",
                    Label = "Disable App Notifications via Group Policy",
                    Category = "Privacy & Telemetry Policy",
                    Description =
                        "Sets NoToastApplicationNotification=1 to block all application-level toast notifications via Group Policy. "
                        + "This machine-wide policy prevents individual users from re-enabling per-app notifications in Windows Settings, "
                        + "ensuring consistent notification suppression across all user accounts on the machine.",
                    Tags = ["notifications", "apps", "policy", "gpo"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Enforces notification silence across all apps; overrides per-app user notification settings.",
                    ApplyOps = [RegOp.SetDword(Key, "NoToastApplicationNotification", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NoToastApplicationNotification")],
                    DetectOps = [RegOp.CheckDword(Key, "NoToastApplicationNotification", 1)],
                },
                new TweakDef
                {
                    Id = "pnp-disable-cloud-notifications",
                    Label = "Disable Cloud-Sourced Notifications",
                    Category = "Privacy & Telemetry Policy",
                    Description =
                        "Sets NoCloudApplicationNotification=1 to prevent Windows from delivering notifications that originate from cloud services "
                        + "via Windows Push Notification Services (WNS). Reduces notification-related network traffic and prevents cloud-sourced "
                        + "promotional content from being delivered to the Action Center.",
                    Tags = ["notifications", "cloud", "wns", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Blocks cloud-pushed alerts; may suppress Microsoft 365 renewal nudges and Store update notifications.",
                    ApplyOps = [RegOp.SetDword(Key, "NoCloudApplicationNotification", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NoCloudApplicationNotification")],
                    DetectOps = [RegOp.CheckDword(Key, "NoCloudApplicationNotification", 1)],
                },
                new TweakDef
                {
                    Id = "pnp-disable-tile-notifications",
                    Label = "Disable Live Tile Notifications",
                    Category = "Privacy & Telemetry Policy",
                    Description =
                        "Sets NoTileApplicationNotification=1 to prevent apps from updating live tile badges and content on the Start menu. "
                        + "Eliminates background polling by Start tile update engines, reduces network usage from tile refresh requests, "
                        + "and removes animated content from the Start menu.",
                    Tags = ["notifications", "live-tiles", "start-menu", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Freezes Start menu tile content; cosmetic change with minor background bandwidth savings.",
                    ApplyOps = [RegOp.SetDword(Key, "NoTileApplicationNotification", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NoTileApplicationNotification")],
                    DetectOps = [RegOp.CheckDword(Key, "NoTileApplicationNotification", 1)],
                },
                new TweakDef
                {
                    Id = "pnp-disable-notification-mirroring",
                    Label = "Disable Notification Mirroring to Linked Devices",
                    Category = "Privacy & Telemetry Policy",
                    Description =
                        "Sets DisallowNotificationMirroring=1 to prevent Windows from forwarding notification content to linked devices "
                        + "via Bluetooth pairing or the Phone Link (Your Phone) application. Mirrored notifications bypass per-app "
                        + "notification settings on the receiving device and may expose sensitive notification content to paired hardware.",
                    Tags = ["notifications", "mirroring", "bluetooth", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Stops notification mirroring to paired phones/devices; local notifications unaffected.",
                    ApplyOps = [RegOp.SetDword(Key, "DisallowNotificationMirroring", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisallowNotificationMirroring")],
                    DetectOps = [RegOp.CheckDword(Key, "DisallowNotificationMirroring", 1)],
                },
                new TweakDef
                {
                    Id = "pnp-disable-cloud-toast-notifications",
                    Label = "Disable Cloud Toast Notification Delivery",
                    Category = "Privacy & Telemetry Policy",
                    Description =
                        "Sets DisallowCloudToastNotification=1 to block toast notifications delivered through the Windows Push Notification "
                        + "Services (WNS) cloud infrastructure. Prevents the WNS channel from delivering push content from app backends, "
                        + "closing the WNS delivery path independently of the local toast-enabled setting.",
                    Tags = ["notifications", "wns", "cloud", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Disables WNS push channel; push-enabled apps lose real-time alert capability.",
                    ApplyOps = [RegOp.SetDword(Key, "DisallowCloudToastNotification", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisallowCloudToastNotification")],
                    DetectOps = [RegOp.CheckDword(Key, "DisallowCloudToastNotification", 1)],
                },
                new TweakDef
                {
                    Id = "pnp-disable-badge-on-lockscreen",
                    Label = "Disable App Badge Counters on Lock Screen",
                    Category = "Privacy & Telemetry Policy",
                    Description =
                        "Sets NoLockScreenApplicationBadge=1 to prevent app badge counters from appearing on the Windows lock screen. "
                        + "Badge numbers (unread email count, missed calls, calendar items) are hidden, reducing information leakage "
                        + "on unattended endpoints that may be visible to passers-by.",
                    Tags = ["notifications", "badge", "lock-screen", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Hides app badge counters on lock screen; minor privacy improvement with no functional impact.",
                    ApplyOps = [RegOp.SetDword(Key, "NoLockScreenApplicationBadge", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NoLockScreenApplicationBadge")],
                    DetectOps = [RegOp.CheckDword(Key, "NoLockScreenApplicationBadge", 1)],
                },
                new TweakDef
                {
                    Id = "pnp-disable-user-notification-changes",
                    Label = "Prevent User Changes to Notification Settings",
                    Category = "Privacy & Telemetry Policy",
                    Description =
                        "Sets DisallowUserChanges=1 to prevent end-users from modifying notification priorities and quiet-hours settings "
                        + "in the Windows Settings app. Policy-enforced notification settings cannot be overridden per-user when this "
                        + "value is set, ensuring consistent notification governance across all accounts on the endpoint.",
                    Tags = ["notifications", "quiet-hours", "restriction", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ImpactNote = "Locks the notification settings page; users cannot re-enable notifications suppressed by policy.",
                    ApplyOps = [RegOp.SetDword(Key, "DisallowUserChanges", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisallowUserChanges")],
                    DetectOps = [RegOp.CheckDword(Key, "DisallowUserChanges", 1)],
                },
                new TweakDef
                {
                    Id = "pnp-restrict-push-notification-network",
                    Label = "Block Push Notification Network Access",
                    Category = "Privacy & Telemetry Policy",
                    Description =
                        "Sets NoNetworkNotification=1 to prevent Windows from delivering network-status notifications (connection changes, "
                        + "captive portal prompts, VPN status). Reduces notification noise in environments with frequent network transitions "
                        + "and prevents the captive-portal pop-up behavior on corporate or hotel networks.",
                    Tags = ["notifications", "network", "captive-portal", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ImpactNote = "Suppresses network-change toast alerts; captive portal sign-in prompts will not appear automatically.",
                    ApplyOps = [RegOp.SetDword(Key, "NoNetworkNotification", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NoNetworkNotification")],
                    DetectOps = [RegOp.CheckDword(Key, "NoNetworkNotification", 1)],
                },
            ];
    
    }

    // ── SearchWebPolicy ──
    private static class _SearchWebPolicy
    {    
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search";
    
        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "srchweb-disable-cloud-search",
                Label = "Disable Cloud-Augmented Search Results",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Prevents Windows Search from combining local results with cloud (Bing/MSN index) results. Search returns only locally-indexed content.",
                Tags = ["search", "cloud", "bing", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Removes Bing cloud augmentation from Windows Search; fully local results only.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowCloudSearch", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowCloudSearch")],
                DetectOps = [RegOp.CheckDword(Key, "AllowCloudSearch", 0)],
            },
            new TweakDef
            {
                Id = "srchweb-disable-cortana-policy",
                Label = "Disable Cortana AI Assistant (Machine Policy)",
                Category = "Privacy & Telemetry Policy",
                Description = "Machine-wide Group Policy to disable Cortana integration in Windows Search. Overrides per-user and per-app settings.",
                Tags = ["cortana", "search", "ai", "policy", "privacy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Machine-wide policy disable for Cortana AI integration.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowCortana", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowCortana")],
                DetectOps = [RegOp.CheckDword(Key, "AllowCortana", 0)],
            },
            new TweakDef
            {
                Id = "srchweb-disable-cortana-above-lock",
                Label = "Disable Cortana on Lock Screen",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Prevents Cortana from responding to voice or text queries when the device is locked. Blocks unauthenticated access to Cortana assistant features.",
                Tags = ["cortana", "lock-screen", "policy", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents unauthenticated access to Cortana from locked state.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowCortanaAboveLock", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowCortanaAboveLock")],
                DetectOps = [RegOp.CheckDword(Key, "AllowCortanaAboveLock", 0)],
            },
            new TweakDef
            {
                Id = "srchweb-disable-web-results",
                Label = "Disable Web Results in Windows Search",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Prevents Windows Search from including Bing web results alongside local search results. Search is limited to locally-indexed files and apps.",
                Tags = ["search", "web", "bing", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Removes all Bing web results from Windows Search; local results only.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ConnectedSearchUseWeb", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "ConnectedSearchUseWeb")],
                DetectOps = [RegOp.CheckDword(Key, "ConnectedSearchUseWeb", 0)],
            },
            new TweakDef
            {
                Id = "srchweb-disable-web-over-metered",
                Label = "Disable Web Search over Metered Connections",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Blocks web-augmented search results when the device is connected via a metered network. Reduces unexpected data usage during web search on limited plans.",
                Tags = ["search", "metered", "data", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Blocks Bing web search over metered connections.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ConnectedSearchUseWebOverMeteredConnections", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "ConnectedSearchUseWebOverMeteredConnections")],
                DetectOps = [RegOp.CheckDword(Key, "ConnectedSearchUseWebOverMeteredConnections", 0)],
            },
            new TweakDef
            {
                Id = "srchweb-disable-search-location",
                Label = "Disable Location Access in Windows Search",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Prevents Windows Search from using the device's current location to improve local search results. Removes a location-data disclosure path.",
                Tags = ["search", "location", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents location data from being used to augment search results.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowSearchToUseLocation", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowSearchToUseLocation")],
                DetectOps = [RegOp.CheckDword(Key, "AllowSearchToUseLocation", 0)],
            },
            new TweakDef
            {
                Id = "srchweb-disable-bing-in-search",
                Label = "Disable Bing Web Search in Start/Taskbar",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Completely disables Bing web search from appearing in the Windows Start menu and taskbar search. All search results come from the local index only.",
                Tags = ["search", "bing", "start", "taskbar", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Completely removes Bing from Start/taskbar search.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableWebSearch", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableWebSearch")],
                DetectOps = [RegOp.CheckDword(Key, "DisableWebSearch", 1)],
            },
            new TweakDef
            {
                Id = "srchweb-enforce-safe-search",
                Label = "Enforce Strict SafeSearch for Web Results",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Forces Bing SafeSearch to Strict mode for all web search results delivered through Windows Search. Value 2 = Strict. Recommended for managed shared devices.",
                Tags = ["search", "safesearch", "policy", "content-filter"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Forces Bing SafeSearch to Strict mode for shared/managed devices.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "SafeSearchMode", 2)],
                RemoveOps = [RegOp.DeleteValue(Key, "SafeSearchMode")],
                DetectOps = [RegOp.CheckDword(Key, "SafeSearchMode", 2)],
            },
            new TweakDef
            {
                Id = "srchweb-disable-dynamic-content-wsb",
                Label = "Disable Dynamic Content in Windows Search Box",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Prevents Windows Search Box from showing dynamic highlights, trending topics, and promotional content. Keeps the search bar focused on user-typed queries.",
                Tags = ["search", "search-box", "highlights", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Removes trending topics and promotional content from the search box.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableDynamicContentInWSB", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableDynamicContentInWSB")],
                DetectOps = [RegOp.CheckDword(Key, "EnableDynamicContentInWSB", 0)],
            },
            new TweakDef
            {
                Id = "srchweb-disable-indexing-encrypted",
                Label = "Disable Indexing of Encrypted Files",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Prevents Windows Search from indexing encrypted files and stores. Reduces the risk that sensitive encrypted content is extractable via the search index.",
                Tags = ["search", "encryption", "indexing", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents encrypted file contents from appearing in the search index.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowIndexingEncryptedStoresOrItems", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowIndexingEncryptedStoresOrItems")],
                DetectOps = [RegOp.CheckDword(Key, "AllowIndexingEncryptedStoresOrItems", 0)],
            },
        ];
    
    }

    // ── SensorPolicy ──
    private static class _SensorPolicy
    {    
        private const string LocSensors = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LocationAndSensors";
    
        // CapabilityAccessManager ConsentStore — machine-wide capability deny
        private const string CamBase = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore";
    
        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "sensor-block-location-scripting",
                Label = "Block Script Access to Location Services",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["sensor", "location", "scripting", "privacy", "policy"],
                Description =
                    "Prevents scripts (Windows Script Host, PowerShell, browser scripts via MSHTML) from "
                    + "accessing the Windows Location Platform API (DisableLocationScripting=1). Scripts "
                    + "in browser controls and automation tools cannot query the device's geographic position. "
                    + "Distinct from the full location disable in Privacy.cs.",
                ApplyOps = [RegOp.SetDword(LocSensors, "DisableLocationScripting", 1)],
                RemoveOps = [RegOp.DeleteValue(LocSensors, "DisableLocationScripting")],
                DetectOps = [RegOp.CheckDword(LocSensors, "DisableLocationScripting", 1)],
            },
            new TweakDef
            {
                Id = "sensor-disable-all-sensors",
                Label = "Disable All Sensor Devices via Policy",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["sensor", "hardware", "disable", "privacy", "policy"],
                Description =
                    "Disables all sensor hardware (ambient light sensors, accelerometers, compasses, "
                    + "barometers, proximity sensors) via policy (DisableSensors=1). Applications cannot "
                    + "query sensor data. Does not affect GPS/location which is controlled separately. "
                    + "Useful for kiosk and high-security deployments.",
                ApplyOps = [RegOp.SetDword(LocSensors, "DisableSensors", 1)],
                RemoveOps = [RegOp.DeleteValue(LocSensors, "DisableSensors")],
                DetectOps = [RegOp.CheckDword(LocSensors, "DisableSensors", 1)],
            },
            new TweakDef
            {
                Id = "sensor-disable-windows-location-provider",
                Label = "Disable Windows Location Platform Provider",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["sensor", "location", "provider", "windows", "privacy"],
                Description =
                    "Disables the Windows Location Platform, which aggregates GPS, Wi-Fi triangulation, "
                    + "and IP-based positioning (DisableWindowsLocationProvider=1). Applications requesting "
                    + "location data receive no position fix even when GPS hardware is present. "
                    + "Works alongside Privacy.cs DisableLocation for defence-in-depth.",
                ApplyOps = [RegOp.SetDword(LocSensors, "DisableWindowsLocationProvider", 1)],
                RemoveOps = [RegOp.DeleteValue(LocSensors, "DisableWindowsLocationProvider")],
                DetectOps = [RegOp.CheckDword(LocSensors, "DisableWindowsLocationProvider", 1)],
            },
            new TweakDef
            {
                Id = "sensor-block-location-user-override",
                Label = "Prevent Users Re-Enabling Location Services",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["sensor", "location", "user-override", "policy", "lock"],
                Description =
                    "Prevents users from enabling Location Services in the Settings app after an admin "
                    + "policy has disabled them (DisableLocationSettingUserOverride=1). Locks the location "
                    + "setting to the machine policy value and hides the toggle in Privacy & Security → "
                    + "Location settings.",
                ApplyOps = [RegOp.SetDword(LocSensors, "DisableLocationSettingUserOverride", 1)],
                RemoveOps = [RegOp.DeleteValue(LocSensors, "DisableLocationSettingUserOverride")],
                DetectOps = [RegOp.CheckDword(LocSensors, "DisableLocationSettingUserOverride", 1)],
            },
            new TweakDef
            {
                Id = "sensor-deny-radios-capability",
                Label = "Deny App Access to Radio (Bluetooth/Wi-Fi) Controls",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["sensor", "radios", "bluetooth", "wifi", "privacy"],
                Description =
                    "Revokes app access to radio-control capability (turn Bluetooth/Wi-Fi on or off) "
                    + "for all applications via the CapabilityAccessManager ConsentStore. Apps cannot "
                    + "programmatically toggle wireless radios. Prevents rogue apps from disabling Wi-Fi "
                    + "to force expensive cellular data usage.",
                ApplyOps = [RegOp.SetString($@"{CamBase}\radios", "Value", "Deny")],
                RemoveOps = [RegOp.DeleteValue($@"{CamBase}\radios", "Value")],
                DetectOps = [RegOp.CheckString($@"{CamBase}\radios", "Value", "Deny")],
            },
            new TweakDef
            {
                Id = "sensor-deny-activity-capability",
                Label = "Deny App Access to User Activity / Fitness Data",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["sensor", "activity", "fitness", "privacy", "capability"],
                Description =
                    "Denies all apps access to activity and fitness sensor data (step counts, movement "
                    + "patterns) via ConsentStore/activity. Prevents UWP and packaged apps from reading "
                    + "physical activity data without explicit user consent enforcement.",
                ApplyOps = [RegOp.SetString($@"{CamBase}\activity", "Value", "Deny")],
                RemoveOps = [RegOp.DeleteValue($@"{CamBase}\activity", "Value")],
                DetectOps = [RegOp.CheckString($@"{CamBase}\activity", "Value", "Deny")],
            },
            new TweakDef
            {
                Id = "sensor-deny-gaze-input-capability",
                Label = "Deny App Access to Gaze / Eye-Tracking Input",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["sensor", "gaze", "eye-tracking", "input", "privacy"],
                Description =
                    "Denies all apps access to gaze input / eye-tracking hardware "
                    + "(ConsentStore/gazeInput Value=Deny). Prevents applications from tracking where a "
                    + "user is looking on screen — a high-value data source for profiling and surveillance.",
                ApplyOps = [RegOp.SetString($@"{CamBase}\gazeInput", "Value", "Deny")],
                RemoveOps = [RegOp.DeleteValue($@"{CamBase}\gazeInput", "Value")],
                DetectOps = [RegOp.CheckString($@"{CamBase}\gazeInput", "Value", "Deny")],
            },
            new TweakDef
            {
                Id = "sensor-deny-contacts-capability",
                Label = "Deny App Access to Contacts",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["sensor", "contacts", "privacy", "capability", "data"],
                Description =
                    "Denies all apps machine-wide access to the user's Contacts/People data "
                    + "(ConsentStore/contacts Value=Deny). Prevents packaged apps from reading the "
                    + "address book. Protect personally identifiable information and corporate directory "
                    + "data from apps that should not need contact access.",
                ApplyOps = [RegOp.SetString($@"{CamBase}\contacts", "Value", "Deny")],
                RemoveOps = [RegOp.DeleteValue($@"{CamBase}\contacts", "Value")],
                DetectOps = [RegOp.CheckString($@"{CamBase}\contacts", "Value", "Deny")],
            },
            new TweakDef
            {
                Id = "sensor-deny-email-capability",
                Label = "Deny App Access to Email",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["sensor", "email", "privacy", "capability", "data"],
                Description =
                    "Denies all apps machine-wide access to the user's email data "
                    + "(ConsentStore/email Value=Deny). Prevents packaged apps from reading mailbox content "
                    + "or sending email on the user's behalf. Important in environments where only "
                    + "approved email clients should have inbox access.",
                ApplyOps = [RegOp.SetString($@"{CamBase}\email", "Value", "Deny")],
                RemoveOps = [RegOp.DeleteValue($@"{CamBase}\email", "Value")],
                DetectOps = [RegOp.CheckString($@"{CamBase}\email", "Value", "Deny")],
            },
            new TweakDef
            {
                Id = "sensor-deny-bluetooth-sync-capability",
                Label = "Deny App Access to Bluetooth Sync",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                Tags = ["sensor", "bluetooth", "sync", "privacy", "capability"],
                Description =
                    "Denies all apps access to the Bluetooth synchronisation capability "
                    + "(ConsentStore/bluetoothSync Value=Deny). Prevents apps from syncing data to/from "
                    + "paired Bluetooth devices without explicit authorisation. Reduces the Bluetooth "
                    + "data-exfiltration attack surface on shared workstations.",
                ApplyOps = [RegOp.SetString($@"{CamBase}\bluetoothSync", "Value", "Deny")],
                RemoveOps = [RegOp.DeleteValue($@"{CamBase}\bluetoothSync", "Value")],
                DetectOps = [RegOp.CheckString($@"{CamBase}\bluetoothSync", "Value", "Deny")],
            },
        ];
    
    }

    // ── SensorServicePolicy ──
    private static class _SensorServicePolicy
    {    
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LocationAndSensors";
    
        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "sensor-disable-location",
                Label = "Disable Location Services",
                Category = "Privacy & Telemetry Policy",
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
                Category = "Privacy & Telemetry Policy",
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
                Category = "Privacy & Telemetry Policy",
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
                Category = "Privacy & Telemetry Policy",
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
                Category = "Privacy & Telemetry Policy",
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
                Category = "Privacy & Telemetry Policy",
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
                Category = "Privacy & Telemetry Policy",
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
                Category = "Privacy & Telemetry Policy",
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
                Category = "Privacy & Telemetry Policy",
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
                Category = "Privacy & Telemetry Policy",
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

    // ── SpellingAndTypingPolicy ──
    private static class _SpellingAndTypingPolicy
    {    
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SpellingAndTyping";
    
        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "sptype-disable-auto-correct",
                Label = "Disable Autocorrect via Policy",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
                Description =
                    "Sets AutocorrectMisspelledWords=0 in the SpellingAndTyping policy key. "
                    + "Prevents Windows from automatically replacing mis-typed words with "
                    + "spell-check suggestions via the system-wide autocorrect engine. "
                    + "Autocorrect substitutions in technical fields such as CLI terminals, "
                    + "code editors, and database query tools can silently corrupt commands "
                    + "and identifiers. Default: 1. Recommended: 0 for power users.",
                Tags = ["spelling", "autocorrect", "typing", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AutocorrectMisspelledWords", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AutocorrectMisspelledWords")],
                DetectOps = [RegOp.CheckDword(Key, "AutocorrectMisspelledWords", 0)],
            },
            new TweakDef
            {
                Id = "sptype-disable-spell-checking",
                Label = "Disable System-Wide Spell Checking",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
                Description =
                    "Sets SpellCheckingEnabled=0 in the SpellingAndTyping policy key. Disables "
                    + "the Windows spell-checking engine that underlies red-underline annotations "
                    + "in text input controls system-wide. Applications using the WinRT "
                    + "TextBox or Web-based inputs inside WebView2 share this engine. "
                    + "Programmatic text fields used by developers and technical writers "
                    + "often benefit from disabling system spell-check in favour of IDE-level checkers. "
                    + "Default: 1. Recommended: 0 on developer workstations.",
                Tags = ["spelling", "spellcheck", "typing", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "SpellCheckingEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "SpellCheckingEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "SpellCheckingEnabled", 0)],
            },
            new TweakDef
            {
                Id = "sptype-disable-text-prediction",
                Label = "Disable Text Prediction (Inline Suggestions)",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
                Description =
                    "Sets TextPredictionEnabled=0 in the SpellingAndTyping policy key. "
                    + "Removes inline word-completion suggestions that appear while typing in "
                    + "the touch keyboard, hardware keyboard (Windows 11), and select UWP text "
                    + "fields. Text prediction samples typed input stream to populate the "
                    + "suggestion bar and may transmit partial words to the cloud language "
                    + "model on some configurations. Default: 1. Recommended: 0.",
                Tags = ["text", "prediction", "suggestion", "typing", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "TextPredictionEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "TextPredictionEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "TextPredictionEnabled", 0)],
            },
            new TweakDef
            {
                Id = "sptype-disable-highlight-misspelled",
                Label = "Disable Misspelling Underline Highlight",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true, CorpSafe = true, ImpactScore = 1, SafetyRating = 5,
                Description =
                    "Sets HighlightMisspelledWords=0 in the SpellingAndTyping policy key. "
                    + "Stops the system spell engine from drawing red wavy underlines beneath "
                    + "unrecognised words in native text controls. In password fields and API "
                    + "key editors the underlines can reveal that a typed value was not "
                    + "dictionary-recognised, inadvertently confirming password contents to "
                    + "shoulder-surfers. Default: 1. Recommended: 0.",
                Tags = ["spelling", "highlight", "underline", "typing", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "HighlightMisspelledWords", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "HighlightMisspelledWords")],
                DetectOps = [RegOp.CheckDword(Key, "HighlightMisspelledWords", 0)],
            },
            new TweakDef
            {
                Id = "sptype-disable-typing-insights",
                Label = "Disable Typing Insights",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
                Description =
                    "Sets InsightsEnabled=0 in the SpellingAndTyping policy key. Disables "
                    + "the Windows Typing Insights feature that collects word-frequency data "
                    + "to personalise autocorrect, text prediction, and word suggestions over "
                    + "time. The insights database is stored per-user and contains an "
                    + "implicit record of vocabulary, project names, and identifiers typed on "
                    + "the machine. Default: 1. Recommended: 0.",
                Tags = ["typing", "insights", "personalisation", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "InsightsEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "InsightsEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "InsightsEnabled", 0)],
            },
            new TweakDef
            {
                Id = "sptype-disable-hardware-keyboard-suggestions",
                Label = "Disable Hardware Keyboard Suggestions Bar",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
                Description =
                    "Sets HardwareKeyboardTextSuggestions=0 in the SpellingAndTyping policy key. "
                    + "Removes the candidate-word suggestions bar that Windows 11 shows above "
                    + "the caret when typing with a physical keyboard in compatible applications. "
                    + "The bar requires per-keystroke processing by the suggestion engine and "
                    + "adds visual distraction in fast-typing contexts. "
                    + "Default: 1 (Windows 11). Recommended: 0.",
                Tags = ["keyboard", "suggestions", "hardware", "typing", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "HardwareKeyboardTextSuggestions", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "HardwareKeyboardTextSuggestions")],
                DetectOps = [RegOp.CheckDword(Key, "HardwareKeyboardTextSuggestions", 0)],
            },
            new TweakDef
            {
                Id = "sptype-disable-swipe-typing",
                Label = "Disable Touch Keyboard Swipe Typing",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
                Description =
                    "Sets SwipeKeyboardEnabled=0 in the SpellingAndTyping policy key. Disables "
                    + "swipe (gesture) typing on the Windows touch keyboard, requiring each "
                    + "character to be tapped individually. Swipe typing continuously records "
                    + "finger-trajectory paths across the keyboard surface, feeding a gesture "
                    + "recognition model that accesses typed words indirectly via path geometry. "
                    + "Default: 1. Recommended: 0 on secure devices.",
                Tags = ["swipe", "gesture", "touch", "keyboard", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "SwipeKeyboardEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "SwipeKeyboardEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "SwipeKeyboardEnabled", 0)],
            },
            new TweakDef
            {
                Id = "sptype-disable-typing-telemetry",
                Label = "Disable Typing Telemetry Upload",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
                Description =
                    "Sets TypingDataCollectionEnabled=0 in the SpellingAndTyping policy key. "
                    + "Prevents Windows from uploading typing-pattern telemetry to Microsoft's "
                    + "cloud services. Typing telemetry includes keystroke timing, word-choice "
                    + "corrections, and session length, aggregated to improve the shared "
                    + "language model. This data constitutes detailed behavioural profiling "
                    + "even when individual keystrokes are not transmitted. "
                    + "Default: 1. Recommended: 0.",
                Tags = ["typing", "telemetry", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "TypingDataCollectionEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "TypingDataCollectionEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "TypingDataCollectionEnabled", 0)],
            },
            new TweakDef
            {
                Id = "sptype-disable-handwriting-recognition",
                Label = "Disable Handwriting Recognition Improvement",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
                Description =
                    "Sets HandwritingAcceptedSamples=0 in the SpellingAndTyping policy key. "
                    + "Stops Windows from collecting pen-stroke samples to improve the on-device "
                    + "handwriting recognition model via the Windows Feedback infrastructure. "
                    + "Sample collection records user handwriting during normal use and "
                    + "periodically packages ink data for processing. "
                    + "Default: 1. Recommended: 0.",
                Tags = ["handwriting", "recognition", "ink", "samples", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "HandwritingAcceptedSamples", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "HandwritingAcceptedSamples")],
                DetectOps = [RegOp.CheckDword(Key, "HandwritingAcceptedSamples", 0)],
            },
            new TweakDef
            {
                Id = "sptype-disable-autocomplete",
                Label = "Disable System Autocomplete",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
                Description =
                    "Sets AutoCompleteEnabled=0 in the SpellingAndTyping policy key. Disables "
                    + "the system-wide autocomplete engine that suggests previously entered "
                    + "values in form fields, search boxes, and address bars using the Windows "
                    + "Activity History store. Autocomplete draws on stored usage history to "
                    + "reconstruct past inputs, and its suggestion pool can disclose previously "
                    + "visited URLs or typed content to other users of the same account. "
                    + "Default: 1. Recommended: 0.",
                Tags = ["autocomplete", "form", "history", "typing", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AutoCompleteEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AutoCompleteEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "AutoCompleteEnabled", 0)],
            },
        ];
    
    }

    // ── WindowsDiagnostics ──
    private static class _WindowsDiagnostics
    {    
        private const string WerPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting";
        private const string WerService = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting";
        private const string WerPCHealth = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PCHealth\ErrorReporting";
        private const string AppCompatPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat";
        private const string AppCompatFlags = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\AppCompatFlags";
        private const string FeedbackPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection";
    
        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "wdiag-disable-wer-reporting",
                Label = "Diagnostics: Disable Windows Error Reporting (WER)",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true,
                CorpSafe = false,
                MinBuild = 17763,
                ImpactScore = 3,
                SafetyRating = 4,
                RegistryKeys = [WerPolicy],
                Tags = ["diagnostics", "wer", "error-reporting", "privacy", "telemetry", "crash", "microsoft"],
                Description =
                    "Sets Disabled=1 in the Windows Error Reporting policy. "
                    + "Stops the WerFault.exe process from uploading crash dumps, minidumps, and "
                    + "application error reports to Microsoft. Reduces background I/O on crash events "
                    + "and prevents unintentional sensitive data transmission in crash reports.",
                ApplyOps = [RegOp.SetDword(WerPolicy, "Disabled", 1)],
                RemoveOps = [RegOp.DeleteValue(WerPolicy, "Disabled")],
                DetectOps = [RegOp.CheckDword(WerPolicy, "Disabled", 1)],
            },
            new TweakDef
            {
                Id = "wdiag-disable-wer-queue",
                Label = "Diagnostics: Disable Windows Error Report Queue (Prevent Deferred Uploads)",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true,
                CorpSafe = false,
                MinBuild = 17763,
                ImpactScore = 2,
                SafetyRating = 4,
                RegistryKeys = [WerService],
                Tags = ["diagnostics", "wer", "error-reporting", "privacy", "disk"],
                Description =
                    "Sets DontSendAdditionalData=1 in the WER service key. "
                    + "Prevents WER from sending additional data beyond the basic error notification "
                    + "(e.g. queued minidump files, extended diagnostic data) to Microsoft. "
                    + "The queue can accumulate gigabytes of crash archives on unstable systems.",
                ApplyOps = [RegOp.SetDword(WerService, "DontSendAdditionalData", 1)],
                RemoveOps = [RegOp.DeleteValue(WerService, "DontSendAdditionalData")],
                DetectOps = [RegOp.CheckDword(WerService, "DontSendAdditionalData", 1)],
            },
            new TweakDef
            {
                Id = "wdiag-opt-out-wer-enterprise",
                Label = "Diagnostics: Opt Out of Windows Error Reporting Enterprise Tier",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 17763,
                ImpactScore = 2,
                SafetyRating = 5,
                RegistryKeys = [WerPCHealth],
                Tags = ["diagnostics", "wer", "error-reporting", "privacy", "enterprise"],
                Description =
                    "Sets DoReport=0 in PCHealth\\ErrorReporting. "
                    + "Disables Windows Error Reporting at the PCHealth layer (legacy 'Dr Watson' path). "
                    + "This covers older error-reporting pathways that bypass the modern WER policy. "
                    + "CorpSafe=true because corporate environments often control WER separately.",
                ApplyOps = [RegOp.SetDword(WerPCHealth, "DoReport", 0)],
                RemoveOps = [RegOp.DeleteValue(WerPCHealth, "DoReport")],
                DetectOps = [RegOp.CheckDword(WerPCHealth, "DoReport", 0)],
            },
            new TweakDef
            {
                Id = "wdiag-disable-app-compat-telemetry",
                Label = "Diagnostics: Disable Application Compatibility Telemetry (CEIP)",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true,
                CorpSafe = false,
                MinBuild = 17763,
                ImpactScore = 3,
                SafetyRating = 4,
                RegistryKeys = [AppCompatPolicy],
                Tags = ["diagnostics", "app-compat", "telemetry", "privacy", "ceip"],
                Description =
                    "Sets DisableInventory=1 and DisableUAR=1 in AppCompat policy. "
                    + "Stops the Application Compatibility Telemetry component from collecting and "
                    + "uploading application compatibility data and 'User Activity Reporting' logs. "
                    + "This data feeds into Microsoft's CEIP program.",
                ApplyOps = [RegOp.SetDword(AppCompatPolicy, "DisableInventory", 1), RegOp.SetDword(AppCompatPolicy, "DisableUAR", 1)],
                RemoveOps = [RegOp.DeleteValue(AppCompatPolicy, "DisableInventory"), RegOp.DeleteValue(AppCompatPolicy, "DisableUAR")],
                DetectOps = [RegOp.CheckDword(AppCompatPolicy, "DisableInventory", 1), RegOp.CheckDword(AppCompatPolicy, "DisableUAR", 1)],
            },
            new TweakDef
            {
                Id = "wdiag-disable-app-compat-engine",
                Label = "Diagnostics: Disable Application Compatibility Engine",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true,
                CorpSafe = false,
                MinBuild = 17763,
                ImpactScore = 3,
                SafetyRating = 3,
                RegistryKeys = [AppCompatPolicy],
                Tags = ["diagnostics", "app-compat", "performance", "privacy"],
                Description =
                    "Sets DisableProgramLog=1 in AppCompat policy. "
                    + "Disables the Windows Application Compatibility engine (Shim Engine). "
                    + "Slightly reduces application launch latency. "
                    + "WARNING: may prevent older 16-bit or poorly-written apps from running "
                    + "because compatibility shims are no longer applied at launch.",
                ApplyOps = [RegOp.SetDword(AppCompatPolicy, "DisableProgramLog", 1)],
                RemoveOps = [RegOp.DeleteValue(AppCompatPolicy, "DisableProgramLog")],
                DetectOps = [RegOp.CheckDword(AppCompatPolicy, "DisableProgramLog", 1)],
            },
            new TweakDef
            {
                Id = "wdiag-disable-psr",
                Label = "Diagnostics: Disable Problem Steps Recorder (PSR / psr.exe)",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 17763,
                ImpactScore = 2,
                SafetyRating = 5,
                RegistryKeys = [WerPolicy],
                Tags = ["diagnostics", "psr", "screen-capture", "privacy", "security"],
                Description =
                    "Sets DisableArchive=1 in the WER policy (blocks PSR archive creation). "
                    + "Disables the Problem Steps Recorder (psr.exe) which can silently screenshot "
                    + "an entire troubleshooting session. Prevents potential privacy exposure if "
                    + "support staff misuse PSR on sensitive workloads.",
                ApplyOps = [RegOp.SetDword(WerPolicy, "DisableArchive", 1)],
                RemoveOps = [RegOp.DeleteValue(WerPolicy, "DisableArchive")],
                DetectOps = [RegOp.CheckDword(WerPolicy, "DisableArchive", 1)],
            },
            new TweakDef
            {
                Id = "wdiag-disable-feedback-notifications",
                Label = "Diagnostics: Disable Windows Feedback and Satisfaction Surveys",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 17763,
                ImpactScore = 2,
                SafetyRating = 5,
                RegistryKeys = [FeedbackPolicy],
                Tags = ["diagnostics", "feedback", "feedback-hub", "notifications", "privacy", "debloat"],
                Description =
                    "Sets DoNotShowFeedbackNotifications=1 in DataCollection policy. "
                    + "Prevents Windows from displaying pop-up feedback request dialogs "
                    + "(\"How do you like Windows?\", \"Rate your experience\"). "
                    + "These notifications are distracting and require user interaction to dismiss.",
                ApplyOps = [RegOp.SetDword(FeedbackPolicy, "DoNotShowFeedbackNotifications", 1)],
                RemoveOps = [RegOp.DeleteValue(FeedbackPolicy, "DoNotShowFeedbackNotifications")],
                DetectOps = [RegOp.CheckDword(FeedbackPolicy, "DoNotShowFeedbackNotifications", 1)],
            },
            new TweakDef
            {
                Id = "wdiag-disable-reliability-monitor",
                Label = "Diagnostics: Disable Reliability Monitor Data Collection",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true,
                CorpSafe = false,
                MinBuild = 17763,
                ImpactScore = 2,
                SafetyRating = 4,
                RegistryKeys = [AppCompatFlags],
                Tags = ["diagnostics", "reliability-monitor", "privacy", "disk", "performance"],
                Description =
                    "Sets DisableReliabilityAnalysisComponent=1 in AppCompatFlags. "
                    + "Stops the Reliability Analysis Component (RAC) from tracking application "
                    + "failures and system events in the Reliability Monitor database (RacAgent task). "
                    + "Reduces background disk activity. Reliability Monitor data is also deleted.",
                ApplyOps = [RegOp.SetDword(AppCompatFlags, "DisableReliabilityAnalysisComponent", 1)],
                RemoveOps = [RegOp.DeleteValue(AppCompatFlags, "DisableReliabilityAnalysisComponent")],
                DetectOps = [RegOp.CheckDword(AppCompatFlags, "DisableReliabilityAnalysisComponent", 1)],
            },
            new TweakDef
            {
                Id = "wdiag-set-wer-local-dumps",
                Label = "Diagnostics: Redirect Crash Dumps to Local Folder (No Upload)",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 17763,
                ImpactScore = 3,
                SafetyRating = 5,
                RegistryKeys = [WerService],
                Tags = ["diagnostics", "wer", "crash-dump", "privacy", "forensics", "debugging"],
                Description =
                    "Sets LocalDumps path to %LOCALAPPDATA%\\CrashDumps in WER service key. "
                    + "Configures WER to save application crash minidumps locally instead of uploading "
                    + "to Microsoft. Allows developers and IT to analyse crashes without cloud upload. "
                    + "Dumps are useful for post-mortem debugging while keeping data on-premises.",
                ApplyOps =
                [
                    RegOp.SetExpandString(
                        @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps",
                        "DumpFolder",
                        @"%LOCALAPPDATA%\CrashDumps"
                    ),
                    RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps", "DumpCount", 10),
                    RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps", "DumpType", 2),
                ],
                RemoveOps = [RegOp.DeleteTree(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps")],
                DetectOps =
                [
                    RegOp.CheckString(
                        @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps",
                        "DumpFolder",
                        @"%LOCALAPPDATA%\CrashDumps"
                    ),
                ],
            },
            new TweakDef
            {
                Id = "wdiag-disable-insider-preview-builds",
                Label = "Diagnostics: Block Windows Insider Preview Build Notifications",
                Category = "Privacy & Telemetry Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 17763,
                ImpactScore = 2,
                SafetyRating = 5,
                RegistryKeys = [FeedbackPolicy],
                Tags = ["diagnostics", "insider", "windows-update", "notifications", "privacy", "debloat"],
                Description =
                    "Sets DisableWindowsConsumerFeatures=1 in DataCollection policy. "
                    + "Prevents Windows from offering Insider Preview builds, feature previews, and "
                    + "consumer-targeted nag screens in enterprise/standalone environments. "
                    + "Keeps production machines on stable release channels.",
                ApplyOps = [RegOp.SetDword(FeedbackPolicy, "DisableWindowsConsumerFeatures", 1)],
                RemoveOps = [RegOp.DeleteValue(FeedbackPolicy, "DisableWindowsConsumerFeatures")],
                DetectOps = [RegOp.CheckDword(FeedbackPolicy, "DisableWindowsConsumerFeatures", 1)],
            },
        ];
    
    }

    // ── WindowsDiagnosticsInfraPolicy ──
    private static class _WindowsDiagnosticsInfraPolicy
    {    
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDI";
    
        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wdip-disable-scenario-execution",
                    Label = "Disable WDI Diagnostic Scenario Execution",
                    Category = "Privacy & Telemetry Policy",
                    Description =
                        "Sets ScenarioExecutionEnabled=0 to prevent Windows Diagnostic Infrastructure from running "
                        + "built-in diagnostic scenarios. WDI scenarios collect hardware, driver, and application state "
                        + "data for Microsoft telemetry and self-healing analysis. Disabling reduces background data collection "
                        + "and eliminates associated CPU/disk spikes from diagnostic scenario runners.",
                    Tags = ["diagnostics", "wdi", "telemetry", "performance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Disables WDI scenario runners; Windows self-healing and troubleshooter automation loses data inputs.",
                    ApplyOps = [RegOp.SetDword(Key, "ScenarioExecutionEnabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ScenarioExecutionEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "ScenarioExecutionEnabled", 0)],
                },
                new TweakDef
                {
                    Id = "wdip-disable-diagnostic-triggers",
                    Label = "Disable WDI Diagnostic Trigger Execution",
                    Category = "Privacy & Telemetry Policy",
                    Description =
                        "Sets DiagnosticTriggerExecution=0 to prevent WDI from launching diagnostic routines in response "
                        + "to system event triggers (crash, hang, driver error). Trigger-based diagnostics collect snapshot data "
                        + "sent to the Windows Error Reporting and telemetry pipelines. Disabling reduces post-fault data gathering.",
                    Tags = ["diagnostics", "wdi", "triggers", "telemetry", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Prevents event-triggered WDI diagnostics; reduces telemetry sent on crash/hang events.",
                    ApplyOps = [RegOp.SetDword(Key, "DiagnosticTriggerExecution", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DiagnosticTriggerExecution")],
                    DetectOps = [RegOp.CheckDword(Key, "DiagnosticTriggerExecution", 0)],
                },
                new TweakDef
                {
                    Id = "wdip-disable-result-summary",
                    Label = "Disable WDI Diagnostic Result Summary Collection",
                    Category = "Privacy & Telemetry Policy",
                    Description =
                        "Sets ResultSummaryEnabled=0 to prevent Windows Diagnostic Infrastructure from generating and storing "
                        + "diagnostic result summaries. These summaries aggregate diagnostic run outcomes and feed Windows reliability "
                        + "history and performance monitoring panels. Disabling reduces disk writes and summary data leakage.",
                    Tags = ["diagnostics", "wdi", "results", "telemetry", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "No diagnostic result summaries stored; Reliability Monitor may show reduced detail.",
                    ApplyOps = [RegOp.SetDword(Key, "ResultSummaryEnabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ResultSummaryEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "ResultSummaryEnabled", 0)],
                },
                new TweakDef
                {
                    Id = "wdip-disable-task-collection",
                    Label = "Disable WDI Diagnostic Task Collection",
                    Category = "Privacy & Telemetry Policy",
                    Description =
                        "Sets EnableDiagnosticTaskCollection=0 to stop WDI from scheduling and collecting data via "
                        + "background diagnostic tasks. Diagnostic task collection uses scheduled tasks in the "
                        + "'Microsoft\\Windows\\WDI' task folder to periodically gather system state; disabling "
                        + "prevents these tasks from running and collecting data.",
                    Tags = ["diagnostics", "wdi", "tasks", "telemetry", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Disables WDI scheduled task data collection; Windows automatic maintenance diagnose routines stop.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableDiagnosticTaskCollection", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableDiagnosticTaskCollection")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableDiagnosticTaskCollection", 0)],
                },
                new TweakDef
                {
                    Id = "wdip-disable-scenario-logging",
                    Label = "Disable WDI Diagnostic Scenario Event Logging",
                    Category = "Privacy & Telemetry Policy",
                    Description =
                        "Sets ScenarioLoggingEnabled=0 to prevent WDI from writing diagnostic scenario execution results "
                        + "to the Windows event log. Reduces event log noise from the WDI event provider and prevents "
                        + "diagnostic scenario details from appearing in event log SIEM forwarding pipelines.",
                    Tags = ["diagnostics", "wdi", "logging", "event-log", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Stops WDI event log writes; reduces diagnostic event volume in Event Viewer.",
                    ApplyOps = [RegOp.SetDword(Key, "ScenarioLoggingEnabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ScenarioLoggingEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "ScenarioLoggingEnabled", 0)],
                },
                new TweakDef
                {
                    Id = "wdip-disable-results-caching",
                    Label = "Disable WDI Diagnostic Results Caching",
                    Category = "Privacy & Telemetry Policy",
                    Description =
                        "Sets ResultsCachingEnabled=0 to prevent WDI from caching diagnostic run results to disk. "
                        + "Without caching, each diagnostic scenario must re-run fully if retriggered instead of serving "
                        + "a prior result. This reduces disk I/O from the WDI cache directory while slightly increasing "
                        + "CPU usage if scenarios are repeatedly triggered.",
                    Tags = ["diagnostics", "wdi", "caching", "disk", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Disables result cache on disk; minimal impact unless WDI runs in tight repeat cycles.",
                    ApplyOps = [RegOp.SetDword(Key, "ResultsCachingEnabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ResultsCachingEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "ResultsCachingEnabled", 0)],
                },
                new TweakDef
                {
                    Id = "wdip-set-max-persistence-days",
                    Label = "Limit WDI Scenario Result Persistence",
                    Category = "Privacy & Telemetry Policy",
                    Description =
                        "Sets MaxScenarioPersistenceDurationDays=1 to keep diagnostic scenario result data on disk for "
                        + "no more than 1 day. Stale diagnostic data is purged quickly, limiting the window in which "
                        + "stored diagnostic snapshots (which may include sensitive system state) persist on the endpoint.",
                    Tags = ["diagnostics", "wdi", "retention", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Purges diagnostic results after 1 day; limits disk footprint of WDI data store.",
                    ApplyOps = [RegOp.SetDword(Key, "MaxScenarioPersistenceDurationDays", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "MaxScenarioPersistenceDurationDays")],
                    DetectOps = [RegOp.CheckDword(Key, "MaxScenarioPersistenceDurationDays", 1)],
                },
                new TweakDef
                {
                    Id = "wdip-disable-msa-diagnostics",
                    Label = "Disable MSA-Linked WDI Diagnostics",
                    Category = "Privacy & Telemetry Policy",
                    Description =
                        "Sets DisableDiagnosticsMSA=1 to prevent WDI from associating diagnostic results and telemetry "
                        + "with the user's Microsoft Account (MSA). MSA-linked diagnostics allow personalised Windows "
                        + "troubleshooting recommendations based on cloud-side analysis of collected data. Disabling "
                        + "reduces cloud data upload associated with MSA-bound user sessions.",
                    Tags = ["diagnostics", "wdi", "microsoft-account", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Severs diagnostic→MSA linkage; personalised troubleshooter suggestions unavailable.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableDiagnosticsMSA", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableDiagnosticsMSA")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableDiagnosticsMSA", 1)],
                },
                new TweakDef
                {
                    Id = "wdip-disable-boot-diagnostics",
                    Label = "Disable WDI Boot Diagnostic Collection",
                    Category = "Privacy & Telemetry Policy",
                    Description =
                        "Sets EnableBootDiagnostics=0 to prevent WDI from collecting boot-time diagnostic data during "
                        + "the Windows startup phase. Boot diagnostics capture driver initialisation timing, boot event "
                        + "sequences, and early-phase performance counters used by the Windows startup performance "
                        + "optimisation engine. Disabling reduces boot-phase disk activity.",
                    Tags = ["diagnostics", "wdi", "boot", "performance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ImpactNote = "Suppresses boot-phase WDI data collection; Windows startup optimiser loses recent boot data.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableBootDiagnostics", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableBootDiagnostics")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableBootDiagnostics", 0)],
                },
                new TweakDef
                {
                    Id = "wdip-prevent-diagnostic-task-execution",
                    Label = "Prevent WDI Diagnostic Task Execution via Policy",
                    Category = "Privacy & Telemetry Policy",
                    Description =
                        "Sets PreventDiagnosticTaskExecution=1 to apply a hard Group Policy block on all WDI-managed "
                        + "diagnostic task execution. This policy-level enforcement takes precedence over local WDI "
                        + "configuration and prevents individual users or services from re-enabling WDI task execution "
                        + "through Task Scheduler or WMI. Combines with ScenarioExecutionEnabled=0 for full lockdown.",
                    Tags = ["diagnostics", "wdi", "policy", "lockdown", "gpo"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Policy-level WDI task block; Windows troubleshooters and self-healing routines will not trigger.",
                    ApplyOps = [RegOp.SetDword(Key, "PreventDiagnosticTaskExecution", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "PreventDiagnosticTaskExecution")],
                    DetectOps = [RegOp.CheckDword(Key, "PreventDiagnosticTaskExecution", 1)],
                },
            ];
    
    }

    // ── WindowsDiagTrackPolicy ──
    private static class _WindowsDiagTrackPolicy
    {    
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection";
    
        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "diagtrk-set-telemetry-security-only",
                Label = "Windows DiagTrack: Set Telemetry to Security (Level 0)",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Sets the Windows diagnostic data collection level to Security (level 0) — the lowest telemetry tier. "
                    + "Security-level telemetry sends only data required for Windows Defender ATP and Windows security functions. "
                    + "No app usage, browsing, or performance data is collected at this level. "
                    + "Note: Security level (0) applies to Enterprise and Education editions only; other editions receive Basic as their minimum.",
                Tags = ["telemetry", "diagtrack", "privacy", "security-level", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowTelemetry", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "AllowTelemetry", 0)],
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Minimizes telemetry to security-only; maximum privacy for enterprise endpoints.",
            },
            new TweakDef
            {
                Id = "diagtrk-disable-opt-in-change-notifications",
                Label = "Windows DiagTrack: Disable Telemetry Opt-In Change Notifications",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Prevents Windows from prompting users to change their diagnostic data settings. "
                    + "When telemetry is centrally managed via GPO, user-facing prompts to adjust data collection settings are noise and could cause confusion. "
                    + "This policy suppresses the Settings dialog and toast notifications that ask users to consent to telemetry level changes. "
                    + "Removing this policy re-enables opt-in change notifications and Settings prompts.",
                Tags = ["telemetry", "diagtrack", "opt-in", "notifications", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableTelemetryOptInChangeNotification", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableTelemetryOptInChangeNotification")],
                DetectOps = [RegOp.CheckDword(Key, "DisableTelemetryOptInChangeNotification", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Suppresses telemetry opt-in dialogs; keeps managed telemetry settings stable.",
            },
            new TweakDef
            {
                Id = "diagtrk-disable-opt-in-settings-ui",
                Label = "Windows DiagTrack: Disable Telemetry Opt-In Settings UI",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Hides the diagnostic data opt-in settings page in the Privacy section of Windows Settings. "
                    + "When telemetry is managed via GPO or Intune, the Settings page is redundant and could confuse users into thinking they can adjust the policy. "
                    + "Hiding the page ensures users do not inadvertently change settings that are centrally managed. "
                    + "Removing this policy restores the telemetry settings UI in Windows Settings > Privacy.",
                Tags = ["telemetry", "diagtrack", "settings-ui", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableTelemetryOptInSettingsUx", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableTelemetryOptInSettingsUx")],
                DetectOps = [RegOp.CheckDword(Key, "DisableTelemetryOptInSettingsUx", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Removes telemetry UI from Settings; prevents user changes to managed telemetry policy.",
            },
            new TweakDef
            {
                Id = "diagtrk-disable-enterprise-auth-proxy",
                Label = "Windows DiagTrack: Disable Enterprise Authentication Proxy for Telemetry",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Prevents the DiagTrack service from using authenticated proxy servers for telemetry data uploads. "
                    + "When an enterprise proxy requires NTLM/Kerberos authentication, the DiagTrack service may authenticate with machine credentials. "
                    + "Disabling this proxy allows the service to fail uploads rather than authenticate, reducing credential exposure over potentially monitored proxies. "
                    + "Removing this policy allows DiagTrack to use the enterprise auth proxy for uploads.",
                Tags = ["telemetry", "diagtrack", "proxy", "enterprise", "credentials", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableEnterpriseAuthProxy", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableEnterpriseAuthProxy")],
                DetectOps = [RegOp.CheckDword(Key, "DisableEnterpriseAuthProxy", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Blocks DiagTrack proxy auth; prevents machine credential use for telemetry uploads.",
            },
            new TweakDef
            {
                Id = "diagtrk-disable-device-name-in-telemetry",
                Label = "Windows DiagTrack: Prevent Device Name from Being Sent in Telemetry",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Prevents Windows from including the device hostname in diagnostic telemetry data sent to Microsoft. "
                    + "Device names can reveal organizational naming conventions, asset tag formats, and employee details. "
                    + "With this policy set, telemetry reports use an anonymized device identifier instead of the human-readable hostname. "
                    + "Removing this policy allows the device name to be included in telemetry.",
                Tags = ["telemetry", "diagtrack", "device-name", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowDeviceNameInTelemetry", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowDeviceNameInTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "AllowDeviceNameInTelemetry", 0)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Excludes hostname from telemetry; prevents device naming convention disclosure.",
            },
            new TweakDef
            {
                Id = "diagtrk-limit-diagnostic-log-collection",
                Label = "Windows DiagTrack: Limit Diagnostic Log Collection",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Limits the amount of diagnostic log data that the Connected User Experiences and Telemetry service can collect. "
                    + "Unrestricted log collection can gather large volumes of user activity and application usage data. "
                    + "Limiting collection reduces the diagnostic data footprint while still allowing critical security telemetry. "
                    + "Removing this policy restores unrestricted diagnostic log collection by DiagTrack.",
                Tags = ["telemetry", "diagtrack", "log-collection", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "LimitDiagnosticLogCollection", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "LimitDiagnosticLogCollection")],
                DetectOps = [RegOp.CheckDword(Key, "LimitDiagnosticLogCollection", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Limits diagnostic log volume; reduces telemetry data footprint on managed endpoints.",
            },
            new TweakDef
            {
                Id = "diagtrk-disable-dump-collection",
                Label = "Windows DiagTrack: Disable Crash Dump Collection for Telemetry",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Prevents the DiagTrack service from collecting and uploading process crash dumps as part of diagnostic telemetry. "
                    + "Crash dumps can contain sensitive memory contents, user data, and application secrets captured at the moment of a crash. "
                    + "Disabling dump collection ensures no memory contents are transmitted to Microsoft telemetry endpoints. "
                    + "Removing this policy allows DiagTrack to include crash dumps in telemetry uploads.",
                Tags = ["telemetry", "diagtrack", "crash-dump", "privacy", "memory", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DoNotShowFeedbackNotifications", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DoNotShowFeedbackNotifications")],
                DetectOps = [RegOp.CheckDword(Key, "DoNotShowFeedbackNotifications", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Suppresses feedback collection notifications; reduces DiagTrack memory upload.",
            },
            new TweakDef
            {
                Id = "diagtrk-limit-dump-collection",
                Label = "Windows DiagTrack: Limit Dump Collection Level",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Limits the diagnostic dump collection level to minimize the amount of memory captured during crash events used for telemetry. "
                    + "Windows can collect kernel dumps, mini dumps, or full memory dumps for telemetry reporting. "
                    + "Setting a lower collection level reduces sensitive data exposure while still enabling crash analysis. "
                    + "Removing this policy reverts to the default dump collection level for telemetry.",
                Tags = ["telemetry", "diagtrack", "dump", "memory", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "LimitDumpCollection", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "LimitDumpCollection")],
                DetectOps = [RegOp.CheckDword(Key, "LimitDumpCollection", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Limits memory dump level for telemetry; reduces sensitive data in crash reports.",
            },
            new TweakDef
            {
                Id = "diagtrk-disable-one-settings-downloads",
                Label = "Windows DiagTrack: Disable OneSettings Policy Downloads",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Prevents Windows from downloading configuration and feature flag updates via the OneSettings service used by DiagTrack. "
                    + "OneSettings allows Microsoft to remotely change Windows behavior, feature availability, and telemetry configurations. "
                    + "Blocking OneSettings downloads ensures that remote policy changes cannot override locally set configurations on managed endpoints. "
                    + "Removing this policy allows OneSettings to download and apply remote configuration changes.",
                Tags = ["telemetry", "diagtrack", "onesettings", "remote-config", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableOneSettingsDownloads", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableOneSettingsDownloads")],
                DetectOps = [RegOp.CheckDword(Key, "DisableOneSettingsDownloads", 1)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Blocks remote Windows configuration changes via OneSettings; preserves local policy integrity.",
            },
            new TweakDef
            {
                Id = "diagtrk-disable-cloud-clipboard-integration",
                Label = "Windows DiagTrack: Disable Cloud Clipboard Telemetry",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Prevents the DiagTrack service from collecting clipboard content samples as part of cloud clipboard telemetry. "
                    + "When Cloud Clipboard is enabled, diagnostic telemetry may include usage metadata that could indirectly expose clipboard patterns. "
                    + "This policy disables the cloud clipboard data collection path within DiagTrack. "
                    + "Removing this policy restores Cloud Clipboard telemetry collection within DiagTrack.",
                Tags = ["telemetry", "diagtrack", "clipboard", "cloud", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowClipboardHistory", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowClipboardHistory")],
                DetectOps = [RegOp.CheckDword(Key, "AllowClipboardHistory", 0)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Blocks clipboard telemetry via DiagTrack; prevents clipboard usage patterns from being collected.",
            },
        ];
    
    }

    // ── WindowsInfoProtectionPolicy ──
    private static class _WindowsInfoProtectionPolicy
    {    
        private const string WipKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataProtection";
        private const string EdpKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EnterpriseDataProtection";
        private const string NetIsoKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkIsolation";
    
        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "wippol-allow-user-decrypt",
                Label = "WIP: Allow User to Decrypt Protected Files",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Allows the owner of a WIP-protected file to decrypt it. When disabled, only IT admins can remove protection. Enabled by default; disable for stricter DLP control.",
                Tags = ["wip", "edp", "dlp", "data-protection", "encryption", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Prevents users from self-decrypting WIP-protected files; admin-only removal.",
                RegistryKeys = [WipKey],
                ApplyOps = [RegOp.SetDword(WipKey, "AllowUserDecryption", 0)],
                RemoveOps = [RegOp.DeleteValue(WipKey, "AllowUserDecryption")],
                DetectOps = [RegOp.CheckDword(WipKey, "AllowUserDecryption", 0)],
            },
            new TweakDef
            {
                Id = "wippol-require-protection-under-lock",
                Label = "WIP: Require Protection While Under Lock",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Requires that WIP-protected data remain encrypted even when the device is locked. Prevents data access from unauthorized physical access when the screen is locked.",
                Tags = ["wip", "edp", "lock-screen", "data-protection", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Keeps WIP data encrypted when device is locked.",
                RegistryKeys = [WipKey],
                ApplyOps = [RegOp.SetDword(WipKey, "RequireProtectionUnderLockConfig", 1)],
                RemoveOps = [RegOp.DeleteValue(WipKey, "RequireProtectionUnderLockConfig")],
                DetectOps = [RegOp.CheckDword(WipKey, "RequireProtectionUnderLockConfig", 1)],
            },
            new TweakDef
            {
                Id = "wippol-enable-edp",
                Label = "WIP: Enable Enterprise Data Protection",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Enables Windows Information Protection (formerly Enterprise Data Protection/EDP) on the device. Mode 3 = Block — prevents users from copying work data to personal apps.",
                Tags = ["wip", "edp", "dlp", "enterprise", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote = "Enables WIP/EDP on the device — prerequisite for all WIP policy tweaks.",
                RegistryKeys = [EdpKey],
                ApplyOps = [RegOp.SetDword(EdpKey, "WIPEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(EdpKey, "WIPEnabled")],
                DetectOps = [RegOp.CheckDword(EdpKey, "WIPEnabled", 1)],
            },
            new TweakDef
            {
                Id = "wippol-silent-mode",
                Label = "WIP: Set Silent Enforcement (Audit Without Block)",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Sets WIP/EDP to silent enforcement mode. Personal data leakage is logged to the event log but not blocked. Useful for piloting WIP before enforcing restrictions.",
                Tags = ["wip", "edp", "dlp", "audit", "silent", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Sets WIP to audit-only mode for piloting before full enforcement.",
                RegistryKeys = [EdpKey],
                ApplyOps = [RegOp.SetDword(EdpKey, "EnforcementMode", 1)],
                RemoveOps = [RegOp.DeleteValue(EdpKey, "EnforcementMode")],
                DetectOps = [RegOp.CheckDword(EdpKey, "EnforcementMode", 1)],
            },
            new TweakDef
            {
                Id = "wippol-block-copy-to-personal",
                Label = "WIP: Block Copying Work Data to Personal Apps",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Enforces WIP to Block mode — users cannot copy, paste, or share protected work data to personal or unmanaged applications. The strictly enforced DLP level.",
                Tags = ["wip", "edp", "dlp", "block", "clipboard", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote = "Block mode — prevents copying work data to personal apps.",
                RegistryKeys = [EdpKey],
                ApplyOps = [RegOp.SetDword(EdpKey, "EnforcementMode", 3)],
                RemoveOps = [RegOp.DeleteValue(EdpKey, "EnforcementMode")],
                DetectOps = [RegOp.CheckDword(EdpKey, "EnforcementMode", 3)],
            },
            new TweakDef
            {
                Id = "wippol-disable-bing-results-wip",
                Label = "WIP: Disable Bing Integration for Work Searches",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Prevents Windows Search from sending work-context search queries to Bing when WIP is enabled. Keeps enterprise search results isolated from the internet.",
                Tags = ["wip", "edp", "bing", "search", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents work-context searches from going to Bing when WIP is active.",
                RegistryKeys = [EdpKey],
                ApplyOps = [RegOp.SetDword(EdpKey, "DisableWindowsBingSearch", 1)],
                RemoveOps = [RegOp.DeleteValue(EdpKey, "DisableWindowsBingSearch")],
                DetectOps = [RegOp.CheckDword(EdpKey, "DisableWindowsBingSearch", 1)],
            },
            new TweakDef
            {
                Id = "wippol-revoke-on-unenroll",
                Label = "WIP: Revoke Access Keys on MDM Unenrollment",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Automatically revokes WIP encryption keys when the device is unenrolled from MDM. Prevents access to protected work data after device management is removed.",
                Tags = ["wip", "edp", "mdm", "revoke", "unenroll", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Revokes WIP keys when device leaves MDM management.",
                RegistryKeys = [EdpKey],
                ApplyOps = [RegOp.SetDword(EdpKey, "RevokeOnMDMHandoff", 1)],
                RemoveOps = [RegOp.DeleteValue(EdpKey, "RevokeOnMDMHandoff")],
                DetectOps = [RegOp.CheckDword(EdpKey, "RevokeOnMDMHandoff", 1)],
            },
            new TweakDef
            {
                Id = "wippol-show-ede-icons",
                Label = "WIP: Show Enterprise Data Protection Icons on Protected Files",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Displays a work briefcase icon overlay on WIP-protected files in Explorer and on the Start menu to visually distinguish protected corporate data from personal files.",
                Tags = ["wip", "edp", "icons", "explorer", "visibility", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Shows work briefcase icon overlay on WIP-protected files.",
                RegistryKeys = [EdpKey],
                ApplyOps = [RegOp.SetDword(EdpKey, "ShowIcons", 1)],
                RemoveOps = [RegOp.DeleteValue(EdpKey, "ShowIcons")],
                DetectOps = [RegOp.CheckDword(EdpKey, "ShowIcons", 1)],
            },
            new TweakDef
            {
                Id = "wippol-restrict-clipboard",
                Label = "WIP: Restrict Clipboard Sharing Between Work and Personal",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Restricts clipboard operations to prevent copying WIP-protected (work) content into unmanaged (personal) applications. Prevents clipboard-based data exfiltration.",
                Tags = ["wip", "edp", "clipboard", "dlp", "restriction", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Restricts clipboard to prevent work-to-personal data leakage.",
                RegistryKeys = [EdpKey],
                ApplyOps = [RegOp.SetDword(EdpKey, "ClipboardProtectionLevel", 2)],
                RemoveOps = [RegOp.DeleteValue(EdpKey, "ClipboardProtectionLevel")],
                DetectOps = [RegOp.CheckDword(EdpKey, "ClipboardProtectionLevel", 2)],
            },
            new TweakDef
            {
                Id = "wippol-enterprise-ip-isolation",
                Label = "WIP: Enable Enterprise Network Isolation",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Activates WIP network isolation policy — only IP ranges and domains defined in the enterprise network boundary list are treated as 'work' destinations.",
                Tags = ["wip", "edp", "network-isolation", "enterprise", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Activates WIP network isolation policy for enterprise boundaries.",
                RegistryKeys = [NetIsoKey],
                ApplyOps = [RegOp.SetDword(NetIsoKey, "EnterpriseCloudResources", 1)],
                RemoveOps = [RegOp.DeleteValue(NetIsoKey, "EnterpriseCloudResources")],
                DetectOps = [RegOp.CheckDword(NetIsoKey, "EnterpriseCloudResources", 1)],
            },
        ];
    
    }

    // ── WindowsMapsPolicy ──
    private static class _WindowsMapsPolicy
    {    
        private const string Maps = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Maps";
    
        private const string WinSearch = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search";
    
        private const string WinSearchCu = @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Windows Search";
    
        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "wmaps-disable-auto-download",
                Label = "Maps: Disable automatic map data download and update",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Sets AutoDownloadAndUpdateMapData=0 in the Maps policy key. Prevents the Maps app "
                    + "from automatically downloading and updating offline map data in the background.",
                Tags = ["maps", "download", "background", "data", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Maps, "AutoDownloadAndUpdateMapData", 0)],
                RemoveOps = [RegOp.DeleteValue(Maps, "AutoDownloadAndUpdateMapData")],
                DetectOps = [RegOp.CheckDword(Maps, "AutoDownloadAndUpdateMapData", 0)],
            },
            new TweakDef
            {
                Id = "wmaps-disable-untriggered-network",
                Label = "Maps: Disable untriggered network traffic from Maps settings page",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Sets AllowUntriggeredNetworkTrafficOnSettingsPage=0. Prevents the Maps settings page "
                    + "from making unsolicited network requests to check for map updates or new regions.",
                Tags = ["maps", "network", "traffic", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Maps, "AllowUntriggeredNetworkTrafficOnSettingsPage", 0)],
                RemoveOps = [RegOp.DeleteValue(Maps, "AllowUntriggeredNetworkTrafficOnSettingsPage")],
                DetectOps = [RegOp.CheckDword(Maps, "AllowUntriggeredNetworkTrafficOnSettingsPage", 0)],
            },
            new TweakDef
            {
                Id = "wmaps-no-connected-search-privacy",
                Label = "Search: Enforce maximum privacy for Connected Search",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Sets ConnectedSearchPrivacy=3 (machine policy). Value 3 blocks web search from "
                    + "the taskbar search box, enforcing the strictest privacy posture.",
                Tags = ["search", "connected-search", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(WinSearch, "ConnectedSearchPrivacy", 3)],
                RemoveOps = [RegOp.DeleteValue(WinSearch, "ConnectedSearchPrivacy")],
                DetectOps = [RegOp.CheckDword(WinSearch, "ConnectedSearchPrivacy", 3)],
            },
            new TweakDef
            {
                Id = "wmaps-enforce-safe-search-strict",
                Label = "Search: Enforce strict SafeSearch in Windows Search",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Sets ConnectedSearchSafeSearch=2 (machine policy). Value 2 = Strict; filters adult "
                    + "content from Windows Search results. Default: 1 (moderate).",
                Tags = ["search", "safe-search", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(WinSearch, "ConnectedSearchSafeSearch", 2)],
                RemoveOps = [RegOp.DeleteValue(WinSearch, "ConnectedSearchSafeSearch")],
                DetectOps = [RegOp.CheckDword(WinSearch, "ConnectedSearchSafeSearch", 2)],
            },
            new TweakDef
            {
                Id = "wmaps-disable-search-highlights",
                Label = "Search: Disable dynamic search highlights in the taskbar",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Sets AllowSearchHighlights=0 (machine policy). Prevents Windows from displaying "
                    + "rotating 'Search Highlights' icons and animations in the taskbar search box.",
                Tags = ["search", "search-highlights", "taskbar", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(WinSearch, "AllowSearchHighlights", 0)],
                RemoveOps = [RegOp.DeleteValue(WinSearch, "AllowSearchHighlights")],
                DetectOps = [RegOp.CheckDword(WinSearch, "AllowSearchHighlights", 0)],
            },
            new TweakDef
            {
                Id = "wmaps-disable-cortana-aad",
                Label = "Search: Disable Cortana for Azure AD accounts (machine policy)",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Sets AllowCortanaInAAD=0 in Windows Search policy. Prevents Cortana from being "
                    + "available for Azure Active Directory (Microsoft Entra ID) signed-in accounts.",
                Tags = ["search", "cortana", "aad", "enterprise", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(WinSearch, "AllowCortanaInAAD", 0)],
                RemoveOps = [RegOp.DeleteValue(WinSearch, "AllowCortanaInAAD")],
                DetectOps = [RegOp.CheckDword(WinSearch, "AllowCortanaInAAD", 0)],
            },
            new TweakDef
            {
                Id = "wmaps-user-no-connected-search-privacy",
                Label = "Search (user): Enforce maximum Connected Search privacy per user",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Sets ConnectedSearchPrivacy=3 at HKCU user-policy scope. Enforces per-user strictest "
                    + "Connected Search privacy for the current signed-in account.",
                Tags = ["search", "connected-search", "privacy", "policy"],
                NeedsAdmin = false,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(WinSearchCu, "ConnectedSearchPrivacy", 3)],
                RemoveOps = [RegOp.DeleteValue(WinSearchCu, "ConnectedSearchPrivacy")],
                DetectOps = [RegOp.CheckDword(WinSearchCu, "ConnectedSearchPrivacy", 3)],
            },
            new TweakDef
            {
                Id = "wmaps-user-enforce-safe-search",
                Label = "Search (user): Enforce strict SafeSearch per user",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Sets ConnectedSearchSafeSearch=2 at HKCU user-policy scope. Enforces strict search "
                    + "result filtering for the current user's Windows Search results.",
                Tags = ["search", "safe-search", "policy"],
                NeedsAdmin = false,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(WinSearchCu, "ConnectedSearchSafeSearch", 2)],
                RemoveOps = [RegOp.DeleteValue(WinSearchCu, "ConnectedSearchSafeSearch")],
                DetectOps = [RegOp.CheckDword(WinSearchCu, "ConnectedSearchSafeSearch", 2)],
            },
            new TweakDef
            {
                Id = "wmaps-user-disable-search-highlights",
                Label = "Search (user): Disable search highlights per user",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Sets AllowSearchHighlights=0 at HKCU user-policy scope. Hides the rotating "
                    + "Search Highlights animations in the taskbar for the current user.",
                Tags = ["search", "search-highlights", "taskbar", "policy"],
                NeedsAdmin = false,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(WinSearchCu, "AllowSearchHighlights", 0)],
                RemoveOps = [RegOp.DeleteValue(WinSearchCu, "AllowSearchHighlights")],
                DetectOps = [RegOp.CheckDword(WinSearchCu, "AllowSearchHighlights", 0)],
            },
            new TweakDef
            {
                Id = "wmaps-user-disable-cortana-aad",
                Label = "Search (user): Disable Cortana for AAD accounts per user",
                Category = "Privacy & Telemetry Policy",
                Description =
                    "Sets AllowCortanaInAAD=0 at HKCU user-policy scope. Disables Cortana for Azure AD "
                    + "accounts at the individual user level, complementing the machine-wide policy.",
                Tags = ["search", "cortana", "aad", "enterprise", "policy"],
                NeedsAdmin = false,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(WinSearchCu, "AllowCortanaInAAD", 0)],
                RemoveOps = [RegOp.DeleteValue(WinSearchCu, "AllowCortanaInAAD")],
                DetectOps = [RegOp.CheckDword(WinSearchCu, "AllowCortanaInAAD", 0)],
            },
        ];
    
    }

}
