#nullable enable
using System.Collections.Generic;
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

// Kiosk & Assigned Access — lockdown settings for public terminals, self-service kiosks,
// and shared-use devices.
// HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System
// HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System
// HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\AssignedAccessConfiguration
// HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat
internal static class KioskAssignedAccess
{
    private const string SysPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System";
    private const string WinPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";
    private const string AppCompatPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat";
    private const string GpoSys = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization";
    private const string ExplorerPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "kiosk-disable-task-manager",
            Label = "Kiosk: Disable Task Manager",
            Category = "Kiosk & Assigned Access",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [SysPolicy],
            Tags = ["kiosk", "lockdown", "task-manager", "policy"],
            Description =
                "Sets DisableTaskMgr=1 in Policies\\System. Prevents users from opening Task Manager "
                + "via Ctrl+Shift+Esc or right-clicking the taskbar. "
                + "Default: Task Manager accessible. Recommended for shared/kiosk machines.",
            ApplyOps = [RegOp.SetDword(SysPolicy, "DisableTaskMgr", 1)],
            RemoveOps = [RegOp.DeleteValue(SysPolicy, "DisableTaskMgr")],
            DetectOps = [RegOp.CheckDword(SysPolicy, "DisableTaskMgr", 1)],
        },
        new TweakDef
        {
            Id = "kiosk-disable-registry-editor",
            Label = "Kiosk: Disable Registry Editor Access",
            Category = "Kiosk & Assigned Access",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [SysPolicy],
            Tags = ["kiosk", "lockdown", "registry-editor", "policy"],
            Description =
                "Sets DisableRegistryTools=1 in Policies\\System. Blocks users from running regedit.exe. "
                + "Prevents tampering with registry settings on shared or public machines. "
                + "Default: regedit accessible to non-admin users in their hive.",
            ApplyOps = [RegOp.SetDword(SysPolicy, "DisableRegistryTools", 1)],
            RemoveOps = [RegOp.DeleteValue(SysPolicy, "DisableRegistryTools")],
            DetectOps = [RegOp.CheckDword(SysPolicy, "DisableRegistryTools", 1)],
        },
        new TweakDef
        {
            Id = "kiosk-disable-cmd",
            Label = "Kiosk: Disable Command Prompt for Standard Users",
            Category = "Kiosk & Assigned Access",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [SysPolicy],
            Tags = ["kiosk", "lockdown", "cmd", "command-prompt", "policy"],
            Description =
                "Sets DisableCMD=2 in Policies\\System. Prevents users from running cmd.exe or batch files. "
                + "Value 2 also disables batch file execution. "
                + "Default: cmd accessible. Kiosk/shared-device hardening.",
            ApplyOps = [RegOp.SetDword(SysPolicy, "DisableCMD", 2)],
            RemoveOps = [RegOp.DeleteValue(SysPolicy, "DisableCMD")],
            DetectOps = [RegOp.CheckDword(SysPolicy, "DisableCMD", 2)],
        },
        new TweakDef
        {
            Id = "kiosk-disable-control-panel",
            Label = "Kiosk: Block Control Panel and PC Settings",
            Category = "Kiosk & Assigned Access",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [ExplorerPolicy],
            Tags = ["kiosk", "lockdown", "control-panel", "settings", "policy"],
            Description =
                "Sets NoControlPanel=1 in Explorer policy. Removes Control Panel and Settings app from "
                + "Start menu and restricts direct access. "
                + "Default: accessible. Essential for kiosk and shared-terminal lockdown.",
            ApplyOps = [RegOp.SetDword(ExplorerPolicy, "NoControlPanel", 1)],
            RemoveOps = [RegOp.DeleteValue(ExplorerPolicy, "NoControlPanel")],
            DetectOps = [RegOp.CheckDword(ExplorerPolicy, "NoControlPanel", 1)],
        },
        new TweakDef
        {
            Id = "kiosk-disable-run-dialog",
            Label = "Kiosk: Remove Run Dialog from Start Menu",
            Category = "Kiosk & Assigned Access",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [ExplorerPolicy],
            Tags = ["kiosk", "lockdown", "run-dialog", "shell", "policy"],
            Description =
                "Sets NoRun=1 in Explorer policy. Removes the Run entry from the Start menu and "
                + "disables the Win+R keyboard shortcut. "
                + "Default: Run dialog accessible. Prevents launching arbitrary executables on kiosk.",
            ApplyOps = [RegOp.SetDword(ExplorerPolicy, "NoRun", 1)],
            RemoveOps = [RegOp.DeleteValue(ExplorerPolicy, "NoRun")],
            DetectOps = [RegOp.CheckDword(ExplorerPolicy, "NoRun", 1)],
        },
        new TweakDef
        {
            Id = "kiosk-lock-lock-screen",
            Label = "Kiosk: Prevent Changing Lock Screen Image via Policy",
            Category = "Kiosk & Assigned Access",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [GpoSys],
            Tags = ["kiosk", "lockscreen", "personalization", "policy"],
            Description =
                "Sets NoChangingLockScreen=1 in Personalization policy. Prevents users from changing the "
                + "lock screen image. Preserves corporate/kiosk branding. "
                + "Default: user can change the lock screen. Recommended for corporate shared machines.",
            ApplyOps = [RegOp.SetDword(GpoSys, "NoChangingLockScreen", 1)],
            RemoveOps = [RegOp.DeleteValue(GpoSys, "NoChangingLockScreen")],
            DetectOps = [RegOp.CheckDword(GpoSys, "NoChangingLockScreen", 1)],
        },
        new TweakDef
        {
            Id = "kiosk-disable-slideshow-personalization-gpo",
            Label = "Kiosk: Disable Lock Screen Slideshow via Personalization Policy",
            Category = "Kiosk & Assigned Access",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [GpoSys],
            Tags = ["kiosk", "lockscreen", "slideshow", "personalization", "policy"],
            Description =
                "Sets NoLockScreenSlideshow=1 in Personalization policy. Prevents the lock screen from "
                + "cycling through pictures from Libraries or OneDrive. "
                + "Default: slideshow can be enabled by user. Useful for kiosk branding consistency.",
            ApplyOps = [RegOp.SetDword(GpoSys, "NoLockScreenSlideshow", 1)],
            RemoveOps = [RegOp.DeleteValue(GpoSys, "NoLockScreenSlideshow")],
            DetectOps = [RegOp.CheckDword(GpoSys, "NoLockScreenSlideshow", 1)],
        },
        new TweakDef
        {
            Id = "kiosk-disable-camera-on-lockscreen",
            Label = "Kiosk: Disable Camera Access from Lock Screen",
            Category = "Kiosk & Assigned Access",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [GpoSys],
            Tags = ["kiosk", "lockscreen", "camera", "privacy", "policy"],
            Description =
                "Sets AllowCameraOnLockScreen=0 in Personalization policy (GPO). Prevents the lock screen "
                + "camera shortcut from activating the camera without authentication. "
                + "Default: camera accessible from lock screen. Privacy-relevant for unattended public terminals.",
            ApplyOps = [RegOp.SetDword(GpoSys, "AllowCameraOnLockScreen", 0)],
            RemoveOps = [RegOp.DeleteValue(GpoSys, "AllowCameraOnLockScreen")],
            DetectOps = [RegOp.CheckDword(GpoSys, "AllowCameraOnLockScreen", 0)],
        },
        new TweakDef
        {
            Id = "kiosk-disable-app-compat-wizard",
            Label = "Kiosk: Disable Program Compatibility Wizard",
            Category = "Kiosk & Assigned Access",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [AppCompatPolicy],
            Tags = ["kiosk", "app-compat", "wizard", "policy", "lockdown"],
            Description =
                "Sets DisableEngine=2 in AppCompat policy. Disables the Program Compatibility Wizard "
                + "that appears when applications crash. Prevents users from interacting with compatibility "
                + "repairs and sending data to Microsoft. Default: wizard enabled.",
            ApplyOps = [RegOp.SetDword(AppCompatPolicy, "DisableEngine", 2)],
            RemoveOps = [RegOp.DeleteValue(AppCompatPolicy, "DisableEngine")],
            DetectOps = [RegOp.CheckDword(AppCompatPolicy, "DisableEngine", 2)],
        },
        new TweakDef
        {
            Id = "kiosk-prevent-logoff-shutdown",
            Label = "Kiosk: Remove Shut Down and Restart from Start Menu",
            Category = "Kiosk & Assigned Access",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [ExplorerPolicy],
            Tags = ["kiosk", "shutdown", "restart", "lockdown", "policy"],
            Description =
                "Sets NoClose=1 in Explorer policy. Removes the Shut Down, Restart, Sleep, and Log Off "
                + "options from the Start menu. Users cannot initiate system shutdown. "
                + "Default: all power options visible. Useful for kiosk sessions requiring admin to power off.",
            ApplyOps = [RegOp.SetDword(ExplorerPolicy, "NoClose", 1)],
            RemoveOps = [RegOp.DeleteValue(ExplorerPolicy, "NoClose")],
            DetectOps = [RegOp.CheckDword(ExplorerPolicy, "NoClose", 1)],
        },
    ];
}
