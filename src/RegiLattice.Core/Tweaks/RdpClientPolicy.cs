// RegiLattice.Core — Tweaks/RdpClientPolicy.cs
// Remote Desktop Protocol (RDP) client-side security policy tweaks (Sprint 107).
// Slug: "rdpclt-*" — distinct from RemoteDesktop.cs (server-side configuration).
// Focus: client-side NLA enforcement, server authentication, credential delegation,
// clipboard/drive redirection, and RDP over HTTPS (gateway).
// Registry base: HKCU\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services
//                HKLM\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class RdpClientPolicy
{
    private const string RdpClientPolicyKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services\Client";
    private const string RdpClientUserKey = @"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services";
    private const string RdpServerPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "rdpclt-require-nla",
            Label = "RDP Client: Require Network Level Authentication (NLA)",
            Category = "Remote Desktop (Client)",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 17763,
            ImpactScore = 5,
            SafetyRating = 5,
            RegistryKeys = [RdpClientPolicyKey],
            Tags = ["rdp", "remote-desktop", "nla", "authentication", "security", "hardening"],
            Description =
                "Sets fRequireNLA=1 in Terminal Services\\Client policy. "
                + "Enforces Network Level Authentication before the RDP connection is established. "
                + "NLA authenticates the user before the full desktop session is created, preventing "
                + "pre-authentication credential-theft attacks and reducing the attack surface of the "
                + "Remote Desktop logon screen.",
            ApplyOps = [RegOp.SetDword(RdpClientPolicyKey, "fRequireNLA", 1)],
            RemoveOps = [RegOp.DeleteValue(RdpClientPolicyKey, "fRequireNLA")],
            DetectOps = [RegOp.CheckDword(RdpClientPolicyKey, "fRequireNLA", 1)],
        },
        new TweakDef
        {
            Id = "rdpclt-warn-on-auth-fail",
            Label = "RDP Client: Warn When Server Authentication Fails",
            Category = "Remote Desktop (Client)",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 17763,
            ImpactScore = 3,
            SafetyRating = 5,
            RegistryKeys = [RdpClientPolicyKey],
            Tags = ["rdp", "remote-desktop", "server-authentication", "certificate", "security", "mitm"],
            Description =
                "Sets AuthenticationLevel=1 in Terminal Services\\Client policy. "
                + "Prompts the user with a warning before connecting when the server's identity "
                + "certificate cannot be verified. 0=always connect, 1=warn (default), 2=never connect "
                + "if auth fails. Setting 1 ensures users see MITM warnings instead of connecting silently.",
            ApplyOps = [RegOp.SetDword(RdpClientPolicyKey, "AuthenticationLevel", 1)],
            RemoveOps = [RegOp.DeleteValue(RdpClientPolicyKey, "AuthenticationLevel")],
            DetectOps = [RegOp.CheckDword(RdpClientPolicyKey, "AuthenticationLevel", 1)],
        },
        new TweakDef
        {
            Id = "rdpclt-deny-on-auth-fail",
            Label = "RDP Client: Block Connection When Server Authentication Fails",
            Category = "Remote Desktop (Client)",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 17763,
            ImpactScore = 4,
            SafetyRating = 4,
            RegistryKeys = [RdpClientPolicyKey],
            Tags = ["rdp", "remote-desktop", "server-authentication", "certificate", "security", "hardening", "mitm"],
            Description =
                "Sets AuthenticationLevel=2 in Terminal Services\\Client policy. "
                + "Blocks the RDP connection entirely if the server's identity certificate cannot be "
                + "verified. Provides the strongest protection against MITM interception of RDP sessions. "
                + "Requires valid certificates on all RDP servers.",
            ApplyOps = [RegOp.SetDword(RdpClientPolicyKey, "AuthenticationLevel", 2)],
            RemoveOps = [RegOp.DeleteValue(RdpClientPolicyKey, "AuthenticationLevel")],
            DetectOps = [RegOp.CheckDword(RdpClientPolicyKey, "AuthenticationLevel", 2)],
        },
        new TweakDef
        {
            Id = "rdpclt-disable-clipboard-redirection",
            Label = "RDP Client: Disable Clipboard Redirection",
            Category = "Remote Desktop (Client)",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 17763,
            ImpactScore = 3,
            SafetyRating = 5,
            RegistryKeys = [RdpServerPolicy],
            Tags = ["rdp", "remote-desktop", "clipboard", "data-exfiltration", "security", "dlp"],
            Description =
                "Sets fDisableClip=1 in Terminal Services server policy. "
                + "Prevents clipboard content from being shared between the RDP client and the remote "
                + "server session. Blocks a common data-exfiltration vector where users copy sensitive "
                + "data between the corporate remote desktop and their local machine.",
            ApplyOps = [RegOp.SetDword(RdpServerPolicy, "fDisableClip", 1)],
            RemoveOps = [RegOp.DeleteValue(RdpServerPolicy, "fDisableClip")],
            DetectOps = [RegOp.CheckDword(RdpServerPolicy, "fDisableClip", 1)],
        },
        new TweakDef
        {
            Id = "rdpclt-disable-drive-redirection",
            Label = "RDP Client: Disable Local Drive Redirection",
            Category = "Remote Desktop (Client)",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 17763,
            ImpactScore = 3,
            SafetyRating = 5,
            RegistryKeys = [RdpServerPolicy],
            Tags = ["rdp", "remote-desktop", "drive-redirection", "data-exfiltration", "security", "dlp"],
            Description =
                "Sets fDisableCdm=1 in Terminal Services server policy. "
                + "Blocks access to local client drives (C:, D:, USB, etc.) from within the RDP "
                + "session. Prevents data exfiltration via drag-and-drop or file copy between the "
                + "remote session and local storage.",
            ApplyOps = [RegOp.SetDword(RdpServerPolicy, "fDisableCdm", 1)],
            RemoveOps = [RegOp.DeleteValue(RdpServerPolicy, "fDisableCdm")],
            DetectOps = [RegOp.CheckDword(RdpServerPolicy, "fDisableCdm", 1)],
        },
        new TweakDef
        {
            Id = "rdpclt-disable-printer-redirection",
            Label = "RDP Client: Disable Printer Redirection",
            Category = "Remote Desktop (Client)",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 17763,
            ImpactScore = 2,
            SafetyRating = 5,
            RegistryKeys = [RdpServerPolicy],
            Tags = ["rdp", "remote-desktop", "printer", "redirection", "security"],
            Description =
                "Sets fDisableCpm=1 in Terminal Services server policy. "
                + "Blocks local printers from being redirected into the RDP session. "
                + "Reduces the attack surface by preventing spooler access from the remote session "
                + "and blocking potential PrintNightmare-style printer driver exploitation via RDP.",
            ApplyOps = [RegOp.SetDword(RdpServerPolicy, "fDisableCpm", 1)],
            RemoveOps = [RegOp.DeleteValue(RdpServerPolicy, "fDisableCpm")],
            DetectOps = [RegOp.CheckDword(RdpServerPolicy, "fDisableCpm", 1)],
        },
        new TweakDef
        {
            Id = "rdpclt-force-encryption-high",
            Label = "RDP Client: Enforce High (128-Bit) RDP Encryption",
            Category = "Remote Desktop (Client)",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 17763,
            ImpactScore = 4,
            SafetyRating = 5,
            RegistryKeys = [RdpServerPolicy],
            Tags = ["rdp", "remote-desktop", "encryption", "tls", "security", "hardening"],
            Description =
                "Sets MinEncryptionLevel=3 in Terminal Services policy (3=High/128-bit). "
                + "Enforces 128-bit RC4 or TLS encryption for all RDP session data. "
                + "Level 1=low, 2=medium (legacy 56-bit), 3=high, 4=FIPS-compliant. "
                + "Modern RDP (TLS mode) supersedes this, but the policy prevents fallback to weak ciphers.",
            ApplyOps = [RegOp.SetDword(RdpServerPolicy, "MinEncryptionLevel", 3)],
            RemoveOps = [RegOp.DeleteValue(RdpServerPolicy, "MinEncryptionLevel")],
            DetectOps = [RegOp.CheckDword(RdpServerPolicy, "MinEncryptionLevel", 3)],
        },
        new TweakDef
        {
            Id = "rdpclt-set-session-timeout-30min",
            Label = "RDP Client: Disconnect Idle Sessions After 30 Minutes",
            Category = "Remote Desktop (Client)",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 17763,
            ImpactScore = 3,
            SafetyRating = 5,
            RegistryKeys = [RdpServerPolicy],
            Tags = ["rdp", "remote-desktop", "idle-timeout", "security"],
            Description =
                "Sets MaxIdleTime=1800000 (30 minutes in ms) in Terminal Services policy. "
                + "Automatically disconnects RDP sessions that have been idle for more than 30 minutes. "
                + "Reduces the risk of unattended remote sessions being hijacked and prevents license "
                + "consumption by orphaned sessions.",
            ApplyOps = [RegOp.SetDword(RdpServerPolicy, "MaxIdleTime", 1800000)],
            RemoveOps = [RegOp.DeleteValue(RdpServerPolicy, "MaxIdleTime")],
            DetectOps = [RegOp.CheckDword(RdpServerPolicy, "MaxIdleTime", 1800000)],
        },
        new TweakDef
        {
            Id = "rdpclt-disable-password-save",
            Label = "RDP Client: Prevent Saving Passwords in Remote Desktop Client",
            Category = "Remote Desktop (Client)",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 17763,
            ImpactScore = 3,
            SafetyRating = 5,
            RegistryKeys = [RdpClientPolicyKey],
            Tags = ["rdp", "remote-desktop", "credentials", "password", "security", "credential-manager"],
            Description =
                "Sets DisablePasswordSaving=1 in Terminal Services\\Client policy. "
                + "Disables the 'Save Password' option in the Remote Desktop Connection client (mstsc). "
                + "Prevents RDP credentials from being stored in Windows Credential Manager where they "
                + "can be extracted by credential-dumping tools.",
            ApplyOps = [RegOp.SetDword(RdpClientPolicyKey, "DisablePasswordSaving", 1)],
            RemoveOps = [RegOp.DeleteValue(RdpClientPolicyKey, "DisablePasswordSaving")],
            DetectOps = [RegOp.CheckDword(RdpClientPolicyKey, "DisablePasswordSaving", 1)],
        },
        new TweakDef
        {
            Id = "rdpclt-enable-audit-logging",
            Label = "RDP Client: Enable RDP Session Audit Logging",
            Category = "Remote Desktop (Client)",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 17763,
            ImpactScore = 3,
            SafetyRating = 5,
            RegistryKeys = [RdpServerPolicy],
            Tags = ["rdp", "remote-desktop", "audit", "logging", "security", "compliance", "event-log"],
            Description =
                "Sets fLogonDisabled=0 and EnableLogonWatermarking=1 in Terminal Services policy. "
                + "Ensures that RDP logon events are audited and that the watermarking feature "
                + "(which embeds session metadata for forensics) is enabled. "
                + "Supports SOC investigation of lateral movement via RDP.",
            ApplyOps = [RegOp.SetDword(RdpServerPolicy, "fLogonDisabled", 0), RegOp.SetDword(RdpServerPolicy, "EnableLogonWatermarking", 1)],
            RemoveOps = [RegOp.DeleteValue(RdpServerPolicy, "fLogonDisabled"), RegOp.DeleteValue(RdpServerPolicy, "EnableLogonWatermarking")],
            DetectOps = [RegOp.CheckDword(RdpServerPolicy, "fLogonDisabled", 0), RegOp.CheckDword(RdpServerPolicy, "EnableLogonWatermarking", 1)],
        },
    ];
}
