using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace RegiLattice.GUI.Controls;

/// <summary>
/// Animated expand/collapse button for category headers in the main form.
/// Draws the Fluent Icons chevron that smoothly rotates 90° on expand/collapse.
/// Theme-aware background + hover highlight.
/// </summary>
public sealed class CategoryExpandButton : Control
{
    private bool  _expanded = true;
    private float _angle    = 90f;   // 0°=collapsed (▶), 90°=expanded (▼)
    private float _targetAngle = 90f;
    private bool  _hovered;
    private readonly System.Windows.Forms.Timer _anim;
    private const int AnimIntervalMs = 16;

    // ── Theme ──────────────────────────────────────────────────────────────
    private Color _fg       = Color.FromArgb(205, 214, 244);
    private Color _hoverBg  = Color.FromArgb(40, 137, 180, 250);

    public event EventHandler? ExpandedChanged;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool Expanded
    {
        get => _expanded;
        set
        {
            if (_expanded == value) return;
            _expanded    = value;
            _targetAngle = value ? 90f : 0f;
            _anim.Start();
            ExpandedChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public void ApplyTheme(Color fg, Color accent)
    {
        _fg      = fg;
        _hoverBg = Color.FromArgb(40, accent);
        Invalidate();
    }

    public CategoryExpandButton()
    {
        SetStyle(
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.AllPaintingInWmPaint  |
            ControlStyles.UserPaint, true);

        Size   = new Size(20, 20);
        Cursor = Cursors.Hand;
        TabStop = false;

        _anim = new System.Windows.Forms.Timer { Interval = AnimIntervalMs };
        _anim.Tick += (_, _) =>
        {
            float delta = _targetAngle - _angle;
            if (Math.Abs(delta) < 1f)
            {
                _angle = _targetAngle;
                _anim.Stop();
            }
            else
            {
                _angle += delta * 0.35f;
            }
            Invalidate();
        };

        AccessibleRole = AccessibleRole.PushButton;
        AccessibleName = "Expand/Collapse category";
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        if (_hovered)
        {
            using var hb = new SolidBrush(_hoverBg);
            g.FillEllipse(hb, 1, 1, Width - 2, Height - 2);
        }

        // Draw rotated chevron
        float cx = Width  / 2f;
        float cy = Height / 2f;
        g.TranslateTransform(cx, cy);
        g.RotateTransform(_angle);
        g.TranslateTransform(-cx, -cy);

        // Chevron: points facing right (▶) then rotated
        int half = Math.Min(Width, Height) / 2 - 4;
        PointF[] pts =
        [
            new PointF(cx - half * 0.5f, cy - half),
            new PointF(cx + half * 0.5f, cy),
            new PointF(cx - half * 0.5f, cy + half),
        ];
        using Pen pen = new Pen(_fg, 2f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round };
        g.DrawLines(pen, pts);
    }

    protected override void OnMouseClick(MouseEventArgs e)
    {
        base.OnMouseClick(e);
        if (e.Button == MouseButtons.Left)
            Expanded = !Expanded;
    }

    protected override void OnMouseEnter(EventArgs e) { base.OnMouseEnter(e); _hovered = true;  Invalidate(); }
    protected override void OnMouseLeave(EventArgs e) { base.OnMouseLeave(e); _hovered = false; Invalidate(); }

    protected override void Dispose(bool disposing)
    {
        if (disposing) _anim.Dispose();
        base.Dispose(disposing);
    }
}
