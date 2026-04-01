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
    ];
}
