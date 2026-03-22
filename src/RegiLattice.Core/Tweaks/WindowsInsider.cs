// RegiLattice.Core — Tweaks/WindowsInsider.cs
// Block Windows Insider Program enrollment, feature flighting, and feedback prompts.
// Slug: "insider" — PreviewBuilds GPO policy + WindowsSelfHost applicability keys.

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WindowsInsider
{
    private const string PreviewBuildsPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PreviewBuilds";

    private const string DataCollection = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection";

    private const string CloudContent = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent";

    private const string SelfHostApplicability = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\WindowsSelfHost\Applicability";

    private const string FeedbackRules = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Siuf\Rules";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "insider-block-preview-builds",
            Label = "Block Windows Insider Preview Build Enrollment (GPO)",
            Category = "Windows Insider",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["insider", "preview", "flighting", "windows update", "policy"],
            Description =
                "Prevents users from enrolling this device in the Windows Insider Program "
                + "via Group Policy. AllowBuildPreview=0. The 'Windows Insider Program' page "
                + "in Settings is greyed out and the device receives only stable Windows builds.",
            ApplyOps = [RegOp.SetDword(PreviewBuildsPolicy, "AllowBuildPreview", 0)],
            RemoveOps = [RegOp.DeleteValue(PreviewBuildsPolicy, "AllowBuildPreview")],
            DetectOps = [RegOp.CheckDword(PreviewBuildsPolicy, "AllowBuildPreview", 0)],
        },
        new TweakDef
        {
            Id = "insider-disable-config-flighting",
            Label = "Disable Configuration Flighting (A/B Feature Tests)",
            Category = "Windows Insider",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["insider", "flighting", "a/b testing", "telemetry", "privacy"],
            Description =
                "Disables Windows configuration flighting — the mechanism Microsoft uses to "
                + "remotely enable or disable features on specific devices as part of A/B tests. "
                + "EnableConfigFlighting=0 prevents undocumented feature changes pushed by Microsoft.",
            ApplyOps = [RegOp.SetDword(PreviewBuildsPolicy, "EnableConfigFlighting", 0)],
            RemoveOps = [RegOp.DeleteValue(PreviewBuildsPolicy, "EnableConfigFlighting")],
            DetectOps = [RegOp.CheckDword(PreviewBuildsPolicy, "EnableConfigFlighting", 0)],
        },
        new TweakDef
        {
            Id = "insider-disable-experimentation",
            Label = "Disable Windows Experimentation (A/B Feature Trials)",
            Category = "Windows Insider",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["insider", "experimentation", "a/b testing", "telemetry"],
            Description =
                "Prevents Windows from participating in Microsoft's experimentation framework "
                + "used to validate new features before official release. "
                + "EnableExperimentation=0 ensures a fully stable, non-experimental Windows build.",
            ApplyOps = [RegOp.SetDword(PreviewBuildsPolicy, "EnableExperimentation", 0)],
            RemoveOps = [RegOp.DeleteValue(PreviewBuildsPolicy, "EnableExperimentation")],
            DetectOps = [RegOp.CheckDword(PreviewBuildsPolicy, "EnableExperimentation", 0)],
        },
        new TweakDef
        {
            Id = "insider-disable-feedback-notifications",
            Label = "Disable Windows Feedback Notification Popups",
            Category = "Windows Insider",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["insider", "feedback", "notification", "privacy"],
            Description =
                "Suppresses the periodic 'How is Windows working for you?' feedback popups. "
                + "DoNotShowFeedbackNotifications=1. These prompts collect usage data and "
                + "interrupt the user — disabling them keeps the UI clean.",
            ApplyOps = [RegOp.SetDword(DataCollection, "DoNotShowFeedbackNotifications", 1)],
            RemoveOps = [RegOp.DeleteValue(DataCollection, "DoNotShowFeedbackNotifications")],
            DetectOps = [RegOp.CheckDword(DataCollection, "DoNotShowFeedbackNotifications", 1)],
        },
        new TweakDef
        {
            Id = "insider-set-retail-ring",
            Label = "Set Device to Retail (Non-Insider) Ring",
            Category = "Windows Insider",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["insider", "ring", "retail", "preview builds"],
            Description =
                "Sets the Windows Update ring to Retail via the WindowsSelfHost applicability "
                + "registry. EnablePreviewBuilds=0 ensures this device uses stable production "
                + "builds only, exiting any Insider ring it may have been enrolled in.",
            ApplyOps = [RegOp.SetDword(SelfHostApplicability, "EnablePreviewBuilds", 0)],
            RemoveOps = [RegOp.DeleteValue(SelfHostApplicability, "EnablePreviewBuilds")],
            DetectOps = [RegOp.CheckDword(SelfHostApplicability, "EnablePreviewBuilds", 0)],
        },
        new TweakDef
        {
            Id = "insider-disable-feedback-frequency",
            Label = "Stop Windows Feedback Frequency Prompts",
            Category = "Windows Insider",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["insider", "feedback", "siuf", "privacy"],
            Description =
                "Sets the Windows SIUF (System-Initiated User Feedback) period count to 0, "
                + "silencing all Windows 'Rate your experience' prompts. "
                + "NumberOfSIUFInPeriod=0 in the user feedback rules key.",
            ApplyOps = [RegOp.SetDword(FeedbackRules, "NumberOfSIUFInPeriod", 0)],
            RemoveOps = [RegOp.DeleteValue(FeedbackRules, "NumberOfSIUFInPeriod")],
            DetectOps = [RegOp.CheckDword(FeedbackRules, "NumberOfSIUFInPeriod", 0)],
        },
        new TweakDef
        {
            Id = "insider-disable-consumer-features",
            Label = "Disable Windows Consumer (Non-Enterprise) Features",
            Category = "Windows Insider",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["insider", "consumer features", "cloud content", "privacy"],
            Description =
                "Disables Windows consumer-only experiences: auto pre-install of promoted apps, "
                + "Spotlight suggestions on the Start menu, and per-user app recommendations. "
                + "DisableWindowsConsumerFeatures=1 in Cloud Content policy.",
            ApplyOps = [RegOp.SetDword(CloudContent, "DisableWindowsConsumerFeatures", 1)],
            RemoveOps = [RegOp.DeleteValue(CloudContent, "DisableWindowsConsumerFeatures")],
            DetectOps = [RegOp.CheckDword(CloudContent, "DisableWindowsConsumerFeatures", 1)],
        },
        new TweakDef
        {
            Id = "insider-disable-soft-landing",
            Label = "Disable Soft Landing Tips (New Feature Suggestions)",
            Category = "Windows Insider",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["insider", "soft landing", "cloud content", "tips", "privacy"],
            Description =
                "Blocks 'soft landing' — the mechanism Windows uses to show first-run tips, "
                + "feature highlight cards, and What's New overlays after major updates. "
                + "DisableSoftLanding=1. Keeps post-update UI identical to pre-update.",
            ApplyOps = [RegOp.SetDword(CloudContent, "DisableSoftLanding", 1)],
            RemoveOps = [RegOp.DeleteValue(CloudContent, "DisableSoftLanding")],
            DetectOps = [RegOp.CheckDword(CloudContent, "DisableSoftLanding", 1)],
        },
        new TweakDef
        {
            Id = "insider-disable-cloud-optimized-content",
            Label = "Disable Cloud-Optimized Content Delivery",
            Category = "Windows Insider",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["insider", "cloud content", "content delivery", "privacy"],
            Description =
                "Disables cloud-optimized content that Microsoft delivers to the lock screen, "
                + "Start menu, and desktop (e.g., personalized ads, app promotions). "
                + "DisableCloudOptimizedContent=1 in Cloud Content policy.",
            ApplyOps = [RegOp.SetDword(CloudContent, "DisableCloudOptimizedContent", 1)],
            RemoveOps = [RegOp.DeleteValue(CloudContent, "DisableCloudOptimizedContent")],
            DetectOps = [RegOp.CheckDword(CloudContent, "DisableCloudOptimizedContent", 1)],
        },
        new TweakDef
        {
            Id = "insider-disable-cloud-content-experience",
            Label = "Disable Cloud Content for Windows Suggestions",
            Category = "Windows Insider",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["insider", "cloud content", "suggestions", "windows tips", "privacy"],
            Description =
                "Disables cloud-delivered content shown in the Windows welcome experience, "
                + "Settings highlights, and the first-run screen after major updates. "
                + "DisableTailoredExperiencesWithDiagnosticData=1 prevents Clippy-style "
                + "personalized suggestions based on diagnostics.",
            ApplyOps = [RegOp.SetDword(DataCollection, "DisableTailoredExperiencesWithDiagnosticData", 1)],
            RemoveOps = [RegOp.DeleteValue(DataCollection, "DisableTailoredExperiencesWithDiagnosticData")],
            DetectOps = [RegOp.CheckDword(DataCollection, "DisableTailoredExperiencesWithDiagnosticData", 1)],
        },
    ];
}
