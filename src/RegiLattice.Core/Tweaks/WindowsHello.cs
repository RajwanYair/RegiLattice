// RegiLattice.Core — Tweaks/WindowsHello.cs
// Windows Hello PIN, biometrics, Dynamic Lock, and FIDO2 controls.
// Uses slug "hello" — no overlap with existing modules.

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WindowsHello
{
    private const string BioPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Biometrics";
    private const string CredProvPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";
    private const string PinCplx = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PassportForWork\PINComplexity";
    private const string HelloPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PassportForWork";
    private const string DynLock = @"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Winlogon";
    private const string CredProv = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\LogonUI";
    private const string BioFace = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Biometrics\FacialFeatures";
    private const string NgcPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "hello-disable-hello-for-work",
            Label = "Disable Windows Hello for Business Provisioning",
            Category = "Windows Hello",
            NeedsAdmin = true,
            CorpSafe = false,
            Tags = ["windows hello", "biometrics", "security", "policy"],
            Description =
                "Prevents Windows Hello for Business from provisioning on this device "
                + "via Group Policy. Useful on personal machines enrolled in Intune by mistake.",
            ApplyOps = [RegOp.SetDword(HelloPol, "Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(HelloPol, "Enabled")],
            DetectOps = [RegOp.CheckDword(HelloPol, "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "hello-disable-biometric-enrollment",
            Label = "Disable Biometric Enrollment (GPO)",
            Category = "Windows Hello",
            NeedsAdmin = true,
            CorpSafe = false,
            Tags = ["windows hello", "biometrics", "fingerprint", "policy"],
            Description = "Blocks all biometric enrollment via GPO. Prevents Windows from " + "prompting to add a fingerprint or face after sign-in.",
            ApplyOps = [RegOp.SetDword(BioPol, "Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(BioPol, "Enabled")],
            DetectOps = [RegOp.CheckDword(BioPol, "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "hello-disable-facial-recognition-enhanced",
            Label = "Disable Enhanced Anti-Spoofing for Facial Recognition",
            Category = "Windows Hello",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["windows hello", "biometrics", "face recognition"],
            Description =
                "Disables the enhanced anti-spoofing check for facial recognition, "
                + "useful when using a basic webcam that doesn't support 3D scanning.",
            ApplyOps = [RegOp.SetDword(BioFace, "EnhancedAntiSpoofing", 0)],
            RemoveOps = [RegOp.DeleteValue(BioFace, "EnhancedAntiSpoofing")],
            DetectOps = [RegOp.CheckDword(BioFace, "EnhancedAntiSpoofing", 0)],
        },
        new TweakDef
        {
            Id = "hello-disable-dynamic-lock",
            Label = "Disable Dynamic Lock (Phone Proximity Auto-Lock)",
            Category = "Windows Hello",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["windows hello", "lock screen", "bluetooth", "dynamic lock"],
            Description =
                "Disables Dynamic Lock, which automatically locks the PC when your "
                + "paired phone walks away. Stops false positives from brief Bluetooth drops.",
            ApplyOps = [RegOp.SetDword(DynLock, "EnableGoodbye", 0)],
            RemoveOps = [RegOp.DeleteValue(DynLock, "EnableGoodbye")],
            DetectOps = [RegOp.CheckDword(DynLock, "EnableGoodbye", 0)],
        },
        new TweakDef
        {
            Id = "hello-disable-pin-recovery",
            Label = "Disable PIN Recovery via Microsoft Account",
            Category = "Windows Hello",
            NeedsAdmin = true,
            CorpSafe = false,
            Tags = ["windows hello", "pin", "privacy", "microsoft account"],
            Description =
                "Prevents Windows from uploading a PIN recovery key to your " + "Microsoft account, keeping the PIN credential fully local.",
            ApplyOps = [RegOp.SetDword(HelloPol, "EnablePinRecovery", 0)],
            RemoveOps = [RegOp.DeleteValue(HelloPol, "EnablePinRecovery")],
            DetectOps = [RegOp.CheckDword(HelloPol, "EnablePinRecovery", 0)],
        },
        new TweakDef
        {
            Id = "hello-enforce-strong-pin-length",
            Label = "Enforce Minimum PIN Length (6 Digits)",
            Category = "Windows Hello",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["windows hello", "pin", "security", "hardening"],
            Description =
                "Sets the minimum PIN length to 6 digits via Group Policy, " + "improving resistance to brute-force attacks on the local login.",
            ApplyOps = [RegOp.SetDword(PinCplx, "MinimumPINLength", 6)],
            RemoveOps = [RegOp.DeleteValue(PinCplx, "MinimumPINLength")],
            DetectOps = [RegOp.CheckDword(PinCplx, "MinimumPINLength", 6)],
        },
        new TweakDef
        {
            Id = "hello-enable-pin-expiry",
            Label = "Enable PIN Expiry (90-Day Rotation)",
            Category = "Windows Hello",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["windows hello", "pin", "security", "policy"],
            Description = "Requires the PIN to be changed every 90 days, matching common " + "enterprise password rotation policies.",
            ApplyOps = [RegOp.SetDword(PinCplx, "Expiration", 90)],
            RemoveOps = [RegOp.DeleteValue(PinCplx, "Expiration")],
            DetectOps = [RegOp.CheckDword(PinCplx, "Expiration", 90)],
        },
        new TweakDef
        {
            Id = "hello-disable-companion-device-unlock",
            Label = "Disable Companion Device Unlock (Phone as Key)",
            Category = "Windows Hello",
            NeedsAdmin = true,
            CorpSafe = false,
            Tags = ["windows hello", "companion device", "bluetooth", "security"],
            Description =
                "Disables the Companion Device Framework used by Phone Link and "
                + "other apps to unlock Windows with a paired device, reducing the "
                + "Bluetooth attack surface.",
            ApplyOps = [RegOp.SetDword(CredProvPol, "AllowDomainPINLogon", 0)],
            RemoveOps = [RegOp.DeleteValue(CredProvPol, "AllowDomainPINLogon")],
            DetectOps = [RegOp.CheckDword(CredProvPol, "AllowDomainPINLogon", 0)],
        },
        new TweakDef
        {
            Id = "hello-disable-picture-password",
            Label = "Disable Picture Password Logon",
            Category = "Windows Hello",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["windows hello", "login", "security", "picture password"],
            Description =
                "Blocks picture-password (gesture on photo) from being set as a "
                + "sign-in option. Picture passwords are weaker than PINs due to pattern guessing.",
            ApplyOps = [RegOp.SetDword(CredProvPol, "BlockDomainPicturePassword", 1)],
            RemoveOps = [RegOp.DeleteValue(CredProvPol, "BlockDomainPicturePassword")],
            DetectOps = [RegOp.CheckDword(CredProvPol, "BlockDomainPicturePassword", 1)],
        },
        new TweakDef
        {
            Id = "hello-disable-pin-on-device-provisioning",
            Label = "Disable Forced PIN Creation During Setup",
            Category = "Windows Hello",
            NeedsAdmin = true,
            CorpSafe = false,
            Tags = ["windows hello", "pin", "setup", "oobe"],
            Description =
                "Prevents Windows from prompting to create a Windows Hello PIN " + "during the Out-of-Box Experience. Allows password-only accounts.",
            ApplyOps = [RegOp.SetDword(HelloPol, "DisablePostLogonProvisioning", 1)],
            RemoveOps = [RegOp.DeleteValue(HelloPol, "DisablePostLogonProvisioning")],
            DetectOps = [RegOp.CheckDword(HelloPol, "DisablePostLogonProvisioning", 1)],
        },
    ];
}
