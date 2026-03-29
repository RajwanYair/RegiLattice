// RegiLattice.Core — Tweaks/WsaAndroidPolicy.cs
// Windows Subsystem for Android (WSA) core settings and Android runtime controls — Sprint 467.
// Category: "WSA Android Policy" | Slug: wsacore
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\WindowsSubsystemForAndroid

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WsaAndroidPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsSubsystemForAndroid";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "wsacore-disable-wsa",
                Label = "Disable Windows Subsystem for Android",
                Category = "WSA Android Policy",
                Description =
                    "Disables the Windows Subsystem for Android (WSA) entirely, preventing Android app installation and the associated Amazon Appstore service from running.",
                Tags = ["wsa", "android", "subsystem", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "WSA disabled; Android apps cannot be installed or launched.",
                ApplyOps = [RegOp.SetDword(Key, "DisableWindowsSubsystemForAndroid", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableWindowsSubsystemForAndroid")],
                DetectOps = [RegOp.CheckDword(Key, "DisableWindowsSubsystemForAndroid", 1)],
            },
            new TweakDef
            {
                Id = "wsacore-block-amazon-appstore",
                Label = "Block Amazon Appstore Integration with WSA",
                Category = "WSA Android Policy",
                Description =
                    "Blocks the Amazon Appstore integration in WSA, preventing users from browsing, installing, or updating Android apps via the Amazon storefront.",
                Tags = ["wsa", "amazon-appstore", "android", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Amazon Appstore blocked in WSA; Android app discovery and install blocked.",
                ApplyOps = [RegOp.SetDword(Key, "BlockAmazonAppstoreIntegration", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockAmazonAppstoreIntegration")],
                DetectOps = [RegOp.CheckDword(Key, "BlockAmazonAppstoreIntegration", 1)],
            },
            new TweakDef
            {
                Id = "wsacore-disable-android-diagnostics",
                Label = "Disable WSA Diagnostic Data Upload",
                Category = "WSA Android Policy",
                Description =
                    "Disables the upload of Android runtime diagnostic data (crash reports, performance telemetry) from WSA to Microsoft and Amazon servers.",
                Tags = ["wsa", "android", "diagnostics", "telemetry", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "WSA diagnostic data not uploaded; Android app crashes not sent to telemetry endpoints.",
                ApplyOps = [RegOp.SetDword(Key, "DisableAndroidDiagnosticsUpload", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAndroidDiagnosticsUpload")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAndroidDiagnosticsUpload", 1)],
            },
            new TweakDef
            {
                Id = "wsacore-disable-wsa-autostart",
                Label = "Disable WSA Auto-Start on Windows Startup",
                Category = "WSA Android Policy",
                Description =
                    "Prevents the WSA container virtual machine from starting automatically on Windows boot, reducing memory and CPU overhead on systems where Android apps are rarely used.",
                Tags = ["wsa", "android", "autostart", "startup", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "WSA VM not auto-started; first Android app launch takes slightly longer.",
                ApplyOps = [RegOp.SetDword(Key, "DisableWSAAutoStart", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableWSAAutoStart")],
                DetectOps = [RegOp.CheckDword(Key, "DisableWSAAutoStart", 1)],
            },
            new TweakDef
            {
                Id = "wsacore-block-android-clipboard",
                Label = "Block Android App Access to Windows Clipboard",
                Category = "WSA Android Policy",
                Description =
                    "Prevents Android applications running in WSA from reading or writing the Windows clipboard, isolating Android app clipboard access from sensitive Windows application data.",
                Tags = ["wsa", "android", "clipboard", "isolation", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Android apps cannot access Windows clipboard; paste between Windows and Android apps blocked.",
                ApplyOps = [RegOp.SetDword(Key, "BlockAndroidClipboardAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockAndroidClipboardAccess")],
                DetectOps = [RegOp.CheckDword(Key, "BlockAndroidClipboardAccess", 1)],
            },
            new TweakDef
            {
                Id = "wsacore-block-android-camera",
                Label = "Block Android App Camera Access in WSA",
                Category = "WSA Android Policy",
                Description =
                    "Prevents Android applications in WSA from accessing the Windows system camera, blocking Android apps from using the webcam or integrated camera hardware.",
                Tags = ["wsa", "android", "camera", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Camera access blocked for Android apps in WSA; video calls and photo apps in WSA cannot use the webcam.",
                ApplyOps = [RegOp.SetDword(Key, "BlockAndroidCameraAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockAndroidCameraAccess")],
                DetectOps = [RegOp.CheckDword(Key, "BlockAndroidCameraAccess", 1)],
            },
            new TweakDef
            {
                Id = "wsacore-block-android-microphone",
                Label = "Block Android App Microphone Access in WSA",
                Category = "WSA Android Policy",
                Description =
                    "Prevents Android applications in WSA from accessing the system microphone, blocking audio recording by Android apps running in the Windows subsystem.",
                Tags = ["wsa", "android", "microphone", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Microphone blocked for Android apps in WSA; voice recording Android apps will see no audio input.",
                ApplyOps = [RegOp.SetDword(Key, "BlockAndroidMicrophoneAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockAndroidMicrophoneAccess")],
                DetectOps = [RegOp.CheckDword(Key, "BlockAndroidMicrophoneAccess", 1)],
            },
            new TweakDef
            {
                Id = "wsacore-disable-android-gpu",
                Label = "Disable Android GPU Hardware Acceleration in WSA",
                Category = "WSA Android Policy",
                Description =
                    "Disables hardware GPU acceleration for the WSA Android container, forcing software rendering, which reduces GPU load and prevents direct GPU driver access from Android apps.",
                Tags = ["wsa", "android", "gpu", "acceleration", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Android GPU acceleration disabled; apps use software rendering (slower but isolated from GPU driver).",
                ApplyOps = [RegOp.SetDword(Key, "DisableAndroidGPUAcceleration", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAndroidGPUAcceleration")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAndroidGPUAcceleration", 1)],
            },
            new TweakDef
            {
                Id = "wsacore-restrict-android-location",
                Label = "Restrict Android App Location Access in WSA",
                Category = "WSA Android Policy",
                Description =
                    "Blocks Android applications in WSA from accessing the Windows location service, preventing Android apps from determining geolocation via GPS, Wi-Fi triangulation, or IP-based lookup.",
                Tags = ["wsa", "android", "location", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Location blocked for Android apps in WSA; apps request location and receive denials.",
                ApplyOps = [RegOp.SetDword(Key, "BlockAndroidLocationAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockAndroidLocationAccess")],
                DetectOps = [RegOp.CheckDword(Key, "BlockAndroidLocationAccess", 1)],
            },
            new TweakDef
            {
                Id = "wsacore-limit-android-memory",
                Label = "Limit WSA Container Memory to 4 GB",
                Category = "WSA Android Policy",
                Description =
                    "Limits the maximum RAM allocation for the WSA Android container to 4 GB, preventing Android apps from consuming excessive system memory on devices with limited RAM.",
                Tags = ["wsa", "android", "memory", "resource-limit", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "WSA container capped at 4 GB RAM; memory-intensive Android apps may be terminated by the Android OOM killer.",
                ApplyOps = [RegOp.SetDword(Key, "AndroidContainerMaxMemoryMB", 4096)],
                RemoveOps = [RegOp.DeleteValue(Key, "AndroidContainerMaxMemoryMB")],
                DetectOps = [RegOp.CheckDword(Key, "AndroidContainerMaxMemoryMB", 4096)],
            },
        ];
}
