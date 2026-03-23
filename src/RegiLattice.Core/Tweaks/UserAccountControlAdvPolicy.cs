// User Account Control Advanced Policy — Sprint 151
// Slug "uacadv" — additional UAC and logon policy values in Policies\System
// and Winlogon not already used by UserAccount.cs.
//
// HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System
// HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon
#nullable enable
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

internal static class UserAccountControlAdvPolicy
{
    private const string UacAdv =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System";

    private const string Winlogon =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "uacadv-disable-arso",
            Label = "Logon: Disable automatic restart sign-on (ARSO)",
            Category = "User Account Control Advanced Policy",
            Description =
                "Sets DisableAutomaticRestartSignOn=1 in Policies\\System. Prevents Windows from "
                + "automatically signing in and locking the last interactive user after a reboot "
                + "(e.g., triggered by a Windows Update restart).",
            Tags = ["uac", "logon", "arso", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(UacAdv, "DisableAutomaticRestartSignOn", 1)],
            RemoveOps = [RegOp.DeleteValue(UacAdv, "DisableAutomaticRestartSignOn")],
            DetectOps = [RegOp.CheckDword(UacAdv, "DisableAutomaticRestartSignOn", 1)],
        },
        new TweakDef
        {
            Id = "uacadv-hide-network-selection-ui",
            Label = "Logon: Hide the network selection UI on the sign-in screen",
            Category = "User Account Control Advanced Policy",
            Description =
                "Sets DontDisplayNetworkSelectionUI=1 in Policies\\System. Removes the Wi-Fi/network "
                + "chooser button from the Windows logon screen, preventing unauthenticated network changes.",
            Tags = ["uac", "logon", "network", "lock-screen", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(UacAdv, "DontDisplayNetworkSelectionUI", 1)],
            RemoveOps = [RegOp.DeleteValue(UacAdv, "DontDisplayNetworkSelectionUI")],
            DetectOps = [RegOp.CheckDword(UacAdv, "DontDisplayNetworkSelectionUI", 1)],
        },
        new TweakDef
        {
            Id = "uacadv-hide-failed-unlock-text",
            Label = "Logon: Hide failed-unlock notification text on lock screen",
            Category = "User Account Control Advanced Policy",
            Description =
                "Sets DontDisplayFailedUnlock=1 in Policies\\System. Suppresses the 'Your account has "
                + "been locked' / 'too many attempts' banner shown on the lock screen after failed logins.",
            Tags = ["uac", "logon", "lock-screen", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(UacAdv, "DontDisplayFailedUnlock", 1)],
            RemoveOps = [RegOp.DeleteValue(UacAdv, "DontDisplayFailedUnlock")],
            DetectOps = [RegOp.CheckDword(UacAdv, "DontDisplayFailedUnlock", 1)],
        },
        new TweakDef
        {
            Id = "uacadv-hide-locked-user-id",
            Label = "Logon: Hide user identity information on the lock screen",
            Category = "User Account Control Advanced Policy",
            Description =
                "Sets DontDisplayLockedUserId=3 in Policies\\System. Value 3 hides both user name and "
                + "email from the lock screen, preventing information disclosure of signed-in users.",
            Tags = ["uac", "logon", "lock-screen", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(UacAdv, "DontDisplayLockedUserId", 3)],
            RemoveOps = [RegOp.DeleteValue(UacAdv, "DontDisplayLockedUserId")],
            DetectOps = [RegOp.CheckDword(UacAdv, "DontDisplayLockedUserId", 3)],
        },
        new TweakDef
        {
            Id = "uacadv-require-msa-optional",
            Label = "Logon: Make Microsoft Account sign-in optional (allow local accounts)",
            Category = "User Account Control Advanced Policy",
            Description =
                "Sets MSAOptional=1 in Policies\\System. Allows users on apps and services to proceed "
                + "without a Microsoft Account when the service offers a local-account alternative.",
            Tags = ["uac", "msa", "microsoft-account", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(UacAdv, "MSAOptional", 1)],
            RemoveOps = [RegOp.DeleteValue(UacAdv, "MSAOptional")],
            DetectOps = [RegOp.CheckDword(UacAdv, "MSAOptional", 1)],
        },
        new TweakDef
        {
            Id = "uacadv-disable-shutdown-without-logon",
            Label = "Logon: Prevent shutdown from the lock screen (require logon)",
            Category = "User Account Control Advanced Policy",
            Description =
                "Sets ShutdownWithoutLogon=0 in Policies\\System. Removes the 'Shut down' button from "
                + "the Windows lock screen, requiring users to authenticate before powering off.",
            Tags = ["uac", "logon", "shutdown", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(UacAdv, "ShutdownWithoutLogon", 0)],
            RemoveOps = [RegOp.DeleteValue(UacAdv, "ShutdownWithoutLogon")],
            DetectOps = [RegOp.CheckDword(UacAdv, "ShutdownWithoutLogon", 0)],
        },
        new TweakDef
        {
            Id = "uacadv-disable-lock-workstation",
            Label = "Logon: Prevent users from locking the workstation via keyboard shortcut",
            Category = "User Account Control Advanced Policy",
            Description =
                "Sets DisableLockWorkstation=1 in Policies\\System. Disables the Win+L and "
                + "Ctrl+Alt+Del > Lock option, preventing interactive users from manually locking the PC.",
            Tags = ["uac", "logon", "lock-workstation", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(UacAdv, "DisableLockWorkstation", 1)],
            RemoveOps = [RegOp.DeleteValue(UacAdv, "DisableLockWorkstation")],
            DetectOps = [RegOp.CheckDword(UacAdv, "DisableLockWorkstation", 1)],
        },
        new TweakDef
        {
            Id = "uacadv-disable-change-password",
            Label = "Logon: Prevent users from changing their password via Ctrl+Alt+Del",
            Category = "User Account Control Advanced Policy",
            Description =
                "Sets DisableChangePassword=1 in Policies\\System. Removes the 'Change password' "
                + "option from the Ctrl+Alt+Del security screen (useful in kiosk/shared-PC scenarios).",
            Tags = ["uac", "logon", "password", "kiosk", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(UacAdv, "DisableChangePassword", 1)],
            RemoveOps = [RegOp.DeleteValue(UacAdv, "DisableChangePassword")],
            DetectOps = [RegOp.CheckDword(UacAdv, "DisableChangePassword", 1)],
        },
        new TweakDef
        {
            Id = "uacadv-legal-notice-caption",
            Label = "Logon: Set a legal notice caption on the sign-in screen",
            Category = "User Account Control Advanced Policy",
            Description =
                "Sets LegalNoticeCaption to 'Authorized Access Only' in Policies\\System. Displays a "
                + "custom caption banner on the Windows logon screen — common in corporate environments.",
            Tags = ["uac", "logon", "legal-notice", "corporate", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetString(UacAdv, "LegalNoticeCaption", "Authorized Access Only")],
            RemoveOps = [RegOp.DeleteValue(UacAdv, "LegalNoticeCaption")],
            DetectOps =
            [
                RegOp.CheckString(UacAdv, "LegalNoticeCaption", "Authorized Access Only"),
            ],
        },
        new TweakDef
        {
            Id = "uacadv-disable-task-manager",
            Label = "Logon: Disable Task Manager for non-admin users",
            Category = "User Account Control Advanced Policy",
            Description =
                "Sets DisableTaskMgr=1 in Policies\\System. Prevents standard user accounts from "
                + "opening Task Manager (Ctrl+Shift+Esc / Ctrl+Alt+Del). Admin accounts are unaffected.",
            Tags = ["uac", "task-manager", "standard-user", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(UacAdv, "DisableTaskMgr", 1)],
            RemoveOps = [RegOp.DeleteValue(UacAdv, "DisableTaskMgr")],
            DetectOps = [RegOp.CheckDword(UacAdv, "DisableTaskMgr", 1)],
        },
    ];
}
