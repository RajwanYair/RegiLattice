namespace RegiLattice.GUI.PackageManagers;

/// <summary>
/// Runs Windows system health and maintenance commands (DISM, SFC, disk cleanup, etc.).
/// All commands require administrator privileges and run with <see cref="ShellRunner"/>.
/// </summary>
internal static class WindowsHealthManager
{
    /// <summary>Describes a single health/maintenance command.</summary>
    internal sealed record HealthCommand(
        string Id,
        string Label,
        string Description,
        string FileName,
        string[] Args,
        bool NeedsAdmin = true,
        TimeSpan? Timeout = null
    );

    /// <summary>All available health commands, ordered by typical workflow.</summary>
    internal static IReadOnlyList<HealthCommand> Commands { get; } =
    [
        // ── DISM Component Store ────────────────────────────────────────
        new(
            "dism-analyze-store",
            "DISM: Analyze Component Store",
            "Analyzes the WinSxS component store size and reports whether cleanup is recommended.",
            "DISM.exe",
            ["/Online", "/Cleanup-Image", "/AnalyzeComponentStore"]
        ),
        new(
            "dism-cleanup-store",
            "DISM: Clean Component Store",
            "Removes superseded versions of components from the WinSxS store to reclaim disk space.",
            "DISM.exe",
            ["/Online", "/Cleanup-Image", "/StartComponentCleanup"],
            Timeout: TimeSpan.FromMinutes(10)
        ),
        new(
            "dism-reset-base",
            "DISM: Reset Base (Remove SP Backup)",
            "Removes all superseded service pack components. Cannot uninstall updates afterwards.",
            "DISM.exe",
            ["/Online", "/Cleanup-Image", "/StartComponentCleanup", "/ResetBase"],
            Timeout: TimeSpan.FromMinutes(15)
        ),
        // ── DISM Image Health ───────────────────────────────────────────
        new(
            "dism-check-health",
            "DISM: Check Health",
            "Performs a quick check for component store corruption markers.",
            "DISM.exe",
            ["/Online", "/Cleanup-Image", "/CheckHealth"]
        ),
        new(
            "dism-scan-health",
            "DISM: Scan Health (Deep)",
            "Performs a deep scan of the component store for corruption. May take several minutes.",
            "DISM.exe",
            ["/Online", "/Cleanup-Image", "/ScanHealth"],
            Timeout: TimeSpan.FromMinutes(15)
        ),
        new(
            "dism-restore-health",
            "DISM: Restore Health",
            "Repairs corrupted system files using Windows Update as a source. Requires internet.",
            "DISM.exe",
            ["/Online", "/Cleanup-Image", "/RestoreHealth"],
            Timeout: TimeSpan.FromMinutes(20)
        ),
        // ── System File Checker ─────────────────────────────────────────
        new(
            "sfc-scannow",
            "SFC: Scan and Repair System Files",
            "Scans all protected system files and replaces corrupted files with cached copies.",
            "sfc.exe",
            ["/scannow"],
            Timeout: TimeSpan.FromMinutes(15)
        ),
        new(
            "sfc-verify-only",
            "SFC: Verify Only (No Repair)",
            "Scans integrity of all protected system files without making repairs.",
            "sfc.exe",
            ["/verifyonly"],
            Timeout: TimeSpan.FromMinutes(15)
        ),
        // ── Disk Cleanup & Optimization ─────────────────────────────────
        new(
            "cleanmgr-sage",
            "Disk Cleanup: Run Silently",
            "Runs Windows Disk Cleanup with previously configured settings (sagerun:1).",
            "cleanmgr.exe",
            ["/sagerun:1"],
            Timeout: TimeSpan.FromMinutes(10)
        ),
        new(
            "defrag-analyze",
            "Defrag: Analyze C: Drive",
            "Analyzes fragmentation level on the system drive (skipped on SSDs by the OS).",
            "defrag.exe",
            ["C:", "/A"],
            Timeout: TimeSpan.FromMinutes(5)
        ),
        new(
            "defrag-optimize",
            "Defrag: Optimize C: Drive",
            "Runs TRIM on SSDs or defragmentation on HDDs for the system drive.",
            "defrag.exe",
            ["C:", "/O"],
            Timeout: TimeSpan.FromMinutes(15)
        ),
        // ── Network Diagnostics ─────────────────────────────────────────
        new(
            "ipconfig-flushdns",
            "Flush DNS Resolver Cache",
            "Purges the DNS client resolver cache, forcing fresh lookups for all domains.",
            "ipconfig.exe",
            ["/flushdns"],
            NeedsAdmin: false
        ),
        new(
            "netsh-reset-winsock",
            "Reset Winsock Catalog",
            "Resets the Winsock catalog to a clean state. Fixes corrupted network stacks. Requires reboot.",
            "netsh.exe",
            ["winsock", "reset"]
        ),
        new(
            "netsh-reset-ip",
            "Reset TCP/IP Stack",
            "Resets the TCP/IP stack to default. Fixes persistent network issues. Requires reboot.",
            "netsh.exe",
            ["int", "ip", "reset"]
        ),
        // ── Windows Update ──────────────────────────────────────────────
        new(
            "wu-scan",
            "Windows Update: Check for Updates",
            "Triggers a scan for available Windows updates via USOClient.",
            "USOClient.exe",
            ["StartScan"],
            Timeout: TimeSpan.FromMinutes(5)
        ),
        // ── Disk Health ─────────────────────────────────────────────────
        new(
            "chkdsk-scan",
            "CHKDSK: Scan C: (Read-Only)",
            "Scans the system drive for file system errors without making changes.",
            "chkdsk.exe",
            ["C:", "/scan"],
            Timeout: TimeSpan.FromMinutes(10)
        ),
        // ── Memory Diagnostics ──────────────────────────────────────────
        new(
            "mdsched-check",
            "Schedule Memory Diagnostic",
            "Schedules the Windows Memory Diagnostic to run on next reboot.",
            "MdSched.exe",
            [],
            NeedsAdmin: false
        ),
        // ── Power Efficiency ────────────────────────────────────────────
        new(
            "powercfg-batteryreport",
            "Generate Battery Report",
            "Creates a detailed HTML battery health report in %USERPROFILE%.",
            "powercfg.exe",
            ["/batteryreport", "/output", "%USERPROFILE%\\battery-report.html"],
            NeedsAdmin: false
        ),
        new(
            "powercfg-energy",
            "Power Efficiency Diagnostics",
            "Runs a 60-second power efficiency trace and generates an HTML report.",
            "powercfg.exe",
            ["/energy", "/output", "%USERPROFILE%\\energy-report.html"],
            Timeout: TimeSpan.FromMinutes(2)
        ),
    ];

    /// <summary>Run a health command and return (exitCode, combined output).</summary>
    internal static async Task<(int ExitCode, string Output)> RunCommandAsync(HealthCommand cmd, CancellationToken ct = default)
    {
        var timeout = cmd.Timeout ?? TimeSpan.FromMinutes(5);
        var (code, stdout, stderr) = await ShellRunner.RunAsync(cmd.FileName, cmd.Args, ct, timeout).ConfigureAwait(false);
        string output = string.IsNullOrWhiteSpace(stdout) ? stderr : stdout;
        return (code, output.Trim());
    }

    /// <summary>Check whether the current process is running as administrator.</summary>
    internal static bool IsAdmin()
    {
        using var identity = System.Security.Principal.WindowsIdentity.GetCurrent();
        var principal = new System.Security.Principal.WindowsPrincipal(identity);
        return principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
    }
}
