namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Privacy
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "priv-disable-telemetry",
            Label = "Disable Windows Telemetry",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 5,
            SafetyRating = 4,
            ImpactNote = "Stops Windows diagnostic data uploads; reduces background CPU and network usage.",
            Description = "Disables Windows telemetry and feedback notifications.",
            Tags = ["privacy", "telemetry", "microsoft"],
            RegistryKeys =
            [
                @"HKLM\SOFTWARE\Policies\Microsoft\Windows\DataCollection",
                @"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\DataCollection",
            ],
            ApplyOps =
            [
                RegOp.SetDword(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "AllowTelemetry", 0),
                RegOp.SetDword(@"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\DataCollection", "AllowTelemetry", 0),
                RegOp.SetDword(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "DoNotShowFeedbackNotifications", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "AllowTelemetry", 3),
                RegOp.SetDword(@"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\DataCollection", "AllowTelemetry", 3),
                RegOp.DeleteValue(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "DoNotShowFeedbackNotifications"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "AllowTelemetry", 0)],
        },
        new TweakDef
        {
            Id = "priv-disable-cortana",
            Label = "Disable Cortana",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Eliminates Cortana and web search overhead; speeds up Start Menu and search bar.",
            Description = "Disables Cortana and web search integration.",
            Tags = ["privacy", "cortana", "search"],
            RegistryKeys = [@"HKLM\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowCortana", 0),
                RegOp.SetDword(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowSearchToUseLocation", 0),
                RegOp.SetDword(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "DisableWebSearch", 1),
                RegOp.SetDword(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "ConnectedSearchUseWeb", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowCortana"),
                RegOp.DeleteValue(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowSearchToUseLocation"),
                RegOp.DeleteValue(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "DisableWebSearch"),
                RegOp.DeleteValue(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "ConnectedSearchUseWeb"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowCortana", 0)],
        },
        new TweakDef
        {
            Id = "priv-disable-activity-history",
            Label = "Disable Activity History",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Stops Windows recording and syncing everything you open, view, and do.",
            Description = "Disables Windows Activity History (Timeline), preventing activity data collection and cloud sync.",
            Tags = ["privacy", "activity", "timeline"],
            RegistryKeys = [@"HKLM\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\System", "EnableActivityFeed", 0),
                RegOp.SetDword(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\System", "PublishUserActivities", 0),
                RegOp.SetDword(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\System", "UploadUserActivities", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\System", "EnableActivityFeed"),
                RegOp.DeleteValue(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\System", "PublishUserActivities"),
                RegOp.DeleteValue(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\System", "UploadUserActivities"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\System", "EnableActivityFeed", 0)],
        },
        new TweakDef
        {
            Id = "priv-disable-location",
            Label = "Disable Location Tracking",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Prevents apps and Windows services from accessing your GPS/network location.",
            Description = "Disables location tracking for all apps and Windows services.",
            Tags = ["privacy", "location", "tracking"],
            RegistryKeys =
            [
                @"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\location",
                @"HKLM\SOFTWARE\Policies\Microsoft\Windows\LocationAndSensors",
            ],
            ApplyOps =
            [
                RegOp.SetString(@"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\location", "Value", "Deny"),
                RegOp.SetDword(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\LocationAndSensors", "DisableLocation", 1),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\location", "Value", "Allow"),
                RegOp.DeleteValue(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\LocationAndSensors", "DisableLocation"),
            ],
            DetectOps =
            [
                RegOp.CheckString(@"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\location", "Value", "Deny"),
            ],
        },
        new TweakDef
        {
            Id = "priv-disable-advertising-id",
            Label = "Disable Advertising ID",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Windows advertising ID used for cross-app ad targeting.",
            Tags = ["privacy", "advertising", "tracking"],
            RegistryKeys =
            [
                @"HKCU\Software\Microsoft\Windows\CurrentVersion\AdvertisingInfo",
                @"HKLM\SOFTWARE\Policies\Microsoft\Windows\AdvertisingInfo",
            ],
            ApplyOps =
            [
                RegOp.SetDword(@"HKCU\Software\Microsoft\Windows\CurrentVersion\AdvertisingInfo", "Enabled", 0),
                RegOp.SetDword(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\AdvertisingInfo", "DisabledByGroupPolicy", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKCU\Software\Microsoft\Windows\CurrentVersion\AdvertisingInfo", "Enabled", 1),
                RegOp.DeleteValue(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\AdvertisingInfo", "DisabledByGroupPolicy"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKCU\Software\Microsoft\Windows\CurrentVersion\AdvertisingInfo", "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "priv-disable-camera-access",
            Label = "Deny Camera Access (Apps)",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Denies camera access for UWP/Store apps by default.",
            Tags = ["privacy", "camera", "hardware"],
            RegistryKeys = [@"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\webcam"],
            ApplyOps =
            [
                RegOp.SetString(@"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\webcam", "Value", "Deny"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\webcam", "Value", "Allow"),
            ],
            DetectOps =
            [
                RegOp.CheckString(@"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\webcam", "Value", "Deny"),
            ],
        },
        new TweakDef
        {
            Id = "priv-disable-microphone-access",
            Label = "Deny Microphone Access (Apps)",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Denies microphone access for UWP/Store apps by default.",
            Tags = ["privacy", "microphone", "hardware"],
            RegistryKeys = [@"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\microphone"],
            ApplyOps =
            [
                RegOp.SetString(@"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\microphone", "Value", "Deny"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\microphone", "Value", "Allow"),
            ],
            DetectOps =
            [
                RegOp.CheckString(@"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\microphone", "Value", "Deny"),
            ],
        },
        new TweakDef
        {
            Id = "priv-disable-diagtrack",
            Label = "Disable DiagTrack (CEIP)",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Connected User Experiences and Telemetry (DiagTrack).",
            Tags = ["privacy", "telemetry", "diagtrack"],
            RegistryKeys = [@"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Diagnostics\DiagTrack"],
            ApplyOps = [RegOp.SetDword(@"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Diagnostics\DiagTrack", "ShowedToastAtLevel", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Diagnostics\DiagTrack", "ShowedToastAtLevel")],
            DetectOps = [RegOp.CheckDword(@"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Diagnostics\DiagTrack", "ShowedToastAtLevel", 1)],
        },
        new TweakDef
        {
            Id = "priv-disable-online-speech",
            Label = "Disable Online Speech Recognition",
            Category = "Privacy",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Stops sending voice data to Microsoft for online speech recognition.",
            Tags = ["privacy", "speech", "voice"],
            RegistryKeys = [@"HKCU\Software\Microsoft\Speech_OneCore\Settings\OnlineSpeechPrivacy"],
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\Microsoft\Speech_OneCore\Settings\OnlineSpeechPrivacy", "HasAccepted", 0)],
            RemoveOps = [RegOp.SetDword(@"HKCU\Software\Microsoft\Speech_OneCore\Settings\OnlineSpeechPrivacy", "HasAccepted", 1)],
            DetectOps = [RegOp.CheckDword(@"HKCU\Software\Microsoft\Speech_OneCore\Settings\OnlineSpeechPrivacy", "HasAccepted", 0)],
        },
        new TweakDef
        {
            Id = "priv-disable-inking-personalization",
            Label = "Disable Inking & Typing Personalization",
            Category = "Privacy",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows from collecting typing/inking data for personalization.",
            Tags = ["privacy", "inking", "typing"],
            RegistryKeys = [@"HKCU\Software\Microsoft\InputPersonalization", @"HKCU\Software\Microsoft\InputPersonalization\TrainedDataStore"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKCU\Software\Microsoft\InputPersonalization", "RestrictImplicitInkCollection", 1),
                RegOp.SetDword(@"HKCU\Software\Microsoft\InputPersonalization", "RestrictImplicitTextCollection", 1),
                RegOp.SetDword(@"HKCU\Software\Microsoft\InputPersonalization\TrainedDataStore", "HarvestContacts", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKCU\Software\Microsoft\InputPersonalization", "RestrictImplicitInkCollection", 0),
                RegOp.SetDword(@"HKCU\Software\Microsoft\InputPersonalization", "RestrictImplicitTextCollection", 0),
                RegOp.SetDword(@"HKCU\Software\Microsoft\InputPersonalization\TrainedDataStore", "HarvestContacts", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKCU\Software\Microsoft\InputPersonalization", "RestrictImplicitInkCollection", 1)],
        },
        new TweakDef
        {
            Id = "priv-privacy-disable-clipboard-history",
            Label = "Disable Clipboard History",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Windows clipboard history (Win+V). Prevents sensitive copied data from being stored. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["privacy", "clipboard", "history"],
            RegistryKeys = [@"HKLM\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\System", "AllowClipboardHistory", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\System", "AllowClipboardHistory")],
            DetectOps = [RegOp.CheckDword(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\System", "AllowClipboardHistory", 0)],
        },
        new TweakDef
        {
            Id = "priv-privacy-disable-online-speech",
            Label = "Disable Online Speech Recognition",
            Category = "Privacy",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables online speech recognition which sends voice data to Microsoft servers. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["privacy", "speech", "recognition", "telemetry"],
            RegistryKeys = [@"HKCU\Software\Microsoft\Speech_OneCore\Settings\OnlineSpeechPrivacy"],
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\Microsoft\Speech_OneCore\Settings\OnlineSpeechPrivacy", "HasAccepted", 0)],
            RemoveOps = [RegOp.SetDword(@"HKCU\Software\Microsoft\Speech_OneCore\Settings\OnlineSpeechPrivacy", "HasAccepted", 1)],
            DetectOps = [RegOp.CheckDword(@"HKCU\Software\Microsoft\Speech_OneCore\Settings\OnlineSpeechPrivacy", "HasAccepted", 0)],
        },
        new TweakDef
        {
            Id = "priv-privacy-disable-activity-history",
            Label = "Disable Activity History",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables Windows activity history and timeline via Group Policy. Prevents activity feed and user activity publishing. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["privacy", "activity", "history", "timeline", "policy"],
            RegistryKeys = [@"HKLM\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\System", "EnableActivityFeed", 0),
                RegOp.SetDword(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\System", "PublishUserActivities", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\System", "EnableActivityFeed"),
                RegOp.DeleteValue(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\System", "PublishUserActivities"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\System", "EnableActivityFeed", 0),
                RegOp.CheckDword(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\System", "PublishUserActivities", 0),
            ],
        },
        new TweakDef
        {
            Id = "priv-privacy-disable-tailored-experiences",
            Label = "Disable Tailored Experiences",
            Category = "Privacy",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Microsoft tailored experiences that use diagnostic data to personalise ads, tips, and recommendations. Default: enabled. Recommended: disabled.",
            Tags = ["privacy", "tailored", "diagnostic", "personalisation"],
            RegistryKeys = [@"HKCU\Software\Microsoft\Windows\CurrentVersion\Privacy"],
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\Microsoft\Windows\CurrentVersion\Privacy", "TailoredExperiencesWithDiagnosticDataEnabled", 0)],
            RemoveOps =
            [
                RegOp.SetDword(@"HKCU\Software\Microsoft\Windows\CurrentVersion\Privacy", "TailoredExperiencesWithDiagnosticDataEnabled", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKCU\Software\Microsoft\Windows\CurrentVersion\Privacy", "TailoredExperiencesWithDiagnosticDataEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "priv-privacy-disable-windows-tips",
            Label = "Disable Windows Tips & Suggestions",
            Category = "Privacy",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Windows tips, tricks, and 'Get started' suggestions that appear in Start menu, lock screen and notifications. Default: enabled. Recommended: disabled.",
            Tags = ["privacy", "tips", "suggestions", "content-delivery"],
            RegistryKeys = [@"HKCU\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKCU\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SoftLandingEnabled", 0),
                RegOp.SetDword(@"HKCU\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SubscribedContent-338389Enabled", 0),
                RegOp.SetDword(@"HKCU\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SubscribedContent-310093Enabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKCU\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SoftLandingEnabled", 1),
                RegOp.SetDword(@"HKCU\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SubscribedContent-338389Enabled", 1),
                RegOp.SetDword(@"HKCU\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SubscribedContent-310093Enabled", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKCU\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SoftLandingEnabled", 0)],
        },
        new TweakDef
        {
            Id = "priv-privacy-disable-app-launch-tracking",
            Label = "Disable App Launch Tracking",
            Category = "Privacy",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Prevents Windows from tracking which apps you launch to improve Start menu suggestions. Default: enabled. Recommended: disabled.",
            Tags = ["privacy", "tracking", "start-menu", "launch"],
            RegistryKeys = [@"HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackProgs", 0)],
            RemoveOps = [RegOp.SetDword(@"HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackProgs", 1)],
            DetectOps = [RegOp.CheckDword(@"HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackProgs", 0)],
        },
        new TweakDef
        {
            Id = "priv-privacy-disable-settings-suggestions",
            Label = "Disable Suggested Content in Settings",
            Category = "Privacy",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables suggested content and recommendations in the Windows Settings app. Default: enabled. Recommended: disabled.",
            Tags = ["privacy", "settings", "suggestions", "content-delivery"],
            RegistryKeys = [@"HKCU\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKCU\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SubscribedContent-338393Enabled", 0),
                RegOp.SetDword(@"HKCU\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SubscribedContent-353694Enabled", 0),
                RegOp.SetDword(@"HKCU\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SubscribedContent-353696Enabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKCU\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SubscribedContent-338393Enabled", 1),
                RegOp.SetDword(@"HKCU\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SubscribedContent-353694Enabled", 1),
                RegOp.SetDword(@"HKCU\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SubscribedContent-353696Enabled", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKCU\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SubscribedContent-338393Enabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "priv-disable-handwriting-sharing",
            Label = "Disable Handwriting Data Sharing",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables implicit ink/text collection and handwriting error reports. Prevents handwriting data from being shared with Microsoft. Default: Enabled. Recommended: Disabled.",
            Tags = ["privacy", "handwriting", "ink", "data-sharing"],
            RegistryKeys = [@"HKCU\Software\Microsoft\InputPersonalization", @"HKLM\SOFTWARE\Policies\Microsoft\Windows\HandwritingErrorReports"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKCU\Software\Microsoft\InputPersonalization", "RestrictImplicitInkCollection", 1),
                RegOp.SetDword(@"HKCU\Software\Microsoft\InputPersonalization", "RestrictImplicitTextCollection", 1),
                RegOp.SetDword(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\HandwritingErrorReports", "PreventHandwritingErrorReports", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKCU\Software\Microsoft\InputPersonalization", "RestrictImplicitInkCollection"),
                RegOp.DeleteValue(@"HKCU\Software\Microsoft\InputPersonalization", "RestrictImplicitTextCollection"),
                RegOp.DeleteValue(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\HandwritingErrorReports", "PreventHandwritingErrorReports"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKCU\Software\Microsoft\InputPersonalization", "RestrictImplicitInkCollection", 1),
                RegOp.CheckDword(@"HKCU\Software\Microsoft\InputPersonalization", "RestrictImplicitTextCollection", 1),
                RegOp.CheckDword(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\HandwritingErrorReports", "PreventHandwritingErrorReports", 1),
            ],
        },
        new TweakDef
        {
            Id = "priv-disable-launch-tracking",
            Label = "Disable App Launch Tracking (Policy)",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables User Activity Reporting (UAR) at the machine policy level. Prevents Windows from tracking application launches system-wide. Default: Enabled. Recommended: Disabled.",
            Tags = ["privacy", "tracking", "launch", "uar", "policy"],
            RegistryKeys = [@"HKLM\SOFTWARE\Policies\Microsoft\Windows\AppCompat"],
            ApplyOps = [RegOp.SetDword(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableUAR", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableUAR")],
            DetectOps = [RegOp.CheckDword(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableUAR", 1)],
        },
        new TweakDef
        {
            Id = "priv-disable-cross-device",
            Label = "Disable Cross-Device Experiences",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Connected Devices Platform (CDP) which enables cross-device experiences like phone-to-PC linking and shared clipboard. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["privacy", "cross-device", "cdp", "phone-link"],
            RegistryKeys = [@"HKLM\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\System", "EnableCdp", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\System", "EnableCdp")],
            DetectOps = [RegOp.CheckDword(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\System", "EnableCdp", 0)],
        },
        new TweakDef
        {
            Id = "priv-disable-app-diagnostics",
            Label = "Disable App Diagnostics Access",
            Category = "Privacy",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Denies applications access to diagnostic information about other apps. Protects inter-app data leakage. Default: Allow. Recommended: Deny.",
            Tags = ["privacy", "app-diagnostics", "consent", "capability"],
            RegistryKeys = [@"HKCU\Software\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\appDiagnostics"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKCU\Software\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\appDiagnostics",
                    "Value",
                    "Deny"
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKCU\Software\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\appDiagnostics", "Value"),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKCU\Software\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\appDiagnostics",
                    "Value",
                    "Deny"
                ),
            ],
        },
        new TweakDef
        {
            Id = "priv-disable-account-info-access",
            Label = "Disable Account Info Access",
            Category = "Privacy",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Denies applications access to user account information (name, picture). Improves privacy. Default: Allow. Recommended: Deny.",
            Tags = ["privacy", "account-info", "consent", "capability"],
            RegistryKeys = [@"HKCU\Software\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\userAccountInformation"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKCU\Software\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\userAccountInformation",
                    "Value",
                    "Deny"
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKCU\Software\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\userAccountInformation",
                    "Value"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKCU\Software\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\userAccountInformation",
                    "Value",
                    "Deny"
                ),
            ],
        },
        new TweakDef
        {
            Id = "priv-disable-notification-listener",
            Label = "Disable Notification Listener Access",
            Category = "Privacy",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Denies applications access to user notification content. Prevents apps from reading notification text. Default: Allow. Recommended: Deny.",
            Tags = ["privacy", "notifications", "listener", "consent"],
            RegistryKeys = [@"HKCU\Software\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\userNotificationListener"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKCU\Software\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\userNotificationListener",
                    "Value",
                    "Deny"
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKCU\Software\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\userNotificationListener",
                    "Value"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKCU\Software\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\userNotificationListener",
                    "Value",
                    "Deny"
                ),
            ],
        },
        new TweakDef
        {
            Id = "priv-disable-contacts-access",
            Label = "Block Apps from Accessing Contacts",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents apps from accessing your contacts list via the AppPrivacy policy. Value 2 = Force Deny. Default: Allow (0). Recommended: Force Deny.",
            Tags = ["privacy", "contacts", "appprivacy", "consent"],
            RegistryKeys = [@"HKLM\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy"],
            ApplyOps = [RegOp.SetDword(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsAccessContacts", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsAccessContacts")],
            DetectOps = [RegOp.CheckDword(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsAccessContacts", 2)],
        },
        new TweakDef
        {
            Id = "priv-disable-calendar-access",
            Label = "Block Apps from Accessing Calendar",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents apps from reading or modifying your calendar via the AppPrivacy policy. Value 2 = Force Deny. Default: Allow (0). Recommended: Force Deny.",
            Tags = ["privacy", "calendar", "appprivacy", "consent"],
            RegistryKeys = [@"HKLM\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy"],
            ApplyOps = [RegOp.SetDword(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsAccessCalendar", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsAccessCalendar")],
            DetectOps = [RegOp.CheckDword(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsAccessCalendar", 2)],
        },
        new TweakDef
        {
            Id = "priv-disable-radios-access",
            Label = "Block Apps from Controlling Radios (Bluetooth/Wi-Fi)",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents apps from toggling Bluetooth or Wi-Fi radios via the AppPrivacy policy. Value 2 = Force Deny. Default: Allow (0). Recommended: Force Deny.",
            Tags = ["privacy", "radios", "bluetooth", "wifi", "appprivacy", "consent"],
            RegistryKeys = [@"HKLM\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy"],
            ApplyOps = [RegOp.SetDword(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsAccessRadios", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsAccessRadios")],
            DetectOps = [RegOp.CheckDword(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsAccessRadios", 2)],
        },
        new TweakDef
        {
            Id = "priv-disable-ink-workspace",
            Label = "Disable Windows Ink Workspace",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Windows Ink Workspace feature via policy. Prevents the pen/stylus workspace from loading. Default: Enabled. Recommended: Disabled on non-tablet devices.",
            Tags = ["privacy", "ink", "workspace", "pen", "stylus", "policy"],
            RegistryKeys = [@"HKLM\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace"],
            ApplyOps = [RegOp.SetDword(@"HKLM\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace", "AllowWindowsInkWorkspace", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKLM\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace", "AllowWindowsInkWorkspace")],
            DetectOps = [RegOp.CheckDword(@"HKLM\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace", "AllowWindowsInkWorkspace", 0)],
        },
        new TweakDef
        {
            Id = "priv-disable-call-history",
            Label = "Block Apps from Accessing Call History",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets LetAppsAccessCallHistory=2 (Force Deny) via AppPrivacy policy. Prevents all apps from reading your phone call history. Default: Allow (0). Recommended: Force Deny.",
            Tags = ["privacy", "call-history", "phone", "appprivacy", "policy"],
            RegistryKeys = [@"HKLM\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy"],
            ApplyOps = [RegOp.SetDword(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsAccessCallHistory", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsAccessCallHistory")],
            DetectOps = [RegOp.CheckDword(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsAccessCallHistory", 2)],
        },
        new TweakDef
        {
            Id = "priv-disable-email-access",
            Label = "Block Apps from Accessing Email",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets LetAppsAccessEmail=2 (Force Deny) via AppPrivacy policy. Prevents all UWP apps from reading your email accounts and messages. Default: Allow (0). Recommended: Force Deny.",
            Tags = ["privacy", "email", "mail", "appprivacy", "policy"],
            RegistryKeys = [@"HKLM\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy"],
            ApplyOps = [RegOp.SetDword(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsAccessEmail", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsAccessEmail")],
            DetectOps = [RegOp.CheckDword(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsAccessEmail", 2)],
        },
        new TweakDef
        {
            Id = "priv-disable-tasks-access",
            Label = "Block Apps from Accessing Tasks",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets LetAppsAccessTasks=2 (Force Deny) via AppPrivacy policy. Prevents all apps from reading or modifying your task/to-do lists. Default: Allow (0). Recommended: Force Deny.",
            Tags = ["privacy", "tasks", "todo", "appprivacy", "policy"],
            RegistryKeys = [@"HKLM\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy"],
            ApplyOps = [RegOp.SetDword(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsAccessTasks", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsAccessTasks")],
            DetectOps = [RegOp.CheckDword(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsAccessTasks", 2)],
        },
        new TweakDef
        {
            Id = "priv-disable-messaging-access",
            Label = "Block Apps from Accessing SMS/MMS Messages",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets LetAppsAccessMessaging=2 (Force Deny) via AppPrivacy policy. Prevents all apps from reading or sending SMS/MMS messages. Default: Allow (0). Recommended: Force Deny.",
            Tags = ["privacy", "messaging", "sms", "mms", "appprivacy", "policy"],
            RegistryKeys = [@"HKLM\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy"],
            ApplyOps = [RegOp.SetDword(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsAccessMessaging", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsAccessMessaging")],
            DetectOps = [RegOp.CheckDword(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsAccessMessaging", 2)],
        },
        new TweakDef
        {
            Id = "priv-disable-settings-sync",
            Label = "Disable Settings Sync to Microsoft Cloud",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables syncing Windows settings (theme, passwords, language) to Microsoft cloud. Default: enabled.",
            Tags = ["privacy", "sync", "cloud", "settings"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SettingSync"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SettingSync", "DisableSettingSync", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SettingSync", "DisableSettingSync")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SettingSync", "DisableSettingSync", 2)],
        },
        new TweakDef
        {
            Id = "priv-disable-input-personalization",
            Label = "Disable Input Personalization (Inking & Typing)",
            Category = "Privacy",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables online speech recognition, inking, and typing personalization. Prevents sending typing data to Microsoft. Default: enabled.",
            Tags = ["privacy", "input", "inking", "typing", "personalization"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\InputPersonalization"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\InputPersonalization", "RestrictImplicitInkCollection", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\InputPersonalization", "RestrictImplicitInkCollection")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\InputPersonalization", "RestrictImplicitInkCollection", 1)],
        },
        new TweakDef
        {
            Id = "priv-disable-feedback-notifications",
            Label = "Disable Feedback Notifications",
            Category = "Privacy",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets feedback frequency to never. Prevents Windows from asking for feedback. Default: automatic.",
            Tags = ["privacy", "feedback", "notifications", "disable"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Siuf\Rules"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Siuf\Rules", "NumberOfSIUFInPeriod", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Siuf\Rules", "NumberOfSIUFInPeriod")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Siuf\Rules", "NumberOfSIUFInPeriod", 0)],
        },
        new TweakDef
        {
            Id = "priv-disable-tailored-experiences",
            Label = "Disable Tailored Experiences",
            Category = "Privacy",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables tailored experiences with diagnostic data. Prevents Microsoft from using your data to customize tips and recommendations. Default: enabled.",
            Tags = ["privacy", "tailored", "experiences", "recommendations"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Privacy"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Privacy",
                    "TailoredExperiencesWithDiagnosticDataEnabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Privacy",
                    "TailoredExperiencesWithDiagnosticDataEnabled",
                    1
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Privacy",
                    "TailoredExperiencesWithDiagnosticDataEnabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "priv-disable-web-search-in-start",
            Label = "Disable Web Search Results in Start Menu",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Start menu search from querying Bing web results. Keeps search results local only. Faster and more private.",
            Tags = ["privacy", "search", "bing", "web", "start"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "DisableWebSearch", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "DisableWebSearch")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "DisableWebSearch", 1)],
        },
        new TweakDef
        {
            Id = "priv-disable-cloud-content-search",
            Label = "Disable Cloud Content Search (OneDrive/Outlook)",
            Category = "Privacy",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows Search from indexing and returning results from OneDrive and Outlook cloud content.",
            Tags = ["privacy", "search", "cloud", "onedrive"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsAADCloudSearchEnabled", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsMSACloudSearchEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsAADCloudSearchEnabled"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsMSACloudSearchEnabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsAADCloudSearchEnabled", 0),
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsMSACloudSearchEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "priv-disable-suggest-ways-to-finish-setup",
            Label = "Disable 'Suggest Ways to Finish Setting Up' Prompt",
            Category = "Privacy",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the 'Suggest ways to finish setting up my device' prompt that pushes Microsoft account linking and OneDrive setup.",
            Tags = ["privacy", "setup", "nag", "microsoft"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-310093Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-310093Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-310093Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "priv-disable-app-launch-tracking",
            Label = "Disable App Launch Tracking",
            Category = "Privacy",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows from tracking which apps you launch to improve Start menu suggestions. Improves privacy.",
            Tags = ["privacy", "tracking", "launch", "start"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackProgs", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackProgs", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackProgs", 0)],
        },
        new TweakDef
        {
            Id = "priv-disable-handwriting-error-reports",
            Label = "Disable Handwriting Error Reporting",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Windows from sending handwriting recognition error reports to Microsoft. Default: enabled.",
            Tags = ["privacy", "handwriting", "errors", "telemetry"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HandwritingErrorReports"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HandwritingErrorReports",
                    "PreventHandwritingErrorReports",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HandwritingErrorReports",
                    "PreventHandwritingErrorReports"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HandwritingErrorReports",
                    "PreventHandwritingErrorReports",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "priv-disable-customer-experience-program",
            Label = "Opt Out of Customer Experience Improvement Program",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Opts out of the Windows Customer Experience Improvement Program (CEIP). Prevents anonymous usage data collection.",
            Tags = ["privacy", "ceip", "telemetry", "optout"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient\Windows"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient\Windows", "CEIPEnable", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient\Windows", "CEIPEnable")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient\Windows", "CEIPEnable", 0)],
        },
        new TweakDef
        {
            Id = "priv-disable-inventory-collector",
            Label = "Disable Application Inventory Collector",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Application Inventory Collector which sends a list of installed programs to Microsoft. Default: enabled.",
            Tags = ["privacy", "inventory", "apps", "telemetry"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableInventory", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableInventory")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableInventory", 1)],
        },
        new TweakDef
        {
            Id = "priv-disable-motion-access",
            Label = "Block Apps from Accessing Motion Sensors",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets LetAppsAccessMotion=2 (Force Deny) in AppPrivacy policy. Prevents all apps from reading accelerometer, gyroscope, and other motion sensor data.",
            Tags = ["privacy", "motion", "sensors", "app-access"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsAccessMotion", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsAccessMotion")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsAccessMotion", 2)],
        },
        new TweakDef
        {
            Id = "priv-disable-phone-call-access",
            Label = "Block Apps from Making Phone Calls",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets LetAppsAccessPhone=2 (Force Deny) in AppPrivacy policy. Prevents apps from initiating or reading phone calls via the Windows Phone Call subsystem.",
            Tags = ["privacy", "phone", "calls", "app-access"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsAccessPhone", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsAccessPhone")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsAccessPhone", 2)],
        },
        new TweakDef
        {
            Id = "priv-disable-trusted-devices-access",
            Label = "Block Apps from Accessing Trusted Devices",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets LetAppsAccessTrustedDevices=2 (Force Deny). Prevents apps from communicating with paired trusted peripherals such as smartcards and wearables.",
            Tags = ["privacy", "trusted-devices", "peripherals", "app-access"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsAccessTrustedDevices", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsAccessTrustedDevices")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsAccessTrustedDevices", 2)],
        },
        new TweakDef
        {
            Id = "priv-disable-background-app-access",
            Label = "Block Apps from Running in Background",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets LetAppsRunInBackground=2 (Force Deny) via AppPrivacy policy. Prevents UWP apps from executing background tasks or receiving push updates when not in use.",
            Tags = ["privacy", "background", "apps", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsRunInBackground", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsRunInBackground")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsRunInBackground", 2)],
        },
        new TweakDef
        {
            Id = "priv-disable-sync-with-devices",
            Label = "Block Apps from Syncing with Unpaired Devices",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets LetAppsSyncWithDevices=2 (Force Deny). Prevents apps from synchronising data with USB, Bluetooth, and other peripheral devices without user consent.",
            Tags = ["privacy", "sync", "devices", "bluetooth"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsSyncWithDevices", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsSyncWithDevices")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsSyncWithDevices", 2)],
        },
        new TweakDef
        {
            Id = "priv-disable-documents-app-access",
            Label = "Block Apps from Accessing Documents Library",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets LetAppsAccessDocumentsLibrary=2 (Force Deny). Prevents UWP apps from reading or writing files in the user's Documents folder without explicit per-file consent.",
            Tags = ["privacy", "documents", "library", "app-access"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsAccessDocumentsLibrary", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsAccessDocumentsLibrary")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsAccessDocumentsLibrary", 2)],
        },
        new TweakDef
        {
            Id = "priv-disable-pictures-app-access",
            Label = "Block Apps from Accessing Pictures Library",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets LetAppsAccessPicturesLibrary=2 (Force Deny). Prevents UWP apps from browsing or modifying files in the Pictures library.",
            Tags = ["privacy", "pictures", "library", "app-access"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsAccessPicturesLibrary", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsAccessPicturesLibrary")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsAccessPicturesLibrary", 2)],
        },
        new TweakDef
        {
            Id = "priv-disable-videos-app-access",
            Label = "Block Apps from Accessing Videos Library",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets LetAppsAccessVideosLibrary=2 (Force Deny). Prevents UWP apps from reading or writing files in the Videos library.",
            Tags = ["privacy", "videos", "library", "app-access"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsAccessVideosLibrary", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsAccessVideosLibrary")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsAccessVideosLibrary", 2)],
        },
        new TweakDef
        {
            Id = "priv-disable-voice-activation-apps",
            Label = "Block Apps from Using Voice Activation",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets LetAppsActivateWithVoice=2 (Force Deny). Prevents apps from waking on a voice keyword while the screen is on.",
            Tags = ["privacy", "voice", "activation", "microphone"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsActivateWithVoice", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsActivateWithVoice")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsActivateWithVoice", 2)],
        },
        new TweakDef
        {
            Id = "priv-disable-voice-above-lock",
            Label = "Block Voice Activation Above Lock Screen",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets LetAppsActivateWithVoiceAboveLock=2 (Force Deny). Prevents voice assistants from activating when the screen is locked, stopping passive microphone listening.",
            Tags = ["privacy", "voice", "lock-screen", "microphone"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsActivateWithVoiceAboveLock", 2)],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsActivateWithVoiceAboveLock"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsActivateWithVoiceAboveLock", 2),
            ],
        },
        new TweakDef
        {
            Id = "priv-disable-notification-app-access",
            Label = "Block Apps from Accessing Notifications",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets LetAppsAccessNotifications=2 (Force Deny). Prevents apps from reading the system notification feed, stopping cross-app notification snooping.",
            Tags = ["privacy", "notifications", "app-access"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsAccessNotifications", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsAccessNotifications")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsAccessNotifications", 2)],
        },
        new TweakDef
        {
            Id = "priv-disable-gaze-input-access",
            Label = "Block Apps from Accessing Gaze / Eye-Tracking Input",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets LetAppsAccessGazeInput=2 (Force Deny). Prevents apps from reading eye-tracking or gaze input data from supported hardware.",
            Tags = ["privacy", "gaze", "eye-tracking", "input", "app-access"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsAccessGazeInput", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsAccessGazeInput")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsAccessGazeInput", 2)],
        },
        new TweakDef
        {
            Id = "priv-disable-publish-user-activities",
            Label = "Disable Publishing User Activities to Timeline",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets PublishUserActivities=0 in Windows System policy. Stops the OS from publishing app usage events to the cross-device Timeline/Activity feed, keeping local activity history private.",
            Tags = ["privacy", "activity-feed", "timeline", "user-data"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "PublishUserActivities", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "PublishUserActivities")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "PublishUserActivities", 0)],
        },
    ];
}

// ── Merged from WindowsRecall.cs ──────────────────────────────────────────────────

internal static class WindowsRecall
{
    private const string LmKey = @"HKEY_LOCAL_MACHINE";
    private const string CuKey = @"HKEY_CURRENT_USER";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "recall-disable-recall",
            Label = "Disable Windows Recall",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 26100,
            Description = "Disables Windows Recall (AI-powered timeline snapshots) system-wide via Group Policy.",
            Tags = ["recall", "ai", "privacy", "copilot-plus", "snapshots"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\WindowsAI"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "DisableAIDataAnalysis", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "DisableAIDataAnalysis")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "DisableAIDataAnalysis", 1)],
        },
        new TweakDef
        {
            Id = "recall-disable-saving-snapshots",
            Label = "Disable Recall Snapshot Saving",
            Category = "Privacy",
            NeedsAdmin = false,
            CorpSafe = true,
            MinBuild = 26100,
            Description = "Disables saving of Recall snapshots for the current user.",
            Tags = ["recall", "ai", "privacy", "snapshots"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisableRecallSaving", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisableRecallSaving")],
            DetectOps = [RegOp.CheckDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisableRecallSaving", 1)],
        },
        new TweakDef
        {
            Id = "recall-disable-ai-suggestions",
            Label = "Disable AI Suggestions in Start",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            Description = "Disables AI-powered suggested content and recommendations in Start menu.",
            Tags = ["recall", "ai", "start-menu", "suggestions"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\WindowsAI"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "TurnOffWindowsCopilot", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "TurnOffWindowsCopilot")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "TurnOffWindowsCopilot", 1)],
        },
        new TweakDef
        {
            Id = "recall-disable-semantic-indexing",
            Label = "Disable Semantic Indexing",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 26100,
            Description = "Disables the AI semantic indexing component used by Recall and enhanced search.",
            Tags = ["recall", "ai", "indexing", "performance", "privacy"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\WindowsAI"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "AllowSemanticIndexing", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "AllowSemanticIndexing")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "AllowSemanticIndexing", 0)],
        },
        new TweakDef
        {
            Id = "recall-disable-cocreator",
            Label = "Disable Cocreator in Paint",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            Description = "Disables the AI Cocreator feature in Microsoft Paint.",
            Tags = ["recall", "ai", "paint", "cocreator"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Paint"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Paint", "DisableCocreator", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Paint", "DisableCocreator")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Paint", "DisableCocreator", 1)],
        },
        new TweakDef
        {
            Id = "recall-disable-image-creator",
            Label = "Disable Image Creator in Paint",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            Description = "Disables the AI Image Creator feature in Microsoft Paint.",
            Tags = ["recall", "ai", "paint", "image-creator"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Paint"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Paint", "DisableImageCreator", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Paint", "DisableImageCreator")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Paint", "DisableImageCreator", 1)],
        },
        new TweakDef
        {
            Id = "recall-disable-generative-fill",
            Label = "Disable Generative Fill in Photos",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            Description = "Disables the AI generative fill/erase feature in Microsoft Photos.",
            Tags = ["recall", "ai", "photos", "generative"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Photos"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Photos", "DisableGenerativeFill", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Photos", "DisableGenerativeFill")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Photos", "DisableGenerativeFill", 1)],
        },
        new TweakDef
        {
            Id = "recall-disable-ai-in-notepad",
            Label = "Disable AI Features in Notepad",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            Description = "Disables AI-powered rewrite and summarize features in Windows Notepad.",
            Tags = ["recall", "ai", "notepad", "copilot"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Notepad"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Notepad", "DisableAI", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Notepad", "DisableAI")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Notepad", "DisableAI", 1)],
        },
        new TweakDef
        {
            Id = "recall-disable-web-content-eval",
            Label = "Disable Web Content Evaluation",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            Description = "Prevents Windows from sending web content to Microsoft for AI analysis.",
            Tags = ["recall", "ai", "privacy", "web", "telemetry"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\WindowsAI"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "DisableWebContentEvaluation", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "DisableWebContentEvaluation")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\WindowsAI", "DisableWebContentEvaluation", 1)],
        },
        new TweakDef
        {
            Id = "recall-disable-cross-device-resume",
            Label = "Disable Cross-Device Resume (AI)",
            Category = "Privacy",
            NeedsAdmin = false,
            CorpSafe = true,
            MinBuild = 22621,
            Description = "Disables AI-powered cross-device activity resume and handoff.",
            Tags = ["recall", "ai", "cross-device", "privacy", "sync"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\CDP"],
            ApplyOps = [RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\CDP", "CdpSessionUserAuthzPolicy", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\CDP", "CdpSessionUserAuthzPolicy")],
            DetectOps = [RegOp.CheckDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\CDP", "CdpSessionUserAuthzPolicy", 0)],
        },
        new TweakDef
        {
            Id = "recall-disable-ai-search-highlights",
            Label = "Disable AI Search Highlights",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            Description = "Disables AI-generated search highlights and trending content in Windows Search.",
            Tags = ["recall", "ai", "search", "highlights", "privacy"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "EnableDynamicContentInWSB", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "EnableDynamicContentInWSB")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "EnableDynamicContentInWSB", 0)],
        },
        new TweakDef
        {
            Id = "recall-disable-inking-and-typing-personalization",
            Label = "Disable Inking & Typing AI Personalization",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables AI-powered inking and typing personalization that sends data to Microsoft.",
            Tags = ["recall", "ai", "privacy", "typing", "inking", "personalization"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\InputPersonalization"],
            ApplyOps =
            [
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\InputPersonalization", "AllowInputPersonalization", 0),
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\InputPersonalization", "RestrictImplicitInkCollection", 1),
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\InputPersonalization", "RestrictImplicitTextCollection", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\InputPersonalization", "AllowInputPersonalization"),
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\InputPersonalization", "RestrictImplicitInkCollection"),
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\InputPersonalization", "RestrictImplicitTextCollection"),
            ],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\InputPersonalization", "AllowInputPersonalization", 0)],
        },
        new TweakDef
        {
            Id = "recall-disable-activity-history",
            Label = "Disable Activity History",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            Description = "Disables activity history collection and timeline features used by Recall.",
            Tags = ["recall", "ai", "privacy", "activity", "timeline"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps =
            [
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\System", "EnableActivityFeed", 0),
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\System", "PublishUserActivities", 0),
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\System", "UploadUserActivities", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\System", "EnableActivityFeed"),
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\System", "PublishUserActivities"),
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\System", "UploadUserActivities"),
            ],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\System", "EnableActivityFeed", 0)],
        },
        new TweakDef
        {
            Id = "recall-disable-voice-activation",
            Label = "Disable Voice Activation for AI",
            Category = "Privacy",
            NeedsAdmin = false,
            CorpSafe = true,
            MinBuild = 22621,
            Description = "Prevents AI assistants from listening for voice activation keywords.",
            Tags = ["recall", "ai", "privacy", "voice", "microphone"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Speech_OneCore\Settings\VoiceActivation\UserPreferenceForAllApps"],
            ApplyOps =
            [
                RegOp.SetDword(
                    $@"{CuKey}\Software\Microsoft\Speech_OneCore\Settings\VoiceActivation\UserPreferenceForAllApps",
                    "AgentActivationEnabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    $@"{CuKey}\Software\Microsoft\Speech_OneCore\Settings\VoiceActivation\UserPreferenceForAllApps",
                    "AgentActivationEnabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    $@"{CuKey}\Software\Microsoft\Speech_OneCore\Settings\VoiceActivation\UserPreferenceForAllApps",
                    "AgentActivationEnabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "recall-disable-online-tips",
            Label = "Disable Online Tips & Suggestions",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            Description = "Disables cloud-based tips and suggestions throughout Windows.",
            Tags = ["recall", "ai", "tips", "suggestions", "cloud"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "AllowOnlineTips", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\Policies\Explorer", "AllowOnlineTips")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "AllowOnlineTips", 0)],
        },
        new TweakDef
        {
            Id = "recall-disable-copilot-key",
            Label = "Disable Copilot Keyboard Key",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            Description = "Disables the dedicated Copilot key on supported keyboards.",
            Tags = ["recall", "ai", "copilot", "keyboard", "hardware"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisableCopilotKey", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisableCopilotKey")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisableCopilotKey", 1)],
        },
        new TweakDef
        {
            Id = "recall-disable-suggested-actions",
            Label = "Disable AI Suggested Actions",
            Category = "Privacy",
            NeedsAdmin = false,
            CorpSafe = true,
            MinBuild = 22621,
            Description = "Disables AI-powered suggested actions when copying dates, phone numbers, or addresses.",
            Tags = ["recall", "ai", "clipboard", "suggestions"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\SmartActionPlatform\SmartClipboard"],
            ApplyOps = [RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\SmartActionPlatform\SmartClipboard", "Disabled", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\SmartActionPlatform\SmartClipboard", "Disabled")],
            DetectOps = [RegOp.CheckDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\SmartActionPlatform\SmartClipboard", "Disabled", 1)],
        },
        new TweakDef
        {
            Id = "recall-disable-publish-user-activities",
            Label = "Disable Publishing User Activities (HKCU)",
            Category = "Privacy",
            NeedsAdmin = false,
            CorpSafe = true,
            MinBuild = 17763,
            Description = "Disables user-level Activity Feed and user activity publishing tracked in HKCU for Timeline and Jump Lists.",
            Tags = ["recall", "ai", "activity", "history", "privacy", "timeline"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Privacy"],
            ApplyOps =
            [
                RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Privacy", "EnableActivityFeed", 0),
                RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Privacy", "PublishUserActivities", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Privacy", "EnableActivityFeed"),
                RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Privacy", "PublishUserActivities"),
            ],
            DetectOps = [RegOp.CheckDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Privacy", "EnableActivityFeed", 0)],
        },
        new TweakDef
        {
            Id = "recall-disable-cross-device-clipboard",
            Label = "Disable Cross-Device Clipboard Sync",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 17763,
            Description = "Disables cross-device clipboard synchronization that shares clipboard content across Windows devices.",
            Tags = ["recall", "ai", "clipboard", "cloud", "privacy", "sync", "cross-device"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\System", "AllowCrossDeviceClipboard", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\System", "AllowCrossDeviceClipboard")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\System", "AllowCrossDeviceClipboard", 0)],
        },
        new TweakDef
        {
            Id = "recall-disable-typing-insights",
            Label = "Disable Typing Insights Collection",
            Category = "Privacy",
            NeedsAdmin = false,
            CorpSafe = true,
            MinBuild = 19041,
            Description = "Disables Typing Insights, which collects text input data to improve autocorrect and predictions.",
            Tags = ["recall", "ai", "typing", "personalization", "privacy", "input"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Input\Settings"],
            ApplyOps = [RegOp.SetDword($@"{CuKey}\Software\Microsoft\Input\Settings", "InsightsEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\Input\Settings", "InsightsEnabled")],
            DetectOps = [RegOp.CheckDword($@"{CuKey}\Software\Microsoft\Input\Settings", "InsightsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "recall-disable-taskbar-ai-widget-content",
            Label = "Disable AI Dynamic Content in Taskbar Widgets",
            Category = "Privacy",
            NeedsAdmin = false,
            CorpSafe = true,
            MinBuild = 22621,
            Description = "Disables AI-powered dynamic and personalized content displayed in the Taskbar widgets panel.",
            Tags = ["recall", "ai", "taskbar", "widgets", "news", "dynamic"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Feeds\DSB"],
            ApplyOps = [RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Feeds\DSB", "ShowDynamicContent", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Feeds\DSB", "ShowDynamicContent")],
            DetectOps = [RegOp.CheckDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Feeds\DSB", "ShowDynamicContent", 0)],
        },
        new TweakDef
        {
            Id = "recall-disable-cloud-search-results",
            Label = "Disable Cloud Search in Windows Search",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 17763,
            Description = "Disables cloud-powered search results from Bing and OneDrive inside Windows Search.",
            Tags = ["recall", "ai", "search", "cloud", "bing", "privacy", "onedrive"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowCloudSearch", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowCloudSearch")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowCloudSearch", 0)],
        },
        new TweakDef
        {
            Id = "recall-disable-voice-data-collection",
            Label = "Disable Online Speech Recognition Data Upload",
            Category = "Privacy",
            NeedsAdmin = false,
            CorpSafe = true,
            MinBuild = 17763,
            Description = "Opts out of uploading voice samples to Microsoft for online speech recognition improvement.",
            Tags = ["recall", "ai", "speech", "voice", "privacy", "microphone"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Speech_OneCore\Settings\OnlineSpeechPrivacy"],
            ApplyOps = [RegOp.SetDword($@"{CuKey}\Software\Microsoft\Speech_OneCore\Settings\OnlineSpeechPrivacy", "HasAccepted", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\Speech_OneCore\Settings\OnlineSpeechPrivacy", "HasAccepted")],
            DetectOps = [RegOp.CheckDword($@"{CuKey}\Software\Microsoft\Speech_OneCore\Settings\OnlineSpeechPrivacy", "HasAccepted", 0)],
        },
        new TweakDef
        {
            Id = "recall-disable-content-delivery-features",
            Label = "Disable ContentDelivery Feature Management",
            Category = "Privacy",
            NeedsAdmin = false,
            CorpSafe = true,
            MinBuild = 19041,
            Description = "Disables ContentDeliveryManager feature management, which enables future AI-driven content pushes.",
            Tags = ["recall", "ai", "content-delivery", "suggestions", "privacy", "advertising"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps = [RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "FeatureManagementEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "FeatureManagementEnabled")],
            DetectOps =
            [
                RegOp.CheckDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "FeatureManagementEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "recall-disable-spotlight-on-settings",
            Label = "Disable Windows Spotlight in Settings App",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 19041,
            Description = "Disables AI/Spotlight-powered content suggestions embedded in the Windows Settings application.",
            Tags = ["recall", "ai", "spotlight", "settings", "advertising", "suggestions"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\CloudContent"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsSpotlightOnSettings", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsSpotlightOnSettings")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsSpotlightOnSettings", 1)],
        },
    ];
}

// === Merged from: TelemetryAdvanced.cs ===


internal static class TelemetryAdvanced
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "telem-disable-diag-optin",
            Label = "Block Diagnostic Data Settings Changes",
            Category = "Telemetry Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the diagnostic data viewer and prevents users from changing opt-in level via Settings. Default: allowed. Recommended: 1 (blocked).",
            Tags = ["telemetry", "diagnostic", "settings", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "DisableDiagnosticDataViewer", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "DisableOneSettingsSyncDiag", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "DisableDiagnosticDataViewer"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "DisableOneSettingsSyncDiag"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "DisableDiagnosticDataViewer", 1),
            ],
        },
        new TweakDef
        {
            Id = "telem-disable-app-telemetry",
            Label = "Disable App Telemetry (Steps Recorder + Inventory)",
            Category = "Telemetry Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Steps Recorder (UAR) and Application Inventory collection. Reduces background telemetry. Default: enabled. Recommended: disabled.",
            Tags = ["telemetry", "app", "steps-recorder", "inventory"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableUAR", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableInventory", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableUAR"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableInventory"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableInventory", 1)],
        },
        new TweakDef
        {
            Id = "telem-disable-handwriting",
            Label = "Disable Handwriting Data Sharing",
            Category = "Telemetry Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents handwriting recognition data and error reports from being sent to Microsoft. Default: allowed. Recommended: 1 (blocked).",
            Tags = ["telemetry", "handwriting", "privacy", "tablet"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TabletPC"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TabletPC", "PreventHandwritingDataSharing", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TabletPC", "PreventHandwritingErrorReports", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TabletPC", "PreventHandwritingDataSharing"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TabletPC", "PreventHandwritingErrorReports"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TabletPC", "PreventHandwritingDataSharing", 1)],
        },
        new TweakDef
        {
            Id = "telem-disable-feedback",
            Label = "Disable Feedback Notifications",
            Category = "Telemetry Advanced",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets feedback frequency to 0 (never). Stops 'Rate Windows' and similar feedback prompts. Default: automatic. Recommended: 0 (never).",
            Tags = ["telemetry", "feedback", "notifications", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Siuf\Rules"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Siuf\Rules", "NumberOfSIUFInPeriod", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Siuf\Rules", "NumberOfSIUFInPeriod")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Siuf\Rules", "NumberOfSIUFInPeriod", 0)],
        },
        new TweakDef
        {
            Id = "telem-disable-input-telemetry",
            Label = "Disable Typing/Inking Telemetry",
            Category = "Telemetry Advanced",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables collection of typing and inking data for improving language recognition. Default: 1 (enabled). Recommended: 0 (disabled).",
            Tags = ["telemetry", "typing", "inking", "input", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Input\TIPC"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Input\TIPC", "Enabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Input\TIPC", "Enabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Input\TIPC", "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "telem-disable-tailored-experiences",
            Label = "Disable Tailored Experiences",
            Category = "Telemetry Advanced",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Prevents Windows from using diagnostic data to provide personalized tips, ads, and recommendations. Default: allowed. Recommended: 1 (disabled).",
            Tags = ["telemetry", "tailored", "suggestions", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\CloudContent"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\CloudContent",
                    "DisableTailoredExperiencesWithDiagnosticData",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\CloudContent",
                    "DisableTailoredExperiencesWithDiagnosticData"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\CloudContent",
                    "DisableTailoredExperiencesWithDiagnosticData",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "telem-disable-inventory-collector",
            Label = "Disable Inventory Collector",
            Category = "Telemetry Advanced",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Inventory Collector that sends application/driver data to Microsoft. Default: Enabled. Recommended: Disabled.",
            Tags = ["telemetry", "inventory", "collector", "appcompat"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableInventory", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableInventory", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableInventory", 1)],
        },
        new TweakDef
        {
            Id = "telem-disable-sqm-upload",
            Label = "Disable SQM Telemetry Upload",
            Category = "Telemetry Advanced",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Software Quality Metrics (SQM) CEIPEnable key, preventing telemetry data from being uploaded to Microsoft. Default: Enabled. Recommended: Disabled.",
            Tags = ["telemetry", "sqm", "ceip", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient", "CEIPEnable", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient", "CEIPEnable")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient", "CEIPEnable", 0)],
        },
        new TweakDef
        {
            Id = "telem-disable-mrt-report",
            Label = "Disable MRT Infection Reporting",
            Category = "Telemetry Advanced",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Prevents the Malicious Software Removal Tool from reporting infection information to Microsoft. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["telemetry", "mrt", "malware", "reporting", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MRT"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MRT", "DontReportInfectionInformation", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MRT", "DontReportInfectionInformation")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MRT", "DontReportInfectionInformation", 1)],
        },
        new TweakDef
        {
            Id = "telem-disable-speech-model-update",
            Label = "Disable Speech Model Automatic Update",
            Category = "Telemetry Advanced",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Prevents Windows from automatically downloading updated speech recognition models. Reduces background network activity. Default: Enabled. Recommended: Disabled.",
            Tags = ["telemetry", "speech", "model", "update", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Speech"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Speech", "AllowSpeechModelUpdate", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Speech", "AllowSpeechModelUpdate")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Speech", "AllowSpeechModelUpdate", 0)],
        },
        new TweakDef
        {
            Id = "telem-disable-license-telemetry",
            Label = "Disable License Telemetry (NoGenTicket)",
            Category = "Telemetry Advanced",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets NoGenTicket to prevent the Software Protection Platform from sending licensing telemetry to Microsoft. Default: Disabled. Recommended: Enabled for privacy.",
            Tags = ["telemetry", "license", "spp", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SoftwareProtectionPlatform"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SoftwareProtectionPlatform", "NoGenTicket", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SoftwareProtectionPlatform", "NoGenTicket")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SoftwareProtectionPlatform", "NoGenTicket", 1)],
        },
        new TweakDef
        {
            Id = "telem-disable-ncsi-probing",
            Label = "Disable NCSI Active Probe",
            Category = "Telemetry Advanced",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Network Connectivity Status Indicator active probe that contacts Microsoft servers to check internet connectivity on login. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["telemetry", "ncsi", "probe", "network", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkConnectivityStatusIndicator"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkConnectivityStatusIndicator", "NoActiveProbe", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkConnectivityStatusIndicator", "NoActiveProbe"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkConnectivityStatusIndicator", "NoActiveProbe", 1),
            ],
        },
        new TweakDef
        {
            Id = "telem-disable-customer-experience",
            Label = "Disable Customer Experience Improvement Program",
            Category = "Telemetry Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables CEIP data collection. Hardware/software usage data won't be sent to Microsoft. Default: enabled.",
            Tags = ["telemetry", "ceip", "data-collection", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient\Windows"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient\Windows", "CEIPEnable", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient\Windows", "CEIPEnable")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient\Windows", "CEIPEnable", 0)],
        },
        new TweakDef
        {
            Id = "telem-disable-handwriting-data",
            Label = "Disable Handwriting Data Sharing",
            Category = "Telemetry Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables sending handwriting error reports and data to Microsoft. Default: enabled.",
            Tags = ["telemetry", "handwriting", "data", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TabletPC"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TabletPC", "PreventHandwritingDataSharing", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TabletPC", "PreventHandwritingDataSharing")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TabletPC", "PreventHandwritingDataSharing", 1)],
        },
        new TweakDef
        {
            Id = "telem-disable-kms-client-emulation",
            Label = "Disable KMS Client Online AVS Validation",
            Category = "Telemetry Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables automatic KMS client activation validation pings. Reduces outbound telemetry traffic. Default: enabled.",
            Tags = ["telemetry", "kms", "activation", "validation"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\CurrentVersion\Software Protection Platform"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\CurrentVersion\Software Protection Platform",
                    "NoGenTicket",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\CurrentVersion\Software Protection Platform",
                    "NoGenTicket"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\CurrentVersion\Software Protection Platform",
                    "NoGenTicket",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "telem-disable-advertising-id",
            Label = "Disable Advertising ID (Policy)",
            Category = "Telemetry Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the per-user advertising ID via Group Policy. Prevents personalised ad tracking across apps. Default: enabled.",
            Tags = ["telemetry", "advertising-id", "privacy", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AdvertisingInfo"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AdvertisingInfo", "DisabledByGroupPolicy", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AdvertisingInfo", "DisabledByGroupPolicy")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AdvertisingInfo", "DisabledByGroupPolicy", 1)],
        },
        new TweakDef
        {
            Id = "telem-disable-diagtrack-autologger",
            Label = "Disable DiagTrack AutoLogger",
            Category = "Telemetry Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the DiagTrack/Autologger-Diagtrack-Listener ETW session. Prevents early-boot telemetry logging. Default: enabled.",
            Tags = ["telemetry", "diagtrack", "autologger", "etw"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\WMI\Autologger\AutoLogger-Diagtrack-Listener"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\WMI\Autologger\AutoLogger-Diagtrack-Listener", "Start", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\WMI\Autologger\AutoLogger-Diagtrack-Listener", "Start", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\WMI\Autologger\AutoLogger-Diagtrack-Listener", "Start", 0),
            ],
        },
        new TweakDef
        {
            Id = "telem-disable-win-error-reporting",
            Label = "Disable Windows Error Reporting (Policy)",
            Category = "Telemetry Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Windows Error Reporting via Group Policy. Prevents crash dumps and error data from being sent to Microsoft. Default: enabled.",
            Tags = ["telemetry", "wer", "error-reporting", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "Disabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "Disabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "Disabled", 1)],
        },
        new TweakDef
        {
            Id = "telem-security-only",
            Label = "Set Telemetry to Security Only",
            Category = "Telemetry Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets Windows telemetry to Security only (0 = Enterprise only) via Group Policy. Minimum data collection level. Default: Full (3).",
            Tags = ["telemetry", "security", "minimal", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "AllowTelemetry", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "AllowTelemetry")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "AllowTelemetry", 0)],
        },
        new TweakDef
        {
            Id = "telem-telemetry-set-max-size",
            Label = "Set Telemetry Cache Max Size to 0 MB",
            Category = "Telemetry Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the maximum diagnostic data cache size to 0 MB. Prevents telemetry data from accumulating on disk. Default: 1024 MB.",
            Tags = ["telemetry", "cache", "size", "disk"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "MaxTelemetryCacheSize", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "MaxTelemetryCacheSize")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "MaxTelemetryCacheSize", 0)],
        },
        new TweakDef
        {
            Id = "telem-disable-ceip",
            Label = "Disable Customer Experience Improvement Programme (SQMClient)",
            Category = "Telemetry Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets CEIPEnable=0 in the SQMClient Windows key. Disables the CEIP data-collection component associated with program quality telemetry uploads.",
            Tags = ["telemetry", "ceip", "sqm", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient\Windows"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient\Windows", "CEIPEnable", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient\Windows", "CEIPEnable")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient\Windows", "CEIPEnable", 0)],
        },
        new TweakDef
        {
            Id = "telem-disable-one-settings-download",
            Label = "Disable OneSettings Telemetry Configuration Downloads",
            Category = "Telemetry Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableOneSettingsDownloads=1 in DataCollection policy. Prevents Windows from downloading dynamic telemetry configuration updates (\"OneSettings\") from Microsoft servers.",
            Tags = ["telemetry", "one-settings", "download", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "DisableOneSettingsDownloads", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "DisableOneSettingsDownloads")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "DisableOneSettingsDownloads", 1),
            ],
        },
        new TweakDef
        {
            Id = "telem-limit-diagnostic-log",
            Label = "Limit Diagnostic Log Collection to Minimum",
            Category = "Telemetry Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets LimitDiagnosticLogCollection=1. Restricts the additional diagnostic log bundles gathered by Windows Feedback Hub and Windows Error Reporting to the minimum required.",
            Tags = ["telemetry", "diagnostic", "logs", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "LimitDiagnosticLogCollection", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "LimitDiagnosticLogCollection")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "LimitDiagnosticLogCollection", 1),
            ],
        },
        new TweakDef
        {
            Id = "telem-disable-cloud-analytics-enhanced",
            Label = "Restrict Enhanced Diagnostic Data to Windows Analytics",
            Category = "Telemetry Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets LimitEnhancedDiagnosticDataWindowsAnalytics=1. Even when Enhanced telemetry is enabled, only the subset required by Windows Analytics for update-health monitoring is uploaded.",
            Tags = ["telemetry", "enhanced", "analytics", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection",
                    "LimitEnhancedDiagnosticDataWindowsAnalytics",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection",
                    "LimitEnhancedDiagnosticDataWindowsAnalytics"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection",
                    "LimitEnhancedDiagnosticDataWindowsAnalytics",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "telem-disable-device-metadata-lookup",
            Label = "Disable Device Metadata Service URL Lookups",
            Category = "Telemetry Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableDeviceMetadataServiceUrlAccess=1 in DataCollection. Prevents Windows from contacting Microsoft's Device Metadata Service to retrieve driver cosmetic information, eliminating outbound telemetry-adjacent HTTP requests.",
            Tags = ["telemetry", "device", "metadata", "network", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "DisableDeviceMetadataServiceUrlAccess", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "DisableDeviceMetadataServiceUrlAccess"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection",
                    "DisableDeviceMetadataServiceUrlAccess",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "telem-disable-language-list-share",
            Label = "Disable Language List Sharing with Websites",
            Category = "Telemetry Advanced",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets HttpAcceptLanguageOptOut=1 in the International User Profile key. Sends a generic Accept-Language header to websites instead of the user's locale list, preventing language-based fingerprinting.",
            Tags = ["telemetry", "language", "privacy", "fingerprinting"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\International\User Profile"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\International\User Profile", "HttpAcceptLanguageOptOut", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Control Panel\International\User Profile", "HttpAcceptLanguageOptOut")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Control Panel\International\User Profile", "HttpAcceptLanguageOptOut", 1)],
        },
        new TweakDef
        {
            Id = "telem-disable-insider-builds",
            Label = "Disable Windows Insider Preview Build Access",
            Category = "Telemetry Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets AllowBuildPreview=0 via PreviewBuilds policy. Prevents the device from enrolling in Windows Insider Program flights, eliminating associated feedback and diagnostic data collection.",
            Tags = ["telemetry", "insider", "preview", "builds"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PreviewBuilds"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PreviewBuilds", "AllowBuildPreview", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PreviewBuilds", "AllowBuildPreview")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PreviewBuilds", "AllowBuildPreview", 0)],
        },
        new TweakDef
        {
            Id = "telem-disable-inking-telemetry",
            Label = "Disable Inking and Typing Privacy Telemetry",
            Category = "Telemetry Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableInkingAndTypingPrivacy=1 in DataCollection policy. Prevents Windows from uploading inking and typing samples used to improve handwriting recognition and autocorrect models.",
            Tags = ["telemetry", "inking", "typing", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "DisableInkingAndTypingPrivacy", 1)],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "DisableInkingAndTypingPrivacy"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "DisableInkingAndTypingPrivacy", 1),
            ],
        },
        new TweakDef
        {
            Id = "telem-disable-edge-data-opt-in",
            Label = "Opt Out of Microsoft Edge Telemetry Data",
            Category = "Telemetry Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets MicrosoftEdgeDataOptIn=0 in DataCollection policy. Prevents the legacy Edge engine from contributing usage and diagnostic data to Microsoft's browser analytics programme.",
            Tags = ["telemetry", "edge", "browser", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\DataCollection"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\DataCollection", "MicrosoftEdgeDataOptIn", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\DataCollection", "MicrosoftEdgeDataOptIn"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\DataCollection",
                    "MicrosoftEdgeDataOptIn",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "telem-disable-map-auto-update",
            Label = "Disable Windows Maps Automatic Data Updates",
            Category = "Telemetry Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets AutoUpdateEnabled=0 in HKLM\\SYSTEM\\Maps. Prevents Windows from silently downloading offline map data updates in the background.",
            Tags = ["telemetry", "maps", "auto-update", "network", "bandwidth"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\Maps"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\Maps", "AutoUpdateEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\Maps", "AutoUpdateEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\Maps", "AutoUpdateEnabled", 0)],
        },
        new TweakDef
        {
            Id = "telem-disable-oobe-privacy-wizard",
            Label = "Disable OOBE Privacy Experience Wizard",
            Category = "Telemetry Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisablePrivacyExperience=1 in the OOBE key. Prevents Windows from launching the privacy-settings wizard that prompts users to send diagnostic data after installation or upgrade.",
            Tags = ["telemetry", "oobe", "setup", "privacy", "wizard"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\OOBE"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\OOBE", "DisablePrivacyExperience", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\OOBE", "DisablePrivacyExperience")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\OOBE", "DisablePrivacyExperience", 1)],
        },
    ];
}
