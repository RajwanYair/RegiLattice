#nullable enable

using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

internal static class CredentialUiPolicy
{
    private const string CredUi = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CredUI";
    private const string CredUiCu = @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\CredUI";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "credui-disable-password-reveal",
            Label = "Disable Password Reveal Button in Credential UI",
            Category = "Credential UI Policy",
            Description = "Hides the password-reveal eye icon in credential dialogs and the lock screen, reducing shoulder-surfing risk.",
            Tags = ["credential", "security", "group-policy", "hardening", "password"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(CredUi, "DisablePasswordReveal", 1)],
            RemoveOps = [RegOp.DeleteValue(CredUi, "DisablePasswordReveal")],
            DetectOps = [RegOp.CheckDword(CredUi, "DisablePasswordReveal", 1)],
        },
        new TweakDef
        {
            Id = "credui-disable-administrator-enumeration",
            Label = "Disable Administrator Account Enumeration in Credential UI",
            Category = "Credential UI Policy",
            Description = "Prevents the credential prompt from enumerating or listing administrator accounts, reducing account information leakage.",
            Tags = ["credential", "security", "group-policy", "hardening"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(CredUi, "EnumerateAdministrators", 0)],
            RemoveOps = [RegOp.DeleteValue(CredUi, "EnumerateAdministrators")],
            DetectOps = [RegOp.CheckDword(CredUi, "EnumerateAdministrators", 0)],
        },
        new TweakDef
        {
            Id = "credui-no-local-password-reset-questions",
            Label = "Disable Local Account Password Reset Security Questions",
            Category = "Credential UI Policy",
            Description = "Prevents setup and use of security questions for local account password resets, requiring admin intervention for locked-out accounts.",
            Tags = ["credential", "security", "group-policy", "hardening", "password"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(CredUi, "NoLocalPasswordResetQuestions", 1)],
            RemoveOps = [RegOp.DeleteValue(CredUi, "NoLocalPasswordResetQuestions")],
            DetectOps = [RegOp.CheckDword(CredUi, "NoLocalPasswordResetQuestions", 1)],
        },
        new TweakDef
        {
            Id = "credui-enable-secure-credential-prompting",
            Label = "Require Secure Desktop for Credential UI Prompts",
            Category = "Credential UI Policy",
            Description = "Forces credential dialogs to appear on the secure desktop, preventing malicious programs from intercepting or spoofing credential prompts.",
            Tags = ["credential", "security", "group-policy", "hardening", "uac"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(CredUi, "EnableSecureCredentialPrompting", 1)],
            RemoveOps = [RegOp.DeleteValue(CredUi, "EnableSecureCredentialPrompting")],
            DetectOps = [RegOp.CheckDword(CredUi, "EnableSecureCredentialPrompting", 1)],
        },
        new TweakDef
        {
            Id = "credui-disable-visual-prompt",
            Label = "Disable Credential UI Visual Prompt Animation",
            Category = "Credential UI Policy",
            Description = "Suppresses the animated shimmer/glow visual prompt in the credential UI, reducing distraction in kiosk and focused-work environments.",
            Tags = ["credential", "ui", "group-policy", "kiosk"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(CredUi, "NoVisualPrompt", 1)],
            RemoveOps = [RegOp.DeleteValue(CredUi, "NoVisualPrompt")],
            DetectOps = [RegOp.CheckDword(CredUi, "NoVisualPrompt", 1)],
        },
        new TweakDef
        {
            Id = "credui-disable-save-credentials",
            Label = "Disable Save Credentials for RDP",
            Category = "Credential UI Policy",
            Description = "Prevents the OS from saving RDP credentials in the Windows Credential Manager, requiring re-entry on each connection.",
            Tags = ["credential", "security", "group-policy", "rdp", "hardening"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(CredUi, "DisableSaveCredentials", 1)],
            RemoveOps = [RegOp.DeleteValue(CredUi, "DisableSaveCredentials")],
            DetectOps = [RegOp.CheckDword(CredUi, "DisableSaveCredentials", 1)],
        },
        new TweakDef
        {
            Id = "credui-disable-windows-hello-pinlogin",
            Label = "Disable Windows Hello PIN Login from Credential UI",
            Category = "Credential UI Policy",
            Description = "Prevents PIN authentication from appearing as an option in network credential prompts, enforcing password-only authentication.",
            Tags = ["credential", "security", "group-policy", "windows-hello", "pin"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(CredUi, "DisableWindowsHelloPINLogin", 1)],
            RemoveOps = [RegOp.DeleteValue(CredUi, "DisableWindowsHelloPINLogin")],
            DetectOps = [RegOp.CheckDword(CredUi, "DisableWindowsHelloPINLogin", 1)],
        },
        new TweakDef
        {
            Id = "credui-disable-user-password-reveal-cu",
            Label = "Disable Password Reveal (User Policy)",
            Category = "Credential UI Policy",
            Description = "Applies the disable-password-reveal rule at the current-user scope, ensuring the eye icon is hidden even without machine admin rights.",
            Tags = ["credential", "security", "group-policy", "password"],
            NeedsAdmin = false,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(CredUiCu, "DisablePasswordReveal", 1)],
            RemoveOps = [RegOp.DeleteValue(CredUiCu, "DisablePasswordReveal")],
            DetectOps = [RegOp.CheckDword(CredUiCu, "DisablePasswordReveal", 1)],
        },
        new TweakDef
        {
            Id = "credui-block-generic-credential-caching",
            Label = "Block Generic Network Credential Caching",
            Category = "Credential UI Policy",
            Description = "Prevents Windows from caching plaintext generic network credentials in the Credential Manager store.",
            Tags = ["credential", "security", "group-policy", "hardening"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(CredUi, "CacheLogonCredentials", 0)],
            RemoveOps = [RegOp.DeleteValue(CredUi, "CacheLogonCredentials")],
            DetectOps = [RegOp.CheckDword(CredUi, "CacheLogonCredentials", 0)],
        },
        new TweakDef
        {
            Id = "credui-disable-autofill-on-credential-forms",
            Label = "Disable Auto-Fill on Credential Input Forms",
            Category = "Credential UI Policy",
            Description = "Prevents the credential UI from auto-filling remembered usernames and passwords on domain credential dialogs.",
            Tags = ["credential", "security", "group-policy", "password"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(CredUi, "DisableAutofill", 1)],
            RemoveOps = [RegOp.DeleteValue(CredUi, "DisableAutofill")],
            DetectOps = [RegOp.CheckDword(CredUi, "DisableAutofill", 1)],
        },
    ];
}
