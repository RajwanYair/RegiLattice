using RegiLattice.Native.PackageManagers;

namespace RegiLattice.Native.Forms;

/// <summary>pip package manager dialog.</summary>
internal sealed class PipManagerDialog : Form
{
    private readonly string        _pythonExe;
    private readonly ListBox       _lstInstalled = new();
    private readonly TextBox       _txtName      = new();
    private readonly CheckBox      _chkUser      = new();
    private readonly Label         _lblStatus    = new();
    private readonly Button        _btnRefresh   = new();
    private readonly Button        _btnInstall   = new();
    private readonly Button        _btnRemove    = new();
    private readonly Button        _btnUpdate    = new();

    private CancellationTokenSource _cts = new();

    internal PipManagerDialog(string pythonExe)
    {
        _pythonExe      = pythonExe;
        Text            = "pip Package Manager";
        FormBorderStyle = FormBorderStyle.Sizable;
        StartPosition   = FormStartPosition.CenterParent;
        MinimumSize     = new Size(560, 500);
        ClientSize      = new Size(600, 580);
        BackColor       = AppTheme.Bg;
        ForeColor       = AppTheme.Fg;
        Font            = AppTheme.Regular;

        BuildLayout();
        Load        += async (_, _) => await RefreshAsync();
        FormClosed  += (_, _) => _cts.Cancel();
    }

    // ── Layout ──────────────────────────────────────────────────────────
    private void BuildLayout()
    {
        var lblTitle = new Label
        {
            Text      = "pip Package Manager",
            Font      = AppTheme.Title,
            ForeColor = AppTheme.Accent,
            Dock      = DockStyle.Top,
            Height    = 36,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding   = new Padding(8, 0, 0, 0),
        };

        _lblStatus.Text      = $"Python: {_pythonExe}";
        _lblStatus.ForeColor = AppTheme.FgDim;
        _lblStatus.Dock      = DockStyle.Top;
        _lblStatus.Height    = 24;
        _lblStatus.Padding   = new Padding(8, 0, 0, 0);

        // Scope row
        var scopePanel = new Panel { Dock = DockStyle.Top, Height = 34, BackColor = AppTheme.Surface, Padding = new Padding(8, 4, 8, 4) };
        _chkUser.Text      = "User install only (--user)";
        _chkUser.Checked   = true;
        _chkUser.ForeColor = AppTheme.Fg;
        _chkUser.BackColor = AppTheme.Surface;
        _chkUser.Location  = new Point(8, 7);
        _chkUser.AutoSize  = true;
        _chkUser.CheckedChanged += async (_, _) => await RefreshAsync();
        scopePanel.Controls.Add(_chkUser);

        // Installed list
        AppTheme.Apply(_lstInstalled);
        _lstInstalled.Dock          = DockStyle.Fill;
        _lstInstalled.Font          = AppTheme.Mono;
        _lstInstalled.SelectionMode = SelectionMode.One;
        _lstInstalled.SelectedIndexChanged += (_, _) =>
        {
            if (_lstInstalled.SelectedItem is string name) _txtName.Text = name;
        };

        // Control panel
        var ctrlPanel = new Panel { Dock = DockStyle.Bottom, Height = 42, BackColor = AppTheme.Surface };
        _txtName.Width           = 190;
        _txtName.BackColor       = AppTheme.Overlay;
        _txtName.ForeColor       = AppTheme.Fg;
        _txtName.PlaceholderText = "package name";
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

        // Quick-install
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
        foreach (string pkg in PipManager.PopularPackages)
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

        Controls.AddRange([ctrlPanel, flowQuick, quickLabel, _lstInstalled, scopePanel, _lblStatus, lblTitle]);
    }

    // ── Operations ──────────────────────────────────────────────────────
    private async Task RefreshAsync()
    {
        bool userOnly = _chkUser.Checked;
        SetBusy(true, "Loading packages...");
        try
        {
            var list = await PipManager.ListInstalledAsync(_pythonExe, userOnly, _cts.Token);
            _lstInstalled.Items.Clear();
            foreach (var p in list) _lstInstalled.Items.Add(p);
            _lblStatus.Text      = $"Python: {System.IO.Path.GetFileName(_pythonExe)} — {list.Count} package(s)";
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
        string name     = _txtName.Text.Trim();
        bool   userOnly = _chkUser.Checked;
        if (name.Length == 0) return;
        SetBusy(true, $"Installing {name}...");
        try
        {
            await PipManager.InstallAsync(name, _pythonExe, userOnly, _cts.Token);
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
            await PipManager.UninstallAsync(name, _pythonExe, _cts.Token);
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
        string name     = (_lstInstalled.SelectedItem as string ?? _txtName.Text).Trim();
        bool   userOnly = _chkUser.Checked;
        if (name.Length == 0) return;
        SetBusy(true, $"Updating {name}...");
        try
        {
            await PipManager.UpdateAsync(name, _pythonExe, userOnly, _cts.Token);
            await RefreshAsync();
        }
        catch (Exception ex)
        {
            _lblStatus.Text      = $"Error: {ex.Message}";
            _lblStatus.ForeColor = AppTheme.Red;
        }
        finally { SetBusy(false); }
    }

    private void SetBusy(bool busy, string msg = "")
    {
        _btnInstall.Enabled = !busy;
        _btnRemove.Enabled  = !busy;
        _btnUpdate.Enabled  = !busy;
        _btnRefresh.Enabled = !busy;
        _chkUser.Enabled    = !busy;
        if (msg.Length > 0) _lblStatus.Text = msg;
    }
}
