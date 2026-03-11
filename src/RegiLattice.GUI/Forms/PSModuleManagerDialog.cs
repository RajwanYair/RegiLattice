using RegiLattice.GUI.PackageManagers;

namespace RegiLattice.GUI.Forms;

/// <summary>PowerShell module manager dialog (via PSGet / Install-Module).</summary>
internal sealed class PSModuleManagerDialog : Form
{
    private readonly ListBox  _lstInstalled = new();
    private readonly TextBox  _txtName      = new();
    private readonly ComboBox _cmbScope     = new();
    private readonly Label    _lblStatus    = new();
    private readonly Button   _btnRefresh   = new();
    private readonly Button   _btnInstall   = new();
    private readonly Button   _btnRemove    = new();
    private readonly Button   _btnUpdate    = new();

    private CancellationTokenSource _cts = new();

    internal PSModuleManagerDialog()
    {
        Text            = "PowerShell Modules Manager";
        FormBorderStyle = FormBorderStyle.Sizable;
        StartPosition   = FormStartPosition.CenterParent;
        MinimumSize     = new Size(560, 500);
        ClientSize      = new Size(600, 580);
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
            Text      = "PowerShell Modules Manager",
            Font      = AppTheme.Title,
            ForeColor = AppTheme.Accent,
            Dock      = DockStyle.Top,
            Height    = 36,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding   = new Padding(8, 0, 0, 0),
        };

        _lblStatus.Text      = "Ready";
        _lblStatus.ForeColor = AppTheme.FgDim;
        _lblStatus.Dock      = DockStyle.Top;
        _lblStatus.Height    = 24;
        _lblStatus.Padding   = new Padding(8, 0, 0, 0);

        var scopePanel = new Panel { Dock = DockStyle.Top, Height = 34, BackColor = AppTheme.Surface, Padding = new Padding(8, 4, 8, 4) };
        var lblScope   = new Label { Text = "Scope:", AutoSize = true, Location = new Point(8, 8), ForeColor = AppTheme.Fg };
        _cmbScope.DropDownStyle = ComboBoxStyle.DropDownList;
        _cmbScope.Items.AddRange(["CurrentUser", "AllUsers"]);
        _cmbScope.SelectedIndex = 0;
        _cmbScope.BackColor     = AppTheme.Overlay;
        _cmbScope.ForeColor     = AppTheme.Fg;
        _cmbScope.FlatStyle     = FlatStyle.Flat;
        _cmbScope.Location      = new Point(58, 5);
        _cmbScope.Width         = 130;
        _cmbScope.SelectedIndexChanged += async (_, _) => await RefreshAsync();
        scopePanel.Controls.AddRange([lblScope, _cmbScope]);

        AppTheme.Apply(_lstInstalled);
        _lstInstalled.Dock          = DockStyle.Fill;
        _lstInstalled.Font          = AppTheme.Mono;
        _lstInstalled.SelectionMode = SelectionMode.One;
        _lstInstalled.SelectedIndexChanged += (_, _) =>
        {
            if (_lstInstalled.SelectedItem is string name) _txtName.Text = name;
        };

        var ctrlPanel = new Panel { Dock = DockStyle.Bottom, Height = 42, BackColor = AppTheme.Surface };
        _txtName.Width           = 190;
        _txtName.BackColor       = AppTheme.Overlay;
        _txtName.ForeColor       = AppTheme.Fg;
        _txtName.PlaceholderText = "module name";
        _txtName.Location        = new Point(8, 8);
        _txtName.KeyDown        += (_, e) => { if (e.KeyCode == Keys.Enter) { _ = InstallAsync(); e.Handled = true; } };

        void StyleBtn(Button b, Color bg, string t, int x)
        {
            b.Text = t; b.BackColor = bg; b.ForeColor = AppTheme.Bg;
            b.FlatStyle = FlatStyle.Flat; b.Location = new Point(x, 7); b.Size = new Size(75, 27);
        }

        StyleBtn(_btnInstall, AppTheme.Green,  "Install", 206);
        StyleBtn(_btnRemove,  AppTheme.Red,    "Remove",  286);
        StyleBtn(_btnUpdate,  AppTheme.Yellow, "Update",  366);
        StyleBtn(_btnRefresh, AppTheme.Accent, "Refresh", 446);

        _btnInstall.Click += async (_, _) => await InstallAsync();
        _btnRemove.Click  += async (_, _) => await RemoveAsync();
        _btnUpdate.Click  += async (_, _) => await UpdateAsync();
        _btnRefresh.Click += async (_, _) => await RefreshAsync();

        ctrlPanel.Controls.AddRange([_txtName, _btnInstall, _btnRemove, _btnUpdate, _btnRefresh]);

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
        foreach (string mod in PSModuleManager.PopularModules)
        {
            string captured = mod;
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

        Controls.AddRange([ctrlPanel, flowQuick, quickLabel, _lstInstalled, scopePanel, _lblStatus, lblTitle]);
    }

    private async Task RefreshAsync()
    {
        string scope = _cmbScope.SelectedItem?.ToString() ?? "CurrentUser";
        SetBusy(true, "Loading modules...");
        try
        {
            var list = await PSModuleManager.ListInstalledAsync(scope, _cts.Token);
            _lstInstalled.Items.Clear();
            foreach (var m in list) _lstInstalled.Items.Add(m);
            _lblStatus.Text      = $"Scope: {scope} - {list.Count} module(s)";
            _lblStatus.ForeColor = AppTheme.Fg;
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
        string name  = _txtName.Text.Trim();
        string scope = _cmbScope.SelectedItem?.ToString() ?? "CurrentUser";
        if (name.Length == 0) return;
        SetBusy(true, $"Installing {name} ({scope})...");
        try
        {
            await PSModuleManager.InstallAsync(name, scope, _cts.Token);
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
            await PSModuleManager.RemoveAsync(name, _cts.Token);
            await RefreshAsync();
        }
        catch (Exception ex)
        {
            _lblStatus.Text      = $"Error: {ex.Message}";
            _lblStatus.ForeColor = AppTheme.Red;
        }
        finally { SetBusy(false); }
    }

    private async Task UpdateAsync()
    {
        string name  = (_lstInstalled.SelectedItem as string ?? _txtName.Text).Trim();
        string scope = _cmbScope.SelectedItem?.ToString() ?? "CurrentUser";
        if (name.Length == 0) return;
        SetBusy(true, $"Updating {name} ({scope})...");
        try
        {
            await PSModuleManager.UpdateAsync(name, scope, _cts.Token);
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
        _btnUpdate.Enabled  = !busy;
        _btnRefresh.Enabled = !busy;
        if (message is not null)
            _lblStatus.Text = message;
    }
}
