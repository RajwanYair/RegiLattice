// RegiLattice.GUI — Forms/NetworkBandwidthDialog.cs
// Sprint 42: Real-time network bandwidth monitor (Phase 4 item 38).

#nullable enable

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegiLattice.GUI.Forms;

/// <summary>
/// Shows real-time send / receive rates for all active network adapters,
/// updated every second. Rates are computed as delta-bytes between polls.
/// </summary>
[SupportedOSPlatform("windows")]
internal sealed class NetworkBandwidthDialog : BaseDialog
{
    private sealed record AdapterSnapshot(string Name, long BytesSent, long BytesReceived, DateTime Stamp);

    private readonly ListView _listView = new()
    {
        Dock = DockStyle.Fill,
        FullRowSelect = true,
        View = View.Details,
        GridLines = true,
    };
    private readonly Label _statusLabel = new()
    {
        Dock = DockStyle.Bottom,
        Height = 22,
        TextAlign = ContentAlignment.MiddleLeft,
        Padding = new Padding(6, 0, 0, 0),
    };
    private readonly Button _btnClose = new()
    {
        Text = "Close",
        Width = 80,
        Height = 30,
    };

    private readonly Dictionary<string, AdapterSnapshot> _prevSnapshots = new();
    private System.Windows.Forms.Timer? _pollTimer;

    internal NetworkBandwidthDialog()
        : base("Network Bandwidth Monitor", new Size(640, 400), resizable: true)
    {
        BuildLayout();
        PollOnce();
        StartPolling();
    }

    private void BuildLayout()
    {
        _listView.Columns.AddRange([
            new ColumnHeader { Text = "Adapter", Width = 220 },
            new ColumnHeader { Text = "↑ Send", Width = 100 },
            new ColumnHeader { Text = "↓ Receive", Width = 100 },
            new ColumnHeader { Text = "Total ↑ (MB)", Width = 100 },
            new ColumnHeader { Text = "Total ↓ (MB)", Width = 100 },
        ]);
        ListViewColumnSorter.AttachTo(_listView);

        var btnPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            Height = 44,
            FlowDirection = FlowDirection.RightToLeft,
            WrapContents = false,
            Padding = new Padding(8, 6, 8, 6),
        };
        btnPanel.Controls.Add(_btnClose);

        Controls.Add(_listView);
        Controls.Add(_statusLabel);
        Controls.Add(btnPanel);

        _btnClose.Click += (_, _) => Close();
        FormClosed += (_, _) =>
        {
            _pollTimer?.Stop();
            _pollTimer?.Dispose();
        };

        AppTheme.Apply(this);
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        AppTheme.Apply(this);
    }

    private void StartPolling()
    {
        _pollTimer = new System.Windows.Forms.Timer { Interval = 1000 };
        _pollTimer.Tick += (_, _) => PollOnce();
        _pollTimer.Start();
    }

    private void PollOnce()
    {
        if (!IsHandleCreated)
            return;

        var adapters = NetworkInterface
            .GetAllNetworkInterfaces()
            .Where(n => n.OperationalStatus == OperationalStatus.Up && n.NetworkInterfaceType != NetworkInterfaceType.Loopback)
            .ToList();

        var now = DateTime.UtcNow;

        _listView.BeginUpdate();
        _listView.Items.Clear();

        foreach (var nic in adapters)
        {
            IPv4InterfaceStatistics stats;
            try
            {
                stats = nic.GetIPv4Statistics();
            }
            catch
            {
                continue;
            }

            long sent = stats.BytesSent;
            long recv = stats.BytesReceived;

            double sendRate = 0;
            double recvRate = 0;

            if (_prevSnapshots.TryGetValue(nic.Id, out var prev))
            {
                double secs = (now - prev.Stamp).TotalSeconds;
                if (secs > 0)
                {
                    sendRate = (sent - prev.BytesSent) / secs;
                    recvRate = (recv - prev.BytesReceived) / secs;
                }
            }

            _prevSnapshots[nic.Id] = new AdapterSnapshot(nic.Name, sent, recv, now);

            var item = new ListViewItem(TruncateName(nic.Name, 30));
            item.SubItems.Add(FormatRate(sendRate));
            item.SubItems.Add(FormatRate(recvRate));
            item.SubItems.Add((sent / 1024.0 / 1024.0).ToString("F1"));
            item.SubItems.Add((recv / 1024.0 / 1024.0).ToString("F1"));

            // Highlight active adapters
            if (sendRate > 1024 || recvRate > 1024)
                item.ForeColor = Color.FromArgb(30, 144, 255);

            _listView.Items.Add(item);
        }

        _listView.EndUpdate();
        _statusLabel.Text = $"Polling: {adapters.Count} adapter(s) active  |  Last: {DateTime.Now:HH:mm:ss}";
        AppTheme.Apply(this);
    }

    private static string FormatRate(double bytesPerSec)
    {
        if (bytesPerSec >= 1_048_576)
            return $"{bytesPerSec / 1_048_576:F1} MB/s";
        if (bytesPerSec >= 1_024)
            return $"{bytesPerSec / 1_024:F1} KB/s";
        return $"{bytesPerSec:F0} B/s";
    }

    private static string TruncateName(string name, int maxLen) => name.Length > maxLen ? name[..maxLen] + "…" : name;
}
