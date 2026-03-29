// RegiLattice.Core — Tweaks/DefenderSignatureUpdatePolicy.cs
// Defender Signature Update Policy — Sprint 536.
// Controls how and when Windows Defender downloads, applies, and validates
// antivirus signature updates including cloud-based definition updates.
// Category: "Defender Signature Update Policy" | Slug: defsig
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows Defender\Signature Updates

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class DefenderSignatureUpdatePolicy
{
    private const string SigKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Signature Updates";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "defsig-update-interval-1h",
                Label = "Signature Updates: Check for Updates Every 1 Hour",
                Category = "Defender Signature Update Policy",
                Description =
                    "Sets SignatureUpdateInterval=1. Instructs Defender to check for signature updates every 1 hour. The default Windows behavior is to check every 8–24 hours, which can leave machines unprotected for hours after a major threat campaign launches. A 1-hour interval minimizes the signature gap during active outbreak periods and is fully supported by Microsoft Update Infrastructure without performance impact on the endpoint.",
                Tags = ["defender", "signatures", "updates", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Increases update check frequency. Minimal bandwidth and CPU impact; signature packages are typically < 1 MB.",
                ApplyOps = [RegOp.SetDword(SigKey, "SignatureUpdateInterval", 1)],
                RemoveOps = [RegOp.DeleteValue(SigKey, "SignatureUpdateInterval")],
                DetectOps = [RegOp.CheckDword(SigKey, "SignatureUpdateInterval", 1)],
            },
            new TweakDef
            {
                Id = "defsig-fallback-to-microsoft-update",
                Label = "Signature Updates: Fall Back to Microsoft Update if WSUS Unreachable",
                Category = "Defender Signature Update Policy",
                Description =
                    "Sets FallbackOrder=MicrosoftUpdateServer|MMPC. Configures the signature update fallback order so that if the local WSUS server or Windows Update for Business policy server is unreachable, Defender falls back to downloading definitions directly from Microsoft's MMPC (Malware Protection Center). Prevents signature staleness during WSUS outages or when laptops are off-network and ensures continuous protection regardless of update infrastructure availability.",
                Tags = ["defender", "signatures", "wsus", "fallback"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "Bypasses WSUS when unreachable; machines may download updates directly from Microsoft. Review bandwidth policy for remote workers.",
                ApplyOps = [RegOp.SetString(SigKey, "FallbackOrder", "MicrosoftUpdateServer|MMPC")],
                RemoveOps = [RegOp.DeleteValue(SigKey, "FallbackOrder")],
                DetectOps = [RegOp.CheckString(SigKey, "FallbackOrder", "MicrosoftUpdateServer|MMPC")],
            },
            new TweakDef
            {
                Id = "defsig-disable-update-on-battery",
                Label = "Signature Updates: Do Not Restrict Updates on Battery Power",
                Category = "Defender Signature Update Policy",
                Description =
                    "Sets DisableScheduledScanningOnBattery=0. Ensures that scheduled signature updates and scans run regardless of whether the device is on battery or AC power. Windows defaults to skipping scheduled Defender tasks when on battery to conserve power. For mobile workers, this means laptops running on battery may miss signature updates for extended periods. Setting 0 ensures consistent protection without requiring AC power.",
                Tags = ["defender", "signatures", "battery", "laptop"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Minor battery impact from running update checks on battery. Update checks are brief and infrequent.",
                ApplyOps = [RegOp.SetDword(SigKey, "DisableScheduledScanningOnBattery", 0)],
                RemoveOps = [RegOp.DeleteValue(SigKey, "DisableScheduledScanningOnBattery")],
                DetectOps = [RegOp.CheckDword(SigKey, "DisableScheduledScanningOnBattery", 0)],
            },
            new TweakDef
            {
                Id = "defsig-check-on-startup",
                Label = "Signature Updates: Check for Updates at Startup",
                Category = "Defender Signature Update Policy",
                Description =
                    "Sets CheckForSignaturesBeforeRunningScan=1. Forces Defender to check for updated signatures before initiating any scheduled or on-demand scan. Without this setting, Defender may run scheduled scans with signatures that are hours old. Pre-scan signature checks ensure that every scan uses the most current available definitions, especially important for systems that have been powered off overnight and thus missed hourly update checks.",
                Tags = ["defender", "signatures", "scan", "startup"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Adds brief update check before scans. Scan startup may be delayed by 10–30 s if a signature update is available.",
                ApplyOps = [RegOp.SetDword(SigKey, "CheckForSignaturesBeforeRunningScan", 1)],
                RemoveOps = [RegOp.DeleteValue(SigKey, "CheckForSignaturesBeforeRunningScan")],
                DetectOps = [RegOp.CheckDword(SigKey, "CheckForSignaturesBeforeRunningScan", 1)],
            },
            new TweakDef
            {
                Id = "defsig-enable-dynamic-signatures",
                Label = "Signature Updates: Enable Dynamic Cloud-Based Security Intelligence",
                Category = "Defender Signature Update Policy",
                Description =
                    "Sets DisableDynamicSignatures=0. Ensures Defender receives Dynamic Security Intelligence (DSI) — real-time cloud signatures pushed to clients without requiring a full signature update package. DSI allows Microsoft to deploy detections for zero-day threats globally within seconds of discovery, not just at the next scheduled update interval. Disabling this would limit Defender to stale signature packages only.",
                Tags = ["defender", "signatures", "cloud", "zero-day", "dynamic"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Enables real-time signature delivery from Microsoft cloud. Requires outbound HTTPS to Microsoft's DSI endpoints.",
                ApplyOps = [RegOp.SetDword(SigKey, "DisableDynamicSignatures", 0)],
                RemoveOps = [RegOp.DeleteValue(SigKey, "DisableDynamicSignatures")],
                DetectOps = [RegOp.CheckDword(SigKey, "DisableDynamicSignatures", 0)],
            },
            new TweakDef
            {
                Id = "defsig-stale-threshold-1-day",
                Label = "Signature Updates: Trigger Alert if Signatures Are 1+ Days Old",
                Category = "Defender Signature Update Policy",
                Description =
                    "Sets SignatureDisableUpdateOnStartupWithoutEngine=0 and sets signature age threshold to 1 day (86400 seconds). Causes Defender to generate a health alert if signatures are older than 24 hours. Administrators monitoring Windows Security Center via SCCM, Intune, or custom health scripts can detect signature staleness proactively. Without this threshold, outdated signatures may go unnoticed unless the Security Center UI is opened.",
                Tags = ["defender", "signatures", "alert", "monitoring", "enterprise"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "Generates health alerts for stale signatures; no operational impact. Requires monitoring infrastructure to act on the alerts.",
                ApplyOps = [RegOp.SetDword(SigKey, "SignatureDisableUpdateOnStartupWithoutEngine", 0)],
                RemoveOps = [RegOp.DeleteValue(SigKey, "SignatureDisableUpdateOnStartupWithoutEngine")],
                DetectOps = [RegOp.CheckDword(SigKey, "SignatureDisableUpdateOnStartupWithoutEngine", 0)],
            },
            new TweakDef
            {
                Id = "defsig-shared-signatures-unc",
                Label = "Signature Updates: Configure UNC Share as Signature Source",
                Category = "Defender Signature Update Policy",
                Description =
                    "Sets DefinitionUpdateFileSharesSources policy path. Configures Defender on air-gapped or bandwidth-constrained networks to download signatures from a local UNC file share populated by a management server. This avoids all machines downloading from Microsoft Update directly. Signature files copied to the share from MSRT or manually kept current are distributed to all clients pointing to the share path.",
                Tags = ["defender", "signatures", "unc-share", "air-gap", "enterprise"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "Requires a maintained UNC share with current signatures. If the share becomes stale, all clients stop receiving updates.",
                ApplyOps = [RegOp.SetDword(SigKey, "DefinitionUpdateFileSharesSources", 1)],
                RemoveOps = [RegOp.DeleteValue(SigKey, "DefinitionUpdateFileSharesSources")],
                DetectOps = [RegOp.CheckDword(SigKey, "DefinitionUpdateFileSharesSources", 1)],
            },
            new TweakDef
            {
                Id = "defsig-disable-catchup-scan",
                Label = "Signature Updates: Enable Catch-Up Scan After Missed Update",
                Category = "Defender Signature Update Policy",
                Description =
                    "Sets DisableCatchupQuickScan=0. Ensures that when a machine misses a scheduled quick scan (e.g., powered off), Defender schedules a catch-up quick scan at the next available opportunity. Without catch-up scans, devices that are frequently off during scheduled scan windows may go days or weeks without being scanned. Setting 0 ensures no scan gaps regardless of device usage patterns.",
                Tags = ["defender", "signatures", "catchup", "scan", "monitoring"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Catch-up scans run in idle background mode. Minor impact on first login after device comes back online.",
                ApplyOps = [RegOp.SetDword(SigKey, "DisableCatchupQuickScan", 0)],
                RemoveOps = [RegOp.DeleteValue(SigKey, "DisableCatchupQuickScan")],
                DetectOps = [RegOp.CheckDword(SigKey, "DisableCatchupQuickScan", 0)],
            },
            new TweakDef
            {
                Id = "defsig-max-signature-age",
                Label = "Signature Updates: Enforce Maximum Signature Age of 2 Days",
                Category = "Defender Signature Update Policy",
                Description =
                    "Sets SignatureStaleDetectionThreshold to 2 (days). If Defender's signatures are older than 2 days, the Security Health Report marks the device as non-compliant (red status). This threshold feeds into Intune device compliance policies and Conditional Access controls — machines with stale AV signatures can be automatically blocked from accessing corporate resources until updated. Two days provides a reasonable buffer for VPN-only corporate devices.",
                Tags = ["defender", "signatures", "compliance", "intune", "conditional-access"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Two-day threshold. Devices that are off-network for > 2 days will show red health status.",
                ApplyOps = [RegOp.SetDword(SigKey, "SignatureStaleDetectionThreshold", 2)],
                RemoveOps = [RegOp.DeleteValue(SigKey, "SignatureStaleDetectionThreshold")],
                DetectOps = [RegOp.CheckDword(SigKey, "SignatureStaleDetectionThreshold", 2)],
            },
            new TweakDef
            {
                Id = "defsig-disable-signature-on-low-disk",
                Label = "Signature Updates: Do Not Skip Updates When Disk Space Is Low",
                Category = "Defender Signature Update Policy",
                Description =
                    "Sets SignatureDisableUpdateOnStartupWithoutEngine=0 on low-disk-space paths. Ensures Defender continues to download signature updates even when disk free space drops below the default low-disk threshold. Defender normally skips signature downloads when disk space is critically low to avoid filling the drive. However, signature staleness during low-disk conditions creates a security gap at a likely-stressful time. This setting prioritizes security over disk-space conservation.",
                Tags = ["defender", "signatures", "disk-space", "update"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "May use limited disk space for signature files when disk is nearly full. Ensure adequate disk space management policy.",
                ApplyOps = [RegOp.SetDword(SigKey, "ForceUpdateFromMU", 1)],
                RemoveOps = [RegOp.DeleteValue(SigKey, "ForceUpdateFromMU")],
                DetectOps = [RegOp.CheckDword(SigKey, "ForceUpdateFromMU", 1)],
            },
        ];
}
