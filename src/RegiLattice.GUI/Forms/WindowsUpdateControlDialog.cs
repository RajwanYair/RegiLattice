// RegiLattice.GUI — Forms/WindowsUpdateControlDialog.cs
// Windows Update pause/resume controls and update settings management.
#nullable enable

using Microsoft.Win32;
using System.Diagnostics;

namespace RegiLattice.GUI.Forms;

/// <summary>
/// Windows Update Control dialog.
/// Pause/resume Feature and Quality updates via registry (no Group Policy required).
/// Also triggers manual update check and opens Windows Update settings.
/// </summary>
internal sealed class WindowsUpdateControlDialog : BaseDialog
{
    private const string WuSettingsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\WindowsUpdate\UX\Settings";
    private const string WuPolicyKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate";
    private const string WuAuKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU";

    private readonly Label _lblFeatureStatus = new() { AutoSize = true };
    private readonly Label _lblQualityStatus = new() { AutoSize = true };
    private readonly ComboBox _cboPauseDays = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 140 };
    private readonly Button _btnPauseFeature = new() { Text = "Pause Feature Updates", Width = 175, Height = 28 };
    private readonly Button _btnPauseQuality = new() { Text = "Pause Quality Updates", Width = 175, Height = 28 };
    private readonly Button _btnResumeAll = new() { Text = "Resume All Updates", Width = 160, Height = 28 };
    private readonly Button _btnCheckNow = new() { Text = "Check for Updates Now", Width = 175, Height = 28 };
    private readonly Button _btnOpenSettings = new() { Text = "Open WU Settings", Width = 155, Height = 28 };
    private readonly Button _btnRefresh = new() { Text = "⟳ Refresh", Width = 90, Height = 28 };
    private readonly Button _btnClose = new() { Text = "Close", Width = 75, Height = 28, DialogResult = DialogResult.Cancel };
    private readonly Label _lblInfo = new() { Dock = DockStyle.Top, Height = 28, TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(8, 0, 0, 0) };

    public WindowsUpdateControlDialog()
        : base("Windows Update Control", new Size(620, 480), resizable: false)
    {
        EnableStandaloneMode();
        BuildLayout();
        LoadStatus();
        WireEvents();
    }

    private void BuildLayout()
    {
        var outer = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 4,
            ColumnCount = 1,
            Padding = new Padding(14, 10, 14, 8),
        };

        // Pause duration selector
        _cboPauseDays.Items.AddRange(new object[] { "7 days", "14 days", "21 days", "28 days", "35 days" });
        _cboPauseDays.SelectedIndex = 0;

        // Status group
        var grpStatus = new GroupBox { Text = "Current Update Status", Dock = DockStyle.Fill, Height = 100 };
        var statusLayout = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 2, ColumnCount = 2, Padding = new Padding(8, 6, 0, 0) };
        statusLayout.Controls.Add(new Label { Text = "Feature Updates:", AutoSize = true }, 0, 0);
        statusLayout.Controls.Add(_lblFeatureStatus, 1, 0);
        statusLayout.Controls.Add(new Label { Text = "Quality Updates:", AutoSize = true }, 0, 1);
        statusLayout.Controls.Add(_lblQualityStatus, 1, 1);
        grpStatus.Controls.Add(statusLayout);

        // Controls group
        var grpControl = new GroupBox { Text = "Pause / Resume", Dock = DockStyle.Fill, Height = 160 };
        var durRow = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, AutoSize = true, WrapContents = false, Padding = new Padding(8, 8, 0, 0) };
        durRow.Controls.Add(new Label { Text = "Pause for:", AutoSize = true, Margin = new Padding(0, 4, 8, 0) });
        durRow.Controls.Add(_cboPauseDays);
        var btnRow = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, AutoSize = true, WrapContents = false, Padding = new Padding(8, 4, 0, 0) };
        btnRow.Controls.AddRange(new Control[] { _btnPauseFeature, _btnPauseQuality, _btnResumeAll });
        var controlContent = new FlowLayoutPanel { FlowDirection = FlowDirection.TopDown, Dock = DockStyle.Fill, WrapContents = false };
        controlContent.Controls.AddRange(new Control[] { durRow, btnRow });
        grpControl.Controls.Add(controlContent);

        // Quick actions group
        var grpActions = new GroupBox { Text = "Quick Actions", Dock = DockStyle.Fill, Height = 80 };
        var actRow = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, Dock = DockStyle.Fill, Padding = new Padding(8, 8, 0, 0), WrapContents = false };
        actRow.Controls.AddRange(new Control[] { _btnCheckNow, _btnOpenSettings, _btnRefresh, _btnClose });
        grpActions.Controls.Add(actRow);

        outer.Controls.Add(_lblInfo, 0, 0);
        outer.Controls.Add(grpStatus, 0, 1);
        outer.Controls.Add(grpControl, 0, 2);
        outer.Controls.Add(grpActions, 0, 3);
        Controls.Add(outer);
    }

    private void WireEvents()
    {
        _btnPauseFeature.Click += (_, _) => PauseFeatureUpdates();
        _btnPauseQuality.Click += (_, _) => PauseQualityUpdates();
        _btnResumeAll.Click += (_, _) => ResumeAllUpdates();
        _btnCheckNow.Click += (_, _) => CheckNow();
        _btnOpenSettings.Click += (_, _) => Process.Start(new ProcessStartInfo("ms-settings:windowsupdate") { UseShellExecute = true });
        _btnRefresh.Click += (_, _) => LoadStatus();
        _btnClose.Click += (_, _) => Close();
    }

    private void LoadStatus()
    {
        DateTime? featureEnd = ReadPauseDate("PauseFeatureUpdatesEndTime");
        DateTime? qualityEnd = ReadPauseDate("PauseQualityUpdatesEndTime");
        DateTime now = DateTime.Now;

        if (featureEnd.HasValue && featureEnd.Value > now)
        {
            _lblFeatureStatus.Text = $"⏸ Paused until {featureEnd:d MMM yyyy}";
            _lblFeatureStatus.ForeColor = Color.FromArgb(180, 140, 0);
        }
        else
        {
            _lblFeatureStatus.Text = "✓ Active (not paused)";
            _lblFeatureStatus.ForeColor = Color.FromArgb(0, 140, 0);
        }

        if (qualityEnd.HasValue && qualityEnd.Value > now)
        {
            _lblQualityStatus.Text = $"⏸ Paused until {qualityEnd:d MMM yyyy}";
            _lblQualityStatus.ForeColor = Color.FromArgb(180, 140, 0);
        }
        else
        {
            _lblQualityStatus.Text = "✓ Active (not paused)";
            _lblQualityStatus.ForeColor = Color.FromArgb(0, 140, 0);
        }

        _lblInfo.Text = "Changes require administrator privileges.";
    }

    private static DateTime? ReadPauseDate(string valueName)
    {
        string? val = Registry.GetValue(WuSettingsKey, valueName, null)?.ToString();
        if (string.IsNullOrEmpty(val)) return null;
        return DateTime.TryParse(val, out DateTime dt) ? dt : (DateTime?)null;
    }

    private int PauseDays() => (_cboPauseDays.SelectedIndex + 1) * 7;

    private void PauseFeatureUpdates()
    {
        try
        {
            string endDate = DateTime.Now.AddDays(PauseDays()).ToString("o");
            Registry.SetValue(WuSettingsKey, "PauseFeatureUpdatesStartTime", DateTime.Now.ToString("o"), RegistryValueKind.String);
            Registry.SetValue(WuSettingsKey, "PauseFeatureUpdatesEndTime", endDate, RegistryValueKind.String);
            _lblInfo.Text = $"✓ Feature Updates paused for {PauseDays()} days.";
            LoadStatus();
        }
        catch (UnauthorizedAccessException)
        {
            _lblInfo.Text = "✗ Access denied. Run as administrator.";
        }
    }

    private void PauseQualityUpdates()
    {
        try
        {
            string endDate = DateTime.Now.AddDays(PauseDays()).ToString("o");
            Registry.SetValue(WuSettingsKey, "PauseQualityUpdatesStartTime", DateTime.Now.ToString("o"), RegistryValueKind.String);
            Registry.SetValue(WuSettingsKey, "PauseQualityUpdatesEndTime", endDate, RegistryValueKind.String);
            _lblInfo.Text = $"✓ Quality Updates paused for {PauseDays()} days.";
            LoadStatus();
        }
        catch (UnauthorizedAccessException)
        {
            _lblInfo.Text = "✗ Access denied. Run as administrator.";
        }
    }

    private void ResumeAllUpdates()
    {
        try
        {
            foreach (string v in new[] { "PauseFeatureUpdatesStartTime", "PauseFeatureUpdatesEndTime", "PauseQualityUpdatesStartTime", "PauseQualityUpdatesEndTime" })
            {
                Registry.SetValue(WuSettingsKey, v, "", RegistryValueKind.String);
            }
            _lblInfo.Text = "✓ All updates resumed.";
            LoadStatus();
        }
        catch (UnauthorizedAccessException)
        {
            _lblInfo.Text = "✗ Access denied. Run as administrator.";
        }
    }

    private void CheckNow()
    {
        try
        {
            Process.Start(new ProcessStartInfo("usoclient", "StartScan") { CreateNoWindow = true, UseShellExecute = false });
            _lblInfo.Text = "✓ Update scan triggered. Check Windows Update for results.";
        }
        catch
        {
            Process.Start(new ProcessStartInfo("ms-settings:windowsupdate") { UseShellExecute = true });
        }
    }
}
