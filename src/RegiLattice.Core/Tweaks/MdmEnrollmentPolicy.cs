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
            ImpactScore = 4,
            SafetyRating = 3,
            ImpactNote = "Prevents automatic Intune enrollment on Azure AD join; deploy deliberately.",
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
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Blocks self-service MDM registration by users.",
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
            ImpactScore = 3,
            SafetyRating = 3,
            ImpactNote = "Prevents self-service Azure AD registration; may impact BYOD scenarios.",
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
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Prevents automatic Azure AD/Entra ID device registration during sign-in.",
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
            ImpactScore = 4,
            SafetyRating = 3,
            ImpactNote = "Disables enterprise WHfB provisioning; users cannot set up PKI-backed credentials.",
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
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Ensures WHfB credentials are TPM-protected, not software-emulated.",
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
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Keeps PIN credentials fully local; disables cloud recovery backup.",
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
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Disables using a paired phone as a PC sign-in credential.",
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
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Forces PIN-only authentication; no fingerprint or face ID.",
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
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Disables Bluetooth proximity-based automatic PC locking.",
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DynamicLock"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DynamicLock", "AllowDynamicLock", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DynamicLock", "AllowDynamicLock")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DynamicLock", "AllowDynamicLock", 0)],
        },
    ];
}
