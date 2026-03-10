namespace RegiLattice.Native.Forms;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null!;

    // ── ToolStrip controls ─────────────────────────────────────────────────
    private ToolStrip         _toolStrip      = null!;
    private ToolStripButton   _btnApply       = null!;
    private ToolStripButton   _btnRemove      = null!;
    private ToolStripButton   _btnRefresh     = null!;
    private ToolStripLabel    _filterLabel    = null!;
    private ToolStripComboBox _filterCombo    = null!;
    private ToolStripLabel    _profileLabel   = null!;
    private ToolStripComboBox _profileCombo   = null!;
    private ToolStripTextBox  _searchBox      = null!;
    private ToolStripButton   _forceCheck     = null!;
    private ToolStripLabel    _scopeLabel     = null!;
    private ToolStripComboBox _scopeCombo     = null!;
    private ToolStripButton   _btnSelectAll   = null!;
    private ToolStripButton   _btnDeselectAll = null!;
    private ToolStripButton   _btnInvert      = null!;

    // ── MenuStrip ──────────────────────────────────────────────────────────
    private MenuStrip         _menuStrip      = null!;

    // ── Main layout ────────────────────────────────────────────────────────
    private SplitContainer    _split      = null!;
    private TreeView          _treeView   = null!;
    private ListView          _listView   = null!;

    // ── Log panel ──────────────────────────────────────────────────────────
    private Panel       _logPanel = null!;
    private RichTextBox _logBox   = null!;

    // ── Context menu ───────────────────────────────────────────────────────
    private ContextMenuStrip _listContextMenu = null!;

    // ── StatusStrip ────────────────────────────────────────────────────────
    private StatusStrip          _statusStrip   = null!;
    private ToolStripStatusLabel _statusLabel   = null!;
    private ToolStripStatusLabel _progressLabel = null!;
    private ToolStripProgressBar _progressBar   = null!;

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
        var mnuExportPs1    = new ToolStripMenuItem("Export as PowerShell (.ps1)...")  { ShortcutKeys = Keys.Control | Keys.Shift | Keys.E };
        var mnuExportJson   = new ToolStripMenuItem("Export selected IDs as JSON...")  { ShortcutKeys = Keys.Control | Keys.Shift | Keys.J };
        var mnuImportJson   = new ToolStripMenuItem("Import tweak IDs from JSON...")   { ShortcutKeys = Keys.Control | Keys.Shift | Keys.I };
        var mnuExit         = new ToolStripMenuItem("Exit");

        var mnuFile = new ToolStripMenuItem("&File");
        mnuFile.DropDownItems.AddRange(new ToolStripItem[]
        {
            mnuExportPs1, mnuExportJson, mnuImportJson,
            new ToolStripSeparator(),
            mnuExit,
        });

        var mnuScoopMgr     = new ToolStripMenuItem("Scoop Manager...");
        var mnuPsMgr        = new ToolStripMenuItem("PowerShell Modules...");
        var mnuPipMgr       = new ToolStripMenuItem("pip Packages...");
        var mnuToolsRefresh = new ToolStripMenuItem("Refresh Status");
        var mnuSelectAll2   = new ToolStripMenuItem("Select All");
        var mnuDeselectAll2 = new ToolStripMenuItem("Deselect All");
        var mnuInvert2      = new ToolStripMenuItem("Invert Selection");

        var mnuTools = new ToolStripMenuItem("&Tools");
        mnuTools.DropDownItems.AddRange(new ToolStripItem[]
        {
            mnuScoopMgr, mnuPsMgr, mnuPipMgr,
            new ToolStripSeparator(),
            mnuToolsRefresh,
            new ToolStripSeparator(),
            mnuSelectAll2, mnuDeselectAll2, mnuInvert2,
        });

        var mnuToggleLog = new ToolStripMenuItem("Toggle Log Panel");
        var mnuExpandAll = new ToolStripMenuItem("Expand All Categories");
        var mnuView      = new ToolStripMenuItem("&View");
        mnuView.DropDownItems.AddRange(new ToolStripItem[] { mnuToggleLog, new ToolStripSeparator(), mnuExpandAll });

        var mnuAbout = new ToolStripMenuItem("About RegiLattice...");
        var mnuHelp  = new ToolStripMenuItem("&Help");
        mnuHelp.DropDownItems.Add(mnuAbout);

        _menuStrip = new MenuStrip();
        _menuStrip.Items.AddRange(new ToolStripItem[] { mnuFile, mnuTools, mnuView, mnuHelp });
        _menuStrip.Dock = DockStyle.Top;

        // Wire menu events
        mnuExportPs1.Click    += async (_, _) => await OnExportPs1Async();
        mnuExportJson.Click   += async (_, _) => await OnExportJsonAsync();
        mnuImportJson.Click   += async (_, _) => await OnImportJsonAsync();
        mnuExit.Click         += (_, _) => Close();
        mnuScoopMgr.Click     += (_, _) => OnOpenScoopManager();
        mnuPsMgr.Click        += (_, _) => OnOpenPSModuleManager();
        mnuPipMgr.Click       += async (_, _) => await OnOpenPipManagerAsync();
        mnuToolsRefresh.Click += async (_, _) => await RefreshTweaksAsync();
        mnuSelectAll2.Click   += (_, _) => SelectAllListItems();
        mnuDeselectAll2.Click += (_, _) => DeselectAllListItems();
        mnuInvert2.Click      += (_, _) => InvertListSelection();
        mnuToggleLog.Click    += (_, _) => ToggleLogPanel();
        mnuExpandAll.Click    += (_, _) => _treeView.ExpandAll();
        mnuAbout.Click        += (_, _) => OnAbout();

        // ── ToolStrip ──────────────────────────────────────────────────────
        _btnApply   = new ToolStripButton("Apply")   { ToolTipText = "Apply selected tweaks (Ctrl+Enter)", DisplayStyle = ToolStripItemDisplayStyle.Text };
        _btnRemove  = new ToolStripButton("Remove")  { ToolTipText = "Remove selected tweaks (Ctrl+Del)",  DisplayStyle = ToolStripItemDisplayStyle.Text };
        _btnRefresh = new ToolStripButton("Refresh") { ToolTipText = "Reload all tweaks (F5)",             DisplayStyle = ToolStripItemDisplayStyle.Text };

        _filterLabel = new ToolStripLabel("Status:");
        _filterCombo = new ToolStripComboBox { DropDownStyle = ComboBoxStyle.DropDownList, ToolTipText = "Filter tweaks by status" };
        _filterCombo.Items.AddRange(new object[] { "All", "applied", "not_applied", "unknown" });
        _filterCombo.SelectedIndex = 0;

        _profileLabel = new ToolStripLabel("Profile:");
        _profileCombo = new ToolStripComboBox { DropDownStyle = ComboBoxStyle.DropDownList, ToolTipText = "Select a tweak profile" };
        _profileCombo.Items.AddRange(new object[] { "(None)", "business", "gaming", "privacy", "minimal", "server" });
        _profileCombo.SelectedIndex = 0;

        _searchBox = new ToolStripTextBox { ToolTipText = "Search tweaks (Ctrl+F)", Size = new Size(200, 25) };
        _searchBox.TextChanged += OnSearchTextChanged;

        _forceCheck = new ToolStripButton("Force")
        {
            ToolTipText  = "Bypass corporate network restrictions",
            CheckOnClick = true,
            DisplayStyle = ToolStripItemDisplayStyle.Text,
        };

        _scopeLabel = new ToolStripLabel("Scope:");
        _scopeCombo = new ToolStripComboBox { DropDownStyle = ComboBoxStyle.DropDownList, ToolTipText = "Filter by registry scope", Width = 110 };
        _scopeCombo.Items.AddRange(new object[] { "All Scopes", "User (HKCU)", "Machine (HKLM)" });
        _scopeCombo.SelectedIndex = 0;

        _btnSelectAll   = new ToolStripButton("Sel All")   { ToolTipText = "Select all (Ctrl+A)",        DisplayStyle = ToolStripItemDisplayStyle.Text };
        _btnDeselectAll = new ToolStripButton("Desel All") { ToolTipText = "Deselect all (Ctrl+D)",      DisplayStyle = ToolStripItemDisplayStyle.Text };
        _btnInvert      = new ToolStripButton("Invert")    { ToolTipText = "Invert selection (Ctrl+I)", DisplayStyle = ToolStripItemDisplayStyle.Text };

        _btnApply.Click           += OnApplyClicked;
        _btnRemove.Click          += OnRemoveClicked;
        _btnRefresh.Click         += OnRefreshClicked;
        _filterCombo.SelectedIndexChanged  += OnFilterChanged;
        _profileCombo.SelectedIndexChanged += OnProfileChanged;
        _scopeCombo.SelectedIndexChanged   += OnFilterChanged;
        _btnSelectAll.Click   += (_, _) => SelectAllListItems();
        _btnDeselectAll.Click += (_, _) => DeselectAllListItems();
        _btnInvert.Click      += (_, _) => InvertListSelection();

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
            _btnSelectAll, _btnDeselectAll, _btnInvert,
        });
        _toolStrip.Dock = DockStyle.Top;

        // ── TreeView ───────────────────────────────────────────────────────
        _treeView = new TreeView { Dock = DockStyle.Fill, ShowLines = true, Font = new Font("Segoe UI", 9f) };
        _treeView.AfterSelect += OnTreeAfterSelect;

        // ── ListView ───────────────────────────────────────────────────────
        _listView = new ListView
        {
            Dock          = DockStyle.Fill,
            View          = View.Details,
            FullRowSelect = true,
            GridLines     = true,
            MultiSelect   = true,
            CheckBoxes    = true,
            Font          = new Font("Segoe UI", 9f),
        };
        _listView.Columns.AddRange(new[]
        {
            new ColumnHeader { Text = "Label",       Width = 260 },
            new ColumnHeader { Text = "Status",      Width =  90 },
            new ColumnHeader { Text = "Scope",       Width =  70 },
            new ColumnHeader { Text = "Admin",       Width =  55 },
            new ColumnHeader { Text = "Corp Safe",   Width =  75 },
            new ColumnHeader { Text = "Description", Width = 250 },
        });

        // ── Context menu on ListView ───────────────────────────────────────
        _listContextMenu = new ContextMenuStrip();
        var ctxApply    = new ToolStripMenuItem("Apply Selected");
        var ctxRemove   = new ToolStripMenuItem("Remove Selected");
        var ctxCopyId   = new ToolStripMenuItem("Copy ID");
        var ctxCopyKeys = new ToolStripMenuItem("Copy Registry Keys");
        var ctxSelAll   = new ToolStripMenuItem("Select All");
        var ctxDeselAll = new ToolStripMenuItem("Deselect All");
        _listContextMenu.Items.AddRange(new ToolStripItem[]
        {
            ctxApply, ctxRemove,
            new ToolStripSeparator(),
            ctxCopyId, ctxCopyKeys,
            new ToolStripSeparator(),
            ctxSelAll, ctxDeselAll,
        });
        ctxApply.Click    += async (_, _) => await ApplySelectedAsync();
        ctxRemove.Click   += async (_, _) => await RemoveSelectedAsync();
        ctxCopyId.Click   += (_, _) => CopySelectedId();
        ctxCopyKeys.Click += (_, _) => CopySelectedRegistryKeys();
        ctxSelAll.Click   += (_, _) => SelectAllListItems();
        ctxDeselAll.Click += (_, _) => DeselectAllListItems();
        _listView.ContextMenuStrip = _listContextMenu;

        // ── SplitContainer ─────────────────────────────────────────────────
        _split = new SplitContainer { Dock = DockStyle.Fill, SplitterDistance = 220 };
        _split.Panel1.Controls.Add(_treeView);
        _split.Panel2.Controls.Add(_listView);

        // ── Log panel (bottom, collapsed by default) ───────────────────────
        _logBox = new RichTextBox
        {
            Dock        = DockStyle.Fill,
            ReadOnly    = true,
            BackColor   = Color.FromArgb(24, 24, 37),
            ForeColor   = Color.FromArgb(166, 227, 161),
            Font        = new Font("Consolas", 9f),
            BorderStyle = BorderStyle.None,
        };
        _logPanel = new Panel { Dock = DockStyle.Bottom, Height = 150, Visible = false };
        _logPanel.Controls.Add(_logBox);

        // ── StatusStrip ────────────────────────────────────────────────────
        _statusLabel   = new ToolStripStatusLabel("Ready") { Spring = false };
        _progressLabel = new ToolStripStatusLabel("") { Spring = true, TextAlign = ContentAlignment.MiddleLeft };
        _progressBar   = new ToolStripProgressBar { Visible = false, Size = new Size(150, 16), Style = ProgressBarStyle.Marquee };
        _statusStrip   = new StatusStrip();
        _statusStrip.Items.AddRange(new ToolStripItem[] { _statusLabel, _progressLabel, _progressBar });
        _statusStrip.Dock = DockStyle.Bottom;

        // ── Form ───────────────────────────────────────────────────────────
        AutoScaleDimensions = new SizeF(96f, 96f);
        AutoScaleMode       = AutoScaleMode.Dpi;
        ClientSize          = new Size(1200, 750);
        MinimumSize         = new Size(800, 500);
        Font                = new Font("Segoe UI", 9f);

        // Dock order (higher Controls index = processed first = gets outer edge):
        //   _statusStrip (index 4) → absolute bottom
        //   _logPanel    (index 3) → above status
        //   _menuStrip   (index 2) → absolute top
        //   _toolStrip   (index 1) → below menu
        //   _split       (index 0) → Fill (takes remaining centre)
        Controls.Add(_split);
        Controls.Add(_toolStrip);
        Controls.Add(_menuStrip);
        Controls.Add(_logPanel);
        Controls.Add(_statusStrip);
        MainMenuStrip = _menuStrip;

        // Keyboard shortcuts
        KeyPreview = true;
        KeyDown   += OnGlobalKeyDown;
    }
}
