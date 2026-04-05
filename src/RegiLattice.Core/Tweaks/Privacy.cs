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
    ];
}

// === Merged from: TelemetryAdvanced.cs ===

internal static class TelemetryAdvanced
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "telem-disable-handwriting",
            Label = "Disable Handwriting Data Sharing",
            Category = "Privacy",
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
            Id = "telem-disable-tailored-experiences",
            Label = "Disable Tailored Experiences",
            Category = "Privacy",
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
            Id = "telem-disable-sqm-upload",
            Label = "Disable SQM Telemetry Upload",
            Category = "Privacy",
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
            Id = "telem-disable-speech-model-update",
            Label = "Disable Speech Model Automatic Update",
            Category = "Privacy",
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
            Category = "Privacy",
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
            Id = "telem-telemetry-set-max-size",
            Label = "Set Telemetry Cache Max Size to 0 MB",
            Category = "Privacy",
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
            Id = "telem-disable-device-metadata-lookup",
            Label = "Disable Device Metadata Service URL Lookups",
            Category = "Privacy",
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
            Id = "telem-disable-inking-telemetry",
            Label = "Disable Inking and Typing Privacy Telemetry",
            Category = "Privacy",
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
            Category = "Privacy",
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
            Category = "Privacy",
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
    ];
}

// ── merged from PolicyPrivacy.cs ──
// RegiLattice.Core — Tweaks/PolicyPrivacy.cs
// Advertising ID, data collection, feedback, location sensors, push notifications, Windows diagnostics, and privacy policy controls
// Category: "Privacy & Telemetry Policy"
// Consolidated from 18 modules.

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
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AdvertisingInfo";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "advinfo-disable-advertising-id",
                    Label = "Disable Windows Advertising ID (RUID)",
                    Category = "Privacy — Advertising Info",
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
                    Category = "Privacy — Advertising Info",
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
                    Category = "Privacy — Advertising Info",
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
                    Category = "Privacy — Advertising Info",
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
                    Category = "Privacy — Advertising Info",
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
                    Category = "Privacy — Advertising Info",
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
                    Category = "Privacy — Advertising Info",
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
                    Category = "Privacy — Advertising Info",
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
                    Category = "Privacy — Advertising Info",
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
                    Category = "Privacy — Advertising Info",
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
                Category = "Privacy — Advertising Info",
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
                Category = "Privacy — Advertising Info",
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
                Id = "datacol-disable-device-delete-button",
                Label = "Disable Delete Device Diagnostic Data Button",
                Category = "Privacy — Advertising Info",
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
                Id = "datacol-disable-ceip",
                Label = "Disable Customer Experience Improvement Program",
                Category = "Privacy — Advertising Info",
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
                Category = "Privacy — Advertising Info",
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
                Category = "Privacy — Advertising Info",
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
                Category = "Privacy — Advertising Info",
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
                Category = "Privacy — Advertising Info",
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
                Category = "Privacy — Advertising Info",
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
                Category = "Privacy — Advertising Info",
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
                Category = "Privacy — Advertising Info",
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
                Category = "Privacy — Advertising Info",
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
                Category = "Privacy — Advertising Info",
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
                Category = "Privacy — Advertising Info",
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
                Category = "Privacy — Advertising Info",
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
                Category = "Privacy — Advertising Info",
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
                Category = "Privacy — Advertising Info",
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
                Category = "Privacy — Advertising Info",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
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
                Category = "Privacy — Advertising Info",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
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
                Category = "Privacy — Advertising Info",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
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
                Category = "Privacy — Advertising Info",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
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
                Category = "Privacy — Advertising Info",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
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
                Category = "Privacy — Advertising Info",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
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
                Category = "Privacy — Advertising Info",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
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
                Category = "Privacy — Advertising Info",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
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
                Category = "Privacy — Advertising Info",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
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
                Category = "Privacy — Advertising Info",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
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
        private const string UserLocation =
            @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\location";
        private const string UserActivity =
            @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\activity";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "loc-policy-deny-app-motion",
                Label = "Policy: Force-Deny All UWP Apps Motion Sensor Access",
                Category = "Privacy — Advertising Info",
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
                Id = "loc-user-deny-location",
                Label = "Turn Off Location Access for Current User",
                Category = "Privacy — Advertising Info",
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
                Category = "Privacy — Advertising Info",
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
                Category = "Privacy — Advertising Info",
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
                    Id = "locsns-disable-windowed-location",
                    Label = "Disable Windowed Location Mode",
                    Category = "Privacy — Advertising Info",
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
                    Category = "Privacy — Advertising Info",
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
                    Category = "Privacy — Advertising Info",
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
                    Id = "locsns-disable-cellular-location",
                    Label = "Disable Cellular Data for Location",
                    Category = "Privacy — Advertising Info",
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
                    Category = "Privacy — Advertising Info",
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
                    Category = "Privacy — Advertising Info",
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
                Id = "msa-block-msa-signin",
                Label = "Block Microsoft Account Sign-In for Apps",
                Category = "Privacy — Advertising Info",
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
                Id = "msa-disable-app-access-account-info",
                Label = "Disable App Access to Account Information (Privacy)",
                Category = "Privacy — Advertising Info",
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
                Id = "msa-disable-theme-sync",
                Label = "Disable Theme Sync via Microsoft Account",
                Category = "Privacy — Advertising Info",
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
                Category = "Privacy — Advertising Info",
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
                Category = "Privacy — Advertising Info",
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
                Category = "Privacy — Advertising Info",
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
                Category = "Privacy — Advertising Info",
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
                Category = "Privacy — Advertising Info",
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
                Category = "Privacy — Advertising Info",
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
                Category = "Privacy — Advertising Info",
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
                Category = "Privacy — Advertising Info",
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
                Category = "Privacy — Advertising Info",
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
                Category = "Privacy — Advertising Info",
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
                    Category = "Privacy — Advertising Info",
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
                    Category = "Privacy — Advertising Info",
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
                    Category = "Privacy — Advertising Info",
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
                    Category = "Privacy — Advertising Info",
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
                    Category = "Privacy — Advertising Info",
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
                    Category = "Privacy — Advertising Info",
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
                    Category = "Privacy — Advertising Info",
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
                    Category = "Privacy — Advertising Info",
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
                    Category = "Privacy — Advertising Info",
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
                    Category = "Privacy — Advertising Info",
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
                Id = "srchweb-enforce-safe-search",
                Label = "Enforce Strict SafeSearch for Web Results",
                Category = "Privacy — Advertising Info",
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
                Id = "sensor-block-location-user-override",
                Label = "Prevent Users Re-Enabling Location Services",
                Category = "Privacy — Sensor",
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
                Category = "Privacy — Sensor",
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
                Category = "Privacy — Sensor",
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
                Category = "Privacy — Sensor",
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
                Category = "Privacy — Sensor",
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
                Category = "Privacy — Sensor",
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
                Category = "Privacy — Sensor",
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
                Id = "sensor-disable-geocoder",
                Label = "Disable Geocoder Service",
                Category = "Privacy — Sensor",
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
                Id = "sensor-disable-permission-changes",
                Label = "Disable Location Permission Changes",
                Category = "Privacy — Sensor",
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
                Category = "Privacy — Sensor",
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
                Category = "Privacy — Sensor",
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
                Category = "Privacy — Sensor",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
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
                Category = "Privacy — Sensor",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
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
                Category = "Privacy — Sensor",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
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
                Category = "Privacy — Sensor",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
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
                Category = "Privacy — Sensor",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
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
                Category = "Privacy — Sensor",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
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
                Category = "Privacy — Sensor",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
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
                Category = "Privacy — Sensor",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
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
                Category = "Privacy — Sensor",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
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
                Category = "Privacy — Sensor",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
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
                Id = "wdiag-disable-app-compat-engine",
                Label = "Diagnostics: Disable Application Compatibility Engine",
                Category = "Privacy — Sensor",
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
                Id = "wdiag-disable-reliability-monitor",
                Label = "Diagnostics: Disable Reliability Monitor Data Collection",
                Category = "Privacy — Sensor",
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
                Id = "wdiag-disable-insider-preview-builds",
                Label = "Diagnostics: Block Windows Insider Preview Build Notifications",
                Category = "Privacy — Sensor",
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
                    Category = "Privacy — Sensor",
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
                    Category = "Privacy — Sensor",
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
                    Category = "Privacy — Sensor",
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
                    Category = "Privacy — Sensor",
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
                    Category = "Privacy — Sensor",
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
                    Category = "Privacy — Sensor",
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
                    Category = "Privacy — Sensor",
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
                    Category = "Privacy — Sensor",
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
                    Category = "Privacy — Sensor",
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
                    Category = "Privacy — Sensor",
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
                Id = "diagtrk-limit-dump-collection",
                Label = "Windows DiagTrack: Limit Dump Collection Level",
                Category = "Privacy — Sensor",
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
                Id = "diagtrk-disable-cloud-clipboard-integration",
                Label = "Windows DiagTrack: Disable Cloud Clipboard Telemetry",
                Category = "Privacy — Sensor",
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
                Category = "Privacy — Sensor",
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
                Category = "Privacy — Sensor",
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
                Category = "Privacy — Sensor",
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
                Id = "wippol-block-copy-to-personal",
                Label = "WIP: Block Copying Work Data to Personal Apps",
                Category = "Privacy — Sensor",
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
                Category = "Privacy — Sensor",
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
                Category = "Privacy — Sensor",
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
                Category = "Privacy — Sensor",
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
                Category = "Privacy — Sensor",
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
                Category = "Privacy — Sensor",
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
                Category = "Privacy — Sensor",
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
                Category = "Privacy — Sensor",
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
                Id = "wmaps-disable-search-highlights",
                Label = "Search: Disable dynamic search highlights in the taskbar",
                Category = "Privacy — Sensor",
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
                Category = "Privacy — Sensor",
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
                Category = "Privacy — Sensor",
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
                Category = "Privacy — Sensor",
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
                Category = "Privacy — Sensor",
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
                Category = "Privacy — Sensor",
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

internal static class PolicyWindowsInk
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "winks-disable-ink-touch-keyboard-autoinvoke",
            Label = "Disable Touch Keyboard Auto-Invoke in Ink",
            Category = "Privacy — Sensor",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents the touch keyboard from automatically appearing when a text field is focused in Windows Ink apps. Reduces accidental keyboard pop-ups on pen-only tablet workflows.",
            Tags = ["ink", "touch-keyboard", "tablet", "policy", "windows-ink"],
            RegistryKeys = [Key],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Touch keyboard no longer auto-invokes in ink context; pen workflow uninterrupted.",
            ApplyOps = [RegOp.SetDword(Key, "TouchKeyboardAutoInvokeEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "TouchKeyboardAutoInvokeEnabled")],
            DetectOps = [RegOp.CheckDword(Key, "TouchKeyboardAutoInvokeEnabled", 0)],
        },
        new TweakDef
        {
            Id = "winks-disable-ink-workspace-telemetry",
            Label = "Disable Windows Ink Workspace Telemetry",
            Category = "Privacy — Sensor",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks telemetry data collection from Windows Ink Workspace usage patterns. Prevents Microsoft from receiving information about which ink tools are used and how frequently.",
            Tags = ["ink", "telemetry", "privacy", "policy", "windows-ink", "data-collection"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Ink workspace usage statistics not reported to Microsoft.",
            ApplyOps = [RegOp.SetDword(Key, "AllowWindowsInkWorkspaceTelemetry", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowWindowsInkWorkspaceTelemetry")],
            DetectOps = [RegOp.CheckDword(Key, "AllowWindowsInkWorkspaceTelemetry", 0)],
        },
    ];
}

// ─────────────────────────────────────────────────────────────────────────────
// Sprint 644 — PolicyLocationSensors (Location & Sensors Group Policy)

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

// ─────────────────────────────────────────────────────────────────────────────
// Sprint 645 — PolicyCloudClipboard (Cloud Clipboard & Clipboard History Policy)

internal static class PolicyCloudClipboard
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "clipol-disable-phone-clipboard-sync",
            Label = "Disable Phone-to-PC Clipboard Sync",
            Category = "Privacy — Sensor",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents clipboard content from being shared between a paired Android/iPhone and the Windows PC via Phone Link. Disables the mobile-to-desktop clipboard relay channel.",
            Tags = ["clipboard", "phone", "android", "phone-link", "privacy", "policy"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Phone-to-PC clipboard bridge disabled; mobile clipboard items not transferred to PC.",
            ApplyOps = [RegOp.SetDword(Key, "DisableClipboardForPhone", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableClipboardForPhone")],
            DetectOps = [RegOp.CheckDword(Key, "DisableClipboardForPhone", 1)],
        },
        new TweakDef
        {
            Id = "clipol-disable-clipboard-gpt-integration",
            Label = "Disable Clipboard AI / Copilot Integration",
            Category = "Privacy — Sensor",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks Windows Copilot and AI features from reading clipboard content for contextual suggestions. Prevents AI models from processing clipboard data including passwords, banking information, or confidential documents inadvertently copied.",
            Tags = ["clipboard", "ai", "copilot", "gpt", "privacy", "policy"],
            RegistryKeys = [Key],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "AI/Copilot clipboard access blocked; sensitive copied data not processed by AI.",
            ApplyOps = [RegOp.SetDword(Key, "AllowCopilotClipboardAccess", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowCopilotClipboardAccess")],
            DetectOps = [RegOp.CheckDword(Key, "AllowCopilotClipboardAccess", 0)],
        },
        new TweakDef
        {
            Id = "clipol-disable-clipboard-hello-sync",
            Label = "Disable Clipboard Sync via Windows Hello",
            Category = "Privacy — Sensor",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents clipboard content from being relayed between devices using Windows Hello companion device authentication. Stops clipboard sharing initiated through the Hello companion device framework.",
            Tags = ["clipboard", "windows-hello", "sync", "privacy", "policy"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Hello companion-device clipboard relay disabled.",
            ApplyOps = [RegOp.SetDword(Key, "AllowCrossDeviceClipboardViaWindowsHello", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowCrossDeviceClipboardViaWindowsHello")],
            DetectOps = [RegOp.CheckDword(Key, "AllowCrossDeviceClipboardViaWindowsHello", 0)],
        },
        new TweakDef
        {
            Id = "clipol-disable-clipboard-rdp-passthrough",
            Label = "Disable RDP Clipboard Passthrough",
            Category = "Security — Privacy Hardening",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks clipboard redirection between an RDP session and the remote machine. Prevents users from pasting data from a remote desktop session into local applications, blocking a common data-exfiltration channel.",
            Tags = ["clipboard", "rdp", "remote-desktop", "exfiltration", "policy", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services"],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "RDP clipboard redirect blocked; copy-paste between RDP and local machine disabled.",
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "DisableClipboardRedirection", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "DisableClipboardRedirection"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "DisableClipboardRedirection", 1),
            ],
        },
        new TweakDef
        {
            Id = "clipol-disable-clipboard-remote-viewer",
            Label = "Disable Clipboard in Remote Assistance Sessions",
            Category = "Security — Privacy Hardening",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks clipboard access during Windows Remote Assistance sessions. Prevents the remote assistant from copying sensitive data from the user's clipboard during a coached support session.",
            Tags = ["clipboard", "remote-assistance", "security", "policy", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Remote assistance helper cannot access clipboard; data exfiltration during support blocked.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableClip", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableClip")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableClip", 1)],
        },
        new TweakDef
        {
            Id = "clipol-clear-clipboard-on-lock",
            Label = "Clear Clipboard on Screen Lock",
            Category = "Privacy — Sensor",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Automatically clears all clipboard history and the current clipboard when the screen locks. Prevents sensitive information (passwords, tokens, PII) from remaining in clipboard after the user leaves their desk.",
            Tags = ["clipboard", "lock-screen", "clear", "privacy", "policy", "security"],
            RegistryKeys = [Key],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Clipboard contents wiped on every screen lock; sensitive data never retained when unattended.",
            ApplyOps = [RegOp.SetDword(Key, "ClearClipboardOnLock", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "ClearClipboardOnLock")],
            DetectOps = [RegOp.CheckDword(Key, "ClearClipboardOnLock", 1)],
        },
        new TweakDef
        {
            Id = "clipol-disable-clipboard-audit",
            Label = "Disable Clipboard Audit Logging",
            Category = "Privacy — Sensor",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables clipboard operation audit logging in the Windows Security event log. Stops clipboard read/write events from being written to Security log for privacy-focused deployments without audit requirements.",
            Tags = ["clipboard", "audit", "logging", "privacy", "policy"],
            RegistryKeys = [Key],
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "Clipboard operations not logged to Security audit log.",
            ApplyOps = [RegOp.SetDword(Key, "ClipboardAuditLogging", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "ClipboardAuditLogging")],
            DetectOps = [RegOp.CheckDword(Key, "ClipboardAuditLogging", 0)],
        },
        new TweakDef
        {
            Id = "clipol-disable-clipboard-suggested-actions",
            Label = "Disable Clipboard Smart Actions / Suggested Text Actions",
            Category = "Privacy — Sensor",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Clipboard smart actions feature that analyses copied text and suggests contextual actions (add to calendar, call a phone number, open a URL). Stops clipboard content from being sent to local AI processing pipelines.",
            Tags = ["clipboard", "smart-actions", "ai", "privacy", "policy"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Clipboard content analysis for smart actions disabled; no AI text interpretation of copied data.",
            ApplyOps = [RegOp.SetDword(Key, "AllowClipboardSuggestedActions", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowClipboardSuggestedActions")],
            DetectOps = [RegOp.CheckDword(Key, "AllowClipboardSuggestedActions", 0)],
        },
    ];
}

// ─────────────────────────────────────────────────────────────────────────────
// Sprint 646 — PolicyNetworkIsolation (Network Isolation / AppContainer Policy)

internal static class PolicyWindowsSearch
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "wsepol-block-remote-query",
            Label = "Block Remote Cortana Query via Policy",
            Category = "Privacy — Sensor",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableRemoteQuery=1 under the Windows Search Group Policy path. "
                + "Prevents Cortana from querying remote services for information when invoked. "
                + "Queries are processed locally only, reducing data exfiltration risk.",
            Tags = ["search", "cortana", "remote", "policy", "security"],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Blocks Cortana remote queries; local processing only.",
            ApplyOps = [RegOp.SetDword(Key, "DisableRemoteQuery", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableRemoteQuery")],
            DetectOps = [RegOp.CheckDword(Key, "DisableRemoteQuery", 1)],
        },
    ];
}

/// <summary>
/// Sprint 653 — App privacy access via Group Policy (10 tweaks).
/// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy
/// Controls which app capabilities (camera, microphone, contacts, etc.)
/// apps may access on the machine. LetApps* = 2 forces access off for all apps.
/// </summary>
internal static class PolicyAppPrivacy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } = [];
}

/// <summary>
/// Sprint 654 — Windows CloudContent / User Experience policies (10 tweaks).
/// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\CloudContent
/// These Group Policy keys disable Microsoft promotional content,
/// Spotlight features, lock screen suggestions, and tailored experiences
/// that are delivered via cloud services.
/// </summary>
/// <summary>
/// Sprint 655 — Windows Event Log sizing and access policies (10 tweaks).
/// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\EventLog\{Application|Security|System|Setup}
/// These Group Policy keys configure maximum event log sizes and guest access
/// restrictions for the four primary Windows event log channels.
/// </summary>
internal static class PolicyEventLogAudit
{
    private const string AppLog = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\Application";
    private const string SecLog = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\Security";
    private const string SysLog = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\System";
    private const string SetupLog = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\Setup";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } = [];
}

/// <summary>
/// Sprint 656 — Windows Settings Sync policies (10 tweaks).
/// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\SettingSync
/// Disables roaming sync of various Windows settings categories via
/// Group Policy. Suitable for environments where settings should remain
/// machine-local and not roam through Microsoft accounts or Azure AD.
/// </summary>
internal static class PolicySyncSettings
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SettingSync";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } = [];
}

internal static class PolicyLocation
{
    public static IReadOnlyList<TweakDef> Tweaks => [.. _MapsPolicy.Data, .. _LocationSensorsPolicy.Data];

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
                    Category = "Privacy — Sensor",
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
                    Category = "Privacy — Sensor",
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
                    Category = "Privacy — Sensor",
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
                    Category = "Privacy — Sensor",
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
                    Category = "Privacy — Sensor",
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
                    Category = "Privacy — Location Sensors",
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
                    Category = "Privacy — Location Sensors",
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
                    Category = "Privacy — Location Sensors",
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
                    Category = "Privacy — Location Sensors",
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
                    Category = "Privacy — Location Sensors",
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
    public static IReadOnlyList<TweakDef> Tweaks => [.. _DataCollectionPolicy.Data, .. _AppCompatPolicy.Data];

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
                    Category = "Privacy — Location Sensors",
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
                    Category = "Privacy — Location Sensors",
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
                    Category = "Privacy — Location Sensors",
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
                    Category = "Privacy — Location Sensors",
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

internal static class PolicyWindowsFeeds
{
    // HKLM\SOFTWARE\Policies\Microsoft\Windows\WindowsFeeds — Windows RSS Feeds/
    // Ticker controlled via Group Policy (distinct from the Dsh/News widgets path).

    private const string FeedsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsFeeds";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "wsfeed-disable-windows-feeds",
            Label = "Disable Windows Feeds via Policy",
            Category = "Privacy — Location Sensors",
            Description =
                "Sets DisableWindowsFeeds=1 in the WindowsFeeds Group Policy key. "
                + "Prevents the Windows Feeds (RSS/Atom) integration from running in the taskbar and File Explorer. "
                + "Eliminates background network polling for feed updates and removes the news ticker UI.",
            Tags = ["feeds", "rss", "news", "taskbar", "policy", "privacy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Disables Windows Feed subscriptions and background RSS polling.",
            ApplyOps = [RegOp.SetDword(FeedsKey, "DisableWindowsFeeds", 1)],
            RemoveOps = [RegOp.DeleteValue(FeedsKey, "DisableWindowsFeeds")],
            DetectOps = [RegOp.CheckDword(FeedsKey, "DisableWindowsFeeds", 1)],
        },
        new TweakDef
        {
            Id = "wsfeed-disable-background-sync",
            Label = "Disable Background Feed Synchronisation",
            Category = "Privacy — Location Sensors",
            Description =
                "Sets BackgroundSyncEnabled=0 in the WindowsFeeds Group Policy key. "
                + "Prevents Windows from silently synchronising feed content in the background. "
                + "Reduces network traffic and CPU wakeups from feed polling tasks.",
            Tags = ["feeds", "sync", "background", "network", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Feed content no longer syncs in the background; pages only update when manually opened.",
            ApplyOps = [RegOp.SetDword(FeedsKey, "BackgroundSyncEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(FeedsKey, "BackgroundSyncEnabled")],
            DetectOps = [RegOp.CheckDword(FeedsKey, "BackgroundSyncEnabled", 0)],
        },
        new TweakDef
        {
            Id = "wsfeed-prevent-feed-subscription",
            Label = "Prevent Users from Subscribing to Feeds",
            Category = "Privacy — Location Sensors",
            Description =
                "Sets PreventSubscription=1 in the WindowsFeeds Group Policy key. "
                + "Blocks users from subscribing to new RSS/Atom feeds via Internet Explorer or Feed Discovery. "
                + "Useful in controlled environments where feed subscriptions must be centrally managed.",
            Tags = ["feeds", "subscription", "policy", "enterprise"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Users cannot add new RSS feed subscriptions in browsers or via auto-discovery.",
            ApplyOps = [RegOp.SetDword(FeedsKey, "PreventSubscription", 1)],
            RemoveOps = [RegOp.DeleteValue(FeedsKey, "PreventSubscription")],
            DetectOps = [RegOp.CheckDword(FeedsKey, "PreventSubscription", 1)],
        },
        new TweakDef
        {
            Id = "wsfeed-prevent-feed-discovery",
            Label = "Prevent Automatic Feed Discovery",
            Category = "Privacy — Location Sensors",
            Description =
                "Sets PreventAutoDiscovery=1 in the WindowsFeeds Group Policy key. "
                + "Stops Internet Explorer and other browsers from automatically discovering available feeds "
                + "on visited web pages. Eliminates the feed icon in the toolbar and related network probes.",
            Tags = ["feeds", "discovery", "browser", "policy", "privacy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Feed discovery in browsers is disabled; no auto-detection of RSS/Atom links.",
            ApplyOps = [RegOp.SetDword(FeedsKey, "PreventAutoDiscovery", 1)],
            RemoveOps = [RegOp.DeleteValue(FeedsKey, "PreventAutoDiscovery")],
            DetectOps = [RegOp.CheckDword(FeedsKey, "PreventAutoDiscovery", 1)],
        },
        new TweakDef
        {
            Id = "wsfeed-disable-unlocked-feeds",
            Label = "Lock Feed List to Prevent User Modifications",
            Category = "Privacy — Location Sensors",
            Description =
                "Sets FeedListLocked=1 in the WindowsFeeds Group Policy key. "
                + "Prevents users from adding, removing, or modifying feed subscriptions. "
                + "Administrators retain full control over what feed sources are available systemwide.",
            Tags = ["feeds", "lockdown", "policy", "enterprise"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Feed list is read-only for standard users; only admins can change subscriptions.",
            ApplyOps = [RegOp.SetDword(FeedsKey, "FeedListLocked", 1)],
            RemoveOps = [RegOp.DeleteValue(FeedsKey, "FeedListLocked")],
            DetectOps = [RegOp.CheckDword(FeedsKey, "FeedListLocked", 1)],
        },
        new TweakDef
        {
            Id = "wsfeed-disable-feed-download",
            Label = "Block Feed Content Download",
            Category = "Privacy — Location Sensors",
            Description =
                "Sets AllowFeedDownload=0 in the WindowsFeeds Group Policy key. "
                + "Prevents Windows from downloading feed content to the local machine, stopping "
                + "offline reading caches and news-article pre-fetch from consuming bandwidth and storage.",
            Tags = ["feeds", "download", "bandwidth", "policy", "privacy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Feed articles are not pre-fetched; online access required to view feed content.",
            ApplyOps = [RegOp.SetDword(FeedsKey, "AllowFeedDownload", 0)],
            RemoveOps = [RegOp.DeleteValue(FeedsKey, "AllowFeedDownload")],
            DetectOps = [RegOp.CheckDword(FeedsKey, "AllowFeedDownload", 0)],
        },
        new TweakDef
        {
            Id = "wsfeed-disable-third-party-feeds",
            Label = "Block Third-Party Feed Providers",
            Category = "Privacy — Location Sensors",
            Description =
                "Sets AllowThirdPartyFeeds=0 in the WindowsFeeds Group Policy key. "
                + "Restricts feed aggregation to Windows-native sources only, preventing third-party "
                + "feed aggregators or browser extensions from registering as system-level feed providers.",
            Tags = ["feeds", "third-party", "policy", "enterprise", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "Only Windows-native feed mechanisms are permitted; third-party aggregators are blocked.",
            ApplyOps = [RegOp.SetDword(FeedsKey, "AllowThirdPartyFeeds", 0)],
            RemoveOps = [RegOp.DeleteValue(FeedsKey, "AllowThirdPartyFeeds")],
            DetectOps = [RegOp.CheckDword(FeedsKey, "AllowThirdPartyFeeds", 0)],
        },
        new TweakDef
        {
            Id = "wsfeed-disable-feed-reading-pane",
            Label = "Disable Feed Reading Pane",
            Category = "Privacy — Location Sensors",
            Description =
                "Sets DisableReadingPane=1 in the WindowsFeeds Group Policy key. "
                + "Removes the feed reading pane from Internet Explorer and Windows RSS Platform view. "
                + "Reduces distraction and prevents previewing unapproved news content inside the browser.",
            Tags = ["feeds", "reading-pane", "browser", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "Reading pane in feed viewer is hidden; feeds show in list-only mode.",
            ApplyOps = [RegOp.SetDword(FeedsKey, "DisableReadingPane", 1)],
            RemoveOps = [RegOp.DeleteValue(FeedsKey, "DisableReadingPane")],
            DetectOps = [RegOp.CheckDword(FeedsKey, "DisableReadingPane", 1)],
        },
        new TweakDef
        {
            Id = "wsfeed-disable-enclosure-download",
            Label = "Block Feed Enclosure (Podcast) Auto-Download",
            Category = "Privacy — Location Sensors",
            Description =
                "Sets AllowEnclosureDownload=0 in the WindowsFeeds Group Policy key. "
                + "Prevents Windows from automatically downloading podcast and media enclosures "
                + "attached to RSS feed items. Eliminates background large-file downloads triggered by feed updates.",
            Tags = ["feeds", "podcast", "enclosure", "download", "bandwidth", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Podcast/media enclosures in RSS feeds are not auto-downloaded.",
            ApplyOps = [RegOp.SetDword(FeedsKey, "AllowEnclosureDownload", 0)],
            RemoveOps = [RegOp.DeleteValue(FeedsKey, "AllowEnclosureDownload")],
            DetectOps = [RegOp.CheckDword(FeedsKey, "AllowEnclosureDownload", 0)],
        },
        new TweakDef
        {
            Id = "wsfeed-restrict-feed-secure-only",
            Label = "Restrict Feeds to HTTPS Sources Only",
            Category = "Privacy — Location Sensors",
            Description =
                "Sets SecureFeedsOnly=1 in the WindowsFeeds Group Policy key. "
                + "Enforces that only feeds served over HTTPS are accepted by the Windows RSS Platform. "
                + "Blocks plain HTTP feed URLs which could be subject to man-in-the-middle injection and content tampering.",
            Tags = ["feeds", "https", "security", "policy", "network"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "HTTP feed URLs are rejected; all feed sources must use HTTPS.",
            ApplyOps = [RegOp.SetDword(FeedsKey, "SecureFeedsOnly", 1)],
            RemoveOps = [RegOp.DeleteValue(FeedsKey, "SecureFeedsOnly")],
            DetectOps = [RegOp.CheckDword(FeedsKey, "SecureFeedsOnly", 1)],
        },
    ];
}

internal static class PolicySpeechInput
{
    // HKLM\SOFTWARE\Policies\Microsoft\Windows\Speech — Speech Recognition / Voice Access.
    // HKLM\SOFTWARE\Policies\Microsoft\Windows\SpeechModel — online speech model policies.

    private const string SpeechKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Speech";
    private const string ModelKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SpeechModel";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "spkinput-disable-online-speech-recognition",
            Label = "Disable Online Speech Recognition via Policy",
            Category = "Privacy — Location Sensors",
            Description =
                "Sets AllowSpeechRecognition=0 in the Speech Group Policy key. "
                + "Prevents the cloud speech recognition service from being used for Windows speech features. "
                + "Voice data is only processed on-device; no audio is transmitted to Microsoft Speech servers. "
                + "Applies broadly to Cortana voice queries, Voice Typing, and Voice Access cloud enhancement.",
            Tags = ["speech", "recognition", "cloud", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Cloud speech recognition disabled; on-device speech processing continues.",
            ApplyOps = [RegOp.SetDword(SpeechKey, "AllowSpeechRecognition", 0)],
            RemoveOps = [RegOp.DeleteValue(SpeechKey, "AllowSpeechRecognition")],
            DetectOps = [RegOp.CheckDword(SpeechKey, "AllowSpeechRecognition", 0)],
        },
        new TweakDef
        {
            Id = "spkinput-disable-voice-activation",
            Label = "Block Always-On Voice Activation",
            Category = "Privacy — Location Sensors",
            Description =
                "Sets AllowVoiceActivation=0 in the Speech Group Policy key. "
                + "Prevents applications from using the always-on voice listening hook (keyword detection). "
                + "Eliminates the continuous microphone monitoring required for wake words ('Hey Cortana', etc.), "
                + "removing a permanent audio capture pipeline from the endpoint.",
            Tags = ["speech", "voice-activation", "wake-word", "microphone", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Always-on wake word detection is disabled; microphone not continuously monitored.",
            ApplyOps = [RegOp.SetDword(SpeechKey, "AllowVoiceActivation", 0)],
            RemoveOps = [RegOp.DeleteValue(SpeechKey, "AllowVoiceActivation")],
            DetectOps = [RegOp.CheckDword(SpeechKey, "AllowVoiceActivation", 0)],
        },
        new TweakDef
        {
            Id = "spkinput-disable-speech-model-update",
            Label = "Block Automatic Speech Model Updates",
            Category = "Privacy — Location Sensors",
            Description =
                "Sets AllowSpeechModelUpdate=0 in the SpeechModel Group Policy key. "
                + "Prevents Windows from automatically downloading and applying updated cloud or on-device "
                + "speech recognition model files. Stabilises speech behaviour in validated regulated "
                + "environments where untested model changes could affect accessibility tools.",
            Tags = ["speech", "model-update", "policy", "enterprise", "compliance"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "Speech model files are frozen; updates require IT-managed deployment.",
            ApplyOps = [RegOp.SetDword(ModelKey, "AllowSpeechModelUpdate", 0)],
            RemoveOps = [RegOp.DeleteValue(ModelKey, "AllowSpeechModelUpdate")],
            DetectOps = [RegOp.CheckDword(ModelKey, "AllowSpeechModelUpdate", 0)],
        },
        new TweakDef
        {
            Id = "spkinput-disable-speech-telemetry",
            Label = "Disable Speech Input Telemetry",
            Category = "Privacy — Location Sensors",
            Description =
                "Sets AllowSpeechTelemetry=0 in the Speech Group Policy key. "
                + "Blocks the Speech subsystem from sending diagnostic voice data, recognition accuracy "
                + "metrics, and corrected text snippets to Microsoft for model improvement. "
                + "Audio utterances and transcription corrections are classified as personal data under GDPR/HIPAA.",
            Tags = ["speech", "telemetry", "privacy", "compliance", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Speech telemetry suppressed; no voice samples or transcription data transmitted.",
            ApplyOps = [RegOp.SetDword(SpeechKey, "AllowSpeechTelemetry", 0)],
            RemoveOps = [RegOp.DeleteValue(SpeechKey, "AllowSpeechTelemetry")],
            DetectOps = [RegOp.CheckDword(SpeechKey, "AllowSpeechTelemetry", 0)],
        },
        new TweakDef
        {
            Id = "spkinput-disable-voice-typing",
            Label = "Disable Voice Typing via Policy",
            Category = "Privacy — Location Sensors",
            Description =
                "Sets AllowVoiceTyping=0 in the Speech Group Policy key. "
                + "Disables the Voice Typing feature (Win+H) systemwide via Group Policy. "
                + "Prevents users from dictating text into any application, stopping the microphone "
                + "activation path associated with dictation on shared and kiosk workstations.",
            Tags = ["speech", "voice-typing", "dictation", "microphone", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Voice Typing (Win+H) is disabled; microphone not used for dictation.",
            ApplyOps = [RegOp.SetDword(SpeechKey, "AllowVoiceTyping", 0)],
            RemoveOps = [RegOp.DeleteValue(SpeechKey, "AllowVoiceTyping")],
            DetectOps = [RegOp.CheckDword(SpeechKey, "AllowVoiceTyping", 0)],
        },
        new TweakDef
        {
            Id = "spkinput-disable-cortana-voice",
            Label = "Disable Cortana Voice Interaction via Policy",
            Category = "Privacy — Location Sensors",
            Description =
                "Sets AllowCortanaVoice=0 in the Speech Group Policy key. "
                + "Prevents Cortana from accepting voice input and responding to spoken queries. "
                + "Complements the Cortana keyboard disable by also closing the audio/microphone channel "
                + "used for Cortana's voice assistant functionality.",
            Tags = ["speech", "cortana", "voice", "microphone", "policy", "privacy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Cortana no longer accepts voice queries; keyboard interaction unaffected.",
            ApplyOps = [RegOp.SetDword(SpeechKey, "AllowCortanaVoice", 0)],
            RemoveOps = [RegOp.DeleteValue(SpeechKey, "AllowCortanaVoice")],
            DetectOps = [RegOp.CheckDword(SpeechKey, "AllowCortanaVoice", 0)],
        },
        new TweakDef
        {
            Id = "spkinput-disable-speech-personalization",
            Label = "Block Speech Personalisation Data Collection",
            Category = "Privacy — Location Sensors",
            Description =
                "Sets AllowSpeechPersonalization=0 in the Speech Group Policy key. "
                + "Stops Windows from collecting contacts, calendar events, frequently typed words, "
                + "and app usage patterns to personalise speech recognition accuracy. "
                + "This dataset would stay on-device but its aggregation represents a privacy concern "
                + "in regulated environments where data minimisation principles apply.",
            Tags = ["speech", "personalisation", "privacy", "policy", "compliance"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Speech personalisation disabled; recognition accuracy unchanged.",
            ApplyOps = [RegOp.SetDword(SpeechKey, "AllowSpeechPersonalization", 0)],
            RemoveOps = [RegOp.DeleteValue(SpeechKey, "AllowSpeechPersonalization")],
            DetectOps = [RegOp.CheckDword(SpeechKey, "AllowSpeechPersonalization", 0)],
        },
        new TweakDef
        {
            Id = "spkinput-disable-voice-access-start",
            Label = "Prevent Voice Access from Starting at Login",
            Category = "Privacy — Location Sensors",
            Description =
                "Sets AllowVoiceAccessStartup=0 in the Speech Group Policy key. "
                + "Prevents the Windows Voice Access feature from automatically starting when a user logs "
                + "into Windows. Voice Access requires persistent microphone access; letting it auto-start "
                + "runs an unnecessary audio capture pipeline on workstations not requiring accessibility.",
            Tags = ["speech", "voice-access", "startup", "microphone", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Voice Access does not auto-start; users can still launch it manually.",
            ApplyOps = [RegOp.SetDword(SpeechKey, "AllowVoiceAccessStartup", 0)],
            RemoveOps = [RegOp.DeleteValue(SpeechKey, "AllowVoiceAccessStartup")],
            DetectOps = [RegOp.CheckDword(SpeechKey, "AllowVoiceAccessStartup", 0)],
        },
        new TweakDef
        {
            Id = "spkinput-restrict-online-speech-model",
            Label = "Block Online Speech Model Download",
            Category = "Privacy — Location Sensors",
            Description =
                "Sets AllowOnlineSpeechModel=0 in the SpeechModel Group Policy key. "
                + "Prevents Windows from downloading an enhanced online speech recognition model "
                + "that improves accuracy beyond the locally installed model. "
                + "Disabling the download removes a background network data transfer and pins "
                + "speech processing to on-device models vetted by the organisation.",
            Tags = ["speech", "model", "download", "network", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "Online speech model is not downloaded; on-device model used exclusively.",
            ApplyOps = [RegOp.SetDword(ModelKey, "AllowOnlineSpeechModel", 0)],
            RemoveOps = [RegOp.DeleteValue(ModelKey, "AllowOnlineSpeechModel")],
            DetectOps = [RegOp.CheckDword(ModelKey, "AllowOnlineSpeechModel", 0)],
        },
        new TweakDef
        {
            Id = "spkinput-disable-speech-access-across-lock",
            Label = "Disable Speech Recognition on Lock Screen",
            Category = "Privacy — Location Sensors",
            Description =
                "Sets AllowSpeechOnLockScreen=0 in the Speech Group Policy key. "
                + "Prevents voice assistants and speech recognition from accepting voice input when "
                + "the workstation screen is locked. Eliminates the attack surface where an attacker "
                + "with physical access to a locked machine can issue voice commands to local assistants.",
            Tags = ["speech", "lock-screen", "security", "policy", "physical-security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Voice assistant cannot be invoked from locked screen; prevents audio-based attacks.",
            ApplyOps = [RegOp.SetDword(SpeechKey, "AllowSpeechOnLockScreen", 0)],
            RemoveOps = [RegOp.DeleteValue(SpeechKey, "AllowSpeechOnLockScreen")],
            DetectOps = [RegOp.CheckDword(SpeechKey, "AllowSpeechOnLockScreen", 0)],
        },
    ];
}

internal static class PolicyReliabilityMonitor
{
    private const string RacKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Reliability";
    private const string WerKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting";
    private const string PcKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PCHealth\ErrorReporting";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "maint-reliability-shutdown-reason-text",
            Label = "Require Shutdown Reason Text",
            Category = "Privacy — Location Sensors",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets ShutdownReasonUI=1 and ReasonCodeRequired=1 in Reliability policy. "
                + "Forces users to select a shutdown reason and enter explanatory text when initiating a planned or unplanned shutdown. "
                + "Improves uptime tracking and post-incident root cause analysis in managed enterprise environments.",
            Tags = ["reliability", "shutdown", "reason", "audit", "uptime"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Prompts for shutdown reason; visible user impact at every shutdown/restart.",
            ApplyOps = [RegOp.SetDword(RacKey, "ShutdownReasonUI", 1), RegOp.SetDword(RacKey, "ReasonCodeRequired", 1)],
            RemoveOps = [RegOp.DeleteValue(RacKey, "ShutdownReasonUI"), RegOp.DeleteValue(RacKey, "ReasonCodeRequired")],
            DetectOps = [RegOp.CheckDword(RacKey, "ShutdownReasonUI", 1), RegOp.CheckDword(RacKey, "ReasonCodeRequired", 1)],
        },
        new TweakDef
        {
            Id = "maint-reliability-racevent-interval",
            Label = "Extend Reliability Event Logging Interval",
            Category = "Privacy — Location Sensors",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets TimeStampInterval=7 in Reliability policy, extending the RAC (Reliability Analysis Component) time-stamp interval to 7 days. "
                + "Reduces disk I/O for reliability data collection on endpoints where the default hourly reliability logging is excessive. "
                + "Useful for write-sensitive devices such as those with eMMC storage.",
            Tags = ["reliability", "rac", "logging", "interval", "disk-io"],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Reduces reliability event logging frequency; less granular uptime data.",
            ApplyOps = [RegOp.SetDword(RacKey, "TimeStampInterval", 7)],
            RemoveOps = [RegOp.DeleteValue(RacKey, "TimeStampInterval")],
            DetectOps = [RegOp.CheckDword(RacKey, "TimeStampInterval", 7)],
        },
        new TweakDef
        {
            Id = "maint-wer-disable-default-consent",
            Label = "Disable Windows Error Reporting Default Consent",
            Category = "Privacy — Location Sensors",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DefaultConsent=1 in WER policy (Always Ask). "
                + "Requires explicit user or administrator consent before any error report is sent to Microsoft. "
                + "Prevents automatic or silent submission of crash dumps and application error telemetry that may contain sensitive process memory contents.",
            Tags = ["wer", "error-reporting", "consent", "privacy", "telemetry"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Prompts before any error report upload; no silent data submission.",
            ApplyOps = [RegOp.SetDword(WerKey, "DefaultConsent", 1)],
            RemoveOps = [RegOp.DeleteValue(WerKey, "DefaultConsent")],
            DetectOps = [RegOp.CheckDword(WerKey, "DefaultConsent", 1)],
        },
        new TweakDef
        {
            Id = "maint-wer-disable-kernel-faults",
            Label = "Exclude Kernel-Level Faults from WER",
            Category = "Privacy — Location Sensors",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets ExcludeKernelFaults=1 in WER policy. "
                + "Prevents kernel-level crash events from being included in Windows Error Reporting submissions. "
                + "Kernel dumps can contain entire memory contents including encryption keys and privileged process memory, making them unsuitable for external submission.",
            Tags = ["wer", "kernel-dump", "crash", "memory", "security"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Excludes kernel crash data from WER submissions; reduces data leakage risk.",
            ApplyOps = [RegOp.SetDword(WerKey, "ExcludeKernelFaults", 1)],
            RemoveOps = [RegOp.DeleteValue(WerKey, "ExcludeKernelFaults")],
            DetectOps = [RegOp.CheckDword(WerKey, "ExcludeKernelFaults", 1)],
        },
        new TweakDef
        {
            Id = "maint-wer-disable-archive-behavior",
            Label = "Disable WER Problem Reporting Queue Archival",
            Category = "Privacy — Location Sensors",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DumpType=0 in WER policy. "
                + "Prevents Windows Error Reporting from archiving application crash mini-dumps to the local queue directory for later upload. "
                + "Reduces disk usage from accumulated crash dump files and prevents sensitive process memory from persisting on disk beyond the immediate crash event.",
            Tags = ["wer", "archive", "dump", "disk", "privacy"],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Prevents crash dump accumulation on disk; no deferred upload queue.",
            ApplyOps = [RegOp.SetDword(WerKey, "DumpType", 0)],
            RemoveOps = [RegOp.DeleteValue(WerKey, "DumpType")],
            DetectOps = [RegOp.CheckDword(WerKey, "DumpType", 0)],
        },
        new TweakDef
        {
            Id = "maint-pch-disable-all-error-reporting",
            Label = "Disable All PCHealth Error Reporting Channels",
            Category = "Privacy — Location Sensors",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets AllOrNone=1 and ShowUI=0 in PCHealth\\ErrorReporting policy. "
                + "Blocks the PCHealth component from showing any error reporting UI and from queuing reports to any reporting channel. "
                + "Complements the DoReport=0 setting to ensure the legacy error reporting subsystem is fully silent.",
            Tags = ["pchealth", "error-reporting", "legacy", "silent", "ui"],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Silences legacy PCHealth error dialogs and submission queue.",
            ApplyOps = [RegOp.SetDword(PcKey, "AllOrNone", 1), RegOp.SetDword(PcKey, "ShowUI", 0)],
            RemoveOps = [RegOp.DeleteValue(PcKey, "AllOrNone"), RegOp.DeleteValue(PcKey, "ShowUI")],
            DetectOps = [RegOp.CheckDword(PcKey, "AllOrNone", 1), RegOp.CheckDword(PcKey, "ShowUI", 0)],
        },
        new TweakDef
        {
            Id = "maint-pch-force-queue-mode",
            Label = "Set PCHealth Reporting to Queue Mode Only",
            Category = "Privacy — Location Sensors",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets ForceQueue=1 in PCHealth\\ErrorReporting policy. "
                + "Forces error reports to accumulate in a local queue rather than being submitted immediately or interactively. "
                + "Gives administrators time to review and approve queued reports before any data leaves the endpoint.",
            Tags = ["pchealth", "queue", "error-reporting", "review", "approval"],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Queues reports for admin review; no immediate uploads.",
            ApplyOps = [RegOp.SetDword(PcKey, "ForceQueue", 1)],
            RemoveOps = [RegOp.DeleteValue(PcKey, "ForceQueue")],
            DetectOps = [RegOp.CheckDword(PcKey, "ForceQueue", 1)],
        },
        new TweakDef
        {
            Id = "maint-pch-disable-report-by-app",
            Label = "Disable Per-Application Error Reporting Override",
            Category = "Privacy — Location Sensors",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets IncludeMicrosoftApps=0 and IncludeWindowsApps=0 in PCHealth\\ErrorReporting policy. "
                + "Prevents individual Microsoft and Windows applications from independently initiating error reports through the PCHealth channel. "
                + "Ensures that the enterprise error reporting policy cannot be overridden by per-application reporting preferences.",
            Tags = ["pchealth", "per-app", "error-reporting", "override", "policy"],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Blocks per-app PCHealth error submissions regardless of app preference.",
            ApplyOps = [RegOp.SetDword(PcKey, "IncludeMicrosoftApps", 0), RegOp.SetDword(PcKey, "IncludeWindowsApps", 0)],
            RemoveOps = [RegOp.DeleteValue(PcKey, "IncludeMicrosoftApps"), RegOp.DeleteValue(PcKey, "IncludeWindowsApps")],
            DetectOps = [RegOp.CheckDword(PcKey, "IncludeMicrosoftApps", 0), RegOp.CheckDword(PcKey, "IncludeWindowsApps", 0)],
        },
    ];
}
