namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class TouchPen
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "touch-disable-ink-workspace",
            Label = "Disable Windows Ink Workspace",
            Category = "Touch & Pen",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Windows Ink Workspace via Group Policy. Hides the Ink Workspace button and features.",
            Tags = ["touch", "pen", "ink", "workspace", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace", "AllowWindowsInkWorkspace", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace", "AllowWindowsInkWorkspace", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace", "AllowWindowsInkWorkspace", 0)],
        },
        new TweakDef
        {
            Id = "touch-disable-pen-button",
            Label = "Hide Pen Workspace Taskbar Button",
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Hides the Windows Ink Workspace button from the taskbar. Pen still works; only the shortcut button is hidden.",
            Tags = ["touch", "pen", "taskbar", "button"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PenWorkspace"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PenWorkspace", "PenWorkspaceButtonDesiredVisibility", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PenWorkspace", "PenWorkspaceButtonDesiredVisibility", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PenWorkspace",
                    "PenWorkspaceButtonDesiredVisibility",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "touch-disable-handwriting",
            Label = "Disable Handwriting Personalisation",
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Stops Windows from collecting handwriting and inking data for personalisation. Recommended: Disabled for privacy.",
            Tags = ["touch", "pen", "handwriting", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\InputPersonalization"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\InputPersonalization", "RestrictImplicitInkCollection", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\InputPersonalization", "RestrictImplicitInkCollection", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\InputPersonalization", "RestrictImplicitInkCollection", 1)],
        },
        new TweakDef
        {
            Id = "touch-disable-ink-suggestions",
            Label = "Disable Ink Work Suggested Apps",
            Category = "Touch & Pen",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes suggested apps from the Windows Ink Workspace. Policy setting.",
            Tags = ["touch", "pen", "ink", "suggestions", "ads"],
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
            Id = "touch-pen-screenshot",
            Label = "Pen Double-Click: Screen Sketch",
            Category = "Touch & Pen",
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
            Category = "Touch & Pen",
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
            Id = "touch-disable-tablet-auto",
            Label = "Disable Tablet Mode Auto-Switch",
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows from switching to tablet mode when a keyboard is detached or folded.",
            Tags = ["touch", "tablet", "mode", "convertible"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ImmersiveShell"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ImmersiveShell", "SignInMode", 1),
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ImmersiveShell",
                    "ConvertibleSlateModePromptPreference",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ImmersiveShell", "SignInMode"),
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ImmersiveShell",
                    "ConvertibleSlateModePromptPreference"
                ),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ImmersiveShell", "SignInMode", 1)],
        },
        new TweakDef
        {
            Id = "touch-disable-handwriting-panel",
            Label = "Disable Touch Handwriting Panel",
            Category = "Touch & Pen",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the handwriting input panel that appears when tapping text fields with a pen. Policy setting.",
            Tags = ["touch", "pen", "handwriting", "panel", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\TabletPC"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\InputPersonalization", "RestrictImplicitInkCollection", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\InputPersonalization", "RestrictImplicitInkCollection", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\InputPersonalization", "RestrictImplicitInkCollection", 1)],
        },
        new TweakDef
        {
            Id = "touch-disable-3finger",
            Label = "Disable Three-Finger Gestures",
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables three-finger tap and slide gestures on precision touchpads (task view, volume, etc.).",
            Tags = ["touch", "touchpad", "gestures", "three-finger"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "ThreeFingerSlideEnabled", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "ThreeFingerTapEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "ThreeFingerSlideEnabled", 1),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "ThreeFingerTapEnabled", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "ThreeFingerSlideEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "touch-disable-4finger",
            Label = "Disable Four-Finger Gestures",
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables four-finger tap and slide gestures on precision touchpads (desktop switch, etc.).",
            Tags = ["touch", "touchpad", "gestures", "four-finger"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "FourFingerSlideEnabled", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "FourFingerTapEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "FourFingerSlideEnabled", 1),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "FourFingerTapEnabled", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "FourFingerSlideEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "touch-disable-edge-swipe",
            Label = "Disable Edge Swipe Gesture (Policy)",
            Category = "Touch & Pen",
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
            Category = "Touch & Pen",
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
            Id = "touch-disable-2finger-tap",
            Label = "Disable Two-Finger Tap (Right-Click)",
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables two-finger tap as a right-click gesture on precision touchpads. Prevents accidental right-click menus while typing. Default: Enabled.",
            Tags = ["touch", "touchpad", "two-finger", "right-click", "gesture"],
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
            Id = "touch-disable-pinch-zoom",
            Label = "Disable Touchpad Pinch-to-Zoom",
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the two-finger pinch-to-zoom gesture on precision touchpads. Prevents accidental zoom changes. Default: Enabled.",
            Tags = ["touch", "touchpad", "pinch", "zoom", "gesture"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "ZoomEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "ZoomEnabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "ZoomEnabled", 0)],
        },
        new TweakDef
        {
            Id = "touch-reverse-scroll",
            Label = "Enable Reverse (Natural) Scrolling",
            Category = "Touch & Pen",
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
            Category = "Touch & Pen",
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
            Category = "Touch & Pen",
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
            Category = "Touch & Pen",
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
            Category = "Touch & Pen",
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
            Category = "Touch & Pen",
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
            Category = "Touch & Pen",
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
            Id = "touch-disable-pen-workspace",
            Label = "Disable Pen Workspace Button",
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Hides the Windows Ink Workspace button from the taskbar system tray. Default: visible.",
            Tags = ["touch", "pen", "ink-workspace", "taskbar"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PenWorkspace"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PenWorkspace", "PenWorkspaceButtonDesiredVisibility", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PenWorkspace", "PenWorkspaceButtonDesiredVisibility"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PenWorkspace",
                    "PenWorkspaceButtonDesiredVisibility",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "touch-disable-visual-feedback",
            Label = "Disable Touch Visual Feedback",
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the visual feedback animations shown when touching the screen. Removes the touch ripple effects. Default: enabled.",
            Tags = ["touch", "visual", "feedback", "animation"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Cursors"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Cursors", "ContactVisualization", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Cursors", "ContactVisualization", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Control Panel\Cursors", "ContactVisualization", 0)],
        },
        new TweakDef
        {
            Id = "touch-disable-flicks-policy",
            Label = "Disable Pen Flicks via Group Policy",
            Category = "Touch & Pen",
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
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables suggested app recommendations inside the Windows Ink Workspace. Default: enabled.",
            Tags = ["touch", "pen", "ink", "suggestions"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\PenWorkspace"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\PenWorkspace", "PenWorkspaceAppSuggestionsEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\PenWorkspace", "PenWorkspaceAppSuggestionsEnabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\PenWorkspace", "PenWorkspaceAppSuggestionsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "touch-disable-handwriting-panel-auto",
            Label = "Disable Handwriting Panel Auto-Invoke",
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables automatic pop-up of the touch handwriting panel when a text field is focussed with a pen. Default: auto-show enabled.",
            Tags = ["touch", "pen", "handwriting"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\TabletTip\1.7"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\TabletTip\1.7", "TipbandDesiredVisibility", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\TabletTip\1.7", "TipbandDesiredVisibility")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\TabletTip\1.7", "TipbandDesiredVisibility", 0)],
        },
        new TweakDef
        {
            Id = "touch-disable-touch-keyboard-deploy",
            Label = "Disable Auto Touch Keyboard Deployment",
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables automatic deployment of the touch keyboard when a text field is tapped. Useful when using an external keyboard with touch input. Default: auto-deploy.",
            Tags = ["touch", "keyboard", "auto"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\TabletTip\1.7"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\TabletTip\1.7", "EnableDesktopModeAutoInvoke", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\TabletTip\1.7", "EnableDesktopModeAutoInvoke", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\TabletTip\1.7", "EnableDesktopModeAutoInvoke", 0)],
        },
        new TweakDef
        {
            Id = "touch-disable-touch-keyboard-suggestions",
            Label = "Disable Touch Keyboard Autocomplete Suggestions",
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables word suggestions shown above the touch keyboard. Reduces distraction and speeds up touch typing. Default: suggestions enabled.",
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
            Category = "Touch & Pen",
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
            Category = "Touch & Pen",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Tablet PC Input Service which supports pen and touch functionality on non-tablet PCs. Frees resources on desktop systems. Default: enabled.",
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
            Category = "Touch & Pen",
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
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables predictive touch movement smoothing. Reduces latency for precise stylus work. Default: prediction enabled.",
            Tags = ["touch", "pen", "prediction", "latency"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Precision Touchpad"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Precision Touchpad", "TouchPredictionEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Precision Touchpad", "TouchPredictionEnabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Precision Touchpad", "TouchPredictionEnabled", 0)],
        },
        new TweakDef
        {
            Id = "touch-disable-pen-workspace-button",
            Label = "Hide Pen Workspace Button from Taskbar",
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes the pen workspace icon from the taskbar notification area. Default: visible when pen is attached.",
            Tags = ["touch", "pen", "taskbar", "ink"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\PenWorkspace"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\PenWorkspace", "PenWorkspaceButtonDesiredVisibility", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\PenWorkspace", "PenWorkspaceButtonDesiredVisibility")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\PenWorkspace", "PenWorkspaceButtonDesiredVisibility", 0)],
        },
    ];
}
