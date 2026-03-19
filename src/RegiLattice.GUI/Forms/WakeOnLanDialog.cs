// RegiLattice.GUI — Forms/WakeOnLanDialog.cs
// Enable/disable Wake-on-LAN per network adapter via registry + Device Power Policy.
#nullable enable

using System.Management;
using System.Net.NetworkInformation;
using Microsoft.Win32;

namespace RegiLattice.GUI.Forms;

/// <summary>
/// Wake-on-LAN (WoL) Configuration.
/// Lists physical network adapters and allows toggling WoL via the device
/// power management registry key (Device Power Policy AllowWakeFromPower).
/// Also shows the MAC address needed for WoL magic packets.
/// </summary>
internal sealed class WakeOnLanDialog : BaseDialog
{
    private sealed record AdapterInfo(string Name, string MacAddress, string PciSlot, bool WolEnabled, string DeviceId);

    private readonly ListView _list = new()
    {
        View = View.Details,
        FullRowSelect = true,
        MultiSelect = false,
        GridLines = true,
        Dock = DockStyle.Fill,
    };
    private readonly Button _btnEnable = new()
    {
        Text = "Enable WoL",
        Width = 105,
        Height = 28,
        Enabled = false,
    };
    private readonly Button _btnDisable = new()
    {
        Text = "Disable WoL",
        Width = 105,
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
    private readonly Label _lblStatus = new()
    {
        Dock = DockStyle.Top,
        Height = 26,
        TextAlign = ContentAlignment.MiddleLeft,
        Padding = new Padding(6, 0, 0, 0),
    };
    private readonly Label _lblNote = new()
    {
        Text = "ℹ WoL requires BIOS/UEFI support and may need a one-time enable in Device Manager advanced settings.",
        Dock = DockStyle.Top,
        Height = 26,
        TextAlign = ContentAlignment.MiddleLeft,
        ForeColor = Color.FromArgb(0, 100, 160),
        Padding = new Padding(6, 0, 0, 0),
    };

    private List<AdapterInfo> _adapters = [];

    // Manual WoL sender
    private readonly TextBox _txtSendMac = new() { PlaceholderText = "AA:BB:CC:DD:EE:FF", Width = 170 };
    private readonly Button _btnSendWol = new()
    {
        Text = "\u2192 Send WoL Packet",
        Width = 130,
        Height = 26,
    };

    // WoL history
    private readonly ListBox _lstWolHistory = new()
    {
        Height = 72,
        Dock = DockStyle.Fill,
        HorizontalScrollbar = true,
    };

    public WakeOnLanDialog()
        : base("Wake-on-LAN Configuration", new Size(860, 520), resizable: true)
    {
        MinimumSize = new Size(680, 400);
        EnableStandaloneMode();

        _list.Columns.AddRange([
            new ColumnHeader { Text = "Adapter Name", Width = 280 },
            new ColumnHeader { Text = "MAC Address", Width = 140 },
            new ColumnHeader { Text = "WoL Status", Width = 100 },
            new ColumnHeader { Text = "Device ID", Width = 240 },
        ]);
        ListViewColumnSorter.AttachTo(_list);
        _list.SelectedIndexChanged += (_, _) => UpdateButtons();

        var btnPanel = new Panel { Dock = DockStyle.Bottom, Height = 38 };
        int bx = 8;
        foreach (Button b in new[] { _btnEnable, _btnDisable, _btnRefresh })
        {
            b.Location = new Point(bx, 5);
            bx += b.Width + 6;
        }
        _btnClose.Location = new Point(btnPanel.Width - _btnClose.Width - 8, 5);
        btnPanel.Resize += (_, _) => _btnClose.Location = new Point(btnPanel.Width - _btnClose.Width - 8, 5);
        _btnEnable.Click += async (_, _) => await SetWolAsync(true);
        _btnDisable.Click += async (_, _) => await SetWolAsync(false);
        _btnRefresh.Click += async (_, _) => await LoadAdaptersAsync();
        _btnClose.Click += (_, _) => Close();
        btnPanel.Controls.AddRange(new Control[] { _btnEnable, _btnDisable, _btnRefresh, _btnClose });

        // ── Manual WoL sender panel ──────────────────────────────
        _txtSendMac.BackColor = Color.FromArgb(30, 30, 40);
        _txtSendMac.ForeColor = Color.White;
        _btnSendWol.Click += OnSendWolPacket;
        var sendPanel = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 34,
            Padding = new Padding(6, 4, 6, 0),
        };
        var sendLabel = new Label
        {
            Text = "Manual WoL:",
            AutoSize = true,
            Location = new Point(6, 8),
        };
        _txtSendMac.Location = new Point(88, 4);
        _btnSendWol.Location = new Point(264, 4);
        sendPanel.Controls.AddRange(new Control[] { sendLabel, _txtSendMac, _btnSendWol });

        // ── WoL history panel ────────────────────────────────────
        _lstWolHistory.BackColor = Color.FromArgb(25, 25, 35);
        _lstWolHistory.ForeColor = Color.LightGray;
        var histContainer = new GroupBox
        {
            Text = "WoL Send History",
            Dock = DockStyle.Bottom,
            Height = 100,
            ForeColor = Color.LightGray,
        };
        histContainer.Controls.Add(_lstWolHistory);

        Controls.Add(_list);
        Controls.Add(btnPanel);
        Controls.Add(sendPanel);
        Controls.Add(histContainer);
        Controls.Add(_lblStatus);
        Controls.Add(_lblNote);

        _ = LoadAdaptersAsync();
    }

    private async Task LoadAdaptersAsync()
    {
        _btnRefresh.Enabled = false;
        _lblStatus.Text = "Detecting network adapters…";
        _list.Items.Clear();

        _adapters = await Task.Run(DiscoverAdapters);
        Populate();
        _lblStatus.Text = $"{_adapters.Count} physical network adapter(s) found.";
        _btnRefresh.Enabled = true;
    }

    private static List<AdapterInfo> DiscoverAdapters()
    {
        var result = new List<AdapterInfo>();
        var macMap = NetworkInterface
            .GetAllNetworkInterfaces()
            .Where(n =>
                n.NetworkInterfaceType is NetworkInterfaceType.Ethernet or NetworkInterfaceType.Wireless80211
                && n.OperationalStatus != OperationalStatus.Unknown
            )
            .ToDictionary(n => n.Name, n => FormatMac(n.GetPhysicalAddress()));

        try
        {
            using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapter WHERE PhysicalAdapter=True");
            foreach (ManagementObject obj in searcher.Get())
            {
                string name = obj["Name"]?.ToString() ?? "";
                string mac = obj["MACAddress"]?.ToString() ?? "";
                string devId = obj["PNPDeviceID"]?.ToString() ?? "";

                bool wolEnabled = ReadWolRegistry(devId);
                result.Add(new AdapterInfo(name, mac, "", wolEnabled, devId));
            }
        }
        catch
        { /* WMI unavailable */
        }

        return result;
    }

    private static bool ReadWolRegistry(string pnpDeviceId)
    {
        if (string.IsNullOrEmpty(pnpDeviceId))
            return false;
        // HKLM\SYSTEM\CurrentControlSet\Enum\<PNPDeviceID>\Device Parameters\Power Management
        string regPath = $@"SYSTEM\CurrentControlSet\Enum\{pnpDeviceId}\Device Parameters\Power Management";
        using var key = Registry.LocalMachine.OpenSubKey(regPath);
        if (key is null)
            return false;
        return (key.GetValue("WakeFromD0") as int? ?? 0) != 0;
    }

    private void Populate()
    {
        _list.BeginUpdate();
        _list.Items.Clear();
        foreach (AdapterInfo a in _adapters)
        {
            var lvi = new ListViewItem(a.Name);
            lvi.SubItems.Add(a.MacAddress);
            lvi.SubItems.Add(a.WolEnabled ? "✓ Enabled" : "✗ Disabled");
            lvi.SubItems.Add(a.DeviceId);
            if (!a.WolEnabled)
                lvi.ForeColor = Color.FromArgb(128, 128, 128);
            _list.Items.Add(lvi);
        }
        _list.EndUpdate();
    }

    private void UpdateButtons()
    {
        bool sel = _list.SelectedItems.Count > 0;
        _btnEnable.Enabled = sel;
        _btnDisable.Enabled = sel;
    }

    private async Task SetWolAsync(bool enable)
    {
        if (_list.SelectedItems.Count == 0)
            return;
        int idx = _list.SelectedIndices[0];
        if (idx >= _adapters.Count)
            return;
        AdapterInfo adapter = _adapters[idx];

        _btnEnable.Enabled = _btnDisable.Enabled = false;
        _lblStatus.Text = $"{(enable ? "Enabling" : "Disabling")} WoL for {adapter.Name}…";

        bool success = await Task.Run(() => SetWolRegistry(adapter.DeviceId, enable));
        _lblStatus.Text = success ? $"✓ WoL {(enable ? "enabled" : "disabled")} for {adapter.Name}." : "✗ Access denied. Run as administrator.";
        await LoadAdaptersAsync();
    }

    private static bool SetWolRegistry(string pnpDeviceId, bool enable)
    {
        if (string.IsNullOrEmpty(pnpDeviceId))
            return false;
        try
        {
            string regPath = $@"SYSTEM\CurrentControlSet\Enum\{pnpDeviceId}\Device Parameters\Power Management";
            using var key = Registry.LocalMachine.OpenSubKey(regPath, writable: true);
            if (key is null)
                return false;
            int val = enable ? 1 : 0;
            key.SetValue("WakeFromD0", val, RegistryValueKind.DWord);
            key.SetValue("WakeFromD3Cold_SupportS0", val, RegistryValueKind.DWord);
            return true;
        }
        catch (UnauthorizedAccessException)
        {
            return false;
        }
    }

    private static string FormatMac(System.Net.NetworkInformation.PhysicalAddress addr)
    {
        var bytes = addr.GetAddressBytes();
        return string.Join(":", bytes.Select(b => b.ToString("X2")));
    }

    private void OnSendWolPacket(object? sender, EventArgs e)
    {
        string raw = _txtSendMac.Text.Trim();
        if (string.IsNullOrEmpty(raw))
        {
            // Auto-fill from selected adapter if available
            if (_list.SelectedItems.Count > 0)
            {
                int idx = _list.SelectedIndices[0];
                if (idx < _adapters.Count)
                    raw = _adapters[idx].MacAddress;
            }
        }
        if (string.IsNullOrWhiteSpace(raw))
        {
            _lblStatus.Text = "\u274c Enter a MAC address or select an adapter first.";
            return;
        }

        try
        {
            // Normalise: remove separators, expect 12 hex digits
            string hex = raw.Replace(":", "").Replace("-", "").Replace(".", "").ToUpperInvariant();
            if (hex.Length != 12 || !hex.All(c => "0123456789ABCDEF".Contains(c)))
            {
                _lblStatus.Text = "\u274c Invalid MAC address format.";
                return;
            }

            // Build magic packet: 6 × 0xFF then 16 × MAC (102 bytes)
            var mac = Enumerable.Range(0, 6).Select(i => Convert.ToByte(hex.Substring(i * 2, 2), 16)).ToArray();
            var packet = new byte[102];
            for (int i = 0; i < 6; i++)
                packet[i] = 0xFF;
            for (int rep = 0; rep < 16; rep++)
                Array.Copy(mac, 0, packet, 6 + rep * 6, 6);

            using var udp = new System.Net.Sockets.UdpClient();
            udp.EnableBroadcast = true;
            udp.Connect(System.Net.IPAddress.Broadcast, 9);
            udp.Send(packet, packet.Length);

            string entry = $"{DateTime.Now:HH:mm:ss}  WoL \u2714  {raw}";
            _lstWolHistory.Items.Insert(0, entry);
            _lblStatus.Text = $"\u2705 Magic packet sent to {raw}.";
        }
        catch (Exception ex)
        {
            _lblStatus.Text = $"\u274c {ex.Message}";
        }
    }
}
