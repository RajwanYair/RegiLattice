#nullable enable
using System.Collections.Generic;
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

// Remote Assistance — GPO policy and runtime control settings.
// HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services
// HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services\RAUnsolicit
// HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Remote Assistance
internal static class RemoteAssistancePolicy
{
    private const string RaPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services";
    private const string RaOffered = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services\RAUnsolicit";
    private const string RaRuntime = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Remote Assistance";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "rast-disable-solicited-gpo",
            Label = "Remote Assistance: Block Solicited RA via GPO",
            Category = "Remote Assistance Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [RaPolicy],
            Tags = ["remote-assistance", "gpo", "policy", "security", "network"],
            Description =
                "Sets fAllowToGetHelp=0 in Terminal Services policy. Prevents users from sending Remote Assistance "
                + "invitations. Enforced as a Group Policy that overrides the per-user Windows Security setting. "
                + "Default: enabled. Recommended: disable on hardened systems.",
            ApplyOps = [RegOp.SetDword(RaPolicy, "fAllowToGetHelp", 0)],
            RemoveOps = [RegOp.DeleteValue(RaPolicy, "fAllowToGetHelp")],
            DetectOps = [RegOp.CheckDword(RaPolicy, "fAllowToGetHelp", 0)],
        },
        new TweakDef
        {
            Id = "rast-view-only-gpo",
            Label = "Remote Assistance: Restrict to View-Only via GPO",
            Category = "Remote Assistance Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [RaPolicy],
            Tags = ["remote-assistance", "gpo", "view-only", "policy", "security"],
            Description =
                "Sets fAllowFullControl=0 in Terminal Services policy. Helpers can watch the screen but cannot "
                + "operate the keyboard or mouse during a Remote Assistance session. "
                + "Default: full control allowed. Recommended: view-only for minimum-privilege RA sessions.",
            ApplyOps = [RegOp.SetDword(RaPolicy, "fAllowFullControl", 0)],
            RemoveOps = [RegOp.DeleteValue(RaPolicy, "fAllowFullControl")],
            DetectOps = [RegOp.CheckDword(RaPolicy, "fAllowFullControl", 0)],
        },
        new TweakDef
        {
            Id = "rast-limit-ticket-6hr-gpo",
            Label = "Remote Assistance: Limit Invitation Lifetime to 6 Hours (GPO)",
            Category = "Remote Assistance Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [RaPolicy],
            Tags = ["remote-assistance", "gpo", "ticket", "expiry", "policy"],
            Description =
                "Sets MaxTicketExpiry=6 and MaxTicketExpiryUnits=1 (hours) in Terminal Services policy. "
                + "Limits the window during which a stolen or forwarded RA invitation file can be used. "
                + "Default: no strict policy limit. Recommended: 1–6 hours.",
            ApplyOps = [RegOp.SetDword(RaPolicy, "MaxTicketExpiry", 6), RegOp.SetDword(RaPolicy, "MaxTicketExpiryUnits", 1)],
            RemoveOps = [RegOp.DeleteValue(RaPolicy, "MaxTicketExpiry"), RegOp.DeleteValue(RaPolicy, "MaxTicketExpiryUnits")],
            DetectOps = [RegOp.CheckDword(RaPolicy, "MaxTicketExpiry", 6)],
        },
        new TweakDef
        {
            Id = "rast-disable-offer-gpo",
            Label = "Remote Assistance: Block Unsolicited (Offer) RA via GPO",
            Category = "Remote Assistance Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [RaOffered],
            Tags = ["remote-assistance", "gpo", "offer", "unsolicited", "security"],
            Description =
                "Sets fAllowUnsolicited=0 in the RAUnsolicit policy key. Prevents IT admins from initiating "
                + "Remote Assistance connections without an end-user invitation (Offer RA / DCOM RA). "
                + "Default: Offer RA allowed. Recommended: disable unless actively used by IT.",
            ApplyOps = [RegOp.SetDword(RaOffered, "fAllowUnsolicited", 0)],
            RemoveOps = [RegOp.DeleteValue(RaOffered, "fAllowUnsolicited")],
            DetectOps = [RegOp.CheckDword(RaOffered, "fAllowUnsolicited", 0)],
        },
        new TweakDef
        {
            Id = "rast-limit-offer-ticket-1hr",
            Label = "Remote Assistance: Limit Offer RA Token Expiry to 1 Hour",
            Category = "Remote Assistance Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [RaOffered],
            Tags = ["remote-assistance", "offer", "ticket", "expiry", "gpo"],
            Description =
                "Sets MaxTicketExpiry=1 and MaxTicketExpiryUnits=1 in the RAUnsolicit key. "
                + "Offer RA tokens granted by IT expire after 1 hour. Default: no strict limit. "
                + "Reduces risk of stale credentials being replayed for extended access.",
            ApplyOps = [RegOp.SetDword(RaOffered, "MaxTicketExpiry", 1), RegOp.SetDword(RaOffered, "MaxTicketExpiryUnits", 1)],
            RemoveOps = [RegOp.DeleteValue(RaOffered, "MaxTicketExpiry"), RegOp.DeleteValue(RaOffered, "MaxTicketExpiryUnits")],
            DetectOps = [RegOp.CheckDword(RaOffered, "MaxTicketExpiry", 1)],
        },
        new TweakDef
        {
            Id = "rast-disable-runtime",
            Label = "Remote Assistance: Disable via Runtime Control Key",
            Category = "Remote Assistance Policy",
            NeedsAdmin = true,
            CorpSafe = false,
            RegistryKeys = [RaRuntime],
            Tags = ["remote-assistance", "disable", "runtime", "security"],
            Description =
                "Sets fAllowToGetHelp=0 in the runtime Remote Assistance control key. "
                + "Disables Remote Assistance at the OS level for non-domain / standalone machines. "
                + "Default: enabled. complements the GPO setting. Effective without domain membership.",
            ApplyOps = [RegOp.SetDword(RaRuntime, "fAllowToGetHelp", 0)],
            RemoveOps = [RegOp.SetDword(RaRuntime, "fAllowToGetHelp", 1)],
            DetectOps = [RegOp.CheckDword(RaRuntime, "fAllowToGetHelp", 0)],
        },
        new TweakDef
        {
            Id = "rast-view-only-runtime",
            Label = "Remote Assistance: Restrict to View-Only (Runtime)",
            Category = "Remote Assistance Policy",
            NeedsAdmin = true,
            CorpSafe = false,
            RegistryKeys = [RaRuntime],
            Tags = ["remote-assistance", "view-only", "runtime", "privacy"],
            Description =
                "Sets fAllowFullControl=0 in the runtime Remote Assistance key. "
                + "Helpers can observe the session but cannot control the keyboard or mouse. "
                + "Default: full control. Recommended: view-only to limit surface during RA.",
            ApplyOps = [RegOp.SetDword(RaRuntime, "fAllowFullControl", 0)],
            RemoveOps = [RegOp.SetDword(RaRuntime, "fAllowFullControl", 1)],
            DetectOps = [RegOp.CheckDword(RaRuntime, "fAllowFullControl", 0)],
        },
        new TweakDef
        {
            Id = "rast-limit-ticket-1hr-runtime",
            Label = "Remote Assistance: Limit Invitation Lifetime to 1 Hour (Runtime)",
            Category = "Remote Assistance Policy",
            NeedsAdmin = true,
            CorpSafe = false,
            RegistryKeys = [RaRuntime],
            Tags = ["remote-assistance", "ticket", "expiry", "runtime", "security"],
            Description =
                "Sets MaxTicketExpiry=1 and MaxTicketExpiryUnits=1 in the runtime key. "
                + "RA invitations expire after 1 hour. Default: 6 hours (Windows default). "
                + "Reduces the time window during which a leaked invitation can be exploited.",
            ApplyOps = [RegOp.SetDword(RaRuntime, "MaxTicketExpiry", 1), RegOp.SetDword(RaRuntime, "MaxTicketExpiryUnits", 1)],
            RemoveOps = [RegOp.SetDword(RaRuntime, "MaxTicketExpiry", 6), RegOp.SetDword(RaRuntime, "MaxTicketExpiryUnits", 1)],
            DetectOps = [RegOp.CheckDword(RaRuntime, "MaxTicketExpiry", 1)],
        },
        new TweakDef
        {
            Id = "rast-disable-chat",
            Label = "Remote Assistance: Disable Chat Window",
            Category = "Remote Assistance Policy",
            NeedsAdmin = true,
            CorpSafe = false,
            RegistryKeys = [RaRuntime],
            Tags = ["remote-assistance", "chat", "privacy", "runtime"],
            Description =
                "Sets fEnableChatControl=0. Disables the text chat panel during Remote Assistance sessions. "
                + "Reduces communication surface and prevents helpers from exfiltrating data via chat. "
                + "Default: chat enabled. Recommended: disable on sensitive systems.",
            ApplyOps = [RegOp.SetDword(RaRuntime, "fEnableChatControl", 0)],
            RemoveOps = [RegOp.SetDword(RaRuntime, "fEnableChatControl", 1)],
            DetectOps = [RegOp.CheckDword(RaRuntime, "fEnableChatControl", 0)],
        },
        new TweakDef
        {
            Id = "rast-deny-ts-connections-gpo",
            Label = "Remote Assistance: Block All Remote Desktop/RA Connections via GPO",
            Category = "Remote Assistance Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [RaPolicy],
            Tags = ["remote-assistance", "rdp", "gpo", "lockdown", "security"],
            Description =
                "Sets fDenyTSConnections=1 in Terminal Services policy. Blocks all incoming Remote Desktop "
                + "and Remote Assistance connections at the policy level. "
                + "Default: connections allowed. Use on high-security kiosk or server systems without RDP needs.",
            ApplyOps = [RegOp.SetDword(RaPolicy, "fDenyTSConnections", 1)],
            RemoveOps = [RegOp.DeleteValue(RaPolicy, "fDenyTSConnections")],
            DetectOps = [RegOp.CheckDword(RaPolicy, "fDenyTSConnections", 1)],
        },
    ];
}
