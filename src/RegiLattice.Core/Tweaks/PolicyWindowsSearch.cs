namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// ─────────────────────────────────────────────────────────────────────────────
// Sprint 646 (1 existing) + Sprint 688 (+9 new) — PolicyWindowsSearch
// Windows Search Group Policy hardening — 10 tweaks total.

internal static class PolicyWindowsSearch
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        // ── Web / privacy ─────────────────────────────────────────────────────
        new TweakDef
        {
            Id = "wsepol-block-remote-query",
            Label = "Block Remote Cortana Query via Policy",
            Category = "Privacy — Sensor",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableRemoteQuery=1 under the Windows Search Group Policy path. "
                + "Prevents Cortana from querying remote services for information when invoked. "
                + "Queries are processed locally only, reducing data exfiltration risk.",
            Tags = ["search", "cortana", "remote", "policy", "security"],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Blocks Cortana remote queries; local processing only.",
            ApplyOps = [RegOp.SetDword(Key, "DisableRemoteQuery", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableRemoteQuery")],
            DetectOps = [RegOp.CheckDword(Key, "DisableRemoteQuery", 1)],
        },
        new TweakDef
        {
            Id = "wsepol-no-web-results",
            Label = "Prevent Web Results in Windows Search via Policy",
            Category = "Privacy — Sensor",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DoNotUseWebResults=1 in the Windows Search Group Policy. "
                + "Suppresses Bing and other web results from appearing in Start menu and "
                + "taskbar search; limits results to local content only.",
            Tags = ["search", "bing", "web", "policy", "privacy", "telemetry"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Removes web/Bing results from the Start/taskbar search box.",
            ApplyOps = [RegOp.SetDword(Key, "DoNotUseWebResults", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DoNotUseWebResults")],
            DetectOps = [RegOp.CheckDword(Key, "DoNotUseWebResults", 1)],
        },
        new TweakDef
        {
            Id = "wsepol-no-connected-search-web",
            Label = "Disable Web in Connected Windows Search via Policy",
            Category = "Privacy — Sensor",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets ConnectedSearchUseWeb=0 in the Windows Search Group Policy. "
                + "Prevents Windows Search from connecting to the web for auto-suggestions "
                + "and query expansion, keeping all search activity offline.",
            Tags = ["search", "web", "connected", "policy", "privacy"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Disables web connectivity for Windows Search queries.",
            ApplyOps = [RegOp.SetDword(Key, "ConnectedSearchUseWeb", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "ConnectedSearchUseWeb")],
            DetectOps = [RegOp.CheckDword(Key, "ConnectedSearchUseWeb", 0)],
        },
        new TweakDef
        {
            Id = "wsepol-safe-search-moderate",
            Label = "Set Connected Search Safe Filtering to Moderate via Policy",
            Category = "Privacy — Sensor",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets ConnectedSearchSafeSearch=1 (moderate) in the Windows Search Group Policy. "
                + "Applies safe-search filtering to connected Windows Search results, reducing "
                + "exposure to explicit content on shared or managed machines.",
            Tags = ["search", "safe", "policy", "safesearch", "content-filter"],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Enables moderate safe-search filtering for connected search results.",
            ApplyOps = [RegOp.SetDword(Key, "ConnectedSearchSafeSearch", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "ConnectedSearchSafeSearch")],
            DetectOps = [RegOp.CheckDword(Key, "ConnectedSearchSafeSearch", 1)],
        },
        // ── Indexing hardening ────────────────────────────────────────────────
        new TweakDef
        {
            Id = "wsepol-prevent-index-exchange",
            Label = "Prevent Indexing Uncached Exchange Folders via Policy",
            Category = "Indexing & Search",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets PreventIndexingUncachedExchangeFolders=1 in the Windows Search Group Policy. "
                + "Stops the Indexer from crawling Exchange or IMAP mail folders that are not "
                + "cached locally, preventing unnecessary network traffic and keeping remote "
                + "mail data out of the local search index.",
            Tags = ["indexing", "exchange", "policy", "email", "search"],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Limits indexer to locally cached mail; no remote Exchange crawl.",
            ApplyOps = [RegOp.SetDword(Key, "PreventIndexingUncachedExchangeFolders", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "PreventIndexingUncachedExchangeFolders")],
            DetectOps = [RegOp.CheckDword(Key, "PreventIndexingUncachedExchangeFolders", 1)],
        },
        new TweakDef
        {
            Id = "wsepol-prevent-index-offline",
            Label = "Prevent Indexing Offline Files Cache via Policy",
            Category = "Indexing & Search",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets PreventIndexingOfflineFiles=1 in the Windows Search Group Policy. "
                + "Prevents the Indexer from crawling files in the Offline Files cache (CSC "
                + "folder), reducing indexing overhead and preventing leakage of network share "
                + "metadata into the local search index.",
            Tags = ["indexing", "offline", "policy", "csc", "search"],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Stops Indexer from indexing offline files — reduces overhead.",
            ApplyOps = [RegOp.SetDword(Key, "PreventIndexingOfflineFiles", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "PreventIndexingOfflineFiles")],
            DetectOps = [RegOp.CheckDword(Key, "PreventIndexingOfflineFiles", 1)],
        },
        new TweakDef
        {
            Id = "wsepol-prevent-index-attachments",
            Label = "Prevent Indexing Email Attachment Content via Policy",
            Category = "Indexing & Search",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets PreventIndexingEmailAttachments=1 in the Windows Search Group Policy. "
                + "Prevents the Indexer from extracting and indexing email attachment content "
                + "(PDFs, Word docs, etc.), reducing resource usage and keeping attachment "
                + "content out of the local search index.",
            Tags = ["indexing", "email", "attachments", "policy", "search"],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Email attachments are not indexed — saves disk space and CPU.",
            ApplyOps = [RegOp.SetDword(Key, "PreventIndexingEmailAttachments", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "PreventIndexingEmailAttachments")],
            DetectOps = [RegOp.CheckDword(Key, "PreventIndexingEmailAttachments", 1)],
        },
        new TweakDef
        {
            Id = "wsepol-prevent-index-outlook",
            Label = "Prevent Indexing Microsoft Outlook via Policy",
            Category = "Indexing & Search",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets PreventIndexingMicrosoftOfficeOutlook=1 in the Windows Search Group Policy. "
                + "Stops the Indexer from indexing Outlook mail data, preventing email content "
                + "from appearing in the Windows Search results. Outlook's own built-in search "
                + "continues to work regardless.",
            Tags = ["indexing", "outlook", "policy", "email", "search", "office"],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Outlook mail not indexed by Windows Search; Outlook search still works.",
            ApplyOps = [RegOp.SetDword(Key, "PreventIndexingMicrosoftOfficeOutlook", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "PreventIndexingMicrosoftOfficeOutlook")],
            DetectOps = [RegOp.CheckDword(Key, "PreventIndexingMicrosoftOfficeOutlook", 1)],
        },
        new TweakDef
        {
            Id = "wsepol-allow-diacritics",
            Label = "Enable Diacritic-Sensitive Search via Policy",
            Category = "Indexing & Search",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets AllowUsingDiacritics=1 in the Windows Search Group Policy. "
                + "Enables diacritic-sensitive search matching so that accented characters "
                + "(e.g., é vs e, ñ vs n) are treated as distinct in search queries. "
                + "Recommended for multilingual environments.",
            Tags = ["indexing", "search", "language", "diacritics", "policy"],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Improves multilingual search accuracy by distinguishing diacritical marks.",
            ApplyOps = [RegOp.SetDword(Key, "AllowUsingDiacritics", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowUsingDiacritics")],
            DetectOps = [RegOp.CheckDword(Key, "AllowUsingDiacritics", 1)],
        },
        new TweakDef
        {
            Id = "wsepol-auto-lang-detect",
            Label = "Enable Automatic Language Detection in Search Indexer via Policy",
            Category = "Indexing & Search",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets AlwaysUseAutoLangDetection=1 in the Windows Search Group Policy. "
                + "Instructs the Indexer to automatically detect the language of each document "
                + "rather than relying solely on the system locale. Improves search relevance "
                + "in multilingual document collections.",
            Tags = ["indexing", "search", "language", "detection", "policy"],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Better indexing accuracy for mixed-language document libraries.",
            ApplyOps = [RegOp.SetDword(Key, "AlwaysUseAutoLangDetection", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AlwaysUseAutoLangDetection")],
            DetectOps = [RegOp.CheckDword(Key, "AlwaysUseAutoLangDetection", 1)],
        },
    ];
}
