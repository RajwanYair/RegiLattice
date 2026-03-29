// RegiLattice.Core — Tweaks/EtwSessionPolicy.cs
// ETW trace session management, provider registration, auto-logger, and telemetry policy — Sprint 496.
// Category: "ETW Session Policy" | Slug: etwses
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\ETW

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class EtwSessionPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ETW";
    private const string EvtSysKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventSystem";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "etwses-disable-auto-logger-startup",
                Label = "Disable ETW Auto-Logger Sessions at Startup",
                Category = "ETW Session Policy",
                Description =
                    "Prevents ETW auto-logger trace sessions from starting automatically at system boot, reducing the number of persistent trace sessions that consume memory and logging bandwidth during normal operation.",
                Tags = ["etw", "auto-logger", "startup", "performance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "ETW auto-logger startup sessions disabled; fewer background trace sessions at boot.",
                ApplyOps = [RegOp.SetDword(Key, "DisableAutoLoggerAtStartup", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoLoggerAtStartup")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAutoLoggerAtStartup", 1)],
            },
            new TweakDef
            {
                Id = "etwses-block-user-trace-sessions",
                Label = "Block Standard Users from Creating ETW Trace Sessions",
                Category = "ETW Session Policy",
                Description =
                    "Prevents standard (non-administrator) user accounts from creating new ETW trace sessions via StartTrace API, restricting diagnostic trace collection to administrator-initiated sessions only.",
                Tags = ["etw", "trace-session", "standard-user", "admin", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "ETW trace session creation restricted to admins; standard users cannot start new trace sessions.",
                ApplyOps = [RegOp.SetDword(Key, "BlockUserTraceSessions", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockUserTraceSessions")],
                DetectOps = [RegOp.CheckDword(Key, "BlockUserTraceSessions", 1)],
            },
            new TweakDef
            {
                Id = "etwses-disable-wpp-tracing",
                Label = "Disable WPP Software Tracing Buffer Logging",
                Category = "ETW Session Policy",
                Description =
                    "Disables Windows Pre-Processing (WPP) software tracing buffer logging, stopping WPP-instrumented drivers and services from maintaining in-memory circular trace buffers that consume non-paged pool memory.",
                Tags = ["etw", "wpp", "software-tracing", "memory", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "WPP trace buffer logging disabled; WPP-instrumented component tracing stopped, freeing pool memory.",
                ApplyOps = [RegOp.SetDword(Key, "DisableWPPTracing", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableWPPTracing")],
                DetectOps = [RegOp.CheckDword(Key, "DisableWPPTracing", 1)],
            },
            new TweakDef
            {
                Id = "etwses-set-max-trace-sessions-8",
                Label = "Limit Maximum Concurrent ETW Trace Sessions to 8",
                Category = "ETW Session Policy",
                Description =
                    "Sets the maximum number of concurrent ETW trace sessions to 8, reducing resource usage from trace session handle tables and preventing excessive trace session proliferation from misconfigured applications.",
                Tags = ["etw", "max-sessions", "resource-limit", "tracing", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Maximum concurrent ETW trace sessions limited to 8; fewer trace sessions reduces per-session overhead.",
                ApplyOps = [RegOp.SetDword(Key, "MaxTraceSessionCount", 8)],
                RemoveOps = [RegOp.DeleteValue(Key, "MaxTraceSessionCount")],
                DetectOps = [RegOp.CheckDword(Key, "MaxTraceSessionCount", 8)],
            },
            new TweakDef
            {
                Id = "etwses-block-third-party-providers",
                Label = "Block Third-Party ETW Provider Registration",
                Category = "ETW Session Policy",
                Description =
                    "Prevents third-party applications from registering new ETW providers in the system namespace, restricting ETW instrumentation to Microsoft-signed components and reducing the attack surface for provider injection.",
                Tags = ["etw", "provider-registration", "third-party", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Third-party ETW provider registration blocked; only Microsoft-signed ETW providers allowed.",
                ApplyOps = [RegOp.SetDword(Key, "BlockThirdPartyProviderRegistration", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockThirdPartyProviderRegistration")],
                DetectOps = [RegOp.CheckDword(Key, "BlockThirdPartyProviderRegistration", 1)],
            },
            new TweakDef
            {
                Id = "etwses-disable-diagnostic-sessions",
                Label = "Disable Automatic Diagnostic ETW Session Startup",
                Category = "ETW Session Policy",
                Description =
                    "Disables automatic startup of Windows diagnostic ETW sessions (DiagTrack, WdiContextLog, AppModel) that run at boot to support telemetry and diagnostics, reducing process creation overhead and memory footprint.",
                Tags = ["etw", "diagnostic-sessions", "telemetry", "startup-performance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Automatic diagnostic ETW sessions disabled at startup; DiagTrack/WdiContextLog no longer auto-started.",
                ApplyOps = [RegOp.SetDword(Key, "DisableDiagnosticETWSessions", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDiagnosticETWSessions")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDiagnosticETWSessions", 1)],
            },
            new TweakDef
            {
                Id = "etwses-enable-session-audit",
                Label = "Enable ETW Trace Session Creation and Deletion Audit",
                Category = "ETW Session Policy",
                Description =
                    "Enables audit log entries when ETW trace sessions are created or deleted, providing visibility into which processes are setting up system-level event tracing that could be used for monitoring or exfiltration.",
                Tags = ["etw", "audit", "trace-session", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "ETW trace session creation/deletion events audited; trace session activity is logged for review.",
                ApplyOps = [RegOp.SetDword(Key, "AuditTraceSessionActivity", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditTraceSessionActivity")],
                DetectOps = [RegOp.CheckDword(Key, "AuditTraceSessionActivity", 1)],
            },
            new TweakDef
            {
                Id = "etwses-disable-telemetry-reporting",
                Label = "Disable ETW Telemetry Reporting to Microsoft",
                Category = "ETW Session Policy",
                Description =
                    "Prevents the ETW infrastructure from sending trace session statistics and provider usage telemetry to Microsoft, keeping internal diagnostic trace topology and provider utilisation patterns from cloud disclosure.",
                Tags = ["etw", "telemetry", "privacy", "microsoft", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "ETW telemetry to Microsoft disabled; trace session statistics not sent to cloud.",
                ApplyOps = [RegOp.SetDword(Key, "DisableETWTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableETWTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "DisableETWTelemetry", 1)],
            },
            new TweakDef
            {
                Id = "etwses-disable-com-event-system",
                Label = "Disable COM+ Event System ETW Tracing",
                Category = "ETW Session Policy",
                Description =
                    "Disables the COM+ Event System event tracing provider, stopping background COM subscription events from being generated and reducing ETW trace volume on systems where COM+ subscriptions are unused.",
                Tags = ["etw", "com+", "event-system", "tracing", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "COM+ Event System ETW provider disabled; COM subscription events no longer traced.",
                ApplyOps = [RegOp.SetDword(EvtSysKey, "DisableEventSystem", 1)],
                RemoveOps = [RegOp.DeleteValue(EvtSysKey, "DisableEventSystem")],
                DetectOps = [RegOp.CheckDword(EvtSysKey, "DisableEventSystem", 1)],
            },
            new TweakDef
            {
                Id = "etwses-disable-kernel-logger",
                Label = "Disable ETW NT Kernel Logger Trace Session",
                Category = "ETW Session Policy",
                Description =
                    "Disables the ETW NT Kernel Logger trace session that captures system-wide kernel events (process, thread, I/O, network), reducing the background monitoring overhead on production systems not undergoing active diagnostics.",
                Tags = ["etw", "kernel-logger", "performance", "tracing", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "ETW NT Kernel Logger disabled; system-wide kernel event tracing stopped. Impacts some diagnostic tools.",
                ApplyOps = [RegOp.SetDword(Key, "DisableNTKernelLogger", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableNTKernelLogger")],
                DetectOps = [RegOp.CheckDword(Key, "DisableNTKernelLogger", 1)],
            },
        ];
}
