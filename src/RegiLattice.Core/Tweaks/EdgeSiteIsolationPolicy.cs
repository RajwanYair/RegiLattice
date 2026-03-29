// RegiLattice.Core — Tweaks/EdgeSiteIsolationPolicy.cs
// Edge per-site process isolation, origin keying, renderer sandbox, and CORB controls — Sprint 455.
// Category: "Edge Site Isolation Policy" | Slug: edgiso
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Edge

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class EdgeSiteIsolationPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "edgiso-enable-site-isolation",
                Label = "Enable Full Site Isolation in Edge",
                Category = "Edge Site Isolation Policy",
                Description =
                    "Enables full Site Isolation in Microsoft Edge, running every site origin in a dedicated renderer process to mitigate Spectre/Meltdown cross-origin data leaks.",
                Tags = ["edge", "site-isolation", "security", "spectre", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Each site runs in isolated renderer; memory use increases but Spectre-class leaks are effectively blocked.",
                ApplyOps = [RegOp.SetDword(Key, "SitePerProcess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "SitePerProcess")],
                DetectOps = [RegOp.CheckDword(Key, "SitePerProcess", 1)],
            },
            new TweakDef
            {
                Id = "edgiso-enable-strict-origin-isolation",
                Label = "Enable Strict Origin Isolation in Edge",
                Category = "Edge Site Isolation Policy",
                Description =
                    "Enables strict origin isolation so every unique origin (scheme+host+port) runs in its own renderer process instead of grouping origins by site.",
                Tags = ["edge", "origin-isolation", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Each origin gets dedicated process; more granular isolation than site-level.",
                ApplyOps = [RegOp.SetDword(Key, "IsolateOrigins", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "IsolateOrigins")],
                DetectOps = [RegOp.CheckDword(Key, "IsolateOrigins", 1)],
            },
            new TweakDef
            {
                Id = "edgiso-enable-renderer-sandbox",
                Label = "Enable Renderer Process Sandbox in Edge",
                Category = "Edge Site Isolation Policy",
                Description =
                    "Enables the renderer process sandbox in Edge, restricting renderer processes' access to the OS to reduce the impact of renderer compromises.",
                Tags = ["edge", "sandbox", "renderer", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Renderer processes sandboxed; OS-level attacks from compromised renderer are blocked.",
                ApplyOps = [RegOp.SetDword(Key, "RendererCodeIntegrityEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RendererCodeIntegrityEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "RendererCodeIntegrityEnabled", 1)],
            },
            new TweakDef
            {
                Id = "edgiso-block-cross-origin-reads",
                Label = "Enable Cross-Origin Read Blocking (CORB) in Edge",
                Category = "Edge Site Isolation Policy",
                Description =
                    "Enables Cross-Origin Read Blocking (CORB) to prevent sensitive cross-origin responses (HTML, JSON, XML) from being readable by cross-origin scripts, mitigating Spectre side-channel attacks.",
                Tags = ["edge", "corb", "cross-origin", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Cross-origin sensitive response bodies blocked from scripts; Spectre leaks via network data mitigated.",
                ApplyOps = [RegOp.SetDword(Key, "CrossOriginReadBlocking", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "CrossOriginReadBlocking")],
                DetectOps = [RegOp.CheckDword(Key, "CrossOriginReadBlocking", 1)],
            },
            new TweakDef
            {
                Id = "edgiso-disable-shared-memory",
                Label = "Disable Cross-Process Shared Memory in Edge",
                Category = "Edge Site Isolation Policy",
                Description =
                    "Disables shared memory IPC between renderer processes and the browser process, reducing cross-process information leakage vectors in Edge.",
                Tags = ["edge", "shared-memory", "ipc", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Shared memory for renderer IPC disabled; slight performance overhead for cross-process messages.",
                ApplyOps = [RegOp.SetDword(Key, "SharedMemoryDisabled", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "SharedMemoryDisabled")],
                DetectOps = [RegOp.CheckDword(Key, "SharedMemoryDisabled", 1)],
            },
            new TweakDef
            {
                Id = "edgiso-enable-gpu-sandbox",
                Label = "Enable GPU Process Sandbox in Edge",
                Category = "Edge Site Isolation Policy",
                Description =
                    "Enables sandboxing of the Edge GPU process to restrict GPU process access to OS resources, reducing the impact of GPU driver exploits.",
                Tags = ["edge", "gpu", "sandbox", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "GPU process sandboxed; GPU driver exploit impact limited to renderer context.",
                ApplyOps = [RegOp.SetDword(Key, "GpuSandboxEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "GpuSandboxEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "GpuSandboxEnabled", 1)],
            },
            new TweakDef
            {
                Id = "edgiso-block-mixed-content",
                Label = "Block Mixed Active Content in Edge",
                Category = "Edge Site Isolation Policy",
                Description =
                    "Blocks loading of active mixed content (scripts, stylesheets from HTTP on HTTPS pages), preventing downgrade and man-in-the-middle injection on secure pages.",
                Tags = ["edge", "mixed-content", "https", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "HTTP scripts/styles blocked on HTTPS pages; legacy intranet sites with mixed content may break.",
                ApplyOps = [RegOp.SetDword(Key, "InsecureContentAllowedForUrls", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "InsecureContentAllowedForUrls")],
                DetectOps = [RegOp.CheckDword(Key, "InsecureContentAllowedForUrls", 0)],
            },
            new TweakDef
            {
                Id = "edgiso-force-https-first",
                Label = "Force HTTPS-First Mode in Edge",
                Category = "Edge Site Isolation Policy",
                Description =
                    "Forces Edge to attempt HTTPS connections before HTTP, automatically upgrading site navigation to HTTPS where supported.",
                Tags = ["edge", "https", "upgrade", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Navigation attempts HTTPS first; HTTP-only sites show warning or fail to load.",
                ApplyOps = [RegOp.SetDword(Key, "HttpsFirstModeEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "HttpsFirstModeEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "HttpsFirstModeEnabled", 1)],
            },
            new TweakDef
            {
                Id = "edgiso-enable-enhanced-tracking-protection",
                Label = "Enable Strict Tracking Prevention in Edge",
                Category = "Edge Site Isolation Policy",
                Description =
                    "Configures Edge Tracking Prevention to Strict mode, blocking known trackers and fingerprinting scripts from all sites including first-party contexts.",
                Tags = ["edge", "tracking-prevention", "privacy", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Strict tracking prevention active; some social widgets and embedded content may fail to load.",
                ApplyOps = [RegOp.SetDword(Key, "TrackingPrevention", 3)],
                RemoveOps = [RegOp.DeleteValue(Key, "TrackingPrevention")],
                DetectOps = [RegOp.CheckDword(Key, "TrackingPrevention", 3)],
            },
            new TweakDef
            {
                Id = "edgiso-disable-webrtc-leak",
                Label = "Disable WebRTC IP Address Leak in Edge",
                Category = "Edge Site Isolation Policy",
                Description =
                    "Configures WebRTC to use only public-facing IP addresses for ICE candidate generation, preventing local and VPN tunnel IP address leakage via WebRTC API.",
                Tags = ["edge", "webrtc", "ip-leak", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Local/VPN IP addresses not exposed via WebRTC; improves privacy for VPN users.",
                ApplyOps = [RegOp.SetDword(Key, "WebRtcIPHandling", 2)],
                RemoveOps = [RegOp.DeleteValue(Key, "WebRtcIPHandling")],
                DetectOps = [RegOp.CheckDword(Key, "WebRtcIPHandling", 2)],
            },
        ];
}
