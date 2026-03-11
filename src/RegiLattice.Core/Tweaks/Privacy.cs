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
            Description = "Disables Windows telemetry and feedback notifications.",
            Tags = ["privacy", "telemetry", "microsoft"],
            RegistryKeys = [@"HKLM\SOFTWARE\Policies\Microsoft\Windows\DataCollection", @"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\DataCollection"],
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
            Description = "Disables location tracking for all apps and Windows services.",
            Tags = ["privacy", "location", "tracking"],
            RegistryKeys = [@"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\location", @"HKLM\SOFTWARE\Policies\Microsoft\Windows\LocationAndSensors"],
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
            DetectOps = [RegOp.CheckString(@"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\location", "Value", "Deny")],
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
            RegistryKeys = [@"HKCU\Software\Microsoft\Windows\CurrentVersion\AdvertisingInfo", @"HKLM\SOFTWARE\Policies\Microsoft\Windows\AdvertisingInfo"],
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
            ApplyOps = [RegOp.SetString(@"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\webcam", "Value", "Deny")],
            RemoveOps = [RegOp.SetString(@"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\webcam", "Value", "Allow")],
            DetectOps = [RegOp.CheckString(@"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\webcam", "Value", "Deny")],
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
            ApplyOps = [RegOp.SetString(@"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\microphone", "Value", "Deny")],
            RemoveOps = [RegOp.SetString(@"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\microphone", "Value", "Allow")],
            DetectOps = [RegOp.CheckString(@"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\microphone", "Value", "Deny")],
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
            Description = "Disables Windows clipboard history (Win+V). Prevents sensitive copied data from being stored. Default: Enabled. Recommended: Disabled for privacy.",
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
            Description = "Disables online speech recognition which sends voice data to Microsoft servers. Default: Enabled. Recommended: Disabled for privacy.",
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
            Description = "Disables Windows activity history and timeline via Group Policy. Prevents activity feed and user activity publishing. Default: Enabled. Recommended: Disabled for privacy.",
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
            Id = "priv-privacy-disable-advertising-id",
            Label = "Disable Advertising ID",
            Category = "Privacy",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the Windows advertising ID for the current user. Prevents apps from using the ID for cross-app ad targeting. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["privacy", "advertising", "id", "tracking"],
            RegistryKeys = [@"HKCU\Software\Microsoft\Windows\CurrentVersion\AdvertisingInfo"],
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\Microsoft\Windows\CurrentVersion\AdvertisingInfo", "Enabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKCU\Software\Microsoft\Windows\CurrentVersion\AdvertisingInfo", "Enabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKCU\Software\Microsoft\Windows\CurrentVersion\AdvertisingInfo", "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "priv-privacy-disable-tailored-experiences",
            Label = "Disable Tailored Experiences",
            Category = "Privacy",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Microsoft tailored experiences that use diagnostic data to personalise ads, tips, and recommendations. Default: enabled. Recommended: disabled.",
            Tags = ["privacy", "tailored", "diagnostic", "personalisation"],
            RegistryKeys = [@"HKCU\Software\Microsoft\Windows\CurrentVersion\Privacy"],
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\Microsoft\Windows\CurrentVersion\Privacy", "TailoredExperiencesWithDiagnosticDataEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKCU\Software\Microsoft\Windows\CurrentVersion\Privacy", "TailoredExperiencesWithDiagnosticDataEnabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKCU\Software\Microsoft\Windows\CurrentVersion\Privacy", "TailoredExperiencesWithDiagnosticDataEnabled", 0)],
        },
        new TweakDef
        {
            Id = "priv-privacy-disable-windows-tips",
            Label = "Disable Windows Tips & Suggestions",
            Category = "Privacy",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Windows tips, tricks, and 'Get started' suggestions that appear in Start menu, lock screen and notifications. Default: enabled. Recommended: disabled.",
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
            Description = "Prevents Windows from tracking which apps you launch to improve Start menu suggestions. Default: enabled. Recommended: disabled.",
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
            DetectOps = [RegOp.CheckDword(@"HKCU\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SubscribedContent-338393Enabled", 0)],
        },
        new TweakDef
        {
            Id = "priv-disable-handwriting-sharing",
            Label = "Disable Handwriting Data Sharing",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables implicit ink/text collection and handwriting error reports. Prevents handwriting data from being shared with Microsoft. Default: Enabled. Recommended: Disabled.",
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
            Description = "Disables User Activity Reporting (UAR) at the machine policy level. Prevents Windows from tracking application launches system-wide. Default: Enabled. Recommended: Disabled.",
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
            Description = "Disables the Connected Devices Platform (CDP) which enables cross-device experiences like phone-to-PC linking and shared clipboard. Default: Enabled. Recommended: Disabled for privacy.",
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
            Description = "Denies applications access to diagnostic information about other apps. Protects inter-app data leakage. Default: Allow. Recommended: Deny.",
            Tags = ["privacy", "app-diagnostics", "consent", "capability"],
            RegistryKeys = [@"HKCU\Software\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\appDiagnostics"],
            ApplyOps = [RegOp.SetString(@"HKCU\Software\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\appDiagnostics", "Value", "Deny")],
            RemoveOps = [RegOp.DeleteValue(@"HKCU\Software\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\appDiagnostics", "Value")],
            DetectOps = [RegOp.CheckString(@"HKCU\Software\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\appDiagnostics", "Value", "Deny")],
        },
        new TweakDef
        {
            Id = "priv-disable-account-info-access",
            Label = "Disable Account Info Access",
            Category = "Privacy",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Denies applications access to user account information (name, picture). Improves privacy. Default: Allow. Recommended: Deny.",
            Tags = ["privacy", "account-info", "consent", "capability"],
            RegistryKeys = [@"HKCU\Software\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\userAccountInformation"],
            ApplyOps = [RegOp.SetString(@"HKCU\Software\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\userAccountInformation", "Value", "Deny")],
            RemoveOps = [RegOp.DeleteValue(@"HKCU\Software\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\userAccountInformation", "Value")],
            DetectOps = [RegOp.CheckString(@"HKCU\Software\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\userAccountInformation", "Value", "Deny")],
        },
        new TweakDef
        {
            Id = "priv-disable-notification-listener",
            Label = "Disable Notification Listener Access",
            Category = "Privacy",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Denies applications access to user notification content. Prevents apps from reading notification text. Default: Allow. Recommended: Deny.",
            Tags = ["privacy", "notifications", "listener", "consent"],
            RegistryKeys = [@"HKCU\Software\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\userNotificationListener"],
            ApplyOps = [RegOp.SetString(@"HKCU\Software\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\userNotificationListener", "Value", "Deny")],
            RemoveOps = [RegOp.DeleteValue(@"HKCU\Software\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\userNotificationListener", "Value")],
            DetectOps = [RegOp.CheckString(@"HKCU\Software\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\userNotificationListener", "Value", "Deny")],
        },
        new TweakDef
        {
            Id = "priv-disable-contacts-access",
            Label = "Block Apps from Accessing Contacts",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents apps from accessing your contacts list via the AppPrivacy policy. Value 2 = Force Deny. Default: Allow (0). Recommended: Force Deny.",
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
            Description = "Prevents apps from reading or modifying your calendar via the AppPrivacy policy. Value 2 = Force Deny. Default: Allow (0). Recommended: Force Deny.",
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
            Description = "Prevents apps from toggling Bluetooth or Wi-Fi radios via the AppPrivacy policy. Value 2 = Force Deny. Default: Allow (0). Recommended: Force Deny.",
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
            Description = "Disables the Windows Ink Workspace feature via policy. Prevents the pen/stylus workspace from loading. Default: Enabled. Recommended: Disabled on non-tablet devices.",
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
            Description = "Sets LetAppsAccessCallHistory=2 (Force Deny) via AppPrivacy policy. Prevents all apps from reading your phone call history. Default: Allow (0). Recommended: Force Deny.",
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
            Description = "Sets LetAppsAccessEmail=2 (Force Deny) via AppPrivacy policy. Prevents all UWP apps from reading your email accounts and messages. Default: Allow (0). Recommended: Force Deny.",
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
            Description = "Sets LetAppsAccessTasks=2 (Force Deny) via AppPrivacy policy. Prevents all apps from reading or modifying your task/to-do lists. Default: Allow (0). Recommended: Force Deny.",
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
            Description = "Sets LetAppsAccessMessaging=2 (Force Deny) via AppPrivacy policy. Prevents all apps from reading or sending SMS/MMS messages. Default: Allow (0). Recommended: Force Deny.",
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
            Description = "Disables online speech recognition, inking, and typing personalization. Prevents sending typing data to Microsoft. Default: enabled.",
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
            Description = "Disables tailored experiences with diagnostic data. Prevents Microsoft from using your data to customize tips and recommendations. Default: enabled.",
            Tags = ["privacy", "tailored", "experiences", "recommendations"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Privacy"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Privacy", "TailoredExperiencesWithDiagnosticDataEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Privacy", "TailoredExperiencesWithDiagnosticDataEnabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Privacy", "TailoredExperiencesWithDiagnosticDataEnabled", 0)],
        },
    ];
}
