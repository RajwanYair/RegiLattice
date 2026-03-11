using Xunit;

namespace RegiLattice.GUI.Tests;

/// <summary>Tests for AppTheme colours, fonts, and helpers.</summary>
public sealed class ThemeTests
{
    [Fact]
    public void Bg_IsNotNull()
        => Assert.NotEqual(default, AppTheme.Bg);

    [Fact]
    public void Fg_IsNotNull()
        => Assert.NotEqual(default, AppTheme.Fg);

    [Fact]
    public void Accent_IsNotNull()
        => Assert.NotEqual(default, AppTheme.Accent);

    [Fact]
    public void Regular_FontIsSegoeUI()
        => Assert.Equal("Segoe UI", AppTheme.Regular.FontFamily.Name);

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
}
