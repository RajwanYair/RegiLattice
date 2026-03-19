// RegiLattice.GUI — Forms/SleepTimerDialog.cs
// Sprint 31: Sleep/hibernate timer + monitor power-off timer.

#nullable enable

using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using RegiLattice.Core;

namespace RegiLattice.GUI.Forms;

/// <summary>
/// Lets the user schedule a one-shot or countdown-based:
///  • Sleep  (suspend to RAM)
///  • Hibernate (suspend to disk)
///  • Shutdown
///  • Monitor off (turns off display)
/// Uses <c>shutdown.exe</c> and the Windows <c>powercfg /timer</c> approach via
/// <c>SetThreadExecutionState</c> for monitor off.
/// </summary>
internal sealed class SleepTimerDialog : BaseDialog
{
    // ── Controls ──────────────────────────────────────────────────────────────
    private readonly RadioButton _rbSleep = new() { Text = "Sleep (Suspend to RAM)", Checked = true, AutoSize = true };
    private readonly RadioButton _rbHibernate = new() { Text = "Hibernate (Suspend to Disk)", AutoSize = true };
    private readonly RadioButton _rbShutdown = new() { Text = "Shutdown", AutoSize = true };
    private readonly RadioButton _rbMonitorOff = new() { Text = "Monitor Off Only", AutoSize = true };

    private readonly RadioButton _rbCountdown = new() { Text = "Countdown:", Checked = true, AutoSize = true };
    private readonly NumericUpDown _nudMinutes = new()
    {
        Minimum = 1,
        Maximum = 480,
        Value = 30,
        Width = 70,
        Increment = 5,
    };
    private readonly Label _lblMinutes = new() { Text = "minutes", AutoSize = true, Margin = new Padding(4, 3, 0, 0) };

    private readonly RadioButton _rbAtTime = new() { Text = "At specific time:", AutoSize = true };
    private readonly DateTimePicker _timePicker = new()
    {
        Format = DateTimePickerFormat.Time,
        ShowUpDown = true,
        Width = 90,
        Enabled = false,
    };

    private readonly Button _btnStart = new() { Text = "Start Timer", Width = 100 };
    private readonly Button _btnCancel = new() { Text = "Cancel Timer", Width = 100, Enabled = false };
    private readonly Button _btnClose = new() { Text = "Close", Width = 80 };

    private readonly Label _statusLabel = new()
    {
        Dock = DockStyle.Bottom,
        Height = 24,
        TextAlign = ContentAlignment.MiddleLeft,
        Padding = new Padding(6, 0, 0, 0),
        Text = "No timer running.",
    };
    private readonly ProgressBar _progress = new()
    {
        Dock = DockStyle.Bottom,
        Height = 10,
        Style = ProgressBarStyle.Continuous,
        Minimum = 0,
        Maximum = 100,
        Visible = false,
    };

    private System.Windows.Forms.Timer? _uiTimer;
    private DateTime _startTime;
    private DateTime _targetTime;
    private bool _timerRunning;
    private int _shutdownPid = -1; // PID of shutdown.exe for cancellation

    // ── Construction ──────────────────────────────────────────────────────────
    internal SleepTimerDialog()
        : base("Sleep / Hibernate Timer", new Size(480, 380), resizable: false)
    {
        BuildLayout();

        _rbCountdown.CheckedChanged += (_, _) => _nudMinutes.Enabled = _rbCountdown.Checked;
        _rbAtTime.CheckedChanged += (_, _) => _timePicker.Enabled = _rbAtTime.Checked;
        _btnStart.Click += OnStartTimer;
        _btnCancel.Click += OnCancelTimer;
        _btnClose.Click += (_, _) => Close();
        FormClosing += (_, _) => CancelAnyRunningTimer();
    }

    // ── Layout ────────────────────────────────────────────────────────────────
    private void BuildLayout()
    {
        // Action group
        var grpAction = new GroupBox { Text = "Action", Dock = DockStyle.Top, Height = 110, Padding = new Padding(8) };
        var actionFlow = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown };
        actionFlow.Controls.AddRange([_rbSleep, _rbHibernate, _rbShutdown, _rbMonitorOff]);
        grpAction.Controls.Add(actionFlow);

        // Timing group
        var grpTiming = new GroupBox { Text = "When", Dock = DockStyle.Top, Height = 100, Padding = new Padding(8) };
        var timingFlow = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown };

        var countdownRow = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, AutoSize = true };
        countdownRow.Controls.AddRange([_rbCountdown, _nudMinutes, _lblMinutes]);

        var atTimeRow = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, AutoSize = true };
        atTimeRow.Controls.AddRange([_rbAtTime, _timePicker]);

        timingFlow.Controls.AddRange([countdownRow, atTimeRow]);
        grpTiming.Controls.Add(timingFlow);

        // Buttons
        var btnRow = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 42,
            Padding = new Padding(6, 6, 6, 4),
            FlowDirection = FlowDirection.LeftToRight,
        };
        btnRow.Controls.AddRange([_btnStart, _btnCancel, _btnClose]);

        Controls.AddRange([grpAction, grpTiming, btnRow, _progress, _statusLabel]);
    }

    // ── Timer Logic ───────────────────────────────────────────────────────────
    private void OnStartTimer(object? sender, EventArgs e)
    {
        if (_timerRunning)
            return;

        DateTime now = DateTime.Now;
        if (_rbCountdown.Checked)
        {
            _startTime = now;
            _targetTime = now.AddMinutes((double)_nudMinutes.Value);
        }
        else
        {
            var todayTarget = DateTime.Today.Add(_timePicker.Value.TimeOfDay);
            _targetTime = todayTarget <= now ? todayTarget.AddDays(1) : todayTarget;
            _startTime = now;
        }

        double totalSecs = (_targetTime - _startTime).TotalSeconds;
        int shutdownSecs = Math.Max(1, (int)(_targetTime - now).TotalSeconds);

        if (_rbMonitorOff.Checked)
        {
            ScheduleMonitorOff(shutdownSecs);
        }
        else
        {
            string shutdownArgs = BuildShutdownArgs(shutdownSecs);
            try
            {
                var psi = new ProcessStartInfo("shutdown.exe", shutdownArgs) { CreateNoWindow = true, UseShellExecute = false };
                var proc = Process.Start(psi);
                _shutdownPid = proc?.Id ?? -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to schedule: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        _timerRunning = true;
        _progress.Visible = true;
        _btnStart.Enabled = false;
        _btnCancel.Enabled = true;

        _uiTimer = new System.Windows.Forms.Timer { Interval = 1000 };
        _uiTimer.Tick += (_, _) =>
        {
            if (!_timerRunning) return;
            double remaining = (_targetTime - DateTime.Now).TotalSeconds;
            if (remaining <= 0)
            {
                StopUiTimer();
                _statusLabel.Text = "Timer elapsed — action triggered.";
                return;
            }
            TimeSpan rem = TimeSpan.FromSeconds(remaining);
            _statusLabel.Text = $"Time remaining: {rem:hh\\:mm\\:ss} → {GetActionLabel()}";
            double elapsed = (DateTime.Now - _startTime).TotalSeconds;
            double total = (double)(_targetTime - _startTime).TotalSeconds;
            _progress.Value = (int)Math.Min(100, elapsed / total * 100);
        };
        _uiTimer.Start();
        _statusLabel.Text = $"Timer started → {GetActionLabel()} at {_targetTime:HH:mm:ss}";
    }

    private void OnCancelTimer(object? sender, EventArgs e)
    {
        CancelAnyRunningTimer();
        _statusLabel.Text = "Timer cancelled.";
    }

    private void CancelAnyRunningTimer()
    {
        if (!_timerRunning)
            return;
        StopUiTimer();
        if (!_rbMonitorOff.Checked)
        {
            try { Process.Start(new ProcessStartInfo("shutdown.exe", "/a") { CreateNoWindow = true, UseShellExecute = false }); }
            catch { /* ignore */ }
        }
    }

    private void StopUiTimer()
    {
        _timerRunning = false;
        _uiTimer?.Stop();
        _uiTimer?.Dispose();
        _uiTimer = null;
        _progress.Value = 0;
        _progress.Visible = false;
        _btnStart.Enabled = true;
        _btnCancel.Enabled = false;
    }

    private string BuildShutdownArgs(int seconds)
    {
        if (_rbSleep.Checked) return $"/h /f /t {seconds}"; // hybrid sleep
        if (_rbHibernate.Checked) return $"/h /f";           // immediate hibernate
        if (_rbShutdown.Checked) return $"/s /f /t {seconds}";
        return $"/s /f /t {seconds}";
    }

    private void ScheduleMonitorOff(int delaySeconds)
    {
        // Use a one-shot WinForms timer to send WM_SYSCOMMAND SC_MONITORPOWER
        var t = new System.Windows.Forms.Timer { Interval = delaySeconds * 1000 };
        t.Tick += (_, _) =>
        {
            t.Stop(); t.Dispose();
            SendMessage(Handle, 0x0112 /*WM_SYSCOMMAND*/, new IntPtr(0xF170 /*SC_MONITORPOWER*/), new IntPtr(2));
        };
        t.Start();
    }

    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

    private string GetActionLabel()
    {
        if (_rbSleep.Checked) return "Sleep";
        if (_rbHibernate.Checked) return "Hibernate";
        if (_rbShutdown.Checked) return "Shutdown";
        return "Monitor Off";
    }
}
