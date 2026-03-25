// RegiLattice.Core — Tweaks/SecureBootPolicy.cs
// Sprint 285: Secure Boot Group Policy (10 tweaks)
// Category: "Secure Boot Policy" | Slug: secboot
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SecureBoot

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class SecureBootPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SecureBoot";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "secboot-enable-db-update",
            Label = "Enable Secure Boot DB Update",
            Category = "Secure Boot Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 4, SafetyRating = 5,
            Description =
                "Sets AllowUpdateOfSecureBootDb=1 in the SecureBoot policy key. Permits "
                + "Windows Update to deliver updated Secure Boot Allowed (db) and "
                + "Revocation (dbx) signature databases to the UEFI firmware store. "
                + "Microsoft periodically revokes compromised boot loaders (e.g., "
                + "BlackLotus / Storm-0558) via dbx updates; blocking these updates "
                + "leaves the device vulnerable to known bootkit exploits. "
                + "Default: 1. Keep at 1 (enabled).",
            Tags = ["secureboot", "db", "dbx", "uefi", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AllowUpdateOfSecureBootDb", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowUpdateOfSecureBootDb")],
            DetectOps = [RegOp.CheckDword(Key, "AllowUpdateOfSecureBootDb", 1)],
        },
        new TweakDef
        {
            Id = "secboot-disable-test-signing",
            Label = "Disable Test-Signing Mode Boot",
            Category = "Secure Boot Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 5, SafetyRating = 5,
            Description =
                "Sets DisableTestSigning=1 in the SecureBoot policy key. Prevents the "
                + "system from booting with the test-signing BCD flag enabled, which "
                + "would allow loading of drivers signed with test certificates not "
                + "chained to a trusted production CA. Test-signing mode is a "
                + "common bypass technique for loading rootkits and unsigned malicious "
                + "kernel drivers. Default: 0. Recommended: 1.",
            Tags = ["secureboot", "test-signing", "drivers", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableTestSigning", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableTestSigning")],
            DetectOps = [RegOp.CheckDword(Key, "DisableTestSigning", 1)],
        },
        new TweakDef
        {
            Id = "secboot-enable-user-mode-ci",
            Label = "Enable User-Mode Code Integrity (UMCI)",
            Category = "Secure Boot Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 5, SafetyRating = 4,
            Description =
                "Sets EnableUMCI=1 in the SecureBoot policy key. Activates User-Mode "
                + "Code Integrity enforcement, ensuring that user-mode binaries and "
                + "scripts are validated against the Windows Driver Certificate policy "
                + "before execution. UMCI blocks execution of unsigned or revoked "
                + "binaries and is a key component of Windows Device Guard. "
                + "Default: 0. Recommended: 1 where an allow-list policy is in place.",
            Tags = ["secureboot", "umci", "device-guard", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableUMCI", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableUMCI")],
            DetectOps = [RegOp.CheckDword(Key, "EnableUMCI", 1)],
        },
        new TweakDef
        {
            Id = "secboot-enable-kernel-ci",
            Label = "Enable Kernel Code Integrity Enforcement",
            Category = "Secure Boot Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 5, SafetyRating = 4,
            Description =
                "Sets EnableKernelCI=1 in the SecureBoot policy key. Enforces that only "
                + "WHQL or EV-code-signed kernel-mode drivers are permitted to load "
                + "during and after the boot sequence. Kernel code integrity blocks "
                + "unsigned, test-signed, and revoked drivers including legacy "
                + "rootkits and BYOVD (Bring Your Own Vulnerable Driver) techniques "
                + "that attackers use as privilege-escalation primitives. Default: 0.",
            Tags = ["secureboot", "kernel-ci", "hvci", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableKernelCI", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableKernelCI")],
            DetectOps = [RegOp.CheckDword(Key, "EnableKernelCI", 1)],
        },
        new TweakDef
        {
            Id = "secboot-enable-secinitrd",
            Label = "Enable Secure Initial Ramdisk (secInitrd)",
            Category = "Secure Boot Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 4, SafetyRating = 5,
            Description =
                "Sets EnableSecInitRd=1 in the SecureBoot policy key. Requires the "
                + "Windows Recovery Environment and boot-critical drivers compressed "
                + "into the initial ramdisk to be signed and validated by the bootmgr "
                + "before they are decompressed and executed. An unsigned initial "
                + "ramdisk is a pre-OS persistence vector for bootkits that cannot "
                + "survive signed ramdisk validation. Default: 0.",
            Tags = ["secureboot", "initrd", "winre", "boot", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableSecInitRd", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableSecInitRd")],
            DetectOps = [RegOp.CheckDword(Key, "EnableSecInitRd", 1)],
        },
        new TweakDef
        {
            Id = "secboot-disable-network-unlock",
            Label = "Disable Secure Boot Network Unlock",
            Category = "Secure Boot Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
            Description =
                "Sets DisableNetworkUnlock=1 in the SecureBoot policy key. Prevents "
                + "BitLocker Network Unlock from releasing the volume encryption key "
                + "based solely on network presence of a WDS/NPS unlock server, "
                + "without any user-interactive PIN or key protector challenge. "
                + "Network Unlock convenience can undermine pre-boot authentication "
                + "goals if an attacker clones the network identity. Default: 0.",
            Tags = ["secureboot", "network-unlock", "bitlocker", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableNetworkUnlock", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableNetworkUnlock")],
            DetectOps = [RegOp.CheckDword(Key, "DisableNetworkUnlock", 1)],
        },
        new TweakDef
        {
            Id = "secboot-enforce-secure-mos-policy",
            Label = "Enforce Secure Managed OS Policy",
            Category = "Secure Boot Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 4, SafetyRating = 4,
            Description =
                "Sets EnforceManagedOsPolicy=1 in the SecureBoot policy key. Requires "
                + "that the device's Secure Boot configuration matches a defined managed "
                + "OS policy baseline, which includes allowed boot-path signatures, "
                + "revocation list freshness, and HVCI/VBS state. Deviation from the "
                + "policy baseline marks the device as out-of-compliance for conditional "
                + "access gatekeeping. Default: 0.",
            Tags = ["secureboot", "managed-os", "compliance", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnforceManagedOsPolicy", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnforceManagedOsPolicy")],
            DetectOps = [RegOp.CheckDword(Key, "EnforceManagedOsPolicy", 1)],
        },
        new TweakDef
        {
            Id = "secboot-disable-custom-pk",
            Label = "Disable Custom Platform Key Enrollment",
            Category = "Secure Boot Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 4, SafetyRating = 5,
            Description =
                "Sets DisableCustomPk=1 in the SecureBoot policy key. Prevents users "
                + "or local administrators from replacing the UEFI Platform Key (PK) "
                + "with a custom or self-signed certificate, which would allow the "
                + "installation of a custom Secure Boot database permitting arbitrary "
                + "unsigned boot-path code. Custom PK enrollment is the first step in "
                + "most bootkit persistence chains on managed devices. Default: 0.",
            Tags = ["secureboot", "platform-key", "pk", "uefi", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableCustomPk", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableCustomPk")],
            DetectOps = [RegOp.CheckDword(Key, "DisableCustomPk", 1)],
        },
        new TweakDef
        {
            Id = "secboot-require-bootloader-revocation",
            Label = "Require Bootloader Revocation Check",
            Category = "Secure Boot Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 4, SafetyRating = 5,
            Description =
                "Sets RequireBootloaderRevocationCheck=1 in the SecureBoot policy key. "
                + "Forces the Windows boot manager to verify the dbx (Forbidden "
                + "Signature Database) revocation list before launching the OS loader, "
                + "blocking boot loaders that have been revoked due to known "
                + "vulnerabilities (e.g., old signed shim binaries used in BlackLotus "
                + "and similar bootkit campaigns). Default: 0. Recommended: 1.",
            Tags = ["secureboot", "revocation", "dbx", "bootloader", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RequireBootloaderRevocationCheck", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireBootloaderRevocationCheck")],
            DetectOps = [RegOp.CheckDword(Key, "RequireBootloaderRevocationCheck", 1)],
        },
        new TweakDef
        {
            Id = "secboot-disable-secure-boot-telemetry",
            Label = "Disable Secure Boot Telemetry",
            Category = "Secure Boot Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets DisableSecureBootTelemetry=1 in the SecureBoot policy key. Stops "
                + "the Windows boot manager and Secure Boot runtime from emitting "
                + "telemetry events that report db/dbx database versions, UEFI firmware "
                + "vendor, and Secure Boot validation outcomes to Microsoft's telemetry "
                + "pipeline. Firmware vendor identifiers and signing certificate "
                + "thumbprints constitute device fingerprint data. Default: 0.",
            Tags = ["secureboot", "telemetry", "privacy", "uefi", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableSecureBootTelemetry", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableSecureBootTelemetry")],
            DetectOps = [RegOp.CheckDword(Key, "DisableSecureBootTelemetry", 1)],
        },
    ];
}
