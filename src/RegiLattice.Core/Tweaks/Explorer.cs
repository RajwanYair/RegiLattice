namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Explorer
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "explorer-show-hidden-files",
            Label = "Show Hidden Files",
            Category = "Explorer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Shows hidden files and folders in Explorer.",
            Tags = ["explorer", "files"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Hidden", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Hidden", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Hidden", 1)],
        },
        new TweakDef
        {
            Id = "explorer-show-super-hidden",
            Label = "Show Protected OS Files",
            Category = "Explorer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Shows protected operating system files (super hidden).",
            Tags = ["explorer", "files", "advanced"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowSuperHidden", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowSuperHidden", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowSuperHidden", 1)],
        },
        new TweakDef
        {
            Id = "explorer-open-this-pc",
            Label = "Open Explorer to This PC",
            Category = "Explorer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Opens File Explorer to 'This PC' instead of Quick Access.",
            Tags = ["explorer", "navigation"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "LaunchTo", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "LaunchTo", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "LaunchTo", 1)],
        },
        new TweakDef
        {
            Id = "explorer-disable-thumbnails",
            Label = "Disable Folder Thumbnails",
            Category = "Explorer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Shows icons instead of thumbnails for faster folder browsing.",
            Tags = ["explorer", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "IconsOnly", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "IconsOnly", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "IconsOnly", 1)],
        },
        new TweakDef
        {
            Id = "explorer-full-path-title",
            Label = "Full Path in Title Bar",
            Category = "Explorer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Shows the full folder path in the Explorer title bar.",
            Tags = ["explorer", "navigation"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\CabinetState"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\CabinetState", "FullPath", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\CabinetState", "FullPath", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\CabinetState", "FullPath", 1)],
        },
        new TweakDef
        {
            Id = "explorer-disable-recent-files",
            Label = "Disable Recent Files in Quick Access",
            Category = "Explorer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables recent and frequent files from appearing in Quick Access.",
            Tags = ["explorer", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "ShowRecent", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "ShowFrequent", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "ShowRecent", 1),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "ShowFrequent", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "ShowRecent", 0)],
        },
        new TweakDef
        {
            Id = "explorer-disable-search-history",
            Label = "Disable Search History",
            Category = "Explorer",
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
            Category = "Explorer",
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
            Id = "explorer-compact-view",
            Label = "Enable Compact View",
            Category = "Explorer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Reduces item spacing in Explorer for a denser file list.",
            Tags = ["explorer", "win11"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "UseCompactMode", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "UseCompactMode", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "UseCompactMode", 1)],
        },
        new TweakDef
        {
            Id = "explorer-disable-auto-folder-type",
            Label = "Disable Auto Folder Type Detection",
            Category = "Explorer",
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
            Id = "explorer-disable-breadcrumbs",
            Label = "Disable Breadcrumb Bar",
            Category = "Explorer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Shows the full path in Explorer address bar instead of breadcrumbs.",
            Tags = ["explorer", "navigation"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\CabinetState"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\CabinetState", "FullPath", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\CabinetState", "FullPath")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\CabinetState", "FullPath", 1)],
        },
        new TweakDef
        {
            Id = "explorer-disable-merge-conflicts",
            Label = "Disable Folder Merge Conflicts",
            Category = "Explorer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Hides folder merge conflict prompts when copying/moving folders.",
            Tags = ["explorer", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "HideMergeConflicts", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "HideMergeConflicts", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "HideMergeConflicts", 1)],
        },
        new TweakDef
        {
            Id = "explorer-thumbnail-performance",
            Label = "Optimize Thumbnail Caching & Quality",
            Category = "Explorer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Optimizes Explorer thumbnail display: keeps thumbnail cache, increases size to 256px and quality to 100%, disables thumbs.db on network folders. Results in sharper, faster file previews. Default: 96px low quality. Recommended: 256px max quality.",
            Tags = ["explorer", "thumbnails", "performance", "quality"],
            RegistryKeys =
            [
                @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer",
                @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced",
            ],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisableThumbnailCache", 0),
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced",
                    "DisableThumbsDBOnNetworkFolders",
                    1
                ),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "ThumbnailSize", 256),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "ThumbnailQuality", 100),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisableThumbnailCache"),
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced",
                    "DisableThumbsDBOnNetworkFolders",
                    0
                ),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "ThumbnailSize"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "ThumbnailQuality"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "ThumbnailSize", 256)],
        },
        new TweakDef
        {
            Id = "explorer-show-status-bar",
            Label = "Show Explorer Status Bar",
            Category = "Explorer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Enables the status bar at the bottom of Explorer windows showing selected item count, size, and free space. Default: Hidden. Recommended: Shown.",
            Tags = ["explorer", "ux", "status-bar"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowStatusBar", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowStatusBar", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowStatusBar", 1)],
        },
        new TweakDef
        {
            Id = "explorer-disable-recent-docs",
            Label = "Disable Recent Documents History",
            Category = "Explorer",
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
            Id = "explorer-disable-thumbnail-cache",
            Label = "Disable Thumbnail Cache",
            Category = "Explorer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables thumbnail cache (thumbs.db) creation in folders. Reduces disk writes and avoids locked files on network shares. Default: Enabled. Recommended: Disabled on SSDs/network drives.",
            Tags = ["explorer", "thumbnails", "cache", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisableThumbnailCache", 1)],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisableThumbnailCache", 0),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisableThumbnailCache", 1),
            ],
        },
        new TweakDef
        {
            Id = "explorer-disable-quick-access",
            Label = "Disable Quick Access Recent Files",
            Category = "Explorer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Quick Access recent and frequent files display. Improves privacy and reduces Explorer clutter. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["explorer", "quick-access", "recent", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "ShowRecent", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "ShowFrequent", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "ShowRecent", 1),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "ShowFrequent", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "ShowRecent", 0),
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "ShowFrequent", 0),
            ],
        },
        new TweakDef
        {
            Id = "explorer-disable-thumb-cache-cleanup",
            Label = "Disable Thumbnail Cache Auto-Cleanup",
            Category = "Explorer",
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
            Id = "explorer-navpane-expand",
            Label = "Expand Nav Pane to Current Folder",
            Category = "Explorer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Automatically expands the navigation pane tree to show the current folder location. Default: collapsed. Recommended: expanded.",
            Tags = ["explorer", "navigation", "pane", "expand", "folder"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "NavPaneExpandToCurrentFolder", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "NavPaneExpandToCurrentFolder", 0),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "NavPaneExpandToCurrentFolder", 1),
            ],
        },
        new TweakDef
        {
            Id = "explorer-folder-size-tips",
            Label = "Show File Size in Folder Tooltips",
            Category = "Explorer",
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
            Id = "explorer-disable-sharing-wizard",
            Label = "Disable Sharing Wizard",
            Category = "Explorer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the simplified sharing wizard and uses the advanced security permissions dialog instead. Default: enabled. Recommended: disabled.",
            Tags = ["explorer", "sharing", "wizard", "advanced", "permissions"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SharingWizardOn", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SharingWizardOn", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SharingWizardOn", 0)],
        },
        new TweakDef
        {
            Id = "explorer-disable-new-app-alert",
            Label = "Disable 'New App' Notifications",
            Category = "Explorer",
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
            Id = "explorer-checkbox-selection",
            Label = "Enable Item Check Boxes",
            Category = "Explorer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Shows checkboxes next to file and folder names for easier multi-selection without holding Ctrl. Default: disabled. Recommended: personal preference.",
            Tags = ["explorer", "checkbox", "selection", "multi-select"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "AutoCheckSelect", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "AutoCheckSelect", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "AutoCheckSelect", 1)],
        },
        new TweakDef
        {
            Id = "explorer-disable-sync-ads",
            Label = "Disable Sync Provider Ads",
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Id = "explorer-open-to-this-pc",
            Label = "Open Explorer to This PC",
            Category = "Explorer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Opens File Explorer to 'This PC' instead of 'Quick Access' or 'Home'. Default: Home (Win11) / Quick Access (Win10).",
            Tags = ["explorer", "this-pc", "quick-access", "home"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "LaunchTo", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "LaunchTo", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "LaunchTo", 1)],
        },
        new TweakDef
        {
            Id = "explorer-expand-to-open-folder",
            Label = "Expand Navigation Pane to Open Folder",
            Category = "Explorer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Automatically expands the navigation pane to the currently open folder. Default: collapsed.",
            Tags = ["explorer", "navigation", "expand", "folder"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "NavPaneExpandToCurrentFolder", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "NavPaneExpandToCurrentFolder", 0),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "NavPaneExpandToCurrentFolder", 1),
            ],
        },
        new TweakDef
        {
            Id = "explorer-disable-folder-type-discovery",
            Label = "Disable Automatic Folder Type Discovery",
            Category = "Explorer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Explorer from auto-detecting folder type (documents, music, pictures) and changing layout. Default: auto-detect.",
            Tags = ["explorer", "folder", "type", "discovery", "layout"],
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
        // ── Restored stubs with real registry operations ──────────────────

        new TweakDef
        {
            Id = "explorer-ai-thumbnail",
            Label = "Register AI Image PerceivedType",
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Id = "explorer-recent-places",
            Label = "Disable Recent Places Tracking",
            Category = "Explorer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Explorer from tracking recently accessed folders and showing them in Quick Access.",
            Tags = ["explorer", "recent", "places", "privacy", "quick-access"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackDocs", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackDocs")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackDocs", 0)],
        },
        new TweakDef
        {
            Id = "explorer-show-file-extensions",
            Label = "Show File Extensions",
            Category = "Explorer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Shows file name extensions for all known file types. Default: hidden. Recommended: shown.",
            Tags = ["explorer", "file-extensions", "security", "visibility"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "HideFileExt", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "HideFileExt")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "HideFileExt", 0)],
        },
        new TweakDef
        {
            Id = "explorer-stl-thumbnail",
            Label = "Register STL as 3D Model Type",
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Id = "clip-decrease-drag-threshold",
            Label = "Decrease Drag-Drop Threshold (2 px)",
            Category = "Explorer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Decreases drag start threshold to 2 pixels for easier dragging. Default: 4 pixels. Recommended: 2 (for touchscreen/pen).",
            Tags = ["clipboard", "drag", "drop", "sensitivity", "touch"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragWidth", "2"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragHeight", "2"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragWidth", "4"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragHeight", "4"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragWidth", "2")],
        },
        new TweakDef
        {
            Id = "clip-instant-drag-delay",
            Label = "Set Instant Drag Delay (0 ms)",
            Category = "Explorer",
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
            Category = "Explorer",
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
            Id = "clip-disable-cloud-clipboard",
            Label = "Disable Cloud Clipboard Sync",
            Category = "Explorer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables cloud clipboard sync and automatic upload. Prevents clipboard data from being sent to Microsoft cloud services. Default: enabled. Recommended: disabled for privacy.",
            Tags = ["clipboard", "cloud", "sync", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Clipboard"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "AllowCrossDeviceClipboard", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "AllowCrossDeviceClipboard")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "AllowCrossDeviceClipboard", 0)],
        },
        new TweakDef
        {
            Id = "clip-disable-clipboard-roaming",
            Label = "Disable Clipboard Roaming (Policy)",
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Id = "clip-drag-threshold-medium",
            Label = "Set Drag-Drop Minimum Distance (8 px)",
            Category = "Explorer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Increases the minimum mouse movement required to initiate a drag-drop operation from 4 px to 8 px. Prevents accidental dragging. Default: 4 px.",
            Tags = ["clipboard", "drag", "drop", "threshold", "mouse"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragWidth", "8"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragHeight", "8"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragWidth", "4"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragHeight", "4"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragWidth", "8")],
        },
        new TweakDef
        {
            Id = "clip-disable-clipboard-history",
            Label = "Disable Clipboard History",
            Category = "Explorer",
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
            Id = "clip-increase-drag-sensitivity",
            Label = "Increase Drag Sensitivity to 10px",
            Category = "Explorer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Increases the drag threshold from 4 to 10 pixels. Prevents accidental drag-and-drop. Default: 4 pixels.",
            Tags = ["clipboard", "drag", "sensitivity", "threshold"],
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
            Id = "clip-disable-drop-target-hovering",
            Label = "Disable Drop Target Window Activation",
            Category = "Explorer",
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
            Id = "clip-disable-clipboard-history-roaming",
            Label = "Disable Clipboard History Roaming",
            Category = "Explorer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents clipboard history from roaming across devices signed into the same Microsoft account. Default: user-configurable.",
            Tags = ["clipboard", "history", "roaming", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "AllowClipboardHistory", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "AllowClipboardHistory")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "AllowClipboardHistory", 0)],
        },
        new TweakDef
        {
            Id = "clip-set-drag-sensitivity-6",
            Label = "Increase Drag Sensitivity to 6px",
            Category = "Explorer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Increases the drag threshold to 6 pixels. Prevents accidental drag when clicking. Default: 4px.",
            Tags = ["clipboard", "drag", "sensitivity", "threshold"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragWidth", "6")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragWidth", "4")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragWidth", "6")],
        },
        new TweakDef
        {
            Id = "clip-disable-clipboard-experience",
            Label = "Disable Clipboard Experience UI",
            Category = "Explorer",
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
            Id = "clip-disable-cloud-sync",
            Label = "Disable Clipboard Cloud Sync",
            Category = "Explorer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables clipboard cloud synchronisation across devices via Group Policy. Prevents clipboard data from leaving the device. Default: allowed.",
            Tags = ["clipboard", "cloud", "sync", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "AllowCrossDeviceClipboard", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "AllowCrossDeviceClipboard")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "AllowCrossDeviceClipboard", 0)],
        },
        new TweakDef
        {
            Id = "clip-disable-history-policy",
            Label = "Disable Clipboard History (Policy)",
            Category = "Explorer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Clipboard History via Group Policy. Prevents Windows from storing clipboard entries. Default: allowed.",
            Tags = ["clipboard", "history", "policy", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "AllowClipboardHistory", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "AllowClipboardHistory")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "AllowClipboardHistory", 0)],
        },
        new TweakDef
        {
            Id = "clip-disable-roaming",
            Label = "Disable Clipboard Roaming",
            Category = "Explorer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables clipboard roaming (syncing clipboard content to other signed-in devices). Keeps clipboard data local. Default: enabled.",
            Tags = ["clipboard", "roaming", "sync", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Clipboard"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Clipboard", "CloudClipboardAutomaticUpload", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Clipboard", "CloudClipboardAutomaticUpload")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Clipboard", "CloudClipboardAutomaticUpload", 0)],
        },
        new TweakDef
        {
            Id = "clip-disable-suggestions",
            Label = "Disable Clipboard Suggestions",
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Id = "clip-disable-windows-ink-workspace",
            Label = "Disable Windows Ink Workspace",
            Category = "Explorer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Turns off the Windows Ink Workspace pen quick-launch overlay accessible from the taskbar.",
            Tags = ["clipboard", "ink", "pen", "taskbar"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace", "AllowWindowsInkWorkspace", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace", "AllowWindowsInkWorkspace")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace", "AllowWindowsInkWorkspace", 0)],
        },
        new TweakDef
        {
            Id = "clip-disable-typing-insights",
            Label = "Disable Typing Insights Collection",
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Id = "ctx-remove-send-to-compressed",
            Label = "Remove 'Compressed (zipped) folder' from Send To",
            Category = "Explorer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes the obsolete 'Compressed (zipped) folder' from the Send To menu. Use 7-Zip instead. Default: shown.",
            Tags = ["context-menu", "send-to", "zip", "remove"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "HideFileExt", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "HideFileExt")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "HideFileExt", 0)],
        },
        new TweakDef
        {
            Id = "ctx-remove-previous-versions",
            Label = "Remove 'Restore Previous Versions' from Context Menu",
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Id = "shell-compact-file-explorer",
            Label = "Enable Compact View in File Explorer",
            Category = "Explorer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables the compact layout in File Explorer, reducing padding between items.",
            Tags = ["shell", "explorer", "compact", "layout"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "UseCompactMode", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "UseCompactMode", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "UseCompactMode", 1)],
        },
        new TweakDef
        {
            Id = "shell-disable-ink-workspace",
            Label = "Disable Windows Ink Workspace",
            Category = "Explorer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Windows Ink Workspace (pen/touch drawing overlay). Frees resources on non-touch devices. Default: Enabled. Recommended: Disabled.",
            Tags = ["shell", "ink", "workspace", "pen"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace", "AllowWindowsInkWorkspace", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace", "AllowWindowsInkWorkspace")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace", "AllowWindowsInkWorkspace", 0)],
        },
        new TweakDef
        {
            Id = "shell-enable-dark-mode-console",
            Label = "Enable Dark Mode for Console Windows",
            Category = "Explorer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables dark mode for legacy console host windows. Default: light.",
            Tags = ["shell", "console", "dark-mode", "appearance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "ForceV2", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "ForceV2")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "ForceV2", 1)],
        },
        new TweakDef
        {
            Id = "shell-set-console-buffer-9999",
            Label = "Set Console Screen Buffer to 9999 Lines",
            Category = "Explorer",
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
            Id = "shell-enable-quickedit-mode",
            Label = "Enable Console QuickEdit Mode",
            Category = "Explorer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables QuickEdit mode in console windows. Allows mouse text selection. Default: enabled.",
            Tags = ["shell", "console", "quickedit", "mouse"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "QuickEdit", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "QuickEdit", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "QuickEdit", 1)],
        },
        new TweakDef
        {
            Id = "shell-disable-cmd-autorun",
            Label = "Disable CMD AutoRun Commands",
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Category = "Explorer",
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
            Id = "shell-disable-thumbnail-net-cache",
            Label = "Disable Thumbs.db on Network Folders",
            Category = "Explorer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets DisableThumbsDBOnNetworkFolders=1 in Explorer Advanced. Stops Windows from creating Thumbs.db thumbnail cache files on UNC and mapped network drives, which can cause file-locking issues for other users.",
            Tags = ["shell", "explorer", "thumbnail", "network"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced",
                    "DisableThumbsDBOnNetworkFolders",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced",
                    "DisableThumbsDBOnNetworkFolders",
                    0
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced",
                    "DisableThumbsDBOnNetworkFolders",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "shell-enable-numlock-startup",
            Label = "Enable Num Lock on Startup",
            Category = "Explorer",
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
            Id = "shell-disable-folder-info-tips",
            Label = "Disable Folder Info Tips in Explorer",
            Category = "Explorer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets FolderContentsInfoTip=0 in Explorer Advanced. Removes the tooltip that appears when hovering over a folder showing its file count and size — reduces Explorer popups.",
            Tags = ["shell", "explorer", "tooltip", "info"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "FolderContentsInfoTip", 0)],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "FolderContentsInfoTip", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "FolderContentsInfoTip", 0),
            ],
        },
        new TweakDef
        {
            Id = "shell-disable-sharing-wizard",
            Label = "Disable File Sharing Wizard",
            Category = "Explorer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets SharingWizardOn=0 in Explorer Advanced. Removes the simplified sharing wizard from context menus, giving direct access to advanced sharing permissions without the guided wizard.",
            Tags = ["shell", "explorer", "sharing", "network"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SharingWizardOn", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SharingWizardOn", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SharingWizardOn", 0)],
        },
        new TweakDef
        {
            Id = "shell-restore-previous-folders",
            Label = "Reopen Previous Folder Windows on Login",
            Category = "Explorer",
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
            Id = "shell-show-encrypted-color",
            Label = "Show Encrypted & Compressed Files in Color",
            Category = "Explorer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets ShowEncryptCompressedColor=1 in Explorer Advanced. Displays encrypted files in green and NTFS-compressed files in blue in Explorer, making protected content visually distinct.",
            Tags = ["shell", "explorer", "encryption", "color"],
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
            Id = "shell-disable-recent-docs-policy",
            Label = "Disable Recent Documents Tracking (Policy)",
            Category = "Explorer",
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
        new TweakDef
        {
            Id = "shell-show-status-bar",
            Label = "Show Status Bar in Explorer Windows",
            Category = "Explorer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets ShowStatusBar=1 in Explorer Advanced. Enables the bottom status bar in Explorer windows that shows selected item counts, sizes, and folder details.",
            Tags = ["shell", "explorer", "status-bar", "ui"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowStatusBar", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowStatusBar", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowStatusBar", 1)],
        },
    ];
}
