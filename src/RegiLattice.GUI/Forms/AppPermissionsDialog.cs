// RegiLattice.GUI — Forms/AppPermissionsDialog.cs
// Sprint 32: Granular app permission manager (camera, mic, location, etc.).

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
/// Provides a single-window view of all Windows app capability permissions
/// (camera, microphone, location, calendar, contacts, etc.) with per-capability
/// "Allow" / "Block" controls via HKLM policy keys.
/// </summary>
[SupportedOSPlatform("windows")]
internal sealed class AppPermissionsDialog : BaseDialog
{
    // ── Model ─────────────────────────────────────────────────────────────────
    private sealed record PermissionItem(
        string Category,
        string Name,
        string Description,
        string PolicyKeyPath,
        string PolicyValueName,
        int BlockValue = 2,
        int AllowValue = 0
    );

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
    private readonly RichTextBox _descBox = new()
    {
        Dock = DockStyle.Bottom,
        Height = 80,
        ReadOnly = true,
        BorderStyle = BorderStyle.FixedSingle,
        Font = new Font(SystemFonts.DefaultFont.FontFamily, 8.5f),
    };
    private readonly FlowLayoutPanel _btnPanel = new()
    {
        Dock = DockStyle.Bottom,
        Height = 42,
        Padding = new Padding(6, 6, 6, 4),
        FlowDirection = FlowDirection.LeftToRight,
    };
    private readonly Button _btnBlockAll = new() { Text = "Block All Checked", Width = 120 };
    private readonly Button _btnAllowAll = new() { Text = "Allow All", Width = 90 };
    private readonly Button _btnClose = new() { Text = "Close", Width = 80 };
    private readonly Label _statusLabel = new()
    {
        Dock = DockStyle.Bottom,
        Height = 22,
        TextAlign = ContentAlignment.MiddleLeft,
        Padding = new Padding(6, 0, 0, 0),
    };

    private readonly List<PermissionItem> _permissions = [];

    // ── Construction ──────────────────────────────────────────────────────────
    internal AppPermissionsDialog()
        : base("App Permission Manager", new Size(800, 560), resizable: true)
    {
        BuildPermissions();
        BuildLayout();
        LoadCurrentState();

        _btnBlockAll.Click += OnBlockAll;
        _btnAllowAll.Click += OnAllowAll;
        _btnClose.Click += (_, _) => Close();
        _list.SelectedIndexChanged += OnSelectionChanged;

        if (!Elevation.IsAdmin())
            Controls.Add(CreateAdminBanner("⚠  Administrator rights required to set permission policies."));
    }

    // ── Permission Definitions ────────────────────────────────────────────────
    private void BuildPermissions()
    {
        const string AppPrivacy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy";

        _permissions.AddRange([
            new PermissionItem(
                "Hardware",
                "Camera Access",
                "Controls whether apps can access the camera. "
                    + "Block = all apps (including desktop apps via policy) are denied camera access. "
                    + "0 = User-controlled, 1 = Allow (user choice), 2 = Block (deny all).",
                AppPrivacy,
                "LetAppsAccessCamera"
            ),
            new PermissionItem(
                "Hardware",
                "Microphone Access",
                "Controls whether apps can access the microphone. " + "Blocking microphone access prevents voice recording and voice calls in apps.",
                AppPrivacy,
                "LetAppsAccessMicrophone"
            ),
            new PermissionItem(
                "Location",
                "Location Access",
                "Controls whether apps can access the device's precise geographic location. "
                    + "Blocking prevents apps from using GPS/network-based location data.",
                AppPrivacy,
                "LetAppsAccessLocation"
            ),
            new PermissionItem(
                "Communication",
                "Contacts Access",
                "Controls whether apps can read, create, or edit contacts in your address book. "
                    + "Block prevents apps from harvesting your contact list.",
                AppPrivacy,
                "LetAppsAccessContacts"
            ),
            new PermissionItem(
                "Communication",
                "Calendar Access",
                "Controls whether apps can access your calendar events and appointments.",
                AppPrivacy,
                "LetAppsAccessCalendar"
            ),
            new PermissionItem(
                "Communication",
                "Email Access",
                "Controls whether apps can access and send email via your accounts.",
                AppPrivacy,
                "LetAppsAccessEmail"
            ),
            new PermissionItem(
                "Communication",
                "Messaging (SMS/MMS)",
                "Controls whether apps can read or send SMS and MMS messages.",
                AppPrivacy,
                "LetAppsAccessMessaging"
            ),
            new PermissionItem("Communication", "Phone Calls", "Controls whether apps can make phone calls.", AppPrivacy, "LetAppsAccessPhone"),
            new PermissionItem(
                "Hardware",
                "Motion & Activity Sensors",
                "Controls whether apps can access motion and fitness sensor data (accelerometer, gyroscope).",
                AppPrivacy,
                "LetAppsAccessMotion"
            ),
            new PermissionItem(
                "Hardware",
                "Background Sensor Activity",
                "Controls whether apps can access sensors while running in the background.",
                AppPrivacy,
                "LetAppsRunInBackground"
            ),
            new PermissionItem(
                "Identity",
                "Account Information",
                "Controls whether apps can access account name, picture, and other account info.",
                AppPrivacy,
                "LetAppsAccessAccountInfo"
            ),
            new PermissionItem(
                "Identity",
                "User Notification History",
                "Controls whether apps can access your notification history.",
                AppPrivacy,
                "LetAppsAccessNotifications"
            ),
            new PermissionItem(
                "Hardware",
                "Radios (Bluetooth, etc.)",
                "Controls whether apps can access and control wireless radios (Bluetooth, NFC, etc.).",
                AppPrivacy,
                "LetAppsAccessRadios"
            ),
            new PermissionItem(
                "Files",
                "Documents Library",
                "Controls whether apps can access the Documents folder.",
                AppPrivacy,
                "LetAppsAccessDocumentsLibrary"
            ),
            new PermissionItem(
                "Files",
                "Pictures & Videos Libraries",
                "Controls whether apps can access your Pictures and Videos folders.",
                AppPrivacy,
                "LetAppsAccessPicturesLibrary"
            ),
            new PermissionItem(
                "Diagnostics",
                "App Diagnostic Information",
                "Controls whether apps can access diagnostic information about other running apps "
                    + "(process list, memory usage, battery drain data).",
                AppPrivacy,
                "LetAppsAccessDiagnosticInformation"
            ),
        ]);
    }

    // ── Layout ────────────────────────────────────────────────────────────────
    private void BuildLayout()
    {
        _list.Columns.AddRange([
            new ColumnHeader { Text = "Category", Width = 100 },
            new ColumnHeader { Text = "Permission", Width = 200 },
            new ColumnHeader { Text = "Policy Status", Width = 120 },
            new ColumnHeader { Text = "Registry Path", Width = 260 },
        ]);

        _btnPanel.Controls.AddRange([_btnBlockAll, _btnAllowAll, _btnClose]);
        Controls.AddRange([_list, _descBox, _statusLabel, _btnPanel]);
    }

    // ── Load State ────────────────────────────────────────────────────────────
    private void LoadCurrentState()
    {
        _list.BeginUpdate();
        _list.Items.Clear();

        foreach (var perm in _permissions)
        {
            string status = ReadPolicyStatus(perm);
            bool isBlocked = status == "Blocked";
            var lvi = new ListViewItem(perm.Category) { Tag = perm, Checked = false };
            lvi.SubItems.Add(perm.Name);
            lvi.SubItems.Add(isBlocked ? "Blocked ✓" : status);
            lvi.SubItems.Add($"AppPrivacy\\{perm.PolicyValueName}");
            if (isBlocked)
                lvi.ForeColor = Color.DarkOrange;
            _list.Items.Add(lvi);
        }

        _list.EndUpdate();
        int blocked = 0;
        foreach (ListViewItem lvi in _list.Items)
            if (lvi.ForeColor == Color.DarkOrange)
                blocked++;
        _statusLabel.Text = $"{blocked}/{_permissions.Count} permissions currently blocked by policy.";
    }

    private static string ReadPolicyStatus(PermissionItem perm)
    {
        try
        {
            string subKey = perm.PolicyKeyPath.Substring("HKEY_LOCAL_MACHINE\\".Length);
            using var key = Registry.LocalMachine.OpenSubKey(subKey);
            if (key is null)
                return "Not set (User-controlled)";
            var val = key.GetValue(perm.PolicyValueName);
            if (val is int i)
                return i == perm.BlockValue ? "Blocked"
                    : i == perm.AllowValue ? "Allowed"
                    : $"Value={i}";
            return "Not set (User-controlled)";
        }
        catch
        {
            return "Error reading";
        }
    }

    // ── Apply ─────────────────────────────────────────────────────────────────
    private void OnBlockAll(object? sender, EventArgs e)
    {
        if (!Elevation.IsAdmin())
        {
            MessageBox.Show("Administrator rights required.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        int blocked = 0;
        foreach (ListViewItem lvi in _list.Items)
        {
            if (!lvi.Checked || lvi.Tag is not PermissionItem perm)
                continue;
            try
            {
                string subKey = perm.PolicyKeyPath.Substring("HKEY_LOCAL_MACHINE\\".Length);
                using var key = Registry.LocalMachine.CreateSubKey(subKey, writable: true);
                key?.SetValue(perm.PolicyValueName, perm.BlockValue, RegistryValueKind.DWord);
                lvi.SubItems[2].Text = "Blocked ✓";
                lvi.ForeColor = Color.DarkOrange;
                blocked++;
            }
            catch
            {
                lvi.SubItems[2].Text = "Error";
            }
        }

        _statusLabel.Text = $"✓ {blocked} permission(s) blocked. Changes take effect at next app launch.";
    }

    private void OnAllowAll(object? sender, EventArgs e)
    {
        if (!Elevation.IsAdmin())
        {
            MessageBox.Show("Administrator rights required.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        int allowed = 0;
        foreach (ListViewItem lvi in _list.Items)
        {
            if (lvi.Tag is not PermissionItem perm)
                continue;
            try
            {
                string subKey = perm.PolicyKeyPath.Substring("HKEY_LOCAL_MACHINE\\".Length);
                using var key = Registry.LocalMachine.OpenSubKey(subKey, writable: true);
                if (key != null)
                {
                    key.DeleteValue(perm.PolicyValueName, throwOnMissingValue: false);
                    lvi.SubItems[2].Text = "Not set (User-controlled)";
                    lvi.ForeColor = SystemColors.WindowText;
                    allowed++;
                }
            }
            catch
            { /* ignore */
            }
        }

        _statusLabel.Text = $"{allowed} permission policy restriction(s) removed (user-controlled).";
    }

    private void OnSelectionChanged(object? sender, EventArgs e)
    {
        if (_list.SelectedItems.Count > 0 && _list.SelectedItems[0].Tag is PermissionItem perm)
            _descBox.Text = perm.Description;
    }
}
