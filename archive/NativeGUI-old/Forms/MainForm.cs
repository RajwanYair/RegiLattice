using System.Text.Json;
using RegiLattice.Native.Models;
using RegiLattice.Native.PackageManagers;

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
    // Use AppTheme shared palette
    private static Color ThemeBg      => AppTheme.Bg;
    private static Color ThemeSurface => AppTheme.Surface;
    private static Color ThemeFg      => AppTheme.Fg;
    private static Color ThemeAccent  => AppTheme.Accent;
    private static Color ThemeGreen   => AppTheme.Green;
    private static Color ThemeRed     => AppTheme.Red;
    private static Color ThemeYellow  => AppTheme.Yellow;
    private static Color ThemeOverlay => AppTheme.Overlay;

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

        var renderer = new DarkToolStripRenderer();
        _toolStrip.BackColor = ThemeSurface;
        _toolStrip.ForeColor = ThemeFg;
        _toolStrip.Renderer  = renderer;

        _menuStrip.BackColor = ThemeSurface;
        _menuStrip.ForeColor = ThemeFg;
        _menuStrip.Renderer  = renderer;

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

        _listContextMenu.BackColor = ThemeSurface;
        _listContextMenu.ForeColor = ThemeFg;
        _listContextMenu.Renderer  = renderer;
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
        string search    = _searchBox.Text.Trim();
        string scopeSel  = _scopeCombo.SelectedItem?.ToString() ?? "All Scopes";

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

        if (scopeSel != "All Scopes")
        {
            bool wantUser    = scopeSel.StartsWith("User",    StringComparison.OrdinalIgnoreCase);
            bool wantMachine = scopeSel.StartsWith("Machine", StringComparison.OrdinalIgnoreCase);
            filtered = filtered.Where(t =>
                wantUser    ? t.ScopeBadge.Equals("USER",    StringComparison.OrdinalIgnoreCase) :
                wantMachine ? t.ScopeBadge.Equals("MACHINE", StringComparison.OrdinalIgnoreCase) :
                true);
        }

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

    private void DeselectAllListItems()
    {
        foreach (ListViewItem item in _listView.Items)
            item.Checked = false;
    }

    private void InvertListSelection()
    {
        foreach (ListViewItem item in _listView.Items)
            item.Checked = !item.Checked;
    }

    private void ToggleLogPanel()
    {
        _logPanel.Visible = !_logPanel.Visible;
    }

    private void AppendLog(string message)
    {
        string line = $"[{DateTime.Now:HH:mm:ss}] {message}";
        _logBox.AppendText(line + Environment.NewLine);
        _logBox.ScrollToCaret();
    }

    private void CopySelectedId()
    {
        if (_listView.FocusedItem?.Tag is TweakInfo tw)
            Clipboard.SetText(tw.Id);
    }

    private void CopySelectedRegistryKeys()
    {
        if (_listView.FocusedItem?.Tag is TweakInfo tw && tw.RegistryKeys.Count > 0)
            Clipboard.SetText(string.Join(Environment.NewLine, tw.RegistryKeys));
    }

    // ── Export / Import ────────────────────────────────────────────────────
    private Task OnExportPs1Async()
    {
        var tweaks = GetCheckedTweaks();
        if (tweaks.Count == 0)
        {
            MessageBox.Show("No tweaks selected.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return Task.CompletedTask;
        }
        using var dlg = new SaveFileDialog
        {
            Title      = "Export as PowerShell Script",
            Filter     = "PowerShell Script|*.ps1",
            FileName   = "regilattice-tweaks.ps1",
        };
        if (dlg.ShowDialog(this) != DialogResult.OK) return Task.CompletedTask;

        var sb = new System.Text.StringBuilder();
        sb.AppendLine("# RegiLattice — exported tweaks");
        sb.AppendLine($"# Generated: {DateTime.Now:yyyy-MM-dd HH:mm}");
        sb.AppendLine();
        foreach (var tw in tweaks)
        {
            sb.AppendLine($"# {tw.Label} ({tw.Id})");
            foreach (string key in tw.RegistryKeys)
                sb.AppendLine($"# Registry: {key}");
            sb.AppendLine($"# python -m regilattice apply {tw.Id} -y");
            sb.AppendLine();
        }
        File.WriteAllText(dlg.FileName, sb.ToString(), System.Text.Encoding.UTF8);
        AppendLog($"Exported {tweaks.Count} tweaks to {dlg.FileName}");
        SetStatus($"Exported {tweaks.Count} tweak(s) to PS1.");
        return Task.CompletedTask;
    }

    private Task OnExportJsonAsync()
    {
        var tweaks = GetCheckedTweaks();
        if (tweaks.Count == 0)
        {
            MessageBox.Show("No tweaks selected.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return Task.CompletedTask;
        }
        using var dlg = new SaveFileDialog
        {
            Title    = "Export selected IDs as JSON",
            Filter   = "JSON file|*.json",
            FileName = "regilattice-selection.json",
        };
        if (dlg.ShowDialog(this) != DialogResult.OK) return Task.CompletedTask;

        string json = JsonSerializer.Serialize(tweaks.Select(t => t.Id).ToList(),
            new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(dlg.FileName, json, System.Text.Encoding.UTF8);
        AppendLog($"Exported {tweaks.Count} IDs to {dlg.FileName}");
        SetStatus($"Exported {tweaks.Count} ID(s) to JSON.");
        return Task.CompletedTask;
    }

    private Task OnImportJsonAsync()
    {
        using var dlg = new OpenFileDialog
        {
            Title  = "Import tweak IDs from JSON",
            Filter = "JSON file|*.json",
        };
        if (dlg.ShowDialog(this) != DialogResult.OK) return Task.CompletedTask;

        List<string>? ids;
        try
        {
            string raw = File.ReadAllText(dlg.FileName);
            ids = JsonSerializer.Deserialize<List<string>>(raw);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to parse JSON:\n{ex.Message}", "Import Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return Task.CompletedTask;
        }
        if (ids is null || ids.Count == 0) { SetStatus("JSON contained no IDs."); return Task.CompletedTask; }

        var idSet = new HashSet<string>(ids, StringComparer.OrdinalIgnoreCase);
        int matched = 0;
        foreach (ListViewItem item in _listView.Items)
        {
            if (item.Tag is TweakInfo tw && idSet.Contains(tw.Id))
            {
                item.Checked = true;
                matched++;
            }
        }
        AppendLog($"Imported: matched {matched}/{ids.Count} IDs.");
        SetStatus($"Import complete: {matched}/{ids.Count} matched.");
        return Task.CompletedTask;
    }

    // ── Package manager dialogs ────────────────────────────────────────────
    private void OnOpenScoopManager()
    {
        using var dlg = new ScoopManagerDialog();
        AppTheme.Apply(dlg);
        dlg.ShowDialog(this);
    }

    private void OnOpenPSModuleManager()
    {
        using var dlg = new PSModuleManagerDialog();
        AppTheme.Apply(dlg);
        dlg.ShowDialog(this);
    }

    private Task OnOpenPipManagerAsync()
    {
        string pythonPath = _bridge is not null ? PythonBridge.FindPython() : "python";
        using var dlg = new PipManagerDialog(pythonPath);
        AppTheme.Apply(dlg);
        dlg.ShowDialog(this);
        return Task.CompletedTask;
    }

    private void OnAbout()
    {
        int tweakCount    = _allTweaks.Count;
        int categoryCount = _tweaksByCategory.Count;
        string pythonPath = _bridge is not null ? PythonBridge.FindPython() : "(bridge not initialised)";
        bool isCorp       = false;  // TODO: wire to corpguard if exposed via CLI
        using var dlg = new AboutDialog(tweakCount, categoryCount, pythonPath, isCorp);
        dlg.ShowDialog(this);
    }

    private void UpdateCounters()
    {
        int applied    = _allTweaks.Count(t => t.IsApplied);
        int notApplied = _allTweaks.Count(t => t.Status == "not_applied");
        int unknown    = _allTweaks.Count(t => t.Status == "unknown");
        _statusLabel.Text = $"Total: {_allTweaks.Count} | Applied: {applied} | Default: {notApplied} | Unknown: {unknown}";
    }

    private void SetStatus(string message)
    {
        _progressLabel.Text = message;
        AppendLog(message);
    }

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

    private void OnGlobalKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Control && e.KeyCode == Keys.Enter)  { OnApplyClicked(this, e);    e.Handled = true; return; }
        if (e.Control && e.KeyCode == Keys.Delete) { OnRemoveClicked(this, e);   e.Handled = true; return; }
        if (e.KeyCode == Keys.F5)                  { OnRefreshClicked(this, e);  e.Handled = true; return; }
        if (e.Control && e.KeyCode == Keys.F)      { _searchBox.Focus();         e.Handled = true; return; }
        if (e.KeyCode == Keys.Escape)              { _searchBox.Text = "";       e.Handled = true; return; }
        if (e.Control && e.KeyCode == Keys.A)      { SelectAllListItems();       e.Handled = true; return; }
        if (e.Control && e.KeyCode == Keys.D)      { DeselectAllListItems();     e.Handled = true; return; }
        if (e.Control && e.KeyCode == Keys.I)      { InvertListSelection();      e.Handled = true; return; }
        if (e.Control && e.KeyCode == Keys.L)      { ToggleLogPanel();           e.Handled = true; return; }
        if (e.Control && e.KeyCode == Keys.E)      { _treeView.ExpandAll();      e.Handled = true; return; }
    }

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
