// RegiLattice.Core — Tweaks/PackagedAppDebugPolicy.cs
// Packaged (UWP/MSIX) application debugging machine-scope GPO controls — Sprint 211.
// Controls Developer Mode, sideloading, application debugging capabilities, and debug tokens.
// Category: "Packaged App Debug Policy" | Slug: padebug
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PackagedAppXDebug

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PackagedAppDebugPolicy
{
    private const string Key =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PackagedAppXDebug";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "padebug-disable-developer-mode",
                Label = "Disable Windows Developer Mode",
                Category = "Packaged App Debug Policy",
                Description =
                    "Prevents enabling Windows Developer Mode, which allows sideloading of unsigned or developer-signed MSIX/AppX packages and activates various debug features. Reduces the attack surface on production endpoints. Default: toggle available to users. Recommended: 1.",
                Tags = ["developer-mode", "sideloading", "appx", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Developer Mode toggle in Settings is greyed out; unsigned package sideloading is blocked.",
                ApplyOps = [RegOp.SetDword(Key, "BlockDeveloperMode", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockDeveloperMode")],
                DetectOps = [RegOp.CheckDword(Key, "BlockDeveloperMode", 1)],
            },
            new TweakDef
            {
                Id = "padebug-disable-debuggable-package-install",
                Label = "Block Installation of Debug-Flagged Packages",
                Category = "Packaged App Debug Policy",
                Description =
                    "Prevents installation of MSIX/AppX packages compiled with the debuggable attribute. Debug-flagged packages may expose app internals to debugger attachment without normal authentication. Default: install allowed. Recommended: 1 on production machines.",
                Tags = ["developer-mode", "debuggable", "appx", "package", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "AppX packages with debuggable=true are rejected during install; reduces debugger injection risk.",
                ApplyOps = [RegOp.SetDword(Key, "BlockDebuggablePackageInstall", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockDebuggablePackageInstall")],
                DetectOps = [RegOp.CheckDword(Key, "BlockDebuggablePackageInstall", 1)],
            },
            new TweakDef
            {
                Id = "padebug-disable-test-signing",
                Label = "Disable AppX Test Signing Mode",
                Category = "Packaged App Debug Policy",
                Description =
                    "Prevents Windows from entering AppX test-signing mode that allows packages signed with developer test certificates to execute. Only packages from the Microsoft Store or trusted enterprise signing chains may run. Default: test signing disabled by default on non-dev machines. Recommended: 1.",
                Tags = ["developer-mode", "test-signing", "certificate", "appx", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Packages signed with test certificates are rejected; only Store-signed or enterprise-signed packages run.",
                ApplyOps = [RegOp.SetDword(Key, "DisableTestSigning", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableTestSigning")],
                DetectOps = [RegOp.CheckDword(Key, "DisableTestSigning", 1)],
            },
            new TweakDef
            {
                Id = "padebug-disable-loopback-for-packages",
                Label = "Disable AppX Network Loopback Exemption",
                Category = "Packaged App Debug Policy",
                Description =
                    "Prevents packaged apps from using the network loopback (localhost 127.0.0.1), which is normally blocked by AppContainer isolation. Loopback exemption is a common developer workaround that weakens sandbox isolation. Default: loopback blocked by default. Recommended: 1 to lockdown on production.",
                Tags = ["developer-mode", "loopback", "appcontainer", "sandbox", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "AppX packages cannot access localhost; AppContainer network isolation is fully enforced.",
                ApplyOps = [RegOp.SetDword(Key, "BlockLoopbackExemption", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockLoopbackExemption")],
                DetectOps = [RegOp.CheckDword(Key, "BlockLoopbackExemption", 1)],
            },
            new TweakDef
            {
                Id = "padebug-disable-device-portal",
                Label = "Disable Windows Device Portal",
                Category = "Packaged App Debug Policy",
                Description =
                    "Blocks enabling the Windows Device Portal (a web-based debug interface accessible over Wi-Fi/Ethernet when Developer Mode is on). Eliminates a remote code execution surface. Default: disabled unless Developer Mode is on. Recommended: 1.",
                Tags = ["developer-mode", "device-portal", "remote-access", "web", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Windows Device Portal cannot be enabled; remote debug web service is fully blocked.",
                ApplyOps = [RegOp.SetDword(Key, "BlockDevicePortal", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockDevicePortal")],
                DetectOps = [RegOp.CheckDword(Key, "BlockDevicePortal", 1)],
            },
            new TweakDef
            {
                Id = "padebug-disable-diagnostics-tracking",
                Label = "Block AppX Diagnostic Tracking",
                Category = "Packaged App Debug Policy",
                Description =
                    "Stops packaged apps from submitting debug diagnostics and crash telemetry to the Windows Debug & Diagnostics channel. Prevents app stability data from leaving the device. Default: tracking enabled. Recommended: 1 for data-sovereignty.",
                Tags = ["developer-mode", "diagnostics", "tracking", "telemetry", "appx", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "AppX crash reports and diagnostic telemetry are blocked from leaving the device.",
                ApplyOps = [RegOp.SetDword(Key, "BlockAppDiagnosticsTracking", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockAppDiagnosticsTracking")],
                DetectOps = [RegOp.CheckDword(Key, "BlockAppDiagnosticsTracking", 1)],
            },
            new TweakDef
            {
                Id = "padebug-disable-background-task-debug",
                Label = "Block Background Task Debugger Attachment",
                Category = "Packaged App Debug Policy",
                Description =
                    "Prevents debuggers from attaching to packaged app background task processes. Reduces risk of debugger-based runtime code injection targeting background agents. Default: not restricted. Recommended: 1 on production endpoints.",
                Tags = ["developer-mode", "background-task", "debugger", "injection", "appx", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Debugger cannot attach to AppX background tasks; protects background agents from runtime code injection.",
                ApplyOps = [RegOp.SetDword(Key, "BlockBackgroundTaskDebug", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockBackgroundTaskDebug")],
                DetectOps = [RegOp.CheckDword(Key, "BlockBackgroundTaskDebug", 1)],
            },
            new TweakDef
            {
                Id = "padebug-disable-fiddler-proxy-debug",
                Label = "Block HTTP Debug Proxy for AppX Traffic",
                Category = "Packaged App Debug Policy",
                Description =
                    "Prevents packaged apps from routing their HTTP/HTTPS traffic through a debugging proxy (such as Fiddler). AppContainer typically blocks proxy use; this policy reinforces that restriction. Default: proxy debug blocked. Recommended: 1.",
                Tags = ["developer-mode", "proxy", "fiddler", "appcontainer", "appx", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "AppX HTTP traffic cannot be intercepted via local debug proxies; AppContainer isolation is reinforced.",
                ApplyOps = [RegOp.SetDword(Key, "BlockHttpDebugProxy", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockHttpDebugProxy")],
                DetectOps = [RegOp.CheckDword(Key, "BlockHttpDebugProxy", 1)],
            },
            new TweakDef
            {
                Id = "padebug-enable-package-integrity-check",
                Label = "Enforce AppX Package Integrity Check on Load",
                Category = "Packaged App Debug Policy",
                Description =
                    "Enables cryptographic integrity verification on packaged app binaries at load time. Detects and blocks tampered or patched AppX packages before execution. Default: integrity checks at install time only. Recommended: 1 for high-security deployments.",
                Tags = ["developer-mode", "integrity", "signature", "appx", "anti-tamper", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Binary integrity is checked on every AppX load; tampered packages are blocked before they execute.",
                ApplyOps = [RegOp.SetDword(Key, "EnforcePackageIntegrityOnLoad", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforcePackageIntegrityOnLoad")],
                DetectOps = [RegOp.CheckDword(Key, "EnforcePackageIntegrityOnLoad", 1)],
            },
            new TweakDef
            {
                Id = "padebug-log-sideload-attempts",
                Label = "Audit Log All AppX Sideload Attempts",
                Category = "Packaged App Debug Policy",
                Description =
                    "Records all attempts to sideload (install from outside the Store) AppX/MSIX packages to the Security audit log. Provides forensic visibility into unauthorised package install attempts. Default: not audited. Recommended: 1.",
                Tags = ["developer-mode", "sideload", "audit", "appx", "forensics", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Sideload install attempts are written to the Security log; detectable by SIEM as potential policy violation.",
                ApplyOps = [RegOp.SetDword(Key, "AuditSideloadAttempts", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditSideloadAttempts")],
                DetectOps = [RegOp.CheckDword(Key, "AuditSideloadAttempts", 1)],
            },
        ];
}
