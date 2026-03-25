// RegiLattice.Core — Tweaks/CbsUpdatePolicy.cs
// Sprint 346: CBS Update Policy tweaks (10 tweaks)
// Category: "CBS Update Policy" | Slug: cbsupd
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CBS

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class CbsUpdatePolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CBS";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "cbsupd-enable-auto-repair",
            Label = "Enable Automatic Component-Based Servicing Repair",
            Category = "CBS Update Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Component-Based Servicing (CBS) is the Windows component store infrastructure that manages OS component installation, updates, and repairs through the DISM subsystem. Enabling automatic CBS repair ensures that corrupted or missing system components are automatically detected and repaired from the Windows component store without manual intervention. CBS corruption can prevent Windows Update from installing updates and security patches creating security vulnerabilities from missed patching cycles. Automatic repair through CBS uses the component manifest store to verify component integrity and restore damaged components to their correct state. Organizations should enable automatic CBS repair to ensure that system component corruption does not cause persistent patching failures or security gaps. CBS repair events are logged in the CBS.log file which should be reviewed during system health checks to identify recurring repair needs.",
            Tags = ["cbs", "component-repair", "system-integrity", "update", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AutomaticRepair", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AutomaticRepair")],
            DetectOps = [RegOp.CheckDword(Key, "AutomaticRepair", 1)],
        },
        new TweakDef
        {
            Id = "cbsupd-enforce-component-hash-verification",
            Label = "Enforce Cryptographic Hash Verification for CBS Components",
            Category = "CBS Update Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            Description =
                "CBS component hash verification validates the cryptographic hash of each system component against the component manifest preventing installation of tampered or corrupted components. Enforcing hash verification for CBS operations ensures that only genuine Microsoft-signed components are installed as part of servicing operations. Component hash bypass attacks attempt to install modified system files by manipulating the CBS manifest or hash database to accept attacker-controlled components. CBS hash verification provides a layer of protection against supply chain attacks that attempt to replace legitimate system files with backdoored versions. Organizations should ensure that CBS integrity checking is enabled and that the component store hash database has not been modified through monitoring. CBS hash verification failures generate events in the CBS.log that should be treated as high-severity alerts indicating potential system tampering.",
            Tags = ["cbs", "hash-verification", "integrity", "supply-chain", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnforceHashVerification", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnforceHashVerification")],
            DetectOps = [RegOp.CheckDword(Key, "EnforceHashVerification", 1)],
        },
        new TweakDef
        {
            Id = "cbsupd-restrict-cbs-offline-servicing",
            Label = "Restrict CBS Offline Servicing to Authorized Administrators",
            Category = "CBS Update Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "CBS offline servicing allows modification of Windows component store contents from offline boot environments which is a powerful capability that can be used to bypass OS-level security controls. Restricting CBS offline servicing to authorized administrators prevents unauthorized use of offline tools to modify system components outside the normal OS boot environment. BitLocker full-disk encryption is the primary defense against offline servicing attacks as it prevents booting from external media to access the encrypted drive. Organizations running Secure Boot with TPM-based integrity measurement provide additional protection against offline servicing attacks by detecting changes to the boot environment. CBS offline servicing is a legitimate maintenance capability used for repair scenarios but should be restricted through physical security and encryption rather than software policy alone. Organizations should include CBS offline servicing in their threat model and ensure physical security controls prevent unauthorized access to servers.",
            Tags = ["cbs", "offline-servicing", "admin-restriction", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictOfflineServicing", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictOfflineServicing")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictOfflineServicing", 1)],
        },
        new TweakDef
        {
            Id = "cbsupd-enable-cbs-cleanup-scheduled",
            Label = "Enable Scheduled Cleanup of Superseded CBS Components",
            Category = "CBS Update Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "The Windows component store retains superseded component versions after updates to support rollback capability but accumulates significant disk space over time. Enabling scheduled CBS cleanup removes superseded components after a defined retention period freeing disk space while retaining recent versions for rollback. System drives running close to capacity due to component store accumulation can cause update failures when insufficient space exists for patch installation. The DISM cleanup task removes components that can no longer be uninstalled based on the uninstall window policy reducing disk usage by 10-20% on long-running systems. Organizations should balance component store cleanup with rollback requirements as aggressive cleanup prevents rolling back recent updates if problems are discovered. WSFC clusters during CBS cleanup and fail-safe mechanisms prevent critical system failures due to premature component removal.",
            Tags = ["cbs", "cleanup", "disk-space", "maintenance", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "ScheduledCleanup", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "ScheduledCleanup")],
            DetectOps = [RegOp.CheckDword(Key, "ScheduledCleanup", 1)],
        },
        new TweakDef
        {
            Id = "cbsupd-enforce-manifest-signing",
            Label = "Enforce Digital Signature on CBS Component Manifests",
            Category = "CBS Update Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            Description =
                "CBS component manifests describe the contents and attributes of system components and are signed by Microsoft to ensure their integrity and prevent modification. Enforcing manifest signature verification ensures that modified manifests that attempt to introduce malicious components or disable security features are rejected. Component manifest signing is part of the Windows Trusted Installer infrastructure that protects system file integrity against unauthorized modification. Manifests that have been tampered with to override hash values or add unauthorized components will be rejected when signature enforcement is active. Manifest signature enforcement is a defense-in-depth measure complementing Windows Resource Protection (WRP) and other component store integrity mechanisms. Organizations should treat CBS manifest signature verification failures as critical security events indicating potential kernel-level or bootkit-level compromise.",
            Tags = ["cbs", "manifest-signing", "code-signing", "integrity", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnforceManifestSigning", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnforceManifestSigning")],
            DetectOps = [RegOp.CheckDword(Key, "EnforceManifestSigning", 1)],
        },
        new TweakDef
        {
            Id = "cbsupd-enable-cbs-verbose-logging",
            Label = "Enable Verbose CBS Logging for Update Failure Diagnostics",
            Category = "CBS Update Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "CBS verbose logging captures detailed information about servicing operations including component installations, updates, and failures in the CBS.log file for troubleshooting. Enabling verbose CBS logging provides the detailed diagnostic data needed to identify root causes of Windows Update failures that may indicate security patching gaps. CBS.log is typically several hundred megabytes to gigabytes in size with verbose logging and should be captured and analyzed as part of update compliance monitoring. Update failures identified through CBS verbose logging should be cross-referenced with security vulnerability databases to prioritize remediation of security-relevant failures. Organizations with update compliance requirements should monitor CBS logs for persistent failures that indicate systems are not receiving security patches. Verbose CBS logging helps distinguish between installation failures caused by disk space, compatibility, corruption, or other factors to inform targeted remediation.",
            Tags = ["cbs", "verbose-logging", "diagnostics", "update-compliance", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "VerboseLogging", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "VerboseLogging")],
            DetectOps = [RegOp.CheckDword(Key, "VerboseLogging", 1)],
        },
        new TweakDef
        {
            Id = "cbsupd-set-cbs-store-health-check-interval",
            Label = "Set Scheduled Interval for CBS Component Store Health Verification",
            Category = "CBS Update Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "CBS component store health checks verify the integrity of the Windows component store by comparing installed component hashes against the reference values in the component manifest. Setting regular health check intervals ensures that component store corruption is detected promptly before it leads to update failures or security vulnerabilities. Component store corruption can occur due to disk errors, unexpected shutdowns, or malware modification of system files. Regular health verification similar to running DISM /CheckHealth provides ongoing assurance that the system components match their expected values. Health check interval policies complement automatic repair by detecting corruption early before it causes operational problems. Organizations should define health check intervals based on their risk posture with more frequent checks for high-security systems and critical infrastructure.",
            Tags = ["cbs", "health-check", "component-integrity", "maintenance", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "HealthCheckInterval", 7)],
            RemoveOps = [RegOp.DeleteValue(Key, "HealthCheckInterval")],
            DetectOps = [RegOp.CheckDword(Key, "HealthCheckInterval", 7)],
        },
        new TweakDef
        {
            Id = "cbsupd-block-unsigned-packages",
            Label = "Block Installation of Unsigned or Untrusted CBS Packages",
            Category = "CBS Update Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            Description =
                "CBS package signature verification ensures that only packages signed by trusted certificate authorities including Microsoft and hardware vendors can be installed through the CBS servicing infrastructure. Blocking unsigned CBS packages prevents installation of tampered or third-party packages that could introduce vulnerabilities or backdoors into the system component store. Unsigned packages submitted to CBS represent a significant threat vector for supply chain attacks where unauthorized components are installed as system components. CBS package signature enforcement should apply to both online and offline servicing operations to prevent bypass through offline tools. Organizations running Windows Server should audit the custom packages installed through CBS to identify any unsigned or questionable packages in the component store. CBS signature enforcement is complementary to Windows code signing policies and should be aligned with the organization's overall application trust model.",
            Tags = ["cbs", "unsigned-packages", "code-signing", "supply-chain", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "BlockUnsignedPackages", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockUnsignedPackages")],
            DetectOps = [RegOp.CheckDword(Key, "BlockUnsignedPackages", 1)],
        },
        new TweakDef
        {
            Id = "cbsupd-restrict-cbs-to-trusted-sources",
            Label = "Restrict CBS Package Sources to Microsoft Update and WSUS Only",
            Category = "CBS Update Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "CBS package source restriction limits where the CBS servicing infrastructure can obtain component packages to Microsoft Update or organizational WSUS servers. Restricting CBS to trusted sources prevents the use of arbitrary package sources that could deliver malicious components masked as system updates. Third-party package sources for CBS are rarely needed in enterprise environments where updates are managed through WSUS or Configuration Manager. Source restriction for CBS complements Windows Update source restrictions to create a consistent update trust chain from Microsoft to the endpoint. Organizations should configure both Windows Update and CBS source policies together to ensure coherent update supply chain protection. Audit CB package installation events to detect any packages sourced from unexpected origins that may indicate a source restriction bypass.",
            Tags = ["cbs", "trusted-sources", "update-chain", "wsus", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictToTrustedSources", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictToTrustedSources")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictToTrustedSources", 1)],
        },
        new TweakDef
        {
            Id = "cbsupd-enable-servicing-stack-updates-priority",
            Label = "Enable Priority Installation of Servicing Stack Updates",
            Category = "CBS Update Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Servicing Stack Updates (SSUs) update the foundational CBS infrastructure itself and must be installed before cumulative updates that depend on the updated servicing stack. Enabling priority installation of SSUs ensures that the servicing stack is always current before applying other updates preventing installation failures from an outdated stack. Outdated servicing stacks are a common cause of Windows Update failure where cumulative updates cannot be installed because they require SSU capabilities not yet present. SSU prioritization is implemented in Windows 10 1903 and later through the Unified Update Platform that automatically handles SSU installation order. Organizations running older Windows versions should prioritize SSU installation in their WSUS or Configuration Manager patch deployment groups. Servicing stack currency is a prerequisite for comprehensive security patching and should be verified during update compliance audits.",
            Tags = ["cbs", "servicing-stack", "update-priority", "patching", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "PrioritizeServicingStackUpdates", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "PrioritizeServicingStackUpdates")],
            DetectOps = [RegOp.CheckDword(Key, "PrioritizeServicingStackUpdates", 1)],
        },
    ];
}
