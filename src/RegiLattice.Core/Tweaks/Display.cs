namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Display
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "display-force-96dpi",
            Label = "Force 96 DPI (100% Scaling)",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Forces the display to use 96 DPI (100% scaling), disabling any high-DPI scaling that Windows may apply.",
            Tags = ["display", "dpi", "scaling", "96dpi"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "LogPixels", 96)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "LogPixels")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "LogPixels", 96)],
        },
        new TweakDef
        {
            Id = "display-disable-transparency",
            Label = "Disable Transparency Effects",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the acrylic/blur transparency effects on the taskbar, Start menu, and window backgrounds.",
            Tags = ["display", "transparency", "performance", "visual"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "EnableTransparency", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "EnableTransparency", 1)],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "EnableTransparency", 0),
            ],
        },
        new TweakDef
        {
            Id = "display-disable-animations",
            Label = "Disable Window Animations",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables minimize and maximize window animations for snappier window management.",
            Tags = ["display", "animation", "performance", "visual"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "MinAnimate", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "MinAnimate", "1")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "MinAnimate", "0")],
        },
        new TweakDef
        {
            Id = "display-disable-wallpaper-compression",
            Label = "Disable Wallpaper JPEG Compression",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets wallpaper JPEG import quality to 100%, preventing Windows from compressing desktop wallpapers.",
            Tags = ["display", "wallpaper", "quality", "compression"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "JPEGImportQuality", 100)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "JPEGImportQuality")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "JPEGImportQuality", 100)],
        },
        new TweakDef
        {
            Id = "display-accent-title-bars",
            Label = "Accent Color on Title Bars",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Shows the Windows accent colour on title bars and window borders.",
            Tags = ["display", "accent", "color", "titlebar", "dwm"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "ColorPrevalence", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "ColorPrevalence", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "ColorPrevalence", 1)],
        },
        new TweakDef
        {
            Id = "display-disable-edge-swipe",
            Label = "Disable Screen Edge Swipe",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the screen edge swipe gesture that opens the Charms bar or Action Center on touch devices.",
            Tags = ["display", "edge", "swipe", "gesture", "touch"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EdgeUI"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EdgeUI", "AllowEdgeSwipe", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EdgeUI", "AllowEdgeSwipe")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EdgeUI", "AllowEdgeSwipe", 0)],
        },
        new TweakDef
        {
            Id = "display-disable-adaptive-brightness",
            Label = "Disable Adaptive Brightness",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Adaptive Brightness sensor service. Prevents automatic screen brightness changes based on ambient light. Default: Enabled (3=Manual). Recommended: Disabled (4).",
            Tags = ["display", "brightness", "adaptive", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SensrSvc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SensrSvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SensrSvc", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SensrSvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "display-dpi-override",
            Label = "Set Display Scaling DPI Override",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Forces display scaling to 100% (96 DPI) using the legacy DPI override. Disables DPI virtualization for crisp rendering. Default: System-managed. Recommended: 96 DPI for external monitors.",
            Tags = ["display", "dpi", "scaling", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "Win8DpiScaling", 1),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "LogPixels", 96),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "Win8DpiScaling"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "LogPixels"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "Win8DpiScaling", 1)],
        },
        new TweakDef
        {
            Id = "display-disable-adaptive-brightness-icm",
            Label = "Disable Adaptive Brightness (ICM)",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables adaptive brightness via ICM display calibration. Prevents automatic brightness adjustments based on content. Default: Enabled. Recommended: Disabled for consistent brightness.",
            Tags = ["display", "brightness", "icm", "calibration"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\ICM\Calibration"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SensrSvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SensrSvc", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SensrSvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "display-hardware-cursor",
            Label = "Force Hardware Cursor",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Forces hardware cursor rendering and disables smooth scrolling. Reduces input lag and cursor rendering overhead. Default: Smooth scrolling on. Recommended: Hardware cursor for gaming.",
            Tags = ["display", "cursor", "hardware", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "SmoothScroll", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "SmoothScroll", "1")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "SmoothScroll", "0")],
        },
        new TweakDef
        {
            Id = "display-disable-transparency-effect",
            Label = "Disable Window Transparency Effect",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables window transparency and acrylic blur effects. Improves rendering performance on integrated GPUs. Default: Enabled. Recommended: Disabled for performance.",
            Tags = ["display", "transparency", "acrylic", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "EnableTransparency", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "EnableTransparency", 1)],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "EnableTransparency", 0),
            ],
        },
        new TweakDef
        {
            Id = "display-disable-auto-color-mgmt",
            Label = "Disable Auto Color Management",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Windows Desktop Window Manager automatic color management for manual ICC profile control. Default: Enabled. Recommended: Disabled for color-critical work.",
            Tags = ["display", "color", "management", "dwm", "icc"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "EnableAutoColorManagement", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "EnableAutoColorManagement")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "EnableAutoColorManagement", 0)],
        },
        new TweakDef
        {
            Id = "display-disable-cursor-shadow",
            Label = "Disable Cursor Drop Shadow",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes the drop shadow rendered under the mouse cursor. Very slightly reduces compositor workload. Default: Enabled.",
            Tags = ["display", "cursor", "shadow", "performance", "rendering"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "CursorShadow", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "CursorShadow", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "CursorShadow", 0)],
        },
        new TweakDef
        {
            Id = "display-reduce-blur-intensity",
            Label = "Reduce DWM Blur Intensity (50%)",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets the DWM blur-behind intensity to 50. Reduces the visual weight of frosted-glass Mica/Acrylic effects. Default: Not set (OS default full blur).",
            Tags = ["display", "dwm", "blur", "aero", "visual"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "BlurIntensity", 50)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "BlurIntensity")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "BlurIntensity", 50)],
        },
        new TweakDef
        {
            Id = "display-force-aero-composition",
            Label = "Force Desktop Composition (Aero)",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Explicitly enables Desktop Window Manager composition (Aero). Ensures DWM is active even on systems where it was manually disabled. Default: Enabled.",
            Tags = ["display", "dwm", "aero", "composition", "visual"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "Composition", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "Composition")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "Composition", 1)],
        },
        new TweakDef
        {
            Id = "display-disable-hdr-streaming",
            Label = "Disable HDR Streaming",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables HDR video streaming. Prevents auto-HDR tone mapping for streaming content. Default: enabled if HDR supported.",
            Tags = ["display", "hdr", "streaming", "video"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\VideoSettings"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\VideoSettings", "EnableHDRForPlayback", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\VideoSettings", "EnableHDRForPlayback")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\VideoSettings", "EnableHDRForPlayback", 0)],
        },
        new TweakDef
        {
            Id = "display-disable-auto-color-management",
            Label = "Disable Auto Color Management",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Windows auto color management. Use when accurate colors are managed by a dedicated ICC profile. Default: enabled.",
            Tags = ["display", "color", "management", "icc"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\AutoColorManagement"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\AutoColorManagement", "Enable", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\AutoColorManagement", "Enable")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\AutoColorManagement", "Enable", 0)],
        },
        new TweakDef
        {
            Id = "display-disable-window-animations",
            Label = "Disable Window Animations",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables minimize/maximize window animations. Makes window transitions instant. Default: animated.",
            Tags = ["display", "animation", "window", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "MinAnimate", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "MinAnimate", "1")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "MinAnimate", "0")],
        },
        new TweakDef
        {
            Id = "display-set-dpi-scaling-override",
            Label = "Force Per-Monitor DPI Awareness",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Forces per-monitor DPI awareness for legacy applications system-wide. Reduces blurry scaling. Default: system-aware.",
            Tags = ["display", "dpi", "scaling", "per-monitor"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\SideBySide", "PreferExternalManifest", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\SideBySide", "PreferExternalManifest")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\SideBySide", "PreferExternalManifest", 1)],
        },
        new TweakDef
        {
            Id = "display-disable-animation-effects",
            Label = "Disable Window Animation Effects",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables window minimize/maximize and transition animation effects. Snappier window management. Default: animated.",
            Tags = ["display", "animation", "effects", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "MinAnimate", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "MinAnimate", "1")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "MinAnimate", "0")],
        },
        new TweakDef
        {
            Id = "display-disable-screensaver-policy",
            Label = "Disable Screen Saver via Policy",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables screen saver activation via Group Policy. Prevents screen saver from interrupting work. Default: user-controlled.",
            Tags = ["display", "screensaver", "policy", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop", "ScreenSaveActive", "0")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop", "ScreenSaveActive")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop", "ScreenSaveActive", "0")],
        },
        new TweakDef
        {
            Id = "display-set-font-smoothing-gamma",
            Label = "Set Font Smoothing Gamma",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets the ClearType font smoothing gamma correction to optimal value. Improves text rendering contrast on LCD displays. Default: system default.",
            Tags = ["display", "font", "gamma", "cleartype"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothingGamma", 1400)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothingGamma")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothingGamma", 1400)],
        },
        new TweakDef
        {
            Id = "display-increase-icon-spacing-horizontal",
            Label = "Increase Horizontal Icon Spacing",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Increases horizontal icon spacing on the desktop from the default -1125 to -1500 twips. Reduces icon overlap on high-DPI displays.",
            Tags = ["display", "icons", "spacing", "desktop"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "IconSpacing", "-1500")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "IconSpacing", "-1125")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "IconSpacing", "-1500")],
        },
        new TweakDef
        {
            Id = "display-increase-icon-spacing-vertical",
            Label = "Increase Vertical Icon Spacing",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Increases vertical icon spacing on the desktop from the default -1125 to -1500 twips. Reduces icon overlap on high-DPI displays.",
            Tags = ["display", "icons", "spacing", "desktop"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "IconVerticalSpacing", "-1500")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "IconVerticalSpacing", "-1125")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "IconVerticalSpacing", "-1500")],
        },
        new TweakDef
        {
            Id = "display-set-scrollbar-width",
            Label = "Set Narrow Scrollbar Width",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Reduces the scrollbar width from the default -255 to -200 twips. Gives more screen real estate for content. Requires logoff.",
            Tags = ["display", "scrollbar", "width", "compact"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "ScrollWidth", "-200")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "ScrollWidth", "-255")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "ScrollWidth", "-200")],
        },
        new TweakDef
        {
            Id = "display-set-scrollbar-height",
            Label = "Set Narrow Scrollbar Height",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Reduces the horizontal scrollbar height from the default -255 to -200 twips. More compact UI. Requires logoff.",
            Tags = ["display", "scrollbar", "height", "compact"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "ScrollHeight", "-200")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "ScrollHeight", "-255")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "ScrollHeight", "-200")],
        },
        new TweakDef
        {
            Id = "display-set-border-width",
            Label = "Set Thin Window Border",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Reduces the window border width from the default -15 to -1 twips for a thinner, modern look. Requires logoff.",
            Tags = ["display", "border", "width", "thin"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "BorderWidth", "-1")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "BorderWidth", "-15")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "BorderWidth", "-1")],
        },
        new TweakDef
        {
            Id = "display-disable-window-shake",
            Label = "Disable Aero Shake (Minimize All)",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the Aero Shake feature that minimizes all other windows when shaking a title bar. Prevents accidental minimization.",
            Tags = ["display", "aero", "shake", "minimize"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisallowShaking", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisallowShaking")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisallowShaking", 1)],
        },
        new TweakDef
        {
            Id = "display-enable-text-cursor-indicator",
            Label = "Enable Text Cursor Indicator",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables a coloured visual indicator at the text cursor position. Improves cursor visibility in dense text.",
            Tags = ["display", "cursor", "indicator", "accessibility"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Accessibility"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Accessibility", "Configuration", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Accessibility", "Configuration")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Accessibility", "Configuration", 1)],
        },
        new TweakDef
        {
            Id = "display-set-tooltip-initial-delay",
            Label = "Reduce Tooltip Initial Delay",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Reduces the initial tooltip popup delay from 400ms to 100ms. Shows hover info faster.",
            Tags = ["display", "tooltip", "delay", "speed"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Mouse"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseHoverTime", "100")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseHoverTime", "400")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseHoverTime", "100")],
        },
        // ── Sprint 19 additions ────────────────────────────────────────────
        new TweakDef
        {
            Id = "display-disable-windows-ink-workspace",
            Label = "Disable Windows Ink Workspace",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Windows Ink Workspace overlay. Removes the pen/ink button from the taskbar area. Default: enabled.",
            Tags = ["display", "ink", "workspace", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace", "AllowWindowsInkWorkspace", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace", "AllowWindowsInkWorkspace")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace", "AllowWindowsInkWorkspace", 0)],
        },
        new TweakDef
        {
            Id = "display-force-disable-hdr",
            Label = "Force Disable HDR",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Forces HDR to be disabled system-wide. Useful for monitors that don't support HDR properly. Default: auto-detect.",
            Tags = ["display", "hdr", "disable"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings", "EnableHDRForPlayback", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings", "EnableHDRForPlayback")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings", "EnableHDRForPlayback", 0)],
        },
        new TweakDef
        {
            Id = "display-set-color-depth-32bit",
            Label = "Force 32-bit Color Depth",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets the desktop color depth to 32-bit (True Color). Ensures maximum color quality. Default: 32-bit on most systems.",
            Tags = ["display", "color", "depth", "32bit"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "ShellIconBPP", "32")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "ShellIconBPP")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "ShellIconBPP", "32")],
        },
        new TweakDef
        {
            Id = "display-disable-auto-rotation",
            Label = "Disable Auto-Rotation",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables automatic screen rotation on tablets and convertible laptops. Default: enabled on tablets.",
            Tags = ["display", "rotation", "tablet", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\AutoRotation"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\AutoRotation", "Enable", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\AutoRotation", "Enable", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\AutoRotation", "Enable", 0)],
        },
        new TweakDef
        {
            Id = "display-set-caption-button-height",
            Label = "Increase Title Bar Button Height",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Increases the height of window title bar caption buttons (minimize/maximize/close) for easier targeting. Default: -270.",
            Tags = ["display", "titlebar", "caption", "height"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "CaptionHeight", "-330")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "CaptionHeight", "-270")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "CaptionHeight", "-330")],
        },
        new TweakDef
        {
            Id = "display-disable-mouse-hover-select",
            Label = "Disable Mouse Hover Window Activation",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents windows from activating when the mouse hovers over them (X-Mouse behaviour). Default: disabled.",
            Tags = ["display", "mouse", "hover", "activation"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ActiveWndTrkMouse", "0")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ActiveWndTrkMouse")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ActiveWndTrkMouse", "0")],
        },
        new TweakDef
        {
            Id = "display-force-full-screen-optimize",
            Label = "Disable Fullscreen Optimizations Globally",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows fullscreen optimizations system-wide. Can improve performance in legacy games. Default: enabled.",
            Tags = ["display", "fullscreen", "optimization", "gaming"],
            RegistryKeys = [@"HKEY_CURRENT_USER\System\GameConfigStore"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_FSEBehaviorMode", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_FSEBehaviorMode")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_FSEBehaviorMode", 2)],
        },
        new TweakDef
        {
            Id = "display-set-menu-animation-fade",
            Label = "Set Menu Animation to Fade",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets menu animation style to fade instead of scroll. Feels smoother on modern hardware. Default: scroll.",
            Tags = ["display", "menu", "animation", "fade"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "MenuAnimation", "1")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "MenuAnimation", "0")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "MenuAnimation", "1")],
        },
        new TweakDef
        {
            Id = "display-disable-peek-desktop",
            Label = "Disable Aero Peek at Desktop",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the Aero Peek feature that shows the desktop when hovering over the Show Desktop button. Default: enabled.",
            Tags = ["display", "peek", "desktop", "aero"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "EnableAeroPeek", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "EnableAeroPeek", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "EnableAeroPeek", 0)],
        },
    ];
}

// ── Merged from NightLight.cs ──────────────────────────────────────────────────

internal static class NightLight
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "night-enable-hdr",
            Label = "Enable HDR Video Playback",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables HDR video playback on HDR-capable displays. Requires hardware support. Default: Disabled.",
            Tags = ["night-light", "hdr", "display", "video"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings", "EnableHDR", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings", "EnableHDR", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings", "EnableHDR", 1)],
        },
        new TweakDef
        {
            Id = "night-hdr-auto-brightness",
            Label = "Enable Auto HDR Brightness",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables automatic brightness adjustment for HDR content. Optimises SDR-to-HDR content mapping.",
            Tags = ["night-light", "hdr", "brightness", "auto"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings", "AutoHDR", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings", "AutoHDR", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings", "AutoHDR", 1)],
        },
        new TweakDef
        {
            Id = "night-disable-cabc",
            Label = "Disable Content Adaptive Brightness",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Content Adaptive Brightness Control (CABC) which adjusts screen brightness based on content. Can cause distracting brightness shifts. Recommended: Disabled for content creation.",
            Tags = ["night-light", "brightness", "cabc", "display"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "CABCEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "CABCEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "CABCEnabled", 0)],
        },
        new TweakDef
        {
            Id = "night-disable-adaptive-colour",
            Label = "Disable Adaptive Colour",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the adaptive colour feature that shifts display colours based on ambient light. Provides consistent colour output.",
            Tags = ["night-light", "colour", "adaptive", "calibration"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\ICM"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\ICM", "AdaptiveColorEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\ICM", "AdaptiveColorEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\ICM", "AdaptiveColorEnabled", 0)],
        },
        new TweakDef
        {
            Id = "night-enable-wcg",
            Label = "Enable Wide Colour Gamut (WCG)",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables Wide Colour Gamut support for richer colours on compatible displays. Default: Disabled.",
            Tags = ["night-light", "wcg", "colour", "gamut", "display"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings", "EnableWCG", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings", "EnableWCG", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings", "EnableWCG", 1)],
        },
        new TweakDef
        {
            Id = "night-disable-hdr-streaming",
            Label = "Disable HDR for Streaming Video",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables HDR playback for streaming video apps. Saves bandwidth and prevents colour issues on unsupported displays.",
            Tags = ["night-light", "hdr", "streaming", "video"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings", "EnableHDRForStreamingVideo", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings", "EnableHDRForStreamingVideo", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings", "EnableHDRForStreamingVideo", 0),
            ],
        },
        new TweakDef
        {
            Id = "night-per-process-gpu",
            Label = "Enable Per-Process GPU Selection",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables DirectX swap chain upgrade for better GPU selection per application. May improve hybrid GPU laptops.",
            Tags = ["night-light", "gpu", "directx", "per-process"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\DirectX\UserGpuPreferences"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_CURRENT_USER\Software\Microsoft\DirectX\UserGpuPreferences",
                    "DirectXUserGlobalSettings",
                    "SwapEffectUpgradeEnable=1;"
                ),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\DirectX\UserGpuPreferences", "DirectXUserGlobalSettings")],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_CURRENT_USER\Software\Microsoft\DirectX\UserGpuPreferences",
                    "DirectXUserGlobalSettings",
                    "SwapEffectUpgradeEnable=1;"
                ),
            ],
        },
        new TweakDef
        {
            Id = "night-disable-display-gp",
            Label = "Lock Display Settings (Policy)",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents users from changing display settings via Group Policy. Useful for kiosk/shared machines.",
            Tags = ["night-light", "display", "policy", "lock"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Display"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Display", "DisableDisplaySettings", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Display", "DisableDisplaySettings")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Display", "DisableDisplaySettings", 1)],
        },
        new TweakDef
        {
            Id = "night-srgb-default",
            Label = "Set Default Colour Profile to sRGB",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets the default colour profile to standard sRGB IEC61966-2.1. Ensures consistent colour across applications.",
            Tags = ["night-light", "colour", "srgb", "profile", "calibration"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\ICM"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\ICM", "ICMProfile", "sRGB IEC61966-2.1")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\ICM", "ICMProfile")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\ICM", "ICMProfile", "sRGB IEC61966-2.1")],
        },
        new TweakDef
        {
            Id = "night-disable-dwm-hdr",
            Label = "Disable DWM HDR Compositor (Policy)",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Desktop Window Manager HDR compositor via policy. Force SDR mode even on HDR displays.",
            Tags = ["night-light", "hdr", "dwm", "compositor", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DWM"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DWM", "DisableHDR", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DWM", "DisableHDR")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DWM", "DisableHDR", 1)],
        },
        new TweakDef
        {
            Id = "night-enable-vivid-colour",
            Label = "Enable Vivid Display Colour Mode",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets the video colour profile to Vivid mode (UseHDR=2). Increases saturation for SDR content on HDR displays. Default: Off.",
            Tags = ["night-light", "hdr", "vivid", "colour", "display"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings", "UseHDR", 2)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings", "UseHDR", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings", "UseHDR", 2)],
        },
        new TweakDef
        {
            Id = "night-disable-icc-auto",
            Label = "Disable Auto ICC Colour Profile",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables automatic ICC colour profile activation by Windows Colour Management. Useful when custom calibration profiles cause unintended colour shifts. Default: Enabled.",
            Tags = ["night-light", "icc", "colour", "calibration", "display"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\ICM"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\ICM", "ICMSystemActivationEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\ICM", "ICMSystemActivationEnabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\ICM", "ICMSystemActivationEnabled", 0)],
        },
        new TweakDef
        {
            Id = "night-disable-color-filters",
            Label = "Disable Color Filters",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the Windows color filter overlay (grayscale, inverted, etc.). Default: disabled.",
            Tags = ["nightlight", "color-filter", "accessibility", "display"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\ColorFiltering"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\ColorFiltering", "Active", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\ColorFiltering", "Active")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\ColorFiltering", "Active", 0)],
        },
        new TweakDef
        {
            Id = "night-disable-dynamic-refresh",
            Label = "Disable Dynamic Refresh Rate",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables dynamic refresh rate switching. Display stays at a fixed refresh rate instead of auto-adjusting. Avoids flickering issues. Default: dynamic.",
            Tags = ["display", "refresh-rate", "dynamic", "disable"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\DirectX\UserGpuPreferences"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\DirectX\UserGpuPreferences", "DirectXUserGlobalSettings", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\DirectX\UserGpuPreferences", "DirectXUserGlobalSettings")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\DirectX\UserGpuPreferences", "DirectXUserGlobalSettings", 0)],
        },
        new TweakDef
        {
            Id = "night-disable-schedule",
            Label = "Disable Night Light Schedule",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the automatic Night Light schedule. Night Light must be toggled manually. Default: scheduled based on sunset/sunrise.",
            Tags = ["night-light", "schedule", "disable", "manual"],
            RegistryKeys =
            [
                @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CloudStore\Store\DefaultAccount\Current\default$windows.data.bluelightreduction.settings",
            ],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CloudStore\Store\DefaultAccount\Current\default$windows.data.bluelightreduction.settings\windows.data.bluelightreduction.settings",
                    "ScheduleEnabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CloudStore\Store\DefaultAccount\Current\default$windows.data.bluelightreduction.settings\windows.data.bluelightreduction.settings",
                    "ScheduleEnabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CloudStore\Store\DefaultAccount\Current\default$windows.data.bluelightreduction.settings\windows.data.bluelightreduction.settings",
                    "ScheduleEnabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "night-enable-night-light",
            Label = "Enable Night Light",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables the Windows Night Light blue light filter. Reduces blue light emission to reduce eye strain. Default: disabled.",
            Tags = ["night-light", "blue-light", "enable", "display"],
            RegistryKeys =
            [
                @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CloudStore\Store\DefaultAccount\Current\default$windows.data.bluelightreduction.bluelightreductionstate",
            ],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CloudStore\Store\DefaultAccount\Current\default$windows.data.bluelightreduction.bluelightreductionstate\windows.data.bluelightreduction.bluelightreductionstate",
                    "Enabled",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CloudStore\Store\DefaultAccount\Current\default$windows.data.bluelightreduction.bluelightreductionstate\windows.data.bluelightreduction.bluelightreductionstate",
                    "Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CloudStore\Store\DefaultAccount\Current\default$windows.data.bluelightreduction.bluelightreductionstate\windows.data.bluelightreduction.bluelightreductionstate",
                    "Enabled",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "night-keep-hdr-battery",
            Label = "Keep HDR On Battery",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Keeps HDR enabled when running on battery power. Prevents auto-disabling HDR to save power. Default: HDR disabled on battery.",
            Tags = ["display", "hdr", "battery", "power"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\AutoRotation"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\AutoRotation", "Enable", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\AutoRotation", "Enable")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\AutoRotation", "Enable", 0)],
        },
    ];
}

// ── merged from Fonts.cs ────────────────────────────────────────
internal static class Fonts
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "font-enable-cleartype",
            Label = "Enable ClearType Font Rendering",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables ClearType sub-pixel rendering for sharper text on LCD displays (sets FontSmoothingType to 2).",
            Tags = ["fonts", "cleartype", "rendering", "display"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothingType", "2")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothingType", "1")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothingType", "2")],
        },
        new TweakDef
        {
            Id = "font-disable-antialiasing",
            Label = "Disable Font Antialiasing (Performance)",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Turns off all font smoothing and antialiasing for a minor performance gain — text will appear jagged on LCD displays.",
            Tags = ["fonts", "antialiasing", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothingType", "0"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothing", "0"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothingType", "2"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothing", "2"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothingType", "0")],
        },
        new TweakDef
        {
            Id = "font-set-segoe-ui",
            Label = "Set Default System Font to Segoe UI",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Registers Segoe UI and its variants as the per-user default font, overriding any previous user-level font substitution.",
            Tags = ["fonts", "segoe", "default", "system"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Fonts"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Fonts", "Segoe UI (TrueType)", "segoeui.ttf"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Fonts", "Segoe UI Bold (TrueType)", "segoeuib.ttf"),
                RegOp.SetString(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Fonts",
                    "Segoe UI Italic (TrueType)",
                    "segoeuii.ttf"
                ),
                RegOp.SetString(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Fonts",
                    "Segoe UI Bold Italic (TrueType)",
                    "segoeuiz.ttf"
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Fonts", "Segoe UI (TrueType)"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Fonts", "Segoe UI Bold (TrueType)"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Fonts", "Segoe UI Italic (TrueType)"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Fonts", "Segoe UI Bold Italic (TrueType)"),
            ],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Fonts", "Segoe UI (TrueType)", "segoeui.ttf"),
            ],
        },
        new TweakDef
        {
            Id = "font-disable-fontcache-service",
            Label = "Disable Font Cache Service",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Windows Font Cache Service (FontCache). May reduce memory usage but can slow down font loading.",
            Tags = ["fonts", "cache", "service", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\FontCache"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\FontCache", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\FontCache", "Start", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\FontCache", "Start", 4)],
        },
        new TweakDef
        {
            Id = "font-disable-fontcache3-service",
            Label = "Disable Font Cache 3.0 Service",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Windows Presentation Foundation Font Cache 3.0 Service used by WPF applications.",
            Tags = ["fonts", "cache", "wpf", "service"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\FontCache3.0.0.0"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\FontCache", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\FontCache", "Start", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\FontCache", "Start", 4)],
        },
        new TweakDef
        {
            Id = "font-cleartype-tuning",
            Label = "Set ClearType Tuning to Maximum",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets the ClearType rendering level to 100 (maximum) for WPF and Avalon-based applications on the primary display.",
            Tags = ["fonts", "cleartype", "tuning", "wpf", "rendering"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Avalon.Graphics\DISPLAY1"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Avalon.Graphics\DISPLAY1", "ClearTypeLevel", 100)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Avalon.Graphics\DISPLAY1", "ClearTypeLevel")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Avalon.Graphics\DISPLAY1", "ClearTypeLevel", 100)],
        },
        new TweakDef
        {
            Id = "font-natural-cleartype-contrast",
            Label = "Enable Natural ClearType Contrast",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets the WPF text contrast level to 1 for a more natural, softer ClearType appearance on the primary display.",
            Tags = ["fonts", "cleartype", "contrast", "wpf", "rendering"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Avalon.Graphics\DISPLAY1"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Avalon.Graphics\DISPLAY1", "TextContrastLevel", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Avalon.Graphics\DISPLAY1", "TextContrastLevel")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Avalon.Graphics\DISPLAY1", "TextContrastLevel", 1)],
        },
        new TweakDef
        {
            Id = "font-wpf-hw-text-rendering",
            Label = "Enable WPF Hardware Text Rendering",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Ensures WPF applications use GPU-accelerated text rendering by explicitly setting DisableHWAcceleration to 0.",
            Tags = ["fonts", "wpf", "gpu", "hardware", "rendering"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Avalon.Graphics"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Avalon.Graphics", "DisableHWAcceleration", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Avalon.Graphics", "DisableHWAcceleration")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Avalon.Graphics", "DisableHWAcceleration", 0)],
        },
        new TweakDef
        {
            Id = "font-fonts-disable-font-fallback",
            Label = "Disable Font Fallback",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Overrides MS Shell Dlg font fallback to Segoe UI for consistent rendering across legacy and modern applications. Default: Microsoft Sans Serif. Recommended: Segoe UI.",
            Tags = ["fonts", "fallback", "substitutes", "rendering"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\FontSubstitutes"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\FontSubstitutes", "MS Shell Dlg", "Segoe UI"),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\FontSubstitutes", "MS Shell Dlg 2", "Segoe UI"),
            ],
            RemoveOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\FontSubstitutes",
                    "MS Shell Dlg",
                    "Microsoft Sans Serif"
                ),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\FontSubstitutes", "MS Shell Dlg 2", "Tahoma"),
            ],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\FontSubstitutes", "MS Shell Dlg", "Segoe UI"),
            ],
        },
        new TweakDef
        {
            Id = "font-fonts-disable-font-antialiasing",
            Label = "Disable Font Anti-Aliasing",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables font smoothing/anti-aliasing for sharper pixel-aligned text. May improve readability on low-DPI screens. Default: 2 (enabled). Recommended: Disabled for CRT/low-DPI.",
            Tags = ["fonts", "antialiasing", "smoothing", "rendering"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothing", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothing", "2")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothing", "0")],
        },
        new TweakDef
        {
            Id = "font-set-smoothing-orientation",
            Label = "Set Font Smoothing Orientation to RGB",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets subpixel font smoothing orientation to RGB for standard LCD panels. Improves ClearType rendering on horizontal RGB displays. Default: 0 (auto). Recommended: 1 (RGB) for most monitors.",
            Tags = ["fonts", "cleartype", "subpixel", "orientation", "rendering"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothingOrientation", "1")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothingOrientation", "0")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothingOrientation", "1")],
        },
        new TweakDef
        {
            Id = "font-set-cleartype-contrast",
            Label = "Set ClearType High Contrast",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets ClearType gamma to 1000 for higher contrast text rendering. Makes text appear bolder and easier to read on most displays. Default: 1400. Recommended: 1000 for high-DPI screens.",
            Tags = ["fonts", "cleartype", "contrast", "gamma", "rendering"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothingGamma", 1000)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothingGamma", 1400)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothingGamma", 1000)],
        },
        new TweakDef
        {
            Id = "font-increase-glyph-cache",
            Label = "Increase GDI Glyph Cache Size",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Increases the GDI font glyph cache from default 2 MB to 4 MB. Reduces glyph re-rasterization in multi-font or CJK workloads. Default: 2097152 (~2 MB). Recommended: 4194304 (4 MB).",
            Tags = ["fonts", "glyph-cache", "gdi", "performance", "rendering"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\GRE_Initialize"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\GRE_Initialize", "GlyphCacheSize", 4194304)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\GRE_Initialize", "GlyphCacheSize")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\GRE_Initialize", "GlyphCacheSize", 4194304),
            ],
        },
        new TweakDef
        {
            Id = "font-disable-font-streaming",
            Label = "Disable Font Streaming",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows font streaming (downloading fonts on demand from Microsoft). Reduces network calls. Default: enabled.",
            Tags = ["fonts", "streaming", "download", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableFontProviders", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableFontProviders")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableFontProviders", 0)],
        },
        new TweakDef
        {
            Id = "font-disable-font-smoothing",
            Label = "Disable ClearType Font Smoothing",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables ClearType font smoothing. Can improve sharpness on some displays. Default: enabled.",
            Tags = ["fonts", "cleartype", "smoothing", "rendering"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothing", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothing", "2")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothing", "0")],
        },
        new TweakDef
        {
            Id = "font-block-untrusted-fonts",
            Label = "Block Untrusted Fonts",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Blocks untrusted fonts from loading in processes. Mitigates font parsing vulnerabilities. Default: off.",
            Tags = ["fonts", "security", "untrusted", "mitigation"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Kernel"],
            ApplyOps =
            [
                RegOp.SetQword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Kernel", "MitigationOptions", 0x1000000000000),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Kernel", "MitigationOptions")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Kernel", "MitigationOptions", 1)],
        },
        new TweakDef
        {
            Id = "font-set-default-console-font",
            Label = "Set Cascadia Mono as Default Console Font",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets Cascadia Mono as the default console/terminal font. Requires the font to be installed. Default: Consolas.",
            Tags = ["fonts", "console", "terminal", "cascadia"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Console", "FaceName", "Cascadia Mono")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Console", "FaceName", "Consolas")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Console", "FaceName", "Cascadia Mono")],
        },
        new TweakDef
        {
            Id = "font-disable-font-installation-user",
            Label = "Disable Per-User Font Installation",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents users from installing fonts per-user. Requires admin font installation. Default: allowed.",
            Tags = ["fonts", "installation", "user", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "BlockNonAdminUserInstall", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "BlockNonAdminUserInstall")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "BlockNonAdminUserInstall", 1)],
        },
        new TweakDef
        {
            Id = "font-block-ie-zone-download",
            Label = "Block Font Downloads in IE Zones",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks font downloads via Internet Explorer security zones. Prevents malicious font exploitation via web. Default: allowed.",
            Tags = ["fonts", "ie", "download", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\Internet Settings\Zones\3"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\Internet Settings\Zones\3", "1604", 3),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\Internet Settings\Zones\3", "1604"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\Internet Settings\Zones\3", "1604", 3),
            ],
        },
        new TweakDef
        {
            Id = "font-block-untrusted",
            Label = "Block Untrusted Fonts",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks loading of untrusted fonts to mitigate font parsing vulnerabilities. Only system-installed fonts are rendered. Default: allowed.",
            Tags = ["fonts", "untrusted", "security", "block"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Kernel"],
            ApplyOps =
            [
                RegOp.SetQword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Kernel", "MitigationOptions", 0x1000000000000),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Kernel", "MitigationOptions")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\MitigationOptions", "MitigationOptions_FontBocking", 1),
            ],
        },
        new TweakDef
        {
            Id = "font-disable-download-edge",
            Label = "Disable Font Download in Edge",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables web font downloading in Edge browser. Prevents remote font rendering. Pages may render with system fonts. Default: enabled.",
            Tags = ["fonts", "edge", "download", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "DefaultWebFontsSetting", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "DefaultWebFontsSetting")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "DefaultWebFontsSetting", 2)],
        },
        new TweakDef
        {
            Id = "font-enable-smoothing",
            Label = "Enable ClearType Font Smoothing",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables ClearType sub-pixel font smoothing. Improves text readability on LCD displays. Default: enabled on most systems.",
            Tags = ["fonts", "cleartype", "smoothing", "display"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothing", "2")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothing", "0")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothing", "2")],
        },
        new TweakDef
        {
            Id = "font-fonts-cleartype-performance",
            Label = "Optimize ClearType for Performance",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets ClearType tuning to optimize for rendering performance over maximum quality. Reduces font rendering overhead. Default: quality-optimized.",
            Tags = ["fonts", "cleartype", "performance", "tuning"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothingType", "1")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothingType", "2")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothingType", "1")],
        },
        new TweakDef
        {
            Id = "font-fonts-disable-streaming",
            Label = "Disable Font Streaming",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Windows font streaming (cloud font download). Prevents background font fetching from Microsoft servers. Default: enabled.",
            Tags = ["fonts", "streaming", "cloud", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableFontProviders", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableFontProviders")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableFontProviders", 0)],
        },
        // ── Sprint 19 additions ────────────────────────────────────────────
        new TweakDef
        {
            Id = "font-set-dpi-aware-font-scaling",
            Label = "Set DPI-Aware Font Scaling",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables per-monitor DPI-aware font scaling for sharper text on high-DPI displays. Default: system-level scaling.",
            Tags = ["fonts", "dpi", "scaling", "high-dpi"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "Win8DpiScaling", "1")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "Win8DpiScaling", "0")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "Win8DpiScaling", "1")],
        },
        new TweakDef
        {
            Id = "font-disable-font-substitution-policy",
            Label = "Disable Font Substitution",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables automatic font substitution for missing fonts. Applications will use their fallback fonts instead. Default: enabled.",
            Tags = ["fonts", "substitution", "fallback", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\FontSubstitutes"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\FontSubstitutes", "MS Shell Dlg", "Segoe UI"),
            ],
            RemoveOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\FontSubstitutes",
                    "MS Shell Dlg",
                    "Microsoft Sans Serif"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\FontSubstitutes", "MS Shell Dlg", "Segoe UI"),
            ],
        },
        new TweakDef
        {
            Id = "font-set-icon-title-font-cascadia",
            Label = "Set Icon Title Font to Cascadia Mono",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Changes the icon title font to Cascadia Mono for a modern developer-friendly look. Default: Segoe UI.",
            Tags = ["fonts", "icon", "cascadia", "developer"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "IconFont", "Cascadia Mono")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "IconFont", "Segoe UI")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "IconFont", "Cascadia Mono")],
        },
        new TweakDef
        {
            Id = "font-force-truetype-rendering",
            Label = "Force TrueType Font Rendering",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Forces TrueType rendering mode for all fonts, preventing bitmap font fallback. Default: auto-select.",
            Tags = ["fonts", "truetype", "rendering", "quality"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothing", "2")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothing", "0")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothing", "2")],
        },
        new TweakDef
        {
            Id = "font-disable-font-hinting",
            Label = "Disable Font Hinting",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables font hinting for smoother-looking text on high-DPI displays. May reduce sharpness on low-DPI. Default: enabled.",
            Tags = ["fonts", "hinting", "smoothing", "high-dpi"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothingOrientation", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothingOrientation", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothingOrientation", 0)],
        },
        new TweakDef
        {
            Id = "font-set-system-font-size-default",
            Label = "Reset System Font Size to Default",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Resets the system-wide font size to the Windows default (96 DPI = 100%). Overrides any custom scaling.",
            Tags = ["fonts", "size", "default", "reset"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "LogPixels", 96)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "LogPixels")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "LogPixels", 96)],
        },
        new TweakDef
        {
            Id = "font-enable-directwrite",
            Label = "Force DirectWrite Rendering",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Forces DirectWrite text rendering for improved subpixel anti-aliasing and colour accuracy. Default: auto.",
            Tags = ["fonts", "directwrite", "rendering", "subpixel"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\DirectWrite"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\DirectWrite", "GammaLevel", 2200)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\DirectWrite", "GammaLevel")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\DirectWrite", "GammaLevel", 2200)],
        },
        new TweakDef
        {
            Id = "font-disable-font-providers",
            Label = "Disable Cloud Font Providers",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Blocks Windows from contacting cloud font providers, preventing font downloads over the network. Default: enabled.",
            Tags = ["fonts", "cloud", "providers", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "AllowFontProviders", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "AllowFontProviders")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "AllowFontProviders", 0)],
        },
        new TweakDef
        {
            Id = "font-set-caption-font-weight",
            Label = "Set Bold Caption Font",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets the window caption (title bar) font weight to bold for improved readability. Default: normal weight.",
            Tags = ["fonts", "caption", "bold", "titlebar"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "CaptionWidth", "-270")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "CaptionWidth", "-225")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "CaptionWidth", "-270")],
        },
        new TweakDef
        {
            Id = "font-set-message-font-default",
            Label = "Reset Message Box Font",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Resets the message box font to the default Segoe UI 9pt. Fixes applications displaying incorrect dialog fonts.",
            Tags = ["fonts", "message", "dialog", "reset"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "MessageFont", "Segoe UI")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "MessageFont")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "MessageFont", "Segoe UI")],
        },
    ];
}

// ── merged from DesktopCustomization.cs ──
/// <summary>
/// Desktop shell customization tweaks — Explorer behaviour, Quick Access,
/// ribbon, status bar, folder view, Recent/Frequent, notifications, and
/// other desktop-level UX tweaks.
/// Sprint 25 — Phase 5 roadmap items.
/// </summary>
internal static class DesktopCustomization
{
    private const string Explorer = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer";
    private const string Advanced = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced";
    private const string CabinetState = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\CabinetState";
    private const string Policies = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer";
    private const string Ribbon = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Ribbon";
    private const string ContentDelivery = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\ContentDeliveryManager";
    private const string Search = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Search";
    private const string Feeds = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Feeds";
    private const string PenWorkspace = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\PenWorkspace";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        // ── Explorer View Options ────────────────────────────────────────

        new TweakDef
        {
            Id = "dtcust-show-hidden-files",
            Label = "Show Hidden Files and Folders",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Makes hidden files and folders visible in File Explorer.",
            Tags = ["desktop", "explorer", "hidden", "files"],
            RegistryKeys = [Advanced],
            ApplyOps = [RegOp.SetDword(Advanced, "Hidden", 1)],
            RemoveOps = [RegOp.SetDword(Advanced, "Hidden", 2)],
            DetectOps = [RegOp.CheckDword(Advanced, "Hidden", 1)],
        },
        new TweakDef
        {
            Id = "dtcust-show-super-hidden",
            Label = "Show Protected OS Files",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Shows protected operating system files in Explorer (e.g., desktop.ini, thumbs.db).",
            Tags = ["desktop", "explorer", "protected", "system"],
            RegistryKeys = [Advanced],
            ApplyOps = [RegOp.SetDword(Advanced, "ShowSuperHidden", 1)],
            RemoveOps = [RegOp.SetDword(Advanced, "ShowSuperHidden", 0)],
            DetectOps = [RegOp.CheckDword(Advanced, "ShowSuperHidden", 1)],
        },
        new TweakDef
        {
            Id = "dtcust-show-full-path-title",
            Label = "Show Full Path in Explorer Title Bar",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Displays the complete folder path in the Explorer title bar instead of just the folder name.",
            Tags = ["desktop", "explorer", "path", "titlebar"],
            RegistryKeys = [CabinetState],
            ApplyOps = [RegOp.SetDword(CabinetState, "FullPath", 1)],
            RemoveOps = [RegOp.SetDword(CabinetState, "FullPath", 0)],
            DetectOps = [RegOp.CheckDword(CabinetState, "FullPath", 1)],
        },
        new TweakDef
        {
            Id = "dtcust-show-full-path-address",
            Label = "Show Full Path in Explorer Address Bar",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Uses the full file system path in Explorer's address bar instead of breadcrumb navigation.",
            Tags = ["desktop", "explorer", "path", "address"],
            RegistryKeys = [CabinetState],
            ApplyOps = [RegOp.SetDword(CabinetState, "FullPathAddress", 1)],
            RemoveOps = [RegOp.SetDword(CabinetState, "FullPathAddress", 0)],
            DetectOps = [RegOp.CheckDword(CabinetState, "FullPathAddress", 1)],
        },
        new TweakDef
        {
            Id = "dtcust-show-status-bar",
            Label = "Show Explorer Status Bar",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Always displays the status bar at the bottom of Explorer windows showing item info.",
            Tags = ["desktop", "explorer", "statusbar"],
            RegistryKeys = [Advanced],
            ApplyOps = [RegOp.SetDword(Advanced, "ShowStatusBar", 1)],
            RemoveOps = [RegOp.SetDword(Advanced, "ShowStatusBar", 0)],
            DetectOps = [RegOp.CheckDword(Advanced, "ShowStatusBar", 1)],
        },
        new TweakDef
        {
            Id = "dtcust-launch-to-this-pc",
            Label = "Open Explorer to 'This PC' (Not Quick Access)",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Opens File Explorer to This PC view instead of Quick Access on launch.",
            Tags = ["desktop", "explorer", "thispc", "launch"],
            RegistryKeys = [Advanced],
            ApplyOps = [RegOp.SetDword(Advanced, "LaunchTo", 1)],
            RemoveOps = [RegOp.SetDword(Advanced, "LaunchTo", 2)],
            DetectOps = [RegOp.CheckDword(Advanced, "LaunchTo", 1)],
        },
        new TweakDef
        {
            Id = "dtcust-disable-quick-access-recent",
            Label = "Disable Recent Files in Quick Access",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Stops Quick Access from showing recently opened files.",
            Tags = ["desktop", "explorer", "quick-access", "recent"],
            RegistryKeys = [Explorer],
            ApplyOps = [RegOp.SetDword(Explorer, "ShowRecent", 0)],
            RemoveOps = [RegOp.SetDword(Explorer, "ShowRecent", 1)],
            DetectOps = [RegOp.CheckDword(Explorer, "ShowRecent", 0)],
        },
        new TweakDef
        {
            Id = "dtcust-disable-quick-access-frequent",
            Label = "Disable Frequent Folders in Quick Access",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Stops Quick Access from showing frequently accessed folders.",
            Tags = ["desktop", "explorer", "quick-access", "frequent"],
            RegistryKeys = [Explorer],
            ApplyOps = [RegOp.SetDword(Explorer, "ShowFrequent", 0)],
            RemoveOps = [RegOp.SetDword(Explorer, "ShowFrequent", 1)],
            DetectOps = [RegOp.CheckDword(Explorer, "ShowFrequent", 0)],
        },
        new TweakDef
        {
            Id = "dtcust-show-file-extensions",
            Label = "Always Show File Name Extensions",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Displays file extensions (.txt, .exe, .pdf) for all files in Explorer.",
            Tags = ["desktop", "explorer", "extensions", "security"],
            RegistryKeys = [Advanced],
            ApplyOps = [RegOp.SetDword(Advanced, "HideFileExt", 0)],
            RemoveOps = [RegOp.SetDword(Advanced, "HideFileExt", 1)],
            DetectOps = [RegOp.CheckDword(Advanced, "HideFileExt", 0)],
        },
        new TweakDef
        {
            Id = "dtcust-show-merge-conflicts",
            Label = "Show Folder Merge Conflicts",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prompts for merge conflicts when copying folders with overlapping file names.",
            Tags = ["desktop", "explorer", "merge", "conflict"],
            RegistryKeys = [Advanced],
            ApplyOps = [RegOp.SetDword(Advanced, "HideMergeConflicts", 0)],
            RemoveOps = [RegOp.SetDword(Advanced, "HideMergeConflicts", 1)],
            DetectOps = [RegOp.CheckDword(Advanced, "HideMergeConflicts", 0)],
        },
        // ── Compact View & Layout ────────────────────────────────────────

        new TweakDef
        {
            Id = "dtcust-compact-view",
            Label = "Use Compact View in Explorer",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Reduces spacing between items in Explorer for a denser file view (Win11 inflated spacing).",
            Tags = ["desktop", "explorer", "compact", "spacing"],
            RegistryKeys = [Advanced],
            ApplyOps = [RegOp.SetDword(Advanced, "UseCompactMode", 1)],
            RemoveOps = [RegOp.SetDword(Advanced, "UseCompactMode", 0)],
            DetectOps = [RegOp.CheckDword(Advanced, "UseCompactMode", 1)],
        },
        new TweakDef
        {
            Id = "dtcust-checkbox-selection",
            Label = "Use Checkboxes to Select Items",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Adds checkboxes next to files and folders for easy multi-selection.",
            Tags = ["desktop", "explorer", "checkbox", "selection"],
            RegistryKeys = [Advanced],
            ApplyOps = [RegOp.SetDword(Advanced, "AutoCheckSelect", 1)],
            RemoveOps = [RegOp.SetDword(Advanced, "AutoCheckSelect", 0)],
            DetectOps = [RegOp.CheckDword(Advanced, "AutoCheckSelect", 1)],
        },
        new TweakDef
        {
            Id = "dtcust-expand-to-current-folder",
            Label = "Expand Navigation Pane to Current Folder",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Automatically expands the navigation pane to highlight the current folder location.",
            Tags = ["desktop", "explorer", "navigation", "expand"],
            RegistryKeys = [Advanced],
            ApplyOps = [RegOp.SetDword(Advanced, "NavPaneExpandToCurrentFolder", 1)],
            RemoveOps = [RegOp.SetDword(Advanced, "NavPaneExpandToCurrentFolder", 0)],
            DetectOps = [RegOp.CheckDword(Advanced, "NavPaneExpandToCurrentFolder", 1)],
        },
        new TweakDef
        {
            Id = "dtcust-show-all-folders-nav",
            Label = "Show All Folders in Navigation Pane",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Displays all folders (including Control Panel, Recycle Bin) in the navigation pane.",
            Tags = ["desktop", "explorer", "navigation", "folders"],
            RegistryKeys = [Advanced],
            ApplyOps = [RegOp.SetDword(Advanced, "NavPaneShowAllFolders", 1)],
            RemoveOps = [RegOp.SetDword(Advanced, "NavPaneShowAllFolders", 0)],
            DetectOps = [RegOp.CheckDword(Advanced, "NavPaneShowAllFolders", 1)],
        },
        // ── Ribbon & Toolbar ─────────────────────────────────────────────

        new TweakDef
        {
            Id = "dtcust-minimize-ribbon",
            Label = "Minimise Explorer Ribbon by Default",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Collapses the ribbon toolbar in Explorer by default, giving more space to file content.",
            Tags = ["desktop", "explorer", "ribbon", "minimize"],
            RegistryKeys = [Ribbon],
            ApplyOps = [RegOp.SetDword(Ribbon, "MinimizedStateTabletModeOff", 1)],
            RemoveOps = [RegOp.SetDword(Ribbon, "MinimizedStateTabletModeOff", 0)],
            DetectOps = [RegOp.CheckDword(Ribbon, "MinimizedStateTabletModeOff", 1)],
        },
        // ── Taskbar & System Tray ────────────────────────────────────────

        new TweakDef
        {
            Id = "dtcust-show-seconds-clock",
            Label = "Show Seconds in Taskbar Clock",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Displays seconds (HH:MM:SS) in the taskbar system clock.",
            Tags = ["desktop", "taskbar", "clock", "seconds"],
            RegistryKeys = [Advanced],
            ApplyOps = [RegOp.SetDword(Advanced, "ShowSecondsInSystemClock", 1)],
            RemoveOps = [RegOp.SetDword(Advanced, "ShowSecondsInSystemClock", 0)],
            DetectOps = [RegOp.CheckDword(Advanced, "ShowSecondsInSystemClock", 1)],
        },
        new TweakDef
        {
            Id = "dtcust-small-taskbar-icons",
            Label = "Use Small Taskbar Icons",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Uses smaller icons on the taskbar, reducing its height (Win10 style, may not work on Win11).",
            Tags = ["desktop", "taskbar", "icons", "small"],
            RegistryKeys = [Advanced],
            ApplyOps = [RegOp.SetDword(Advanced, "TaskbarSmallIcons", 1)],
            RemoveOps = [RegOp.SetDword(Advanced, "TaskbarSmallIcons", 0)],
            DetectOps = [RegOp.CheckDword(Advanced, "TaskbarSmallIcons", 1)],
        },
        new TweakDef
        {
            Id = "dtcust-hide-taskbar-search",
            Label = "Hide Taskbar Search Box",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Completely hides the search box/icon from the taskbar.",
            Tags = ["desktop", "taskbar", "search", "hide"],
            RegistryKeys = [Search],
            ApplyOps = [RegOp.SetDword(Search, "SearchboxTaskbarMode", 0)],
            RemoveOps = [RegOp.SetDword(Search, "SearchboxTaskbarMode", 2)],
            DetectOps = [RegOp.CheckDword(Search, "SearchboxTaskbarMode", 0)],
        },
        new TweakDef
        {
            Id = "dtcust-search-icon-only",
            Label = "Taskbar Search: Icon Only",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Shows just the search icon (magnifying glass) instead of the full search box.",
            Tags = ["desktop", "taskbar", "search", "icon"],
            RegistryKeys = [Search],
            ApplyOps = [RegOp.SetDword(Search, "SearchboxTaskbarMode", 1)],
            RemoveOps = [RegOp.SetDword(Search, "SearchboxTaskbarMode", 2)],
            DetectOps = [RegOp.CheckDword(Search, "SearchboxTaskbarMode", 1)],
        },
        new TweakDef
        {
            Id = "dtcust-hide-task-view-button",
            Label = "Hide Task View Button",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes the Task View (virtual desktops) button from the taskbar.",
            Tags = ["desktop", "taskbar", "task-view", "hide"],
            RegistryKeys = [Advanced],
            ApplyOps = [RegOp.SetDword(Advanced, "ShowTaskViewButton", 0)],
            RemoveOps = [RegOp.SetDword(Advanced, "ShowTaskViewButton", 1)],
            DetectOps = [RegOp.CheckDword(Advanced, "ShowTaskViewButton", 0)],
        },
        new TweakDef
        {
            Id = "dtcust-hide-widgets-button",
            Label = "Hide Widgets Button from Taskbar",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes the Widgets button/panel from the taskbar on Windows 11.",
            Tags = ["desktop", "taskbar", "widgets", "hide"],
            RegistryKeys = [Advanced],
            ApplyOps = [RegOp.SetDword(Advanced, "TaskbarDa", 0)],
            RemoveOps = [RegOp.SetDword(Advanced, "TaskbarDa", 1)],
            DetectOps = [RegOp.CheckDword(Advanced, "TaskbarDa", 0)],
        },
        new TweakDef
        {
            Id = "dtcust-hide-chat-button",
            Label = "Hide Chat (Teams) Button from Taskbar",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes the Chat (Microsoft Teams) button from the Windows 11 taskbar.",
            Tags = ["desktop", "taskbar", "chat", "teams"],
            RegistryKeys = [Advanced],
            ApplyOps = [RegOp.SetDword(Advanced, "TaskbarMn", 0)],
            RemoveOps = [RegOp.SetDword(Advanced, "TaskbarMn", 1)],
            DetectOps = [RegOp.CheckDword(Advanced, "TaskbarMn", 0)],
        },
        new TweakDef
        {
            Id = "dtcust-taskbar-never-combine",
            Label = "Never Combine Taskbar Buttons",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Shows separate taskbar buttons for each window instead of grouping by application.",
            Tags = ["desktop", "taskbar", "combine", "buttons"],
            RegistryKeys = [Advanced],
            ApplyOps = [RegOp.SetDword(Advanced, "TaskbarGlomLevel", 2)],
            RemoveOps = [RegOp.SetDword(Advanced, "TaskbarGlomLevel", 0)],
            DetectOps = [RegOp.CheckDword(Advanced, "TaskbarGlomLevel", 2)],
        },
        // ── Start Menu ───────────────────────────────────────────────────

        new TweakDef
        {
            Id = "dtcust-disable-start-suggestions",
            Label = "Disable Start Menu Suggestions",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes app suggestions and ads from the Start menu's recommended section.",
            Tags = ["desktop", "start", "suggestions", "ads"],
            RegistryKeys = [ContentDelivery],
            ApplyOps =
            [
                RegOp.SetDword(ContentDelivery, "SystemPaneSuggestionsEnabled", 0),
                RegOp.SetDword(ContentDelivery, "SubscribedContent-338388Enabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(ContentDelivery, "SystemPaneSuggestionsEnabled"),
                RegOp.DeleteValue(ContentDelivery, "SubscribedContent-338388Enabled"),
            ],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SystemPaneSuggestionsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "dtcust-disable-start-bing-search",
            Label = "Disable Bing Search in Start Menu",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Start menu search from sending queries to Bing — local results only.",
            Tags = ["desktop", "start", "bing", "search", "privacy"],
            RegistryKeys = [Search],
            ApplyOps = [RegOp.SetDword(Search, "BingSearchEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(Search, "BingSearchEnabled")],
            DetectOps = [RegOp.CheckDword(Search, "BingSearchEnabled", 0)],
        },
        // ── Notifications & Action Centre ────────────────────────────────

        new TweakDef
        {
            Id = "dtcust-disable-action-center",
            Label = "Disable Action Centre (Notification Panel)",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Hides the notification/action centre panel from the taskbar.",
            Tags = ["desktop", "action-center", "notifications", "hide"],
            RegistryKeys = [Policies],
            ApplyOps = [RegOp.SetDword(Policies, "DisableNotificationCenter", 1)],
            RemoveOps = [RegOp.DeleteValue(Policies, "DisableNotificationCenter")],
            DetectOps = [RegOp.CheckDword(Policies, "DisableNotificationCenter", 1)],
        },
        new TweakDef
        {
            Id = "dtcust-disable-news-feed",
            Label = "Disable News and Interests Feed",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the news and interests widget/feed from the taskbar.",
            Tags = ["desktop", "news", "feed", "taskbar"],
            RegistryKeys = [Feeds],
            ApplyOps = [RegOp.SetDword(Feeds, "ShellFeedsTaskbarViewMode", 2)],
            RemoveOps = [RegOp.SetDword(Feeds, "ShellFeedsTaskbarViewMode", 0)],
            DetectOps = [RegOp.CheckDword(Feeds, "ShellFeedsTaskbarViewMode", 2)],
        },
        // ── Pen, Touch & Misc ────────────────────────────────────────────

        new TweakDef
        {
            Id = "dtcust-disable-pen-workspace",
            Label = "Disable Windows Ink Workspace",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the Windows Ink Workspace button and functionality from the taskbar.",
            Tags = ["desktop", "ink", "pen", "workspace"],
            RegistryKeys = [PenWorkspace],
            ApplyOps = [RegOp.SetDword(PenWorkspace, "PenWorkspaceButtonDesiredVisibility", 0)],
            RemoveOps = [RegOp.SetDword(PenWorkspace, "PenWorkspaceButtonDesiredVisibility", 1)],
            DetectOps = [RegOp.CheckDword(PenWorkspace, "PenWorkspaceButtonDesiredVisibility", 0)],
        },
        new TweakDef
        {
            Id = "dtcust-disable-suggestions-lockscreen",
            Label = "Disable Lock Screen Suggestions",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes fun facts, tips, and ads from the Windows lock screen.",
            Tags = ["desktop", "lockscreen", "suggestions", "ads"],
            RegistryKeys = [ContentDelivery],
            ApplyOps = [RegOp.SetDword(ContentDelivery, "RotatingLockScreenOverlayEnabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "RotatingLockScreenOverlayEnabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "RotatingLockScreenOverlayEnabled", 0)],
        },
        new TweakDef
        {
            Id = "dtcust-disable-app-suggestions",
            Label = "Disable Suggested Apps (Silently Installed)",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows from silently installing suggested apps like Candy Crush in the Start menu.",
            Tags = ["desktop", "apps", "suggestions", "bloat"],
            RegistryKeys = [ContentDelivery],
            ApplyOps =
            [
                RegOp.SetDword(ContentDelivery, "SilentInstalledAppsEnabled", 0),
                RegOp.SetDword(ContentDelivery, "OemPreInstalledAppsEnabled", 0),
                RegOp.SetDword(ContentDelivery, "PreInstalledAppsEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(ContentDelivery, "SilentInstalledAppsEnabled"),
                RegOp.DeleteValue(ContentDelivery, "OemPreInstalledAppsEnabled"),
                RegOp.DeleteValue(ContentDelivery, "PreInstalledAppsEnabled"),
            ],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SilentInstalledAppsEnabled", 0)],
        },
        // ── File Operations ──────────────────────────────────────────────

        new TweakDef
        {
            Id = "dtcust-always-show-transfer-details",
            Label = "Always Show File Transfer Details",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Automatically expands the details section in file copy/move dialogs.",
            Tags = ["desktop", "explorer", "copy", "transfer"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\OperationStatusManager"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\OperationStatusManager", "EnthusiastMode", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\OperationStatusManager", "EnthusiastMode", 0),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\OperationStatusManager", "EnthusiastMode", 1),
            ],
        },
        new TweakDef
        {
            Id = "dtcust-disable-sharing-wizard",
            Label = "Disable Sharing Wizard",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Uses the classic security tab instead of the simplified sharing wizard for file/folder sharing.",
            Tags = ["desktop", "explorer", "sharing", "wizard"],
            RegistryKeys = [Advanced],
            ApplyOps = [RegOp.SetDword(Advanced, "SharingWizardOn", 0)],
            RemoveOps = [RegOp.SetDword(Advanced, "SharingWizardOn", 1)],
            DetectOps = [RegOp.CheckDword(Advanced, "SharingWizardOn", 0)],
        },
        // ── Recycle Bin & Thumbnails ─────────────────────────────────────

        new TweakDef
        {
            Id = "dtcust-skip-recycle-bin",
            Label = "Skip Recycle Bin (Delete Directly)",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = false,
            Description = "Deletes files directly without sending to Recycle Bin. Use with caution.",
            Tags = ["desktop", "recycle", "bin", "delete"],
            RegistryKeys = [Policies],
            ApplyOps = [RegOp.SetDword(Policies, "NoRecycleFiles", 1)],
            RemoveOps = [RegOp.DeleteValue(Policies, "NoRecycleFiles")],
            DetectOps = [RegOp.CheckDword(Policies, "NoRecycleFiles", 1)],
        },
        new TweakDef
        {
            Id = "dtcust-disable-delete-confirmation",
            Label = "Disable Delete Confirmation Dialog",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sends files to Recycle Bin without the 'Are you sure?' prompt.",
            Tags = ["desktop", "delete", "confirmation", "dialog"],
            RegistryKeys = [Policies],
            ApplyOps = [RegOp.SetDword(Policies, "ConfirmFileDelete", 0)],
            RemoveOps = [RegOp.SetDword(Policies, "ConfirmFileDelete", 1)],
            DetectOps = [RegOp.CheckDword(Policies, "ConfirmFileDelete", 0)],
        },
        new TweakDef
        {
            Id = "dtcust-disable-thumbnail-cache",
            Label = "Disable Thumbnail Cache",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Explorer from creating thumbs.db thumbnail cache files.",
            Tags = ["desktop", "explorer", "thumbnails", "cache"],
            RegistryKeys = [Advanced],
            ApplyOps = [RegOp.SetDword(Advanced, "DisableThumbnailCache", 1)],
            RemoveOps = [RegOp.SetDword(Advanced, "DisableThumbnailCache", 0)],
            DetectOps = [RegOp.CheckDword(Advanced, "DisableThumbnailCache", 1)],
        },
        new TweakDef
        {
            Id = "dtcust-always-show-icons-never-thumbnails",
            Label = "Always Show Icons, Never Thumbnails",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Shows file type icons instead of image/video thumbnails in Explorer for faster browsing.",
            Tags = ["desktop", "explorer", "icons", "thumbnails"],
            RegistryKeys = [Advanced],
            ApplyOps = [RegOp.SetDword(Advanced, "IconsOnly", 1)],
            RemoveOps = [RegOp.SetDword(Advanced, "IconsOnly", 0)],
            DetectOps = [RegOp.CheckDword(Advanced, "IconsOnly", 1)],
        },
    ];
}

// ── Merged from WindowAppearance.cs ──────────────────────────────────────────────────

internal static class WindowAppearance
{
    private const string Metrics = @"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics";
    private const string Desktop = @"HKEY_CURRENT_USER\Control Panel\Desktop";
    private const string Mouse = @"HKEY_CURRENT_USER\Control Panel\Mouse";
    private const string Dwm = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\DWM";
    private const string Explorer = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced";
    private const string Themes = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize";
    private const string Accessibility = @"HKEY_CURRENT_USER\Control Panel\Accessibility\StickyKeys";
    private const string Cursors = @"HKEY_CURRENT_USER\Control Panel\Cursors";
    private const string Colors = @"HKEY_CURRENT_USER\Control Panel\Colors";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        // ── Title Bar & Window Chrome ────────────────────────────────────

        new TweakDef
        {
            Id = "winapp-titlebar-color-active",
            Label = "Show Accent Color on Title Bars",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Colours active title bars and window borders with the system accent colour.",
            Tags = ["appearance", "titlebar", "accent", "color"],
            RegistryKeys = [Dwm],
            ApplyOps = [RegOp.SetDword(Dwm, "ColorPrevalence", 1)],
            RemoveOps = [RegOp.SetDword(Dwm, "ColorPrevalence", 0)],
            DetectOps = [RegOp.CheckDword(Dwm, "ColorPrevalence", 1)],
        },
        new TweakDef
        {
            Id = "winapp-titlebar-color-inactive",
            Label = "Show Accent Color on Inactive Title Bars",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Extends the accent colour to inactive window title bars for a uniform look.",
            Tags = ["appearance", "titlebar", "accent", "inactive"],
            RegistryKeys = [Dwm],
            ApplyOps = [RegOp.SetDword(Dwm, "AccentColorInactive", 1)],
            RemoveOps = [RegOp.DeleteValue(Dwm, "AccentColorInactive")],
            DetectOps = [RegOp.CheckDword(Dwm, "AccentColorInactive", 1)],
        },
        new TweakDef
        {
            Id = "winapp-start-taskbar-accent",
            Label = "Show Accent Color on Start and Taskbar",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Applies the system accent colour to the Start menu and taskbar background.",
            Tags = ["appearance", "accent", "start", "taskbar"],
            RegistryKeys = [Themes],
            ApplyOps = [RegOp.SetDword(Themes, "ColorPrevalence", 1)],
            RemoveOps = [RegOp.SetDword(Themes, "ColorPrevalence", 0)],
            DetectOps = [RegOp.CheckDword(Themes, "ColorPrevalence", 1)],
        },
        new TweakDef
        {
            Id = "winapp-disable-title-bar-flashing",
            Label = "Disable Title Bar Flashing",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents applications from flashing their title bar to attract attention.",
            Tags = ["appearance", "titlebar", "flash", "focus"],
            RegistryKeys = [Desktop],
            ApplyOps = [RegOp.SetDword(Desktop, "ForegroundFlashCount", 0)],
            RemoveOps = [RegOp.SetDword(Desktop, "ForegroundFlashCount", 3)],
            DetectOps = [RegOp.CheckDword(Desktop, "ForegroundFlashCount", 0)],
        },
        new TweakDef
        {
            Id = "winapp-disable-window-shake",
            Label = "Disable Aero Shake (Minimize on Shake)",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents shaking a window title bar from minimising all other windows.",
            Tags = ["appearance", "aero", "shake", "minimize"],
            RegistryKeys = [Explorer],
            ApplyOps = [RegOp.SetDword(Explorer, "DisallowShaking", 1)],
            RemoveOps = [RegOp.DeleteValue(Explorer, "DisallowShaking")],
            DetectOps = [RegOp.CheckDword(Explorer, "DisallowShaking", 1)],
        },
        // ── Scrollbar & Window Metrics ───────────────────────────────────

        new TweakDef
        {
            Id = "winapp-scrollbar-width-thin",
            Label = "Thin Scrollbars (13px)",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets scrollbar width to thin 13 pixels (default is 17). Requires sign-out.",
            Tags = ["appearance", "scrollbar", "width", "thin"],
            RegistryKeys = [Metrics],
            ApplyOps = [RegOp.SetString(Metrics, "ScrollWidth", "-195")],
            RemoveOps = [RegOp.SetString(Metrics, "ScrollWidth", "-255")],
            DetectOps = [RegOp.CheckString(Metrics, "ScrollWidth", "-195")],
            SideEffects = "Requires sign-out to take effect.",
        },
        new TweakDef
        {
            Id = "winapp-scrollbar-height-thin",
            Label = "Thin Scroll Arrows (13px)",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets scrollbar button height to thin 13 pixels (default is 17). Requires sign-out.",
            Tags = ["appearance", "scrollbar", "height", "thin"],
            RegistryKeys = [Metrics],
            ApplyOps = [RegOp.SetString(Metrics, "ScrollHeight", "-195")],
            RemoveOps = [RegOp.SetString(Metrics, "ScrollHeight", "-255")],
            DetectOps = [RegOp.CheckString(Metrics, "ScrollHeight", "-195")],
            SideEffects = "Requires sign-out to take effect.",
        },
        new TweakDef
        {
            Id = "winapp-border-width-thin",
            Label = "Thin Window Borders (1px)",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets window border width to 1 pixel (default is padded). Requires sign-out.",
            Tags = ["appearance", "border", "width", "thin"],
            RegistryKeys = [Metrics],
            ApplyOps = [RegOp.SetString(Metrics, "BorderWidth", "-15")],
            RemoveOps = [RegOp.SetString(Metrics, "BorderWidth", "-15")],
            DetectOps = [RegOp.CheckString(Metrics, "BorderWidth", "-15")],
            SideEffects = "Requires sign-out to take effect.",
        },
        new TweakDef
        {
            Id = "winapp-padded-border-zero",
            Label = "Remove Window Padding Border",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes the extra padded border around windows (-60 = 4px default, 0 = none). Requires sign-out.",
            Tags = ["appearance", "border", "padding"],
            RegistryKeys = [Metrics],
            ApplyOps = [RegOp.SetString(Metrics, "PaddedBorderWidth", "0")],
            RemoveOps = [RegOp.SetString(Metrics, "PaddedBorderWidth", "-60")],
            DetectOps = [RegOp.CheckString(Metrics, "PaddedBorderWidth", "0")],
            SideEffects = "Requires sign-out to take effect.",
        },
        new TweakDef
        {
            Id = "winapp-caption-height-compact",
            Label = "Compact Title Bar Height (20px)",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Reduces the title bar height to 20 pixels for more screen space (default ~22). Requires sign-out.",
            Tags = ["appearance", "titlebar", "height", "compact"],
            RegistryKeys = [Metrics],
            ApplyOps = [RegOp.SetString(Metrics, "CaptionHeight", "-300")],
            RemoveOps = [RegOp.SetString(Metrics, "CaptionHeight", "-330")],
            DetectOps = [RegOp.CheckString(Metrics, "CaptionHeight", "-300")],
            SideEffects = "Requires sign-out to take effect.",
        },
        // ── Icon Spacing ─────────────────────────────────────────────────

        new TweakDef
        {
            Id = "winapp-icon-spacing-h-compact",
            Label = "Compact Horizontal Icon Spacing (60px)",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Reduces horizontal desktop icon spacing to 60 pixels (default 75). Requires sign-out.",
            Tags = ["appearance", "icon", "spacing", "horizontal"],
            RegistryKeys = [Metrics],
            ApplyOps = [RegOp.SetString(Metrics, "IconSpacing", "-900")],
            RemoveOps = [RegOp.SetString(Metrics, "IconSpacing", "-1125")],
            DetectOps = [RegOp.CheckString(Metrics, "IconSpacing", "-900")],
            SideEffects = "Requires sign-out to take effect.",
        },
        new TweakDef
        {
            Id = "winapp-icon-spacing-v-compact",
            Label = "Compact Vertical Icon Spacing (60px)",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Reduces vertical desktop icon spacing to 60 pixels (default 75). Requires sign-out.",
            Tags = ["appearance", "icon", "spacing", "vertical"],
            RegistryKeys = [Metrics],
            ApplyOps = [RegOp.SetString(Metrics, "IconVerticalSpacing", "-900")],
            RemoveOps = [RegOp.SetString(Metrics, "IconVerticalSpacing", "-1125")],
            DetectOps = [RegOp.CheckString(Metrics, "IconVerticalSpacing", "-900")],
            SideEffects = "Requires sign-out to take effect.",
        },
        // ── Menu & Animation ─────────────────────────────────────────────

        new TweakDef
        {
            Id = "winapp-menu-show-delay-fast",
            Label = "Fast Menu Show Delay (100ms)",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Reduces the delay before sub-menus appear to 100ms (default 400ms).",
            Tags = ["appearance", "menu", "delay", "speed"],
            RegistryKeys = [Desktop],
            ApplyOps = [RegOp.SetString(Desktop, "MenuShowDelay", "100")],
            RemoveOps = [RegOp.SetString(Desktop, "MenuShowDelay", "400")],
            DetectOps = [RegOp.CheckString(Desktop, "MenuShowDelay", "100")],
        },
        new TweakDef
        {
            Id = "winapp-menu-show-delay-instant",
            Label = "Instant Menu Show Delay (0ms)",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Eliminates the delay before sub-menus appear (default 400ms).",
            Tags = ["appearance", "menu", "delay", "instant"],
            RegistryKeys = [Desktop],
            ApplyOps = [RegOp.SetString(Desktop, "MenuShowDelay", "0")],
            RemoveOps = [RegOp.SetString(Desktop, "MenuShowDelay", "400")],
            DetectOps = [RegOp.CheckString(Desktop, "MenuShowDelay", "0")],
        },
        new TweakDef
        {
            Id = "winapp-disable-menu-animations",
            Label = "Disable Menu Fade/Slide Animations",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables fade and slide effects on menus for instant display.",
            Tags = ["appearance", "menu", "animation", "fade"],
            RegistryKeys = [Desktop],
            ApplyOps = [RegOp.SetString(Desktop, "UserPreferencesMask", "9012038010000000")],
            RemoveOps = [RegOp.DeleteValue(Desktop, "UserPreferencesMask")],
            DetectOps = [RegOp.CheckString(Desktop, "UserPreferencesMask", "9012038010000000")],
        },
        new TweakDef
        {
            Id = "winapp-disable-window-animation",
            Label = "Disable Window Min/Max Animations",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the animation when minimising or maximising windows.",
            Tags = ["appearance", "window", "animation", "minimize", "maximize"],
            RegistryKeys = [Desktop],
            ApplyOps = [RegOp.SetString(Desktop, "MinAnimate", "0")],
            RemoveOps = [RegOp.SetString(Desktop, "MinAnimate", "1")],
            DetectOps = [RegOp.CheckString(Desktop, "MinAnimate", "0")],
        },
        new TweakDef
        {
            Id = "winapp-disable-cursor-blink",
            Label = "Disable Cursor Blinking",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Stops the text cursor from blinking by setting the rate to -1 (infinite).",
            Tags = ["appearance", "cursor", "blink"],
            RegistryKeys = [Desktop],
            ApplyOps = [RegOp.SetString(Desktop, "CursorBlinkRate", "-1")],
            RemoveOps = [RegOp.SetString(Desktop, "CursorBlinkRate", "530")],
            DetectOps = [RegOp.CheckString(Desktop, "CursorBlinkRate", "-1")],
        },
        // ── Tooltip ──────────────────────────────────────────────────────

        new TweakDef
        {
            Id = "winapp-tooltip-delay-fast",
            Label = "Fast Tooltip Delay (200ms)",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Reduces tooltip delay to 200ms (default 400ms) for faster hover info.",
            Tags = ["appearance", "tooltip", "delay"],
            RegistryKeys = [Mouse],
            ApplyOps = [RegOp.SetString(Mouse, "MouseHoverTime", "200")],
            RemoveOps = [RegOp.SetString(Mouse, "MouseHoverTime", "400")],
            DetectOps = [RegOp.CheckString(Mouse, "MouseHoverTime", "200")],
        },
        new TweakDef
        {
            Id = "winapp-tooltip-delay-instant",
            Label = "Instant Tooltip Delay (0ms)",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Eliminates tooltip delay for instant hover info display.",
            Tags = ["appearance", "tooltip", "delay", "instant"],
            RegistryKeys = [Mouse],
            ApplyOps = [RegOp.SetString(Mouse, "MouseHoverTime", "0")],
            RemoveOps = [RegOp.SetString(Mouse, "MouseHoverTime", "400")],
            DetectOps = [RegOp.CheckString(Mouse, "MouseHoverTime", "0")],
        },
        // ── Alt+Tab & Multitasking Appearance ────────────────────────────

        new TweakDef
        {
            Id = "winapp-alt-tab-classic",
            Label = "Classic Alt+Tab (No Thumbnails)",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Switches Alt+Tab to the classic icon-only style without window thumbnails.",
            Tags = ["appearance", "alt-tab", "classic", "multitasking"],
            RegistryKeys = [Explorer],
            ApplyOps = [RegOp.SetDword(Explorer, "AltTabSettings", 1)],
            RemoveOps = [RegOp.DeleteValue(Explorer, "AltTabSettings")],
            DetectOps = [RegOp.CheckDword(Explorer, "AltTabSettings", 1)],
        },
        new TweakDef
        {
            Id = "winapp-alt-tab-no-edge-tabs",
            Label = "Alt+Tab: Open Windows Only (No Edge Tabs)",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Edge browser tabs from appearing as separate items in Alt+Tab.",
            Tags = ["appearance", "alt-tab", "edge", "tabs"],
            RegistryKeys = [Explorer],
            ApplyOps = [RegOp.SetDword(Explorer, "MultiTaskingAltTabFilter", 3)],
            RemoveOps = [RegOp.DeleteValue(Explorer, "MultiTaskingAltTabFilter")],
            DetectOps = [RegOp.CheckDword(Explorer, "MultiTaskingAltTabFilter", 3)],
        },
        // ── Transparency & DWM Effects ───────────────────────────────────

        new TweakDef
        {
            Id = "winapp-enable-transparency",
            Label = "Enable Transparency Effects",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables transparency effects on Start, taskbar, and action centre.",
            Tags = ["appearance", "transparency", "visual"],
            RegistryKeys = [Themes],
            ApplyOps = [RegOp.SetDword(Themes, "EnableTransparency", 1)],
            RemoveOps = [RegOp.SetDword(Themes, "EnableTransparency", 0)],
            DetectOps = [RegOp.CheckDword(Themes, "EnableTransparency", 1)],
        },
        new TweakDef
        {
            Id = "winapp-disable-transparency",
            Label = "Disable Transparency Effects",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables all transparency effects for a solid look and slight performance gain.",
            Tags = ["appearance", "transparency", "performance"],
            RegistryKeys = [Themes],
            ApplyOps = [RegOp.SetDword(Themes, "EnableTransparency", 0)],
            RemoveOps = [RegOp.SetDword(Themes, "EnableTransparency", 1)],
            DetectOps = [RegOp.CheckDword(Themes, "EnableTransparency", 0)],
        },
        new TweakDef
        {
            Id = "winapp-dwm-enable-blur",
            Label = "Enable DWM Blur Behind",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables the blur-behind effect for DWM-managed windows (Aero Glass look).",
            Tags = ["appearance", "dwm", "blur", "aero"],
            RegistryKeys = [Dwm],
            ApplyOps = [RegOp.SetDword(Dwm, "EnableAeroPeek", 1)],
            RemoveOps = [RegOp.SetDword(Dwm, "EnableAeroPeek", 0)],
            DetectOps = [RegOp.CheckDword(Dwm, "EnableAeroPeek", 1)],
        },
        new TweakDef
        {
            Id = "winapp-dwm-disable-peek",
            Label = "Disable Desktop Peek (Aero Peek)",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents hovering over Show Desktop from making windows transparent.",
            Tags = ["appearance", "dwm", "peek", "desktop"],
            RegistryKeys = [Dwm],
            ApplyOps = [RegOp.SetDword(Dwm, "EnableAeroPeek", 0)],
            RemoveOps = [RegOp.SetDword(Dwm, "EnableAeroPeek", 1)],
            DetectOps = [RegOp.CheckDword(Dwm, "EnableAeroPeek", 0)],
        },
        new TweakDef
        {
            Id = "winapp-dwm-disable-flip3d",
            Label = "Disable Flip3D Effect",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the 3D flip window switching effect (legacy feature, saves GPU resources).",
            Tags = ["appearance", "dwm", "flip3d", "visual"],
            RegistryKeys = [Dwm],
            ApplyOps = [RegOp.SetDword(Dwm, "Composition", 0)],
            RemoveOps = [RegOp.SetDword(Dwm, "Composition", 1)],
            DetectOps = [RegOp.CheckDword(Dwm, "Composition", 0)],
        },
        new TweakDef
        {
            Id = "winapp-enable-round-corners",
            Label = "Enable Rounded Window Corners",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Ensures rounded corners on windows (default on Win11). Set UseWindowFrameStagingBuffer to 1.",
            Tags = ["appearance", "corners", "rounded", "win11"],
            RegistryKeys = [Dwm],
            ApplyOps = [RegOp.SetDword(Dwm, "UseWindowFrameStagingBuffer", 1)],
            RemoveOps = [RegOp.DeleteValue(Dwm, "UseWindowFrameStagingBuffer")],
            DetectOps = [RegOp.CheckDword(Dwm, "UseWindowFrameStagingBuffer", 1)],
        },
        // ── Cursor & Mouse Visual ────────────────────────────────────────

        new TweakDef
        {
            Id = "winapp-cursor-shadow",
            Label = "Enable Cursor Shadow",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Adds a shadow under the mouse cursor for better visibility on light backgrounds.",
            Tags = ["appearance", "cursor", "shadow"],
            RegistryKeys = [Cursors],
            ApplyOps = [RegOp.SetString(Cursors, "CursorShadow", "1")],
            RemoveOps = [RegOp.SetString(Cursors, "CursorShadow", "0")],
            DetectOps = [RegOp.CheckString(Cursors, "CursorShadow", "1")],
        },
        new TweakDef
        {
            Id = "winapp-disable-cursor-shadow",
            Label = "Disable Cursor Shadow",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes the shadow under the mouse cursor for a cleaner look.",
            Tags = ["appearance", "cursor", "shadow"],
            RegistryKeys = [Cursors],
            ApplyOps = [RegOp.SetString(Cursors, "CursorShadow", "0")],
            RemoveOps = [RegOp.SetString(Cursors, "CursorShadow", "1")],
            DetectOps = [RegOp.CheckString(Cursors, "CursorShadow", "0")],
        },
        new TweakDef
        {
            Id = "winapp-cursor-size-large",
            Label = "Large Mouse Cursor (48px)",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Increases the mouse cursor size to large (48px) for better visibility.",
            Tags = ["appearance", "cursor", "size", "large", "accessibility"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Accessibility"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Accessibility", "CursorSize", 3)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Accessibility", "CursorSize", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Accessibility", "CursorSize", 3)],
        },
        // ── Dark/Light Mode Per-Area ─────────────────────────────────────

        new TweakDef
        {
            Id = "winapp-dark-mode-apps",
            Label = "Dark Mode for Apps",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Forces dark mode for modern UWP/WinUI applications only.",
            Tags = ["appearance", "dark", "mode", "apps"],
            RegistryKeys = [Themes],
            ApplyOps = [RegOp.SetDword(Themes, "AppsUseLightTheme", 0)],
            RemoveOps = [RegOp.SetDword(Themes, "AppsUseLightTheme", 1)],
            DetectOps = [RegOp.CheckDword(Themes, "AppsUseLightTheme", 0)],
        },
        new TweakDef
        {
            Id = "winapp-light-mode-apps",
            Label = "Light Mode for Apps",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Forces light mode for modern UWP/WinUI applications only.",
            Tags = ["appearance", "light", "mode", "apps"],
            RegistryKeys = [Themes],
            ApplyOps = [RegOp.SetDword(Themes, "AppsUseLightTheme", 1)],
            RemoveOps = [RegOp.SetDword(Themes, "AppsUseLightTheme", 0)],
            DetectOps = [RegOp.CheckDword(Themes, "AppsUseLightTheme", 1)],
        },
        new TweakDef
        {
            Id = "winapp-dark-mode-system",
            Label = "Dark Mode for System UI",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Forces dark mode for system UI elements (Start, taskbar, action centre).",
            Tags = ["appearance", "dark", "mode", "system"],
            RegistryKeys = [Themes],
            ApplyOps = [RegOp.SetDword(Themes, "SystemUsesLightTheme", 0)],
            RemoveOps = [RegOp.SetDword(Themes, "SystemUsesLightTheme", 1)],
            DetectOps = [RegOp.CheckDword(Themes, "SystemUsesLightTheme", 0)],
        },
        new TweakDef
        {
            Id = "winapp-light-mode-system",
            Label = "Light Mode for System UI",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Forces light mode for system UI elements (Start, taskbar, action centre).",
            Tags = ["appearance", "light", "mode", "system"],
            RegistryKeys = [Themes],
            ApplyOps = [RegOp.SetDword(Themes, "SystemUsesLightTheme", 1)],
            RemoveOps = [RegOp.SetDword(Themes, "SystemUsesLightTheme", 0)],
            DetectOps = [RegOp.CheckDword(Themes, "SystemUsesLightTheme", 1)],
        },
        // ── Font & Text ──────────────────────────────────────────────────

        new TweakDef
        {
            Id = "winapp-menu-font-size-small",
            Label = "Small Menu Font (14px)",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets the system menu font height to 14 pixels for compact menus. Requires sign-out.",
            Tags = ["appearance", "font", "menu", "size", "small"],
            RegistryKeys = [Metrics],
            ApplyOps = [RegOp.SetString(Metrics, "MenuHeight", "-210")],
            RemoveOps = [RegOp.SetString(Metrics, "MenuHeight", "-285")],
            DetectOps = [RegOp.CheckString(Metrics, "MenuHeight", "-210")],
            SideEffects = "Requires sign-out to take effect.",
        },
        new TweakDef
        {
            Id = "winapp-small-caption-font",
            Label = "Small Caption Font for Toolbars",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Reduces the small caption font height used by floating toolbars and palettes. Requires sign-out.",
            Tags = ["appearance", "font", "caption", "toolbar"],
            RegistryKeys = [Metrics],
            ApplyOps = [RegOp.SetString(Metrics, "SmCaptionHeight", "-225")],
            RemoveOps = [RegOp.SetString(Metrics, "SmCaptionHeight", "-330")],
            DetectOps = [RegOp.CheckString(Metrics, "SmCaptionHeight", "-225")],
            SideEffects = "Requires sign-out to take effect.",
        },
        // ── Desktop & Visual Tweaks ──────────────────────────────────────

        new TweakDef
        {
            Id = "winapp-disable-desktop-icons",
            Label = "Hide All Desktop Icons",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Hides all desktop icons for a clean workspace look.",
            Tags = ["appearance", "desktop", "icons", "clean"],
            RegistryKeys = [Explorer],
            ApplyOps = [RegOp.SetDword(Explorer, "HideIcons", 1)],
            RemoveOps = [RegOp.SetDword(Explorer, "HideIcons", 0)],
            DetectOps = [RegOp.CheckDword(Explorer, "HideIcons", 1)],
        },
        new TweakDef
        {
            Id = "winapp-show-file-extensions",
            Label = "Show File Extensions on Desktop",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Shows file extensions for desktop icons, matching Explorer behaviour.",
            Tags = ["appearance", "desktop", "extensions", "files"],
            RegistryKeys = [Explorer],
            ApplyOps = [RegOp.SetDword(Explorer, "HideFileExt", 0)],
            RemoveOps = [RegOp.SetDword(Explorer, "HideFileExt", 1)],
            DetectOps = [RegOp.CheckDword(Explorer, "HideFileExt", 0)],
        },
        new TweakDef
        {
            Id = "winapp-disable-snap-assist-flyout",
            Label = "Disable Snap Assist Flyout",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents the snap layout flyout from appearing when hovering the maximise button.",
            Tags = ["appearance", "snap", "flyout", "maximize"],
            RegistryKeys = [Explorer],
            ApplyOps = [RegOp.SetDword(Explorer, "EnableSnapAssistFlyout", 0)],
            RemoveOps = [RegOp.SetDword(Explorer, "EnableSnapAssistFlyout", 1)],
            DetectOps = [RegOp.CheckDword(Explorer, "EnableSnapAssistFlyout", 0)],
        },
        new TweakDef
        {
            Id = "winapp-taskbar-top-align",
            Label = "Taskbar: Left-Align Icons",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Moves taskbar icons to the left instead of the default centred layout on Windows 11.",
            Tags = ["appearance", "taskbar", "alignment", "left"],
            RegistryKeys = [Explorer],
            ApplyOps = [RegOp.SetDword(Explorer, "TaskbarAl", 0)],
            RemoveOps = [RegOp.SetDword(Explorer, "TaskbarAl", 1)],
            DetectOps = [RegOp.CheckDword(Explorer, "TaskbarAl", 0)],
        },
        new TweakDef
        {
            Id = "winapp-drag-full-windows",
            Label = "Drag Full Windows (Not Outlines)",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Shows the full window content whilst dragging instead of a wire-frame outline.",
            Tags = ["appearance", "drag", "windows", "visual"],
            RegistryKeys = [Desktop],
            ApplyOps = [RegOp.SetString(Desktop, "DragFullWindows", "1")],
            RemoveOps = [RegOp.SetString(Desktop, "DragFullWindows", "0")],
            DetectOps = [RegOp.CheckString(Desktop, "DragFullWindows", "1")],
        },
        new TweakDef
        {
            Id = "winapp-drag-outline-only",
            Label = "Drag Window Outlines Only",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Shows only a wire-frame outline when dragging windows for lower CPU usage.",
            Tags = ["appearance", "drag", "outline", "performance"],
            RegistryKeys = [Desktop],
            ApplyOps = [RegOp.SetString(Desktop, "DragFullWindows", "0")],
            RemoveOps = [RegOp.SetString(Desktop, "DragFullWindows", "1")],
            DetectOps = [RegOp.CheckString(Desktop, "DragFullWindows", "0")],
        },
        // ── Wallpaper & Background ───────────────────────────────────────

        new TweakDef
        {
            Id = "winapp-wallpaper-quality-max",
            Label = "Maximum Wallpaper JPEG Quality",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets wallpaper JPEG compression to maximum quality (100%). Prevents blurry desktop backgrounds.",
            Tags = ["appearance", "wallpaper", "quality", "jpeg"],
            RegistryKeys = [Desktop],
            ApplyOps = [RegOp.SetDword(Desktop, "JPEGImportQuality", 100)],
            RemoveOps = [RegOp.DeleteValue(Desktop, "JPEGImportQuality")],
            DetectOps = [RegOp.CheckDword(Desktop, "JPEGImportQuality", 100)],
        },
        new TweakDef
        {
            Id = "winapp-solid-color-background",
            Label = "Use Solid Color Desktop Background",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Switches the desktop background to a solid colour (no wallpaper) for a minimal look.",
            Tags = ["appearance", "wallpaper", "solid", "minimal"],
            RegistryKeys = [Desktop],
            ApplyOps = [RegOp.SetString(Desktop, "Wallpaper", "")],
            RemoveOps = [RegOp.DeleteValue(Desktop, "Wallpaper")],
            DetectOps = [RegOp.CheckString(Desktop, "Wallpaper", "")],
        },
        new TweakDef
        {
            Id = "winapp-disable-wallpaper-slideshow",
            Label = "Disable Desktop Slideshow",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents the desktop wallpaper from cycling through images at timed intervals.",
            Tags = ["appearance", "wallpaper", "slideshow"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "DesktopSlideShowEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "DesktopSlideShowEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "DesktopSlideShowEnabled", 0)],
        },
        // ── Focus & Activation ───────────────────────────────────────────

        new TweakDef
        {
            Id = "winapp-focus-follows-mouse",
            Label = "Focus Follows Mouse (X-Mouse)",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Activates windows simply by hovering the mouse, without clicking (X11-style focus).",
            Tags = ["appearance", "focus", "mouse", "x-mouse"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Mouse"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "ActiveWindowTracking", "1")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "ActiveWindowTracking", "0")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "ActiveWindowTracking", "1")],
        },
        new TweakDef
        {
            Id = "winapp-auto-raise-on-hover",
            Label = "Auto-Raise Window on Hover",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Automatically brings the hovered window to the front (requires Focus Follows Mouse).",
            Tags = ["appearance", "focus", "hover", "raise"],
            RegistryKeys = [Desktop],
            ApplyOps = [RegOp.SetDword(Desktop, "ActiveWndTrkTimeout", 200)],
            RemoveOps = [RegOp.DeleteValue(Desktop, "ActiveWndTrkTimeout")],
            DetectOps = [RegOp.CheckDword(Desktop, "ActiveWndTrkTimeout", 200)],
        },
        new TweakDef
        {
            Id = "winapp-foreground-lock-timeout",
            Label = "Reduce Foreground Lock Timeout",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Reduces the timeout that prevents apps from stealing focus to 0ms (instant focus switch).",
            Tags = ["appearance", "focus", "foreground", "lock"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ForegroundLockTimeout", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ForegroundLockTimeout", 200000)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ForegroundLockTimeout", 0)],
        },
        // ── Visual Effects Granular ──────────────────────────────────────

        new TweakDef
        {
            Id = "winapp-disable-smooth-scrolling",
            Label = "Disable Smooth Scrolling",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables smooth scrolling animations in listboxes and controls.",
            Tags = ["appearance", "scrolling", "smooth", "performance"],
            RegistryKeys = [Desktop],
            ApplyOps = [RegOp.SetDword(Desktop, "SmoothScroll", 0)],
            RemoveOps = [RegOp.SetDword(Desktop, "SmoothScroll", 1)],
            DetectOps = [RegOp.CheckDword(Desktop, "SmoothScroll", 0)],
        },
        new TweakDef
        {
            Id = "winapp-disable-font-smoothing",
            Label = "Disable Font Smoothing",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables all font smoothing for a crisp pixel-perfect text look on high-DPI displays.",
            Tags = ["appearance", "font", "smoothing"],
            RegistryKeys = [Desktop],
            ApplyOps = [RegOp.SetString(Desktop, "FontSmoothing", "0")],
            RemoveOps = [RegOp.SetString(Desktop, "FontSmoothing", "2")],
            DetectOps = [RegOp.CheckString(Desktop, "FontSmoothing", "0")],
        },
        // ── merged from: Taskbar.cs ──────────────────────────────────────────────────
        new TweakDef
        {
            Id = "tb-taskbar-align-left",
            Label = "Align Taskbar Left (Win11)",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets Windows 11 taskbar alignment to left instead of center. Default: center. Recommended: left.",
            Tags = ["taskbar", "alignment", "win11"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarAl", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarAl", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarAl", 0)],
        },
        new TweakDef
        {
            Id = "tb-taskbar-small-icons",
            Label = "Use Small Taskbar Icons",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Shrinks taskbar icons and reduces taskbar height (Win10). Default: large icons. Recommended: small icons.",
            Tags = ["taskbar", "icons", "size"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarSmallIcons", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarSmallIcons", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarSmallIcons", 1)],
        },
        new TweakDef
        {
            Id = "tb-taskbar-hide-task-view",
            Label = "Hide Task View Button",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Hides the Task View button from the taskbar. You can still use Win+Tab for virtual desktops. Default: enabled. Recommended: disabled.",
            Tags = ["taskbar", "task-view", "declutter"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowTaskViewButton", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowTaskViewButton", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowTaskViewButton", 0)],
        },
        new TweakDef
        {
            Id = "tb-taskbar-hide-widgets",
            Label = "Hide Widgets (Policy)",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables Widgets board and weather widget via HKLM policy. Frees resources used by the Edge WebView2 widget host. Default: enabled. Recommended: disabled.",
            Tags = ["taskbar", "widgets", "policy", "performance"],
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Dsh",
                @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced",
            ],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Dsh", "AllowNewsAndInterests", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarDa", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Dsh", "AllowNewsAndInterests"),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarDa", 1),
            ],
        },
        new TweakDef
        {
            Id = "tb-taskbar-hide-chat",
            Label = "Hide Chat / Teams Icon",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Hides the Microsoft Teams Chat icon from the taskbar. Default: enabled. Recommended: disabled.",
            Tags = ["taskbar", "chat", "teams", "declutter"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarMn", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarMn", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarMn", 0)],
        },
        new TweakDef
        {
            Id = "tb-taskbar-never-combine",
            Label = "Never Combine Taskbar Buttons",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Prevents taskbar from grouping windows of the same app. Each window gets its own button with a visible label. Default: always combine. Recommended: never combine.",
            Tags = ["taskbar", "grouping", "buttons", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarGlomLevel", 2)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarGlomLevel", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarGlomLevel", 2)],
        },
        new TweakDef
        {
            Id = "tb-taskbar-disable-badges",
            Label = "Disable Notification Badges",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables unread message count badges on taskbar app icons. Default: enabled. Recommended: disabled.",
            Tags = ["taskbar", "badges", "notifications", "declutter"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarBadges", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarBadges", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarBadges", 0)],
        },
        new TweakDef
        {
            Id = "tb-taskbar-disable-flashing",
            Label = "Disable Taskbar Button Flashing",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Stops taskbar buttons from flashing to get your attention. Default: enabled. Recommended: disabled.",
            Tags = ["taskbar", "flashing", "focus", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarFlashing", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarFlashing", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarFlashing", 0)],
        },
        new TweakDef
        {
            Id = "tb-taskbar-end-task",
            Label = "Enable End Task in Taskbar",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Adds an End Task option to the taskbar right-click menu for quickly killing unresponsive apps. Default: disabled. Recommended: enabled.",
            Tags = ["taskbar", "end-task", "productivity"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarEndTask", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarEndTask", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarEndTask", 1)],
        },
        new TweakDef
        {
            Id = "tb-taskbar-disable-recent-search",
            Label = "Disable Recent Searches in Taskbar",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables recent search suggestions shown in the taskbar search box. Default: enabled. Recommended: disabled.",
            Tags = ["taskbar", "search", "privacy", "declutter"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "SearchboxTaskbarRecentSearchesEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "SearchboxTaskbarRecentSearchesEnabled", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "SearchboxTaskbarRecentSearchesEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "tb-taskbar-disable-notification-badges",
            Label = "Disable Notification Badge Overlay",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables unread notification badges on taskbar app icons. Default: Enabled. Recommended: Disabled.",
            Tags = ["taskbar", "badges", "notifications"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarBadges", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarBadges", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarBadges", 0)],
        },
        new TweakDef
        {
            Id = "tb-taskbar-disable-people",
            Label = "Disable People Bar on Taskbar",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes the People bar from the taskbar. Default: Enabled. Recommended: Disabled.",
            Tags = ["taskbar", "people", "social", "declutter"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced\People"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced\People", "PeopleBand", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced\People", "PeopleBand", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced\People", "PeopleBand", 0)],
        },
        new TweakDef
        {
            Id = "tb-disable-meet-now",
            Label = "Disable Meet Now Icon",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Hides the Meet Now (Skype) icon from the taskbar notification area. Default: Shown. Recommended: Hidden.",
            Tags = ["taskbar", "meet-now", "skype", "declutter"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "HideSCAMeetNow", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "HideSCAMeetNow")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "HideSCAMeetNow", 1)],
        },
        new TweakDef
        {
            Id = "tb-show-seconds-clock",
            Label = "Show Seconds in System Clock",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Shows seconds in the taskbar system clock for precision timing. Default: Hidden. Recommended: Personal preference.",
            Tags = ["taskbar", "clock", "seconds", "time"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowSecondsInSystemClock", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowSecondsInSystemClock"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowSecondsInSystemClock", 1),
            ],
        },
        new TweakDef
        {
            Id = "tb-disable-animations",
            Label = "Disable Taskbar Animations",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables taskbar button animations for a snappier feel. Default: Enabled. Recommended: Disabled for performance.",
            Tags = ["taskbar", "animations", "performance", "ui"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarAnimations", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarAnimations")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarAnimations", 0)],
        },
        new TweakDef
        {
            Id = "tb-disable-recent-docs",
            Label = "Disable Recent Documents Tracking",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Windows from tracking recently opened documents for taskbar jump lists. Reduces filesystem activity and improves privacy. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["taskbar", "recent-docs", "privacy", "history", "tracking"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackDocs", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackDocs")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackDocs", 0)],
        },
        new TweakDef
        {
            Id = "tb-lock-taskbar",
            Label = "Lock Taskbar Position and Size",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Locks the taskbar to prevent accidental resizing or repositioning. Default: Unlocked. Recommended: Locked for stable work environments.",
            Tags = ["taskbar", "lock", "resize", "position", "ui"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarSizeMove", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarSizeMove")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarSizeMove", 0)],
        },
        new TweakDef
        {
            Id = "tb-show-all-tray-icons",
            Label = "Always Show All System Tray Icons",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the auto-hide feature for system tray icons. All tray icons are always visible without clicking the expand arrow. Default: Auto-hide. Recommended: Show all for quick access.",
            Tags = ["taskbar", "tray", "icons", "notification-area", "ui"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableAutoTray", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableAutoTray")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableAutoTray", 0)],
        },
        new TweakDef
        {
            Id = "tb-set-taskbar-small-icons",
            Label = "Use Small Taskbar Icons",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets the taskbar to use small icons, increasing available space. Only works on Windows 10. Default: large.",
            Tags = ["taskbar", "small", "icons", "appearance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarSmallIcons", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarSmallIcons", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarSmallIcons", 1)],
        },
        new TweakDef
        {
            Id = "tb-disable-taskbar-people",
            Label = "Disable People Button on Taskbar",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Hides the People button from the taskbar. Default: shown.",
            Tags = ["taskbar", "people", "contacts", "hide"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced\People"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced\People", "PeopleBand", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced\People", "PeopleBand", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced\People", "PeopleBand", 0)],
        },
        new TweakDef
        {
            Id = "tb-move-taskbar-left",
            Label = "Align Taskbar Icons to Left (Windows 11)",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Aligns taskbar icons to the left side instead of centered. Windows 11 only. Default: center.",
            Tags = ["taskbar", "alignment", "left", "windows-11"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarAl", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarAl", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarAl", 0)],
        },
        new TweakDef
        {
            Id = "tb-set-button-grouping",
            Label = "Never Group Taskbar Buttons",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents the taskbar from grouping similar windows together. Each window gets its own button. Default: always combine.",
            Tags = ["taskbar", "grouping", "buttons", "combine"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarGlomLevel", 2)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarGlomLevel", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarGlomLevel", 2)],
        },
        new TweakDef
        {
            Id = "tb-show-full-path-title",
            Label = "Show Full Path in Explorer Title Bar",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Shows the full folder path in Explorer window title bars, making it easier to identify windows. Default: folder name only.",
            Tags = ["taskbar", "explorer", "path", "title"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\CabinetState"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\CabinetState", "FullPath", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\CabinetState", "FullPath", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\CabinetState", "FullPath", 1)],
        },
        new TweakDef
        {
            Id = "tb-disable-cortana-taskbar",
            Label = "Disable Cortana in Taskbar",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Cortana from appearing in the taskbar via Group Policy. Default: enabled.",
            Tags = ["taskbar", "cortana", "disable", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowCortana", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowCortana")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowCortana", 0)],
        },
        new TweakDef
        {
            Id = "tb-disable-taskbar-animations",
            Label = "Disable Taskbar Animations",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables taskbar button animations (slide, pulse, flash). Reduces visual distractions. Default: enabled.",
            Tags = ["taskbar", "animation", "disable", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarAnimations", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarAnimations", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarAnimations", 0)],
        },
        new TweakDef
        {
            Id = "tb-taskbar-multi-display-show-all",
            Label = "Show Taskbar on All Displays",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Shows the taskbar on all connected monitors in a multi-display setup. Default: primary only on Win11.",
            Tags = ["taskbar", "multi-display", "monitor", "show"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "MMTaskbarEnabled", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "MMTaskbarEnabled", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "MMTaskbarEnabled", 1)],
        },
        new TweakDef
        {
            Id = "tb-disable-thumbnail-preview",
            Label = "Disable Taskbar Thumbnail Previews",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the thumbnail preview popup when hovering over taskbar buttons. Shows tooltip text instead. Default: enabled.",
            Tags = ["taskbar", "thumbnail", "preview", "hover"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ExtendedUIHoverTime", 30000),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ExtendedUIHoverTime")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ExtendedUIHoverTime", 30000),
            ],
        },
        new TweakDef
        {
            Id = "tb-set-thumbnail-size",
            Label = "Increase Thumbnail Preview Size",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Increases the size of taskbar thumbnail previews from 200 to 350 pixels wide. Default: 200.",
            Tags = ["taskbar", "thumbnail", "size", "preview"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Taskband"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Taskband", "MaxThumbSizePx", 350)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Taskband", "MaxThumbSizePx")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Taskband", "MaxThumbSizePx", 350)],
        },
        new TweakDef
        {
            Id = "tb-hide-clock-from-taskbar",
            Label = "Hide Clock from Taskbar Notification Area",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets HideClock=1 in Explorer policies. Removes the clock and date display from the system tray area of the taskbar, reclaiming tray space.",
            Tags = ["taskbar", "clock", "tray", "hide"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "HideClock", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "HideClock")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "HideClock", 1)],
        },
        new TweakDef
        {
            Id = "tb-set-search-icon-only",
            Label = "Show Search as Icon Only (Not Full Box)",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets SearchboxTaskbarMode=1 in the Search key. Shows a compact search icon on the taskbar instead of the expanded search box (mode 2) or nothing (mode 0), saving taskbar space.",
            Tags = ["taskbar", "search", "icon", "compact"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "SearchboxTaskbarMode", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "SearchboxTaskbarMode", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "SearchboxTaskbarMode", 1)],
        },
        new TweakDef
        {
            Id = "tb-enable-compact-mode",
            Label = "Enable Compact Taskbar Mode (Windows 11)",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            MinBuild = 22000,
            Description =
                "Sets TaskbarDensity=0 in Explorer Advanced. Switches the Windows 11 taskbar to compact density mode with smaller button padding, reclaiming vertical screen space.",
            Tags = ["taskbar", "compact", "density", "win11"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarDensity", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarDensity", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarDensity", 0)],
        },
        new TweakDef
        {
            Id = "tb-disable-overflow-menu",
            Label = "Disable Taskbar Icon Overflow Menu",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets EnableTaskbarOverflow=0 in Explorer Advanced. Removes the overflow chevron (\"^\") that appears when too many pinned icons exist, preventing hidden icons from piling up out of sight.",
            Tags = ["taskbar", "overflow", "icons", "tray"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableTaskbarOverflow", 0)],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableTaskbarOverflow", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableTaskbarOverflow", 0),
            ],
        },
        new TweakDef
        {
            Id = "tb-hide-language-bar",
            Label = "Hide Language Bar from Taskbar",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets ShowStatus=3 in the CTF LangBar key. Hides the language/input method indicator from the system tray. Useful on single-language machines where the bar wastes tray space.",
            Tags = ["taskbar", "language", "bar", "tray"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\CTF\LangBar"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\CTF\LangBar", "ShowStatus", 3)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\CTF\LangBar", "ShowStatus", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\CTF\LangBar", "ShowStatus", 3)],
        },
        new TweakDef
        {
            Id = "tb-hide-recently-added-apps",
            Label = "Hide Recently Added Apps in Start Menu",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets HideRecentlyAddedApps=1 in the Explorer policy. Removes the \"Recently added\" section from the top of the Start Menu's app list, reducing distraction after new software installs.",
            Tags = ["taskbar", "start-menu", "recently-added", "apps"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "HideRecentlyAddedApps", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "HideRecentlyAddedApps")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "HideRecentlyAddedApps", 1)],
        },
        new TweakDef
        {
            Id = "tb-disable-most-used-apps",
            Label = "Remove Most Used Apps from Start Menu",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets NoStartMenuMFUprogramsList=1 in the Explorer policy. Hides the \"Most used\" / frequently-used apps section from the Start Menu, producing a cleaner app list.",
            Tags = ["taskbar", "start-menu", "most-used", "apps"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "NoStartMenuMFUprogramsList", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "NoStartMenuMFUprogramsList")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "NoStartMenuMFUprogramsList", 1)],
        },
        new TweakDef
        {
            Id = "tb-disable-start-suggestions",
            Label = "Disable App Suggestions in Start Menu",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets SystemPaneSuggestionsEnabled=0 in ContentDeliveryManager. Removes Microsoft-promoted app suggestions from appearing in the Start Menu's recommended section.",
            Tags = ["taskbar", "start-menu", "suggestions", "ads"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SystemPaneSuggestionsEnabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SystemPaneSuggestionsEnabled",
                    1
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SystemPaneSuggestionsEnabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "tb-hide-show-desktop-button",
            Label = "Hide the Show Desktop Button (Bottom-Right Corner)",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets TaskbarSd=0 in Explorer Advanced. Removes the tiny \"Show Desktop\" peek button in the bottom-right corner of the taskbar, preventing accidental desktop exposure.",
            Tags = ["taskbar", "show-desktop", "button", "hide"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarSd", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarSd", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarSd", 0)],
        },
        new TweakDef
        {
            Id = "tb-set-multimonitor-local-windows",
            Label = "Show Only Local Windows on Each Monitor's Taskbar",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets MMTaskbarMode=2 in Explorer Advanced. When multi-monitor taskbars are enabled, each monitor's taskbar shows only the windows that belong to apps on that monitor.",
            Tags = ["taskbar", "multimonitor", "windows", "display"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "MMTaskbarMode", 2)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "MMTaskbarMode", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "MMTaskbarMode", 2)],
        },
        new TweakDef
        {
            Id = "tb-disable-pinning-to-taskbar",
            Label = "Disable Pinning Apps to Taskbar (Policy)",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets NoPinningToTaskbar=1 in Explorer policies. Prevents items from being pinned to the taskbar by the user or via setup wizards, locking the taskbar layout.",
            Tags = ["taskbar", "pinning", "lock", "policy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoPinningToTaskbar", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoPinningToTaskbar")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoPinningToTaskbar", 1)],
        },
    ];
}

