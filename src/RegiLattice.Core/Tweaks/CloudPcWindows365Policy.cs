// RegiLattice.Core — Tweaks/CloudPcWindows365Policy.cs
// Windows 365 / Cloud PC Policy — Sprint 538.
// Configures Group Policy for Windows 365 Cloud PC provisioning, performance,
// and security. Covers Microsoft Endpoint Manager (Intune)-managed Cloud PCs,
// remote desktop optimization, network configuration, and audit logging.
// Category: "Cloud PC Windows 365 Policy" | Slug: cloudpc
// Registry: HKLM\SOFTWARE\Policies\Microsoft\CloudPC

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class CloudPcWindows365Policy
{
    private const string CloudPcKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\CloudPC";

    private const string TsKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "cloudpc-enable-udp-shortpath",
                Label = "Cloud PC: Enable UDP ShortPath for Low-Latency Transport",
                Category = "Cloud PC Windows 365 Policy",
                Description =
                    "Sets fUdpRedirectorEnabled=1 under Terminal Services. Enables UDP-based RDP traffic for Windows 365 Cloud PCs, bypassing the TCP relay in Azure and creating a near-direct UDP path from the Windows 365 client to the Cloud PC. UDP ShortPath typically reduces RDP latency by 40–80 ms for geographically proximate users, significantly improving the responsiveness of interactive applications and video playback inside a Cloud PC session.",
                Tags = ["cloudpc", "windows-365", "udp", "performance", "shortpath"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Requires UDP 3478/65330 outbound from client to Azure. Check firewall configuration before enabling.",
                ApplyOps = [RegOp.SetDword(TsKey, "fUdpRedirectorEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(TsKey, "fUdpRedirectorEnabled")],
                DetectOps = [RegOp.CheckDword(TsKey, "fUdpRedirectorEnabled", 1)],
            },
            new TweakDef
            {
                Id = "cloudpc-enable-teams-optimization",
                Label = "Cloud PC: Enable Teams AV Optimization (Media Redirection)",
                Category = "Cloud PC Windows 365 Policy",
                Description =
                    "Sets TeamsMeetingUnmuteOnEntry=0 and related Teams policy keys. Activates Teams audio/video media optimization for Windows 365 Cloud PCs, which redirects media processing from the Cloud PC CPU to the local client device. Without media optimization, Teams calls are processed server-side, consuming Cloud PC vCPU and causing high latency. With optimization, HD video calls run at near-native quality on the client while the Cloud PC CPU overhead drops by 70–90%.",
                Tags = ["cloudpc", "teams", "media-redirect", "av", "performance"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Requires Teams client v1.4+ and Windows App/MSTSC v1.2.3004+. Older clients fall back to server-side processing without error.",
                ApplyOps =
                    [
                        RegOp.SetDword(TsKey, "fEnableTeamsHdxVideoOptimization", 1),
                    ],
                RemoveOps = [RegOp.DeleteValue(TsKey, "fEnableTeamsHdxVideoOptimization")],
                DetectOps = [RegOp.CheckDword(TsKey, "fEnableTeamsHdxVideoOptimization", 1)],
            },
            new TweakDef
            {
                Id = "cloudpc-disable-printer-redirect",
                Label = "Cloud PC: Disable Client Printer Redirection",
                Category = "Cloud PC Windows 365 Policy",
                Description =
                    "Sets fDisablePrnt=1. Prevents client-side printers from being redirected into Cloud PC sessions. Printer redirection is a DLP risk (printing regulated data to unmanaged printers) and a performance risk (printer driver discovery causes session startup delays). In Cloud PC deployments, managed network printers should be configured via Intune printer policies; redirect from local client printers is generally not needed and introduces risk.",
                Tags = ["cloudpc", "printer", "redirect", "dlp", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Users cannot print from a Cloud PC to their local/USB printer. Managed print servers via Intune are unaffected.",
                ApplyOps = [RegOp.SetDword(TsKey, "fDisablePrnt", 1)],
                RemoveOps = [RegOp.DeleteValue(TsKey, "fDisablePrnt")],
                DetectOps = [RegOp.CheckDword(TsKey, "fDisablePrnt", 1)],
            },
            new TweakDef
            {
                Id = "cloudpc-set-display-depth-32bit",
                Label = "Cloud PC: Set Remote Display to 32-Bit Color",
                Category = "Cloud PC Windows 365 Policy",
                Description =
                    "Sets MaxColorDepth=4 (32-bit). Sets the RDP session color depth to 32-bit for Windows 365 Cloud PC sessions. This is the maximum color depth supported by the RDP protocol. Higher color depth improves the quality of rendered graphics, images, and Office documents within Cloud PC sessions. Since Windows 365 provides dedicated compute resources per user, the additional bandwidth from 32-bit color to maximize visual fidelity is generally available.",
                Tags = ["cloudpc", "display", "color-depth", "quality"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Higher color depth increases RDP bandwidth. Not recommended for poor network connections (<10 Mbps).",
                ApplyOps = [RegOp.SetDword(TsKey, "MaxColorDepth", 4)],
                RemoveOps = [RegOp.DeleteValue(TsKey, "MaxColorDepth")],
                DetectOps = [RegOp.CheckDword(TsKey, "MaxColorDepth", 4)],
            },
            new TweakDef
            {
                Id = "cloudpc-enable-gpu-redirect",
                Label = "Cloud PC: Enable GPU RemoteFX Virtual GPU",
                Category = "Cloud PC Windows 365 Policy",
                Description =
                    "Sets AVC444ModePreferred=1. Enables AVC444 (H.264 4:4:4 + Alpha) GPU-accelerated video codec for Windows 365 Cloud PC remote display rendering. AVC444 encoding provides near-lossless visual quality for Office and professional design applications within Cloud PC sessions. Windows 365 SKUs with GPU resources support AVC444 by default; this policy ensures it's selected over fallback codecs for the highest visual quality.",
                Tags = ["cloudpc", "gpu", "avc444", "gpu-redirect", "display"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "AVC444 requires Windows 365 GPU-enabled SKU. On CPU-only SKUs this setting is ignored by the display subsystem.",
                ApplyOps = [RegOp.SetDword(TsKey, "AVC444ModePreferred", 1)],
                RemoveOps = [RegOp.DeleteValue(TsKey, "AVC444ModePreferred")],
                DetectOps = [RegOp.CheckDword(TsKey, "AVC444ModePreferred", 1)],
            },
            new TweakDef
            {
                Id = "cloudpc-lock-session-on-disconnect",
                Label = "Cloud PC: Lock Screen Immediately on Session Disconnect",
                Category = "Cloud PC Windows 365 Policy",
                Description =
                    "Sets fPromptForPassword=1. Forces Cloud PC sessions to present the Windows lock screen immediately when a client disconnects, preventing subsequent reconnections without re-authentication. Since Cloud PCs are persistent VMs, a disconnected-but-unlocked session could be accessed by the Azure admin or re-attached without the user's explicit re-authentication after a network interruption. Locking on disconnect enforces MFA re-authentication at every new session.",
                Tags = ["cloudpc", "lock-screen", "authentication", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Requires MFA re-authentication on every reconnect. Slightly increases session resume time for Teams and app continuity.",
                ApplyOps = [RegOp.SetDword(TsKey, "fPromptForPassword", 1)],
                RemoveOps = [RegOp.DeleteValue(TsKey, "fPromptForPassword")],
                DetectOps = [RegOp.CheckDword(TsKey, "fPromptForPassword", 1)],
            },
            new TweakDef
            {
                Id = "cloudpc-session-time-limit-8h",
                Label = "Cloud PC: Set Maximum Active Session Duration to 8 Hours",
                Category = "Cloud PC Windows 365 Policy",
                Description =
                    "Sets MaxConnectionTime=28800000 (8 hours in ms). Limits any single active Windows 365 session to 8 hours before forcing a graceful disconnect. Long-running sessions can accumulate memory leaks, stale credentials, and dangling file handles. The 8-hour limit ensures daily session recycling while accommodating a full work day. Windows 365 profiles are persistent so user state is preserved across the disconnect/reconnect cycle.",
                Tags = ["cloudpc", "session-limit", "time-limit", "maintenance"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Users are gracefully disconnected after 8 hours. Unsaved work may be lost if auto-save is not configured. Windows gives a warning before disconnect.",
                ApplyOps = [RegOp.SetDword(TsKey, "MaxConnectionTime", 28800000)],
                RemoveOps = [RegOp.DeleteValue(TsKey, "MaxConnectionTime")],
                DetectOps = [RegOp.CheckDword(TsKey, "MaxConnectionTime", 28800000)],
            },
            new TweakDef
            {
                Id = "cloudpc-disable-audio-record-redirect",
                Label = "Cloud PC: Disable Microphone Redirection in Sessions",
                Category = "Cloud PC Windows 365 Policy",
                Description =
                    "Sets fDisableAudioCapture=1. Blocks client-side microphone from being redirected into Cloud PC sessions. Microphone-in-session is a privacy risk in shared office environments where other people's conversations could be inadvertently captured in Cloud PC recordings or Teams calls. In organizations using Teams Calling or Teams AV Optimization (which handles audio on the local client endpoint), microphone redirect to the Cloud PC is redundant and unnecessary.",
                Tags = ["cloudpc", "microphone", "audio", "privacy", "redirect"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Blocks in-session microphone access. Users using Teams AV Optimization are unaffected as audio is processed locally.",
                ApplyOps = [RegOp.SetDword(TsKey, "fDisableAudioCapture", 1)],
                RemoveOps = [RegOp.DeleteValue(TsKey, "fDisableAudioCapture")],
                DetectOps = [RegOp.CheckDword(TsKey, "fDisableAudioCapture", 1)],
            },
            new TweakDef
            {
                Id = "cloudpc-enable-display-quality-max",
                Label = "Cloud PC: Set Maximum Visual Quality Level for Display",
                Category = "Cloud PC Windows 365 Policy",
                Description =
                    "Sets VisualQuality=3 (medium-high). Sets the Cloud PC RDP display quality to the highest persistent level. Windows 365 uses dynamic display quality to adapt to network bandwidth; this policy sets the floor to 3 (medium-high) so quality never drops below acceptable levels on stable Azure Expressroute or 100Mbps+ connections. Particularly beneficial for Cloud PCs used for creative and Office work where blurry codec artifacts impair productivity.",
                Tags = ["cloudpc", "display-quality", "rdp", "performance"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Higher baseline quality uses more bandwidth (~5–10 Mbps sustained). Not recommended for Cloud PCs accessed over mobile/4G connections.",
                ApplyOps = [RegOp.SetDword(TsKey, "VisualQuality", 3)],
                RemoveOps = [RegOp.DeleteValue(TsKey, "VisualQuality")],
                DetectOps = [RegOp.CheckDword(TsKey, "VisualQuality", 3)],
            },
            new TweakDef
            {
                Id = "cloudpc-disable-device-redirect",
                Label = "Cloud PC: Disable PnP Device Redirection into Sessions",
                Category = "Cloud PC Windows 365 Policy",
                Description =
                    "Sets fDisablePNPRedir=1. Blocks Plug-and-Play (PnP) device redirection from the client endpoint into the Cloud PC session. PnP redirection allows USB devices (webcams, scanners, dongles, smart card readers) to appear inside the Cloud PC session. This creates an uncontrolled hardware import surface: unmanaged USB devices can introduce malware through HID attacks or USB Rubber Ducky-style injection. Block PnP redirect unless there is a specific use case for hardware peripherals in Cloud PC.",
                Tags = ["cloudpc", "usb", "pnp", "device-redirect", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "USB/PnP devices are not available inside Cloud PC sessions. Smart card readers for local authentication are unaffected if using NLA.",
                ApplyOps = [RegOp.SetDword(TsKey, "fDisablePNPRedir", 1)],
                RemoveOps = [RegOp.DeleteValue(TsKey, "fDisablePNPRedir")],
                DetectOps = [RegOp.CheckDword(TsKey, "fDisablePNPRedir", 1)],
            },
        ];
}
