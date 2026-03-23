// RegiLattice.Core — Tweaks/InternetZonePolicy.cs
// Internet Explorer / legacy MSHTML security zone policy hardening (Sprint 129, T8.2).
// Slug "izone" — HKLM Internet Settings Zones\3 (Internet) and root policy key.
// Relevant for MSHTML/WebBrowser-hosted LOB apps, legacy Intranet portals, and Office WebView.
// Distinct from Edge.cs (Chromium-based), BrowserCommon.cs (UX), Security.cs (OS-level).

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class InternetZonePolicy
{
    // Root Internet Settings policy — applies machine-wide
    private const string InetPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\Internet Settings";

    // Zone 3 = Internet (HKLM policy version overrides HKCU)
    private const string Zone3 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\Internet Settings\Zones\3";

    // IE SmartScreen / Phishing Filter
    private const string PhishFilter = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Internet Explorer\PhishingFilter";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "izone-lock-zones-to-machine",
            Label = "Lock Security Zones to Machine Policy",
            Category = "Internet Zone Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["internet", "security", "zones", "policy", "hardening"],
            Description =
                "Forces all Internet security zone settings to be read from HKLM machine policy. "
                + "Users cannot change zone configurations via the Internet Options dialog. "
                + "Essential in managed environments to enforce uniform zone security. "
                + "Security_HKLM_only=1.",
            ApplyOps = [RegOp.SetDword(InetPol, "Security_HKLM_only", 1)],
            RemoveOps = [RegOp.DeleteValue(InetPol, "Security_HKLM_only")],
            DetectOps = [RegOp.CheckDword(InetPol, "Security_HKLM_only", 1)],
        },
        new TweakDef
        {
            Id = "izone-block-activex-internet",
            Label = "Disable ActiveX Controls in Internet Zone",
            Category = "Internet Zone Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["internet", "activex", "security", "zone", "hardening"],
            Description =
                "Disables execution of ActiveX controls and plug-ins in the Internet security zone "
                + "(zone 3, action code 1200 = 3). Prevents drive-by download and ActiveX exploitation. "
                + "Legacy LOB apps using ActiveX must be moved to the Trusted Sites zone.",
            ApplyOps = [RegOp.SetDword(Zone3, "1200", 3)],
            RemoveOps = [RegOp.DeleteValue(Zone3, "1200")],
            DetectOps = [RegOp.CheckDword(Zone3, "1200", 3)],
        },
        new TweakDef
        {
            Id = "izone-block-activescript-internet",
            Label = "Disable Active Scripting in Internet Zone",
            Category = "Internet Zone Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["internet", "scripting", "javascript", "zone", "security"],
            Description =
                "Disables JavaScript and VBScript execution in the Internet security zone for MSHTML "
                + "applications (action code 1400 = 3). Reduces XSS and script-injection attack surface "
                + "for WebView2/MSHTML-hosted content in legacy line-of-business applications.",
            ApplyOps = [RegOp.SetDword(Zone3, "1400", 3)],
            RemoveOps = [RegOp.DeleteValue(Zone3, "1400")],
            DetectOps = [RegOp.CheckDword(Zone3, "1400", 3)],
        },
        new TweakDef
        {
            Id = "izone-prevent-cert-error-bypass",
            Label = "Prevent Users Bypassing Certificate Errors",
            Category = "Internet Zone Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["internet", "certificate", "tls", "security", "ssl"],
            Description =
                "Prevents users from clicking through TLS/SSL certificate errors to continue to HTTPS "
                + "sites with invalid certificates. PreventIgnoreCertErrors=1. Effective defence against "
                + "certificate spoofing and man-in-the-middle downgrade attacks.",
            ApplyOps = [RegOp.SetDword(InetPol, "PreventIgnoreCertErrors", 1)],
            RemoveOps = [RegOp.DeleteValue(InetPol, "PreventIgnoreCertErrors")],
            DetectOps = [RegOp.CheckDword(InetPol, "PreventIgnoreCertErrors", 1)],
        },
        new TweakDef
        {
            Id = "izone-block-auto-file-download",
            Label = "Block Automatic File Download Prompts in Internet Zone",
            Category = "Internet Zone Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["internet", "download", "zone", "security", "policy"],
            Description =
                "Blocks automatic file download prompts triggered by Internet zone content in MSHTML apps "
                + "(action 1803 = 3, disable automatic prompting for file downloads). Targets legacy LOB "
                + "apps — modern browsers manage downloads independently of zone settings.",
            ApplyOps = [RegOp.SetDword(Zone3, "1803", 3)],
            RemoveOps = [RegOp.DeleteValue(Zone3, "1803")],
            DetectOps = [RegOp.CheckDword(Zone3, "1803", 3)],
        },
        new TweakDef
        {
            Id = "izone-enable-smartscreen-legacy",
            Label = "Enable SmartScreen Phishing Filter for Legacy Apps",
            Category = "Internet Zone Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["internet", "smartscreen", "phishing", "filter", "security"],
            Description =
                "Forces the SmartScreen phishing filter on for all zones in the legacy MSHTML engine "
                + "and Office WebBrowser controls. EnabledV9=1 ensures the filter is active regardless "
                + "of per-user settings. Relevant on systems with LOB apps using WebBrowser control.",
            ApplyOps = [RegOp.SetDword(PhishFilter, "EnabledV9", 1)],
            RemoveOps = [RegOp.DeleteValue(PhishFilter, "EnabledV9")],
            DetectOps = [RegOp.CheckDword(PhishFilter, "EnabledV9", 1)],
        },
        new TweakDef
        {
            Id = "izone-block-form-submit-unencrypted",
            Label = "Block Unencrypted Form Submission in Internet Zone",
            Category = "Internet Zone Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["internet", "form", "https", "encryption", "data"],
            Description =
                "Prevents MSHTML-based apps from submitting form data to HTTP (non-HTTPS) endpoints "
                + "from the Internet zone (action 1601 = 3). Stops accidental credential submission to "
                + "unencrypted servers from legacy application WebBrowser controls.",
            ApplyOps = [RegOp.SetDword(Zone3, "1601", 3)],
            RemoveOps = [RegOp.DeleteValue(Zone3, "1601")],
            DetectOps = [RegOp.CheckDword(Zone3, "1601", 3)],
        },
        new TweakDef
        {
            Id = "izone-block-mixed-content",
            Label = "Block Mixed HTTP/HTTPS Content in Internet Zone",
            Category = "Internet Zone Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["internet", "mixed-content", "https", "http", "security"],
            Description =
                "Disables loading of mixed content (HTTP resources inside HTTPS pages) in the Internet "
                + "zone for MSHTML/WebBrowser-hosted content (action 1609 = 3). Prevents downgrade and "
                + "protocol confusion attacks without just prompting.",
            ApplyOps = [RegOp.SetDword(Zone3, "1609", 3)],
            RemoveOps = [RegOp.DeleteValue(Zone3, "1609")],
            DetectOps = [RegOp.CheckDword(Zone3, "1609", 3)],
        },
        new TweakDef
        {
            Id = "izone-block-unsafe-activex-init",
            Label = "Block Unsafe ActiveX Initialisation in Internet Zone",
            Category = "Internet Zone Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["internet", "activex", "signed", "unsafe", "zone"],
            Description =
                "Disables initialisation and scripting of ActiveX controls not marked as safe for "
                + "scripting (APTCA) in the Internet zone (action 1201 = 3). Reduces exploitation "
                + "of legacy ActiveX controls while allowing properly marked-safe controls.",
            ApplyOps = [RegOp.SetDword(Zone3, "1201", 3)],
            RemoveOps = [RegOp.DeleteValue(Zone3, "1201")],
            DetectOps = [RegOp.CheckDword(Zone3, "1201", 3)],
        },
        new TweakDef
        {
            Id = "izone-block-script-clipboard-internet",
            Label = "Block Script Access to Clipboard in Internet Zone",
            Category = "Internet Zone Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["internet", "clipboard", "script", "zone", "security"],
            Description =
                "Prevents scripts running in the Internet security zone from accessing the clipboard "
                + "programmatically (action 1406 = 3 — disable cut/copy/paste via script). Prevents "
                + "malicious web content from stealing clipboard data such as passwords or auth tokens.",
            ApplyOps = [RegOp.SetDword(Zone3, "1406", 3)],
            RemoveOps = [RegOp.DeleteValue(Zone3, "1406")],
            DetectOps = [RegOp.CheckDword(Zone3, "1406", 3)],
        },
    ];
}
