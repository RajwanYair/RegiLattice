// RegiLattice.Core — Tweaks/WslDistroManagementPolicy.cs
// WSL distribution installation and lifecycle management Group Policy controls (Sprint 607).
// Category: "WSL Distro Management Policy" | Slug: wsldist
// Key: HKLM\SOFTWARE\Policies\Microsoft\Windows\Lxss\Distros

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WslDistroManagementPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Lxss\Distros";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "wsldist-block-external-distro-sources",
            Label = "WSL Distro: Block Installation of Unverified External Distros",
            Category = "WSL Distro Management Policy",
            Description = "Sets AllowOnlyApprovedDistributions=1 in Lxss Distros policy. Restricts WSL distro installation to the set of distributions approved in this enterprise policy, blocking users from installing unverified third-party Linux distributions. " +
                "Third-party WSL distros installed from .tar.gz archives or custom OCI images bypass the Microsoft Store signing process, are not subject to Windows Defender malware scanning during import, and may include custom kernel modules or services that establish network connections to external command-and-control infrastructure. Restricting to approved distributions ensures all WSL environments meet the organisation's security baseline.",
            Tags = ["wsl", "distro", "installation", "security", "enterprise"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Only approved WSL distros installable; blocks arbitrary Linux environments from unverified sources.",
            ApplyOps = [RegOp.SetDword(Key, "AllowOnlyApprovedDistributions", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowOnlyApprovedDistributions")],
            DetectOps = [RegOp.CheckDword(Key, "AllowOnlyApprovedDistributions", 1)],
        },
        new TweakDef
        {
            Id = "wsldist-disable-distro-export",
            Label = "WSL Distro: Disable Distro Export to External Archive",
            Category = "WSL Distro Management Policy",
            Description = "Sets DisableDistributionExport=1 in Lxss Distros policy. Prevents users from running 'wsl --export' to create .tar.gz archives of installed WSL distributions. " +
                "Exporting a WSL distro creates a portable archive of the entire Linux file system — including any data, credentials, keys, or sensitive files stored within the Linux home directory. This archive can then be transferred to an unmanaged device. Blocking distro export prevents the Linux container's data from being extracted and exfiltrated outside the managed device.",
            Tags = ["wsl", "distro", "export", "data-exfiltration", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Blocks WSL distro export; Linux container filesystem cannot be archived and transferred off-device.",
            ApplyOps = [RegOp.SetDword(Key, "DisableDistributionExport", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableDistributionExport")],
            DetectOps = [RegOp.CheckDword(Key, "DisableDistributionExport", 1)],
        },
        new TweakDef
        {
            Id = "wsldist-disable-distro-import",
            Label = "WSL Distro: Disable Distro Import from External Archive",
            Category = "WSL Distro Management Policy",
            Description = "Sets DisableDistributionImport=1 in Lxss Distros policy. Prevents users from running 'wsl --import' to install a custom Linux distribution from a .tar.gz or OCI container archive. " +
                "Importing a custom WSL distribution bypasses all Microsoft Store distribution vetting. An attacker who has compromised a development machine can create a custom Linux distro archive with embedded persistence mechanisms, additional network listeners, or credential theft tooling, then import it on other machines using only standard user 'wsl' CLI commands. Disabling import forces all WSL distro installations through the Store pipeline.",
            Tags = ["wsl", "distro", "import", "lateral-movement", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Blocks WSL custom distro import; all distributions must come from the Microsoft Store.",
            ApplyOps = [RegOp.SetDword(Key, "DisableDistributionImport", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableDistributionImport")],
            DetectOps = [RegOp.CheckDword(Key, "DisableDistributionImport", 1)],
        },
        new TweakDef
        {
            Id = "wsldist-set-max-distros-allowed-2",
            Label = "WSL Distro: Limit Maximum Installed Distros to 2",
            Category = "WSL Distro Management Policy",
            Description = "Sets MaxDistributionsAllowed=2 in Lxss Distros policy. Caps the number of WSL distributions that a user can have installed simultaneously to 2. " +
                "Each installed WSL distribution adds to the attack surface: an additional Linux kernel, an additional network-accessible file system, and an additional set of Linux packages that may have known CVEs. Limiting users to 2 simultaneous distros (e.g., one primary development environment and one for testing) reduces this footprint while still supporting legitimate multi-environment development workflows.",
            Tags = ["wsl", "distro", "limit", "attack-surface", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Max 2 WSL distros; limits Linux environment proliferation on managed devices.",
            ApplyOps = [RegOp.SetDword(Key, "MaxDistributionsAllowed", 2)],
            RemoveOps = [RegOp.DeleteValue(Key, "MaxDistributionsAllowed")],
            DetectOps = [RegOp.CheckDword(Key, "MaxDistributionsAllowed", 2)],
        },
        new TweakDef
        {
            Id = "wsldist-require-admin-for-distro-removal",
            Label = "WSL Distro: Require Administrative Approval to Unregister Distros",
            Category = "WSL Distro Management Policy",
            Description = "Sets RequireAdminForDistributionRemoval=1 in Lxss Distros policy. Requires administrator privileges to unregister or remove a WSL distribution via 'wsl --unregister'. " +
                "If a WSL distro becomes compromised, malware running within the Linux environment may attempt to cover its tracks by unregistering the distro after data exfiltration, destroying forensic evidence. Requiring admin elevation to remove a distro ensures that Linux environment removal is a deliberate IT/admin action, not something malware inside the WSL container can trigger via WSL CLI commands.",
            Tags = ["wsl", "distro", "removal", "forensics", "admin"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Admin required to unregister WSL distros; prevents malware from destroying Linux container forensic evidence.",
            ApplyOps = [RegOp.SetDword(Key, "RequireAdminForDistributionRemoval", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireAdminForDistributionRemoval")],
            DetectOps = [RegOp.CheckDword(Key, "RequireAdminForDistributionRemoval", 1)],
        },
        new TweakDef
        {
            Id = "wsldist-disable-distro-version-downgrade",
            Label = "WSL Distro: Block Downgrading Distros to WSL 1 Mode",
            Category = "WSL Distro Management Policy",
            Description = "Sets DisableDistributionVersionDowngrade=1 in Lxss Distros policy. Prevents users from converting installed WSL 2 distributions back to WSL 1 mode via 'wsl --set-version'. " +
                "WSL 1 uses a translation layer (instead of a real Linux kernel) that is significantly more permissive in its Windows-Linux boundary enforcement. WSL 2 uses a Hyper-V lightweight VM with stronger isolation. Downgrading to WSL 1 weakens the isolation model and re-enables file system access patterns that WSL 2's VM architecture blocks, potentially creating a security regression on machines where WSL 2 was specifically required for its isolation guarantees.",
            Tags = ["wsl", "wsl1", "wsl2", "isolation", "downgrade"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Prevents WSL 2→WSL 1 downgrade; preserves Hyper-V VM isolation for all distros.",
            ApplyOps = [RegOp.SetDword(Key, "DisableDistributionVersionDowngrade", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableDistributionVersionDowngrade")],
            DetectOps = [RegOp.CheckDword(Key, "DisableDistributionVersionDowngrade", 1)],
        },
        new TweakDef
        {
            Id = "wsldist-disable-distro-backup-creation",
            Label = "WSL Distro: Disable Automatic Distro Backup Creation",
            Category = "WSL Distro Management Policy",
            Description = "Sets DisableDistributionBackup=1 in Lxss Distros policy. Prevents automatic backup snapshots of WSL distribution state from being written to the Windows user profile directory. " +
                "WSL distribution backups are compressed archives of the Linux VHD that can be several gigabytes in size. On managed devices with roaming profiles or OneDrive-synced user profiles, these large backup files are undesirably synchronised to cloud storage, consuming bandwidth and cloud quota. Additionally, backups may contain sensitive Linux-resident credentials.",
            Tags = ["wsl", "backup", "vhd", "profile", "cloud-sync"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "No automatic WSL distro backups; prevents large Linux VHD archives from consuming cloud sync quota.",
            ApplyOps = [RegOp.SetDword(Key, "DisableDistributionBackup", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableDistributionBackup")],
            DetectOps = [RegOp.CheckDword(Key, "DisableDistributionBackup", 1)],
        },
        new TweakDef
        {
            Id = "wsldist-enable-distro-audit-logging",
            Label = "WSL Distro: Enable Audit Logging for Distro Install and Remove Events",
            Category = "WSL Distro Management Policy",
            Description = "Sets EnableDistributionAuditLogging=1 in Lxss Distros policy. Enables Security Event Log entries when WSL distributions are registered (installed), unregistered (removed), or converted between WSL 1/2 modes. " +
                "Without distro lifecycle logging, there is no Security event log record of when Linux environments were created or deleted on a machine. If an attacker installs a WSL distro for lateral movement and then removes it to destroy evidence, the only forensic trace would be file system artefacts. Event log entries for distro lifecycle operations enable detection rules in SIEM systems.",
            Tags = ["wsl", "audit", "logging", "siem", "lifecycle"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Security log entries for WSL distro install/remove; enables SIEM detection of Linux environment manipulation.",
            ApplyOps = [RegOp.SetDword(Key, "EnableDistributionAuditLogging", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableDistributionAuditLogging")],
            DetectOps = [RegOp.CheckDword(Key, "EnableDistributionAuditLogging", 1)],
        },
        new TweakDef
        {
            Id = "wsldist-disable-distro-rename",
            Label = "WSL Distro: Disable User Renaming of Installed Distros",
            Category = "WSL Distro Management Policy",
            Description = "Sets DisableDistributionRename=1 in Lxss Distros policy. Prevents users from renaming installed WSL distributions via 'wsl --rename' or through the Windows registry. " +
                "Distribution names are used by monitoring tools, DLP agents, and endpoint security software to identify WSL environments and apply appropriate policies. If a user renames a restricted distribution (e.g., a distro named 'blocked-distro') to an unrestricted name, policy enforcement based on distribution identity may be bypassed. Locking distribution names preserves the integrity of name-based policy enforcement.",
            Tags = ["wsl", "distro", "rename", "policy-bypass", "identity"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Distro names locked; prevents renaming to bypass name-based policy enforcement.",
            ApplyOps = [RegOp.SetDword(Key, "DisableDistributionRename", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableDistributionRename")],
            DetectOps = [RegOp.CheckDword(Key, "DisableDistributionRename", 1)],
        },
        new TweakDef
        {
            Id = "wsldist-disable-distro-updates-without-approval",
            Label = "WSL Distro: Require Admin Approval for Distro Auto-Updates",
            Category = "WSL Distro Management Policy",
            Description = "Sets RequireAdminForDistributionUpdates=1 in Lxss Distros policy. Requires administrator approval before a WSL distribution is allowed to automatically update its base image to a newer version from the Microsoft Store. " +
                "While updating a WSL base image is generally desirable for security patch coverage, uncontrolled automatic updates can change the Linux environment's toolchain version, breaking developer builds that depend on specific library or compiler versions. Requiring admin approval gates distribution updates through change management, ensuring updates are tested before deployment.",
            Tags = ["wsl", "distro", "updates", "change-management", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "WSL distro updates require admin approval; prevents uncontrolled auto-updates breaking environment baselines.",
            ApplyOps = [RegOp.SetDword(Key, "RequireAdminForDistributionUpdates", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireAdminForDistributionUpdates")],
            DetectOps = [RegOp.CheckDword(Key, "RequireAdminForDistributionUpdates", 1)],
        },
    ];
}
