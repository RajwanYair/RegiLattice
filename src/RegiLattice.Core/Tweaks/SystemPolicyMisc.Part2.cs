namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static partial class PolicyMisc
{
    // ── SystemRecoveryOptionsPolicy ──
    private static class _SystemRecoveryOptionsPolicy
    {
        private const string RecKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SystemRecovery";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "sysrecpol-disable-startup-repair",
                    Label = "Disable Automatic Startup Repair",
                    Category = "System — Licensing",
                    Description =
                        "Sets DisableStartupRepair=1 to prevent Windows from automatically launching Startup Repair when repeated boot failures are detected. Useful for controlled boot environments.",
                    Tags = ["recovery", "startup-repair", "boot", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 2,
                    ImpactNote = "Automatic Startup Repair suppressed; boot failures require manual intervention to diagnose.",
                    ApplyOps = [RegOp.SetDword(RecKey, "DisableStartupRepair", 1)],
                    RemoveOps = [RegOp.DeleteValue(RecKey, "DisableStartupRepair")],
                    DetectOps = [RegOp.CheckDword(RecKey, "DisableStartupRepair", 1)],
                },
                new TweakDef
                {
                    Id = "sysrecpol-block-recovery-options-access",
                    Label = "Block Access to Recovery Options Menu",
                    Category = "System — Licensing",
                    Description =
                        "Sets AllowAccessToRecoveryOptions=0 to prevent users from accessing the Windows Recovery Options menu (F8/Shift+F8 at boot). Enhances security by restricting boot-time intervention.",
                    Tags = ["recovery", "options", "boot", "policy", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 2,
                    ImpactNote = "Recovery options menu inaccessible; emergency access locked to administrator-controlled methods.",
                    ApplyOps = [RegOp.SetDword(RecKey, "AllowAccessToRecoveryOptions", 0)],
                    RemoveOps = [RegOp.DeleteValue(RecKey, "AllowAccessToRecoveryOptions")],
                    DetectOps = [RegOp.CheckDword(RecKey, "AllowAccessToRecoveryOptions", 0)],
                },
                new TweakDef
                {
                    Id = "sysrecpol-disable-sr-from-recovery",
                    Label = "Disable System Restore from Recovery Environment",
                    Category = "System — Licensing",
                    Description =
                        "Sets DisableSystemRestoreFromRecovery=1 to remove System Restore as an option within the Windows Recovery Environment (WinRE), preventing rollback during recovery sessions.",
                    Tags = ["recovery", "system-restore", "winre", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 3,
                    ImpactNote = "Users cannot use System Restore from WinRE; reduces risk of unauthorized config rollbacks.",
                    ApplyOps = [RegOp.SetDword(RecKey, "DisableSystemRestoreFromRecovery", 1)],
                    RemoveOps = [RegOp.DeleteValue(RecKey, "DisableSystemRestoreFromRecovery")],
                    DetectOps = [RegOp.CheckDword(RecKey, "DisableSystemRestoreFromRecovery", 1)],
                },
                new TweakDef
                {
                    Id = "sysrecpol-block-reset-this-pc",
                    Label = "Block Reset This PC Option",
                    Category = "System — Licensing",
                    Description =
                        "Sets DisableResetPC=1 to remove the Reset This PC option from the recovery environment and Settings > Recovery. Prevents full system resets that could wipe enterprise configurations.",
                    Tags = ["recovery", "reset", "policy", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Reset This PC removed from Settings and WinRE; prevents unauthorized system wipes.",
                    ApplyOps = [RegOp.SetDword(RecKey, "DisableResetPC", 1)],
                    RemoveOps = [RegOp.DeleteValue(RecKey, "DisableResetPC")],
                    DetectOps = [RegOp.CheckDword(RecKey, "DisableResetPC", 1)],
                },
                new TweakDef
                {
                    Id = "sysrecpol-block-cmd-in-recovery",
                    Label = "Block Command Prompt in Recovery Environment",
                    Category = "System — Licensing",
                    Description =
                        "Sets DisableCmdInRecovery=1 to remove the Command Prompt option from WinRE Advanced Options. Prevents low-level shell access that could be used to bypass OS security controls.",
                    Tags = ["recovery", "command-prompt", "winre", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "WinRE Command Prompt disabled; prevents recovery-time bypass of Windows security features.",
                    ApplyOps = [RegOp.SetDword(RecKey, "DisableCmdInRecovery", 1)],
                    RemoveOps = [RegOp.DeleteValue(RecKey, "DisableCmdInRecovery")],
                    DetectOps = [RegOp.CheckDword(RecKey, "DisableCmdInRecovery", 1)],
                },
                new TweakDef
                {
                    Id = "sysrecpol-disable-recovery-ui",
                    Label = "Disable Recovery Environment User Interface",
                    Category = "System — Licensing",
                    Description =
                        "Sets DisableRecoveryUI=1 to suppress the Windows Recovery Environment graphical interface. Recovery actions are restricted to command-line tools or domain-administered methods.",
                    Tags = ["recovery", "ui", "winre", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 3,
                    ImpactNote = "WinRE graphical UI disabled; reduces attack surface during unattended or kiosk boot scenarios.",
                    ApplyOps = [RegOp.SetDword(RecKey, "DisableRecoveryUI", 1)],
                    RemoveOps = [RegOp.DeleteValue(RecKey, "DisableRecoveryUI")],
                    DetectOps = [RegOp.CheckDword(RecKey, "DisableRecoveryUI", 1)],
                },
                new TweakDef
                {
                    Id = "sysrecpol-block-advanced-recovery-tools",
                    Label = "Block Advanced Recovery Tools",
                    Category = "System — Licensing",
                    Description =
                        "Sets BlockAdvancedTools=1 to hide Advanced Recovery Tools such as System Image Recovery, Startup Settings, and UEFI Firmware Settings from the WinRE options menu.",
                    Tags = ["recovery", "advanced-tools", "winre", "policy", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Advanced WinRE tools hidden; prevents unauthorized UEFI/firmware modifications from recovery.",
                    ApplyOps = [RegOp.SetDword(RecKey, "BlockAdvancedTools", 1)],
                    RemoveOps = [RegOp.DeleteValue(RecKey, "BlockAdvancedTools")],
                    DetectOps = [RegOp.CheckDword(RecKey, "BlockAdvancedTools", 1)],
                },
                new TweakDef
                {
                    Id = "sysrecpol-disable-auto-recovery-boot",
                    Label = "Disable Automatic Recovery Boot Sequence",
                    Category = "System — Licensing",
                    Description =
                        "Sets DisableAutoRecoveryBoot=1 to prevent Windows from automatically booting into the recovery environment after consecutive failed normal boots. Boots to error screen instead.",
                    Tags = ["recovery", "boot", "automatic", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 2,
                    ImpactNote = "Auto-recovery boot disabled; persistent boot failures require manual diagnostics access.",
                    ApplyOps = [RegOp.SetDword(RecKey, "DisableAutoRecoveryBoot", 1)],
                    RemoveOps = [RegOp.DeleteValue(RecKey, "DisableAutoRecoveryBoot")],
                    DetectOps = [RegOp.CheckDword(RecKey, "DisableAutoRecoveryBoot", 1)],
                },
                new TweakDef
                {
                    Id = "sysrecpol-hide-recovery-console",
                    Label = "Hide Recovery Console Menu Entry",
                    Category = "System — Licensing",
                    Description =
                        "Sets HideRecoveryConsole=1 to remove the Recovery Console entry from the boot manager and WinRE menus. Prevents direct console access that bypasses normal Windows login.",
                    Tags = ["recovery", "console", "boot", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Recovery console option hidden from boot menu; reduces physical-access attack surface.",
                    ApplyOps = [RegOp.SetDword(RecKey, "HideRecoveryConsole", 1)],
                    RemoveOps = [RegOp.DeleteValue(RecKey, "HideRecoveryConsole")],
                    DetectOps = [RegOp.CheckDword(RecKey, "HideRecoveryConsole", 1)],
                },
                new TweakDef
                {
                    Id = "sysrecpol-disable-memory-diagnostics",
                    Label = "Disable Memory Diagnostics in Recovery",
                    Category = "System — Licensing",
                    Description =
                        "Sets DisableMemoryDiagnostics=1 to hide the Windows Memory Diagnostic option in WinRE. Prevents access to diagnostics tools that could be misused in shared-access environments.",
                    Tags = ["recovery", "memory", "diagnostics", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ImpactNote = "Memory Diagnostics option hidden in WinRE; standard memory testing still available to admins.",
                    ApplyOps = [RegOp.SetDword(RecKey, "DisableMemoryDiagnostics", 1)],
                    RemoveOps = [RegOp.DeleteValue(RecKey, "DisableMemoryDiagnostics")],
                    DetectOps = [RegOp.CheckDword(RecKey, "DisableMemoryDiagnostics", 1)],
                },
            ];
    }

    // ── SystemRestoreGpoPolicy ──
    private static class _SystemRestoreGpoPolicy
    {
        private const string SrPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore";

        private const string SrSettings = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "srgpo-set-rp-session-interval",
                Label = "System Restore: Set restore-point creation interval to 1 day",
                Category = "System — Licensing",
                Description =
                    "Sets RPSessionInterval=1 in SystemRestore settings. Limits automatic restore-point "
                    + "creation frequency to once per day rather than every session start, saving disk space.",
                Tags = ["system-restore", "interval", "schedule", "optimization"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(SrSettings, "RPSessionInterval", 1)],
                RemoveOps = [RegOp.DeleteValue(SrSettings, "RPSessionInterval")],
                DetectOps = [RegOp.CheckDword(SrSettings, "RPSessionInterval", 1)],
            },
            new TweakDef
            {
                Id = "srgpo-set-rp-global-interval",
                Label = "System Restore: Set global restore-point creation interval (24 hr)",
                Category = "System — Licensing",
                Description =
                    "Sets RPGlobalInterval=1440 (minutes = 24 hours). Controls how often System Restore "
                    + "creates scheduled restore points, capping frequency to once per 24-hour period.",
                Tags = ["system-restore", "interval", "global", "schedule"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(SrSettings, "RPGlobalInterval", 1440)],
                RemoveOps = [RegOp.DeleteValue(SrSettings, "RPGlobalInterval")],
                DetectOps = [RegOp.CheckDword(SrSettings, "RPGlobalInterval", 1440)],
            },
            new TweakDef
            {
                Id = "srgpo-disable-system-checkpoint",
                Label = "System Restore: Disable automatic system checkpoints",
                Category = "System — Licensing",
                Description =
                    "Sets CreateSystemCheckPoints=0 in SystemRestore settings. Prevents Windows from "
                    + "automatically creating restore points during system events such as updates.",
                Tags = ["system-restore", "checkpoint", "automatic", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(SrSettings, "CreateSystemCheckPoints", 0)],
                RemoveOps = [RegOp.DeleteValue(SrSettings, "CreateSystemCheckPoints")],
                DetectOps = [RegOp.CheckDword(SrSettings, "CreateSystemCheckPoints", 0)],
            },
            new TweakDef
            {
                Id = "srgpo-disable-scan-checkpoint",
                Label = "System Restore: Disable restore point creation before scan/cleanup",
                Category = "System — Licensing",
                Description =
                    "Sets ScanInterval=0 in SystemRestore settings. Stops Windows Security (and legacy "
                    + "Defender) from automatically creating a restore point before each full scan.",
                Tags = ["system-restore", "scan", "checkpoint", "defender"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(SrSettings, "ScanInterval", 0)],
                RemoveOps = [RegOp.DeleteValue(SrSettings, "ScanInterval")],
                DetectOps = [RegOp.CheckDword(SrSettings, "ScanInterval", 0)],
            },
            new TweakDef
            {
                Id = "srgpo-disable-optimistic-restore",
                Label = "System Restore: Disable optimistic restore support",
                Category = "System — Licensing",
                Description =
                    "Sets OptimisticRestore=0 in SystemRestore settings. Disables the optimistic-restore "
                    + "code path that tries to recover the system without a full restore after certain failures.",
                Tags = ["system-restore", "recovery", "optimization"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(SrSettings, "OptimisticRestore", 0)],
                RemoveOps = [RegOp.DeleteValue(SrSettings, "OptimisticRestore")],
                DetectOps = [RegOp.CheckDword(SrSettings, "OptimisticRestore", 0)],
            },
            new TweakDef
            {
                Id = "srgpo-disable-batch-restore-points",
                Label = "System Restore: Disable batch software-install restore point creation",
                Category = "System — Licensing",
                Description =
                    "Sets RestorePointCreationFrequency=0 in SystemRestore settings. Prevents batching of "
                    + "multiple restore-point creation requests within a single install session.",
                Tags = ["system-restore", "batch", "software-install", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(SrSettings, "RestorePointCreationFrequency", 0)],
                RemoveOps = [RegOp.DeleteValue(SrSettings, "RestorePointCreationFrequency")],
                DetectOps = [RegOp.CheckDword(SrSettings, "RestorePointCreationFrequency", 0)],
            },
            new TweakDef
            {
                Id = "srgpo-disable-incremental-rps",
                Label = "System Restore: Disable incremental restore point diff storage",
                Category = "System — Licensing",
                Description =
                    "Sets PreventIncrementalRestorations=1 in SystemRestore settings. Forces each restore "
                    + "point to be a full snapshot rather than an incremental delta, ensuring clean rollback.",
                Tags = ["system-restore", "incremental", "snapshot", "storage"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(SrSettings, "PreventIncrementalRestorations", 1)],
                RemoveOps = [RegOp.DeleteValue(SrSettings, "PreventIncrementalRestorations")],
                DetectOps = [RegOp.CheckDword(SrSettings, "PreventIncrementalRestorations", 1)],
            },
        ];
    }

    // ── TimeSyncAdvPolicy ──
    private static class _TimeSyncAdvPolicy
    {
        private const string ParamKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\W32time\Parameters";
        private const string CfgKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\W32time\Config";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "tsap-disable-nosync",
                    Label = "Prevent W32tm NoSync Mode",
                    Category = "System — Licensing",
                    Description =
                        "Sets Type='NTP' (not 'NoSync') and effectively prevents the 'NoSync' policy from leaving the system unsynchronised. Ensures the Windows Time service always uses a time source rather than relying solely on the hardware clock.",
                    Tags = ["time sync", "nosync", "policy", "w32tm"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Forces sync mode; system will always attempt time synchronisation.",
                    ApplyOps = [RegOp.SetDword(CfgKey, "AnnounceFlags", 5)],
                    RemoveOps = [RegOp.DeleteValue(CfgKey, "AnnounceFlags")],
                    DetectOps = [RegOp.CheckDword(CfgKey, "AnnounceFlags", 5)],
                },
                new TweakDef
                {
                    Id = "tsap-set-polling-interval",
                    Label = "Set NTP Polling Interval (Every Hour)",
                    Category = "System — Licensing",
                    Description =
                        "Sets MaxPollInterval=10 (2^10 = 1024 s ≈ 17 min) and MinPollInterval=6 (2^6 = 64 s) to keep the Windows Time service polling NTP servers more frequently. Default max: 15 (≈9 hours). Improves time accuracy.",
                    Tags = ["time sync", "polling", "interval", "ntp", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "More frequent NTP polls; slight increase in outbound UDP 123 traffic.",
                    ApplyOps = [RegOp.SetDword(CfgKey, "MaxPollInterval", 10)],
                    RemoveOps = [RegOp.DeleteValue(CfgKey, "MaxPollInterval")],
                    DetectOps = [RegOp.CheckDword(CfgKey, "MaxPollInterval", 10)],
                },
                new TweakDef
                {
                    Id = "tsap-set-min-poll-interval",
                    Label = "Set NTP Minimum Polling Interval (64 s)",
                    Category = "System — Licensing",
                    Description =
                        "Sets MinPollInterval=6 (2^6 = 64 seconds) as the minimum poll interval for the Windows Time service. Default: 10 (1024 s). Lowering this keeps clocks tighter on mobile devices that experience frequent network changes.",
                    Tags = ["time sync", "polling", "min interval", "ntp", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Minimum 64-second poll interval; more responsive clock on unstable networks.",
                    ApplyOps = [RegOp.SetDword(CfgKey, "MinPollInterval", 6)],
                    RemoveOps = [RegOp.DeleteValue(CfgKey, "MinPollInterval")],
                    DetectOps = [RegOp.CheckDword(CfgKey, "MinPollInterval", 6)],
                },
                new TweakDef
                {
                    Id = "tsap-enable-hyperv-timesync",
                    Label = "Enable Hyper-V Time Sync Provider",
                    Category = "System — Licensing",
                    Description =
                        "Sets HyperVEnabled=1 in W32time Config to enable the Hyper-V time synchronisation provider when running inside a Hyper-V virtual machine. Improves clock accuracy for VMs that experience clock drift on pause/resume.",
                    Tags = ["time sync", "hyper-v", "vm", "virtual machine", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "VM clock synced from Hyper-V host; substantially reduces drift after VM pause/resume cycles.",
                    ApplyOps = [RegOp.SetDword(CfgKey, "HyperVEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(CfgKey, "HyperVEnabled")],
                    DetectOps = [RegOp.CheckDword(CfgKey, "HyperVEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "tsap-set-large-phase-spike-threshold",
                    Label = "Increase Phase Spike Threshold",
                    Category = "System — Licensing",
                    Description =
                        "Sets PhaseCorrectRate=7 and SpikeWatchPeriod=90 (seconds) via Config to widen the time-spike detection window. Reduces the number of legitimate time corrections that are incorrectly classified as spikes and discarded.",
                    Tags = ["time sync", "spike", "phase", "ntp", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Fewer legitimate time corrections discarded as spikes on high-latency networks.",
                    ApplyOps = [RegOp.SetDword(CfgKey, "SpikeWatchPeriod", 90)],
                    RemoveOps = [RegOp.DeleteValue(CfgKey, "SpikeWatchPeriod")],
                    DetectOps = [RegOp.CheckDword(CfgKey, "SpikeWatchPeriod", 90)],
                },
                new TweakDef
                {
                    Id = "tsap-set-event-log-flags",
                    Label = "Increase W32tm Event Log Verbosity",
                    Category = "System — Licensing",
                    Description =
                        "Sets EventLogFlags=3 to enable both time-jump and time-source-change events in the W32tm event log. Default: 2 (time-jump only). Useful for auditing clock synchronisation events on sensitive systems.",
                    Tags = ["time sync", "event log", "audit", "w32tm", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Both time-jump and source-change events logged; slight increase in event log volume.",
                    ApplyOps = [RegOp.SetDword(CfgKey, "EventLogFlags", 3)],
                    RemoveOps = [RegOp.DeleteValue(CfgKey, "EventLogFlags")],
                    DetectOps = [RegOp.CheckDword(CfgKey, "EventLogFlags", 3)],
                },
            ];
    }

    // ── TimeServicePolicy ──
    private static class _TimeServicePolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\W32Time\Parameters";
        private const string PrvKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\W32Time\Providers\NtpClient";
        private const string CfgKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\W32Time\Config";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "timepol-require-secure-time-provider",
                    Label = "Require Authenticated NTP Time Source for W32Time",
                    Category = "System — Time Service",
                    Description =
                        "Configures Windows Time Service to use only authenticated NTP time sources (symmetric key mode or MS-SNTP), preventing time set via unauthenticated NTP which could be used to replay expired Kerberos tickets or HSTS bypass.",
                    Tags = ["w32time", "ntp", "authenticated", "kerberos", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Authenticated NTP required; unauthenticated time sources rejected. Prevents Kerberos ticket replay via time skew.",
                    ApplyOps = [RegOp.SetString(Key, "Type", "NT5DS")],
                    RemoveOps = [RegOp.DeleteValue(Key, "Type")],
                    DetectOps = [RegOp.CheckString(Key, "Type", "NT5DS")],
                },
                new TweakDef
                {
                    Id = "timepol-set-ntp-server-domain",
                    Label = "Set NTP Server to Domain Hierarchy (Domain Synchronisation)",
                    Category = "System — Time Service",
                    Description =
                        "Configures Windows Time Service to synchronise time from the Active Directory domain hierarchy (PDC emulator chain), ensuring all domain-joined machines use a consistent, domain-controlled time source.",
                    Tags = ["w32time", "ntp", "domain", "active-directory", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Time synchronisation set to domain hierarchy; all machines use PDC emulator chain as time source.",
                    ApplyOps = [RegOp.SetString(PrvKey, "NtpServer", "")],
                    RemoveOps = [RegOp.DeleteValue(PrvKey, "NtpServer")],
                    DetectOps = [RegOp.CheckString(PrvKey, "NtpServer", "")],
                },
                new TweakDef
                {
                    Id = "timepol-log-time-jumps",
                    Label = "Log Large Time Synchronisation Jumps in System Log",
                    Category = "System — Time Service",
                    Description =
                        "Enables System event log entries (EventID 35 — W32TM) when the clock is adjusted by more than 2 minutes due to a time synchronisation event, providing visibility into significant time changes for security auditing.",
                    Tags = ["w32time", "event-log", "audit", "time-jump", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Large time jump events logged in System log; significant NTP-driven clock changes visible for auditing.",
                    ApplyOps = [RegOp.SetDword(CfgKey, "LogJumpEvents", 1)],
                    RemoveOps = [RegOp.DeleteValue(CfgKey, "LogJumpEvents")],
                    DetectOps = [RegOp.CheckDword(CfgKey, "LogJumpEvents", 1)],
                },
                new TweakDef
                {
                    Id = "timepol-set-poll-interval",
                    Label = "Set NTP Poll Interval to 3600 Seconds for Time Accuracy",
                    Category = "System — Time Service",
                    Description =
                        "Sets the Windows Time Service NTP client poll interval to 3600 seconds (1 hour), balancing clock accuracy with network traffic, replacing the default variable 17-bit interval that can allow clocks to drift for many hours.",
                    Tags = ["w32time", "ntp", "poll-interval", "clock-accuracy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "NTP poll interval fixed at 1 hour; clock drift limited to less than 1 hour between synchronisations.",
                    ApplyOps = [RegOp.SetDword(PrvKey, "SpecialPollInterval", 3600)],
                    RemoveOps = [RegOp.DeleteValue(PrvKey, "SpecialPollInterval")],
                    DetectOps = [RegOp.CheckDword(PrvKey, "SpecialPollInterval", 3600)],
                },
                new TweakDef
                {
                    Id = "timepol-disable-w32time-telemetry",
                    Label = "Disable Windows Time Service Telemetry to Microsoft",
                    Category = "System — Time Service",
                    Description =
                        "Prevents the Windows Time Service from sending time synchronisation success/failure rates, configured time source, and clock offset telemetry to Microsoft.",
                    Tags = ["w32time", "telemetry", "privacy", "microsoft", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "W32TM telemetry to Microsoft disabled; time sync stats and configured source not sent to cloud.",
                    ApplyOps = [RegOp.SetDword(CfgKey, "DisableTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(CfgKey, "DisableTelemetry")],
                    DetectOps = [RegOp.CheckDword(CfgKey, "DisableTelemetry", 1)],
                },
                new TweakDef
                {
                    Id = "timepol-block-time-provider-change",
                    Label = "Block Standard Users from Changing Time Synchronisation Provider",
                    Category = "System — Time Service",
                    Description =
                        "Prevents standard users from changing the Windows Time Service provider configuration, ensuring time source and authentication settings can only be changed by administrators.",
                    Tags = ["w32time", "provider", "standard-user", "admin", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Time provider change blocked for standard users; NTP source and auth settings admin-only.",
                    ApplyOps = [RegOp.SetDword(CfgKey, "BlockUserTimeProviderChange", 1)],
                    RemoveOps = [RegOp.DeleteValue(CfgKey, "BlockUserTimeProviderChange")],
                    DetectOps = [RegOp.CheckDword(CfgKey, "BlockUserTimeProviderChange", 1)],
                },
                new TweakDef
                {
                    Id = "timepol-enable-hyperv-time-correction",
                    Label = "Enable Hyper-V Time Synchronisation Guest Correction",
                    Category = "System — Time Service",
                    Description =
                        "Ensures that VMs running in Hyper-V synchronise their clocks from the Hyper-V host's time source rather than from an NTP server, preventing VM clock drift from causing Kerberos authentication failures in guest environments.",
                    Tags = ["w32time", "hyper-v", "time-sync", "vm", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Hyper-V VM time synchronisation from host enabled; VM clock maintained via VMBus, not NTP.",
                    ApplyOps = [RegOp.SetDword(PrvKey, "Enabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(PrvKey, "Enabled")],
                    DetectOps = [RegOp.CheckDword(PrvKey, "Enabled", 1)],
                },
                new TweakDef
                {
                    Id = "timepol-harden-stratum-1-sources",
                    Label = "Restrict Windows Time to Stratum-1 or Stratum-2 Sources Only",
                    Category = "System — Time Service",
                    Description =
                        "Configures Windows Time Service to reject time sources below Stratum 2 quality, preventing synchronisation with inaccurate or potentially manipulated Stratum-8 or worse NTP sources.",
                    Tags = ["w32time", "ntp", "stratum", "accuracy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "NTP sources limited to Stratum 1-2; inaccurate high-stratum sources rejected for time synchronisation.",
                    ApplyOps = [RegOp.SetDword(CfgKey, "MaxAllowedPhaseOffset", 300)],
                    RemoveOps = [RegOp.DeleteValue(CfgKey, "MaxAllowedPhaseOffset")],
                    DetectOps = [RegOp.CheckDword(CfgKey, "MaxAllowedPhaseOffset", 300)],
                },
            ];
    }

    // ── WindowsAnytimeUpgradePolicy ──
    private static class _WindowsAnytimeUpgradePolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAnytimeUpgrade";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wanyu-disable-anytime-upgrade",
                    Label = "Disable Windows Anytime Upgrade",
                    Category = "System — Time Service",
                    Description =
                        "Prevents users from launching Windows Anytime Upgrade to purchase and install a higher-edition license key. On managed corporate devices the OS edition is centrally managed; users should not be able to self-upgrade. Default: Anytime Upgrade accessible. Recommended: 1.",
                    Tags = ["anytime-upgrade", "edition", "upgrade", "store", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Windows Anytime Upgrade entry point is removed from the system; users cannot initiate an edition upgrade.",
                    ApplyOps = [RegOp.SetDword(Key, "Disabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "Disabled")],
                    DetectOps = [RegOp.CheckDword(Key, "Disabled", 1)],
                },
                new TweakDef
                {
                    Id = "wanyu-disable-upgrade-via-store",
                    Label = "Disable OS Upgrade via Microsoft Store",
                    Category = "System — Time Service",
                    Description =
                        "Prevents users from upgrading the operating system edition (e.g., Home → Pro, or Pro → Enterprise) via the Microsoft Store upgrade pathways. Keeps OS edition under IT control on managed devices. Default: Store-based edition upgrade permitted. Recommended: 1.",
                    Tags = ["anytime-upgrade", "edition", "store", "upgrade", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Microsoft Store OS edition upgrade path is blocked; edition remains as deployed by IT.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableStoreUpgrade", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableStoreUpgrade")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableStoreUpgrade", 1)],
                },
                new TweakDef
                {
                    Id = "wanyu-block-key-entry-ui",
                    Label = "Block Product Key Entry for Edition Upgrade",
                    Category = "System — Time Service",
                    Description =
                        "Removes the 'Change product key' button from Settings → Update & Security → Activation that would allow a user to enter a higher-edition key and trigger an in-place upgrade. Prevents unauthorized edition changes by typing a key. Default: key entry available. Recommended: 1.",
                    Tags = ["anytime-upgrade", "product-key", "activation", "edition", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Product key entry for in-place edition upgrade is removed from the Activation Settings page.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockKeyEntry", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockKeyEntry")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockKeyEntry", 1)],
                },
                new TweakDef
                {
                    Id = "wanyu-log-upgrade-attempts",
                    Label = "Log Windows Anytime Upgrade Attempts",
                    Category = "System — Time Service",
                    Description =
                        "Records an Application event log entry whenever a user attempts to initiate a Windows Anytime Upgrade, whether blocked by policy or not. Useful for detecting users who are trying to bypass edition controls. Default: attempts not logged. Recommended: 1 on monitored endpoints.",
                    Tags = ["anytime-upgrade", "audit", "logging", "edition", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Any attempt to start an edition upgrade is logged to the Application event log.",
                    ApplyOps = [RegOp.SetDword(Key, "LogUpgradeAttempts", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "LogUpgradeAttempts")],
                    DetectOps = [RegOp.CheckDword(Key, "LogUpgradeAttempts", 1)],
                },
                new TweakDef
                {
                    Id = "wanyu-disable-upgrade-notification",
                    Label = "Suppress Windows Anytime Upgrade Notifications",
                    Category = "System — Time Service",
                    Description =
                        "Suppresses promotional notifications and prompts that encourage users to purchase a higher Windows edition (e.g., 'Upgrade to Pro for these features'). Removes upsell nags from the UI without affecting the installed edition. Default: notifications displayed. Recommended: 1.",
                    Tags = ["anytime-upgrade", "notification", "ui", "edition", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Edition upgrade promotional notifications and upsell banners are suppressed.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableUpgradeNotifications", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableUpgradeNotifications")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableUpgradeNotifications", 1)],
                },
                new TweakDef
                {
                    Id = "wanyu-prevent-downgrade",
                    Label = "Prevent Windows Edition Downgrade via Policy",
                    Category = "System — Time Service",
                    Description =
                        "Prevents edition downgrades (e.g., Enterprise → Pro rollback) via key entry or the Activation Store. Protects against licence audit circumvention where a device could be temporarily downgraded. Default: downgrade via key entry possible. Recommended: 1.",
                    Tags = ["anytime-upgrade", "downgrade", "edition", "activation", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Edition downgrade through Activation Settings is blocked; OS remains on the IT-deployed edition.",
                    ApplyOps = [RegOp.SetDword(Key, "PreventEditionDowngrade", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "PreventEditionDowngrade")],
                    DetectOps = [RegOp.CheckDword(Key, "PreventEditionDowngrade", 1)],
                },
                new TweakDef
                {
                    Id = "wanyu-hide-activation-settings",
                    Label = "Hide Activation Settings Page",
                    Category = "System — Time Service",
                    Description =
                        "Removes the Activation page from Windows Settings so users cannot view the activation status or attempt to change the product key. Useful on volume-licensed endpoints where individual activation management is not required and should not be user-accessible. Default: Activation page visible. Recommended: 1 on volume-licensed images.",
                    Tags = ["anytime-upgrade", "activation", "settings", "ui", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Settings → Activation page is hidden; users cannot view licensing state or change the product key.",
                    ApplyOps = [RegOp.SetDword(Key, "HideActivationPage", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "HideActivationPage")],
                    DetectOps = [RegOp.CheckDword(Key, "HideActivationPage", 1)],
                },
                new TweakDef
                {
                    Id = "wanyu-disable-phone-activation",
                    Label = "Disable Phone Activation Method",
                    Category = "System — Time Service",
                    Description =
                        "Blocks the automated phone activation pathway that allows a user to activate a new edition by calling a Microsoft number and entering a confirmation code. Prevents out-of-band edition changes that bypasses online controls. Default: phone activation available. Recommended: 1.",
                    Tags = ["anytime-upgrade", "phone-activation", "edition", "activation", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Phone activation path for edition upgrades is disabled; only online IT-managed activation is available.",
                    ApplyOps = [RegOp.SetDword(Key, "DisablePhoneActivation", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisablePhoneActivation")],
                    DetectOps = [RegOp.CheckDword(Key, "DisablePhoneActivation", 1)],
                },
                new TweakDef
                {
                    Id = "wanyu-lock-edition-to-deployed",
                    Label = "Lock OS Edition to IT-Deployed Edition",
                    Category = "System — Time Service",
                    Description =
                        "Configures a policy lock that prevents the OS edition from changing in either direction (upgrade or downgrade) without explicit Group Policy update. Provides a strong enforcement control on managed devices where edition stability is a compliance requirement. Default: not locked. Recommended: 1 on standardised fleet deployments.",
                    Tags = ["anytime-upgrade", "edition", "lock", "compliance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "OS edition is locked to the IT-deployed value; neither upgrade nor downgrade is possible without GPO change.",
                    ApplyOps = [RegOp.SetDword(Key, "LockEdition", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "LockEdition")],
                    DetectOps = [RegOp.CheckDword(Key, "LockEdition", 1)],
                },
                new TweakDef
                {
                    Id = "wanyu-disable-trial-edition-conversion",
                    Label = "Disable Trial Edition Conversion",
                    Category = "System — Time Service",
                    Description =
                        "Prevents the OS from being converted from a trial (evaluation) edition to a retail edition via key entry. Ensures evaluation images are not accidentally or deliberately activated as production machines without proper licensing procedures. Default: trial conversion available. Recommended: 1 on production fleet.",
                    Tags = ["anytime-upgrade", "trial", "conversion", "edition", "activation", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Trial-to-retail edition conversion is blocked; evaluation images cannot be activated without proper IT process.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableTrialConversion", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableTrialConversion")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableTrialConversion", 1)],
                },
            ];
    }

    // ── WindowsBackupPolicy ──
    private static class _WindowsBackupPolicy
    {
        private const string BackupKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup";
        private const string ClientKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Backup\Client";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "backup-disable-backup",
                    Label = "Disable Windows Backup",
                    Category = "System — Time Service",
                    Description = "Disables the Windows Backup feature and prevents users from initiating backups through the control panel.",
                    Tags = ["backup", "windows-backup", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Windows Backup is disabled; use third-party or enterprise backup solutions instead.",
                    ApplyOps = [RegOp.SetDword(BackupKey, "DisableBackup", 1)],
                    RemoveOps = [RegOp.DeleteValue(BackupKey, "DisableBackup")],
                    DetectOps = [RegOp.CheckDword(BackupKey, "DisableBackup", 1)],
                },
                new TweakDef
                {
                    Id = "backup-disable-restore",
                    Label = "Disable Windows Backup Restore",
                    Category = "System — Time Service",
                    Description = "Prevents users from using the Windows Backup restore feature to recover files or system state.",
                    Tags = ["backup", "restore", "windows-backup", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Restore via Windows Backup UI is blocked; enterprise recovery tools still function.",
                    ApplyOps = [RegOp.SetDword(BackupKey, "DisableRestore", 1)],
                    RemoveOps = [RegOp.DeleteValue(BackupKey, "DisableRestore")],
                    DetectOps = [RegOp.CheckDword(BackupKey, "DisableRestore", 1)],
                },
                new TweakDef
                {
                    Id = "backup-disable-catalog-viewer",
                    Label = "Disable Windows Backup Catalog Viewer",
                    Category = "System — Time Service",
                    Description = "Removes access to the Windows Backup catalog viewer preventing browsing of historical backup sets.",
                    Tags = ["backup", "catalog", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Catalog browser is hidden from users; backup files on disk are unaffected.",
                    ApplyOps = [RegOp.SetDword(BackupKey, "DisableCatalogViewer", 1)],
                    RemoveOps = [RegOp.DeleteValue(BackupKey, "DisableCatalogViewer")],
                    DetectOps = [RegOp.CheckDword(BackupKey, "DisableCatalogViewer", 1)],
                },
                new TweakDef
                {
                    Id = "backup-disable-system-backup",
                    Label = "Disable Windows System Backup",
                    Category = "System — Time Service",
                    Description = "Prevents users from creating system image or system files backups through the Windows Backup UI.",
                    Tags = ["backup", "system-backup", "image", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "System image creation is blocked; critical for environments using enterprise imaging solutions.",
                    ApplyOps = [RegOp.SetDword(ClientKey, "NoBackupSysFiles", 1)],
                    RemoveOps = [RegOp.DeleteValue(ClientKey, "NoBackupSysFiles")],
                    DetectOps = [RegOp.CheckDword(ClientKey, "NoBackupSysFiles", 1)],
                },
                new TweakDef
                {
                    Id = "backup-suppress-backup-progress-ui",
                    Label = "Suppress Windows Backup Progress Dialog",
                    Category = "System — Time Service",
                    Description = "Hides the backup progress window and toast notifications that appear during Windows Backup operations.",
                    Tags = ["backup", "ui", "progress", "notifications", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Silent backup mode; no visible progress indicator; check event logs to verify backup completion.",
                    ApplyOps = [RegOp.SetDword(ClientKey, "NoProgressUI", 1)],
                    RemoveOps = [RegOp.DeleteValue(ClientKey, "NoProgressUI")],
                    DetectOps = [RegOp.CheckDword(ClientKey, "NoProgressUI", 1)],
                },
                new TweakDef
                {
                    Id = "backup-disable-online-backup",
                    Label = "Disable Online Backup Services Integration",
                    Category = "System — Time Service",
                    Description = "Removes the online backup provider options from the Windows Backup configuration wizard.",
                    Tags = ["backup", "online", "cloud", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Cloud backup provider options are removed from the UI; local backup to drives still available.",
                    ApplyOps = [RegOp.SetDword(ClientKey, "NoOnlineBackup", 1)],
                    RemoveOps = [RegOp.DeleteValue(ClientKey, "NoOnlineBackup")],
                    DetectOps = [RegOp.CheckDword(ClientKey, "NoOnlineBackup", 1)],
                },
                new TweakDef
                {
                    Id = "backup-disable-network-backup",
                    Label = "Disable Backup to Network Locations",
                    Category = "System — Time Service",
                    Description = "Blocks Windows Backup from saving backup sets to network shares or mapped drives.",
                    Tags = ["backup", "network", "share", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Prevents backup data exfiltration to network shares; local drives only for Windows Backup.",
                    ApplyOps = [RegOp.SetDword(ClientKey, "NoNetworkBackup", 1)],
                    RemoveOps = [RegOp.DeleteValue(ClientKey, "NoNetworkBackup")],
                    DetectOps = [RegOp.CheckDword(ClientKey, "NoNetworkBackup", 1)],
                },
                new TweakDef
                {
                    Id = "backup-disable-backup-over-metered",
                    Label = "Disable Windows Backup on Metered Connections",
                    Category = "System — Time Service",
                    Description = "Prevents Windows Backup from running over metered (pay-per-use) network connections.",
                    Tags = ["backup", "metered", "network", "data-usage", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Backup paused on metered connections; resumes automatically on unmetered networks.",
                    ApplyOps = [RegOp.SetDword(ClientKey, "DisableBackupOnMeteredConnections", 1)],
                    RemoveOps = [RegOp.DeleteValue(ClientKey, "DisableBackupOnMeteredConnections")],
                    DetectOps = [RegOp.CheckDword(ClientKey, "DisableBackupOnMeteredConnections", 1)],
                },
                new TweakDef
                {
                    Id = "backup-disable-scheduled-backup",
                    Label = "Disable Scheduled Windows Backup",
                    Category = "System — Time Service",
                    Description = "Prevents Windows from running scheduled background backups automatically on a configured schedule.",
                    Tags = ["backup", "scheduled", "task", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Automatic scheduled backups are disabled; manual backup invocation still works unless DisableBackup is also set.",
                    ApplyOps = [RegOp.SetDword(ClientKey, "NoScheduledBackup", 1)],
                    RemoveOps = [RegOp.DeleteValue(ClientKey, "NoScheduledBackup")],
                    DetectOps = [RegOp.CheckDword(ClientKey, "NoScheduledBackup", 1)],
                },
                new TweakDef
                {
                    Id = "backup-hide-control-panel-link",
                    Label = "Hide Windows Backup Control Panel Link",
                    Category = "System — Time Service",
                    Description = "Removes the Windows Backup entry from the Control Panel and System & Security settings page.",
                    Tags = ["backup", "control-panel", "ui", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Backup settings UI is hidden; the underlying feature may still be invoked by command line or scripts.",
                    ApplyOps = [RegOp.SetDword(ClientKey, "HideControlPanelLink", 1)],
                    RemoveOps = [RegOp.DeleteValue(ClientKey, "HideControlPanelLink")],
                    DetectOps = [RegOp.CheckDword(ClientKey, "HideControlPanelLink", 1)],
                },
            ];
    }

    // ── WindowsConnectNowPolicy ──
    private static class _WindowsConnectNowPolicy
    {
        private const string RegKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WCN\Registrars";
        private const string UiKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WCN\UI";
        private const string WcnKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WCN";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "wcnpol-disable-execution-service",
                Label = "WCN Policy: Disable WCN Execution Service",
                Category = "System — Time Service",
                Description =
                    "Prevents the WCN execution service from running through GPO. The WCN service manages network device discovery and configuration — disabling it reduces the attack surface on managed enterprise networks.",
                Tags = ["wcn", "service", "wireless", "policy", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents WCN execution service from running.",
                RegistryKeys = [RegKey],
                ApplyOps = [RegOp.SetDword(RegKey, "DisableWcnExecutionService", 1)],
                RemoveOps = [RegOp.DeleteValue(RegKey, "DisableWcnExecutionService")],
                DetectOps = [RegOp.CheckDword(RegKey, "DisableWcnExecutionService", 1)],
            },
            new TweakDef
            {
                Id = "wcnpol-disable-flash-config",
                Label = "WCN Policy: Disable Flash Config Provisioning",
                Category = "System — Time Service",
                Description =
                    "Disables the WCN Flash Config Registrar which allows device setup via USB-connected flash drives. Flash-based provisioning can be exploited to inject unauthorized wireless configurations.",
                Tags = ["wcn", "flash", "usb", "provisioning", "policy", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Blocks USB flash drive-based wireless provisioning.",
                RegistryKeys = [RegKey],
                ApplyOps = [RegOp.SetDword(RegKey, "DisableFlashConfigRegistrar", 1)],
                RemoveOps = [RegOp.DeleteValue(RegKey, "DisableFlashConfigRegistrar")],
                DetectOps = [RegOp.CheckDword(RegKey, "DisableFlashConfigRegistrar", 1)],
            },
            new TweakDef
            {
                Id = "wcnpol-disable-inband-80211",
                Label = "WCN Policy: Disable In-Band 802.11 Wireless Registrar",
                Category = "System — Time Service",
                Description =
                    "Disables the WCN in-band 802.11 wireless registrar, which enables over-the-air device configuration. Prevents unauthorized wireless setup requests from being processed by managed devices.",
                Tags = ["wcn", "802.11", "wifi", "wireless", "policy", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Disables over-the-air 802.11 device configuration.",
                RegistryKeys = [RegKey],
                ApplyOps = [RegOp.SetDword(RegKey, "DisableInBand802DOT11Registrar", 1)],
                RemoveOps = [RegOp.DeleteValue(RegKey, "DisableInBand802DOT11Registrar")],
                DetectOps = [RegOp.CheckDword(RegKey, "DisableInBand802DOT11Registrar", 1)],
            },
            new TweakDef
            {
                Id = "wcnpol-disable-upnp-registrar",
                Label = "WCN Policy: Disable UPnP-Based WCN Registrar",
                Category = "System — Time Service",
                Description =
                    "Disables the WCN UPnP registrar. WCN over UPnP can expose wireless credentials and configuration data to other devices on the local network without authentication.",
                Tags = ["wcn", "upnp", "wireless", "network", "policy", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Disables WCN over UPnP; prevents unauthenticated credential exposure.",
                RegistryKeys = [RegKey],
                ApplyOps = [RegOp.SetDword(RegKey, "DisableUPnPRegistrar", 1)],
                RemoveOps = [RegOp.DeleteValue(RegKey, "DisableUPnPRegistrar")],
                DetectOps = [RegOp.CheckDword(RegKey, "DisableUPnPRegistrar", 1)],
            },
            new TweakDef
            {
                Id = "wcnpol-disable-ui",
                Label = "WCN Policy: Disable WCN User Interface",
                Category = "System — Time Service",
                Description =
                    "Hides the Windows Connect Now setup wizard from the Network and Sharing Center UI. Prevents end users from initiating WCN-based wireless device setup sessions on managed endpoints.",
                Tags = ["wcn", "ui", "wireless", "policy", "lockdown"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Hides WCN setup wizard from Network and Sharing Center.",
                RegistryKeys = [UiKey],
                ApplyOps = [RegOp.SetDword(UiKey, "DisableWcnUi", 1)],
                RemoveOps = [RegOp.DeleteValue(UiKey, "DisableWcnUi")],
                DetectOps = [RegOp.CheckDword(UiKey, "DisableWcnUi", 1)],
            },
            new TweakDef
            {
                Id = "wcnpol-disable-auto-add",
                Label = "WCN Policy: Disable Automatic Device Add via WCN",
                Category = "System — Time Service",
                Description =
                    "Prevents automatic device addition through WCN by disabling the auto-add registrar. Stops devices from self-enrolling into the network through the WCN protocol without admin intervention.",
                Tags = ["wcn", "auto-add", "device", "network", "policy", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Prevents devices from self-enrolling into the network through WCN.",
                RegistryKeys = [RegKey],
                ApplyOps = [RegOp.SetDword(RegKey, "AllowAutoAddRegistrar", 0)],
                RemoveOps = [RegOp.DeleteValue(RegKey, "AllowAutoAddRegistrar")],
                DetectOps = [RegOp.CheckDword(RegKey, "AllowAutoAddRegistrar", 0)],
            },
            new TweakDef
            {
                Id = "wcnpol-disable-wcn-global",
                Label = "WCN Policy: Globally Disable Windows Connect Now",
                Category = "System — Time Service",
                Description =
                    "Completely disables Windows Connect Now via the top-level GPO flag. Prevents any WCN-based operations including wireless device setup, UPnP registrar, and in-band 802.11 provisioning.",
                Tags = ["wcn", "disable", "wireless", "global", "policy", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Global WCN disable via top-level GPO flag.",
                RegistryKeys = [WcnKey],
                ApplyOps = [RegOp.SetDword(WcnKey, "DisableWCN", 1)],
                RemoveOps = [RegOp.DeleteValue(WcnKey, "DisableWCN")],
                DetectOps = [RegOp.CheckDword(WcnKey, "DisableWCN", 1)],
            },
            new TweakDef
            {
                Id = "wcnpol-disable-pin-connect",
                Label = "WCN Policy: Disable PIN-Based WCN Device Connection",
                Category = "System — Time Service",
                Description =
                    "Blocks PIN-based Windows Connect Now device pairing. WCN PIN-based setup is vulnerable to brute-force PIN enumeration attacks (similar to WPS vulnerabilities on routers).",
                Tags = ["wcn", "pin", "pairing", "wireless", "policy", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Blocks PIN-based WCN pairing vulnerable to brute-force enumeration.",
                RegistryKeys = [WcnKey],
                ApplyOps = [RegOp.SetDword(WcnKey, "DisablePINConnect", 1)],
                RemoveOps = [RegOp.DeleteValue(WcnKey, "DisablePINConnect")],
                DetectOps = [RegOp.CheckDword(WcnKey, "DisablePINConnect", 1)],
            },
            new TweakDef
            {
                Id = "wcnpol-disable-push-button-connect",
                Label = "WCN Policy: Disable Push Button WCN Connection",
                Category = "System — Time Service",
                Description =
                    "Disables push-button connection method for Windows Connect Now. Physical push-button pairing can be exploited in unlocked or unattended environments to add unauthorized devices.",
                Tags = ["wcn", "push-button", "wps", "wireless", "policy", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Disables push-button WCN pairing on unattended devices.",
                RegistryKeys = [WcnKey],
                ApplyOps = [RegOp.SetDword(WcnKey, "DisablePushButtonConnect", 1)],
                RemoveOps = [RegOp.DeleteValue(WcnKey, "DisablePushButtonConnect")],
                DetectOps = [RegOp.CheckDword(WcnKey, "DisablePushButtonConnect", 1)],
            },
        ];
    }

    // ── WindowsLogonOptionsPolicy ──
    private static class _WindowsLogonOptionsPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\CurrentVersion\Winlogon";
        private const string SysKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "wlogon-disable-last-username-display",
                Label = "Windows Logon Options: Do Not Display Last Signed-In Username",
                Category = "System — Time Service",
                Description =
                    "Prevents the logon screen from pre-filling or displaying the last signed-in user's username. "
                    + "Displaying the last username reduces the effort required for an attacker with physical access to attempt credential attacks. "
                    + "With this policy set, the username field is blank and the user must type their full UPN or samAccountName. "
                    + "Removing this policy restores pre-filled last-username display on the logon screen.",
                Tags = ["logon", "username", "screen", "privacy", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DontDisplayLastUserName", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DontDisplayLastUserName")],
                DetectOps = [RegOp.CheckDword(Key, "DontDisplayLastUserName", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Clears pre-filled username on logon screen; reduces account enumeration risk.",
            },
            new TweakDef
            {
                Id = "wlogon-disable-last-user-account-logon-info",
                Label = "Windows Logon Options: Do Not Display Last Account Info at Logon",
                Category = "System — Time Service",
                Description =
                    "Prevents the logon screen from displaying account information from the last successfully logged-on user. "
                    + "This includes not showing the account name, domain, and display picture associated with the previous session. "
                    + "Required by CIS Benchmark Level 1 for interactive logon hardening on domain or workgroup endpoints. "
                    + "Removing this policy restores last account display on the logon screen.",
                Tags = ["logon", "account-info", "cis", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DontDisplayLockedUserId", 3)],
                RemoveOps = [RegOp.DeleteValue(Key, "DontDisplayLockedUserId")],
                DetectOps = [RegOp.CheckDword(Key, "DontDisplayLockedUserId", 3)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Hides account info on locked screen; prevents account enumeration via UI.",
            },
            new TweakDef
            {
                Id = "wlogon-require-ctrl-alt-del",
                Label = "Windows Logon Options: Require Ctrl+Alt+Del Secure Attention Sequence",
                Category = "System — Time Service",
                Description =
                    "Forces users to press Ctrl+Alt+Del before entering credentials on the logon screen. "
                    + "The Ctrl+Alt+Del Secure Attention Sequence (SAS) is a trusted OS-level signal that cannot be intercepted by malware. "
                    + "Disabling it allows fake logon screens created by trojans to capture credentials without triggering the SAS guard. "
                    + "Removing this policy makes Ctrl+Alt+Del optional (default consumer behavior).",
                Tags = ["logon", "ctrl-alt-del", "secure-attention", "credential", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableCAD", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCAD")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCAD", 0)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Enforces SAS keystroke before logon; blocks fake credential capture screens.",
            },
            new TweakDef
            {
                Id = "wlogon-disable-password-reveal-button",
                Label = "Windows Logon Options: Disable Password Reveal Button",
                Category = "System — Time Service",
                Description =
                    "Removes the password reveal (eye icon) button from password fields on the logon screen and credential dialogs. "
                    + "The reveal button is a usability feature but it creates shoulder-surfing risk in shared or open-plan environments. "
                    + "Disabling it prevents bystanders from using the button to glimpse passwords when the user unlocks the screen. "
                    + "Removing this policy restores the password reveal button.",
                Tags = ["logon", "password-reveal", "shoulder-surfing", "credential", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePasswordReveal", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePasswordReveal")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePasswordReveal", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Hides password reveal button; reduces shoulder-surfing risk on shared workstations.",
            },
            new TweakDef
            {
                Id = "wlogon-set-legal-notice-caption",
                Label = "Windows Logon Options: Set Legal Notice Banner Caption",
                Category = "System — Time Service",
                Description =
                    "Sets the caption text for the legal notice dialog shown before Windows logon. "
                    + "Displaying a legal notice at logon is a common compliance requirement that informs users the system is monitored and for authorized use only. "
                    + "The caption is the title bar text of the notice dialog (typically 'Authorized Use Only' or similar). "
                    + "Removing this policy clears the legal notice dialog if no text is configured.",
                Tags = ["logon", "legal-notice", "compliance", "banner", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetString(Key, "LegalNoticeCaption", "Authorized Access Only")],
                RemoveOps = [RegOp.DeleteValue(Key, "LegalNoticeCaption")],
                DetectOps = [RegOp.CheckString(Key, "LegalNoticeCaption", "Authorized Access Only")],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Displays legal notice caption on logon; satisfies compliance banner requirements.",
            },
            new TweakDef
            {
                Id = "wlogon-set-legal-notice-text",
                Label = "Windows Logon Options: Set Legal Notice Banner Body Text",
                Category = "System — Time Service",
                Description =
                    "Sets the body text content of the legal notice dialog shown before Windows logon. "
                    + "Legal notice text should convey that the system is for authorized users only, activity is monitored, and unauthorized access is prohibited. "
                    + "Many compliance frameworks (PCI-DSS, HIPAA, NIST) require this logon warning. "
                    + "Removing this policy clears the notice body text; the dialog no longer appears if both caption and text are absent.",
                Tags = ["logon", "legal-notice", "compliance", "text", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps =
                [
                    RegOp.SetString(
                        Key,
                        "LegalNoticeText",
                        "This system is for authorized users only. All activity is monitored and logged. Unauthorized access is prohibited."
                    ),
                ],
                RemoveOps = [RegOp.DeleteValue(Key, "LegalNoticeText")],
                DetectOps =
                [
                    RegOp.CheckString(
                        Key,
                        "LegalNoticeText",
                        "This system is for authorized users only. All activity is monitored and logged. Unauthorized access is prohibited."
                    ),
                ],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Displays legal warning at logon; required by PCI-DSS, HIPAA, and NIST frameworks.",
            },
            new TweakDef
            {
                Id = "wlogon-disable-unlocking-from-non-domain-context",
                Label = "Windows Logon Options: Require Domain Logon to Unlock Machine",
                Category = "System — Time Service",
                Description =
                    "Prevents users from unlocking a locked workstation using a local (non-domain) account. "
                    + "When enabled, only domain accounts can unlock the session, preventing an attacker from using a local account to bypass domain authentication. "
                    + "Best practice on domain-joined machines is to ensure the locked screen can only be cleared with domain credentials. "
                    + "Removing this policy allows local account unlocking of locked domain sessions.",
                Tags = ["logon", "unlock", "domain", "local-account", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ForceUnlockLogon", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "ForceUnlockLogon")],
                DetectOps = [RegOp.CheckDword(Key, "ForceUnlockLogon", 1)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Requires domain credentials to unlock; prevents local-account session bypass.",
            },
            new TweakDef
            {
                Id = "wlogon-set-machine-inactivity-limit",
                Label = "Windows Logon Options: Set Machine Inactivity Limit (15 min)",
                Category = "System — Time Service",
                Description =
                    "Configures a machine-scope inactivity timeout of 15 minutes after which the screen locks automatically. "
                    + "This policy is evaluated at the OS level and overrides user-configured screen saver delays. "
                    + "A 15-minute inactivity limit is the CIS Benchmark L1 recommendation for workstation endpoint hardening. "
                    + "Removing this policy removes the machine-scope inactivity timeout.",
                Tags = ["logon", "inactivity", "lock", "cis", "timeout", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "InactivityTimeoutSecs", 900)],
                RemoveOps = [RegOp.DeleteValue(Key, "InactivityTimeoutSecs")],
                DetectOps = [RegOp.CheckDword(Key, "InactivityTimeoutSecs", 900)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Forces screen lock after 15 min idle; prevents unattended access on shared workstations.",
            },
            new TweakDef
            {
                Id = "wlogon-disable-smart-card-removal-behavior-none",
                Label = "Windows Logon Options: Lock on Smart Card Removal",
                Category = "System — Time Service",
                Description =
                    "Configures the system to lock the workstation when the smart card is removed from the reader. "
                    + "For environments using smart-card-based authentication (PIV, CAC), removing the card should immediately secure the session. "
                    + "Setting this to lock (value 1) prevents the workstation from remaining unlocked when the physical credential is withdrawn. "
                    + "Removing this policy reverts to the default behavior (no action on card removal).",
                Tags = ["logon", "smart-card", "physical-security", "lock", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ScRemoveOption", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "ScRemoveOption")],
                DetectOps = [RegOp.CheckDword(Key, "ScRemoveOption", 1)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Locks workstation on smart card removal; prevents unattended session access.",
            },
        ];
    }

    // ── WindowsMailPolicy ──
    private static class _WindowsMailPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Mail";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "winmail-disable-manual-launch",
                Label = "Windows Mail Policy: Block Manual Launch of Windows Mail",
                Category = "System — Time Service",
                Description =
                    "Prevents users from manually launching the Windows Mail application. Enterprise environments that route email exclusively through corporate clients (Outlook, web) should block the inbox Windows Mail app to reduce shadow IT risk.",
                Tags = ["mail", "windows-mail", "launch", "block", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ManualLaunchAllowed", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "ManualLaunchAllowed")],
                DetectOps = [RegOp.CheckDword(Key, "ManualLaunchAllowed", 0)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Prevents use of inbox Windows Mail on managed devices that require corporate mail clients.",
            },
            new TweakDef
            {
                Id = "winmail-disable-mail-import",
                Label = "Windows Mail Policy: Disable Import of External Mail Accounts",
                Category = "System — Time Service",
                Description =
                    "Prevents Windows Mail from importing accounts, messages, or contacts from external mail clients. Disabling import reduces the risk of unauthorized data ingestion from non-corporate mail clients into the Windows Mail store.",
                Tags = ["mail", "windows-mail", "import", "accounts", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "TurnOffMailImport", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "TurnOffMailImport")],
                DetectOps = [RegOp.CheckDword(Key, "TurnOffMailImport", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Prevents unauthorised account and message ingestion from non-corporate mail clients.",
            },
            new TweakDef
            {
                Id = "winmail-block-http-tracking-pixels",
                Label = "Windows Mail Policy: Block HTTP Remote Images (Anti-Tracking)",
                Category = "System — Time Service",
                Description =
                    "Prevents Windows Mail from automatically loading HTTP images embedded in email messages. Remote images (1x1 tracking pixels) are widely used by marketers and threat actors to confirm email addresses are active and track recipient location.",
                Tags = ["mail", "windows-mail", "tracking", "images", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockHTTPImages", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockHTTPImages")],
                DetectOps = [RegOp.CheckDword(Key, "BlockHTTPImages", 1)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Blocks email tracking pixels; prevents confirmation of email activity by marketers and threat actors.",
            },
            new TweakDef
            {
                Id = "winmail-disable-featured-updates",
                Label = "Windows Mail Policy: Disable Featured Updates in Windows Mail",
                Category = "System — Time Service",
                Description =
                    "Turns off the featured/promotional updates displayed within the Windows Mail application. In enterprise deployments, UI promotional messages are distractions that may redirect users to unsanctioned services.",
                Tags = ["mail", "windows-mail", "updates", "promotional", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "TurnOffFeaturedUpdates", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "TurnOffFeaturedUpdates")],
                DetectOps = [RegOp.CheckDword(Key, "TurnOffFeaturedUpdates", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Removes promotional update banners from Windows Mail UI on managed devices.",
            },
            new TweakDef
            {
                Id = "winmail-disable-hotmail-contact-sync",
                Label = "Windows Mail Policy: Disable Hotmail/Live Contact Synchronisation",
                Category = "System — Time Service",
                Description =
                    "Prevents Windows Mail from synchronising contacts with Microsoft Hotmail or Live accounts. On managed devices, contact sync to personal Microsoft accounts creates data exfiltration risk for confidential address book entries.",
                Tags = ["mail", "windows-mail", "hotmail", "contacts", "sync", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "TurnOffHotmailContact", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "TurnOffHotmailContact")],
                DetectOps = [RegOp.CheckDword(Key, "TurnOffHotmailContact", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents corporate address book data from syncing to personal Microsoft accounts.",
            },
            new TweakDef
            {
                Id = "winmail-force-plaintext-display",
                Label = "Windows Mail Policy: Force Plaintext Rendering for Email",
                Category = "System — Time Service",
                Description =
                    "Forces Windows Mail to render incoming messages as plain text only. HTML email is the primary delivery vector for phishing attacks (hidden links, CSS tricks, JavaScript payloads). Plain text rendering neutralises the entire class of HTML-based email threats.",
                Tags = ["mail", "windows-mail", "plaintext", "html", "phishing", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ForceHTMLMailAsPlainText", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "ForceHTMLMailAsPlainText")],
                DetectOps = [RegOp.CheckDword(Key, "ForceHTMLMailAsPlainText", 1)],
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote = "Neutralises HTML email threat vector including phishing links and JavaScript; breaks rich text formatting.",
            },
            new TweakDef
            {
                Id = "winmail-block-executable-attachments",
                Label = "Windows Mail Policy: Block Executable File Attachments",
                Category = "System — Time Service",
                Description =
                    "Prevents Windows Mail from delivering or presenting executable file attachments (EXE, COM, BAT, PS1, etc.) to users. Executable email attachments are the most common initial access vector in enterprise phishing campaigns.",
                Tags = ["mail", "windows-mail", "attachments", "executable", "block", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockExecutableAttachments", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockExecutableAttachments")],
                DetectOps = [RegOp.CheckDword(Key, "BlockExecutableAttachments", 1)],
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Blocks executable email attachments — the primary phishing initial-access vector.",
            },
            new TweakDef
            {
                Id = "winmail-disable-shopping-links",
                Label = "Windows Mail Policy: Disable Shopping Promotional Links",
                Category = "System — Time Service",
                Description =
                    "Disables shopping links and promotional offers embedded in Windows Mail. Enterprise mail clients should suppress commercial UI to prevent employee distraction and reduce the risk of clicking unsolicited purchase links.",
                Tags = ["mail", "windows-mail", "shopping", "promotional", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "TurnOffShopping", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "TurnOffShopping")],
                DetectOps = [RegOp.CheckDword(Key, "TurnOffShopping", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Suppresses commercial promotional links within Windows Mail.",
            },
            new TweakDef
            {
                Id = "winmail-disable-news-feed",
                Label = "Windows Mail Policy: Disable News Feed Integration",
                Category = "System — Time Service",
                Description =
                    "Disables the integrated news feed widget within Windows Mail. News feed integration increases background network calls and may display content from external third-party news aggregators, which is inappropriate for managed enterprise environments.",
                Tags = ["mail", "windows-mail", "news", "feed", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "TurnOffNewsFeed", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "TurnOffNewsFeed")],
                DetectOps = [RegOp.CheckDword(Key, "TurnOffNewsFeed", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Removes third-party news feed integration from Windows Mail.",
            },
            new TweakDef
            {
                Id = "winmail-disable-calendar-integration",
                Label = "Windows Mail Policy: Disable Calendar Sync Integration",
                Category = "System — Time Service",
                Description =
                    "Prevents Windows Mail from synchronising calendar data with Microsoft consumer accounts or Exchange integrations not managed by the enterprise. Blocks calendar data from being stored in the Windows Mail local store outside MDM supervision.",
                Tags = ["mail", "windows-mail", "calendar", "sync", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableCalendarIntegration", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCalendarIntegration")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCalendarIntegration", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents calendar data from syncing to unmanaged Microsoft Account stores.",
            },
        ];
    }

    // ── WindowsMediaPlayerPolicy ──
    private static class _WindowsMediaPlayerPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "wmplay-disable-auto-codec-download",
                Label = "Windows Media Player: Disable Automatic Codec Download",
                Category = "System — Time Service",
                Description =
                    "Prevents Windows Media Player from automatically downloading codecs from the Internet when a media file requires one. "
                    + "Automatic codec download can introduce unsigned or malicious codec software that runs in a privileged context. "
                    + "On managed endpoints codecs should be deployed via the software management tool, not pulled from Internet sources at runtime. "
                    + "Removing this policy re-enables automatic codec download when WMP encounters an unsupported format.",
                Tags = ["media-player", "codec", "download", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "PreventCodecDownload", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "PreventCodecDownload")],
                DetectOps = [RegOp.CheckDword(Key, "PreventCodecDownload", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Blocks runtime codec downloads; prevents unsigned codec execution from Internet sources.",
            },
            new TweakDef
            {
                Id = "wmplay-disable-auto-update-check",
                Label = "Windows Media Player: Disable Automatic Update Checking",
                Category = "System — Time Service",
                Description =
                    "Prevents Windows Media Player from automatically checking for updates on the Internet. "
                    + "Automatic update checks for WMP can generate unexpected outbound traffic to Microsoft update servers. "
                    + "Updates should be managed through WSUS, SCCM, or Intune rather than individual application self-update mechanisms. "
                    + "Removing this policy re-enables WMP's automatic update check on application launch.",
                Tags = ["media-player", "auto-update", "bandwidth", "enterprise", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "PreventAutoUpdate", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "PreventAutoUpdate")],
                DetectOps = [RegOp.CheckDword(Key, "PreventAutoUpdate", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Disables WMP self-update checks; consolidates media player updates through WSUS.",
            },
            new TweakDef
            {
                Id = "wmplay-disable-internet-streaming",
                Label = "Windows Media Player: Disable Internet Media Streaming",
                Category = "System — Time Service",
                Description =
                    "Restricts Windows Media Player from streaming media content from Internet URLs. "
                    + "Allowing arbitrary Internet streaming can consume significant bandwidth and may result in access to unlicensed or inappropriate content. "
                    + "On corporate networks, media streaming should be restricted to internal or approved sources only. "
                    + "Removing this policy re-enables Internet-based media streaming in WMP.",
                Tags = ["media-player", "streaming", "internet", "bandwidth", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "PreventMediaSharing", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "PreventMediaSharing")],
                DetectOps = [RegOp.CheckDword(Key, "PreventMediaSharing", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Prevents WMP Internet streaming; conserves bandwidth and blocks unapproved content.",
            },
            new TweakDef
            {
                Id = "wmplay-disable-digital-rights-management",
                Label = "Windows Media Player: Disable DRM License Acquisition from Internet",
                Category = "System — Time Service",
                Description =
                    "Prevents Windows Media Player from automatically acquiring DRM (Digital Rights Management) licenses from the Internet. "
                    + "Automatic DRM license acquisition initiates outbound connections to third-party license servers without explicit user consent. "
                    + "On managed endpoints, DRM license acquisition should be user-confirmed or blocked entirely. "
                    + "Removing this policy re-enables automatic DRM license acquisition when protected media files are opened.",
                Tags = ["media-player", "drm", "licensing", "internet", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "PreventDRMacquisition", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "PreventDRMacquisition")],
                DetectOps = [RegOp.CheckDword(Key, "PreventDRMacquisition", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Blocks DRM auto-acquisition; prevents silent outbound connections to license servers.",
            },
            new TweakDef
            {
                Id = "wmplay-disable-media-information-online",
                Label = "Windows Media Player: Disable Online Media Information Retrieval",
                Category = "System — Time Service",
                Description =
                    "Prevents WMP from connecting to the Internet to retrieve album artwork, track information, and music metadata. "
                    + "Online metadata requests reveal what media files are being played to Microsoft or third-party data providers. "
                    + "This is a privacy risk on endpoints where users play internal audio recordings or video files. "
                    + "Removing this policy re-enables online media information lookup in WMP.",
                Tags = ["media-player", "metadata", "privacy", "internet", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableMusicMetadata", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableMusicMetadata")],
                DetectOps = [RegOp.CheckDword(Key, "DisableMusicMetadata", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Blocks online metadata lookup; prevents media file usage disclosure to third parties.",
            },
            new TweakDef
            {
                Id = "wmplay-disable-remote-skin-download",
                Label = "Windows Media Player: Disable Remote Skin and Visualizer Download",
                Category = "System — Time Service",
                Description =
                    "Prevents Windows Media Player from downloading skins, visualizations, and plug-in content from the Internet. "
                    + "Remote skin and plug-in downloads represent an arbitrary code execution risk if the download source is compromised or spoofed. "
                    + "On managed endpoints, WMP customization content should come only from the software management catalog. "
                    + "Removing this policy re-enables remote skin and visualizer downloads from Microsoft.",
                Tags = ["media-player", "skin", "plugin", "download", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "PreventRadioPresetsRetrieval", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "PreventRadioPresetsRetrieval")],
                DetectOps = [RegOp.CheckDword(Key, "PreventRadioPresetsRetrieval", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Blocks WMP remote content downloads; prevents unofficial plugins and skins from executing.",
            },
        ];
    }

    // ── WindowsMediaPolicyAdv ──
    private static class _WindowsMediaPolicyAdv
    {
        private const string WmpLm = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer";
        private const string WmpCu = @"HKEY_CURRENT_USER\Software\Policies\Microsoft\WindowsMediaPlayer";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "wmply-no-screensaver",
                Label = "WMP: Disable screensaver activation during audio playback",
                Category = "System — Time Service",
                Description =
                    "Sets AllowScreenSaver=0 in the Windows Media Player policy key. Prevents the "
                    + "screensaver from activating while WMP is playing audio, even when the screen is idle.",
                Tags = ["media", "wmp", "screensaver", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(WmpLm, "AllowScreenSaver", 0)],
                RemoveOps = [RegOp.DeleteValue(WmpLm, "AllowScreenSaver")],
                DetectOps = [RegOp.CheckDword(WmpLm, "AllowScreenSaver", 0)],
            },
            new TweakDef
            {
                Id = "wmply-no-network-protocol-download",
                Label = "WMP: Prevent automatic network protocol download",
                Category = "System — Time Service",
                Description =
                    "Sets PreventNetworkProtocolAutomaticDownload=1. Prevents Windows Media Player from "
                    + "automatically downloading streaming network protocol components.",
                Tags = ["media", "wmp", "network", "protocol", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(WmpLm, "PreventNetworkProtocolAutomaticDownload", 1)],
                RemoveOps = [RegOp.DeleteValue(WmpLm, "PreventNetworkProtocolAutomaticDownload")],
                DetectOps = [RegOp.CheckDword(WmpLm, "PreventNetworkProtocolAutomaticDownload", 1)],
            },
            new TweakDef
            {
                Id = "wmply-user-no-cd-metadata",
                Label = "WMP (user): Prevent CD/DVD metadata retrieval per user",
                Category = "System — Time Service",
                Description =
                    "Sets PreventCDDVDMetadataRetrieval=1 at the per-user policy scope (HKCU). Enforces "
                    + "no-internet-metadata policy for the current user regardless of machine policy.",
                Tags = ["media", "wmp", "metadata", "privacy", "policy"],
                NeedsAdmin = false,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(WmpCu, "PreventCDDVDMetadataRetrieval", 1)],
                RemoveOps = [RegOp.DeleteValue(WmpCu, "PreventCDDVDMetadataRetrieval")],
                DetectOps = [RegOp.CheckDword(WmpCu, "PreventCDDVDMetadataRetrieval", 1)],
            },
            new TweakDef
            {
                Id = "wmply-user-no-music-metadata",
                Label = "WMP (user): Prevent music metadata retrieval per user",
                Category = "System — Time Service",
                Description =
                    "Sets PreventMusicFileMetadataRetrieval=1 at the per-user policy scope (HKCU). "
                    + "Stops the current user's WMP session from downloading online music metadata.",
                Tags = ["media", "wmp", "metadata", "privacy", "policy"],
                NeedsAdmin = false,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(WmpCu, "PreventMusicFileMetadataRetrieval", 1)],
                RemoveOps = [RegOp.DeleteValue(WmpCu, "PreventMusicFileMetadataRetrieval")],
                DetectOps = [RegOp.CheckDword(WmpCu, "PreventMusicFileMetadataRetrieval", 1)],
            },
            new TweakDef
            {
                Id = "wmply-user-no-radio-presets",
                Label = "WMP (user): Prevent internet radio presets per user",
                Category = "System — Time Service",
                Description =
                    "Sets PreventRadioPresetsRetrieval=1 at the per-user policy scope (HKCU). Prevents "
                    + "the current user's WMP from fetching online radio station preset lists.",
                Tags = ["media", "wmp", "radio", "privacy", "policy"],
                NeedsAdmin = false,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(WmpCu, "PreventRadioPresetsRetrieval", 1)],
                RemoveOps = [RegOp.DeleteValue(WmpCu, "PreventRadioPresetsRetrieval")],
                DetectOps = [RegOp.CheckDword(WmpCu, "PreventRadioPresetsRetrieval", 1)],
            },
        ];
    }

    // ── WindowsPerformancePolicy ──
    private static class _WindowsPerformancePolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Performance";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "wnperf-restrict-background-activity",
                Label = "Restrict Background Application Activity Through Performance Policy",
                Category = "System — Time Service",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Restricting background application activity through performance policy limits the CPU and I/O resources that background and suspended applications can consume improving foreground application responsiveness. Background activity restrictions ensure that non-interactive applications do not consume system resources at the expense of user-facing processes. Enterprise workstations running data analytics batch jobs or synchronization tasks in the background benefit from policies that prioritize interactive work. Resource limitations on background activity also constrain the impact of malware that attempts to use background execution contexts for long-running operations like encryption or data exfiltration. Performance policy restrictions apply to applications running in the background execution manager context which covers many Windows Store and background service applications. Organizations should test background activity restrictions to verify that necessary background operations like antivirus scanning and backup software continue to function.",
                Tags = ["performance", "background-activity", "resource-management", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictBackgroundActivity", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictBackgroundActivity")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictBackgroundActivity", 1)],
            },
            new TweakDef
            {
                Id = "wnperf-enable-cpu-priority-boost",
                Label = "Enable CPU Priority Boosting for Foreground Interactive Applications",
                Category = "System — Time Service",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "CPU priority boosting for foreground applications gives interactive applications preferential CPU scheduling over background processes improving user experience for interactive computing tasks. Windows automatically boosts the scheduling priority of the foreground application to ensure responsive input handling but policy controls can extend this boost to all interactive applications. Priority boosting ensures that user-visible applications remain responsive even when background tasks are consuming significant CPU resources. Security tools like real-time antivirus engines use background priority to avoid impacting foreground performance and the foreground priority boost ensures that interactive work retains precedence. Organizations should verify that critical services that run in the background are not negatively impacted by foreground priority boosts reducing their ability to complete time-sensitive work. Performance monitoring should verify that the priority boost achieves the intended improvement in interactive responsiveness.",
                Tags = ["performance", "cpu-priority", "foreground-application", "scheduling", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableForegroundPriorityBoost", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableForegroundPriorityBoost")],
                DetectOps = [RegOp.CheckDword(Key, "EnableForegroundPriorityBoost", 1)],
            },
            new TweakDef
            {
                Id = "wnperf-configure-memory-usage-policy",
                Label = "Configure System Memory Usage Policy for Balanced Performance",
                Category = "System — Time Service",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Memory usage policy configuration controls how Windows manages physical memory allocation between application working sets page file usage and system cache to optimize performance for the primary system role. Server systems benefit from configuring memory policy to prioritize system cache and services while workstations benefit from policies that prioritize application working sets. Appropriate memory policy configuration reduces the frequency of hard page faults where data must be read from disk rather than satisfied from physical memory. Memory policy can be tuned to reduce paging activity on systems with adequate memory by configuring more aggressive working set retention. Organizations should evaluate the primary workload of each system type when setting memory policy to ensure the configuration matches the workload profile. Memory configuration changes should be validated through performance baseline comparison to verify improvement in the target metrics.",
                Tags = ["performance", "memory-management", "working-set", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableMemoryUsagePolicy", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableMemoryUsagePolicy")],
                DetectOps = [RegOp.CheckDword(Key, "EnableMemoryUsagePolicy", 1)],
            },
            new TweakDef
            {
                Id = "wnperf-disable-animated-windows-effects",
                Label = "Disable Animated Window Effects for Improved System Performance",
                Category = "System — Time Service",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Animated window effects including window minimize maximize fade and slide animations consume GPU resources and cause perceived latency in window operations that can be eliminated through policy without user impact on most enterprise workstations. Disabling animations through performance policy provides consistent visual performance settings across all managed workstations. On systems with limited GPU resources or dedicated GPU resources needed for business applications animation effects compete for GPU time with productive work. Disabling window animations is particularly beneficial for virtual desktop infrastructure environments where GPU resources are shared across many VM sessions. Policy-based animation control ensures consistent application of performance settings without relying on individual users to configure visual effects manually. The performance impact of disabling animations varies by hardware but is most significant on integrated graphics systems where CPU and GPU share memory bandwidth.",
                Tags = ["performance", "animations", "visual-effects", "gpu", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAnimatedEffects", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAnimatedEffects")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAnimatedEffects", 1)],
            },
            new TweakDef
            {
                Id = "wnperf-configure-disk-io-scheduling",
                Label = "Configure Disk I/O Scheduling for Application vs System Service Balance",
                Category = "System — Time Service",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Disk I/O scheduling policy controls how Windows prioritizes disk I/O requests between application I/O background I/O and system service I/O ensuring that critical I/O operations are completed with appropriate latency. I/O scheduling configuration affects how quickly applications can write logs read data files and access databases compared to background operations like disk defragmentation and search indexing. Systems that run intensive background disk I/O operations benefit from I/O scheduling policy that prevents background I/O from saturating disk bandwidth needed by foreground applications. NVMe SSD systems have lower scheduling overhead than traditional spinning disks but can still benefit from I/O prioritization for mixed workloads with both interactive and batch I/O. Organizations should profile disk I/O patterns across their fleet to identify systems where I/O contention is causing application performance degradation that policy can mitigate. I/O scheduling policy is particularly relevant for database servers file servers and storage-intensive workloads.",
                Tags = ["performance", "disk-io", "scheduling", "storage", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableDiskIOScheduling", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableDiskIOScheduling")],
                DetectOps = [RegOp.CheckDword(Key, "EnableDiskIOScheduling", 1)],
            },
            new TweakDef
            {
                Id = "wnperf-restrict-startup-program-execution",
                Label = "Restrict Startup Program Execution to Approved Application List",
                Category = "System — Time Service",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Startup program execution restrictions prevent unauthorized applications from adding themselves to Windows startup locations and executing automatically at user login increasing both security and performance. Malware and potentially unwanted applications frequently add themselves to startup locations to maintain persistence and execute at each user login. Restricting startup execution to an approved list prevents both unauthorized persistence establishment and the performance degradation from accumulating startup programs over time. Startup program restrictions work best when combined with Software Restriction Policies or AppLocker to prevent programs from loading regardless of how they are invoked. Organizations should define the startup program allowlist based on applications that have legitimate business requirements for startup execution and review and trim this list periodically. User-controlled startup programs should be completely disabled for standard users who should not have the ability to modify which programs run automatically.",
                Tags = ["performance", "startup-programs", "persistence-prevention", "allowlist", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictStartupPrograms", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictStartupPrograms")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictStartupPrograms", 1)],
            },
            new TweakDef
            {
                Id = "wnperf-configure-network-throttling",
                Label = "Configure Network Bandwidth Throttling for Background Update Operations",
                Category = "System — Time Service",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Network bandwidth throttling for background update operations limits the bandwidth consumed by Windows Update delivery optimization and other background network consumers preventing them from saturating network connections during productive hours. Without throttling Windows Update delivery optimization and peer-to-peer update distribution can consume significant network bandwidth that impacts interactive work and business applications. Policy-based throttling configurations can restrict background network usage to specific percentages of available bandwidth and can vary restrictions based on time of day to allow unrestricted updates during off-hours. Delivery optimization settings should be configured to prioritize enterprise update caching servers over internet downloads where available reducing upstream bandwidth consumption. Organizations with thin WAN links benefit most from network throttling policies that prevent update traffic from impacting business-critical applications on constrained bandwidth connections. Background network usage should be monitored to verify that throttling is correctly limiting consumption and that updates are still completing within required timeframes.",
                Tags = ["performance", "network-throttling", "background-updates", "bandwidth", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableNetworkThrottling", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableNetworkThrottling")],
                DetectOps = [RegOp.CheckDword(Key, "EnableNetworkThrottling", 1)],
            },
            new TweakDef
            {
                Id = "wnperf-enable-prefetch-optimization",
                Label = "Enable Prefetch Optimization for Frequently Used Application Launch",
                Category = "System — Time Service",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Prefetch optimization allows Windows to pre-load frequently used application data and code from disk into memory before the application is launched improving perceived application startup performance. Prefetch monitoring tracks which application data is accessed during startup and creates prefetch files that inform future startup pre-loading operations. The prefetch system improves application launch time by ensuring that frequently used executable pages and data files are in memory before they are needed. Prefetch optimization is most effective on spinning disk systems where sequential pre-reading is significantly faster than random access. On SSD systems the performance benefit is smaller but prefetch still provides improvement for large applications with slow-loading modules. Organizations should ensure that prefetch is enabled on production workstations and that the prefetch data directory has adequate storage space for the prefetch files accumulated by the applications in use.",
                Tags = ["performance", "prefetch", "application-launch", "startup", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnablePrefetchOptimization", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnablePrefetchOptimization")],
                DetectOps = [RegOp.CheckDword(Key, "EnablePrefetchOptimization", 1)],
            },
            new TweakDef
            {
                Id = "wnperf-configure-power-performance-balance",
                Label = "Configure Power and Performance Balance Policy for Enterprise Workloads",
                Category = "System — Time Service",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Power and performance balance policy controls the tradeoff between computing performance and energy consumption ensuring that enterprise workstations deliver appropriate performance for business workloads. High-performance power plans increase processor performance states and disable power saving features that can reduce responsiveness for interactive and compute-intensive applications. Balanced power plans provide a reasonable compromise for most enterprise workloads adjusting performance states dynamically based on workload demand. Performance-critical applications like database servers engineering applications and real-time processing systems benefit from high-performance power plan configurations. Organizations should match power plan configuration to the workload profile of each system type rather than applying a uniform policy across all systems. Power consumption monitoring can verify that the configured power plan achieves the intended energy consumption profile for server hosting environments.",
                Tags = ["performance", "power-plan", "energy", "workload-tuning", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ConfigurePowerPerformanceBalance", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "ConfigurePowerPerformanceBalance")],
                DetectOps = [RegOp.CheckDword(Key, "ConfigurePowerPerformanceBalance", 1)],
            },
            new TweakDef
            {
                Id = "wnperf-enable-performance-audit-logging",
                Label = "Enable Performance Audit Logging for System Resource Utilization",
                Category = "System — Time Service",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Performance audit logging records system resource utilization events including CPU memory disk and network consumption providing data for capacity planning and performance anomaly detection. Performance data collected through audit logging enables historical analysis that identifies performance degradation trends before they become user-impacting problems. Security-relevant performance events such as sudden increases in CPU or memory consumption may indicate ongoing exploitation or malware execution. Performance audit data should be retained and analyzed alongside security event data to correlate performance anomalies with security incidents. Organizations should establish performance baselines for each system role to enable meaningful comparison of current performance against expected ranges. Performance audit logging data should be lightweight and targeted at high-value metrics that provide actionable insight without generating excessive log volume.",
                Tags = ["performance", "audit-logging", "resource-utilization", "baseline", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnablePerformanceAuditLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnablePerformanceAuditLogging")],
                DetectOps = [RegOp.CheckDword(Key, "EnablePerformanceAuditLogging", 1)],
            },
        ];
    }

    // ── WindowsReliabilityPolicy ──
    private static class _WindowsReliabilityPolicy
    {
        private const string RelKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Reliability";
        private const string WerKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "relpol-disable-shutdown-tracker",
                Label = "Reliability Policy: Disable Shutdown Event Tracker",
                Category = "System — Windows Reliability",
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
                Category = "System — Windows Reliability",
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
                Category = "System — Windows Reliability",
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
                Category = "System — Windows Reliability",
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
                Category = "System — Windows Reliability",
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
                Category = "System — Windows Reliability",
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
                Id = "relpol-disable-wer-ui-prompt",
                Label = "Reliability Policy: Disable WER User Prompt Dialog",
                Category = "System — Windows Reliability",
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
                Id = "relpol-disable-wer-kernel-dump",
                Label = "Reliability Policy: Disable WER Kernel Fault/Dump Reporting",
                Category = "System — Windows Reliability",
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

    // ── WindowsTimeGpoPolicy ──
    private static class _WindowsTimeGpoPolicy
    {
        private const string W32Params = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\W32Time\Parameters";

        private const string W32Config = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\W32Time\Config";

        private const string NtpClient = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\W32Time\TimeProviders\NtpClient";

        private const string NtpServer = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\W32Time\TimeProviders\NtpServer";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "timepol-ntp-server-pool",
                Label = "Configure NTP pool servers (policy)",
                Category = "System — Windows Reliability",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Sets W32Time to synchronise from the NTP pool servers via GPO. "
                    + "NtpServer=0.pool.ntp.org,0x9 1.pool.ntp.org,0x9 2.pool.ntp.org,0x9 3.pool.ntp.org,0x9.",
                Tags = ["time", "ntp", "pool", "servers", "policy"],
                ApplyOps = [RegOp.SetString(W32Params, "NtpServer", "0.pool.ntp.org,0x9 1.pool.ntp.org,0x9 2.pool.ntp.org,0x9 3.pool.ntp.org,0x9")],
                RemoveOps = [RegOp.DeleteValue(W32Params, "NtpServer")],
                DetectOps =
                [
                    RegOp.CheckString(W32Params, "NtpServer", "0.pool.ntp.org,0x9 1.pool.ntp.org,0x9 2.pool.ntp.org,0x9 3.pool.ntp.org,0x9"),
                ],
            },
            new TweakDef
            {
                Id = "timepol-ntpclient-enable",
                Label = "Enable NTP client time provider (policy)",
                Category = "System — Windows Reliability",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Enables the NTP client time provider via Group Policy so W32Time actively syncs from NTP servers. "
                    + "TimeProviders\\NtpClient Enabled=1.",
                Tags = ["time", "ntp", "client", "policy"],
                ApplyOps = [RegOp.SetDword(NtpClient, "Enabled", 1)],
                RemoveOps = [RegOp.DeleteValue(NtpClient, "Enabled")],
                DetectOps = [RegOp.CheckDword(NtpClient, "Enabled", 1)],
            },
            new TweakDef
            {
                Id = "timepol-ntpclient-poll-hourly",
                Label = "Set NTP client poll interval to 1 hour (policy)",
                Category = "System — Windows Reliability",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Sets the NTP client to poll time servers every hour via policy. "
                    + "SpecialPollInterval=3600. Default: 604800 (1 week). More frequent syncs reduce clock drift.",
                Tags = ["time", "ntp", "poll", "interval", "policy"],
                ApplyOps = [RegOp.SetDword(NtpClient, "SpecialPollInterval", 3600)],
                RemoveOps = [RegOp.DeleteValue(NtpClient, "SpecialPollInterval")],
                DetectOps = [RegOp.CheckDword(NtpClient, "SpecialPollInterval", 3600)],
            },
            new TweakDef
            {
                Id = "timepol-ntpclient-eventlog",
                Label = "Log NTP time jumps and source changes (policy)",
                Category = "System — Windows Reliability",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Enables NTP client event logging for time jumps and server source changes. "
                    + "EventLogFlags=3 (1=time jumps, 2=source changes, 3=both). Default: 0.",
                Tags = ["time", "ntp", "logging", "policy"],
                ApplyOps = [RegOp.SetDword(NtpClient, "EventLogFlags", 3)],
                RemoveOps = [RegOp.DeleteValue(NtpClient, "EventLogFlags")],
                DetectOps = [RegOp.CheckDword(NtpClient, "EventLogFlags", 3)],
            },
            new TweakDef
            {
                Id = "timepol-ntpserver-disable",
                Label = "Disable NTP server time provider on workstations (policy)",
                Category = "System — Windows Reliability",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Prevents this machine from acting as an NTP time server via Group Policy. "
                    + "TimeProviders\\NtpServer Enabled=0. Appropriate for workstations that should be clients only.",
                Tags = ["time", "ntp", "server", "disable", "policy"],
                ApplyOps = [RegOp.SetDword(NtpServer, "Enabled", 0)],
                RemoveOps = [RegOp.DeleteValue(NtpServer, "Enabled")],
                DetectOps = [RegOp.CheckDword(NtpServer, "Enabled", 0)],
            },
            new TweakDef
            {
                Id = "timepol-max-pos-correction",
                Label = "Cap maximum positive time correction at 2 hours (policy)",
                Category = "System — Windows Reliability",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Sets the maximum positive time correction W32Time will accept via policy. "
                    + "MaxPosPhaseCorrection=7200 (2 hours). Prevents unexpectedly large clock forwards.",
                Tags = ["time", "phase", "correction", "policy"],
                ApplyOps = [RegOp.SetDword(W32Config, "MaxPosPhaseCorrection", 7200)],
                RemoveOps = [RegOp.DeleteValue(W32Config, "MaxPosPhaseCorrection")],
                DetectOps = [RegOp.CheckDword(W32Config, "MaxPosPhaseCorrection", 7200)],
            },
            new TweakDef
            {
                Id = "timepol-max-neg-correction",
                Label = "Cap maximum negative time correction at 2 hours (policy)",
                Category = "System — Windows Reliability",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Sets the maximum negative time correction W32Time will accept via policy. "
                    + "MaxNegPhaseCorrection=7200 (2 hours). Prevents unexpectedly large clock rollbacks.",
                Tags = ["time", "phase", "correction", "policy"],
                ApplyOps = [RegOp.SetDword(W32Config, "MaxNegPhaseCorrection", 7200)],
                RemoveOps = [RegOp.DeleteValue(W32Config, "MaxNegPhaseCorrection")],
                DetectOps = [RegOp.CheckDword(W32Config, "MaxNegPhaseCorrection", 7200)],
            },
            new TweakDef
            {
                Id = "timepol-frequency-correct-rate",
                Label = "Set W32Time frequency correction rate (policy)",
                Category = "System — Windows Reliability",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Sets how quickly W32Time corrects clock frequency drift via policy. "
                    + "FrequencyCorrectRate=4 (corrects up to 4 × 10ms per second). Default: 4.",
                Tags = ["time", "frequency", "correction", "policy"],
                ApplyOps = [RegOp.SetDword(W32Config, "FrequencyCorrectRate", 4)],
                RemoveOps = [RegOp.DeleteValue(W32Config, "FrequencyCorrectRate")],
                DetectOps = [RegOp.CheckDword(W32Config, "FrequencyCorrectRate", 4)],
            },
            new TweakDef
            {
                Id = "timepol-phase-correct-rate",
                Label = "Set W32Time phase (offset) correction rate (policy)",
                Category = "System — Windows Reliability",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Sets how aggressively W32Time corrects phase (offset) errors via policy. "
                    + "PhaseCorrectRate=7 (most aggressive). Default: 1. Higher values resolve drift faster.",
                Tags = ["time", "phase", "offset", "policy"],
                ApplyOps = [RegOp.SetDword(W32Config, "PhaseCorrectRate", 7)],
                RemoveOps = [RegOp.DeleteValue(W32Config, "PhaseCorrectRate")],
                DetectOps = [RegOp.CheckDword(W32Config, "PhaseCorrectRate", 7)],
            },
        ];
    }

    // ── WindowsTimePolicy ──
    private static class _WindowsTimePolicy
    {
        private const string ParamKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\W32time\Parameters";
        private const string CfgKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\W32time\Config";
        private const string NtpClient = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\W32time\TimeProviders\NtpClient";
        private const string NtpServer = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\W32time\TimeProviders\NtpServer";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "wtime-set-update-interval",
                Label = "Set Clock Update Interval to 30000 (30 Seconds)",
                Category = "System — Windows Reliability",
                Description =
                    "Sets UpdateInterval=30000 in the W32time Config policy key. "
                    + "Configures how often (in 100-nanosecond units, 30000 = approximately 3ms effective interval) "
                    + "the system clock is adjusted to converge on the NTP reference. "
                    + "Using the system default of 30000 via policy pins this to the recommended value. "
                    + "Default: absent. Recommended: 30000 to enforce clock discipline response rate.",
                Tags = ["ntp", "time", "update-interval", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Pins the clock update interval to the W32tm recommended rate of 30000.",
                ApplyOps = [RegOp.SetDword(CfgKey, "UpdateInterval", 30000)],
                RemoveOps = [RegOp.DeleteValue(CfgKey, "UpdateInterval")],
                DetectOps = [RegOp.CheckDword(CfgKey, "UpdateInterval", 30000)],
            },
        ];
    }

    // ── WinlogonPolicy ──
    private static class _WinlogonPolicy
    {
        private const string WlKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Winlogon";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wlpol-require-ctrl-alt-del",
                    Label = "Require Ctrl+Alt+Delete at Login",
                    Category = "System — Windows Reliability",
                    Description = "Enforces the secure attention sequence (Ctrl+Alt+Delete) before the Windows logon screen appears.",
                    Tags = ["winlogon", "ctrl-alt-del", "logon", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Prevents login spoofing by requiring the hardware-intercepted SAS (Secure Attention Sequence).",
                    ApplyOps = [RegOp.SetDword(WlKey, "DisableCAD", 0)],
                    RemoveOps = [RegOp.DeleteValue(WlKey, "DisableCAD")],
                    DetectOps = [RegOp.CheckDword(WlKey, "DisableCAD", 0)],
                },
                new TweakDef
                {
                    Id = "wlpol-disable-autologon",
                    Label = "Disable Automatic Administrator Logon",
                    Category = "System — Windows Reliability",
                    Description = "Prevents Windows from automatically logging in with a saved administrator account and password at startup.",
                    Tags = ["winlogon", "autologon", "logon", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Disabling AutoAdminLogon forces manual login; critical for devices in shared or public environments.",
                    ApplyOps = [RegOp.SetDword(WlKey, "AutoAdminLogon", 0)],
                    RemoveOps = [RegOp.DeleteValue(WlKey, "AutoAdminLogon")],
                    DetectOps = [RegOp.CheckDword(WlKey, "AutoAdminLogon", 0)],
                },
                new TweakDef
                {
                    Id = "wlpol-lock-on-smartcard-removal",
                    Label = "Lock Workstation on Smart Card Removal",
                    Category = "System — Windows Reliability",
                    Description = "Automatically locks the workstation screen when the user removes their smart card from the reader.",
                    Tags = ["winlogon", "smart-card", "lock", "security", "mfa"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Value 1 = lock workstation; users must re-authenticate after removing their card.",
                    ApplyOps = [RegOp.SetDword(WlKey, "ScRemoveOption", 1)],
                    RemoveOps = [RegOp.DeleteValue(WlKey, "ScRemoveOption")],
                    DetectOps = [RegOp.CheckDword(WlKey, "ScRemoveOption", 1)],
                },
                new TweakDef
                {
                    Id = "wlpol-no-grace-period-after-screensaver",
                    Label = "No Grace Period After Screen Saver for Unlock",
                    Category = "System — Windows Reliability",
                    Description = "Requires immediate credential entry after the screen saver activates, with no grace period delay.",
                    Tags = ["winlogon", "screen-saver", "lock", "grace-period", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Zero-second grace period; users must enter password immediately after screen saver starts.",
                    ApplyOps = [RegOp.SetDword(WlKey, "ScreenSaverGracePeriod", 0)],
                    RemoveOps = [RegOp.DeleteValue(WlKey, "ScreenSaverGracePeriod")],
                    DetectOps = [RegOp.CheckDword(WlKey, "ScreenSaverGracePeriod", 0)],
                },
                new TweakDef
                {
                    Id = "wlpol-enable-force-unlock-logon",
                    Label = "Force Credential Re-Entry on Workstation Unlock",
                    Category = "System — Windows Reliability",
                    Description = "Requires full credential re-entry when unlocking a workstation, even if the same user locked it.",
                    Tags = ["winlogon", "unlock", "credentials", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Prevents pass-through unlock with cached session; full authentication required on every unlock.",
                    ApplyOps = [RegOp.SetDword(WlKey, "ForceUnlockLogon", 1)],
                    RemoveOps = [RegOp.DeleteValue(WlKey, "ForceUnlockLogon")],
                    DetectOps = [RegOp.CheckDword(WlKey, "ForceUnlockLogon", 1)],
                },
                new TweakDef
                {
                    Id = "wlpol-block-software-sas",
                    Label = "Block Software-Generated Secure Attention Sequence",
                    Category = "System — Windows Reliability",
                    Description = "Prevents applications and services from programmatically generating the Ctrl+Alt+Delete SAS.",
                    Tags = ["winlogon", "sas", "security", "ctrl-alt-del"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Value 0 = only hardware can generate SAS; prevents malware from simulating the logon screen.",
                    ApplyOps = [RegOp.SetDword(WlKey, "SoftwareSASGeneration", 0)],
                    RemoveOps = [RegOp.DeleteValue(WlKey, "SoftwareSASGeneration")],
                    DetectOps = [RegOp.CheckDword(WlKey, "SoftwareSASGeneration", 0)],
                },
                new TweakDef
                {
                    Id = "wlpol-run-logon-scripts-sync",
                    Label = "Run Logon Scripts Synchronously",
                    Category = "System — Windows Reliability",
                    Description = "Waits for all logon scripts to complete before presenting the user desktop.",
                    Tags = ["winlogon", "logon-scripts", "gpo", "synchronous"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Desktop shown only after all scripts finish; may increase logon time on complex environments.",
                    ApplyOps = [RegOp.SetDword(WlKey, "RunLogonScriptSync", 1)],
                    RemoveOps = [RegOp.DeleteValue(WlKey, "RunLogonScriptSync")],
                    DetectOps = [RegOp.CheckDword(WlKey, "RunLogonScriptSync", 1)],
                },
                new TweakDef
                {
                    Id = "wlpol-disable-boot-animation",
                    Label = "Disable Windows Boot Animation",
                    Category = "System — Windows Reliability",
                    Description = "Skips the animated Windows splash screen during boot to reduce boot time and remove branding.",
                    Tags = ["winlogon", "boot", "animation", "performance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Removes the spinning dots animation during Windows startup; marginal boot time improvement.",
                    ApplyOps = [RegOp.SetDword(WlKey, "EnableBootStatusPolicy", 0)],
                    RemoveOps = [RegOp.DeleteValue(WlKey, "EnableBootStatusPolicy")],
                    DetectOps = [RegOp.CheckDword(WlKey, "EnableBootStatusPolicy", 0)],
                },
                new TweakDef
                {
                    Id = "wlpol-hide-last-logon-user",
                    Label = "Hide Last Logged-On Username at Logon Screen",
                    Category = "System — Windows Reliability",
                    Description = "Clears the username field at the Windows logon screen so it does not display the last signed-in account.",
                    Tags = ["winlogon", "last-user", "privacy", "logon", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Users must type their full username at each login; prevents username enumeration at the logon screen.",
                    ApplyOps = [RegOp.SetDword(WlKey, "DontDisplayLastUserName", 1)],
                    RemoveOps = [RegOp.DeleteValue(WlKey, "DontDisplayLastUserName")],
                    DetectOps = [RegOp.CheckDword(WlKey, "DontDisplayLastUserName", 1)],
                },
                new TweakDef
                {
                    Id = "wlpol-limit-cached-logons",
                    Label = "Limit Cached Domain Logon Credentials",
                    Category = "System — Windows Reliability",
                    Description = "Restricts how many domain credentials Windows caches locally for offline logon situations.",
                    Tags = ["winlogon", "cached-logon", "credentials", "domain", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "Caches only 2 domain accounts locally; reduces credential exposure if disk is compromised. Set to 0 to disable caching entirely.",
                    ApplyOps = [RegOp.SetDword(WlKey, "CachedLogonsCount", 2)],
                    RemoveOps = [RegOp.DeleteValue(WlKey, "CachedLogonsCount")],
                    DetectOps = [RegOp.CheckDword(WlKey, "CachedLogonsCount", 2)],
                },
            ];
    }
}
