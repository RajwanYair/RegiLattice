// RegiLattice.Core — Tweaks/DefenderAntivirusAdvancedPolicy.cs
// Windows Defender Antivirus engine tuning, scan scheduling, submission, and quarantine policy — Sprint 504.
// Category: "Defender Antivirus Advanced Policy" | Slug: avadv
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows Defender

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class DefenderAntivirusAdvancedPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender";
    private const string ScanKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Scan";
    private const string SpynetKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Spynet";
    private const string QtnKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Quarantine";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "avadv-disable-tamper-protection",
                Label = "Prevent Standard Users from Disabling Tamper Protection",
                Category = "Defender Antivirus Advanced Policy",
                Description =
                    "Sets a policy requirement that Tamper Protection remains enabled, preventing standard users and non-authorised scripts from disabling Windows Defender via registry or settings, a common malware persistence technique.",
                Tags = ["defender", "tamper-protection", "antivirus", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Tamper Protection enforced via policy; Defender cannot be disabled by users or scripts.",
                ApplyOps = [RegOp.SetDword(Key, "DisableTamperProtection", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableTamperProtection")],
                DetectOps = [RegOp.CheckDword(Key, "DisableTamperProtection", 0)],
            },
            new TweakDef
            {
                Id = "avadv-block-sample-submission-non-consent",
                Label = "Block Automatic Sample Submission Without User Consent",
                Category = "Defender Antivirus Advanced Policy",
                Description =
                    "Configures Defender to always prompt before sending potentially sensitive file samples to Microsoft for cloud analysis, preventing automatic cloud submission of suspicious documents that may contain confidential data.",
                Tags = ["defender", "sample-submission", "privacy", "cloud", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Sample submission requires user consent; suspicious files not silently sent to Microsoft.",
                ApplyOps = [RegOp.SetDword(SpynetKey, "SubmitSamplesConsent", 2)],
                RemoveOps = [RegOp.DeleteValue(SpynetKey, "SubmitSamplesConsent")],
                DetectOps = [RegOp.CheckDword(SpynetKey, "SubmitSamplesConsent", 2)],
            },
            new TweakDef
            {
                Id = "avadv-set-cloud-protection-level-high",
                Label = "Set Defender Cloud Protection Level to High",
                Category = "Defender Antivirus Advanced Policy",
                Description =
                    "Sets Windows Defender's cloud-delivered protection level to High, enabling more aggressive cloud-based heuristic analysis and slightly longer scan timeouts to catch sophisticated polymorphic threats missed by signature-only scans.",
                Tags = ["defender", "cloud-protection", "heuristics", "antivirus", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Cloud protection level set to High; aggressive heuristics enabled for zero-day threat detection.",
                ApplyOps = [RegOp.SetDword(SpynetKey, "MAPSReporting", 2)],
                RemoveOps = [RegOp.DeleteValue(SpynetKey, "MAPSReporting")],
                DetectOps = [RegOp.CheckDword(SpynetKey, "MAPSReporting", 2)],
            },
            new TweakDef
            {
                Id = "avadv-set-scan-scheduled-quick-daily",
                Label = "Schedule Daily Quick Scan at 02:00",
                Category = "Defender Antivirus Advanced Policy",
                Description =
                    "Configures Windows Defender to perform a daily quick scan at 02:00 AM (hour 2), ensuring endpoint malware is detected and cleared on a daily schedule without relying on user-initiated scans.",
                Tags = ["defender", "scheduled-scan", "quick-scan", "antivirus", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Daily quick scan scheduled at 02:00 AM; ensures regular automated endpoint malware detection.",
                ApplyOps = [RegOp.SetDword(ScanKey, "ScheduleQuickScanTime", 120)],
                RemoveOps = [RegOp.DeleteValue(ScanKey, "ScheduleQuickScanTime")],
                DetectOps = [RegOp.CheckDword(ScanKey, "ScheduleQuickScanTime", 120)],
            },
            new TweakDef
            {
                Id = "avadv-enable-realtime-protection",
                Label = "Enforce Real-Time Protection is Always On",
                Category = "Defender Antivirus Advanced Policy",
                Description =
                    "Sets a policy that ensures Windows Defender real-time protection monitoring is always active, preventing GPO or local policy from disabling file-system monitoring on covered endpoints.",
                Tags = ["defender", "real-time-protection", "always-on", "antivirus", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Real-time protection enforced as always-on; cannot be disabled via local policy or settings.",
                ApplyOps =
                [
                    RegOp.SetDword(
                        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection",
                        "DisableRealtimeMonitoring",
                        0
                    ),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(
                        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection",
                        "DisableRealtimeMonitoring"
                    ),
                ],
                DetectOps =
                [
                    RegOp.CheckDword(
                        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection",
                        "DisableRealtimeMonitoring",
                        0
                    ),
                ],
            },
            new TweakDef
            {
                Id = "avadv-block-ioav-disable",
                Label = "Block Disabling On-Access Scan for Downloaded Files",
                Category = "Defender Antivirus Advanced Policy",
                Description =
                    "Prevents policy-level disabling of the Internet Origin/Anti-virus (IOAV) scan that checks files downloaded from the internet, ensuring that browser-downloaded executables are always scanned before execution.",
                Tags = ["defender", "ioav", "download-scan", "antivirus", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "IOAV download scan enforced; all internet-downloaded files automatically scanned before execution.",
                ApplyOps =
                [
                    RegOp.SetDword(
                        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection",
                        "DisableIOAVProtection",
                        0
                    ),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(
                        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection",
                        "DisableIOAVProtection"
                    ),
                ],
                DetectOps =
                [
                    RegOp.CheckDword(
                        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection",
                        "DisableIOAVProtection",
                        0
                    ),
                ],
            },
            new TweakDef
            {
                Id = "avadv-enable-scanning-mapped-drives",
                Label = "Enable Scanning of Network Mapped Drive Files",
                Category = "Defender Antivirus Advanced Policy",
                Description =
                    "Configures Windows Defender to scan files on mapped network drives in addition to local files, protecting against malware distribution via shared network storage that may not have server-side scanning enabled.",
                Tags = ["defender", "network-scan", "mapped-drives", "antivirus", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Mapped network drive scanning enabled; files on network shares scanned before access.",
                ApplyOps = [RegOp.SetDword(ScanKey, "DisableScanningMappedNetworkDrivesForFullScan", 0)],
                RemoveOps = [RegOp.DeleteValue(ScanKey, "DisableScanningMappedNetworkDrivesForFullScan")],
                DetectOps = [RegOp.CheckDword(ScanKey, "DisableScanningMappedNetworkDrivesForFullScan", 0)],
            },
            new TweakDef
            {
                Id = "avadv-set-quarantine-purge-days-30",
                Label = "Set Quarantine Auto-Purge to 30 Days",
                Category = "Defender Antivirus Advanced Policy",
                Description =
                    "Configures Windows Defender to automatically delete quarantined files after 30 days, preventing unbounded growth of the quarantine store while retaining files long enough for forensic analysis if needed.",
                Tags = ["defender", "quarantine", "purge", "antivirus", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Quarantine auto-purge set to 30 days; quarantined malware files deleted after 30-day retention period.",
                ApplyOps = [RegOp.SetDword(QtnKey, "PurgeItemsAfterDelay", 30)],
                RemoveOps = [RegOp.DeleteValue(QtnKey, "PurgeItemsAfterDelay")],
                DetectOps = [RegOp.CheckDword(QtnKey, "PurgeItemsAfterDelay", 30)],
            },
            new TweakDef
            {
                Id = "avadv-enable-behavior-monitoring",
                Label = "Enforce Defender Behaviour Monitoring is Always On",
                Category = "Defender Antivirus Advanced Policy",
                Description =
                    "Ensures Windows Defender behavioural monitoring (which detects suspicious process activities and file access patterns in real-time) is enforced as always active via policy, providing protection against fileless malware.",
                Tags = ["defender", "behaviour-monitoring", "fileless", "antivirus", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Defender behaviour monitoring enforced; fileless and process-based malware detected in real-time.",
                ApplyOps =
                [
                    RegOp.SetDword(
                        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection",
                        "DisableBehaviorMonitoring",
                        0
                    ),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(
                        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection",
                        "DisableBehaviorMonitoring"
                    ),
                ],
                DetectOps =
                [
                    RegOp.CheckDword(
                        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection",
                        "DisableBehaviorMonitoring",
                        0
                    ),
                ],
            },
            new TweakDef
            {
                Id = "avadv-disable-av-ui-telemetry",
                Label = "Disable Defender Antivirus UI Telemetry to Microsoft",
                Category = "Defender Antivirus Advanced Policy",
                Description =
                    "Prevents Windows Defender from sending UI interaction telemetry (which settings pages are visited, what scans are triggered) to Microsoft, reducing cloud data exposure while keeping all antivirus protection active.",
                Tags = ["defender", "telemetry", "privacy", "ui", "microsoft", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Defender UI telemetry to Microsoft disabled; antivirus protection unaffected, usage data not sent.",
                ApplyOps = [RegOp.SetDword(Key, "DisableMpTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableMpTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "DisableMpTelemetry", 1)],
            },
        ];
}
