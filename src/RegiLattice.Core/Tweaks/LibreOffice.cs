namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class LibreOffice
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "lo-disable-libreoffice-autoupdate",
            Label = "Disable LibreOffice Auto-Update",
            Category = "LibreOffice",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the LibreOffice / OpenOffice maintenance service auto-update mechanism.",
            Tags = ["libreoffice", "openoffice", "update"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\LibreOffice\MaintenanceService", @"HKEY_LOCAL_MACHINE\SOFTWARE\OpenOffice.org\MaintenanceService"],
        },
        new TweakDef
        {
            Id = "lo-disable-libreoffice-crash-reporter",
            Label = "Disable LibreOffice Crash Reporter",
            Category = "LibreOffice",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the LibreOffice crash reporter and auto-submit.",
            Tags = ["libreoffice", "telemetry", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\LibreOffice\CrashReport"],
        },
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
            Id = "lo-libreoffice-default-handler",
            Label = "Set LibreOffice as Default Handler",
            Category = "LibreOffice",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Registers LibreOffice as the default handler for common document formats (.doc, .docx, .xls, .xlsx, .ppt, .odt, etc.).",
            Tags = ["libreoffice", "file-association", "default"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\FileExts"],
        },
        new TweakDef
        {
            Id = "lo-disable-libreoffice-recovery",
            Label = "Disable LibreOffice Recovery",
            Category = "LibreOffice",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables LibreOffice crash recovery and auto-save dialogs.",
            Tags = ["libreoffice", "recovery", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\LibreOffice\Recovery"],
        },
        new TweakDef
        {
            Id = "lo-disable-libreoffice-startcenter",
            Label = "Disable LibreOffice Start Center",
            Category = "LibreOffice",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Opens LibreOffice directly to a new document instead of the Start Center.",
            Tags = ["libreoffice", "startcenter", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\LibreOffice\StartCenter"],
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
            Id = "lo-libreoffice-disable-macro-exec",
            Label = "Disable LibreOffice Macro Execution",
            Category = "LibreOffice",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Sets LibreOffice macro security level to Very High (3) via policy. Only trusted signed macros will execute. Default: 1 (Medium). Recommended: 3 (Very High) for security.",
            Tags = ["libreoffice", "macros", "security", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Common\Security\Scripting"],
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
            Id = "lo-enable-hw-acceleration",
            Label = "Enable LibreOffice Hardware Acceleration",
            Category = "LibreOffice",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Enables OpenGL hardware acceleration in LibreOffice via policy. Default: Disabled. Recommended: Enabled.",
            Tags = ["libreoffice", "hardware", "acceleration", "opengl", "gpu"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\LibreOffice\org.openoffice.Office.Common\VCL"],
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
    ];
}
