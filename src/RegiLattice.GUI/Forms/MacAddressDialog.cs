// RegiLattice.GUI — Forms/MacAddressDialog.cs
// Sprint 42: MAC address viewer and randomizer (Phase 4 item 40).

#nullable enable

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Management;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using RegiLattice.Core;

namespace RegiLattice.GUI.Forms;

/// <summary>
/// Displays MAC addresses for all physical network adapters (via WMI Win32_NetworkAdapter).
/// Allows viewing and optionally randomizing the locally-administered MAC address via
/// the registry NetworkAddress value and netsh adapter reset.
/// </summary>
[SupportedOSPlatform("windows")]
internal sealed class MacAddressDialog : BaseDialog
{
    private sealed record AdapterInfo(
        string Name, string MacAddress, string Description, bool IsPhysical, string PnpId);

    private readonly ListView _listView = new()
    {
        Dock = DockStyle.Fill,
        FullRowSelect = true,
        View = View.Details,
        GridLines = true,
        MultiSelect = false,
    };
    private readonly Label _statusLabel = new()
    {
        Dock = DockStyle.Bottom,
        Height = 22,
        TextAlign = ContentAlignment.MiddleLeft,
        Padding = new Padding(6, 0, 0, 0),
    };
    private readonly Button _btnRefresh = new() { Text = "\u21BB  Refresh", Width = 100, Height = 30 };
    private readonly Button _btnRandomize = new() { Text = "\uD83C\uDFB2  Randomize MAC", Width = 140, Height = 30, Enabled = false };
    private readonly Button _btnCopy = new() { Text = "Copy MAC", Width = 90, Height = 30, Enabled = false };
    private readonly Button _btnClose = new() { Text = "Close", Width = 80, Height = 30 };

    private readonly List<AdapterInfo> _adapters = [];

    internal MacAddressDialog()
        : base("MAC Address Manager", new Size(640, 440), resizable: true)
    {
        BuildLayout();
        _ = RefreshAsync();
    }

    private void BuildLayout()
    {
        _listView.Columns.AddRange([
            new ColumnHeader { Text = "Adapter Name", Width = 200 },
            new ColumnHeader { Text = "MAC Address", Width = 150 },
            new ColumnHeader { Text = "Description", Width = 220 },
        ]);

        var btnPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            Height = 44,
            FlowDirection = FlowDirection.RightToLeft,
            WrapContents = false,
            Padding = new Padding(8, 6, 8, 6),
        };
        btnPanel.Controls.AddRange([_btnClose, _btnCopy, _btnRandomize, _btnRefresh]);

        var warnLabel = CreateWarningBanner(
            "MAC randomization requires administrator rights and a NIC that supports " +
            "the NetworkAddress registry override. Disable/re-enable the adapter to apply.");

        Controls.Add(_listView);
        Controls.Add(_statusLabel);
        Controls.Add(btnPanel);
        Controls.Add(warnLabel);

        _listView.SelectedIndexChanged += OnSelectionChanged;
        _btnRefresh.Click += async (_, _) => await RefreshAsync();
        _btnCopy.Click += OnCopy;
        _btnRandomize.Click += async (_, _) => await RandomizeMacAsync();
        _btnClose.Click += (_, _) => Close();

        AppTheme.Apply(this);
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        AppTheme.Apply(this);
    }

    private void OnSelectionChanged(object? sender, EventArgs e)
    {
        bool hasSelection = _listView.SelectedIndices.Count > 0;
        _btnCopy.Enabled = hasSelection;
        _btnRandomize.Enabled = hasSelection;
    }

    private void OnCopy(object? sender, EventArgs e)
    {
        if (_listView.SelectedIndices.Count == 0) return;
        int idx = _listView.SelectedIndices[0];
        if (idx < _adapters.Count)
        {
            Clipboard.SetText(_adapters[idx].MacAddress);
            _statusLabel.Text = $"Copied: {_adapters[idx].MacAddress}";
        }
    }

    private async Task RefreshAsync()
    {
        _btnRefresh.Enabled = false;
        _statusLabel.Text = "Enumerating network adapters…";

        var adapters = await Task.Run(QueryAdapters);

        _adapters.Clear();
        _adapters.AddRange(adapters);

        _listView.BeginUpdate();
        _listView.Items.Clear();
        foreach (var a in _adapters)
        {
            var item = new ListViewItem(a.Name);
            item.SubItems.Add(FormatMac(a.MacAddress));
            item.SubItems.Add(a.Description);
            if (!a.IsPhysical) item.ForeColor = Color.Gray;
            _listView.Items.Add(item);
        }
        _listView.EndUpdate();

        _statusLabel.Text = $"{adapters.Count} physical adapter(s) found  |  {DateTime.Now:HH:mm:ss}";
        _btnRefresh.Enabled = true;
        AppTheme.Apply(this);
    }

    private static List<AdapterInfo> QueryAdapters()
    {
        var results = new List<AdapterInfo>();
        try
        {
            using var mos = new ManagementObjectSearcher(
                "SELECT Name, MACAddress, Description, PNPDeviceID, PhysicalAdapter " +
                "FROM Win32_NetworkAdapter WHERE MACAddress IS NOT NULL"
            );
            foreach (ManagementObject obj in mos.Get())
            {
                string name = obj["Name"]?.ToString() ?? "Unknown";
                string mac = obj["MACAddress"]?.ToString() ?? string.Empty;
                string desc = obj["Description"]?.ToString() ?? string.Empty;
                bool physical = obj["PhysicalAdapter"] is bool b && b;
                string pnp = obj["PNPDeviceID"]?.ToString() ?? string.Empty;
                results.Add(new AdapterInfo(name, mac, desc, physical, pnp));
            }
        }
        catch (ManagementException) { /* ignore */ }
        return results;
    }

    private async Task RandomizeMacAsync()
    {
        if (_listView.SelectedIndices.Count == 0) return;
        int idx = _listView.SelectedIndices[0];
        if (idx >= _adapters.Count) return;

        var adapter = _adapters[idx];
        var confirm = MessageBox.Show(
            $"Randomize MAC address for:\n\n{adapter.Name}\nCurrent: {FormatMac(adapter.MacAddress)}\n\n" +
            "This writes to the registry NetworkAddress key and disables/re-enables the adapter.\n" +
            "Administrator rights are required. Continue?",
            "Confirm MAC Randomization",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning
        );
        if (confirm != DialogResult.Yes) return;

        _btnRandomize.Enabled = false;
        _statusLabel.Text = "Randomizing MAC…";

        var (success, msg) = await Task.Run(() => ApplyRandomMac(adapter));
        _statusLabel.Text = msg;
        _btnRandomize.Enabled = true;

        if (success) await RefreshAsync();
    }

    private static (bool Success, string Message) ApplyRandomMac(AdapterInfo adapter)
    {
        // Generate a locally-administered unicast MAC:
        // Set bit 1 (locally administered) of first byte, clear bit 0 (unicast)
        var rng = new Random();
        var macBytes = new byte[6];
        rng.NextBytes(macBytes);
        macBytes[0] = (byte)((macBytes[0] & 0xFE) | 0x02); // unicast + locally administered

        string newMac = BitConverter.ToString(macBytes).Replace("-", string.Empty);

        // Write via netsh (simplest cross-device approach — sets NetworkAddress via registry)
        var (exitCode, _, _) = ShellRunner.Run(
            "netsh",
            ["interface", "set", "interface", adapter.Name, "admin=disable"]);
        if (exitCode != 0)
            return (false, $"Failed to disable adapter '{adapter.Name}' (exit {exitCode})");

        // Set NetworkAddress in adapter's HKLM registry path
        try
        {
            // Find the adapter's registry index under Network\Config\Class
            string? regPath = FindAdapterRegPath(adapter.PnpId);
            if (regPath != null)
            {
                using var key = Registry.LocalMachine.OpenSubKey(regPath, writable: true);
                key?.SetValue("NetworkAddress", newMac, RegistryValueKind.String);
            }
        }
        catch (UnauthorizedAccessException)
        {
            ShellRunner.Run("netsh", ["interface", "set", "interface", adapter.Name, "admin=enable"]);
            return (false, "Access denied — run RegiLattice as Administrator.");
        }

        ShellRunner.Run("netsh", ["interface", "set", "interface", adapter.Name, "admin=enable"]);
        return (true, $"MAC randomized to {FormatMac(newMac)}. Adapter re-enabled.");
    }

    private static string? FindAdapterRegPath(string pnpId)
    {
        if (string.IsNullOrEmpty(pnpId)) return null;
        const string classKey = @"SYSTEM\CurrentControlSet\Control\Class\{4D36E972-E325-11CE-BFC1-08002BE10318}";
        using var classRoot = Registry.LocalMachine.OpenSubKey(classKey);
        if (classRoot == null) return null;
        foreach (string sub in classRoot.GetSubKeyNames())
        {
            using var subKey = classRoot.OpenSubKey(sub);
            if (subKey?.GetValue("MatchingDeviceId") is string id &&
                string.Equals(id, pnpId, StringComparison.OrdinalIgnoreCase))
                return $@"{classKey}\{sub}";
        }
        return null;
    }

    private static string FormatMac(string raw)
    {
        if (string.IsNullOrEmpty(raw)) return "(unknown)";
        // Already formatted (AA:BB:CC...) or raw 12-char
        if (raw.Contains(':') || raw.Contains('-')) return raw;
        if (raw.Length == 12)
            return string.Join(":", Enumerable.Range(0, 6).Select(i => raw.Substring(i * 2, 2)));
        return raw;
    }
}
