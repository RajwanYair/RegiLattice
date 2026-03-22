// RegiLattice.Core — Tweaks/KerberosAdvanced.cs
// Advanced Kerberos ticket-lifetime, encryption, and pre-auth policy tweaks (Sprint 107).
// Slug: "krb-*" — distinct from Security.cs (RunAsPPL) and Hardening.cs (general GPO).
// Registry bases:
//   HKLM\SYSTEM\CurrentControlSet\Control\Lsa\Kerberos\Parameters
//   HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\Kerberos\Parameters

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class KerberosAdvanced
{
    private const string KrbLsaParams = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\Kerberos\Parameters";
    private const string KrbPolicyParams = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\Kerberos\Parameters";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "krb-require-aes256-encryption",
            Label = "Kerberos: Require AES-256 Encryption for Ticket Grants",
            Category = "Kerberos Authentication",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 17763,
            ImpactScore = 4,
            SafetyRating = 4,
            RegistryKeys = [KrbLsaParams],
            Tags = ["kerberos", "authentication", "aes256", "encryption", "security", "hardening"],
            Description =
                "Sets SupportedEncryptionTypes=0x18 (24 decimal) in Kerberos\\Parameters. "
                + "Bit mask 0x18 enables only AES-128-CTS-HMAC-SHA1-96 and AES-256-CTS-HMAC-SHA1-96. "
                + "Disables the weaker RC4-HMAC (NTLM-hash-based) cipher used in older 'Kerberoasting' "
                + "attacks. Requires Windows Server 2008 R2 or later as the KDC.",
            ApplyOps = [RegOp.SetDword(KrbLsaParams, "SupportedEncryptionTypes", 0x18)],
            RemoveOps = [RegOp.DeleteValue(KrbLsaParams, "SupportedEncryptionTypes")],
            DetectOps = [RegOp.CheckDword(KrbLsaParams, "SupportedEncryptionTypes", 0x18)],
        },
        new TweakDef
        {
            Id = "krb-disable-des-encryption",
            Label = "Kerberos: Disable DES Encryption Types",
            Category = "Kerberos Authentication",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 17763,
            ImpactScore = 4,
            SafetyRating = 5,
            RegistryKeys = [KrbPolicyParams],
            Tags = ["kerberos", "authentication", "des", "encryption", "security", "hardening"],
            Description =
                "Sets SupportedEncryptionTypes=0x7FFFFFF8 in Windows Kerberos policy. "
                + "Disables all DES and RC4-MD5 ciphers (bits 0–2 cleared), retaining AES-128/256 "
                + "and future ciphers. DES was retired by NIST in 2004; its presence enables "
                + "down-grade attacks in mixed-mode Active Directory environments.",
            ApplyOps = [RegOp.SetDword(KrbPolicyParams, "SupportedEncryptionTypes", 0x7FFFFFF8)],
            RemoveOps = [RegOp.DeleteValue(KrbPolicyParams, "SupportedEncryptionTypes")],
            DetectOps = [RegOp.CheckDword(KrbPolicyParams, "SupportedEncryptionTypes", 0x7FFFFFF8)],
        },
        new TweakDef
        {
            Id = "krb-set-max-ticket-life-8h",
            Label = "Kerberos: Set Maximum Ticket Lifetime to 8 Hours",
            Category = "Kerberos Authentication",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 17763,
            ImpactScore = 3,
            SafetyRating = 5,
            RegistryKeys = [KrbLsaParams],
            Tags = ["kerberos", "authentication", "ticket-lifetime", "security"],
            Description =
                "Sets MaxTicketAge=8 in Kerberos\\Parameters. "
                + "Limits the maximum lifetime of Kerberos service tickets to 8 hours (value in hours). "
                + "Default is 10 hours. Shorter lifetime reduces the window in which a stolen ticket "
                + "can be replayed (golden/silver ticket attacks have a narrower useful window).",
            ApplyOps = [RegOp.SetDword(KrbLsaParams, "MaxTicketAge", 8)],
            RemoveOps = [RegOp.DeleteValue(KrbLsaParams, "MaxTicketAge")],
            DetectOps = [RegOp.CheckDword(KrbLsaParams, "MaxTicketAge", 8)],
        },
        new TweakDef
        {
            Id = "krb-set-max-renewable-ticket-life-7d",
            Label = "Kerberos: Set Maximum Renewable Ticket Lifetime to 7 Days",
            Category = "Kerberos Authentication",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 17763,
            ImpactScore = 3,
            SafetyRating = 5,
            RegistryKeys = [KrbLsaParams],
            Tags = ["kerberos", "authentication", "ticket-lifetime", "security"],
            Description =
                "Sets MaxRenewAge=7 in Kerberos\\Parameters. "
                + "Limits the maximum renewable lifetime of a Kerberos TGT to 7 days (value in days). "
                + "Default is 7 days; setting it explicitly via registry ensures it is not overridden. "
                + "Renewable tickets allow continuous session renewal without re-entering credentials.",
            ApplyOps = [RegOp.SetDword(KrbLsaParams, "MaxRenewAge", 7)],
            RemoveOps = [RegOp.DeleteValue(KrbLsaParams, "MaxRenewAge")],
            DetectOps = [RegOp.CheckDword(KrbLsaParams, "MaxRenewAge", 7)],
        },
        new TweakDef
        {
            Id = "krb-disable-clock-skew-tolerance",
            Label = "Kerberos: Tighten Clock-Skew Tolerance to 3 Minutes",
            Category = "Kerberos Authentication",
            NeedsAdmin = true,
            CorpSafe = false,
            MinBuild = 17763,
            ImpactScore = 2,
            SafetyRating = 3,
            RegistryKeys = [KrbLsaParams],
            Tags = ["kerberos", "authentication", "clock-skew", "security", "ntp"],
            Description =
                "Sets SkewTime=3 in Kerberos\\Parameters. "
                + "Reduces the accepted clock skew between the client and KDC from 5 minutes (default) "
                + "to 3 minutes. A smaller window narrows replay-attack opportunities while still "
                + "tolerating NTP drift. Requires accurate NTP synchronisation.",
            ApplyOps = [RegOp.SetDword(KrbLsaParams, "SkewTime", 3)],
            RemoveOps = [RegOp.DeleteValue(KrbLsaParams, "SkewTime")],
            DetectOps = [RegOp.CheckDword(KrbLsaParams, "SkewTime", 3)],
        },
        new TweakDef
        {
            Id = "krb-require-preauth",
            Label = "Kerberos: Require Pre-Authentication for All Accounts",
            Category = "Kerberos Authentication",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 17763,
            ImpactScore = 4,
            SafetyRating = 5,
            RegistryKeys = [KrbPolicyParams],
            Tags = ["kerberos", "authentication", "pre-auth", "as-rep-roasting", "security"],
            Description =
                "Sets AllowForestTrustsForKerberos=0 in Kerberos policy. "
                + "When combined with per-account pre-auth requirements, this prevents AS-REP Roasting: "
                + "attackers cannot request a TGT for accounts without supplying a valid password hash. "
                + "All modern Active Directory accounts should have pre-auth enabled (it is on by default).",
            ApplyOps = [RegOp.SetDword(KrbPolicyParams, "AllowForestTrustsForKerberos", 0)],
            RemoveOps = [RegOp.DeleteValue(KrbPolicyParams, "AllowForestTrustsForKerberos")],
            DetectOps = [RegOp.CheckDword(KrbPolicyParams, "AllowForestTrustsForKerberos", 0)],
        },
        new TweakDef
        {
            Id = "krb-enable-cbac",
            Label = "Kerberos: Enable Claims-Based Access Control (CBAC)",
            Category = "Kerberos Authentication",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 17763,
            ImpactScore = 3,
            SafetyRating = 5,
            RegistryKeys = [KrbPolicyParams],
            Tags = ["kerberos", "authentication", "cbac", "dynamic-access-control", "security", "active-directory"],
            Description =
                "Sets EnableCbacAndArmor=1 in Kerberos policy. "
                + "Enables Dynamic Access Control (DAC) claims and Kerberos armoring (FAST). "
                + "Claims-based authentication allows AD to include user/device attributes in Kerberos "
                + "tickets for richer conditional access policies. FAST provides channel-binding "
                + "protection against PKINIT spoofing.",
            ApplyOps = [RegOp.SetDword(KrbPolicyParams, "EnableCbacAndArmor", 1)],
            RemoveOps = [RegOp.DeleteValue(KrbPolicyParams, "EnableCbacAndArmor")],
            DetectOps = [RegOp.CheckDword(KrbPolicyParams, "EnableCbacAndArmor", 1)],
        },
        new TweakDef
        {
            Id = "krb-disable-rc4-hmac",
            Label = "Kerberos: Explicitly Disable RC4-HMAC (ARCFOUR) Encryption",
            Category = "Kerberos Authentication",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 19041,
            ImpactScore = 4,
            SafetyRating = 4,
            RegistryKeys = [KrbLsaParams],
            Tags = ["kerberos", "authentication", "rc4", "arcfour", "kerberoasting", "security", "hardening"],
            Description =
                "Sets SupportedEncryptionTypes=0x7FFFFFB8 in Kerberos\\Parameters (clears RC4 bits). "
                + "Explicitly removes RC4-HMAC (type 23) from the supported encryption set. "
                + "RC4-HMAC is vulnerable to Kerberoasting (offline NTLM-hash cracking) and should "
                + "not be used in environments with Windows Server 2019+ domain controllers.",
            ApplyOps = [RegOp.SetDword(KrbLsaParams, "SupportedEncryptionTypes", 0x7FFFFFB8)],
            RemoveOps = [RegOp.DeleteValue(KrbLsaParams, "SupportedEncryptionTypes")],
            DetectOps = [RegOp.CheckDword(KrbLsaParams, "SupportedEncryptionTypes", 0x7FFFFFB8)],
        },
        new TweakDef
        {
            Id = "krb-log-authentication-events",
            Label = "Kerberos: Enable Verbose Authentication Logging",
            Category = "Kerberos Authentication",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 17763,
            ImpactScore = 2,
            SafetyRating = 5,
            RegistryKeys = [KrbLsaParams],
            Tags = ["kerberos", "authentication", "audit", "logging", "security"],
            Description =
                "Sets LogLevel=1 in Kerberos\\Parameters. "
                + "Enables verbose Kerberos diagnostic logging to the System event log. "
                + "Logs KDC errors, ticket failures, and encryption type negotiations. "
                + "Very useful for diagnosing Kerberos failures but generates moderate event log volume.",
            ApplyOps = [RegOp.SetDword(KrbLsaParams, "LogLevel", 1)],
            RemoveOps = [RegOp.DeleteValue(KrbLsaParams, "LogLevel")],
            DetectOps = [RegOp.CheckDword(KrbLsaParams, "LogLevel", 1)],
        },
        new TweakDef
        {
            Id = "krb-purge-ticket-cache-on-logoff",
            Label = "Kerberos: Purge Kerberos Ticket Cache on User Logoff",
            Category = "Kerberos Authentication",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 17763,
            ImpactScore = 3,
            SafetyRating = 5,
            RegistryKeys = [KrbLsaParams],
            Tags = ["kerberos", "authentication", "ticket-cache", "logoff", "security", "credential-hygiene"],
            Description =
                "Sets PurgeTicketCacheOnLogoff=1 in Kerberos\\Parameters. "
                + "Forces the Kerberos subsystem to clear all cached TGTs and service tickets when a "
                + "user logs off. Prevents cached tickets from being recovered from a hibernation "
                + "file or memory dump after the session ends.",
            ApplyOps = [RegOp.SetDword(KrbLsaParams, "PurgeTicketCacheOnLogoff", 1)],
            RemoveOps = [RegOp.DeleteValue(KrbLsaParams, "PurgeTicketCacheOnLogoff")],
            DetectOps = [RegOp.CheckDword(KrbLsaParams, "PurgeTicketCacheOnLogoff", 1)],
        },
    ];
}
