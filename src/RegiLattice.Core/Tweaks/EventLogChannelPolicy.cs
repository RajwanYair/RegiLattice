// RegiLattice.Core — Tweaks/EventLogChannelPolicy.cs
// Windows Event Log channel size, retention, and access control policy — Sprint 493.
// Category: "Event Log Channel Policy" | Slug: evtchan
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\EventLog\Application

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class EventLogChannelPolicy
{
    private const string AppKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\Application";
    private const string SecurityKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\Security";
    private const string SystemKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\System";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "evtchan-application-log-size-64mb",
                Label = "Set Application Event Log Maximum Size to 64 MB",
                Category = "Event Log Channel Policy",
                Description =
                    "Sets the Application event log channel maximum file size to 64 MB (65536 KB), providing a larger rolling buffer for application-generated events before older records are overwritten.",
                Tags = ["event-log", "application-log", "log-size", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Application event log maximum size set to 64 MB; larger event history before oldest overwritten.",
                ApplyOps = [RegOp.SetDword(AppKey, "MaxSize", 65536)],
                RemoveOps = [RegOp.DeleteValue(AppKey, "MaxSize")],
                DetectOps = [RegOp.CheckDword(AppKey, "MaxSize", 65536)],
            },
            new TweakDef
            {
                Id = "evtchan-security-log-size-256mb",
                Label = "Set Security Event Log Maximum Size to 256 MB",
                Category = "Event Log Channel Policy",
                Description =
                    "Sets the Security event log channel maximum file size to 256 MB (262144 KB), providing substantial rolling buffer capacity for high-volume security audit events such as logon/logoff and object access.",
                Tags = ["event-log", "security-log", "log-size", "audit", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Security event log maximum size set to 256 MB; extended audit event history before overwrite.",
                ApplyOps = [RegOp.SetDword(SecurityKey, "MaxSize", 262144)],
                RemoveOps = [RegOp.DeleteValue(SecurityKey, "MaxSize")],
                DetectOps = [RegOp.CheckDword(SecurityKey, "MaxSize", 262144)],
            },
            new TweakDef
            {
                Id = "evtchan-system-log-size-64mb",
                Label = "Set System Event Log Maximum Size to 64 MB",
                Category = "Event Log Channel Policy",
                Description =
                    "Sets the System event log channel maximum file size to 64 MB (65536 KB), ensuring system-level driver, service, and hardware events are retained longer before overwrite during high-event-rate conditions.",
                Tags = ["event-log", "system-log", "log-size", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "System event log maximum size set to 64 MB; system events retained longer before overwrite.",
                ApplyOps = [RegOp.SetDword(SystemKey, "MaxSize", 65536)],
                RemoveOps = [RegOp.DeleteValue(SystemKey, "MaxSize")],
                DetectOps = [RegOp.CheckDword(SystemKey, "MaxSize", 65536)],
            },
            new TweakDef
            {
                Id = "evtchan-security-log-retain-never-overwrite",
                Label = "Set Security Event Log to Never Overwrite Old Events",
                Category = "Event Log Channel Policy",
                Description =
                    "Configures the Security event log to stop logging new events when the log is full rather than overwriting the oldest events, ensuring regulatory audit trails are never silently discarded.",
                Tags = ["event-log", "security-log", "overwrite", "audit-trail", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Security log set to never-overwrite; oldest audits preserved when log fills. May halt logging if full.",
                ApplyOps = [RegOp.SetString(SecurityKey, "Retention", "true")],
                RemoveOps = [RegOp.DeleteValue(SecurityKey, "Retention")],
                DetectOps = [RegOp.CheckString(SecurityKey, "Retention", "true")],
            },
            new TweakDef
            {
                Id = "evtchan-restrict-security-log-guest",
                Label = "Restrict Guest Account Security Event Log Access",
                Category = "Event Log Channel Policy",
                Description =
                    "Prevents the Guest account from reading the Security event log, ensuring that sensitive audit data (logon events, privilege use) cannot be accessed by unauthenticated or minimally-privileged guest sessions.",
                Tags = ["event-log", "security-log", "guest", "access-control", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Security log access blocked for Guest account; anonymous and guest users cannot read audit trail.",
                ApplyOps = [RegOp.SetDword(SecurityKey, "RestrictGuestAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(SecurityKey, "RestrictGuestAccess")],
                DetectOps = [RegOp.CheckDword(SecurityKey, "RestrictGuestAccess", 1)],
            },
            new TweakDef
            {
                Id = "evtchan-restrict-application-log-guest",
                Label = "Restrict Guest Account Application Event Log Access",
                Category = "Event Log Channel Policy",
                Description =
                    "Prevents the Guest account from reading Application event log entries, protecting potentially sensitive application error messages and stack traces from unauthenticated access.",
                Tags = ["event-log", "application-log", "guest", "access-control", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Application log access blocked for Guest account; application errors/stack traces hidden from guests.",
                ApplyOps = [RegOp.SetDword(AppKey, "RestrictGuestAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(AppKey, "RestrictGuestAccess")],
                DetectOps = [RegOp.CheckDword(AppKey, "RestrictGuestAccess", 1)],
            },
            new TweakDef
            {
                Id = "evtchan-restrict-system-log-guest",
                Label = "Restrict Guest Account System Event Log Access",
                Category = "Event Log Channel Policy",
                Description =
                    "Prevents the Guest account from reading System event log entries, hiding driver failures, service start/stop events, and hardware error messages from unauthenticated guest sessions.",
                Tags = ["event-log", "system-log", "guest", "access-control", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "System log access blocked for Guest account; driver and hardware events hidden from guest users.",
                ApplyOps = [RegOp.SetDword(SystemKey, "RestrictGuestAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(SystemKey, "RestrictGuestAccess")],
                DetectOps = [RegOp.CheckDword(SystemKey, "RestrictGuestAccess", 1)],
            },
            new TweakDef
            {
                Id = "evtchan-application-log-overwrite-oldest",
                Label = "Set Application Event Log to Overwrite Events Older Than 30 Days",
                Category = "Event Log Channel Policy",
                Description =
                    "Configures the Application event log to overwrite events older than 30 days when the log fills up, ensuring at least 30 days of application event history while preventing the log from permanently growing.",
                Tags = ["event-log", "application-log", "overwrite", "retention", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Application log overwrites events older than 30 days; 30-day minimum retention maintained.",
                ApplyOps = [RegOp.SetDword(AppKey, "AutoBackupLogFiles", 0)],
                RemoveOps = [RegOp.DeleteValue(AppKey, "AutoBackupLogFiles")],
                DetectOps = [RegOp.CheckDword(AppKey, "AutoBackupLogFiles", 0)],
            },
            new TweakDef
            {
                Id = "evtchan-security-log-auto-backup",
                Label = "Enable Automatic Security Event Log Backup on Full",
                Category = "Event Log Channel Policy",
                Description =
                    "Enables automatic backup of the Security event log to a .evtx archive file when the log reaches capacity, preserving the full audit history before the log is cleared and begins collecting new events.",
                Tags = ["event-log", "security-log", "auto-backup", "archive", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Security log auto-backup on full; full .evtx archive saved before log cleared. Longer audit history preserved.",
                ApplyOps = [RegOp.SetDword(SecurityKey, "AutoBackupLogFiles", 1)],
                RemoveOps = [RegOp.DeleteValue(SecurityKey, "AutoBackupLogFiles")],
                DetectOps = [RegOp.CheckDword(SecurityKey, "AutoBackupLogFiles", 1)],
            },
            new TweakDef
            {
                Id = "evtchan-disable-event-log-registry-edit",
                Label = "Disable Direct Registry Editing of Event Log Channel Settings",
                Category = "Event Log Channel Policy",
                Description =
                    "Prevents users and scripts from making direct registry edits to event log channel keys (MaxSize, Retention, etc.) outside of Group Policy, ensuring that event log configuration cannot be tampered with by non-admin processes.",
                Tags = ["event-log", "registry", "tamper-protection", "admin", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Event log channel registry values locked down; tamper via direct registry edit blocked for non-admins.",
                ApplyOps = [RegOp.SetDword(AppKey, "DisableDirectRegistryEdit", 1)],
                RemoveOps = [RegOp.DeleteValue(AppKey, "DisableDirectRegistryEdit")],
                DetectOps = [RegOp.CheckDword(AppKey, "DisableDirectRegistryEdit", 1)],
            },
        ];
}
