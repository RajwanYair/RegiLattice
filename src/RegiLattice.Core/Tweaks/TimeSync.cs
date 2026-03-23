// RegiLattice.Core — Tweaks/TimeSync.cs
// Windows Time Service and NTP configuration settings (Sprint 87).
// Slug "time" — HKLM\SYSTEM\CurrentControlSet\Services\W32Time\*.
// Distinct from Boot.cs (boot timing) and System.cs (date/locale format).

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class TimeSync
{
    private const string W32TimeParams = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\W32Time\Parameters";

    private const string W32TimeConfig = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\W32Time\Config";

    private const string W32TimeNtpClient = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\W32Time\TimeProviders\NtpClient";

    private const string W32TimeNtpServer = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\W32Time\TimeProviders\NtpServer";

    private const string TimeZoneInfo = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\TimeZoneInformation";

    private const string VmicTimeProv = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\W32Time\TimeProviders\VMICTimeProvider";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "time-set-ntp-pool-servers",
            Label = "Set NTP Servers to pool.ntp.org",
            Category = "Time Synchronization",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["time", "ntp", "sync", "servers"],
            Description =
                "Sets the NTP time server to the global pool.ntp.org pool, which "
                + "provides geographically distributed, highly available time sources. "
                + "More reliable than the default time.windows.com.",
            ApplyOps = [RegOp.SetString(W32TimeParams, "NtpServer", "0.pool.ntp.org,0x9 1.pool.ntp.org,0x9 2.pool.ntp.org,0x9 3.pool.ntp.org,0x9")],
            RemoveOps = [RegOp.SetString(W32TimeParams, "NtpServer", "time.windows.com,0x9")],
            DetectOps =
            [
                RegOp.CheckString(W32TimeParams, "NtpServer", "0.pool.ntp.org,0x9 1.pool.ntp.org,0x9 2.pool.ntp.org,0x9 3.pool.ntp.org,0x9"),
            ],
        },
        new TweakDef
        {
            Id = "time-set-cloudflare-ntp",
            Label = "Set NTP Server to Cloudflare time.cloudflare.com",
            Category = "Time Synchronization",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["time", "ntp", "cloudflare", "sync"],
            Description =
                "Sets the NTP server to Cloudflare's time.cloudflare.com, which uses "
                + "anycast routing and roughtime for high accuracy. Privacy-respecting "
                + "and does not log queries.",
            ApplyOps = [RegOp.SetString(W32TimeParams, "NtpServer", "time.cloudflare.com,0x9")],
            RemoveOps = [RegOp.SetString(W32TimeParams, "NtpServer", "time.windows.com,0x9")],
            DetectOps = [RegOp.CheckString(W32TimeParams, "NtpServer", "time.cloudflare.com,0x9")],
        },
        new TweakDef
        {
            Id = "time-increase-sync-interval",
            Label = "Increase NTP Sync Interval to 12 Hours",
            Category = "Time Synchronization",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["time", "ntp", "sync", "interval"],
            Description =
                "Increases the NTP polling interval to 43,200 seconds (12 hours). "
                + "Reduces network traffic to the NTP server while still keeping the "
                + "clock accurate for most workstation use cases.",
            ApplyOps = [RegOp.SetDword(W32TimeConfig, "MaxPosPhaseCorrection", 43200)],
            RemoveOps = [RegOp.SetDword(W32TimeConfig, "MaxPosPhaseCorrection", 3600)],
            DetectOps = [RegOp.CheckDword(W32TimeConfig, "MaxPosPhaseCorrection", 43200)],
        },
        new TweakDef
        {
            Id = "time-decrease-sync-interval",
            Label = "Decrease NTP Sync Interval to 30 Minutes (High Accuracy)",
            Category = "Time Synchronization",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["time", "ntp", "sync", "accuracy", "interval"],
            Description =
                "Lowers the minimum NTP polling interval to 1,800 seconds (30 minutes) "
                + "for tighter clock accuracy. Useful for logging servers, stock traders, "
                + "or any system where precise timestamps matter.",
            ApplyOps = [RegOp.SetDword(W32TimeNtpClient, "SpecialPollInterval", 1800)],
            RemoveOps = [RegOp.SetDword(W32TimeNtpClient, "SpecialPollInterval", 3600)],
            DetectOps = [RegOp.CheckDword(W32TimeNtpClient, "SpecialPollInterval", 1800)],
        },
        new TweakDef
        {
            Id = "time-enable-ntp-client",
            Label = "Enable NTP Client (W32Time Provider)",
            Category = "Time Synchronization",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["time", "ntp", "client", "enable"],
            Description =
                "Ensures the NTP client time provider is enabled in W32Time. "
                + "Can become disabled if Windows Time Service is partially configured "
                + "or if a third-party time sync tool interfered.",
            ApplyOps = [RegOp.SetDword(W32TimeNtpClient, "Enabled", 1)],
            RemoveOps = [RegOp.DeleteValue(W32TimeNtpClient, "Enabled")],
            DetectOps = [RegOp.CheckDword(W32TimeNtpClient, "Enabled", 1)],
        },
        new TweakDef
        {
            Id = "time-disable-ntp-server",
            Label = "Disable NTP Server Role (Workstation Only)",
            Category = "Time Synchronization",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["time", "ntp", "server", "disable", "security"],
            Description =
                "Disables the Windows Time NTP server role. Non-server Windows machines "
                + "should not act as NTP servers; disabling reduces UDP 123 exposure "
                + "and eliminates NTP amplification DDoS risk.",
            ApplyOps = [RegOp.SetDword(W32TimeNtpServer, "Enabled", 0)],
            RemoveOps = [RegOp.SetDword(W32TimeNtpServer, "Enabled", 1)],
            DetectOps = [RegOp.CheckDword(W32TimeNtpServer, "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "time-enable-utc-hardware-clock",
            Label = "Store Hardware Clock in UTC (Linux Dual-Boot)",
            Category = "Time Synchronization",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 3,
            SafetyRating = 4,
            Tags = ["time", "utc", "rtc", "dual boot", "linux"],
            Description =
                "Configures Windows to treat the hardware (RTC) clock as UTC rather than "
                + "local time. Required for dual-boot systems with Linux to prevent time "
                + "drift between OS sessions.",
            ApplyOps = [RegOp.SetDword(TimeZoneInfo, "RealTimeIsUniversal", 1)],
            RemoveOps = [RegOp.DeleteValue(TimeZoneInfo, "RealTimeIsUniversal")],
            DetectOps = [RegOp.CheckDword(TimeZoneInfo, "RealTimeIsUniversal", 1)],
        },
        new TweakDef
        {
            Id = "time-set-type-ntp",
            Label = "Set W32Time to Use NTP (Internet Sync)",
            Category = "Time Synchronization",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["time", "ntp", "type", "sync", "w32time"],
            Description =
                "Sets the Windows Time Service type to 'NTP', enabling internet-based "
                + "synchronisation. The default may be 'NT5DS' (domain hierarchy sync) "
                + "on domain-joined machines. Switch to NTP on standalone workstations.",
            ApplyOps = [RegOp.SetString(W32TimeParams, "Type", "NTP")],
            RemoveOps = [RegOp.SetString(W32TimeParams, "Type", "NT5DS")],
            DetectOps = [RegOp.CheckString(W32TimeParams, "Type", "NTP")],
        },
        new TweakDef
        {
            Id = "time-increase-max-neg-correction",
            Label = "Increase Max Negative Time Correction to 24 Hours",
            Category = "Time Synchronization",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 2,
            SafetyRating = 4,
            Tags = ["time", "correction", "drift", "sync"],
            Description =
                "Allows W32Time to step the clock backwards by up to 86,400 seconds "
                + "(24 h). By default, very large negative corrections are rejected. "
                + "Useful after hibernation or VM resume when the clock has drifted far.",
            ApplyOps = [RegOp.SetDword(W32TimeConfig, "MaxNegPhaseCorrection", 86400)],
            RemoveOps = [RegOp.SetDword(W32TimeConfig, "MaxNegPhaseCorrection", 3600)],
            DetectOps = [RegOp.CheckDword(W32TimeConfig, "MaxNegPhaseCorrection", 86400)],
        },
        new TweakDef
        {
            Id = "time-set-crosssite-sync-flags",
            Label = "Set NTP Cross-Site Sync Flags (AllFlags)",
            Category = "Time Synchronization",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["time", "ntp", "cross-site", "flags", "sync"],
            Description =
                "Sets the NTP client CrossSiteSyncFlags to 2 (UseAnyDomainController), "
                + "allowing the client to sync from domain controllers in other sites "
                + "when the local site DC is unavailable. Improves time sync reliability.",
            ApplyOps = [RegOp.SetDword(W32TimeNtpClient, "CrossSiteSyncFlags", 2)],
            RemoveOps = [RegOp.DeleteValue(W32TimeNtpClient, "CrossSiteSyncFlags")],
            DetectOps = [RegOp.CheckDword(W32TimeNtpClient, "CrossSiteSyncFlags", 2)],
        },
        new TweakDef
        {
            Id = "time-set-max-pos-phase-correction",
            Label = "Set Maximum Forward Time Correction to 1 Hour",
            Category = "Time Synchronization",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["time", "ntp", "phase", "correction", "sync"],
            Description =
                "Sets the maximum positive phase correction (MaxPosPhaseCorrection) to "
                + "3600 seconds (1 hour). Prevents W32Time from jumping the clock forward by "
                + "more than one hour in a single correction, protecting against rogue NTP servers.",
            ApplyOps = [RegOp.SetDword(W32TimeConfig, "MaxPosPhaseCorrection", 3600)],
            RemoveOps = [RegOp.DeleteValue(W32TimeConfig, "MaxPosPhaseCorrection")],
            DetectOps = [RegOp.CheckDword(W32TimeConfig, "MaxPosPhaseCorrection", 3600)],
        },
        new TweakDef
        {
            Id = "time-set-update-interval",
            Label = "Set W32Time Clock Update Interval (100 000 units = 10 s)",
            Category = "Time Synchronization",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["time", "ntp", "update", "interval", "precision"],
            Description =
                "Sets the W32Time UpdateInterval to 100 000 (100 ms ticks × 100 000 = 10 s). "
                + "Controls how often W32Time updates the local clock when it is in slew mode, "
                + "reducing the time between small, smooth clock adjustments.",
            ApplyOps = [RegOp.SetDword(W32TimeConfig, "UpdateInterval", 100000)],
            RemoveOps = [RegOp.DeleteValue(W32TimeConfig, "UpdateInterval")],
            DetectOps = [RegOp.CheckDword(W32TimeConfig, "UpdateInterval", 100000)],
        },
        new TweakDef
        {
            Id = "time-disable-event-logging",
            Label = "Disable W32Time Verbose Event Log",
            Category = "Time Synchronization",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Tags = ["time", "ntp", "event log", "logging", "privacy"],
            Description =
                "Sets W32Time EventLogFlags to 0, suppressing informational and debug "
                + "entries in the System event log. Errors are still logged. Reduces "
                + "noise in the event log on high-uptime servers.",
            ApplyOps = [RegOp.SetDword(W32TimeConfig, "EventLogFlags", 0)],
            RemoveOps = [RegOp.SetDword(W32TimeConfig, "EventLogFlags", 2)],
            DetectOps = [RegOp.CheckDword(W32TimeConfig, "EventLogFlags", 0)],
        },
        new TweakDef
        {
            Id = "time-set-announce-flags",
            Label = "Set W32Time Announce Flags (Reliable Time Source)",
            Category = "Time Synchronization",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["time", "ntp", "announce", "domain", "server"],
            Description =
                "Sets AnnounceFlags to 5 (0x05 = always reliable, always announce). "
                + "Marks this computer as a reliable time source for domain clients. "
                + "Recommended for PDC emulators and dedicated NTP servers.",
            ApplyOps = [RegOp.SetDword(W32TimeConfig, "AnnounceFlags", 5)],
            RemoveOps = [RegOp.DeleteValue(W32TimeConfig, "AnnounceFlags")],
            DetectOps = [RegOp.CheckDword(W32TimeConfig, "AnnounceFlags", 5)],
        },
        new TweakDef
        {
            Id = "time-set-hold-period",
            Label = "Set W32Time Hold Period (5 Iterations)",
            Category = "Time Synchronization",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["time", "ntp", "hold period", "slew", "stability"],
            Description =
                "Sets HoldPeriod to 5. After a large phase correction, W32Time waits 5 "
                + "poll intervals before adjusting frequencies again. Reduces instability "
                + "from rapid successive corrections.",
            ApplyOps = [RegOp.SetDword(W32TimeConfig, "HoldPeriod", 5)],
            RemoveOps = [RegOp.DeleteValue(W32TimeConfig, "HoldPeriod")],
            DetectOps = [RegOp.CheckDword(W32TimeConfig, "HoldPeriod", 5)],
        },
        new TweakDef
        {
            Id = "time-set-local-clock-dispersion",
            Label = "Set Local Clock Dispersion to 10 Seconds",
            Category = "Time Synchronization",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["time", "ntp", "dispersion", "precision", "peers"],
            Description =
                "Sets LocalClockDispersion to 10 seconds, which is the advertised "
                + "uncertainty of the local clock to NTP peers. Lower values improve "
                + "how this machine is ranked as a time source.",
            ApplyOps = [RegOp.SetDword(W32TimeConfig, "LocalClockDispersion", 10)],
            RemoveOps = [RegOp.DeleteValue(W32TimeConfig, "LocalClockDispersion")],
            DetectOps = [RegOp.CheckDword(W32TimeConfig, "LocalClockDispersion", 10)],
        },
        new TweakDef
        {
            Id = "time-set-large-phase-offset-threshold",
            Label = "Set Large Phase Offset Threshold to 50 ms",
            Category = "Time Synchronization",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["time", "ntp", "large offset", "threshold", "spike"],
            Description =
                "Sets LargePhaseOffset to 50 000 000 (100 ns units = 5 s). When clock "
                + "drift exceeds this threshold, W32Time steps instead of slewing. "
                + "Setting to 50 ms (5 000 000) enables faster response to large drifts.",
            ApplyOps = [RegOp.SetDword(W32TimeConfig, "LargePhaseOffset", 5000000)],
            RemoveOps = [RegOp.DeleteValue(W32TimeConfig, "LargePhaseOffset")],
            DetectOps = [RegOp.CheckDword(W32TimeConfig, "LargePhaseOffset", 5000000)],
        },
        new TweakDef
        {
            Id = "time-enable-ntp-server-provider",
            Label = "Enable NTP Server (Share System Time via UDP/123)",
            Category = "Time Synchronization",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Tags = ["time", "ntp", "server", "share", "udp"],
            Description =
                "Enables the NtpServer time provider, allowing this machine to serve "
                + "time via NTP on UDP port 123. Intended for machines that act as an "
                + "internal NTP server for a local network.",
            ApplyOps = [RegOp.SetDword(W32TimeNtpServer, "Enabled", 1)],
            RemoveOps = [RegOp.SetDword(W32TimeNtpServer, "Enabled", 0)],
            DetectOps = [RegOp.CheckDword(W32TimeNtpServer, "Enabled", 1)],
        },
        new TweakDef
        {
            Id = "time-disable-vmictimeprovider",
            Label = "Disable Hyper-V / VM Integration Time Sync",
            Category = "Time Synchronization",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Tags = ["time", "hyper-v", "vm", "vmic", "virtualization"],
            Description =
                "Disables the VMICTimeProvider that synchronises the clock of Hyper-V "
                + "guest VMs from the host. Use when the guest should use an independent "
                + "NTP source instead of relying on the hypervisor clock.",
            ApplyOps = [RegOp.SetDword(VmicTimeProv, "Enabled", 0)],
            RemoveOps = [RegOp.SetDword(VmicTimeProv, "Enabled", 1)],
            DetectOps = [RegOp.CheckDword(VmicTimeProv, "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "time-set-ntp-poll-interval-1h",
            Label = "Set NTP Client Poll Interval to 1 Hour",
            Category = "Time Synchronization",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["time", "ntp", "poll", "interval", "bandwidth"],
            Description =
                "Sets the NTP client SpecialPollInterval to 3600 seconds (1 hour). "
                + "Reduces NTP traffic on low-bandwidth or metered connections while "
                + "maintaining reasonable time accuracy.",
            ApplyOps = [RegOp.SetDword(W32TimeNtpClient, "SpecialPollInterval", 3600)],
            RemoveOps = [RegOp.DeleteValue(W32TimeNtpClient, "SpecialPollInterval")],
            DetectOps = [RegOp.CheckDword(W32TimeNtpClient, "SpecialPollInterval", 3600)],
        },
    ];
}
