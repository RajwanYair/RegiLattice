namespace RegiLattice.GUI;

/// <summary>Multi-theme engine with 4 switchable colour palettes (Catppuccin Mocha/Latte, Nord, Dracula).</summary>
internal static class AppTheme
{
    // ── Theme definition ───────────────────────────────────────────────────
    internal sealed record ThemeDef(
        string Name,
        Color Bg,
        Color Surface,
        Color Surface2,
        Color Fg,
        Color FgDim,
        Color Accent,
        Color Green,
        Color Red,
        Color Yellow,
        Color Overlay,
        Color Success,
        Color Danger,
        Color Info
    )
    {
        /// <summary>Lighter accent for hover states (30% alpha over Surface).</summary>
        internal Color AccentHover => Color.FromArgb(40, Accent);

        /// <summary>Darker accent for pressed states.</summary>
        internal Color AccentPressed => Color.FromArgb(70, Accent);

        /// <summary>Subtle border colour for cards and panels.</summary>
        internal Color Border => Color.FromArgb(50, Fg);

        /// <summary>Very subtle separator line colour.</summary>
        internal Color Separator => Color.FromArgb(30, Fg);
    }

    private static readonly Dictionary<string, ThemeDef> Themes = new(StringComparer.OrdinalIgnoreCase)
    {
        ["catppuccin-mocha"] = new(
            "Catppuccin Mocha",
            Bg: Color.FromArgb(30, 30, 46),
            Surface: Color.FromArgb(49, 50, 68),
            Surface2: Color.FromArgb(88, 91, 112),
            Fg: Color.FromArgb(205, 214, 244),
            FgDim: Color.FromArgb(166, 173, 200),
            Accent: Color.FromArgb(137, 180, 250),
            Green: Color.FromArgb(166, 227, 161),
            Red: Color.FromArgb(243, 139, 168),
            Yellow: Color.FromArgb(249, 226, 175),
            Overlay: Color.FromArgb(69, 71, 90),
            Success: Color.FromArgb(64, 160, 43),
            Danger: Color.FromArgb(230, 74, 25),
            Info: Color.FromArgb(30, 102, 245)
        ),

        ["catppuccin-latte"] = new(
            "Catppuccin Latte",
            Bg: Color.FromArgb(239, 241, 245),
            Surface: Color.FromArgb(204, 208, 218),
            Surface2: Color.FromArgb(172, 176, 190),
            Fg: Color.FromArgb(76, 79, 105),
            FgDim: Color.FromArgb(92, 95, 119),
            Accent: Color.FromArgb(30, 102, 245),
            Green: Color.FromArgb(64, 160, 43),
            Red: Color.FromArgb(210, 15, 57),
            Yellow: Color.FromArgb(223, 142, 29),
            Overlay: Color.FromArgb(156, 160, 176),
            Success: Color.FromArgb(64, 160, 43),
            Danger: Color.FromArgb(210, 15, 57),
            Info: Color.FromArgb(30, 102, 245)
        ),

        ["nord"] = new(
            "Nord",
            Bg: Color.FromArgb(46, 52, 64),
            Surface: Color.FromArgb(59, 66, 82),
            Surface2: Color.FromArgb(76, 86, 106),
            Fg: Color.FromArgb(236, 239, 244),
            FgDim: Color.FromArgb(216, 222, 233),
            Accent: Color.FromArgb(136, 192, 208),
            Green: Color.FromArgb(163, 190, 140),
            Red: Color.FromArgb(191, 97, 106),
            Yellow: Color.FromArgb(235, 203, 139),
            Overlay: Color.FromArgb(67, 76, 94),
            Success: Color.FromArgb(163, 190, 140),
            Danger: Color.FromArgb(191, 97, 106),
            Info: Color.FromArgb(129, 161, 193)
        ),

        ["dracula"] = new(
            "Dracula",
            Bg: Color.FromArgb(40, 42, 54),
            Surface: Color.FromArgb(68, 71, 90),
            Surface2: Color.FromArgb(98, 114, 164),
            Fg: Color.FromArgb(248, 248, 242),
            FgDim: Color.FromArgb(189, 147, 249),
            Accent: Color.FromArgb(139, 233, 253),
            Green: Color.FromArgb(80, 250, 123),
            Red: Color.FromArgb(255, 85, 85),
            Yellow: Color.FromArgb(241, 250, 140),
            Overlay: Color.FromArgb(68, 71, 90),
            Success: Color.FromArgb(80, 250, 123),
            Danger: Color.FromArgb(255, 85, 85),
            Info: Color.FromArgb(139, 233, 253)
        ),
    };

    // ── Current theme ──────────────────────────────────────────────────────
    private static ThemeDef _current = Themes["catppuccin-mocha"];

    internal static ThemeDef Current => _current;

    // ── Colour accessors (backward-compatible properties) ──────────────────
    internal static Color Bg => _current.Bg;
    internal static Color Surface => _current.Surface;
    internal static Color Surface2 => _current.Surface2;
    internal static Color Fg => _current.Fg;
    internal static Color FgDim => _current.FgDim;
    internal static Color Accent => _current.Accent;
    internal static Color Green => _current.Green;
    internal static Color Red => _current.Red;
    internal static Color Yellow => _current.Yellow;
    internal static Color Overlay => _current.Overlay;
    internal static Color Success => _current.Success;
    internal static Color Danger => _current.Danger;
    internal static Color Info => _current.Info;

    // ── Computed colour shorthand ──────────────────────────────────────────
    internal static Color AccentHover => _current.AccentHover;
    internal static Color AccentPressed => _current.AccentPressed;
    internal static Color Border => _current.Border;
    internal static Color Separator => _current.Separator;

    // ── Fonts ──────────────────────────────────────────────────────────────
    internal static readonly Font Regular = new("Segoe UI", 9f);
    internal static readonly Font Small = new("Segoe UI", 8f);
    internal static readonly Font Bold = new("Segoe UI", 9f, FontStyle.Bold);
    internal static readonly Font Title = new("Segoe UI", 12f, FontStyle.Bold);
    internal static readonly Font Mono = new("Consolas", 9f);
    internal static readonly Font SmallBold = new("Segoe UI", 7.5f, FontStyle.Bold);

    // ── Theme API ──────────────────────────────────────────────────────────
    internal static string[] AvailableThemes() => [.. Themes.Keys];

    internal static string CurrentThemeName() => Themes.FirstOrDefault(kv => kv.Value == _current).Key ?? "catppuccin-mocha";

    internal static void SetTheme(string name)
    {
        if (Themes.TryGetValue(name, out var def))
            _current = def;
    }

    /// <summary>Event raised after theme changes — subscribers should re-apply colours.</summary>
    internal static event Action? ThemeChanged;

    internal static void RaiseThemeChanged() => ThemeChanged?.Invoke();

    // ── Helpers ────────────────────────────────────────────────────────────
    /// <summary>Recursively apply theme colours to a control tree.</summary>
    internal static void Apply(Control root)
    {
        root.BackColor = Bg;
        root.ForeColor = Fg;
        root.Font = Regular;
        foreach (Control c in root.Controls)
            Apply(c);
    }

    /// <summary>Style a Button with the given background colour.</summary>
    internal static Button StyledButton(string text, Color bg, Color fg, EventHandler click)
    {
        var btn = new Button
        {
            Text = text,
            BackColor = bg,
            ForeColor = fg,
            FlatStyle = FlatStyle.Flat,
            Font = Bold,
            AutoSize = true,
            Padding = new Padding(8, 2, 8, 2),
        };
        btn.FlatAppearance.BorderSize = 0;
        btn.Click += click;
        return btn;
    }

    /// <summary>Create a styled ListBox for use inside package manager dialogs.</summary>
    internal static ListBox StyledListBox() =>
        new()
        {
            BackColor = Surface,
            ForeColor = Fg,
            Font = Regular,
            SelectionMode = SelectionMode.One,
            BorderStyle = BorderStyle.None,
            HorizontalScrollbar = true,
        };

    /// <summary>Create a styled TextBox.</summary>
    internal static TextBox StyledTextBox(int width = 200) =>
        new()
        {
            BackColor = Overlay,
            ForeColor = Fg,
            Font = Regular,
            BorderStyle = BorderStyle.FixedSingle,
            Width = width,
        };

    // ── Modern GDI+ helpers ────────────────────────────────────────────────

    /// <summary>Draw a filled rounded rectangle.</summary>
    internal static void FillRoundedRect(Graphics g, Brush brush, Rectangle rect, int radius)
    {
        if (radius <= 0)
        {
            g.FillRectangle(brush, rect);
            return;
        }
        using var path = RoundedRectPath(rect, radius);
        g.FillPath(brush, path);
    }

    /// <summary>Draw a rounded rectangle border.</summary>
    internal static void DrawRoundedRect(Graphics g, Pen pen, Rectangle rect, int radius)
    {
        if (radius <= 0)
        {
            g.DrawRectangle(pen, rect);
            return;
        }
        using var path = RoundedRectPath(rect, radius);
        g.DrawPath(pen, path);
    }

    /// <summary>Create a GraphicsPath for a rounded rectangle.</summary>
    internal static System.Drawing.Drawing2D.GraphicsPath RoundedRectPath(Rectangle rect, int radius)
    {
        int d = radius * 2;
        var path = new System.Drawing.Drawing2D.GraphicsPath();
        path.AddArc(rect.X, rect.Y, d, d, 180, 90);
        path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
        path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
        path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }

    /// <summary>Draw a pill-shaped badge with text.</summary>
    internal static void DrawPill(Graphics g, string text, Font font, Color bg, Color fg, int x, int y, int hPad = 6, int vPad = 1)
    {
        var textSize = TextRenderer.MeasureText(text, font);
        int w = textSize.Width + hPad * 2;
        int h = textSize.Height + vPad * 2;
        var rect = new Rectangle(x, y, w, h);

        using var brush = new SolidBrush(Color.FromArgb(35, bg));
        FillRoundedRect(g, brush, rect, h / 2);

        using var border = new Pen(Color.FromArgb(80, bg), 1f);
        DrawRoundedRect(g, border, rect, h / 2);

        TextRenderer.DrawText(g, text, font, rect, fg, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
    }
}
