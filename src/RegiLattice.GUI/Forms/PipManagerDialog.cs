using RegiLattice.GUI.PackageManagers;

namespace RegiLattice.GUI.Forms;

/// <summary>pip (Python) package manager dialog.</summary>
internal sealed class PipManagerDialog : Form
{
    private readonly ListBox _lstInstalled = new();
    private readonly TextBox _txtName = new();
    private readonly Label _lblStatus = new();
    private readonly Button _btnRefresh = new();
    private readonly Button _btnInstall = new();
    private readonly Button _btnRemove = new();
    private readonly Button _btnUpgrade = new();

    private CancellationTokenSource _cts = new();

    internal PipManagerDialog()
    {
        Text = "pip Package Manager";
        FormBorderStyle = FormBorderStyle.Sizable;
        StartPosition = FormStartPosition.CenterParent;
        MinimumSize = new Size(560, 500);
        ClientSize = new Size(600, 580);
        BackColor = AppTheme.Bg;
        ForeColor = AppTheme.Fg;
        Font = AppTheme.Regular;

        BuildLayout();
        Load += async (_, _) => await RefreshAsync();
        FormClosed += (_, _) => _cts.Cancel();
    }

    private void BuildLayout()
    {
        var lblTitle = new Label
        {
            Text = "pip Package Manager",
            Font = AppTheme.Title,
            ForeColor = AppTheme.Accent,
            Dock = DockStyle.Top,
            Height = 36,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(8, 0, 0, 0),
        };

        bool installed = PipManager.IsPipInstalled();
        _lblStatus.Text = installed ? "pip: installed" : "pip: NOT FOUND (install Python first)";
        _lblStatus.ForeColor = installed ? AppTheme.Green : AppTheme.Red;
        _lblStatus.Dock = DockStyle.Top;
        _lblStatus.Height = 24;
        _lblStatus.Padding = new Padding(8, 0, 0, 0);

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
        foreach (string pkg in PipManager.PopularPackages)
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

        Controls.AddRange([ctrlPanel, flowQuick, quickLabel, _lstInstalled, _lblStatus, lblTitle]);
    }

    private async Task RefreshAsync()
    {
        SetBusy(true, "Loading...");
        try
        {
            var list = await PipManager.ListInstalledAsync(_cts.Token);
            _lstInstalled.Items.Clear();
            foreach (var p in list) _lstInstalled.Items.Add(p);
            _lblStatus.Text = $"pip: {list.Count} package(s) installed";
            _lblStatus.ForeColor = AppTheme.Green;
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
            await PipManager.InstallAsync(name, _cts.Token);
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
            await PipManager.UninstallAsync(name, _cts.Token);
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
            await PipManager.UpgradeAsync(name, _cts.Token);
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
}
