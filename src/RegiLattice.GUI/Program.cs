using System.Diagnostics;
using System.Runtime.InteropServices;
using RegiLattice.Core;
using RegiLattice.GUI.Forms;

namespace RegiLattice.GUI;

internal static class Program
{
    private static readonly Stopwatch s_uptime = Stopwatch.StartNew();

    [STAThread]
    static void Main(string[] args)
    {
        // ── Global exception handlers ───────────────────────────────────────
        // Ensure any unhandled exception produces a visible error dialog rather
        // than silently terminating the process (which produces "nothing shows").
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        Application.ThreadException += static (_, e) => ShowFatalError(e.Exception);
        AppDomain.CurrentDomain.UnhandledException += static (_, e) => ShowFatalError(e.ExceptionObject as Exception);

        ApplicationConfiguration.Initialize();

        try
        {
            Form? managerForm = ResolveManagerArg(args);
            if (managerForm is not null)
            {
                AppTheme.Apply(managerForm);
                Application.Run(managerForm);
                return;
            }

            Application.Run(new MainForm());
        }
        catch (Exception ex)
        {
            ShowFatalError(ex);
        }
    }

    /// <summary>
    /// Shows a fatal startup error as a MessageBox and writes a crash log to
    /// <c>%LOCALAPPDATA%\RegiLattice\crash.log</c> for later diagnosis.
    /// </summary>
    [DllImport("user32.dll")]
    private static extern int GetGuiResources(IntPtr hProcess, int uiFlags);

    private static void ShowFatalError(Exception? ex)
    {
        string msg = BuildDetailedCrashReport(ex);

        // Write crash log before showing the dialog so the file exists even if
        // the MessageBox itself hits a secondary exception.
        try
        {
            string logDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "RegiLattice");
            Directory.CreateDirectory(logDir);
            string logPath = Path.Combine(logDir, "crash.log");
            // Append to preserve prior crashes in the same session
            File.AppendAllText(logPath, msg + "\n\n");
        }
        catch
        { /* last resort; don't recurse */
        }

        try
        {
            // Show a short summary in the dialog (full details in crash.log)
            string shortMsg = ex is null ? "An unexpected error occurred." : $"{ex.GetType().Name}: {ex.Message}";

            MessageBox.Show(
                $"RegiLattice encountered a fatal error.\n\n{shortMsg}\n\n"
                    + "A detailed crash log has been written to:\n"
                    + "%LOCALAPPDATA%\\RegiLattice\\crash.log",
                "RegiLattice — Fatal Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }
        catch
        { /* absolute last resort */
        }
    }

    /// <summary>
    /// Builds a detailed crash report string including GDI/USER handle counts,
    /// memory stats, and the full exception chain. Overload for callers that
    /// have a pre-formatted detail string instead of an exception object.
    /// </summary>
    internal static string BuildDetailedCrashReport(string details)
    {
        var sb = new System.Text.StringBuilder(2048);
        sb.AppendLine($"═══ RegiLattice Crash Report ═══");
        sb.AppendLine($"Timestamp : {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
        sb.AppendLine($"Uptime    : {s_uptime.Elapsed:hh\\:mm\\:ss\\.fff}");
        sb.AppendLine($"Version   : {typeof(TweakEngine).Assembly.GetName().Version}");
        sb.AppendLine($"OS        : {Environment.OSVersion} ({RuntimeInformation.OSArchitecture})");
        sb.AppendLine($"CLR       : {RuntimeInformation.FrameworkDescription}");
        sb.AppendLine($"PID       : {Environment.ProcessId}");
        AppendHandleStats(sb);
        sb.AppendLine();
        sb.AppendLine(details);
        sb.AppendLine("═══ End Crash Report ═══");
        return sb.ToString();
    }

    private static string BuildDetailedCrashReport(Exception? ex)
    {
        var sb = new System.Text.StringBuilder(2048);
        sb.AppendLine($"═══ RegiLattice Crash Report ═══");
        sb.AppendLine($"Timestamp : {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
        sb.AppendLine($"Uptime    : {s_uptime.Elapsed:hh\\:mm\\:ss\\.fff}");
        sb.AppendLine($"Version   : {typeof(TweakEngine).Assembly.GetName().Version}");
        sb.AppendLine($"OS        : {Environment.OSVersion} ({RuntimeInformation.OSArchitecture})");
        sb.AppendLine($"CLR       : {RuntimeInformation.FrameworkDescription}");
        sb.AppendLine($"PID       : {Environment.ProcessId}");
        AppendHandleStats(sb);

        sb.AppendLine();

        if (ex is null)
        {
            sb.AppendLine("Exception: (null — unknown error)");
        }
        else
        {
            // Walk the full exception chain
            int depth = 0;
            Exception? current = ex;
            while (current is not null && depth < 10)
            {
                string prefix = depth == 0 ? "Exception" : $"InnerException[{depth}]";
                sb.AppendLine($"{prefix}: {current.GetType().FullName}");
                sb.AppendLine($"  Message: {current.Message}");
                sb.AppendLine($"  HResult: 0x{current.HResult:X8}");
                if (current is System.ComponentModel.Win32Exception w32)
                    sb.AppendLine($"  NativeErrorCode: {w32.NativeErrorCode} (0x{w32.NativeErrorCode:X8})");
                sb.AppendLine($"  StackTrace:");
                sb.AppendLine(current.StackTrace ?? "    (none)");
                sb.AppendLine();
                current = current.InnerException;
                depth++;
            }
        }

        sb.AppendLine("═══ End Crash Report ═══");
        return sb.ToString();
    }

    private static void AppendHandleStats(System.Text.StringBuilder sb)
    {
        // GDI / USER handle counts — critical for diagnosing handle exhaustion
        try
        {
            using var proc = Process.GetCurrentProcess();
            int gdiHandles = GetGuiResources(proc.Handle, 0);  // GR_GDIOBJECTS
            int userHandles = GetGuiResources(proc.Handle, 1); // GR_USEROBJECTS
            int gdiPeak = GetGuiResources(proc.Handle, 2);     // GR_GDIOBJECTS_PEAK
            int userPeak = GetGuiResources(proc.Handle, 4);    // GR_USEROBJECTS_PEAK
            sb.AppendLine($"GDI handles  : {gdiHandles:N0} (peak: {gdiPeak:N0}) [limit: 10,000]");
            sb.AppendLine($"USER handles : {userHandles:N0} (peak: {userPeak:N0}) [limit: 10,000]");
            sb.AppendLine($"Total handles: {proc.HandleCount:N0}");
            sb.AppendLine($"Threads      : {proc.Threads.Count}");
            sb.AppendLine($"WorkingSet   : {proc.WorkingSet64 / (1024 * 1024):N0} MB");
            sb.AppendLine($"PrivateBytes : {proc.PrivateMemorySize64 / (1024 * 1024):N0} MB");
            sb.AppendLine($"GC Heap      : {GC.GetTotalMemory(false) / (1024 * 1024):N0} MB");
        }
        catch
        {
            sb.AppendLine("(handle/memory stats unavailable)");
        }

        // Open forms and total control count — helps identify control/handle leaks
        try
        {
            int formCount = Application.OpenForms.Count;
            int controlCount = 0;
            foreach (Form f in Application.OpenForms)
                controlCount += CountControls(f);
            sb.AppendLine($"Open forms   : {formCount}");
            sb.AppendLine($"Total ctrls  : {controlCount:N0}");
        }
        catch
        {
            sb.AppendLine("(form/control stats unavailable)");
        }
    }

    private static int CountControls(Control c)
    {
        int count = 1;
        foreach (Control child in c.Controls)
            count += CountControls(child);
        return count;
    }

    /// <summary>
    /// Parses --manager &lt;name&gt; (or --tool &lt;name&gt;) and returns the matching dialog,
    /// or null for normal main-window launch.
    /// Supported names: scoop | psmodule | pip | winget | chocolatey |
    ///                  toolversions | windowshealth | network | startup |
    ///                  service | scheduledtask | powerplan | privacy |    ///                  contextmenu | hostsfile | tempcleaner | installedapps |
    ///                  memorycleaner | diskspace | portscan | batteryhealth |
    ///                  marketplace | preferences | whatsnew
    /// </summary>
    private static Form? ResolveManagerArg(string[] args)
    {
        for (int i = 0; i < args.Length - 1; i++)
        {
            bool isFlag =
                args[i].Equals("--manager", StringComparison.OrdinalIgnoreCase) || args[i].Equals("--tool", StringComparison.OrdinalIgnoreCase);
            if (!isFlag)
                continue;

            return args[i + 1].ToLowerInvariant() switch
            {
                // Package managers
                "scoop" => new ScoopManagerDialog(),
                "psmodule" => new PSModuleManagerDialog(),
                "pip" => new PipManagerDialog(),
                "winget" => new WinGetManagerDialog(),
                "chocolatey" => new ChocolateyManagerDialog(),

                // System tools
                "toolversions" => new ToolVersionsDialog(),
                "windowshealth" => new WindowsHealthDialog(),
                "network" => new NetworkToolsDialog(),
                "startup" => new StartupManagerDialog(),
                "service" => new ServiceManagerDialog(),
                "scheduledtask" => new ScheduledTaskManagerDialog(),
                "powerplan" => new PowerPlanDialog(),
                "privacy" => new PrivacyDashboardDialog(),

                // System utilities
                "contextmenu" => new ContextMenuManagerDialog(),
                "hostsfile" => new HostsFileManagerDialog(),
                "tempcleaner" => new TempFileCleanerDialog(),
                "installedapps" => new InstalledAppsDialog(),

                // Power & energy (Sprint 31)
                "powerscheduler" => new PowerSchedulerDialog(),
                "sleeptimer" => new SleepTimerDialog(),
                "batterysaver" => new BatterySaverDialog(),
                "usbpower" => new UsbPowerDialog(),

                // Privacy & ad removal (Sprint 32)
                "adremoval" => new AdRemovalWizardDialog(),
                "telemetry" => new TelemetryDashboardDialog(),
                "apppermissions" => new AppPermissionsDialog(),
                "dnsoverhttps" => new DnsOverHttpsDialog(),

                // Network tools (Sprint 33)
                "netrepair" => new NetworkRepairDialog(),
                "dnsswitcher" => new DnsSwitcherDialog(),
                "netadapter" => new NetworkAdapterDialog(),
                "wifiprofiles" => new WiFiProfileDialog(),

                // Security & system tools (Sprint 34)
                "firewallrules" => new FirewallRulesDialog(),
                "proxyconfig" => new ProxyConfigDialog(),
                "shellextensions" => new ShellExtensionDialog(),
                "bootanalyzer" => new BootTimeAnalyzerDialog(),
                "wucontrol" => new WindowsUpdateControlDialog(),
                "notifmgr" => new NotificationManagerDialog(),
                "browsercache" => new BrowserCacheCleanerDialog(),
                "driverchecker" => new DriverUpdateCheckerDialog(),
                "wakeonlan" => new WakeOnLanDialog(),
                "brightness" => new BrightnessSchedulerDialog(),

                // System monitor tools (Sprint 41)
                "memorycleaner" => new MemoryCleanerDialog(),
                "diskspace" => new DiskSpaceDialog(),
                "portscan" => new PortScannerDialog(),
                "batteryhealth" => new BatteryHealthDialog(),

                // Hardware & network monitors (Sprint 42)
                "hwtempmon" => new HardwareTemperatureDialog(),
                "netbandwidth" => new NetworkBandwidthDialog(),
                "macaddress" => new MacAddressDialog(),

                // Other dialogs
                "marketplace" => new MarketplaceDialog(),
                "preferences" => CreateStandalonePreferences(),
                "whatsnew" => new WhatsNewDialog(),

                _ => null,
            };
        }

        return null;
    }

    private static Form CreateStandalonePreferences()
    {
        var cfg = AppConfig.Load();
        return new PreferencesDialog(cfg);
    }
}
