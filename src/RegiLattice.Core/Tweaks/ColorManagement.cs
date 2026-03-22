// RegiLattice.Core — Tweaks/ColorManagement.cs
// Windows Color Management and ICC/ICM profile settings (Sprint 80).
// Slug "color" — ICC profile defaults, HDR calibration, color space settings.
// Distinct from Display.cs (resolution/refresh/scaling) and NightLight.cs.

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class ColorManagement
{
    private const string IcmUser = @"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\ICMRegData";

    private const string IcmSys = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\ICM";

    private const string DwmCompose = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM";

    private const string DisplayGamma = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\ICM";

    private const string HdrProfile = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Configuration";

    private const string ColorPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Display";

    private const string VideoSettings = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "color-disable-hdr-battery-impact",
            Label = "Disable HDR Streaming Battery Compensation",
            Category = "Color Management",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["color", "hdr", "battery", "display"],
            Description =
                "Disables the automatic brightness compensation that Windows applies "
                + "when HDR is active on battery power. Maintains consistent HDR brightness "
                + "regardless of power source.",
            ApplyOps = [RegOp.SetDword(VideoSettings, "HDRBatteryOptimization", 0)],
            RemoveOps = [RegOp.DeleteValue(VideoSettings, "HDRBatteryOptimization")],
            DetectOps = [RegOp.CheckDword(VideoSettings, "HDRBatteryOptimization", 0)],
        },
        new TweakDef
        {
            Id = "color-set-sdr-brightness-hdr-mode",
            Label = "Increase SDR Content Brightness in HDR Mode (80 nits)",
            Category = "Color Management",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["color", "hdr", "sdr", "brightness", "display"],
            Description =
                "Sets the SDR content brightness compensation to 80 nits when HDR mode "
                + "is active. Prevents SDR apps and desktop from appearing washed out next "
                + "to HDR content. Range: 0–100.",
            ApplyOps = [RegOp.SetDword(VideoSettings, "SDRBrightness", 80)],
            RemoveOps = [RegOp.DeleteValue(VideoSettings, "SDRBrightness")],
            DetectOps = [RegOp.CheckDword(VideoSettings, "SDRBrightness", 80)],
        },
        new TweakDef
        {
            Id = "color-disable-auto-color-management",
            Label = "Disable Automatic Display Color Management",
            Category = "Color Management",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["color", "icc", "profile", "display", "management"],
            Description =
                "Disables Windows automatic color management that adjusts output based "
                + "on ICC profiles. Useful for content creators who manage color profiles "
                + "manually through a dedicated color calibration tool.",
            ApplyOps = [RegOp.SetDword(ColorPolicy, "DisableAutoColorManagement", 1)],
            RemoveOps = [RegOp.DeleteValue(ColorPolicy, "DisableAutoColorManagement")],
            DetectOps = [RegOp.CheckDword(ColorPolicy, "DisableAutoColorManagement", 1)],
        },
        new TweakDef
        {
            Id = "color-enable-hdr-wcg-apps",
            Label = "Enable HDR and WCG for Apps",
            Category = "Color Management",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["color", "hdr", "wcg", "display", "wide color gamut"],
            Description =
                "Enables Wide Color Gamut (WCG) and HDR support for Windows apps on "
                + "compatible displays. Allows HDR-aware applications to display full "
                + "HDR content through the DirectX swap chain.",
            ApplyOps = [RegOp.SetDword(VideoSettings, "EnableHDRForApps", 1)],
            RemoveOps = [RegOp.DeleteValue(VideoSettings, "EnableHDRForApps")],
            DetectOps = [RegOp.CheckDword(VideoSettings, "EnableHDRForApps", 1)],
        },
        new TweakDef
        {
            Id = "color-disable-dwm-color-depth",
            Label = "Disable DWM 10-bit Color Override",
            Category = "Color Management",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["color", "dwm", "depth", "display"],
            Description =
                "Disables the Desktop Window Manager (DWM) 10-bit color processing "
                + "override. Reverts to 8-bit color depth through DWM composition. "
                + "Can fix color banding or rendering artifacts on some display drivers.",
            ApplyOps = [RegOp.SetDword(DwmCompose, "Use10BitColorDepth", 0)],
            RemoveOps = [RegOp.DeleteValue(DwmCompose, "Use10BitColorDepth")],
            DetectOps = [RegOp.CheckDword(DwmCompose, "Use10BitColorDepth", 0)],
        },
        new TweakDef
        {
            Id = "color-enable-dwm-color-depth",
            Label = "Enable DWM 10-bit Color Depth",
            Category = "Color Management",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["color", "dwm", "10bit", "display", "hdr"],
            Description =
                "Enables 10-bit color depth processing through the Desktop Window Manager. "
                + "Reduces banding in smooth gradients and improves color accuracy on "
                + "HDR/10-bit-capable displays.",
            ApplyOps = [RegOp.SetDword(DwmCompose, "Use10BitColorDepth", 1)],
            RemoveOps = [RegOp.DeleteValue(DwmCompose, "Use10BitColorDepth")],
            DetectOps = [RegOp.CheckDword(DwmCompose, "Use10BitColorDepth", 1)],
        },
        new TweakDef
        {
            Id = "color-disable-night-light-gamma",
            Label = "Disable Night Light Gamma Ramp Interference",
            Category = "Color Management",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["color", "night light", "gamma", "display", "calibration"],
            Description =
                "Disables the Night Light gamma ramp that can interfere with ICC color "
                + "profile calibration. Set to 0 to prevent Night Light from overriding "
                + "your display's calibrated color temperature.",
            ApplyOps = [RegOp.SetDword(DwmCompose, "ColorizationOpaqueBlend", 0)],
            RemoveOps = [RegOp.DeleteValue(DwmCompose, "ColorizationOpaqueBlend")],
            DetectOps = [RegOp.CheckDword(DwmCompose, "ColorizationOpaqueBlend", 0)],
        },
        new TweakDef
        {
            Id = "color-enable-calibration-system",
            Label = "Enable ICM Gamma Calibration System-Wide",
            Category = "Color Management",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["color", "icc", "icm", "calibration", "gamma"],
            Description =
                "Enables the Windows ICM gamma calibration system for all displays "
                + "at the session level. Required for ICC profiles that include gamma "
                + "correction to be applied correctly.",
            ApplyOps = [RegOp.SetDword(IcmSys, "GammaCal", 1)],
            RemoveOps = [RegOp.DeleteValue(IcmSys, "GammaCal")],
            DetectOps = [RegOp.CheckDword(IcmSys, "GammaCal", 1)],
        },
        new TweakDef
        {
            Id = "color-disable-hdr-auto-adjust",
            Label = "Disable HDR Automatic Tone Mapping",
            Category = "Color Management",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["color", "hdr", "tone mapping", "display", "auto"],
            Description =
                "Disables Windows automatic HDR tone mapping that adjusts content "
                + "brightness dynamically. Useful for video editors who need precise "
                + "HDR level control without system interference.",
            ApplyOps = [RegOp.SetDword(VideoSettings, "HDRToneMapEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(VideoSettings, "HDRToneMapEnabled")],
            DetectOps = [RegOp.CheckDword(VideoSettings, "HDRToneMapEnabled", 0)],
        },
        new TweakDef
        {
            Id = "color-force-full-color-range",
            Label = "Force Full Color Range RGB (0-255)",
            Category = "Color Management",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["color", "rgb", "range", "full", "display"],
            Description =
                "Forces the display output to use Full RGB color range (0–255) rather "
                + "than Limited range (16–235). Eliminates washed-out colors and improves "
                + "contrast on monitors that support full range input.",
            ApplyOps = [RegOp.SetDword(VideoSettings, "ForceFullColorRange", 1)],
            RemoveOps = [RegOp.DeleteValue(VideoSettings, "ForceFullColorRange")],
            DetectOps = [RegOp.CheckDword(VideoSettings, "ForceFullColorRange", 1)],
        },
    ];
}
