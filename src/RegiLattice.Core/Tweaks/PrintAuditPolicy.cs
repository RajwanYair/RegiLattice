// RegiLattice.Core — Tweaks/PrintAuditPolicy.cs
// Print Audit Policy — Sprint 563.
// Configures Group Policy for printer usage auditing, print job tracking,
// document watermarking policy, and sensitive document print controls.
// Category: "Print Audit Policy" | Slug: prtaud
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows NT\Printers\AuditPrint

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PrintAuditPolicy
{
    private const string AudKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\AuditPrint";

    private const string PrtKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "prtaud-enable-print-job-auditing",
                Label = "Print Audit: Enable Print Job Audit Events",
                Category = "Print Audit Policy",
                Description =
                    "Sets AuditPrintJobs=1 in AuditPrint policy. Enables security audit events for every print job processed by the Windows print spooler. When enabled, the Windows Security event log receives Event ID 4624 (document print event) for each job including: user name, computer name, printer name, document name, job ID, number of pages, and bytes printed. This provides a complete record of document print activity — essential for data loss prevention auditing (detecting mass printing of PII), compliance (HIPAA, SOX printed document requirements), and forensic investigation.",
                Tags = ["print-audit", "security-log", "dlp", "print-jobs", "monitoring"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Every print job generates a security audit event. Security event log volume increases — ensure the event log size is sufficient and logs are forwarded to a SIEM. Document names in the log may contain sensitive information from the job metadata.",
                ApplyOps = [RegOp.SetDword(AudKey, "AuditPrintJobs", 1)],
                RemoveOps = [RegOp.DeleteValue(AudKey, "AuditPrintJobs")],
                DetectOps = [RegOp.CheckDword(AudKey, "AuditPrintJobs", 1)],
            },
            new TweakDef
            {
                Id = "prtaud-enable-printer-config-auditing",
                Label = "Print Audit: Enable Printer Configuration Change Auditing",
                Category = "Print Audit Policy",
                Description =
                    "Sets AuditPrinterConfiguration=1 in AuditPrint policy. Enables audit events when printer configuration changes are made: printer added, printer deleted, default printer changed, printer properties modified, printer sharing enabled or disabled. Unauthorised printer configuration changes can be used by attackers to redirect print jobs (malicious printer substitution attack) or to create new printer shares for lateral movement. Configuration change auditing creates an immutable log of every printer infrastructure modification for forensic review.",
                Tags = ["print-audit", "configuration", "printer-add", "security", "compliance"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Printer configuration changes generate audit events. SIEM rules for suspicious printer configuration changes (printers added/modified by non-admin accounts) detect potential print spooler abuse. Minimal event volume in stable environments.",
                ApplyOps = [RegOp.SetDword(AudKey, "AuditPrinterConfiguration", 1)],
                RemoveOps = [RegOp.DeleteValue(AudKey, "AuditPrinterConfiguration")],
                DetectOps = [RegOp.CheckDword(AudKey, "AuditPrinterConfiguration", 1)],
            },
            new TweakDef
            {
                Id = "prtaud-enable-driver-install-auditing",
                Label = "Print Audit: Enable Printer Driver Installation Auditing",
                Category = "Print Audit Policy",
                Description =
                    "Sets AuditDriverInstall=1 in AuditPrint policy. Enables audit events for printer driver installation and removal operations. Printer driver installations are a critical security event path — PrintNightmare and related exploits specifically used driver installation as the code execution vector. Auditing every driver install event provides a detection opportunity: SIEM rules can alert on driver installations by non-IT accounts, installations of unexpected driver names, or driver installs that occur at unusual times. Complements the restriction policies that require admin rights for driver installation.",
                Tags = ["print-audit", "driver-install", "printnightmare", "security", "detection"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Printer driver installations and removals generate audit events. Alerts on unexpected driver installs are a high-fidelity PrintNightmare indicator. Negligible event volume in controlled environments.",
                ApplyOps = [RegOp.SetDword(AudKey, "AuditDriverInstall", 1)],
                RemoveOps = [RegOp.DeleteValue(AudKey, "AuditDriverInstall")],
                DetectOps = [RegOp.CheckDword(AudKey, "AuditDriverInstall", 1)],
            },
            new TweakDef
            {
                Id = "prtaud-enable-print-server-connections",
                Label = "Print Audit: Enable Audit for Print Server Connection Events",
                Category = "Print Audit Policy",
                Description =
                    "Sets AuditServerConnections=1 in AuditPrint policy. Enables audit events when clients connect to and disconnect from the print server's spooler service via RPC. Each connection event records the client machine name, user account, and connection timestamp. Print server connection auditing is particularly valuable for detecting exploitation of print spooler RPC vulnerabilities: an attacker scanning for PrintNightmare-vulnerable servers will generate connection events before any exploit payload is sent. The connection pattern (connection from unusual machines, outside business hours) is detectable.",
                Tags = ["print-audit", "server-connections", "rpc", "security", "detection"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Print server RPC connection and disconnection events are logged. In environments with many print clients, this generates high event volume. Consider applying to high-value print servers only and forwarding to central SIEM for analysis.",
                ApplyOps = [RegOp.SetDword(AudKey, "AuditServerConnections", 1)],
                RemoveOps = [RegOp.DeleteValue(AudKey, "AuditServerConnections")],
                DetectOps = [RegOp.CheckDword(AudKey, "AuditServerConnections", 1)],
            },
            new TweakDef
            {
                Id = "prtaud-set-print-log-max-7days",
                Label = "Print Audit: Retain Print Audit Log for 7 Days",
                Category = "Print Audit Policy",
                Description =
                    "Sets AuditLogRetentionDays=7 in AuditPrint policy. Sets the minimum retention period for print audit log entries to 7 days. Print audit log retention of at least 7 days satisfies most operational investigation requirements: typical incident detection occurs within 24-48 hours, and 7 days provides sufficient lookback to correlate print events with the full timeline of an incident. Retaining logs beyond 30 days without SIEM export strains local storage on print servers. This policy sets the minimum — logs should be forwarded to a SIEM for long-term retention independently.",
                Tags = ["print-audit", "log-retention", "compliance", "siem", "investigation"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Print audit logs are retained locally for at minimum 7 days. SIEM forwarding is recommended for longer retention. Local disk space consumption is proportional to job volume.",
                ApplyOps = [RegOp.SetDword(AudKey, "AuditLogRetentionDays", 7)],
                RemoveOps = [RegOp.DeleteValue(AudKey, "AuditLogRetentionDays")],
                DetectOps = [RegOp.CheckDword(AudKey, "AuditLogRetentionDays", 7)],
            },
            new TweakDef
            {
                Id = "prtaud-disable-direct-printing-bypass",
                Label = "Print Audit: Disable Direct Printing Bypass (Enforce Spooler Path)",
                Category = "Print Audit Policy",
                Description =
                    "Sets DisableDirectPrinting=1 in Printers policy. Prevents applications from sending print jobs directly to printer hardware ports, bypassing the Windows print spooler. Applications that print directly to a port (WriteFile to LPT1:, socket to port 9100, or direct Win32 printer I/O) bypass the entire print audit chain — no job events, no audit log, no DLP scanning. Enforcing the spooler path ensures all print output is intercepted, logged, and subject to print quota policies. Required for complete print audit coverage.",
                Tags = ["direct-printing", "spooler-bypass", "dlp", "audit", "control"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Applications that bypass the spooler with direct port I/O (legacy manufacturing, point-of-sale, label printers) may stop printing. Test with all applications that use non-standard printing methods before deploying. Standard Windows GDI/WDM/XPS printing paths are unaffected.",
                ApplyOps = [RegOp.SetDword(PrtKey, "DisableDirectPrinting", 1)],
                RemoveOps = [RegOp.DeleteValue(PrtKey, "DisableDirectPrinting")],
                DetectOps = [RegOp.CheckDword(PrtKey, "DisableDirectPrinting", 1)],
            },
            new TweakDef
            {
                Id = "prtaud-enable-page-count-tracking",
                Label = "Print Audit: Enable Per-User Print Page Count Tracking",
                Category = "Print Audit Policy",
                Description =
                    "Sets EnablePageTracking=1 in AuditPrint policy. Enables per-user print page count tracking in the Windows print spooler. Page count data is accumulated in the print quota subsystem and can be consumed by print accounting software, print management consoles, and quota enforcement systems. Without page tracking, print accountability is based on job counts rather than page volumes — a user printing 500-page documents daily appears identical to one printing 10 single-page emails. Page tracking is prerequisite to enforcing any meaningful print volume policy.",
                Tags = ["print-audit", "page-tracking", "quota", "accounting", "usage"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Per-user and per-printer page count data is tracked. Negligible overhead. Data is accessible via Print Management console and print accounting APIs. Does not enforce quotas by itself — pair with a print quota enforcement solution.",
                ApplyOps = [RegOp.SetDword(AudKey, "EnablePageTracking", 1)],
                RemoveOps = [RegOp.DeleteValue(AudKey, "EnablePageTracking")],
                DetectOps = [RegOp.CheckDword(AudKey, "EnablePageTracking", 1)],
            },
            new TweakDef
            {
                Id = "prtaud-restrict-color-printing",
                Label = "Print Audit: Restrict Colour Printing to Authorised Users",
                Category = "Print Audit Policy",
                Description =
                    "Sets RestrictColorPrinting=1 in AuditPrint policy. Restricts colour printing capability on managed printers to users who are members of an authorised colour printing security group. All other users are limited to monochrome (black and white) output. Colour printing costs are typically 5-10× higher than monochrome per page. Unrestricted colour printing is a significant operational cost driver in large organisations. Restricting colour printing to users with a business need (design, marketing, executive) provides measurable cost reduction without impacting most users.",
                Tags = ["print-audit", "colour-printing", "cost-control", "restriction", "compliance"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Colour printing is restricted to authorised users. Unauthorised users print in monochrome regardless of printer capability. Colour authorisation group must be configured in print server properties. Significant toner cost reduction in large deployments.",
                ApplyOps = [RegOp.SetDword(AudKey, "RestrictColorPrinting", 1)],
                RemoveOps = [RegOp.DeleteValue(AudKey, "RestrictColorPrinting")],
                DetectOps = [RegOp.CheckDword(AudKey, "RestrictColorPrinting", 1)],
            },
            new TweakDef
            {
                Id = "prtaud-enable-secure-print-release",
                Label = "Print Audit: Enable Secure Print Release (Hold-and-Release)",
                Category = "Print Audit Policy",
                Description =
                    "Sets EnableSecurePrint=1 in AuditPrint policy. Enables print job hold-and-release (secure print) mode: jobs are queued on the print server but not released to the physical printer until the user authenticates at the printer panel (PIN, smart card, or badge). Documents are not printed and left unattended on the printer tray — a significant physical security and confidentiality control. Sensitive documents printed to shared office printers routinely sit uncollected for minutes to hours. Secure print release eliminates physical information disclosure.",
                Tags = ["print-audit", "secure-print", "hold-release", "physical-security", "confidentiality"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Print jobs are held until the submitter authenticates at the printer. Requires printer hardware that supports hold-and-release (most enterprise MFPs). Users must approach the printer to release jobs. Uncollected jobs expire after the configured timeout.",
                ApplyOps = [RegOp.SetDword(AudKey, "EnableSecurePrint", 1)],
                RemoveOps = [RegOp.DeleteValue(AudKey, "EnableSecurePrint")],
                DetectOps = [RegOp.CheckDword(AudKey, "EnableSecurePrint", 1)],
            },
            new TweakDef
            {
                Id = "prtaud-log-deleted-print-jobs",
                Label = "Print Audit: Log Deleted and Cancelled Print Jobs",
                Category = "Print Audit Policy",
                Description =
                    "Sets AuditDeletedJobs=1 in AuditPrint policy. Enables audit events when print jobs are deleted or cancelled from the print queue. Print job deletion events capture the who (user account that cancelled), what (document name, printer, job ID), and when (timestamp). Deletions by accounts that did not submit the job indicate queue manipulation — an administrator (or attacker with elevated privileges) deleting another user's print job. This is relevant in secure print environments where deleted-before-release events indicate tampering with the print queue.",
                Tags = ["print-audit", "deleted-jobs", "queue-manipulation", "security", "forensics"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Cancelled and deleted print jobs generate audit events. SIEM correlation of the submitter vs. the deleting account detects queue manipulation. Negligible event volume in normal environments.",
                ApplyOps = [RegOp.SetDword(AudKey, "AuditDeletedJobs", 1)],
                RemoveOps = [RegOp.DeleteValue(AudKey, "AuditDeletedJobs")],
                DetectOps = [RegOp.CheckDword(AudKey, "AuditDeletedJobs", 1)],
            },
        ];
}
