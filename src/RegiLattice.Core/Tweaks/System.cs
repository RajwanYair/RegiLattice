namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Explorer
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "explorer-disable-search-history",
            Label = "Disable Search History",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows from storing device search history.",
            Tags = ["explorer", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsDeviceSearchHistoryEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsDeviceSearchHistoryEnabled", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsDeviceSearchHistoryEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "explorer-disable-gallery",
            Label = "Disable Gallery in Nav Pane",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes the Gallery entry from Explorer navigation pane (23H2+).",
            Tags = ["explorer", "win11"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Classes\CLSID\{e88865ea-0e1c-4e20-9aa6-edcd0212c87c}"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Classes\CLSID\{e88865ea-0e1c-4e20-9aa6-edcd0212c87c}",
                    "System.IsPinnedToNameSpaceTree",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Classes\CLSID\{e88865ea-0e1c-4e20-9aa6-edcd0212c87c}",
                    "System.IsPinnedToNameSpaceTree"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Classes\CLSID\{e88865ea-0e1c-4e20-9aa6-edcd0212c87c}",
                    "System.IsPinnedToNameSpaceTree",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "explorer-disable-auto-folder-type",
            Label = "Disable Auto Folder Type Detection",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Explorer from auto-detecting folder content type (e.g. 'Pictures', 'Music') which causes slow loading.",
            Tags = ["explorer", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Classes\Local Settings\Software\Microsoft\Windows\Shell\Bags\AllFolders\Shell"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_CURRENT_USER\Software\Classes\Local Settings\Software\Microsoft\Windows\Shell\Bags\AllFolders\Shell",
                    "FolderType",
                    "NotSpecified"
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Classes\Local Settings\Software\Microsoft\Windows\Shell\Bags\AllFolders\Shell",
                    "FolderType"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_CURRENT_USER\Software\Classes\Local Settings\Software\Microsoft\Windows\Shell\Bags\AllFolders\Shell",
                    "FolderType",
                    "NotSpecified"
                ),
            ],
        },
        new TweakDef
        {
            Id = "explorer-disable-recent-docs",
            Label = "Disable Recent Documents History",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables recent documents tracking in the Start menu and File Explorer. Improves privacy by not recording file access. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["explorer", "recent", "privacy", "history"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoRecentDocsHistory", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoRecentDocsHistory")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoRecentDocsHistory", 1),
            ],
        },
        new TweakDef
        {
            Id = "explorer-disable-thumb-cache-cleanup",
            Label = "Disable Thumbnail Cache Auto-Cleanup",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents Windows from automatically deleting the thumbnail cache during Disk Cleanup. Improves Explorer browsing performance for folders with many images. Default: auto-cleanup enabled. Recommended: disabled.",
            Tags = ["explorer", "thumbnail", "cache", "cleanup", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\VolumeCaches\Thumbnail Cache"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\VolumeCaches\Thumbnail Cache", "Autorun", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\VolumeCaches\Thumbnail Cache", "Autorun", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\VolumeCaches\Thumbnail Cache", "Autorun", 0),
            ],
        },
        new TweakDef
        {
            Id = "explorer-folder-size-tips",
            Label = "Show File Size in Folder Tooltips",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Shows the total file size and count in folder tooltips when hovering over a folder. Default: disabled. Recommended: enabled.",
            Tags = ["explorer", "folder", "size", "tooltip", "info"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "FolderContentsInfoTip", 1)],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "FolderContentsInfoTip", 0),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "FolderContentsInfoTip", 1),
            ],
        },
        new TweakDef
        {
            Id = "explorer-disable-new-app-alert",
            Label = "Disable 'New App' Notifications",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the 'A new app can open this type of file' notification that appears when a new program is installed. Default: shown. Recommended: disabled.",
            Tags = ["explorer", "notification", "new-app", "association"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "NoNewAppAlert", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "NoNewAppAlert")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "NoNewAppAlert", 1)],
        },
        new TweakDef
        {
            Id = "explorer-disable-sync-ads",
            Label = "Disable Sync Provider Ads",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables sync provider notifications in Explorer that show ads for OneDrive and other cloud services. Default: Shown. Recommended: Disabled.",
            Tags = ["explorer", "ads", "onedrive", "sync", "notifications"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowSyncProviderNotifications", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowSyncProviderNotifications", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced",
                    "ShowSyncProviderNotifications",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "explorer-always-show-menus",
            Label = "Always Show Classic Menu Bar",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Always shows the classic File/Edit/View/Help menu bar in Explorer windows. Default: Hidden. Recommended: Shown for power users.",
            Tags = ["explorer", "menu", "classic", "toolbar"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "AlwaysShowMenus", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "AlwaysShowMenus", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "AlwaysShowMenus", 1)],
        },
        new TweakDef
        {
            Id = "explorer-separate-process",
            Label = "Launch Folders in Separate Process",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Launches each Explorer folder window in its own process. Prevents a crash in one window from closing all others. Default: Shared process. Recommended: Separate for stability.",
            Tags = ["explorer", "process", "stability", "crash-recovery"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SeparateProcess", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SeparateProcess", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SeparateProcess", 1)],
        },
        new TweakDef
        {
            Id = "explorer-show-drive-letters-first",
            Label = "Show Drive Letters Before Drive Names",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Displays drive letters (e.g. C:) before the drive name in Explorer. Value 4 = drive letter first. Default: after name (0). Recommended: 4.",
            Tags = ["explorer", "drives", "navigation", "display"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowDriveLettersFirst", 4)],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowDriveLettersFirst", 0),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowDriveLettersFirst", 4),
            ],
        },
        new TweakDef
        {
            Id = "explorer-show-encrypted-color",
            Label = "Show Encrypted/Compressed Files in Color",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Colors encrypted files green and compressed files blue in Explorer. Useful for quickly identifying EFS-encrypted or NTFS-compressed files. Default: no color (0). Recommended: enabled for visibility.",
            Tags = ["explorer", "encrypted", "compressed", "color", "display", "efs"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowEncryptCompressedColor", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowEncryptCompressedColor", 0),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowEncryptCompressedColor", 1),
            ],
        },
        new TweakDef
        {
            Id = "explorer-always-show-icons",
            Label = "Always Show File Icons (Disable Live Thumbnails)",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Forces Explorer to always display file type icons instead of thumbnail previews. Improves performance on slow disks or network shares. Default: thumbnails enabled (0). Recommended: icons-only on slow systems.",
            Tags = ["explorer", "icons", "thumbnails", "performance", "display"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "AlwaysShowIcons", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "AlwaysShowIcons", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "AlwaysShowIcons", 1)],
        },
        new TweakDef
        {
            Id = "explorer-show-empty-drives",
            Label = "Show Empty Removable Drives in This PC",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Shows empty removable drives (USB, optical) in This PC even without media. Default: hidden (1). Recommended: shown when needed.",
            Tags = ["explorer", "drives", "removable", "usb", "display"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "HideDrivesWithNoMedia", 0)],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "HideDrivesWithNoMedia", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "HideDrivesWithNoMedia", 0),
            ],
        },
        // ── Restored stubs with real registry operations ──────────────────

        new TweakDef
        {
            Id = "explorer-ai-thumbnail",
            Label = "Register AI Image PerceivedType",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets PerceivedType=image for .aimodel files so Explorer treats them as image assets for thumbnail generation.",
            Tags = ["explorer", "thumbnail", "ai", "file-type"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.aimodel"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.aimodel", "PerceivedType", "image"),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.aimodel", "ContentType", "application/x-aimodel"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.aimodel", "PerceivedType"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.aimodel", "ContentType"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.aimodel", "PerceivedType", "image")],
        },
        new TweakDef
        {
            Id = "explorer-avif-thumbnail",
            Label = "Register AVIF as Image Type",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets PerceivedType=image and ContentType for .avif so Explorer generates thumbnails.",
            Tags = ["explorer", "thumbnail", "avif", "image"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.avif"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.avif", "PerceivedType", "image"),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.avif", "ContentType", "image/avif"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.avif", "PerceivedType"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.avif", "ContentType"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.avif", "PerceivedType", "image")],
        },
        new TweakDef
        {
            Id = "explorer-eps-thumbnail",
            Label = "Register EPS as Image Type",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets PerceivedType=image and ContentType for .eps so Explorer treats it as an image type.",
            Tags = ["explorer", "thumbnail", "eps", "image"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.eps"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.eps", "PerceivedType", "image"),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.eps", "ContentType", "application/postscript"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.eps", "PerceivedType"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.eps", "ContentType"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.eps", "PerceivedType", "image")],
        },
        new TweakDef
        {
            Id = "explorer-font-thumbnail",
            Label = "Register Font PerceivedType",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets PerceivedType=document for .ttf/.otf so Explorer can show font previews.",
            Tags = ["explorer", "thumbnail", "font", "preview"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.ttf", @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.otf"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.ttf", "PerceivedType", "document"),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.otf", "PerceivedType", "document"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.ttf", "PerceivedType"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.otf", "PerceivedType"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.ttf", "PerceivedType", "document")],
        },
        new TweakDef
        {
            Id = "explorer-heic-thumbnail",
            Label = "Register HEIC as Image Type",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets PerceivedType=image and ContentType for .heic/.heif so Explorer generates thumbnails.",
            Tags = ["explorer", "thumbnail", "heic", "heif", "image"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.heic", @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.heif"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.heic", "PerceivedType", "image"),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.heic", "ContentType", "image/heif"),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.heif", "PerceivedType", "image"),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.heif", "ContentType", "image/heif"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.heic", "PerceivedType"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.heic", "ContentType"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.heif", "PerceivedType"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.heif", "ContentType"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.heic", "PerceivedType", "image")],
        },
        new TweakDef
        {
            Id = "explorer-launch-to-this-pc",
            Label = "Open Explorer to This PC (Policy)",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Forces Explorer to open to 'This PC' via machine-level policy. Complements the per-user LaunchTo setting.",
            Tags = ["explorer", "this-pc", "launch", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "LaunchTo", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "LaunchTo")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "LaunchTo", 1)],
        },
        new TweakDef
        {
            Id = "explorer-pdf-thumbnail",
            Label = "Register PDF as Document Type",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets PerceivedType=document and ContentType for .pdf to enable thumbnail generation.",
            Tags = ["explorer", "thumbnail", "pdf", "document"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.pdf"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.pdf", "PerceivedType", "document"),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.pdf", "ContentType", "application/pdf"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.pdf", "PerceivedType"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.pdf", "ContentType"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.pdf", "PerceivedType", "document")],
        },
        new TweakDef
        {
            Id = "explorer-ps-here",
            Label = "Open PowerShell Here Context Menu",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Adds 'Open PowerShell Here' to the folder background context menu.",
            Tags = ["explorer", "powershell", "context-menu", "shell"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Directory\Background\shell\PowerShellHere"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Directory\Background\shell\PowerShellHere", "", "Open PowerShell Here"),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Directory\Background\shell\PowerShellHere", "Icon", "powershell.exe"),
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Directory\Background\shell\PowerShellHere\command",
                    "",
                    "powershell.exe -NoExit -Command Set-Location -LiteralPath '%V'"
                ),
            ],
            RemoveOps = [RegOp.DeleteTree(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Directory\Background\shell\PowerShellHere")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Directory\Background\shell\PowerShellHere", "", "Open PowerShell Here"),
            ],
        },
        new TweakDef
        {
            Id = "explorer-psd-thumbnail",
            Label = "Register PSD as Image Type",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets PerceivedType=image and ContentType for .psd (Photoshop) files.",
            Tags = ["explorer", "thumbnail", "psd", "photoshop", "image"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.psd"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.psd", "PerceivedType", "image"),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.psd", "ContentType", "image/vnd.adobe.photoshop"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.psd", "PerceivedType"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.psd", "ContentType"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.psd", "PerceivedType", "image")],
        },
        new TweakDef
        {
            Id = "explorer-raw-thumbnail",
            Label = "Register RAW Camera Files as Image",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets PerceivedType=image for common RAW camera formats (.cr2, .nef, .arw, .dng).",
            Tags = ["explorer", "thumbnail", "raw", "camera", "image"],
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.cr2",
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.nef",
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.arw",
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.dng",
            ],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.cr2", "PerceivedType", "image"),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.nef", "PerceivedType", "image"),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.arw", "PerceivedType", "image"),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.dng", "PerceivedType", "image"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.cr2", "PerceivedType"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.nef", "PerceivedType"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.arw", "PerceivedType"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.dng", "PerceivedType"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.cr2", "PerceivedType", "image")],
        },
        new TweakDef
        {
            Id = "explorer-stl-thumbnail",
            Label = "Register STL as 3D Model Type",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets PerceivedType and ContentType for .stl (3D printing) files.",
            Tags = ["explorer", "thumbnail", "stl", "3d", "model"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.stl"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.stl", "PerceivedType", "document"),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.stl", "ContentType", "model/stl"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.stl", "PerceivedType"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.stl", "ContentType"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.stl", "PerceivedType", "document")],
        },
        new TweakDef
        {
            Id = "explorer-svg-thumbnail",
            Label = "Register SVG as Image Type",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets PerceivedType=image and ContentType for .svg so Explorer treats it as an image.",
            Tags = ["explorer", "thumbnail", "svg", "image", "vector"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.svg"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.svg", "PerceivedType", "image"),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.svg", "ContentType", "image/svg+xml"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.svg", "PerceivedType"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.svg", "ContentType"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.svg", "PerceivedType", "image")],
        },
        new TweakDef
        {
            Id = "explorer-webp-thumbnail",
            Label = "Register WebP as Image Type",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets PerceivedType=image and ContentType for .webp so Explorer generates thumbnails.",
            Tags = ["explorer", "thumbnail", "webp", "image"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.webp"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.webp", "PerceivedType", "image"),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.webp", "ContentType", "image/webp"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.webp", "PerceivedType"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.webp", "ContentType"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.webp", "PerceivedType", "image")],
        },
    ];
}

// ── merged from Clipboard.cs ────────────────────────────────────────
internal static class Clipboard
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "clip-increase-drag-threshold",
            Label = "Increase Drag-Drop Threshold (10 px)",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Increases drag start threshold to 10 pixels. Prevents accidental drag on high-DPI screens. Default: 4 pixels. Recommended: 10.",
            Tags = ["clipboard", "drag", "drop", "sensitivity", "dpi"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragWidth", "10"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragHeight", "10"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragWidth", "4"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragHeight", "4"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragWidth", "10")],
        },
        new TweakDef
        {
            Id = "clip-instant-drag-delay",
            Label = "Set Instant Drag Delay (0 ms)",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Removes the 200 ms delay before a drag operation begins. Makes drag-and-drop feel more responsive. Default: 200 ms. Recommended: 0.",
            Tags = ["clipboard", "drag", "delay", "responsiveness"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragDelay", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragDelay", "200")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragDelay", "0")],
        },
        new TweakDef
        {
            Id = "clip-disable-suggested-actions",
            Label = "Disable Clipboard Suggested Actions",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the suggested actions popup that appears when copying phone numbers/dates (Win11 22H2+). Default: enabled. Recommended: 0 (disabled).",
            Tags = ["clipboard", "suggestions", "popup", "win11"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableClipboardSuggestedActions", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableClipboardSuggestedActions")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableClipboardSuggestedActions", 0)],
        },
        new TweakDef
        {
            Id = "clip-disable-clipboard-roaming",
            Label = "Disable Clipboard Roaming (Policy)",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables cross-device clipboard roaming via Group Policy. Prevents clipboard content from syncing across devices. Default: allowed. Recommended: disabled for security.",
            Tags = ["clipboard", "roaming", "policy", "cross-device"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Clipboard", "CloudClipboardAutomaticUpload", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Clipboard", "CloudClipboardAutomaticUpload")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Clipboard", "CloudClipboardAutomaticUpload", 0)],
        },
        new TweakDef
        {
            Id = "clip-disable-drag-full-windows",
            Label = "Disable Full Window Drag",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Shows window outline instead of full content while dragging. Reduces GPU load and improves drag responsiveness. Default: Full window. Recommended: Outline for performance.",
            Tags = ["clipboard", "drag", "window", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragFullWindows", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragFullWindows", "1")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragFullWindows", "0")],
        },
        new TweakDef
        {
            Id = "clip-max-history-items",
            Label = "Increase Clipboard History Limit",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets maximum clipboard history entries to 50 via policy. Allows storing more copied items in clipboard history. Default: 25 items. Recommended: 50 for productivity.",
            Tags = ["clipboard", "history", "limit", "policy", "productivity"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "MaxClipboardHistoryItems", 50)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "MaxClipboardHistoryItems")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "MaxClipboardHistoryItems", 50)],
        },
        new TweakDef
        {
            Id = "clip-disable-clipboard-history",
            Label = "Disable Clipboard History",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows clipboard history via Group Policy. Only the last copied item is kept. Default: user setting.",
            Tags = ["clipboard", "history", "disable", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "AllowClipboardHistory", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "AllowClipboardHistory")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "AllowClipboardHistory", 0)],
        },
        new TweakDef
        {
            Id = "clip-disable-drop-target-hovering",
            Label = "Disable Drop Target Window Activation",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents windows from coming to the foreground when hovering a drag item over a taskbar button. Default: enabled.",
            Tags = ["clipboard", "drag-drop", "window", "activation"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragFromMaximize", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragFromMaximize", "1")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragFromMaximize", "0")],
        },
        new TweakDef
        {
            Id = "clip-disable-clipboard-experience",
            Label = "Disable Clipboard Experience UI",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the Windows 10/11 Clipboard Experience feature (Win+V panel). Falls back to traditional clipboard. Default: enabled.",
            Tags = ["clipboard", "experience", "win-v", "disable"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Clipboard"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Clipboard", "EnableClipboardContentParsing", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Clipboard", "EnableClipboardContentParsing")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Clipboard", "EnableClipboardContentParsing", 0)],
        },
        new TweakDef
        {
            Id = "clip-disable-suggestions",
            Label = "Disable Clipboard Suggestions",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables clipboard content suggestions and recommended actions. Prevents UI pop-ups when copying content. Default: enabled.",
            Tags = ["clipboard", "suggestions", "actions", "disable"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Clipboard"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Clipboard", "EnableSuggestedClipboardActions", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Clipboard", "EnableSuggestedClipboardActions")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Clipboard", "EnableSuggestedClipboardActions", 0)],
        },
        new TweakDef
        {
            Id = "clip-disable-text-suggestions",
            Label = "Disable Text Suggestions (Input)",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables text suggestions and autocomplete for hardware keyboard input. Reduces background processing. Default: enabled.",
            Tags = ["clipboard", "text", "suggestions", "input"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Input\Settings"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Input\Settings", "EnableHwkbTextPrediction", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Input\Settings", "EnableHwkbTextPrediction")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Input\Settings", "EnableHwkbTextPrediction", 0)],
        },
        new TweakDef
        {
            Id = "clip-enable-history-user",
            Label = "Enable Clipboard History (User)",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables the Clipboard History feature at the user level. Allows Win+V to show clipboard history. Default: off.",
            Tags = ["clipboard", "history", "user", "enable"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Clipboard"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Clipboard", "EnableClipboardHistory", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Clipboard", "EnableClipboardHistory", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Clipboard", "EnableClipboardHistory", 1)],
        },
        new TweakDef
        {
            Id = "clip-enable-smart-paste",
            Label = "Enable Smart Paste",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables the Smart Paste feature that intelligently reformats pasted content. Default: off.",
            Tags = ["clipboard", "smart-paste", "formatting", "productivity"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Clipboard"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Clipboard", "EnableSmartPaste", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Clipboard", "EnableSmartPaste", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Clipboard", "EnableSmartPaste", 1)],
        },
        // ── Sprint 47 additions ───────────────────────────────────────────────
        new TweakDef
        {
            Id = "clip-disable-emoji-panel",
            Label = "Disable Emoji Panel",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the emoji panel that appears when pressing Windows+. (period).",
            Tags = ["clipboard", "emoji", "keyboard"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Input\Settings"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Input\Settings", "EnableExpressiveInputShellHotkey", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Input\Settings", "EnableExpressiveInputShellHotkey")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Input\Settings", "EnableExpressiveInputShellHotkey", 0)],
        },
        new TweakDef
        {
            Id = "clip-disable-clipboard-sync-across-devices",
            Label = "Disable Clipboard Sync Across Devices",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows from syncing clipboard data to other signed-in devices via Microsoft account.",
            Tags = ["clipboard", "privacy", "sync"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Clipboard"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Clipboard", "EnableCloudClipboard", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Clipboard", "EnableCloudClipboard")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Clipboard", "EnableCloudClipboard", 0)],
        },
        new TweakDef
        {
            Id = "clip-disable-paste-preview",
            Label = "Disable Clipboard Paste Preview Suggestions",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the paste format suggestion tooltip that appears after pasting content.",
            Tags = ["clipboard", "paste", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Clipboard"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Clipboard", "ShowPasteOptions", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Clipboard", "ShowPasteOptions")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Clipboard", "ShowPasteOptions", 0)],
        },
        new TweakDef
        {
            Id = "clip-disable-snip-sketch-clipboard-auto",
            Label = "Disable Snip & Sketch Auto-Copy to Clipboard",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Snipping Tool / Snip & Sketch from automatically copying screenshots to the clipboard.",
            Tags = ["clipboard", "snip", "screenshot"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\ScreenshotIndex"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\ScreenshotIndex", "AutoCopyToClipboard", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\ScreenshotIndex", "AutoCopyToClipboard"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\ScreenshotIndex", "AutoCopyToClipboard", 0),
            ],
        },
        new TweakDef
        {
            Id = "clip-disable-gif-panel",
            Label = "Disable GIF Panel",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes the GIF search pane that appears in the emoji picker (Windows+. panel).",
            Tags = ["clipboard", "gif", "keyboard"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Input\Settings"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Input\Settings", "EnableGifSearch", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Input\Settings", "EnableGifSearch")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Input\Settings", "EnableGifSearch", 0)],
        },
        new TweakDef
        {
            Id = "clip-disable-clipboard-notifications",
            Label = "Disable Clipboard Copy Notifications",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Suppresses the toast notification that appears after saving a screenshot with Print Screen.",
            Tags = ["clipboard", "notifications"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableSnapAssist", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableSnapAssist")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableSnapAssist", 0)],
        },
        new TweakDef
        {
            Id = "clip-disable-sticker-panel",
            Label = "Disable Sticker / Meme Panel",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes the sticker and meme categories from the emoji picker panel.",
            Tags = ["clipboard", "stickers", "keyboard"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Input\Settings"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Input\Settings", "EnableStickerPanel", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Input\Settings", "EnableStickerPanel")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Input\Settings", "EnableStickerPanel", 0)],
        },
        new TweakDef
        {
            Id = "clip-disable-cloud-clipboard-prompt",
            Label = "Disable Cloud Clipboard Activation Prompt",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows from asking you to enable cloud clipboard (cross-device sync) in Windows 10/11.",
            Tags = ["clipboard", "cloud", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Clipboard"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Clipboard", "SessionWistenessConsent", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Clipboard", "SessionWistenessConsent")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Clipboard", "SessionWistenessConsent", 1)],
        },
        new TweakDef
        {
            Id = "clip-disable-typing-insights",
            Label = "Disable Typing Insights Collection",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Windows collecting typing and touch keyboard usage patterns for improvement feedback.",
            Tags = ["clipboard", "privacy", "keyboard", "typing"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Input\TIPC"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Input\TIPC", "Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Input\TIPC", "Enabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Input\TIPC", "Enabled", 0)],
        },
        // ── merged from: ContextMenu.cs ──────────────────────────────────────────────────
        new TweakDef
        {
            Id = "ctx-add-powershell-here",
            Label = "Add 'Open PowerShell Here' to Context Menu",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Adds an 'Open PowerShell Here' entry to the directory background context menu for quick terminal access. Default: not present. Recommended: add for power users.",
            Tags = ["context-menu", "powershell", "terminal", "productivity"],
            RegistryKeys =
            [
                @"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\PowerShellHere",
                @"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\PowerShellHere\command",
            ],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\PowerShellHere", "Icon", "powershell.exe")],
            RemoveOps = [RegOp.DeleteTree(@"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\PowerShellHere")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\PowerShellHere", "Icon", "powershell.exe"),
            ],
        },
        new TweakDef
        {
            Id = "ctx-add-wt-here",
            Label = "Add 'Open Windows Terminal Here' to Context Menu",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Adds 'Open Windows Terminal Here' to the folder background context menu. Opens a new terminal window in the current directory. Default: not present. Recommended: add for developer workflows.",
            Tags = ["context-menu", "terminal", "wt", "developer", "shell"],
            RegistryKeys =
            [
                @"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\wt",
                @"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\wt\command",
            ],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\wt", "Icon", "wt.exe")],
            RemoveOps = [RegOp.DeleteTree(@"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\wt")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\wt", "Icon", "wt.exe")],
        },
        new TweakDef
        {
            Id = "ctx-add-cmd-here",
            Label = "Add 'Open Command Prompt Here' to Context Menu",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Adds 'Open Command Prompt Here' to the folder background context menu. Opens cmd.exe in the current directory. Default: not present. Recommended: add for quick access.",
            Tags = ["context-menu", "cmd", "command-prompt", "shell", "developer"],
            RegistryKeys =
            [
                @"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\cmd",
                @"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\cmd\command",
            ],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\cmd", "Icon", "cmd.exe")],
            RemoveOps = [RegOp.DeleteTree(@"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\cmd")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\cmd", "Icon", "cmd.exe")],
        },
        new TweakDef
        {
            Id = "ctx-add-copy-path",
            Label = "Add 'Copy Path' to File Context Menu",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Adds a 'Copy Path' option to the right-click menu for all files. Copies the full file path (with quotes) to the clipboard. Default: not present. Recommended: add for power users.",
            Tags = ["context-menu", "copy-path", "clipboard", "file", "productivity"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Classes\*\shell\CopyPath", @"HKEY_CURRENT_USER\Software\Classes\*\shell\CopyPath\command"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Software\Classes\*\shell\CopyPath", "Icon", "shell32.dll,-265")],
            RemoveOps = [RegOp.DeleteTree(@"HKEY_CURRENT_USER\Software\Classes\*\shell\CopyPath")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Software\Classes\*\shell\CopyPath", "Icon", "shell32.dll,-265")],
        },
        new TweakDef
        {
            Id = "ctx-add-open-cmd-here",
            Label = "Add Open Command Prompt Here",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Adds an 'Open Command Prompt Here' option to the folder context menu. Default: removed in Win11.",
            Tags = ["context-menu", "command-prompt", "cmd", "terminal"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\cmd_here"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\cmd_here", "", "Open Command Prompt Here"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\cmd_here", "Icon", "cmd.exe"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\cmd_here\command", "", "cmd.exe /k cd /d \"%V\""),
            ],
            RemoveOps = [RegOp.DeleteTree(@"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\cmd_here")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\cmd_here", "", "Open Command Prompt Here"),
            ],
        },
        new TweakDef
        {
            Id = "ctx-remove-share-context-menu",
            Label = "Remove Share Context Menu",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes the 'Share' option from the right-click context menu. Default: visible.",
            Tags = ["context-menu", "share", "remove"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked",
                    "{E2BF9676-5F8F-435C-97EB-11607A5BEDF7}",
                    ""
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked",
                    "{E2BF9676-5F8F-435C-97EB-11607A5BEDF7}"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked",
                    "{E2BF9676-5F8F-435C-97EB-11607A5BEDF7}",
                    ""
                ),
            ],
        },
        new TweakDef
        {
            Id = "ctx-remove-give-access-to",
            Label = "Remove Give Access To Menu",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes the 'Give access to' sharing option from the context menu. Default: visible.",
            Tags = ["context-menu", "give-access", "sharing", "remove"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked",
                    "{F81E9010-6EA4-11CE-A7FF-00AA003CA9F6}",
                    ""
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked",
                    "{F81E9010-6EA4-11CE-A7FF-00AA003CA9F6}"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked",
                    "{F81E9010-6EA4-11CE-A7FF-00AA003CA9F6}",
                    ""
                ),
            ],
        },
        new TweakDef
        {
            Id = "ctx-remove-troubleshoot-compatibility",
            Label = "Remove 'Troubleshoot Compatibility' from Context Menu",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes the 'Troubleshoot compatibility' option from file context menus. Default: shown.",
            Tags = ["context-menu", "troubleshoot", "compatibility", "remove"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked",
                    "{1d27f844-3a1f-4410-85ac-14651078412d}",
                    ""
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked",
                    "{1d27f844-3a1f-4410-85ac-14651078412d}"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked",
                    "{1d27f844-3a1f-4410-85ac-14651078412d}",
                    ""
                ),
            ],
        },
        new TweakDef
        {
            Id = "ctx-remove-cast-to-device",
            Label = "Remove 'Cast to Device' from Context Menu",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes the 'Cast to device' (Play To) option from the context menu. Default: shown.",
            Tags = ["context-menu", "cast", "device", "remove"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked",
                    "{7AD84985-87B4-4a16-BE58-8B72A5B390F7}",
                    ""
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked",
                    "{7AD84985-87B4-4a16-BE58-8B72A5B390F7}"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked",
                    "{7AD84985-87B4-4a16-BE58-8B72A5B390F7}",
                    ""
                ),
            ],
        },
        new TweakDef
        {
            Id = "ctx-remove-edit-with-paint3d",
            Label = "Remove 'Edit with Paint 3D' from Context Menu",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes 'Edit with Paint 3D' from the right-click menu for image files. Default: shown.",
            Tags = ["context-menu", "paint3d", "image", "remove"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked",
                    "{D2B1A1A9-0B2A-4ED6-B4D7-679E2A5D2B8E}",
                    ""
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked",
                    "{D2B1A1A9-0B2A-4ED6-B4D7-679E2A5D2B8E}"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked",
                    "{D2B1A1A9-0B2A-4ED6-B4D7-679E2A5D2B8E}",
                    ""
                ),
            ],
        },
        new TweakDef
        {
            Id = "ctx-remove-include-in-library",
            Label = "Remove 'Include in Library' from Context Menu",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes the 'Include in library' option from folder context menus. Default: shown.",
            Tags = ["context-menu", "library", "folder", "remove"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked",
                    "{3dad6c5d-2167-4cae-9914-f99e41c12cfa}",
                    ""
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked",
                    "{3dad6c5d-2167-4cae-9914-f99e41c12cfa}"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked",
                    "{3dad6c5d-2167-4cae-9914-f99e41c12cfa}",
                    ""
                ),
            ],
        },
        new TweakDef
        {
            Id = "ctx-remove-previous-versions",
            Label = "Remove 'Restore Previous Versions' from Context Menu",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes the 'Restore previous versions' tab from file/folder properties. Default: shown.",
            Tags = ["context-menu", "previous-versions", "restore", "remove"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked",
                    "{596AB062-B4D2-4215-9F74-E9109B0A8153}",
                    ""
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked",
                    "{596AB062-B4D2-4215-9F74-E9109B0A8153}"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked",
                    "{596AB062-B4D2-4215-9F74-E9109B0A8153}",
                    ""
                ),
            ],
        },
        new TweakDef
        {
            Id = "ctx-add-take-ownership",
            Label = "Add 'Take Ownership' to Context Menu",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = false,
            Description =
                "Adds a 'Take Ownership' option to the file/folder context menu. Runs takeown + icacls to grant full permissions. Default: not present.",
            Tags = ["context-menu", "take-ownership", "permissions", "security"],
            RegistryKeys =
            [
                @"HKEY_CURRENT_USER\Software\Classes\*\shell\TakeOwnership",
                @"HKEY_CURRENT_USER\Software\Classes\Directory\shell\TakeOwnership",
            ],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\Classes\*\shell\TakeOwnership", "", "Take Ownership"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\Classes\*\shell\TakeOwnership", "Icon", "shield.exe"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\Classes\*\shell\TakeOwnership", "HasLUAShield", ""),
                RegOp.SetString(
                    @"HKEY_CURRENT_USER\Software\Classes\*\shell\TakeOwnership\command",
                    "",
                    @"cmd.exe /c takeown /f ""%1"" && icacls ""%1"" /grant administrators:F"
                ),
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\Classes\Directory\shell\TakeOwnership", "", "Take Ownership"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\Classes\Directory\shell\TakeOwnership", "Icon", "shield.exe"),
                RegOp.SetString(
                    @"HKEY_CURRENT_USER\Software\Classes\Directory\shell\TakeOwnership\command",
                    "",
                    @"cmd.exe /c takeown /f ""%1"" /r /d y && icacls ""%1"" /grant administrators:F /t"
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteTree(@"HKEY_CURRENT_USER\Software\Classes\*\shell\TakeOwnership"),
                RegOp.DeleteTree(@"HKEY_CURRENT_USER\Software\Classes\Directory\shell\TakeOwnership"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Software\Classes\*\shell\TakeOwnership", "", "Take Ownership")],
        },
        new TweakDef
        {
            Id = "ctx-add-open-with-notepad",
            Label = "Add 'Open with Notepad' to Context Menu",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Adds 'Open with Notepad' to the right-click menu for all files. Quick way to view file contents. Default: not present.",
            Tags = ["context-menu", "notepad", "open-with", "text"],
            RegistryKeys =
            [
                @"HKEY_CURRENT_USER\Software\Classes\*\shell\OpenWithNotepad",
                @"HKEY_CURRENT_USER\Software\Classes\*\shell\OpenWithNotepad\command",
            ],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\Classes\*\shell\OpenWithNotepad", "", "Open with Notepad"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\Classes\*\shell\OpenWithNotepad", "Icon", "notepad.exe"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\Classes\*\shell\OpenWithNotepad\command", "", @"notepad.exe ""%1"""),
            ],
            RemoveOps = [RegOp.DeleteTree(@"HKEY_CURRENT_USER\Software\Classes\*\shell\OpenWithNotepad")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Software\Classes\*\shell\OpenWithNotepad", "", "Open with Notepad")],
        },
        // ── Restored stubs with real registry operations ──────────────────

        new TweakDef
        {
            Id = "ctx-classic-context-menu",
            Label = "Force Classic Context Menu (User)",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Restores the Windows 10 full context menu by overriding the Windows 11 compact menu CLSID.",
            Tags = ["context-menu", "classic", "win11", "right-click"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Classes\CLSID\{86ca1aa0-34aa-4e8b-a509-50c905bae2a2}\InprocServer32"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Software\Classes\CLSID\{86ca1aa0-34aa-4e8b-a509-50c905bae2a2}\InprocServer32", "", "")],
            RemoveOps = [RegOp.DeleteTree(@"HKEY_CURRENT_USER\Software\Classes\CLSID\{86ca1aa0-34aa-4e8b-a509-50c905bae2a2}")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_CURRENT_USER\Software\Classes\CLSID\{86ca1aa0-34aa-4e8b-a509-50c905bae2a2}\InprocServer32", "", ""),
            ],
        },
        new TweakDef
        {
            Id = "ctx-remove-defender-scan",
            Label = "Remove 'Scan with Defender' Context",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Removes the 'Scan with Microsoft Defender' entry from the right-click context menu.",
            Tags = ["context-menu", "defender", "security", "clean"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\CLSID\{09A47860-11B0-4DA5-AFA5-26D86198A780}\InprocServer32"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\CLSID\{09A47860-11B0-4DA5-AFA5-26D86198A780}\InprocServer32", "", "")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\CLSID\{09A47860-11B0-4DA5-AFA5-26D86198A780}\InprocServer32", "")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\CLSID\{09A47860-11B0-4DA5-AFA5-26D86198A780}\InprocServer32", "", ""),
            ],
        },
        new TweakDef
        {
            Id = "ctx-remove-give-access",
            Label = "Remove 'Give Access To' Context",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes the 'Give access to' sharing menu from the right-click context menu.",
            Tags = ["context-menu", "give-access", "sharing", "clean"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\*\shellex\ContextMenuHandlers\Sharing"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\*\shellex\ContextMenuHandlers\Sharing", "", "")],
            RemoveOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\*\shellex\ContextMenuHandlers\Sharing",
                    "",
                    "{f81e9010-6ea4-11ce-a7ff-00aa003ca9f6}"
                ),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\*\shellex\ContextMenuHandlers\Sharing", "", "")],
        },
        new TweakDef
        {
            Id = "ctx-remove-include-library",
            Label = "Remove 'Include in Library' Context",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes the 'Include in library' entry from the folder context menu.",
            Tags = ["context-menu", "library", "folder", "clean"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Folder\shellex\ContextMenuHandlers\Library Location"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Folder\shellex\ContextMenuHandlers\Library Location", "", "")],
            RemoveOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Folder\shellex\ContextMenuHandlers\Library Location",
                    "",
                    "{3dad6c5d-2167-4cae-9914-f99e41c12cfa}"
                ),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Folder\shellex\ContextMenuHandlers\Library Location", "", "")],
        },
        new TweakDef
        {
            Id = "ctx-remove-paint3d-edit",
            Label = "Remove 'Edit with Paint 3D' Context",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes the 'Edit with Paint 3D' option from image file context menus.",
            Tags = ["context-menu", "paint-3d", "image", "clean"],
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\SystemFileAssociations\.bmp\Shell\3D Edit",
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\SystemFileAssociations\.jpg\Shell\3D Edit",
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\SystemFileAssociations\.png\Shell\3D Edit",
            ],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\SystemFileAssociations\.bmp\Shell\3D Edit", "ProgrammaticAccessOnly", ""),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\SystemFileAssociations\.jpg\Shell\3D Edit", "ProgrammaticAccessOnly", ""),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\SystemFileAssociations\.png\Shell\3D Edit", "ProgrammaticAccessOnly", ""),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\SystemFileAssociations\.bmp\Shell\3D Edit", "ProgrammaticAccessOnly"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\SystemFileAssociations\.jpg\Shell\3D Edit", "ProgrammaticAccessOnly"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\SystemFileAssociations\.png\Shell\3D Edit", "ProgrammaticAccessOnly"),
            ],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\SystemFileAssociations\.bmp\Shell\3D Edit", "ProgrammaticAccessOnly", ""),
            ],
        },
        new TweakDef
        {
            Id = "ctx-remove-photos-edit",
            Label = "Remove 'Edit with Photos' Context",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes the 'Edit with Photos' option from image file context menus.",
            Tags = ["context-menu", "photos", "image", "clean"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\AppX43hnxtbyyps62jhe9sqpdzxn1790zetc\Shell\ShellEdit"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\AppX43hnxtbyyps62jhe9sqpdzxn1790zetc\Shell\ShellEdit",
                    "ProgrammaticAccessOnly",
                    ""
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\AppX43hnxtbyyps62jhe9sqpdzxn1790zetc\Shell\ShellEdit",
                    "ProgrammaticAccessOnly"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\AppX43hnxtbyyps62jhe9sqpdzxn1790zetc\Shell\ShellEdit",
                    "ProgrammaticAccessOnly",
                    ""
                ),
            ],
        },
        new TweakDef
        {
            Id = "ctx-remove-pin-to-start",
            Label = "Remove 'Pin to Start' Context Entry",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes the 'Pin to Start' context menu option from files and folders.",
            Tags = ["context-menu", "pin", "start-menu", "clean"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Folder\shellex\ContextMenuHandlers\PintoStartScreen"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Folder\shellex\ContextMenuHandlers\PintoStartScreen", "", "")],
            RemoveOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Folder\shellex\ContextMenuHandlers\PintoStartScreen",
                    "",
                    "{470C0EBD-5D73-4d58-9CED-E91E22E23282}"
                ),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Folder\shellex\ContextMenuHandlers\PintoStartScreen", "", "")],
        },
        new TweakDef
        {
            Id = "ctx-remove-print",
            Label = "Remove 'Print' Context Menu Entry",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes the 'Print' entry from the right-click context menu for supported file types.",
            Tags = ["context-menu", "print", "clean"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\SystemFileAssociations\image\shell\print"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\SystemFileAssociations\image\shell\print", "ProgrammaticAccessOnly", ""),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\SystemFileAssociations\image\shell\print", "ProgrammaticAccessOnly"),
            ],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\SystemFileAssociations\image\shell\print", "ProgrammaticAccessOnly", ""),
            ],
        },
        new TweakDef
        {
            Id = "ctx-remove-send-to",
            Label = "Remove 'Send To' Context Menu",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes the 'Send to' cascading menu from the right-click context menu.",
            Tags = ["context-menu", "send-to", "clean"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\AllFilesystemObjects\shellex\ContextMenuHandlers\SendTo"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\AllFilesystemObjects\shellex\ContextMenuHandlers\SendTo", "", "")],
            RemoveOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\AllFilesystemObjects\shellex\ContextMenuHandlers\SendTo",
                    "",
                    "{7BA4C740-9E81-11CF-99D3-00AA004AE837}"
                ),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\AllFilesystemObjects\shellex\ContextMenuHandlers\SendTo", "", "")],
        },
        new TweakDef
        {
            Id = "ctx-remove-share",
            Label = "Remove 'Share' Context Menu Entry",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes the 'Share' modern sharing entry from the right-click context menu.",
            Tags = ["context-menu", "share", "modern", "clean"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Classes\*\shellex\ContextMenuHandlers\ModernSharing"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Software\Classes\*\shellex\ContextMenuHandlers\ModernSharing", "", "")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Classes\*\shellex\ContextMenuHandlers\ModernSharing", "")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Software\Classes\*\shellex\ContextMenuHandlers\ModernSharing", "", "")],
        },
        new TweakDef
        {
            Id = "ctx-remove-troubleshoot-compat",
            Label = "Remove 'Troubleshoot Compatibility'",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes the 'Troubleshoot compatibility' entry from the context menu for executables.",
            Tags = ["context-menu", "troubleshoot", "compatibility", "clean"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\exefile\shellex\ContextMenuHandlers\Compatibility"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\exefile\shellex\ContextMenuHandlers\Compatibility", "", "")],
            RemoveOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\exefile\shellex\ContextMenuHandlers\Compatibility",
                    "",
                    "{1d27f844-3a1f-4410-85ac-14651078412d}"
                ),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\exefile\shellex\ContextMenuHandlers\Compatibility", "", "")],
        },
        new TweakDef
        {
            Id = "ctx-remove-wmp-context",
            Label = "Remove Windows Media Context Entries",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes Windows Media Player context menu entries (Play, Add to playlist, etc.).",
            Tags = ["context-menu", "media-player", "wmp", "clean"],
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\SystemFileAssociations\audio\shell\Enqueue",
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\SystemFileAssociations\audio\shell\Play",
            ],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\SystemFileAssociations\audio\shell\Enqueue", "ProgrammaticAccessOnly", ""),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\SystemFileAssociations\audio\shell\Play", "ProgrammaticAccessOnly", ""),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\SystemFileAssociations\audio\shell\Enqueue", "ProgrammaticAccessOnly"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\SystemFileAssociations\audio\shell\Play", "ProgrammaticAccessOnly"),
            ],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\SystemFileAssociations\audio\shell\Enqueue", "ProgrammaticAccessOnly", ""),
            ],
        },
    ];
}

// === Merged from: Shell.cs ===

internal static class Shell
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "shell-set-console-buffer-9999",
            Label = "Set Console Screen Buffer to 9999 Lines",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets the console screen buffer size to 9999 lines. More scrollback history. Default: 300.",
            Tags = ["shell", "console", "buffer", "scrollback"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "ScreenBufferSize", 655279999)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "ScreenBufferSize", 19660920)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "ScreenBufferSize", 655279999)],
        },
        new TweakDef
        {
            Id = "shell-disable-cmd-autorun",
            Label = "Disable CMD AutoRun Commands",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Clears the CMD AutoRun registry value. Prevents potentially malicious auto-execution. Default: not set.",
            Tags = ["shell", "cmd", "autorun", "security"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Command Processor"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Software\Microsoft\Command Processor", "AutoRun", "")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Command Processor", "AutoRun")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Software\Microsoft\Command Processor", "AutoRun", "")],
        },
        new TweakDef
        {
            Id = "shell-set-powershell-execution-remotesigned",
            Label = "Set PowerShell Execution Policy to RemoteSigned",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the machine-level PowerShell execution policy to RemoteSigned. Local scripts run freely; downloaded scripts need a signature. Default: Restricted.",
            Tags = ["shell", "powershell", "execution-policy", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PowerShell\1\ShellIds\Microsoft.PowerShell"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PowerShell\1\ShellIds\Microsoft.PowerShell",
                    "ExecutionPolicy",
                    "RemoteSigned"
                ),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PowerShell\1\ShellIds\Microsoft.PowerShell", "ExecutionPolicy", "Restricted"),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PowerShell\1\ShellIds\Microsoft.PowerShell",
                    "ExecutionPolicy",
                    "RemoteSigned"
                ),
            ],
        },
        new TweakDef
        {
            Id = "shell-add-python-to-path",
            Label = "Add Python App Installer to PATH",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Adds the WindowsApps Python alias directory to the user PATH. Enables running 'python' from any terminal without the Store alias redirect. Default: not in PATH.",
            Tags = ["shell", "python", "path", "environment"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
            ApplyOps = [RegOp.SetExpandString(@"HKEY_CURRENT_USER\Environment", "Path", @"%LOCALAPPDATA%\Microsoft\WindowsApps;%PATH%")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Environment", "Path")],
            DetectAction = () =>
            {
                var path = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.User) ?? "";
                return path.Contains("WindowsApps", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "shell-classic-context-menu",
            Label = "Restore Classic Context Menu (Shell)",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Restores the classic full right-click context menu by overriding the Windows 11 modern context menu via shell registry key. Default: modern menu.",
            Tags = ["shell", "context-menu", "classic", "win11"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Classes\CLSID\{86ca1aa0-34aa-4e8b-a509-50c905bae2a2}\InprocServer32"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Software\Classes\CLSID\{86ca1aa0-34aa-4e8b-a509-50c905bae2a2}\InprocServer32", "", "")],
            RemoveOps = [RegOp.DeleteTree(@"HKEY_CURRENT_USER\Software\Classes\CLSID\{86ca1aa0-34aa-4e8b-a509-50c905bae2a2}")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_CURRENT_USER\Software\Classes\CLSID\{86ca1aa0-34aa-4e8b-a509-50c905bae2a2}\InprocServer32", "", ""),
            ],
        },
        new TweakDef
        {
            Id = "shell-cmd-autocomplete",
            Label = "Enable Command Prompt AutoComplete",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Enables Tab-key autocomplete in Command Prompt (cmd.exe). Sets CompletionChar and PathCompletionChar to Tab (0x9). Default: disabled.",
            Tags = ["shell", "cmd", "autocomplete", "tab"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Command Processor"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Command Processor", "CompletionChar", 0x9),
                RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Command Processor", "PathCompletionChar", 0x9),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Command Processor", "CompletionChar", 0x40),
                RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Command Processor", "PathCompletionChar", 0x40),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Command Processor", "CompletionChar", 0x9)],
        },
        new TweakDef
        {
            Id = "shell-disable-python-store-alias",
            Label = "Disable Python Store Redirect Alias",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the Windows Store app execution alias for 'python.exe' and 'python3.exe'. Prevents the Store redirect when Python is already installed. Default: enabled.",
            Tags = ["shell", "python", "store", "alias"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\python.exe"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\SOFTWARE\Classes\Local Settings\Software\Microsoft\Windows\CurrentVersion\AppModel\PackageRepository\Extensions\ProgIDs\AppX9rkaq77s0jzh1tyccadx9ghba15r6t3h",
                    "Disabled",
                    1
                ),
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\SOFTWARE\Classes\Local Settings\Software\Microsoft\Windows\CurrentVersion\AppModel\PackageRepository\Extensions\ProgIDs\AppXdfn65rtf6m01bfv5r26cj8xtf0jclb0p",
                    "Disabled",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\SOFTWARE\Classes\Local Settings\Software\Microsoft\Windows\CurrentVersion\AppModel\PackageRepository\Extensions\ProgIDs\AppX9rkaq77s0jzh1tyccadx9ghba15r6t3h",
                    "Disabled"
                ),
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\SOFTWARE\Classes\Local Settings\Software\Microsoft\Windows\CurrentVersion\AppModel\PackageRepository\Extensions\ProgIDs\AppXdfn65rtf6m01bfv5r26cj8xtf0jclb0p",
                    "Disabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\SOFTWARE\Classes\Local Settings\Software\Microsoft\Windows\CurrentVersion\AppModel\PackageRepository\Extensions\ProgIDs\AppX9rkaq77s0jzh1tyccadx9ghba15r6t3h",
                    "Disabled",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "shell-file-hash-context",
            Label = "Add File Hash to Context Menu",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Adds a 'Calculate File Hash' option to the right-click context menu using PowerShell's Get-FileHash. Default: not available.",
            Tags = ["shell", "hash", "context-menu", "powershell"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\*\shell\GetFileHash"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\*\shell\GetFileHash", "", "Calculate File Hash"),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\*\shell\GetFileHash", "Icon", "shell32.dll,23"),
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\*\shell\GetFileHash\command",
                    "",
                    @"powershell.exe -NoProfile -Command ""Get-FileHash -Algorithm SHA256 '%1' | Format-List; pause"""
                ),
            ],
            RemoveOps = [RegOp.DeleteTree(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\*\shell\GetFileHash")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\*\shell\GetFileHash", "", "Calculate File Hash")],
        },
        new TweakDef
        {
            Id = "shell-open-cmd-here",
            Label = "Add 'Open CMD Here' to Context Menu",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Adds 'Open Command Prompt Here' to the directory background context menu. Default: not available.",
            Tags = ["shell", "cmd", "context-menu", "directory"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Directory\Background\shell\OpenCmdHere"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Directory\Background\shell\OpenCmdHere", "", "Open Command Prompt Here"),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Directory\Background\shell\OpenCmdHere", "Icon", "cmd.exe"),
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Directory\Background\shell\OpenCmdHere\command",
                    "",
                    @"cmd.exe /k cd /d ""%V"""
                ),
            ],
            RemoveOps = [RegOp.DeleteTree(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Directory\Background\shell\OpenCmdHere")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Directory\Background\shell\OpenCmdHere", "", "Open Command Prompt Here"),
            ],
        },
        new TweakDef
        {
            Id = "shell-open-ps-here",
            Label = "Add 'Open PowerShell Here' to Context Menu",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Adds 'Open PowerShell Here' to the directory background context menu. Default: not available.",
            Tags = ["shell", "powershell", "context-menu", "directory"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Directory\Background\shell\OpenPSHere"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Directory\Background\shell\OpenPSHere", "", "Open PowerShell Here"),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Directory\Background\shell\OpenPSHere", "Icon", "powershell.exe"),
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Directory\Background\shell\OpenPSHere\command",
                    "",
                    @"powershell.exe -NoExit -Command ""Set-Location '%V'"""
                ),
            ],
            RemoveOps = [RegOp.DeleteTree(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Directory\Background\shell\OpenPSHere")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Directory\Background\shell\OpenPSHere", "", "Open PowerShell Here")],
        },
        new TweakDef
        {
            Id = "shell-open-wt-here",
            Label = "Add 'Open Windows Terminal Here'",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Adds 'Open Windows Terminal Here' to the directory background context menu. Requires Windows Terminal to be installed. Default: not available.",
            Tags = ["shell", "terminal", "context-menu", "directory"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Directory\Background\shell\OpenWTHere"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Directory\Background\shell\OpenWTHere", "", "Open Windows Terminal Here"),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Directory\Background\shell\OpenWTHere", "Icon", "wt.exe"),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Directory\Background\shell\OpenWTHere\command", "", @"wt.exe -d ""%V"""),
            ],
            RemoveOps = [RegOp.DeleteTree(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Directory\Background\shell\OpenWTHere")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Directory\Background\shell\OpenWTHere", "", "Open Windows Terminal Here"),
            ],
        },
        new TweakDef
        {
            Id = "shell-take-ownership",
            Label = "Add 'Take Ownership' to Context Menu",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Adds a 'Take Ownership' option to the right-click context menu for files and folders. Uses takeown and icacls to reclaim NTFS permissions. Default: not available.",
            Tags = ["shell", "ownership", "context-menu", "permissions"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\*\shell\TakeOwnership"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\*\shell\TakeOwnership", "", "Take Ownership"),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\*\shell\TakeOwnership", "Icon", "imageres.dll,101"),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\*\shell\TakeOwnership", "HasLUAShield", ""),
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\*\shell\TakeOwnership\command",
                    "",
                    @"cmd.exe /c takeown /f ""%1"" && icacls ""%1"" /grant administrators:F"
                ),
            ],
            RemoveOps = [RegOp.DeleteTree(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\*\shell\TakeOwnership")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\*\shell\TakeOwnership", "", "Take Ownership")],
        },
        new TweakDef
        {
            Id = "shell-enable-numlock-startup",
            Label = "Enable Num Lock on Startup",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets InitialKeyboardIndicators=2 in the Keyboard control panel key. Ensures Num Lock is enabled each time the desktop session starts.",
            Tags = ["shell", "keyboard", "numlock", "startup"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Keyboard"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Keyboard", "InitialKeyboardIndicators", "2")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Keyboard", "InitialKeyboardIndicators", "0")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Keyboard", "InitialKeyboardIndicators", "2")],
        },
        new TweakDef
        {
            Id = "shell-restore-previous-folders",
            Label = "Reopen Previous Folder Windows on Login",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets PersistBrowsers=1 in Explorer Advanced. Restores all previously open Explorer folder windows after a reboot or sign-in, resuming your workspace automatically.",
            Tags = ["shell", "explorer", "folders", "restore"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "PersistBrowsers", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "PersistBrowsers", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "PersistBrowsers", 1)],
        },
        new TweakDef
        {
            Id = "shell-disable-recent-docs-policy",
            Label = "Disable Recent Documents Tracking (Policy)",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets NoRecentDocsHistory=1 in Explorer policies. Prevents Windows from recording history of recently opened files in the shell across all users on the machine via Group Policy.",
            Tags = ["shell", "privacy", "recent-docs", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoRecentDocsHistory", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoRecentDocsHistory")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoRecentDocsHistory", 1),
            ],
        },
    ];
}

internal static class Performance
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "perf-disable-search-protocol-host",
            Label = "Disable SearchProtocolHost Priority Boost",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables SearchProtocolHost priority boost to reduce background CPU usage from Windows Search indexing.",
            Tags = ["performance", "search", "indexing"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Search", "SetupCompletedSuccessfully", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Search", "SetupCompletedSuccessfully", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Search", "SetupCompletedSuccessfully", 0)],
        },
        new TweakDef
        {
            Id = "perf-optimize-processor-scheduling",
            Label = "Optimize for Programs (Not Services)",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            ImpactNote = "Boosts foreground app CPU priority; noticeably snappier UI response and lower input latency.",
            Description =
                "Sets Win32PrioritySeparation to 38 (0x26): short variable quantum with maximum foreground boost. Prioritizes interactive desktop apps over background services and increases scheduler responsiveness. Default: 2. Recommended: 38 for desktops, 2 for servers.",
            Tags = ["performance", "cpu", "scheduling", "responsiveness", "priority", "foreground", "quantum"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl", "Win32PrioritySeparation", 38)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl", "Win32PrioritySeparation", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl", "Win32PrioritySeparation", 38)],
        },
        new TweakDef
        {
            Id = "perf-disable-ntfs-encryption",
            Label = "Disable NTFS Encryption (EFS) Service",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables NTFS Encrypting File System to reduce filesystem overhead. Not recommended if EFS encryption is in use.",
            Tags = ["performance", "ntfs", "encryption", "filesystem"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableEncryption", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableEncryption", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableEncryption", 1)],
        },
        new TweakDef
        {
            Id = "perf-unpark-cpu-cores",
            Label = "Unpark All CPU Cores",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables CPU core parking so all cores remain active at all times. Reduces latency spikes in real-time and gaming workloads. Default: Windows-managed. Recommended: disabled for desktops and gaming rigs.",
            Tags = ["performance", "cpu", "core-parking", "latency", "gaming"],
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\54533251-82be-4824-96c1-47b60b740d00\0cc5b647-c1df-4637-891a-dec35c318583",
            ],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\54533251-82be-4824-96c1-47b60b740d00\0cc5b647-c1df-4637-891a-dec35c318583",
                    "ValueMax",
                    0
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\54533251-82be-4824-96c1-47b60b740d00\0cc5b647-c1df-4637-891a-dec35c318583",
                    "ValueMin",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\54533251-82be-4824-96c1-47b60b740d00\0cc5b647-c1df-4637-891a-dec35c318583",
                    "ValueMax"
                ),
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\54533251-82be-4824-96c1-47b60b740d00\0cc5b647-c1df-4637-891a-dec35c318583",
                    "ValueMin"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\54533251-82be-4824-96c1-47b60b740d00\0cc5b647-c1df-4637-891a-dec35c318583",
                    "ValueMax",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "perf-disable-modern-standby",
            Label = "Disable Modern Standby (S0)",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables Modern Standby (S0 Low Power Idle) and restores classic S3 sleep. Prevents wake-from-sleep issues and battery drain. Default: Modern Standby. Recommended: disabled on desktops.",
            Tags = ["performance", "standby", "sleep", "s3", "power"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "PlatformAoAcOverride", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "PlatformAoAcOverride")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "PlatformAoAcOverride", 0)],
        },
        new TweakDef
        {
            Id = "perf-disable-memory-compression",
            Label = "Disable Memory Page Combining",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables memory page combining (compression) to reduce CPU overhead on systems with ample RAM. Default: Enabled. Recommended: Disabled on 16 GB+ systems.",
            Tags = ["performance", "memory", "compression", "page-combining"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "DisablePageCombining", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "DisablePageCombining"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "DisablePageCombining", 1),
            ],
        },
        new TweakDef
        {
            Id = "perf-always-unload-dll",
            Label = "Always Unload DLLs on Process Exit",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Forces Windows to immediately unload unused DLLs from memory when processes exit. Frees RAM faster and reduces memory fragmentation. Default: Not set. Recommended: Enabled.",
            Tags = ["performance", "dll", "memory", "unload", "ram"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "AlwaysUnloadDLL", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "AlwaysUnloadDLL")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "AlwaysUnloadDLL", 1)],
        },
        new TweakDef
        {
            Id = "perf-increase-icon-cache",
            Label = "Increase Explorer Icon Cache Size",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Increases Explorer's icon cache to 4096 entries. Reduces icon reloading delays when switching between folders with many files. Default: 500. Recommended: 4096 for large libraries.",
            Tags = ["performance", "explorer", "icon", "cache", "ui"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "Max Cached Icons", "4096")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "Max Cached Icons")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "Max Cached Icons", "4096")],
        },
        new TweakDef
        {
            Id = "perf-disable-thumbnails-network",
            Label = "Disable Thumbnails on Network Folders",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables thumbnail generation for files on network folders. Prevents slow Explorer loading when browsing network shares. Default: enabled.",
            Tags = ["performance", "thumbnails", "network", "explorer"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced",
                    "DisableThumbnailsOnNetworkFolders",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced",
                    "DisableThumbnailsOnNetworkFolders"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced",
                    "DisableThumbnailsOnNetworkFolders",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "perf-large-page-minimum",
            Label = "Set Large Page Minimum to 128MB",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets large page minimum allocation to 128MB. Improves memory performance for applications that support large pages. Default: not set.",
            Tags = ["performance", "memory", "large-pages", "allocation"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "LargePageMinimum",
                    134217728
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "LargePageMinimum"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "LargePageMinimum",
                    134217728
                ),
            ],
        },
        new TweakDef
        {
            Id = "perf-menu-show-delay",
            Label = "Set Menu Show Delay to 0ms",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets the menu show delay to 0 milliseconds. Makes menus appear instantly without animation delay. Default: 400ms.",
            Tags = ["performance", "menu", "delay", "responsiveness"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "MenuShowDelay", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "MenuShowDelay", "400")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "MenuShowDelay", "0")],
        },
        new TweakDef
        {
            Id = "perf-performance",
            Label = "Set Visual Effects to Best Performance",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets Windows visual effects to 'Adjust for best performance'. Disables all animations, shadows, thumbnails. Default: Let Windows decide.",
            Tags = ["performance", "visual-effects", "animations", "system"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects", "VisualFXSetting", 2)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects", "VisualFXSetting", 0)],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects", "VisualFXSetting", 2),
            ],
        },
        new TweakDef
        {
            Id = "perf-svchost-split",
            Label = "Reduce SvcHost Splitting Threshold",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets SvcHostSplitThresholdInKB to match installed RAM. Reduces the number of svchost.exe processes on systems with ample memory. Default: auto.",
            Tags = ["performance", "svchost", "memory", "processes"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "SvcHostSplitThresholdInKB", 67108864)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "SvcHostSplitThresholdInKB")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "SvcHostSplitThresholdInKB", 67108864)],
        },
        new TweakDef
        {
            Id = "perf-disable-low-disk-warning",
            Label = "Disable Low Disk Space Warning",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the low disk space check and warning balloon notification. Prevents Explorer from scanning drives periodically.",
            Tags = ["performance", "disk", "notification", "explorer"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoLowDiskSpaceChecks", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoLowDiskSpaceChecks")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoLowDiskSpaceChecks", 1),
            ],
        },
        new TweakDef
        {
            Id = "perf-disable-power-throttling",
            Label = "Disable Power Throttling",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Windows power throttling that reduces CPU frequency for background and foreground apps. Ensures maximum CPU performance at all times.",
            Tags = ["performance", "power", "throttling", "cpu"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerThrottling"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerThrottling", "PowerThrottlingOff", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerThrottling", "PowerThrottlingOff")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerThrottling", "PowerThrottlingOff", 1)],
        },
        new TweakDef
        {
            Id = "perf-disable-aero-peek",
            Label = "Disable Aero Peek",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the Aero Peek preview that shows the desktop when hovering over the Show Desktop button. Eliminates the associated DWM composition overhead.",
            Tags = ["performance", "aero", "animations", "dwm"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableAeroPeek", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableAeroPeek")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableAeroPeek", 0)],
        },
        new TweakDef
        {
            Id = "perf-increase-smb-max-cmds",
            Label = "Increase SMB Client Maximum Command Queue",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Increases MaxCmds to 32 in LanmanWorkstation parameters. Allows more simultaneous outstanding SMB requests, improving file-share throughput under multi-threaded workloads.",
            Tags = ["performance", "smb", "network", "server"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters", "MaxCmds", 32)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters", "MaxCmds")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters", "MaxCmds", 32)],
        },
        new TweakDef
        {
            Id = "perf-disable-listview-shadow",
            Label = "Disable ListView Item Drop Shadows",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Removes the drop-shadow rendering from ListView items in Explorer. Small rendering overhead eliminated on large file lists.",
            Tags = ["performance", "explorer", "ui", "rendering"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ListviewShadow", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ListviewShadow")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ListviewShadow", 0)],
        },
        new TweakDef
        {
            Id = "perf-crash-log-event-off",
            Label = "Disable BSOD Event-Log Entry",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets LogEvent=0 to prevent the CrashControl service from writing a System event log entry on each BSOD. Reduces disk writes during crash recovery.",
            Tags = ["performance", "bsod", "event-log", "crash"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "LogEvent", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "LogEvent")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "LogEvent", 0)],
        },
        new TweakDef
        {
            Id = "perf-set-app-kill-timeout",
            Label = "Reduce Application Shutdown Wait to 5 s",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets WaitToKillAppTimeout to 5 000 ms. Unresponsive applications are terminated 5 seconds after shutdown is initiated instead of the default 20 s.",
            Tags = ["performance", "shutdown", "app", "timeout"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WaitToKillAppTimeout", 5000)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WaitToKillAppTimeout")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WaitToKillAppTimeout", 5000)],
        },
        new TweakDef
        {
            Id = "perf-disable-listview-alpha-select",
            Label = "Disable ListView Alpha-Select Highlight",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the translucent alpha-blend selection rectangle in Explorer list views (ListviewAlphaSelect=0). Eliminates the per-frame alpha compositing cost during rubber-band selection.",
            Tags = ["performance", "explorer", "ui", "rendering"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ListviewAlphaSelect", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ListviewAlphaSelect")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ListviewAlphaSelect", 0),
            ],
        },
    ];
}

internal static class SystemOptimization
{
    private const string MemMgmt = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management";
    private const string FileSystem = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem";
    private const string SessionMgr = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager";
    private const string PriorityCtrl = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl";
    private const string CrashCtrl = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl";
    private const string Lsa = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa";
    private const string Power = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power";
    private const string Kernel = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile";
    private const string WinLogon = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon";
    private const string WinErr = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting";
    private const string Explorer = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        // ── Memory Management ────────────────────────────────────────────

        // ── File System ──────────────────────────────────────────────────

        // ── Process Priority ─────────────────────────────────────────────

        // ── Multimedia / Gaming Scheduling ───────────────────────────────

        // ── Crash & Error Handling ───────────────────────────────────────

        // ── Boot & Logon ─────────────────────────────────────────────────

        new TweakDef
        {
            Id = "sysopt-auto-logon-last-user",
            Label = "Auto-Logon Last User (Skip Lock Screen)",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Automatically logs in the last user at boot, skipping the lock screen (not for shared PCs).",
            Tags = ["optimization", "logon", "auto", "boot"],
            RegistryKeys = [WinLogon],
            ApplyOps = [RegOp.SetDword(WinLogon, "AutoRestartShell", 1)],
            RemoveOps = [RegOp.SetDword(WinLogon, "AutoRestartShell", 0)],
            DetectOps = [RegOp.CheckDword(WinLogon, "AutoRestartShell", 1)],
        },
        // ── Security & LSA ───────────────────────────────────────────────

        // ── Power & Energy ───────────────────────────────────────────────

        // ── Visual Effects Minimal ───────────────────────────────────────

        // ── Misc Performance ─────────────────────────────────────────────

        // ── Network Buffer Tuning ────────────────────────────────────────

        // ── UI Responsiveness ────────────────────────────────────────────
    ];
}

internal static class SystemTweaks
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "sys-detailed-bsod",
            Label = "Enable Detailed Blue Screen Info",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Shows technical parameters on BSoD screens instead of just the QR code and sad-face. Useful for diagnosing crash causes. Default: disabled. Recommended: enabled.",
            Tags = ["system", "bsod", "crash", "diagnostic", "blue-screen"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "DisplayParameters", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "DisplayParameters")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "DisplayParameters", 1)],
        },
        new TweakDef
        {
            Id = "sys-disable-wpbt",
            Label = "Disable WPBT (Vendor Bloatware Injection)",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Windows Platform Binary Table which allows vendors to inject software via UEFI firmware (e.g., Lenovo, HP bloatware). Default: enabled. Recommended: disabled.",
            Tags = ["system", "wpbt", "uefi", "bloatware", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "DisableWpbtExecution", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "DisableWpbtExecution")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "DisableWpbtExecution", 1)],
        },
        new TweakDef
        {
            Id = "sys-enable-utc-hardware-clock",
            Label = "Set Hardware Clock to UTC",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the Windows hardware clock (RTC) to UTC instead of local time. Fixes time drift in dual-boot with Linux. Default: local.",
            Tags = ["system", "clock", "utc", "dual-boot"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\TimeZoneInformation"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\TimeZoneInformation", "RealTimeIsUniversal", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\TimeZoneInformation", "RealTimeIsUniversal")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\TimeZoneInformation", "RealTimeIsUniversal", 1)],
        },
        new TweakDef
        {
            Id = "sys-memory-limit-none",
            Label = "Remove System Memory Limit",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Removes the PhysicalMemoryAllocationPolicy value from Memory Management to let Windows use all available RAM without a capped upper bound. Default: not set.",
            Tags = ["system", "memory", "ram", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "PhysicalMemoryAllocationPolicy"
                ),
            ],
            // NOTE: No RemoveOps — this tweak deletes a cap value that was set by the user or
            // OEM. We cannot safely restore to an unknown prior value; re-enabling any limit
            // would require knowing what it was. Removal is intentionally one-directional.
            RemoveOps = [],
            DetectOps =
            [
                RegOp.CheckMissing(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "PhysicalMemoryAllocationPolicy"
                ),
            ],
        },
        new TweakDef
        {
            Id = "sys-io-priority-boost",
            Label = "Enable I/O Priority Boost for Foreground",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets ITEPriority=3 in PriorityControl to give foreground processes an I/O priority boost (Normal+). Improves responsiveness under disk-heavy background workloads. Default: 3.",
            Tags = ["system", "io", "priority", "foreground"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl", "ITEPriority", 3)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl", "ITEPriority")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl", "ITEPriority", 3)],
        },
        new TweakDef
        {
            Id = "sys-gdi-batch-limit",
            Label = "Set GDI Batch Limit to 256",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets GDIBatchLimit=256 in the Session Manager key. Increases the GDI batch flush threshold, reducing context switches for apps that make many consecutive GDI calls. Default: 0 (unbatched).",
            Tags = ["system", "gdi", "graphics", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "GDIBatchLimit", 256)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "GDIBatchLimit")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "GDIBatchLimit", 256)],
        },
        new TweakDef
        {
            Id = "sys-vm-write-watch-off",
            Label = "Disable VM Write Watch",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets WriteWatch=0 in Memory Management to disable write-watch tracking. Reduces kernel overhead when this feature is not needed by the workload. Default: 0.",
            Tags = ["system", "vm", "memory", "kernel"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "WriteWatch", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "WriteWatch")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "WriteWatch", 0)],
        },
        new TweakDef
        {
            Id = "sys-idle-task-priority",
            Label = "Set Idle Task CPU Priority to Low",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets IdleTaskPriority=1 in PriorityControl to ensure idle maintenance tasks run at lowest possible CPU priority. Prevents background tasks from stealing CPU time. Default: 1.",
            Tags = ["system", "idle", "priority", "background"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl", "IdleTaskPriority", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl", "IdleTaskPriority")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl", "IdleTaskPriority", 1)],
        },
    ];
}

internal static class RegistryTweaks
{
    private const string CfgMgr = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "reg-set-hive-checkpoint-60s",
            Label = "Set Registry Hive Checkpoint Interval to 60s",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the registry hive flush/checkpoint interval to 60 seconds. Reduces write pressure on disk while keeping hive state reasonably current. Default: system-defined.",
            Tags = ["registry", "hive", "checkpoint", "performance"],
            RegistryKeys = [CfgMgr],
            ApplyOps = [RegOp.SetDword(CfgMgr, "CheckpointInterval", 60)],
            RemoveOps = [RegOp.DeleteValue(CfgMgr, "CheckpointInterval")],
            DetectOps = [RegOp.CheckDword(CfgMgr, "CheckpointInterval", 60)],
        },
        new TweakDef
        {
            Id = "reg-set-max-log-files",
            Label = "Set Max Registry Log Files to 20",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Limits the maximum number of registry transaction log files retained. Default: system-defined (unlimited).",
            Tags = ["registry", "logs", "disk-space"],
            RegistryKeys = [CfgMgr],
            ApplyOps = [RegOp.SetDword(CfgMgr, "MaxCountLogs", 20)],
            RemoveOps = [RegOp.DeleteValue(CfgMgr, "MaxCountLogs")],
            DetectOps = [RegOp.CheckDword(CfgMgr, "MaxCountLogs", 20)],
        },
        new TweakDef
        {
            Id = "reg-enable-hive-autorepair",
            Label = "Enable Registry Hive Auto-Repair",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables automatic repair of registry hives on corruption detection. Helps recover from partial writes. Default: may be disabled on some configurations.",
            Tags = ["registry", "hive", "repair", "reliability"],
            RegistryKeys = [CfgMgr],
            ApplyOps = [RegOp.SetDword(CfgMgr, "EnableAutoRepair", 1)],
            RemoveOps = [RegOp.DeleteValue(CfgMgr, "EnableAutoRepair")],
            DetectOps = [RegOp.CheckDword(CfgMgr, "EnableAutoRepair", 1)],
        },
        new TweakDef
        {
            Id = "reg-set-hive-size-hint",
            Label = "Set Registry Hive Pre-Allocated Size to 2048 KB",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Hints to Windows that the registry hive should be pre-allocated at 2048 KB. Reduces hive file fragmentation over time. Default: 0 (no hint).",
            Tags = ["registry", "hive", "allocation", "fragmentation"],
            RegistryKeys = [CfgMgr],
            ApplyOps = [RegOp.SetDword(CfgMgr, "MaxRegistrySizeHint", 2048)],
            RemoveOps = [RegOp.DeleteValue(CfgMgr, "MaxRegistrySizeHint")],
            DetectOps = [RegOp.CheckDword(CfgMgr, "MaxRegistrySizeHint", 2048)],
        },
        new TweakDef
        {
            Id = "reg-enable-reg-journal",
            Label = "Enable Registry Transaction Journal",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables transaction journalling for registry hive modifications. Improves crash consistency and recovery of registry state.",
            Tags = ["registry", "journal", "transaction", "reliability"],
            RegistryKeys = [CfgMgr],
            ApplyOps = [RegOp.SetDword(CfgMgr, "EnableJournal", 1)],
            RemoveOps = [RegOp.DeleteValue(CfgMgr, "EnableJournal")],
            DetectOps = [RegOp.CheckDword(CfgMgr, "EnableJournal", 1)],
        },
        new TweakDef
        {
            Id = "reg-set-idle-time-limit",
            Label = "Set Registry Idle Flush Delay to 300s",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Configures the idle time in seconds before a lazy-flush of dirty registry hive pages is triggered. 300s defers writes during active sessions. Default: system-defined.",
            Tags = ["registry", "flush", "idle", "performance"],
            RegistryKeys = [CfgMgr],
            ApplyOps = [RegOp.SetDword(CfgMgr, "IdleTimeInSeconds", 300)],
            RemoveOps = [RegOp.DeleteValue(CfgMgr, "IdleTimeInSeconds")],
            DetectOps = [RegOp.CheckDword(CfgMgr, "IdleTimeInSeconds", 300)],
        },
        new TweakDef
        {
            Id = "reg-enable-reg-shadow-mount",
            Label = "Enable Registry Shadow Mount for Offline Systems",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables shadow-mount mode for registry hives accessed by offline servicing tools. Useful for WinPE/deployment scenarios.",
            Tags = ["registry", "shadow", "offline", "servicing"],
            RegistryKeys = [CfgMgr],
            ApplyOps = [RegOp.SetDword(CfgMgr, "EnableShadowMount", 1)],
            RemoveOps = [RegOp.DeleteValue(CfgMgr, "EnableShadowMount")],
            DetectOps = [RegOp.CheckDword(CfgMgr, "EnableShadowMount", 1)],
        },
        new TweakDef
        {
            Id = "reg-disable-notify-overflow",
            Label = "Disable Registry Notification Overflow Dropping",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents the registry from dropping change notifications when the internal notification queue overflows. Default: dropping enabled under load.",
            Tags = ["registry", "notification", "overflow", "reliability"],
            RegistryKeys = [CfgMgr],
            ApplyOps = [RegOp.SetDword(CfgMgr, "NotifyOverflowDropped", 0)],
            RemoveOps = [RegOp.DeleteValue(CfgMgr, "NotifyOverflowDropped")],
            DetectOps = [RegOp.CheckDword(CfgMgr, "NotifyOverflowDropped", 0)],
        },
        new TweakDef
        {
            Id = "reg-set-hive-prealloc",
            Label = "Set Registry Hive Pre-Allocation Block to 64 KB",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the registry hive block Pre-Allocation adjustment to 64 KB. Reduces frequency of hive file growth operations. Default: 0.",
            Tags = ["registry", "hive", "prealloc", "performance"],
            RegistryKeys = [CfgMgr],
            ApplyOps = [RegOp.SetDword(CfgMgr, "PreAllocationAdjustment", 65536)],
            RemoveOps = [RegOp.DeleteValue(CfgMgr, "PreAllocationAdjustment")],
            DetectOps = [RegOp.CheckDword(CfgMgr, "PreAllocationAdjustment", 65536)],
        },
        new TweakDef
        {
            Id = "reg-disable-log-overflow",
            Label = "Disable Registry Log Overflow Truncation",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents the registry manager from truncating transaction logs when they reach their size limit. Retains full history for crash recovery. Default: truncation enabled.",
            Tags = ["registry", "log", "truncation", "reliability"],
            RegistryKeys = [CfgMgr],
            ApplyOps = [RegOp.SetDword(CfgMgr, "DisableLogOverflow", 1)],
            RemoveOps = [RegOp.DeleteValue(CfgMgr, "DisableLogOverflow")],
            DetectOps = [RegOp.CheckDword(CfgMgr, "DisableLogOverflow", 1)],
        },
    ];
}

internal static class Startup
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "startup-disable-startup-delay",
            Label = "Disable Startup Delay",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes the artificial startup delay for Run-key programs, allowing them to launch immediately at login.",
            Tags = ["startup", "performance", "boot"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Serialize"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Serialize", "StartupDelayInMSec", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Serialize", "StartupDelayInMSec")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Serialize", "StartupDelayInMSec", 0),
            ],
        },
        new TweakDef
        {
            Id = "startup-disable-cortana-startup",
            Label = "Disable Cortana Startup",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes Cortana from the HKCU Run key to prevent auto-start at login.",
            Tags = ["startup", "cortana", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "CortanaUI"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "Cortana"),
            ],
            DetectOps = [RegOp.CheckMissing(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "CortanaUI")],
        },
        new TweakDef
        {
            Id = "startup-disable-login-background",
            Label = "Use Solid Color Login Background",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Replaces the Windows Spotlight / hero image on the login screen with a plain solid color background.",
            Tags = ["startup", "login", "appearance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "DisableLogonBackgroundImage", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "DisableLogonBackgroundImage")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "DisableLogonBackgroundImage", 1)],
        },
        new TweakDef
        {
            Id = "startup-disable-first-logon-animation",
            Label = "Disable First Login Animation",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the 'Hi / We're getting things ready' first-logon animation shown after a new user profile is created.",
            Tags = ["startup", "animation", "login", "boot"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "EnableFirstLogonAnimation", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "EnableFirstLogonAnimation", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "EnableFirstLogonAnimation", 0),
            ],
        },
        new TweakDef
        {
            Id = "startup-start-boot-numlock-on",
            Label = "Set Boot-Up Num Lock to On",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables Num Lock at the Windows login screen by default. Default: Off. Recommended: On for desktop keyboards.",
            Tags = ["startup", "numlock", "keyboard", "boot"],
            RegistryKeys = [@"HKEY_USERS\.DEFAULT\Control Panel\Keyboard"],
            ApplyOps = [RegOp.SetString(@"HKEY_USERS\.DEFAULT\Control Panel\Keyboard", "InitialKeyboardIndicators", "2")],
            RemoveOps = [RegOp.SetString(@"HKEY_USERS\.DEFAULT\Control Panel\Keyboard", "InitialKeyboardIndicators", "0")],
            DetectOps = [RegOp.CheckString(@"HKEY_USERS\.DEFAULT\Control Panel\Keyboard", "InitialKeyboardIndicators", "2")],
        },
        new TweakDef
        {
            Id = "startup-start-disable-app-restart",
            Label = "Disable Automatic App Restart on Login",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Prevents Windows from automatically restarting apps that were open before shutdown/restart. Default: Enabled. Recommended: Disabled.",
            Tags = ["startup", "restart", "apps", "login", "winlogon"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Winlogon"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Winlogon", "RestartApps", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Winlogon", "RestartApps")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Winlogon", "RestartApps", 0)],
        },
        new TweakDef
        {
            Id = "startup-set-boot-timeout-3s",
            Label = "Set Boot Menu Timeout to 3 Seconds",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the multi-boot OS selection timeout to 3 seconds instead of the default 30. Faster boot on single-OS machines.",
            Tags = ["startup", "boot", "timeout", "speed"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\BCD00000000\Objects\{9dea862c-5cdd-4e70-acc1-f32b344d4795}\Elements\25000004"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "SystemBootDevice", 3)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "SystemBootDevice")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "SystemBootDevice", 3)],
        },
        // ── Sprint 20 additions ─────────────────────────────────────────────

        new TweakDef
        {
            Id = "startup-disable-boot-logo",
            Label = "Disable Boot Logo Display",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Suppresses the Windows boot logo animation for faster POST-to-desktop times.",
            Tags = ["startup", "boot", "logo", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\ControlSet001\Control\BootDisplay", "DisableBootLogo", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\ControlSet001\Control\BootDisplay", "DisableBootLogo")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\ControlSet001\Control\BootDisplay", "DisableBootLogo", 1)],
        },
        new TweakDef
        {
            Id = "startup-disable-narrator-at-login",
            Label = "Disable Narrator at Login Screen",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Narrator auto-start at the Windows login screen.",
            Tags = ["startup", "narrator", "accessibility", "login"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\Narrator.exe"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\Narrator.exe",
                    "Debugger",
                    @"%SystemRoot%\System32\systray.exe"
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\Narrator.exe",
                    "Debugger"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\Narrator.exe",
                    "Debugger",
                    @"%SystemRoot%\System32\systray.exe"
                ),
            ],
        },
        new TweakDef
        {
            Id = "startup-disable-fast-user-switching",
            Label = "Disable Fast User Switching",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables fast user switching at login. Simplifies the login screen and slightly reduces memory usage on shared PCs.",
            Tags = ["startup", "login", "user-switching", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "HideFastUserSwitching", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "HideFastUserSwitching")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "HideFastUserSwitching", 1),
            ],
        },
        new TweakDef
        {
            Id = "startup-disable-edge-prelaunch",
            Label = "Disable Edge Pre-Launch at Login",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Microsoft Edge from pre-launching in the background at login. Reduces startup memory and CPU usage.",
            Tags = ["startup", "edge", "prelaunch", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MicrosoftEdge\Main"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MicrosoftEdge\Main", "AllowPrelaunch", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MicrosoftEdge\Main", "AllowPrelaunch")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MicrosoftEdge\Main", "AllowPrelaunch", 0)],
        },
        new TweakDef
        {
            Id = "startup-disable-compatibility-assistant",
            Label = "Disable Program Compatibility Assistant",
            Category = "System",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the Program Compatibility Assistant that checks applications for compatibility issues at launch.",
            Tags = ["startup", "compatibility", "performance", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\AppCompat"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\AppCompat", "DisablePCA", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\AppCompat", "DisablePCA")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\AppCompat", "DisablePCA", 1)],
        },
    ];
}

internal static class Boot
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "boot-disable-secboot-check",
            Label = "Suppress Secure Boot Status Check",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Suppresses the Secure Boot status notification in Windows by setting UEFISecureBootEnabled to 0 in the registry.",
            Tags = ["boot", "security", "uefi"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecureBoot\State"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecureBoot\State", "UEFISecureBootEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecureBoot\State", "UEFISecureBootEnabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecureBoot\State", "UEFISecureBootEnabled", 0)],
        },
        new TweakDef
        {
            Id = "boot-disable-boot-anim",
            Label = "Disable Boot Animation/Spinner",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Windows boot animation/spinner for a faster perceived boot. The boot process skips the animated dots. Default: enabled. Recommended: disabled for faster boot.",
            Tags = ["boot", "animation", "performance", "spinner"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootAnimationParams"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootAnimationParams", "Disabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootAnimationParams", "Disabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootAnimationParams", "Disabled", 1)],
        },
        new TweakDef
        {
            Id = "boot-enable-fast-startup",
            Label = "Enable Fast Startup (Hiberboot)",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Enables Windows Fast Startup which uses a hybrid shutdown with hibernation to speed up boot time. Default: Usually enabled. Recommended: Enabled for fast boot.",
            Tags = ["boot", "fast-startup", "hiberboot", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Power"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Power", "HiberbootEnabled", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Power", "HiberbootEnabled", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Power", "HiberbootEnabled", 1)],
        },
        new TweakDef
        {
            Id = "boot-prefetch-optimized",
            Label = "Set Prefetch to Optimized Mode",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Enables both boot and application prefetching for optimal performance. Value 3 = boot + app prefetch. Default: 3. Recommended: 3 for SSDs and HDDs.",
            Tags = ["boot", "prefetch", "performance", "startup"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                    "EnablePrefetcher",
                    3
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                    "EnablePrefetcher",
                    0
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                    "EnablePrefetcher",
                    3
                ),
            ],
        },
        new TweakDef
        {
            Id = "boot-clear-pagefile",
            Label = "Clear Pagefile at Shutdown",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Clears the virtual memory pagefile at every shutdown. Prevents sensitive data from being recovered from pagefile.sys. Note: significantly increases shutdown time on large systems. Default: not cleared. Recommended: Apply on secure workstations.",
            Tags = ["boot", "security", "pagefile", "shutdown", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "ClearPageFileAtShutdown",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "ClearPageFileAtShutdown",
                    0
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "ClearPageFileAtShutdown",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "boot-disable-boot-ux",
            Label = "Disable Boot UI Animation",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Windows boot animation (spinning dots). Shows a simple progress bar instead. Default: animated.",
            Tags = ["boot", "animation", "ui", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootControl", "BootProgressAnimation", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootControl", "BootProgressAnimation")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootControl", "BootProgressAnimation", 0)],
        },
        new TweakDef
        {
            Id = "boot-set-timeout-5s",
            Label = "Set Boot Menu Timeout to 5 Seconds",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the boot manager menu timeout to 5 seconds for dual-boot systems. Default: 30 seconds.",
            Tags = ["boot", "timeout", "dual-boot", "menu"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\BCD00000000\Objects\{9dea862c-5cdd-4e70-acc1-f32b344d4795}\Elements\25000004"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\BCD00000000\Objects\{9dea862c-5cdd-4e70-acc1-f32b344d4795}\Elements\25000004", "Element", 5),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\BCD00000000\Objects\{9dea862c-5cdd-4e70-acc1-f32b344d4795}\Elements\25000004", "Element", 30),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\BCD00000000\Objects\{9dea862c-5cdd-4e70-acc1-f32b344d4795}\Elements\25000004", "Element", 5),
            ],
        },
        new TweakDef
        {
            Id = "boot-verbose-status-messages",
            Label = "Enable Verbose Boot Status Messages",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Shows detailed status messages during boot, shutdown, logon, and logoff. Default: hidden.",
            Tags = ["boot", "verbose", "status", "messages"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "VerboseStatus", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "VerboseStatus")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "VerboseStatus", 1)],
        },
        // ── Command-based boot tweaks (bcdedit) ────────────────────────────
        new TweakDef
        {
            Id = "boot-bcd-quiet-boot",
            Label = "Enable Quiet Boot (bcdedit)",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables Windows quiet boot mode via bcdedit — suppresses the boot logo and status messages for faster boot appearance.",
            Tags = ["boot", "bcdedit", "quiet", "logo"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/set", "{current}", "quietboot", "yes"]);
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/set", "{current}", "quietboot", "no"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{current}"]);
                return stdout.Contains("quietboot", StringComparison.OrdinalIgnoreCase) && stdout.Contains("Yes", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-bcd-timeout-3s",
            Label = "Set Boot Menu Timeout to 3 Seconds (bcdedit)",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the boot manager timeout to 3 seconds via bcdedit. Speeds up boot when multi-boot options exist.",
            Tags = ["boot", "bcdedit", "timeout", "fast"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/timeout", "3"]);
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/timeout", "30"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{bootmgr}"]);
                return stdout.Contains("timeout", StringComparison.OrdinalIgnoreCase) && stdout.Contains("3", StringComparison.Ordinal);
            },
        },
        new TweakDef
        {
            Id = "boot-bcd-disable-recovery",
            Label = "Disable Automatic Recovery (bcdedit)",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the automatic recovery/repair environment via bcdedit. Prevents boot loops but removes automatic repair capability.",
            Tags = ["boot", "bcdedit", "recovery", "repair", "server"],
            KindHint = TweakKind.SystemCommand,
            SideEffects = "Disables automatic repair on boot failure.",
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/set", "{current}", "recoveryenabled", "no"]);
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/set", "{current}", "recoveryenabled", "yes"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{current}"]);
                return stdout.Contains("recoveryenabled", StringComparison.OrdinalIgnoreCase)
                    && stdout.Contains("No", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-driver-verifier-reset",
            Label = "Reset Driver Verifier (verifier)",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Resets Driver Verifier settings to none. Useful after debugging driver issues when verifier was left enabled.",
            Tags = ["boot", "verifier", "driver", "diagnostic", "reset"],
            KindHint = TweakKind.SystemCommand,
            SideEffects = "Requires reboot to take effect.",
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("verifier", ["/reset"]);
            },
            // NOTE: No RemoveAction — "reset" is a one-shot diagnostic action. There is no
            // meaningful inverse; re-enabling verifier requires choosing specific drivers.
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("verifier", ["/query"]);
                return stdout.Contains("No drivers", StringComparison.OrdinalIgnoreCase)
                    || stdout.Contains("not loaded", StringComparison.OrdinalIgnoreCase);
            },
        },
        // ── Restored stubs with real operations ──────────────────

        new TweakDef
        {
            Id = "boot-disable-auto-repair",
            Label = "Disable Automatic Startup Repair",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Prevents Windows from launching Automatic Repair after consecutive boot failures. Use with caution.",
            Tags = ["boot", "auto-repair", "recovery", "bcdedit"],
            KindHint = TweakKind.SystemCommand,
            SideEffects = "System will not auto-recover from boot failures.",
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/set", "{current}", "bootstatuspolicy", "IgnoreAllFailures"]);
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/deletevalue", "{current}", "bootstatuspolicy"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{current}"]);
                return stdout.Contains("bootstatuspolicy", StringComparison.OrdinalIgnoreCase)
                    && stdout.Contains("IgnoreAllFailures", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-disable-boot-logo",
            Label = "Disable Boot Logo (bcdedit)",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Windows boot logo via bcdedit for a minimalist boot screen.",
            Tags = ["boot", "logo", "bcdedit", "ux"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/set", "{current}", "quietboot", "yes"]);
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/deletevalue", "{current}", "quietboot"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{current}"]);
                return stdout.Contains("quietboot", StringComparison.OrdinalIgnoreCase) && stdout.Contains("Yes", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-disable-driver-verifier",
            Label = "Disable Driver Verifier Flags",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Clears Driver Verifier flags in the registry. Useful after debugging when verifier causes boot loops.",
            Tags = ["boot", "verifier", "driver", "registry"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "VerifyDriverLevel", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "VerifyDriverLevel"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "VerifyDriverLevel", 0),
            ],
        },
        new TweakDef
        {
            Id = "boot-disable-logo",
            Label = "Disable OEM Boot Logo",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the OEM manufacturer logo during boot via bcdedit nologo option.",
            Tags = ["boot", "logo", "oem", "bcdedit"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/set", "{globalsettings}", "custom:16000067", "true"]);
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/deletevalue", "{globalsettings}", "custom:16000067"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{globalsettings}"]);
                return stdout.Contains("16000067", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-disable-winre",
            Label = "Disable WinRE Partition",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Windows Recovery Environment. Frees recovery partition but removes repair tools.",
            Tags = ["boot", "winre", "recovery", "disk-space"],
            KindHint = TweakKind.SystemCommand,
            SideEffects = "Removes access to Windows Recovery tools.",
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("reagentc", ["/disable"]);
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("reagentc", ["/enable"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("reagentc", ["/info"]);
                return stdout.Contains("Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-fast-boot-timeout",
            Label = "Set Boot Timeout to 0 Seconds",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Sets BCD boot menu timeout to 0 seconds for instant boot-through. No OS selection screen shown.",
            Tags = ["boot", "timeout", "bcdedit", "fast"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/timeout", "0"]);
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/timeout", "30"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{bootmgr}"]);
                return stdout.Contains("timeout", StringComparison.OrdinalIgnoreCase) && stdout.Contains("0", StringComparison.Ordinal);
            },
        },
        new TweakDef
        {
            Id = "boot-log",
            Label = "Enable Boot Logging (ntbtlog.txt)",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables boot logging via bcdedit. Writes driver load info to %%SystemRoot%%\\ntbtlog.txt.",
            Tags = ["boot", "logging", "bcdedit", "diagnostic"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/set", "{current}", "bootlog", "yes"]);
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/set", "{current}", "bootlog", "no"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{current}"]);
                return stdout.Contains("bootlog", StringComparison.OrdinalIgnoreCase) && stdout.Contains("Yes", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-max-proc-count",
            Label = "Use All CPU Cores at Boot",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Configures msconfig-equivalent setting to use all processor cores during boot.",
            Tags = ["boot", "cpu", "cores", "performance", "bcdedit"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/deletevalue", "{current}", "numproc"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{current}"]);
                return !stdout.Contains("numproc", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-menu-timeout",
            Label = "Set Boot Menu Timeout to 10s",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the boot menu display timeout to 10 seconds. Useful for dual-boot systems.",
            Tags = ["boot", "timeout", "menu", "dual-boot", "bcdedit"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/timeout", "10"]);
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/timeout", "30"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{bootmgr}"]);
                return stdout.Contains("timeout", StringComparison.OrdinalIgnoreCase) && stdout.Contains("10", StringComparison.Ordinal);
            },
        },
        new TweakDef
        {
            Id = "boot-verbose-boot",
            Label = "Enable Verbose Boot Messages",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Shows detailed status messages during boot instead of the logo. Useful for debugging slow boot.",
            Tags = ["boot", "verbose", "diagnostic", "bcdedit"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/set", "{current}", "sos", "on"]);
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("bcdedit", ["/set", "{current}", "sos", "off"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{current}"]);
                return stdout.Contains("sos", StringComparison.OrdinalIgnoreCase) && stdout.Contains("Yes", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-fast-startup-gpo",
            Label = "Enable Fast Startup via Group Policy",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets HiberbootEnabled=1 in the Windows System policy key to enforce fast startup at GPO level. Complements the standard fast startup registry setting. Default: not set.",
            Tags = ["boot", "fast-startup", "policy", "hibernate"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "HiberbootEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "HiberbootEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "HiberbootEnabled", 1)],
        },
        new TweakDef
        {
            Id = "boot-global-wait-timeout",
            Label = "Set Global Shutdown Wait Timeout to 5s",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets WaitForIdleState=5 in the system Timeout key. Controls how long Windows waits for the system to become idle before shutdown completes. Default: 2.",
            Tags = ["boot", "shutdown", "timeout", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Timeout"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Timeout", "WaitForIdleState", 5)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Timeout", "WaitForIdleState")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Timeout", "WaitForIdleState", 5)],
        },
        new TweakDef
        {
            Id = "boot-menu-timeout-policy",
            Label = "Set Boot Menu Display Timeout Policy to 10s",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets BootTimeoutSeconds=10 in the Windows System policy key. Controls the boot menu display time at policy level. Default: not set (uses BCD value).",
            Tags = ["boot", "menu", "timeout", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "BootTimeoutSeconds", 10)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "BootTimeoutSeconds")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "BootTimeoutSeconds", 10)],
        },
        new TweakDef
        {
            Id = "boot-hyperv-launch-off",
            Label = "Disable Hyper-V Hypervisor Launch",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Runs 'bcdedit /set hypervisorlaunchtype off' to disable the Hyper-V hypervisor at boot. Improves native performance on bare-metal gaming/workstation installs. Default: auto.",
            Tags = ["boot", "hyper-v", "bcd", "performance"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = dryRun =>
            {
                Elevation.AssertAdmin(dryRun);
                Elevation.RunElevated("bcdedit", ["/set", "hypervisorlaunchtype", "off"]);
            },
            RemoveAction = dryRun =>
            {
                Elevation.AssertAdmin(dryRun);
                Elevation.RunElevated("bcdedit", ["/set", "hypervisorlaunchtype", "auto"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{current}"]);
                return stdout.Contains("hypervisorlaunchtype", StringComparison.OrdinalIgnoreCase)
                    && stdout.Contains("Off", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-test-signing-off",
            Label = "Disable Test Signing Mode",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Runs 'bcdedit /set testsigning off' to disable test-signing mode. Prevents unsigned test drivers from loading. Default: off.",
            Tags = ["boot", "bcd", "security", "test-signing"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = dryRun =>
            {
                Elevation.AssertAdmin(dryRun);
                Elevation.RunElevated("bcdedit", ["/set", "testsigning", "off"]);
            },
            RemoveAction = dryRun =>
            {
                Elevation.AssertAdmin(dryRun);
                Elevation.RunElevated("bcdedit", ["/set", "testsigning", "on"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{current}"]);
                return stdout.Contains("testsigning", StringComparison.OrdinalIgnoreCase)
                    && stdout.Contains("No", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-report-ok",
            Label = "Enable Boot-OK Reporting to Winlogon",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets ReportBootOk=1 in Winlogon to signal that the current boot is clean and should be saved as the last known good configuration. Default: 1.",
            Tags = ["boot", "winlogon", "last-known-good", "recovery"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "ReportBootOk", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "ReportBootOk")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "ReportBootOk", 1)],
        },
        new TweakDef
        {
            Id = "boot-kernel-debug-filter",
            Label = "Suppress Kernel Debug Print Filter",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DEFAULT=0x0 in the Debug Print Filter to suppress kernel debug messages, reducing DbgPrint overhead on retail builds. Default: 0x8 or not set.",
            Tags = ["boot", "kernel", "debug", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Debug Print Filter"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Debug Print Filter", "DEFAULT", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Debug Print Filter", "DEFAULT")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Debug Print Filter", "DEFAULT", 0)],
        },
        new TweakDef
        {
            Id = "boot-winre-policy-allow",
            Label = "Allow Windows Recovery Environment Policy",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableWinRE=0 in WinRE policy to ensure the Windows Recovery Environment remains accessible. Prevents accidental policy lockout of recovery tools. Default: 0.",
            Tags = ["boot", "recovery", "winre", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinRE"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinRE", "DisableWinRE", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinRE", "DisableWinRE")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinRE", "DisableWinRE", 0)],
        },
        new TweakDef
        {
            Id = "boot-legacy-f8-menu",
            Label = "Enable Legacy F8 Boot Menu",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Runs 'bcdedit /set {bootmgr} displaybootmenu yes' to enable the legacy F8 boot menu. Allows access to safe mode and other startup options. Default: off on modern Windows.",
            Tags = ["boot", "bcd", "safe-mode", "f8"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = dryRun =>
            {
                Elevation.AssertAdmin(dryRun);
                Elevation.RunElevated("bcdedit", ["/set", "{bootmgr}", "displaybootmenu", "yes"]);
            },
            RemoveAction = dryRun =>
            {
                Elevation.AssertAdmin(dryRun);
                Elevation.RunElevated("bcdedit", ["/set", "{bootmgr}", "displaybootmenu", "no"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{bootmgr}"]);
                return stdout.Contains("displaybootmenu", StringComparison.OrdinalIgnoreCase)
                    && stdout.Contains("Yes", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-bcd-nx-optin",
            Label = "Set Data Execution Prevention to OptIn",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Runs 'bcdedit /set nx OptIn' to enable DEP (Data Execution Prevention) only for OS-protected processes. Balances security and compatibility. Default: OptIn.",
            Tags = ["boot", "bcd", "dep", "security"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = dryRun =>
            {
                Elevation.AssertAdmin(dryRun);
                Elevation.RunElevated("bcdedit", ["/set", "nx", "OptIn"]);
            },
            RemoveAction = dryRun =>
            {
                Elevation.AssertAdmin(dryRun);
                Elevation.RunElevated("bcdedit", ["/set", "nx", "OptIn"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("bcdedit", ["/enum", "{current}"]);
                return stdout.Contains("nx", StringComparison.OrdinalIgnoreCase) && stdout.Contains("OptIn", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "boot-disable-startup-app-delay",
            Label = "Disable Startup App Launch Delay",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets StartupDelayInMSec=0 to eliminate the artificial delay Windows introduces before launching registered startup applications. Speeds up the post-login experience. Default: 10-second delay.",
            Tags = ["boot", "startup", "delay", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Serialize"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Serialize", "StartupDelayInMSec", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Serialize", "StartupDelayInMSec")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Serialize", "StartupDelayInMSec", 0),
            ],
        },
        new TweakDef
        {
            Id = "boot-disable-livedump",
            Label = "Disable Kernel Live Dump Collection",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Windows Kernel Live Dump collection (EnableLiveDump=0). Live dumps are taken by heuristics without a full crash; disabling reduces unexpected disk I/O and performance spikes. Default: enabled.",
            Tags = ["boot", "dump", "kernel", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "EnableLiveDump", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "EnableLiveDump", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "EnableLiveDump", 0)],
        },
        new TweakDef
        {
            Id = "boot-enable-nmi-crash-dump",
            Label = "Enable NMI-Triggered Crash Dump",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables triggering a crash dump via a Non-Maskable Interrupt (NMI) button or debugger. Useful for generating a dump on a completely hung system that cannot respond to other input. Default: disabled.",
            Tags = ["boot", "nmi", "dump", "debug"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "NMICrashDump", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "NMICrashDump", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "NMICrashDump", 1)],
        },
        new TweakDef
        {
            Id = "boot-disable-bsod-beep",
            Label = "Disable System Beep on BSOD",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the PC speaker beep that Windows emits when a BSOD (blue screen of death) occurs. Reduces noise in server rooms or overnight unattended machines. Default: 1 (beep enabled).",
            Tags = ["boot", "bsod", "beep", "crash"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "Beep", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "Beep", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "Beep", 0)],
        },
        new TweakDef
        {
            Id = "boot-disable-always-keep-dump",
            Label = "Do Not Permanently Keep Memory Dump",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets AlwaysKeepMemoryDump=0 so Windows does not permanently retain the memory dump even when low on disk. Lets the pagefile cleanup process remove the dump to free space. Default: 0.",
            Tags = ["boot", "dump", "disk", "cleanup"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "AlwaysKeepMemoryDump", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "AlwaysKeepMemoryDump")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "AlwaysKeepMemoryDump", 0)],
        },
        new TweakDef
        {
            Id = "boot-set-system-eventlog-size",
            Label = "Increase System Event Log Size to 50 MB",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the System event log maximum size to 50 MB (52428800 bytes). Allows retention of more historical system events before wrapping. Default: 20 MB.",
            Tags = ["boot", "event-log", "system", "size"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\EventLog\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\EventLog\System", "MaxSize", 52428800)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\EventLog\System", "MaxSize", 20971520)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\EventLog\System", "MaxSize", 52428800)],
        },
        new TweakDef
        {
            Id = "boot-disable-boot-status-display",
            Label = "Disable Boot Status / Spinner Display",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the display of boot status messages (spinner/dots) during startup by clearing DisplayStatusMessages. Produces a cleaner, faster-feeling boot sequence. Default: enabled.",
            Tags = ["boot", "ui", "spinner", "speed"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootStatus"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootStatus", "DisplayStatusMessages", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootStatus", "DisplayStatusMessages", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\BootStatus", "DisplayStatusMessages", 0)],
        },
        // ── merged from: Services.cs ──────────────────────────────────────────────────
        new TweakDef
        {
            Id = "svc-disable-sysmain-service",
            Label = "Disable SysMain (Superfetch)",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the SysMain (Superfetch) service — beneficial on SSD systems.",
            Tags = ["services", "performance", "ssd"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SysMain"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SysMain", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SysMain", "Start", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SysMain", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-diagsvc",
            Label = "Disable Diagnostic Service",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Diagnostic Service (DiagSvc) that runs troubleshooters.",
            Tags = ["services", "telemetry", "diagnostics"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DiagSvc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DiagSvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DiagSvc", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DiagSvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-wbiosrvc",
            Label = "Disable Biometric Service",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows Biometric Service (WbioSrvc). Useful if fingerprint/face login is not used.",
            Tags = ["services", "biometric", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WbioSrvc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WbioSrvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WbioSrvc", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WbioSrvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-remote-registry",
            Label = "Disable Remote Registry",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Remote Registry service which allows remote access to the Windows registry. Security hardening measure.",
            Tags = ["services", "security", "remote"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\RemoteRegistry"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\RemoteRegistry", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\RemoteRegistry", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\RemoteRegistry", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-geolocation-service",
            Label = "Disable Geolocation Service",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Windows Geolocation Service for privacy.",
            Tags = ["services", "privacy", "location"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\lfsvc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\lfsvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\lfsvc", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\lfsvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-delivery-optimization-svc",
            Label = "Disable Delivery Optimization",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Delivery Optimization service which shares Windows Update data with other PCs on LAN and internet.",
            Tags = ["services", "update", "bandwidth", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DoSvc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DoSvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DoSvc", "Start", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DoSvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-fax",
            Label = "Disable Fax Service (Cleanup)",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the legacy Fax service to free resources. Default: Manual. Recommended: Disabled.",
            Tags = ["services", "fax", "legacy", "cleanup"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Fax"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Fax", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Fax", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Fax", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-smartcard",
            Label = "Disable Smart Card Service",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Smart Card service (SCardSvr) for smart-card readers. Safe to disable if no smart cards are used. Default: Manual. Recommended: Disabled.",
            Tags = ["services", "smartcard", "scardsvr", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SCardSvr"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SCardSvr", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SCardSvr", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SCardSvr", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-link-tracking",
            Label = "Disable Distributed Link Tracking Client",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Distributed Link Tracking Client (TrkWks) that maintains NTFS file links across networked computers. Default: Manual. Recommended: Disabled for standalone PCs.",
            Tags = ["services", "link-tracking", "trkwks", "ntfs"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\TrkWks"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\TrkWks", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\TrkWks", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\TrkWks", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-wallet",
            Label = "Disable Wallet Service",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Windows Wallet Service used for NFC-based payments. Safe to disable if contactless payments are unused. Default: Manual. Recommended: Disabled.",
            Tags = ["services", "wallet", "nfc", "payment"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WalletService"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WalletService", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WalletService", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WalletService", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-secondary-logon",
            Label = "Disable Secondary Logon Service",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Secondary Logon (RunAs) service. Reduces privilege escalation surface. Default: manual.",
            Tags = ["services", "secondary-logon", "runas", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\seclogon"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\seclogon", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\seclogon", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\seclogon", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-xbox-live-networking",
            Label = "Disable Xbox Live Networking Service",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Xbox Live Networking service. Not needed if you don't use Xbox features. Default: manual.",
            Tags = ["services", "xbox", "networking", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\XboxNetApiSvc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\XboxNetApiSvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\XboxNetApiSvc", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\XboxNetApiSvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-webclient",
            Label = "Disable WebClient (WebDAV) Service",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the WebClient service (WebDAV). Reduces attack surface for NTLM relay. Default: manual.",
            Tags = ["services", "webclient", "webdav", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WebClient"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WebClient", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WebClient", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WebClient", "Start", 4)],
        },
        // ── Command-based service tweaks (sc.exe) ──────────────────────────
        new TweakDef
        {
            Id = "svc-stop-xbox-services",
            Label = "Stop & Disable All Xbox Services (sc)",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Stops and disables all Xbox-related services (XblAuthManager, XblGameSave, XboxGipSvc, XboxNetApiSvc) to free resources.",
            Tags = ["services", "xbox", "disable", "gaming", "resources"],
            KindHint = TweakKind.ServiceControl,
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                foreach (var svc in new[] { "XblAuthManager", "XblGameSave", "XboxGipSvc", "XboxNetApiSvc" })
                {
                    Elevation.RunElevated("sc", ["stop", svc]);
                    Elevation.RunElevated("sc", ["config", svc, "start=", "disabled"]);
                }
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                foreach (var svc in new[] { "XblAuthManager", "XblGameSave", "XboxGipSvc", "XboxNetApiSvc" })
                {
                    Elevation.RunElevated("sc", ["config", svc, "start=", "demand"]);
                }
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("sc", ["qc", "XblAuthManager"]);
                return stdout.Contains("DISABLED", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "svc-stop-connected-devices",
            Label = "Stop & Disable Connected Devices Platform Service (sc)",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Connected Devices Platform (CDP) service used for cross-device experiences, Timeline, and nearby sharing.",
            Tags = ["services", "cdp", "connected", "devices", "cross-device"],
            KindHint = TweakKind.ServiceControl,
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("sc", ["stop", "CDPSvc"]);
                Elevation.RunElevated("sc", ["config", "CDPSvc", "start=", "disabled"]);
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("sc", ["config", "CDPSvc", "start=", "auto"]);
                Elevation.RunElevated("sc", ["start", "CDPSvc"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("sc", ["qc", "CDPSvc"]);
                return stdout.Contains("DISABLED", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "svc-disable-upnphost",
            Label = "Disable UPnP Device Host Service",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the UPnP Device Host service. Prevents this machine from acting as a discoverable UPnP host, reducing the attack surface on untrusted networks.",
            Tags = ["services", "upnp", "network", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\upnphost"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\upnphost", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\upnphost", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\upnphost", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-fdphost",
            Label = "Disable Function Discovery Provider Host",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the fdPHost service. Stops Windows from using WS-Discovery and other protocols to automatically find networked printers and devices.",
            Tags = ["services", "fdphost", "discovery", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\fdPHost"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\fdPHost", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\fdPHost", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\fdPHost", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-fdrespub",
            Label = "Disable Function Discovery Resource Publication Service",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the FDResPub service. Prevents this machine from advertising itself on the local network via WS-Discovery, removing it from the Network neighbourhood of other PCs.",
            Tags = ["services", "fdrespub", "publication", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\FDResPub"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\FDResPub", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\FDResPub", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\FDResPub", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-icssvc",
            Label = "Disable Internet Connection Sharing (ICS)",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the SharedAccess (ICS) service. Removes Windows' built-in NAT router capability, preventing accidental or unauthorised sharing of the internet connection.",
            Tags = ["services", "ics", "sharing", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-mapbroker",
            Label = "Disable Downloaded Maps Manager Service",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the MapsBroker service. Stops Windows from periodically downloading offline map data updates in the background.",
            Tags = ["services", "maps", "offline", "bandwidth"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\MapsBroker"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\MapsBroker", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\MapsBroker", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\MapsBroker", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-remoteaccess",
            Label = "Disable Routing and Remote Access (RRAS) Service",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the RemoteAccess (RRAS) service. Stops Windows from acting as a software router/VPN server. Not needed on standard workstations.",
            Tags = ["services", "routing", "vpn", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\RemoteAccess"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\RemoteAccess", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\RemoteAccess", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\RemoteAccess", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-wisvc",
            Label = "Disable Windows Insider Service",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the wisvc (Windows Insider Service). Prevents the device from being enrolled in Windows Insider preview flight deliveries and associated telemetry collection.",
            Tags = ["services", "insider", "preview", "telemetry"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\wisvc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\wisvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\wisvc", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\wisvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-autotimesvc",
            Label = "Disable Cellular Time Synchronisation Service",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the autotimesvc (Cellular Time) service. This service syncs the system clock via mobile-broadband data — not needed on non-cellular or always-connected PCs.",
            Tags = ["services", "time", "cellular", "sync"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\autotimesvc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\autotimesvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\autotimesvc", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\autotimesvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-napagent",
            Label = "Disable Network Access Protection (NAP) Agent",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the napagent (Network Access Protection Agent) service. NAP is deprecated since Windows Server 2012 R2 and the agent is unused on modern workstations.",
            Tags = ["services", "nap", "legacy", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\napagent"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\napagent", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\napagent", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\napagent", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-tzautoupdate",
            Label = "Disable Automatic Time Zone Updater Service",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the tzautoupdate service. Prevents Windows from automatically adjusting the system time zone based on location data. Useful for servers and VMs where the time zone should be fixed.",
            Tags = ["services", "timezone", "automatic", "location"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\tzautoupdate"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\tzautoupdate", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\tzautoupdate", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\tzautoupdate", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-diagnosticshub",
            Label = "Disable Diagnostics Hub Standard Collector Service",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the diagnosticshub.standardcollector.service. This service collects real-time diagnostic events from ETW providers for Visual Studio profiling sessions — not needed outside of profiling.",
            Tags = ["services", "diagnostics", "etw", "profiling"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\diagnosticshub.standardcollector.service"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\diagnosticshub.standardcollector.service", "Start", 4)],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\diagnosticshub.standardcollector.service", "Start", 3),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\diagnosticshub.standardcollector.service", "Start", 4),
            ],
        },
    ];
}

