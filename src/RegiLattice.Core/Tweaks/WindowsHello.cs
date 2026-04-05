namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// ── Merged from WindowsHello.cs ──────────────────────────────────────────────────

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
            Id = "hello-disable-dynamic-lock",
            Label = "Disable Dynamic Lock (Phone Proximity Auto-Lock)",
            Category = "User Account",
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
            Id = "hello-disable-companion-device-unlock",
            Label = "Disable Companion Device Unlock (Phone as Key)",
            Category = "User Account",
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
            Category = "User Account",
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
            Id = "hello-disable-cloud-trust-kerberos",
            Label = "Disable Cloud Kerberos Trust for Hybrid Hello",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = false,
            Tags = ["windows hello", "kerberos", "cloud trust", "azure ad", "hybrid"],
            Description =
                "Disables the Cloud Trust authentication model for Windows Hello "
                + "for Business on Entra ID hybrid-joined devices. Forces the "
                + "traditional on-premises PKI trust path instead of the cloud "
                + "Kerberos ticket flow.",
            ApplyOps = [RegOp.SetDword(HelloPol, "UseCloudTrustForOnPremAuth", 0)],
            RemoveOps = [RegOp.DeleteValue(HelloPol, "UseCloudTrustForOnPremAuth")],
            DetectOps = [RegOp.CheckDword(HelloPol, "UseCloudTrustForOnPremAuth", 0)],
        },
        new TweakDef
        {
            Id = "hello-disable-phone-sign-in",
            Label = "Disable Phone (Companion Device) Sign-In for Hello",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = false,
            Tags = ["windows hello", "phone", "companion device", "unlock", "security"],
            Description =
                "Blocks Windows Hello companion device (phone/wearable) unlock "
                + "via the Microsoft Account + Bluetooth proximity mechanism. "
                + "Enforces that each sign-in requires on-device biometric or PIN.",
            ApplyOps = [RegOp.SetDword(HelloPol, "AllowPhoneLinkDevice", 0)],
            RemoveOps = [RegOp.DeleteValue(HelloPol, "AllowPhoneLinkDevice")],
            DetectOps = [RegOp.CheckDword(HelloPol, "AllowPhoneLinkDevice", 0)],
        },
        new TweakDef
        {
            Id = "hello-disable-web-sign-in",
            Label = "Disable Web Sign-In for Windows (Hello MSA Flow)",
            Category = "User Account",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["windows hello", "web sign-in", "msa", "fido2", "account"],
            Description =
                "Disables the Web Sign-In credential provider which shows an "
                + "embedded browser for FIDO2/MSA login. Prevents web-based "
                + "phishing flows from appearing on the lock screen.",
            ApplyOps = [RegOp.SetDword(CredProvPol, "EnableWebSignIn", 0)],
            RemoveOps = [RegOp.DeleteValue(CredProvPol, "EnableWebSignIn")],
            DetectOps = [RegOp.CheckDword(CredProvPol, "EnableWebSignIn", 0)],
        },
    ];
}
