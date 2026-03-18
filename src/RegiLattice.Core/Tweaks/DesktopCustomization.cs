namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

/// <summary>
/// Desktop shell customization tweaks — Explorer behaviour, Quick Access,
/// ribbon, status bar, folder view, Recent/Frequent, notifications, and
/// other desktop-level UX tweaks.
/// Sprint 25 — Phase 5 roadmap items.
/// </summary>
internal static class DesktopCustomization
{
    private const string Explorer = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer";
    private const string Advanced = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced";
    private const string CabinetState = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\CabinetState";
    private const string Policies = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer";
    private const string Ribbon = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Ribbon";
    private const string ContentDelivery = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\ContentDeliveryManager";
    private const string Search = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Search";
    private const string Feeds = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Feeds";
    private const string PenWorkspace = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\PenWorkspace";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        // ── Explorer View Options ────────────────────────────────────────

        new TweakDef
        {
            Id = "dtcust-show-hidden-files",
            Label = "Show Hidden Files and Folders",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Makes hidden files and folders visible in File Explorer.",
            Tags = ["desktop", "explorer", "hidden", "files"],
            RegistryKeys = [Advanced],
            ApplyOps = [RegOp.SetDword(Advanced, "Hidden", 1)],
            RemoveOps = [RegOp.SetDword(Advanced, "Hidden", 2)],
            DetectOps = [RegOp.CheckDword(Advanced, "Hidden", 1)],
        },
        new TweakDef
        {
            Id = "dtcust-show-super-hidden",
            Label = "Show Protected OS Files",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Shows protected operating system files in Explorer (e.g., desktop.ini, thumbs.db).",
            Tags = ["desktop", "explorer", "protected", "system"],
            RegistryKeys = [Advanced],
            ApplyOps = [RegOp.SetDword(Advanced, "ShowSuperHidden", 1)],
            RemoveOps = [RegOp.SetDword(Advanced, "ShowSuperHidden", 0)],
            DetectOps = [RegOp.CheckDword(Advanced, "ShowSuperHidden", 1)],
        },
        new TweakDef
        {
            Id = "dtcust-show-full-path-title",
            Label = "Show Full Path in Explorer Title Bar",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Displays the complete folder path in the Explorer title bar instead of just the folder name.",
            Tags = ["desktop", "explorer", "path", "titlebar"],
            RegistryKeys = [CabinetState],
            ApplyOps = [RegOp.SetDword(CabinetState, "FullPath", 1)],
            RemoveOps = [RegOp.SetDword(CabinetState, "FullPath", 0)],
            DetectOps = [RegOp.CheckDword(CabinetState, "FullPath", 1)],
        },
        new TweakDef
        {
            Id = "dtcust-show-full-path-address",
            Label = "Show Full Path in Explorer Address Bar",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Uses the full file system path in Explorer's address bar instead of breadcrumb navigation.",
            Tags = ["desktop", "explorer", "path", "address"],
            RegistryKeys = [CabinetState],
            ApplyOps = [RegOp.SetDword(CabinetState, "FullPathAddress", 1)],
            RemoveOps = [RegOp.SetDword(CabinetState, "FullPathAddress", 0)],
            DetectOps = [RegOp.CheckDword(CabinetState, "FullPathAddress", 1)],
        },
        new TweakDef
        {
            Id = "dtcust-show-status-bar",
            Label = "Show Explorer Status Bar",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Always displays the status bar at the bottom of Explorer windows showing item info.",
            Tags = ["desktop", "explorer", "statusbar"],
            RegistryKeys = [Advanced],
            ApplyOps = [RegOp.SetDword(Advanced, "ShowStatusBar", 1)],
            RemoveOps = [RegOp.SetDword(Advanced, "ShowStatusBar", 0)],
            DetectOps = [RegOp.CheckDword(Advanced, "ShowStatusBar", 1)],
        },
        new TweakDef
        {
            Id = "dtcust-launch-to-this-pc",
            Label = "Open Explorer to 'This PC' (Not Quick Access)",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Opens File Explorer to This PC view instead of Quick Access on launch.",
            Tags = ["desktop", "explorer", "thispc", "launch"],
            RegistryKeys = [Advanced],
            ApplyOps = [RegOp.SetDword(Advanced, "LaunchTo", 1)],
            RemoveOps = [RegOp.SetDword(Advanced, "LaunchTo", 2)],
            DetectOps = [RegOp.CheckDword(Advanced, "LaunchTo", 1)],
        },
        new TweakDef
        {
            Id = "dtcust-disable-quick-access-recent",
            Label = "Disable Recent Files in Quick Access",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Stops Quick Access from showing recently opened files.",
            Tags = ["desktop", "explorer", "quick-access", "recent"],
            RegistryKeys = [Explorer],
            ApplyOps = [RegOp.SetDword(Explorer, "ShowRecent", 0)],
            RemoveOps = [RegOp.SetDword(Explorer, "ShowRecent", 1)],
            DetectOps = [RegOp.CheckDword(Explorer, "ShowRecent", 0)],
        },
        new TweakDef
        {
            Id = "dtcust-disable-quick-access-frequent",
            Label = "Disable Frequent Folders in Quick Access",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Stops Quick Access from showing frequently accessed folders.",
            Tags = ["desktop", "explorer", "quick-access", "frequent"],
            RegistryKeys = [Explorer],
            ApplyOps = [RegOp.SetDword(Explorer, "ShowFrequent", 0)],
            RemoveOps = [RegOp.SetDword(Explorer, "ShowFrequent", 1)],
            DetectOps = [RegOp.CheckDword(Explorer, "ShowFrequent", 0)],
        },
        new TweakDef
        {
            Id = "dtcust-show-file-extensions",
            Label = "Always Show File Name Extensions",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Displays file extensions (.txt, .exe, .pdf) for all files in Explorer.",
            Tags = ["desktop", "explorer", "extensions", "security"],
            RegistryKeys = [Advanced],
            ApplyOps = [RegOp.SetDword(Advanced, "HideFileExt", 0)],
            RemoveOps = [RegOp.SetDword(Advanced, "HideFileExt", 1)],
            DetectOps = [RegOp.CheckDword(Advanced, "HideFileExt", 0)],
        },
        new TweakDef
        {
            Id = "dtcust-show-merge-conflicts",
            Label = "Show Folder Merge Conflicts",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prompts for merge conflicts when copying folders with overlapping file names.",
            Tags = ["desktop", "explorer", "merge", "conflict"],
            RegistryKeys = [Advanced],
            ApplyOps = [RegOp.SetDword(Advanced, "HideMergeConflicts", 0)],
            RemoveOps = [RegOp.SetDword(Advanced, "HideMergeConflicts", 1)],
            DetectOps = [RegOp.CheckDword(Advanced, "HideMergeConflicts", 0)],
        },
        // ── Compact View & Layout ────────────────────────────────────────

        new TweakDef
        {
            Id = "dtcust-compact-view",
            Label = "Use Compact View in Explorer",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Reduces spacing between items in Explorer for a denser file view (Win11 inflated spacing).",
            Tags = ["desktop", "explorer", "compact", "spacing"],
            RegistryKeys = [Advanced],
            ApplyOps = [RegOp.SetDword(Advanced, "UseCompactMode", 1)],
            RemoveOps = [RegOp.SetDword(Advanced, "UseCompactMode", 0)],
            DetectOps = [RegOp.CheckDword(Advanced, "UseCompactMode", 1)],
        },
        new TweakDef
        {
            Id = "dtcust-checkbox-selection",
            Label = "Use Checkboxes to Select Items",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Adds checkboxes next to files and folders for easy multi-selection.",
            Tags = ["desktop", "explorer", "checkbox", "selection"],
            RegistryKeys = [Advanced],
            ApplyOps = [RegOp.SetDword(Advanced, "AutoCheckSelect", 1)],
            RemoveOps = [RegOp.SetDword(Advanced, "AutoCheckSelect", 0)],
            DetectOps = [RegOp.CheckDword(Advanced, "AutoCheckSelect", 1)],
        },
        new TweakDef
        {
            Id = "dtcust-expand-to-current-folder",
            Label = "Expand Navigation Pane to Current Folder",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Automatically expands the navigation pane to highlight the current folder location.",
            Tags = ["desktop", "explorer", "navigation", "expand"],
            RegistryKeys = [Advanced],
            ApplyOps = [RegOp.SetDword(Advanced, "NavPaneExpandToCurrentFolder", 1)],
            RemoveOps = [RegOp.SetDword(Advanced, "NavPaneExpandToCurrentFolder", 0)],
            DetectOps = [RegOp.CheckDword(Advanced, "NavPaneExpandToCurrentFolder", 1)],
        },
        new TweakDef
        {
            Id = "dtcust-show-all-folders-nav",
            Label = "Show All Folders in Navigation Pane",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Displays all folders (including Control Panel, Recycle Bin) in the navigation pane.",
            Tags = ["desktop", "explorer", "navigation", "folders"],
            RegistryKeys = [Advanced],
            ApplyOps = [RegOp.SetDword(Advanced, "NavPaneShowAllFolders", 1)],
            RemoveOps = [RegOp.SetDword(Advanced, "NavPaneShowAllFolders", 0)],
            DetectOps = [RegOp.CheckDword(Advanced, "NavPaneShowAllFolders", 1)],
        },
        // ── Ribbon & Toolbar ─────────────────────────────────────────────

        new TweakDef
        {
            Id = "dtcust-minimize-ribbon",
            Label = "Minimise Explorer Ribbon by Default",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Collapses the ribbon toolbar in Explorer by default, giving more space to file content.",
            Tags = ["desktop", "explorer", "ribbon", "minimize"],
            RegistryKeys = [Ribbon],
            ApplyOps = [RegOp.SetDword(Ribbon, "MinimizedStateTabletModeOff", 1)],
            RemoveOps = [RegOp.SetDword(Ribbon, "MinimizedStateTabletModeOff", 0)],
            DetectOps = [RegOp.CheckDword(Ribbon, "MinimizedStateTabletModeOff", 1)],
        },
        // ── Taskbar & System Tray ────────────────────────────────────────

        new TweakDef
        {
            Id = "dtcust-show-seconds-clock",
            Label = "Show Seconds in Taskbar Clock",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Displays seconds (HH:MM:SS) in the taskbar system clock.",
            Tags = ["desktop", "taskbar", "clock", "seconds"],
            RegistryKeys = [Advanced],
            ApplyOps = [RegOp.SetDword(Advanced, "ShowSecondsInSystemClock", 1)],
            RemoveOps = [RegOp.SetDword(Advanced, "ShowSecondsInSystemClock", 0)],
            DetectOps = [RegOp.CheckDword(Advanced, "ShowSecondsInSystemClock", 1)],
        },
        new TweakDef
        {
            Id = "dtcust-small-taskbar-icons",
            Label = "Use Small Taskbar Icons",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Uses smaller icons on the taskbar, reducing its height (Win10 style, may not work on Win11).",
            Tags = ["desktop", "taskbar", "icons", "small"],
            RegistryKeys = [Advanced],
            ApplyOps = [RegOp.SetDword(Advanced, "TaskbarSmallIcons", 1)],
            RemoveOps = [RegOp.SetDword(Advanced, "TaskbarSmallIcons", 0)],
            DetectOps = [RegOp.CheckDword(Advanced, "TaskbarSmallIcons", 1)],
        },
        new TweakDef
        {
            Id = "dtcust-hide-taskbar-search",
            Label = "Hide Taskbar Search Box",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Completely hides the search box/icon from the taskbar.",
            Tags = ["desktop", "taskbar", "search", "hide"],
            RegistryKeys = [Search],
            ApplyOps = [RegOp.SetDword(Search, "SearchboxTaskbarMode", 0)],
            RemoveOps = [RegOp.SetDword(Search, "SearchboxTaskbarMode", 2)],
            DetectOps = [RegOp.CheckDword(Search, "SearchboxTaskbarMode", 0)],
        },
        new TweakDef
        {
            Id = "dtcust-search-icon-only",
            Label = "Taskbar Search: Icon Only",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Shows just the search icon (magnifying glass) instead of the full search box.",
            Tags = ["desktop", "taskbar", "search", "icon"],
            RegistryKeys = [Search],
            ApplyOps = [RegOp.SetDword(Search, "SearchboxTaskbarMode", 1)],
            RemoveOps = [RegOp.SetDword(Search, "SearchboxTaskbarMode", 2)],
            DetectOps = [RegOp.CheckDword(Search, "SearchboxTaskbarMode", 1)],
        },
        new TweakDef
        {
            Id = "dtcust-hide-task-view-button",
            Label = "Hide Task View Button",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes the Task View (virtual desktops) button from the taskbar.",
            Tags = ["desktop", "taskbar", "task-view", "hide"],
            RegistryKeys = [Advanced],
            ApplyOps = [RegOp.SetDword(Advanced, "ShowTaskViewButton", 0)],
            RemoveOps = [RegOp.SetDword(Advanced, "ShowTaskViewButton", 1)],
            DetectOps = [RegOp.CheckDword(Advanced, "ShowTaskViewButton", 0)],
        },
        new TweakDef
        {
            Id = "dtcust-hide-widgets-button",
            Label = "Hide Widgets Button from Taskbar",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes the Widgets button/panel from the taskbar on Windows 11.",
            Tags = ["desktop", "taskbar", "widgets", "hide"],
            RegistryKeys = [Advanced],
            ApplyOps = [RegOp.SetDword(Advanced, "TaskbarDa", 0)],
            RemoveOps = [RegOp.SetDword(Advanced, "TaskbarDa", 1)],
            DetectOps = [RegOp.CheckDword(Advanced, "TaskbarDa", 0)],
        },
        new TweakDef
        {
            Id = "dtcust-hide-chat-button",
            Label = "Hide Chat (Teams) Button from Taskbar",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes the Chat (Microsoft Teams) button from the Windows 11 taskbar.",
            Tags = ["desktop", "taskbar", "chat", "teams"],
            RegistryKeys = [Advanced],
            ApplyOps = [RegOp.SetDword(Advanced, "TaskbarMn", 0)],
            RemoveOps = [RegOp.SetDword(Advanced, "TaskbarMn", 1)],
            DetectOps = [RegOp.CheckDword(Advanced, "TaskbarMn", 0)],
        },
        new TweakDef
        {
            Id = "dtcust-taskbar-never-combine",
            Label = "Never Combine Taskbar Buttons",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Shows separate taskbar buttons for each window instead of grouping by application.",
            Tags = ["desktop", "taskbar", "combine", "buttons"],
            RegistryKeys = [Advanced],
            ApplyOps = [RegOp.SetDword(Advanced, "TaskbarGlomLevel", 2)],
            RemoveOps = [RegOp.SetDword(Advanced, "TaskbarGlomLevel", 0)],
            DetectOps = [RegOp.CheckDword(Advanced, "TaskbarGlomLevel", 2)],
        },
        // ── Start Menu ───────────────────────────────────────────────────

        new TweakDef
        {
            Id = "dtcust-disable-start-suggestions",
            Label = "Disable Start Menu Suggestions",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes app suggestions and ads from the Start menu's recommended section.",
            Tags = ["desktop", "start", "suggestions", "ads"],
            RegistryKeys = [ContentDelivery],
            ApplyOps =
            [
                RegOp.SetDword(ContentDelivery, "SystemPaneSuggestionsEnabled", 0),
                RegOp.SetDword(ContentDelivery, "SubscribedContent-338388Enabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(ContentDelivery, "SystemPaneSuggestionsEnabled"),
                RegOp.DeleteValue(ContentDelivery, "SubscribedContent-338388Enabled"),
            ],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SystemPaneSuggestionsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "dtcust-disable-start-bing-search",
            Label = "Disable Bing Search in Start Menu",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Start menu search from sending queries to Bing — local results only.",
            Tags = ["desktop", "start", "bing", "search", "privacy"],
            RegistryKeys = [Search],
            ApplyOps = [RegOp.SetDword(Search, "BingSearchEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(Search, "BingSearchEnabled")],
            DetectOps = [RegOp.CheckDword(Search, "BingSearchEnabled", 0)],
        },
        // ── Notifications & Action Centre ────────────────────────────────

        new TweakDef
        {
            Id = "dtcust-disable-action-center",
            Label = "Disable Action Centre (Notification Panel)",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Hides the notification/action centre panel from the taskbar.",
            Tags = ["desktop", "action-center", "notifications", "hide"],
            RegistryKeys = [Policies],
            ApplyOps = [RegOp.SetDword(Policies, "DisableNotificationCenter", 1)],
            RemoveOps = [RegOp.DeleteValue(Policies, "DisableNotificationCenter")],
            DetectOps = [RegOp.CheckDword(Policies, "DisableNotificationCenter", 1)],
        },
        new TweakDef
        {
            Id = "dtcust-disable-news-feed",
            Label = "Disable News and Interests Feed",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the news and interests widget/feed from the taskbar.",
            Tags = ["desktop", "news", "feed", "taskbar"],
            RegistryKeys = [Feeds],
            ApplyOps = [RegOp.SetDword(Feeds, "ShellFeedsTaskbarViewMode", 2)],
            RemoveOps = [RegOp.SetDword(Feeds, "ShellFeedsTaskbarViewMode", 0)],
            DetectOps = [RegOp.CheckDword(Feeds, "ShellFeedsTaskbarViewMode", 2)],
        },
        // ── Pen, Touch & Misc ────────────────────────────────────────────

        new TweakDef
        {
            Id = "dtcust-disable-pen-workspace",
            Label = "Disable Windows Ink Workspace",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the Windows Ink Workspace button and functionality from the taskbar.",
            Tags = ["desktop", "ink", "pen", "workspace"],
            RegistryKeys = [PenWorkspace],
            ApplyOps = [RegOp.SetDword(PenWorkspace, "PenWorkspaceButtonDesiredVisibility", 0)],
            RemoveOps = [RegOp.SetDword(PenWorkspace, "PenWorkspaceButtonDesiredVisibility", 1)],
            DetectOps = [RegOp.CheckDword(PenWorkspace, "PenWorkspaceButtonDesiredVisibility", 0)],
        },
        new TweakDef
        {
            Id = "dtcust-disable-suggestions-lockscreen",
            Label = "Disable Lock Screen Suggestions",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes fun facts, tips, and ads from the Windows lock screen.",
            Tags = ["desktop", "lockscreen", "suggestions", "ads"],
            RegistryKeys = [ContentDelivery],
            ApplyOps = [RegOp.SetDword(ContentDelivery, "RotatingLockScreenOverlayEnabled", 0)],
            RemoveOps = [RegOp.SetDword(ContentDelivery, "RotatingLockScreenOverlayEnabled", 1)],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "RotatingLockScreenOverlayEnabled", 0)],
        },
        new TweakDef
        {
            Id = "dtcust-disable-app-suggestions",
            Label = "Disable Suggested Apps (Silently Installed)",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows from silently installing suggested apps like Candy Crush in the Start menu.",
            Tags = ["desktop", "apps", "suggestions", "bloat"],
            RegistryKeys = [ContentDelivery],
            ApplyOps =
            [
                RegOp.SetDword(ContentDelivery, "SilentInstalledAppsEnabled", 0),
                RegOp.SetDword(ContentDelivery, "OemPreInstalledAppsEnabled", 0),
                RegOp.SetDword(ContentDelivery, "PreInstalledAppsEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(ContentDelivery, "SilentInstalledAppsEnabled"),
                RegOp.DeleteValue(ContentDelivery, "OemPreInstalledAppsEnabled"),
                RegOp.DeleteValue(ContentDelivery, "PreInstalledAppsEnabled"),
            ],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SilentInstalledAppsEnabled", 0)],
        },
        // ── File Operations ──────────────────────────────────────────────

        new TweakDef
        {
            Id = "dtcust-always-show-transfer-details",
            Label = "Always Show File Transfer Details",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Automatically expands the details section in file copy/move dialogs.",
            Tags = ["desktop", "explorer", "copy", "transfer"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\OperationStatusManager"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\OperationStatusManager", "EnthusiastMode", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\OperationStatusManager", "EnthusiastMode", 0),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\OperationStatusManager", "EnthusiastMode", 1),
            ],
        },
        new TweakDef
        {
            Id = "dtcust-disable-sharing-wizard",
            Label = "Disable Sharing Wizard",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Uses the classic security tab instead of the simplified sharing wizard for file/folder sharing.",
            Tags = ["desktop", "explorer", "sharing", "wizard"],
            RegistryKeys = [Advanced],
            ApplyOps = [RegOp.SetDword(Advanced, "SharingWizardOn", 0)],
            RemoveOps = [RegOp.SetDword(Advanced, "SharingWizardOn", 1)],
            DetectOps = [RegOp.CheckDword(Advanced, "SharingWizardOn", 0)],
        },
        // ── Recycle Bin & Thumbnails ─────────────────────────────────────

        new TweakDef
        {
            Id = "dtcust-skip-recycle-bin",
            Label = "Skip Recycle Bin (Delete Directly)",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = false,
            Description = "Deletes files directly without sending to Recycle Bin. Use with caution.",
            Tags = ["desktop", "recycle", "bin", "delete"],
            RegistryKeys = [Policies],
            ApplyOps = [RegOp.SetDword(Policies, "NoRecycleFiles", 1)],
            RemoveOps = [RegOp.DeleteValue(Policies, "NoRecycleFiles")],
            DetectOps = [RegOp.CheckDword(Policies, "NoRecycleFiles", 1)],
        },
        new TweakDef
        {
            Id = "dtcust-disable-delete-confirmation",
            Label = "Disable Delete Confirmation Dialog",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sends files to Recycle Bin without the 'Are you sure?' prompt.",
            Tags = ["desktop", "delete", "confirmation", "dialog"],
            RegistryKeys = [Policies],
            ApplyOps = [RegOp.SetDword(Policies, "ConfirmFileDelete", 0)],
            RemoveOps = [RegOp.SetDword(Policies, "ConfirmFileDelete", 1)],
            DetectOps = [RegOp.CheckDword(Policies, "ConfirmFileDelete", 0)],
        },
        new TweakDef
        {
            Id = "dtcust-disable-thumbnail-cache",
            Label = "Disable Thumbnail Cache",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Explorer from creating thumbs.db thumbnail cache files.",
            Tags = ["desktop", "explorer", "thumbnails", "cache"],
            RegistryKeys = [Advanced],
            ApplyOps = [RegOp.SetDword(Advanced, "DisableThumbnailCache", 1)],
            RemoveOps = [RegOp.SetDword(Advanced, "DisableThumbnailCache", 0)],
            DetectOps = [RegOp.CheckDword(Advanced, "DisableThumbnailCache", 1)],
        },
        new TweakDef
        {
            Id = "dtcust-always-show-icons-never-thumbnails",
            Label = "Always Show Icons, Never Thumbnails",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Shows file type icons instead of image/video thumbnails in Explorer for faster browsing.",
            Tags = ["desktop", "explorer", "icons", "thumbnails"],
            RegistryKeys = [Advanced],
            ApplyOps = [RegOp.SetDword(Advanced, "IconsOnly", 1)],
            RemoveOps = [RegOp.SetDword(Advanced, "IconsOnly", 0)],
            DetectOps = [RegOp.CheckDword(Advanced, "IconsOnly", 1)],
        },
    ];
}
