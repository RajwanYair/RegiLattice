#nullable enable
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

internal static class TpmSecurityPolicy
{
    private const string Tpm   = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\TPM";
    private const string TpmDg = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceGuard";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "tpmgpo-require-active-directory-backup",
            Label = "Require TPM Owner Info Backup to Active Directory",
            Category = "TPM Security Policy",
            Description = "Requires TPM owner authorization information to be backed up to Active Directory before TPM operations are allowed. Prevents TPM ownership from being set on machines where AD backup fails, ensuring recoverability. Default: 0. Recommended: 1 for AD-joined enterprise machines.",
            Tags = ["tpm", "active-directory", "backup", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Tpm],
            ApplyOps   = [RegOp.SetDword(Tpm, "RequireActiveDirectoryBackup", 1)],
            RemoveOps  = [RegOp.DeleteValue(Tpm, "RequireActiveDirectoryBackup")],
            DetectOps  = [RegOp.CheckDword(Tpm, "RequireActiveDirectoryBackup", 1)],
        },
        new TweakDef
        {
            Id = "tpmgpo-enable-active-directory-backup",
            Label = "Enable TPM Owner Info Active Directory Backup",
            Category = "TPM Security Policy",
            Description = "Enables automatic backup of TPM owner authorization to Active Directory. When combined with RequireActiveDirectoryBackup, ensures all TPM-protected machines have recoverable owner keys in AD. Default: 0. Recommended: 1.",
            Tags = ["tpm", "active-directory", "backup", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Tpm],
            ApplyOps   = [RegOp.SetDword(Tpm, "ActiveDirectoryBackup", 1)],
            RemoveOps  = [RegOp.DeleteValue(Tpm, "ActiveDirectoryBackup")],
            DetectOps  = [RegOp.CheckDword(Tpm, "ActiveDirectoryBackup", 1)],
        },
        new TweakDef
        {
            Id = "tpmgpo-os-managed-auth-level",
            Label = "Set TPM OS-Managed Auth Level to Full Delegation",
            Category = "TPM Security Policy",
            Description = "Sets OS-managed TPM auth level to 4 (full delegation). This determines how much of the TPM authorization is delegated to the OS vs retained by the hardware. Level 4 allows OS full control needed for BitLocker and Device Guard. Default: 4. Recommended: 4.",
            Tags = ["tpm", "auth", "delegation", "bitlocker"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Tpm],
            ApplyOps   = [RegOp.SetDword(Tpm, "OSManagedAuthLevel", 4)],
            RemoveOps  = [RegOp.DeleteValue(Tpm, "OSManagedAuthLevel")],
            DetectOps  = [RegOp.CheckDword(Tpm, "OSManagedAuthLevel", 4)],
        },
        new TweakDef
        {
            Id = "tpmgpo-standard-user-lockout-threshold",
            Label = "Set TPM Standard-User Authorization Failure Threshold",
            Category = "TPM Security Policy",
            Description = "Sets the TPM lockout threshold for standard users to 32 failed authorization attempts before the TPM enters lockout mode. Balances brute-force protection with usability. Default: 32. Recommended: 9 for stricter environments.",
            Tags = ["tpm", "lockout", "brute-force", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Tpm],
            ApplyOps   = [RegOp.SetDword(Tpm, "StandardUserAuthorizationFailureTotalThreshold", 9)],
            RemoveOps  = [RegOp.DeleteValue(Tpm, "StandardUserAuthorizationFailureTotalThreshold")],
            DetectOps  = [RegOp.CheckDword(Tpm, "StandardUserAuthorizationFailureTotalThreshold", 9)],
        },
        new TweakDef
        {
            Id = "tpmgpo-standard-user-lockout-duration",
            Label = "Set TPM Standard-User Lockout Duration to 1 Hour",
            Category = "TPM Security Policy",
            Description = "Sets the TPM lockout observation window to 3 600 seconds (1 hour). Failed authorization attempts within this window count toward the lockout threshold. After the window expires, failed counts reset. Default: 7200. Recommended: 3600.",
            Tags = ["tpm", "lockout", "duration", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Tpm],
            ApplyOps   = [RegOp.SetDword(Tpm, "StandardUserAuthorizationFailureDuration", 3600)],
            RemoveOps  = [RegOp.DeleteValue(Tpm, "StandardUserAuthorizationFailureDuration")],
            DetectOps  = [RegOp.CheckDword(Tpm, "StandardUserAuthorizationFailureDuration", 3600)],
        },
        new TweakDef
        {
            Id = "tpmgpo-standard-user-individual-lockout",
            Label = "Set TPM Standard-User Individual Auth Failure Threshold",
            Category = "TPM Security Policy",
            Description = "Sets the TPM individual authorization failure threshold for standard users to 4. A single TPM authorization can fail at most 4 times within the observation window before triggering lockout for that key. Default: 4. Recommended: 4.",
            Tags = ["tpm", "lockout", "individual", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Tpm],
            ApplyOps   = [RegOp.SetDword(Tpm, "StandardUserAuthorizationFailureIndividualThreshold", 4)],
            RemoveOps  = [RegOp.DeleteValue(Tpm, "StandardUserAuthorizationFailureIndividualThreshold")],
            DetectOps  = [RegOp.CheckDword(Tpm, "StandardUserAuthorizationFailureIndividualThreshold", 4)],
        },
        new TweakDef
        {
            Id = "tpmgpo-enable-credential-guard",
            Label = "Enable Credential Guard via Device Guard Policy",
            Category = "TPM Security Policy",
            Description = "Enables Windows Defender Credential Guard through Group Policy. Credential Guard uses VBS to isolate LSA credential storage, protecting NTLM hashes and Kerberos tickets from pass-the-hash attacks. Default: 0. Recommended: 1.",
            Tags = ["tpm", "credential-guard", "vbs", "security", "lsa"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [TpmDg],
            ApplyOps   = [RegOp.SetDword(TpmDg, "LsaCfgFlags", 1)],
            RemoveOps  = [RegOp.DeleteValue(TpmDg, "LsaCfgFlags")],
            DetectOps  = [RegOp.CheckDword(TpmDg, "LsaCfgFlags", 1)],
        },
        new TweakDef
        {
            Id = "tpmgpo-enable-hvci",
            Label = "Enable Hypervisor-Protected Code Integrity (HVCI)",
            Category = "TPM Security Policy",
            Description = "Enables Hypervisor-Protected Code Integrity (HVCI / Memory Integrity) via Device Guard GPO policy. HVCI uses VBS to ensure only code signed by trusted authorities runs in kernel mode, blocking unsigned driver exploits. Default: 0. Recommended: 1.",
            Tags = ["tpm", "hvci", "memory-integrity", "vbs", "kernel", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [TpmDg],
            ApplyOps   = [RegOp.SetDword(TpmDg, "HypervisorEnforcedCodeIntegrity", 1)],
            RemoveOps  = [RegOp.DeleteValue(TpmDg, "HypervisorEnforcedCodeIntegrity")],
            DetectOps  = [RegOp.CheckDword(TpmDg, "HypervisorEnforcedCodeIntegrity", 1)],
        },
        new TweakDef
        {
            Id = "tpmgpo-enable-secure-launch",
            Label = "Enable Secure Launch (DRTM) via Device Guard",
            Category = "TPM Security Policy",
            Description = "Enables Secure Launch (Dynamic Root of Trust for Measurement / DRTM) through Device Guard GPO. DRTM provides a hardware-attested boot chain using Intel TXT or AMD Skinit, protecting firmware from being modified. Default: 0. Recommended: 1 on supported hardware.",
            Tags = ["tpm", "secure-launch", "drtm", "firmware", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [TpmDg],
            ApplyOps   = [RegOp.SetDword(TpmDg, "ConfigureSystemGuardLaunch", 1)],
            RemoveOps  = [RegOp.DeleteValue(TpmDg, "ConfigureSystemGuardLaunch")],
            DetectOps  = [RegOp.CheckDword(TpmDg, "ConfigureSystemGuardLaunch", 1)],
        },
        new TweakDef
        {
            Id = "tpmgpo-enable-vbs",
            Label = "Enable Virtualization-Based Security (VBS)",
            Category = "TPM Security Policy",
            Description = "Enables Virtualization-Based Security (VBS) through Device Guard GPO. VBS uses the hypervisor to create an isolated memory region for security code — prerequisite for Credential Guard and HVCI. Default: 0. Recommended: 1.",
            Tags = ["tpm", "vbs", "virtualization", "security", "hypervisor"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [TpmDg],
            ApplyOps   = [RegOp.SetDword(TpmDg, "EnableVirtualizationBasedSecurity", 1)],
            RemoveOps  = [RegOp.DeleteValue(TpmDg, "EnableVirtualizationBasedSecurity")],
            DetectOps  = [RegOp.CheckDword(TpmDg, "EnableVirtualizationBasedSecurity", 1)],
        },
    ];
}
