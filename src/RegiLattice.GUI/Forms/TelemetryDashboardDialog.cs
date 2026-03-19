// RegiLattice.GUI — Forms/TelemetryDashboardDialog.cs
// Sprint 32: Telemetry dashboard — view and control what data Windows sends.

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
/// Shows a visual dashboard of Windows telemetry and data-collection settings.
/// Provides one-click controls to minimise each telemetry category.
/// Covers: Diagnostic data level, Activity history, Location, App diagnostics,
/// Feedback frequency, CEIP, Customer Experience, Error Reporting, and more.
/// </summary>
[SupportedOSPlatform("windows")]
internal sealed class TelemetryDashboardDialog : BaseDialog
{
    // ── Model ─────────────────────────────────────────────────────────────────
    private sealed record TelemetryControl(
        string Category,
        string Name,
        string Description,
        string RegPath,
        string ValueName,
        int PrivateValue,
        int DefaultValue,
        RegistryValueKind Kind = RegistryValueKind.DWord
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
    private readonly Button _btnSelectAll = new() { Text = "Select All", Width = 90 };
    private readonly Button _btnApply = new() { Text = "Apply Selected", Width = 110 };
    private readonly Button _btnRestoreDefaults = new() { Text = "Restore Defaults", Width = 125 };
    private readonly Button _btnClose = new() { Text = "Close", Width = 80 };
    private readonly Label _statusLabel = new()
    {
        Dock = DockStyle.Bottom,
        Height = 22,
        TextAlign = ContentAlignment.MiddleLeft,
        Padding = new Padding(6, 0, 0, 0),
    };

    private readonly List<TelemetryControl> _controls = [];

    // ── Construction ──────────────────────────────────────────────────────────
    internal TelemetryDashboardDialog()
        : base("Telemetry & Data Collection Dashboard", new Size(820, 580), resizable: true)
    {
        BuildControls();
        BuildLayout();
        LoadCurrentState();

        _btnSelectAll.Click += (_, _) => { foreach (ListViewItem i in _list.Items) i.Checked = true; };
        _btnApply.Click += OnApply;
        _btnRestoreDefaults.Click += OnRestoreDefaults;
        _btnClose.Click += (_, _) => Close();
        _list.SelectedIndexChanged += OnSelectionChanged;

        if (!Elevation.IsAdmin())
            Controls.Add(CreateAdminBanner("⚠  Some telemetry settings require Administrator rights."));
    }

    // ── Telemetry Controls ────────────────────────────────────────────────────
    private void BuildControls()
    {
        const string Telemetry = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection";
        const string TelemetryHklm = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\DataCollection";
        const string ActivityFeed = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";
        const string LocationHklm = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LocationAndSensors";
        const string ErrorReporting = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting";
        const string CEIP = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient\Windows";
        const string FeedbackHkcu = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Siuf\Rules";
        const string AppDiag = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy";
        const string Search = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Search";

        _controls.AddRange([
            new TelemetryControl(
                "Diagnostic Data",
                "Diagnostic Data Level",
                "Sets the Windows diagnostic data level. 0 = Security (minimum, Enterprise only), "
                + "1 = Basic (required diagnostic data), 3 = Full (enhanced/optional). "
                + "RegiLattice sets this to 1 (Basic) to minimize data sent while keeping Windows Update working.",
                Telemetry, "AllowTelemetry", 1, 3
            ),
            new TelemetryControl(
                "Diagnostic Data",
                "Diagnostic Data (User Policy Override)",
                "Same as above but via the user-accessible policy path. Both need to be set for full effect.",
                TelemetryHklm, "AllowTelemetry", 1, 3
            ),
            new TelemetryControl(
                "Activity History",
                "Windows Activity History",
                "Disables Windows Activity History and Timeline — stops logging apps, files, and web pages "
                + "you viewed. Prevents this data from syncing to Microsoft cloud via Connected Experiences.",
                ActivityFeed, "EnableActivityFeed", 0, 1
            ),
            new TelemetryControl(
                "Activity History",
                "Upload Activity History to Microsoft",
                "Prevents Windows from uploading your activity history to Microsoft servers for Timeline sync.",
                ActivityFeed, "PublishUserActivities", 0, 1
            ),
            new TelemetryControl(
                "Location",
                "Location Services (System)",
                "Disables Windows location services at the system level via Group Policy. "
                + "Apps will not be able to request your location.",
                LocationHklm, "DisableLocation", 1, 0
            ),
            new TelemetryControl(
                "Error Reporting",
                "Windows Error Reporting",
                "Disables automatic submission of crash reports and application error data to Microsoft. "
                + "Error reports can contain stack traces, memory dumps, and process lists.",
                ErrorReporting, "Disabled", 1, 0
            ),
            new TelemetryControl(
                "Error Reporting",
                "Error Reporting Consent",
                "Sets Windows Error Reporting consent to 'Not send' (2). "
                + "This is the additional consent flag that controls whether reports are queued or sent.",
                ErrorReporting, "AutoApproveOSDumps", 0, 1
            ),
            new TelemetryControl(
                "CEIP",
                "Customer Experience Improvement Program",
                "Opts out of the Windows Customer Experience Improvement Program (CEIP/SQM). "
                + "CEIP collects usage statistics about how you use Windows and sends them to Microsoft.",
                CEIP, "CEIPEnable", 0, 1
            ),
            new TelemetryControl(
                "Feedback",
                "Feedback Frequency",
                "Disables Windows Feedback notifications (the 'How did that go?' dialogs). "
                + "0 = Never. Setting this prevents Microsoft from soliciting usage surveys.",
                FeedbackHkcu, "NumberOfSIUFInPeriod", 0, 1
            ),
            new TelemetryControl(
                "App Diagnostics",
                "App Diagnostic Access",
                "Denies apps' access to diagnostic information about other running apps. "
                + "This includes process lists, app usage times, and battery consumption data.",
                AppDiag, "LetAppsAccessDiagnosticInformation", 2, 0
            ),
            new TelemetryControl(
                "Search",
                "SafeSearch and Bing in Search",
                "Disables Bing integration and web search in the Windows Search bar. "
                + "Keystrokes typed in search will not be sent to Microsoft's Bing servers.",
                Search, "BingSearchEnabled", 0, 1
            ),
            new TelemetryControl(
                "Search",
                "Cortana Allowed",
                "Disables Cortana via the Windows Search policy key. "
                + "Cortana sends voice, search, and usage data to Microsoft.",
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowCortana", 0, 1
            ),
        ]);
    }

    // ── Layout ────────────────────────────────────────────────────────────────
    private void BuildLayout()
    {
        _list.Columns.AddRange([
            new ColumnHeader { Text = "Category", Width = 130 },
            new ColumnHeader { Text = "Setting", Width = 230 },
            new ColumnHeader { Text = "Status", Width = 110 },
            new ColumnHeader { Text = "When Applied", Width = 100 },
        ]);

        _btnPanel.Controls.AddRange([_btnSelectAll, _btnApply, _btnRestoreDefaults, _btnClose]);
        Controls.AddRange([_list, _descBox, _statusLabel, _btnPanel]);
    }

    // ── Load State ────────────────────────────────────────────────────────────
    private void LoadCurrentState()
    {
        _list.BeginUpdate();
        _list.Items.Clear();

        foreach (var ctrl in _controls)
        {
            bool isPrivate = ReadIsPrivacyValue(ctrl);
            var lvi = new ListViewItem(ctrl.Category) { Tag = ctrl, Checked = !isPrivate };
            lvi.SubItems.Add(ctrl.Name);
            lvi.SubItems.Add(isPrivate ? "✓ Private" : "Default");
            lvi.SubItems.Add(isPrivate ? "—" : "Click Apply");
            if (isPrivate)
                lvi.ForeColor = Color.ForestGreen;
            _list.Items.Add(lvi);
        }

        _list.EndUpdate();
        int alreadyPrivate = 0;
        foreach (ListViewItem lvi in _list.Items)
            if (lvi.ForeColor == Color.ForestGreen) alreadyPrivate++;
        _statusLabel.Text = $"{alreadyPrivate}/{_controls.Count} settings already set to private.";
    }

    private static bool ReadIsPrivacyValue(TelemetryControl ctrl)
    {
        try
        {
            bool isHkcu = ctrl.RegPath.StartsWith("HKEY_CURRENT_USER", StringComparison.OrdinalIgnoreCase);
            string subKey = isHkcu
                ? ctrl.RegPath.Substring("HKEY_CURRENT_USER\\".Length)
                : ctrl.RegPath.Substring("HKEY_LOCAL_MACHINE\\".Length);
            using var key = isHkcu
                ? Registry.CurrentUser.OpenSubKey(subKey)
                : Registry.LocalMachine.OpenSubKey(subKey);
            if (key is null) return false;
            var val = key.GetValue(ctrl.ValueName);
            return val is int i && i == ctrl.PrivateValue;
        }
        catch { return false; }
    }

    // ── Apply ─────────────────────────────────────────────────────────────────
    private void OnApply(object? sender, EventArgs e)
    {
        int applied = 0, failed = 0;

        foreach (ListViewItem lvi in _list.Items)
        {
            if (!lvi.Checked || lvi.Tag is not TelemetryControl ctrl)
                continue;

            try
            {
                bool isHkcu = ctrl.RegPath.StartsWith("HKEY_CURRENT_USER", StringComparison.OrdinalIgnoreCase);
                string subKey = isHkcu
                    ? ctrl.RegPath.Substring("HKEY_CURRENT_USER\\".Length)
                    : ctrl.RegPath.Substring("HKEY_LOCAL_MACHINE\\".Length);

                using var key = isHkcu
                    ? Registry.CurrentUser.CreateSubKey(subKey, writable: true)
                    : Registry.LocalMachine.CreateSubKey(subKey, writable: true);
                key?.SetValue(ctrl.ValueName, ctrl.PrivateValue, ctrl.Kind);
                lvi.SubItems[2].Text = "✓ Private";
                lvi.SubItems[3].Text = "—";
                lvi.ForeColor = Color.ForestGreen;
                applied++;
            }
            catch
            {
                lvi.SubItems[2].Text = "Error";
                lvi.ForeColor = Color.Red;
                failed++;
            }
        }

        _statusLabel.Text = $"✓ {applied} setting(s) set to private. {(failed > 0 ? $"{failed} failed (may need Admin)." : "")}";
    }

    private void OnRestoreDefaults(object? sender, EventArgs e)
    {
        int restored = 0;
        foreach (ListViewItem lvi in _list.Items)
        {
            if (lvi.Tag is not TelemetryControl ctrl)
                continue;
            try
            {
                bool isHkcu = ctrl.RegPath.StartsWith("HKEY_CURRENT_USER", StringComparison.OrdinalIgnoreCase);
                string subKey = isHkcu
                    ? ctrl.RegPath.Substring("HKEY_CURRENT_USER\\".Length)
                    : ctrl.RegPath.Substring("HKEY_LOCAL_MACHINE\\".Length);
                using var key = isHkcu
                    ? Registry.CurrentUser.OpenSubKey(subKey, writable: true)
                    : Registry.LocalMachine.OpenSubKey(subKey, writable: true);
                if (key != null)
                {
                    key.SetValue(ctrl.ValueName, ctrl.DefaultValue, ctrl.Kind);
                    lvi.SubItems[2].Text = "Default";
                    lvi.ForeColor = SystemColors.WindowText;
                    restored++;
                }
            }
            catch { /* ignore */ }
        }
        _statusLabel.Text = $"{restored} setting(s) restored to Windows defaults.";
    }

    private void OnSelectionChanged(object? sender, EventArgs e)
    {
        if (_list.SelectedItems.Count > 0 && _list.SelectedItems[0].Tag is TelemetryControl ctrl)
            _descBox.Text = ctrl.Description;
    }
}
