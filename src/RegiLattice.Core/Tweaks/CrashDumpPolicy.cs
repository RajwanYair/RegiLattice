// RegiLattice.Core — Tweaks/CrashDumpPolicy.cs
// Crash Dump & Error Recovery Policy — Sprint 424.
// Controls Windows crash dump generation, automatic restart behavior,
// and error recovery telemetry via the CrashControl registry key.
// Category: "Crash Dump Policy" | Slug: cdump
// Registry path: HKLM\SYSTEM\CurrentControlSet\Control\CrashControl

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class CrashDumpPolicy
{
    private const string CcKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "cdump-disable-crash-dump",
                Label = "Disable Crash Dump Generation",
                Category = "Crash Dump Policy",
                Description =
                    "Sets CrashDumpEnabled=0 to disable creation of any memory dump file when Windows encounters a stop error (BSOD). Frees disk space on constrained devices and prevents large dump files from accumulating. Default: 7 (auto).",
                Tags = ["crash dump", "bsod", "memory", "storage", "policy"],
                NeedsAdmin = true,
                CorpSafe = false,
                ImpactScore = 3,
                SafetyRating = 2,
                ImpactNote = "Disables all crash dumps; BSODs produce no diagnostic artifacts. Reduces disk use but hampers debugging.",
                ApplyOps = [RegOp.SetDword(CcKey, "CrashDumpEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(CcKey, "CrashDumpEnabled")],
                DetectOps = [RegOp.CheckDword(CcKey, "CrashDumpEnabled", 0)],
            },
            new TweakDef
            {
                Id = "cdump-set-mini-dump",
                Label = "Set Crash Dump to Minidump Only",
                Category = "Crash Dump Policy",
                Description =
                    "Sets CrashDumpEnabled=3 to configure Windows to write only a small minidump (~256 KB) on stop errors instead of a full or kernel memory dump. Balances debuggability with disk usage. Default: 7 (auto).",
                Tags = ["crash dump", "minidump", "bsod", "storage", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "256 KB minidump on BSOD; sufficient for driver crash analysis with low disk overhead.",
                ApplyOps = [RegOp.SetDword(CcKey, "CrashDumpEnabled", 3)],
                RemoveOps = [RegOp.DeleteValue(CcKey, "CrashDumpEnabled")],
                DetectOps = [RegOp.CheckDword(CcKey, "CrashDumpEnabled", 3)],
            },
            new TweakDef
            {
                Id = "cdump-disable-auto-reboot",
                Label = "Disable Automatic Reboot on BSOD",
                Category = "Crash Dump Policy",
                Description =
                    "Sets AutoReboot=0 to prevent Windows from automatically restarting immediately after a stop error. The system halts at the blue screen allowing the error code to be read. Useful for physical machines and servers.",
                Tags = ["crash dump", "auto reboot", "bsod", "server", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "System stays at BSOD screen until manually rebooted; stop code visible for diagnosis.",
                ApplyOps = [RegOp.SetDword(CcKey, "AutoReboot", 0)],
                RemoveOps = [RegOp.DeleteValue(CcKey, "AutoReboot")],
                DetectOps = [RegOp.CheckDword(CcKey, "AutoReboot", 0)],
            },
            new TweakDef
            {
                Id = "cdump-disable-log-event",
                Label = "Disable BSOD Event Log Entry",
                Category = "Crash Dump Policy",
                Description =
                    "Sets LogEvent=0 to prevent Windows from writing an event log entry to the System log when a stop error occurs. Default: 1 (log enabled). Reduces event log verbosity on systems with frequent crash-recovery cycles.",
                Tags = ["crash dump", "event log", "bsod", "logging", "policy"],
                NeedsAdmin = true,
                CorpSafe = false,
                ImpactScore = 2,
                SafetyRating = 3,
                ImpactNote = "No event log entry on BSOD; reduces auditability of stop errors.",
                ApplyOps = [RegOp.SetDword(CcKey, "LogEvent", 0)],
                RemoveOps = [RegOp.DeleteValue(CcKey, "LogEvent")],
                DetectOps = [RegOp.CheckDword(CcKey, "LogEvent", 0)],
            },
            new TweakDef
            {
                Id = "cdump-disable-send-alert",
                Label = "Disable BSOD Admin Alert",
                Category = "Crash Dump Policy",
                Description =
                    "Sets SendAlert=0 to prevent Windows from sending a network alert to the designated administrator message server when a stop error occurs. Default: 1 in domain environments. Relevant for workgroup machines.",
                Tags = ["crash dump", "alert", "network", "admin", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "No network alert sent on BSOD; appropriate for standalone or home-network machines.",
                ApplyOps = [RegOp.SetDword(CcKey, "SendAlert", 0)],
                RemoveOps = [RegOp.DeleteValue(CcKey, "SendAlert")],
                DetectOps = [RegOp.CheckDword(CcKey, "SendAlert", 0)],
            },
            new TweakDef
            {
                Id = "cdump-disable-storage-telemetry",
                Label = "Disable Crash Dump Telemetry Collection",
                Category = "Crash Dump Policy",
                Description =
                    "Sets StorageTelemetryEnabled=0 to prevent WER from uploading crash dump telemetry to Microsoft when connected. Combines with WER upload policies for comprehensive crash data privacy.",
                Tags = ["crash dump", "telemetry", "wer", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Crash dump data not uploaded for telemetry analysis; local dumps still created if enabled.",
                ApplyOps = [RegOp.SetDword(CcKey, "StorageTelemetryEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(CcKey, "StorageTelemetryEnabled")],
                DetectOps = [RegOp.CheckDword(CcKey, "StorageTelemetryEnabled", 0)],
            },
            new TweakDef
            {
                Id = "cdump-disable-dump-log-file",
                Label = "Disable Crash Dump Log File",
                Category = "Crash Dump Policy",
                Description =
                    "Sets EnableLogFile=0 to prevent the crash dump subsystem from writing the memory.dmp log header file alongside the dump. Default: 1. Reduces extra disk writes and keeps the dump directory clean.",
                Tags = ["crash dump", "log file", "disk", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "No supplemental dump log file written; core dump file (if enabled) still created.",
                ApplyOps = [RegOp.SetDword(CcKey, "EnableLogFile", 0)],
                RemoveOps = [RegOp.DeleteValue(CcKey, "EnableLogFile")],
                DetectOps = [RegOp.CheckDword(CcKey, "EnableLogFile", 0)],
            },
            new TweakDef
            {
                Id = "cdump-overwrite-existing-dump",
                Label = "Overwrite Existing Crash Dump",
                Category = "Crash Dump Policy",
                Description =
                    "Sets Overwrite=1 so that each new crash dump overwrites the previous dump file rather than keeping multiple copies. Prevents disk space exhaustion on managed devices that experience occasional stop errors.",
                Tags = ["crash dump", "overwrite", "disk", "storage", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "New BSOD dump replaces old one; only most recent crash is retained on disk.",
                ApplyOps = [RegOp.SetDword(CcKey, "Overwrite", 1)],
                RemoveOps = [RegOp.DeleteValue(CcKey, "Overwrite")],
                DetectOps = [RegOp.CheckDword(CcKey, "Overwrite", 1)],
            },
            new TweakDef
            {
                Id = "cdump-disable-filter-pages",
                Label = "Disable Crash Dump Page Filtering",
                Category = "Crash Dump Policy",
                Description =
                    "Sets FilterPages=0 to disable the Windows crash dump page-filtering feature that removes unnecessary memory pages before writing the dump file. Produces more complete dumps at the cost of larger file size. Default: 1.",
                Tags = ["crash dump", "filter", "memory", "debugging", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Full unfiltered memory dumps; larger files but more useful for deep kernel analysis.",
                ApplyOps = [RegOp.SetDword(CcKey, "FilterPages", 0)],
                RemoveOps = [RegOp.DeleteValue(CcKey, "FilterPages")],
                DetectOps = [RegOp.CheckDword(CcKey, "FilterPages", 0)],
            },
            new TweakDef
            {
                Id = "cdump-disable-dedicated-dump-file",
                Label = "Disable Dedicated Dump File",
                Category = "Crash Dump Policy",
                Description =
                    "Sets DisableDedicatedDumpFile=1 to prevent Windows from reserving a dedicated page-file-adjacent dump file (used on devices where the page file is too small for a full dump). Default: 0 (dedicated file used when needed).",
                Tags = ["crash dump", "dedicated file", "pagefile", "storage", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "No reserved dump file; on low-pagefile devices the dump may fail to be captured.",
                ApplyOps = [RegOp.SetDword(CcKey, "DisableDedicatedDumpFile", 1)],
                RemoveOps = [RegOp.DeleteValue(CcKey, "DisableDedicatedDumpFile")],
                DetectOps = [RegOp.CheckDword(CcKey, "DisableDedicatedDumpFile", 1)],
            },
        ];
}
