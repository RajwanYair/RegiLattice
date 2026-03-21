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
            ApplyOps = [RegOp.SetString(@"HKEY_USERS\.DEFAULT\Control Panel\Keyboard", "InitialKeyboardIndicators", "2")],
            RemoveOps = [RegOp.SetString(@"HKEY_USERS\.DEFAULT\Control Panel\Keyboard", "InitialKeyboardIndicators", "0")],
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
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecureBoot\State", "UEFISecureBootEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecureBoot\State", "UEFISecureBootEnabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecureBoot\State", "UEFISecureBootEnabled", 0)],
        },
        new TweakDef
        {
            Id = "boot-disable-boot-anim",
            Label = "Disable Boot Animation/Spinner",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Windows boot animation/spinner for a faster perceived boot. The boot process skips the animated dots. Default: enabled. Recommended: disabled for faster boot.",
            Tags = ["boot", "animation", "performance", "spinner"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootAnimationParams"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootAnimationParams", "Disabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootAnimationParams", "Disabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootAnimationParams", "Disabled", 1)],
        },
        new TweakDef
        {
            Id = "boot-enable-fast-startup",
            Label = "Enable Fast Startup (Hiberboot)",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Enables Windows Fast Startup which uses a hybrid shutdown with hibernation to speed up boot time. Default: Usually enabled. Recommended: Enabled for fast boot.",
            Tags = ["boot", "fast-startup", "hiberboot", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Power"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Power", "HiberbootEnabled", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Power", "HiberbootEnabled", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Power", "HiberbootEnabled", 1)],
        },
        new TweakDef
        {
            Id = "boot-prefetch-optimized",
            Label = "Set Prefetch to Optimized Mode",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Enables both boot and application prefetching for optimal performance. Value 3 = boot + app prefetch. Default: 3. Recommended: 3 for SSDs and HDDs.",
            Tags = ["boot", "prefetch", "performance", "startup"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                    "EnablePrefetcher",
                    3
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                    "EnablePrefetcher",
                    0
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                    "EnablePrefetcher",
                    3
                ),
            ],
        },
        new TweakDef
        {
            Id = "boot-disable-auto-reboot",
            Label = "Disable Auto-Reboot on BSOD",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables automatic reboot after a Blue Screen of Death. Allows reading the full BSOD error before the system restarts. Default: Enabled. Recommended: Disabled for debugging.",
            Tags = ["boot", "bsod", "reboot", "crash", "debugging"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "AutoReboot", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "AutoReboot", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "AutoReboot", 0)],
        },
        new TweakDef
        {
            Id = "boot-full-bsod-info",
            Label = "Show Full BSOD Parameters",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Shows detailed crash parameters on the Blue Screen of Death. Displays bug-check code and arguments for troubleshooting. Default: Hidden. Recommended: Shown for diagnostics.",
            Tags = ["boot", "bsod", "parameters", "crash", "diagnostics"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "DisplayParameters", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "DisplayParameters")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "DisplayParameters", 1)],
        },
        new TweakDef
        {
            Id = "boot-disable-paging-executive",
            Label = "Disable Paging Executive",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Keeps kernel and drivers in physical RAM instead of paging to disk. Improves system responsiveness on machines with ample RAM. Default: Paging allowed. Recommended: Disabled.",
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
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "DisablePagingExecutive",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "boot-reduce-service-timeout",
            Label = "Reduce Service Shutdown Timeout",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Reduces WaitToKillServiceTimeout to 2 seconds (default: 20s). Windows force-kills stuck services faster during shutdown. Default: 20000 ms. Recommended: 2000 ms for fast shutdown.",
            Tags = ["boot", "shutdown", "service", "timeout", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "WaitToKillServiceTimeout", "2000")],
            RemoveOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "WaitToKillServiceTimeout", "20000")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "WaitToKillServiceTimeout", "2000")],
        },
        new TweakDef
        {
            Id = "boot-reduce-hung-app-timeout",
            Label = "Reduce Hung Application Timeout",
            Category = "Boot",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Reduces HungAppTimeout to 1 second (default: 5s). Non-responsive apps are flagged as hung sooner, showing the 'not responding' dialog faster. Default: 5000 ms. Recommended: 1000 ms.",
            Tags = ["boot", "shutdown", "application", "hung", "timeout", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "HungAppTimeout", "1000")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "HungAppTimeout", "5000")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "HungAppTimeout", "1000")],
        },
        new TweakDef
        {
            Id = "boot-reduce-wait-kill-app",
            Label = "Reduce Wait-to-Kill App Timeout",
            Category = "Boot",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Reduces WaitToKillAppTimeout to 2 seconds (default: 20s). Windows force-terminates unresponsive apps faster during shutdown. Default: 20000 ms. Recommended: 2000 ms for fast shutdown.",
            Tags = ["boot", "shutdown", "app", "timeout", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WaitToKillAppTimeout", "2000")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WaitToKillAppTimeout", "20000")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WaitToKillAppTimeout", "2000")],
        },
        new TweakDef
        {
            Id = "boot-clear-pagefile",
            Label = "Clear Pagefile at Shutdown",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Clears the virtual memory pagefile at every shutdown. Prevents sensitive data from being recovered from pagefile.sys. Note: significantly increases shutdown time on large systems. Default: not cleared. Recommended: Apply on secure workstations.",
            Tags = ["boot", "security", "pagefile", "shutdown", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
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
            Id = "boot-disable-crash-dumps",
            Label = "Disable Crash Dump Creation",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables creation of memory dump files on BSOD (CrashDumpEnabled=0). Saves disk space and prevents sensitive memory data from being written to disk. Default: Small memory dump (7). Recommended: Disabled on production systems.",
            Tags = ["boot", "crash-dump", "bsod", "disk", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "CrashDumpEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "CrashDumpEnabled", 7)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "CrashDumpEnabled", 0)],
        },
        new TweakDef
        {
            Id = "boot-enable-boot-log",
            Label = "Enable Boot Log",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables the boot log file (ntbtlog.txt) that records all drivers loaded during startup. Useful for diagnosing boot issues. Default: disabled.",
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\BCD00000000\Objects\{9dea862c-5cdd-4e70-acc1-f32b344d4795}\Elements\25000004", "Element", 5),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\BCD00000000\Objects\{9dea862c-5cdd-4e70-acc1-f32b344d4795}\Elements\25000004", "Element", 30),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\BCD00000000\Objects\{9dea862c-5cdd-4e70-acc1-f32b344d4795}\Elements\25000004", "Element", 5),
            ],
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
        // ── Command-based boot tweaks (bcdedit) ────────────────────────────
        new TweakDef
        {
            Id = "boot-bcd-quiet-boot",
            Label = "Enable Quiet Boot (bcdedit)",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables Windows quiet boot mode via bcdedit — suppresses the boot logo and status messages for faster boot appearance.",
            Tags = ["boot", "bcdedit", "quiet", "logo"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/set", "{current}", "quietboot", "yes"]);
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/set", "{current}", "quietboot", "no"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{current}"]);
                return stdout.Contains("quietboot", StringComparison.OrdinalIgnoreCase) && stdout.Contains("Yes", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-bcd-timeout-3s",
            Label = "Set Boot Menu Timeout to 3 Seconds (bcdedit)",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the boot manager timeout to 3 seconds via bcdedit. Speeds up boot when multi-boot options exist.",
            Tags = ["boot", "bcdedit", "timeout", "fast"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/timeout", "3"]);
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/timeout", "30"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{bootmgr}"]);
                return stdout.Contains("timeout", StringComparison.OrdinalIgnoreCase) && stdout.Contains("3", StringComparison.Ordinal);
            },
        },
        new TweakDef
        {
            Id = "boot-bcd-disable-recovery",
            Label = "Disable Automatic Recovery (bcdedit)",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the automatic recovery/repair environment via bcdedit. Prevents boot loops but removes automatic repair capability.",
            Tags = ["boot", "bcdedit", "recovery", "repair", "server"],
            KindHint = TweakKind.SystemCommand,
            SideEffects = "Disables automatic repair on boot failure.",
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/set", "{current}", "recoveryenabled", "no"]);
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/set", "{current}", "recoveryenabled", "yes"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{current}"]);
                return stdout.Contains("recoveryenabled", StringComparison.OrdinalIgnoreCase)
                    && stdout.Contains("No", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-driver-verifier-reset",
            Label = "Reset Driver Verifier (verifier)",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Resets Driver Verifier settings to none. Useful after debugging driver issues when verifier was left enabled.",
            Tags = ["boot", "verifier", "driver", "diagnostic", "reset"],
            KindHint = TweakKind.SystemCommand,
            SideEffects = "Requires reboot to take effect.",
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("verifier", ["/reset"]);
            },
            // NOTE: No RemoveAction — "reset" is a one-shot diagnostic action. There is no
            // meaningful inverse; re-enabling verifier requires choosing specific drivers.
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("verifier", ["/query"]);
                return stdout.Contains("No drivers", StringComparison.OrdinalIgnoreCase)
                    || stdout.Contains("not loaded", StringComparison.OrdinalIgnoreCase);
            },
        },
        // ── Restored stubs with real operations ──────────────────

        new TweakDef
        {
            Id = "boot-disable-auto-repair",
            Label = "Disable Automatic Startup Repair",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Prevents Windows from launching Automatic Repair after consecutive boot failures. Use with caution.",
            Tags = ["boot", "auto-repair", "recovery", "bcdedit"],
            KindHint = TweakKind.SystemCommand,
            SideEffects = "System will not auto-recover from boot failures.",
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/set", "{current}", "bootstatuspolicy", "IgnoreAllFailures"]);
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/deletevalue", "{current}", "bootstatuspolicy"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{current}"]);
                return stdout.Contains("bootstatuspolicy", StringComparison.OrdinalIgnoreCase)
                    && stdout.Contains("IgnoreAllFailures", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-disable-boot-logo",
            Label = "Disable Boot Logo (bcdedit)",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Windows boot logo via bcdedit for a minimalist boot screen.",
            Tags = ["boot", "logo", "bcdedit", "ux"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/set", "{current}", "quietboot", "yes"]);
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/deletevalue", "{current}", "quietboot"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{current}"]);
                return stdout.Contains("quietboot", StringComparison.OrdinalIgnoreCase) && stdout.Contains("Yes", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-disable-driver-verifier",
            Label = "Disable Driver Verifier Flags",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Clears Driver Verifier flags in the registry. Useful after debugging when verifier causes boot loops.",
            Tags = ["boot", "verifier", "driver", "registry"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "VerifyDriverLevel", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "VerifyDriverLevel"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "VerifyDriverLevel", 0),
            ],
        },
        new TweakDef
        {
            Id = "boot-disable-hiberfile",
            Label = "Disable Hibernation File (hiberfil.sys)",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables hibernation, deleting hiberfil.sys and freeing disk space equal to RAM size.",
            Tags = ["boot", "hibernation", "disk-space", "power"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power"],
            SideEffects = "Disables hibernate and may disable Fast Startup.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "HibernateEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "HibernateEnabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "HibernateEnabled", 0)],
        },
        new TweakDef
        {
            Id = "boot-disable-logo",
            Label = "Disable OEM Boot Logo",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the OEM manufacturer logo during boot via bcdedit nologo option.",
            Tags = ["boot", "logo", "oem", "bcdedit"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/set", "{globalsettings}", "custom:16000067", "true"]);
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/deletevalue", "{globalsettings}", "custom:16000067"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{globalsettings}"]);
                return stdout.Contains("16000067", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-disable-winre",
            Label = "Disable WinRE Partition",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Windows Recovery Environment. Frees recovery partition but removes repair tools.",
            Tags = ["boot", "winre", "recovery", "disk-space"],
            KindHint = TweakKind.SystemCommand,
            SideEffects = "Removes access to Windows Recovery tools.",
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("reagentc", ["/disable"]);
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("reagentc", ["/enable"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("reagentc", ["/info"]);
                return stdout.Contains("Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-fast-boot-timeout",
            Label = "Set Boot Timeout to 0 Seconds",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Sets BCD boot menu timeout to 0 seconds for instant boot-through. No OS selection screen shown.",
            Tags = ["boot", "timeout", "bcdedit", "fast"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/timeout", "0"]);
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/timeout", "30"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{bootmgr}"]);
                return stdout.Contains("timeout", StringComparison.OrdinalIgnoreCase) && stdout.Contains("0", StringComparison.Ordinal);
            },
        },
        new TweakDef
        {
            Id = "boot-ignore-boot-failures",
            Label = "Ignore All Boot Failures",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Configures Windows to ignore all boot failures and skip the recovery screen. Use on stable systems only.",
            Tags = ["boot", "failures", "policy", "server"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            SideEffects = "Boot failures will not trigger automatic repair.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "DisplayDisabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "DisplayDisabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "DisplayDisabled", 1)],
        },
        new TweakDef
        {
            Id = "boot-log",
            Label = "Enable Boot Logging (ntbtlog.txt)",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables boot logging via bcdedit. Writes driver load info to %%SystemRoot%%\\ntbtlog.txt.",
            Tags = ["boot", "logging", "bcdedit", "diagnostic"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/set", "{current}", "bootlog", "yes"]);
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/set", "{current}", "bootlog", "no"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{current}"]);
                return stdout.Contains("bootlog", StringComparison.OrdinalIgnoreCase) && stdout.Contains("Yes", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-max-proc-count",
            Label = "Use All CPU Cores at Boot",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Configures msconfig-equivalent setting to use all processor cores during boot.",
            Tags = ["boot", "cpu", "cores", "performance", "bcdedit"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/deletevalue", "{current}", "numproc"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{current}"]);
                return !stdout.Contains("numproc", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-menu-timeout",
            Label = "Set Boot Menu Timeout to 10s",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the boot menu display timeout to 10 seconds. Useful for dual-boot systems.",
            Tags = ["boot", "timeout", "menu", "dual-boot", "bcdedit"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/timeout", "10"]);
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/timeout", "30"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{bootmgr}"]);
                return stdout.Contains("timeout", StringComparison.OrdinalIgnoreCase) && stdout.Contains("10", StringComparison.Ordinal);
            },
        },
        new TweakDef
        {
            Id = "boot-verbose-boot",
            Label = "Enable Verbose Boot Messages",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Shows detailed status messages during boot instead of the logo. Useful for debugging slow boot.",
            Tags = ["boot", "verbose", "diagnostic", "bcdedit"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/set", "{current}", "sos", "on"]);
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/set", "{current}", "sos", "off"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{current}"]);
                return stdout.Contains("sos", StringComparison.OrdinalIgnoreCase) && stdout.Contains("Yes", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-fast-startup-gpo",
            Label = "Enable Fast Startup via Group Policy",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets HiberbootEnabled=1 in the Windows System policy key to enforce fast startup at GPO level. Complements the standard fast startup registry setting. Default: not set.",
            Tags = ["boot", "fast-startup", "policy", "hibernate"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "HiberbootEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "HiberbootEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "HiberbootEnabled", 1)],
        },
        new TweakDef
        {
            Id = "boot-global-wait-timeout",
            Label = "Set Global Shutdown Wait Timeout to 5s",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets WaitForIdleState=5 in the system Timeout key. Controls how long Windows waits for the system to become idle before shutdown completes. Default: 2.",
            Tags = ["boot", "shutdown", "timeout", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Timeout"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Timeout", "WaitForIdleState", 5)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Timeout", "WaitForIdleState")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Timeout", "WaitForIdleState", 5)],
        },
        new TweakDef
        {
            Id = "boot-menu-timeout-policy",
            Label = "Set Boot Menu Display Timeout Policy to 10s",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets BootTimeoutSeconds=10 in the Windows System policy key. Controls the boot menu display time at policy level. Default: not set (uses BCD value).",
            Tags = ["boot", "menu", "timeout", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "BootTimeoutSeconds", 10)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "BootTimeoutSeconds")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "BootTimeoutSeconds", 10)],
        },
        new TweakDef
        {
            Id = "boot-hyperv-launch-off",
            Label = "Disable Hyper-V Hypervisor Launch",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Runs 'bcdedit /set hypervisorlaunchtype off' to disable the Hyper-V hypervisor at boot. Improves native performance on bare-metal gaming/workstation installs. Default: auto.",
            Tags = ["boot", "hyper-v", "bcd", "performance"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = dryRun =>
            {
                Elevation.AssertAdmin(dryRun);
                Elevation.RunElevated("bcdedit", ["/set", "hypervisorlaunchtype", "off"]);
            },
            RemoveAction = dryRun =>
            {
                Elevation.AssertAdmin(dryRun);
                Elevation.RunElevated("bcdedit", ["/set", "hypervisorlaunchtype", "auto"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{current}"]);
                return stdout.Contains("hypervisorlaunchtype", StringComparison.OrdinalIgnoreCase)
                    && stdout.Contains("Off", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-test-signing-off",
            Label = "Disable Test Signing Mode",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Runs 'bcdedit /set testsigning off' to disable test-signing mode. Prevents unsigned test drivers from loading. Default: off.",
            Tags = ["boot", "bcd", "security", "test-signing"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = dryRun =>
            {
                Elevation.AssertAdmin(dryRun);
                Elevation.RunElevated("bcdedit", ["/set", "testsigning", "off"]);
            },
            RemoveAction = dryRun =>
            {
                Elevation.AssertAdmin(dryRun);
                Elevation.RunElevated("bcdedit", ["/set", "testsigning", "on"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{current}"]);
                return stdout.Contains("testsigning", StringComparison.OrdinalIgnoreCase)
                    && stdout.Contains("No", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-report-ok",
            Label = "Enable Boot-OK Reporting to Winlogon",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets ReportBootOk=1 in Winlogon to signal that the current boot is clean and should be saved as the last known good configuration. Default: 1.",
            Tags = ["boot", "winlogon", "last-known-good", "recovery"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "ReportBootOk", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "ReportBootOk")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "ReportBootOk", 1)],
        },
        new TweakDef
        {
            Id = "boot-kernel-debug-filter",
            Label = "Suppress Kernel Debug Print Filter",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DEFAULT=0x0 in the Debug Print Filter to suppress kernel debug messages, reducing DbgPrint overhead on retail builds. Default: 0x8 or not set.",
            Tags = ["boot", "kernel", "debug", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Debug Print Filter"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Debug Print Filter", "DEFAULT", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Debug Print Filter", "DEFAULT")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Debug Print Filter", "DEFAULT", 0)],
        },
        new TweakDef
        {
            Id = "boot-winre-policy-allow",
            Label = "Allow Windows Recovery Environment Policy",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableWinRE=0 in WinRE policy to ensure the Windows Recovery Environment remains accessible. Prevents accidental policy lockout of recovery tools. Default: 0.",
            Tags = ["boot", "recovery", "winre", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinRE"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinRE", "DisableWinRE", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinRE", "DisableWinRE")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinRE", "DisableWinRE", 0)],
        },
        new TweakDef
        {
            Id = "boot-legacy-f8-menu",
            Label = "Enable Legacy F8 Boot Menu",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Runs 'bcdedit /set {bootmgr} displaybootmenu yes' to enable the legacy F8 boot menu. Allows access to safe mode and other startup options. Default: off on modern Windows.",
            Tags = ["boot", "bcd", "safe-mode", "f8"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = dryRun =>
            {
                Elevation.AssertAdmin(dryRun);
                Elevation.RunElevated("bcdedit", ["/set", "{bootmgr}", "displaybootmenu", "yes"]);
            },
            RemoveAction = dryRun =>
            {
                Elevation.AssertAdmin(dryRun);
                Elevation.RunElevated("bcdedit", ["/set", "{bootmgr}", "displaybootmenu", "no"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{bootmgr}"]);
                return stdout.Contains("displaybootmenu", StringComparison.OrdinalIgnoreCase)
                    && stdout.Contains("Yes", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-bcd-nx-optin",
            Label = "Set Data Execution Prevention to OptIn",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Runs 'bcdedit /set nx OptIn' to enable DEP (Data Execution Prevention) only for OS-protected processes. Balances security and compatibility. Default: OptIn.",
            Tags = ["boot", "bcd", "dep", "security"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = dryRun =>
            {
                Elevation.AssertAdmin(dryRun);
                Elevation.RunElevated("bcdedit", ["/set", "nx", "OptIn"]);
            },
            RemoveAction = dryRun =>
            {
                Elevation.AssertAdmin(dryRun);
                Elevation.RunElevated("bcdedit", ["/set", "nx", "OptIn"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{current}"]);
                return stdout.Contains("nx", StringComparison.OrdinalIgnoreCase) && stdout.Contains("OptIn", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-disable-startup-app-delay",
            Label = "Disable Startup App Launch Delay",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets StartupDelayInMSec=0 to eliminate the artificial delay Windows introduces before launching registered startup applications. Speeds up the post-login experience. Default: 10-second delay.",
            Tags = ["boot", "startup", "delay", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Serialize"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Serialize", "StartupDelayInMSec", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Serialize", "StartupDelayInMSec")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Serialize", "StartupDelayInMSec", 0),
            ],
        },
        new TweakDef
        {
            Id = "boot-disable-livedump",
            Label = "Disable Kernel Live Dump Collection",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Windows Kernel Live Dump collection (EnableLiveDump=0). Live dumps are taken by heuristics without a full crash; disabling reduces unexpected disk I/O and performance spikes. Default: enabled.",
            Tags = ["boot", "dump", "kernel", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "EnableLiveDump", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "EnableLiveDump", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "EnableLiveDump", 0)],
        },
        new TweakDef
        {
            Id = "boot-disable-crash-alert",
            Label = "Disable Admin Alert on System Crash",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the administrator alert notification (SendAlert=0) that Windows generates on a fatal system crash. Reduces noise in environments where crashes are monitored externally. Default: 0 (disabled by default on most builds).",
            Tags = ["boot", "crash", "alert", "admin"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "SendAlert", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "SendAlert")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "SendAlert", 0)],
        },
        new TweakDef
        {
            Id = "boot-overwrite-memory-dump",
            Label = "Always Overwrite Previous Memory Dump",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Forces Windows to always overwrite an existing memory dump file (Overwrite=1) instead of keeping multiple dump files. Prevents disk space accumulation from repeated crashes. Default: 1 (overwrite).",
            Tags = ["boot", "dump", "disk", "crash"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "Overwrite", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "Overwrite", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "Overwrite", 1)],
        },
        new TweakDef
        {
            Id = "boot-enable-nmi-crash-dump",
            Label = "Enable NMI-Triggered Crash Dump",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables triggering a crash dump via a Non-Maskable Interrupt (NMI) button or debugger. Useful for generating a dump on a completely hung system that cannot respond to other input. Default: disabled.",
            Tags = ["boot", "nmi", "dump", "debug"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "NMICrashDump", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "NMICrashDump", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "NMICrashDump", 1)],
        },
        new TweakDef
        {
            Id = "boot-disable-bsod-beep",
            Label = "Disable System Beep on BSOD",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the PC speaker beep that Windows emits when a BSOD (blue screen of death) occurs. Reduces noise in server rooms or overnight unattended machines. Default: 1 (beep enabled).",
            Tags = ["boot", "bsod", "beep", "crash"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "Beep", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "Beep", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "Beep", 0)],
        },
        new TweakDef
        {
            Id = "boot-disable-always-keep-dump",
            Label = "Do Not Permanently Keep Memory Dump",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets AlwaysKeepMemoryDump=0 so Windows does not permanently retain the memory dump even when low on disk. Lets the pagefile cleanup process remove the dump to free space. Default: 0.",
            Tags = ["boot", "dump", "disk", "cleanup"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "AlwaysKeepMemoryDump", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "AlwaysKeepMemoryDump")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "AlwaysKeepMemoryDump", 0)],
        },
        new TweakDef
        {
            Id = "boot-set-system-eventlog-size",
            Label = "Increase System Event Log Size to 50 MB",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the System event log maximum size to 50 MB (52428800 bytes). Allows retention of more historical system events before wrapping. Default: 20 MB.",
            Tags = ["boot", "event-log", "system", "size"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\EventLog\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\EventLog\System", "MaxSize", 52428800)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\EventLog\System", "MaxSize", 20971520)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\EventLog\System", "MaxSize", 52428800)],
        },
        new TweakDef
        {
            Id = "boot-disable-boot-status-display",
            Label = "Disable Boot Status / Spinner Display",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the display of boot status messages (spinner/dots) during startup by clearing DisplayStatusMessages. Produces a cleaner, faster-feeling boot sequence. Default: enabled.",
            Tags = ["boot", "ui", "spinner", "speed"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootStatus"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootStatus", "DisplayStatusMessages", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootStatus", "DisplayStatusMessages", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootStatus", "DisplayStatusMessages", 0)],
        },
        new TweakDef
        {
            Id = "boot-enable-lsa-ppl",
            Label = "Enable LSASS Protected Process Light (PPL)",
            Category = "Boot",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Runs the Local Security Authority Subsystem (lsass.exe) as a Protected Process Light (PPL) by setting RunAsPPL=1. Prevents non-authorised processes from reading LSASS memory (credential dumping). Requires UEFI Secure Boot. Default: disabled.",
            Tags = ["boot", "lsass", "security", "ppl", "credential-protection"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RunAsPPL", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RunAsPPL", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RunAsPPL", 1)],
        },
    ];
}
