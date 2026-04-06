namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class SecurityWDAG
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\AppVirtualization\ApplicationGuard";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "wdag-enable-managed-mode",
                Label = "Enable WDAG in Managed Enterprise Mode",
                Category = "Security — Application Guard",
                Description =
                    "Configures Windows Defender Application Guard (WDAG) to run in managed Enterprise mode. "
                    + "Provides hardware-isolated browsing in Microsoft Edge using Hyper-V. "
                    + "Default: disabled. Recommended: enabled for organisations.",
                Tags = ["wdag", "app-guard", "enterprise", "isolation", "edge", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ApplyOps = [RegOp.SetDword(Key, "AllowWindowsDefenderApplicationGuard", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowWindowsDefenderApplicationGuard")],
                DetectOps = [RegOp.CheckDword(Key, "AllowWindowsDefenderApplicationGuard", 1)],
            },
            new TweakDef
            {
                Id = "wdag-disable-persistence",
                Label = "Disable WDAG Container Data Persistence",
                Category = "Security — Application Guard",
                Description =
                    "Prevents Application Guard from retaining browsing data between sessions. "
                    + "Each container session starts clean, eliminating cross-session data leakage. "
                    + "Default: persistence enabled. Recommended: disabled.",
                Tags = ["wdag", "persistence", "isolation", "sessions", "browsing-data"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(Key, "AllowPersistence", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowPersistence")],
                DetectOps = [RegOp.CheckDword(Key, "AllowPersistence", 0)],
            },
            new TweakDef
            {
                Id = "wdag-disable-virtual-gpu",
                Label = "Disable WDAG Hardware GPU Acceleration",
                Category = "Security — Application Guard",
                Description =
                    "Forces App Guard to use software rendering only. "
                    + "Virtual GPU access creates an extra attack surface and may expose graphics driver vulnerabilities inside the container. "
                    + "Default: hardware acceleration enabled. Recommended: disabled.",
                Tags = ["wdag", "gpu", "graphics", "isolation", "attack-surface"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(Key, "AllowVirtualGPU", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowVirtualGPU")],
                DetectOps = [RegOp.CheckDword(Key, "AllowVirtualGPU", 0)],
            },
            new TweakDef
            {
                Id = "wdag-disable-clipboard",
                Label = "Block All Clipboard Operations in WDAG",
                Category = "Security — Application Guard",
                Description =
                    "Disables all clipboard data transfer between the Application Guard container and the host. "
                    + "Value 0 blocks bidirectional clipboard; higher values permit specific directions. "
                    + "Default: some clipboard allowed. Recommended: fully blocked.",
                Tags = ["wdag", "clipboard", "isolation", "data-exfiltration", "copy-paste"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(Key, "ClipboardSettings", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "ClipboardSettings")],
                DetectOps = [RegOp.CheckDword(Key, "ClipboardSettings", 0)],
            },
            new TweakDef
            {
                Id = "wdag-disable-printing",
                Label = "Disable Printing from WDAG Container",
                Category = "Security — Application Guard",
                Description =
                    "Disables all printing from within the Application Guard container. "
                    + "Printing could expose sensitive container content to host or network printers. "
                    + "Default: some printing allowed. Recommended: fully disabled.",
                Tags = ["wdag", "printing", "isolation", "data-exfiltration", "printers"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(Key, "PrintingSettings", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "PrintingSettings")],
                DetectOps = [RegOp.CheckDword(Key, "PrintingSettings", 0)],
            },
            new TweakDef
            {
                Id = "wdag-disable-camera-mic",
                Label = "Block Camera and Microphone in WDAG",
                Category = "Security — Application Guard",
                Description =
                    "Prevents the Application Guard container from accessing the camera and microphone. "
                    + "Ensures container-isolated browsing cannot capture audio or video from the device. "
                    + "Default: camera and mic may be available. Recommended: blocked.",
                Tags = ["wdag", "camera", "microphone", "privacy", "isolation"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(Key, "AllowCameraMicrophoneRedirection", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowCameraMicrophoneRedirection")],
                DetectOps = [RegOp.CheckDword(Key, "AllowCameraMicrophoneRedirection", 0)],
            },
            new TweakDef
            {
                Id = "wdag-enable-audit",
                Label = "Enable WDAG Audit Event Logging",
                Category = "Security — Application Guard",
                Description =
                    "Enables Windows Event Log entries for Application Guard activations, container actions, and policy violations. "
                    + "Required for SIEM integration and container-level security monitoring. "
                    + "Default: auditing disabled. Recommended: enabled.",
                Tags = ["wdag", "auditing", "event-log", "siem", "monitoring"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(Key, "AuditApplicationGuard", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditApplicationGuard")],
                DetectOps = [RegOp.CheckDword(Key, "AuditApplicationGuard", 1)],
            },
            new TweakDef
            {
                Id = "wdag-disable-file-save-to-host",
                Label = "Block Saving Container Files to Host",
                Category = "Security — Application Guard",
                Description =
                    "Prevents files downloaded or opened inside the Application Guard container from being saved to the host filesystem. "
                    + "Reduces risk of malicious file extraction from the isolated container. "
                    + "Default: saving may be allowed. Recommended: blocked.",
                Tags = ["wdag", "file-download", "isolation", "data-exfiltration", "host"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(Key, "SaveFilesToHost", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "SaveFilesToHost")],
                DetectOps = [RegOp.CheckDword(Key, "SaveFilesToHost", 0)],
            },
            new TweakDef
            {
                Id = "wdag-disable-hardware-acceleration",
                Label = "Disable Hardware Acceleration in WDAG Container",
                Category = "Security — Application Guard",
                Description =
                    "Configures WDAG to use software-only rendering, eliminating hardware acceleration. "
                    + "Reduces attack surface from graphics driver vulnerabilities leaking into the container. "
                    + "Default: hardware acceleration on. Recommended: off for maximum isolation.",
                Tags = ["wdag", "hardware-acceleration", "isolation", "security", "rendering"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(Key, "HardwareAcceleration", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "HardwareAcceleration")],
                DetectOps = [RegOp.CheckDword(Key, "HardwareAcceleration", 0)],
            },
            new TweakDef
            {
                Id = "wdag-clear-data-on-disable",
                Label = "Clear WDAG Container Data on Policy Disable",
                Category = "Security — Application Guard",
                Description =
                    "Forces all Application Guard container data to be wiped when WDAG is disabled or reconfigured. "
                    + "Prevents residual session data from persisting on the host after policy changes. "
                    + "Default: data retained. Recommended: cleared.",
                Tags = ["wdag", "data-retention", "cleanup", "isolation", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(Key, "RetainDataOnWipeRequest", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "RetainDataOnWipeRequest")],
                DetectOps = [RegOp.CheckDword(Key, "RetainDataOnWipeRequest", 0)],
            },
        ];
}
