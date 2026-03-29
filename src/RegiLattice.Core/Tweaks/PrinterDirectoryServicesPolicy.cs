// RegiLattice.Core — Tweaks/PrinterDirectoryServicesPolicy.cs
// Printer Active Directory Directory Services publishing policy — Sprint 430.
// Controls how printers are published to, pruned from, and discovered via
// Active Directory Directory Services via Group Policy registry paths.
// Category: "Printer Directory Services Policy" | Slug: pdssp
// Registry paths: HKLM\SOFTWARE\Policies\Microsoft\Windows NT\Printers\DS

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PrinterDirectoryServicesPolicy
{
    private const string DsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\DS";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "pdssp-disable-printer-publishing",
                Label = "Disable Automatic Printer Publishing to AD",
                Category = "Printer Directory Services Policy",
                Description =
                    "Sets PublishPrinters=0 to prevent Windows from automatically publishing printers to Active Directory "
                    + "Directory Services when they are added to the system. Unpublished printers are not discoverable via "
                    + "AD queries, reducing AD pollution from transient or personal printers on endpoints.",
                Tags = ["printing", "active-directory", "publishing", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Stops auto-publishing printers to AD; manually published printers and shared printers unaffected.",
                ApplyOps = [RegOp.SetDword(DsKey, "PublishPrinters", 0)],
                RemoveOps = [RegOp.DeleteValue(DsKey, "PublishPrinters")],
                DetectOps = [RegOp.CheckDword(DsKey, "PublishPrinters", 0)],
            },
            new TweakDef
            {
                Id = "pdssp-disable-printer-pruning",
                Label = "Disable Printer Object Pruning from AD",
                Category = "Printer Directory Services Policy",
                Description =
                    "Sets PruningRetries=0 to disable the printer pruning mechanism that removes stale printer objects "
                    + "from Active Directory when the print server is unreachable. Prevents pruning in environments where "
                    + "print servers go offline temporarily (maintenance, DR failover) but should remain resolvable via AD.",
                Tags = ["printing", "active-directory", "pruning", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Disables AD pruning; stale printer objects persist in AD if print servers are decommissioned.",
                ApplyOps = [RegOp.SetDword(DsKey, "PruningRetries", 0)],
                RemoveOps = [RegOp.DeleteValue(DsKey, "PruningRetries")],
                DetectOps = [RegOp.CheckDword(DsKey, "PruningRetries", 0)],
            },
            new TweakDef
            {
                Id = "pdssp-set-pruning-interval",
                Label = "Set Printer Pruning Check Interval",
                Category = "Printer Directory Services Policy",
                Description =
                    "Sets PruningInterval=480 to check every 8 hours (480 minutes) whether printer objects in Active Directory "
                    + "should be pruned. The default check interval is every 8 hours; a longer interval reduces AD queries "
                    + "from the Directory Service pruning subsystem on domain controllers.",
                Tags = ["printing", "active-directory", "pruning", "interval", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Sets pruning poll interval to 480 min; reduces printer-related AD query load.",
                ApplyOps = [RegOp.SetDword(DsKey, "PruningInterval", 480)],
                RemoveOps = [RegOp.DeleteValue(DsKey, "PruningInterval")],
                DetectOps = [RegOp.CheckDword(DsKey, "PruningInterval", 480)],
            },
            new TweakDef
            {
                Id = "pdssp-set-pruning-priority",
                Label = "Set Printer Pruning Thread Priority",
                Category = "Printer Directory Services Policy",
                Description =
                    "Sets PruningPriority=0 to run the printer pruning thread at low priority. "
                    + "Reduces CPU contention from the background AD pruning process on heavily loaded print servers "
                    + "and domain controllers that share hardware resources.",
                Tags = ["printing", "active-directory", "pruning", "performance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Low-priority pruning thread; negligible performance impact on print servers.",
                ApplyOps = [RegOp.SetDword(DsKey, "PruningPriority", 0)],
                RemoveOps = [RegOp.DeleteValue(DsKey, "PruningPriority")],
                DetectOps = [RegOp.CheckDword(DsKey, "PruningPriority", 0)],
            },
            new TweakDef
            {
                Id = "pdssp-log-pruning-events",
                Label = "Enable Printer Pruning Event Logging",
                Category = "Printer Directory Services Policy",
                Description =
                    "Sets PruningRetryLog=1 to record printer pruning retry and failure events to the Windows Application event log. "
                    + "Provides audit visibility into AD printer object lifecycle events for SIEM ingestion and printer infrastructure monitoring.",
                Tags = ["printing", "active-directory", "pruning", "logging", "audit", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Logs pruning events to Application log; minor increase in event log volume.",
                ApplyOps = [RegOp.SetDword(DsKey, "PruningRetryLog", 1)],
                RemoveOps = [RegOp.DeleteValue(DsKey, "PruningRetryLog")],
                DetectOps = [RegOp.CheckDword(DsKey, "PruningRetryLog", 1)],
            },
            new TweakDef
            {
                Id = "pdssp-disable-non-published-printer-access",
                Label = "Block Access to Non-Published AD Printers",
                Category = "Printer Directory Services Policy",
                Description =
                    "Sets NonPublishedPrinters=0 to prevent users from connecting to network printers that are not published "
                    + "in Active Directory. Ensures all printer installations go through the AD Directory Services vetting process "
                    + "and prevents rogue or personal printers from being added via direct UNC paths.",
                Tags = ["printing", "active-directory", "access-control", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Blocks non-AD-published printer connections; only AD-listed printers can be installed.",
                ApplyOps = [RegOp.SetDword(DsKey, "NonPublishedPrinters", 0)],
                RemoveOps = [RegOp.DeleteValue(DsKey, "NonPublishedPrinters")],
                DetectOps = [RegOp.CheckDword(DsKey, "NonPublishedPrinters", 0)],
            },
            new TweakDef
            {
                Id = "pdssp-disable-ipp-web-printing",
                Label = "Disable IPP Web Printing via AD",
                Category = "Printer Directory Services Policy",
                Description =
                    "Sets DisableWebPrinting=1 to prevent users from installing printers via Internet Printing Protocol (IPP) "
                    + "URLs discovered through Active Directory. Web-based printer installation bypasses network printer "
                    + "deployment controls and may allow connection to external or untrusted print services.",
                Tags = ["printing", "ipp", "web-printing", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Blocks IPP web printer discovery via AD; direct \\server\\printer UNC paths still work.",
                ApplyOps = [RegOp.SetDword(DsKey, "DisableWebPrinting", 1)],
                RemoveOps = [RegOp.DeleteValue(DsKey, "DisableWebPrinting")],
                DetectOps = [RegOp.CheckDword(DsKey, "DisableWebPrinting", 1)],
            },
            new TweakDef
            {
                Id = "pdssp-set-server-thread-count",
                Label = "Limit Printer DS Server Thread Count",
                Category = "Printer Directory Services Policy",
                Description =
                    "Sets ServerThread=2 to limit the number of concurrent threads used by the spooler for Active Directory "
                    + "printer publishing operations. Reducing thread count lowers CPU usage on print servers with many shared "
                    + "printers during AD bulk-publish events after policy refresh.",
                Tags = ["printing", "active-directory", "performance", "threads", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Limits AD spooler threads to 2; larger environments may need higher values for timely publishing.",
                ApplyOps = [RegOp.SetDword(DsKey, "ServerThread", 2)],
                RemoveOps = [RegOp.DeleteValue(DsKey, "ServerThread")],
                DetectOps = [RegOp.CheckDword(DsKey, "ServerThread", 2)],
            },
            new TweakDef
            {
                Id = "pdssp-enforce-pre-publish-printers",
                Label = "Enforce Pre-Publication of Printers to AD",
                Category = "Printer Directory Services Policy",
                Description =
                    "Sets PrePublishPrinters=1 to require printers to be pre-published to Active Directory before they "
                    + "become available to clients. Pre-publishing ensures printer metadata is available for directory browsing "
                    + "before the first client connection attempt, reducing discovery latency for distributed print deployments.",
                Tags = ["printing", "active-directory", "publishing", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Requires pre-publishing; AD objects created before printers accept connections.",
                ApplyOps = [RegOp.SetDword(DsKey, "PrePublishPrinters", 1)],
                RemoveOps = [RegOp.DeleteValue(DsKey, "PrePublishPrinters")],
                DetectOps = [RegOp.CheckDword(DsKey, "PrePublishPrinters", 1)],
            },
            new TweakDef
            {
                Id = "pdssp-set-max-pruning-retries",
                Label = "Set Maximum Printer Pruning Retry Count",
                Category = "Printer Directory Services Policy",
                Description =
                    "Sets PruningRetries=2 to limit the number of times the pruning mechanism retries an unreachable "
                    + "print server before removing its printer AD objects. A lower retry count speeds up cleanup of "
                    + "permanently decommissioned print servers while reducing spurious pruning from temporary outages.",
                Tags = ["printing", "active-directory", "pruning", "retry", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 4,
                ImpactNote = "Limits pruning retries to 2; printers may get pruned sooner after a short server outage.",
                ApplyOps = [RegOp.SetDword(DsKey, "PruningRetries", 2)],
                RemoveOps = [RegOp.DeleteValue(DsKey, "PruningRetries")],
                DetectOps = [RegOp.CheckDword(DsKey, "PruningRetries", 2)],
            },
        ];
}
