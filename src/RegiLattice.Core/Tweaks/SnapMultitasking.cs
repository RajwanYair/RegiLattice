namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class SnapMultitasking
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "snap-disable-snap-assist",
            Label = "Disable Snap Assist",
            Category = "Snap & Multitasking",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disable showing suggestions when snapping windows. Default: enabled. Recommended: personal preference.",
            Tags = ["snap", "assist", "window", "suggestion"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SnapAssist", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SnapAssist", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SnapAssist", 0)],
        },
        new TweakDef
        {
            Id = "snap-disable-snap-layouts",
            Label = "Disable Snap Layouts Flyout",
            Category = "Snap & Multitasking",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disable the hover-over maximize button Snap Layouts flyout (Win11). Default: enabled.",
            Tags = ["snap", "layouts", "flyout", "maximize", "win11"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableSnapAssistFlyout", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableSnapAssistFlyout", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableSnapAssistFlyout", 0)],
        },
        new TweakDef
        {
            Id = "snap-disable-snap-groups",
            Label = "Disable Snap Groups in Alt+Tab",
            Category = "Snap & Multitasking",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disable grouping Snap windows in Alt+Tab and taskbar. Default: enabled.",
            Tags = ["snap", "groups", "alt-tab", "taskbar"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableTaskGroups", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableTaskGroups", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableTaskGroups", 0)],
        },
        new TweakDef
        {
            Id = "snap-alttab-windows-only",
            Label = "Alt+Tab: Open Windows Only",
            Category = "Snap & Multitasking",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Show only open windows in Alt+Tab, not browser tabs. Default: includes Edge tabs.",
            Tags = ["alt-tab", "tabs", "edge", "windows"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\MultitaskingView\AllUpView"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\MultitaskingView\AllUpView", "Enabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\MultitaskingView\AllUpView", "Enabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\MultitaskingView\AllUpView", "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "snap-disable-aero-shake",
            Label = "Disable Aero Shake",
            Category = "Snap & Multitasking",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disable shaking a window title bar to minimise all others. Default: enabled.",
            Tags = ["aero", "shake", "minimize"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisallowShaking", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisallowShaking", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisallowShaking", 1)],
        },
        new TweakDef
        {
            Id = "snap-disable-window-animations",
            Label = "Disable Window Min/Max Animations",
            Category = "Snap & Multitasking",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disable minimize/maximize window animation for snappier feel. Default: enabled.",
            Tags = ["animation", "minimize", "maximize", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "MinAnimate", "0"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "MinAnimate", "1"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "MinAnimate", "0")],
        },
        new TweakDef
        {
            Id = "snap-disable-vd-edge-swipe",
            Label = "Disable Virtual Desktop Edge Swipe",
            Category = "Snap & Multitasking",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disable edge swipe to switch virtual desktops (policy). Default: enabled.",
            Tags = ["virtual-desktop", "edge", "swipe", "gesture"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EdgeUI"],
        },
        new TweakDef
        {
            Id = "snap-vd-all-monitors",
            Label = "Show Desktops on All Monitors",
            Category = "Snap & Multitasking",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Show virtual desktop windows on all monitors in taskbar. Default: current monitor only.",
            Tags = ["virtual-desktop", "monitor", "taskbar"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "VirtualDesktopTaskbarFilter", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "VirtualDesktopTaskbarFilter", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "VirtualDesktopTaskbarFilter", 0)],
        },
        new TweakDef
        {
            Id = "snap-disable-auto-arrange",
            Label = "Disable Auto-Arrange on Dock",
            Category = "Snap & Multitasking",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disable automatic window rearrangement when docking/undocking. Default: enabled.",
            Tags = ["dock", "arrange", "resize"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "JointResize", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "JointResize", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "JointResize", 0)],
        },
        new TweakDef
        {
            Id = "snap-disable-snap-fill",
            Label = "Disable Snap Fill Available Space",
            Category = "Snap & Multitasking",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disable automatically filling available space when snapping a window. Default: enabled.",
            Tags = ["snap", "fill", "space", "resize"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SnapFill", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SnapFill", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SnapFill", 0)],
        },
        new TweakDef
        {
            Id = "snap-disable-snap-suggestions",
            Label = "Disable Snap Window Suggestions",
            Category = "Snap & Multitasking",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disable AI/suggested windows when snapping. Default: enabled.",
            Tags = ["snap", "suggestion", "ai"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SnapAssist", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DITest", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DITest", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DITest", 0)],
        },
        new TweakDef
        {
            Id = "snap-disable-dwm-anim-policy",
            Label = "Disable DWM Animations (Policy)",
            Category = "Snap & Multitasking",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Machine-wide policy to disable Desktop Window Manager animations. Default: enabled.",
            Tags = ["dwm", "animation", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DWM"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DWM", "DisallowAnimations", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DWM", "DisallowAnimations"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DWM", "DisallowAnimations", 1)],
        },
        new TweakDef
        {
            Id = "snap-disable-edge-swipe-nav",
            Label = "Disable Edge Swipe Navigation",
            Category = "Snap & Multitasking",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables edge swipe navigation gestures on touchscreens. Default: Enabled. Recommended: Disabled on desktops.",
            Tags = ["snap", "edge", "swipe", "gesture", "touch"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EdgeUI"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EdgeUI", "AllowEdgeSwipe", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EdgeUI", "AllowEdgeSwipe", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EdgeUI", "AllowEdgeSwipe", 0)],
        },
        new TweakDef
        {
            Id = "snap-disable-desktop-peek",
            Label = "Disable Desktop Peek",
            Category = "Snap & Multitasking",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Aero Peek / desktop preview when hovering over the Show Desktop button. Default: Enabled.",
            Tags = ["snap", "peek", "desktop", "aero"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
        },
        new TweakDef
        {
            Id = "snap-disable-flyout",
            Label = "Disable Snap Fly-Out Overlay",
            Category = "Snap & Multitasking",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the snap bar fly-out overlay when dragging windows. Reduces visual clutter during window arrangement. Default: Enabled. Recommended: Disabled.",
            Tags = ["snap", "flyout", "overlay", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableSnapBar", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableSnapBar"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableSnapBar", 0)],
        },
        new TweakDef
        {
            Id = "snap-vd-switch-anim-speed",
            Label = "Set Virtual Desktop Switch Animation to Fast",
            Category = "Snap & Multitasking",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets virtual desktop switch animation speed to 100ms (fast). Reduces perceived lag when switching desktops. Default: 200. Recommended: 100.",
            Tags = ["snap", "virtual-desktop", "animation", "speed"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM"],
        },
        new TweakDef
        {
            Id = "snap-disable-resize-snap",
            Label = "Disable Window Resize Snap Assist",
            Category = "Snap & Multitasking",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables joint resize when dragging the border between two snapped windows. Prevents accidental resizing of adjacent snapped windows. Default: Enabled. Recommended: Disabled.",
            Tags = ["snap", "resize", "joint", "multitasking"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "JointResize", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "JointResize", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "JointResize", 0)],
        },
    ];
}
