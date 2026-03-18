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
    public void AvailableThemes_ReturnsElevenThemes()
    {
        var themes = AppTheme.AvailableThemes();
        Assert.Equal(11, themes.Length);
        Assert.Contains("catppuccin-mocha", themes);
        Assert.Contains("catppuccin-latte", themes);
        Assert.Contains("nord", themes);
        Assert.Contains("dracula", themes);
        Assert.Contains("tokyo-night", themes);
        Assert.Contains("gruvbox-dark", themes);
        Assert.Contains("solarized-dark", themes);
        Assert.Contains("one-dark", themes);
        Assert.Contains("rose-pine", themes);
        Assert.Contains("everforest", themes);
        Assert.Contains("cyberpunk", themes);
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

    [Fact]
    public void DetectSystemTheme_ReturnsValidThemeKey()
    {
        var result = AppTheme.DetectSystemTheme();
        Assert.Contains(result, new[] { "catppuccin-mocha", "catppuccin-latte" });
    }

    [Fact]
    public void DetectSystemTheme_ThemeKeyExistsInAvailable()
    {
        var result = AppTheme.DetectSystemTheme();
        Assert.Contains(result, AppTheme.AvailableThemes());
    }

    // ── Additional colour properties ────────────────────────────────────

    [Fact]
    public void Surface_IsNotDefault() => Assert.NotEqual(default, AppTheme.Surface);

    [Fact]
    public void Surface2_IsNotDefault() => Assert.NotEqual(default, AppTheme.Surface2);

    [Fact]
    public void FgDim_IsNotDefault() => Assert.NotEqual(default, AppTheme.FgDim);

    [Fact]
    public void Green_IsNotDefault() => Assert.NotEqual(default, AppTheme.Green);

    [Fact]
    public void Red_IsNotDefault() => Assert.NotEqual(default, AppTheme.Red);

    [Fact]
    public void Yellow_IsNotDefault() => Assert.NotEqual(default, AppTheme.Yellow);

    [Fact]
    public void Overlay_IsNotDefault() => Assert.NotEqual(default, AppTheme.Overlay);

    [Fact]
    public void Success_IsNotDefault() => Assert.NotEqual(default, AppTheme.Success);

    [Fact]
    public void Danger_IsNotDefault() => Assert.NotEqual(default, AppTheme.Danger);

    [Fact]
    public void Info_IsNotDefault() => Assert.NotEqual(default, AppTheme.Info);

    // ── Additional computed colours ─────────────────────────────────────

    [Fact]
    public void AccentPressed_HasAlpha70() => Assert.Equal(70, AppTheme.AccentPressed.A);

    // ── Font variants ───────────────────────────────────────────────────

    [Fact]
    public void Small_FontExists() => Assert.Equal("Segoe UI", AppTheme.Small.FontFamily.Name);

    [Fact]
    public void Bold_FontIsBold() => Assert.True(AppTheme.Bold.Bold);

    [Fact]
    public void Title_FontIsBold() => Assert.True(AppTheme.Title.Bold);

    [Fact]
    public void Title_FontIsLarger() => Assert.True(AppTheme.Title.Size > AppTheme.Regular.Size);

    [Fact]
    public void Mono_FontIsConsolas() => Assert.Equal("Consolas", AppTheme.Mono.FontFamily.Name);

    // ── SetTheme case-insensitivity ─────────────────────────────────────

    [Fact]
    public void SetTheme_CaseInsensitive_Works()
    {
        try
        {
            AppTheme.SetTheme("NORD");
            Assert.Equal("nord", AppTheme.CurrentThemeName());
        }
        finally
        {
            AppTheme.SetTheme("catppuccin-mocha");
        }
    }

    [Fact]
    public void SetTheme_MixedCase_Works()
    {
        try
        {
            AppTheme.SetTheme("Dracula");
            Assert.Equal("dracula", AppTheme.CurrentThemeName());
        }
        finally
        {
            AppTheme.SetTheme("catppuccin-mocha");
        }
    }

    // ── All 4 themes colour verification ────────────────────────────────

    [Theory]
    [InlineData("catppuccin-mocha")]
    [InlineData("catppuccin-latte")]
    [InlineData("nord")]
    [InlineData("dracula")]
    [InlineData("tokyo-night")]
    [InlineData("gruvbox-dark")]
    [InlineData("solarized-dark")]
    [InlineData("one-dark")]
    [InlineData("rose-pine")]
    [InlineData("everforest")]
    [InlineData("cyberpunk")]
    public void AllThemes_HaveDistinctBgAndFg(string themeName)
    {
        try
        {
            AppTheme.SetTheme(themeName);
            Assert.NotEqual(AppTheme.Bg, AppTheme.Fg);
            Assert.NotEqual(default, AppTheme.Accent);
        }
        finally
        {
            AppTheme.SetTheme("catppuccin-mocha");
        }
    }

    [Fact]
    public void Nord_IsDarkTheme()
    {
        try
        {
            AppTheme.SetTheme("nord");
            Assert.True(AppTheme.Bg.R < 80, $"Nord Bg.R={AppTheme.Bg.R} expected < 80");
        }
        finally
        {
            AppTheme.SetTheme("catppuccin-mocha");
        }
    }

    [Fact]
    public void Dracula_IsDarkTheme()
    {
        try
        {
            AppTheme.SetTheme("dracula");
            Assert.True(AppTheme.Bg.R < 80, $"Dracula Bg.R={AppTheme.Bg.R} expected < 80");
        }
        finally
        {
            AppTheme.SetTheme("catppuccin-mocha");
        }
    }

    // ── CurrentThemeName default ────────────────────────────────────────

    [Fact]
    public void CurrentThemeName_Default_IsMocha()
    {
        AppTheme.SetTheme("catppuccin-mocha");
        Assert.Equal("catppuccin-mocha", AppTheme.CurrentThemeName());
    }

    // ── ThemeChanged event ──────────────────────────────────────────────

    [Fact]
    public void RaiseThemeChanged_InvokesSubscriber()
    {
        bool fired = false;
        void handler() => fired = true;
        AppTheme.ThemeChanged += handler;
        try
        {
            AppTheme.RaiseThemeChanged();
            Assert.True(fired);
        }
        finally
        {
            AppTheme.ThemeChanged -= handler;
        }
    }

    // ── Styled controls ─────────────────────────────────────────────────

    [Fact]
    public void StyledButton_HasCorrectProperties()
    {
        using var btn = AppTheme.StyledButton("Test", AppTheme.Accent, AppTheme.Fg, (_, _) => { });
        Assert.Equal("Test", btn.Text);
        Assert.Equal(AppTheme.Accent, btn.BackColor);
        Assert.Equal(AppTheme.Fg, btn.ForeColor);
        Assert.Equal(FlatStyle.Flat, btn.FlatStyle);
    }

    [Fact]
    public void StyledListBox_HasCorrectColors()
    {
        using var lb = AppTheme.StyledListBox();
        Assert.Equal(AppTheme.Surface, lb.BackColor);
        Assert.Equal(AppTheme.Fg, lb.ForeColor);
        Assert.Equal(SelectionMode.One, lb.SelectionMode);
    }

    [Fact]
    public void StyledTextBox_DefaultWidth()
    {
        using var tb = AppTheme.StyledTextBox();
        Assert.Equal(200, tb.Width);
        Assert.Equal(AppTheme.Overlay, tb.BackColor);
    }

    [Fact]
    public void StyledTextBox_CustomWidth()
    {
        using var tb = AppTheme.StyledTextBox(350);
        Assert.Equal(350, tb.Width);
    }

    // ── RoundedRectPath edge cases ──────────────────────────────────────

    [Fact]
    public void FillRoundedRect_RadiusZero_DoesNotThrow()
    {
        using var bmp = new System.Drawing.Bitmap(100, 100);
        using var g = Graphics.FromImage(bmp);
        using var brush = new SolidBrush(Color.Red);
        AppTheme.FillRoundedRect(g, brush, new Rectangle(0, 0, 50, 30), 0);
        // No exception means success
    }

    [Fact]
    public void DrawRoundedRect_RadiusZero_DoesNotThrow()
    {
        using var bmp = new System.Drawing.Bitmap(100, 100);
        using var g = Graphics.FromImage(bmp);
        using var pen = new Pen(Color.Red);
        AppTheme.DrawRoundedRect(g, pen, new Rectangle(0, 0, 50, 30), 0);
        // No exception means success
    }

    [Fact]
    public void DrawPill_DoesNotThrow()
    {
        using var bmp = new System.Drawing.Bitmap(200, 50);
        using var g = Graphics.FromImage(bmp);
        AppTheme.DrawPill(g, "USER", AppTheme.SmallBold, AppTheme.Green, AppTheme.Fg, 5, 5);
        // No exception means success
    }

    [Fact]
    public void RoundedRectPath_LargeRadius_DoesNotThrow()
    {
        var rect = new System.Drawing.Rectangle(0, 0, 200, 100);
        using var path = AppTheme.RoundedRectPath(rect, 50);
        Assert.True(path.PointCount > 0);
    }

    // ── Sprint 18 — New theme dark/light verification ───────────────────

    [Theory]
    [InlineData("tokyo-night")]
    [InlineData("gruvbox-dark")]
    [InlineData("solarized-dark")]
    [InlineData("one-dark")]
    [InlineData("rose-pine")]
    [InlineData("everforest")]
    [InlineData("cyberpunk")]
    public void NewDarkThemes_AreDark(string themeName)
    {
        try
        {
            AppTheme.SetTheme(themeName);
            Assert.True(AppTheme.Bg.R < 80, $"{themeName} Bg.R={AppTheme.Bg.R} expected < 80");
        }
        finally
        {
            AppTheme.SetTheme("catppuccin-mocha");
        }
    }

    [Theory]
    [InlineData("tokyo-night")]
    [InlineData("gruvbox-dark")]
    [InlineData("solarized-dark")]
    [InlineData("one-dark")]
    [InlineData("rose-pine")]
    [InlineData("everforest")]
    [InlineData("cyberpunk")]
    public void NewDarkThemes_HaveNonDefaultAccent(string themeName)
    {
        try
        {
            AppTheme.SetTheme(themeName);
            Assert.NotEqual(default, AppTheme.Accent);
            Assert.NotEqual(default, AppTheme.Green);
            Assert.NotEqual(default, AppTheme.Red);
        }
        finally
        {
            AppTheme.SetTheme("catppuccin-mocha");
        }
    }

    [Theory]
    [InlineData("tokyo-night")]
    [InlineData("gruvbox-dark")]
    [InlineData("solarized-dark")]
    [InlineData("one-dark")]
    [InlineData("rose-pine")]
    [InlineData("everforest")]
    [InlineData("cyberpunk")]
    public void NewDarkThemes_ComputedColorsWork(string themeName)
    {
        try
        {
            AppTheme.SetTheme(themeName);
            Assert.Equal(40, AppTheme.AccentHover.A);
            Assert.Equal(70, AppTheme.AccentPressed.A);
            Assert.Equal(50, AppTheme.Border.A);
            Assert.Equal(30, AppTheme.Separator.A);
        }
        finally
        {
            AppTheme.SetTheme("catppuccin-mocha");
        }
    }

    // ── Apply(Control) ──────────────────────────────────────────────────

    [Fact]
    public void Apply_SetsControlColors()
    {
        try
        {
            AppTheme.SetTheme("catppuccin-mocha");
            var panel = new Panel();
            AppTheme.Apply(panel);
            Assert.Equal(AppTheme.Bg, panel.BackColor);
            Assert.Equal(AppTheme.Fg, panel.ForeColor);
        }
        finally
        {
            AppTheme.SetTheme("catppuccin-mocha");
        }
    }

    [Fact]
    public void Apply_RecursesIntoChildControls()
    {
        try
        {
            AppTheme.SetTheme("nord");
            var parent = new Panel();
            var child = new Label();
            parent.Controls.Add(child);
            AppTheme.Apply(parent);
            Assert.Equal(AppTheme.Bg, child.BackColor);
            Assert.Equal(AppTheme.Fg, child.ForeColor);
        }
        finally
        {
            AppTheme.SetTheme("catppuccin-mocha");
        }
    }

    // ── Per-theme Success/Danger/Info ────────────────────────────────────

    [Theory]
    [InlineData("catppuccin-mocha")]
    [InlineData("catppuccin-latte")]
    [InlineData("nord")]
    [InlineData("dracula")]
    [InlineData("tokyo-night")]
    [InlineData("gruvbox-dark")]
    [InlineData("solarized-dark")]
    [InlineData("one-dark")]
    [InlineData("rose-pine")]
    [InlineData("everforest")]
    [InlineData("cyberpunk")]
    public void AllThemes_HaveSuccessDangerInfoDefined(string themeName)
    {
        try
        {
            AppTheme.SetTheme(themeName);
            Assert.NotEqual(default, AppTheme.Success);
            Assert.NotEqual(default, AppTheme.Danger);
            Assert.NotEqual(default, AppTheme.Info);
        }
        finally
        {
            AppTheme.SetTheme("catppuccin-mocha");
        }
    }

    // ── Font definitions ────────────────────────────────────────────────

    [Fact]
    public void MonoFont_IsConsolas()
    {
        Assert.Equal("Consolas", AppTheme.Mono.FontFamily.Name);
        Assert.Equal(9f, AppTheme.Mono.Size);
    }

    [Fact]
    public void SmallBoldFont_IsBold()
    {
        Assert.True(AppTheme.SmallBold.Bold);
        Assert.Equal(7.5f, AppTheme.SmallBold.Size);
    }

    // ── CurrentThemeName ────────────────────────────────────────────────

    [Fact]
    public void CurrentThemeName_MatchesSetTheme()
    {
        try
        {
            AppTheme.SetTheme("dracula");
            Assert.Equal("dracula", AppTheme.CurrentThemeName());
        }
        finally
        {
            AppTheme.SetTheme("catppuccin-mocha");
        }
    }

    // ── Accent distinct from Bg ─────────────────────────────────────────

    [Theory]
    [InlineData("catppuccin-mocha")]
    [InlineData("catppuccin-latte")]
    [InlineData("nord")]
    [InlineData("dracula")]
    [InlineData("tokyo-night")]
    [InlineData("gruvbox-dark")]
    [InlineData("solarized-dark")]
    [InlineData("one-dark")]
    [InlineData("rose-pine")]
    [InlineData("everforest")]
    [InlineData("cyberpunk")]
    public void AllThemes_AccentIsDistinctFromBg(string themeName)
    {
        try
        {
            AppTheme.SetTheme(themeName);
            Assert.NotEqual(AppTheme.Bg, AppTheme.Accent);
        }
        finally
        {
            AppTheme.SetTheme("catppuccin-mocha");
        }
    }

    // ── StyledTextBox ───────────────────────────────────────────────────

    [Fact]
    public void StyledTextBox_HasCorrectProperties()
    {
        var tb = AppTheme.StyledTextBox(300);
        Assert.Equal(AppTheme.Overlay, tb.BackColor);
        Assert.Equal(AppTheme.Fg, tb.ForeColor);
        Assert.Equal(BorderStyle.FixedSingle, tb.BorderStyle);
        Assert.Equal(300, tb.Width);
    }

    // ── StyledListBox ───────────────────────────────────────────────────

    [Fact]
    public void StyledListBox_HasCorrectProperties()
    {
        var lb = AppTheme.StyledListBox();
        Assert.Equal(AppTheme.Surface, lb.BackColor);
        Assert.Equal(AppTheme.Fg, lb.ForeColor);
        Assert.Equal(BorderStyle.None, lb.BorderStyle);
        Assert.True(lb.HorizontalScrollbar);
    }
}

// ── Sprint 26: WhatsNewDialog ──────────────────────────────────────────────

public sealed class WhatsNewDialogTests
{
    [Fact]
    public void ShouldShow_ReturnsBool()
    {
        // ShouldShow compares AppConfig.LastSeenVersion to assembly version.
        // In test context it should return a bool (no exceptions).
        var result = Forms.WhatsNewDialog.ShouldShow();
        Assert.IsType<bool>(result);
    }
}
