namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

[TweakModule]
internal static class PolicyWindowsRAHardening
{
    private const string RaKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemoteAssistance";

    private const string RaUnsolicit = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services\RAUnsolicit";

    private const string SidebarKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Sidebar";

    private const string EnhStorKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EnhancedStorageDevices";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        // ── Remote Assistance Security ───────────────────────────────────────────
        new TweakDef
        {
            Id = "raharden-block-solicited",
            Label = "Remote Assistance: Block Solicited RA (RemoteAssistance Policy Key)",
            Category = "Remote Desktop — Remote Assistance",
            Description =
                "Sets fAllowToGetHelp=0 under the Windows\\RemoteAssistance Group Policy key. "
                + "Prevents users from sending Remote Assistance invitations to helpers. "
                + "Complements the Terminal Services policy (rast-disable-solicited-gpo) on a separate registry path.",
            Tags = ["remote-assistance", "gpo", "security", "unsolicited"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Solicited Remote Assistance requests blocked from the Windows\\RemoteAssistance policy key.",
            ApplyOps = [RegOp.SetDword(RaKey, "fAllowToGetHelp", 0)],
            RemoveOps = [RegOp.DeleteValue(RaKey, "fAllowToGetHelp")],
            DetectOps = [RegOp.CheckDword(RaKey, "fAllowToGetHelp", 0)],
        },
        new TweakDef
        {
            Id = "raharden-view-only-ra",
            Label = "Remote Assistance: Restrict Helper to View-Only (RemoteAssistance Key)",
            Category = "Remote Desktop — Remote Assistance",
            Description =
                "Sets fAllowFullControl=0 under the Windows\\RemoteAssistance Group Policy key. "
                + "Helpers can view the screen but cannot control the keyboard or mouse. "
                + "Applies the view-only restriction at the RemoteAssistance policy sub-key specifically.",
            Tags = ["remote-assistance", "view-only", "security", "gpo"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "RA helper restricted to screen-view only; interactive control disabled.",
            ApplyOps = [RegOp.SetDword(RaKey, "fAllowFullControl", 0)],
            RemoveOps = [RegOp.DeleteValue(RaKey, "fAllowFullControl")],
            DetectOps = [RegOp.CheckDword(RaKey, "fAllowFullControl", 0)],
        },
        new TweakDef
        {
            Id = "raharden-encrypt-ra-tickets",
            Label = "Remote Assistance: Require Encrypted Invitation Tickets (RAUnsolicit)",
            Category = "Remote Desktop — Remote Assistance",
            Description =
                "Forces Remote Assistance invitation tickets in the RAUnsolicit key to use encryption. "
                + "Plain-text (unencrypted) Offer RA tickets are rejected, preventing interception. "
                + "An attacker who captures an unencrypted ticket file cannot replay it to gain access.",
            Tags = ["remote-assistance", "encryption", "ticket", "hardening"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Unencrypted RA tickets rejected in the Offer RA (RAUnsolicit) policy context.",
            ApplyOps = [RegOp.SetDword(RaUnsolicit, "CreateEncryptedOnlyTickets", 1)],
            RemoveOps = [RegOp.DeleteValue(RaUnsolicit, "CreateEncryptedOnlyTickets")],
            DetectOps = [RegOp.CheckDword(RaUnsolicit, "CreateEncryptedOnlyTickets", 1)],
        },
        new TweakDef
        {
            Id = "raharden-disable-pnrp-easy-connect",
            Label = "Remote Assistance: Disable PNRP Easy Connect Relay",
            Category = "Remote Desktop — Remote Assistance",
            Description =
                "Sets fEnableEasilyConnect=0 under the Windows\\RemoteAssistance policy key. "
                + "Disables the Peer Name Resolution Protocol (PNRP) relay used for Easy Connect RA. "
                + "Distinct from fAllowEasyConnect — this controls the underlying PNRP relay mechanism.",
            Tags = ["remote-assistance", "pnrp", "easy-connect", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "PNRP relay for passwordless Easy Connect disabled; invitation-file method remains available.",
            ApplyOps = [RegOp.SetDword(RaKey, "fEnableEasilyConnect", 0)],
            RemoveOps = [RegOp.DeleteValue(RaKey, "fEnableEasilyConnect")],
            DetectOps = [RegOp.CheckDword(RaKey, "fEnableEasilyConnect", 0)],
        },
        // ── Windows Sidebar / Gadgets ────────────────────────────────────────────
        new TweakDef
        {
            Id = "sidebar-disable-gadget-download",
            Label = "Block Gadget Download from Gallery",
            Category = "Security — Legacy Features Policy",
            Description =
                "Sets DownloadOnline=0 under the Windows\\Sidebar Group Policy key. "
                + "Prevents users from downloading new Gadgets from the online gallery. "
                + "Third-party Gadgets are an attack surface: they run with user privileges and can be exploited by "
                + "malicious gadget packages distributed via external gallery sites.",
            Tags = ["sidebar", "gadgets", "download", "gpo", "legacy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Online Gadget gallery download blocked; existing installed Gadgets unaffected.",
            ApplyOps = [RegOp.SetDword(SidebarKey, "DownloadOnline", 0)],
            RemoveOps = [RegOp.DeleteValue(SidebarKey, "DownloadOnline")],
            DetectOps = [RegOp.CheckDword(SidebarKey, "DownloadOnline", 0)],
        },
        new TweakDef
        {
            Id = "sidebar-block-user-gadgets",
            Label = "Prevent Users from Installing or Running Desktop Gadgets",
            Category = "Security — Legacy Features Policy",
            Description =
                "Sets TurnOffUserInstalledSidebar=1 under the Windows\\Sidebar Group Policy key. "
                + "Blocks standard users from installing third-party Gadgets obtained from external sources. "
                + "Reduces exposure to malicious or exploitable Gadget packages without disabling system Gadgets completely.",
            Tags = ["sidebar", "gadgets", "user-restriction", "gpo", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Users cannot install or run third-party Gadgets; system Gadgets may still be accessible.",
            ApplyOps = [RegOp.SetDword(SidebarKey, "TurnOffUserInstalledSidebar", 1)],
            RemoveOps = [RegOp.DeleteValue(SidebarKey, "TurnOffUserInstalledSidebar")],
            DetectOps = [RegOp.CheckDword(SidebarKey, "TurnOffUserInstalledSidebar", 1)],
        },
        // ── Enhanced Storage Devices (IEEE 1667) ─────────────────────────────────
        new TweakDef
        {
            Id = "estg-block-non-enhanced-storage",
            Label = "Require IEEE 1667 Enhanced Storage for Removable Devices",
            Category = "Security — Legacy Features Policy",
            Description =
                "Sets DisallowLegacyDiskDevices=1 under the Windows\\EnhancedStorageDevices policy key. "
                + "Allows only IEEE 1667 Enhanced Storage compliant removable devices; plain USB mass storage is blocked. "
                + "Enforces hardware authentication for all removable storage in security-sensitive environments.",
            Tags = ["enhanced-storage", "ieee-1667", "removable-storage", "gpo", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Non-IEEE-1667 USB drives blocked; only hardware-authenticated Enhanced Storage allowed.",
            ApplyOps = [RegOp.SetDword(EnhStorKey, "DisallowLegacyDiskDevices", 1)],
            RemoveOps = [RegOp.DeleteValue(EnhStorKey, "DisallowLegacyDiskDevices")],
            DetectOps = [RegOp.CheckDword(EnhStorKey, "DisallowLegacyDiskDevices", 1)],
        },
        new TweakDef
        {
            Id = "estg-lock-on-machine-lock",
            Label = "Lock Enhanced Storage Device When Machine Is Locked",
            Category = "Security — Legacy Features Policy",
            Description =
                "Sets LockDeviceOnMachineUnlock=1 under the Windows\\EnhancedStorageDevices policy key. "
                + "IEEE 1667 Enhanced Storage devices are locked automatically when the machine screen locks. "
                + "Data on the Enhanced Storage device cannot be accessed while the workstation is unattended.",
            Tags = ["enhanced-storage", "ieee-1667", "screen-lock", "security", "gpo"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Enhanced Storage auto-locks with workstation; protects data if machine is left unattended.",
            ApplyOps = [RegOp.SetDword(EnhStorKey, "LockDeviceOnMachineUnlock", 1)],
            RemoveOps = [RegOp.DeleteValue(EnhStorKey, "LockDeviceOnMachineUnlock")],
            DetectOps = [RegOp.CheckDword(EnhStorKey, "LockDeviceOnMachineUnlock", 1)],
        },
        new TweakDef
        {
            Id = "estg-require-usb-root-hub",
            Label = "Restrict Enhanced Storage to USB Root Hub Connections",
            Category = "Security — Legacy Features Policy",
            Description =
                "Sets RootHubConnectedEnhancedStorageDevices=1 under the Windows\\EnhancedStorageDevices policy key. "
                + "Permits only IEEE 1667 Enhanced Storage devices connected directly to a USB root hub port. "
                + "Devices plugged into USB hubs or extension cables are rejected, preventing hub-bypass attacks.",
            Tags = ["enhanced-storage", "ieee-1667", "usb-root-hub", "gpo", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Enhanced Storage only allowed on direct root-hub USB ports; hub/extender connections blocked.",
            ApplyOps = [RegOp.SetDword(EnhStorKey, "RootHubConnectedEnhancedStorageDevices", 1)],
            RemoveOps = [RegOp.DeleteValue(EnhStorKey, "RootHubConnectedEnhancedStorageDevices")],
            DetectOps = [RegOp.CheckDword(EnhStorKey, "RootHubConnectedEnhancedStorageDevices", 1)],
        },
        new TweakDef
        {
            Id = "estg-block-bus-powered-devices",
            Label = "Block Bus-Powered IEEE 1667 Enhanced Storage Devices",
            Category = "Security — Legacy Features Policy",
            Description =
                "Sets DisallowUSBConnectivity=1 under the Windows\\EnhancedStorageDevices policy key. "
                + "Prevents bus-powered (no external power) IEEE 1667 Enhanced Storage devices from connecting. "
                + "Bus-powered devices lack the power budget required for proper secure-authentication challenges.",
            Tags = ["enhanced-storage", "ieee-1667", "bus-powered", "gpo", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Bus-powered Enhanced Storage devices blocked; only externally powered ones allowed.",
            ApplyOps = [RegOp.SetDword(EnhStorKey, "DisallowUSBConnectivity", 1)],
            RemoveOps = [RegOp.DeleteValue(EnhStorKey, "DisallowUSBConnectivity")],
            DetectOps = [RegOp.CheckDword(EnhStorKey, "DisallowUSBConnectivity", 1)],
        },
    ];
}
