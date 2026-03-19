// RegiLattice.GUI — Forms/InstalledAppsDialog.cs
// Sprint 30: Installed-programs viewer with quick-uninstall trigger.

#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace RegiLattice.GUI.Forms;

/// <summary>
/// Lists installed programs sourced from the Windows Uninstall registry keys.
/// Allows the user to launch the native uninstaller of a selected item.
/// </summary>
[SupportedOSPlatform("windows")]
internal sealed class InstalledAppsDialog : BaseDialog
{
    // ── Registry locations for installed programs ─────────────────────────────
    private static readonly string[] UninstallKeys =
    [
        @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
        @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall",
    ];

    // ── Model ─────────────────────────────────────────────────────────────────
    private sealed record AppEntry(
        string Name,
        string Publisher,
        string Version,
        string InstallDate,
        string SizeDisplay,
        string? UninstallString,
        bool SystemComponent
    );

    // ── Controls ─────────────────────────────────────────────────────────────
    private readonly TextBox _searchBox = new() { Width = 250, PlaceholderText = "Search apps…" };
    private readonly ComboBox _scopeFilter = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 130 };
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
        Height = 42,
        Padding = new Padding(6),
    };
    private readonly FlowLayoutPanel _btnPanel = new()
    {
        Dock = DockStyle.Bottom,
        Height = 42,
        FlowDirection = FlowDirection.LeftToRight,
        WrapContents = false,
        Padding = new Padding(6, 6, 6, 4),
    };
    private readonly Button _btnUninstall = new()
    {
        Text = "Uninstall…",
        Width = 100,
        Enabled = false,
    };
    private readonly Button _btnRefresh = new() { Text = "Refresh", Width = 80 };
    private readonly Button _btnClose = new() { Text = "Close", Width = 80 };
    private readonly Label _countLabel = new()
    {
        Dock = DockStyle.Bottom,
        Height = 22,
        TextAlign = ContentAlignment.MiddleLeft,
        Padding = new Padding(6, 0, 0, 0),
    };

    private List<AppEntry> _allApps = [];
    private CancellationTokenSource _cts = new();

    internal InstalledAppsDialog()
        : base("Installed Applications", new Size(880, 560), resizable: true)
    {
        BuildControls();
    }

    // ── Construction ──────────────────────────────────────────────────────────
    private void BuildControls()
    {
        // Top filter bar
        var searchLabel = new Label
        {
            Text = "Search:",
            AutoSize = true,
            Margin = new Padding(0, 8, 4, 0),
        };
        var scopeLabel = new Label
        {
            Text = "Scope:",
            AutoSize = true,
            Margin = new Padding(8, 8, 4, 0),
        };
        _scopeFilter.Items.AddRange(["All", "HKLM (Machine)", "HKCU (User)"]);
        _scopeFilter.SelectedIndex = 0;
        _searchBox.TextChanged += (_, _) => FilterList();
        _scopeFilter.SelectedIndexChanged += (_, _) => FilterList();

        var flow = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false,
        };
        flow.Controls.AddRange(new Control[] { searchLabel, _searchBox, scopeLabel, _scopeFilter });
        _topPanel.Controls.Add(flow);

        // List columns
        _list.Columns.AddRange([
            new ColumnHeader { Text = "Application", Width = 260 },
            new ColumnHeader { Text = "Publisher", Width = 160 },
            new ColumnHeader { Text = "Version", Width = 100 },
            new ColumnHeader { Text = "Install Date", Width = 100 },
            new ColumnHeader { Text = "Size", Width = 90 },
        ]);
        _list.ColumnClick += OnColumnClick;
        _list.SelectedIndexChanged += OnSelectionChanged;

        _btnUninstall.Click += OnUninstall;
        _btnRefresh.Click += async (_, _) => await LoadAppsAsync();
        _btnClose.Click += (_, _) => Close();

        _btnPanel.Controls.AddRange(new Control[] { _btnUninstall, _btnRefresh, _btnClose });

        Controls.Add(_list);
        Controls.Add(_topPanel);
        Controls.Add(_countLabel);
        Controls.Add(_btnPanel);
    }

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        await LoadAppsAsync();
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _cts.Cancel();
        base.OnFormClosed(e);
    }

    // ── Data loading ──────────────────────────────────────────────────────────
    private async Task LoadAppsAsync()
    {
        _btnRefresh.Enabled = false;
        _btnUninstall.Enabled = false;
        _countLabel.Text = "Loading installed applications…";

        _cts.Cancel();
        _cts = new CancellationTokenSource();
        var token = _cts.Token;

        _allApps = await Task.Run(() => ScanInstalledApps(), token);

        if (token.IsCancellationRequested)
            return;

        FilterList();
        _countLabel.Text = $"{_allApps.Count} applications found.";
        _btnRefresh.Enabled = true;
    }

    private static List<AppEntry> ScanInstalledApps()
    {
        var result = new List<AppEntry>();

        foreach (var registryRoot in new[] { Registry.LocalMachine, Registry.CurrentUser })
        {
            foreach (var keyPath in UninstallKeys)
            {
                using var root = registryRoot.OpenSubKey(keyPath);
                if (root is null)
                    continue;

                foreach (var sub in root.GetSubKeyNames())
                {
                    using var appKey = root.OpenSubKey(sub);
                    if (appKey is null)
                        continue;

                    string? name = appKey.GetValue("DisplayName")?.ToString();
                    if (string.IsNullOrWhiteSpace(name))
                        continue;

                    // Skip system components and Windows updates
                    if (appKey.GetValue("SystemComponent") is int sc && sc == 1)
                        continue;
                    if (appKey.GetValue("ReleaseType")?.ToString() is "Security Update" or "Update")
                        continue;

                    string publisher = appKey.GetValue("Publisher")?.ToString() ?? "";
                    string version = appKey.GetValue("DisplayVersion")?.ToString() ?? "";
                    string installDate = appKey.GetValue("InstallDate")?.ToString() ?? "";
                    string? uninstall = appKey.GetValue("UninstallString")?.ToString();
                    long.TryParse(appKey.GetValue("EstimatedSize")?.ToString(), out long kbSize);
                    string sizeDisplay = kbSize > 0 ? FormatKb(kbSize) : "";

                    // Format date YYYYMMDD → YYYY-MM-DD
                    if (installDate.Length == 8 && long.TryParse(installDate, out _))
                        installDate = $"{installDate[..4]}-{installDate[4..6]}-{installDate[6..]}";

                    result.Add(new AppEntry(name, publisher, version, installDate, sizeDisplay, uninstall, false));
                }
            }
        }

        // De-duplicate by name (HKLM + HKCU might both list the same app)
        return result
            .GroupBy(a => a.Name, StringComparer.OrdinalIgnoreCase)
            .Select(g => g.First())
            .OrderBy(a => a.Name, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    // ── Filter ────────────────────────────────────────────────────────────────
    private int _sortColumn = 0;
    private bool _sortAsc = true;

    private void FilterList()
    {
        string search = _searchBox.Text.Trim();
        var filtered = _allApps.AsEnumerable();

        if (!string.IsNullOrEmpty(search))
            filtered = filtered.Where(a =>
                a.Name.Contains(search, StringComparison.OrdinalIgnoreCase)
                || a.Publisher.Contains(search, StringComparison.OrdinalIgnoreCase)
                || a.Version.Contains(search, StringComparison.OrdinalIgnoreCase)
            );

        var sorted = _sortColumn switch
        {
            1 => _sortAsc ? filtered.OrderBy(a => a.Publisher) : filtered.OrderByDescending(a => a.Publisher),
            2 => _sortAsc ? filtered.OrderBy(a => a.Version) : filtered.OrderByDescending(a => a.Version),
            3 => _sortAsc ? filtered.OrderBy(a => a.InstallDate) : filtered.OrderByDescending(a => a.InstallDate),
            _ => _sortAsc
                ? filtered.OrderBy(a => a.Name, StringComparer.OrdinalIgnoreCase)
                : filtered.OrderByDescending(a => a.Name, StringComparer.OrdinalIgnoreCase),
        };

        _list.BeginUpdate();
        _list.Items.Clear();
        foreach (var a in sorted)
        {
            var item = new ListViewItem(a.Name) { Tag = a };
            item.SubItems.Add(a.Publisher);
            item.SubItems.Add(a.Version);
            item.SubItems.Add(a.InstallDate);
            item.SubItems.Add(a.SizeDisplay);
            if (a.UninstallString is null)
                item.ForeColor = SystemColors.GrayText;
            _list.Items.Add(item);
        }
        _list.EndUpdate();

        _countLabel.Text = $"{_list.Items.Count} applications shown.";
    }

    private void OnColumnClick(object? sender, ColumnClickEventArgs e)
    {
        if (_sortColumn == e.Column)
            _sortAsc = !_sortAsc;
        else
        {
            _sortColumn = e.Column;
            _sortAsc = true;
        }
        FilterList();
    }

    // ── Uninstall ─────────────────────────────────────────────────────────────
    private void OnUninstall(object? sender, EventArgs e)
    {
        if (_list.SelectedItems.Count == 0)
            return;
        if (_list.SelectedItems[0].Tag is not AppEntry app)
            return;

        if (string.IsNullOrWhiteSpace(app.UninstallString))
        {
            MessageBox.Show("No uninstall command found for this application.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var confirm = MessageBox.Show(
            $"Launch the uninstaller for:\n\n{app.Name}\n\nThis will open the application's own uninstaller.",
            "Confirm Uninstall",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question
        );
        if (confirm != DialogResult.Yes)
            return;

        try
        {
            // UninstallString may be a full command line (e.g. "msiexec /x {GUID}")
            string cmd = app.UninstallString;
            if (cmd.StartsWith('"'))
            {
                int end = cmd.IndexOf('"', 1);
                string exe = cmd[1..end];
                string args = cmd[(end + 1)..].Trim();
                Process.Start(new ProcessStartInfo(exe, args) { UseShellExecute = true });
            }
            else
            {
                Process.Start(new ProcessStartInfo("cmd", $"/c {cmd}") { UseShellExecute = true });
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to start uninstaller:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void OnSelectionChanged(object? sender, EventArgs e)
    {
        bool hasUninstall = _list.SelectedItems.Count > 0 && _list.SelectedItems[0].Tag is AppEntry { UninstallString: not null };
        _btnUninstall.Enabled = hasUninstall;
    }

    // ── Formatting ────────────────────────────────────────────────────────────
    private static string FormatKb(long kbSize) =>
        kbSize switch
        {
            >= 1_048_576 => $"{kbSize / 1_048_576.0:F1} GB",
            >= 1_024 => $"{kbSize / 1_024.0:F1} MB",
            _ => $"{kbSize} KB",
        };
}
