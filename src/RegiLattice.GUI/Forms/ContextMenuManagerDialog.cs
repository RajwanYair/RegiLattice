// RegiLattice.GUI — Forms/ContextMenuManagerDialog.cs
// Sprint 30: Context-menu entry manager — view, enable, and disable shell context-menu extensions.

#nullable enable

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using RegiLattice.Core;
using RegiLattice.Core.Services;

namespace RegiLattice.GUI.Forms;

/// <summary>
/// Lets the user view and toggle shell context-menu extensions stored in the registry
/// under HKLM/HKCR \*\shellex\ContextMenuHandlers and similar paths.
/// </summary>
[SupportedOSPlatform("windows")]
internal sealed class ContextMenuManagerDialog : BaseDialog
{
    // ── Registry locations scanned for context-menu handlers ─────────────────
    private static readonly (string Path, string Group)[] RegistryLocations =
    [
        (@"HKEY_CLASSES_ROOT\*\shellex\ContextMenuHandlers", "Files (any)"),
        (@"HKEY_CLASSES_ROOT\Directory\shellex\ContextMenuHandlers", "Folders"),
        (@"HKEY_CLASSES_ROOT\Directory\Background\shellex\ContextMenuHandlers", "Desktop Background"),
        (@"HKEY_CLASSES_ROOT\Drive\shellex\ContextMenuHandlers", "Drives"),
        (@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\*\shellex\ContextMenuHandlers", "Files (HKLM)"),
    ];

    // ── Model ─────────────────────────────────────────────────────────────────
    private sealed record ContextEntry(string Name, string Clsid, string Group, bool Enabled, string RegistryPath);

    // ── Controls ─────────────────────────────────────────────────────────────
    private readonly TextBox _searchBox = new() { Width = 220, PlaceholderText = "Filter entries…" };
    private readonly ComboBox _groupFilter = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 160 };
    private readonly ListView _list = new()
    {
        Dock = DockStyle.Fill,
        FullRowSelect = true,
        MultiSelect = true,
        View = View.Details,
        GridLines = true,
    };
    private readonly Panel _topPanel = new()
    {
        Dock = DockStyle.Top,
        Height = 42,
        Padding = new Padding(6, 6, 6, 2),
    };
    private readonly FlowLayoutPanel _btnPanel = new()
    {
        Dock = DockStyle.Bottom,
        Height = 42,
        Padding = new Padding(6, 6, 6, 4),
        FlowDirection = FlowDirection.LeftToRight,
        WrapContents = false,
    };
    private readonly Button _btnEnable = new()
    {
        Text = "Enable",
        Width = 86,
        Enabled = false,
    };
    private readonly Button _btnDisable = new()
    {
        Text = "Disable",
        Width = 86,
        Enabled = false,
    };
    private readonly Button _btnRefresh = new() { Text = "Refresh", Width = 80 };
    private readonly Button _btnClose = new() { Text = "Close", Width = 80 };
    private readonly Label _statusLabel = new()
    {
        Dock = DockStyle.Bottom,
        Height = 22,
        TextAlign = ContentAlignment.MiddleLeft,
        Padding = new Padding(6, 0, 0, 0),
    };

    private List<ContextEntry> _allEntries = [];
    private CancellationTokenSource _cts = new();

    internal ContextMenuManagerDialog()
        : base("Context Menu Manager", new Size(780, 540), resizable: true)
    {
        BuildControls();
    }

    // ── Construction ──────────────────────────────────────────────────────────
    private void BuildControls()
    {
        if (!Elevation.IsAdmin())
            Controls.Add(CreateAdminBanner("Some entries require administrator privileges to modify."));

        // Top search/filter bar
        var searchLabel = new Label
        {
            Text = "Search:",
            AutoSize = true,
            Margin = new Padding(0, 8, 4, 0),
        };
        var filterLabel = new Label
        {
            Text = "Group:",
            AutoSize = true,
            Margin = new Padding(8, 8, 4, 0),
        };
        _groupFilter.Items.Add("All Groups");
        _searchBox.TextChanged += (_, _) => FilterList();
        _groupFilter.SelectedIndexChanged += (_, _) => FilterList();
        _groupFilter.SelectedIndex = 0;

        var flow = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false,
        };
        flow.Controls.AddRange(new Control[] { searchLabel, _searchBox, filterLabel, _groupFilter });
        _topPanel.Controls.Add(flow);

        // List columns
        _list.Columns.AddRange([
            new ColumnHeader { Text = "Name", Width = 180 },
            new ColumnHeader { Text = "CLSID", Width = 220 },
            new ColumnHeader { Text = "Group", Width = 150 },
            new ColumnHeader { Text = "Status", Width = 90 },
        ]);
        _list.SelectedIndexChanged += OnSelectionChanged;

        // Button panel
        _btnEnable.Click += async (_, _) => await SetSelectedStatusAsync(enable: true);
        _btnDisable.Click += async (_, _) => await SetSelectedStatusAsync(enable: false);
        _btnRefresh.Click += async (_, _) => await LoadEntriesAsync();
        _btnClose.Click += (_, _) => Close();

        _btnPanel.Controls.AddRange(new Control[] { _btnEnable, _btnDisable, _btnRefresh, _btnClose });

        Controls.Add(_list);
        Controls.Add(_topPanel);
        Controls.Add(_statusLabel);
        Controls.Add(_btnPanel);
    }

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        await LoadEntriesAsync();
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _cts.Cancel();
        base.OnFormClosed(e);
    }

    // ── Data loading ──────────────────────────────────────────────────────────
    private async Task LoadEntriesAsync()
    {
        _btnRefresh.Enabled = false;
        _statusLabel.Text = "Scanning registry…";

        _cts.Cancel();
        _cts = new CancellationTokenSource();
        var token = _cts.Token;

        _allEntries = await Task.Run(() => ScanContextMenuHandlers(), token);

        if (token.IsCancellationRequested)
            return;

        // Populate group filter
        var groups = _allEntries.Select(e => e.Group).Distinct().OrderBy(g => g).ToList();
        _groupFilter.Items.Clear();
        _groupFilter.Items.Add("All Groups");
        foreach (var g in groups)
            _groupFilter.Items.Add(g);
        _groupFilter.SelectedIndex = 0;

        FilterList();
        _statusLabel.Text = $"{_allEntries.Count} context-menu entries found.";
        _btnRefresh.Enabled = true;
    }

    private static List<ContextEntry> ScanContextMenuHandlers()
    {
        var result = new List<ContextEntry>();

        foreach (var (path, group) in RegistryLocations)
        {
            try
            {
                var (root, subKey) = SplitHive(path);
                using var key = root?.OpenSubKey(subKey);
                if (key is null)
                    continue;

                foreach (var name in key.GetSubKeyNames())
                {
                    using var sub = key.OpenSubKey(name);
                    string? clsid = sub?.GetValue(null)?.ToString() ?? name;
                    bool enabled = !clsid.StartsWith('-');
                    string cleanClsid = enabled ? clsid : clsid[1..];
                    result.Add(new ContextEntry(name, cleanClsid, group, enabled, $"{path}\\{name}"));
                }
            }
            catch
            { /* key may be inaccessible — skip */
            }
        }

        return result.OrderBy(e => e.Group).ThenBy(e => e.Name).ToList();
    }

    // ── Filter ────────────────────────────────────────────────────────────────
    private void FilterList()
    {
        string search = _searchBox.Text.Trim();
        string group = _groupFilter.SelectedItem?.ToString() ?? "All Groups";

        var filtered = _allEntries.AsEnumerable();
        if (!string.IsNullOrEmpty(search))
            filtered = filtered.Where(e =>
                e.Name.Contains(search, StringComparison.OrdinalIgnoreCase) || e.Clsid.Contains(search, StringComparison.OrdinalIgnoreCase)
            );
        if (group != "All Groups")
            filtered = filtered.Where(e => e.Group == group);

        _list.BeginUpdate();
        _list.Items.Clear();
        foreach (var e in filtered)
        {
            var item = new ListViewItem(e.Name) { Tag = e };
            item.SubItems.Add(e.Clsid);
            item.SubItems.Add(e.Group);
            item.SubItems.Add(e.Enabled ? "Enabled" : "Disabled");
            item.ForeColor = e.Enabled ? SystemColors.WindowText : SystemColors.GrayText;
            _list.Items.Add(item);
        }
        _list.EndUpdate();
    }

    // ── Enable / Disable ─────────────────────────────────────────────────────
    private async Task SetSelectedStatusAsync(bool enable)
    {
        var selected = _list.SelectedItems.Cast<ListViewItem>().Select(i => i.Tag as ContextEntry).Where(e => e is not null).ToList();

        if (selected.Count == 0)
            return;

        _btnEnable.Enabled = false;
        _btnDisable.Enabled = false;
        _statusLabel.Text = $"Updating {selected.Count} entry/entries…";

        int changed = 0;
        await Task.Run(() =>
        {
            foreach (var entry in selected)
            {
                try
                {
                    var (root, subKey) = SplitHive(entry!.RegistryPath);
                    using var key = root?.OpenSubKey(subKey, writable: true);
                    if (key is null)
                        continue;

                    // Enable: remove leading '-'; Disable: prepend '-'
                    string current = key.GetValue(null)?.ToString() ?? "";
                    string newVal = enable ? (current.StartsWith('-') ? current[1..] : current) : (current.StartsWith('-') ? current : "-" + current);
                    key.SetValue(null, newVal);
                    changed++;
                }
                catch
                { /* insufficient privileges or key missing */
                }
            }
        });

        await LoadEntriesAsync();
        _statusLabel.Text = $"{changed} entry/entries updated.";
    }

    private void OnSelectionChanged(object? sender, EventArgs e)
    {
        bool any = _list.SelectedItems.Count > 0;
        _btnEnable.Enabled = any;
        _btnDisable.Enabled = any;
    }

    /// <summary>Splits a full registry path like HKEY_CLASSES_ROOT\sub\key into (hive, subKey).</summary>
    private static (RegistryKey? Hive, string SubKey) SplitHive(string path)
    {
        int idx = path.IndexOf('\\');
        if (idx < 0)
            return (null, path);
        string hive = path[..idx].ToUpperInvariant();
        string sub = path[(idx + 1)..];
        RegistryKey? root = hive switch
        {
            "HKEY_CLASSES_ROOT" or "HKCR" => Registry.ClassesRoot,
            "HKEY_LOCAL_MACHINE" or "HKLM" => Registry.LocalMachine,
            "HKEY_CURRENT_USER" or "HKCU" => Registry.CurrentUser,
            "HKEY_USERS" or "HKU" => Registry.Users,
            _ => null,
        };
        return (root, sub);
    }
}
