namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WindowsAccessibilityPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Accessibility";
    private const string MagnKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Accessibility\Magnifier";
    private const string NarratorKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Accessibility\Narrator";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "a11ypol-disable-serial-keys",
            Label = "Accessibility Policy: Disable Serial Keys Support",
            Category = "Windows Accessibility Policy",
            Description =
                "Disables Serial Keys accessibility support, which allows alternative input devices (joysticks, switches) connected to the serial port. Disabling reduces the attack surface on managed endpoints without physical accessibility hardware.",
            Tags = ["accessibility", "serial-keys", "input", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "Reduces attack surface on managed endpoints without accessibility hardware.",
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableSerialKeysSupport", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableSerialKeysSupport")],
            DetectOps = [RegOp.CheckDword(Key, "DisableSerialKeysSupport", 1)],
        },
        new TweakDef
        {
            Id = "a11ypol-disable-sound-sentry",
            Label = "Accessibility Policy: Disable SoundSentry Visual Flash",
            Category = "Windows Accessibility Policy",
            Description =
                "Disables SoundSentry, which flashes the screen or a window when a critical sound plays. On enterprise environments with active CAD/3D rendering, unexpected screen flashes can interfere with rendering workflows.",
            Tags = ["accessibility", "soundsentry", "flash", "audio", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "Prevents unexpected screen flashes interfering with CAD/3D rendering workflows.",
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableSoundSentryFunctionality", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableSoundSentryFunctionality")],
            DetectOps = [RegOp.CheckDword(Key, "DisableSoundSentryFunctionality", 1)],
        },
        new TweakDef
        {
            Id = "a11ypol-disable-high-contrast-hotkey",
            Label = "Accessibility Policy: Disable High Contrast Mode Hotkey",
            Category = "Windows Accessibility Policy",
            Description =
                "Prevents users from accidentally enabling High Contrast mode via the Left Alt+Left Shift+Print Screen keyboard shortcut. Avoids unexpected UI colour inversions that can disrupt productivity applications.",
            Tags = ["accessibility", "high-contrast", "hotkey", "keyboard", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Prevents accidental Alt+Shift+PrtScr activation of high contrast mode.",
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableHighContrastHotKey", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableHighContrastHotKey")],
            DetectOps = [RegOp.CheckDword(Key, "DisableHighContrastHotKey", 1)],
        },
        new TweakDef
        {
            Id = "a11ypol-disable-toggle-keys",
            Label = "Accessibility Policy: Disable Toggle Keys Hotkey",
            Category = "Windows Accessibility Policy",
            Description =
                "Disables activation of Toggle Keys (a beep when pressing Caps Lock, Num Lock, or Scroll Lock) via the Num Lock hotkey shortcut. Prevents unexpected beeping on endpoints with shared keyboards.",
            Tags = ["accessibility", "toggle-keys", "keyboard", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Prevents accidental beeping when Num Lock key is pressed on shared keyboards.",
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableToggleKeysHotKey", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableToggleKeysHotKey")],
            DetectOps = [RegOp.CheckDword(Key, "DisableToggleKeysHotKey", 1)],
        },
        new TweakDef
        {
            Id = "a11ypol-disable-sticky-keys-hotkey",
            Label = "Accessibility Policy: Disable Sticky Keys Hotkey",
            Category = "Windows Accessibility Policy",
            Description =
                "Disables the Sticky Keys prompt when Shift is pressed 5 times. Sticky Keys can interrupt gaming and productivity workflows when activated accidentally, and is better enabled via Settings if needed.",
            Tags = ["accessibility", "sticky-keys", "keyboard", "hotkey", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Prevents 5\u00d7Shift shortcut from interrupting gaming and productivity workflows.",
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableStickyKeysHotKey", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableStickyKeysHotKey")],
            DetectOps = [RegOp.CheckDword(Key, "DisableStickyKeysHotKey", 1)],
        },
        new TweakDef
        {
            Id = "a11ypol-disable-filter-keys-hotkey",
            Label = "Accessibility Policy: Disable Filter Keys Hotkey",
            Category = "Windows Accessibility Policy",
            Description =
                "Disables the Filter Keys shortcut activated by holding the Right Shift key for 8 seconds. Filter Keys can cause significant input delay if triggered accidentally.",
            Tags = ["accessibility", "filter-keys", "keyboard", "hotkey", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Prevents 8-second Shift hold from triggering keyboard input delay mode.",
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableFilterKeysHotKey", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableFilterKeysHotKey")],
            DetectOps = [RegOp.CheckDword(Key, "DisableFilterKeysHotKey", 1)],
        },
        new TweakDef
        {
            Id = "a11ypol-disable-bounce-keys",
            Label = "Accessibility Policy: Disable Bounce Keys for Keyboard Repeat",
            Category = "Windows Accessibility Policy",
            Description =
                "Disables Bounce Keys (Filter Keys variant) that ignores brief multiple key presses. While useful for accessibility, this setting can reduce keyboard responsiveness when not needed.",
            Tags = ["accessibility", "bounce-keys", "filter-keys", "keyboard", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "Restores normal keyboard repeat; disables brief multiple-keypress filtering.",
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableBounceKeyboardSettings", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableBounceKeyboardSettings")],
            DetectOps = [RegOp.CheckDword(Key, "DisableBounceKeyboardSettings", 1)],
        },
        new TweakDef
        {
            Id = "a11ypol-disable-mouse-keys-hotkey",
            Label = "Accessibility Policy: Disable Mouse Keys Hotkey",
            Category = "Windows Accessibility Policy",
            Description =
                "Disables activation of Mouse Keys via the Left Alt+Left Shift+Num Lock shortcut. Mouse Keys redirects numpad input to pointer movement, which is a common source of unexpected mouse behaviour on laptops.",
            Tags = ["accessibility", "mouse-keys", "hotkey", "numpad", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Prevents numpad-to-mouse redirect being accidentally activated on laptops.",
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableMouseKeysHotKey", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableMouseKeysHotKey")],
            DetectOps = [RegOp.CheckDword(Key, "DisableMouseKeysHotKey", 1)],
        },
        new TweakDef
        {
            Id = "a11ypol-disable-magnifier-startup",
            Label = "Accessibility Policy: Disable Magnifier Auto-Start",
            Category = "Windows Accessibility Policy",
            Description =
                "Prevents Windows Magnifier from starting automatically when a user signs in. Magnifier auto-start is sometimes triggered by a registry artefact on downgraded or re-imaged systems.",
            Tags = ["accessibility", "magnifier", "startup", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Prevents Magnifier from auto-starting on re-imaged systems.",
            RegistryKeys = [MagnKey],
            ApplyOps = [RegOp.SetDword(MagnKey, "StartMinimized", 1)],
            RemoveOps = [RegOp.DeleteValue(MagnKey, "StartMinimized")],
            DetectOps = [RegOp.CheckDword(MagnKey, "StartMinimized", 1)],
        },
        new TweakDef
        {
            Id = "a11ypol-disable-narrator-startup",
            Label = "Accessibility Policy: Disable Narrator Auto-Start on Sign-In",
            Category = "Windows Accessibility Policy",
            Description =
                "Prevents Windows Narrator (screen reader) from starting automatically at sign-in. Narrator auto-activation can be triggered by accessibility registry artefacts on shared or re-used endpoints.",
            Tags = ["accessibility", "narrator", "screen-reader", "startup", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Prevents Narrator screen reader from auto-activating on shared or re-used endpoints.",
            RegistryKeys = [NarratorKey],
            ApplyOps = [RegOp.SetDword(NarratorKey, "DisableNarratorAutoStart", 1)],
            RemoveOps = [RegOp.DeleteValue(NarratorKey, "DisableNarratorAutoStart")],
            DetectOps = [RegOp.CheckDword(NarratorKey, "DisableNarratorAutoStart", 1)],
        },
    ];
}
