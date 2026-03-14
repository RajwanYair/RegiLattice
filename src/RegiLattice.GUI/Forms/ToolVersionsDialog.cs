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
