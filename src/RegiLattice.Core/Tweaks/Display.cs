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
