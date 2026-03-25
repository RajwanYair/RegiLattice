// RegiLattice.Core — Tweaks/WebThreatDefensePolicy.cs
// Sprint 270: Web Threat Defense Group Policy (10 tweaks)
// Category: "Web Threat Defense Policy" | Slug: wtd
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WebThreatDefense

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WebThreatDefensePolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WebThreatDefense";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "wtd-disable-service",
            Label = "Disable Web Threat Defense Service",
            Category = "Web Threat Defense Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 3,
            Description =
                "Sets ServiceEnabled=0 in the WebThreatDefense policy key. Disables the "
                + "Windows Web Threat Defense service, which provides reputation-based "
                + "protection for URLs and executables accessed via Edge and other browsers. "
                + "The service contacts Microsoft cloud to evaluate link safety in real time. "
                + "Default: 1 (service enabled). Recommended: 0 when using a third-party "
                + "URL filtering solution or zero-trust network access proxy.",
            Tags = ["web-threat-defense", "smartscreen", "cloud", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "ServiceEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "ServiceEnabled")],
            DetectOps = [RegOp.CheckDword(Key, "ServiceEnabled", 0)],
        },
        new TweakDef
        {
            Id = "wtd-lock-ui",
            Label = "Lock Web Threat Defense UI Toggle",
            Category = "Web Threat Defense Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Sets UILockdown=1 in the WebThreatDefense policy key. Prevents end users "
                + "from toggling the reputation-based protection setting in Windows Security "
                + "→ App & browser control. The toggle remains visible but is greyed out. "
                + "Ensures that the administrator-configured state cannot be changed without "
                + "elevated privileges. Default: 0. Recommended: 1 in managed environments.",
            Tags = ["web-threat-defense", "ui", "lockdown", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "UILockdown", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "UILockdown")],
            DetectOps = [RegOp.CheckDword(Key, "UILockdown", 1)],
        },
        new TweakDef
        {
            Id = "wtd-disable-phishing-filter",
            Label = "Disable Web Threat Defense Phishing Filter",
            Category = "Web Threat Defense Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            Description =
                "Sets DisablePhishingFilter=1 in the WebThreatDefense policy key. Stops "
                + "real-time checks against Microsoft's phishing site list for URLs accessed "
                + "in the browser. Environments using a network proxy, DNS sinkhole, or "
                + "zero-trust access gateway that provides phishing protection at a lower "
                + "level may find this check redundant. "
                + "Default: 0. Recommended: only with compensating network-layer controls.",
            Tags = ["web-threat-defense", "phishing", "filter", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisablePhishingFilter", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisablePhishingFilter")],
            DetectOps = [RegOp.CheckDword(Key, "DisablePhishingFilter", 1)],
        },
        new TweakDef
        {
            Id = "wtd-disable-malicious-url-block",
            Label = "Disable Web Threat Defense Malicious URL Blocking",
            Category = "Web Threat Defense Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            Description =
                "Sets DisableMaliciousURLBlock=1 in the WebThreatDefense policy key. "
                + "Prevents Windows from blocking navigation to URLs that Microsoft's threat "
                + "intelligence has flagged as distributing malware. In research, sandboxed, "
                + "or security-testing environments that intentionally access known-bad URLs, "
                + "this block is an impediment. "
                + "Default: 0. Recommended: only in isolated research environments.",
            Tags = ["web-threat-defense", "malicious-url", "block", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableMaliciousURLBlock", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableMaliciousURLBlock")],
            DetectOps = [RegOp.CheckDword(Key, "DisableMaliciousURLBlock", 1)],
        },
        new TweakDef
        {
            Id = "wtd-disable-download-reputation",
            Label = "Disable Web Threat Defense Download Reputation",
            Category = "Web Threat Defense Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            Description =
                "Sets DisableDownloadReputation=1 in the WebThreatDefense policy key. "
                + "Disables reputation lookups for executables and archives downloaded from "
                + "the internet. Without reputation checks, unsigned or newly-published files "
                + "are no longer blocked automatically. "
                + "Default: 0 (checks enabled). Recommended: 1 only in developer or "
                + "air-gapped environments where cloud lookups are impractical.",
            Tags = ["web-threat-defense", "download", "reputation", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableDownloadReputation", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableDownloadReputation")],
            DetectOps = [RegOp.CheckDword(Key, "DisableDownloadReputation", 1)],
        },
        new TweakDef
        {
            Id = "wtd-disable-cloud-check",
            Label = "Disable Web Threat Defense Cloud Lookup",
            Category = "Web Threat Defense Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            Description =
                "Sets DisableCloudLookup=1 in the WebThreatDefense policy key. Prevents "
                + "the WTD service from contacting Microsoft cloud endpoints to evaluate "
                + "URL reputation at browse time. All evaluations fall back to local lists "
                + "only. Reduces outgoing connections to Microsoft but degrades freshness "
                + "of threat intelligence. "
                + "Default: 0. Recommended: 1 in strict outbound firewall environments.",
            Tags = ["web-threat-defense", "cloud", "lookup", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableCloudLookup", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableCloudLookup")],
            DetectOps = [RegOp.CheckDword(Key, "DisableCloudLookup", 1)],
        },
        new TweakDef
        {
            Id = "wtd-disable-behaviour-monitoring",
            Label = "Disable Web Threat Defense Behaviour Monitoring",
            Category = "Web Threat Defense Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            Description =
                "Sets DisableBehaviourMonitoring=1 in the WebThreatDefense policy key. "
                + "Disables heuristic behaviour analysis of browser sessions performed "
                + "by the Web Threat Defense engine. Behaviour monitoring catches "
                + "zero-day exploits that don't match static URL signatures but adds "
                + "browser overhead. Default: 0. Recommended: 1 when browser performance "
                + "is critical and alternative EDR covers exploit detection.",
            Tags = ["web-threat-defense", "behaviour", "monitoring", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableBehaviourMonitoring", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableBehaviourMonitoring")],
            DetectOps = [RegOp.CheckDword(Key, "DisableBehaviourMonitoring", 1)],
        },
        new TweakDef
        {
            Id = "wtd-disable-telemetry-upload",
            Label = "Disable Web Threat Defense Telemetry Upload",
            Category = "Web Threat Defense Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Sets DisableTelemetryUpload=1 in the WebThreatDefense policy key. Prevents "
                + "the WTD service from uploading URL visit patterns, block events, and "
                + "engine statistics to Microsoft's security telemetry pipeline. This data "
                + "helps improve threat intelligence but is transmitted outside the standard "
                + "diagnostic data consent. Default: 0. Recommended: 1 for privacy.",
            Tags = ["web-threat-defense", "telemetry", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableTelemetryUpload", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableTelemetryUpload")],
            DetectOps = [RegOp.CheckDword(Key, "DisableTelemetryUpload", 1)],
        },
        new TweakDef
        {
            Id = "wtd-disable-enhanced-protection",
            Label = "Disable Web Threat Defense Enhanced Protection Mode",
            Category = "Web Threat Defense Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            Description =
                "Sets DisableEnhancedProtection=1 in the WebThreatDefense policy key. Opts "
                + "the device out of Enhanced Protection mode, which sends more URL data to "
                + "Microsoft for deeper analysis. Standard protection uses local models only; "
                + "enhanced protection requires cloud connectivity and shares browsing context. "
                + "Default: 0 (enhanced enabled when opted in). Recommended: 1 for "
                + "privacy-first configurations.",
            Tags = ["web-threat-defense", "enhanced", "protection", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableEnhancedProtection", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableEnhancedProtection")],
            DetectOps = [RegOp.CheckDword(Key, "DisableEnhancedProtection", 1)],
        },
        new TweakDef
        {
            Id = "wtd-disable-credential-warning",
            Label = "Disable Web Threat Defense Credential Entry Warning",
            Category = "Web Threat Defense Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Sets DisableCredentialWarning=1 in the WebThreatDefense policy key. Turns "
                + "off the browser warning displayed when Windows detects that a user is "
                + "entering corporate credentials on a potentially phishing or non-corporate "
                + "site. In environments where users authenticate via SSO or SAML, these "
                + "warnings can appear falsely on legitimate third-party login pages. "
                + "Default: 0. Recommended: 1 when SSO eliminates manual credential entry.",
            Tags = ["web-threat-defense", "credential", "warning", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableCredentialWarning", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableCredentialWarning")],
            DetectOps = [RegOp.CheckDword(Key, "DisableCredentialWarning", 1)],
        },
    ];
}
