using RegiLattice.Core.Plugins;

namespace RegiLattice.GUI.Forms;

/// <summary>Tweak Pack marketplace browser and manager dialog.</summary>
internal sealed class MarketplaceDialog : Form
{
    private readonly TabControl _tabs = new();
    private readonly ListView _lstBrowse = new();
    private readonly ListView _lstInstalled = new();
    private readonly TextBox _txtSearch = new();
    private readonly Label _lblStatus = new();
    private readonly Button _btnInstall = new();
    private readonly Button _btnUninstall = new();
    private readonly Button _btnUpdate = new();
    private readonly Button _btnRefresh = new();
    private readonly Button _btnInstallFile = new();
    private readonly RichTextBox _txtDetails = new();

    private readonly PackManager _pm = new();
    private CancellationTokenSource _cts = new();

    internal MarketplaceDialog()
    {
        Text = "\U0001F4E6 Tweak Pack Marketplace";
        FormBorderStyle = FormBorderStyle.Sizable;
        StartPosition = FormStartPosition.CenterParent;
        MinimumSize = new Size(700, 520);
        ClientSize = new Size(780, 600);
        BackColor = AppTheme.Bg;
        ForeColor = AppTheme.Fg;
        Font = AppTheme.Regular;

        BuildLayout();
        Load += async (_, _) => await RefreshBrowseAsync();
        FormClosed += (_, _) => _cts.Cancel();
    }

    private void BuildLayout()
    {
        var lblTitle = new Label
        {
            Text = "Tweak Pack Marketplace",
            Font = AppTheme.Title,
            ForeColor = AppTheme.Accent,
            Dock = DockStyle.Top,
            Height = 36,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(8, 0, 0, 0),
        };

        _lblStatus.Text = "Loading marketplace…";
        _lblStatus.Dock = DockStyle.Top;
        _lblStatus.Height = 22;
        _lblStatus.Padding = new Padding(8, 0, 0, 0);

        // ── Browse tab ─────────────────────────────────────────────────
        var browseTab = new TabPage("Browse") { BackColor = AppTheme.Bg };
        var browsePanel = new Panel { Dock = DockStyle.Fill };

        _txtSearch.Dock = DockStyle.Top;
        _txtSearch.Height = 28;
        _txtSearch.PlaceholderText = "Search packs…";
        _txtSearch.BackColor = AppTheme.Overlay;
        _txtSearch.ForeColor = AppTheme.Fg;
        _txtSearch.TextChanged += async (_, _) => await RefreshBrowseAsync();

        ConfigureListView(_lstBrowse);
        _lstBrowse.Columns.Add("Name", 180);
        _lstBrowse.Columns.Add("Version", 80);
        _lstBrowse.Columns.Add("Tweaks", 65);
        _lstBrowse.Columns.Add("Author", 120);
        _lstBrowse.Columns.Add("Description", 280);
        _lstBrowse.Dock = DockStyle.Fill;
        _lstBrowse.SelectedIndexChanged += OnBrowseSelectionChanged;

        var browseButtons = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            Height = 42,
            FlowDirection = FlowDirection.LeftToRight,
            BackColor = AppTheme.Surface,
            Padding = new Padding(4),
        };
        StyleButton(_btnInstall, "\u2714 Install");
        StyleButton(_btnRefresh, "\U0001F504 Refresh");
        StyleButton(_btnInstallFile, "\U0001F4C1 Install from File…");
        _btnInstall.Click += async (_, _) => await InstallSelectedAsync();
        _btnRefresh.Click += async (_, _) => await RefreshBrowseAsync();
        _btnInstallFile.Click += OnInstallFromFile;
        browseButtons.Controls.AddRange([_btnInstall, _btnRefresh, _btnInstallFile]);

        browsePanel.Controls.Add(_lstBrowse);
        browsePanel.Controls.Add(_txtSearch);
        browseTab.Controls.Add(browsePanel);
        browseTab.Controls.Add(browseButtons);

        // ── Installed tab ──────────────────────────────────────────────
        var installedTab = new TabPage("Installed") { BackColor = AppTheme.Bg };
        var installedPanel = new Panel { Dock = DockStyle.Fill };

        ConfigureListView(_lstInstalled);
        _lstInstalled.Columns.Add("Name", 180);
        _lstInstalled.Columns.Add("Version", 80);
        _lstInstalled.Columns.Add("Tweaks", 65);
        _lstInstalled.Columns.Add("Author", 120);
        _lstInstalled.Columns.Add("Description", 280);
        _lstInstalled.Dock = DockStyle.Fill;
        _lstInstalled.SelectedIndexChanged += OnInstalledSelectionChanged;

        var installedButtons = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            Height = 42,
            FlowDirection = FlowDirection.LeftToRight,
            BackColor = AppTheme.Surface,
            Padding = new Padding(4),
        };
        StyleButton(_btnUninstall, "\u2716 Uninstall");
        StyleButton(_btnUpdate, "\u2B06 Update");
        _btnUninstall.Click += OnUninstallSelected;
        _btnUpdate.Click += async (_, _) => await UpdateSelectedAsync();
        installedButtons.Controls.AddRange([_btnUninstall, _btnUpdate]);

        installedPanel.Controls.Add(_lstInstalled);
        installedTab.Controls.Add(installedPanel);
        installedTab.Controls.Add(installedButtons);

        // ── Details panel (bottom) ─────────────────────────────────────
        _txtDetails.Dock = DockStyle.Bottom;
        _txtDetails.Height = 110;
        _txtDetails.ReadOnly = true;
        _txtDetails.BackColor = AppTheme.Surface;
        _txtDetails.ForeColor = AppTheme.FgDim;
        _txtDetails.Font = AppTheme.Mono;
        _txtDetails.BorderStyle = BorderStyle.None;

        // ── Tabs ───────────────────────────────────────────────────────
        _tabs.Dock = DockStyle.Fill;
        _tabs.TabPages.AddRange([browseTab, installedTab]);
        _tabs.SelectedIndexChanged += async (_, _) =>
        {
            if (_tabs.SelectedIndex == 1)
                RefreshInstalled();
            else
                await RefreshBrowseAsync();
        };

        SuspendLayout();
        Controls.Add(_tabs);
        Controls.Add(_txtDetails);
        Controls.Add(_lblStatus);
        Controls.Add(lblTitle);
        ResumeLayout(true);
    }

    // ── Browse ─────────────────────────────────────────────────────────

    private async Task RefreshBrowseAsync()
    {
        _lblStatus.Text = "Fetching marketplace index…";
        _lblStatus.ForeColor = AppTheme.Fg;
        _lstBrowse.Items.Clear();

        try
        {
            var query = _txtSearch.Text.Trim();
            IReadOnlyList<PackDef> packs;

            if (string.IsNullOrWhiteSpace(query))
            {
                var index = await _pm.FetchIndexAsync(_cts.Token);
                packs = index.Packs;
            }
            else
            {
                packs = await _pm.SearchPacksAsync(query, _cts.Token);
            }

            _lstBrowse.BeginUpdate();
            foreach (var p in packs)
            {
                var item = new ListViewItem(p.Name) { Tag = p };
                item.SubItems.AddRange([p.Version, p.TweakCount.ToString(), p.Author, p.Description]);
                _lstBrowse.Items.Add(item);
            }
            _lstBrowse.EndUpdate();

            _lblStatus.Text = $"{packs.Count} packs available.";
            _lblStatus.ForeColor = AppTheme.Green;
        }
        catch (HttpRequestException ex)
        {
            _lblStatus.Text = $"Network error: {ex.Message}";
            _lblStatus.ForeColor = AppTheme.Red;
        }
        catch (OperationCanceledException)
        {
            // Dialog closed during fetch
        }
    }

    private async Task InstallSelectedAsync()
    {
        if (_lstBrowse.SelectedItems.Count == 0) return;
        var pack = (PackDef)_lstBrowse.SelectedItems[0].Tag!;

        _lblStatus.Text = $"Installing '{pack.DisplayName}'…";
        _lblStatus.ForeColor = AppTheme.Fg;

        try
        {
            var (installed, tweaks) = await _pm.InstallPackAsync(pack.Name, _cts.Token);
            _lblStatus.Text = $"\u2705 Installed '{installed.DisplayName}' v{installed.Version} ({tweaks.Count} tweaks).";
            _lblStatus.ForeColor = AppTheme.Green;
        }
        catch (Exception ex) when (ex is ArgumentException or HttpRequestException or InvalidOperationException)
        {
            _lblStatus.Text = $"\u274c {ex.Message}";
            _lblStatus.ForeColor = AppTheme.Red;
        }
    }

    private void OnInstallFromFile(object? sender, EventArgs e)
    {
        using var dlg = new OpenFileDialog
        {
            Title = "Select Tweak Pack JSON",
            Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
        };
        if (dlg.ShowDialog(this) != DialogResult.OK) return;

        try
        {
            var (pack, tweaks) = _pm.InstallFromFile(dlg.FileName);
            _lblStatus.Text = $"\u2705 Installed '{pack.DisplayName}' v{pack.Version} ({tweaks.Count} tweaks).";
            _lblStatus.ForeColor = AppTheme.Green;
            RefreshInstalled();
        }
        catch (Exception ex) when (ex is InvalidOperationException or ArgumentException)
        {
            _lblStatus.Text = $"\u274c {ex.Message}";
            _lblStatus.ForeColor = AppTheme.Red;
        }
    }

    // ── Installed ──────────────────────────────────────────────────────

    private void RefreshInstalled()
    {
        _lstInstalled.Items.Clear();
        var installed = _pm.InstalledPacks();

        _lstInstalled.BeginUpdate();
        foreach (var p in installed)
        {
            var item = new ListViewItem(p.Name) { Tag = p };
            item.SubItems.AddRange([p.Version, p.TweakCount.ToString(), p.Author, p.Description]);
            _lstInstalled.Items.Add(item);
        }
        _lstInstalled.EndUpdate();

        _lblStatus.Text = $"{installed.Count} packs installed.";
        _lblStatus.ForeColor = installed.Count > 0 ? AppTheme.Green : AppTheme.Fg;
    }

    private void OnUninstallSelected(object? sender, EventArgs e)
    {
        if (_lstInstalled.SelectedItems.Count == 0) return;
        var pack = (PackDef)_lstInstalled.SelectedItems[0].Tag!;

        var result = MessageBox.Show(
            this,
            $"Uninstall '{pack.DisplayName}' v{pack.Version}?",
            "Confirm Uninstall",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);
        if (result != DialogResult.Yes) return;

        _pm.UninstallPack(pack.Name);
        _lblStatus.Text = $"Uninstalled '{pack.DisplayName}'.";
        _lblStatus.ForeColor = AppTheme.Green;
        RefreshInstalled();
    }

    private async Task UpdateSelectedAsync()
    {
        if (_lstInstalled.SelectedItems.Count == 0) return;
        var pack = (PackDef)_lstInstalled.SelectedItems[0].Tag!;

        _lblStatus.Text = $"Updating '{pack.DisplayName}'…";
        _lblStatus.ForeColor = AppTheme.Fg;

        try
        {
            var (updated, tweaks) = await _pm.UpdatePackAsync(pack.Name, _cts.Token);
            _lblStatus.Text = $"\u2705 Updated '{updated.DisplayName}' to v{updated.Version} ({tweaks.Count} tweaks).";
            _lblStatus.ForeColor = AppTheme.Green;
            RefreshInstalled();
        }
        catch (Exception ex) when (ex is ArgumentException or HttpRequestException)
        {
            _lblStatus.Text = $"\u274c {ex.Message}";
            _lblStatus.ForeColor = AppTheme.Red;
        }
    }

    // ── Details panel ──────────────────────────────────────────────────

    private void OnBrowseSelectionChanged(object? sender, EventArgs e)
    {
        if (_lstBrowse.SelectedItems.Count == 0) { _txtDetails.Clear(); return; }
        ShowPackDetails((PackDef)_lstBrowse.SelectedItems[0].Tag!);
    }

    private void OnInstalledSelectionChanged(object? sender, EventArgs e)
    {
        if (_lstInstalled.SelectedItems.Count == 0) { _txtDetails.Clear(); return; }
        ShowPackDetails((PackDef)_lstInstalled.SelectedItems[0].Tag!);
    }

    private void ShowPackDetails(PackDef p)
    {
        _txtDetails.Text =
            $"Name: {p.DisplayName}  (ID: {p.Name})\n" +
            $"Version: {p.Version}  |  Author: {p.Author}  |  Tweaks: {p.TweakCount}\n" +
            $"Categories: {string.Join(", ", p.Categories)}\n" +
            $"Tags: {string.Join(", ", p.Tags)}\n" +
            $"{p.Description}";
    }

    // ── Helpers ─────────────────────────────────────────────────────────

    private static void ConfigureListView(ListView lv)
    {
        lv.View = View.Details;
        lv.FullRowSelect = true;
        lv.MultiSelect = false;
        lv.HeaderStyle = ColumnHeaderStyle.Nonclickable;
        lv.BackColor = AppTheme.Bg;
        lv.ForeColor = AppTheme.Fg;
        lv.BorderStyle = BorderStyle.None;
    }

    private static void StyleButton(Button btn, string text)
    {
        btn.Text = text;
        btn.FlatStyle = FlatStyle.Flat;
        btn.BackColor = AppTheme.Surface;
        btn.ForeColor = AppTheme.Fg;
        btn.FlatAppearance.BorderColor = AppTheme.Accent;
        btn.Margin = new Padding(4, 4, 4, 4);
        btn.Height = 30;
        btn.Width = 140;
    }
}
