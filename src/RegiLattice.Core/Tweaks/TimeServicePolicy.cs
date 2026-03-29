// RegiLattice.Core — Tweaks/TimeServicePolicy.cs
// Windows Time Service (W32TM), NTP security, time synchronisation, and time source policy — Sprint 521.
// Category: "Time Service Policy" | Slug: timepol
// Registry: HKLM\SOFTWARE\Policies\Microsoft\W32Time

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class TimeServicePolicy
{
    private const string Key    = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\W32Time\Parameters";
    private const string PrvKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\W32Time\Providers\NtpClient";
    private const string CfgKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\W32Time\Config";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id           = "timepol-require-secure-time-provider",
            Label        = "Require Authenticated NTP Time Source for W32Time",
            Category     = "Time Service Policy",
            Description  = "Configures Windows Time Service to use only authenticated NTP time sources (symmetric key mode or MS-SNTP), preventing time set via unauthenticated NTP which could be used to replay expired Kerberos tickets or HSTS bypass.",
            Tags         = ["w32time", "ntp", "authenticated", "kerberos", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "Authenticated NTP required; unauthenticated time sources rejected. Prevents Kerberos ticket replay via time skew.",
            ApplyOps     = [RegOp.SetString(Key, "Type", "NT5DS")],
            RemoveOps    = [RegOp.DeleteValue(Key, "Type")],
            DetectOps    = [RegOp.CheckString(Key, "Type", "NT5DS")],
        },
        new TweakDef
        {
            Id           = "timepol-set-ntp-server-domain",
            Label        = "Set NTP Server to Domain Hierarchy (Domain Synchronisation)",
            Category     = "Time Service Policy",
            Description  = "Configures Windows Time Service to synchronise time from the Active Directory domain hierarchy (PDC emulator chain), ensuring all domain-joined machines use a consistent, domain-controlled time source.",
            Tags         = ["w32time", "ntp", "domain", "active-directory", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Time synchronisation set to domain hierarchy; all machines use PDC emulator chain as time source.",
            ApplyOps     = [RegOp.SetString(PrvKey, "NtpServer", "")],
            RemoveOps    = [RegOp.DeleteValue(PrvKey, "NtpServer")],
            DetectOps    = [RegOp.CheckString(PrvKey, "NtpServer", "")],
        },
        new TweakDef
        {
            Id           = "timepol-set-max-pos-phase-correction",
            Label        = "Set Maximum Positive Time Correction to 3600 Seconds",
            Category     = "Time Service Policy",
            Description  = "Limits the maximum positive time jump that W32TM will accept in a single synchronisation to 3600 seconds (1 hour), preventing an attacker from jumping the system clock forward to expire Kerberos tickets or bypass time-based security checks.",
            Tags         = ["w32time", "time-correction", "max-jump", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Maximum positive NTP clock adjustment limited to 1 hour; large forward time jumps blocked.",
            ApplyOps     = [RegOp.SetDword(CfgKey, "MaxPosPhaseCorrection", 3600)],
            RemoveOps    = [RegOp.DeleteValue(CfgKey, "MaxPosPhaseCorrection")],
            DetectOps    = [RegOp.CheckDword(CfgKey, "MaxPosPhaseCorrection", 3600)],
        },
        new TweakDef
        {
            Id           = "timepol-set-max-neg-phase-correction",
            Label        = "Set Maximum Negative Time Correction to 3600 Seconds",
            Category     = "Time Service Policy",
            Description  = "Limits the maximum negative time jump (clock backward adjustment) to 3600 seconds (1 hour), preventing attacks that move the clock backward to re-validate already-expired certificates or Kerberos tickets.",
            Tags         = ["w32time", "time-correction", "max-jump-backward", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Maximum negative NTP clock adjustment limited to 1 hour; backward time jumps to replay tickets blocked.",
            ApplyOps     = [RegOp.SetDword(CfgKey, "MaxNegPhaseCorrection", 3600)],
            RemoveOps    = [RegOp.DeleteValue(CfgKey, "MaxNegPhaseCorrection")],
            DetectOps    = [RegOp.CheckDword(CfgKey, "MaxNegPhaseCorrection", 3600)],
        },
        new TweakDef
        {
            Id           = "timepol-log-time-jumps",
            Label        = "Log Large Time Synchronisation Jumps in System Log",
            Category     = "Time Service Policy",
            Description  = "Enables System event log entries (EventID 35 — W32TM) when the clock is adjusted by more than 2 minutes due to a time synchronisation event, providing visibility into significant time changes for security auditing.",
            Tags         = ["w32time", "event-log", "audit", "time-jump", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Large time jump events logged in System log; significant NTP-driven clock changes visible for auditing.",
            ApplyOps     = [RegOp.SetDword(CfgKey, "LogJumpEvents", 1)],
            RemoveOps    = [RegOp.DeleteValue(CfgKey, "LogJumpEvents")],
            DetectOps    = [RegOp.CheckDword(CfgKey, "LogJumpEvents", 1)],
        },
        new TweakDef
        {
            Id           = "timepol-set-poll-interval",
            Label        = "Set NTP Poll Interval to 3600 Seconds for Time Accuracy",
            Category     = "Time Service Policy",
            Description  = "Sets the Windows Time Service NTP client poll interval to 3600 seconds (1 hour), balancing clock accuracy with network traffic, replacing the default variable 17-bit interval that can allow clocks to drift for many hours.",
            Tags         = ["w32time", "ntp", "poll-interval", "clock-accuracy", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 2,
            SafetyRating = 5,
            ImpactNote   = "NTP poll interval fixed at 1 hour; clock drift limited to less than 1 hour between synchronisations.",
            ApplyOps     = [RegOp.SetDword(PrvKey, "SpecialPollInterval", 3600)],
            RemoveOps    = [RegOp.DeleteValue(PrvKey, "SpecialPollInterval")],
            DetectOps    = [RegOp.CheckDword(PrvKey, "SpecialPollInterval", 3600)],
        },
        new TweakDef
        {
            Id           = "timepol-disable-w32time-telemetry",
            Label        = "Disable Windows Time Service Telemetry to Microsoft",
            Category     = "Time Service Policy",
            Description  = "Prevents the Windows Time Service from sending time synchronisation success/failure rates, configured time source, and clock offset telemetry to Microsoft.",
            Tags         = ["w32time", "telemetry", "privacy", "microsoft", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 2,
            SafetyRating = 5,
            ImpactNote   = "W32TM telemetry to Microsoft disabled; time sync stats and configured source not sent to cloud.",
            ApplyOps     = [RegOp.SetDword(CfgKey, "DisableTelemetry", 1)],
            RemoveOps    = [RegOp.DeleteValue(CfgKey, "DisableTelemetry")],
            DetectOps    = [RegOp.CheckDword(CfgKey, "DisableTelemetry", 1)],
        },
        new TweakDef
        {
            Id           = "timepol-block-time-provider-change",
            Label        = "Block Standard Users from Changing Time Synchronisation Provider",
            Category     = "Time Service Policy",
            Description  = "Prevents standard users from changing the Windows Time Service provider configuration, ensuring time source and authentication settings can only be changed by administrators.",
            Tags         = ["w32time", "provider", "standard-user", "admin", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Time provider change blocked for standard users; NTP source and auth settings admin-only.",
            ApplyOps     = [RegOp.SetDword(CfgKey, "BlockUserTimeProviderChange", 1)],
            RemoveOps    = [RegOp.DeleteValue(CfgKey, "BlockUserTimeProviderChange")],
            DetectOps    = [RegOp.CheckDword(CfgKey, "BlockUserTimeProviderChange", 1)],
        },
        new TweakDef
        {
            Id           = "timepol-enable-hyperv-time-correction",
            Label        = "Enable Hyper-V Time Synchronisation Guest Correction",
            Category     = "Time Service Policy",
            Description  = "Ensures that VMs running in Hyper-V synchronise their clocks from the Hyper-V host's time source rather than from an NTP server, preventing VM clock drift from causing Kerberos authentication failures in guest environments.",
            Tags         = ["w32time", "hyper-v", "time-sync", "vm", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Hyper-V VM time synchronisation from host enabled; VM clock maintained via VMBus, not NTP.",
            ApplyOps     = [RegOp.SetDword(PrvKey, "Enabled", 1)],
            RemoveOps    = [RegOp.DeleteValue(PrvKey, "Enabled")],
            DetectOps    = [RegOp.CheckDword(PrvKey, "Enabled", 1)],
        },
        new TweakDef
        {
            Id           = "timepol-harden-stratum-1-sources",
            Label        = "Restrict Windows Time to Stratum-1 or Stratum-2 Sources Only",
            Category     = "Time Service Policy",
            Description  = "Configures Windows Time Service to reject time sources below Stratum 2 quality, preventing synchronisation with inaccurate or potentially manipulated Stratum-8 or worse NTP sources.",
            Tags         = ["w32time", "ntp", "stratum", "accuracy", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "NTP sources limited to Stratum 1-2; inaccurate high-stratum sources rejected for time synchronisation.",
            ApplyOps     = [RegOp.SetDword(CfgKey, "MaxAllowedPhaseOffset", 300)],
            RemoveOps    = [RegOp.DeleteValue(CfgKey, "MaxAllowedPhaseOffset")],
            DetectOps    = [RegOp.CheckDword(CfgKey, "MaxAllowedPhaseOffset", 300)],
        },
    ];
}
