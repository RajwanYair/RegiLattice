using RegiLattice.GUI.PackageManagers;

namespace RegiLattice.GUI.Forms;

/// <summary>Chocolatey package manager dialog.</summary>
internal sealed class ChocolateyManagerDialog : Form
{
    private readonly ListBox _lstInstalled = new();
    private readonly TextBox _txtName = new();
    private readonly Label _lblStatus = new();
    private readonly Label _lblOutdated = new();
    private readonly Button _btnRefresh = new();
    private readonly Button _btnInstall = new();
    private readonly Button _btnRemove = new();
    private readonly Button _btnUpgrade = new();

    private CancellationTokenSource _cts = new();
    private bool _prereqMet;

    internal ChocolateyManagerDialog()
    {
        Text = "🍫 Chocolatey Package Manager";
        Icon = AppIcons.ChocolateyIcon;
        FormBorderStyle = FormBorderStyle.Sizable;
        StartPosition = FormStartPosition.CenterParent;
        MinimumSize = new Size(560, 500);
        ClientSize = new Size(600, 580);
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
            Text = "Chocolatey Package Manager",
            Font = AppTheme.Title,
            ForeColor = AppTheme.Accent,
            Dock = DockStyle.Top,
            Height = 36,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(8, 0, 0, 0),
        };

        bool installed = ChocolateyManager.IsChocoInstalled();
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
            {
                int paren = name.IndexOf(" (", StringComparison.Ordinal);
                _txtName.Text = paren > 0 ? name[..paren] : name;
            }
        };

        var ctrlPanel = new Panel { Dock = DockStyle.Bottom, Height = 42, BackColor = AppTheme.Surface };
        _txtName.Width = 180;
        _txtName.BackColor = AppTheme.Overlay;
        _txtName.ForeColor = AppTheme.Fg;
        _txtName.PlaceholderText = "package name";
        _txtName.Location = new Point(8, 8);
        _txtName.KeyDown += (_, e) => { if (e.KeyCode == Keys.Enter) { _ = InstallAsync(); e.Handled = true; } };

        void StyleBtn(Button b, Color bg, string t, int x)
        {
            b.Text = t; b.BackColor = bg; b.ForeColor = AppTheme.Bg;
            b.FlatStyle = FlatStyle.Flat; b.Location = new Point(x, 7); b.Size = new Size(75, 27);
        }

        StyleBtn(_btnInstall, AppTheme.Green, "Install", 196);
        StyleBtn(_btnRemove, AppTheme.Red, "Remove", 276);
        StyleBtn(_btnUpgrade, AppTheme.Yellow, "Upgrade", 356);
        StyleBtn(_btnRefresh, AppTheme.Accent, "Refresh", 436);

        _btnInstall.Click += async (_, _) => await InstallAsync();
        _btnRemove.Click += async (_, _) => await RemoveAsync();
        _btnUpgrade.Click += async (_, _) => await UpgradeAsync();
        _btnRefresh.Click += async (_, _) => await RefreshAsync();

        ctrlPanel.Controls.AddRange([_txtName, _btnInstall, _btnRemove, _btnUpgrade, _btnRefresh]);

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
        foreach (string pkg in ChocolateyManager.PopularPackages)
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
        SetBusy(true, "Loading...");
        try
        {
            var list = await ChocolateyManager.ListInstalledAsync(_cts.Token);
            _lstInstalled.Items.Clear();
            foreach (var p in list) _lstInstalled.Items.Add(p);
            _lblStatus.Text = $"Chocolatey: {list.Count} package(s) installed";
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
            var outdated = await ChocolateyManager.ListOutdatedAsync(_cts.Token);
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

    private async Task InstallAsync()
    {
        string name = _txtName.Text.Trim();
        if (name.Length == 0) return;
        SetBusy(true, $"Installing {name}...");
        try
        {
            await ChocolateyManager.InstallAsync(name, _cts.Token);
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
        string name = ExtractName();
        if (name.Length == 0) return;
        SetBusy(true, $"Removing {name}...");
        try
        {
            await ChocolateyManager.UninstallAsync(name, _cts.Token);
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
        string name = ExtractName();
        if (name.Length == 0) return;
        SetBusy(true, $"Upgrading {name}...");
        try
        {
            await ChocolateyManager.UpgradeAsync(name, _cts.Token);
            await RefreshAsync();
        }
        catch (Exception ex)
        {
            _lblStatus.Text = $"Error: {ex.Message}";
            _lblStatus.ForeColor = AppTheme.Red;
        }
        finally { SetBusy(false); }
    }

    private string ExtractName()
    {
        string raw = (_lstInstalled.SelectedItem as string ?? _txtName.Text).Trim();
        int paren = raw.IndexOf(" (", StringComparison.Ordinal);
        return paren > 0 ? raw[..paren] : raw;
    }

    private void SetBusy(bool busy, string? message = null)
    {
        _btnInstall.Enabled = !busy;
        _btnRemove.Enabled = !busy;
        _btnUpgrade.Enabled = !busy;
        _btnRefresh.Enabled = !busy;
        if (message is not null)
            _lblStatus.Text = message;
    }

    private void SetMainEnabled(bool enabled)
    {
        _btnInstall.Enabled = enabled;
        _btnRemove.Enabled = enabled;
        _btnUpgrade.Enabled = enabled;
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
            Text = _prereqMet ? "\u2705 Chocolatey is installed and ready" : "\u274C Chocolatey is not installed",
            AutoSize = true,
            Location = new Point(8, 8),
            ForeColor = _prereqMet ? AppTheme.Green : AppTheme.Red,
        };
        panel.Controls.Add(lbl);

        if (!_prereqMet)
        {
            var btn = new Button
            {
                Text = "Install Choco",
                BackColor = AppTheme.Accent,
                ForeColor = AppTheme.Bg,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(110, 25),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
            };
            btn.Location = new Point(panel.ClientSize.Width - btn.Width - 8, 4);
            btn.Click += async (_, _) =>
            {
                btn.Enabled = false;
                btn.Text = "Installing...";
                lbl.Text = "\u23F3 Installing Chocolatey (may need admin)...";
                lbl.ForeColor = AppTheme.Yellow;
                panel.BackColor = Color.FromArgb(20, AppTheme.Yellow);
                try
                {
                    await ChocolateyManager.InstallChocoAsync(_cts.Token);
                    _prereqMet = true;
                    lbl.Text = "\u2705 Chocolatey installed successfully";
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
