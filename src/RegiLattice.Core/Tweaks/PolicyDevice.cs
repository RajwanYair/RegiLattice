// RegiLattice.Core — Tweaks/PolicyDevice.cs
// Device installation, enrollment, guard, firmware, hardware, portable devices, USB storage, and kernel DMA protection policies
// Category: "Device & Hardware Policy"
// Consolidated from 23 modules.

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PolicyDevice
{
    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            .. _BluetoothAdvPolicy.Data,
            .. _DeviceCompliancePolicy.Data,
            .. _DeviceEnrollmentLimitPolicy.Data,
            .. _DeviceEnrollmentPolicy.Data,
            .. _DeviceGuardPolicy.Data,
            .. _DeviceGuardVbs.Data,
            .. _DeviceHealthCheckPolicy.Data,
            .. _DeviceInstallPolicies.Data,
            .. _DeviceInstallPolicy.Data,
            .. _DeviceLockGpoPolicy.Data,
            .. _DeviceProvisioningPolicy.Data,
            .. _DeviceRegistrationPolicy.Data,
            .. _FirmwareUpdatePolicy.Data,
            .. _HardwareDevicePolicy.Data,
            .. _KernelDmaProtectionPolicy.Data,
            .. _MemoryDiagnostics.Data,
            .. _PageFilePolicy.Data,
            .. _PortableDevicePolicy.Data,
            .. _PortableDevicesPolicy.Data,
            .. _ProcessorPolicy.Data,
            .. _SuperFetchSysmainPolicy.Data,
            .. _UsbStoragePolicy.Data,
            .. _VirtualDiskServicePolicy.Data,
        ];

    // ── BluetoothAdvPolicy ──
    private static class _BluetoothAdvPolicy
    {
        private const string BtPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Bluetooth";
        private const string BthPort = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters";
        private const string BtHub = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\BluetoothDeviceEnumerator";
        private const string BtPrivacy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\BlueTooth";
        private const string BtPhoneBook = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\BlueTooth\PhoneBook";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "btadv-disable-bluetooth-advertising",
                Label = "BT Advertising: Disable Bluetooth Advertising (BLE Beacon)",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [BtPolicy],
                Tags = ["bluetooth", "advertising", "ble", "beacon", "privacy", "security"],
                Description =
                    "Sets DisableAdvertising=1 in Bluetooth policy. Stops the Bluetooth adapter from "
                    + "broadcasting advertising packets (BLE beacon). Prevents passive tracking and "
                    + "reduces RF attack surface. Default: advertising enabled.",
                ApplyOps = [RegOp.SetDword(BtPolicy, "DisableAdvertising", 1)],
                RemoveOps = [RegOp.DeleteValue(BtPolicy, "DisableAdvertising")],
                DetectOps = [RegOp.CheckDword(BtPolicy, "DisableAdvertising", 1)],
            },
            new TweakDef
            {
                Id = "btadv-disable-promiscuous-mode",
                Label = "BT Advertising: Disable Bluetooth Promiscuous Mode",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [BthPort],
                Tags = ["bluetooth", "promiscuous", "sniffing", "security", "hardening"],
                Description =
                    "Sets PromiscuousMode=0 in BTHPORT Parameters. Prevents the Bluetooth adapter from "
                    + "entering promiscuous receive mode which would capture all BT packets in range. "
                    + "Default: 0 (already off). Explicit enforcement ensures the value is never changed.",
                ApplyOps = [RegOp.SetDword(BthPort, "PromiscuousMode", 0)],
                RemoveOps = [RegOp.DeleteValue(BthPort, "PromiscuousMode")],
                DetectOps = [RegOp.CheckDword(BthPort, "PromiscuousMode", 0)],
            },
            new TweakDef
            {
                Id = "btadv-disable-bluetooth-pairing-notification",
                Label = "BT Advertising: Disable Bluetooth Auto-Pairing Notifications",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [BtPolicy],
                Tags = ["bluetooth", "pairing", "notification", "lockdown", "security"],
                Description =
                    "Sets DisablePairingNotifications=1 in Bluetooth policy. Suppresses automatic "
                    + "pairing prompts when Bluetooth devices are discovered nearby. "
                    + "Default: notifications enabled. Prevents social engineering via proximity.",
                ApplyOps = [RegOp.SetDword(BtPolicy, "DisablePairingNotifications", 1)],
                RemoveOps = [RegOp.DeleteValue(BtPolicy, "DisablePairingNotifications")],
                DetectOps = [RegOp.CheckDword(BtPolicy, "DisablePairingNotifications", 1)],
            },
            new TweakDef
            {
                Id = "btadv-set-connectable-timeout-short",
                Label = "BT Advertising: Limit Bluetooth Discoverable/Connectable Timeout",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [BthPort],
                Tags = ["bluetooth", "discoverable", "timeout", "privacy", "security"],
                Description =
                    "Sets ConnectableTimeout=30 in BTHPORT Parameters. Limits how long the adapter "
                    + "remains in connectable mode after being made visible. "
                    + "Default: 180 seconds. Shorter window reduces passive attack exposure.",
                ApplyOps = [RegOp.SetDword(BthPort, "ConnectableTimeout", 30)],
                RemoveOps = [RegOp.DeleteValue(BthPort, "ConnectableTimeout")],
                DetectOps = [RegOp.CheckDword(BthPort, "ConnectableTimeout", 30)],
            },
            new TweakDef
            {
                Id = "btadv-disable-bluetooth-file-transfer",
                Label = "BT Advertising: Disable Bluetooth OBEX File Transfer",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [BtPolicy],
                Tags = ["bluetooth", "obex", "file-transfer", "security", "lockdown"],
                Description =
                    "Sets DisableFileTransfer=1 in Bluetooth policy. Blocks OBEX-based file exchange "
                    + "over Bluetooth (Push and FTP profiles). Prevents data exfiltration via wireless. "
                    + "Default: file transfer enabled. Recommended in high-security environments.",
                ApplyOps = [RegOp.SetDword(BtPolicy, "DisableFileTransfer", 1)],
                RemoveOps = [RegOp.DeleteValue(BtPolicy, "DisableFileTransfer")],
                DetectOps = [RegOp.CheckDword(BtPolicy, "DisableFileTransfer", 1)],
            },
            new TweakDef
            {
                Id = "btadv-disable-bluetooth-phonebook-access",
                Label = "BT Advertising: Disable Bluetooth Phone Book Access",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [BtPolicy],
                Tags = ["bluetooth", "phonebook", "pbap", "privacy", "security"],
                Description =
                    "Sets DisablePhoneBookAccess=1 in Bluetooth policy. Blocks the Phone Book Access "
                    + "Profile (PBAP). Prevents paired devices from reading local contacts. "
                    + "Default: PBAP enabled. Disabling protects contact data from BT-paired devices.",
                ApplyOps = [RegOp.SetDword(BtPolicy, "DisablePhoneBookAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(BtPolicy, "DisablePhoneBookAccess")],
                DetectOps = [RegOp.CheckDword(BtPolicy, "DisablePhoneBookAccess", 1)],
            },
            new TweakDef
            {
                Id = "btadv-require-bt-encryption",
                Label = "BT Advertising: Require Encryption on Bluetooth Connections",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [BthPort],
                Tags = ["bluetooth", "encryption", "security", "hardening"],
                Description =
                    "Sets EncryptionEnabled=1 in BTHPORT Parameters. Enforces that all Bluetooth "
                    + "connections use link-layer encryption. Unencrypted pairing attempts are rejected. "
                    + "Default: optional. Explicit enforcement prevents plaintext BT sessions.",
                ApplyOps = [RegOp.SetDword(BthPort, "EncryptionEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(BthPort, "EncryptionEnabled")],
                DetectOps = [RegOp.CheckDword(BthPort, "EncryptionEnabled", 1)],
            },
            new TweakDef
            {
                Id = "btadv-disable-remote-audio-playback",
                Label = "BT Advertising: Disable Remote Audio Playback over Bluetooth",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [BtPolicy],
                Tags = ["bluetooth", "audio", "a2dp", "remote", "security", "lockdown"],
                Description =
                    "Sets DisableRemoteAudioPlayback=1 in Bluetooth policy. Prevents audio streaming "
                    + "to remote Bluetooth devices (A2DP sink). Disables Bluetooth speakers as data channel. "
                    + "Default: audio playback allowed. Recommended in air-gapped/classified environments.",
                ApplyOps = [RegOp.SetDword(BtPolicy, "DisableRemoteAudioPlayback", 1)],
                RemoveOps = [RegOp.DeleteValue(BtPolicy, "DisableRemoteAudioPlayback")],
                DetectOps = [RegOp.CheckDword(BtPolicy, "DisableRemoteAudioPlayback", 1)],
            },
            new TweakDef
            {
                Id = "btadv-disable-bt-discoverable-state",
                Label = "BT Advertising: Force Bluetooth Always Non-Discoverable",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [BtPolicy],
                Tags = ["bluetooth", "discoverable", "visibility", "privacy", "security"],
                Description =
                    "Sets ForceNonDiscoverable=1 in Bluetooth policy. Keeps the Bluetooth adapter in "
                    + "non-discoverable state at all times. Prevents detection from BT scanning tools. "
                    + "Default: users can toggle discoverability. Policy lock prevents exposure.",
                ApplyOps = [RegOp.SetDword(BtPolicy, "ForceNonDiscoverable", 1)],
                RemoveOps = [RegOp.DeleteValue(BtPolicy, "ForceNonDiscoverable")],
                DetectOps = [RegOp.CheckDword(BtPolicy, "ForceNonDiscoverable", 1)],
            },
            new TweakDef
            {
                Id = "btadv-disable-bluetooth-shared-experiences",
                Label = "BT Advertising: Disable Bluetooth Shared Experiences",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [BtPolicy],
                Tags = ["bluetooth", "shared-experiences", "nearby-share", "privacy", "security"],
                Description =
                    "Sets DisableSharedExperiences=1 in Bluetooth policy. Blocks the Bluetooth-based "
                    + "'Shared Experiences' feature which is used for Nearby Share file transfers. "
                    + "Default: enabled. Disabling removes an additional passive data transfer vector.",
                ApplyOps = [RegOp.SetDword(BtPolicy, "DisableSharedExperiences", 1)],
                RemoveOps = [RegOp.DeleteValue(BtPolicy, "DisableSharedExperiences")],
                DetectOps = [RegOp.CheckDword(BtPolicy, "DisableSharedExperiences", 1)],
            },
        ];

    }

    // ── DeviceCompliancePolicy ──
    private static class _DeviceCompliancePolicy
    {
        private const string DhaKey =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceHealthAttestation";

        private const string HcKey =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HealthCenter";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "devcpl-enable-health-attestation",
                    Label = "Device Compliance: Enable Device Health Attestation",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Sets EnableHealthAttestation=1 in DeviceHealthAttestation policy. Enables the Windows Device Health Attestation (DHA) service which uses the device's TPM to cryptographically attest its boot sequence. The DHA service generates a health certificate that can be consumed by MDM providers (Intune, SCCM) and conditional access systems to verify that the device booted without tampering: Secure Boot was enabled, BitLocker is active, the boot path was not modified, and no ELAM-detected malware was present. Without DHA, conditional access can only rely on OS-reported state — which malware can spoof.",
                    Tags = ["dha", "health-attestation", "tpm", "conditional-access", "boot-integrity"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Device boot integrity is cryptographically attested using the TPM. Requires TPM 2.0. Health certificates are generated and periodically sent to the configured DHA server. Enables hardware-backed conditional access decisions.",
                    ApplyOps = [RegOp.SetDword(DhaKey, "EnableHealthAttestation", 1)],
                    RemoveOps = [RegOp.DeleteValue(DhaKey, "EnableHealthAttestation")],
                    DetectOps = [RegOp.CheckDword(DhaKey, "EnableHealthAttestation", 1)],
                },
                new TweakDef
                {
                    Id = "devcpl-require-bitlocker-for-compliance",
                    Label = "Device Compliance: Require BitLocker Encryption for Compliance",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Sets RequireBitLockerForCompliance=1 in HealthCenter policy. Marks a device as non-compliant if BitLocker Drive Encryption is not enabled on the system drive. Compliance status is reported to MDM (Intune/SCCM) and can trigger conditional access policies that block the device from connecting to corporate resources until BitLocker is enabled. Data loss from stolen or lost unencrypted laptops is one of the most common sources of data breaches. Requiring BitLocker for compliance ensures all mobile devices connecting to corporate resources are encrypted.",
                    Tags = ["compliance", "bitlocker", "encryption", "conditional-access", "data-protection"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Devices without BitLocker on the system drive report as non-compliant. Non-compliant devices may be blocked from corporate resources via conditional access. Requires MDM enrolment and conditional access policies to enforce the compliance gate.",
                    ApplyOps = [RegOp.SetDword(HcKey, "RequireBitLockerForCompliance", 1)],
                    RemoveOps = [RegOp.DeleteValue(HcKey, "RequireBitLockerForCompliance")],
                    DetectOps = [RegOp.CheckDword(HcKey, "RequireBitLockerForCompliance", 1)],
                },
                new TweakDef
                {
                    Id = "devcpl-require-antivirus-for-compliance",
                    Label = "Device Compliance: Require Active Antivirus for Compliance",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Sets RequireAntivirusForCompliance=1 in HealthCenter policy. Marks a device as non-compliant if a registered and up-to-date antivirus product is not detected by the Security Center. Real-time protection must be active and signatures cannot be critically outdated. Devices that have disabled antivirus, have expired protection subscriptions, or have antivirus that is consuming no CPU (indicative of process termination by malware) are flagged. Security Center status is checked periodically and on every MDM compliance check cycle.",
                    Tags = ["compliance", "antivirus", "security-center", "real-time-protection", "endpoint"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Devices without active, up-to-date antivirus report as non-compliant. Devices with disabled or expired AV may lose access to corporate resources. Requires MDM and conditional access to enforce. Windows Defender Antivirus or any ELAM-registered product satisfies the requirement.",
                    ApplyOps = [RegOp.SetDword(HcKey, "RequireAntivirusForCompliance", 1)],
                    RemoveOps = [RegOp.DeleteValue(HcKey, "RequireAntivirusForCompliance")],
                    DetectOps = [RegOp.CheckDword(HcKey, "RequireAntivirusForCompliance", 1)],
                },
                new TweakDef
                {
                    Id = "devcpl-set-compliance-check-interval-4h",
                    Label = "Device Compliance: Set Compliance Check Interval to 4 Hours",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Sets ComplianceCheckIntervalHours=4 in HealthCenter policy. Sets the interval at which Windows re-evaluates device compliance state and sends the current status to the MDM provider. A default compliance check interval that is too long (24+ hours) means a device that becomes non-compliant (user disables BitLocker, AV signs expire, firewall turned off) continues to access corporate resources for up to a day before its compliance status is updated. 4 hours ensures compliance violations are detected and reflected in conditional access within the business day after they occur.",
                    Tags = ["compliance", "check-interval", "mdm", "detection", "conditional-access"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Compliance state is evaluated every 4 hours. A device that becomes non-compliant is detected within 4 hours. Slightly higher MDM service check-in frequency — negligible network overhead.",
                    ApplyOps = [RegOp.SetDword(HcKey, "ComplianceCheckIntervalHours", 4)],
                    RemoveOps = [RegOp.DeleteValue(HcKey, "ComplianceCheckIntervalHours")],
                    DetectOps = [RegOp.CheckDword(HcKey, "ComplianceCheckIntervalHours", 4)],
                },
                new TweakDef
                {
                    Id = "devcpl-require-secure-boot-for-compliance",
                    Label = "Device Compliance: Require Secure Boot for Compliance",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Sets RequireSecureBootForCompliance=1 in HealthCenter policy. Marks a device as non-compliant if UEFI Secure Boot is not enabled. Secure Boot prevents bootkit malware and rootkits from replacing the boot path with untrusted code — without Secure Boot, an attacker with brief physical access can boot from a USB drive to bypass Windows authentication or install a persistent bootkit. Devices with Secure Boot disabled cannot be trusted to run an uncompromised OS. This check complements DHA attestation with a policy-layer enforcement.",
                    Tags = ["compliance", "secure-boot", "uefi", "bootkit", "physical-security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Devices without Secure Boot enabled report as non-compliant. Very old hardware (pre-2012) may not support Secure Boot. Devices that were deliberately configured without Secure Boot for BIOS compatibility reasons must be re-evaluated.",
                    ApplyOps = [RegOp.SetDword(HcKey, "RequireSecureBootForCompliance", 1)],
                    RemoveOps = [RegOp.DeleteValue(HcKey, "RequireSecureBootForCompliance")],
                    DetectOps = [RegOp.CheckDword(HcKey, "RequireSecureBootForCompliance", 1)],
                },
                new TweakDef
                {
                    Id = "devcpl-enable-compliance-grace-period-7days",
                    Label = "Device Compliance: Enable 7-Day Grace Period for Non-Compliant Devices",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Sets ComplianceGracePeriodDays=7 in HealthCenter policy. Grants newly enrolled devices or devices that first become non-compliant a 7-day grace period before conditional access blocks are enforced. Without a grace period, a device that enrolls in MDM but has not yet completed all compliance remediation (BitLocker encrypting, definitions updating) is immediately blocked from corporate resources — creating a chicken-and-egg problem. The grace period allows IT to remediate the device before it loses access. After 7 days without remediation, access restrictions are enforced.",
                    Tags = ["compliance", "grace-period", "enrolment", "remediation", "mdm"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Non-compliant devices have 7 days to reach compliance before access restrictions are applied. Provides IT time for remediation without disrupting new enrolments. After 7 days, non-compliant devices are subject to conditional access blocks.",
                    ApplyOps = [RegOp.SetDword(HcKey, "ComplianceGracePeriodDays", 7)],
                    RemoveOps = [RegOp.DeleteValue(HcKey, "ComplianceGracePeriodDays")],
                    DetectOps = [RegOp.CheckDword(HcKey, "ComplianceGracePeriodDays", 7)],
                },
                new TweakDef
                {
                    Id = "devcpl-require-minimum-os-build",
                    Label = "Device Compliance: Require Minimum OS Build for Compliance",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Sets RequireMinimumOsBuild=1 in HealthCenter policy. Enables minimum OS build checking as a compliance criterion. When enabled, devices running OS builds older than the configured minimum (set separately as MinimumBuildNumber) report as non-compliant. This policy ensures that devices running versions of Windows that are out of Microsoft's support cycle (no security patches) or that have known unpatched critical vulnerabilities are flagged before they access corporate resources. Combined with Windows Update policies, this creates an enforced minimum security baseline.",
                    Tags = ["compliance", "os-build", "patch-level", "security-baseline", "outdated-os"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Devices below the minimum OS build report as non-compliant. Requires configuring MinimumBuildNumber separately. Devices on unsupported or unpatched OS build are blocked pending upgrade. Coordinate with Windows Update deadline policies.",
                    ApplyOps = [RegOp.SetDword(HcKey, "RequireMinimumOsBuild", 1)],
                    RemoveOps = [RegOp.DeleteValue(HcKey, "RequireMinimumOsBuild")],
                    DetectOps = [RegOp.CheckDword(HcKey, "RequireMinimumOsBuild", 1)],
                },
                new TweakDef
                {
                    Id = "devcpl-require-firewall-for-compliance",
                    Label = "Device Compliance: Require Windows Firewall Active for Compliance",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Sets RequireFirewallForCompliance=1 in HealthCenter policy. Marks a device as non-compliant if Windows Defender Firewall (or a registered third-party firewall) is not active on all network profiles (domain, private, public). The Windows Firewall is a critical network-based attack prevention control. Users may disable the firewall when troubleshooting connection issues and forget to re-enable it. A device with no host firewall on a public network is exposed to direct network attacks. This compliance check ensures firewalls stay active.",
                    Tags = ["compliance", "firewall", "network-protection", "security-center", "perimeter"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Devices with disabled Windows Defender Firewall or no registered firewall are non-compliant. Third-party firewalls registered with Security Center satisfy the requirement. Devices that turned off the firewall for temporary diagnostics and forgot to restore will be flagged.",
                    ApplyOps = [RegOp.SetDword(HcKey, "RequireFirewallForCompliance", 1)],
                    RemoveOps = [RegOp.DeleteValue(HcKey, "RequireFirewallForCompliance")],
                    DetectOps = [RegOp.CheckDword(HcKey, "RequireFirewallForCompliance", 1)],
                },
                new TweakDef
                {
                    Id = "devcpl-block-noncompliant-resource-access",
                    Label = "Device Compliance: Block Non-Compliant Devices from Joining AD Resources",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Sets BlockNonCompliantNetworkAccess=1 in HealthCenter policy. Enables a local enforcement hook that checks compliance state before allowing the device to connect to protected network resources. When this is enabled and the device is marked non-compliant by the health centre, outbound connections to domain-classified resources can be blocked at the Windows Filtering Platform (WFP) layer. This provides local enforcement independent of whether external conditional access (AAD, MFA, proxy) is in place — useful as defence-in-depth for environments where some legacy resources lack conditional access support.",
                    Tags = ["compliance", "network-access", "block", "conditional-access", "wfp"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote = "Non-compliant devices are blocked from accessing domain network resources at the WFP layer. This is a local enforcement on the device itself — not a network-layer block. A device misidentifying its compliance state may block its own legitimate access. Test thoroughly before broad deployment.",
                    ApplyOps = [RegOp.SetDword(HcKey, "BlockNonCompliantNetworkAccess", 1)],
                    RemoveOps = [RegOp.DeleteValue(HcKey, "BlockNonCompliantNetworkAccess")],
                    DetectOps = [RegOp.CheckDword(HcKey, "BlockNonCompliantNetworkAccess", 1)],
                },
                new TweakDef
                {
                    Id = "devcpl-enable-tpm-attestation-logging",
                    Label = "Device Compliance: Enable TPM Health Attestation Event Logging",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Sets TpmAttestationLogging=1 in DeviceHealthAttestation policy. Enables event log entries for TPM health attestation operations: TPM measurement capture, health certificate request, health certificate delivery, and health attestation failures. Without attestation logging, diagnosing why a device cannot obtain a health certificate (TPM in reduced functionality mode, endorsement key provisioning failure, attestation service unreachable) is difficult. Log entries enable IT helpdesk to diagnose attestation failures and restore compliance without escalating to infrastructure teams.",
                    Tags = ["tpm", "attestation", "logging", "compliance", "diagnostics"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "TPM attestation events are logged. Events include certificate request, success, failure, and failure reasons. Negligible disk overhead. Enables rapid helpdesk diagnosis of attestation failures without advanced tooling.",
                    ApplyOps = [RegOp.SetDword(DhaKey, "TpmAttestationLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(DhaKey, "TpmAttestationLogging")],
                    DetectOps = [RegOp.CheckDword(DhaKey, "TpmAttestationLogging", 1)],
                },
            ];

    }

    // ── DeviceEnrollmentLimitPolicy ──
    private static class _DeviceEnrollmentLimitPolicy
    {
        private const string EnlKey =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceEnrollment";

        private const string MdmKey =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\MDM";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "devenl-set-max-devices-per-user-5",
                    Label = "Device Enrollment Limit: Set Maximum Devices per User to 5",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Sets MaxDevicesPerUser=5 in DeviceEnrollment policy. Limits the number of devices a single user account can enroll in MDM to 5. Without per-user limits, a single compromised account can be used to enroll large numbers of devices into the MDM tenant, consuming Intune licenses, polluting the device inventory, and potentially using the MDM service to push malware to enrolled devices. A limit of 5 is generous enough for users with a phone, tablet, laptop, home PC, and a spare device, while preventing bulk enrollment abuse.",
                    Tags = ["enrollment", "device-limit", "per-user", "abuse-prevention", "inventory"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Each user can enroll a maximum of 5 devices. Attempts to enroll a 6th device are rejected until an existing device is unenrolled. Adjust the limit if your organisation has users with more than 5 managed devices (e.g., kiosk operators managing multiple shared devices with a single service account).",
                    ApplyOps = [RegOp.SetDword(EnlKey, "MaxDevicesPerUser", 5)],
                    RemoveOps = [RegOp.DeleteValue(EnlKey, "MaxDevicesPerUser")],
                    DetectOps = [RegOp.CheckDword(EnlKey, "MaxDevicesPerUser", 5)],
                },
                new TweakDef
                {
                    Id = "devenl-block-byod-personal-enrollment",
                    Label = "Device Enrollment Limit: Block Personal BYOD Devices from Enrolling",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Sets BlockPersonalDeviceEnrollment=1 in DeviceEnrollment policy. Prevents devices that are registered as personal devices (not Azure AD Joined or Hybrid Joined) from enrolling in corporate MDM. A personally-owned device that enrolls in corporate MDM becomes subject to remote wipe commands — which could irreversibly delete personal data. Blocking personal device enrollment prevents accidental enrollment of personal hardware into MDM while protecting users' personal devices from corporate management actions. Users who need BYOD access should use Workplace Join with limited MDM (MAM without device enrollment) instead.",
                    Tags = ["byod", "personal-device", "enrollment-block", "remote-wipe-protection", "privacy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Personal (non-AAD-Joined) devices cannot enroll in corporate MDM. Users who attempt to add a work account on a personal device get a generic failure. BYOD users should sign in with MAM-only (app-level management) via Outlook or Teams apps instead. Requires AAD Join for full MDM enrollment.",
                    ApplyOps = [RegOp.SetDword(EnlKey, "BlockPersonalDeviceEnrollment", 1)],
                    RemoveOps = [RegOp.DeleteValue(EnlKey, "BlockPersonalDeviceEnrollment")],
                    DetectOps = [RegOp.CheckDword(EnlKey, "BlockPersonalDeviceEnrollment", 1)],
                },
                new TweakDef
                {
                    Id = "devenl-require-device-category-on-enroll",
                    Label = "Device Enrollment Limit: Require Device Category Assignment at Enrollment",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Sets RequireDeviceCategoryOnEnrollment=1 in DeviceEnrollment policy. Requires administrators to assign a device category (e.g., Corporate Laptop, Kiosk, Shared Workstation) at enrollment time. Device categories in Intune are used to automatically assign devices to dynamic groups, which in turn receive different policy sets. Without mandatory category assignment, all devices land in the uncategorised default group and receive a single policy set. Mandatory categories ensure that kiosk devices, shared workstations, and executive laptops each receive appropriately scoped policies from the moment of enrollment.",
                    Tags = ["enrollment", "device-category", "dynamic-group", "policy-scoping", "intune"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Device category must be assigned before enrollment completes. Enrollment fails if no category is selected. Category assignment is performed by the enrolling admin or in automated flows by the Autopilot assignment group. No user-facing UI change for standard users.",
                    ApplyOps = [RegOp.SetDword(EnlKey, "RequireDeviceCategoryOnEnrollment", 1)],
                    RemoveOps = [RegOp.DeleteValue(EnlKey, "RequireDeviceCategoryOnEnrollment")],
                    DetectOps = [RegOp.CheckDword(EnlKey, "RequireDeviceCategoryOnEnrollment", 1)],
                },
                new TweakDef
                {
                    Id = "devenl-block-unused-enrollment-profiles",
                    Label = "Device Enrollment Limit: Block Devices Without an Enrollment Profile",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Sets RequireEnrollmentProfile=1 in DeviceEnrollment policy. Prevents devices from enrolling unless they match a pre-configured Intune enrollment profile (Device Enrollment Program, Autopilot profile, or bulk enrollment token). Without this restriction, any device that has credentials for a licensed user can self-enroll in MDM using the standard Settings > Accounts flow. Pre-requiring an enrollment profile means that only devices that IT has explicitly authorized for enrollment (by creating or assigning a profile) can join MDM — unknown or unauthorized devices are rejected.",
                    Tags = ["enrollment", "enrollment-profile", "autopilot", "authorization", "unknown-devices"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Only devices that match a pre-configured enrollment profile can enroll. Devices without a matching profile are rejected at enrollment. Devices must be registered in Intune/Autopilot before attempting enrollment. Prevents rogue devices from enrolling with valid user credentials.",
                    ApplyOps = [RegOp.SetDword(EnlKey, "RequireEnrollmentProfile", 1)],
                    RemoveOps = [RegOp.DeleteValue(EnlKey, "RequireEnrollmentProfile")],
                    DetectOps = [RegOp.CheckDword(EnlKey, "RequireEnrollmentProfile", 1)],
                },
                new TweakDef
                {
                    Id = "devenl-restrict-enrollment-to-aad-join",
                    Label = "Device Enrollment Limit: Restrict MDM Enrollment to AAD Joined Devices Only",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Sets RestrictEnrollmentToAadJoin=1 in MDM policy. Prevents MDM enrollment from completing unless the device is Azure AD Joined (not just Workplace Joined). Workplace Join provides a limited form of registration that does not require the device to be AAD-joined — this allows personal devices to register without a full AAD Join. By restricting enrollment to AAD Join, this policy ensures that enrolled devices are fully registered in Azure AD with a machine account, which is required for Hybrid Join, Conditional Access device trust, and all domain-level group policies backed by AAD.",
                    Tags = ["enrollment", "aad-join", "device-trust", "conditional-access", "hybrid-join"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Enrollment is restricted to devices that complete Azure AD Join. Workplace Join-only devices cannot enroll. Hybrid Joined devices (on-premises AD + AAD) satisfy the AAD Join requirement. Purely on-premises AD-joined devices without AAD sync must Hybrid-Join before they can enroll.",
                    ApplyOps = [RegOp.SetDword(MdmKey, "RestrictEnrollmentToAadJoin", 1)],
                    RemoveOps = [RegOp.DeleteValue(MdmKey, "RestrictEnrollmentToAadJoin")],
                    DetectOps = [RegOp.CheckDword(MdmKey, "RestrictEnrollmentToAadJoin", 1)],
                },
                new TweakDef
                {
                    Id = "devenl-enable-enrollment-status-page",
                    Label = "Device Enrollment Limit: Enable MDM Enrollment Status Page During OOBE",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Sets ShowEnrollmentStatusPage=1 in DeviceEnrollment policy. Enables the Enrollment Status Page (ESP) during Autopilot or standard OOBE enrollment. The ESP shows the user (and IT) the real-time progress of device setup: account provisioning, app installations, policy applications, and certificate enrollments. Without the ESP, the user is deposited at the desktop while apps are still installing or policies are still applying — the device may appear functional but actually be in an incomplete configuration state. The ESP holds the user at the setup screen until all critical configurations are complete.",
                    Tags = ["enrollment", "esp", "oobe", "autopilot", "setup-progress"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Enrollment Status Page is shown during Autopilot/OOBE. Users are blocked at the ESP until all required apps and policies are applied. Prevents users from using a partially-configured device. Increases initial setup time by the duration of app installations.",
                    ApplyOps = [RegOp.SetDword(EnlKey, "ShowEnrollmentStatusPage", 1)],
                    RemoveOps = [RegOp.DeleteValue(EnlKey, "ShowEnrollmentStatusPage")],
                    DetectOps = [RegOp.CheckDword(EnlKey, "ShowEnrollmentStatusPage", 1)],
                },
                new TweakDef
                {
                    Id = "devenl-block-enrollment-from-unknown-networks",
                    Label = "Device Enrollment Limit: Block Enrollment Attempts from Non-Corporate Networks",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Sets RequireCorporateNetworkForEnrollment=1 in DeviceEnrollment policy. Restricts MDM enrollment to devices on corporate networks (defined by the network location awareness profile). Enrollment attempts from unclassified or public networks are blocked. This prevents bulk enrollment of devices by an attacker using stolen credentials from outside the corporate network perimeter. While this is most relevant for legacy MDM setups without Azure AD conditional access, it adds network perimeter enforcement as an extra enrollment control — enrollment over public networks requires re-evaluation of the risk posture.",
                    Tags = ["enrollment", "network-restriction", "corporate-network", "nla", "perimeter"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 3,
                    ImpactNote = "MDM enrollment is only permitted from networks classified as corporate (domain controller reachable, NLA domain profile active). Devices on guest, public, or unclassified networks cannot enroll. This may prevent legitimate remote onboarding — coordinate with VPN policies to ensure remote enrollment is still possible via corporate VPN.",
                    ApplyOps = [RegOp.SetDword(EnlKey, "RequireCorporateNetworkForEnrollment", 1)],
                    RemoveOps = [RegOp.DeleteValue(EnlKey, "RequireCorporateNetworkForEnrollment")],
                    DetectOps = [RegOp.CheckDword(EnlKey, "RequireCorporateNetworkForEnrollment", 1)],
                },
                new TweakDef
                {
                    Id = "devenl-log-enrollment-failures",
                    Label = "Device Enrollment Limit: Enable Detailed Logging of Enrollment Failures",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Sets LogEnrollmentFailures=1 in DeviceEnrollment policy. Enables detailed logging of MDM enrollment failure events to the Windows Event Log. Enrollment failures are written to the Microsoft-Windows-DeviceManagement-Enterprise-Diagnostics-Provider/Admin channel with structured error codes (HRESULT), the enrollment phase that failed (token acquisition, DRS discovery, enrollment registration, certificate acquisition), and whether the failure was a network error, authentication error, or server error. This significantly accelerates helpdesk troubleshooting of Autopilot and enrollment failures.",
                    Tags = ["enrollment", "failure-logging", "event-log", "diagnostics", "helpdesk"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "MDM enrollment failures are logged with structured error codes and phase information. Logs written to the Microsoft-Windows-DeviceManagement-Enterprise-Diagnostics-Provider channel. No performance impact — logging only occurs on failure paths.",
                    ApplyOps = [RegOp.SetDword(EnlKey, "LogEnrollmentFailures", 1)],
                    RemoveOps = [RegOp.DeleteValue(EnlKey, "LogEnrollmentFailures")],
                    DetectOps = [RegOp.CheckDword(EnlKey, "LogEnrollmentFailures", 1)],
                },
                new TweakDef
                {
                    Id = "devenl-require-mfa-for-enrollment",
                    Label = "Device Enrollment Limit: Require MFA at MDM Enrollment Time",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Sets RequireMfaForEnrollment=1 in DeviceEnrollment policy. Requires multi-factor authentication at the time of MDM enrollment in addition to the standard password credential. Without MFA at enrollment, a stolen password is sufficient to enroll an attacker's device into the corporate MDM tenant. With enrollment MFA enforced, the attacker must also have the victim's second factor (phone, hardware key) to complete enrollment. MDM enrollment grants the device significant privileges (policy application, certificate issuance, resource access upon compliance) — requiring MFA at this critical step is essential.",
                    Tags = ["mfa", "enrollment", "authentication", "conditional-access", "identity"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Users must complete MFA during MDM enrollment. Requires Azure AD MFA or equivalent. Autopilot deployments using device-identity-based enrollment (PPKG or DEM account) may need exemption. Test Autopilot flows before broad deployment.",
                    ApplyOps = [RegOp.SetDword(EnlKey, "RequireMfaForEnrollment", 1)],
                    RemoveOps = [RegOp.DeleteValue(EnlKey, "RequireMfaForEnrollment")],
                    DetectOps = [RegOp.CheckDword(EnlKey, "RequireMfaForEnrollment", 1)],
                },
                new TweakDef
                {
                    Id = "devenl-audit-enrollment-activity",
                    Label = "Device Enrollment Limit: Audit All Device Enrollment Activity",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Sets AuditEnrollmentActivity=1 in DeviceEnrollment policy. Enables audit logging for all device enrollment activity: successful enrollments, failed enrollment attempts, enrollment profile matching and rejection, and unenrollment events. Audit records include the user UPN that initiated the enrollment, the device serial number and hardware ID, the enrollment profile matched (or lack thereof), and the outcome. Enrollment audit logs are written to the Microsoft-Windows-DeviceManagement-Enterprise-Diagnostics-Provider/Admin channel and can be forwarded to SIEM for detection of rogue enrollment attempts.",
                    Tags = ["enrollment", "audit", "siem", "monitoring", "security-event"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "All enrollment attempts are logged with user, device, and outcome details. Audit events can be forwarded to SIEM. Detection: unusually high enrollment failures from a single user may indicate credential stuffing. No performance overhead — logging is asynchronous.",
                    ApplyOps = [RegOp.SetDword(EnlKey, "AuditEnrollmentActivity", 1)],
                    RemoveOps = [RegOp.DeleteValue(EnlKey, "AuditEnrollmentActivity")],
                    DetectOps = [RegOp.CheckDword(EnlKey, "AuditEnrollmentActivity", 1)],
                },
            ];

    }

    // ── DeviceEnrollmentPolicy ──
    private static class _DeviceEnrollmentPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\MDMEnrollment";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "devenrl-disable-mdm-enrollment",
                Label = "Disable Automatic MDM Enrollment with Azure AD Join",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                Description =
                    "Automatic MDM enrollment triggers when a device joins Azure Active Directory and automatically enrolls it in the linked Intune Mobile Device Management tenant. Disabling automatic MDM enrollment prevents devices from auto-enrolling in MDM when users join their Azure AD accounts to devices. In managed environments where all devices should be enrolled, automatic enrollment is desirable, but in specialized scenarios enrollment may need to be controlled. Specialized devices like developer workstations, lab systems, or shared equipment may have specific reasons to avoid automatic MDM enrollment. Disabling auto-enrollment does not prevent manual IT-initiated enrollment which can still occur through IT-directed processes. Organizations should carefully evaluate this setting as it can create unmanaged device gaps in environments expecting universal MDM coverage.",
                Tags = ["mdm", "enrollment", "azure-ad", "device-management", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AutoEnrollMDM", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AutoEnrollMDM")],
                DetectOps = [RegOp.CheckDword(Key, "AutoEnrollMDM", 0)],
            },
            new TweakDef
            {
                Id = "devenrl-disable-bulk-enrollment",
                Label = "Disable Bulk MDM Enrollment via Provisioning Packages",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Bulk enrollment provisioning packages allow an administrator to enroll multiple devices in MDM simultaneously using a pre-configured package. Disabling bulk enrollment prevents provisioning packages from enrolling devices without interactive authentication preventing unauthorized mass enrollment. Bulk enrollment packages contain authentication credentials and if the package is captured it could be used to enroll unauthorized devices into the MDM tenant. IT administrators should use certificate-based bulk enrollment with short-validity certificates rather than username and password provisioning packages. Disabling bulk enrollment forces all device enrollment to use individual authenticated enrollment preventing bulk package replay attacks. Organizations that need bulk enrollment should use Windows Autopilot instead which provides stronger enrollment authentication.",
                Tags = ["mdm", "bulk-enrollment", "provisioning", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableBulkEnrollment", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableBulkEnrollment")],
                DetectOps = [RegOp.CheckDword(Key, "DisableBulkEnrollment", 1)],
            },
            new TweakDef
            {
                Id = "devenrl-enable-enrollment-status-page",
                Label = "Enable MDM Enrollment Status Page During Autopilot",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "The MDM Enrollment Status Page blocks user access to the device until all required MDM policies and applications are successfully applied during Autopilot provisioning. Enabling the enrollment status page ensures that users cannot bypass required security configurations by accessing the device before MDM setup is complete. Without the enrollment status page users can log on before required security applications like endpoint protection are installed creating a window of vulnerability. The enrollment status page prevents devices from entering use without all compliance and security configurations required by MDM policy. Blocking access during enrollment is particularly important for security-critical configurations like full disk encryption that must complete before data is created. Organizations should configure a meaningful timeout and error handling to prevent enrollment failures from permanently blocking device access.",
                Tags = ["mdm", "enrollment-status", "autopilot", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableEnrollmentStatusPage", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableEnrollmentStatusPage")],
                DetectOps = [RegOp.CheckDword(Key, "EnableEnrollmentStatusPage", 1)],
            },
            new TweakDef
            {
                Id = "devenrl-block-unknown-unenrollment",
                Label = "Block User-Initiated MDM Unenrollment",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "User-initiated MDM unenrollment allows end users to remove corporate management from their devices through the Settings application. Blocking user-initiated unenrollment prevents employees from removing corporate MDM management to evade security policies or monitoring. Unenrollment from MDM would remove all deployed security policies, applications, and configurations leaving the device non-compliant. Disabling unenrollment ensures that devices remain under corporate management for their operational lifetime without requiring IT intervention to re-enroll. Blocking unenrollment is particularly important for CYOD and COPE scenarios where corporate data must remain protected at all times. IT processes for legitimate device retirement or reassignment should include formal MDM unenrollment through administrative procedures rather than user self-service.",
                Tags = ["mdm", "unenrollment", "device-management", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisallowMDMUnenrollment", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisallowMDMUnenrollment")],
                DetectOps = [RegOp.CheckDword(Key, "DisallowMDMUnenrollment", 1)],
            },
            new TweakDef
            {
                Id = "devenrl-require-enrollment-compliance",
                Label = "Require MDM Enrollment Compliance Before Resource Access",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "MDM enrollment compliance requirements block access to corporate resources from devices that are not enrolled in MDM management. Requiring enrollment compliance implements zero-trust access principles by ensuring only managed devices can access corporate email, applications, and data. Non-enrolled devices lack the security configuration baselines, endpoint protection, and monitoring that managed endpoints provide. Compliance-gated resource access forces all devices seeking corporate data to register under management before receiving access. Organizations should combine enrollment requirements with conditional access policies in Azure AD and Intune for comprehensive enforcement. Compliance requirements should be communicated to users during device provisioning so they understand the enrollment requirement for resource access.",
                Tags = ["mdm", "compliance", "access-control", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequireComplianceCheck", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireComplianceCheck")],
                DetectOps = [RegOp.CheckDword(Key, "RequireComplianceCheck", 1)],
            },
            new TweakDef
            {
                Id = "devenrl-enable-enrollment-certificate-auth",
                Label = "Require Certificate Authentication for MDM Enrollment",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Certificate authentication for MDM enrollment replaces username and password credentials with device certificates that are more resistant to phishing and credential theft. Requiring certificate authentication ensures that only devices with valid enterprise certificates issued by the organizational PKI can enroll in MDM. Strong device identity through certificates ensures that MDM enrollment is limited to devices that have gone through the IT provisioning process. Certificate-based enrollment prevents attackers from enrolling unauthorized devices using stolen credentials. Enterprise certificates for device enrollment should be issued with appropriate validity periods and revocation capabilities. Certificate authentication for MDM enrollment aligns with zero-trust principles of verified device identity before granting management access.",
                Tags = ["mdm", "certificate-auth", "enrollment", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequireCertificateAuth", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireCertificateAuth")],
                DetectOps = [RegOp.CheckDword(Key, "RequireCertificateAuth", 1)],
            },
            new TweakDef
            {
                Id = "devenrl-audit-enrollment-events",
                Label = "Enable Audit Logging for MDM Enrollment Events",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "MDM enrollment audit logging records all enrollment actions including successful enrollments, failed attempts, and unenrollment operations. Enabling enrollment audit logging provides a complete record of device management changes for security investigation and compliance reporting. Enrollment logs help detect unauthorized enrollment attempts by unauthorized users trying to add managed credentials to devices they should not access. Unenrollment events in audit logs can alert security teams when devices are removed from management unexpectedly. MDM enrollment audit events should be forwarded to SIEM for correlation with other device and identity events. Regular review of enrollment audit logs helps identify devices that have been enrolled multiple times which may indicate credential theft or device cloning attempts.",
                Tags = ["mdm", "audit", "enrollment-logging", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AuditEnrollmentEvents", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditEnrollmentEvents")],
                DetectOps = [RegOp.CheckDword(Key, "AuditEnrollmentEvents", 1)],
            },
            new TweakDef
            {
                Id = "devenrl-set-enrollment-retry-limit",
                Label = "Set Maximum MDM Enrollment Retry Limit",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "MDM enrollment retry limits prevent automated brute-force enrollment attempts by limiting the number of consecutive enrollment failures before locking the enrollment channel. Setting enrollment retry limits reduces the effectiveness of automated attacks against MDM enrollment endpoints using credential stuffing or brute force. Excessive enrollment failures may indicate a misconfigured provisioning package or an unauthorized device attempting to enroll using stolen credentials. After reaching the retry limit the device should require IT intervention to reset before enrollment can be attempted again. The retry limit should be set high enough to accommodate legitimate transient network failures but low enough to detect automated attack patterns. Enrollment retry events should be monitored and alerts triggered when retry limits are approached or reached.",
                Tags = ["mdm", "retry-limit", "brute-force-protection", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "MaxEnrollmentRetries", 5)],
                RemoveOps = [RegOp.DeleteValue(Key, "MaxEnrollmentRetries")],
                DetectOps = [RegOp.CheckDword(Key, "MaxEnrollmentRetries", 5)],
            },
            new TweakDef
            {
                Id = "devenrl-enforce-enrollment-encryption",
                Label = "Enforce BitLocker Before MDM Enrollment Completion",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "BitLocker enforcement during MDM enrollment ensures that full disk encryption is enabled before the device is classified as enrolled and compliant. Requiring BitLocker during enrollment ensures that corporate data cannot be created on unencrypted devices that could bypass data loss prevention policies. Enrollment-time BitLocker enforcement prevents devices from accessing corporate resources until encryption is fully enabled and the recovery key is escrowed to Azure AD or Active Directory. Without enrollment-time BitLocker enforcement a device could access corporate data with a temporary compliance bypass before encryption is configured. BitLocker Silent Encryption using key escrow to Azure AD provides zero-touch encryption during Autopilot enrollment without user interaction. BitLocker enforcement requirements should be defined in the MDM compliance policy and verified before granting access to sensitive resources.",
                Tags = ["mdm", "bitlocker", "encryption", "enrollment", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequireBitLockerAtEnrollment", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireBitLockerAtEnrollment")],
                DetectOps = [RegOp.CheckDword(Key, "RequireBitLockerAtEnrollment", 1)],
            },
            new TweakDef
            {
                Id = "devenrl-restrict-enrollment-to-approved-tenant",
                Label = "Restrict MDM Enrollment to Approved Tenant Only",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "Tenant enrollment restriction limits MDM enrollment to a specific Azure AD tenant preventing devices from being enrolled in unauthorized or attacker-controlled tenants. Restricting enrollment to the approved tenant prevents adversaries from enrolling corporate devices in a rogue MDM tenant to gain management control. Tenant-restricted enrollment is particularly important for corporate shared devices and lab equipment that may be accessed by multiple users. Without tenant restrictions a user with global administrator rights in a different tenant could enroll a device they have physical access to. Enrollment tenant restrictions should be configured through Windows Registry as a machine-wide policy that applies regardless of the currently signed-in user. Organizations should combine tenant enrollment restrictions with Windows Defender ATP device enrollment to ensure all devices are enrolled in the correct tenant.",
                Tags = ["mdm", "tenant-restriction", "enrollment", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictToApprovedTenant", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictToApprovedTenant")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictToApprovedTenant", 1)],
            },
        ];

    }

    // ── DeviceGuardPolicy ──
    private static class _DeviceGuardPolicy
    {
        private const string DgKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceGuard";

        public static IReadOnlyList<TweakDef> Data =>
        [
            new TweakDef
            {
                Id = "devguard-enable-vbs",
                Label = "Enable Virtualization-Based Security (VBS)",
                Category = "Device & Hardware Policy",
                Description = "Enables Virtualization-Based Security, which uses the hypervisor to isolate critical security processes.",
                Tags = ["vbs", "device-guard", "virtualization", "security", "hyperv"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 3,
                ImpactNote = "Requires Hyper-V, UEFI, and Secure Boot; significant performance impact on systems without HVCI-capable hardware.",
                ApplyOps = [RegOp.SetDword(DgKey, "EnableVirtualizationBasedSecurity", 1)],
                RemoveOps = [RegOp.DeleteValue(DgKey, "EnableVirtualizationBasedSecurity")],
                DetectOps = [RegOp.CheckDword(DgKey, "EnableVirtualizationBasedSecurity", 1)],
            },
            new TweakDef
            {
                Id = "devguard-require-secure-boot-dma",
                Label = "Require Secure Boot and DMA Protection for VBS",
                Category = "Device & Hardware Policy",
                Description = "Requires Secure Boot and hardware DMA protection as platform security features for VBS to run.",
                Tags = ["vbs", "device-guard", "secure-boot", "dma", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 3,
                ImpactNote = "Value 3 = Secure Boot + DMA; requires appropriate firmware/hardware; VBS disabled without both features.",
                ApplyOps = [RegOp.SetDword(DgKey, "RequirePlatformSecurityFeatures", 3)],
                RemoveOps = [RegOp.DeleteValue(DgKey, "RequirePlatformSecurityFeatures")],
                DetectOps = [RegOp.CheckDword(DgKey, "RequirePlatformSecurityFeatures", 3)],
            },
            new TweakDef
            {
                Id = "devguard-enable-hvci",
                Label = "Enable Hypervisor-Enforced Code Integrity (HVCI)",
                Category = "Device & Hardware Policy",
                Description = "Enables Memory Integrity (HVCI) which uses the hypervisor to validate kernel code before execution.",
                Tags = ["hvci", "memory-integrity", "device-guard", "kernel", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 3,
                ImpactNote = "Value 2 = on without UEFI lock; blocks unsigned kernel drivers; may cause incompatibilities with legacy drivers.",
                ApplyOps = [RegOp.SetDword(DgKey, "HypervisorEnforcedCodeIntegrity", 2)],
                RemoveOps = [RegOp.DeleteValue(DgKey, "HypervisorEnforcedCodeIntegrity")],
                DetectOps = [RegOp.CheckDword(DgKey, "HypervisorEnforcedCodeIntegrity", 2)],
            },
            new TweakDef
            {
                Id = "devguard-require-uefi-mat",
                Label = "Require UEFI Memory Attributes Table for HVCI",
                Category = "Device & Hardware Policy",
                Description = "Requires the firmware to expose a UEFI Memory Attributes Table, enabling stricter HVCI enforcement.",
                Tags = ["hvci", "uefi", "mat", "device-guard", "firmware", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "Ensures UEFI properly marks regions; old firmware without UEFI MAT will fail HVCI initialisation.",
                ApplyOps = [RegOp.SetDword(DgKey, "HVCIMATRequired", 1)],
                RemoveOps = [RegOp.DeleteValue(DgKey, "HVCIMATRequired")],
                DetectOps = [RegOp.CheckDword(DgKey, "HVCIMATRequired", 1)],
            },
            new TweakDef
            {
                Id = "devguard-enable-credential-guard",
                Label = "Enable Credential Guard",
                Category = "Device & Hardware Policy",
                Description = "Isolates NTLM hashes and Kerberos tickets inside a VBS-protected virtual machine to prevent Pass-the-Hash attacks.",
                Tags = ["credential-guard", "device-guard", "vbs", "lsa", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 3,
                ImpactNote = "Value 2 = enabled without UEFI lock (removable); requires VBS; breaks smart-card-only environments in some configs.",
                ApplyOps = [RegOp.SetDword(DgKey, "LsaCfgFlags", 2)],
                RemoveOps = [RegOp.DeleteValue(DgKey, "LsaCfgFlags")],
                DetectOps = [RegOp.CheckDword(DgKey, "LsaCfgFlags", 2)],
            },
            new TweakDef
            {
                Id = "devguard-enable-system-guard",
                Label = "Enable System Guard Secure Launch",
                Category = "Device & Hardware Policy",
                Description = "Enables System Guard Secure Launch to verify platform integrity at boot using Dynamic Root of Trust (DRTM).",
                Tags = ["system-guard", "drtm", "secure-launch", "device-guard", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Requires Intel TXT or AMD SKINIT; boot verified via secure measurement; no effect on unsupported hardware.",
                ApplyOps = [RegOp.SetDword(DgKey, "ConfigureSystemGuardLaunch", 1)],
                RemoveOps = [RegOp.DeleteValue(DgKey, "ConfigureSystemGuardLaunch")],
                DetectOps = [RegOp.CheckDword(DgKey, "ConfigureSystemGuardLaunch", 1)],
            },
            new TweakDef
            {
                Id = "devguard-enable-kernel-shadow-stack",
                Label = "Enable Kernel Mode Hardware-Enforced Stack Protection",
                Category = "Device & Hardware Policy",
                Description = "Activates Hardware-Enforced Call Stack Protection (CET Shadow Stack) for kernel mode to resist ROP attacks.",
                Tags = ["cet", "shadow-stack", "device-guard", "kernel", "security", "rop"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "Requires Intel CET (Tiger Lake+) or AMD equivalent; blocks ROP/JOP exploits in kernel; may conflict with old drivers.",
                ApplyOps = [RegOp.SetDword(DgKey, "ConfigureKernelShadowStacksLaunchControl", 1)],
                RemoveOps = [RegOp.DeleteValue(DgKey, "ConfigureKernelShadowStacksLaunchControl")],
                DetectOps = [RegOp.CheckDword(DgKey, "ConfigureKernelShadowStacksLaunchControl", 1)],
            },
            new TweakDef
            {
                Id = "devguard-enable-hvci-audit-mode",
                Label = "Disable HVCI Audit Mode (Enforce Mode)",
                Category = "Device & Hardware Policy",
                Description = "Ensures HVCI operates in enforcement mode rather than audit-only mode for active code integrity protection.",
                Tags = ["hvci", "audit-mode", "device-guard", "enforce", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "Audit mode only logs violations; enforce mode blocks them. Disabling audit ensures active protection.",
                ApplyOps = [RegOp.SetDword(DgKey, "HypervisorEnforcedCodeIntegrityAuditModeEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(DgKey, "HypervisorEnforcedCodeIntegrityAuditModeEnabled")],
                DetectOps = [RegOp.CheckDword(DgKey, "HypervisorEnforcedCodeIntegrityAuditModeEnabled", 0)],
            },
            new TweakDef
            {
                Id = "devguard-block-unsigned-drivers",
                Label = "Block Unsigned Kernel Drivers via Policy",
                Category = "Device & Hardware Policy",
                Description = "Prevents loading of unsigned kernel-mode drivers, supplementing HVCI at the policy layer.",
                Tags = ["device-guard", "drivers", "signing", "kernel", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "Blocks unsigned drivers; WHQL or Microsoft signing required; may break niche hardware with unsigned drivers.",
                ApplyOps = [RegOp.SetDword(DgKey, "RequireDriverSignature", 1)],
                RemoveOps = [RegOp.DeleteValue(DgKey, "RequireDriverSignature")],
                DetectOps = [RegOp.CheckDword(DgKey, "RequireDriverSignature", 1)],
            },
            new TweakDef
            {
                Id = "devguard-audit-device-guard-status",
                Label = "Enable Device Guard Status Auditing",
                Category = "Device & Hardware Policy",
                Description = "Logs Device Guard and Credential Guard startup status to the event log for compliance monitoring.",
                Tags = ["device-guard", "audit", "logging", "compliance", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Writes Device Guard status events at boot; purely informational, no security side effects.",
                ApplyOps = [RegOp.SetDword(DgKey, "AuditDeviceGuardStatus", 1)],
                RemoveOps = [RegOp.DeleteValue(DgKey, "AuditDeviceGuardStatus")],
                DetectOps = [RegOp.CheckDword(DgKey, "AuditDeviceGuardStatus", 1)],
            },
        ];

    }

    // ── DeviceGuardVbs ──
    private static class _DeviceGuardVbs
    {
        private const string DeviceGuard = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard";

        private const string Scenarios = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard\Scenarios";

        private const string HvciScenario = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard\Scenarios\HypervisorEnforcedCodeIntegrity";

        private const string CredentialGuard = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa";

        private const string CodeIntegrity = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CI\Config";

        private const string DeviceGuardPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceGuard";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "vbs-enable-hvci",
                Label = "Enable Hypervisor-Protected Code Integrity (HVCI)",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                Tags = ["vbs", "hvci", "code integrity", "security", "virtualization"],
                Description =
                    "Enables HVCI (Memory Integrity) which uses the Hyper-V hypervisor to "
                    + "verify kernel-mode code signatures at runtime. Prevents kernel exploits "
                    + "and driver code injection. Requires a reboot. May impact performance by 5–15% "
                    + "on systems without MBEC hardware support.",
                ApplyOps = [RegOp.SetDword(HvciScenario, "Enabled", 1)],
                RemoveOps = [RegOp.SetDword(HvciScenario, "Enabled", 0)],
                DetectOps = [RegOp.CheckDword(HvciScenario, "Enabled", 1)],
            },
            new TweakDef
            {
                Id = "vbs-enable-credential-guard",
                Label = "Enable Windows Defender Credential Guard",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                Tags = ["vbs", "credential guard", "pass the hash", "security"],
                Description =
                    "Enables Credential Guard, which runs the Windows NTLM and Kerberos "
                    + "authentication subsystems in a VBS-isolated container. Prevents "
                    + "pass-the-hash and pass-the-ticket credential theft attacks. "
                    + "LsaIso isolation. Requires reboot.",
                ApplyOps = [RegOp.SetDword(CredentialGuard, "LsaCfgFlags", 1)],
                RemoveOps = [RegOp.SetDword(CredentialGuard, "LsaCfgFlags", 0)],
                DetectOps = [RegOp.CheckDword(CredentialGuard, "LsaCfgFlags", 1)],
            },
            new TweakDef
            {
                Id = "vbs-enable-vbs-platform",
                Label = "Enable Virtualization-Based Security Platform",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                Tags = ["vbs", "virtualization", "security", "hypervisor"],
                Description =
                    "Enables the VBS (Virtualization-Based Security) platform in Device Guard. "
                    + "EnableVirtualizationBasedSecurity=1. Prerequisites: Secure Boot, "
                    + "UEFI firmware, hardware virtualization (Intel VT-x / AMD-V). Requires reboot.",
                ApplyOps = [RegOp.SetDword(DeviceGuard, "EnableVirtualizationBasedSecurity", 1)],
                RemoveOps = [RegOp.SetDword(DeviceGuard, "EnableVirtualizationBasedSecurity", 0)],
                DetectOps = [RegOp.CheckDword(DeviceGuard, "EnableVirtualizationBasedSecurity", 1)],
            },
            new TweakDef
            {
                Id = "vbs-require-secure-boot-dma",
                Label = "Require Secure Boot + DMA Protection for VBS",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                Tags = ["vbs", "secure boot", "dma", "protection", "policy"],
                Description =
                    "Sets RequirePlatformSecurityFeatures=3 (require both Secure Boot and "
                    + "DMA protection). Prevents VBS from running on systems that lack "
                    + "IOMMU/VT-d DMA protection, ensuring the highest security level.",
                ApplyOps = [RegOp.SetDword(DeviceGuard, "RequirePlatformSecurityFeatures", 3)],
                RemoveOps = [RegOp.SetDword(DeviceGuard, "RequirePlatformSecurityFeatures", 1)],
                DetectOps = [RegOp.CheckDword(DeviceGuard, "RequirePlatformSecurityFeatures", 3)],
            },
            new TweakDef
            {
                Id = "vbs-enable-policy-device-guard",
                Label = "Enable Device Guard Policy via Group Policy",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                Tags = ["vbs", "device guard", "policy", "gpo"],
                Description =
                    "Enables Device Guard and VBS configuration via the machine-wide "
                    + "Group Policy path. Enables both VBS and HVCI through a single "
                    + "policy flag set. Reboot required for changes to take effect.",
                ApplyOps = [RegOp.SetDword(DeviceGuardPolicy, "EnableVirtualizationBasedSecurity", 1)],
                RemoveOps = [RegOp.DeleteValue(DeviceGuardPolicy, "EnableVirtualizationBasedSecurity")],
                DetectOps = [RegOp.CheckDword(DeviceGuardPolicy, "EnableVirtualizationBasedSecurity", 1)],
            },
            new TweakDef
            {
                Id = "vbs-enable-config-ci-policy",
                Label = "Enable Configurable Code Integrity (WDAC Boot Policy)",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 3,
                Tags = ["vbs", "wdac", "code integrity", "kernel", "policy"],
                Description =
                    "Sets the CodeIntegrity configurable policy option. When Enabled=1, "
                    + "the Windows Defender Application Control boot policy is loaded. "
                    + "WARNING: requires a valid WDAC policy file to be present, or the "
                    + "system may fail to boot drivers not covered by the policy.",
                ApplyOps = [RegOp.SetDword(CodeIntegrity, "Enabled", 1)],
                RemoveOps = [RegOp.SetDword(CodeIntegrity, "Enabled", 0)],
                DetectOps = [RegOp.CheckDword(CodeIntegrity, "Enabled", 1)],
            },
            new TweakDef
            {
                Id = "vbs-disable-test-signing",
                Label = "Disable Test Signing Mode (Production Security)",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Tags = ["vbs", "test signing", "driver", "security", "kernel"],
                Description =
                    "Ensures TestSigning=0, disabling Windows test-signing mode. Test signing "
                    + "is sometimes left enabled after driver development. Leaving it on "
                    + "allows unsigned kernel drivers, weakening system integrity.",
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecureBoot\State", "UEFISecureBootEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecureBoot\State", "UEFISecureBootEnabled")],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecureBoot\State", "UEFISecureBootEnabled", 1)],
            },
            new TweakDef
            {
                Id = "vbs-enable-kernel-shadow-stacks",
                Label = "Enable Kernel Shadow Stacks (Control Flow Guard Enforcement)",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Tags = ["vbs", "shadow stack", "cfg", "control flow guard", "exploit"],
                Description =
                    "Enables kernel shadow stacks (also called Kernel CFG Enforcement) which "
                    + "uses hardware CET (Control-flow Enforcement Technology) to harden "
                    + "return address integrity in kernel mode. Intel Tiger Lake and later. "
                    + "KernelShadowStacksEnabled=1.",
                ApplyOps = [RegOp.SetDword(HvciScenario, "WasEnabledBy", 1)],
                RemoveOps = [RegOp.DeleteValue(HvciScenario, "WasEnabledBy")],
                DetectOps = [RegOp.CheckDword(HvciScenario, "WasEnabledBy", 1)],
            },
            new TweakDef
            {
                Id = "vbs-lock-hvci",
                Label = "Lock HVCI to Prevent Disable Without Reboot",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Tags = ["vbs", "hvci", "lock", "tamper protection"],
                Description =
                    "Sets HVCI Locked=1, preventing the policy from being disabled at runtime "
                    + "without a reboot. Once locked, changes to Memory Integrity require "
                    + "a system restart to take effect, protecting against live tampering.",
                ApplyOps = [RegOp.SetDword(HvciScenario, "Locked", 1)],
                RemoveOps = [RegOp.SetDword(HvciScenario, "Locked", 0)],
                DetectOps = [RegOp.CheckDword(HvciScenario, "Locked", 1)],
            },
            new TweakDef
            {
                Id = "vbs-disable-lsa-protection-audit-mode",
                Label = "Disable LSA Protected Process Audit Mode",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["vbs", "lsa", "protected process", "audit"],
                Description =
                    "Disables LSA Protected Process audit mode (RunAsPPL audit mode = 0). "
                    + "Audit mode logs what would be blocked if LSA Protection were enabled "
                    + "without actually enabling protection. Disable once LSA Protection "
                    + "is confirmed stable on the system.",
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RunAsPPLBoot", 2)],
                RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RunAsPPLBoot")],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RunAsPPLBoot", 2)],
            },
        ];

    }

    // ── DeviceHealthCheckPolicy ──
    private static class _DeviceHealthCheckPolicy
    {
        private const string HcKey =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceHealthAttestation";

        private const string TpmKey =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\TPM";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "devhc-enable-tpm-health-check",
                    Label = "Device Health: Enable TPM Health State Evaluation",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Sets EnableTpmHealthCheck=1 in DeviceHealthAttestation policy. Enables evaluation of TPM health state as part of the device health check. The TPM health check evaluates whether the TPM is enabled, activated, owned, and in a known-good state. TPMs can enter a reduced-functionality mode (e.g., after detecting too many failed PIN attempts or a firmware update that changes the platform configuration registers). A TPM in degraded state cannot attest the boot chain, which can silently cause attestation failures unless the health check actively reports the degraded status.",
                    Tags = ["tpm", "health-check", "attestation", "degraded-state", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "TPM health is evaluated on every DHA cycle. Degraded or disabled TPM is reported as a health issue. Enables IT to detect TPM lockout or firmware-changed PCR states before they cause silent attestation failures.",
                    ApplyOps = [RegOp.SetDword(HcKey, "EnableTpmHealthCheck", 1)],
                    RemoveOps = [RegOp.DeleteValue(HcKey, "EnableTpmHealthCheck")],
                    DetectOps = [RegOp.CheckDword(HcKey, "EnableTpmHealthCheck", 1)],
                },
                new TweakDef
                {
                    Id = "devhc-require-elam-driver-for-health",
                    Label = "Device Health: Require ELAM Driver Active for Healthy State",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Sets RequireElamDriverForHealth=1 in DeviceHealthAttestation policy. Reports the device as unhealthy if an Early Launch Anti-Malware (ELAM) driver is not loaded and active at boot. ELAM drivers are loaded before all other non-Microsoft drivers, giving them the ability to evaluate and classify boot drivers as trusted, untrusted, or unknown before they are allowed to initialize. Without an active ELAM driver, the device's pre-OS environment cannot be assessed for rootkits or boot drivers installed by malware. Windows Defender is an ELAM-registered product and satisfies this requirement.",
                    Tags = ["elam", "health", "boot-security", "early-launch", "malware-prevention"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Devices without an active ELAM driver are reported as unhealthy by DHA. Windows Defender satisfies this requirement by default. Third-party ELAM-registered AV products also satisfy it. Devices with all AV disabled will fail this check.",
                    ApplyOps = [RegOp.SetDword(HcKey, "RequireElamDriverForHealth", 1)],
                    RemoveOps = [RegOp.DeleteValue(HcKey, "RequireElamDriverForHealth")],
                    DetectOps = [RegOp.CheckDword(HcKey, "RequireElamDriverForHealth", 1)],
                },
                new TweakDef
                {
                    Id = "devhc-evaluate-secure-boot-measurement",
                    Label = "Device Health: Evaluate Secure Boot PCR Measurement Consistency",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Sets EvaluateSecureBootMeasurement=1 in DeviceHealthAttestation policy. Enables DHA to evaluate the consistency of Secure Boot Platform Configuration Register (PCR) measurements. TPM PCR values record hashes of every component in the boot chain. If the PCR values in the most recent health certificate differ from the baseline (e.g., a firmware update changed a boot component hash), the attestation service can detect this deviation and flag the device. This catches scenarios where a firmware update inadvertently introduced an unsigned component or where a bootkit altered a measured value.",
                    Tags = ["secure-boot", "pcr", "measurement", "attestation", "boot-chain"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Secure Boot PCR measurements are included in DHA health certificates. Changes to PCR values (firmware update, boot component change) are detected. Legitimate firmware updates may transiently mark the device as unhealthy until the DHA baseline is updated.",
                    ApplyOps = [RegOp.SetDword(HcKey, "EvaluateSecureBootMeasurement", 1)],
                    RemoveOps = [RegOp.DeleteValue(HcKey, "EvaluateSecureBootMeasurement")],
                    DetectOps = [RegOp.CheckDword(HcKey, "EvaluateSecureBootMeasurement", 1)],
                },
                new TweakDef
                {
                    Id = "devhc-set-health-report-retention-30days",
                    Label = "Device Health: Retain Health Reports for 30 Days",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Sets HealthReportRetentionDays=30 in DeviceHealthAttestation policy. Sets the number of days that device health reports are retained locally before being purged. Retaining health reports for 30 days provides a rolling audit of the device's health state history. This is useful for post-incident forensics: if a device was compromised, the health report history can show the exact point at which the TPM measurements changed, when Secure Boot was disabled, or when the ELAM driver was removed — correlating health state changes with suspicious events in the device's event log.",
                    Tags = ["health-report", "retention", "forensics", "audit", "30-days"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Health report data is retained for 30 days locally. Provides 30 days of health state history for forensic investigation. Small disk footprint — health reports are compact JSON structures, typically a few KB each.",
                    ApplyOps = [RegOp.SetDword(HcKey, "HealthReportRetentionDays", 30)],
                    RemoveOps = [RegOp.DeleteValue(HcKey, "HealthReportRetentionDays")],
                    DetectOps = [RegOp.CheckDword(HcKey, "HealthReportRetentionDays", 30)],
                },
                new TweakDef
                {
                    Id = "devhc-disable-health-check-bypass",
                    Label = "Device Health: Disable Health Check Bypass for Non-Compliant State",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Sets DisableHealthCheckBypass=1 in DeviceHealthAttestation policy. Prevents clients (including local administrators) from bypassing or suppressing the device health check. Without this policy, a sophisticated user or malware with admin privileges can modify the health state cache or suppress health certificate requests, causing the device to appear healthy to conditional access systems while actually being compromised. Disabling the bypass ensures that the DHA client cannot be locally tampered with to present a false healthy state.",
                    Tags = ["health-check", "bypass-prevention", "anti-tampering", "admin-restriction", "integrity"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote = "Health check processes cannot be bypassed or suppressed by local admins. Prevents malware or sophisticated users from spoofing a healthy state to conditional access systems. May complicate debugging of attestation issues in development environments.",
                    ApplyOps = [RegOp.SetDword(HcKey, "DisableHealthCheckBypass", 1)],
                    RemoveOps = [RegOp.DeleteValue(HcKey, "DisableHealthCheckBypass")],
                    DetectOps = [RegOp.CheckDword(HcKey, "DisableHealthCheckBypass", 1)],
                },
                new TweakDef
                {
                    Id = "devhc-enable-health-check-auto-remediation",
                    Label = "Device Health: Enable Automatic Remediation for Known Health Issues",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Sets EnableHealthAutoRemediation=1 in DeviceHealthAttestation policy. Enables the Device Health agent to attempt automatic remediation for known, non-critical health issues. Remediable issues include re-enabling Windows Defender real-time protection that was automatically disabled by a third-party AV (after that AV was uninstalled), re-enrolling the TPM endorsement key if the certificate expired, or restarting stalled health service processes. Automatic remediation reduces helpdesk tickets for transient compliance failures caused by installation or configuration drift.",
                    Tags = ["health", "auto-remediation", "defender", "tpm", "service-restart"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "The health agent automatically resolves known fixable issues (re-enables AV, restarts health services, re-provisions TPM EK). Only remediates known, low-risk issues — it will never force-enable BitLocker or change user-configured settings. Review the list of supported remediations for your OS build.",
                    ApplyOps = [RegOp.SetDword(HcKey, "EnableHealthAutoRemediation", 1)],
                    RemoveOps = [RegOp.DeleteValue(HcKey, "EnableHealthAutoRemediation")],
                    DetectOps = [RegOp.CheckDword(HcKey, "EnableHealthAutoRemediation", 1)],
                },
                new TweakDef
                {
                    Id = "devhc-enable-tpm-endorsement-key-validation",
                    Label = "Device Health: Enable TPM Endorsement Key Validation",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Sets ValidateTpmEndorsementKey=1 in TPM policy. Enables validation that the TPM's Endorsement Key (EK) certificate is in a known-valid certificate chain rooted at a trusted TPM manufacturer CA. The EK uniquely identifies the physical TPM chip. If EK validation is disabled or skipped, software-based fake TPM implementations (used in virtual machines without vTPM, or malicious virtual TPM drivers) can pass attestation checks. EK validation ensures the attestation chain is anchored to a real hardware chip with a manufacturer-issued certificate.",
                    Tags = ["tpm", "endorsement-key", "ek-validation", "hardware-anchor", "attestation"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote = "TPM endorsement key certificates are validated against the manufacturer CA chain. VMs with software vTPM (Hyper-V vTPM, VMware vTPM) have EK certificates signed by Microsoft or the platform vendor and will pass if those CAs are trusted. Non-certified TPMs in custom hardware may fail.",
                    ApplyOps = [RegOp.SetDword(TpmKey, "ValidateTpmEndorsementKey", 1)],
                    RemoveOps = [RegOp.DeleteValue(TpmKey, "ValidateTpmEndorsementKey")],
                    DetectOps = [RegOp.CheckDword(TpmKey, "ValidateTpmEndorsementKey", 1)],
                },
                new TweakDef
                {
                    Id = "devhc-require-tpm-version-20",
                    Label = "Device Health: Require TPM 2.0 for Health Attestation",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Sets RequireTpm20ForHealthAttestation=1 in TPM policy. Marks devices as unable to provide health attestation if they only have a TPM 1.2 chip (as opposed to a TPM 2.0). TPM 1.2 supports SHA-1 algorithm measurement banks. TPM 2.0 adds SHA-256 banks, algorithm agility, and enhanced authorization structures. Modern DHA services require TPM 2.0's enhanced capabilities for accurate, tamper-resistant attestation. TPM 1.2 attestation can be spoofed more easily and lacks support for Credential Guard, Device Guard, and Virtualization-Based Security measurements.",
                    Tags = ["tpm", "tpm-20", "attestation", "sha256", "vbs"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Devices with only TPM 1.2 cannot provide health attestation and are treated as unhealthy. Hardware manufactured before 2016 may only have TPM 1.2. Devices with no TPM are already unable to attest. Review device fleet hardware compatibility before enforcing.",
                    ApplyOps = [RegOp.SetDword(TpmKey, "RequireTpm20ForHealthAttestation", 1)],
                    RemoveOps = [RegOp.DeleteValue(TpmKey, "RequireTpm20ForHealthAttestation")],
                    DetectOps = [RegOp.CheckDword(TpmKey, "RequireTpm20ForHealthAttestation", 1)],
                },
                new TweakDef
                {
                    Id = "devhc-enable-code-integrity-measurement",
                    Label = "Device Health: Enable Code Integrity State in Health Reports",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Sets IncludeCodeIntegrityInReport=1 in DeviceHealthAttestation policy. Includes Windows Code Integrity (CI) enforcement state in the DHA health certificate. Code Integrity state records whether Windows Defender Application Control (WDAC) or Device Guard is active, whether CI is in audit vs. enforcement mode, and whether User-Mode Code Integrity (UMCI) is enabled in addition to HVCI (Hypervisor-Protected Code Integrity). Including CI state in the attestation report allows conditional access systems to require not just that the device is healthy but that it is actively enforcing application whitelisting.",
                    Tags = ["code-integrity", "wdac", "device-guard", "hvci", "attestation"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Code Integrity (WDAC/HVCI) state is included in the DHA health certificate. Conditional access can now require that a device have CI enforcement mode active. Devices in CI audit-only mode can be flagged as less secure than those in enforcement mode.",
                    ApplyOps = [RegOp.SetDword(HcKey, "IncludeCodeIntegrityInReport", 1)],
                    RemoveOps = [RegOp.DeleteValue(HcKey, "IncludeCodeIntegrityInReport")],
                    DetectOps = [RegOp.CheckDword(HcKey, "IncludeCodeIntegrityInReport", 1)],
                },
                new TweakDef
                {
                    Id = "devhc-enable-vbs-state-measurement",
                    Label = "Device Health: Include VBS/Credential Guard State in Health Reports",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Sets IncludeVbsStateInReport=1 in DeviceHealthAttestation policy. Includes Virtualization-Based Security (VBS) and Credential Guard state in the DHA health certificate. VBS isolates critical OS components (LSA, UEFI variable writes) inside a secure virtual machine backed by the CPU hypervisor, making credential theft attacks (Pass-the-Hash, Pass-the-Ticket) significantly harder. Including VBS state in attestation reports allows conditional access to enforce that only VBS-enabled devices handle sensitive workloads — for example, requiring VBS for devices that access privileged admin consoles.",
                    Tags = ["vbs", "credential-guard", "hypervisor", "attestation", "lsa-protection"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "VBS and Credential Guard state is included in DHA health certificates. Conditional access can require VBS/Credential Guard for high-privilege resource access. Devices without hardware VBS support (no hardware-enforced DEP, SLAT, or IOMMU) cannot satisfy this requirement.",
                    ApplyOps = [RegOp.SetDword(HcKey, "IncludeVbsStateInReport", 1)],
                    RemoveOps = [RegOp.DeleteValue(HcKey, "IncludeVbsStateInReport")],
                    DetectOps = [RegOp.CheckDword(HcKey, "IncludeVbsStateInReport", 1)],
                },
            ];

    }

    // ── DeviceInstallPolicies ──
    private static class _DeviceInstallPolicies
    {
        private const string Restrictions = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceInstall\Restrictions";

        private const string Settings = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceInstall\Settings";

        private const string DriverSearching = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DriverSearching";

        private const string DeviceMetadata = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Device Metadata";

        private const string DeviceInstaller = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Device Installer";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "dinst-deny-removable-install",
                Label = "Device Install Policy: Deny Installation of Removable Devices",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                Tags = ["device-install", "removable", "policy", "security", "dlp"],
                Description =
                    "Sets DenyRemovableDevices=1 in the DeviceInstall Restrictions policy. "
                    + "Prevents Windows from installing any device driver for a removable device class. "
                    + "Blocks USB storage drives, external HDDs, SD card readers, and other removable media "
                    + "from being added to the system as new devices.",
                SideEffects =
                    "Prevents installation of new USB storage and removable media devices. Existing already-installed devices continue to work.",
                ApplyOps = [RegOp.SetDword(Restrictions, "DenyRemovableDevices", 1)],
                RemoveOps = [RegOp.DeleteValue(Restrictions, "DenyRemovableDevices")],
                DetectOps = [RegOp.CheckDword(Restrictions, "DenyRemovableDevices", 1)],
            },
            new TweakDef
            {
                Id = "dinst-enable-device-id-block",
                Label = "Device Install Policy: Enable Hardware Device ID Restriction List",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Tags = ["device-install", "device-id", "block-list", "policy"],
                Description =
                    "Sets DenyDeviceIDs=1 in the DeviceInstall Restrictions policy. "
                    + "Activates the hardware device ID restriction list, allowing administrators to block "
                    + "specific devices by their hardware ID strings (e.g., 'USB\\VID_XXXX&PID_XXXX'). "
                    + "This flag enables the list; devices to block are configured separately via Group Policy.",
                ApplyOps = [RegOp.SetDword(Restrictions, "DenyDeviceIDs", 1)],
                RemoveOps = [RegOp.DeleteValue(Restrictions, "DenyDeviceIDs")],
                DetectOps = [RegOp.CheckDword(Restrictions, "DenyDeviceIDs", 1)],
            },
            new TweakDef
            {
                Id = "dinst-enable-class-block",
                Label = "Device Install Policy: Enable Setup Class GUID Restriction List",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Tags = ["device-install", "class-guid", "block-list", "policy"],
                Description =
                    "Sets DenyDeviceClasses=1 in the DeviceInstall Restrictions policy. "
                    + "Activates the setup class GUID restriction list, allowing administrators to block entire "
                    + "categories of devices (e.g., USB storage class {36FC9E60-C465-11CF-8056-444553540000}). "
                    + "This flag enables the list; class GUIDs to block are configured separately.",
                ApplyOps = [RegOp.SetDword(Restrictions, "DenyDeviceClasses", 1)],
                RemoveOps = [RegOp.DeleteValue(Restrictions, "DenyDeviceClasses")],
                DetectOps = [RegOp.CheckDword(Restrictions, "DenyDeviceClasses", 1)],
            },
            new TweakDef
            {
                Id = "dinst-admin-override-allowed",
                Label = "Device Install Policy: Allow Administrators to Override Device Restrictions",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Tags = ["device-install", "admin", "override", "policy"],
                Description =
                    "Sets AllowAdminInstall=1 in the DeviceInstall Restrictions policy. "
                    + "Allows members of the local Administrators group to install any device driver, "
                    + "bypassing the device ID and class GUID restriction lists. "
                    + "Enables admins to add exceptions without modifying Group Policy.",
                ApplyOps = [RegOp.SetDword(Restrictions, "AllowAdminInstall", 1)],
                RemoveOps = [RegOp.DeleteValue(Restrictions, "AllowAdminInstall")],
                DetectOps = [RegOp.CheckDword(Restrictions, "AllowAdminInstall", 1)],
            },
            new TweakDef
            {
                Id = "dinst-retroactive-id-block",
                Label = "Device Install Policy: Apply Device ID Blocks Retroactively",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Tags = ["device-install", "device-id", "retroactive", "policy"],
                Description =
                    "Sets DenyDeviceIDsRetroactive=1 in the DeviceInstall Restrictions policy. "
                    + "Extends the hardware device ID block list to affect devices that were already installed "
                    + "before the policy was applied. Without this, only new device installations are blocked. "
                    + "Retroactive blocking disables already-installed matched devices.",
                SideEffects = "Can disable currently working devices that match the device ID block list.",
                ApplyOps = [RegOp.SetDword(Restrictions, "DenyDeviceIDsRetroactive", 1)],
                RemoveOps = [RegOp.DeleteValue(Restrictions, "DenyDeviceIDsRetroactive")],
                DetectOps = [RegOp.CheckDword(Restrictions, "DenyDeviceIDsRetroactive", 1)],
            },
            new TweakDef
            {
                Id = "dinst-retroactive-class-block",
                Label = "Device Install Policy: Apply Class GUID Blocks Retroactively",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Tags = ["device-install", "class-guid", "retroactive", "policy"],
                Description =
                    "Sets DenyDeviceClassesRetroactive=1 in the DeviceInstall Restrictions policy. "
                    + "Extends the class GUID block list to also disable devices already installed before the "
                    + "policy was enforced. Combined with 'dinst-enable-class-block' to fully restrict an entire "
                    + "device class including previously installed instances.",
                SideEffects = "Can disable currently working devices in blocked setup classes.",
                ApplyOps = [RegOp.SetDword(Restrictions, "DenyDeviceClassesRetroactive", 1)],
                RemoveOps = [RegOp.DeleteValue(Restrictions, "DenyDeviceClassesRetroactive")],
                DetectOps = [RegOp.CheckDword(Restrictions, "DenyDeviceClassesRetroactive", 1)],
            },
            new TweakDef
            {
                Id = "dinst-disable-driver-web-search",
                Label = "Device Install Policy: Disable Driver Search via Windows Update",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Tags = ["device-install", "driver", "windows-update", "network", "policy"],
                Description =
                    "Sets SearchOrderConfig=0 in the DriverSearching policy key. "
                    + "Prevents Windows from searching Windows Update to locate and download device drivers. "
                    + "Requires administrators to pre-stage drivers or use WSUS/SCCM for driver distribution. "
                    + "Prevents unknown or unvetted drivers from being automatically pulled from the internet.",
                ApplyOps = [RegOp.SetDword(DriverSearching, "SearchOrderConfig", 0)],
                RemoveOps = [RegOp.DeleteValue(DriverSearching, "SearchOrderConfig")],
                DetectOps = [RegOp.CheckDword(DriverSearching, "SearchOrderConfig", 0)],
            },
            new TweakDef
            {
                Id = "dinst-disable-co-installers",
                Label = "Device Install Policy: Disable Third-Party Co-Installer Loading",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = false,
                ImpactScore = 3,
                SafetyRating = 3,
                Tags = ["device-install", "co-installer", "driver", "security"],
                SideEffects = "Some hardware drivers (e.g., printers, audio interfaces) require co-installers for full functionality.",
                Description =
                    "Sets DisableCoInstallers=1 in the Device Installer key. "
                    + "Blocks device co-installers — DLLs registered by driver packages to run additional code "
                    + "during device setup. Co-installers are a common attack vector: malicious or vulnerable "
                    + "co-installers can escalate privileges or install persistent malware during device installation.",
                ApplyOps = [RegOp.SetDword(DeviceInstaller, "DisableCoInstallers", 1)],
                RemoveOps = [RegOp.DeleteValue(DeviceInstaller, "DisableCoInstallers")],
                DetectOps = [RegOp.CheckDword(DeviceInstaller, "DisableCoInstallers", 1)],
            },
            new TweakDef
            {
                Id = "dinst-disable-wer-missing-driver",
                Label = "Device Install Policy: Disable WER Reports for Missing Drivers",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Tags = ["device-install", "wer", "error-reporting", "privacy"],
                Description =
                    "Sets DisableSendGenericDriverNotFoundToWER=1 in the DeviceInstall Settings policy. "
                    + "Prevents Windows Error Reporting from sending problem reports when a device driver is not "
                    + "found during Plug and Play device detection. "
                    + "Reduces unsolicited telemetry uploads to Microsoft servers.",
                ApplyOps = [RegOp.SetDword(Settings, "DisableSendGenericDriverNotFoundToWER", 1)],
                RemoveOps = [RegOp.DeleteValue(Settings, "DisableSendGenericDriverNotFoundToWER")],
                DetectOps = [RegOp.CheckDword(Settings, "DisableSendGenericDriverNotFoundToWER", 1)],
            },
            new TweakDef
            {
                Id = "dinst-block-device-metadata-internet",
                Label = "Device Install Policy: Block Device Metadata Downloads from the Internet",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Tags = ["device-install", "metadata", "privacy", "network", "policy"],
                Description =
                    "Sets PreventDeviceMetadataFromNetwork=1 in the Device Metadata policy key. "
                    + "Prevents Windows Device Stage from downloading device metadata (icons, descriptions, "
                    + "software links) from the Windows Metadata and Internet Services (WMIS) server. "
                    + "Stops unnecessary network connections to Microsoft servers triggered by new device insertion.",
                ApplyOps = [RegOp.SetDword(DeviceMetadata, "PreventDeviceMetadataFromNetwork", 1)],
                RemoveOps = [RegOp.DeleteValue(DeviceMetadata, "PreventDeviceMetadataFromNetwork")],
                DetectOps = [RegOp.CheckDword(DeviceMetadata, "PreventDeviceMetadataFromNetwork", 1)],
            },
        ];

    }

    // ── DeviceInstallPolicy ──
    private static class _DeviceInstallPolicy
    {
        private const string DiKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceInstall";
        private const string DiRestrictKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceInstall\Restrictions";

        public static IReadOnlyList<TweakDef> Data =>
        [
            new TweakDef
            {
                Id = "devinstall-deny-unspecified-devices",
                Label = "Deny Installation of Unlisted Device Classes",
                Category = "Device & Hardware Policy",
                Description = "Prevents Windows from installing devices whose class is not explicitly permitted by device installation policy.",
                Tags = ["device-install", "device-class", "restriction", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                ImpactNote = "Blocks any device not on an allow-list; aggressive setting — combine with device class allow-lists.",
                ApplyOps = [RegOp.SetDword(DiRestrictKey, "DenyUnspecified", 1)],
                RemoveOps = [RegOp.DeleteValue(DiRestrictKey, "DenyUnspecified")],
                DetectOps = [RegOp.CheckDword(DiRestrictKey, "DenyUnspecified", 1)],
            },
            new TweakDef
            {
                Id = "devinstall-deny-removable-devices",
                Label = "Deny Installation of Removable Storage Devices",
                Category = "Device & Hardware Policy",
                Description = "Blocks Windows from installing USB drives, SD cards, and other removable storage devices.",
                Tags = ["device-install", "removable-storage", "usb", "restriction", "dlp", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Key DLP control; blocks USB exfiltration. Removable storage that was already installed still works.",
                ApplyOps = [RegOp.SetDword(DiRestrictKey, "DenyRemovableDevices", 1)],
                RemoveOps = [RegOp.DeleteValue(DiRestrictKey, "DenyRemovableDevices")],
                DetectOps = [RegOp.CheckDword(DiRestrictKey, "DenyRemovableDevices", 1)],
            },
            new TweakDef
            {
                Id = "devinstall-no-admin-override",
                Label = "Prevent Admins from Overriding Device Installation Restrictions",
                Category = "Device & Hardware Policy",
                Description = "Removes the administrator privilege that normally allows bypassing device installation policy restrictions.",
                Tags = ["device-install", "admin", "restriction", "override", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Even local admins cannot install blocked device classes; requires GPO change to allow an exception.",
                ApplyOps = [RegOp.SetDword(DiRestrictKey, "AllowAdminInstall", 0)],
                RemoveOps = [RegOp.DeleteValue(DiRestrictKey, "AllowAdminInstall")],
                DetectOps = [RegOp.CheckDword(DiRestrictKey, "AllowAdminInstall", 0)],
            },
            new TweakDef
            {
                Id = "devinstall-enable-setup-logging",
                Label = "Enable Verbose Device Installation Event Logging",
                Category = "Device & Hardware Policy",
                Description = "Enables detailed event logging in the Windows device installation subsystem for auditing and diagnostics.",
                Tags = ["device-install", "logging", "audit", "events", "monitoring"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Enables verbose installation logs in the Windows Device Setup event channel; minimal performance impact.",
                ApplyOps = [RegOp.SetDword(DiKey, "EnableSetupSystemRestoreCheckpoints", 1)],
                RemoveOps = [RegOp.DeleteValue(DiKey, "EnableSetupSystemRestoreCheckpoints")],
                DetectOps = [RegOp.CheckDword(DiKey, "EnableSetupSystemRestoreCheckpoints", 1)],
            },
            new TweakDef
            {
                Id = "devinstall-disable-driver-search-online",
                Label = "Disable Online Driver Search During Device Install",
                Category = "Device & Hardware Policy",
                Description = "Prevents Windows from searching the Internet (Windows Update) for drivers during device installation.",
                Tags = ["device-install", "driver", "windows-update", "online", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Blocks unsigned or unvetted driver downloads from WU; IT manages and deploys approved drivers.",
                ApplyOps = [RegOp.SetDword(DiKey, "SearchOrderConfig", 1)],
                RemoveOps = [RegOp.DeleteValue(DiKey, "SearchOrderConfig")],
                DetectOps = [RegOp.CheckDword(DiKey, "SearchOrderConfig", 1)],
            },
            new TweakDef
            {
                Id = "devinstall-no-driver-store-from-wer",
                Label = "Disable WER-Triggered Driver Package Downloads",
                Category = "Device & Hardware Policy",
                Description = "Prevents Windows Error Reporting from triggering automatic driver package downloads from the Internet.",
                Tags = ["device-install", "wer", "driver", "download", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Closes a secondary driver download path triggered by crash events; driver management stays in IT control.",
                ApplyOps = [RegOp.SetDword(DiKey, "AllowUserPnP", 0)],
                RemoveOps = [RegOp.DeleteValue(DiKey, "AllowUserPnP")],
                DetectOps = [RegOp.CheckDword(DiKey, "AllowUserPnP", 0)],
            },
            new TweakDef
            {
                Id = "devinstall-block-legacy-ieee1394",
                Label = "Restrict IEEE 1394 (FireWire) Device Installation",
                Category = "Device & Hardware Policy",
                Description = "Blocks installation of IEEE 1394 (FireWire) bus controllers, which support DMA and can bypass OS memory protection.",
                Tags = ["device-install", "firewire", "ieee1394", "dma", "security", "hardware"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "FireWire DMA attacks allow direct memory access bypassing the OS; only impacts systems with legacy ports.",
                ApplyOps = [RegOp.SetDword(DiRestrictKey, "DenyDeviceIDs", 1)],
                RemoveOps = [RegOp.DeleteValue(DiRestrictKey, "DenyDeviceIDs")],
                DetectOps = [RegOp.CheckDword(DiRestrictKey, "DenyDeviceIDs", 1)],
            },
            new TweakDef
            {
                Id = "devinstall-create-restore-point-on-install",
                Label = "Create System Restore Point During Driver Installation",
                Category = "Device & Hardware Policy",
                Description = "Forces Windows to create a system restore point before installing any new device driver, enabling rollback.",
                Tags = ["device-install", "driver", "restore-point", "rollback", "safety"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Restore point created before each driver install; enables quick recovery from bad driver installations.",
                ApplyOps = [RegOp.SetDword(DiKey, "DisableSystemRestore", 0)],
                RemoveOps = [RegOp.DeleteValue(DiKey, "DisableSystemRestore")],
                DetectOps = [RegOp.CheckDword(DiKey, "DisableSystemRestore", 0)],
            },
            new TweakDef
            {
                Id = "devinstall-disable-drivers-from-cd",
                Label = "Disable Driver Installation from Optical Media",
                Category = "Device & Hardware Policy",
                Description = "Prevents Windows from using drivers stored on removable optical media (CD/DVD) during device installation.",
                Tags = ["device-install", "cd", "dvd", "optical", "driver", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Blocks CD/DVD as a driver source; relevant on systems with optical drives that accept physical media.",
                ApplyOps = [RegOp.SetDword(DiRestrictKey, "DenyRemovableDevicesRetroactive", 1)],
                RemoveOps = [RegOp.DeleteValue(DiRestrictKey, "DenyRemovableDevicesRetroactive")],
                DetectOps = [RegOp.CheckDword(DiRestrictKey, "DenyRemovableDevicesRetroactive", 1)],
            },
            new TweakDef
            {
                Id = "devinstall-notify-admin-on-block",
                Label = "Notify Admins When Device Installation Is Blocked",
                Category = "Device & Hardware Policy",
                Description = "Sends a notification to administrators when a device installation attempt is blocked by policy.",
                Tags = ["device-install", "notification", "admin", "audit", "monitoring"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Generates a Windows event log entry when device install is denied; helps with security monitoring.",
                ApplyOps = [RegOp.SetDword(DiRestrictKey, "AlertOnDeviceInstallation", 1)],
                RemoveOps = [RegOp.DeleteValue(DiRestrictKey, "AlertOnDeviceInstallation")],
                DetectOps = [RegOp.CheckDword(DiRestrictKey, "AlertOnDeviceInstallation", 1)],
            },
        ];

    }

    // ── DeviceLockGpoPolicy ──
    private static class _DeviceLockGpoPolicy
    {
        private const string PassportKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PassportForWork";
        private const string DesktopKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop";
        private const string SystemKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "devlockgpo-disable-windows-hello-business",
                Label = "Device Lock GPO: Disable Windows Hello for Business Enrollment",
                Category = "Device & Hardware Policy",
                Description = "Prevents users and devices from enrolling in Windows Hello for Business (WHfB), the enterprise PIN-based authentication system that replaces passwords with asymmetric-key credentials tied to the device TPM. In environments where smart cards or alternative MFA tokens are used instead of WHfB, disabling enrollment prevents credential proliferation.",
                Tags = ["windows hello", "pin", "mfa", "enrollment", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [PassportKey],
                ApplyOps = [RegOp.SetDword(PassportKey, "Enabled", 0)],
                RemoveOps = [RegOp.DeleteValue(PassportKey, "Enabled")],
                DetectOps = [RegOp.CheckDword(PassportKey, "Enabled", 0)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Disables Windows Hello for Business on the device; standard Windows Hello (local PIN) is unaffected.",
            },
            new TweakDef
            {
                Id = "devlockgpo-disable-hello-post-logon-provisioning",
                Label = "Device Lock GPO: Disable Windows Hello Post-Logon Provisioning Prompt",
                Category = "Device & Hardware Policy",
                Description = "Prevents Windows from launching the Windows Hello for Business provisioning wizard immediately after the first interactive logon. By default, Windows shows the WHfB setup screen right after AD/Azure AD join and logon. This policy suppresses that prompt so administrators can provision Windows Hello on a controlled schedule.",
                Tags = ["windows hello", "pin", "provisioning", "logon", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [PassportKey],
                ApplyOps = [RegOp.SetDword(PassportKey, "DisablePostLogonProvisioning", 1)],
                RemoveOps = [RegOp.DeleteValue(PassportKey, "DisablePostLogonProvisioning")],
                DetectOps = [RegOp.CheckDword(PassportKey, "DisablePostLogonProvisioning", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Suppresses the Windows Hello provisioning screen shown after first logon on new machines.",
            },
            new TweakDef
            {
                Id = "devlockgpo-disable-hello-pin-recovery",
                Label = "Device Lock GPO: Disable Windows Hello PIN Recovery Service",
                Category = "Device & Hardware Policy",
                Description = "Disables the cloud-based Windows Hello PIN recovery service that allows users to reset their device PIN via their Microsoft account or Azure AD credentials. The PIN recovery service sends encrypted PIN reset data to Microsoft cloud servers. In high-security environments where no cloud dependencies are allowed, this service should be disabled.",
                Tags = ["windows hello", "pin", "recovery", "cloud", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [PassportKey],
                ApplyOps = [RegOp.SetDword(PassportKey, "DisablePinRecovery", 1)],
                RemoveOps = [RegOp.DeleteValue(PassportKey, "DisablePinRecovery")],
                DetectOps = [RegOp.CheckDword(PassportKey, "DisablePinRecovery", 1)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Disables cloud PIN reset; users must reset via IT admin if they forget their PIN.",
            },
            new TweakDef
            {
                Id = "devlockgpo-require-tpm-for-hello",
                Label = "Device Lock GPO: Require TPM Chip for Windows Hello Credential Storage",
                Category = "Device & Hardware Policy",
                Description = "Requires that Windows Hello for Business private keys be stored in the device's Trusted Platform Module (TPM) hardware security chip, not in software-only storage. Without a TPM requirement, WHfB keys are stored in software, making them vulnerable to extraction from disk. TPM binding ensures that private keys never leave the secure chip.",
                Tags = ["windows hello", "tpm", "security", "key storage", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [PassportKey],
                ApplyOps = [RegOp.SetDword(PassportKey, "RequireSecurityDevice", 1)],
                RemoveOps = [RegOp.DeleteValue(PassportKey, "RequireSecurityDevice")],
                DetectOps = [RegOp.CheckDword(PassportKey, "RequireSecurityDevice", 1)],
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Enforces TPM-based key storage; devices without a TPM cannot use Windows Hello for Business.",
            },
            new TweakDef
            {
                Id = "devlockgpo-require-screensaver-password",
                Label = "Device Lock GPO: Require Password When Resuming from Screen Saver",
                Category = "Device & Hardware Policy",
                Description = "Forces Windows to require the user's password (or PIN) when resuming from a screen saver or after a period of inactivity. This is a foundational physical security control that prevents unauthorized access to unattended workstations. Without this policy, an unlocked workstation can be accessed by anyone who sits down at the keyboard.",
                Tags = ["screen saver", "lock", "password", "unattended", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [DesktopKey],
                ApplyOps = [RegOp.SetString(DesktopKey, "ScreenSaverIsSecure", "1")],
                RemoveOps = [RegOp.DeleteValue(DesktopKey, "ScreenSaverIsSecure")],
                DetectOps = [RegOp.CheckString(DesktopKey, "ScreenSaverIsSecure", "1")],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Prevents access to unattended workstations after screen saver activates.",
            },
            new TweakDef
            {
                Id = "devlockgpo-enable-screensaver",
                Label = "Device Lock GPO: Enable Screen Saver (Enforce Lock on Idle)",
                Category = "Device & Hardware Policy",
                Description = "Enables and enforces the screen saver policy on the workstation, ensuring that it activates after a configurable period of user inactivity. When combined with the ScreenSaverIsSecure policy, this guarantees all unattended workstations will automatically lock. Without this policy, users can disable the screen saver entirely, defeating the auto-lock mechanism.",
                Tags = ["screen saver", "enable", "idle", "lock", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [DesktopKey],
                ApplyOps = [RegOp.SetString(DesktopKey, "ScreenSaveActive", "1")],
                RemoveOps = [RegOp.DeleteValue(DesktopKey, "ScreenSaveActive")],
                DetectOps = [RegOp.CheckString(DesktopKey, "ScreenSaveActive", "1")],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Enforces screen saver activation; prevents users from disabling the auto-lock mechanism.",
            },
            new TweakDef
            {
                Id = "devlockgpo-set-screensaver-timeout-600",
                Label = "Device Lock GPO: Set Screen Saver Timeout to 10 Minutes (600 s)",
                Category = "Device & Hardware Policy",
                Description = "Sets the screen saver / auto-lock timeout to 600 seconds (10 minutes). Industry security frameworks (CIS, NIST SP 800-53, PCI DSS) recommend an idle timeout of 10–15 minutes for standard workstations. A 10-minute timeout balances security with productivity, locking unattended machines before a brief absence creates risk.",
                Tags = ["screen saver", "timeout", "idle", "lock", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [DesktopKey],
                ApplyOps = [RegOp.SetString(DesktopKey, "ScreenSaveTimeOut", "600")],
                RemoveOps = [RegOp.DeleteValue(DesktopKey, "ScreenSaveTimeOut")],
                DetectOps = [RegOp.CheckString(DesktopKey, "ScreenSaveTimeOut", "600")],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "10-minute idle lock; CIS Benchmark recommended value for standard workstations.",
            },
            new TweakDef
            {
                Id = "devlockgpo-disable-lock-screen-notifications",
                Label = "Device Lock GPO: Disable Notifications on Lock Screen",
                Category = "Device & Hardware Policy",
                Description = "Prevents Windows from displaying app notifications (toast notifications) on the lock screen. Lock-screen notifications can expose sensitive information to passersby — email previews, chat messages, calendar events — without requiring authentication. This policy disables all notification content from appearing while the screen is locked.",
                Tags = ["lock screen", "notifications", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SystemKey],
                ApplyOps = [RegOp.SetDword(SystemKey, "NoLockScreenNotificationsTitle", 1)],
                RemoveOps = [RegOp.DeleteValue(SystemKey, "NoLockScreenNotificationsTitle")],
                DetectOps = [RegOp.CheckDword(SystemKey, "NoLockScreenNotificationsTitle", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Hides notification content on the lock screen; prevents data exposure to unauthenticated viewers.",
            },
            new TweakDef
            {
                Id = "devlockgpo-disable-camera-on-lockscreen",
                Label = "Device Lock GPO: Disable Camera Access on Lock Screen",
                Category = "Device & Hardware Policy",
                Description = "Prevents cameras (webcams, built-in laptop cameras) from being activated while the workstation is locked. Some Windows Hello facial recognition implementations allow the camera to be used on the lock screen, but malicious code or physical manipulation could trigger unauthorized image capture. Disabling the camera on the lock screen closes this attack surface.",
                Tags = ["lock screen", "camera", "privacy", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SystemKey],
                ApplyOps = [RegOp.SetDword(SystemKey, "NoLockScreenCamera", 1)],
                RemoveOps = [RegOp.DeleteValue(SystemKey, "NoLockScreenCamera")],
                DetectOps = [RegOp.CheckDword(SystemKey, "NoLockScreenCamera", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Disables camera hardware access from the lock screen; Windows Hello face recognition still works at login.",
            },
            new TweakDef
            {
                Id = "devlockgpo-disable-ease-access-on-lockscreen",
                Label = "Device Lock GPO: Disable Ease of Access Menu on Lock Screen",
                Category = "Device & Hardware Policy",
                Description = "Removes the Ease of Access (accessibility) button from the Windows lock screen. The Ease of Access menu on the lock screen provides access to Narrator, Magnifier, and On-Screen Keyboard before authentication, which has historically been exploited to gain SYSTEM-level access through sticky-key and narrator command injection techniques on older Windows versions.",
                Tags = ["lock screen", "ease of access", "security", "accessibility", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SystemKey],
                ApplyOps = [RegOp.SetDword(SystemKey, "DisableLockScreenAppNotifications", 1)],
                RemoveOps = [RegOp.DeleteValue(SystemKey, "DisableLockScreenAppNotifications")],
                DetectOps = [RegOp.CheckDword(SystemKey, "DisableLockScreenAppNotifications", 1)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Removes app notifications from lock screen; does not affect standard accessibility tools at login.",
            },
        ];

    }

    // ── DeviceProvisioningPolicy ──
    private static class _DeviceProvisioningPolicy
    {
        private const string Oobe = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\OOBE";
        private const string HomeGrp = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HomeGroup";
        private const string WpjPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WorkplaceJoin";
        private const string CloudContent = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "devprov-disable-oobe-privacy",
                Label = "OOBE: Skip the privacy experience setup page",
                Category = "Device & Hardware Policy",
                Description =
                    "Sets DisablePrivacyExperience=1 in the OOBE policy key. Suppresses the Windows "
                    + "privacy settings page that appears during first sign-in after setup or a feature update.",
                Tags = ["oobe", "privacy", "setup", "policy", "provisioning"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Oobe, "DisablePrivacyExperience", 1)],
                RemoveOps = [RegOp.DeleteValue(Oobe, "DisablePrivacyExperience")],
                DetectOps = [RegOp.CheckDword(Oobe, "DisablePrivacyExperience", 1)],
            },
            new TweakDef
            {
                Id = "devprov-skip-machine-oobe",
                Label = "OOBE: Skip the machine out-of-box experience setup",
                Category = "Device & Hardware Policy",
                Description =
                    "Sets SkipMachineOOBE=1 in the OOBE policy key. Prevents the machine-level OOBE "
                    + "wizard from running, useful for pre-provisioned enterprise devices.",
                Tags = ["oobe", "setup", "provisioning", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Oobe, "SkipMachineOOBE", 1)],
                RemoveOps = [RegOp.DeleteValue(Oobe, "SkipMachineOOBE")],
                DetectOps = [RegOp.CheckDword(Oobe, "SkipMachineOOBE", 1)],
            },
            new TweakDef
            {
                Id = "devprov-skip-user-oobe",
                Label = "OOBE: Skip the user out-of-box experience setup",
                Category = "Device & Hardware Policy",
                Description =
                    "Sets SkipUserOOBE=1 in the OOBE policy key. Skips the per-user OOBE wizard that "
                    + "prompts for Cortana, account sign-in, and other optional setup steps.",
                Tags = ["oobe", "setup", "provisioning", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Oobe, "SkipUserOOBE", 1)],
                RemoveOps = [RegOp.DeleteValue(Oobe, "SkipUserOOBE")],
                DetectOps = [RegOp.CheckDword(Oobe, "SkipUserOOBE", 1)],
            },
            new TweakDef
            {
                Id = "devprov-no-connected-oobe",
                Label = "OOBE: Disable cloud-connected experience during OOBE",
                Category = "Device & Hardware Policy",
                Description =
                    "Sets DisableOOBEWithNetworkConnectivity=1 in the OOBE policy key. Prevents the "
                    + "OOBE wizard from triggering cloud-connected steps when network connectivity is detected.",
                Tags = ["oobe", "cloud", "setup", "provisioning", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Oobe, "DisableOOBEWithNetworkConnectivity", 1)],
                RemoveOps = [RegOp.DeleteValue(Oobe, "DisableOOBEWithNetworkConnectivity")],
                DetectOps = [RegOp.CheckDword(Oobe, "DisableOOBEWithNetworkConnectivity", 1)],
            },
            new TweakDef
            {
                Id = "devprov-disable-homegroup",
                Label = "HomeGroup: Prevent computers from joining a HomeGroup",
                Category = "Device & Hardware Policy",
                Description =
                    "Sets DisableHomeGroup=1 in the HomeGroup policy key. Prevents users from joining "
                    + "or creating HomeGroups. Recommended on domain-joined and managed devices.",
                Tags = ["homegroup", "sharing", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(HomeGrp, "DisableHomeGroup", 1)],
                RemoveOps = [RegOp.DeleteValue(HomeGrp, "DisableHomeGroup")],
                DetectOps = [RegOp.CheckDword(HomeGrp, "DisableHomeGroup", 1)],
            },
            new TweakDef
            {
                Id = "devprov-block-aad-workplace-join",
                Label = "Workplace Join: Block Azure AD Workplace Join",
                Category = "Device & Hardware Policy",
                Description =
                    "Sets BlockAADWorkplaceJoin=1 in the WorkplaceJoin policy key. Prevents users from "
                    + "adding a work or school account via the 'Access work or school' Settings page.",
                Tags = ["workplace-join", "aad", "mdm", "enrollment", "policy"],
                NeedsAdmin = true,
                CorpSafe = false,
                ApplyOps = [RegOp.SetDword(WpjPol, "BlockAADWorkplaceJoin", 1)],
                RemoveOps = [RegOp.DeleteValue(WpjPol, "BlockAADWorkplaceJoin")],
                DetectOps = [RegOp.CheckDword(WpjPol, "BlockAADWorkplaceJoin", 1)],
            },
            new TweakDef
            {
                Id = "devprov-disable-wpj-flyout",
                Label = "Workplace Join: Disable the 'Connect to work or school' flyout",
                Category = "Device & Hardware Policy",
                Description =
                    "Sets FlyoutDisabled=1 in the WorkplaceJoin policy key. Hides the Workplace Join "
                    + "notification flyout from the Action Center and Settings entry point.",
                Tags = ["workplace-join", "flyout", "notification", "policy"],
                NeedsAdmin = true,
                CorpSafe = false,
                ApplyOps = [RegOp.SetDword(WpjPol, "FlyoutDisabled", 1)],
                RemoveOps = [RegOp.DeleteValue(WpjPol, "FlyoutDisabled")],
                DetectOps = [RegOp.CheckDword(WpjPol, "FlyoutDisabled", 1)],
            },
            new TweakDef
            {
                Id = "devprov-disable-find-my-device",
                Label = "Cloud Content: Disable the Find My Device feature",
                Category = "Device & Hardware Policy",
                Description =
                    "Sets DisableFindMyDevice=1 in the CloudContent policy key. Prevents Windows from "
                    + "registering the device with Microsoft's Find My Device location tracking service.",
                Tags = ["find-my-device", "cloud", "location", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(CloudContent, "DisableFindMyDevice", 1)],
                RemoveOps = [RegOp.DeleteValue(CloudContent, "DisableFindMyDevice")],
                DetectOps = [RegOp.CheckDword(CloudContent, "DisableFindMyDevice", 1)],
            },
            new TweakDef
            {
                Id = "devprov-disable-windows-tips",
                Label = "Cloud Content: Disable Windows Tips and suggestions",
                Category = "Device & Hardware Policy",
                Description =
                    "Sets DisableSoftLanding=1 in the CloudContent policy key. Prevents Windows from "
                    + "showing 'tips', 'fun facts', and soft-landing welcome content to the user.",
                Tags = ["tips", "suggestions", "cloud", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(CloudContent, "DisableSoftLanding", 1)],
                RemoveOps = [RegOp.DeleteValue(CloudContent, "DisableSoftLanding")],
                DetectOps = [RegOp.CheckDword(CloudContent, "DisableSoftLanding", 1)],
            },
            new TweakDef
            {
                Id = "devprov-disable-tailored-experiences",
                Label = "Cloud Content: Disable tailored experiences with diagnostic data",
                Category = "Device & Hardware Policy",
                Description =
                    "Sets DisableTailoredExperiencesWithDiagnosticData=1. Prevents Microsoft from using "
                    + "diagnostic and usage telemetry to personalise tips, ads, and recommendations.",
                Tags = ["tailored-experiences", "telemetry", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(CloudContent, "DisableTailoredExperiencesWithDiagnosticData", 1)],
                RemoveOps = [RegOp.DeleteValue(CloudContent, "DisableTailoredExperiencesWithDiagnosticData")],
                DetectOps = [RegOp.CheckDword(CloudContent, "DisableTailoredExperiencesWithDiagnosticData", 1)],
            },
        ];

    }

    // ── DeviceRegistrationPolicy ──
    private static class _DeviceRegistrationPolicy
    {
        private const string Key =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceRegistration";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "devreg-disable-auto-device-registration",
                    Label = "Disable Automatic Azure AD Device Registration",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Prevents the device from automatically registering with Azure Active Directory / Entra ID during domain join or user sign-in. Gives IT full control over when and how devices are registered. Default: auto-register on domain join. Recommended: 1 when phased registration is required.",
                    Tags = ["device-registration", "azure-ad", "entra", "mdm", "enrollment", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Device does not automatically register with Azure AD/Entra on domain join; manual or scripted registration is required.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableAutoDeviceRegistration", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoDeviceRegistration")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableAutoDeviceRegistration", 1)],
                },
                new TweakDef
                {
                    Id = "devreg-require-tpm-for-registration",
                    Label = "Require TPM for Device Registration",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Mandates that a TPM 2.0 chip is present and functional before the device can complete Azure AD registration. Ensures only hardware-attested devices can enrol; blocks VMs and devices without TPM. Default: TPM not required. Recommended: 1 for Zero Trust deployments.",
                    Tags = ["device-registration", "tpm", "hardware-attestation", "zero-trust", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote = "Devices without TPM 2.0 cannot register with Azure AD; hardware attestation is mandatory.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireTpmForRegistration", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireTpmForRegistration")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireTpmForRegistration", 1)],
                },
                new TweakDef
                {
                    Id = "devreg-set-registration-retry-3",
                    Label = "Set Device Registration Retry Count to 3",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Limits the number of automatic re-registration attempts when initial Azure AD registration fails (e.g., due to network error) to 3 before stopping. Prevents persistent registration loops. Default: unlimited retries. Recommended: 3.",
                    Tags = ["device-registration", "retry", "enrollment", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Device stops attempting re-registration after 3 failures; reduces background registration loop network noise.",
                    ApplyOps = [RegOp.SetDword(Key, "MaxRegistrationRetries", 3)],
                    RemoveOps = [RegOp.DeleteValue(Key, "MaxRegistrationRetries")],
                    DetectOps = [RegOp.CheckDword(Key, "MaxRegistrationRetries", 3)],
                },
                new TweakDef
                {
                    Id = "devreg-block-personal-account-registration",
                    Label = "Block Personal MSA Device Registration",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Prevents users from registering the device with their personal Microsoft Account (MSA). Only corporate Azure AD / Entra accounts can register the device. Default: MSA registration allowed. Recommended: 1 on managed corporate endpoints.",
                    Tags = ["device-registration", "msa", "personal-account", "corporate", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Personal MSA device registration is blocked; only Entra ID / Azure AD corporate accounts can register.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockPersonalAccountDeviceRegistration", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockPersonalAccountDeviceRegistration")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockPersonalAccountDeviceRegistration", 1)],
                },
                new TweakDef
                {
                    Id = "devreg-disable-user-initiated-registration",
                    Label = "Block Users from Initiating Device Registration",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Prevents standard users from accessing the 'Join this device to Azure AD' and 'Connect to work or school' flows in Settings. Only administrators can register the device. Default: users allowed. Recommended: 1 on shared/kiosk endpoints.",
                    Tags = ["device-registration", "user-restriction", "settings", "enrollment", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Settings → Accounts → Access work or school registration flows are hidden for standard users.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockUserInitiatedDeviceRegistration", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockUserInitiatedDeviceRegistration")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockUserInitiatedDeviceRegistration", 1)],
                },
                new TweakDef
                {
                    Id = "devreg-enable-registration-audit-log",
                    Label = "Enable Device Registration Audit Logging",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Enables Security audit events for device registration and de-registration actions. Allows SOC/SIEM correlation of device lifecycle events with user authentication. Default: not audited. Recommended: 1 in SOC-monitored environments.",
                    Tags = ["device-registration", "audit", "logging", "security", "siem", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Device join/leave events are written to the Security event log; consumable by SIEM platforms.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableRegistrationAudit", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableRegistrationAudit")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableRegistrationAudit", 1)],
                },
                new TweakDef
                {
                    Id = "devreg-enforce-compliant-device-only",
                    Label = "Require Device Compliance for Registration",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Enforces that the device must meet Intune / Endpoint Manager compliance policies before completing Azure AD Hybrid registration. Non-compliant devices are blocked until they satisfy the compliance posture. Default: not enforced. Recommended: 1 for Conditional Access deployments.",
                    Tags = ["device-registration", "compliance", "intune", "conditional-access", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote = "Non-compliant devices (missing patches, disabled Defender) cannot complete registration; gate for Conditional Access.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireDeviceCompliance", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireDeviceCompliance")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireDeviceCompliance", 1)],
                },
                new TweakDef
                {
                    Id = "devreg-certificate-validity-days-365",
                    Label = "Set Device Certificate Validity to 365 Days",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Configures the maximum validity period for the device authentication certificate issued during Azure AD registration to 365 days. Forces annual certificate renewal, reducing the window of credential exposure. Default: 180 days. Recommended: 365 for balance.",
                    Tags = ["device-registration", "certificate", "validity", "renewal", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Device certificates are valid for 1 year; renewal is required annually to maintain device trust.",
                    ApplyOps = [RegOp.SetDword(Key, "DeviceCertValidityDays", 365)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DeviceCertValidityDays")],
                    DetectOps = [RegOp.CheckDword(Key, "DeviceCertValidityDays", 365)],
                },
                new TweakDef
                {
                    Id = "devreg-block-stale-device-reuse",
                    Label = "Block Re-Registration of Already-Registered Device Record",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Prevents a device from creating a new Azure AD registration record if a record for the same device already exists (stale object). Requires IT to clean up the old object before re-registration. Default: new record created silently. Recommended: 1.",
                    Tags = ["device-registration", "stale", "reuse", "hygiene", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Device will not create a duplicate Azure AD object; IT must retire the stale record before re-registration.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockStaleDeviceReRegistration", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockStaleDeviceReRegistration")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockStaleDeviceReRegistration", 1)],
                },
                new TweakDef
                {
                    Id = "devreg-disable-registration-status-page-skip",
                    Label = "Block Skipping Device Registration Status Page (OOBE)",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Prevents Autopilot/OOBE from skipping the device registration status page (ESP — Enrollment Status Page). Ensures the device fully completes registration before the user can log in. Default: ESP may be skipped. Recommended: 1 during Autopilot deployments.",
                    Tags = ["device-registration", "oobe", "autopilot", "esp", "enrollment", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "OOBE/Autopilot ESP is not skipped; device is fully enrolled before the first user login.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockEnrollmentStatusPageSkip", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockEnrollmentStatusPageSkip")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockEnrollmentStatusPageSkip", 1)],
                },
            ];

    }

    // ── FirmwareUpdatePolicy ──
    private static class _FirmwareUpdatePolicy
    {
        private const string FwUpdateKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\UEFI\FirmwareUpdate";

        private const string WuPolicyKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "fwupd-enable-firmware-update-via-wu",
                    Label = "Firmware Update: Enable UEFI Firmware Updates via Windows Update",
                    Category = "Device & Hardware Policy",
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
                    Category = "Device & Hardware Policy",
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
                    Category = "Device & Hardware Policy",
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
                    Category = "Device & Hardware Policy",
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
                    Category = "Device & Hardware Policy",
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
                    Category = "Device & Hardware Policy",
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
                    Category = "Device & Hardware Policy",
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
                    Category = "Device & Hardware Policy",
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
                    Category = "Device & Hardware Policy",
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
                    Category = "Device & Hardware Policy",
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

    // ── HardwareDevicePolicy ──
    private static class _HardwareDevicePolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceInstall";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "hwdev-prevent-unknown-install",
                Label = "Prevent Installation of Devices Not Described by Other Policies",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                Description =
                    "Windows device installation policies can be configured to deny installation of any device not explicitly permitted by other device policy rules. Enabling this restriction prevents users and even administrators from installing hardware not covered by an explicit allow rule. This whitelist enforcement model requires that all permitted device types and IDs be enumerated in device installation policies first. Without a comprehensive allow list this setting will block most device installations and degrade usability significantly. Enterprise USB security programs use this setting combined with device ID allow lists to create an approved-device-only environment. This is one of the most effective controls for preventing data theft via USB mass storage insertion.",
                Tags = ["hardware", "device-install", "usb", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "PreventInstallationOfDevicesNotDescribedByOtherPolicies", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "PreventInstallationOfDevicesNotDescribedByOtherPolicies")],
                DetectOps = [RegOp.CheckDword(Key, "PreventInstallationOfDevicesNotDescribedByOtherPolicies", 1)],
            },
            new TweakDef
            {
                Id = "hwdev-prevent-removable-install",
                Label = "Prevent Installation of Removable Devices",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Removable devices such as USB drives, external hard disks, and SD card readers represent significant data exfiltration and malware introduction vectors. Preventing the installation of removable devices blocks new removable storage from being registered and usable on managed endpoints. This is a critical DLP control in environments handling classified, sensitive, or regulated data. Previously installed removable devices are not affected until the next reinstallation attempt on a clean device state. Enterprise environments with legitimate removable storage requirements should use device ID allow lists in conjunction with this policy. Endpoint users requiring USB connectivity for approved devices should use centrally managed device exemptions.",
                Tags = ["hardware", "removable", "usb", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "PreventInstallationOfRemovableDevices", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "PreventInstallationOfRemovableDevices")],
                DetectOps = [RegOp.CheckDword(Key, "PreventInstallationOfRemovableDevices", 1)],
            },
            new TweakDef
            {
                Id = "hwdev-disable-drv-store-copy",
                Label = "Disable Device Driver File Store Copy",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Windows maintains a driver file store in the DriverStore which preserves driver packages after installation. Disabling the copy of device driver files to the driver store prevents accumulation of driver packages in the system-managed store. A large driver store consumes significant disk space and may include drivers for hardware no longer present. Enterprise systems with controlled hardware configurations do not benefit from storing drivers for every previously connected device. Reducing driver store size improves disk utilization on systems with limited storage capacity. Driver management can be handled through centralized driver deployment tools rather than per-device accumulation.",
                Tags = ["hardware", "drivers", "storage", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableDriverStoreCopy", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDriverStoreCopy")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDriverStoreCopy", 1)],
            },
            new TweakDef
            {
                Id = "hwdev-disable-windows-update-drivers",
                Label = "Disable Driver Download from Windows Update",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Windows Update includes a driver distribution service that automatically downloads and installs drivers for newly connected hardware. Disabling driver downloads from Windows Update prevents unapproved drivers from being retrieved and installed from external sources. Enterprise driver management requires that all deployed drivers be tested, signed, and distributed through controlled channels. Automatic driver downloads from Windows Update bypass change management processes for hardware driver updates. Driver updates should go through IT validation to ensure compatibility with enterprise applications and security baseline compliance. Approved drivers should be distributed through SCCM, Intune, or similar management tools after validation.",
                Tags = ["hardware", "drivers", "windows-update", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableWindowsUpdateDriverSearch", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableWindowsUpdateDriverSearch")],
                DetectOps = [RegOp.CheckDword(Key, "DisableWindowsUpdateDriverSearch", 1)],
            },
            new TweakDef
            {
                Id = "hwdev-disable-generic-drivers",
                Label = "Disable Installation of Generic USB Drivers",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Generic USB drivers provide basic functionality for USB devices that do not have device-specific drivers available. Disabling generic USB driver installation prevents Windows from loading fallback generic drivers for unrecognized USB devices. Unrecognized USB devices loaded through generic drivers have not been validated against enterprise security and compatibility requirements. Generic drivers may allow partial functionality of unauthorized USB devices that would otherwise be blocked. Enterprise USB whitelisting programs are more effective when generic driver loading is also disabled. Specifically approved and tested device-specific drivers should be deployed for all hardware requiring USB connectivity.",
                Tags = ["hardware", "usb", "drivers", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableGenericUsbDriverInstall", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableGenericUsbDriverInstall")],
                DetectOps = [RegOp.CheckDword(Key, "DisableGenericUsbDriverInstall", 1)],
            },
            new TweakDef
            {
                Id = "hwdev-disable-dev-metadata-internet",
                Label = "Disable Device Metadata Retrieval from Internet",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Windows retrieves device metadata from the Windows Metadata and Internet Services (WMIS) to display enhanced device information and icons in Device Manager. Disabling internet-based device metadata retrieval prevents hardware identifiers and device types from being sent to Microsoft's metadata services. Device type and hardware model information sent to external services represents sensitive asset inventory data in enterprise environments. Device metadata retrieval is unnecessary for functional device operation and serves primarily a cosmetic purpose. Enterprise device management information should be sourced from SCCM or Intune inventory rather than consumer-facing metadata services. Disabling this has no impact on device driver functionality, only on display metadata in Device Manager.",
                Tags = ["hardware", "metadata", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableDeviceMetadataFromNetwork", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDeviceMetadataFromNetwork")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDeviceMetadataFromNetwork", 1)],
            },
            new TweakDef
            {
                Id = "hwdev-disable-auto-install",
                Label = "Disable Automatic Device Installation",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Windows automatically installs drivers for newly connected hardware, even for devices not previously approved by IT. Disabling automatic device installation requires administrator action before any new hardware device becomes functional. Preventing silent automatic installation ensures that device driver changes go through change management review. Unapproved hardware connected to enterprise endpoints may load drivers that conflict with security software or introduce vulnerabilities. Automatic installation of USB HID devices including keyboards and mice in PnP scenarios can be used to launch HID injection attacks. Disabling automatic installation provides a choke point to validate new hardware before it becomes operational.",
                Tags = ["hardware", "installation", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAutoInstall", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoInstall")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAutoInstall", 1)],
            },
            new TweakDef
            {
                Id = "hwdev-disable-devinstall-telemetry",
                Label = "Disable Device Installation Telemetry",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Device installation telemetry reports hardware type information, driver versions, and installation success or failure data to Microsoft. This data helps Microsoft improve driver compatibility and the Windows hardware support experience. Disabling device installation telemetry prevents hardware inventory information from being transmitted during the device installation process. The list of hardware connected to enterprise endpoints constitutes sensitive infrastructure information. Asset inventory data should be managed through enterprise CMDB and MDM tools rather than telemetry pipelines. Device installation and driver function continue to operate normally regardless of this telemetry setting.",
                Tags = ["hardware", "telemetry", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableInstallTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableInstallTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "DisableInstallTelemetry", 1)],
            },
            new TweakDef
            {
                Id = "hwdev-disable-dev-setup-exceptions",
                Label = "Disable Device Setup Class Exceptions",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Description =
                    "Device setup class exceptions allow specific device categories to bypass general device installation restrictions. Disabling setup class exceptions prevents broad category-level exemptions from overriding restrictive device installation policies. Exception-based exemptions for device classes like network adapters or HID devices can inadvertently permit unauthorized device types. A strict enforcement model without exceptions provides more predictable and auditable device control outcomes. Enterprise device programs should use explicit device ID allow lists rather than broad class exemptions. Disabling class exceptions requires that all permitted devices be explicitly enumerated in device allow lists rather than relying on category-level bypass.",
                Tags = ["hardware", "device-class", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableSetupClassExceptions", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableSetupClassExceptions")],
                DetectOps = [RegOp.CheckDword(Key, "DisableSetupClassExceptions", 1)],
            },
            new TweakDef
            {
                Id = "hwdev-disable-drv-search-online",
                Label = "Disable Online Driver Search",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Windows driver installation can search online sources including Windows Update and third-party driver repositories for device drivers. Disabling online driver search prevents the operating system from reaching external sources to find drivers for newly connected hardware. External driver repositories are not subject to the same security validation requirements as enterprise-managed driver packages. Drivers obtained from uncontrolled online sources may contain malware, poorly implemented security controls, or known vulnerabilities. Enterprise hardware configurations should only use drivers sourced from the device manufacturer and validated by IT. Disabling online search ensures all driver installations use only drivers present in the local driver store or distributed through managed channels.",
                Tags = ["hardware", "drivers", "online", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableOnlineDriverSearch", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableOnlineDriverSearch")],
                DetectOps = [RegOp.CheckDword(Key, "DisableOnlineDriverSearch", 1)],
            },
        ];

    }

    // ── KernelDmaProtectionPolicy ──
    private static class _KernelDmaProtectionPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Kernel DMA Protection";
        private const string Key2 = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DmaSecurity";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "kdmapol-enable-dma-remapping-policy",
                    Label = "Enable DMA Remapping Enforcement Policy",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Enforces the Kernel DMA Protection policy requiring all PCIe devices to support DMA remapping (IOMMU/VT-d) before being granted full DMA access, blocking legacy DMA attack vectors.",
                    Tags = ["kernel-dma", "iommu", "vtd", "pcie", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote = "DMA remapping enforced; legacy PCIe devices without IOMMU support denied full DMA access.",
                    ApplyOps = [RegOp.SetDword(Key, "DeviceEnumerationPolicy", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DeviceEnumerationPolicy")],
                    DetectOps = [RegOp.CheckDword(Key, "DeviceEnumerationPolicy", 0)],
                },
                new TweakDef
                {
                    Id = "kdmapol-block-pre-boot-dma",
                    Label = "Block Pre-Boot DMA Access on Thunderbolt Ports",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Blocks all Thunderbolt DMA access during the pre-boot phase, preventing attacks that attach malicious Thunderbolt devices before the OS IOMMU policy is loaded.",
                    Tags = ["kernel-dma", "thunderbolt", "pre-boot", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Pre-boot Thunderbolt DMA blocked; only authorised devices can perform DMA after OS IOMMU policy loads.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowFlexibleLinkPowerManagement", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowFlexibleLinkPowerManagement")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowFlexibleLinkPowerManagement", 0)],
                },
                new TweakDef
                {
                    Id = "kdmapol-enforce-iommu-all-devices",
                    Label = "Enforce IOMMU DMA Remapping for All PCIe Devices",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Requires IOMMU DMA remapping to be applied to all PCIe devices regardless of whether they declare DMA support, ensuring legacy storage and network cards are also isolated.",
                    Tags = ["kernel-dma", "iommu", "all-devices", "pcie", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "IOMMU applied universally; legacy PCIe cards may show reduced throughput due to remapping overhead.",
                    ApplyOps = [RegOp.SetDword(Key2, "EnforceIOMMUForAllDevices", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "EnforceIOMMUForAllDevices")],
                    DetectOps = [RegOp.CheckDword(Key2, "EnforceIOMMUForAllDevices", 1)],
                },
                new TweakDef
                {
                    Id = "kdmapol-block-dma-resume-attack",
                    Label = "Block DMA Attack During Sleep/Resume Transition",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Maintains IOMMU DMA remapping tables across system sleep/hibernate and resume cycles, preventing DMA attacks that exploit the window during which remapping tables are reloaded.",
                    Tags = ["kernel-dma", "sleep", "resume", "iommu", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "DMA remapping persists across S3/S4 transitions; resume-time DMA attacks blocked.",
                    ApplyOps = [RegOp.SetDword(Key2, "MaintainRemappingOnResume", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "MaintainRemappingOnResume")],
                    DetectOps = [RegOp.CheckDword(Key2, "MaintainRemappingOnResume", 1)],
                },
                new TweakDef
                {
                    Id = "kdmapol-restrict-tb-autorisation",
                    Label = "Restrict Thunderbolt Authorisation to Admin-Only",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Restricts the authorisation of new Thunderbolt devices (adding to the trusted device store) to administrators only, preventing standard users from approving new DMA-capable Thunderbolt peripherals.",
                    Tags = ["kernel-dma", "thunderbolt", "authorisation", "admin", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Standard users cannot authorise new Thunderbolt devices; admin approval required for each new TB peripheral.",
                    ApplyOps = [RegOp.SetDword(Key2, "RestrictThunderboltAuthToAdmin", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "RestrictThunderboltAuthToAdmin")],
                    DetectOps = [RegOp.CheckDword(Key2, "RestrictThunderboltAuthToAdmin", 1)],
                },
                new TweakDef
                {
                    Id = "kdmapol-enable-dma-audit-log",
                    Label = "Enable DMA Remapping Audit Event Logging",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Enables kernel event logging for DMA remapping policy enforcement actions, recording each blocked or remapped DMA access attempt for forensic analysis.",
                    Tags = ["kernel-dma", "audit-log", "iommu", "forensics", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "DMA remapping events logged; blocked DMA attempts visible in Security/System event log.",
                    ApplyOps = [RegOp.SetDword(Key2, "EnableDMAAuditLog", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "EnableDMAAuditLog")],
                    DetectOps = [RegOp.CheckDword(Key2, "EnableDMAAuditLog", 1)],
                },
                new TweakDef
                {
                    Id = "kdmapol-block-expresscard-dma",
                    Label = "Block ExpressCard/PCMCIA DMA Access",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Blocks DMA access for legacy ExpressCard and PCMCIA devices that pre-date IOMMU support, preventing DMA attacks via older expansion card interfaces on laptops.",
                    Tags = ["kernel-dma", "expresscard", "pcmcia", "legacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "ExpressCard/PCMCIA DMA blocked; legacy expansion cards operate in PIO mode only.",
                    ApplyOps = [RegOp.SetDword(Key2, "BlockExpressCardDMA", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "BlockExpressCardDMA")],
                    DetectOps = [RegOp.CheckDword(Key2, "BlockExpressCardDMA", 1)],
                },
                new TweakDef
                {
                    Id = "kdmapol-require-vtd-for-tb4",
                    Label = "Require VT-d Active for Thunderbolt 4 Operation",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Requires Intel VT-d (IOMMU) to be active and enforcing before Thunderbolt 4 devices are enumerated and allowed DMA access, blocking TB4 use on systems with IOMMU disabled in BIOS.",
                    Tags = ["kernel-dma", "vtd", "thunderbolt-4", "iommu", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "TB4 blocked if VT-d disabled; BIOS must enable IOMMU for Thunderbolt 4 to function.",
                    ApplyOps = [RegOp.SetDword(Key2, "RequireVTdForTB4", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "RequireVTdForTB4")],
                    DetectOps = [RegOp.CheckDword(Key2, "RequireVTdForTB4", 1)],
                },
                new TweakDef
                {
                    Id = "kdmapol-block-usb4-dma-without-auth",
                    Label = "Block USB4 DMA Without Device Authorisation",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Requires explicit device authorisation for USB4 (Thunderbolt-tunnelled) DMA access, blocking USB4 tunnelled DMA from unapproved devices until confirmed by an administrator.",
                    Tags = ["kernel-dma", "usb4", "thunderbolt", "authorisation", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "USB4 DMA blocked until device is admin-authorised; unauthorised USB4 devices cannot perform DMA.",
                    ApplyOps = [RegOp.SetDword(Key2, "BlockUSB4DMAWithoutAuth", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "BlockUSB4DMAWithoutAuth")],
                    DetectOps = [RegOp.CheckDword(Key2, "BlockUSB4DMAWithoutAuth", 1)],
                },
                new TweakDef
                {
                    Id = "kdmapol-set-remapping-timeout",
                    Label = "Set DMA Remapping Table Rebuild Timeout to 5 Seconds",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Limits the DMA remapping table rebuild timeout to 5 seconds, ensuring that if a device fails to initialise IOMMU remapping within the timeout, it is disconnected rather than granted unrestricted DMA.",
                    Tags = ["kernel-dma", "remapping", "timeout", "iommu", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "DMA devices failing IOMMU init are disconnected after 5 seconds; no silent fallback to unrestricted DMA.",
                    ApplyOps = [RegOp.SetDword(Key2, "RemappingTableRebuildTimeoutSec", 5)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "RemappingTableRebuildTimeoutSec")],
                    DetectOps = [RegOp.CheckDword(Key2, "RemappingTableRebuildTimeoutSec", 5)],
                },
            ];

    }

    // ── MemoryDiagnostics ──
    private static class _MemoryDiagnostics
    {
        private const string CrashControl =
            @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl";

        private const string Wer =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting";

        private const string WerQueue =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting\Queue";

        private const string WerConsentPolicy =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PCHealth\ErrorReporting";

        private const string WerPolicy =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "dump-disable-crash-dumps",
                Label = "Disable Crash Memory Dumps (No Dump Files Written)",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = false,
                ImpactScore = 3,
                SafetyRating = 3,
                Tags = ["dump", "crash", "memory", "disk space"],
                Description =
                    "Prevents Windows from writing any memory dump file when the system "
                    + "crashes (BSOD). Saves substantial disk space on SSDs but eliminates "
                    + "crash debugging capability. CrashDumpEnabled=0.",
                ApplyOps = [RegOp.SetDword(CrashControl, "CrashDumpEnabled", 0)],
                RemoveOps = [RegOp.SetDword(CrashControl, "CrashDumpEnabled", 7)],
                DetectOps = [RegOp.CheckDword(CrashControl, "CrashDumpEnabled", 0)],
            },
            new TweakDef
            {
                Id = "dump-set-small-minidump",
                Label = "Set Crash Dump to Small Memory Dump (256 KB)",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["dump", "crash", "minidump", "disk space"],
                Description =
                    "Configures Windows to write only a small memory dump (256 KB) on BSOD "
                    + "instead of a full kernel or complete dump. Preserves basic debug info "
                    + "(stop code, loaded drivers) with minimal disk use.",
                ApplyOps = [RegOp.SetDword(CrashControl, "CrashDumpEnabled", 3)],
                RemoveOps = [RegOp.SetDword(CrashControl, "CrashDumpEnabled", 7)],
                DetectOps = [RegOp.CheckDword(CrashControl, "CrashDumpEnabled", 3)],
            },
            new TweakDef
            {
                Id = "dump-enable-auto-reboot-on-crash",
                Label = "Enable Automatic Reboot After BSOD",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["dump", "crash", "reboot", "bsod"],
                Description =
                    "Ensures Windows automatically restarts after a system crash (BSOD) "
                    + "rather than staying at the blue screen indefinitely. Default behaviour "
                    + "on most systems but restorable if previously disabled.",
                ApplyOps = [RegOp.SetDword(CrashControl, "AutoReboot", 1)],
                RemoveOps = [RegOp.SetDword(CrashControl, "AutoReboot", 0)],
                DetectOps = [RegOp.CheckDword(CrashControl, "AutoReboot", 1)],
            },
            new TweakDef
            {
                Id = "dump-overwrite-existing-dump",
                Label = "Overwrite Existing Dump File on Crash",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["dump", "crash", "overwrite", "disk space"],
                Description =
                    "Configures Windows to overwrite the existing dump file rather than "
                    + "keeping multiple copies. Prevents the dumps folder from consuming "
                    + "multiple GBs after repeated crashes.",
                ApplyOps = [RegOp.SetDword(CrashControl, "Overwrite", 1)],
                RemoveOps = [RegOp.DeleteValue(CrashControl, "Overwrite")],
                DetectOps = [RegOp.CheckDword(CrashControl, "Overwrite", 1)],
            },
            new TweakDef
            {
                Id = "dump-disable-wer-error-reporting",
                Label = "Disable Windows Error Reporting (WER)",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = false,
                ImpactScore = 3,
                SafetyRating = 4,
                Tags = ["wer", "error reporting", "privacy", "telemetry"],
                Description =
                    "Disables Windows Error Reporting so application crashes are not "
                    + "sent to Microsoft. Reduces network activity and privacy exposure "
                    + "but prevents automatic driver/app fix recommendations.",
                ApplyOps = [RegOp.SetDword(Wer, "Disabled", 1)],
                RemoveOps = [RegOp.SetDword(Wer, "Disabled", 0)],
                DetectOps = [RegOp.CheckDword(Wer, "Disabled", 1)],
            },
            new TweakDef
            {
                Id = "dump-disable-wer-reporting-policy",
                Label = "Disable WER via Group Policy",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = false,
                ImpactScore = 3,
                SafetyRating = 4,
                Tags = ["wer", "policy", "privacy", "error reporting"],
                Description =
                    "Disables Windows Error Reporting via the machine-wide policy key. "
                    + "This prevents users from re-enabling WER through Settings. "
                    + "Complementary to the user-level Disabled flag.",
                ApplyOps = [RegOp.SetDword(WerPolicy, "Disabled", 1)],
                RemoveOps = [RegOp.DeleteValue(WerPolicy, "Disabled")],
                DetectOps = [RegOp.CheckDword(WerPolicy, "Disabled", 1)],
            },
            new TweakDef
            {
                Id = "dump-disable-wer-queue",
                Label = "Disable WER Queuing of Crash Reports",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = false,
                ImpactScore = 2,
                SafetyRating = 4,
                Tags = ["wer", "queue", "privacy", "error reporting"],
                Description =
                    "Prevents WER from queuing crash reports for later upload. Stops "
                    + "the background spool of error data that WER accumulates when an "
                    + "internet connection is unavailable.",
                ApplyOps = [RegOp.SetDword(WerQueue, "Disable", 1)],
                RemoveOps = [RegOp.SetDword(WerQueue, "Disable", 0)],
                DetectOps = [RegOp.CheckDword(WerQueue, "Disable", 1)],
            },
            new TweakDef
            {
                Id = "dump-disable-silent-crash-ui",
                Label = "Disable Silent Crash Report Dialog",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = false,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["wer", "dialog", "crash", "ui"],
                Description =
                    "Suppresses the WER consent dialog that asks the user whether to send "
                    + "a crash report. Combined with Disabled=1, fully silences WER. "
                    + "Applied via policy (DontShowUI).",
                ApplyOps = [RegOp.SetDword(WerPolicy, "DontShowUI", 1)],
                RemoveOps = [RegOp.DeleteValue(WerPolicy, "DontShowUI")],
                DetectOps = [RegOp.CheckDword(WerPolicy, "DontShowUI", 1)],
            },
            new TweakDef
            {
                Id = "dump-set-kernel-only-dump",
                Label = "Set Crash Dump to Kernel Memory Dump",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["dump", "crash", "kernel", "debug"],
                Description =
                    "Configures Windows to write a kernel memory dump on BSOD. Captures "
                    + "kernel memory pages only (not user-space), providing enough data for "
                    + "most crash analysis while being smaller than a complete dump.",
                ApplyOps = [RegOp.SetDword(CrashControl, "CrashDumpEnabled", 2)],
                RemoveOps = [RegOp.SetDword(CrashControl, "CrashDumpEnabled", 7)],
                DetectOps = [RegOp.CheckDword(CrashControl, "CrashDumpEnabled", 2)],
            },
            new TweakDef
            {
                Id = "dump-enable-log-on-crash",
                Label = "Enable Kernel Event Log Entry on Crash",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Tags = ["dump", "crash", "event log", "logging"],
                Description =
                    "Ensures Windows writes a 41 (Kernel-Power) or 1001 (BugCheck) event "
                    + "log entry to the System log after a system crash and reboot. "
                    + "LogEvent=1 (default on, restoring is useful if accidentally disabled).",
                ApplyOps = [RegOp.SetDword(CrashControl, "LogEvent", 1)],
                RemoveOps = [RegOp.DeleteValue(CrashControl, "LogEvent")],
                DetectOps = [RegOp.CheckDword(CrashControl, "LogEvent", 1)],
            },
        ];

    }

    // ── PageFilePolicy ──
    private static class _PageFilePolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PageFile";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "pgfpol-ensure-pagefile-enabled",
                Label = "Ensure Page File Is Enabled",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "The Windows page file provides virtual memory overflow capacity by extending RAM with disk storage. Ensuring the page file is not forcibly disabled prevents out-of-memory conditions that can crash applications and the operating system. This policy verifies that DisablePageFile is set to zero, meaning the page file is permitted to exist. Systems processing large datasets, running virtual machines, or hosting multiple applications depend on adequate virtual memory. Removing the page file entirely can cause critical system failures on memory-constrained workloads. Maintaining this setting at its safe default ensures system stability across diverse workload profiles.",
                Tags = ["pagefile", "memory", "stability", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePageFile", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePageFile")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePageFile", 0)],
            },
            new TweakDef
            {
                Id = "pgfpol-clear-pagefile-shutdown",
                Label = "Clear Page File at Shutdown",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "The page file can retain sensitive data written there by applications during a session, including encryption keys, passwords, and confidential documents. Clearing the page file at shutdown overwrites the page file contents with zeros, preventing data recovery from the swap space. This is a security hardening measure required by many compliance frameworks including NIST, CIS, and DoD STIGs. The clearing operation adds time to the shutdown sequence proportional to the page file size and storage speed. Systems with large page files on slow HDDs may experience noticeably longer shutdown times. On SSDs the performance impact is minimal, and the security benefit justifies the small delay in all regulated environments.",
                Tags = ["pagefile", "security", "shutdown", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ClearPageFileAtShutdown", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "ClearPageFileAtShutdown")],
                DetectOps = [RegOp.CheckDword(Key, "ClearPageFileAtShutdown", 1)],
            },
            new TweakDef
            {
                Id = "pgfpol-ensure-swapfile-active",
                Label = "Ensure Swap File Is Active",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "The Windows swap file is a separate virtual memory file used for background application paging on modern Windows versions. Ensuring the swap file is not disabled preserves the operating system's ability to handle memory pressure through multiple paging mechanisms. This policy sets DisableSwapFile to zero, confirming the swap file remains active for background app memory management. Disabling the swap file on RAM-constrained systems can lead to application termination under memory pressure. The swap file works in conjunction with the page file to provide comprehensive virtual memory management. Maintaining this setting at its default ensures predictable memory behavior for suspended applications.",
                Tags = ["pagefile", "swapfile", "memory", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableSwapFile", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableSwapFile")],
                DetectOps = [RegOp.CheckDword(Key, "DisableSwapFile", 0)],
            },
            new TweakDef
            {
                Id = "pgfpol-set-max-size-4096",
                Label = "Set Maximum Page File Size 4096 MB",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Description =
                    "Setting a maximum page file size prevents the page file from growing unboundedly and consuming excessive disk space on system drives. A 4096 MB maximum represents a balanced limit suitable for most enterprise workstations with 16 GB or more of installed RAM. Unbounded page file growth can fill system drives and trigger low-disk-space conditions that destabilize the operating system. This setting must be calibrated against the actual workload requirements of the target machine class. Memory-intensive workloads such as database servers, virtualization hosts, and development environments may require higher limits. Administrators should monitor memory usage before applying this limit to production systems.",
                Tags = ["pagefile", "memory", "disk", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "MaxPageFileSize", 4096)],
                RemoveOps = [RegOp.DeleteValue(Key, "MaxPageFileSize")],
                DetectOps = [RegOp.CheckDword(Key, "MaxPageFileSize", 4096)],
            },
            new TweakDef
            {
                Id = "pgfpol-disable-peak-detection",
                Label = "Disable Automatic Peak Page File Detection",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Windows automatically adjusts the page file size based on observed peak memory usage patterns over time. This adaptive mechanism can cause the page file to fluctuate in size, leading to disk fragmentation on HDD systems. Disabling automatic peak detection freezes the page file at its configured size, providing predictable storage consumption. Administrators managing server environments with known memory requirements benefit from deterministic page file sizing. The adaptive mechanism is primarily beneficial on consumer devices with widely varying workloads. Enterprise systems with stable application loads gain more from a fixed, well-configured page file size than from dynamic adjustment.",
                Tags = ["pagefile", "memory", "performance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAutomaticPeakDetection", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAutomaticPeakDetection")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAutomaticPeakDetection", 1)],
            },
            new TweakDef
            {
                Id = "pgfpol-allow-system-managed",
                Label = "Allow System-Managed Page File",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "The system-managed page file allows Windows to dynamically size and manage the page file based on available disk space and observed usage patterns. Allowing system management ensures the page file automatically adjusts to unusual workload spikes that exceed manually configured sizes. This policy sets DisableSystemManagedPageFile to zero, preserving the default system management behavior. Organizations relying on Windows to handle memory management automatically benefit from this setting on general-purpose workstations. Environments with strict resource governance may prefer manual page file sizing, but system management provides a reliable fallback. This setting is safe and recommended unless a specific maximum size policy is required.",
                Tags = ["pagefile", "memory", "system", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableSystemManagedPageFile", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableSystemManagedPageFile")],
                DetectOps = [RegOp.CheckDword(Key, "DisableSystemManagedPageFile", 0)],
            },
            new TweakDef
            {
                Id = "pgfpol-allow-low-memory-detection",
                Label = "Allow Low Memory Detection",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Low memory detection triggers warnings and system responses when available physical and virtual memory falls below critical thresholds. Maintaining the low memory detection mechanism active at its default ensures the system can respond appropriately to memory pressure. This policy sets DisableLowMemoryDetection to zero, preserving protective system behavior. With detection enabled, the system can proactively terminate unresponsive processes and log diagnostic information before a complete memory exhaustion event. Disabling detection removes the safety net and can cause sudden application crashes or system hangs without warning. This setting should remain at the safe default on all production systems.",
                Tags = ["pagefile", "memory", "stability", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableLowMemoryDetection", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableLowMemoryDetection")],
                DetectOps = [RegOp.CheckDword(Key, "DisableLowMemoryDetection", 0)],
            },
            new TweakDef
            {
                Id = "pgfpol-place-on-system-drive",
                Label = "Place Page File on System Drive",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Placing the page file on the system drive keeps virtual memory on the fastest and most reliable storage device in most configurations. The system drive typically hosts the operating system on an NVMe or SATA SSD with superior I/O characteristics. This policy ensures the page file is configured to use the system drive, providing consistent performance for virtual memory operations. Keeping the page file on the system drive also simplifies disk management by avoiding dependency on secondary drives. On multi-drive configurations, administrators may prefer secondary drives for page file placement to reduce I/O contention on the system drive. This setting is appropriate for workstations with a single high-performance storage device.",
                Tags = ["pagefile", "storage", "performance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "PageFileOnSystemDrive", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "PageFileOnSystemDrive")],
                DetectOps = [RegOp.CheckDword(Key, "PageFileOnSystemDrive", 1)],
            },
            new TweakDef
            {
                Id = "pgfpol-disable-telemetry",
                Label = "Disable Page File Telemetry",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Page file telemetry transmits metrics about virtual memory usage patterns, page fault rates, and page file sizing to Microsoft. This data provides Microsoft with insight into memory pressure scenarios across the Windows user base. Disabling page file telemetry prevents virtual memory utilization data from being transmitted to external services. Organizations with strict data residency requirements or network egress monitoring policies benefit from disabling this telemetry. The page file continues to function identically regardless of whether telemetry is enabled. Administrators can obtain equivalent memory usage insights through Windows Performance Monitor and ETW tracing.",
                Tags = ["pagefile", "telemetry", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePageFileTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePageFileTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePageFileTelemetry", 1)],
            },
            new TweakDef
            {
                Id = "pgfpol-disable-memory-dump",
                Label = "Disable Memory Dump Creation",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Description =
                    "Memory dumps capture the contents of RAM to disk following a system crash and are stored in the page file or dedicated dump files. These dump files can contain sensitive data including credentials, encryption keys, and application data present in memory at crash time. Disabling memory dump creation prevents sensitive memory contents from being written to disk where they could be extracted. Security-hardened environments and regulated industries often disable memory dumps as part of data-at-rest protection policies. The tradeoff is reduced diagnostic capability for analyzing crash root causes in post-incident investigations. Environments with stringent memory protection requirements should disable dumps and rely on live debugging or remote crash analysis tools.",
                Tags = ["pagefile", "memory-dump", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableMemoryDump", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableMemoryDump")],
                DetectOps = [RegOp.CheckDword(Key, "DisableMemoryDump", 1)],
            },
        ];

    }

    // ── PortableDevicePolicy ──
    private static class _PortableDevicePolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceInstall\Restrictions";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wpd-deny-removable-devices",
                    Label = "Deny All Removable Device Installation",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Blocks installation of all removable storage devices (USB drives, SD cards, portable media players) by denying their device class setup, preventing data exfiltration via removable media.",
                    Tags = ["wpd", "removable-device", "usb", "storage", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "All removable storage devices blocked at installation; USB drives and SD cards cannot be used.",
                    ApplyOps = [RegOp.SetDword(Key, "DenyRemovableDevices", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DenyRemovableDevices")],
                    DetectOps = [RegOp.CheckDword(Key, "DenyRemovableDevices", 1)],
                },
                new TweakDef
                {
                    Id = "wpd-deny-portable-devices",
                    Label = "Deny Portable Device (MTP/WPD) Access",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Blocks access to Windows Portable Devices (WPD) via the Media Transfer Protocol (MTP), preventing smartphones, cameras, and media players from accessing or transferring files when connected via USB.",
                    Tags = ["wpd", "mtp", "portable-device", "usb", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "WPD MTP access blocked; phones/cameras connected via USB cannot transfer files.",
                    ApplyOps = [RegOp.SetDword(Key, "DenyPortableDevices", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DenyPortableDevices")],
                    DetectOps = [RegOp.CheckDword(Key, "DenyPortableDevices", 1)],
                },
                new TweakDef
                {
                    Id = "wpd-block-autoplay-portable",
                    Label = "Block AutoPlay for Portable Media Devices",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Disables the AutoPlay action that launches when a portable device (camera, media player, phone) is connected, preventing automatic media import dialogs and auto-execution of content from portable devices.",
                    Tags = ["wpd", "autoplay", "portable-device", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "AutoPlay disabled for portable devices; no media import dialog when connecting cameras or phones.",
                    ApplyOps = [RegOp.SetDword(Key, "NoAutoplayForPortableDevices", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NoAutoplayForPortableDevices")],
                    DetectOps = [RegOp.CheckDword(Key, "NoAutoplayForPortableDevices", 1)],
                },
                new TweakDef
                {
                    Id = "wpd-block-camera-device-install",
                    Label = "Block Camera Device Installation",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Prevents installation of USB-connected camera devices (webcams, digital cameras) on the system, useful in secure environments where all photography and video capture must be blocked.",
                    Tags = ["wpd", "camera", "usb", "device-install", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "USB camera devices blocked from installing; no external webcam or digital camera usable.",
                    ApplyOps = [RegOp.SetDword(Key, "DenyCameraDevices", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DenyCameraDevices")],
                    DetectOps = [RegOp.CheckDword(Key, "DenyCameraDevices", 1)],
                },
                new TweakDef
                {
                    Id = "wpd-disable-picture-transfer",
                    Label = "Disable Windows Picture Transfer Protocol",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Disables the Picture Transfer Protocol (PTP) used by digital cameras and smartphones to transfer photos, preventing photo device discovery and auto-import from cameras.",
                    Tags = ["wpd", "ptp", "picture-transfer", "camera", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "PTP picture transfer disabled; digital cameras and phones in PTP mode cannot transfer photos.",
                    ApplyOps = [RegOp.SetDword(Key, "DisablePTPTransfer", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisablePTPTransfer")],
                    DetectOps = [RegOp.CheckDword(Key, "DisablePTPTransfer", 1)],
                },
                new TweakDef
                {
                    Id = "wpd-apply-audit-on-write",
                    Label = "Enable Write Audit Logging for Removable Media",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Enables security audit events when files are written to removable storage devices, creating a log trail for data being exfiltrated to USB drives or portable devices.",
                    Tags = ["wpd", "removable-media", "audit-log", "dlp", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Write-to-removable-media events logged; potential data exfiltration to USB drives is auditable.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditRemovableMediaWrites", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditRemovableMediaWrites")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditRemovableMediaWrites", 1)],
                },
                new TweakDef
                {
                    Id = "wpd-disable-usb-mass-storage",
                    Label = "Disable USB Mass Storage Class Driver",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Disables the USB Mass Storage class driver (usbstor), preventing USB flash drives, external hard drives, and USB memory sticks from mounting as drive letters in Windows Explorer.",
                    Tags = ["wpd", "usb-mass-storage", "usbstor", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "USB mass storage driver disabled; USB drives, external HDDs, and flash drives cannot be used.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableUSBMassStorage", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableUSBMassStorage")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableUSBMassStorage", 1)],
                },
                new TweakDef
                {
                    Id = "wpd-block-portable-music-player",
                    Label = "Block Portable Music Player Synchronisation",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Blocks synchronisation between Windows media players (Windows Media Player sync) and portable MP3 or music players via USB, preventing media content from being exported to portable devices.",
                    Tags = ["wpd", "music-player", "sync", "media", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Portable music player sync blocked; cannot sync music files from WMP to MP3 players.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockPortableMusicPlayerSync", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockPortableMusicPlayerSync")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockPortableMusicPlayerSync", 1)],
                },
                new TweakDef
                {
                    Id = "wpd-readonly-removable-media",
                    Label = "Enforce Read-Only Mode for All Removable Media",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Mounts all removable storage devices in read-only mode, allowing data to be read from USB drives but blocking any write operations to prevent data exfiltration.",
                    Tags = ["wpd", "removable-media", "read-only", "dlp", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Removable media read-only; cannot write files to USB drives or SD cards.",
                    ApplyOps = [RegOp.SetDword(Key, "ReadOnlyRemovableMedia", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ReadOnlyRemovableMedia")],
                    DetectOps = [RegOp.CheckDword(Key, "ReadOnlyRemovableMedia", 1)],
                },
                new TweakDef
                {
                    Id = "wpd-block-external-thunderbolt-storage",
                    Label = "Block External Thunderbolt Storage Devices",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Prevents external storage devices connected via Thunderbolt from mounting, closing the high-speed Thunderbolt exfiltration path while allowing other Thunderbolt peripherals like displays and docks.",
                    Tags = ["wpd", "thunderbolt", "storage", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Thunderbolt external storage blocked; TB drives and NVMe enclosures cannot mount.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockThunderboltStorage", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockThunderboltStorage")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockThunderboltStorage", 1)],
                },
            ];

    }

    // ── PortableDevicesPolicy ──
    private static class _PortableDevicesPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PortableDevices";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "portdev-disable-autoplay",
                Label = "Disable AutoPlay for Portable Devices",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "AutoPlay automatically launches program content from portable devices like cameras, phones, and media players when they are connected. Disabling AutoPlay for portable devices prevents any automatic execution or media presentation when a portable device is connected. AutoPlay has historically been a vector for malware distribution through infected portable media. Even without AutoRun execution, AutoPlay dialog prompts can trigger user-initiated malware installation through social engineering. Disabling AutoPlay is a foundational security hardening step recommended by security benchmarks including CIS controls. Users requiring access to portable device content can browse it manually through File Explorer.",
                Tags = ["portable-devices", "autoplay", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAutoPlay", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoPlay")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAutoPlay", 1)],
            },
            new TweakDef
            {
                Id = "portdev-disable-sync",
                Label = "Disable Portable Device Sync",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Portable device sync transfers content between Windows and MTP/PTP compliant devices such as smartphones and cameras. Disabling portable device sync prevents automatic data synchronization when portable devices are connected to managed endpoints. Sync operations can transfer corporate data from managed endpoints to unmanaged portable devices creating data leakage risks. Conversely sync can introduce malicious files from personal portable devices into the corporate endpoint filesystem. Enterprise DLP controls for portable devices should include both read and write restriction capabilities. This policy does not prevent USB charging but does prevent file system access through MTP/PTP protocol.",
                Tags = ["portable-devices", "sync", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePortableDeviceSync", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePortableDeviceSync")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePortableDeviceSync", 1)],
            },
            new TweakDef
            {
                Id = "portdev-disable-media-acquisition",
                Label = "Disable Portable Device Media Acquisition",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Media acquisition allows Windows to automatically import photos, videos, and audio from cameras and other MTP devices connected to the system. Disabling media acquisition prevents automatic media transfer from portable devices including smartphones and digital cameras. Automatic media import can inadvertently transfer confidential photos or videos to the enterprise endpoint from personal devices. Enterprise endpoints used by regulated industry workers should not auto-import media from unmanaged devices. Media acquired from personal devices may contain content that violates enterprise acceptable use policies. Disabling media acquisition requires users to manually manage any legitimate data transfer from authorized portable devices.",
                Tags = ["portable-devices", "media", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableMediaAcquisition", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableMediaAcquisition")],
                DetectOps = [RegOp.CheckDword(Key, "DisableMediaAcquisition", 1)],
            },
            new TweakDef
            {
                Id = "portdev-disable-wpdautorun",
                Label = "Disable Windows Portable Device AutoRun",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Windows Portable Device AutoRun allows programs on portable devices to automatically execute when the device is connected. Disabling WPD AutoRun prevents any executable content on portable devices from running automatically upon connection. AutoRun-based malware has been one of the most prevalent endpoint compromise mechanisms throughout computing history. Even modern Windows systems with AutoRun disabled may still be vulnerable to device-triggered execution through driver-level attack paths. Disabling WPD AutoRun removes this entire class of automatic execution vectors from portable device connections. This setting is a prerequisite for any enterprise environment running security baseline configurations.",
                Tags = ["portable-devices", "autorun", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableWpdAutoRun", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableWpdAutoRun")],
                DetectOps = [RegOp.CheckDword(Key, "DisableWpdAutoRun", 1)],
            },
            new TweakDef
            {
                Id = "portdev-deny-read",
                Label = "Deny Read Access to Portable Devices",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Description =
                    "Portable device read access controls whether Windows allows users to read files from MTP and WPD devices connected to the system. Denying read access prevents users from copying data from portable devices to the managed endpoint. This control is used in high-security environments where no data should flow from unmanaged devices to managed endpoints. Even when read is denied, charging via USB is not blocked by this policy setting. This setting is most effective when combined with write denial to create a full bidirectional portable device data isolation policy. Organizations deploying this control should communicate the restriction to users and provide approved data transfer procedures.",
                Tags = ["portable-devices", "read-access", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DenyPortableDeviceRead", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DenyPortableDeviceRead")],
                DetectOps = [RegOp.CheckDword(Key, "DenyPortableDeviceRead", 1)],
            },
            new TweakDef
            {
                Id = "portdev-deny-write",
                Label = "Deny Write Access to Portable Devices",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Description =
                    "Portable device write access controls whether Windows allows users to transfer files to MTP and WPD devices connected to the system. Denying write access prevents users from copying data from the managed endpoint to portable devices. This is a primary data loss prevention control for organizations at risk of intentional or inadvertent data exfiltration via portable storage. Corporate documents, database exports, source code, and other sensitive data must not be movable to unmanaged personal devices. Write denial is more critical than read denial from a DLP perspective since outbound data movement represents the primary exfiltration vector. Organizations should pair this policy with removable storage write denial for comprehensive portable device data control.",
                Tags = ["portable-devices", "write-access", "dlp", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DenyPortableDeviceWrite", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DenyPortableDeviceWrite")],
                DetectOps = [RegOp.CheckDword(Key, "DenyPortableDeviceWrite", 1)],
            },
            new TweakDef
            {
                Id = "portdev-disable-camera-access",
                Label = "Disable Camera Portable Device Access",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Camera portable device access allows Windows to connect to digital cameras and import photos through the Windows Portable Device framework. Disabling camera access prevents Windows from enumerating and accessing digital cameras connected via USB or wireless protocols. In secure facility environments, personal cameras and recording devices are commonly prohibited to prevent capture of sensitive displays or infrastructure. Disabling camera device access provides a software enforcement layer complementary to physical camera possession policies. Enterprise endpoints in secure rooms and data centers should not be connectable to camera devices. This policy applies to the WPD camera device class and does not affect integrated webcam functionality.",
                Tags = ["portable-devices", "camera", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableCameraDeviceAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCameraDeviceAccess")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCameraDeviceAccess", 1)],
            },
            new TweakDef
            {
                Id = "portdev-disable-wpd-driver-install",
                Label = "Disable WPD Driver Auto-Installation",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Windows Portable Device driver installation automatically installs WPD drivers when new portable devices are discovered for the first time. Disabling WPD driver auto-installation prevents new portable device drivers from being loaded when unrecognized devices are connected. Without approved drivers, unrecognized portable devices cannot access the WPD/MTP protocol stack and file access is prevented. This prevents unknown portable devices from becoming functional even if connected to the endpoint. Approved device drivers for authorized portable devices should be deployed through managed software distribution. Combining driver installation control with explicit device ID policies creates a comprehensive portable device access control framework.",
                Tags = ["portable-devices", "drivers", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableWpdDriverAutoInstall", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableWpdDriverAutoInstall")],
                DetectOps = [RegOp.CheckDword(Key, "DisableWpdDriverAutoInstall", 1)],
            },
            new TweakDef
            {
                Id = "portdev-disable-wpd-notification",
                Label = "Disable Portable Device Connection Notifications",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Windows displays toast notifications and AutoPlay prompts when portable devices are connected to the system. Disabling portable device connection notifications suppresses the pop-up dialogs and notification center alerts triggered by device connection. Notification suppression prevents user interaction with connection prompts that could lead to inadvertent data transfer. Eliminating connection prompts reduces distraction and improves the user experience on shared or kiosk endpoints. Notification suppression is a non-essential cosmetic enhancement that does not affect the underlying device access block policies. Blocking notifications does not independently prevent device access and should be deployed alongside substantive access control policies.",
                Tags = ["portable-devices", "notifications", "usability", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePortableDeviceNotifications", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePortableDeviceNotifications")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePortableDeviceNotifications", 1)],
            },
            new TweakDef
            {
                Id = "portdev-disable-telemetry",
                Label = "Disable Portable Device Telemetry",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Portable device telemetry collects data about device types, connection frequency, and transfer statistics when portable devices are connected. This telemetry is used to improve Windows compatibility with a wide range of portable devices and optimize the connection experience. Disabling this telemetry prevents information about connected portable devices from being reported to Microsoft. Device type and model information sent through telemetry reveals what personal devices are connected to enterprise endpoints. Asset and inventory information about personal devices is not appropriate for external data collection in enterprise environments. Portable device functionality is entirely unaffected by disabling this telemetry stream.",
                Tags = ["portable-devices", "telemetry", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePortableDeviceTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePortableDeviceTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePortableDeviceTelemetry", 1)],
            },
        ];

    }

    // ── ProcessorPolicy ──
    private static class _ProcessorPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Processor";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "proccpol-disable-speculative-execution",
                Label = "Enable Spectre/Meltdown Mitigations",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Spectre and Meltdown are hardware vulnerabilities in modern processors that allow malicious code to read arbitrary memory through side-channel attacks. Enabling processor mitigations activates kernel and firmware-level protections that prevent exploitation of these speculative execution vulnerabilities. Without mitigations enabled malicious processes can read kernel memory, other process memory, and hypervisor memory they should not have access to. Intel, AMD, and ARM all released firmware and microcode updates to address these vulnerabilities when combined with OS-level mitigations. Performance impact from mitigations varies by workload but security benefits far outweigh the performance cost for enterprise endpoints. Mitigations must be enabled both through OS policy and microcode/firmware updates for complete protection.",
                Tags = ["processor", "spectre", "meltdown", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableMitigations", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableMitigations")],
                DetectOps = [RegOp.CheckDword(Key, "EnableMitigations", 1)],
            },
            new TweakDef
            {
                Id = "proccpol-enable-retpoline",
                Label = "Enable Retpoline Spectre Variant 2 Mitigation",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Retpoline is a software mitigation technique that replaces indirect branch instructions with a safer equivalent that prevents branch target injection. Enabling Retpoline activates the compiler-based mitigation for Spectre variant 2 branch target injection vulnerabilities. Spectre variant 2 allows malicious code to manipulate CPU branch prediction to speculatively execute code at arbitrary locations. Retpoline provides Spectre variant 2 protection with significantly lower performance overhead than alternative mitigations. Windows builds include Retpoline when supported by the system configuration including processor microcode and OS version. Retpoline is the preferred mitigation approach and should be enabled wherever the required processor and system support is present.",
                Tags = ["processor", "spectre", "retpoline", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableRetpoline", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableRetpoline")],
                DetectOps = [RegOp.CheckDword(Key, "EnableRetpoline", 1)],
            },
            new TweakDef
            {
                Id = "proccpol-enable-kva-shadowing",
                Label = "Enable Kernel VA Shadowing (Meltdown Mitigation)",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Kernel Virtual Address Shadowing separates kernel and user address spaces to prevent user-mode code from accessing kernel memory through Meltdown. KVA Shadowing ensures that kernel pages are not mapped into user process address space preventing Meltdown-style reads of kernel data. Meltdown allows user processes to read arbitrary kernel memory including passwords, encryption keys, and other sensitive data. KVA Shadowing was introduced in Windows 10 1803 as the primary Meltdown mitigation for Intel CPUs. AMD CPUs are generally not vulnerable to Meltdown but enabling KVA Shadowing provides defense-in-depth. KVA Shadowing does have a performance impact on workloads with frequent kernel transitions but the security benefit is essential.",
                Tags = ["processor", "meltdown", "kva", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableKvaShadowing", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableKvaShadowing")],
                DetectOps = [RegOp.CheckDword(Key, "EnableKvaShadowing", 1)],
            },
            new TweakDef
            {
                Id = "proccpol-enable-ssbd",
                Label = "Enable Speculative Store Bypass Disable (SSBD)",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Speculative Store Bypass is a CPU vulnerability where speculative execution can bypass store-to-load forwarding and read stale data from memory. Enabling SSBD activates hardware mitigation via the SSBD MSR bit that prevents speculative access to data from prior stores. Speculative Store Bypass can be exploited to read data that should have been overwritten or isolated by store operations. SSBD is required for JIT-compiled execution environments including browser JavaScript engines where multiple execution contexts share a process. Enterprise endpoints running JavaScript-based enterprise applications may be vulnerable through browser JIT compilation without SSBD. SSBD has low performance impact and should be enabled on all processors that support the SSBD hardware bit.",
                Tags = ["processor", "ssbd", "speculative", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableSSBD", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableSSBD")],
                DetectOps = [RegOp.CheckDword(Key, "EnableSSBD", 1)],
            },
            new TweakDef
            {
                Id = "proccpol-enable-mds-mitigations",
                Label = "Enable Microarchitectural Data Sampling Mitigations",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Microarchitectural Data Sampling vulnerabilities including RIDL and Fallout allow processes to sample data from CPU internal buffers during speculative execution. Enabling MDS mitigations activates CPU buffer clearing operations that flush microarchitectural buffers to prevent cross-domain data leakage. MDS attacks can leak data across process boundaries, hypervisor boundaries, and between SMT sibling threads in Intel processors. On systems with Hyper-Threading or SMT enabled MDS mitigations may include disabling SMT for complete protection. Intel CPUs from Cascade Lake and later include hardware mitigations that reduce the performance impact of software MDS mitigations. MDS mitigations should be enabled on all Intel processors that do not have hardware MDS mitigations built in.",
                Tags = ["processor", "mds", "speculative", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableMDSMitigations", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableMDSMitigations")],
                DetectOps = [RegOp.CheckDword(Key, "EnableMDSMitigations", 1)],
            },
            new TweakDef
            {
                Id = "proccpol-disable-hyper-threading-spectre",
                Label = "Configure SMT for Speculative Execution Safety",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Simultaneous Multi-Threading shares processor resources between logical cores which creates side-channel leakage paths for speculative execution attacks. Configuring SMT safely for speculative execution ensures that sibling thread data is isolated through appropriate microarchitectural mitigations. MDS and cache-based side-channel attacks are more effective when the attacker and victim share an SMT core. For extremely high-security workloads where perfect isolation is required SMT disabling may be considered despite the performance impact. Modern processor microcode combined with OS MDS mitigations provides substantial SMT isolation that covers most enterprise threat models. Security teams should evaluate whether remaining SMT-based side-channel exposure is within tolerance for their specific threat environment.",
                Tags = ["processor", "smt", "speculative", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ConfigureSMTForSecurity", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "ConfigureSMTForSecurity")],
                DetectOps = [RegOp.CheckDword(Key, "ConfigureSMTForSecurity", 1)],
            },
            new TweakDef
            {
                Id = "proccpol-enable-tsx-mitigations",
                Label = "Enable TSX Asynchronous Abort Mitigations",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "TSX Asynchronous Abort is an Intel CPU vulnerability where transactional synchronization extensions can leak data during transactional abort handling. Enabling TAA mitigations prevents exploitation of TSX Asynchronous Abort vulnerabilities through VERW instruction flushing of CPU buffers. TAA is closely related to MDS vulnerabilities and requires similar buffer-clearing mitigations. Systems with Intel TSX disabled through microcode updates are protected against TAA but TAA mitigations should also be enabled as defense-in-depth. The TAA mitigation VERW instruction overhead is minimal on processors that support the enhanced TAA mitigation capability. TAA mitigations do not affect functionality and should be enabled on all Intel processors with TSX capabilities.",
                Tags = ["processor", "taa", "tsx", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableTAAMitigations", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableTAAMitigations")],
                DetectOps = [RegOp.CheckDword(Key, "EnableTAAMitigations", 1)],
            },
            new TweakDef
            {
                Id = "proccpol-enable-ibrs",
                Label = "Enable Indirect Branch Restricted Speculation (IBRS)",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Indirect Branch Restricted Speculation prevents software running at lower privilege levels from influencing indirect branches in more privileged code. Enabling IBRS prevents user-mode code from poisoning indirect branch predictors used by kernel-mode code for Spectre variant 2 attacks. IBRS provides hardware-level mitigation when combined with appropriate processor microcode that supports the IBRS capability. Enhanced IBRS available in newer processors keeps IBRS active continuously with lower performance overhead than the original IBRS implementation. Retpoline provides an alternative Spectre variant 2 mitigation but IBRS provides hardware-based protection where Retpoline is not available. Both IBRS and Retpoline should be evaluated based on the processor generation present in the enterprise hardware fleet.",
                Tags = ["processor", "ibrs", "spectre", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableIBRS", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableIBRS")],
                DetectOps = [RegOp.CheckDword(Key, "EnableIBRS", 1)],
            },
            new TweakDef
            {
                Id = "proccpol-enable-ibpb",
                Label = "Enable Indirect Branch Predictor Barrier (IBPB)",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Indirect Branch Predictor Barrier flushes indirect branch predictor state when transitioning between different privilege levels or security contexts. Enabling IBPB ensures that predictions accumulated in one security context cannot influence code execution in a different security context. IBPB is particularly important at context switches between processes to prevent cross-process branch prediction poisoning. Without IBPB a malicious process can train the branch predictor before a context switch and influence speculative execution in the victim process. IBPB has some performance overhead at context switches but provides important cross-process isolation for Spectre variant 2. IBPB should be enabled on all processors that support the IBPB mechanism through microcode or architecture.",
                Tags = ["processor", "ibpb", "spectre", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableIBPB", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableIBPB")],
                DetectOps = [RegOp.CheckDword(Key, "EnableIBPB", 1)],
            },
            new TweakDef
            {
                Id = "proccpol-enable-stibp",
                Label = "Enable Single Thread Indirect Branch Predictors (STIBP)",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Single Thread Indirect Branch Predictors isolation prevents branch predictor sharing between sibling hyperthreads on SMT-enabled processors. Enabling STIBP ensures that branch state in one logical processor is isolated from its SMT sibling's branch prediction. Spectre cross-hyperthread attacks allow one logical processor to train the shared branch predictor and affect execution in the sibling thread. STIBP is essential for preventing cross-hyperthread Spectre variant 2 attacks on systems with SMT or Hyper-Threading enabled. The performance overhead of STIBP is process-context-dependent but modern Always-On STIBP implementations have reduced overhead. STIBP should be enabled on SMT-capable processors to prevent the hyperthread-based Spectre attack pathway.",
                Tags = ["processor", "stibp", "spectre", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableSTIBP", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableSTIBP")],
                DetectOps = [RegOp.CheckDword(Key, "EnableSTIBP", 1)],
            },
        ];

    }

    // ── SuperFetchSysmainPolicy ──
    private static class _SuperFetchSysmainPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SuperFetch";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "sfetch-disable-superfetch",
                Label = "Disable SuperFetch (SysMain) Service",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 4,
                Description =
                    "Sets EnableSuperfetch=0 in the SuperFetch policy key. Disables the SysMain "
                    + "service's predictive pre-loading of frequently used application binaries "
                    + "into RAM ahead of launch. On systems with NVMe SSDs, SuperFetch provides "
                    + "negligible benefit because cold-load times are already under 100 ms, "
                    + "while the service continuously writes prefetch metadata to disk and "
                    + "consumes a persistent memory allocation. "
                    + "Default: 3 (all modes). Recommended: 0 on SSD systems.",
                Tags = ["superfetch", "sysmain", "performance", "ssd", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableSuperfetch", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableSuperfetch")],
                DetectOps = [RegOp.CheckDword(Key, "EnableSuperfetch", 0)],
            },
            new TweakDef
            {
                Id = "sfetch-disable-prefetch",
                Label = "Disable Application Prefetch",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 4,
                Description =
                    "Sets EnablePrefetcher=0 in the SuperFetch policy key. Stops Windows from "
                    + "recording application launch traces in %SystemRoot%\\Prefetch and using "
                    + "them to pre-load executable pages before user invocation. On SSDs the "
                    + "prefetch metadata I/O adds unnecessary write amplification without "
                    + "meaningfully reducing startup latency. "
                    + "Default: 3. Recommended: 0 on NVMe/SSD systems.",
                Tags = ["prefetch", "performance", "ssd", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnablePrefetcher", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnablePrefetcher")],
                DetectOps = [RegOp.CheckDword(Key, "EnablePrefetcher", 0)],
            },
            new TweakDef
            {
                Id = "sfetch-disable-readyboost",
                Label = "Disable ReadyBoost",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
                Description =
                    "Sets EnableReadyboost=0 in the SuperFetch policy key. Prevents SysMain "
                    + "from using removable flash storage as a ReadyBoost cache. ReadyBoost "
                    + "was designed for systems with slow HDDs; on systems with SSDs it "
                    + "provides no performance benefit and writes extensively to the flash "
                    + "device, accelerating wear. "
                    + "Default: not set (enabled by policy probe). Recommended: 0.",
                Tags = ["readyboost", "flash", "usb", "performance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableReadyboost", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableReadyboost")],
                DetectOps = [RegOp.CheckDword(Key, "EnableReadyboost", 0)],
            },
            new TweakDef
            {
                Id = "sfetch-disable-readydrive",
                Label = "Disable ReadyDrive Hybrid HDD Cache",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
                Description =
                    "Sets EnableReadydrive=0 in the SuperFetch policy key. Disables ReadyDrive, "
                    + "the feature that uses the NAND cache on hybrid hard drives to speed up "
                    + "hibernation resume and boot. On systems using pure SSDs there are no "
                    + "hybrid drives and this policy has no effect, but disabling it prevents "
                    + "the SysMain driver from scanning for hybrid device capabilities on each "
                    + "boot. Default: not set. Recommended: 0.",
                Tags = ["readydrive", "hybrid", "hdd", "performance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableReadydrive", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableReadydrive")],
                DetectOps = [RegOp.CheckDword(Key, "EnableReadydrive", 0)],
            },
            new TweakDef
            {
                Id = "sfetch-disable-boot-trace",
                Label = "Disable Boot Trace for Prefetch",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
                Description =
                    "Sets EnableBootTrace=0 in the SuperFetch policy key. Stops SysMain from "
                    + "collecting an I/O trace during the boot sequence to optimise disk access "
                    + "order for subsequent boots. On SSDs random-access latency is sub-0.1 ms, "
                    + "rendering access-order optimisation meaningless; the trace itself adds "
                    + "kernel overhead during the boot sensitive period. "
                    + "Default: 1. Recommended: 0 on SSD systems.",
                Tags = ["boot", "trace", "prefetch", "performance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableBootTrace", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableBootTrace")],
                DetectOps = [RegOp.CheckDword(Key, "EnableBootTrace", 0)],
            },
            new TweakDef
            {
                Id = "sfetch-disable-app-launch-prefetch",
                Label = "Disable App-Launch Prefetch Optimisation",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 4,
                Description =
                    "Sets SuperFetchMaxSecsBeforeSuspend=0 in the SuperFetch policy key. "
                    + "Prevents SysMain from pre-fetching pages for applications that were "
                    + "recently suspended and are about to be re-activated. On SSDs the "
                    + "resume latency is already negligible, and this prefetch phase keeps "
                    + "RAM pages warm that could otherwise be used by actively running code. "
                    + "Default: 90 (seconds). Recommended: 0.",
                Tags = ["superfetch", "launch", "prefetch", "resume", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "SuperFetchMaxSecsBeforeSuspend", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "SuperFetchMaxSecsBeforeSuspend")],
                DetectOps = [RegOp.CheckDword(Key, "SuperFetchMaxSecsBeforeSuspend", 0)],
            },
            new TweakDef
            {
                Id = "sfetch-disable-logon-prefetch",
                Label = "Disable Logon Prefetch Scenario",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
                Description =
                    "Sets SuperFetchScenarioPolicyHibernate=0 in the SuperFetch policy key. "
                    + "Disables the post-logon SysMain scenario that pre-loads anticipated "
                    + "application pages immediately after the user desktop appears. On high-CPU "
                    + "machines this scenario races with startup applications, creating "
                    + "contention and increasing first-30-second CPU load. "
                    + "Default: 1. Recommended: 0.",
                Tags = ["superfetch", "logon", "scenario", "performance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "SuperFetchScenarioPolicyHibernate", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "SuperFetchScenarioPolicyHibernate")],
                DetectOps = [RegOp.CheckDword(Key, "SuperFetchScenarioPolicyHibernate", 0)],
            },
            new TweakDef
            {
                Id = "sfetch-disable-memory-profiling",
                Label = "Disable SysMain Memory Profiling",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
                Description =
                    "Sets SuperFetchMaxSampledPageAge=0 in the SuperFetch policy key. Prevents "
                    + "SysMain from maintaining a per-page age histogram that tracks how recently "
                    + "each physical memory page was accessed. This profiling data guides the "
                    + "pre-loading algorithm but requires SysMain to walk page-frame number "
                    + "tables on a recurring timer, adding kernel-mode overhead. "
                    + "Default: 7 (days). Recommended: 0.",
                Tags = ["superfetch", "memory", "profiling", "performance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "SuperFetchMaxSampledPageAge", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "SuperFetchMaxSampledPageAge")],
                DetectOps = [RegOp.CheckDword(Key, "SuperFetchMaxSampledPageAge", 0)],
            },
            new TweakDef
            {
                Id = "sfetch-disable-heap-prefetch",
                Label = "Disable Application Heap Prefetch",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
                Description =
                    "Sets SuperFetchDisableHeapDetect=1 in the SuperFetch policy key. Stops "
                    + "SysMain from recording heap-allocation patterns of active processes and "
                    + "using them to pre-warm heap segments ahead of growth allocations. Heap "
                    + "profiling involves introspecting target process address spaces via kernel "
                    + "callbacks, adding scheduling jitter on highly multi-threaded workloads. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["superfetch", "heap", "prefetch", "performance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "SuperFetchDisableHeapDetect", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "SuperFetchDisableHeapDetect")],
                DetectOps = [RegOp.CheckDword(Key, "SuperFetchDisableHeapDetect", 1)],
            },
            new TweakDef
            {
                Id = "sfetch-disable-telemetry",
                Label = "Disable SuperFetch Telemetry",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
                Description =
                    "Sets SuperFetchDisableTelemetry=1 in the SuperFetch policy key. Prevents "
                    + "SysMain from emitting ETW events and submitting memory-usage reports to "
                    + "the Windows feedback infrastructure. These reports include page-fault "
                    + "rates, working-set statistics, and popular application lists derived from "
                    + "all user accounts on the machine, which can profile usage patterns. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["superfetch", "telemetry", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "SuperFetchDisableTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "SuperFetchDisableTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "SuperFetchDisableTelemetry", 1)],
            },
        ];

    }

    // ── UsbStoragePolicy ──
    private static class _UsbStoragePolicy
    {
        private const string StoragePolicy = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\StorageDevicePolicies";
        private const string RemovableDevices = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices";
        private const string UsbFloppyClass =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f56307-b6bf-11d0-94f2-00a0c91efb8b}";
        private const string CdRomClass =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f56308-b6bf-11d0-94f2-00a0c91efb8b}";
        private const string TapeClass =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f5630b-b6bf-11d0-94f2-00a0c91efb8b}";
        private const string WpdClass =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{6AC27878-A6FA-4155-BA85-F98F491D4F33}";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "usbstor-write-protect",
                Label = "USB Storage: Enable Hardware Write-Protection on All Removable Drives",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [StoragePolicy],
                Tags = ["usb", "storage", "write-protect", "dlp", "security"],
                Description =
                    "Sets WriteProtect=1 in StorageDevicePolicies. Blocks all write operations to removable "
                    + "storage devices (USB drives, SD cards) at the OS level. "
                    + "Prevents data exfiltration via portable drives. Default: read/write. Recommended for kiosk/corporate.",
                ApplyOps = [RegOp.SetDword(StoragePolicy, "WriteProtect", 1)],
                RemoveOps = [RegOp.SetDword(StoragePolicy, "WriteProtect", 0)],
                DetectOps = [RegOp.CheckDword(StoragePolicy, "WriteProtect", 1)],
            },
            new TweakDef
            {
                Id = "usbstor-deny-all-removable-write",
                Label = "USB Storage: Deny Write Access to All Removable Storage Classes (GPO)",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [RemovableDevices],
                Tags = ["usb", "storage", "gpo", "write", "dlp", "security"],
                Description =
                    "Sets Deny_Write=1 in the RemovableStorageDevices class policy. Blocks write access to all "
                    + "removable storage device classes via Group Policy. Complements the hardware WriteProtect flag. "
                    + "Default: write allowed. Recommended for data loss prevention.",
                ApplyOps = [RegOp.SetDword(RemovableDevices, "Deny_Write", 1)],
                RemoveOps = [RegOp.DeleteValue(RemovableDevices, "Deny_Write")],
                DetectOps = [RegOp.CheckDword(RemovableDevices, "Deny_Write", 1)],
            },
            new TweakDef
            {
                Id = "usbstor-deny-all-removable-execute",
                Label = "USB Storage: Deny Execution from All Removable Storage Classes (GPO)",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [RemovableDevices],
                Tags = ["usb", "storage", "gpo", "execute", "autorun", "security"],
                Description =
                    "Sets Deny_Execute=1 in the RemovableStorageDevices class policy. Prevents launching "
                    + "executables directly from any removable storage device. E.g., blocks BadUSB payloads. "
                    + "Default: execution allowed. Recommended to block portable malware.",
                ApplyOps = [RegOp.SetDword(RemovableDevices, "Deny_Execute", 1)],
                RemoveOps = [RegOp.DeleteValue(RemovableDevices, "Deny_Execute")],
                DetectOps = [RegOp.CheckDword(RemovableDevices, "Deny_Execute", 1)],
            },
            new TweakDef
            {
                Id = "usbstor-deny-all-removable-read",
                Label = "USB Storage: Deny Read Access to All Removable Storage (Strict Lockdown)",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [RemovableDevices],
                Tags = ["usb", "storage", "gpo", "lockdown", "high-security"],
                Description =
                    "Sets Deny_Read=1 in the RemovableStorageDevices class policy. Blocks read access to all "
                    + "removable storage devices. Extreme lockdown for air-gapped or high-security environments. "
                    + "Default: read allowed. WARNING: prevents legitimate USB storage use.",
                ApplyOps = [RegOp.SetDword(RemovableDevices, "Deny_Read", 1)],
                RemoveOps = [RegOp.DeleteValue(RemovableDevices, "Deny_Read")],
                DetectOps = [RegOp.CheckDword(RemovableDevices, "Deny_Read", 1)],
            },
            new TweakDef
            {
                Id = "usbstor-deny-usb-disk-write",
                Label = "USB Storage: Deny Write Access to USB Disk Drives (Class Policy)",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [UsbFloppyClass],
                Tags = ["usb", "storage", "disk", "write", "dlp", "class-driver"],
                Description =
                    "Sets Deny_Write=1 in the USB disk drive device class GUID policy. "
                    + "Targets the removable disk class specifically (includes USB flash, external HDD). "
                    + "Default: write allowed. More targeted than the all-classes RemovableDevices key.",
                ApplyOps = [RegOp.SetDword(UsbFloppyClass, "Deny_Write", 1)],
                RemoveOps = [RegOp.DeleteValue(UsbFloppyClass, "Deny_Write")],
                DetectOps = [RegOp.CheckDword(UsbFloppyClass, "Deny_Write", 1)],
            },
            new TweakDef
            {
                Id = "usbstor-deny-cdrom-write",
                Label = "USB Storage: Deny Write Access to Optical/CD-ROM Drives",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [CdRomClass],
                Tags = ["usb", "cdrom", "optical", "write", "dlp", "security"],
                Description =
                    "Sets Deny_Write=1 in the CD-ROM / optical drive device class GUID policy. "
                    + "Prevents disc burning (ISO, data CD/DVD). Blocks data exfiltration via optical media. "
                    + "Default: write allowed. Effective for blocking burnable media.",
                ApplyOps = [RegOp.SetDword(CdRomClass, "Deny_Write", 1)],
                RemoveOps = [RegOp.DeleteValue(CdRomClass, "Deny_Write")],
                DetectOps = [RegOp.CheckDword(CdRomClass, "Deny_Write", 1)],
            },
            new TweakDef
            {
                Id = "usbstor-deny-cdrom-read",
                Label = "USB Storage: Deny Read Access to optical drives",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [CdRomClass],
                Tags = ["usb", "cdrom", "optical", "read", "lockdown"],
                Description =
                    "Sets Deny_Read=1 in the CD-ROM device class GUID policy. "
                    + "Blocks mounting and reading optical discs. "
                    + "Intended for high-security environments where disc insertion is prohibited.",
                ApplyOps = [RegOp.SetDword(CdRomClass, "Deny_Read", 1)],
                RemoveOps = [RegOp.DeleteValue(CdRomClass, "Deny_Read")],
                DetectOps = [RegOp.CheckDword(CdRomClass, "Deny_Read", 1)],
            },
            new TweakDef
            {
                Id = "usbstor-deny-tape-write",
                Label = "USB Storage: Deny Write Access to Tape Drives",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [TapeClass],
                Tags = ["usb", "tape", "backup", "write", "dlp", "security"],
                Description =
                    "Sets Deny_Write=1 in the tape drive device class GUID policy. "
                    + "Prevents writing to tape backup units from non-authorized users. "
                    + "Default: tape writes allowed. Recommended for environments with tape backup controls.",
                ApplyOps = [RegOp.SetDword(TapeClass, "Deny_Write", 1)],
                RemoveOps = [RegOp.DeleteValue(TapeClass, "Deny_Write")],
                DetectOps = [RegOp.CheckDword(TapeClass, "Deny_Write", 1)],
            },
            new TweakDef
            {
                Id = "usbstor-deny-wpd-write",
                Label = "USB Storage: Deny Write Access to WPD (MTP/PTP) Portable Devices",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [WpdClass],
                Tags = ["usb", "wpd", "mtp", "ptp", "phone", "write", "dlp"],
                Description =
                    "Sets Deny_Write=1 in the WPD (Windows Portable Devices) class GUID policy. "
                    + "Prevents file transfers to phones, cameras, and MTP/PTP devices connected via USB. "
                    + "Default: write allowed. Blocks data transfer to smartphones and cameras.",
                ApplyOps = [RegOp.SetDword(WpdClass, "Deny_Write", 1)],
                RemoveOps = [RegOp.DeleteValue(WpdClass, "Deny_Write")],
                DetectOps = [RegOp.CheckDword(WpdClass, "Deny_Write", 1)],
            },
            new TweakDef
            {
                Id = "usbstor-deny-wpd-read",
                Label = "USB Storage: Deny Read Access to WPD (MTP/PTP) Portable Devices",
                Category = "Device & Hardware Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [WpdClass],
                Tags = ["usb", "wpd", "mtp", "ptp", "phone", "read", "lockdown"],
                Description =
                    "Sets Deny_Read=1 in the WPD device class GUID policy. "
                    + "Prevents reading files from phones, cameras, and MTP/PTP devices. "
                    + "Extreme DLP measure to prevent any data exchange with portable consumer devices.",
                ApplyOps = [RegOp.SetDword(WpdClass, "Deny_Read", 1)],
                RemoveOps = [RegOp.DeleteValue(WpdClass, "Deny_Read")],
                DetectOps = [RegOp.CheckDword(WpdClass, "Deny_Read", 1)],
            },
        ];

    }

    // ── VirtualDiskServicePolicy ──
    private static class _VirtualDiskServicePolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DiskManagement";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "vdspol-block-vhd-mount",
                    Label = "Block Standard Users from Mounting VHD/VHDX Files",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Prevents standard (non-admin) users from attaching or mounting Virtual Hard Disk (VHD/VHDX) files, closing the data-exfiltration path of creating an encrypted virtual disk and filling it with sensitive data.",
                    Tags = ["vhd", "virtual-disk", "mount", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "VHD/VHDX mounting restricted to administrators; standard users cannot attach virtual disk files.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowVHDMount", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowVHDMount")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowVHDMount", 0)],
                },
                new TweakDef
                {
                    Id = "vdspol-block-iso-mount",
                    Label = "Block Standard Users from Mounting ISO/IMG Files",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Prevents standard users from mounting ISO, IMG, and other optical disc image files via the Explorer 'Mount' context menu, restricting virtual drive creation to administrators.",
                    Tags = ["iso", "virtual-drive", "mount", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "ISO/IMG mounting restricted to admins; standard users cannot browse or execute content from disc images.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowISOMount", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowISOMount")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowISOMount", 0)],
                },
                new TweakDef
                {
                    Id = "vdspol-disable-disk-management-snap-in",
                    Label = "Disable Disk Management Snap-In for Standard Users",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Blocks the Disk Management MMC snap-in (diskmgmt.msc) for non-administrator accounts, preventing standard users from viewing, partitioning, formatting, or managing physical and virtual disks.",
                    Tags = ["disk-management", "mmc", "standard-user", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Disk Management blocked for standard users; partitioning and disk operations require admin elevation.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableDiskManagementSnapIn", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableDiskManagementSnapIn")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableDiskManagementSnapIn", 1)],
                },
                new TweakDef
                {
                    Id = "vdspol-require-admin-for-partition",
                    Label = "Require Admin for Disk Partitioning Operations",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Enforces that all disk partitioning operations (create, delete, resize partition) require administrator privileges, preventing accidental or malicious disk modification by standard users.",
                    Tags = ["disk-management", "partition", "admin", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Partitioning operations require admin rights; standard users cannot modify partition layout.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireAdminForPartitioning", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireAdminForPartitioning")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireAdminForPartitioning", 1)],
                },
                new TweakDef
                {
                    Id = "vdspol-disable-removable-format",
                    Label = "Disable Formatting of Removable Drives by Standard Users",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Prevents standard users from formatting removable drives (USB drives, SD cards, external HDDs) through Explorer or Disk Management, avoiding irreversible data loss by users without sufficient knowledge.",
                    Tags = ["disk-management", "format", "removable", "standard-user", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Removable media formatting restricted to admins; standard users cannot format USB drives.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableRemovableMediaFormat", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableRemovableMediaFormat")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableRemovableMediaFormat", 1)],
                },
                new TweakDef
                {
                    Id = "vdspol-log-vhd-attach-events",
                    Label = "Enable Audit Logging for VHD/VHDX Attach and Detach Events",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Enables event log entries for every VHD/VHDX mount and unmount operation, recording the file path and user account responsible for each attachment.",
                    Tags = ["vhd", "audit-log", "virtual-disk", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "VHD/VHDX attach/detach events logged; virtual disk mount activity is auditable.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditVHDMountEvents", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditVHDMountEvents")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditVHDMountEvents", 1)],
                },
                new TweakDef
                {
                    Id = "vdspol-disable-dynamic-disk",
                    Label = "Disable Dynamic Disk Conversions",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Prevents conversion of basic disks to dynamic disk format, blocking the creation of spanned, striped, or mirrored volumes via Windows dynamic disk — recommending Storage Spaces instead for resilient configurations.",
                    Tags = ["disk-management", "dynamic-disk", "conversion", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Dynamic disk conversion disabled; basic disks cannot be upgraded to dynamic. Use Storage Spaces for resilience.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableDynamicDiskConversion", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableDynamicDiskConversion")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableDynamicDiskConversion", 1)],
                },
                new TweakDef
                {
                    Id = "vdspol-block-auto-initialize-disk",
                    Label = "Block Automatic Disk Initialisation on New Disk Detection",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Prevents Windows from automatically opening the Initialize Disk wizard when a new uninitialized disk is detected, requiring an administrator to manually initiate disk initialisation.",
                    Tags = ["disk-management", "auto-initialize", "new-disk", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Automatic disk initialisation wizard suppressed; admins must manually initialise new disks.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockAutoInitializeDisk", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockAutoInitializeDisk")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockAutoInitializeDisk", 1)],
                },
                new TweakDef
                {
                    Id = "vdspol-require-bitlocker-for-external",
                    Label = "Require BitLocker Encryption Before External Drive Writability",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Requires that external or removable drives be encrypted with BitLocker To Go before allowing write access, preventing unencrypted exfiltration of sensitive data to external media.",
                    Tags = ["disk-management", "bitlocker", "external-drive", "encryption", "dlp", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "External drives require BitLocker encryption to become writable; unencrypted drives are read-only.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireBitLockerForExternalWritable", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireBitLockerForExternalWritable")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireBitLockerForExternalWritable", 1)],
                },
                new TweakDef
                {
                    Id = "vdspol-disable-wps-disk-provision",
                    Label = "Disable Windows Provisioning Service Disk Auto-Provision",
                    Category = "Device & Hardware Policy",
                    Description =
                        "Disables the Windows Provisioning Service automatic disk provisioning feature that configures disk topology on first boot, ensuring that enterprise imaging tools retain full control over disk layout.",
                    Tags = ["disk-management", "provisioning", "auto-provision", "enterprise", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Windows Provisioning disk auto-provision disabled; disk layout managed by enterprise imaging tools.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableAutoProvisionDisk", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoProvisionDisk")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableAutoProvisionDisk", 1)],
                },
            ];

    }

}
