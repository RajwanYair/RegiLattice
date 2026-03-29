// RegiLattice.Core — Tweaks/AddRemoveProgramsPolicy.cs
// Add or Remove Programs applet Group Policy controls — Sprint 428.
// Controls visibility and access to the Add/Remove Programs Control Panel applet
// and related application management interfaces via Group Policy registry paths.
// Category: "Add or Remove Programs Policy" | Slug: arpp
// Registry path: HKLM\SOFTWARE\Policies\Microsoft\Windows\AddRemovePrograms

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class AddRemoveProgramsPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AddRemovePrograms";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "arpp-disable-add-remove-programs",
                Label = "Disable Add or Remove Programs Applet",
                Category = "Add or Remove Programs Policy",
                Description =
                    "Sets NoAddRemovePrograms=1 to hide the Add or Remove Programs item from Control Panel entirely. "
                    + "Users cannot access the application management interface for installing, modifying, or removing software. "
                    + "Standard users are prevented from browsing the installed application list via this interface.",
                Tags = ["add-remove-programs", "control-panel", "restriction", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Hides Add/Remove Programs applet entirely; blocks user-initiated software management via Control Panel.",
                ApplyOps = [RegOp.SetDword(Key, "NoAddRemovePrograms", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoAddRemovePrograms")],
                DetectOps = [RegOp.CheckDword(Key, "NoAddRemovePrograms", 1)],
            },
            new TweakDef
            {
                Id = "arpp-hide-add-new-programs",
                Label = "Hide Add New Programs Tab",
                Category = "Add or Remove Programs Policy",
                Description =
                    "Sets NoAddPage=1 to hide the 'Add New Programs' tab in the Add or Remove Programs applet. "
                    + "Users cannot browse or install programs from CD/DVD, floppy disk, or network-hosted installation packages "
                    + "via the Control Panel interface.",
                Tags = ["add-remove-programs", "installation", "restriction", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Removes the Add New Programs tab; users cannot initiate software installs from media via this interface.",
                ApplyOps = [RegOp.SetDword(Key, "NoAddPage", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoAddPage")],
                DetectOps = [RegOp.CheckDword(Key, "NoAddPage", 1)],
            },
            new TweakDef
            {
                Id = "arpp-hide-windows-components",
                Label = "Hide Add/Remove Windows Components Page",
                Category = "Add or Remove Programs Policy",
                Description =
                    "Sets NoWindowsSetupPage=1 to remove the 'Add/Remove Windows Components' page from the applet. "
                    + "Prevents users from adding or removing built-in Windows features and optional components such as "
                    + "IIS, Hyper-V, and legacy subsystem components through the Control Panel interface.",
                Tags = ["add-remove-programs", "windows-features", "restriction", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Hides optional Windows component management page; installed component state unchanged.",
                ApplyOps = [RegOp.SetDword(Key, "NoWindowsSetupPage", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoWindowsSetupPage")],
                DetectOps = [RegOp.CheckDword(Key, "NoWindowsSetupPage", 1)],
            },
            new TweakDef
            {
                Id = "arpp-prevent-program-changes",
                Label = "Prevent Changing Installed Programs",
                Category = "Add or Remove Programs Policy",
                Description =
                    "Sets NoChangePage=1 to hide the Change Program button in Add or Remove Programs. "
                    + "Users are prevented from running application setup programs to modify or repair installed software. "
                    + "This prevents self-repair and configuration changes that could alter application behaviour.",
                Tags = ["add-remove-programs", "modification", "restriction", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Blocks in-place software modification; repair or reconfigure via Add/Remove Programs will not work.",
                ApplyOps = [RegOp.SetDword(Key, "NoChangePage", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoChangePage")],
                DetectOps = [RegOp.CheckDword(Key, "NoChangePage", 1)],
            },
            new TweakDef
            {
                Id = "arpp-block-remove-programs",
                Label = "Block Remove Programs Page",
                Category = "Add or Remove Programs Policy",
                Description =
                    "Sets NoRemovePage=1 to hide the Remove Programs section in Add or Remove Programs. "
                    + "Prevents standard users from uninstalling any software via the Control Panel interface, "
                    + "ensuring software removal requires elevated admin credentials and IT oversight.",
                Tags = ["add-remove-programs", "uninstall", "restriction", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Prevents software removal via Control Panel; uninstall must be performed by an administrator.",
                ApplyOps = [RegOp.SetDword(Key, "NoRemovePage", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoRemovePage")],
                DetectOps = [RegOp.CheckDword(Key, "NoRemovePage", 1)],
            },
            new TweakDef
            {
                Id = "arpp-hide-support-info",
                Label = "Hide Software Support Information Links",
                Category = "Add or Remove Programs Policy",
                Description =
                    "Sets NoSupportInfo=1 to remove the Support Information hyperlink from the Change or Remove Programs list. "
                    + "Prevents disclosure of vendor contact information and URLs embedded in installed software registry entries, "
                    + "reducing information available to users attempting to circumvent IT software management policies.",
                Tags = ["add-remove-programs", "support", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Hides manufacturer support links in the applet list; cosmetic administrative control.",
                ApplyOps = [RegOp.SetDword(Key, "NoSupportInfo", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoSupportInfo")],
                DetectOps = [RegOp.CheckDword(Key, "NoSupportInfo", 1)],
            },
            new TweakDef
            {
                Id = "arpp-block-add-from-network",
                Label = "Block Adding Programs from Network Shares",
                Category = "Add or Remove Programs Policy",
                Description =
                    "Sets NoAddFromNetwork=1 to hide the option to add programs from a network share or corporate distribution point "
                    + "within the Add or Remove Programs applet. Prevents users from browsing network paths and self-installing "
                    + "software packages without IT approval.",
                Tags = ["add-remove-programs", "network", "installation", "restriction", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Removes network-install option; admin/SCCM-pushed deployments are unaffected.",
                ApplyOps = [RegOp.SetDword(Key, "NoAddFromNetwork", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoAddFromNetwork")],
                DetectOps = [RegOp.CheckDword(Key, "NoAddFromNetwork", 1)],
            },
            new TweakDef
            {
                Id = "arpp-hide-services-page",
                Label = "Hide Add/Remove Services Tab",
                Category = "Add or Remove Programs Policy",
                Description =
                    "Sets NoServices=1 to remove the 'Services' tab from the Add or Remove Programs applet, "
                    + "preventing users from enabling or disabling optional system services via this interface. "
                    + "Service state management is restricted to administrators using Services.msc or sc.exe.",
                Tags = ["add-remove-programs", "services", "restriction", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Hides the Services tab; service configuration state is not changed.",
                ApplyOps = [RegOp.SetDword(Key, "NoServices", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoServices")],
                DetectOps = [RegOp.CheckDword(Key, "NoServices", 1)],
            },
            new TweakDef
            {
                Id = "arpp-hide-choose-programs",
                Label = "Hide Set Program Access and Defaults Page",
                Category = "Add or Remove Programs Policy",
                Description =
                    "Sets NoChooseProgramsPage=1 to hide the 'Set Program Access and Computer Defaults' page "
                    + "that allows users to configure default programs for file types and protocols. "
                    + "Complements DefaultBrowserPolicy and file-association GPO controls for consistent default-app enforcement.",
                Tags = ["add-remove-programs", "defaults", "file-associations", "restriction", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Removes the Set Program Access page; users cannot change default program assignments via this interface.",
                ApplyOps = [RegOp.SetDword(Key, "NoChooseProgramsPage", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoChooseProgramsPage")],
                DetectOps = [RegOp.CheckDword(Key, "NoChooseProgramsPage", 1)],
            },
            new TweakDef
            {
                Id = "arpp-enforce-category-view",
                Label = "Enforce Category View in Add/Remove Programs",
                Category = "Add or Remove Programs Policy",
                Description =
                    "Sets DefaultCategory=1 to force the Add or Remove Programs applet to display the installed program list "
                    + "sorted by category as the default view. Overrides any per-user sort preference stored in the user profile, "
                    + "presenting a consistent categorised view for all users on the machine.",
                Tags = ["add-remove-programs", "category", "ui", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Forces category-sorted view in Add/Remove Programs; cosmetic administrative preference only.",
                ApplyOps = [RegOp.SetDword(Key, "DefaultCategory", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DefaultCategory")],
                DetectOps = [RegOp.CheckDword(Key, "DefaultCategory", 1)],
            },
        ];
}
