namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

[TweakModule]
internal static class PolicyCompressedFolders
{
    // HKLM\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\Explorer — controls
    // ZIP/compressed folder integration in File Explorer and shell.
    // HKLM\SOFTWARE\Policies\Microsoft\Windows\CompressedFolders — dedicated key.

    private const string ZipKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CompressedFolders";
    private const string ExplKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "zipfld-disable-compressed-folders",
            Label = "Disable ZIP Compressed Folder Support in Explorer",
            Category = "Storage — Work Folders",
            Description =
                "Sets DisableCompressedFolders=1 in the CompressedFolders Group Policy key. "
                + "Removes the native ZIP/compressed folder handler from File Explorer. "
                + "Users can no longer double-click a ZIP file to browse it as a folder within Explorer. "
                + "Useful when a third-party archiver (7-Zip, WinRAR) is the preferred tool on managed machines.",
            Tags = ["zip", "compressed", "explorer", "policy", "enterprise"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "ZIP files no longer open as virtual folders in Explorer; requires a third-party archiver.",
            ApplyOps = [RegOp.SetDword(ZipKey, "DisableCompressedFolders", 1)],
            RemoveOps = [RegOp.DeleteValue(ZipKey, "DisableCompressedFolders")],
            DetectOps = [RegOp.CheckDword(ZipKey, "DisableCompressedFolders", 1)],
        },
        new TweakDef
        {
            Id = "zipfld-disable-extract-all",
            Label = "Remove 'Extract All' Context-Menu Option",
            Category = "Storage — Work Folders",
            Description =
                "Sets DisableExtractAll=1 in the CompressedFolders Group Policy key. "
                + "Hides the 'Extract All' entry from the right-click context menu on ZIP files. "
                + "Combined with a managed archiver deployment, this enforces the corporate tool for archive extraction.",
            Tags = ["zip", "extract", "context-menu", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "'Extract All' is removed from ZIP context menus; users must use an installed archiver.",
            ApplyOps = [RegOp.SetDword(ZipKey, "DisableExtractAll", 1)],
            RemoveOps = [RegOp.DeleteValue(ZipKey, "DisableExtractAll")],
            DetectOps = [RegOp.CheckDword(ZipKey, "DisableExtractAll", 1)],
        },
        new TweakDef
        {
            Id = "zipfld-disable-compress-selected-files",
            Label = "Remove 'Compress to ZIP' Context-Menu Option",
            Category = "Storage — Work Folders",
            Description =
                "Sets DisableNewCompressedFolder=1 in the CompressedFolders Group Policy key. "
                + "Removes the 'Compress to ZIP file' entry from the File Explorer shell context menu. "
                + "Prevents users from creating ZIP files directly from Explorer, directing archive operations to managed tools.",
            Tags = ["zip", "compress", "context-menu", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "ZIP creation from Explorer context menu is hidden; archiver tool required.",
            ApplyOps = [RegOp.SetDword(ZipKey, "DisableNewCompressedFolder", 1)],
            RemoveOps = [RegOp.DeleteValue(ZipKey, "DisableNewCompressedFolder")],
            DetectOps = [RegOp.CheckDword(ZipKey, "DisableNewCompressedFolder", 1)],
        },
        new TweakDef
        {
            Id = "zipfld-block-network-archive-open",
            Label = "Block Opening Remote ZIP Files as Virtual Folders",
            Category = "Storage — Work Folders",
            Description =
                "Sets DisableNetworkCompressedFolders=1 in the CompressedFolders Group Policy key. "
                + "Prevents users from browsing ZIP archives located on network shares as virtual folders. "
                + "Reduces risk of data exfiltration via archive browsing of network resources and prevents "
                + "potential path-traversal attacks embedded in malicious remote ZIP files.",
            Tags = ["zip", "network", "security", "policy", "enterprise"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "ZIP files on network drives cannot be browsed as virtual folders in Explorer.",
            ApplyOps = [RegOp.SetDword(ZipKey, "DisableNetworkCompressedFolders", 1)],
            RemoveOps = [RegOp.DeleteValue(ZipKey, "DisableNetworkCompressedFolders")],
            DetectOps = [RegOp.CheckDword(ZipKey, "DisableNetworkCompressedFolders", 1)],
        },
        new TweakDef
        {
            Id = "zipfld-disable-cab-browsing",
            Label = "Disable CAB File Browsing in Explorer",
            Category = "Storage — Work Folders",
            Description =
                "Sets DisableCabFolders=1 in the CompressedFolders Group Policy key. "
                + "Prevents File Explorer from opening Microsoft Cabinet (.cab) files as virtual folders. "
                + "CAB files are used as installers and update containers — browsing them directly can "
                + "expose sensitive setup binaries. Forcing use of proper extraction tools adds an audit layer.",
            Tags = ["cab", "cabinet", "compressed", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = ".cab files no longer open as virtual folders; dedicated extraction required.",
            ApplyOps = [RegOp.SetDword(ZipKey, "DisableCabFolders", 1)],
            RemoveOps = [RegOp.DeleteValue(ZipKey, "DisableCabFolders")],
            DetectOps = [RegOp.CheckDword(ZipKey, "DisableCabFolders", 1)],
        },
        new TweakDef
        {
            Id = "zipfld-restrict-autorun-in-archive",
            Label = "Block AutoRun Execution Inside Archive Folders",
            Category = "Storage — Work Folders",
            Description =
                "Sets BlockArchiveAutoRun=1 in the CompressedFolders Group Policy key. "
                + "Prevents autorun.inf scripts embedded in ZIP/CAB archives from executing when the archive "
                + "is browsed as a virtual folder. Removes a potential initial-access vector for malware "
                + "distributed via weaponised archives delivered over email or USB.",
            Tags = ["zip", "autorun", "security", "malware", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "AutoRun scripts inside archives are blocked from executing within Explorer.",
            ApplyOps = [RegOp.SetDword(ZipKey, "BlockArchiveAutoRun", 1)],
            RemoveOps = [RegOp.DeleteValue(ZipKey, "BlockArchiveAutoRun")],
            DetectOps = [RegOp.CheckDword(ZipKey, "BlockArchiveAutoRun", 1)],
        },
        new TweakDef
        {
            Id = "zipfld-disable-zip-sendto",
            Label = "Remove 'Send To Compressed Folder' from Right-Click",
            Category = "Storage — Work Folders",
            Description =
                "Sets DisableSendToCompressed=1 in the CompressedFolders Group Policy key. "
                + "Removes the 'Compressed (zipped) folder' destination from the Send To context menu entry. "
                + "Prevents casual in-place ZIP creation that bypasses DLP scanning on managed endpoints.",
            Tags = ["zip", "sendto", "context-menu", "dlp", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Send To > Compressed Folder is hidden; users must use an explicit archiver.",
            ApplyOps = [RegOp.SetDword(ZipKey, "DisableSendToCompressed", 1)],
            RemoveOps = [RegOp.DeleteValue(ZipKey, "DisableSendToCompressed")],
            DetectOps = [RegOp.CheckDword(ZipKey, "DisableSendToCompressed", 1)],
        },
        new TweakDef
        {
            Id = "zipfld-restrict-archive-max-size",
            Label = "Enforce Maximum Archive Size Limit",
            Category = "Storage — Work Folders",
            Description =
                "Sets MaxArchiveSizeMB=512 in the CompressedFolders Group Policy key. "
                + "Limits the maximum size of archives that Explorer will open as virtual folders to 512 MB. "
                + "Prevents ZIP-bomb denial-of-service attacks and runaway memory consumption when users "
                + "accidentally open decompression-ratio-maximised archives.",
            Tags = ["zip", "size-limit", "security", "dos", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Archives larger than 512 MB will not open as virtual folders in Explorer.",
            ApplyOps = [RegOp.SetDword(ZipKey, "MaxArchiveSizeMB", 512)],
            RemoveOps = [RegOp.DeleteValue(ZipKey, "MaxArchiveSizeMB")],
            DetectOps = [RegOp.CheckDword(ZipKey, "MaxArchiveSizeMB", 512)],
        },
        new TweakDef
        {
            Id = "zipfld-disable-archive-preview-handler",
            Label = "Disable Archive Preview Handler in Reading Pane",
            Category = "Storage — Work Folders",
            Description =
                "Sets DisableArchivePreviewHandler=1 in the CompressedFolders Group Policy key. "
                + "Prevents the Explorer Reading Pane from rendering a ZIP/CAB file preview when it is selected. "
                + "Preview rendering parses archive headers in-process; disabling it reduces attack surface for "
                + "vulnerabilities in the compressed-folders shell handler.",
            Tags = ["zip", "preview", "reading-pane", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Archive files show no preview in Explorer Reading Pane.",
            ApplyOps = [RegOp.SetDword(ZipKey, "DisableArchivePreviewHandler", 1)],
            RemoveOps = [RegOp.DeleteValue(ZipKey, "DisableArchivePreviewHandler")],
            DetectOps = [RegOp.CheckDword(ZipKey, "DisableArchivePreviewHandler", 1)],
        },
        new TweakDef
        {
            Id = "zipfld-enforce-archive-scan-on-open",
            Label = "Enforce Antivirus Scan Before Opening Archive Content",
            Category = "Storage — Work Folders",
            Description =
                "Sets RequireScanBeforeArchiveOpen=1 in the CompressedFolders Group Policy key. "
                + "Forces Windows Defender or the registered antivirus to scan archive contents before "
                + "the virtual folder view is presented to the user. Prevents deferred-scan gaps where "
                + "malicious payloads inside archives reach the desktop before AV inspection completes.",
            Tags = ["zip", "antivirus", "scan", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Archive contents are AV-scanned before being displayed in Explorer.",
            ApplyOps = [RegOp.SetDword(ZipKey, "RequireScanBeforeArchiveOpen", 1)],
            RemoveOps = [RegOp.DeleteValue(ZipKey, "RequireScanBeforeArchiveOpen")],
            DetectOps = [RegOp.CheckDword(ZipKey, "RequireScanBeforeArchiveOpen", 1)],
        },
    ];
}
