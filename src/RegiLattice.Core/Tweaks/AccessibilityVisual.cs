// AccessibilityVisual.cs — visual accessibility tweaks for text, contrast, and display
// Category: Accessibility  |  IDs: accvisual-*  |  10 tweaks

#nullable enable
using RegiLattice.Core.Models;
using RegiLattice.Core.Registry;

namespace RegiLattice.Core.Tweaks;

internal static class AccessibilityVisual
{
    private const string DesktopKey = @"HKEY_CURRENT_USER\Control Panel\Desktop";
    private const string AccessKey = @"HKEY_CURRENT_USER\Control Panel\Accessibility";
    private const string DwmKey = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM";
    private const string NtKey = @"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\SystemRestore";
    private const string MetricsKey = @"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics";
    private const string FocusAssist =
        @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CloudStore\Store\DefaultAccount\Current\default$windows.data.notifications.quiethourssettings\windows.data.notifications.quiethourssettings";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "accvisual-caret-blink-off",
            Label = "Disable Cursor Caret Blinking",
            Category = "Accessibility",
            Description =
                "Stops the text insertion caret from blinking (sets blink period to 0), helping users with photosensitivity or attention disorders by removing distracting movement.",
            Tags = ["accessibility", "visual", "caret", "blink", "focus"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Eliminates caret blink distractions for users with photosensitivity or ADHD.",
            ApplyOps = [RegOp.SetDword(DesktopKey, "CursorBlinkRate", -1)],
            RemoveOps = [RegOp.SetDword(DesktopKey, "CursorBlinkRate", 530)],
            DetectOps = [RegOp.CheckDword(DesktopKey, "CursorBlinkRate", -1)],
        },
        new TweakDef
        {
            Id = "accvisual-caret-width-3",
            Label = "Set Caret Width to 3px for Better Visibility",
            Category = "Accessibility",
            Description =
                "Increases the cursor caret width from the default 1px to 3px, making it easier to locate in text for users with low vision.",
            Tags = ["accessibility", "visual", "caret", "width", "low-vision"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Wider caret is easier to locate for users with low vision or macular degeneration.",
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "CaretWidth", 3)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "CaretWidth", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "CaretWidth", 3)],
        },
        new TweakDef
        {
            Id = "accvisual-font-smoothing-cleartype",
            Label = "Enable ClearType Font Smoothing",
            Category = "Accessibility",
            Description = "Enables ClearType font rendering, significantly improving text readability on LCD monitors for users with low vision.",
            Tags = ["accessibility", "visual", "font", "cleartype", "smoothing", "low-vision"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Sharper, cleaner text on LCD screens; reduces eyestrain for extended reading sessions.",
            ApplyOps = [RegOp.SetDword(DesktopKey, "FontSmoothing", 2), RegOp.SetDword(DesktopKey, "FontSmoothingType", 2)],
            RemoveOps = [RegOp.SetDword(DesktopKey, "FontSmoothing", 0), RegOp.SetDword(DesktopKey, "FontSmoothingType", 0)],
            DetectOps = [RegOp.CheckDword(DesktopKey, "FontSmoothing", 2)],
        },
        new TweakDef
        {
            Id = "accvisual-focus-border-wider",
            Label = "Increase Focus Rectangle Border Width",
            Category = "Accessibility",
            Description =
                "Makes the dotted focus rectangle around UI elements thicker (2px) so keyboard-only users can clearly see which element has focus.",
            Tags = ["accessibility", "visual", "focus", "border", "keyboard-navigation"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "More visible focus indicator for keyboard navigation users.",
            ApplyOps = [RegOp.SetDword(MetricsKey, "FocusBorderWidth", 2), RegOp.SetDword(MetricsKey, "FocusBorderHeight", 2)],
            RemoveOps = [RegOp.SetDword(MetricsKey, "FocusBorderWidth", 1), RegOp.SetDword(MetricsKey, "FocusBorderHeight", 1)],
            DetectOps = [RegOp.CheckDword(MetricsKey, "FocusBorderWidth", 2)],
        },
        new TweakDef
        {
            Id = "accvisual-animations-off",
            Label = "Disable All Visual Animations",
            Category = "Accessibility",
            Description =
                "Disables window animation effects (fade, slide, etc.) for users with vestibular disorders or motion sensitivity who may experience discomfort from animated UI transitions.",
            Tags = ["accessibility", "visual", "animations", "motion-sensitivity", "vestibular"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "Eliminates all motion-based UI — essential for users with vestibular or motion-sensitivity disorders.",
            ApplyOps = [RegOp.SetDword(DesktopKey, "UserPreferencesMask", unchecked((int)0x92012020u))],
            RemoveOps = [RegOp.DeleteValue(DesktopKey, "UserPreferencesMask")],
            DetectOps = [RegOp.CheckDword(DesktopKey, "UserPreferencesMask", unchecked((int)0x92012020u))],
        },
        new TweakDef
        {
            Id = "accvisual-high-dpi-scaling-gdi",
            Label = "Enable GDI DPI Scaling for Legacy Apps",
            Category = "Accessibility",
            Description =
                "Enables GDI DPI scaling so legacy desktop applications are scaled at the system level rather than appearing tiny on high-DPI monitors.",
            Tags = ["accessibility", "visual", "dpi", "scaling", "high-dpi", "legacy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Makes legacy apps readable on 4K/2K displays; particularly useful for low-vision users.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\SideBySide", "PreferExternalManifest", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\SideBySide", "PreferExternalManifest", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\SideBySide", "PreferExternalManifest", 1)],
        },
        new TweakDef
        {
            Id = "accvisual-cursor-size-large",
            Label = "Increase System Cursor Size",
            Category = "Accessibility",
            Description =
                "Enlarges the Windows mouse cursor to size 3 (from 1) for better visibility, especially useful for low-vision users or users on large high-DPI screens.",
            Tags = ["accessibility", "visual", "cursor", "size", "low-vision"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Larger cursor is significantly easier to locate for low-vision or elderly users.",
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Accessibility", "CursorSize", 3)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Accessibility", "CursorSize", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Accessibility", "CursorSize", 3)],
        },
        new TweakDef
        {
            Id = "accvisual-text-scale-125",
            Label = "Set Text Scale Factor to 125%",
            Category = "Accessibility",
            Description =
                "Raises the system text size to 125% without changing the full display DPI. This enlarges text in all apps that respect the text size setting.",
            Tags = ["accessibility", "visual", "text", "scale", "font-size", "low-vision"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Increases readability across all system text without full display zoom.",
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Accessibility", "TextScaleFactor", 125)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Accessibility", "TextScaleFactor", 100)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Accessibility", "TextScaleFactor", 125)],
        },
        new TweakDef
        {
            Id = "accvisual-notification-duration",
            Label = "Increase Notification Display Duration to 30s",
            Category = "Accessibility",
            Description =
                "Extends pop-up notification display time to 30 seconds so users who read or process information slowly have more time to respond before notifications disappear.",
            Tags = ["accessibility", "visual", "notification", "duration", "cognitive"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Prevents notifications disappearing before users with slower reading speed can act on them.",
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Accessibility", "MessageDuration", 30)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Accessibility", "MessageDuration", 5)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Control Panel\Accessibility", "MessageDuration", 30)],
        },
        new TweakDef
        {
            Id = "accvisual-show-always-scrollbar",
            Label = "Show Scrollbars Always in UWP Apps",
            Category = "Accessibility",
            Description =
                "Forces XAML / UWP apps to always show scrollbars rather than hiding them, improving discoverability and usability for users with cognitive or visual impairments.",
            Tags = ["accessibility", "visual", "scrollbar", "uwp", "cognitive"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Persistent scrollbars help users understand page length and navigate without hover-state discovery.",
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Accessibility", "DynamicScrollbars", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Accessibility", "DynamicScrollbars", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Control Panel\Accessibility", "DynamicScrollbars", 0)],
        },
    ];
}
