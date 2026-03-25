// RegiLattice.Core — Tweaks/FontProviderPolicy.cs
// Sprint 302: Font Provider Policy tweaks (10 tweaks)
// Category: "Font Provider Policy" | Slug: fontprov
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\FontProvider

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class FontProviderPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\FontProvider";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "fontprov-disable-online-fonts",
            Label = "Disable Online Font Provider",
            Category = "Font Provider Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "The Windows online font provider downloads fonts from Microsoft's online font store and makes them available to applications through the DirectWrite font API. Disabling the online font provider prevents Windows from connecting to Microsoft's online font service to download cloud-hosted fonts. Online font connections disclose font usage patterns and installed application names to external font services. Enterprise endpoints should only use fonts distributed through managed IT channels rather than dynamically downloading from cloud services. Offline font repositories ensure that document rendering is consistent and does not depend on external network connectivity. Disabling the online provider has no impact on locally installed fonts which continue to function normally.",
            Tags = ["fonts", "online", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableOnlineFontProvider", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableOnlineFontProvider")],
            DetectOps = [RegOp.CheckDword(Key, "DisableOnlineFontProvider", 1)],
        },
        new TweakDef
        {
            Id = "fontprov-disable-font-streaming",
            Label = "Disable Font Streaming",
            Category = "Font Provider Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Font streaming allows Windows to download only the portions of a font file needed for immediate rendering rather than downloading the complete font. While efficient for network bandwidth, font streaming creates persistent outbound connections to external font servers during document rendering. Disabling font streaming prevents incremental font download requests from being made during document rendering operations. Streaming requests expose document content characteristics to font provider infrastructure through the specific glyph ranges requested. In air-gapped or strictly controlled environments, preventing external network requests during document rendering is an important isolation property. Enterprise fonts should be fully installed locally to eliminate any dependency on streaming from external services.",
            Tags = ["fonts", "streaming", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableFontStreaming", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableFontStreaming")],
            DetectOps = [RegOp.CheckDword(Key, "DisableFontStreaming", 1)],
        },
        new TweakDef
        {
            Id = "fontprov-disable-font-download-suggestions",
            Label = "Disable Font Download Suggestions",
            Category = "Font Provider Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Description =
                "Font download suggestions appear in applications when a document contains characters that require fonts not currently installed on the system. Disabling download suggestions prevents Windows from presenting prompts to download fonts from the online store. Font suggestions are consumer-oriented features intended for home use and are not appropriate in enterprise document environments. Suggestion prompts can lead users to inadvertently install fonts from external sources that bypass IT font management. Enterprise font governance requires that all font installations go through the managed software deployment pipeline. Disabling suggestions keeps the user experience consistent and prevents unsanctioned software additions through font installation prompts.",
            Tags = ["fonts", "suggestions", "usability", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableFontDownloadSuggestions", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableFontDownloadSuggestions")],
            DetectOps = [RegOp.CheckDword(Key, "DisableFontDownloadSuggestions", 1)],
        },
        new TweakDef
        {
            Id = "fontprov-disable-update-check",
            Label = "Disable Font Provider Update Checks",
            Category = "Font Provider Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "The font provider periodically checks for updated versions of installed fonts and new font offerings available in the online font catalog. Disabling update checks prevents outbound connections to font provider services to enumerate font catalog changes. Update checks disclose which fonts are installed on the endpoint to external font provider infrastructure. Enterprise patch and update management should handle font updates through controlled channels rather than automatic cloud-driven updates. Preventing update checks reduces unnecessary outbound network connections from managed endpoints. Font rendering and all locally installed font functionality continue normally without font provider update connectivity.",
            Tags = ["fonts", "updates", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableFontProviderUpdateChecks", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableFontProviderUpdateChecks")],
            DetectOps = [RegOp.CheckDword(Key, "DisableFontProviderUpdateChecks", 1)],
        },
        new TweakDef
        {
            Id = "fontprov-disable-auto-install",
            Label = "Disable Font Auto-Installation",
            Category = "Font Provider Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Font auto-installation silently downloads and installs fonts referenced in documents or web pages when the current font set does not include the required typeface. Disabling font auto-installation prevents new font files from being downloaded and installed without explicit administrator approval. Auto-installed fonts from unknown sources may embed malicious content targeting font rendering vulnerabilities. Font parsing vulnerabilities have historically been exploited through specially crafted font files embedded in documents and web pages. Enterprise font libraries should be curated, validated, and distributed by IT to prevent exposure to malicious font files. Disabling auto-installation ensures font additions require deliberate change management.",
            Tags = ["fonts", "installation", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableFontAutoInstall", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableFontAutoInstall")],
            DetectOps = [RegOp.CheckDword(Key, "DisableFontAutoInstall", 1)],
        },
        new TweakDef
        {
            Id = "fontprov-disable-per-user-fonts",
            Label = "Disable Per-User Font Installation",
            Category = "Font Provider Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Per-user font installation allows standard user accounts to install fonts without administrator privileges by placing them in the user font directory. Disabling per-user font installation prevents non-administrator accounts from installing any fonts on the managed endpoint. Standard users installing fonts bypass IT font governance and could introduce malicious fonts targeting applications and rendering pipelines. Enterprise font management standards require administrator-level approval for all software including font installation. Centralized font distribution ensures all users have a consistent and auditable font library on managed endpoints. Disabling per-user installation ensures font changes require change management review and administrator action.",
            Tags = ["fonts", "per-user", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisablePerUserFontInstallation", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisablePerUserFontInstallation")],
            DetectOps = [RegOp.CheckDword(Key, "DisablePerUserFontInstallation", 1)],
        },
        new TweakDef
        {
            Id = "fontprov-disable-font-cache",
            Label = "Disable Font Provider Cache",
            Category = "Font Provider Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "The font provider maintains a cache of recently used fonts to improve rendering performance when font files are streamed or remotely hosted. Disabling the font provider cache forces font data to be retrieved on every rendering operation rather than using cached data. Cache data from remote font providers may contain residual content that reveals document rendering activity. In secure environments where font rendering history should not persist, disabling the cache provides a privacy benefit. Cache invalidation removes stale font data that could cause rendering inconsistencies after font library changes. Locally installed fonts have their own file system cache managed by the OS and are unaffected by this provider cache setting.",
            Tags = ["fonts", "cache", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableFontProviderCache", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableFontProviderCache")],
            DetectOps = [RegOp.CheckDword(Key, "DisableFontProviderCache", 1)],
        },
        new TweakDef
        {
            Id = "fontprov-disable-telemetry",
            Label = "Disable Font Provider Telemetry",
            Category = "Font Provider Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Description =
                "Font provider telemetry reports data about font usage patterns, popular font requests, and download success metrics to Microsoft. This helps improve the online font catalog and font delivery infrastructure for future Windows releases. Disabling font provider telemetry prevents font usage statistics from being transmitted to Microsoft's analytics systems. Font usage patterns can correlate with document types and application usage revealing sensitive business activity information. Consumer-facing analytics telemetry is generally not appropriate in enterprise environments under data governance frameworks. All font rendering and locally installed font functionality continues to operate normally without telemetry.",
            Tags = ["fonts", "telemetry", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableFontProviderTelemetry", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableFontProviderTelemetry")],
            DetectOps = [RegOp.CheckDword(Key, "DisableFontProviderTelemetry", 1)],
        },
        new TweakDef
        {
            Id = "fontprov-disable-emoji-font",
            Label = "Disable Cloud Emoji Font Downloads",
            Category = "Font Provider Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Description =
                "Windows can download updated emoji font data from Microsoft's online services to ensure current emoji rendering support for new Unicode code points. Disabling cloud emoji font downloads prevents automatic retrieval of updated emoji rendering data from external services. Emoji font update connections represent unnecessary outbound network traffic from managed enterprise endpoints. Enterprise communication tools using emoji do not require the latest Unicode version for functional operation. Content filtering and web proxy logs can be simplified by preventing font-related cloud connections from endpoints. Disabling cloud emoji font updates has only cosmetic impact and does not affect any functional capability.",
            Tags = ["fonts", "emoji", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableCloudEmojiFont", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableCloudEmojiFont")],
            DetectOps = [RegOp.CheckDword(Key, "DisableCloudEmojiFont", 1)],
        },
        new TweakDef
        {
            Id = "fontprov-disable-third-party-provider",
            Label = "Disable Third-Party Font Provider Registration",
            Category = "Font Provider Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Windows font provider infrastructure allows third-party vendors to register custom font providers that deliver additional font content. Disabling third-party font provider registration prevents external font service vendors from integrating their font libraries into the Windows font rendering pipeline. Third-party font providers have varying security review standards and may introduce font files from unvalidated sources. Enterprise endpoints should use only the built-in Microsoft font infrastructure with fonts distributed through IT management. Third-party provider code executing in the rendering pipeline creates an additional attack surface for privilege escalation. Disabling third-party providers ensures only verified Microsoft font infrastructure handles font rendering on managed endpoints.",
            Tags = ["fonts", "third-party", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableThirdPartyFontProvider", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableThirdPartyFontProvider")],
            DetectOps = [RegOp.CheckDword(Key, "DisableThirdPartyFontProvider", 1)],
        },
    ];
}
