namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Boot
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "boot-verbose-boot",
            Label = "Verbose Boot Messages",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Shows detailed service and driver status messages during boot.",
            Tags = ["boot", "debug", "diagnostic"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
        },
        new TweakDef
        {
            Id = "boot-disable-boot-logo",
            Label = "Disable Boot Splash Logo",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Windows boot splash animation for a text-mode boot.",
            Tags = ["boot", "performance", "splash"],
        },
        new TweakDef
        {
            Id = "boot-fast-boot-timeout",
            Label = "Reduce Boot Menu Timeout (3s)",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Reduces the OS selection boot menu timeout from 30s to 3s.",
            Tags = ["boot", "performance", "timeout"],
        },
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
            Id = "boot-disable-driver-verifier",
            Label = "Disable Boot Driver Verifier (Perf)",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Windows Driver Verifier at boot for faster startup. Only relevant if Driver Verifier was previously enabled for debugging. Default: Disabled. Recommended: Keep disabled for production.",
            Tags = ["boot", "performance", "driver"],
        },
        new TweakDef
        {
            Id = "boot-log",
            Label = "Enable Boot Log (ntbtlog.txt)",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Creates a text file (ntbtlog.txt) listing all drivers loaded during boot. Useful for diagnosing boot problems.",
            Tags = ["boot", "debug", "log"],
        },
        new TweakDef
        {
            Id = "boot-disable-winre",
            Label = "Disable Windows Recovery Environment",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Windows Recovery Environment (WinRE). Saves ~450 MB of reserved disk space but prevents automatic repair on failures.",
            Tags = ["boot", "recovery", "disk"],
        },
        new TweakDef
        {
            Id = "boot-disable-auto-repair",
            Label = "Disable Automatic Boot Repair",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Prevents Windows from launching Automatic Repair after boot failures. Useful when the repair loop causes more harm than good.",
            Tags = ["boot", "repair", "recovery"],
        },
        new TweakDef
        {
            Id = "boot-ignore-boot-failures",
            Label = "Ignore All Boot Failures",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Sets boot status policy to ignore all failures, suppressing the 'Windows did not start successfully' screen.",
            Tags = ["boot", "performance", "failure"],
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
            Id = "boot-disable-logo",
            Label = "Disable Boot Logo",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Shows verbose status messages during boot instead of the Windows logo animation. Useful for diagnosing slow boot. Default: Logo. Recommended: Verbose for troubleshooting.",
            Tags = ["boot", "verbose", "diagnostics"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control"],
        },
        new TweakDef
        {
            Id = "boot-menu-timeout",
            Label = "Reduce Boot Menu Timeout",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Reduces the automatic disk check (ChkDsk) countdown from 10 to 3 seconds on boot. Speeds up boot when a disk check is pending. Default: 10s. Recommended: 3s.",
            Tags = ["boot", "chkdsk", "timeout", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager"],
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
            Id = "boot-max-proc-count",
            Label = "Disable Registry Flush Bandwidth Limit",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the bandwidth limit for registry hive flushing during boot and shutdown. Can speed up boot on systems with fast storage. Default: system-managed. Recommended: 0 (unlimited) on NVMe/SSD.",
            Tags = ["boot", "registry", "flush", "performance", "ssd"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager"],
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
            Id = "boot-disable-startup-sound",
            Label = "Disable Boot Startup Sound",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Windows startup sound that plays during boot. Useful for silent or headless environments. Default: Enabled. Recommended: Disabled for quiet boot.",
            Tags = ["boot", "sound", "startup", "silent"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\LogonUI\BootAnimation"],
        },
        new TweakDef
        {
            Id = "boot-disable-hiberfile",
            Label = "Disable Hibernation",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables hibernation and removes the hiberfil.sys file to free disk space. Default: Enabled. Recommended: Disabled on desktops with SSD.",
            Tags = ["boot", "hibernation", "disk-space", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power"],
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
    ];
}
