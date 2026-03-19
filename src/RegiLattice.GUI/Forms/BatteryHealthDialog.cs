// RegiLattice.GUI — Forms/BatteryHealthDialog.cs
// Sprint 41: Battery health monitor for laptops (Phase 2 item 18).

#nullable enable

using System;
using System.Drawing;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;

namespace RegiLattice.GUI.Forms;

/// <summary>
/// Shows laptop battery health: design capacity vs full charge capacity (wear level),
/// current charge, voltage, charge rate, and recent cycle count via WMI.
/// Displays a per-battery visual health bar.
/// </summary>
[SupportedOSPlatform("windows")]
internal sealed class BatteryHealthDialog : BaseDialog
{
    // ── Controls ─────────────────────────────────────────────────────────────
    private readonly Panel _batteryPanel = new()
    {
        Dock = DockStyle.Fill,
        AutoScroll = true,
        Padding = new Padding(12, 8, 12, 8),
    };
    private readonly Button _btnRefresh = new()
    {
        Text = "\u21BB  Refresh",
        Width = 100,
        Height = 30,
    };
    private readonly Button _btnOpenReport = new()
    {
        Text = "\uD83D\uDCCB  Full Report",
        Width = 120,
        Height = 30,
    };
    private readonly Button _btnClose = new()
    {
        Text = "Close",
        Width = 80,
        Height = 30,
    };
    private readonly Label _statusLabel = new()
    {
        Dock = DockStyle.Bottom,
        Height = 22,
        TextAlign = ContentAlignment.MiddleLeft,
        Padding = new Padding(6, 0, 0, 0),
    };

    internal BatteryHealthDialog()
        : base("Battery Health", new Size(500, 400), resizable: true)
    {
        BuildLayout();
        _ = RefreshAsync();
    }

    private void BuildLayout()
    {
        var btnPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            Height = 44,
            FlowDirection = FlowDirection.RightToLeft,
            WrapContents = false,
            Padding = new Padding(8, 6, 8, 6),
        };
        btnPanel.Controls.AddRange([_btnClose, _btnOpenReport, _btnRefresh]);

        Controls.Add(_batteryPanel);
        Controls.Add(_statusLabel);
        Controls.Add(btnPanel);

        _btnRefresh.Click    += async (_, _) => await RefreshAsync();
        _btnOpenReport.Click += OnOpenReport;
        _btnClose.Click      += (_, _) => Close();

        AppTheme.Apply(this);
    }

    private async Task RefreshAsync()
    {
        _btnRefresh.Enabled = false;
        _statusLabel.Text = "Reading battery data…";
        _batteryPanel.Controls.Clear();

        try
        {
            var rows = await Task.Run(ReadBatteries);
            if (rows.Count == 0)
            {
                var noLaptopLabel = new Label
                {
                    Text = "No battery detected.\nThis device is either a desktop or no battery data is available.",
                    AutoSize = false,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    ForeColor = AppTheme.FgDim,
                };
                _batteryPanel.Controls.Add(noLaptopLabel);
                _statusLabel.Text = "No battery detected.";
            }
            else
            {
                _batteryPanel.SuspendLayout();
                foreach (var row in rows)
                    _batteryPanel.Controls.Add(row);
                _batteryPanel.ResumeLayout(true);
                _statusLabel.Text = $"{rows.Count} battery slot(s) detected.";
            }
        }
        catch (Exception ex)
        {
            _statusLabel.Text = $"Error: {ex.Message}";
        }

        _btnRefresh.Enabled = true;
    }

    private System.Collections.Generic.List<Panel> ReadBatteries()
    {
        var panels = new System.Collections.Generic.List<Panel>();
        try
        {
            using var searcher = new ManagementObjectSearcher("SELECT * FROM BatteryStaticData");
            foreach (ManagementObject obj in searcher.Get())
            {
                string name       = obj["Name"]?.ToString() ?? "Battery";
                ulong designCap   = obj["DesignedCapacity"] is ulong dc ? dc : 0;
                ulong fullCap     = obj["FullChargedCapacity"] is ulong fc ? fc : 0;
                int health        = designCap > 0 ? (int)(fullCap * 100 / designCap) : 0;
                string cycleStr   = obj["CycleCount"]?.ToString() ?? "—";

                // Dynamic data
                ulong currentCap = 0;
                string chargeRate = "—";
                string voltage    = "—";
                try
                {
                    using var dyn = new ManagementObjectSearcher("SELECT * FROM BatteryStatus WHERE Tag='" + obj["Tag"] + "'");
                    foreach (ManagementObject d in dyn.Get())
                    {
                        currentCap = d["RemainingCapacity"] is ulong rc ? rc : 0;
                        voltage    = d["Voltage"] is ulong v ? $"{v / 1000.0:F2} V" : "—";
                        chargeRate = d["ChargeRate"] is ulong cr ? $"+{cr} mW"
                                   : d["DischargeRate"] is ulong dr ? $"-{dr} mW" : "—";
                    }
                }
                catch (Exception) { /* ignore dynamic fallback */ }

                int chargePct = fullCap > 0 ? (int)(currentCap * 100 / fullCap) : 0;

                // Build panel on UI thread
                if (InvokeRequired)
                    Invoke(() => panels.Add(BuildBatteryPanel(name, designCap, fullCap, health, chargePct, cycleStr, chargeRate, voltage)));
                else
                    panels.Add(BuildBatteryPanel(name, designCap, fullCap, health, chargePct, cycleStr, chargeRate, voltage));
            }
        }
        catch (Exception) { /* WMI not available — return empty list */ }
        return panels;
    }

    private Panel BuildBatteryPanel(string name, ulong designCap, ulong fullCap, int health,
        int chargePct, string cycles, string chargeRate, string voltage)
    {
        Color healthColor = health >= 80 ? Color.FromArgb(166, 227, 161)
                          : health >= 60 ? Color.FromArgb(249, 226, 175)
                          :                Color.FromArgb(243, 139, 168);

        var panel = new Panel
        {
            Width = _batteryPanel.ClientSize.Width - 24,
            Height = 130,
            Margin = new Padding(0, 0, 0, 12),
            BackColor = AppTheme.Surface,
        };

        var lblName = new Label
        {
            Text = $"\uD83D\uDD0B {name}",
            AutoSize = true, Location = new Point(8, 8),
            Font = AppTheme.Bold, ForeColor = AppTheme.Fg, BackColor = Color.Transparent,
        };
        var lblHealth = new Label
        {
            Text = $"Health: {health}%  |  Design: {designCap} mWh  |  Full charge: {fullCap} mWh",
            AutoSize = true, Location = new Point(8, 30),
            Font = AppTheme.Regular, ForeColor = healthColor, BackColor = Color.Transparent,
        };
        var lblStats = new Label
        {
            Text = $"Current: {chargePct}%  |  Cycles: {cycles}  |  Voltage: {voltage}  |  Rate: {chargeRate}",
            AutoSize = true, Location = new Point(8, 50),
            Font = AppTheme.Regular, ForeColor = AppTheme.FgDim, BackColor = Color.Transparent,
        };

        // Health bar
        var healthBarBg = new Panel { Location = new Point(8, 72), Width = panel.Width - 30, Height = 12, BackColor = AppTheme.Overlay };
        healthBarBg.Paint += (_, e) =>
        {
            int fillW = health > 0 ? Math.Max(2, (int)(healthBarBg.Width * health / 100.0)) : 0;
            using var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                new Rectangle(0, 0, Math.Max(1, fillW), healthBarBg.Height),
                Color.FromArgb(180, healthColor), healthColor,
                System.Drawing.Drawing2D.LinearGradientMode.Horizontal);
            e.Graphics.FillRectangle(brush, 0, 0, fillW, healthBarBg.Height);
        };
        var lblHealthTitle = new Label
        {
            Text = "Battery Health", AutoSize = true, Location = new Point(8, 90),
            Font = AppTheme.Regular, ForeColor = AppTheme.FgDim, BackColor = Color.Transparent,
        };
        // Charge bar
        var chargeBarBg = new Panel { Location = new Point(8, 108), Width = panel.Width - 30, Height = 12, BackColor = AppTheme.Overlay };
        chargeBarBg.Paint += (_, e) =>
        {
            int fillW = chargePct > 0 ? Math.Max(2, (int)(chargeBarBg.Width * chargePct / 100.0)) : 0;
            Color chgColor = Color.FromArgb(137, 180, 250); // blue accent
            using var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                new Rectangle(0, 0, Math.Max(1, fillW), chargeBarBg.Height),
                Color.FromArgb(180, chgColor), chgColor,
                System.Drawing.Drawing2D.LinearGradientMode.Horizontal);
            e.Graphics.FillRectangle(brush, 0, 0, fillW, chargeBarBg.Height);
        };

        panel.Controls.AddRange([lblName, lblHealth, lblStats, healthBarBg, lblHealthTitle, chargeBarBg]);
        return panel;
    }

    private void OnOpenReport(object? sender, EventArgs e)
    {
        try
        {
            string reportPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "battery-report.html");
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c powercfg /batteryreport /output \"{reportPath}\" && start \"\" \"{reportPath}\"",
                UseShellExecute = false,
                CreateNoWindow = true,
            });
            _statusLabel.Text = "Battery report generated and opened.";
        }
        catch (Exception ex)
        {
            _statusLabel.Text = $"Could not generate report: {ex.Message}";
        }
    }
}
