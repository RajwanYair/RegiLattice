namespace RegiLattice.GUI;

using RegiLattice.Core.Models;

/// <summary>
/// Generates themed icons programmatically for the main application and each package manager dialog.
/// <para>
/// <see cref="AppIcon"/> produces a multi-size ICO (16, 32, 48, 64, 128, 256 px) so Windows 11
/// can pick the appropriate size for the taskbar, Alt+Tab switcher, and File Explorer.
/// All other icons remain 32×32 (sufficient for dialog title bars and system tray).
/// </para>
/// </summary>
internal static class AppIcons
{
    private static readonly Dictionary<string, Icon> _cache = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Multi-size application icon (16 / 32 / 48 / 64 / 128 / 256 px, 32bpp RGBA).
    /// Windows 11 uses the 256px frame for the taskbar button, Alt+Tab, and File Explorer.
    /// </summary>
    internal static Icon AppIcon => GetOrCreateMultiSize("app-multi", DrawRegistryIcon);

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

    /// <summary>What's New / changelog star icon.</summary>
    internal static Icon WhatsNewIcon => GetOrCreate("whatsnew", DrawWhatsNewIcon);

    /// <summary>Preferences / gear icon.</summary>
    internal static Icon PreferencesIcon => GetOrCreate("prefs", DrawPreferencesIcon);

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
    internal static Bitmap StartupMenuBitmap => MenuBitmap("menu-startup", DrawStartupIcon);
    internal static Bitmap ServiceMenuBitmap => MenuBitmap("menu-service", DrawServiceIcon);
    internal static Bitmap PerformanceMenuBitmap => MenuBitmap("menu-perf", DrawPerformanceIcon);
    internal static Bitmap PrivacyMenuBitmap => MenuBitmap("menu-privacy", DrawPrivacyIcon);

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

    /// <summary>
    /// Creates a multi-size ICO (16, 32, 48, 64, 128, 256 px) so Windows 11 can choose the
    /// best frame for the taskbar button, Alt+Tab switcher, and File Explorer thumbnails.
    /// Each frame is a 32bpp RGBA PNG stored using the Vista+ PNG-in-ICO format.
    /// </summary>
    private static Icon GetOrCreateMultiSize(string key, Action<Graphics, int> draw)
    {
        if (_cache.TryGetValue(key, out var cached))
            return cached;

        int[] sizes = [16, 32, 48, 64, 128, 256];

        // Render each size to a PNG byte array.
        byte[][] pngs = sizes
            .Select(sz =>
            {
                using var bmp = new Bitmap(sz, sz, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                using var g = Graphics.FromImage(bmp);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                g.Clear(Color.Transparent);
                draw(g, sz);
                using var ms = new System.IO.MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            })
            .ToArray();

        // Pack PNG frames into an ICO binary stream (RFC-compatible PNG-in-ICO, Vista+).
        using var ico = new System.IO.MemoryStream();
        using var bw = new System.IO.BinaryWriter(ico);

        // ICO file header
        bw.Write((short)0); // Reserved
        bw.Write((short)1); // Type: 1 = ICO
        bw.Write((short)sizes.Length); // Image count

        // Directory entries (16 bytes each)
        int offset = 6 + sizes.Length * 16;
        for (int i = 0; i < sizes.Length; i++)
        {
            int sz = sizes[i];
            bw.Write((byte)(sz >= 256 ? 0 : sz)); // Width  (0 encodes 256)
            bw.Write((byte)(sz >= 256 ? 0 : sz)); // Height (0 encodes 256)
            bw.Write((byte)0); // Color count (0 = TrueColor)
            bw.Write((byte)0); // Reserved
            bw.Write((short)1); // Planes
            bw.Write((short)32); // Bit depth
            bw.Write((int)pngs[i].Length); // Data size in bytes
            bw.Write((int)offset); // Offset from start of file
            offset += pngs[i].Length;
        }

        // PNG frame data
        foreach (byte[] png in pngs)
            bw.Write(png);

        bw.Flush();
        ico.Position = 0;
        var icon = new Icon(ico);
        _cache[key] = icon;
        return icon;
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

    /// <summary>Service icon: dark-blue gradient with a gear/cog glyph.</summary>
    private static void DrawServiceIcon(Graphics g, int s)
    {
        using var gradient = new System.Drawing.Drawing2D.LinearGradientBrush(
            new Rectangle(0, 0, s, s),
            Color.FromArgb(60, 80, 200),
            Color.FromArgb(20, 40, 140),
            System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
        );
        using var fgPen = new Pen(Color.White, 1.5f);
        using var fgBrush = new SolidBrush(Color.White);

        AppTheme.FillRoundedRect(g, gradient, new Rectangle(2, 2, s - 4, s - 4), 5);

        // Simple gear: outer circle + inner circle + 4 teeth
        int cx = s / 2,
            cy = s / 2,
            ro = s / 3,
            ri = s / 5;
        g.DrawEllipse(fgPen, cx - ro, cy - ro, ro * 2, ro * 2);
        g.FillEllipse(fgBrush, cx - ri, cy - ri, ri * 2, ri * 2);
        foreach (int angle in new[] { 0, 90, 180, 270 })
        {
            double rad = angle * Math.PI / 180;
            int tx = (int)(cx + ro * Math.Cos(rad)),
                ty = (int)(cy + ro * Math.Sin(rad));
            g.FillRectangle(fgBrush, tx - 2, ty - 2, 4, 4);
        }
    }

    /// <summary>Startup icon: green-to-teal gradient with a rocket/launch glyph.</summary>
    private static void DrawStartupIcon(Graphics g, int s)
    {
        using var gradient = new System.Drawing.Drawing2D.LinearGradientBrush(
            new Rectangle(0, 0, s, s),
            Color.FromArgb(50, 200, 80),
            Color.FromArgb(0, 160, 130),
            System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
        );
        using var fgPen = new Pen(Color.White, 1.5f);
        using var fgBrush = new SolidBrush(Color.White);

        AppTheme.FillRoundedRect(g, gradient, new Rectangle(2, 2, s - 4, s - 4), 5);

        // Simple rocket: body triangle + flame
        int cx = s / 2;
        var body = new[] { new Point(cx, 3), new Point(cx - 4, s - 6), new Point(cx + 4, s - 6) };
        g.FillPolygon(fgBrush, body);
        g.DrawLine(fgPen, cx - 4, s - 6, cx - 6, s - 3);
        g.DrawLine(fgPen, cx + 4, s - 6, cx + 6, s - 3);
        g.DrawLine(fgPen, cx - 6, s - 3, cx + 6, s - 3);
    }

    /// <summary>Network icon: teal-to-blue gradient with a globe/network glyph.</summary>
    private static void DrawNetworkIcon(Graphics g, int s)
    {
        using var gradient = new System.Drawing.Drawing2D.LinearGradientBrush(
            new Rectangle(0, 0, s, s),
            Color.FromArgb(0, 200, 200),
            Color.FromArgb(30, 100, 220),
            System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
        );
        using var fgPen = new Pen(Color.White, 1.5f);
        using var fgBrush = new SolidBrush(Color.White);

        AppTheme.FillRoundedRect(g, gradient, new Rectangle(2, 2, s - 4, s - 4), 5);

        // Simple globe outline: circle + horizontal + vertical lines
        int cx = s / 2,
            cy = s / 2,
            r = s / 3;
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
    internal static Bitmap ExplorerMenuBitmap => MenuBitmap("menu-explorer", DrawExplorerIcon);
    internal static Bitmap CleanupMenuBitmap => MenuBitmap("menu-cleanup", DrawCleanupIcon);
    internal static Bitmap ThermometerMenuBitmap => MenuBitmap("menu-thermometer", DrawThermometerIcon);
    internal static Bitmap BandwidthMenuBitmap => MenuBitmap("menu-bandwidth", DrawBandwidthIcon);
    internal static Bitmap MacAddressMenuBitmap => MenuBitmap("menu-macaddress", DrawMacAddressIcon);

    private static void DrawThermometerIcon(Graphics g, int s)
    {
        using var gradient = new System.Drawing.Drawing2D.LinearGradientBrush(
            new Rectangle(0, 0, s, s),
            Color.FromArgb(255, 90, 50),
            Color.FromArgb(180, 30, 0),
            System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
        );
        AppTheme.FillRoundedRect(g, gradient, new Rectangle(2, 2, s - 4, s - 4), 4);
        using var pen = new Pen(Color.White, 1.5f);
        int cx = s / 2;
        // thermometer tube
        g.DrawLine(pen, cx, 4, cx, s - 7);
        // bulb
        g.DrawEllipse(pen, cx - 3, s - 9, 6, 6);
        // tick marks
        g.DrawLine(pen, cx - 4, 7, cx - 1, 7);
        g.DrawLine(pen, cx - 4, 11, cx - 1, 11);
        g.DrawLine(pen, cx - 4, 15, cx - 1, 15);
    }

    private static void DrawBandwidthIcon(Graphics g, int s)
    {
        using var gradient = new System.Drawing.Drawing2D.LinearGradientBrush(
            new Rectangle(0, 0, s, s),
            Color.FromArgb(40, 180, 255),
            Color.FromArgb(0, 100, 200),
            System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
        );
        AppTheme.FillRoundedRect(g, gradient, new Rectangle(2, 2, s - 4, s - 4), 4);
        using var pen = new Pen(Color.White, 1.5f);
        // up arrow
        int cx = s / 2 - 4;
        g.DrawLine(pen, cx, s - 5, cx, 7);
        g.DrawLine(pen, cx - 3, 11, cx, 7);
        g.DrawLine(pen, cx + 3, 11, cx, 7);
        // down arrow
        cx = s / 2 + 4;
        g.DrawLine(pen, cx, 5, cx, s - 8);
        g.DrawLine(pen, cx - 3, s - 12, cx, s - 8);
        g.DrawLine(pen, cx + 3, s - 12, cx, s - 8);
    }

    private static void DrawMacAddressIcon(Graphics g, int s)
    {
        using var gradient = new System.Drawing.Drawing2D.LinearGradientBrush(
            new Rectangle(0, 0, s, s),
            Color.FromArgb(100, 100, 220),
            Color.FromArgb(50, 50, 160),
            System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
        );
        AppTheme.FillRoundedRect(g, gradient, new Rectangle(2, 2, s - 4, s - 4), 4);
        using var pen = new Pen(Color.White, 1.5f);
        int m = 4;
        int row = s / 4;
        // Draw a simple NIC chip shape
        g.DrawRectangle(pen, m, row, s - m * 2, s - row * 2);
        g.DrawLine(pen, m - 1, row + 3, m + 2, row + 3);
        g.DrawLine(pen, m - 1, row + 7, m + 2, row + 7);
        g.DrawLine(pen, s - m - 2, row + 3, s - m + 1, row + 3);
        g.DrawLine(pen, s - m - 2, row + 7, s - m + 1, row + 7);
    }

    private static void DrawExplorerIcon(Graphics g, int s)
    {
        using var gradient = new System.Drawing.Drawing2D.LinearGradientBrush(
            new Rectangle(0, 0, s, s),
            Color.FromArgb(255, 200, 40),
            Color.FromArgb(200, 130, 0),
            System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
        );
        AppTheme.FillRoundedRect(g, gradient, new Rectangle(2, 2, s - 4, s - 4), 4);
        // Draw a simple folder shape
        using var pen = new Pen(Color.White, 1.5f);
        int m = s / 4;
        g.DrawRectangle(pen, m, m + 2, s - m * 2, s - m * 2 - 2);
        g.DrawLine(pen, m, m + 2, m + (s - m * 2) / 3, m - 1);
        g.DrawLine(pen, m + (s - m * 2) / 3, m - 1, m + (s - m * 2) / 2, m + 2);
    }

    private static void DrawCleanupIcon(Graphics g, int s)
    {
        using var gradient = new System.Drawing.Drawing2D.LinearGradientBrush(
            new Rectangle(0, 0, s, s),
            Color.FromArgb(100, 200, 100),
            Color.FromArgb(40, 130, 40),
            System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
        );
        AppTheme.FillRoundedRect(g, gradient, new Rectangle(2, 2, s - 4, s - 4), 4);
        // Draw a broom / sweep symbol
        using var pen = new Pen(Color.White, 1.5f);
        int cx = s / 2;
        g.DrawLine(pen, cx - 3, 4, cx + 3, s - 5); // handle
        g.DrawLine(pen, cx - 5, s - 5, cx + 5, s - 5); // head
        g.DrawLine(pen, cx - 4, s - 4, cx + 4, s - 4);
    }

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

    /// <summary>Performance icon: amber-to-orange gradient with a lightning bolt glyph.</summary>
    private static void DrawPerformanceIcon(Graphics g, int s)
    {
        using var gradient = new System.Drawing.Drawing2D.LinearGradientBrush(
            new Rectangle(0, 0, s, s),
            Color.FromArgb(255, 200, 0),
            Color.FromArgb(255, 100, 0),
            System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
        );
        using var fgBrush = new SolidBrush(Color.White);

        AppTheme.FillRoundedRect(g, gradient, new Rectangle(2, 2, s - 4, s - 4), 5);

        // Lightning bolt: top-right diagonal then bottom-left
        int cx = s / 2;
        var bolt = new[]
        {
            new Point(cx + 3, 3),
            new Point(cx - 1, s / 2 - 1),
            new Point(cx + 3, s / 2 - 1),
            new Point(cx - 3, s - 3),
            new Point(cx + 1, s / 2 + 1),
            new Point(cx - 3, s / 2 + 1),
        };
        g.FillPolygon(fgBrush, bolt);
    }

    /// <summary>Privacy icon: purple-to-indigo gradient with a shield glyph.</summary>
    private static void DrawPrivacyIcon(Graphics g, int s)
    {
        using var gradient = new System.Drawing.Drawing2D.LinearGradientBrush(
            new Rectangle(0, 0, s, s),
            Color.FromArgb(150, 60, 220),
            Color.FromArgb(60, 20, 160),
            System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
        );
        using var fgPen = new Pen(Color.White, 1.5f);
        using var fgBrush = new SolidBrush(Color.White);

        AppTheme.FillRoundedRect(g, gradient, new Rectangle(2, 2, s - 4, s - 4), 5);

        // Shield: rounded-top rectangle with pointed bottom
        int m = 3,
            w = s - m * 2,
            cx = s / 2;
        var shield = new[]
        {
            new Point(m, m + 3),
            new Point(m, s / 2 + 2),
            new Point(cx, s - m),
            new Point(m + w, s / 2 + 2),
            new Point(m + w, m + 3),
            new Point(cx, m),
        };
        g.DrawPolygon(fgPen, shield);
        // Lock icon inside shield
        g.DrawEllipse(fgPen, cx - 2, s / 2 - 4, 5, 4);
        g.FillRectangle(fgBrush, cx - 3, s / 2, 7, 5);
    }

    // ── Additional icon bitmaps added for v3.6.0 ─────────────────────────

    internal static Bitmap AboutMenuBitmap => MenuBitmap("menu-about", DrawAboutIcon);
    internal static Bitmap HwInfoMenuBitmap => MenuBitmap("menu-hwinfo", DrawHwInfoIcon);
    internal static Bitmap WhatsNewMenuBitmap => MenuBitmap("menu-whatsnew", DrawWhatsNewIcon);
    internal static Bitmap CheckUpdatesMenuBitmap => MenuBitmap("menu-checkupdates", DrawCheckUpdatesIcon);
    internal static Bitmap ExitMenuBitmap => MenuBitmap("menu-exit", DrawExitIcon);
    internal static Bitmap PreferencesMenuBitmap => MenuBitmap("menu-prefs", DrawPreferencesIcon);
    internal static Bitmap ImportMenuBitmap => MenuBitmap("menu-import", DrawImportIcon);

    /// <summary>About icon: teal circle with lowercase "i".</summary>
    private static void DrawAboutIcon(Graphics g, int s)
    {
        using var bg = new SolidBrush(Color.FromArgb(0, 160, 200));
        using var fg = new SolidBrush(Color.White);
        g.FillEllipse(bg, 2, 2, s - 4, s - 4);
        using var font = new Font("Segoe UI", 7f, FontStyle.Bold);
        g.DrawString("i", font, fg, (s / 2) - 2f, (s / 2) - 5f);
    }

    /// <summary>Hardware info icon: dark gray PCB chip with gold pins.</summary>
    private static void DrawHwInfoIcon(Graphics g, int s)
    {
        using var chipBrush = new SolidBrush(Color.FromArgb(60, 60, 60));
        using var pinBrush = new SolidBrush(Color.FromArgb(200, 160, 0));
        int m = 4,
            w = s - m * 2;
        g.FillRectangle(chipBrush, m, m, w, w);
        // Pins left
        for (int i = 0; i < 3; i++)
            g.FillRectangle(pinBrush, 1, m + 2 + i * 3, m - 1, 2);
        // Pins right
        for (int i = 0; i < 3; i++)
            g.FillRectangle(pinBrush, s - m, m + 2 + i * 3, m - 1, 2);
    }

    /// <summary>What's New icon: yellow star on dark background.</summary>
    private static void DrawWhatsNewIcon(Graphics g, int s)
    {
        using var bg = new SolidBrush(Color.FromArgb(40, 40, 60));
        using var star = new SolidBrush(Color.FromArgb(255, 200, 0));
        g.FillRectangle(bg, 2, 2, s - 4, s - 4);
        int cx = s / 2,
            cy = s / 2,
            r = s / 2 - 3;
        var pts = new System.Collections.Generic.List<Point>();
        for (int i = 0; i < 10; i++)
        {
            double angle = Math.PI / 2 + i * Math.PI / 5;
            int rad = (i % 2 == 0) ? r : r / 2;
            pts.Add(new Point(cx + (int)(rad * Math.Cos(angle)), cy - (int)(rad * Math.Sin(angle))));
        }
        g.FillPolygon(star, pts.ToArray());
    }

    /// <summary>Check Updates icon: green sync arrows.</summary>
    private static void DrawCheckUpdatesIcon(Graphics g, int s)
    {
        using var bg = new SolidBrush(Color.FromArgb(0, 140, 70));
        using var fg = new Pen(Color.White, 2f);
        g.FillEllipse(bg, 2, 2, s - 4, s - 4);
        int cx = s / 2,
            cy = s / 2,
            r = s / 2 - 4;
        g.DrawArc(fg, cx - r, cy - r, r * 2, r * 2, 30, 240);
        // Arrow head
        g.DrawLine(fg, cx + r - 1, cy - 2, cx + r + 2, cy + 2);
        g.DrawLine(fg, cx + r - 1, cy - 2, cx + r - 4, cy - 2);
    }

    /// <summary>Exit icon: red door with arrow.</summary>
    private static void DrawExitIcon(Graphics g, int s)
    {
        using var door = new SolidBrush(Color.FromArgb(180, 40, 40));
        using var arrow = new SolidBrush(Color.White);
        g.FillRectangle(door, 3, 2, s - 8, s - 4);
        // Arrow pointing right (exit)
        var tri = new[] { new Point(s - 5, s / 2), new Point(s - 10, s / 2 - 3), new Point(s - 10, s / 2 + 3) };
        g.FillPolygon(arrow, tri);
        g.FillRectangle(arrow, s - 13, s / 2 - 1, 5, 3);
    }

    /// <summary>Preferences icon: gear (cog).</summary>
    private static void DrawPreferencesIcon(Graphics g, int s)
    {
        using var bg = new SolidBrush(Color.FromArgb(80, 80, 100));
        using var fg = new SolidBrush(Color.FromArgb(210, 210, 230));
        g.FillEllipse(bg, 2, 2, s - 4, s - 4);
        int cx = s / 2,
            cy = s / 2,
            r = s / 2 - 3,
            ir = r - 3;
        using var pen = new Pen(fg, 2f);
        for (int i = 0; i < 8; i++)
        {
            double a1 = i * Math.PI / 4,
                a2 = a1 + Math.PI / 8;
            g.DrawLine(
                pen,
                cx + (int)(r * Math.Cos(a1)),
                cy + (int)(r * Math.Sin(a1)),
                cx + (int)((r + 2) * Math.Cos(a2)),
                cy + (int)((r + 2) * Math.Sin(a2))
            );
        }
        g.FillEllipse(new SolidBrush(AppTheme.Bg), cx - ir + 1, cy - ir + 1, (ir - 1) * 2, (ir - 1) * 2);
    }

    /// <summary>Import icon: green downward arrow into a tray.</summary>
    private static void DrawImportIcon(Graphics g, int s)
    {
        using var bg = new SolidBrush(Color.FromArgb(0, 160, 80));
        using var fg = new SolidBrush(Color.White);
        g.FillRectangle(bg, 2, 2, s - 4, s - 4);
        int cx = s / 2;
        // Tray at bottom
        g.FillRectangle(fg, 3, s - 6, s - 6, 2);
        // Down arrow
        var tri = new[] { new Point(cx, s - 8), new Point(cx - 3, s - 13), new Point(cx + 3, s - 13) };
        g.FillPolygon(fg, tri);
        g.FillRectangle(fg, cx - 1, 3, 3, s - 14);
    }

    // ── Tools / View submenu icons ────────────────────────────────────────

    internal static Bitmap PowerMenuBitmap => MenuBitmap("menu-power", DrawPowerIcon);
    internal static Bitmap WizardMenuBitmap => MenuBitmap("menu-wizard", DrawWizardIcon);
    internal static Bitmap InvertSelectionMenuBitmap => MenuBitmap("menu-invertsel", DrawInvertSelectionIcon);
    internal static Bitmap LogMenuBitmap => MenuBitmap("menu-log", DrawLogIcon);
    internal static Bitmap ExpandMenuBitmap => MenuBitmap("menu-expand", DrawExpandIcon);

    /// <summary>Power button icon: navy-to-blue gradient with a power ring and vertical bar glyph.</summary>
    private static void DrawPowerIcon(Graphics g, int s)
    {
        using var gradient = new System.Drawing.Drawing2D.LinearGradientBrush(
            new Rectangle(0, 0, s, s),
            Color.FromArgb(20, 80, 160),
            Color.FromArgb(5, 40, 100),
            System.Drawing.Drawing2D.LinearGradientMode.Vertical
        );
        using var pen = new Pen(Color.FromArgb(80, 200, 255), 1.5f);
        AppTheme.FillRoundedRect(g, gradient, new Rectangle(2, 2, s - 4, s - 4), 5);
        int cx = s / 2,
            cy = s / 2,
            r = s / 2 - 4;
        // Open power ring (gap at top)
        g.DrawArc(pen, cx - r, cy - r + 1, r * 2, r * 2, 120, 300);
        // Vertical bar through gap
        g.DrawLine(pen, cx, cy - r, cx, cy - 1);
    }

    /// <summary>Wizard/wand icon: gold-to-amber gradient with a diagonal wand and sparkle tip.</summary>
    private static void DrawWizardIcon(Graphics g, int s)
    {
        using var gradient = new System.Drawing.Drawing2D.LinearGradientBrush(
            new Rectangle(0, 0, s, s),
            Color.FromArgb(255, 200, 40),
            Color.FromArgb(200, 120, 0),
            System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
        );
        using var pen = new Pen(Color.White, 1.5f);
        AppTheme.FillRoundedRect(g, gradient, new Rectangle(2, 2, s - 4, s - 4), 5);
        // Wand handle: diagonal line bottom-left to top-right
        g.DrawLine(pen, 5, s - 5, s - 5, 4);
        // Star sparkle at tip (top-right)
        int tx = s - 5,
            ty = 4;
        g.DrawLine(pen, tx - 2, ty, tx + 2, ty);
        g.DrawLine(pen, tx, ty - 2, tx, ty + 2);
        g.DrawLine(pen, tx - 1, ty - 1, tx + 1, ty + 1);
        g.DrawLine(pen, tx + 1, ty - 1, tx - 1, ty + 1);
    }

    /// <summary>Invert selection icon: teal gradient with a split filled/empty rectangle and swap arrow.</summary>
    private static void DrawInvertSelectionIcon(Graphics g, int s)
    {
        using var gradient = new System.Drawing.Drawing2D.LinearGradientBrush(
            new Rectangle(0, 0, s, s),
            Color.FromArgb(0, 180, 180),
            Color.FromArgb(0, 100, 140),
            System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
        );
        using var pen = new Pen(Color.White, 1.5f);
        using var fill = new SolidBrush(Color.White);
        AppTheme.FillRoundedRect(g, gradient, new Rectangle(2, 2, s - 4, s - 4), 5);
        // Left square: filled (selected)
        g.FillRectangle(fill, 4, 4, 5, 6);
        // Right square: outline only (unselected → will become selected after invert)
        g.DrawRectangle(pen, s - 10, 4, 5, 6);
        // Horizontal swap arrow below
        int ay = s / 2 + 3;
        g.DrawLine(pen, 4, ay, s - 5, ay);
        g.DrawLine(pen, 4, ay, 7, ay - 2);
        g.DrawLine(pen, 4, ay, 7, ay + 2);
        g.DrawLine(pen, s - 5, ay, s - 8, ay - 2);
        g.DrawLine(pen, s - 5, ay, s - 8, ay + 2);
    }

    /// <summary>Log panel icon: dark-blue gradient with three horizontal list lines.</summary>
    private static void DrawLogIcon(Graphics g, int s)
    {
        using var gradient = new System.Drawing.Drawing2D.LinearGradientBrush(
            new Rectangle(0, 0, s, s),
            Color.FromArgb(30, 100, 200),
            Color.FromArgb(10, 50, 130),
            System.Drawing.Drawing2D.LinearGradientMode.Vertical
        );
        using var pen = new Pen(Color.White, 1.2f);
        using var accentBrush = new SolidBrush(Color.FromArgb(100, 200, 255));
        AppTheme.FillRoundedRect(g, gradient, new Rectangle(2, 2, s - 4, s - 4), 4);
        // Three horizontal text lines
        for (int i = 0; i < 4; i++)
            g.DrawLine(pen, 6, 5 + i * 3, s - 5, 5 + i * 3);
        // Small cyan bullets at the left of each line
        for (int i = 0; i < 4; i++)
            g.FillRectangle(accentBrush, 4, 4 + i * 3, 1, 1);
    }

    /// <summary>Expand categories icon: amber gradient with outward corner arrows.</summary>
    private static void DrawExpandIcon(Graphics g, int s)
    {
        using var gradient = new System.Drawing.Drawing2D.LinearGradientBrush(
            new Rectangle(0, 0, s, s),
            Color.FromArgb(255, 180, 0),
            Color.FromArgb(200, 100, 0),
            System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
        );
        using var pen = new Pen(Color.White, 1.5f);
        AppTheme.FillRoundedRect(g, gradient, new Rectangle(2, 2, s - 4, s - 4), 4);
        // Four corner arrows pointing outward (expand gesture)
        int m = 4,
            d = 3;
        g.DrawLine(pen, m, m, m + d, m); // top-left → right
        g.DrawLine(pen, m, m, m, m + d); // top-left → down
        g.DrawLine(pen, s - m, m, s - m - d, m); // top-right → left
        g.DrawLine(pen, s - m, m, s - m, m + d); // top-right → down
        g.DrawLine(pen, m, s - m, m + d, s - m); // bottom-left → right
        g.DrawLine(pen, m, s - m, m, s - m - d); // bottom-left → up
        g.DrawLine(pen, s - m, s - m, s - m - d, s - m); // bottom-right → left
        g.DrawLine(pen, s - m, s - m, s - m, s - m - d); // bottom-right → up
    }
}
