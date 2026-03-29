// RegiLattice.Core — Tweaks/WindowsDiagnosticsInfraPolicy.cs
// Windows Diagnostic Infrastructure (WDI) Group Policy controls — Sprint 431.
// Controls WDI scenario execution, diagnostic task scheduling, result caching,
// and telemetry-adjacent data collection via Group Policy registry paths.
// Category: "Windows Diagnostics Infrastructure Policy" | Slug: wdip
// Registry path: HKLM\SOFTWARE\Policies\Microsoft\Windows\WDI

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WindowsDiagnosticsInfraPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDI";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "wdip-disable-scenario-execution",
                Label = "Disable WDI Diagnostic Scenario Execution",
                Category = "Windows Diagnostics Infrastructure Policy",
                Description =
                    "Sets ScenarioExecutionEnabled=0 to prevent Windows Diagnostic Infrastructure from running "
                    + "built-in diagnostic scenarios. WDI scenarios collect hardware, driver, and application state "
                    + "data for Microsoft telemetry and self-healing analysis. Disabling reduces background data collection "
                    + "and eliminates associated CPU/disk spikes from diagnostic scenario runners.",
                Tags = ["diagnostics", "wdi", "telemetry", "performance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Disables WDI scenario runners; Windows self-healing and troubleshooter automation loses data inputs.",
                ApplyOps = [RegOp.SetDword(Key, "ScenarioExecutionEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "ScenarioExecutionEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "ScenarioExecutionEnabled", 0)],
            },
            new TweakDef
            {
                Id = "wdip-disable-diagnostic-triggers",
                Label = "Disable WDI Diagnostic Trigger Execution",
                Category = "Windows Diagnostics Infrastructure Policy",
                Description =
                    "Sets DiagnosticTriggerExecution=0 to prevent WDI from launching diagnostic routines in response "
                    + "to system event triggers (crash, hang, driver error). Trigger-based diagnostics collect snapshot data "
                    + "sent to the Windows Error Reporting and telemetry pipelines. Disabling reduces post-fault data gathering.",
                Tags = ["diagnostics", "wdi", "triggers", "telemetry", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Prevents event-triggered WDI diagnostics; reduces telemetry sent on crash/hang events.",
                ApplyOps = [RegOp.SetDword(Key, "DiagnosticTriggerExecution", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "DiagnosticTriggerExecution")],
                DetectOps = [RegOp.CheckDword(Key, "DiagnosticTriggerExecution", 0)],
            },
            new TweakDef
            {
                Id = "wdip-disable-result-summary",
                Label = "Disable WDI Diagnostic Result Summary Collection",
                Category = "Windows Diagnostics Infrastructure Policy",
                Description =
                    "Sets ResultSummaryEnabled=0 to prevent Windows Diagnostic Infrastructure from generating and storing "
                    + "diagnostic result summaries. These summaries aggregate diagnostic run outcomes and feed Windows reliability "
                    + "history and performance monitoring panels. Disabling reduces disk writes and summary data leakage.",
                Tags = ["diagnostics", "wdi", "results", "telemetry", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "No diagnostic result summaries stored; Reliability Monitor may show reduced detail.",
                ApplyOps = [RegOp.SetDword(Key, "ResultSummaryEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "ResultSummaryEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "ResultSummaryEnabled", 0)],
            },
            new TweakDef
            {
                Id = "wdip-disable-task-collection",
                Label = "Disable WDI Diagnostic Task Collection",
                Category = "Windows Diagnostics Infrastructure Policy",
                Description =
                    "Sets EnableDiagnosticTaskCollection=0 to stop WDI from scheduling and collecting data via "
                    + "background diagnostic tasks. Diagnostic task collection uses scheduled tasks in the "
                    + "'Microsoft\\Windows\\WDI' task folder to periodically gather system state; disabling "
                    + "prevents these tasks from running and collecting data.",
                Tags = ["diagnostics", "wdi", "tasks", "telemetry", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Disables WDI scheduled task data collection; Windows automatic maintenance diagnose routines stop.",
                ApplyOps = [RegOp.SetDword(Key, "EnableDiagnosticTaskCollection", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableDiagnosticTaskCollection")],
                DetectOps = [RegOp.CheckDword(Key, "EnableDiagnosticTaskCollection", 0)],
            },
            new TweakDef
            {
                Id = "wdip-disable-scenario-logging",
                Label = "Disable WDI Diagnostic Scenario Event Logging",
                Category = "Windows Diagnostics Infrastructure Policy",
                Description =
                    "Sets ScenarioLoggingEnabled=0 to prevent WDI from writing diagnostic scenario execution results "
                    + "to the Windows event log. Reduces event log noise from the WDI event provider and prevents "
                    + "diagnostic scenario details from appearing in event log SIEM forwarding pipelines.",
                Tags = ["diagnostics", "wdi", "logging", "event-log", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Stops WDI event log writes; reduces diagnostic event volume in Event Viewer.",
                ApplyOps = [RegOp.SetDword(Key, "ScenarioLoggingEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "ScenarioLoggingEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "ScenarioLoggingEnabled", 0)],
            },
            new TweakDef
            {
                Id = "wdip-disable-results-caching",
                Label = "Disable WDI Diagnostic Results Caching",
                Category = "Windows Diagnostics Infrastructure Policy",
                Description =
                    "Sets ResultsCachingEnabled=0 to prevent WDI from caching diagnostic run results to disk. "
                    + "Without caching, each diagnostic scenario must re-run fully if retriggered instead of serving "
                    + "a prior result. This reduces disk I/O from the WDI cache directory while slightly increasing "
                    + "CPU usage if scenarios are repeatedly triggered.",
                Tags = ["diagnostics", "wdi", "caching", "disk", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Disables result cache on disk; minimal impact unless WDI runs in tight repeat cycles.",
                ApplyOps = [RegOp.SetDword(Key, "ResultsCachingEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "ResultsCachingEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "ResultsCachingEnabled", 0)],
            },
            new TweakDef
            {
                Id = "wdip-set-max-persistence-days",
                Label = "Limit WDI Scenario Result Persistence",
                Category = "Windows Diagnostics Infrastructure Policy",
                Description =
                    "Sets MaxScenarioPersistenceDurationDays=1 to keep diagnostic scenario result data on disk for "
                    + "no more than 1 day. Stale diagnostic data is purged quickly, limiting the window in which "
                    + "stored diagnostic snapshots (which may include sensitive system state) persist on the endpoint.",
                Tags = ["diagnostics", "wdi", "retention", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Purges diagnostic results after 1 day; limits disk footprint of WDI data store.",
                ApplyOps = [RegOp.SetDword(Key, "MaxScenarioPersistenceDurationDays", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "MaxScenarioPersistenceDurationDays")],
                DetectOps = [RegOp.CheckDword(Key, "MaxScenarioPersistenceDurationDays", 1)],
            },
            new TweakDef
            {
                Id = "wdip-disable-msa-diagnostics",
                Label = "Disable MSA-Linked WDI Diagnostics",
                Category = "Windows Diagnostics Infrastructure Policy",
                Description =
                    "Sets DisableDiagnosticsMSA=1 to prevent WDI from associating diagnostic results and telemetry "
                    + "with the user's Microsoft Account (MSA). MSA-linked diagnostics allow personalised Windows "
                    + "troubleshooting recommendations based on cloud-side analysis of collected data. Disabling "
                    + "reduces cloud data upload associated with MSA-bound user sessions.",
                Tags = ["diagnostics", "wdi", "microsoft-account", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Severs diagnostic→MSA linkage; personalised troubleshooter suggestions unavailable.",
                ApplyOps = [RegOp.SetDword(Key, "DisableDiagnosticsMSA", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDiagnosticsMSA")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDiagnosticsMSA", 1)],
            },
            new TweakDef
            {
                Id = "wdip-disable-boot-diagnostics",
                Label = "Disable WDI Boot Diagnostic Collection",
                Category = "Windows Diagnostics Infrastructure Policy",
                Description =
                    "Sets EnableBootDiagnostics=0 to prevent WDI from collecting boot-time diagnostic data during "
                    + "the Windows startup phase. Boot diagnostics capture driver initialisation timing, boot event "
                    + "sequences, and early-phase performance counters used by the Windows startup performance "
                    + "optimisation engine. Disabling reduces boot-phase disk activity.",
                Tags = ["diagnostics", "wdi", "boot", "performance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Suppresses boot-phase WDI data collection; Windows startup optimiser loses recent boot data.",
                ApplyOps = [RegOp.SetDword(Key, "EnableBootDiagnostics", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableBootDiagnostics")],
                DetectOps = [RegOp.CheckDword(Key, "EnableBootDiagnostics", 0)],
            },
            new TweakDef
            {
                Id = "wdip-prevent-diagnostic-task-execution",
                Label = "Prevent WDI Diagnostic Task Execution via Policy",
                Category = "Windows Diagnostics Infrastructure Policy",
                Description =
                    "Sets PreventDiagnosticTaskExecution=1 to apply a hard Group Policy block on all WDI-managed "
                    + "diagnostic task execution. This policy-level enforcement takes precedence over local WDI "
                    + "configuration and prevents individual users or services from re-enabling WDI task execution "
                    + "through Task Scheduler or WMI. Combines with ScenarioExecutionEnabled=0 for full lockdown.",
                Tags = ["diagnostics", "wdi", "policy", "lockdown", "gpo"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Policy-level WDI task block; Windows troubleshooters and self-healing routines will not trigger.",
                ApplyOps = [RegOp.SetDword(Key, "PreventDiagnosticTaskExecution", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "PreventDiagnosticTaskExecution")],
                DetectOps = [RegOp.CheckDword(Key, "PreventDiagnosticTaskExecution", 1)],
            },
        ];
}
