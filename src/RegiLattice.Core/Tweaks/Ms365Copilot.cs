namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Ms365Copilot
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "m365-disable-copilot",
            Label = "Disable M365 Copilot (Master Switch)",
            Category = "M365 Copilot",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Microsoft 365 Copilot globally via Office policy. Prevents the AI assistant from appearing in Word, Excel, PowerPoint, and Outlook. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["m365", "copilot", "office", "ai", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\officecopilot", @"HKEY_CURRENT_USER\Software\Policies\Microsoft\office\16.0\common\officecopilot"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\copilotwebsearch", "DisableCopilotWebSearch", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\copilotwebsearch", "DisableCopilotWebSearch"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\copilotwebsearch", "DisableCopilotWebSearch", 1)],
        },
        new TweakDef
        {
            Id = "m365-disable-copilot-web-search",
            Label = "Disable M365 Copilot Web Search",
            Category = "M365 Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents M365 Copilot from using Bing web search to enhance responses. Keeps Copilot responses limited to local/org data only. Default: Enabled. Recommended: Disabled for data privacy.",
            Tags = ["m365", "copilot", "web", "search", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\copilotwebsearch"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\copilotwebsearch", "DisableCopilotWebSearch", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\copilotwebsearch", "DisableCopilotWebSearch"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\copilotwebsearch", "DisableCopilotWebSearch", 1)],
        },
        new TweakDef
        {
            Id = "m365-disable-copilot-plugins",
            Label = "Disable M365 Copilot Plugins",
            Category = "M365 Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Blocks third-party Copilot plugins and extensions in M365 apps. Reduces attack surface and data exposure. Default: Enabled. Recommended: Disabled in enterprise.",
            Tags = ["m365", "copilot", "plugins", "extensions", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\copilotplugins"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\copilotplugins", "DisableCopilotPlugins", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\copilotplugins", "DisableCopilotPlugins"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\copilotplugins", "DisableCopilotPlugins", 1)],
        },
        new TweakDef
        {
            Id = "m365-disable-copilot-outlook",
            Label = "Disable M365 Copilot in Outlook",
            Category = "M365 Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Copilot AI features in Microsoft Outlook including email summarization and draft suggestions. Default: Enabled. Recommended: Disabled for email privacy.",
            Tags = ["m365", "copilot", "outlook", "email", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\outlook\options\mail"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\outlook\options\mail", "DisableCopilotOutlook", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\outlook\options\mail", "DisableCopilotOutlook"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\outlook\options\mail", "DisableCopilotOutlook", 1)],
        },
        new TweakDef
        {
            Id = "m365-disable-copilot-teams",
            Label = "Disable M365 Copilot in Teams",
            Category = "M365 Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Copilot AI features in Microsoft Teams including meeting summaries and chat suggestions. Default: Enabled. Recommended: Disabled for meeting privacy.",
            Tags = ["m365", "copilot", "teams", "meetings", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Teams", @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Teams"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Teams", "DisableCopilot", 1),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Teams", "DisableCopilot", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Teams", "DisableCopilot"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Teams", "DisableCopilot"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Teams", "DisableCopilot", 1)],
        },
        new TweakDef
        {
            Id = "m365-disable-copilot-word",
            Label = "Disable M365 Copilot in Word",
            Category = "M365 Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Copilot AI features in Microsoft Word including text generation and rewriting. Default: Enabled. Recommended: Disabled for document privacy.",
            Tags = ["m365", "copilot", "word", "documents", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\word\options"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\word\options", "DisableCopilotWord", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\word\options", "DisableCopilotWord"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\word\options", "DisableCopilotWord", 1)],
        },
        new TweakDef
        {
            Id = "m365-disable-copilot-excel",
            Label = "Disable M365 Copilot in Excel",
            Category = "M365 Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Copilot AI features in Microsoft Excel including formula generation and data analysis. Default: Enabled. Recommended: Disabled for spreadsheet privacy.",
            Tags = ["m365", "copilot", "excel", "data", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\excel\options"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\excel\options", "DisableCopilotExcel", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\excel\options", "DisableCopilotExcel"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\excel\options", "DisableCopilotExcel", 1)],
        },
        new TweakDef
        {
            Id = "m365-disable-copilot-ppt",
            Label = "Disable M365 Copilot in PowerPoint",
            Category = "M365 Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Copilot AI features in PowerPoint including slide generation and presentation summaries. Default: Enabled. Recommended: Disabled for presentation privacy.",
            Tags = ["m365", "copilot", "powerpoint", "presentations", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\powerpoint\options"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\powerpoint\options", "DisableCopilotPowerPoint", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\powerpoint\options", "DisableCopilotPowerPoint"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\powerpoint\options", "DisableCopilotPowerPoint", 1)],
        },
        new TweakDef
        {
            Id = "m365-disable-connected-experiences",
            Label = "Disable M365 Connected Experiences",
            Category = "M365 Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Office connected experiences that send data to Microsoft cloud for analysis, including Copilot prerequisites. Default: Enabled. Recommended: Disabled for maximum privacy.",
            Tags = ["m365", "copilot", "connected", "cloud", "privacy", "telemetry"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\privacy", @"HKEY_CURRENT_USER\Software\Policies\Microsoft\office\16.0\common\privacy"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\privacy", "DisconnectedState", 2),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\privacy", "ControllerConnectedServicesEnabled", 2),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\office\16.0\common\privacy", "DisconnectedState", 2),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\office\16.0\common\privacy", "ControllerConnectedServicesEnabled", 2),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"key", "DisconnectedState"),
                RegOp.DeleteValue(@"key", "ControllerConnectedServicesEnabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\privacy", "DisconnectedState", 2)],
        },
        new TweakDef
        {
            Id = "m365-disable-loop",
            Label = "Disable Microsoft Loop",
            Category = "M365 Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Microsoft Loop, the collaborative workspace that integrates with M365 Copilot for real-time AI-assisted editing. Default: Enabled. Recommended: Disabled if not needed.",
            Tags = ["m365", "loop", "collaboration", "ai"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Loop"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Loop", "DisableLoop", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Loop", "DisableLoop"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Loop", "DisableLoop", 1)],
        },
        new TweakDef
        {
            Id = "m365-disable-coauth-telemetry",
            Label = "Disable M365 Co-Authoring Telemetry",
            Category = "M365 Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables telemetry from real-time co-authoring sessions. Reduces data sent to Microsoft during collaborative editing. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["m365", "coauth", "telemetry", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\coauth"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\coauth", "DisableCoAuthTelemetry", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\coauth", "DisableCoAuthTelemetry"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\coauth", "DisableCoAuthTelemetry", 1)],
        },
        new TweakDef
        {
            Id = "m365-disable-copilot-data-collection",
            Label = "Disable M365 Copilot Data Collection",
            Category = "M365 Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents M365 Copilot from collecting interaction data for model training and improvement. Default: Enabled. Recommended: Disabled for data sovereignty.",
            Tags = ["m365", "copilot", "data", "collection", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\officecopilot"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\officecopilot", "DisableCopilotDataCollection", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\officecopilot", "DisableCopilotDataCollection"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\officecopilot", "DisableCopilotDataCollection", 1)],
        },
        new TweakDef
        {
            Id = "m365-disable-copilot-autosuggest",
            Label = "Disable M365 Copilot Auto-Suggestions",
            Category = "M365 Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables automatic Copilot suggestions that appear while typing in Office apps. Reduces AI interruptions. Default: Enabled. Recommended: Disabled for focus.",
            Tags = ["m365", "copilot", "autosuggest", "ux"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\officecopilot"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\officecopilot", "DisableCopilotAutoSuggestions", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\officecopilot", "DisableCopilotAutoSuggestions"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\officecopilot", "DisableCopilotAutoSuggestions", 1)],
        },
        new TweakDef
        {
            Id = "m365-disable-copilot-web",
            Label = "Disable M365 Copilot Web Access (User)",
            Category = "M365 Copilot",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables M365 Copilot web/graph access at the user level via OfficeGraph policy. Prevents Copilot from reaching external data sources. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["m365", "copilot", "web", "privacy", "officegraph"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Common\OfficeGraph"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\copilotwebsearch", "DisableCopilotWebSearch", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\copilotwebsearch", "DisableCopilotWebSearch"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\copilotwebsearch", "DisableCopilotWebSearch", 1)],
        },
        new TweakDef
        {
            Id = "m365-disable-loop-components",
            Label = "Disable Loop Components in M365 (User)",
            Category = "M365 Copilot",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Microsoft Loop embedded components in Office apps at the user level. Prevents Fluid/Loop collaborative blocks from rendering. Default: Enabled. Recommended: Disabled if Loop is not needed.",
            Tags = ["m365", "loop", "fluid", "components", "collaboration"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Common\Fluid"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Loop", "DisableLoop", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Loop", "DisableLoop"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Loop", "DisableLoop", 1)],
        },
        new TweakDef
        {
            Id = "m365-disable-word-copilot-compose",
            Label = "Disable Copilot Compose in Word",
            Category = "M365 Copilot",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Copilot compose/draft features in Word via policy. Prevents AI-assisted text generation in documents. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["m365", "copilot", "word", "compose", "draft"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\word\options\vpref"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\word\options\vpref", "EnableCopilotCompose", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\word\options\vpref", "EnableCopilotCompose"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\word\options\vpref", "EnableCopilotCompose", 0)],
        },
        new TweakDef
        {
            Id = "m365-disable-copilot-preview",
            Label = "Disable Copilot Preview Features",
            Category = "M365 Copilot",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables M365 Copilot preview/experimental features via policy. Prevents early-access AI features from appearing in Office apps. Default: Enabled. Recommended: Disabled for stability.",
            Tags = ["m365", "copilot", "preview", "experimental", "features"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\copilotpreview"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\copilotpreview", "EnablePreviewFeatures", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\copilotpreview", "EnablePreviewFeatures"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\copilotpreview", "EnablePreviewFeatures", 0)],
        },
        new TweakDef
        {
            Id = "m365-disable-outlook-ai-suggestions",
            Label = "Disable AI-Powered Suggestions in Outlook",
            Category = "M365 Copilot",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables AI-powered compose suggestions in Outlook via policy. Prevents AI text predictions and smart reply features. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["m365", "copilot", "outlook", "ai", "suggestions"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\outlook\options\mail\compose"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\outlook\options\mail\compose", "EnableAISuggestions", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\outlook\options\mail\compose", "EnableAISuggestions"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\outlook\options\mail\compose", "EnableAISuggestions", 0)],
        },
    ];
}
