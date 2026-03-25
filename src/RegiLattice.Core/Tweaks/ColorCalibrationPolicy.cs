// RegiLattice.Core — Tweaks/ColorCalibrationPolicy.cs
// Sprint 265: Color Calibration Group Policy (10 tweaks)
// Category: "Color Calibration Policy" | Slug: colcal
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ColorControl

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class ColorCalibrationPolicy
{
    private const string Key =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ColorControl";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "colcal-disable-calibration",
            Label = "Disable Display Color Calibration",
            Category = "Color Calibration Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Sets DisableColorCalibration=1 in the ColorControl policy key. Prevents "
                + "users from running the Windows Display Color Calibration tool "
                + "(dccw.exe) and applying custom color calibration profiles via the "
                + "Color Management wizard. Ensures a consistent visual baseline across "
                + "managed workstations. Default: calibration is available to users. "
                + "Recommended: 1 on corporate workstations with standardised displays.",
            Tags = ["display", "color", "calibration", "group-policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableColorCalibration", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableColorCalibration")],
            DetectOps = [RegOp.CheckDword(Key, "DisableColorCalibration", 1)],
        },
        new TweakDef
        {
            Id = "colcal-disable-icm-support",
            Label = "Disable Image Colour Management (ICM)",
            Category = "Color Calibration Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Sets EnableICMSupport=0 in the ColorControl policy key. Disables Image "
                + "Colour Management within GDI, preventing Windows from applying ICC "
                + "colour profiles to rendered output. On machines where colour accuracy "
                + "is irrelevant (kiosks, servers, projectors) this removes unnecessary "
                + "profile-load overhead at startup. Default: ICM is active when a "
                + "profile is associated with the display.",
            Tags = ["display", "color", "icm", "profile", "group-policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableICMSupport", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableICMSupport")],
            DetectOps = [RegOp.CheckDword(Key, "EnableICMSupport", 0)],
        },
        new TweakDef
        {
            Id = "colcal-hide-color-management-ui",
            Label = "Hide Color Management Control Panel",
            Category = "Color Calibration Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Sets HideColorManagement=1 in the ColorControl policy key. Removes the "
                + "Color Management applet from the Control Panel and Display Properties "
                + "dialog so users cannot associate, install, or remove ICC profiles. "
                + "Ensures that centrally deployed colour profiles cannot be overridden. "
                + "Default: Color Management is accessible to all users.",
            Tags = ["display", "color", "control-panel", "group-policy", "lockdown"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "HideColorManagement", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "HideColorManagement")],
            DetectOps = [RegOp.CheckDword(Key, "HideColorManagement", 1)],
        },
        new TweakDef
        {
            Id = "colcal-disable-auto-display-calibration",
            Label = "Disable Automatic Display Calibration",
            Category = "Color Calibration Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Sets DisableAutoCalibration=1 in the ColorControl policy key. Prevents "
                + "Windows from scheduling and running the automatic display calibration "
                + "cycle that nudges display gamma toward sRGB using hardware VCGT data. "
                + "On monitors with hardware calibration this is redundant; disabling "
                + "avoids conflicting with dedicated calibration software. Default: "
                + "automatic calibration runs on schedule when enabled.",
            Tags = ["display", "color", "calibration", "automation", "group-policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableAutoCalibration", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoCalibration")],
            DetectOps = [RegOp.CheckDword(Key, "DisableAutoCalibration", 1)],
        },
        new TweakDef
        {
            Id = "colcal-block-icc-profile-install",
            Label = "Block User ICC Profile Installation",
            Category = "Color Calibration Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Sets PreventUserICCInstall=1 in the ColorControl policy key. Blocks "
                + "standard users from installing ICC/ICM colour profile files. Only "
                + "administrators can add new profiles to the system profile store. "
                + "Prevents inadvertent colour profile swaps that may affect colour-critical "
                + "workflows or introduce unsigned third-party profiles. Default: any user "
                + "can install profiles.",
            Tags = ["display", "color", "icc", "security", "group-policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "PreventUserICCInstall", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "PreventUserICCInstall")],
            DetectOps = [RegOp.CheckDword(Key, "PreventUserICCInstall", 1)],
        },
        new TweakDef
        {
            Id = "colcal-disable-night-light-gpo",
            Label = "Disable Night Light via Group Policy",
            Category = "Color Calibration Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Sets DisableNightLight=1 in the ColorControl policy key. Prevents users "
                + "from enabling or scheduling the Night Light (blue-light reduction) "
                + "feature via Settings. Ensures consistent colour temperature on "
                + "colour-critical workstations (photo/video editing, medical imaging) "
                + "where Night Light would distort colour accuracy. Default: Night Light "
                + "is user-configurable.",
            Tags = ["display", "night-light", "color", "group-policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableNightLight", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableNightLight")],
            DetectOps = [RegOp.CheckDword(Key, "DisableNightLight", 1)],
        },
        new TweakDef
        {
            Id = "colcal-disable-hdr-gpo",
            Label = "Disable HDR Display Support via Group Policy",
            Category = "Color Calibration Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Sets DisableHDRSupport=1 in the ColorControl policy key. Forces Windows "
                + "to treat all displays as SDR (Standard Dynamic Range), preventing "
                + "users from enabling HDR in Display Settings. Useful on workstations "
                + "where HDR causes UI element clipping in SDR applications or "
                + "introduces colour management inconsistencies. Default: HDR enabled "
                + "when supported hardware is detected.",
            Tags = ["display", "hdr", "color", "group-policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableHDRSupport", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableHDRSupport")],
            DetectOps = [RegOp.CheckDword(Key, "DisableHDRSupport", 1)],
        },
        new TweakDef
        {
            Id = "colcal-disable-wcs-service",
            Label = "Disable Windows Color System Background Service",
            Category = "Color Calibration Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Sets DisableWCSService=1 in the ColorControl policy key. Prevents the "
                + "Windows Colour System (WCS) background service from loading, which "
                + "handles baseline display characterisation data and WCS profile "
                + "mapping. On machines without colour-critical workflows this service "
                + "consumes memory for no perceptible benefit. Default: WCS service "
                + "runs when the system boots.",
            Tags = ["display", "color", "wcs", "services", "group-policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableWCSService", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableWCSService")],
            DetectOps = [RegOp.CheckDword(Key, "DisableWCSService", 1)],
        },
        new TweakDef
        {
            Id = "colcal-disable-color-rendering-intent",
            Label = "Lock Colour Rendering Intent to Absolute Colorimetric",
            Category = "Color Calibration Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Sets ForceAbsoluteColorimetric=1 in the ColorControl policy key. Overrides "
                + "per-application colour rendering intent settings and forces all GDI "
                + "colour-managed output to use Absolute Colorimetric intent. Eliminates "
                + "gamma and white-point remapping that can introduce colour shifts on "
                + "wide-gamut P3 or AdobeRGB displays. Default: intent is set per profile "
                + "and application. Caution: may affect soft-proofing workflows.",
            Tags = ["display", "color", "rendering", "group-policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "ForceAbsoluteColorimetric", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "ForceAbsoluteColorimetric")],
            DetectOps = [RegOp.CheckDword(Key, "ForceAbsoluteColorimetric", 1)],
        },
        new TweakDef
        {
            Id = "colcal-disable-auto-color-correction",
            Label = "Disable Auto Color Correction",
            Category = "Color Calibration Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Sets DisableAutoColorCorrection=1 in the ColorControl policy key. Stops "
                + "Windows from applying the automatic colour correction layer that "
                + "adjusts output colour based on ambient light sensor data or display "
                + "warm-up compensation. Prevents unexpected colour shifts during the "
                + "work session. Default: auto correction is applied when supported by "
                + "display hardware. Recommended: 1 on precision colour workstations.",
            Tags = ["display", "color", "correction", "ambient", "group-policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableAutoColorCorrection", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoColorCorrection")],
            DetectOps = [RegOp.CheckDword(Key, "DisableAutoColorCorrection", 1)],
        },
    ];
}
