#nullable enable
using System.Collections.Generic;
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

// Sprint 249 — Windows Time Service (NTP) Group Policy (10 tweaks)
// Keys: HKLM\SOFTWARE\Policies\Microsoft\W32time\Parameters
//       HKLM\SOFTWARE\Policies\Microsoft\W32time\Config
//       HKLM\SOFTWARE\Policies\Microsoft\W32time\TimeProviders\NtpClient
//       HKLM\SOFTWARE\Policies\Microsoft\W32time\TimeProviders\NtpServer
internal static class WindowsTimePolicy
{
    private const string ParamKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\W32time\Parameters";
    private const string CfgKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\W32time\Config";
    private const string NtpClient = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\W32time\TimeProviders\NtpClient";
    private const string NtpServer = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\W32time\TimeProviders\NtpServer";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "wtime-set-ntp-server",
            Label = "Configure NTP Server to time.windows.com",
            Category = "Windows Time Policy",
            Description =
                "Sets the NtpServer value in the W32time Parameters policy key to 'time.windows.com,0x9'. "
                + "Configures Windows Time Service to sync from Microsoft's public NTP server using the NT5DS+NTP type. "
                + "Type=NTP is required for non-domain workstations; domain-joined machines default to NT5DS hierarchy. "
                + "Default: 'time.windows.com,0x9' (NTP). Recommended: enforce via policy to prevent drift.",
            Tags = ["ntp", "time", "sync", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Forces NTP sync from time.windows.com; system clock stays accurate on non-domain machines.",
            ApplyOps = [RegOp.SetString(ParamKey, "NtpServer", "time.windows.com,0x9")],
            RemoveOps = [RegOp.DeleteValue(ParamKey, "NtpServer")],
            DetectOps = [RegOp.CheckString(ParamKey, "NtpServer", "time.windows.com,0x9")],
        },
        new TweakDef
        {
            Id = "wtime-force-ntp-type",
            Label = "Force Time Sync Type to NTP",
            Category = "Windows Time Policy",
            Description =
                "Sets Type=NTP in the W32time Parameters policy key. "
                + "Forces the Windows Time Service to use NTP (Network Time Protocol) as the sync source "
                + "rather than the domain hierarchy (NT5DS) or no sync (NoSync). "
                + "Essential for workgroup machines to maintain accurate time against external NTP servers. "
                + "Default: NT5DS on domain, NTP on standalone. Recommended: NTP for all non-domain machines.",
            Tags = ["ntp", "time", "type", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Time service uses NTP for synchronisation; do not set on domain machines (breaks DC hierarchy).",
            ApplyOps = [RegOp.SetString(ParamKey, "Type", "NTP")],
            RemoveOps = [RegOp.DeleteValue(ParamKey, "Type")],
            DetectOps = [RegOp.CheckString(ParamKey, "Type", "NTP")],
        },
        new TweakDef
        {
            Id = "wtime-enable-ntp-client",
            Label = "Enable NTP Client Provider",
            Category = "Windows Time Policy",
            Description =
                "Sets Enabled=1 in the W32time NtpClient TimeProvider policy key. "
                + "Ensures the built-in NTP client provider is active and allowed to gather time samples "
                + "from configured NTP servers. Without this, the W32tm service cannot get NTP time. "
                + "Default: absent (enabled by default). Recommended: 1 to explicitly enforce via policy.",
            Tags = ["ntp", "client", "time", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "NTP client provider explicitly enabled by policy; system will actively sync from NTP servers.",
            ApplyOps = [RegOp.SetDword(NtpClient, "Enabled", 1)],
            RemoveOps = [RegOp.DeleteValue(NtpClient, "Enabled")],
            DetectOps = [RegOp.CheckDword(NtpClient, "Enabled", 1)],
        },
        new TweakDef
        {
            Id = "wtime-disable-ntp-server",
            Label = "Disable NTP Server Provider",
            Category = "Windows Time Policy",
            Description =
                "Sets Enabled=0 in the W32time NtpServer TimeProvider policy key. "
                + "Disables this machine from acting as an NTP server for other clients on the network. "
                + "Workstations should never serve NTP time to peers; only dedicated time servers or DCs should. "
                + "Default: absent (NTP server role disabled on non-DC machines). Recommended: 0 on workstations.",
            Tags = ["ntp", "server", "time", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "This machine will not serve NTP time to other clients; inbound NTP queries are ignored.",
            ApplyOps = [RegOp.SetDword(NtpServer, "Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(NtpServer, "Enabled")],
            DetectOps = [RegOp.CheckDword(NtpServer, "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "wtime-set-poll-interval-6",
            Label = "Set NTP Poll Interval to 64 Seconds (Accurate)",
            Category = "Windows Time Policy",
            Description =
                "Sets SpecialPollInterval=64 in the W32time NtpClient policy key. "
                + "Configures the NTP client to query the time server every 64 seconds (2^6), "
                + "providing fast correction for machines where tight time accuracy is required "
                + "(e.g., Kerberos authentication, certificate validity checks). "
                + "Default: absent (OS default 604800 = 1 week for workstations). "
                + "Recommended: 64 on highly accurate or compliance-sensitive deployments.",
            Tags = ["ntp", "poll-interval", "accuracy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "NTP polled every 64 seconds; increases NTP traffic but keeps clock within ±1 second.",
            ApplyOps = [RegOp.SetDword(NtpClient, "SpecialPollInterval", 64)],
            RemoveOps = [RegOp.DeleteValue(NtpClient, "SpecialPollInterval")],
            DetectOps = [RegOp.CheckDword(NtpClient, "SpecialPollInterval", 64)],
        },
        new TweakDef
        {
            Id = "wtime-set-max-pos-phase-correction",
            Label = "Set Maximum Positive Phase Correction to 3600s",
            Category = "Windows Time Policy",
            Description =
                "Sets MaxPosPhaseCorrection=3600 in the W32time Config policy key. "
                + "Limits how far the clock can jump forward in a single correction to 3600 seconds (1 hour). "
                + "Prevents time-jump attacks where an attacker injects a far-future timestamp. "
                + "Default: absent (OS default 48 hours). Recommended: 3600 for security-hardened environments.",
            Tags = ["ntp", "time", "security", "phase-correction", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Clock cannot jump forward more than 1 hour in a single NTP correction; protects against time injection attacks.",
            ApplyOps = [RegOp.SetDword(CfgKey, "MaxPosPhaseCorrection", 3600)],
            RemoveOps = [RegOp.DeleteValue(CfgKey, "MaxPosPhaseCorrection")],
            DetectOps = [RegOp.CheckDword(CfgKey, "MaxPosPhaseCorrection", 3600)],
        },
        new TweakDef
        {
            Id = "wtime-set-max-neg-phase-correction",
            Label = "Set Maximum Negative Phase Correction to 3600s",
            Category = "Windows Time Policy",
            Description =
                "Sets MaxNegPhaseCorrection=3600 in the W32time Config policy key. "
                + "Limits how far the clock can jump backward in a single correction to 3600 seconds (1 hour). "
                + "Prevents time-rollback attacks that could revalidate expired certificates or bypass time-based access controls. "
                + "Default: absent (OS default 48 hours). Recommended: 3600 for certificate-sensitive environments.",
            Tags = ["ntp", "time", "security", "phase-correction", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Clock cannot jump backward more than 1 hour in a single NTP correction; protects against rollback attacks.",
            ApplyOps = [RegOp.SetDword(CfgKey, "MaxNegPhaseCorrection", 3600)],
            RemoveOps = [RegOp.DeleteValue(CfgKey, "MaxNegPhaseCorrection")],
            DetectOps = [RegOp.CheckDword(CfgKey, "MaxNegPhaseCorrection", 3600)],
        },
        new TweakDef
        {
            Id = "wtime-set-update-interval",
            Label = "Set Clock Update Interval to 30000 (30 Seconds)",
            Category = "Windows Time Policy",
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
        new TweakDef
        {
            Id = "wtime-set-phase-correction-rate",
            Label = "Set NTP Phase Correction Rate to 1 (Fast)",
            Category = "Windows Time Policy",
            Description =
                "Sets FrequencyCorrectRate=4 in the W32time Config policy key. "
                + "Controls how aggressively W32tm corrects the local oscillator frequency to match the NTP reference. "
                + "Value 4 (the OS default) represents a balanced correction rate suitable for most workstations. "
                + "Setting explicitly via policy prevents drift caused by third-party tools resetting this to slower values. "
                + "Default: absent. Recommended: 4.",
            Tags = ["ntp", "time", "frequency", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "Frequency correction rate pinned to 4 (OS default); prevents third-party tools from changing it.",
            ApplyOps = [RegOp.SetDword(CfgKey, "FrequencyCorrectRate", 4)],
            RemoveOps = [RegOp.DeleteValue(CfgKey, "FrequencyCorrectRate")],
            DetectOps = [RegOp.CheckDword(CfgKey, "FrequencyCorrectRate", 4)],
        },
        new TweakDef
        {
            Id = "wtime-set-spike-watchdog",
            Label = "Enable NTP Spike Watchdog Protection",
            Category = "Windows Time Policy",
            Description =
                "Sets SpikeWatchPeriod=900 in the W32time Config policy key (900 seconds = 15 minutes). "
                + "Sets the window during which W32tm detects and ignores suspicious time spike samples "
                + "from NTP servers — large, sudden deviations that may indicate NTP spoofing or misconfigured servers. "
                + "Default: absent (W32tm uses a shorter default window). "
                + "Recommended: 900 to extend spike detection for high-value machines.",
            Tags = ["ntp", "time", "security", "spike", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Suspicious NTP spikes are ignored for 15 minutes before re-evaluating; hardens against NTP injection.",
            ApplyOps = [RegOp.SetDword(CfgKey, "SpikeWatchPeriod", 900)],
            RemoveOps = [RegOp.DeleteValue(CfgKey, "SpikeWatchPeriod")],
            DetectOps = [RegOp.CheckDword(CfgKey, "SpikeWatchPeriod", 900)],
        },
    ];
}
