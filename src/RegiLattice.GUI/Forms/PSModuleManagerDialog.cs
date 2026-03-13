using RegiLattice.GUI.PackageManagers;

namespace RegiLattice.GUI.Forms;

/// <summary>PowerShell module manager dialog with ListView, per-item outdated, close button.</summary>
internal sealed class PSModuleManagerDialog : Form
{
    private readonly ListView _lstInstalled = new();
    private readonly TextBox _txtName = new();
    private readonly ComboBox _cmbScope = new();
    private readonly Label _lblStatus = new();
    private readonly Label _lblOutdated = new();
    private readonly FlowLayoutPanel _flowQuick = new();
    private readonly Button _btnRefresh = new();
    private readonly Button _btnInstall = new();
    private readonly Button _btnRemove = new();
    private readonly Button _btnUpdate = new();
    private readonly Button _btnClose = new();

    private HashSet<string> _installedNames = new(StringComparer.OrdinalIgnoreCase);
    private HashSet<string> _outdatedNames = new(StringComparer.OrdinalIgnoreCase);
    private CancellationTokenSource _cts = new();
    private bool _prereqMet;

    internal PSModuleManagerDialog()
    {
        Text = "⚡ PowerShell Modules Manager";
        Icon = AppIcons.PSModuleIcon;
        FormBorderStyle = FormBorderStyle.Sizable;
        StartPosition = FormStartPosition.CenterParent;
        MinimumSize = new Size(600, 520);
        ClientSize = new Size(640, 600);
        BackColor = AppTheme.Bg;
        ForeColor = AppTheme.Fg;
        Font = AppTheme.Regular;

        BuildLayout();
        Load += async (_, _) =>
        {
            if (_prereqMet)
                await RefreshAsync();
        };
        FormClosed += (_, _) => _cts.Cancel();
    }

    private void BuildLayout()
    {
        var lblTitle = new Label
        {
            Text = "PowerShell Modules Manager",
            Font = AppTheme.Title,
            ForeColor = AppTheme.Accent,
            Dock = DockStyle.Top,
            Height = 36,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(8, 0, 0, 0),
        };

        _prereqMet = PSModuleManager.IsPowerShellGetAvailable();
        var prereqPanel = BuildPrereqPanel();

        _lblStatus.Text = "Ready";
        _lblStatus.ForeColor = AppTheme.FgDim;
        _lblStatus.Dock = DockStyle.Top;
        _lblStatus.Height = 24;
        _lblStatus.Padding = new Padding(8, 0, 0, 0);

        _lblOutdated.Text = "";
        _lblOutdated.ForeColor = AppTheme.Yellow;
        _lblOutdated.Dock = DockStyle.Top;
        _lblOutdated.Height = 22;
        _lblOutdated.Padding = new Padding(8, 0, 0, 0);
        _lblOutdated.Font = AppTheme.Regular;

        var scopePanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 34,
            BackColor = AppTheme.Surface,
            Padding = new Padding(8, 4, 8, 4),
        };
        var lblScope = new Label
        {
            Text = "Scope:",
            AutoSize = true,
            Location = new Point(8, 8),
            ForeColor = AppTheme.Fg,
        };
        _cmbScope.DropDownStyle = ComboBoxStyle.DropDownList;
        _cmbScope.Items.AddRange(["CurrentUser", "AllUsers"]);
        _cmbScope.SelectedIndex = 0;
        _cmbScope.BackColor = AppTheme.Overlay;
        _cmbScope.ForeColor = AppTheme.Fg;
        _cmbScope.FlatStyle = FlatStyle.Flat;
        _cmbScope.Location = new Point(58, 5);
        _cmbScope.Width = 130;
        _cmbScope.SelectedIndexChanged += async (_, _) => await RefreshAsync();
        scopePanel.Controls.AddRange([lblScope, _cmbScope]);

        // ListView with Module, Version, Status columns
        _lstInstalled.Dock = DockStyle.Fill;
        _lstInstalled.View = View.Details;
        _lstInstalled.FullRowSelect = true;
        _lstInstalled.GridLines = false;
        _lstInstalled.MultiSelect = false;
        _lstInstalled.BackColor = AppTheme.Bg;
        _lstInstalled.ForeColor = AppTheme.Fg;
        _lstInstalled.Font = AppTheme.Mono;
        _lstInstalled.BorderStyle = BorderStyle.None;
        _lstInstalled.Columns.AddRange([
            new ColumnHeader { Text = "Module", Width = 220 },
            new ColumnHeader { Text = "Version", Width = 140 },
            new ColumnHeader { Text = "Status", Width = 140 },
        ]);
        _lstInstalled.SelectedIndexChanged += (_, _) =>
        {
            if (_lstInstalled.FocusedItem is { Tag: string name })
                _txtName.Text = name;
        };

        var ctrlPanel = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 80,
            BackColor = AppTheme.Surface,
        };
        _txtName.Width = 190;
        _txtName.BackColor = AppTheme.Overlay;
        _txtName.ForeColor = AppTheme.Fg;
        _txtName.PlaceholderText = "module name";
        _txtName.Location = new Point(8, 8);
        _txtName.KeyDown += (_, e) =>
        {
            if (e.KeyCode == Keys.Enter)
            {
                _ = InstallAsync();
                e.Handled = true;
            }
        };

        void StyleBtn(Button b, Color bg, string t, int x, int y)
        {
            b.Text = t;
            b.BackColor = bg;
            b.ForeColor = AppTheme.Bg;
            b.FlatStyle = FlatStyle.Flat;
            b.Location = new Point(x, y);
            b.Size = new Size(80, 27);
        }

        StyleBtn(_btnInstall, AppTheme.Green, "Install", 206, 7);
        StyleBtn(_btnRemove, AppTheme.Red, "Remove", 292, 7);
        StyleBtn(_btnUpdate, AppTheme.Yellow, "Update", 378, 7);
        StyleBtn(_btnRefresh, AppTheme.Accent, "Refresh", 464, 7);

        _btnClose.Text = "Close";
        _btnClose.BackColor = AppTheme.Overlay;
        _btnClose.ForeColor = AppTheme.Fg;
        _btnClose.FlatStyle = FlatStyle.Flat;
        _btnClose.Location = new Point(464, 42);
        _btnClose.Size = new Size(80, 27);
        _btnClose.Click += (_, _) => Close();

        _btnInstall.Click += async (_, _) => await InstallAsync();
        _btnRemove.Click += async (_, _) => await RemoveAsync();
        _btnUpdate.Click += async (_, _) => await UpdateAsync();
        _btnRefresh.Click += async (_, _) => await RefreshAsync();

        ctrlPanel.Controls.AddRange([_txtName, _btnInstall, _btnRemove, _btnUpdate, _btnRefresh, _btnClose]);

        var quickLabel = new Label
        {
            Text = "Quick install (not yet installed):",
            ForeColor = AppTheme.FgDim,
            Dock = DockStyle.Top,
            Height = 22,
            Padding = new Padding(8, 4, 0, 0),
        };

        _flowQuick.Dock = DockStyle.Top;
        _flowQuick.Height = 80;
        _flowQuick.BackColor = AppTheme.Surface;
        _flowQuick.Padding = new Padding(4);
        _flowQuick.AutoScroll = true;

        Controls.AddRange([_lstInstalled, ctrlPanel, _flowQuick, quickLabel, scopePanel, _lblOutdated, _lblStatus, prereqPanel, lblTitle]);

        if (!_prereqMet)
            SetMainEnabled(false);
    }

    private async Task RefreshAsync()
    {
        string scope = _cmbScope.SelectedItem?.ToString() ?? "CurrentUser";
        SetBusy(true, "Loading modules...");
        try
        {
            var list = await PSModuleManager.ListInstalledAsync(scope, _cts.Token);
            _installedNames = await PSModuleManager.ListInstalledNamesAsync(scope, _cts.Token);
            _lstInstalled.Items.Clear();
            foreach (var m in list)
            {
                var item = new ListViewItem(m) { Tag = m };
                item.SubItems.Add(""); // Version placeholder
                item.SubItems.Add("Installed");
                item.ForeColor = AppTheme.Fg;
                _lstInstalled.Items.Add(item);
            }
            _lblStatus.Text = $"Scope: {scope} — {list.Count} module(s)";
            _lblStatus.ForeColor = AppTheme.Fg;
            RebuildQuickInstallButtons();
            _ = CheckOutdatedAsync(scope);
        }
        catch (Exception ex)
        {
            _lblStatus.Text = $"Error: {ex.Message}";
            _lblStatus.ForeColor = AppTheme.Red;
        }
        finally
        {
            SetBusy(false);
        }
    }

    private async Task CheckOutdatedAsync(string scope)
    {
        try
        {
            var outdated = await PSModuleManager.ListOutdatedAsync(scope, _cts.Token);
            _outdatedNames = [.. outdated];
            foreach (ListViewItem item in _lstInstalled.Items)
            {
                if (item.Tag is string name && _outdatedNames.Contains(name))
                {
                    item.SubItems[2].Text = "\u26A0 Update available";
                    item.ForeColor = AppTheme.Yellow;
                }
            }
            _lblOutdated.Text = outdated.Count > 0 ? $"\u26A0 {outdated.Count} update(s) available" : "\u2714 All modules up to date";
            _lblOutdated.ForeColor = outdated.Count > 0 ? AppTheme.Yellow : AppTheme.Green;
        }
        catch
        {
            _lblOutdated.Text = "";
        }
    }

    private void RebuildQuickInstallButtons()
    {
        _flowQuick.Controls.Clear();
        foreach (string mod in PSModuleManager.PopularModules)
        {
            if (_installedNames.Contains(mod))
                continue;
            string captured = mod;
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
            _flowQuick.Controls.Add(btn);
        }
    }

    private async Task InstallAsync()
    {
        string name = _txtName.Text.Trim();
        string scope = _cmbScope.SelectedItem?.ToString() ?? "CurrentUser";
        if (name.Length == 0)
            return;
        SetBusy(true, $"Installing {name} ({scope})...");
        try
        {
            await PSModuleManager.InstallAsync(name, scope, _cts.Token);
            await RefreshAsync();
        }
        catch (Exception ex)
        {
            _lblStatus.Text = $"Error: {ex.Message}";
            _lblStatus.ForeColor = AppTheme.Red;
        }
        finally
        {
            SetBusy(false);
        }
    }

    private async Task RemoveAsync()
    {
        string name = (_lstInstalled.FocusedItem?.Tag as string ?? _txtName.Text).Trim();
        if (name.Length == 0)
            return;
        SetBusy(true, $"Removing {name}...");
        try
        {
            await PSModuleManager.RemoveAsync(name, _cts.Token);
            await RefreshAsync();
        }
        catch (Exception ex)
        {
            _lblStatus.Text = $"Error: {ex.Message}";
            _lblStatus.ForeColor = AppTheme.Red;
        }
        finally
        {
            SetBusy(false);
        }
    }

    private async Task UpdateAsync()
    {
        string name = (_lstInstalled.FocusedItem?.Tag as string ?? _txtName.Text).Trim();
        string scope = _cmbScope.SelectedItem?.ToString() ?? "CurrentUser";
        if (name.Length == 0)
            return;
        SetBusy(true, $"Updating {name} ({scope})...");
        try
        {
            await PSModuleManager.UpdateAsync(name, scope, _cts.Token);
            await RefreshAsync();
        }
        catch (Exception ex)
        {
            _lblStatus.Text = $"Error: {ex.Message}";
            _lblStatus.ForeColor = AppTheme.Red;
        }
        finally
        {
            SetBusy(false);
        }
    }

    private void SetBusy(bool busy, string? message = null)
    {
        _btnInstall.Enabled = !busy;
        _btnRemove.Enabled = !busy;
        _btnUpdate.Enabled = !busy;
        _btnRefresh.Enabled = !busy;
        if (message is not null)
            _lblStatus.Text = message;
    }

    private void SetMainEnabled(bool enabled)
    {
        _btnInstall.Enabled = enabled;
        _btnRemove.Enabled = enabled;
        _btnUpdate.Enabled = enabled;
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
            Text = _prereqMet ? "\u2705 PowerShellGet is available" : "\u274C PowerShellGet module not found",
            AutoSize = true,
            Location = new Point(8, 8),
            ForeColor = _prereqMet ? AppTheme.Green : AppTheme.Red,
        };
        panel.Controls.Add(lbl);

        if (!_prereqMet)
        {
            var btn = new Button
            {
                Text = "Install PSGet",
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
                lbl.Text = "\u23F3 Installing PowerShellGet...";
                lbl.ForeColor = AppTheme.Yellow;
                panel.BackColor = Color.FromArgb(20, AppTheme.Yellow);
                try
                {
                    await PSModuleManager.InstallPowerShellGetAsync(_cts.Token);
                    _prereqMet = true;
                    lbl.Text = "\u2705 PowerShellGet installed successfully";
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
