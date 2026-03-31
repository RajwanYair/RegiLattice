// RegiLattice.GUI — Forms/ProfileSchedulerDialog.cs
// Sprint 51: Auto-switch profiles by time-of-day or system event.

#nullable enable

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using RegiLattice.Core;
using RegiLattice.Core.Services;

namespace RegiLattice.GUI.Forms;

/// <summary>
/// Dialog for managing automatic profile-switch schedules.
/// Schedules are persisted via <see cref="AppConfig.ProfileSchedules"/>.
/// </summary>
internal sealed class ProfileSchedulerDialog : BaseDialog
{
    // ── Controls ─────────────────────────────────────────────────────────────
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
        Text = "Add Schedule\u2026",
        Width = 120,
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
        Text = "Enable / Disable",
        Width = 110,
        Height = 28,
        Enabled = false,
    };
    private readonly Button _btnClose = new()
    {
        Text = "Close",
        Width = 80,
        Height = 28,
    };
    private readonly Label _lblStatus = new()
    {
        Dock = DockStyle.Bottom,
        AutoSize = false,
        Height = 22,
        TextAlign = ContentAlignment.MiddleLeft,
        Padding = new Padding(2, 0, 0, 0),
    };

    // ── State ─────────────────────────────────────────────────────────────────
    private List<ProfileScheduleEntry> _schedules = [];

    // ── Constructor ──────────────────────────────────────────────────────────
    public ProfileSchedulerDialog()
        : base("Profile Scheduler", new Size(640, 440), resizable: true)
    {
        _btnAdd.Click += OnAdd;
        _btnRemove.Click += OnRemove;
        _btnToggle.Click += OnToggle;
        _btnClose.Click += (_, _) => Close();
        _list.SelectedIndexChanged += (_, _) => UpdateButtons();

        BuildLayout();
        LoadSchedules();
    }

    // ── Layout ────────────────────────────────────────────────────────────────
    private void BuildLayout()
    {
        _list.Columns.Add("Profile", 130);
        _list.Columns.Add("Trigger", 100);
        _list.Columns.Add("Time", 80);
        _list.Columns.Add("Enabled", 70);
        _list.Columns.Add("Created", 150);

        var btnPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            AutoSize = true,
            Padding = new Padding(4, 4, 4, 4),
            FlowDirection = FlowDirection.LeftToRight,
        };
        btnPanel.Controls.AddRange([_btnAdd, _btnRemove, _btnToggle, _btnClose]);

        Controls.Add(_list);
        Controls.Add(_lblStatus);
        Controls.Add(btnPanel);
    }

    // ── Data ──────────────────────────────────────────────────────────────────
    private void LoadSchedules()
    {
        var cfg = AppConfig.Load();
        _schedules = [.. cfg.ProfileSchedules];
        PopulateList();
    }

    private void SaveSchedules()
    {
        var cfg = AppConfig.Load();
        cfg.ProfileSchedules = _schedules;
        cfg.Save();
    }

    private void PopulateList()
    {
        _list.BeginUpdate();
        _list.Items.Clear();
        foreach (var s in _schedules)
        {
            var item = new ListViewItem(s.Profile);
            item.SubItems.Add(s.Trigger);
            item.SubItems.Add(s.Time);
            item.SubItems.Add(s.Enabled ? "Yes" : "No");
            item.SubItems.Add(s.CreatedAt.ToLocalTime().ToString("yyyy-MM-dd HH:mm"));
            item.Tag = s;
            _list.Items.Add(item);
        }
        _list.EndUpdate();
        _lblStatus.Text = $"{_schedules.Count} schedule(s) configured.";
        UpdateButtons();
    }

    private void UpdateButtons()
    {
        bool sel = _list.SelectedItems.Count > 0;
        _btnRemove.Enabled = sel;
        _btnToggle.Enabled = sel;
    }

    // ── Event Handlers ────────────────────────────────────────────────────────
    private void OnAdd(object? sender, EventArgs e)
    {
        var profiles = TweakEngine.Profiles.Select(p => p.Name).ToArray();
        using var dlg = new AddProfileScheduleDialog(profiles);
        if (dlg.ShowDialog(this) != DialogResult.OK || dlg.Result is null)
            return;

        _schedules.Add(dlg.Result);
        SaveSchedules();
        PopulateList();
        SetStatus("Schedule added.");
    }

    private void OnRemove(object? sender, EventArgs e)
    {
        if (_list.SelectedItems.Count == 0)
            return;

        var entry = (ProfileScheduleEntry)_list.SelectedItems[0].Tag!;
        var answer = MessageBox.Show(
            $"Remove schedule for profile '{entry.Profile}'?",
            "Confirm Remove",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question
        );

        if (answer != DialogResult.Yes)
            return;

        _schedules.Remove(entry);
        SaveSchedules();
        PopulateList();
        SetStatus("Schedule removed.");
    }

    private void OnToggle(object? sender, EventArgs e)
    {
        if (_list.SelectedItems.Count == 0)
            return;

        var entry = (ProfileScheduleEntry)_list.SelectedItems[0].Tag!;
        int idx = _schedules.IndexOf(entry);
        if (idx < 0)
            return;

        _schedules[idx] = entry with { Enabled = !entry.Enabled };
        SaveSchedules();
        PopulateList();
        SetStatus($"Schedule '{entry.Profile}' {(entry.Enabled ? "disabled" : "enabled")}.");
    }

    private void SetStatus(string text)
    {
        _lblStatus.Text = text;
    }
}

// ── Wizard dialog for adding a profile schedule ───────────────────────────────
internal sealed class AddProfileScheduleDialog : Form
{
    // Result property — not designer-serializable; set at runtime when the user confirms.
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ProfileScheduleEntry? Result { get; private set; }

    private readonly ComboBox _cmbProfile = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 200 };
    private readonly ComboBox _cmbTrigger = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 200 };
    private readonly DateTimePicker _timePicker = new()
    {
        Format = DateTimePickerFormat.Time,
        ShowUpDown = true,
        Width = 120,
    };
    private readonly Label _lblTime = new() { Text = "Time (HH:mm):", AutoSize = true };
    private readonly Button _btnOk = new()
    {
        Text = "Add",
        Width = 80,
        DialogResult = DialogResult.None,
    };
    private readonly Button _btnCancel = new()
    {
        Text = "Cancel",
        DialogResult = DialogResult.Cancel,
        Width = 80,
    };

    public AddProfileScheduleDialog(string[] profiles)
    {
        Text = "Add Profile Schedule";
        Icon = AppIcons.AppIcon;
        Size = new Size(380, 260);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;

        _cmbProfile.Items.AddRange(profiles.Cast<object>().ToArray());
        if (_cmbProfile.Items.Count > 0)
            _cmbProfile.SelectedIndex = 0;

        _cmbTrigger.Items.AddRange(["daily", "on_boot", "on_login"]);
        _cmbTrigger.SelectedIndex = 0;
        _cmbTrigger.SelectedIndexChanged += (_, _) =>
        {
            bool isDaily = _cmbTrigger.SelectedItem?.ToString() == "daily";
            _lblTime.Visible = isDaily;
            _timePicker.Visible = isDaily;
        };

        _btnOk.Click += OnOk;
        CancelButton = _btnCancel;
        AcceptButton = _btnOk;

        BuildLayout();
    }

    private void BuildLayout()
    {
        var table = new TableLayoutPanel
        {
            ColumnCount = 2,
            RowCount = 4,
            AutoSize = true,
            Padding = new Padding(12),
            Dock = DockStyle.Fill,
        };
        table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        table.Controls.Add(
            new Label
            {
                Text = "Profile:",
                AutoSize = true,
                Anchor = AnchorStyles.Left,
            },
            0,
            0
        );
        table.Controls.Add(_cmbProfile, 1, 0);
        table.Controls.Add(
            new Label
            {
                Text = "Trigger:",
                AutoSize = true,
                Anchor = AnchorStyles.Left,
            },
            0,
            1
        );
        table.Controls.Add(_cmbTrigger, 1, 1);
        table.Controls.Add(_lblTime, 0, 2);
        table.Controls.Add(_timePicker, 1, 2);

        var btnRow = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.RightToLeft,
            Dock = DockStyle.Bottom,
            AutoSize = true,
            Padding = new Padding(8),
        };
        btnRow.Controls.AddRange([_btnCancel, _btnOk]);

        Controls.Add(table);
        Controls.Add(btnRow);
        AppTheme.Apply3D(this);
    }

    private void OnOk(object? sender, EventArgs e)
    {
        if (_cmbProfile.SelectedItem is not string profile || string.IsNullOrWhiteSpace(profile))
        {
            MessageBox.Show("Please select a profile.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        string trigger = _cmbTrigger.SelectedItem?.ToString() ?? "daily";
        string time = trigger == "daily" ? _timePicker.Value.ToString("HH:mm") : "";

        Result = new ProfileScheduleEntry
        {
            Profile = profile,
            Trigger = trigger,
            Time = time,
        };
        DialogResult = DialogResult.OK;
    }
}
