using Xunit;

namespace RegiLattice.GUI.Tests;

/// <summary>Tests for AppTheme colours, fonts, and helpers.</summary>
public sealed class ThemeTests
{
    [Fact]
    public void Bg_IsNotNull() => Assert.NotEqual(default, AppTheme.Bg);

    [Fact]
    public void Fg_IsNotNull() => Assert.NotEqual(default, AppTheme.Fg);

    [Fact]
    public void Accent_IsNotNull() => Assert.NotEqual(default, AppTheme.Accent);

    [Fact]
    public void Regular_FontIsSegoeUI() => Assert.Equal("Segoe UI", AppTheme.Regular.FontFamily.Name);

    [Fact]
    public void AllColors_AreDark()
    {
        // Catppuccin Mocha background should be very dark
        Assert.True(AppTheme.Bg.R < 80, $"Bg.R={AppTheme.Bg.R} expected < 80");
        Assert.True(AppTheme.Bg.G < 80, $"Bg.G={AppTheme.Bg.G} expected < 80");
        Assert.True(AppTheme.Bg.B < 80, $"Bg.B={AppTheme.Bg.B} expected < 80");
    }

    [Fact]
    public void Surface_DarkerThanOverlay()
    {
        int surfBrightness = AppTheme.Surface.R + AppTheme.Surface.G + AppTheme.Surface.B;
        int overlayBrightness = AppTheme.Overlay.R + AppTheme.Overlay.G + AppTheme.Overlay.B;
        Assert.True(surfBrightness <= overlayBrightness);
    }

    [Fact]
    public void AvailableThemes_ReturnsFourThemes()
    {
        var themes = AppTheme.AvailableThemes();
        Assert.Equal(4, themes.Length);
        Assert.Contains("catppuccin-mocha", themes);
        Assert.Contains("catppuccin-latte", themes);
        Assert.Contains("nord", themes);
        Assert.Contains("dracula", themes);
    }

    [Fact]
    public void SetTheme_SwitchesColors()
    {
        var originalBg = AppTheme.Bg;
        try
        {
            AppTheme.SetTheme("dracula");
            Assert.NotEqual(originalBg, AppTheme.Bg);
            Assert.Equal("dracula", AppTheme.CurrentThemeName());
        }
        finally
        {
            AppTheme.SetTheme("catppuccin-mocha");
        }
    }

    [Fact]
    public void SetTheme_InvalidName_KeepsCurrent()
    {
        var before = AppTheme.CurrentThemeName();
        AppTheme.SetTheme("nonexistent-theme");
        Assert.Equal(before, AppTheme.CurrentThemeName());
    }

    [Fact]
    public void CatppuccinLatte_IsLightTheme()
    {
        try
        {
            AppTheme.SetTheme("catppuccin-latte");
            // Latte background should be bright
            Assert.True(AppTheme.Bg.R > 200, $"Latte Bg.R={AppTheme.Bg.R} expected > 200");
        }
        finally
        {
            AppTheme.SetTheme("catppuccin-mocha");
        }
    }

    [Fact]
    public void SmallBold_FontExists() => Assert.Equal("Segoe UI", AppTheme.SmallBold.FontFamily.Name);

    [Fact]
    public void AccentHover_HasLowAlpha() => Assert.Equal(40, AppTheme.AccentHover.A);

    [Fact]
    public void Border_HasLowAlpha() => Assert.Equal(50, AppTheme.Border.A);

    [Fact]
    public void Separator_HasLowAlpha() => Assert.Equal(30, AppTheme.Separator.A);

    [Fact]
    public void RoundedRectPath_CreatesFigure()
    {
        var rect = new System.Drawing.Rectangle(0, 0, 100, 30);
        using var path = AppTheme.RoundedRectPath(rect, 8);
        Assert.True(path.PointCount > 0);
    }
}
