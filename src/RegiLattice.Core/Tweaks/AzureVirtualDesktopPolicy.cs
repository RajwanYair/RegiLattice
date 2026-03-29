// RegiLattice.Core — Tweaks/AzureVirtualDesktopPolicy.cs
// Azure Virtual Desktop (AVD) Policy — Sprint 537.
// Configures Group Policy settings for Azure Virtual Desktop host pools,
// session hosts, and client connections. Covers session limits, RDP settings,
// and AVD-specific security hardening for enterprise virtual desktop environments.
// Category: "Azure Virtual Desktop Policy" | Slug: avd
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class AzureVirtualDesktopPolicy
{
    private const string TsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services";

    private const string AvdKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services\Azure Virtual Desktop";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "avd-enable-watermarking",
                Label = "AVD: Enable Screen Watermarking for Session Hosts",
                Category = "Azure Virtual Desktop Policy",
                Description =
                    "Sets EnableWatermarking=1 and WatermarkingHeightFactor/WidthFactor. Overlays a semi-transparent QR code on AVD session screens that encodes the user's UPN and session identifier. This watermark is user-invisible during normal work but visible in screenshots and screen captures. Watermarking is essential for data loss investigations and insider threat deterrence in environments handling sensitive or regulated data.",
                Tags = ["avd", "watermarking", "dlp", "session", "enterprise"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "Transparent watermark; no user impact. QR code is visible in screen captures which may affect screen-sharing in training.",
                ApplyOps = [RegOp.SetDword(TsKey, "EnableWatermarking", 1)],
                RemoveOps = [RegOp.DeleteValue(TsKey, "EnableWatermarking")],
                DetectOps = [RegOp.CheckDword(TsKey, "EnableWatermarking", 1)],
            },
            new TweakDef
            {
                Id = "avd-disable-clipboard-redirect",
                Label = "AVD: Disable Clipboard Redirection in Sessions",
                Category = "Azure Virtual Desktop Policy",
                Description =
                    "Sets fDisableClip=1. Blocks bidirectional clipboard redirection between the AVD session and the client device. Clipboard is a primary data exfiltration vector in VDI environments: users copy sensitive data from the session and paste it outside the controlled environment. Disabling clipboard redirection is a key DLP control for finance, healthcare, and legal VDI deployments. Some AVD workflows may require clipboard for productivity; evaluate per use-case.",
                Tags = ["avd", "clipboard", "dlp", "data-exfiltration", "enterprise"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "Prevents copy/paste between session and client. Significant productivity impact for workflows requiring copy of data from session.",
                ApplyOps = [RegOp.SetDword(TsKey, "fDisableClip", 1)],
                RemoveOps = [RegOp.DeleteValue(TsKey, "fDisableClip")],
                DetectOps = [RegOp.CheckDword(TsKey, "fDisableClip", 1)],
            },
            new TweakDef
            {
                Id = "avd-disable-drive-redirect",
                Label = "AVD: Disable Drive Redirection in Sessions",
                Category = "Azure Virtual Desktop Policy",
                Description =
                    "Sets fDisableCdm=1. Prevents client-side drives (USB sticks, local hard drives, network shares) from being mounted in AVD sessions. Drive redirection is exploited for both data exfiltration (copying from session to external media) and malware delivery (running executables from a USB drive in the session). Removing drive redirection is a standard DLP and malware containment control in supervised VDI environments.",
                Tags = ["avd", "drive-redirect", "usb", "dlp", "malware"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Removes session access to client drives. Users cannot access USB media or local files from within the AVD session.",
                ApplyOps = [RegOp.SetDword(TsKey, "fDisableCdm", 1)],
                RemoveOps = [RegOp.DeleteValue(TsKey, "fDisableCdm")],
                DetectOps = [RegOp.CheckDword(TsKey, "fDisableCdm", 1)],
            },
            new TweakDef
            {
                Id = "avd-idle-disconnect-30min",
                Label = "AVD: Disconnect Idle Sessions After 30 Minutes",
                Category = "Azure Virtual Desktop Policy",
                Description =
                    "Sets MaxIdleTime=1800000 (30 minutes in milliseconds). Automatically disconnects AVD sessions that have been idle for 30 minutes. Idle sessions consume Azure compute costs and create an unattended-session security risk where unlocked sessions could be accessed by physical access to an unattended client. Auto-disconnect after 30 minutes is the standard enterprise baseline for cost and security management of AVD session hosts.",
                Tags = ["avd", "idle", "session-management", "cost", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Users with unsaved work may lose state if idle for 30 minutes. Pair with auto-save policies.",
                ApplyOps = [RegOp.SetDword(TsKey, "MaxIdleTime", 1800000)],
                RemoveOps = [RegOp.DeleteValue(TsKey, "MaxIdleTime")],
                DetectOps = [RegOp.CheckDword(TsKey, "MaxIdleTime", 1800000)],
            },
            new TweakDef
            {
                Id = "avd-enable-screen-capture-protection",
                Label = "AVD: Enable Screen Capture Protection (DRM-Level)",
                Category = "Azure Virtual Desktop Policy",
                Description =
                    "Sets fEnableScreenCaptureProtect=1. Enables AVD screen capture protection, which uses DRM-style OS hooks to prevent the AVD session content from being captured by screenshots, screen recording software, or GPU frame capture tools on the client side. The session content appears as a black region in any screen capture. Essential for protecting classified information displays, financial data, and healthcare PHI from accidental or intentional screen capture exfiltration.",
                Tags = ["avd", "screen-capture", "dlp", "drm", "enterprise"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote =
                    "Session content is blackened in all screen captures on the client. Training recordings and accessibility tools that capture the screen will not see session content.",
                ApplyOps = [RegOp.SetDword(TsKey, "fEnableScreenCaptureProtect", 1)],
                RemoveOps = [RegOp.DeleteValue(TsKey, "fEnableScreenCaptureProtect")],
                DetectOps = [RegOp.CheckDword(TsKey, "fEnableScreenCaptureProtect", 1)],
            },
            new TweakDef
            {
                Id = "avd-enable-private-mode",
                Label = "AVD: Enable Private Mode for Session Hosts",
                Category = "Azure Virtual Desktop Policy",
                Description =
                    "Sets EnablePrivateMode=1. Activates AVD Private Mode which restricts session actions to reduce data leakage risk: disables local clipboard, file transfers, printing to local printers, and local drive access in a single policy. Private Mode is designed for shared/kiosk session hosts in sensitive environments where multiple users share the same session host profile. Equivalent to enabling fDisableClip + fDisableCdm + printer restrictions together.",
                Tags = ["avd", "private-mode", "kiosk", "dlp", "enterprise"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Restricts all peripheral redirection. Not suitable for productivity use-cases requiring local file access or printing.",
                ApplyOps = [RegOp.SetDword(TsKey, "EnablePrivateMode", 1)],
                RemoveOps = [RegOp.DeleteValue(TsKey, "EnablePrivateMode")],
                DetectOps = [RegOp.CheckDword(TsKey, "EnablePrivateMode", 1)],
            },
            new TweakDef
            {
                Id = "avd-set-rdp-security-layer",
                Label = "AVD: Enforce TLS 1.2+ for RDP Transport Layer",
                Category = "Azure Virtual Desktop Policy",
                Description =
                    "Sets SecurityLayer=2 (TLS). Forces the Remote Desktop Protocol transport layer to use SSL/TLS 1.2 or later for all connections to AVD session hosts. Value 0 = RDP legacy (cleartext-vulnerable), value 1 = negotiate (downgrade possible), value 2 = TLS required. In Azure, the network path is encrypted at the Azure backbone level; however, enforcing TLS at the RDP layer provides defence-in-depth and satisfies compliance requirements for encrypted-in-transit data.",
                Tags = ["avd", "rdp", "tls", "encryption", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Requires TLS; breaks connections from very old RDP clients that cannot negotiate TLS. All modern clients support TLS.",
                ApplyOps = [RegOp.SetDword(TsKey, "SecurityLayer", 2)],
                RemoveOps = [RegOp.DeleteValue(TsKey, "SecurityLayer")],
                DetectOps = [RegOp.CheckDword(TsKey, "SecurityLayer", 2)],
            },
            new TweakDef
            {
                Id = "avd-require-nla",
                Label = "AVD: Require Network Level Authentication for Sessions",
                Category = "Azure Virtual Desktop Policy",
                Description =
                    "Sets UserAuthentication=1. Requires Network Level Authentication (NLA) before establishing an RDP session to the AVD host. NLA authenticates the user before allocating session resources, preventing unauthenticated users from reaching the Windows login screen and mounting DoS attacks by opening many half-authenticated sessions. AVD natively enforces Azure AD authentication; this setting adds an additional OS-level NLA gate.",
                Tags = ["avd", "nla", "authentication", "rdp", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Requires NLA-capable RDP clients (all modern clients support NLA). Very old RDP clients may not connect.",
                ApplyOps = [RegOp.SetDword(TsKey, "UserAuthentication", 1)],
                RemoveOps = [RegOp.DeleteValue(TsKey, "UserAuthentication")],
                DetectOps = [RegOp.CheckDword(TsKey, "UserAuthentication", 1)],
            },
            new TweakDef
            {
                Id = "avd-limit-session-connections",
                Label = "AVD: Limit Users to a Single Active Session",
                Category = "Azure Virtual Desktop Policy",
                Description =
                    "Sets fSingleSessionPerUser=1. Restricts each user to a single simultaneous AVD session across all host pool machines. Without this limit, a user can open multiple sessions (e.g., from multiple devices simultaneously), multiplying their compute cost and creating multiple unmanaged session states. Single-session enforcement reduces Azure compute costs proportionally to the number of multi-device users and simplifies session management.",
                Tags = ["avd", "session-limit", "cost", "enterprise"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Single session per user. Prevents opening the same session from multiple client devices simultaneously.",
                ApplyOps = [RegOp.SetDword(TsKey, "fSingleSessionPerUser", 1)],
                RemoveOps = [RegOp.DeleteValue(TsKey, "fSingleSessionPerUser")],
                DetectOps = [RegOp.CheckDword(TsKey, "fSingleSessionPerUser", 1)],
            },
            new TweakDef
            {
                Id = "avd-enable-shortpath-udp",
                Label = "AVD: Enable UDP ShortPath for Optimized Network Performance",
                Category = "Azure Virtual Desktop Policy",
                Description =
                    "Sets fClientShortPathEndpointEnabled=1. Activates Azure Virtual Desktop UDP ShortPath, which establishes direct UDP-based transport between the AVD client and session host instead of routing all traffic through the Azure gateway TCP relay. UDP ShortPath reduces round-trip latency from 50–200 ms (TCP relay) to near-direct network latency, dramatically improving display responsiveness and Teams/audio quality in AVD sessions.",
                Tags = ["avd", "shortpath", "udp", "latency", "performance"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Requires UDP 65330 outbound from client to be open on the firewall. Check network policy before enabling.",
                ApplyOps = [RegOp.SetDword(TsKey, "fClientShortPathEndpointEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(TsKey, "fClientShortPathEndpointEnabled")],
                DetectOps = [RegOp.CheckDword(TsKey, "fClientShortPathEndpointEnabled", 1)],
            },
        ];
}
