// RegiLattice.GUI — Forms/PipManagerDialog.cs
// pip package manager dialog with user/machine scope selector and version display.

using RegiLattice.GUI.PackageManagers;
using Elevation = RegiLattice.Core.Elevation;

namespace RegiLattice.GUI.Forms;

/// <summary>
/// pip (Python) package manager dialog.
/// Supports user-level vs machine-level (all) package listing.
/// Shows installed packages with versions, outdated indicators, and close button.
/// When running as admin, allows selecting packages for a specific scope.
/// </summary>
internal sealed class PipManagerDialog : Form
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
    private readonly Button _btnUpgrade = new();
    private readonly Button _btnClose = new();

    private HashSet<string> _installedNames = new(StringComparer.OrdinalIgnoreCase);
    private CancellationTokenSource _cts = new();
    private bool _prereqMet;

    internal PipManagerDialog()
    {
        Text = "\U0001F40D pip Package Manager";
        Icon = AppIcons.PipIcon;
        FormBorderStyle = FormBorderStyle.Sizable;
        StartPosition = FormStartPosition.CenterParent;
        MinimumSize = new Size(620, 520);
        ClientSize = new Size(660, 620);
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
            Text = "pip Package Manager",
            Font = AppTheme.Title,
            ForeColor = AppTheme.Accent,
            Dock = DockStyle.Top,
            Height = 36,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(8, 0, 0, 0),
        };

        bool installed = PipManager.IsPipInstalled();
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

        // Scope selector panel (user-level vs machine-level packages)
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
        _cmbScope.Items.AddRange(["All packages", "User-level only"]);
        _cmbScope.SelectedIndex = 0;
        _cmbScope.BackColor = AppTheme.Overlay;
        _cmbScope.ForeColor = AppTheme.Fg;
        _cmbScope.FlatStyle = FlatStyle.Flat;
        _cmbScope.Location = new Point(58, 5);
        _cmbScope.Width = 160;
        _cmbScope.SelectedIndexChanged += async (_, _) => await RefreshAsync();

        bool isAdmin = Elevation.IsAdmin();
        var lblAdmin = new Label
        {
            Text = isAdmin ? "\U0001F6E1 Admin mode — can manage machine-level packages" : "\U0001F464 User mode — showing user-level scope",
            AutoSize = true,
            Location = new Point(230, 8),
            ForeColor = isAdmin ? AppTheme.Green : AppTheme.FgDim,
            Font = AppTheme.Small,
        };
        scopePanel.Controls.AddRange([lblScope, _cmbScope, lblAdmin]);

        // ListView with Name, Version, Status columns
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
            new ColumnHeader { Text = "Package", Width = 240 },
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
        _txtName.Width = 180;
        _txtName.BackColor = AppTheme.Overlay;
        _txtName.ForeColor = AppTheme.Fg;
        _txtName.PlaceholderText = "package name";
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

        StyleBtn(_btnInstall, AppTheme.Green, "Install", 196, 7);
        StyleBtn(_btnRemove, AppTheme.Red, "Remove", 282, 7);
        StyleBtn(_btnUpgrade, AppTheme.Yellow, "Upgrade", 368, 7);
        StyleBtn(_btnRefresh, AppTheme.Accent, "Refresh", 454, 7);

        _btnClose.Text = "Close";
        _btnClose.BackColor = AppTheme.Overlay;
        _btnClose.ForeColor = AppTheme.Fg;
        _btnClose.FlatStyle = FlatStyle.Flat;
        _btnClose.Location = new Point(454, 42);
        _btnClose.Size = new Size(80, 27);
        _btnClose.Click += (_, _) => Close();

        _btnInstall.Click += async (_, _) => await InstallAsync();
        _btnRemove.Click += async (_, _) => await RemoveAsync();
        _btnUpgrade.Click += async (_, _) => await UpgradeAsync();
        _btnRefresh.Click += async (_, _) => await RefreshAsync();

        ctrlPanel.Controls.AddRange([_txtName, _btnInstall, _btnRemove, _btnUpgrade, _btnRefresh, _btnClose]);

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

    /// <summary>Rebuilds quick-install buttons, hiding packages already installed.</summary>
    private void RebuildQuickInstallButtons()
    {
        _flowQuick.Controls.Clear();
        foreach (string pkg in PipManager.PopularPackages)
        {
            if (_installedNames.Contains(pkg))
                continue;
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
            _flowQuick.Controls.Add(btn);
        }
    }

    private async Task RefreshAsync()
    {
        bool userOnly = _cmbScope.SelectedIndex == 1;
        SetBusy(true, "Loading...");
        try
        {
            var list = await PipManager.ListInstalledAsync(userOnly, _cts.Token);
            _installedNames = await PipManager.ListInstalledNamesAsync(_cts.Token);

            _lstInstalled.Items.Clear();
            foreach (var entry in list)
            {
                int paren = entry.IndexOf(" (", StringComparison.Ordinal);
                string name = paren > 0 ? entry[..paren] : entry;
                string version = paren > 0 ? entry[(paren + 2)..].TrimEnd(')') : "";
                var item = new ListViewItem(name) { Tag = name };
                item.SubItems.Add(version);
                item.SubItems.Add("\u2714 Up to date");
                item.ForeColor = AppTheme.Fg;
                _lstInstalled.Items.Add(item);
            }

            string scopeLabel = userOnly ? "user-level" : "all";
            _lblStatus.Text = $"pip: {list.Count} {scopeLabel} package(s)";
            _lblStatus.ForeColor = AppTheme.Green;

            RebuildQuickInstallButtons();
            _ = CheckOutdatedAsync();
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

    private async Task CheckOutdatedAsync()
    {
        try
        {
            var outdated = await PipManager.ListOutdatedAsync(_cts.Token);
            var outdatedNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var entry in outdated)
            {
                int paren = entry.IndexOf(" (", StringComparison.Ordinal);
                outdatedNames.Add(paren > 0 ? entry[..paren] : entry);
            }

            // Update status column for outdated packages
            foreach (ListViewItem item in _lstInstalled.Items)
            {
                if (item.Tag is string pkgName && outdatedNames.Contains(pkgName))
                {
                    item.SubItems[2].Text = "\u26A0 Update available";
                    item.SubItems[2].ForeColor = AppTheme.Yellow;
                }
            }

            _lblOutdated.Text = outdated.Count > 0 ? $"\u26A0 {outdated.Count} update(s) available" : "\u2714 All packages up to date";
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
        if (name.Length == 0)
            return;
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
            await PipManager.UninstallAsync(name, _cts.Token);
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

    private async Task UpgradeAsync()
    {
        string name = (_lstInstalled.FocusedItem?.Tag as string ?? _txtName.Text).Trim();
        if (name.Length == 0)
            return;
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
        finally
        {
            SetBusy(false);
        }
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
            Text = _prereqMet ? "\u2705 Python + pip installed and ready" : "\u274C Python / pip not found",
            AutoSize = true,
            Location = new Point(8, 8),
            ForeColor = _prereqMet ? AppTheme.Green : AppTheme.Red,
        };
        panel.Controls.Add(lbl);

        if (!_prereqMet)
        {
            var btn = new Button
            {
                Text = "Install Python",
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
                lbl.Text = "\u23F3 Installing Python via winget...";
                lbl.ForeColor = AppTheme.Yellow;
                panel.BackColor = Color.FromArgb(20, AppTheme.Yellow);
                try
                {
                    await PipManager.InstallPythonAsync(_cts.Token);
                    _prereqMet = true;
                    lbl.Text = "\u2705 Python installed \u2014 restart dialog to use pip";
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
