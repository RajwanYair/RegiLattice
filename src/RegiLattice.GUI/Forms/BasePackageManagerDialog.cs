// RegiLattice.GUI — Forms/BasePackageManagerDialog.cs
// Abstract base for all five package manager dialogs.
// Provides: SplitContainer layout with resizable log panel, shared controls,
//           prereq banner (with async install flow), quick-install strip,
//           AppendLog, SetBusy/SetStatus/SetOutdated, and shared async wrappers.

namespace RegiLattice.GUI.Forms;

/// <summary>
/// Abstract base for all package manager dialogs.
/// Provides SplitContainer layout (package list above, log below), a shared prereq
/// banner with async tool installation, and helper methods used by all subclasses.
/// </summary>
internal abstract class BasePackageManagerDialog : Form
{
    // ── Shared controls accessible to subclasses ──────────────────────────
    protected readonly ListView _lstInstalled = new();
    protected readonly TextBox _txtName = new();
    protected readonly Label _lblStatus = new();
    protected readonly Label _lblOutdated = new();
    protected readonly FlowLayoutPanel _flowQuick = new();
    protected readonly Button _btnRefresh = new();
    protected readonly Button _btnInstall = new();
    protected readonly Button _btnRemove = new();
    protected readonly Button _btnUpgrade = new(); // may show "Update" for PSModule
    protected readonly Button _btnClose = new();
    private readonly RichTextBox _rtbLog = new();

    // ── Shared state ──────────────────────────────────────────────────────
    protected HashSet<string> _installedNames = new(StringComparer.OrdinalIgnoreCase);
    protected HashSet<string> _outdatedNames = new(StringComparer.OrdinalIgnoreCase);
    protected CancellationTokenSource _cts = new();
    protected bool _prereqMet;

    // ── Abstract: tool identity ───────────────────────────────────────────
    protected abstract string DialogTitle { get; }
    protected abstract Icon? DialogIcon { get; }
    protected abstract string PrereqReadyText { get; } // shown when tool is installed
    protected abstract string PrereqMissingText { get; } // shown when tool is missing
    protected abstract string PrereqInstallingText { get; } // shown during install
    protected abstract string PrereqInstallButtonText { get; } // button label
    protected abstract Task InstallPrereqAsync(CancellationToken ct);
    protected abstract bool CheckPrereq();
    protected abstract IReadOnlyList<string> PopularPackages { get; }
    protected abstract ColumnHeader[] BuildListColumns();
    protected abstract string UpgradeText { get; } // "Upgrade" or "Update"

    // ── Abstract: per-tool async operations ──────────────────────────────
    protected abstract Task RefreshCoreAsync(CancellationToken ct);
    protected abstract Task InstallCoreAsync(string name, CancellationToken ct);
    protected abstract Task RemoveCoreAsync(string name, CancellationToken ct);
    protected abstract Task UpgradeCoreAsync(string name, CancellationToken ct);

    // ── Virtual: extension points ─────────────────────────────────────────
    /// <summary>Override to return a scope-selector panel (Pip, PSModule). Default: null.</summary>
    protected virtual Control? BuildScopePanel() => null;

    /// <summary>Override to insert extra buttons before Refresh (e.g. Search in WinGet).</summary>
    protected virtual void AddExtraButtons(Panel ctrlPanel, ref int x) { }

    // ── Constructor ───────────────────────────────────────────────────────
    protected BasePackageManagerDialog()
    {
        Text = DialogTitle;
        Icon = DialogIcon;
        FormBorderStyle = FormBorderStyle.Sizable;
        StartPosition = FormStartPosition.CenterParent;
        MinimumSize = new Size(600, 580);
        ClientSize = new Size(700, 720);
        BackColor = AppTheme.Bg;
        ForeColor = AppTheme.Fg;
        Font = AppTheme.Regular;

        _prereqMet = CheckPrereq();
        BuildLayout();
        Load += async (_, _) =>
        {
            AppTheme.Apply3D(this);
            if (_prereqMet)
                await RefreshAsync();
        };
        FormClosed += (_, _) => _cts.Cancel();
    }

    // ── Layout ────────────────────────────────────────────────────────────
    private void BuildLayout()
    {
        var lblTitle = new Label
        {
            Text = DialogTitle,
            Font = AppTheme.Title,
            ForeColor = AppTheme.Accent,
            Dock = DockStyle.Top,
            Height = 36,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(8, 0, 0, 0),
        };

        var prereqPanel = BuildPrereqBanner();

        _lblStatus.Dock = DockStyle.Top;
        _lblStatus.Height = 24;
        _lblStatus.Padding = new Padding(8, 0, 0, 0);

        _lblOutdated.Text = "";
        _lblOutdated.ForeColor = AppTheme.Yellow;
        _lblOutdated.Dock = DockStyle.Top;
        _lblOutdated.Height = 22;
        _lblOutdated.Padding = new Padding(8, 0, 0, 0);
        _lblOutdated.Font = AppTheme.Regular;

        // ── ListView ─────────────────────────────────────────────────────
        _lstInstalled.View = View.Details;
        _lstInstalled.FullRowSelect = true;
        _lstInstalled.GridLines = false;
        _lstInstalled.MultiSelect = true;
        _lstInstalled.BackColor = AppTheme.Bg;
        _lstInstalled.ForeColor = AppTheme.Fg;
        _lstInstalled.Font = AppTheme.Mono;
        _lstInstalled.BorderStyle = BorderStyle.None;
        _lstInstalled.Dock = DockStyle.Fill;
        _lstInstalled.Columns.AddRange(BuildListColumns());
        ListViewColumnSorter.AttachTo(_lstInstalled);
        _lstInstalled.SelectedIndexChanged += (_, _) =>
        {
            if (_lstInstalled.SelectedItems.Count == 1 && _lstInstalled.SelectedItems[0].Tag is string nm)
                _txtName.Text = nm;
        };

        // ── Quick install strip ──────────────────────────────────────────
        _flowQuick.Dock = DockStyle.Bottom;
        _flowQuick.Height = 56;
        _flowQuick.BackColor = AppTheme.Surface;
        _flowQuick.Padding = new Padding(4, 2, 4, 2);
        _flowQuick.AutoScroll = true;

        // ── Log panel (bottom half of SplitContainer) ────────────────────
        _rtbLog.ReadOnly = true;
        _rtbLog.BackColor = AppTheme.LogBg;
        _rtbLog.ForeColor = AppTheme.LogFg;
        _rtbLog.Font = AppTheme.Mono;
        _rtbLog.Dock = DockStyle.Fill;
        _rtbLog.BorderStyle = BorderStyle.None;
        _rtbLog.ScrollBars = RichTextBoxScrollBars.Vertical;
        _rtbLog.WordWrap = false;

        var logHeader = new Label
        {
            Text = "  Log",
            ForeColor = AppTheme.FgDim,
            BackColor = AppTheme.Surface2,
            Dock = DockStyle.Top,
            Height = 20,
            Font = AppTheme.SmallBold,
            TextAlign = ContentAlignment.MiddleLeft,
        };

        // ── SplitContainer: top = list, bottom = log ─────────────────────
        var split = new SplitContainer
        {
            Dock = DockStyle.Fill,
            Orientation = Orientation.Horizontal,
            SplitterDistance = 320,
            Panel1MinSize = 100,
            Panel2MinSize = 60,
            BackColor = AppTheme.Bg,
        };
        split.Panel1.Controls.AddRange([_lstInstalled, _flowQuick]);
        split.Panel2.Controls.AddRange([_rtbLog, logHeader]);

        // ── Button control panel ─────────────────────────────────────────
        var ctrlPanel = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 80,
            BackColor = AppTheme.Surface,
        };

        _txtName.Width = 200;
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

        void StyleBtn(Button b, Color bg, int x, int y)
        {
            b.BackColor = bg;
            b.ForeColor = AppTheme.Bg;
            b.FlatStyle = FlatStyle.Flat;
            b.Location = new Point(x, y);
            b.Size = new Size(84, 28);
        }

        _btnInstall.Text = "Install";
        _btnRemove.Text = "Remove";
        _btnUpgrade.Text = UpgradeText;
        _btnRefresh.Text = "Refresh";
        _btnClose.Text = "Close";

        StyleBtn(_btnInstall, AppTheme.Green, 216, 7);
        StyleBtn(_btnRemove, AppTheme.Red, 306, 7);
        StyleBtn(_btnUpgrade, AppTheme.Yellow, 396, 7);
        int extraX = 486;
        AddExtraButtons(ctrlPanel, ref extraX);
        StyleBtn(_btnRefresh, AppTheme.Accent, extraX, 7);

        _btnClose.BackColor = AppTheme.Overlay;
        _btnClose.ForeColor = AppTheme.Fg;
        _btnClose.FlatStyle = FlatStyle.Flat;
        _btnClose.Location = new Point(extraX, 42);
        _btnClose.Size = new Size(84, 28);

        _btnInstall.Click += async (_, _) => await InstallAsync();
        _btnRemove.Click += async (_, _) => await RemoveAsync();
        _btnUpgrade.Click += async (_, _) => await UpgradeAsync();
        _btnRefresh.Click += async (_, _) => await RefreshAsync();
        _btnClose.Click += (_, _) => Close();

        ctrlPanel.Controls.AddRange([_txtName, _btnInstall, _btnRemove, _btnUpgrade, _btnRefresh, _btnClose]);

        // ── Compose: last-added DockStyle.Top ends up topmost ────────────
        Controls.AddRange([split, ctrlPanel, _lblOutdated, _lblStatus]);
        var scopePanel = BuildScopePanel();
        if (scopePanel is not null)
            Controls.Add(scopePanel);
        Controls.AddRange([prereqPanel, lblTitle]);

        if (!_prereqMet)
            SetMainEnabled(false);
    }

    private Panel BuildPrereqBanner()
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
            Text = _prereqMet ? $"✅ {PrereqReadyText}" : $"❌ {PrereqMissingText}",
            AutoSize = true,
            Location = new Point(8, 8),
            ForeColor = _prereqMet ? AppTheme.Green : AppTheme.Red,
        };
        panel.Controls.Add(lbl);
        if (!_prereqMet)
        {
            var btn = new Button
            {
                Text = PrereqInstallButtonText,
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
                lbl.Text = $"⏳ {PrereqInstallingText}";
                lbl.ForeColor = AppTheme.Yellow;
                panel.BackColor = Color.FromArgb(20, AppTheme.Yellow);
                try
                {
                    await InstallPrereqAsync(_cts.Token);
                    _prereqMet = true;
                    lbl.Text = $"✅ {PrereqReadyText}";
                    lbl.ForeColor = AppTheme.Green;
                    panel.BackColor = Color.FromArgb(20, AppTheme.Green);
                    btn.Visible = false;
                    SetMainEnabled(true);
                    await RefreshAsync();
                }
                catch (Exception ex)
                {
                    lbl.Text = $"❌ Install failed: {ex.Message}";
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

    // ── Quick install strip ───────────────────────────────────────────────
    protected void RebuildQuickInstallButtons()
    {
        _flowQuick.Controls.Clear();
        foreach (string pkg in PopularPackages)
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

    // ── Log output ────────────────────────────────────────────────────────
    protected void AppendLog(string message, Color? color = null)
    {
        if (InvokeRequired)
        {
            Invoke(() => AppendLog(message, color));
            return;
        }
        int start = _rtbLog.TextLength;
        string line = $"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}";
        _rtbLog.AppendText(line);
        if (color is not null)
        {
            _rtbLog.Select(start, line.Length);
            _rtbLog.SelectionColor = color.Value;
            _rtbLog.SelectionLength = 0;
        }
        _rtbLog.ScrollToCaret();
    }

    // ── Busy / enable ─────────────────────────────────────────────────────
    protected void SetBusy(bool busy, string? message = null)
    {
        if (InvokeRequired)
        {
            Invoke(() => SetBusy(busy, message));
            return;
        }
        _btnInstall.Enabled = !busy;
        _btnRemove.Enabled = !busy;
        _btnUpgrade.Enabled = !busy;
        _btnRefresh.Enabled = !busy;
        if (message is not null)
        {
            _lblStatus.Text = message;
            _lblStatus.ForeColor = AppTheme.FgDim;
        }
    }

    protected void SetMainEnabled(bool enabled)
    {
        _btnInstall.Enabled = enabled;
        _btnRemove.Enabled = enabled;
        _btnUpgrade.Enabled = enabled;
        _btnRefresh.Enabled = enabled;
        _txtName.Enabled = enabled;
    }

    protected void SetStatus(string msg, Color? color = null)
    {
        _lblStatus.Text = msg;
        _lblStatus.ForeColor = color ?? AppTheme.Fg;
    }

    protected void SetOutdated(string msg, Color? color = null)
    {
        _lblOutdated.Text = msg;
        _lblOutdated.ForeColor = color ?? AppTheme.Yellow;
    }

    // ── Shared async wrappers ─────────────────────────────────────────────
    protected async Task RefreshAsync()
    {
        SetBusy(true, "Loading...");
        AppendLog("Refreshing package list...");
        try
        {
            await RefreshCoreAsync(_cts.Token);
        }
        catch (Exception ex)
        {
            SetStatus($"Error: {ex.Message}", AppTheme.Red);
            AppendLog($"Error: {ex.Message}", AppTheme.Red);
        }
        finally
        {
            SetBusy(false);
        }
    }

    protected async Task InstallAsync()
    {
        string name = _txtName.Text.Trim();
        if (name.Length == 0)
            return;
        SetBusy(true, $"Installing {name}...");
        AppendLog($"Installing: {name}");
        try
        {
            await InstallCoreAsync(name, _cts.Token);
            AppendLog($"Installed: {name}", AppTheme.Green);
            await RefreshAsync();
        }
        catch (Exception ex)
        {
            SetStatus($"Error: {ex.Message}", AppTheme.Red);
            AppendLog($"Error: {ex.Message}", AppTheme.Red);
        }
        finally
        {
            SetBusy(false);
        }
    }

    protected async Task RemoveAsync()
    {
        var names = GetSelectedOrTypedNames();
        if (names.Count == 0)
            return;
        SetBusy(true, $"Removing {names.Count} package(s)...");
        AppendLog($"Removing: {string.Join(", ", names)}");
        try
        {
            foreach (string name in names)
            {
                await RemoveCoreAsync(name, _cts.Token);
                AppendLog($"Removed: {name}", AppTheme.Red);
            }
            await RefreshAsync();
        }
        catch (Exception ex)
        {
            SetStatus($"Error: {ex.Message}", AppTheme.Red);
            AppendLog($"Error: {ex.Message}", AppTheme.Red);
        }
        finally
        {
            SetBusy(false);
        }
    }

    protected async Task UpgradeAsync()
    {
        var names = GetSelectedOrTypedNames();
        if (names.Count == 0)
            return;
        SetBusy(true, $"{UpgradeText}ing {names.Count} package(s)...");
        AppendLog($"{UpgradeText}: {string.Join(", ", names)}");
        try
        {
            foreach (string name in names)
            {
                await UpgradeCoreAsync(name, _cts.Token);
                AppendLog($"{UpgradeText}d: {name}", AppTheme.Yellow);
            }
            await RefreshAsync();
        }
        catch (Exception ex)
        {
            SetStatus($"Error: {ex.Message}", AppTheme.Red);
            AppendLog($"Error: {ex.Message}", AppTheme.Red);
        }
        finally
        {
            SetBusy(false);
        }
    }

    private IReadOnlyList<string> GetSelectedOrTypedNames()
    {
        var selected = _lstInstalled
            .SelectedItems.Cast<ListViewItem>()
            .Select(i => i.Tag as string)
            .Where(n => !string.IsNullOrEmpty(n))
            .Select(n => n!)
            .ToList();
        if (selected.Count > 0)
            return selected;
        string typed = _txtName.Text.Trim();
        return typed.Length > 0 ? [typed] : [];
    }
}
