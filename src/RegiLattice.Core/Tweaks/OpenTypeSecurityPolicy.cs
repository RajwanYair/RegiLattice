// RegiLattice.Core — Tweaks/OpenTypeSecurityPolicy.cs
// OpenType and TrueType font security, parsing mitigation, and font driver policy — Sprint 508.
// Category: "OpenType Security Policy" | Slug: otfpol
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows NT\MitigationOptions

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class OpenTypeSecurityPolicy
{
    private const string Key     = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\MitigationOptions";
    private const string FontKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\MpEngine";
    private const string GdipKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Fonts";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id           = "otfpol-block-opentype-kernel-parsing",
            Label        = "Block OpenType Font Parsing in the Windows Kernel",
            Category     = "OpenType Security Policy",
            Description  = "Moves OpenType font parsing out of the Windows kernel (win32k.sys) and into a user-mode font parsing process, eliminating kernel-level font parsing vulnerabilities exploitable via specially-crafted font files in web content.",
            Tags         = ["opentype", "font-parsing", "kernel", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 5,
            SafetyRating = 5,
            ImpactNote   = "OpenType kernel parsing disabled; font parsing moved to user-mode — eliminates kernel font exploit surface.",
            ApplyOps     = [RegOp.SetDword(Key, "BlockOpenTypeKernelParser", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "BlockOpenTypeKernelParser")],
            DetectOps    = [RegOp.CheckDword(Key, "BlockOpenTypeKernelParser", 1)],
        },
        new TweakDef
        {
            Id           = "otfpol-disable-legacy-font-drivers",
            Label        = "Disable Loading of Legacy TrueType Font Drivers",
            Category     = "OpenType Security Policy",
            Description  = "Prevents legacy third-party TrueType font drivers from loading in the Windows font subsystem, reducing attack surface from unmaintained or vulnerable font drivers that may contain known CVEs.",
            Tags         = ["truetype", "font-driver", "legacy", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "Legacy TrueType font drivers blocked from loading; only Windows-provided drivers used for font rendering.",
            ApplyOps     = [RegOp.SetDword(GdipKey, "DisableLegacyFontDrivers", 1)],
            RemoveOps    = [RegOp.DeleteValue(GdipKey, "DisableLegacyFontDrivers")],
            DetectOps    = [RegOp.CheckDword(GdipKey, "DisableLegacyFontDrivers", 1)],
        },
        new TweakDef
        {
            Id           = "otfpol-restrict-embedded-font-trusted",
            Label        = "Restrict Embedded Fonts to Trusted Documents Only",
            Category     = "OpenType Security Policy",
            Description  = "Sets the Windows font embedding policy so that embedded fonts in Office and PDF documents are only rendered when the document originates from a trusted location, blocking remote exploitation via malicious embedded fonts in untrusted files.",
            Tags         = ["fonts", "embedded-font", "trusted", "office", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "Embedded fonts rendered only in trusted documents; fonts in untrusted attachments not processed.",
            ApplyOps     = [RegOp.SetDword(GdipKey, "RestrictEmbeddedFontToTrusted", 1)],
            RemoveOps    = [RegOp.DeleteValue(GdipKey, "RestrictEmbeddedFontToTrusted")],
            DetectOps    = [RegOp.CheckDword(GdipKey, "RestrictEmbeddedFontToTrusted", 1)],
        },
        new TweakDef
        {
            Id           = "otfpol-disable-variable-font-web",
            Label        = "Disable Variable Font Loading from Web Content",
            Category     = "OpenType Security Policy",
            Description  = "Prevents loading of OpenType Variable Fonts (OTF/TTF with variation axes) referenced in web content via browser font stacks, reducing the parsing attack surface from variable font table complexity in browser rendering engines.",
            Tags         = ["opentype", "variable-font", "web", "browser", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Variable font loading from web disabled in browser; reduces OTF/TTF parsing surface in browser renderer.",
            ApplyOps     = [RegOp.SetDword(GdipKey, "DisableVariableFontFromWeb", 1)],
            RemoveOps    = [RegOp.DeleteValue(GdipKey, "DisableVariableFontFromWeb")],
            DetectOps    = [RegOp.CheckDword(GdipKey, "DisableVariableFontFromWeb", 1)],
        },
        new TweakDef
        {
            Id           = "otfpol-enable-font-integrity-check",
            Label        = "Enable Font File Integrity Check Before Loading",
            Category     = "OpenType Security Policy",
            Description  = "Enables a Windows Security Health check that verifies the integrity of installed system fonts against known-good checksums before loading, detecting tampering with font files used in critical UI rendering.",
            Tags         = ["fonts", "integrity-check", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Font file integrity verified before loading; tampered system fonts detected before rendering.",
            ApplyOps     = [RegOp.SetDword(GdipKey, "EnableFontIntegrityCheck", 1)],
            RemoveOps    = [RegOp.DeleteValue(GdipKey, "EnableFontIntegrityCheck")],
            DetectOps    = [RegOp.CheckDword(GdipKey, "EnableFontIntegrityCheck", 1)],
        },
        new TweakDef
        {
            Id           = "otfpol-block-remote-font-download-edge",
            Label        = "Block Remote Font Downloads in Microsoft Edge",
            Category     = "OpenType Security Policy",
            Description  = "Prevents Microsoft Edge from downloading and rendering fonts referenced by web page CSS from remote URLs, eliminating an attack vector where crafted web fonts hosted externally could exploit the browser font parser.",
            Tags         = ["fonts", "edge", "remote-font", "css", "browser-security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "Edge blocked from loading remote fonts via CSS; all fonts must be system-installed. May break web typography.",
            ApplyOps     = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "AllowWebFonts", 0)],
            RemoveOps    = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "AllowWebFonts")],
            DetectOps    = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "AllowWebFonts", 0)],
        },
        new TweakDef
        {
            Id           = "otfpol-enable-gdi-font-sandbox",
            Label        = "Enable GDI Font Sandbox in AppContainer Sessions",
            Category     = "OpenType Security Policy",
            Description  = "Enables the GDI+ font rendering sandbox in AppContainer (browser sandboxed renderer) sessions, ensuring that font parsing for sandbox processes occurs in a restricted context rather than directly in win32k.sys.",
            Tags         = ["fonts", "gdi", "sandbox", "appcontainer", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "GDI font sandbox enabled in AppContainer; font parsing for sandbox processes isolated from kernel.",
            ApplyOps     = [RegOp.SetDword(GdipKey, "EnableGDIFontSandbox", 1)],
            RemoveOps    = [RegOp.DeleteValue(GdipKey, "EnableGDIFontSandbox")],
            DetectOps    = [RegOp.CheckDword(GdipKey, "EnableGDIFontSandbox", 1)],
        },
        new TweakDef
        {
            Id           = "otfpol-disable-type1-fonts",
            Label        = "Disable Loading of Legacy Type1 Fonts",
            Category     = "OpenType Security Policy",
            Description  = "Disables support for loading Adobe Type 1 (PostScript) legacy fonts in GDI/GDI+, an aging format with limited security patching, reducing exposure to Type1 font parsing CVEs in the PostScript interpreter.",
            Tags         = ["fonts", "type1", "postscript", "legacy", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 4,
            ImpactNote   = "Type1/PostScript font loading disabled; legacy PS fonts not rendered. Most modern apps use OpenType.",
            ApplyOps     = [RegOp.SetDword(GdipKey, "DisableType1FontRendering", 1)],
            RemoveOps    = [RegOp.DeleteValue(GdipKey, "DisableType1FontRendering")],
            DetectOps    = [RegOp.CheckDword(GdipKey, "DisableType1FontRendering", 1)],
        },
        new TweakDef
        {
            Id           = "otfpol-log-font-parse-failures",
            Label        = "Log Font File Parse Failures for Security Monitoring",
            Category     = "OpenType Security Policy",
            Description  = "Enables event log entries when a font file fails parsing validation (malformed tables, invalid checksums), providing visibility into attempts to load crafted malicious fonts on the endpoint.",
            Tags         = ["fonts", "parse-failure", "event-log", "audit", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Font parse failure events logged; malformed or crafted font load attempts visible in security log.",
            ApplyOps     = [RegOp.SetDword(GdipKey, "LogFontParseFailures", 1)],
            RemoveOps    = [RegOp.DeleteValue(GdipKey, "LogFontParseFailures")],
            DetectOps    = [RegOp.CheckDword(GdipKey, "LogFontParseFailures", 1)],
        },
        new TweakDef
        {
            Id           = "otfpol-disable-font-driver-telemetry",
            Label        = "Disable Font Driver Telemetry Reporting to Microsoft",
            Category     = "OpenType Security Policy",
            Description  = "Prevents the Windows font subsystem from sending font usage, load failure, and driver interaction telemetry to Microsoft, protecting information about installed and loaded fonts from cloud disclosure.",
            Tags         = ["fonts", "driver", "telemetry", "privacy", "microsoft", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 2,
            SafetyRating = 5,
            ImpactNote   = "Font driver telemetry to Microsoft disabled; font load / failure statistics not sent to cloud.",
            ApplyOps     = [RegOp.SetDword(GdipKey, "DisableFontDriverTelemetry", 1)],
            RemoveOps    = [RegOp.DeleteValue(GdipKey, "DisableFontDriverTelemetry")],
            DetectOps    = [RegOp.CheckDword(GdipKey, "DisableFontDriverTelemetry", 1)],
        },
    ];
}
