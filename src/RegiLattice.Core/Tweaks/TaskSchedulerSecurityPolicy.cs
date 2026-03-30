// RegiLattice.Core — Tweaks/TaskSchedulerSecurityPolicy.cs
// Task Scheduler access control and security policy — Sprint 634.
// Category: "Task Scheduler Security Policy" | Slug: schtasksec
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\Task Scheduler5.0

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class TaskSchedulerSecurityPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Task Scheduler5.0";
    private const string CompatKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Task Scheduler5.0\Compatibility";
    private const string MaintKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Task Scheduler5.0\Maintenance";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id           = "schtasksec-disable-task-creation",
            Label        = "Prevent Non-Admin Task Creation",
            Category     = "Task Scheduler Security Policy",
            Description  = "Prevents standard users from creating new scheduled tasks. Only administrators can create, modify, or delete tasks. Default: allowed. Recommended for hardened systems.",
            Tags         = ["scheduled-tasks", "security", "hardening", "user-restriction", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 3,
            ImpactNote   = "Standard users cannot create scheduled tasks; admin tasks unaffected.",
            ApplyOps     = [RegOp.SetDword(Key, "TaskCreation", 0)],
            RemoveOps    = [RegOp.DeleteValue(Key, "TaskCreation")],
            DetectOps    = [RegOp.CheckDword(Key, "TaskCreation", 0)],
        },
        new TweakDef
        {
            Id           = "schtasksec-disable-task-deletion",
            Label        = "Prevent Non-Admin Task Deletion",
            Category     = "Task Scheduler Security Policy",
            Description  = "Prevents standard users from deleting existing scheduled tasks. Protects system maintenance and backup tasks from accidental removal. Default: allowed.",
            Tags         = ["scheduled-tasks", "security", "hardening", "user-restriction", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 3,
            ImpactNote   = "Standard users cannot remove any scheduled tasks.",
            ApplyOps     = [RegOp.SetDword(Key, "TaskDeletion", 0)],
            RemoveOps    = [RegOp.DeleteValue(Key, "TaskDeletion")],
            DetectOps    = [RegOp.CheckDword(Key, "TaskDeletion", 0)],
        },
        new TweakDef
        {
            Id           = "schtasksec-disable-execution-control",
            Label        = "Prevent Non-Admin Manual Task Execution",
            Category     = "Task Scheduler Security Policy",
            Description  = "Prevents standard users from running existing tasks on-demand (right-click → Run). Tasks still execute on their configured schedule. Default: allowed.",
            Tags         = ["scheduled-tasks", "security", "hardening", "execution", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 4,
            ImpactNote   = "Standard users can't manually trigger tasks; scheduled execution continues normally.",
            ApplyOps     = [RegOp.SetDword(Key, "Execution", 0)],
            RemoveOps    = [RegOp.DeleteValue(Key, "Execution")],
            DetectOps    = [RegOp.CheckDword(Key, "Execution", 0)],
        },
        new TweakDef
        {
            Id           = "schtasksec-disable-drag-drop-run",
            Label        = "Disable Drag-and-Drop Task Execution",
            Category     = "Task Scheduler Security Policy",
            Description  = "Prevents running a scheduled task by dragging and dropping a file onto its entry. Reduces accidental or social-engineering-based task execution. Default: allowed.",
            Tags         = ["scheduled-tasks", "security", "hardening", "drag-drop", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 2,
            SafetyRating = 5,
            ImpactNote   = "Drag-and-drop execution blocked; no impact on normal scheduled execution.",
            ApplyOps     = [RegOp.SetDword(Key, "DragAndDrop", 0)],
            RemoveOps    = [RegOp.DeleteValue(Key, "DragAndDrop")],
            DetectOps    = [RegOp.CheckDword(Key, "DragAndDrop", 0)],
        },
        new TweakDef
        {
            Id           = "schtasksec-disable-property-pages",
            Label        = "Hide Task Scheduler Property Pages",
            Category     = "Task Scheduler Security Policy",
            Description  = "Hides the property pages (settings, triggers, conditions, history) for existing scheduled tasks from standard users. Prevents information disclosure of task details. Default: visible.",
            Tags         = ["scheduled-tasks", "security", "information-disclosure", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 2,
            SafetyRating = 5,
            ImpactNote   = "Standard users cannot view task configuration details; admin view unaffected.",
            ApplyOps     = [RegOp.SetDword(Key, "PropertyPages", 0)],
            RemoveOps    = [RegOp.DeleteValue(Key, "PropertyPages")],
            DetectOps    = [RegOp.CheckDword(Key, "PropertyPages", 0)],
        },
        new TweakDef
        {
            Id           = "schtasksec-disable-browse-network",
            Label        = "Disable Network Browse in Task Scheduler",
            Category     = "Task Scheduler Security Policy",
            Description  = "Prevents the Task Scheduler from browsing the network for remote task targets. Limits attack surface when managing scheduled tasks. Default: allowed.",
            Tags         = ["scheduled-tasks", "security", "network", "hardening", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 2,
            SafetyRating = 5,
            ImpactNote   = "Cannot browse remote computers from Task Scheduler; local management unaffected.",
            ApplyOps     = [RegOp.SetDword(Key, "AllowBrowse", 0)],
            RemoveOps    = [RegOp.DeleteValue(Key, "AllowBrowse")],
            DetectOps    = [RegOp.CheckDword(Key, "AllowBrowse", 0)],
        },
        new TweakDef
        {
            Id           = "schtasksec-disable-at-compatibility",
            Label        = "Disable AT Command Compatibility in Task Scheduler",
            Category     = "Task Scheduler Security Policy",
            Description  = "Disables the legacy AT.exe command compatibility layer. AT-scheduled tasks bypass modern security controls. Default: compatible.",
            Tags         = ["scheduled-tasks", "security", "legacy", "at-command", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 4,
            ImpactNote   = "Legacy AT.exe tasks no longer execute; modern schtasks/Task Scheduler UI unaffected.",
            ApplyOps     = [RegOp.SetDword(CompatKey, "DisableATCompatibility", 1)],
            RemoveOps    = [RegOp.DeleteValue(CompatKey, "DisableATCompatibility")],
            DetectOps    = [RegOp.CheckDword(CompatKey, "DisableATCompatibility", 1)],
        },
        new TweakDef
        {
            Id           = "schtasksec-disable-v1-api",
            Label        = "Disable Task Scheduler 1.0 API Compatibility",
            Category     = "Task Scheduler Security Policy",
            Description  = "Disables the legacy Task Scheduler 1.0 COM API. Prevents applications using the old interface from creating or modifying tasks. Default: enabled.",
            Tags         = ["scheduled-tasks", "security", "legacy", "api", "com", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 3,
            ImpactNote   = "Legacy COM-based task management disabled; may break old automation scripts.",
            ApplyOps     = [RegOp.SetDword(CompatKey, "DisableV1Api", 1)],
            RemoveOps    = [RegOp.DeleteValue(CompatKey, "DisableV1Api")],
            DetectOps    = [RegOp.CheckDword(CompatKey, "DisableV1Api", 1)],
        },
        new TweakDef
        {
            Id           = "schtasksec-set-maint-boundary-days",
            Label        = "Set Maintenance Task Deadline to 7 Days",
            Category     = "Task Scheduler Security Policy",
            Description  = "Sets the automatic maintenance activation boundary to 7 days. If maintenance has not run in 7 days, the system forces it on next idle. Default: 2 days.",
            Tags         = ["scheduled-tasks", "maintenance", "deadline", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 2,
            SafetyRating = 5,
            ImpactNote   = "Maintenance forced after 7 days of missed windows; more lenient than default 2 days.",
            ApplyOps     = [RegOp.SetDword(MaintKey, "DeadlineDays", 7)],
            RemoveOps    = [RegOp.DeleteValue(MaintKey, "DeadlineDays")],
            DetectOps    = [RegOp.CheckDword(MaintKey, "DeadlineDays", 7)],
        },
        new TweakDef
        {
            Id           = "schtasksec-disable-maint-wakeup",
            Label        = "Disable Maintenance Task Wake-Up Timer",
            Category     = "Task Scheduler Security Policy",
            Description  = "Prevents the automatic maintenance task from waking the computer from sleep. Maintenance only runs when the device is already awake. Default: may wake.",
            Tags         = ["scheduled-tasks", "maintenance", "wake-timer", "power", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Computer not woken from sleep for maintenance; maintenance runs on next idle wake.",
            ApplyOps     = [RegOp.SetDword(MaintKey, "WakeUp", 0)],
            RemoveOps    = [RegOp.DeleteValue(MaintKey, "WakeUp")],
            DetectOps    = [RegOp.CheckDword(MaintKey, "WakeUp", 0)],
        },
    ];
}
