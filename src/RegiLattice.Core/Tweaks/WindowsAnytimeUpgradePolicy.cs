// RegiLattice.Core — Tweaks/WindowsAnytimeUpgradePolicy.cs
// Windows Anytime Upgrade (WAU) GPO controls — Sprint 216.
// Prevents unauthorized OS edition upgrades via WAU or the Store.
// Category: "Windows Anytime Upgrade Policy" | Slug: wanyu
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAnytimeUpgrade

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WindowsAnytimeUpgradePolicy
{
    private const string Key =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAnytimeUpgrade";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "wanyu-disable-anytime-upgrade",
                Label = "Disable Windows Anytime Upgrade",
                Category = "Windows Anytime Upgrade Policy",
                Description =
                    "Prevents users from launching Windows Anytime Upgrade to purchase and install a higher-edition license key. On managed corporate devices the OS edition is centrally managed; users should not be able to self-upgrade. Default: Anytime Upgrade accessible. Recommended: 1.",
                Tags = ["anytime-upgrade", "edition", "upgrade", "store", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Windows Anytime Upgrade entry point is removed from the system; users cannot initiate an edition upgrade.",
                ApplyOps = [RegOp.SetDword(Key, "Disabled", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "Disabled")],
                DetectOps = [RegOp.CheckDword(Key, "Disabled", 1)],
            },
            new TweakDef
            {
                Id = "wanyu-disable-upgrade-via-store",
                Label = "Disable OS Upgrade via Microsoft Store",
                Category = "Windows Anytime Upgrade Policy",
                Description =
                    "Prevents users from upgrading the operating system edition (e.g., Home → Pro, or Pro → Enterprise) via the Microsoft Store upgrade pathways. Keeps OS edition under IT control on managed devices. Default: Store-based edition upgrade permitted. Recommended: 1.",
                Tags = ["anytime-upgrade", "edition", "store", "upgrade", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Microsoft Store OS edition upgrade path is blocked; edition remains as deployed by IT.",
                ApplyOps = [RegOp.SetDword(Key, "DisableStoreUpgrade", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableStoreUpgrade")],
                DetectOps = [RegOp.CheckDword(Key, "DisableStoreUpgrade", 1)],
            },
            new TweakDef
            {
                Id = "wanyu-block-key-entry-ui",
                Label = "Block Product Key Entry for Edition Upgrade",
                Category = "Windows Anytime Upgrade Policy",
                Description =
                    "Removes the 'Change product key' button from Settings → Update & Security → Activation that would allow a user to enter a higher-edition key and trigger an in-place upgrade. Prevents unauthorized edition changes by typing a key. Default: key entry available. Recommended: 1.",
                Tags = ["anytime-upgrade", "product-key", "activation", "edition", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Product key entry for in-place edition upgrade is removed from the Activation Settings page.",
                ApplyOps = [RegOp.SetDword(Key, "BlockKeyEntry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockKeyEntry")],
                DetectOps = [RegOp.CheckDword(Key, "BlockKeyEntry", 1)],
            },
            new TweakDef
            {
                Id = "wanyu-log-upgrade-attempts",
                Label = "Log Windows Anytime Upgrade Attempts",
                Category = "Windows Anytime Upgrade Policy",
                Description =
                    "Records an Application event log entry whenever a user attempts to initiate a Windows Anytime Upgrade, whether blocked by policy or not. Useful for detecting users who are trying to bypass edition controls. Default: attempts not logged. Recommended: 1 on monitored endpoints.",
                Tags = ["anytime-upgrade", "audit", "logging", "edition", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Any attempt to start an edition upgrade is logged to the Application event log.",
                ApplyOps = [RegOp.SetDword(Key, "LogUpgradeAttempts", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "LogUpgradeAttempts")],
                DetectOps = [RegOp.CheckDword(Key, "LogUpgradeAttempts", 1)],
            },
            new TweakDef
            {
                Id = "wanyu-disable-upgrade-notification",
                Label = "Suppress Windows Anytime Upgrade Notifications",
                Category = "Windows Anytime Upgrade Policy",
                Description =
                    "Suppresses promotional notifications and prompts that encourage users to purchase a higher Windows edition (e.g., 'Upgrade to Pro for these features'). Removes upsell nags from the UI without affecting the installed edition. Default: notifications displayed. Recommended: 1.",
                Tags = ["anytime-upgrade", "notification", "ui", "edition", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Edition upgrade promotional notifications and upsell banners are suppressed.",
                ApplyOps = [RegOp.SetDword(Key, "DisableUpgradeNotifications", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableUpgradeNotifications")],
                DetectOps = [RegOp.CheckDword(Key, "DisableUpgradeNotifications", 1)],
            },
            new TweakDef
            {
                Id = "wanyu-prevent-downgrade",
                Label = "Prevent Windows Edition Downgrade via Policy",
                Category = "Windows Anytime Upgrade Policy",
                Description =
                    "Prevents edition downgrades (e.g., Enterprise → Pro rollback) via key entry or the Activation Store. Protects against licence audit circumvention where a device could be temporarily downgraded. Default: downgrade via key entry possible. Recommended: 1.",
                Tags = ["anytime-upgrade", "downgrade", "edition", "activation", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Edition downgrade through Activation Settings is blocked; OS remains on the IT-deployed edition.",
                ApplyOps = [RegOp.SetDword(Key, "PreventEditionDowngrade", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "PreventEditionDowngrade")],
                DetectOps = [RegOp.CheckDword(Key, "PreventEditionDowngrade", 1)],
            },
            new TweakDef
            {
                Id = "wanyu-hide-activation-settings",
                Label = "Hide Activation Settings Page",
                Category = "Windows Anytime Upgrade Policy",
                Description =
                    "Removes the Activation page from Windows Settings so users cannot view the activation status or attempt to change the product key. Useful on volume-licensed endpoints where individual activation management is not required and should not be user-accessible. Default: Activation page visible. Recommended: 1 on volume-licensed images.",
                Tags = ["anytime-upgrade", "activation", "settings", "ui", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Settings → Activation page is hidden; users cannot view licensing state or change the product key.",
                ApplyOps = [RegOp.SetDword(Key, "HideActivationPage", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "HideActivationPage")],
                DetectOps = [RegOp.CheckDword(Key, "HideActivationPage", 1)],
            },
            new TweakDef
            {
                Id = "wanyu-disable-phone-activation",
                Label = "Disable Phone Activation Method",
                Category = "Windows Anytime Upgrade Policy",
                Description =
                    "Blocks the automated phone activation pathway that allows a user to activate a new edition by calling a Microsoft number and entering a confirmation code. Prevents out-of-band edition changes that bypasses online controls. Default: phone activation available. Recommended: 1.",
                Tags = ["anytime-upgrade", "phone-activation", "edition", "activation", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Phone activation path for edition upgrades is disabled; only online IT-managed activation is available.",
                ApplyOps = [RegOp.SetDword(Key, "DisablePhoneActivation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePhoneActivation")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePhoneActivation", 1)],
            },
            new TweakDef
            {
                Id = "wanyu-lock-edition-to-deployed",
                Label = "Lock OS Edition to IT-Deployed Edition",
                Category = "Windows Anytime Upgrade Policy",
                Description =
                    "Configures a policy lock that prevents the OS edition from changing in either direction (upgrade or downgrade) without explicit Group Policy update. Provides a strong enforcement control on managed devices where edition stability is a compliance requirement. Default: not locked. Recommended: 1 on standardised fleet deployments.",
                Tags = ["anytime-upgrade", "edition", "lock", "compliance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "OS edition is locked to the IT-deployed value; neither upgrade nor downgrade is possible without GPO change.",
                ApplyOps = [RegOp.SetDword(Key, "LockEdition", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "LockEdition")],
                DetectOps = [RegOp.CheckDword(Key, "LockEdition", 1)],
            },
            new TweakDef
            {
                Id = "wanyu-disable-trial-edition-conversion",
                Label = "Disable Trial Edition Conversion",
                Category = "Windows Anytime Upgrade Policy",
                Description =
                    "Prevents the OS from being converted from a trial (evaluation) edition to a retail edition via key entry. Ensures evaluation images are not accidentally or deliberately activated as production machines without proper licensing procedures. Default: trial conversion available. Recommended: 1 on production fleet.",
                Tags = ["anytime-upgrade", "trial", "conversion", "edition", "activation", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Trial-to-retail edition conversion is blocked; evaluation images cannot be activated without proper IT process.",
                ApplyOps = [RegOp.SetDword(Key, "DisableTrialConversion", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableTrialConversion")],
                DetectOps = [RegOp.CheckDword(Key, "DisableTrialConversion", 1)],
            },
        ];
}
