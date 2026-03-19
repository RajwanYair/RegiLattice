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
    private ToolStripMenuItem _mnuMarketplace = null!;

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
        var mnuImportJson = new ToolStripMenuItem("Import tweak IDs from JSON...") { ShortcutKeys = Keys.Control | Keys.Shift | Keys.I };
        var mnuExit = new ToolStripMenuItem("Exit");
        var mnuPreferences = new ToolStripMenuItem("Preferences…") { ShortcutKeys = Keys.Control | Keys.Shift | Keys.P };

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
        var mnuToolsRefresh = new ToolStripMenuItem("Refresh Status");
        var mnuSelectAll2 = new ToolStripMenuItem("Select All");
        var mnuDeselectAll2 = new ToolStripMenuItem("Deselect All");
        var mnuInvert2 = new ToolStripMenuItem("Invert Selection");

        var mnuTools = new ToolStripMenuItem("&Tools");
        mnuTools.DropDownItems.AddRange(new ToolStripItem[]
        {
            _mnuScoopMgr, _mnuPsMgr, _mnuPipMgr, _mnuWinGetMgr, _mnuChocoMgr,
            new ToolStripSeparator(),
            _mnuToolVersions,
            _mnuWinHealth,
            _mnuNetTools,
            _mnuStartupMgr,
            _mnuServiceMgr,
            _mnuSchedTaskMgr,
            _mnuPowerPlan,
            _mnuPrivacyDash,
            new ToolStripSeparator(),
            _mnuContextMenuMgr,
            _mnuHostsFileMgr,
            _mnuTempCleaner,
            _mnuInstalledApps,
            new ToolStripSeparator(),
            _mnuPowerScheduler,
            _mnuSleepTimer,
            _mnuBatterySaver,
            _mnuUsbPower,
            new ToolStripSeparator(),
            _mnuAdRemoval,
            _mnuTelemetryDash,
            _mnuAppPermissions,
            _mnuDnsOverHttps,
            new ToolStripSeparator(),
            _mnuNetRepair,
            _mnuDnsSwitcher,
            _mnuNetAdapter,
            _mnuWiFiProfiles,
            new ToolStripSeparator(),
            _mnuFirewallRules,
            _mnuProxyConfig,
            _mnuShellExtensions,
            _mnuBootAnalyzer,
            new ToolStripSeparator(),
            _mnuMarketplace = new ToolStripMenuItem("Tweak Pack Marketplace…", AppIcons.MarketplaceMenuBitmap, (_, _) => OnOpenMarketplace()),
            new ToolStripSeparator(),
            mnuToolsRefresh,
            new ToolStripSeparator(),
            mnuSelectAll2, mnuDeselectAll2, mnuInvert2,
        });

        var mnuToggleLog = new ToolStripMenuItem("Toggle Log Panel");
        var mnuExpandAll = new ToolStripMenuItem("Expand All Categories");
        var mnuView = new ToolStripMenuItem("&View") { Image = AppIcons.ViewMenuBitmap };
        mnuView.DropDownItems.AddRange(new ToolStripItem[] { mnuToggleLog, new ToolStripSeparator(), mnuExpandAll });

        var mnuAbout = new ToolStripMenuItem("About RegiLattice...");
        var mnuHwInfo = new ToolStripMenuItem("Hardware Info...");
        var mnuWhatsNew = new ToolStripMenuItem("What's New...");
        var mnuHelp = new ToolStripMenuItem("&Help") { Image = AppIcons.HelpMenuBitmap };
        mnuHelp.DropDownItems.AddRange(new ToolStripItem[] { mnuWhatsNew, mnuHwInfo, new ToolStripSeparator(), mnuAbout });

        _menuStrip = new MenuStrip();
        _menuStrip.Items.AddRange(new ToolStripItem[] { mnuFile, mnuTools, mnuView, mnuHelp });
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
        mnuToolsRefresh.Click += async (_, _) => await RefreshStatusAsync();
        mnuSelectAll2.Click += (_, _) => SelectAllListItems();
        mnuDeselectAll2.Click += (_, _) => DeselectAllListItems();
        mnuInvert2.Click += (_, _) => InvertListSelection();
        mnuToggleLog.Click += (_, _) => ToggleLogPanel();
        mnuExpandAll.Click += (_, _) => _treeView.ExpandAll();
        mnuAbout.Click += (_, _) => OnAbout();
        mnuHwInfo.Click += (_, _) => OnHardwareInfo();
        mnuWhatsNew.Click += (_, _) => new WhatsNewDialog().ShowDialog(this);

        // ── ToolStrip ──────────────────────────────────────────────────────
        _btnApply = new ToolStripButton("Apply") { ToolTipText = "Apply selected tweaks (Ctrl+Enter)", Image = AppIcons.ApplyMenuBitmap, DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
        _btnRemove = new ToolStripButton("Remove") { ToolTipText = "Remove selected tweaks (Ctrl+Del)", Image = AppIcons.RemoveMenuBitmap, DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
        _btnRefresh = new ToolStripButton("Refresh") { ToolTipText = "Reload all tweaks (F5)", Image = AppIcons.RefreshMenuBitmap, DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };

        _filterLabel = new ToolStripLabel("Status:");
        _filterCombo = new ToolStripComboBox { DropDownStyle = ComboBoxStyle.DropDownList, ToolTipText = "Filter tweaks by status" };
        _filterCombo.Items.AddRange(new object[] { "All", "Applied", "Not Applied", "Default", "Pending", "Unknown", "Errors" });
        _filterCombo.SelectedIndex = 0;

        _profileLabel = new ToolStripLabel("Profile:");
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

        _scopeLabel = new ToolStripLabel("Scope:");
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

        _kindLabel = new ToolStripLabel("Kind:");
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
            _btnApply, _btnRemove,
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
        _listView.ItemCheck += OnListViewItemCheck;

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
        _statusLabel = new ToolStripStatusLabel("Ready") { Spring = false, Font = AppTheme.Small };
        _progressLabel = new ToolStripStatusLabel("") { Spring = true, TextAlign = ContentAlignment.MiddleLeft, Font = AppTheme.Small };
        _progressBar = new ToolStripProgressBar { Visible = false, Size = new Size(180, 16), Style = ProgressBarStyle.Marquee };
        _cpuLabel = new ToolStripStatusLabel("CPU: --") { Spring = false, Font = AppTheme.Small, ForeColor = AppTheme.Accent };
        _memLabel = new ToolStripStatusLabel("RAM: --") { Spring = false, Font = AppTheme.Small, ForeColor = AppTheme.Accent };
        _statusStrip = new StatusStrip { SizingGrip = false };
        _statusStrip.Items.AddRange(new ToolStripItem[] { _statusLabel, _progressLabel, _progressBar, new ToolStripSeparator(), _cpuLabel, _memLabel });
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
