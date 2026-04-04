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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\General", "DisableBootToOfficeStart", 1),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\General", "ShownFirstRunOptin", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\General", "DisableBootToOfficeStart"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\General", "ShownFirstRunOptin"),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Privacy", "ControllerConnectedServicesEnabled", 2),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Privacy", "DisconnectedState", 2),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Privacy", "ControllerConnectedServicesEnabled"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Privacy", "DisconnectedState"),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Word\Security", "VBAWarnings", 1),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Word\Security", "AccessVBOM", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Word\Security", "VBAWarnings"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Word\Security", "AccessVBOM"),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Feedback", "Enabled", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Feedback", "IncludeEmail", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Feedback", "Enabled"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Feedback", "IncludeEmail"),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Common\Privacy", "ControllerConnectedServicesEnabled", 2),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Common\Privacy", "DisconnectedState", 2),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Common\Privacy", "ControllerConnectedServicesEnabled"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Common\Privacy", "DisconnectedState"),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Common\Feedback", "Enabled", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Common\Feedback", "IncludeEmail", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Common\Feedback", "Enabled"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Common\Feedback", "IncludeEmail"),
            ],
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
            Id = "office-disable-pv-attachment-files",
            Label = "Disable Protected View for email attachments in Word",
            Category = "Office",
            Tags = ["office", "word", "protected-view", "attachments", "security"],
            NeedsAdmin = false,
            CorpSafe = false,
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Word\Security\ProtectedView", "DisableAttachmentsInPV", 1)],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Word\Security\ProtectedView", "DisableAttachmentsInPV"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Word\Security\ProtectedView", "DisableAttachmentsInPV", 1),
            ],
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

// ── merged from Adobe.cs ────────────────────────────────────────
internal static class Adobe
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "adobe-enable-adobe-protected-mode",
            Label = "Enable Adobe Protected Mode",
            Category = "Office",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables Protected Mode sandbox and Protected View for all PDF files.",
            Tags = ["adobe", "security", "sandbox"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown", "bProtectedMode", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown", "iProtectedView", 2),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown", "bProtectedMode"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown", "iProtectedView"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown", "bProtectedMode", 1)],
        },
        new TweakDef
        {
            Id = "adobe-disable-cc-files",
            Label = "Disable Adobe Creative Cloud File Sync",
            Category = "Office",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Adobe Creative Cloud file synchronization.",
            Tags = ["adobe", "cloud", "sync"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\CCXWelcome"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\CCXWelcome", "SyncDisabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\CCXWelcome", "SyncDisabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\CCXWelcome", "SyncDisabled", 1)],
        },
        new TweakDef
        {
            Id = "adobe-disable-genuine-check",
            Label = "Disable Adobe Genuine Software Check",
            Category = "Office",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Adobe Genuine Software integrity check.",
            Tags = ["adobe", "genuine", "licensing"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\AdobeGenuineSoftware"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\AdobeGenuineSoftware", "AdobeGenuineEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\AdobeGenuineSoftware", "AdobeGenuineEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\AdobeGenuineSoftware", "AdobeGenuineEnabled", 0)],
        },
        new TweakDef
        {
            Id = "adobe-disable-crash-reporter",
            Label = "Disable Adobe Crash Reporter",
            Category = "Office",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Adobe crash reporting.",
            Tags = ["adobe", "telemetry", "crash"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\CCXProcess"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\CCXProcess", "CrashReporting", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\CCXProcess", "CrashReporting")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\CCXProcess", "CrashReporting", 0)],
        },
        new TweakDef
        {
            Id = "adobe-disable-home-screen",
            Label = "Disable Adobe Home Screen on Launch",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the Adobe home screen shown on application launch.",
            Tags = ["adobe", "ux", "home"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Adobe\CommonFiles\UsageCC"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Adobe\CommonFiles\UsageCC", "ShowHomeScreen", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Adobe\CommonFiles\UsageCC", "ShowHomeScreen", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Adobe\CommonFiles\UsageCC", "ShowHomeScreen", 0)],
        },
        new TweakDef
        {
            Id = "adobe-disable-font-sync",
            Label = "Disable Adobe Font Sync",
            Category = "Office",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Adobe Creative Cloud font synchronization.",
            Tags = ["adobe", "fonts", "sync"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\CCXDesktop"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\CCXDesktop", "DisableFontSync", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\CCXDesktop", "DisableFontSync")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\CCXDesktop", "DisableFontSync", 1)],
        },
        new TweakDef
        {
            Id = "adobe-disable-updater",
            Label = "Disable Adobe Updater",
            Category = "Office",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Adobe Acrobat Reader automatic updater. Updates must be applied manually. Default: Enabled. Recommended: Disabled for managed environments.",
            Tags = ["adobe", "updater", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\ARM\1\ARM"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Adobe\Adobe ARM\1.0\ARM", "iCheckReader", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown", "bUpdater", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Adobe Acrobat\DC\FeatureLockDown", "bUpdater", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Adobe\Adobe ARM\1.0\ARM", "iCheckReader", 3),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown", "bUpdater"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Adobe Acrobat\DC\FeatureLockDown", "bUpdater"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Adobe\Adobe ARM\1.0\ARM", "iCheckReader", 0)],
        },
        new TweakDef
        {
            Id = "adobe-reduce-memory",
            Label = "Adobe Reduce Memory Usage",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Forces Adobe Reader to reuse existing instances instead of spawning new processes. Reduces memory footprint. Default: New instance. Recommended: Reuse.",
            Tags = ["adobe", "memory", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Adobe\Acrobat Reader\DC\AVGeneral"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Adobe\Acrobat Reader\DC\AVGeneral", "bReuseAcrobatInstance", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Adobe\Acrobat Reader\DC\AVGeneral", "bReuseAcrobatInstance")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Adobe\Acrobat Reader\DC\AVGeneral", "bReuseAcrobatInstance", 1)],
        },
        new TweakDef
        {
            Id = "adobe-disable-cloud-sync",
            Label = "Disable Adobe Creative Cloud Sync",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Adobe Creative Cloud file and settings sync. Reduces background network activity and cloud dependency. Default: Enabled. Recommended: Disabled on managed machines.",
            Tags = ["adobe", "cloud", "sync", "creative-cloud"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Adobe\CreativeCloud"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown\cCloud", "bDisableADCFileStore", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown\cCloud", "bUpdatesHidden", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown\cCloud", "bDisableADCFileStore"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown\cCloud", "bUpdatesHidden"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown\cCloud", "bDisableADCFileStore", 1),
            ],
        },
        new TweakDef
        {
            Id = "adobe-disable-acrobat-cloud",
            Label = "Disable Adobe Acrobat Cloud Services",
            Category = "Office",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Adobe Document Cloud file store integration in Acrobat Reader. Prevents cloud save prompts. Default: enabled. Recommended: disabled.",
            Tags = ["adobe", "acrobat", "cloud", "document-cloud"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown\cCloud"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown\cCloud", "bDisableADCFileStore", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown\cCloud", "bDisableADCFileStore"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown\cCloud", "bDisableADCFileStore", 1),
            ],
        },
        new TweakDef
        {
            Id = "adobe-pdf-single-page-view",
            Label = "Set PDF Default View to Single Page",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets the default PDF page layout to single page view. Overrides continuous scroll as the default. Default: continuous. Recommended: single page.",
            Tags = ["adobe", "pdf", "view", "layout", "single-page"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Adobe\Acrobat Reader\DC\AVGeneral"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Adobe\Acrobat Reader\DC\AVGeneral", "iPageViewLayoutMode", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Adobe\Acrobat Reader\DC\AVGeneral", "iPageViewLayoutMode")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Adobe\Acrobat Reader\DC\AVGeneral", "iPageViewLayoutMode", 0)],
        },
        new TweakDef
        {
            Id = "adobe-disable-welcome-screen",
            Label = "Disable Adobe What's New Screen",
            Category = "Office",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the What's New promotional screen shown after Adobe Reader updates. Different from the start screen. Default: shown. Recommended: hidden.",
            Tags = ["adobe", "welcome", "whats-new", "ux"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown\cWelcomeScreen"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown\cWelcomeScreen",
                    "bShowWelcomeScreen",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown\cWelcomeScreen",
                    "bShowWelcomeScreen"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown\cWelcomeScreen",
                    "bShowWelcomeScreen",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "adobe-disable-reader-cloud-sync",
            Label = "Disable Reader Cloud Sync",
            Category = "Office",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Adobe Reader cloud document synchronization. Prevents files from being synced to Adobe Document Cloud. Default: enabled.",
            Tags = ["adobe", "cloud", "sync", "reader"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown\cCloud"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown\cCloud", "bAdobeSendPluginToggle", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown\cCloud", "bAdobeSendPluginToggle"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown\cCloud", "bAdobeSendPluginToggle", 0),
            ],
        },
        new TweakDef
        {
            Id = "adobe-disable-sharepoint-integration",
            Label = "Disable SharePoint Integration",
            Category = "Office",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Adobe Reader integration with SharePoint and Office 365. Default: enabled.",
            Tags = ["adobe", "sharepoint", "office365", "integration"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown\cSharePoint"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown\cSharePoint",
                    "bDisableSharePointFeatures",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown\cSharePoint",
                    "bDisableSharePointFeatures"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown\cSharePoint",
                    "bDisableSharePointFeatures",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "adobe-disable-webmail-integration",
            Label = "Disable Webmail Integration",
            Category = "Office",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Adobe Reader webmail integration for sending files via web-based email. Default: enabled.",
            Tags = ["adobe", "webmail", "email", "integration"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown\cWebmailProfiles"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown\cWebmailProfiles",
                    "bDisableWebmail",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown\cWebmailProfiles",
                    "bDisableWebmail"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown\cWebmailProfiles",
                    "bDisableWebmail",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "adobe-disable-fill-sign",
            Label = "Disable Fill & Sign Feature",
            Category = "Office",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Fill & Sign feature in Adobe Reader. Prevents use of electronic signature in PDFs. Default: enabled.",
            Tags = ["adobe", "fill", "sign", "pdf"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown", "bDisableFillSign", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown", "bDisableFillSign")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown", "bDisableFillSign", 1)],
        },
        new TweakDef
        {
            Id = "adobe-disable-acrobat-upsell-banners",
            Label = "Disable Acrobat Upsell Banners",
            Category = "Office",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Welcome Screen that appears when launching Adobe Acrobat Reader. Default: shown.",
            Tags = ["adobe", "welcome", "startup", "reader"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown", "bToggleToDoList", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown", "bToggleToDoList")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown", "bToggleToDoList", 0)],
        },
        new TweakDef
        {
            Id = "adobe-disable-adobe-javascript",
            Label = "Disable Adobe JavaScript",
            Category = "Office",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables JavaScript execution in Adobe Acrobat/Reader. Mitigates PDF-based JavaScript exploits. Default: enabled.",
            Tags = ["adobe", "javascript", "security", "pdf"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown", "bDisableJavaScript", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown", "bDisableJavaScript")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown", "bDisableJavaScript", 1)],
        },
        new TweakDef
        {
            Id = "adobe-disable-adobe-telemetry",
            Label = "Disable Adobe Telemetry",
            Category = "Office",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Adobe product usage and analytics telemetry. Prevents data collection by Adobe. Default: enabled.",
            Tags = ["adobe", "telemetry", "analytics", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown", "bUsageMeasurement", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown", "bUsageMeasurement")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown", "bUsageMeasurement", 0)],
        },
        new TweakDef
        {
            Id = "adobe-disable-adobe-update",
            Label = "Disable Adobe Auto-Update",
            Category = "Office",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Adobe automatic updater service. Prevents background update checks and downloads. Default: enabled.",
            Tags = ["adobe", "update", "auto-update", "service"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown", "bUpdater", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown", "bUpdater")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown", "bUpdater", 0)],
        },
        new TweakDef
        {
            Id = "adobe-disable-adobe-welcome",
            Label = "Disable Adobe Welcome Screen",
            Category = "Office",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Adobe Reader/Acrobat welcome screen and home view. Opens directly to recent files. Default: shown.",
            Tags = ["adobe", "welcome", "home", "startup"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown\cWelcomeScreen"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown\cWelcomeScreen",
                    "bShowWelcomeScreen",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown\cWelcomeScreen",
                    "bShowWelcomeScreen"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown\cWelcomeScreen",
                    "bShowWelcomeScreen",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "adobe-disable-analytics",
            Label = "Disable Adobe Analytics Reporting",
            Category = "Office",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Adobe experience analytics and crash reporting. Prevents diagnostic data uploads. Default: enabled.",
            Tags = ["adobe", "analytics", "crash", "reporting"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown", "bToggleFTE", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown", "bToggleFTE")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown", "bToggleFTE", 0)],
        },
        new TweakDef
        {
            Id = "adobe-disable-cef-subprocess",
            Label = "Disable Adobe CEF Subprocess",
            Category = "Office",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Adobe Chromium Embedded Framework subprocess. Prevents embedded browser for cloud services. Default: enabled.",
            Tags = ["adobe", "cef", "chromium", "subprocess"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown\cServices"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown\cServices",
                    "bToggleAdobeDocumentServices",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown\cServices",
                    "bToggleAdobeDocumentServices"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown\cServices",
                    "bToggleAdobeDocumentServices",
                    1
                ),
            ],
        },
    ];
}

// ── merged from LibreOffice.cs ────────────────────────────────────────
internal static class LibreOffice
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "lo-libreoffice-default-ooxml",
            Label = "Default Save as OOXML (docx/xlsx)",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets LibreOffice default save format to Microsoft OOXML (docx, xlsx, pptx) for better interoperability.",
            Tags = ["libreoffice", "format", "compatibility"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\LibreOffice\DefaultFormat"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\LibreOffice\DefaultFormat", "Writer", "MS Word 2007 XML"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\LibreOffice\DefaultFormat", "Calc", "Calc MS Excel 2007 XML"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\LibreOffice\DefaultFormat", "Impress", "Impress MS PowerPoint 2007 XML"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\LibreOffice\DefaultFormat", "Writer"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\LibreOffice\DefaultFormat", "Calc"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\LibreOffice\DefaultFormat", "Impress"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Software\LibreOffice\DefaultFormat", "Writer", "MS Word 2007 XML")],
        },
        new TweakDef
        {
            Id = "lo-libreoffice-disable-crash-reporting",
            Label = "Disable LibreOffice Crash Reporting",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables LibreOffice crash reporting via UNO Misc setting.",
            Tags = ["libreoffice", "telemetry", "privacy", "crash"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\LibreOffice\UNO\Misc"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\LibreOffice\CrashReport", "Enable", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\LibreOffice\CrashReport", "AutoSubmit", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\LibreOffice\CrashReport", "Enable"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\LibreOffice\CrashReport", "AutoSubmit"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\LibreOffice\CrashReport", "Enable", 0)],
        },
        new TweakDef
        {
            Id = "lo-libreoffice-disable-online-update",
            Label = "Disable LibreOffice Online Update Check",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the LibreOffice online update check.",
            Tags = ["libreoffice", "update", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\LibreOffice\UNO\Misc"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Software\LibreOffice\UNO\Misc", "AutoCheckEnabled", "false")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Software\LibreOffice\UNO\Misc", "AutoCheckEnabled", "true")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Software\LibreOffice\UNO\Misc", "AutoCheckEnabled", "false")],
        },
        new TweakDef
        {
            Id = "lo-libreoffice-disable-startcenter-news",
            Label = "Disable Start Center Recent News",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the recent news feed on the LibreOffice Start Center.",
            Tags = ["libreoffice", "startcenter", "ux", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\LibreOffice\UNO\Misc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\LibreOffice\StartCenter", "ShowStartCenter", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\LibreOffice\StartCenter", "ShowStartCenter")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\LibreOffice\StartCenter", "ShowStartCenter", 0)],
        },
        new TweakDef
        {
            Id = "lo-libreoffice-disable-macros",
            Label = "Disable LibreOffice Macro Execution",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets LibreOffice macro security level to Very High (3), effectively disabling macro execution.",
            Tags = ["libreoffice", "macros", "security"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\LibreOffice\UNO\Misc"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Software\LibreOffice\UNO\Misc", "MacroSecurityLevel", "3")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Software\LibreOffice\UNO\Misc", "MacroSecurityLevel", "1")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Software\LibreOffice\UNO\Misc", "MacroSecurityLevel", "3")],
        },
        new TweakDef
        {
            Id = "lo-libreoffice-disable-send-feedback",
            Label = "Disable LibreOffice Send Feedback",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the LibreOffice send feedback feature.",
            Tags = ["libreoffice", "telemetry", "privacy", "feedback"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\LibreOffice\UNO\Misc"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Software\LibreOffice\UNO\Misc", "SendFeedback", "false")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Software\LibreOffice\UNO\Misc", "SendFeedback", "true")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Software\LibreOffice\UNO\Misc", "SendFeedback", "false")],
        },
        new TweakDef
        {
            Id = "lo-libre-disable-java",
            Label = "Disable LibreOffice Java Runtime",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Java runtime in LibreOffice. Reduces memory usage and startup time. Some wizards/macros may require Java. Default: Enabled. Recommended: Disabled.",
            Tags = ["libreoffice", "java", "performance", "memory"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\LibreOffice\UNO\Misc"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Software\LibreOffice\UNO\Misc", "JavaEnabled", "false")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Software\LibreOffice\UNO\Misc", "JavaEnabled", "true")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Software\LibreOffice\UNO\Misc", "JavaEnabled", "false")],
        },
        new TweakDef
        {
            Id = "lo-libre-autosave-interval",
            Label = "Reduce LibreOffice Auto-Save Interval",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets LibreOffice auto-save interval to 3 minutes for better crash recovery. Default: 10 minutes. Recommended: 3 minutes.",
            Tags = ["libreoffice", "autosave", "recovery"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\LibreOffice\UNO\Misc"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Software\LibreOffice\UNO\Misc", "AutoSaveTimeIntervall", "3")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Software\LibreOffice\UNO\Misc", "AutoSaveTimeIntervall", "10")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Software\LibreOffice\UNO\Misc", "AutoSaveTimeIntervall", "3")],
        },
        new TweakDef
        {
            Id = "lo-libreoffice-disable-recovery",
            Label = "Disable LibreOffice Document Recovery",
            Category = "Office",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables LibreOffice automatic document recovery via Group Policy. Prevents crash recovery prompts on startup. Default: Enabled. Recommended: Disabled for managed environments.",
            Tags = ["libreoffice", "recovery", "autosave", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Recovery\Recovery"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\LibreOffice\Recovery", "AutoSaveEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\LibreOffice\Recovery", "AutoSaveEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\LibreOffice\Recovery", "AutoSaveEnabled", 0)],
        },
        new TweakDef
        {
            Id = "lo-disable-crash-recovery",
            Label = "Disable LibreOffice Crash Recovery Prompt",
            Category = "Office",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables crash recovery prompt on LibreOffice startup. Default: Enabled. Recommended: Disabled.",
            Tags = ["libreoffice", "crash", "recovery", "prompt"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Recovery\RecoveryList"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\LibreOffice\CrashReport", "Enable", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\LibreOffice\CrashReport", "AutoSubmit", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\LibreOffice\CrashReport", "Enable"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\LibreOffice\CrashReport", "AutoSubmit"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\LibreOffice\CrashReport", "Enable", 0)],
        },
        new TweakDef
        {
            Id = "lo-disable-update-check",
            Label = "Disable LibreOffice Online Update Check",
            Category = "Office",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables automatic online update checks in LibreOffice via policy. Default: Enabled. Recommended: Disabled.",
            Tags = ["libreoffice", "update", "check", "online"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Jobs\AutoCheckUpdate"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Jobs\AutoCheckUpdate", "AutoCheckEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Jobs\AutoCheckUpdate", "AutoCheckEnabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Jobs\AutoCheckUpdate",
                    "AutoCheckEnabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "lo-disable-crash-reporter",
            Label = "Disable LibreOffice Crash Reporter",
            Category = "Office",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the LibreOffice crash reporting dialog. Default: enabled.",
            Tags = ["libreoffice", "crash", "reporter", "telemetry"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Recovery"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Recovery", "CrashReporter", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Recovery", "CrashReporter")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Recovery", "CrashReporter", 0)],
        },
        new TweakDef
        {
            Id = "lo-disable-start-center",
            Label = "Disable LibreOffice Start Center",
            Category = "Office",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the LibreOffice Start Center splash screen on launch. Default: enabled.",
            Tags = ["libreoffice", "start-center", "splash", "ui"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Setup"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Setup", "ShowStartCenter", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Setup", "ShowStartCenter")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Setup", "ShowStartCenter", 0)],
        },
        new TweakDef
        {
            Id = "lo-disable-macro-execution",
            Label = "Disable LibreOffice Macro Execution",
            Category = "Office",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets LibreOffice macro security to Very High, blocking all macros. Prevents macro-based malware. Default: Medium.",
            Tags = ["libreoffice", "macro", "security", "block"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Common\Security\Scripting"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Common\Security\Scripting",
                    "MacroSecurityLevel",
                    3
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Common\Security\Scripting",
                    "MacroSecurityLevel"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Common\Security\Scripting",
                    "MacroSecurityLevel",
                    3
                ),
            ],
        },
        new TweakDef
        {
            Id = "lo-disable-online-update-check",
            Label = "Disable LibreOffice Online Update Check",
            Category = "Office",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the online update check (separate from auto-update). No network calls to check for new versions. Default: enabled.",
            Tags = ["libreoffice", "update", "online", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Jobs"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Jobs", "UpdateCheck", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Jobs", "UpdateCheck")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Jobs", "UpdateCheck", 0)],
        },
        new TweakDef
        {
            Id = "lo-disable-tip-of-day",
            Label = "Disable LibreOffice Tip of the Day",
            Category = "Office",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Tip of the Day dialog shown on LibreOffice launch. Default: enabled.",
            Tags = ["libreoffice", "tip", "dialog", "ui"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Common\Misc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Common\Misc", "ShowTipOfTheDay", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Common\Misc", "ShowTipOfTheDay")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Common\Misc", "ShowTipOfTheDay", 0),
            ],
        },
        new TweakDef
        {
            Id = "lo-disable-libreoffice-autoupdate",
            Label = "Disable LibreOffice Auto-Update",
            Category = "Office",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables automatic update checks for LibreOffice. Prevents background network requests for version checks. Default: enabled.",
            Tags = ["libreoffice", "update", "auto-update", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Jobs\Jobs\UpdateCheck\Arguments"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Jobs\Jobs\UpdateCheck\Arguments",
                    "AutoCheckEnabled",
                    "false"
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Jobs\Jobs\UpdateCheck\Arguments",
                    "AutoCheckEnabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Jobs\Jobs\UpdateCheck\Arguments",
                    "AutoCheckEnabled",
                    "false"
                ),
            ],
        },
        new TweakDef
        {
            Id = "lo-disable-libreoffice-crash-reporter",
            Label = "Disable LibreOffice Crash Reporter",
            Category = "Office",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables LibreOffice crash reporting. Prevents crash dumps from being sent to The Document Foundation. Default: enabled.",
            Tags = ["libreoffice", "crash", "reporter", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Common\Misc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Common\Misc", "CrashReport", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Common\Misc", "CrashReport")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Common\Misc", "CrashReport", 0)],
        },
        new TweakDef
        {
            Id = "lo-disable-libreoffice-recovery",
            Label = "Disable LibreOffice Auto-Recovery",
            Category = "Office",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables LibreOffice automatic recovery and document backup. Reduces disk I/O from periodic saves. Default: enabled.",
            Tags = ["libreoffice", "recovery", "auto-save", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Recovery\AutoSave"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Recovery\AutoSave", "Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Recovery\AutoSave", "Enabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Recovery\AutoSave", "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "lo-disable-libreoffice-startcenter",
            Label = "Disable LibreOffice Start Center",
            Category = "Office",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the LibreOffice Start Center splash screen. Opens directly to a blank document. Default: shown.",
            Tags = ["libreoffice", "start-center", "splash", "startup"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Setup\Office"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Setup\Office", "ShowStartCenter", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Setup\Office", "ShowStartCenter")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Setup\Office", "ShowStartCenter", 0)],
        },
        new TweakDef
        {
            Id = "lo-enable-hw-acceleration",
            Label = "Enable LibreOffice Hardware Acceleration",
            Category = "Office",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables hardware (GPU) acceleration for LibreOffice rendering. Improves drawing and display performance. Default: disabled on some systems.",
            Tags = ["libreoffice", "gpu", "acceleration", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Common\VCL"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Common\VCL", "UseOpenGL", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Common\VCL", "UseOpenGL")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Common\VCL", "UseOpenGL", 1)],
        },
        new TweakDef
        {
            Id = "lo-libreoffice-default-handler",
            Label = "Set LibreOffice as Default Office Handler",
            Category = "Office",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Configures LibreOffice as the default handler for Office document types (.docx, .xlsx, .pptx). Default: system-determined.",
            Tags = ["libreoffice", "default", "handler", "office"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\LibreOffice\UNO\InstallPath"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Setup\Office", "MSDocumentHandler", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Setup\Office", "MSDocumentHandler")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Setup\Office", "MSDocumentHandler", 1)],
        },
        new TweakDef
        {
            Id = "lo-libreoffice-disable-macro-exec",
            Label = "Disable LibreOffice Macro Execution",
            Category = "Office",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables macro execution in LibreOffice documents. Prevents potentially malicious macros from running. Default: prompt.",
            Tags = ["libreoffice", "macro", "security", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Common\Security\Scripting"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Common\Security\Scripting",
                    "MacroSecurityLevel",
                    3
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Common\Security\Scripting",
                    "MacroSecurityLevel"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Common\Security\Scripting",
                    "MacroSecurityLevel",
                    3
                ),
            ],
        },
    ];
}
