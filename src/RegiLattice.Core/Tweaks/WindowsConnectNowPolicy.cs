namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WindowsConnectNowPolicy
{
    private const string RegKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WCN\Registrars";
    private const string UiKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WCN\UI";
    private const string WcnKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WCN";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "wcnpol-disable-registrars",
            Label = "WCN Policy: Disable WCN Registrars",
            Category = "Windows Connect Now Policy",
            Description = "Disables all Windows Connect Now (WCN) registrar functionality via Group Policy. WCN allows wireless device configuration over the network — a potential attack vector on corporate Wi-Fi.",
            Tags = ["wcn", "wireless", "wifi", "registrar", "policy", "hardening"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [RegKey],
            ApplyOps = [RegOp.SetDword(RegKey, "EnableRegistrars", 0)],
            RemoveOps = [RegOp.DeleteValue(RegKey, "EnableRegistrars")],
            DetectOps = [RegOp.CheckDword(RegKey, "EnableRegistrars", 0)],
        },
        new TweakDef
        {
            Id = "wcnpol-disable-execution-service",
            Label = "WCN Policy: Disable WCN Execution Service",
            Category = "Windows Connect Now Policy",
            Description = "Prevents the WCN execution service from running through GPO. The WCN service manages network device discovery and configuration — disabling it reduces the attack surface on managed enterprise networks.",
            Tags = ["wcn", "service", "wireless", "policy", "hardening"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [RegKey],
            ApplyOps = [RegOp.SetDword(RegKey, "DisableWcnExecutionService", 1)],
            RemoveOps = [RegOp.DeleteValue(RegKey, "DisableWcnExecutionService")],
            DetectOps = [RegOp.CheckDword(RegKey, "DisableWcnExecutionService", 1)],
        },
        new TweakDef
        {
            Id = "wcnpol-disable-flash-config",
            Label = "WCN Policy: Disable Flash Config Provisioning",
            Category = "Windows Connect Now Policy",
            Description = "Disables the WCN Flash Config Registrar which allows device setup via USB-connected flash drives. Flash-based provisioning can be exploited to inject unauthorized wireless configurations.",
            Tags = ["wcn", "flash", "usb", "provisioning", "policy", "hardening"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [RegKey],
            ApplyOps = [RegOp.SetDword(RegKey, "DisableFlashConfigRegistrar", 1)],
            RemoveOps = [RegOp.DeleteValue(RegKey, "DisableFlashConfigRegistrar")],
            DetectOps = [RegOp.CheckDword(RegKey, "DisableFlashConfigRegistrar", 1)],
        },
        new TweakDef
        {
            Id = "wcnpol-disable-inband-80211",
            Label = "WCN Policy: Disable In-Band 802.11 Wireless Registrar",
            Category = "Windows Connect Now Policy",
            Description = "Disables the WCN in-band 802.11 wireless registrar, which enables over-the-air device configuration. Prevents unauthorized wireless setup requests from being processed by managed devices.",
            Tags = ["wcn", "802.11", "wifi", "wireless", "policy", "hardening"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [RegKey],
            ApplyOps = [RegOp.SetDword(RegKey, "DisableInBand802DOT11Registrar", 1)],
            RemoveOps = [RegOp.DeleteValue(RegKey, "DisableInBand802DOT11Registrar")],
            DetectOps = [RegOp.CheckDword(RegKey, "DisableInBand802DOT11Registrar", 1)],
        },
        new TweakDef
        {
            Id = "wcnpol-disable-upnp-registrar",
            Label = "WCN Policy: Disable UPnP-Based WCN Registrar",
            Category = "Windows Connect Now Policy",
            Description = "Disables the WCN UPnP registrar. WCN over UPnP can expose wireless credentials and configuration data to other devices on the local network without authentication.",
            Tags = ["wcn", "upnp", "wireless", "network", "policy", "hardening"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [RegKey],
            ApplyOps = [RegOp.SetDword(RegKey, "DisableUPnPRegistrar", 1)],
            RemoveOps = [RegOp.DeleteValue(RegKey, "DisableUPnPRegistrar")],
            DetectOps = [RegOp.CheckDword(RegKey, "DisableUPnPRegistrar", 1)],
        },
        new TweakDef
        {
            Id = "wcnpol-disable-ui",
            Label = "WCN Policy: Disable WCN User Interface",
            Category = "Windows Connect Now Policy",
            Description = "Hides the Windows Connect Now setup wizard from the Network and Sharing Center UI. Prevents end users from initiating WCN-based wireless device setup sessions on managed endpoints.",
            Tags = ["wcn", "ui", "wireless", "policy", "lockdown"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [UiKey],
            ApplyOps = [RegOp.SetDword(UiKey, "DisableWcnUi", 1)],
            RemoveOps = [RegOp.DeleteValue(UiKey, "DisableWcnUi")],
            DetectOps = [RegOp.CheckDword(UiKey, "DisableWcnUi", 1)],
        },
        new TweakDef
        {
            Id = "wcnpol-disable-auto-add",
            Label = "WCN Policy: Disable Automatic Device Add via WCN",
            Category = "Windows Connect Now Policy",
            Description = "Prevents automatic device addition through WCN by disabling the auto-add registrar. Stops devices from self-enrolling into the network through the WCN protocol without admin intervention.",
            Tags = ["wcn", "auto-add", "device", "network", "policy", "hardening"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [RegKey],
            ApplyOps = [RegOp.SetDword(RegKey, "AllowAutoAddRegistrar", 0)],
            RemoveOps = [RegOp.DeleteValue(RegKey, "AllowAutoAddRegistrar")],
            DetectOps = [RegOp.CheckDword(RegKey, "AllowAutoAddRegistrar", 0)],
        },
        new TweakDef
        {
            Id = "wcnpol-disable-wcn-global",
            Label = "WCN Policy: Globally Disable Windows Connect Now",
            Category = "Windows Connect Now Policy",
            Description = "Completely disables Windows Connect Now via the top-level GPO flag. Prevents any WCN-based operations including wireless device setup, UPnP registrar, and in-band 802.11 provisioning.",
            Tags = ["wcn", "disable", "wireless", "global", "policy", "hardening"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [WcnKey],
            ApplyOps = [RegOp.SetDword(WcnKey, "DisableWCN", 1)],
            RemoveOps = [RegOp.DeleteValue(WcnKey, "DisableWCN")],
            DetectOps = [RegOp.CheckDword(WcnKey, "DisableWCN", 1)],
        },
        new TweakDef
        {
            Id = "wcnpol-disable-pin-connect",
            Label = "WCN Policy: Disable PIN-Based WCN Device Connection",
            Category = "Windows Connect Now Policy",
            Description = "Blocks PIN-based Windows Connect Now device pairing. WCN PIN-based setup is vulnerable to brute-force PIN enumeration attacks (similar to WPS vulnerabilities on routers).",
            Tags = ["wcn", "pin", "pairing", "wireless", "policy", "hardening"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [WcnKey],
            ApplyOps = [RegOp.SetDword(WcnKey, "DisablePINConnect", 1)],
            RemoveOps = [RegOp.DeleteValue(WcnKey, "DisablePINConnect")],
            DetectOps = [RegOp.CheckDword(WcnKey, "DisablePINConnect", 1)],
        },
        new TweakDef
        {
            Id = "wcnpol-disable-push-button-connect",
            Label = "WCN Policy: Disable Push Button WCN Connection",
            Category = "Windows Connect Now Policy",
            Description = "Disables push-button connection method for Windows Connect Now. Physical push-button pairing can be exploited in unlocked or unattended environments to add unauthorized devices.",
            Tags = ["wcn", "push-button", "wps", "wireless", "policy", "hardening"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [WcnKey],
            ApplyOps = [RegOp.SetDword(WcnKey, "DisablePushButtonConnect", 1)],
            RemoveOps = [RegOp.DeleteValue(WcnKey, "DisablePushButtonConnect")],
            DetectOps = [RegOp.CheckDword(WcnKey, "DisablePushButtonConnect", 1)],
        },
    ];
}
