// RegiLattice.Core — Tweaks/WindowsServicingPolicy.cs
// Windows Update for Business Servicing Policy — deployment ring, deferral, and feature update controls (Sprint 621).
// Category: "Windows Servicing Policy" | Slug: winsvc
// Key: HKLM\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WindowsServicingPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "winsvc-set-target-ga-release-channel",
            Label = "Servicing: Set Windows Update for Business Channel to GA Release Channel",
            Category = "Windows Servicing Policy",
            Description = "Sets TargetReleaseVersionInfo=\"GA\" in WindowsUpdate policy. Configures Windows Update for Business to target the General Availability (GA) channel, ensuring the endpoint only receives fully released Windows 11/10 builds rather than Beta channel, Release Preview builds, or Insider Preview builds, providing the most stable update experience. " +
                "Without an explicit channel configuration, a Windows endpoint may be enrolled in a Windows Insider Program channel from a previous administrator action and continue receiving pre-release builds. Pre-release builds are not covered by the standard Microsoft support lifecycle and may contain known stability regressions. Locking the endpoint to the GA channel ensures only fully supported, production-validated Windows builds are ever installed.",
            Tags = ["windows-servicing", "release-channel", "ga", "insider", "update"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Windows Update locked to GA channel; pre-release Insider and beta builds cannot be installed.",
            ApplyOps = [RegOp.SetString(Key, "TargetReleaseVersionInfo", "GA")],
            RemoveOps = [RegOp.DeleteValue(Key, "TargetReleaseVersionInfo")],
            DetectOps = [RegOp.CheckString(Key, "TargetReleaseVersionInfo", "GA")],
        },
        new TweakDef
        {
            Id = "winsvc-defer-feature-updates-90-days",
            Label = "Servicing: Defer Windows Feature Updates for 90 Days from GA Release",
            Category = "Windows Servicing Policy",
            Description = "Sets DeferFeatureUpdatesPeriodInDays=90 in WindowsUpdate policy. Delays the installation of Windows Feature Updates (major annual or semi-annual releases introducing new OS capabilities) by 90 days from the date they are first made publicly available, giving Microsoft time to issue compatibility fixes and giving IT time to complete validation and application compatibility testing. " +
                "New Windows Feature Updates (e.g., Windows 11 version upgrades) introduce significant changes to the OS, including driver model changes, security changes, and UI modifications. Enterprises that immediately deploy new feature updates (0-day) routinely encounter application compatibility regressions, driver failures for specialised hardware, and Group Policy setting changes that require updated ADMX templates. A 90-day deferral provides buffer for Microsoft to release hotfixes and for enterprise IT to complete testing.",
            Tags = ["windows-servicing", "feature-update", "deferral", "compatibility", "testing"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Feature updates deferred 90 days; Microsoft and IT have time to address compatibility issues before enterprise deployment.",
            ApplyOps = [RegOp.SetDword(Key, "DeferFeatureUpdatesPeriodInDays", 90)],
            RemoveOps = [RegOp.DeleteValue(Key, "DeferFeatureUpdatesPeriodInDays")],
            DetectOps = [RegOp.CheckDword(Key, "DeferFeatureUpdatesPeriodInDays", 90)],
        },
        new TweakDef
        {
            Id = "winsvc-defer-quality-updates-7-days",
            Label = "Servicing: Defer Windows Quality Updates for 7 Days to Allow Reliability Monitoring",
            Category = "Windows Servicing Policy",
            Description = "Sets DeferQualityUpdatesPeriodInDays=7 in WindowsUpdate policy. Delays the installation of Windows Quality Updates (monthly Patch Tuesday cumulative updates containing security fixes, reliability improvements, and bug fixes) by 7 days from their initial release to allow time for early-adopter reports to surface critical issues before enterprise-wide deployment. " +
                "Monthly Patch Tuesday cumulative updates occasionally introduce regressions — caused by a security fix that changes underlying API behaviour or a reliability fix interacting unexpectedly with specific application configurations. In prior years, Patch Tuesday updates have introduced BSoDs for specific driver configurations, performance regressions in SMB file server workloads, and print spooler failures. A 7-day deferral allows Microsoft, the community, and independent testing labs to publish regression reports before the update reaches production endpoints.",
            Tags = ["windows-servicing", "quality-update", "patch-tuesday", "deferral", "regression"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Quality updates deferred 7 days; regression reports communicated before production deployment.",
            ApplyOps = [RegOp.SetDword(Key, "DeferQualityUpdatesPeriodInDays", 7)],
            RemoveOps = [RegOp.DeleteValue(Key, "DeferQualityUpdatesPeriodInDays")],
            DetectOps = [RegOp.CheckDword(Key, "DeferQualityUpdatesPeriodInDays", 7)],
        },
        new TweakDef
        {
            Id = "winsvc-disable-dual-scan",
            Label = "Servicing: Disable WUfB Dual-Scan (WSUS + Windows Update Cloud Simultaneously)",
            Category = "Windows Servicing Policy",
            Description = "Sets DisableDualScan=1 in WindowsUpdate policy. Prevents Windows Update for Business from simultaneously scanning both the corporate WSUS server and the Windows Update cloud service for updates, restricting update source to the configured primary source only (typically WSUS). Without this setting, endpoints configured with both WSUS and WUfB policies may accidentally install cloud-sourced updates that haven't been approved in WSUS. " +
                "WSUS environments use update approval workflows to prevent unapproved patches from installing. Windows Update for Business cloud scanning bypasses WSUS approval workflows — an update that is DECLINED in WSUS may still install if the endpoint simultaneously scans and finds the update approved in the Windows Update cloud service. Dual scan effectively breaks WSUS update governance by allowing cloud updates to supersede WSUS-declined updates.",
            Tags = ["windows-servicing", "dual-scan", "wsus", "wufb", "update-governance"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Dual-scan disabled; updates only sourced from configured primary (WSUS/WUfB); cloud updates cannot bypass WSUS approval.",
            ApplyOps = [RegOp.SetDword(Key, "DisableDualScan", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableDualScan")],
            DetectOps = [RegOp.CheckDword(Key, "DisableDualScan", 1)],
        },
        new TweakDef
        {
            Id = "winsvc-block-preview-builds",
            Label = "Servicing: Block Windows Preview Builds and Insider Preview Enrollment",
            Category = "Windows Servicing Policy",
            Description = "Sets ManagePreviewBuilds=1 in WindowsUpdate policy. Prevents Windows from accessing Insider Preview builds, blocks the Windows Insider Program from enrolling the device, and hides the 'Windows Insider Program' section from Settings > Windows Update, making it impossible for users or administrators to opt into Insider Preview channels that would replace the production OS with a pre-release build. " +
                "Windows Insider Program enrolment replaces the production Windows build with a pre-release build that may have known critical vulnerabilities (disclosed during the Insider period), removed security features under development, or APIs with breaking changes from the production build. On enterprise endpoints, any path that allows downgrading from a supported production build to an unsupported pre-release build bypasses the enterprise's patching SLA and software support commitments.",
            Tags = ["windows-servicing", "insider-preview", "preview-builds", "insider-program", "lockdown"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Windows Insider Program blocked; device cannot be enrolled in preview channels or receive pre-release builds.",
            ApplyOps = [RegOp.SetDword(Key, "ManagePreviewBuilds", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "ManagePreviewBuilds")],
            DetectOps = [RegOp.CheckDword(Key, "ManagePreviewBuilds", 1)],
        },
        new TweakDef
        {
            Id = "winsvc-exclude-drivers-from-quality-updates",
            Label = "Servicing: Exclude Driver Updates from Monthly Quality Update Package",
            Category = "Windows Servicing Policy",
            Description = "Sets ExcludeWUDriversInQualityUpdate=1 in WindowsUpdate policy. Prevents Windows Update for Business from installing driver updates as part of the monthly cumulative quality update package, requiring that driver updates are sourced and approved separately through the driver management pipeline rather than being bundled into the OS quality update. " +
                "Driver updates bundled into Windows quality updates have been a source of hardware compatibility regressions, particularly for specialised peripherals, storage controllers, and graphics subsystems. A mandatory driver update included in a cumulative update may replace a tested, stable OEM driver with a Microsoft-provided inbox driver that behaves differently for specific hardware configurations. Excluding drivers from quality updates allows IT to validate and approve driver updates independently on a slower cadence.",
            Tags = ["windows-servicing", "drivers", "quality-update", "regression", "driver-management"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Driver updates excluded from quality update packages; drivers validated and deployed on separate IT-controlled schedule.",
            ApplyOps = [RegOp.SetDword(Key, "ExcludeWUDriversInQualityUpdate", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "ExcludeWUDriversInQualityUpdate")],
            DetectOps = [RegOp.CheckDword(Key, "ExcludeWUDriversInQualityUpdate", 1)],
        },
        new TweakDef
        {
            Id = "winsvc-block-optional-content-updates",
            Label = "Servicing: Block Optional Windows Content Updates (Media Features, Language Packs)",
            Category = "Windows Servicing Policy",
            Description = "Sets AllowOptionalContent=0 in WindowsUpdate policy. Prevents Windows Update from automatically downloading and installing optional content updates — including optional feature updates, language experience packs, optional cumulative update components, and regional supplemental content packs — without explicit IT administrator approval for each optional package. " +
                "Optional content includes media feature packs, additional language support, and supplemental features that Microsoft offers but does not install by default. While largely benign, optional content can consume hundreds of MB of disk space per package and is not required for enterprise operation. In disk-constrained environments (VDI thin clients, 128 GB endpoint SSDs) or bandwidth-constrained environments (WAN-connected branch offices), automatic download of optional content packages creates unnecessary overhead without enterprise benefit.",
            Tags = ["windows-servicing", "optional-content", "language-packs", "disk-space", "bandwidth"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Optional Windows content updates blocked; language packs and optional features not auto-downloaded.",
            ApplyOps = [RegOp.SetDword(Key, "AllowOptionalContent", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowOptionalContent")],
            DetectOps = [RegOp.CheckDword(Key, "AllowOptionalContent", 0)],
        },
        new TweakDef
        {
            Id = "winsvc-set-readiness-level-general-availability",
            Label = "Servicing: Set Branch Readiness Level to General Availability Channel",
            Category = "Windows Servicing Policy",
            Description = "Sets BranchReadinessLevel=16 in WindowsUpdate policy. Sets the Windows Update for Business readiness level (deployment ring) to General Availability Channel (value 16), directing the endpoint to receive feature updates only after they have been on the General Availability channel for the configured deferral period, rather than from the Beta or Release Preview channels. " +
                "BranchReadinessLevel determines which update channel feeds feature update availability. A value of 2 selects the Release Preview channel; 16 selects General Availability. Enterprises that configure WUfB without explicitly setting the readiness level may receive updates from the Release Preview channel, which contains builds that are near-final but may still have issues resolved between Release Preview and GA. Explicit GA targeting closes this gap.",
            Tags = ["windows-servicing", "branch-readiness", "ga-channel", "feature-update", "wufb"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "WUfB readiness level set to GA Channel (16); only fully released feature updates are eligible for deployment.",
            ApplyOps = [RegOp.SetDword(Key, "BranchReadinessLevel", 16)],
            RemoveOps = [RegOp.DeleteValue(Key, "BranchReadinessLevel")],
            DetectOps = [RegOp.CheckDword(Key, "BranchReadinessLevel", 16)],
        },
        new TweakDef
        {
            Id = "winsvc-enable-safe-os-update-rollback",
            Label = "Servicing: Enable SafeOS Update Rollback on Feature Update Failure Detection",
            Category = "Windows Servicing Policy",
            Description = "Sets EnableSafeOSUpdateRollback=1 in WindowsUpdate policy. Enables the Windows Safe OS rollback mechanism for failed feature updates. When a feature update installation fails (BSoD during upgrade, driver incompatibility detected, boot loop), Windows automatically rolls back to the previous working build rather than leaving the endpoint in an unbootable or partially-upgraded state. " +
                "Feature update installation failures can leave an endpoint in a state where it has partially installed the new version but cannot boot successfully. Without SafeOS rollback enabled, the endpoint may enter a boot repair loop, requiring IT to perform manual recovery (recovery console, reimaging). With SafeOS rollback, Windows detects the boot failure and automatically recovers to the last known good state, minimising end-user downtime and IT support demand for failed feature update deployments.",
            Tags = ["windows-servicing", "rollback", "feature-update", "safeos", "recovery"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "SafeOS rollback enabled; failed feature updates auto-revert to previous working build without manual IT intervention.",
            ApplyOps = [RegOp.SetDword(Key, "EnableSafeOSUpdateRollback", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableSafeOSUpdateRollback")],
            DetectOps = [RegOp.CheckDword(Key, "EnableSafeOSUpdateRollback", 1)],
        },
        new TweakDef
        {
            Id = "winsvc-enable-compliance-deadline-enforcement",
            Label = "Servicing: Enable Compliance Deadline Enforcement to Prevent Indefinite Update Deferral",
            Category = "Windows Servicing Policy",
            Description = "Sets EnableComplianceDeadlineEnforcement=1 in WindowsUpdate policy. Enables the WUfB compliance deadline mechanism, which automatically enforces update installation (overriding user-controlled active hours and post-deadline deferral settings) when a security update has been available beyond the configured deadline period, ensuring security patches cannot be deferred indefinitely by end-users. " +
                "Windows Update for Business user deadline controls allow end-users to dismiss and defer reboot prompts after updates are downloaded. In environments without compliance deadline enforcement, a user who repeatedly dismisses reboot prompts can delay security patch installation for weeks or months. The compliance deadline enforcement mechanism ensures that regardless of user behaviour, a security update that has been downloaded for more than the configured deadline period (typically 3–7 days) will install on the next restart.",
            Tags = ["windows-servicing", "compliance-deadline", "security-patch", "forced-reboot", "sla"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Compliance deadline enforcement active; security updates cannot be deferred indefinitely by end-users; SLA enforced.",
            ApplyOps = [RegOp.SetDword(Key, "EnableComplianceDeadlineEnforcement", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableComplianceDeadlineEnforcement")],
            DetectOps = [RegOp.CheckDword(Key, "EnableComplianceDeadlineEnforcement", 1)],
        },
    ];
}
