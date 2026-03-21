// RegiLattice.Core — Tweaks/CopilotPlus.cs
// Copilot+ PC specific AI/NPU features (Win11 23H2/24H2 only).
// Uses slug "cplplus" — covers NPU policy, Recall advanced, AI Paint, Live Captions AI,
// and Cocreator features NOT already present in Copilot.cs (slug "ai-").

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class CopilotPlus
{
    // Snapshots / Recall advanced controls
    private const string RecallAdv = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI";

    // NPU / AI runtime
    private const string NpuPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AI";
    private const string NpuSched = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\AI";

    // Live Captions AI enhancement
    private const string CaptionsPol = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Accessibility\LiveCaptions";

    // AI Writing Assistant in Office/OneNote
    private const string WritingAi = @"HKEY_CURRENT_USER\Software\Microsoft\Office\Common\AI";

    // AI Phonetic Suggestions (IME / Text Input)
    private const string ImeAi = @"HKEY_CURRENT_USER\Software\Microsoft\InputMethod\Settings\CHS";

    // Paint Cocreator (AI image generation inside Paint)
    private const string PaintCo = @"HKEY_CURRENT_USER\Software\Microsoft\Paint\Capabilities";

    // Bing Visual Search / AI enhanced search in Explorer
    private const string ExplorerAi = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Exp\Explorer\Advanced";

    // Copilot+ Key remapping preference (the dedicated Copilot key on new hardware)
    private const string CopilotKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PolicyManager\current\device\WindowsAI";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "cplplus-disable-recall-snapshots",
            Label = "Disable Windows Recall Snapshot Storage",
            Category = "Copilot+ Features",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["copilot+", "recall", "privacy", "npu", "ai"],
            Description =
                "Prevents Windows Recall from storing encrypted screenshots "
                + "(snapshots) of your screen activity. The feature remains installed "
                + "but the continuous screen capture pipeline is stopped.",
            ApplyOps = [RegOp.SetDword(RecallAdv, "DisableAIDataAnalysis", 1), RegOp.SetDword(RecallAdv, "AllowRecallEnablement", 0)],
            RemoveOps = [RegOp.DeleteValue(RecallAdv, "DisableAIDataAnalysis"), RegOp.DeleteValue(RecallAdv, "AllowRecallEnablement")],
            DetectOps = [RegOp.CheckDword(RecallAdv, "DisableAIDataAnalysis", 1)],
        },
        new TweakDef
        {
            Id = "cplplus-disable-npu-inference-policy",
            Label = "Disable NPU Inference Scheduling",
            Category = "Copilot+ Features",
            NeedsAdmin = true,
            CorpSafe = false,
            Tags = ["copilot+", "npu", "ai", "performance"],
            Description =
                "Disables the Windows AI inference scheduling policy that routes "
                + "workloads to the NPU. Forces all AI inference to the CPU/GPU, "
                + "which may be faster for GPU-accelerated models (e.g., NVIDIA CUDA).",
            ApplyOps = [RegOp.SetDword(NpuPolicy, "AllowNPUInference", 0)],
            RemoveOps = [RegOp.DeleteValue(NpuPolicy, "AllowNPUInference")],
            DetectOps = [RegOp.CheckDword(NpuPolicy, "AllowNPUInference", 0)],
        },
        new TweakDef
        {
            Id = "cplplus-disable-npu-always-on",
            Label = "Disable NPU Always-On Mode",
            Category = "Copilot+ Features",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["copilot+", "npu", "ai", "battery", "energy"],
            Description =
                "Stops the NPU from remaining active in an always-on standby state "
                + "to serve background AI features. Reduces idle power draw on Copilot+ "
                + "PCs with Qualcomm or Intel NPUs.",
            ApplyOps = [RegOp.SetDword(NpuSched, "NPUAlwaysOnEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(NpuSched, "NPUAlwaysOnEnabled")],
            DetectOps = [RegOp.CheckDword(NpuSched, "NPUAlwaysOnEnabled", 0)],
        },
        new TweakDef
        {
            Id = "cplplus-disable-ai-enhanced-live-captions",
            Label = "Disable AI-Enhanced Live Captions",
            Category = "Copilot+ Features",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["copilot+", "live captions", "ai", "accessibility"],
            Description =
                "Turns off the AI-powered noise-suppression and speaker-diarization "
                + "enhancements in Live Captions. Reverts to the standard offline "
                + "speech-to-text engine, which uses less CPU and RAM.",
            ApplyOps = [RegOp.SetDword(CaptionsPol, "AIEnhancedCaptions", 0)],
            RemoveOps = [RegOp.DeleteValue(CaptionsPol, "AIEnhancedCaptions")],
            DetectOps = [RegOp.CheckDword(CaptionsPol, "AIEnhancedCaptions", 0)],
        },
        new TweakDef
        {
            Id = "cplplus-disable-ai-writing-suggestions",
            Label = "Disable AI Writing Suggestions in Office Apps",
            Category = "Copilot+ Features",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["copilot+", "ai", "office", "writing", "privacy"],
            Description =
                "Prevents Microsoft Office (Word, Outlook, OneNote) from using the "
                + "AI Writing Assistant to auto-complete sentences and suggest edits. "
                + "Also prevents local text from being sent to Copilot cloud services.",
            ApplyOps = [RegOp.SetDword(WritingAi, "InlineAIEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(WritingAi, "InlineAIEnabled")],
            DetectOps = [RegOp.CheckDword(WritingAi, "InlineAIEnabled", 0)],
        },
        new TweakDef
        {
            Id = "cplplus-disable-paint-cocreator",
            Label = "Disable Paint Cocreator (AI Image Generation)",
            Category = "Copilot+ Features",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["copilot+", "ai", "paint", "image generation", "privacy"],
            Description =
                "Removes the Cocreator panel from Microsoft Paint that connects to "
                + "DALL-E / Bing Image Creator for AI-generated images. Prevents Paint "
                + "from making outbound requests during normal use.",
            ApplyOps = [RegOp.SetDword(PaintCo, "CoCreatorEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(PaintCo, "CoCreatorEnabled")],
            DetectOps = [RegOp.CheckDword(PaintCo, "CoCreatorEnabled", 0)],
        },
        new TweakDef
        {
            Id = "cplplus-disable-explorer-ai-search",
            Label = "Disable AI-Enhanced Search in File Explorer",
            Category = "Copilot+ Features",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["copilot+", "ai", "explorer", "search", "privacy"],
            Description =
                "Disables the AI-powered semantic search feature in File Explorer "
                + "(natural language file searches). Reverts to traditional indexed "
                + "search, which is faster for exact-match queries.",
            ApplyOps = [RegOp.SetDword(ExplorerAi, "EnableAISemanticSearch", 0)],
            RemoveOps = [RegOp.DeleteValue(ExplorerAi, "EnableAISemanticSearch")],
            DetectOps = [RegOp.CheckDword(ExplorerAi, "EnableAISemanticSearch", 0)],
        },
        new TweakDef
        {
            Id = "cplplus-disable-copilot-key-action",
            Label = "Disable Copilot Key Hardware Button",
            Category = "Copilot+ Features",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["copilot+", "ai", "keyboard", "copilot key"],
            Description =
                "Disables the system action triggered by the dedicated Copilot key "
                + "present on Copilot+ certified keyboards. The key press produces no "
                + "action (can be remapped separately with PowerToys).",
            ApplyOps = [RegOp.SetDword(CopilotKey, "TurnOffWindowsCopilot", 1)],
            RemoveOps = [RegOp.DeleteValue(CopilotKey, "TurnOffWindowsCopilot")],
            DetectOps = [RegOp.CheckDword(CopilotKey, "TurnOffWindowsCopilot", 1)],
        },
        new TweakDef
        {
            Id = "cplplus-disable-ime-ai-suggestions",
            Label = "Disable AI Phonetic IME Suggestions",
            Category = "Copilot+ Features",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["copilot+", "ai", "ime", "keyboard", "input"],
            Description =
                "Turns off the AI-powered phonetic and contextual input suggestions "
                + "in the Windows Chinese (Simplified) IME. Reduces background model "
                + "inference on Copilot+ PCs with East-Asian language packs.",
            ApplyOps = [RegOp.SetDword(ImeAi, "UseAISuggestions", 0)],
            RemoveOps = [RegOp.DeleteValue(ImeAi, "UseAISuggestions")],
            DetectOps = [RegOp.CheckDword(ImeAi, "UseAISuggestions", 0)],
        },
        new TweakDef
        {
            Id = "cplplus-disable-recall-indexing",
            Label = "Disable Recall Search Indexing of Snapshots",
            Category = "Copilot+ Features",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["copilot+", "recall", "privacy", "indexing", "ai"],
            Description =
                "Stops the Recall search pipeline from indexing the encrypted "
                + "snapshot database even if Recall capture is still active. "
                + "Prevents the AI content-extraction process from running on stored "
                + "screenshots, reducing CPU usage and privacy exposure.",
            ApplyOps = [RegOp.SetDword(RecallAdv, "DisableRecallSearch", 1)],
            RemoveOps = [RegOp.DeleteValue(RecallAdv, "DisableRecallSearch")],
            DetectOps = [RegOp.CheckDword(RecallAdv, "DisableRecallSearch", 1)],
        },
    ];
}
