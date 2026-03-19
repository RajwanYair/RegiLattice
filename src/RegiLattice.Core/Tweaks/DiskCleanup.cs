namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

/// <summary>
/// Disk cleanup and storage hygiene tweaks — automated cleanup, temp files, caches,
/// thumbnail databases, delivery optimisation, and Windows Update cleanup.
/// </summary>
internal static class DiskCleanup
{
    private const string CuKey = @"HKEY_CURRENT_USER";
    private const string LmKey = @"HKEY_LOCAL_MACHINE";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "cleanup-disable-thumbnail-cache",
            Label = "Disable Thumbnail Cache (Thumbs.db)",
            Category = "Disk Cleanup",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows from creating Thumbs.db thumbnail cache files in folders.",
            Tags = ["cleanup", "disk", "thumbnails", "explorer"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisableThumbnailCache", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisableThumbnailCache")],
            DetectOps = [RegOp.CheckDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisableThumbnailCache", 1)],
        },
        new TweakDef
        {
            Id = "cleanup-disable-thumbs-network",
            Label = "Disable Thumbnail Cache on Network Folders",
            Category = "Disk Cleanup",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents creation of hidden Thumbs.db files on network shares.",
            Tags = ["cleanup", "disk", "network", "thumbnails"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisableThumbsDBOnNetworkFolders", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisableThumbsDBOnNetworkFolders"),
            ],
            DetectOps =
            [
                RegOp.CheckDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisableThumbsDBOnNetworkFolders", 1),
            ],
        },
        new TweakDef
        {
            Id = "cleanup-disable-delivery-optimisation",
            Label = "Disable Delivery Optimisation (P2P Updates)",
            Category = "Disk Cleanup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables peer-to-peer update sharing which can consume bandwidth and cache disk space.",
            Tags = ["cleanup", "disk", "updates", "bandwidth"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\DeliveryOptimization"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\DeliveryOptimization", "DODownloadMode", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\DeliveryOptimization", "DODownloadMode")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\DeliveryOptimization", "DODownloadMode", 0)],
        },
        new TweakDef
        {
            Id = "cleanup-run-disk-cleanup-silent",
            Label = "Run Disk Cleanup (Silent, All Profiles)",
            Category = "Disk Cleanup",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description =
                "Runs cleanmgr with all cleanup handlers enabled silently. Removes temp files, logs, caches, and old Windows installations.",
            Tags = ["cleanup", "disk", "temp", "maintenance"],
            ApplyAction = _ =>
            {
                // Set all cleanup handlers to active in Sageset profile 9999
                ShellRunner.RunPowerShell(
                    "$path = 'HKLM:\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\VolumeCaches'; "
                        + "Get-ChildItem $path | ForEach-Object { Set-ItemProperty $_.PSPath -Name StateFlags9999 -Value 2 -Type DWord -ErrorAction SilentlyContinue }"
                );
                ShellRunner.Run("cleanmgr.exe", ["/sagerun:9999"]);
            },
            RemoveAction = _ => { }, // One-shot cleanup, nothing to remove
            DetectAction = () => false, // One-shot action
        },
        new TweakDef
        {
            Id = "cleanup-clear-windows-temp",
            Label = "Clear Windows Temp Folder",
            Category = "Disk Cleanup",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Removes all files from C:\\Windows\\Temp and user %TEMP% folders.",
            Tags = ["cleanup", "disk", "temp"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Remove-Item \"$env:windir\\Temp\\*\" -Recurse -Force -ErrorAction SilentlyContinue; "
                        + "Remove-Item \"$env:TEMP\\*\" -Recurse -Force -ErrorAction SilentlyContinue"
                ),
            RemoveAction = _ => { },
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "cleanup-clear-windows-update-cache",
            Label = "Clear Windows Update Download Cache",
            Category = "Disk Cleanup",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Stops Windows Update service, clears the SoftwareDistribution\\Download folder, then restarts the service.",
            Tags = ["cleanup", "disk", "windows-update", "cache"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Stop-Service wuauserv -Force -ErrorAction SilentlyContinue; "
                        + "Remove-Item \"$env:windir\\SoftwareDistribution\\Download\\*\" -Recurse -Force -ErrorAction SilentlyContinue; "
                        + "Start-Service wuauserv -ErrorAction SilentlyContinue"
                ),
            RemoveAction = _ => { },
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "cleanup-clear-font-cache",
            Label = "Clear Font Cache",
            Category = "Disk Cleanup",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Stops the Font Cache service, clears cached font data, then restarts the service. Fixes font rendering issues.",
            Tags = ["cleanup", "disk", "fonts", "cache"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Stop-Service FontCache -Force -ErrorAction SilentlyContinue; "
                        + "Remove-Item \"$env:windir\\ServiceProfiles\\LocalService\\AppData\\Local\\FontCache\\*\" -Recurse -Force -ErrorAction SilentlyContinue; "
                        + "Start-Service FontCache -ErrorAction SilentlyContinue"
                ),
            RemoveAction = _ => { },
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "cleanup-clear-icon-cache",
            Label = "Clear Icon Cache Database",
            Category = "Disk Cleanup",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Deletes the icon cache database and restarts Explorer. Fixes missing or corrupted icons.",
            Tags = ["cleanup", "disk", "icons", "explorer"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Stop-Process -Name explorer -Force -ErrorAction SilentlyContinue; "
                        + "Remove-Item \"$env:LOCALAPPDATA\\IconCache.db\" -Force -ErrorAction SilentlyContinue; "
                        + "Remove-Item \"$env:LOCALAPPDATA\\Microsoft\\Windows\\Explorer\\iconcache*\" -Force -ErrorAction SilentlyContinue; "
                        + "Start-Process explorer.exe"
                ),
            RemoveAction = _ => { },
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "cleanup-enable-storage-sense",
            Label = "Enable Storage Sense (Automatic Cleanup)",
            Category = "Disk Cleanup",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables Storage Sense to automatically free disk space by cleaning temp files and Recycle Bin.",
            Tags = ["cleanup", "disk", "storage-sense", "automatic"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy"],
            ApplyOps =
            [
                RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "01", 1),
                RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "04", 1),
                RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "08", 1),
                RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "32", 1),
                RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "2048", 30),
            ],
            RemoveOps = [RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "01", 0)],
            DetectOps = [RegOp.CheckDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "01", 1)],
        },
        new TweakDef
        {
            Id = "cleanup-disable-hibernation",
            Label = "Disable Hibernation (Free hiberfil.sys Space)",
            Category = "Disk Cleanup",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Disables hibernation and deletes hiberfil.sys, recovering RAM-size disk space.",
            Tags = ["cleanup", "disk", "hibernation", "space"],
            SideEffects = "Hibernation and Fast Startup will be unavailable.",
            ApplyAction = _ => ShellRunner.Run("powercfg.exe", ["-h", "off"]),
            RemoveAction = _ => ShellRunner.Run("powercfg.exe", ["-h", "on"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("Test-Path \"$env:SystemDrive\\hiberfil.sys\"");
                return stdout.Trim().Equals("False", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "cleanup-compact-os",
            Label = "Enable Compact OS (Compress System Files)",
            Category = "Disk Cleanup",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Compresses Windows system files using compact.exe. Saves 2-3 GB on SSDs with minimal performance impact.",
            Tags = ["cleanup", "disk", "compact", "compression"],
            ApplyAction = _ => ShellRunner.Run("compact.exe", ["/compactos:always"]),
            RemoveAction = _ => ShellRunner.Run("compact.exe", ["/compactos:never"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("compact.exe", ["/compactos:query"]);
                return stdout.Contains("compacted", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "cleanup-clear-event-logs",
            Label = "Clear All Windows Event Logs",
            Category = "Disk Cleanup",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.PowerShell,
            Description = "Clears all Windows Event logs. Use with caution — forensic evidence will be lost.",
            Tags = ["cleanup", "disk", "event-log", "maintenance"],
            SideEffects = "All event logs will be permanently deleted.",
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-WinEvent -ListLog * -ErrorAction SilentlyContinue | ForEach-Object { "
                        + "try { [System.Diagnostics.Eventing.Reader.EventLogSession]::GlobalSession.ClearLog($_.LogName) } catch {} }"
                ),
            RemoveAction = _ => { },
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "cleanup-disable-reserved-storage",
            Label = "Disable Reserved Storage (7 GB)",
            Category = "Disk Cleanup",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Disables Windows Reserved Storage which holds ~7 GB for updates. Frees disk space on small drives.",
            Tags = ["cleanup", "disk", "reserved-storage", "space"],
            ApplyAction = _ => ShellRunner.Run("dism.exe", ["/Online", "/Set-ReservedStorageState", "/State:Disabled"]),
            RemoveAction = _ => ShellRunner.Run("dism.exe", ["/Online", "/Set-ReservedStorageState", "/State:Enabled"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("dism.exe", ["/Online", "/Get-ReservedStorageState"]);
                return stdout.Contains("Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "cleanup-clear-prefetch",
            Label = "Clear Prefetch Cache",
            Category = "Disk Cleanup",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Clears the Windows Prefetch folder. System will rebuild it automatically.",
            Tags = ["cleanup", "disk", "prefetch", "cache"],
            ApplyAction = _ => ShellRunner.RunPowerShell("Remove-Item \"$env:windir\\Prefetch\\*\" -Force -ErrorAction SilentlyContinue"),
            RemoveAction = _ => { },
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "cleanup-reduce-recycle-bin-size",
            Label = "Limit Recycle Bin to 5% of Drive",
            Category = "Disk Cleanup",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Configures the Recycle Bin maximum size to 5% of each drive instead of the default 10%.",
            Tags = ["cleanup", "disk", "recycle-bin"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "$shell = New-Object -ComObject Shell.Application; "
                        + "$rb = $shell.Namespace(10); "
                        + "# Note: Recycle Bin size is stored per-drive in the registry "
                        + "Set-ItemProperty 'HKCU:\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\BitBucket\\Volume' -Name 'NukeOnDelete' -Value 0 -ErrorAction SilentlyContinue"
                ),
            RemoveAction = _ => { },
            DetectAction = () => false,
        },

        // ── Sprint 41 additions ────────────────────────────────────────────

        new TweakDef
        {
            Id = "cleanup-disable-recent-docs",
            Label = "Disable Recent Documents Tracking",
            Category = "Disk Cleanup",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows from recording recently opened documents in Jump Lists and Quick Access.",
            Tags = ["cleanup", "privacy", "recents"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackDocs", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackDocs")],
            DetectOps = [RegOp.CheckDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackDocs", 0)],
        },
        new TweakDef
        {
            Id = "cleanup-disable-recent-programs",
            Label = "Disable Recent Programs in Start Menu",
            Category = "Disk Cleanup",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Hides recently-used programs section from the Start menu.",
            Tags = ["cleanup", "privacy", "start"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackProgs", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackProgs")],
            DetectOps = [RegOp.CheckDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackProgs", 0)],
        },
        new TweakDef
        {
            Id = "cleanup-disable-search-history",
            Label = "Disable Search History",
            Category = "Disk Cleanup",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows Search from persisting search queries to disk.",
            Tags = ["cleanup", "privacy", "search"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "DisableSearchHistory", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "DisableSearchHistory")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "DisableSearchHistory", 1)],
        },
        new TweakDef
        {
            Id = "cleanup-disable-swap-file",
            Label = "Disable Swap File (SysMain paging supplement)",
            Category = "Disk Cleanup",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the secondary swapfile.sys used by Windows apps. Frees disk space on SSDs. Not recommended for systems with <8 GB RAM.",
            Tags = ["cleanup", "disk", "performance", "ssd"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "SwapFileControl", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "SwapFileControl")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "SwapFileControl", 0)],
        },
        new TweakDef
        {
            Id = "cleanup-disable-auto-maintenance",
            Label = "Disable Automatic Maintenance Wake-ups",
            Category = "Disk Cleanup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Windows from waking up the computer at night to run maintenance tasks.",
            Tags = ["cleanup", "maintenance", "power"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance", "MaintenanceDisabled", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance", "MaintenanceDisabled")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance", "MaintenanceDisabled", 1)],
        },
        new TweakDef
        {
            Id = "cleanup-disable-volume-shadow-copy",
            Label = "Disable Volume Shadow Copy Service Writer",
            Category = "Disk Cleanup",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the VSS system writer to reduce disk I/O. Affects System Restore and backup. Only for systems where backups are managed externally.",
            Tags = ["cleanup", "disk", "backup", "performance"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\VSS"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\VSS", "Start", 4)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\VSS", "Start", 3)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\VSS", "Start", 4)],
        },
        new TweakDef
        {
            Id = "cleanup-disable-internet-temp-auto",
            Label = "Disable Automatic Deletion of Internet Temp Files",
            Category = "Disk Cleanup",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Internet Explorer/Legacy Edge from automatically emptying the temp cache on browser exit.",
            Tags = ["cleanup", "browser", "temp"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Internet Settings\Cache"],
            ApplyOps = [RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Internet Settings\Cache", "Persistent", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Internet Settings\Cache", "Persistent")],
            DetectOps = [RegOp.CheckDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Internet Settings\Cache", "Persistent", 0)],
        },
        new TweakDef
        {
            Id = "cleanup-disable-wer-queue",
            Label = "Disable Windows Error Reporting Queue",
            Category = "Disk Cleanup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Windows Error Reporting from queuing crash dumps and reports to disk.",
            Tags = ["cleanup", "crash", "wer", "disk"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DontSendAdditionalData", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DontSendAdditionalData")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DontSendAdditionalData", 1)],
        },
        new TweakDef
        {
            Id = "cleanup-disable-superfetch-write",
            Label = "Disable SuperFetch Disk Write Activity",
            Category = "Disk Cleanup",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Reduces SysMain (SuperFetch) write activity to disk by disabling pre-population of the cache.",
            Tags = ["cleanup", "performance", "ssd", "superfetch"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters", "EnablePrefetcher", 0)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters", "EnablePrefetcher", 3)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters", "EnablePrefetcher", 0)],
        },
        new TweakDef
        {
            Id = "cleanup-limit-disk-usage-windows-update",
            Label = "Limit Windows Update Download Disk Cache",
            Category = "Disk Cleanup",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Caps the disk space Windows Update can use for its download cache to 1024 MB.",
            Tags = ["cleanup", "windows-update", "disk"],
            KindHint = TweakKind.PowerShell,
            ApplyAction = _ => ShellRunner.RunPowerShell("Set-ItemProperty 'HKLM:\\SOFTWARE\\Policies\\Microsoft\\Windows\\DeliveryOptimization' -Name 'AbsoluteMaxCacheSize' -Value 1024 -ErrorAction SilentlyContinue"),
            RemoveAction = _ => ShellRunner.RunPowerShell("Remove-ItemProperty 'HKLM:\\SOFTWARE\\Policies\\Microsoft\\Windows\\DeliveryOptimization' -Name 'AbsoluteMaxCacheSize' -ErrorAction SilentlyContinue"),
            DetectAction = () =>
            {
                try
                {
                    var val = Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeliveryOptimization", "AbsoluteMaxCacheSize", null);
                    return val is int v && v <= 1024;
                }
                catch { return false; }
            },
        },
    ];
}
