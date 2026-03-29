// RegiLattice.Core — Tweaks/EdgeSleepingTabsPolicy.cs
// Edge Sleeping Tabs, startup boost, memory optimization, and background processing controls — Sprint 454.
// Category: "Edge Sleeping Tabs Policy" | Slug: edgsleep
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Edge

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class EdgeSleepingTabsPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "edgsleep-enable-sleeping-tabs",
                Label = "Enable Sleeping Tabs in Edge",
                Category = "Edge Sleeping Tabs Policy",
                Description =
                    "Enables the Sleeping Tabs feature in Microsoft Edge, which puts inactive tabs to sleep after a configurable timeout to reduce memory and CPU usage.",
                Tags = ["edge", "sleeping-tabs", "performance", "memory", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Inactive tabs suspended; significant memory savings on machines with many open tabs.",
                ApplyOps = [RegOp.SetDword(Key, "SleepingTabsEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "SleepingTabsEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "SleepingTabsEnabled", 1)],
            },
            new TweakDef
            {
                Id = "edgsleep-set-timeout-300",
                Label = "Set Sleeping Tabs Timeout to 5 Minutes",
                Category = "Edge Sleeping Tabs Policy",
                Description =
                    "Sets the Sleeping Tabs inactivity timeout to 300 seconds (5 minutes), after which idle tabs are suspended to reclaim memory.",
                Tags = ["edge", "sleeping-tabs", "timeout", "performance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Tabs inactive for 5+ minutes suspended; shorter timeout = more aggressive memory reclaim.",
                ApplyOps = [RegOp.SetDword(Key, "SleepingTabsTimeout", 300)],
                RemoveOps = [RegOp.DeleteValue(Key, "SleepingTabsTimeout")],
                DetectOps = [RegOp.CheckDword(Key, "SleepingTabsTimeout", 300)],
            },
            new TweakDef
            {
                Id = "edgsleep-disable-startup-boost",
                Label = "Disable Edge Startup Boost",
                Category = "Edge Sleeping Tabs Policy",
                Description =
                    "Disables Edge Startup Boost which pre-launches Edge browser processes at Windows startup to improve launch speed at the cost of persistent background memory consumption.",
                Tags = ["edge", "startup-boost", "background", "performance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Edge no longer pre-runs in background; slightly slower first launch, reduced idle RAM.",
                ApplyOps = [RegOp.SetDword(Key, "StartupBoostEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "StartupBoostEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "StartupBoostEnabled", 0)],
            },
            new TweakDef
            {
                Id = "edgsleep-disable-background-run",
                Label = "Disable Edge Background Running After All Windows Closed",
                Category = "Edge Sleeping Tabs Policy",
                Description =
                    "Prevents Edge from running background processes after all browser windows are closed, fully releasing resources when the user is done browsing.",
                Tags = ["edge", "background", "performance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Edge processes terminate on last window close; push notifications and extensions stop when closed.",
                ApplyOps = [RegOp.SetDword(Key, "BackgroundModeEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "BackgroundModeEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "BackgroundModeEnabled", 0)],
            },
            new TweakDef
            {
                Id = "edgsleep-enable-efficiency-mode",
                Label = "Enable Edge Efficiency Mode",
                Category = "Edge Sleeping Tabs Policy",
                Description =
                    "Enables Edge Efficiency Mode which reduces CPU usage when running on battery or when device resources are constrained, improving battery life on laptops.",
                Tags = ["edge", "efficiency", "battery", "performance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Edge scales back CPU when resources are low; better battery life at slight performance cost.",
                ApplyOps = [RegOp.SetDword(Key, "EfficiencyModeEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EfficiencyModeEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "EfficiencyModeEnabled", 1)],
            },
            new TweakDef
            {
                Id = "edgsleep-disable-tab-preloading",
                Label = "Disable Edge New Tab Page Preloading",
                Category = "Edge Sleeping Tabs Policy",
                Description =
                    "Disables new tab page preloading in Edge which pre-fetches the new tab page in the background to reduce perceived open time at the cost of memory and network usage.",
                Tags = ["edge", "preloading", "new-tab", "performance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "New tab page not pre-loaded; slightly slower new tab but lower background resource use.",
                ApplyOps = [RegOp.SetDword(Key, "NewTabPagePrerenderEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "NewTabPagePrerenderEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "NewTabPagePrerenderEnabled", 0)],
            },
            new TweakDef
            {
                Id = "edgsleep-disable-speculative-prerendering",
                Label = "Disable Speculative Prerendering in Edge",
                Category = "Edge Sleeping Tabs Policy",
                Description =
                    "Disables speculative prerendering of links in Edge, which pre-loads pages you might navigate to. Reduces network and memory usage at the cost of navigation latency.",
                Tags = ["edge", "prerender", "privacy", "performance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Pages not pre-fetched speculatively; slightly slower navigation, less background network traffic.",
                ApplyOps = [RegOp.SetDword(Key, "NetworkPredictionOptions", 2)],
                RemoveOps = [RegOp.DeleteValue(Key, "NetworkPredictionOptions")],
                DetectOps = [RegOp.CheckDword(Key, "NetworkPredictionOptions", 2)],
            },
            new TweakDef
            {
                Id = "edgsleep-disable-tab-thumbnail-capture",
                Label = "Disable Tab Thumbnail Capture for Inactive Tabs",
                Category = "Edge Sleeping Tabs Policy",
                Description =
                    "Disables background thumbnail capture for inactive tabs which wakes sleeping tabs unnecessarily to generate preview images for the tab overview.",
                Tags = ["edge", "thumbnail", "sleeping-tabs", "performance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Sleeping tabs not woken for thumbnail capture; memory savings preserved.",
                ApplyOps = [RegOp.SetDword(Key, "TabPreviewEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "TabPreviewEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "TabPreviewEnabled", 0)],
            },
            new TweakDef
            {
                Id = "edgsleep-set-memory-saver-threshold",
                Label = "Enable Memory Saver Mode in Edge",
                Category = "Edge Sleeping Tabs Policy",
                Description =
                    "Enables Edge Memory Saver mode which aggressively frees memory from background tabs when system memory is constrained.",
                Tags = ["edge", "memory-saver", "performance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Background tabs freed from memory under pressure; page reloads needed when switching to freed tabs.",
                ApplyOps = [RegOp.SetDword(Key, "MemorySaverEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "MemorySaverEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "MemorySaverEnabled", 1)],
            },
            new TweakDef
            {
                Id = "edgsleep-disable-reader-mode-preload",
                Label = "Disable Reader Mode Preloading in Edge",
                Category = "Edge Sleeping Tabs Policy",
                Description =
                    "Disables automatic Reader Mode preloading that parses every article page in the background to prepare a distraction-free view, consuming CPU and memory unnecessarily.",
                Tags = ["edge", "reader-mode", "preload", "performance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Reader Mode not pre-parsed in background; manual Reader Mode activation still works.",
                ApplyOps = [RegOp.SetDword(Key, "ReaderModeEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "ReaderModeEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "ReaderModeEnabled", 0)],
            },
        ];
}
