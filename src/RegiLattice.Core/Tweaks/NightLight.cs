namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class NightLight
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "night-enable-night-light",
            Label = "Enable Night Light (Blue Light Filter)",
            Category = "Night Light & Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables Windows Night Light to reduce blue light emission. Schedule can be configured in Windows Settings.",
            Tags = ["night-light", "blue-light", "display", "eye-strain"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CloudStore\Store\DefaultAccount\Current\default$windows.data.bluelightreduction.bluelightreductionstate\windows.data.bluelightreduction.bluelightreductionstate"],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CloudStore\Store\DefaultAccount\Current\default$windows.data.bluelightreduction.bluelightreductionstate\windows.data.bluelightreduction.bluelightreductionstate", "Data"),
            ],
        },
        new TweakDef
        {
            Id = "night-enable-hdr",
            Label = "Enable HDR Video Playback",
            Category = "Night Light & Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables HDR video playback on HDR-capable displays. Requires hardware support. Default: Disabled.",
            Tags = ["night-light", "hdr", "display", "video"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings", "EnableHDR", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings", "EnableHDR", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings", "EnableHDR", 1)],
        },
        new TweakDef
        {
            Id = "night-hdr-auto-brightness",
            Label = "Enable Auto HDR Brightness",
            Category = "Night Light & Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables automatic brightness adjustment for HDR content. Optimises SDR-to-HDR content mapping.",
            Tags = ["night-light", "hdr", "brightness", "auto"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings", "AutoHDR", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings", "AutoHDR", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings", "AutoHDR", 1)],
        },
        new TweakDef
        {
            Id = "night-disable-cabc",
            Label = "Disable Content Adaptive Brightness",
            Category = "Night Light & Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Content Adaptive Brightness Control (CABC) which adjusts screen brightness based on content. Can cause distracting brightness shifts. Recommended: Disabled for content creation.",
            Tags = ["night-light", "brightness", "cabc", "display"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "CABCEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "CABCEnabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "CABCEnabled", 0)],
        },
        new TweakDef
        {
            Id = "night-disable-adaptive-colour",
            Label = "Disable Adaptive Colour",
            Category = "Night Light & Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the adaptive colour feature that shifts display colours based on ambient light. Provides consistent colour output.",
            Tags = ["night-light", "colour", "adaptive", "calibration"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\ICM"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\ICM", "AdaptiveColorEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\ICM", "AdaptiveColorEnabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\ICM", "AdaptiveColorEnabled", 0)],
        },
        new TweakDef
        {
            Id = "night-enable-wcg",
            Label = "Enable Wide Colour Gamut (WCG)",
            Category = "Night Light & Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables Wide Colour Gamut support for richer colours on compatible displays. Default: Disabled.",
            Tags = ["night-light", "wcg", "colour", "gamut", "display"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings", "EnableWCG", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings", "EnableWCG", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings", "EnableWCG", 1)],
        },
        new TweakDef
        {
            Id = "night-disable-hdr-streaming",
            Label = "Disable HDR for Streaming Video",
            Category = "Night Light & Display",
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
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings", "EnableHDRForStreamingVideo", 0)],
        },
        new TweakDef
        {
            Id = "night-per-process-gpu",
            Label = "Enable Per-Process GPU Selection",
            Category = "Night Light & Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables DirectX swap chain upgrade for better GPU selection per application. May improve hybrid GPU laptops.",
            Tags = ["night-light", "gpu", "directx", "per-process"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\DirectX\UserGpuPreferences"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\Microsoft\DirectX\UserGpuPreferences", "DirectXUserGlobalSettings", "SwapEffectUpgradeEnable=1;"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\DirectX\UserGpuPreferences", "DirectXUserGlobalSettings"),
            ],
        },
        new TweakDef
        {
            Id = "night-disable-display-gp",
            Label = "Lock Display Settings (Policy)",
            Category = "Night Light & Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents users from changing display settings via Group Policy. Useful for kiosk/shared machines.",
            Tags = ["night-light", "display", "policy", "lock"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Display"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Display", "DisableDisplaySettings", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Display", "DisableDisplaySettings"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Display", "DisableDisplaySettings", 1)],
        },
        new TweakDef
        {
            Id = "night-keep-hdr-battery",
            Label = "Keep HDR on Battery Power",
            Category = "Night Light & Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows from automatically disabling HDR when running on battery. May reduce battery life.",
            Tags = ["night-light", "hdr", "battery", "laptop"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings"],
        },
        new TweakDef
        {
            Id = "night-srgb-default",
            Label = "Set Default Colour Profile to sRGB",
            Category = "Night Light & Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets the default colour profile to standard sRGB IEC61966-2.1. Ensures consistent colour across applications.",
            Tags = ["night-light", "colour", "srgb", "profile", "calibration"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\ICM"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\ICM", "ICMProfile", "sRGB IEC61966-2.1"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\ICM", "ICMProfile"),
            ],
        },
        new TweakDef
        {
            Id = "night-disable-dwm-hdr",
            Label = "Disable DWM HDR Compositor (Policy)",
            Category = "Night Light & Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Desktop Window Manager HDR compositor via policy. Force SDR mode even on HDR displays.",
            Tags = ["night-light", "hdr", "dwm", "compositor", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DWM"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DWM", "DisableHDR", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DWM", "DisableHDR"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DWM", "DisableHDR", 1)],
        },
        new TweakDef
        {
            Id = "night-disable-cleartype",
            Label = "Disable ClearType Font Smoothing",
            Category = "Night Light & Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables ClearType/anti-aliased font rendering. Reverts to standard aliased fonts. Some users prefer sharper pixel-perfect text. Default: ClearType enabled (value 2).",
            Tags = ["night-light", "display", "cleartype", "fonts", "rendering"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothing", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothing", 2),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothing", 0)],
        },
        new TweakDef
        {
            Id = "night-disable-dynamic-refresh",
            Label = "Disable Dynamic Refresh Rate Switching",
            Category = "Night Light & Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the GraphicsDrivers dynamic refresh rate switch. Forces a static refresh rate, which can prevent flicker on some displays. Default: Dynamic switching enabled.",
            Tags = ["night-light", "display", "refresh-rate", "gpu", "graphics"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
        },
        new TweakDef
        {
            Id = "night-enable-vivid-colour",
            Label = "Enable Vivid Display Colour Mode",
            Category = "Night Light & Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets the video colour profile to Vivid mode (UseHDR=2). Increases saturation for SDR content on HDR displays. Default: Off.",
            Tags = ["night-light", "hdr", "vivid", "colour", "display"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings", "UseHDR", 2),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings", "UseHDR", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings", "UseHDR", 2)],
        },
        new TweakDef
        {
            Id = "night-disable-schedule",
            Label = "Disable Night Light Auto Schedule",
            Category = "Night Light & Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Turns off the Night Light automatic schedule so it does not enable at sunset/sunrise. Useful when Night Light was enabled but scheduling is unwanted. Default: Varies.",
            Tags = ["night-light", "schedule", "display", "blue-light"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CloudStore\Store\DefaultAccount\Current\default$windows.data.bluelightreduction.settings\windows.data.bluelightreduction.settings"],
        },
        new TweakDef
        {
            Id = "night-disable-icc-auto",
            Label = "Disable Auto ICC Colour Profile",
            Category = "Night Light & Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables automatic ICC colour profile activation by Windows Colour Management. Useful when custom calibration profiles cause unintended colour shifts. Default: Enabled.",
            Tags = ["night-light", "icc", "colour", "calibration", "display"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\ICM"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\ICM", "ICMSystemActivationEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\ICM", "ICMSystemActivationEnabled", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\ICM", "ICMSystemActivationEnabled", 0)],
        },
        new TweakDef
        {
            Id = "night-disable-hwsch",
            Label = "Disable Hardware GPU Scheduler",
            Category = "Night Light & Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Hardware-Accelerated GPU Scheduler (HWSCH). Can resolve flickering or latency issues on some GPU models. Default: Enabled (HwSchMode=2). Recommended: Disable if experiencing display artefacts.",
            Tags = ["night-light", "gpu", "scheduler", "hwsch", "latency", "display"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "HwSchMode", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "HwSchMode", 2),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "HwSchMode", 1)],
        },
        new TweakDef
        {
            Id = "night-disable-adaptive-color",
            Label = "Disable Adaptive Color Temperature",
            Category = "Night Light & Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Windows adaptive color temperature adjustment. Keeps display color consistent. Default: varies.",
            Tags = ["nightlight", "adaptive", "color", "temperature"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CloudStore\Store\DefaultAccount\Current\default$windows.data.bluelightreduction.settings"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "EnableTransparency", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "EnableTransparency")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "EnableTransparency", 1)],
        },
        new TweakDef
        {
            Id = "night-enable-dark-mode",
            Label = "Enable System Dark Mode",
            Category = "Night Light & Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables dark mode for Windows system elements and apps. Default: light mode.",
            Tags = ["nightlight", "dark-mode", "theme", "ui"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "SystemUsesLightTheme", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "SystemUsesLightTheme", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "SystemUsesLightTheme", 0)],
        },
        new TweakDef
        {
            Id = "night-enable-app-dark-mode",
            Label = "Enable App Dark Mode",
            Category = "Night Light & Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables dark mode for Windows apps specifically (separate from system theme). Default: light.",
            Tags = ["nightlight", "dark-mode", "apps", "theme"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "AppsUseLightTheme", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "AppsUseLightTheme", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "AppsUseLightTheme", 0)],
        },
        new TweakDef
        {
            Id = "night-disable-transparency-effects",
            Label = "Disable Transparency Effects",
            Category = "Night Light & Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables transparency/blur effects in Windows (taskbar, start menu, action center). Saves GPU resources. Default: enabled.",
            Tags = ["nightlight", "transparency", "blur", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "EnableTransparency", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "EnableTransparency", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "EnableTransparency", 0)],
        },
        new TweakDef
        {
            Id = "night-disable-color-filters",
            Label = "Disable Color Filters",
            Category = "Night Light & Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the Windows color filter overlay (grayscale, inverted, etc.). Default: disabled.",
            Tags = ["nightlight", "color-filter", "accessibility", "display"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\ColorFiltering"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\ColorFiltering", "Active", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\ColorFiltering", "Active")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\ColorFiltering", "Active", 0)],
        },
    ];
}
