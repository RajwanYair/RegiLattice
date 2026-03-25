// RegiLattice.Core — Tweaks/PrintManagementPolicy.cs
// Sprint 296: Print Management Policy tweaks (10 tweaks)
// Category: "Print Management Policy" | Slug: prtmgmt
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PrintManagement

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PrintManagementPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PrintManagement";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "prtmgmt-disable-mmc",
            Label = "Disable Print Management MMC Console",
            Category = "Print Management Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "The Print Management MMC console provides a graphical interface for managing printers, print queues, and printer servers across an organization. Disabling the Print Management MMC console prevents users and non-print-admin accounts from accessing this powerful management interface. Centralized print management is handled by designated print server administrators rather than individual workstation users. Restricting access to the MMC prevents accidental or unauthorized printer configuration changes. Printer management tasks are delegated to specialized IT staff through proper RBAC mechanisms. Standard print functionality including printing documents and managing local print jobs remains fully accessible to users.",
            Tags = ["printing", "management", "mmc", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisablePrintManagementMmc", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisablePrintManagementMmc")],
            DetectOps = [RegOp.CheckDword(Key, "DisablePrintManagementMmc", 1)],
        },
        new TweakDef
        {
            Id = "prtmgmt-disable-driver-autoinstall",
            Label = "Disable Printer Driver Auto-Install",
            Category = "Print Management Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "Printer driver auto-installation automatically downloads and installs printer drivers when a network printer is connected or added. Disabling auto-installation prevents arbitrary printer drivers from being installed from network sources without administrator approval. The Windows print driver ecosystem has historically been a significant source of privilege escalation vulnerabilities including PrintNightmare. Requiring administrator approval for all printer driver installations enforces a curated and tested driver baseline. Enterprise print environments deploy standardized, approved driver packages through SCCM or similar management platforms. Blocking automatic driver installation is a key mitigation for print spooler attack vectors on managed endpoints.",
            Tags = ["printing", "drivers", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisablePrinterDriverAutoInstall", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisablePrinterDriverAutoInstall")],
            DetectOps = [RegOp.CheckDword(Key, "DisablePrinterDriverAutoInstall", 1)],
        },
        new TweakDef
        {
            Id = "prtmgmt-disable-default-mgmt",
            Label = "Disable Default Printer Management",
            Category = "Print Management Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Windows automatically manages the default printer by changing it to the most recently used printer when the dynamic default printer setting is active. Disabling default printer management through policy prevents Windows from automatically changing the configured default printer. Enterprise print environments rely on precise default printer assignments tied to physical location or department, which should not be dynamically changed. Automatic default printer changes cause user confusion and support calls when the wrong printer is selected for important documents. Lock-in of the default printer to a specific device is commonly required for compliance and operational workflow reasons. Disabling dynamic default management ensures the IT-configured default printer assignment remains stable.",
            Tags = ["printing", "default-printer", "management", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableDefaultPrinterManagement", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableDefaultPrinterManagement")],
            DetectOps = [RegOp.CheckDword(Key, "DisableDefaultPrinterManagement", 1)],
        },
        new TweakDef
        {
            Id = "prtmgmt-disable-queue-sharing",
            Label = "Disable Print Queue Sharing",
            Category = "Print Management Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Print queue sharing allows workstations to act as print servers by sharing locally connected printers to other network clients. Disabling queue sharing prevents workstations from hosting shared print queues, consolidating print spooler exposure to designated print servers. Workstation-based print sharing expands the print spooler attack surface to a larger number of targets on the network. Enterprise printing should be managed through dedicated print servers with hardened configurations and regular patching. Reducing the number of hosts running print server functionality limits the blast radius of print-related vulnerabilities. Centralized print servers with properly configured access controls provide superior audit and management capabilities compared to peer-to-peer sharing.",
            Tags = ["printing", "sharing", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableQueueSharing", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableQueueSharing")],
            DetectOps = [RegOp.CheckDword(Key, "DisableQueueSharing", 1)],
        },
        new TweakDef
        {
            Id = "prtmgmt-disable-print-pdf-rdp",
            Label = "Disable Print to PDF from RDP",
            Category = "Print Management Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            Description =
                "Print to PDF from Remote Desktop sessions allows users to redirect print jobs from remote desktop sessions to local PDF files on the connecting client. Disabling this feature prevents document exfiltration through the Print to PDF mechanism in RDP sessions. Sensitive documents viewed in remote desktop sessions can be saved locally on unauthorized or unmanaged client devices via PDF redirection. Data governance policies require that documents accessed through remote desktop remain on the enterprise endpoint and are not redirected to potentially unmanaged clients. Print redirection in general represents a data movement risk in RDP scenarios handling confidential information. Organizations with strict data residency requirements should disable all print redirection in remote desktop sessions.",
            Tags = ["printing", "rdp", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisablePrintToPdfFromRdp", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisablePrintToPdfFromRdp")],
            DetectOps = [RegOp.CheckDword(Key, "DisablePrintToPdfFromRdp", 1)],
        },
        new TweakDef
        {
            Id = "prtmgmt-disable-telemetry",
            Label = "Disable Print Management Telemetry",
            Category = "Print Management Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Description =
                "Print management telemetry reports usage statistics about printer configurations, print job characteristics, and driver versions to Microsoft. This data helps improve printer compatibility and print subsystem performance in future Windows releases. Disabling print management telemetry prevents printer configuration and usage data from being transmitted outside the enterprise. Print infrastructure details including printer models, drivers, and queue configurations represent sensitive IT asset information. Enterprise print environment information should not be disclosed through telemetry to external parties. Print functionality is completely unaffected by disabling this telemetry stream.",
            Tags = ["printing", "telemetry", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisablePrintManagementTelemetry", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisablePrintManagementTelemetry")],
            DetectOps = [RegOp.CheckDword(Key, "DisablePrintManagementTelemetry", 1)],
        },
        new TweakDef
        {
            Id = "prtmgmt-disable-discovery",
            Label = "Disable Printer Discovery",
            Category = "Print Management Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Printer discovery uses WS-Discovery, mDNS, and network broadcast protocols to automatically find printers on the local network. Disabling printer discovery prevents workstations from automatically enumerating network printers and presenting them to users for installation. Enterprise printer deployments are managed through Group Policy printer deployment, not automatic discovery. Automatic printer discovery can expose users to rogue printers or allow installation of unapproved printer devices. Centrally managed printer policies ensure users only have access to approved printers with validated drivers. Disabling discovery does not remove previously installed network printers from the device.",
            Tags = ["printing", "discovery", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisablePrinterDiscovery", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisablePrinterDiscovery")],
            DetectOps = [RegOp.CheckDword(Key, "DisablePrinterDiscovery", 1)],
        },
        new TweakDef
        {
            Id = "prtmgmt-disable-xps-writer",
            Label = "Disable Microsoft XPS Document Writer",
            Category = "Print Management Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "The Microsoft XPS Document Writer is a virtual printer that saves print output as XPS format files on the local filesystem. Disabling the XPS Document Writer prevents users from saving print jobs as local XPS files, limiting an alternate document export path. XPS files created through the virtual printer can bypass DLP controls that monitor file creation through standard save dialogs. In environments where Microsoft Print to PDF is also disabled, disabling the XPS writer eliminates file-based print redirection. Enterprise document management workflows should use approved document export paths rather than virtual printer mechanisms. Disabling this virtual printer reduces the attack surface while preserving all physical printer functionality.",
            Tags = ["printing", "xps", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableMicrosoftXpsDocumentWriter", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableMicrosoftXpsDocumentWriter")],
            DetectOps = [RegOp.CheckDword(Key, "DisableMicrosoftXpsDocumentWriter", 1)],
        },
        new TweakDef
        {
            Id = "prtmgmt-disable-internet-printing",
            Label = "Disable Internet Printing",
            Category = "Print Management Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "Internet Printing Protocol (IPP) allows clients to submit print jobs to remote printers over HTTP connections including those on the public internet. Disabling internet printing prevents the Windows print spooler from acting as an IPP client or accepting IPP-based print requests from the internet. Internet-accessible print services represent an attack vector used in PrintNightmare and related print spooler exploitation chains. Enterprise printers are managed on segmented internal networks and should not be accessible or reachable via internet-routable paths. Removing internet printing capability eliminates printer-based lateral movement vectors used in network attacks. All internal network printing through standard SMB and IPP on the corporate LAN remains unaffected.",
            Tags = ["printing", "internet", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableInternetPrinting", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableInternetPrinting")],
            DetectOps = [RegOp.CheckDword(Key, "DisableInternetPrinting", 1)],
        },
        new TweakDef
        {
            Id = "prtmgmt-disable-cloud-print-sharing",
            Label = "Disable Cloud Print Sharing",
            Category = "Print Management Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Cloud print sharing allows enterprise printers to be shared through Microsoft's cloud printing infrastructure for access from mobile devices and remote workers. Disabling cloud print sharing prevents printers from being registered with cloud print services and accessed through cloud endpoints. Cloud-shared printers send print jobs through Microsoft's cloud infrastructure, which creates data residency and confidentiality concerns for sensitive documents. Organizations handling classified, regulated, or sensitive documents should ensure all print paths remain within the enterprise boundary. Cloud print sharing functionality is appropriate for consumer scenarios but requires careful data governance evaluation for enterprise use. Disabling cloud print sharing ensures all print data stays within the physical enterprise network infrastructure.",
            Tags = ["printing", "cloud", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableCloudPrintSharing", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableCloudPrintSharing")],
            DetectOps = [RegOp.CheckDword(Key, "DisableCloudPrintSharing", 1)],
        },
    ];
}
