#nullable enable
using System.Collections.Generic;
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

// Sprint 255 — Early Launch Anti-Malware (ELAM) Policy (10 tweaks)
// Keys: HKLM\SYSTEM\CurrentControlSet\Policies\EarlyLaunch
//       HKLM\SOFTWARE\Policies\Microsoft\Windows\DeviceGuard
//       HKLM\SYSTEM\CurrentControlSet\Control\EarlyLaunch
internal static class EarlyLaunchAMPolicy
{
    private const string ElaKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Policies\EarlyLaunch";
    private const string ElaCtrl = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\EarlyLaunch";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "elam-set-driver-init-policy-good",
            Label = "ELAM: Allow Only Known-Good Drivers at Boot",
            Category = "Early Launch Anti-Malware Policy",
            Description =
                "Sets DriverLoadPolicy=1 in the EarlyLaunch Policies key. "
                + "Instructs Windows Boot Manager to load ONLY drivers rated as 'Good' by the ELAM driver during boot. "
                + "Drivers classified as 'Unknown' or 'Bad' are blocked from initialising, giving the strongest "
                + "pre-OS boot protection. "
                + "Values: 1=Good only, 3=Good+Unknown, 7=Good+Unknown+Bad(BootCritical), 0=Unknown+Bad all. "
                + "Default: 3 (Good+Unknown). Recommended: 1 for maximum hardening.",
            Tags = ["elam", "boot", "driver", "malware", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 3,
            ImpactNote = "Only ELAM-rated 'Good' boot drivers are loaded; unknown third-party drivers may be blocked at boot.",
            ApplyOps = [RegOp.SetDword(ElaKey, "DriverLoadPolicy", 1)],
            RemoveOps = [RegOp.DeleteValue(ElaKey, "DriverLoadPolicy")],
            DetectOps = [RegOp.CheckDword(ElaKey, "DriverLoadPolicy", 1)],
        },
        new TweakDef
        {
            Id = "elam-set-driver-init-policy-good-unknown",
            Label = "ELAM: Allow Good and Unknown Drivers at Boot",
            Category = "Early Launch Anti-Malware Policy",
            Description =
                "Sets DriverLoadPolicy=3 in the EarlyLaunch Policies key. "
                + "Instructs Windows Boot Manager to load drivers rated as 'Good' or 'Unknown' by the ELAM driver. "
                + "This is the Windows default and appropriate for most systems — blocks only definitively 'Bad' drivers. "
                + "Recommended over DriverLoadPolicy=1 when third-party boot-start drivers (e.g., storage controllers) "
                + "are present that have not yet received an ELAM classification. "
                + "Default: 3. Recommended: 3 as a balanced baseline.",
            Tags = ["elam", "boot", "driver", "balanced", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Boot drivers rated 'Good' or 'Unknown' are loaded; only 'Bad'-rated drivers are blocked.",
            ApplyOps = [RegOp.SetDword(ElaKey, "DriverLoadPolicy", 3)],
            RemoveOps = [RegOp.DeleteValue(ElaKey, "DriverLoadPolicy")],
            DetectOps = [RegOp.CheckDword(ElaKey, "DriverLoadPolicy", 3)],
        },
        new TweakDef
        {
            Id = "elam-set-driver-init-critical-only",
            Label = "ELAM: Allow Good + Unknown + Bad-Critical Drivers",
            Category = "Early Launch Anti-Malware Policy",
            Description =
                "Sets DriverLoadPolicy=7 in the EarlyLaunch Policies key. "
                + "Allows boot drivers rated 'Good', 'Unknown', and even 'Bad' if they are marked as "
                + "boot-critical (system would not boot without them). Provides compatibility for legacy "
                + "hardware with drivers that ELAM cannot classify. "
                + "Appropriate only when DriverLoadPolicy=3 causes hardware failures. "
                + "Default: not set. Recommended: use only if 3 causes boot failures.",
            Tags = ["elam", "boot", "driver", "compatibility", "legacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 3,
            ImpactNote = "Boot-critical bad-rated drivers are allowed; compatibility maximised, security reduced.",
            ApplyOps = [RegOp.SetDword(ElaKey, "DriverLoadPolicy", 7)],
            RemoveOps = [RegOp.DeleteValue(ElaKey, "DriverLoadPolicy")],
            DetectOps = [RegOp.CheckDword(ElaKey, "DriverLoadPolicy", 7)],
        },
        new TweakDef
        {
            Id = "elam-disable-elam-driver",
            Label = "Disable Early Launch Anti-Malware Driver",
            Category = "Early Launch Anti-Malware Policy",
            Description =
                "Sets DisableElam=1 in the EarlyLaunch control key. "
                + "Disables the Windows Early Launch Anti-Malware driver entirely, removing pre-boot "
                + "driver classification. Not recommended for production systems — use only when the "
                + "ELAM driver conflicts with specific virtualisation or firmware configurations. "
                + "Default: absent (ELAM enabled). Setting 1 disables ELAM protection.",
            Tags = ["elam", "boot", "disable", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = false,
            MinBuild = 19041,
            ImpactScore = 3,
            SafetyRating = 2,
            ImpactNote = "ELAM boot protection fully disabled; no pre-boot driver classification or blocking.",
            ApplyOps = [RegOp.SetDword(ElaCtrl, "DisableElam", 1)],
            RemoveOps = [RegOp.DeleteValue(ElaCtrl, "DisableElam")],
            DetectOps = [RegOp.CheckDword(ElaCtrl, "DisableElam", 1)],
        },
        new TweakDef
        {
            Id = "elam-set-scan-timeout-increased",
            Label = "Increase ELAM Scan Timeout",
            Category = "Early Launch Anti-Malware Policy",
            Description =
                "Sets ElamDriverTimeout=30000 (30 seconds) in the EarlyLaunch Policies key. "
                + "Sets the maximum time in milliseconds the Windows Boot Manager waits for the ELAM "
                + "driver to scan and classify a boot-start driver before treating it as 'Unknown'. "
                + "Default: absent (default ~0.5–2 seconds). "
                + "Increase when ELAM scanning of large or complex drivers causes boot timeouts.",
            Tags = ["elam", "boot", "timeout", "scanning", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "ELAM scan timeout increased to 30 s; useful for machines with many heavy boot drivers.",
            ApplyOps = [RegOp.SetDword(ElaKey, "ElamDriverTimeout", 30000)],
            RemoveOps = [RegOp.DeleteValue(ElaKey, "ElamDriverTimeout")],
            DetectOps = [RegOp.CheckDword(ElaKey, "ElamDriverTimeout", 30000)],
        },
        new TweakDef
        {
            Id = "elam-enable-elam-event-logging",
            Label = "Enable ELAM Boot Classification Event Logging",
            Category = "Early Launch Anti-Malware Policy",
            Description =
                "Sets EnableEventLogging=1 in the EarlyLaunch Policies key. "
                + "Instructs the ELAM subsystem to log each boot-driver classification decision to "
                + "the Windows Event Log (Microsoft-Windows-EarlyLaunch channel) after boot. "
                + "Provides an audit trail of which drivers were allowed, blocked, or classified unknown. "
                + "Default: absent (no event logging). Recommended: 1 in security-audited environments.",
            Tags = ["elam", "logging", "audit", "event-log", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "ELAM boot driver classification decisions logged to the Windows Event Log.",
            ApplyOps = [RegOp.SetDword(ElaKey, "EnableEventLogging", 1)],
            RemoveOps = [RegOp.DeleteValue(ElaKey, "EnableEventLogging")],
            DetectOps = [RegOp.CheckDword(ElaKey, "EnableEventLogging", 1)],
        },
        new TweakDef
        {
            Id = "elam-block-unknown-boot-drivers",
            Label = "Block 'Unknown' Boot Drivers via ELAM Heuristics",
            Category = "Early Launch Anti-Malware Policy",
            Description =
                "Sets TreatUnknownAsGood=0 in the EarlyLaunch Policies key. "
                + "Overrides the default ELAM heuristic that treats unclassified ('Unknown') boot drivers "
                + "as safe to load. Setting 0 instructs ELAM to be conservative: unclassified drivers "
                + "are treated as potentially bad, not good. Increases protection at the cost of possible "
                + "compatibility issues with lesser-known driver packages. "
                + "Default: 1 (unknown=good). Recommended: 0 for hardened servers.",
            Tags = ["elam", "unknown", "heuristics", "boot", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            ImpactNote = "Unknown boot drivers treated as potentially malicious by ELAM; may block unrecognised hardware at boot.",
            ApplyOps = [RegOp.SetDword(ElaKey, "TreatUnknownAsGood", 0)],
            RemoveOps = [RegOp.DeleteValue(ElaKey, "TreatUnknownAsGood")],
            DetectOps = [RegOp.CheckDword(ElaKey, "TreatUnknownAsGood", 0)],
        },
        new TweakDef
        {
            Id = "elam-enable-network-elam",
            Label = "Enable Network ELAM Protection",
            Category = "Early Launch Anti-Malware Policy",
            Description =
                "Sets EnableNetworkELAM=1 in the EarlyLaunch Policies key. "
                + "Activates the Network ELAM extension that classifies network driver stack components "
                + "(NDIS miniport, filter, and protocol drivers) during the early launch phase. "
                + "Provides pre-OS-network protection before traditional antivirus can initialise. "
                + "Default: absent. Recommended: 1 on systems with network security requirements.",
            Tags = ["elam", "network", "ndis", "drivers", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Network stack drivers (NDIS) classified by ELAM during boot; malicious network drivers blocked.",
            ApplyOps = [RegOp.SetDword(ElaKey, "EnableNetworkELAM", 1)],
            RemoveOps = [RegOp.DeleteValue(ElaKey, "EnableNetworkELAM")],
            DetectOps = [RegOp.CheckDword(ElaKey, "EnableNetworkELAM", 1)],
        },
        new TweakDef
        {
            Id = "elam-enable-measured-boot",
            Label = "Enable Windows Measured Boot Attestation",
            Category = "Early Launch Anti-Malware Policy",
            Description =
                "Sets EnableMeasuredBoot=1 in the EarlyLaunch Policies key. "
                + "Activates Windows Measured Boot, which records boot measurements (PCR values) "
                + "in the system TPM for each boot phase, including the ELAM driver's assessments. "
                + "Enables remote attestation of the boot sequence for Device Health Attestation services. "
                + "Default: absent. Recommended: 1 on TPM-equipped machines in zero-trust environments.",
            Tags = ["elam", "measured-boot", "tpm", "attestation", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Windows Measured Boot enabled; boot PCR values stored in TPM for remote attestation.",
            ApplyOps = [RegOp.SetDword(ElaKey, "EnableMeasuredBoot", 1)],
            RemoveOps = [RegOp.DeleteValue(ElaKey, "EnableMeasuredBoot")],
            DetectOps = [RegOp.CheckDword(ElaKey, "EnableMeasuredBoot", 1)],
        },
        new TweakDef
        {
            Id = "elam-enable-boot-log-persistence",
            Label = "Persist ELAM Boot Log Across Reboots",
            Category = "Early Launch Anti-Malware Policy",
            Description =
                "Sets PersistBootLog=1 in the EarlyLaunch Policies key. "
                + "Enables persistence of the ELAM boot log across reboots, allowing security tools "
                + "and the antimalware service to review prior boot classifications even after subsequent "
                + "restarts. Assists forensic analysis of boot-time driver activity. "
                + "Default: absent (log cleared after each boot). Recommended: 1 in forensic/IR environments.",
            Tags = ["elam", "logging", "persistence", "forensics", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "ELAM boot classification log persisted across reboots for forensic and audit access.",
            ApplyOps = [RegOp.SetDword(ElaKey, "PersistBootLog", 1)],
            RemoveOps = [RegOp.DeleteValue(ElaKey, "PersistBootLog")],
            DetectOps = [RegOp.CheckDword(ElaKey, "PersistBootLog", 1)],
        },
    ];
}
