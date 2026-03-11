namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Fonts
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "font-enable-cleartype",
            Label = "Enable ClearType Font Rendering",
            Category = "Fonts",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables ClearType sub-pixel rendering for sharper text on LCD displays (sets FontSmoothingType to 2).",
            Tags = ["fonts", "cleartype", "rendering", "display"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothingType", "2"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothingType", "1"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothingType", "2")],
        },
        new TweakDef
        {
            Id = "font-enable-smoothing",
            Label = "Enable Font Smoothing",
            Category = "Fonts",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Activates font smoothing at the system level so all text benefits from anti-aliased rendering.",
            Tags = ["fonts", "smoothing", "display"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
        },
        new TweakDef
        {
            Id = "font-disable-antialiasing",
            Label = "Disable Font Antialiasing (Performance)",
            Category = "Fonts",
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
            Category = "Fonts",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Registers Segoe UI and its variants as the per-user default font, overriding any previous user-level font substitution.",
            Tags = ["fonts", "segoe", "default", "system"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Fonts"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Fonts", "Segoe UI (TrueType)", "segoeui.ttf"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Fonts", "Segoe UI Bold (TrueType)", "segoeuib.ttf"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Fonts", "Segoe UI Italic (TrueType)", "segoeuii.ttf"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Fonts", "Segoe UI Bold Italic (TrueType)", "segoeuiz.ttf"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Fonts", "Segoe UI (TrueType)"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Fonts", "Segoe UI Bold (TrueType)"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Fonts", "Segoe UI Italic (TrueType)"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Fonts", "Segoe UI Bold Italic (TrueType)"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Fonts", "Segoe UI (TrueType)", "segoeui.ttf")],
        },
        new TweakDef
        {
            Id = "font-disable-download-edge",
            Label = "Disable Font Download in Edge",
            Category = "Fonts",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Microsoft Edge from downloading web fonts via the DefaultFontDownloadSetting policy (value 2 = block).",
            Tags = ["fonts", "edge", "download", "policy", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
        },
        new TweakDef
        {
            Id = "font-block-untrusted",
            Label = "Block Untrusted Font Loading",
            Category = "Fonts",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Blocks loading of untrusted fonts from user-writable locations via the kernel MitigationOptions flag — hardens the system against font-based exploits.",
            Tags = ["fonts", "untrusted", "security", "mitigation", "kernel"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Kernel"],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Kernel", "MitigationOptions"),
            ],
        },
        new TweakDef
        {
            Id = "font-disable-fontcache-service",
            Label = "Disable Font Cache Service",
            Category = "Fonts",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Windows Font Cache Service (FontCache). May reduce memory usage but can slow down font loading.",
            Tags = ["fonts", "cache", "service", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\FontCache"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\FontCache", "Start", 4),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\FontCache", "Start", 2),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\FontCache", "Start", 4)],
        },
        new TweakDef
        {
            Id = "font-disable-fontcache3-service",
            Label = "Disable Font Cache 3.0 Service",
            Category = "Fonts",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Windows Presentation Foundation Font Cache 3.0 Service used by WPF applications.",
            Tags = ["fonts", "cache", "wpf", "service"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\FontCache3.0.0.0"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\FontCache", "Start", 4),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\FontCache", "Start", 2),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\FontCache", "Start", 4)],
        },
        new TweakDef
        {
            Id = "font-cleartype-tuning",
            Label = "Set ClearType Tuning to Maximum",
            Category = "Fonts",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets the ClearType rendering level to 100 (maximum) for WPF and Avalon-based applications on the primary display.",
            Tags = ["fonts", "cleartype", "tuning", "wpf", "rendering"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Avalon.Graphics\DISPLAY1"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Avalon.Graphics\DISPLAY1", "ClearTypeLevel", 100),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Avalon.Graphics\DISPLAY1", "ClearTypeLevel"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Avalon.Graphics\DISPLAY1", "ClearTypeLevel", 100)],
        },
        new TweakDef
        {
            Id = "font-natural-cleartype-contrast",
            Label = "Enable Natural ClearType Contrast",
            Category = "Fonts",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets the WPF text contrast level to 1 for a more natural, softer ClearType appearance on the primary display.",
            Tags = ["fonts", "cleartype", "contrast", "wpf", "rendering"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Avalon.Graphics\DISPLAY1"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Avalon.Graphics\DISPLAY1", "TextContrastLevel", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Avalon.Graphics\DISPLAY1", "TextContrastLevel"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Avalon.Graphics\DISPLAY1", "TextContrastLevel", 1)],
        },
        new TweakDef
        {
            Id = "font-wpf-hw-text-rendering",
            Label = "Enable WPF Hardware Text Rendering",
            Category = "Fonts",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Ensures WPF applications use GPU-accelerated text rendering by explicitly setting DisableHWAcceleration to 0.",
            Tags = ["fonts", "wpf", "gpu", "hardware", "rendering"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Avalon.Graphics"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Avalon.Graphics", "DisableHWAcceleration", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Avalon.Graphics", "DisableHWAcceleration"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Avalon.Graphics", "DisableHWAcceleration", 0)],
        },
        new TweakDef
        {
            Id = "font-block-ie-zone-download",
            Label = "Block Font Downloads in Internet Zone",
            Category = "Fonts",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Blocks downloading of fonts in the Internet security zone (Zone 3) via the 1604 policy value — prevents drive-by font-based exploits in legacy applications.",
            Tags = ["fonts", "internet", "zone", "download", "security", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\Internet Settings\Zones\3"],
        },
        new TweakDef
        {
            Id = "font-fonts-disable-streaming",
            Label = "Disable Font Streaming",
            Category = "Fonts",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables cloud font streaming from Microsoft. Prevents background font downloads. Reduces network traffic. Default: Enabled. Recommended: Disabled.",
            Tags = ["fonts", "streaming", "network", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
        },
        new TweakDef
        {
            Id = "font-fonts-cleartype-performance",
            Label = "Set ClearType Gamma",
            Category = "Fonts",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Optimizes ClearType font rendering gamma to 2200 for better readability on LCD displays. Default: 1800. Recommended: 2200.",
            Tags = ["fonts", "cleartype", "rendering", "display"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Avalon.Graphics\DISPLAY1"],
        },
        new TweakDef
        {
            Id = "font-fonts-disable-font-fallback",
            Label = "Disable Font Fallback",
            Category = "Fonts",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Overrides MS Shell Dlg font fallback to Segoe UI for consistent rendering across legacy and modern applications. Default: Microsoft Sans Serif. Recommended: Segoe UI.",
            Tags = ["fonts", "fallback", "substitutes", "rendering"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\FontSubstitutes"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\FontSubstitutes", "MS Shell Dlg", "Segoe UI"),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\FontSubstitutes", "MS Shell Dlg 2", "Segoe UI"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\FontSubstitutes", "MS Shell Dlg", "Microsoft Sans Serif"),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\FontSubstitutes", "MS Shell Dlg 2", "Tahoma"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\FontSubstitutes", "MS Shell Dlg", "Segoe UI")],
        },
        new TweakDef
        {
            Id = "font-fonts-disable-font-antialiasing",
            Label = "Disable Font Anti-Aliasing",
            Category = "Fonts",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables font smoothing/anti-aliasing for sharper pixel-aligned text. May improve readability on low-DPI screens. Default: 2 (enabled). Recommended: Disabled for CRT/low-DPI.",
            Tags = ["fonts", "antialiasing", "smoothing", "rendering"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothing", "0"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothing", "2"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothing", "0")],
        },
        new TweakDef
        {
            Id = "font-set-smoothing-orientation",
            Label = "Set Font Smoothing Orientation to RGB",
            Category = "Fonts",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets subpixel font smoothing orientation to RGB for standard LCD panels. Improves ClearType rendering on horizontal RGB displays. Default: 0 (auto). Recommended: 1 (RGB) for most monitors.",
            Tags = ["fonts", "cleartype", "subpixel", "orientation", "rendering"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothingOrientation", "1"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothingOrientation", "0"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothingOrientation", "1")],
        },
        new TweakDef
        {
            Id = "font-set-cleartype-contrast",
            Label = "Set ClearType High Contrast",
            Category = "Fonts",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets ClearType gamma to 1000 for higher contrast text rendering. Makes text appear bolder and easier to read on most displays. Default: 1400. Recommended: 1000 for high-DPI screens.",
            Tags = ["fonts", "cleartype", "contrast", "gamma", "rendering"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothingGamma", 1000),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothingGamma", 1400),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothingGamma", 1000)],
        },
        new TweakDef
        {
            Id = "font-increase-glyph-cache",
            Label = "Increase GDI Glyph Cache Size",
            Category = "Fonts",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Increases the GDI font glyph cache from default 2 MB to 4 MB. Reduces glyph re-rasterization in multi-font or CJK workloads. Default: 2097152 (~2 MB). Recommended: 4194304 (4 MB).",
            Tags = ["fonts", "glyph-cache", "gdi", "performance", "rendering"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\GRE_Initialize"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\GRE_Initialize", "GlyphCacheSize", 4194304),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\GRE_Initialize", "GlyphCacheSize"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\GRE_Initialize", "GlyphCacheSize", 4194304)],
        },
        new TweakDef
        {
            Id = "font-disable-font-streaming",
            Label = "Disable Font Streaming",
            Category = "Fonts",
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
            Category = "Fonts",
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
            Category = "Fonts",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Blocks untrusted fonts from loading in processes. Mitigates font parsing vulnerabilities. Default: off.",
            Tags = ["fonts", "security", "untrusted", "mitigation"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Kernel"],
            ApplyOps = [RegOp.SetQword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Kernel", "MitigationOptions", 0x1000000000000)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Kernel", "MitigationOptions")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Kernel", "MitigationOptions", 1)],
        },
        new TweakDef
        {
            Id = "font-set-default-console-font",
            Label = "Set Cascadia Mono as Default Console Font",
            Category = "Fonts",
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
            Category = "Fonts",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents users from installing fonts per-user. Requires admin font installation. Default: allowed.",
            Tags = ["fonts", "installation", "user", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "BlockNonAdminUserInstall", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "BlockNonAdminUserInstall")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "BlockNonAdminUserInstall", 1)],
        },
    ];
}
