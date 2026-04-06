namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class SecurityCredentialGuard
{
    private const string DevGuardKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceGuard";

    private const string WDigestKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\WDigest";

    private const string CredDelegKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CredentialsDelegation";

    private const string LsaKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa";

    private const string PassportKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PassportForWork";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "credguard-enable-vbs",
                Label = "Enable Virtualisation-Based Security (VBS)",
                Category = "Security — Credential Guard",
                Description =
                    "Enables Virtualisation-Based Security in the Device Guard policy. VBS creates an isolated hypervisor environment "
                    + "to protect security-critical components including Credential Guard. "
                    + "Default: disabled. Recommended: enabled on hardware supporting VBS.",
                Tags = ["vbs", "credential-guard", "hypervisor", "device-guard", "kernel-isolation"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ApplyOps = [RegOp.SetDword(DevGuardKey, "EnableVirtualizationBasedSecurity", 1)],
                RemoveOps = [RegOp.DeleteValue(DevGuardKey, "EnableVirtualizationBasedSecurity")],
                DetectOps = [RegOp.CheckDword(DevGuardKey, "EnableVirtualizationBasedSecurity", 1)],
            },
            new TweakDef
            {
                Id = "credguard-enable-credential-guard-uefi",
                Label = "Enable Credential Guard with UEFI Lock",
                Category = "Security — Credential Guard",
                Description =
                    "Enables Windows Defender Credential Guard and locks the configuration with UEFI firmware. "
                    + "LsaCfgFlags=1 enables Credential Guard and writes the configuration to UEFI, preventing reversal without physical access. "
                    + "Default: disabled. Recommended: 1 (enabled with UEFI lock).",
                Tags = ["credential-guard", "uefi", "lsa", "device-guard", "pass-the-hash", "credential-theft"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ApplyOps = [RegOp.SetDword(DevGuardKey, "LsaCfgFlags", 1)],
                RemoveOps = [RegOp.DeleteValue(DevGuardKey, "LsaCfgFlags")],
                DetectOps = [RegOp.CheckDword(DevGuardKey, "LsaCfgFlags", 1)],
            },
            new TweakDef
            {
                Id = "credguard-disable-wdigest-cleartext",
                Label = "Disable WDigest Cleartext Password Storage",
                Category = "Security — Credential Guard",
                Description =
                    "Prevents WDigest authentication from caching plaintext credentials in lsass memory. "
                    + "UseLogonCredential=0 removes the cleartext credential cache that tools like Mimikatz exploit. "
                    + "Default: disabled on modern Windows. Recommended: explicitly disabled.",
                Tags = ["wdigest", "plaintext-credentials", "mimikatz", "lsass", "credential-dumping"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(WDigestKey, "UseLogonCredential", 0)],
                RemoveOps = [RegOp.DeleteValue(WDigestKey, "UseLogonCredential")],
                DetectOps = [RegOp.CheckDword(WDigestKey, "UseLogonCredential", 0)],
            },
            new TweakDef
            {
                Id = "credguard-enable-remote-credential-guard",
                Label = "Enable Remote Credential Guard for RDP",
                Category = "Security — Credential Guard",
                Description =
                    "Allows Credential Guard tokens to be used in Remote Desktop sessions instead of sending credentials to the remote host. "
                    + "Prevents credential material from being exposed on the destination RDP server. "
                    + "Default: not configured. Recommended: enabled (AllowRemoteCredGuard=1).",
                Tags = ["credential-guard", "rdp", "remote-desktop", "kerberos", "restricted-admin"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(CredDelegKey, "AllowRemoteCredGuard", 1)],
                RemoveOps = [RegOp.DeleteValue(CredDelegKey, "AllowRemoteCredGuard")],
                DetectOps = [RegOp.CheckDword(CredDelegKey, "AllowRemoteCredGuard", 1)],
            },
            new TweakDef
            {
                Id = "credguard-block-ntlm-default-creds",
                Label = "Block Default Credential Delegation over NTLM",
                Category = "Security — Credential Guard",
                Description =
                    "Prevents the system from delegating default credentials over NTLM-authenticated channels. "
                    + "Default credentials include the currently logged-on user's Kerberos TGT; NTLM delegation to rogue servers can expose these. "
                    + "Default: delegation on NTLM may be enabled. Recommended: disabled.",
                Tags = ["credential-delegation", "ntlm", "default-creds", "kerberos", "lateral-movement"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(CredDelegKey, "AllowDefCredentialsWhenNTLMOnly", 0)],
                RemoveOps = [RegOp.DeleteValue(CredDelegKey, "AllowDefCredentialsWhenNTLMOnly")],
                DetectOps = [RegOp.CheckDword(CredDelegKey, "AllowDefCredentialsWhenNTLMOnly", 0)],
            },
            new TweakDef
            {
                Id = "credguard-block-saved-creds",
                Label = "Block Saved Credential Delegation",
                Category = "Security — Credential Guard",
                Description =
                    "Prevents saved (cached) credentials from being forwarded to remote servers via credential delegation. "
                    + "AllowSavedCredentials=0 stops tools that extract saved credential material from forwarding them in delegation. "
                    + "Default: saved credential delegation may be allowed. Recommended: blocked.",
                Tags = ["credential-delegation", "saved-credentials", "cached-credentials", "delegation"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(CredDelegKey, "AllowSavedCredentials", 0)],
                RemoveOps = [RegOp.DeleteValue(CredDelegKey, "AllowSavedCredentials")],
                DetectOps = [RegOp.CheckDword(CredDelegKey, "AllowSavedCredentials", 0)],
            },
            new TweakDef
            {
                Id = "credguard-block-saved-ntlm-creds",
                Label = "Block Saved Credential Delegation over NTLM Only",
                Category = "Security — Credential Guard",
                Description =
                    "Prevents saved credentials from being forwarded in NTLM-only authenticated sessions. "
                    + "NTLM sessions lack mutual authentication; forwarding saved credentials exposes them to rogue servers. "
                    + "Default: may be allowed. Recommended: blocked.",
                Tags = ["credential-delegation", "ntlm", "saved-credentials", "lateral-movement"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(CredDelegKey, "AllowSavedCredentialsWhenNTLMOnly", 0)],
                RemoveOps = [RegOp.DeleteValue(CredDelegKey, "AllowSavedCredentialsWhenNTLMOnly")],
                DetectOps = [RegOp.CheckDword(CredDelegKey, "AllowSavedCredentialsWhenNTLMOnly", 0)],
            },
            new TweakDef
            {
                Id = "credguard-block-fresh-ntlm-creds",
                Label = "Block Fresh Credential Delegation over NTLM Only",
                Category = "Security — Credential Guard",
                Description =
                    "Prevents freshly-captured user credentials from being forwarded when the authentication channel is NTLM only. "
                    + "AllowFreshCredentialsWhenNTLMOnly=0 stops credential forwarding to NTLM servers that haven't proven their identity. "
                    + "Default: may be allowed. Recommended: blocked.",
                Tags = ["credential-delegation", "ntlm", "fresh-credentials", "authentication", "lateral-movement"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(CredDelegKey, "AllowFreshCredentialsWhenNTLMOnly", 0)],
                RemoveOps = [RegOp.DeleteValue(CredDelegKey, "AllowFreshCredentialsWhenNTLMOnly")],
                DetectOps = [RegOp.CheckDword(CredDelegKey, "AllowFreshCredentialsWhenNTLMOnly", 0)],
            },
            new TweakDef
            {
                Id = "credguard-enable-restricted-admin-rdp",
                Label = "Enable Restricted Admin Mode for RDP Sessions",
                Category = "Security — Credential Guard",
                Description =
                    "Enables Restricted Admin mode for Remote Desktop, preventing full credentials from being sent to the remote host. "
                    + "DisableRestrictedAdmin=0 allows the mode to be used; the session uses host credentials rather than forwarded user creds. "
                    + "Default: may be disabled. Recommended: enabled (0).",
                Tags = ["rdp", "restricted-admin", "credential-guard", "lateral-movement", "pass-the-hash"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(LsaKey, "DisableRestrictedAdmin", 0)],
                RemoveOps = [RegOp.DeleteValue(LsaKey, "DisableRestrictedAdmin")],
                DetectOps = [RegOp.CheckDword(LsaKey, "DisableRestrictedAdmin", 0)],
            },
            new TweakDef
            {
                Id = "credguard-require-tpm-for-credentials",
                Label = "Require TPM for Windows Hello Credential Storage",
                Category = "Security — Credential Guard",
                Description =
                    "Requires a Trusted Platform Module (TPM) for Windows Hello for Business key storage. "
                    + "Without a TPM requirement, Windows Hello keys may be stored in software, which is far easier to extract. "
                    + "Default: software keys allowed. Recommended: TPM required.",
                Tags = ["windows-hello", "tpm", "credential-guard", "key-storage", "fido2", "hardware-backed"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(PassportKey, "RequireSecurityDevice", 1)],
                RemoveOps = [RegOp.DeleteValue(PassportKey, "RequireSecurityDevice")],
                DetectOps = [RegOp.CheckDword(PassportKey, "RequireSecurityDevice", 1)],
            },
        ];
}
