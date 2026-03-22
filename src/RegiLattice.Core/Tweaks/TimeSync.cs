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
    ];
}
