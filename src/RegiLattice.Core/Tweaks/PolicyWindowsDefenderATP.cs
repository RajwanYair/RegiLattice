namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PolicyWindowsDefenderATP
{
    private const string AtpKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Advanced Threat Protection";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "atp-disable-onboarding-ui",
            Label = "Disable ATP Onboarding UI Prompt",
            Category = "Defender ATP Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets ForceDefenderPassiveMode=1 in Windows Advanced Threat Protection policy. "
                + "Prevents Windows Defender ATP from showing onboarding prompts and forces the sensor "
                + "into passive mode, which is useful in environments using a third-party EDR solution.",
            Tags = ["atp", "defender", "edr", "onboarding", "policy"],
            RegistryKeys = [AtpKey],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "ATP sensor enters passive mode; third-party EDR takes precedence.",
            ApplyOps = [RegOp.SetDword(AtpKey, "ForceDefenderPassiveMode", 1)],
            RemoveOps = [RegOp.DeleteValue(AtpKey, "ForceDefenderPassiveMode")],
            DetectOps = [RegOp.CheckDword(AtpKey, "ForceDefenderPassiveMode", 1)],
        },
        new TweakDef
        {
            Id = "atp-disable-sample-upload",
            Label = "Disable ATP Sample Upload",
            Category = "Defender ATP Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets AllowSampleCollection=0 in Windows Advanced Threat Protection policy. "
                + "Prevents the ATP sensor from uploading suspicious file samples to Microsoft for "
                + "cloud analysis. Reduces data exfiltration risk in sensitive environments.",
            Tags = ["atp", "defender", "sample", "upload", "privacy", "policy"],
            RegistryKeys = [AtpKey],
            ImpactScore = 4,
            SafetyRating = 3,
            ImpactNote = "No suspicious samples sent to Microsoft; cloud-based detection reduced.",
            ApplyOps = [RegOp.SetDword(AtpKey, "AllowSampleCollection", 0)],
            RemoveOps = [RegOp.DeleteValue(AtpKey, "AllowSampleCollection")],
            DetectOps = [RegOp.CheckDword(AtpKey, "AllowSampleCollection", 0)],
        },
        new TweakDef
        {
            Id = "atp-disable-telemetry-reporting",
            Label = "Disable ATP Telemetry Reporting",
            Category = "Defender ATP Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets TelemetryProxyServer=\"\" (empty) in Windows Advanced Threat Protection policy. "
                + "Clears the ATP telemetry proxy server, effectively disabling ATP telemetry forwarding "
                + "in air-gapped or restricted environments where outbound telemetry is not permitted.",
            Tags = ["atp", "defender", "telemetry", "proxy", "policy"],
            RegistryKeys = [AtpKey],
            ImpactScore = 3,
            SafetyRating = 3,
            ImpactNote = "ATP telemetry proxy cleared; sensor cannot forward telemetry to MSFT cloud.",
            ApplyOps = [RegOp.SetString(AtpKey, "TelemetryProxyServer", "")],
            RemoveOps = [RegOp.DeleteValue(AtpKey, "TelemetryProxyServer")],
            DetectOps = [RegOp.CheckString(AtpKey, "TelemetryProxyServer", "")],
        },
        new TweakDef
        {
            Id = "atp-set-onboarding-state-off",
            Label = "Set ATP Onboarding State to Off",
            Category = "Defender ATP Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets OnboardingState=0 in Windows Advanced Threat Protection policy. "
                + "Marks the endpoint as not onboarded to Defender ATP. Useful for preventing "
                + "unmanaged workstations from auto-enrolling into the organisation's ATP tenant.",
            Tags = ["atp", "defender", "onboarding", "state", "policy"],
            RegistryKeys = [AtpKey],
            ImpactScore = 4,
            SafetyRating = 3,
            ImpactNote = "Device is not onboarded to ATP; no EDR telemetry collected.",
            ApplyOps = [RegOp.SetDword(AtpKey, "OnboardingState", 0)],
            RemoveOps = [RegOp.DeleteValue(AtpKey, "OnboardingState")],
            DetectOps = [RegOp.CheckDword(AtpKey, "OnboardingState", 0)],
        },
        new TweakDef
        {
            Id = "atp-disable-auto-exclusions",
            Label = "Disable ATP Auto Exclusions",
            Category = "Defender ATP Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableAutoExclusions=1 in Windows Advanced Threat Protection policy. "
                + "Prevents ATP from automatically excluding server roles and features from scanning. "
                + "Forces full coverage scanning on all files, removing implicit trust in server-role paths.",
            Tags = ["atp", "defender", "exclusions", "scanning", "policy", "security"],
            RegistryKeys = [AtpKey],
            ImpactScore = 4,
            SafetyRating = 3,
            ImpactNote = "No server-role auto-exclusions; scanning is comprehensive but may increase CPU.",
            ApplyOps = [RegOp.SetDword(AtpKey, "DisableAutoExclusions", 1)],
            RemoveOps = [RegOp.DeleteValue(AtpKey, "DisableAutoExclusions")],
            DetectOps = [RegOp.CheckDword(AtpKey, "DisableAutoExclusions", 1)],
        },
        new TweakDef
        {
            Id = "atp-block-at-first-sight",
            Label = "Enable ATP Block at First Sight",
            Category = "Defender ATP Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableBlockAtFirstSeen=0 in Windows Advanced Threat Protection policy. "
                + "Ensures that Block at First Sight is enabled, allowing ATP to block unknown files "
                + "after a few seconds of cloud analysis rather than allowing them to execute.",
            Tags = ["atp", "defender", "block", "first-sight", "policy", "security"],
            RegistryKeys = [AtpKey],
            ImpactScore = 5,
            SafetyRating = 4,
            ImpactNote = "Unknown files are blocked within seconds pending cloud verdict.",
            ApplyOps = [RegOp.SetDword(AtpKey, "DisableBlockAtFirstSeen", 0)],
            RemoveOps = [RegOp.DeleteValue(AtpKey, "DisableBlockAtFirstSeen")],
            DetectOps = [RegOp.CheckDword(AtpKey, "DisableBlockAtFirstSeen", 0)],
        },
        new TweakDef
        {
            Id = "atp-set-cloud-block-timeout-60s",
            Label = "Set ATP Cloud Block Timeout to 60 Seconds",
            Category = "Defender ATP Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets CloudExtendedTimeout=50 in Windows Advanced Threat Protection policy. "
                + "Extends the cloud analysis timeout by 50 seconds (total ~60s). Gives the cloud "
                + "more time to return a verdict before releasing unknown files for execution.",
            Tags = ["atp", "defender", "cloud", "timeout", "policy"],
            RegistryKeys = [AtpKey],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Unknown files held for up to 60s pending cloud verdict; slight user delay.",
            ApplyOps = [RegOp.SetDword(AtpKey, "CloudExtendedTimeout", 50)],
            RemoveOps = [RegOp.DeleteValue(AtpKey, "CloudExtendedTimeout")],
            DetectOps = [RegOp.CheckDword(AtpKey, "CloudExtendedTimeout", 50)],
        },
        new TweakDef
        {
            Id = "atp-enable-network-protection",
            Label = "Enable ATP Network Protection (Block Mode)",
            Category = "Defender ATP Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets EnableNetworkProtection=1 in Windows Advanced Threat Protection policy. "
                + "Enables Network Protection in block mode, preventing connections to known malicious "
                + "domains, IP addresses, and URLs identified by Microsoft Threat Intelligence.",
            Tags = ["atp", "defender", "network", "protection", "policy", "security"],
            RegistryKeys = [AtpKey],
            ImpactScore = 5,
            SafetyRating = 4,
            ImpactNote = "Connections to known malicious destinations are actively blocked.",
            ApplyOps = [RegOp.SetDword(AtpKey, "EnableNetworkProtection", 1)],
            RemoveOps = [RegOp.DeleteValue(AtpKey, "EnableNetworkProtection")],
            DetectOps = [RegOp.CheckDword(AtpKey, "EnableNetworkProtection", 1)],
        },
        new TweakDef
        {
            Id = "atp-disable-data-collection-consent",
            Label = "Suppress ATP Data Collection Consent Prompt",
            Category = "Defender ATP Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableDataCollectionConsent=1 in Windows Advanced Threat Protection policy. "
                + "Suppresses the user consent prompt for ATP data collection, allowing the organisation "
                + "to centrally manage consent via Group Policy without per-user interaction.",
            Tags = ["atp", "defender", "consent", "data-collection", "policy"],
            RegistryKeys = [AtpKey],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "ATP consent prompts suppressed; consent managed centrally.",
            ApplyOps = [RegOp.SetDword(AtpKey, "DisableDataCollectionConsent", 1)],
            RemoveOps = [RegOp.DeleteValue(AtpKey, "DisableDataCollectionConsent")],
            DetectOps = [RegOp.CheckDword(AtpKey, "DisableDataCollectionConsent", 1)],
        },
        new TweakDef
        {
            Id = "atp-enable-edr-in-block-mode",
            Label = "Enable ATP EDR in Block Mode",
            Category = "Defender ATP Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets EnableEDRInBlockMode=1 in Windows Advanced Threat Protection policy. "
                + "Enables EDR in block mode, allowing Defender ATP to block malicious artefacts "
                + "detected post-breach even when a third-party antivirus is the primary real-time "
                + "protection engine. Adds a defence-in-depth layer.",
            Tags = ["atp", "defender", "edr", "block-mode", "policy", "security"],
            RegistryKeys = [AtpKey],
            ImpactScore = 5,
            SafetyRating = 4,
            ImpactNote = "EDR can actively block post-breach artefacts alongside third-party AV.",
            ApplyOps = [RegOp.SetDword(AtpKey, "EnableEDRInBlockMode", 1)],
            RemoveOps = [RegOp.DeleteValue(AtpKey, "EnableEDRInBlockMode")],
            DetectOps = [RegOp.CheckDword(AtpKey, "EnableEDRInBlockMode", 1)],
        },
    ];
}
