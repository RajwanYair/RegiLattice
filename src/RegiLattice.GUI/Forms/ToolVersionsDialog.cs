using System.Diagnostics;
using RegiLattice.Core;
using RegiLattice.GUI.PackageManagers;

namespace RegiLattice.GUI.Forms;

/// <summary>Shows installed versions of external tools used by RegiLattice.</summary>
internal sealed class ToolVersionsDialog : Form
{
    private readonly ListView _lstTools = new();
    private readonly Label _lblStatus = new();
    private readonly Button _btnRefresh = new();
    private readonly CheckBox _chkUpdates = new();
    private readonly bool _checkUpdates;

    // ── Install guide panel (shown when a not-found tool is selected) ─────────────
    private readonly Panel _installGuidePanel = new();
    private readonly Label _lblGuideTitle = new();
    private readonly TextBox _txtGuideCmd = new();
    private readonly Button _btnGuideCopy = new();
    private readonly Button _btnGuideDocs = new();
    private readonly ContextMenuStrip _ctxMenu = new();

    private CancellationTokenSource _cts = new();

    internal ToolVersionsDialog()
    {
        Text = "🔧 External Tool Versions";
        Icon = AppIcons.ToolVersionsIcon;
        FormBorderStyle = FormBorderStyle.Sizable;
        StartPosition = FormStartPosition.CenterParent;
        MinimumSize = new Size(560, 380);
        ClientSize = new Size(620, 440);
        BackColor = AppTheme.Bg;
        ForeColor = AppTheme.Fg;
        Font = AppTheme.Regular;

        _checkUpdates = AppConfig.Load().CheckToolUpdates;

        BuildLayout();
        BuildInstallGuidePanel();
        BuildContextMenu();
        Load += async (_, _) => await RefreshAsync();
        FormClosed += (_, _) => _cts.Cancel();
    }

    private void BuildLayout()
    {
        var lblTitle = new Label
        {
            Text = "External Tool Versions",
            Font = AppTheme.Title,
            ForeColor = AppTheme.Accent,
            Dock = DockStyle.Top,
            Height = 36,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(8, 0, 0, 0),
        };

        _lblStatus.Text = "Checking tool versions...";
        _lblStatus.ForeColor = AppTheme.FgDim;
        _lblStatus.Dock = DockStyle.Top;
        _lblStatus.Height = 24;
        _lblStatus.Padding = new Padding(8, 0, 0, 0);

        _lstTools.View = View.Details;
        _lstTools.FullRowSelect = true;
        _lstTools.GridLines = true;
        _lstTools.Dock = DockStyle.Fill;
        _lstTools.Font = AppTheme.Mono;
        _lstTools.BackColor = AppTheme.Surface;
        _lstTools.ForeColor = AppTheme.Fg;
        _lstTools.Columns.AddRange([
            new ColumnHeader { Text = "Tool", Width = 130 },
            new ColumnHeader { Text = "Installed", Width = 120 },
            new ColumnHeader { Text = "Latest", Width = 120 },
            new ColumnHeader { Text = "Status", Width = 160 },
        ]);
        ListViewColumnSorter.AttachTo(_lstTools);

        var ctrlPanel = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 42,
            BackColor = AppTheme.Surface,
        };
        _btnRefresh.Text = "Refresh";
        _btnRefresh.BackColor = AppTheme.Accent;
        _btnRefresh.ForeColor = AppTheme.Bg;
        _btnRefresh.FlatStyle = FlatStyle.Flat;
        _btnRefresh.Location = new Point(8, 7);
        _btnRefresh.Size = new Size(90, 27);
        _btnRefresh.Click += async (_, _) => await RefreshAsync();

        _chkUpdates.Text = "Check for updates";
        _chkUpdates.Checked = _checkUpdates;
        _chkUpdates.ForeColor = AppTheme.FgDim;
        _chkUpdates.Location = new Point(110, 10);
        _chkUpdates.AutoSize = true;

        ctrlPanel.Controls.AddRange([_btnRefresh, _chkUpdates]);
        Controls.AddRange([_lstTools, ctrlPanel, _lblStatus, lblTitle]);

        // wire selection change to show/hide install guide
        _lstTools.SelectedIndexChanged += OnToolSelectionChanged;
    }

    private void BuildInstallGuidePanel()
    {
        _installGuidePanel.Dock = DockStyle.Bottom;
        _installGuidePanel.Height = 62;
        _installGuidePanel.BackColor = Color.FromArgb(20, AppTheme.Accent);
        _installGuidePanel.Padding = new Padding(8, 6, 8, 6);
        _installGuidePanel.Visible = false;

        _lblGuideTitle.AutoSize = true;
        _lblGuideTitle.Location = new Point(8, 8);
        _lblGuideTitle.ForeColor = AppTheme.Accent;
        _lblGuideTitle.Font = AppTheme.Regular;

        _txtGuideCmd.ReadOnly = true;
        _txtGuideCmd.Location = new Point(8, 30);
        _txtGuideCmd.Height = 22;
        _txtGuideCmd.Width = 400;
        _txtGuideCmd.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
        _txtGuideCmd.BackColor = AppTheme.Surface;
        _txtGuideCmd.ForeColor = AppTheme.Fg;
        _txtGuideCmd.Font = AppTheme.Mono;
        _txtGuideCmd.BorderStyle = BorderStyle.FixedSingle;

        _btnGuideCopy.Text = "📋 Copy";
        _btnGuideCopy.BackColor = AppTheme.Overlay;
        _btnGuideCopy.ForeColor = AppTheme.Fg;
        _btnGuideCopy.FlatStyle = FlatStyle.Flat;
        _btnGuideCopy.Size = new Size(68, 22);
        _btnGuideCopy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        _btnGuideCopy.Location = new Point(0, 30);
        _btnGuideCopy.Click += (_, _) =>
        {
            if (!string.IsNullOrEmpty(_txtGuideCmd.Text))
                Clipboard.SetText(_txtGuideCmd.Text);
        };

        _btnGuideDocs.Text = "🌐 Docs";
        _btnGuideDocs.BackColor = AppTheme.Overlay;
        _btnGuideDocs.ForeColor = AppTheme.Fg;
        _btnGuideDocs.FlatStyle = FlatStyle.Flat;
        _btnGuideDocs.Size = new Size(68, 22);
        _btnGuideDocs.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        _btnGuideDocs.Location = new Point(0, 30);
        _btnGuideDocs.Tag = ""; // URL set when selection changes
        _btnGuideDocs.Click += (_, _) =>
        {
            if (_btnGuideDocs.Tag is string url && !string.IsNullOrEmpty(url))
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        };

        void LayoutGuideRow(int w)
        {
            int right = w - 8;
            _btnGuideDocs.Location = new Point(right - _btnGuideDocs.Width, 30);
            right = _btnGuideDocs.Left - 4;
            _btnGuideCopy.Location = new Point(right - _btnGuideCopy.Width, 30);
            _txtGuideCmd.Width = _btnGuideCopy.Left - _txtGuideCmd.Left - 4;
        }

        _installGuidePanel.Resize += (_, _) => LayoutGuideRow(_installGuidePanel.ClientSize.Width);
        _installGuidePanel.Controls.AddRange([_lblGuideTitle, _txtGuideCmd, _btnGuideCopy, _btnGuideDocs]);
        Controls.Add(_installGuidePanel);
        AppTheme.Apply3D(this);
    }

    private void BuildContextMenu()
    {
        _lstTools.ContextMenuStrip = _ctxMenu;
        _ctxMenu.Opening += (_, e) =>
        {
            _ctxMenu.Items.Clear();
            if (_lstTools.SelectedItems.Count == 0)
            {
                e.Cancel = true;
                return;
            }
            string toolName = _lstTools.SelectedItems[0].Text;
            bool notFound =
                _lstTools.SelectedItems[0].SubItems.Count > 3
                && _lstTools.SelectedItems[0].SubItems[3].Text.StartsWith("Not Found", StringComparison.OrdinalIgnoreCase);
            if (!notFound)
            {
                e.Cancel = true;
                return;
            }
            var (cmd, url) = ToolVersionChecker.GetInstallGuide(toolName);
            if (cmd is null && url is null)
            {
                e.Cancel = true;
                return;
            }
            var miTitle = new ToolStripMenuItem($"📎 How to install {toolName}") { Enabled = false };
            _ctxMenu.Items.Add(miTitle);
            _ctxMenu.Items.Add(new ToolStripSeparator());
            if (cmd is not null)
            {
                var miCopy = new ToolStripMenuItem("📋 Copy install command");
                string capturedCmd = cmd;
                miCopy.Click += (_, _) => Clipboard.SetText(capturedCmd);
                _ctxMenu.Items.Add(miCopy);
            }
            if (url is not null)
            {
                var miDocs = new ToolStripMenuItem("🌐 Open download page");
                string capturedUrl = url;
                miDocs.Click += (_, _) => Process.Start(new ProcessStartInfo(capturedUrl) { UseShellExecute = true });
                _ctxMenu.Items.Add(miDocs);
            }
        };
    }

    private void OnToolSelectionChanged(object? sender, EventArgs e)
    {
        if (_lstTools.SelectedItems.Count == 0)
        {
            _installGuidePanel.Visible = false;
            return;
        }
        var item = _lstTools.SelectedItems[0];
        bool notFound = item.SubItems.Count > 3 && item.SubItems[3].Text.StartsWith("Not Found", StringComparison.OrdinalIgnoreCase);
        if (!notFound)
        {
            _installGuidePanel.Visible = false;
            return;
        }
        string toolName = item.Text;
        var (cmd, url) = ToolVersionChecker.GetInstallGuide(toolName);
        if (cmd is null && url is null)
        {
            _installGuidePanel.Visible = false;
            return;
        }
        _lblGuideTitle.Text = $"How to install {toolName}:";
        _txtGuideCmd.Text = cmd ?? "";
        _btnGuideCopy.Visible = cmd is not null;
        _btnGuideDocs.Visible = url is not null;
        _btnGuideDocs.Tag = url ?? "";
        _installGuidePanel.Visible = true;
    }

    private async Task RefreshAsync()
    {
        _btnRefresh.Enabled = false;
        bool checkUpdates = _chkUpdates.Checked;
        _lblStatus.Text = checkUpdates ? "Checking tool versions and available updates..." : "Checking tool versions...";
        _lblStatus.ForeColor = AppTheme.FgDim;
        _lstTools.Items.Clear();

        try
        {
            var tools = checkUpdates
                ? await ToolVersionChecker.CheckAllWithUpdatesAsync(_cts.Token)
                : await ToolVersionChecker.CheckAllAsync(_cts.Token);

            _lstTools.Items.Clear();
            int updateCount = 0;
            foreach (var tool in tools)
            {
                var item = new ListViewItem(tool.Name);
                item.SubItems.Add(tool.InstalledVersion ?? "—");
                item.SubItems.Add(tool.LatestVersion ?? "—");

                if (!tool.IsInstalled)
                {
                    item.SubItems.Add("Not Found");
                    item.ForeColor = AppTheme.Red;
                }
                else if (tool.UpdateAvailable)
                {
                    item.SubItems.Add("⬆ Update Available");
                    item.ForeColor = AppTheme.Yellow;
                    updateCount++;
                }
                else
                {
                    item.SubItems.Add("✓ Up to Date");
                    item.ForeColor = AppTheme.Green;
                }
                _lstTools.Items.Add(item);
            }

            int found = tools.Count(t => t.IsInstalled);
            string statusText = $"{found}/{tools.Count} tool(s) found";
            if (checkUpdates && updateCount > 0)
                statusText += $"  •  {updateCount} update(s) available";
            _lblStatus.Text = statusText;
            _lblStatus.ForeColor = updateCount > 0 ? AppTheme.Yellow : AppTheme.Fg;
        }
        catch (Exception ex)
        {
            _lblStatus.Text = $"Error: {ex.Message}";
            _lblStatus.ForeColor = AppTheme.Red;
        }
        finally
        {
            _btnRefresh.Enabled = true;
        }
    }
}
