// RegiLattice.Core — Tweaks/WindowsUpdateUsoPolicy.cs
// Windows Update Service Orchestrator (USO) source and connectivity controls (Sprint 598).
// Category: "WU Update Source Policy" | Slug: wuuso
// Key: HKLM\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate
// All tweaks are CorpSafe = true (Group Policy path).

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WindowsUpdateUsoPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "wuuso-block-wu-downloads-metered-network",
            Label = "WU USO: Block Windows Update Downloads on Metered Networks",
            Category = "WU Update Source Policy",
            Description = "Sets AllowAutoWindowsUpdateDownloadOverMeteredNetwork=0 in WU policy. Prevents Windows Update from automatically downloading update packages when the active network connection is marked as metered. " +
                "On mobile devices and machines on cellular or satellite connections, unrestricted WU downloads can exhaust data allowances or incur substantial overage charges. This policy applies to both background and foreground download scenarios.",
            Tags = ["windows-update", "metered", "network", "bandwidth", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Blocks WU auto-downloads on metered connections; prevents data-plan exhaustion on mobile/satellite links.",
            ApplyOps = [RegOp.SetDword(Key, "AllowAutoWindowsUpdateDownloadOverMeteredNetwork", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowAutoWindowsUpdateDownloadOverMeteredNetwork")],
            DetectOps = [RegOp.CheckDword(Key, "AllowAutoWindowsUpdateDownloadOverMeteredNetwork", 0)],
        },
        new TweakDef
        {
            Id = "wuuso-block-temporary-enterprise-feature-drops",
            Label = "WU USO: Block In-Period Temporary Enterprise Feature Drops",
            Category = "WU Update Source Policy",
            Description = "Sets AllowTemporaryEnterpriseFeatureControl=0 in WU policy. Disables the delivery of optional 'temporary enterprise feature' updates — incremental functionality enhancements that Microsoft ships between major version releases. " +
                "These in-period feature drops are not security updates and can change application behaviour mid-support-lifecycle. Blocking them keeps the OS in a stable, enterprise-validated state between planned upgrade windows.",
            Tags = ["windows-update", "features", "enterprise", "stability", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Blocks temporary enterprise feature drops; keeps OS behaviour predictable between scheduled upgrade events.",
            ApplyOps = [RegOp.SetDword(Key, "AllowTemporaryEnterpriseFeatureControl", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowTemporaryEnterpriseFeatureControl")],
            DetectOps = [RegOp.CheckDword(Key, "AllowTemporaryEnterpriseFeatureControl", 0)],
        },
        new TweakDef
        {
            Id = "wuuso-prevent-user-pausing-updates",
            Label = "WU USO: Prevent Users from Pausing Windows Updates",
            Category = "WU Update Source Policy",
            Description = "Sets SetDisablePauseUXAccess=1 in WU policy (AU subkey). Removes the 'Pause Updates' option from the Windows Update settings UI. " +
                "Without this policy, standard users can pause updates for up to 5 weeks, leaving machines unpatched and out of compliance. This is a key control in corporate environments operating under patch management SLAs where user-initiated update deferrals are not permitted.",
            Tags = ["windows-update", "pause", "user", "compliance", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Removes pause updates control from user UI; ensures patch compliance SLAs are not bypassed by users.",
            ApplyOps = [RegOp.SetDword(Key, "SetDisablePauseUXAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "SetDisablePauseUXAccess")],
            DetectOps = [RegOp.CheckDword(Key, "SetDisablePauseUXAccess", 1)],
        },
        new TweakDef
        {
            Id = "wuuso-disable-dual-scan-on-wsus",
            Label = "WU USO: Disable Dual-Scan When WSUS Is Configured",
            Category = "WU Update Source Policy",
            Description = "Sets DisableDualScan=1 in WU policy. When a WSUS server (WUServer) is configured, Windows 10/11 will by default simultaneously scan both the WSUS server and the public Windows Update/Microsoft Update cloud. " +
                "This 'dual scan' allows unapproved updates to arrive from the cloud even when WSUS approval workflows are in place. Disabling dual scan ensures all updates flow exclusively through WSUS, preserving IT update approval control.",
            Tags = ["windows-update", "wsus", "dual-scan", "enterprise", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Blocks cloud WU source when WSUS is configured; enforces WSUS approval pipeline with no cloud bypass.",
            ApplyOps = [RegOp.SetDword(Key, "DisableDualScan", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableDualScan")],
            DetectOps = [RegOp.CheckDword(Key, "DisableDualScan", 1)],
        },
        new TweakDef
        {
            Id = "wuuso-block-internet-wu-when-wsus-active",
            Label = "WU USO: Block Internet Windows Update Access When WSUS Active",
            Category = "WU Update Source Policy",
            Description = "Sets DoNotConnectToWindowsUpdateInternetLocations=1 in WU policy. When active, prevents the WU client from connecting to the public internet endpoints for update detection, metadata, or downloads. " +
                "This is required in air-gapped or WSUS-only environments where all internet traffic is blocked by firewall policy. Without this setting, WU may attempt internet connections that trigger firewall alerts or fail silently and produce misleading update status.",
            Tags = ["windows-update", "wsus", "internet", "air-gapped", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Blocks all public WU internet connections; required for WSUS-only or air-gapped deployment scenarios.",
            ApplyOps = [RegOp.SetDword(Key, "DoNotConnectToWindowsUpdateInternetLocations", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DoNotConnectToWindowsUpdateInternetLocations")],
            DetectOps = [RegOp.CheckDword(Key, "DoNotConnectToWindowsUpdateInternetLocations", 1)],
        },
        new TweakDef
        {
            Id = "wuuso-block-recommended-updates-auto-install",
            Label = "WU USO: Block Automatic Installation of Recommended Updates",
            Category = "WU Update Source Policy",
            Description = "Sets IncludeRecommendedUpdates=0 in WU policy. Prevents Windows Update from automatically installing 'recommended' updates which include non-security improvements, application updates, and optional Windows features. " +
                "In enterprise environments, recommended updates should be reviewed and approved through a patch management process rather than automatically deployed, as they can change application behaviour without a security justification.",
            Tags = ["windows-update", "recommended", "auto-install", "enterprise", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Blocks auto-install of recommended updates; only critical and security updates deploy automatically.",
            ApplyOps = [RegOp.SetDword(Key, "IncludeRecommendedUpdates", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "IncludeRecommendedUpdates")],
            DetectOps = [RegOp.CheckDword(Key, "IncludeRecommendedUpdates", 0)],
        },
        new TweakDef
        {
            Id = "wuuso-allow-only-trusted-publisher-certs",
            Label = "WU USO: Accept Only Updates from Trusted Publisher Certificates",
            Category = "WU Update Source Policy",
            Description = "Sets AcceptTrustedPublisherCerts=1 in WU policy. Configures the WU client to only accept and install updates that are signed by certificates in the machine's Trusted Publishers certificate store. " +
                "This prevents installation of updates signed by untrusted authority chains, which is relevant in WSUS deployments where custom update packages may be published by third parties or internal teams.",
            Tags = ["windows-update", "trusted-publisher", "certificate", "signing", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Only installs updates signed by trusted publisher certificates; guards against malicious WSUS packages.",
            ApplyOps = [RegOp.SetDword(Key, "AcceptTrustedPublisherCerts", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AcceptTrustedPublisherCerts")],
            DetectOps = [RegOp.CheckDword(Key, "AcceptTrustedPublisherCerts", 1)],
        },
        new TweakDef
        {
            Id = "wuuso-block-optional-content-updates",
            Label = "WU USO: Block Optional Windows Content Updates",
            Category = "WU Update Source Policy",
            Description = "Sets AllowOptionalContent=0 in WU policy. Prevents Windows Update from offering and installing optional content packages — these include font packs, additional language components, accessibility features, and recreational apps. " +
                "Optional content updates consume storage and bandwidth and are not security-relevant. Blocking them reduces WU noise and storage footprint on tightly managed enterprise machines.",
            Tags = ["windows-update", "optional", "content", "storage", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Blocks optional Windows content updates; reduces WU bandwidth and storage usage on managed devices.",
            ApplyOps = [RegOp.SetDword(Key, "AllowOptionalContent", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowOptionalContent")],
            DetectOps = [RegOp.CheckDword(Key, "AllowOptionalContent", 0)],
        },
        new TweakDef
        {
            Id = "wuuso-block-featured-software-via-wu",
            Label = "WU USO: Block Automatic Installation of Featured Software",
            Category = "WU Update Source Policy",
            Description = "Sets EnableFeaturedSoftware=0 in WU policy. Stops Windows Update from offering and automatically installing 'featured software' — typically free Microsoft utilities, game trials, and promotional apps. " +
                "Without this setting, WU silently installs marketing-tied software packages that were never requested by the user or IT administrator, increasing the installed application footprint and creating an unexpected change management event.",
            Tags = ["windows-update", "featured", "software", "bloat", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Blocks OEM/Microsoft featured software installs via WU; prevents unsolicited app additions on managed devices.",
            ApplyOps = [RegOp.SetDword(Key, "EnableFeaturedSoftware", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableFeaturedSoftware")],
            DetectOps = [RegOp.CheckDword(Key, "EnableFeaturedSoftware", 0)],
        },
        new TweakDef
        {
            Id = "wuuso-block-policy-driven-other-update-source",
            Label = "WU USO: Force Policy-Driven Update Source for Other Updates",
            Category = "WU Update Source Policy",
            Description = "Sets SetPolicyDrivenUpdateSourceForOtherUpdates=1 in WU policy. Ensures that non-feature, non-quality updates (such as drivers from the 'Other' category in WU) are sourced exclusively through the configured policy-driven update source (WSUS/SCCM). " +
                "Without this setting, updates in the 'Other' category may still be retrieved directly from Microsoft Update regardless of the WSUS or DeliveryOptimization configuration.",
            Tags = ["windows-update", "wsus", "policy-driven", "other-updates", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Routes 'Other' category updates through policy-driven source; closes WSUS bypass for non-standard update types.",
            ApplyOps = [RegOp.SetDword(Key, "SetPolicyDrivenUpdateSourceForOtherUpdates", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "SetPolicyDrivenUpdateSourceForOtherUpdates")],
            DetectOps = [RegOp.CheckDword(Key, "SetPolicyDrivenUpdateSourceForOtherUpdates", 1)],
        },
    ];
}
