#nullable enable
using System.Drawing.Drawing2D;

namespace RegiLattice.GUI.Controls;

/// <summary>
/// "Tools" section panel — a categorized grid of icon-buttons replacing
/// the deeply-nested Tools menu hierarchy.  Each button shows an icon,
/// name, and one-line description.  Clicking invokes an action delegate.
/// </summary>
internal sealed class ToolsHubPanel : Panel
{
    // ── Types ──────────────────────────────────────────────────────────────
    private sealed record ToolCard(string Icon, string Name, string Description, string Group, Action Invoke, bool AdminRequired = false);

    // ── Fields ─────────────────────────────────────────────────────────────
    private readonly List<ToolCard> _tools = [];
    private readonly Panel _contentPanel;
    private readonly TextBox _searchBox;
    private string _searchText = string.Empty;

    // ── Construction ───────────────────────────────────────────────────────
    internal ToolsHubPanel()
    {
        SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

        // Header
        var header = new Label
        {
            Text = "System Tools",
            Font = new Font(AppTheme.Bold.FontFamily, 14f, FontStyle.Bold),
            ForeColor = AppTheme.Fg,
            BackColor = AppTheme.Bg,
            AutoSize = true,
            Location = new Point(24, 14),
        };

        _searchBox = new TextBox
        {
            Width = 220,
            Height = 28,
            BorderStyle = BorderStyle.FixedSingle,
            Font = AppTheme.Bold,
            BackColor = AppTheme.Surface,
            ForeColor = AppTheme.Fg,
            PlaceholderText = "Search tools…",
            Location = new Point(24, 46),
        };
        _searchBox.TextChanged += (_, _) =>
        {
            _searchText = _searchBox.Text.Trim();
            RebuildGrid();
        };

        var headerPanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 84,
            BackColor = AppTheme.Bg,
        };
        headerPanel.Controls.AddRange(new Control[] { _searchBox, header });

        _contentPanel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = AppTheme.Bg,
            AutoScroll = true,
        };

        Controls.Add(_contentPanel);
        Controls.Add(headerPanel);
    }

    // ── Public API ─────────────────────────────────────────────────────────
    internal void RegisterTool(string icon, string name, string description, string group, Action invoke, bool adminRequired = false) =>
        _tools.Add(new ToolCard(icon, name, description, group, invoke, adminRequired));

    internal void Build()
    {
        RebuildGrid();
    }

    internal void ApplyTheme()
    {
        BackColor = AppTheme.Bg;
        _contentPanel.BackColor = AppTheme.Bg;
        _searchBox.BackColor = AppTheme.Surface;
        _searchBox.ForeColor = AppTheme.Fg;
        RebuildGrid();
    }

    // ── Grid building ──────────────────────────────────────────────────────
    private void RebuildGrid()
    {
        _contentPanel.SuspendLayout();
        _contentPanel.Controls.Clear();

        var visible = string.IsNullOrEmpty(_searchText)
            ? _tools
            : _tools
                .Where(t =>
                    t.Name.Contains(_searchText, StringComparison.OrdinalIgnoreCase)
                    || t.Description.Contains(_searchText, StringComparison.OrdinalIgnoreCase)
                    || t.Group.Contains(_searchText, StringComparison.OrdinalIgnoreCase)
                )
                .ToList();

        var grouped = visible.GroupBy(t => t.Group).OrderBy(g => g.Key).ToList();

        int y = 8;
        foreach (var grp in grouped)
        {
            // Group header
            var grpLabel = new Label
            {
                Text = grp.Key,
                Font = new Font(AppTheme.Bold.FontFamily, 9f, FontStyle.Bold),
                ForeColor = AppTheme.Accent,
                BackColor = Color.Transparent,
                AutoSize = true,
                Location = new Point(16, y),
            };
            _contentPanel.Controls.Add(grpLabel);
            y += 24;

            // Flow layout for cards in this group
            var flow = new FlowLayoutPanel
            {
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                Location = new Point(8, y),
                Width = _contentPanel.ClientSize.Width - 20,
                BackColor = Color.Transparent,
                Padding = new Padding(0),
                Margin = new Padding(0),
            };

            foreach (var tool in grp)
            {
                var btn = CreateToolButton(tool);
                flow.Controls.Add(btn);
            }

            _contentPanel.Controls.Add(flow);
            y += flow.PreferredSize.Height + 16;
        }

        _contentPanel.ResumeLayout(true);
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        // Adjust flow panel widths
        foreach (Control c in _contentPanel.Controls)
        {
            if (c is FlowLayoutPanel fl)
                fl.Width = _contentPanel.ClientSize.Width - 20;
        }
    }

    // ── Tool button factory ────────────────────────────────────────────────
    private static ToolButton CreateToolButton(ToolCard tool)
    {
        var btn = new ToolButton(tool.Icon, tool.Name, tool.Description, tool.AdminRequired);
        btn.Clicked += tool.Invoke;
        return btn;
    }
}

/// <summary>
/// Individual tool icon-button (140×80px) showing icon, name, description,
/// and an optional Admin shield badge.
/// </summary>
internal sealed class ToolButton : Control
{
    internal event Action? Clicked;

    private readonly string _icon;
    private readonly string _name;
    private readonly string _desc;
    private readonly bool _adminRequired;
    private bool _hovered;

    internal ToolButton(string icon, string name, string desc, bool adminRequired)
    {
        _icon = icon;
        _name = name;
        _desc = desc;
        _adminRequired = adminRequired;

        Size = new Size(152, 80);
        Margin = new Padding(4);
        Cursor = Cursors.Hand;

        SetStyle(
            ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint,
            true
        );
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        // Card background
        Color bg = _hovered ? AppTheme.Surface : AppTheme.Bg;
        using var bgBrush = new SolidBrush(bg);
        AppTheme.FillRoundedRect(g, bgBrush, ClientRectangle, 8);

        // Border
        using var pen = new Pen(_hovered ? AppTheme.Accent : AppTheme.Border, 1);
        AppTheme.DrawRoundedRect(g, pen, ClientRectangle, 8);

        // Icon
        var iconRect = new Rectangle(12, 14, 26, 26);
        Color iconColor = _hovered ? AppTheme.Accent : AppTheme.FgDim;
        FluentIcons.DrawGlyph(g, _icon, iconColor, new PointF(iconRect.X, iconRect.Y), iconRect.Height);

        // Admin badge
        if (_adminRequired)
        {
            using var shieldBrush = new SolidBrush(AppTheme.Yellow);
            using var shieldFont = new Font(AppTheme.Bold.FontFamily, 7f, FontStyle.Bold);
            g.DrawString("⚙", shieldFont, shieldBrush, Width - 20, 6);
        }

        // Name
        using var nameFont = new Font(AppTheme.Bold.FontFamily, 9f, FontStyle.Bold);
        using var nameBrush = new SolidBrush(_hovered ? AppTheme.Fg : AppTheme.Fg);
        g.DrawString(_name, nameFont, nameBrush, new RectangleF(12, 44, Width - 20, 16));

        // Description
        using var descFont = new Font(AppTheme.Bold.FontFamily, 7.5f, FontStyle.Regular);
        using var descBrush = new SolidBrush(AppTheme.FgDim);
        string shortDesc = _desc.Length > 40 ? _desc[..40] + "…" : _desc;
        g.DrawString(shortDesc, descFont, descBrush, new RectangleF(12, 60, Width - 20, 16));
    }

    protected override void OnMouseEnter(EventArgs e)
    {
        _hovered = true;
        Invalidate();
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        _hovered = false;
        Invalidate();
    }

    protected override void OnMouseClick(MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
            Clicked?.Invoke();
    }
}
