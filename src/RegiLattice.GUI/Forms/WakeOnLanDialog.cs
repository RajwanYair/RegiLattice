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

        Controls.Add(_list);
        Controls.Add(btnPanel);
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
}
