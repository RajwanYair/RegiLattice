#nullable enable
using RegiLattice.Core.Models;
using RegiLattice.Core.Registry;

namespace RegiLattice.Core.Tweaks;

[TweakModule]
internal static class WinDbgSettings
{
    private const string AeDebugKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\AeDebug";
    private const string AeDebug32Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\Windows NT\CurrentVersion\AeDebug";
    private const string CrashCtlKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl";
    private const string WerKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting";
    private const string WerUserKey = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\Windows Error Reporting";
    private const string DbgKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "windbg-enable-kernel-crash-dump",
            Label = "Debugger: Write Complete Kernel Memory Dump on Crash",
            Category = "Dev Drive / Developer",
            Tags = ["debugger", "crash-dump", "kernel", "developer", "bsod"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            SideEffects = "Full kernel dump requires disk space equal to installed RAM.",
            ImpactNote = "Complete kernel dump enables full post-mortem crash analysis in WinDbg.",
            Description =
                "Sets CrashDumpEnabled=1 (Complete memory dump) in CrashControl. Configures "
                + "the system to write a complete memory dump on kernel crash (BSOD). "
                + "Values: 0=disabled, 1=complete, 2=kernel, 3=small (minidump), 7=automatic. "
                + "Default: 7 (automatic). Value 1 captures all physical memory for deep analysis.",
            RegistryKeys = [CrashCtlKey],
            ApplyOps = [RegOp.SetDword(CrashCtlKey, "CrashDumpEnabled", 1)],
            RemoveOps = [RegOp.SetDword(CrashCtlKey, "CrashDumpEnabled", 7)],
            DetectOps = [RegOp.CheckDword(CrashCtlKey, "CrashDumpEnabled", 1)],
        },
        new TweakDef
        {
            Id = "windbg-enable-kernel-dump",
            Label = "Debugger: Write Kernel Memory Dump (Filtered) on Crash",
            Category = "Dev Drive / Developer",
            Tags = ["debugger", "crash-dump", "kernel", "developer"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Kernel dump captures most crash data while using less disk space than full dump.",
            Description =
                "Sets CrashDumpEnabled=2 (Kernel memory dump) in CrashControl. Writes a "
                + "kernel-only memory dump that omits user-mode pages. Typically one-third "
                + "the size of a complete dump but contains all kernel structures needed "
                + "to diagnose most BSOD crashes.",
            RegistryKeys = [CrashCtlKey],
            ApplyOps = [RegOp.SetDword(CrashCtlKey, "CrashDumpEnabled", 2)],
            RemoveOps = [RegOp.SetDword(CrashCtlKey, "CrashDumpEnabled", 7)],
            DetectOps = [RegOp.CheckDword(CrashCtlKey, "CrashDumpEnabled", 2)],
        },
        new TweakDef
        {
            Id = "windbg-overwrite-existing-dump",
            Label = "Debugger: Overwrite Existing Dump File on Next Crash",
            Category = "Dev Drive / Developer",
            Tags = ["debugger", "crash-dump", "overwrite", "developer"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Prevents multiple crash dumps from filling the system drive.",
            Description =
                "Sets Overwrite=1 in CrashControl. When a crash occurs, the new dump file "
                + "overwrites the previously saved dump instead of incrementing the filename. "
                + "Conserves disk space on systems that crash infrequently.",
            RegistryKeys = [CrashCtlKey],
            ApplyOps = [RegOp.SetDword(CrashCtlKey, "Overwrite", 1)],
            RemoveOps = [RegOp.DeleteValue(CrashCtlKey, "Overwrite")],
            DetectOps = [RegOp.CheckDword(CrashCtlKey, "Overwrite", 1)],
        },
        new TweakDef
        {
            Id = "windbg-disable-auto-reboot-on-crash",
            Label = "Debugger: Disable Automatic Reboot After BSOD",
            Category = "Dev Drive / Developer",
            Tags = ["debugger", "bsod", "reboot", "developer", "crash"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "System stays at BSOD screen so the stop code and parameters can be read.",
            Description =
                "Sets AutoReboot=0 in CrashControl. Prevents Windows from automatically "
                + "restarting after a kernel crash. The blue screen remains visible so that "
                + "the error code and parameters can be noted before rebooting. ",
            RegistryKeys = [CrashCtlKey],
            ApplyOps = [RegOp.SetDword(CrashCtlKey, "AutoReboot", 0)],
            RemoveOps = [RegOp.SetDword(CrashCtlKey, "AutoReboot", 1)],
            DetectOps = [RegOp.CheckDword(CrashCtlKey, "AutoReboot", 0)],
        },
        new TweakDef
        {
            Id = "windbg-set-jit-debugger-windbg",
            Label = "Debugger: Register WinDbg as the JIT (Just-In-Time) Debugger",
            Category = "Dev Drive / Developer",
            Tags = ["debugger", "jit", "windbg", "developer", "crash"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            SideEffects = "Requires WinDbg to be installed; path may differ.",
            ImpactNote = "Automatically attaches WinDbg when unhandled exceptions occur in any process.",
            Description =
                "Sets Debugger and Auto values in AeDebug to register WinDbg as the "
                + "JIT debugger. When any process throws an unhandled exception, Windows "
                + "launches WinDbg attached to that process for live debugging. "
                + "Requires WinDbg at the default install path.",
            RegistryKeys = [AeDebugKey],
            ApplyOps =
            [
                RegOp.SetString(AeDebugKey, "Debugger", @"""C:\Program Files (x86)\Windows Kits\10\Debuggers\x64\windbg.exe"" -p %ld -e %ld -g"),
                RegOp.SetString(AeDebugKey, "Auto", "1"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(AeDebugKey, "Debugger"),
                RegOp.DeleteValue(AeDebugKey, "Auto"),
            ],
            DetectOps = [RegOp.CheckString(AeDebugKey, "Auto", "1")],
        },
        new TweakDef
        {
            Id = "windbg-disable-jit-debugger",
            Label = "Debugger: Disable JIT Debugger (Show App Error Dialog Instead)",
            Category = "Dev Drive / Developer",
            Tags = ["debugger", "jit", "disable", "developer"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Reverts to the standard 'application stopped working' error dialog.",
            Description =
                "Sets Auto=0 in AeDebug. When a process crashes, Windows shows the standard "
                + "crash dialog instead of launching a JIT debugger. "
                + "Default on client Windows. Useful on production machines where debugger "
                + "pop-ups would be disruptive.",
            RegistryKeys = [AeDebugKey],
            ApplyOps = [RegOp.SetString(AeDebugKey, "Auto", "0")],
            RemoveOps = [RegOp.DeleteValue(AeDebugKey, "Auto")],
            DetectOps = [RegOp.CheckString(AeDebugKey, "Auto", "0")],
        },
        new TweakDef
        {
            Id = "windbg-disable-wer-crash-reports",
            Label = "Debugger: Disable Windows Error Reporting (No Crash Data Upload)",
            Category = "Dev Drive / Developer",
            Tags = ["wer", "crash-report", "privacy", "developer", "upload"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Prevents crash telemetry from being sent to Microsoft while debugging.",
            Description =
                "Sets Disabled=1 in Windows Error Reporting key. Completely disables WER "
                + "so that no crash data is collected or uploaded. Useful on developer "
                + "machines to prevent partial dumps from being sent to Microsoft during "
                + "internal debugging sessions.",
            RegistryKeys = [WerKey],
            ApplyOps = [RegOp.SetDword(WerKey, "Disabled", 1)],
            RemoveOps = [RegOp.SetDword(WerKey, "Disabled", 0)],
            DetectOps = [RegOp.CheckDword(WerKey, "Disabled", 1)],
        },
        new TweakDef
        {
            Id = "windbg-enable-wer-local-dumps",
            Label = "Debugger: Store WER Crash Dumps Locally",
            Category = "Dev Drive / Developer",
            Tags = ["wer", "crash-dump", "local", "developer"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Local WER queue retains crash data for developer analysis without uploading.",
            Description =
                "Sets DontSendAdditionalData=1 and LoggingDisabled=0 in WER. Configures "
                + "Windows Error Reporting to keep crash queue files locally and not send "
                + "them to Microsoft. Crash mini-dumps remain on disk in "
                + @"%LOCALAPPDATA%\CrashDumps for developer examination.",
            RegistryKeys = [WerKey],
            ApplyOps =
            [
                RegOp.SetDword(WerKey, "DontSendAdditionalData", 1),
                RegOp.SetDword(WerKey, "LoggingDisabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(WerKey, "DontSendAdditionalData"),
                RegOp.DeleteValue(WerKey, "LoggingDisabled"),
            ],
            DetectOps = [RegOp.CheckDword(WerKey, "DontSendAdditionalData", 1)],
        },
        new TweakDef
        {
            Id = "windbg-disable-wer-ui-crash-dialog",
            Label = "Debugger: Suppress Crash Dialog Pop-Up During Debugging",
            Category = "Dev Drive / Developer",
            Tags = ["wer", "crash-dialog", "developer", "suppress"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Suppressing the dialog prevents interruptions during automated testing.",
            Description =
                "Sets DontShowUI=1 in the user WER key. Prevents the 'application stopped "
                + "working' dialog from appearing for the current user. Particularly useful "
                + "when running automated tests or performance benchmarks where crash dialogs "
                + "would block test execution.",
            RegistryKeys = [WerUserKey],
            ApplyOps = [RegOp.SetDword(WerUserKey, "DontShowUI", 1)],
            RemoveOps = [RegOp.SetDword(WerUserKey, "DontShowUI", 0)],
            DetectOps = [RegOp.CheckDword(WerUserKey, "DontShowUI", 1)],
        },
        new TweakDef
        {
            Id = "windbg-enable-exception-log",
            Label = "Debugger: Enable First-Chance Exception Logging",
            Category = "Dev Drive / Developer",
            Tags = ["debugger", "exception", "logging", "developer"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Logs first-chance exceptions to the event log for post-mortem analysis.",
            Description =
                "Sets EnableApplicationCrashOnException=1 in CrashControl. Configures the "
                + "kernel to log first-chance exceptions to the System event log before "
                + "they are dispatched to user-mode exception handlers. "
                + "Useful for diagnosing silent exception swallowing in production services.",
            RegistryKeys = [CrashCtlKey],
            ApplyOps = [RegOp.SetDword(CrashCtlKey, "EnableApplicationCrashOnException", 1)],
            RemoveOps = [RegOp.DeleteValue(CrashCtlKey, "EnableApplicationCrashOnException")],
            DetectOps = [RegOp.CheckDword(CrashCtlKey, "EnableApplicationCrashOnException", 1)],
        },
    ];
}
