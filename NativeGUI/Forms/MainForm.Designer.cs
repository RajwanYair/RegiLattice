namespace RegiLattice.Native.Forms;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null!;

    // ── Controls ───────────────────────────────────────────────────────────
    private ToolStrip         _toolStrip    = null!;
    private ToolStripButton   _btnApply     = null!;
    private ToolStripButton   _btnRemove    = null!;
    private ToolStripButton   _btnRefresh   = null!;
    private ToolStripLabel    _filterLabel  = null!;
    private ToolStripComboBox _filterCombo  = null!;
    private ToolStripLabel    _profileLabel = null!;
    private ToolStripComboBox _profileCombo = null!;
    private ToolStripTextBox  _searchBox    = null!;
    private ToolStripButton   _forceCheck   = null!;

    private SplitContainer    _split        = null!;
    private TreeView          _treeView     = null!;
    private ListView          _listView     = null!;

    private StatusStrip       _statusStrip  = null!;
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

        // ── ToolStrip ──────────────────────────────────────────────────────
        _btnApply   = new ToolStripButton("Apply")   { ToolTipText = "Apply selected tweaks (Ctrl+Enter)", DisplayStyle = ToolStripItemDisplayStyle.Text };
        _btnRemove  = new ToolStripButton("Remove")  { ToolTipText = "Remove selected tweaks (Ctrl+Del)", DisplayStyle = ToolStripItemDisplayStyle.Text };
        _btnRefresh = new ToolStripButton("Refresh")          { ToolTipText = "Reload all tweaks (F5)", DisplayStyle = ToolStripItemDisplayStyle.Text };

        _filterLabel = new ToolStripLabel("Status:");
        _filterCombo = new ToolStripComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            ToolTipText   = "Filter tweaks by status",
        };
        _filterCombo.Items.AddRange(new object[] { "All", "applied", "not_applied", "unknown" });
        _filterCombo.SelectedIndex = 0;

        _profileLabel = new ToolStripLabel("Profile:");
        _profileCombo = new ToolStripComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            ToolTipText   = "Select a tweak profile",
        };
        _profileCombo.Items.AddRange(new object[] { "(None)", "business", "gaming", "privacy", "minimal", "server" });
        _profileCombo.SelectedIndex = 0;

        _searchBox = new ToolStripTextBox
        {
            ToolTipText = "Search tweaks (Ctrl+F)",
            Size        = new Size(200, 25),
        };
        _searchBox.TextChanged += OnSearchTextChanged;

        _forceCheck = new ToolStripButton("Force")
        {
            ToolTipText   = "Bypass corporate network restrictions",
            CheckOnClick  = true,
            DisplayStyle  = ToolStripItemDisplayStyle.Text,
        };

        _btnApply.Click   += OnApplyClicked;
        _btnRemove.Click  += OnRemoveClicked;
        _btnRefresh.Click += OnRefreshClicked;
        _filterCombo.SelectedIndexChanged += OnFilterChanged;
        _profileCombo.SelectedIndexChanged += OnProfileChanged;

        _toolStrip = new ToolStrip();
        _toolStrip.Items.AddRange(new ToolStripItem[]
        {
            _btnApply,
            _btnRemove,
            new ToolStripSeparator(),
            _btnRefresh,
            new ToolStripSeparator(),
            _filterLabel,
            _filterCombo,
            _profileLabel,
            _profileCombo,
            new ToolStripSeparator(),
            _searchBox,
            new ToolStripSeparator(),
            _forceCheck,
        });

        // ── TreeView ───────────────────────────────────────────────────────
        _treeView = new TreeView
        {
            Dock      = DockStyle.Fill,
            ShowLines = true,
            Font      = new Font("Segoe UI", 9f),
        };
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

        // ── SplitContainer ─────────────────────────────────────────────────
        _split = new SplitContainer
        {
            Dock             = DockStyle.Fill,
            SplitterDistance = 220,
        };
        _split.Panel1.Controls.Add(_treeView);
        _split.Panel2.Controls.Add(_listView);

        // ── StatusStrip ────────────────────────────────────────────────────
        _statusLabel   = new ToolStripStatusLabel("Ready") { Spring = false };
        _progressLabel = new ToolStripStatusLabel("")       { Spring = true,  TextAlign = ContentAlignment.MiddleLeft };
        _progressBar   = new ToolStripProgressBar           { Visible = false, Size = new Size(150, 16), Style = ProgressBarStyle.Marquee };

        _statusStrip = new StatusStrip();
        _statusStrip.Items.AddRange(new ToolStripItem[] { _statusLabel, _progressLabel, _progressBar });

        // ── Form ───────────────────────────────────────────────────────────
        AutoScaleDimensions = new SizeF(96f, 96f);
        AutoScaleMode       = AutoScaleMode.Dpi;
        ClientSize          = new Size(1100, 700);
        MinimumSize         = new Size(700, 450);
        Font                = new Font("Segoe UI", 9f);

        Controls.Add(_split);
        Controls.Add(_toolStrip);
        Controls.Add(_statusStrip);

        // Keyboard shortcuts
        KeyPreview = true;
        KeyDown += (_, e) =>
        {
            if (e.Control && e.KeyCode == Keys.Enter)      { OnApplyClicked(this, e);   e.Handled = true; }
            if (e.Control && e.KeyCode == Keys.Delete)     { OnRemoveClicked(this, e);  e.Handled = true; }
            if (e.KeyCode == Keys.F5)                      { OnRefreshClicked(this, e); e.Handled = true; }
            if (e.Control && e.KeyCode == Keys.F)          { _searchBox.Focus();        e.Handled = true; }
            if (e.KeyCode == Keys.Escape)                  { _searchBox.Text = "";      e.Handled = true; }
            if (e.Control && e.KeyCode == Keys.A)          { SelectAllListItems();      e.Handled = true; }
        };
    }
}
