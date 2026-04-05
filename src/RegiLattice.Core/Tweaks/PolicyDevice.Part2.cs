namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static partial class PolicyDevice
{
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
                    Category = "Peripherals — Device Install",
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
                    Category = "Peripherals — Device Install",
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
                    Category = "Peripherals — Device Install",
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
                    Category = "Peripherals — Device Install",
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
                    Category = "Peripherals — Device Install",
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
                    Category = "Peripherals — Device Install",
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
                    Category = "Peripherals — Device Install",
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
                    Category = "Peripherals — Device Install",
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
                    Category = "Peripherals — Device Install",
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
                    Category = "Peripherals — Device Install",
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
                Category = "Peripherals — Device Install",
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
                Category = "Peripherals — Device Install",
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
                Category = "Peripherals — Device Install",
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
                Category = "Peripherals — Device Install",
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
                Category = "Peripherals — Device Install",
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
                Category = "Peripherals — Device Install",
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
                Category = "Peripherals — Device Install",
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
                Category = "Peripherals — Device Install",
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
                Category = "Peripherals — Device Install",
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
                Category = "Peripherals — Device Install",
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
                    Id = "kdmapol-block-pre-boot-dma",
                    Label = "Block Pre-Boot DMA Access on Thunderbolt Ports",
                    Category = "Peripherals — Device Install",
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
                    Category = "Peripherals — Device Install",
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
                    Category = "Peripherals — Device Install",
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
                    Category = "Peripherals — Device Install",
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
                    Category = "Peripherals — Device Install",
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
                    Category = "Peripherals — Device Install",
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
                    Category = "Peripherals — Device Install",
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
                    Category = "Peripherals — Device Install",
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
                    Category = "Peripherals — Device Install",
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
        private const string CrashControl = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl";

        private const string Wer = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting";

        private const string WerQueue = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting\Queue";

        private const string WerConsentPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PCHealth\ErrorReporting";

        private const string WerPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "dump-disable-wer-queue",
                Label = "Disable WER Queuing of Crash Reports",
                Category = "Peripherals — Device Install",
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
                Category = "Peripherals — Device Install",
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
                Category = "Peripherals — Device Install",
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
                Category = "Peripherals — Device Install",
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
                Category = "Peripherals — Device Install",
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
                Category = "Peripherals — Device Install",
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
                Category = "Peripherals — Device Install",
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
                Category = "Peripherals — Device Install",
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
                Category = "Peripherals — Device Install",
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
                Category = "Peripherals — Device Install",
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
                Category = "Peripherals — Device Install",
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
                    Id = "wpd-deny-portable-devices",
                    Label = "Deny Portable Device (MTP/WPD) Access",
                    Category = "Peripherals — Portable Device",
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
                    Category = "Peripherals — Portable Device",
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
                    Category = "Peripherals — Portable Device",
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
                    Category = "Peripherals — Portable Device",
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
                    Category = "Peripherals — Portable Device",
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
                    Category = "Peripherals — Portable Device",
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
                    Category = "Peripherals — Portable Device",
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
                    Category = "Peripherals — Portable Device",
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
                    Category = "Peripherals — Portable Device",
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
                Category = "Peripherals — Portable Device",
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
                Category = "Peripherals — Portable Device",
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
                Category = "Peripherals — Portable Device",
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
                Category = "Peripherals — Portable Device",
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
                Category = "Peripherals — Portable Device",
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
                Category = "Peripherals — Portable Device",
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
                Category = "Peripherals — Portable Device",
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
                Category = "Peripherals — Portable Device",
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
                Category = "Peripherals — Portable Device",
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
                Category = "Peripherals — Portable Device",
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
                Category = "Peripherals — Portable Device",
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
                Category = "Peripherals — Portable Device",
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
                Category = "Peripherals — Portable Device",
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
                Category = "Peripherals — Portable Device",
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
                Category = "Peripherals — Portable Device",
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
                Category = "Peripherals — Portable Device",
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
                Category = "Peripherals — Portable Device",
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
                Category = "Peripherals — Portable Device",
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
                Category = "Peripherals — Portable Device",
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
                Category = "Peripherals — Portable Device",
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
                Category = "Peripherals — Portable Device",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
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
                Category = "Peripherals — Portable Device",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
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
                Category = "Peripherals — Portable Device",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
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
                Category = "Peripherals — Portable Device",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
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
                Category = "Peripherals — Portable Device",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
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
                Category = "Peripherals — Portable Device",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
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
                Category = "Peripherals — Portable Device",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
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
                Category = "Peripherals — Portable Device",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
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
                Category = "Peripherals — Portable Device",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
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
                Category = "Peripherals — Portable Device",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
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
                Category = "Peripherals — Portable Device",
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
                Category = "Peripherals — Portable Device",
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
                Category = "Peripherals — Portable Device",
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
                Category = "Peripherals — Portable Device",
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
                Category = "Peripherals — Portable Device",
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
                Category = "Peripherals — Portable Device",
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
                Category = "Peripherals — Portable Device",
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
                Category = "Peripherals — Portable Device",
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
                Category = "Peripherals — Portable Device",
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
                Category = "Peripherals — Portable Device",
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
                    Category = "Peripherals — Virtual Disk Service",
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
                    Category = "Peripherals — Virtual Disk Service",
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
                    Category = "Peripherals — Virtual Disk Service",
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
                    Category = "Peripherals — Virtual Disk Service",
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
                    Category = "Peripherals — Virtual Disk Service",
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
                    Category = "Peripherals — Virtual Disk Service",
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
                    Category = "Peripherals — Virtual Disk Service",
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
                    Category = "Peripherals — Virtual Disk Service",
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
                    Category = "Peripherals — Virtual Disk Service",
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
                    Category = "Peripherals — Virtual Disk Service",
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
