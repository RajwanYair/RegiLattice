// RegiLattice.GUI — Forms/BrightnessSchedulerDialog.cs
// Schedule display brightness changes at specified times using powercfg.
#nullable enable

using RegiLattice.Core;

namespace RegiLattice.GUI.Forms;

/// <summary>
/// Screen Brightness Scheduler.
/// Lets the user define day-time and night-time brightness levels
/// and the times at which to switch. A background timer evaluates
/// the schedule every minute and applies changes via powercfg.
/// Settings are persisted to AppConfig.
/// </summary>
internal sealed class BrightnessSchedulerDialog : BaseDialog
{
    // ── Controls ──────────────────────────────────────────────────────────────
    private readonly CheckBox _chkEnabled = new() { Text = "Enable brightness scheduler", AutoSize = true };
    private readonly Label _lblDay = new() { Text = "Day brightness (%)", AutoSize = true };
    private readonly TrackBar _trkDay = new()
    {
        Minimum = 0,
        Maximum = 100,
        Value = 80,
        TickFrequency = 10,
        Width = 340,
    };
    private readonly Label _lblDayVal = new() { Width = 35, TextAlign = ContentAlignment.MiddleLeft };
    private readonly Label _lblDayTime = new() { Text = "Start time (HH:mm)", AutoSize = true };
    private readonly DateTimePicker _dtpDayTime = new()
    {
        Format = DateTimePickerFormat.Time,
        ShowUpDown = true,
        Width = 100,
    };

    private readonly Label _lblNight = new() { Text = "Night brightness (%)", AutoSize = true };
    private readonly TrackBar _trkNight = new()
    {
        Minimum = 0,
        Maximum = 100,
        Value = 40,
        TickFrequency = 10,
        Width = 340,
    };
    private readonly Label _lblNightVal = new() { Width = 35, TextAlign = ContentAlignment.MiddleLeft };
    private readonly Label _lblNightTime = new() { Text = "Start time (HH:mm)", AutoSize = true };
    private readonly DateTimePicker _dtpNightTime = new()
    {
        Format = DateTimePickerFormat.Time,
        ShowUpDown = true,
        Width = 100,
    };

    private readonly Button _btnApplyNow = new()
    {
        Text = "Apply Now",
        Width = 100,
        Height = 28,
    };
    private readonly Button _btnSave = new()
    {
        Text = "Save Settings",
        Width = 110,
        Height = 28,
    };
    private readonly Button _btnClose = new()
    {
        Text = "Close",
        Width = 75,
        Height = 28,
        DialogResult = DialogResult.Cancel,
    };
    private readonly Label _lblStatus = new()
    {
        Dock = DockStyle.Bottom,
        Height = 26,
        TextAlign = ContentAlignment.MiddleLeft,
        Padding = new Padding(6, 0, 0, 0),
    };
    private readonly System.Windows.Forms.Timer _timer = new() { Interval = 60_000 };
    private readonly Label _lblNote = new()
    {
        Text = "ℹ Brightness control requires hardware support (laptop/external monitor with DDC).",
        AutoSize = true,
        ForeColor = Color.FromArgb(0, 100, 160),
    };

    public BrightnessSchedulerDialog()
        : base("Screen Brightness Scheduler", new Size(540, 440), resizable: false)
    {
        EnableStandaloneMode();
        BuildLayout();
        LoadConfig();

        _trkDay.ValueChanged += (_, _) => _lblDayVal.Text = $"{_trkDay.Value}%";
        _trkNight.ValueChanged += (_, _) => _lblNightVal.Text = $"{_trkNight.Value}%";
        _trkDay.Value = _trkDay.Value; // trigger initial label update
        _trkNight.Value = _trkNight.Value;

        _btnApplyNow.Click += (_, _) => ApplyCurrentSchedule();
        _btnSave.Click += (_, _) => SaveConfig();
        _btnClose.Click += (_, _) => Close();

        _timer.Tick += (_, _) => ApplyCurrentSchedule();
        _timer.Enabled = _chkEnabled.Checked;
        _chkEnabled.CheckedChanged += (_, _) => _timer.Enabled = _chkEnabled.Checked;

        FormClosing += (_, _) => _timer.Stop();
    }

    private void BuildLayout()
    {
        var pnl = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 3,
            RowCount = 9,
            Padding = new Padding(14, 10, 14, 4),
        };
        pnl.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160));
        pnl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        pnl.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 42));

        int row = 0;
        pnl.Controls.Add(_chkEnabled, 0, row);
        pnl.SetColumnSpan(_chkEnabled, 3);
        row++;
        pnl.Controls.Add(new Label { Height = 8, Dock = DockStyle.Fill }, 0, row);
        pnl.SetColumnSpan(pnl.Controls[^1], 3);
        row++;

        pnl.Controls.Add(_lblDay, 0, row);
        pnl.Controls.Add(_trkDay, 1, row);
        pnl.Controls.Add(_lblDayVal, 2, row);
        row++;

        pnl.Controls.Add(_lblDayTime, 0, row);
        pnl.Controls.Add(_dtpDayTime, 1, row);
        row++;

        pnl.Controls.Add(new Label { Height = 8, Dock = DockStyle.Fill }, 0, row);
        pnl.SetColumnSpan(pnl.Controls[^1], 3);
        row++;

        pnl.Controls.Add(_lblNight, 0, row);
        pnl.Controls.Add(_trkNight, 1, row);
        pnl.Controls.Add(_lblNightVal, 2, row);
        row++;

        pnl.Controls.Add(_lblNightTime, 0, row);
        pnl.Controls.Add(_dtpNightTime, 1, row);
        row++;

        pnl.Controls.Add(_lblNote, 0, row);
        pnl.SetColumnSpan(_lblNote, 3);
        row++;

        var btnPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            FlowDirection = FlowDirection.RightToLeft,
            Height = 38,
            Padding = new Padding(6, 5, 6, 0),
        };
        btnPanel.Controls.AddRange(new Control[] { _btnClose, _btnSave, _btnApplyNow });

        Controls.Add(pnl);
        Controls.Add(btnPanel);
        Controls.Add(_lblStatus);
    }

    // ── Config persistence ────────────────────────────────────────────────────

    private void LoadConfig()
    {
        var cfg = AppConfig.Load();
        _chkEnabled.Checked = cfg.BrightnessSchedulerEnabled;
        _trkDay.Value = Math.Clamp(cfg.BrightnessDayPct, 0, 100);
        _trkNight.Value = Math.Clamp(cfg.BrightnessNightPct, 0, 100);
        _dtpDayTime.Value = ParseTime(cfg.BrightnessDayTime);
        _dtpNightTime.Value = ParseTime(cfg.BrightnessNightTime);
        _lblDayVal.Text = $"{_trkDay.Value}%";
        _lblNightVal.Text = $"{_trkNight.Value}%";
    }

    private void SaveConfig()
    {
        var cfg = AppConfig.Load();
        cfg.BrightnessSchedulerEnabled = _chkEnabled.Checked;
        cfg.BrightnessDayPct = _trkDay.Value;
        cfg.BrightnessNightPct = _trkNight.Value;
        cfg.BrightnessDayTime = _dtpDayTime.Value.ToString("HH:mm");
        cfg.BrightnessNightTime = _dtpNightTime.Value.ToString("HH:mm");
        cfg.Save();
        _timer.Enabled = _chkEnabled.Checked;
        _lblStatus.Text = "✓ Settings saved.";
    }

    // ── Brightness application ────────────────────────────────────────────────

    private void ApplyCurrentSchedule()
    {
        if (!_chkEnabled.Checked)
            return;
        var now = DateTime.Now.TimeOfDay;

        TimeSpan dayStart = _dtpDayTime.Value.TimeOfDay;
        TimeSpan nightStart = _dtpNightTime.Value.TimeOfDay;
        int targetPct;

        if (dayStart < nightStart)
            targetPct = now >= dayStart && now < nightStart ? _trkDay.Value : _trkNight.Value;
        else
            targetPct = now >= dayStart || now < nightStart ? _trkDay.Value : _trkNight.Value;

        bool ok = SetBrightness(targetPct);
        _lblStatus.Text = ok
            ? $"Brightness set to {targetPct}% at {DateTime.Now:HH:mm:ss}"
            : $"Brightness control unavailable at {DateTime.Now:HH:mm:ss}";
    }

    private static bool SetBrightness(int pct)
    {
        try
        {
            // Try WMI WmiMonitorBrightnessMethods first (laptop built-in display)
            using var mos = new System.Management.ManagementObjectSearcher(@"\\.\root\wmi", "SELECT * FROM WmiMonitorBrightnessMethods");
            var list = mos.Get();
            bool found = false;
            foreach (System.Management.ManagementObject o in list)
            {
                o.InvokeMethod("WmiSetBrightness", [0u, (byte)pct]);
                found = true;
            }
            if (found)
                return true;
        }
        catch
        { /* WMI channel not available */
        }

        // Fallback: powercfg active scheme brightness
        try
        {
            using var proc = new System.Diagnostics.Process();
            proc.StartInfo = new System.Diagnostics.ProcessStartInfo("powercfg", $"/SETACVALUEINDEX SCHEME_CURRENT SUB_VIDEO VIDEOADAPT {pct}")
            {
                CreateNoWindow = true,
                UseShellExecute = false,
            };
            proc.Start();
            proc.WaitForExit(3000);
            return proc.ExitCode == 0;
        }
        catch
        {
            return false;
        }
    }

    private static DateTime ParseTime(string s)
    {
        if (TimeSpan.TryParse(s, out var ts))
            return DateTime.Today.Add(ts);
        return DateTime.Today.AddHours(7);
    }
}
