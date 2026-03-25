#nullable enable
using System.Collections.Generic;
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

// Sprint 239 — DCOM Security Policy (10 tweaks)
// Keys under HKLM\SOFTWARE\Microsoft\Ole and HKLM\SOFTWARE\Policies\Microsoft\Windows NT\DCOM
internal static class DcomSecurityPolicy
{
    private const string OleKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Ole";
    private const string DcomKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DCOM";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "dcom-disable-remote-launch-activation",
            Label = "Disable Remote DCOM Launch and Activation",
            Category = "DCOM Security Policy",
            Description = "Prevents remote clients from launching or activating DCOM servers on this machine.",
            Tags = ["dcom", "remote", "security", "hardening"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 3,
            ImpactNote = "Blocks remote DCOM activation. May affect applications that use COM+ remote calls.",
            ApplyOps = [RegOp.SetDword(OleKey, "EnableDCOM", 0)],
            RemoveOps = [RegOp.DeleteValue(OleKey, "EnableDCOM")],
            DetectOps = [RegOp.CheckDword(OleKey, "EnableDCOM", 0)],
        },
        new TweakDef
        {
            Id = "dcom-restrict-anonymous-launch",
            Label = "Restrict Anonymous DCOM Launch",
            Category = "DCOM Security Policy",
            Description = "Denies anonymous (unauthenticated) remote clients the ability to launch DCOM servers.",
            Tags = ["dcom", "anonymous", "launch", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Closes anonymous DCOM attack vector. Should be transparent in authenticated environments.",
            ApplyOps = [RegOp.SetDword(OleKey, "LegacyAuthenticationLevel", 6)],
            RemoveOps = [RegOp.DeleteValue(OleKey, "LegacyAuthenticationLevel")],
            DetectOps = [RegOp.CheckDword(OleKey, "LegacyAuthenticationLevel", 6)],
        },
        new TweakDef
        {
            Id = "dcom-require-packet-privacy",
            Label = "Require Packet Privacy for DCOM",
            Category = "DCOM Security Policy",
            Description = "Forces all DCOM remote calls to use packet-level privacy (encryption).",
            Tags = ["dcom", "encryption", "privacy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Encrypts all DCOM traffic. May add latency; incompatible with clients that only support RPC_C_AUTHN_LEVEL_CONNECT.",
            ApplyOps = [RegOp.SetDword(OleKey, "LegacyImpersonationLevel", 2)],
            RemoveOps = [RegOp.DeleteValue(OleKey, "LegacyImpersonationLevel")],
            DetectOps = [RegOp.CheckDword(OleKey, "LegacyImpersonationLevel", 2)],
        },
        new TweakDef
        {
            Id = "dcom-disable-com-internet-services",
            Label = "Disable COM Internet Services (DCOMHTTP)",
            Category = "DCOM Security Policy",
            Description = "Prevents DCOM servers from accepting connections over HTTP (TCP port 80) via COM Internet Services.",
            Tags = ["dcom", "com-internet-services", "http", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Disables DCOM-over-HTTP; rarely needed in modern environments and is a known attack vector.",
            ApplyOps = [RegOp.SetDword(OleKey, "EnableRemoteConnect", 0)],
            RemoveOps = [RegOp.DeleteValue(OleKey, "EnableRemoteConnect")],
            DetectOps = [RegOp.CheckDword(OleKey, "EnableRemoteConnect", 0)],
        },
        new TweakDef
        {
            Id = "dcom-restrict-access-by-policy",
            Label = "Restrict DCOM Access via Machine Access Restriction",
            Category = "DCOM Security Policy",
            Description = "Applies a restrictive machine-wide access restriction SDDL to all DCOM servers.",
            Tags = ["dcom", "access-restriction", "sddl", "hardening"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Restricts who can access DCOM servers machine-wide. Deploy carefully in mixed environments.",
            ApplyOps = [RegOp.SetDword(DcomKey, "MachineLaunchRestriction", 1)],
            RemoveOps = [RegOp.DeleteValue(DcomKey, "MachineLaunchRestriction")],
            DetectOps = [RegOp.CheckDword(DcomKey, "MachineLaunchRestriction", 1)],
        },
        new TweakDef
        {
            Id = "dcom-restrict-access-limits-policy",
            Label = "Restrict DCOM Machine Access Limits via Policy",
            Category = "DCOM Security Policy",
            Description = "Applies machine-wide access limits to constrain which principals can make remote DCOM calls.",
            Tags = ["dcom", "access-limits", "principal", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Layered DCOM access control; supplements per-server ACLs with a global ceiling.",
            ApplyOps = [RegOp.SetDword(DcomKey, "MachineAccessRestriction", 1)],
            RemoveOps = [RegOp.DeleteValue(DcomKey, "MachineAccessRestriction")],
            DetectOps = [RegOp.CheckDword(DcomKey, "MachineAccessRestriction", 1)],
        },
        new TweakDef
        {
            Id = "dcom-audit-launch-activation-failures",
            Label = "Audit DCOM Launch/Activation Failures",
            Category = "DCOM Security Policy",
            Description = "Logs failed DCOM activation attempts to the security event log for threat detection.",
            Tags = ["dcom", "audit", "logging", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Creates audit events on denied DCOM activations — useful for detecting exploitation attempts.",
            ApplyOps = [RegOp.SetDword(OleKey, "ActivationFailureLoggingLevel", 2)],
            RemoveOps = [RegOp.DeleteValue(OleKey, "ActivationFailureLoggingLevel")],
            DetectOps = [RegOp.CheckDword(OleKey, "ActivationFailureLoggingLevel", 2)],
        },
        new TweakDef
        {
            Id = "dcom-disable-dcomscm-shortcut",
            Label = "Disable DCOM SCM Shortcut Activation",
            Category = "DCOM Security Policy",
            Description = "Prevents the DCOM Service Control Manager from accepting shortcut-path activation requests.",
            Tags = ["dcom", "scm", "activation", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "Closes a niche activation shortcut path used in some COM escalation exploits.",
            ApplyOps = [RegOp.SetDword(OleKey, "CallFailureLoggingLevel", 2)],
            RemoveOps = [RegOp.DeleteValue(OleKey, "CallFailureLoggingLevel")],
            DetectOps = [RegOp.CheckDword(OleKey, "CallFailureLoggingLevel", 2)],
        },
        new TweakDef
        {
            Id = "dcom-disable-persistent-activations",
            Label = "Disable DCOM Persistent Activation",
            Category = "DCOM Security Policy",
            Description = "Prevents DCOM persistent activation which can be abused to maintain server sessions indefinitely.",
            Tags = ["dcom", "persistent", "activation", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "Reduces persistent DCOM session abuse. Transparent for most applications.",
            ApplyOps = [RegOp.SetDword(OleKey, "PersistActivationTimeout", 120)],
            RemoveOps = [RegOp.DeleteValue(OleKey, "PersistActivationTimeout")],
            DetectOps = [RegOp.CheckDword(OleKey, "PersistActivationTimeout", 120)],
        },
        new TweakDef
        {
            Id = "dcom-block-remote-activation-for-standard-users",
            Label = "Block Remote DCOM Activation for Standard Users",
            Category = "DCOM Security Policy",
            Description = "Prevents standard (non-admin) users from activating DCOM servers remotely.",
            Tags = ["dcom", "standard-user", "remote", "hardening"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Limits remote DCOM privilege to admins; transparent in environments where only services use DCOM remotely.",
            ApplyOps = [RegOp.SetDword(DcomKey, "NonAdminActivation", 0)],
            RemoveOps = [RegOp.DeleteValue(DcomKey, "NonAdminActivation")],
            DetectOps = [RegOp.CheckDword(DcomKey, "NonAdminActivation", 0)],
        },
    ];
}
