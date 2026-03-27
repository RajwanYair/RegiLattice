// WindowsFlightedFeaturesPolicy.cs — Windows feature-flighting (A/B ring) enforcement
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\FlightedFeatures
// Slug: flight
// Category: Windows Flighted Features Policy

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WindowsFlightedFeaturesPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\FlightedFeatures";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "flight-disable-feature-trials",
            Label = "Windows Flighted Features: Disable Feature Trials",
            Category = "Windows Flighted Features Policy",
            Description =
                "Prevents Windows from enrolling this device in feature trials via the flighting (A/B testing) mechanism. "
                + "Feature trials push experimental or partially-ready features to a subset of devices without user opt-in. "
                + "Disabling trials ensures only fully-released, validated features are active on enterprise endpoints. "
                + "Removing this policy re-enables Microsoft's ability to push feature trial updates.",
            Tags = ["flighting", "feature-trial", "stability", "enterprise", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnabledFlightedFeatures", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnabledFlightedFeatures")],
            DetectOps = [RegOp.CheckDword(Key, "EnabledFlightedFeatures", 0)],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Prevents experimental feature roll-outs; improves endpoint stability and predictability.",
        },
        new TweakDef
        {
            Id = "flight-block-preview-builds",
            Label = "Windows Flighted Features: Block Preview Build Features",
            Category = "Windows Flighted Features Policy",
            Description =
                "Prevents preview-ring feature flags from being activated on production endpoints via the flighting registry policy. "
                + "Preview builds may include unstable code paths, driver compatibility issues, or features not yet hardened for enterprise use. "
                + "Blocking preview feature activation is a standard practice in SOE (Standard Operating Environment) management. "
                + "Removing this policy allows Microsoft flighting to selectively enable preview features on this device.",
            Tags = ["flighting", "preview", "windows-update", "stability", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisablePreviewFeatures", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisablePreviewFeatures")],
            DetectOps = [RegOp.CheckDword(Key, "DisablePreviewFeatures", 1)],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Blocks preview feature activation; reduces risk of unstable code on production machines.",
        },
        new TweakDef
        {
            Id = "flight-set-branch-readiness-semi-annual",
            Label = "Windows Flighted Features: Set Branch Readiness to Semi-Annual Channel",
            Category = "Windows Flighted Features Policy",
            Description =
                "Configures the Windows flighting branch readiness level to the Semi-Annual Channel (production ring). "
                + "The branch readiness level controls which update ring the device belongs to — insider, beta, or release. "
                + "Semi-Annual Channel (value 32) ensures only fully validated updates are offered. "
                + "Removing this policy allows Windows Update to assign the device to its default ring.",
            Tags = ["flighting", "branch", "update-ring", "semi-annual", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "BranchReadinessLevel", 32)],
            RemoveOps = [RegOp.DeleteValue(Key, "BranchReadinessLevel")],
            DetectOps = [RegOp.CheckDword(Key, "BranchReadinessLevel", 32)],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Locks device to Semi-Annual Channel; prevents insider/beta feature ring enrollment.",
        },
        new TweakDef
        {
            Id = "flight-disable-diagnostic-data-upload",
            Label = "Windows Flighted Features: Disable Diagnostic Data Upload for Flights",
            Category = "Windows Flighted Features Policy",
            Description =
                "Disables the upload of diagnostic data specifically associated with flighted (experimental) feature usage. "
                + "When a feature trial is active, Windows collects enhanced telemetry to evaluate the trial's effectiveness. "
                + "This policy stops that additional telemetry while still permitting baseline diagnostic data. "
                + "Removing this policy re-enables flight-specific enhanced diagnostic data collection.",
            Tags = ["flighting", "telemetry", "privacy", "diagnostic-data", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableFlightDiagnosticData", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableFlightDiagnosticData")],
            DetectOps = [RegOp.CheckDword(Key, "DisableFlightDiagnosticData", 1)],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Reduces telemetry associated with feature trials; improves privacy on managed endpoints.",
        },
        new TweakDef
        {
            Id = "flight-disable-experimentation",
            Label = "Windows Flighted Features: Disable A/B Experimentation",
            Category = "Windows Flighted Features Policy",
            Description =
                "Prevents Windows from applying A/B experimentation overrides via the flighting system. "
                + "A/B experimentation can silently change UI layouts, default settings, or feature availability without the user's knowledge. "
                + "On managed endpoints, unpredictable behaviour changes caused by experiments can interfere with helpdesk scripts and SOE policies. "
                + "Removing this policy re-allows A/B experiments to be applied to this device.",
            Tags = ["flighting", "experimentation", "ab-test", "enterprise", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableExperimentation", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableExperimentation")],
            DetectOps = [RegOp.CheckDword(Key, "DisableExperimentation", 1)],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Prevents silent A/B experiments; ensures consistent, predictable Windows behaviour.",
        },
        new TweakDef
        {
            Id = "flight-set-target-release-version",
            Label = "Windows Flighted Features: Set Target Release Version (24H2)",
            Category = "Windows Flighted Features Policy",
            Description =
                "Pins the device to Windows 11 24H2 as the target feature update version via the flighting policy. "
                + "Pinning the target release prevents automatic upgrade to newer feature releases before IT validation is complete. "
                + "This is critical in environments with validated SOE images and application compatibility dependencies. "
                + "Removing this policy allows Windows Update to offer the next feature release when available.",
            Tags = ["flighting", "target-release", "version-pin", "feature-update", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "TargetReleaseVersion", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "TargetReleaseVersion")],
            DetectOps = [RegOp.CheckDword(Key, "TargetReleaseVersion", 1)],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Pins device to a target release; prevents unvetted feature update upgrades.",
        },
        new TweakDef
        {
            Id = "flight-disable-insider-content",
            Label = "Windows Flighted Features: Disable Insider Tip Content",
            Category = "Windows Flighted Features Policy",
            Description =
                "Blocks Windows Insider tip and promotional content pushed via the flighting infrastructure. "
                + "Insider tips are shown in Start, Tips app, and Settings to encourage enrollment in the Insider Program. "
                + "On enterprise endpoints this content is irrelevant and can distract users from productivity. "
                + "Removing this policy re-enables Insider tip content delivery via the flighting system.",
            Tags = ["flighting", "insider", "tips", "content", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableInsiderContent", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableInsiderContent")],
            DetectOps = [RegOp.CheckDword(Key, "DisableInsiderContent", 1)],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Suppresses Insider Program promotional content; cleaner enterprise desktop experience.",
        },
        new TweakDef
        {
            Id = "flight-disable-rollback-on-failure",
            Label = "Windows Flighted Features: Disable Automatic Rollback on Flight Failure",
            Category = "Windows Flighted Features Policy",
            Description =
                "Controls whether Windows automatically rolls back a failed flight update without administrator approval. "
                + "Automatic rollback can interfere with change-management processes in enterprise environments where all changes must be audited. "
                + "Disabling automatic rollback requires IT to explicitly approve any reversion action. "
                + "Removing this policy re-enables Windows automatic rollback on flight failure.",
            Tags = ["flighting", "rollback", "change-management", "enterprise", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableRollbackOnFailure", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableRollbackOnFailure")],
            DetectOps = [RegOp.CheckDword(Key, "DisableRollbackOnFailure", 1)],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Prevents automatic silent rollback; keeps change-management audit trail intact.",
        },
        new TweakDef
        {
            Id = "flight-disable-feature-notifications",
            Label = "Windows Flighted Features: Disable Feature Notification Banners",
            Category = "Windows Flighted Features Policy",
            Description =
                "Suppresses notification banners introduced as part of flight updates — new feature announcements, upgrade prompts, and welcome screens. "
                + "Flight-related notifications interrupt workflows and are inappropriate in a managed enterprise environment. "
                + "This policy blocks those banners from appearing regardless of which features are active. "
                + "Removing this policy re-enables flight-driven notification banners.",
            Tags = ["flighting", "notifications", "banners", "ux", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableFeatureNotifications", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableFeatureNotifications")],
            DetectOps = [RegOp.CheckDword(Key, "DisableFeatureNotifications", 1)],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Stops flight-driven notification banners; reduces user interruptions on managed desktops.",
        },
        new TweakDef
        {
            Id = "flight-enforce-production-ring",
            Label = "Windows Flighted Features: Enforce Production Ring Only",
            Category = "Windows Flighted Features Policy",
            Description =
                "Forces the flighting infrastructure to treat this device as production-ring only, blocking all early-access feature assignments. "
                + "In combination with BranchReadinessLevel, this ensures the device cannot be reclassified by Microsoft's backend assignment logic. "
                + "Enforcing production ring is mandatory for PCI-DSS and SOX environments where any change to production systems requires prior approval. "
                + "Removing this policy allows the backend to reclassify the device into any ring.",
            Tags = ["flighting", "production-ring", "compliance", "enterprise", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnforceProductionRing", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnforceProductionRing")],
            DetectOps = [RegOp.CheckDword(Key, "EnforceProductionRing", 1)],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Locks device to production ring permanently; critical for compliance-controlled endpoints.",
        },
    ];
}
