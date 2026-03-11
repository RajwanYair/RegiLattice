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
            Description = "Disables Microsoft Office telemetry, feedback, and connected services data collection across all installed versions (2010-365).",
            Tags = ["office", "telemetry", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Office\Common\ClientTelemetry"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\Common\ClientTelemetry", "DisableTelemetry", 1),
                RegOp.SetDword(@"feedback", "Enabled", 0),
                RegOp.SetDword(@"privacy", "DisconnectedState", 2),
                RegOp.SetDword(@"privacy", "UserContentDisabled", 2),
                RegOp.SetDword(@"privacy", "DownloadContentDisabled", 2),
                RegOp.SetDword(@"privacy", "ControllerConnectedServicesEnabled", 2),
                RegOp.SetDword(@"policy", "DisableTelemetry", 1),
                RegOp.SetDword(@"policy", "SendTelemetry", 3),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\Common\ClientTelemetry", "DisableTelemetry"),
                RegOp.DeleteValue(@"feedback", "Enabled"),
                RegOp.DeleteValue(@"policy", "DisableTelemetry"),
                RegOp.DeleteValue(@"policy", "SendTelemetry"),
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
                RegOp.SetDword(@"general", "DisableBootToOfficeStart", 1),
                RegOp.SetDword(@"general", "ShownFirstRunOptin", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"general", "DisableBootToOfficeStart"),
                RegOp.DeleteValue(@"general", "ShownFirstRunOptin"),
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
                RegOp.SetDword(@"privacy", "ControllerConnectedServicesEnabled", 2),
                RegOp.SetDword(@"privacy", "DisconnectedState", 2),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"privacy", "ControllerConnectedServicesEnabled"),
                RegOp.DeleteValue(@"privacy", "DisconnectedState"),
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
            ApplyOps =
            [
                RegOp.SetDword(@"graphics", "DisableHardwareAcceleration", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"graphics", "DisableHardwareAcceleration"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Graphics", "DisableHardwareAcceleration", 1)],
        },
        new TweakDef
        {
            Id = "office-macro-trust",
            Label = "Trust VBA Macros (All Apps)",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = false,
            Description = "Lowers macro security to 'Enable all' and trusts the VBA project object model (Word, Excel, PowerPoint, Access, Outlook, Publisher).",
            Tags = ["office", "macros", "security"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Word\Security"],
            ApplyOps =
            [
                RegOp.SetDword(@"sec", "VBAWarnings", 1),
                RegOp.SetDword(@"sec", "AccessVBOM", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"sec", "VBAWarnings"),
                RegOp.DeleteValue(@"sec", "AccessVBOM"),
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
            ApplyOps =
            [
                RegOp.SetDword(@"opts", "SaveInterval", 2),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"opts", "SaveInterval", 10),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(@"graphics", "DisableAnimations", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"graphics", "DisableAnimations"),
            ],
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
                RegOp.SetDword(@"feedback", "Enabled", 0),
                RegOp.SetDword(@"feedback", "IncludeEmail", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"feedback", "Enabled"),
                RegOp.DeleteValue(@"feedback", "IncludeEmail"),
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
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\officeupdate", "EnableAutomaticUpdates", 0)],
        },
        new TweakDef
        {
            Id = "office-disable-telemetry-dash",
            Label = "Disable Office Telemetry Dashboard",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Office telemetry dashboard and diagnostic data collection. Reduces network traffic and CPU from Office telemetry agent. Default: Enabled. Recommended: Disabled.",
            Tags = ["office", "telemetry", "privacy", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\OSM"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\Common\ClientTelemetry", "DisableTelemetry", 1),
                RegOp.SetDword(@"feedback", "Enabled", 0),
                RegOp.SetDword(@"privacy", "DisconnectedState", 2),
                RegOp.SetDword(@"privacy", "UserContentDisabled", 2),
                RegOp.SetDword(@"privacy", "DownloadContentDisabled", 2),
                RegOp.SetDword(@"privacy", "ControllerConnectedServicesEnabled", 2),
                RegOp.SetDword(@"policy", "DisableTelemetry", 1),
                RegOp.SetDword(@"policy", "SendTelemetry", 3),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\Common\ClientTelemetry", "DisableTelemetry"),
                RegOp.DeleteValue(@"feedback", "Enabled"),
                RegOp.DeleteValue(@"policy", "DisableTelemetry"),
                RegOp.DeleteValue(@"policy", "SendTelemetry"),
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
            Description = "Disables automatic background Office Click-to-Run updates. Updates must be applied manually through Office or WSUS. Default: Enabled. Recommended: Disabled for managed environments.",
            Tags = ["office", "updates", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Office\ClickToRun\Configuration"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Office\ClickToRun\Configuration", "UpdatesEnabled", "False"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Office\ClickToRun\Configuration", "UpdatesEnabled", "True"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Office\ClickToRun\Configuration", "UpdatesEnabled", "False")],
        },
        new TweakDef
        {
            Id = "office-disable-connected-experiences",
            Label = "Disable Office Connected Experiences",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables optional connected experiences in Office 365. Prevents cloud-powered features like LinkedIn integration and 3D maps. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["office", "connected", "privacy", "cloud"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Common\Privacy"],
            ApplyOps =
            [
                RegOp.SetDword(@"privacy", "ControllerConnectedServicesEnabled", 2),
                RegOp.SetDword(@"privacy", "DisconnectedState", 2),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"privacy", "ControllerConnectedServicesEnabled"),
                RegOp.DeleteValue(@"privacy", "DisconnectedState"),
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
            Description = "Disables Office feedback survey prompts and the feedback button. Prevents interruptions during work. Default: Enabled. Recommended: Disabled.",
            Tags = ["office", "feedback", "survey", "notifications"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Common\Feedback"],
            ApplyOps =
            [
                RegOp.SetDword(@"feedback", "Enabled", 0),
                RegOp.SetDword(@"feedback", "IncludeEmail", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"feedback", "Enabled"),
                RegOp.DeleteValue(@"feedback", "IncludeEmail"),
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
            Description = "Disables Office telemetry dashboard logging and data upload. Prevents usage data collection via OSM. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["office", "telemetry", "dashboard", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\OSM"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\Common\ClientTelemetry", "DisableTelemetry", 1),
                RegOp.SetDword(@"feedback", "Enabled", 0),
                RegOp.SetDword(@"privacy", "DisconnectedState", 2),
                RegOp.SetDword(@"privacy", "UserContentDisabled", 2),
                RegOp.SetDword(@"privacy", "DownloadContentDisabled", 2),
                RegOp.SetDword(@"privacy", "ControllerConnectedServicesEnabled", 2),
                RegOp.SetDword(@"policy", "DisableTelemetry", 1),
                RegOp.SetDword(@"policy", "SendTelemetry", 3),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\Common\ClientTelemetry", "DisableTelemetry"),
                RegOp.DeleteValue(@"feedback", "Enabled"),
                RegOp.DeleteValue(@"policy", "DisableTelemetry"),
                RegOp.DeleteValue(@"policy", "SendTelemetry"),
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
            Description = "Disables Office feedback collection via machine-level policy. Blocks feedback button and email inclusion in reports. Default: Enabled. Recommended: Disabled.",
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
            Description = "Sets the Office 365 visual theme to Dark Gray (theme 4). Reduces eye strain in low-light environments. Default: Colorful (0). Recommended: Dark for dark-mode users.",
            Tags = ["office", "theme", "dark", "appearance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common", "UI Theme", 4),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common", "UI Theme"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common", "UI Theme", 4)],
        },        new TweakDef
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
        },    new TweakDef
        {
            Id = "office-disable-office-cloud-save",
            Label = "Disable Office Cloud Save Prompt",
            Category = "Office",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the Office default-to-cloud save prompt. Files save locally by default instead of OneDrive. Default: cloud save prompt.",
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
            Description = "Disables LinkedIn integration in Microsoft Office applications. Removes LinkedIn profile cards and Resume Assistant. Default: enabled.",
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
            Description = "Disables the recent documents list in Office applications. Enhances privacy by not tracking opened files. Default: enabled.",
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
            Description = "Disables Protected View for files from the internet in Word. Documents open directly without sandbox. Security risk — use cautiously. Default: protected.",
            Tags = ["office", "protected-view", "security", "relax"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Word\Security\ProtectedView"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Word\Security\ProtectedView", "DisableInternetFilesInPV", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Word\Security\ProtectedView", "DisableInternetFilesInPV")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Word\Security\ProtectedView", "DisableInternetFilesInPV", 1)],
        },
    ];
}
