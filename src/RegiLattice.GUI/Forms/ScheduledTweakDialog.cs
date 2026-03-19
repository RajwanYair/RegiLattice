// RegiLattice.GUI — Forms/ScheduledTweakDialog.cs
// Sprint 50: GUI for adding, viewing, and removing scheduled tweak tasks.

#nullable enable

using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using RegiLattice.Core.Services;

namespace RegiLattice.GUI.Forms;

/// <summary>
/// Dialog for managing scheduled tweak tasks.
/// Uses <see cref="ScheduledTweakService"/> to persist entries in
/// <c>%LOCALAPPDATA%\RegiLattice\scheduled-tweaks.json</c>.
/// </summary>
internal sealed class ScheduledTweakDialog : BaseDialog
{
    // ── Service ───────────────────────────────────────────────────────────────
    private readonly ScheduledTweakService _svc = new();

    // ── Controls ──────────────────────────────────────────────────────────────
    private readonly ListView _list = new()
    {
        Dock = DockStyle.Fill,
        FullRowSelect = true,
        MultiSelect = false,
        View = View.Details,
        GridLines = true,
    };
    private readonly Button _btnAdd = new()
    {
        Text = "Add Schedule…",
        Width = 110,
        Height = 28,
    };
    private readonly Button _btnRemove = new()
    {
        Text = "Remove",
        Width = 80,
        Height = 28,
        Enabled = false,
    };
    private readonly Button _btnToggle = new()
    {
        Text = "Enable/Disable",
        Width = 110,
        Height = 28,
        Enabled = false,
    };
    private readonly Button _btnRefresh = new()
    {
        Text = "Refresh",
        Width = 80,
        Height = 28,
    };
    private readonly Button _btnClose = new()
    {
        Text = "Close",
        Width = 80,
        Height = 28,
    };
    private readonly Label _statusLabel = new()
    {
        Dock = DockStyle.Bottom,
        Height = 22,
        TextAlign = ContentAlignment.MiddleLeft,
        Padding = new Padding(6, 0, 0, 0),
    };

    // ── Construction ──────────────────────────────────────────────────────────
    internal ScheduledTweakDialog()
        : base("Scheduled Tweak Manager", new Size(760, 460), resizable: true)
    {
        BuildLayout();
        WireEvents();
        _svc.Load();
        RefreshList();
    }

    private void BuildLayout()
    {
        _list.Columns.AddRange(
        [
            new ColumnHeader { Text = "Tweak ID",  Width = 260 },
            new ColumnHeader { Text = "Trigger",   Width = 100 },
            new ColumnHeader { Text = "Interval",  Width = 90  },
            new ColumnHeader { Text = "Enabled",   Width = 70  },
            new ColumnHeader { Text = "Last Run",  Width = 160 },
        ]);

        var btnPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            Height = 44,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false,
            Padding = new Padding(6, 6, 6, 6),
        };
        btnPanel.Controls.AddRange([_btnAdd, _btnRemove, _btnToggle, _btnRefresh, _btnClose]);

        Controls.Add(_list);
        Controls.Add(_statusLabel);
        Controls.Add(btnPanel);
    }

    private void WireEvents()
    {
        _btnAdd.Click += OnAddSchedule;
        _btnRemove.Click += OnRemoveSchedule;
        _btnToggle.Click += OnToggleSchedule;
        _btnRefresh.Click += (_, _) => { _svc.Load(); RefreshList(); };
        _btnClose.Click += (_, _) => Close();
        _list.SelectedIndexChanged += (_, _) => UpdateButtons();
    }

    // ── List population ───────────────────────────────────────────────────────
    private void RefreshList()
    {
        _list.BeginUpdate();
        _list.Items.Clear();

        foreach (TweakSchedule s in _svc.Schedules)
        {
            string interval = s.Trigger == ScheduleTrigger.Timer
                ? $"{s.IntervalMinutes} min"
                : "—";
            string lastRun = s.LastRun.HasValue
                ? s.LastRun.Value.ToLocalTime().ToString("yyyy-MM-dd HH:mm")
                : "Never";

            var item = new ListViewItem(s.TweakId) { Tag = s };
            item.SubItems.Add(s.Trigger.ToString());
            item.SubItems.Add(interval);
            item.SubItems.Add(s.Enabled ? "✓" : "✗");
            item.SubItems.Add(lastRun);
            item.ForeColor = s.Enabled ? SystemColors.WindowText : SystemColors.GrayText;
            _list.Items.Add(item);
        }

        _list.EndUpdate();
        UpdateButtons();
        _statusLabel.Text = $"{_svc.Schedules.Count} schedule(s) loaded.";
    }

    private void UpdateButtons()
    {
        bool sel = _list.SelectedItems.Count > 0;
        _btnRemove.Enabled = sel;
        _btnToggle.Enabled = sel;
    }

    // ── Add schedule wizard ───────────────────────────────────────────────────
    private void OnAddSchedule(object? sender, EventArgs e)
    {
        using var dlg = new Form
        {
            Text = "Add Scheduled Tweak",
            Size = new Size(380, 230),
            FormBorderStyle = FormBorderStyle.FixedDialog,
            StartPosition = FormStartPosition.CenterParent,
            MaximizeBox = false,
            MinimizeBox = false,
        };

        var lblId = new Label { Text = "Tweak ID:", Location = new Point(12, 18), Width = 90 };
        var txtId = new TextBox { Location = new Point(110, 15), Width = 240, PlaceholderText = "e.g. perf-disable-animations" };

        var lblTrigger = new Label { Text = "Trigger:", Location = new Point(12, 52), Width = 90 };
        var cboTrigger = new ComboBox
        {
            Location = new Point(110, 49),
            Width = 160,
            DropDownStyle = ComboBoxStyle.DropDownList,
        };
        cboTrigger.Items.AddRange(Enum.GetNames<ScheduleTrigger>());
        cboTrigger.SelectedIndex = 0;

        var lblInterval = new Label { Text = "Interval (min):", Location = new Point(12, 86), Width = 95 };
        var numInterval = new NumericUpDown
        {
            Location = new Point(110, 83),
            Width = 80,
            Minimum = 1,
            Maximum = 10080,
            Value = 60,
            Enabled = false,
        };
        cboTrigger.SelectedIndexChanged += (_, _) =>
            numInterval.Enabled = cboTrigger.SelectedItem?.ToString() == ScheduleTrigger.Timer.ToString();

        var btnOk = new Button { Text = "Add", DialogResult = DialogResult.OK, Location = new Point(190, 160), Width = 75 };
        var btnCancel = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel, Location = new Point(275, 160), Width = 75 };
        dlg.AcceptButton = btnOk;
        dlg.CancelButton = btnCancel;

        dlg.Controls.AddRange([lblId, txtId, lblTrigger, cboTrigger, lblInterval, numInterval, btnOk, btnCancel]);

        if (dlg.ShowDialog(this) != DialogResult.OK)
            return;

        string tweakId = txtId.Text.Trim();
        if (string.IsNullOrWhiteSpace(tweakId))
        {
            MessageBox.Show("Please enter a Tweak ID.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var trigger = Enum.Parse<ScheduleTrigger>(cboTrigger.SelectedItem!.ToString()!);
        var schedule = new TweakSchedule
        {
            TweakId = tweakId,
            Trigger = trigger,
            IntervalMinutes = trigger == ScheduleTrigger.Timer ? (int)numInterval.Value : 0,
        };

        _svc.AddSchedule(schedule);
        _svc.Save();
        RefreshList();
        _statusLabel.Text = $"Schedule added for '{tweakId}'.";
    }

    // ── Remove ────────────────────────────────────────────────────────────────
    private void OnRemoveSchedule(object? sender, EventArgs e)
    {
        if (_list.SelectedItems.Count == 0 || _list.SelectedItems[0].Tag is not TweakSchedule s)
            return;

        var result = MessageBox.Show(
            $"Remove schedule for '{s.TweakId}'?",
            "Confirm",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (result != DialogResult.Yes)
            return;

        _svc.RemoveSchedule(s.TweakId);
        _svc.Save();
        RefreshList();
        _statusLabel.Text = $"Schedule removed for '{s.TweakId}'.";
    }

    // ── Toggle enabled ────────────────────────────────────────────────────────
    private void OnToggleSchedule(object? sender, EventArgs e)
    {
        if (_list.SelectedItems.Count == 0 || _list.SelectedItems[0].Tag is not TweakSchedule s)
            return;

        _svc.SetEnabled(s.TweakId, !s.Enabled);
        _svc.Save();
        RefreshList();
        _statusLabel.Text = $"Schedule '{s.TweakId}' {(s.Enabled ? "disabled" : "enabled")}.";
    }
}
