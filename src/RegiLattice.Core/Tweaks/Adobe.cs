namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Adobe
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "adobe-enable-adobe-protected-mode",
            Label = "Enable Adobe Protected Mode",
            Category = "Adobe",
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
            Category = "Adobe",
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
            Category = "Adobe",
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
            Category = "Adobe",
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
            Category = "Adobe",
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
            Category = "Adobe",
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
            Category = "Adobe",
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
            Category = "Adobe",
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
            Id = "adobe-disable-updater-service",
            Label = "Disable Adobe Updater Service",
            Category = "Adobe",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Adobe Updater service via registry policy. Prevents background update checks and downloads.",
            Tags = ["adobe", "updater", "service", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\AdobeUpdate"],
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
            Id = "adobe-disable-cloud-sync",
            Label = "Disable Adobe Creative Cloud Sync",
            Category = "Adobe",
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
            Category = "Adobe",
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
            Category = "Adobe",
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
            Category = "Adobe",
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
            Category = "Adobe",
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
            Category = "Adobe",
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
            Category = "Adobe",
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
            Category = "Adobe",
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
            Category = "Adobe",
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
            Id = "adobe-disable-adobe-cloud",
            Label = "Disable Adobe Cloud Services",
            Category = "Adobe",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Adobe Creative Cloud online services and sync features. Prevents background cloud communication. Default: enabled.",
            Tags = ["adobe", "cloud", "sync", "disable"],
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
            Id = "adobe-disable-adobe-javascript",
            Label = "Disable Adobe JavaScript",
            Category = "Adobe",
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
            Category = "Adobe",
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
            Category = "Adobe",
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
            Category = "Adobe",
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
            Category = "Adobe",
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
            Category = "Adobe",
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
