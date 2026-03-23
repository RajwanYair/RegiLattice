#nullable enable
using RegiLattice.Core.Models;
using System.Collections.Generic;

namespace RegiLattice.Core.Tweaks;

// Slug "smartscr" — Windows Defender SmartScreen GPO policy.
// SOFTWARE\Policies\Microsoft\Windows\System (shell SmartScreen)
// SOFTWARE\Policies\Microsoft\Windows Defender\SmartScreen (app install control)
// SOFTWARE\Policies\Microsoft\MicrosoftEdge\PhishingFilter (IE/legacy Edge)
// SOFTWARE\Policies\Microsoft\Edge (new Chromium Edge SmartScreen)
internal static class SmartScreenPolicy
{
    private const string WinSys = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";
    private const string DefSS = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\SmartScreen";
    private const string IEPhish = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MicrosoftEdge\PhishingFilter";
    private const string EdgePol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "smartscr-enable-shell",
            Label = "Enable Windows SmartScreen (shell)",
            Category = "SmartScreen Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables Windows Defender SmartScreen for file and app checks in Explorer/shell via GPO. "
                + "EnableSmartScreen=1. Default: enabled. Recommended: enforced via policy.",
            Tags = ["smartscreen", "shell", "security", "policy"],
            ApplyOps = [RegOp.SetDword(WinSys, "EnableSmartScreen", 1)],
            RemoveOps = [RegOp.DeleteValue(WinSys, "EnableSmartScreen")],
            DetectOps = [RegOp.CheckDword(WinSys, "EnableSmartScreen", 1)],
        },
        new TweakDef
        {
            Id = "smartscr-shell-block-level",
            Label = "Set SmartScreen shell level to Block",
            Category = "SmartScreen Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Forces SmartScreen to block unknown/malicious files instead of just warning. "
                + "ShellSmartScreenLevel=Block. Default: Warn.",
            Tags = ["smartscreen", "shell", "block", "policy"],
            ApplyOps = [RegOp.SetString(WinSys, "ShellSmartScreenLevel", "Block")],
            RemoveOps = [RegOp.DeleteValue(WinSys, "ShellSmartScreenLevel")],
            DetectOps = [RegOp.CheckString(WinSys, "ShellSmartScreenLevel", "Block")],
        },
        new TweakDef
        {
            Id = "smartscr-app-install-control-enabled",
            Label = "Enable Defender SmartScreen app install control",
            Category = "SmartScreen Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Activates the policy that controls app installation via Windows Defender SmartScreen. "
                + "ConfigureAppInstallControlEnabled=1. Required before ConfigureAppInstallControl takes effect.",
            Tags = ["smartscreen", "defender", "app", "install", "policy"],
            ApplyOps = [RegOp.SetDword(DefSS, "ConfigureAppInstallControlEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(DefSS, "ConfigureAppInstallControlEnabled")],
            DetectOps = [RegOp.CheckDword(DefSS, "ConfigureAppInstallControlEnabled", 1)],
        },
        new TweakDef
        {
            Id = "smartscr-recommend-store-only",
            Label = "Warn on non-Store app installs (SmartScreen)",
            Category = "SmartScreen Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Configures SmartScreen to recommend the Microsoft Store and warn on non-Store app installs. "
                + "ConfigureAppInstallControl=Recommend. Use StoreOnly to block all non-Store apps.",
            Tags = ["smartscreen", "defender", "store", "install", "policy"],
            ApplyOps = [RegOp.SetString(DefSS, "ConfigureAppInstallControl", "Recommend")],
            RemoveOps = [RegOp.DeleteValue(DefSS, "ConfigureAppInstallControl")],
            DetectOps = [RegOp.CheckString(DefSS, "ConfigureAppInstallControl", "Recommend")],
        },
        new TweakDef
        {
            Id = "smartscr-ie-phishing-filter",
            Label = "Enable IE/Edge Legacy SmartScreen phishing filter",
            Category = "SmartScreen Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables the IE/Edge Legacy SmartScreen phishing filter via policy. "
                + "EnabledV9=1. Blocks known phishing sites and malware downloads.",
            Tags = ["smartscreen", "ie", "phishing", "policy"],
            ApplyOps = [RegOp.SetDword(IEPhish, "EnabledV9", 1)],
            RemoveOps = [RegOp.DeleteValue(IEPhish, "EnabledV9")],
            DetectOps = [RegOp.CheckDword(IEPhish, "EnabledV9", 1)],
        },
        new TweakDef
        {
            Id = "smartscr-ie-prevent-site-override",
            Label = "Prevent user bypassing SmartScreen for malicious sites",
            Category = "SmartScreen Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents users from bypassing SmartScreen filter warnings for malicious websites in IE/Edge Legacy. "
                + "PreventOverride=1.",
            Tags = ["smartscreen", "ie", "override", "policy"],
            ApplyOps = [RegOp.SetDword(IEPhish, "PreventOverride", 1)],
            RemoveOps = [RegOp.DeleteValue(IEPhish, "PreventOverride")],
            DetectOps = [RegOp.CheckDword(IEPhish, "PreventOverride", 1)],
        },
        new TweakDef
        {
            Id = "smartscr-ie-prevent-app-rep-override",
            Label = "Prevent user bypassing SmartScreen for unknown apps",
            Category = "SmartScreen Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents users from bypassing SmartScreen warnings for files with unknown application reputation. "
                + "PreventOverrideAppRepUnknown=1.",
            Tags = ["smartscreen", "ie", "app-rep", "override", "policy"],
            ApplyOps = [RegOp.SetDword(IEPhish, "PreventOverrideAppRepUnknown", 1)],
            RemoveOps = [RegOp.DeleteValue(IEPhish, "PreventOverrideAppRepUnknown")],
            DetectOps = [RegOp.CheckDword(IEPhish, "PreventOverrideAppRepUnknown", 1)],
        },
        new TweakDef
        {
            Id = "smartscr-edge-enable",
            Label = "Enable Microsoft Edge SmartScreen (policy)",
            Category = "SmartScreen Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables Windows Defender SmartScreen in Microsoft Edge via managed GPO policy. "
                + "SmartScreenEnabled=1. Recommended: always enforced.",
            Tags = ["smartscreen", "edge", "security", "policy"],
            ApplyOps = [RegOp.SetDword(EdgePol, "SmartScreenEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(EdgePol, "SmartScreenEnabled")],
            DetectOps = [RegOp.CheckDword(EdgePol, "SmartScreenEnabled", 1)],
        },
        new TweakDef
        {
            Id = "smartscr-edge-pua-block",
            Label = "Enable Edge SmartScreen PUA blocking (policy)",
            Category = "SmartScreen Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables Microsoft Edge SmartScreen detection and blocking of Potentially Unwanted Applications (PUA). "
                + "SmartScreenPuaEnabled=1.",
            Tags = ["smartscreen", "edge", "pua", "policy"],
            ApplyOps = [RegOp.SetDword(EdgePol, "SmartScreenPuaEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(EdgePol, "SmartScreenPuaEnabled")],
            DetectOps = [RegOp.CheckDword(EdgePol, "SmartScreenPuaEnabled", 1)],
        },
        new TweakDef
        {
            Id = "smartscr-edge-force-enabled",
            Label = "Force-enable Edge SmartScreen (user cannot disable)",
            Category = "SmartScreen Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Forces Edge SmartScreen on and prevents users from turning it off. "
                + "SmartScreenForceEnabled=1. Stronger than SmartScreenEnabled alone.",
            Tags = ["smartscreen", "edge", "force", "policy"],
            ApplyOps = [RegOp.SetDword(EdgePol, "SmartScreenForceEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(EdgePol, "SmartScreenForceEnabled")],
            DetectOps = [RegOp.CheckDword(EdgePol, "SmartScreenForceEnabled", 1)],
        },
    ];
}
