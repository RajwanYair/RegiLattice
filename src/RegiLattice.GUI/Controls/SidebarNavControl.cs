#nullable enable
using System.Drawing.Drawing2D;

namespace RegiLattice.GUI.Controls;

/// <summary>
/// Vertical sidebar navigation bar with icon + label buttons.
/// Renders a flat dark panel with themed hover/selected states.
/// Usage: add items via <see cref="AddItem"/>, subscribe to <see cref="ItemSelected"/>.
/// </summary>
internal sealed class SidebarNavControl : Control
{
    // ── Types ──────────────────────────────────────────────────────────────
    internal sealed record NavItem(string Key, string Icon, string Label, string? Badge = null);

    // ── Fields ─────────────────────────────────────────────────────────────
    private readonly List<NavItem> _items = [];
    private string _selectedKey = string.Empty;
    private string _hoveredKey = string.Empty;

    // Layout constants
    private const int ItemHeight = 56;
    private const int IconSize = 22;
    private const int PaddingLeft = 14;
    private const int FooterGap = 8; // gap between last nav item and footer items
    private readonly List<NavItem> _footerItems = [];

    // ── Events ─────────────────────────────────────────────────────────────
    internal event Action<string>? ItemSelected;

    // ── Constructor ────────────────────────────────────────────────────────
    internal SidebarNavControl()
    {
        SetStyle(
            ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint,
            true
        );
        Width = 180;
        Cursor = Cursors.Hand;
    }

    // ── Public API ─────────────────────────────────────────────────────────
    internal void AddItem(string key, string icon, string label, string? badge = null) => _items.Add(new NavItem(key, icon, label, badge));

    internal void AddFooterItem(string key, string icon, string label) => _footerItems.Add(new NavItem(key, icon, label));

    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    internal string SelectedKey
    {
        get => _selectedKey;
        set
        {
            if (_selectedKey == value)
                return;
            _selectedKey = value;
            Invalidate();
            ItemSelected?.Invoke(value);
        }
    }

    internal void SetBadge(string key, string? badge)
    {
        int idx = _items.FindIndex(i => i.Key == key);
        if (idx >= 0)
        {
            _items[idx] = _items[idx] with { Badge = badge };
            Invalidate();
        }
    }

    // ── Painting ───────────────────────────────────────────────────────────
    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        // Background
        g.Clear(AppTheme.Surface);

        // Draw logo / app name at top
        DrawAppHeader(g);

        // Draw main items
        int y = 70; // below header
        foreach (var item in _items)
        {
            DrawItem(g, item, y);
            y += ItemHeight;
        }

        // Draw separator above footer items
        if (_footerItems.Count > 0)
        {
            int sepY = Height - (_footerItems.Count * ItemHeight) - FooterGap;
            using var sepPen = new Pen(AppTheme.Border, 1);
            g.DrawLine(sepPen, PaddingLeft, sepY, Width - PaddingLeft, sepY);

            int fy = sepY + FooterGap;
            foreach (var item in _footerItems)
            {
                DrawItem(g, item, fy);
                fy += ItemHeight;
            }
        }

        // Right-edge shadow line
        using var borderPen = new Pen(AppTheme.Border, 1);
        g.DrawLine(borderPen, Width - 1, 0, Width - 1, Height);
    }

    private void DrawAppHeader(Graphics g)
    {
        // Small RL logo area at top
        var logoRect = new Rectangle(PaddingLeft, 14, 32, 32);
        using var logoBrush = new SolidBrush(AppTheme.Accent);
        using var gp = new GraphicsPath();
        gp.AddEllipse(logoRect);
        g.FillPath(logoBrush, gp);

        using var logoFont = new Font(AppTheme.Mono.FontFamily, 11f, FontStyle.Bold);
        using var logoTextBrush = new SolidBrush(AppTheme.Bg);
        var logoTextRect = new RectangleF(logoRect.X, logoRect.Y + 5, logoRect.Width, logoRect.Height);
        using var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        g.DrawString("RL", logoFont, logoTextBrush, logoTextRect, sf);

        using var titleFont = new Font(AppTheme.Bold.FontFamily, 10f, FontStyle.Bold);
        using var titleBrush = new SolidBrush(AppTheme.Fg);
        g.DrawString("RegiLattice", titleFont, titleBrush, PaddingLeft + 40, 20);
    }

    private void DrawItem(Graphics g, NavItem item, int y)
    {
        bool isSelected = item.Key == _selectedKey;
        bool isHovered = item.Key == _hoveredKey;
        var itemRect = new Rectangle(0, y, Width, ItemHeight);

        // Background highlight
        if (isSelected)
        {
            using var selBrush = new SolidBrush(Color.FromArgb(40, AppTheme.Accent));
            g.FillRectangle(selBrush, itemRect);

            // Left accent bar
            using var accentBrush = new SolidBrush(AppTheme.Accent);
            g.FillRectangle(accentBrush, new Rectangle(0, y + 8, 3, ItemHeight - 16));
        }
        else if (isHovered)
        {
            using var hoverBrush = new SolidBrush(Color.FromArgb(18, AppTheme.Fg));
            g.FillRectangle(hoverBrush, itemRect);
        }

        // Icon (Segoe Fluent Icons glyph)
        Color iconColor = isSelected ? AppTheme.Accent : (isHovered ? AppTheme.Fg : AppTheme.FgDim);
        int iconY = y + (ItemHeight - IconSize) / 2;
        FluentIcons.DrawGlyph(g, item.Icon, iconColor, new PointF(PaddingLeft + 4, iconY), IconSize);

        // Label
        Color labelColor = isSelected ? AppTheme.Fg : (isHovered ? AppTheme.Fg : AppTheme.FgDim);
        using var labelFont = isSelected
            ? new Font(AppTheme.Bold.FontFamily, 9.5f, FontStyle.Bold)
            : new Font(AppTheme.Bold.FontFamily, 9.5f, FontStyle.Regular);
        using var labelBrush = new SolidBrush(labelColor);
        float labelX = PaddingLeft + IconSize + 12;
        float labelY = y + (ItemHeight - labelFont.Height) / 2f;
        g.DrawString(item.Label, labelFont, labelBrush, labelX, labelY);

        // Badge (e.g., "9")
        if (!string.IsNullOrEmpty(item.Badge))
        {
            using var badgeFont = new Font(AppTheme.Bold.FontFamily, 7.5f, FontStyle.Bold);
            var badgeSize = g.MeasureString(item.Badge, badgeFont);
            float bw = Math.Max(badgeSize.Width + 8, 18);
            float bh = 16;
            float bx = Width - bw - 12;
            float by = y + (ItemHeight - bh) / 2f;
            using var accentBadgeBrush = new SolidBrush(AppTheme.Accent);
            AppTheme.FillRoundedRect(g, accentBadgeBrush, Rectangle.Round(new RectangleF(bx, by, bw, bh)), 8);
            using var badgeBrush = new SolidBrush(AppTheme.Bg);
            using var bsf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            g.DrawString(item.Badge, badgeFont, badgeBrush, new RectangleF(bx, by, bw, bh), bsf);
        }
    }

    // ── Mouse Handling ─────────────────────────────────────────────────────
    protected override void OnMouseMove(MouseEventArgs e)
    {
        string hit = HitTest(e.Y);
        if (hit != _hoveredKey)
        {
            _hoveredKey = hit;
            Invalidate();
        }
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        _hoveredKey = string.Empty;
        Invalidate();
    }

    protected override void OnMouseClick(MouseEventArgs e)
    {
        string hit = HitTest(e.Y);
        if (!string.IsNullOrEmpty(hit))
            SelectedKey = hit;
    }

    private string HitTest(int y)
    {
        // Main items
        int startY = 70;
        for (int i = 0; i < _items.Count; i++)
        {
            int itemY = startY + i * ItemHeight;
            if (y >= itemY && y < itemY + ItemHeight)
                return _items[i].Key;
        }
        // Footer items
        if (_footerItems.Count > 0)
        {
            int sepY = Height - (_footerItems.Count * ItemHeight) - FooterGap;
            int fy = sepY + FooterGap;
            for (int i = 0; i < _footerItems.Count; i++)
            {
                int itemY = fy + i * ItemHeight;
                if (y >= itemY && y < itemY + ItemHeight)
                    return _footerItems[i].Key;
            }
        }
        return string.Empty;
    }

    protected override void OnMouseEnter(
        EventArgs e
    ) { /* enable mousemove tracking */
    }
}
