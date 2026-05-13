namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// ── Merged from VirtualDesktops.cs ──────────────────────────────────────────────────

[TweakModule]
internal static class VirtualDesktops
{
    private const string VdKey = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\VirtualDesktops";

    private const string TaskView = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced";

    private const string TaskViewPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer";

    private const string AltTabKey = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced";

    private const string DwmKey = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "vd-show-all-windows-in-alt-tab",
            Label = "Show All VD Windows in Alt+Tab",
            Category = "Windows 11 2",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["virtual desktops", "alt-tab", "task view", "windows"],
            Description =
                "Configures Alt+Tab to show windows from all virtual desktops (default "
                + "Win11 behavior). Value 1 = all desktops. Useful if a previous policy "
                + "restricted Alt+Tab to the current desktop only.",
            ApplyOps = [RegOp.SetDword(AltTabKey, "VirtualDesktopAltTabFilter", 1)],
            RemoveOps = [RegOp.DeleteValue(AltTabKey, "VirtualDesktopAltTabFilter")],
            DetectOps = [RegOp.CheckDword(AltTabKey, "VirtualDesktopAltTabFilter", 1)],
        },
        new TweakDef
        {
            Id = "vd-disable-task-view-system",
            Label = "Disable Task View Feature via Policy",
            Category = "Windows 11 2",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 2,
            SafetyRating = 4,
            Tags = ["virtual desktops", "task view", "policy", "disable"],
            Description =
                "Disables the Task View (Win+Tab) feature entirely via system policy. "
                + "Users cannot access virtual desktops, and the shortcut is disabled. "
                + "Intended for kiosk/corporate lockdown environments.",
            ApplyOps = [RegOp.SetDword(TaskViewPolicy, "DisableTaskView", 1)],
            RemoveOps = [RegOp.DeleteValue(TaskViewPolicy, "DisableTaskView")],
            DetectOps = [RegOp.CheckDword(TaskViewPolicy, "DisableTaskView", 1)],
        },
        new TweakDef
        {
            Id = "vd-disable-snap-assist-flyout",
            Label = "Disable Snap Assist Desktop Flyout on Switch",
            Category = "Windows 11 2",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["virtual desktops", "snap", "flyout", "animation", "ux"],
            Description =
                "Disables the snap group flyout that appears when hovering over taskbar "
                + "buttons during virtual desktop operations. Reduces UI clutter for power "
                + "users who prefer compact taskbar behavior.",
            ApplyOps = [RegOp.SetDword(TaskView, "EnableSnapAssistFlyout", 0)],
            RemoveOps = [RegOp.SetDword(TaskView, "EnableSnapAssistFlyout", 1)],
            DetectOps = [RegOp.CheckDword(TaskView, "EnableSnapAssistFlyout", 0)],
        },
        new TweakDef
        {
            Id = "vd-disable-desktops-on-taskbar",
            Label = "Disable Virtual Desktop Previews on Taskbar Hover",
            Category = "Windows 11 2",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["virtual desktops", "taskbar", "preview", "hover"],
            Description =
                "Disables the virtual desktop thumbnail previews that appear when hovering "
                + "over the Task View button. Saves screen space and reduces compositor load "
                + "on hover-intensive workflows.",
            ApplyOps = [RegOp.SetDword(TaskView, "TaskView", 0)],
            RemoveOps = [RegOp.SetDword(TaskView, "TaskView", 1)],
            DetectOps = [RegOp.CheckDword(TaskView, "TaskView", 0)],
        },
        new TweakDef
        {
            Id = "vd-set-taskbar-multimonitor-all",
            Label = "Show All Desktop Windows on All Monitor Taskbars",
            Category = "Windows 11 2",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["virtual desktops", "taskbar", "multimonitor", "windows"],
            Description =
                "Configures multi-monitor taskbars to show buttons for windows "
                + "from all virtual desktops (MMTaskbarMode=0). The default shows "
                + "only the current desktop on each monitor's taskbar.",
            ApplyOps = [RegOp.SetDword(TaskView, "MMTaskbarMode", 0)],
            RemoveOps = [RegOp.DeleteValue(TaskView, "MMTaskbarMode")],
            DetectOps = [RegOp.CheckDword(TaskView, "MMTaskbarMode", 0)],
        },
        new TweakDef
        {
            Id = "vd-disable-snap-revert",
            Label = "Disable Snap Revert (Don't Move Partner Window When Moving Dragged Window)",
            Category = "Windows 11 2",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["virtual desktops", "snap", "revert", "layout"],
            Description =
                "When dragging a snapped window away from its position, disables the "
                + "automatic reverting of the paired window to its pre-snap size and position "
                + "(SnapRevert=0). Gives more predictable behavior.",
            ApplyOps = [RegOp.SetDword(TaskView, "SnapRevert", 0)],
            RemoveOps = [RegOp.SetDword(TaskView, "SnapRevert", 1)],
            DetectOps = [RegOp.CheckDword(TaskView, "SnapRevert", 0)],
        },
        new TweakDef
        {
            Id = "vd-enable-background-per-desktop",
            Label = "Enable Unique Wallpaper Per Virtual Desktop",
            Category = "Windows 11 2",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Tags = ["virtual desktops", "wallpaper", "background", "personalization"],
            Description =
                "Allows each virtual desktop to have its own wallpaper that changes "
                + "automatically when you switch desktops. Enables the per-desktop background "
                + "feature (BackgroundChangesOnDesktopSwitch=1).",
            ApplyOps = [RegOp.SetDword(VdKey, "BackgroundChangesOnDesktopSwitch", 1)],
            RemoveOps = [RegOp.SetDword(VdKey, "BackgroundChangesOnDesktopSwitch", 0)],
            DetectOps = [RegOp.CheckDword(VdKey, "BackgroundChangesOnDesktopSwitch", 1)],
        },
        new TweakDef
        {
            Id = "vd-show-taskbar-on-secondary-monitors",
            Label = "Show Taskbar on All Monitors (Multi-Monitor)",
            Category = "Windows 11 2",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["virtual desktops", "taskbar", "multimonitor", "secondary"],
            Description =
                "Enables the extended taskbar on secondary monitors so each display "
                + "shows its own taskbar. Useful when the multi-monitor taskbar was "
                + "previously disabled (MMTaskbarEnabled=1).",
            ApplyOps = [RegOp.SetDword(TaskView, "MMTaskbarEnabled", 1)],
            RemoveOps = [RegOp.SetDword(TaskView, "MMTaskbarEnabled", 0)],
            DetectOps = [RegOp.CheckDword(TaskView, "MMTaskbarEnabled", 1)],
        },
        new TweakDef
        {
            Id = "vd-disable-taskbar-end-task-button",
            Label = "Disable End Task Button on Taskbar (Windows 11 23H2+)",
            Category = "Windows 11 2",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Tags = ["virtual desktops", "taskbar", "end-task", "windows 11"],
            Description =
                "Removes the 'End Task' button that appears in the right-click context "
                + "menu for taskbar buttons in Windows 11 version 23H2 and later. "
                + "Prevents accidental process termination (TaskbarEndTask=0).",
            ApplyOps = [RegOp.SetDword(TaskView, "TaskbarEndTask", 0)],
            RemoveOps = [RegOp.SetDword(TaskView, "TaskbarEndTask", 1)],
            DetectOps = [RegOp.CheckDword(TaskView, "TaskbarEndTask", 0)],
        },
    ];
}
