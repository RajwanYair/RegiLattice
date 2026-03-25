// RegiLattice.Core — Tweaks/HardwareDevicePolicy.cs
// Sprint 299: Hardware Device Policy tweaks (10 tweaks)
// Category: "Hardware Device Policy" | Slug: hwdev
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceInstall

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class HardwareDevicePolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceInstall";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "hwdev-prevent-unknown-install",
            Label = "Prevent Installation of Devices Not Described by Other Policies",
            Category = "Hardware Device Policy",
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
            Category = "Hardware Device Policy",
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
            Category = "Hardware Device Policy",
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
            Category = "Hardware Device Policy",
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
            Category = "Hardware Device Policy",
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
            Category = "Hardware Device Policy",
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
            Category = "Hardware Device Policy",
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
            Category = "Hardware Device Policy",
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
            Category = "Hardware Device Policy",
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
            Category = "Hardware Device Policy",
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
