namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Display
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "display-disable-dpi-scaling",
            Label = "Disable DPI Scaling Override",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables the Windows 8-style DPI scaling override, forcing the system DPI setting for all applications.",
            Tags = ["display", "dpi", "scaling"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "Win8DpiScaling", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DpiScalingVer"),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "Win8DpiScaling", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "Win8DpiScaling", 1)],
        },
        new TweakDef
        {
            Id = "display-enable-cleartype",
            Label = "Enable ClearType Font Smoothing",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables ClearType sub-pixel font rendering for sharper text on LCD screens.",
            Tags = ["display", "cleartype", "font", "smoothing"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothing", "2"),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothingType", 2),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothing", "0"),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothingType", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothingType", 2)],
        },
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "LogPixels", 96),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "LogPixels"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "LogPixels", 96)],
        },
        new TweakDef
        {
            Id = "display-dark-mode-apps",
            Label = "Dark Mode for Apps",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Switches UWP and modern apps to their dark colour scheme.",
            Tags = ["display", "dark", "theme", "apps"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "AppsUseLightTheme", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "AppsUseLightTheme", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "AppsUseLightTheme", 0)],
        },
        new TweakDef
        {
            Id = "display-dark-mode-system",
            Label = "Dark Mode for System",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Switches the Windows system theme (taskbar, Start menu, Action Center) to dark mode.",
            Tags = ["display", "dark", "theme", "system"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "SystemUsesLightTheme", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "SystemUsesLightTheme", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "SystemUsesLightTheme", 0)],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "EnableTransparency", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "EnableTransparency", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "EnableTransparency", 0)],
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
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "MinAnimate", "0"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "MinAnimate", "1"),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "JPEGImportQuality", 100),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "JPEGImportQuality"),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "ColorPrevalence", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "ColorPrevalence", 0),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EdgeUI", "AllowEdgeSwipe", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EdgeUI", "AllowEdgeSwipe"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EdgeUI", "AllowEdgeSwipe", 0)],
        },
        new TweakDef
        {
            Id = "display-disable-adaptive-brightness",
            Label = "Disable Adaptive Brightness",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Adaptive Brightness sensor service. Prevents automatic screen brightness changes based on ambient light. Default: Enabled (3=Manual). Recommended: Disabled (4).",
            Tags = ["display", "brightness", "adaptive", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SensrSvc"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SensrSvc", "Start", 4),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SensrSvc", "Start", 3),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SensrSvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "display-dpi-override",
            Label = "Set Display Scaling DPI Override",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Forces display scaling to 100% (96 DPI) using the legacy DPI override. Disables DPI virtualization for crisp rendering. Default: System-managed. Recommended: 96 DPI for external monitors.",
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
            Description = "Disables adaptive brightness via ICM display calibration. Prevents automatic brightness adjustments based on content. Default: Enabled. Recommended: Disabled for consistent brightness.",
            Tags = ["display", "brightness", "icm", "calibration"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\ICM\Calibration"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SensrSvc", "Start", 4),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SensrSvc", "Start", 3),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SensrSvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "display-hardware-cursor",
            Label = "Force Hardware Cursor",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Forces hardware cursor rendering and disables smooth scrolling. Reduces input lag and cursor rendering overhead. Default: Smooth scrolling on. Recommended: Hardware cursor for gaming.",
            Tags = ["display", "cursor", "hardware", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "SmoothScroll", "0"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "SmoothScroll", "1"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "SmoothScroll", "0")],
        },
        new TweakDef
        {
            Id = "display-disable-transparency-effect",
            Label = "Disable Window Transparency Effect",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables window transparency and acrylic blur effects. Improves rendering performance on integrated GPUs. Default: Enabled. Recommended: Disabled for performance.",
            Tags = ["display", "transparency", "acrylic", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "EnableTransparency", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "EnableTransparency", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "EnableTransparency", 0)],
        },
        new TweakDef
        {
            Id = "display-disable-animation-effects",
            Label = "Disable Minimize/Maximize Animation",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables minimize and maximize window animations via WindowMetrics. Makes window switching feel instant. Default: Enabled. Recommended: Disabled for responsiveness.",
            Tags = ["display", "animation", "minimize", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics"],
        },
        new TweakDef
        {
            Id = "display-force-gpu-scaling",
            Label = "Force GPU Scaling Mode",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables GPU-based DPI scaling (Win8DpiScaling) for sharper rendering on non-native resolutions. Default: Disabled. Recommended: Enabled for high-DPI displays.",
            Tags = ["display", "gpu", "scaling", "dpi", "rendering"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "Win8DpiScaling", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "Win8DpiScaling", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "Win8DpiScaling", 1)],
        },
        new TweakDef
        {
            Id = "display-disable-auto-color-mgmt",
            Label = "Disable Auto Color Management",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Windows Desktop Window Manager automatic color management for manual ICC profile control. Default: Enabled. Recommended: Disabled for color-critical work.",
            Tags = ["display", "color", "management", "dwm", "icc"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "EnableAutoColorManagement", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "EnableAutoColorManagement"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "EnableAutoColorManagement", 0)],
        },
        new TweakDef
        {
            Id = "display-set-font-smoothing-gamma",
            Label = "Set ClearType Font Smoothing Gamma",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets ClearType font smoothing gamma to 1200 for optimal text contrast on LCD displays. Default: Not set. Recommended: 1200 for standard LCD.",
            Tags = ["display", "cleartype", "font", "gamma", "text"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "CursorShadow", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "CursorShadow", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "CursorShadow", 0)],
        },
        new TweakDef
        {
            Id = "display-reduce-blur-intensity",
            Label = "Reduce DWM Blur Intensity (50%)",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets the DWM blur-behind intensity to 50. Reduces the visual weight of frosted-glass Mica/Acrylic effects. Default: Not set (OS default full blur).",
            Tags = ["display", "dwm", "blur", "aero", "visual"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "BlurIntensity", 50),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "BlurIntensity"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "BlurIntensity", 50)],
        },
        new TweakDef
        {
            Id = "display-disable-screensaver-policy",
            Label = "Disable Screen Saver (Per-User Policy)",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the screen saver through the per-user control panel policy. Prevents screen saver from activating regardless of system setting. Default: Active.",
            Tags = ["display", "screensaver", "policy", "power", "idle"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Control Panel\Desktop"],
        },
        new TweakDef
        {
            Id = "display-force-aero-composition",
            Label = "Force Desktop Composition (Aero)",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Explicitly enables Desktop Window Manager composition (Aero). Ensures DWM is active even on systems where it was manually disabled. Default: Enabled.",
            Tags = ["display", "dwm", "aero", "composition", "visual"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "Composition", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "Composition"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "Composition", 1)],
        },
    ];
}
