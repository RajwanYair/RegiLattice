namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Printing
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "printing-disable-spooler-autostart",
            Label = "Set Print Spooler to Manual Start",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Sets the Print Spooler service to Manual start. Reduces attack surface (PrintNightmare) and improves boot time if no printer is used.",
            Tags = ["printing", "spooler", "security", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Spooler"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Spooler", "Start", 3),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Spooler", "Start", 2),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Spooler", "Start", 3)],
        },
        new TweakDef
        {
            Id = "printing-driver-isolation",
            Label = "Enable Print Driver Isolation",
            Category = "Printing",
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
            Id = "printing-disable-pointandprint",
            Label = "Restrict Point-and-Print Drivers",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Requires admin approval for Point-and-Print driver installs. Mitigates PrintNightmare and similar spooler vulnerabilities.",
            Tags = ["printing", "security", "pointandprint", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint", "RestrictDriverInstallationToAdministrators", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint", "NoWarningNoElevationOnInstall", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint", "RestrictDriverInstallationToAdministrators"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint", "NoWarningNoElevationOnInstall"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint", "RestrictDriverInstallationToAdministrators", 1)],
        },
        new TweakDef
        {
            Id = "printing-disable-default-mgmt",
            Label = "Disable Windows Default Printer Management",
            Category = "Printing",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Stops Windows from automatically setting the default printer to the last one used. Keeps your chosen default printer fixed.",
            Tags = ["printing", "default", "management"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Windows"],
        },
        new TweakDef
        {
            Id = "printing-disable-xps-writer",
            Label = "Disable XPS Document Writer",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes the Microsoft XPS Document Writer virtual printer from the printers list. Reduces clutter if you never use XPS.",
            Tags = ["printing", "xps", "cleanup"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableXPSDocumentWriter", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableXPSDocumentWriter"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableXPSDocumentWriter", 1)],
        },
        new TweakDef
        {
            Id = "printing-disable-publishing",
            Label = "Disable Network Printer Publishing",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents shared printers from being published in Active Directory. Reduces AD clutter on enterprise networks.",
            Tags = ["printing", "network", "activedirectory", "enterprise"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers"],
        },
        new TweakDef
        {
            Id = "printing-disable-internet-printing",
            Label = "Disable Internet Printing (IPP)",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Internet Printing Protocol and Web PnP driver downloads. Reduces attack surface from internet-facing print services.",
            Tags = ["printing", "internet", "security", "ipp"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableHTTPPrinting", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableWebPnPDownload", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableHTTPPrinting"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableWebPnPDownload"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableHTTPPrinting", 1)],
        },
        new TweakDef
        {
            Id = "printing-disable-queue-logging",
            Label = "Disable Print Queue Logging",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables event logging for print queue events. Reduces disk I/O from print job tracking. Default: 1 (Enabled). Recommended: 0 (Disabled).",
            Tags = ["printing", "logging", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print\Providers"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print\Providers", "EventLog", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print\Providers", "EventLog", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print\Providers", "EventLog", 0)],
        },
        new TweakDef
        {
            Id = "printing-limit-spooler-memory",
            Label = "Limit Print Spooler Memory",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets Print Spooler to normal priority. Prevents spooler from consuming excessive CPU during large print jobs. Default: Above Normal. Recommended: Normal.",
            Tags = ["printing", "memory", "performance", "priority"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "SpoolerPriority", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "SpoolerPriority"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "SpoolerPriority", 0)],
        },
        new TweakDef
        {
            Id = "printing-disable-remote",
            Label = "Disable Remote Printing",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Blocks incoming remote print connections. Reduces attack surface by preventing remote users from sending print jobs to this machine. Default: Enabled. Recommended: Disabled for security.",
            Tags = ["printing", "remote", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableRemotePrinting", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableRemotePrinting"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableRemotePrinting", 1)],
        },
        new TweakDef
        {
            Id = "printing-disable-print-spooler",
            Label = "Disable Print Spooler Service",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Print Spooler service entirely (Start=4). Reduces attack surface on machines that do not use printers. Default: Automatic (2). Recommended: Disabled (4) if no printing needed.",
            Tags = ["printing", "spooler", "service", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Spooler"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Spooler", "Start", 4),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Spooler", "Start", 2),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Spooler", "Start", 4)],
        },
        new TweakDef
        {
            Id = "printing-restrict-driver-install",
            Label = "Restrict Printer Driver Installation",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Restricts printer driver installation to administrators only. Mitigates PrintNightmare-class vulnerabilities. Default: not restricted. Recommended: restricted.",
            Tags = ["printing", "driver", "security", "restriction"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "RestrictDriverInstallationToAdministrators", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "RestrictDriverInstallationToAdministrators"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "RestrictDriverInstallationToAdministrators", 1)],
        },
        new TweakDef
        {
            Id = "printing-print-disable-legacy-mode",
            Label = "Disable Print Spooler Legacy Mode",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables legacy default printer mode in the print spooler. Uses modern Windows-managed default printer instead. Default: Legacy. Recommended: Disabled (modern mode).",
            Tags = ["printing", "spooler", "legacy", "default-printer"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "LegacyDefaultPrinterMode", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "LegacyDefaultPrinterMode"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "LegacyDefaultPrinterMode", 0)],
        },
        new TweakDef
        {
            Id = "printing-print-default-paper-a4",
            Label = "Set Default Paper Size to A4",
            Category = "Printing",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets the default paper size to A4 (paper ID 9) for all printers. Default: Letter (US). Recommended: A4 outside North America.",
            Tags = ["printing", "paper", "a4", "international"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Windows"],
        },
        new TweakDef
        {
            Id = "printing-print-point-and-print-restrict",
            Label = "Enable Point and Print Restrictions",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Enables strict Point and Print restrictions: trusted servers only, no silent installs, UAC prompts on updates. Mitigates PrintNightmare. Default: unrestricted. Recommended: restricted.",
            Tags = ["printing", "point-and-print", "security", "printnightmare"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint", "Restricted", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint", "TrustedServers", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint", "InForest", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint", "NoWarningNoElevationOnInstall", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint", "UpdatePromptSettings", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint", "Restricted"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint", "TrustedServers"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint", "InForest"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint", "NoWarningNoElevationOnInstall"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint", "UpdatePromptSettings"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint", "Restricted", 1)],
        },
        new TweakDef
        {
            Id = "printing-copy-files-policy",
            Label = "Restrict Printer Driver Copy Files (CVE-2021-34527 Fix)",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Enables CopyFilesPolicy=1 to restrict printer driver copy file operations. Mitigates PrintNightmare attack vector CVE-2021-34527. Default: Unrestricted. Recommended: Restricted.",
            Tags = ["printing", "security", "printnightmare", "driver", "copy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers"],
        },
        new TweakDef
        {
            Id = "printing-emf-despooling",
            Label = "Enable EMF Direct Despooling",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables direct EMF despooling to bypass spooler for local printers. Can improve print speed for EMF print jobs. Default: Disabled. Recommended: Enabled for performance.",
            Tags = ["printing", "emf", "despooling", "performance", "local"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print"],
        },
        new TweakDef
        {
            Id = "printing-disable-client-side-map",
            Label = "Disable Client-Port Printer Mapping in RDP",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables client-side COM port mapping in RDP sessions. Prevents COM port printer redirection from RDP clients. Default: Enabled. Recommended: Disabled for security.",
            Tags = ["printing", "rdp", "com-port", "redirect", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services"],
        },
        new TweakDef
        {
            Id = "printing-disable-spooler-log",
            Label = "Disable Spooler Always-On Logging",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables LogAlways verbose spooler logging. Reduces log noise and disk writes from print subsystem. Default: Enabled. Recommended: Disabled.",
            Tags = ["printing", "logging", "spooler", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers"],
        },
        new TweakDef
        {
            Id = "printing-package-point-server-list",
            Label = "Restrict Package Point-and-Print to Server List",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Enables Package Point-and-Print server list restriction. Limits which servers can silently install printer drivers. Default: Unrestricted. Recommended: Restricted.",
            Tags = ["printing", "package", "point-and-print", "server", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers"],
        },
        new TweakDef
        {
            Id = "printing-disable-remote-inbound",
            Label = "Disable Remote Inbound Printing",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables accepting inbound print jobs from remote machines. Default: enabled.",
            Tags = ["printing", "remote", "inbound", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "RegisterSpoolerRemoteRpcEndPoint", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "RegisterSpoolerRemoteRpcEndPoint")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "RegisterSpoolerRemoteRpcEndPoint", 2)],
        },
        new TweakDef
        {
            Id = "printing-disable-driver-isolation",
            Label = "Enforce Printer Driver Isolation",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Forces printer drivers to load in isolated processes. Prevents a buggy driver from crashing the spooler. Default: per-driver setting.",
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
            Category = "Printing",
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
            Id = "printing-disable-web-printing",
            Label = "Disable Internet Printing (IPP)",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Internet Printing Protocol (IPP) support. Prevents printing via HTTP/HTTPS URLs. Default: enabled.",
            Tags = ["printing", "ipp", "internet", "web", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableHTTPPrinting", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableHTTPPrinting")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableHTTPPrinting", 1)],
        },
        new TweakDef
        {
            Id = "printing-disable-spooler-remote-access",
            Label = "Disable Print Spooler Remote Access",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Prevents the Print Spooler from accepting remote connections. Mitigates PrintNightmare-class vulnerabilities. Default: allowed.",
            Tags = ["printing", "spooler", "remote", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "RegisterSpoolerRemoteRpcEndPoint", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "RegisterSpoolerRemoteRpcEndPoint")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "RegisterSpoolerRemoteRpcEndPoint", 2)],
        },
    ];
}
