namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// ── merged from PolicyDevice.cs ──
// RegiLattice.Core — Tweaks/PolicyDevice.cs
// Device installation, enrollment, guard, firmware, hardware, portable devices, USB storage, and kernel DMA protection policies
// Category: "Device & Hardware Policy"
// Consolidated from 23 modules.

internal static partial class PolicyDevice
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
                Category = "Peripherals — Bluetooth Adv",
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
                Category = "Peripherals — Bluetooth Adv",
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
                Category = "Peripherals — Bluetooth Adv",
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
                Category = "Peripherals — Bluetooth Adv",
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
                Category = "Peripherals — Bluetooth Adv",
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
                Category = "Peripherals — Bluetooth Adv",
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
                Category = "Peripherals — Bluetooth Adv",
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
                Category = "Peripherals — Bluetooth Adv",
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
                Category = "Peripherals — Bluetooth Adv",
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
                Category = "Peripherals — Bluetooth Adv",
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
        private const string DhaKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceHealthAttestation";

        private const string HcKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HealthCenter";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "devcpl-enable-health-attestation",
                    Label = "Device Compliance: Enable Device Health Attestation",
                    Category = "Peripherals — Bluetooth Adv",
                    Description =
                        "Sets EnableHealthAttestation=1 in DeviceHealthAttestation policy. Enables the Windows Device Health Attestation (DHA) service which uses the device's TPM to cryptographically attest its boot sequence. The DHA service generates a health certificate that can be consumed by MDM providers (Intune, SCCM) and conditional access systems to verify that the device booted without tampering: Secure Boot was enabled, BitLocker is active, the boot path was not modified, and no ELAM-detected malware was present. Without DHA, conditional access can only rely on OS-reported state — which malware can spoof.",
                    Tags = ["dha", "health-attestation", "tpm", "conditional-access", "boot-integrity"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Device boot integrity is cryptographically attested using the TPM. Requires TPM 2.0. Health certificates are generated and periodically sent to the configured DHA server. Enables hardware-backed conditional access decisions.",
                    ApplyOps = [RegOp.SetDword(DhaKey, "EnableHealthAttestation", 1)],
                    RemoveOps = [RegOp.DeleteValue(DhaKey, "EnableHealthAttestation")],
                    DetectOps = [RegOp.CheckDword(DhaKey, "EnableHealthAttestation", 1)],
                },
                new TweakDef
                {
                    Id = "devcpl-require-bitlocker-for-compliance",
                    Label = "Device Compliance: Require BitLocker Encryption for Compliance",
                    Category = "Peripherals — Bluetooth Adv",
                    Description =
                        "Sets RequireBitLockerForCompliance=1 in HealthCenter policy. Marks a device as non-compliant if BitLocker Drive Encryption is not enabled on the system drive. Compliance status is reported to MDM (Intune/SCCM) and can trigger conditional access policies that block the device from connecting to corporate resources until BitLocker is enabled. Data loss from stolen or lost unencrypted laptops is one of the most common sources of data breaches. Requiring BitLocker for compliance ensures all mobile devices connecting to corporate resources are encrypted.",
                    Tags = ["compliance", "bitlocker", "encryption", "conditional-access", "data-protection"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Devices without BitLocker on the system drive report as non-compliant. Non-compliant devices may be blocked from corporate resources via conditional access. Requires MDM enrolment and conditional access policies to enforce the compliance gate.",
                    ApplyOps = [RegOp.SetDword(HcKey, "RequireBitLockerForCompliance", 1)],
                    RemoveOps = [RegOp.DeleteValue(HcKey, "RequireBitLockerForCompliance")],
                    DetectOps = [RegOp.CheckDword(HcKey, "RequireBitLockerForCompliance", 1)],
                },
                new TweakDef
                {
                    Id = "devcpl-require-antivirus-for-compliance",
                    Label = "Device Compliance: Require Active Antivirus for Compliance",
                    Category = "Peripherals — Bluetooth Adv",
                    Description =
                        "Sets RequireAntivirusForCompliance=1 in HealthCenter policy. Marks a device as non-compliant if a registered and up-to-date antivirus product is not detected by the Security Center. Real-time protection must be active and signatures cannot be critically outdated. Devices that have disabled antivirus, have expired protection subscriptions, or have antivirus that is consuming no CPU (indicative of process termination by malware) are flagged. Security Center status is checked periodically and on every MDM compliance check cycle.",
                    Tags = ["compliance", "antivirus", "security-center", "real-time-protection", "endpoint"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Devices without active, up-to-date antivirus report as non-compliant. Devices with disabled or expired AV may lose access to corporate resources. Requires MDM and conditional access to enforce. Windows Defender Antivirus or any ELAM-registered product satisfies the requirement.",
                    ApplyOps = [RegOp.SetDword(HcKey, "RequireAntivirusForCompliance", 1)],
                    RemoveOps = [RegOp.DeleteValue(HcKey, "RequireAntivirusForCompliance")],
                    DetectOps = [RegOp.CheckDword(HcKey, "RequireAntivirusForCompliance", 1)],
                },
                new TweakDef
                {
                    Id = "devcpl-set-compliance-check-interval-4h",
                    Label = "Device Compliance: Set Compliance Check Interval to 4 Hours",
                    Category = "Peripherals — Bluetooth Adv",
                    Description =
                        "Sets ComplianceCheckIntervalHours=4 in HealthCenter policy. Sets the interval at which Windows re-evaluates device compliance state and sends the current status to the MDM provider. A default compliance check interval that is too long (24+ hours) means a device that becomes non-compliant (user disables BitLocker, AV signs expire, firewall turned off) continues to access corporate resources for up to a day before its compliance status is updated. 4 hours ensures compliance violations are detected and reflected in conditional access within the business day after they occur.",
                    Tags = ["compliance", "check-interval", "mdm", "detection", "conditional-access"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Compliance state is evaluated every 4 hours. A device that becomes non-compliant is detected within 4 hours. Slightly higher MDM service check-in frequency — negligible network overhead.",
                    ApplyOps = [RegOp.SetDword(HcKey, "ComplianceCheckIntervalHours", 4)],
                    RemoveOps = [RegOp.DeleteValue(HcKey, "ComplianceCheckIntervalHours")],
                    DetectOps = [RegOp.CheckDword(HcKey, "ComplianceCheckIntervalHours", 4)],
                },
                new TweakDef
                {
                    Id = "devcpl-require-secure-boot-for-compliance",
                    Label = "Device Compliance: Require Secure Boot for Compliance",
                    Category = "Peripherals — Bluetooth Adv",
                    Description =
                        "Sets RequireSecureBootForCompliance=1 in HealthCenter policy. Marks a device as non-compliant if UEFI Secure Boot is not enabled. Secure Boot prevents bootkit malware and rootkits from replacing the boot path with untrusted code — without Secure Boot, an attacker with brief physical access can boot from a USB drive to bypass Windows authentication or install a persistent bootkit. Devices with Secure Boot disabled cannot be trusted to run an uncompromised OS. This check complements DHA attestation with a policy-layer enforcement.",
                    Tags = ["compliance", "secure-boot", "uefi", "bootkit", "physical-security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Devices without Secure Boot enabled report as non-compliant. Very old hardware (pre-2012) may not support Secure Boot. Devices that were deliberately configured without Secure Boot for BIOS compatibility reasons must be re-evaluated.",
                    ApplyOps = [RegOp.SetDword(HcKey, "RequireSecureBootForCompliance", 1)],
                    RemoveOps = [RegOp.DeleteValue(HcKey, "RequireSecureBootForCompliance")],
                    DetectOps = [RegOp.CheckDword(HcKey, "RequireSecureBootForCompliance", 1)],
                },
                new TweakDef
                {
                    Id = "devcpl-enable-compliance-grace-period-7days",
                    Label = "Device Compliance: Enable 7-Day Grace Period for Non-Compliant Devices",
                    Category = "Peripherals — Bluetooth Adv",
                    Description =
                        "Sets ComplianceGracePeriodDays=7 in HealthCenter policy. Grants newly enrolled devices or devices that first become non-compliant a 7-day grace period before conditional access blocks are enforced. Without a grace period, a device that enrolls in MDM but has not yet completed all compliance remediation (BitLocker encrypting, definitions updating) is immediately blocked from corporate resources — creating a chicken-and-egg problem. The grace period allows IT to remediate the device before it loses access. After 7 days without remediation, access restrictions are enforced.",
                    Tags = ["compliance", "grace-period", "enrolment", "remediation", "mdm"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Non-compliant devices have 7 days to reach compliance before access restrictions are applied. Provides IT time for remediation without disrupting new enrolments. After 7 days, non-compliant devices are subject to conditional access blocks.",
                    ApplyOps = [RegOp.SetDword(HcKey, "ComplianceGracePeriodDays", 7)],
                    RemoveOps = [RegOp.DeleteValue(HcKey, "ComplianceGracePeriodDays")],
                    DetectOps = [RegOp.CheckDword(HcKey, "ComplianceGracePeriodDays", 7)],
                },
                new TweakDef
                {
                    Id = "devcpl-require-minimum-os-build",
                    Label = "Device Compliance: Require Minimum OS Build for Compliance",
                    Category = "Peripherals — Bluetooth Adv",
                    Description =
                        "Sets RequireMinimumOsBuild=1 in HealthCenter policy. Enables minimum OS build checking as a compliance criterion. When enabled, devices running OS builds older than the configured minimum (set separately as MinimumBuildNumber) report as non-compliant. This policy ensures that devices running versions of Windows that are out of Microsoft's support cycle (no security patches) or that have known unpatched critical vulnerabilities are flagged before they access corporate resources. Combined with Windows Update policies, this creates an enforced minimum security baseline.",
                    Tags = ["compliance", "os-build", "patch-level", "security-baseline", "outdated-os"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Devices below the minimum OS build report as non-compliant. Requires configuring MinimumBuildNumber separately. Devices on unsupported or unpatched OS build are blocked pending upgrade. Coordinate with Windows Update deadline policies.",
                    ApplyOps = [RegOp.SetDword(HcKey, "RequireMinimumOsBuild", 1)],
                    RemoveOps = [RegOp.DeleteValue(HcKey, "RequireMinimumOsBuild")],
                    DetectOps = [RegOp.CheckDword(HcKey, "RequireMinimumOsBuild", 1)],
                },
                new TweakDef
                {
                    Id = "devcpl-require-firewall-for-compliance",
                    Label = "Device Compliance: Require Windows Firewall Active for Compliance",
                    Category = "Peripherals — Bluetooth Adv",
                    Description =
                        "Sets RequireFirewallForCompliance=1 in HealthCenter policy. Marks a device as non-compliant if Windows Defender Firewall (or a registered third-party firewall) is not active on all network profiles (domain, private, public). The Windows Firewall is a critical network-based attack prevention control. Users may disable the firewall when troubleshooting connection issues and forget to re-enable it. A device with no host firewall on a public network is exposed to direct network attacks. This compliance check ensures firewalls stay active.",
                    Tags = ["compliance", "firewall", "network-protection", "security-center", "perimeter"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Devices with disabled Windows Defender Firewall or no registered firewall are non-compliant. Third-party firewalls registered with Security Center satisfy the requirement. Devices that turned off the firewall for temporary diagnostics and forgot to restore will be flagged.",
                    ApplyOps = [RegOp.SetDword(HcKey, "RequireFirewallForCompliance", 1)],
                    RemoveOps = [RegOp.DeleteValue(HcKey, "RequireFirewallForCompliance")],
                    DetectOps = [RegOp.CheckDword(HcKey, "RequireFirewallForCompliance", 1)],
                },
                new TweakDef
                {
                    Id = "devcpl-block-noncompliant-resource-access",
                    Label = "Device Compliance: Block Non-Compliant Devices from Joining AD Resources",
                    Category = "Peripherals — Bluetooth Adv",
                    Description =
                        "Sets BlockNonCompliantNetworkAccess=1 in HealthCenter policy. Enables a local enforcement hook that checks compliance state before allowing the device to connect to protected network resources. When this is enabled and the device is marked non-compliant by the health centre, outbound connections to domain-classified resources can be blocked at the Windows Filtering Platform (WFP) layer. This provides local enforcement independent of whether external conditional access (AAD, MFA, proxy) is in place — useful as defence-in-depth for environments where some legacy resources lack conditional access support.",
                    Tags = ["compliance", "network-access", "block", "conditional-access", "wfp"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote =
                        "Non-compliant devices are blocked from accessing domain network resources at the WFP layer. This is a local enforcement on the device itself — not a network-layer block. A device misidentifying its compliance state may block its own legitimate access. Test thoroughly before broad deployment.",
                    ApplyOps = [RegOp.SetDword(HcKey, "BlockNonCompliantNetworkAccess", 1)],
                    RemoveOps = [RegOp.DeleteValue(HcKey, "BlockNonCompliantNetworkAccess")],
                    DetectOps = [RegOp.CheckDword(HcKey, "BlockNonCompliantNetworkAccess", 1)],
                },
                new TweakDef
                {
                    Id = "devcpl-enable-tpm-attestation-logging",
                    Label = "Device Compliance: Enable TPM Health Attestation Event Logging",
                    Category = "Peripherals — Bluetooth Adv",
                    Description =
                        "Sets TpmAttestationLogging=1 in DeviceHealthAttestation policy. Enables event log entries for TPM health attestation operations: TPM measurement capture, health certificate request, health certificate delivery, and health attestation failures. Without attestation logging, diagnosing why a device cannot obtain a health certificate (TPM in reduced functionality mode, endorsement key provisioning failure, attestation service unreachable) is difficult. Log entries enable IT helpdesk to diagnose attestation failures and restore compliance without escalating to infrastructure teams.",
                    Tags = ["tpm", "attestation", "logging", "compliance", "diagnostics"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "TPM attestation events are logged. Events include certificate request, success, failure, and failure reasons. Negligible disk overhead. Enables rapid helpdesk diagnosis of attestation failures without advanced tooling.",
                    ApplyOps = [RegOp.SetDword(DhaKey, "TpmAttestationLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(DhaKey, "TpmAttestationLogging")],
                    DetectOps = [RegOp.CheckDword(DhaKey, "TpmAttestationLogging", 1)],
                },
            ];
    }

    // ── DeviceEnrollmentLimitPolicy ──
    private static class _DeviceEnrollmentLimitPolicy
    {
        private const string EnlKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceEnrollment";

        private const string MdmKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\MDM";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "devenl-set-max-devices-per-user-5",
                    Label = "Device Enrollment Limit: Set Maximum Devices per User to 5",
                    Category = "Peripherals — Bluetooth Adv",
                    Description =
                        "Sets MaxDevicesPerUser=5 in DeviceEnrollment policy. Limits the number of devices a single user account can enroll in MDM to 5. Without per-user limits, a single compromised account can be used to enroll large numbers of devices into the MDM tenant, consuming Intune licenses, polluting the device inventory, and potentially using the MDM service to push malware to enrolled devices. A limit of 5 is generous enough for users with a phone, tablet, laptop, home PC, and a spare device, while preventing bulk enrollment abuse.",
                    Tags = ["enrollment", "device-limit", "per-user", "abuse-prevention", "inventory"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Each user can enroll a maximum of 5 devices. Attempts to enroll a 6th device are rejected until an existing device is unenrolled. Adjust the limit if your organisation has users with more than 5 managed devices (e.g., kiosk operators managing multiple shared devices with a single service account).",
                    ApplyOps = [RegOp.SetDword(EnlKey, "MaxDevicesPerUser", 5)],
                    RemoveOps = [RegOp.DeleteValue(EnlKey, "MaxDevicesPerUser")],
                    DetectOps = [RegOp.CheckDword(EnlKey, "MaxDevicesPerUser", 5)],
                },
                new TweakDef
                {
                    Id = "devenl-block-byod-personal-enrollment",
                    Label = "Device Enrollment Limit: Block Personal BYOD Devices from Enrolling",
                    Category = "Peripherals — Bluetooth Adv",
                    Description =
                        "Sets BlockPersonalDeviceEnrollment=1 in DeviceEnrollment policy. Prevents devices that are registered as personal devices (not Azure AD Joined or Hybrid Joined) from enrolling in corporate MDM. A personally-owned device that enrolls in corporate MDM becomes subject to remote wipe commands — which could irreversibly delete personal data. Blocking personal device enrollment prevents accidental enrollment of personal hardware into MDM while protecting users' personal devices from corporate management actions. Users who need BYOD access should use Workplace Join with limited MDM (MAM without device enrollment) instead.",
                    Tags = ["byod", "personal-device", "enrollment-block", "remote-wipe-protection", "privacy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Personal (non-AAD-Joined) devices cannot enroll in corporate MDM. Users who attempt to add a work account on a personal device get a generic failure. BYOD users should sign in with MAM-only (app-level management) via Outlook or Teams apps instead. Requires AAD Join for full MDM enrollment.",
                    ApplyOps = [RegOp.SetDword(EnlKey, "BlockPersonalDeviceEnrollment", 1)],
                    RemoveOps = [RegOp.DeleteValue(EnlKey, "BlockPersonalDeviceEnrollment")],
                    DetectOps = [RegOp.CheckDword(EnlKey, "BlockPersonalDeviceEnrollment", 1)],
                },
                new TweakDef
                {
                    Id = "devenl-require-device-category-on-enroll",
                    Label = "Device Enrollment Limit: Require Device Category Assignment at Enrollment",
                    Category = "Peripherals — Bluetooth Adv",
                    Description =
                        "Sets RequireDeviceCategoryOnEnrollment=1 in DeviceEnrollment policy. Requires administrators to assign a device category (e.g., Corporate Laptop, Kiosk, Shared Workstation) at enrollment time. Device categories in Intune are used to automatically assign devices to dynamic groups, which in turn receive different policy sets. Without mandatory category assignment, all devices land in the uncategorised default group and receive a single policy set. Mandatory categories ensure that kiosk devices, shared workstations, and executive laptops each receive appropriately scoped policies from the moment of enrollment.",
                    Tags = ["enrollment", "device-category", "dynamic-group", "policy-scoping", "intune"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Device category must be assigned before enrollment completes. Enrollment fails if no category is selected. Category assignment is performed by the enrolling admin or in automated flows by the Autopilot assignment group. No user-facing UI change for standard users.",
                    ApplyOps = [RegOp.SetDword(EnlKey, "RequireDeviceCategoryOnEnrollment", 1)],
                    RemoveOps = [RegOp.DeleteValue(EnlKey, "RequireDeviceCategoryOnEnrollment")],
                    DetectOps = [RegOp.CheckDword(EnlKey, "RequireDeviceCategoryOnEnrollment", 1)],
                },
                new TweakDef
                {
                    Id = "devenl-block-unused-enrollment-profiles",
                    Label = "Device Enrollment Limit: Block Devices Without an Enrollment Profile",
                    Category = "Peripherals — Bluetooth Adv",
                    Description =
                        "Sets RequireEnrollmentProfile=1 in DeviceEnrollment policy. Prevents devices from enrolling unless they match a pre-configured Intune enrollment profile (Device Enrollment Program, Autopilot profile, or bulk enrollment token). Without this restriction, any device that has credentials for a licensed user can self-enroll in MDM using the standard Settings > Accounts flow. Pre-requiring an enrollment profile means that only devices that IT has explicitly authorized for enrollment (by creating or assigning a profile) can join MDM — unknown or unauthorized devices are rejected.",
                    Tags = ["enrollment", "enrollment-profile", "autopilot", "authorization", "unknown-devices"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Only devices that match a pre-configured enrollment profile can enroll. Devices without a matching profile are rejected at enrollment. Devices must be registered in Intune/Autopilot before attempting enrollment. Prevents rogue devices from enrolling with valid user credentials.",
                    ApplyOps = [RegOp.SetDword(EnlKey, "RequireEnrollmentProfile", 1)],
                    RemoveOps = [RegOp.DeleteValue(EnlKey, "RequireEnrollmentProfile")],
                    DetectOps = [RegOp.CheckDword(EnlKey, "RequireEnrollmentProfile", 1)],
                },
                new TweakDef
                {
                    Id = "devenl-restrict-enrollment-to-aad-join",
                    Label = "Device Enrollment Limit: Restrict MDM Enrollment to AAD Joined Devices Only",
                    Category = "Peripherals — Bluetooth Adv",
                    Description =
                        "Sets RestrictEnrollmentToAadJoin=1 in MDM policy. Prevents MDM enrollment from completing unless the device is Azure AD Joined (not just Workplace Joined). Workplace Join provides a limited form of registration that does not require the device to be AAD-joined — this allows personal devices to register without a full AAD Join. By restricting enrollment to AAD Join, this policy ensures that enrolled devices are fully registered in Azure AD with a machine account, which is required for Hybrid Join, Conditional Access device trust, and all domain-level group policies backed by AAD.",
                    Tags = ["enrollment", "aad-join", "device-trust", "conditional-access", "hybrid-join"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Enrollment is restricted to devices that complete Azure AD Join. Workplace Join-only devices cannot enroll. Hybrid Joined devices (on-premises AD + AAD) satisfy the AAD Join requirement. Purely on-premises AD-joined devices without AAD sync must Hybrid-Join before they can enroll.",
                    ApplyOps = [RegOp.SetDword(MdmKey, "RestrictEnrollmentToAadJoin", 1)],
                    RemoveOps = [RegOp.DeleteValue(MdmKey, "RestrictEnrollmentToAadJoin")],
                    DetectOps = [RegOp.CheckDword(MdmKey, "RestrictEnrollmentToAadJoin", 1)],
                },
                new TweakDef
                {
                    Id = "devenl-enable-enrollment-status-page",
                    Label = "Device Enrollment Limit: Enable MDM Enrollment Status Page During OOBE",
                    Category = "Peripherals — Bluetooth Adv",
                    Description =
                        "Sets ShowEnrollmentStatusPage=1 in DeviceEnrollment policy. Enables the Enrollment Status Page (ESP) during Autopilot or standard OOBE enrollment. The ESP shows the user (and IT) the real-time progress of device setup: account provisioning, app installations, policy applications, and certificate enrollments. Without the ESP, the user is deposited at the desktop while apps are still installing or policies are still applying — the device may appear functional but actually be in an incomplete configuration state. The ESP holds the user at the setup screen until all critical configurations are complete.",
                    Tags = ["enrollment", "esp", "oobe", "autopilot", "setup-progress"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Enrollment Status Page is shown during Autopilot/OOBE. Users are blocked at the ESP until all required apps and policies are applied. Prevents users from using a partially-configured device. Increases initial setup time by the duration of app installations.",
                    ApplyOps = [RegOp.SetDword(EnlKey, "ShowEnrollmentStatusPage", 1)],
                    RemoveOps = [RegOp.DeleteValue(EnlKey, "ShowEnrollmentStatusPage")],
                    DetectOps = [RegOp.CheckDword(EnlKey, "ShowEnrollmentStatusPage", 1)],
                },
                new TweakDef
                {
                    Id = "devenl-block-enrollment-from-unknown-networks",
                    Label = "Device Enrollment Limit: Block Enrollment Attempts from Non-Corporate Networks",
                    Category = "Peripherals — Bluetooth Adv",
                    Description =
                        "Sets RequireCorporateNetworkForEnrollment=1 in DeviceEnrollment policy. Restricts MDM enrollment to devices on corporate networks (defined by the network location awareness profile). Enrollment attempts from unclassified or public networks are blocked. This prevents bulk enrollment of devices by an attacker using stolen credentials from outside the corporate network perimeter. While this is most relevant for legacy MDM setups without Azure AD conditional access, it adds network perimeter enforcement as an extra enrollment control — enrollment over public networks requires re-evaluation of the risk posture.",
                    Tags = ["enrollment", "network-restriction", "corporate-network", "nla", "perimeter"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 3,
                    ImpactNote =
                        "MDM enrollment is only permitted from networks classified as corporate (domain controller reachable, NLA domain profile active). Devices on guest, public, or unclassified networks cannot enroll. This may prevent legitimate remote onboarding — coordinate with VPN policies to ensure remote enrollment is still possible via corporate VPN.",
                    ApplyOps = [RegOp.SetDword(EnlKey, "RequireCorporateNetworkForEnrollment", 1)],
                    RemoveOps = [RegOp.DeleteValue(EnlKey, "RequireCorporateNetworkForEnrollment")],
                    DetectOps = [RegOp.CheckDword(EnlKey, "RequireCorporateNetworkForEnrollment", 1)],
                },
                new TweakDef
                {
                    Id = "devenl-log-enrollment-failures",
                    Label = "Device Enrollment Limit: Enable Detailed Logging of Enrollment Failures",
                    Category = "Peripherals — Bluetooth Adv",
                    Description =
                        "Sets LogEnrollmentFailures=1 in DeviceEnrollment policy. Enables detailed logging of MDM enrollment failure events to the Windows Event Log. Enrollment failures are written to the Microsoft-Windows-DeviceManagement-Enterprise-Diagnostics-Provider/Admin channel with structured error codes (HRESULT), the enrollment phase that failed (token acquisition, DRS discovery, enrollment registration, certificate acquisition), and whether the failure was a network error, authentication error, or server error. This significantly accelerates helpdesk troubleshooting of Autopilot and enrollment failures.",
                    Tags = ["enrollment", "failure-logging", "event-log", "diagnostics", "helpdesk"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "MDM enrollment failures are logged with structured error codes and phase information. Logs written to the Microsoft-Windows-DeviceManagement-Enterprise-Diagnostics-Provider channel. No performance impact — logging only occurs on failure paths.",
                    ApplyOps = [RegOp.SetDword(EnlKey, "LogEnrollmentFailures", 1)],
                    RemoveOps = [RegOp.DeleteValue(EnlKey, "LogEnrollmentFailures")],
                    DetectOps = [RegOp.CheckDword(EnlKey, "LogEnrollmentFailures", 1)],
                },
                new TweakDef
                {
                    Id = "devenl-require-mfa-for-enrollment",
                    Label = "Device Enrollment Limit: Require MFA at MDM Enrollment Time",
                    Category = "Peripherals — Bluetooth Adv",
                    Description =
                        "Sets RequireMfaForEnrollment=1 in DeviceEnrollment policy. Requires multi-factor authentication at the time of MDM enrollment in addition to the standard password credential. Without MFA at enrollment, a stolen password is sufficient to enroll an attacker's device into the corporate MDM tenant. With enrollment MFA enforced, the attacker must also have the victim's second factor (phone, hardware key) to complete enrollment. MDM enrollment grants the device significant privileges (policy application, certificate issuance, resource access upon compliance) — requiring MFA at this critical step is essential.",
                    Tags = ["mfa", "enrollment", "authentication", "conditional-access", "identity"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Users must complete MFA during MDM enrollment. Requires Azure AD MFA or equivalent. Autopilot deployments using device-identity-based enrollment (PPKG or DEM account) may need exemption. Test Autopilot flows before broad deployment.",
                    ApplyOps = [RegOp.SetDword(EnlKey, "RequireMfaForEnrollment", 1)],
                    RemoveOps = [RegOp.DeleteValue(EnlKey, "RequireMfaForEnrollment")],
                    DetectOps = [RegOp.CheckDword(EnlKey, "RequireMfaForEnrollment", 1)],
                },
                new TweakDef
                {
                    Id = "devenl-audit-enrollment-activity",
                    Label = "Device Enrollment Limit: Audit All Device Enrollment Activity",
                    Category = "Peripherals — Bluetooth Adv",
                    Description =
                        "Sets AuditEnrollmentActivity=1 in DeviceEnrollment policy. Enables audit logging for all device enrollment activity: successful enrollments, failed enrollment attempts, enrollment profile matching and rejection, and unenrollment events. Audit records include the user UPN that initiated the enrollment, the device serial number and hardware ID, the enrollment profile matched (or lack thereof), and the outcome. Enrollment audit logs are written to the Microsoft-Windows-DeviceManagement-Enterprise-Diagnostics-Provider/Admin channel and can be forwarded to SIEM for detection of rogue enrollment attempts.",
                    Tags = ["enrollment", "audit", "siem", "monitoring", "security-event"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "All enrollment attempts are logged with user, device, and outcome details. Audit events can be forwarded to SIEM. Detection: unusually high enrollment failures from a single user may indicate credential stuffing. No performance overhead — logging is asynchronous.",
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
                Category = "Peripherals — Bluetooth Adv",
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
                Category = "Peripherals — Bluetooth Adv",
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
                Category = "Peripherals — Bluetooth Adv",
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
                Category = "Peripherals — Bluetooth Adv",
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
                Category = "Peripherals — Bluetooth Adv",
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
                Category = "Peripherals — Bluetooth Adv",
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
                Category = "Peripherals — Bluetooth Adv",
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
                Category = "Peripherals — Bluetooth Adv",
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
                Category = "Peripherals — Bluetooth Adv",
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
                Category = "Peripherals — Bluetooth Adv",
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
                    Id = "devguard-require-uefi-mat",
                    Label = "Require UEFI Memory Attributes Table for HVCI",
                    Category = "Peripherals — Bluetooth Adv",
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
                    Id = "devguard-enable-system-guard",
                    Label = "Enable System Guard Secure Launch",
                    Category = "Peripherals — Bluetooth Adv",
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
                    Category = "Peripherals — Bluetooth Adv",
                    Description = "Activates Hardware-Enforced Call Stack Protection (CET Shadow Stack) for kernel mode to resist ROP attacks.",
                    Tags = ["cet", "shadow-stack", "device-guard", "kernel", "security", "rop"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote =
                        "Requires Intel CET (Tiger Lake+) or AMD equivalent; blocks ROP/JOP exploits in kernel; may conflict with old drivers.",
                    ApplyOps = [RegOp.SetDword(DgKey, "ConfigureKernelShadowStacksLaunchControl", 1)],
                    RemoveOps = [RegOp.DeleteValue(DgKey, "ConfigureKernelShadowStacksLaunchControl")],
                    DetectOps = [RegOp.CheckDword(DgKey, "ConfigureKernelShadowStacksLaunchControl", 1)],
                },
                new TweakDef
                {
                    Id = "devguard-enable-hvci-audit-mode",
                    Label = "Disable HVCI Audit Mode (Enforce Mode)",
                    Category = "Peripherals — Bluetooth Adv",
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
                    Category = "Peripherals — Bluetooth Adv",
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
                    Category = "Peripherals — Bluetooth Adv",
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

        private const string HvciScenario =
            @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard\Scenarios\HypervisorEnforcedCodeIntegrity";

        private const string CredentialGuard = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa";

        private const string CodeIntegrity = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CI\Config";

        private const string DeviceGuardPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceGuard";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "vbs-enable-hvci",
                Label = "Enable Hypervisor-Protected Code Integrity (HVCI)",
                Category = "Peripherals — Bluetooth Adv",
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
                Id = "vbs-require-secure-boot-dma",
                Label = "Require Secure Boot + DMA Protection for VBS",
                Category = "Peripherals — Bluetooth Adv",
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
                Id = "vbs-enable-config-ci-policy",
                Label = "Enable Configurable Code Integrity (WDAC Boot Policy)",
                Category = "Peripherals — Bluetooth Adv",
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
                Id = "vbs-enable-kernel-shadow-stacks",
                Label = "Enable Kernel Shadow Stacks (Control Flow Guard Enforcement)",
                Category = "Peripherals — Bluetooth Adv",
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
                Category = "Peripherals — Bluetooth Adv",
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
                Category = "Peripherals — Bluetooth Adv",
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
        private const string HcKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceHealthAttestation";

        private const string TpmKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\TPM";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "devhc-enable-tpm-health-check",
                    Label = "Device Health: Enable TPM Health State Evaluation",
                    Category = "Peripherals — Bluetooth Adv",
                    Description =
                        "Sets EnableTpmHealthCheck=1 in DeviceHealthAttestation policy. Enables evaluation of TPM health state as part of the device health check. The TPM health check evaluates whether the TPM is enabled, activated, owned, and in a known-good state. TPMs can enter a reduced-functionality mode (e.g., after detecting too many failed PIN attempts or a firmware update that changes the platform configuration registers). A TPM in degraded state cannot attest the boot chain, which can silently cause attestation failures unless the health check actively reports the degraded status.",
                    Tags = ["tpm", "health-check", "attestation", "degraded-state", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "TPM health is evaluated on every DHA cycle. Degraded or disabled TPM is reported as a health issue. Enables IT to detect TPM lockout or firmware-changed PCR states before they cause silent attestation failures.",
                    ApplyOps = [RegOp.SetDword(HcKey, "EnableTpmHealthCheck", 1)],
                    RemoveOps = [RegOp.DeleteValue(HcKey, "EnableTpmHealthCheck")],
                    DetectOps = [RegOp.CheckDword(HcKey, "EnableTpmHealthCheck", 1)],
                },
                new TweakDef
                {
                    Id = "devhc-require-elam-driver-for-health",
                    Label = "Device Health: Require ELAM Driver Active for Healthy State",
                    Category = "Peripherals — Bluetooth Adv",
                    Description =
                        "Sets RequireElamDriverForHealth=1 in DeviceHealthAttestation policy. Reports the device as unhealthy if an Early Launch Anti-Malware (ELAM) driver is not loaded and active at boot. ELAM drivers are loaded before all other non-Microsoft drivers, giving them the ability to evaluate and classify boot drivers as trusted, untrusted, or unknown before they are allowed to initialize. Without an active ELAM driver, the device's pre-OS environment cannot be assessed for rootkits or boot drivers installed by malware. Windows Defender is an ELAM-registered product and satisfies this requirement.",
                    Tags = ["elam", "health", "boot-security", "early-launch", "malware-prevention"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Devices without an active ELAM driver are reported as unhealthy by DHA. Windows Defender satisfies this requirement by default. Third-party ELAM-registered AV products also satisfy it. Devices with all AV disabled will fail this check.",
                    ApplyOps = [RegOp.SetDword(HcKey, "RequireElamDriverForHealth", 1)],
                    RemoveOps = [RegOp.DeleteValue(HcKey, "RequireElamDriverForHealth")],
                    DetectOps = [RegOp.CheckDword(HcKey, "RequireElamDriverForHealth", 1)],
                },
                new TweakDef
                {
                    Id = "devhc-evaluate-secure-boot-measurement",
                    Label = "Device Health: Evaluate Secure Boot PCR Measurement Consistency",
                    Category = "Peripherals — Bluetooth Adv",
                    Description =
                        "Sets EvaluateSecureBootMeasurement=1 in DeviceHealthAttestation policy. Enables DHA to evaluate the consistency of Secure Boot Platform Configuration Register (PCR) measurements. TPM PCR values record hashes of every component in the boot chain. If the PCR values in the most recent health certificate differ from the baseline (e.g., a firmware update changed a boot component hash), the attestation service can detect this deviation and flag the device. This catches scenarios where a firmware update inadvertently introduced an unsigned component or where a bootkit altered a measured value.",
                    Tags = ["secure-boot", "pcr", "measurement", "attestation", "boot-chain"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Secure Boot PCR measurements are included in DHA health certificates. Changes to PCR values (firmware update, boot component change) are detected. Legitimate firmware updates may transiently mark the device as unhealthy until the DHA baseline is updated.",
                    ApplyOps = [RegOp.SetDword(HcKey, "EvaluateSecureBootMeasurement", 1)],
                    RemoveOps = [RegOp.DeleteValue(HcKey, "EvaluateSecureBootMeasurement")],
                    DetectOps = [RegOp.CheckDword(HcKey, "EvaluateSecureBootMeasurement", 1)],
                },
                new TweakDef
                {
                    Id = "devhc-set-health-report-retention-30days",
                    Label = "Device Health: Retain Health Reports for 30 Days",
                    Category = "Peripherals — Bluetooth Adv",
                    Description =
                        "Sets HealthReportRetentionDays=30 in DeviceHealthAttestation policy. Sets the number of days that device health reports are retained locally before being purged. Retaining health reports for 30 days provides a rolling audit of the device's health state history. This is useful for post-incident forensics: if a device was compromised, the health report history can show the exact point at which the TPM measurements changed, when Secure Boot was disabled, or when the ELAM driver was removed — correlating health state changes with suspicious events in the device's event log.",
                    Tags = ["health-report", "retention", "forensics", "audit", "30-days"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Health report data is retained for 30 days locally. Provides 30 days of health state history for forensic investigation. Small disk footprint — health reports are compact JSON structures, typically a few KB each.",
                    ApplyOps = [RegOp.SetDword(HcKey, "HealthReportRetentionDays", 30)],
                    RemoveOps = [RegOp.DeleteValue(HcKey, "HealthReportRetentionDays")],
                    DetectOps = [RegOp.CheckDword(HcKey, "HealthReportRetentionDays", 30)],
                },
                new TweakDef
                {
                    Id = "devhc-disable-health-check-bypass",
                    Label = "Device Health: Disable Health Check Bypass for Non-Compliant State",
                    Category = "Peripherals — Bluetooth Adv",
                    Description =
                        "Sets DisableHealthCheckBypass=1 in DeviceHealthAttestation policy. Prevents clients (including local administrators) from bypassing or suppressing the device health check. Without this policy, a sophisticated user or malware with admin privileges can modify the health state cache or suppress health certificate requests, causing the device to appear healthy to conditional access systems while actually being compromised. Disabling the bypass ensures that the DHA client cannot be locally tampered with to present a false healthy state.",
                    Tags = ["health-check", "bypass-prevention", "anti-tampering", "admin-restriction", "integrity"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "Health check processes cannot be bypassed or suppressed by local admins. Prevents malware or sophisticated users from spoofing a healthy state to conditional access systems. May complicate debugging of attestation issues in development environments.",
                    ApplyOps = [RegOp.SetDword(HcKey, "DisableHealthCheckBypass", 1)],
                    RemoveOps = [RegOp.DeleteValue(HcKey, "DisableHealthCheckBypass")],
                    DetectOps = [RegOp.CheckDword(HcKey, "DisableHealthCheckBypass", 1)],
                },
                new TweakDef
                {
                    Id = "devhc-enable-health-check-auto-remediation",
                    Label = "Device Health: Enable Automatic Remediation for Known Health Issues",
                    Category = "Peripherals — Bluetooth Adv",
                    Description =
                        "Sets EnableHealthAutoRemediation=1 in DeviceHealthAttestation policy. Enables the Device Health agent to attempt automatic remediation for known, non-critical health issues. Remediable issues include re-enabling Windows Defender real-time protection that was automatically disabled by a third-party AV (after that AV was uninstalled), re-enrolling the TPM endorsement key if the certificate expired, or restarting stalled health service processes. Automatic remediation reduces helpdesk tickets for transient compliance failures caused by installation or configuration drift.",
                    Tags = ["health", "auto-remediation", "defender", "tpm", "service-restart"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "The health agent automatically resolves known fixable issues (re-enables AV, restarts health services, re-provisions TPM EK). Only remediates known, low-risk issues — it will never force-enable BitLocker or change user-configured settings. Review the list of supported remediations for your OS build.",
                    ApplyOps = [RegOp.SetDword(HcKey, "EnableHealthAutoRemediation", 1)],
                    RemoveOps = [RegOp.DeleteValue(HcKey, "EnableHealthAutoRemediation")],
                    DetectOps = [RegOp.CheckDword(HcKey, "EnableHealthAutoRemediation", 1)],
                },
                new TweakDef
                {
                    Id = "devhc-enable-tpm-endorsement-key-validation",
                    Label = "Device Health: Enable TPM Endorsement Key Validation",
                    Category = "Peripherals — Bluetooth Adv",
                    Description =
                        "Sets ValidateTpmEndorsementKey=1 in TPM policy. Enables validation that the TPM's Endorsement Key (EK) certificate is in a known-valid certificate chain rooted at a trusted TPM manufacturer CA. The EK uniquely identifies the physical TPM chip. If EK validation is disabled or skipped, software-based fake TPM implementations (used in virtual machines without vTPM, or malicious virtual TPM drivers) can pass attestation checks. EK validation ensures the attestation chain is anchored to a real hardware chip with a manufacturer-issued certificate.",
                    Tags = ["tpm", "endorsement-key", "ek-validation", "hardware-anchor", "attestation"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "TPM endorsement key certificates are validated against the manufacturer CA chain. VMs with software vTPM (Hyper-V vTPM, VMware vTPM) have EK certificates signed by Microsoft or the platform vendor and will pass if those CAs are trusted. Non-certified TPMs in custom hardware may fail.",
                    ApplyOps = [RegOp.SetDword(TpmKey, "ValidateTpmEndorsementKey", 1)],
                    RemoveOps = [RegOp.DeleteValue(TpmKey, "ValidateTpmEndorsementKey")],
                    DetectOps = [RegOp.CheckDword(TpmKey, "ValidateTpmEndorsementKey", 1)],
                },
                new TweakDef
                {
                    Id = "devhc-require-tpm-version-20",
                    Label = "Device Health: Require TPM 2.0 for Health Attestation",
                    Category = "Peripherals — Bluetooth Adv",
                    Description =
                        "Sets RequireTpm20ForHealthAttestation=1 in TPM policy. Marks devices as unable to provide health attestation if they only have a TPM 1.2 chip (as opposed to a TPM 2.0). TPM 1.2 supports SHA-1 algorithm measurement banks. TPM 2.0 adds SHA-256 banks, algorithm agility, and enhanced authorization structures. Modern DHA services require TPM 2.0's enhanced capabilities for accurate, tamper-resistant attestation. TPM 1.2 attestation can be spoofed more easily and lacks support for Credential Guard, Device Guard, and Virtualization-Based Security measurements.",
                    Tags = ["tpm", "tpm-20", "attestation", "sha256", "vbs"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "Devices with only TPM 1.2 cannot provide health attestation and are treated as unhealthy. Hardware manufactured before 2016 may only have TPM 1.2. Devices with no TPM are already unable to attest. Review device fleet hardware compatibility before enforcing.",
                    ApplyOps = [RegOp.SetDword(TpmKey, "RequireTpm20ForHealthAttestation", 1)],
                    RemoveOps = [RegOp.DeleteValue(TpmKey, "RequireTpm20ForHealthAttestation")],
                    DetectOps = [RegOp.CheckDword(TpmKey, "RequireTpm20ForHealthAttestation", 1)],
                },
                new TweakDef
                {
                    Id = "devhc-enable-code-integrity-measurement",
                    Label = "Device Health: Enable Code Integrity State in Health Reports",
                    Category = "Peripherals — Bluetooth Adv",
                    Description =
                        "Sets IncludeCodeIntegrityInReport=1 in DeviceHealthAttestation policy. Includes Windows Code Integrity (CI) enforcement state in the DHA health certificate. Code Integrity state records whether Windows Defender Application Control (WDAC) or Device Guard is active, whether CI is in audit vs. enforcement mode, and whether User-Mode Code Integrity (UMCI) is enabled in addition to HVCI (Hypervisor-Protected Code Integrity). Including CI state in the attestation report allows conditional access systems to require not just that the device is healthy but that it is actively enforcing application whitelisting.",
                    Tags = ["code-integrity", "wdac", "device-guard", "hvci", "attestation"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Code Integrity (WDAC/HVCI) state is included in the DHA health certificate. Conditional access can now require that a device have CI enforcement mode active. Devices in CI audit-only mode can be flagged as less secure than those in enforcement mode.",
                    ApplyOps = [RegOp.SetDword(HcKey, "IncludeCodeIntegrityInReport", 1)],
                    RemoveOps = [RegOp.DeleteValue(HcKey, "IncludeCodeIntegrityInReport")],
                    DetectOps = [RegOp.CheckDword(HcKey, "IncludeCodeIntegrityInReport", 1)],
                },
                new TweakDef
                {
                    Id = "devhc-enable-vbs-state-measurement",
                    Label = "Device Health: Include VBS/Credential Guard State in Health Reports",
                    Category = "Peripherals — Bluetooth Adv",
                    Description =
                        "Sets IncludeVbsStateInReport=1 in DeviceHealthAttestation policy. Includes Virtualization-Based Security (VBS) and Credential Guard state in the DHA health certificate. VBS isolates critical OS components (LSA, UEFI variable writes) inside a secure virtual machine backed by the CPU hypervisor, making credential theft attacks (Pass-the-Hash, Pass-the-Ticket) significantly harder. Including VBS state in attestation reports allows conditional access to enforce that only VBS-enabled devices handle sensitive workloads — for example, requiring VBS for devices that access privileged admin consoles.",
                    Tags = ["vbs", "credential-guard", "hypervisor", "attestation", "lsa-protection"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "VBS and Credential Guard state is included in DHA health certificates. Conditional access can require VBS/Credential Guard for high-privilege resource access. Devices without hardware VBS support (no hardware-enforced DEP, SLAT, or IOMMU) cannot satisfy this requirement.",
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
                Id = "dinst-enable-class-block",
                Label = "Device Install Policy: Enable Setup Class GUID Restriction List",
                Category = "Peripherals — Bluetooth Adv",
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
                Id = "dinst-retroactive-id-block",
                Label = "Device Install Policy: Apply Device ID Blocks Retroactively",
                Category = "Peripherals — Bluetooth Adv",
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
                Category = "Peripherals — Bluetooth Adv",
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
                Category = "Peripherals — Bluetooth Adv",
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
                Category = "Peripherals — Bluetooth Adv",
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
                Category = "Peripherals — Bluetooth Adv",
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
                Category = "Peripherals — Bluetooth Adv",
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
                    Category = "Peripherals — Device Install",
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
                    Category = "Peripherals — Device Install",
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
                    Category = "Peripherals — Device Install",
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
                    Category = "Peripherals — Device Install",
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
                    Category = "Peripherals — Device Install",
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
                    Category = "Peripherals — Device Install",
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
                    Category = "Peripherals — Device Install",
                    Description =
                        "Blocks installation of IEEE 1394 (FireWire) bus controllers, which support DMA and can bypass OS memory protection.",
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
                    Category = "Peripherals — Device Install",
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
                    Category = "Peripherals — Device Install",
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
                    Category = "Peripherals — Device Install",
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
                Id = "devlockgpo-disable-hello-pin-recovery",
                Label = "Device Lock GPO: Disable Windows Hello PIN Recovery Service",
                Category = "Peripherals — Device Install",
                Description =
                    "Disables the cloud-based Windows Hello PIN recovery service that allows users to reset their device PIN via their Microsoft account or Azure AD credentials. The PIN recovery service sends encrypted PIN reset data to Microsoft cloud servers. In high-security environments where no cloud dependencies are allowed, this service should be disabled.",
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
                Id = "devlockgpo-require-screensaver-password",
                Label = "Device Lock GPO: Require Password When Resuming from Screen Saver",
                Category = "Peripherals — Device Install",
                Description =
                    "Forces Windows to require the user's password (or PIN) when resuming from a screen saver or after a period of inactivity. This is a foundational physical security control that prevents unauthorized access to unattended workstations. Without this policy, an unlocked workstation can be accessed by anyone who sits down at the keyboard.",
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
                Id = "devlockgpo-set-screensaver-timeout-600",
                Label = "Device Lock GPO: Set Screen Saver Timeout to 10 Minutes (600 s)",
                Category = "Peripherals — Device Install",
                Description =
                    "Sets the screen saver / auto-lock timeout to 600 seconds (10 minutes). Industry security frameworks (CIS, NIST SP 800-53, PCI DSS) recommend an idle timeout of 10–15 minutes for standard workstations. A 10-minute timeout balances security with productivity, locking unattended machines before a brief absence creates risk.",
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
                Category = "Peripherals — Device Install",
                Description =
                    "Prevents Windows from displaying app notifications (toast notifications) on the lock screen. Lock-screen notifications can expose sensitive information to passersby — email previews, chat messages, calendar events — without requiring authentication. This policy disables all notification content from appearing while the screen is locked.",
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
                Category = "Peripherals — Device Install",
                Description =
                    "Prevents cameras (webcams, built-in laptop cameras) from being activated while the workstation is locked. Some Windows Hello facial recognition implementations allow the camera to be used on the lock screen, but malicious code or physical manipulation could trigger unauthorized image capture. Disabling the camera on the lock screen closes this attack surface.",
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
                Id = "devprov-skip-machine-oobe",
                Label = "OOBE: Skip the machine out-of-box experience setup",
                Category = "Peripherals — Device Install",
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
                Category = "Peripherals — Device Install",
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
                Category = "Peripherals — Device Install",
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
                Category = "Peripherals — Device Install",
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
                Id = "devprov-disable-wpj-flyout",
                Label = "Workplace Join: Disable the 'Connect to work or school' flyout",
                Category = "Peripherals — Device Install",
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
                Category = "Peripherals — Device Install",
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
        ];
    }

    // ── DeviceRegistrationPolicy ──
    private static class _DeviceRegistrationPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceRegistration";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "devreg-disable-auto-device-registration",
                    Label = "Disable Automatic Azure AD Device Registration",
                    Category = "Peripherals — Device Install",
                    Description =
                        "Prevents the device from automatically registering with Azure Active Directory / Entra ID during domain join or user sign-in. Gives IT full control over when and how devices are registered. Default: auto-register on domain join. Recommended: 1 when phased registration is required.",
                    Tags = ["device-registration", "azure-ad", "entra", "mdm", "enrollment", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote =
                        "Device does not automatically register with Azure AD/Entra on domain join; manual or scripted registration is required.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableAutoDeviceRegistration", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoDeviceRegistration")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableAutoDeviceRegistration", 1)],
                },
                new TweakDef
                {
                    Id = "devreg-require-tpm-for-registration",
                    Label = "Require TPM for Device Registration",
                    Category = "Peripherals — Device Install",
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
                    Category = "Peripherals — Device Install",
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
                    Category = "Peripherals — Device Install",
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
                    Category = "Peripherals — Device Install",
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
                    Category = "Peripherals — Device Install",
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
                    Category = "Peripherals — Device Install",
                    Description =
                        "Enforces that the device must meet Intune / Endpoint Manager compliance policies before completing Azure AD Hybrid registration. Non-compliant devices are blocked until they satisfy the compliance posture. Default: not enforced. Recommended: 1 for Conditional Access deployments.",
                    Tags = ["device-registration", "compliance", "intune", "conditional-access", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "Non-compliant devices (missing patches, disabled Defender) cannot complete registration; gate for Conditional Access.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireDeviceCompliance", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireDeviceCompliance")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireDeviceCompliance", 1)],
                },
                new TweakDef
                {
                    Id = "devreg-certificate-validity-days-365",
                    Label = "Set Device Certificate Validity to 365 Days",
                    Category = "Peripherals — Device Install",
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
                    Category = "Peripherals — Device Install",
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
                    Category = "Peripherals — Device Install",
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
}
