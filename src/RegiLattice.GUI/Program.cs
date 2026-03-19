using RegiLattice.Core;
using RegiLattice.GUI.Forms;

namespace RegiLattice.GUI;

internal static class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        ApplicationConfiguration.Initialize();

        Form? managerForm = ResolveManagerArg(args);
        if (managerForm is not null)
        {
            AppTheme.Apply(managerForm);
            Application.Run(managerForm);
            return;
        }

        Application.Run(new MainForm());
    }

    /// <summary>
    /// Parses --manager &lt;name&gt; (or --tool &lt;name&gt;) and returns the matching dialog,
    /// or null for normal main-window launch.
    /// Supported names: scoop | psmodule | pip | winget | chocolatey |
    ///                  toolversions | windowshealth | network | startup |
    ///                  service | scheduledtask | powerplan | privacy |    ///                  contextmenu | hostsfile | tempcleaner | installedapps |    ///                  marketplace | preferences | whatsnew
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
