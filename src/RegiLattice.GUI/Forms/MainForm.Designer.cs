using RegiLattice.Core;

namespace RegiLattice.GUI.Forms;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null!;

    // ── ToolStrip controls ─────────────────────────────────────────────────
    private ToolStrip _toolStrip = null!;
    private ToolStripButton _btnApply = null!;
    private ToolStripButton _btnRemove = null!;
    private ToolStripButton _btnRefresh = null!;
    private ToolStripLabel _filterLabel = null!;
    private ToolStripComboBox _filterCombo = null!;
    private ToolStripLabel _profileLabel = null!;
    private ToolStripComboBox _profileCombo = null!;
    private ToolStripTextBox _searchBox = null!;
    private ToolStripButton _searchClear = null!;
    private ToolStripButton _forceCheck = null!;
    private ToolStripLabel _scopeLabel = null!;
    private ToolStripComboBox _scopeCombo = null!;
    private ToolStripButton _btnSelectAll = null!;
    private ToolStripButton _btnDeselectAll = null!;
    private ToolStripButton _btnInvert = null!;
    private ToolStripButton _btnSettings = null!;
    private ToolStripButton _btnUndoLast = null!;

    // ── Kind filter ────────────────────────────────────────────────────────
    private ToolStripLabel _kindLabel = null!;
    private ToolStripComboBox _kindCombo = null!;

    // ── MenuStrip ──────────────────────────────────────────────────────────
    private MenuStrip _menuStrip = null!;
    private ToolStripMenuItem _mnuScoopMgr = null!;
    private ToolStripMenuItem _mnuPsMgr = null!;
    private ToolStripMenuItem _mnuPipMgr = null!;
    private ToolStripMenuItem _mnuWinGetMgr = null!;
    private ToolStripMenuItem _mnuChocoMgr = null!;
    private ToolStripMenuItem _mnuToolVersions = null!;
    private ToolStripMenuItem _mnuWinHealth = null!;
    private ToolStripMenuItem _mnuNetTools = null!;
    private ToolStripMenuItem _mnuStartupMgr = null!;
    private ToolStripMenuItem _mnuServiceMgr = null!;
    private ToolStripMenuItem _mnuSchedTaskMgr = null!;
    private ToolStripMenuItem _mnuPowerPlan = null!;
    private ToolStripMenuItem _mnuPrivacyDash = null!;
    private ToolStripMenuItem _mnuContextMenuMgr = null!;
    private ToolStripMenuItem _mnuHostsFileMgr = null!;
    private ToolStripMenuItem _mnuTempCleaner = null!;
    private ToolStripMenuItem _mnuInstalledApps = null!;
    private ToolStripMenuItem _mnuPowerScheduler = null!;
    private ToolStripMenuItem _mnuSleepTimer = null!;
    private ToolStripMenuItem _mnuBatterySaver = null!;
    private ToolStripMenuItem _mnuUsbPower = null!;
    private ToolStripMenuItem _mnuAdRemoval = null!;
    private ToolStripMenuItem _mnuTelemetryDash = null!;
    private ToolStripMenuItem _mnuAppPermissions = null!;
    private ToolStripMenuItem _mnuDnsOverHttps = null!;
    private ToolStripMenuItem _mnuNetRepair = null!;
    private ToolStripMenuItem _mnuDnsSwitcher = null!;
    private ToolStripMenuItem _mnuNetAdapter = null!;
    private ToolStripMenuItem _mnuWiFiProfiles = null!;
    private ToolStripMenuItem _mnuFirewallRules = null!;
    private ToolStripMenuItem _mnuProxyConfig = null!;
    private ToolStripMenuItem _mnuShellExtensions = null!;
    private ToolStripMenuItem _mnuBootAnalyzer = null!;
    private ToolStripMenuItem _mnuWuControl = null!;
    private ToolStripMenuItem _mnuNotifMgr = null!;
    private ToolStripMenuItem _mnuBrowserCache = null!;
    private ToolStripMenuItem _mnuDriverChecker = null!;
    private ToolStripMenuItem _mnuWakeOnLan = null!;
    private ToolStripMenuItem _mnuBrightness = null!;
    private ToolStripMenuItem _mnuMemoryCleaner = null!;
    private ToolStripMenuItem _mnuDiskSpace = null!;
    private ToolStripMenuItem _mnuPortScan = null!;
    private ToolStripMenuItem _mnuBatteryHealth = null!;
    private ToolStripMenuItem _mnuHwTempMon = null!;
    private ToolStripMenuItem _mnuNetBandwidth = null!;
    private ToolStripMenuItem _mnuMacAddress = null!;
    private ToolStripMenuItem _mnuMarketplace = null!;
    private ToolStripMenuItem _mnuProfileWizard = null!;
    private ToolStripMenuItem _mnuSmartScan = null!;
    private ToolStripMenuItem _mnuProfileCompare = null!;

    // ── Top-level and grouped sub-menus (need refresh on theme change) ─────
    private ToolStripMenuItem _mnuTools = null!;
    private ToolStripMenuItem _mnuPkgMgr = null!;
    private ToolStripMenuItem _subSysDiag = null!;
    private ToolStripMenuItem _subSysMgmt = null!;
    private ToolStripMenuItem _subPower = null!;
    private ToolStripMenuItem _subPrivSec = null!;
    private ToolStripMenuItem _subNetwork = null!;
    private ToolStripMenuItem _subCleanup = null!;

    // ── Main layout ────────────────────────────────────────────────────────
    private SplitContainer _split = null!;
    private TreeView _treeView = null!;
    private ImageList _categoryImageList = null!;
    private ListView _listView = null!;

    // ── Detail panel fields ───────────────────────────────────────────────
    private Panel _detailPanel = null!;
    private RichTextBox _detailBox = null!;

    // ── Log panel ──────────────────────────────────────────────────────────
    private Panel _logPanel = null!;
    private RichTextBox _logBox = null!;

    // ── Context menu ───────────────────────────────────────────────────────
    private ContextMenuStrip _listContextMenu = null!;

    // ── StatusStrip ────────────────────────────────────────────────────────
    private StatusStrip _statusStrip = null!;
    private ToolStripStatusLabel _statusLabel = null!;
    private ToolStripStatusLabel _progressLabel = null!;
    private ToolStripProgressBar _progressBar = null!;    private ToolStripStatusLabel _cpuLabel = null!;
    private ToolStripStatusLabel _memLabel = null!;
    private ToolStripStatusLabel _netLabel = null!;
    private ToolStripStatusLabel _adminBadge = null!;
    private System.Windows.Forms.Timer _monitorTimer = null!;
    private readonly SystemMonitor _sysMonitor = new();
    // ── Tray icon ──────────────────────────────────────────────────────────
    private NotifyIcon _trayIcon = null!;
    private ContextMenuStrip _trayMenu = null!;

    // ── Dispose ────────────────────────────────────────────────────────────
    protected override void Dispose(bool disposing)
    {
        if (disposing && components is not null)
            components.Dispose();
        base.Dispose(disposing);
    }

    // ── InitializeComponent ────────────────────────────────────────────────
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();

        // ── MenuStrip ──────────────────────────────────────────────────────
        var mnuExportPs1 = new ToolStripMenuItem("Export as PowerShell (.ps1)...") { ShortcutKeys = Keys.Control | Keys.Shift | Keys.E };
        var mnuExportJson = new ToolStripMenuItem("Export selected IDs as JSON...") { ShortcutKeys = Keys.Control | Keys.Shift | Keys.J };
        var mnuExportReg = new ToolStripMenuItem("Export as .REG file...") { ShortcutKeys = Keys.Control | Keys.Shift | Keys.R };
        var mnuImportJson = new ToolStripMenuItem("Import tweak IDs from JSON...") { ShortcutKeys = Keys.Control | Keys.Shift | Keys.I, Image = AppIcons.ImportMenuBitmap };
        var mnuExit = new ToolStripMenuItem("Exit") { Image = AppIcons.ExitMenuBitmap };
        var mnuPreferences = new ToolStripMenuItem("Preferences…") { ShortcutKeys = Keys.Control | Keys.Shift | Keys.P, Image = AppIcons.PreferencesMenuBitmap };

        mnuExportPs1.Image = AppIcons.ExportMenuBitmap;
        mnuExportJson.Image = AppIcons.ExportMenuBitmap;
        mnuExportReg.Image = AppIcons.ExportMenuBitmap;

        var mnuFile = new ToolStripMenuItem("&File") { Image = AppIcons.FileMenuBitmap };
        mnuFile.DropDownItems.AddRange(new ToolStripItem[]
        {
            mnuExportPs1, mnuExportJson, mnuExportReg, mnuImportJson,
            new ToolStripSeparator(),
            mnuPreferences,
            new ToolStripSeparator(),
            mnuExit,
        });

        _mnuScoopMgr = new ToolStripMenuItem("Scoop Manager...") { Image = AppIcons.ScoopMenuBitmap };
        _mnuPsMgr = new ToolStripMenuItem("PowerShell Modules...") { Image = AppIcons.PSModuleMenuBitmap };
        _mnuPipMgr = new ToolStripMenuItem("pip Manager...") { Image = AppIcons.PipMenuBitmap };
        _mnuWinGetMgr = new ToolStripMenuItem("WinGet Manager...") { Image = AppIcons.WinGetMenuBitmap };
        _mnuChocoMgr = new ToolStripMenuItem("Chocolatey Manager...") { Image = AppIcons.ChocolateyMenuBitmap };
        _mnuToolVersions = new ToolStripMenuItem("Tool Versions...") { Image = AppIcons.ToolVersionsMenuBitmap };
        _mnuWinHealth = new ToolStripMenuItem("Windows Health...") { Image = AppIcons.WindowsHealthMenuBitmap };
        _mnuNetTools = new ToolStripMenuItem("Network Tools...") { Image = AppIcons.NetworkMenuBitmap };
        _mnuStartupMgr = new ToolStripMenuItem("Startup Manager...") { Image = AppIcons.StartupMenuBitmap };
        _mnuServiceMgr = new ToolStripMenuItem("Service Manager...") { Image = AppIcons.ServiceMenuBitmap };
        _mnuSchedTaskMgr = new ToolStripMenuItem("Scheduled Task Manager...") { Image = AppIcons.ServiceMenuBitmap };
        _mnuPowerPlan = new ToolStripMenuItem("Power Plan Manager...") { Image = AppIcons.PerformanceMenuBitmap };
        _mnuPrivacyDash = new ToolStripMenuItem("Privacy Dashboard...") { Image = AppIcons.PrivacyMenuBitmap };
        _mnuContextMenuMgr = new ToolStripMenuItem("Context Menu Manager...") { Image = AppIcons.ExplorerMenuBitmap };
        _mnuHostsFileMgr  = new ToolStripMenuItem("Hosts File Manager...") { Image = AppIcons.NetworkMenuBitmap };
        _mnuTempCleaner   = new ToolStripMenuItem("Temp File Cleaner...") { Image = AppIcons.CleanupMenuBitmap };
        _mnuInstalledApps = new ToolStripMenuItem("Installed Applications...") { Image = AppIcons.MarketplaceMenuBitmap };
        _mnuPowerScheduler = new ToolStripMenuItem("Power Plan Scheduler...") { Image = AppIcons.PerformanceMenuBitmap };
        _mnuSleepTimer = new ToolStripMenuItem("Sleep / Hibernate Timer...") { Image = AppIcons.PerformanceMenuBitmap };
        _mnuBatterySaver = new ToolStripMenuItem("Battery Saver Settings...") { Image = AppIcons.PerformanceMenuBitmap };
        _mnuUsbPower = new ToolStripMenuItem("USB Power & Selective Suspend...") { Image = AppIcons.ServiceMenuBitmap };
        _mnuAdRemoval = new ToolStripMenuItem("Ad & Tip Removal Wizard...") { Image = AppIcons.PrivacyMenuBitmap };
        _mnuTelemetryDash = new ToolStripMenuItem("Telemetry Dashboard...") { Image = AppIcons.PrivacyMenuBitmap };
        _mnuAppPermissions = new ToolStripMenuItem("App Permissions...") { Image = AppIcons.PrivacyMenuBitmap };
        _mnuDnsOverHttps = new ToolStripMenuItem("DNS-over-HTTPS Setup...") { Image = AppIcons.NetworkMenuBitmap };
        _mnuNetRepair = new ToolStripMenuItem("Network Repair Wizard...") { Image = AppIcons.NetworkMenuBitmap };
        _mnuDnsSwitcher = new ToolStripMenuItem("DNS Server Quick-Switch...") { Image = AppIcons.NetworkMenuBitmap };
        _mnuNetAdapter = new ToolStripMenuItem("Network Adapter Manager...") { Image = AppIcons.NetworkMenuBitmap };
        _mnuWiFiProfiles = new ToolStripMenuItem("Wi-Fi Profile Manager...") { Image = AppIcons.NetworkMenuBitmap };
        _mnuFirewallRules = new ToolStripMenuItem("Firewall Rules...") { Image = AppIcons.NetworkMenuBitmap };
        _mnuProxyConfig = new ToolStripMenuItem("Proxy Configuration...") { Image = AppIcons.NetworkMenuBitmap };
        _mnuShellExtensions = new ToolStripMenuItem("Shell Extension Manager...") { Image = AppIcons.ExplorerMenuBitmap };
        _mnuBootAnalyzer = new ToolStripMenuItem("Boot Time Analyzer...") { Image = AppIcons.PerformanceMenuBitmap };
        _mnuWuControl = new ToolStripMenuItem("Windows Update Control...") { Image = AppIcons.WindowsHealthMenuBitmap };
        _mnuNotifMgr = new ToolStripMenuItem("Notification Manager...") { Image = AppIcons.PrivacyMenuBitmap };
        _mnuBrowserCache = new ToolStripMenuItem("Browser Cache Cleaner...") { Image = AppIcons.CleanupMenuBitmap };
        _mnuDriverChecker = new ToolStripMenuItem("Driver Update Checker...") { Image = AppIcons.PerformanceMenuBitmap };
        _mnuWakeOnLan   = new ToolStripMenuItem("Wake-on-LAN Configuration...") { Image = AppIcons.NetworkMenuBitmap };
        _mnuBrightness  = new ToolStripMenuItem("Brightness Scheduler...") { Image = AppIcons.PerformanceMenuBitmap };
        _mnuMemoryCleaner = new ToolStripMenuItem("Memory Cache Cleaner...") { Image = AppIcons.PerformanceMenuBitmap };
        _mnuDiskSpace   = new ToolStripMenuItem("Disk Space Analyser...") { Image = AppIcons.PerformanceMenuBitmap };
        _mnuPortScan    = new ToolStripMenuItem("Port / Connectivity Tester...") { Image = AppIcons.NetworkMenuBitmap };
        _mnuBatteryHealth = new ToolStripMenuItem("Battery Health...") { Image = AppIcons.PerformanceMenuBitmap };
        _mnuHwTempMon   = new ToolStripMenuItem("Hardware Temperature...") { Image = AppIcons.ThermometerMenuBitmap };
        _mnuNetBandwidth = new ToolStripMenuItem("Network Bandwidth Monitor...") { Image = AppIcons.BandwidthMenuBitmap };
        _mnuMacAddress  = new ToolStripMenuItem("MAC Address Manager...") { Image = AppIcons.MacAddressMenuBitmap };
        var mnuToolsRefresh = new ToolStripMenuItem("Refresh Status") { Image = AppIcons.RefreshMenuBitmap };
        var mnuSelectAll2 = new ToolStripMenuItem("Select All") { Image = AppIcons.ApplyMenuBitmap };
        var mnuDeselectAll2 = new ToolStripMenuItem("Deselect All") { Image = AppIcons.RemoveMenuBitmap };
        var mnuInvert2 = new ToolStripMenuItem("Invert Selection") { Image = AppIcons.InvertSelectionMenuBitmap };

        // ── Tools submenus ─────────────────────────────────────────────────
        _subSysDiag = new ToolStripMenuItem("System &Diagnostics") { Image = AppIcons.ToolVersionsMenuBitmap };
        _subSysDiag.DropDownItems.AddRange(new ToolStripItem[]
        {
            _mnuWinHealth,
            _mnuToolVersions,
            _mnuBootAnalyzer,
            _mnuWuControl,
            _mnuDriverChecker,
            new ToolStripSeparator(),
            _mnuBatteryHealth,
            _mnuHwTempMon,
        });

        _subSysMgmt = new ToolStripMenuItem("System &Management") { Image = AppIcons.ServiceMenuBitmap };
        _subSysMgmt.DropDownItems.AddRange(new ToolStripItem[]
        {
            _mnuStartupMgr,
            _mnuServiceMgr,
            _mnuSchedTaskMgr,
            new ToolStripSeparator(),
            _mnuInstalledApps,
            _mnuContextMenuMgr,
            _mnuShellExtensions,
        });

        _subPower = new ToolStripMenuItem("&Power && Energy") { Image = AppIcons.PowerMenuBitmap };
        _subPower.DropDownItems.AddRange(new ToolStripItem[]
        {
            _mnuPowerPlan,
            _mnuPowerScheduler,
            _mnuSleepTimer,
            new ToolStripSeparator(),
            _mnuBatterySaver,
            _mnuUsbPower,
            _mnuBrightness,
        });

        _subPrivSec = new ToolStripMenuItem("&Privacy && Security") { Image = AppIcons.PrivacyMenuBitmap };
        _subPrivSec.DropDownItems.AddRange(new ToolStripItem[]
        {
            _mnuPrivacyDash,
            _mnuAdRemoval,
            _mnuTelemetryDash,
            _mnuAppPermissions,
            new ToolStripSeparator(),
            _mnuFirewallRules,
            _mnuNotifMgr,
        });

        _subNetwork = new ToolStripMenuItem("&Network") { Image = AppIcons.NetworkMenuBitmap };
        _subNetwork.DropDownItems.AddRange(new ToolStripItem[]
        {
            _mnuNetTools,
            _mnuNetRepair,
            new ToolStripSeparator(),
            _mnuNetAdapter,
            _mnuWiFiProfiles,
            _mnuHostsFileMgr,
            _mnuProxyConfig,
            new ToolStripSeparator(),
            _mnuDnsSwitcher,
            _mnuDnsOverHttps,
            new ToolStripSeparator(),
            _mnuPortScan,
            _mnuNetBandwidth,
            _mnuWakeOnLan,
            _mnuMacAddress,
        });

        _subCleanup = new ToolStripMenuItem("&Cleanup && Performance") { Image = AppIcons.CleanupMenuBitmap };
        _subCleanup.DropDownItems.AddRange(new ToolStripItem[]
        {
            _mnuTempCleaner,
            _mnuBrowserCache,
            _mnuMemoryCleaner,
            _mnuDiskSpace,
        });

        _mnuTools = new ToolStripMenuItem("&Tools") { Image = AppIcons.ToolVersionsMenuBitmap };
        _mnuTools.DropDownItems.AddRange(new ToolStripItem[]
        {
            _subSysDiag,
            _subSysMgmt,
            _subPower,
            _subPrivSec,
            _subNetwork,
            _subCleanup,
            new ToolStripSeparator(),            _mnuSmartScan = new ToolStripMenuItem("\u26A1 Smart Scan\u2026") { Image = AppIcons.WizardMenuBitmap },            _mnuProfileCompare = new ToolStripMenuItem("\uD83D\uDD0D Profile Comparison\u2026") { Image = AppIcons.WizardMenuBitmap },            _mnuProfileWizard = new ToolStripMenuItem("Profile Recommendation Wizard…") { Image = AppIcons.WizardMenuBitmap },
            new ToolStripSeparator(),
            mnuToolsRefresh,
            new ToolStripSeparator(),
            mnuSelectAll2, mnuDeselectAll2, mnuInvert2,
        });

        _mnuPkgMgr = new ToolStripMenuItem("&Package Manager") { Image = AppIcons.MarketplaceMenuBitmap };
        _mnuPkgMgr.DropDownItems.AddRange(new ToolStripItem[]
        {
            _mnuScoopMgr, _mnuPsMgr, _mnuPipMgr, _mnuWinGetMgr, _mnuChocoMgr,
            new ToolStripSeparator(),
            _mnuMarketplace = new ToolStripMenuItem("Tweak Pack Marketplace\u2026", AppIcons.MarketplaceMenuBitmap, (_, _) => OnOpenMarketplace()),
        });

        var mnuToggleLog = new ToolStripMenuItem("Toggle Log Panel") { Image = AppIcons.LogMenuBitmap };
        var mnuExpandAll = new ToolStripMenuItem("Expand All Categories") { Image = AppIcons.ExpandMenuBitmap };
        var mnuView = new ToolStripMenuItem("&View") { Image = AppIcons.ViewMenuBitmap };
        mnuView.DropDownItems.AddRange(new ToolStripItem[] { mnuToggleLog, new ToolStripSeparator(), mnuExpandAll });

        var mnuAbout = new ToolStripMenuItem("About RegiLattice...") { Image = AppIcons.AboutMenuBitmap };
        var mnuHwInfo = new ToolStripMenuItem("Hardware Info...") { Image = AppIcons.HwInfoMenuBitmap };
        var mnuWhatsNew = new ToolStripMenuItem("What's New...") { Image = AppIcons.WhatsNewMenuBitmap };
        var mnuCheckUpdates = new ToolStripMenuItem("Check for Updates...") { Image = AppIcons.CheckUpdatesMenuBitmap };
        var mnuHelp = new ToolStripMenuItem("&Help") { Image = AppIcons.HelpMenuBitmap };
        mnuHelp.DropDownItems.AddRange(new ToolStripItem[] { mnuWhatsNew, mnuHwInfo, mnuCheckUpdates, new ToolStripSeparator(), mnuAbout });

        _menuStrip = new MenuStrip();
        _menuStrip.Items.AddRange(new ToolStripItem[] { mnuFile, _mnuPkgMgr, _mnuTools, mnuView, mnuHelp });
        _menuStrip.Dock = DockStyle.Top;

        // Wire menu events
        mnuExportPs1.Click += (_, _) => OnExportPs1();
        mnuExportJson.Click += (_, _) => OnExportJson();
        mnuExportReg.Click += (_, _) => OnExportReg();
        mnuImportJson.Click += (_, _) => OnImportJson();
        mnuPreferences.Click += (_, _) => OnOpenPreferences();
        mnuExit.Click += (_, _) => Close();
        _mnuScoopMgr.Click += (_, _) => OnOpenScoopManager();
        _mnuPsMgr.Click += (_, _) => OnOpenPSModuleManager();
        _mnuPipMgr.Click += (_, _) => OnOpenPipManager();
        _mnuWinGetMgr.Click += (_, _) => OnOpenWinGetManager();
        _mnuChocoMgr.Click += (_, _) => OnOpenChocolateyManager();
        _mnuToolVersions.Click += (_, _) => OnOpenToolVersions();
        _mnuWinHealth.Click += (_, _) => OnOpenWindowsHealth();
        _mnuNetTools.Click += (_, _) => OnOpenNetworkTools();
        _mnuStartupMgr.Click += (_, _) => OnOpenStartupManager();
        _mnuServiceMgr.Click += (_, _) => OnOpenServiceManager();
        _mnuSchedTaskMgr.Click += (_, _) => OnOpenScheduledTaskManager();
        _mnuPowerPlan.Click += (_, _) => OnOpenPowerPlan();
        _mnuPrivacyDash.Click       += (_, _) => OnOpenPrivacyDashboard();
        _mnuContextMenuMgr.Click      += (_, _) => OnOpenContextMenuManager();
        _mnuHostsFileMgr.Click        += (_, _) => OnOpenHostsFileManager();
        _mnuTempCleaner.Click         += (_, _) => OnOpenTempFileCleaner();
        _mnuInstalledApps.Click       += (_, _) => OnOpenInstalledApps();
        _mnuPowerScheduler.Click      += (_, _) => OnOpenPowerScheduler();
        _mnuSleepTimer.Click          += (_, _) => OnOpenSleepTimer();
        _mnuBatterySaver.Click        += (_, _) => OnOpenBatterySaver();
        _mnuUsbPower.Click            += (_, _) => OnOpenUsbPower();
        _mnuAdRemoval.Click           += (_, _) => OnOpenAdRemoval();
        _mnuTelemetryDash.Click       += (_, _) => OnOpenTelemetryDashboard();
        _mnuAppPermissions.Click      += (_, _) => OnOpenAppPermissions();
        _mnuDnsOverHttps.Click        += (_, _) => OnOpenDnsOverHttps();
        _mnuNetRepair.Click            += (_, _) => OnOpenNetworkRepair();
        _mnuDnsSwitcher.Click          += (_, _) => OnOpenDnsSwitcher();
        _mnuNetAdapter.Click           += (_, _) => OnOpenNetworkAdapter();
        _mnuWiFiProfiles.Click         += (_, _) => OnOpenWiFiProfiles();
        _mnuFirewallRules.Click       += (_, _) => OnOpenFirewallRules();
        _mnuProxyConfig.Click         += (_, _) => OnOpenProxyConfig();
        _mnuShellExtensions.Click     += (_, _) => OnOpenShellExtensions();
        _mnuBootAnalyzer.Click        += (_, _) => OnOpenBootAnalyzer();
        _mnuWuControl.Click            += (_, _) => OnOpenWuControl();
        _mnuNotifMgr.Click             += (_, _) => OnOpenNotifMgr();
        _mnuBrowserCache.Click         += (_, _) => OnOpenBrowserCache();
        _mnuDriverChecker.Click        += (_, _) => OnOpenDriverChecker();
        _mnuWakeOnLan.Click            += (_, _) => OnOpenWakeOnLan();
        _mnuBrightness.Click           += (_, _) => OnOpenBrightness();
        _mnuMemoryCleaner.Click        += (_, _) => OnOpenMemoryCleaner();
        _mnuDiskSpace.Click            += (_, _) => OnOpenDiskSpace();
        _mnuPortScan.Click             += (_, _) => OnOpenPortScanner();
        _mnuBatteryHealth.Click        += (_, _) => OnOpenBatteryHealth();
        _mnuHwTempMon.Click            += (_, _) => OnOpenHardwareTemp();
        _mnuNetBandwidth.Click         += (_, _) => OnOpenNetBandwidth();
        _mnuMacAddress.Click           += (_, _) => OnOpenMacAddress();
        _mnuSmartScan.Click += (_, _) => OnOpenSmartScan();
        _mnuProfileCompare.Click += (_, _) => OnOpenProfileCompare();
        _mnuProfileWizard.Click += (_, _) => OnOpenProfileWizard();
        mnuToolsRefresh.Click += async (_, _) => await RefreshStatusAsync();
        mnuSelectAll2.Click += (_, _) => SelectAllListItems();
        mnuDeselectAll2.Click += (_, _) => DeselectAllListItems();
        mnuInvert2.Click += (_, _) => InvertListSelection();
        mnuToggleLog.Click += (_, _) => ToggleLogPanel();
        mnuExpandAll.Click += (_, _) => _treeView.ExpandAll();
        mnuAbout.Click += (_, _) => OnAbout();
        mnuHwInfo.Click += (_, _) => OnHardwareInfo();
        mnuWhatsNew.Click += (_, _) => new WhatsNewDialog().ShowDialog(this);
        mnuCheckUpdates.Click += (_, _) => OnCheckForUpdates();

        // ── ToolStrip ──────────────────────────────────────────────────────
        _btnApply = new ToolStripButton("Apply") { ToolTipText = "Apply selected tweaks (Ctrl+Enter)", Image = AppIcons.ApplyMenuBitmap, DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
        _btnRemove = new ToolStripButton("Remove") { ToolTipText = "Remove selected tweaks (Ctrl+Del)", Image = AppIcons.RemoveMenuBitmap, DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
        _btnRefresh = new ToolStripButton("Refresh") { ToolTipText = "Reload all tweaks (F5)", Image = AppIcons.RefreshMenuBitmap, DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };

        _filterLabel = new ToolStripLabel("Status:") { ToolTipText = "Filter tweaks by their current applied status" };
        _filterCombo = new ToolStripComboBox { DropDownStyle = ComboBoxStyle.DropDownList, ToolTipText = "Filter tweaks by status" };
        _filterCombo.Items.AddRange(new object[] { "All", "Applied", "Not Applied", "Default", "Pending", "Unknown", "Errors" });
        _filterCombo.SelectedIndex = 0;

        _profileLabel = new ToolStripLabel("Profile:") { ToolTipText = "Select a profile to highlight relevant tweaks for that use case" };
        _profileCombo = new ToolStripComboBox { DropDownStyle = ComboBoxStyle.DropDownList, ToolTipText = "Select a tweak profile" };
        _profileCombo.Items.AddRange(new object[] { "(None)", "business", "gaming", "privacy", "minimal", "server" });
        _profileCombo.SelectedIndex = 0;

        _searchBox = new ToolStripTextBox { ToolTipText = "Search tweaks (Ctrl+F)", Size = new Size(200, 25) };
        _searchBox.TextChanged += OnSearchTextChanged;

        _searchClear = new ToolStripButton("\u2715")
        {
            ToolTipText = "Clear search (Esc)",
            DisplayStyle = ToolStripItemDisplayStyle.Text,
            Visible = false,
            Margin = new Padding(-4, 0, 2, 0),
        };
        _searchClear.Click += (_, _) => { _searchBox.Text = ""; _searchBox.Focus(); };

        _forceCheck = new ToolStripButton("\U0001F6E1 Force")
        {
            ToolTipText = "Bypass corporate network restrictions",
            CheckOnClick = true,
            DisplayStyle = ToolStripItemDisplayStyle.Text,
        };

        _scopeLabel = new ToolStripLabel("Scope:") { ToolTipText = "Filter tweaks by registry scope (HKCU vs HKLM)" };
        _scopeCombo = new ToolStripComboBox { DropDownStyle = ComboBoxStyle.DropDownList, ToolTipText = "Filter by registry scope", Width = 110 };
        _scopeCombo.Items.AddRange(new object[] { "All Scopes", "User (HKCU)", "Machine (HKLM)" });
        _scopeCombo.SelectedIndex = 0;

        _btnSelectAll = new ToolStripButton("\u2611 All") { ToolTipText = "Select all (Ctrl+A)", DisplayStyle = ToolStripItemDisplayStyle.Text };
        _btnDeselectAll = new ToolStripButton("\u2610 None") { ToolTipText = "Deselect all (Ctrl+D)", DisplayStyle = ToolStripItemDisplayStyle.Text };
        _btnInvert = new ToolStripButton("\U0001F504 Invert") { ToolTipText = "Invert selection (Ctrl+I)", DisplayStyle = ToolStripItemDisplayStyle.Text };

        // Settings / Preferences button (theme is now managed exclusively through Preferences)
        _btnSettings = new ToolStripButton("\u2699 Settings")
        {
            ToolTipText = "Open Preferences dialog (Ctrl+Shift+P)",
            DisplayStyle = ToolStripItemDisplayStyle.Text,
        };
        _btnSettings.Click += (_, _) => OnOpenPreferences();

        _btnUndoLast = new ToolStripButton("↩ Undo Last")
        {
            ToolTipText = "Undo the last tweak apply/remove operation",
            DisplayStyle = ToolStripItemDisplayStyle.Text,
            Enabled = false,
        };
        _btnUndoLast.Click += async (_, _) => await OnUndoLastAsync();

        _kindLabel = new ToolStripLabel("Kind:") { ToolTipText = "Filter tweaks by their operation kind (Registry, PowerShell, Service, etc.)" };
        _kindCombo = new ToolStripComboBox { DropDownStyle = ComboBoxStyle.DropDownList, ToolTipText = "Filter by tweak kind", Width = 130 };
        _kindCombo.Items.AddRange(new object[] { "All Kinds", "Registry", "PowerShell", "System Cmd", "Service", "Sched Task", "File Config", "Group Policy", "Package Mgr" });
        _kindCombo.SelectedIndex = 0;
        _kindCombo.SelectedIndexChanged += OnFilterChanged;

        _btnApply.Click += OnApplyClicked;
        _btnRemove.Click += OnRemoveClicked;
        _btnRefresh.Click += OnRefreshClicked;
        _filterCombo.SelectedIndexChanged += OnFilterChanged;
        _profileCombo.SelectedIndexChanged += OnProfileChanged;
        _scopeCombo.SelectedIndexChanged += OnFilterChanged;
        _btnSelectAll.Click += (_, _) => SelectAllListItems();
        _btnDeselectAll.Click += (_, _) => DeselectAllListItems();
        _btnInvert.Click += (_, _) => InvertListSelection();

        _toolStrip = new ToolStrip();
        _toolStrip.Items.AddRange(new ToolStripItem[]
        {
            _btnApply, _btnRemove, _btnUndoLast,
            new ToolStripSeparator(),
            _btnRefresh,
            new ToolStripSeparator(),
            _filterLabel, _filterCombo,
            _profileLabel, _profileCombo,
            new ToolStripSeparator(),
            _searchBox, _searchClear,
            new ToolStripSeparator(),
            _forceCheck,
            new ToolStripSeparator(),
            _scopeLabel, _scopeCombo,
            new ToolStripSeparator(),
            _kindLabel, _kindCombo,
            new ToolStripSeparator(),
            _btnSelectAll, _btnDeselectAll, _btnInvert,
            new ToolStripSeparator(),
            _btnSettings,
        });
        _toolStrip.Dock = DockStyle.Top;

        // ── TreeView ───────────────────────────────────────────────────────
        _categoryImageList = AppIcons.BuildCategoryImageList();
        _treeView = new TreeView
        {
            Dock = DockStyle.Fill,
            ShowLines = false,
            ShowRootLines = false,
            Font = AppTheme.Regular,
            BorderStyle = BorderStyle.None,
            ItemHeight = 26,
            FullRowSelect = true,
            HotTracking = true,
            ImageList = _categoryImageList,
        };
        _treeView.AfterSelect += OnTreeAfterSelect;

        // ── ListView ───────────────────────────────────────────────────────
        _listView = new ListView
        {
            Dock = DockStyle.Fill,
            View = View.Details,
            FullRowSelect = true,
            GridLines = false,
            MultiSelect = true,
            CheckBoxes = true,
            Font = AppTheme.Regular,
            BorderStyle = BorderStyle.None,
            HotTracking = false,
            SmallImageList = _categoryImageList,
        };
        _listView.Columns.AddRange(new[]
        {
            new ColumnHeader { Text = "Label",       Width = 260 },
            new ColumnHeader { Text = "Kind",        Width =  55 },
            new ColumnHeader { Text = "Status",      Width =  90 },
            new ColumnHeader { Text = "Scope",       Width =  80 },
            new ColumnHeader { Text = "Admin",       Width =  55 },
            new ColumnHeader { Text = "Corp Safe",   Width =  75 },
            new ColumnHeader { Text = "Description", Width = 260 },
        });

        // ── Context menu on ListView ───────────────────────────────────────
        _listContextMenu = new ContextMenuStrip();
        var ctxApply = new ToolStripMenuItem("Apply Selected");
        var ctxRemove = new ToolStripMenuItem("Remove Selected");
        var ctxCopyId = new ToolStripMenuItem("Copy ID");
        var ctxCopyKeys = new ToolStripMenuItem("Copy Registry Keys");
        var ctxSelAll = new ToolStripMenuItem("Select All");
        var ctxDeselAll = new ToolStripMenuItem("Deselect All");
        _listContextMenu.Items.AddRange(new ToolStripItem[]
        {
            ctxApply, ctxRemove,
            new ToolStripSeparator(),
            ctxCopyId, ctxCopyKeys,
            new ToolStripSeparator(),
            ctxSelAll, ctxDeselAll,
        });
        ctxApply.Click += async (_, _) => await ApplySelectedAsync();
        ctxRemove.Click += async (_, _) => await RemoveSelectedAsync();
        ctxCopyId.Click += (_, _) => CopySelectedId();
        ctxCopyKeys.Click += (_, _) => CopySelectedRegistryKeys();
        ctxSelAll.Click += (_, _) => SelectAllListItems();
        ctxDeselAll.Click += (_, _) => DeselectAllListItems();
        _listView.ContextMenuStrip = _listContextMenu;

        // Column sorting and filtering
        _listView.ListViewItemSorter = _columnSorter;
        _listView.ColumnClick += OnColumnClick;

        // ── ListView event handlers ────────────────────────────────────────
        _listView.MouseDoubleClick += OnListViewMouseDoubleClick;
        _listView.MouseMove        += OnListViewMouseMove;
        _listView.ItemCheck        += OnListViewItemCheck;

        // ── SplitContainer ─────────────────────────────────────────────────
        _split = new SplitContainer { Dock = DockStyle.Fill, SplitterDistance = 220 };
        _split.Panel1.Controls.Add(_treeView);

        // ── Detail panel (bottom of Panel2) ────────────────────────────────
        _detailBox = new RichTextBox
        {
            ReadOnly = true,                         // set FIRST so BackColor is not overridden
            BorderStyle = BorderStyle.None,
            ScrollBars = RichTextBoxScrollBars.Vertical,
            Multiline = true,
            WordWrap = true,
            Dock = DockStyle.Fill,
            TabStop = false,
            Text = "\U0001F4CB Select a tweak to see details.",
        };
        // Reinforce colors AFTER ReadOnly is set to prevent Windows from using System defaults
        _detailBox.BackColor = AppTheme.Surface;
        _detailBox.ForeColor = AppTheme.FgDim;
        _detailBox.Font = AppTheme.Regular;
        // Prevent the default white RichTextBox context menu from stealing focus
        _detailBox.MouseClick += (_, _) => _listView.Focus();
        _detailPanel = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 130,
            BackColor = AppTheme.Surface,
            BorderStyle = BorderStyle.None,
            Padding = new Padding(4),
        };
        _detailPanel.Paint += (_, pe) =>
        {
            // Accent left bar
            using var accentBrush = new SolidBrush(AppTheme.Accent);
            pe.Graphics.FillRectangle(accentBrush, 0, 4, 3, _detailPanel.Height - 8);
            // Top separator line
            using var sepPen = new Pen(AppTheme.Separator, 1f);
            pe.Graphics.DrawLine(sepPen, 0, 0, _detailPanel.Width, 0);
        };
        _detailPanel.Controls.Add(_detailBox);

        _split.Panel2.Controls.Add(_listView);
        _split.Panel2.Controls.Add(_detailPanel);
        _listView.SelectedIndexChanged += OnListViewSelectionChanged;

        // ── Log panel (bottom, collapsed by default) ───────────────────────
        _logBox = new RichTextBox
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            BackColor = AppTheme.Bg,
            ForeColor = AppTheme.Green,
            Font = AppTheme.Mono,
            BorderStyle = BorderStyle.None,
            ScrollBars = RichTextBoxScrollBars.Vertical,
        };
        _logPanel = new Panel { Dock = DockStyle.Bottom, Height = 150, Visible = true };
        _logPanel.Paint += (_, pe) =>
        {
            // Top separator line
            using var sepPen = new Pen(AppTheme.Separator, 1f);
            pe.Graphics.DrawLine(sepPen, 0, 0, _logPanel.Width, 0);
        };
        _logPanel.Controls.Add(_logBox);

        // ── StatusStrip ────────────────────────────────────────────────────
        _statusLabel = new ToolStripStatusLabel("Ready") { Spring = false, Font = AppTheme.Small, AutoToolTip = true, ToolTipText = "Current operation status" };
        _progressLabel = new ToolStripStatusLabel("") { Spring = true, TextAlign = ContentAlignment.MiddleLeft, Font = AppTheme.Small, AutoToolTip = true, ToolTipText = "Progress of the current batch operation" };
        _progressBar = new ToolStripProgressBar { Visible = false, Size = new Size(180, 16), Style = ProgressBarStyle.Marquee };
        _cpuLabel = new ToolStripStatusLabel("CPU: --") { Spring = false, Font = AppTheme.Small, ForeColor = AppTheme.Accent, AutoToolTip = true, ToolTipText = "CPU usage — updated every 2 seconds" };
        _memLabel = new ToolStripStatusLabel("RAM: --") { Spring = false, Font = AppTheme.Small, ForeColor = AppTheme.Accent, AutoToolTip = true, ToolTipText = "RAM usage (used / total) — updated every 2 seconds" };
        _netLabel = new ToolStripStatusLabel("Net: --") { Spring = false, Font = AppTheme.Small, ForeColor = AppTheme.Accent, AutoToolTip = true, ToolTipText = "Network connectivity status — updated every 2 seconds" };
        _statusStrip = new StatusStrip { SizingGrip = false };
        _adminBadge = new ToolStripStatusLabel("🛡 ADMIN")
        {
            Spring = false,
            Font = new Font(AppTheme.Small, FontStyle.Bold),
            ForeColor = Color.Black,
            BackColor = Color.Firebrick,
            Visible = Elevation.IsAdmin(),
            BorderSides = ToolStripStatusLabelBorderSides.All,
            BorderStyle = Border3DStyle.Flat,
            AutoToolTip = true,
            ToolTipText = "Running with administrator privileges — all registry operations are available",
        };
        _statusStrip.Items.AddRange(new ToolStripItem[] { _statusLabel, _progressLabel, _progressBar, new ToolStripSeparator(), _cpuLabel, _memLabel, new ToolStripSeparator(), _netLabel, new ToolStripSeparator(), _adminBadge });
        _statusStrip.Dock = DockStyle.Bottom;

        // ── System monitor timer ───────────────────────────────────────────
        _monitorTimer = new System.Windows.Forms.Timer(components!) { Interval = 2000 };
        _monitorTimer.Tick += OnMonitorTimerTick;

        // ── Tray icon ──────────────────────────────────────────────────────
        _trayMenu = new ContextMenuStrip();
        _trayMenu.Items.Add("Show RegiLattice", null, (_, _) => RestoreFromTray());
        _trayMenu.Items.Add(new ToolStripSeparator());
        _trayMenu.Items.Add("Exit", null, (_, _) => { _trayIcon.Visible = false; Application.Exit(); });

        _trayIcon = new NotifyIcon(components!)
        {
            Icon = AppIcons.AppIcon,
            Text = "RegiLattice",
            ContextMenuStrip = _trayMenu,
            Visible = false,
        };
        _trayIcon.DoubleClick += (_, _) => RestoreFromTray();

        // ── Form ───────────────────────────────────────────────────────────
        AutoScaleDimensions = new SizeF(96f, 96f);
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(1200, 750);
        MinimumSize = new Size(800, 500);
        Font = AppTheme.Regular;

        Controls.Add(_split);
        Controls.Add(_toolStrip);
        Controls.Add(_menuStrip);
        Controls.Add(_logPanel);
        Controls.Add(_statusStrip);
        MainMenuStrip = _menuStrip;

        // Keyboard shortcuts
        KeyPreview = true;
        KeyDown += OnGlobalKeyDown;
    }
}
