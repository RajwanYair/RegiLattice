namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// RegiLattice.Core — Tweaks/PolicyCredentialUI.cs
// Windows Credential UI and Credential Providers Group Policy security hardening tweaks.
// Category: "Security"
// Sprint 671 (v6.11.0)

internal static class PolicyCredentialUI
{
    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            .. _CredUIPolicy.Data,
            .. _CredProviderPolicy.Data,
        ];

    // ── Sprint 671a — Credential UI (LogonUI) Policy ──────────────────────────
    private static class _CredUIPolicy
    {
        private const string CredUiKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CredUI";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "sec-credui-disable-enumerateadmins",
                    Label = "Disable Administrator Account Enumeration at Elevation Prompt",
                    Category = "Security",
                    Description =
                        "Prevents UAC elevation prompts from listing local administrator accounts as clickable options. When enumeration is allowed, an attacker learns valid admin account names. Disabling forces explicit credential entry. Default: enumeration allowed. Recommended: disabled (CIS L1).",
                    Tags = ["uac", "elevation", "credential", "admin", "security", "policy", "cis"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ApplyOps = [RegOp.SetDword(CredUiKey, "EnumerateAdministrators", 0)],
                    RemoveOps = [RegOp.DeleteValue(CredUiKey, "EnumerateAdministrators")],
                    DetectOps = [RegOp.CheckDword(CredUiKey, "EnumerateAdministrators", 0)],
                },
                new TweakDef
                {
                    Id = "sec-credui-require-trusted-path",
                    Label = "Require Trusted Path for Credential Entry",
                    Category = "Security",
                    Description =
                        "Forces users to enter credentials through the Windows trusted path (Ctrl+Alt+Del secure desktop), rather than through possibly spoofed application dialogs. Prevents credential theft by fake login windows. Default: trusted path optional. Recommended: required.",
                    Tags = ["credential", "secure-desktop", "uac", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ApplyOps = [RegOp.SetDword(CredUiKey, "RequireTrustedPath", 1)],
                    RemoveOps = [RegOp.DeleteValue(CredUiKey, "RequireTrustedPath")],
                    DetectOps = [RegOp.CheckDword(CredUiKey, "RequireTrustedPath", 1)],
                },
                new TweakDef
                {
                    Id = "sec-credui-disable-generic-prompt",
                    Label = "Disable Generic Credential Entry Prompts",
                    Category = "Security",
                    Description =
                        "Disables generic credential entry prompts from applications. Generic prompts allow any application to display a Windows-branded credential dialog, which can be exploited for credential phishing. Disabling forces apps through proper Windows authentication channels. Default: generic prompts allowed. Recommended: disabled.",
                    Tags = ["credential", "prompt", "phishing", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 3,
                    ApplyOps = [RegOp.SetDword(CredUiKey, "DisablePasswordReveal", 1)],
                    RemoveOps = [RegOp.DeleteValue(CredUiKey, "DisablePasswordReveal")],
                    DetectOps = [RegOp.CheckDword(CredUiKey, "DisablePasswordReveal", 1)],
                },
                new TweakDef
                {
                    Id = "sec-credui-no-anon-logon-ux",
                    Label = "Block Anonymous Logon Credential UI Display",
                    Category = "Security",
                    Description =
                        "Prevents the credential UI from displaying account chooser entries for anonymous or null-session logon. These can be used to enumerate account names in network authentication scenarios. Default: anonymous entries shown. Recommended: hidden.",
                    Tags = ["credential", "anonymous", "logon", "enumeration", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ApplyOps = [RegOp.SetDword(CredUiKey, "NoLocalPasswordResetQuestions", 1)],
                    RemoveOps = [RegOp.DeleteValue(CredUiKey, "NoLocalPasswordResetQuestions")],
                    DetectOps = [RegOp.CheckDword(CredUiKey, "NoLocalPasswordResetQuestions", 1)],
                },
                new TweakDef
                {
                    Id = "sec-credui-disable-web-creds-provider",
                    Label = "Disable Web Credential Provider in Logon UI",
                    Category = "Security",
                    Description =
                        "Blocks the Web Credentials provider tile (Microsoft Account, AAD web-auth) from appearing in the Windows Logon UI and UAC elevation dialogs. Reduces the attack surface by removing web-based authentication paths at the logon screen. Default: web credential tile shown. Recommended: disabled on enterprise domain machines.",
                    Tags = ["credential", "web", "msa", "logon", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ApplyOps = [RegOp.SetDword(CredUiKey, "DisableWebCredentialUI", 1)],
                    RemoveOps = [RegOp.DeleteValue(CredUiKey, "DisableWebCredentialUI")],
                    DetectOps = [RegOp.CheckDword(CredUiKey, "DisableWebCredentialUI", 1)],
                },
            ];
    }

    // ── Sprint 671b — Credential Provider (Winlogon) Policy ──────────────────
    private static class _CredProviderPolicy
    {
        private const string ProvKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "sec-credprov-disable-domain-picker-logon",
                    Label = "Disable Domain Account Picker at Logon",
                    Category = "Security",
                    Description =
                        "Prevents the logon screen from displaying trusted domain accounts in the account picker. Eliminates enumeration of available domain accounts by any user observing the logon screen, a common information-gathering technique before lateral movement. Default: domain accounts listed. Recommended: disabled.",
                    Tags = ["credential", "domain", "logon", "enumeration", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ApplyOps = [RegOp.SetDword(ProvKey, "DontDisplayNetworkSelectionUI", 1)],
                    RemoveOps = [RegOp.DeleteValue(ProvKey, "DontDisplayNetworkSelectionUI")],
                    DetectOps = [RegOp.CheckDword(ProvKey, "DontDisplayNetworkSelectionUI", 1)],
                },
                new TweakDef
                {
                    Id = "sec-credprov-block-cached-logon-display",
                    Label = "Block Display of Cached Credentials at Logon",
                    Category = "Security",
                    Description =
                        "Prevents previously logged-on usernames from appearing on the Windows logon screen. Displaying cached usernames allows an observer to enumerate valid account names. Default: last logged-on user displayed. Recommended: hidden.",
                    Tags = ["credential", "cached", "logon", "enumeration", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ApplyOps = [RegOp.SetDword(ProvKey, "DontDisplayLastUserName", 1)],
                    RemoveOps = [RegOp.DeleteValue(ProvKey, "DontDisplayLastUserName")],
                    DetectOps = [RegOp.CheckDword(ProvKey, "DontDisplayLastUserName", 1)],
                },
                new TweakDef
                {
                    Id = "sec-credprov-disable-lock-screen-logon-last-user",
                    Label = "Block Display of Last User on Lock Screen",
                    Category = "Security",
                    Description =
                        "Hides the username and account picture of the last logged-on user on the Windows lock screen. Prevents physical observers from learning valid account names from the lock screen. Default: last user displayed. Recommended: hidden.",
                    Tags = ["credential", "lock-screen", "username", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ApplyOps = [RegOp.SetDword(ProvKey, "DontDisplayLockedUserId", 3)],
                    RemoveOps = [RegOp.DeleteValue(ProvKey, "DontDisplayLockedUserId")],
                    DetectOps = [RegOp.CheckDword(ProvKey, "DontDisplayLockedUserId", 3)],
                },
                new TweakDef
                {
                    Id = "sec-credprov-enable-logon-legal-notice",
                    Label = "Enable Legal Notice at Logon (Compliance Banner)",
                    Category = "Security",
                    Description =
                        "Displays a legal notice (warning banner) to all users before they log on. Required by many compliance frameworks (NIST 800-53 AC-8, CIS L1, STIG) to establish authorized-use policy. Default: no legal notice. Recommended: enabled with organization-specific text.",
                    Tags = ["credential", "logon", "compliance", "legal", "banner", "cis", "nist", "stig", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ApplyOps = [RegOp.SetString(ProvKey, "legalnoticecaption", "Authorized Use Only")],
                    RemoveOps = [RegOp.DeleteValue(ProvKey, "legalnoticecaption")],
                    DetectOps = [RegOp.CheckMissing(ProvKey, "legalnoticecaption")],
                },
                new TweakDef
                {
                    Id = "sec-credprov-disable-shutdown-without-logon",
                    Label = "Disable Shutdown Button on Windows Logon Screen",
                    Category = "Security",
                    Description =
                        "Removes the Shutdown button from the Windows logon/lock screen. Prevents unauthenticated users from shutting down the machine, which could interrupt services, bypass auto-start security tools, or cause data loss. Default: Shutdown button visible. Recommended: disabled on servers and shared workstations.",
                    Tags = ["logon", "shutdown", "lockscreen", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ApplyOps = [RegOp.SetDword(ProvKey, "ShutdownWithoutLogon", 0)],
                    RemoveOps = [RegOp.DeleteValue(ProvKey, "ShutdownWithoutLogon")],
                    DetectOps = [RegOp.CheckDword(ProvKey, "ShutdownWithoutLogon", 0)],
                },
            ];
    }
}
