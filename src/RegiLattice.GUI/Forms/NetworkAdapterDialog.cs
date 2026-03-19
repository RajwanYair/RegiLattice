// RegiLattice.GUI — Forms/NetworkAdapterDialog.cs
// Network adapter enable / disable / diagnostics panel using WMI + netsh.
#nullable enable

using System.Diagnostics;
using System.Management;
using System.Net.NetworkInformation;
using System.Text;
using RegiLattice.Core;

namespace RegiLattice.GUI.Forms;

/// <summary>
/// Lists all network adapters with status details.
/// Lets the user enable/disable adapters and run a quick connectivity diagnostic.
/// </summary>
internal sealed class NetworkAdapterDialog : BaseDialog
{
    private sealed record AdapterInfo(
        string Name,
        string Description,
        string Status,
        string Type,
        string MacAddress,
        string IpAddress,
        string Gateway,
        string Speed,
        string DeviceId
    );

    private readonly ListView _adapterList = new()
    {
        View = View.Details,
        FullRowSelect = true,
        MultiSelect = false,
        GridLines = true,
        Dock = DockStyle.Fill,
    };

    private readonly RichTextBox _detailBox = new()
    {
        ReadOnly = true,
        Dock = DockStyle.Fill,
        ScrollBars = RichTextBoxScrollBars.Vertical,
        BackColor = SystemColors.Info,
        ForeColor = SystemColors.InfoText,
    };

    private readonly Label _statusLabel = new()
    {
        Dock = DockStyle.Top,
        Height = 26,
        TextAlign = ContentAlignment.MiddleLeft,
        Padding = new Padding(6, 0, 0, 0),
    };
    private readonly Button _btnEnable = new()
    {
        Text = "Enable",
        Width = 80,
        Height = 28,
        Enabled = false,
    };
    private readonly Button _btnDisable = new()
    {
        Text = "Disable",
        Width = 80,
        Height = 28,
        Enabled = false,
    };
    private readonly Button _btnDiagnose = new()
    {
        Text = "Diagnose",
        Width = 90,
        Height = 28,
        Enabled = false,
    };
    private readonly Button _btnRefresh = new()
    {
        Text = "⟳ Refresh",
        Width = 90,
        Height = 28,
    };
    private readonly Button _btnClose = new()
    {
        Text = "Close",
        Width = 75,
        Height = 28,
        DialogResult = DialogResult.Cancel,
    };
    private readonly Panel _btnPanel = new() { Dock = DockStyle.Bottom, Height = 38 };

    private List<AdapterInfo> _adapters = [];

    public NetworkAdapterDialog()
        : base("Network Adapter Manager", new Size(940, 620), resizable: true)
    {
        MinimumSize = new Size(800, 500);
        EnableStandaloneMode();

        _adapterList.Columns.AddRange([
            new ColumnHeader { Text = "Adapter Name", Width = 200 },
            new ColumnHeader { Text = "Status", Width = 80 },
            new ColumnHeader { Text = "Type", Width = 110 },
            new ColumnHeader { Text = "IP Address", Width = 130 },
            new ColumnHeader { Text = "Speed", Width = 90 },
            new ColumnHeader { Text = "MAC", Width = 140 },
        ]);
        ListViewColumnSorter.AttachTo(_adapterList);

        _adapterList.SelectedIndexChanged += OnSelectionChanged;

        _btnEnable.Click += async (_, _) => await SetAdapterStateAsync(enable: true);
        _btnDisable.Click += async (_, _) => await SetAdapterStateAsync(enable: false);
        _btnDiagnose.Click += async (_, _) => await DiagnoseAsync();
        _btnRefresh.Click += async (_, _) => await RefreshAsync();
        _btnClose.Click += (_, _) => Close();

        _btnPanel.Controls.AddRange([_btnEnable, _btnDisable, _btnDiagnose, _btnRefresh, _btnClose]);

        if (!Elevation.IsAdmin())
            Controls.Add(CreateAdminBanner("⚠  Administrator rights required to enable or disable adapters."));

        BuildLayout();
        LayoutButtons();

        _ = RefreshAsync();
    }

    private void BuildLayout()
    {
        var splitter = new SplitContainer
        {
            Dock = DockStyle.Fill,
            Orientation = Orientation.Horizontal,
            SplitterDistance = 260,
        };
        splitter.Panel1.Controls.Add(_adapterList);
        splitter.Panel2.Controls.Add(_detailBox);

        Controls.Add(splitter);
        Controls.Add(_statusLabel);
        Controls.Add(_btnPanel);
    }

    private void LayoutButtons()
    {
        int x = 8;
        foreach (Button b in new[] { _btnEnable, _btnDisable, _btnDiagnose, _btnRefresh })
        {
            b.Location = new Point(x, 5);
            x += b.Width + 6;
        }
        _btnClose.Location = new Point(_btnPanel.Width - _btnClose.Width - 8, 5);
        _btnPanel.Resize += (_, _) => _btnClose.Location = new Point(_btnPanel.Width - _btnClose.Width - 8, 5);
    }

    private async Task RefreshAsync()
    {
        _btnRefresh.Enabled = false;
        _statusLabel.Text = "Loading adapters…";
        _adapters = await Task.Run(EnumerateAdapters);

        _adapterList.BeginUpdate();
        _adapterList.Items.Clear();
        foreach (AdapterInfo a in _adapters)
        {
            string statusIcon =
                a.Status == "Up" ? "● "
                : a.Status == "Down" ? "○ "
                : "~ ";
            var lvi = new ListViewItem(a.Name);
            lvi.SubItems.Add(statusIcon + a.Status);
            lvi.SubItems.Add(a.Type);
            lvi.SubItems.Add(string.IsNullOrEmpty(a.IpAddress) ? "—" : a.IpAddress);
            lvi.SubItems.Add(a.Speed);
            lvi.SubItems.Add(a.MacAddress);
            lvi.ForeColor = a.Status == "Up" ? Color.FromArgb(0, 180, 80) : Color.FromArgb(150, 150, 150);
            _adapterList.Items.Add(lvi);
        }
        _adapterList.EndUpdate();

        int up = _adapters.Count(a => a.Status == "Up");
        _statusLabel.Text = $"{_adapters.Count} adapter(s) found  ·  {up} connected";
        _btnRefresh.Enabled = true;
    }

    private static List<AdapterInfo> EnumerateAdapters()
    {
        var result = new List<AdapterInfo>();
        var nicMap = NetworkInterface
            .GetAllNetworkInterfaces()
            .Where(n => n.NetworkInterfaceType != NetworkInterfaceType.Loopback)
            .ToDictionary(n => n.Name, StringComparer.OrdinalIgnoreCase);

        try
        {
            using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapter WHERE NetEnabled IS NOT NULL");
            foreach (ManagementObject obj in searcher.Get().Cast<ManagementObject>())
            {
                string name = obj["Name"] as string ?? "(unknown)";
                string desc = obj["Description"] as string ?? "";
                string mac = obj["MACAddress"] as string ?? "";
                string deviceId = obj["DeviceID"] as string ?? "";
                bool? netEnabled = obj["NetEnabled"] as bool?;
                string status =
                    netEnabled == true ? "Up"
                    : netEnabled == false ? "Down"
                    : "Unknown";
                string type = obj["AdapterType"] as string ?? "Unknown";
                if (type.Length > 20)
                    type = type[..20];

                // Try to get IP info from NetworkInterface
                string ip = "",
                    gateway = "",
                    speed = "";
                if (nicMap.TryGetValue(name, out NetworkInterface? nic))
                {
                    status = nic.OperationalStatus == OperationalStatus.Up ? "Up" : "Down";
                    var props = nic.GetIPProperties();
                    ip =
                        props
                            .UnicastAddresses.Where(a => a.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            .Select(a => a.Address.ToString())
                            .FirstOrDefault()
                        ?? "";
                    gateway = props.GatewayAddresses.Select(g => g.Address.ToString()).FirstOrDefault() ?? "";
                    long bps = nic.Speed;
                    speed =
                        bps >= 1_000_000_000 ? $"{bps / 1_000_000_000} Gbps"
                        : bps >= 1_000_000 ? $"{bps / 1_000_000} Mbps"
                        : bps > 0 ? $"{bps / 1_000} Kbps"
                        : "—";
                }

                result.Add(new AdapterInfo(name, desc, status, type, mac, ip, gateway, speed, deviceId));
            }
        }
        catch
        {
            // Fallback: use NetworkInterface only
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                    continue;
                string status2 = nic.OperationalStatus == OperationalStatus.Up ? "Up" : "Down";
                var props = nic.GetIPProperties();
                string ip2 =
                    props
                        .UnicastAddresses.Where(a => a.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        .Select(a => a.Address.ToString())
                        .FirstOrDefault()
                    ?? "";
                string gw = props.GatewayAddresses.Select(g => g.Address.ToString()).FirstOrDefault() ?? "";
                long bps = nic.Speed;
                string spd =
                    bps >= 1_000_000_000 ? $"{bps / 1_000_000_000} Gbps"
                    : bps >= 1_000_000 ? $"{bps / 1_000_000} Mbps"
                    : bps > 0 ? $"{bps / 1_000} Kbps"
                    : "—";
                result.Add(
                    new AdapterInfo(
                        nic.Name,
                        nic.Description,
                        status2,
                        nic.NetworkInterfaceType.ToString(),
                        nic.GetPhysicalAddress().ToString(),
                        ip2,
                        gw,
                        spd,
                        ""
                    )
                );
            }
        }
        return result;
    }

    private void OnSelectionChanged(object? sender, EventArgs e)
    {
        if (_adapterList.SelectedItems.Count == 0)
            return;
        int idx = _adapterList.SelectedItems[0].Index;
        AdapterInfo a = _adapters[idx];
        _btnEnable.Enabled = a.Status != "Up";
        _btnDisable.Enabled = a.Status == "Up";
        _btnDiagnose.Enabled = true;

        var sb = new StringBuilder();
        sb.AppendLine($"Name:         {a.Name}");
        sb.AppendLine($"Description:  {a.Description}");
        sb.AppendLine($"Status:       {a.Status}");
        sb.AppendLine($"Type:         {a.Type}");
        sb.AppendLine($"IP Address:   {(string.IsNullOrEmpty(a.IpAddress) ? "—" : a.IpAddress)}");
        sb.AppendLine($"Gateway:      {(string.IsNullOrEmpty(a.Gateway) ? "—" : a.Gateway)}");
        sb.AppendLine($"Speed:        {a.Speed}");
        sb.AppendLine($"MAC Address:  {a.MacAddress}");
        _detailBox.Text = sb.ToString();
    }

    private async Task SetAdapterStateAsync(bool enable)
    {
        if (_adapterList.SelectedItems.Count == 0)
            return;
        AdapterInfo adapter = _adapters[_adapterList.SelectedItems[0].Index];
        _btnEnable.Enabled = false;
        _btnDisable.Enabled = false;
        _statusLabel.Text = $"{(enable ? "Enabling" : "Disabling")} \"{adapter.Name}\"…";

        try
        {
            await Task.Run(() =>
            {
                string action = enable ? "enable" : "disable";
                var info = new ProcessStartInfo("netsh", $"interface set interface \"{adapter.Name}\" admin={action}")
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                };
                using Process? proc = Process.Start(info);
                proc?.WaitForExit();
            });
            _statusLabel.Text = $"✓ \"{adapter.Name}\" {(enable ? "enabled" : "disabled")}.";
            await RefreshAsync();
        }
        catch (Exception ex)
        {
            _statusLabel.Text = $"✗ Failed: {ex.Message}";
            _btnEnable.Enabled = !enable;
            _btnDisable.Enabled = enable;
        }
    }

    private async Task DiagnoseAsync()
    {
        if (_adapterList.SelectedItems.Count == 0)
            return;
        AdapterInfo adapter = _adapters[_adapterList.SelectedItems[0].Index];
        _btnDiagnose.Enabled = false;
        _statusLabel.Text = $"Running diagnostics for \"{adapter.Name}\"…";

        var sb = new StringBuilder();
        sb.AppendLine($"=== Diagnostics: {adapter.Name} ===\r\n");

        await Task.Run(() =>
        {
            // Ping gateway
            if (!string.IsNullOrEmpty(adapter.Gateway))
            {
                sb.AppendLine($"Ping gateway ({adapter.Gateway}):");
                try
                {
                    using var ping = new Ping();
                    PingReply r = ping.Send(adapter.Gateway, 2000);
                    sb.AppendLine($"  {r.Status}  {(r.Status == IPStatus.Success ? $"({r.RoundtripTime} ms)" : "")}");
                }
                catch (Exception ex)
                {
                    sb.AppendLine($"  Error: {ex.Message}");
                }
            }

            // Ping well-known DNS
            sb.AppendLine("\r\nPing 1.1.1.1 (Cloudflare DNS):");
            try
            {
                using var ping = new Ping();
                PingReply r = ping.Send("1.1.1.1", 2000);
                sb.AppendLine($"  {r.Status}  {(r.Status == IPStatus.Success ? $"({r.RoundtripTime} ms)" : "")}");
            }
            catch (Exception ex)
            {
                sb.AppendLine($"  Error: {ex.Message}");
            }

            // DNS resolution test
            sb.AppendLine("\r\nDNS resolution test (github.com):");
            try
            {
                var addresses = System.Net.Dns.GetHostAddresses("github.com");
                sb.AppendLine($"  Resolved to: {string.Join(", ", addresses.Take(2).Select(a => a.ToString()))}");
            }
            catch (Exception ex)
            {
                sb.AppendLine($"  Error: {ex.Message}");
            }
        });

        _detailBox.Text = sb.ToString();
        _statusLabel.Text = "Diagnostic complete.";
        _btnDiagnose.Enabled = true;
    }
}
