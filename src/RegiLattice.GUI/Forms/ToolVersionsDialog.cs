using RegiLattice.GUI.PackageManagers;

namespace RegiLattice.GUI.Forms;

/// <summary>Shows installed versions of external tools used by RegiLattice.</summary>
internal sealed class ToolVersionsDialog : Form
{
    private readonly ListView _lstTools = new();
    private readonly Label _lblStatus = new();
    private readonly Button _btnRefresh = new();

    private CancellationTokenSource _cts = new();

    internal ToolVersionsDialog()
    {
        Text = "🔧 External Tool Versions";
        Icon = SystemIcons.Information;
        FormBorderStyle = FormBorderStyle.Sizable;
        StartPosition = FormStartPosition.CenterParent;
        MinimumSize = new Size(460, 360);
        ClientSize = new Size(500, 420);
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
        _lstTools.Columns.AddRange(
        [
            new ColumnHeader { Text = "Tool", Width = 140 },
            new ColumnHeader { Text = "Version", Width = 160 },
            new ColumnHeader { Text = "Status", Width = 140 },
        ]);

        var ctrlPanel = new Panel { Dock = DockStyle.Bottom, Height = 42, BackColor = AppTheme.Surface };
        _btnRefresh.Text = "Refresh";
        _btnRefresh.BackColor = AppTheme.Accent;
        _btnRefresh.ForeColor = AppTheme.Bg;
        _btnRefresh.FlatStyle = FlatStyle.Flat;
        _btnRefresh.Location = new Point(8, 7);
        _btnRefresh.Size = new Size(90, 27);
        _btnRefresh.Click += async (_, _) => await RefreshAsync();
        ctrlPanel.Controls.Add(_btnRefresh);

        Controls.AddRange([ctrlPanel, _lstTools, _lblStatus, lblTitle]);
    }

    private async Task RefreshAsync()
    {
        _btnRefresh.Enabled = false;
        _lblStatus.Text = "Checking tool versions...";
        _lblStatus.ForeColor = AppTheme.FgDim;
        _lstTools.Items.Clear();

        try
        {
            var tools = await ToolVersionChecker.CheckAllAsync(_cts.Token);
            _lstTools.Items.Clear();
            foreach (var tool in tools)
            {
                var item = new ListViewItem(tool.Name);
                item.SubItems.Add(tool.InstalledVersion ?? "—");
                item.SubItems.Add(tool.IsInstalled ? "Installed" : "Not Found");
                item.ForeColor = tool.IsInstalled ? AppTheme.Green : AppTheme.Red;
                _lstTools.Items.Add(item);
            }
            int found = tools.Count(t => t.IsInstalled);
            _lblStatus.Text = $"{found}/{tools.Count} tool(s) found";
            _lblStatus.ForeColor = AppTheme.Fg;
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
