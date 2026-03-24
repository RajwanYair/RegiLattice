namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WindowsReliabilityPolicy
{
    private const string RelKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Reliability";
    private const string WerKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "relpol-disable-shutdown-tracker",
            Label = "Reliability Policy: Disable Shutdown Event Tracker",
            Category = "Windows Reliability Policy",
            Description =
                "Disables the Shutdown Event Tracker dialog that prompts users or admins for a reason when the system is shut down or restarted. Useful for desktops that do not require uptime tracking.",
            Tags = ["reliability", "shutdown", "event-tracker", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Removes the shutdown-reason prompt on desktops without uptime tracking.",
            RegistryKeys = [RelKey],
            ApplyOps = [RegOp.SetDword(RelKey, "ShutdownEventTrackerDisabled", 1)],
            RemoveOps = [RegOp.DeleteValue(RelKey, "ShutdownEventTrackerDisabled")],
            DetectOps = [RegOp.CheckDword(RelKey, "ShutdownEventTrackerDisabled", 1)],
        },
        new TweakDef
        {
            Id = "relpol-disable-rac-reporting",
            Label = "Reliability Policy: Disable RAC Problem Reporting to Microsoft",
            Category = "Windows Reliability Policy",
            Description =
                "Disables the Reliability Analysis Component (RAC) from forwarding problem report data to Microsoft. RAC gathers application crash data and forwards it to Problem Reports and Solutions (WER).",
            Tags = ["reliability", "rac", "wer", "reporting", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Stops RAC from forwarding application crash data to Microsoft via WER.",
            RegistryKeys = [RelKey],
            ApplyOps = [RegOp.SetDword(RelKey, "PCH_DoNotReport", 1)],
            RemoveOps = [RegOp.DeleteValue(RelKey, "PCH_DoNotReport")],
            DetectOps = [RegOp.CheckDword(RelKey, "PCH_DoNotReport", 1)],
        },
        new TweakDef
        {
            Id = "relpol-disable-archive",
            Label = "Reliability Policy: Disable Reliability Data Archive",
            Category = "Windows Reliability Policy",
            Description =
                "Disables the reliability history archive database written by the Reliability Analysis Component (RACAgent). Prevents creation and retention of Windows reliability scores and application failure records.",
            Tags = ["reliability", "archive", "rac", "history", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Prevents creation of the reliability score database and failure history file.",
            RegistryKeys = [RelKey],
            ApplyOps = [RegOp.SetDword(RelKey, "DisableArchive", 1)],
            RemoveOps = [RegOp.DeleteValue(RelKey, "DisableArchive")],
            DetectOps = [RegOp.CheckDword(RelKey, "DisableArchive", 1)],
        },
        new TweakDef
        {
            Id = "relpol-limit-archive-count",
            Label = "Reliability Policy: Limit Reliability Archive Maximum Count",
            Category = "Windows Reliability Policy",
            Description =
                "Limits the number of reliability history records stored in the RAC database. Reducing the max archive count prevents unbounded growth of reliability data on low-disk-space endpoints.",
            Tags = ["reliability", "archive", "limit", "disk-space", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Caps reliability history records to prevent unbounded disk usage.",
            RegistryKeys = [RelKey],
            ApplyOps = [RegOp.SetDword(RelKey, "MaxArchiveCount", 10)],
            RemoveOps = [RegOp.DeleteValue(RelKey, "MaxArchiveCount")],
            DetectOps = [RegOp.CheckDword(RelKey, "MaxArchiveCount", 10)],
        },
        new TweakDef
        {
            Id = "relpol-disable-shutdown-reason-required",
            Label = "Reliability Policy: Disable Shutdown Reason Requirement",
            Category = "Windows Reliability Policy",
            Description =
                "Removes the requirement for users to provide an annotated reason when shutting down or restarting the system. Complements the Shutdown Event Tracker disable for unattended workstations.",
            Tags = ["reliability", "shutdown", "reason", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Removes the mandatory reason field from the shutdown/restart dialog.",
            RegistryKeys = [RelKey],
            ApplyOps = [RegOp.SetDword(RelKey, "ReasonRequired", 0)],
            RemoveOps = [RegOp.DeleteValue(RelKey, "ReasonRequired")],
            DetectOps = [RegOp.CheckDword(RelKey, "ReasonRequired", 0)],
        },
        new TweakDef
        {
            Id = "relpol-disable-shutdown-reason-display",
            Label = "Reliability Policy: Disable Shutdown Reason UI Display",
            Category = "Windows Reliability Policy",
            Description =
                "Disables the on-screen display of shutdown reason annotations set by the Shutdown Event Tracker. Reduces noise in end-user shutdown flows where reason data is collected only for IT audit purposes.",
            Tags = ["reliability", "shutdown", "reason", "ui", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Hides shutdown reason annotations from end-user shutdown flows.",
            RegistryKeys = [RelKey],
            ApplyOps = [RegOp.SetDword(RelKey, "ShutdownReasonOn", 0)],
            RemoveOps = [RegOp.DeleteValue(RelKey, "ShutdownReasonOn")],
            DetectOps = [RegOp.CheckDword(RelKey, "ShutdownReasonOn", 0)],
        },
        new TweakDef
        {
            Id = "relpol-block-wer-auto-upload",
            Label = "Reliability Policy: Block WER Auto-Upload of Crash Dumps",
            Category = "Windows Reliability Policy",
            Description =
                "Prevents Windows Error Reporting from automatically uploading minidumps and full memory dumps to Microsoft. Crash dumps can contain sensitive application data, PII, or credentials from process memory.",
            Tags = ["reliability", "wer", "crash-dump", "upload", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Prevents automatic upload of crash dumps; protects PII and credentials in memory.",
            RegistryKeys = [WerKey],
            ApplyOps = [RegOp.SetDword(WerKey, "LoggingDisabled", 1)],
            RemoveOps = [RegOp.DeleteValue(WerKey, "LoggingDisabled")],
            DetectOps = [RegOp.CheckDword(WerKey, "LoggingDisabled", 1)],
        },
        new TweakDef
        {
            Id = "relpol-disable-wer-ui-prompt",
            Label = "Reliability Policy: Disable WER User Prompt Dialog",
            Category = "Windows Reliability Policy",
            Description =
                "Suppresses the Windows Error Reporting prompt dialog when an application crashes. On headless or thin-client deployments, the WER dialog can block process termination and require remote intervention.",
            Tags = ["reliability", "wer", "dialog", "prompt", "headless", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Suppresses WER crash dialogs on headless/thin-client deployments.",
            RegistryKeys = [WerKey],
            ApplyOps = [RegOp.SetDword(WerKey, "DisableUI", 1)],
            RemoveOps = [RegOp.DeleteValue(WerKey, "DisableUI")],
            DetectOps = [RegOp.CheckDword(WerKey, "DisableUI", 1)],
        },
        new TweakDef
        {
            Id = "relpol-limit-wer-queue-count",
            Label = "Reliability Policy: Limit WER Report Queue Size",
            Category = "Windows Reliability Policy",
            Description =
                "Limits the maximum number of error reports held in the WER queue to a small number. On heavily-used endpoints, an unbounded WER queue can consume significant disk space.",
            Tags = ["reliability", "wer", "queue", "limit", "disk-space", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Caps the WER queue to prevent unbounded disk usage on busy endpoints.",
            RegistryKeys = [WerKey],
            ApplyOps = [RegOp.SetDword(WerKey, "MaxQueueCount", 5)],
            RemoveOps = [RegOp.DeleteValue(WerKey, "MaxQueueCount")],
            DetectOps = [RegOp.CheckDword(WerKey, "MaxQueueCount", 5)],
        },
        new TweakDef
        {
            Id = "relpol-disable-wer-kernel-dump",
            Label = "Reliability Policy: Disable WER Kernel Fault/Dump Reporting",
            Category = "Windows Reliability Policy",
            Description =
                "Disables Windows Error Reporting capture of kernel-mode fault data (BSoD minidumps). Prevents automatic transmission of kernel dump data to Microsoft after BSODs on sensitive systems.",
            Tags = ["reliability", "wer", "kernel-dump", "bsod", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Disables kernel fault dump collection; protects sensitive kernel-space data.",
            RegistryKeys = [WerKey],
            ApplyOps = [RegOp.SetDword(WerKey, "DisableKernelFaultLogging", 1)],
            RemoveOps = [RegOp.DeleteValue(WerKey, "DisableKernelFaultLogging")],
            DetectOps = [RegOp.CheckDword(WerKey, "DisableKernelFaultLogging", 1)],
        },
    ];
}
