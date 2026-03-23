// RegiLattice.Core — Tweaks/ShellRestrictionsPolicy.cs
// Category: "Shell Restrictions Policy" — Slug "shellrst"
// HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer
// Machine-wide shell restrictions used in hardened / kiosk deployments.

#nullable enable

using System.Collections.Generic;
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

internal static class ShellRestrictionsPolicy
{
    private const string Pol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "shellrst-no-run-dialog",
            Label = "Remove Run from Start Menu",
            Category = "Shell Restrictions Policy",
            Description =
                "Sets NoRun=1 in Policies\\Explorer. Removes the Run entry from the Start menu and disables the Win+R shortcut, closing an easy execution vector for arbitrary programs. Default: Run is visible.",
            Tags = ["shell", "restriction", "policy", "gpo", "kiosk"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(Pol, "NoRun", 1)],
            RemoveOps = [RegOp.DeleteValue(Pol, "NoRun")],
            DetectOps = [RegOp.CheckDword(Pol, "NoRun", 1)],
        },
        new TweakDef
        {
            Id = "shellrst-no-find-command",
            Label = "Remove Find/Search from Start Menu",
            Category = "Shell Restrictions Policy",
            Description =
                "Sets NoFind=1 in Policies\\Explorer. Removes the Find/Search shortcut and menu item from the Start menu. Prevents quick enumeration of file system contents via the built-in search dialog. Default: Find is visible.",
            Tags = ["shell", "restriction", "search", "policy", "gpo"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(Pol, "NoFind", 1)],
            RemoveOps = [RegOp.DeleteValue(Pol, "NoFind")],
            DetectOps = [RegOp.CheckDword(Pol, "NoFind", 1)],
        },
        new TweakDef
        {
            Id = "shellrst-no-close-session",
            Label = "Remove Shut Down from Start Menu",
            Category = "Shell Restrictions Policy",
            Description =
                "Sets NoClose=1 in Policies\\Explorer. Removes the Shut Down, Restart, and Sleep options from the Start menu. Useful for kiosk machines or shared terminals where the power state should be managed by administrators only.",
            Tags = ["shell", "restriction", "shutdown", "policy", "kiosk"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(Pol, "NoClose", 1)],
            RemoveOps = [RegOp.DeleteValue(Pol, "NoClose")],
            DetectOps = [RegOp.CheckDword(Pol, "NoClose", 1)],
        },
        new TweakDef
        {
            Id = "shellrst-no-logoff-menu",
            Label = "Remove Log Off from Start Menu",
            Category = "Shell Restrictions Policy",
            Description =
                "Sets NoLogoff=1 in Policies\\Explorer. Removes the Log Off entry from the Start menu, preventing users from signing out via the Start menu shortcut. Session management must be performed through other means (Task Manager, CTRL+ALT+DEL).",
            Tags = ["shell", "restriction", "logoff", "policy", "gpo"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(Pol, "NoLogoff", 1)],
            RemoveOps = [RegOp.DeleteValue(Pol, "NoLogoff")],
            DetectOps = [RegOp.CheckDword(Pol, "NoLogoff", 1)],
        },
        new TweakDef
        {
            Id = "shellrst-no-desktop-icons",
            Label = "Hide All Desktop Icons",
            Category = "Shell Restrictions Policy",
            Description =
                "Sets NoDesktop=1 in Policies\\Explorer. Removes all icons from the desktop surface including This PC, Recycle Bin, and user-placed shortcuts. Desktop background is still visible. Used to create clean-slate kiosk or thin-client desktops.",
            Tags = ["shell", "restriction", "desktop", "policy", "kiosk"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(Pol, "NoDesktop", 1)],
            RemoveOps = [RegOp.DeleteValue(Pol, "NoDesktop")],
            DetectOps = [RegOp.CheckDword(Pol, "NoDesktop", 1)],
        },
        new TweakDef
        {
            Id = "shellrst-no-drives-page",
            Label = "Remove Drives Tab from Computer Properties",
            Category = "Shell Restrictions Policy",
            Description =
                "Sets NoDrivesPage=1 in Policies\\Explorer. Removes the Drives tab from the Hardware and Storage area in System Properties, preventing detailed enumeration of physical drive properties. Reduces information leakage on shared systems.",
            Tags = ["shell", "restriction", "drives", "policy", "gpo"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(Pol, "NoDrivesPage", 1)],
            RemoveOps = [RegOp.DeleteValue(Pol, "NoDrivesPage")],
            DetectOps = [RegOp.CheckDword(Pol, "NoDrivesPage", 1)],
        },
        new TweakDef
        {
            Id = "shellrst-no-control-panel-applets",
            Label = "Block All Control Panel Applets",
            Category = "Shell Restrictions Policy",
            Description =
                "Sets NoCplApplets=1 in Policies\\Explorer. Prevents all Control Panel .cpl applets from launching, including Display, Sound, Network, System, etc. Combined with the GPO applet allow-list this creates a restricted-access Control Panel for standard users.",
            Tags = ["shell", "restriction", "controlpanel", "policy", "gpo"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(Pol, "NoCplApplets", 1)],
            RemoveOps = [RegOp.DeleteValue(Pol, "NoCplApplets")],
            DetectOps = [RegOp.CheckDword(Pol, "NoCplApplets", 1)],
        },
        new TweakDef
        {
            Id = "shellrst-no-display-cpl",
            Label = "Hide Display Control Panel Applet",
            Category = "Shell Restrictions Policy",
            Description =
                "Sets NoDispCPL=1 in Policies\\Explorer. Prevents users from opening the Display applet from Control Panel or the desktop right-click menu, blocking wallpaper, resolution, and colour depth changes. Used to enforce corporate visual standards.",
            Tags = ["shell", "restriction", "display", "policy", "gpo"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(Pol, "NoDispCPL", 1)],
            RemoveOps = [RegOp.DeleteValue(Pol, "NoDispCPL")],
            DetectOps = [RegOp.CheckDword(Pol, "NoDispCPL", 1)],
        },
        new TweakDef
        {
            Id = "shellrst-restrict-run-list",
            Label = "Enable DisallowRun Application Restriction",
            Category = "Shell Restrictions Policy",
            Description =
                "Sets DisallowRun=1 in Policies\\Explorer. Activates the DisallowRun enforcement mode, which blocks execution of any application names listed under the adjacent DisallowRun sub-key. Enables per-application deny-listing without requiring AppLocker or WDAC.",
            Tags = ["shell", "restriction", "allowlist", "policy", "gpo"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(Pol, "DisallowRun", 1)],
            RemoveOps = [RegOp.DeleteValue(Pol, "DisallowRun")],
            DetectOps = [RegOp.CheckDword(Pol, "DisallowRun", 1)],
        },
        new TweakDef
        {
            Id = "shellrst-no-network-neighborhood",
            Label = "Hide Network Neighborhood from Explorer",
            Category = "Shell Restrictions Policy",
            Description =
                "Sets NoNetHood=1 in Policies\\Explorer. Removes the Network Neighborhood (Network Places) icon from Explorer and the desktop. Users can still access UNC paths directly; this only removes the browsable discovery pane that enumerates visible network shares.",
            Tags = ["shell", "restriction", "network", "policy", "gpo"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(Pol, "NoNetHood", 1)],
            RemoveOps = [RegOp.DeleteValue(Pol, "NoNetHood")],
            DetectOps = [RegOp.CheckDword(Pol, "NoNetHood", 1)],
        },
    ];
}
