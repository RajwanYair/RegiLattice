using System.Text;
using System.Text.Json;
using RegiLattice.Core;
using RegiLattice.Core.Models;

namespace RegiLattice.GUI.Forms;

/// <summary>
/// RegiLattice GUI — main window.
/// Uses TweakEngine directly for all registry operations.
/// </summary>
public partial class MainForm : Form
{
    // ── State ──────────────────────────────────────────────────────────────
    private readonly TweakEngine _engine = new();
    private CancellationTokenSource _cts = new();

    // Cached status per tweak ID — refreshed on demand.
    private Dictionary<string, TweakResult> _statusCache = [];

    // ── Construction ───────────────────────────────────────────────────────
    public MainForm()
    {
        SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
        InitializeComponent();
        Text = "RegiLattice";
        Icon = SystemIcons.Shield;

        // Load saved theme from config
        var cfg = AppConfig.Load();
        AppTheme.SetTheme(cfg.Theme);
        _themeCombo.SelectedItem = AppTheme.CurrentThemeName();

        ApplyTheme();
    }

    // ── Lifecycle ──────────────────────────────────────────────────────────
    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        await InitialiseEngineAsync();
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        _cts.Cancel();
        base.OnFormClosing(e);
    }

    // ── Theme ──────────────────────────────────────────────────────────────
    private void ApplyTheme()
    {
        SuspendLayout();

        BackColor = AppTheme.Bg;
        ForeColor = AppTheme.Fg;

        var renderer = new DarkToolStripRenderer();
        _toolStrip.BackColor = AppTheme.Surface;
        _toolStrip.ForeColor = AppTheme.Fg;
        _toolStrip.Renderer = renderer;

        _menuStrip.BackColor = AppTheme.Surface;
        _menuStrip.ForeColor = AppTheme.Fg;
        _menuStrip.Renderer = renderer;

        _treeView.BackColor = AppTheme.Bg;
        _treeView.ForeColor = AppTheme.Fg;

        _listView.BackColor = AppTheme.Surface;
        _listView.ForeColor = AppTheme.Fg;
        _listView.OwnerDraw = true;
        _listView.DrawColumnHeader -= OnDrawColumnHeader;
        _listView.DrawSubItem -= OnDrawSubItem;
        _listView.DrawColumnHeader += OnDrawColumnHeader;
        _listView.DrawSubItem += OnDrawSubItem;

        _statusStrip.BackColor = AppTheme.Overlay;
        _statusStrip.ForeColor = AppTheme.Fg;

        _searchBox.BackColor = AppTheme.Overlay;
        _searchBox.ForeColor = AppTheme.Fg;

        _logBox.BackColor = AppTheme.Bg;
        _logBox.ForeColor = AppTheme.Green;

        _listContextMenu.BackColor = AppTheme.Surface;
        _listContextMenu.ForeColor = AppTheme.Fg;
        _listContextMenu.Renderer = renderer;

        _detailPanel.BackColor = AppTheme.Surface;
        _detailLabel.ForeColor = AppTheme.FgDim;

        // Re-colour tree nodes
        foreach (TreeNode node in _treeView.Nodes)
            node.ForeColor = AppTheme.Fg;

        ResumeLayout(true);
        _listView.Invalidate();
    }

    // ── Owner-draw for dark ListView ───────────────────────────────────────
    private void OnDrawColumnHeader(object? sender, DrawListViewColumnHeaderEventArgs e)
    {
        using var bg = new SolidBrush(AppTheme.Overlay);
        e.Graphics.FillRectangle(bg, e.Bounds);
        TextRenderer.DrawText(e.Graphics, e.Header?.Text ?? "", Font, e.Bounds, AppTheme.Accent,
            TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
    }

    private void OnDrawSubItem(object? sender, DrawListViewSubItemEventArgs e)
    {
        if (e.Item is null) return;
        Color bg = e.ItemIndex % 2 == 0 ? AppTheme.Surface : AppTheme.Bg;
        if (e.Item.Selected) bg = AppTheme.Overlay;

        using var brush = new SolidBrush(bg);
        e.Graphics.FillRectangle(brush, e.Bounds);

        // Status column (index 2) — coloured text
        Color fg = AppTheme.Fg;
        if (e.ColumnIndex == 2 && e.Item.Tag is TweakDef td)
        {
            var status = _statusCache.GetValueOrDefault(td.Id, TweakResult.Unknown);
            fg = status switch
            {
                TweakResult.Applied => AppTheme.Green,
                TweakResult.NotApplied => AppTheme.Red,
                _ => AppTheme.Yellow,
            };
        }

        TextRenderer.DrawText(e.Graphics, e.SubItem?.Text ?? "", Font, e.Bounds, fg,
            TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
    }

    // ── Initialisation ─────────────────────────────────────────────────────
    private async Task InitialiseEngineAsync()
    {
        SetBusy(true, "Loading tweaks...");
        try
        {
            await Task.Run(() => _engine.RegisterBuiltins(), _cts.Token);
            SetStatus($"Loaded {_engine.TweakCount} tweaks across {_engine.CategoryCount} categories.");
            await RefreshStatusAsync();
        }
        catch (OperationCanceledException) { }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to initialise engine:\n{ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            SetStatus("Error loading tweaks.");
        }
        finally { SetBusy(false); }
    }

    // ── Actions ────────────────────────────────────────────────────────────
    internal async Task RefreshStatusAsync()
    {
        SetBusy(true, "Detecting tweak statuses...");
        try
        {
            _statusCache = await Task.Run(() => _engine.StatusMap(parallel: true), _cts.Token);
            PopulateTree();
            UpdateCounters();
            SetStatus($"Status refreshed for {_statusCache.Count} tweaks.");
        }
        catch (OperationCanceledException) { }
        catch (Exception ex)
        {
            SetStatus($"Error refreshing status: {ex.Message}");
        }
        finally { SetBusy(false); }
    }

    private async Task ApplySelectedAsync()
    {
        var selected = GetCheckedTweaks();
        if (selected.Count == 0) return;

        bool force = _forceCheck.Checked;
        SetBusy(true, $"Applying {selected.Count} tweak(s)...");
        try
        {
            var results = await Task.Run(() =>
                _engine.ApplyBatch(selected, forceCorp: force), _cts.Token);

            int success = results.Values.Count(r => r == TweakResult.Applied);
            foreach (var (id, result) in results)
                _statusCache[id] = _engine.DetectStatus(_engine.GetTweak(id)!);

            PopulateTree();
            RefreshListView();
            UpdateCounters();
            SetStatus($"Applied {success}/{selected.Count} tweak(s).");
        }
        catch (OperationCanceledException) { }
        catch (Exception ex)
        {
            SetStatus($"Error applying tweaks: {ex.Message}");
        }
        finally { SetBusy(false); }
    }

    private async Task RemoveSelectedAsync()
    {
        var selected = GetCheckedTweaks();
        if (selected.Count == 0) return;

        bool force = _forceCheck.Checked;
        SetBusy(true, $"Removing {selected.Count} tweak(s)...");
        try
        {
            var results = await Task.Run(() =>
                _engine.RemoveBatch(selected, forceCorp: force), _cts.Token);

            int success = results.Values.Count(r => r == TweakResult.NotApplied);
            foreach (var (id, _) in results)
                _statusCache[id] = _engine.DetectStatus(_engine.GetTweak(id)!);

            PopulateTree();
            RefreshListView();
            UpdateCounters();
            SetStatus($"Removed {success}/{selected.Count} tweak(s).");
        }
        catch (OperationCanceledException) { }
        catch (Exception ex)
        {
            SetStatus($"Error removing tweaks: {ex.Message}");
        }
        finally { SetBusy(false); }
    }

    // ── Tree / List population ─────────────────────────────────────────────
    private void PopulateTree()
    {
        string? previousCat = _treeView.SelectedNode?.Tag as string;

        _treeView.BeginUpdate();
        _treeView.Nodes.Clear();

        // Get profile filter
        string profile = _profileCombo.SelectedItem?.ToString() ?? "(None)";
        HashSet<string>? profileCats = null;
        if (profile != "(None)")
        {
            var prof = TweakEngine.GetProfile(profile);
            if (prof is not null)
                profileCats = new HashSet<string>(prof.ApplyCategories, StringComparer.OrdinalIgnoreCase);
        }

        TreeNode? selectNode = null;
        var byCategory = _engine.TweaksByCategory();
        foreach (var kvp in byCategory.OrderBy(k => k.Key))
        {
            if (profileCats is not null && !profileCats.Contains(kvp.Key))
                continue;

            int count = kvp.Value.Count;
            int applied = kvp.Value.Count(t => _statusCache.GetValueOrDefault(t.Id) == TweakResult.Applied);
            string icon = CategoryIcons.GetSymbol(CategoryIcons.GetIcon(kvp.Key));
            var node = new TreeNode($"{icon} {kvp.Key}  ({applied}/{count})") { Tag = kvp.Key };
            node.ForeColor = AppTheme.Fg;
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
        string scopeSel = _scopeCombo.SelectedItem?.ToString() ?? "All Scopes";

        _listView.BeginUpdate();
        _listView.Items.Clear();

        var byCategory = _engine.TweaksByCategory();
        if (!byCategory.TryGetValue(category, out var tweaks))
        {
            _listView.EndUpdate();
            return;
        }

        IEnumerable<TweakDef> filtered = tweaks;

        // Status filter
        if (filter != "All")
        {
            TweakResult target = filter switch
            {
                "Applied" => TweakResult.Applied,
                "Not Applied" => TweakResult.NotApplied,
                _ => TweakResult.Unknown,
            };
            filtered = filtered.Where(t => _statusCache.GetValueOrDefault(t.Id) == target);
        }

        // Scope filter
        if (scopeSel != "All Scopes")
        {
            bool wantUser = scopeSel.StartsWith("User", StringComparison.OrdinalIgnoreCase);
            bool wantMachine = scopeSel.StartsWith("Machine", StringComparison.OrdinalIgnoreCase);
            filtered = filtered.Where(t =>
                wantUser ? t.Scope == TweakScope.User :
                wantMachine ? t.Scope == TweakScope.Machine :
                true);
        }

        // Search filter
        if (search.Length > 0)
        {
            filtered = filtered.Where(t =>
                t.Label.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                t.Id.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                t.Description.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        foreach (var td in filtered)
        {
            var status = _statusCache.GetValueOrDefault(td.Id, TweakResult.Unknown);
            string statusText = status switch
            {
                TweakResult.Applied => "Applied",
                TweakResult.NotApplied => "Default",
                TweakResult.Error => "Error",
                _ => "Unknown",
            };

            var item = new ListViewItem(td.Label) { Tag = td, UseItemStyleForSubItems = false };
            Color statusColor = status switch
            {
                TweakResult.Applied => AppTheme.Green,
                TweakResult.NotApplied => AppTheme.Red,
                _ => AppTheme.Yellow,
            };

            string scopeBadge = td.Scope switch
            {
                TweakScope.User => "USER",
                TweakScope.Machine => "MACHINE",
                TweakScope.Both => "BOTH",
                _ => "?",
            };

            string kindSymbol = CategoryIcons.GetKindSymbol(td.Kind);
            item.SubItems.Add(new ListViewItem.ListViewSubItem(item, kindSymbol) { ForeColor = AppTheme.Accent });
            item.SubItems.Add(new ListViewItem.ListViewSubItem(item, statusText) { ForeColor = statusColor });
            item.SubItems.Add(new ListViewItem.ListViewSubItem(item, scopeBadge) { ForeColor = AppTheme.Accent });
            item.SubItems.Add(new ListViewItem.ListViewSubItem(item, td.NeedsAdmin ? "Yes" : "No"));
            item.SubItems.Add(new ListViewItem.ListViewSubItem(item, td.CorpSafe ? "Yes" : "No"));
            item.SubItems.Add(new ListViewItem.ListViewSubItem(item, td.Description));
            _listView.Items.Add(item);
        }

        _listView.EndUpdate();
    }

    private void RefreshListView()
    {
        string search = _searchBox.Text.Trim();
        if (search.Length > 0)
        {
            // Cross-category search: show matching tweaks from ALL categories
            PopulateSearchResults(search);
        }
        else if (_treeView.SelectedNode is { Tag: string cat })
        {
            PopulateList(cat);
        }
    }

    private void PopulateSearchResults(string search)
    {
        string filter = _filterCombo.SelectedItem?.ToString() ?? "All";
        string scopeSel = _scopeCombo.SelectedItem?.ToString() ?? "All Scopes";

        _listView.BeginUpdate();
        _listView.Items.Clear();

        IEnumerable<TweakDef> all = _engine.Search(search);

        // Status filter
        if (filter != "All")
        {
            TweakResult target = filter switch
            {
                "Applied" => TweakResult.Applied,
                "Not Applied" => TweakResult.NotApplied,
                _ => TweakResult.Unknown,
            };
            all = all.Where(t => _statusCache.GetValueOrDefault(t.Id) == target);
        }

        // Scope filter
        if (scopeSel != "All Scopes")
        {
            bool wantUser = scopeSel.StartsWith("User", StringComparison.OrdinalIgnoreCase);
            bool wantMachine = scopeSel.StartsWith("Machine", StringComparison.OrdinalIgnoreCase);
            all = all.Where(t =>
                wantUser ? t.Scope == TweakScope.User :
                wantMachine ? t.Scope == TweakScope.Machine :
                true);
        }

        foreach (var td in all)
        {
            var status = _statusCache.GetValueOrDefault(td.Id, TweakResult.Unknown);
            string statusText = status switch
            {
                TweakResult.Applied => "Applied",
                TweakResult.NotApplied => "Default",
                TweakResult.Error => "Error",
                _ => "Unknown",
            };
            string kindSymbol = CategoryIcons.GetKindSymbol(td.Kind);
            var item = new ListViewItem(td.Label);
            item.SubItems.AddRange([kindSymbol, statusText, td.Scope.ToString(), td.NeedsAdmin ? "Yes" : "No", td.CorpSafe ? "Yes" : "No", $"[{td.Category}] {td.Description}"]);
            item.Tag = td;
            _listView.Items.Add(item);
        }

        _listView.EndUpdate();
    }

    // ── Helpers / Utilities ────────────────────────────────────────────────
    private List<TweakDef> GetCheckedTweaks()
        => _listView.CheckedItems
                    .Cast<ListViewItem>()
                    .Select(i => (TweakDef)i.Tag!)
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
        if (_listView.FocusedItem?.Tag is TweakDef td)
            Clipboard.SetText(td.Id);
    }

    private void CopySelectedRegistryKeys()
    {
        if (_listView.FocusedItem?.Tag is TweakDef td && td.RegistryKeys.Count > 0)
            Clipboard.SetText(string.Join(Environment.NewLine, td.RegistryKeys));
    }

    // ── Export / Import ────────────────────────────────────────────────────
    private void OnExportPs1()
    {
        var tweaks = GetCheckedTweaks();
        if (tweaks.Count == 0)
        {
            MessageBox.Show("No tweaks selected.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        using var dlg = new SaveFileDialog
        {
            Title = "Export as PowerShell Script",
            Filter = "PowerShell Script|*.ps1",
            FileName = "regilattice-tweaks.ps1",
        };
        if (dlg.ShowDialog(this) != DialogResult.OK) return;

        var sb = new StringBuilder();
        sb.AppendLine("# RegiLattice — exported tweaks");
        sb.AppendLine($"# Generated: {DateTime.Now:yyyy-MM-dd HH:mm}");
        sb.AppendLine();
        foreach (var td in tweaks)
        {
            sb.AppendLine($"# {td.Label} ({td.Id})");
            foreach (string key in td.RegistryKeys)
                sb.AppendLine($"# Registry: {key}");
            sb.AppendLine();
        }
        File.WriteAllText(dlg.FileName, sb.ToString(), Encoding.UTF8);
        AppendLog($"Exported {tweaks.Count} tweaks to {dlg.FileName}");
        SetStatus($"Exported {tweaks.Count} tweak(s) to PS1.");
    }

    private void OnExportJson()
    {
        var tweaks = GetCheckedTweaks();
        if (tweaks.Count == 0)
        {
            MessageBox.Show("No tweaks selected.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        using var dlg = new SaveFileDialog
        {
            Title = "Export selected IDs as JSON",
            Filter = "JSON file|*.json",
            FileName = "regilattice-selection.json",
        };
        if (dlg.ShowDialog(this) != DialogResult.OK) return;

        string json = JsonSerializer.Serialize(tweaks.Select(t => t.Id).ToList(),
            new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(dlg.FileName, json, Encoding.UTF8);
        AppendLog($"Exported {tweaks.Count} IDs to {dlg.FileName}");
        SetStatus($"Exported {tweaks.Count} ID(s) to JSON.");
    }

    private void OnImportJson()
    {
        using var dlg = new OpenFileDialog
        {
            Title = "Import tweak IDs from JSON",
            Filter = "JSON file|*.json",
        };
        if (dlg.ShowDialog(this) != DialogResult.OK) return;

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
            return;
        }
        if (ids is null || ids.Count == 0) { SetStatus("JSON contained no IDs."); return; }

        var idSet = new HashSet<string>(ids, StringComparer.OrdinalIgnoreCase);
        int matched = 0;
        foreach (ListViewItem item in _listView.Items)
        {
            if (item.Tag is TweakDef td && idSet.Contains(td.Id))
            {
                item.Checked = true;
                matched++;
            }
        }
        AppendLog($"Imported: matched {matched}/{ids.Count} IDs.");
        SetStatus($"Import complete: {matched}/{ids.Count} matched.");
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

    private void OnOpenPipManager()
    {
        using var dlg = new PipManagerDialog();
        AppTheme.Apply(dlg);
        dlg.ShowDialog(this);
    }

    private void OnOpenWinGetManager()
    {
        using var dlg = new WinGetManagerDialog();
        AppTheme.Apply(dlg);
        dlg.ShowDialog(this);
    }

    private void OnOpenChocolateyManager()
    {
        using var dlg = new ChocolateyManagerDialog();
        AppTheme.Apply(dlg);
        dlg.ShowDialog(this);
    }

    private void OnOpenToolVersions()
    {
        using var dlg = new ToolVersionsDialog();
        AppTheme.Apply(dlg);
        dlg.ShowDialog(this);
    }

    private void OnAbout()
    {
        bool isCorp = CorporateGuard.IsCorporateNetwork();
        using var dlg = new AboutDialog(_engine.TweakCount, _engine.CategoryCount, isCorp);
        dlg.ShowDialog(this);
    }

    private void UpdateCounters()
    {
        int applied = _statusCache.Values.Count(r => r == TweakResult.Applied);
        int notApplied = _statusCache.Values.Count(r => r == TweakResult.NotApplied);
        int unknown = _statusCache.Values.Count(r => r == TweakResult.Unknown);
        int error = _statusCache.Values.Count(r => r == TweakResult.Error);
        _statusLabel.Text = $"Total: {_statusCache.Count} | Applied: {applied} | Default: {notApplied} | Unknown: {unknown} | Errors: {error}";
    }

    private void SetStatus(string message)
    {
        _progressLabel.Text = message;
        AppendLog(message);
    }

    private void SetBusy(bool busy, string? message = null)
    {
        _progressBar.Visible = busy;
        _progressBar.Style = busy ? ProgressBarStyle.Marquee : ProgressBarStyle.Blocks;
        _btnApply.Enabled = !busy;
        _btnRemove.Enabled = !busy;
        _btnRefresh.Enabled = !busy;
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
        => await RefreshStatusAsync();

    private void OnFilterChanged(object? sender, EventArgs e)
        => RefreshListView();

    private void OnProfileChanged(object? sender, EventArgs e)
        => PopulateTree();

    private void OnThemeChanged(object? sender, EventArgs e)
    {
        if (_themeCombo.SelectedItem is not string name) return;
        AppTheme.SetTheme(name);
        ApplyTheme();
        PopulateTree();

        // Persist choice
        var cfg = AppConfig.Load();
        cfg.Theme = name;
        cfg.Save();
    }

    private void OnSearchTextChanged(object? sender, EventArgs e)
        => RefreshListView();

    private void OnListViewSelectionChanged(object? sender, EventArgs e)
    {
        if (_listView.FocusedItem?.Tag is TweakDef td)
        {
            var status = _statusCache.GetValueOrDefault(td.Id, TweakResult.Unknown);
            string statusStr = status switch
            {
                TweakResult.Applied => "Applied",
                TweakResult.NotApplied => "Default",
                TweakResult.Error => "Error",
                _ => "Unknown",
            };
            string tags = td.Tags.Count > 0 ? string.Join(", ", td.Tags) : "—";
            string keys = td.RegistryKeys.Count > 0
                ? string.Join("; ", td.RegistryKeys.Take(3))
                : (td.ApplyOps.Count > 0 ? "Registry operations" : "Command-based");
            _detailLabel.Text =
                $"ID: {td.Id}   |   Kind: {CategoryIcons.GetKindSymbol(td.Kind)} {td.Kind}   |   Status: {statusStr}   |   Scope: {td.Scope}\n" +
                $"Tags: {tags}\n" +
                $"Keys: {keys}" +
                (td.SideEffects.Length > 0 ? $"\nSide Effects: {td.SideEffects}" : "");
            _detailLabel.ForeColor = AppTheme.Fg;
        }
        else
        {
            _detailLabel.Text = "Select a tweak to see details.";
            _detailLabel.ForeColor = AppTheme.FgDim;
        }
    }

    private void OnGlobalKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Control && e.KeyCode == Keys.Enter) { OnApplyClicked(this, e); e.Handled = true; return; }
        if (e.Control && e.KeyCode == Keys.Delete) { OnRemoveClicked(this, e); e.Handled = true; return; }
        if (e.KeyCode == Keys.F5) { OnRefreshClicked(this, e); e.Handled = true; return; }
        if (e.Control && e.KeyCode == Keys.F) { _searchBox.Focus(); e.Handled = true; return; }
        if (e.KeyCode == Keys.Escape) { _searchBox.Text = ""; e.Handled = true; return; }
        if (e.Control && e.KeyCode == Keys.A) { SelectAllListItems(); e.Handled = true; return; }
        if (e.Control && e.KeyCode == Keys.D) { DeselectAllListItems(); e.Handled = true; return; }
        if (e.Control && e.KeyCode == Keys.I) { InvertListSelection(); e.Handled = true; return; }
        if (e.Control && e.KeyCode == Keys.L) { ToggleLogPanel(); e.Handled = true; return; }
        if (e.Control && e.KeyCode == Keys.E) { _treeView.ExpandAll(); e.Handled = true; return; }
    }

    // ── Dark ToolStrip Renderer ────────────────────────────────────────────
    private sealed class DarkToolStripRenderer : ToolStripProfessionalRenderer
    {
        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            using var brush = new SolidBrush(AppTheme.Surface);
            e.Graphics.FillRectangle(brush, e.AffectedBounds);
        }

        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            if (e.Item.Selected || e.Item.Pressed)
            {
                using var brush = new SolidBrush(AppTheme.Overlay);
                e.Graphics.FillRectangle(brush, new Rectangle(Point.Empty, e.Item.Size));
            }
            else if (e.Item is ToolStripButton { Checked: true })
            {
                using var brush = new SolidBrush(Color.FromArgb(60, AppTheme.Accent));
                e.Graphics.FillRectangle(brush, new Rectangle(Point.Empty, e.Item.Size));
            }
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            e.TextColor = AppTheme.Fg;
            base.OnRenderItemText(e);
        }

        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            int y = e.Item.Bounds.Height / 2;
            using var pen = new Pen(AppTheme.Overlay);
            e.Graphics.DrawLine(pen, 0, y, e.Item.Width, y);
        }
    }
}
