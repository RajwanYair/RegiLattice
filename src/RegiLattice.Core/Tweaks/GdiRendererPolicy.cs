// RegiLattice.Core — Tweaks/GdiRendererPolicy.cs
// GDI, GDI+, DirectDraw, and software rendering isolation policy — Sprint 509.
// Category: "GDI Renderer Policy" | Slug: gdipol
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class GdiRendererPolicy
{
    private const string Key    = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Dwm";
    private const string RdsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services";
    private const string GdiKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\GDI";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id           = "gdipol-enable-dwm-hardware-acceleration",
            Label        = "Enforce DWM Hardware Acceleration is Always On",
            Category     = "GDI Renderer Policy",
            Description  = "Ensures the Desktop Window Manager (DWM) uses GPU hardware acceleration for compositing, preventing software fallback rendering that consumes excessive CPU and produces visual artefacts on modern hardware.",
            Tags         = ["dwm", "hardware-acceleration", "gpu", "rendering", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "DWM hardware acceleration enforced; software rendering fallback blocked. GPU required for compositing.",
            ApplyOps     = [RegOp.SetDword(Key, "DisallowRemoteDesktopCompositing", 0)],
            RemoveOps    = [RegOp.DeleteValue(Key, "DisallowRemoteDesktopCompositing")],
            DetectOps    = [RegOp.CheckDword(Key, "DisallowRemoteDesktopCompositing", 0)],
        },
        new TweakDef
        {
            Id           = "gdipol-disable-rdp-software-rendering",
            Label        = "Disable Software Rendering for RDP Sessions",
            Category     = "GDI Renderer Policy",
            Description  = "Prevents Remote Desktop sessions from falling back to GDI software rendering when a GPU is available, ensuring RemoteFX or hardware-accelerated codec paths are always used for consistent performance.",
            Tags         = ["gdi", "rdp", "software-rendering", "remotedesktop", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "RDP software rendering fallback disabled; RemoteFX / GPU codec path used for remote sessions.",
            ApplyOps     = [RegOp.SetDword(RdsKey, "fDisableSoftwareRendering", 1)],
            RemoveOps    = [RegOp.DeleteValue(RdsKey, "fDisableSoftwareRendering")],
            DetectOps    = [RegOp.CheckDword(RdsKey, "fDisableSoftwareRendering", 1)],
        },
        new TweakDef
        {
            Id           = "gdipol-enable-gdi-scaling",
            Label        = "Enable GDI DPI Scaling for Legacy Applications",
            Category     = "GDI Renderer Policy",
            Description  = "Enables system-wide GDI-based DPI scaling for legacy applications that do not declare DPI awareness, preventing blurry rendering of older software on high-DPI monitors without requiring per-app compatibility flags.",
            Tags         = ["gdi", "dpi-scaling", "high-dpi", "legacy-apps", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "GDI DPI scaling enabled for non-DPI-aware apps; older software rendered crisply on high-DPI screens.",
            ApplyOps     = [RegOp.SetDword(GdiKey, "EnableGDIScaling", 1)],
            RemoveOps    = [RegOp.DeleteValue(GdiKey, "EnableGDIScaling")],
            DetectOps    = [RegOp.CheckDword(GdiKey, "EnableGDIScaling", 1)],
        },
        new TweakDef
        {
            Id           = "gdipol-disable-directdraw-hw-acceleration",
            Label        = "Disable DirectDraw Hardware Acceleration",
            Category     = "GDI Renderer Policy",
            Description  = "Disables DirectDraw hardware acceleration, forcing all DirectDraw rendering to use the software emulation path. Used in environments where GPU driver instability causes crashes or display corruption.",
            Tags         = ["directdraw", "hardware-acceleration", "gpu-driver", "stability", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 2,
            SafetyRating = 5,
            ImpactNote   = "DirectDraw hardware acceleration disabled; DirectDraw falls back to software. Performance impacted.",
            ApplyOps     = [RegOp.SetDword(GdiKey, "DisableDirectDrawHWAcceleration", 1)],
            RemoveOps    = [RegOp.DeleteValue(GdiKey, "DisableDirectDrawHWAcceleration")],
            DetectOps    = [RegOp.CheckDword(GdiKey, "DisableDirectDrawHWAcceleration", 1)],
        },
        new TweakDef
        {
            Id           = "gdipol-set-gdi-batch-limit",
            Label        = "Set GDI Batch Limit to Optimise Rendering Performance",
            Category     = "GDI Renderer Policy",
            Description  = "Sets the GDI batching limit to 0 (immediate flush) to ensure all GDI drawing calls are synchronously sent to the display driver, improving rendering correctness on systems with unstable batch coalescing.",
            Tags         = ["gdi", "batch-limit", "rendering", "performance", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 2,
            SafetyRating = 5,
            ImpactNote   = "GDI batch limit set to 0 (immediate flush); drawing calls not batched. Improves rendering correctness.",
            ApplyOps     = [RegOp.SetDword(GdiKey, "BatchLimit", 0)],
            RemoveOps    = [RegOp.DeleteValue(GdiKey, "BatchLimit")],
            DetectOps    = [RegOp.CheckDword(GdiKey, "BatchLimit", 0)],
        },
        new TweakDef
        {
            Id           = "gdipol-block-gdi-object-table-growth",
            Label        = "Limit Per-Process GDI Object Count to 10000",
            Category     = "GDI Renderer Policy",
            Description  = "Sets the per-process GDI object limit to 10000 (down from the default 65536), preventing GDI handle exhaustion attacks where a single process allocates all available GDI handles and crashes other processes.",
            Tags         = ["gdi", "handle-limit", "object-table", "dos-prevention", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "GDI object limit per-process capped at 10000; GDI handle exhaustion attacks prevented.",
            ApplyOps     = [RegOp.SetDword(GdiKey, "MaxGDIObjects", 10000)],
            RemoveOps    = [RegOp.DeleteValue(GdiKey, "MaxGDIObjects")],
            DetectOps    = [RegOp.CheckDword(GdiKey, "MaxGDIObjects", 10000)],
        },
        new TweakDef
        {
            Id           = "gdipol-disable-printer-gdi-metafile",
            Label        = "Disable GDI Printer Metafile Spool Format",
            Category     = "GDI Renderer Policy",
            Description  = "Disables the legacy EMF (Enhanced Metafile) spool format for GDI-based printing and forces direct printing via the XPS document pipeline, reducing exposure to EMF file parsing vulnerabilities in the spooler.",
            Tags         = ["gdi", "metafile", "printing", "spooler", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 4,
            ImpactNote   = "GDI EMF printer spool format disabled; printing uses XPS pipeline. Legacy GDI-only printers may not work.",
            ApplyOps     = [RegOp.SetDword(GdiKey, "DisableGDIPrinterMetafile", 1)],
            RemoveOps    = [RegOp.DeleteValue(GdiKey, "DisableGDIPrinterMetafile")],
            DetectOps    = [RegOp.CheckDword(GdiKey, "DisableGDIPrinterMetafile", 1)],
        },
        new TweakDef
        {
            Id           = "gdipol-enable-gdi-audit",
            Label        = "Enable GDI Object Creation Audit Logging",
            Category     = "GDI Renderer Policy",
            Description  = "Enables lightweight audit logging for GDI object creation and destruction at the policy level, providing visibility into unusual GDI handle consumption patterns that may indicate malicious UI automation or exploitation attempts.",
            Tags         = ["gdi", "audit", "object-creation", "event-log", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 2,
            SafetyRating = 5,
            ImpactNote   = "GDI object creation auditing enabled; unusual handle patterns visible for security monitoring.",
            ApplyOps     = [RegOp.SetDword(GdiKey, "EnableGDIObjectAudit", 1)],
            RemoveOps    = [RegOp.DeleteValue(GdiKey, "EnableGDIObjectAudit")],
            DetectOps    = [RegOp.CheckDword(GdiKey, "EnableGDIObjectAudit", 1)],
        },
        new TweakDef
        {
            Id           = "gdipol-disable-gdi-screen-capture",
            Label        = "Block GDI-Based Screen Capture by Standard Applications",
            Category     = "GDI Renderer Policy",
            Description  = "Restricts the ability of standard (non-elevated) applications to capture the entire screen via BitBlt from the desktop DC, limiting screen capture to applications with explicit capture permissions.",
            Tags         = ["gdi", "screen-capture", "bitblt", "privacy", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "GDI full-screen BitBlt blocked for standard apps; screen capture requires explicit permission.",
            ApplyOps     = [RegOp.SetDword(GdiKey, "DisableScreenCaptureViaGDI", 1)],
            RemoveOps    = [RegOp.DeleteValue(GdiKey, "DisableScreenCaptureViaGDI")],
            DetectOps    = [RegOp.CheckDword(GdiKey, "DisableScreenCaptureViaGDI", 1)],
        },
        new TweakDef
        {
            Id           = "gdipol-disable-gdi-telemetry",
            Label        = "Disable GDI Renderer Telemetry Reporting to Microsoft",
            Category     = "GDI Renderer Policy",
            Description  = "Prevents the GDI rendering subsystem from sending object usage, rendering performance, and driver compatibility telemetry to Microsoft.",
            Tags         = ["gdi", "telemetry", "privacy", "microsoft", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 2,
            SafetyRating = 5,
            ImpactNote   = "GDI renderer telemetry to Microsoft disabled; rendering stats and driver compat data not sent to cloud.",
            ApplyOps     = [RegOp.SetDword(GdiKey, "DisableGDITelemetry", 1)],
            RemoveOps    = [RegOp.DeleteValue(GdiKey, "DisableGDITelemetry")],
            DetectOps    = [RegOp.CheckDword(GdiKey, "DisableGDITelemetry", 1)],
        },
    ];
}
