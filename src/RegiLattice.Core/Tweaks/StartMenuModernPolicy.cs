// Start Menu & Modern Shell Policy — Sprint 148
// Slug "smmod" — controls Start menu/taskbar layout, People bar, recent apps,
// and Explorer shell behaviors via Group Policy paths not used in other modules.
//
// HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer      (new values)
// HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\StartMenuExperience
#nullable enable
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

internal static class StartMenuModernPolicy
{
    private const string ExplPol =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer";

    private const string SmExp =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\StartMenuExperience";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "smmod-disable-recent-apps-in-start",
            Label = "Start Menu: Disable recently added apps list in Start",
            Category = "Start Menu Modern Policy",
            Description =
                "Sets DisableRecentAppsInStart=1 in StartMenuExperience policy. Hides the 'Recently "
                + "added' section at the top of the Start menu application list.",
            Tags = ["start-menu", "recent-apps", "ui", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(SmExp, "DisableRecentAppsInStart", 1)],
            RemoveOps = [RegOp.DeleteValue(SmExp, "DisableRecentAppsInStart")],
            DetectOps = [RegOp.CheckDword(SmExp, "DisableRecentAppsInStart", 1)],
        },
        new TweakDef
        {
            Id = "smmod-disable-frequently-used-programs",
            Label = "Start Menu: Disable frequently used programs list",
            Category = "Start Menu Modern Policy",
            Description =
                "Sets NoFrequentUsedPrograms=1 in Explorer policy. Prevents Windows from tracking and "
                + "displaying the most frequently launched applications in the Start menu.",
            Tags = ["start-menu", "frequent-apps", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(ExplPol, "NoFrequentUsedPrograms", 1)],
            RemoveOps = [RegOp.DeleteValue(ExplPol, "NoFrequentUsedPrograms")],
            DetectOps = [RegOp.CheckDword(ExplPol, "NoFrequentUsedPrograms", 1)],
        },
        new TweakDef
        {
            Id = "smmod-hide-people-bar",
            Label = "Taskbar: Hide the People bar (contacts flyout)",
            Category = "Start Menu Modern Policy",
            Description =
                "Sets HidePeopleBar=1 in StartMenuExperience policy. Removes the People button from "
                + "the taskbar, hiding the contacts/people flyout feature.",
            Tags = ["taskbar", "people", "contacts", "ui", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(SmExp, "HidePeopleBar", 1)],
            RemoveOps = [RegOp.DeleteValue(SmExp, "HidePeopleBar")],
            DetectOps = [RegOp.CheckDword(SmExp, "HidePeopleBar", 1)],
        },
        new TweakDef
        {
            Id = "smmod-disable-start-recent-docs",
            Label = "Start Menu: Disable recent documents history in Start",
            Category = "Start Menu Modern Policy",
            Description =
                "Sets NoRecentDocsMenu=1 in Explorer policy. Stops Windows from tracking and showing "
                + "a recent-documents shortcut list in the Start menu and jump lists.",
            Tags = ["start-menu", "recent-docs", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(ExplPol, "NoRecentDocsMenu", 1)],
            RemoveOps = [RegOp.DeleteValue(ExplPol, "NoRecentDocsMenu")],
            DetectOps = [RegOp.CheckDword(ExplPol, "NoRecentDocsMenu", 1)],
        },
        new TweakDef
        {
            Id = "smmod-disable-start-app-suggestions",
            Label = "Start Menu: Disable app suggestions / promoted apps in Start",
            Category = "Start Menu Modern Policy",
            Description =
                "Sets DisableRecommendedAppsInStart=1 in StartMenuExperience policy. Removes "
                + "Microsoft-promoted app suggestions and advertisements from the Start menu.",
            Tags = ["start-menu", "suggestions", "ads", "debloat", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(SmExp, "DisableRecommendedAppsInStart", 1)],
            RemoveOps = [RegOp.DeleteValue(SmExp, "DisableRecommendedAppsInStart")],
            DetectOps = [RegOp.CheckDword(SmExp, "DisableRecommendedAppsInStart", 1)],
        },
        new TweakDef
        {
            Id = "smmod-disable-start-recommended-section",
            Label = "Start Menu: Disable the Recommended section in Windows 11 Start",
            Category = "Start Menu Modern Policy",
            Description =
                "Sets DisableRecommendedItemsInStart=1 in StartMenuExperience policy. Hides the "
                + "'Recommended' tile area at the bottom of the Windows 11 Start menu.",
            Tags = ["start-menu", "recommended", "w11", "ui", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(SmExp, "DisableRecommendedItemsInStart", 1)],
            RemoveOps = [RegOp.DeleteValue(SmExp, "DisableRecommendedItemsInStart")],
            DetectOps = [RegOp.CheckDword(SmExp, "DisableRecommendedItemsInStart", 1)],
        },
        new TweakDef
        {
            Id = "smmod-disable-preview-pane",
            Label = "Explorer: Disable the Preview Pane in File Explorer",
            Category = "Start Menu Modern Policy",
            Description =
                "Sets NoPreviewPane=1 in Explorer policy. Disables the Preview Pane panel that "
                + "renders a file preview on the right side of File Explorer windows.",
            Tags = ["explorer", "preview-pane", "ui", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(ExplPol, "NoPreviewPane", 1)],
            RemoveOps = [RegOp.DeleteValue(ExplPol, "NoPreviewPane")],
            DetectOps = [RegOp.CheckDword(ExplPol, "NoPreviewPane", 1)],
        },
        new TweakDef
        {
            Id = "smmod-disable-details-pane",
            Label = "Explorer: Disable the Details Pane in File Explorer",
            Category = "Start Menu Modern Policy",
            Description =
                "Sets NoDetailsPane=1 in Explorer policy. Removes the Details Pane that displays "
                + "file metadata (size, dates, author) on the right side of File Explorer.",
            Tags = ["explorer", "details-pane", "ui", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(ExplPol, "NoDetailsPane", 1)],
            RemoveOps = [RegOp.DeleteValue(ExplPol, "NoDetailsPane")],
            DetectOps = [RegOp.CheckDword(ExplPol, "NoDetailsPane", 1)],
        },
        new TweakDef
        {
            Id = "smmod-disable-taskbar-msa-notification",
            Label = "Taskbar: Disable MSA sign-in notification badge on taskbar",
            Category = "Start Menu Modern Policy",
            Description =
                "Sets DisableMSANotification=1 in StartMenuExperience policy. Suppresses the "
                + "notification badge that prompts users to sign in with a Microsoft Account.",
            Tags = ["taskbar", "msa", "notification", "debloat", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(SmExp, "DisableMSANotification", 1)],
            RemoveOps = [RegOp.DeleteValue(SmExp, "DisableMSANotification")],
            DetectOps = [RegOp.CheckDword(SmExp, "DisableMSANotification", 1)],
        },
        new TweakDef
        {
            Id = "smmod-no-machine-boot-uninstall",
            Label = "Start Menu: Preserve pinned items across machine boot (no uninstall prompt)",
            Category = "Start Menu Modern Policy",
            Description =
                "Sets NoMachineBootUninstall=1 in Explorer policy. Prevents Windows from prompting to "
                + "remove pinned Start menu items for apps that were uninstalled on another user profile.",
            Tags = ["start-menu", "pin", "uninstall", "boot", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(ExplPol, "NoMachineBootUninstall", 1)],
            RemoveOps = [RegOp.DeleteValue(ExplPol, "NoMachineBootUninstall")],
            DetectOps = [RegOp.CheckDword(ExplPol, "NoMachineBootUninstall", 1)],
        },
    ];
}
