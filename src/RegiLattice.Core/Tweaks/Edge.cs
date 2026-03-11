namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Edge
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "edge-disable-collections",
            Label = "Disable Edge Collections",
            Category = "Edge",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Edge Collections feature used for organizing web content. Reduces UI clutter and memory usage. Default: Enabled. Recommended: Disabled if not used.",
            Tags = ["edge", "collections", "ux", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeCollectionsEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeCollectionsEnabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeCollectionsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-mini-menu",
            Label = "Disable Edge Mini Context Menu",
            Category = "Edge",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the mini context menu that appears on text selection in Edge. Removes the floating toolbar with search/copy/etc. Default: Enabled. Recommended: Disabled for cleaner UX.",
            Tags = ["edge", "mini-menu", "context-menu", "ux"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "MiniContextMenuEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "MiniContextMenuEnabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "MiniContextMenuEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-shopping-assistant",
            Label = "Disable Edge Shopping Assistant",
            Category = "Edge",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Edge shopping/coupon assistant that appears on retail sites. Default: enabled.",
            Tags = ["edge", "shopping", "coupons", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeShoppingAssistantEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeShoppingAssistantEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "EdgeShoppingAssistantEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-startup-boost",
            Label = "Disable Edge Startup Boost",
            Category = "Edge",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Edge Startup Boost that preloads Edge processes at Windows startup. Saves RAM. Default: enabled.",
            Tags = ["edge", "startup", "boost", "memory"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "StartupBoostEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "StartupBoostEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "StartupBoostEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-personalization",
            Label = "Disable Edge Personalization and Advertising",
            Category = "Edge",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables personalization and advertising features in Edge that track browsing behavior. Default: enabled.",
            Tags = ["edge", "personalization", "advertising", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "PersonalizationReportingEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "PersonalizationReportingEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "PersonalizationReportingEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edge-disable-browser-sign-in",
            Label = "Disable Automatic Browser Sign-In",
            Category = "Edge",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Edge from automatically signing into the browser using the Windows account. Default: auto-sign-in.",
            Tags = ["edge", "sign-in", "account", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "BrowserSignin", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "BrowserSignin")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "BrowserSignin", 0)],
        },
    ];
}
