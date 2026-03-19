// RegiLattice.GUI — Forms/ScheduledTaskManagerDialog.cs
// Sprint 30: Scheduled task manager dialog.

#nullable enable

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using RegiLattice.Core;
using RegiLattice.Core.Services;

namespace RegiLattice.GUI.Forms;

/// <summary>
/// Dialog for listing, filtering, enabling, disabling, and deleting
/// Windows scheduled tasks.
/// </summary>
internal sealed class ScheduledTaskManagerDialog : BaseDialog
{
    // ── Controls ──────────────────────────────────────────────────────────────
    private readonly TextBox _searchBox = new() { Width = 220, PlaceholderText = "Filter tasks…" };
    private readonly ComboBox _filterCombo = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 130 };
    private readonly ListView _list = new()
    {
        Dock = DockStyle.Fill,
        FullRowSelect = true,
        MultiSelect = false,
        View = View.Details,
        GridLines = true,
    };
    private readonly Panel _topPanel = new()
    {
        Dock = DockStyle.Top,
        Height = 40,
        Padding = new Padding(6, 6, 6, 2),
    };
    private readonly FlowLayoutPanel _btnPanel = new()
    {
        Dock = DockStyle.Bottom,
        Height = 42,
        Padding = new Padding(6, 6, 6, 4),
        FlowDirection = FlowDirection.LeftToRight,
        WrapContents = false,
    };
    private readonly Button _btnEnable = new()
    {
        Text = "Enable",
        Width = 80,
        Enabled = false,
    };
    private readonly Button _btnDisable = new()
    {
        Text = "Disable",
        Width = 80,
        Enabled = false,
    };
    private readonly Button _btnRunNow = new()
    {
        Text = "Run Now",
        Width = 80,
        Enabled = false,
    };
    private readonly Button _btnDelete = new()
    {
        Text = "Delete",
        Width = 80,
        Enabled = false,
    };
    private readonly Button _btnRefresh = new() { Text = "Refresh", Width = 80 };
    private readonly Button _btnClose = new() { Text = "Close", Width = 80 };
    private readonly Label _statusLabel = new()
    {
        Dock = DockStyle.Bottom,
        Height = 22,
        TextAlign = ContentAlignment.MiddleLeft,
        Padding = new Padding(6, 0, 0, 0),
    };
    private readonly Panel _adminBanner = new()
    {
        Dock = DockStyle.Top,
        Height = 28,
        BackColor = Color.FromArgb(50, 150, 250),
        Visible = false,
    };
    private readonly Label _adminLabel = new()
    {
        Dock = DockStyle.Fill,
        ForeColor = Color.White,
        TextAlign = ContentAlignment.MiddleCenter,
    };

    private IReadOnlyList<ScheduledTaskEntry> _allTasks = Array.Empty<ScheduledTaskEntry>();
    private CancellationTokenSource _cts = new();
    private bool _busy;

    // ── Construction ──────────────────────────────────────────────────────────
    internal ScheduledTaskManagerDialog()
        : base("Scheduled Task Manager", new Size(900, 560), resizable: true)
    {
        BuildLayout();
        ApplyTheme();
        _btnRefresh.Click += async (_, _) => await RefreshAsync().ConfigureAwait(false);
        _btnClose.Click += (_, _) => Close();
        _btnEnable.Click += async (_, _) => await ApplyOperationAsync(ScheduledTaskManager.EnableAsync).ConfigureAwait(false);
        _btnDisable.Click += async (_, _) => await ApplyOperationAsync(ScheduledTaskManager.DisableAsync).ConfigureAwait(false);
        _btnDelete.Click += async (_, _) => await ConfirmDeleteAsync().ConfigureAwait(false);
        _btnRunNow.Click += async (_, _) => await ApplyOperationAsync(ScheduledTaskManager.RunNowAsync).ConfigureAwait(false);
        _searchBox.TextChanged += (_, _) => ApplyFilter();
        _filterCombo.SelectedIndexChanged += (_, _) => ApplyFilter();
        _list.SelectedIndexChanged += (_, _) => UpdateButtons();

        Load += async (_, _) => await RefreshAsync().ConfigureAwait(false);

        if (!Elevation.IsAdmin())
        {
            _adminLabel.Text = "⚠  Administrator rights required to enable, disable, or delete tasks.";
            _adminBanner.Visible = true;
        }
    }

    // ── Layout ────────────────────────────────────────────────────────────────
    private void BuildLayout()
    {
        _list.Columns.AddRange([
            new ColumnHeader { Text = "Task Name", Width = 300 },
            new ColumnHeader { Text = "Status", Width = 90 },
            new ColumnHeader { Text = "Next Run", Width = 150 },
            new ColumnHeader { Text = "Last Run", Width = 150 },
            new ColumnHeader { Text = "Author", Width = 200 },
        ]);
        ListViewColumnSorter.AttachTo(_list);

        _filterCombo.Items.AddRange(["All", "Ready", "Disabled", "Running"]);
        _filterCombo.SelectedIndex = 0;

        _adminBanner.Controls.Add(_adminLabel);

        _topPanel.Controls.AddRange([
            _searchBox,
            new Label
            {
                Text = "Status:",
                AutoSize = true,
                Margin = new Padding(8, 3, 4, 0),
            },
            _filterCombo,
        ]);

        _btnPanel.Controls.AddRange([_btnEnable, _btnDisable, _btnRunNow, _btnDelete, _btnRefresh, _btnClose]);

        Controls.AddRange([_list, _statusLabel, _btnPanel, _topPanel, _adminBanner]);
    }

    // ── Refresh ────────────────────────────────────────────────────────────────
    private async Task RefreshAsync()
    {
        if (_busy)
            return;
        SetBusy(true, "Loading scheduled tasks…");
        try
        {
            _cts.Cancel();
            _cts = new CancellationTokenSource();
            _allTasks = await Task.Run(ScheduledTaskManager.GetAllTasks, _cts.Token).ConfigureAwait(false);
            ApplyFilter();
            _statusLabel.Text = $"{_allTasks.Count} tasks found.";
        }
        finally
        {
            SetBusy(false);
        }
    }

    private void ApplyFilter()
    {
        string search = _searchBox.Text.Trim();
        string filter = _filterCombo.SelectedItem?.ToString() ?? "All";

        var filtered = _allTasks.AsEnumerable();
        if (!string.IsNullOrEmpty(search))
            filtered = filtered.Where(t =>
                t.TaskName.Contains(search, StringComparison.OrdinalIgnoreCase) || t.Author.Contains(search, StringComparison.OrdinalIgnoreCase)
            );

        if (filter != "All")
        {
            var status = filter switch
            {
                "Ready" => ScheduledTaskStatus.Ready,
                "Disabled" => ScheduledTaskStatus.Disabled,
                "Running" => ScheduledTaskStatus.Running,
                _ => (ScheduledTaskStatus?)null,
            };
            if (status.HasValue)
                filtered = filtered.Where(t => t.Status == status.Value);
        }

        _list.SuspendLayout();
        _list.BeginUpdate();
        _list.Items.Clear();
        foreach (var t in filtered)
        {
            var item = new ListViewItem(t.TaskName) { Tag = t };
            item.SubItems.Add(t.Status.ToString());
            item.SubItems.Add(t.NextRunTime);
            item.SubItems.Add(t.LastRunTime);
            item.SubItems.Add(t.Author);
            if (t.Status == ScheduledTaskStatus.Disabled)
                item.ForeColor = AppTheme.FgDim;
            _list.Items.Add(item);
        }
        _list.EndUpdate();
        _list.ResumeLayout();
        UpdateButtons();
    }

    private void UpdateButtons()
    {
        bool selected = _list.SelectedItems.Count > 0;
        bool isAdmin = Elevation.IsAdmin();
        _btnEnable.Enabled = selected && isAdmin;
        _btnDisable.Enabled = selected && isAdmin;
        _btnRunNow.Enabled = selected && isAdmin;
        _btnDelete.Enabled = selected && isAdmin;
    }

    private async Task ApplyOperationAsync(Func<string, CancellationToken, Task> operation)
    {
        if (_list.SelectedItems.Count == 0 || _busy)
            return;
        var entry = (ScheduledTaskEntry)_list.SelectedItems[0].Tag!;
        SetBusy(true, $"Working on '{entry.DisplayName}'…");
        try
        {
            await operation(entry.TaskName, _cts.Token).ConfigureAwait(false);
            await RefreshAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Operation failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            SetBusy(false);
        }
    }

    private async Task ConfirmDeleteAsync()
    {
        if (_list.SelectedItems.Count == 0 || _busy)
            return;
        var entry = (ScheduledTaskEntry)_list.SelectedItems[0].Tag!;
        var answer = MessageBox.Show(
            $"Permanently delete task '{entry.DisplayName}'?\n\nThis cannot be undone.",
            "Confirm Delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning
        );

        if (answer != DialogResult.Yes)
            return;
        await ApplyOperationAsync(ScheduledTaskManager.DeleteAsync).ConfigureAwait(false);
    }

    private void SetBusy(bool busy, string? message = null)
    {
        _busy = busy;
        if (message != null)
            _statusLabel.Text = message;
        _btnRefresh.Enabled = !busy;
        if (!busy)
            UpdateButtons();
        Cursor = busy ? Cursors.WaitCursor : Cursors.Default;
    }

    private void ApplyTheme()
    {
        BackColor = AppTheme.Bg;
        ForeColor = AppTheme.Fg;
        _list.BackColor = AppTheme.Surface;
        _list.ForeColor = AppTheme.Fg;
        _statusLabel.BackColor = AppTheme.Overlay;
        _statusLabel.ForeColor = AppTheme.Fg;
        _topPanel.BackColor = AppTheme.Surface;
        _btnPanel.BackColor = AppTheme.Surface;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            _cts.Dispose();
        base.Dispose(disposing);
    }
}
