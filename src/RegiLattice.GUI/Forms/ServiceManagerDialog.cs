#nullable enable

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using RegiLattice.Core;
using RegiLattice.Core.Services;

namespace RegiLattice.GUI.Forms;

/// <summary>
/// Dialog for listing, filtering, starting, stopping, and changing the
/// startup type of Windows services.
/// </summary>
internal sealed class ServiceManagerDialog : BaseDialog
{
    // ── Controls ─────────────────────────────────────────────────────────────
    private readonly TextBox _searchBox = new() { Width = 200, PlaceholderText = "Filter services…" };
    private readonly ListView _list = new()
    {
        Dock = DockStyle.Fill,
        FullRowSelect = true,
        MultiSelect = false,
        View = View.Details,
        GridLines = true,
    };
    private readonly Panel _topPanel = new()
    {
        Dock = DockStyle.Top,
        Height = 38,
        Padding = new Padding(6, 6, 6, 2),
    };
    private readonly Panel _btnPanel = new()
    {
        Dock = DockStyle.Bottom,
        Height = 40,
        Padding = new Padding(6, 6, 6, 4),
    };
    private readonly Button _btnStart = new()
    {
        Text = "Start",
        Width = 72,
        Enabled = false,
    };
    private readonly Button _btnStop = new()
    {
        Text = "Stop",
        Width = 72,
        Enabled = false,
    };
    private readonly Button _btnEnable = new()
    {
        Text = "Enable",
        Width = 72,
        Enabled = false,
    };
    private readonly Button _btnDisable = new()
    {
        Text = "Disable",
        Width = 72,
        Enabled = false,
    };
    private readonly Button _btnRefresh = new() { Text = "Refresh", Width = 72 };
    private readonly Button _btnClose = new() { Text = "Close", Width = 72 };
    private readonly Button _btnPresets = new() { Text = "Presets ▾", Width = 88 };
    private readonly ContextMenuStrip _presetMenu = new();
    private readonly Button _btnRestart = new()
    {
        Text = "Restart",
        Width = 72,
        Enabled = false,
    };
    private readonly Button _btnSetAuto = new()
    {
        Text = "Set Auto",
        Width = 72,
        Enabled = false,
    };
    private readonly RichTextBox _descBox = new()
    {
        Dock = DockStyle.Bottom,
        Height = 60,
        ReadOnly = true,
        BorderStyle = BorderStyle.None,
        BackColor = SystemColors.Control,
    };
    private readonly Label _statusLabel = new()
    {
        Dock = DockStyle.Bottom,
        Height = 22,
        TextAlign = ContentAlignment.MiddleLeft,
        Padding = new Padding(6, 0, 0, 0),
    };
    private readonly Panel _adminBanner = new()
    {
        Dock = DockStyle.Top,
        Height = 30,
        BackColor = Color.FromArgb(50, 150, 250),
    };
    private readonly Label _adminLabel = new()
    {
        Dock = DockStyle.Fill,
        ForeColor = Color.White,
        TextAlign = ContentAlignment.MiddleCenter,
    };

    private IReadOnlyList<ServiceEntry> _allEntries = Array.Empty<ServiceEntry>();
    private IReadOnlyList<ServiceEntry> _shown = Array.Empty<ServiceEntry>();
    private CancellationTokenSource _cts = new();
    private bool _busy;

    // ── Construction ─────────────────────────────────────────────────────────

    internal ServiceManagerDialog()
        : base("Service Manager", new Size(1000, 600), resizable: true)
    {
        MinimumSize = new Size(700, 440);

        BuildTopPanel();
        BuildButtons();
        BuildAdminBanner();

        Controls.Add(_list);
        Controls.Add(_descBox);
        Controls.Add(_statusLabel);
        Controls.Add(_btnPanel);
        Controls.Add(_topPanel);
        Controls.Add(_adminBanner);

        _list.SelectedIndexChanged += OnSelectionChanged;
        FormClosed += (_, _) =>
        {
            _cts.Cancel();
            _cts.Dispose();
        };
        Load += async (_, _) => await LoadServicesAsync();
    }

    // ── Layout ───────────────────────────────────────────────────────────────

    private void BuildTopPanel()
    {
        _list.Columns.Add("Service Name", 180);
        _list.Columns.Add("Status", 90);
        _list.Columns.Add("Start Type", 100);
        _list.Columns.Add("Display Name", 360);
        ListViewColumnSorter.AttachTo(_list);

        var label = new Label
        {
            Text = "Filter:",
            AutoSize = true,
            Location = new Point(6, 12),
        };
        _searchBox.Location = new Point(52, 8);
        _searchBox.TextChanged += (_, _) => ApplyFilter();

        _topPanel.Controls.Add(label);
        _topPanel.Controls.Add(_searchBox);
    }

    private void BuildButtons()
    {
        _btnStart.Click += async (_, _) => await ControlServiceAsync(start: true);
        _btnStop.Click += async (_, _) => await ControlServiceAsync(start: false);
        _btnEnable.Click += async (_, _) => await SetStartTypeAsync(ServiceStartMode.Manual);
        _btnDisable.Click += async (_, _) => await SetStartTypeAsync(ServiceStartMode.Disabled);
        _btnRefresh.Click += async (_, _) => await LoadServicesAsync();
        _btnClose.Click += (_, _) => Close();
        _btnRestart.Click += async (_, _) => await RestartServiceAsync();
        _btnSetAuto.Click += async (_, _) => await SetStartTypeAsync(ServiceStartMode.Automatic);
        BuildPresetMenu();
        _btnPresets.Click += (_, e) => _presetMenu.Show(_btnPresets, new Point(0, _btnPresets.Height));

        _btnPanel.Controls.AddRange([_btnStart, _btnStop, _btnEnable, _btnDisable, _btnRefresh, _btnRestart, _btnSetAuto, _btnPresets, _btnClose]);

        int x = 0;
        foreach (Button b in new[] { _btnStart, _btnStop, _btnEnable, _btnDisable, _btnRefresh, _btnRestart, _btnSetAuto, _btnPresets })
        {
            b.Location = new Point(x, 6);
            x += b.Width + 4;
        }
        _btnClose.Location = new Point(_btnPanel.Width - 78, 6);
        _btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    }

    private void BuildAdminBanner()
    {
        bool isAdmin = Elevation.IsAdmin();
        _adminLabel.Text = isAdmin
            ? "Running as Administrator — Start/Stop/Disable operations available"
            : "Not running as Administrator — service control requires elevation";
        if (!isAdmin)
            _adminBanner.BackColor = Color.FromArgb(180, 120, 0);

        _adminBanner.Controls.Add(_adminLabel);
    }

    // ── Data ─────────────────────────────────────────────────────────────────

    private async Task LoadServicesAsync()
    {
        SetBusy(true, "Loading services…");
        try
        {
            _allEntries = await Task.Run(ServiceManager.GetAllServices, _cts.Token);
            ApplyFilter();
            SetStatus($"{_allEntries.Count} services loaded.");
        }
        catch (OperationCanceledException) { }
        catch (Exception ex)
        {
            SetStatus($"Error: {ex.Message}");
        }
        finally
        {
            SetBusy(false);
        }
    }

    private void ApplyFilter()
    {
        string query = _searchBox.Text.Trim();
        _shown = string.IsNullOrEmpty(query)
            ? _allEntries
            :
            [
                .. System.Linq.Enumerable.Where(
                    _allEntries,
                    e =>
                        e.ServiceName.Contains(query, StringComparison.OrdinalIgnoreCase)
                        || e.DisplayName.Contains(query, StringComparison.OrdinalIgnoreCase)
                ),
            ];

        PopulateList();
    }

    private void PopulateList()
    {
        _list.BeginUpdate();
        _list.Items.Clear();

        foreach (ServiceEntry e in _shown)
        {
            var item = new ListViewItem(e.ServiceName) { Tag = e };
            item.SubItems.Add(StatusText(e.Status));
            item.SubItems.Add(StartTypeText(e.StartType));
            item.SubItems.Add(e.DisplayName);
            item.ForeColor = StatusColor(e.Status);
            _list.Items.Add(item);
        }

        _list.EndUpdate();
        UpdateButtons(null);
    }

    // ── Actions ──────────────────────────────────────────────────────────────

    private async Task ControlServiceAsync(bool start)
    {
        if (SelectedEntry() is not ServiceEntry entry)
            return;

        SetBusy(true, $"{(start ? "Starting" : "Stopping")} {entry.ServiceName}…");
        try
        {
            if (start)
                await ServiceManager.StartAsync(entry.ServiceName, _cts.Token);
            else
                await ServiceManager.StopAsync(entry.ServiceName, _cts.Token);

            SetStatus($"{(start ? "Started" : "Stopped")}: {entry.ServiceName}");
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Could not {(start ? "start" : "stop")} '{entry.ServiceName}':\n{ex.Message}",
                "Service Manager",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
        }
        finally
        {
            SetBusy(false);
            await LoadServicesAsync();
        }
    }

    private async Task SetStartTypeAsync(ServiceStartMode mode)
    {
        if (SelectedEntry() is not ServiceEntry entry)
            return;

        SetBusy(true, $"Setting start type for {entry.ServiceName}…");
        try
        {
            await ServiceManager.SetStartTypeAsync(entry.ServiceName, mode, ct: _cts.Token);
            SetStatus($"Start type set to {mode} for: {entry.ServiceName}");
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Could not change start type of '{entry.ServiceName}':\n{ex.Message}",
                "Service Manager",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
        }
        finally
        {
            SetBusy(false);
            await LoadServicesAsync();
        }
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    private ServiceEntry? SelectedEntry() => _list.SelectedItems.Count > 0 ? _list.SelectedItems[0].Tag as ServiceEntry : null;

    private void OnSelectionChanged(object? sender, EventArgs e)
    {
        ServiceEntry? entry = SelectedEntry();
        UpdateButtons(entry);
        if (entry is null)
        {
            _descBox.Text = string.Empty;
            return;
        }
        try
        {
            using var sc = new ServiceController(entry.ServiceName);
            var dependsOn = sc.ServicesDependedOn.Select(s => s.ServiceName).ToArray();
            var dependents = sc.DependentServices.Select(s => s.ServiceName).ToArray();
            var sb = new System.Text.StringBuilder();
            if (!string.IsNullOrEmpty(entry.Description))
                sb.AppendLine(entry.Description);
            if (dependsOn.Length > 0)
                sb.AppendLine($"Depends on: {string.Join(", ", dependsOn)}");
            if (dependents.Length > 0)
                sb.AppendLine($"Required by: {string.Join(", ", dependents)}");
            _descBox.Text = sb.ToString().TrimEnd();
        }
        catch
        {
            _descBox.Text = entry.Description;
        }
    }

    private void UpdateButtons(ServiceEntry? entry)
    {
        bool admin = Elevation.IsAdmin();
        _btnStart.Enabled = !_busy && admin && entry is { Status: ServiceControllerStatus.Stopped };
        _btnStop.Enabled = !_busy && admin && entry is { Status: ServiceControllerStatus.Running, CanStop: true };
        _btnEnable.Enabled = !_busy && admin && entry is { StartType: ServiceStartMode.Disabled };
        _btnDisable.Enabled = !_busy && admin && entry?.StartType is ServiceStartMode.Automatic or ServiceStartMode.Manual;
        _btnRestart.Enabled = !_busy && admin && entry is { Status: ServiceControllerStatus.Running, CanStop: true };
        _btnSetAuto.Enabled = !_busy && admin && entry?.StartType is ServiceStartMode.Manual or ServiceStartMode.Disabled;
    }

    private void SetBusy(bool busy, string? message = null)
    {
        _busy = busy;
        if (message is not null)
            SetStatus(message);
        foreach (Button b in new[] { _btnStart, _btnStop, _btnEnable, _btnDisable, _btnRefresh })
            b.Enabled = !busy;
        if (!busy)
            UpdateButtons(SelectedEntry());
    }

    private void SetStatus(string text) => _statusLabel.Text = text;

    private static string StatusText(ServiceControllerStatus s) =>
        s switch
        {
            ServiceControllerStatus.Running => "Running",
            ServiceControllerStatus.Stopped => "Stopped",
            ServiceControllerStatus.Paused => "Paused",
            ServiceControllerStatus.StartPending => "Starting…",
            ServiceControllerStatus.StopPending => "Stopping…",
            ServiceControllerStatus.PausePending => "Pausing…",
            ServiceControllerStatus.ContinuePending => "Resuming…",
            _ => s.ToString(),
        };

    private static string StartTypeText(ServiceStartMode m) =>
        m switch
        {
            ServiceStartMode.Automatic => "Automatic",
            ServiceStartMode.Manual => "Manual",
            ServiceStartMode.Disabled => "Disabled",
            ServiceStartMode.Boot => "Boot",
            ServiceStartMode.System => "System",
            _ => m.ToString(),
        };

    private async Task RestartServiceAsync()
    {
        if (SelectedEntry() is not ServiceEntry entry)
            return;
        SetBusy(true, $"Restarting {entry.ServiceName}…");
        try
        {
            await ServiceManager.StopAsync(entry.ServiceName, _cts.Token);
            await ServiceManager.StartAsync(entry.ServiceName, _cts.Token);
            SetStatus($"Restarted: {entry.ServiceName}");
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Could not restart '{entry.ServiceName}':\n{ex.Message}",
                "Service Manager",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
        }
        finally
        {
            SetBusy(false);
            await LoadServicesAsync();
        }
    }

    private static Color StatusColor(ServiceControllerStatus s) =>
        s switch
        {
            ServiceControllerStatus.Running => Color.FromArgb(0, 150, 0),
            ServiceControllerStatus.Stopped => Color.Gray,
            ServiceControllerStatus.Paused => Color.DarkOrange,
            _ => SystemColors.WindowText,
        };

    // ── Preset profiles ───────────────────────────────────────────────────────

    private static readonly Dictionary<string, (string Label, string[] Disable, string[] Enable)> s_servicePresets = new()
    {
        ["Gaming"] = (
            "Gaming — stop telemetry & indexing for max performance",
            ["DiagTrack", "dmwappushservice", "WSearch", "SysMain", "PcaSvc"],
            []
        ),
        ["Privacy"] = (
            "Privacy — stop data-collection services",
            ["DiagTrack", "dmwappushservice", "wisvc", "WerSvc", "PcaSvc", "MapsBroker"],
            []
        ),
        ["Minimal"] = (
            "Minimal — stop non-essential background services",
            ["DiagTrack", "dmwappushservice", "WSearch", "SysMain", "PcaSvc", "WerSvc", "wisvc", "Fax", "RetailDemo"],
            []
        ),
        ["Restore Defaults"] = (
            "Restore Defaults — re-enable common services",
            [],
            ["DiagTrack", "WSearch", "SysMain", "PcaSvc", "WerSvc"]
        ),
    };

    private void BuildPresetMenu()
    {
        foreach (var (name, (label, _, _)) in s_servicePresets)
        {
            var item = _presetMenu.Items.Add($"{name}: {label}");
            string captured = name;
            item.Click += async (_, _) => await ApplyServicePresetAsync(captured);
        }
    }

    private async Task ApplyServicePresetAsync(string presetName)
    {
        if (!s_servicePresets.TryGetValue(presetName, out var preset))
            return;
        if (!Elevation.IsAdmin())
        {
            MessageBox.Show("Administrator rights required to apply service presets.", "Service Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        var result = MessageBox.Show(
            $"Apply '{presetName}' preset?\n\nDisable: {(preset.Disable.Length > 0 ? string.Join(", ", preset.Disable) : "none")}\nEnable: {(preset.Enable.Length > 0 ? string.Join(", ", preset.Enable) : "none")}",
            "Service Preset", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (result != DialogResult.Yes)
            return;

        SetBusy(true, $"Applying '{presetName}' preset…");
        int changed = 0;
        try
        {
            await Task.Run(() =>
            {
                foreach (string svc in preset.Disable)
                {
                    try
                    {
                        ServiceManager.SetStartTypeAsync(svc, ServiceStartMode.Disabled).GetAwaiter().GetResult();
                        changed++;
                    }
                    catch { /* service may not exist */ }
                }
                foreach (string svc in preset.Enable)
                {
                    try
                    {
                        ServiceManager.SetStartTypeAsync(svc, ServiceStartMode.Manual).GetAwaiter().GetResult();
                        changed++;
                    }
                    catch { }
                }
            }, _cts.Token);
            SetStatus($"Preset '{presetName}' applied — {changed} service(s) changed.");
        }
        catch (Exception ex)
        {
            SetStatus($"Preset failed: {ex.Message}");
        }
        finally
        {
            SetBusy(false);
            await LoadServicesAsync();
        }
    }
}
