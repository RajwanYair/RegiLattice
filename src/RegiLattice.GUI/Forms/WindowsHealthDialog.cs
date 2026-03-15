// RegiLattice.GUI — Forms/WindowsHealthDialog.cs
// Windows Health & Maintenance dialog — runs DISM, SFC, disk, network and power diagnostics.

using RegiLattice.GUI.PackageManagers;

namespace RegiLattice.GUI.Forms;

/// <summary>
/// Windows Health dialog.
/// Lists system maintenance commands (DISM, SFC, disk cleanup, network, power) with
/// description, admin requirement badge, and a "Run" button per command.
/// Output is shown in a scrollable log panel.
/// </summary>
internal sealed class WindowsHealthDialog : Form
{
    private readonly ListView _lstCommands = new();
    private readonly TextBox _txtLog = new();
    private readonly Label _lblStatus = new();
    private readonly Button _btnRunSelected = new();
    private readonly Button _btnRunAll = new();
    private readonly Button _btnClear = new();
    private readonly Button _btnClose = new();
    private readonly ProgressBar _progressBar = new();

    private CancellationTokenSource _cts = new();
    private readonly bool _isAdmin;

    internal WindowsHealthDialog()
    {
        _isAdmin = WindowsHealthManager.IsAdmin();

        Text = "\U0001F3E5 Windows Health & Maintenance";
        Icon = AppIcons.WindowsHealthIcon;
        FormBorderStyle = FormBorderStyle.Sizable;
        StartPosition = FormStartPosition.CenterParent;
        MinimumSize = new Size(700, 580);
        ClientSize = new Size(780, 680);
        BackColor = AppTheme.Bg;
        ForeColor = AppTheme.Fg;
        Font = AppTheme.Regular;

        BuildLayout();
        Load += (_, _) => PopulateCommands();
        FormClosed += (_, _) => _cts.Cancel();
    }

    private void BuildLayout()
    {
        var lblTitle = new Label
        {
            Text = "Windows Health & Maintenance",
            Font = AppTheme.Title,
            ForeColor = AppTheme.Accent,
            Dock = DockStyle.Top,
            Height = 36,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(8, 0, 0, 0),
        };

        // Admin status badge
        var adminPanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 30,
            BackColor = _isAdmin ? Color.FromArgb(20, AppTheme.Green) : Color.FromArgb(20, AppTheme.Yellow),
            Padding = new Padding(8, 4, 8, 4),
        };
        var lblAdmin = new Label
        {
            Text = _isAdmin
                ? "\u2705 Running as Administrator \u2014 all commands available"
                : "\u26A0 Not running as Administrator \u2014 some commands may fail",
            AutoSize = true,
            Location = new Point(8, 6),
            ForeColor = _isAdmin ? AppTheme.Green : AppTheme.Yellow,
        };
        adminPanel.Controls.Add(lblAdmin);

        _lblStatus.Dock = DockStyle.Top;
        _lblStatus.Height = 24;
        _lblStatus.Padding = new Padding(8, 0, 0, 0);
        _lblStatus.Text = $"{WindowsHealthManager.Commands.Count} commands available";
        _lblStatus.ForeColor = AppTheme.FgDim;

        // Command list
        _lstCommands.Dock = DockStyle.Top;
        _lstCommands.Height = 260;
        _lstCommands.View = View.Details;
        _lstCommands.FullRowSelect = true;
        _lstCommands.GridLines = false;
        _lstCommands.CheckBoxes = true;
        _lstCommands.MultiSelect = true;
        _lstCommands.BackColor = AppTheme.Bg;
        _lstCommands.ForeColor = AppTheme.Fg;
        _lstCommands.Font = AppTheme.Regular;
        _lstCommands.BorderStyle = BorderStyle.None;
        _lstCommands.Columns.AddRange([
            new ColumnHeader { Text = "Command", Width = 260 },
            new ColumnHeader { Text = "Description", Width = 340 },
            new ColumnHeader { Text = "Admin", Width = 60 },
            new ColumnHeader { Text = "Status", Width = 90 },
        ]);

        // Progress bar
        _progressBar.Dock = DockStyle.Top;
        _progressBar.Height = 6;
        _progressBar.Style = ProgressBarStyle.Continuous;
        _progressBar.Visible = false;

        // Log output
        _txtLog.Dock = DockStyle.Fill;
        _txtLog.Multiline = true;
        _txtLog.ScrollBars = ScrollBars.Both;
        _txtLog.ReadOnly = true;
        _txtLog.WordWrap = false;
        _txtLog.BackColor = AppTheme.Surface;
        _txtLog.ForeColor = AppTheme.Fg;
        _txtLog.Font = AppTheme.Mono;
        _txtLog.BorderStyle = BorderStyle.None;

        // Buttons
        var ctrlPanel = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 42,
            BackColor = AppTheme.Surface,
        };

        void StyleBtn(Button b, Color bg, string text, int x)
        {
            b.Text = text;
            b.BackColor = bg;
            b.ForeColor = AppTheme.Bg;
            b.FlatStyle = FlatStyle.Flat;
            b.Location = new Point(x, 7);
            b.Size = new Size(110, 28);
        }

        StyleBtn(_btnRunSelected, AppTheme.Green, "Run Selected", 8);
        StyleBtn(_btnRunAll, AppTheme.Accent, "Run All", 126);
        StyleBtn(_btnClear, AppTheme.Overlay, "Clear Log", 244);
        _btnClear.ForeColor = AppTheme.Fg;

        _btnClose.Text = "Close";
        _btnClose.BackColor = AppTheme.Overlay;
        _btnClose.ForeColor = AppTheme.Fg;
        _btnClose.FlatStyle = FlatStyle.Flat;
        _btnClose.Size = new Size(80, 28);
        _btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        _btnClose.Location = new Point(ctrlPanel.ClientSize.Width - 88, 7);
        _btnClose.Click += (_, _) => Close();

        _btnRunSelected.Click += async (_, _) => await RunSelectedAsync();
        _btnRunAll.Click += async (_, _) => await RunAllAsync();
        _btnClear.Click += (_, _) =>
        {
            _txtLog.Clear();
            ResetStatuses();
        };

        ctrlPanel.Controls.AddRange([_btnRunSelected, _btnRunAll, _btnClear, _btnClose]);

        // Splitter label between list and log
        var lblLog = new Label
        {
            Text = " Output Log",
            Dock = DockStyle.Top,
            Height = 22,
            ForeColor = AppTheme.FgDim,
            BackColor = AppTheme.Surface,
            Padding = new Padding(8, 4, 0, 0),
        };

        // Order: bottom-up for Dock layout
        Controls.AddRange([_txtLog, ctrlPanel, lblLog, _progressBar, _lstCommands, _lblStatus, adminPanel, lblTitle]);
    }

    private void PopulateCommands()
    {
        _lstCommands.Items.Clear();
        foreach (var cmd in WindowsHealthManager.Commands)
        {
            var item = new ListViewItem(cmd.Label) { Tag = cmd };
            item.SubItems.Add(cmd.Description);
            item.SubItems.Add(cmd.NeedsAdmin ? "Yes" : "No");
            item.SubItems.Add("Ready");
            item.ForeColor = AppTheme.Fg;

            if (cmd.NeedsAdmin && !_isAdmin)
                item.ForeColor = AppTheme.FgDim;

            _lstCommands.Items.Add(item);
        }
    }

    private void ResetStatuses()
    {
        foreach (ListViewItem item in _lstCommands.Items)
            item.SubItems[3].Text = "Ready";
    }

    private IReadOnlyList<(ListViewItem Item, WindowsHealthManager.HealthCommand Cmd)> GetCheckedCommands()
    {
        var list = new List<(ListViewItem, WindowsHealthManager.HealthCommand)>();
        foreach (ListViewItem item in _lstCommands.CheckedItems)
        {
            if (item.Tag is WindowsHealthManager.HealthCommand cmd)
                list.Add((item, cmd));
        }
        return list;
    }

    private async Task RunSelectedAsync()
    {
        var commands = GetCheckedCommands();
        if (commands.Count == 0)
        {
            _lblStatus.Text = "No commands selected \u2014 check the boxes next to commands to run.";
            _lblStatus.ForeColor = AppTheme.Yellow;
            return;
        }
        await RunBatchAsync(commands);
    }

    private async Task RunAllAsync()
    {
        var commands = new List<(ListViewItem, WindowsHealthManager.HealthCommand)>();
        foreach (ListViewItem item in _lstCommands.Items)
        {
            if (item.Tag is WindowsHealthManager.HealthCommand cmd)
                commands.Add((item, cmd));
        }
        await RunBatchAsync(commands);
    }

    private async Task RunBatchAsync(IReadOnlyList<(ListViewItem Item, WindowsHealthManager.HealthCommand Cmd)> commands)
    {
        SetBusy(true);
        _progressBar.Visible = true;
        _progressBar.Maximum = commands.Count;
        _progressBar.Value = 0;

        int passed = 0;
        int failed = 0;

        foreach (var (item, cmd) in commands)
        {
            if (_cts.IsCancellationRequested)
                break;

            item.SubItems[3].Text = "\u23F3 Running...";
            item.SubItems[3].ForeColor = AppTheme.Yellow;
            _lstCommands.EnsureVisible(item.Index);

            AppendLog(
                $"\u2500\u2500 {cmd.Label} \u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500"
            );
            AppendLog($"> {cmd.FileName} {string.Join(' ', cmd.Args)}");
            _lblStatus.Text = $"Running: {cmd.Label}...";
            _lblStatus.ForeColor = AppTheme.Yellow;

            try
            {
                var (exitCode, output) = await WindowsHealthManager.RunCommandAsync(cmd, _cts.Token);

                if (!string.IsNullOrWhiteSpace(output))
                    AppendLog(output);

                if (exitCode == 0)
                {
                    item.SubItems[3].Text = "\u2714 Done";
                    item.SubItems[3].ForeColor = AppTheme.Green;
                    AppendLog($"\u2714 Exit code: {exitCode}");
                    passed++;
                }
                else
                {
                    item.SubItems[3].Text = $"\u26A0 Code {exitCode}";
                    item.SubItems[3].ForeColor = AppTheme.Yellow;
                    AppendLog($"\u26A0 Exit code: {exitCode}");
                    failed++;
                }
            }
            catch (OperationCanceledException)
            {
                item.SubItems[3].Text = "\u274C Cancelled";
                item.SubItems[3].ForeColor = AppTheme.Red;
                AppendLog("\u274C Command cancelled.");
                break;
            }
            catch (Exception ex)
            {
                item.SubItems[3].Text = "\u274C Error";
                item.SubItems[3].ForeColor = AppTheme.Red;
                AppendLog($"\u274C Error: {ex.Message}");
                failed++;
            }

            AppendLog("");
            _progressBar.Value = Math.Min(_progressBar.Value + 1, _progressBar.Maximum);
        }

        _progressBar.Visible = false;
        _lblStatus.Text = $"Completed: {passed} succeeded, {failed} failed";
        _lblStatus.ForeColor = failed > 0 ? AppTheme.Yellow : AppTheme.Green;
        SetBusy(false);
    }

    private void AppendLog(string line)
    {
        _txtLog.AppendText(line + Environment.NewLine);
    }

    private void SetBusy(bool busy)
    {
        _btnRunSelected.Enabled = !busy;
        _btnRunAll.Enabled = !busy;
    }
}
