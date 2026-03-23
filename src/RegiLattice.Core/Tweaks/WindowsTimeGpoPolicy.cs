#nullable enable
using System.Collections.Generic;
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

// Slug "timepol" — W32Time GPO policy path (SOFTWARE\Policies\Microsoft\W32Time\*).
// NOTE: TimeSync.cs uses SYSTEM\CurrentControlSet\Services\W32Time\* (service config).
//       This module enforces the same settings via the separate GPO enforcement path.
// SOFTWARE\Policies\Microsoft\W32Time\Parameters
// SOFTWARE\Policies\Microsoft\W32Time\Config
// SOFTWARE\Policies\Microsoft\W32Time\TimeProviders\NtpClient
// SOFTWARE\Policies\Microsoft\W32Time\TimeProviders\NtpServer
internal static class WindowsTimeGpoPolicy
{
    private const string W32Params = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\W32Time\Parameters";

    private const string W32Config = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\W32Time\Config";

    private const string NtpClient = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\W32Time\TimeProviders\NtpClient";

    private const string NtpServer = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\W32Time\TimeProviders\NtpServer";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "timepol-type-ntp",
            Label = "Enforce W32Time sync type NTP (policy)",
            Category = "Time Service Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Configures W32Time to use NTP as the synchronisation source via Group Policy. "
                + "Type=NTP. Standalone workstations default to NT5DS on domain or NTP when standalone.",
            Tags = ["time", "ntp", "w32time", "policy"],
            ApplyOps = [RegOp.SetString(W32Params, "Type", "NTP")],
            RemoveOps = [RegOp.DeleteValue(W32Params, "Type")],
            DetectOps = [RegOp.CheckString(W32Params, "Type", "NTP")],
        },
        new TweakDef
        {
            Id = "timepol-ntp-server-pool",
            Label = "Configure NTP pool servers (policy)",
            Category = "Time Service Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets W32Time to synchronise from the NTP pool servers via GPO. "
                + "NtpServer=0.pool.ntp.org,0x9 1.pool.ntp.org,0x9 2.pool.ntp.org,0x9 3.pool.ntp.org,0x9.",
            Tags = ["time", "ntp", "pool", "servers", "policy"],
            ApplyOps = [RegOp.SetString(W32Params, "NtpServer", "0.pool.ntp.org,0x9 1.pool.ntp.org,0x9 2.pool.ntp.org,0x9 3.pool.ntp.org,0x9")],
            RemoveOps = [RegOp.DeleteValue(W32Params, "NtpServer")],
            DetectOps = [RegOp.CheckString(W32Params, "NtpServer", "0.pool.ntp.org,0x9 1.pool.ntp.org,0x9 2.pool.ntp.org,0x9 3.pool.ntp.org,0x9")],
        },
        new TweakDef
        {
            Id = "timepol-ntpclient-enable",
            Label = "Enable NTP client time provider (policy)",
            Category = "Time Service Policy",
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
            Category = "Time Service Policy",
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
            Category = "Time Service Policy",
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
            Category = "Time Service Policy",
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
            Category = "Time Service Policy",
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
            Category = "Time Service Policy",
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
            Category = "Time Service Policy",
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
            Category = "Time Service Policy",
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
