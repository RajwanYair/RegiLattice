using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace RegiLattice.GUI.Controls;

/// <summary>
/// A double-buffered <see cref="Panel"/> that draws its own rounded-corner border
/// and an optional Mica-like translucent tint background. Drop-in replacement for
/// plain <see cref="Panel"/> cards in dialogs and the main form.
/// </summary>
public sealed class RoundedPanel : Panel
{
    private int _cornerRadius = 10;
    private Color _borderColor = Color.Transparent;
    private int _borderWidth = 1;
    private float _tintAlpha = 0f; // 0 = solid BackColor, >0 = tinted
    private Color _tintColor = Color.White;
    private Region? _cachedRegion;
    private Size _lastRegionSize;

    // ── Properties ─────────────────────────────────────────────────────────
    /// <summary>Corner radius in pixels (default 10).</summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int CornerRadius
    {
        get => _cornerRadius;
        set
        {
            _cornerRadius = value;
            Invalidate();
        }
    }

    /// <summary>Border colour; transparent = no border (default).</summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Color BorderColor
    {
        get => _borderColor;
        set
        {
            _borderColor = value;
            Invalidate();
        }
    }

    /// <summary>Border thickness in pixels (default 1).</summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int BorderWidth
    {
        get => _borderWidth;
        set
        {
            _borderWidth = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Alpha 0-1 for a Mica-style translucent tint drawn over BackColor.
    /// 0 = opaque BackColor, 1 = fully TintColor over BackColor.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public float TintAlpha
    {
        get => _tintAlpha;
        set
        {
            _tintAlpha = Math.Clamp(value, 0f, 1f);
            Invalidate();
        }
    }

    /// <summary>Tint colour applied at <see cref="TintAlpha"/> opacity.</summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Color TintColor
    {
        get => _tintColor;
        set
        {
            _tintColor = value;
            Invalidate();
        }
    }

    // ── Construction ───────────────────────────────────────────────────────
    public RoundedPanel()
    {
        SetStyle(
            ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.AllPaintingInWmPaint
                | ControlStyles.UserPaint
                | ControlStyles.ResizeRedraw
                | ControlStyles.SupportsTransparentBackColor,
            true
        );
    }

    // ── Paint ──────────────────────────────────────────────────────────────
    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        Rectangle r = new Rectangle(_borderWidth, _borderWidth, Width - _borderWidth * 2 - 1, Height - _borderWidth * 2 - 1);

        using var path = RoundedPath(r, _cornerRadius);

        // ── Fill ──────────────────────────────────────────────────────────
        using SolidBrush back = new SolidBrush(BackColor);
        g.FillPath(back, path);

        if (_tintAlpha > 0.001f)
        {
            int a = (int)(_tintAlpha * 255);
            using var tintBrush = new SolidBrush(Color.FromArgb(a, _tintColor));
            g.FillPath(tintBrush, path);
        }

        // ── Border ────────────────────────────────────────────────────────
        if (_borderColor.A > 0 && _borderWidth > 0)
        {
            using Pen pen = new Pen(_borderColor, _borderWidth);
            g.DrawPath(pen, path);
        }

        // Clip child controls to rounded region — only recreate when size changes.
        if (_cachedRegion is null || _lastRegionSize != Size)
        {
            _cachedRegion?.Dispose();
            _cachedRegion = new Region(path);
            _lastRegionSize = Size;
        }
        var oldRegion = Region;
        Region = _cachedRegion.Clone();
        oldRegion?.Dispose();

        base.OnPaint(e);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            _cachedRegion?.Dispose();
        base.Dispose(disposing);
    }

    // ── Helper ─────────────────────────────────────────────────────────────
    private static GraphicsPath RoundedPath(Rectangle r, int radius)
    {
        int d = radius * 2;
        var path = new GraphicsPath();
        path.AddArc(r.Left, r.Top, d, d, 180, 90);
        path.AddArc(r.Right - d, r.Top, d, d, 270, 90);
        path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
        path.AddArc(r.Left, r.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }
}
