namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Boot
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "boot-disable-secboot-check",
            Label = "Suppress Secure Boot Status Check",
            Category = "System 2",
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
            Category = "System 2",
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
            Category = "System 2",
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
            Category = "System 2",
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
            Id = "boot-clear-pagefile",
            Label = "Clear Pagefile at Shutdown",
            Category = "System 2",
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
            Id = "boot-disable-boot-ux",
            Label = "Disable Boot UI Animation",
            Category = "System 2",
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
            Category = "System 2",
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
            Category = "System 2",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Shows detailed status messages during boot, shutdown, logon, and logoff. Default: hidden.",
            Tags = ["boot", "verbose", "status", "messages"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "VerboseStatus", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "VerboseStatus")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "VerboseStatus", 1)],
        },
        // ── Command-based boot tweaks (bcdedit) ────────────────────────────
        new TweakDef
        {
            Id = "boot-bcd-quiet-boot",
            Label = "Enable Quiet Boot (bcdedit)",
            Category = "System 2",
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
            Category = "System 2",
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
            Category = "System 2",
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
            Category = "System 2",
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
            Category = "System 3",
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
            Category = "System 3",
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
            Category = "System 3",
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
            Id = "boot-disable-logo",
            Label = "Disable OEM Boot Logo",
            Category = "System 3",
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
            Category = "System 3",
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
            Category = "System 3",
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
            Id = "boot-log",
            Label = "Enable Boot Logging (ntbtlog.txt)",
            Category = "System 3",
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
            Category = "System 3",
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
            Category = "System 3",
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
            Category = "System 3",
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
            Category = "System 3",
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
            Category = "System 3",
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
            Category = "System 3",
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
            Category = "System 3",
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
            Category = "System 3",
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
            Category = "System 3",
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
            Category = "System 3",
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
            Category = "System 3",
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
            Category = "System 3",
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
            Category = "System 3",
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
            Category = "System 3",
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
            Category = "System 3",
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
            Id = "boot-enable-nmi-crash-dump",
            Label = "Enable NMI-Triggered Crash Dump",
            Category = "System 3",
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
            Category = "System 3",
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
            Category = "System 3",
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
            Category = "System 3",
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
            Category = "System 3",
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
        // ── merged from: Services.cs ──────────────────────────────────────────────────
        new TweakDef
        {
            Id = "svc-disable-sysmain-service",
            Label = "Disable SysMain (Superfetch)",
            Category = "System 3",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the SysMain (Superfetch) service — beneficial on SSD systems.",
            Tags = ["services", "performance", "ssd"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SysMain"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SysMain", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SysMain", "Start", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SysMain", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-diagsvc",
            Label = "Disable Diagnostic Service",
            Category = "System 3",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Diagnostic Service (DiagSvc) that runs troubleshooters.",
            Tags = ["services", "telemetry", "diagnostics"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DiagSvc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DiagSvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DiagSvc", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DiagSvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-wbiosrvc",
            Label = "Disable Biometric Service",
            Category = "System 3",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows Biometric Service (WbioSrvc). Useful if fingerprint/face login is not used.",
            Tags = ["services", "biometric", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WbioSrvc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WbioSrvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WbioSrvc", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WbioSrvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-remote-registry",
            Label = "Disable Remote Registry",
            Category = "System 3",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Remote Registry service which allows remote access to the Windows registry. Security hardening measure.",
            Tags = ["services", "security", "remote"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\RemoteRegistry"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\RemoteRegistry", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\RemoteRegistry", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\RemoteRegistry", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-geolocation-service",
            Label = "Disable Geolocation Service",
            Category = "System 3",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Windows Geolocation Service for privacy.",
            Tags = ["services", "privacy", "location"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\lfsvc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\lfsvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\lfsvc", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\lfsvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-delivery-optimization-svc",
            Label = "Disable Delivery Optimization",
            Category = "System 3",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Delivery Optimization service which shares Windows Update data with other PCs on LAN and internet.",
            Tags = ["services", "update", "bandwidth", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DoSvc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DoSvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DoSvc", "Start", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DoSvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-fax",
            Label = "Disable Fax Service (Cleanup)",
            Category = "System 3",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the legacy Fax service to free resources. Default: Manual. Recommended: Disabled.",
            Tags = ["services", "fax", "legacy", "cleanup"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Fax"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Fax", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Fax", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Fax", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-smartcard",
            Label = "Disable Smart Card Service",
            Category = "System 3",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Smart Card service (SCardSvr) for smart-card readers. Safe to disable if no smart cards are used. Default: Manual. Recommended: Disabled.",
            Tags = ["services", "smartcard", "scardsvr", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SCardSvr"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SCardSvr", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SCardSvr", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SCardSvr", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-link-tracking",
            Label = "Disable Distributed Link Tracking Client",
            Category = "System 3",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Distributed Link Tracking Client (TrkWks) that maintains NTFS file links across networked computers. Default: Manual. Recommended: Disabled for standalone PCs.",
            Tags = ["services", "link-tracking", "trkwks", "ntfs"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\TrkWks"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\TrkWks", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\TrkWks", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\TrkWks", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-wallet",
            Label = "Disable Wallet Service",
            Category = "System 3",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Windows Wallet Service used for NFC-based payments. Safe to disable if contactless payments are unused. Default: Manual. Recommended: Disabled.",
            Tags = ["services", "wallet", "nfc", "payment"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WalletService"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WalletService", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WalletService", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WalletService", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-secondary-logon",
            Label = "Disable Secondary Logon Service",
            Category = "System 3",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Secondary Logon (RunAs) service. Reduces privilege escalation surface. Default: manual.",
            Tags = ["services", "secondary-logon", "runas", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\seclogon"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\seclogon", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\seclogon", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\seclogon", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-xbox-live-networking",
            Label = "Disable Xbox Live Networking Service",
            Category = "System 3",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Xbox Live Networking service. Not needed if you don't use Xbox features. Default: manual.",
            Tags = ["services", "xbox", "networking", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\XboxNetApiSvc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\XboxNetApiSvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\XboxNetApiSvc", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\XboxNetApiSvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-webclient",
            Label = "Disable WebClient (WebDAV) Service",
            Category = "System 3",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the WebClient service (WebDAV). Reduces attack surface for NTLM relay. Default: manual.",
            Tags = ["services", "webclient", "webdav", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WebClient"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WebClient", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WebClient", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WebClient", "Start", 4)],
        },
        // ── Command-based service tweaks (sc.exe) ──────────────────────────
        new TweakDef
        {
            Id = "svc-stop-xbox-services",
            Label = "Stop & Disable All Xbox Services (sc)",
            Category = "System 3",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Stops and disables all Xbox-related services (XblAuthManager, XblGameSave, XboxGipSvc, XboxNetApiSvc) to free resources.",
            Tags = ["services", "xbox", "disable", "gaming", "resources"],
            KindHint = TweakKind.ServiceControl,
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                foreach (var svc in new[] { "XblAuthManager", "XblGameSave", "XboxGipSvc", "XboxNetApiSvc" })
                {
                    Elevation.RunElevated("sc", ["stop", svc]);
                    Elevation.RunElevated("sc", ["config", svc, "start=", "disabled"]);
                }
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                foreach (var svc in new[] { "XblAuthManager", "XblGameSave", "XboxGipSvc", "XboxNetApiSvc" })
                {
                    Elevation.RunElevated("sc", ["config", svc, "start=", "demand"]);
                }
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("sc", ["qc", "XblAuthManager"]);
                return stdout.Contains("DISABLED", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "svc-stop-connected-devices",
            Label = "Stop & Disable Connected Devices Platform Service (sc)",
            Category = "System 3",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Connected Devices Platform (CDP) service used for cross-device experiences, Timeline, and nearby sharing.",
            Tags = ["services", "cdp", "connected", "devices", "cross-device"],
            KindHint = TweakKind.ServiceControl,
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("sc", ["stop", "CDPSvc"]);
                Elevation.RunElevated("sc", ["config", "CDPSvc", "start=", "disabled"]);
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("sc", ["config", "CDPSvc", "start=", "auto"]);
                Elevation.RunElevated("sc", ["start", "CDPSvc"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("sc", ["qc", "CDPSvc"]);
                return stdout.Contains("DISABLED", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "svc-disable-upnphost",
            Label = "Disable UPnP Device Host Service",
            Category = "System 3",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the UPnP Device Host service. Prevents this machine from acting as a discoverable UPnP host, reducing the attack surface on untrusted networks.",
            Tags = ["services", "upnp", "network", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\upnphost"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\upnphost", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\upnphost", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\upnphost", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-fdphost",
            Label = "Disable Function Discovery Provider Host",
            Category = "System 3",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the fdPHost service. Stops Windows from using WS-Discovery and other protocols to automatically find networked printers and devices.",
            Tags = ["services", "fdphost", "discovery", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\fdPHost"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\fdPHost", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\fdPHost", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\fdPHost", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-fdrespub",
            Label = "Disable Function Discovery Resource Publication Service",
            Category = "System 3",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the FDResPub service. Prevents this machine from advertising itself on the local network via WS-Discovery, removing it from the Network neighbourhood of other PCs.",
            Tags = ["services", "fdrespub", "publication", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\FDResPub"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\FDResPub", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\FDResPub", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\FDResPub", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-icssvc",
            Label = "Disable Internet Connection Sharing (ICS)",
            Category = "System 3",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the SharedAccess (ICS) service. Removes Windows' built-in NAT router capability, preventing accidental or unauthorised sharing of the internet connection.",
            Tags = ["services", "ics", "sharing", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-mapbroker",
            Label = "Disable Downloaded Maps Manager Service",
            Category = "System 3",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the MapsBroker service. Stops Windows from periodically downloading offline map data updates in the background.",
            Tags = ["services", "maps", "offline", "bandwidth"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\MapsBroker"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\MapsBroker", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\MapsBroker", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\MapsBroker", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-remoteaccess",
            Label = "Disable Routing and Remote Access (RRAS) Service",
            Category = "System 3",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the RemoteAccess (RRAS) service. Stops Windows from acting as a software router/VPN server. Not needed on standard workstations.",
            Tags = ["services", "routing", "vpn", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\RemoteAccess"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\RemoteAccess", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\RemoteAccess", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\RemoteAccess", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-wisvc",
            Label = "Disable Windows Insider Service",
            Category = "System 3",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the wisvc (Windows Insider Service). Prevents the device from being enrolled in Windows Insider preview flight deliveries and associated telemetry collection.",
            Tags = ["services", "insider", "preview", "telemetry"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\wisvc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\wisvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\wisvc", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\wisvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-autotimesvc",
            Label = "Disable Cellular Time Synchronisation Service",
            Category = "System 3",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the autotimesvc (Cellular Time) service. This service syncs the system clock via mobile-broadband data — not needed on non-cellular or always-connected PCs.",
            Tags = ["services", "time", "cellular", "sync"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\autotimesvc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\autotimesvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\autotimesvc", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\autotimesvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-napagent",
            Label = "Disable Network Access Protection (NAP) Agent",
            Category = "System 3",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the napagent (Network Access Protection Agent) service. NAP is deprecated since Windows Server 2012 R2 and the agent is unused on modern workstations.",
            Tags = ["services", "nap", "legacy", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\napagent"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\napagent", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\napagent", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\napagent", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-tzautoupdate",
            Label = "Disable Automatic Time Zone Updater Service",
            Category = "System 3",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the tzautoupdate service. Prevents Windows from automatically adjusting the system time zone based on location data. Useful for servers and VMs where the time zone should be fixed.",
            Tags = ["services", "timezone", "automatic", "location"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\tzautoupdate"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\tzautoupdate", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\tzautoupdate", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\tzautoupdate", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-diagnosticshub",
            Label = "Disable Diagnostics Hub Standard Collector Service",
            Category = "System 3",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the diagnosticshub.standardcollector.service. This service collects real-time diagnostic events from ETW providers for Visual Studio profiling sessions — not needed outside of profiling.",
            Tags = ["services", "diagnostics", "etw", "profiling"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\diagnosticshub.standardcollector.service"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\diagnosticshub.standardcollector.service", "Start", 4)],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\diagnosticshub.standardcollector.service", "Start", 3),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\diagnosticshub.standardcollector.service", "Start", 4),
            ],
        },
    ];
}
