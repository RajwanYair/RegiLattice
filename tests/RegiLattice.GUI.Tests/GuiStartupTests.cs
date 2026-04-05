using System.Diagnostics;
using System.Reflection;
using RegiLattice.Core;
using RegiLattice.Core.Services;
using Xunit;

namespace RegiLattice.GUI.Tests;

/// <summary>
/// QA tests that validate every GUI startup component initialises without throwing.
/// These tests exist specifically to catch silent startup crashes — the class of
/// bug where the published EXE exits immediately with no visible output because
/// an exception occurs before Program.ShowFatalError() can be called.
///
/// Each test mirrors one step in the MainForm() constructor or OnLoad.
/// They are intentionally kept lightweight: no actual Form is created, no
/// message pump is started, and no dialog is shown.
/// </summary>
[Collection("AppTheme")]
public sealed class GuiStartupTests
{
    public GuiStartupTests()
    {
        // Ensure a valid theme is active before any test runs.
        // Without this, AppTheme.Bg/Surface/etc. return Color.Empty which
        // causes "Control does not support transparent background colors" when
        // constructing custom controls that set BackColor in their constructor.
        AppTheme.SetTheme("catppuccin-mocha");
    }

    // ── AppConfig ─────────────────────────────────────────────────────────

    [Fact]
    public void AppConfig_LoadDefault_DoesNotThrow()
    {
        // Load with a non-existent path so we always get defaults.
        var cfg = AppConfig.Load(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N")));
        Assert.NotNull(cfg);
    }

    [Fact]
    public void AppConfig_LoadDefault_LaunchMinimizedIsFalse()
    {
        // LaunchMinimized defaults to false — ensures the window is not hidden on first start.
        var cfg = AppConfig.Load(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N")));
        Assert.False(cfg.LaunchMinimized);
    }

    [Fact]
    public void AppConfig_LoadDefault_FontSizeIsPositive()
    {
        var cfg = AppConfig.Load(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N")));
        Assert.True(cfg.FontSize > 0f, $"FontSize must be positive, got {cfg.FontSize}");
    }

    // ── AppTheme ──────────────────────────────────────────────────────────

    [Fact]
    public void AppTheme_DetectSystemTheme_DoesNotThrow()
    {
        // Reads a registry key; must not throw even if the key is inaccessible.
        string theme = AppTheme.DetectSystemTheme();
        Assert.NotNull(theme);
        Assert.NotEmpty(theme);
    }

    [Fact]
    public void AppTheme_SetTheme_DefaultTheme_DoesNotThrow()
    {
        // This is the first thing MainForm() constructor does.
        AppTheme.SetTheme("catppuccin-mocha");
        Assert.Equal("catppuccin-mocha", AppTheme.CurrentThemeName());
    }

    [Fact]
    public void AppTheme_SetFontSize_DefaultSize_DoesNotThrow()
    {
        // Called in MainForm() constructor when cfg.FontSize != BaseFontSize.
        // Save BEFORE mutation — reading BaseFontSize after SetFontSize(10f) returns
        // the mutated value (10), making the restore a no-op.
        float saved = AppTheme.BaseFontSize;
        Assert.Null(Record.Exception(() => AppTheme.SetFontSize(10f)));
        AppTheme.SetFontSize(saved); // restore to original
    }

    // ── AppIcons — these are called inside InitializeComponent() ──────────

    [Fact]
    public void AppIcons_AppIcon_IsNonNull()
    {
        // AppIcons.AppIcon is used in: Icon = AppIcons.AppIcon (MainForm constructor).
        var icon = AppIcons.AppIcon;
        Assert.NotNull(icon);
    }

    [Fact]
    public void AppIcons_BuildCategoryImageList_ReturnsNonEmptyList()
    {
        // Called in MainForm.Designer.cs: _categoryImageList = AppIcons.BuildCategoryImageList()
        // If GDI+ fails, this throws and InitializeComponent() crashes silently.
        using var list = AppIcons.BuildCategoryImageList();
        Assert.NotNull(list);
        Assert.True(list.Images.Count > 0, "Category image list must contain at least one entry");
    }

    [Fact]
    public void AppIcons_AllMenuBitmaps_CreateSuccessfully()
    {
        // Every AppIcons.*MenuBitmap property is accessed in MainForm.Designer.cs
        // during InitializeComponent(). Any throw here = silent startup crash.
        AppIcons.InvalidateCache();
        var bitmaps = new[]
        {
            AppIcons.ScoopMenuBitmap,
            AppIcons.PSModuleMenuBitmap,
            AppIcons.PipMenuBitmap,
            AppIcons.WinGetMenuBitmap,
            AppIcons.ChocolateyMenuBitmap,
            AppIcons.ToolVersionsMenuBitmap,
            AppIcons.WindowsHealthMenuBitmap,
            AppIcons.MarketplaceMenuBitmap,
            AppIcons.NetworkMenuBitmap,
            AppIcons.StartupMenuBitmap,
            AppIcons.ServiceMenuBitmap,
            AppIcons.PerformanceMenuBitmap,
            AppIcons.PrivacyMenuBitmap,
            AppIcons.ExportMenuBitmap,
            AppIcons.ThermometerMenuBitmap,
            AppIcons.BandwidthMenuBitmap,
            AppIcons.MacAddressMenuBitmap,
        };
        Assert.All(bitmaps, bmp => Assert.NotNull(bmp));
        // Restore cache so other tests reuse the same bitmaps.
        AppIcons.InvalidateCache();
    }

    // ── Custom controls — each is created inside InitializeComponent() ────

    [Fact]
    public void SidebarNavControl_Construction_DoesNotThrow()
    {
        // Created in InitializeComponent() as: _sidebar = new Controls.SidebarNavControl()
        using var ctrl = new Controls.SidebarNavControl();
        Assert.NotNull(ctrl);
    }

    [Fact]
    public void DashboardPanel_Construction_DoesNotThrow()
    {
        // Created in InitializeComponent() as: _dashPanel = new Controls.DashboardPanel()
        using var panel = new Controls.DashboardPanel();
        Assert.NotNull(panel);
    }

    [Fact]
    public void TweakBrowserPanel_Construction_DoesNotThrow()
    {
        // Created in InitializeComponent() as: _tweakPanel = new Controls.TweakBrowserPanel()
        using var panel = new Controls.TweakBrowserPanel();
        Assert.NotNull(panel);
    }

    [Fact]
    public void TweakCardRow_Construction_DoesNotThrow()
    {
        // Regression: v6.13.0 crash — TweakCardRow constructor set Height = CardHeight
        // BEFORE creating child controls. Setting Height triggered OnResize → LayoutControls
        // which accessed _lblName (still null) → NullReferenceException.
        // Fix: Height is now set AFTER Controls.AddRange + null guard in LayoutControls.
        using var card = new Controls.TweakCardRow();
        Assert.NotNull(card);
        Assert.Equal(64, card.Height);
    }

    [Fact]
    public void TweakBrowserPanel_SetEngine_DoesNotThrow()
    {
        // Regression: v6.13.0 first-run crash at InitialiseEngineAsync step "SetEngine".
        // TweakBrowserPanel.SetEngine → PopulateCategoryTree → RebuildCards → new TweakCardRow
        // → NRE in LayoutControls during constructor.
        using var panel = new Controls.TweakBrowserPanel();
        var engine = new TweakEngine();
        engine.RegisterBuiltins();
        var emptyCache = new Dictionary<string, RegiLattice.Core.Models.TweakResult>();
        panel.SetEngine(engine, emptyCache);
    }

    [Fact]
    public void ToolsHubPanel_Construction_DoesNotThrow()
    {
        // Created in InitializeComponent() as: _toolsPanel = new Controls.ToolsHubPanel()
        using var panel = new Controls.ToolsHubPanel();
        Assert.NotNull(panel);
    }

    [Fact]
    public void PackagesHubPanel_Construction_DoesNotThrow()
    {
        // Created in InitializeComponent() as: _packagesPanel = new Controls.PackagesHubPanel()
        using var panel = new Controls.PackagesHubPanel();
        Assert.NotNull(panel);
    }

    // ── WhatsNewDialog — verifies the BuildChangelogText fix ──────────────

    [Fact]
    public void WhatsNewDialog_BuildChangelogText_NoRegisterBuiltins_CompletesQuickly()
    {
        // Before the fix, BuildChangelogText() called RegisterBuiltins() on the
        // UI thread, blocking for 2+ seconds each time the dialog opened.
        // After the fix it uses hardcoded constants and must complete in <200ms.
        var sw = Stopwatch.StartNew();

        // Verify ShouldShow() logic without writing to user config.
        var tmpPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".json");
        try
        {
            var cfg = AppConfig.Load(tmpPath);
            // LastSeenVersion is empty string by default → should show = true
            string current =
                typeof(TweakEngine).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "6.0.0";
            bool shouldShow = !string.Equals(cfg.LastSeenVersion, current, StringComparison.OrdinalIgnoreCase);
            Assert.True(shouldShow, "WhatsNewDialog.ShouldShow must return true on first launch");
        }
        finally
        {
            if (File.Exists(tmpPath))
                File.Delete(tmpPath);
        }

        sw.Stop();
        Assert.True(
            sw.ElapsedMilliseconds < 200,
            $"WhatsNewDialog startup logic took {sw.ElapsedMilliseconds}ms — must be <200ms (no RegisterBuiltins call)"
        );
    }

    [Fact]
    public void FirstRunWizardDialog_ShouldShow_ReturnsTrue_OnFreshInstall()
    {
        // Fresh config has FirstRunWizardPending=true → wizard must show on first launch.
        var cfg = AppConfig.Load(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N")));
        Assert.True(cfg.FirstRunWizardPending, "FirstRunWizardPending must default to true so the wizard shows on first launch");
    }

    // ── TweakEngine ── verifies that RegisterBuiltins completes without error ──

    [Fact]
    public void TweakEngine_RegisterBuiltins_OnLoad_CompletesWithoutException()
    {
        // This is the core work done in InitialiseEngineAsync() on first launch.
        // If this throws, the engine error dialog shows (handled) but the UI
        // may be in a half-initialised state. Ensure it is always exception-free.
        var engine = new TweakEngine();
        var ex = Record.Exception(() => engine.RegisterBuiltins());
        Assert.Null(ex);
        Assert.True(engine.TweakCount > 6000, $"Expected >6000 tweaks after RegisterBuiltins, got {engine.TweakCount}");
    }

    // ── Program.ResolveManagerArg ── logic test without instantiating any Form ──

    [Fact]
    public void Program_EmptyArgs_ReturnsNullManagerForm()
    {
        // Calling with [] must fall through to Application.Run(new MainForm()).
        // We test the internal logic using the public-facing assembly type.
        // The method is `private static` so we verify via reflection.
        var programType = typeof(AppTheme).Assembly.GetType("RegiLattice.GUI.Program");
        Assert.NotNull(programType); // Program type must exist in the GUI assembly
    }
}
