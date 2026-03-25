#nullable enable
using System.Collections.Generic;
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

// Sprint 250 — Windows Photo Acquisition (WIA) Policy (10 tweaks)
// Keys: HKLM\SOFTWARE\Policies\Microsoft\Windows\PhotoAcquire
//       HKCU\Software\Policies\Microsoft\Windows\PhotoAcquire
//       HKLM\SOFTWARE\Policies\Microsoft\Windows\DeviceMetadata
internal static class PhotoAcquisitionPolicy
{
    private const string PaLm = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PhotoAcquire";
    private const string PaCu = @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\PhotoAcquire";
    private const string DevMetaKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceMetadata";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "photo-disable-acquire-wizard",
            Label = "Disable Photo Acquisition Wizard",
            Category = "Photo Acquisition Policy",
            Description =
                "Sets DisableAutoPlayForCamera=1 in the machine Photo Acquire policy key. "
                + "Prevents the Windows Photo Acquisition Wizard from launching automatically when a camera "
                + "or memory card is connected. Avoids the 'What do you want to do?' photo import prompt. "
                + "Default: absent (wizard auto-launches). Recommended: 1 to reduce unwanted automatic import.",
            Tags = ["photo", "camera", "import", "wizard", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Photo import wizard no longer auto-launches on camera/memory card connect; manual import via Photos app still works.",
            ApplyOps = [RegOp.SetDword(PaLm, "DisableAutoPlayForCamera", 1)],
            RemoveOps = [RegOp.DeleteValue(PaLm, "DisableAutoPlayForCamera")],
            DetectOps = [RegOp.CheckDword(PaLm, "DisableAutoPlayForCamera", 1)],
        },
        new TweakDef
        {
            Id = "photo-disable-acquire-wizard-user",
            Label = "Disable Photo Acquisition Wizard (Current User)",
            Category = "Photo Acquisition Policy",
            Description =
                "Sets DisableAutoPlayForCamera=1 in the per-user Photo Acquire policy key. "
                + "Suppresses the photo import prompt for the current user when a camera is connected, "
                + "without applying the restriction machine-wide. "
                + "Default: absent. Recommended: 1 on user profiles where automated photo import is undesired.",
            Tags = ["photo", "camera", "import", "user", "policy"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Photo import wizard suppressed for this user only.",
            ApplyOps = [RegOp.SetDword(PaCu, "DisableAutoPlayForCamera", 1)],
            RemoveOps = [RegOp.DeleteValue(PaCu, "DisableAutoPlayForCamera")],
            DetectOps = [RegOp.CheckDword(PaCu, "DisableAutoPlayForCamera", 1)],
        },
        new TweakDef
        {
            Id = "photo-disable-delete-after-import",
            Label = "Prevent Photo Deletion After Import",
            Category = "Photo Acquisition Policy",
            Description =
                "Sets NeverDeleteOriginalFiles=1 in the machine Photo Acquire policy key. "
                + "Prevents the Photo Acquisition Wizard from offering or performing deletion of photos "
                + "from the source camera/card after importing them to the PC. "
                + "Default: absent (deletion allowed). Recommended: 1 to protect source media from accidental deletion.",
            Tags = ["photo", "import", "delete", "protection", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Source photos never deleted after import; the wizard's 'Delete from device' option is disabled.",
            ApplyOps = [RegOp.SetDword(PaLm, "NeverDeleteOriginalFiles", 1)],
            RemoveOps = [RegOp.DeleteValue(PaLm, "NeverDeleteOriginalFiles")],
            DetectOps = [RegOp.CheckDword(PaLm, "NeverDeleteOriginalFiles", 1)],
        },
        new TweakDef
        {
            Id = "photo-disable-tag-on-import",
            Label = "Disable Automatic Tagging During Photo Import",
            Category = "Photo Acquisition Policy",
            Description =
                "Sets DisableTaggingOnAcquire=1 in the machine Photo Acquire policy key. "
                + "Stops the Photo Acquisition Wizard from automatically adding metadata tags to photos "
                + "during the import process. Useful when tagging must be done by a specific application. "
                + "Default: absent (tagging allowed). Recommended: 1 when a DMS or DAM system handles metadata.",
            Tags = ["photo", "import", "tagging", "metadata", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "Photos imported without auto-applied tags; metadata management left to the user or DMS.",
            ApplyOps = [RegOp.SetDword(PaLm, "DisableTaggingOnAcquire", 1)],
            RemoveOps = [RegOp.DeleteValue(PaLm, "DisableTaggingOnAcquire")],
            DetectOps = [RegOp.CheckDword(PaLm, "DisableTaggingOnAcquire", 1)],
        },
        new TweakDef
        {
            Id = "photo-disable-rotate-on-import",
            Label = "Disable Auto-Rotate During Photo Import",
            Category = "Photo Acquisition Policy",
            Description =
                "Sets DisableRotateOnAcquire=1 in the machine Photo Acquire policy key. "
                + "Prevents the acquisition wizard from auto-rotating photos based on EXIF orientation metadata. "
                + "Useful when images must be preserved in their original orientation for processing pipelines. "
                + "Default: absent (auto-rotate enabled). Recommended: 1 for raw capture workflows.",
            Tags = ["photo", "import", "rotate", "exif", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "EXIF auto-rotation skipped during import; images stored in their raw capture orientation.",
            ApplyOps = [RegOp.SetDword(PaLm, "DisableRotateOnAcquire", 1)],
            RemoveOps = [RegOp.DeleteValue(PaLm, "DisableRotateOnAcquire")],
            DetectOps = [RegOp.CheckDword(PaLm, "DisableRotateOnAcquire", 1)],
        },
        new TweakDef
        {
            Id = "photo-disable-title-on-import",
            Label = "Disable Title Prompt During Photo Import",
            Category = "Photo Acquisition Policy",
            Description =
                "Sets DisableTitleOnAcquire=1 in the machine Photo Acquire policy key. "
                + "Skips the title/description prompt during the photo acquisition wizard. "
                + "Useful in automated or batch import scenarios where manual metadata entry is not desired. "
                + "Default: absent (title prompt shown). Recommended: 1 in automated import pipelines.",
            Tags = ["photo", "import", "title", "metadata", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "Title/description prompt skipped during photo import; wizard completes without metadata entry.",
            ApplyOps = [RegOp.SetDword(PaLm, "DisableTitleOnAcquire", 1)],
            RemoveOps = [RegOp.DeleteValue(PaLm, "DisableTitleOnAcquire")],
            DetectOps = [RegOp.CheckDword(PaLm, "DisableTitleOnAcquire", 1)],
        },
        new TweakDef
        {
            Id = "photo-disable-open-explorer-after",
            Label = "Disable 'Open Folder' After Photo Import",
            Category = "Photo Acquisition Policy",
            Description =
                "Sets DisableOpenFilesystemAfterAcquire=1 in the machine Photo Acquire policy key. "
                + "Prevents the Photo Acquisition Wizard from automatically opening the destination folder "
                + "in Windows Explorer after importing photos. Reduces unnecessary window creation in workflows. "
                + "Default: absent (folder opens after import). Recommended: 1 in scripted/automated deployments.",
            Tags = ["photo", "import", "explorer", "post-import", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "Explorer does not open the import destination after the wizard completes; silent import.",
            ApplyOps = [RegOp.SetDword(PaLm, "DisableOpenFilesystemAfterAcquire", 1)],
            RemoveOps = [RegOp.DeleteValue(PaLm, "DisableOpenFilesystemAfterAcquire")],
            DetectOps = [RegOp.CheckDword(PaLm, "DisableOpenFilesystemAfterAcquire", 1)],
        },
        new TweakDef
        {
            Id = "photo-disable-wia-device-install",
            Label = "Disable WIA Device Metadata Internet Download",
            Category = "Photo Acquisition Policy",
            Description =
                "Sets PreventDeviceMetadataFromNetwork=1 in the Device Metadata policy key. "
                + "Prevents Windows from downloading WIA (Windows Image Acquisition) device metadata "
                + "and drivers from the internet for cameras, scanners, and photo devices. "
                + "Default: absent (online download allowed). Recommended: 1 on air-gapped or bandwidth-limited systems.",
            Tags = ["photo", "wia", "device-metadata", "network", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "WIA photo device metadata not downloaded from the internet; local or WSUS-served drivers only.",
            ApplyOps = [RegOp.SetDword(DevMetaKey, "PreventDeviceMetadataFromNetwork", 1)],
            RemoveOps = [RegOp.DeleteValue(DevMetaKey, "PreventDeviceMetadataFromNetwork")],
            DetectOps = [RegOp.CheckDword(DevMetaKey, "PreventDeviceMetadataFromNetwork", 1)],
        },
        new TweakDef
        {
            Id = "photo-disable-scanner-events",
            Label = "Disable WIA Scanner Device Events",
            Category = "Photo Acquisition Policy",
            Description =
                "Sets DisableScannerEvents=1 in the machine Photo Acquire policy key. "
                + "Suppresses WIA scanner events that trigger the photo acquisition wizard or image scanning dialogs "
                + "when a scanner button is pressed or paper is inserted, preventing interruptions. "
                + "Default: absent (scanner events enabled). Recommended: 1 on machines without attached scanners.",
            Tags = ["photo", "scanner", "wia", "events", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "Scanner hardware events suppressed; scanner still manually operable via its application.",
            ApplyOps = [RegOp.SetDword(PaLm, "DisableScannerEvents", 1)],
            RemoveOps = [RegOp.DeleteValue(PaLm, "DisableScannerEvents")],
            DetectOps = [RegOp.CheckDword(PaLm, "DisableScannerEvents", 1)],
        },
        new TweakDef
        {
            Id = "photo-disable-camera-events",
            Label = "Disable WIA Camera Device Events",
            Category = "Photo Acquisition Policy",
            Description =
                "Sets DisableCameraEvents=1 in the machine Photo Acquire policy key. "
                + "Suppresses WIA camera events that trigger the photo acquisition wizard when a digital camera "
                + "is connected via USB and a camera-side button is pressed. Prevents unplanned import dialogs. "
                + "Default: absent (camera events enabled). Recommended: 1 on machines without regular camera connections.",
            Tags = ["photo", "camera", "wia", "events", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "WIA camera button-press events suppressed; camera still functions normally when used manually.",
            ApplyOps = [RegOp.SetDword(PaLm, "DisableCameraEvents", 1)],
            RemoveOps = [RegOp.DeleteValue(PaLm, "DisableCameraEvents")],
            DetectOps = [RegOp.CheckDword(PaLm, "DisableCameraEvents", 1)],
        },
    ];
}
