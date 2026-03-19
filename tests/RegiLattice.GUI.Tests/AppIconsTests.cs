using System.Drawing;
using RegiLattice.Core.Models;
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

    // ── Sprint 18 — New menu icon bitmaps ──────────────────────────────

    [Fact]
    public void FileMenuBitmap_IsValid16x16()
    {
        var bmp = AppIcons.FileMenuBitmap;
        AssertValidBitmap(bmp, 16);
    }

    [Fact]
    public void ViewMenuBitmap_IsValid16x16()
    {
        var bmp = AppIcons.ViewMenuBitmap;
        AssertValidBitmap(bmp, 16);
    }

    [Fact]
    public void HelpMenuBitmap_IsValid16x16()
    {
        var bmp = AppIcons.HelpMenuBitmap;
        AssertValidBitmap(bmp, 16);
    }

    [Fact]
    public void ApplyMenuBitmap_IsValid16x16()
    {
        var bmp = AppIcons.ApplyMenuBitmap;
        AssertValidBitmap(bmp, 16);
    }

    [Fact]
    public void RemoveMenuBitmap_IsValid16x16()
    {
        var bmp = AppIcons.RemoveMenuBitmap;
        AssertValidBitmap(bmp, 16);
    }

    [Fact]
    public void RefreshMenuBitmap_IsValid16x16()
    {
        var bmp = AppIcons.RefreshMenuBitmap;
        AssertValidBitmap(bmp, 16);
    }

    [Fact]
    public void ExportMenuBitmap_IsValid16x16()
    {
        var bmp = AppIcons.ExportMenuBitmap;
        AssertValidBitmap(bmp, 16);
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

// ── Category ImageList & KindBitmap tests ──────────────────────────────

public sealed class CategoryImageListTests
{
    [Fact]
    public void BuildCategoryImageList_Returns23Images()
    {
        using var list = AppIcons.BuildCategoryImageList();
        int enumCount = Enum.GetValues<CategoryIcon>().Length;
        Assert.Equal(enumCount, list.Images.Count);
    }

    [Fact]
    public void BuildCategoryImageList_ImageSize_Is16x16()
    {
        using var list = AppIcons.BuildCategoryImageList();
        Assert.Equal(new Size(16, 16), list.ImageSize);
    }

    [Fact]
    public void BuildCategoryImageList_ColorDepth_Is32Bit()
    {
        using var list = AppIcons.BuildCategoryImageList();
        Assert.Equal(ColorDepth.Depth32Bit, list.ColorDepth);
    }

    [Theory]
    [InlineData(CategoryIcon.Shield)]
    [InlineData(CategoryIcon.Globe)]
    [InlineData(CategoryIcon.Monitor)]
    [InlineData(CategoryIcon.Gear)]
    [InlineData(CategoryIcon.Lock)]
    [InlineData(CategoryIcon.HardDrive)]
    [InlineData(CategoryIcon.Cpu)]
    [InlineData(CategoryIcon.Keyboard)]
    [InlineData(CategoryIcon.Speaker)]
    [InlineData(CategoryIcon.Cloud)]
    [InlineData(CategoryIcon.App)]
    [InlineData(CategoryIcon.Terminal)]
    [InlineData(CategoryIcon.Mail)]
    [InlineData(CategoryIcon.Palette)]
    [InlineData(CategoryIcon.Notification)]
    [InlineData(CategoryIcon.Wrench)]
    [InlineData(CategoryIcon.Phone)]
    [InlineData(CategoryIcon.Desktop)]
    [InlineData(CategoryIcon.Windows)]
    [InlineData(CategoryIcon.Search)]
    [InlineData(CategoryIcon.Camera)]
    [InlineData(CategoryIcon.Printer)]
    [InlineData(CategoryIcon.Code)]
    public void BuildCategoryImageList_ContainsKeyForEachEnum(CategoryIcon icon)
    {
        using var list = AppIcons.BuildCategoryImageList();
        Assert.True(list.Images.ContainsKey(icon.ToString()), $"Missing key: {icon}");
    }

    [Theory]
    [InlineData(CategoryIcon.Shield)]
    [InlineData(CategoryIcon.Globe)]
    [InlineData(CategoryIcon.Cpu)]
    [InlineData(CategoryIcon.Code)]
    public void BuildCategoryImageList_EachImage_Is16x16(CategoryIcon icon)
    {
        using var list = AppIcons.BuildCategoryImageList();
        var img = list.Images[icon.ToString()];
        Assert.NotNull(img);
        Assert.Equal(16, img.Width);
        Assert.Equal(16, img.Height);
    }

    [Theory]
    [InlineData("Privacy", "Lock")]
    [InlineData("Network", "Globe")]
    [InlineData("Performance", "Cpu")]
    [InlineData("VS Code", "Code")]
    [InlineData("Explorer", "Palette")]
    public void CategoryImageKey_ReturnsExpectedEnumName(string category, string expected)
    {
        string key = AppIcons.CategoryImageKey(category);
        Assert.Equal(expected, key);
    }

    [Fact]
    public void CategoryImageKey_UnknownCategory_ReturnsFallback()
    {
        string key = AppIcons.CategoryImageKey("NonexistentCategory");
        // Should still return a valid enum name (likely Gear as fallback)
        Assert.False(string.IsNullOrEmpty(key));
        Assert.True(Enum.TryParse<CategoryIcon>(key, out _));
    }

    [Theory]
    [InlineData(TweakKind.Registry)]
    [InlineData(TweakKind.PowerShell)]
    [InlineData(TweakKind.SystemCommand)]
    [InlineData(TweakKind.ServiceControl)]
    [InlineData(TweakKind.ScheduledTask)]
    [InlineData(TweakKind.FileConfig)]
    [InlineData(TweakKind.GroupPolicy)]
    [InlineData(TweakKind.PackageManager)]
    public void KindBitmap_EachKind_IsValid16x16(TweakKind kind)
    {
        var bmp = AppIcons.KindBitmap(kind);
        Assert.NotNull(bmp);
        Assert.Equal(16, bmp.Width);
        Assert.Equal(16, bmp.Height);
    }

    [Fact]
    public void KindBitmap_SameKind_ReturnsCachedInstance()
    {
        var first = AppIcons.KindBitmap(TweakKind.Registry);
        var second = AppIcons.KindBitmap(TweakKind.Registry);
        Assert.Same(first, second);
    }

    [Fact]
    public void BuildCategoryImageList_CalledTwice_ReturnsDifferentInstances()
    {
        using var list1 = AppIcons.BuildCategoryImageList();
        using var list2 = AppIcons.BuildCategoryImageList();
        Assert.NotSame(list1, list2);
    }
}
