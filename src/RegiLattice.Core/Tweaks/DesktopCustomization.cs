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

// ── Merged from WindowAppearance.cs ──────────────────────────────────────────────────

internal static class WindowAppearance
{
    private const string Metrics = @"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics";
    private const string Desktop = @"HKEY_CURRENT_USER\Control Panel\Desktop";
    private const string Mouse = @"HKEY_CURRENT_USER\Control Panel\Mouse";
    private const string Dwm = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\DWM";
    private const string Explorer = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced";
    private const string Themes = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize";
    private const string Accessibility = @"HKEY_CURRENT_USER\Control Panel\Accessibility\StickyKeys";
    private const string Cursors = @"HKEY_CURRENT_USER\Control Panel\Cursors";
    private const string Colors = @"HKEY_CURRENT_USER\Control Panel\Colors";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        // ── Title Bar & Window Chrome ────────────────────────────────────

        new TweakDef
        {
            Id = "winapp-titlebar-color-active",
            Label = "Show Accent Color on Title Bars",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Colours active title bars and window borders with the system accent colour.",
            Tags = ["appearance", "titlebar", "accent", "color"],
            RegistryKeys = [Dwm],
            ApplyOps = [RegOp.SetDword(Dwm, "ColorPrevalence", 1)],
            RemoveOps = [RegOp.SetDword(Dwm, "ColorPrevalence", 0)],
            DetectOps = [RegOp.CheckDword(Dwm, "ColorPrevalence", 1)],
        },
        new TweakDef
        {
            Id = "winapp-titlebar-color-inactive",
            Label = "Show Accent Color on Inactive Title Bars",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Extends the accent colour to inactive window title bars for a uniform look.",
            Tags = ["appearance", "titlebar", "accent", "inactive"],
            RegistryKeys = [Dwm],
            ApplyOps = [RegOp.SetDword(Dwm, "AccentColorInactive", 1)],
            RemoveOps = [RegOp.DeleteValue(Dwm, "AccentColorInactive")],
            DetectOps = [RegOp.CheckDword(Dwm, "AccentColorInactive", 1)],
        },
        new TweakDef
        {
            Id = "winapp-start-taskbar-accent",
            Label = "Show Accent Color on Start and Taskbar",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Applies the system accent colour to the Start menu and taskbar background.",
            Tags = ["appearance", "accent", "start", "taskbar"],
            RegistryKeys = [Themes],
            ApplyOps = [RegOp.SetDword(Themes, "ColorPrevalence", 1)],
            RemoveOps = [RegOp.SetDword(Themes, "ColorPrevalence", 0)],
            DetectOps = [RegOp.CheckDword(Themes, "ColorPrevalence", 1)],
        },
        new TweakDef
        {
            Id = "winapp-disable-title-bar-flashing",
            Label = "Disable Title Bar Flashing",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents applications from flashing their title bar to attract attention.",
            Tags = ["appearance", "titlebar", "flash", "focus"],
            RegistryKeys = [Desktop],
            ApplyOps = [RegOp.SetDword(Desktop, "ForegroundFlashCount", 0)],
            RemoveOps = [RegOp.SetDword(Desktop, "ForegroundFlashCount", 3)],
            DetectOps = [RegOp.CheckDword(Desktop, "ForegroundFlashCount", 0)],
        },
        new TweakDef
        {
            Id = "winapp-disable-window-shake",
            Label = "Disable Aero Shake (Minimize on Shake)",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents shaking a window title bar from minimising all other windows.",
            Tags = ["appearance", "aero", "shake", "minimize"],
            RegistryKeys = [Explorer],
            ApplyOps = [RegOp.SetDword(Explorer, "DisallowShaking", 1)],
            RemoveOps = [RegOp.DeleteValue(Explorer, "DisallowShaking")],
            DetectOps = [RegOp.CheckDword(Explorer, "DisallowShaking", 1)],
        },
        // ── Scrollbar & Window Metrics ───────────────────────────────────

        new TweakDef
        {
            Id = "winapp-scrollbar-width-thin",
            Label = "Thin Scrollbars (13px)",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets scrollbar width to thin 13 pixels (default is 17). Requires sign-out.",
            Tags = ["appearance", "scrollbar", "width", "thin"],
            RegistryKeys = [Metrics],
            ApplyOps = [RegOp.SetString(Metrics, "ScrollWidth", "-195")],
            RemoveOps = [RegOp.SetString(Metrics, "ScrollWidth", "-255")],
            DetectOps = [RegOp.CheckString(Metrics, "ScrollWidth", "-195")],
            SideEffects = "Requires sign-out to take effect.",
        },
        new TweakDef
        {
            Id = "winapp-scrollbar-height-thin",
            Label = "Thin Scroll Arrows (13px)",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets scrollbar button height to thin 13 pixels (default is 17). Requires sign-out.",
            Tags = ["appearance", "scrollbar", "height", "thin"],
            RegistryKeys = [Metrics],
            ApplyOps = [RegOp.SetString(Metrics, "ScrollHeight", "-195")],
            RemoveOps = [RegOp.SetString(Metrics, "ScrollHeight", "-255")],
            DetectOps = [RegOp.CheckString(Metrics, "ScrollHeight", "-195")],
            SideEffects = "Requires sign-out to take effect.",
        },
        new TweakDef
        {
            Id = "winapp-border-width-thin",
            Label = "Thin Window Borders (1px)",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets window border width to 1 pixel (default is padded). Requires sign-out.",
            Tags = ["appearance", "border", "width", "thin"],
            RegistryKeys = [Metrics],
            ApplyOps = [RegOp.SetString(Metrics, "BorderWidth", "-15")],
            RemoveOps = [RegOp.SetString(Metrics, "BorderWidth", "-15")],
            DetectOps = [RegOp.CheckString(Metrics, "BorderWidth", "-15")],
            SideEffects = "Requires sign-out to take effect.",
        },
        new TweakDef
        {
            Id = "winapp-padded-border-zero",
            Label = "Remove Window Padding Border",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes the extra padded border around windows (-60 = 4px default, 0 = none). Requires sign-out.",
            Tags = ["appearance", "border", "padding"],
            RegistryKeys = [Metrics],
            ApplyOps = [RegOp.SetString(Metrics, "PaddedBorderWidth", "0")],
            RemoveOps = [RegOp.SetString(Metrics, "PaddedBorderWidth", "-60")],
            DetectOps = [RegOp.CheckString(Metrics, "PaddedBorderWidth", "0")],
            SideEffects = "Requires sign-out to take effect.",
        },
        new TweakDef
        {
            Id = "winapp-caption-height-compact",
            Label = "Compact Title Bar Height (20px)",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Reduces the title bar height to 20 pixels for more screen space (default ~22). Requires sign-out.",
            Tags = ["appearance", "titlebar", "height", "compact"],
            RegistryKeys = [Metrics],
            ApplyOps = [RegOp.SetString(Metrics, "CaptionHeight", "-300")],
            RemoveOps = [RegOp.SetString(Metrics, "CaptionHeight", "-330")],
            DetectOps = [RegOp.CheckString(Metrics, "CaptionHeight", "-300")],
            SideEffects = "Requires sign-out to take effect.",
        },
        // ── Icon Spacing ─────────────────────────────────────────────────

        new TweakDef
        {
            Id = "winapp-icon-spacing-h-compact",
            Label = "Compact Horizontal Icon Spacing (60px)",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Reduces horizontal desktop icon spacing to 60 pixels (default 75). Requires sign-out.",
            Tags = ["appearance", "icon", "spacing", "horizontal"],
            RegistryKeys = [Metrics],
            ApplyOps = [RegOp.SetString(Metrics, "IconSpacing", "-900")],
            RemoveOps = [RegOp.SetString(Metrics, "IconSpacing", "-1125")],
            DetectOps = [RegOp.CheckString(Metrics, "IconSpacing", "-900")],
            SideEffects = "Requires sign-out to take effect.",
        },
        new TweakDef
        {
            Id = "winapp-icon-spacing-v-compact",
            Label = "Compact Vertical Icon Spacing (60px)",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Reduces vertical desktop icon spacing to 60 pixels (default 75). Requires sign-out.",
            Tags = ["appearance", "icon", "spacing", "vertical"],
            RegistryKeys = [Metrics],
            ApplyOps = [RegOp.SetString(Metrics, "IconVerticalSpacing", "-900")],
            RemoveOps = [RegOp.SetString(Metrics, "IconVerticalSpacing", "-1125")],
            DetectOps = [RegOp.CheckString(Metrics, "IconVerticalSpacing", "-900")],
            SideEffects = "Requires sign-out to take effect.",
        },
        // ── Menu & Animation ─────────────────────────────────────────────

        new TweakDef
        {
            Id = "winapp-menu-show-delay-fast",
            Label = "Fast Menu Show Delay (100ms)",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Reduces the delay before sub-menus appear to 100ms (default 400ms).",
            Tags = ["appearance", "menu", "delay", "speed"],
            RegistryKeys = [Desktop],
            ApplyOps = [RegOp.SetString(Desktop, "MenuShowDelay", "100")],
            RemoveOps = [RegOp.SetString(Desktop, "MenuShowDelay", "400")],
            DetectOps = [RegOp.CheckString(Desktop, "MenuShowDelay", "100")],
        },
        new TweakDef
        {
            Id = "winapp-menu-show-delay-instant",
            Label = "Instant Menu Show Delay (0ms)",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Eliminates the delay before sub-menus appear (default 400ms).",
            Tags = ["appearance", "menu", "delay", "instant"],
            RegistryKeys = [Desktop],
            ApplyOps = [RegOp.SetString(Desktop, "MenuShowDelay", "0")],
            RemoveOps = [RegOp.SetString(Desktop, "MenuShowDelay", "400")],
            DetectOps = [RegOp.CheckString(Desktop, "MenuShowDelay", "0")],
        },
        new TweakDef
        {
            Id = "winapp-disable-menu-animations",
            Label = "Disable Menu Fade/Slide Animations",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables fade and slide effects on menus for instant display.",
            Tags = ["appearance", "menu", "animation", "fade"],
            RegistryKeys = [Desktop],
            ApplyOps = [RegOp.SetString(Desktop, "UserPreferencesMask", "9012038010000000")],
            RemoveOps = [RegOp.DeleteValue(Desktop, "UserPreferencesMask")],
            DetectOps = [RegOp.CheckString(Desktop, "UserPreferencesMask", "9012038010000000")],
        },
        new TweakDef
        {
            Id = "winapp-disable-window-animation",
            Label = "Disable Window Min/Max Animations",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the animation when minimising or maximising windows.",
            Tags = ["appearance", "window", "animation", "minimize", "maximize"],
            RegistryKeys = [Desktop],
            ApplyOps = [RegOp.SetString(Desktop, "MinAnimate", "0")],
            RemoveOps = [RegOp.SetString(Desktop, "MinAnimate", "1")],
            DetectOps = [RegOp.CheckString(Desktop, "MinAnimate", "0")],
        },
        new TweakDef
        {
            Id = "winapp-disable-cursor-blink",
            Label = "Disable Cursor Blinking",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Stops the text cursor from blinking by setting the rate to -1 (infinite).",
            Tags = ["appearance", "cursor", "blink"],
            RegistryKeys = [Desktop],
            ApplyOps = [RegOp.SetString(Desktop, "CursorBlinkRate", "-1")],
            RemoveOps = [RegOp.SetString(Desktop, "CursorBlinkRate", "530")],
            DetectOps = [RegOp.CheckString(Desktop, "CursorBlinkRate", "-1")],
        },
        // ── Tooltip ──────────────────────────────────────────────────────

        new TweakDef
        {
            Id = "winapp-tooltip-delay-fast",
            Label = "Fast Tooltip Delay (200ms)",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Reduces tooltip delay to 200ms (default 400ms) for faster hover info.",
            Tags = ["appearance", "tooltip", "delay"],
            RegistryKeys = [Mouse],
            ApplyOps = [RegOp.SetString(Mouse, "MouseHoverTime", "200")],
            RemoveOps = [RegOp.SetString(Mouse, "MouseHoverTime", "400")],
            DetectOps = [RegOp.CheckString(Mouse, "MouseHoverTime", "200")],
        },
        new TweakDef
        {
            Id = "winapp-tooltip-delay-instant",
            Label = "Instant Tooltip Delay (0ms)",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Eliminates tooltip delay for instant hover info display.",
            Tags = ["appearance", "tooltip", "delay", "instant"],
            RegistryKeys = [Mouse],
            ApplyOps = [RegOp.SetString(Mouse, "MouseHoverTime", "0")],
            RemoveOps = [RegOp.SetString(Mouse, "MouseHoverTime", "400")],
            DetectOps = [RegOp.CheckString(Mouse, "MouseHoverTime", "0")],
        },
        // ── Alt+Tab & Multitasking Appearance ────────────────────────────

        new TweakDef
        {
            Id = "winapp-alt-tab-classic",
            Label = "Classic Alt+Tab (No Thumbnails)",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Switches Alt+Tab to the classic icon-only style without window thumbnails.",
            Tags = ["appearance", "alt-tab", "classic", "multitasking"],
            RegistryKeys = [Explorer],
            ApplyOps = [RegOp.SetDword(Explorer, "AltTabSettings", 1)],
            RemoveOps = [RegOp.DeleteValue(Explorer, "AltTabSettings")],
            DetectOps = [RegOp.CheckDword(Explorer, "AltTabSettings", 1)],
        },
        new TweakDef
        {
            Id = "winapp-alt-tab-no-edge-tabs",
            Label = "Alt+Tab: Open Windows Only (No Edge Tabs)",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Edge browser tabs from appearing as separate items in Alt+Tab.",
            Tags = ["appearance", "alt-tab", "edge", "tabs"],
            RegistryKeys = [Explorer],
            ApplyOps = [RegOp.SetDword(Explorer, "MultiTaskingAltTabFilter", 3)],
            RemoveOps = [RegOp.DeleteValue(Explorer, "MultiTaskingAltTabFilter")],
            DetectOps = [RegOp.CheckDword(Explorer, "MultiTaskingAltTabFilter", 3)],
        },
        // ── Transparency & DWM Effects ───────────────────────────────────

        new TweakDef
        {
            Id = "winapp-enable-transparency",
            Label = "Enable Transparency Effects",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables transparency effects on Start, taskbar, and action centre.",
            Tags = ["appearance", "transparency", "visual"],
            RegistryKeys = [Themes],
            ApplyOps = [RegOp.SetDword(Themes, "EnableTransparency", 1)],
            RemoveOps = [RegOp.SetDword(Themes, "EnableTransparency", 0)],
            DetectOps = [RegOp.CheckDword(Themes, "EnableTransparency", 1)],
        },
        new TweakDef
        {
            Id = "winapp-disable-transparency",
            Label = "Disable Transparency Effects",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables all transparency effects for a solid look and slight performance gain.",
            Tags = ["appearance", "transparency", "performance"],
            RegistryKeys = [Themes],
            ApplyOps = [RegOp.SetDword(Themes, "EnableTransparency", 0)],
            RemoveOps = [RegOp.SetDword(Themes, "EnableTransparency", 1)],
            DetectOps = [RegOp.CheckDword(Themes, "EnableTransparency", 0)],
        },
        new TweakDef
        {
            Id = "winapp-dwm-enable-blur",
            Label = "Enable DWM Blur Behind",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables the blur-behind effect for DWM-managed windows (Aero Glass look).",
            Tags = ["appearance", "dwm", "blur", "aero"],
            RegistryKeys = [Dwm],
            ApplyOps = [RegOp.SetDword(Dwm, "EnableAeroPeek", 1)],
            RemoveOps = [RegOp.SetDword(Dwm, "EnableAeroPeek", 0)],
            DetectOps = [RegOp.CheckDword(Dwm, "EnableAeroPeek", 1)],
        },
        new TweakDef
        {
            Id = "winapp-dwm-disable-peek",
            Label = "Disable Desktop Peek (Aero Peek)",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents hovering over Show Desktop from making windows transparent.",
            Tags = ["appearance", "dwm", "peek", "desktop"],
            RegistryKeys = [Dwm],
            ApplyOps = [RegOp.SetDword(Dwm, "EnableAeroPeek", 0)],
            RemoveOps = [RegOp.SetDword(Dwm, "EnableAeroPeek", 1)],
            DetectOps = [RegOp.CheckDword(Dwm, "EnableAeroPeek", 0)],
        },
        new TweakDef
        {
            Id = "winapp-dwm-disable-flip3d",
            Label = "Disable Flip3D Effect",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the 3D flip window switching effect (legacy feature, saves GPU resources).",
            Tags = ["appearance", "dwm", "flip3d", "visual"],
            RegistryKeys = [Dwm],
            ApplyOps = [RegOp.SetDword(Dwm, "Composition", 0)],
            RemoveOps = [RegOp.SetDword(Dwm, "Composition", 1)],
            DetectOps = [RegOp.CheckDword(Dwm, "Composition", 0)],
        },
        new TweakDef
        {
            Id = "winapp-enable-round-corners",
            Label = "Enable Rounded Window Corners",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Ensures rounded corners on windows (default on Win11). Set UseWindowFrameStagingBuffer to 1.",
            Tags = ["appearance", "corners", "rounded", "win11"],
            RegistryKeys = [Dwm],
            ApplyOps = [RegOp.SetDword(Dwm, "UseWindowFrameStagingBuffer", 1)],
            RemoveOps = [RegOp.DeleteValue(Dwm, "UseWindowFrameStagingBuffer")],
            DetectOps = [RegOp.CheckDword(Dwm, "UseWindowFrameStagingBuffer", 1)],
        },
        // ── Cursor & Mouse Visual ────────────────────────────────────────

        new TweakDef
        {
            Id = "winapp-cursor-shadow",
            Label = "Enable Cursor Shadow",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Adds a shadow under the mouse cursor for better visibility on light backgrounds.",
            Tags = ["appearance", "cursor", "shadow"],
            RegistryKeys = [Cursors],
            ApplyOps = [RegOp.SetString(Cursors, "CursorShadow", "1")],
            RemoveOps = [RegOp.SetString(Cursors, "CursorShadow", "0")],
            DetectOps = [RegOp.CheckString(Cursors, "CursorShadow", "1")],
        },
        new TweakDef
        {
            Id = "winapp-disable-cursor-shadow",
            Label = "Disable Cursor Shadow",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes the shadow under the mouse cursor for a cleaner look.",
            Tags = ["appearance", "cursor", "shadow"],
            RegistryKeys = [Cursors],
            ApplyOps = [RegOp.SetString(Cursors, "CursorShadow", "0")],
            RemoveOps = [RegOp.SetString(Cursors, "CursorShadow", "1")],
            DetectOps = [RegOp.CheckString(Cursors, "CursorShadow", "0")],
        },
        new TweakDef
        {
            Id = "winapp-cursor-size-large",
            Label = "Large Mouse Cursor (48px)",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Increases the mouse cursor size to large (48px) for better visibility.",
            Tags = ["appearance", "cursor", "size", "large", "accessibility"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Accessibility"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Accessibility", "CursorSize", 3)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Accessibility", "CursorSize", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Accessibility", "CursorSize", 3)],
        },
        // ── Dark/Light Mode Per-Area ─────────────────────────────────────

        new TweakDef
        {
            Id = "winapp-dark-mode-apps",
            Label = "Dark Mode for Apps",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Forces dark mode for modern UWP/WinUI applications only.",
            Tags = ["appearance", "dark", "mode", "apps"],
            RegistryKeys = [Themes],
            ApplyOps = [RegOp.SetDword(Themes, "AppsUseLightTheme", 0)],
            RemoveOps = [RegOp.SetDword(Themes, "AppsUseLightTheme", 1)],
            DetectOps = [RegOp.CheckDword(Themes, "AppsUseLightTheme", 0)],
        },
        new TweakDef
        {
            Id = "winapp-light-mode-apps",
            Label = "Light Mode for Apps",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Forces light mode for modern UWP/WinUI applications only.",
            Tags = ["appearance", "light", "mode", "apps"],
            RegistryKeys = [Themes],
            ApplyOps = [RegOp.SetDword(Themes, "AppsUseLightTheme", 1)],
            RemoveOps = [RegOp.SetDword(Themes, "AppsUseLightTheme", 0)],
            DetectOps = [RegOp.CheckDword(Themes, "AppsUseLightTheme", 1)],
        },
        new TweakDef
        {
            Id = "winapp-dark-mode-system",
            Label = "Dark Mode for System UI",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Forces dark mode for system UI elements (Start, taskbar, action centre).",
            Tags = ["appearance", "dark", "mode", "system"],
            RegistryKeys = [Themes],
            ApplyOps = [RegOp.SetDword(Themes, "SystemUsesLightTheme", 0)],
            RemoveOps = [RegOp.SetDword(Themes, "SystemUsesLightTheme", 1)],
            DetectOps = [RegOp.CheckDword(Themes, "SystemUsesLightTheme", 0)],
        },
        new TweakDef
        {
            Id = "winapp-light-mode-system",
            Label = "Light Mode for System UI",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Forces light mode for system UI elements (Start, taskbar, action centre).",
            Tags = ["appearance", "light", "mode", "system"],
            RegistryKeys = [Themes],
            ApplyOps = [RegOp.SetDword(Themes, "SystemUsesLightTheme", 1)],
            RemoveOps = [RegOp.SetDword(Themes, "SystemUsesLightTheme", 0)],
            DetectOps = [RegOp.CheckDword(Themes, "SystemUsesLightTheme", 1)],
        },
        // ── Font & Text ──────────────────────────────────────────────────

        new TweakDef
        {
            Id = "winapp-menu-font-size-small",
            Label = "Small Menu Font (14px)",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets the system menu font height to 14 pixels for compact menus. Requires sign-out.",
            Tags = ["appearance", "font", "menu", "size", "small"],
            RegistryKeys = [Metrics],
            ApplyOps = [RegOp.SetString(Metrics, "MenuHeight", "-210")],
            RemoveOps = [RegOp.SetString(Metrics, "MenuHeight", "-285")],
            DetectOps = [RegOp.CheckString(Metrics, "MenuHeight", "-210")],
            SideEffects = "Requires sign-out to take effect.",
        },
        new TweakDef
        {
            Id = "winapp-small-caption-font",
            Label = "Small Caption Font for Toolbars",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Reduces the small caption font height used by floating toolbars and palettes. Requires sign-out.",
            Tags = ["appearance", "font", "caption", "toolbar"],
            RegistryKeys = [Metrics],
            ApplyOps = [RegOp.SetString(Metrics, "SmCaptionHeight", "-225")],
            RemoveOps = [RegOp.SetString(Metrics, "SmCaptionHeight", "-330")],
            DetectOps = [RegOp.CheckString(Metrics, "SmCaptionHeight", "-225")],
            SideEffects = "Requires sign-out to take effect.",
        },
        // ── Desktop & Visual Tweaks ──────────────────────────────────────

        new TweakDef
        {
            Id = "winapp-disable-desktop-icons",
            Label = "Hide All Desktop Icons",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Hides all desktop icons for a clean workspace look.",
            Tags = ["appearance", "desktop", "icons", "clean"],
            RegistryKeys = [Explorer],
            ApplyOps = [RegOp.SetDword(Explorer, "HideIcons", 1)],
            RemoveOps = [RegOp.SetDword(Explorer, "HideIcons", 0)],
            DetectOps = [RegOp.CheckDword(Explorer, "HideIcons", 1)],
        },
        new TweakDef
        {
            Id = "winapp-show-file-extensions",
            Label = "Show File Extensions on Desktop",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Shows file extensions for desktop icons, matching Explorer behaviour.",
            Tags = ["appearance", "desktop", "extensions", "files"],
            RegistryKeys = [Explorer],
            ApplyOps = [RegOp.SetDword(Explorer, "HideFileExt", 0)],
            RemoveOps = [RegOp.SetDword(Explorer, "HideFileExt", 1)],
            DetectOps = [RegOp.CheckDword(Explorer, "HideFileExt", 0)],
        },
        new TweakDef
        {
            Id = "winapp-disable-snap-assist-flyout",
            Label = "Disable Snap Assist Flyout",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents the snap layout flyout from appearing when hovering the maximise button.",
            Tags = ["appearance", "snap", "flyout", "maximize"],
            RegistryKeys = [Explorer],
            ApplyOps = [RegOp.SetDword(Explorer, "EnableSnapAssistFlyout", 0)],
            RemoveOps = [RegOp.SetDword(Explorer, "EnableSnapAssistFlyout", 1)],
            DetectOps = [RegOp.CheckDword(Explorer, "EnableSnapAssistFlyout", 0)],
        },
        new TweakDef
        {
            Id = "winapp-taskbar-top-align",
            Label = "Taskbar: Left-Align Icons",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Moves taskbar icons to the left instead of the default centred layout on Windows 11.",
            Tags = ["appearance", "taskbar", "alignment", "left"],
            RegistryKeys = [Explorer],
            ApplyOps = [RegOp.SetDword(Explorer, "TaskbarAl", 0)],
            RemoveOps = [RegOp.SetDword(Explorer, "TaskbarAl", 1)],
            DetectOps = [RegOp.CheckDword(Explorer, "TaskbarAl", 0)],
        },
        new TweakDef
        {
            Id = "winapp-drag-full-windows",
            Label = "Drag Full Windows (Not Outlines)",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Shows the full window content whilst dragging instead of a wire-frame outline.",
            Tags = ["appearance", "drag", "windows", "visual"],
            RegistryKeys = [Desktop],
            ApplyOps = [RegOp.SetString(Desktop, "DragFullWindows", "1")],
            RemoveOps = [RegOp.SetString(Desktop, "DragFullWindows", "0")],
            DetectOps = [RegOp.CheckString(Desktop, "DragFullWindows", "1")],
        },
        new TweakDef
        {
            Id = "winapp-drag-outline-only",
            Label = "Drag Window Outlines Only",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Shows only a wire-frame outline when dragging windows for lower CPU usage.",
            Tags = ["appearance", "drag", "outline", "performance"],
            RegistryKeys = [Desktop],
            ApplyOps = [RegOp.SetString(Desktop, "DragFullWindows", "0")],
            RemoveOps = [RegOp.SetString(Desktop, "DragFullWindows", "1")],
            DetectOps = [RegOp.CheckString(Desktop, "DragFullWindows", "0")],
        },
        // ── Wallpaper & Background ───────────────────────────────────────

        new TweakDef
        {
            Id = "winapp-wallpaper-quality-max",
            Label = "Maximum Wallpaper JPEG Quality",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets wallpaper JPEG compression to maximum quality (100%). Prevents blurry desktop backgrounds.",
            Tags = ["appearance", "wallpaper", "quality", "jpeg"],
            RegistryKeys = [Desktop],
            ApplyOps = [RegOp.SetDword(Desktop, "JPEGImportQuality", 100)],
            RemoveOps = [RegOp.DeleteValue(Desktop, "JPEGImportQuality")],
            DetectOps = [RegOp.CheckDword(Desktop, "JPEGImportQuality", 100)],
        },
        new TweakDef
        {
            Id = "winapp-solid-color-background",
            Label = "Use Solid Color Desktop Background",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Switches the desktop background to a solid colour (no wallpaper) for a minimal look.",
            Tags = ["appearance", "wallpaper", "solid", "minimal"],
            RegistryKeys = [Desktop],
            ApplyOps = [RegOp.SetString(Desktop, "Wallpaper", "")],
            RemoveOps = [RegOp.DeleteValue(Desktop, "Wallpaper")],
            DetectOps = [RegOp.CheckString(Desktop, "Wallpaper", "")],
        },
        new TweakDef
        {
            Id = "winapp-disable-wallpaper-slideshow",
            Label = "Disable Desktop Slideshow",
            Category = "Desktop Customization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents the desktop wallpaper from cycling through images at timed intervals.",
            Tags = ["appearance", "wallpaper", "slideshow"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "DesktopSlideShowEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "DesktopSlideShowEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "DesktopSlideShowEnabled", 0)],
        },
        // ── Focus & Activation ───────────────────────────────────────────

        new TweakDef
        {
            Id = "winapp-focus-follows-mouse",
            Label = "Focus Follows Mouse (X-Mouse)",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Activates windows simply by hovering the mouse, without clicking (X11-style focus).",
            Tags = ["appearance", "focus", "mouse", "x-mouse"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Mouse"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "ActiveWindowTracking", "1")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "ActiveWindowTracking", "0")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "ActiveWindowTracking", "1")],
        },
        new TweakDef
        {
            Id = "winapp-auto-raise-on-hover",
            Label = "Auto-Raise Window on Hover",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Automatically brings the hovered window to the front (requires Focus Follows Mouse).",
            Tags = ["appearance", "focus", "hover", "raise"],
            RegistryKeys = [Desktop],
            ApplyOps = [RegOp.SetDword(Desktop, "ActiveWndTrkTimeout", 200)],
            RemoveOps = [RegOp.DeleteValue(Desktop, "ActiveWndTrkTimeout")],
            DetectOps = [RegOp.CheckDword(Desktop, "ActiveWndTrkTimeout", 200)],
        },
        new TweakDef
        {
            Id = "winapp-foreground-lock-timeout",
            Label = "Reduce Foreground Lock Timeout",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Reduces the timeout that prevents apps from stealing focus to 0ms (instant focus switch).",
            Tags = ["appearance", "focus", "foreground", "lock"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ForegroundLockTimeout", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ForegroundLockTimeout", 200000)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ForegroundLockTimeout", 0)],
        },
        // ── Visual Effects Granular ──────────────────────────────────────

        new TweakDef
        {
            Id = "winapp-disable-smooth-scrolling",
            Label = "Disable Smooth Scrolling",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables smooth scrolling animations in listboxes and controls.",
            Tags = ["appearance", "scrolling", "smooth", "performance"],
            RegistryKeys = [Desktop],
            ApplyOps = [RegOp.SetDword(Desktop, "SmoothScroll", 0)],
            RemoveOps = [RegOp.SetDword(Desktop, "SmoothScroll", 1)],
            DetectOps = [RegOp.CheckDword(Desktop, "SmoothScroll", 0)],
        },
        new TweakDef
        {
            Id = "winapp-disable-font-smoothing",
            Label = "Disable Font Smoothing",
            Category = "Desktop Customization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables all font smoothing for a crisp pixel-perfect text look on high-DPI displays.",
            Tags = ["appearance", "font", "smoothing"],
            RegistryKeys = [Desktop],
            ApplyOps = [RegOp.SetString(Desktop, "FontSmoothing", "0")],
            RemoveOps = [RegOp.SetString(Desktop, "FontSmoothing", "2")],
            DetectOps = [RegOp.CheckString(Desktop, "FontSmoothing", "0")],
        },
    ];
}
