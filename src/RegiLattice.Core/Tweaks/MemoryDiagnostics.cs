// RegiLattice.Core — Tweaks/MemoryDiagnostics.cs
// Windows crash dump and Windows Error Reporting settings (Sprint 85).
// Slug "dump" / "wer" — Configure minidumps, full dumps, WER queue, recovery.
// Distinct from Crash.cs (which handles SilentlyContinue/bug check settings).

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class MemoryDiagnostics
{
    private const string CrashControl =
        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl";

    private const string Wer =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting";

    private const string WerQueue =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting\Queue";

    private const string WerConsentPolicy =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PCHealth\ErrorReporting";

    private const string WerPolicy =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "dump-disable-crash-dumps",
            Label = "Disable Crash Memory Dumps (No Dump Files Written)",
            Category = "Memory & Crash Diagnostics",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 3,
            SafetyRating = 3,
            Tags = ["dump", "crash", "memory", "disk space"],
            Description =
                "Prevents Windows from writing any memory dump file when the system "
                + "crashes (BSOD). Saves substantial disk space on SSDs but eliminates "
                + "crash debugging capability. CrashDumpEnabled=0.",
            ApplyOps = [RegOp.SetDword(CrashControl, "CrashDumpEnabled", 0)],
            RemoveOps = [RegOp.SetDword(CrashControl, "CrashDumpEnabled", 7)],
            DetectOps = [RegOp.CheckDword(CrashControl, "CrashDumpEnabled", 0)],
        },
        new TweakDef
        {
            Id = "dump-set-small-minidump",
            Label = "Set Crash Dump to Small Memory Dump (256 KB)",
            Category = "Memory & Crash Diagnostics",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["dump", "crash", "minidump", "disk space"],
            Description =
                "Configures Windows to write only a small memory dump (256 KB) on BSOD "
                + "instead of a full kernel or complete dump. Preserves basic debug info "
                + "(stop code, loaded drivers) with minimal disk use.",
            ApplyOps = [RegOp.SetDword(CrashControl, "CrashDumpEnabled", 3)],
            RemoveOps = [RegOp.SetDword(CrashControl, "CrashDumpEnabled", 7)],
            DetectOps = [RegOp.CheckDword(CrashControl, "CrashDumpEnabled", 3)],
        },
        new TweakDef
        {
            Id = "dump-enable-auto-reboot-on-crash",
            Label = "Enable Automatic Reboot After BSOD",
            Category = "Memory & Crash Diagnostics",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["dump", "crash", "reboot", "bsod"],
            Description =
                "Ensures Windows automatically restarts after a system crash (BSOD) "
                + "rather than staying at the blue screen indefinitely. Default behaviour "
                + "on most systems but restorable if previously disabled.",
            ApplyOps = [RegOp.SetDword(CrashControl, "AutoReboot", 1)],
            RemoveOps = [RegOp.SetDword(CrashControl, "AutoReboot", 0)],
            DetectOps = [RegOp.CheckDword(CrashControl, "AutoReboot", 1)],
        },
        new TweakDef
        {
            Id = "dump-overwrite-existing-dump",
            Label = "Overwrite Existing Dump File on Crash",
            Category = "Memory & Crash Diagnostics",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["dump", "crash", "overwrite", "disk space"],
            Description =
                "Configures Windows to overwrite the existing dump file rather than "
                + "keeping multiple copies. Prevents the dumps folder from consuming "
                + "multiple GBs after repeated crashes.",
            ApplyOps = [RegOp.SetDword(CrashControl, "Overwrite", 1)],
            RemoveOps = [RegOp.DeleteValue(CrashControl, "Overwrite")],
            DetectOps = [RegOp.CheckDword(CrashControl, "Overwrite", 1)],
        },
        new TweakDef
        {
            Id = "dump-disable-wer-error-reporting",
            Label = "Disable Windows Error Reporting (WER)",
            Category = "Memory & Crash Diagnostics",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 3,
            SafetyRating = 4,
            Tags = ["wer", "error reporting", "privacy", "telemetry"],
            Description =
                "Disables Windows Error Reporting so application crashes are not "
                + "sent to Microsoft. Reduces network activity and privacy exposure "
                + "but prevents automatic driver/app fix recommendations.",
            ApplyOps = [RegOp.SetDword(Wer, "Disabled", 1)],
            RemoveOps = [RegOp.SetDword(Wer, "Disabled", 0)],
            DetectOps = [RegOp.CheckDword(Wer, "Disabled", 1)],
        },
        new TweakDef
        {
            Id = "dump-disable-wer-reporting-policy",
            Label = "Disable WER via Group Policy",
            Category = "Memory & Crash Diagnostics",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 3,
            SafetyRating = 4,
            Tags = ["wer", "policy", "privacy", "error reporting"],
            Description =
                "Disables Windows Error Reporting via the machine-wide policy key. "
                + "This prevents users from re-enabling WER through Settings. "
                + "Complementary to the user-level Disabled flag.",
            ApplyOps = [RegOp.SetDword(WerPolicy, "Disabled", 1)],
            RemoveOps = [RegOp.DeleteValue(WerPolicy, "Disabled")],
            DetectOps = [RegOp.CheckDword(WerPolicy, "Disabled", 1)],
        },
        new TweakDef
        {
            Id = "dump-disable-wer-queue",
            Label = "Disable WER Queuing of Crash Reports",
            Category = "Memory & Crash Diagnostics",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 2,
            SafetyRating = 4,
            Tags = ["wer", "queue", "privacy", "error reporting"],
            Description =
                "Prevents WER from queuing crash reports for later upload. Stops "
                + "the background spool of error data that WER accumulates when an "
                + "internet connection is unavailable.",
            ApplyOps = [RegOp.SetDword(WerQueue, "Disable", 1)],
            RemoveOps = [RegOp.SetDword(WerQueue, "Disable", 0)],
            DetectOps = [RegOp.CheckDword(WerQueue, "Disable", 1)],
        },
        new TweakDef
        {
            Id = "dump-disable-silent-crash-ui",
            Label = "Disable Silent Crash Report Dialog",
            Category = "Memory & Crash Diagnostics",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["wer", "dialog", "crash", "ui"],
            Description =
                "Suppresses the WER consent dialog that asks the user whether to send "
                + "a crash report. Combined with Disabled=1, fully silences WER. "
                + "Applied via policy (DontShowUI).",
            ApplyOps = [RegOp.SetDword(WerPolicy, "DontShowUI", 1)],
            RemoveOps = [RegOp.DeleteValue(WerPolicy, "DontShowUI")],
            DetectOps = [RegOp.CheckDword(WerPolicy, "DontShowUI", 1)],
        },
        new TweakDef
        {
            Id = "dump-set-kernel-only-dump",
            Label = "Set Crash Dump to Kernel Memory Dump",
            Category = "Memory & Crash Diagnostics",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["dump", "crash", "kernel", "debug"],
            Description =
                "Configures Windows to write a kernel memory dump on BSOD. Captures "
                + "kernel memory pages only (not user-space), providing enough data for "
                + "most crash analysis while being smaller than a complete dump.",
            ApplyOps = [RegOp.SetDword(CrashControl, "CrashDumpEnabled", 2)],
            RemoveOps = [RegOp.SetDword(CrashControl, "CrashDumpEnabled", 7)],
            DetectOps = [RegOp.CheckDword(CrashControl, "CrashDumpEnabled", 2)],
        },
        new TweakDef
        {
            Id = "dump-enable-log-on-crash",
            Label = "Enable Kernel Event Log Entry on Crash",
            Category = "Memory & Crash Diagnostics",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Tags = ["dump", "crash", "event log", "logging"],
            Description =
                "Ensures Windows writes a 41 (Kernel-Power) or 1001 (BugCheck) event "
                + "log entry to the System log after a system crash and reboot. "
                + "LogEvent=1 (default on, restoring is useful if accidentally disabled).",
            ApplyOps = [RegOp.SetDword(CrashControl, "LogEvent", 1)],
            RemoveOps = [RegOp.DeleteValue(CrashControl, "LogEvent")],
            DetectOps = [RegOp.CheckDword(CrashControl, "LogEvent", 1)],
        },
    ];
}
