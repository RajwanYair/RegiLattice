#nullable enable
using System.Collections.Generic;
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

// Application Restart & Crash Policy — controls Windows Error Reporting, automatic
// reboots after crashes, kernel dump settings, and AeDebug (just-in-time debugging).
// HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\AeDebug
// HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl
// HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting
internal static class ApplicationRestartPolicy
{
    private const string AeDebug = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\AeDebug";
    private const string CrashCtl = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl";
    private const string WerPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting";
    private const string WerMain = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "apprstrt-disable-aedebug-auto",
            Label = "App Restart: Disable Automatic JIT Debugger Attachment on Crash",
            Category = "Application Restart Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [AeDebug],
            Tags = ["crash", "aedebug", "jit-debug", "security", "policy"],
            Description =
                "Sets Auto=0 in AeDebug. Prevents the system from automatically attaching a "
                + "just-in-time debugger to crashing processes. Stops unauthorised debugger invocation. "
                + "Default: 1 (auto-attach). Disabling prevents debugger-based privilege escalation.",
            ApplyOps = [RegOp.SetString(AeDebug, "Auto", "0")],
            RemoveOps = [RegOp.DeleteValue(AeDebug, "Auto")],
            DetectOps = [RegOp.CheckString(AeDebug, "Auto", "0")],
        },
        new TweakDef
        {
            Id = "apprstrt-disable-auto-reboot-on-bugcheck",
            Label = "App Restart: Disable Auto-Reboot After BSOD (Allow Admin Review)",
            Category = "Application Restart Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [CrashCtl],
            Tags = ["crash", "bsod", "auto-reboot", "bugcheck", "security", "forensics"],
            Description =
                "Sets AutoReboot=0 in CrashControl. Stops Windows from automatically rebooting "
                + "after a kernel bug check (BSOD). The blue screen remains until manually rebooted. "
                + "Default: 1 (auto-reboot). Disabling allows forensic capture of the crash dump.",
            ApplyOps = [RegOp.SetDword(CrashCtl, "AutoReboot", 0)],
            RemoveOps = [RegOp.SetDword(CrashCtl, "AutoReboot", 1)],
            DetectOps = [RegOp.CheckDword(CrashCtl, "AutoReboot", 0)],
        },
        new TweakDef
        {
            Id = "apprstrt-enable-crash-dump-complete",
            Label = "App Restart: Enable Complete Memory Dump for Forensic Analysis",
            Category = "Application Restart Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [CrashCtl],
            Tags = ["crash", "dump", "memory", "forensics", "analysis", "security"],
            Description =
                "Sets CrashDumpEnabled=1 in CrashControl. Configures Windows to write a complete "
                + "physical memory dump to disk on bug check. "
                + "Value 1=Complete, 2=Kernel, 3=Small, 7=Automatic. Default: 7. Use 2 (Kernel) for balanced analysis.",
            ApplyOps = [RegOp.SetDword(CrashCtl, "CrashDumpEnabled", 2)],
            RemoveOps = [RegOp.SetDword(CrashCtl, "CrashDumpEnabled", 7)],
            DetectOps = [RegOp.CheckDword(CrashCtl, "CrashDumpEnabled", 2)],
        },
        new TweakDef
        {
            Id = "apprstrt-log-crash-event",
            Label = "App Restart: Log Kernel Crash to Windows Event Log",
            Category = "Application Restart Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [CrashCtl],
            Tags = ["crash", "event-log", "audit", "security", "forensics"],
            Description =
                "Sets LogEvent=1 in CrashControl. Writes an event to the System event log when "
                + "a kernel crash occurs. Enables SIEM alerting on system-level faults. "
                + "Default: 1 (already enabled). Explicit enforcement ensures SIEM visibility.",
            ApplyOps = [RegOp.SetDword(CrashCtl, "LogEvent", 1)],
            RemoveOps = [RegOp.DeleteValue(CrashCtl, "LogEvent")],
            DetectOps = [RegOp.CheckDword(CrashCtl, "LogEvent", 1)],
        },
        new TweakDef
        {
            Id = "apprstrt-disable-wer-reporting",
            Label = "App Restart: Disable Windows Error Reporting (WER) Telemetry",
            Category = "Application Restart Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [WerPolicy],
            Tags = ["wer", "error-reporting", "telemetry", "privacy", "policy"],
            Description =
                "Sets Disabled=1 in WER policy. Prevents crash reports, problem reports, and "
                + "application error data from being sent to Microsoft. "
                + "Default: 0 (enabled). Disabling stops potential crash dump data leaving the network.",
            ApplyOps = [RegOp.SetDword(WerPolicy, "Disabled", 1)],
            RemoveOps = [RegOp.DeleteValue(WerPolicy, "Disabled")],
            DetectOps = [RegOp.CheckDword(WerPolicy, "Disabled", 1)],
        },
        new TweakDef
        {
            Id = "apprstrt-disable-wer-queue",
            Label = "App Restart: Disable WER Problem Queue",
            Category = "Application Restart Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [WerPolicy],
            Tags = ["wer", "error-reporting", "queue", "privacy", "policy"],
            Description =
                "Sets DontSendAdditionalData=1 in WER policy. Prevents WER from queuing and "
                + "retrying transmission of error reports. Stops background upload attempts. "
                + "Default: 0. Recommended alongside WER Disabled policy.",
            ApplyOps = [RegOp.SetDword(WerPolicy, "DontSendAdditionalData", 1)],
            RemoveOps = [RegOp.DeleteValue(WerPolicy, "DontSendAdditionalData")],
            DetectOps = [RegOp.CheckDword(WerPolicy, "DontSendAdditionalData", 1)],
        },
        new TweakDef
        {
            Id = "apprstrt-bypass-data-throttling",
            Label = "App Restart: Disable WER Data Throttling",
            Category = "Application Restart Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [WerPolicy],
            Tags = ["wer", "error-reporting", "throttle", "privacy", "policy"],
            Description =
                "Sets BypassDataThrottling=1 in WER policy. Removes WER's bandwidth throttling "
                + "which controls rate of error data transmission. "
                + "Default: 0 (throttled). Used with WER Disabled to fully suppress crash telemetry.",
            ApplyOps = [RegOp.SetDword(WerPolicy, "BypassDataThrottling", 1)],
            RemoveOps = [RegOp.DeleteValue(WerPolicy, "BypassDataThrottling")],
            DetectOps = [RegOp.CheckDword(WerPolicy, "BypassDataThrottling", 1)],
        },
        new TweakDef
        {
            Id = "apprstrt-disable-wer-ui-consent",
            Label = "App Restart: Disable WER User Consent Prompts",
            Category = "Application Restart Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [WerPolicy],
            Tags = ["wer", "error-reporting", "consent", "ui", "privacy", "policy"],
            Description =
                "Sets DefaultConsent=1 in WER policy. Silently suppresses all WER user-consent "
                + "prompts ('Report this problem?'). No dialogs presented to users. "
                + "Default: 4 (prompt). Value 1=Always Ask is overridden; combine with Disabled=1.",
            ApplyOps = [RegOp.SetDword(WerMain, "DefaultConsent", 1)],
            RemoveOps = [RegOp.DeleteValue(WerMain, "DefaultConsent")],
            DetectOps = [RegOp.CheckDword(WerMain, "DefaultConsent", 1)],
        },
        new TweakDef
        {
            Id = "apprstrt-wer-minimum-dump-size",
            Label = "App Restart: Reduce WER Dump Log Retention to 1 Entry",
            Category = "Application Restart Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [WerMain],
            Tags = ["wer", "error-reporting", "dump", "retention", "privacy", "policy"],
            Description =
                "Sets MaxQueueSize=1 in WER. Limits the local WER crash queue to a single entry. "
                + "Reduces the volume of crash report files sitting in AppData waiting for upload. "
                + "Default: 50. Helps limit local crash data accumulation.",
            ApplyOps = [RegOp.SetDword(WerMain, "MaxQueueSize", 1)],
            RemoveOps = [RegOp.DeleteValue(WerMain, "MaxQueueSize")],
            DetectOps = [RegOp.CheckDword(WerMain, "MaxQueueSize", 1)],
        },
        new TweakDef
        {
            Id = "apprstrt-overwrite-existing-dump",
            Label = "App Restart: Overwrite Existing Crash Dump on New Crash",
            Category = "Application Restart Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [CrashCtl],
            Tags = ["crash", "dump", "overwrite", "disk-space", "forensics"],
            Description =
                "Sets Overwrite=1 in CrashControl. Overwrites the existing memory dump file instead "
                + "of generating a new timestamped file. Prevents disk space exhaustion from crash loops. "
                + "Default: 1 (overwrite). Explicit enforcement maintains expected disk usage.",
            ApplyOps = [RegOp.SetDword(CrashCtl, "Overwrite", 1)],
            RemoveOps = [RegOp.DeleteValue(CrashCtl, "Overwrite")],
            DetectOps = [RegOp.CheckDword(CrashCtl, "Overwrite", 1)],
        },
    ];
}
