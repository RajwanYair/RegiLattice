// RegiLattice.Core — Tweaks/PrintJobManagementPolicy.cs
// Print Job Management Policy — Sprint 564.
// Configures Group Policy for print job lifecycle management: print queue
// priority, job scheduling, automatic purge on print server restart,
// separator page enforcement, and print job log size limits.
// Category: "Print Job Management Policy" | Slug: prtjob
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows NT\Printers\JobManagement

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PrintJobManagementPolicy
{
    private const string JobKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\JobManagement";

    private const string PrtKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "prtjob-purge-jobs-on-restart",
                Label = "Print Job Management: Purge All Print Jobs on Spooler Restart",
                Category = "Print Job Management Policy",
                Description =
                    "Sets PurgeJobsOnRestart=1 in JobManagement policy. Clears all pending print jobs from all print queues when the Print Spooler service restarts. By default, the spooler preserves queued jobs across restarts, which can cause problems when a restarted spooler encounters corrupted spool files (EMF or RAW) from a failed previous session — leading to an infinite loop where the spooler starts, crashes processing a bad job, and restarts. Purging on restart ensures the spooler always starts with a clean queue. Lost jobs must be resubmitted by users.",
                Tags = ["print-job", "spooler-restart", "queue-purge", "stability", "recovery"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Pending print jobs are lost when the spooler restarts (service restart, machine reboot). Users must resubmit their print jobs. Prevents spooler crash loops caused by corrupted spool files. Useful on print servers with history of spooler instability.",
                ApplyOps = [RegOp.SetDword(JobKey, "PurgeJobsOnRestart", 1)],
                RemoveOps = [RegOp.DeleteValue(JobKey, "PurgeJobsOnRestart")],
                DetectOps = [RegOp.CheckDword(JobKey, "PurgeJobsOnRestart", 1)],
            },
            new TweakDef
            {
                Id = "prtjob-set-max-spool-file-size-1gb",
                Label = "Print Job Management: Set Maximum Spool File Size to 1 GB",
                Category = "Print Job Management Policy",
                Description =
                    "Sets MaxSpoolFileSize=1073741824 in JobManagement policy (1 GB in bytes). Sets the maximum allowed size for individual print spool files. Without a spool file size limit, a single print job (e.g., a 10,000-page CAD print run or a large PDF) can generate a spool file that consumes all available disk space on the print server, starving all other users' jobs. 1 GB is sufficient for most large-format print jobs while protecting against runaway spool generation. Jobs exceeding the limit are rejected with a 'Spool file too large' error.",
                Tags = ["print-job", "spool-file", "disk-space", "limit", "print-server"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Print jobs exceeding 1 GB spool file size are rejected. Very large print jobs (high-resolution CAD, book-length PDFs) may need to be split into smaller jobs. Protects print server disk from runaway spool consumption.",
                ApplyOps = [RegOp.SetDword(JobKey, "MaxSpoolFileSize", 1073741824)],
                RemoveOps = [RegOp.DeleteValue(JobKey, "MaxSpoolFileSize")],
                DetectOps = [RegOp.CheckDword(JobKey, "MaxSpoolFileSize", 1073741824)],
            },
            new TweakDef
            {
                Id = "prtjob-enable-separator-page",
                Label = "Print Job Management: Enable Separator Page Between Print Jobs",
                Category = "Print Job Management Policy",
                Description =
                    "Sets UseSeparatorPage=1 in JobManagement policy. Enables job separator pages (banner pages) between print jobs. A separator page is a printed page inserted before each job containing: user name, date, time, and job ID. In shared printer environments, separator pages allow users to find their document among others' output in the printer tray output bin. Without separator pages, documents from multiple users in a busy shared printer pile together, causing users to accidentally take others' confidential documents — a physical information disclosure risk.",
                Tags = ["print-job", "separator-page", "banner-page", "physical-security", "shared-printer"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "A separator page is printed before each job. One additional page of toner consumed per job. Users can identify their documents in the output tray. Separator page format is configured per-printer (PCL, PostScript, Windows default).",
                ApplyOps = [RegOp.SetDword(JobKey, "UseSeparatorPage", 1)],
                RemoveOps = [RegOp.DeleteValue(JobKey, "UseSeparatorPage")],
                DetectOps = [RegOp.CheckDword(JobKey, "UseSeparatorPage", 1)],
            },
            new TweakDef
            {
                Id = "prtjob-set-job-expiry-8hours",
                Label = "Print Job Management: Expire Unprinted Jobs After 8 Hours",
                Category = "Print Job Management Policy",
                Description =
                    "Sets JobExpiryHours=8 in JobManagement policy. Automatically removes print jobs that have been queued but not processed (printed) within 8 hours. Jobs can accumulate in a queue when a printer is taken offline, goes into an error state, or is deliberately paused. Without expiry, a queue can accumulate hundreds of stale jobs — some of which may contain sensitive documents submitted by users who no longer need them. 8 hours aligns with a standard business day — a job submitted in the morning and not printed by end of day is auto-purged.",
                Tags = ["print-job", "expiry", "queue-management", "security", "cleanup"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Print jobs not printed within 8 hours are automatically removed. Users must resubmit jobs if the printer was unavailable for more than 8 hours. Prevents accumulation of stale documents in print queues.",
                ApplyOps = [RegOp.SetDword(JobKey, "JobExpiryHours", 8)],
                RemoveOps = [RegOp.DeleteValue(JobKey, "JobExpiryHours")],
                DetectOps = [RegOp.CheckDword(JobKey, "JobExpiryHours", 8)],
            },
            new TweakDef
            {
                Id = "prtjob-disable-interactive-print-sharing",
                Label = "Print Job Management: Disable Interactive Console Print Sharing",
                Category = "Print Job Management Policy",
                Description =
                    "Sets DisableInteractivePrinterSharing=1 in Printers policy. Prevents users from interactively sharing printers through the Windows Printer Properties dialog. Without this restriction, any local user can share their local printer to the network — creating unmanaged, unmonitored print shares that bypass central print server controls. Printer sharing should only be managed through Group Policy printer deployment or by administrators. Unmanaged printer shares can also have misconfigured permissions, allowing unauthenticated network print access.",
                Tags = ["print-job", "printer-sharing", "management", "control", "network"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "The 'Share this printer' checkbox in Printer Properties is disabled. Users cannot share their local printers to the network. Print sharing is only possible by administrators via Print Management or Group Policy.",
                ApplyOps = [RegOp.SetDword(PrtKey, "DisableInteractivePrinterSharing", 1)],
                RemoveOps = [RegOp.DeleteValue(PrtKey, "DisableInteractivePrinterSharing")],
                DetectOps = [RegOp.CheckDword(PrtKey, "DisableInteractivePrinterSharing", 1)],
            },
            new TweakDef
            {
                Id = "prtjob-enforce-default-queue-priority",
                Label = "Print Job Management: Enforce Default Queue Priority Level (49)",
                Category = "Print Job Management Policy",
                Description =
                    "Sets DefaultPriority=49 in JobManagement policy. Sets the default print job priority to 49 (scale of 1-99, where 99 is highest). When a user does not specify a priority or when they have priority escalation rights, print jobs default to priority 49. This ensures administrators can designate executive or time-critical queues with priority 50+ that will always preempt standard user jobs. Without a defined default, systems may inherit OS defaults that vary between Windows versions, making priority management unpredictable.",
                Tags = ["print-job", "priority", "queue-management", "fairness", "scheduling"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Standard user print jobs use priority 49. Priority 50-99 reserved for administrator-managed high-priority queues. No change in observed behaviour for most users — priority ordering is internal to the print queue scheduler.",
                ApplyOps = [RegOp.SetDword(JobKey, "DefaultPriority", 49)],
                RemoveOps = [RegOp.DeleteValue(JobKey, "DefaultPriority")],
                DetectOps = [RegOp.CheckDword(JobKey, "DefaultPriority", 49)],
            },
            new TweakDef
            {
                Id = "prtjob-set-spool-directory-to-secured",
                Label = "Print Job Management: Set Secure Spool Directory ACL Enforcement",
                Category = "Print Job Management Policy",
                Description =
                    "Sets SecureSpoolDirectory=1 in JobManagement policy. Enables ACL enforcement on the print spool directory (%SystemRoot%\\System32\\spool\\PRINTERS). By default this directory has permissive ACLs that allow any authenticated user to read or delete spool files. Spool files contain the raw or EMF rendering of documents being printed — reading them is equivalent to reading the document. With SecureSpoolDirectory enabled, only the SYSTEM account and print administrators can read spool files. Standard users cannot access other users' spool files.",
                Tags = ["print-job", "spool-directory", "acl", "file-security", "information-disclosure"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "The print spool directory is protected with restrictive ACLs. Standard users cannot read or delete other users' spool files. Third-party print monitoring software that reads spool files directly may require the SYSTEM or print administrator context.",
                ApplyOps = [RegOp.SetDword(JobKey, "SecureSpoolDirectory", 1)],
                RemoveOps = [RegOp.DeleteValue(JobKey, "SecureSpoolDirectory")],
                DetectOps = [RegOp.CheckDword(JobKey, "SecureSpoolDirectory", 1)],
            },
            new TweakDef
            {
                Id = "prtjob-disable-printer-status-popup",
                Label = "Print Job Management: Disable Print Status Notification Popups",
                Category = "Print Job Management Policy",
                Description =
                    "Sets DisablePrinterstatusNotifications=1 in JobManagement policy. Prevents the print status notification system tray balloon and popup messages from appearing when a print job completes successfully. In enterprise environments with high print volumes, completed print job notifications are a source of notification fatigue — users who print dozens of documents per day receive an equal number of transient notifications that they learn to dismiss immediately. Disabling successful-completion notifications reduces noise; error notifications (failure, out of paper) are separately configurable and should remain enabled.",
                Tags = ["print-job", "notifications", "user-experience", "task-bar", "status"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Print job completion notifications are suppressed in the system tray. Users are not notified when their document successfully prints. Error notifications (print failure, printer offline) can be separately enabled. Reduces notification clutter in high-volume printing environments.",
                ApplyOps = [RegOp.SetDword(JobKey, "DisablePrinterstatusNotifications", 1)],
                RemoveOps = [RegOp.DeleteValue(JobKey, "DisablePrinterstatusNotifications")],
                DetectOps = [RegOp.CheckDword(JobKey, "DisablePrinterstatusNotifications", 1)],
            },
            new TweakDef
            {
                Id = "prtjob-enable-emf-spool-format",
                Label = "Print Job Management: Use Enhanced Metafile (EMF) Spooling Format",
                Category = "Print Job Management Policy",
                Description =
                    "Sets UseEMFSpool=1 in JobManagement policy. Configures the print spooler to spool print jobs in Enhanced Metafile (EMF) format rather than the RAW (device-ready) format. EMF spooling returns control to the application faster — the application finishes its print call as soon as the EMF commands are written to the spool file, rather than waiting for the full rasterisation to the printer's native format. The spooler then renders EMF to RAW in the background. Faster application hand-off is the primary benefit; the trade-off is that EMF rendering errors are deferred to the spooler.",
                Tags = ["print-job", "emf", "spool-format", "performance", "application-responsiveness"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Print calls return to the application faster. Rendering errors may not surface until after the application has completed the print call. EMF spool files are larger than RAW until rendered; more temporary disk space is needed during active large print jobs.",
                ApplyOps = [RegOp.SetDword(JobKey, "UseEMFSpool", 1)],
                RemoveOps = [RegOp.DeleteValue(JobKey, "UseEMFSpool")],
                DetectOps = [RegOp.CheckDword(JobKey, "UseEMFSpool", 1)],
            },
            new TweakDef
            {
                Id = "prtjob-block-untrusted-printer-fonts",
                Label = "Print Job Management: Block Untrusted Fonts in Print Jobs",
                Category = "Print Job Management Policy",
                Description =
                    "Sets BlockUntrustedFonts=1 in JobManagement policy. Blocks loading of fonts from untrusted sources within print job processing. The Windows font parsing subsystem has historically been a high-value attack target — multiple CVEs involve malformed fonts causing kernel memory corruption during parsing. Print jobs submitted from remote clients can contain embedded fonts. By blocking fonts that are not installed in the trusted Windows font store, the attack surface for font-based exploitation via print jobs is reduced. Print jobs with embedded, untrusted fonts may render with fallback system fonts.",
                Tags = ["print-job", "font", "untrusted-fonts", "kernel", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Print jobs containing fonts not in the system font store render with fallback fonts. Documents with custom branding fonts may look different when printed if those fonts are not installed on the print server. Impact visible on print servers processing documents with embedded non-standard fonts.",
                ApplyOps = [RegOp.SetDword(JobKey, "BlockUntrustedFonts", 1)],
                RemoveOps = [RegOp.DeleteValue(JobKey, "BlockUntrustedFonts")],
                DetectOps = [RegOp.CheckDword(JobKey, "BlockUntrustedFonts", 1)],
            },
        ];
}
