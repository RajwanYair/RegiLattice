using RegiLattice.GUI.PackageManagers;

namespace RegiLattice.GUI.Forms;

/// <summary>Scoop package manager dialog.</summary>
internal sealed class ScoopManagerDialog : Form
{
    private readonly ListBox _lstInstalled = new();
    private readonly TextBox _txtName      = new();
    private readonly Label   _lblStatus    = new();
    private readonly Button  _btnRefresh   = new();
    private readonly Button  _btnInstall   = new();
    private readonly Button  _btnRemove    = new();

    private CancellationTokenSource _cts = new();

    internal ScoopManagerDialog()
    {
        Text            = "Scoop Package Manager";
        FormBorderStyle = FormBorderStyle.Sizable;
        StartPosition   = FormStartPosition.CenterParent;
        MinimumSize     = new Size(540, 500);
        ClientSize      = new Size(580, 560);
        BackColor       = AppTheme.Bg;
        ForeColor       = AppTheme.Fg;
        Font            = AppTheme.Regular;

        BuildLayout();
        Load       += async (_, _) => await RefreshAsync();
        FormClosed += (_, _) => _cts.Cancel();
    }

    private void BuildLayout()
    {
        var lblTitle = new Label
        {
            Text      = "Scoop Package Manager",
            Font      = AppTheme.Title,
            ForeColor = AppTheme.Accent,
            Dock      = DockStyle.Top,
            Height    = 36,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding   = new Padding(8, 0, 0, 0),
        };

        _lblStatus.Text      = ScoopManager.IsScoopInstalled() ? "Scoop: installed" : "Scoop: NOT FOUND (install from scoop.sh)";
        _lblStatus.ForeColor = ScoopManager.IsScoopInstalled() ? AppTheme.Green : AppTheme.Red;
        _lblStatus.Dock      = DockStyle.Top;
        _lblStatus.Height    = 24;
        _lblStatus.Padding   = new Padding(8, 0, 0, 0);

        AppTheme.Apply(_lstInstalled);
        _lstInstalled.Dock          = DockStyle.Fill;
        _lstInstalled.Font          = AppTheme.Mono;
        _lstInstalled.SelectionMode = SelectionMode.One;
        _lstInstalled.SelectedIndexChanged += (_, _) =>
        {
            if (_lstInstalled.SelectedItem is string name)
                _txtName.Text = name;
        };

        var ctrlPanel = new Panel { Dock = DockStyle.Bottom, Height = 42, BackColor = AppTheme.Surface };
        _txtName.Width           = 200;
        _txtName.BackColor       = AppTheme.Overlay;
        _txtName.ForeColor       = AppTheme.Fg;
        _txtName.PlaceholderText = "package name";
        _txtName.Location        = new Point(8, 8);
        _txtName.KeyDown        += (_, e) => { if (e.KeyCode == Keys.Enter) { _ = InstallAsync(); e.Handled = true; } };

        _btnInstall.Text      = "Install";
        _btnInstall.BackColor = AppTheme.Green;
        _btnInstall.ForeColor = AppTheme.Bg;
        _btnInstall.FlatStyle = FlatStyle.Flat;
        _btnInstall.Location  = new Point(216, 7);
        _btnInstall.Size      = new Size(75, 27);
        _btnInstall.Click    += async (_, _) => await InstallAsync();

        _btnRemove.Text      = "Remove";
        _btnRemove.BackColor = AppTheme.Red;
        _btnRemove.ForeColor = AppTheme.Bg;
        _btnRemove.FlatStyle = FlatStyle.Flat;
        _btnRemove.Location  = new Point(296, 7);
        _btnRemove.Size      = new Size(75, 27);
        _btnRemove.Click    += async (_, _) => await RemoveAsync();

        _btnRefresh.Text      = "Refresh";
        _btnRefresh.BackColor = AppTheme.Accent;
        _btnRefresh.ForeColor = AppTheme.Bg;
        _btnRefresh.FlatStyle = FlatStyle.Flat;
        _btnRefresh.Location  = new Point(376, 7);
        _btnRefresh.Size      = new Size(75, 27);
        _btnRefresh.Click    += async (_, _) => await RefreshAsync();

        ctrlPanel.Controls.AddRange([_txtName, _btnInstall, _btnRemove, _btnRefresh]);

        var quickLabel = new Label
        {
            Text      = "Quick install:",
            ForeColor = AppTheme.FgDim,
            Dock      = DockStyle.Top,
            Height    = 22,
            Padding   = new Padding(8, 4, 0, 0),
        };

        var flowQuick = new FlowLayoutPanel
        {
            Dock       = DockStyle.Top,
            Height     = 80,
            BackColor  = AppTheme.Surface,
            Padding    = new Padding(4),
            AutoScroll = true,
        };
        foreach (string pkg in ScoopManager.PopularTools)
        {
            string captured = pkg;
            var btn = new Button
            {
                Text      = captured,
                BackColor = AppTheme.Overlay,
                ForeColor = AppTheme.Fg,
                FlatStyle = FlatStyle.Flat,
                Margin    = new Padding(3),
                AutoSize  = true,
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
            var list = await ScoopManager.ListInstalledAsync(_cts.Token);
            _lstInstalled.Items.Clear();
            foreach (var p in list) _lstInstalled.Items.Add(p);
            _lblStatus.Text      = list.Count > 0
                ? $"Scoop: installed - {list.Count} packages"
                : "Scoop: installed - no packages";
            _lblStatus.ForeColor = AppTheme.Green;
        }
        catch (Exception ex)
        {
            _lblStatus.Text      = $"Error: {ex.Message}";
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
            await ScoopManager.InstallAsync(name, _cts.Token);
            await RefreshAsync();
        }
        catch (Exception ex)
        {
            _lblStatus.Text      = $"Error: {ex.Message}";
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
            await ScoopManager.UninstallAsync(name, _cts.Token);
            await RefreshAsync();
        }
        catch (Exception ex)
        {
            _lblStatus.Text      = $"Error: {ex.Message}";
            _lblStatus.ForeColor = AppTheme.Red;
        }
        finally { SetBusy(false); }
    }

    private void SetBusy(bool busy, string? message = null)
    {
        _btnInstall.Enabled = !busy;
        _btnRemove.Enabled  = !busy;
        _btnRefresh.Enabled = !busy;
        if (message is not null)
            _lblStatus.Text = message;
    }
}
