namespace RegiLattice.GUI;

/// <summary>
/// Segoe Fluent Icons glyph constants (Unicode codepoints) and helper methods.
/// Uses the "Segoe Fluent Icons" or "Segoe MDL2 Assets" font, both ship with Windows 10/11.
/// Reference: https://learn.microsoft.com/en-us/windows/apps/design/style/segoe-fluent-icons-font
/// </summary>
internal static class FluentIcons
{
    // ── Glyph codepoints ──────────────────────────────────────────────────
    public const string Apply      = "\uE73E"; // ✔  CheckMark
    public const string Remove     = "\uE711"; // ✖  Delete
    public const string Search     = "\uE721"; // 🔍 Search
    public const string Refresh    = "\uE72C"; // 🔄 Refresh
    public const string Settings   = "\uE713"; // ⚙  Settings
    public const string Filter     = "\uE71C"; // 🔽 Filter
    public const string Export     = "\uEDE1"; // ↗  Export
    public const string Import     = "\uE8B5"; // ↙  Download
    public const string Favorite   = "\uE735"; // ★  Favorite (filled)
    public const string FavoriteOff= "\uE734"; // ☆  Favorite (outline)
    public const string Profile    = "\uE77B"; // 👤 Contact
    public const string Category   = "\uE737"; // ⊞  Tag
    public const string History    = "\uE823"; // 🕐 History
    public const string Warning    = "\uE7BA"; // ⚠  Warning
    public const string Info       = "\uE946"; // ℹ  Info
    public const string Shield     = "\uEA18"; // 🛡  Shield
    public const string Lock       = "\uE72E"; // 🔒 Lock
    public const string Unlock     = "\uE785"; // 🔓 Unlock
    public const string Snapshot   = "\uE722"; // 📷 Camera (snapshot)
    public const string Restore    = "\uE777"; // ↩  Undo
    public const string Report     = "\uE9F9"; // 📋 ClipboardList
    public const string Chart      = "\uE9D2"; // 📊 BarChart
    public const string Star       = "\uE735"; // ★  Star
    public const string Tag        = "\uE8EC"; // 🏷  Tag
    public const string Add        = "\uE710"; // ＋  Add
    public const string Edit       = "\uE70F"; // ✏  Edit
    public const string Save       = "\uE74E"; // 💾 Save
    public const string Open       = "\uED43"; // 📂 OpenFolderHorizontal
    public const string Close      = "\uE8BB"; // ✕  ChromeClose
    public const string Back       = "\uE72B"; // ← Back
    public const string Forward    = "\uE72A"; // → Forward
    public const string Up         = "\uE74A"; // ↑ Up
    public const string Down       = "\uE74B"; // ↓ Down
    public const string Play       = "\uE768"; // ▶ Play
    public const string Pause      = "\uE769"; // ⏸ Pause
    public const string SortAsc    = "\uE74A"; // Sort ascending
    public const string SortDesc   = "\uE74B"; // Sort descending
    public const string Computer   = "\uE7F8"; // 💻 ThisPC
    public const string Network    = "\uE704"; // Network
    public const string Security   = "\uE8D7"; // Security
    public const string Package    = "\uE7B8"; // Package
    public const string Terminal   = "\uE756"; // CommandPrompt
    public const string Eye        = "\uE7B3"; // View
    public const string EyeHide    = "\uED1A"; // Hide
    public const string Health     = "\uE95E"; // Heart
    public const string Scan       = "\uECA5"; // Scan
    public const string Plugin     = "\uE737"; // Plugin (reuse Tag)
    public const string Update     = "\uE72C"; // Update (reuse Refresh)
    public const string Compliance = "\uEB4D"; // Certificate
    public const string Schedule   = "\uE787"; // Calendar
    public const string Analytics  = "\uE9D2"; // Analytics
    public const string Dashboard  = "\uECA5"; // Dashboard

    // ── Font helpers ──────────────────────────────────────────────────────
    private static Font? _fluentFont;

    /// <summary>
    /// Returns the best available icon font at the given em-size.
    /// Prefers "Segoe Fluent Icons" (Win11), falls back to "Segoe MDL2 Assets" (Win10).
    /// </summary>
    public static Font GetFont(float emSize = 14f)
    {
        if (_fluentFont is null || Math.Abs(_fluentFont.Size - emSize) > 0.1f)
        {
            _fluentFont?.Dispose();
            _fluentFont = TryMakeFont("Segoe Fluent Icons", emSize)
                        ?? TryMakeFont("Segoe MDL2 Assets", emSize)
                        ?? SystemFonts.SmallCaptionFont;
        }
        return _fluentFont!;
    }

    /// <summary>Measure the pixel size of a single icon glyph at the given em-size.</summary>
    public static SizeF MeasureGlyph(string glyph, float emSize = 14f)
    {
        using Graphics g = Graphics.FromHwnd(IntPtr.Zero);
        return g.MeasureString(glyph, GetFont(emSize));
    }

    /// <summary>Draw a single icon glyph into a Graphics context.</summary>
    public static void DrawGlyph(Graphics g, string glyph, Color color, PointF location, float emSize = 14f)
    {
        using SolidBrush b = new SolidBrush(color);
        g.DrawString(glyph, GetFont(emSize), b, location);
    }

    /// <summary>Create a Bitmap of a single icon glyph (useful for ImageList).</summary>
    public static Bitmap CreateGlyphBitmap(string glyph, Color color, int size = 16)
    {
        var bmp = new Bitmap(size, size);
        using (Graphics g = Graphics.FromImage(bmp))
        {
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            g.Clear(Color.Transparent);
            float emSize = size * 0.72f;
            DrawGlyph(g, glyph, color, new PointF(0, 0), emSize);
        }
        return bmp;
    }

    private static Font? TryMakeFont(string family, float size)
    {
        try
        {
            var f = new Font(family, size, GraphicsUnit.Pixel);
            return f.Name == family ? f : null;
        }
        catch
        {
            return null;
        }
    }
}
