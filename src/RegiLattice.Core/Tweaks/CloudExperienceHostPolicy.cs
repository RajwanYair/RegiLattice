namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class CloudExperienceHostPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudExperienceHost";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "cehpol-disable-cloud-experience",
            Label = "CXH Policy: Disable Windows Cloud Experience Host",
            Category = "Cloud Experience Host Policy",
            Description = "Disables the Windows Cloud Experience Host (CXH) process that manages OOBE, Tips, and cloud-connected first-run experiences. Reduces telemetry and suppresses pop-up prompts to connect Microsoft services.",
            Tags = ["cxh", "oobe", "cloud", "experience", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableCloudExperienceHost", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableCloudExperienceHost")],
            DetectOps = [RegOp.CheckDword(Key, "DisableCloudExperienceHost", 1)],
        },
        new TweakDef
        {
            Id = "cehpol-disable-oobe-privacy-page",
            Label = "CXH Policy: Disable Privacy Settings Page in OOBE",
            Category = "Cloud Experience Host Policy",
            Description = "Skips the Privacy Settings experience page during Windows Out-of-Box Experience (OOBE). Ensures default privacy settings are applied silently without user interaction during provisioning.",
            Tags = ["cxh", "oobe", "privacy", "setup", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisablePrivacyExperiencePage", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisablePrivacyExperiencePage")],
            DetectOps = [RegOp.CheckDword(Key, "DisablePrivacyExperiencePage", 1)],
        },
        new TweakDef
        {
            Id = "cehpol-skip-machine-oobe",
            Label = "CXH Policy: Skip Machine-Level OOBE on First Boot",
            Category = "Cloud Experience Host Policy",
            Description = "Skips the machine-level Windows OOBE experience on the first boot of a provisioned device. Useful for enterprise images where OOBE is unnecessary and should be bypassed for imaging targets.",
            Tags = ["cxh", "oobe", "provisioning", "first-boot", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "SkipMachineOOBE", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "SkipMachineOOBE")],
            DetectOps = [RegOp.CheckDword(Key, "SkipMachineOOBE", 1)],
        },
        new TweakDef
        {
            Id = "cehpol-disable-tailored-experience",
            Label = "CXH Policy: Disable Tailored Experiences with Diagnostic Data",
            Category = "Cloud Experience Host Policy",
            Description = "Prevents Windows from using diagnostic data to deliver personalised tips, ads, and recommendations via the Cloud Experience Host. Applies at the machine level via Group Policy.",
            Tags = ["cxh", "tailored", "diagnostic", "telemetry", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "PreventTailoredExperiences", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "PreventTailoredExperiences")],
            DetectOps = [RegOp.CheckDword(Key, "PreventTailoredExperiences", 1)],
        },
        new TweakDef
        {
            Id = "cehpol-disable-frx-telemetry",
            Label = "CXH Policy: Disable OOBE Telemetry Data Submission",
            Category = "Cloud Experience Host Policy",
            Description = "Disables telemetry data collection and submission during the OOBE First-Run Experience (Frx). Prevents Microsoft from receiving device setup analytics from enterprise-provisioned devices.",
            Tags = ["cxh", "oobe", "telemetry", "frx", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableOOBETelemetry", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableOOBETelemetry")],
            DetectOps = [RegOp.CheckDword(Key, "DisableOOBETelemetry", 1)],
        },
        new TweakDef
        {
            Id = "cehpol-disable-account-setup-page",
            Label = "CXH Policy: Disable Account Setup Page in Provisioning",
            Category = "Cloud Experience Host Policy",
            Description = "Bypasses the Microsoft Account / Azure AD account setup page during OOBE provisioning. Ensures the device is silently joined to the corporate domain without displaying the consumer account prompt.",
            Tags = ["cxh", "oobe", "account", "provisioning", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableAccountSetupPage", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableAccountSetupPage")],
            DetectOps = [RegOp.CheckDword(Key, "DisableAccountSetupPage", 1)],
        },
        new TweakDef
        {
            Id = "cehpol-disable-cortana-oobe",
            Label = "CXH Policy: Disable Cortana during OOBE",
            Category = "Cloud Experience Host Policy",
            Description = "Prevents the Cortana voice assistant from launching during OOBE. Stops Cortana from speaking during initial setup on enterprise-provisioned devices, reducing unexpected data transmission.",
            Tags = ["cxh", "oobe", "cortana", "voice", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableCortanaDuringOOBE", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableCortanaDuringOOBE")],
            DetectOps = [RegOp.CheckDword(Key, "DisableCortanaDuringOOBE", 1)],
        },
        new TweakDef
        {
            Id = "cehpol-disable-device-encryption-page",
            Label = "CXH Policy: Skip Device Encryption Page in OOBE",
            Category = "Cloud Experience Host Policy",
            Description = "Bypasses the BitLocker Device Encryption setup page during OOBE. Enterprises typically deploy their own BitLocker policy via MDM/GPO and do not want users configuring encryption manually.",
            Tags = ["cxh", "oobe", "bitlocker", "encryption", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "SkipDeviceEncryptionPage", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "SkipDeviceEncryptionPage")],
            DetectOps = [RegOp.CheckDword(Key, "SkipDeviceEncryptionPage", 1)],
        },
        new TweakDef
        {
            Id = "cehpol-disable-windows-hello-oobe",
            Label = "CXH Policy: Skip Windows Hello Setup in OOBE",
            Category = "Cloud Experience Host Policy",
            Description = "Bypasses the Windows Hello biometric/PIN setup prompts during OOBE. Enterprises deploying Windows Hello for Business via GPO/MDM do not need the consumer OOBE Hello setup flow.",
            Tags = ["cxh", "oobe", "windows-hello", "biometrics", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "SkipWindowsHelloSetupPage", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "SkipWindowsHelloSetupPage")],
            DetectOps = [RegOp.CheckDword(Key, "SkipWindowsHelloSetupPage", 1)],
        },
        new TweakDef
        {
            Id = "cehpol-disable-oobe-network-page",
            Label = "CXH Policy: Skip Network Connection Page in OOBE",
            Category = "Cloud Experience Host Policy",
            Description = "Skips the Wi-Fi / network connection page during OOBE. Enterprise devices are typically pre-configured with wireless profiles via MDM, removing the need to prompt users during provisioning.",
            Tags = ["cxh", "oobe", "network", "wifi", "provisioning", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "SkipNetworkConnectionPage", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "SkipNetworkConnectionPage")],
            DetectOps = [RegOp.CheckDword(Key, "SkipNetworkConnectionPage", 1)],
        },
    ];
}
