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
            Category = "System 1",
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
            Category = "System 1",
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
            Category = "System 1",
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
            Category = "System 1",
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
            Category = "System 1",
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
            Category = "System 1",
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
            Category = "System 1",
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
            Category = "System 1",
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
            Category = "System 1",
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
            Category = "System 1",
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
            Category = "System 1",
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
            Category = "System 1",
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
            Category = "System 1",
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
            Category = "System 1",
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

        new TweakDef
        {
            Id = "explorer-ai-thumbnail",
            Label = "Register AI Image PerceivedType",
            Category = "System 1",
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
            Category = "System 1",
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
            Category = "System 1",
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
            Category = "System 1",
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
            Category = "System 1",
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
            Category = "System 1",
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
            Category = "System 1",
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
            Category = "System 1",
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
            Category = "System 1",
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
            Category = "System 1",
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
            Category = "System 1",
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
            Category = "System 1",
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
            Category = "System 1",
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
