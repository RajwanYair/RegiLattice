namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class SystemTweaks
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "sys-enable-long-paths",
            Label = "Enable Win32 Long Paths",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Allows Win32 applications to use paths longer than 260 chars.",
            Tags = ["system", "filesystem", "long-paths"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
        },
        new TweakDef
        {
            Id = "sys-disable-reserved-storage",
            Label = "Disable Reserved Storage (~7 GB)",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Windows Reserved Storage which holds ~7 GB for updates.",
            Tags = ["system", "disk", "storage", "cleanup"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\ReserveManager"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\ReserveManager", "ShippedWithReserves", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\ReserveManager", "ShippedWithReserves", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\ReserveManager", "ShippedWithReserves", 0)],
        },
        new TweakDef
        {
            Id = "sys-disable-remote-assistance",
            Label = "Disable Remote Assistance",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Remote Assistance to reduce attack surface.",
            Tags = ["system", "security", "remote"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Remote Assistance"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Remote Assistance", "fAllowToGetHelp", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Remote Assistance", "fAllowToGetHelp", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Remote Assistance", "fAllowToGetHelp", 0)],
        },
        new TweakDef
        {
            Id = "sys-high-timer-resolution",
            Label = "Enable High Timer Resolution (Perf)",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Enables global high timer resolution (0.5ms) for improved scheduling accuracy. Benefits real-time audio, gaming, and latency-sensitive applications. Default: Disabled. Recommended: Enabled for gaming/audio.",
            Tags = ["system", "performance", "timer", "latency"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel", "GlobalTimerResolutionRequests", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel", "GlobalTimerResolutionRequests"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel", "GlobalTimerResolutionRequests", 1)],
        },
        new TweakDef
        {
            Id = "sys-disable-uac-dimming",
            Label = "Disable UAC Secure Desktop Dimming",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the dimmed secure desktop for UAC prompts. UAC still prompts but without screen dimming (faster). Default: Enabled. Recommended: Disabled for power users.",
            Tags = ["system", "uac", "ux"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "PromptOnSecureDesktop", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "PromptOnSecureDesktop", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "PromptOnSecureDesktop", 0)],
        },
        new TweakDef
        {
            Id = "sys-verbose-boot-status",
            Label = "Enable Verbose Boot Messages",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Shows detailed status messages during Windows startup and shutdown instead of the generic loading screen.",
            Tags = ["system", "boot", "diagnostics"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "VerboseStatus", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "VerboseStatus"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "VerboseStatus", 1)],
        },
        new TweakDef
        {
            Id = "sys-disable-autoplay",
            Label = "Disable AutoPlay",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables AutoPlay for all removable media and devices. Security best practice to avoid malware auto-execution.",
            Tags = ["system", "security", "autoplay", "usb"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers", "DisableAutoplay", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers", "DisableAutoplay", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers", "DisableAutoplay", 1)],
        },
        new TweakDef
        {
            Id = "sys-disable-activity-history",
            Label = "Disable Activity History",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Activity History, preventing Windows from collecting and uploading activity data (timeline, app usage).",
            Tags = ["system", "privacy", "activity-history", "timeline"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableActivityFeed", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "PublishUserActivities", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "UploadUserActivities", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableActivityFeed"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "PublishUserActivities"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "UploadUserActivities"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableActivityFeed", 0)],
        },
        new TweakDef
        {
            Id = "sys-disable-clipboard-history",
            Label = "Disable Clipboard History",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows Clipboard History (Win+V). Prevents clipboard content from being stored and synced.",
            Tags = ["system", "privacy", "clipboard"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "AllowClipboardHistory", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "AllowClipboardHistory"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "AllowClipboardHistory", 0)],
        },
        new TweakDef
        {
            Id = "sys-disable-admin-shares",
            Label = "Disable Administrative Shares (C$, ADMIN$)",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables default administrative shares (C$, ADMIN$). Reduces lateral-movement attack surface on workstations.",
            Tags = ["system", "security", "network", "shares"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "AutoShareWks", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "AutoShareWks"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "AutoShareWks", 0)],
        },
        new TweakDef
        {
            Id = "sys-system-disable-tips",
            Label = "Disable Windows Tips and Suggestions",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Windows tips, suggestions, and 'Get Even More Out of Windows' popups. Reduces distractions. Default: Enabled. Recommended: Disabled.",
            Tags = ["system", "tips", "suggestions", "nag"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SubscribedContent-338389Enabled", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SoftLandingEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SubscribedContent-338389Enabled"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SoftLandingEnabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SubscribedContent-338389Enabled", 0)],
        },
        new TweakDef
        {
            Id = "sys-system-disable-auto-maintenance",
            Label = "Disable Automatic Maintenance",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows automatic maintenance tasks (defrag, diagnostics, updates). Prevents unexpected disk and CPU usage. Default: Enabled. Recommended: Disabled for manual control.",
            Tags = ["system", "maintenance", "performance", "scheduled"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance", "MaintenanceDisabled", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance", "MaintenanceDisabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance", "MaintenanceDisabled", 1)],
        },
        new TweakDef
        {
            Id = "sys-disable-fast-startup",
            Label = "Disable Fast Startup (HiberBoot)",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Fast Startup (hybrid shutdown). Ensures clean boot every time. Default: Enabled. Recommended: Disabled.",
            Tags = ["system", "fast-startup", "hiberboot", "boot"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Power"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Power", "HiberbootEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Power", "HiberbootEnabled", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Power", "HiberbootEnabled", 0)],
        },
        new TweakDef
        {
            Id = "sys-disable-error-reporting",
            Label = "Disable Windows Error Reporting",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Windows Error Reporting via Group Policy. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["system", "error-reporting", "privacy", "wer"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "Disabled", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "Disabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "Disabled", 1)],
        },
        new TweakDef
        {
            Id = "sys-verbose-logon",
            Label = "Enable Verbose Logon Messages",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Shows detailed status messages during logon, logoff, startup, and shutdown (e.g., 'Applying Group Policy'). Default: disabled. Recommended: enabled for troubleshooting.",
            Tags = ["system", "verbose", "logon", "boot", "diagnostic"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
        },
        new TweakDef
        {
            Id = "sys-detailed-bsod",
            Label = "Enable Detailed Blue Screen Info",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Shows technical parameters on BSoD screens instead of just the QR code and sad-face. Useful for diagnosing crash causes. Default: disabled. Recommended: enabled.",
            Tags = ["system", "bsod", "crash", "diagnostic", "blue-screen"],
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
            Id = "sys-disable-wpbt",
            Label = "Disable WPBT (Vendor Bloatware Injection)",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Windows Platform Binary Table which allows vendors to inject software via UEFI firmware (e.g., Lenovo, HP bloatware). Default: enabled. Recommended: disabled.",
            Tags = ["system", "wpbt", "uefi", "bloatware", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "DisableWpbtExecution", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "DisableWpbtExecution"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "DisableWpbtExecution", 1)],
        },
        new TweakDef
        {
            Id = "sys-disable-storage-sense",
            Label = "Disable Storage Sense",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Storage Sense which automatically deletes temporary files, recycle bin contents, and Downloads folder items. Default: may be enabled. Recommended: disabled for control.",
            Tags = ["system", "storage-sense", "cleanup", "disk"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "01", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "01", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "01", 0)],
        },
        new TweakDef
        {
            Id = "sys-disable-auto-reboot",
            Label = "Disable Auto-Reboot After Crash",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Windows from automatically rebooting after a BSoD crash, giving time to read the error information. Default: auto-reboot. Recommended: disabled.",
            Tags = ["system", "crash", "reboot", "bsod", "diagnostic"],
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
            Id = "sys-restore-frequency",
            Label = "Enable Unlimited System Restore Frequency",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Sets RPSessionInterval to 0, allowing unlimited system restore point creation frequency. Default: 1. Recommended: 0 for frequent snapshots.",
            Tags = ["system", "restore", "backup", "frequency"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore", "RPSessionInterval", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore", "RPSessionInterval"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore", "RPSessionInterval", 0)],
        },
        new TweakDef
        {
            Id = "sys-disable-tips",
            Label = "Disable Windows Tips and Suggestions",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Windows tips, tricks, and suggestions popups. Default: Enabled. Recommended: Disabled.",
            Tags = ["system", "tips", "suggestions", "notifications", "nag"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SubscribedContent-338389Enabled", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SoftLandingEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SubscribedContent-338389Enabled"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SoftLandingEnabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SubscribedContent-338389Enabled", 0)],
        },
        new TweakDef
        {
            Id = "sys-disable-power-throttling",
            Label = "Disable Power Throttling",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Windows power throttling which limits background app performance. Default: Enabled. Recommended: Disabled for desktops.",
            Tags = ["system", "power", "throttling", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerThrottling"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerThrottling", "PowerThrottlingOff", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerThrottling", "PowerThrottlingOff"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerThrottling", "PowerThrottlingOff", 1)],
        },
        new TweakDef
        {
            Id = "sys-disable-hibernation",
            Label = "Disable Hibernation",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows hibernation, freeing disk space used by hiberfil.sys. Default: Enabled. Recommended: Disabled for desktops with SSDs.",
            Tags = ["system", "hibernation", "power", "disk", "ssd"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "HibernateEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "HibernateEnabled", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "HibernateEnabled", 0)],
        },
        new TweakDef
        {
            Id = "sys-disable-8dot3",
            Label = "Disable 8.3 Short Filename Creation",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables NTFS 8.3 short filename generation for improved file system performance. Default: Enabled. Recommended: Disabled on modern systems.",
            Tags = ["system", "ntfs", "8dot3", "filesystem", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisable8dot3NameCreation", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisable8dot3NameCreation", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisable8dot3NameCreation", 1)],
        },
        new TweakDef
        {
            Id = "sys-increase-crash-dump-verbosity",
            Label = "Set Crash Dump to Kernel Memory Dump",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets crash dump mode to Kernel Memory Dump (2). Provides better debugging info than Mini dump. Default: Automatic (7).",
            Tags = ["system", "crash", "dump", "debug"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "CrashDumpEnabled", 2)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "CrashDumpEnabled", 7)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "CrashDumpEnabled", 2)],
        },
        new TweakDef
        {
            Id = "sys-disable-autoplay-all-drives",
            Label = "Disable AutoPlay for All Drives",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables AutoPlay for all drive types (USB, CD, network). Prevents malware auto-execution. Default: enabled.",
            Tags = ["system", "autoplay", "security", "drives"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoDriveTypeAutoRun", 255)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoDriveTypeAutoRun")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoDriveTypeAutoRun", 255)],
        },
        new TweakDef
        {
            Id = "sys-enable-utc-hardware-clock",
            Label = "Set Hardware Clock to UTC",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the Windows hardware clock (RTC) to UTC instead of local time. Fixes time drift in dual-boot with Linux. Default: local.",
            Tags = ["system", "clock", "utc", "dual-boot"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\TimeZoneInformation"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\TimeZoneInformation", "RealTimeIsUniversal", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\TimeZoneInformation", "RealTimeIsUniversal")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\TimeZoneInformation", "RealTimeIsUniversal", 1)],
        },
        new TweakDef
        {
            Id = "sys-increase-irp-stack-size",
            Label = "Increase IRP Stack Size",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases the I/O Request Packet stack size to 32. Improves network file sharing performance. Default: 15.",
            Tags = ["system", "irp", "network", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "IRPStackSize", 32)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "IRPStackSize")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "IRPStackSize", 32)],
        },
        new TweakDef
        {
            Id = "sys-disable-error-reporting-queue",
            Label = "Disable Windows Error Reporting Queue",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the WER queue that stores crash reports before upload. Saves disk space and reduces background IO. Default: enabled.",
            Tags = ["system", "error-reporting", "wer", "queue"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DisableQueue", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DisableQueue")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DisableQueue", 1)],
        },
    ];
}
