// RegiLattice.Core — Tweaks/FeedbackPolicy.cs
// Sprint 284: Windows Feedback Hub Group Policy (10 tweaks)
// Category: "Feedback Policy" | Slug: fbk
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Feedback

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class FeedbackPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Feedback";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "fbk-disable-feedback-notifications",
            Label = "Disable Feedback Notifications",
            Category = "Feedback Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
            Description =
                "Sets DisableFeedbackNotifications=1 in the Feedback policy key. Prevents "
                + "Windows from displaying in-product popups asking users to rate their "
                + "experience, review new features, or complete satisfaction surveys. "
                + "Feedback prompts appear on lock screens, Start, and Settings and "
                + "are disruptive on shared kiosk or call-centre machines. "
                + "Default: 0. Recommended: 1 on enterprise or kiosk deployments.",
            Tags = ["feedback", "notifications", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableFeedbackNotifications", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableFeedbackNotifications")],
            DetectOps = [RegOp.CheckDword(Key, "DisableFeedbackNotifications", 1)],
        },
        new TweakDef
        {
            Id = "fbk-disable-feedback-hub",
            Label = "Disable Feedback Hub",
            Category = "Feedback Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
            Description =
                "Sets DisableFeedbackHub=1 in the Feedback policy key. Blocks the "
                + "Feedback Hub application from submitting problem reports, feature "
                + "requests, or screenshots to Microsoft's developer pipeline. Feedback "
                + "Hub submissions include attached device diagnostics and may capture "
                + "window contents and audio, raising privacy concerns in regulated "
                + "environments. Default: 0. Recommended: 1.",
            Tags = ["feedback", "hub", "privacy", "telemetry", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableFeedbackHub", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableFeedbackHub")],
            DetectOps = [RegOp.CheckDword(Key, "DisableFeedbackHub", 1)],
        },
        new TweakDef
        {
            Id = "fbk-disable-feedback-hub-nps",
            Label = "Disable Feedback Hub NPS Surveys",
            Category = "Feedback Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets DisableNpsSurveys=1 in the Feedback policy key. Suppresses "
                + "Net Promoter Score (NPS) survey overlays that ask how likely the "
                + "user is to recommend Windows. NPS surveys are triggered "
                + "automatically after usage milestones and appear over full-screen "
                + "applications, causing unexpected context loss and accessibility "
                + "issues on touchscreen devices. Default: 0.",
            Tags = ["feedback", "nps", "survey", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableNpsSurveys", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableNpsSurveys")],
            DetectOps = [RegOp.CheckDword(Key, "DisableNpsSurveys", 1)],
        },
        new TweakDef
        {
            Id = "fbk-disable-feedback-telemetry-upload",
            Label = "Disable Feedback Telemetry Upload",
            Category = "Feedback Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 4, SafetyRating = 5,
            Description =
                "Sets DisableTelemetryUpload=1 in the Feedback policy key. Explicitly "
                + "blocks the Feedback Hub service from uploading telemetry bundles "
                + "that contain app crash data, device configuration snapshots, and "
                + "event traces to Microsoft's backend. This is complementary to the "
                + "global telemetry policy and specifically targets the Feedback Hub's "
                + "own upload queue. Default: 0. Recommended: 1.",
            Tags = ["feedback", "telemetry", "upload", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableTelemetryUpload", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableTelemetryUpload")],
            DetectOps = [RegOp.CheckDword(Key, "DisableTelemetryUpload", 1)],
        },
        new TweakDef
        {
            Id = "fbk-disable-screen-capture-feedback",
            Label = "Disable Feedback Screen Capture",
            Category = "Feedback Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 4, SafetyRating = 5,
            Description =
                "Sets DisableScreenCapture=1 in the Feedback policy key. Prevents the "
                + "Feedback Hub from automatically capturing a screenshot when the "
                + "user initiates a problem report. Automatic screen capture collects "
                + "potentially sensitive data visible on the screen at the moment of "
                + "capture, including text, passwords, and documents. "
                + "Default: 0. Recommended: 1.",
            Tags = ["feedback", "screenshot", "screen-capture", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableScreenCapture", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableScreenCapture")],
            DetectOps = [RegOp.CheckDword(Key, "DisableScreenCapture", 1)],
        },
        new TweakDef
        {
            Id = "fbk-disable-recording-feedback",
            Label = "Disable Feedback Steps Recorder",
            Category = "Feedback Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
            Description =
                "Sets DisableStepsRecorder=1 in the Feedback policy key. Blocks the "
                + "Steps Recorder component of the Feedback Hub from recording a "
                + "sequence of UI interactions and screenshots to attach to a problem "
                + "report. Steps Recorder captures all input events and window "
                + "contents for the recording duration, representing a significant "
                + "privacy and data-leakage risk on shared machines. Default: 0.",
            Tags = ["feedback", "steps-recorder", "recording", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableStepsRecorder", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableStepsRecorder")],
            DetectOps = [RegOp.CheckDword(Key, "DisableStepsRecorder", 1)],
        },
        new TweakDef
        {
            Id = "fbk-disable-feedback-app-prompts",
            Label = "Disable In-App Feedback Prompts",
            Category = "Feedback Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets DisableInAppFeedbackPrompts=1 in the Feedback policy key. Prevents "
                + "Windows-inbox applications and UWP system apps from showing inline "
                + "'Send feedback' or thumbs-up/down rating controls in their UI. These "
                + "prompts appear in Calendar, Mail, Weather, and Edge and consume "
                + "reachable screen space on constrained display sizes and kiosk shells. "
                + "Default: 0. Recommended: 1 on locked-down deployments.",
            Tags = ["feedback", "in-app", "prompts", "ux", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableInAppFeedbackPrompts", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableInAppFeedbackPrompts")],
            DetectOps = [RegOp.CheckDword(Key, "DisableInAppFeedbackPrompts", 1)],
        },
        new TweakDef
        {
            Id = "fbk-set-feedback-frequency-never",
            Label = "Set Feedback Frequency to Never",
            Category = "Feedback Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
            Description =
                "Sets FeedbackFrequency=0 in the Feedback policy key. Sets the periodic "
                + "feedback prompt interval to 0 (Never), so Windows never schedules "
                + "a feedback dialog based on elapsed time or usage events. The "
                + "FeedbackFrequency policy values are 0=Never, 1=Always, 2=Once. "
                + "Setting this to Never is the most restrictive option and aligns with "
                + "an enterprise no-feedback posture. Default: 1.",
            Tags = ["feedback", "frequency", "never", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "FeedbackFrequency", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "FeedbackFrequency")],
            DetectOps = [RegOp.CheckDword(Key, "FeedbackFrequency", 0)],
        },
        new TweakDef
        {
            Id = "fbk-disable-voluntary-data-collection",
            Label = "Disable Voluntary Data Collection via Feedback",
            Category = "Feedback Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
            Description =
                "Sets DisableVoluntaryDataCollection=1 in the Feedback policy key. Opts "
                + "the device out of voluntary data collection programmes (such as the "
                + "Customer Experience Improvement Program) that are triggered through "
                + "Feedback Hub consent dialogs. Consent obtained via feedback prompts "
                + "may be granted by users without full understanding of what diagnostic "
                + "data is collected. Default: 0. Recommended: 1.",
            Tags = ["feedback", "ceip", "data-collection", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableVoluntaryDataCollection", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableVoluntaryDataCollection")],
            DetectOps = [RegOp.CheckDword(Key, "DisableVoluntaryDataCollection", 1)],
        },
        new TweakDef
        {
            Id = "fbk-disable-feedback-account-requirement",
            Label = "Disable Feedback Account Sign-In Requirement",
            Category = "Feedback Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets DisableAccountRequirement=1 in the Feedback policy key. Prevents "
                + "the Feedback Hub from prompting users to sign in to a Microsoft "
                + "Account as a prerequisite for submitting feedback. Removing the "
                + "sign-in requirement limits correlation of feedback submissions with "
                + "individual user identities in Microsoft's backend systems. "
                + "Default: 0. Recommended: 1.",
            Tags = ["feedback", "account", "msa", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableAccountRequirement", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableAccountRequirement")],
            DetectOps = [RegOp.CheckDword(Key, "DisableAccountRequirement", 1)],
        },
    ];
}
