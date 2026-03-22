using System.Text;
using System.Text.Json;
using RegiLattice.Core;
using RegiLattice.Core.Models;
using RegiLattice.Core.Services;

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

    // Double-click debounce — prevents ItemCheck firing twice for one double-click sequence.
    private int _lastCheckedIndex = -1;
    private long _lastCheckedTick;

    // Column sorting and filtering state.
    private readonly ListViewColumnSorter _columnSorter = new();
    private readonly Dictionary<int, HashSet<string>> _columnFilters = [];

    // Categories whose tweak status has been loaded (lazy-load on select).
    private readonly HashSet<string> _loadedCategories = new(StringComparer.OrdinalIgnoreCase);

    // Reusable search debounce timer (prevents per-keystroke timer allocations).
    private readonly System.Windows.Forms.Timer _searchDebounceTimer = new() { Interval = 250 };
    private string _lastSearchText = string.Empty;

    // Tooltip that shows score-change preview when hovering over a category node.
    private readonly ToolTip _catScoreTip = new()
    {
        AutoPopDelay = 8000,
        InitialDelay = 600,
        ReshowDelay = 300,
    };

    // Tooltip that shows full tweak details when hovering over a ListView item.
    private readonly ToolTip _listItemTip = new()
    {
        AutoPopDelay = 9000,
        InitialDelay = 400,
        ReshowDelay = 200,
    };
    private string? _lastTooltipId;

    // Profile schedule timer — checks AppConfig.ProfileSchedules every 60 s.
    private readonly System.Windows.Forms.Timer _profileScheduleTimer = new() { Interval = 60_000 };

    // Toast notification service — initialized after _trayIcon is created in InitializeComponent.
    private ToastNotificationService? _toast;

    // ── Construction ───────────────────────────────────────────────────────
    public MainForm()
    {
        SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
        InitializeComponent();
        Text = "RegiLattice";
        Icon = AppIcons.AppIcon;

        // Load saved theme from config (or auto-detect from Windows)
        var cfg = AppConfig.Load();
        string theme = string.IsNullOrEmpty(cfg.Theme) ? AppTheme.DetectSystemTheme() : cfg.Theme;
        AppTheme.SetTheme(theme);

        // Apply saved font size (only if non-default to avoid redundant font creation)
        if (Math.Abs(cfg.FontSize - AppTheme.BaseFontSize) > 0.01f)
            AppTheme.SetFontSize(cfg.FontSize);

        _searchDebounceTimer.Tick += OnSearchDebounceTick;
        _profileScheduleTimer.Tick += OnProfileScheduleTick;
        _treeView.NodeMouseHover += OnTreeNodeScoreHover;
        ApplyTheme();
        AppTheme.Apply3D(this);
        _logPanel.Visible = cfg.ShowLogPanel;
        _logPanel.Height = Math.Max(60, cfg.LogPanelHeight);
        _detailPanel.Height = Math.Max(80, cfg.DetailPanelHeight);

        // Restore splitter position if configured
        if (cfg.RememberSplitter && cfg.SplitterDistance > 0)
            _split.SplitterDistance = Math.Clamp(cfg.SplitterDistance, 150, 600);

        // Persist splitter position on move
        _split.SplitterMoved += (_, _) =>
        {
            var c = AppConfig.Load();
            if (c.RememberSplitter)
            {
                c.SplitterDistance = _split.SplitterDistance;
                c.Save();
            }
        };

        // Start minimized to tray if configured
        if (cfg.LaunchMinimized)
        {
            WindowState = FormWindowState.Minimized;
            _trayIcon.Visible = true;
            ShowInTaskbar = false;
        }

        ApplyAdminRestrictions();

        // Notification services — must be initialised AFTER InitializeComponent so _trayIcon exists.
        _toast = new ToastNotificationService(_trayIcon);
        _ = JumpListService.UpdateAsync();
    }

    // ── Admin-visibility gate ──────────────────────────────────────────────
    /// <summary>
    /// Disables menu items whose primary function requires administrator privileges.
    /// Dialogs that still offer a read-only / view mode without admin (ServiceMgr,
    /// ScheduledTaskMgr, StartupMgr, HostsFileMgr, etc.) are left enabled — they
    /// show their own admin banner and disable write controls internally.
    /// </summary>
    private void ApplyAdminRestrictions()
    {
        if (Elevation.IsAdmin())
            return;

        ToolStripMenuItem[] adminOnly =
        [
            _mnuAppPermissions, // HKLM permission policies — all operations need admin
            _mnuBatterySaver, // powercfg power settings — all operations need admin
            _mnuDnsOverHttps, // DNS-over-HTTPS configuration — all operations need admin
            _mnuDnsSwitcher, // DNS server switching — requires netsh admin
            _mnuNetAdapter, // adapter enable/disable — requires admin
            _mnuNetRepair, // netsh/network repair commands — all need admin
            _mnuPowerPlan, // power plan switching — requires admin
            _mnuPowerScheduler, // power plan scheduler (plan activation needs admin)
            _mnuUsbPower, // USB selective suspend — requires admin
            _mnuWinHealth, // Windows Health (DISM/SFC/bcdedit) — all commands need admin
        ];

        foreach (ToolStripMenuItem item in adminOnly)
            item.Enabled = false;
    }

    // ── Lifecycle ──────────────────────────────────────────────────────────
    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        _monitorTimer.Start();
        _profileScheduleTimer.Start();
        await InitialiseEngineAsync();

        if (FirstRunWizardDialog.ShouldShow())
        {
            using var wiz = new FirstRunWizardDialog();
            if (wiz.ShowDialog(this) == DialogResult.OK)
            {
                // Save the profile choice and dry-run preference from the wizard.
                var cfg = AppConfig.Load();
                if (!string.IsNullOrEmpty(wiz.SelectedProfile))
                    cfg.DefaultProfile = wiz.SelectedProfile;
                cfg.DryRun = wiz.EnableDryRun;
                cfg.Save();
                FirstRunWizardDialog.MarkSeen();
            }
            else
            {
                // User skipped — still mark seen so wizard doesn't reappear.
                FirstRunWizardDialog.MarkSeen();
            }
        }

        if (WhatsNewDialog.ShouldShow())
        {
            using var dlg = new WhatsNewDialog();
            dlg.ShowDialog(this);
            WhatsNewDialog.MarkSeen();
        }
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        _monitorTimer.Stop();
        _profileScheduleTimer.Stop();

        // Warn user about pending reboot tweaks
        if (_pendingRebootIds.Count > 0)
        {
            MessageBox.Show(
                $"{_pendingRebootIds.Count} tweak(s) have been applied but require a reboot to take effect.\n\n"
                    + "Changes will only be fully active after restarting your computer.",
                "Reboot Pending",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        _searchDebounceTimer.Stop();
        _searchDebounceTimer.Tick -= OnSearchDebounceTick;
        _profileScheduleTimer.Stop();
        _profileScheduleTimer.Tick -= OnProfileScheduleTick;

        _trayIcon.Visible = false;
        _cts.Cancel();
        base.OnFormClosing(e);
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        if (WindowState == FormWindowState.Minimized && AppConfig.Load().MinimizeToTray)
        {
            _trayIcon.Visible = true;
            ShowInTaskbar = false;
            Hide();
        }
    }

    private void RestoreFromTray()
    {
        Show();
        ShowInTaskbar = true;
        WindowState = FormWindowState.Normal;
        _trayIcon.Visible = false;
        Activate();
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

        _logBox.BackColor = AppTheme.LogBg;
        _logBox.ForeColor = AppTheme.LogFg;

        _listContextMenu.BackColor = AppTheme.Surface;
        _listContextMenu.ForeColor = AppTheme.Fg;
        _listContextMenu.Renderer = renderer;

        // Detail panel with accent left border
        _detailPanel.BackColor = AppTheme.Surface;
        _detailPanel.Padding = new Padding(8, 6, 8, 6);
        // NOTE: BackColor must be set AFTER ReadOnly; on some Windows versions ReadOnly forces the color back to the system default.
        _detailBox.BackColor = AppTheme.Surface;
        _detailBox.ForeColor = AppTheme.FgDim;
        _detailBox.Font = AppTheme.Regular;
        _detailBox.Refresh();

        // Re-colour tree nodes
        foreach (TreeNode node in _treeView.Nodes)
            node.ForeColor = AppTheme.Fg;

        RefreshMenuImages();
        ResumeLayout(true);
        _listView.Invalidate();
    }

    // ── Owner-draw for dark ListView ───────────────────────────────────────
    private void OnDrawColumnHeader(object? sender, DrawListViewColumnHeaderEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        // Gradient header background (surface → overlay)
        using var gradientBg = new System.Drawing.Drawing2D.LinearGradientBrush(
            e.Bounds,
            AppTheme.Surface,
            AppTheme.Overlay,
            System.Drawing.Drawing2D.LinearGradientMode.Vertical
        );
        g.FillRectangle(gradientBg, e.Bounds);

        // Accent underline (brighter, thicker)
        using var accentPen = new Pen(AppTheme.Accent, 2f);
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
        {
            // Gradient selection highlight: accent tint → overlay
            using var selGradient = new System.Drawing.Drawing2D.LinearGradientBrush(
                e.Bounds,
                Color.FromArgb(40, AppTheme.Accent),
                AppTheme.Overlay,
                System.Drawing.Drawing2D.LinearGradientMode.Horizontal
            );
            g.FillRectangle(selGradient, e.Bounds);
        }
        else
        {
            using var bgBrush = new SolidBrush(bg);
            g.FillRectangle(bgBrush, e.Bounds);
        }

        // Checkbox for first column
        if (e.ColumnIndex == 0)
        {
            int cbSize = 14;
            int cbX = e.Bounds.X + 4;
            int cbY = e.Bounds.Y + (e.Bounds.Height - cbSize) / 2;
            var cbRect = new Rectangle(cbX, cbY, cbSize, cbSize);

            // Checkbox border
            using var borderPen = new Pen(AppTheme.FgDim, 1f);
            AppTheme.DrawRoundedRect(g, borderPen, cbRect, 2);

            if (e.Item.Checked)
            {
                // Filled checkbox with accent
                using var fillBrush = new SolidBrush(AppTheme.Accent);
                var inner = new Rectangle(cbX + 1, cbY + 1, cbSize - 2, cbSize - 2);
                AppTheme.FillRoundedRect(g, fillBrush, inner, 2);

                // Checkmark
                using var checkPen = new Pen(AppTheme.Bg, 2f);
                g.DrawLine(checkPen, cbX + 3, cbY + 7, cbX + 6, cbY + 10);
                g.DrawLine(checkPen, cbX + 6, cbY + 10, cbX + 11, cbY + 4);
            }
        }

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

        int textLeft = e.ColumnIndex == 0 ? 24 : 4; // offset for checkbox in first column
        var textBounds = new Rectangle(e.Bounds.X + textLeft, e.Bounds.Y, e.Bounds.Width - textLeft - 2, e.Bounds.Height);
        string cellText = e.SubItem?.Text ?? "";
        string search = _searchBox.Text.Trim();

        // Search highlight: bold the matched portion in Label (col 0) and Description (col 6)
        if (search.Length > 0 && (e.ColumnIndex == 0 || e.ColumnIndex == 6))
        {
            int matchIdx = cellText.IndexOf(search, StringComparison.OrdinalIgnoreCase);
            if (matchIdx >= 0)
            {
                DrawHighlightedText(g, cellText, search, matchIdx, textBounds, fg);
                return;
            }
        }

        TextRenderer.DrawText(g, cellText, Font, textBounds, fg, TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
    }

    /// <summary>Draws text with the matched search portion highlighted in accent/bold.</summary>
    private void DrawHighlightedText(Graphics g, string text, string search, int matchIdx, Rectangle bounds, Color fg)
    {
        const TextFormatFlags baseFlags = TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.NoPadding;

        string before = text[..matchIdx];
        string match = text.Substring(matchIdx, search.Length);
        string after = text[(matchIdx + search.Length)..];

        using var boldFont = new Font(Font, FontStyle.Bold);
        int x = bounds.X;

        if (before.Length > 0)
        {
            var sz = TextRenderer.MeasureText(g, before, Font, bounds.Size, baseFlags);
            var r = new Rectangle(x, bounds.Y, sz.Width, bounds.Height);
            TextRenderer.DrawText(g, before, Font, r, fg, baseFlags);
            x += sz.Width;
        }

        // Highlight background + bold text for the match
        var matchSz = TextRenderer.MeasureText(g, match, boldFont, bounds.Size, baseFlags);
        var matchRect = new Rectangle(x, bounds.Y, matchSz.Width, bounds.Height);
        using var hlBrush = new SolidBrush(Color.FromArgb(40, AppTheme.Accent));
        g.FillRectangle(hlBrush, x, bounds.Y + 2, matchSz.Width, bounds.Height - 4);
        TextRenderer.DrawText(g, match, boldFont, matchRect, AppTheme.Accent, baseFlags);
        x += matchSz.Width;

        if (after.Length > 0)
        {
            var r = new Rectangle(x, bounds.Y, bounds.Right - x, bounds.Height);
            TextRenderer.DrawText(g, after, Font, r, fg, baseFlags);
        }
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

        // Optionally skip tweaks already detected as applied
        var cfg = AppConfig.Load();
        if (cfg.SkipAppliedOnBatch)
        {
            var statusMap = _engine.StatusMap(parallel: false, ids: selected.Select(t => t.Id).ToList());
            selected = selected.Where(t => !statusMap.TryGetValue(t.Id, out var s) || s != TweakResult.Applied).ToList();
            if (selected.Count == 0)
            {
                SetStatus("All selected tweaks are already applied.");
                return;
            }
        }

        bool force = _forceCheck.Checked;

        // Show log panel so user can watch execution progress
        if (!_logPanel.Visible)
            _logPanel.Visible = true;

        SetBusy(true, $"Applying {selected.Count} tweak(s)...", totalSteps: selected.Count);
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
                            SetProgress(progress);
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
                _toast?.ShowApplyComplete(success, selected.Count);
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

        SetBusy(true, $"Removing {selected.Count} tweak(s)...", totalSteps: selected.Count);
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
                            SetProgress(progress);
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

        // Virtual node: Recently Applied (from TweakHistory)
        if (TweakHistory.Count > 0)
        {
            var recentNode = new TreeNode("\U0001F552 Recently Applied") { Tag = "__recent__", ForeColor = AppTheme.Accent };
            _treeView.Nodes.Add(recentNode);
            if (previousCat == "__recent__")
                _treeView.SelectedNode = recentNode;
        }

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
            string imageKey = AppIcons.CategoryImageKey(kvp.Key);
            var node = new TreeNode(nodeText)
            {
                Tag = kvp.Key,
                ForeColor = AppTheme.Fg,
                ImageKey = imageKey,
                SelectedImageKey = imageKey,
            };
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
        var byCategory = _engine.TweaksByCategory();
        if (!byCategory.TryGetValue(category, out var tweaks))
        {
            _listView.BeginUpdate();
            _listView.Items.Clear();
            _listView.EndUpdate();
            return;
        }

        IEnumerable<TweakDef> source = tweaks;
        string search = _searchBox.Text.Trim();
        if (search.Length > 0)
        {
            source = source.Where(t =>
                t.Label.Contains(search, StringComparison.OrdinalIgnoreCase)
                || t.Id.Contains(search, StringComparison.OrdinalIgnoreCase)
                || t.Description.Contains(search, StringComparison.OrdinalIgnoreCase)
            );
        }

        PopulateListViewFiltered(ApplyFilters(source), showCategory: false);
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
        PopulateListViewFiltered(ApplyFilters(_engine.Search(search)), showCategory: true);
    }

    /// <summary>Shared filter logic for status, scope, and kind combos.</summary>
    private IEnumerable<TweakDef> ApplyFilters(IEnumerable<TweakDef> tweaks)
    {
        string filter = _filterCombo.SelectedItem?.ToString() ?? "All";
        string scopeSel = _scopeCombo.SelectedItem?.ToString() ?? "All Scopes";
        string kindSel = _kindCombo.SelectedItem?.ToString() ?? "All Kinds";

        if (filter != "All")
        {
            if (filter == "Pending")
            {
                tweaks = tweaks.Where(t => _pendingRebootIds.Contains(t.Id));
            }
            else
            {
                TweakResult target = filter switch
                {
                    "Applied" => TweakResult.Applied,
                    "Not Applied" or "Default" => TweakResult.NotApplied,
                    "Errors" => TweakResult.Error,
                    _ => TweakResult.Unknown,
                };
                tweaks = tweaks.Where(t => _statusCache.GetValueOrDefault(t.Id) == target);
            }
        }

        if (scopeSel != "All Scopes")
        {
            bool wantUser = scopeSel.StartsWith("User", StringComparison.OrdinalIgnoreCase);
            bool wantMachine = scopeSel.StartsWith("Machine", StringComparison.OrdinalIgnoreCase);
            tweaks = tweaks.Where(t =>
                wantUser ? t.Scope == TweakScope.User
                : wantMachine ? t.Scope == TweakScope.Machine
                : true
            );
        }

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
                tweaks = tweaks.Where(t => t.Kind == wantKind.Value);
        }

        return tweaks;
    }

    /// <summary>Shared ListView population from a filtered tweak sequence.</summary>
    private void PopulateListViewFiltered(IEnumerable<TweakDef> filtered, bool showCategory)
    {
        bool isAdmin = Elevation.IsAdmin();

        _listView.BeginUpdate();
        _listView.Items.Clear();

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
                ImageKey = AppIcons.CategoryImageKey(td.Category),
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
            string desc = showCategory ? $"[{td.Category}] {td.Description}" : td.Description;
            item.SubItems.Add(new ListViewItem.ListViewSubItem(item, desc));
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

    private void OnOpenWindowsHealth() => ShowManagerDialog(new WindowsHealthDialog());

    private void OnOpenNetworkTools()
    {
        using var dlg = new NetworkToolsDialog();
        AppTheme.Apply(dlg);
        dlg.ShowDialog(this);
    }

    private void OnOpenStartupManager()
    {
        using var dlg = new StartupManagerDialog();
        AppTheme.Apply(dlg);
        dlg.ShowDialog(this);
    }

    private void OnOpenServiceManager()
    {
        using var dlg = new ServiceManagerDialog();
        AppTheme.Apply(dlg);
        dlg.ShowDialog(this);
    }

    private void OnOpenScheduledTaskManager()
    {
        using var dlg = new ScheduledTaskManagerDialog();
        AppTheme.Apply(dlg);
        dlg.ShowDialog(this);
    }

    private void OnOpenProfileScheduler()
    {
        using var dlg = new ProfileSchedulerDialog();
        AppTheme.Apply(dlg);
        dlg.ShowDialog(this);
    }

    private void OnOpenPowerPlan()
    {
        using var dlg = new PowerPlanDialog();
        AppTheme.Apply(dlg);
        dlg.ShowDialog(this);
    }

    private void OnOpenPrivacyDashboard()
    {
        using var dlg = new PrivacyDashboardDialog();
        AppTheme.Apply(dlg);
        dlg.ShowDialog(this);
    }

    private void OnOpenContextMenuManager() => ShowManagerDialog(new ContextMenuManagerDialog());

    private void OnOpenHostsFileManager() => ShowManagerDialog(new HostsFileManagerDialog());

    private void OnOpenTempFileCleaner()
    {
        using var dlg = new TempFileCleanerDialog();
        AppTheme.Apply(dlg);
        dlg.ShowDialog(this);
    }

    private void OnOpenInstalledApps() => ShowManagerDialog(new InstalledAppsDialog());

    private void OnOpenPowerScheduler() => ShowManagerDialog(new PowerSchedulerDialog());

    private void OnOpenSleepTimer() => ShowManagerDialog(new SleepTimerDialog());

    private void OnOpenBatterySaver() => ShowManagerDialog(new BatterySaverDialog());

    private void OnOpenUsbPower() => ShowManagerDialog(new UsbPowerDialog());

    private void OnOpenAdRemoval() => ShowManagerDialog(new AdRemovalWizardDialog());

    private void OnOpenTelemetryDashboard() => ShowManagerDialog(new TelemetryDashboardDialog());

    private void OnOpenAppPermissions() => ShowManagerDialog(new AppPermissionsDialog());

    private void OnOpenDnsOverHttps() => ShowManagerDialog(new DnsOverHttpsDialog());

    private void OnOpenNetworkRepair() => ShowManagerDialog(new NetworkRepairDialog());

    private void OnOpenDnsSwitcher() => ShowManagerDialog(new DnsSwitcherDialog());

    private void OnOpenNetworkAdapter() => ShowManagerDialog(new NetworkAdapterDialog());

    private void OnOpenWiFiProfiles() => ShowManagerDialog(new WiFiProfileDialog());

    private void OnOpenFirewallRules() => ShowManagerDialog(new FirewallRulesDialog());

    private void OnOpenProxyConfig() => ShowManagerDialog(new ProxyConfigDialog());

    private void OnOpenShellExtensions() => ShowManagerDialog(new ShellExtensionDialog());

    private void OnOpenBootAnalyzer() => ShowManagerDialog(new BootTimeAnalyzerDialog());

    private void OnOpenWuControl() => ShowManagerDialog(new WindowsUpdateControlDialog());

    private void OnOpenNotifMgr() => ShowManagerDialog(new NotificationManagerDialog());

    private void OnOpenBrowserCache() => ShowManagerDialog(new BrowserCacheCleanerDialog());

    private void OnOpenDriverChecker() => ShowManagerDialog(new DriverUpdateCheckerDialog());

    private void OnOpenWakeOnLan() => ShowManagerDialog(new WakeOnLanDialog());

    private void OnOpenBrightness() => ShowManagerDialog(new BrightnessSchedulerDialog());

    // Sprint 41 — system monitor tools
    private void OnOpenMemoryCleaner() => ShowManagerDialog(new MemoryCleanerDialog());

    private void OnOpenDiskSpace() => ShowManagerDialog(new DiskSpaceDialog());

    private void OnOpenPortScanner() => ShowManagerDialog(new PortScannerDialog());

    private void OnOpenBatteryHealth() => ShowManagerDialog(new BatteryHealthDialog());

    // Sprint 42 — hardware & network monitors
    private void OnOpenHardwareTemp() => ShowManagerDialog(new HardwareTemperatureDialog());

    private void OnOpenNetBandwidth() => ShowManagerDialog(new NetworkBandwidthDialog());

    private void OnOpenMacAddress() => ShowManagerDialog(new MacAddressDialog());

    private void OnOpenMarketplace() => ShowManagerDialog(new MarketplaceDialog());

    private void OnOpenSmartScan() => ShowManagerDialog(new SmartScanDialog(_engine, _statusCache.Count > 0 ? _statusCache : null));

    private void OnOpenProfileCompare() => ShowManagerDialog(new ProfileCompareDialog(_engine));

    private void OnOpenDepGraph() => ShowManagerDialog(new DependencyGraphDialog(_engine));

    private void OnExportComplianceReport()
    {
        using var sfd = new SaveFileDialog
        {
            Title = "Export Compliance Report",
            Filter = "HTML file (*.html)|*.html",
            FileName = $"regilattice-compliance-{DateTime.Now:yyyyMMdd}.html",
        };
        if (sfd.ShowDialog(this) != DialogResult.OK)
            return;
        try
        {
            ComplianceReportExporter.ExportHtml(_engine, _statusCache.Count > 0 ? _statusCache : null, sfd.FileName);
            AppendLog($"Compliance report saved: {sfd.FileName}");
            MessageBox.Show($"Report saved:\n{sfd.FileName}", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (IOException ex)
        {
            MessageBox.Show($"Export failed: {ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void OnOpenProfileWizard()
    {
        using var dlg = new ProfileWizardDialog();
        if (dlg.ShowDialog(this) == DialogResult.OK)
            AppendLog($"Profile wizard: '{dlg.RecommendedProfile}' saved as default profile.");
    }

    private void OnCheckForUpdates() => ShowManagerDialog(new UpdateCheckerDialog());

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
        int applied = 0,
            notApplied = 0,
            unknown = 0,
            error = 0;
        foreach (var r in _statusCache.Values)
        {
            switch (r)
            {
                case TweakResult.Applied:
                    applied++;
                    break;
                case TweakResult.NotApplied:
                    notApplied++;
                    break;
                case TweakResult.Error:
                    error++;
                    break;
                default:
                    unknown++;
                    break;
            }
        }
        int pending = _pendingRebootIds.Count;
        int selected = _listView.CheckedItems.Count;
        string selectedStr = selected > 0 ? $"  \u2502  \u2611 {selected} selected" : "";
        string pendingStr = pending > 0 ? $"  \u2502  \u23F3 {pending} pending" : "";
        _statusLabel.Text =
            $"\U0001F4CA {_statusCache.Count} tweaks  \u2502  \u2705 {applied}  \u2502  \u274C {notApplied}  \u2502  \u2753 {unknown}  \u2502  \u26A0 {error}{selectedStr}{pendingStr}";
    }

    private void SetStatus(string message)
    {
        _progressLabel.Text = message;
        AppendLog(message);
    }

    private void OnMonitorTimerTick(object? sender, EventArgs e)
    {
        int cpu = _sysMonitor.GetCpuUsagePercent();
        var (usedMb, totalMb, memPct) = SystemMonitor.GetMemoryUsage();
        _cpuLabel.Text = $"CPU: {cpu}%";
        _memLabel.Text = $"RAM: {usedMb / 1024.0:F1}/{totalMb / 1024.0:F0} GB ({memPct}%)";

        // Phase 2 #17 — network connectivity indicator
        bool isOnline = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
        _netLabel.Text = isOnline ? "Net: \u2713" : "Net: \u2717";

        // Phase 2 #14 — update tray icon tooltip with RAM %
        _trayIcon.Text = $"RegiLattice \u2014 RAM: {memPct}%";

        if (AppConfig.Load().MonitorColorCoded)
        {
            _cpuLabel.ForeColor = cpu switch
            {
                >= 80 => Color.FromArgb(255, 85, 85), // red
                >= 60 => Color.FromArgb(255, 184, 108), // amber
                _ => Color.FromArgb(80, 250, 123), // green (Dracula-ish)
            };
            _memLabel.ForeColor = memPct switch
            {
                >= 80 => Color.FromArgb(255, 85, 85),
                >= 60 => Color.FromArgb(255, 184, 108),
                _ => Color.FromArgb(80, 250, 123),
            };
            _netLabel.ForeColor = isOnline ? Color.FromArgb(80, 250, 123) : Color.FromArgb(255, 85, 85);
        }
        else
        {
            _cpuLabel.ForeColor = _statusLabel.ForeColor;
            _memLabel.ForeColor = _statusLabel.ForeColor;
            _netLabel.ForeColor = _statusLabel.ForeColor;
        }

        // Phase 2 #13 — auto memory clean when RAM exceeds configured threshold
        var cfg = AppConfig.Load();
        if (cfg.AutoCleanMemoryThreshold > 0 && memPct >= cfg.AutoCleanMemoryThreshold)
        {
            _ = Task.Run(static () =>
            {
                try
                {
                    // Purge working sets via EmptyWorkingSet (same as MemoryCleanerDialog)
                    foreach (var proc in System.Diagnostics.Process.GetProcesses())
                    {
                        try
                        {
                            proc.MinWorkingSet = proc.MinWorkingSet;
                        }
                        catch
                        { /* ignore access-denied processes */
                        }
                        finally
                        {
                            proc.Dispose();
                        }
                    }
                }
                catch
                { /* ignore */
                }
            });
        }
    }

    // Sprint 51 §3 — check profile schedules every 60 s and auto-switch if due.
    private void OnProfileScheduleTick(object? sender, EventArgs e)
    {
        try
        {
            var cfg = AppConfig.Load();
            if (cfg.ProfileSchedules.Count == 0)
                return;

            string now = DateTime.Now.ToString("HH:mm");
            foreach (var schedule in cfg.ProfileSchedules)
            {
                if (!schedule.Enabled)
                    continue;
                if (schedule.Trigger != "daily" || schedule.Time != now)
                    continue;

                // Switch the profile filter in the combo box on the UI thread.
                if (_profileCombo.Items.Contains(schedule.Profile))
                {
                    _profileCombo.SelectedItem = schedule.Profile;
                    AppendLog($"[Profile Scheduler] Auto-switched to profile '{schedule.Profile}' at {now}.");
                }
            }
        }
        catch
        { /* non-fatal — ignore any config I/O errors */
        }
    }

    private void SetBusy(bool busy, string? message = null, int totalSteps = 0)
    {
        _progressBar.Visible = busy;
        if (busy && totalSteps > 0)
        {
            _progressBar.Style = ProgressBarStyle.Blocks;
            _progressBar.Minimum = 0;
            _progressBar.Maximum = totalSteps;
            _progressBar.Value = 0;
        }
        else
        {
            _progressBar.Style = busy ? ProgressBarStyle.Marquee : ProgressBarStyle.Blocks;
        }
        _btnApply.Enabled = !busy;
        _btnRemove.Enabled = !busy;
        _btnRefresh.Enabled = !busy;
        _btnUndoLast.Enabled = !busy && TweakHistory.Count > 0;
        if (message is not null)
            SetStatus(message);
        else if (!busy)
            _progressLabel.Text = string.Empty;
    }

    private void SetProgress(int current)
    {
        if (_progressBar.Style == ProgressBarStyle.Blocks && current <= _progressBar.Maximum)
            _progressBar.Value = current;
    }

    // ── Event handlers ─────────────────────────────────────────────────────
    /// <summary>
    /// Shows a health-score preview tooltip when the mouse hovers over a category node.
    /// Computes the before/after score assuming all tweaks in the category are applied.
    /// </summary>
    private void OnTreeNodeScoreHover(object? sender, TreeNodeMouseHoverEventArgs e)
    {
        if (e.Node?.Tag is not string cat || cat == "__recent__")
            return;

        var svc = new HealthScoreService(_engine);
        var (before, after) = svc.PreviewCategoryImpact(cat, _statusCache);

        int deltaOver = after.Overall - before.Overall;
        if (deltaOver == 0)
        {
            _catScoreTip.SetToolTip(_treeView, $"{cat}: All tweaks already applied — no score change.");
            return;
        }

        int deltaPriv = after.Privacy - before.Privacy;
        int deltaPerf = after.Performance - before.Performance;
        int deltaSec = after.Security - before.Security;
        int deltaStab = after.Stability - before.Stability;

        var sb = new StringBuilder();
        sb.AppendLine($"Applying all '{cat}' tweaks would change your health score:");
        if (deltaPriv != 0)
            sb.AppendLine($"  Privacy:     {deltaPriv:+0;-0} pts");
        if (deltaPerf != 0)
            sb.AppendLine($"  Performance: {deltaPerf:+0;-0} pts");
        if (deltaSec != 0)
            sb.AppendLine($"  Security:    {deltaSec:+0;-0} pts");
        if (deltaStab != 0)
            sb.AppendLine($"  Stability:   {deltaStab:+0;-0} pts");
        sb.Append($"  Overall:     {deltaOver:+0;-0} pts  ({before.Overall}% → {after.Overall}%)");
        _catScoreTip.SetToolTip(_treeView, sb.ToString());
    }

    private async void OnTreeAfterSelect(object? sender, TreeViewEventArgs e)
    {
        if (e.Node?.Tag is not string cat)
            return;

        // Virtual category: Recently Applied
        if (cat == "__recent__")
        {
            PopulateRecentlyApplied();
            return;
        }

        // Lazy-load status for this category on first select
        if (!_loadedCategories.Contains(cat))
        {
            await LoadCategoryStatusAsync(cat);
            UpdateTreeNode(e.Node, cat);
            UpdateCounters();
        }
        PopulateList(cat);
    }

    /// <summary>Populates the ListView with recently applied tweaks from TweakHistory.</summary>
    private void PopulateRecentlyApplied()
    {
        var recent = TweakHistory.Recent(50);
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var tweaks = new List<TweakDef>();

        foreach (var entry in recent)
        {
            if (!seen.Add(entry.TweakId))
                continue;
            var td = _engine.GetTweak(entry.TweakId);
            if (td is not null)
                tweaks.Add(td);
        }

        PopulateListViewFiltered(ApplyFilters(tweaks), showCategory: true);
        SetStatus($"Recently Applied — {tweaks.Count} unique tweak(s)");
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

    private async Task OnUndoLastAsync()
    {
        var recent = TweakHistory.Recent(1);
        if (recent.Count == 0)
        {
            SetStatus("Nothing to undo.");
            return;
        }

        HistoryEntry last = recent[0];
        TweakDef? tweak = _engine.AllTweaks().FirstOrDefault(t => t.Id.Equals(last.TweakId, StringComparison.OrdinalIgnoreCase));

        if (tweak is null)
        {
            SetStatus($"Tweak '{last.TweakId}' not found \u2014 cannot undo.");
            AppendLog($"\u26A0 Undo skipped: tweak '{last.TweakId}' is not registered.");
            return;
        }

        bool force = _forceCheck.Checked;

        if (!_logPanel.Visible)
            _logPanel.Visible = true;

        string inverseLabel = last.Action.Equals("apply", StringComparison.OrdinalIgnoreCase) ? "removing" : "re-applying";
        AppendLog($"\u21A9 Undoing last op \u2014 {inverseLabel} '{tweak.Label}' ({tweak.Id})");
        SetBusy(true, $"Undoing: {tweak.Label}\u2026", totalSteps: 1);
        try
        {
            var result = await Task.Run(
                () =>
                    last.Action.Equals("apply", StringComparison.OrdinalIgnoreCase)
                        ? _engine.Remove(tweak, forceCorp: force)
                        : _engine.Apply(tweak, forceCorp: force),
                _cts.Token
            );

            _statusCache[tweak.Id] = _engine.DetectStatus(tweak);
            PopulateTree();
            RefreshListView();
            UpdateCounters();

            string icon = result is TweakResult.Applied or TweakResult.NotApplied ? "\u2705" : "\u26A0";
            AppendLog($"{icon} Undo result for '{tweak.Id}': {result}");
            SetStatus($"Undo complete: {tweak.Label} \u2192 {result}");
        }
        catch (OperationCanceledException) { }
        catch (Exception ex)
        {
            SetStatus($"Undo error: {ex.Message}");
            AppendLog($"\u274C Undo error: {ex.Message}");
        }
        finally
        {
            SetBusy(false);
            _btnUndoLast.Enabled = TweakHistory.Count > 0;
        }
    }

    private async void OnRefreshClicked(object? sender, EventArgs e) => await RefreshStatusAsync();

    private void OnFilterChanged(object? sender, EventArgs e) => RefreshListView();

    private void OnProfileChanged(object? sender, EventArgs e) => PopulateTree();

    private void OnOpenPreferences()
    {
        var cfg = AppConfig.Load();
        using var dlg = new PreferencesDialog(cfg);
        if (dlg.ShowDialog(this) != DialogResult.OK)
            return;

        dlg.ApplyToConfig();

        // Apply theme if changed
        if (dlg.ThemeChanged)
        {
            AppTheme.SetTheme(cfg.Theme);
            AppIcons.InvalidateCache();
            Icon = AppIcons.AppIcon;
        }

        // Apply font size if changed — requires full theme re-apply
        if (dlg.FontSizeChanged)
            AppTheme.SetFontSize(cfg.FontSize);

        // Re-apply theme colours and fonts to all controls
        if (dlg.ThemeChanged || dlg.FontSizeChanged)
        {
            ApplyTheme();
            PopulateTree();
        }

        // Apply panel dimensions
        _detailPanel.Height = Math.Max(80, cfg.DetailPanelHeight);
        _logPanel.Height = Math.Max(60, cfg.LogPanelHeight);
        _logPanel.Visible = cfg.ShowLogPanel;

        // Apply monitor timer
        if (cfg.StatusBarMonitor)
            _monitorTimer.Start();
        else
            _monitorTimer.Stop();

        // Sync force checkbox
        _forceCheck.Checked = cfg.ForceCorp;
    }

    /// <summary>Re-assign menu item images after the AppIcons bitmap cache has been invalidated.</summary>
    private void RefreshMenuImages()
    {
        // Top-level and sub-group headers
        _mnuTools.Image = AppIcons.ToolVersionsMenuBitmap;
        _mnuPkgMgr.Image = AppIcons.MarketplaceMenuBitmap;
        _subSysDiag.Image = AppIcons.ToolVersionsMenuBitmap;
        _subSysMgmt.Image = AppIcons.ServiceMenuBitmap;
        _subPower.Image = AppIcons.PowerMenuBitmap;
        _subPrivSec.Image = AppIcons.PrivacyMenuBitmap;
        _subNetwork.Image = AppIcons.NetworkMenuBitmap;
        _subCleanup.Image = AppIcons.CleanupMenuBitmap;

        // Package manager items
        _mnuScoopMgr.Image = AppIcons.ScoopMenuBitmap;
        _mnuPsMgr.Image = AppIcons.PSModuleMenuBitmap;
        _mnuPipMgr.Image = AppIcons.PipMenuBitmap;
        _mnuWinGetMgr.Image = AppIcons.WinGetMenuBitmap;
        _mnuChocoMgr.Image = AppIcons.ChocolateyMenuBitmap;
        _mnuMarketplace.Image = AppIcons.MarketplaceMenuBitmap;

        // Tools items
        _mnuToolVersions.Image = AppIcons.ToolVersionsMenuBitmap;
        _mnuWinHealth.Image = AppIcons.WindowsHealthMenuBitmap;
        _mnuNetTools.Image = AppIcons.NetworkMenuBitmap;
        _mnuStartupMgr.Image = AppIcons.StartupMenuBitmap;
        _mnuServiceMgr.Image = AppIcons.ServiceMenuBitmap;
        _mnuSchedTaskMgr.Image = AppIcons.ServiceMenuBitmap;
        _mnuPowerPlan.Image = AppIcons.PerformanceMenuBitmap;
        _mnuPrivacyDash.Image = AppIcons.PrivacyMenuBitmap;
        _mnuContextMenuMgr.Image = AppIcons.ExplorerMenuBitmap;
        _mnuHostsFileMgr.Image = AppIcons.NetworkMenuBitmap;
        _mnuTempCleaner.Image = AppIcons.CleanupMenuBitmap;
        _mnuInstalledApps.Image = AppIcons.MarketplaceMenuBitmap;
        _mnuPowerScheduler.Image = AppIcons.PerformanceMenuBitmap;
        _mnuSleepTimer.Image = AppIcons.PerformanceMenuBitmap;
        _mnuBatterySaver.Image = AppIcons.PerformanceMenuBitmap;
        _mnuUsbPower.Image = AppIcons.ServiceMenuBitmap;
        _mnuAdRemoval.Image = AppIcons.PrivacyMenuBitmap;
        _mnuTelemetryDash.Image = AppIcons.PrivacyMenuBitmap;
        _mnuAppPermissions.Image = AppIcons.PrivacyMenuBitmap;
        _mnuDnsOverHttps.Image = AppIcons.NetworkMenuBitmap;
        _mnuNetRepair.Image = AppIcons.NetworkMenuBitmap;
        _mnuDnsSwitcher.Image = AppIcons.NetworkMenuBitmap;
        _mnuNetAdapter.Image = AppIcons.NetworkMenuBitmap;
        _mnuWiFiProfiles.Image = AppIcons.NetworkMenuBitmap;
        _mnuFirewallRules.Image = AppIcons.NetworkMenuBitmap;
        _mnuProxyConfig.Image = AppIcons.NetworkMenuBitmap;
        _mnuShellExtensions.Image = AppIcons.ExplorerMenuBitmap;
        _mnuBootAnalyzer.Image = AppIcons.PerformanceMenuBitmap;
        _mnuWuControl.Image = AppIcons.WindowsHealthMenuBitmap;
        _mnuNotifMgr.Image = AppIcons.PrivacyMenuBitmap;
        _mnuBrowserCache.Image = AppIcons.CleanupMenuBitmap;
        _mnuDriverChecker.Image = AppIcons.PerformanceMenuBitmap;
        _mnuWakeOnLan.Image = AppIcons.NetworkMenuBitmap;
        _mnuBrightness.Image = AppIcons.PerformanceMenuBitmap;
        _mnuMemoryCleaner.Image = AppIcons.PerformanceMenuBitmap;
        _mnuDiskSpace.Image = AppIcons.PerformanceMenuBitmap;
        _mnuPortScan.Image = AppIcons.NetworkMenuBitmap;
        _mnuBatteryHealth.Image = AppIcons.PerformanceMenuBitmap;
        _mnuHwTempMon.Image = AppIcons.ThermometerMenuBitmap;
        _mnuNetBandwidth.Image = AppIcons.BandwidthMenuBitmap;
        _mnuMacAddress.Image = AppIcons.MacAddressMenuBitmap;
        _mnuSmartScan.Image = AppIcons.WizardMenuBitmap;
        _mnuProfileCompare.Image = AppIcons.WizardMenuBitmap;
        _mnuDepGraph.Image = AppIcons.WizardMenuBitmap;
        _mnuProfileWizard.Image = AppIcons.WizardMenuBitmap;
    }

    private void OnSearchDebounceTick(object? sender, EventArgs e)
    {
        _searchDebounceTimer.Stop();

        string search = _searchBox.Text.Trim();
        if (string.Equals(search, _lastSearchText, StringComparison.Ordinal))
            return;

        _lastSearchText = search;
        RefreshListView();
    }

    private void OnSearchTextChanged(object? sender, EventArgs e)
    {
        _searchClear.Visible = _searchBox.Text.Length > 0;

        _searchDebounceTimer.Stop();
        _searchDebounceTimer.Start();
    }

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
        // Prefer SelectedItems[0] over FocusedItem — FocusedItem can be null mid-selection
        var selected = _listView.SelectedItems.Count > 0 ? _listView.SelectedItems[0] : _listView.FocusedItem;
        if (selected?.Tag is TweakDef td)
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
            string expected = td.GetExpectedResult();

            // Before/After registry value preview (first 2 SetValue ops only)
            string beforeAfter = "";
            if (td.Kind == TweakKind.Registry || td.Kind == TweakKind.GroupPolicy)
            {
                var setOps = td.ApplyOps.Where(o => o.Kind == RegOpKind.SetValue).Take(2).ToList();
                if (setOps.Count > 0)
                {
                    var lines = new System.Text.StringBuilder("\n\U0001F50D Before \u2192 After:");
                    foreach (RegOp op in setOps)
                    {
                        object? curVal = null;
                        try
                        {
                            curVal = Microsoft.Win32.Registry.GetValue(op.Path, op.Name, null);
                        }
                        catch (Exception)
                        { /* ignore — registry access requires admin or key may not exist */
                        }
                        string curStr = curVal is null ? "(not set)" : curVal.ToString() ?? "(null)";
                        string willStr = op.Value?.ToString() ?? "(null)";
                        lines.Append($"\n  {op.Name}: {curStr}  \u2192  {willStr}");
                    }
                    beforeAfter = lines.ToString();
                }
            }

            // Use RTF approach for reliable rendering with dark backgrounds
            UpdateDetailBox(
                $"{kindSymbol} {td.Label}   \u2502   {statusStr}   \u2502   {scopeStr}\n"
                    + $"ID: {td.Id}   \u2502   Admin: {(td.NeedsAdmin ? "Yes" : "No")}   \u2502   Corp Safe: {(td.CorpSafe ? "Yes" : "No")}\n"
                    + $"Tags: {tags}\n"
                    + $"Registry: {keys}\n"
                    + $"Description: {desc}\n"
                    + $"\U0001F4CB Expected: {expected}"
                    + beforeAfter
                    + (td.SideEffects.Length > 0 ? $"\n\u26A0 Side Effects: {td.SideEffects}" : "")
                    + pendingNote,
                isPending ? AppTheme.Yellow : AppTheme.Fg
            );
        }
        else
        {
            UpdateDetailBox("\U0001F4CB Select a tweak to see details.", AppTheme.FgDim);
        }
    }

    /// <summary>
    /// Safely updates the detail RichTextBox, ensuring BackColor is always correct
    /// (ReadOnly = true on some Windows versions can override BackColor to gray).
    /// </summary>
    private void UpdateDetailBox(string text, Color foreColor)
    {
        _detailBox.SuspendLayout();
        _detailBox.BackColor = AppTheme.Surface;
        _detailBox.ForeColor = foreColor;
        _detailBox.Text = text;
        _detailBox.ResumeLayout(false);
        _detailBox.Invalidate();
        _detailPanel.Invalidate();
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
        if (e.Control && e.KeyCode == Keys.W)
        {
            _treeView.CollapseAll();
            e.Handled = true;
            return;
        }
    }

    // ── ListView hover tooltip ─────────────────────────────────────────────

    // Tooltip text for each column header (index-aligned with _listView.Columns).
    private static readonly string[] ColumnHeaderTooltips =
    [
        "Tweak label — click header to sort alphabetically",
        "Operation kind: Registry · PowerShell · System Cmd · Service · Sched Task · File Config · Group Policy · Package Mgr",
        "Current state: Applied ✓ · Not Applied ✗ · Unknown ? · Error !",
        "Registry scope: USER = HKCU (current user only) · MACHINE = HKLM (all users) · BOTH = both hives",
        "Admin required: Yes = needs elevated privileges · No = can run as standard user",
        "Corp safe: Yes = safe on managed/domain-joined machines · No = may conflict with Group Policy",
        "Full description of what the tweak does and the expected outcome",
    ];

    private void OnListViewMouseMove(object? sender, MouseEventArgs e)
    {
        // Column header zone — show per-column explanation tooltip.
        int headerHeight = _listView.Items.Count > 0 ? _listView.GetItemRect(0).Top : (AppTheme.Regular.Height + 8);

        if (e.Y >= 0 && e.Y < headerHeight)
        {
            int colIdx = GetColumnAtX(e.X);
            string colKey = $"__hdr_{colIdx}";
            if (colKey == _lastTooltipId)
                return;
            _lastTooltipId = colKey;
            string tip = colIdx >= 0 && colIdx < ColumnHeaderTooltips.Length ? ColumnHeaderTooltips[colIdx] : "";
            _listItemTip.SetToolTip(_listView, tip);
            return;
        }

        var hit = _listView.HitTest(e.X, e.Y);
        if (hit.Item?.Tag is TweakDef td)
        {
            if (td.Id == _lastTooltipId)
                return;
            _lastTooltipId = td.Id;
            _listItemTip.SetToolTip(_listView, BuildItemTooltip(td));
        }
        else
        {
            if (_lastTooltipId is null)
                return;
            _lastTooltipId = null;
            _listItemTip.SetToolTip(_listView, string.Empty);
        }
    }

    /// <summary>Returns the zero-based column index at the given x coordinate, or -1 if past the last column.</summary>
    private int GetColumnAtX(int x)
    {
        int cumX = 0;
        for (int i = 0; i < _listView.Columns.Count; i++)
        {
            cumX += _listView.Columns[i].Width;
            if (x < cumX)
                return i;
        }
        return -1;
    }

    private static string BuildItemTooltip(TweakDef td)
    {
        var sb = new System.Text.StringBuilder();
        sb.Append('[').Append(td.Category).Append("]  ").AppendLine(td.Label);
        if (!string.IsNullOrWhiteSpace(td.Description))
        {
            string desc = td.Description.Length > 140 ? string.Concat(td.Description.AsSpan(0, 137), "...") : td.Description;
            sb.AppendLine(desc);
        }
        string tagsLine = td.Tags.Count > 0 ? string.Join(", ", td.Tags) : "none";
        sb.AppendLine($"Tags: {tagsLine}");
        string scope = td.Scope switch
        {
            TweakScope.User => "User",
            TweakScope.Machine => "Machine",
            _ => "Both",
        };
        sb.Append($"Admin: {(td.NeedsAdmin ? "Yes" : "No")}  |  Scope: {scope}  |  Corp-safe: {(td.CorpSafe ? "Yes" : "No")}");
        if (!string.IsNullOrWhiteSpace(td.SourceUrl))
            sb.AppendLine().Append("Source: ").Append(td.SourceUrl);
        return sb.ToString();
    }

    private void OnListViewMouseDoubleClick(object? sender, MouseEventArgs e)
    {
        var hit = _listView.HitTest(e.X, e.Y);
        if (hit.Item is null)
            return;

        // If the double-click landed on the native checkbox (StateImage), the first click
        // of the double-click sequence already fired ItemCheck and toggled it — skip here
        // to avoid toggling a second time (the flicker bug).
        if ((hit.Location & ListViewHitTestLocations.StateImage) != 0)
            return;

        hit.Item.Checked = !hit.Item.Checked;
    }

    private void OnListViewItemCheck(object? sender, ItemCheckEventArgs e)
    {
        long now = Environment.TickCount64;
        if (e.Index == _lastCheckedIndex && (now - _lastCheckedTick) < SystemInformation.DoubleClickTime)
        {
            // Cancel the second checkbox toggle that fires during a double-click.
            e.NewValue = e.CurrentValue;
            _lastCheckedIndex = -1;
            return;
        }
        _lastCheckedIndex = e.Index;
        _lastCheckedTick = now;
        this.BeginInvoke(UpdateCounters);
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
