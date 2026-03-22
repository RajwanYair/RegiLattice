// RegiLattice.Core — Tweaks/LapsSecurity.cs
// Windows LAPS (Local Administrator Password Solution) policy tweaks (Sprint 106).
// Slug: "laps" — configures Windows LAPS (built-in since April 2023 / Windows 11 22H2+).
// Distinct from Elevation.cs (UAC) and Security.cs (RunAsPPL, credential guards).
// Registry base: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LAPS
// Requires Windows 11 22H2 / Windows 10 22H2 (KB5025221) or Windows Server 2019+.

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class LapsSecurity
{
    private const string LapsPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LAPS";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "laps-backup-to-azure-ad",
            Label = "LAPS: Back Up Local Admin Password to Azure AD / Entra ID",
            Category = "Local Admin Password (LAPS)",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            ImpactScore = 4,
            SafetyRating = 4,
            Tags = ["laps", "entra-id", "azure-ad", "password-backup", "admin-password"],
            Description =
                "Sets BackupDirectory=1 in the Windows LAPS policy. "
                + "Configures Windows LAPS to back up the managed local administrator password to "
                + "Azure Active Directory (now Microsoft Entra ID). Values: 0=disabled, 1=Azure AD, 2=Active Directory. "
                + "Allows IT admins to retrieve the password via the Entra ID portal without on-premises AD infrastructure.",
            ApplyOps = [RegOp.SetDword(LapsPolicy, "BackupDirectory", 1)],
            RemoveOps = [RegOp.DeleteValue(LapsPolicy, "BackupDirectory")],
            DetectOps = [RegOp.CheckDword(LapsPolicy, "BackupDirectory", 1)],
        },
        new TweakDef
        {
            Id = "laps-backup-to-ad",
            Label = "LAPS: Back Up Local Admin Password to Active Directory",
            Category = "Local Admin Password (LAPS)",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            ImpactScore = 4,
            SafetyRating = 4,
            Tags = ["laps", "active-directory", "password-backup", "admin-password", "on-premises"],
            Description =
                "Sets BackupDirectory=2 in the Windows LAPS policy. "
                + "Configures Windows LAPS to back up the managed local administrator password to "
                + "on-premises Active Directory. Values: 0=disabled, 1=Azure AD/Entra, 2=AD on-premises. "
                + "Requires the AD schema to be extended for Windows LAPS (distinct from legacy Microsoft LAPS).",
            ApplyOps = [RegOp.SetDword(LapsPolicy, "BackupDirectory", 2)],
            RemoveOps = [RegOp.DeleteValue(LapsPolicy, "BackupDirectory")],
            DetectOps = [RegOp.CheckDword(LapsPolicy, "BackupDirectory", 2)],
        },
        new TweakDef
        {
            Id = "laps-max-age-14-days",
            Label = "LAPS: Set Maximum Password Age to 14 Days",
            Category = "Local Admin Password (LAPS)",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            ImpactScore = 3,
            SafetyRating = 4,
            Tags = ["laps", "password-age", "rotation", "admin-password"],
            Description =
                "Sets PasswordAgeDays=14 in the Windows LAPS policy. "
                + "Requires the local administrator password to be rotated at least every 14 days. "
                + "The default is 30 days. Shorter rotation limits the window of exposure if a cached/leaked "
                + "password is used in a pass-the-hash or lateral movement attack.",
            ApplyOps = [RegOp.SetDword(LapsPolicy, "PasswordAgeDays", 14)],
            RemoveOps = [RegOp.DeleteValue(LapsPolicy, "PasswordAgeDays")],
            DetectOps = [RegOp.CheckDword(LapsPolicy, "PasswordAgeDays", 14)],
        },
        new TweakDef
        {
            Id = "laps-password-length-20",
            Label = "LAPS: Set Minimum Password Length to 20 Characters",
            Category = "Local Admin Password (LAPS)",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            ImpactScore = 3,
            SafetyRating = 4,
            Tags = ["laps", "password-length", "strength", "admin-password"],
            Description =
                "Sets PasswordLength=20 in the Windows LAPS policy. "
                + "Requires the generated local administrator password to be at least 20 characters long. "
                + "The default is 14 characters. At 20 characters with mixed complexity, the password provides "
                + "over 100 bits of entropy, making offline brute-force attacks computationally infeasible.",
            ApplyOps = [RegOp.SetDword(LapsPolicy, "PasswordLength", 20)],
            RemoveOps = [RegOp.DeleteValue(LapsPolicy, "PasswordLength")],
            DetectOps = [RegOp.CheckDword(LapsPolicy, "PasswordLength", 20)],
        },
        new TweakDef
        {
            Id = "laps-password-max-complexity",
            Label = "LAPS: Require Maximum Password Complexity (All Character Types)",
            Category = "Local Admin Password (LAPS)",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            ImpactScore = 3,
            SafetyRating = 4,
            Tags = ["laps", "password-complexity", "strength", "admin-password"],
            Description =
                "Sets PasswordComplexity=4 in the Windows LAPS policy. "
                + "Requires the generated password to include large letters, small letters, numbers, and "
                + "special characters. Values: 1=large only, 2=large+small, 3=large+small+numbers, "
                + "4=large+small+numbers+specials (maximum complexity).",
            ApplyOps = [RegOp.SetDword(LapsPolicy, "PasswordComplexity", 4)],
            RemoveOps = [RegOp.DeleteValue(LapsPolicy, "PasswordComplexity")],
            DetectOps = [RegOp.CheckDword(LapsPolicy, "PasswordComplexity", 4)],
        },
        new TweakDef
        {
            Id = "laps-enable-password-encryption",
            Label = "LAPS: Enable Encrypted Password Storage in Active Directory",
            Category = "Local Admin Password (LAPS)",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            ImpactScore = 4,
            SafetyRating = 4,
            Tags = ["laps", "encryption", "active-directory", "password-storage"],
            Description =
                "Sets ADPasswordEncryptionEnabled=1 in the Windows LAPS policy. "
                + "Encrypts the LAPS password before it is stored in Active Directory, using the AD computer "
                + "object's NTDS DPAPI master key. Only authorized AD principals (admins, the computer itself) "
                + "can decrypt the password. Requires on-premises AD with the Windows LAPS schema extension.",
            ApplyOps = [RegOp.SetDword(LapsPolicy, "ADPasswordEncryptionEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(LapsPolicy, "ADPasswordEncryptionEnabled")],
            DetectOps = [RegOp.CheckDword(LapsPolicy, "ADPasswordEncryptionEnabled", 1)],
        },
        new TweakDef
        {
            Id = "laps-post-auth-reset-and-logoff",
            Label = "LAPS: Reset Password and Log Off After Admin Authentication",
            Category = "Local Admin Password (LAPS)",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            ImpactScore = 3,
            SafetyRating = 4,
            Tags = ["laps", "post-auth", "password-reset", "logoff", "hygiene"],
            Description =
                "Sets PostAuthenticationActions=3 in the Windows LAPS policy. "
                + "After the local admin account is used to authenticate (e.g., for a break-glass login), "
                + "LAPS automatically resets the password AND logs off active sessions. "
                + "Values: 1=reset password only, 2=logoff+reset, 3=logoff+reset+terminate processes.",
            ApplyOps = [RegOp.SetDword(LapsPolicy, "PostAuthenticationActions", 3)],
            RemoveOps = [RegOp.DeleteValue(LapsPolicy, "PostAuthenticationActions")],
            DetectOps = [RegOp.CheckDword(LapsPolicy, "PostAuthenticationActions", 3)],
        },
        new TweakDef
        {
            Id = "laps-post-auth-delay-24h",
            Label = "LAPS: Set 24-Hour Grace Period Before Post-Auth Password Reset",
            Category = "Local Admin Password (LAPS)",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            ImpactScore = 2,
            SafetyRating = 4,
            Tags = ["laps", "post-auth", "delay", "grace-period"],
            Description =
                "Sets PostAuthenticationResetDelay=24 in the Windows LAPS policy. "
                + "Specifies how many hours Windows waits after an authentication event before triggering "
                + "the post-authentication password rotation action. "
                + "A 24-hour delay gives administrators time to complete maintenance work before the "
                + "account is reset and active sessions are terminated.",
            ApplyOps = [RegOp.SetDword(LapsPolicy, "PostAuthenticationResetDelay", 24)],
            RemoveOps = [RegOp.DeleteValue(LapsPolicy, "PostAuthenticationResetDelay")],
            DetectOps = [RegOp.CheckDword(LapsPolicy, "PostAuthenticationResetDelay", 24)],
        },
        new TweakDef
        {
            Id = "laps-encrypt-history-12",
            Label = "LAPS: Retain 12 Encrypted Previous Passwords in AD History",
            Category = "Local Admin Password (LAPS)",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            ImpactScore = 2,
            SafetyRating = 4,
            Tags = ["laps", "history", "encryption", "active-directory", "audit"],
            Description =
                "Sets ADEncryptedPasswordHistorySize=12 in the Windows LAPS policy. "
                + "Configures Windows LAPS to retain the last 12 encrypted historical passwords in "
                + "Active Directory. Enables recovery of previously used passwords for forensic analysis "
                + "or rolling back after an incident. Requires ADPasswordEncryptionEnabled=1.",
            ApplyOps = [RegOp.SetDword(LapsPolicy, "ADEncryptedPasswordHistorySize", 12)],
            RemoveOps = [RegOp.DeleteValue(LapsPolicy, "ADEncryptedPasswordHistorySize")],
            DetectOps = [RegOp.CheckDword(LapsPolicy, "ADEncryptedPasswordHistorySize", 12)],
        },
        new TweakDef
        {
            Id = "laps-disable-legacy-laps",
            Label = "LAPS: Disable Legacy Microsoft LAPS (Allow Only Windows LAPS)",
            Category = "Local Admin Password (LAPS)",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            ImpactScore = 3,
            SafetyRating = 4,
            Tags = ["laps", "legacy", "migration", "admin-password"],
            Description =
                "Sets LegacyMicrosoftLAPSEnabled=0 in the Windows LAPS policy. "
                + "Disables the legacy Microsoft LAPS CSE (Client-Side Extension) when the built-in Windows "
                + "LAPS is configured. Prevents both legacy and new LAPS from running simultaneously, which "
                + "could cause password conflicts. Required when migrating from legacy LAPS to Windows LAPS.",
            ApplyOps = [RegOp.SetDword(LapsPolicy, "LegacyMicrosoftLAPSEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(LapsPolicy, "LegacyMicrosoftLAPSEnabled")],
            DetectOps = [RegOp.CheckDword(LapsPolicy, "LegacyMicrosoftLAPSEnabled", 0)],
        },
    ];
}
