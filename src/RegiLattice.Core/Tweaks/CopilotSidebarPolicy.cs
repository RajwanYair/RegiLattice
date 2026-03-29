// RegiLattice.Core — Tweaks/CopilotSidebarPolicy.cs
// Windows Copilot sidebar, Copilot key, and Copilot chat UI policy controls — Sprint 485.
// Category: "Copilot Sidebar Policy" | Slug: copsbar
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\WindowsCopilot

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class CopilotSidebarPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsCopilot";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "copsbar-disable-copilot-sidebar",
                Label = "Disable Windows Copilot Sidebar",
                Category = "Copilot Sidebar Policy",
                Description =
                    "Disables the Windows Copilot chat sidebar (the AI assistant panel on the right edge of the screen), removing the Copilot button from the taskbar and preventing the sidebar from opening.",
                Tags = ["copilot", "sidebar", "taskbar", "ai", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Windows Copilot sidebar disabled; Copilot taskbar button removed.",
                ApplyOps = [RegOp.SetDword(Key, "TurnOffWindowsCopilot", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "TurnOffWindowsCopilot")],
                DetectOps = [RegOp.CheckDword(Key, "TurnOffWindowsCopilot", 1)],
            },
            new TweakDef
            {
                Id = "copsbar-disable-copilot-key",
                Label = "Disable the Copilot Hardware Key",
                Category = "Copilot Sidebar Policy",
                Description =
                    "Remaps or disables the dedicated Copilot hardware key found on new Copilot+ keyboards, preventing accidental or unauthorised launch of the Copilot sidebar in enterprise environments.",
                Tags = ["copilot", "keyboard", "copilot-key", "hardware", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Copilot key disabled; pressing the dedicated key does nothing or launches the configured alternative action.",
                ApplyOps = [RegOp.SetDword(Key, "DisableCopilotKey", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCopilotKey")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCopilotKey", 1)],
            },
            new TweakDef
            {
                Id = "copsbar-block-copilot-in-edge",
                Label = "Block Copilot Integration in Microsoft Edge",
                Category = "Copilot Sidebar Policy",
                Description =
                    "Removes the Copilot icon from the Microsoft Edge toolbar and disables AI-powered sidebar features in Edge (summarise, compose, insights), preventing web browsing data from flowing into the Copilot AI.",
                Tags = ["copilot", "edge", "browser", "ai", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Copilot in Edge disabled; AI summarise/compose/insights sidebar not available.",
                ApplyOps = [RegOp.SetDword(Key, "DisableCopilotInEdge", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCopilotInEdge")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCopilotInEdge", 1)],
            },
            new TweakDef
            {
                Id = "copsbar-block-copilot-file-suggestions",
                Label = "Block Copilot File and App Recommendations",
                Category = "Copilot Sidebar Policy",
                Description =
                    "Prevents the Copilot sidebar from suggesting recently opened files, applications, and contacts based on activity history, stopping AI-driven targeted content recommendations in the sidebar.",
                Tags = ["copilot", "file-suggestions", "recommendations", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Copilot file and app recommendations disabled; sidebar shows no suggested content.",
                ApplyOps = [RegOp.SetDword(Key, "DisableCopilotFileSuggestions", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCopilotFileSuggestions")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCopilotFileSuggestions", 1)],
            },
            new TweakDef
            {
                Id = "copsbar-restrict-copilot-to-work-account",
                Label = "Restrict Copilot to Work/School Account Only",
                Category = "Copilot Sidebar Policy",
                Description =
                    "Requires that Windows Copilot is always signed into a work or school Azure AD account rather than a personal Microsoft account, ensuring Copilot interactions are subject to enterprise data governance.",
                Tags = ["copilot", "work-account", "aad", "enterprise", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Copilot restricted to work/school accounts; personal MSA accounts cannot use enterprise Copilot.",
                ApplyOps = [RegOp.SetDword(Key, "RestrictCopilotToWorkAccount", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictCopilotToWorkAccount")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictCopilotToWorkAccount", 1)],
            },
            new TweakDef
            {
                Id = "copsbar-disable-copilot-plugins",
                Label = "Disable Third-Party Plugins for Windows Copilot",
                Category = "Copilot Sidebar Policy",
                Description =
                    "Blocks the installation and use of third-party plugins that extend Windows Copilot with additional skills or API access, limiting Copilot's capability surface to built-in Microsoft functions.",
                Tags = ["copilot", "plugins", "third-party", "extensibility", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Third-party Copilot plugins blocked; only built-in Microsoft Copilot skills available.",
                ApplyOps = [RegOp.SetDword(Key, "DisableCopilotPlugins", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCopilotPlugins")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCopilotPlugins", 1)],
            },
            new TweakDef
            {
                Id = "copsbar-disable-copilot-quick-settings",
                Label = "Remove Copilot Shortcut from Quick Settings Panel",
                Category = "Copilot Sidebar Policy",
                Description =
                    "Removes the Windows Copilot button from the Quick Settings (notification area flyout) panel, applying an additional removal point beyond the taskbar button disable.",
                Tags = ["copilot", "quick-settings", "taskbar", "ui", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Copilot removed from Quick Settings panel and system tray area.",
                ApplyOps = [RegOp.SetDword(Key, "RemoveCopilotFromQuickSettings", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RemoveCopilotFromQuickSettings")],
                DetectOps = [RegOp.CheckDword(Key, "RemoveCopilotFromQuickSettings", 1)],
            },
            new TweakDef
            {
                Id = "copsbar-disable-copilot-context-menu",
                Label = "Disable 'Ask Copilot' Context Menu Entry",
                Category = "Copilot Sidebar Policy",
                Description =
                    "Removes the 'Ask Copilot' right-click context menu entry from Windows Explorer and the desktop, preventing users from submitting files, selections, or queries directly to Copilot from the context menu.",
                Tags = ["copilot", "context-menu", "explorer", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "'Ask Copilot' removed from right-click context menus in Explorer and on the desktop.",
                ApplyOps = [RegOp.SetDword(Key, "DisableCopilotContextMenu", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCopilotContextMenu")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCopilotContextMenu", 1)],
            },
            new TweakDef
            {
                Id = "copsbar-block-copilot-history-sync",
                Label = "Block Copilot Chat History Cloud Sync",
                Category = "Copilot Sidebar Policy",
                Description =
                    "Prevents Copilot from syncing chat history and conversation context to Microsoft cloud servers, keeping all Copilot conversation logs on the local device only.",
                Tags = ["copilot", "chat-history", "cloud-sync", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Copilot history sync disabled; conversation history remains local and is not synced to the cloud.",
                ApplyOps = [RegOp.SetDword(Key, "BlockCopilotHistorySync", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockCopilotHistorySync")],
                DetectOps = [RegOp.CheckDword(Key, "BlockCopilotHistorySync", 1)],
            },
            new TweakDef
            {
                Id = "copsbar-disable-copilot-at-logon",
                Label = "Prevent Copilot from Launching Automatically on First Logon",
                Category = "Copilot Sidebar Policy",
                Description =
                    "Suppresses the automatic first-run Copilot onboarding or sidebar launch that occurs on new user sessions, preventing Copilot from interrupting the user experience on first logon.",
                Tags = ["copilot", "first-run", "logon", "oobe", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Copilot auto-launch on first logon suppressed; no onboarding dialog shown to new users.",
                ApplyOps = [RegOp.SetDword(Key, "SuppressCopilotFirstRun", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "SuppressCopilotFirstRun")],
                DetectOps = [RegOp.CheckDword(Key, "SuppressCopilotFirstRun", 1)],
            },
        ];
}
