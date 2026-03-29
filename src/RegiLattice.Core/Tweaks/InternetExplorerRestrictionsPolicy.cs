// RegiLattice.Core — Tweaks/InternetExplorerRestrictionsPolicy.cs
// Internet Explorer user interface restriction Group Policy controls — Sprint 429.
// Controls toolbar access, browser UI elements, context menus, and view
// options to harden IE and Edge IE Mode for managed enterprise endpoints.
// Category: "Internet Explorer Restrictions Policy" | Slug: ierest
// Registry path: HKLM\SOFTWARE\Policies\Microsoft\Internet Explorer\Restrictions

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class InternetExplorerRestrictionsPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Internet Explorer\Restrictions";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "ierest-disable-context-menu",
                Label = "Disable IE Right-Click Context Menu",
                Category = "Internet Explorer Restrictions Policy",
                Description =
                    "Sets NoBrowserContextMenu=1 to disable the right-click context menu in Internet Explorer and Edge IE Mode tabs. "
                    + "Prevents users from accessing context-menu options such as Save As, View Source, and Print from within "
                    + "the browser frame, reducing information exfiltration vectors.",
                Tags = ["ie", "context-menu", "restriction", "edge-ie-mode", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Removes right-click menu in IE/IE Mode; keyboard shortcuts for copy/paste remain functional.",
                ApplyOps = [RegOp.SetDword(Key, "NoBrowserContextMenu", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoBrowserContextMenu")],
                DetectOps = [RegOp.CheckDword(Key, "NoBrowserContextMenu", 1)],
            },
            new TweakDef
            {
                Id = "ierest-disable-browser-options",
                Label = "Disable IE Internet Options Dialog",
                Category = "Internet Explorer Restrictions Policy",
                Description =
                    "Sets NoBrowserOptions=1 to remove access to the Internet Options dialog from IE and Edge IE Mode. "
                    + "Prevents users from modifying proxy settings, security zone configurations, privacy controls, "
                    + "and cached data through the browser settings interface.",
                Tags = ["ie", "options", "settings", "restriction", "edge-ie-mode", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Blocks Internet Options access; zone and proxy settings can only be changed by an administrator.",
                ApplyOps = [RegOp.SetDword(Key, "NoBrowserOptions", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoBrowserOptions")],
                DetectOps = [RegOp.CheckDword(Key, "NoBrowserOptions", 1)],
            },
            new TweakDef
            {
                Id = "ierest-disable-view-source",
                Label = "Disable IE View Source",
                Category = "Internet Explorer Restrictions Policy",
                Description =
                    "Sets NoViewSource=1 to prevent users from viewing the HTML source code of web pages in IE and Edge IE Mode. "
                    + "Removing view-source access discourages extraction of embedded credentials, internal URLs, and application "
                    + "structure from intranet and web application pages rendered in IE Mode.",
                Tags = ["ie", "view-source", "security", "restriction", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Disables View Source; developers using IE Mode for legacy apps lose quick HTML inspection.",
                ApplyOps = [RegOp.SetDword(Key, "NoViewSource", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoViewSource")],
                DetectOps = [RegOp.CheckDword(Key, "NoViewSource", 1)],
            },
            new TweakDef
            {
                Id = "ierest-disable-favorites",
                Label = "Disable IE Favorites Menu",
                Category = "Internet Explorer Restrictions Policy",
                Description =
                    "Sets NoFavorites=1 to remove the Favorites menu and prevent users from adding or accessing "
                    + "bookmarked sites in Internet Explorer and Edge IE Mode. Favorites-based URL access creates "
                    + "persistent local references to sites that may bypass proxy policies if bookmarks are stale.",
                Tags = ["ie", "favorites", "bookmarks", "restriction", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Removes Favorites menu from IE and IE Mode; existing bookmarks are not deleted.",
                ApplyOps = [RegOp.SetDword(Key, "NoFavorites", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoFavorites")],
                DetectOps = [RegOp.CheckDword(Key, "NoFavorites", 1)],
            },
            new TweakDef
            {
                Id = "ierest-disable-select-download-dir",
                Label = "Prevent Changing IE Download Folder",
                Category = "Internet Explorer Restrictions Policy",
                Description =
                    "Sets NoSelectDownloadDir=1 to prevent users from changing the download destination folder in IE. "
                    + "Forces all file downloads to use the administrator-configured download directory, "
                    + "enabling consistent DLP monitoring of the download path.",
                Tags = ["ie", "download", "folder", "dlp", "restriction", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Locks IE download directory; all browser downloads go to the policy-specified folder.",
                ApplyOps = [RegOp.SetDword(Key, "NoSelectDownloadDir", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoSelectDownloadDir")],
                DetectOps = [RegOp.CheckDword(Key, "NoSelectDownloadDir", 1)],
            },
            new TweakDef
            {
                Id = "ierest-disable-find-files",
                Label = "Disable IE Find Files Command",
                Category = "Internet Explorer Restrictions Policy",
                Description =
                    "Sets NoFindFiles=1 to disable the Find > Files or Folders command within Internet Explorer. "
                    + "Prevents users from using the built-in file search capability that can expose the local filesystem "
                    + "from within the browser interface.",
                Tags = ["ie", "find", "files", "restriction", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Disables the Find Files menu entry in IE; file search via Explorer and other tools unaffected.",
                ApplyOps = [RegOp.SetDword(Key, "NoFindFiles", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoFindFiles")],
                DetectOps = [RegOp.CheckDword(Key, "NoFindFiles", 1)],
            },
            new TweakDef
            {
                Id = "ierest-disable-open-in-new-window",
                Label = "Prevent IE Links Opening in New Windows",
                Category = "Internet Explorer Restrictions Policy",
                Description =
                    "Sets NoOpenInNewWnd=1 to prevent hyperlinks and scripts in Internet Explorer from opening content "
                    + "in new browser windows. Stops script-driven window spawning used by pop-up ads and potentially "
                    + "malicious redirects rendered via IE Mode.",
                Tags = ["ie", "new-window", "pop-up", "restriction", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Prevents link-in-new-window; may break legacy IE Mode apps that use popup dialogs.",
                ApplyOps = [RegOp.SetDword(Key, "NoOpenInNewWnd", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoOpenInNewWnd")],
                DetectOps = [RegOp.CheckDword(Key, "NoOpenInNewWnd", 1)],
            },
            new TweakDef
            {
                Id = "ierest-disable-browser-toolbar",
                Label = "Remove IE Browser Toolbar",
                Category = "Internet Explorer Restrictions Policy",
                Description =
                    "Sets NoToolBar=1 to remove the toolbar from Internet Explorer and Edge IE Mode. "
                    + "Prevents access to toolbar controls, add-ons, and navigation shortcuts from the toolbar area. "
                    + "Reduces the visible browser surface area for kiosk-style IE Mode deployments.",
                Tags = ["ie", "toolbar", "restriction", "kiosk", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Removes toolbar from IE and IE Mode; address bar and navigation controls remain available.",
                ApplyOps = [RegOp.SetDword(Key, "NoToolBar", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoToolBar")],
                DetectOps = [RegOp.CheckDword(Key, "NoToolBar", 1)],
            },
            new TweakDef
            {
                Id = "ierest-disable-theater-mode",
                Label = "Disable IE Theater / Full-Screen Mode",
                Category = "Internet Explorer Restrictions Policy",
                Description =
                    "Sets NoTheaterMode=1 to disable the Theater Mode (full-screen F11 view) in Internet Explorer. "
                    + "Prevents users from entering full-screen presentation mode, which hides the taskbar and system indicators "
                    + "and can be exploited for phishing overlays that mimic OS dialogs.",
                Tags = ["ie", "theater-mode", "full-screen", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Disables F11 full-screen mode in IE; minor usability change for normal browser operation.",
                ApplyOps = [RegOp.SetDword(Key, "NoTheaterMode", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoTheaterMode")],
                DetectOps = [RegOp.CheckDword(Key, "NoTheaterMode", 1)],
            },
            new TweakDef
            {
                Id = "ierest-disable-close-browser",
                Label = "Disable IE Close Browser Button",
                Category = "Internet Explorer Restrictions Policy",
                Description =
                    "Sets NoBrowserClose=1 to prevent users from closing the Internet Explorer window via the X button or "
                    + "File > Close. Used in kiosk and locked-down browsing scenarios where IE is the only interface "
                    + "and the browser must remain open for the session.",
                Tags = ["ie", "close", "kiosk", "restriction", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 3,
                ImpactNote = "Prevents closing IE window; intended for kiosk deployments only — may frustrate users on normal endpoints.",
                ApplyOps = [RegOp.SetDword(Key, "NoBrowserClose", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoBrowserClose")],
                DetectOps = [RegOp.CheckDword(Key, "NoBrowserClose", 1)],
            },
        ];
}
