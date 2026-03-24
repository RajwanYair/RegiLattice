namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class MdmEnrollmentPolicy
{
    private const string MdmKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\MDM";
    private const string WpjKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WorkplaceJoin";
    private const string HelloKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PassportForWork";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "mdmpol-disable-auto-enroll",
            Label = "Disable Automatic MDM Enrollment on Azure AD Join",
            Category = "MDM Enrollment Policy",
            Description =
                "Prevents the device from automatically enrolling into Mobile Device Management (MDM/Intune) when joined to Azure Active Directory. Requires explicit manual enrollment.",
            Tags = ["mdm", "intune", "azure-ad", "enrollment", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [MdmKey],
            ApplyOps = [RegOp.SetDword(MdmKey, "AutoEnrollMDM", 0)],
            RemoveOps = [RegOp.DeleteValue(MdmKey, "AutoEnrollMDM")],
            DetectOps = [RegOp.CheckDword(MdmKey, "AutoEnrollMDM", 0)],
        },
        new TweakDef
        {
            Id = "mdmpol-disable-user-registration",
            Label = "Disable User-Initiated MDM Registration",
            Category = "MDM Enrollment Policy",
            Description =
                "Prevents users from manually registering the device with a Mobile Device Management server. Only administrators can initiate MDM enrollment.",
            Tags = ["mdm", "enrollment", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [MdmKey],
            ApplyOps = [RegOp.SetDword(MdmKey, "EnableRegistration", 0)],
            RemoveOps = [RegOp.DeleteValue(MdmKey, "EnableRegistration")],
            DetectOps = [RegOp.CheckDword(MdmKey, "EnableRegistration", 0)],
        },
        new TweakDef
        {
            Id = "mdmpol-block-aad-workplace-join",
            Label = "Block Azure AD Workplace Join",
            Category = "MDM Enrollment Policy",
            Description =
                "Prevents the device from being registered with Azure Active Directory as a workplace-joined device. Blocks self-service Azure AD registration from Settings.",
            Tags = ["azure-ad", "workplace-join", "mdm", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [WpjKey],
            ApplyOps = [RegOp.SetDword(WpjKey, "BlockAADWorkplaceJoin", 1)],
            RemoveOps = [RegOp.DeleteValue(WpjKey, "BlockAADWorkplaceJoin")],
            DetectOps = [RegOp.CheckDword(WpjKey, "BlockAADWorkplaceJoin", 1)],
        },
        new TweakDef
        {
            Id = "mdmpol-disable-auto-workplace-join",
            Label = "Disable Automatic Workplace Registration",
            Category = "MDM Enrollment Policy",
            Description =
                "Prevents the device from automatically registering with a workplace (Azure AD/Entra ID) during user sign-in. Requires explicit admin-driven join workflow.",
            Tags = ["azure-ad", "workplace-join", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [WpjKey],
            ApplyOps = [RegOp.SetDword(WpjKey, "autoWorkplaceJoin", 0)],
            RemoveOps = [RegOp.DeleteValue(WpjKey, "autoWorkplaceJoin")],
            DetectOps = [RegOp.CheckDword(WpjKey, "autoWorkplaceJoin", 0)],
        },
        new TweakDef
        {
            Id = "mdmpol-disable-hello-for-business",
            Label = "Disable Windows Hello for Business",
            Category = "MDM Enrollment Policy",
            Description =
                "Disables Windows Hello for Business (WHFB) enterprise credential provisioning. Users cannot set up WHFB biometrics or PIN backed by PKI infrastructure.",
            Tags = ["windows-hello", "hello-for-business", "credential", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [HelloKey],
            ApplyOps = [RegOp.SetDword(HelloKey, "Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(HelloKey, "Enabled")],
            DetectOps = [RegOp.CheckDword(HelloKey, "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "mdmpol-require-hello-tpm",
            Label = "Require TPM for Windows Hello for Business",
            Category = "MDM Enrollment Policy",
            Description =
                "Requires a Trusted Platform Module (TPM) chip for Windows Hello for Business provisioning. Prevents software-only (less secure) TPM emulation from being used.",
            Tags = ["windows-hello", "tpm", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [HelloKey],
            ApplyOps = [RegOp.SetDword(HelloKey, "RequireSecurityDevice", 1)],
            RemoveOps = [RegOp.DeleteValue(HelloKey, "RequireSecurityDevice")],
            DetectOps = [RegOp.CheckDword(HelloKey, "RequireSecurityDevice", 1)],
        },
        new TweakDef
        {
            Id = "mdmpol-disable-hello-pin-recovery",
            Label = "Disable Windows Hello PIN Recovery Service",
            Category = "MDM Enrollment Policy",
            Description =
                "Disables the cloud-based PIN recovery service for Windows Hello. PINs cannot be reset via Microsoft account cloud backup. Keeps credentials fully local.",
            Tags = ["windows-hello", "pin", "recovery", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [HelloKey],
            ApplyOps = [RegOp.SetDword(HelloKey, "EnablePinRecovery", 0)],
            RemoveOps = [RegOp.DeleteValue(HelloKey, "EnablePinRecovery")],
            DetectOps = [RegOp.CheckDword(HelloKey, "EnablePinRecovery", 0)],
        },
        new TweakDef
        {
            Id = "mdmpol-disable-hello-remote",
            Label = "Disable Remote Windows Hello (Phone Sign-In)",
            Category = "MDM Enrollment Policy",
            Description =
                "Disables the Remote Windows Hello feature that allows using a phone or paired device as a sign-in credential for the PC. Available since Windows 10 1607.",
            Tags = ["windows-hello", "remote", "phone", "credential", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [$@"{HelloKey}\Remote"],
            ApplyOps = [RegOp.SetDword($@"{HelloKey}\Remote", "Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{HelloKey}\Remote", "Enabled")],
            DetectOps = [RegOp.CheckDword($@"{HelloKey}\Remote", "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "mdmpol-disable-hello-biometrics",
            Label = "Disable Biometrics for Windows Hello",
            Category = "MDM Enrollment Policy",
            Description =
                "Disables the use of biometrics (fingerprint, face recognition) for Windows Hello authentication. PIN remains available as the fallback credential.",
            Tags = ["windows-hello", "biometrics", "fingerprint", "face-id", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [$@"{HelloKey}\Biometrics"],
            ApplyOps = [RegOp.SetDword($@"{HelloKey}\Biometrics", "UseBiometrics", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{HelloKey}\Biometrics", "UseBiometrics")],
            DetectOps = [RegOp.CheckDword($@"{HelloKey}\Biometrics", "UseBiometrics", 0)],
        },
        new TweakDef
        {
            Id = "mdmpol-disable-dynamic-lock",
            Label = "Disable Dynamic Lock (Phone Proximity Lock)",
            Category = "MDM Enrollment Policy",
            Description =
                "Disables Dynamic Lock, which automatically locks the PC when a paired Bluetooth phone moves out of range. Prevents unintended automatic locking in enterprise environments.",
            Tags = ["dynamic-lock", "bluetooth", "lock", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DynamicLock"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DynamicLock", "AllowDynamicLock", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DynamicLock", "AllowDynamicLock")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DynamicLock", "AllowDynamicLock", 0)],
        },
    ];
}
