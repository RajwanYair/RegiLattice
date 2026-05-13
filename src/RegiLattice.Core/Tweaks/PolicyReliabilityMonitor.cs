namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

[TweakModule]
internal static class PolicyReliabilityMonitor
{
    private const string RacKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Reliability";
    private const string WerKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting";
    private const string PcKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PCHealth\ErrorReporting";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "maint-reliability-shutdown-reason-text",
            Label = "Require Shutdown Reason Text",
            Category = "Privacy — Location Sensors",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets ShutdownReasonUI=1 and ReasonCodeRequired=1 in Reliability policy. "
                + "Forces users to select a shutdown reason and enter explanatory text when initiating a planned or unplanned shutdown. "
                + "Improves uptime tracking and post-incident root cause analysis in managed enterprise environments.",
            Tags = ["reliability", "shutdown", "reason", "audit", "uptime"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Prompts for shutdown reason; visible user impact at every shutdown/restart.",
            ApplyOps = [RegOp.SetDword(RacKey, "ShutdownReasonUI", 1), RegOp.SetDword(RacKey, "ReasonCodeRequired", 1)],
            RemoveOps = [RegOp.DeleteValue(RacKey, "ShutdownReasonUI"), RegOp.DeleteValue(RacKey, "ReasonCodeRequired")],
            DetectOps = [RegOp.CheckDword(RacKey, "ShutdownReasonUI", 1), RegOp.CheckDword(RacKey, "ReasonCodeRequired", 1)],
        },
        new TweakDef
        {
            Id = "maint-reliability-racevent-interval",
            Label = "Extend Reliability Event Logging Interval",
            Category = "Privacy — Location Sensors",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets TimeStampInterval=7 in Reliability policy, extending the RAC (Reliability Analysis Component) time-stamp interval to 7 days. "
                + "Reduces disk I/O for reliability data collection on endpoints where the default hourly reliability logging is excessive. "
                + "Useful for write-sensitive devices such as those with eMMC storage.",
            Tags = ["reliability", "rac", "logging", "interval", "disk-io"],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Reduces reliability event logging frequency; less granular uptime data.",
            ApplyOps = [RegOp.SetDword(RacKey, "TimeStampInterval", 7)],
            RemoveOps = [RegOp.DeleteValue(RacKey, "TimeStampInterval")],
            DetectOps = [RegOp.CheckDword(RacKey, "TimeStampInterval", 7)],
        },
        new TweakDef
        {
            Id = "maint-wer-disable-default-consent",
            Label = "Disable Windows Error Reporting Default Consent",
            Category = "Privacy — Location Sensors",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DefaultConsent=1 in WER policy (Always Ask). "
                + "Requires explicit user or administrator consent before any error report is sent to Microsoft. "
                + "Prevents automatic or silent submission of crash dumps and application error telemetry that may contain sensitive process memory contents.",
            Tags = ["wer", "error-reporting", "consent", "privacy", "telemetry"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Prompts before any error report upload; no silent data submission.",
            ApplyOps = [RegOp.SetDword(WerKey, "DefaultConsent", 1)],
            RemoveOps = [RegOp.DeleteValue(WerKey, "DefaultConsent")],
            DetectOps = [RegOp.CheckDword(WerKey, "DefaultConsent", 1)],
        },
        new TweakDef
        {
            Id = "maint-wer-disable-kernel-faults",
            Label = "Exclude Kernel-Level Faults from WER",
            Category = "Privacy — Location Sensors",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets ExcludeKernelFaults=1 in WER policy. "
                + "Prevents kernel-level crash events from being included in Windows Error Reporting submissions. "
                + "Kernel dumps can contain entire memory contents including encryption keys and privileged process memory, making them unsuitable for external submission.",
            Tags = ["wer", "kernel-dump", "crash", "memory", "security"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Excludes kernel crash data from WER submissions; reduces data leakage risk.",
            ApplyOps = [RegOp.SetDword(WerKey, "ExcludeKernelFaults", 1)],
            RemoveOps = [RegOp.DeleteValue(WerKey, "ExcludeKernelFaults")],
            DetectOps = [RegOp.CheckDword(WerKey, "ExcludeKernelFaults", 1)],
        },
        new TweakDef
        {
            Id = "maint-wer-disable-archive-behavior",
            Label = "Disable WER Problem Reporting Queue Archival",
            Category = "Privacy — Location Sensors",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DumpType=0 in WER policy. "
                + "Prevents Windows Error Reporting from archiving application crash mini-dumps to the local queue directory for later upload. "
                + "Reduces disk usage from accumulated crash dump files and prevents sensitive process memory from persisting on disk beyond the immediate crash event.",
            Tags = ["wer", "archive", "dump", "disk", "privacy"],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Prevents crash dump accumulation on disk; no deferred upload queue.",
            ApplyOps = [RegOp.SetDword(WerKey, "DumpType", 0)],
            RemoveOps = [RegOp.DeleteValue(WerKey, "DumpType")],
            DetectOps = [RegOp.CheckDword(WerKey, "DumpType", 0)],
        },
        new TweakDef
        {
            Id = "maint-pch-disable-all-error-reporting",
            Label = "Disable All PCHealth Error Reporting Channels",
            Category = "Privacy — Location Sensors",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets AllOrNone=1 and ShowUI=0 in PCHealth\\ErrorReporting policy. "
                + "Blocks the PCHealth component from showing any error reporting UI and from queuing reports to any reporting channel. "
                + "Complements the DoReport=0 setting to ensure the legacy error reporting subsystem is fully silent.",
            Tags = ["pchealth", "error-reporting", "legacy", "silent", "ui"],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Silences legacy PCHealth error dialogs and submission queue.",
            ApplyOps = [RegOp.SetDword(PcKey, "AllOrNone", 1), RegOp.SetDword(PcKey, "ShowUI", 0)],
            RemoveOps = [RegOp.DeleteValue(PcKey, "AllOrNone"), RegOp.DeleteValue(PcKey, "ShowUI")],
            DetectOps = [RegOp.CheckDword(PcKey, "AllOrNone", 1), RegOp.CheckDword(PcKey, "ShowUI", 0)],
        },
        new TweakDef
        {
            Id = "maint-pch-force-queue-mode",
            Label = "Set PCHealth Reporting to Queue Mode Only",
            Category = "Privacy — Location Sensors",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets ForceQueue=1 in PCHealth\\ErrorReporting policy. "
                + "Forces error reports to accumulate in a local queue rather than being submitted immediately or interactively. "
                + "Gives administrators time to review and approve queued reports before any data leaves the endpoint.",
            Tags = ["pchealth", "queue", "error-reporting", "review", "approval"],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Queues reports for admin review; no immediate uploads.",
            ApplyOps = [RegOp.SetDword(PcKey, "ForceQueue", 1)],
            RemoveOps = [RegOp.DeleteValue(PcKey, "ForceQueue")],
            DetectOps = [RegOp.CheckDword(PcKey, "ForceQueue", 1)],
        },
        new TweakDef
        {
            Id = "maint-pch-disable-report-by-app",
            Label = "Disable Per-Application Error Reporting Override",
            Category = "Privacy — Location Sensors",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets IncludeMicrosoftApps=0 and IncludeWindowsApps=0 in PCHealth\\ErrorReporting policy. "
                + "Prevents individual Microsoft and Windows applications from independently initiating error reports through the PCHealth channel. "
                + "Ensures that the enterprise error reporting policy cannot be overridden by per-application reporting preferences.",
            Tags = ["pchealth", "per-app", "error-reporting", "override", "policy"],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Blocks per-app PCHealth error submissions regardless of app preference.",
            ApplyOps = [RegOp.SetDword(PcKey, "IncludeMicrosoftApps", 0), RegOp.SetDword(PcKey, "IncludeWindowsApps", 0)],
            RemoveOps = [RegOp.DeleteValue(PcKey, "IncludeMicrosoftApps"), RegOp.DeleteValue(PcKey, "IncludeWindowsApps")],
            DetectOps = [RegOp.CheckDword(PcKey, "IncludeMicrosoftApps", 0), RegOp.CheckDword(PcKey, "IncludeWindowsApps", 0)],
        },
    ];
}
