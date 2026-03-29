// RegiLattice.Core — Tweaks/EdgeAppGuardPolicy.cs
// Microsoft Edge Application Guard isolation, file transfer, clipboard, and printing controls — Sprint 453.
// Category: "Edge App Guard Policy" | Slug: eaguard
// Registry: HKLM\SOFTWARE\Policies\Microsoft\AppHVSI

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class EdgeAppGuardPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\AppHVSI";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "eaguard-enable-app-guard",
                Label = "Enable Application Guard for Edge",
                Category = "Edge App Guard Policy",
                Description =
                    "Enables Microsoft Defender Application Guard for Microsoft Edge, isolating untrusted web sessions in a hardware-based Hyper-V container.",
                Tags = ["edge", "app-guard", "isolation", "hyper-v", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Untrusted sites opened in isolated Hyper-V containers; requires Hyper-V and Windows Pro/Enterprise.",
                ApplyOps = [RegOp.SetDword(Key, "AllowAppHVSI_ProviderSet", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowAppHVSI_ProviderSet")],
                DetectOps = [RegOp.CheckDword(Key, "AllowAppHVSI_ProviderSet", 1)],
            },
            new TweakDef
            {
                Id = "eaguard-block-clipboard-host-to-container",
                Label = "Block Clipboard Copy Host → App Guard Container",
                Category = "Edge App Guard Policy",
                Description =
                    "Blocks clipboard copy operations from the host to the Application Guard container, preventing data injection into the isolated browsing session.",
                Tags = ["edge", "app-guard", "clipboard", "hardening", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Cannot paste host clipboard content into App Guard session; data enters container via other means only.",
                ApplyOps = [RegOp.SetDword(Key, "AppHVSIClipboardSettings", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AppHVSIClipboardSettings")],
                DetectOps = [RegOp.CheckDword(Key, "AppHVSIClipboardSettings", 0)],
            },
            new TweakDef
            {
                Id = "eaguard-block-printing",
                Label = "Block Printing from Application Guard Sessions",
                Category = "Edge App Guard Policy",
                Description =
                    "Prevents users from printing from within Application Guard container sessions, blocking data exfiltration via printed documents from isolated browsing sessions.",
                Tags = ["edge", "app-guard", "printing", "hardening", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Printing from App Guard session blocked; all print operations must occur from trusted host context.",
                ApplyOps = [RegOp.SetDword(Key, "AppHVSIPrintingSettings", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AppHVSIPrintingSettings")],
                DetectOps = [RegOp.CheckDword(Key, "AppHVSIPrintingSettings", 0)],
            },
            new TweakDef
            {
                Id = "eaguard-block-data-persistence",
                Label = "Block Data Persistence in Application Guard",
                Category = "Edge App Guard Policy",
                Description =
                    "Disables data persistence in Application Guard containers so all browsing data (cookies, cached files, saved passwords) is discarded when the container terminates.",
                Tags = ["edge", "app-guard", "persistence", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Container data wiped on session close; no session data survives for attackers to access.",
                ApplyOps = [RegOp.SetDword(Key, "AllowPersistence", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowPersistence")],
                DetectOps = [RegOp.CheckDword(Key, "AllowPersistence", 0)],
            },
            new TweakDef
            {
                Id = "eaguard-block-camera-mic",
                Label = "Block Camera and Microphone in App Guard",
                Category = "Edge App Guard Policy",
                Description =
                    "Blocks camera and microphone access for Application Guard container sessions, preventing untrusted web content from capturing audio or video from host hardware.",
                Tags = ["edge", "app-guard", "camera", "microphone", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Camera and mic blocked in isolated sessions; media capture by untrusted sites impossible.",
                ApplyOps = [RegOp.SetDword(Key, "AllowCameraMicrophoneRedirection", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowCameraMicrophoneRedirection")],
                DetectOps = [RegOp.CheckDword(Key, "AllowCameraMicrophoneRedirection", 0)],
            },
            new TweakDef
            {
                Id = "eaguard-enable-enterprise-mode",
                Label = "Enable Enterprise Management of Application Guard",
                Category = "Edge App Guard Policy",
                Description =
                    "Enables enterprise management mode for Application Guard, allowing IT to configure trusted site lists and manage container policies centrally.",
                Tags = ["edge", "app-guard", "enterprise", "management", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "App Guard policies centrally managed; enterprise trust list governs which sites use the container.",
                ApplyOps = [RegOp.SetDword(Key, "AllowAppHVSI_ManagedBrowser", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowAppHVSI_ManagedBrowser")],
                DetectOps = [RegOp.CheckDword(Key, "AllowAppHVSI_ManagedBrowser", 1)],
            },
            new TweakDef
            {
                Id = "eaguard-block-virtual-gpu",
                Label = "Block Hardware Accelerated Rendering in App Guard",
                Category = "Edge App Guard Policy",
                Description =
                    "Disables hardware-accelerated (virtual GPU) rendering inside Application Guard, forcing software rendering to prevent GPU-based container escape attacks.",
                Tags = ["edge", "app-guard", "gpu", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "vGPU disabled in App Guard; rendering performance reduced but GPU attack surface eliminated.",
                ApplyOps = [RegOp.SetDword(Key, "AllowVirtualGPU", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowVirtualGPU")],
                DetectOps = [RegOp.CheckDword(Key, "AllowVirtualGPU", 0)],
            },
            new TweakDef
            {
                Id = "eaguard-block-download-host",
                Label = "Block Downloads from App Guard to Host",
                Category = "Edge App Guard Policy",
                Description =
                    "Prevents files downloaded inside the Application Guard container from being saved or transferred to the host file system, preventing malware delivery via trusted-site download.",
                Tags = ["edge", "app-guard", "download", "hardening", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Container downloads stay isolated; files cannot be moved from App Guard to host.",
                ApplyOps = [RegOp.SetDword(Key, "SaveFilesToHost", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "SaveFilesToHost")],
                DetectOps = [RegOp.CheckDword(Key, "SaveFilesToHost", 0)],
            },
            new TweakDef
            {
                Id = "eaguard-enable-audit-events",
                Label = "Enable Application Guard Audit Events",
                Category = "Edge App Guard Policy",
                Description =
                    "Enables audit event logging for Application Guard operations, recording container creation, clipboard events, and policy violations to the event log.",
                Tags = ["edge", "app-guard", "audit", "logging", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "App Guard events logged; security monitoring and forensics improve.",
                ApplyOps = [RegOp.SetDword(Key, "AuditApplicationGuard", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditApplicationGuard")],
                DetectOps = [RegOp.CheckDword(Key, "AuditApplicationGuard", 1)],
            },
            new TweakDef
            {
                Id = "eaguard-enforce-network-isolation",
                Label = "Enforce Network Isolation in Application Guard",
                Category = "Edge App Guard Policy",
                Description =
                    "Enforces strict network isolation on Application Guard containers, ensuring they can only access the internet and cannot reach internal corporate network resources.",
                Tags = ["edge", "app-guard", "network-isolation", "hardening", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "App Guard containers isolated from intranet; compromised sessions cannot pivot to internal resources.",
                ApplyOps = [RegOp.SetDword(Key, "BlockNonEnterpriseContent", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockNonEnterpriseContent")],
                DetectOps = [RegOp.CheckDword(Key, "BlockNonEnterpriseContent", 1)],
            },
        ];
}
