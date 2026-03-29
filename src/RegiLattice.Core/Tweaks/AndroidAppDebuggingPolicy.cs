// RegiLattice.Core — Tweaks/AndroidAppDebuggingPolicy.cs
// WSA Android app debugging, ADB access, and developer mode controls — Sprint 468.
// Category: "Android App Debugging Policy" | Slug: wsadbg
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\WindowsSubsystemForAndroid\Debugging

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class AndroidAppDebuggingPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsSubsystemForAndroid\Debugging";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "wsadbg-disable-adb-access",
                Label = "Disable ADB Access to WSA Android Container",
                Category = "Android App Debugging Policy",
                Description =
                    "Disables Android Debug Bridge (ADB) access to the WSA container, preventing developer debug connections, app sideloading via ADB, and adb shell command execution.",
                Tags = ["wsa", "android", "adb", "debugging", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "ADB disabled for WSA; cannot sideload APKs or access Android shell via adb connect.",
                ApplyOps = [RegOp.SetDword(Key, "DisableADBAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableADBAccess")],
                DetectOps = [RegOp.CheckDword(Key, "DisableADBAccess", 1)],
            },
            new TweakDef
            {
                Id = "wsadbg-disable-android-developer-options",
                Label = "Disable Android Developer Options in WSA",
                Category = "Android App Debugging Policy",
                Description =
                    "Disables the Android Developer Options menu in WSA settings, blocking users from enabling USB debugging, mock location, or other developer settings within the Android container.",
                Tags = ["wsa", "android", "developer-options", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Developer Options hidden in WSA; advanced Android debug settings unavailable.",
                ApplyOps = [RegOp.SetDword(Key, "DisableAndroidDeveloperOptions", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAndroidDeveloperOptions")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAndroidDeveloperOptions", 1)],
            },
            new TweakDef
            {
                Id = "wsadbg-block-sideloaded-apks",
                Label = "Block Sideloaded APK Installation in WSA",
                Category = "Android App Debugging Policy",
                Description =
                    "Blocks the installation of APK files from outside the Amazon Appstore, preventing users from installing potentially malicious Android apps via direct APK transfer.",
                Tags = ["wsa", "android", "sideloading", "apk", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "APK sideloading blocked; only Amazon Appstore apps can be installed in WSA.",
                ApplyOps = [RegOp.SetDword(Key, "BlockSideloadedAPKInstallation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockSideloadedAPKInstallation")],
                DetectOps = [RegOp.CheckDword(Key, "BlockSideloadedAPKInstallation", 1)],
            },
            new TweakDef
            {
                Id = "wsadbg-disable-logcat-output",
                Label = "Disable Android Logcat Access from Host OS",
                Category = "Android App Debugging Policy",
                Description =
                    "Prevents the Windows host OS processes from reading Android logcat output from the WSA container, reducing diagnostic data exposure from Android app crash logs.",
                Tags = ["wsa", "android", "logcat", "debugging", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Android logcat access from host blocked; app crash data from WSA not accessible to host processes.",
                ApplyOps = [RegOp.SetDword(Key, "DisableLogcatFromHost", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableLogcatFromHost")],
                DetectOps = [RegOp.CheckDword(Key, "DisableLogcatFromHost", 1)],
            },
            new TweakDef
            {
                Id = "wsadbg-disable-android-crash-reporting",
                Label = "Disable Android App Crash Report Upload from WSA",
                Category = "Android App Debugging Policy",
                Description =
                    "Disables the automatic upload of Android application crash reports from within the WSA container to app developers or Amazon, preventing personal or usage data from reaching third parties.",
                Tags = ["wsa", "android", "crash-report", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Android app crash reports not uploaded; developers and Amazon receive no crash telemetry.",
                ApplyOps = [RegOp.SetDword(Key, "DisableAndroidCrashReporting", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAndroidCrashReporting")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAndroidCrashReporting", 1)],
            },
            new TweakDef
            {
                Id = "wsadbg-block-android-root-detection-bypass",
                Label = "Block Android Root-Detection Bypass Tools in WSA",
                Category = "Android App Debugging Policy",
                Description =
                    "Blocks Magisk-style root detection bypass frameworks from being installed or operating within the WSA container, preventing banking and DRM apps from being tricked into running on a 'rooted' environment.",
                Tags = ["wsa", "android", "root-detection", "magisk", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Root bypass frameworks blocked; WSA reports its true environment status to apps.",
                ApplyOps = [RegOp.SetDword(Key, "BlockRootDetectionBypass", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockRootDetectionBypass")],
                DetectOps = [RegOp.CheckDword(Key, "BlockRootDetectionBypass", 1)],
            },
            new TweakDef
            {
                Id = "wsadbg-require-apk-signature-verify",
                Label = "Require APK Signature Verification Before Install",
                Category = "Android App Debugging Policy",
                Description =
                    "Enforces APK v2/v3 signature verification for all Android packages installed in WSA, blocking install of APKs with tampered or missing signatures.",
                Tags = ["wsa", "android", "apk-signature", "integrity", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "APK signature required for install; tampered or unsigned APKs rejected.",
                ApplyOps = [RegOp.SetDword(Key, "RequireAPKSignatureVerification", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireAPKSignatureVerification")],
                DetectOps = [RegOp.CheckDword(Key, "RequireAPKSignatureVerification", 1)],
            },
            new TweakDef
            {
                Id = "wsadbg-block-android-profiling",
                Label = "Block Android Performance Profiling from Host",
                Category = "Android App Debugging Policy",
                Description =
                    "Prevents host-side profiling tools (simpleperf, systrace, atrace) from attaching to Android processes in the WSA container, protecting Android app internals from host-side introspection.",
                Tags = ["wsa", "android", "profiling", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Android performance profiling from host blocked; APK memory and execution cannot be profiled from Windows.",
                ApplyOps = [RegOp.SetDword(Key, "BlockAndroidProfilingFromHost", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockAndroidProfilingFromHost")],
                DetectOps = [RegOp.CheckDword(Key, "BlockAndroidProfilingFromHost", 1)],
            },
            new TweakDef
            {
                Id = "wsadbg-disable-android-mock-location",
                Label = "Disable Mock Location in Android WSA",
                Category = "Android App Debugging Policy",
                Description =
                    "Disables the Android mock location provider in WSA, preventing apps and developers from injecting fake GPS coordinates to spoof location-based services.",
                Tags = ["wsa", "android", "mock-location", "gps", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Mock location disabled in WSA; GPS spoofing via Android developer options not possible.",
                ApplyOps = [RegOp.SetDword(Key, "DisableMockLocation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableMockLocation")],
                DetectOps = [RegOp.CheckDword(Key, "DisableMockLocation", 1)],
            },
            new TweakDef
            {
                Id = "wsadbg-block-wifi-password-sharing",
                Label = "Block Android Wi-Fi Password Sharing from WSA",
                Category = "Android App Debugging Policy",
                Description =
                    "Blocks the Android Wi-Fi password sharing feature in WSA that can export saved Wi-Fi credentials from the Android container as a QR code, preventing corporate Wi-Fi key leakage.",
                Tags = ["wsa", "android", "wifi", "credential-sharing", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Android Wi-Fi password QR sharing blocked; saved Wi-Fi credentials cannot be exported from WSA.",
                ApplyOps = [RegOp.SetDword(Key, "BlockAndroidWifiPasswordSharing", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockAndroidWifiPasswordSharing")],
                DetectOps = [RegOp.CheckDword(Key, "BlockAndroidWifiPasswordSharing", 1)],
            },
        ];
}
