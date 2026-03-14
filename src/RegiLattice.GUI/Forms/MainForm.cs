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

    // Tweaks that are not applicable to the current hardware.
    private readonly HashSet<string> _inapplicableIds = new(StringComparer.Ordinal);

    // Tweaks pending reboot/restart to take effect (applied but not yet active).
    private readonly HashSet<string> _pendingRebootIds = new(StringComparer.Ordinal);

    // Column sorting and filtering state.
    private readonly ListViewColumnSorter _columnSorter = new();
    private readonly Dictionary<int, HashSet<string>> _columnFilters = [];

    // Categories whose tweak status has been loaded (lazy-load on select).
    private readonly HashSet<string> _loadedCategories = new(StringComparer.OrdinalIgnoreCase);

    // ── Construction ───────────────────────────────────────────────────────
    public MainForm()
    {
        SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
        InitializeComponent();
        Text = "RegiLattice";
        Icon = AppIcons.AppIcon;

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
        _toolStrip.GripStyle = ToolStripGripStyle.Hidden;
        _toolStrip.Padding = new Padding(4, 2, 4, 2);

        _menuStrip.BackColor = AppTheme.Surface;
        _menuStrip.ForeColor = AppTheme.Fg;
        _menuStrip.Renderer = renderer;

        _treeView.BackColor = AppTheme.Bg;
        _treeView.ForeColor = AppTheme.Fg;
        _treeView.BorderStyle = BorderStyle.None;
        _treeView.Indent = 20;

        _listView.BackColor = AppTheme.Surface;
        _listView.ForeColor = AppTheme.Fg;
        _listView.BorderStyle = BorderStyle.None;
        _listView.OwnerDraw = true;
        _listView.DrawColumnHeader -= OnDrawColumnHeader;
        _listView.DrawSubItem -= OnDrawSubItem;
        _listView.DrawColumnHeader += OnDrawColumnHeader;
        _listView.DrawSubItem += OnDrawSubItem;

        _statusStrip.BackColor = AppTheme.Overlay;
        _statusStrip.ForeColor = AppTheme.Fg;
        _statusStrip.SizingGrip = false;

        _searchBox.BackColor = AppTheme.Overlay;
        _searchBox.ForeColor = AppTheme.Fg;

        _logBox.BackColor = AppTheme.Bg;
        _logBox.ForeColor = AppTheme.Green;

        _listContextMenu.BackColor = AppTheme.Surface;
        _listContextMenu.ForeColor = AppTheme.Fg;
        _listContextMenu.Renderer = renderer;

        // Detail panel with accent left border
        _detailPanel.BackColor = AppTheme.Surface;
        _detailPanel.Padding = new Padding(8, 6, 8, 6);
        _detailLabel.ForeColor = AppTheme.FgDim;
        _detailLabel.Font = AppTheme.Regular;

        // Re-colour tree nodes
        foreach (TreeNode node in _treeView.Nodes)
            node.ForeColor = AppTheme.Fg;

        ResumeLayout(true);
        _listView.Invalidate();
    }

    // ── Owner-draw for dark ListView ───────────────────────────────────────
    private void OnDrawColumnHeader(object? sender, DrawListViewColumnHeaderEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        // Header background
        using var bgBr = new SolidBrush(AppTheme.Overlay);
        g.FillRectangle(bgBr, e.Bounds);

        // Accent underline
        using var accentPen = new Pen(Color.FromArgb(80, AppTheme.Accent), 2f);
        g.DrawLine(accentPen, e.Bounds.Left, e.Bounds.Bottom - 1, e.Bounds.Right, e.Bounds.Bottom - 1);

        // Sort indicator arrow
        string sortArrow = "";
        if (_columnSorter.ColumnIndex == e.ColumnIndex && _columnSorter.Order != SortOrder.None)
            sortArrow = _columnSorter.Order == SortOrder.Ascending ? " \u25B2" : " \u25BC";

        // Filter indicator dot
        string filterDot = _columnFilters.ContainsKey(e.ColumnIndex) ? " \u25CF" : "";

        string headerText = (e.Header?.Text ?? "") + sortArrow + filterDot;
        var textRect = new Rectangle(e.Bounds.X + 6, e.Bounds.Y, e.Bounds.Width - 22, e.Bounds.Height);
        TextRenderer.DrawText(
            g,
            headerText,
            AppTheme.Bold,
            textRect,
            AppTheme.Accent,
            TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis
        );

        // Small dropdown arrow on the right side for filter
        var dropRect = new Rectangle(e.Bounds.Right - 18, e.Bounds.Y, 16, e.Bounds.Height);
        TextRenderer.DrawText(
            g,
            "\u25BE",
            AppTheme.Small,
            dropRect,
            AppTheme.FgDim,
            TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter
        );
    }

    private void OnDrawSubItem(object? sender, DrawListViewSubItemEventArgs e)
    {
        if (e.Item is null)
            return;
        var g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        // Alternating rows with selection highlight
        Color bg = e.ItemIndex % 2 == 0 ? AppTheme.Surface : AppTheme.Bg;
        if (e.Item.Selected)
            bg = Color.FromArgb(AppTheme.Overlay.R, AppTheme.Overlay.G, AppTheme.Overlay.B);

        using var bgBrush = new SolidBrush(bg);
        g.FillRectangle(bgBrush, e.Bounds);

        // Left accent bar on selected row (first column only)
        if (e.Item.Selected && e.ColumnIndex == 0)
        {
            using var accentBrush = new SolidBrush(AppTheme.Accent);
            g.FillRectangle(accentBrush, e.Bounds.X, e.Bounds.Y + 2, 3, e.Bounds.Height - 4);
        }

        bool applicable = e.Item.Tag is TweakDef tda && !_inapplicableIds.Contains(tda.Id);

        // Status column (index 2) — pill badge with pending reboot support
        if (e.ColumnIndex == 2 && e.Item.Tag is TweakDef td)
        {
            string statusText = e.SubItem?.Text ?? "";
            Color pillBg;
            Color pillFg;

            if (!applicable)
            {
                pillBg = AppTheme.FgDim;
                pillFg = AppTheme.FgDim;
            }
            else if (_pendingRebootIds.Contains(td.Id))
            {
                statusText = "\u23F3 Pending";
                pillBg = AppTheme.Yellow;
                pillFg = AppTheme.Yellow;
            }
            else
            {
                var status = _statusCache.GetValueOrDefault(td.Id, TweakResult.Unknown);
                (pillBg, pillFg) = status switch
                {
                    TweakResult.Applied => (AppTheme.Green, AppTheme.Green),
                    TweakResult.NotApplied => (AppTheme.Red, AppTheme.Red),
                    _ => (AppTheme.Yellow, AppTheme.Yellow),
                };
            }

            int pillY = e.Bounds.Y + (e.Bounds.Height - 18) / 2;
            AppTheme.DrawPill(g, statusText, AppTheme.SmallBold, pillBg, pillFg, e.Bounds.X + 4, pillY, 6, 1);
            return;
        }

        // Scope column (index 3) — pill badge
        if (e.ColumnIndex == 3 && e.Item.Tag is TweakDef tdScope)
        {
            string scopeText = e.SubItem?.Text ?? "";
            Color scopeColor = tdScope.Scope switch
            {
                TweakScope.User => AppTheme.Green,
                TweakScope.Machine => AppTheme.Info,
                TweakScope.Both => AppTheme.Yellow,
                _ => AppTheme.FgDim,
            };

            int pillY = e.Bounds.Y + (e.Bounds.Height - 18) / 2;
            AppTheme.DrawPill(g, scopeText, AppTheme.SmallBold, scopeColor, scopeColor, e.Bounds.X + 4, pillY, 6, 1);
            return;
        }

        // Default text rendering
        Color fg = applicable ? AppTheme.Fg : AppTheme.FgDim;

        // Kind column — accent colour
        if (e.ColumnIndex == 1)
            fg = applicable ? AppTheme.Accent : AppTheme.FgDim;

        var textBounds = new Rectangle(e.Bounds.X + (e.ColumnIndex == 0 ? 6 : 4), e.Bounds.Y, e.Bounds.Width - 6, e.Bounds.Height);
        TextRenderer.DrawText(
            g,
            e.SubItem?.Text ?? "",
            Font,
            textBounds,
            fg,
            TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis
        );
    }

    // ── Initialisation ─────────────────────────────────────────────────────
    private async Task InitialiseEngineAsync()
    {
        SetBusy(true, "Loading tweaks...");
        try
        {
            await Task.Run(() => _engine.RegisterBuiltins(), _cts.Token);
            SetStatus($"Loaded {_engine.TweakCount} tweaks across {_engine.CategoryCount} categories.");

            // Evaluate hardware applicability — group by category to avoid redundant checks
            await Task.Run(
                () =>
                {
                    var categoryApplicable = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
                    foreach (var td in _engine.AllTweaks())
                    {
                        // Custom predicates or tag-based checks must run per-tweak
                        if (
                            td.IsApplicable is not null
                            || td.Tags.Any(t =>
                                t.Equals("nvidia", StringComparison.OrdinalIgnoreCase)
                                || t.Equals("amd-gpu", StringComparison.OrdinalIgnoreCase)
                                || t.Equals("docker", StringComparison.OrdinalIgnoreCase)
                                || t.Equals("laptop", StringComparison.OrdinalIgnoreCase)
                            )
                        )
                        {
                            if (!TweakEngine.IsApplicableOnHardware(td))
                                _inapplicableIds.Add(td.Id);
                        }
                        else if (categoryApplicable.TryGetValue(td.Category, out var cached))
                        {
                            if (!cached)
                                _inapplicableIds.Add(td.Id);
                        }
                        else
                        {
                            var applicable = TweakEngine.IsApplicableOnHardware(td);
                            categoryApplicable[td.Category] = applicable;
                            if (!applicable)
                                _inapplicableIds.Add(td.Id);
                        }
                    }
                },
                _cts.Token
            );

            // Populate the category tree without selecting any node.
            // Tweak statuses are lazy-loaded when the user clicks a category.
            PopulateTree(autoSelect: false);
            UpdateCounters();
        }
        catch (OperationCanceledException) { }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to initialise engine:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            SetStatus("Error loading tweaks.");
        }
        finally
        {
            SetBusy(false);
        }
    }

    // ── Actions ────────────────────────────────────────────────────────────
    internal async Task RefreshStatusAsync()
    {
        SetBusy(true, "Detecting tweak statuses...");
        try
        {
            _statusCache = await Task.Run(() => _engine.StatusMap(parallel: true), _cts.Token);
            _loadedCategories.Clear();
            foreach (var cat in _engine.Categories())
                _loadedCategories.Add(cat);
            PopulateTree();
            UpdateCounters();
            SetStatus($"Status refreshed for {_statusCache.Count} tweaks.");
        }
        catch (OperationCanceledException) { }
        catch (Exception ex)
        {
            SetStatus($"Error refreshing status: {ex.Message}");
        }
        finally
        {
            SetBusy(false);
        }
    }

    private async Task ApplySelectedAsync()
    {
        var selected = GetCheckedTweaks();
        if (selected.Count == 0)
            return;

        bool force = _forceCheck.Checked;

        // Show log panel so user can watch execution progress
        if (!_logPanel.Visible)
            _logPanel.Visible = true;

        SetBusy(true, $"Applying {selected.Count} tweak(s)...");
        try
        {
            int progress = 0;
            var results = await Task.Run(
                () =>
                {
                    var res = new Dictionary<string, TweakResult>(StringComparer.Ordinal);
                    foreach (var td in selected)
                    {
                        progress++;
                        Invoke(() =>
                        {
                            AppendLog($"[{progress}/{selected.Count}] Applying: {td.Label} ({td.Id})");
                            _progressLabel.Text = $"Applying {progress}/{selected.Count}: {td.Label}";
                        });
                        var r = _engine.Apply(td, forceCorp: force);
                        res[td.Id] = r;
                    }
                    return res;
                },
                _cts.Token
            );

            int success = results.Values.Count(r => r == TweakResult.Applied);

            // Mark successfully applied tweaks as pending reboot
            foreach (var (id, result) in results)
            {
                _statusCache[id] = _engine.DetectStatus(_engine.GetTweak(id)!);
                if (result == TweakResult.Applied)
                {
                    _pendingRebootIds.Add(id);
                    AppendLog($"\u2705 Applied: {id} — pending reboot/restart to take effect");
                }
                else
                {
                    AppendLog($"\u274C Failed: {id} — {result}");
                }
            }

            PopulateTree();
            RefreshListView();
            UpdateCounters();

            string rebootNote = success > 0 ? " (restart may be needed to activate changes)" : "";
            SetStatus($"Applied {success}/{selected.Count} tweak(s).{rebootNote}");

            if (success > 0)
            {
                AppendLog($"\u26A0 {success} tweak(s) applied — a reboot or restart may be required for changes to take effect.");
            }
        }
        catch (OperationCanceledException) { }
        catch (Exception ex)
        {
            SetStatus($"Error applying tweaks: {ex.Message}");
            AppendLog($"\u274C Error: {ex.Message}");
        }
        finally
        {
            SetBusy(false);
        }
    }

    private async Task RemoveSelectedAsync()
    {
        var selected = GetCheckedTweaks();
        if (selected.Count == 0)
            return;

        bool force = _forceCheck.Checked;

        if (!_logPanel.Visible)
            _logPanel.Visible = true;

        SetBusy(true, $"Removing {selected.Count} tweak(s)...");
        try
        {
            int progress = 0;
            var results = await Task.Run(
                () =>
                {
                    var res = new Dictionary<string, TweakResult>(StringComparer.Ordinal);
                    foreach (var td in selected)
                    {
                        progress++;
                        Invoke(() =>
                        {
                            AppendLog($"[{progress}/{selected.Count}] Removing: {td.Label} ({td.Id})");
                            _progressLabel.Text = $"Removing {progress}/{selected.Count}: {td.Label}";
                        });
                        var r = _engine.Remove(td, forceCorp: force);
                        res[td.Id] = r;
                    }
                    return res;
                },
                _cts.Token
            );

            int success = results.Values.Count(r => r == TweakResult.NotApplied);
            foreach (var (id, result) in results)
            {
                _statusCache[id] = _engine.DetectStatus(_engine.GetTweak(id)!);
                _pendingRebootIds.Remove(id);
                if (result == TweakResult.NotApplied)
                    AppendLog($"\u2705 Removed: {id}");
                else
                    AppendLog($"\u274C Remove failed: {id} — {result}");
            }

            PopulateTree();
            RefreshListView();
            UpdateCounters();
            SetStatus($"Removed {success}/{selected.Count} tweak(s).");
        }
        catch (OperationCanceledException) { }
        catch (Exception ex)
        {
            SetStatus($"Error removing tweaks: {ex.Message}");
            AppendLog($"\u274C Error: {ex.Message}");
        }
        finally
        {
            SetBusy(false);
        }
    }

    // ── Tree / List population ─────────────────────────────────────────────
    private void PopulateTree(bool autoSelect = true)
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
            string icon = CategoryIcons.GetSymbol(CategoryIcons.GetIcon(kvp.Key));
            string nodeText;
            if (_loadedCategories.Contains(kvp.Key))
            {
                int applied = kvp.Value.Count(t => _statusCache.GetValueOrDefault(t.Id) == TweakResult.Applied);
                nodeText = $"{icon} {kvp.Key}  ({applied}/{count})";
            }
            else
            {
                nodeText = $"{icon} {kvp.Key}  ({count})";
            }
            var node = new TreeNode(nodeText) { Tag = kvp.Key };
            node.ForeColor = AppTheme.Fg;
            _treeView.Nodes.Add(node);
            if (kvp.Key == previousCat)
                selectNode = node;
        }
        _treeView.EndUpdate();

        // Auto-size splitter to fit longest category label
        int maxWidth = 0;
        foreach (TreeNode n in _treeView.Nodes)
        {
            var sz = TextRenderer.MeasureText(n.Text, _treeView.Font);
            if (sz.Width > maxWidth)
                maxWidth = sz.Width;
        }
        int desired = maxWidth + 40; // padding for icons + scrollbar
        if (desired >= 180 && desired <= _split.Width / 2)
            _split.SplitterDistance = desired;

        if (selectNode is not null)
        {
            _treeView.SelectedNode = selectNode;
            PopulateList((string)selectNode.Tag!);
        }
        else if (autoSelect && _treeView.Nodes.Count > 0)
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
        string kindSel = _kindCombo.SelectedItem?.ToString() ?? "All Kinds";

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
            filtered = filter switch
            {
                "Applied" => filtered.Where(t => _statusCache.GetValueOrDefault(t.Id) == TweakResult.Applied),
                "Not Applied" => filtered.Where(t => _statusCache.GetValueOrDefault(t.Id) == TweakResult.NotApplied),
                "Default" => filtered.Where(t => _statusCache.GetValueOrDefault(t.Id) == TweakResult.NotApplied),
                "Errors" => filtered.Where(t => _statusCache.GetValueOrDefault(t.Id) == TweakResult.Error),
                "Unknown" => filtered.Where(t => _statusCache.GetValueOrDefault(t.Id) == TweakResult.Unknown),
                _ => filtered,
            };
        }

        // Scope filter
        if (scopeSel != "All Scopes")
        {
            bool wantUser = scopeSel.StartsWith("User", StringComparison.OrdinalIgnoreCase);
            bool wantMachine = scopeSel.StartsWith("Machine", StringComparison.OrdinalIgnoreCase);
            filtered = filtered.Where(t =>
                wantUser ? t.Scope == TweakScope.User
                : wantMachine ? t.Scope == TweakScope.Machine
                : true
            );
        }

        // Kind filter
        if (kindSel != "All Kinds")
        {
            TweakKind? wantKind = kindSel switch
            {
                "Registry" => TweakKind.Registry,
                "PowerShell" => TweakKind.PowerShell,
                "System Cmd" => TweakKind.SystemCommand,
                "Service" => TweakKind.ServiceControl,
                "Sched Task" => TweakKind.ScheduledTask,
                "File Config" => TweakKind.FileConfig,
                "Group Policy" => TweakKind.GroupPolicy,
                "Package Mgr" => TweakKind.PackageManager,
                _ => null,
            };
            if (wantKind.HasValue)
                filtered = filtered.Where(t => t.Kind == wantKind.Value);
        }

        // Search filter
        if (search.Length > 0)
        {
            filtered = filtered.Where(t =>
                t.Label.Contains(search, StringComparison.OrdinalIgnoreCase)
                || t.Id.Contains(search, StringComparison.OrdinalIgnoreCase)
                || t.Description.Contains(search, StringComparison.OrdinalIgnoreCase)
            );
        }

        // Admin-only dimming
        bool isAdmin = Elevation.IsAdmin();

        foreach (var td in filtered)
        {
            bool applicable = !_inapplicableIds.Contains(td.Id);
            var status = _statusCache.GetValueOrDefault(td.Id, TweakResult.Unknown);
            string statusText;
            if (!applicable)
                statusText = "N/A";
            else if (_pendingRebootIds.Contains(td.Id))
                statusText = "\u23F3 Pending";
            else
                statusText = status switch
                {
                    TweakResult.Applied => "Applied",
                    TweakResult.NotApplied => "Default",
                    TweakResult.Error => "Error",
                    _ => "Unknown",
                };

            bool dimmed = !applicable || (!isAdmin && td.NeedsAdmin && td.Scope != TweakScope.User);
            Color itemFg = dimmed ? AppTheme.FgDim : AppTheme.Fg;
            var item = new ListViewItem(td.Label)
            {
                Tag = td,
                UseItemStyleForSubItems = false,
                ForeColor = itemFg,
            };
            Color statusColor = dimmed
                ? AppTheme.FgDim
                : status switch
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
            SetStatus($"Search: \"{search}\" — {_listView.Items.Count} result(s) across all categories.");
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
        string kindSel = _kindCombo.SelectedItem?.ToString() ?? "All Kinds";

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
                wantUser ? t.Scope == TweakScope.User
                : wantMachine ? t.Scope == TweakScope.Machine
                : true
            );
        }

        // Kind filter
        if (kindSel != "All Kinds")
        {
            TweakKind? wantKind = kindSel switch
            {
                "Registry" => TweakKind.Registry,
                "PowerShell" => TweakKind.PowerShell,
                "System Cmd" => TweakKind.SystemCommand,
                "Service" => TweakKind.ServiceControl,
                "Sched Task" => TweakKind.ScheduledTask,
                "File Config" => TweakKind.FileConfig,
                "Group Policy" => TweakKind.GroupPolicy,
                "Package Mgr" => TweakKind.PackageManager,
                _ => null,
            };
            if (wantKind.HasValue)
                all = all.Where(t => t.Kind == wantKind.Value);
        }

        foreach (var td in all)
        {
            bool applicable = !_inapplicableIds.Contains(td.Id);
            var status = _statusCache.GetValueOrDefault(td.Id, TweakResult.Unknown);
            string statusText;
            if (!applicable)
                statusText = "N/A";
            else if (_pendingRebootIds.Contains(td.Id))
                statusText = "\u23F3 Pending";
            else
                statusText = status switch
                {
                    TweakResult.Applied => "Applied",
                    TweakResult.NotApplied => "Default",
                    TweakResult.Error => "Error",
                    _ => "Unknown",
                };
            Color itemFg = applicable ? AppTheme.Fg : AppTheme.FgDim;
            string kindSymbol = CategoryIcons.GetKindSymbol(td.Kind);
            var item = new ListViewItem(td.Label) { ForeColor = itemFg };
            item.SubItems.AddRange([
                kindSymbol,
                statusText,
                td.Scope.ToString(),
                td.NeedsAdmin ? "Yes" : "No",
                td.CorpSafe ? "Yes" : "No",
                $"[{td.Category}] {td.Description}",
            ]);
            item.Tag = td;
            _listView.Items.Add(item);
        }

        _listView.EndUpdate();
    }

    // ── Helpers / Utilities ────────────────────────────────────────────────
    private List<TweakDef> GetCheckedTweaks() => _listView.CheckedItems.Cast<ListViewItem>().Select(i => (TweakDef)i.Tag!).ToList();

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
        if (dlg.ShowDialog(this) != DialogResult.OK)
            return;

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
        if (dlg.ShowDialog(this) != DialogResult.OK)
            return;

        string json = JsonSerializer.Serialize(tweaks.Select(t => t.Id).ToList(), new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(dlg.FileName, json, Encoding.UTF8);
        AppendLog($"Exported {tweaks.Count} IDs to {dlg.FileName}");
        SetStatus($"Exported {tweaks.Count} ID(s) to JSON.");
    }

    private void OnExportReg()
    {
        var tweaks = GetCheckedTweaks();
        if (tweaks.Count == 0)
        {
            MessageBox.Show("No tweaks selected.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        using var dlg = new SaveFileDialog
        {
            Title = "Export as Windows Registry (.reg) file",
            Filter = "Registry file|*.reg",
            FileName = "regilattice-tweaks.reg",
        };
        if (dlg.ShowDialog(this) != DialogResult.OK)
            return;

        var sb = new StringBuilder();
        sb.AppendLine("Windows Registry Editor Version 5.00");
        sb.AppendLine($"; RegiLattice — exported tweaks ({tweaks.Count} selected)");
        sb.AppendLine($"; Generated: {DateTime.Now:yyyy-MM-dd HH:mm}");
        sb.AppendLine();

        foreach (var td in tweaks)
        {
            sb.AppendLine($"; {td.Label} ({td.Id})");
            foreach (var op in td.ApplyOps)
            {
                var regPath = op
                    .Path.Replace("HKLM\\", "HKEY_LOCAL_MACHINE\\", StringComparison.OrdinalIgnoreCase)
                    .Replace("HKCU\\", "HKEY_CURRENT_USER\\", StringComparison.OrdinalIgnoreCase);
                sb.AppendLine($"[{regPath}]");
                sb.AppendLine(FormatRegOp(op));
            }
            sb.AppendLine();
        }

        File.WriteAllText(dlg.FileName, sb.ToString(), Encoding.Unicode);
        AppendLog($"Exported {tweaks.Count} tweaks to {dlg.FileName}");
        SetStatus($"Exported {tweaks.Count} tweak(s) to .REG file.");
    }

    private static string FormatRegOp(RegOp op)
    {
        var quotedName = string.IsNullOrEmpty(op.Name) ? "@" : $"\"{op.Name}\"";
        if (op.Kind == RegOpKind.DeleteValue)
            return $"{quotedName}=-";
        if (op.Kind != RegOpKind.SetValue)
            return $"; {quotedName}=(unsupported op {op.Kind})";

        return op.ValueKind switch
        {
            Microsoft.Win32.RegistryValueKind.DWord when op.Value is int dw => $"{quotedName}=dword:{dw:x8}",
            Microsoft.Win32.RegistryValueKind.String when op.Value is string s => $"{quotedName}=\"{s}\"",
            Microsoft.Win32.RegistryValueKind.QWord when op.Value is long qw =>
                $"{quotedName}=hex(b):{string.Join(",", BitConverter.GetBytes(qw).Select(b => b.ToString("x2")))}",
            _ => $"; {quotedName}=(type {op.ValueKind})",
        };
    }

    private void OnImportJson()
    {
        using var dlg = new OpenFileDialog { Title = "Import tweak IDs from JSON", Filter = "JSON file|*.json" };
        if (dlg.ShowDialog(this) != DialogResult.OK)
            return;

        List<string>? ids;
        try
        {
            string raw = File.ReadAllText(dlg.FileName);
            ids = JsonSerializer.Deserialize<List<string>>(raw);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to parse JSON:\n{ex.Message}", "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        if (ids is null || ids.Count == 0)
        {
            SetStatus("JSON contained no IDs.");
            return;
        }

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

    // ── Package manager dialogs (non-modal so the main GUI stays responsive) ──
    private static void ShowManagerDialog(Form dlg)
    {
        AppTheme.Apply(dlg);
        dlg.FormClosed += (_, _) => dlg.Dispose();
        dlg.Show();
    }

    private void OnOpenScoopManager() => ShowManagerDialog(new ScoopManagerDialog());

    private void OnOpenPSModuleManager() => ShowManagerDialog(new PSModuleManagerDialog());

    private void OnOpenPipManager() => ShowManagerDialog(new PipManagerDialog());

    private void OnOpenWinGetManager() => ShowManagerDialog(new WinGetManagerDialog());

    private void OnOpenChocolateyManager() => ShowManagerDialog(new ChocolateyManagerDialog());

    private void OnOpenToolVersions() => ShowManagerDialog(new ToolVersionsDialog());

    private void OnOpenMarketplace() => ShowManagerDialog(new MarketplaceDialog());

    private void OnAbout()
    {
        bool isCorp = CorporateGuard.IsCorporateNetwork();
        using var dlg = new AboutDialog(_engine.TweakCount, _engine.CategoryCount, isCorp);
        dlg.ShowDialog(this);
    }

    private void OnHardwareInfo()
    {
        try
        {
            string summary = HardwareInfo.Summary();
            var hw = HardwareInfo.DetectHardware();
            string details =
                $"CPU: {hw.Cpu.Name} ({hw.Cpu.PhysicalCores}C / {hw.Cpu.LogicalCores}T, {hw.Cpu.Architecture})\n"
                + $"RAM: {hw.Memory.TotalMb / 1024} GB ({hw.Memory.AvailableMb / 1024} GB available)\n"
                + $"GPU: {string.Join(", ", hw.Gpus.Select(g => $"{g.Name} ({g.AdapterRamMb} MB)"))}\n"
                + $"Disk: {hw.Disk.Drive} — {hw.Disk.TotalGb} GB total, {hw.Disk.FreeGb} GB free\n\n"
                + $"Windows Build: {hw.WindowsBuild}\n"
                + $"Hyper-V: {(hw.HasHyperV ? "Yes" : "No")}   |   WSL: {(hw.HasWsl ? "Yes" : "No")}\n"
                + $"TPM: {(hw.HasTpm ? "Yes" : "No")}   |   Secure Boot: {(hw.HasSecureBoot ? "Yes" : "No")}\n"
                + $"Battery: {(hw.HasBattery ? "Yes (Laptop)" : "No (Desktop)")}\n\n"
                + $"Suggested Profile: {HardwareInfo.SuggestProfile()}";
            MessageBox.Show(details, "Hardware Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to detect hardware:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void UpdateCounters()
    {
        int applied = _statusCache.Values.Count(r => r == TweakResult.Applied);
        int notApplied = _statusCache.Values.Count(r => r == TweakResult.NotApplied);
        int unknown = _statusCache.Values.Count(r => r == TweakResult.Unknown);
        int error = _statusCache.Values.Count(r => r == TweakResult.Error);
        int pending = _pendingRebootIds.Count;
        string pendingStr = pending > 0 ? $"  \u2502  \u23F3 {pending} pending" : "";
        _statusLabel.Text =
            $"\U0001F4CA {_statusCache.Count} tweaks  \u2502  \u2705 {applied}  \u2502  \u274C {notApplied}  \u2502  \u2753 {unknown}  \u2502  \u26A0 {error}{pendingStr}";
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
        if (message is not null)
            SetStatus(message);
    }

    // ── Event handlers ─────────────────────────────────────────────────────
    private async void OnTreeAfterSelect(object? sender, TreeViewEventArgs e)
    {
        if (e.Node?.Tag is not string cat)
            return;

        // Lazy-load status for this category on first select
        if (!_loadedCategories.Contains(cat))
        {
            await LoadCategoryStatusAsync(cat);
            UpdateTreeNode(e.Node, cat);
            UpdateCounters();
        }
        PopulateList(cat);
    }

    /// <summary>Detects tweak status for a single category and merges into the cache.</summary>
    private async Task LoadCategoryStatusAsync(string category)
    {
        var byCategory = _engine.TweaksByCategory();
        if (!byCategory.TryGetValue(category, out var tweaks))
            return;

        SetBusy(true, $"Loading status for {category}...");
        try
        {
            var statuses = await Task.Run(
                () =>
                {
                    var result = new Dictionary<string, TweakResult>(tweaks.Count, StringComparer.Ordinal);
                    foreach (var td in tweaks)
                        result[td.Id] = _engine.DetectStatus(td);
                    return result;
                },
                _cts.Token
            );

            foreach (var (id, status) in statuses)
                _statusCache[id] = status;

            _loadedCategories.Add(category);
        }
        catch (OperationCanceledException) { }
        finally
        {
            SetBusy(false);
        }
    }

    /// <summary>Updates a single tree node's label with the loaded applied/total count.</summary>
    private void UpdateTreeNode(TreeNode node, string category)
    {
        var byCategory = _engine.TweaksByCategory();
        if (!byCategory.TryGetValue(category, out var tweaks))
            return;
        int count = tweaks.Count;
        int applied = tweaks.Count(t => _statusCache.GetValueOrDefault(t.Id) == TweakResult.Applied);
        string icon = CategoryIcons.GetSymbol(CategoryIcons.GetIcon(category));
        node.Text = $"{icon} {category}  ({applied}/{count})";
    }

    private async void OnApplyClicked(object? sender, EventArgs e) => await ApplySelectedAsync();

    private async void OnRemoveClicked(object? sender, EventArgs e) => await RemoveSelectedAsync();

    private async void OnRefreshClicked(object? sender, EventArgs e) => await RefreshStatusAsync();

    private void OnFilterChanged(object? sender, EventArgs e) => RefreshListView();

    private void OnProfileChanged(object? sender, EventArgs e) => PopulateTree();

    private void OnThemeChanged(object? sender, EventArgs e)
    {
        if (_themeCombo.SelectedItem is not string name)
            return;
        AppTheme.SetTheme(name);
        AppIcons.InvalidateCache();
        Icon = AppIcons.AppIcon;
        ApplyTheme();
        PopulateTree();

        // Persist choice
        var cfg = AppConfig.Load();
        cfg.Theme = name;
        cfg.Save();
    }

    private void OnSearchTextChanged(object? sender, EventArgs e) => RefreshListView();

    /// <summary>
    /// Handles column header clicks for sorting and filtering.
    /// Left-click toggles sort order (A→Z, Z→A). Right side opens filter dropdown.
    /// </summary>
    private void OnColumnClick(object? sender, ColumnClickEventArgs e)
    {
        // Determine if user clicked on the filter dropdown zone (right 18px of header)
        var cursorPos = _listView.PointToClient(Cursor.Position);
        int headerRight = 0;
        for (int i = 0; i <= e.Column && i < _listView.Columns.Count; i++)
            headerRight += _listView.Columns[i].Width;
        int headerLeft = headerRight - _listView.Columns[e.Column].Width;
        bool clickedFilterZone = cursorPos.X > headerRight - 20;

        if (clickedFilterZone)
        {
            ShowColumnFilter(e.Column, headerLeft, _listView.Font.Height + 8);
        }
        else
        {
            // Toggle sort order
            if (e.Column == _columnSorter.ColumnIndex)
            {
                _columnSorter.Order = _columnSorter.Order == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
            }
            else
            {
                _columnSorter.ColumnIndex = e.Column;
                _columnSorter.Order = SortOrder.Ascending;
            }
            _listView.Sort();
            _listView.Invalidate();
        }
    }

    /// <summary>Shows a filter dropdown for the given column with unique values from current items.</summary>
    private void ShowColumnFilter(int columnIndex, int xOffset, int yOffset)
    {
        var uniqueValues = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (ListViewItem item in _listView.Items)
        {
            if (columnIndex < item.SubItems.Count)
                uniqueValues.Add(item.SubItems[columnIndex].Text);
        }

        _columnFilters.TryGetValue(columnIndex, out var previousFilter);

        var popup = new ColumnFilterPopup(uniqueValues, previousFilter);
        var screenPos = _listView.PointToScreen(new Point(xOffset, yOffset));
        popup.Location = screenPos;
        popup.ShowDialog(this);

        if (popup.Applied)
        {
            if (popup.SelectedValues.Count == 0)
            {
                // Clear filter — show all
                _columnFilters.Remove(columnIndex);
            }
            else
            {
                _columnFilters[columnIndex] = popup.SelectedValues;
            }
            ApplyColumnFilters();
        }
    }

    /// <summary>Hides/shows ListView items based on active column filters.</summary>
    private void ApplyColumnFilters()
    {
        if (_columnFilters.Count == 0)
        {
            // No filters active — restore normal view
            RefreshListView();
            return;
        }

        _listView.BeginUpdate();
        var toRemove = new List<ListViewItem>();
        foreach (ListViewItem item in _listView.Items)
        {
            bool visible = true;
            foreach (var (col, allowedValues) in _columnFilters)
            {
                if (col < item.SubItems.Count)
                {
                    string cellText = item.SubItems[col].Text;
                    if (!allowedValues.Contains(cellText))
                    {
                        visible = false;
                        break;
                    }
                }
            }
            if (!visible)
                toRemove.Add(item);
        }
        foreach (var item in toRemove)
            _listView.Items.Remove(item);
        _listView.EndUpdate();
    }

    private void OnListViewSelectionChanged(object? sender, EventArgs e)
    {
        if (_listView.FocusedItem?.Tag is TweakDef td)
        {
            var status = _statusCache.GetValueOrDefault(td.Id, TweakResult.Unknown);
            bool isPending = _pendingRebootIds.Contains(td.Id);
            string statusStr;
            if (isPending)
                statusStr = "\u23F3 Pending Reboot";
            else
                statusStr = status switch
                {
                    TweakResult.Applied => "\u2705 Applied",
                    TweakResult.NotApplied => "\u274C Default",
                    TweakResult.Error => "\u26A0 Error",
                    _ => "\u2753 Unknown",
                };

            string scopeStr = td.Scope switch
            {
                TweakScope.User => "\U0001F464 User",
                TweakScope.Machine => "\U0001F5A5 Machine",
                TweakScope.Both => "\U0001F504 Both",
                _ => "?",
            };
            string tags = td.Tags.Count > 0 ? string.Join("  \u2022  ", td.Tags) : "\u2014";
            string keys =
                td.RegistryKeys.Count > 0
                    ? string.Join("; ", td.RegistryKeys.Take(3))
                    : (td.ApplyOps.Count > 0 ? "\U0001F511 Registry operations" : "\u2699 Command-based");
            string kindSymbol = CategoryIcons.GetKindSymbol(td.Kind);

            // Build multi-line detail with full description that wraps to window width
            string desc = td.Description.Length > 0 ? td.Description : "(no description)";
            string pendingNote = isPending ? "\n\u26A0 Restart/reboot needed for this change to take effect." : "";

            _detailLabel.Text =
                $"{kindSymbol} {td.Label}   \u2502   {statusStr}   \u2502   {scopeStr}\n"
                + $"ID: {td.Id}   \u2502   Admin: {(td.NeedsAdmin ? "Yes" : "No")}   \u2502   Corp Safe: {(td.CorpSafe ? "Yes" : "No")}\n"
                + $"Tags: {tags}\n"
                + $"Registry: {keys}\n"
                + $"Description: {desc}"
                + (td.SideEffects.Length > 0 ? $"\n\u26A0 Side Effects: {td.SideEffects}" : "")
                + pendingNote;
            _detailLabel.ForeColor = isPending ? AppTheme.Yellow : AppTheme.Fg;
        }
        else
        {
            _detailLabel.Text = "\U0001F4CB Select a tweak to see details.";
            _detailLabel.ForeColor = AppTheme.FgDim;
        }
    }

    private void OnGlobalKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Control && e.KeyCode == Keys.Enter)
        {
            OnApplyClicked(this, e);
            e.Handled = true;
            return;
        }
        if (e.Control && e.KeyCode == Keys.Delete)
        {
            OnRemoveClicked(this, e);
            e.Handled = true;
            return;
        }
        if (e.KeyCode == Keys.F5)
        {
            OnRefreshClicked(this, e);
            e.Handled = true;
            return;
        }
        if (e.Control && e.KeyCode == Keys.F)
        {
            _searchBox.Focus();
            e.Handled = true;
            return;
        }
        if (e.KeyCode == Keys.Escape)
        {
            _searchBox.Text = "";
            e.Handled = true;
            return;
        }
        if (e.Control && e.KeyCode == Keys.A)
        {
            SelectAllListItems();
            e.Handled = true;
            return;
        }
        if (e.Control && e.KeyCode == Keys.D)
        {
            DeselectAllListItems();
            e.Handled = true;
            return;
        }
        if (e.Control && e.KeyCode == Keys.I)
        {
            InvertListSelection();
            e.Handled = true;
            return;
        }
        if (e.Control && e.KeyCode == Keys.L)
        {
            ToggleLogPanel();
            e.Handled = true;
            return;
        }
        if (e.Control && e.KeyCode == Keys.E)
        {
            _treeView.ExpandAll();
            e.Handled = true;
            return;
        }
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
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            var rect = new Rectangle(1, 1, e.Item.Width - 2, e.Item.Height - 2);

            if (e.Item.Pressed)
            {
                using var brush = new SolidBrush(AppTheme.AccentPressed);
                AppTheme.FillRoundedRect(g, brush, rect, 4);
            }
            else if (e.Item.Selected)
            {
                using var brush = new SolidBrush(AppTheme.AccentHover);
                AppTheme.FillRoundedRect(g, brush, rect, 4);
            }
            else if (e.Item is ToolStripButton { Checked: true })
            {
                using var brush = new SolidBrush(Color.FromArgb(60, AppTheme.Accent));
                AppTheme.FillRoundedRect(g, brush, rect, 4);
                using var pen = new Pen(Color.FromArgb(100, AppTheme.Accent), 1f);
                AppTheme.DrawRoundedRect(g, pen, rect, 4);
            }
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            if (e.Item.Selected)
            {
                var rect = new Rectangle(2, 0, e.Item.Width - 4, e.Item.Height);
                using var brush = new SolidBrush(AppTheme.AccentHover);
                AppTheme.FillRoundedRect(g, brush, rect, 4);
            }
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            e.TextColor = e.Item.Selected ? AppTheme.Accent : AppTheme.Fg;
            base.OnRenderItemText(e);
        }

        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            int y = e.Item.Bounds.Height / 2;
            int margin = 4;
            using var pen = new Pen(AppTheme.Separator, 1f);
            e.Graphics.DrawLine(pen, margin, y, e.Item.Width - margin, y);
        }

        protected override void OnRenderDropDownButtonBackground(ToolStripItemRenderEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            if (e.Item.Selected || e.Item.Pressed)
            {
                var rect = new Rectangle(1, 1, e.Item.Width - 2, e.Item.Height - 2);
                using var brush = new SolidBrush(AppTheme.AccentHover);
                AppTheme.FillRoundedRect(g, brush, rect, 4);
            }
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            // No border — clean flat look
        }

        protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
        {
            // Fill menu image margin with surface color for consistency
            using var brush = new SolidBrush(AppTheme.Surface);
            e.Graphics.FillRectangle(brush, e.AffectedBounds);
        }
    }
}
