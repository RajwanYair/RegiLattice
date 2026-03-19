// RegiLattice.GUI — Forms/PowerPlanDialog.cs
// Sprint 31: Power plan manager dialog — list, switch, and manage power plans.

#nullable enable

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using RegiLattice.Core;
using RegiLattice.Core.Services;

namespace RegiLattice.GUI.Forms;

/// <summary>
/// Lists Windows power plans and lets the user switch the active plan.
/// Shows well-known plans (Balanced, High Performance, Power Saver, Ultimate Performance).
/// </summary>
internal sealed class PowerPlanDialog : BaseDialog
{
    // ── Controls ──────────────────────────────────────────────────────────────
    private readonly ListView _list = new()
    {
        Dock = DockStyle.Fill,
        FullRowSelect = true,
        MultiSelect = false,
        View = View.Details,
        GridLines = true,
    };
    private readonly FlowLayoutPanel _btnPanel = new()
    {
        Dock = DockStyle.Bottom,
        Height = 42,
        Padding = new Padding(6, 6, 6, 4),
        FlowDirection = FlowDirection.LeftToRight,
        WrapContents = false,
    };
    private readonly Button _btnActivate = new()
    {
        Text = "Set Active",
        Width = 96,
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
    private readonly Panel _quickPanel = new()
    {
        Dock = DockStyle.Top,
        Height = 40,
        Padding = new Padding(6, 6, 6, 0),
    };
    private readonly Label _quickLabel = new()
    {
        AutoSize = true,
        Text = "Quick switch: ",
        Margin = new Padding(0, 3, 6, 0),
    };
    private readonly Button _btnBalanced = new()
    {
        Text = "Balanced",
        Width = 100,
        Height = 26,
    };
    private readonly Button _btnHighPerf = new()
    {
        Text = "High Performance",
        Width = 120,
        Height = 26,
    };
    private readonly Button _btnPowerSaver = new()
    {
        Text = "Power Saver",
        Width = 100,
        Height = 26,
    };

    private IReadOnlyList<PowerPlanEntry> _plans = Array.Empty<PowerPlanEntry>();
    private CancellationTokenSource _cts = new();
    private bool _busy;

    // ── Construction ──────────────────────────────────────────────────────────
    internal PowerPlanDialog()
        : base("Power Plan Manager", new Size(640, 440), resizable: true)
    {
        BuildLayout();
        ApplyTheme();

        _btnRefresh.Click += async (_, _) => await RefreshAsync().ConfigureAwait(false);
        _btnClose.Click += (_, _) => Close();
        _btnActivate.Click += async (_, _) => await ActivateSelectedAsync().ConfigureAwait(false);
        _btnBalanced.Click += async (_, _) => await QuickActivateAsync(PowerPlanManager.Balanced).ConfigureAwait(false);
        _btnHighPerf.Click += async (_, _) => await QuickActivateAsync(PowerPlanManager.HighPerformance).ConfigureAwait(false);
        _btnPowerSaver.Click += async (_, _) => await QuickActivateAsync(PowerPlanManager.PowerSaver).ConfigureAwait(false);
        _list.SelectedIndexChanged += (_, _) => UpdateButtons();

        Load += async (_, _) => await RefreshAsync().ConfigureAwait(false);

        if (!Elevation.IsAdmin())
        {
            _adminLabel.Text = "⚠  Administrator rights required to change the active power plan.";
            _adminBanner.Visible = true;
        }
    }

    // ── Layout ────────────────────────────────────────────────────────────────
    private void BuildLayout()
    {
        _list.Columns.AddRange([
            new ColumnHeader { Text = "Plan Name", Width = 260 },
            new ColumnHeader { Text = "GUID", Width = 280 },
            new ColumnHeader { Text = "Active", Width = 70 },
        ]);

        _adminBanner.Controls.Add(_adminLabel);

        var quickFlow = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false,
        };
        quickFlow.Controls.AddRange([_quickLabel, _btnBalanced, _btnHighPerf, _btnPowerSaver]);
        _quickPanel.Controls.Add(quickFlow);

        _btnPanel.Controls.AddRange([_btnActivate, _btnRefresh, _btnClose]);

        Controls.AddRange([_list, _statusLabel, _btnPanel, _quickPanel, _adminBanner]);
    }

    // ── Operations ────────────────────────────────────────────────────────────
    private async Task RefreshAsync()
    {
        if (_busy)
            return;
        SetBusy(true, "Loading power plans…");
        try
        {
            _cts.Cancel();
            _cts = new CancellationTokenSource();
            _plans = await Task.Run(PowerPlanManager.GetAllPlans, _cts.Token).ConfigureAwait(false);
            PopulateList();
            _statusLabel.Text = $"{_plans.Count} power plans found.";
        }
        finally
        {
            SetBusy(false);
        }
    }

    private void PopulateList()
    {
        _list.SuspendLayout();
        _list.BeginUpdate();
        _list.Items.Clear();
        foreach (var plan in _plans)
        {
            var item = new ListViewItem(plan.Name) { Tag = plan };
            item.SubItems.Add(plan.Guid.ToString("D"));
            item.SubItems.Add(plan.IsActive ? "✓" : "");
            if (plan.IsActive)
            {
                item.Font = AppTheme.Bold;
                item.ForeColor = AppTheme.Accent;
            }
            _list.Items.Add(item);
        }
        _list.EndUpdate();
        _list.ResumeLayout();
        UpdateButtons();
    }

    private void UpdateButtons()
    {
        bool isAdmin = Elevation.IsAdmin();
        bool selected = _list.SelectedItems.Count > 0;
        bool selectedIsActive = selected && ((PowerPlanEntry)_list.SelectedItems[0].Tag!).IsActive;
        _btnActivate.Enabled = selected && !selectedIsActive && isAdmin;
        _btnBalanced.Enabled = isAdmin;
        _btnHighPerf.Enabled = isAdmin;
        _btnPowerSaver.Enabled = isAdmin;
    }

    private async Task ActivateSelectedAsync()
    {
        if (_list.SelectedItems.Count == 0 || _busy)
            return;
        var plan = (PowerPlanEntry)_list.SelectedItems[0].Tag!;
        await SetActivePlanWithFeedbackAsync(plan.Guid, plan.Name).ConfigureAwait(false);
    }

    private async Task QuickActivateAsync(Guid guid)
    {
        if (_busy)
            return;
        await SetActivePlanWithFeedbackAsync(guid, guid.ToString("D")).ConfigureAwait(false);
    }

    private async Task SetActivePlanWithFeedbackAsync(Guid planGuid, string planName)
    {
        SetBusy(true, $"Activating '{planName}'…");
        try
        {
            await PowerPlanManager.SetActivePlanAsync(planGuid, _cts.Token).ConfigureAwait(false);
            await RefreshAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to activate plan: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            SetBusy(false);
        }
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
        _btnPanel.BackColor = AppTheme.Surface;
        _quickPanel.BackColor = AppTheme.Surface;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            _cts.Dispose();
        base.Dispose(disposing);
    }
}
