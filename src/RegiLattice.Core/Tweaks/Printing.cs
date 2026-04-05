namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// === Merged from: Printing.cs ===

internal static class Printing
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "printing-driver-isolation",
            Label = "Enable Print Driver Isolation",
            Category = "Maintenance 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Runs third-party print drivers in an isolated process. Prevents a buggy driver from crashing the spooler service.",
            Tags = ["printing", "driver", "stability", "isolation"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "PrintDriverIsolationGroups", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "PrintDriverIsolationOverrideCompat", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "PrintDriverIsolationGroups"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "PrintDriverIsolationOverrideCompat"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "PrintDriverIsolationGroups", 1)],
        },
        new TweakDef
        {
            Id = "printing-disable-queue-logging",
            Label = "Disable Print Queue Logging",
            Category = "Maintenance 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables event logging for print queue events. Reduces disk I/O from print job tracking. Default: 1 (Enabled). Recommended: 0 (Disabled).",
            Tags = ["printing", "logging", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print\Providers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print\Providers", "EventLog", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print\Providers", "EventLog", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print\Providers", "EventLog", 0)],
        },
        new TweakDef
        {
            Id = "printing-limit-spooler-memory",
            Label = "Limit Print Spooler Memory",
            Category = "Maintenance 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets Print Spooler to normal priority. Prevents spooler from consuming excessive CPU during large print jobs. Default: Above Normal. Recommended: Normal.",
            Tags = ["printing", "memory", "performance", "priority"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "SpoolerPriority", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "SpoolerPriority")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "SpoolerPriority", 0)],
        },
        new TweakDef
        {
            Id = "printing-disable-remote",
            Label = "Disable Remote Printing",
            Category = "Maintenance 2",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks incoming remote print connections. Reduces attack surface by preventing remote users from sending print jobs to this machine. Default: Enabled. Recommended: Disabled for security.",
            Tags = ["printing", "remote", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableRemotePrinting", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableRemotePrinting")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableRemotePrinting", 1)],
        },
        new TweakDef
        {
            Id = "printing-print-disable-legacy-mode",
            Label = "Disable Print Spooler Legacy Mode",
            Category = "Maintenance 2",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables legacy default printer mode in the print spooler. Uses modern Windows-managed default printer instead. Default: Legacy. Recommended: Disabled (modern mode).",
            Tags = ["printing", "spooler", "legacy", "default-printer"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "LegacyDefaultPrinterMode", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "LegacyDefaultPrinterMode")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "LegacyDefaultPrinterMode", 0)],
        },
        new TweakDef
        {
            Id = "printing-disable-driver-isolation",
            Label = "Enforce Printer Driver Isolation",
            Category = "Maintenance 2",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Forces printer drivers to load in isolated processes. Prevents a buggy driver from crashing the spooler. Default: per-driver setting.",
            Tags = ["printing", "driver", "isolation", "stability"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "PrintDriverIsolationOverride", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "PrintDriverIsolationOverride")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "PrintDriverIsolationOverride", 2)],
        },
        new TweakDef
        {
            Id = "printing-disable-printer-sharing",
            Label = "Disable Printer Sharing Across Network",
            Category = "Maintenance 2",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables sharing printers with other network computers. Default: not shared.",
            Tags = ["printing", "sharing", "network", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableServerThread", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableServerThread")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableServerThread", 1)],
        },
        new TweakDef
        {
            Id = "printing-copy-files-policy",
            Label = "Disable Point and Print Copy Files",
            Category = "Maintenance 2",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables copying of driver files during Point and Print connections. Mitigates PrintNightmare-class driver injection attacks. Default: allowed.",
            Tags = ["printing", "point-and-print", "copy-files", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint",
                    "RestrictDriverInstallationToAdministrators",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint",
                    "RestrictDriverInstallationToAdministrators"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint",
                    "RestrictDriverInstallationToAdministrators",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "printing-disable-default-mgmt",
            Label = "Disable Windows Manage Default Printer",
            Category = "Maintenance 2",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Prevents Windows from automatically changing the default printer based on the last printer used at each network. Default: managed.",
            Tags = ["printing", "default", "management", "automatic"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows", "LegacyDefaultPrinterMode", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows", "LegacyDefaultPrinterMode")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows", "LegacyDefaultPrinterMode", 1)],
        },
        new TweakDef
        {
            Id = "printing-disable-spooler-log",
            Label = "Disable Print Spooler Event Logging",
            Category = "Maintenance 2",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables verbose event logging by the Print Spooler service. Reduces disk I/O from spooler log writes. Default: enabled.",
            Tags = ["printing", "spooler", "logging", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "SpoolerEventLogging", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "SpoolerEventLogging")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "SpoolerEventLogging", 0)],
        },
        new TweakDef
        {
            Id = "printing-emf-despooling",
            Label = "Enable EMF Despooling for Faster Printing",
            Category = "Maintenance 2",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables EMF despooling for faster print rendering. Applications regain control sooner while printing continues in background. Default: off.",
            Tags = ["printing", "emf", "spool", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "ForceEMFDespooling", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "ForceEMFDespooling")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "ForceEMFDespooling", 1)],
        },
        new TweakDef
        {
            Id = "printing-print-default-paper-a4",
            Label = "Set Default Paper Size to A4",
            Category = "Maintenance 2",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets the default paper size to A4 (international standard). Changes from US Letter default. Default: Letter.",
            Tags = ["printing", "paper", "a4", "default"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows", "DefaultPaperSize", "A4")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows", "DefaultPaperSize")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows", "DefaultPaperSize", "A4")],
        },
        new TweakDef
        {
            Id = "printing-disable-print-notifications",
            Label = "Disable Print Job Notifications",
            Category = "Maintenance 2",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Suppresses toast notifications for print job completion. Default: notifications enabled.",
            Tags = ["printing", "notifications"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Notifications\Settings\Windows.PrintDialog"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Notifications\Settings\Windows.PrintDialog",
                    "Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Notifications\Settings\Windows.PrintDialog",
                    "Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Notifications\Settings\Windows.PrintDialog",
                    "Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "printing-disable-lpr-monitor",
            Label = "Disable LPR Port Monitor",
            Category = "Maintenance 2",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the LPR (Line Printer Remote) port monitor. Not needed on modern networks without legacy Unix/Linux printers. Default: enabled.",
            Tags = ["printing", "lpr", "legacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\lpdsvc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\lpdsvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\lpdsvc", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\lpdsvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "printing-disable-print-workflow-svc",
            Label = "Disable Print Workflow Service",
            Category = "Maintenance 2",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Print Workflow service used for custom print dialogs in Store apps. Default: manual start.",
            Tags = ["printing", "workflow", "services"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\PrintWorkflowUserSvc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\PrintWorkflowUserSvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\PrintWorkflowUserSvc", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\PrintWorkflowUserSvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "printing-spooler-crash-recovery-off",
            Label = "Disable Spooler Auto-Restart on Crash",
            Category = "Maintenance 2",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables automatic restart of the print spooler on failure. Useful in hardened server environments. Default: auto-restart enabled.",
            Tags = ["printing", "spooler", "services", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Spooler"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Spooler", "FailureActions", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Spooler", "FailureActions")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Spooler", "FailureActions", 0)],
        },
        new TweakDef
        {
            Id = "printing-disable-shared-printer-browse",
            Label = "Disable Printer Browsing on Network",
            Category = "Maintenance 2",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables automatic browsing and publishing of shared printers on the network. Reduces network noise. Default: enabled.",
            Tags = ["printing", "network", "sharing"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableBrowsing", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableBrowsing")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableBrowsing", 1)],
        },
        new TweakDef
        {
            Id = "printing-disable-web-pnp",
            Label = "Disable Web-Based Printer Plug and Play",
            Category = "Maintenance 2",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Web Plug and Play printer discovery (DisableWebPnP). Prevents printers from being located via internet/intranet discovery protocols. Default: web PnP enabled.",
            Tags = ["printing", "pnp", "network", "discovery"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableWebPnP", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableWebPnP")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableWebPnP", 1)],
        },
        new TweakDef
        {
            Id = "printing-disable-ipp-over-usb",
            Label = "Disable IPP over USB Printing",
            Category = "Maintenance 2",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Internet Printing Protocol over USB (IPP-USB). Prevents modern USB printers from using the IPP-USB communication path. Default: IPP-USB enabled.",
            Tags = ["printing", "ipp", "usb"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\IPOverUSB"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\IPOverUSB", "DisableIPOverUSB", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\IPOverUSB", "DisableIPOverUSB")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\IPOverUSB", "DisableIPOverUSB", 1)],
        },
        new TweakDef
        {
            Id = "printing-enable-client-side-render",
            Label = "Enable Client-Side Print Rendering",
            Category = "Maintenance 2",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Enables Enhanced Metafile (EMF) de-spooling so the client renders print jobs locally. Reduces print server CPU and network load. Default: server-side rendering.",
            Tags = ["printing", "performance", "rendering"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows", "EMFDespoolingEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows", "EMFDespoolingEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows", "EMFDespoolingEnabled", 1)],
        },
        new TweakDef
        {
            Id = "printing-disable-ad-publish",
            Label = "Disable Auto-Publish Printers to Active Directory",
            Category = "Maintenance 2",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents printers from being automatically published to Active Directory. Reduces AD directory pollution on workgroup machines. Default: auto-publish enabled.",
            Tags = ["printing", "active-directory", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "PublishPrinters", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "PublishPrinters")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "PublishPrinters", 0)],
        },
        new TweakDef
        {
            Id = "printing-disable-driver-auto-update",
            Label = "Disable Automatic Printer Driver Updates",
            Category = "Maintenance 2",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables automatic Windows Update-driven printer driver updates. Prevents unexpected driver changes that can break printing configuration. Default: auto-update enabled.",
            Tags = ["printing", "drivers", "windows-update"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "AutoUpdateDriverEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "AutoUpdateDriverEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "AutoUpdateDriverEnabled", 0)],
        },
    ];
}
