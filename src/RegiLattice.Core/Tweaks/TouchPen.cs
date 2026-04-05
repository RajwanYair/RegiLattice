namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class TouchPen
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "touch-pen-screenshot",
            Label = "Pen Double-Click: Screen Sketch",
            Category = "Peripherals — Virtual Disk Service",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Maps pen button double-click to Screen Sketch (screenshot annotation). Default: Nothing.",
            Tags = ["touch", "pen", "screenshot", "shortcut"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Pen"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Pen", "DoubleClickAction", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Pen", "DoubleClickAction", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Pen", "DoubleClickAction", 4)],
        },
        new TweakDef
        {
            Id = "touch-pen-longpress",
            Label = "Pen Long-Press: Ink Workspace",
            Category = "Peripherals — Virtual Disk Service",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Maps pen button long-press to open the Ink Workspace. Default: Nothing.",
            Tags = ["touch", "pen", "workspace", "shortcut"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Pen"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Pen", "LongPressAction", 3)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Pen", "LongPressAction", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Pen", "LongPressAction", 3)],
        },
        new TweakDef
        {
            Id = "touch-disable-edge-swipe",
            Label = "Disable Edge Swipe Gesture (Policy)",
            Category = "Peripherals — Virtual Disk Service",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the screen-edge swipe gesture that opens Action Centre / notification pane. Prevents accidental triggers.",
            Tags = ["touch", "edge", "swipe", "gesture", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EdgeUI"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EdgeUI", "DisableSwipe", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EdgeUI", "DisableSwipe")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EdgeUI", "DisableSwipe", 1)],
        },
        new TweakDef
        {
            Id = "touch-disable-flicks",
            Label = "Disable Pen Flick Gestures",
            Category = "Peripherals — Virtual Disk Service",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables pen flick gestures (quick strokes for scroll, back, forward). Prevents accidental navigation.",
            Tags = ["touch", "pen", "flicks", "gestures"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Wisp\Pen\SysEventParameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Wisp\Pen\SysEventParameters", "FlicksEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Wisp\Pen\SysEventParameters", "FlicksEnabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Wisp\Pen\SysEventParameters", "FlicksEnabled", 0)],
        },
        new TweakDef
        {
            Id = "touch-reverse-scroll",
            Label = "Enable Reverse (Natural) Scrolling",
            Category = "Peripherals — Virtual Disk Service",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Reverses touchpad scroll direction so content moves in the same direction as your fingers (natural/Mac-style scrolling). Default: Traditional.",
            Tags = ["touch", "touchpad", "scroll", "direction", "natural"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "ScrollDirection", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "ScrollDirection", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "ScrollDirection", 0)],
        },
        new TweakDef
        {
            Id = "touch-sensitivity-high",
            Label = "Set Touchpad Sensitivity to High",
            Category = "Peripherals — Virtual Disk Service",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets precision touchpad sensitivity to maximum (AAPThreshold=0). Registers even the lightest touch. Default: Medium (2). Recommended: High for light-touch users.",
            Tags = ["touch", "touchpad", "sensitivity", "threshold"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "AAPThreshold", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "AAPThreshold", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "AAPThreshold", 0)],
        },
        new TweakDef
        {
            Id = "touch-disable-palm-rejection",
            Label = "Disable Touchpad Palm Rejection",
            Category = "Peripherals — Virtual Disk Service",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables touchpad palm rejection and cursor-leave detection. Useful when palm rejection causes missed inputs. Default: Enabled.",
            Tags = ["touch", "touchpad", "palm", "rejection"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "LeaveOnEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "LeaveOnEnabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "LeaveOnEnabled", 0)],
        },
        new TweakDef
        {
            Id = "touch-disable-touch-feedback",
            Label = "Disable Touch Visual Feedback",
            Category = "Peripherals — Virtual Disk Service",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the visual ripple effect when touching the screen. Default: enabled.",
            Tags = ["touch", "feedback", "visual", "disable"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Cursors"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Cursors", "ContactVisualization", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Cursors", "ContactVisualization", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Control Panel\Cursors", "ContactVisualization", 0)],
        },
        new TweakDef
        {
            Id = "touch-disable-gesture-feedback",
            Label = "Disable Touch Gesture Visual Feedback",
            Category = "Peripherals — Virtual Disk Service",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the visual feedback for multi-finger gestures (pinch, swipe). Default: enabled.",
            Tags = ["touch", "gesture", "feedback", "visual"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Cursors"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Cursors", "GestureVisualization", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Cursors", "GestureVisualization", 31)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Control Panel\Cursors", "GestureVisualization", 0)],
        },
        new TweakDef
        {
            Id = "touch-disable-touch-screen",
            Label = "Disable Touch Screen Input (HID)",
            Category = "Peripherals — Virtual Disk Service",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the HID-compliant touch screen. Touch will not work until re-enabled. Default: enabled.",
            Tags = ["touch", "screen", "hid", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TabletPC"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TabletPC", "TurnOffTouchInput", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TabletPC", "TurnOffTouchInput")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TabletPC", "TurnOffTouchInput", 1)],
        },
        new TweakDef
        {
            Id = "touch-disable-pen-handwriting-panel",
            Label = "Disable Pen Handwriting Panel Auto-Invoke",
            Category = "Peripherals — Virtual Disk Service",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents the handwriting panel from auto-appearing when using a pen in text fields. Default: auto.",
            Tags = ["touch", "pen", "handwriting", "panel"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7", "EnableDesktopModeAutoInvoke", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7", "EnableDesktopModeAutoInvoke", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7", "EnableDesktopModeAutoInvoke", 0)],
        },
        new TweakDef
        {
            Id = "touch-disable-flicks-policy",
            Label = "Disable Pen Flicks via Group Policy",
            Category = "Peripherals — Virtual Disk Service",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables pen flicks (gesture shortcuts) system-wide via Group Policy. Default: pen flicks enabled.",
            Tags = ["touch", "pen", "flicks", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\TabletPC"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\TabletPC", "TurnOffFlicks", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\TabletPC", "TurnOffFlicks")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\TabletPC", "TurnOffFlicks", 1)],
        },
        new TweakDef
        {
            Id = "touch-disable-ink-workspace-app-suggestions",
            Label = "Disable Ink Workspace App Suggestions",
            Category = "Peripherals — Virtual Disk Service",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables suggested app recommendations inside the Windows Ink Workspace. Default: enabled.",
            Tags = ["touch", "pen", "ink", "suggestions"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\PenWorkspace"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\PenWorkspace", "PenWorkspaceAppSuggestionsEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\PenWorkspace", "PenWorkspaceAppSuggestionsEnabled", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\PenWorkspace", "PenWorkspaceAppSuggestionsEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "touch-disable-touch-keyboard-suggestions",
            Label = "Disable Touch Keyboard Autocomplete Suggestions",
            Category = "Peripherals — Virtual Disk Service",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables word suggestions shown above the touch keyboard. Reduces distraction and speeds up touch typing. Default: suggestions enabled.",
            Tags = ["touch", "keyboard", "suggestions", "autocomplete"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Input\Settings"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Input\Settings", "EnableHwKeyboardAutocorrect", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Input\Settings", "EnableHwKeyboardAutocorrect", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Input\Settings", "EnableHwKeyboardAutocorrect", 0)],
        },
        new TweakDef
        {
            Id = "touch-set-double-tap-speed",
            Label = "Reduce Pen Double-Tap Detection Speed",
            Category = "Peripherals — Virtual Disk Service",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Reduces the time window for pen double-tap detection. Makes double-tap feel snappier. Default: 500ms window.",
            Tags = ["touch", "pen", "double-tap", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Accessibility\SlateLaunch"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Accessibility\SlateLaunch", "ATWRelaxTimeout", 200)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Control Panel\Accessibility\SlateLaunch", "ATWRelaxTimeout")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Control Panel\Accessibility\SlateLaunch", "ATWRelaxTimeout", 200)],
        },
        new TweakDef
        {
            Id = "touch-disable-tablet-pc-input-service",
            Label = "Disable Tablet PC Input Service",
            Category = "Peripherals — Virtual Disk Service",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Tablet PC Input Service which supports pen and touch functionality on non-tablet PCs. Frees resources on desktop systems. Default: enabled.",
            Tags = ["touch", "tablet-pc", "services"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\TabletInputService"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\TabletInputService", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\TabletInputService", "Start", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\TabletInputService", "Start", 4)],
        },
        new TweakDef
        {
            Id = "touch-disable-pen-flick-sound",
            Label = "Disable Pen Flick Sound",
            Category = "Peripherals — Virtual Disk Service",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the sound effect played when performing pen flick gestures. Default: sound enabled.",
            Tags = ["touch", "pen", "sound", "flick"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Tablet"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Tablet", "FlickSoundEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Tablet", "FlickSoundEnabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Tablet", "FlickSoundEnabled", 0)],
        },
        new TweakDef
        {
            Id = "touch-disable-touch-prediction",
            Label = "Disable Touch Input Prediction",
            Category = "Peripherals — Virtual Disk Service",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables predictive touch movement smoothing. Reduces latency for precise stylus work. Default: prediction enabled.",
            Tags = ["touch", "pen", "prediction", "latency"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Precision Touchpad"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Precision Touchpad", "TouchPredictionEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Precision Touchpad", "TouchPredictionEnabled", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Precision Touchpad", "TouchPredictionEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "touch-disable-autocorrect",
            Label = "Disable Touch Keyboard Auto-Correct",
            Category = "Peripherals — Virtual Disk Service",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables automatic text correction on the touch keyboard. Keeps typed words exactly as entered. Default: autocorrect enabled.",
            Tags = ["touch", "keyboard", "autocorrect"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\TabletTip\1.7"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\TabletTip\1.7", "EnableAutoCorrection", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\TabletTip\1.7", "EnableAutoCorrection", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\TabletTip\1.7", "EnableAutoCorrection", 0)],
        },
        new TweakDef
        {
            Id = "touch-disable-tablet-mode-auto-switch",
            Label = "Disable Automatic Tablet Mode Switching",
            Category = "Peripherals — Virtual Disk Service",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Prevents Windows from automatically switching to tablet mode when a keyboard is detached. Keeps desktop mode at all times. Default: auto-switch enabled on convertibles.",
            Tags = ["touch", "tablet-mode", "ui"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\ImmersiveShell"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\ImmersiveShell", "TabletMode", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\ImmersiveShell", "TabletMode")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\ImmersiveShell", "TabletMode", 0)],
        },
        new TweakDef
        {
            Id = "touch-disable-auto-keyboard-invoke",
            Label = "Disable Automatic Touch Keyboard Pop-Up",
            Category = "Peripherals — Virtual Disk Service",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents the touch keyboard from automatically appearing when a text input field is focused on a touchscreen. Requires manual keyboard invocation. Default: auto-invoke enabled.",
            Tags = ["touch", "keyboard", "auto-invoke", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsTouchKeyboard"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsTouchKeyboard", "DisableAutoKeyboard", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsTouchKeyboard", "DisableAutoKeyboard")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsTouchKeyboard", "DisableAutoKeyboard", 1)],
        },
        new TweakDef
        {
            Id = "touch-disable-edge-gesture",
            Label = "Disable Touch Edge Gesture Swipe",
            Category = "Peripherals — Virtual Disk Service",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the edge swipe gesture that opens Action Center or other system panels on touchscreen devices. Reduces accidental activation. Default: edge gestures enabled.",
            Tags = ["touch", "gesture", "edge-swipe", "ui"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\ImmersiveShell\EdgeUi"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\ImmersiveShell\EdgeUi", "DisabledEdges", 3)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\ImmersiveShell\EdgeUi", "DisabledEdges")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\ImmersiveShell\EdgeUi", "DisabledEdges", 3)],
        },
    ];
}
