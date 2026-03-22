// RegiLattice.Core — Tweaks/VirtualDesktops.cs
// Windows 11 Virtual Desktops and Task View behavior settings (Sprint 81).
// Slug "vd" — Task View, virtual desktop taskbar behavior, desktop-switch animations.
// Distinct from Win11.cs (visual/snap) and Taskbar.cs (taskbar pinning/layout).

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class VirtualDesktops
{
    private const string VdKey = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\VirtualDesktops";

    private const string TaskView = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced";

    private const string TaskViewPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer";

    private const string AltTabKey = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "vd-show-all-windows-in-alt-tab",
            Label = "Show All VD Windows in Alt+Tab",
            Category = "Virtual Desktops",
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
            Id = "vd-show-current-desktop-in-alt-tab",
            Label = "Show Only Current VD Windows in Alt+Tab",
            Category = "Virtual Desktops",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["virtual desktops", "alt-tab", "focus", "productivity"],
            Description =
                "Restricts Alt+Tab to only show windows from the current virtual desktop. "
                + "Value 2 = current desktop only. Reduces clutter when using many desktops.",
            ApplyOps = [RegOp.SetDword(AltTabKey, "VirtualDesktopAltTabFilter", 2)],
            RemoveOps = [RegOp.DeleteValue(AltTabKey, "VirtualDesktopAltTabFilter")],
            DetectOps = [RegOp.CheckDword(AltTabKey, "VirtualDesktopAltTabFilter", 2)],
        },
        new TweakDef
        {
            Id = "vd-show-all-taskbar-buttons",
            Label = "Show Taskbar Buttons from All Virtual Desktops",
            Category = "Virtual Desktops",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["virtual desktops", "taskbar", "buttons", "all desktops"],
            Description =
                "Configures the taskbar to show app buttons from all virtual desktops "
                + "instead of only the current one. Value 1 = show all. Useful for quick "
                + "cross-desktop app switching without Task View.",
            ApplyOps = [RegOp.SetDword(TaskView, "VirtualDesktopTaskbarFilter", 1)],
            RemoveOps = [RegOp.DeleteValue(TaskView, "VirtualDesktopTaskbarFilter")],
            DetectOps = [RegOp.CheckDword(TaskView, "VirtualDesktopTaskbarFilter", 1)],
        },
        new TweakDef
        {
            Id = "vd-show-current-taskbar-buttons",
            Label = "Show Only Current VD Taskbar Buttons",
            Category = "Virtual Desktops",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["virtual desktops", "taskbar", "current desktop", "focus"],
            Description =
                "Restricts the taskbar to only show buttons for apps open on the current "
                + "virtual desktop. Value 2 = current desktop only. Keeps the taskbar clean "
                + "when using many virtual desktops.",
            ApplyOps = [RegOp.SetDword(TaskView, "VirtualDesktopTaskbarFilter", 2)],
            RemoveOps = [RegOp.DeleteValue(TaskView, "VirtualDesktopTaskbarFilter")],
            DetectOps = [RegOp.CheckDword(TaskView, "VirtualDesktopTaskbarFilter", 2)],
        },
        new TweakDef
        {
            Id = "vd-disable-task-view-button",
            Label = "Hide Task View Button from Taskbar",
            Category = "Virtual Desktops",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["virtual desktops", "task view", "taskbar", "button", "clean"],
            Description =
                "Hides the Task View (multi-desktop) button from the taskbar. "
                + "Virtual desktops remain functional via Win+Tab or Win+Ctrl+D. "
                + "Reduces taskbar clutter on single-user desktops.",
            ApplyOps = [RegOp.SetDword(TaskView, "ShowTaskViewButton", 0)],
            RemoveOps = [RegOp.SetDword(TaskView, "ShowTaskViewButton", 1)],
            DetectOps = [RegOp.CheckDword(TaskView, "ShowTaskViewButton", 0)],
        },
        new TweakDef
        {
            Id = "vd-disable-task-view-system",
            Label = "Disable Task View Feature via Policy",
            Category = "Virtual Desktops",
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
            Id = "vd-disable-timeline",
            Label = "Disable Windows Timeline",
            Category = "Virtual Desktops",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["virtual desktops", "timeline", "activity history", "privacy"],
            Description =
                "Disables the Windows Timeline feature in Task View that tracks your "
                + "recent activity and documents. Prevents activity history collection "
                + "and removes the timeline from Task View.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableActivityFeed", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableActivityFeed")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableActivityFeed", 0)],
        },
        new TweakDef
        {
            Id = "vd-disable-timeline-upload",
            Label = "Disable Timeline Activity Sync to Cloud",
            Category = "Virtual Desktops",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["virtual desktops", "timeline", "cloud sync", "privacy", "microsoft account"],
            Description =
                "Prevents Timeline activity history from being uploaded to Microsoft cloud "
                + "servers (requires a Microsoft account). Activity remains local only even "
                + "if Timeline is still enabled.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "UploadUserActivities", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "UploadUserActivities")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "UploadUserActivities", 0)],
        },
        new TweakDef
        {
            Id = "vd-disable-snap-assist-flyout",
            Label = "Disable Snap Assist Desktop Flyout on Switch",
            Category = "Virtual Desktops",
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
            Category = "Virtual Desktops",
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
    ];
}
