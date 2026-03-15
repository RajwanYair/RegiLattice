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
    private ToolStripButton _forceCheck = null!;
    private ToolStripLabel _scopeLabel = null!;
    private ToolStripComboBox _scopeCombo = null!;
    private ToolStripButton _btnSelectAll = null!;
    private ToolStripButton _btnDeselectAll = null!;
    private ToolStripButton _btnInvert = null!;
    private ToolStripLabel _themeLabel = null!;
    private ToolStripComboBox _themeCombo = null!;

    // ── Kind filter ────────────────────────────────────────────────────────
    private ToolStripLabel _kindLabel = null!;
    private ToolStripComboBox _kindCombo = null!;

    // ── MenuStrip ──────────────────────────────────────────────────────────
    private MenuStrip _menuStrip = null!;

    // ── Main layout ────────────────────────────────────────────────────────
    private SplitContainer _split = null!;
    private TreeView _treeView = null!;
    private ListView _listView = null!;

    // ── Detail panel fields ───────────────────────────────────────────────
    private Panel _detailPanel = null!;
    private Label _detailLabel = null!;

    // ── Log panel ──────────────────────────────────────────────────────────
    private Panel _logPanel = null!;
    private RichTextBox _logBox = null!;

    // ── Context menu ───────────────────────────────────────────────────────
    private ContextMenuStrip _listContextMenu = null!;

    // ── StatusStrip ────────────────────────────────────────────────────────
    private StatusStrip _statusStrip = null!;
    private ToolStripStatusLabel _statusLabel = null!;
    private ToolStripStatusLabel _progressLabel = null!;
    private ToolStripProgressBar _progressBar = null!;

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

        var mnuFile = new ToolStripMenuItem("&File");
        mnuFile.DropDownItems.AddRange(new ToolStripItem[]
        {
            mnuExportPs1, mnuExportJson, mnuExportReg, mnuImportJson,
            new ToolStripSeparator(),
            mnuExit,
        });

        var mnuScoopMgr = new ToolStripMenuItem("Scoop Manager...") { Image = AppIcons.ScoopMenuBitmap };
        var mnuPsMgr = new ToolStripMenuItem("PowerShell Modules...") { Image = AppIcons.PSModuleMenuBitmap };
        var mnuPipMgr = new ToolStripMenuItem("pip Manager...") { Image = AppIcons.PipMenuBitmap };
        var mnuWinGetMgr = new ToolStripMenuItem("WinGet Manager...") { Image = AppIcons.WinGetMenuBitmap };
        var mnuChocoMgr = new ToolStripMenuItem("Chocolatey Manager...") { Image = AppIcons.ChocolateyMenuBitmap };
        var mnuToolVersions = new ToolStripMenuItem("Tool Versions...") { Image = AppIcons.ToolVersionsMenuBitmap };
        var mnuWinHealth = new ToolStripMenuItem("Windows Health...") { Image = AppIcons.WindowsHealthMenuBitmap };
        var mnuToolsRefresh = new ToolStripMenuItem("Refresh Status");
        var mnuSelectAll2 = new ToolStripMenuItem("Select All");
        var mnuDeselectAll2 = new ToolStripMenuItem("Deselect All");
        var mnuInvert2 = new ToolStripMenuItem("Invert Selection");

        var mnuTools = new ToolStripMenuItem("&Tools");
        mnuTools.DropDownItems.AddRange(new ToolStripItem[]
        {
            mnuScoopMgr, mnuPsMgr, mnuPipMgr, mnuWinGetMgr, mnuChocoMgr,
            new ToolStripSeparator(),
            mnuToolVersions,
            mnuWinHealth,
            new ToolStripSeparator(),
            new ToolStripMenuItem("Tweak Pack Marketplace…", AppIcons.MarketplaceMenuBitmap, (_, _) => OnOpenMarketplace()),
            new ToolStripSeparator(),
            mnuToolsRefresh,
            new ToolStripSeparator(),
            mnuSelectAll2, mnuDeselectAll2, mnuInvert2,
        });

        var mnuToggleLog = new ToolStripMenuItem("Toggle Log Panel");
        var mnuExpandAll = new ToolStripMenuItem("Expand All Categories");
        var mnuView = new ToolStripMenuItem("&View");
        mnuView.DropDownItems.AddRange(new ToolStripItem[] { mnuToggleLog, new ToolStripSeparator(), mnuExpandAll });

        var mnuAbout = new ToolStripMenuItem("About RegiLattice...");
        var mnuHwInfo = new ToolStripMenuItem("Hardware Info...");
        var mnuHelp = new ToolStripMenuItem("&Help");
        mnuHelp.DropDownItems.AddRange(new ToolStripItem[] { mnuHwInfo, new ToolStripSeparator(), mnuAbout });

        _menuStrip = new MenuStrip();
        _menuStrip.Items.AddRange(new ToolStripItem[] { mnuFile, mnuTools, mnuView, mnuHelp });
        _menuStrip.Dock = DockStyle.Top;

        // Wire menu events
        mnuExportPs1.Click += (_, _) => OnExportPs1();
        mnuExportJson.Click += (_, _) => OnExportJson();
        mnuExportReg.Click += (_, _) => OnExportReg();
        mnuImportJson.Click += (_, _) => OnImportJson();
        mnuExit.Click += (_, _) => Close();
        mnuScoopMgr.Click += (_, _) => OnOpenScoopManager();
        mnuPsMgr.Click += (_, _) => OnOpenPSModuleManager();
        mnuPipMgr.Click += (_, _) => OnOpenPipManager();
        mnuWinGetMgr.Click += (_, _) => OnOpenWinGetManager();
        mnuChocoMgr.Click += (_, _) => OnOpenChocolateyManager();
        mnuToolVersions.Click += (_, _) => OnOpenToolVersions();
        mnuWinHealth.Click += (_, _) => OnOpenWindowsHealth();
        mnuToolsRefresh.Click += async (_, _) => await RefreshStatusAsync();
        mnuSelectAll2.Click += (_, _) => SelectAllListItems();
        mnuDeselectAll2.Click += (_, _) => DeselectAllListItems();
        mnuInvert2.Click += (_, _) => InvertListSelection();
        mnuToggleLog.Click += (_, _) => ToggleLogPanel();
        mnuExpandAll.Click += (_, _) => _treeView.ExpandAll();
        mnuAbout.Click += (_, _) => OnAbout();
        mnuHwInfo.Click += (_, _) => OnHardwareInfo();

        // ── ToolStrip ──────────────────────────────────────────────────────
        _btnApply = new ToolStripButton("\u2714 Apply") { ToolTipText = "Apply selected tweaks (Ctrl+Enter)", DisplayStyle = ToolStripItemDisplayStyle.Text };
        _btnRemove = new ToolStripButton("\u2716 Remove") { ToolTipText = "Remove selected tweaks (Ctrl+Del)", DisplayStyle = ToolStripItemDisplayStyle.Text };
        _btnRefresh = new ToolStripButton("\U0001F504 Refresh") { ToolTipText = "Reload all tweaks (F5)", DisplayStyle = ToolStripItemDisplayStyle.Text };

        _filterLabel = new ToolStripLabel("Status:");
        _filterCombo = new ToolStripComboBox { DropDownStyle = ComboBoxStyle.DropDownList, ToolTipText = "Filter tweaks by status" };
        _filterCombo.Items.AddRange(new object[] { "All", "Applied", "Not Applied", "Default", "Unknown", "Errors" });
        _filterCombo.SelectedIndex = 0;

        _profileLabel = new ToolStripLabel("Profile:");
        _profileCombo = new ToolStripComboBox { DropDownStyle = ComboBoxStyle.DropDownList, ToolTipText = "Select a tweak profile" };
        _profileCombo.Items.AddRange(new object[] { "(None)", "business", "gaming", "privacy", "minimal", "server" });
        _profileCombo.SelectedIndex = 0;

        _searchBox = new ToolStripTextBox { ToolTipText = "Search tweaks (Ctrl+F)", Size = new Size(200, 25) };
        _searchBox.TextChanged += OnSearchTextChanged;

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

        _themeLabel = new ToolStripLabel("Theme:");
        _themeCombo = new ToolStripComboBox { DropDownStyle = ComboBoxStyle.DropDownList, ToolTipText = "Switch colour theme" };
        foreach (string t in AppTheme.AvailableThemes())
            _themeCombo.Items.Add(t);
        _themeCombo.SelectedItem = AppTheme.CurrentThemeName();
        _themeCombo.SelectedIndexChanged += OnThemeChanged;

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
            _searchBox,
            new ToolStripSeparator(),
            _forceCheck,
            new ToolStripSeparator(),
            _scopeLabel, _scopeCombo,
            new ToolStripSeparator(),
            _kindLabel, _kindCombo,
            new ToolStripSeparator(),
            _btnSelectAll, _btnDeselectAll, _btnInvert,
            new ToolStripSeparator(),
            _themeLabel, _themeCombo,
        });
        _toolStrip.Dock = DockStyle.Top;

        // ── TreeView ───────────────────────────────────────────────────────
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

        // ── SplitContainer ─────────────────────────────────────────────────
        _split = new SplitContainer { Dock = DockStyle.Fill, SplitterDistance = 220 };
        _split.Panel1.Controls.Add(_treeView);

        // ── Detail panel (bottom of Panel2) ────────────────────────────────
        _detailLabel = new Label
        {
            Dock = DockStyle.Fill,
            ForeColor = AppTheme.FgDim,
            Font = AppTheme.Regular,
            Padding = new Padding(10, 6, 10, 6),
            AutoSize = false,
            TextAlign = ContentAlignment.TopLeft,
            Text = "\U0001F4CB Select a tweak to see details.",
        };
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
        _detailPanel.Controls.Add(_detailLabel);

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
        _statusStrip = new StatusStrip { SizingGrip = false };
        _statusStrip.Items.AddRange(new ToolStripItem[] { _statusLabel, _progressLabel, _progressBar });
        _statusStrip.Dock = DockStyle.Bottom;

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
