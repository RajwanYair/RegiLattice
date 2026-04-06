namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class SecurityTimeService
{
    private const string W32ParamKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\W32time\Parameters";

    private const string TimeProvidersKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\W32time\TimeProviders\NtpClient";

    private const string W32ConfigKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\W32time\Config";

    private const string NtpServerKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\W32time\TimeProviders\NtpServer";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "ntpsec-force-domain-time-sync",
                Label = "Force Domain Hierarchy NTP Time Source",
                Category = "Security — Time Service",
                Description =
                    "Forces the W32Time service to synchronise only from the domain hierarchy (NT5DS type). "
                    + "Prevents external NTP manipulation attacks by confining time sources to the domain controller chain. "
                    + "Default: NTP type (public internet servers). Recommended: NT5DS on domain members.",
                Tags = ["w32time", "ntp", "domain", "time-sync", "security", "time-manipulation"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetString(W32ParamKey, "Type", "NT5DS")],
                RemoveOps = [RegOp.DeleteValue(W32ParamKey, "Type")],
                DetectOps = [RegOp.CheckString(W32ParamKey, "Type", "NT5DS")],
            },
            new TweakDef
            {
                Id = "ntpsec-enable-ntpclient",
                Label = "Enable NTP Client (W32tm)",
                Category = "Security — Time Service",
                Description =
                    "Enables the Windows NTP client service to keep system time synchronised. "
                    + "Accurate NTP synchronisation is required for Kerberos authentication (5-minute skew limit). "
                    + "Default: enabled on workstations. Recommended: always enabled.",
                Tags = ["w32time", "ntp", "time-sync", "kerberos", "authentication"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(TimeProvidersKey, "Enabled", 1)],
                RemoveOps = [RegOp.DeleteValue(TimeProvidersKey, "Enabled")],
                DetectOps = [RegOp.CheckDword(TimeProvidersKey, "Enabled", 1)],
            },
            new TweakDef
            {
                Id = "ntpsec-enable-ntpserver",
                Label = "Enable NTP Server Mode on PDC Emulator",
                Category = "Security — Time Service",
                Description =
                    "Enables the Windows NTP server to serve time to domain clients. "
                    + "Required on domain controllers (PDC emulator) to become the authoritative time source for the domain. "
                    + "Default: server disabled on member machines. Recommended: enabled on PDC only.",
                Tags = ["w32time", "ntp", "ntp-server", "pdc", "domain-controller", "authoritative"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(NtpServerKey, "Enabled", 1)],
                RemoveOps = [RegOp.DeleteValue(NtpServerKey, "Enabled")],
                DetectOps = [RegOp.CheckDword(NtpServerKey, "Enabled", 1)],
            },
            new TweakDef
            {
                Id = "ntpsec-max-phase-correction-limit",
                Label = "Limit Maximum NTP Phase Correction",
                Category = "Security — Time Service",
                Description =
                    "Sets the maximum number of seconds the W32Time service will adjust in a single correction step. "
                    + "MaxPosPhaseCorrection=3600 and MaxNegPhaseCorrection=3600 (1 hour) prevent large sudden time jumps "
                    + "that could be used in replay attack windows. Recommended: 3600 (1 hour).",
                Tags = ["w32time", "phase-correction", "time-jump", "replay-attack", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(W32ConfigKey, "MaxPosPhaseCorrection", 3600)],
                RemoveOps = [RegOp.DeleteValue(W32ConfigKey, "MaxPosPhaseCorrection")],
                DetectOps = [RegOp.CheckDword(W32ConfigKey, "MaxPosPhaseCorrection", 3600)],
            },
            new TweakDef
            {
                Id = "ntpsec-max-neg-phase-correction-limit",
                Label = "Limit Maximum Negative NTP Phase Correction",
                Category = "Security — Time Service",
                Description =
                    "Limits the maximum backward time adjustment W32Time may make in a single step to 3600 seconds (1 hour). "
                    + "Prevents adversaries from rolling time back to manipulate Kerberos ticket windows or log timestamps. "
                    + "Default: unlimited. Recommended: 3600.",
                Tags = ["w32time", "phase-correction", "time-rollback", "kerberos", "replay"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(W32ConfigKey, "MaxNegPhaseCorrection", 3600)],
                RemoveOps = [RegOp.DeleteValue(W32ConfigKey, "MaxNegPhaseCorrection")],
                DetectOps = [RegOp.CheckDword(W32ConfigKey, "MaxNegPhaseCorrection", 3600)],
            },
            new TweakDef
            {
                Id = "ntpsec-restrict-poll-intervals",
                Label = "Enforce Minimum NTP Poll Interval",
                Category = "Security — Time Service",
                Description =
                    "Sets the minimum NTP polling interval to 64 seconds (2^6). "
                    + "Prevents W32Time from hammering NTP servers with rapid-fire requests, which can indicate a time-based attack probe. "
                    + "Default: variable. Recommended: 6 (64 seconds).",
                Tags = ["w32time", "poll-interval", "ntp", "resource-exhaustion", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(TimeProvidersKey, "SpecialPollInterval", 3600)],
                RemoveOps = [RegOp.DeleteValue(TimeProvidersKey, "SpecialPollInterval")],
                DetectOps = [RegOp.CheckDword(TimeProvidersKey, "SpecialPollInterval", 3600)],
            },
            new TweakDef
            {
                Id = "ntpsec-event-log-flags",
                Label = "Enable W32Time Event Log Verbosity",
                Category = "Security — Time Service",
                Description =
                    "Enables detailed event logging for the Windows Time service into the System event log. "
                    + "EventLogFlags=3 logs both time-change and time-source events, critical for forensic audit of time manipulation. "
                    + "Default: minimal logging. Recommended: 3 (full logging).",
                Tags = ["w32time", "event-log", "audit", "forensic", "time-service"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(W32ConfigKey, "EventLogFlags", 3)],
                RemoveOps = [RegOp.DeleteValue(W32ConfigKey, "EventLogFlags")],
                DetectOps = [RegOp.CheckDword(W32ConfigKey, "EventLogFlags", 3)],
            },
            new TweakDef
            {
                Id = "ntpsec-announce-flags-reliable",
                Label = "Set W32Time as Reliable Time Source",
                Category = "Security — Time Service",
                Description =
                    "Marks the Windows Time service as a reliable time source by setting AnnounceFlags to combined bits. "
                    + "AnnounceFlags=5 marks the service as both a time server and a reliable time source (required for PDC). "
                    + "Default: 10 (non-reliable NTP). Recommended: 5 on PDC emulators.",
                Tags = ["w32time", "announce-flags", "pdc", "reliable", "time-server"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(W32ConfigKey, "AnnounceFlags", 5)],
                RemoveOps = [RegOp.DeleteValue(W32ConfigKey, "AnnounceFlags")],
                DetectOps = [RegOp.CheckDword(W32ConfigKey, "AnnounceFlags", 5)],
            },
            new TweakDef
            {
                Id = "ntpsec-ntpclient-crosssite",
                Label = "Disable Cross-Site NTP Client Queries",
                Category = "Security — Time Service",
                Description =
                    "Prevents the NTP client from querying time sources across Active Directory site boundaries. "
                    + "Restricts time synchronisation to within-site domain controllers, reducing exposure to rogue DCs in remote sites. "
                    + "Default: cross-site queries allowed. Recommended: disabled in multi-site environments.",
                Tags = ["w32time", "ntp", "cross-site", "active-directory", "domain-controller"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(TimeProvidersKey, "CrossSiteSyncFlags", 0)],
                RemoveOps = [RegOp.DeleteValue(TimeProvidersKey, "CrossSiteSyncFlags")],
                DetectOps = [RegOp.CheckDword(TimeProvidersKey, "CrossSiteSyncFlags", 0)],
            },
            new TweakDef
            {
                Id = "ntpsec-spike-watch-period",
                Label = "Set Short Spike Watch Period for Time Anomalies",
                Category = "Security — Time Service",
                Description =
                    "Reduces the spike watch period to 900 seconds. "
                    + "A shorter spike detection window means W32Time reacts faster to sudden time anomalies introduced by rogue servers, "
                    + "limiting the attack window for time-based bypass attacks. "
                    + "Default: 900 seconds. Recommended: explicitly set for audit compliance.",
                Tags = ["w32time", "spike-detection", "time-anomaly", "attack-window", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(W32ConfigKey, "SpikeWatchPeriod", 900)],
                RemoveOps = [RegOp.DeleteValue(W32ConfigKey, "SpikeWatchPeriod")],
                DetectOps = [RegOp.CheckDword(W32ConfigKey, "SpikeWatchPeriod", 900)],
            },
        ];
}
