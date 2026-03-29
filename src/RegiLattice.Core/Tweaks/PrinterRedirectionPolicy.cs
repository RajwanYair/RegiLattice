// RegiLattice.Core — Tweaks/PrinterRedirectionPolicy.cs
// Printer Redirection Policy — Sprint 565.
// Configures Group Policy for printer redirection in Remote Desktop Services,
// Citrix, and VDI environments: client printer mapping, auto-creation,
// fallback driver behaviour, and Easy Print controls.
// Category: "Printer Redirection Policy" | Slug: prtred
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PrinterRedirectionPolicy
{
    private const string TsKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "prtred-disable-client-printer-redirect",
                Label = "Printer Redirection: Disable Client Printer Redirection in RDS Sessions",
                Category = "Printer Redirection Policy",
                Description =
                    "Sets fDisableCam=1 in Terminal Services policy. Prevents client printers from being automatically mapped into Remote Desktop Services sessions. When client printer redirection is enabled, every printer installed on the client machine is mapped into the RDS session as a session-specific printer. In large VDI deployments this creates hundreds of ghost printer objects per session host, causing significant spooler memory consumption, slow logon (each session must enumerate and map client printers), and instability. For environments where users should only print to central print servers, disabling client printer redirection is the recommended configuration.",
                Tags = ["rds", "printer-redirection", "vdi", "rdp", "logon-speed"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Client local printers are not mapped into RDS/VDI sessions. Users print to centrally managed network printers deployed via Group Policy. Users with home printers or local USB printers cannot print from remote sessions. Best used when central print server coverage is complete.",
                ApplyOps = [RegOp.SetDword(TsKey, "fDisableCam", 1)],
                RemoveOps = [RegOp.DeleteValue(TsKey, "fDisableCam")],
                DetectOps = [RegOp.CheckDword(TsKey, "fDisableCam", 1)],
            },
            new TweakDef
            {
                Id = "prtred-enable-easy-print",
                Label = "Printer Redirection: Enable Remote Desktop Easy Print Driver",
                Category = "Printer Redirection Policy",
                Description =
                    "Sets UseUniversalPrinter=1 in Terminal Services policy. Enables the Remote Desktop Easy Print driver as the primary driver for redirected client printers. When Easy Print is enabled, redirected client printers use a single universal print driver on the session host rather than requiring the client's specific printer driver to be installed on every session host server. This eliminates the printer driver management burden of server-side driver installation: a 200-server RDS farm no longer needs every printer driver for every model used by clients. The Easy Print driver communicates rendering instructions to the client, which uses its own installed driver.",
                Tags = ["rds", "easy-print", "universal-driver", "printer-redirection", "vdi"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Redirected client printers use the Easy Print universal driver. Printer-specific features (stapling, booklet mode, duplex) may be unavailable through Easy Print. Print rendering is sent to the client; print quality is consistent with local direct printing.",
                ApplyOps = [RegOp.SetDword(TsKey, "UseUniversalPrinter", 1)],
                RemoveOps = [RegOp.DeleteValue(TsKey, "UseUniversalPrinter")],
                DetectOps = [RegOp.CheckDword(TsKey, "UseUniversalPrinter", 1)],
            },
            new TweakDef
            {
                Id = "prtred-set-printer-redirection-timeout-60s",
                Label = "Printer Redirection: Set Printer Redirection Timeout to 60 Seconds",
                Category = "Printer Redirection Policy",
                Description =
                    "Sets PrinterRedirectionTimeout=60 in Terminal Services policy. Sets the maximum wait time during session logon for redirected printers to become available. When client printer redirection is enabled, the session host waits for the RDP printer redirection channel to report all client printers before proceeding with logon. On slow WAN connections, printer enumeration over RDP can take tens of seconds. If the session host waits indefinitely, logon appears to hang. Setting a 60-second timeout ensures logon proceeds even if some client printers fail to enumerate, preventing printer redirection from delaying session startup.",
                Tags = ["rds", "printer-redirection", "timeout", "logon-speed", "rdp"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Printer redirection enumeration is limited to 60 seconds. Printers that do not enumerate within 60 seconds are not available in the session. User logon proceeds after 60 seconds regardless of printer redirection status. Slow WAN clients may see fewer redirected printers.",
                ApplyOps = [RegOp.SetDword(TsKey, "PrinterRedirectionTimeout", 60)],
                RemoveOps = [RegOp.DeleteValue(TsKey, "PrinterRedirectionTimeout")],
                DetectOps = [RegOp.CheckDword(TsKey, "PrinterRedirectionTimeout", 60)],
            },
            new TweakDef
            {
                Id = "prtred-disable-xps-redirection",
                Label = "Printer Redirection: Disable XPS Printer Redirection",
                Category = "Printer Redirection Policy",
                Description =
                    "Sets DisableXpsRedirection=1 in Terminal Services policy. Prevents the Microsoft XPS Document Writer virtual printer from being redirected into user sessions. The XPS Document Writer is a file-generation virtual printer: when a user 'prints' to it, a .XPS file is created on the user's local machine. In RDS sessions, redirected XPS printing places XPS files on the user's local machine through the RDP file system redirection channel. This creates a data exfiltration path: users on session hosts with sensitive application data can 'print' documents as XPS files and take them home. Disabling XPS redirection closes this path.",
                Tags = ["rds", "xps-printer", "data-exfiltration", "restriction", "virtual-printer"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "XPS Document Writer is not available in RDS sessions. Users cannot create XPS files by printing from within RDS sessions. Physical and network printers are unaffected.",
                ApplyOps = [RegOp.SetDword(TsKey, "DisableXpsRedirection", 1)],
                RemoveOps = [RegOp.DeleteValue(TsKey, "DisableXpsRedirection")],
                DetectOps = [RegOp.CheckDword(TsKey, "DisableXpsRedirection", 1)],
            },
            new TweakDef
            {
                Id = "prtred-restrict-auto-printer-creation",
                Label = "Printer Redirection: Restrict Automatic Session Printer Creation to Default Only",
                Category = "Printer Redirection Policy",
                Description =
                    "Sets LoadDriversForDefaultPrinterOnly=1 in Terminal Services policy. Limits automatic printer creation in RDS sessions to the client's default printer only, rather than all client printers. Mapping every client printer into every session is the primary cause of session host spooler memory exhaustion in large VDI farms. A user with 5 printers on their client machine causes 5 session-specific printer entries on every session host they connect to. 'Default printer only' mode preserves the one-click printing experience for the user's preferred printer while eliminating the overhead of mapping every lesser-used printer.",
                Tags = ["rds", "printer-auto-creation", "default-printer", "vdi", "performance"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Only the client's default printer is mapped into RDS sessions. Secondary client printers are not available in sessions unless manually added by the user. Significant reduction in session host spooler memory usage in VDI deployments.",
                ApplyOps = [RegOp.SetDword(TsKey, "LoadDriversForDefaultPrinterOnly", 1)],
                RemoveOps = [RegOp.DeleteValue(TsKey, "LoadDriversForDefaultPrinterOnly")],
                DetectOps = [RegOp.CheckDword(TsKey, "LoadDriversForDefaultPrinterOnly", 1)],
            },
            new TweakDef
            {
                Id = "prtred-disable-pdf-printer-redirect",
                Label = "Printer Redirection: Disable PDF Printer Redirection in RDS",
                Category = "Printer Redirection Policy",
                Description =
                    "Sets DisablePDFRedirection=1 in Terminal Services policy. Prevents the Microsoft Print to PDF virtual printer from being redirected into RDS sessions. Microsoft Print to PDF, like XPS, is a file-generation virtual printer that creates PDF files on the user's local machine via the RDP file system redirection channel. This is an equally effective data exfiltration path: users can take sensitive documents from session hosts as PDF files. Enterprise DRM-protected documents that cannot be copied via clipboard or USB may still be 'printed' to local PDF files through this channel.",
                Tags = ["rds", "pdf-printer", "data-exfiltration", "dlp", "virtual-printer"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Microsoft Print to PDF is not available in RDS sessions. Users cannot create PDF files by printing from within RDS sessions. Physical and network printers are unaffected. Users relying on session-based PDF generation need an alternative (server-side PDF converter).",
                ApplyOps = [RegOp.SetDword(TsKey, "DisablePDFRedirection", 1)],
                RemoveOps = [RegOp.DeleteValue(TsKey, "DisablePDFRedirection")],
                DetectOps = [RegOp.CheckDword(TsKey, "DisablePDFRedirection", 1)],
            },
            new TweakDef
            {
                Id = "prtred-enable-bidirectional-communication",
                Label = "Printer Redirection: Enable Bidirectional Printer Communication",
                Category = "Printer Redirection Policy",
                Description =
                    "Sets BidiComm=1 in Terminal Services policy. Enables bidirectional (bidi) printer communication for redirected printers in RDS sessions. Bidi communication allows the session to query the printer's current status — toner levels, paper jam conditions, available paper sizes, and duplexing capability — from within the session. Without bidi, users cannot see printer status from their RDS session and the print driver cannot adapt to the printer's available options. Bidi requires the Easy Print driver path and the client to support bidi reporting.",
                Tags = ["rds", "bidi", "printer-status", "toner", "bidirectional"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Redirected printers report toner, paper, and status information to the RDS session. Users see accurate printer capability information in print dialogs. Requires Easy Print driver and a printer with bidi reporting capability.",
                ApplyOps = [RegOp.SetDword(TsKey, "BidiComm", 1)],
                RemoveOps = [RegOp.DeleteValue(TsKey, "BidiComm")],
                DetectOps = [RegOp.CheckDword(TsKey, "BidiComm", 1)],
            },
            new TweakDef
            {
                Id = "prtred-set-max-redirected-printers-5",
                Label = "Printer Redirection: Limit Redirected Printers Per Session to 5",
                Category = "Printer Redirection Policy",
                Description =
                    "Sets MaxRedirectedPrinters=5 in Terminal Services policy. Caps the maximum number of client printers that can be redirected into a single RDS session. Without this limit, a user with 20+ printers installed (e.g., a power user with many VPN-connected branch printers) will have all 20 mapped into every session — consuming substantial memory and logon time on the session host server. Limiting to 5 redirected printers covers virtually all legitimate printing needs while preventing excessive session host resource consumption from clients with large printer inventories.",
                Tags = ["rds", "printer-limit", "session", "performance", "resource"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "At most 5 client printers are mapped per RDS session. Clients with more than 5 printers have their first 5 (by enumeration order) mapped. Users rarely need more than 5 printers in a session. Configure in conjunction with default-printer-only policy for maximum benefit.",
                ApplyOps = [RegOp.SetDword(TsKey, "MaxRedirectedPrinters", 5)],
                RemoveOps = [RegOp.DeleteValue(TsKey, "MaxRedirectedPrinters")],
                DetectOps = [RegOp.CheckDword(TsKey, "MaxRedirectedPrinters", 5)],
            },
            new TweakDef
            {
                Id = "prtred-use-compression-for-print-data",
                Label = "Printer Redirection: Enable Compression for Redirected Print Data",
                Category = "Printer Redirection Policy",
                Description =
                    "Sets CompressPrintData=1 in Terminal Services policy. Enables compression of print job data transmitted through the RDP printer redirection channel. Print job data (especially EMF) can be highly compressible — text-heavy documents may compress by 80%+. Without compression, printing large documents over WAN-connected RDS sessions consumes significant RDP session bandwidth. With compression enabled, the RDP virtual channel compresses the print data stream before transmission, reducing the bandwidth and time required to print large documents over slow connections.",
                Tags = ["rds", "print-compression", "bandwidth", "wan", "rdp"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Print data is compressed before transmission through the RDP channel. Significant bandwidth savings for text-heavy documents over WAN connections. Minor CPU overhead on the session host for compression. No user-visible impact.",
                ApplyOps = [RegOp.SetDword(TsKey, "CompressPrintData", 1)],
                RemoveOps = [RegOp.DeleteValue(TsKey, "CompressPrintData")],
                DetectOps = [RegOp.CheckDword(TsKey, "CompressPrintData", 1)],
            },
            new TweakDef
            {
                Id = "prtred-allow-only-easy-print-fallback",
                Label = "Printer Redirection: Use Easy Print as Exclusive Fallback Driver",
                Category = "Printer Redirection Policy",
                Description =
                    "Sets FallbackToEasyPrint=1 in Terminal Services policy. Configures RDS to use the Easy Print driver as the exclusive fallback when the client printer's specific driver is not installed on the session host. Without this setting, if the specific printer driver is absent, redirection may fail entirely or attempt to download the driver automatically. With FallbackToEasyPrint enabled, any printer whose driver is not on the server falls back to Easy Print — ensuring the printer is always usable even if not optimally configured. Eliminates 'Printer unavailable' errors from driver-absent conditions.",
                Tags = ["rds", "easy-print", "fallback-driver", "printer-availability", "rdp"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Printers without a matching server-side driver fall back to Easy Print. All client printers are available in sessions, potentially with reduced feature sets. Eliminates printer redirection failures due to missing drivers without requiring driver installation on session hosts.",
                ApplyOps = [RegOp.SetDword(TsKey, "FallbackToEasyPrint", 1)],
                RemoveOps = [RegOp.DeleteValue(TsKey, "FallbackToEasyPrint")],
                DetectOps = [RegOp.CheckDword(TsKey, "FallbackToEasyPrint", 1)],
            },
        ];
}
