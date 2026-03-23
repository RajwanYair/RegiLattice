#nullable enable
using RegiLattice.Core.Models;
using System.Collections.Generic;

namespace RegiLattice.Core.Tweaks;

// Slug "logonpol" — Windows logon screen privacy and security GPO policy.
// SOFTWARE\Policies\Microsoft\Windows\System (logon-related keys)
// Distinct from LockScreen.cs (which covers screensaver/timeout/HKCU keys).
internal static class LogonGpoPolicy
{
    private const string LogonSys =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "logonpol-hide-last-username",
            Label = "Hide last signed-in username at logon screen (policy)",
            Category = "Logon Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents the last interactive user's username from being displayed on the logon screen. "
                + "DontDisplayLastUserName=1. Protects user enumeration on shared/kiosk machines.",
            Tags = ["logon", "privacy", "username", "policy"],
            ApplyOps = [RegOp.SetDword(LogonSys, "DontDisplayLastUserName", 1)],
            RemoveOps = [RegOp.DeleteValue(LogonSys, "DontDisplayLastUserName")],
            DetectOps = [RegOp.CheckDword(LogonSys, "DontDisplayLastUserName", 1)],
        },
        new TweakDef
        {
            Id = "logonpol-hide-network-selection",
            Label = "Hide network selection UI at logon screen (policy)",
            Category = "Logon Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Removes the network selection widget from the logon screen, preventing network changes before sign-in. "
                + "DontDisplayNetworkSelectionUI=1. Reduces attack surface on shared machines.",
            Tags = ["logon", "network", "ui", "policy"],
            ApplyOps = [RegOp.SetDword(LogonSys, "DontDisplayNetworkSelectionUI", 1)],
            RemoveOps = [RegOp.DeleteValue(LogonSys, "DontDisplayNetworkSelectionUI")],
            DetectOps = [RegOp.CheckDword(LogonSys, "DontDisplayNetworkSelectionUI", 1)],
        },
        new TweakDef
        {
            Id = "logonpol-hide-account-details-on-signin",
            Label = "Block users from showing account details at sign-in (policy)",
            Category = "Logon Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents users from showing account details (email/username) on the sign-in screen. "
                + "BlockUserFromShowingAccountDetailsOnSignin=1. Reduces personal data exposure.",
            Tags = ["logon", "account", "privacy", "policy"],
            ApplyOps = [RegOp.SetDword(LogonSys, "BlockUserFromShowingAccountDetailsOnSignin", 1)],
            RemoveOps = [RegOp.DeleteValue(LogonSys, "BlockUserFromShowingAccountDetailsOnSignin")],
            DetectOps = [RegOp.CheckDword(LogonSys, "BlockUserFromShowingAccountDetailsOnSignin", 1)],
        },
        new TweakDef
        {
            Id = "logonpol-disable-arso",
            Label = "Disable Automatic Restart Sign-On (ARSO) (policy)",
            Category = "Logon Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Windows Automatic Restart Sign-On, which re-signs in the last user after an update restart. "
                + "DisableAutomaticRestartSignOn=1. Prevents unattended desktop exposure after reboot.",
            Tags = ["logon", "arso", "restart", "autologon", "policy"],
            ApplyOps = [RegOp.SetDword(LogonSys, "DisableAutomaticRestartSignOn", 1)],
            RemoveOps = [RegOp.DeleteValue(LogonSys, "DisableAutomaticRestartSignOn")],
            DetectOps = [RegOp.CheckDword(LogonSys, "DisableAutomaticRestartSignOn", 1)],
        },
        new TweakDef
        {
            Id = "logonpol-disable-startup-sound",
            Label = "Disable Windows startup sound (policy)",
            Category = "Logon Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Windows startup sound via policy, regardless of user sound settings. "
                + "DisableStartupSound=1. Useful in enterprise/kiosk environments.",
            Tags = ["logon", "startup", "sound", "policy"],
            ApplyOps = [RegOp.SetDword(LogonSys, "DisableStartupSound", 1)],
            RemoveOps = [RegOp.DeleteValue(LogonSys, "DisableStartupSound")],
            DetectOps = [RegOp.CheckDword(LogonSys, "DisableStartupSound", 1)],
        },
        new TweakDef
        {
            Id = "logonpol-block-msa-connected-account",
            Label = "Block Microsoft Account connected users (policy)",
            Category = "Logon Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents Microsoft Account-connected users from signing in. "
                + "NoConnectedUser=3 (block all MSA users). Values: 0=allowed, 1=no new MSA, 3=block all MSA.",
            Tags = ["logon", "msa", "account", "policy"],
            ApplyOps = [RegOp.SetDword(LogonSys, "NoConnectedUser", 3)],
            RemoveOps = [RegOp.DeleteValue(LogonSys, "NoConnectedUser")],
            DetectOps = [RegOp.CheckDword(LogonSys, "NoConnectedUser", 3)],
        },
        new TweakDef
        {
            Id = "logonpol-hide-locked-user-id",
            Label = "Hide locked user info on the lock screen (policy)",
            Category = "Logon Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Controls what user information is displayed when a session is locked. "
                + "DontDisplayLockedUserId=3 (show nothing: display name, domain, and username hidden). "
                + "Values: 1=display name only, 2=display name+domain, 3=nothing.",
            Tags = ["logon", "lock-screen", "privacy", "policy"],
            ApplyOps = [RegOp.SetDword(LogonSys, "DontDisplayLockedUserId", 3)],
            RemoveOps = [RegOp.DeleteValue(LogonSys, "DontDisplayLockedUserId")],
            DetectOps = [RegOp.CheckDword(LogonSys, "DontDisplayLockedUserId", 3)],
        },
        new TweakDef
        {
            Id = "logonpol-max-device-password-failed-attempts",
            Label = "Lock device after failed sign-in attempts (policy)",
            Category = "Logon Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Triggers a device lockout after a specified number of failed sign-in attempts. "
                + "MaxDevicePasswordFailedAttempts=10. Default: 0 (disabled). "
                + "Activates on tablets/convertibles with BitLocker.",
            Tags = ["logon", "lockout", "password", "policy"],
            ApplyOps = [RegOp.SetDword(LogonSys, "MaxDevicePasswordFailedAttempts", 10)],
            RemoveOps = [RegOp.DeleteValue(LogonSys, "MaxDevicePasswordFailedAttempts")],
            DetectOps = [RegOp.CheckDword(LogonSys, "MaxDevicePasswordFailedAttempts", 10)],
        },
        new TweakDef
        {
            Id = "logonpol-disable-lock-screen-app-notifications",
            Label = "Disable app notifications on the lock screen (policy)",
            Category = "Logon Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents application notifications from appearing on the lock screen. "
                + "DisableLockScreenAppNotifications=1. Default: not set. "
                + "Reduces information disclosure before authentication.",
            Tags = ["logon", "lock-screen", "notifications", "privacy", "policy"],
            ApplyOps = [RegOp.SetDword(LogonSys, "DisableLockScreenAppNotifications", 1)],
            RemoveOps = [RegOp.DeleteValue(LogonSys, "DisableLockScreenAppNotifications")],
            DetectOps = [RegOp.CheckDword(LogonSys, "DisableLockScreenAppNotifications", 1)],
        },
        new TweakDef
        {
            Id = "logonpol-hide-power-button-at-logon",
            Label = "Hide power button on logon screen (policy)",
            Category = "Logon Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Removes the Shut Down/Restart button from the Windows logon screen. "
                + "HideFastUserSwitching=0 (keep switching; this key is separate). "
                + "PowerButtonDenied=1 prevents shutdown before sign-in on kiosk machines.",
            Tags = ["logon", "power", "kiosk", "policy"],
            ApplyOps = [RegOp.SetDword(LogonSys, "PowerButtonDenied", 1)],
            RemoveOps = [RegOp.DeleteValue(LogonSys, "PowerButtonDenied")],
            DetectOps = [RegOp.CheckDword(LogonSys, "PowerButtonDenied", 1)],
        },
    ];
}
