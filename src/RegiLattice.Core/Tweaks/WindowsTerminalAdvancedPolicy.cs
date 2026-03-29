// RegiLattice.Core — Tweaks/WindowsTerminalAdvancedPolicy.cs
// Windows Terminal JSON settings storage, update, telemetry, and SSH controls — Sprint 447.
// Category: "Windows Terminal Advanced Policy" | Slug: termadv
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal
//           HKLM\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal\Updates

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WindowsTerminalAdvancedPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal";
    private const string Key2 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal\Updates";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "termadv-disable-auto-update",
                Label = "Disable Windows Terminal Auto-Update",
                Category = "Windows Terminal Advanced Policy",
                Description =
                    "Disables automatic update checks and downloads for Windows Terminal, ensuring the terminal version is managed by WSUS or package management rather than in-app updates.",
                Tags = ["terminal", "update", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Windows Terminal will not auto-update; version management via package manager or WSUS.",
                ApplyOps = [RegOp.SetDword(Key, "DisableAutoUpdate", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoUpdate")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAutoUpdate", 1)],
            },
            new TweakDef
            {
                Id = "termadv-disable-telemetry",
                Label = "Disable Windows Terminal Telemetry",
                Category = "Windows Terminal Advanced Policy",
                Description =
                    "Disables usage telemetry collection in Windows Terminal including keyboard shortcut usage, profile creation frequency, and renderer performance data.",
                Tags = ["terminal", "telemetry", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Terminal telemetry disabled; no usage data sent to Microsoft.",
                ApplyOps = [RegOp.SetDword(Key, "DisableTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "DisableTelemetry", 1)],
            },
            new TweakDef
            {
                Id = "termadv-disable-store-launch",
                Label = "Disable Store Launch from Windows Terminal",
                Category = "Windows Terminal Advanced Policy",
                Description =
                    "Prevents Windows Terminal from launching the Microsoft Store for extensions, themes, or profile suggestions, reducing MS Store telemetry exposure.",
                Tags = ["terminal", "store", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "MS Store launch button in terminal disabled.",
                ApplyOps = [RegOp.SetDword(Key, "DisableStoreLaunch", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableStoreLaunch")],
                DetectOps = [RegOp.CheckDword(Key, "DisableStoreLaunch", 1)],
            },
            new TweakDef
            {
                Id = "termadv-disable-startup-tasks",
                Label = "Disable Windows Terminal Startup Tasks",
                Category = "Windows Terminal Advanced Policy",
                Description =
                    "Disables Windows Terminal startup task registration that auto-starts terminal on user login, reducing unnecessary background process startup.",
                Tags = ["terminal", "startup", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Terminal does not auto-launch at logon.",
                ApplyOps = [RegOp.SetDword(Key, "DisableStartupTasks", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableStartupTasks")],
                DetectOps = [RegOp.CheckDword(Key, "DisableStartupTasks", 1)],
            },
            new TweakDef
            {
                Id = "termadv-enforce-restricted-profile",
                Label = "Enforce Restricted Profile in Windows Terminal",
                Category = "Windows Terminal Advanced Policy",
                Description =
                    "Enables restricted profile enforcement in Windows Terminal, blocking users from modifying terminal profiles, settings JSON, or key bindings.",
                Tags = ["terminal", "profile", "restriction", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Users cannot modify terminal settings; only admin-defined profiles are available.",
                ApplyOps = [RegOp.SetDword(Key, "EnforceRestrictedProfile", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceRestrictedProfile")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceRestrictedProfile", 1)],
            },
            new TweakDef
            {
                Id = "termadv-disable-extensions",
                Label = "Disable Windows Terminal Extensions",
                Category = "Windows Terminal Advanced Policy",
                Description =
                    "Disables the ability to install or run third-party extensions in Windows Terminal, reducing the attack surface from unvetted extension code execution.",
                Tags = ["terminal", "extensions", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Terminal extensions disabled; only built-in functionality available.",
                ApplyOps = [RegOp.SetDword(Key, "DisableExtensions", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableExtensions")],
                DetectOps = [RegOp.CheckDword(Key, "DisableExtensions", 1)],
            },
            new TweakDef
            {
                Id = "termadv-block-ssh-agent",
                Label = "Block SSH Agent Integration in Windows Terminal",
                Category = "Windows Terminal Advanced Policy",
                Description =
                    "Disables the SSH agent forwarding integration in Windows Terminal, preventing terminal sessions from forwarding SSH keys to remote hosts.",
                Tags = ["terminal", "ssh", "agent", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "SSH agent forwarding blocked from terminal; prevents key forwarding to hostile servers.",
                ApplyOps = [RegOp.SetDword(Key, "BlockSshAgentIntegration", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockSshAgentIntegration")],
                DetectOps = [RegOp.CheckDword(Key, "BlockSshAgentIntegration", 1)],
            },
            new TweakDef
            {
                Id = "termadv-disable-preview-builds",
                Label = "Disable Windows Terminal Preview Build Channel",
                Category = "Windows Terminal Advanced Policy",
                Description =
                    "Forces Windows Terminal to the stable release channel, disabling the Preview and Canary build channels to ensure only stable, vetted versions are used.",
                Tags = ["terminal", "preview", "channel", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Terminal locked to stable channel; Preview/Canary builds not offered.",
                ApplyOps = [RegOp.SetDword(Key2, "DisablePreviewBuilds", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "DisablePreviewBuilds")],
                DetectOps = [RegOp.CheckDword(Key2, "DisablePreviewBuilds", 1)],
            },
            new TweakDef
            {
                Id = "termadv-disable-update-notifications",
                Label = "Disable Update Notifications in Windows Terminal",
                Category = "Windows Terminal Advanced Policy",
                Description =
                    "Suppresses in-app update available notifications in Windows Terminal, which can distract users and prompt unauthorized manual updates.",
                Tags = ["terminal", "update", "notifications", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Update reminder banners not shown in terminal.",
                ApplyOps = [RegOp.SetDword(Key2, "DisableUpdateNotifications", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "DisableUpdateNotifications")],
                DetectOps = [RegOp.CheckDword(Key2, "DisableUpdateNotifications", 1)],
            },
            new TweakDef
            {
                Id = "termadv-block-manual-updates",
                Label = "Block Manual Windows Terminal Updates by Users",
                Category = "Windows Terminal Advanced Policy",
                Description =
                    "Prevents standard users from triggering manual Windows Terminal update checks or downloads, ensuring that all terminal update operations require administrator rights.",
                Tags = ["terminal", "update", "restriction", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Standard users cannot manually update terminal; admin action required.",
                ApplyOps = [RegOp.SetDword(Key2, "BlockManualUpdates", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "BlockManualUpdates")],
                DetectOps = [RegOp.CheckDword(Key2, "BlockManualUpdates", 1)],
            },
        ];
}
