// RegiLattice.GUI — Forms/UsbPowerDialog.cs
// Sprint 31: USB selective suspend per-device control + general USB power settings.

#nullable enable

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Versioning;
using System.Windows.Forms;
using Microsoft.Win32;
using RegiLattice.Core;

namespace RegiLattice.GUI.Forms;

/// <summary>
/// Configures USB selective suspend and USB hub power settings.
/// USB selective suspend allows hubs to suspend idle USB devices to save power.
/// Disabling it can fix instability with certain USB devices (audio, mice).
/// </summary>
[SupportedOSPlatform("windows")]
internal sealed class UsbPowerDialog : BaseDialog
{
    // ── Model ─────────────────────────────────────────────────────────────────
    private sealed record UsbSetting(string Description, string RegPath, string ValueName, int EnabledValue, int DisabledValue, string Note);

    // ── Controls ──────────────────────────────────────────────────────────────
    private readonly ListView _list = new()
    {
        Dock = DockStyle.Fill,
        FullRowSelect = true,
        MultiSelect = false,
        View = View.Details,
        GridLines = true,
        CheckBoxes = true,
    };
    private readonly FlowLayoutPanel _btnPanel = new()
    {
        Dock = DockStyle.Bottom,
        Height = 42,
        Padding = new Padding(6, 6, 6, 4),
        FlowDirection = FlowDirection.LeftToRight,
        WrapContents = false,
    };
    private readonly Button _btnApply = new() { Text = "Apply Selected", Width = 110 };
    private readonly Button _btnDisableAll = new() { Text = "Disable All USB Suspend", Width = 150 };
    private readonly Button _btnRestore = new() { Text = "Restore Defaults", Width = 120 };
    private readonly Button _btnClose = new() { Text = "Close", Width = 80 };
    // Sprint 51 §11a: per-device override list
    private readonly Button _btnDeviceOverrides = new() { Text = "Per-Device Overrides…", Width = 150 };
    // Sprint 51 §11b: log power events
    private readonly CheckBox _chkLogPowerEvents = new() { Text = "Log USB power events", AutoSize = true };
    private readonly List<string> _powerEventLog = [];
    private readonly Label _statusLabel = new()
    {
        Dock = DockStyle.Bottom,
        Height = 22,
        TextAlign = ContentAlignment.MiddleLeft,
        Padding = new Padding(6, 0, 0, 0),
    };
    private readonly RichTextBox _descBox = new()
    {
        Dock = DockStyle.Bottom,
        Height = 72,
        ReadOnly = true,
        BorderStyle = BorderStyle.None,
        BackColor = SystemColors.Control,
        Font = new Font(SystemFonts.DefaultFont.FontFamily, 8.5f),
        Margin = new Padding(4),
    };

    private readonly List<UsbSetting> _settings = [];

    // ── Construction ──────────────────────────────────────────────────────────
    internal UsbPowerDialog()
        : base("USB Power & Selective Suspend", new Size(700, 460), resizable: true)
    {
        BuildSettings();
        BuildLayout();
        LoadCurrentState();

        _btnApply.Click += OnApply;
        _btnDisableAll.Click += OnDisableAllSuspend;
        _btnRestore.Click += OnRestoreDefaults;
        _btnClose.Click += (_, _) => Close();
        _list.SelectedIndexChanged += OnSelectionChanged;

        if (!Elevation.IsAdmin())
            Controls.Add(CreateAdminBanner("⚠  Administrator rights required to modify USB power settings."));
    }

    // ── Settings Definitions ──────────────────────────────────────────────────
    private void BuildSettings()
    {
        _settings.AddRange([
            new UsbSetting(
                "USB Selective Suspend (AC power)",
                @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USB",
                "DisableSelectiveSuspend",
                0,
                1,
                "Allows Windows to suspend idle USB devices when on AC power to save energy. "
                    + "Disable to prevent audio dropouts, mouse disconnects, or USB device instability."
            ),
            new UsbSetting(
                "USB Selective Suspend (Battery)",
                @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USB",
                "DisableSelectiveSuspend",
                0,
                1,
                "Same as above but applies when on battery power."
            ),
            new UsbSetting(
                "Allow USB Hub to power off idle ports",
                @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\usbhub",
                "DisableSelectiveSuspend",
                0,
                1,
                "Allows the USB hub driver to power off ports with idle devices. "
                    + "Useful for reducing power draw. Disable if connected USB devices lose connection unexpectedly."
            ),
            new UsbSetting(
                "USB 3.0 Link Power Management",
                @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\USB",
                "DisableIdleForce",
                0,
                1,
                "Controls USB 3.0 link power management. When enabled, USB 3.0 links can enter "
                    + "lower-power U1/U2 states. Disable if you experience USB 3.0 device disconnections."
            ),
            new UsbSetting(
                "Allow wake from USB (WoL via USB)",
                @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Class\{36FC9E60-C465-11CF-8056-444553540000}\0000",
                "EnhancedPowerManagementEnabled",
                0,
                1,
                "Allows USB devices (keyboard, mouse) to wake the system from sleep. "
                    + "Disable to prevent accidental wake from vibration or USB noise."
            ),
        ]);
    }

    // ── Layout ────────────────────────────────────────────────────────────────
    private void BuildLayout()
    {
        _list.Columns.AddRange([
            new ColumnHeader { Text = "Setting", Width = 330 },
            new ColumnHeader { Text = "Current State", Width = 120 },
            new ColumnHeader { Text = "Recommended", Width = 110 },
        ]);
        ListViewColumnSorter.AttachTo(_list);

        _btnDeviceOverrides.Click += OnOpenDeviceOverrides;
        _btnPanel.Controls.AddRange([_btnApply, _btnDisableAll, _btnRestore, _btnDeviceOverrides, _btnClose]);
        Controls.AddRange([_list, _descBox, _statusLabel, _chkLogPowerEvents, _btnPanel]);
    }

    // ── Load State ────────────────────────────────────────────────────────────
    private void LoadCurrentState()
    {
        _list.BeginUpdate();
        _list.Items.Clear();

        foreach (var s in _settings)
        {
            string currentState = ReadCurrentState(s);
            bool isEnabled = currentState == "Enabled";

            var item = new ListViewItem(s.Description) { Tag = s, Checked = isEnabled };
            item.SubItems.Add(currentState);
            item.SubItems.Add("Enabled");
            if (!isEnabled)
                item.ForeColor = Color.DarkOrange;
            _list.Items.Add(item);
        }

        _list.EndUpdate();
        _statusLabel.Text = "Current USB power settings loaded.";
    }

    private static string ReadCurrentState(UsbSetting s)
    {
        try
        {
            string hive = s.RegPath.StartsWith("HKEY_LOCAL_MACHINE", StringComparison.OrdinalIgnoreCase)
                ? s.RegPath.Substring("HKEY_LOCAL_MACHINE\\".Length)
                : s.RegPath;

            using var key = Registry.LocalMachine.OpenSubKey(hive);
            if (key is null)
                return "Not set";
            var val = key.GetValue(s.ValueName);
            if (val is int i)
                return i == s.EnabledValue ? "Enabled" : "Disabled";
            return "Not set";
        }
        catch
        {
            return "Error";
        }
    }

    // ── Apply ─────────────────────────────────────────────────────────────────
    private void OnApply(object? sender, EventArgs e)
    {
        if (!Elevation.IsAdmin())
        {
            MessageBox.Show("Administrator rights required.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        int changed = 0;
        foreach (ListViewItem item in _list.Items)
        {
            if (item.Tag is not UsbSetting s)
                continue;
            bool enable = item.Checked;
            int valueToWrite = enable ? s.EnabledValue : s.DisabledValue;

            try
            {
                string subKey = s.RegPath.StartsWith("HKEY_LOCAL_MACHINE", StringComparison.OrdinalIgnoreCase)
                    ? s.RegPath.Substring("HKEY_LOCAL_MACHINE\\".Length)
                    : s.RegPath;

                using var key = Registry.LocalMachine.CreateSubKey(subKey, writable: true);
                key?.SetValue(s.ValueName, valueToWrite, RegistryValueKind.DWord);
                item.SubItems[1].Text = enable ? "Enabled" : "Disabled";
                item.ForeColor = enable ? SystemColors.WindowText : Color.DarkOrange;
                changed++;
            }
            catch (Exception ex)
            {
                _statusLabel.Text = $"Error writing {s.Description}: {ex.Message}";
                return;
            }
        }

        _statusLabel.Text = $"✓ {changed} setting(s) applied. Some changes require a restart to take effect.";
    }

    private void OnDisableAllSuspend(object? sender, EventArgs e)
    {
        foreach (ListViewItem item in _list.Items)
            item.Checked = false;
        _statusLabel.Text = "All USB suspend settings unchecked — click Apply to write.";
    }

    private void OnRestoreDefaults(object? sender, EventArgs e)
    {
        foreach (ListViewItem item in _list.Items)
            item.Checked = true;
        _statusLabel.Text = "All restored to recommended defaults — click Apply to write.";
    }

    private void OnSelectionChanged(object? sender, EventArgs e)
    {
        if (_list.SelectedItems.Count == 0)
            return;
        if (_list.SelectedItems[0].Tag is UsbSetting s)
            _descBox.Text = s.Note;
    }

    // Sprint 51 §11a — Per-device override list.
    private void OnOpenDeviceOverrides(object? sender, EventArgs e)
    {
        using var dlg = new Form
        {
            Text = "Per-Device USB Power Overrides",
            Size = new Size(560, 400),
            FormBorderStyle = FormBorderStyle.Sizable,
            StartPosition = FormStartPosition.CenterParent,
        };
        var lv = new ListView
        {
            Dock = DockStyle.Fill,
            View = View.Details,
            FullRowSelect = true,
            GridLines = true,
        };
        lv.Columns.Add("Device Path / ID", 320);
        lv.Columns.Add("Power Mode", 180);
        var hint = new Label
        {
            Text = "Add device instance paths (from Device Manager) and their power mode overrides.",
            Dock = DockStyle.Bottom,
            AutoSize = false,
            Height = 36,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(4, 0, 0, 0),
        };
        var btnClose2 = new Button { Text = "Close", Dock = DockStyle.Bottom, Height = 30 };
        btnClose2.Click += (_, _) => dlg.Close();
        dlg.Controls.AddRange([lv, hint, btnClose2]);
        dlg.ShowDialog(this);
    }
}
