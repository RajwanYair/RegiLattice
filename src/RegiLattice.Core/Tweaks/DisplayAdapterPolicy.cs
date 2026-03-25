// RegiLattice.Core — Tweaks/DisplayAdapterPolicy.cs
// Sprint 266: Display Adapter Group Policy (10 tweaks)
// Category: "Display Adapter Policy" | Slug: dispadp
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DisplayAdapters

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class DisplayAdapterPolicy
{
    private const string Key =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DisplayAdapters";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "dispadp-block-driver-install",
            Label = "Block Display Driver Installation by Users",
            Category = "Display Adapter Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Sets DisableDriverInstall=1 in the DisplayAdapters policy key. Prevents "
                + "standard users from installing or updating display adapter drivers "
                + "without administrator approval. Stops unauthorised GPU driver "
                + "changes that could destabilise corporate workstations or bypass "
                + "validated driver stacks. Default: users with elevated privileges can "
                + "install drivers. Recommended: 1 on managed corporate images.",
            Tags = ["display", "driver", "lockdown", "security", "group-policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableDriverInstall", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableDriverInstall")],
            DetectOps = [RegOp.CheckDword(Key, "DisableDriverInstall", 1)],
        },
        new TweakDef
        {
            Id = "dispadp-force-standard-vga",
            Label = "Force Standard VGA Display Adapter",
            Category = "Display Adapter Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 3,
            Description =
                "Sets ForceStandardVGA=1 in the DisplayAdapters policy key. Forces Windows "
                + "to use the Standard VGA driver instead of vendor-supplied GPU drivers. "
                + "Useful during OS deployment, driver troubleshooting, or when locking "
                + "VDI sessions to a baseline display mode. Disables GPU acceleration and "
                + "hardware rendering in applications. Default: vendor driver is used. "
                + "Caution: significantly degrades graphical performance.",
            Tags = ["display", "vga", "driver", "vdi", "group-policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "ForceStandardVGA", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "ForceStandardVGA")],
            DetectOps = [RegOp.CheckDword(Key, "ForceStandardVGA", 1)],
        },
        new TweakDef
        {
            Id = "dispadp-disable-dxva",
            Label = "Disable DirectX Video Acceleration (DXVA)",
            Category = "Display Adapter Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            Description =
                "Sets DisableDXVA=1 in the DisplayAdapters policy key. Prevents "
                + "applications from using DirectX Video Acceleration (DXVA2) for "
                + "hardware-accelerated video decoding. Forces software-based video "
                + "decode. Useful in virtual environments where DXVA causes rendering "
                + "artefacts or when validating software rendering consistency. Default: "
                + "DXVA is active when supported by the GPU.",
            Tags = ["display", "dxva", "gpu", "video", "group-policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableDXVA", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableDXVA")],
            DetectOps = [RegOp.CheckDword(Key, "DisableDXVA", 1)],
        },
        new TweakDef
        {
            Id = "dispadp-disable-wddm-gpu-compute",
            Label = "Disable GPU Compute (DirectCompute) in WDDM",
            Category = "Display Adapter Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            Description =
                "Sets DisableGPUCompute=1 in the DisplayAdapters policy key. Prevents "
                + "applications from issuing DirectCompute (D3D11 Compute Shader / OpenCL) "
                + "workloads through the WDDM display driver. Eliminates the GPU compute "
                + "attack surface in locked-down environments and prevents unauthorised "
                + "use of GPU resources for cryptocurrency mining. Default: GPU compute "
                + "workloads are permitted.",
            Tags = ["display", "gpu", "compute", "security", "wddm", "group-policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableGPUCompute", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableGPUCompute")],
            DetectOps = [RegOp.CheckDword(Key, "DisableGPUCompute", 1)],
        },
        new TweakDef
        {
            Id = "dispadp-disable-display-scaling",
            Label = "Disable Custom Display Scaling (DPI Override) via Policy",
            Category = "Display Adapter Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Sets DisableDisplayScaling=1 in the DisplayAdapters policy key. Prevents "
                + "users from overriding the system DPI scaling factor set by IT. Ensures "
                + "a uniform 100% or 125% display scale across all managed workstations "
                + "for consistent application layout and screen recording output. Default: "
                + "DPI scale is user-configurable via Display Settings.",
            Tags = ["display", "dpi", "scaling", "group-policy", "lockdown"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableDisplayScaling", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableDisplayScaling")],
            DetectOps = [RegOp.CheckDword(Key, "DisableDisplayScaling", 1)],
        },
        new TweakDef
        {
            Id = "dispadp-disable-display-rotation",
            Label = "Disable Display Rotation by Users",
            Category = "Display Adapter Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Sets DisableDisplayRotation=1 in the DisplayAdapters policy key. Prevents "
                + "accidental or unauthorised screen rotation by locking the display "
                + "orientation to landscape (0°). Eliminates the risk of users or "
                + "applications flipping the display to portrait/inverted modes on "
                + "fixed-mount kiosk or desktop units. Default: rotation is freely "
                + "adjustable via Settings or keyboard shortcuts.",
            Tags = ["display", "rotation", "kiosk", "lockdown", "group-policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableDisplayRotation", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableDisplayRotation")],
            DetectOps = [RegOp.CheckDword(Key, "DisableDisplayRotation", 1)],
        },
        new TweakDef
        {
            Id = "dispadp-disable-mirroring",
            Label = "Disable Multi-Monitor Display Mirroring",
            Category = "Display Adapter Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Sets DisableMirroring=1 in the DisplayAdapters policy key. Prevents users "
                + "from setting up display mirroring (duplicate/clone) configurations "
                + "through Display Settings. Enforces extended desktop topology on "
                + "multi-monitor workstations where mirroring would reduce display "
                + "bandwidth or allow information to appear on a connected projector "
                + "without IT authorisation. Default: mirroring is available.",
            Tags = ["display", "mirroring", "multi-monitor", "group-policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableMirroring", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableMirroring")],
            DetectOps = [RegOp.CheckDword(Key, "DisableMirroring", 1)],
        },
        new TweakDef
        {
            Id = "dispadp-disable-resolution-change",
            Label = "Lock Display Resolution via Policy",
            Category = "Display Adapter Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Sets LockDisplayResolution=1 in the DisplayAdapters policy key. Prevents "
                + "users from changing the screen resolution beyond what has been set by "
                + "administrators. Ensures that kiosk, point-of-sale, or digital signage "
                + "displays maintain the correct native resolution at all times. Default: "
                + "resolution is freely adjustable. Recommended: 1 on fixed-function "
                + "machines with a designated native resolution.",
            Tags = ["display", "resolution", "kiosk", "lockdown", "group-policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "LockDisplayResolution", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "LockDisplayResolution")],
            DetectOps = [RegOp.CheckDword(Key, "LockDisplayResolution", 1)],
        },
        new TweakDef
        {
            Id = "dispadp-disable-refresh-rate-change",
            Label = "Lock Display Refresh Rate via Policy",
            Category = "Display Adapter Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Sets LockRefreshRate=1 in the DisplayAdapters policy key. Prevents users "
                + "from changing the monitor refresh rate. Ensures that the validated "
                + "refresh rate chosen by IT (e.g., 60 Hz for broadcast-safe output or "
                + "75 Hz for flicker-sensitive users) remains in force. Default: refresh "
                + "rate is configurable within the ranges supported by the display. "
                + "Recommended: 1 on studio, broadcast, or accessibility-critical systems.",
            Tags = ["display", "refresh-rate", "lockdown", "group-policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "LockRefreshRate", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "LockRefreshRate")],
            DetectOps = [RegOp.CheckDword(Key, "LockRefreshRate", 1)],
        },
        new TweakDef
        {
            Id = "dispadp-disable-color-depth-change",
            Label = "Lock Display Colour Depth via Policy",
            Category = "Display Adapter Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Sets LockColorDepth=1 in the DisplayAdapters policy key. Prevents users "
                + "from changing the colour depth (bits per pixel) of the display. Forces "
                + "the system to remain at the IT-set colour depth (typically 32 bpp). "
                + "Eliminates accidental changes to 16-bit colour mode that degrade UI "
                + "rendering quality and break applications expecting true-colour output. "
                + "Default: colour depth is user-configurable.",
            Tags = ["display", "color-depth", "bpp", "lockdown", "group-policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "LockColorDepth", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "LockColorDepth")],
            DetectOps = [RegOp.CheckDword(Key, "LockColorDepth", 1)],
        },
    ];
}
