// RegiLattice.Core — Tweaks/SettingSyncAdv.cs
// Granular SettingSync group-policy controls plus input personalization (Sprint 136).
// Slug "ssync" — HKLM SettingSync and HKCU/HKLM InputPersonalization paths.
// Complements MicrosoftAccount.cs which covers the master sync toggle and
// four specific categories (theme, credentials, apps, password). This module adds
// seven more granular SettingSync values plus input/handwriting personalization controls.

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class SettingSyncAdv
{
    private const string SyncPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SettingSync";
    private const string InputPers = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\InputPersonalization";
    private const string InputPersPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\InputPersonalization";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "ssync-disable-desktop-theme",
            Label = "Disable Desktop Background Theme Sync",
            Category = "Settings Sync",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Description =
                "Stops the desktop background and visual theme from being synced across "
                + "devices linked to the same Microsoft Account. "
                + "DisableDesktopThemeSettingSync=1.",
            Tags = ["sync", "theme", "desktop", "wallpaper", "msa"],
            RegistryKeys = [SyncPolicy],
            ApplyOps = [RegOp.SetDword(SyncPolicy, "DisableDesktopThemeSettingSync", 1)],
            RemoveOps = [RegOp.DeleteValue(SyncPolicy, "DisableDesktopThemeSettingSync")],
            DetectOps = [RegOp.CheckDword(SyncPolicy, "DisableDesktopThemeSettingSync", 1)],
        },
        new TweakDef
        {
            Id = "ssync-disable-start-layout",
            Label = "Disable Start Menu Layout Sync",
            Category = "Settings Sync",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Description =
                "Prevents the Start menu layout (pinned apps, tile arrangement) from being "
                + "synchronized across devices. DisableStartLayoutSettingSync=1.",
            Tags = ["sync", "start menu", "layout", "msa"],
            RegistryKeys = [SyncPolicy],
            ApplyOps = [RegOp.SetDword(SyncPolicy, "DisableStartLayoutSettingSync", 1)],
            RemoveOps = [RegOp.DeleteValue(SyncPolicy, "DisableStartLayoutSettingSync")],
            DetectOps = [RegOp.CheckDword(SyncPolicy, "DisableStartLayoutSettingSync", 1)],
        },
        new TweakDef
        {
            Id = "ssync-disable-browser-settings",
            Label = "Disable Browser Settings Sync",
            Category = "Settings Sync",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Prevents browser-related settings (favourites, history, home page settings) "
                + "from syncing through Microsoft Account. DisableBrowserSettingSync=1.",
            Tags = ["sync", "browser", "favourites", "msa", "privacy"],
            RegistryKeys = [SyncPolicy],
            ApplyOps = [RegOp.SetDword(SyncPolicy, "DisableBrowserSettingSync", 1)],
            RemoveOps = [RegOp.DeleteValue(SyncPolicy, "DisableBrowserSettingSync")],
            DetectOps = [RegOp.CheckDword(SyncPolicy, "DisableBrowserSettingSync", 1)],
        },
        new TweakDef
        {
            Id = "ssync-disable-language-settings",
            Label = "Disable Language and Regional Settings Sync",
            Category = "Settings Sync",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Description =
                "Prevents language packs, keyboard layouts, and regional settings from "
                + "being synchronized across devices. DisableLanguageSettingSync=1.",
            Tags = ["sync", "language", "regional", "keyboard", "msa"],
            RegistryKeys = [SyncPolicy],
            ApplyOps = [RegOp.SetDword(SyncPolicy, "DisableLanguageSettingSync", 1)],
            RemoveOps = [RegOp.DeleteValue(SyncPolicy, "DisableLanguageSettingSync")],
            DetectOps = [RegOp.CheckDword(SyncPolicy, "DisableLanguageSettingSync", 1)],
        },
        new TweakDef
        {
            Id = "ssync-disable-accessibility-settings",
            Label = "Disable Accessibility/Ease-of-Access Settings Sync",
            Category = "Settings Sync",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Description =
                "Stops Ease of Access settings (Magnifier, Narrator, contrast themes) "
                + "from syncing via Microsoft Account. DisableAccessibilitySettingSync=1.",
            Tags = ["sync", "accessibility", "ease of access", "narrator", "msa"],
            RegistryKeys = [SyncPolicy],
            ApplyOps = [RegOp.SetDword(SyncPolicy, "DisableAccessibilitySettingSync", 1)],
            RemoveOps = [RegOp.DeleteValue(SyncPolicy, "DisableAccessibilitySettingSync")],
            DetectOps = [RegOp.CheckDword(SyncPolicy, "DisableAccessibilitySettingSync", 1)],
        },
        new TweakDef
        {
            Id = "ssync-disable-personalization-settings",
            Label = "Disable Personalization Settings Sync",
            Category = "Settings Sync",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Description =
                "Prevents personalization settings such as colors, lock-screen images, and "
                + "accent colors from being synchronized. DisablePersonalizationSettingSync=1.",
            Tags = ["sync", "personalization", "lock screen", "colors", "msa"],
            RegistryKeys = [SyncPolicy],
            ApplyOps = [RegOp.SetDword(SyncPolicy, "DisablePersonalizationSettingSync", 1)],
            RemoveOps = [RegOp.DeleteValue(SyncPolicy, "DisablePersonalizationSettingSync")],
            DetectOps = [RegOp.CheckDword(SyncPolicy, "DisablePersonalizationSettingSync", 1)],
        },
        new TweakDef
        {
            Id = "ssync-disable-windows-settings",
            Label = "Disable General Windows Platform Settings Sync",
            Category = "Settings Sync",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Disables synchronization of general Windows OS settings (taskbar, search, "
                + "notification preferences) across devices. DisableWindowsSettingSync=1.",
            Tags = ["sync", "windows settings", "taskbar", "msa"],
            RegistryKeys = [SyncPolicy],
            ApplyOps = [RegOp.SetDword(SyncPolicy, "DisableWindowsSettingSync", 1)],
            RemoveOps = [RegOp.DeleteValue(SyncPolicy, "DisableWindowsSettingSync")],
            DetectOps = [RegOp.CheckDword(SyncPolicy, "DisableWindowsSettingSync", 1)],
        },
        new TweakDef
        {
            Id = "ssync-disable-text-personalization",
            Label = "Disable Typing / Text Input Personalization",
            Category = "Settings Sync",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Prevents Windows from collecting your typing patterns for autocorrect and "
                + "next-word predictions. RestrictImplicitTextCollection=1 in InputPersonalization.",
            Tags = ["personalization", "typing", "autocorrect", "privacy", "input"],
            RegistryKeys = [InputPers],
            ApplyOps = [RegOp.SetDword(InputPers, "RestrictImplicitTextCollection", 1)],
            RemoveOps = [RegOp.SetDword(InputPers, "RestrictImplicitTextCollection", 0)],
            DetectOps = [RegOp.CheckDword(InputPers, "RestrictImplicitTextCollection", 1)],
        },
        new TweakDef
        {
            Id = "ssync-disable-ink-personalization",
            Label = "Disable Handwriting / Ink Personalization",
            Category = "Settings Sync",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Stops Windows from collecting handwriting samples to improve ink recognition "
                + "accuracy. RestrictImplicitInkCollection=1 in InputPersonalization.",
            Tags = ["personalization", "handwriting", "ink", "stylus", "privacy"],
            RegistryKeys = [InputPers],
            ApplyOps = [RegOp.SetDword(InputPers, "RestrictImplicitInkCollection", 1)],
            RemoveOps = [RegOp.SetDword(InputPers, "RestrictImplicitInkCollection", 0)],
            DetectOps = [RegOp.CheckDword(InputPers, "RestrictImplicitInkCollection", 1)],
        },
        new TweakDef
        {
            Id = "ssync-disable-input-personalization-policy",
            Label = "Disable Input Personalization — Machine Policy",
            Category = "Settings Sync",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Machine-wide group policy that disables all Windows input personalization "
                + "(typing, handwriting, speech learning) for all users on the device. "
                + "AllowInputPersonalization=0.",
            Tags = ["personalization", "policy", "input", "speech", "privacy"],
            RegistryKeys = [InputPersPolicy],
            ApplyOps = [RegOp.SetDword(InputPersPolicy, "AllowInputPersonalization", 0)],
            RemoveOps = [RegOp.DeleteValue(InputPersPolicy, "AllowInputPersonalization")],
            DetectOps = [RegOp.CheckDword(InputPersPolicy, "AllowInputPersonalization", 0)],
        },
    ];
}
