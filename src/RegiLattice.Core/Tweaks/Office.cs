namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Office
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "office-disable-office-telemetry",
            Label = "Disable Office Telemetry",
            Category = "Office",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables Microsoft Office telemetry, feedback, and connected services data collection across all installed versions (2010-365).",
            Tags = ["office", "telemetry", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Office\Common\ClientTelemetry"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\Common\ClientTelemetry", "DisableTelemetry", 1),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Feedback", "Enabled", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Privacy", "DisconnectedState", 2),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Privacy", "UserContentDisabled", 2),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Privacy", "DownloadContentDisabled", 2),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Privacy", "ControllerConnectedServicesEnabled", 2),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Common\ClientTelemetry", "DisableTelemetry", 1),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Common\ClientTelemetry", "SendTelemetry", 3),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\Common\ClientTelemetry", "DisableTelemetry"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Feedback", "Enabled"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Common\ClientTelemetry", "DisableTelemetry"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Common\ClientTelemetry", "SendTelemetry"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\Common\ClientTelemetry", "DisableTelemetry", 1)],
        },
        new TweakDef
        {
            Id = "office-disable-office-start-screen",
            Label = "Disable Office Start Screen",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Skips the Office Start screen and opens directly to a blank document (all versions).",
            Tags = ["office", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\General"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\General", "DisableBootToOfficeStart", 1), RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\General", "ShownFirstRunOptin", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\General", "DisableBootToOfficeStart"), RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\General", "ShownFirstRunOptin")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\General", "DisableBootToOfficeStart", 1)],
        },
        new TweakDef
        {
            Id = "office-disable-office-connected",
            Label = "Disable Office Connected Experiences",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = false,
            Description = "Disables cloud-powered Office features (Designer, Editor, etc.) across all versions.",
            Tags = ["office", "privacy", "cloud"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Privacy"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Privacy", "ControllerConnectedServicesEnabled", 2), RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Privacy", "DisconnectedState", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Privacy", "ControllerConnectedServicesEnabled"), RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Privacy", "DisconnectedState")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Privacy", "DisconnectedState", 2)],
        },
        new TweakDef
        {
            Id = "office-disable-office-hwaccel",
            Label = "Disable Office Hardware Acceleration",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables GPU hardware acceleration in Office apps to fix display glitches (all versions).",
            Tags = ["office", "performance", "gpu"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Graphics"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Graphics", "DisableHardwareAcceleration", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Graphics", "DisableHardwareAcceleration")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Graphics", "DisableHardwareAcceleration", 1)],
        },
        new TweakDef
        {
            Id = "office-macro-trust",
            Label = "Trust VBA Macros (All Apps)",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = false,
            Description =
                "Lowers macro security to 'Enable all' and trusts the VBA project object model (Word, Excel, PowerPoint, Access, Outlook, Publisher).",
            Tags = ["office", "macros", "security"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Word\Security"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Word\Security", "VBAWarnings", 1), RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Word\Security", "AccessVBOM", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Word\Security", "VBAWarnings"), RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Word\Security", "AccessVBOM")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Word\Security", "VBAWarnings", 1)],
        },
        new TweakDef
        {
            Id = "office-autosave-fast",
            Label = "Set Office AutoRecover to 2 min",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets the AutoRecover interval to 2 minutes for Word, Excel, and PowerPoint (all versions).",
            Tags = ["office", "autosave", "recovery"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Word\Options"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Word\Options", "SaveInterval", 2)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Word\Options", "SaveInterval", 10)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Word\Options", "SaveInterval", 2)],
        },
        new TweakDef
        {
            Id = "office-disable-office-animations",
            Label = "Disable Office UI Animations",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables transitions and animations in Office apps for snappier UI.",
            Tags = ["office", "performance", "animations"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Graphics"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Graphics", "DisableAnimations", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Graphics", "DisableAnimations")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Graphics", "DisableAnimations", 1)],
        },
        new TweakDef
        {
            Id = "office-disable-feedback-button",
            Label = "Disable Office Feedback Button",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes the 'Send a Smile' / feedback button from the Office ribbon.",
            Tags = ["office", "feedback", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Feedback"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Feedback", "Enabled", 0), RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Feedback", "IncludeEmail", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Feedback", "Enabled"), RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Feedback", "IncludeEmail")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Feedback", "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "office-disable-office-updates",
            Label = "Disable Office Auto-Updates",
            Category = "Office",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables automatic Office updates so you control when to update.",
            Tags = ["office", "update", "control"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\officeupdate"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\officeupdate", "EnableAutomaticUpdates", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\officeupdate", "HideEnableDisableUpdates", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\officeupdate", "EnableAutomaticUpdates"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\officeupdate", "HideEnableDisableUpdates"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\officeupdate", "EnableAutomaticUpdates", 0),
            ],
        },
        new TweakDef
        {
            Id = "office-disable-telemetry-dash",
            Label = "Disable Office Telemetry Dashboard",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Office telemetry dashboard and diagnostic data collection. Reduces network traffic and CPU from Office telemetry agent. Default: Enabled. Recommended: Disabled.",
            Tags = ["office", "telemetry", "privacy", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\OSM"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\Common\ClientTelemetry", "DisableTelemetry", 1),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Feedback", "Enabled", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Privacy", "DisconnectedState", 2),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Privacy", "UserContentDisabled", 2),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Privacy", "DownloadContentDisabled", 2),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Privacy", "ControllerConnectedServicesEnabled", 2),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Common\ClientTelemetry", "DisableTelemetry", 1),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Common\ClientTelemetry", "SendTelemetry", 3),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\Common\ClientTelemetry", "DisableTelemetry"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Feedback", "Enabled"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Common\ClientTelemetry", "DisableTelemetry"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Common\ClientTelemetry", "SendTelemetry"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\Common\ClientTelemetry", "DisableTelemetry", 1)],
        },
        new TweakDef
        {
            Id = "office-disable-background-updates",
            Label = "Disable Office Background Updates (C2R)",
            Category = "Office",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables automatic background Office Click-to-Run updates. Updates must be applied manually through Office or WSUS. Default: Enabled. Recommended: Disabled for managed environments.",
            Tags = ["office", "updates", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Office\ClickToRun\Configuration"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Office\ClickToRun\Configuration", "UpdatesEnabled", "False")],
            RemoveOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Office\ClickToRun\Configuration", "UpdatesEnabled", "True")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Office\ClickToRun\Configuration", "UpdatesEnabled", "False")],
        },
        new TweakDef
        {
            Id = "office-disable-connected-experiences",
            Label = "Disable Office Connected Experiences",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables optional connected experiences in Office 365. Prevents cloud-powered features like LinkedIn integration and 3D maps. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["office", "connected", "privacy", "cloud"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Common\Privacy"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Common\Privacy", "ControllerConnectedServicesEnabled", 2), RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Common\Privacy", "DisconnectedState", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Common\Privacy", "ControllerConnectedServicesEnabled"), RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Common\Privacy", "DisconnectedState")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Privacy", "DisconnectedState", 2)],
        },
        new TweakDef
        {
            Id = "office-disable-feedback",
            Label = "Disable Office Feedback Prompts",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Office feedback survey prompts and the feedback button. Prevents interruptions during work. Default: Enabled. Recommended: Disabled.",
            Tags = ["office", "feedback", "survey", "notifications"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Common\Feedback"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Common\Feedback", "Enabled", 0), RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Common\Feedback", "IncludeEmail", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Common\Feedback", "Enabled"), RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Common\Feedback", "IncludeEmail")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Feedback", "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "office-disable-telemetry-dashboard",
            Label = "Disable Office Telemetry Dashboard",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Office telemetry dashboard logging and data upload. Prevents usage data collection via OSM. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["office", "telemetry", "dashboard", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\OSM"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\Common\ClientTelemetry", "DisableTelemetry", 1),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Feedback", "Enabled", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Privacy", "DisconnectedState", 2),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Privacy", "UserContentDisabled", 2),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Privacy", "DownloadContentDisabled", 2),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Privacy", "ControllerConnectedServicesEnabled", 2),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Common\ClientTelemetry", "DisableTelemetry", 1),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Common\ClientTelemetry", "SendTelemetry", 3),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\Common\ClientTelemetry", "DisableTelemetry"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Feedback", "Enabled"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Common\ClientTelemetry", "DisableTelemetry"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Common\ClientTelemetry", "SendTelemetry"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\Common\ClientTelemetry", "DisableTelemetry", 1)],
        },
        new TweakDef
        {
            Id = "office-disable-office-feedback",
            Label = "Disable Office Feedback (Policy)",
            Category = "Office",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables Office feedback collection via machine-level policy. Blocks feedback button and email inclusion in reports. Default: Enabled. Recommended: Disabled.",
            Tags = ["office", "feedback", "policy", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office\16.0\Common\Feedback"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office\16.0\Common\Feedback", "Enabled", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office\16.0\Common\Feedback", "IncludeEmail", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office\16.0\Common\Feedback", "Enabled"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office\16.0\Common\Feedback", "IncludeEmail"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office\16.0\Common\Feedback", "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "office-dark-theme",
            Label = "Set Office Visual Theme to Dark",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets the Office 365 visual theme to Dark Gray (theme 4). Reduces eye strain in low-light environments. Default: Colorful (0). Recommended: Dark for dark-mode users.",
            Tags = ["office", "theme", "dark", "appearance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common", "UI Theme", 4)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common", "UI Theme")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common", "UI Theme", 4)],
        },
        new TweakDef
        {
            Id = "office-disable-start-screen",
            Label = "Disable Office Start Screen",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Skips the Office start screen (template picker) and opens a blank document directly. Default: shown.",
            Tags = ["office", "start-screen", "template", "ui"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\General"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\General", "ShownFirstRunOptin", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\General", "ShownFirstRunOptin")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\General", "ShownFirstRunOptin", 1)],
        },
        new TweakDef
        {
            Id = "office-disable-hardware-acceleration",
            Label = "Disable Office Hardware Acceleration",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables hardware graphics acceleration in Office apps. Fixes rendering glitches on some GPUs. Default: enabled.",
            Tags = ["office", "hardware", "acceleration", "gpu"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Graphics"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Graphics", "DisableHardwareAcceleration", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Graphics", "DisableHardwareAcceleration")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Graphics", "DisableHardwareAcceleration", 1)],
        },
        new TweakDef
        {
            Id = "office-disable-animations",
            Label = "Disable Office Animations",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables UI animations in Office applications. Makes Office feel snappier. Default: enabled.",
            Tags = ["office", "animations", "ui", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Graphics"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Graphics", "DisableAnimations", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Graphics", "DisableAnimations")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Graphics", "DisableAnimations", 1)],
        },
        new TweakDef
        {
            Id = "office-set-default-save-local",
            Label = "Set Office Default Save to Local",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets Office default save location to local PC instead of OneDrive. Default: OneDrive.",
            Tags = ["office", "save", "local", "onedrive"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\General"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\General", "PreferCloudSaveLocations", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\General", "PreferCloudSaveLocations", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\General", "PreferCloudSaveLocations", 0)],
        },
        new TweakDef
        {
            Id = "office-disable-mini-toolbar",
            Label = "Disable Office Mini Toolbar",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the floating mini formatting toolbar that appears on text selection. Default: enabled.",
            Tags = ["office", "mini-toolbar", "selection", "ui"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Toolbars"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Toolbars", "QuickAccessToolbarStyle", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Toolbars", "QuickAccessToolbarStyle")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Toolbars", "QuickAccessToolbarStyle", 0)],
        },
        new TweakDef
        {
            Id = "office-disable-office-cloud-save",
            Label = "Disable Office Cloud Save Prompt",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the Office default-to-cloud save prompt. Files save locally by default instead of OneDrive. Default: cloud save prompt.",
            Tags = ["office", "cloud", "save", "onedrive"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\General"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\General", "PreferCloudSaveLocations", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\General", "PreferCloudSaveLocations")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\General", "PreferCloudSaveLocations", 0)],
        },
        new TweakDef
        {
            Id = "office-disable-office-linkedin",
            Label = "Disable Office LinkedIn Integration",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables LinkedIn integration in Microsoft Office applications. Removes LinkedIn profile cards and Resume Assistant. Default: enabled.",
            Tags = ["office", "linkedin", "integration", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\LinkedIn"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\LinkedIn", "OfficeLinkedIn", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\LinkedIn", "OfficeLinkedIn")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\LinkedIn", "OfficeLinkedIn", 0)],
        },
        new TweakDef
        {
            Id = "office-disable-office-recent-docs",
            Label = "Disable Office Recent Documents",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the recent documents list in Office applications. Enhances privacy by not tracking opened files. Default: enabled.",
            Tags = ["office", "recent", "documents", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\General"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\General", "DisableMRU", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\General", "DisableMRU")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\General", "DisableMRU", 1)],
        },
        new TweakDef
        {
            Id = "office-relax-office-protected-view",
            Label = "Relax Office Protected View",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = false,
            Description =
                "Disables Protected View for files from the internet in Word. Documents open directly without sandbox. Security risk — use cautiously. Default: protected.",
            Tags = ["office", "protected-view", "security", "relax"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Word\Security\ProtectedView"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Word\Security\ProtectedView", "DisableInternetFilesInPV", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Word\Security\ProtectedView", "DisableInternetFilesInPV"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Word\Security\ProtectedView", "DisableInternetFilesInPV", 1),
            ],
        },
        new TweakDef
        {
            Id = "office-first-run-off",
            Label = "Skip Office first-run opt-in experience",
            Category = "Office",
            Tags = ["office", "first-run", "onboarding"],
            NeedsAdmin = false,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common", "DisableClientFirstRunOptIn", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common", "DisableClientFirstRunOptIn")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common", "DisableClientFirstRunOptIn", 1)],
        },
        new TweakDef
        {
            Id = "office-ceip-off",
            Label = "Opt out of Office Customer Experience Improvement Program",
            Category = "Office",
            Tags = ["office", "telemetry", "ceip", "privacy"],
            NeedsAdmin = false,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\Common\ClientTelemetry", "DisableTelemetry", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\Common\ClientTelemetry", "DisableTelemetry")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\Common\ClientTelemetry", "DisableTelemetry", 1)],
        },
        new TweakDef
        {
            Id = "office-connected-services-off",
            Label = "Disable Office connected experiences",
            Category = "Office",
            Tags = ["office", "connected", "cloud", "privacy"],
            NeedsAdmin = false,
            CorpSafe = false,
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Privacy", "DisableConnectedExperiences", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Privacy", "DisableConnectedExperiences")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Privacy", "DisableConnectedExperiences", 1)],
        },
        new TweakDef
        {
            Id = "office-cloud-font-off",
            Label = "Disable Office cloud font download",
            Category = "Office",
            Tags = ["office", "cloud", "font", "network"],
            NeedsAdmin = false,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common", "CloudFontsEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common", "CloudFontsEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common", "CloudFontsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "office-document-recovery-off",
            Label = "Disable Word auto-document recovery",
            Category = "Office",
            Tags = ["office", "word", "recovery", "autosave"],
            NeedsAdmin = false,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Word\Options", "DontUseDocumentRecovery", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Word\Options", "DontUseDocumentRecovery")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Word\Options", "DontUseDocumentRecovery", 1)],
        },
        new TweakDef
        {
            Id = "office-macro-block-internet",
            Label = "Block Office macros from internet sources",
            Category = "Office",
            Tags = ["office", "macro", "security", "internet"],
            NeedsAdmin = false,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Excel\Security", "BlockContentExecutionFromInternet", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Excel\Security", "BlockContentExecutionFromInternet")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Excel\Security", "BlockContentExecutionFromInternet", 1),
            ],
        },
        new TweakDef
        {
            Id = "office-vba-warnings-high",
            Label = "Set VBA macro security to high (disable unsigned macros)",
            Category = "Office",
            Tags = ["office", "vba", "macro", "security"],
            NeedsAdmin = false,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Word\Security", "VBAWarnings", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Word\Security", "VBAWarnings")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Word\Security", "VBAWarnings", 2)],
        },
        new TweakDef
        {
            Id = "office-experiment-opt-out",
            Label = "Opt out of Office experiment programs",
            Category = "Office",
            Tags = ["office", "experiment", "telemetry", "privacy"],
            NeedsAdmin = false,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\ExperimentConfigs", "ExternalExperimentOptIn", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\ExperimentConfigs", "ExternalExperimentOptIn")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\ExperimentConfigs", "ExternalExperimentOptIn", 0),
            ],
        },
        new TweakDef
        {
            Id = "office-update-hide-notifications",
            Label = "Hide Office update notifications",
            Category = "Office",
            Tags = ["office", "update", "notification"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office\16.0\Common\OfficeUpdate", "HideUpdateNotifications", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office\16.0\Common\OfficeUpdate", "HideUpdateNotifications"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office\16.0\Common\OfficeUpdate", "HideUpdateNotifications", 1),
            ],
        },
        new TweakDef
        {
            Id = "office-external-content-off",
            Label = "Block Office from loading external content (images/OLE)",
            Category = "Office",
            Tags = ["office", "external", "content", "security", "network"],
            NeedsAdmin = false,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Word\Options", "fNoCalclinksOnOpen", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Word\Options", "fNoCalclinksOnOpen")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Word\Options", "fNoCalclinksOnOpen", 1)],
        },
        new TweakDef
        {
            Id = "office-disable-activex",
            Label = "Disable all ActiveX controls in Word",
            Category = "Office",
            Tags = ["office", "word", "activex", "security"],
            NeedsAdmin = false,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Word\Security", "DisableAllActiveX", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Word\Security", "DisableAllActiveX")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Word\Security", "DisableAllActiveX", 1)],
        },
        new TweakDef
        {
            Id = "office-disable-pv-internet-files",
            Label = "Disable Protected View for internet files in Word",
            Category = "Office",
            Tags = ["office", "word", "protected-view", "security"],
            NeedsAdmin = false,
            CorpSafe = false,
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Word\Security\ProtectedView", "DisableInternetFilesInPV", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Word\Security\ProtectedView", "DisableInternetFilesInPV")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Word\Security\ProtectedView", "DisableInternetFilesInPV", 1)],
        },
        new TweakDef
        {
            Id = "office-disable-pv-attachment-files",
            Label = "Disable Protected View for email attachments in Word",
            Category = "Office",
            Tags = ["office", "word", "protected-view", "attachments", "security"],
            NeedsAdmin = false,
            CorpSafe = false,
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Word\Security\ProtectedView", "DisableAttachmentsInPV", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Word\Security\ProtectedView", "DisableAttachmentsInPV")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Word\Security\ProtectedView", "DisableAttachmentsInPV", 1)],
        },
        new TweakDef
        {
            Id = "office-require-addin-signature",
            Label = "Require digital signature for Office add-ins",
            Category = "Office",
            Tags = ["office", "add-ins", "signature", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office\16.0\Common\Security", "RequireAddinSig", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office\16.0\Common\Security", "RequireAddinSig")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office\16.0\Common\Security", "RequireAddinSig", 1)],
        },
        new TweakDef
        {
            Id = "office-disable-dde-field-updates",
            Label = "Disable DDE field auto-update in Word (block injection risk)",
            Category = "Office",
            Tags = ["office", "word", "dde", "field", "security"],
            NeedsAdmin = false,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Word\Options", "DontUpdateLinks", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Word\Options", "DontUpdateLinks")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Word\Options", "DontUpdateLinks", 1)],
        },
        new TweakDef
        {
            Id = "office-disable-telemetry-osm",
            Label = "Disable Office telemetry (OSM agent logging)",
            Category = "Office",
            Tags = ["office", "telemetry", "osm", "logging", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office\16.0\OSM", "EnableLogging", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office\16.0\OSM", "EnableLogging")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office\16.0\OSM", "EnableLogging", 0)],
        },
        new TweakDef
        {
            Id = "office-disable-office-store-gpo",
            Label = "Disable Office Add-in Store access via GPO",
            Category = "Office",
            Tags = ["office", "store", "add-in", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office\16.0\Common", "DisableStoreApps", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office\16.0\Common", "DisableStoreApps")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office\16.0\Common", "DisableStoreApps", 1)],
        },
        new TweakDef
        {
            Id = "office-excel-vba-security-high",
            Label = "Set Excel VBA macro security to high (disable all without notification)",
            Category = "Office",
            Tags = ["office", "excel", "vba", "macros", "security"],
            NeedsAdmin = false,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Excel\Security", "VBAWarnings", 4)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Excel\Security", "VBAWarnings")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Excel\Security", "VBAWarnings", 4)],
        },
        new TweakDef
        {
            Id = "office-ppt-vba-security-high",
            Label = "Set PowerPoint VBA macro security to high (disable all without notification)",
            Category = "Office",
            Tags = ["office", "powerpoint", "vba", "macros", "security"],
            NeedsAdmin = false,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\PowerPoint\Security", "VBAWarnings", 4)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\PowerPoint\Security", "VBAWarnings")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\PowerPoint\Security", "VBAWarnings", 4)],
        },
        new TweakDef
        {
            Id = "office-disable-diagnostic-feedback",
            Label = "Disable Office diagnostic data in feedback submissions",
            Category = "Office",
            Tags = ["office", "feedback", "diagnostic", "privacy"],
            NeedsAdmin = false,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Feedback", "IncludeDiagnosticDataInFeedback", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Feedback", "IncludeDiagnosticDataInFeedback")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Feedback", "IncludeDiagnosticDataInFeedback", 0)],
        },
    ];
}
