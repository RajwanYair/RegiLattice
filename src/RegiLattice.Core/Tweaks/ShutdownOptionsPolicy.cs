// RegiLattice.Core — Tweaks/ShutdownOptionsPolicy.cs
// Sprint 286: Shutdown Options Group Policy (10 tweaks)
// Category: "Shutdown Options Policy" | Slug: shtdwn
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ShutdownOptions

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class ShutdownOptionsPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ShutdownOptions";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "shtdwn-disable-shutdown-on-ctrl-alt-del",
            Label = "Disable Shutdown from Ctrl+Alt+Del Screen",
            Category = "Shutdown Options Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
            Description =
                "Sets NoShutdownOnCtrlAltDel=1 in the ShutdownOptions policy key. "
                + "Removes the Shut down button from the Ctrl+Alt+Del secure attention "
                + "sequence screen, preventing non-admin users from shutting down or "
                + "restarting the machine without appropriate privilege. On shared "
                + "workstations and call-centre desktops accidental shutdown of "
                + "production machines causes service disruption. Default: 0.",
            Tags = ["shutdown", "ctrl-alt-del", "security", "lockdown", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "NoShutdownOnCtrlAltDel", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "NoShutdownOnCtrlAltDel")],
            DetectOps = [RegOp.CheckDword(Key, "NoShutdownOnCtrlAltDel", 1)],
        },
        new TweakDef
        {
            Id = "shtdwn-require-shutdown-reason",
            Label = "Require Shutdown Reason and Comment",
            Category = "Shutdown Options Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
            Description =
                "Sets ShutdownReasonOn=1 in the ShutdownOptions policy key. Forces the "
                + "shutdown dialog to display a reason code drop-down and optional "
                + "comment field before accepting a restart or shutdown command. "
                + "Mandatory shutdown reason codes create an audit trail of who shut "
                + "down a system and why, which is valuable in production server "
                + "environments and shared workstations. Default: 0.",
            Tags = ["shutdown", "reason", "audit", "compliance", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "ShutdownReasonOn", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "ShutdownReasonOn")],
            DetectOps = [RegOp.CheckDword(Key, "ShutdownReasonOn", 1)],
        },
        new TweakDef
        {
            Id = "shtdwn-disable-restart-apps",
            Label = "Disable App Restart After Reboot",
            Category = "Shutdown Options Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
            Description =
                "Sets DisableAppRestart=1 in the ShutdownOptions policy key. Prevents "
                + "Windows from re-launching applications registered in the RunOnce "
                + "restart list after a reboot. Some application installers and updaters "
                + "use the restart application list to auto-launch their product after "
                + "a reboot; disabling this keeps the post-reboot session clean and "
                + "consistent in enterprise imaging workflows. Default: 0.",
            Tags = ["shutdown", "restart", "apps", "runonce", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableAppRestart", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableAppRestart")],
            DetectOps = [RegOp.CheckDword(Key, "DisableAppRestart", 1)],
        },
        new TweakDef
        {
            Id = "shtdwn-disable-automatic-restart",
            Label = "Disable Automatic Restart After BSOD",
            Category = "Shutdown Options Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 4, SafetyRating = 5,
            Description =
                "Sets DisableAutomaticRestart=1 in the ShutdownOptions policy key. "
                + "Prevents the system from automatically rebooting after a fatal Stop "
                + "error (Blue Screen of Death). Automatic restart hides the bugcheck "
                + "code and error details from the user before they can note the stop "
                + "code; disabling it allows engineers to read and photograph the BSOD "
                + "screen and correlate it with kernel dump analysis. Default: 0.",
            Tags = ["shutdown", "bsod", "crash", "restart", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableAutomaticRestart", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableAutomaticRestart")],
            DetectOps = [RegOp.CheckDword(Key, "DisableAutomaticRestart", 1)],
        },
        new TweakDef
        {
            Id = "shtdwn-disable-legacy-logoff-script",
            Label = "Disable Legacy Logoff Script Delay",
            Category = "Shutdown Options Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets MaxWaitForScriptDelay=0 in the ShutdownOptions policy key. Sets "
                + "the maximum time the system will wait for legacy Group Policy logoff "
                + "or shutdown scripts to complete before forcing termination to 0 "
                + "seconds. Long-running logoff scripts delay shutdown chains in VDI "
                + "environments and can prevent clean hyper-visor-level snapshotting "
                + "during overnight maintenance windows. Default: 600.",
            Tags = ["shutdown", "script", "logoff", "delay", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "MaxWaitForScriptDelay", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "MaxWaitForScriptDelay")],
            DetectOps = [RegOp.CheckDword(Key, "MaxWaitForScriptDelay", 0)],
        },
        new TweakDef
        {
            Id = "shtdwn-disable-forced-reboot-notification",
            Label = "Disable Forced Reboot Notification Banner",
            Category = "Shutdown Options Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
            Description =
                "Sets DisableForcedRebootNotification=1 in the ShutdownOptions policy "
                + "key. Suppresses the notification banner that warns users of an "
                + "imminent forced restart scheduled by Windows Update or administrator "
                + "policy. While intended to inform users, in unattended VDI and "
                + "server contexts the notification triggers user-interactive dialogs "
                + "that block automated shutdown orchestration scripts. Default: 0.",
            Tags = ["shutdown", "reboot", "notification", "wus", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableForcedRebootNotification", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableForcedRebootNotification")],
            DetectOps = [RegOp.CheckDword(Key, "DisableForcedRebootNotification", 1)],
        },
        new TweakDef
        {
            Id = "shtdwn-disable-power-button-action",
            Label = "Disable Power Button Shutdown",
            Category = "Shutdown Options Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets DisablePowerButton=1 in the ShutdownOptions policy key. Prevents "
                + "the physical power button from triggering a shutdown or hibernate "
                + "action, regardless of the power plan's button-press setting. On "
                + "point-of-sale terminals, kiosk devices, and embedded panels the "
                + "power button is often inadvertently pressed during normal operation "
                + "causing unexpected downtime. Default: 0.",
            Tags = ["shutdown", "power-button", "kiosk", "hardware", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisablePowerButton", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisablePowerButton")],
            DetectOps = [RegOp.CheckDword(Key, "DisablePowerButton", 1)],
        },
        new TweakDef
        {
            Id = "shtdwn-disable-restart-button-start",
            Label = "Disable Restart Option in Start Menu",
            Category = "Shutdown Options Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets NoRestartFromStartMenu=1 in the ShutdownOptions policy key. "
                + "Removes the Restart option from the Start Menu power button flyout, "
                + "preventing standard users from restarting the system from the desktop. "
                + "On thin-client terminals and locked-down workstations, restart should "
                + "only be initiated by IT administrators through remote management "
                + "tools or scheduled maintenance windows. Default: 0.",
            Tags = ["shutdown", "start-menu", "restart", "lockdown", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "NoRestartFromStartMenu", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "NoRestartFromStartMenu")],
            DetectOps = [RegOp.CheckDword(Key, "NoRestartFromStartMenu", 1)],
        },
        new TweakDef
        {
            Id = "shtdwn-disable-hibernate-option",
            Label = "Disable Hibernate Option in Shutdown Menu",
            Category = "Shutdown Options Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
            Description =
                "Sets DisableHibernate=1 in the ShutdownOptions policy key. Removes "
                + "the Hibernate entry from the shutdown and power flyout menus. "
                + "Hibernate writes the full memory contents to the hiberfil.sys "
                + "pagefile, which may contain credentials, encryption keys, and "
                + "sensitive process data as unencrypted pages unless UEFI provides "
                + "a sealed hibernation key. Default: 0. Recommended: 1 on shared machines.",
            Tags = ["shutdown", "hibernate", "security", "power", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableHibernate", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableHibernate")],
            DetectOps = [RegOp.CheckDword(Key, "DisableHibernate", 1)],
        },
        new TweakDef
        {
            Id = "shtdwn-log-shutdown-events",
            Label = "Enable Shutdown Event Logging",
            Category = "Shutdown Options Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
            Description =
                "Sets LogShutdownEvents=1 in the ShutdownOptions policy key. Enables "
                + "recording of shutdown, restart, and logoff events with user identity, "
                + "timestamp, reason code, and any administrator comment to the "
                + "Security event log. Event log evidence of shutdown sequences is "
                + "critical for forensic timelines in incident response and for "
                + "demonstrating change-control compliance in audits. Default: 0.",
            Tags = ["shutdown", "logging", "audit", "event-log", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "LogShutdownEvents", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "LogShutdownEvents")],
            DetectOps = [RegOp.CheckDword(Key, "LogShutdownEvents", 1)],
        },
    ];
}
