namespace RegiLattice.GUI;

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

    /// <summary>Invalidate the cache (call after theme change).</summary>
    internal static void InvalidateCache()
    {
        foreach (var icon in _cache.Values)
            icon.Dispose();
        _cache.Clear();
        foreach (var bmp in _bmpCache.Values)
            bmp.Dispose();
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
        var icon = Icon.FromHandle(bmp.GetHicon());
        _cache[key] = icon;
        return icon;
    }

    // ── Icon drawing methods ───────────────────────────────────────────

    /// <summary>Registry key icon: a blue square with a white "R" and small key glyph.</summary>
    private static void DrawRegistryIcon(Graphics g, int s)
    {
        using var bgBrush = new SolidBrush(Color.FromArgb(30, 102, 245)); // Blue
        using var fgBrush = new SolidBrush(Color.White);
        using var fgPen = new Pen(Color.White, 2f);

        // Rounded-rect background
        g.FillRectangle(bgBrush, 1, 1, s - 2, s - 2);

        // "R" letter
        using var font = new Font("Segoe UI", 16f, FontStyle.Bold);
        var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        g.DrawString("R", font, fgBrush, s / 2f, s / 2f - 1, sf);

        // Small key notch at bottom-right
        g.DrawLine(fgPen, s - 8, s - 6, s - 3, s - 6);
        g.DrawLine(fgPen, s - 5, s - 8, s - 5, s - 4);
    }

    /// <summary>Scoop: a cyan circle with a white "S" (bucket).</summary>
    private static void DrawScoopIcon(Graphics g, int s)
    {
        using var bgBrush = new SolidBrush(Color.FromArgb(0, 150, 200));
        using var fgBrush = new SolidBrush(Color.White);

        g.FillEllipse(bgBrush, 2, 2, s - 4, s - 4);

        using var font = new Font("Segoe UI", 16f, FontStyle.Bold);
        var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        g.DrawString("S", font, fgBrush, s / 2f, s / 2f, sf);
    }

    /// <summary>WinGet: a blue square with a white package box glyph.</summary>
    private static void DrawWinGetIcon(Graphics g, int s)
    {
        using var bgBrush = new SolidBrush(Color.FromArgb(0, 120, 215)); // Windows blue
        using var fgPen = new Pen(Color.White, 2f);
        using var fgBrush = new SolidBrush(Color.White);

        g.FillRectangle(bgBrush, 1, 1, s - 2, s - 2);

        // Package box outline
        int pad = 7;
        g.DrawRectangle(fgPen, pad, pad + 2, s - pad * 2, s - pad * 2 - 2);
        // Ribbon
        g.DrawLine(fgPen, s / 2, pad + 2, s / 2, s - pad);
        g.DrawLine(fgPen, pad, pad + 8, s - pad, pad + 8);
    }

    /// <summary>pip/Python: a yellow circle with "Py" text.</summary>
    private static void DrawPipIcon(Graphics g, int s)
    {
        using var bgBrush = new SolidBrush(Color.FromArgb(55, 118, 171)); // Python blue
        using var fgBrush = new SolidBrush(Color.FromArgb(255, 212, 59)); // Python yellow

        g.FillEllipse(bgBrush, 2, 2, s - 4, s - 4);

        using var font = new Font("Consolas", 11f, FontStyle.Bold);
        var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        g.DrawString("Py", font, fgBrush, s / 2f, s / 2f, sf);
    }

    /// <summary>Chocolatey: a brown rounded square with white "C".</summary>
    private static void DrawChocolateyIcon(Graphics g, int s)
    {
        using var bgBrush = new SolidBrush(Color.FromArgb(128, 80, 30)); // Chocolate brown
        using var fgBrush = new SolidBrush(Color.White);

        g.FillRectangle(bgBrush, 2, 2, s - 4, s - 4);

        using var font = new Font("Segoe UI", 16f, FontStyle.Bold);
        var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        g.DrawString("C", font, fgBrush, s / 2f, s / 2f, sf);
    }

    /// <summary>PowerShell: a dark blue rectangle with ">_" prompt.</summary>
    private static void DrawPSModuleIcon(Graphics g, int s)
    {
        using var bgBrush = new SolidBrush(Color.FromArgb(1, 36, 86)); // PS dark blue
        using var fgBrush = new SolidBrush(Color.FromArgb(238, 237, 240)); // PS light

        g.FillRectangle(bgBrush, 2, 2, s - 4, s - 4);

        using var font = new Font("Consolas", 12f, FontStyle.Bold);
        var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        g.DrawString(">_", font, fgBrush, s / 2f, s / 2f, sf);
    }

    /// <summary>Tool versions: a grey circle with a wrench-like "T" glyph.</summary>
    private static void DrawToolVersionsIcon(Graphics g, int s)
    {
        using var bgBrush = new SolidBrush(Color.FromArgb(100, 100, 120));
        using var fgBrush = new SolidBrush(Color.White);

        g.FillEllipse(bgBrush, 2, 2, s - 4, s - 4);

        using var font = new Font("Segoe UI", 14f, FontStyle.Bold);
        var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        g.DrawString("🔧", font, fgBrush, s / 2f, s / 2f, sf);
    }

    /// <summary>Windows Health: a green shield with a white "+" cross.</summary>
    private static void DrawWindowsHealthIcon(Graphics g, int s)
    {
        using var bgBrush = new SolidBrush(Color.FromArgb(34, 139, 34)); // Forest green
        using var fgBrush = new SolidBrush(Color.White);
        using var fgPen = new Pen(Color.White, 2.5f);

        // Shield shape (rounded-rect)
        g.FillRectangle(bgBrush, 2, 2, s - 4, s - 4);

        // White "+" cross
        int cx = s / 2;
        int cy = s / 2;
        g.DrawLine(fgPen, cx, cy - 7, cx, cy + 7);
        g.DrawLine(fgPen, cx - 7, cy, cx + 7, cy);
    }

    /// <summary>Marketplace: a purple square with a white shopping bag / package glyph.</summary>
    private static void DrawMarketplaceIcon(Graphics g, int s)
    {
        using var bgBrush = new SolidBrush(Color.FromArgb(130, 80, 200)); // Purple
        using var fgBrush = new SolidBrush(Color.White);
        using var fgPen = new Pen(Color.White, 2f);

        g.FillRectangle(bgBrush, 2, 2, s - 4, s - 4);

        // Shopping bag outline
        int pad = s / 4;
        g.DrawRectangle(fgPen, pad, pad + 3, s - pad * 2, s - pad * 2 - 2);
        // Handle arc
        g.DrawArc(fgPen, pad + 3, pad - 2, s - pad * 2 - 6, 10, 180, 180);
    }
}
