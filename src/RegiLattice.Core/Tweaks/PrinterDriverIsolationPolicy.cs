// RegiLattice.Core — Tweaks/PrinterDriverIsolationPolicy.cs
// Printer driver isolation, sandboxing, and third-party driver installation policy — Sprint 473.
// Category: "Printer Driver Isolation Policy" | Slug: pdrv
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows NT\Printers\DriverIsolation

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PrinterDriverIsolationPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\DriverIsolation";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "pdrv-enforce-driver-isolation",
                Label = "Enforce Printer Driver Isolation (Separate Process)",
                Category = "Printer Driver Isolation Policy",
                Description =
                    "Forces printer drivers to run in isolated processes separate from the spooler service, preventing a buggy or malicious printer driver from crashing or compromising the spooler.",
                Tags = ["printing", "driver-isolation", "spooler", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Printer drivers run isolated from spooler; spooler crash impact from bad driver reduced.",
                ApplyOps = [RegOp.SetDword(Key, "IsolationMode", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "IsolationMode")],
                DetectOps = [RegOp.CheckDword(Key, "IsolationMode", 1)],
            },
            new TweakDef
            {
                Id = "pdrv-block-unsigned-drivers",
                Label = "Block Installation of Unsigned Printer Drivers",
                Category = "Printer Driver Isolation Policy",
                Description =
                    "Blocks the installation of printer drivers that do not have a valid WHQL or enterprise certificate signature, preventing malicious or vulnerable unsigned printer drivers from loading.",
                Tags = ["printing", "unsigned-driver", "security", "whql", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Unsigned printer drivers rejected; only WHQL or enterprise-signed drivers install.",
                ApplyOps = [RegOp.SetDword(Key, "BlockUnsignedPrinterDrivers", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockUnsignedPrinterDrivers")],
                DetectOps = [RegOp.CheckDword(Key, "BlockUnsignedPrinterDrivers", 1)],
            },
            new TweakDef
            {
                Id = "pdrv-limit-driver-install-to-admin",
                Label = "Restrict Printer Driver Installation to Administrators Only",
                Category = "Printer Driver Isolation Policy",
                Description =
                    "Requires administrator privileges to install any new printer driver, preventing standard users from adding potentially exploitable printer drivers via easy-to-add printer workflows.",
                Tags = ["printing", "driver-install", "admin", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Standard users cannot install printer drivers; admin credentials required.",
                ApplyOps = [RegOp.SetDword(Key, "RequireAdminForDriverInstall", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireAdminForDriverInstall")],
                DetectOps = [RegOp.CheckDword(Key, "RequireAdminForDriverInstall", 1)],
            },
            new TweakDef
            {
                Id = "pdrv-disable-v3-kernel-drivers",
                Label = "Disable Legacy V3 Kernel-Mode Printer Drivers",
                Category = "Printer Driver Isolation Policy",
                Description =
                    "Disables legacy V3 (kernel-mode) printer drivers, allowing only V4 user-mode drivers which run isolated from the kernel and reduce the risk of privilege escalation via printer drivers.",
                Tags = ["printing", "v3-driver", "kernel-mode", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "V3 kernel-mode printer drivers blocked; some older printers may require V4 driver wrappers.",
                ApplyOps = [RegOp.SetDword(Key, "DisableV3KernelModePrinterDrivers", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableV3KernelModePrinterDrivers")],
                DetectOps = [RegOp.CheckDword(Key, "DisableV3KernelModePrinterDrivers", 1)],
            },
            new TweakDef
            {
                Id = "pdrv-block-network-driver-download",
                Label = "Block Automatic Printer Driver Download from Network",
                Category = "Printer Driver Isolation Policy",
                Description =
                    "Blocks Windows from automatically downloading printer drivers from remote print servers or Windows Update when a new printer is detected, requiring manual driver installation.",
                Tags = ["printing", "auto-driver-download", "network", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Auto printer driver download blocked; must manually install drivers before adding network printers.",
                ApplyOps = [RegOp.SetDword(Key, "BlockAutoPrinterDriverDownload", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockAutoPrinterDriverDownload")],
                DetectOps = [RegOp.CheckDword(Key, "BlockAutoPrinterDriverDownload", 1)],
            },
            new TweakDef
            {
                Id = "pdrv-enable-enhanced-point-and-print",
                Label = "Enable Enhanced Point and Print Restriction",
                Category = "Printer Driver Isolation Policy",
                Description =
                    "Enables enhanced Point and Print restrictions requiring that drivers originate from an approved printer server list, preventing attackers from serving malicious drivers via rogue print servers.",
                Tags = ["printing", "point-and-print", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Point and Print restricted to approved servers; rogue print server attacks blocked.",
                ApplyOps = [RegOp.SetDword(Key, "Restricted", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "Restricted")],
                DetectOps = [RegOp.CheckDword(Key, "Restricted", 1)],
            },
            new TweakDef
            {
                Id = "pdrv-disable-driver-update-prompt",
                Label = "Disable Automatic Printer Driver Update Prompts",
                Category = "Printer Driver Isolation Policy",
                Description =
                    "Suppresses automatic driver update prompts from existing printer drivers via Windows Update, preventing unexpected printer driver updates that could introduce vulnerabilities.",
                Tags = ["printing", "driver-update", "windows-update", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Auto driver update prompts suppressed; printer drivers only update via manual action.",
                ApplyOps = [RegOp.SetDword(Key, "DisableDriverUpdatePrompt", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDriverUpdatePrompt")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDriverUpdatePrompt", 1)],
            },
            new TweakDef
            {
                Id = "pdrv-block-driver-staging-from-drivers-folder",
                Label = "Block Driver Installation from Drivers Folder Without Inbox",
                Category = "Printer Driver Isolation Policy",
                Description =
                    "Prevents printer drivers from being installed from the Windows Drivers directory without being in the inbox driver store, blocking attack paths that stage evil drivers into the Drivers folder.",
                Tags = ["printing", "driver-staging", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Only inbox-store printer drivers installable; staged evil-driver attack paths closed.",
                ApplyOps = [RegOp.SetDword(Key, "BlockStagedDriverInstalls", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockStagedDriverInstalls")],
                DetectOps = [RegOp.CheckDword(Key, "BlockStagedDriverInstalls", 1)],
            },
            new TweakDef
            {
                Id = "pdrv-disable-printer-driver-dcom",
                Label = "Disable DCOM Access for Printer Driver Processes",
                Category = "Printer Driver Isolation Policy",
                Description =
                    "Prevents printer driver host processes from making DCOM calls to other processes, reducing lateral movement risk if a printer driver process is compromised.",
                Tags = ["printing", "dcom", "driver-isolation", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "DCOM disabled for printer driver host processes; compromised driver cannot do COM-based lateral movement.",
                ApplyOps = [RegOp.SetDword(Key, "DisableDCOMInDriverHost", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDCOMInDriverHost")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDCOMInDriverHost", 1)],
            },
            new TweakDef
            {
                Id = "pdrv-log-driver-install-events",
                Label = "Enable Audit Logging for Printer Driver Installs",
                Category = "Printer Driver Isolation Policy",
                Description =
                    "Enables security audit events whenever a printer driver is installed, updated, or removed, providing a log trail for detecting unauthorized driver installation activity.",
                Tags = ["printing", "audit-log", "driver-install", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Printer driver installation events logged in Security event log; unauthorised installs detectable.",
                ApplyOps = [RegOp.SetDword(Key, "AuditDriverInstallEvents", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditDriverInstallEvents")],
                DetectOps = [RegOp.CheckDword(Key, "AuditDriverInstallEvents", 1)],
            },
        ];
}
