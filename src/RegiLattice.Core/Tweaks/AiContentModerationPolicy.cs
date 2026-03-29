// RegiLattice.Core — Tweaks/AiContentModerationPolicy.cs
// Windows AI/Copilot content moderation, responsible AI, and generative AI safety policy — Sprint 484.
// Category: "AI Content Moderation Policy" | Slug: aimod
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\WindowsAI\ContentModeration

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class AiContentModerationPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI\ContentModeration";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "aimod-enable-strict-content-filter",
                Label = "Enable Strict Content Filter for AI Responses",
                Category = "AI Content Moderation Policy",
                Description =
                    "Enables the strict content safety filter for all Windows AI / Copilot response generation, blocking any AI output classified as harmful, violent, sexual, or hate speech at the strictest threshold.",
                Tags = ["ai", "content-moderation", "safety-filter", "copilot", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Strict AI content filter active; AI responses with any harmful content classification blocked.",
                ApplyOps = [RegOp.SetDword(Key, "ContentFilterLevel", 2)],
                RemoveOps = [RegOp.DeleteValue(Key, "ContentFilterLevel")],
                DetectOps = [RegOp.CheckDword(Key, "ContentFilterLevel", 2)],
            },
            new TweakDef
            {
                Id = "aimod-block-ai-code-generation",
                Label = "Block AI Code Generation Features",
                Category = "AI Content Moderation Policy",
                Description =
                    "Disables AI-powered code generation and autocomplete features within Windows Copilot and integrated apps, preventing AI-generated code from being inserted into development workflows without review.",
                Tags = ["ai", "code-generation", "copilot", "developer", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "AI code generation disabled; AI cannot suggest or insert code into editors.",
                ApplyOps = [RegOp.SetDword(Key, "BlockAICodeGeneration", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockAICodeGeneration")],
                DetectOps = [RegOp.CheckDword(Key, "BlockAICodeGeneration", 1)],
            },
            new TweakDef
            {
                Id = "aimod-disable-ai-personalisation",
                Label = "Disable AI Personalisation Data Collection",
                Category = "AI Content Moderation Policy",
                Description =
                    "Prevents Windows AI services from collecting and retaining user behaviour data (typing patterns, app usage, search history) to personalise AI responses, limiting AI training data leakage.",
                Tags = ["ai", "personalisation", "privacy", "telemetry", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "AI personalisation disabled; AI responses not tailored by usage history. No usage data retained.",
                ApplyOps = [RegOp.SetDword(Key, "DisableAIPersonalisation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAIPersonalisation")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAIPersonalisation", 1)],
            },
            new TweakDef
            {
                Id = "aimod-block-ai-web-search-grounding",
                Label = "Block AI Web Search Grounding (Bing Lookups)",
                Category = "AI Content Moderation Policy",
                Description =
                    "Prevents Windows Copilot and on-device AI features from grounding responses with live Bing search results, ensuring all AI answers are generated from pre-trained models without sending queries to external search APIs.",
                Tags = ["ai", "web-search", "bing", "grounding", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "AI web search grounding blocked; Copilot responses rely only on built-in model, no Bing lookups.",
                ApplyOps = [RegOp.SetDword(Key, "BlockWebSearchGrounding", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockWebSearchGrounding")],
                DetectOps = [RegOp.CheckDword(Key, "BlockWebSearchGrounding", 1)],
            },
            new TweakDef
            {
                Id = "aimod-require-human-review-for-ai-actions",
                Label = "Require Human Confirmation for AI System Actions",
                Category = "AI Content Moderation Policy",
                Description =
                    "Requires explicit human confirmation before Windows AI agents execute any system-level actions (file operations, email sends, calendar changes), preventing autonomous AI execution without user oversight.",
                Tags = ["ai", "human-in-the-loop", "confirmation", "agentic-ai", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Human confirmation gate required; AI cannot autonomously perform system actions without approval.",
                ApplyOps = [RegOp.SetDword(Key, "RequireHumanConfirmationForActions", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireHumanConfirmationForActions")],
                DetectOps = [RegOp.CheckDword(Key, "RequireHumanConfirmationForActions", 1)],
            },
            new TweakDef
            {
                Id = "aimod-disable-ai-suggested-replies",
                Label = "Disable AI Suggested Replies in Mail and Messaging",
                Category = "AI Content Moderation Policy",
                Description =
                    "Disables AI-generated suggested reply suggestions in Windows Mail, Outlook, Teams, and other messaging apps, preventing AI from pre-generating reply content that could be accepted without careful reading.",
                Tags = ["ai", "suggested-replies", "email", "messaging", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "AI suggested replies disabled in mail and messaging apps.",
                ApplyOps = [RegOp.SetDword(Key, "DisableAISuggestedReplies", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAISuggestedReplies")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAISuggestedReplies", 1)],
            },
            new TweakDef
            {
                Id = "aimod-block-ai-prompt-cloud-logging",
                Label = "Block AI Prompt and Response Cloud Logging",
                Category = "AI Content Moderation Policy",
                Description =
                    "Prevents user AI prompts and model responses from being sent to Microsoft cloud servers for safety monitoring, model improvement, or abuse reporting, keeping all AI interactions on-device.",
                Tags = ["ai", "cloud-logging", "privacy", "copilot", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "AI prompt/response cloud logging blocked; interactions stay on-device and are not sent to Microsoft.",
                ApplyOps = [RegOp.SetDword(Key, "BlockPromptCloudLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockPromptCloudLogging")],
                DetectOps = [RegOp.CheckDword(Key, "BlockPromptCloudLogging", 1)],
            },
            new TweakDef
            {
                Id = "aimod-disable-ai-clipboard-reading",
                Label = "Disable AI Clipboard Reading for Context Suggestions",
                Category = "AI Content Moderation Policy",
                Description =
                    "Prevents Windows AI features from reading clipboard contents to generate contextual suggestions, stopping the AI from having automatic access to copied passwords, credentials, or sensitive data.",
                Tags = ["ai", "clipboard", "privacy", "context", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "AI clipboard reading disabled; AI features cannot access clipboard content for context.",
                ApplyOps = [RegOp.SetDword(Key, "DisableAIClipboardReading", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAIClipboardReading")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAIClipboardReading", 1)],
            },
            new TweakDef
            {
                Id = "aimod-block-ai-file-system-access",
                Label = "Block AI Features from Accessing File System Context",
                Category = "AI Content Moderation Policy",
                Description =
                    "Restricts Windows AI features from reading file system metadata (recently opened files, folder names) to generate smart suggestions, preventing AI-based discovery of sensitive file names and paths.",
                Tags = ["ai", "file-system", "privacy", "context", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "AI file system context access blocked; recent files and folder names not visible to AI suggestion engines.",
                ApplyOps = [RegOp.SetDword(Key, "BlockAIFileSystemContext", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockAIFileSystemContext")],
                DetectOps = [RegOp.CheckDword(Key, "BlockAIFileSystemContext", 1)],
            },
            new TweakDef
            {
                Id = "aimod-audit-all-copilot-interactions",
                Label = "Enable Audit Logging for All Copilot/AI Interactions",
                Category = "AI Content Moderation Policy",
                Description =
                    "Enables local event log entries for all Copilot and Windows AI text prompt interactions, creating an audit trail of AI feature usage without sending data to the cloud.",
                Tags = ["ai", "copilot", "audit-log", "compliance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "All Copilot/AI interactions logged locally; usage auditable without cloud telemetry.",
                ApplyOps = [RegOp.SetDword(Key, "AuditAllAIInteractions", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditAllAIInteractions")],
                DetectOps = [RegOp.CheckDword(Key, "AuditAllAIInteractions", 1)],
            },
        ];
}
