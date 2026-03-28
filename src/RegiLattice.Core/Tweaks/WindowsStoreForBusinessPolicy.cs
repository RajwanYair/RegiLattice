// WindowsStoreForBusinessPolicy.cs — Windows Store for Business / private store enforcement
// Registry: HKLM\SOFTWARE\Policies\Microsoft\WindowsStore
// Slug: wsfb
// Category: Windows Store For Business Policy

namespace RegiLattice.Core.Tweaks;

using System.Collections.Generic;
using RegiLattice.Core.Models;

internal static class WindowsStoreForBusinessPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "wsfb-disable-store-apps",
            Label = "Windows Store For Business: Disable Store App Installation",
            Category = "Windows Store For Business Policy",
            Description =
                "Prevents users from installing applications from the public Microsoft Store. "
                + "On managed endpoints the public Store should be replaced by the private organizational store or an approved software catalog. "
                + "This policy blocks direct Store access while allowing Microsoft-published in-box apps to receive updates. "
                + "Removing this policy re-enables public Store app installation for all users.",
            Tags = ["store", "app-install", "enterprise", "lockdown", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableStoreApps", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableStoreApps")],
            DetectOps = [RegOp.CheckDword(Key, "DisableStoreApps", 1)],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Blocks public Store installs; reduces shadow-IT and unapproved software risk.",
        },
        new TweakDef
        {
            Id = "wsfb-require-private-store-only",
            Label = "Windows Store For Business: Require Private Store Only",
            Category = "Windows Store For Business Policy",
            Description =
                "Forces the Microsoft Store app to display only the organization's private store tab, hiding the public Store catalog entirely. "
                + "When set, employees see only IT-approved applications and cannot browse or purchase from the public Store. "
                + "This is the recommended configuration for organizations using Microsoft Store for Business or Education. "
                + "Removing this policy restores the public Store tab alongside the private store.",
            Tags = ["store", "private-store", "enterprise", "approved-apps", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RequirePrivateStoreOnly", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequirePrivateStoreOnly")],
            DetectOps = [RegOp.CheckDword(Key, "RequirePrivateStoreOnly", 1)],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Limits Store UI to approved corporate apps only; eliminates public Store browsing.",
        },
        new TweakDef
        {
            Id = "wsfb-disable-automatic-download-app-updates",
            Label = "Windows Store For Business: Disable Automatic App Update Downloads",
            Category = "Windows Store For Business Policy",
            Description =
                "Prevents the Microsoft Store from automatically downloading and installing updates for Store apps in the background. "
                + "Automatic background updates consume bandwidth and may apply changes to apps that IT has not yet validated. "
                + "With this policy set, Store app updates are only delivered through Intune, WSUS, or manually triggered update scans. "
                + "Removing this policy re-enables automatic background Store app update downloads.",
            Tags = ["store", "auto-update", "bandwidth", "enterprise", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AutoDownload", 2)],
            RemoveOps = [RegOp.DeleteValue(Key, "AutoDownload")],
            DetectOps = [RegOp.CheckDword(Key, "AutoDownload", 2)],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Stops background Store app update downloads; reduces unmanaged bandwidth consumption.",
        },
        new TweakDef
        {
            Id = "wsfb-disable-store-purchase",
            Label = "Windows Store For Business: Disable Store Purchases",
            Category = "Windows Store For Business Policy",
            Description =
                "Prevents users from making paid purchases through the Microsoft Store. "
                + "Without this policy, users can purchase apps, games, and media using personal or corporate payment methods. "
                + "On enterprise endpoints, paid Store purchases should be managed through volume licensing, not individual user transactions. "
                + "Removing this policy re-enables user-initiated Store purchases.",
            Tags = ["store", "purchase", "paid-apps", "enterprise", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableStoreApplications", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableStoreApplications")],
            DetectOps = [RegOp.CheckDword(Key, "DisableStoreApplications", 1)],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Blocks Store purchases; prevents unauthorized paid app acquisitions on corporate devices.",
        },
        new TweakDef
        {
            Id = "wsfb-block-non-enterprise-apps",
            Label = "Windows Store For Business: Block Non-Enterprise App Sideloading",
            Category = "Windows Store For Business Policy",
            Description =
                "Prevents installation of non-enterprise (consumer) MSIX/AppX packages via sideloading or developer mode. "
                + "Sideloading allows arbitrary package files to be deployed outside of Store or Intune validation. "
                + "On managed endpoints this creates a risk of malicious or unlicensed application deployment. "
                + "Removing this policy allows MSIX sideloading for testing or development purposes.",
            Tags = ["store", "sideloading", "appx", "enterprise", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AllowAllTrustedApps", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowAllTrustedApps")],
            DetectOps = [RegOp.CheckDword(Key, "AllowAllTrustedApps", 0)],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Blocks sideloaded MSIX/AppX packages; prevents unauthorized app deployment channels.",
        },
        new TweakDef
        {
            Id = "wsfb-disable-store-ui",
            Label = "Windows Store For Business: Disable Store UI Entry Points",
            Category = "Windows Store For Business Policy",
            Description =
                "Removes Store launch shortcuts and promoted links from the Start menu, taskbar, and Settings pages. "
                + "When this policy is applied, clicking app tiles that would normally redirect to the Store shows an error instead. "
                + "This prevents users from discovering and requesting unapproved applications through incidental Store exposure. "
                + "Removing this policy restores all Store UI entry points.",
            Tags = ["store", "ui", "start-menu", "enterprise", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RemoveWindowsStore", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RemoveWindowsStore")],
            DetectOps = [RegOp.CheckDword(Key, "RemoveWindowsStore", 1)],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Removes Store entry points from UI; reduces user exposure to consumer-oriented Store.",
        },
        new TweakDef
        {
            Id = "wsfb-disable-store-implicit-access",
            Label = "Windows Store For Business: Block Store Access for All Users",
            Category = "Windows Store For Business Policy",
            Description =
                "Applies a machine-wide policy blocking all standard (non-admin) user accounts from accessing the Store. "
                + "This complements user-scope Store restrictions by ensuring the policy is active for any user who logs onto the device. "
                + "Useful when Intune MDM enrollment has not yet applied or the GPO has been partially applied. "
                + "Removing this policy removes the machine-wide Store access block.",
            Tags = ["store", "machine-wide", "access-control", "enterprise", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableStoreAppsForAllUsers", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableStoreAppsForAllUsers")],
            DetectOps = [RegOp.CheckDword(Key, "DisableStoreAppsForAllUsers", 1)],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Machine-wide Store block; effective even before per-user policies are applied.",
        },
        new TweakDef
        {
            Id = "wsfb-disable-in-app-purchases",
            Label = "Windows Store For Business: Disable In-App Purchases",
            Category = "Windows Store For Business Policy",
            Description =
                "Prevents in-app purchase (IAP) transactions within Store applications. "
                + "Many free-to-download Store apps monetize via in-app purchases for premium content or subscriptions. "
                + "On corporate devices, in-app purchases can lead to unauthorized charges on corporate payment instruments. "
                + "Removing this policy re-enables in-app purchase capability within Store apps.",
            Tags = ["store", "in-app-purchase", "billing", "enterprise", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableInAppPurchases", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableInAppPurchases")],
            DetectOps = [RegOp.CheckDword(Key, "DisableInAppPurchases", 1)],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Blocks in-app purchases; prevents accidental billing on corporate payment methods.",
        },
        new TweakDef
        {
            Id = "wsfb-disable-gaming-store",
            Label = "Windows Store For Business: Disable Gaming (Xbox) Store Content",
            Category = "Windows Store For Business Policy",
            Description =
                "Hides gaming-related content and Xbox app promotions within the Microsoft Store UX. "
                + "On enterprise endpoints gaming content is irrelevant and can distract users from productivity applications. "
                + "This policy suppresses Xbox Live, Game Pass, and other consumer gaming categories from appearing in Store search and recommendations. "
                + "Removing this policy restores gaming store content visibility.",
            Tags = ["store", "gaming", "xbox", "enterprise", "distraction", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "HideGamingModeFromStore", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "HideGamingModeFromStore")],
            DetectOps = [RegOp.CheckDword(Key, "HideGamingModeFromStore", 1)],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Hides gaming/Xbox content from Store; reduces consumer content exposure on work devices.",
        },
        new TweakDef
        {
            Id = "wsfb-disable-store-pre-install-reqs",
            Label = "Windows Store For Business: Block App Pre-Install Promotions",
            Category = "Windows Store For Business Policy",
            Description =
                "Prevents Microsoft from pre-installing or promoting Store applications via Windows Update or OOBE flows. "
                + "Microsoft occasionally pre-installs promoted applications (such as Spotify, TikTok, or Candy Crush) onto managed devices via the Store pre-install mechanism. "
                + "On corporate SOEs this behavior introduces unapproved software and clutters the Start menu with consumer content. "
                + "Removing this policy allows Microsoft to pre-install promoted Store applications.",
            Tags = ["store", "pre-install", "bloatware", "enterprise", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableOSUpgrade", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableOSUpgrade")],
            DetectOps = [RegOp.CheckDword(Key, "DisableOSUpgrade", 1)],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Blocks Store-driven pre-installs and OOBE promotions; keeps SOE image clean.",
        },
    ];
}
