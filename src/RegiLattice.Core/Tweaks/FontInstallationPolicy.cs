// RegiLattice.Core — Tweaks/FontInstallationPolicy.cs
// Windows font installation, font provider, cloud font, and ClearType policy — Sprint 507.
// Category: "Font Installation Policy" | Slug: fontpol
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\System

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class FontInstallationPolicy
{
    private const string Key      = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";
    private const string CtKey    = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore";
    private const string FontKey  = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Fonts";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id           = "fontpol-block-user-font-install",
            Label        = "Block Standard Users from Installing Fonts",
            Category     = "Font Installation Policy",
            Description  = "Prevents standard (non-administrator) users from installing fonts per-user via the Settings app or drag-and-drop, ensuring font management is controlled by IT and that untrusted fonts (a known exploitation vector) are not installed.",
            Tags         = ["fonts", "installation", "standard-user", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "User-mode font installation blocked; only admins can install fonts system-wide.",
            ApplyOps     = [RegOp.SetDword(Key, "BlockUserFromInstallingFonts", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "BlockUserFromInstallingFonts")],
            DetectOps    = [RegOp.CheckDword(Key, "BlockUserFromInstallingFonts", 1)],
        },
        new TweakDef
        {
            Id           = "fontpol-disable-online-font-provider",
            Label        = "Disable Windows Online Font Provider",
            Category     = "Font Installation Policy",
            Description  = "Disables the Windows Online Font Provider service that streams fonts from Microsoft's cloud on demand, preventing outbound font download requests and ensuring all fonts used are locally installed and auditable.",
            Tags         = ["fonts", "online-provider", "cloud", "privacy", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Online font provider disabled; no fonts streamed from Microsoft cloud. All fonts must be pre-installed.",
            ApplyOps     = [RegOp.SetDword(FontKey, "EnableFontProviders", 0)],
            RemoveOps    = [RegOp.DeleteValue(FontKey, "EnableFontProviders")],
            DetectOps    = [RegOp.CheckDword(FontKey, "EnableFontProviders", 0)],
        },
        new TweakDef
        {
            Id           = "fontpol-disable-font-streaming-uap",
            Label        = "Disable Font Streaming for Universal Apps",
            Category     = "Font Installation Policy",
            Description  = "Prevents Universal Windows Platform (UWP) apps from requesting font streaming from Microsoft's online font provider, ensuring that Store apps cannot silently download fonts as part of rendering pipelines.",
            Tags         = ["fonts", "uwp", "streaming", "cloud", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Font streaming from cloud disabled for UWP apps; fonts must be pre-installed for all Store app rendering.",
            ApplyOps     = [RegOp.SetDword(FontKey, "DisableFontStreamingForUWP", 1)],
            RemoveOps    = [RegOp.DeleteValue(FontKey, "DisableFontStreamingForUWP")],
            DetectOps    = [RegOp.CheckDword(FontKey, "DisableFontStreamingForUWP", 1)],
        },
        new TweakDef
        {
            Id           = "fontpol-block-admin-font-from-web",
            Label        = "Warn Before Installing Fonts Downloaded from the Web",
            Category     = "Font Installation Policy",
            Description  = "Configures Windows to display a security warning when an administrator attempts to install a font file downloaded from the internet, reducing the risk of admins silently installing font files with embedded exploit code.",
            Tags         = ["fonts", "web-download", "security-warning", "admin", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Security warning shown before installing web-downloaded fonts; reduces risk of admin installing malicious fonts.",
            ApplyOps     = [RegOp.SetDword(FontKey, "WarnBeforeInstallingWebFonts", 1)],
            RemoveOps    = [RegOp.DeleteValue(FontKey, "WarnBeforeInstallingWebFonts")],
            DetectOps    = [RegOp.CheckDword(FontKey, "WarnBeforeInstallingWebFonts", 1)],
        },
        new TweakDef
        {
            Id           = "fontpol-block-font-preview",
            Label        = "Block Font Preview in Font Viewer for Untrusted Sources",
            Category     = "Font Installation Policy",
            Description  = "Prevents standard users from previewing font files from untrusted locations in the Font Viewer, reducing the attack surface for font-parsing vulnerabilities triggered simply by previewing a crafted font file.",
            Tags         = ["fonts", "font-preview", "untrusted", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 2,
            SafetyRating = 5,
            ImpactNote   = "Font file preview restricted for untrusted sources; reduces attack surface for malicious font parsing.",
            ApplyOps     = [RegOp.SetDword(FontKey, "BlockFontPreviewFromUntrusted", 1)],
            RemoveOps    = [RegOp.DeleteValue(FontKey, "BlockFontPreviewFromUntrusted")],
            DetectOps    = [RegOp.CheckDword(FontKey, "BlockFontPreviewFromUntrusted", 1)],
        },
        new TweakDef
        {
            Id           = "fontpol-disable-font-telemetry",
            Label        = "Disable Font Provider Telemetry to Microsoft",
            Category     = "Font Installation Policy",
            Description  = "Prevents the Windows font provider service from sending telemetry about font usage, installed font families, and font-related application activity to Microsoft.",
            Tags         = ["fonts", "telemetry", "privacy", "microsoft", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 2,
            SafetyRating = 5,
            ImpactNote   = "Font provider telemetry to Microsoft disabled; font usage statistics not sent to cloud.",
            ApplyOps     = [RegOp.SetDword(FontKey, "DisableFontTelemetry", 1)],
            RemoveOps    = [RegOp.DeleteValue(FontKey, "DisableFontTelemetry")],
            DetectOps    = [RegOp.CheckDword(FontKey, "DisableFontTelemetry", 1)],
        },
        new TweakDef
        {
            Id           = "fontpol-set-font-antialiasing-cleartype",
            Label        = "Enforce ClearType Font Antialiasing for All Users",
            Category     = "Font Installation Policy",
            Description  = "Enforces ClearType sub-pixel antialiasing for all user sessions via policy, overriding per-user font smoothing settings to ensure consistent, high-quality text rendering on all LCD displays in the organisation.",
            Tags         = ["fonts", "cleartype", "antialiasing", "rendering", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 2,
            SafetyRating = 5,
            ImpactNote   = "ClearType antialiasing enforced for all users; consistent sub-pixel rendering across all LCD monitors.",
            ApplyOps     = [RegOp.SetDword(FontKey, "ForceClearType", 1)],
            RemoveOps    = [RegOp.DeleteValue(FontKey, "ForceClearType")],
            DetectOps    = [RegOp.CheckDword(FontKey, "ForceClearType", 1)],
        },
        new TweakDef
        {
            Id           = "fontpol-disable-eudcedit",
            Label        = "Disable Creation of End User Defined Character (EUDC) Fonts",
            Category     = "Font Installation Policy",
            Description  = "Prevents users from creating custom EUDC (End User Defined Character) fonts using the EUDC Editor, which would install per-user font registry entries that are not centrally managed.",
            Tags         = ["fonts", "eudc", "custom-characters", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 2,
            SafetyRating = 5,
            ImpactNote   = "EUDC font creation disabled; users cannot create custom character fonts via EUDC Editor.",
            ApplyOps     = [RegOp.SetDword(FontKey, "DisableEUDCEditor", 1)],
            RemoveOps    = [RegOp.DeleteValue(FontKey, "DisableEUDCEditor")],
            DetectOps    = [RegOp.CheckDword(FontKey, "DisableEUDCEditor", 1)],
        },
        new TweakDef
        {
            Id           = "fontpol-restrict-font-subsetting",
            Label        = "Restrict Font Subsetting to Prevent Embedded Sensitive Data",
            Category     = "Font Installation Policy",
            Description  = "Configures Windows font embedding policy to allow printout-only font embedding, preventing applications from creating documents with fully embedded fonts that could be used to covertly exfiltrate data via font steganography.",
            Tags         = ["fonts", "embedding", "subsetting", "data-exfiltration", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 2,
            SafetyRating = 5,
            ImpactNote   = "Font subsetting restricted to print-only embedding; full font embedding in documents blocked.",
            ApplyOps     = [RegOp.SetDword(FontKey, "RestrictFontSubsetting", 1)],
            RemoveOps    = [RegOp.DeleteValue(FontKey, "RestrictFontSubsetting")],
            DetectOps    = [RegOp.CheckDword(FontKey, "RestrictFontSubsetting", 1)],
        },
        new TweakDef
        {
            Id           = "fontpol-audit-font-install-events",
            Label        = "Audit Font Installation Events in Security Log",
            Category     = "Font Installation Policy",
            Description  = "Enables Security event log entries for every font installation or removal event on the system, providing change-management visibility into font inventory changes for security and compliance auditing.",
            Tags         = ["fonts", "audit", "event-log", "change-management", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 2,
            SafetyRating = 5,
            ImpactNote   = "Font install/remove events logged in Security log; font inventory changes auditable for compliance.",
            ApplyOps     = [RegOp.SetDword(FontKey, "AuditFontInstallEvents", 1)],
            RemoveOps    = [RegOp.DeleteValue(FontKey, "AuditFontInstallEvents")],
            DetectOps    = [RegOp.CheckDword(FontKey, "AuditFontInstallEvents", 1)],
        },
    ];
}
