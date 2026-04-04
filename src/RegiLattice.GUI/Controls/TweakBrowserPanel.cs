#nullable enable
using System.Drawing.Drawing2D;
using RegiLattice.Core;
using RegiLattice.Core.Models;

namespace RegiLattice.GUI.Controls;

/// <summary>
/// Tweak browser panel displayed in the "Tweaks" section.
/// Left: category tree. Right: scrollable list of TweakCard rows
/// with toggle switches for quick ON/OFF.
/// Advanced columns (Kind/Scope/Admin/CorpSafe) are hidden by default; the
/// original full ListView is still accessible via "Advanced View" button.
/// </summary>
internal sealed class TweakBrowserPanel : Panel
{
    // ── Events ─────────────────────────────────────────────────────────────
    internal event Func<TweakDef, bool, Task>? TweakToggled; // (tweak, enable)
    internal event Action<TweakDef>? TweakInfoRequested;
    internal event Action? AdvancedViewRequested;

    // ── Sub-controls ───────────────────────────────────────────────────────
    private readonly Panel _leftPane;
    private readonly TreeView _categoryTree;
    private readonly Panel _rightPane;
    private readonly Panel _filterBar;
    private readonly TextBox _searchBox;
    private readonly ComboBox _statusFilter;
    private readonly Button _btnAdvanced;
    private readonly Panel _cardArea; // scrollable card container

    // ── State ──────────────────────────────────────────────────────────────
    private TweakEngine? _engine;
    private IReadOnlyDictionary<string, TweakResult>? _statusCache;
    private readonly HashSet<string> _inapplicableIds = new(StringComparer.Ordinal);
    private string _selectedCategory = string.Empty;
    private string _searchText = string.Empty;
    private string _statusFilterVal = "All";
    private bool _rebuilding;

    // Card pool to avoid excessive GC
    private readonly List<TweakCardRow> _cardPool = [];

    // debounce
    private readonly System.Windows.Forms.Timer _debounce = new() { Interval = 220 };

    // ── Construction ───────────────────────────────────────────────────────
    internal TweakBrowserPanel()
    {
        _debounce.Tick += (_, _) =>
        {
            _debounce.Stop();
            RebuildCards();
        };

        // ── Left category pane ─────────────────────────────────────────
        _categoryTree = new TreeView
        {
            Dock = DockStyle.Fill,
            BorderStyle = BorderStyle.None,
            ShowLines = false,
            ShowPlusMinus = false,
            ShowRootLines = false,
            HotTracking = true,
            Font = AppTheme.Bold,
            BackColor = AppTheme.Surface,
            ForeColor = AppTheme.FgDim,
        };
        _categoryTree.AfterSelect += OnCategorySelected;
        _categoryTree.DrawMode = TreeViewDrawMode.OwnerDrawText;
        _categoryTree.DrawNode += OnDrawCategoryNode;

        _leftPane = new Panel { Dock = DockStyle.Left, Width = 188 };
        _leftPane.Controls.Add(_categoryTree);

        // ── Filter bar ─────────────────────────────────────────────────
        _searchBox = new TextBox
        {
            Width = 220,
            BorderStyle = BorderStyle.FixedSingle,
            Font = AppTheme.Bold,
            BackColor = AppTheme.Surface,
            ForeColor = AppTheme.Fg,
            PlaceholderText = "Search tweaks…",
        };
        _searchBox.TextChanged += (_, _) =>
        {
            _debounce.Stop();
            _debounce.Start();
        };

        _statusFilter = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            Width = 120,
            Font = AppTheme.Bold,
            BackColor = AppTheme.Surface,
            ForeColor = AppTheme.Fg,
        };
        _statusFilter.Items.AddRange(new object[] { "All", "Applied", "Not Applied", "Errors" });
        _statusFilter.SelectedIndex = 0;
        _statusFilter.SelectedIndexChanged += (_, _) =>
        {
            _statusFilterVal = _statusFilter.SelectedItem?.ToString() ?? "All";
            RebuildCards();
        };

        _btnAdvanced = new Button
        {
            Text = "⊞  Advanced View",
            FlatStyle = FlatStyle.Flat,
            Font = new Font(AppTheme.Bold.FontFamily, 8.5f, FontStyle.Regular),
            ForeColor = AppTheme.FgDim,
            BackColor = AppTheme.Surface,
            Height = 28,
            AutoSize = true,
            Cursor = Cursors.Hand,
        };
        _btnAdvanced.FlatAppearance.BorderColor = AppTheme.Border;
        _btnAdvanced.Click += (_, _) => AdvancedViewRequested?.Invoke();

        var filterLayout = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 40,
            FlowDirection = FlowDirection.LeftToRight,
            Padding = new Padding(8, 6, 8, 0),
            AutoScroll = false,
            BackColor = AppTheme.Bg,
        };
        filterLayout.Controls.AddRange(new Control[] { _searchBox, _statusFilter, _btnAdvanced });
        _filterBar = filterLayout;

        // ── Card area ─────────────────────────────────────────────────
        _cardArea = new Panel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            BackColor = AppTheme.Bg,
            Padding = new Padding(12, 8, 12, 8),
        };

        _rightPane = new Panel { Dock = DockStyle.Fill };
        _rightPane.Controls.Add(_cardArea);
        _rightPane.Controls.Add(_filterBar);

        // Splitter — must use an opaque colour; AppTheme.Border is semi-transparent
        // (alpha < 255) and WinForms Splitter throws ArgumentException on transparent BackColor.
        var splitter = new Splitter
        {
            Dock = DockStyle.Left,
            Width = 5,
            BackColor = AppTheme.Surface,
        };

        Controls.Add(_rightPane);
        Controls.Add(splitter);
        Controls.Add(_leftPane);
    }

    // ── Public API ─────────────────────────────────────────────────────────
    internal void SetEngine(TweakEngine engine, IReadOnlyDictionary<string, TweakResult> statusCache, HashSet<string>? inapplicableIds = null)
    {
        _engine = engine;
        _statusCache = statusCache;
        if (inapplicableIds is not null)
        {
            _inapplicableIds.Clear();
            foreach (var id in inapplicableIds)
                _inapplicableIds.Add(id);
        }
        PopulateCategoryTree();
        RebuildCards();
    }

    internal void UpdateStatusCache(IReadOnlyDictionary<string, TweakResult> cache)
    {
        _statusCache = cache;
        // Refresh toggle states on existing visible cards without full rebuild
        foreach (TweakCardRow card in _cardPool)
        {
            bool visible = card.Visible && card.TweakDef is not null;
            if (visible)
                card.UpdateStatus(cache.GetValueOrDefault(card.TweakDef!.Id, TweakResult.Unknown));
        }
    }

    internal void ApplyTheme()
    {
        _categoryTree.BackColor = AppTheme.Surface;
        _categoryTree.ForeColor = AppTheme.FgDim;
        _filterBar.BackColor = AppTheme.Bg;
        _cardArea.BackColor = AppTheme.Bg;
        _leftPane.BackColor = AppTheme.Surface;
        foreach (var card in _cardPool)
            card.ApplyTheme();
        Invalidate(true);
    }

    internal void FocusSearch() => _searchBox.Focus();

    // ── Category tree population ───────────────────────────────────────────
    private void PopulateCategoryTree()
    {
        if (_engine is null)
            return;

        _categoryTree.BeginUpdate();
        _categoryTree.Nodes.Clear();

        var allNode = new TreeNode("All Tweaks") { Tag = "" };
        _categoryTree.Nodes.Add(allNode);

        var cats = _engine.Categories().OrderBy(c => c).ToList();
        var counts = _engine.CategoryCounts();
        foreach (string cat in cats)
        {
            int cnt = counts.TryGetValue(cat, out int c) ? c : 0;
            var node = new TreeNode($"{cat}  ({cnt})") { Tag = cat };
            _categoryTree.Nodes.Add(node);
        }

        _categoryTree.SelectedNode = allNode;
        _selectedCategory = string.Empty;
        _categoryTree.EndUpdate();
    }

    private void OnCategorySelected(object? sender, TreeViewEventArgs e)
    {
        _selectedCategory = (e.Node?.Tag as string) ?? string.Empty;
        RebuildCards();
    }

    private void OnDrawCategoryNode(object? sender, DrawTreeNodeEventArgs e)
    {
        bool isSelected = (e.State & TreeNodeStates.Selected) != 0;
        var r = e.Bounds;
        if (r.IsEmpty)
        {
            e.DrawDefault = true;
            return;
        }

        using var bg = new SolidBrush(isSelected ? Color.FromArgb(40, AppTheme.Accent) : AppTheme.Surface);
        e.Graphics.FillRectangle(bg, r);

        if (isSelected)
        {
            using var accentPen = new SolidBrush(AppTheme.Accent);
            e.Graphics.FillRectangle(accentPen, new Rectangle(0, r.Y + 2, 3, r.Height - 4));
        }

        Color textColor = isSelected ? AppTheme.Fg : AppTheme.FgDim;
        using var textBrush = new SolidBrush(textColor);
        using var font = isSelected
            ? new Font(AppTheme.Bold.FontFamily, 9f, FontStyle.Bold)
            : new Font(AppTheme.Bold.FontFamily, 9f, FontStyle.Regular);
        var sf = new StringFormat { LineAlignment = StringAlignment.Center };
        e.Graphics.DrawString(e.Node?.Text ?? "", font, textBrush, new RectangleF(r.X + 8, r.Y, r.Width - 8, r.Height), sf);
    }

    // ── Card building ──────────────────────────────────────────────────────
    private void RebuildCards()
    {
        if (_engine is null || _rebuilding)
            return;
        _rebuilding = true;
        _searchText = _searchBox.Text.Trim();

        IEnumerable<TweakDef> tweaks;
        if (!string.IsNullOrEmpty(_searchText))
            tweaks = _engine.Search(_searchText);
        else if (!string.IsNullOrEmpty(_selectedCategory))
            tweaks = _engine.TweaksByCategory().GetValueOrDefault(_selectedCategory) ?? [];
        else
            tweaks = _engine.AllTweaks();

        // Status filter
        if (_statusFilterVal != "All")
        {
            tweaks = _statusFilterVal switch
            {
                "Applied" => tweaks.Where(t => _statusCache?.GetValueOrDefault(t.Id) == TweakResult.Applied),
                "Not Applied" => tweaks.Where(t => _statusCache?.GetValueOrDefault(t.Id) is null or TweakResult.NotApplied),
                "Errors" => tweaks.Where(t => _statusCache?.GetValueOrDefault(t.Id) == TweakResult.Error),
                _ => tweaks,
            };
        }

        var list = tweaks.ToList();

        _cardArea.SuspendLayout();
        _cardArea.AutoScrollPosition = new Point(0, 0);

        // Reuse / extend pool
        while (_cardPool.Count < list.Count)
        {
            var card = new TweakCardRow();
            card.ToggleRequested += OnToggleRequested;
            card.InfoRequested += OnInfoRequested;
            _cardPool.Add(card);
        }

        // Hide extras
        for (int i = list.Count; i < _cardPool.Count; i++)
            _cardPool[i].Visible = false;

        // Populate visible
        int y = 0;
        for (int i = 0; i < list.Count; i++)
        {
            var td = list[i];
            var card = _cardPool[i];
            var status = _statusCache?.GetValueOrDefault(td.Id, TweakResult.Unknown) ?? TweakResult.Unknown;
            bool inapplicable = _inapplicableIds.Contains(td.Id);

            card.SetTweak(td, status, inapplicable);
            card.Location = new Point(0, y);
            card.Width = _cardArea.ClientSize.Width - (SystemInformation.VerticalScrollBarWidth + 4);
            card.Visible = true;
            y += card.Height + 4;

            if (!_cardArea.Controls.Contains(card))
                _cardArea.Controls.Add(card);
        }

        _cardArea.ResumeLayout(true);
        _rebuilding = false;
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        RebuildCards();
    }

    // ── Events from cards ──────────────────────────────────────────────────
    private async void OnToggleRequested(TweakDef td, bool enable)
    {
        if (TweakToggled is not null)
            await TweakToggled(td, enable);
        // Update toggle state without full rebuild
        var card = _cardPool.FirstOrDefault(c => c.TweakDef?.Id == td.Id);
        card?.UpdateStatus(enable ? TweakResult.Applied : TweakResult.NotApplied);
    }

    private void OnInfoRequested(TweakDef td) => TweakInfoRequested?.Invoke(td);
}

/// <summary>
/// Single tweak row with a toggle switch, label, description, and info button.
/// Height is fixed at 64px per card.
/// </summary>
internal sealed class TweakCardRow : Panel
{
    internal event Action<TweakDef, bool>? ToggleRequested;
    internal event Action<TweakDef>? InfoRequested;

    internal TweakDef? TweakDef { get; private set; }

    private readonly ToggleSwitchControl _toggle;
    private readonly Label _lblName;
    private readonly Label _lblDesc;
    private readonly Label _lblBadge;
    private readonly Button _btnInfo;
    private TweakResult _status;
    private bool _inapplicable;

    private const int CardHeight = 64;

    internal TweakCardRow()
    {
        Height = CardHeight;
        BackColor = AppTheme.Surface;

        _toggle = new ToggleSwitchControl { Location = new Point(12, (CardHeight - 22) / 2), Size = new Size(44, 22) };
        _toggle.CheckedChanged += OnToggleChanged;

        _lblName = new Label
        {
            AutoSize = false,
            Height = 20,
            Top = 12,
            Left = 66,
            Font = new Font(AppTheme.Bold.FontFamily, 9f, FontStyle.Bold),
            ForeColor = AppTheme.Fg,
            BackColor = Color.Transparent,
        };

        _lblDesc = new Label
        {
            AutoSize = false,
            Height = 16,
            Top = 32,
            Left = 66,
            Font = new Font(AppTheme.Bold.FontFamily, 8f, FontStyle.Regular),
            ForeColor = AppTheme.FgDim,
            BackColor = Color.Transparent,
        };

        _lblBadge = new Label
        {
            AutoSize = true,
            Top = 12,
            Font = new Font(AppTheme.Bold.FontFamily, 7.5f, FontStyle.Bold),
            ForeColor = AppTheme.Bg,
            BackColor = Color.Transparent,
        };

        _btnInfo = new Button
        {
            Text = "ⓘ",
            FlatStyle = FlatStyle.Flat,
            Size = new Size(24, 24),
            Font = new Font(AppTheme.Bold.FontFamily, 9f, FontStyle.Regular),
            ForeColor = AppTheme.FgDim,
            BackColor = Color.Transparent,
            Cursor = Cursors.Hand,
            TabStop = false,
        };
        _btnInfo.FlatAppearance.BorderSize = 0;
        _btnInfo.Click += (_, _) =>
        {
            if (TweakDef is not null)
                InfoRequested?.Invoke(TweakDef);
        };

        Controls.AddRange(new Control[] { _toggle, _lblName, _lblDesc, _lblBadge, _btnInfo });

        // Bottom divider
        SetStyle(ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        using var pen = new Pen(AppTheme.Border, 1);
        e.Graphics.DrawLine(pen, 0, Height - 1, Width, Height - 1);
    }

    internal void SetTweak(TweakDef td, TweakResult status, bool inapplicable)
    {
        TweakDef = td;
        _status = status;
        _inapplicable = inapplicable;

        _lblName.Text = td.Label;
        _lblDesc.Text =
            string.IsNullOrEmpty(td.Description) ? $"{td.Category}  •  {td.Kind}"
            : td.Description.Length > 80 ? td.Description[..80] + "…"
            : td.Description;

        // Update toggle without firing events
        _toggle.SetCheckedSilent(status == TweakResult.Applied);
        _toggle.Enabled = !inapplicable;
        _toggle.Cursor = inapplicable ? Cursors.No : Cursors.Hand;

        UpdateBadge();
        LayoutControls();
    }

    internal void UpdateStatus(TweakResult status)
    {
        _status = status;
        _toggle.SetCheckedSilent(status == TweakResult.Applied);
        UpdateBadge();
        Invalidate();
    }

    internal void ApplyTheme()
    {
        BackColor = AppTheme.Surface;
        _lblName.ForeColor = AppTheme.Fg;
        _lblDesc.ForeColor = AppTheme.FgDim;
        _btnInfo.ForeColor = AppTheme.FgDim;
        Invalidate();
    }

    private void UpdateBadge()
    {
        (string text, Color color) = _status switch
        {
            TweakResult.Applied => ("ON", AppTheme.Green),
            TweakResult.NotApplied => ("OFF", AppTheme.FgDim),
            TweakResult.Error => ("ERR", AppTheme.Red),
            TweakResult.SkippedCorp => ("CORP", AppTheme.Yellow),
            TweakResult.SkippedBuild => ("BUILD", AppTheme.Yellow),
            TweakResult.SkippedHw => ("N/A", AppTheme.FgDim),
            _ => ("???", AppTheme.FgDim),
        };
        if (_inapplicable)
        {
            text = "N/A";
            color = AppTheme.FgDim;
        }
        _lblBadge.Text = text;
        _lblBadge.BackColor = Color.Transparent;
        // Store color for custom paint; we'll overlay the badge in OnPaint
        _lblBadge.ForeColor = color;
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        LayoutControls();
    }

    private void LayoutControls()
    {
        int w = Width;
        _lblName.Width = w - 66 - 80; // leave room for badge + info btn on right
        _lblDesc.Width = w - 66 - 80;
        _lblBadge.Left = w - 70;
        _btnInfo.Left = w - 36;
        _btnInfo.Top = (CardHeight - 24) / 2;
    }

    private void OnToggleChanged(object? sender, EventArgs e)
    {
        if (TweakDef is null)
            return;
        ToggleRequested?.Invoke(TweakDef, _toggle.Checked);
    }
}
