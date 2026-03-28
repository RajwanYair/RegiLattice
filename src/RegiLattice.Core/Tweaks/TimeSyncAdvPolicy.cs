// RegiLattice.Core — Tweaks/TimeSyncAdvPolicy.cs
// Advanced Time Synchronisation Group Policy — Sprint 426.
// Controls Windows Time service (W32tm) NTP configuration, sync interval,
// and phase correction limits via the W32time policy registry paths.
// Category: "Time Sync Advanced Policy" | Slug: tsap
// Registry path: HKLM\SOFTWARE\Policies\Microsoft\W32time\Parameters
//                HKLM\SOFTWARE\Policies\Microsoft\W32time\Config

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class TimeSyncAdvPolicy
{
    private const string ParamKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\W32time\Parameters";
    private const string CfgKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\W32time\Config";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "tsap-set-ntp-type",
                Label = "Set W32tm Sync Type to NTP",
                Category = "Time Sync Advanced Policy",
                Description =
                    "Sets the W32tm Type value to 'NTP' via policy, instructing the Windows Time service to use NTP time sources only (not domain hierarchy NT5DS). Required on stand-alone machines or workgroup environments.",
                Tags = ["time sync", "ntp", "w32tm", "policy"],
                NeedsAdmin = true,
                CorpSafe = false,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "W32tm uses NTP sources; bypasses domain hierarchy sync which may cause skew in AD.",
                ApplyOps = [RegOp.SetString(ParamKey, "Type", "NTP")],
                RemoveOps = [RegOp.DeleteValue(ParamKey, "Type")],
                DetectOps = [RegOp.CheckString(ParamKey, "Type", "NTP")],
            },
            new TweakDef
            {
                Id = "tsap-set-ntp-server",
                Label = "Set NTP Server to pool.ntp.org",
                Category = "Time Sync Advanced Policy",
                Description =
                    "Sets NtpServer='pool.ntp.org,0x9' as the time source for the Windows Time service via Group Policy. Use '0x9' flags (poll + step). Replaces time.windows.com for environments that prefer public NTP pools.",
                Tags = ["time sync", "ntp", "ntp server", "policy"],
                NeedsAdmin = true,
                CorpSafe = false,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "System syncs to pool.ntp.org; suitable for non-domain SOHO and development machines.",
                ApplyOps = [RegOp.SetString(ParamKey, "NtpServer", "pool.ntp.org,0x9")],
                RemoveOps = [RegOp.DeleteValue(ParamKey, "NtpServer")],
                DetectOps = [RegOp.CheckString(ParamKey, "NtpServer", "pool.ntp.org,0x9")],
            },
            new TweakDef
            {
                Id = "tsap-disable-nosync",
                Label = "Prevent W32tm NoSync Mode",
                Category = "Time Sync Advanced Policy",
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
                Category = "Time Sync Advanced Policy",
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
                Category = "Time Sync Advanced Policy",
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
                Id = "tsap-set-max-pos-phase-correction",
                Label = "Limit Max Positive Phase Correction (1 h)",
                Category = "Time Sync Advanced Policy",
                Description =
                    "Sets MaxPosPhaseCorrection=3600 (seconds) to cap how far forward the system clock can be stepped in a single sync. Default: 0x7FFFFFFF (unlimited). Prevents accidental large forward clock jumps on domain or NTP misconfiguration.",
                Tags = ["time sync", "phase correction", "clock jump", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Clock cannot jump forward more than 1 hour in one step; protects certificate/Kerberos validity.",
                ApplyOps = [RegOp.SetDword(CfgKey, "MaxPosPhaseCorrection", 3600)],
                RemoveOps = [RegOp.DeleteValue(CfgKey, "MaxPosPhaseCorrection")],
                DetectOps = [RegOp.CheckDword(CfgKey, "MaxPosPhaseCorrection", 3600)],
            },
            new TweakDef
            {
                Id = "tsap-set-max-neg-phase-correction",
                Label = "Limit Max Negative Phase Correction (1 h)",
                Category = "Time Sync Advanced Policy",
                Description =
                    "Sets MaxNegPhaseCorrection=3600 (seconds) to cap how far backward the system clock can be stepped. Default: 0x7FFFFFFF (unlimited). Prevents Kerberos ticket invalidation and certificate errors caused by large backward clock jumps.",
                Tags = ["time sync", "phase correction", "clock", "kerberos", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Clock cannot jump back more than 1 hour; protects authentication tokens.",
                ApplyOps = [RegOp.SetDword(CfgKey, "MaxNegPhaseCorrection", 3600)],
                RemoveOps = [RegOp.DeleteValue(CfgKey, "MaxNegPhaseCorrection")],
                DetectOps = [RegOp.CheckDword(CfgKey, "MaxNegPhaseCorrection", 3600)],
            },
            new TweakDef
            {
                Id = "tsap-enable-hyperv-timesync",
                Label = "Enable Hyper-V Time Sync Provider",
                Category = "Time Sync Advanced Policy",
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
                Category = "Time Sync Advanced Policy",
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
                Category = "Time Sync Advanced Policy",
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
