// EasMdmPolicy.cs — Exchange ActiveSync MDM policy enforcement
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EasMdm
// Slug: easmdm
// Category: Exchange ActiveSync MDM Policy

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class EasMdmPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EasMdm";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "easmdm-require-device-password",
            Label = "Exchange ActiveSync MDM Policy: Require Device Password",
            Category = "Exchange ActiveSync MDM Policy",
            Description =
                "Enforces a device password requirement via Exchange ActiveSync MDM policy. "
                + "When enabled, users must configure a PIN or password before the device can synchronise with an Exchange server. "
                + "This aligns with corporate security baselines that mandate authentication on managed endpoints. "
                + "Disabling removes the enforced password requirement imposed by EAS MDM.",
            Tags = ["eas", "mdm", "password", "exchange", "enterprise"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "PasswordEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "PasswordEnabled")],
            DetectOps = [RegOp.CheckDword(Key, "PasswordEnabled", 1)],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Enforces device password via EAS MDM; improves security posture on managed devices.",
        },
        new TweakDef
        {
            Id = "easmdm-min-password-length",
            Label = "Exchange ActiveSync MDM Policy: Set Minimum Password Length (8)",
            Category = "Exchange ActiveSync MDM Policy",
            Description =
                "Sets the minimum device password length to 8 characters via Exchange ActiveSync MDM policy. "
                + "Short passwords are vulnerable to brute-force attacks, especially on mobile and endpoint devices. "
                + "A minimum of 8 characters is recommended by NIST SP 800-63B and aligns with most corporate security policies. "
                + "Removing this policy reverts to the platform default (typically 4 characters).",
            Tags = ["eas", "mdm", "password", "length", "exchange"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "MinDevicePasswordLength", 8)],
            RemoveOps = [RegOp.DeleteValue(Key, "MinDevicePasswordLength")],
            DetectOps = [RegOp.CheckDword(Key, "MinDevicePasswordLength", 8)],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Forces minimum 8-character passwords on EAS-managed devices, reducing brute-force risk.",
        },
        new TweakDef
        {
            Id = "easmdm-max-failed-attempts",
            Label = "Exchange ActiveSync MDM Policy: Limit Max Failed Password Attempts (10)",
            Category = "Exchange ActiveSync MDM Policy",
            Description =
                "Caps the number of consecutive failed password attempts to 10 before triggering a device lockout via Exchange ActiveSync MDM. "
                + "Limiting failed attempts deters brute-force attacks against the device lock screen. "
                + "After the threshold is reached, the device is locked and may require an administrator to unlock or initiate a remote wipe. "
                + "Removing this policy restores the uncapped default.",
            Tags = ["eas", "mdm", "password", "failed-attempts", "lockout"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "MaxDevicePasswordFailedAttempts", 10)],
            RemoveOps = [RegOp.DeleteValue(Key, "MaxDevicePasswordFailedAttempts")],
            DetectOps = [RegOp.CheckDword(Key, "MaxDevicePasswordFailedAttempts", 10)],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Triggers lockout after 10 failed password attempts; protects against brute-force attacks.",
        },
        new TweakDef
        {
            Id = "easmdm-inactivity-lock-5min",
            Label = "Exchange ActiveSync MDM Policy: Lock Device After 5 Minutes Inactivity",
            Category = "Exchange ActiveSync MDM Policy",
            Description =
                "Configures the Exchange ActiveSync MDM policy to lock the device screen after 5 minutes of inactivity. "
                + "Auto-locking an idle device prevents unauthorised access when the device is left unattended. "
                + "Five minutes is the industry-standard timeout recommended for corporate laptops and workstations. "
                + "Removing this policy lifts the MDM-enforced inactivity timeout.",
            Tags = ["eas", "mdm", "lock", "inactivity", "screen-lock"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "MaxInactivityTimeDeviceLock", 5)],
            RemoveOps = [RegOp.DeleteValue(Key, "MaxInactivityTimeDeviceLock")],
            DetectOps = [RegOp.CheckDword(Key, "MaxInactivityTimeDeviceLock", 5)],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Auto-locks device after 5 minutes; reduces risk of unauthorised access to unattended endpoints.",
        },
        new TweakDef
        {
            Id = "easmdm-require-encryption",
            Label = "Exchange ActiveSync MDM Policy: Require Device Encryption",
            Category = "Exchange ActiveSync MDM Policy",
            Description =
                "Requires full device storage encryption via Exchange ActiveSync MDM policy. "
                + "Encryption ensures that data stored on the device cannot be read if the hardware is lost or stolen. "
                + "This policy is mandatory for PCI-DSS, HIPAA, and most corporate data-protection frameworks. "
                + "Removing this setting lifts the MDM encryption mandate.",
            Tags = ["eas", "mdm", "encryption", "bitlocker", "data-protection"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RequireDeviceEncryption", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireDeviceEncryption")],
            DetectOps = [RegOp.CheckDword(Key, "RequireDeviceEncryption", 1)],
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "Mandates full-disk encryption on EAS-managed devices; critical for data-at-rest protection.",
        },
        new TweakDef
        {
            Id = "easmdm-block-wifi",
            Label = "Exchange ActiveSync MDM Policy: Block Wi-Fi Connections",
            Category = "Exchange ActiveSync MDM Policy",
            Description =
                "Disables Wi-Fi connectivity on the device via Exchange ActiveSync MDM policy. "
                + "Blocking Wi-Fi forces the device to use wired or cellular connections, reducing exposure on potentially unsecured wireless networks. "
                + "This is typically applied to high-security endpoints or kiosk devices where wireless connectivity is not permitted. "
                + "Removing this policy restores MDM-controlled Wi-Fi access.",
            Tags = ["eas", "mdm", "wifi", "network", "enterprise"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AllowWiFi", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowWiFi")],
            DetectOps = [RegOp.CheckDword(Key, "AllowWiFi", 0)],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Blocks Wi-Fi on managed devices; enforces wired/cellular-only network access.",
        },
        new TweakDef
        {
            Id = "easmdm-block-removable-storage",
            Label = "Exchange ActiveSync MDM Policy: Block Removable Storage",
            Category = "Exchange ActiveSync MDM Policy",
            Description =
                "Prevents access to removable storage media (SD cards, USB drives) via Exchange ActiveSync MDM policy. "
                + "Removable storage is a common vector for data exfiltration and introduction of malware. "
                + "Blocking it on managed endpoints aligns with DLP (Data Loss Prevention) requirements in regulated industries. "
                + "Removing this policy restores MDM-controlled removable storage access.",
            Tags = ["eas", "mdm", "storage", "dlp", "removable"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AllowRemovableStorage", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowRemovableStorage")],
            DetectOps = [RegOp.CheckDword(Key, "AllowRemovableStorage", 0)],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Blocks SD cards and USB drives on MDM-managed devices; reduces data exfiltration risk.",
        },
        new TweakDef
        {
            Id = "easmdm-block-camera",
            Label = "Exchange ActiveSync MDM Policy: Block Camera Use",
            Category = "Exchange ActiveSync MDM Policy",
            Description =
                "Disables camera hardware on the device via Exchange ActiveSync MDM policy. "
                + "Camera restrictions are commonly required in secure facilities, clean-room environments, or for devices that handle classified information. "
                + "Enforcing this via MDM policy ensures compliance cannot be bypassed by the end user. "
                + "Removing this policy restores camera availability on MDM-managed devices.",
            Tags = ["eas", "mdm", "camera", "privacy", "enterprise"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AllowCamera", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowCamera")],
            DetectOps = [RegOp.CheckDword(Key, "AllowCamera", 0)],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Disables camera on MDM-managed devices; required for secure-facility compliance.",
        },
        new TweakDef
        {
            Id = "easmdm-block-internet-sharing",
            Label = "Exchange ActiveSync MDM Policy: Block Internet Sharing / Hotspot",
            Category = "Exchange ActiveSync MDM Policy",
            Description =
                "Blocks the ability to share the device's internet connection (hotspot/tethering) via Exchange ActiveSync MDM policy. "
                + "Mobile hotspot can bypass corporate network monitoring and proxy controls, introducing compliance gaps. "
                + "Prohibiting internet sharing on managed endpoints is a common corporate policy to maintain network visibility. "
                + "Removing this policy lifts the MDM hotspot restriction.",
            Tags = ["eas", "mdm", "hotspot", "tethering", "network"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AllowInternetSharing", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowInternetSharing")],
            DetectOps = [RegOp.CheckDword(Key, "AllowInternetSharing", 0)],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Prevents hotspot/tethering on managed devices; maintains corporate network control.",
        },
        new TweakDef
        {
            Id = "easmdm-block-bluetooth",
            Label = "Exchange ActiveSync MDM Policy: Block Bluetooth",
            Category = "Exchange ActiveSync MDM Policy",
            Description =
                "Disables Bluetooth connectivity on the device via Exchange ActiveSync MDM policy. "
                + "Bluetooth can be exploited for proximity-based attacks (BlueSnarfing, BIAS) or used to exfiltrate data without leaving a network trace. "
                + "Disabling Bluetooth is recommended for high-security endpoints where physical proximity attacks are a concern. "
                + "Removing this policy restores MDM-controlled Bluetooth access (value 2 = allow, 0 = block).",
            Tags = ["eas", "mdm", "bluetooth", "wireless", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AllowBluetooth", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowBluetooth")],
            DetectOps = [RegOp.CheckDword(Key, "AllowBluetooth", 0)],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Disables Bluetooth on MDM-managed devices; reduces BlueSnarfing and proximity-based attack risk.",
        },
    ];
}
