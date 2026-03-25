// RegiLattice.Core — Tweaks/WindowsFlightingPolicy.cs
// Sprint 312: Windows Flighting Policy tweaks (10 tweaks)
// Category: "Windows Flighting Policy" | Slug: flight
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PreviewBuilds

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WindowsFlightingPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PreviewBuilds";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "flight-disable-insider-preview",
            Label = "Disable Windows Insider Preview Enrollment",
            Category = "Windows Flighting Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Windows Insider Preview allows users to enroll their devices to receive pre-release Windows builds that are not yet generally available. Disabling Insider Preview enrollment prevents users from opting their devices into receiving unstable pre-release Windows builds. Preview builds may contain unfixed security vulnerabilities, missing patches, or experimental changes not appropriate for production environments. Insider builds do not receive the same security testing as general availability releases creating potential exposure to undisclosed vulnerabilities. Enterprise devices should run only tested and approved Windows builds deployed through managed update processes. Preventing insider enrollment ensures that enterprise endpoints remain on tested stable builds with complete security patch coverage.",
            Tags = ["flighting", "insider", "updates", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableConfigFlighting", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableConfigFlighting")],
            DetectOps = [RegOp.CheckDword(Key, "EnableConfigFlighting", 0)],
        },
        new TweakDef
        {
            Id = "flight-disable-preview-builds",
            Label = "Block Preview Build Installation",
            Category = "Windows Flighting Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Beyond enrollment control, Windows Update can be blocked from offering feature preview builds to enrolled users and devices. Blocking preview build installation provides an additional layer of protection ensuring that preview builds cannot be installed even if enrollment somehow occurs. Preview builds installed on enterprise devices create unsupported configurations that may not receive all security patches. IT change management processes require that OS upgrades be tested and validated before enterprise deployment. Preview builds can change system behavior, remove features, or alter security defaults in ways not accounted for by enterprise security baselines. Blocking preview installations ensures enterprise devices remain on the approved and tested configuration.",
            Tags = ["flighting", "insider", "updates", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AllowBuildPreview", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowBuildPreview")],
            DetectOps = [RegOp.CheckDword(Key, "AllowBuildPreview", 0)],
        },
        new TweakDef
        {
            Id = "flight-disable-config-flighting",
            Label = "Disable Windows Configuration Flighting",
            Category = "Windows Flighting Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Configuration flighting extends beyond build previews to include experimental feature toggles and configuration changes delivered through Microsoft's flighting infrastructure. Disabling configuration flighting prevents Microsoft from remotely toggling experimental Windows features on enterprise endpoints without IT awareness or approval. Configuration changes delivered through flighting can alter security settings, enable or disable features, or change system behavior. Enterprise security baselines assume specific feature configurations and flighting changes can invalidate baseline assumptions. IT must maintain awareness of all configuration changes on managed endpoints to ensure security policy compliance. Disabling configuration flighting ensures that the Windows configuration remains consistent with the IT-tested and approved baseline.",
            Tags = ["flighting", "configuration", "management", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableConfigFlightingForFlights", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableConfigFlightingForFlights")],
            DetectOps = [RegOp.CheckDword(Key, "EnableConfigFlightingForFlights", 0)],
        },
        new TweakDef
        {
            Id = "flight-disable-telemetry-for-flighting",
            Label = "Disable Flighting Telemetry Uploads",
            Category = "Windows Flighting Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Flighting telemetry collects usage and diagnostic data from enrolled devices to help Microsoft evaluate preview feature quality and performance. Disabling flighting telemetry prevents upload of diagnostic and usage data associated with preview feature experiments. Flighting telemetry may include details about enterprise software usage, hardware configuration, and user behavior with experimental features. Sending enterprise endpoint telemetry to external parties without approval may violate enterprise data governance policies. Even on non-enrolled devices some flighting infrastructure may attempt to collect diagnostic data. Disabling flighting telemetry ensures that no preview-associated diagnostic data is transmitted from endpoints.",
            Tags = ["flighting", "telemetry", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableFlightingTelemetry", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableFlightingTelemetry")],
            DetectOps = [RegOp.CheckDword(Key, "DisableFlightingTelemetry", 1)],
        },
        new TweakDef
        {
            Id = "flight-disable-feature-rollout",
            Label = "Disable Gradual Feature Rollout",
            Category = "Windows Flighting Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Gradual feature rollouts deliver new features to a percentage of endpoints before full general availability. Disabling gradual feature rollout prevents selected endpoints from receiving new features ahead of the general release schedule. Endpoints receiving features early may have different behavior from other endpoints complicating support and security assessment. Early feature deployments may not have received complete security review and may expose new attack surfaces before hardening guidance is available. Enterprise environments benefit from predictable feature delivery through managed update processes rather than random selection for early rollout. Disabling gradual rollouts ensures consistent behavior across all enterprise endpoints at all times.",
            Tags = ["flighting", "features", "updates", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableGradualRollout", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableGradualRollout")],
            DetectOps = [RegOp.CheckDword(Key, "DisableGradualRollout", 1)],
        },
        new TweakDef
        {
            Id = "flight-disable-experimental-features",
            Label = "Disable Experimental Feature Flags",
            Category = "Windows Flighting Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Experimental feature flags are toggles that can enable incomplete or tentative features that are not yet ready for general release. Disabling experimental feature flags prevents activation of features that may have security vulnerabilities, instabilities, or incomplete implementations. Experimental features may have bypassed the complete security review process that production features undergo before general availability. Enabling experimental flags can expose endpoints to attack vectors not present in released features without corresponding security guidance. Enterprise endpoints should only run features that have completed the full development, testing, and security review lifecycle. Experimental features can be evaluated in isolated sandbox environments by development and security teams without risk to production endpoints.",
            Tags = ["flighting", "experimental", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableExperimentalFeatures", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableExperimentalFeatures")],
            DetectOps = [RegOp.CheckDword(Key, "DisableExperimentalFeatures", 1)],
        },
        new TweakDef
        {
            Id = "flight-disable-a-b-testing",
            Label = "Disable A/B Feature Testing Participation",
            Category = "Windows Flighting Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Microsoft uses A/B testing to evaluate different user interface designs and feature implementations on randomly selected endpoints. Disabling A/B testing participation prevents endpoints from being selected to receive alternative interface designs or feature variants. A/B test subjects may receive features with different defaults or behaviors that deviate from the enterprise-approved baseline configuration. Security assessments and user training are developed for consistent interface implementations and A/B variants complicate these processes. Product feature changes affecting enterprise workflows should be introduced through IT-managed deployment cycles not random selection. Opting out of A/B testing ensures all enterprise endpoints receive the same consistent default Windows experience.",
            Tags = ["flighting", "ab-testing", "management", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableABTesting", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableABTesting")],
            DetectOps = [RegOp.CheckDword(Key, "DisableABTesting", 1)],
        },
        new TweakDef
        {
            Id = "flight-set-insider-ring",
            Label = "Set Windows Insider Ring to None",
            Category = "Windows Flighting Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Windows Insider has multiple rings from Dev Channel receiving the most experimental builds to Release Preview receiving near-release builds. Setting the insider ring to None ensures the endpoint is not associated with any insider channel and receives only generally available updates. Device assignment to any insider ring makes the endpoint eligible for pre-release builds regardless of other enrollment settings. Enterprise endpoints should not be affiliated with any insider ring to ensure they only receive production-quality builds. Insider ring assignments should be cleared to confirm no residual enrollment state persists from previous configurations. Setting the ring to None combined with disabling enrollment provides defense-in-depth against accidental preview build delivery.",
            Tags = ["flighting", "insider", "updates", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "BranchReadinessLevel", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "BranchReadinessLevel")],
            DetectOps = [RegOp.CheckDword(Key, "BranchReadinessLevel", 0)],
        },
        new TweakDef
        {
            Id = "flight-disable-insider-program-settings",
            Label = "Disable Insider Program Settings Access",
            Category = "Windows Flighting Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "The Insider Program section in Windows Settings provides users with the interface to enroll in or change insider program membership. Disabling access to Insider Program settings removes the user-accessible configuration page that controls insider enrollment. Hiding the settings page prevents accidental or deliberate enrollment by users who are unaware of enterprise policy against insider participation. Settings access removal is a complementary control to the enrollment block policy providing defense-in-depth. Users attempting to enroll through the settings page will receive a policy-blocked message rather than the enrollment interface. Administrative access to insider settings remains available for authorized IT change processes.",
            Tags = ["flighting", "insider", "ui", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableInsiderProgramSettings", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableInsiderProgramSettings")],
            DetectOps = [RegOp.CheckDword(Key, "DisableInsiderProgramSettings", 1)],
        },
        new TweakDef
        {
            Id = "flight-disable-optional-feature-updates",
            Label = "Disable Optional Preview Feature Updates",
            Category = "Windows Flighting Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Windows Update includes optional feature updates that users can choose to install before they become mandatory on a future update schedule. Disabling optional feature updates prevents users from installing new Windows features that have not been tested and approved by IT. Optional feature updates may include security-relevant changes that alter system behavior without IT awareness. Features received through optional updates may not be covered by enterprise security baselines creating undefined risk. Enterprise feature deployment should proceed through IT-managed testing and approval processes with appropriate scheduling. Preventing optional update installation ensures IT maintains control over the timing and content of feature changes on managed endpoints.",
            Tags = ["flighting", "updates", "management", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableOptionalFeatureUpdates", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableOptionalFeatureUpdates")],
            DetectOps = [RegOp.CheckDword(Key, "DisableOptionalFeatureUpdates", 1)],
        },
    ];
}
