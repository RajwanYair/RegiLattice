// RegiLattice.Core — Tweaks/EventLogGpoPolicy.cs
// GPO-managed event log channel size and retention policies (Sprint 137).
// Slug "evtgpo" — uses HKLM\SOFTWARE\Policies\Microsoft\Windows\EventLog paths
// (KB units) rather than the Services\EventLog paths (bytes) in EventLogging.cs.
// These settings apply and enforce even when an administrator changes the non-GPO values.

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class EventLogGpoPolicy
{
    private const string GpoEvt = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog";
    private const string App = GpoEvt + @"\Application";
    private const string Sec = GpoEvt + @"\Security";
    private const string Sys = GpoEvt + @"\System";
    private const string Setup = GpoEvt + @"\Setup";
    private const string Forwarded = GpoEvt + @"\ForwardedEvents";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "evtgpo-application-size-128mb",
            Label = "Set Application Event Log Size to 128 MB (GPO)",
            Category = "Event Log Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Sets the Application event log maximum size to 128 MB (131072 KB) via GPO. "
                + "Enforces the size policy even if administrators change the local setting. "
                + "Uses the GPO path Policies\\Windows\\EventLog\\Application\\MaxSize.",
            Tags = ["event log", "application log", "size", "gpo", "audit"],
            RegistryKeys = [App],
            ApplyOps = [RegOp.SetDword(App, "MaxSize", 131072)],
            RemoveOps = [RegOp.DeleteValue(App, "MaxSize")],
            DetectOps = [RegOp.CheckDword(App, "MaxSize", 131072)],
        },
        new TweakDef
        {
            Id = "evtgpo-security-size-1gb",
            Label = "Set Security Event Log Size to 1 GB (GPO)",
            Category = "Event Log Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Sets the Security event log maximum size to 1 GB (1048576 KB) via GPO. "
                + "Recommended for environments with verbose audit policies to avoid overwriting "
                + "forensic evidence. Policies\\Windows\\EventLog\\Security\\MaxSize.",
            Tags = ["event log", "security log", "audit", "forensics", "gpo"],
            RegistryKeys = [Sec],
            ApplyOps = [RegOp.SetDword(Sec, "MaxSize", 1048576)],
            RemoveOps = [RegOp.DeleteValue(Sec, "MaxSize")],
            DetectOps = [RegOp.CheckDword(Sec, "MaxSize", 1048576)],
        },
        new TweakDef
        {
            Id = "evtgpo-system-size-128mb",
            Label = "Set System Event Log Size to 128 MB (GPO)",
            Category = "Event Log Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Sets the System event log maximum size to 128 MB (131072 KB) via GPO. "
                + "Provides adequate retention for driver, service, and system error events "
                + "on busy servers. Policies\\Windows\\EventLog\\System\\MaxSize.",
            Tags = ["event log", "system log", "size", "gpo"],
            RegistryKeys = [Sys],
            ApplyOps = [RegOp.SetDword(Sys, "MaxSize", 131072)],
            RemoveOps = [RegOp.DeleteValue(Sys, "MaxSize")],
            DetectOps = [RegOp.CheckDword(Sys, "MaxSize", 131072)],
        },
        new TweakDef
        {
            Id = "evtgpo-setup-size-64mb",
            Label = "Set Setup Event Log Size to 64 MB (GPO)",
            Category = "Event Log Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Sets the Setup event log maximum size to 64 MB (65536 KB) via GPO. "
                + "Retains Windows feature/update installation history needed for "
                + "troubleshooting failed updates. Policies\\Windows\\EventLog\\Setup\\MaxSize.",
            Tags = ["event log", "setup log", "windows update", "gpo"],
            RegistryKeys = [Setup],
            ApplyOps = [RegOp.SetDword(Setup, "MaxSize", 65536)],
            RemoveOps = [RegOp.DeleteValue(Setup, "MaxSize")],
            DetectOps = [RegOp.CheckDword(Setup, "MaxSize", 65536)],
        },
        new TweakDef
        {
            Id = "evtgpo-forwarded-size-256mb",
            Label = "Set Forwarded Events Log Size to 256 MB (GPO)",
            Category = "Event Log Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Sets the ForwardedEvents log maximum size to 256 MB (262144 KB) via GPO. "
                + "Important for systems acting as WEF (Windows Event Forwarding) subscribers "
                + "that collect events from many remote machines.",
            Tags = ["event log", "forwarded events", "wef", "gpo", "siem"],
            RegistryKeys = [Forwarded],
            ApplyOps = [RegOp.SetDword(Forwarded, "MaxSize", 262144)],
            RemoveOps = [RegOp.DeleteValue(Forwarded, "MaxSize")],
            DetectOps = [RegOp.CheckDword(Forwarded, "MaxSize", 262144)],
        },
        new TweakDef
        {
            Id = "evtgpo-application-overwrite",
            Label = "Overwrite Application Event Log When Full (GPO)",
            Category = "Event Log Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Sets Application event log Retention=0 via GPO, configuring the channel "
                + "to overwrite the oldest events instead of stopping to accept new ones "
                + "when the log is full. Prevents event logging failures.",
            Tags = ["event log", "application log", "retention", "overwrite", "gpo"],
            RegistryKeys = [App],
            ApplyOps = [RegOp.SetDword(App, "Retention", 0)],
            RemoveOps = [RegOp.DeleteValue(App, "Retention")],
            DetectOps = [RegOp.CheckDword(App, "Retention", 0)],
        },
        new TweakDef
        {
            Id = "evtgpo-security-overwrite",
            Label = "Overwrite Security Event Log When Full (GPO)",
            Category = "Event Log Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Sets Security event log Retention=0 via GPO. On systems with CrashOnAuditFail "
                + "disabled, this ensures the security log continues to operate without "
                + "blocking the system when full.",
            Tags = ["event log", "security log", "retention", "overwrite", "gpo"],
            RegistryKeys = [Sec],
            ApplyOps = [RegOp.SetDword(Sec, "Retention", 0)],
            RemoveOps = [RegOp.DeleteValue(Sec, "Retention")],
            DetectOps = [RegOp.CheckDword(Sec, "Retention", 0)],
        },
        new TweakDef
        {
            Id = "evtgpo-system-overwrite",
            Label = "Overwrite System Event Log When Full (GPO)",
            Category = "Event Log Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Sets System event log Retention=0 via GPO. Allows the System log to "
                + "continuously accept new events by overwriting old ones, preventing "
                + "driver/service events from being dropped.",
            Tags = ["event log", "system log", "retention", "overwrite", "gpo"],
            RegistryKeys = [Sys],
            ApplyOps = [RegOp.SetDword(Sys, "Retention", 0)],
            RemoveOps = [RegOp.DeleteValue(Sys, "Retention")],
            DetectOps = [RegOp.CheckDword(Sys, "Retention", 0)],
        },
        new TweakDef
        {
            Id = "evtgpo-setup-overwrite",
            Label = "Overwrite Setup Event Log When Full (GPO)",
            Category = "Event Log Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Sets Setup event log Retention=0 via GPO, allowing the Setup log to "
                + "overwrite old installation/upgrade records when it reaches capacity, "
                + "keeping cumulative update history available.",
            Tags = ["event log", "setup log", "retention", "overwrite", "gpo"],
            RegistryKeys = [Setup],
            ApplyOps = [RegOp.SetDword(Setup, "Retention", 0)],
            RemoveOps = [RegOp.DeleteValue(Setup, "Retention")],
            DetectOps = [RegOp.CheckDword(Setup, "Retention", 0)],
        },
        new TweakDef
        {
            Id = "evtgpo-forwarded-overwrite",
            Label = "Overwrite Forwarded Events Log When Full (GPO)",
            Category = "Event Log Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Sets ForwardedEvents log Retention=0 via GPO. Ensures the Windows Event "
                + "Forwarding collector continues to receive forwarded events even when the "
                + "subscribed log is at capacity.",
            Tags = ["event log", "forwarded events", "wef", "retention", "overwrite", "gpo"],
            RegistryKeys = [Forwarded],
            ApplyOps = [RegOp.SetDword(Forwarded, "Retention", 0)],
            RemoveOps = [RegOp.DeleteValue(Forwarded, "Retention")],
            DetectOps = [RegOp.CheckDword(Forwarded, "Retention", 0)],
        },
    ];
}
