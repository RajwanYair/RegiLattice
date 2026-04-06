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

        ["tokyo-night"] = new(
            "Tokyo Night",
            Bg: Color.FromArgb(26, 27, 38),
            Surface: Color.FromArgb(36, 40, 59),
            Surface2: Color.FromArgb(52, 59, 88),
            Fg: Color.FromArgb(169, 177, 214),
            FgDim: Color.FromArgb(86, 95, 137),
            Accent: Color.FromArgb(122, 162, 247),
            Green: Color.FromArgb(158, 206, 106),
            Red: Color.FromArgb(247, 118, 142),
            Yellow: Color.FromArgb(224, 175, 104),
            Overlay: Color.FromArgb(41, 46, 66),
            Success: Color.FromArgb(115, 218, 202),
            Danger: Color.FromArgb(219, 75, 75),
            Info: Color.FromArgb(125, 207, 255)
        ),

        ["gruvbox-dark"] = new(
            "Gruvbox Dark",
            Bg: Color.FromArgb(40, 40, 40),
            Surface: Color.FromArgb(60, 56, 54),
            Surface2: Color.FromArgb(80, 73, 69),
            Fg: Color.FromArgb(235, 219, 178),
            FgDim: Color.FromArgb(168, 153, 132),
            Accent: Color.FromArgb(250, 189, 47),
            Green: Color.FromArgb(184, 187, 38),
            Red: Color.FromArgb(251, 73, 52),
            Yellow: Color.FromArgb(250, 189, 47),
            Overlay: Color.FromArgb(50, 48, 47),
            Success: Color.FromArgb(184, 187, 38),
            Danger: Color.FromArgb(204, 36, 29),
            Info: Color.FromArgb(131, 165, 152)
        ),

        ["solarized-dark"] = new(
            "Solarized Dark",
            Bg: Color.FromArgb(0, 43, 54),
            Surface: Color.FromArgb(7, 54, 66),
            Surface2: Color.FromArgb(88, 110, 117),
            Fg: Color.FromArgb(131, 148, 150),
            FgDim: Color.FromArgb(101, 123, 131),
            Accent: Color.FromArgb(38, 139, 210),
            Green: Color.FromArgb(133, 153, 0),
            Red: Color.FromArgb(220, 50, 47),
            Yellow: Color.FromArgb(181, 137, 0),
            Overlay: Color.FromArgb(7, 54, 66),
            Success: Color.FromArgb(133, 153, 0),
            Danger: Color.FromArgb(203, 75, 22),
            Info: Color.FromArgb(42, 161, 152)
        ),

        ["one-dark"] = new(
            "One Dark Pro",
            Bg: Color.FromArgb(40, 44, 52),
            Surface: Color.FromArgb(50, 55, 65),
            Surface2: Color.FromArgb(62, 68, 81),
            Fg: Color.FromArgb(171, 178, 191),
            FgDim: Color.FromArgb(92, 99, 112),
            Accent: Color.FromArgb(97, 175, 239),
            Green: Color.FromArgb(152, 195, 121),
            Red: Color.FromArgb(224, 108, 117),
            Yellow: Color.FromArgb(229, 192, 123),
            Overlay: Color.FromArgb(44, 49, 58),
            Success: Color.FromArgb(152, 195, 121),
            Danger: Color.FromArgb(190, 80, 70),
            Info: Color.FromArgb(86, 182, 194)
        ),

        ["rose-pine"] = new(
            "Rosé Pine",
            Bg: Color.FromArgb(25, 23, 36),
            Surface: Color.FromArgb(38, 35, 53),
            Surface2: Color.FromArgb(57, 53, 82),
            Fg: Color.FromArgb(224, 222, 244),
            FgDim: Color.FromArgb(110, 106, 134),
            Accent: Color.FromArgb(196, 167, 231),
            Green: Color.FromArgb(156, 207, 216),
            Red: Color.FromArgb(235, 111, 146),
            Yellow: Color.FromArgb(246, 193, 119),
            Overlay: Color.FromArgb(57, 53, 82),
            Success: Color.FromArgb(156, 207, 216),
            Danger: Color.FromArgb(235, 111, 146),
            Info: Color.FromArgb(49, 116, 143)
        ),

        ["everforest"] = new(
            "Everforest",
            Bg: Color.FromArgb(39, 46, 34),
            Surface: Color.FromArgb(52, 60, 46),
            Surface2: Color.FromArgb(79, 88, 72),
            Fg: Color.FromArgb(211, 198, 170),
            FgDim: Color.FromArgb(135, 134, 115),
            Accent: Color.FromArgb(163, 190, 140),
            Green: Color.FromArgb(167, 192, 128),
            Red: Color.FromArgb(230, 126, 128),
            Yellow: Color.FromArgb(219, 188, 127),
            Overlay: Color.FromArgb(45, 53, 38),
            Success: Color.FromArgb(131, 192, 110),
            Danger: Color.FromArgb(230, 126, 128),
            Info: Color.FromArgb(127, 187, 179)
        ),

        ["cyberpunk"] = new(
            "Cyberpunk",
            Bg: Color.FromArgb(13, 2, 33),
            Surface: Color.FromArgb(25, 10, 56),
            Surface2: Color.FromArgb(45, 20, 90),
            Fg: Color.FromArgb(230, 230, 255),
            FgDim: Color.FromArgb(140, 130, 180),
            Accent: Color.FromArgb(255, 0, 255),
            Green: Color.FromArgb(0, 255, 159),
            Red: Color.FromArgb(255, 45, 85),
            Yellow: Color.FromArgb(255, 230, 0),
            Overlay: Color.FromArgb(30, 15, 60),
            Success: Color.FromArgb(0, 255, 159),
            Danger: Color.FromArgb(255, 45, 85),
            Info: Color.FromArgb(0, 210, 255)
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
    // Base font size — configurable via Preferences → Appearance → Font size.
    private static float _baseFontSize = 9f;
    internal static Font Regular = new("Segoe UI", 9f);
    internal static Font Small = new("Segoe UI", 8f);
    internal static Font Bold = new("Segoe UI", 9f, FontStyle.Bold);
    internal static Font Title = new("Segoe UI", 12f, FontStyle.Bold);
    internal static Font Mono = new("Consolas", 9f);
    internal static Font SmallBold = new("Segoe UI", 7.5f, FontStyle.Bold);

    /// <summary>Current base font size in points (default 9f).</summary>
    internal static float BaseFontSize => _baseFontSize;

    /// <summary>
    /// Recreate all theme fonts using <paramref name="baseSize"/> as the base point size.
    /// Dispose the previous font instances to prevent GDI handle leaks.
    /// </summary>
    internal static void SetFontSize(float baseSize)
    {
        if (baseSize is < 7f or > 16f)
            return;

        _baseFontSize = baseSize;
        float small = MathF.Max(7f, baseSize - 1f);
        float smallBold = MathF.Max(7f, baseSize - 1.5f);
        float title = baseSize + 3f;

        Font prevRegular = Regular,
            prevSmall = Small,
            prevBold = Bold;
        Font prevTitle = Title,
            prevMono = Mono,
            prevSmallBold = SmallBold;

        Regular = new Font("Segoe UI", baseSize);
        Small = new Font("Segoe UI", small);
        Bold = new Font("Segoe UI", baseSize, FontStyle.Bold);
        Title = new Font("Segoe UI", title, FontStyle.Bold);
        Mono = new Font("Consolas", baseSize);
        SmallBold = new Font("Segoe UI", smallBold, FontStyle.Bold);

        prevRegular.Dispose();
        prevSmall.Dispose();
        prevBold.Dispose();
        prevTitle.Dispose();
        prevMono.Dispose();
        prevSmallBold.Dispose();
    }

    // ── User themes ────────────────────────────────────────────────────────
    /// <summary>User-defined JSON themes loaded from <c>%LOCALAPPDATA%\RegiLattice\themes\*.json</c>.</summary>
    private static readonly Dictionary<string, ThemeDef> _userThemes = new(StringComparer.OrdinalIgnoreCase);
    private static FileSystemWatcher? _themeWatcher;

    // ── Theme API ──────────────────────────────────────────────────────────
    internal static string[] AvailableThemes() => [.. Themes.Keys, .. _userThemes.Keys];

    internal static string CurrentThemeName() =>
        Themes.FirstOrDefault(kv => kv.Value == _current).Key ?? _userThemes.FirstOrDefault(kv => kv.Value == _current).Key ?? "catppuccin-mocha";

    /// <summary>Returns <c>true</c> if <paramref name="name"/> refers to a user-defined JSON theme.</summary>
    internal static bool IsUserTheme(string name) => _userThemes.ContainsKey(name);

    internal static void SetTheme(string name)
    {
        if (Themes.TryGetValue(name, out var def) || _userThemes.TryGetValue(name, out def))
            _current = def;
    }

    /// <summary>Detect Windows 10/11 system theme preference (dark vs light).</summary>
    /// <returns>Theme key matching system preference: "catppuccin-mocha" for dark, "catppuccin-latte" for light.</returns>
    internal static string DetectSystemTheme()
    {
        try
        {
            using var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
            if (key?.GetValue("AppsUseLightTheme") is int value)
                return value == 1 ? "catppuccin-latte" : "catppuccin-mocha";
        }
        catch (System.Security.SecurityException) { }
        catch (UnauthorizedAccessException) { }
        return "catppuccin-mocha"; // default to dark
    }

    /// <summary>Event raised after theme changes — subscribers should re-apply colours.</summary>
    internal static event Action? ThemeChanged;

    internal static void RaiseThemeChanged() => ThemeChanged?.Invoke();

    // ── User Theme JSON Loading ─────────────────────────────────────────────
    /// <summary>Raised when a user theme JSON file fails to parse. Arg = descriptive message.</summary>
    internal static event Action<string>? UserThemeLoadError;

    /// <summary>
    /// Load all <c>*.json</c> user theme files from <c>%LOCALAPPDATA%\RegiLattice\themes\</c>
    /// and install a <see cref="FileSystemWatcher"/> for live hot-reload.
    /// Invalid files are silently skipped; <see cref="UserThemeLoadError"/> fires per error.
    /// Safe to call multiple times — the watcher is created only once.
    /// </summary>
    internal static void LoadUserThemes()
    {
        string themesDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "RegiLattice", "themes");
        if (!Directory.Exists(themesDir))
            return;

        foreach (string file in Directory.GetFiles(themesDir, "*.json"))
            TryRegisterUserTheme(file);

        if (_themeWatcher is null)
        {
            _themeWatcher = new FileSystemWatcher(themesDir, "*.json")
            {
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.CreationTime,
                EnableRaisingEvents = true,
            };
            _themeWatcher.Changed += (_, e) =>
            {
                System.Threading.Thread.Sleep(80);
                TryRegisterUserTheme(e.FullPath);
            };
            _themeWatcher.Created += (_, e) =>
            {
                System.Threading.Thread.Sleep(80);
                TryRegisterUserTheme(e.FullPath);
            };
            _themeWatcher.Deleted += (_, e) =>
            {
                _userThemes.Remove($"user:{Path.GetFileNameWithoutExtension(e.FullPath)}");
                RaiseThemeChanged();
            };
            _themeWatcher.Renamed += (_, e) =>
            {
                _userThemes.Remove($"user:{Path.GetFileNameWithoutExtension(e.OldFullPath)}");
                TryRegisterUserTheme(e.FullPath);
            };
        }
    }

    private static void TryRegisterUserTheme(string filePath)
    {
        try
        {
            string json = File.ReadAllText(filePath, System.Text.Encoding.UTF8);
            ThemeDef? def = TryParseUserTheme(json);
            if (def is null)
            {
                UserThemeLoadError?.Invoke($"[Theme] Skipping '{Path.GetFileName(filePath)}': missing required 'name' or colour fields.");
                return;
            }
            _userThemes[$"user:{Path.GetFileNameWithoutExtension(filePath)}"] = def;
            RaiseThemeChanged();
        }
        catch (IOException) { }
        catch (UnauthorizedAccessException) { }
        catch (System.Text.Json.JsonException ex)
        {
            UserThemeLoadError?.Invoke($"[Theme] JSON error in '{Path.GetFileName(filePath)}': {ex.Message}");
        }
    }

    /// <summary>
    /// Parse a user theme JSON string into a <see cref="ThemeDef"/>.
    /// Returns <c>null</c> if required fields (<c>name</c>, <c>background</c>, <c>text</c>) are absent.
    /// </summary>
    internal static ThemeDef? TryParseUserTheme(string json)
    {
        var opts = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true, AllowTrailingCommas = true };
        UserThemeJson? data = System.Text.Json.JsonSerializer.Deserialize<UserThemeJson>(json, opts);
        if (data is null || string.IsNullOrWhiteSpace(data.Name))
            return null;
        if (string.IsNullOrWhiteSpace(data.Background) || string.IsNullOrWhiteSpace(data.Text))
            return null;

        Color bg = Hex(data.Background);
        Color surface = Hex(data.Surface ?? data.Background);
        Color fg = Hex(data.Text);
        Color fgDim = Hex(data.TextSecondary ?? DimHex(data.Text, 0.65f));
        Color accent = Hex(data.Accent ?? data.Primary ?? "#89b4fa");
        Color green = Hex(data.Secondary ?? data.Success ?? "#a6e3a1");
        Color red = Hex(data.Error ?? "#f38ba8");
        Color yellow = Hex(data.Warning ?? "#f9e2af");
        Color success = Hex(data.Success ?? data.Secondary ?? "#a6e3a1");
        Color surface2 = Hex(data.Surface2 ?? BlendHex(data.Surface ?? data.Background, data.Text, 0.15f));
        Color overlay = Hex(data.Overlay ?? BlendHex(data.Background, data.Surface ?? data.Background, 0.4f));
        Color info = Hex(data.Info ?? data.Accent ?? data.Primary ?? "#89dceb");

        return new ThemeDef(
            Name: data.Name,
            Bg: bg,
            Surface: surface,
            Surface2: surface2,
            Fg: fg,
            FgDim: fgDim,
            Accent: accent,
            Green: green,
            Red: red,
            Yellow: yellow,
            Overlay: overlay,
            Success: success,
            Danger: red,
            Info: info
        );
    }

    private static Color Hex(string hex)
    {
        ReadOnlySpan<char> s = hex.AsSpan().TrimStart('#');
        if (s.Length == 6 && int.TryParse(s, System.Globalization.NumberStyles.HexNumber, null, out int rgb))
            return Color.FromArgb((rgb >> 16) & 0xFF, (rgb >> 8) & 0xFF, rgb & 0xFF);
        if (s.Length == 8 && int.TryParse(s, System.Globalization.NumberStyles.HexNumber, null, out int argb))
            return Color.FromArgb((argb >> 24) & 0xFF, (argb >> 16) & 0xFF, (argb >> 8) & 0xFF, argb & 0xFF);
        return Color.Gray;
    }

    private static string DimHex(string hex, float factor)
    {
        Color c = Hex(hex);
        return $"#{(int)(c.R * factor):X2}{(int)(c.G * factor):X2}{(int)(c.B * factor):X2}";
    }

    private static string BlendHex(string hex1, string hex2, float t)
    {
        Color c1 = Hex(hex1);
        Color c2 = Hex(hex2);
        int r = (int)(c1.R + (c2.R - c1.R) * t);
        int g = (int)(c1.G + (c2.G - c1.G) * t);
        int b = (int)(c1.B + (c2.B - c1.B) * t);
        return $"#{r:X2}{g:X2}{b:X2}";
    }

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

    // ── 3D depth and log colour helpers ───────────────────────────────────

    /// <summary>Fixed background for terminal-style log views (independent of current theme).</summary>
    internal static readonly Color LogBg = Color.FromArgb(6, 10, 6);

    /// <summary>Fixed foreground for terminal-style log views (phosphor green, independent of current theme).</summary>
    internal static readonly Color LogFg = Color.FromArgb(0, 218, 50);

    /// <summary>
    /// Recursively apply 3D depth effects to all controls rooted at <paramref name="root"/>:
    /// raised bevel borders on panels, luminous edge on buttons, and green-on-black
    /// for terminal-style <see cref="RichTextBox"/> log controls.
    /// Safe to call multiple times — each panel/button is only styled once (guarded by Tag).
    /// </summary>
    internal static void Apply3D(Control root)
    {
        Apply3DToSingle(root);
        foreach (Control c in root.Controls)
            Apply3D(c);
    }

    private static void Apply3DToSingle(Control c)
    {
        // Terminal-style log RichTextBox → black bg + phosphor green
        if (
            c is RichTextBox rtb
            && (
                rtb.Font.FontFamily.Name.Equals("Consolas", StringComparison.OrdinalIgnoreCase)
                || rtb.Font.FontFamily.Name.Contains("Mono", StringComparison.OrdinalIgnoreCase)
            )
        )
        {
            StyleLogRtb(rtb);
            return;
        }

        // Buttons → luminous raised border
        if (c is Button btn)
        {
            Style3DButton(btn);
            return;
        }

        // Panels (not layout containers, not trivially small) → bevel card border
        if (c is Panel pnl && pnl is not FlowLayoutPanel && pnl is not TableLayoutPanel && pnl.Height > 3 && pnl.Width > 3)
        {
            StyleRaisedPanel(pnl);
        }
    }

    /// <summary>Force a <see cref="RichTextBox"/> to the terminal green-on-black colour scheme.</summary>
    internal static void StyleLogRtb(RichTextBox rtb)
    {
        rtb.BackColor = LogBg;
        rtb.ForeColor = LogFg;
    }

    /// <summary>
    /// Paint a double-layer raised bevel border on a panel to simulate 3D depth.
    /// Top and left edges get a bright highlight; bottom and right get a dark shadow.
    /// </summary>
    internal static void StyleRaisedPanel(Panel panel)
    {
        const string Tag3D = "3d-raised";
        if (panel.Tag?.ToString() == Tag3D)
            return; // prevent double-attach
        panel.Tag = Tag3D;
        panel.Paint += (sender, e) =>
        {
            if (sender is not Panel p)
                return;
            var r = p.ClientRectangle;
            if (r.Width < 6 || r.Height < 6)
                return;
            var g = e.Graphics;
            // Outer bevel
            using var hlOuter = new Pen(Color.FromArgb(55, 255, 255, 255), 1f);
            using var shOuter = new Pen(Color.FromArgb(70, 0, 0, 0), 1f);
            g.DrawLine(hlOuter, r.Left, r.Top, r.Right - 1, r.Top);
            g.DrawLine(hlOuter, r.Left, r.Top, r.Left, r.Bottom - 1);
            g.DrawLine(shOuter, r.Left, r.Bottom - 1, r.Right - 1, r.Bottom - 1);
            g.DrawLine(shOuter, r.Right - 1, r.Top, r.Right - 1, r.Bottom - 1);
            // Inner softer bevel
            using var hlInner = new Pen(Color.FromArgb(28, 255, 255, 255), 1f);
            using var shInner = new Pen(Color.FromArgb(35, 0, 0, 0), 1f);
            g.DrawLine(hlInner, r.Left + 1, r.Top + 1, r.Right - 2, r.Top + 1);
            g.DrawLine(hlInner, r.Left + 1, r.Top + 1, r.Left + 1, r.Bottom - 2);
            g.DrawLine(shInner, r.Left + 1, r.Bottom - 2, r.Right - 2, r.Bottom - 2);
            g.DrawLine(shInner, r.Right - 2, r.Top + 1, r.Right - 2, r.Bottom - 2);
        };
    }

    /// <summary>Add a luminous border to a button to give it a raised appearance.</summary>
    internal static void Style3DButton(Button btn)
    {
        const string Tag3D = "3d-raised";
        if (btn.Tag?.ToString() == Tag3D)
            return; // prevent double-attach
        btn.Tag = Tag3D;
        btn.FlatStyle = FlatStyle.Flat;
        btn.FlatAppearance.BorderSize = 1;
        btn.FlatAppearance.BorderColor = Color.FromArgb(80, 255, 255, 255);
    }
}

// ── User Theme JSON deserialization model ──────────────────────────────────

/// <summary>De-serialisation target for user-defined theme JSON files placed in
/// <c>%LOCALAPPDATA%\RegiLattice\themes\*.json</c>.</summary>
internal sealed class UserThemeJson
{
    [System.Text.Json.Serialization.JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [System.Text.Json.Serialization.JsonPropertyName("isDark")]
    public bool IsDark { get; set; } = true;

    [System.Text.Json.Serialization.JsonPropertyName("background")]
    public string? Background { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("surface")]
    public string? Surface { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("surface2")]
    public string? Surface2 { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("text")]
    public string? Text { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("textSecondary")]
    public string? TextSecondary { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("accent")]
    public string? Accent { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("primary")]
    public string? Primary { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("secondary")]
    public string? Secondary { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("success")]
    public string? Success { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("warning")]
    public string? Warning { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("error")]
    public string? Error { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("overlay")]
    public string? Overlay { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("info")]
    public string? Info { get; set; }
}
