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
            Description =
                "Sets the Print Spooler service to Manual start. Reduces attack surface (PrintNightmare) and improves boot time if no printer is used.",
            Tags = ["printing", "spooler", "security", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Spooler"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Spooler", "Start", 3)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Spooler", "Start", 2)],
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
            Description =
                "Requires admin approval for Point-and-Print driver installs. Mitigates PrintNightmare and similar spooler vulnerabilities.",
            Tags = ["printing", "security", "pointandprint", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint",
                    "RestrictDriverInstallationToAdministrators",
                    1
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint",
                    "NoWarningNoElevationOnInstall",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint",
                    "RestrictDriverInstallationToAdministrators"
                ),
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint",
                    "NoWarningNoElevationOnInstall"
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
            Id = "printing-disable-xps-writer",
            Label = "Disable XPS Document Writer",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes the Microsoft XPS Document Writer virtual printer from the printers list. Reduces clutter if you never use XPS.",
            Tags = ["printing", "xps", "cleanup"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableXPSDocumentWriter", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableXPSDocumentWriter")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableXPSDocumentWriter", 1)],
        },
        new TweakDef
        {
            Id = "printing-disable-internet-printing",
            Label = "Disable Internet Printing (IPP)",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Internet Printing Protocol and Web PnP driver downloads. Reduces attack surface from internet-facing print services.",
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
            Category = "Printing",
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
            Category = "Printing",
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
            Id = "printing-disable-print-spooler",
            Label = "Disable Print Spooler Service",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Print Spooler service entirely (Start=4). Reduces attack surface on machines that do not use printers. Default: Automatic (2). Recommended: Disabled (4) if no printing needed.",
            Tags = ["printing", "spooler", "service", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Spooler"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Spooler", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Spooler", "Start", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Spooler", "Start", 4)],
        },
        new TweakDef
        {
            Id = "printing-restrict-driver-install",
            Label = "Restrict Printer Driver Installation",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Restricts printer driver installation to administrators only. Mitigates PrintNightmare-class vulnerabilities. Default: not restricted. Recommended: restricted.",
            Tags = ["printing", "driver", "security", "restriction"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers",
                    "RestrictDriverInstallationToAdministrators",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers",
                    "RestrictDriverInstallationToAdministrators"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers",
                    "RestrictDriverInstallationToAdministrators",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "printing-print-disable-legacy-mode",
            Label = "Disable Print Spooler Legacy Mode",
            Category = "Printing",
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
            Id = "printing-print-point-and-print-restrict",
            Label = "Enable Point and Print Restrictions",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Enables strict Point and Print restrictions: trusted servers only, no silent installs, UAC prompts on updates. Mitigates PrintNightmare. Default: unrestricted. Recommended: restricted.",
            Tags = ["printing", "point-and-print", "security", "printnightmare"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint", "Restricted", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint", "TrustedServers", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint", "InForest", 0),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint",
                    "NoWarningNoElevationOnInstall",
                    0
                ),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint", "UpdatePromptSettings", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint", "Restricted"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint", "TrustedServers"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint", "InForest"),
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint",
                    "NoWarningNoElevationOnInstall"
                ),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint", "UpdatePromptSettings"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint", "Restricted", 1)],
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
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "RegisterSpoolerRemoteRpcEndPoint"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "RegisterSpoolerRemoteRpcEndPoint", 2),
            ],
        },
        new TweakDef
        {
            Id = "printing-disable-driver-isolation",
            Label = "Enforce Printer Driver Isolation",
            Category = "Printing",
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
            Description =
                "Prevents the Print Spooler from accepting remote connections. Mitigates PrintNightmare-class vulnerabilities. Default: allowed.",
            Tags = ["printing", "spooler", "remote", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "RegisterSpoolerRemoteRpcEndPoint", 2)],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "RegisterSpoolerRemoteRpcEndPoint"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "RegisterSpoolerRemoteRpcEndPoint", 2),
            ],
        },
        new TweakDef
        {
            Id = "printing-copy-files-policy",
            Label = "Disable Point and Print Copy Files",
            Category = "Printing",
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
            Id = "printing-disable-client-side-map",
            Label = "Disable Client-Side Printer Mapping",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables automatic mapping of client printers in remote sessions. Reduces RDP session overhead. Default: enabled.",
            Tags = ["printing", "client-side", "mapping", "rdp"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableCpm", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableCpm")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableCpm", 1)],
        },
        new TweakDef
        {
            Id = "printing-disable-default-mgmt",
            Label = "Disable Windows Manage Default Printer",
            Category = "Printing",
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
            Id = "printing-disable-publishing",
            Label = "Disable Printer Publishing to AD",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents printers from being published to Active Directory. Reduces domain traffic from printer advertisements. Default: enabled.",
            Tags = ["printing", "publishing", "active-directory", "domain"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableHTTPPrinting", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableHTTPPrinting")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableHTTPPrinting", 1)],
        },
        new TweakDef
        {
            Id = "printing-disable-spooler-log",
            Label = "Disable Print Spooler Event Logging",
            Category = "Printing",
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
            Category = "Printing",
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
            Id = "printing-package-point-server-list",
            Label = "Restrict Package Point and Print Servers",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables the approved Package Point and Print server list. Only servers on the approved list can install print drivers via Point and Print. Default: unrestricted.",
            Tags = ["printing", "package", "point-and-print", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PackagePointAndPrint"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PackagePointAndPrint",
                    "PackagePointAndPrintOnly",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PackagePointAndPrint",
                    "PackagePointAndPrintOnly"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PackagePointAndPrint",
                    "PackagePointAndPrintOnly",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "printing-print-default-paper-a4",
            Label = "Set Default Paper Size to A4",
            Category = "Printing",
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
            Id = "printing-disable-ipp-web-client",
            Label = "Disable IPP Web Printing Client",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Internet Printing Protocol (IPP) client feature. Prevents connecting to web-hosted printers. Default: enabled.",
            Tags = ["printing", "internet", "ipp", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableWebPrinting", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableWebPrinting")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableWebPrinting", 1)],
        },
        new TweakDef
        {
            Id = "printing-disable-auto-default-printer",
            Label = "Disable Automatic Default Printer",
            Category = "Printing",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Stops Windows from automatically changing the default printer to the most recently used one. Default: Windows manages default printer.",
            Tags = ["printing", "default-printer"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows", "LegacyDefaultPrinterMode", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows", "LegacyDefaultPrinterMode", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows", "LegacyDefaultPrinterMode", 1)],
        },
        new TweakDef
        {
            Id = "printing-disable-driver-updates-via-wu",
            Label = "Block Printer Driver Downloads via Windows Update",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents Windows Update from automatically downloading printer drivers. Avoids unwanted driver changes. Default: auto-download enabled.",
            Tags = ["printing", "driver", "windows-update"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableHTTPPrinting", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableHTTPPrinting")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableHTTPPrinting", 1)],
        },
        new TweakDef
        {
            Id = "printing-disable-print-notifications",
            Label = "Disable Print Job Notifications",
            Category = "Printing",
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
            Category = "Printing",
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
            Id = "printing-disable-point-and-print",
            Label = "Restrict Point and Print Driver Installation",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Restricts Point and Print to approved servers only. Mitigates PrintNightmare-class vulnerabilities. Default: unrestricted.",
            Tags = ["printing", "security", "point-and-print", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint", "Restricted", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint", "Restricted", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint", "Restricted", 1)],
        },
        new TweakDef
        {
            Id = "printing-disable-print-workflow-svc",
            Label = "Disable Print Workflow Service",
            Category = "Printing",
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
            Category = "Printing",
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
            Category = "Printing",
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
            Id = "printing-disable-http-printing",
            Label = "Disable HTTP Printer Sharing",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables sharing printers over HTTP. Prevents the Print Spooler from accepting HTTP-based print jobs. Default: HTTP sharing permitted.",
            Tags = ["printing", "http", "network", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableHTTPPrinting", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableHTTPPrinting")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableHTTPPrinting", 1)],
        },
        new TweakDef
        {
            Id = "printing-disable-web-pnp",
            Label = "Disable Web-Based Printer Plug and Play",
            Category = "Printing",
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
            Id = "printing-restrict-driver-to-admins",
            Label = "Restrict Printer Driver Installation to Admins",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Requires administrator rights to install printer drivers. Prevents standard users from installing untrusted print drivers. Default: users can install drivers.",
            Tags = ["printing", "drivers", "security", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers",
                    "RestrictDriverInstallationToAdministrators",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers",
                    "RestrictDriverInstallationToAdministrators"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers",
                    "RestrictDriverInstallationToAdministrators",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "printing-block-kernel-mode-drivers",
            Label = "Block Kernel-Mode Printer Drivers",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Blocks kernel-mode printer drivers from running. Forces all print drivers to user mode (V4 drivers), reducing kernel attack surface. Default: kernel-mode drivers permitted.",
            Tags = ["printing", "drivers", "security", "kernel"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "KMPrintersAreBlocked", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "KMPrintersAreBlocked")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "KMPrintersAreBlocked", 1)],
        },
        new TweakDef
        {
            Id = "printing-disable-ipp-over-usb",
            Label = "Disable IPP over USB Printing",
            Category = "Printing",
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
            Category = "Printing",
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
            Category = "Printing",
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
            Category = "Printing",
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

// ── Merged from PrinterAdvanced.cs ──────────────────────────────────────────────────

internal static class PrinterAdvanced
{
    private const string WsdPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WSD";
    private const string PrintPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers";
    private const string SpoolerPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\Workflow";
    private const string IppPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers";
    private const string AuditPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\Security";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "prnta-disable-wsd-printer-discovery",
            Label = "Disable WSD (Web Services on Devices) Printer Discovery",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "wsd", "network", "discovery", "security"],
            Description =
                "Disables the WSD (Web Services on Devices) port monitor, preventing Windows from auto-discovering "
                + "network printers via SOAP/WSD. Reduces broadcast network noise and eliminates a legacy protocol attack surface.",
            ApplyOps = [RegOp.SetDword(WsdPol, "DisableWSDPrinting", 1)],
            RemoveOps = [RegOp.DeleteValue(WsdPol, "DisableWSDPrinting")],
            DetectOps = [RegOp.CheckDword(WsdPol, "DisableWSDPrinting", 1)],
        },
        new TweakDef
        {
            Id = "prnta-require-https-ipp-printing",
            Label = "Require HTTPS for Internet Printing Protocol (IPP)",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "ipp", "https", "security", "tls", "network"],
            Description =
                "Forces Windows to only accept IPP (Internet Printing Protocol) connections over HTTPS (port 443/631 TLS). "
                + "Prevents cleartext print job data from being transmitted over the network.",
            ApplyOps = [RegOp.SetDword(PrintPol, "ForceIPPSsl", 1)],
            RemoveOps = [RegOp.DeleteValue(PrintPol, "ForceIPPSsl")],
            DetectOps = [RegOp.CheckDword(PrintPol, "ForceIPPSsl", 1)],
        },
        new TweakDef
        {
            Id = "prnta-disable-print-workflow-service",
            Label = "Disable Print Workflow App Integration",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "workflow", "uwp", "policy", "attack surface"],
            Description =
                "Prevents third-party UWP applications from registering as print workflow apps. "
                + "Eliminates the risk of a malicious workflow app intercepting or modifying print jobs.",
            ApplyOps = [RegOp.SetDword(SpoolerPol, "DisablePrintWorkflow", 1)],
            RemoveOps = [RegOp.DeleteValue(SpoolerPol, "DisablePrintWorkflow")],
            DetectOps = [RegOp.CheckDword(SpoolerPol, "DisablePrintWorkflow", 1)],
        },
        new TweakDef
        {
            Id = "prnta-disable-wsd-ports",
            Label = "Disable WSD Port Monitor Installation",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "wsd", "port monitor", "security", "lateral movement"],
            Description =
                "Blocks installation of the WSD port monitor via Group Policy. "
                + "WSD ports have been used in lateral-movement scenarios; disabling the monitor prevents auto-creation.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WSD", "DisableWSDPortMonitor", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WSD", "DisableWSDPortMonitor")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WSD", "DisableWSDPortMonitor", 1)],
        },
        new TweakDef
        {
            Id = "prnta-enable-spooler-audit",
            Label = "Enable Print Spooler Service Audit Events",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "audit", "spooler", "logging", "security"],
            Description =
                "Enables detailed audit logging for the Print Spooler service in the Windows event log. "
                + "Required to detect PrintNightmare-style exploitation attempts and unauthorized driver installs.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Spooler", "AuditSpoolerEvents", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Spooler", "AuditSpoolerEvents")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Spooler", "AuditSpoolerEvents", 1)],
        },
        new TweakDef
        {
            Id = "prnta-disable-package-point-and-print",
            Label = "Disable Package Point-and-Print Non-Admin Install",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "security", "package point and print", "driver", "policy"],
            Description =
                "Restricts Package Point-and-Print to prevent non-admin users from installing packaged print drivers. "
                + "Closes a known elevation vector where a malicious print server could install arbitrary kernel drivers.",
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PackagePointAndPrint",
                    "PackagePointAndPrintOnly",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PackagePointAndPrint",
                    "PackagePointAndPrintOnly"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PackagePointAndPrint",
                    "PackagePointAndPrintOnly",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "prnta-restrict-printer-connection-unsigned",
            Label = "Disallow Connecting to Servers with Unsigned Printer Drivers",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "security", "unsigned", "driver", "policy"],
            Description =
                "Prevents Windows from connecting to a print server if the server's driver package is unsigned. "
                + "Ensures all automatically installed print drivers pass Windows Signature Enforcement (WHQL).",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisallowUserPrinterInstall", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisallowUserPrinterInstall")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisallowUserPrinterInstall", 1)],
        },
        new TweakDef
        {
            Id = "prnta-disable-banner-page",
            Label = "Disable Printer Banner/Separator Page",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "paper", "waste", "eco", "banner page"],
            Description =
                "Removes the separator cover page that some print servers insert before each print job. "
                + "Eliminates wasted paper and toner on shared printers in small-office environments.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "DisableBannerPage", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "DisableBannerPage")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "DisableBannerPage", 1)],
        },
        new TweakDef
        {
            Id = "prnta-disable-lpr-port",
            Label = "Disable LPR Port Monitor (Legacy Unix Printing)",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "lpr", "legacy", "security", "port monitor"],
            Description =
                "Disables the legacy LPR/LPD (Line Printer Remote) port monitor via Group Policy. "
                + "LPR is an unencrypted 1980s printing protocol; disabling it eliminates a legacy attack surface.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableLPRPort", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableLPRPort")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableLPRPort", 1)],
        },
        new TweakDef
        {
            Id = "prnta-require-https-spooler",
            Label = "Require HTTPS for Print Spooler Remote Connections",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "spooler", "https", "tls", "security"],
            Description =
                "Enforces TLS-encrypted HTTPS for all inbound remote print spooler "
                + "connections, blocking unencrypted (HTTP) print job submissions "
                + "across the network. Requires the spooler to present a valid certificate.",
            ApplyOps = [RegOp.SetDword(PrintPol, "EnableTLSForHTTPSPrinting", 1)],
            RemoveOps = [RegOp.DeleteValue(PrintPol, "EnableTLSForHTTPSPrinting")],
            DetectOps = [RegOp.CheckDword(PrintPol, "EnableTLSForHTTPSPrinting", 1)],
        },
        new TweakDef
        {
            Id = "prnta-restrict-driver-install-admin",
            Label = "Restrict Print Driver Installation to Administrators",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "driver", "admin", "security", "privilege escalation"],
            Description =
                "Prevents standard users from installing print drivers. "
                + "Unvetted print drivers run in kernel space and are a documented "
                + "privilege-escalation vector (PrintNightmare/CVE-2021-34527 family).",
            ApplyOps = [RegOp.SetDword(PrintPol, "RestrictDriverInstallationToAdministrators", 1)],
            RemoveOps = [RegOp.DeleteValue(PrintPol, "RestrictDriverInstallationToAdministrators")],
            DetectOps = [RegOp.CheckDword(PrintPol, "RestrictDriverInstallationToAdministrators", 1)],
        },
        new TweakDef
        {
            Id = "prnta-disable-cloud-print",
            Label = "Disable Microsoft Cloud Print (Print to Cloud)",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "cloud print", "microsoft", "privacy"],
            Description =
                "Blocks the Microsoft Cloud Print service (formerly Mopria) from "
                + "enumerating and uploading spool data to Microsoft cloud endpoints. "
                + "Keeps all print jobs local.",
            ApplyOps = [RegOp.SetDword(PrintPol, "DisablePrinterCloudPrint", 1)],
            RemoveOps = [RegOp.DeleteValue(PrintPol, "DisablePrinterCloudPrint")],
            DetectOps = [RegOp.CheckDword(PrintPol, "DisablePrinterCloudPrint", 1)],
        },
        new TweakDef
        {
            Id = "prnta-disable-wsd-multicast-discovery",
            Label = "Disable WSD Multicast Printer Discovery",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "wsd", "discovery", "multicast", "network"],
            Description =
                "Prevents the Web Services on Devices (WSD) listener from responding "
                + "to multicast discovery probes on the local subnet. "
                + "Reduces unsolicited network chatter and removes WSD as an attack surface.",
            ApplyOps = [RegOp.SetDword(WsdPol, "DisableDiscovery", 1)],
            RemoveOps = [RegOp.DeleteValue(WsdPol, "DisableDiscovery")],
            DetectOps = [RegOp.CheckDword(WsdPol, "DisableDiscovery", 1)],
        },
        new TweakDef
        {
            Id = "prnta-disable-win32-spool-com",
            Label = "Disable Windows 32-Bit Spooler COM Object",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "spooler", "com", "security", "legacy"],
            Description =
                "Disables the legacy 32-bit in-process COM spooler extensions. "
                + "These extensions can be abused to load arbitrary DLLs into the print spooler process "
                + "under SYSTEM context — a known persistence vector.",
            ApplyOps = [RegOp.SetDword(PrintPol, "DisableWebPnpDownload", 1)],
            RemoveOps = [RegOp.DeleteValue(PrintPol, "DisableWebPnpDownload")],
            DetectOps = [RegOp.CheckDword(PrintPol, "DisableWebPnpDownload", 1)],
        },
        new TweakDef
        {
            Id = "prnta-disable-rpc-over-http-spooler",
            Label = "Disable RPC-over-HTTP for Spooler (Restrict to Named Pipes)",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "rpc", "http", "spooler", "network", "security"],
            Description =
                "Restricts inbound RPC connections to the print spooler to named "
                + "pipes only, blocking RPC-over-HTTP transport. Reduces remote "
                + "exploit surface for CVE-2021-1675 / PrintNightmare variants.",
            ApplyOps = [RegOp.SetDword(PrintPol, "DisableHTTPPrinting", 1)],
            RemoveOps = [RegOp.DeleteValue(PrintPol, "DisableHTTPPrinting")],
            DetectOps = [RegOp.CheckDword(PrintPol, "DisableHTTPPrinting", 1)],
        },
        new TweakDef
        {
            Id = "prnta-disable-internet-print-client",
            Label = "Disable Internet Printing Client",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "internet printing", "ipp", "feature", "security"],
            Description =
                "Disables the Windows Internet Printing Client component "
                + "(connects to HTTP/HTTPS printers by URL). Closes an infrequently "
                + "used remote printing feature that can be abused for SSRF and "
                + "credential-relay attacks.",
            ApplyOps = [RegOp.SetDword(PrintPol, "DisableHTTPPrintingClient", 1)],
            RemoveOps = [RegOp.DeleteValue(PrintPol, "DisableHTTPPrintingClient")],
            DetectOps = [RegOp.CheckDword(PrintPol, "DisableHTTPPrintingClient", 1)],
        },
        new TweakDef
        {
            Id = "prnta-disable-wsd-printer-announce",
            Label = "Disable WSD Printer Announce (Host Advertising)",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "wsd", "announcement", "network", "privacy"],
            Description =
                "Stops Windows from broadcasting WSD printer-announcement packets "
                + "on the network (Hello/Bye messages). Prevents other hosts on "
                + "the subnet from seeing shared printers.",
            ApplyOps = [RegOp.SetDword(WsdPol, "DisableAnnouncement", 1)],
            RemoveOps = [RegOp.DeleteValue(WsdPol, "DisableAnnouncement")],
            DetectOps = [RegOp.CheckDword(WsdPol, "DisableAnnouncement", 1)],
        },
        new TweakDef
        {
            Id = "prnta-disable-driver-update-from-wu",
            Label = "Block Automatic Print Driver Updates via Windows Update",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "driver", "windows update", "wu", "policy"],
            Description =
                "Prevents Windows Update from automatically pushing print driver "
                + "updates. Automatic driver installs have been weaponised by "
                + "PrintNightmare-class vulnerabilities; use WSUS or manual approval instead.",
            ApplyOps = [RegOp.SetDword(PrintPol, "DisableAutoInstallDriverViaPnP", 1)],
            RemoveOps = [RegOp.DeleteValue(PrintPol, "DisableAutoInstallDriverViaPnP")],
            DetectOps = [RegOp.CheckDword(PrintPol, "DisableAutoInstallDriverViaPnP", 1)],
        },
        new TweakDef
        {
            Id = "prnta-disable-inbound-print-spooler",
            Label = "Disable Inbound Remote Print Connections (Spooler Server)",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = false,
            Tags = ["printing", "spooler", "remote", "network", "security"],
            Description =
                "Blocks inbound remote connections to the Windows print spooler "
                + "service. Workstations should not accept remote print jobs; "
                + "this policy closes the highest-impact PrintNightmare attack path "
                + "without disabling the spooler entirely (local printing still works).",
            ApplyOps = [RegOp.SetDword(PrintPol, "NoAddPrinter", 0), RegOp.SetDword(PrintPol, "DisableSpoolerRemote", 1)],
            RemoveOps = [RegOp.DeleteValue(PrintPol, "DisableSpoolerRemote"), RegOp.DeleteValue(PrintPol, "NoAddPrinter")],
            DetectOps = [RegOp.CheckDword(PrintPol, "DisableSpoolerRemote", 1)],
        },
    ];
}
