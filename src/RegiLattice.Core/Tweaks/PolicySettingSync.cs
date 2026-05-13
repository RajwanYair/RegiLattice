namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

[TweakModule]
internal static class PolicySettingSync
{
    private const string SyncKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SettingSync";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "syncp-disable-browser-sync",
                Label = "Settings Sync: Disable Web Browser Settings Synchronisation",
                Category = "Privacy — Settings Sync",
                Description =
                    "Disables web browser favourites, history, and settings synchronisation via Microsoft account. "
                    + "DisableWebBrowserSettingSync=2 prevents browser syncing and prevents users from re-enabling it. "
                    + "Stops browser data from being uploaded to or downloaded from the cloud.",
                Tags = ["settings-sync", "privacy", "microsoft-account", "browser", "cloud"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Browser favourites, history, and settings no longer sync across devices.",
                ApplyOps = [RegOp.SetDword(SyncKey, "DisableWebBrowserSettingSync", 2)],
                RemoveOps = [RegOp.DeleteValue(SyncKey, "DisableWebBrowserSettingSync")],
                DetectOps = [RegOp.CheckDword(SyncKey, "DisableWebBrowserSettingSync", 2)],
            },
            new TweakDef
            {
                Id = "syncp-prevent-user-override",
                Label = "Settings Sync: Prevent User From Re-Enabling Sync",
                Category = "Privacy — Settings Sync",
                Description =
                    "Prevents users from overriding the administrator-set Settings Sync policy. "
                    + "When set, the sync toggle in Settings is greyed out and cannot be turned on by the user. "
                    + "Should be combined with syncp-disable-all-sync for full enforcement.",
                Tags = ["settings-sync", "privacy", "policy-enforcement"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Settings sync toggle locked for non-admin users.",
                ApplyOps = [RegOp.SetDword(SyncKey, "DisableSettingSyncDeviceOverride", 1)],
                RemoveOps = [RegOp.DeleteValue(SyncKey, "DisableSettingSyncDeviceOverride")],
                DetectOps = [RegOp.CheckDword(SyncKey, "DisableSettingSyncDeviceOverride", 1)],
            },
            new TweakDef
            {
                Id = "syncp-disable-credentials-sync",
                Label = "Settings Sync: Disable Password and Credentials Sync",
                Category = "Privacy — Settings Sync",
                Description =
                    "Prevents Windows from syncing saved passwords and credentials via Microsoft account. "
                    + "Credentials stored in the Credential Manager or typed into the Edge password manager "
                    + "will not be uploaded to the cloud or replicated to other devices.",
                Tags = ["settings-sync", "credentials", "password-sync", "privacy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Passwords and credentials kept local; no cloud backup or cross-device sharing of credentials.",
                ApplyOps = [RegOp.SetDword(SyncKey, "DisableCredentialsSettingSync", 2)],
                RemoveOps = [RegOp.DeleteValue(SyncKey, "DisableCredentialsSettingSync")],
                DetectOps = [RegOp.CheckDword(SyncKey, "DisableCredentialsSettingSync", 2)],
            },
            new TweakDef
            {
                Id = "syncp-disable-application-sync",
                Label = "Settings Sync: Disable Application Settings Sync",
                Category = "Privacy — Settings Sync",
                Description =
                    "Prevents Windows from syncing per-application settings across devices. "
                    + "App preferences such as language, layout, and configuration stored by UWP and legacy apps "
                    + "will remain local and not synced when the user signs in on another machine.",
                Tags = ["settings-sync", "app-settings", "privacy", "uwp"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Per-app preferences are not cloud-synced; may require re-configuration on new devices.",
                ApplyOps = [RegOp.SetDword(SyncKey, "DisableApplicationSettingSync", 2)],
                RemoveOps = [RegOp.DeleteValue(SyncKey, "DisableApplicationSettingSync")],
                DetectOps = [RegOp.CheckDword(SyncKey, "DisableApplicationSettingSync", 2)],
            },
            new TweakDef
            {
                Id = "syncp-disable-start-layout-sync",
                Label = "Settings Sync: Disable Start Menu and Taskbar Sync",
                Category = "Privacy — Settings Sync",
                Description =
                    "Prevents Windows from syncing the Start menu layout and taskbar pin configuration. "
                    + "Useful in environments where different machines have different roles and a per-machine "
                    + "Start layout is required rather than a roaming user layout.",
                Tags = ["settings-sync", "start-menu", "taskbar", "layout"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Start menu and taskbar layout stays local; new devices start with the default layout.",
                ApplyOps = [RegOp.SetDword(SyncKey, "DisableStartLayoutSettingSync", 2)],
                RemoveOps = [RegOp.DeleteValue(SyncKey, "DisableStartLayoutSettingSync")],
                DetectOps = [RegOp.CheckDword(SyncKey, "DisableStartLayoutSettingSync", 2)],
            },
            new TweakDef
            {
                Id = "syncp-disable-language-sync",
                Label = "Settings Sync: Disable Language Settings Sync",
                Category = "Privacy — Settings Sync",
                Description =
                    "Prevents Windows from syncing the installed language packs, display language, and locale settings. "
                    + "Ensures that a user who changes their language on one device does not inadvertently "
                    + "change the language on other devices they use (e.g., shared or kiosk machines).",
                Tags = ["settings-sync", "language", "locale", "privacy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Language and locale settings kept per-device; changes on one machine do not propagate.",
                ApplyOps = [RegOp.SetDword(SyncKey, "DisableLanguageSettingSync", 2)],
                RemoveOps = [RegOp.DeleteValue(SyncKey, "DisableLanguageSettingSync")],
                DetectOps = [RegOp.CheckDword(SyncKey, "DisableLanguageSettingSync", 2)],
            },
            new TweakDef
            {
                Id = "syncp-disable-personalization-sync",
                Label = "Settings Sync: Disable Personalization Settings Sync",
                Category = "Privacy — Settings Sync",
                Description =
                    "Disables syncing of personalization items including wallpaper, lock screen image, "
                    + "accent colour, and high contrast settings. "
                    + "Keeps the desktop appearance local and prevents personal images from being uploaded to the cloud.",
                Tags = ["settings-sync", "personalization", "wallpaper", "privacy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Desktop wallpaper and colour scheme syncing is disabled; images stay on the local device.",
                ApplyOps = [RegOp.SetDword(SyncKey, "DisablePersonalizationSettingSync", 2)],
                RemoveOps = [RegOp.DeleteValue(SyncKey, "DisablePersonalizationSettingSync")],
                DetectOps = [RegOp.CheckDword(SyncKey, "DisablePersonalizationSettingSync", 2)],
            },
            new TweakDef
            {
                Id = "syncp-disable-desktop-theme-sync",
                Label = "Settings Sync: Disable Desktop Theme Sync",
                Category = "Privacy — Settings Sync",
                Description =
                    "Prevents Windows desktop themes (colour, sounds, cursor, icons) from being synced "
                    + "across devices via Microsoft account. "
                    + "Complementary to syncp-disable-personalization-sync; specifically targets the theme bundle.",
                Tags = ["settings-sync", "theme", "desktop", "privacy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Desktop theme data (sounds, cursors, colours) stays local.",
                ApplyOps = [RegOp.SetDword(SyncKey, "DisableDesktopThemeSettingSync", 2)],
                RemoveOps = [RegOp.DeleteValue(SyncKey, "DisableDesktopThemeSettingSync")],
                DetectOps = [RegOp.CheckDword(SyncKey, "DisableDesktopThemeSettingSync", 2)],
            },
            new TweakDef
            {
                Id = "syncp-disable-accessibility-settings-sync",
                Label = "Settings Sync: Disable Accessibility Settings Sync",
                Category = "Privacy — Settings Sync",
                Description =
                    "Prevents Windows accessibility settings (Narrator, Magnifier, on-screen keyboard, high contrast, "
                    + "cursor size, text scale) from being synced across devices. "
                    + "Ensures per-device accessibility configuration is not overwritten by another device's profile.",
                Tags = ["settings-sync", "accessibility", "narrator", "privacy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Accessibility settings remain per-device; beneficial when different devices have different users.",
                ApplyOps = [RegOp.SetDword(SyncKey, "DisableAccessibilitySettingSync", 2)],
                RemoveOps = [RegOp.DeleteValue(SyncKey, "DisableAccessibilitySettingSync")],
                DetectOps = [RegOp.CheckDword(SyncKey, "DisableAccessibilitySettingSync", 2)],
            },
            new TweakDef
            {
                Id = "syncp-disable-on-paid-network",
                Label = "Settings Sync: Disable Sync on Metered / Paid Network",
                Category = "Privacy — Settings Sync",
                Description =
                    "Prevents Windows Settings Sync from operating over metered (paid) connections such as "
                    + "mobile hotspots, cellular data, or ISP-capped broadband. "
                    + "Sync only resumes when connected to an unmetered network.",
                Tags = ["settings-sync", "metered", "data-usage", "privacy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Sync paused on metered connections; prevents unexpected data charges on capped plans.",
                ApplyOps = [RegOp.SetDword(SyncKey, "DisableSyncOnPaidNetwork", 1)],
                RemoveOps = [RegOp.DeleteValue(SyncKey, "DisableSyncOnPaidNetwork")],
                DetectOps = [RegOp.CheckDword(SyncKey, "DisableSyncOnPaidNetwork", 1)],
            },
        ];
}
