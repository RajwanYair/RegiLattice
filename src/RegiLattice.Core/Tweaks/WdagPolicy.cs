// RegiLattice.Core — Tweaks/WdagPolicy.cs
// Windows Defender Application Guard (WDAG / AppHVSI) Group Policy settings.
// Slug: "wdagpol" — distinct from WdacCodeIntegrity.cs (ASR rules) and Defender.cs (global AV).
// Registry path: HKLM\SOFTWARE\Policies\Microsoft\AppHVSI  (Group Policy managed WDAG).
// Requires Windows 10 Enterprise / Education (Build 1709+) or Windows 11 for WDAG.

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WdagPolicy
{
    private const string AppHvsi = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\AppHVSI";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "wdagpol-enable-application-guard",
            Label = "Enable Windows Defender Application Guard for Edge",
            Category = "Application Guard Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            Tags = ["wdag", "application guard", "edge", "security", "isolation", "enterprise"],
            Description =
                "Enables Windows Defender Application Guard (WDAG) for Microsoft Edge via Group Policy. "
                + "Untrusted websites open in a Hyper-V isolated container, isolating the host from "
                + "browser-based exploits. AllowAppHVSI_ProviderSet = 1. "
                + "Default: disabled. Requires Virtualization-Based Security (VBS) and Hyper-V.",
            MinBuild = 16299,
            ApplyOps   = [RegOp.SetDword(AppHvsi, "AllowAppHVSI_ProviderSet", 1)],
            RemoveOps  = [RegOp.SetDword(AppHvsi, "AllowAppHVSI_ProviderSet", 0)],
            DetectOps  = [RegOp.CheckDword(AppHvsi, "AllowAppHVSI_ProviderSet", 1)],
        },
        new TweakDef
        {
            Id = "wdagpol-disable-clipboard-to-container",
            Label = "WDAG: Block Clipboard from Host to Container",
            Category = "Application Guard Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Tags = ["wdag", "clipboard", "isolation", "security", "enterprise"],
            Description =
                "Restricts clipboard operations so content cannot be pasted from the host into the "
                + "Application Guard container. AppHVSIClipboardSettings = 1 (copy from container only). "
                + "Prevents credential theft and data exfiltration via clipboard paste into untrusted sites. "
                + "Default: bidirectional clipboard (0). Hardened value: 1.",
            MinBuild = 16299,
            ApplyOps   = [RegOp.SetDword(AppHvsi, "AppHVSIClipboardSettings", 1)],
            RemoveOps  = [RegOp.SetDword(AppHvsi, "AppHVSIClipboardSettings", 0)],
            DetectOps  = [RegOp.CheckDword(AppHvsi, "AppHVSIClipboardSettings", 1)],
        },
        new TweakDef
        {
            Id = "wdagpol-disable-clipboard-to-host",
            Label = "WDAG: Block Clipboard from Container to Host",
            Category = "Application Guard Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Tags = ["wdag", "clipboard", "isolation", "security", "enterprise"],
            Description =
                "Restricts clipboard so content inside the Application Guard container cannot be "
                + "pasted to the host. AppHVSIClipboardSettings = 2 (copy to container only). "
                + "Prevents malicious container content from reaching host applications. "
                + "Combine with wdagpol-disable-clipboard-to-container for full isolation (value 3).",
            MinBuild = 16299,
            ApplyOps   = [RegOp.SetDword(AppHvsi, "AppHVSIClipboardSettings", 2)],
            RemoveOps  = [RegOp.SetDword(AppHvsi, "AppHVSIClipboardSettings", 0)],
            DetectOps  = [RegOp.CheckDword(AppHvsi, "AppHVSIClipboardSettings", 2)],
        },
        new TweakDef
        {
            Id = "wdagpol-disable-printing",
            Label = "WDAG: Disable Printing from Application Guard Container",
            Category = "Application Guard Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["wdag", "printing", "isolation", "security", "enterprise"],
            Description =
                "Disables all printing from within the Application Guard container. "
                + "AppHVSIPrintingSettings = 0 (no printing). "
                + "Prevents document exfiltration via printing from untrusted container sessions. "
                + "Default: printing enabled (network, PDF, XPS, local printers all allowed).",
            MinBuild = 16299,
            ApplyOps   = [RegOp.SetDword(AppHvsi, "AppHVSIPrintingSettings", 0)],
            RemoveOps  = [RegOp.DeleteValue(AppHvsi, "AppHVSIPrintingSettings")],
            DetectOps  = [RegOp.CheckDword(AppHvsi, "AppHVSIPrintingSettings", 0)],
        },
        new TweakDef
        {
            Id = "wdagpol-disable-data-persistence",
            Label = "WDAG: Disable Container Data Persistence",
            Category = "Application Guard Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Tags = ["wdag", "persistence", "isolation", "security", "privacy", "enterprise"],
            Description =
                "Disables data persistence in the Application Guard container. "
                + "AllowPersistence = 0. Browser data (cookies, history, passwords, downloads) is "
                + "deleted when the container session ends. "
                + "Default: persistence disabled. Some orgs enable it for usability — hardened value is 0.",
            MinBuild = 16299,
            ApplyOps   = [RegOp.SetDword(AppHvsi, "AllowPersistence", 0)],
            RemoveOps  = [RegOp.DeleteValue(AppHvsi, "AllowPersistence")],
            DetectOps  = [RegOp.CheckDword(AppHvsi, "AllowPersistence", 0)],
        },
        new TweakDef
        {
            Id = "wdagpol-disable-camera-microphone",
            Label = "WDAG: Disable Camera and Microphone in Container",
            Category = "Application Guard Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Tags = ["wdag", "camera", "microphone", "privacy", "isolation", "security", "enterprise"],
            Description =
                "Prevents the Application Guard container from accessing the camera and microphone. "
                + "AllowCameraMicrophoneRedirection = 0. "
                + "Stops untrusted browser sessions from recording the user without consent. "
                + "Default: access disabled. Must be explicitly enabled if required.",
            MinBuild = 18362,
            ApplyOps   = [RegOp.SetDword(AppHvsi, "AllowCameraMicrophoneRedirection", 0)],
            RemoveOps  = [RegOp.DeleteValue(AppHvsi, "AllowCameraMicrophoneRedirection")],
            DetectOps  = [RegOp.CheckDword(AppHvsi, "AllowCameraMicrophoneRedirection", 0)],
        },
        new TweakDef
        {
            Id = "wdagpol-disable-virtual-gpu",
            Label = "WDAG: Disable Virtual GPU in Container",
            Category = "Application Guard Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["wdag", "gpu", "isolation", "security", "enterprise"],
            Description =
                "Disables hardware-accelerated rendering in the Application Guard container. "
                + "AllowVirtualGPU = 0. Reduces GPU attack surface — a compromised vGPU driver "
                + "could potentially escape the container. Rendering falls back to software. "
                + "Default: hardware GPU disabled for maximum isolation.",
            MinBuild = 16299,
            ApplyOps   = [RegOp.SetDword(AppHvsi, "AllowVirtualGPU", 0)],
            RemoveOps  = [RegOp.DeleteValue(AppHvsi, "AllowVirtualGPU")],
            DetectOps  = [RegOp.CheckDword(AppHvsi, "AllowVirtualGPU", 0)],
        },
        new TweakDef
        {
            Id = "wdagpol-enable-audit-mode",
            Label = "WDAG: Enable Audit Mode for Application Guard",
            Category = "Application Guard Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["wdag", "audit", "logging", "security", "enterprise", "siem"],
            Description =
                "Enables audit logging for Application Guard container events. "
                + "AuditApplicationGuard = 1. Events are logged to the Windows event log "
                + "(Microsoft-Windows-Windows Defender Application Guard/Operational). "
                + "Useful for SIEM integration and security monitoring of container activity.",
            MinBuild = 16299,
            ApplyOps   = [RegOp.SetDword(AppHvsi, "AuditApplicationGuard", 1)],
            RemoveOps  = [RegOp.SetDword(AppHvsi, "AuditApplicationGuard", 0)],
            DetectOps  = [RegOp.CheckDword(AppHvsi, "AuditApplicationGuard", 1)],
        },
        new TweakDef
        {
            Id = "wdagpol-disable-download-to-host",
            Label = "WDAG: Block Saving Container Downloads to Host",
            Category = "Application Guard Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Tags = ["wdag", "downloads", "isolation", "security", "enterprise"],
            Description =
                "Prevents files downloaded in the Application Guard container from being saved "
                + "to the host filesystem. SaveFilesToHost = 0. "
                + "Stops container-side malware payloads from escaping isolation via the Downloads folder. "
                + "Default: download-to-host disabled for maximum isolation.",
            MinBuild = 16299,
            ApplyOps   = [RegOp.SetDword(AppHvsi, "SaveFilesToHost", 0)],
            RemoveOps  = [RegOp.DeleteValue(AppHvsi, "SaveFilesToHost")],
            DetectOps  = [RegOp.CheckDword(AppHvsi, "SaveFilesToHost", 0)],
        },
        new TweakDef
        {
            Id = "wdagpol-configure-network-isolation",
            Label = "WDAG: Enable Network Isolation for Container",
            Category = "Application Guard Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            Tags = ["wdag", "network", "isolation", "security", "enterprise"],
            Description =
                "Sets WDAG to use an isolated network namespace for the container. "
                + "NetworkIsolation = 1. The container gets a separate virtual NIC that cannot "
                + "reach internal corporate resources, only the public internet. "
                + "Prevents lateral movement from a compromised browser session to internal servers.",
            MinBuild = 16299,
            ApplyOps   = [RegOp.SetDword(AppHvsi, "NetworkIsolation", 1)],
            RemoveOps  = [RegOp.SetDword(AppHvsi, "NetworkIsolation", 0)],
            DetectOps  = [RegOp.CheckDword(AppHvsi, "NetworkIsolation", 1)],
        },
    ];
}
