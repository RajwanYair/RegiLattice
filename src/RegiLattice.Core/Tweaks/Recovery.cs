namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Recovery
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "recovery-disable-auto-repair-prompt",
            Label = "Disable Automatic Repair at Boot",
            Category = "Storage — Work Folders",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the automatic startup repair prompt after consecutive boot failures. Prevents boot loops on servers. Default: enabled.",
            Tags = ["recovery", "auto-repair", "boot", "server"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "BootExecuteAutoRecovery", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "BootExecuteAutoRecovery")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "BootExecuteAutoRecovery", 0)],
        },
        new TweakDef
        {
            Id = "recovery-disable-winre-partition",
            Label = "Disable Windows Recovery Environment",
            Category = "Storage — Work Folders",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Windows Recovery Environment (WinRE). Saves ~500 MB disk space but removes recovery boot option. Default: enabled.",
            Tags = ["recovery", "winre", "disable", "disk-space"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager", "WinREEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager", "WinREEnabled", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager", "WinREEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "recovery-enable-last-known-good",
            Label = "Enable Last Known Good Configuration",
            Category = "Storage — Work Folders",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the system to report the last known good configuration to the boot manager. Helps recovery from bad driver installs. Default: system-managed.",
            Tags = ["recovery", "boot", "last-known-good", "driver"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager", "LastKnownGood", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager", "LastKnownGood"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager", "LastKnownGood", 1),
            ],
        },
        new TweakDef
        {
            Id = "recovery-disable-recovery-ui",
            Label = "Disable Windows Recovery UI",
            Category = "Storage — Work Folders",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the recovery UI displayed after consecutive boot failures. Use only on kiosk or server systems where manual intervention is not desired. Default: enabled.",
            Tags = ["recovery", "boot", "ui", "kiosk", "server"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "DisplayDisabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "DisplayDisabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "DisplayDisabled", 1)],
        },
        new TweakDef
        {
            Id = "recovery-disable-crash-auto-reboot-timeout",
            Label = "Set Crash Reboot Timeout to 30s",
            Category = "Storage — Work Folders",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the timeout before automatic reboot after a crash to 30 seconds, giving time to read BSOD information. Default: immediate reboot.",
            Tags = ["recovery", "crash", "bsod", "timeout", "reboot"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "AutoRebootTimeout", 30)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "AutoRebootTimeout")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "AutoRebootTimeout", 30)],
        },
        new TweakDef
        {
            Id = "recovery-disable-automatic-managed-page-file",
            Label = "Disable Auto-Managed Page File",
            Category = "Storage — Work Folders",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables automatic page file management. Allows manual control over page file size for advanced crash dump configuration. Default: auto-managed.",
            Tags = ["recovery", "page-file", "memory", "crash-dump", "advanced"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "PagingFiles", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "PagingFiles")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "PagingFiles", 0),
            ],
        },
        // ── Sprint 18 — 10 new Recovery tweaks ────────────────────────────

        new TweakDef
        {
            Id = "recovery-enable-boot-logging",
            Label = "Enable Boot Logging (ntbtlog.txt)",
            Category = "Storage — Work Folders",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables Windows boot logging to %SystemRoot%\\ntbtlog.txt. Useful for diagnosing driver loading failures. Default: disabled.",
            Tags = ["recovery", "boot", "logging", "driver", "diagnostic"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "BootLog", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "BootLog")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "BootLog", 1)],
        },
        new TweakDef
        {
            Id = "recovery-disable-winre-auto-repair",
            Label = "Disable Automatic Repair (Windows RE)",
            Category = "Storage — Work Folders",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables automatic startup repair in Windows Recovery Environment. Prevents boot loops when auto-repair repeatedly fails. Default: enabled.",
            Tags = ["recovery", "winre", "auto-repair", "boot-loop", "startup"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "BootExecuteAutoRepair", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "BootExecuteAutoRepair")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "BootExecuteAutoRepair", 0)],
        },
        new TweakDef
        {
            Id = "recovery-set-dump-folder-path",
            Label = "Set Minidump Folder to C:\\Minidumps",
            Category = "Storage — Work Folders",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Redirects minidump files to C:\\Minidumps for easier collection and analysis. Default: %SystemRoot%\\Minidump.",
            Tags = ["recovery", "minidump", "folder", "path", "organisation"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetExpandString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "MinidumpDir", @"C:\Minidumps")],
            RemoveOps =
            [
                RegOp.SetExpandString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "MinidumpDir", @"%SystemRoot%\Minidump"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "MinidumpDir", @"C:\Minidumps")],
        },
        new TweakDef
        {
            Id = "recovery-disable-startup-repair-prompt",
            Label = "Disable Startup Repair Recommendation Prompt",
            Category = "Storage — Work Folders",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Suppresses the 'Your PC did not start correctly' startup repair recommendation. Default: prompt shown after improper shutdown.",
            Tags = ["recovery", "startup-repair", "prompt", "boot", "ux"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "AutoChkTimeout", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "AutoChkTimeout")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "AutoChkTimeout", 0)],
        },
    ];
}
