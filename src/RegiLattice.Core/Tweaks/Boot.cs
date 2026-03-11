namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Boot
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "boot-numlock-on-boot",
            Label = "Enable NumLock at Boot",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Turns on NumLock automatically at the login screen. Options: 0=Off, 2=On. Default: 0 (Off). Recommended: On.",
            Tags = ["boot", "keyboard", "numlock"],
            RegistryKeys = [@"HKEY_USERS\.DEFAULT\Control Panel\Keyboard"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_USERS\.DEFAULT\Control Panel\Keyboard", "InitialKeyboardIndicators", "2"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_USERS\.DEFAULT\Control Panel\Keyboard", "InitialKeyboardIndicators", "0"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_USERS\.DEFAULT\Control Panel\Keyboard", "InitialKeyboardIndicators", "2")],
        },
        new TweakDef
        {
            Id = "boot-disable-secboot-check",
            Label = "Suppress Secure Boot Status Check",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Suppresses the Secure Boot status notification in Windows by setting UEFISecureBootEnabled to 0 in the registry.",
            Tags = ["boot", "security", "uefi"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecureBoot\State"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecureBoot\State", "UEFISecureBootEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecureBoot\State", "UEFISecureBootEnabled", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecureBoot\State", "UEFISecureBootEnabled", 0)],
        },
        new TweakDef
        {
            Id = "boot-disable-boot-anim",
            Label = "Disable Boot Animation/Spinner",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Windows boot animation/spinner for a faster perceived boot. The boot process skips the animated dots. Default: enabled. Recommended: disabled for faster boot.",
            Tags = ["boot", "animation", "performance", "spinner"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootAnimationParams"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootAnimationParams", "Disabled", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootAnimationParams", "Disabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootAnimationParams", "Disabled", 1)],
        },
        new TweakDef
        {
            Id = "boot-enable-fast-startup",
            Label = "Enable Fast Startup (Hiberboot)",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Enables Windows Fast Startup which uses a hybrid shutdown with hibernation to speed up boot time. Default: Usually enabled. Recommended: Enabled for fast boot.",
            Tags = ["boot", "fast-startup", "hiberboot", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Power"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Power", "HiberbootEnabled", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Power", "HiberbootEnabled", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Power", "HiberbootEnabled", 1)],
        },
        new TweakDef
        {
            Id = "boot-prefetch-optimized",
            Label = "Set Prefetch to Optimized Mode",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Enables both boot and application prefetching for optimal performance. Value 3 = boot + app prefetch. Default: 3. Recommended: 3 for SSDs and HDDs.",
            Tags = ["boot", "prefetch", "performance", "startup"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters", "EnablePrefetcher", 3),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters", "EnablePrefetcher", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters", "EnablePrefetcher", 3)],
        },
        new TweakDef
        {
            Id = "boot-disable-auto-reboot",
            Label = "Disable Auto-Reboot on BSOD",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables automatic reboot after a Blue Screen of Death. Allows reading the full BSOD error before the system restarts. Default: Enabled. Recommended: Disabled for debugging.",
            Tags = ["boot", "bsod", "reboot", "crash", "debugging"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "AutoReboot", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "AutoReboot", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "AutoReboot", 0)],
        },
        new TweakDef
        {
            Id = "boot-full-bsod-info",
            Label = "Show Full BSOD Parameters",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Shows detailed crash parameters on the Blue Screen of Death. Displays bug-check code and arguments for troubleshooting. Default: Hidden. Recommended: Shown for diagnostics.",
            Tags = ["boot", "bsod", "parameters", "crash", "diagnostics"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "DisplayParameters", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "DisplayParameters"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "DisplayParameters", 1)],
        },
        new TweakDef
        {
            Id = "boot-disable-paging-executive",
            Label = "Disable Paging Executive",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Keeps kernel and drivers in physical RAM instead of paging to disk. Improves system responsiveness on machines with ample RAM. Default: Paging allowed. Recommended: Disabled.",
            Tags = ["boot", "paging", "kernel", "memory", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "DisablePagingExecutive", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "DisablePagingExecutive", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "DisablePagingExecutive", 1)],
        },
        new TweakDef
        {
            Id = "boot-reduce-service-timeout",
            Label = "Reduce Service Shutdown Timeout",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Reduces WaitToKillServiceTimeout to 2 seconds (default: 20s). Windows force-kills stuck services faster during shutdown. Default: 20000 ms. Recommended: 2000 ms for fast shutdown.",
            Tags = ["boot", "shutdown", "service", "timeout", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "WaitToKillServiceTimeout", "2000"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "WaitToKillServiceTimeout", "20000"),
            ],
        },
        new TweakDef
        {
            Id = "boot-reduce-hung-app-timeout",
            Label = "Reduce Hung Application Timeout",
            Category = "Boot",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Reduces HungAppTimeout to 1 second (default: 5s). Non-responsive apps are flagged as hung sooner, showing the 'not responding' dialog faster. Default: 5000 ms. Recommended: 1000 ms.",
            Tags = ["boot", "shutdown", "application", "hung", "timeout", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "HungAppTimeout", "1000"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "HungAppTimeout", "5000"),
            ],
        },
        new TweakDef
        {
            Id = "boot-reduce-wait-kill-app",
            Label = "Reduce Wait-to-Kill App Timeout",
            Category = "Boot",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Reduces WaitToKillAppTimeout to 2 seconds (default: 20s). Windows force-terminates unresponsive apps faster during shutdown. Default: 20000 ms. Recommended: 2000 ms for fast shutdown.",
            Tags = ["boot", "shutdown", "app", "timeout", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WaitToKillAppTimeout", "2000"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WaitToKillAppTimeout", "20000"),
            ],
        },
        new TweakDef
        {
            Id = "boot-clear-pagefile",
            Label = "Clear Pagefile at Shutdown",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Clears the virtual memory pagefile at every shutdown. Prevents sensitive data from being recovered from pagefile.sys. Note: significantly increases shutdown time on large systems. Default: not cleared. Recommended: Apply on secure workstations.",
            Tags = ["boot", "security", "pagefile", "shutdown", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "ClearPageFileAtShutdown", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "ClearPageFileAtShutdown", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "ClearPageFileAtShutdown", 1)],
        },
        new TweakDef
        {
            Id = "boot-disable-crash-dumps",
            Label = "Disable Crash Dump Creation",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables creation of memory dump files on BSOD (CrashDumpEnabled=0). Saves disk space and prevents sensitive memory data from being written to disk. Default: Small memory dump (7). Recommended: Disabled on production systems.",
            Tags = ["boot", "crash-dump", "bsod", "disk", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "CrashDumpEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "CrashDumpEnabled", 7),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "CrashDumpEnabled", 0)],
        },
        new TweakDef
        {
            Id = "boot-enable-boot-log",
            Label = "Enable Boot Log",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables the boot log file (ntbtlog.txt) that records all drivers loaded during startup. Useful for diagnosing boot issues. Default: disabled.",
            Tags = ["boot", "log", "diagnostics", "drivers"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "BootLog", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "BootLog")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "BootLog", 1)],
        },
        new TweakDef
        {
            Id = "boot-disable-boot-ux",
            Label = "Disable Boot UI Animation",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Windows boot animation (spinning dots). Shows a simple progress bar instead. Default: animated.",
            Tags = ["boot", "animation", "ui", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootControl", "BootProgressAnimation", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootControl", "BootProgressAnimation")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootControl", "BootProgressAnimation", 0)],
        },
        new TweakDef
        {
            Id = "boot-set-timeout-5s",
            Label = "Set Boot Menu Timeout to 5 Seconds",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the boot manager menu timeout to 5 seconds for dual-boot systems. Default: 30 seconds.",
            Tags = ["boot", "timeout", "dual-boot", "menu"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\BCD00000000\Objects\{9dea862c-5cdd-4e70-acc1-f32b344d4795}\Elements\25000004"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\BCD00000000\Objects\{9dea862c-5cdd-4e70-acc1-f32b344d4795}\Elements\25000004", "Element", 5)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\BCD00000000\Objects\{9dea862c-5cdd-4e70-acc1-f32b344d4795}\Elements\25000004", "Element", 30)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\BCD00000000\Objects\{9dea862c-5cdd-4e70-acc1-f32b344d4795}\Elements\25000004", "Element", 5)],
        },
        new TweakDef
        {
            Id = "boot-verbose-status-messages",
            Label = "Enable Verbose Boot Status Messages",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Shows detailed status messages during boot, shutdown, logon, and logoff. Default: hidden.",
            Tags = ["boot", "verbose", "status", "messages"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "VerboseStatus", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "VerboseStatus")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "VerboseStatus", 1)],
        },
        new TweakDef
        {
            Id = "boot-disable-windows-recovery",
            Label = "Disable Windows Recovery on Boot Failure",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables automatic recovery attempts after boot failures. Prevents boot repair loops. Default: recovery enabled.",
            Tags = ["boot", "recovery", "auto-repair", "server"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "AutoChkTimeout", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "AutoChkTimeout")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "AutoChkTimeout", 0)],
        },
    ];
}
