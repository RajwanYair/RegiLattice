// RegiLattice.Core — Tweaks/MicrosoftAccount.cs
// Microsoft Account sync, integration, and credential settings (Sprint 90).
// Slug "msa" — HKLM/HKCU MSA, Passport, sync-on-device settings.
// Distinct from Privacy.cs and Identity-related security settings.

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class MicrosoftAccount
{
    private const string MsaPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PolicyManager\current\device\Accounts";

    private const string WinLogon = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon";

    private const string SyncPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SettingSync";

    private const string PassportPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PassportForWork";

    private const string SignIn = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System";

    private const string MsaUserPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Cloud";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "msa-disable-account-sync",
            Label = "Disable Microsoft Account Settings Sync",
            Category = "Microsoft Account",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 3,
            SafetyRating = 4,
            Tags = ["msa", "sync", "settings", "privacy"],
            Description =
                "Disables the Sync Your Settings feature (DisableSettingSync=2). "
                + "Prevents Windows settings like theme, passwords, and language preferences "
                + "from being uploaded to and synced through your Microsoft Account.",
            ApplyOps = [RegOp.SetDword(SyncPolicy, "DisableSettingSync", 2)],
            RemoveOps = [RegOp.DeleteValue(SyncPolicy, "DisableSettingSync")],
            DetectOps = [RegOp.CheckDword(SyncPolicy, "DisableSettingSync", 2)],
        },
        new TweakDef
        {
            Id = "msa-disable-sync-override",
            Label = "Prevent Users from Overriding Settings Sync Policy",
            Category = "Microsoft Account",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 3,
            SafetyRating = 4,
            Tags = ["msa", "sync", "policy", "lock"],
            Description =
                "Sets DisableSettingSyncUserOverride=1 to prevent users from re-enabling "
                + "Settings Sync through the Settings app. Complements DisableSettingSync.",
            ApplyOps = [RegOp.SetDword(SyncPolicy, "DisableSettingSyncUserOverride", 1)],
            RemoveOps = [RegOp.DeleteValue(SyncPolicy, "DisableSettingSyncUserOverride")],
            DetectOps = [RegOp.CheckDword(SyncPolicy, "DisableSettingSyncUserOverride", 1)],
        },
        new TweakDef
        {
            Id = "msa-block-msa-signin",
            Label = "Block Microsoft Account Sign-In for Apps",
            Category = "Microsoft Account",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 3,
            Tags = ["msa", "sign-in", "block", "policy"],
            Description =
                "Blocks apps and services from using Microsoft Account for authentication "
                + "(NoConnectedUser=3). Suitable for managed corporate environments where "
                + "all identity is managed by Azure AD or on-premises AD. "
                + "WARNING: UWP app sign-in and Xbox will break.",
            ApplyOps = [RegOp.SetDword(SignIn, "NoConnectedUser", 3)],
            RemoveOps = [RegOp.DeleteValue(SignIn, "NoConnectedUser")],
            DetectOps = [RegOp.CheckDword(SignIn, "NoConnectedUser", 3)],
        },
        new TweakDef
        {
            Id = "msa-disable-windows-hello-provision",
            Label = "Disable Windows Hello Provisioning (Local Account)",
            Category = "Microsoft Account",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Tags = ["msa", "windows hello", "provisioning", "pin", "policy"],
            Description =
                "Prevents Windows from requiring and prompting users to set up a Windows "
                + "Hello PIN or biometric. Enabled=0 disables the PassportForWork policy. "
                + "Suitable for shared PCs or environments using passwords only.",
            ApplyOps = [RegOp.SetDword(PassportPolicy, "Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(PassportPolicy, "Enabled")],
            DetectOps = [RegOp.CheckDword(PassportPolicy, "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "msa-disable-optional-diagnostic-sync",
            Label = "Disable Optional Diagnostic Data Sync via MSA",
            Category = "Microsoft Account",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["msa", "diagnostic", "sync", "privacy", "telemetry"],
            Description =
                "Disables the optional background upload of diagnostic/telemetry data "
                + "associated with a signed-in Microsoft Account. Complements the main "
                + "telemetry disable tweak with an MSA-scoped opt-out.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "AllowDeviceNameInTelemetry", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "AllowDeviceNameInTelemetry")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "AllowDeviceNameInTelemetry", 0)],
        },
        new TweakDef
        {
            Id = "msa-disable-app-access-account-info",
            Label = "Disable App Access to Account Information (Privacy)",
            Category = "Microsoft Account",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["msa", "account info", "privacy", "app access"],
            Description =
                "Revokes app permission to read your account info (name, picture, username). "
                + "Prevents UWP apps from accessing account details without explicit consent. "
                + "UserConsent=Deny via the privacy capability setting.",
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\DeviceAccess\Global\{C1D23ACC-752B-43E5-8448-8D0E519CD6D6}",
                    "Value",
                    "Deny"
                ),
            ],
            RemoveOps =
            [
                RegOp.SetString(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\DeviceAccess\Global\{C1D23ACC-752B-43E5-8448-8D0E519CD6D6}",
                    "Value",
                    "Allow"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\DeviceAccess\Global\{C1D23ACC-752B-43E5-8448-8D0E519CD6D6}",
                    "Value",
                    "Deny"
                ),
            ],
        },
        new TweakDef
        {
            Id = "msa-disable-suggested-apps-msa",
            Label = "Disable MSA-Based Suggested Apps and Content",
            Category = "Microsoft Account",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["msa", "suggested apps", "ads", "privacy"],
            Description =
                "Disables personalised app and content suggestions delivered through "
                + "the Microsoft Account integration (CloudContent\\DisableSoftLanding=1). "
                + "Stops account-based promotional content in the Start menu and apps.",
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SilentInstalledAppsEnabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SilentInstalledAppsEnabled",
                    1
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SilentInstalledAppsEnabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "msa-disable-theme-sync",
            Label = "Disable Theme Sync via Microsoft Account",
            Category = "Microsoft Account",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Tags = ["msa", "theme", "sync", "personalization"],
            Description =
                "Stops Windows from syncing the desktop theme (wallpaper, colours, sounds) "
                + "across devices linked to the same Microsoft Account. "
                + "DisableThemeSettingSync=1.",
            ApplyOps = [RegOp.SetDword(SyncPolicy, "DisableThemeSettingSync", 1)],
            RemoveOps = [RegOp.DeleteValue(SyncPolicy, "DisableThemeSettingSync")],
            DetectOps = [RegOp.CheckDword(SyncPolicy, "DisableThemeSettingSync", 1)],
        },
        new TweakDef
        {
            Id = "msa-disable-password-sync",
            Label = "Disable Password Sync via Microsoft Account",
            Category = "Microsoft Account",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Tags = ["msa", "password", "sync", "credential", "security"],
            Description =
                "Disables syncing of saved app and website passwords through the "
                + "Microsoft Account sync channel. Prevents credentials from being "
                + "transmitted to Microsoft servers. DisableCredentialsSettingSync=1.",
            ApplyOps = [RegOp.SetDword(SyncPolicy, "DisableCredentialsSettingSync", 1)],
            RemoveOps = [RegOp.DeleteValue(SyncPolicy, "DisableCredentialsSettingSync")],
            DetectOps = [RegOp.CheckDword(SyncPolicy, "DisableCredentialsSettingSync", 1)],
        },
        new TweakDef
        {
            Id = "msa-disable-app-settings-sync",
            Label = "Disable Per-App Settings Sync via Microsoft Account",
            Category = "Microsoft Account",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["msa", "app settings", "sync", "store apps"],
            Description =
                "Disables per-app settings sync so individual app configurations (like "
                + "UWP app preferences) are not uploaded and synced through the Microsoft "
                + "Account. DisableApplicationSettingSync=1.",
            ApplyOps = [RegOp.SetDword(SyncPolicy, "DisableApplicationSettingSync", 1)],
            RemoveOps = [RegOp.DeleteValue(SyncPolicy, "DisableApplicationSettingSync")],
            DetectOps = [RegOp.CheckDword(SyncPolicy, "DisableApplicationSettingSync", 1)],
        },
    ];
}
