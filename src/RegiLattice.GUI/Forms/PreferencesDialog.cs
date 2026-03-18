// RegiLattice.GUI — Forms/PreferencesDialog.cs
// Preferences/Settings dialog for GUI configuration.

using RegiLattice.Core;

namespace RegiLattice.GUI.Forms;

/// <summary>
/// Modal dialog for editing application preferences.
/// Groups settings by category: Appearance, Behaviour, Performance, Data.
/// </summary>
internal sealed class PreferencesDialog : Form
{
    private readonly AppConfig _config;
    private bool _themeChanged;
    private bool _localeChanged;

    // ── Controls ────────────────────────────────────────────────────────
    private readonly TabControl _tabs;

    // Appearance
    private readonly ComboBox _themeCombo;
    private readonly ComboBox _localeCombo;
    private readonly NumericUpDown _detailHeight;

    // Behaviour
    private readonly CheckBox _chkMinimizeToTray;
    private readonly CheckBox _chkConfirmApply;
    private readonly CheckBox _chkConfirmRemove;
    private readonly CheckBox _chkShowInapplicable;
    private readonly CheckBox _chkForceCorp;

    // Performance
    private readonly NumericUpDown _maxWorkers;
    private readonly CheckBox _chkStatusMonitor;

    // Data
    private readonly CheckBox _chkAutoBackup;
    private readonly CheckBox _chkToolUpdates;
    private readonly TextBox _backupDir;
    private readonly Button _btnBrowseBackup;

    // Buttons
    private readonly Button _btnOk;
    private readonly Button _btnCancel;
    private readonly Button _btnDefaults;

    internal PreferencesDialog(AppConfig config)
    {
        _config = config;
        Text = "Preferences";
        Size = new Size(520, 500);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
        ShowInTaskbar = false;

        _tabs = new TabControl { Dock = DockStyle.Fill };

        // ── Appearance Tab ──────────────────────────────────────────────
        var tabAppearance = new TabPage("Appearance");

        _themeCombo = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 200 };
        foreach (string t in AppTheme.AvailableThemes())
            _themeCombo.Items.Add(t);
        _themeCombo.SelectedItem = config.Theme;

        _localeCombo = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 200 };
        _localeCombo.Items.AddRange(new object[] { "en", "de" });
        _localeCombo.SelectedItem = config.Locale;

        _detailHeight = new NumericUpDown
        {
            Minimum = 80,
            Maximum = 400,
            Value = config.DetailPanelHeight,
            Width = 80,
        };

        var appearancePanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 4,
            Padding = new Padding(12),
        };
        appearancePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160));
        appearancePanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

        appearancePanel.Controls.Add(CreateLabel("Theme:"), 0, 0);
        appearancePanel.Controls.Add(_themeCombo, 1, 0);
        appearancePanel.Controls.Add(CreateLabel("Language:"), 0, 1);
        appearancePanel.Controls.Add(_localeCombo, 1, 1);
        appearancePanel.Controls.Add(CreateLabel("Detail panel height:"), 0, 2);
        appearancePanel.Controls.Add(_detailHeight, 1, 2);

        tabAppearance.Controls.Add(appearancePanel);

        // ── Behaviour Tab ───────────────────────────────────────────────
        var tabBehaviour = new TabPage("Behaviour");

        _chkMinimizeToTray = new CheckBox
        {
            Text = "Minimize to system tray",
            Checked = config.MinimizeToTray,
            AutoSize = true,
        };
        _chkConfirmApply = new CheckBox
        {
            Text = "Confirm before applying tweaks",
            Checked = config.ConfirmApply,
            AutoSize = true,
        };
        _chkConfirmRemove = new CheckBox
        {
            Text = "Confirm before removing tweaks",
            Checked = config.ConfirmRemove,
            AutoSize = true,
        };
        _chkShowInapplicable = new CheckBox
        {
            Text = "Show inapplicable tweaks (greyed out)",
            Checked = config.ShowInapplicable,
            AutoSize = true,
        };
        _chkForceCorp = new CheckBox
        {
            Text = "Force corporate guard override",
            Checked = config.ForceCorp,
            AutoSize = true,
        };

        var behaviourPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            Padding = new Padding(12),
            WrapContents = false,
        };
        behaviourPanel.Controls.Add(_chkMinimizeToTray);
        behaviourPanel.Controls.Add(_chkConfirmApply);
        behaviourPanel.Controls.Add(_chkConfirmRemove);
        behaviourPanel.Controls.Add(_chkShowInapplicable);
        behaviourPanel.Controls.Add(_chkForceCorp);

        tabBehaviour.Controls.Add(behaviourPanel);

        // ── Performance Tab ─────────────────────────────────────────────
        var tabPerformance = new TabPage("Performance");

        _maxWorkers = new NumericUpDown
        {
            Minimum = 1,
            Maximum = 32,
            Value = config.MaxWorkers,
            Width = 80,
        };
        _chkStatusMonitor = new CheckBox
        {
            Text = "Show CPU/RAM monitor in status bar",
            Checked = config.StatusBarMonitor,
            AutoSize = true,
        };

        var perfPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 3,
            Padding = new Padding(12),
        };
        perfPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160));
        perfPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

        perfPanel.Controls.Add(CreateLabel("Max parallel workers:"), 0, 0);
        perfPanel.Controls.Add(_maxWorkers, 1, 0);
        perfPanel.Controls.Add(_chkStatusMonitor, 0, 1);
        perfPanel.SetColumnSpan(_chkStatusMonitor, 2);

        tabPerformance.Controls.Add(perfPanel);

        // ── Data Tab ────────────────────────────────────────────────────
        var tabData = new TabPage("Data");

        _chkAutoBackup = new CheckBox
        {
            Text = "Auto-backup before registry changes",
            Checked = config.AutoBackup,
            AutoSize = true,
        };
        _chkToolUpdates = new CheckBox
        {
            Text = "Check for tool updates on startup",
            Checked = config.CheckToolUpdates,
            AutoSize = true,
        };

        _backupDir = new TextBox { Text = config.BackupDir, Width = 270 };
        _btnBrowseBackup = new Button { Text = "Browse…", Width = 80 };
        _btnBrowseBackup.Click += OnBrowseBackup;

        var dataPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 3,
            RowCount = 4,
            Padding = new Padding(12),
        };
        dataPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
        dataPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        dataPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

        dataPanel.Controls.Add(_chkAutoBackup, 0, 0);
        dataPanel.SetColumnSpan(_chkAutoBackup, 3);
        dataPanel.Controls.Add(_chkToolUpdates, 0, 1);
        dataPanel.SetColumnSpan(_chkToolUpdates, 3);
        dataPanel.Controls.Add(CreateLabel("Backup directory:"), 0, 2);
        dataPanel.Controls.Add(_backupDir, 1, 2);
        dataPanel.Controls.Add(_btnBrowseBackup, 2, 2);

        tabData.Controls.Add(dataPanel);

        // ── Add tabs ────────────────────────────────────────────────────
        _tabs.TabPages.AddRange([tabAppearance, tabBehaviour, tabPerformance, tabData]);

        // ── Bottom buttons ──────────────────────────────────────────────
        _btnOk = new Button
        {
            Text = "OK",
            DialogResult = DialogResult.OK,
            Width = 80,
        };
        _btnCancel = new Button
        {
            Text = "Cancel",
            DialogResult = DialogResult.Cancel,
            Width = 80,
        };
        _btnDefaults = new Button { Text = "Restore Defaults", Width = 120 };
        _btnDefaults.Click += OnRestoreDefaults;

        var buttonPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            FlowDirection = FlowDirection.RightToLeft,
            Height = 45,
            Padding = new Padding(8, 6, 8, 6),
        };
        buttonPanel.Controls.Add(_btnCancel);
        buttonPanel.Controls.Add(_btnOk);
        buttonPanel.Controls.Add(_btnDefaults);

        Controls.Add(_tabs);
        Controls.Add(buttonPanel);

        AcceptButton = _btnOk;
        CancelButton = _btnCancel;

        ApplyTheme();
    }

    /// <summary>Applies config changes when user clicks OK. Returns true if applied.</summary>
    internal bool ApplyToConfig()
    {
        _config.Theme = _themeCombo.SelectedItem as string ?? "catppuccin-mocha";
        _config.Locale = _localeCombo.SelectedItem as string ?? "en";
        _config.DetailPanelHeight = (int)_detailHeight.Value;

        _config.MinimizeToTray = _chkMinimizeToTray.Checked;
        _config.ConfirmApply = _chkConfirmApply.Checked;
        _config.ConfirmRemove = _chkConfirmRemove.Checked;
        _config.ShowInapplicable = _chkShowInapplicable.Checked;
        _config.ForceCorp = _chkForceCorp.Checked;

        _config.MaxWorkers = (int)_maxWorkers.Value;
        _config.StatusBarMonitor = _chkStatusMonitor.Checked;

        _config.AutoBackup = _chkAutoBackup.Checked;
        _config.CheckToolUpdates = _chkToolUpdates.Checked;
        _config.BackupDir = _backupDir.Text.Trim();

        _themeChanged = _config.Theme != (AppTheme.CurrentThemeName());
        _localeChanged = _config.Locale != Locale.CurrentLocale;

        _config.Save();
        return true;
    }

    internal bool ThemeChanged => _themeChanged;
    internal bool LocaleChanged => _localeChanged;

    private void OnBrowseBackup(object? sender, EventArgs e)
    {
        using var dlg = new FolderBrowserDialog { Description = "Select backup directory", ShowNewFolderButton = true };
        if (!string.IsNullOrEmpty(_backupDir.Text) && Directory.Exists(_backupDir.Text))
            dlg.InitialDirectory = _backupDir.Text;

        if (dlg.ShowDialog(this) == DialogResult.OK)
            _backupDir.Text = dlg.SelectedPath;
    }

    private void OnRestoreDefaults(object? sender, EventArgs e)
    {
        var defaults = new AppConfig();
        _themeCombo.SelectedItem = defaults.Theme;
        _localeCombo.SelectedItem = defaults.Locale;
        _detailHeight.Value = defaults.DetailPanelHeight;
        _chkMinimizeToTray.Checked = defaults.MinimizeToTray;
        _chkConfirmApply.Checked = defaults.ConfirmApply;
        _chkConfirmRemove.Checked = defaults.ConfirmRemove;
        _chkShowInapplicable.Checked = defaults.ShowInapplicable;
        _chkForceCorp.Checked = defaults.ForceCorp;
        _maxWorkers.Value = defaults.MaxWorkers;
        _chkStatusMonitor.Checked = defaults.StatusBarMonitor;
        _chkAutoBackup.Checked = defaults.AutoBackup;
        _chkToolUpdates.Checked = defaults.CheckToolUpdates;
        _backupDir.Text = defaults.BackupDir;
    }

    private void ApplyTheme()
    {
        BackColor = AppTheme.Bg;
        ForeColor = AppTheme.Fg;
        AppTheme.Apply(this);

        _tabs.BackColor = AppTheme.Bg;
        foreach (TabPage page in _tabs.TabPages)
        {
            page.BackColor = AppTheme.Bg;
            page.ForeColor = AppTheme.Fg;
        }

        _btnOk.BackColor = AppTheme.Accent;
        _btnOk.ForeColor = AppTheme.Bg;
        _btnOk.FlatStyle = FlatStyle.Flat;
        _btnCancel.BackColor = AppTheme.Surface;
        _btnCancel.ForeColor = AppTheme.Fg;
        _btnCancel.FlatStyle = FlatStyle.Flat;
        _btnDefaults.BackColor = AppTheme.Surface;
        _btnDefaults.ForeColor = AppTheme.Fg;
        _btnDefaults.FlatStyle = FlatStyle.Flat;
    }

    private static Label CreateLabel(string text) =>
        new()
        {
            Text = text,
            AutoSize = true,
            Anchor = AnchorStyles.Left,
            Padding = new Padding(0, 4, 0, 0),
        };
}
