namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Chrome
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "chrome-disable-renderer-code-integrity",
            Label = "Disable Chrome Renderer Code Integrity",
            Category = "Chrome",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables renderer code integrity checks in Chrome. Fixes compatibility issues with certain security software. Default: enabled.",
            Tags = ["chrome", "renderer", "code-integrity", "compatibility"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "RendererCodeIntegrityEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "RendererCodeIntegrityEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "RendererCodeIntegrityEnabled", 0)],
        },
        new TweakDef
        {
            Id = "chrome-disable-preloading",
            Label = "Disable Chrome Page Preloading",
            Category = "Chrome",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Chrome from preloading pages it predicts you might visit. Saves bandwidth. Default: enabled.",
            Tags = ["chrome", "preloading", "bandwidth", "prediction"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "NetworkPredictionOptions", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "NetworkPredictionOptions")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "NetworkPredictionOptions", 2)],
        },
        new TweakDef
        {
            Id = "chrome-disable-autofill-addresses",
            Label = "Disable Chrome Address Autofill",
            Category = "Chrome",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Chrome's autofill suggestions for addresses. Prevents address data storage. Default: enabled.",
            Tags = ["chrome", "autofill", "addresses", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "AutofillAddressEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "AutofillAddressEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "AutofillAddressEnabled", 0)],
        },
        new TweakDef
        {
            Id = "chrome-disable-search-suggestions",
            Label = "Disable Chrome Search Suggestions",
            Category = "Chrome",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables search and URL suggestions in the Chrome address bar. Prevents keystrokes from being sent to Google. Default: enabled.",
            Tags = ["chrome", "search", "suggestions", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "SearchSuggestEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "SearchSuggestEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "SearchSuggestEnabled", 0)],
        },
        new TweakDef
        {
            Id = "chrome-disable-background-mode",
            Label = "Disable Chrome Background Mode",
            Category = "Chrome",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Chrome from running in background after all windows are closed. Frees system resources. Default: enabled.",
            Tags = ["chrome", "background", "mode", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "BackgroundModeEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "BackgroundModeEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "BackgroundModeEnabled", 0)],
        },
    ];
}
