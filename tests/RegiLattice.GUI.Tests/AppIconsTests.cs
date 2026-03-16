using System.Drawing;
using Xunit;

namespace RegiLattice.GUI.Tests;

/// <summary>
/// Tests that AppIcons produces valid Bitmaps and Icons, including after
/// cache invalidation (theme change). Verifies the fix for the
/// ArgumentException in ToolStrip.OnPaint caused by disposed menu images.
/// </summary>
public sealed class AppIconsTests
{
    // ── Menu bitmaps (16×16) ───────────────────────────────────────────

    [Fact]
    public void ScoopMenuBitmap_IsValid16x16()
    {
        var bmp = AppIcons.ScoopMenuBitmap;
        AssertValidBitmap(bmp, 16);
    }

    [Fact]
    public void PSModuleMenuBitmap_IsValid16x16()
    {
        var bmp = AppIcons.PSModuleMenuBitmap;
        AssertValidBitmap(bmp, 16);
    }

    [Fact]
    public void PipMenuBitmap_IsValid16x16()
    {
        var bmp = AppIcons.PipMenuBitmap;
        AssertValidBitmap(bmp, 16);
    }

    [Fact]
    public void WinGetMenuBitmap_IsValid16x16()
    {
        var bmp = AppIcons.WinGetMenuBitmap;
        AssertValidBitmap(bmp, 16);
    }

    [Fact]
    public void ChocolateyMenuBitmap_IsValid16x16()
    {
        var bmp = AppIcons.ChocolateyMenuBitmap;
        AssertValidBitmap(bmp, 16);
    }

    [Fact]
    public void ToolVersionsMenuBitmap_IsValid16x16()
    {
        var bmp = AppIcons.ToolVersionsMenuBitmap;
        AssertValidBitmap(bmp, 16);
    }

    [Fact]
    public void WindowsHealthMenuBitmap_IsValid16x16()
    {
        var bmp = AppIcons.WindowsHealthMenuBitmap;
        AssertValidBitmap(bmp, 16);
    }

    [Fact]
    public void MarketplaceMenuBitmap_IsValid16x16()
    {
        var bmp = AppIcons.MarketplaceMenuBitmap;
        AssertValidBitmap(bmp, 16);
    }

    // ── Application icons (32×32) ──────────────────────────────────────

    [Fact]
    public void AppIcon_IsValid32x32()
    {
        var icon = AppIcons.AppIcon;
        Assert.NotNull(icon);
        Assert.Equal(32, icon.Width);
        Assert.Equal(32, icon.Height);
    }

    // ── Cache invalidation — bitmaps remain usable after re-creation ───

    [Fact]
    public void InvalidateCache_ThenMenuBitmaps_AreValidAndFresh()
    {
        // Get bitmaps before invalidation
        var before = AppIcons.ScoopMenuBitmap;
        AssertValidBitmap(before, 16);

        // Simulate theme change
        AppIcons.InvalidateCache();

        // New bitmaps should be valid and different instances
        var after = AppIcons.ScoopMenuBitmap;
        AssertValidBitmap(after, 16);
        Assert.NotSame(before, after);
    }

    [Fact]
    public void InvalidateCache_OldBitmaps_StillAccessible()
    {
        // Capture a reference to the old bitmap (like a ToolStripMenuItem.Image would)
        var oldBmp = AppIcons.PipMenuBitmap;

        AppIcons.InvalidateCache();

        // The old bitmap must NOT be disposed — FrameDimensionsList access must not throw.
        // This is the exact operation that crashed before the fix.
        var dims = oldBmp.FrameDimensionsList;
        Assert.NotNull(dims);
        Assert.NotEmpty(dims);
    }

    [Fact]
    public void InvalidateCache_AppIcon_RecreatesValidIcon()
    {
        var before = AppIcons.AppIcon;
        Assert.NotNull(before);

        AppIcons.InvalidateCache();

        var after = AppIcons.AppIcon;
        Assert.NotNull(after);
        Assert.Equal(32, after.Width);
        Assert.NotSame(before, after);
    }

    // ── MenuBitmap caching ─────────────────────────────────────────────

    [Fact]
    public void MenuBitmap_ReturnsSameInstanceOnRepeatedAccess()
    {
        var first = AppIcons.WinGetMenuBitmap;
        var second = AppIcons.WinGetMenuBitmap;
        Assert.Same(first, second);
    }

    // ── Helper ─────────────────────────────────────────────────────────

    private static void AssertValidBitmap(Bitmap bmp, int expectedSize)
    {
        Assert.NotNull(bmp);
        Assert.Equal(expectedSize, bmp.Width);
        Assert.Equal(expectedSize, bmp.Height);

        // This is the exact property access that threw ArgumentException
        // when the bitmap was disposed. Ensures the bitmap is fully valid.
        var dims = bmp.FrameDimensionsList;
        Assert.NotNull(dims);
    }
}
