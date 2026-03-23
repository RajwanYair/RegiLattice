// RegiLattice.Core — Tweaks/DefenderAdvanced.cs
// Windows Defender cloud protection, MAPS reporting, real-time protection sub-settings,
// and scan configuration policies (Sprint 137).
// Slug "defadv" — complements Defender.cs (ASR, CFA, network protection, PUA, SmartScreen).

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class DefenderAdvanced
{
    private const string DefRoot = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender";
    private const string MpEngine = DefRoot + @"\MpEngine";
    private const string Spynet = DefRoot + @"\Spynet";
    private const string RealTime = DefRoot + @"\Real-Time Protection";
    private const string Scan = DefRoot + @"\Scan";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "defadv-cloud-block-level-high",
            Label = "Set Defender Cloud Block Level to High",
            Category = "Defender Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "Sets MpCloudBlockLevel=4 (High), causing Defender to block more "
                + "aggressively when cloud analysis is inconclusive. Values: "
                + "0=Default, 2=Moderate, 4=High, 6=High+, 8=Zero tolerance.",
            Tags = ["defender", "cloud protection", "block level", "security"],
            RegistryKeys = [MpEngine],
            ApplyOps = [RegOp.SetDword(MpEngine, "MpCloudBlockLevel", 4)],
            RemoveOps = [RegOp.DeleteValue(MpEngine, "MpCloudBlockLevel")],
            DetectOps = [RegOp.CheckDword(MpEngine, "MpCloudBlockLevel", 4)],
        },
        new TweakDef
        {
            Id = "defadv-cloud-extended-timeout",
            Label = "Extend Defender Cloud-Check Timeout to 50 s",
            Category = "Defender Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Sets MpBafsExtendedTimeout=50 to allow 50 seconds (default: 10 s) for the "
                + "cloud to analyse a suspicious file before executing it, improving detection "
                + "rates for novel threats.",
            Tags = ["defender", "cloud protection", "timeout", "bafs"],
            RegistryKeys = [MpEngine],
            ApplyOps = [RegOp.SetDword(MpEngine, "MpBafsExtendedTimeout", 50)],
            RemoveOps = [RegOp.DeleteValue(MpEngine, "MpBafsExtendedTimeout")],
            DetectOps = [RegOp.CheckDword(MpEngine, "MpBafsExtendedTimeout", 50)],
        },
        new TweakDef
        {
            Id = "defadv-maps-advanced-membership",
            Label = "Enable MAPS Advanced Membership (Automatic Sample Reporting)",
            Category = "Defender Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Sets SpynetReporting=2 (Advanced MAPS membership), sending additional "
                + "information to Microsoft about potentially malicious software. Required "
                + "for cloud protection to function fully. 0=Disabled, 1=Basic, 2=Advanced.",
            Tags = ["defender", "maps", "cloud", "spynet", "telemetry"],
            RegistryKeys = [Spynet],
            ApplyOps = [RegOp.SetDword(Spynet, "SpynetReporting", 2)],
            RemoveOps = [RegOp.DeleteValue(Spynet, "SpynetReporting")],
            DetectOps = [RegOp.CheckDword(Spynet, "SpynetReporting", 2)],
        },
        new TweakDef
        {
            Id = "defadv-auto-sample-submission",
            Label = "Enable Automatic Sample Submission (Safe Samples)",
            Category = "Defender Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Sets SubmitSamplesConsent=1 to automatically send safe samples "
                + "(safe file types) to Microsoft for analysis. 0=Always prompt, "
                + "1=Auto safe samples, 2=Never, 3=Always automatic.",
            Tags = ["defender", "sample submission", "maps", "cloud"],
            RegistryKeys = [Spynet],
            ApplyOps = [RegOp.SetDword(Spynet, "SubmitSamplesConsent", 1)],
            RemoveOps = [RegOp.DeleteValue(Spynet, "SubmitSamplesConsent")],
            DetectOps = [RegOp.CheckDword(Spynet, "SubmitSamplesConsent", 1)],
        },
        new TweakDef
        {
            Id = "defadv-enable-behavior-monitoring",
            Label = "Enable Defender Behavior Monitoring",
            Category = "Defender Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Ensures behavior monitoring is enabled in Defender real-time protection "
                + "by setting DisableBehaviorMonitoring=0. Policy-enforced default-on.",
            Tags = ["defender", "behaviour monitoring", "real-time protection", "security"],
            RegistryKeys = [RealTime],
            ApplyOps = [RegOp.SetDword(RealTime, "DisableBehaviorMonitoring", 0)],
            RemoveOps = [RegOp.DeleteValue(RealTime, "DisableBehaviorMonitoring")],
            DetectOps = [RegOp.CheckDword(RealTime, "DisableBehaviorMonitoring", 0)],
        },
        new TweakDef
        {
            Id = "defadv-enable-ioav-protection",
            Label = "Enable Defender On-Access (I/O) Scans",
            Category = "Defender Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Ensures I/O AV protection is enabled by policy (DisableIOAVProtection=0). "
                + "Defender scans all downloaded files and attachments via real-time hooks.",
            Tags = ["defender", "on-access", "ioav", "real-time protection"],
            RegistryKeys = [RealTime],
            ApplyOps = [RegOp.SetDword(RealTime, "DisableIOAVProtection", 0)],
            RemoveOps = [RegOp.DeleteValue(RealTime, "DisableIOAVProtection")],
            DetectOps = [RegOp.CheckDword(RealTime, "DisableIOAVProtection", 0)],
        },
        new TweakDef
        {
            Id = "defadv-enable-script-scanning",
            Label = "Enable Defender Script Scanning",
            Category = "Defender Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Enables real-time scanning of scripts (JS, VBS, PS1 etc.) via policy "
                + "(DisableScriptScanning=0). Mitigates script-based malware delivering "
                + "payloads through browser or Office exploits.",
            Tags = ["defender", "script scanning", "real-time protection", "security"],
            RegistryKeys = [RealTime],
            ApplyOps = [RegOp.SetDword(RealTime, "DisableScriptScanning", 0)],
            RemoveOps = [RegOp.DeleteValue(RealTime, "DisableScriptScanning")],
            DetectOps = [RegOp.CheckDword(RealTime, "DisableScriptScanning", 0)],
        },
        new TweakDef
        {
            Id = "defadv-scan-archives",
            Label = "Enable Defender Archive Scanning",
            Category = "Defender Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Enforces scanning of archive files (ZIP, RAR, 7z, CAB) during both "
                + "on-access and quick/full scans by policy (DisableArchiveScanning=0).",
            Tags = ["defender", "archive scanning", "scan", "security"],
            RegistryKeys = [Scan],
            ApplyOps = [RegOp.SetDword(Scan, "DisableArchiveScanning", 0)],
            RemoveOps = [RegOp.DeleteValue(Scan, "DisableArchiveScanning")],
            DetectOps = [RegOp.CheckDword(Scan, "DisableArchiveScanning", 0)],
        },
        new TweakDef
        {
            Id = "defadv-scan-email",
            Label = "Enable Defender Email Body Scanning",
            Category = "Defender Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Enforces scanning of email message bodies and attachments (EML, MSG, PST) "
                + "during scheduled scans by policy (DisableEmailScanning=0). Detects "
                + "malicious macro documents delivered via email.",
            Tags = ["defender", "email scanning", "scan", "security"],
            RegistryKeys = [Scan],
            ApplyOps = [RegOp.SetDword(Scan, "DisableEmailScanning", 0)],
            RemoveOps = [RegOp.DeleteValue(Scan, "DisableEmailScanning")],
            DetectOps = [RegOp.CheckDword(Scan, "DisableEmailScanning", 0)],
        },
        new TweakDef
        {
            Id = "defadv-randomize-scan-time",
            Label = "Randomize Defender Scheduled Scan Start Time",
            Category = "Defender Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Description =
                "Sets RandomizeScheduleTaskTimes=1 so that Defender scheduled scan tasks "
                + "start at a random offset (±30 minutes) around the configured time, "
                + "spreading load across many machines in enterprise environments.",
            Tags = ["defender", "scheduled scan", "randomize", "performance"],
            RegistryKeys = [Scan],
            ApplyOps = [RegOp.SetDword(Scan, "RandomizeScheduleTaskTimes", 1)],
            RemoveOps = [RegOp.DeleteValue(Scan, "RandomizeScheduleTaskTimes")],
            DetectOps = [RegOp.CheckDword(Scan, "RandomizeScheduleTaskTimes", 1)],
        },
    ];
}
