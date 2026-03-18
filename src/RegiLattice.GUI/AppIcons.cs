namespace RegiLattice.GUI;

using RegiLattice.Core.Models;

/// <summary>
/// Generates themed icons programmatically for the main application and each package manager dialog.
/// Icons are 32×32 bitmaps drawn with theme-aware colours.
/// </summary>
internal static class AppIcons
{
    private static readonly Dictionary<string, Icon> _cache = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>Registry key icon for the main application.</summary>
    internal static Icon AppIcon => GetOrCreate("app", DrawRegistryIcon);

    /// <summary>Scoop bucket icon.</summary>
    internal static Icon ScoopIcon => GetOrCreate("scoop", DrawScoopIcon);

    /// <summary>WinGet / Windows Package Manager icon.</summary>
    internal static Icon WinGetIcon => GetOrCreate("winget", DrawWinGetIcon);

    /// <summary>pip / Python icon.</summary>
    internal static Icon PipIcon => GetOrCreate("pip", DrawPipIcon);

    /// <summary>Chocolatey icon.</summary>
    internal static Icon ChocolateyIcon => GetOrCreate("choco", DrawChocolateyIcon);

    /// <summary>PowerShell modules icon.</summary>
    internal static Icon PSModuleIcon => GetOrCreate("psmodule", DrawPSModuleIcon);

    /// <summary>Tool versions / wrench icon.</summary>
    internal static Icon ToolVersionsIcon => GetOrCreate("toolversions", DrawToolVersionsIcon);

    /// <summary>Windows Health / shield icon.</summary>
    internal static Icon WindowsHealthIcon => GetOrCreate("winhealth", DrawWindowsHealthIcon);

    /// <summary>Tweak Pack / marketplace icon.</summary>
    internal static Icon MarketplaceIcon => GetOrCreate("marketplace", DrawMarketplaceIcon);

    // ── Small bitmaps for menu items (16×16) ───────────────────────────

    private static readonly Dictionary<string, Bitmap> _bmpCache = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>Returns a 16×16 bitmap suitable for ToolStripMenuItem.Image.</summary>
    internal static Bitmap MenuBitmap(string key, Action<Graphics, int> draw)
    {
        if (_bmpCache.TryGetValue(key, out var cached))
            return cached;

        const int size = 16;
        var bmp = new Bitmap(size, size);
        using (var g = Graphics.FromImage(bmp))
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(Color.Transparent);
            draw(g, size);
        }
        _bmpCache[key] = bmp;
        return bmp;
    }

    internal static Bitmap ScoopMenuBitmap => MenuBitmap("menu-scoop", DrawScoopIcon);
    internal static Bitmap PSModuleMenuBitmap => MenuBitmap("menu-ps", DrawPSModuleIcon);
    internal static Bitmap PipMenuBitmap => MenuBitmap("menu-pip", DrawPipIcon);
    internal static Bitmap WinGetMenuBitmap => MenuBitmap("menu-winget", DrawWinGetIcon);
    internal static Bitmap ChocolateyMenuBitmap => MenuBitmap("menu-choco", DrawChocolateyIcon);
    internal static Bitmap ToolVersionsMenuBitmap => MenuBitmap("menu-toolversions", DrawToolVersionsIcon);
    internal static Bitmap WindowsHealthMenuBitmap => MenuBitmap("menu-winhealth", DrawWindowsHealthIcon);
    internal static Bitmap MarketplaceMenuBitmap => MenuBitmap("menu-marketplace", DrawMarketplaceIcon);
    internal static Bitmap NetworkMenuBitmap => MenuBitmap("menu-network", DrawNetworkIcon);

    /// <summary>Invalidate the cache (call after theme change).</summary>
    /// <remarks>
    /// Icons are disposed immediately (form icons are reassigned right after).
    /// Bitmap entries are NOT disposed here because ToolStripMenuItem.Image may
    /// still reference them. The caller must reassign menu images; old bitmaps
    /// become unreferenced and are collected by GC/finalizer.
    /// </remarks>
    internal static void InvalidateCache()
    {
        foreach (var icon in _cache.Values)
            icon.Dispose();
        _cache.Clear();
        _bmpCache.Clear();
    }

    private static Icon GetOrCreate(string key, Action<Graphics, int> draw)
    {
        if (_cache.TryGetValue(key, out var cached))
            return cached;

        const int size = 32;
        using var bmp = new Bitmap(size, size);
        using (var g = Graphics.FromImage(bmp))
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(Color.Transparent);
            draw(g, size);
        }
        // Clone so the Icon owns a managed copy of the image data,
        // independent of the temporary HICON handle from GetHicon().
        using var temp = Icon.FromHandle(bmp.GetHicon());
        var icon = (Icon)temp.Clone();
        _cache[key] = icon;
        return icon;
    }

    // ── Icon drawing methods ───────────────────────────────────────────

    /// <summary>Registry key icon: vibrant blue-to-cyan gradient square with a white "R" and key glyph.</summary>
    private static void DrawRegistryIcon(Graphics g, int s)
    {
        using var gradient = new System.Drawing.Drawing2D.LinearGradientBrush(
            new Rectangle(0, 0, s, s),
            Color.FromArgb(30, 102, 245),
            Color.FromArgb(0, 195, 255),
            System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
        );
        using var fgBrush = new SolidBrush(Color.White);
        using var fgPen = new Pen(Color.White, 2f);

        // Rounded-rect background with gradient
        AppTheme.FillRoundedRect(g, gradient, new Rectangle(1, 1, s - 2, s - 2), 5);

        // "R" letter
        using var font = new Font("Segoe UI", 16f, FontStyle.Bold);
        var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        g.DrawString("R", font, fgBrush, s / 2f, s / 2f - 1, sf);

        // Small key notch at bottom-right
        g.DrawLine(fgPen, s - 8, s - 6, s - 3, s - 6);
        g.DrawLine(fgPen, s - 5, s - 8, s - 5, s - 4);
    }

    /// <summary>Scoop: vibrant cyan-to-teal gradient circle with a white "S".</summary>
    private static void DrawScoopIcon(Graphics g, int s)
    {
        using var gradient = new System.Drawing.Drawing2D.LinearGradientBrush(
            new Rectangle(0, 0, s, s),
            Color.FromArgb(0, 200, 220),
            Color.FromArgb(0, 120, 180),
            System.Drawing.Drawing2D.LinearGradientMode.Vertical
        );
        using var fgBrush = new SolidBrush(Color.White);

        g.FillEllipse(gradient, 2, 2, s - 4, s - 4);

        using var font = new Font("Segoe UI", 16f, FontStyle.Bold);
        var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        g.DrawString("S", font, fgBrush, s / 2f, s / 2f, sf);
    }

    /// <summary>WinGet: Windows blue-to-purple gradient with a white package box glyph.</summary>
    private static void DrawWinGetIcon(Graphics g, int s)
    {
        using var gradient = new System.Drawing.Drawing2D.LinearGradientBrush(
            new Rectangle(0, 0, s, s),
            Color.FromArgb(0, 120, 215),
            Color.FromArgb(80, 60, 200),
            System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
        );
        using var fgPen = new Pen(Color.White, 2f);

        AppTheme.FillRoundedRect(g, gradient, new Rectangle(1, 1, s - 2, s - 2), 5);

        // Package box outline
        int pad = 7;
        g.DrawRectangle(fgPen, pad, pad + 2, s - pad * 2, s - pad * 2 - 2);
        // Ribbon
        g.DrawLine(fgPen, s / 2, pad + 2, s / 2, s - pad);
        g.DrawLine(fgPen, pad, pad + 8, s - pad, pad + 8);
    }

    /// <summary>pip/Python: Python blue-to-gold gradient circle with "Py" text.</summary>
    private static void DrawPipIcon(Graphics g, int s)
    {
        using var gradient = new System.Drawing.Drawing2D.LinearGradientBrush(
            new Rectangle(0, 0, s, s),
            Color.FromArgb(55, 118, 171),
            Color.FromArgb(255, 212, 59),
            System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal
        );
        using var fgBrush = new SolidBrush(Color.White);

        g.FillEllipse(gradient, 2, 2, s - 4, s - 4);

        using var font = new Font("Consolas", 11f, FontStyle.Bold);
        var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        g.DrawString("Py", font, fgBrush, s / 2f, s / 2f, sf);
    }

    /// <summary>Chocolatey: rich chocolate-to-amber gradient with white "C".</summary>
    private static void DrawChocolateyIcon(Graphics g, int s)
    {
        using var gradient = new System.Drawing.Drawing2D.LinearGradientBrush(
            new Rectangle(0, 0, s, s),
            Color.FromArgb(170, 100, 40),
            Color.FromArgb(100, 50, 10),
            System.Drawing.Drawing2D.LinearGradientMode.Vertical
        );
        using var fgBrush = new SolidBrush(Color.White);

        AppTheme.FillRoundedRect(g, gradient, new Rectangle(2, 2, s - 4, s - 4), 5);

        using var font = new Font("Segoe UI", 16f, FontStyle.Bold);
        var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        g.DrawString("C", font, fgBrush, s / 2f, s / 2f, sf);
    }

    /// <summary>PowerShell: dark blue-to-bright blue gradient with ">_" prompt.</summary>
    private static void DrawPSModuleIcon(Graphics g, int s)
    {
        using var gradient = new System.Drawing.Drawing2D.LinearGradientBrush(
            new Rectangle(0, 0, s, s),
            Color.FromArgb(1, 36, 86),
            Color.FromArgb(0, 100, 200),
            System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
        );
        using var fgBrush = new SolidBrush(Color.FromArgb(238, 237, 240));

        AppTheme.FillRoundedRect(g, gradient, new Rectangle(2, 2, s - 4, s - 4), 5);

        using var font = new Font("Consolas", 12f, FontStyle.Bold);
        var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        g.DrawString(">_", font, fgBrush, s / 2f, s / 2f, sf);
    }

    /// <summary>Tool versions: silver-to-steel gradient circle with a wrench glyph.</summary>
    private static void DrawToolVersionsIcon(Graphics g, int s)
    {
        using var gradient = new System.Drawing.Drawing2D.LinearGradientBrush(
            new Rectangle(0, 0, s, s),
            Color.FromArgb(140, 140, 160),
            Color.FromArgb(70, 70, 90),
            System.Drawing.Drawing2D.LinearGradientMode.Vertical
        );
        using var fgBrush = new SolidBrush(Color.White);

        g.FillEllipse(gradient, 2, 2, s - 4, s - 4);

        using var font = new Font("Segoe UI", 14f, FontStyle.Bold);
        var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        g.DrawString("\U0001F527", font, fgBrush, s / 2f, s / 2f, sf);
    }

    /// <summary>Windows Health: green-to-emerald gradient shield with a white "+" cross.</summary>
    private static void DrawWindowsHealthIcon(Graphics g, int s)
    {
        using var gradient = new System.Drawing.Drawing2D.LinearGradientBrush(
            new Rectangle(0, 0, s, s),
            Color.FromArgb(0, 200, 80),
            Color.FromArgb(0, 120, 60),
            System.Drawing.Drawing2D.LinearGradientMode.Vertical
        );
        using var fgPen = new Pen(Color.White, 2.5f);

        // Shield rounded rect
        AppTheme.FillRoundedRect(g, gradient, new Rectangle(2, 2, s - 4, s - 4), 5);

        // White "+" cross
        int cx = s / 2;
        int cy = s / 2;
        g.DrawLine(fgPen, cx, cy - 7, cx, cy + 7);
        g.DrawLine(fgPen, cx - 7, cy, cx + 7, cy);
    }

    /// <summary>Marketplace: purple-to-magenta gradient with a white shopping bag outline.</summary>
    private static void DrawMarketplaceIcon(Graphics g, int s)
    {
        using var gradient = new System.Drawing.Drawing2D.LinearGradientBrush(
            new Rectangle(0, 0, s, s),
            Color.FromArgb(180, 80, 255),
            Color.FromArgb(100, 40, 180),
            System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
        );
        using var fgPen = new Pen(Color.White, 2f);

        AppTheme.FillRoundedRect(g, gradient, new Rectangle(2, 2, s - 4, s - 4), 5);

        // Shopping bag outline
        int pad = s / 4;
        g.DrawRectangle(fgPen, pad, pad + 3, s - pad * 2, s - pad * 2 - 2);
        // Handle arc
        g.DrawArc(fgPen, pad + 3, pad - 2, s - pad * 2 - 6, 10, 180, 180);
    }

    // ── Additional colourful menu icons ────────────────────────────────

    /// <summary>Network icon: teal-to-blue gradient with a globe/network glyph.</summary>
    private static void DrawNetworkIcon(Graphics g, int s)
    {
        using var gradient = new System.Drawing.Drawing2D.LinearGradientBrush(
            new Rectangle(0, 0, s, s),
            Color.FromArgb(0, 200, 200),
            Color.FromArgb(30, 100, 220),
            System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal);
        using var fgPen = new Pen(Color.White, 1.5f);
        using var fgBrush = new SolidBrush(Color.White);

        AppTheme.FillRoundedRect(g, gradient, new Rectangle(2, 2, s - 4, s - 4), 5);

        // Simple globe outline: circle + horizontal + vertical lines
        int cx = s / 2, cy = s / 2, r = s / 3;
        g.DrawEllipse(fgPen, cx - r, cy - r, r * 2, r * 2);
        g.DrawLine(fgPen, cx, cy - r, cx, cy + r);
        g.DrawLine(fgPen, cx - r, cy, cx + r, cy);
        // Horizontal arc hints (latitude lines)
        g.DrawArc(fgPen, cx - r, cy - r / 2, r * 2, r, 0, 180);
    }

    /// <summary>File menu: orange-to-red gradient folder glyph.</summary>
    private static void DrawFileIcon(Graphics g, int s)
    {
        using var gradient = new System.Drawing.Drawing2D.LinearGradientBrush(
            new Rectangle(0, 0, s, s),
            Color.FromArgb(255, 165, 0),
            Color.FromArgb(255, 80, 40),
            System.Drawing.Drawing2D.LinearGradientMode.Vertical
        );
        using var fgPen = new Pen(Color.White, 1.5f);

        // Folder shape
        int m = 2;
        g.FillRectangle(gradient, m, m + 3, s - m * 2, s - m * 2 - 3);
        // Tab
        g.FillRectangle(gradient, m, m, (s - m * 2) / 3, 4);
        // Highlight line
        g.DrawLine(fgPen, m + 2, m + 6, s - m - 2, m + 6);
    }

    /// <summary>View menu: teal-to-blue gradient eye glyph.</summary>
    private static void DrawViewIcon(Graphics g, int s)
    {
        using var gradient = new System.Drawing.Drawing2D.LinearGradientBrush(
            new Rectangle(0, 0, s, s),
            Color.FromArgb(0, 200, 180),
            Color.FromArgb(0, 120, 230),
            System.Drawing.Drawing2D.LinearGradientMode.Horizontal
        );
        // Eye shape
        int cy = s / 2;
        g.FillEllipse(gradient, 2, cy - 5, s - 4, 10);
        // Pupil
        using var pupilBrush = new SolidBrush(Color.White);
        g.FillEllipse(pupilBrush, s / 2 - 3, cy - 3, 6, 6);
    }

    /// <summary>Help menu: gold-to-yellow gradient circle with "?" mark.</summary>
    private static void DrawHelpIcon(Graphics g, int s)
    {
        using var gradient = new System.Drawing.Drawing2D.LinearGradientBrush(
            new Rectangle(0, 0, s, s),
            Color.FromArgb(255, 220, 50),
            Color.FromArgb(255, 170, 0),
            System.Drawing.Drawing2D.LinearGradientMode.Vertical
        );
        using var fgBrush = new SolidBrush(Color.FromArgb(60, 40, 0));

        g.FillEllipse(gradient, 2, 2, s - 4, s - 4);

        using var font = new Font("Segoe UI", 12f, FontStyle.Bold);
        var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        g.DrawString("?", font, fgBrush, s / 2f, s / 2f, sf);
    }

    /// <summary>Apply / check icon: green gradient circle with a white checkmark.</summary>
    private static void DrawApplyIcon(Graphics g, int s)
    {
        using var gradient = new System.Drawing.Drawing2D.LinearGradientBrush(
            new Rectangle(0, 0, s, s),
            Color.FromArgb(0, 220, 100),
            Color.FromArgb(0, 160, 60),
            System.Drawing.Drawing2D.LinearGradientMode.Vertical
        );
        g.FillEllipse(gradient, 2, 2, s - 4, s - 4);

        using var pen = new Pen(Color.White, 2.2f) { LineJoin = System.Drawing.Drawing2D.LineJoin.Round };
        // Checkmark
        g.DrawLines(pen, new PointF[] { new(s * 0.25f, s * 0.5f), new(s * 0.42f, s * 0.7f), new(s * 0.75f, s * 0.3f) });
    }

    /// <summary>Remove / X icon: red gradient circle with a white X.</summary>
    private static void DrawRemoveIcon(Graphics g, int s)
    {
        using var gradient = new System.Drawing.Drawing2D.LinearGradientBrush(
            new Rectangle(0, 0, s, s),
            Color.FromArgb(255, 80, 80),
            Color.FromArgb(180, 30, 30),
            System.Drawing.Drawing2D.LinearGradientMode.Vertical
        );
        g.FillEllipse(gradient, 2, 2, s - 4, s - 4);

        using var pen = new Pen(Color.White, 2.2f);
        float p = s * 0.3f;
        float q = s * 0.7f;
        g.DrawLine(pen, p, p, q, q);
        g.DrawLine(pen, q, p, p, q);
    }

    /// <summary>Refresh icon: blue gradient with circular arrow.</summary>
    private static void DrawRefreshIcon(Graphics g, int s)
    {
        using var pen = new Pen(Color.FromArgb(60, 160, 255), 2.2f)
        {
            StartCap = System.Drawing.Drawing2D.LineCap.Round,
            EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor,
        };
        g.DrawArc(pen, 3, 3, s - 6, s - 6, -60, 300);
    }

    /// <summary>Export icon: cyan gradient square with an arrow pointing out.</summary>
    private static void DrawExportIcon(Graphics g, int s)
    {
        using var gradient = new System.Drawing.Drawing2D.LinearGradientBrush(
            new Rectangle(0, 0, s, s),
            Color.FromArgb(0, 200, 220),
            Color.FromArgb(0, 120, 200),
            System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
        );
        AppTheme.FillRoundedRect(g, gradient, new Rectangle(2, 2, s - 4, s - 4), 4);

        using var pen = new Pen(Color.White, 2f) { EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor };
        int cx = s / 2;
        g.DrawLine(pen, cx, s - 6, cx, 5);
        g.DrawLine(new Pen(Color.White, 1.5f), s / 4, s - 5, s - s / 4, s - 5);
    }

    // ── Menu bitmap accessors for new icons ─────────────────────────────

    internal static Bitmap FileMenuBitmap => MenuBitmap("menu-file", DrawFileIcon);
    internal static Bitmap ViewMenuBitmap => MenuBitmap("menu-view", DrawViewIcon);
    internal static Bitmap HelpMenuBitmap => MenuBitmap("menu-help", DrawHelpIcon);
    internal static Bitmap ApplyMenuBitmap => MenuBitmap("menu-apply", DrawApplyIcon);
    internal static Bitmap RemoveMenuBitmap => MenuBitmap("menu-remove", DrawRemoveIcon);
    internal static Bitmap RefreshMenuBitmap => MenuBitmap("menu-refresh", DrawRefreshIcon);
    internal static Bitmap ExportMenuBitmap => MenuBitmap("menu-export", DrawExportIcon);

    // ── Category colour-coded icon pairs (colour + symbol) ──────────────

    /// <summary>Colour assigned to each <see cref="CategoryIcon"/> for tree/list rendering.</summary>
    private static readonly Dictionary<CategoryIcon, (Color From, Color To, string Glyph)> CategoryColors = new()
    {
        [CategoryIcon.Shield] = (Color.FromArgb(220, 50, 50), Color.FromArgb(150, 20, 20), "🛡"),
        [CategoryIcon.Globe] = (Color.FromArgb(30, 144, 255), Color.FromArgb(0, 80, 180), "🌐"),
        [CategoryIcon.Monitor] = (Color.FromArgb(0, 180, 220), Color.FromArgb(0, 100, 160), "🖥"),
        [CategoryIcon.Gear] = (Color.FromArgb(120, 120, 140), Color.FromArgb(70, 70, 90), "⚙"),
        [CategoryIcon.Lock] = (Color.FromArgb(180, 80, 220), Color.FromArgb(100, 40, 150), "🔒"),
        [CategoryIcon.HardDrive] = (Color.FromArgb(100, 140, 180), Color.FromArgb(50, 80, 120), "💾"),
        [CategoryIcon.Cpu] = (Color.FromArgb(255, 140, 0), Color.FromArgb(200, 90, 0), "⚡"),
        [CategoryIcon.Keyboard] = (Color.FromArgb(100, 180, 100), Color.FromArgb(50, 120, 50), "⌨"),
        [CategoryIcon.Speaker] = (Color.FromArgb(255, 100, 150), Color.FromArgb(200, 50, 100), "🔊"),
        [CategoryIcon.Cloud] = (Color.FromArgb(100, 180, 255), Color.FromArgb(50, 120, 200), "☁"),
        [CategoryIcon.App] = (Color.FromArgb(0, 200, 120), Color.FromArgb(0, 140, 80), "📦"),
        [CategoryIcon.Terminal] = (Color.FromArgb(1, 36, 86), Color.FromArgb(0, 100, 200), ">_"),
        [CategoryIcon.Mail] = (Color.FromArgb(255, 180, 50), Color.FromArgb(200, 120, 0), "✉"),
        [CategoryIcon.Palette] = (Color.FromArgb(255, 100, 200), Color.FromArgb(200, 50, 150), "🎨"),
        [CategoryIcon.Notification] = (Color.FromArgb(255, 200, 50), Color.FromArgb(200, 150, 0), "🔔"),
        [CategoryIcon.Wrench] = (Color.FromArgb(140, 140, 160), Color.FromArgb(90, 90, 110), "🔧"),
        [CategoryIcon.Phone] = (Color.FromArgb(50, 180, 230), Color.FromArgb(20, 120, 180), "📱"),
        [CategoryIcon.Desktop] = (Color.FromArgb(80, 160, 220), Color.FromArgb(40, 100, 160), "🖥"),
        [CategoryIcon.Windows] = (Color.FromArgb(0, 120, 215), Color.FromArgb(0, 70, 160), "🪟"),
        [CategoryIcon.Search] = (Color.FromArgb(100, 200, 100), Color.FromArgb(50, 140, 50), "🔍"),
        [CategoryIcon.Camera] = (Color.FromArgb(180, 100, 200), Color.FromArgb(120, 50, 150), "📷"),
        [CategoryIcon.Printer] = (Color.FromArgb(160, 160, 180), Color.FromArgb(100, 100, 120), "🖨"),
        [CategoryIcon.Code] = (Color.FromArgb(0, 150, 200), Color.FromArgb(0, 100, 160), "</>"),
    };

    /// <summary>Builds an <see cref="ImageList"/> with one 16×16 bitmap per <see cref="CategoryIcon"/>.</summary>
    internal static ImageList BuildCategoryImageList()
    {
        var list = new ImageList { ImageSize = new Size(16, 16), ColorDepth = ColorDepth.Depth32Bit };
        foreach (CategoryIcon ci in Enum.GetValues<CategoryIcon>())
        {
            var bmp = new Bitmap(16, 16);
            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);
                var (from, to, glyph) = CategoryColors.GetValueOrDefault(ci, (Color.Gray, Color.DarkGray, "?"));
                using var gradient = new System.Drawing.Drawing2D.LinearGradientBrush(
                    new Rectangle(0, 0, 16, 16),
                    from,
                    to,
                    System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
                );
                AppTheme.FillRoundedRect(g, gradient, new Rectangle(1, 1, 14, 14), 3);

                // Single-character or short text glyph
                using var font = glyph.Length <= 2 ? new Font("Segoe UI Emoji", 7f, FontStyle.Regular) : new Font("Consolas", 6f, FontStyle.Bold);
                using var fgBrush = new SolidBrush(Color.White);
                var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                g.DrawString(glyph, font, fgBrush, 8f, 8f, sf);
            }
            list.Images.Add(ci.ToString(), bmp);
        }
        return list;
    }

    /// <summary>Returns the <see cref="CategoryIcon"/> name for use as an ImageList key.</summary>
    internal static string CategoryImageKey(string category) => CategoryIcons.GetIcon(category).ToString();

    /// <summary>Creates a 16×16 coloured dot bitmap for a <see cref="TweakKind"/>.</summary>
    internal static Bitmap KindBitmap(TweakKind kind)
    {
        string key = $"kind-{kind}";
        return MenuBitmap(
            key,
            (g, s) =>
            {
                var color = kind switch
                {
                    TweakKind.Registry => Color.FromArgb(30, 144, 255),
                    TweakKind.PowerShell => Color.FromArgb(1, 36, 86),
                    TweakKind.SystemCommand => Color.FromArgb(255, 140, 0),
                    TweakKind.ServiceControl => Color.FromArgb(0, 180, 80),
                    TweakKind.ScheduledTask => Color.FromArgb(180, 80, 220),
                    TweakKind.FileConfig => Color.FromArgb(100, 180, 100),
                    TweakKind.GroupPolicy => Color.FromArgb(220, 50, 50),
                    TweakKind.PackageManager => Color.FromArgb(0, 200, 220),
                    _ => Color.Gray,
                };
                using var brush = new SolidBrush(color);
                g.FillEllipse(brush, 3, 3, s - 6, s - 6);
            }
        );
    }
}
