// RegiLattice.Core — Tweaks/AiCopilotWebPolicy.cs
// AI Copilot Web Policy — Sprint 553.
// Configures Group Policy for Microsoft Copilot web experience, generative
// AI features in Windows and Edge, content filtering, data sharing restrictions,
// and Copilot commercial data protection settings.
// Category: "AI Copilot Web Policy" | Slug: aicw
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\AI\Copilot
//           HKLM\SOFTWARE\Policies\Microsoft\Edge

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class AiCopilotWebPolicy
{
    private const string CopilotKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AI\Copilot";

    private const string EdgeAiKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "aicw-disable-copilot-taskbar",
                Label = "AI Copilot Web: Disable Copilot Button from Windows Taskbar",
                Category = "AI Copilot Web Policy",
                Description =
                    "Sets TurnOffWindowsCopilot=1 in Windows Copilot policy. Removes the Copilot button from the Windows 11 taskbar and disables the keyboard shortcut (Win+C). When Copilot is disabled, the AI assistant panel does not appear and no connection is made to Microsoft's Copilot cloud services from the taskbar entry point. Appropriate for organisations that have not yet adopted Copilot, operate under data sovereignty policies that restrict AI interactions with Microsoft endpoints, or wish to prevent distraction-driven AI usage.",
                Tags = ["copilot", "taskbar", "ai", "windows11", "enterprise"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Copilot taskbar button and Win+C shortcut are removed. Users cannot access Copilot from the taskbar. Copilot in browser or M365 applications is controlled separately.",
                ApplyOps = [RegOp.SetDword(CopilotKey, "TurnOffWindowsCopilot", 1)],
                RemoveOps = [RegOp.DeleteValue(CopilotKey, "TurnOffWindowsCopilot")],
                DetectOps = [RegOp.CheckDword(CopilotKey, "TurnOffWindowsCopilot", 1)],
            },
            new TweakDef
            {
                Id = "aicw-enable-commercial-data-protection",
                Label = "AI Copilot Web: Enable Commercial Data Protection in Copilot",
                Category = "AI Copilot Web Policy",
                Description =
                    "Sets CommercialDataProtection=1 in Copilot policy. Activates Microsoft's commercial data protection tier for Copilot interactions from enterprise accounts. With commercial data protection enabled, Copilot prompts and responses are processed under Microsoft's DPA commitments: prompts are not used to train foundation models, output is not retained beyond the session, and the connection is isolated from consumer Copilot traffic. Required for organisations whose employees interact with proprietary information through Copilot.",
                Tags = ["copilot", "data-protection", "enterprise", "compliance", "ai"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Commercial data protection is activated for Copilot interactions. Requires users to be signed in with a Microsoft 365 commercial account. Consumer-tier protection is automatically replaced with enterprise-tier.",
                ApplyOps = [RegOp.SetDword(CopilotKey, "CommercialDataProtection", 1)],
                RemoveOps = [RegOp.DeleteValue(CopilotKey, "CommercialDataProtection")],
                DetectOps = [RegOp.CheckDword(CopilotKey, "CommercialDataProtection", 1)],
            },
            new TweakDef
            {
                Id = "aicw-disable-copilot-image-creator",
                Label = "AI Copilot Web: Disable Copilot AI Image Creator Feature",
                Category = "AI Copilot Web Policy",
                Description =
                    "Sets DisableImageCreator=1 in Copilot policy. Disables the image generation capability in Microsoft Copilot (powered by DALL-E models). Users cannot create AI-generated images from text prompts through Windows Copilot or Edge's Copilot integration. Image generation carries content policy risks (CSAM generation attempts, IP violation complaints, deepfake content), copyright concerns, and may violate acceptable use policies in educational and professional environments.",
                Tags = ["copilot", "image-creator", "ai", "content-policy", "enterprise"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "AI image generation is disabled in Copilot. Text-based Copilot features are unaffected. Recommended for K-12 education environments and organisations with strict content policies.",
                ApplyOps = [RegOp.SetDword(CopilotKey, "DisableImageCreator", 1)],
                RemoveOps = [RegOp.DeleteValue(CopilotKey, "DisableImageCreator")],
                DetectOps = [RegOp.CheckDword(CopilotKey, "DisableImageCreator", 1)],
            },
            new TweakDef
            {
                Id = "aicw-disable-edge-copilot-sidebar",
                Label = "AI Copilot Web: Disable Copilot Sidebar in Microsoft Edge",
                Category = "AI Copilot Web Policy",
                Description =
                    "Sets HubsSidebarEnabled=0 in Edge policy. Removes the Copilot and Discover sidebar panel from Microsoft Edge. The sidebar contains AI-powered summarisation, writing assistance, and web search features. When disabled, clicking the sidebar toggle button has no effect and the panel does not appear. Reduces distractions in focused work environments, removes browser-based AI features that might transmit page content to cloud services, and simplifies the Edge UI for corporate deployments.",
                Tags = ["copilot", "edge", "sidebar", "browser", "enterprise"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Edge Copilot/Discover sidebar is hidden. AI writing and summarisation features in the Edge sidebar are unavailable. Core browser functionality is unchanged.",
                ApplyOps = [RegOp.SetDword(EdgeAiKey, "HubsSidebarEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeAiKey, "HubsSidebarEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeAiKey, "HubsSidebarEnabled", 0)],
            },
            new TweakDef
            {
                Id = "aicw-disable-bing-ai-chat",
                Label = "AI Copilot Web: Disable Bing AI Chat in Edge Search",
                Category = "AI Copilot Web Policy",
                Description =
                    "Sets BingAIChatEnabled=0 in Edge policy. Disables the Bing AI Chat (Copilot in Bing) entry point in Edge address bar suggestions and the Edge sidebar. Bing AI Chat sends the user's query and optionally the current page content to Microsoft's Bing AI backend for processing. Disabling this prevents inadvertent data disclosure through AI chat queries and maintains consistent browser behaviour across managed devices where AI search features have not been approved.",
                Tags = ["copilot", "bing", "chat", "browser", "ai"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Bing AI Chat is disabled in Edge search and sidebar. Standard Bing web search is unaffected. Chat button and AI suggestions in search results do not appear.",
                ApplyOps = [RegOp.SetDword(EdgeAiKey, "BingAIChatEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(EdgeAiKey, "BingAIChatEnabled")],
                DetectOps = [RegOp.CheckDword(EdgeAiKey, "BingAIChatEnabled", 0)],
            },
            new TweakDef
            {
                Id = "aicw-disable-page-context-sharing",
                Label = "AI Copilot Web: Disable Page Content Sharing with Copilot",
                Category = "AI Copilot Web Policy",
                Description =
                    "Sets SendPageInfoToCopilot=0 in Copilot policy. Prevents Copilot from automatically receiving the current web page's text content, URL, and metadata when the user opens the Copilot panel while browsing. Page context sharing is used to power 'summarise this page' and contextual chat features, but it transmits the full document content to Microsoft's AI services. In environments where employees browse sensitive internal portals, intranet pages, or classified content, page context sharing creates an inadvertent data exfiltration risk.",
                Tags = ["copilot", "page-context", "privacy", "data-sharing", "browser"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Copilot does not receive current page content. Page summarisation and context-aware Copilot responses are disabled. Users can still ask general questions through Copilot.",
                ApplyOps = [RegOp.SetDword(CopilotKey, "SendPageInfoToCopilot", 0)],
                RemoveOps = [RegOp.DeleteValue(CopilotKey, "SendPageInfoToCopilot")],
                DetectOps = [RegOp.CheckDword(CopilotKey, "SendPageInfoToCopilot", 0)],
            },
            new TweakDef
            {
                Id = "aicw-disable-copilot-history",
                Label = "AI Copilot Web: Disable Copilot Conversation History Storage",
                Category = "AI Copilot Web Policy",
                Description =
                    "Sets DisableCopilotHistory=1 in Copilot policy. Prevents Copilot from storing a user's conversation history in the cloud. By default, Copilot maintains a conversation history that allows users to continue previous sessions and view past interactions. For organisations with data minimisation obligations (GDPR, CCPA), storing AI conversation history including employee queries and AI responses may constitute personal data processing that requires explicit consent and a retention policy. Disabling history means each session starts fresh.",
                Tags = ["copilot", "history", "privacy", "gdpr", "data-minimisation"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Copilot conversation history is not stored. Each session starts without context from prior sessions. Users cannot view or continue previous Copilot conversations.",
                ApplyOps = [RegOp.SetDword(CopilotKey, "DisableCopilotHistory", 1)],
                RemoveOps = [RegOp.DeleteValue(CopilotKey, "DisableCopilotHistory")],
                DetectOps = [RegOp.CheckDword(CopilotKey, "DisableCopilotHistory", 1)],
            },
            new TweakDef
            {
                Id = "aicw-restrict-copilot-to-work-account",
                Label = "AI Copilot Web: Restrict Copilot Access to Work Microsoft Account",
                Category = "AI Copilot Web Policy",
                Description =
                    "Sets RestrictCopilotToWorkAccount=1 in Copilot policy. Limits Copilot access to users signed in with a Microsoft 365 work or school account in the current browser profile. Users signed in with personal Microsoft accounts cannot use Copilot on managed devices. This ensures all Copilot sessions are covered by the organisation's Microsoft 365 agreement and commercial data protection. Prevents mixing personal free-tier Copilot (no commercial data protection) with corporate usage.",
                Tags = ["copilot", "work-account", "microsoft365", "enterprise", "access-control"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Copilot is only available when signed in with a work/school Microsoft account. Personal account users are redirected to sign in with a work account. M365 commercial subscription required.",
                ApplyOps = [RegOp.SetDword(CopilotKey, "RestrictCopilotToWorkAccount", 1)],
                RemoveOps = [RegOp.DeleteValue(CopilotKey, "RestrictCopilotToWorkAccount")],
                DetectOps = [RegOp.CheckDword(CopilotKey, "RestrictCopilotToWorkAccount", 1)],
            },
            new TweakDef
            {
                Id = "aicw-disable-copilot-screen-capture",
                Label = "AI Copilot Web: Disable Screen Capture by Copilot AI Features",
                Category = "AI Copilot Web Policy",
                Description =
                    "Sets AllowCopilotScreenAccess=0 in Copilot policy. Prevents Copilot AI features from capturing the current screen or requesting access to screen content for visual analysis. Some Copilot capabilities (Look Up in Copilot, Visual Search) capture the current screen or selected region and send it to AI services for processing. Screen capture by AI features is a significant data sensitivity risk when the screen displays sensitive documents, financial data, or personal information.",
                Tags = ["copilot", "screen-capture", "privacy", "visual-ai", "data-protection"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Screen capture access for Copilot is disabled. Visual AI features (Look Up, Visual Search) are unavailable. Text-based Copilot interactions are unaffected.",
                ApplyOps = [RegOp.SetDword(CopilotKey, "AllowCopilotScreenAccess", 0)],
                RemoveOps = [RegOp.DeleteValue(CopilotKey, "AllowCopilotScreenAccess")],
                DetectOps = [RegOp.CheckDword(CopilotKey, "AllowCopilotScreenAccess", 0)],
            },
            new TweakDef
            {
                Id = "aicw-disable-copilot-clipboard-access",
                Label = "AI Copilot Web: Disable Copilot Automatic Clipboard Content Reading",
                Category = "AI Copilot Web Policy",
                Description =
                    "Sets AllowCopilotClipboardAccess=0 in Copilot policy. Prevents Copilot from automatically monitoring or reading clipboard contents when the Copilot panel is open. Some Copilot implementations automatically suggest analysis or formatting assistance when the user copies text to the clipboard while Copilot is visible. Clipboard content frequently contains sensitive data (passwords, API keys, confidential document excerpts). Disabling automatic clipboard access prevents unintentional AI processing of clipboard contents.",
                Tags = ["copilot", "clipboard", "privacy", "data-loss", "access-control"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Copilot does not automatically read clipboard contents. Users can still manually paste content into the Copilot chat. Prevents accidental AI processing of sensitive copied data.",
                ApplyOps = [RegOp.SetDword(CopilotKey, "AllowCopilotClipboardAccess", 0)],
                RemoveOps = [RegOp.DeleteValue(CopilotKey, "AllowCopilotClipboardAccess")],
                DetectOps = [RegOp.CheckDword(CopilotKey, "AllowCopilotClipboardAccess", 0)],
            },
        ];
}
