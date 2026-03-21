// RegiLattice.GUI — Forms/PowerSchedulerDialog.cs
// Sprint 31: Timer-based automatic power plan switching.

#nullable enable

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using RegiLattice.Core;
using RegiLattice.Core.Services;

namespace RegiLattice.GUI.Forms;

/// <summary>
/// Lets the user define time windows when a specific power plan is automatically
/// activated (e.g. "Gaming Hours" 18:00–23:00 → High Performance).
/// Schedules are stored in AppConfig as JSON and a background System.Threading.Timer
/// checks them every minute while the dialog is open.
/// For a persistent background effect the user should enable Task Scheduler mode
/// (which creates a schtasks entry) or leave the main GUI running.
/// </summary>
internal sealed class PowerSchedulerDialog : BaseDialog
{
    // ── Model ─────────────────────────────────────────────────────────────────
    private sealed class PlanSchedule
    {
        public string Name { get; set; } = "";
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public Guid PlanGuid { get; set; }
        public string PlanName { get; set; } = "";
        public bool Enabled { get; set; } = true;
    }

    // ── Controls ──────────────────────────────────────────────────────────────
    private readonly ListView _list = new()
    {
        Dock = DockStyle.Fill,
        FullRowSelect = true,
        MultiSelect = false,
        View = View.Details,
        GridLines = true,
        CheckBoxes = true,
    };
    private readonly FlowLayoutPanel _btnPanel = new()
    {
        Dock = DockStyle.Bottom,
        Height = 42,
        Padding = new Padding(6, 6, 6, 4),
        FlowDirection = FlowDirection.LeftToRight,
        WrapContents = false,
    };
    private readonly Button _btnAdd = new() { Text = "Add", Width = 76 };
    private readonly Button _btnEdit = new()
    {
        Text = "Edit",
        Width = 76,
        Enabled = false,
    };
    private readonly Button _btnDelete = new()
    {
        Text = "Delete",
        Width = 76,
        Enabled = false,
    };
    private readonly Button _btnApplyNow = new() { Text = "Apply Now", Width = 90 };
    private readonly Button _btnClose = new() { Text = "Close", Width = 80 };

    // Sprint 51 §9a: apply tweak profile on power plan switch
    private readonly Button _btnApplyProfile = new() { Text = "Profile on Plan Switch…", Width = 160 };

    // Sprint 51 §9b: schedule history log
    private readonly Button _btnPlanHistory = new() { Text = "Schedule History", Width = 130 };
    private readonly List<string> _planSwitchLog = [];
    private readonly Label _statusLabel = new()
    {
        Dock = DockStyle.Bottom,
        Height = 22,
        TextAlign = ContentAlignment.MiddleLeft,
        Padding = new Padding(6, 0, 0, 0),
        Text = "Schedules are evaluated every minute while this window is open.",
    };

    private readonly List<PlanSchedule> _schedules = [];
    private IReadOnlyList<PowerPlanEntry> _plans = [];
    private System.Threading.Timer? _checkTimer;
    private CancellationTokenSource _cts = new();

    // ── Construction ──────────────────────────────────────────────────────────
    internal PowerSchedulerDialog()
        : base("Power Plan Scheduler", new Size(660, 440), resizable: true)
    {
        BuildLayout();

        _btnAdd.Click += OnAdd;
        _btnEdit.Click += OnEdit;
        _btnDelete.Click += OnDelete;
        _btnApplyNow.Click += async (_, _) => await ApplyCurrentScheduleAsync().ConfigureAwait(false);
        _btnClose.Click += (_, _) => Close();
        _list.SelectedIndexChanged += (_, _) => UpdateButtons();
        _list.ItemChecked += OnItemChecked;

        Load += async (_, _) =>
        {
            _plans = await Task.Run(PowerPlanManager.GetAllPlans, _cts.Token).ConfigureAwait(false);
            LoadBuiltinSchedules();
            PopulateList();
            StartTimer();
        };

        FormClosing += (_, _) =>
        {
            _cts.Cancel();
            _checkTimer?.Dispose();
        };

        if (!Elevation.IsAdmin())
            Controls.Add(CreateAdminBanner("⚠  Administrator rights required to activate power plans."));
    }

    // ── Layout ────────────────────────────────────────────────────────────────
    private void BuildLayout()
    {
        _list.Columns.AddRange([
            new ColumnHeader { Text = "Schedule Name", Width = 180 },
            new ColumnHeader { Text = "Start", Width = 70 },
            new ColumnHeader { Text = "End", Width = 70 },
            new ColumnHeader { Text = "Power Plan", Width = 190 },
            new ColumnHeader { Text = "Active Now", Width = 88 },
        ]);
        ListViewColumnSorter.AttachTo(_list);

        _btnApplyProfile.Click += OnConfigureProfileOnSwitch;
        _btnPlanHistory.Click += OnShowPlanHistory;
        _btnPanel.Controls.AddRange([_btnAdd, _btnEdit, _btnDelete, _btnApplyNow, _btnApplyProfile, _btnPlanHistory, _btnClose]);
        Controls.AddRange([_list, _statusLabel, _btnPanel]);
    }

    // ── Default Schedules ─────────────────────────────────────────────────────
    private void LoadBuiltinSchedules()
    {
        // Seed some example schedules on first run
        if (_schedules.Count > 0)
            return;

        _schedules.Add(
            new PlanSchedule
            {
                Name = "Work Hours",
                StartTime = new TimeOnly(9, 0),
                EndTime = new TimeOnly(17, 0),
                PlanGuid = PowerPlanManager.Balanced,
                PlanName = "Balanced",
                Enabled = false,
            }
        );
        _schedules.Add(
            new PlanSchedule
            {
                Name = "Gaming Hours",
                StartTime = new TimeOnly(18, 0),
                EndTime = new TimeOnly(23, 0),
                PlanGuid = PowerPlanManager.HighPerformance,
                PlanName = "High Performance",
                Enabled = false,
            }
        );
        _schedules.Add(
            new PlanSchedule
            {
                Name = "Night / Sleep",
                StartTime = new TimeOnly(23, 0),
                EndTime = new TimeOnly(7, 0),
                PlanGuid = PowerPlanManager.PowerSaver,
                PlanName = "Power Saver",
                Enabled = false,
            }
        );
    }

    // ── ListView ──────────────────────────────────────────────────────────────
    private void PopulateList()
    {
        if (InvokeRequired)
        {
            Invoke(PopulateList);
            return;
        }

        _list.SuspendLayout();
        _list.BeginUpdate();
        _list.Items.Clear();

        var now = TimeOnly.FromDateTime(DateTime.Now);
        foreach (var s in _schedules)
        {
            bool activeNow = s.Enabled && IsInWindow(now, s.StartTime, s.EndTime);
            var item = new ListViewItem(s.Name) { Tag = s, Checked = s.Enabled };
            item.SubItems.Add(s.StartTime.ToString("HH:mm"));
            item.SubItems.Add(s.EndTime.ToString("HH:mm"));
            item.SubItems.Add(s.PlanName);
            item.SubItems.Add(activeNow ? "✓" : "");
            if (activeNow)
                item.ForeColor = AppTheme.Accent;
            _list.Items.Add(item);
        }

        _list.EndUpdate();
        _list.ResumeLayout();
        UpdateButtons();
    }

    private void UpdateButtons()
    {
        bool sel = _list.SelectedItems.Count > 0;
        _btnEdit.Enabled = sel;
        _btnDelete.Enabled = sel;
    }

    // ── Timer ─────────────────────────────────────────────────────────────────
    private void StartTimer()
    {
        _checkTimer = new System.Threading.Timer(_ => _ = ApplyCurrentScheduleAsync(), null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));
    }

    private async Task ApplyCurrentScheduleAsync()
    {
        if (!Elevation.IsAdmin())
            return;

        var now = TimeOnly.FromDateTime(DateTime.Now);
        PlanSchedule? match = null;
        foreach (var s in _schedules)
        {
            if (s.Enabled && IsInWindow(now, s.StartTime, s.EndTime))
            {
                match = s;
                break;
            }
        }

        if (match is null)
            return;

        var active = await Task.Run(() => PowerPlanManager.GetActivePlanGuid(), _cts.Token).ConfigureAwait(false);
        if (active == match.PlanGuid)
            return;

        try
        {
            await PowerPlanManager.SetActivePlanAsync(match.PlanGuid, _cts.Token).ConfigureAwait(false);
            SetStatus($"Auto-switched to '{match.PlanName}' ({match.Name} schedule).");
        }
        catch (Exception ex)
        {
            SetStatus($"Auto-switch failed: {ex.Message}");
        }

        PopulateList();
    }

    private static bool IsInWindow(TimeOnly now, TimeOnly start, TimeOnly end)
    {
        // handle overnight windows (end < start)
        if (end < start)
            return now >= start || now <= end;
        return now >= start && now <= end;
    }

    private void SetStatus(string text)
    {
        if (InvokeRequired)
        {
            Invoke(() => SetStatus(text));
            return;
        }
        _statusLabel.Text = text;
    }

    // ── Item Checked ──────────────────────────────────────────────────────────
    private void OnItemChecked(object? sender, ItemCheckedEventArgs e)
    {
        if (e.Item.Tag is PlanSchedule s)
            s.Enabled = e.Item.Checked;
    }

    // ── Add / Edit ────────────────────────────────────────────────────────────
    private void OnAdd(object? sender, EventArgs e)
    {
        using var dlg = new ScheduleEditDialog(_plans);
        if (dlg.ShowDialog(this) == DialogResult.OK && dlg.Result != null)
        {
            _schedules.Add(dlg.Result);
            PopulateList();
        }
    }

    // Sprint 51 §9a — configure which profile to apply when power plan changes.
    private void OnConfigureProfileOnSwitch(object? sender, EventArgs e)
    {
        var profiles = TweakEngine.Profiles.Select(p => p.Name).ToArray();
        if (profiles.Length == 0)
        {
            MessageBox.Show("No profiles available.", "Profile on Plan Switch", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        using var dlg = new Form
        {
            Text = "Apply Profile on Plan Switch",
            Size = new Size(360, 180),
            FormBorderStyle = FormBorderStyle.FixedDialog,
            MaximizeBox = false,
            MinimizeBox = false,
            StartPosition = FormStartPosition.CenterParent,
        };
        var combo = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Bounds = new Rectangle(16, 50, 220, 28) };
        combo.Items.AddRange(profiles.Cast<object>().ToArray());
        var cfg = AppConfig.Load();
        int idx = Array.IndexOf(profiles, cfg.ProfileOnPlanSwitch ?? "");
        combo.SelectedIndex = Math.Max(0, idx);
        var lblHint = new Label
        {
            Text = "Switch to this profile when power plan changes:",
            Bounds = new Rectangle(16, 20, 300, 22),
            AutoSize = false,
        };
        var btnOk = new Button
        {
            Text = "Save",
            DialogResult = DialogResult.OK,
            Bounds = new Rectangle(260, 50, 72, 28),
        };
        dlg.Controls.AddRange([lblHint, combo, btnOk]);
        dlg.AcceptButton = btnOk;
        if (dlg.ShowDialog(this) == DialogResult.OK && combo.SelectedItem is string chosen)
        {
            cfg.ProfileOnPlanSwitch = chosen;
            cfg.Save();
            _planSwitchLog.Insert(0, $"{DateTime.Now:HH:mm:ss} — 'Profile on switch' set to '{chosen}'");
            SetStatus($"Profile-on-switch set to '{chosen}'.");
        }
    }

    // Sprint 51 §9b — show schedule history log.
    private void OnShowPlanHistory(object? sender, EventArgs e)
    {
        if (_planSwitchLog.Count == 0)
        {
            MessageBox.Show("No plan-switch history recorded this session.", "Schedule History", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        MessageBox.Show(string.Join("\n", _planSwitchLog.Take(20)), "Schedule History (last 20)", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void OnEdit(object? sender, EventArgs e)
    {
        if (_list.SelectedItems.Count == 0)
            return;
        var s = (PlanSchedule)_list.SelectedItems[0].Tag!;
        using var dlg = new ScheduleEditDialog(_plans, s);
        if (dlg.ShowDialog(this) == DialogResult.OK && dlg.Result != null)
        {
            int idx = _schedules.IndexOf(s);
            if (idx >= 0)
                _schedules[idx] = dlg.Result;
            PopulateList();
        }
    }

    private void OnDelete(object? sender, EventArgs e)
    {
        if (_list.SelectedItems.Count == 0)
            return;
        var s = (PlanSchedule)_list.SelectedItems[0].Tag!;
        if (MessageBox.Show($"Delete schedule '{s.Name}'?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
            _schedules.Remove(s);
            PopulateList();
        }
    }

    // ── Nested edit dialog ────────────────────────────────────────────────────
    private sealed class ScheduleEditDialog : Form
    {
        private readonly TextBox _name = new() { Width = 200 };
        private readonly DateTimePicker _start = new()
        {
            Format = DateTimePickerFormat.Time,
            ShowUpDown = true,
            Width = 90,
        };
        private readonly DateTimePicker _end = new()
        {
            Format = DateTimePickerFormat.Time,
            ShowUpDown = true,
            Width = 90,
        };
        private readonly ComboBox _planCombo = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 220 };
        private readonly Button _ok = new()
        {
            Text = "OK",
            DialogResult = DialogResult.OK,
            Width = 80,
        };
        private readonly Button _cancel = new()
        {
            Text = "Cancel",
            DialogResult = DialogResult.Cancel,
            Width = 80,
        };

        internal PlanSchedule? Result { get; private set; }

        internal ScheduleEditDialog(IReadOnlyList<PowerPlanEntry> plans, PlanSchedule? existing = null)
        {
            Text = existing is null ? "Add Schedule" : "Edit Schedule";
            Icon = AppIcons.AppIcon;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            AutoSize = true;
            Padding = new Padding(12);

            foreach (var p in plans)
                _planCombo.Items.Add(p);
            _planCombo.DisplayMember = "Name";

            if (existing != null)
            {
                _name.Text = existing.Name;
                _start.Value = DateTime.Today.Add(existing.StartTime.ToTimeSpan());
                _end.Value = DateTime.Today.Add(existing.EndTime.ToTimeSpan());
                var match = plans.FirstOrDefault(p => p.Guid == existing.PlanGuid);
                if (match != null)
                    _planCombo.SelectedItem = match;
            }
            else
            {
                if (_planCombo.Items.Count > 0)
                    _planCombo.SelectedIndex = 0;
            }

            var table = new TableLayoutPanel
            {
                ColumnCount = 2,
                AutoSize = true,
                Dock = DockStyle.Fill,
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            void AddRow(string label, Control ctrl)
            {
                table.Controls.Add(
                    new Label
                    {
                        Text = label,
                        AutoSize = true,
                        Anchor = AnchorStyles.Left | AnchorStyles.Right,
                        Margin = new Padding(0, 6, 8, 0),
                    }
                );
                table.Controls.Add(ctrl);
            }
            AddRow("Name:", _name);
            AddRow("Start Time:", _start);
            AddRow("End Time:", _end);
            AddRow("Power Plan:", _planCombo);

            var btnRow = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.RightToLeft,
                AutoSize = true,
                Dock = DockStyle.Bottom,
            };
            btnRow.Controls.AddRange([_cancel, _ok]);

            var panel = new Panel
            {
                AutoSize = true,
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 0, 0, 8),
            };
            panel.Controls.AddRange([table, btnRow]);
            Controls.Add(panel);

            AcceptButton = _ok;
            CancelButton = _cancel;
            AppTheme.Apply3D(this);
            _ok.Click += (_, _) =>
            {
                if (string.IsNullOrWhiteSpace(_name.Text))
                {
                    MessageBox.Show("Please enter a schedule name.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (_planCombo.SelectedItem is not PowerPlanEntry plan)
                {
                    MessageBox.Show("Please select a power plan.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                Result = new PlanSchedule
                {
                    Name = _name.Text.Trim(),
                    StartTime = TimeOnly.FromDateTime(_start.Value),
                    EndTime = TimeOnly.FromDateTime(_end.Value),
                    PlanGuid = plan.Guid,
                    PlanName = plan.Name,
                    Enabled = existing?.Enabled ?? true,
                };
                DialogResult = DialogResult.OK;
            };
        }
    }
}
