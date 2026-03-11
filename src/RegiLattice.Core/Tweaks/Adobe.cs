namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Adobe
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "adobe-disable-adobe-update",
            Label = "Disable Adobe Auto-Update",
            Category = "Adobe",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables automatic updates for Adobe Reader and Acrobat DC.",
            Tags = ["adobe", "update"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Adobe\Adobe ARM\1.0\ARM", @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown", @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Adobe Acrobat\DC\FeatureLockDown"],
        },
        new TweakDef
        {
            Id = "adobe-disable-adobe-telemetry",
            Label = "Disable Adobe Telemetry",
            Category = "Adobe",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Adobe usage data collection and suppresses upsell prompts.",
            Tags = ["adobe", "telemetry", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown"],
        },
        new TweakDef
        {
            Id = "adobe-disable-adobe-javascript",
            Label = "Disable Adobe PDF JavaScript",
            Category = "Adobe",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables JavaScript execution in PDF documents — major security hardening.",
            Tags = ["adobe", "security", "javascript"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown"],
        },
        new TweakDef
        {
            Id = "adobe-disable-adobe-welcome",
            Label = "Disable Adobe Welcome Screen",
            Category = "Adobe",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Suppresses the Adobe Reader welcome / start screen on launch.",
            Tags = ["adobe", "ux"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown\cWelcomeScreen"],
        },
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
            Id = "adobe-disable-adobe-cloud",
            Label = "Disable Adobe Cloud Services",
            Category = "Adobe",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Adobe Document Cloud file storage integration.",
            Tags = ["adobe", "cloud", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown\cCloud"],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\CCXWelcome", "SyncDisabled", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\CCXWelcome", "SyncDisabled"),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\AdobeGenuineSoftware", "AdobeGenuineEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\AdobeGenuineSoftware", "AdobeGenuineEnabled"),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\CCXProcess", "CrashReporting", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\CCXProcess", "CrashReporting"),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Adobe\CommonFiles\UsageCC", "ShowHomeScreen", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Adobe\CommonFiles\UsageCC", "ShowHomeScreen", 1),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\CCXDesktop", "DisableFontSync", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\CCXDesktop", "DisableFontSync"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\CCXDesktop", "DisableFontSync", 1)],
        },
        new TweakDef
        {
            Id = "adobe-disable-updater",
            Label = "Disable Adobe Updater",
            Category = "Adobe",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Adobe Acrobat Reader automatic updater. Updates must be applied manually. Default: Enabled. Recommended: Disabled for managed environments.",
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
            Description = "Forces Adobe Reader to reuse existing instances instead of spawning new processes. Reduces memory footprint. Default: New instance. Recommended: Reuse.",
            Tags = ["adobe", "memory", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Adobe\Acrobat Reader\DC\AVGeneral"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Adobe\Acrobat Reader\DC\AVGeneral", "bReuseAcrobatInstance", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Adobe\Acrobat Reader\DC\AVGeneral", "bReuseAcrobatInstance"),
            ],
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
            Id = "adobe-disable-analytics",
            Label = "Disable Adobe Analytics",
            Category = "Adobe",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Adobe analytics and telemetry data collection by opting out of usage tracking.",
            Tags = ["adobe", "analytics", "telemetry", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Adobe\CommonFiles\Usage"],
        },
        new TweakDef
        {
            Id = "adobe-disable-cloud-sync",
            Label = "Disable Adobe Creative Cloud Sync",
            Category = "Adobe",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Adobe Creative Cloud file and settings sync. Reduces background network activity and cloud dependency. Default: Enabled. Recommended: Disabled on managed machines.",
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
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown\cCloud", "bDisableADCFileStore", 1)],
        },
        new TweakDef
        {
            Id = "adobe-disable-cef-subprocess",
            Label = "Disable Adobe CEF Helper",
            Category = "Adobe",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Adobe CEF (Chromium Embedded Framework) helper processes via policy. Reduces memory and CPU usage. Default: Enabled. Recommended: Disabled.",
            Tags = ["adobe", "cef", "helper", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\AdobeApp"],
        },
        new TweakDef
        {
            Id = "adobe-disable-acrobat-cloud",
            Label = "Disable Adobe Acrobat Cloud Services",
            Category = "Adobe",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Adobe Document Cloud file store integration in Acrobat Reader. Prevents cloud save prompts. Default: enabled. Recommended: disabled.",
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
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown\cCloud", "bDisableADCFileStore", 1)],
        },
        new TweakDef
        {
            Id = "adobe-pdf-single-page-view",
            Label = "Set PDF Default View to Single Page",
            Category = "Adobe",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets the default PDF page layout to single page view. Overrides continuous scroll as the default. Default: continuous. Recommended: single page.",
            Tags = ["adobe", "pdf", "view", "layout", "single-page"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Adobe\Acrobat Reader\DC\AVGeneral"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Adobe\Acrobat Reader\DC\AVGeneral", "iPageViewLayoutMode", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Adobe\Acrobat Reader\DC\AVGeneral", "iPageViewLayoutMode"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Adobe\Acrobat Reader\DC\AVGeneral", "iPageViewLayoutMode", 0)],
        },
        new TweakDef
        {
            Id = "adobe-disable-welcome-screen",
            Label = "Disable Adobe What's New Screen",
            Category = "Adobe",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the What's New promotional screen shown after Adobe Reader updates. Different from the start screen. Default: shown. Recommended: hidden.",
            Tags = ["adobe", "welcome", "whats-new", "ux"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown\cWelcomeScreen"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown\cWelcomeScreen", "bShowWelcomeScreen", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown\cWelcomeScreen", "bShowWelcomeScreen"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown\cWelcomeScreen", "bShowWelcomeScreen", 0)],
        },
    ];
}
