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
            Description =
                "Disables Microsoft 365 Copilot globally via Office policy. Prevents the AI assistant from appearing in Word, Excel, PowerPoint, and Outlook. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["m365", "copilot", "office", "ai", "privacy"],
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\officecopilot",
                @"HKEY_CURRENT_USER\Software\Policies\Microsoft\office\16.0\common\officecopilot",
            ],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\copilotwebsearch", "DisableCopilotWebSearch", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\copilotwebsearch", "DisableCopilotWebSearch"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\copilotwebsearch", "DisableCopilotWebSearch", 1),
            ],
        },
        new TweakDef
        {
            Id = "m365-disable-copilot-web-search",
            Label = "Disable M365 Copilot Web Search",
            Category = "M365 Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents M365 Copilot from using Bing web search to enhance responses. Keeps Copilot responses limited to local/org data only. Default: Enabled. Recommended: Disabled for data privacy.",
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
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\copilotwebsearch", "DisableCopilotWebSearch", 1),
            ],
        },
        new TweakDef
        {
            Id = "m365-disable-copilot-plugins",
            Label = "Disable M365 Copilot Plugins",
            Category = "M365 Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks third-party Copilot plugins and extensions in M365 apps. Reduces attack surface and data exposure. Default: Enabled. Recommended: Disabled in enterprise.",
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
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\copilotplugins", "DisableCopilotPlugins", 1),
            ],
        },
        new TweakDef
        {
            Id = "m365-disable-copilot-outlook",
            Label = "Disable M365 Copilot in Outlook",
            Category = "M365 Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Copilot AI features in Microsoft Outlook including email summarization and draft suggestions. Default: Enabled. Recommended: Disabled for email privacy.",
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
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\outlook\options\mail", "DisableCopilotOutlook", 1),
            ],
        },
        new TweakDef
        {
            Id = "m365-disable-copilot-teams",
            Label = "Disable M365 Copilot in Teams",
            Category = "M365 Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Copilot AI features in Microsoft Teams including meeting summaries and chat suggestions. Default: Enabled. Recommended: Disabled for meeting privacy.",
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
            Description =
                "Disables Copilot AI features in Microsoft Word including text generation and rewriting. Default: Enabled. Recommended: Disabled for document privacy.",
            Tags = ["m365", "copilot", "word", "documents", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\word\options"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\word\options", "DisableCopilotWord", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\word\options", "DisableCopilotWord")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\word\options", "DisableCopilotWord", 1)],
        },
        new TweakDef
        {
            Id = "m365-disable-copilot-excel",
            Label = "Disable M365 Copilot in Excel",
            Category = "M365 Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Copilot AI features in Microsoft Excel including formula generation and data analysis. Default: Enabled. Recommended: Disabled for spreadsheet privacy.",
            Tags = ["m365", "copilot", "excel", "data", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\excel\options"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\excel\options", "DisableCopilotExcel", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\excel\options", "DisableCopilotExcel")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\excel\options", "DisableCopilotExcel", 1)],
        },
        new TweakDef
        {
            Id = "m365-disable-copilot-ppt",
            Label = "Disable M365 Copilot in PowerPoint",
            Category = "M365 Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Copilot AI features in PowerPoint including slide generation and presentation summaries. Default: Enabled. Recommended: Disabled for presentation privacy.",
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
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\powerpoint\options", "DisableCopilotPowerPoint", 1),
            ],
        },
        new TweakDef
        {
            Id = "m365-disable-connected-experiences",
            Label = "Disable M365 Connected Experiences",
            Category = "M365 Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Office connected experiences that send data to Microsoft cloud for analysis, including Copilot prerequisites. Default: Enabled. Recommended: Disabled for maximum privacy.",
            Tags = ["m365", "copilot", "connected", "cloud", "privacy", "telemetry"],
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\privacy",
                @"HKEY_CURRENT_USER\Software\Policies\Microsoft\office\16.0\common\privacy",
            ],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\privacy", "DisconnectedState", 2),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\privacy", "ControllerConnectedServicesEnabled", 2),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\office\16.0\common\privacy", "DisconnectedState", 2),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\office\16.0\common\privacy", "ControllerConnectedServicesEnabled", 2),
            ],
            RemoveOps = [RegOp.DeleteValue(@"key", "DisconnectedState"), RegOp.DeleteValue(@"key", "ControllerConnectedServicesEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\privacy", "DisconnectedState", 2)],
        },
        new TweakDef
        {
            Id = "m365-disable-loop",
            Label = "Disable Microsoft Loop",
            Category = "M365 Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Microsoft Loop, the collaborative workspace that integrates with M365 Copilot for real-time AI-assisted editing. Default: Enabled. Recommended: Disabled if not needed.",
            Tags = ["m365", "loop", "collaboration", "ai"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Loop"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Loop", "DisableLoop", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Loop", "DisableLoop")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Loop", "DisableLoop", 1)],
        },
        new TweakDef
        {
            Id = "m365-disable-coauth-telemetry",
            Label = "Disable M365 Co-Authoring Telemetry",
            Category = "M365 Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables telemetry from real-time co-authoring sessions. Reduces data sent to Microsoft during collaborative editing. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["m365", "coauth", "telemetry", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\coauth"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\coauth", "DisableCoAuthTelemetry", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\coauth", "DisableCoAuthTelemetry")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\coauth", "DisableCoAuthTelemetry", 1)],
        },
        new TweakDef
        {
            Id = "m365-disable-copilot-data-collection",
            Label = "Disable M365 Copilot Data Collection",
            Category = "M365 Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents M365 Copilot from collecting interaction data for model training and improvement. Default: Enabled. Recommended: Disabled for data sovereignty.",
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
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\officecopilot",
                    "DisableCopilotDataCollection",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "m365-disable-copilot-autosuggest",
            Label = "Disable M365 Copilot Auto-Suggestions",
            Category = "M365 Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables automatic Copilot suggestions that appear while typing in Office apps. Reduces AI interruptions. Default: Enabled. Recommended: Disabled for focus.",
            Tags = ["m365", "copilot", "autosuggest", "ux"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\officecopilot"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\officecopilot",
                    "DisableCopilotAutoSuggestions",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\officecopilot",
                    "DisableCopilotAutoSuggestions"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\officecopilot",
                    "DisableCopilotAutoSuggestions",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "m365-disable-copilot-web",
            Label = "Disable M365 Copilot Web Access (User)",
            Category = "M365 Copilot",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables M365 Copilot web/graph access at the user level via OfficeGraph policy. Prevents Copilot from reaching external data sources. Default: Enabled. Recommended: Disabled for privacy.",
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
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\copilotwebsearch", "DisableCopilotWebSearch", 1),
            ],
        },
        new TweakDef
        {
            Id = "m365-disable-loop-components",
            Label = "Disable Loop Components in M365 (User)",
            Category = "M365 Copilot",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Microsoft Loop embedded components in Office apps at the user level. Prevents Fluid/Loop collaborative blocks from rendering. Default: Enabled. Recommended: Disabled if Loop is not needed.",
            Tags = ["m365", "loop", "fluid", "components", "collaboration"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Common\Fluid"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Loop", "DisableLoop", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Loop", "DisableLoop")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Loop", "DisableLoop", 1)],
        },
        new TweakDef
        {
            Id = "m365-disable-word-copilot-compose",
            Label = "Disable Copilot Compose in Word",
            Category = "M365 Copilot",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables Copilot compose/draft features in Word via policy. Prevents AI-assisted text generation in documents. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["m365", "copilot", "word", "compose", "draft"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\word\options\vpref"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\word\options\vpref", "EnableCopilotCompose", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\word\options\vpref", "EnableCopilotCompose")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\word\options\vpref", "EnableCopilotCompose", 0),
            ],
        },
        new TweakDef
        {
            Id = "m365-disable-copilot-preview",
            Label = "Disable Copilot Preview Features",
            Category = "M365 Copilot",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables M365 Copilot preview/experimental features via policy. Prevents early-access AI features from appearing in Office apps. Default: Enabled. Recommended: Disabled for stability.",
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
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\copilotpreview", "EnablePreviewFeatures", 0),
            ],
        },
        new TweakDef
        {
            Id = "m365-disable-outlook-ai-suggestions",
            Label = "Disable AI-Powered Suggestions in Outlook",
            Category = "M365 Copilot",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables AI-powered compose suggestions in Outlook via policy. Prevents AI text predictions and smart reply features. Default: Enabled. Recommended: Disabled for privacy.",
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
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\outlook\options\mail\compose",
                    "EnableAISuggestions",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "m365-disable-content-analysis",
            Label = "Disable M365 Content Analysis",
            Category = "M365 Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables content analysis for connected experiences in Office apps. Default: enabled.",
            Tags = ["m365", "content", "analysis", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\privacy"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\privacy", "usercontentdisabled", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\privacy", "usercontentdisabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\privacy", "usercontentdisabled", 2)],
        },
        new TweakDef
        {
            Id = "m365-disable-optional-experiences",
            Label = "Disable M365 Optional Connected Experiences",
            Category = "M365 Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables optional connected experiences like LinkedIn resume assistant, 3D maps, etc. Default: enabled.",
            Tags = ["m365", "optional", "connected", "linkedin"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\privacy"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\privacy", "controllerconnectedservicesenabled", 2),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\privacy", "controllerconnectedservicesenabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\privacy",
                    "controllerconnectedservicesenabled",
                    2
                ),
            ],
        },
        new TweakDef
        {
            Id = "m365-disable-linkedin-integration",
            Label = "Disable LinkedIn Integration in Office",
            Category = "M365 Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables LinkedIn feature integration across Office apps. Default: enabled.",
            Tags = ["m365", "linkedin", "integration", "office"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common", "linkedin", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common", "linkedin")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common", "linkedin", 0)],
        },
        new TweakDef
        {
            Id = "m365-disable-download-content",
            Label = "Disable M365 Download Content Experiences",
            Category = "M365 Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables connected experiences that download online content (templates, images, etc.). Default: enabled.",
            Tags = ["m365", "download", "content", "online"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\privacy"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\privacy", "downloadcontentdisabled", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\privacy", "downloadcontentdisabled")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\privacy", "downloadcontentdisabled", 2),
            ],
        },
        new TweakDef
        {
            Id = "m365-disable-office-telemetry-dashboard",
            Label = "Disable Office Telemetry Dashboard",
            Category = "M365 Copilot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Office Telemetry Dashboard and agent. Prevents collection of Office usage data. Default: enabled.",
            Tags = ["m365", "office", "telemetry", "dashboard"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\osm"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\osm", "EnableLogging", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\osm", "EnableLogging")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\osm", "EnableLogging", 0)],
        },
        new TweakDef
        {
            Id = "m365-disable-office-autoupdate",
            Label = "Disable Office 365 Automatic Updates",
            Category = "M365 Copilot",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables automatic Office 365 Click-to-Run updates via Group Policy. Allows IT-controlled update schedules. Note: disabling updates is a security risk; use only in managed environments. Default: auto-update enabled.",
            Tags = ["m365", "office", "update", "gpo"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\officeupdate"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\officeupdate", "enableautomaticupdates", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\officeupdate", "enableautomaticupdates")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\officeupdate", "enableautomaticupdates", 0)],
        },
        new TweakDef
        {
            Id = "m365-disable-office-feedback",
            Label = "Disable Office Feedback (QME Telemetry)",
            Category = "M365 Copilot",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the Office Customer Experience Improvement Program (QME) feedback telemetry per user policy. Stops Office from collecting and sending usage data. Default: enabled.",
            Tags = ["m365", "office", "feedback", "telemetry"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\office\16.0\common"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\office\16.0\common", "qmenable", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\office\16.0\common", "qmenable")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\office\16.0\common", "qmenable", 0)],
        },
        new TweakDef
        {
            Id = "m365-disable-smart-lookup",
            Label = "Disable Smart Lookup / Intelligent Services in Office",
            Category = "M365 Copilot",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Smart Lookup (Researcher) which sends selected text to Bing for AI-powered context lookups within Word, Excel, and PowerPoint. Default: enabled when connected experiences are on.",
            Tags = ["m365", "office", "smart-lookup", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\office\16.0\common\general"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\office\16.0\common\general", "disablesmartlookup", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\office\16.0\common\general", "disablesmartlookup")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\office\16.0\common\general", "disablesmartlookup", 1)],
        },
        new TweakDef
        {
            Id = "m365-disable-live-preview",
            Label = "Disable Office Live Preview (Format Hover)",
            Category = "M365 Copilot",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Live Preview in Office applications which temporarily applies formatting when hovering over styles, themes, and fonts. Reduces rendering load. Default: enabled.",
            Tags = ["m365", "office", "live-preview", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\office\16.0\common\toolbars"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\office\16.0\common\toolbars", "enablelivepreview", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\office\16.0\common\toolbars", "enablelivepreview")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\office\16.0\common\toolbars", "enablelivepreview", 0)],
        },
        new TweakDef
        {
            Id = "m365-disable-mini-toolbar",
            Label = "Disable Office Mini Toolbar on Selection",
            Category = "M365 Copilot",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the floating mini formatting toolbar that appears when text is selected in Office applications. Reduces distracting pop-ups for keyboard-focused users. Default: enabled.",
            Tags = ["m365", "office", "mini-toolbar", "ui"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\office\16.0\common\toolbars"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\office\16.0\common\toolbars", "minitoolbardisabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\office\16.0\common\toolbars", "minitoolbardisabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\office\16.0\common\toolbars", "minitoolbardisabled", 1)],
        },
        new TweakDef
        {
            Id = "m365-disable-translator",
            Label = "Disable Office Built-In Translator",
            Category = "M365 Copilot",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the built-in AI translation feature in Office applications that sends text to Microsoft translation services. Prevents text from leaving the document via translation calls. Default: enabled.",
            Tags = ["m365", "office", "translator", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\office\16.0\common\general"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\office\16.0\common\general", "disabletranslator", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\office\16.0\common\general", "disabletranslator")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\office\16.0\common\general", "disabletranslator", 1)],
        },
        new TweakDef
        {
            Id = "m365-disable-word-smart-tags",
            Label = "Disable Word Smart Tags",
            Category = "M365 Copilot",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Smart Tags in Microsoft Word which automatically detect and label names, dates, addresses, and other recognised data. Reduces AI-powered automatic document analysis. Default: enabled.",
            Tags = ["m365", "word", "smart-tags", "ai"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\office\16.0\word\options"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\office\16.0\word\options", "modernsmarttagsdisabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\office\16.0\word\options", "modernsmarttagsdisabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\office\16.0\word\options", "modernsmarttagsdisabled", 1)],
        },
        new TweakDef
        {
            Id = "m365-disable-insights",
            Label = "Disable Office Insights (MyAnalytics)",
            Category = "M365 Copilot",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Office Insights (formerly MyAnalytics / Researcher) which uses AI to analyse work patterns and provide personalised productivity suggestions. Default: enabled for eligible M365 plans.",
            Tags = ["m365", "insights", "analytics", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\office\16.0\common\general"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\office\16.0\common\general", "disableinsights", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\office\16.0\common\general", "disableinsights")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\office\16.0\common\general", "disableinsights", 1)],
        },
        new TweakDef
        {
            Id = "m365-disable-researcher-tab",
            Label = "Disable Word Researcher Feature",
            Category = "M365 Copilot",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the Researcher tab in Microsoft Word which uses Bing AI to pull in research content while writing. Prevents content lookups from inside the document editor. Default: enabled.",
            Tags = ["m365", "word", "researcher", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\office\16.0\word\options"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\office\16.0\word\options", "disableresearcher", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\office\16.0\word\options", "disableresearcher")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\office\16.0\word\options", "disableresearcher", 1)],
        },
        new TweakDef
        {
            Id = "m365-disable-office-roaming",
            Label = "Disable Office Settings Roaming",
            Category = "M365 Copilot",
            NeedsAdmin = false,
            CorpSafe = false,
            Description =
                "Disables roaming of Office personalisation settings to the cloud. Prevents theme, template, and preference synchronisation via the Office roaming service. Default: enabled when signed in with M365 account.",
            Tags = ["m365", "office", "roaming", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\office\16.0\common\roaming"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\office\16.0\common\roaming", "enableroaming", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\office\16.0\common\roaming", "enableroaming")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\office\16.0\common\roaming", "enableroaming", 0)],
        },
    ];
}
