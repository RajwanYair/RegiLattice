using System.Drawing;
using Xunit;

namespace RegiLattice.GUI.Tests;

// ─────────────────────────────────────────────────────────────────
// RoundedPanel tests
// ─────────────────────────────────────────────────────────────────

public sealed class RoundedPanelTests
{
    [Fact]
    public void CornerRadius_DefaultIs10()
    {
        using var panel = new Controls.RoundedPanel();
        Assert.Equal(10, panel.CornerRadius);
    }

    [Fact]
    public void BorderColor_DefaultIsTransparent()
    {
        using var panel = new Controls.RoundedPanel();
        Assert.Equal(Color.Transparent, panel.BorderColor);
    }

    [Fact]
    public void TintAlpha_DefaultIsZero()
    {
        using var panel = new Controls.RoundedPanel();
        Assert.Equal(0f, panel.TintAlpha);
    }

    [Fact]
    public void TintAlpha_ClampedBelow_StaysAtZero()
    {
        using var panel = new Controls.RoundedPanel();
        panel.TintAlpha = -0.5f;
        Assert.Equal(0f, panel.TintAlpha);
    }

    [Fact]
    public void TintAlpha_ClampedAbove_StaysAtOne()
    {
        using var panel = new Controls.RoundedPanel();
        panel.TintAlpha = 2f;
        Assert.Equal(1f, panel.TintAlpha);
    }

    [Fact]
    public void TintColor_DefaultIsWhite()
    {
        using var panel = new Controls.RoundedPanel();
        Assert.Equal(Color.White, panel.TintColor);
    }

    [Fact]
    public void CornerRadius_CanBeSetToZero()
    {
        using var panel = new Controls.RoundedPanel();
        panel.CornerRadius = 0;
        Assert.Equal(0, panel.CornerRadius);
    }

    [Fact]
    public void Dispose_DoesNotThrow()
    {
        var panel = new Controls.RoundedPanel();
        var ex = Record.Exception(() => panel.Dispose());
        Assert.Null(ex);
    }
}

// ─────────────────────────────────────────────────────────────────
// FluentIcons tests
// ─────────────────────────────────────────────────────────────────

public sealed class FluentIconsTests
{
    [Fact]
    public void Apply_IsCorrectUnicodeCodepoint()
    {
        Assert.Equal("\uE73E", FluentIcons.Apply);
    }

    [Fact]
    public void Remove_IsCorrectUnicodeCodepoint()
    {
        Assert.Equal("\uE711", FluentIcons.Remove);
    }

    [Fact]
    public void Search_IsCorrectUnicodeCodepoint()
    {
        Assert.Equal("\uE721", FluentIcons.Search);
    }

    [Fact]
    public void Settings_IsCorrectUnicodeCodepoint()
    {
        Assert.Equal("\uE713", FluentIcons.Settings);
    }

    [Fact]
    public void GetFont_ReturnsNonNull()
    {
        using Font f = FluentIcons.GetFont(14f);
        Assert.NotNull(f);
    }

    [Fact]
    public void GetFont_EmSizeMatchesRequest()
    {
        float size = 18f;
        using Font f = FluentIcons.GetFont(size);
        Assert.Equal(size, f.Size, precision: 1);
    }

    [Fact]
    public void CreateGlyphBitmap_ReturnsNonNull()
    {
        using Bitmap bmp = FluentIcons.CreateGlyphBitmap(FluentIcons.Apply, Color.White, 16);
        Assert.NotNull(bmp);
    }

    [Fact]
    public void CreateGlyphBitmap_HasCorrectSize()
    {
        int requestedSize = 24;
        using Bitmap bmp = FluentIcons.CreateGlyphBitmap(FluentIcons.Settings, Color.Gray, requestedSize);
        Assert.Equal(requestedSize, bmp.Width);
        Assert.Equal(requestedSize, bmp.Height);
    }

    [Fact]
    public void DrawGlyph_DoesNotThrow()
    {
        using Bitmap bmp = new Bitmap(32, 32);
        using Graphics g = Graphics.FromImage(bmp);
        var ex = Record.Exception(() =>
            FluentIcons.DrawGlyph(g, FluentIcons.Shield, Color.White, PointF.Empty, 14f));
        Assert.Null(ex);
    }
}

// ─────────────────────────────────────────────────────────────────
// CategoryExpandButton tests
// ─────────────────────────────────────────────────────────────────

public sealed class CategoryExpandButtonTests
{
    [Fact]
    public void Expanded_DefaultIsTrue()
    {
        using var btn = new Controls.CategoryExpandButton();
        Assert.True(btn.Expanded);
    }

    [Fact]
    public void SetExpanded_False_ChangesState()
    {
        using var btn = new Controls.CategoryExpandButton();
        btn.Expanded = false;
        Assert.False(btn.Expanded);
    }

    [Fact]
    public void ExpandedChanged_FiresOnToggle()
    {
        using var btn = new Controls.CategoryExpandButton();
        int fired = 0;
        btn.ExpandedChanged += (_, _) => fired++;
        btn.Expanded = false;
        Assert.Equal(1, fired);
    }

    [Fact]
    public void ExpandedChanged_DoesNotFireWhenValueUnchanged()
    {
        using var btn = new Controls.CategoryExpandButton();
        int fired = 0;
        btn.ExpandedChanged += (_, _) => fired++;
        btn.Expanded = true;  // already true
        Assert.Equal(0, fired);
    }

    [Fact]
    public void ApplyTheme_DoesNotThrow()
    {
        using var btn = new Controls.CategoryExpandButton();
        var ex = Record.Exception(() =>
            btn.ApplyTheme(Color.White, Color.FromArgb(137, 180, 250)));
        Assert.Null(ex);
    }

    [Fact]
    public void AccessibleRole_IsPushButton()
    {
        using var btn = new Controls.CategoryExpandButton();
        Assert.Equal(AccessibleRole.PushButton, btn.AccessibleRole);
    }

    [Fact]
    public void Dispose_DoesNotThrow()
    {
        var btn = new Controls.CategoryExpandButton();
        var ex = Record.Exception(() => btn.Dispose());
        Assert.Null(ex);
    }
}
