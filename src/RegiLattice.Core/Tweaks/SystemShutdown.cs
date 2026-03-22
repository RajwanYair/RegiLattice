// RegiLattice.Core — Tweaks/SystemShutdown.cs
// Shutdown, restart, hibernate, and fast startup behaviour (Sprint 89).
// Slug "shdn" — Distinct from Power.cs (power plans) and Boot.cs (boot loader).
// Controls how Windows shuts down, how fast startup works, and logoff UI.

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class SystemShutdown
{
    private const string WinLogon = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon";

    private const string CurrentVersion = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion";

    private const string PoliciesSystem = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System";

    private const string PoliciesExplorer = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer";

    private const string PowerSettings = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Power";

    private const string SessionManager = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "shdn-disable-fast-startup",
            Label = "Disable Fast Startup (Hybrid Boot)",
            Category = "System Shutdown",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["shutdown", "fast startup", "hibernate", "sleep"],
            Description =
                "Disables Hybrid Boot / Fast Startup (HiberbootEnabled=0). Fast startup "
                + "uses hibernation for the kernel session, which can cause issues with "
                + "dual-boot, full disk encryption, and Windows Update applying kernel patches.",
            ApplyOps = [RegOp.SetDword(PowerSettings, "HiberbootEnabled", 0)],
            RemoveOps = [RegOp.SetDword(PowerSettings, "HiberbootEnabled", 1)],
            DetectOps = [RegOp.CheckDword(PowerSettings, "HiberbootEnabled", 0)],
        },
        new TweakDef
        {
            Id = "shdn-reduce-wait-to-kill-timeout",
            Label = "Reduce WaitToKillServiceTimeout to 5 Seconds",
            Category = "System Shutdown",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Tags = ["shutdown", "service", "kill timeout", "speed"],
            Description =
                "Reduces the time Windows waits for services to stop during shutdown "
                + "from the default 20,000 ms to 5,000 ms. Speeds up shutdown at the "
                + "cost of slightly less graceful service termination.",
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "WaitToKillServiceTimeout", "5000")],
            RemoveOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "WaitToKillServiceTimeout", "20000")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "WaitToKillServiceTimeout", "5000")],
        },
        new TweakDef
        {
            Id = "shdn-reduce-hung-app-timeout",
            Label = "Reduce HungAppTimeout to 4 Seconds",
            Category = "System Shutdown",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Tags = ["shutdown", "hung app", "timeout", "speed"],
            Description =
                "Reduces the time Windows waits before showing the 'This application is "
                + "not responding' prompt during shutdown. HungAppTimeout=4000 ms "
                + "(default is 5000 ms). Slightly quicker dialog trigger.",
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "HungAppTimeout", "4000")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "HungAppTimeout", "5000")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "HungAppTimeout", "4000")],
        },
        new TweakDef
        {
            Id = "shdn-enable-auto-end-tasks",
            Label = "Enable AutoEndTasks (Kill Hung Apps on Logout)",
            Category = "System Shutdown",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Tags = ["shutdown", "auto end tasks", "hung app", "logout"],
            Description =
                "Enables AutoEndTasks=1 so Windows automatically terminates applications "
                + "that are hanging during logout or shutdown without waiting for user "
                + "confirmation. Speeds up shutdown considerably.",
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "AutoEndTasks", "1")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "AutoEndTasks", "0")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "AutoEndTasks", "1")],
        },
        new TweakDef
        {
            Id = "shdn-disable-shutdown-event-tracker",
            Label = "Disable Shutdown Event Tracker (No Reason Required)",
            Category = "System Shutdown",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["shutdown", "event tracker", "policy", "server"],
            Description =
                "Disables the Shutdown Event Tracker that asks administrators why they "
                + "are shutting down or restarting. Normally enabled only on Windows Server. "
                + "ShutdownReasonUI=0.",
            ApplyOps = [RegOp.SetDword(PoliciesSystem, "ShutdownReasonUI", 0)],
            RemoveOps = [RegOp.DeleteValue(PoliciesSystem, "ShutdownReasonUI")],
            DetectOps = [RegOp.CheckDword(PoliciesSystem, "ShutdownReasonUI", 0)],
        },
        new TweakDef
        {
            Id = "shdn-suppress-logoff-scripts-run-at-shutdown",
            Label = "Run Logoff Scripts Simultaneously with Shutdown Scripts",
            Category = "System Shutdown",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["shutdown", "logoff", "scripts", "gpo"],
            Description =
                "Configures logoff and shutdown scripts to run simultaneously rather "
                + "than sequentially. Reduces total script execution time during logout. "
                + "RunLogonScriptSync=0.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "RunLogonScriptSync", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "RunLogonScriptSync")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "RunLogonScriptSync", 0)],
        },
        new TweakDef
        {
            Id = "shdn-clear-page-file-on-shutdown",
            Label = "Clear Page File on System Shutdown",
            Category = "System Shutdown",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Tags = ["shutdown", "page file", "security", "memory wipe"],
            Description =
                "Zeroes the page file during system shutdown (ClearPageFileAtShutdown=1). "
                + "Prevents sensitive data left in paging memory from being extracted from "
                + "the disk when hibernation is disabled. Adds a few seconds to shutdown.",
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "ClearPageFileAtShutdown",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "ClearPageFileAtShutdown",
                    0
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "ClearPageFileAtShutdown",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "shdn-reduce-process-kill-timeout",
            Label = "Reduce WaitToKillAppTimeout to 4 Seconds",
            Category = "System Shutdown",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Tags = ["shutdown", "app kill", "timeout", "speed"],
            Description =
                "Reduces WaitToKillAppTimeout to 4,000 ms (from 20,000 ms default). "
                + "Shortens the time the system waits for app processes to exit before "
                + "forcing them to terminate during logout/shutdown.",
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WaitToKillAppTimeout", "4000")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WaitToKillAppTimeout", "20000")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WaitToKillAppTimeout", "4000")],
        },
        new TweakDef
        {
            Id = "shdn-suppress-logoff-slow-scripts-ui",
            Label = "Disable 'Slow Script' Warning at Shutdown",
            Category = "System Shutdown",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Tags = ["shutdown", "gpo", "slow script", "ui"],
            Description =
                "Hides the 'Please wait for the <script> to finish' message shown when "
                + "GPO logoff/shutdown scripts exceed MaxGPOScriptWait. Prevents the UI "
                + "from blocking shutdown on domain machines with slow scripts.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "HideShutdownScripts", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "HideShutdownScripts")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "HideShutdownScripts", 0)],
        },
        new TweakDef
        {
            Id = "shdn-disable-restart-apps-on-signin",
            Label = "Disable Restart of Apps After Reboot/Sign-In",
            Category = "System Shutdown",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["shutdown", "restart", "startup apps", "reboot", "sign-in"],
            Description =
                "Prevents Windows from re-opening apps that were running before a reboot "
                + "or sign-out/sign-in cycle. RestartApps=0. Keeps the desktop clean "
                + "after every login.",
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Winlogon", "RestartApps", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Winlogon", "RestartApps")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Winlogon", "RestartApps", 0)],
        },
    ];
}
