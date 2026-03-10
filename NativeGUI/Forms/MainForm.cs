using RegiLattice.Native.Models;

namespace RegiLattice.Native.Forms;

/// <summary>
/// RegiLattice Native GUI — main window.
///
/// Layout (top -> bottom):
///   ToolStrip  — Apply / Remove / Refresh / Filter / Profile / Search / Force
///   SplitContainer
///     Left  — TreeView of categories
///     Right — ListView of tweaks in selected category
///   StatusStrip — counters + progress label
/// </summary>
public partial class MainForm : Form
{
    // ── Dark-theme palette (Catppuccin Mocha) ──────────────────────────────
    private static readonly Color ThemeBg       = Color.FromArgb(30, 30, 46);
    private static readonly Color ThemeSurface  = Color.FromArgb(49, 50, 68);
    private static readonly Color ThemeFg       = Color.FromArgb(205, 214, 244);
    private static readonly Color ThemeAccent   = Color.FromArgb(137, 180, 250);
    private static readonly Color ThemeGreen    = Color.FromArgb(166, 227, 161);
    private static readonly Color ThemeRed      = Color.FromArgb(243, 139, 168);
    private static readonly Color ThemeYellow   = Color.FromArgb(249, 226, 175);
    private static readonly Color ThemeOverlay  = Color.FromArgb(69, 71, 90);

    // ── State ──────────────────────────────────────────────────────────────
    private PythonBridge?              _bridge;
    private IReadOnlyList<TweakInfo>   _allTweaks   = [];
    private CancellationTokenSource    _cts         = new();

    // Group tweaks by category for O(1) lookups
    private Dictionary<string, List<TweakInfo>> _tweaksByCategory = new(StringComparer.OrdinalIgnoreCase);

    // ── Profiles ───────────────────────────────────────────────────────────
    private static readonly Dictionary<string, HashSet<string>> ProfileCategories = new(StringComparer.OrdinalIgnoreCase)
    {
        ["business"] = ["Accessibility", "AI / Copilot", "Backup & Recovery", "Boot", "Chrome", "Clipboard & Drag-Drop",
            "Cloud Storage", "Communication", "Context Menu", "Cortana & Search", "DNS & Networking Advanced", "Edge",
            "Explorer", "File System", "Fonts", "Indexing & Search", "Input", "LibreOffice", "Lock Screen & Login",
            "M365 Copilot", "Maintenance", "Microsoft Store", "Network", "Night Light & Display", "Notifications",
            "Office", "OneDrive", "Package Management", "Performance", "Power", "Printing", "Privacy",
            "Remote Desktop", "Scheduled Tasks", "Security", "Shell", "Snap & Multitasking", "Startup", "Storage", "Windows Update"],
        ["gaming"] = ["Audio", "Boot", "Cortana & Search", "Crash & Diagnostics", "Display", "Explorer",
            "File System", "GPU / Graphics", "Gaming", "Indexing & Search", "Maintenance", "Multimedia",
            "Network", "Night Light & Display", "Notifications", "Performance", "Phone Link", "Power",
            "Privacy", "Scheduled Tasks", "Security", "Services", "Shell", "Snap & Multitasking",
            "Startup", "Storage", "System", "Taskbar", "Telemetry Advanced", "Widgets & News", "Windows Update"],
        ["privacy"] = ["AI / Copilot", "Chrome", "Cloud Storage", "Communication", "Cortana & Search",
            "Crash & Diagnostics", "Edge", "Explorer", "Firefox", "Indexing & Search", "Lock Screen & Login",
            "M365 Copilot", "Microsoft Store", "Notifications", "Office", "OneDrive", "Phone Link",
            "Privacy", "Scheduled Tasks", "Screensaver & Lock", "Security", "Services", "Shell",
            "Snap & Multitasking", "Startup", "Taskbar", "Telemetry Advanced", "Touch & Pen",
            "Voice Access & Speech", "Widgets & News", "Windows Update"],
        ["minimal"] = ["Boot", "Context Menu", "Cortana & Search", "Explorer", "File System",
            "Indexing & Search", "Maintenance", "Microsoft Store", "Network", "Notifications",
            "Performance", "Phone Link", "Power", "Privacy", "Scheduled Tasks", "Services",
            "Shell", "Snap & Multitasking", "Startup", "Storage", "Taskbar", "Windows Update"],
        ["server"] = ["Boot", "Cortana & Search", "Crash & Diagnostics", "DNS & Networking Advanced",
            "Display", "Explorer", "File System", "Indexing & Search", "Lock Screen & Login",
            "Maintenance", "Microsoft Store", "Network", "Night Light & Display", "Notifications",
            "Performance", "Phone Link", "Power", "Privacy", "Remote Desktop", "Scheduled Tasks",
            "Screensaver & Lock", "Security", "Services", "Shell", "Startup", "Storage",
            "System", "Windows Update"],
    };

    // ── Construction ───────────────────────────────────────────────────────
    public MainForm()
    {
        InitializeComponent();
        Text = "RegiLattice -- Native GUI";
        Icon = SystemIcons.Application;
        ApplyDarkTheme();
    }

    // ── Lifecycle ──────────────────────────────────────────────────────────
    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        await InitialiseBridgeAsync();
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        _cts.Cancel();
        _bridge?.Dispose();
        base.OnFormClosing(e);
    }

    // ── Dark theme ─────────────────────────────────────────────────────────
    private void ApplyDarkTheme()
    {
        BackColor = ThemeBg;
        ForeColor = ThemeFg;

        _toolStrip.BackColor = ThemeSurface;
        _toolStrip.ForeColor = ThemeFg;
        _toolStrip.Renderer  = new DarkToolStripRenderer();

        _treeView.BackColor = ThemeBg;
        _treeView.ForeColor = ThemeFg;

        _listView.BackColor = ThemeSurface;
        _listView.ForeColor = ThemeFg;
        _listView.OwnerDraw = true;
        _listView.DrawColumnHeader += OnDrawColumnHeader;
        _listView.DrawSubItem += OnDrawSubItem;

        _statusStrip.BackColor = ThemeOverlay;
        _statusStrip.ForeColor = ThemeFg;

        _searchBox.BackColor = ThemeOverlay;
        _searchBox.ForeColor = ThemeFg;
    }

    // ── Owner-draw for dark ListView ───────────────────────────────────────
    private void OnDrawColumnHeader(object? sender, DrawListViewColumnHeaderEventArgs e)
    {
        using var bg = new SolidBrush(ThemeOverlay);
        e.Graphics.FillRectangle(bg, e.Bounds);
        TextRenderer.DrawText(e.Graphics, e.Header?.Text ?? "", Font, e.Bounds, ThemeAccent,
            TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
    }

    private void OnDrawSubItem(object? sender, DrawListViewSubItemEventArgs e)
    {
        if (e.Item is null) return;
        Color bg = e.ItemIndex % 2 == 0 ? ThemeSurface : ThemeBg;
        if (e.Item.Selected) bg = ThemeOverlay;

        using var brush = new SolidBrush(bg);
        e.Graphics.FillRectangle(brush, e.Bounds);

        // Status column (index 1) — coloured text
        Color fg = ThemeFg;
        if (e.ColumnIndex == 1 && e.Item.Tag is TweakInfo tw)
        {
            fg = tw.Status switch
            {
                "applied"     => ThemeGreen,
                "not_applied" => ThemeRed,
                _             => ThemeYellow,
            };
        }

        TextRenderer.DrawText(e.Graphics, e.SubItem?.Text ?? "", Font, e.Bounds, fg,
            TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
    }

    // ── Initialisation ─────────────────────────────────────────────────────
    private async Task InitialiseBridgeAsync()
    {
        SetStatus("Locating Python...");
        try
        {
            string pythonPath = PythonBridge.FindPython();
            _bridge = new PythonBridge(pythonPath);
            SetStatus($"Python: {pythonPath}");
            await RefreshTweaksAsync();
        }
        catch (InvalidOperationException ex)
        {
            MessageBox.Show(
                ex.Message,
                "Python not found",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            SetStatus("Error: Python not found.");
        }
    }

    // ── Actions ────────────────────────────────────────────────────────────
    private async Task RefreshTweaksAsync()
    {
        if (_bridge is null) return;
        SetBusy(true, "Loading tweaks...");
        try
        {
            _allTweaks = await _bridge.LoadTweaksAsync(_cts.Token);
            RebuildCategoryIndex();
            PopulateTree();
            UpdateCounters();
            SetStatus($"Loaded {_allTweaks.Count} tweaks.");
        }
        catch (OperationCanceledException) { }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to load tweaks:\n{ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            SetStatus("Error loading tweaks.");
        }
        finally { SetBusy(false); }
    }

    private async Task ApplySelectedAsync()
    {
        if (_bridge is null) return;
        var selected = GetCheckedTweaks();
        if (selected.Count == 0) return;

        SetBusy(true, $"Applying {selected.Count} tweak(s)...");
        int success = 0;
        foreach (var tweak in selected)
        {
            bool force = _forceCheck.Checked;
            var (exitCode, err) = await _bridge.ApplyTweakAsync(tweak.Id, _cts.Token, force);
            if (exitCode == 0)
            {
                tweak.Status = "applied";
                success++;
            }
            else
            {
                SetStatus($"Error applying {tweak.Id}: {err.Trim()}");
            }
        }
        RebuildCategoryIndex();
        PopulateTree();
        RefreshListView();
        UpdateCounters();
        SetBusy(false, $"Applied {success}/{selected.Count} tweak(s).");
    }

    private async Task RemoveSelectedAsync()
    {
        if (_bridge is null) return;
        var selected = GetCheckedTweaks();
        if (selected.Count == 0) return;

        SetBusy(true, $"Removing {selected.Count} tweak(s)...");
        int success = 0;
        foreach (var tweak in selected)
        {
            bool force = _forceCheck.Checked;
            var (exitCode, err) = await _bridge.RemoveTweakAsync(tweak.Id, _cts.Token, force);
            if (exitCode == 0)
            {
                tweak.Status = "not_applied";
                success++;
            }
            else
            {
                SetStatus($"Error removing {tweak.Id}: {err.Trim()}");
            }
        }
        RebuildCategoryIndex();
        PopulateTree();
        RefreshListView();
        UpdateCounters();
        SetBusy(false, $"Removed {success}/{selected.Count} tweak(s).");
    }

    // ── Category index ─────────────────────────────────────────────────────
    private void RebuildCategoryIndex()
    {
        _tweaksByCategory.Clear();
        foreach (var tw in _allTweaks)
        {
            if (!_tweaksByCategory.TryGetValue(tw.Category, out var list))
            {
                list = [];
                _tweaksByCategory[tw.Category] = list;
            }
            list.Add(tw);
        }
    }

    // ── Tree / List population ─────────────────────────────────────────────
    private void PopulateTree()
    {
        string? previousCat = _treeView.SelectedNode?.Tag as string;

        _treeView.BeginUpdate();
        _treeView.Nodes.Clear();

        // Optionally filter by profile
        string profile = _profileCombo.SelectedItem?.ToString() ?? "(None)";
        HashSet<string>? profileCats = profile != "(None)" && ProfileCategories.TryGetValue(profile, out var pc) ? pc : null;

        TreeNode? selectNode = null;
        foreach (var kvp in _tweaksByCategory.OrderBy(k => k.Key))
        {
            if (profileCats is not null && !profileCats.Contains(kvp.Key))
                continue;

            int count   = kvp.Value.Count;
            int applied = kvp.Value.Count(t => t.IsApplied);
            var node    = new TreeNode($"{kvp.Key}  ({applied}/{count})") { Tag = kvp.Key };
            node.ForeColor = ThemeFg;
            _treeView.Nodes.Add(node);
            if (kvp.Key == previousCat) selectNode = node;
        }
        _treeView.EndUpdate();

        if (selectNode is not null)
        {
            _treeView.SelectedNode = selectNode;
            PopulateList((string)selectNode.Tag!);
        }
        else if (_treeView.Nodes.Count > 0)
        {
            _treeView.SelectedNode = _treeView.Nodes[0];
            PopulateList((string)_treeView.Nodes[0].Tag!);
        }
    }

    private void PopulateList(string category)
    {
        string filter = _filterCombo.SelectedItem?.ToString() ?? "All";
        string search = _searchBox.Text.Trim();

        _listView.BeginUpdate();
        _listView.Items.Clear();

        if (!_tweaksByCategory.TryGetValue(category, out var tweaks))
        {
            _listView.EndUpdate();
            return;
        }

        IEnumerable<TweakInfo> filtered = tweaks;
        if (filter != "All")
            filtered = filtered.Where(t => string.Equals(t.Status, filter, StringComparison.OrdinalIgnoreCase));

        if (search.Length > 0)
            filtered = filtered.Where(t =>
                t.Label.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                t.Id.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                t.Description.Contains(search, StringComparison.OrdinalIgnoreCase));

        foreach (var tw in filtered)
        {
            var item = new ListViewItem(tw.Label) { Tag = tw, UseItemStyleForSubItems = false };
            Color statusColor = tw.Status switch
            {
                "applied"     => ThemeGreen,
                "not_applied" => ThemeRed,
                _             => ThemeYellow,
            };
            item.SubItems.Add(new ListViewItem.ListViewSubItem(item, tw.Status)      { ForeColor = statusColor });
            item.SubItems.Add(new ListViewItem.ListViewSubItem(item, tw.ScopeBadge)   { ForeColor = ThemeAccent });
            item.SubItems.Add(new ListViewItem.ListViewSubItem(item, tw.NeedsAdmin ? "Yes" : "No"));
            item.SubItems.Add(new ListViewItem.ListViewSubItem(item, tw.CorpSafe   ? "Yes" : "No"));
            item.SubItems.Add(new ListViewItem.ListViewSubItem(item, tw.Description));
            _listView.Items.Add(item);
        }

        _listView.EndUpdate();
    }

    private void RefreshListView()
    {
        if (_treeView.SelectedNode is { Tag: string cat })
            PopulateList(cat);
    }

    // ── Helpers / Utilities ────────────────────────────────────────────────
    private List<TweakInfo> GetCheckedTweaks()
        => _listView.CheckedItems
                    .Cast<ListViewItem>()
                    .Select(i => (TweakInfo)i.Tag!)
                    .ToList();

    private void SelectAllListItems()
    {
        foreach (ListViewItem item in _listView.Items)
            item.Checked = true;
    }

    private void UpdateCounters()
    {
        int applied    = _allTweaks.Count(t => t.IsApplied);
        int notApplied = _allTweaks.Count(t => t.Status == "not_applied");
        int unknown    = _allTweaks.Count(t => t.Status == "unknown");
        _statusLabel.Text = $"Total: {_allTweaks.Count} | Applied: {applied} | Default: {notApplied} | Unknown: {unknown}";
    }

    private void SetStatus(string message)
        => _progressLabel.Text = message;

    private void SetBusy(bool busy, string? message = null)
    {
        _progressBar.Visible = busy;
        _progressBar.Style   = busy ? ProgressBarStyle.Marquee : ProgressBarStyle.Blocks;
        _btnApply.Enabled    = !busy;
        _btnRemove.Enabled   = !busy;
        _btnRefresh.Enabled  = !busy;
        if (message is not null) SetStatus(message);
    }

    // ── Event handlers ─────────────────────────────────────────────────────
    private void OnTreeAfterSelect(object? sender, TreeViewEventArgs e)
    {
        if (e.Node?.Tag is string cat)
            PopulateList(cat);
    }

    private async void OnApplyClicked(object? sender, EventArgs e)
        => await ApplySelectedAsync();

    private async void OnRemoveClicked(object? sender, EventArgs e)
        => await RemoveSelectedAsync();

    private async void OnRefreshClicked(object? sender, EventArgs e)
        => await RefreshTweaksAsync();

    private void OnFilterChanged(object? sender, EventArgs e)
        => RefreshListView();

    private void OnProfileChanged(object? sender, EventArgs e)
        => PopulateTree();

    private void OnSearchTextChanged(object? sender, EventArgs e)
        => RefreshListView();

    // ── Dark ToolStrip Renderer ────────────────────────────────────────────
    private sealed class DarkToolStripRenderer : ToolStripProfessionalRenderer
    {
        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            using var brush = new SolidBrush(ThemeSurface);
            e.Graphics.FillRectangle(brush, e.AffectedBounds);
        }

        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            if (e.Item.Selected || e.Item.Pressed)
            {
                using var brush = new SolidBrush(ThemeOverlay);
                e.Graphics.FillRectangle(brush, new Rectangle(Point.Empty, e.Item.Size));
            }
            else if (e.Item is ToolStripButton { Checked: true })
            {
                using var brush = new SolidBrush(Color.FromArgb(60, ThemeAccent));
                e.Graphics.FillRectangle(brush, new Rectangle(Point.Empty, e.Item.Size));
            }
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            e.TextColor = ThemeFg;
            base.OnRenderItemText(e);
        }

        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            int y = e.Item.Bounds.Height / 2;
            using var pen = new Pen(ThemeOverlay);
            e.Graphics.DrawLine(pen, 0, y, e.Item.Width, y);
        }
    }
}
