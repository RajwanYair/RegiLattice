namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

[TweakModule]
internal static class Input
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "input-disable-touchpad-tap",
            Label = "Disable Touchpad Tap-to-Click (Perf)",
            Category = "Peripherals — Virtual Disk Service",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables tap-to-click on precision touchpads to reduce accidental clicks and improve input accuracy. Default: Enabled. Recommended: Disabled for desktop users.",
            Tags = ["input", "touchpad", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "TapEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "TapEnabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "TapEnabled", 0)],
        },
        new TweakDef
        {
            Id = "input-filter-keys",
            Label = "Disable Filter Keys",
            Category = "Peripherals — Virtual Disk Service",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Filter Keys accessibility shortcut that can interfere with gaming.",
            Tags = ["input", "accessibility", "gaming"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Accessibility\FilterKeys"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\FilterKeys", "Flags", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\FilterKeys", "Flags", "126")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\FilterKeys", "Flags", "0")],
        },
        new TweakDef
        {
            Id = "input-mouse-scroll-lines",
            Label = "Set Mouse Scroll to 5 Lines",
            Category = "Peripherals — Virtual Disk Service",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets mouse wheel scroll amount to 5 lines (default 3).",
            Tags = ["input", "mouse", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WheelScrollLines", "5")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WheelScrollLines", "3")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WheelScrollLines", "5")],
        },
        new TweakDef
        {
            Id = "input-disable-spell-check",
            Label = "Disable Spell Checking",
            Category = "Peripherals — Virtual Disk Service",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables automatic spell checking in Windows text input. Reduces CPU usage from background spell-check processing. Default: Enabled. Recommended: Disabled for developers.",
            Tags = ["input", "spell-check", "typing", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7", "EnableSpellchecking", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7", "EnableSpellchecking", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7", "EnableSpellchecking", 0)],
        },
        new TweakDef
        {
            Id = "input-disable-text-suggestions",
            Label = "Disable Text Suggestions",
            Category = "Peripherals — Virtual Disk Service",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables text prediction and suggestions while typing. Prevents the suggestion bar from appearing above the keyboard. Default: Enabled. Recommended: Disabled for power users.",
            Tags = ["input", "text-prediction", "suggestions", "typing"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7", "EnableTextPrediction", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7", "EnableTextPrediction", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7", "EnableTextPrediction", 0)],
        },
        new TweakDef
        {
            Id = "input-set-cursor-blink-rate",
            Label = "Set Fast Cursor Blink Rate",
            Category = "Peripherals — Virtual Disk Service",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets the cursor blink rate to 400 ms (faster than default). Makes the text cursor more visible and responsive. Default: 530 ms. Recommended: 400 ms for faster feedback.",
            Tags = ["input", "cursor", "blink-rate", "typing", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "CursorBlinkRate", "400")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "CursorBlinkRate", "530")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "CursorBlinkRate", "400")],
        },
        new TweakDef
        {
            Id = "input-increase-double-click-speed",
            Label = "Increase Double-Click Speed",
            Category = "Peripherals — Virtual Disk Service",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Reduces the double-click detection interval to 200 ms for faster response. Requires quicker double-clicks but feels responsive. Default: 500 ms. Recommended: 200 ms for power users.",
            Tags = ["input", "mouse", "double-click", "speed", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Mouse"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "DoubleClickSpeed", "200")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "DoubleClickSpeed", "500")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "DoubleClickSpeed", "200")],
        },
        new TweakDef
        {
            Id = "input-disable-mouse-accel",
            Label = "Disable Mouse Acceleration",
            Category = "Peripherals — Virtual Disk Service",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables mouse acceleration (enhanced pointer precision). Provides 1:1 mouse movement for gaming and precision work. Default: acceleration enabled.",
            Tags = ["input", "mouse", "acceleration", "gaming"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Mouse"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseSpeed", "0"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseThreshold1", "0"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseThreshold2", "0"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseSpeed", "1"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseThreshold1", "6"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseThreshold2", "10"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseSpeed", "0")],
        },
        // ── Sprint 19 additions ────────────────────────────────────────────
        new TweakDef
        {
            Id = "input-disable-pen-workspace",
            Label = "Disable Pen Workspace",
            Category = "Peripherals — Virtual Disk Service",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Pen Workspace (Screen Sketch, Sticky Notes shortcut). Useful on non-pen devices. Default: enabled.",
            Tags = ["input", "pen", "workspace", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace", "AllowSuggestedAppsInWindowsInkWorkspace", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace", "AllowSuggestedAppsInWindowsInkWorkspace"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace", "AllowSuggestedAppsInWindowsInkWorkspace", 0),
            ],
        },
        new TweakDef
        {
            Id = "input-disable-handwriting-panel",
            Label = "Disable Handwriting Panel",
            Category = "Peripherals — Virtual Disk Service",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the handwriting input panel from appearing on touch/pen input. Default: enabled on pen devices.",
            Tags = ["input", "handwriting", "panel", "disable"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7", "TipbandDesiredVisibility", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7", "TipbandDesiredVisibility")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7", "TipbandDesiredVisibility", 0)],
        },
        new TweakDef
        {
            Id = "input-set-mouse-hover-width",
            Label = "Increase Mouse Hover Detection Area",
            Category = "Peripherals — Virtual Disk Service",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Widens the mouse hover detection rectangle from 4 to 10 pixels. Reduces accidental tooltip triggering. Default: 4.",
            Tags = ["input", "mouse", "hover", "detection"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Mouse"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseHoverWidth", "10")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseHoverWidth", "4")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseHoverWidth", "10")],
        },
        new TweakDef
        {
            Id = "input-disable-writing-insights",
            Label = "Disable Writing Insights",
            Category = "Peripherals — Virtual Disk Service",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Windows Writing Insights that analyse text input for suggestions. Improves privacy. Default: enabled.",
            Tags = ["input", "writing", "insights", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Input\Settings"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Input\Settings", "InsightsEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Input\Settings", "InsightsEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Input\Settings", "InsightsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "input-disable-mouse-sonar",
            Label = "Disable Mouse Sonar (Show Pointer on Ctrl Press)",
            Category = "Peripherals — Virtual Disk Service",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets MouseSonar=0 in Control Panel\\Mouse. Disables the feature that briefly shows animated rings around the mouse pointer when Ctrl is pressed to locate it.",
            Tags = ["input", "mouse", "sonar", "animation"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Mouse"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseSonar", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseSonar", "1")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseSonar", "0")],
        },
        new TweakDef
        {
            Id = "input-disable-mouse-vanish",
            Label = "Disable Mouse Vanish (Hide Pointer While Typing)",
            Category = "Peripherals — Virtual Disk Service",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets MouseVanish=0 in Control Panel\\Mouse. Stops Windows from hiding the mouse pointer whenever the user is typing. Useful when working across multiple monitors.",
            Tags = ["input", "mouse", "vanish", "typing"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Mouse"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseVanish", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseVanish", "1")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseVanish", "0")],
        },
        new TweakDef
        {
            Id = "input-set-caret-width-2",
            Label = "Set Text Cursor (Caret) Width to 2 px",
            Category = "Peripherals — Virtual Disk Service",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets CaretWidth=2 in Desktop. Widens the blinking text insertion caret from the default 1 px to 2 px, making it easier to spot in dense code or document editors.",
            Tags = ["input", "caret", "cursor", "accessibility"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "CaretWidth", 2)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "CaretWidth", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "CaretWidth", 2)],
        },
        new TweakDef
        {
            Id = "input-disable-lang-switch-hotkey",
            Label = "Disable Keyboard Language Switch Hotkey (Alt+Shift)",
            Category = "Peripherals — Virtual Disk Service",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets ToggleHotkey=3 in Keyboard Layout\\Toggle. Disables the hotkey that switches between input languages (default: Left Alt+Shift or Ctrl+Shift), preventing accidental layout changes mid-sentence.",
            Tags = ["input", "keyboard", "language", "hotkey"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Keyboard Layout\Toggle"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Keyboard Layout\Toggle", "ToggleHotkey", "3")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Keyboard Layout\Toggle", "ToggleHotkey", "1")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Keyboard Layout\Toggle", "ToggleHotkey", "3")],
        },
        new TweakDef
        {
            Id = "input-disable-touchpad-two-finger-tap",
            Label = "Disable Precision Touchpad Two-Finger Tap (Right-Click)",
            Category = "Peripherals — Virtual Disk Service",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets TwoFingerTapEnabled=0 in PrecisionTouchPad. Disables the two-finger tap gesture that normally triggers a right-click, useful when two-finger scrolling also fires unintended context menus.",
            Tags = ["input", "touchpad", "gesture", "tap"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "TwoFingerTapEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "TwoFingerTapEnabled", 1)],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "TwoFingerTapEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "input-disable-touchpad-edge-swipe",
            Label = "Disable Precision Touchpad Edge Swipe Gestures",
            Category = "Peripherals — Virtual Disk Service",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets EdgeSwipesEnabled=0 in PrecisionTouchPad. Disables swipe-from-edge gestures (open Action Center, Task View, etc.) that can fire accidentally near the trackpad border.",
            Tags = ["input", "touchpad", "edge", "swipe"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "EdgeSwipesEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "EdgeSwipesEnabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "EdgeSwipesEnabled", 0)],
        },
        new TweakDef
        {
            Id = "input-disable-touchpad-three-finger-tap",
            Label = "Disable Precision Touchpad Three-Finger Tap",
            Category = "Peripherals — Virtual Disk Service",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets ThreeFingerTapEnabled=0 in PrecisionTouchPad. Prevents the three-finger tap from opening Cortana/Search, a common source of unintended gesture activations during fast typing.",
            Tags = ["input", "touchpad", "gesture", "three-finger"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "ThreeFingerTapEnabled", 0)],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "ThreeFingerTapEnabled", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "ThreeFingerTapEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "input-disable-touchpad-four-finger-tap",
            Label = "Disable Precision Touchpad Four-Finger Tap",
            Category = "Peripherals — Virtual Disk Service",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets FourFingerTapEnabled=0 in PrecisionTouchPad. Disables the four-finger tap gesture that opens the Action Center, preventing accidental panel opens.",
            Tags = ["input", "touchpad", "gesture", "four-finger"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "FourFingerTapEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "FourFingerTapEnabled", 1)],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "FourFingerTapEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "input-disable-touchpad-right-zone",
            Label = "Disable Precision Touchpad Right-Click Corner Zone",
            Category = "Peripherals — Virtual Disk Service",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets RightClickZoneEnabled=0 in PrecisionTouchPad. Removes the dedicated bottom-right corner tap zone that fires a right-click, helping users who accidentally trigger it.",
            Tags = ["input", "touchpad", "right-click", "zone"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "RightClickZoneEnabled", 0)],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "RightClickZoneEnabled", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "RightClickZoneEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "input-disable-touchpad-swipe-nav",
            Label = "Disable Precision Touchpad Swipe Navigation (Back/Forward)",
            Category = "Peripherals — Virtual Disk Service",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets SwipeNavigationEnabled=0 in PrecisionTouchPad. Disables three-finger left/right swipe gestures that navigate back and forward in browsers and File Explorer.",
            Tags = ["input", "touchpad", "swipe", "navigation"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "SwipeNavigationEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "SwipeNavigationEnabled", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "SwipeNavigationEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "input-disable-touchpad-pinch-zoom",
            Label = "Disable Precision Touchpad Pinch-to-Zoom",
            Category = "Peripherals — Virtual Disk Service",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets PinchZoomEnabled=0 in PrecisionTouchPad. Disables the two-finger pinch gesture that zooms in/out in supported apps and the browser, preventing accidental zoom changes.",
            Tags = ["input", "touchpad", "pinch", "zoom"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "PinchZoomEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "PinchZoomEnabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "PinchZoomEnabled", 0)],
        },
    ];
}
