// RegiLattice.GUI — Forms/HardwareTemperatureDialog.cs
// Sprint 42: Hardware temperature monitor via WMI (Phase 2 item 20).

#nullable enable

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Management;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegiLattice.GUI.Forms;

/// <summary>
/// Displays CPU thermal zone temperatures via WMI MSAcpi_ThermalZoneTemperature,
/// plus GPU information via Win32_VideoController.
/// Temperatures update every 3 seconds.
/// </summary>
[SupportedOSPlatform("windows")]
internal sealed class HardwareTemperatureDialog : BaseDialog
{
    private readonly Panel _tempPanel = new()
    {
        Dock = DockStyle.Fill,
        AutoScroll = true,
        Padding = new Padding(12, 8, 12, 8),
    };
    private readonly Button _btnRefresh = new() { Text = "\u21BB  Refresh", Width = 100, Height = 30 };
    private readonly Button _btnClose = new() { Text = "Close", Width = 80, Height = 30 };
    private readonly Label _statusLabel = new()
    {
        Dock = DockStyle.Bottom,
        Height = 22,
        TextAlign = ContentAlignment.MiddleLeft,
        Padding = new Padding(6, 0, 0, 0),
    };

    private System.Windows.Forms.Timer? _pollTimer;

    internal HardwareTemperatureDialog()
        : base("Hardware Temperature Monitor", new Size(520, 440), resizable: true)
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
        btnPanel.Controls.AddRange([_btnClose, _btnRefresh]);

        Controls.Add(_tempPanel);
        Controls.Add(_statusLabel);
        Controls.Add(btnPanel);

        _btnRefresh.Click += async (_, _) => await RefreshAsync();
        _btnClose.Click += (_, _) => Close();

        // Auto-refresh every 3 seconds
        _pollTimer = new System.Windows.Forms.Timer { Interval = 3000 };
        _pollTimer.Tick += async (_, _) => await RefreshAsync();

        var autoRefresh = new CheckBox
        {
            Text = "Auto-refresh (3 s)",
            Dock = DockStyle.Bottom,
            Height = 20,
            Padding = new Padding(8, 0, 0, 0),
        };
        autoRefresh.CheckedChanged += (_, _) =>
        {
            if (autoRefresh.Checked) _pollTimer.Start();
            else _pollTimer.Stop();
        };
        Controls.Add(autoRefresh);

        FormClosed += (_, _) => { _pollTimer?.Stop(); _pollTimer?.Dispose(); };
        AppTheme.Apply(this);
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        AppTheme.Apply(this);
    }

    private async Task RefreshAsync()
    {
        _btnRefresh.Enabled = false;
        _statusLabel.Text = "Querying hardware temperatures…";

        var rows = await Task.Run(QueryTemperatures);

        _tempPanel.SuspendLayout();
        _tempPanel.Controls.Clear();

        if (rows.Count == 0)
        {
            var lbl = new Label
            {
                Text = "No temperature data available.\n\nWMI thermal zone data may not be exposed\nby this system's firmware.",
                AutoSize = false,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
            };
            _tempPanel.Controls.Add(lbl);
        }
        else
        {
            foreach (var (name, celsius) in rows)
                _tempPanel.Controls.Add(CreateTempRow(name, celsius));
        }

        _tempPanel.ResumeLayout(true);
        _statusLabel.Text = $"Last updated: {DateTime.Now:HH:mm:ss}  |  {rows.Count} sensor(s) found";
        _btnRefresh.Enabled = true;
        AppTheme.Apply(this);
    }

    private static List<(string Name, double Celsius)> QueryTemperatures()
    {
        var results = new List<(string, double)>();

        // CPU thermal zones via ACPI
        try
        {
            using var mos = new ManagementObjectSearcher(
                "root\\WMI",
                "SELECT * FROM MSAcpi_ThermalZoneTemperature"
            );
            int zoneIndex = 0;
            foreach (ManagementObject obj in mos.Get())
            {
                if (obj["CurrentTemperature"] is uint raw)
                {
                    double celsius = raw / 10.0 - 273.15;
                    string name = obj["InstanceName"]?.ToString() ?? $"Thermal Zone {zoneIndex}";
                    // Shorten long WMI instance names
                    if (name.Length > 50) name = "CPU Zone " + zoneIndex;
                    results.Add((name, celsius));
                    zoneIndex++;
                }
            }
        }
        catch (ManagementException)
        {
            // WMI not supported on this platform — add a placeholder
            results.Add(("CPU Zone (WMI unavailable)", double.NaN));
        }

        // GPU info via Win32_VideoController (no direct temp — show adapter name)
        try
        {
            using var mos = new ManagementObjectSearcher(
                "SELECT Name, AdapterRAM, CurrentRefreshRate FROM Win32_VideoController"
            );
            foreach (ManagementObject obj in mos.Get())
            {
                string gpuName = obj["Name"]?.ToString() ?? "GPU";
                long ramMb = (obj["AdapterRAM"] is uint ram) ? (ram / 1024 / 1024) : 0;
                results.Add(($"GPU: {gpuName} ({ramMb} MB VRAM)", double.NaN));
            }
        }
        catch (ManagementException) { /* ignore */ }

        return results;
    }

    private static Panel CreateTempRow(string name, double celsius)
    {
        var panel = new Panel { Height = 46, Dock = DockStyle.Top, Padding = new Padding(4) };

        var lblName = new Label
        {
            Text = name,
            Location = new Point(4, 4),
            Size = new Size(300, 18),
            Font = new Font(SystemFonts.DefaultFont, FontStyle.Regular),
        };

        if (double.IsNaN(celsius))
        {
            var lblInfo = new Label
            {
                Text = "(info only)",
                Location = new Point(4, 24),
                Size = new Size(100, 16),
                ForeColor = Color.Gray,
                Font = new Font(SystemFonts.DefaultFont, FontStyle.Italic),
            };
            panel.Controls.AddRange([lblName, lblInfo]);
        }
        else
        {
            // Color-coded temperature
            Color barColor = celsius < 60.0 ? Color.FromArgb(30, 180, 60)
                           : celsius < 80.0 ? Color.FromArgb(220, 150, 0)
                           : Color.FromArgb(220, 50, 50);

            var lblTemp = new Label
            {
                Text = $"{celsius:F1} °C",
                Location = new Point(310, 4),
                Size = new Size(90, 18),
                ForeColor = barColor,
                Font = new Font(SystemFonts.DefaultFont, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleRight,
            };

            int barWidth = Math.Max(4, (int)(celsius / 100.0 * 180));
            var bar = new Panel
            {
                BackColor = barColor,
                Location = new Point(4, 26),
                Size = new Size(barWidth, 10),
            };

            panel.Controls.AddRange([lblName, lblTemp, bar]);
        }

        return panel;
    }
}
