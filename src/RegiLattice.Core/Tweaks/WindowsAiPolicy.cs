// RegiLattice.Core — Tweaks/WindowsAiPolicy.cs
// Windows AI / Copilot+ / Recall Group Policy — Sprint 195.
// Controls Windows Recall snapshot saving, Copilot in Windows, on-device AI
// data analysis, AI content scanning, Cocreator, and Click to Do via GPO.
// Requires Windows 11 24H2+ on Copilot+ hardware.
// Category: "Windows AI Policy" | Slug: aipol
// Registry path: HKLM\SOFTWARE\Policies\Microsoft\WindowsAI

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WindowsAiPolicy
{
    private const string AiKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsAI";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "aipol-disable-recall",
                Label = "Disable Windows Recall (GPO)",
                Category = "Windows AI Policy",
                Description =
                    "Sets AllowRecall=0 in the WindowsAI policy key to disable Windows Recall entirely. Prevents the timeline-based AI memory search feature from running, collecting screenshots, or indexing content. Requires Windows 11 24H2 on Copilot+ hardware.",
                Tags = ["recall", "ai", "privacy", "policy", "copilot-plus"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 26100,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Disables Recall timeline completely; significant privacy improvement on Copilot+ PCs.",
                ApplyOps = [RegOp.SetDword(AiKey, "AllowRecall", 0)],
                RemoveOps = [RegOp.DeleteValue(AiKey, "AllowRecall")],
                DetectOps = [RegOp.CheckDword(AiKey, "AllowRecall", 0)],
            },
            new TweakDef
            {
                Id = "aipol-disable-saving-snapshots",
                Label = "Disable Recall Snapshot Saving",
                Category = "Windows AI Policy",
                Description =
                    "Sets TurnOffSavingSnapshots=1 to stop Windows Recall from capturing and storing periodic screenshots of user activity. Prevents screenshot data from being written to the semantic index database on disk.",
                Tags = ["recall", "snapshot", "screenshot", "ai", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 26100,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Stops all Recall screenshot capture; semantic search over past activity is disabled.",
                ApplyOps = [RegOp.SetDword(AiKey, "TurnOffSavingSnapshots", 1)],
                RemoveOps = [RegOp.DeleteValue(AiKey, "TurnOffSavingSnapshots")],
                DetectOps = [RegOp.CheckDword(AiKey, "TurnOffSavingSnapshots", 1)],
            },
            new TweakDef
            {
                Id = "aipol-disable-copilot-windows",
                Label = "Disable Copilot in Windows (GPO)",
                Category = "Windows AI Policy",
                Description =
                    "Sets TurnOffWindowsCopilot=1 to remove the Copilot button from the taskbar and disable the Copilot sidebar. Supersedes the user-space Copilot setting and applies across all user accounts on the device.",
                Tags = ["copilot", "ai", "taskbar", "policy", "windows11"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Removes Copilot taskbar entry and sidebar on Windows 11; no other system impact.",
                ApplyOps = [RegOp.SetDword(AiKey, "TurnOffWindowsCopilot", 1)],
                RemoveOps = [RegOp.DeleteValue(AiKey, "TurnOffWindowsCopilot")],
                DetectOps = [RegOp.CheckDword(AiKey, "TurnOffWindowsCopilot", 1)],
            },
            new TweakDef
            {
                Id = "aipol-disable-ai-data-analysis",
                Label = "Disable Recall AI Data Analysis",
                Category = "Windows AI Policy",
                Description =
                    "Sets TurnOffAIDataAnalysis=1 to prevent Windows Recall's AI engine from semantically analysing captured screenshots against the on-device model. Screenshot collection may still occur but content understanding is disabled.",
                Tags = ["recall", "ai", "analysis", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 26100,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Disables the AI semantic layer; Recall timeline still captures but can't answer queries.",
                ApplyOps = [RegOp.SetDword(AiKey, "TurnOffAIDataAnalysis", 1)],
                RemoveOps = [RegOp.DeleteValue(AiKey, "TurnOffAIDataAnalysis")],
                DetectOps = [RegOp.CheckDword(AiKey, "TurnOffAIDataAnalysis", 1)],
            },
            new TweakDef
            {
                Id = "aipol-disable-on-device-ai",
                Label = "Disable On-Device AI Processing",
                Category = "Windows AI Policy",
                Description =
                    "Sets AllowOnDeviceAI=0. Prevents Windows AI platform services from hosting NPU-accelerated inference on the local device. Blocks background AI model execution for all Windows AI APIs.",
                Tags = ["ai", "npu", "on-device", "inference", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 26100,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Blocks NPU AI inference host; may affect AI-enhanced features like live captions and Studio Effects.",
                ApplyOps = [RegOp.SetDword(AiKey, "AllowOnDeviceAI", 0)],
                RemoveOps = [RegOp.DeleteValue(AiKey, "AllowOnDeviceAI")],
                DetectOps = [RegOp.CheckDword(AiKey, "AllowOnDeviceAI", 0)],
            },
            new TweakDef
            {
                Id = "aipol-disable-click-to-do",
                Label = "Disable Click to Do AI Actions",
                Category = "Windows AI Policy",
                Description =
                    "Sets DisableClickToDo=1. Disables the Recall 'Click to Do' feature that allows users to invoke contextual AI actions on text or images seen in snapshots, preventing AI-driven clipboard and visual-action workflows.",
                Tags = ["recall", "click-to-do", "ai", "action", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 26100,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Removes Click to Do contextual AI actions from Recall snapshots; no other system impact.",
                ApplyOps = [RegOp.SetDword(AiKey, "DisableClickToDo", 1)],
                RemoveOps = [RegOp.DeleteValue(AiKey, "DisableClickToDo")],
                DetectOps = [RegOp.CheckDword(AiKey, "DisableClickToDo", 1)],
            },
            new TweakDef
            {
                Id = "aipol-block-ai-experiences",
                Label = "Block Windows AI Experiences",
                Category = "Windows AI Policy",
                Description =
                    "Sets AllowExperiences=0 in the WindowsAI policy key. Blocks enrollment in Windows AI experience features distributed via Settings > Privacy & Security > Windows AI, disabling the AI feature consent flow.",
                Tags = ["ai", "experiences", "consent", "policy", "privacy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Removes AI experience opt-in prompts from Settings; existing AI features may continue.",
                ApplyOps = [RegOp.SetDword(AiKey, "AllowExperiences", 0)],
                RemoveOps = [RegOp.DeleteValue(AiKey, "AllowExperiences")],
                DetectOps = [RegOp.CheckDword(AiKey, "AllowExperiences", 0)],
            },
            new TweakDef
            {
                Id = "aipol-disable-content-scan",
                Label = "Disable AI Content Scanning",
                Category = "Windows AI Policy",
                Description =
                    "Sets DisableAIContentScan=1. Prevents the Windows AI platform from performing background content scanning of files and screen content that feeds AI indexing services, limiting persistent AI data collection.",
                Tags = ["ai", "content", "scan", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Stops AI background content scanning; may reduce AI feature responsiveness on Copilot+ PCs.",
                ApplyOps = [RegOp.SetDword(AiKey, "DisableAIContentScan", 1)],
                RemoveOps = [RegOp.DeleteValue(AiKey, "DisableAIContentScan")],
                DetectOps = [RegOp.CheckDword(AiKey, "DisableAIContentScan", 1)],
            },
            new TweakDef
            {
                Id = "aipol-prevent-ai-processing",
                Label = "Prevent Background AI Processing",
                Category = "Windows AI Policy",
                Description =
                    "Sets PreventAIProcessing=1. Blocks low-priority background AI processing tasks scheduled by the Windows AI runtime, preventing model-driven inference from running when the system is idle.",
                Tags = ["ai", "background", "processing", "performance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 26100,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents background AI inference tasks; reduces NPU usage but limits proactive AI suggestions.",
                ApplyOps = [RegOp.SetDword(AiKey, "PreventAIProcessing", 1)],
                RemoveOps = [RegOp.DeleteValue(AiKey, "PreventAIProcessing")],
                DetectOps = [RegOp.CheckDword(AiKey, "PreventAIProcessing", 1)],
            },
            new TweakDef
            {
                Id = "aipol-disable-save-screenshots",
                Label = "Disable AI Automatic Screenshot Saving",
                Category = "Windows AI Policy",
                Description =
                    "Sets TurnOffSavingScreenshots=1. Specifically disables the automatic screenshot persistence layer used by the AI subsystem independently of Recall, preventing any AI service from retaining periodic screen captures as learning data.",
                Tags = ["ai", "screenshot", "saving", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 26100,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Stops automatic screenshot retention by the AI platform; user-initiated snips unaffected.",
                ApplyOps = [RegOp.SetDword(AiKey, "TurnOffSavingScreenshots", 1)],
                RemoveOps = [RegOp.DeleteValue(AiKey, "TurnOffSavingScreenshots")],
                DetectOps = [RegOp.CheckDword(AiKey, "TurnOffSavingScreenshots", 1)],
            },
        ];
}
