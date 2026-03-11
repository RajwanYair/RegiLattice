namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class LibreOffice
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "lo-libreoffice-default-ooxml",
            Label = "Default Save as OOXML (docx/xlsx)",
            Category = "LibreOffice",
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
            Category = "LibreOffice",
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
            Category = "LibreOffice",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the LibreOffice online update check.",
            Tags = ["libreoffice", "update", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\LibreOffice\UNO\Misc"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\LibreOffice\UNO\Misc", "AutoCheckEnabled", "false"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\LibreOffice\UNO\Misc", "AutoCheckEnabled", "true"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Software\LibreOffice\UNO\Misc", "AutoCheckEnabled", "false")],
        },
        new TweakDef
        {
            Id = "lo-libreoffice-disable-startcenter-news",
            Label = "Disable Start Center Recent News",
            Category = "LibreOffice",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the recent news feed on the LibreOffice Start Center.",
            Tags = ["libreoffice", "startcenter", "ux", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\LibreOffice\UNO\Misc"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\LibreOffice\StartCenter", "ShowStartCenter", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\LibreOffice\StartCenter", "ShowStartCenter"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\LibreOffice\StartCenter", "ShowStartCenter", 0)],
        },
        new TweakDef
        {
            Id = "lo-libreoffice-disable-macros",
            Label = "Disable LibreOffice Macro Execution",
            Category = "LibreOffice",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets LibreOffice macro security level to Very High (3), effectively disabling macro execution.",
            Tags = ["libreoffice", "macros", "security"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\LibreOffice\UNO\Misc"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\LibreOffice\UNO\Misc", "MacroSecurityLevel", "3"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\LibreOffice\UNO\Misc", "MacroSecurityLevel", "1"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Software\LibreOffice\UNO\Misc", "MacroSecurityLevel", "3")],
        },
        new TweakDef
        {
            Id = "lo-libreoffice-disable-send-feedback",
            Label = "Disable LibreOffice Send Feedback",
            Category = "LibreOffice",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the LibreOffice send feedback feature.",
            Tags = ["libreoffice", "telemetry", "privacy", "feedback"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\LibreOffice\UNO\Misc"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\LibreOffice\UNO\Misc", "SendFeedback", "false"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\LibreOffice\UNO\Misc", "SendFeedback", "true"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Software\LibreOffice\UNO\Misc", "SendFeedback", "false")],
        },
        new TweakDef
        {
            Id = "lo-libre-disable-java",
            Label = "Disable LibreOffice Java Runtime",
            Category = "LibreOffice",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Java runtime in LibreOffice. Reduces memory usage and startup time. Some wizards/macros may require Java. Default: Enabled. Recommended: Disabled.",
            Tags = ["libreoffice", "java", "performance", "memory"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\LibreOffice\UNO\Misc"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\LibreOffice\UNO\Misc", "JavaEnabled", "false"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\LibreOffice\UNO\Misc", "JavaEnabled", "true"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Software\LibreOffice\UNO\Misc", "JavaEnabled", "false")],
        },
        new TweakDef
        {
            Id = "lo-libre-autosave-interval",
            Label = "Reduce LibreOffice Auto-Save Interval",
            Category = "LibreOffice",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets LibreOffice auto-save interval to 3 minutes for better crash recovery. Default: 10 minutes. Recommended: 3 minutes.",
            Tags = ["libreoffice", "autosave", "recovery"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\LibreOffice\UNO\Misc"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\LibreOffice\UNO\Misc", "AutoSaveTimeIntervall", "3"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\LibreOffice\UNO\Misc", "AutoSaveTimeIntervall", "10"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Software\LibreOffice\UNO\Misc", "AutoSaveTimeIntervall", "3")],
        },
        new TweakDef
        {
            Id = "lo-libreoffice-disable-recovery",
            Label = "Disable LibreOffice Document Recovery",
            Category = "LibreOffice",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables LibreOffice automatic document recovery via Group Policy. Prevents crash recovery prompts on startup. Default: Enabled. Recommended: Disabled for managed environments.",
            Tags = ["libreoffice", "recovery", "autosave", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Recovery\Recovery"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\LibreOffice\Recovery", "AutoSaveEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\LibreOffice\Recovery", "AutoSaveEnabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\LibreOffice\Recovery", "AutoSaveEnabled", 0)],
        },
        new TweakDef
        {
            Id = "lo-disable-crash-recovery",
            Label = "Disable LibreOffice Crash Recovery Prompt",
            Category = "LibreOffice",
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
            Category = "LibreOffice",
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
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Jobs\AutoCheckUpdate", "AutoCheckEnabled", 0)],
        },
        new TweakDef
        {
            Id = "lo-disable-crash-reporter",
            Label = "Disable LibreOffice Crash Reporter",
            Category = "LibreOffice",
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
            Category = "LibreOffice",
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
            Category = "LibreOffice",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets LibreOffice macro security to Very High, blocking all macros. Prevents macro-based malware. Default: Medium.",
            Tags = ["libreoffice", "macro", "security", "block"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Common\Security\Scripting"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Common\Security\Scripting", "MacroSecurityLevel", 3)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Common\Security\Scripting", "MacroSecurityLevel")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Common\Security\Scripting", "MacroSecurityLevel", 3)],
        },
        new TweakDef
        {
            Id = "lo-disable-online-update-check",
            Label = "Disable LibreOffice Online Update Check",
            Category = "LibreOffice",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the online update check (separate from auto-update). No network calls to check for new versions. Default: enabled.",
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
            Category = "LibreOffice",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Tip of the Day dialog shown on LibreOffice launch. Default: enabled.",
            Tags = ["libreoffice", "tip", "dialog", "ui"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Common\Misc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Common\Misc", "ShowTipOfTheDay", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Common\Misc", "ShowTipOfTheDay")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Common\Misc", "ShowTipOfTheDay", 0)],
        },
        new TweakDef
        {
            Id = "lo-disable-libreoffice-autoupdate",
            Label = "Disable LibreOffice Auto-Update",
            Category = "LibreOffice",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables automatic update checks for LibreOffice. Prevents background network requests for version checks. Default: enabled.",
            Tags = ["libreoffice", "update", "auto-update", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Jobs\Jobs\UpdateCheck\Arguments"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Jobs\Jobs\UpdateCheck\Arguments", "AutoCheckEnabled", "false")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Jobs\Jobs\UpdateCheck\Arguments", "AutoCheckEnabled")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Jobs\Jobs\UpdateCheck\Arguments", "AutoCheckEnabled", "false")],
        },
        new TweakDef
        {
            Id = "lo-disable-libreoffice-crash-reporter",
            Label = "Disable LibreOffice Crash Reporter",
            Category = "LibreOffice",
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
            Category = "LibreOffice",
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
            Category = "LibreOffice",
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
            Category = "LibreOffice",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables hardware (GPU) acceleration for LibreOffice rendering. Improves drawing and display performance. Default: disabled on some systems.",
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
            Category = "LibreOffice",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Configures LibreOffice as the default handler for Office document types (.docx, .xlsx, .pptx). Default: system-determined.",
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
            Category = "LibreOffice",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables macro execution in LibreOffice documents. Prevents potentially malicious macros from running. Default: prompt.",
            Tags = ["libreoffice", "macro", "security", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Common\Security\Scripting"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Common\Security\Scripting", "MacroSecurityLevel", 3)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Common\Security\Scripting", "MacroSecurityLevel")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Common\Security\Scripting", "MacroSecurityLevel", 3)],
        },
    ];
}
