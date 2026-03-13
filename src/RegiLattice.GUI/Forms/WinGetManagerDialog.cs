using RegiLattice.GUI.PackageManagers;

namespace RegiLattice.GUI.Forms;

/// <summary>WinGet (Windows Package Manager) dialog.</summary>
internal sealed class WinGetManagerDialog : Form
{
    private readonly ListBox _lstInstalled = new();
    private readonly TextBox _txtName = new();
    private readonly Label _lblStatus = new();
    private readonly Label _lblOutdated = new();
    private readonly Button _btnRefresh = new();
    private readonly Button _btnInstall = new();
    private readonly Button _btnRemove = new();
    private readonly Button _btnUpgrade = new();
    private readonly Button _btnSearch = new();

    private CancellationTokenSource _cts = new();
    private bool _prereqMet;

    internal WinGetManagerDialog()
    {
        Text = "📦 WinGet Package Manager";
        Icon = AppIcons.WinGetIcon;
        FormBorderStyle = FormBorderStyle.Sizable;
        StartPosition = FormStartPosition.CenterParent;
        MinimumSize = new Size(580, 520);
        ClientSize = new Size(640, 600);
        BackColor = AppTheme.Bg;
        ForeColor = AppTheme.Fg;
        Font = AppTheme.Regular;

        BuildLayout();
        Load += async (_, _) => { if (_prereqMet) await RefreshAsync(); };
        FormClosed += (_, _) => _cts.Cancel();
    }

    private void BuildLayout()
    {
        var lblTitle = new Label
        {
            Text = "WinGet Package Manager",
            Font = AppTheme.Title,
            ForeColor = AppTheme.Accent,
            Dock = DockStyle.Top,
            Height = 36,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(8, 0, 0, 0),
        };

        bool installed = WinGetManager.IsWinGetInstalled();
        _prereqMet = installed;
        var prereqPanel = BuildPrereqPanel();
        _lblStatus.Dock = DockStyle.Top;
        _lblStatus.Height = 24;
        _lblStatus.Padding = new Padding(8, 0, 0, 0);

        _lblOutdated.Text = "";
        _lblOutdated.ForeColor = AppTheme.Yellow;
        _lblOutdated.Dock = DockStyle.Top;
        _lblOutdated.Height = 22;
        _lblOutdated.Padding = new Padding(8, 0, 0, 0);
        _lblOutdated.Font = AppTheme.Regular;

        AppTheme.Apply(_lstInstalled);
        _lstInstalled.Dock = DockStyle.Fill;
        _lstInstalled.Font = AppTheme.Mono;
        _lstInstalled.SelectionMode = SelectionMode.One;
        _lstInstalled.SelectedIndexChanged += (_, _) =>
        {
            if (_lstInstalled.SelectedItem is string name)
                _txtName.Text = name;
        };

        var ctrlPanel = new Panel { Dock = DockStyle.Bottom, Height = 42, BackColor = AppTheme.Surface };
        _txtName.Width = 170;
        _txtName.BackColor = AppTheme.Overlay;
        _txtName.ForeColor = AppTheme.Fg;
        _txtName.PlaceholderText = "package id (e.g. Git.Git)";
        _txtName.Location = new Point(8, 8);
        _txtName.KeyDown += (_, e) => { if (e.KeyCode == Keys.Enter) { _ = InstallAsync(); e.Handled = true; } };

        void StyleBtn(Button b, Color bg, string t, int x)
        {
            b.Text = t; b.BackColor = bg; b.ForeColor = AppTheme.Bg;
            b.FlatStyle = FlatStyle.Flat; b.Location = new Point(x, 7); b.Size = new Size(75, 27);
        }

        StyleBtn(_btnInstall, AppTheme.Green, "Install", 186);
        StyleBtn(_btnRemove, AppTheme.Red, "Remove", 266);
        StyleBtn(_btnUpgrade, AppTheme.Yellow, "Upgrade", 346);
        StyleBtn(_btnSearch, AppTheme.Accent, "Search", 426);
        StyleBtn(_btnRefresh, AppTheme.Accent, "Refresh", 506);

        _btnInstall.Click += async (_, _) => await InstallAsync();
        _btnRemove.Click += async (_, _) => await RemoveAsync();
        _btnUpgrade.Click += async (_, _) => await UpgradeAsync();
        _btnSearch.Click += async (_, _) => await SearchAsync();
        _btnRefresh.Click += async (_, _) => await RefreshAsync();

        ctrlPanel.Controls.AddRange([_txtName, _btnInstall, _btnRemove, _btnUpgrade, _btnSearch, _btnRefresh]);

        var quickLabel = new Label
        {
            Text = "Quick install:",
            ForeColor = AppTheme.FgDim,
            Dock = DockStyle.Top,
            Height = 22,
            Padding = new Padding(8, 4, 0, 0),
        };

        var flowQuick = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 80,
            BackColor = AppTheme.Surface,
            Padding = new Padding(4),
            AutoScroll = true,
        };
        foreach (string pkg in WinGetManager.PopularPackages)
        {
            string captured = pkg;
            var btn = new Button
            {
                Text = captured,
                BackColor = AppTheme.Overlay,
                ForeColor = AppTheme.Fg,
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(3),
                AutoSize = true,
            };
            btn.FlatAppearance.BorderColor = AppTheme.Accent;
            btn.Click += (_, _) => _txtName.Text = captured;
            flowQuick.Controls.Add(btn);
        }

        Controls.AddRange([_lstInstalled, ctrlPanel, flowQuick, quickLabel, _lblOutdated, _lblStatus, prereqPanel, lblTitle]);

        if (!_prereqMet)
            SetMainEnabled(false);
    }

    private async Task RefreshAsync()
    {
        SetBusy(true, "Loading installed packages...");
        try
        {
            var list = await WinGetManager.ListInstalledAsync(_cts.Token);
            _lstInstalled.Items.Clear();
            foreach (var p in list) _lstInstalled.Items.Add(p);
            _lblStatus.Text = $"winget: {list.Count} package(s) installed";
            _lblStatus.ForeColor = AppTheme.Green;
            _ = CheckOutdatedAsync();
        }
        catch (Exception ex)
        {
            _lblStatus.Text = $"Error: {ex.Message}";
            _lblStatus.ForeColor = AppTheme.Red;
        }
        finally { SetBusy(false); }
    }

    private async Task CheckOutdatedAsync()
    {
        try
        {
            var outdated = await WinGetManager.ListOutdatedAsync(_cts.Token);
            _lblOutdated.Text = outdated.Count > 0
                ? $"\u26A0 {outdated.Count} update(s) available"
                : "\u2714 All packages up to date";
            _lblOutdated.ForeColor = outdated.Count > 0 ? AppTheme.Yellow : AppTheme.Green;
        }
        catch
        {
            _lblOutdated.Text = "";
        }
    }

    private async Task SearchAsync()
    {
        string query = _txtName.Text.Trim();
        if (query.Length == 0) return;
        SetBusy(true, $"Searching for '{query}'...");
        try
        {
            var list = await WinGetManager.SearchAsync(query, _cts.Token);
            _lstInstalled.Items.Clear();
            foreach (var p in list) _lstInstalled.Items.Add(p);
            _lblStatus.Text = $"Search: {list.Count} result(s) for '{query}'";
            _lblStatus.ForeColor = AppTheme.Accent;
        }
        catch (Exception ex)
        {
            _lblStatus.Text = $"Error: {ex.Message}";
            _lblStatus.ForeColor = AppTheme.Red;
        }
        finally { SetBusy(false); }
    }

    private async Task InstallAsync()
    {
        string name = _txtName.Text.Trim();
        if (name.Length == 0) return;
        SetBusy(true, $"Installing {name}...");
        try
        {
            await WinGetManager.InstallAsync(name, _cts.Token);
            await RefreshAsync();
        }
        catch (Exception ex)
        {
            _lblStatus.Text = $"Error: {ex.Message}";
            _lblStatus.ForeColor = AppTheme.Red;
        }
        finally { SetBusy(false); }
    }

    private async Task RemoveAsync()
    {
        string name = (_lstInstalled.SelectedItem as string ?? _txtName.Text).Trim();
        if (name.Length == 0) return;
        SetBusy(true, $"Removing {name}...");
        try
        {
            await WinGetManager.UninstallAsync(name, _cts.Token);
            await RefreshAsync();
        }
        catch (Exception ex)
        {
            _lblStatus.Text = $"Error: {ex.Message}";
            _lblStatus.ForeColor = AppTheme.Red;
        }
        finally { SetBusy(false); }
    }

    private async Task UpgradeAsync()
    {
        string name = (_lstInstalled.SelectedItem as string ?? _txtName.Text).Trim();
        if (name.Length == 0) return;
        SetBusy(true, $"Upgrading {name}...");
        try
        {
            await WinGetManager.UpgradeAsync(name, _cts.Token);
            await RefreshAsync();
        }
        catch (Exception ex)
        {
            _lblStatus.Text = $"Error: {ex.Message}";
            _lblStatus.ForeColor = AppTheme.Red;
        }
        finally { SetBusy(false); }
    }

    private void SetBusy(bool busy, string? message = null)
    {
        _btnInstall.Enabled = !busy;
        _btnRemove.Enabled = !busy;
        _btnUpgrade.Enabled = !busy;
        _btnSearch.Enabled = !busy;
        _btnRefresh.Enabled = !busy;
        if (message is not null)
            _lblStatus.Text = message;
    }

    private void SetMainEnabled(bool enabled)
    {
        _btnInstall.Enabled = enabled;
        _btnRemove.Enabled = enabled;
        _btnUpgrade.Enabled = enabled;
        _btnSearch.Enabled = enabled;
        _btnRefresh.Enabled = enabled;
        _txtName.Enabled = enabled;
    }

    private Panel BuildPrereqPanel()
    {
        var panel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 34,
            BackColor = _prereqMet ? Color.FromArgb(20, AppTheme.Green) : Color.FromArgb(20, AppTheme.Red),
            Padding = new Padding(8, 4, 8, 4),
        };

        var lbl = new Label
        {
            Text = _prereqMet ? "\u2705 WinGet is installed and ready" : "\u274C WinGet is not available",
            AutoSize = true,
            Location = new Point(8, 8),
            ForeColor = _prereqMet ? AppTheme.Green : AppTheme.Red,
        };
        panel.Controls.Add(lbl);

        if (!_prereqMet)
        {
            var btn = new Button
            {
                Text = "Install WinGet",
                BackColor = AppTheme.Accent,
                ForeColor = AppTheme.Bg,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(120, 25),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
            };
            btn.Location = new Point(panel.ClientSize.Width - btn.Width - 8, 4);
            btn.Click += async (_, _) =>
            {
                btn.Enabled = false;
                btn.Text = "Installing...";
                lbl.Text = "\u23F3 Registering App Installer...";
                lbl.ForeColor = AppTheme.Yellow;
                panel.BackColor = Color.FromArgb(20, AppTheme.Yellow);
                try
                {
                    await WinGetManager.InstallWinGetAsync(_cts.Token);
                    _prereqMet = true;
                    lbl.Text = "\u2705 WinGet installed successfully";
                    lbl.ForeColor = AppTheme.Green;
                    panel.BackColor = Color.FromArgb(20, AppTheme.Green);
                    btn.Visible = false;
                    SetMainEnabled(true);
                    await RefreshAsync();
                }
                catch (Exception ex)
                {
                    lbl.Text = $"\u274C Install failed: {ex.Message}";
                    lbl.ForeColor = AppTheme.Red;
                    panel.BackColor = Color.FromArgb(20, AppTheme.Red);
                    btn.Enabled = true;
                    btn.Text = "Retry";
                }
            };
            panel.Controls.Add(btn);
        }

        return panel;
    }
}
