namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WindowsMailPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Mail";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "winmail-disable-manual-launch",
            Label = "Windows Mail Policy: Block Manual Launch of Windows Mail",
            Category = "Windows Mail Policy",
            Description = "Prevents users from manually launching the Windows Mail application. Enterprise environments that route email exclusively through corporate clients (Outlook, web) should block the inbox Windows Mail app to reduce shadow IT risk.",
            Tags = ["mail", "windows-mail", "launch", "block", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "ManualLaunchAllowed", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "ManualLaunchAllowed")],
            DetectOps = [RegOp.CheckDword(Key, "ManualLaunchAllowed", 0)],
        },
        new TweakDef
        {
            Id = "winmail-disable-mail-import",
            Label = "Windows Mail Policy: Disable Import of External Mail Accounts",
            Category = "Windows Mail Policy",
            Description = "Prevents Windows Mail from importing accounts, messages, or contacts from external mail clients. Disabling import reduces the risk of unauthorized data ingestion from non-corporate mail clients into the Windows Mail store.",
            Tags = ["mail", "windows-mail", "import", "accounts", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "TurnOffMailImport", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "TurnOffMailImport")],
            DetectOps = [RegOp.CheckDword(Key, "TurnOffMailImport", 1)],
        },
        new TweakDef
        {
            Id = "winmail-block-http-tracking-pixels",
            Label = "Windows Mail Policy: Block HTTP Remote Images (Anti-Tracking)",
            Category = "Windows Mail Policy",
            Description = "Prevents Windows Mail from automatically loading HTTP images embedded in email messages. Remote images (1x1 tracking pixels) are widely used by marketers and threat actors to confirm email addresses are active and track recipient location.",
            Tags = ["mail", "windows-mail", "tracking", "images", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "BlockHTTPImages", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockHTTPImages")],
            DetectOps = [RegOp.CheckDword(Key, "BlockHTTPImages", 1)],
        },
        new TweakDef
        {
            Id = "winmail-disable-featured-updates",
            Label = "Windows Mail Policy: Disable Featured Updates in Windows Mail",
            Category = "Windows Mail Policy",
            Description = "Turns off the featured/promotional updates displayed within the Windows Mail application. In enterprise deployments, UI promotional messages are distractions that may redirect users to unsanctioned services.",
            Tags = ["mail", "windows-mail", "updates", "promotional", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "TurnOffFeaturedUpdates", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "TurnOffFeaturedUpdates")],
            DetectOps = [RegOp.CheckDword(Key, "TurnOffFeaturedUpdates", 1)],
        },
        new TweakDef
        {
            Id = "winmail-disable-hotmail-contact-sync",
            Label = "Windows Mail Policy: Disable Hotmail/Live Contact Synchronisation",
            Category = "Windows Mail Policy",
            Description = "Prevents Windows Mail from synchronising contacts with Microsoft Hotmail or Live accounts. On managed devices, contact sync to personal Microsoft accounts creates data exfiltration risk for confidential address book entries.",
            Tags = ["mail", "windows-mail", "hotmail", "contacts", "sync", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "TurnOffHotmailContact", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "TurnOffHotmailContact")],
            DetectOps = [RegOp.CheckDword(Key, "TurnOffHotmailContact", 1)],
        },
        new TweakDef
        {
            Id = "winmail-force-plaintext-display",
            Label = "Windows Mail Policy: Force Plaintext Rendering for Email",
            Category = "Windows Mail Policy",
            Description = "Forces Windows Mail to render incoming messages as plain text only. HTML email is the primary delivery vector for phishing attacks (hidden links, CSS tricks, JavaScript payloads). Plain text rendering neutralises the entire class of HTML-based email threats.",
            Tags = ["mail", "windows-mail", "plaintext", "html", "phishing", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "ForceHTMLMailAsPlainText", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "ForceHTMLMailAsPlainText")],
            DetectOps = [RegOp.CheckDword(Key, "ForceHTMLMailAsPlainText", 1)],
        },
        new TweakDef
        {
            Id = "winmail-block-executable-attachments",
            Label = "Windows Mail Policy: Block Executable File Attachments",
            Category = "Windows Mail Policy",
            Description = "Prevents Windows Mail from delivering or presenting executable file attachments (EXE, COM, BAT, PS1, etc.) to users. Executable email attachments are the most common initial access vector in enterprise phishing campaigns.",
            Tags = ["mail", "windows-mail", "attachments", "executable", "block", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "BlockExecutableAttachments", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockExecutableAttachments")],
            DetectOps = [RegOp.CheckDword(Key, "BlockExecutableAttachments", 1)],
        },
        new TweakDef
        {
            Id = "winmail-disable-shopping-links",
            Label = "Windows Mail Policy: Disable Shopping Promotional Links",
            Category = "Windows Mail Policy",
            Description = "Disables shopping links and promotional offers embedded in Windows Mail. Enterprise mail clients should suppress commercial UI to prevent employee distraction and reduce the risk of clicking unsolicited purchase links.",
            Tags = ["mail", "windows-mail", "shopping", "promotional", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "TurnOffShopping", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "TurnOffShopping")],
            DetectOps = [RegOp.CheckDword(Key, "TurnOffShopping", 1)],
        },
        new TweakDef
        {
            Id = "winmail-disable-news-feed",
            Label = "Windows Mail Policy: Disable News Feed Integration",
            Category = "Windows Mail Policy",
            Description = "Disables the integrated news feed widget within Windows Mail. News feed integration increases background network calls and may display content from external third-party news aggregators, which is inappropriate for managed enterprise environments.",
            Tags = ["mail", "windows-mail", "news", "feed", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "TurnOffNewsFeed", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "TurnOffNewsFeed")],
            DetectOps = [RegOp.CheckDword(Key, "TurnOffNewsFeed", 1)],
        },
        new TweakDef
        {
            Id = "winmail-disable-calendar-integration",
            Label = "Windows Mail Policy: Disable Calendar Sync Integration",
            Category = "Windows Mail Policy",
            Description = "Prevents Windows Mail from synchronising calendar data with Microsoft consumer accounts or Exchange integrations not managed by the enterprise. Blocks calendar data from being stored in the Windows Mail local store outside MDM supervision.",
            Tags = ["mail", "windows-mail", "calendar", "sync", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableCalendarIntegration", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableCalendarIntegration")],
            DetectOps = [RegOp.CheckDword(Key, "DisableCalendarIntegration", 1)],
        },
    ];
}
