// RegiLattice.Core — Tweaks/WsaStoragePolicy.cs
// WSA Android container storage access, file sharing, and SD card policy controls — Sprint 471.
// Category: "WSA Storage Policy" | Slug: wsastor
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\WindowsSubsystemForAndroid\Storage

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WsaStoragePolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsSubsystemForAndroid\Storage";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "wsastor-block-android-host-file-access",
                Label = "Block Android Container Access to Windows File System",
                Category = "WSA Storage Policy",
                Description =
                    "Prevents Android applications in WSA from accessing Windows host file system paths (outside the dedicated Android container storage), isolating Android apps from Windows user documents and system files.",
                Tags = ["wsa", "android", "storage", "file-system", "isolation", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Android apps confined to container storage; Windows file system paths invisible to Android apps.",
                ApplyOps = [RegOp.SetDword(Key, "BlockAndroidHostFileAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockAndroidHostFileAccess")],
                DetectOps = [RegOp.CheckDword(Key, "BlockAndroidHostFileAccess", 1)],
            },
            new TweakDef
            {
                Id = "wsastor-disable-android-sd-card",
                Label = "Disable Android Virtual SD Card in WSA",
                Category = "WSA Storage Policy",
                Description =
                    "Disables the virtual SD card / removable storage emulation in WSA, preventing Android apps from accessing or exfiltrating data via the Android external storage API.",
                Tags = ["wsa", "android", "sd-card", "storage", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Virtual SD card removed from WSA; apps using external storage API will see no removable storage.",
                ApplyOps = [RegOp.SetDword(Key, "DisableAndroidSDCard", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAndroidSDCard")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAndroidSDCard", 1)],
            },
            new TweakDef
            {
                Id = "wsastor-restrict-android-download-folder",
                Label = "Restrict Android Download Folder to Container Only",
                Category = "WSA Storage Policy",
                Description =
                    "Restricts the Android Downloads folder to within the WSA container, preventing downloaded files from automatically syncing to the Windows Downloads folder.",
                Tags = ["wsa", "android", "download", "storage", "isolation", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Android downloads stay in container; no automatic sync to Windows Downloads folder.",
                ApplyOps = [RegOp.SetDword(Key, "RestrictAndroidDownloadToContainer", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictAndroidDownloadToContainer")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictAndroidDownloadToContainer", 1)],
            },
            new TweakDef
            {
                Id = "wsastor-limit-container-storage",
                Label = "Limit WSA Container Storage to 16 GB",
                Category = "WSA Storage Policy",
                Description =
                    "Caps the WSA Android container image size at 16 GB, preventing Android apps from consuming excessive disk space on devices with limited storage.",
                Tags = ["wsa", "android", "storage", "quota", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "WSA container storage capped at 16 GB; app installs fail when limit is reached.",
                ApplyOps = [RegOp.SetDword(Key, "ContainerMaxStorageGB", 16)],
                RemoveOps = [RegOp.DeleteValue(Key, "ContainerMaxStorageGB")],
                DetectOps = [RegOp.CheckDword(Key, "ContainerMaxStorageGB", 16)],
            },
            new TweakDef
            {
                Id = "wsastor-block-android-usb-transfer",
                Label = "Block Android MTP/USB File Transfer from WSA",
                Category = "WSA Storage Policy",
                Description =
                    "Blocks the Android Media Transfer Protocol (MTP) and USB file transfer APIs in WSA, preventing Android apps from transferring files to/from connected USB storage devices.",
                Tags = ["wsa", "android", "mtp", "usb", "storage", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Android MTP/USB transfer blocked; Android apps cannot access or write to USB drives.",
                ApplyOps = [RegOp.SetDword(Key, "BlockAndroidMTPTransfer", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockAndroidMTPTransfer")],
                DetectOps = [RegOp.CheckDword(Key, "BlockAndroidMTPTransfer", 1)],
            },
            new TweakDef
            {
                Id = "wsastor-disable-android-photos-sync",
                Label = "Disable Android Photos Auto-Sync with Windows Photos",
                Category = "WSA Storage Policy",
                Description =
                    "Disables automatic synchronisation between the Android container photo gallery and the Windows Photos app, preventing Android photo data from being accessible to Windows apps without explicit sharing.",
                Tags = ["wsa", "android", "photos", "sync", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Android photos not synced to Windows Photos; camera roll data stays in Android container.",
                ApplyOps = [RegOp.SetDword(Key, "DisableAndroidPhotosSync", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAndroidPhotosSync")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAndroidPhotosSync", 1)],
            },
            new TweakDef
            {
                Id = "wsastor-block-android-contact-access",
                Label = "Block Android App Access to Windows Contacts",
                Category = "WSA Storage Policy",
                Description =
                    "Prevents Android applications in WSA from reading Windows People/Contacts data, isolating the Android contact database from the Windows contact store.",
                Tags = ["wsa", "android", "contacts", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Android apps cannot read Windows contacts; contact harvesting by Android apps prevented.",
                ApplyOps = [RegOp.SetDword(Key, "BlockAndroidContactAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockAndroidContactAccess")],
                DetectOps = [RegOp.CheckDword(Key, "BlockAndroidContactAccess", 1)],
            },
            new TweakDef
            {
                Id = "wsastor-disable-android-calendar-sync",
                Label = "Disable Android Calendar Sync with Windows Calendar",
                Category = "WSA Storage Policy",
                Description =
                    "Disables calendar event synchronisation between Android apps in WSA and the Windows Calendar app, preventing calendar data from crossing the Android/Windows boundary.",
                Tags = ["wsa", "android", "calendar", "sync", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Android and Windows calendars not synced; schedule data stays in its respective system.",
                ApplyOps = [RegOp.SetDword(Key, "DisableAndroidCalendarSync", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAndroidCalendarSync")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAndroidCalendarSync", 1)],
            },
            new TweakDef
            {
                Id = "wsastor-enable-android-storage-audit",
                Label = "Enable Audit Logging for Android Storage Operations",
                Category = "WSA Storage Policy",
                Description =
                    "Enables event log entries for significant Android storage operations (large reads/writes, external storage access) to provide visibility into Android app file system behaviour.",
                Tags = ["wsa", "android", "storage", "audit", "logging", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Android storage events logged; bulk read/write operations by Android apps are auditable.",
                ApplyOps = [RegOp.SetDword(Key, "EnableAndroidStorageAuditLog", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableAndroidStorageAuditLog")],
                DetectOps = [RegOp.CheckDword(Key, "EnableAndroidStorageAuditLog", 1)],
            },
            new TweakDef
            {
                Id = "wsastor-block-android-screencapture",
                Label = "Block Screenshot/Screen Recording by Android Apps in WSA",
                Category = "WSA Storage Policy",
                Description =
                    "Prevents Android applications in WSA from capturing screenshots or recording the screen, ensuring Android apps cannot exfiltrate screen contents to their cloud services.",
                Tags = ["wsa", "android", "screenshot", "screen-recording", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Android screenshot and screen recording APIs blocked in WSA; screen data cannot be saved or uploaded by Android apps.",
                ApplyOps = [RegOp.SetDword(Key, "BlockAndroidScreenCapture", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockAndroidScreenCapture")],
                DetectOps = [RegOp.CheckDword(Key, "BlockAndroidScreenCapture", 1)],
            },
        ];
}
