// RegiLattice.Core — Tweaks/FirmwareUpdatePolicy.cs
// UEFI Firmware Update Security Policy — Sprint 585.
// Configures Windows UEFI firmware update controls, capsule delivery
// restriction, firmware driver signing requirements, and automatic
// firmware revision management.
// Category: "Firmware Update Policy" | Slug: fwupd
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\WU
//           HKLM\SOFTWARE\Policies\Microsoft\Windows\UEFI\FirmwareUpdate

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class FirmwareUpdatePolicy
{
    private const string FwUpdateKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\UEFI\FirmwareUpdate";

    private const string WuPolicyKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "fwupd-enable-firmware-update-via-wu",
                Label = "Firmware Update: Enable UEFI Firmware Updates via Windows Update",
                Category = "Firmware Update Policy",
                Description =
                    "Sets EnableFirmwareUpdates=1 in the Firmware Update policy key. Enables delivery of UEFI firmware, microcode, and driver firmware updates via the Windows Update UEFI firmware update mechanism (ESRT — UEFI System Resource Table). Microsoft and OEMs publish firmware updates as Windows Update packages. Enabling this ensures that critical security firmware updates (CPU microcode for Spectre/Meltdown, firmware CVEs, NIC firmware security patches) are delivered automatically alongside Windows updates. Without this, firmware updates must be manually applied from OEM download pages — creating a persistent firmware patching gap in enterprise environments.",
                Tags = ["firmware", "windows-update", "esrt", "microcode", "uefi"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote =
                    "UEFI firmware updates delivered via Windows Update. Firmware updates are applied on next restart. In environments with strict change management, firmware updates should be deferred and tested before broad deployment (use Windows Update deferral policies for feature/quality updates, but firmware updates should be tested separately). Some firmware updates are irreversible — maintain firmware rollback documentation.",
                ApplyOps = [RegOp.SetDword(FwUpdateKey, "EnableFirmwareUpdates", 1)],
                RemoveOps = [RegOp.DeleteValue(FwUpdateKey, "EnableFirmwareUpdates")],
                DetectOps = [RegOp.CheckDword(FwUpdateKey, "EnableFirmwareUpdates", 1)],
            },
            new TweakDef
            {
                Id = "fwupd-require-capsule-signing",
                Label = "Firmware Update: Require Signed Capsule Delivery for All Firmware Updates",
                Category = "Firmware Update Policy",
                Description =
                    "Sets RequireCapsuleSigning=1 in the Firmware Update policy key. Requires that all UEFI firmware update capsules are digitally signed before they are accepted for delivery. An unsigned firmware update capsule (delivered via Windows Update or a local installer) that passes through this check unchallenged could be a malicious replacement firmware (firmware implant). Requiring capsule signing ensures that only OEM or Microsoft-signed firmware capsules are accepted — unauthenticated replacement firmware is rejected.",
                Tags = ["firmware", "capsule", "signing", "firmware-implant", "uefi"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "Firmware update capsules must be signed. OEM firmware update tools that deliver unsigned local firmware packages will fail. All major OEM firmware updates from Dell Update, HP Support Assistant, and Lenovo System Update use signed capsules. Older OEM tools (pre-2020) may use unsigned capsule delivery.",
                ApplyOps = [RegOp.SetDword(FwUpdateKey, "RequireCapsuleSigning", 1)],
                RemoveOps = [RegOp.DeleteValue(FwUpdateKey, "RequireCapsuleSigning")],
                DetectOps = [RegOp.CheckDword(FwUpdateKey, "RequireCapsuleSigning", 1)],
            },
            new TweakDef
            {
                Id = "fwupd-enable-firmware-version-audit",
                Label = "Firmware Update: Enable UEFI Firmware Version Reporting to WU",
                Category = "Firmware Update Policy",
                Description =
                    "Sets EnableFirmwareVersionReporting=1 in the Firmware Update policy key. Enables reporting of the current UEFI firmware version to Windows Update/SCCM/Intune. Firmware version reporting allows IT administrators to audit which firmware versions are deployed across the fleet and identify devices that are behind on firmware updates. Without this reporting, enterprise firmware version visibility requires per-device BIOS queries or WMI polling. Centralised firmware version reporting via Windows Update enables proactive identification of devices vulnerable to known firmware CVEs.",
                Tags = ["firmware", "version-reporting", "audit", "intune", "vulnerability-management"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "Firmware version reported to Windows Update and Intune. Firmware model and version recorded in WU client record. No PII — only hardware identifiers and firmware version. Enables firmware compliance dashboards in Intune. Not available on all OEMs — firmware version reporting depends on ESRT table availability in device firmware.",
                ApplyOps = [RegOp.SetDword(FwUpdateKey, "EnableFirmwareVersionReporting", 1)],
                RemoveOps = [RegOp.DeleteValue(FwUpdateKey, "EnableFirmwareVersionReporting")],
                DetectOps = [RegOp.CheckDword(FwUpdateKey, "EnableFirmwareVersionReporting", 1)],
            },
            new TweakDef
            {
                Id = "fwupd-disable-os-downgrade-firmware-flag",
                Label = "Firmware Update: Disable Firmware-Controlled OS Downgrade (SetOsIndications Clear)",
                Category = "Firmware Update Policy",
                Description =
                    "Sets PreventFirmwareOSIndications=1 in the Firmware Update policy key. Prevents software from setting UEFI OS Indications variables that request the firmware to perform privileged OS-downgrade or firmware recovery operations on next boot. Some firmware implementations respond to OS-set OsIndications values by entering a special recovery or setup mode. An attacker with kernel access who can write UEFI variables could set OsIndications to trigger firmware-level recovery or OS reinstallation on next boot — effectively causing a denial-of-service by re-imaging the OS. Blocking OsIndications writes prevents this DOS vector.",
                Tags = ["firmware", "os-indications", "uefi-variables", "dos-prevention", "kernel"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "OS-controlled UEFI OsIndications blocked. Windows Recovery Environment (WinRE) and advanced boot options that use OS-firmware communication via OsIndications may be affected. Windows Update and normal OEM firmware update tools do not use OS-controlled OsIndications for normal operations — only recovery tools use this mechanism.",
                ApplyOps = [RegOp.SetDword(FwUpdateKey, "PreventFirmwareOSIndications", 1)],
                RemoveOps = [RegOp.DeleteValue(FwUpdateKey, "PreventFirmwareOSIndications")],
                DetectOps = [RegOp.CheckDword(FwUpdateKey, "PreventFirmwareOSIndications", 1)],
            },
            new TweakDef
            {
                Id = "fwupd-set-firmware-update-scan-interval-7days",
                Label = "Firmware Update: Set Firmware Update Scan Frequency to 7 Days",
                Category = "Firmware Update Policy",
                Description =
                    "Sets FirmwareUpdateScanFrequency=7 in the Firmware Update policy key (units: days). Sets the frequency at which the Windows Update client checks for new firmware updates to every 7 days. By default, firmware update check frequency follows the general Windows Update schedule. Setting an explicit 7-day cadence ensures that new firmware security updates are picked up within one week of publication — balancing prompt patching against the operational cost of weekly firmware update deployments. For high-security environments, reduce to 1–3 days.",
                Tags = ["firmware", "update-scan", "cadence", "patch-management", "windows-update"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "Firmware update check performed weekly. Devices not connected to WU for more than 7 days may miss a firmware update check cycle. No visible user impact — check runs in the background. Firm update availability does not automatically install the firmware; installation requires restart approval per the restart policy.",
                ApplyOps = [RegOp.SetDword(FwUpdateKey, "FirmwareUpdateScanFrequency", 7)],
                RemoveOps = [RegOp.DeleteValue(FwUpdateKey, "FirmwareUpdateScanFrequency")],
                DetectOps = [RegOp.CheckDword(FwUpdateKey, "FirmwareUpdateScanFrequency", 7)],
            },
            new TweakDef
            {
                Id = "fwupd-block-user-firmware-rollback",
                Label = "Firmware Update: Block User-Initiated Firmware Version Rollback",
                Category = "Firmware Update Policy",
                Description =
                    "Sets BlockFirmwareRollback=1 in the Firmware Update policy key. Prevents users and non-admin processes from rolling back UEFI firmware to an older version once a newer version has been applied. Firmware rollback (to a version with known, unpatched vulnerabilities) is a prerequisite step for many firmware persistence attacks — an attacker who can roll back to a vulnerable firmware version can then exploit the known vulnerability to plant a firmware implant. Blocking rollback ensures that once a security firmware update is applied, the device cannot be returned to a less-secure firmware state by an attacker.",
                Tags = ["firmware", "rollback-prevention", "downgrade", "firmware-implant", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote =
                    "Firmware version rollback blocked. If a firmware update causes hardware compatibility issues, rolling back requires physical UEFI intervention (OEM recovery tools, BIOS recovery switch). Document the rollback procedure before enabling this policy. IT must maintain OEM firmware recovery tooling and recovery USB media for all device models in fleet.",
                ApplyOps = [RegOp.SetDword(FwUpdateKey, "BlockFirmwareRollback", 1)],
                RemoveOps = [RegOp.DeleteValue(FwUpdateKey, "BlockFirmwareRollback")],
                DetectOps = [RegOp.CheckDword(FwUpdateKey, "BlockFirmwareRollback", 1)],
            },
            new TweakDef
            {
                Id = "fwupd-enable-firmware-update-eventlog",
                Label = "Firmware Update: Enable Firmware Update Event Logging to Windows Event Log",
                Category = "Firmware Update Policy",
                Description =
                    "Sets EnableFirmwareUpdateEventLog=1 in the Firmware Update policy key. Enables logging of UEFI firmware update application events to the Windows event log (System event log, source: UFIUpdate). Events include the firmware component updated, the from/to version, the update status (success/failure), and the reason for failure if applicable. Firmware update event logging supports change management tracking — every firmware update on any enterprise device generates an auditable event that SIEM and asset management tools can correlate with approved firmware change records.",
                Tags = ["firmware", "event-log", "audit", "change-management", "siem"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "Firmware update events logged to Windows event log. Events appear in System log with source UFIUpdate. Minimal event volume — firmware updates are infrequent. SIEM rules can correlate unexpected firmware changes (unapproved firmware version change) as a security anomaly indicator.",
                ApplyOps = [RegOp.SetDword(FwUpdateKey, "EnableFirmwareUpdateEventLog", 1)],
                RemoveOps = [RegOp.DeleteValue(FwUpdateKey, "EnableFirmwareUpdateEventLog")],
                DetectOps = [RegOp.CheckDword(FwUpdateKey, "EnableFirmwareUpdateEventLog", 1)],
            },
            new TweakDef
            {
                Id = "fwupd-require-admin-for-manual-firmware-update",
                Label = "Firmware Update: Require Admin Approval for Manual Firmware Update Execution",
                Category = "Firmware Update Policy",
                Description =
                    "Sets RequireAdminForFirmwareUpdate=1 in the Firmware Update policy key. Requires administrator approval before a manually initiated firmware update (e.g., via OEM firmware update tool run locally) can execute. Without this requirement, a standard user who can run an OEM firmware update utility can potentially flash a modified firmware — especially if the OEM tool accepts a firmware image file path. Requiring admin approval ensures that firmware updates not delivered via Windows Update must be explicitly authorised by an administrator, preventing social engineering attacks where users are deceived into running a firmware installer.",
                Tags = ["firmware", "admin-approval", "manual-update", "uac", "social-engineering"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "Manual firmware updates require administrator approval. Standard users who attempt to run OEM firmware update tools receive UAC elevation prompts. All firmware updates via Windows Update (capsule delivery) are performed by the Windows Update service under SYSTEM and are not affected.",
                ApplyOps = [RegOp.SetDword(FwUpdateKey, "RequireAdminForFirmwareUpdate", 1)],
                RemoveOps = [RegOp.DeleteValue(FwUpdateKey, "RequireAdminForFirmwareUpdate")],
                DetectOps = [RegOp.CheckDword(FwUpdateKey, "RequireAdminForFirmwareUpdate", 1)],
            },
            new TweakDef
            {
                Id = "fwupd-set-firmware-update-defer-days-14",
                Label = "Firmware Update: Set Firmware Update Deferral to 14 Days After Release",
                Category = "Firmware Update Policy",
                Description =
                    "Sets DeferFirmwareUpdatesDays=14 in the Windows Update policy key. Defers firmware update installation by 14 days from the date Microsoft or OEM first publishes them to Windows Update. The 14-day deferral period allows time for reported deployment issues to surface and for regression reports to be filed before the update reaches the enterprise fleet. Firmware updates with critical compatibility issues (e.g., BSOD-inducing microcode updates, display driver firmware breaking external monitors) are often reported in the first 3–7 days post-release. 14 days provides a reasonable canary window without creating an unacceptable security gap.",
                Tags = ["firmware", "deferral", "quality-gate", "canary", "windows-update"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "Firmware updates deferred 14 days from release. Security-critical firmware updates (Spectre-class microcode) are delayed for 14 days. For high-severity firmware CVEs, consider reducing or bypassing the deferral via Windows Update exemption group. For routine firmware maintenance updates, 14 days is appropriate.",
                ApplyOps = [RegOp.SetDword(WuPolicyKey, "DeferFirmwareUpdatesDays", 14)],
                RemoveOps = [RegOp.DeleteValue(WuPolicyKey, "DeferFirmwareUpdatesDays")],
                DetectOps = [RegOp.CheckDword(WuPolicyKey, "DeferFirmwareUpdatesDays", 14)],
            },
            new TweakDef
            {
                Id = "fwupd-enable-legacy-bios-update-block",
                Label = "Firmware Update: Block Legacy BIOS (Non-UEFI) Firmware Update Installation",
                Category = "Firmware Update Policy",
                Description =
                    "Sets BlockLegacyBiosUpdate=1 in the Firmware Update policy key. Prevents the installation of firmware updates designed for legacy BIOS (MBR/CSM-mode) systems. Enterprise Secure Boot and UEFI-based systems should not accept legacy BIOS firmware packages — they are built for different firmware architectures. A malicious actor who delivers a forged 'legacy BIOS update' to a UEFI system may attempt to exploit the UEFI CSM (Compatibility Support Module) or subvert firmware update routing. Blocking legacy BIOS updates ensures only proper UEFI capsule updates are accepted.",
                Tags = ["firmware", "legacy-bios", "csm", "update-filter", "uefi"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote =
                    "Legacy BIOS firmware updates blocked. Only UEFI capsule firmware updates accepted. Relevant only for organisations with mixed BIOS/UEFI fleets. On UEFI-native systems (post-2015 hardware), this policy has no practical impact since legacy BIOS updates are not delivered to UEFI systems by default.",
                ApplyOps = [RegOp.SetDword(FwUpdateKey, "BlockLegacyBiosUpdate", 1)],
                RemoveOps = [RegOp.DeleteValue(FwUpdateKey, "BlockLegacyBiosUpdate")],
                DetectOps = [RegOp.CheckDword(FwUpdateKey, "BlockLegacyBiosUpdate", 1)],
            },
        ];
}
