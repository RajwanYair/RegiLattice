// WindowsLogonOptionsPolicy.cs — Logon UI behavior: legal notice, last user, password reveal
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows NT\CurrentVersion\Winlogon
// Slug: wlogon
// Category: Windows Logon Options Policy

namespace RegiLattice.Core.Tweaks;

using System.Collections.Generic;
using RegiLattice.Core.Models;

internal static class WindowsLogonOptionsPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\CurrentVersion\Winlogon";
    private const string SysKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "wlogon-disable-last-username-display",
            Label = "Windows Logon Options: Do Not Display Last Signed-In Username",
            Category = "Windows Logon Options Policy",
            Description =
                "Prevents the logon screen from pre-filling or displaying the last signed-in user's username. "
                + "Displaying the last username reduces the effort required for an attacker with physical access to attempt credential attacks. "
                + "With this policy set, the username field is blank and the user must type their full UPN or samAccountName. "
                + "Removing this policy restores pre-filled last-username display on the logon screen.",
            Tags = ["logon", "username", "screen", "privacy", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DontDisplayLastUserName", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DontDisplayLastUserName")],
            DetectOps = [RegOp.CheckDword(Key, "DontDisplayLastUserName", 1)],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Clears pre-filled username on logon screen; reduces account enumeration risk.",
        },
        new TweakDef
        {
            Id = "wlogon-disable-last-user-account-logon-info",
            Label = "Windows Logon Options: Do Not Display Last Account Info at Logon",
            Category = "Windows Logon Options Policy",
            Description =
                "Prevents the logon screen from displaying account information from the last successfully logged-on user. "
                + "This includes not showing the account name, domain, and display picture associated with the previous session. "
                + "Required by CIS Benchmark Level 1 for interactive logon hardening on domain or workgroup endpoints. "
                + "Removing this policy restores last account display on the logon screen.",
            Tags = ["logon", "account-info", "cis", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DontDisplayLockedUserId", 3)],
            RemoveOps = [RegOp.DeleteValue(Key, "DontDisplayLockedUserId")],
            DetectOps = [RegOp.CheckDword(Key, "DontDisplayLockedUserId", 3)],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Hides account info on locked screen; prevents account enumeration via UI.",
        },
        new TweakDef
        {
            Id = "wlogon-require-ctrl-alt-del",
            Label = "Windows Logon Options: Require Ctrl+Alt+Del Secure Attention Sequence",
            Category = "Windows Logon Options Policy",
            Description =
                "Forces users to press Ctrl+Alt+Del before entering credentials on the logon screen. "
                + "The Ctrl+Alt+Del Secure Attention Sequence (SAS) is a trusted OS-level signal that cannot be intercepted by malware. "
                + "Disabling it allows fake logon screens created by trojans to capture credentials without triggering the SAS guard. "
                + "Removing this policy makes Ctrl+Alt+Del optional (default consumer behavior).",
            Tags = ["logon", "ctrl-alt-del", "secure-attention", "credential", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableCAD", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableCAD")],
            DetectOps = [RegOp.CheckDword(Key, "DisableCAD", 0)],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Enforces SAS keystroke before logon; blocks fake credential capture screens.",
        },
        new TweakDef
        {
            Id = "wlogon-disable-password-reveal-button",
            Label = "Windows Logon Options: Disable Password Reveal Button",
            Category = "Windows Logon Options Policy",
            Description =
                "Removes the password reveal (eye icon) button from password fields on the logon screen and credential dialogs. "
                + "The reveal button is a usability feature but it creates shoulder-surfing risk in shared or open-plan environments. "
                + "Disabling it prevents bystanders from using the button to glimpse passwords when the user unlocks the screen. "
                + "Removing this policy restores the password reveal button.",
            Tags = ["logon", "password-reveal", "shoulder-surfing", "credential", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisablePasswordReveal", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisablePasswordReveal")],
            DetectOps = [RegOp.CheckDword(Key, "DisablePasswordReveal", 1)],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Hides password reveal button; reduces shoulder-surfing risk on shared workstations.",
        },
        new TweakDef
        {
            Id = "wlogon-set-legal-notice-caption",
            Label = "Windows Logon Options: Set Legal Notice Banner Caption",
            Category = "Windows Logon Options Policy",
            Description =
                "Sets the caption text for the legal notice dialog shown before Windows logon. "
                + "Displaying a legal notice at logon is a common compliance requirement that informs users the system is monitored and for authorized use only. "
                + "The caption is the title bar text of the notice dialog (typically 'Authorized Use Only' or similar). "
                + "Removing this policy clears the legal notice dialog if no text is configured.",
            Tags = ["logon", "legal-notice", "compliance", "banner", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetString(Key, "LegalNoticeCaption", "Authorized Access Only")],
            RemoveOps = [RegOp.DeleteValue(Key, "LegalNoticeCaption")],
            DetectOps = [RegOp.CheckString(Key, "LegalNoticeCaption", "Authorized Access Only")],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Displays legal notice caption on logon; satisfies compliance banner requirements.",
        },
        new TweakDef
        {
            Id = "wlogon-set-legal-notice-text",
            Label = "Windows Logon Options: Set Legal Notice Banner Body Text",
            Category = "Windows Logon Options Policy",
            Description =
                "Sets the body text content of the legal notice dialog shown before Windows logon. "
                + "Legal notice text should convey that the system is for authorized users only, activity is monitored, and unauthorized access is prohibited. "
                + "Many compliance frameworks (PCI-DSS, HIPAA, NIST) require this logon warning. "
                + "Removing this policy clears the notice body text; the dialog no longer appears if both caption and text are absent.",
            Tags = ["logon", "legal-notice", "compliance", "text", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps =
            [
                RegOp.SetString(
                    Key,
                    "LegalNoticeText",
                    "This system is for authorized users only. All activity is monitored and logged. Unauthorized access is prohibited."
                ),
            ],
            RemoveOps = [RegOp.DeleteValue(Key, "LegalNoticeText")],
            DetectOps =
            [
                RegOp.CheckString(
                    Key,
                    "LegalNoticeText",
                    "This system is for authorized users only. All activity is monitored and logged. Unauthorized access is prohibited."
                ),
            ],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Displays legal warning at logon; required by PCI-DSS, HIPAA, and NIST frameworks.",
        },
        new TweakDef
        {
            Id = "wlogon-disable-fast-user-switching",
            Label = "Windows Logon Options: Disable Fast User Switching",
            Category = "Windows Logon Options Policy",
            Description =
                "Prevents multiple user sessions from being simultaneously active via fast user switching. "
                + "Fast user switching allows a second user to log on without the first user logging off, leaving their session unlocked in memory. "
                + "This increases attack surface and can violate compliance policies requiring single-session workstations. "
                + "Removing this policy re-enables fast user switching.",
            Tags = ["logon", "fast-user-switching", "session", "compliance", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [SysKey],
            ApplyOps = [RegOp.SetDword(SysKey, "HideFastUserSwitching", 1)],
            RemoveOps = [RegOp.DeleteValue(SysKey, "HideFastUserSwitching")],
            DetectOps = [RegOp.CheckDword(SysKey, "HideFastUserSwitching", 1)],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Disables fast user switching; enforces single-user session workstation compliance.",
        },
        new TweakDef
        {
            Id = "wlogon-disable-unlocking-from-non-domain-context",
            Label = "Windows Logon Options: Require Domain Logon to Unlock Machine",
            Category = "Windows Logon Options Policy",
            Description =
                "Prevents users from unlocking a locked workstation using a local (non-domain) account. "
                + "When enabled, only domain accounts can unlock the session, preventing an attacker from using a local account to bypass domain authentication. "
                + "Best practice on domain-joined machines is to ensure the locked screen can only be cleared with domain credentials. "
                + "Removing this policy allows local account unlocking of locked domain sessions.",
            Tags = ["logon", "unlock", "domain", "local-account", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "ForceUnlockLogon", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "ForceUnlockLogon")],
            DetectOps = [RegOp.CheckDword(Key, "ForceUnlockLogon", 1)],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Requires domain credentials to unlock; prevents local-account session bypass.",
        },
        new TweakDef
        {
            Id = "wlogon-set-machine-inactivity-limit",
            Label = "Windows Logon Options: Set Machine Inactivity Limit (15 min)",
            Category = "Windows Logon Options Policy",
            Description =
                "Configures a machine-scope inactivity timeout of 15 minutes after which the screen locks automatically. "
                + "This policy is evaluated at the OS level and overrides user-configured screen saver delays. "
                + "A 15-minute inactivity limit is the CIS Benchmark L1 recommendation for workstation endpoint hardening. "
                + "Removing this policy removes the machine-scope inactivity timeout.",
            Tags = ["logon", "inactivity", "lock", "cis", "timeout", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "InactivityTimeoutSecs", 900)],
            RemoveOps = [RegOp.DeleteValue(Key, "InactivityTimeoutSecs")],
            DetectOps = [RegOp.CheckDword(Key, "InactivityTimeoutSecs", 900)],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Forces screen lock after 15 min idle; prevents unattended access on shared workstations.",
        },
        new TweakDef
        {
            Id = "wlogon-disable-smart-card-removal-behavior-none",
            Label = "Windows Logon Options: Lock on Smart Card Removal",
            Category = "Windows Logon Options Policy",
            Description =
                "Configures the system to lock the workstation when the smart card is removed from the reader. "
                + "For environments using smart-card-based authentication (PIV, CAC), removing the card should immediately secure the session. "
                + "Setting this to lock (value 1) prevents the workstation from remaining unlocked when the physical credential is withdrawn. "
                + "Removing this policy reverts to the default behavior (no action on card removal).",
            Tags = ["logon", "smart-card", "physical-security", "lock", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "ScRemoveOption", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "ScRemoveOption")],
            DetectOps = [RegOp.CheckDword(Key, "ScRemoveOption", 1)],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Locks workstation on smart card removal; prevents unattended session access.",
        },
    ];
}
