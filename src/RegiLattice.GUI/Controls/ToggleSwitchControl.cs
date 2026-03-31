using System.ComponentModel;

namespace RegiLattice.GUI.Controls;

/// <summary>
/// Animated toggle switch control (GDI+ drawn, smooth slide animation, theme-aware).
/// Drop-in checkbox replacement for the tweak list. DPI-safe.
/// </summary>
[DefaultEvent("CheckedChanged")]
public sealed class ToggleSwitchControl : Control
{
    // ── Layout constants (scale with DPI) ──────────────────────────────────
    private const int TrackH = 22;
    private const int TrackW = 44;
    private const int KnobPad = 3;
    private const int AnimSteps = 8;   // frames for slide animation
    private const int AnimIntervalMs = 16; // ~60fps

    // ── State ──────────────────────────────────────────────────────────────
    private bool _checked;
    private bool _hovered;
    private bool _animating;
    private float _knobPos;   // 0.0=off … 1.0=on
    private float _knobTarget;
    private readonly System.Windows.Forms.Timer _animTimer;

    // ── Theme colours (updated via ApplyTheme) ─────────────────────────────
    private Color _trackOn = Color.FromArgb(137, 180, 250); // Catppuccin Blue
    private Color _trackOff = Color.FromArgb(88, 91, 112);   // Catppuccin Surface2
    private Color _knob = Color.White;

    // ── Events ─────────────────────────────────────────────────────────────
    public event EventHandler? CheckedChanged;

    // ── Properties ─────────────────────────────────────────────────────────
    [DefaultValue(false)]
    public bool Checked
    {
        get => _checked;
        set
        {
            if (_checked == value) return;
            _checked = value;
            _knobTarget = _checked ? 1f : 0f;
            StartAnimation();
            CheckedChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>Toggle without raising CheckedChanged (for programmatic sync).</summary>
    public void SetCheckedSilent(bool value)
    {
        if (_checked == value) return;
        _checked = value;
        _knobPos = _knobTarget = _checked ? 1f : 0f;
        Invalidate();
    }

    // ── Construction ───────────────────────────────────────────────────────
    public ToggleSwitchControl()
    {
        SetStyle(
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.UserPaint |
            ControlStyles.SupportsTransparentBackColor,
            true);

        Size = new Size(TrackW + 2, TrackH + 2);
        Cursor = Cursors.Hand;
        TabStop = true;
        BackColor = Color.Transparent;

        _animTimer = new System.Windows.Forms.Timer { Interval = AnimIntervalMs };
        _animTimer.Tick += OnAnimTick;

        _knobPos = _knobTarget = 0f;

        // Accessibility
        AccessibleRole = AccessibleRole.CheckButton;
        AccessibleName = "Toggle switch";
    }

    // ── Theme ──────────────────────────────────────────────────────────────
    /// <summary>Update colours to match the current <see cref="AppTheme"/> palette.</summary>
    public void ApplyTheme(Color accent, Color surface2, Color knob)
    {
        _trackOn = accent;
        _trackOff = surface2;
        _knob = knob;
        Invalidate();
    }

    // ── Animation ──────────────────────────────────────────────────────────
    private void StartAnimation()
    {
        _animating = true;
        _animTimer.Start();
    }

    private void OnAnimTick(object? sender, EventArgs e)
    {
        float delta = _knobTarget - _knobPos;
        if (Math.Abs(delta) < 0.05f)
        {
            _knobPos = _knobTarget;
            _animating = false;
            _animTimer.Stop();
        }
        else
        {
            // Ease-out: move 40% of remaining distance each frame
            _knobPos += delta * 0.4f;
        }
        Invalidate();
    }

    // ── DPI helpers ────────────────────────────────────────────────────────
    private float DpiScale
    {
        get
        {
            using Graphics g = CreateGraphics();
            return g.DpiX / 96f;
        }
    }

    private Rectangle ScaledTrackRect()
    {
        float s = DpiScale;
        int tw = (int)(TrackW * s);
        int th = (int)(TrackH * s);
        int x = (Width - tw) / 2;
        int y = (Height - th) / 2;
        return new Rectangle(x, y, tw, th);
    }

    // ── Paint ──────────────────────────────────────────────────────────────
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        Graphics g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        Rectangle track = ScaledTrackRect();
        float s = DpiScale;
        int knobSize = (int)((TrackH - KnobPad * 2) * s);
        int knobTravel = track.Width - knobSize - (int)(KnobPad * 2 * s);
        int knobX = track.Left + (int)(KnobPad * s) + (int)(knobTravel * _knobPos);
        int knobY = track.Top + (int)(KnobPad * s);

        // ── Interpolate track colour ██████████████████████████████████████
        Color trackColour = InterpolateColour(_trackOff, _trackOn, _knobPos);

        // Hover: lighten track slightly
        if (_hovered && !_animating)
            trackColour = ControlPaint.Light(trackColour, 0.15f);

        // Draw rounded track
        DrawRoundedRect(g, track, track.Height / 2, trackColour);

        // Focus ring
        if (Focused)
        {
            using Pen focusPen = new Pen(Color.FromArgb(120, _trackOn), (int)(2 * s));
            DrawRoundedRectOutline(g, Rectangle.Inflate(track, (int)(2 * s), (int)(2 * s)),
                track.Height / 2 + (int)(2 * s), focusPen);
        }

        // Draw knob
        using SolidBrush knobBrush = new SolidBrush(_knob);
        using Pen knobShadow = new Pen(Color.FromArgb(40, 0, 0, 0), 1);
        var knobRect = new Rectangle(knobX, knobY, knobSize, knobSize);
        knobRect.Inflate(-1, -1);
        g.FillEllipse(knobBrush, knobRect);
        g.DrawEllipse(knobShadow, knobRect);
    }

    // ── Drawing helpers ────────────────────────────────────────────────────
    private static void DrawRoundedRect(Graphics g, Rectangle r, int radius, Color fill)
    {
        using SolidBrush b = new SolidBrush(fill);
        using var path = RoundedRectPath(r, radius);
        g.FillPath(b, path);
    }

    private static void DrawRoundedRectOutline(Graphics g, Rectangle r, int radius, Pen pen)
    {
        using var path = RoundedRectPath(r, radius);
        g.DrawPath(pen, path);
    }

    private static System.Drawing.Drawing2D.GraphicsPath RoundedRectPath(Rectangle r, int radius)
    {
        int d = radius * 2;
        var path = new System.Drawing.Drawing2D.GraphicsPath();
        path.AddArc(r.Left, r.Top, d, d, 180, 90);
        path.AddArc(r.Right - d, r.Top, d, d, 270, 90);
        path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
        path.AddArc(r.Left, r.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }

    private static Color InterpolateColour(Color a, Color b, float t)
    {
        t = Math.Clamp(t, 0f, 1f);
        return Color.FromArgb(
            (int)(a.A + (b.A - a.A) * t),
            (int)(a.R + (b.R - a.R) * t),
            (int)(a.G + (b.G - a.G) * t),
            (int)(a.B + (b.B - a.B) * t));
    }

    // ── Input ──────────────────────────────────────────────────────────────
    protected override void OnMouseClick(MouseEventArgs e)
    {
        base.OnMouseClick(e);
        if (e.Button == MouseButtons.Left)
            Checked = !Checked;
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
        if (e.KeyCode is Keys.Space or Keys.Return)
        {
            Checked = !Checked;
            e.Handled = true;
        }
    }

    protected override void OnMouseEnter(EventArgs e) { base.OnMouseEnter(e); _hovered = true; Invalidate(); }
    protected override void OnMouseLeave(EventArgs e) { base.OnMouseLeave(e); _hovered = false; Invalidate(); }
    protected override void OnGotFocus(EventArgs e) { base.OnGotFocus(e); Invalidate(); }
    protected override void OnLostFocus(EventArgs e) { base.OnLostFocus(e); Invalidate(); }

    // ── Preferred size ─────────────────────────────────────────────────────
    protected override Size DefaultSize => new Size(TrackW + 4, TrackH + 4);

    public override Size GetPreferredSize(Size proposedSize) =>
        new Size((int)((TrackW + 4) * DpiScale), (int)((TrackH + 4) * DpiScale));

    // ── Accessibility ──────────────────────────────────────────────────────
    protected override AccessibleObject CreateAccessibilityInstance() =>
        new ToggleAccessible(this);

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            _animTimer.Dispose();
        base.Dispose(disposing);
    }

    // ── Accessible object ──────────────────────────────────────────────────
    private sealed class ToggleAccessible : ControlAccessibleObject
    {
        private readonly ToggleSwitchControl _owner;

        public ToggleAccessible(ToggleSwitchControl owner) : base(owner)
        {
            _owner = owner;
        }

        public override string Description =>
            _owner.Checked ? "On" : "Off";

        public override AccessibleStates State =>
            base.State |
            (_owner.Checked ? AccessibleStates.Checked : AccessibleStates.None);
    }
}
