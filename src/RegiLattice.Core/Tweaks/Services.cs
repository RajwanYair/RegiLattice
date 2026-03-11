namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Services
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "svc-disable-diagtrack-service",
            Label = "Disable DiagTrack Service Startup",
            Category = "Services",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the DiagTrack and dmwappushservice services that collect and send diagnostic data to Microsoft.",
            Tags = ["services", "telemetry", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DiagTrack", @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\dmwappushservice"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DiagTrack", "Start", 4),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\dmwappushservice", "Start", 4),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DiagTrack", "Start", 2),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\dmwappushservice", "Start", 3),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DiagTrack", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-wsearch",
            Label = "Disable Windows Search Indexer",
            Category = "Services",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Windows Search indexer service — helpful on low-resource machines or when using Everything search.",
            Tags = ["services", "performance", "disk"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WSearch"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WSearch", "Start", 4),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WSearch", "Start", 2),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WSearch", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-wer",
            Label = "Disable Windows Error Reporting",
            Category = "Services",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows Error Reporting service and crash dump uploads.",
            Tags = ["services", "telemetry", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WerSvc", @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WerSvc", "Start", 4),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "Disabled", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WerSvc", "Start", 3),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "Disabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WerSvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-sysmain-service",
            Label = "Disable SysMain (Superfetch)",
            Category = "Services",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the SysMain (Superfetch) service — beneficial on SSD systems.",
            Tags = ["services", "performance", "ssd"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SysMain"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SysMain", "Start", 4),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SysMain", "Start", 2),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SysMain", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-diagsvc",
            Label = "Disable Diagnostic Service",
            Category = "Services",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Diagnostic Service (DiagSvc) that runs troubleshooters.",
            Tags = ["services", "telemetry", "diagnostics"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DiagSvc"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DiagSvc", "Start", 4),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DiagSvc", "Start", 3),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DiagSvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-wbiosrvc",
            Label = "Disable Biometric Service",
            Category = "Services",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows Biometric Service (WbioSrvc). Useful if fingerprint/face login is not used.",
            Tags = ["services", "biometric", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WbioSrvc"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WbioSrvc", "Start", 4),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WbioSrvc", "Start", 3),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WbioSrvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-remote-registry",
            Label = "Disable Remote Registry",
            Category = "Services",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Remote Registry service which allows remote access to the Windows registry. Security hardening measure.",
            Tags = ["services", "security", "remote"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\RemoteRegistry"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\RemoteRegistry", "Start", 4),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\RemoteRegistry", "Start", 3),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\RemoteRegistry", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-geolocation-service",
            Label = "Disable Geolocation Service",
            Category = "Services",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Windows Geolocation Service for privacy.",
            Tags = ["services", "privacy", "location"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\lfsvc"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\lfsvc", "Start", 4),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\lfsvc", "Start", 3),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\lfsvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-delivery-optimization-svc",
            Label = "Disable Delivery Optimization",
            Category = "Services",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Delivery Optimization service which shares Windows Update data with other PCs on LAN and internet.",
            Tags = ["services", "update", "bandwidth", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DoSvc"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DoSvc", "Start", 4),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DoSvc", "Start", 2),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DoSvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-alljoyn",
            Label = "Disable AllJoyn Router",
            Category = "Services",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the AllJoyn IoT router service — not needed by most users.",
            Tags = ["services", "iot", "cleanup"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\AJRouter"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\AJRouter", "Start", 4),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\AJRouter", "Start", 3),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\AJRouter", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-search-indexer",
            Label = "Disable Windows Search Indexer",
            Category = "Services",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Windows Search indexing service. Significantly reduces disk I/O and CPU usage. Use Everything Search as alternative. Default: Automatic (2). Recommended: Disabled (4).",
            Tags = ["services", "search", "indexer", "performance", "io"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WSearch"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WSearch", "Start", 4),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WSearch", "Start", 2),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WSearch", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-sysmain",
            Label = "Disable SysMain (Superfetch)",
            Category = "Services",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables SysMain (formerly Superfetch) preloading service. Reduces disk I/O on SSDs where preloading provides minimal benefit. Default: Automatic (2). Recommended: Disabled (4) for SSDs.",
            Tags = ["services", "sysmain", "superfetch", "performance", "ssd"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SysMain"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SysMain", "Start", 4),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SysMain", "Start", 2),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SysMain", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-print-spooler",
            Label = "Disable Print Spooler (Security Hardening)",
            Category = "Services",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Print Spooler to mitigate PrintNightmare vulnerabilities. Default: Automatic. Recommended: Disabled.",
            Tags = ["services", "spooler", "security", "printnightmare"],
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
            Id = "svc-disable-fax",
            Label = "Disable Fax Service (Cleanup)",
            Category = "Services",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the legacy Fax service to free resources. Default: Manual. Recommended: Disabled.",
            Tags = ["services", "fax", "legacy", "cleanup"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Fax"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Fax", "Start", 4),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Fax", "Start", 3),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Fax", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-biometric",
            Label = "Disable Windows Biometric Service",
            Category = "Services",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Windows Biometric Service (WbioSrvc) used for fingerprint and face recognition. Frees resources if biometrics are unused. Default: Manual. Recommended: Disabled.",
            Tags = ["services", "biometric", "wbiosrvc", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WbioSrvc"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WbioSrvc", "Start", 4),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WbioSrvc", "Start", 3),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WbioSrvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-smartcard",
            Label = "Disable Smart Card Service",
            Category = "Services",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Smart Card service (SCardSvr) for smart-card readers. Safe to disable if no smart cards are used. Default: Manual. Recommended: Disabled.",
            Tags = ["services", "smartcard", "scardsvr", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SCardSvr"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SCardSvr", "Start", 4),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SCardSvr", "Start", 3),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SCardSvr", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-geolocation",
            Label = "Disable Geolocation Service",
            Category = "Services",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Geolocation service (lfsvc) that tracks device location. Improves privacy. Default: Manual. Recommended: Disabled for desktops.",
            Tags = ["services", "geolocation", "lfsvc", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\lfsvc"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\lfsvc", "Start", 4),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\lfsvc", "Start", 3),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\lfsvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-cdpsvc",
            Label = "Disable Connected Devices Platform Service",
            Category = "Services",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Connected Devices Platform Service (CDPSvc) used for cross-device experiences. Frees resources if unused. Default: Automatic. Recommended: Disabled.",
            Tags = ["services", "cdpsvc", "cross-device", "platform"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\CDPSvc"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\CDPSvc", "Start", 4),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\CDPSvc", "Start", 2),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\CDPSvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-link-tracking",
            Label = "Disable Distributed Link Tracking Client",
            Category = "Services",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Distributed Link Tracking Client (TrkWks) that maintains NTFS file links across networked computers. Default: Manual. Recommended: Disabled for standalone PCs.",
            Tags = ["services", "link-tracking", "trkwks", "ntfs"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\TrkWks"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\TrkWks", "Start", 4),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\TrkWks", "Start", 3),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\TrkWks", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-wallet",
            Label = "Disable Wallet Service",
            Category = "Services",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Windows Wallet Service used for NFC-based payments. Safe to disable if contactless payments are unused. Default: Manual. Recommended: Disabled.",
            Tags = ["services", "wallet", "nfc", "payment"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WalletService"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WalletService", "Start", 4),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WalletService", "Start", 3),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WalletService", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-secondary-logon",
            Label = "Disable Secondary Logon Service",
            Category = "Services",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Secondary Logon (RunAs) service. Reduces privilege escalation surface. Default: manual.",
            Tags = ["services", "secondary-logon", "runas", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\seclogon"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\seclogon", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\seclogon", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\seclogon", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-xbox-live-networking",
            Label = "Disable Xbox Live Networking Service",
            Category = "Services",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Xbox Live Networking service. Not needed if you don't use Xbox features. Default: manual.",
            Tags = ["services", "xbox", "networking", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\XboxNetApiSvc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\XboxNetApiSvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\XboxNetApiSvc", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\XboxNetApiSvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-webclient",
            Label = "Disable WebClient (WebDAV) Service",
            Category = "Services",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the WebClient service (WebDAV). Reduces attack surface for NTLM relay. Default: manual.",
            Tags = ["services", "webclient", "webdav", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WebClient"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WebClient", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WebClient", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WebClient", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-phone-service",
            Label = "Disable Phone Service",
            Category = "Services",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Phone Service used by Phone Link and telephony APIs. Default: manual.",
            Tags = ["services", "phone", "telephony", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\PhoneSvc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\PhoneSvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\PhoneSvc", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\PhoneSvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "svc-disable-fax-service",
            Label = "Disable Fax Service",
            Category = "Services",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Windows Fax service. Reduces attack surface and saves resources. Default: manual start.",
            Tags = ["services", "fax", "disable", "legacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Fax"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Fax", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Fax", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Fax", "Start", 4)],
        },
    ];
}
