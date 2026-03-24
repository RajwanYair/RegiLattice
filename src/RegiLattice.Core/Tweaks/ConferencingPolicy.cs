namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// Windows Meeting Space / NetMeeting Conference Room Pilot — Conferencing policy.
// Controls the Microsoft Conferencing infrastructure (meeting invitations, peer connections,
// bandwidth limits, and document sharing workspace restrictions).
// HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Conferencing

internal static class ConferencingPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Conferencing";
    private const string InvKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Conferencing\Invitations";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "confer-disable-meeting-space",
            Label = "Conferencing Policy: Disable Windows Meeting Space",
            Category = "Windows Conferencing Policy",
            Description = "Disables Windows Meeting Space (the Vista/7 peer-to-peer collaboration platform). Meeting Space connects devices via ad-hoc Wi-Fi or Bluetooth without authentication requirements. Disabling it removes a legacy unmanaged collaboration channel from the endpoint.",
            Tags = ["conferencing", "meeting-space", "p2p", "legacy", "disable", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableMeetingSpace", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableMeetingSpace")],
            DetectOps = [RegOp.CheckDword(Key, "DisableMeetingSpace", 1)],
        },
        new TweakDef
        {
            Id = "confer-disable-peer-invitations",
            Label = "Conferencing Policy: Disable Peer-to-Peer Meeting Invitations",
            Category = "Windows Conferencing Policy",
            Description = "Prevents users from sending or receiving Windows Meeting Space invitations. Ad-hoc peer invitations in Windows Conferencing use People Near Me (PNRP) which broadcasts user presence on the local network without per-session authentication.",
            Tags = ["conferencing", "invitations", "p2p", "pnrp", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [InvKey],
            ApplyOps = [RegOp.SetDword(InvKey, "NoInvitations", 1)],
            RemoveOps = [RegOp.DeleteValue(InvKey, "NoInvitations")],
            DetectOps = [RegOp.CheckDword(InvKey, "NoInvitations", 1)],
        },
        new TweakDef
        {
            Id = "confer-disable-session-hosting",
            Label = "Conferencing Policy: Disable Session Hosting for Collaborations",
            Category = "Windows Conferencing Policy",
            Description = "Prevents the local machine from acting as a host for Windows Meeting Space sessions. Disabling hosting prevents the machine from accepting incoming PNRP peer connections that are used to establish collaboration sessions without requiring inbound firewall rules.",
            Tags = ["conferencing", "host", "pnrp", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "NoHosting", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "NoHosting")],
            DetectOps = [RegOp.CheckDword(Key, "NoHosting", 1)],
        },
        new TweakDef
        {
            Id = "confer-disable-remote-app-sharing",
            Label = "Conferencing Policy: Disable Remote Application Sharing in Conferences",
            Category = "Windows Conferencing Policy",
            Description = "Disables the remote application sharing capability within Windows conferencing sessions. Application sharing transmits screen content of individual windows to all session participants without per-participant audit logging.",
            Tags = ["conferencing", "app-sharing", "remote", "screen", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "NoRemoteAppSharing", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "NoRemoteAppSharing")],
            DetectOps = [RegOp.CheckDword(Key, "NoRemoteAppSharing", 1)],
        },
        new TweakDef
        {
            Id = "confer-disable-document-handouts",
            Label = "Conferencing Policy: Disable Document Handouts in Conferences",
            Category = "Windows Conferencing Policy",
            Description = "Prevents Windows Meeting Space participants from distributing document handouts to other session members. Document handout distribution bypasses DLP controls because files are transferred over the PNRP peer channel rather than email or SharePoint.",
            Tags = ["conferencing", "documents", "handouts", "dlp", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "NoDocumentHandouts", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "NoDocumentHandouts")],
            DetectOps = [RegOp.CheckDword(Key, "NoDocumentHandouts", 1)],
        },
        new TweakDef
        {
            Id = "confer-block-bandwidth-unlimited",
            Label = "Conferencing Policy: Enforce Maximum Bandwidth Limit for Sessions",
            Category = "Windows Conferencing Policy",
            Description = "Enforces a bandwidth ceiling on Windows Conferencing sessions. Without a policy limit, conferencing sessions can saturate available network bandwidth affecting all other services sharing the network segment.",
            Tags = ["conferencing", "bandwidth", "limit", "network", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "MaxBandwidthKbps", 512)],
            RemoveOps = [RegOp.DeleteValue(Key, "MaxBandwidthKbps")],
            DetectOps = [RegOp.CheckDword(Key, "MaxBandwidthKbps", 512)],
        },
        new TweakDef
        {
            Id = "confer-disable-direct-p2p-connect",
            Label = "Conferencing Policy: Block Direct Peer-to-Peer Connections",
            Category = "Windows Conferencing Policy",
            Description = "Forces Windows Conferencing to route all traffic through a relay server instead of establishing direct peer-to-peer connections. Direct P2P connections bypass network egress monitoring and expose internal IP addressing information to remote participants.",
            Tags = ["conferencing", "p2p", "relay", "network-monitoring", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "NoDirectP2PConnections", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "NoDirectP2PConnections")],
            DetectOps = [RegOp.CheckDword(Key, "NoDirectP2PConnections", 1)],
        },
        new TweakDef
        {
            Id = "confer-disable-people-near-me",
            Label = "Conferencing Policy: Disable People Near Me / PNRP Discovery",
            Category = "Windows Conferencing Policy",
            Description = "Prevents the machine from broadcasting its presence to other machines on the local network via the People Near Me (PNRP) service used by Windows Conferencing. PNRP presence broadcasts reveal device names, user accounts, and network position to all devices on the subnet.",
            Tags = ["conferencing", "pnrp", "people-near-me", "discovery", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "NoPeopleNearMe", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "NoPeopleNearMe")],
            DetectOps = [RegOp.CheckDword(Key, "NoPeopleNearMe", 1)],
        },
        new TweakDef
        {
            Id = "confer-disable-meeting-autostart",
            Label = "Conferencing Policy: Disable Windows Meeting Space Autostart",
            Category = "Windows Conferencing Policy",
            Description = "Prevents Windows Meeting Space from automatically starting during user logon or when other conferencing-related events are triggered (such as projector connection). Autostart increases the attack surface by leaving the PNRP service active even when the user is not actively collaborating.",
            Tags = ["conferencing", "autostart", "startup", "pnrp", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableMeetingAutoStart", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableMeetingAutoStart")],
            DetectOps = [RegOp.CheckDword(Key, "DisableMeetingAutoStart", 1)],
        },
        new TweakDef
        {
            Id = "confer-disable-remember-passwords",
            Label = "Conferencing Policy: Disable Password Storage for Meeting Rooms",
            Category = "Windows Conferencing Policy",
            Description = "Prevents Windows Conferencing from storing meeting room passwords in the credential manager or conference history. Cached meeting passwords can be extracted from the Windows credential store, allowing replay attacks against password-protected legacy meeting rooms.",
            Tags = ["conferencing", "password", "credential-manager", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableRememberPasswords", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableRememberPasswords")],
            DetectOps = [RegOp.CheckDword(Key, "DisableRememberPasswords", 1)],
        },
    ];
}
