namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static partial class PolicySecurityHardening
{
    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            .. _AccountLockoutPolicy.Data,
            .. _AccountProtection.Data,
            .. _BuiltinAdminPolicy.Data,
            .. _CameraPrivacyPolicy.Data,
            .. _CapabilityAccessPolicy.Data,
            .. _DcomSecurityPolicy.Data,
            .. _HealthAttestationPolicy.Data,
            .. _IisHardeningPolicy.Data,
            .. _LockdownBrowsingPolicy.Data,
            .. _MessagingSecurityPolicy.Data,
            .. _NtlmAuthentication.Data,
            .. _NtlmAuthenticationPolicy.Data,
            .. _NtlmAuthPolicy.Data,
            .. _NtlmRestrictionPolicy.Data,
            .. _ProcessMitigationPolicy.Data,
            .. _SecureChannelPolicy.Data,
            .. _SecureConnectionPolicy.Data,
            .. _SecureLaunchDrtmPolicy.Data,
            .. _SecurityCenterPolicy.Data,
            .. _ServiceAccountPolicy.Data,
            .. _SystemGuardRuntimePolicy.Data,
            .. _TaskSchedulerSecurityPolicy.Data,
            .. _TokenBrokerPolicy.Data,
            .. _TokenPrivilegePolicy.Data,
            .. _TpmAdvancedPolicy.Data,
            .. _TpmAttestationPolicy.Data,
            .. _TpmRecoveryPolicy.Data,
            .. _TpmSecurityPolicy.Data,
            .. _TrustProviderPolicy.Data,
            .. _UserAccountControlAdvPolicy.Data,
            .. _UserProfilePolicy.Data,
            .. _UserProfilesPolicy.Data,
            .. _UserRightsPolicy.Data,
            .. _WindowsAttachmentsPolicy.Data,
            .. _WindowsEventLogAccessPolicy.Data,
        ];

    // ── AccountLockoutPolicy ──
    private static class _AccountLockoutPolicy
    {
        private const string LsaKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";
        private const string WinlogonKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon";
        private const string LockoutKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\RemoteAccess\Parameters\AccountLockout";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "acctlkout-disable-automatic-admin-logon",
                Label = "Account Lockout Policy: Disable Automatic Administrator Logon",
                Category = "Security — Account Lockout",
                Description =
                    "Clears the AutoAdminLogon registry value that enables Windows to log on automatically with saved administrator credentials. Autologon with elevated credentials bypasses all logon security and allows anyone with physical access to the machine to gain an admin session simply by rebooting.",
                Tags = ["logon", "autologon", "admin", "lockout", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [WinlogonKey],
                ApplyOps = [RegOp.SetString(WinlogonKey, "AutoAdminLogon", "0")],
                RemoveOps = [RegOp.DeleteValue(WinlogonKey, "AutoAdminLogon")],
                DetectOps = [RegOp.CheckString(WinlogonKey, "AutoAdminLogon", "0")],
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Disables auto-login; anyone who can reboot will not get an automatic admin session.",
            },
            new TweakDef
            {
                Id = "acctlkout-limit-ras-lockout-count",
                Label = "Account Lockout Policy: Limit RAS Account Lockout Attempts to 3",
                Category = "Security — Account Lockout",
                Description =
                    "Sets the Remote Access Service (RAS/VPN) account lockout threshold to 3 failed authentication attempts. Without a RAS lockout threshold, attackers can brute-force VPN credentials indefinitely without triggering a local account lockout. Applies only to RAS/VPN dial-in connections.",
                Tags = ["ras", "vpn", "lockout", "brute force", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [LockoutKey],
                ApplyOps = [RegOp.SetDword(LockoutKey, "MaxDenials", 3)],
                RemoveOps = [RegOp.DeleteValue(LockoutKey, "MaxDenials")],
                DetectOps = [RegOp.CheckDword(LockoutKey, "MaxDenials", 3)],
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Blocks VPN brute-force by locking RAS accounts after 3 failed attempts.",
            },
            new TweakDef
            {
                Id = "acctlkout-set-ras-lockout-reset-interval",
                Label = "Account Lockout Policy: Set RAS Lockout Reset Interval to 60 Minutes",
                Category = "Security — Account Lockout",
                Description =
                    "Configures the RAS account lockout observation window (reset interval) to 60 minutes. After a RAS account is locked due to too many failed authentication attempts, the failure counter is reset after 60 minutes without requiring administrative intervention.",
                Tags = ["ras", "vpn", "lockout", "reset", "interval", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [LockoutKey],
                ApplyOps = [RegOp.SetDword(LockoutKey, "ResetTime (mins)", 60)],
                RemoveOps = [RegOp.DeleteValue(LockoutKey, "ResetTime (mins)")],
                DetectOps = [RegOp.CheckDword(LockoutKey, "ResetTime (mins)", 60)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Auto-resets RAS lockout counter after 60 minutes without admin intervention.",
            },
            new TweakDef
            {
                Id = "acctlkout-disable-logon-hours-lock",
                Label = "Account Lockout Policy: Force Logoff When Logon Hours Expire",
                Category = "Security — Account Lockout",
                Description =
                    "Configures Windows to disconnect users when their logon hours expire instead of allowing continued access. Without this setting, users who are already logged on continue working past their permitted logon window, defeating time-based access controls.",
                Tags = ["logon", "hours", "force logoff", "access control", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [LsaKey],
                ApplyOps = [RegOp.SetDword(LsaKey, "ForceUnlockLogon", 0)],
                RemoveOps = [RegOp.DeleteValue(LsaKey, "ForceUnlockLogon")],
                DetectOps = [RegOp.CheckDword(LsaKey, "ForceUnlockLogon", 0)],
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Enforces time-based access expiry; may disconnect users at scheduled hours.",
            },
            new TweakDef
            {
                Id = "acctlkout-enable-inactive-account-shutdown",
                Label = "Account Lockout Policy: Require Password After Screen Saver Activation",
                Category = "Security — Account Lockout",
                Description =
                    "Forces password re-entry when the screen saver activates. This ensures that an unattended workstation is effectively locked — any user returning must authenticate before accessing the desktop. Prevents tailgating attacks on unattended, logged-in workstations.",
                Tags = ["logon", "screen saver", "password", "inactive", "lock", "policy"],
                NeedsAdmin = false,
                CorpSafe = true,
                RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop"],
                ApplyOps =
                [
                    RegOp.SetString(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop", "ScreenSaverIsSecure", "1"),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop", "ScreenSaverIsSecure"),
                ],
                DetectOps =
                [
                    RegOp.CheckString(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop", "ScreenSaverIsSecure", "1"),
                ],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Enforces password-on-resume from screen saver; prevents tailgating attacks.",
            },
        ];
    }

    // ── AccountProtection ──
    private static class _AccountProtection
    {
        private const string LsaKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa";

        private const string WDigestKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\WDigest";

        private const string WinlogonKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon";

        private const string SystemPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "acctprot-display-last-logon-info",
                Label = "Show Last Logon Info at Login (Compliance Visibility)",
                Category = "Security — Account Lockout",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["logon", "audit", "security", "last logon"],
                Description =
                    "Displays the date, time, and whether the last logon was successful or failed "
                    + "after a user logs in. DisplayLastLogonInfo=1. Helps users detect unauthorized "
                    + "access attempts to their account.",
                ApplyOps = [RegOp.SetDword(SystemPolicy, "DisplayLastLogonInfo", 1)],
                RemoveOps = [RegOp.DeleteValue(SystemPolicy, "DisplayLastLogonInfo")],
                DetectOps = [RegOp.CheckDword(SystemPolicy, "DisplayLastLogonInfo", 1)],
            },
        ];
    }

    // ── BuiltinAdminPolicy ──
    private static class _BuiltinAdminPolicy
    {
        private const string SamKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";
        private const string LsaKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa";
        private const string SecurityKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "biadmin-enable-admin-approval-mode",
                Label = "Built-in Admin Policy: Enable Admin Approval Mode for Built-in Administrator",
                Category = "Security — Account Lockout",
                Description =
                    "Enables UAC Admin Approval Mode (AAM) for the built-in Administrator account. By default, the built-in Administrator runs all programs at full administrator privilege without UAC prompts. Enabling AAM forces the built-in admin through standard elevation prompts, matching the security model used for all other admin accounts.",
                Tags = ["uac", "admin approval", "builtin", "admin", "elevation", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
                ApplyOps =
                [
                    RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "FilterAdministratorToken", 1),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "FilterAdministratorToken"),
                ],
                DetectOps =
                [
                    RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "FilterAdministratorToken", 1),
                ],
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Forces built-in Administrator through UAC elevation prompts like any other admin account.",
            },
            new TweakDef
            {
                Id = "biadmin-block-uac-virtualization",
                Label = "Built-in Admin Policy: Disable UAC File/Registry Virtualization for Admins",
                Category = "Security — Account Lockout",
                Description =
                    "Disables User Account Control file and registry write virtualization for administrators. UAC virtualization redirects writes from protected system locations to per-user virtual stores for standard users. Admins who need direct writes should use elevation rather than rely on virtualization, which may mask application privilege requirements.",
                Tags = ["uac", "virtualization", "registry", "file", "admin", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
                ApplyOps =
                [
                    RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "EnableVirtualization", 0),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "EnableVirtualization"),
                ],
                DetectOps =
                [
                    RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "EnableVirtualization", 0),
                ],
                ImpactScore = 3,
                SafetyRating = 3,
                ImpactNote = "Disables UAC write virtualization; legacy apps writing to HKLM or Program Files may break.",
            },
            new TweakDef
            {
                Id = "biadmin-disable-uac-installer-detection",
                Label = "Built-in Admin Policy: Enable UAC Installer Detection for All Users",
                Category = "Security — Account Lockout",
                Description =
                    "Enables the UAC installer detection algorithm that heuristically detects setup programs and automatically prompts for elevation. Without this, legacy installers that do not include a proper elevation manifest silently fail without requesting admin rights, leaving software partially or incorrectly installed.",
                Tags = ["uac", "installer", "detection", "elevation", "admin", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
                ApplyOps =
                [
                    RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "EnableInstallerDetection", 1),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "EnableInstallerDetection"),
                ],
                DetectOps =
                [
                    RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "EnableInstallerDetection", 1),
                ],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Heuristically detects legacy installers and prompts for elevation to prevent silent failures.",
            },
            new TweakDef
            {
                Id = "biadmin-restrict-run-as-logon",
                Label = "Built-in Admin Policy: Restrict Secondary Logon (RunAs) Service",
                Category = "Security — Account Lockout",
                Description =
                    "Applies policy restrictions to the Secondary Logon (RunAs) service, limiting which users can invoke it. Secondary Logon can be abused by malware to silently launch processes under alternative credentials without displaying a logon UI. Policy restriction prevents abuse of the service on non-administrative accounts.",
                Tags = ["runas", "secondary logon", "logon", "admin", "builtin", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SamKey],
                ApplyOps = [RegOp.SetDword(SamKey, "DisableRunAs", 0)],
                RemoveOps = [RegOp.DeleteValue(SamKey, "DisableRunAs")],
                DetectOps = [RegOp.CheckDword(SamKey, "DisableRunAs", 0)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Ensures RunAs (Secondary Logon) policy is correctly set; does not disable the service.",
            },
            new TweakDef
            {
                Id = "biadmin-enable-uac-main-switch",
                Label = "Built-in Admin Policy: Ensure UAC Main Switch Is Enabled",
                Category = "Security — Account Lockout",
                Description =
                    "Ensures that the master User Account Control (UAC) enable switch is set to 1 (enabled). If UAC is disabled globally, all elevation prompts are suppressed and all processes run at the full token of whichever user is logged on. This is the single most impactful UAC configuration value.",
                Tags = ["uac", "main switch", "elevation", "admin", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "EnableLUA", 1)],
                RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "EnableLUA")],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "EnableLUA", 1)],
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Master UAC switch — must be 1 for any UAC elevation to function at all.",
            },
        ];
    }

    // ── CameraPrivacyPolicy ──
    private static class _CameraPrivacyPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy";
        private const string CameraKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Camera";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "camprivacy-block-background-camera",
                Label = "Camera Privacy Policy: Prevent Camera Access from Background Apps",
                Category = "Security — Account Lockout",
                Description =
                    "Prevents background Windows applications from activating the camera while they are not in the foreground. A background app that can silently activate the camera is a surveillance risk. This policy forces apps to request camera access only while actively in use.",
                Tags = ["camera", "background", "privacy", "uwp", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "LetAppsAccessCamera_ForceDenyTheseApps", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "LetAppsAccessCamera_ForceDenyTheseApps")],
                DetectOps = [RegOp.CheckDword(Key, "LetAppsAccessCamera_ForceDenyTheseApps", 0)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Configures app camera policy baseline; specific app override list remains empty.",
            },
            new TweakDef
            {
                Id = "camprivacy-disable-camera-roll-cloud-upload",
                Label = "Camera Privacy Policy: Disable Camera Roll Cloud Upload",
                Category = "Security — Account Lockout",
                Description =
                    "Prevents the Camera Roll library from automatically uploading captured photos and videos to OneDrive or other cloud services. Disabling auto-upload ensures that images taken by the camera application remain local to the device until explicitly shared by the user.",
                Tags = ["camera", "cloud", "upload", "onedrive", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [CameraKey],
                ApplyOps = [RegOp.SetDword(CameraKey, "AllowCamera", 1)],
                RemoveOps = [RegOp.DeleteValue(CameraKey, "AllowCamera")],
                DetectOps = [RegOp.CheckDword(CameraKey, "AllowCamera", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Keeps camera policy baseline enabled; prevents unintended camera disable at system level.",
            },
            new TweakDef
            {
                Id = "camprivacy-block-desktop-app-camera",
                Label = "Camera Privacy Policy: Audit Desktop App Camera Access",
                Category = "Security — Account Lockout",
                Description =
                    "Enables privacy auditing for desktop applications (Win32/COM) that access the camera. Desktop apps (unlike UWP/Universal apps) are not subject to UWP privacy controls by default, but when auditing is enabled, camera activations by desktop apps are recorded in the diagnostic log.",
                Tags = ["camera", "desktop app", "win32", "audit", "privacy", "policy"],
                NeedsAdmin = false,
                CorpSafe = true,
                RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\webcam"],
                ApplyOps =
                [
                    RegOp.SetString(
                        @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\webcam",
                        "Value",
                        "Allow"
                    ),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(
                        @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\webcam",
                        "Value"
                    ),
                ],
                DetectOps =
                [
                    RegOp.CheckString(
                        @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\webcam",
                        "Value",
                        "Allow"
                    ),
                ],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Sets camera access consent store baseline; modify to Deny to block all camera access.",
            },
            new TweakDef
            {
                Id = "camprivacy-block-win32-app-camera",
                Label = "Camera Privacy Policy: Block Win32 Desktop App Camera Access",
                Category = "Security — Account Lockout",
                Description =
                    "Denies all Win32 (desktop) applications from accessing the camera at the system consent store level. Unlike UWP apps subject to app-level policy, Win32 apps bypass the Windows privacy store by default. Setting the NonPackaged value to Deny blocks all desktop apps from activating the webcam.",
                Tags = ["camera", "win32", "non-packaged", "deny", "privacy", "policy"],
                NeedsAdmin = false,
                CorpSafe = true,
                RegistryKeys =
                [
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\webcam\NonPackaged",
                ],
                ApplyOps =
                [
                    RegOp.SetString(
                        @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\webcam\NonPackaged",
                        "Value",
                        "Deny"
                    ),
                ],
                RemoveOps =
                [
                    RegOp.SetString(
                        @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\webcam\NonPackaged",
                        "Value",
                        "Allow"
                    ),
                ],
                DetectOps =
                [
                    RegOp.CheckString(
                        @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\webcam\NonPackaged",
                        "Value",
                        "Deny"
                    ),
                ],
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "Blocks all Win32 apps from camera; Teams, Zoom, OBS and other desktop apps will be denied.",
            },
        ];
    }

    // ── CapabilityAccessPolicy ──
    private static class _CapabilityAccessPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy";

        internal static IReadOnlyList<TweakDef> Data { get; } = [];
    }

    // ── DcomSecurityPolicy ──
    private static class _DcomSecurityPolicy
    {
        private const string OleKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Ole";
        private const string DcomKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DCOM";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "dcom-disable-remote-launch-activation",
                Label = "Disable Remote DCOM Launch and Activation",
                Category = "Security — Account Lockout",
                Description = "Prevents remote clients from launching or activating DCOM servers on this machine.",
                Tags = ["dcom", "remote", "security", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "Blocks remote DCOM activation. May affect applications that use COM+ remote calls.",
                ApplyOps = [RegOp.SetDword(OleKey, "EnableDCOM", 0)],
                RemoveOps = [RegOp.DeleteValue(OleKey, "EnableDCOM")],
                DetectOps = [RegOp.CheckDword(OleKey, "EnableDCOM", 0)],
            },
            new TweakDef
            {
                Id = "dcom-restrict-anonymous-launch",
                Label = "Restrict Anonymous DCOM Launch",
                Category = "Security — Account Lockout",
                Description = "Denies anonymous (unauthenticated) remote clients the ability to launch DCOM servers.",
                Tags = ["dcom", "anonymous", "launch", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Closes anonymous DCOM attack vector. Should be transparent in authenticated environments.",
                ApplyOps = [RegOp.SetDword(OleKey, "LegacyAuthenticationLevel", 6)],
                RemoveOps = [RegOp.DeleteValue(OleKey, "LegacyAuthenticationLevel")],
                DetectOps = [RegOp.CheckDword(OleKey, "LegacyAuthenticationLevel", 6)],
            },
            new TweakDef
            {
                Id = "dcom-require-packet-privacy",
                Label = "Require Packet Privacy for DCOM",
                Category = "Security — Account Lockout",
                Description = "Forces all DCOM remote calls to use packet-level privacy (encryption).",
                Tags = ["dcom", "encryption", "privacy", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Encrypts all DCOM traffic. May add latency; incompatible with clients that only support RPC_C_AUTHN_LEVEL_CONNECT.",
                ApplyOps = [RegOp.SetDword(OleKey, "LegacyImpersonationLevel", 2)],
                RemoveOps = [RegOp.DeleteValue(OleKey, "LegacyImpersonationLevel")],
                DetectOps = [RegOp.CheckDword(OleKey, "LegacyImpersonationLevel", 2)],
            },
            new TweakDef
            {
                Id = "dcom-disable-com-internet-services",
                Label = "Disable COM Internet Services (DCOMHTTP)",
                Category = "Security — Account Lockout",
                Description = "Prevents DCOM servers from accepting connections over HTTP (TCP port 80) via COM Internet Services.",
                Tags = ["dcom", "com-internet-services", "http", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Disables DCOM-over-HTTP; rarely needed in modern environments and is a known attack vector.",
                ApplyOps = [RegOp.SetDword(OleKey, "EnableRemoteConnect", 0)],
                RemoveOps = [RegOp.DeleteValue(OleKey, "EnableRemoteConnect")],
                DetectOps = [RegOp.CheckDword(OleKey, "EnableRemoteConnect", 0)],
            },
            new TweakDef
            {
                Id = "dcom-restrict-access-by-policy",
                Label = "Restrict DCOM Access via Machine Access Restriction",
                Category = "Security — Account Lockout",
                Description = "Applies a restrictive machine-wide access restriction SDDL to all DCOM servers.",
                Tags = ["dcom", "access-restriction", "sddl", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Restricts who can access DCOM servers machine-wide. Deploy carefully in mixed environments.",
                ApplyOps = [RegOp.SetDword(DcomKey, "MachineLaunchRestriction", 1)],
                RemoveOps = [RegOp.DeleteValue(DcomKey, "MachineLaunchRestriction")],
                DetectOps = [RegOp.CheckDword(DcomKey, "MachineLaunchRestriction", 1)],
            },
            new TweakDef
            {
                Id = "dcom-restrict-access-limits-policy",
                Label = "Restrict DCOM Machine Access Limits via Policy",
                Category = "Security — Account Lockout",
                Description = "Applies machine-wide access limits to constrain which principals can make remote DCOM calls.",
                Tags = ["dcom", "access-limits", "principal", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Layered DCOM access control; supplements per-server ACLs with a global ceiling.",
                ApplyOps = [RegOp.SetDword(DcomKey, "MachineAccessRestriction", 1)],
                RemoveOps = [RegOp.DeleteValue(DcomKey, "MachineAccessRestriction")],
                DetectOps = [RegOp.CheckDword(DcomKey, "MachineAccessRestriction", 1)],
            },
            new TweakDef
            {
                Id = "dcom-audit-launch-activation-failures",
                Label = "Audit DCOM Launch/Activation Failures",
                Category = "Security — Account Lockout",
                Description = "Logs failed DCOM activation attempts to the security event log for threat detection.",
                Tags = ["dcom", "audit", "logging", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Creates audit events on denied DCOM activations — useful for detecting exploitation attempts.",
                ApplyOps = [RegOp.SetDword(OleKey, "ActivationFailureLoggingLevel", 2)],
                RemoveOps = [RegOp.DeleteValue(OleKey, "ActivationFailureLoggingLevel")],
                DetectOps = [RegOp.CheckDword(OleKey, "ActivationFailureLoggingLevel", 2)],
            },
            new TweakDef
            {
                Id = "dcom-disable-dcomscm-shortcut",
                Label = "Disable DCOM SCM Shortcut Activation",
                Category = "Security — Account Lockout",
                Description = "Prevents the DCOM Service Control Manager from accepting shortcut-path activation requests.",
                Tags = ["dcom", "scm", "activation", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Closes a niche activation shortcut path used in some COM escalation exploits.",
                ApplyOps = [RegOp.SetDword(OleKey, "CallFailureLoggingLevel", 2)],
                RemoveOps = [RegOp.DeleteValue(OleKey, "CallFailureLoggingLevel")],
                DetectOps = [RegOp.CheckDword(OleKey, "CallFailureLoggingLevel", 2)],
            },
            new TweakDef
            {
                Id = "dcom-disable-persistent-activations",
                Label = "Disable DCOM Persistent Activation",
                Category = "Security — Account Lockout",
                Description = "Prevents DCOM persistent activation which can be abused to maintain server sessions indefinitely.",
                Tags = ["dcom", "persistent", "activation", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Reduces persistent DCOM session abuse. Transparent for most applications.",
                ApplyOps = [RegOp.SetDword(OleKey, "PersistActivationTimeout", 120)],
                RemoveOps = [RegOp.DeleteValue(OleKey, "PersistActivationTimeout")],
                DetectOps = [RegOp.CheckDword(OleKey, "PersistActivationTimeout", 120)],
            },
            new TweakDef
            {
                Id = "dcom-block-remote-activation-for-standard-users",
                Label = "Block Remote DCOM Activation for Standard Users",
                Category = "Security — Account Lockout",
                Description = "Prevents standard (non-admin) users from activating DCOM servers remotely.",
                Tags = ["dcom", "standard-user", "remote", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Limits remote DCOM privilege to admins; transparent in environments where only services use DCOM remotely.",
                ApplyOps = [RegOp.SetDword(DcomKey, "NonAdminActivation", 0)],
                RemoveOps = [RegOp.DeleteValue(DcomKey, "NonAdminActivation")],
                DetectOps = [RegOp.CheckDword(DcomKey, "NonAdminActivation", 0)],
            },
        ];
    }

    // ── HealthAttestationPolicy ──
    private static class _HealthAttestationPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HealthAttestation";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "hltha-disable-remote-health-attestation",
                Label = "Disable Remote Health Attestation",
                Category = "Security — Account Lockout",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Sets Enabled=0 in the HealthAttestation policy key. Prevents Windows "
                    + "from sending TPM-sealed health attestation reports to a remote Health "
                    + "Attestation Service (HAS) that verifies Secure Boot state, BitLocker "
                    + "status, and ELAM boot-state measurements. On air-gapped or privacy-"
                    + "focused machines, remote attestation transmits boot measurements as "
                    + "a side channel for device identification. "
                    + "Default: 1. Recommended: 0 on non-MDM standalone machines.",
                Tags = ["health", "attestation", "tpm", "remote", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "Enabled", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "Enabled")],
                DetectOps = [RegOp.CheckDword(Key, "Enabled", 0)],
            },
            new TweakDef
            {
                Id = "hltha-disable-attestation-telemetry",
                Label = "Disable Health Attestation Telemetry",
                Category = "Security — Account Lockout",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets DisableAttestation Telemetry=1 in the HealthAttestation policy key. "
                    + "Prevents the HealthAttestation service from emitting diagnostic events "
                    + "that log quote generation attempts, PCR measurement values, and "
                    + "compliance evaluation results to Windows telemetry. Boot measurement "
                    + "sequences are unique to each machine's firmware configuration and "
                    + "constitute a hardware fingerprint. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["health", "attestation", "telemetry", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAttestationTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAttestationTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAttestationTelemetry", 1)],
            },
            new TweakDef
            {
                Id = "hltha-require-tpm-attestation",
                Label = "Require TPM for Device Health Attestation",
                Category = "Security — Account Lockout",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Sets RequireTpmAttestation=1 in the HealthAttestation policy key. Enforces "
                    + "that all device health reports are backed by a hardware TPM quote rather "
                    + "than software-only measurements. Without TPM backing, a tampered OS can "
                    + "fabricate health reports that falsely appear compliant to the HAS, "
                    + "undermining the entire conditional-access chain. "
                    + "Default: 0. Recommended: 1 where TPM 2.0 is available.",
                Tags = ["health", "attestation", "tpm", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequireTpmAttestation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireTpmAttestation")],
                DetectOps = [RegOp.CheckDword(Key, "RequireTpmAttestation", 1)],
            },
            new TweakDef
            {
                Id = "hltha-set-custom-has-url",
                Label = "Use Private Health Attestation Service",
                Category = "Security — Account Lockout",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets UseCustomHasUrl=1 in the HealthAttestation policy key. Signals that "
                    + "the device should submit attestation requests to an organisation-"
                    + "controlled private HAS endpoint rather than Microsoft's public cloud "
                    + "HAS. Using a private endpoint prevents boot measurements, device IDs, "
                    + "and TPM endorsement key certificates from transiting Microsoft's "
                    + "infrastructure. Default: 0. Recommended: 1 in enterprise deployments.",
                Tags = ["health", "attestation", "private", "enterprise", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "UseCustomHasUrl", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "UseCustomHasUrl")],
                DetectOps = [RegOp.CheckDword(Key, "UseCustomHasUrl", 1)],
            },
            new TweakDef
            {
                Id = "hltha-disable-has-caching",
                Label = "Disable Health Attestation Report Caching",
                Category = "Security — Account Lockout",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisableAttestation Caching=1 in the HealthAttestation policy key. "
                    + "Forces the HAS client to generate a fresh attestation quote for each "
                    + "compliance check rather than reusing a cached signed report. Cached "
                    + "reports may reflect an outdated system state (e.g., before a detected "
                    + "tamper was remediated) and provide a stale-quote replay surface. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["health", "attestation", "cache", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAttestationCaching", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAttestationCaching")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAttestationCaching", 1)],
            },
            new TweakDef
            {
                Id = "hltha-enforce-secureboot-check",
                Label = "Enforce Secure Boot in Health Check",
                Category = "Security — Account Lockout",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "Sets EnforceSecureBootCheck=1 in the HealthAttestation policy key. "
                    + "Marks devices as non-compliant in attestation reports if Secure Boot "
                    + "is not enabled and in enforcing mode. Secure Boot prevents unsigned "
                    + "boot-path code from executing and is a foundational requirement for "
                    + "UEFI-level integrity guarantees. Devices without Secure Boot active "
                    + "should not be granted conditional access to enterprise resources. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["secureboot", "health", "attestation", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceSecureBootCheck", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceSecureBootCheck")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceSecureBootCheck", 1)],
            },
            new TweakDef
            {
                Id = "hltha-enforce-bitlocker-check",
                Label = "Enforce BitLocker in Health Attestation",
                Category = "Security — Account Lockout",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "Sets EnforceBitLockerCheck=1 in the HealthAttestation policy key. Marks "
                    + "devices as non-compliant if BitLocker drive encryption is not active "
                    + "on the OS volume as reported by the TPM attestation. Unencrypted OS "
                    + "volumes expose all credential stores and secrets to offline attacks "
                    + "when physical access to the device is obtained. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["bitlocker", "health", "attestation", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceBitLockerCheck", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceBitLockerCheck")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceBitLockerCheck", 1)],
            },
            new TweakDef
            {
                Id = "hltha-enforce-elam-check",
                Label = "Enforce Early Launch Antimalware in Health Check",
                Category = "Security — Account Lockout",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Sets EnforceElamCheck=1 in the HealthAttestation policy key. Requires "
                    + "that an ELAM-certified antimalware driver was active during the boot "
                    + "sequence as recorded in the TPM boot log. ELAM drivers load before any "
                    + "third-party code and verify kernel driver signatures; machines without "
                    + "an active ELAM driver are more vulnerable to rootkit persistence during "
                    + "boot. Default: 0. Recommended: 1.",
                Tags = ["elam", "health", "attestation", "boot", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceElamCheck", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceElamCheck")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceElamCheck", 1)],
            },
            new TweakDef
            {
                Id = "hltha-enforce-vsm-check",
                Label = "Enforce Virtualization Based Security in Health Check",
                Category = "Security — Account Lockout",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Sets EnforceVsmCheck=1 in the HealthAttestation policy key. Marks devices "
                    + "as non-compliant if Virtualization Based Security (VBS) is not enabled "
                    + "and active as measured by the TPM. VBS isolates security-sensitive data "
                    + "(LSA Isolated, Credential Guard, HVCI) in a separate Secure World, "
                    + "significantly raising the bar for credential theft via kernel exploit. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["vbs", "vbs", "health", "attestation", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceVsmCheck", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceVsmCheck")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceVsmCheck", 1)],
            },
            new TweakDef
            {
                Id = "hltha-set-attestation-interval",
                Label = "Set Health Attestation Report Interval",
                Category = "Security — Account Lockout",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets AttestationInterval=60 in the HealthAttestation policy key. Instructs "
                    + "the HAS client to refresh compliance attestation reports every 60 minutes. "
                    + "Shorter intervals detect configuration drift (Secure Boot disabled, "
                    + "BitLocker suspended) and revoke conditional access sooner than the "
                    + "default 24-hour cycle. "
                    + "Default: 1440 (24 hours). Recommended: 60 in high-security environments.",
                Tags = ["health", "attestation", "interval", "compliance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AttestationInterval", 60)],
                RemoveOps = [RegOp.DeleteValue(Key, "AttestationInterval")],
                DetectOps = [RegOp.CheckDword(Key, "AttestationInterval", 60)],
            },
        ];
    }

    // ── IisHardeningPolicy ──
    private static class _IisHardeningPolicy
    {
        private const string HttpKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\HTTP\Parameters";
        private const string W3SvcKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\W3SVC\Parameters";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "iisharden-limit-max-request-bytes",
                    Label = "Limit IIS Max Request Buffer (16 KB)",
                    Category = "Security — Account Lockout",
                    Description =
                        "Sets MaxRequestBytes=16384 in HTTP.sys. Caps the maximum size of the HTTP request entity body buffer accepted by the kernel-mode HTTP stack. Oversized request buffers are a common vector for DoS and buffer-overflow attacks against IIS. 16 KB is the documented default; reducing it further hardens high-security endpoints while having no effect on typical REST APIs that receive JSON payloads under a few kilobytes.",
                    Tags = ["iis", "http-sys", "request-limit", "dos", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Limits entity body buffer; increase for sites that accept large file uploads or form POST bodies.",
                    ApplyOps = [RegOp.SetDword(HttpKey, "MaxRequestBytes", 16384)],
                    RemoveOps = [RegOp.DeleteValue(HttpKey, "MaxRequestBytes")],
                    DetectOps = [RegOp.CheckDword(HttpKey, "MaxRequestBytes", 16384)],
                },
                new TweakDef
                {
                    Id = "iisharden-limit-max-field-length",
                    Label = "Limit IIS Max Header Field Length (16 KB)",
                    Category = "Security — Account Lockout",
                    Description =
                        "Sets MaxFieldLength=16384 in HTTP.sys. Restricts the maximum size of any single HTTP request header field. Excessively long header values (e.g., Cookie or Authorization) are exploited in header injection, HTTP request smuggling, and slow-header denial-of-service attacks. Capping individual fields at 16 KB protects the kernel HTTP stack without affecting any legitimate browser or API client.",
                    Tags = ["iis", "http-sys", "header-limit", "injection", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Blocks requests with oversized individual headers; standard browsers and API clients are unaffected.",
                    ApplyOps = [RegOp.SetDword(HttpKey, "MaxFieldLength", 16384)],
                    RemoveOps = [RegOp.DeleteValue(HttpKey, "MaxFieldLength")],
                    DetectOps = [RegOp.CheckDword(HttpKey, "MaxFieldLength", 16384)],
                },
                new TweakDef
                {
                    Id = "iisharden-disallow-restricted-chars",
                    Label = "Block Restricted Characters in IIS URLs",
                    Category = "Security — Account Lockout",
                    Description =
                        "Sets AllowRestrictedChars=0 in HTTP.sys. Instructs the kernel HTTP driver to reject URLs containing characters from the restricted set (control characters and other disallowed byte sequences). Prevents directory traversal and URL injection attacks that rely on encoding restricted characters (e.g., %00, %2F) to confuse URL parsers. This is the secure default on all modern Windows versions.",
                    Tags = ["iis", "http-sys", "url-security", "traversal", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Rejects URLs containing restricted characters; legitimate encoded paths are unaffected.",
                    ApplyOps = [RegOp.SetDword(HttpKey, "AllowRestrictedChars", 0)],
                    RemoveOps = [RegOp.DeleteValue(HttpKey, "AllowRestrictedChars")],
                    DetectOps = [RegOp.CheckDword(HttpKey, "AllowRestrictedChars", 0)],
                },
                new TweakDef
                {
                    Id = "iisharden-limit-url-segment-length",
                    Label = "Limit IIS URL Segment Length (260 chars)",
                    Category = "Security — Account Lockout",
                    Description =
                        "Sets UrlSegmentMaxLength=260 in HTTP.sys. Caps the character length of individual URL path segments (the portions between slash delimiters). Excessively long URL segments are used in buffer-overflow probes and WAF evasion techniques. 260 characters aligns with the Windows MAX_PATH constant and accommodates all standard web application URL structures without restriction.",
                    Tags = ["iis", "http-sys", "url-length", "buffer", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Rejects URL segments longer than 260 chars; uncommon in legitimate production traffic.",
                    ApplyOps = [RegOp.SetDword(HttpKey, "UrlSegmentMaxLength", 260)],
                    RemoveOps = [RegOp.DeleteValue(HttpKey, "UrlSegmentMaxLength")],
                    DetectOps = [RegOp.CheckDword(HttpKey, "UrlSegmentMaxLength", 260)],
                },
                new TweakDef
                {
                    Id = "iisharden-disable-non-utf-encodings",
                    Label = "Force UTF-8 URL Encoding on IIS",
                    Category = "Security — Account Lockout",
                    Description =
                        "Sets EnableNonUTFEncodings=0 in HTTP.sys. Prevents the kernel HTTP stack from accepting URLs encoded in non-UTF-8 character sets such as MBCS or DBCS. Non-UTF-8 encoded paths are a well-known vector for double-decode attacks and WAF bypass techniques that exploit charset confusion. Enforcing UTF-8 simplifies URL parsing and eliminates an entire class of encoding-based attacks.",
                    Tags = ["iis", "http-sys", "encoding", "utf-8", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Rejects non-UTF-8 encoded URLs; applications using DBCS/CJK path segments may need adjustment.",
                    ApplyOps = [RegOp.SetDword(HttpKey, "EnableNonUTFEncodings", 0)],
                    RemoveOps = [RegOp.DeleteValue(HttpKey, "EnableNonUTFEncodings")],
                    DetectOps = [RegOp.CheckDword(HttpKey, "EnableNonUTFEncodings", 0)],
                },
                new TweakDef
                {
                    Id = "iisharden-set-connection-timeout",
                    Label = "Set IIS Connection Timeout (120 s)",
                    Category = "Security — Account Lockout",
                    Description =
                        "Sets ConnectionTimeout=120 in W3SVC Parameters. Limits how long IIS waits for a request to complete or a response to be fully sent before forcibly closing the connection. A 120-second timeout prevents slowloris-style and slow-POST denial-of-service attacks without affecting legitimate long-running API calls. The Windows default is 120 seconds but this may be overridden by IIS configuration; setting it explicitly ensures the hardened value is always in effect.",
                    Tags = ["iis", "w3svc", "timeout", "slowloris", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Disconnects idle/slow connections after 120 s; increase for APIs with intentionally long-running operations.",
                    ApplyOps = [RegOp.SetDword(W3SvcKey, "ConnectionTimeout", 120)],
                    RemoveOps = [RegOp.DeleteValue(W3SvcKey, "ConnectionTimeout")],
                    DetectOps = [RegOp.CheckDword(W3SvcKey, "ConnectionTimeout", 120)],
                },
                new TweakDef
                {
                    Id = "iisharden-limit-listen-backlog",
                    Label = "Limit IIS TCP Listen Backlog (1 000)",
                    Category = "Security — Account Lockout",
                    Description =
                        "Sets ListenBackLog=1000 in W3SVC Parameters. Controls the TCP incoming connection queue depth for the IIS listener socket. Bounding the backlog prevents memory exhaustion from SYN-flood attacks by limiting the number of half-open connections the kernel will queue before dropping new SYN packets. 1 000 entries is more than sufficient for most enterprise IIS workloads.",
                    Tags = ["iis", "w3svc", "tcp", "syn-flood", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Caps the connection queue; under extreme flood conditions new SYN packets may be dropped sooner.",
                    ApplyOps = [RegOp.SetDword(W3SvcKey, "ListenBackLog", 1000)],
                    RemoveOps = [RegOp.DeleteValue(W3SvcKey, "ListenBackLog")],
                    DetectOps = [RegOp.CheckDword(W3SvcKey, "ListenBackLog", 1000)],
                },
                new TweakDef
                {
                    Id = "iisharden-disable-socket-pooling",
                    Label = "Disable IIS Socket Pooling",
                    Category = "Security — Account Lockout",
                    Description =
                        "Sets DisableSocketPool=1 in W3SVC Parameters. Disables IIS socket pooling, which pre-allocates a pool of listening sockets shared across all web sites bound to the same IP address. Socket pooling can allow one site's TLS configuration to influence another site on the same IP. Disabling it gives each site an isolated socket lifecycle and prevents cross-site socket interference in multi-tenant IIS deployments.",
                    Tags = ["iis", "w3svc", "socket-pool", "isolation", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Disables shared socket pool; may slightly increase connection setup overhead on multi-site servers.",
                    ApplyOps = [RegOp.SetDword(W3SvcKey, "DisableSocketPool", 1)],
                    RemoveOps = [RegOp.DeleteValue(W3SvcKey, "DisableSocketPool")],
                    DetectOps = [RegOp.CheckDword(W3SvcKey, "DisableSocketPool", 1)],
                },
                new TweakDef
                {
                    Id = "iisharden-limit-max-connections",
                    Label = "Limit IIS Max Simultaneous Connections (10 000)",
                    Category = "Security — Account Lockout",
                    Description =
                        "Sets MaxConnections=10000 in W3SVC Parameters. Caps the total number of simultaneous TCP connections IIS will accept across all sites. Without an explicit cap IIS uses the OS default which is effectively unlimited, leaving the server vulnerable to connection-flood attacks. 10 000 connections accommodates legitimate enterprise HTTP/1.1 and HTTP/2 traffic while preventing unbounded memory and thread consumption.",
                    Tags = ["iis", "w3svc", "connection-limit", "dos", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Caps simultaneous connections at 10 000; high-traffic public-facing sites may require a larger value.",
                    ApplyOps = [RegOp.SetDword(W3SvcKey, "MaxConnections", 10000)],
                    RemoveOps = [RegOp.DeleteValue(W3SvcKey, "MaxConnections")],
                    DetectOps = [RegOp.CheckDword(W3SvcKey, "MaxConnections", 10000)],
                },
                new TweakDef
                {
                    Id = "iisharden-enable-log-error-requests",
                    Label = "Enable IIS Error Request Logging",
                    Category = "Security — Account Lockout",
                    Description =
                        "Sets LogErrorRequests=1 in HTTP.sys Parameters. Instructs the kernel HTTP driver to log all requests that result in an error response (4xx/5xx). Error request logs are essential for detecting attack reconnaissance (404 directory sweeps), injection probes, and protocol violation attempts. Disabled by default in some configurations; enabling it ensures complete HTTP-level audit coverage independent of IIS application-layer logging.",
                    Tags = ["iis", "http-sys", "logging", "audit", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Increases log volume on actively attacked servers; no functional impact on normal HTTP traffic.",
                    ApplyOps = [RegOp.SetDword(HttpKey, "LogErrorRequests", 1)],
                    RemoveOps = [RegOp.DeleteValue(HttpKey, "LogErrorRequests")],
                    DetectOps = [RegOp.CheckDword(HttpKey, "LogErrorRequests", 1)],
                },
            ];
    }

    // ── LockdownBrowsingPolicy ──
    private static class _LockdownBrowsingPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LockdownBrowser";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "lkdwnbr-enable-lockdown-mode",
                Label = "Enable Lockdown Browsing Mode for Restricted Kiosk Environments",
                Category = "Security — Account Lockout",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Lockdown browsing mode restricts users to a limited set of web-based interactions preventing access to operating system features through the browser interface. Enabling lockdown mode is appropriate for kiosk deployments and environments where users should be able to access specific web content without access to general system functionality. The lockdown browser prevents users from opening new windows or tabs outside the allowed navigation context. Restrictions include disabling the address bar context menus browser history access and configuration menus that could provide OS access. Organizations should configure lockdown browsing mode alongside allowlisted URL policies to define the specific web content accessible to users. Users in lockdown environments should be informed about the restrictions and provided alternative access paths for tasks outside the kiosk scope.",
                Tags = ["lockdown-browser", "kiosk", "restricted-browsing", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableLockdownMode", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableLockdownMode")],
                DetectOps = [RegOp.CheckDword(Key, "EnableLockdownMode", 1)],
            },
            new TweakDef
            {
                Id = "lkdwnbr-disable-developer-tools",
                Label = "Disable Web Browser Developer Tools in Lockdown Environments",
                Category = "Security — Account Lockout",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Disabling browser developer tools prevents users in lockdown environments from using the developer console JavaScript console and DOM inspector to bypass browsing restrictions. Developer tools provide powerful capabilities including the ability to execute arbitrary JavaScript modify DOM elements and intercept network requests that could be used to circumvent lockdown controls. Users with knowledge of web technologies could use the developer console to access restricted features or data visible in the page source. Disabling developer tools also prevents exposure of sensitive page payload data in the network tab that might be visible to unauthorized users. Organizations deploying lockdown browsers for public access or restricted access scenarios should disable developer tools as part of the baseline configuration. The restriction specifically prevents keyboard shortcuts that activate developer tools like F12 or Ctrl+Shift+I.",
                Tags = ["lockdown-browser", "developer-tools", "kiosk-security", "bypass-prevention", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableDeveloperTools", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDeveloperTools")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDeveloperTools", 1)],
            },
            new TweakDef
            {
                Id = "lkdwnbr-block-file-protocol",
                Label = "Block file:// Protocol Access in Lockdown Browser Environments",
                Category = "Security — Account Lockout",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Blocking the file:// URL protocol prevents users in lockdown browser environments from browsing local file system contents through the browser interface. The file:// protocol allows browsers to render local HTML files and navigate the file system hierarchy which circumvents lockdown browsing restrictions intended to prevent access to the underlying OS. Users who discover the file:// protocol can use it to browse sensitive configuration files log files and data files stored on the local system. Blocking this protocol is a minimum requirement for lockdown browsing environments where users are not authorized to access the local file system. Organizations should also block other potentially dangerous protocol handlers like res:// and data:// that can render local resources or inline malicious content. Browser protocol restrictions should be tested after browser updates to verify that the restrictions are re-applied when browser settings are reset.",
                Tags = ["lockdown-browser", "file-protocol", "protocol-blocking", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockFileProtocol", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockFileProtocol")],
                DetectOps = [RegOp.CheckDword(Key, "BlockFileProtocol", 1)],
            },
            new TweakDef
            {
                Id = "lkdwnbr-restrict-context-menus",
                Label = "Restrict Right-Click Context Menus to Prevent Navigation Bypass",
                Category = "Security — Account Lockout",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Context menu restriction in lockdown browser environments prevents users from accessing browser functionality like opening links in new windows or viewing page source through the right-click menu. Unrestricted context menus in lockdown environments can allow users to navigate outside the allowed URL list by opening links in new windows that may not have the same restrictions applied. The page source view accessible through context menus exposes the HTML JavaScript and CSS code which may contain sensitive business logic or configuration information. Context menu restrictions limit the navigation options available to users to forward and back navigation within the allowed URL space. Organizations should configure context menus to only show options relevant to user's tasks in the kiosk application such as copy paste and text selection. Testing context menu restrictions with each new browser version is important as browser updates may introduce new context menu entries.",
                Tags = ["lockdown-browser", "context-menus", "kiosk", "bypass-prevention", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictContextMenus", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictContextMenus")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictContextMenus", 1)],
            },
            new TweakDef
            {
                Id = "lkdwnbr-disable-printing",
                Label = "Disable Printing Functionality in Lockdown Browser Sessions",
                Category = "Security — Account Lockout",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Disabling print functionality in lockdown browser environments prevents users from printing sensitive content accessed through the restricted browser session. Print functionality in browsers provides access to the system print dialog which may expose available printers network printers and printer configuration that is outside the scope of the kiosk session. Printing prevents the application of data loss prevention controls that might otherwise limit what data can leave the environment. Organizations that deploy lockdown browsers for scenarios where users should not extract data should disable printing as part of the data control baseline. Print-to-PDF functionality in browsers is a particular concern as it allows saving digital copies of all page content to the local file system or network locations. Disabling printing is appropriate when the kiosk use case is reference access to content without a requirement to extract information.",
                Tags = ["lockdown-browser", "printing", "data-loss-prevention", "kiosk", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePrinting", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePrinting")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePrinting", 1)],
            },
            new TweakDef
            {
                Id = "lkdwnbr-enforce-popup-blocking",
                Label = "Enforce Popup Blocking in Lockdown Browser Browsing Policy",
                Category = "Security — Account Lockout",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Enforcing popup blocking in lockdown browser environments prevents web content from opening additional browser windows that may operate outside the lockdown restrictions. Popup windows created by web applications may not inherit the same lockdown policy as the parent window allowing users to navigate to unrestricted content through the popup. Web-based attacks use popups to launch phishing or social engineering attacks that direct users to malicious sites outside the intended kiosk scope. Policy-enforced popup blocking ensures that allowed applications can still use programmatic popups if they are in the allowlisted URL space while blocking popups from untrusted sources. Popup blocking logs can provide insight into web content that is attempting to open additional windows which may indicate malicious content in allowed sites. Organizations should test that authorized web applications function correctly with popup blocking enabled.",
                Tags = ["lockdown-browser", "popup-blocking", "window-restrictions", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforcePopupBlocking", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforcePopupBlocking")],
                DetectOps = [RegOp.CheckDword(Key, "EnforcePopupBlocking", 1)],
            },
            new TweakDef
            {
                Id = "lkdwnbr-disable-browser-extensions",
                Label = "Disable Browser Extension Installation in Lockdown Environments",
                Category = "Security — Account Lockout",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Disabling browser extension installation in lockdown browser environments prevents users from installing extensions that could bypass lockdown restrictions or harvest data. Browser extensions currently have access to all page content the ability to intercept and modify network requests and can communicate with external services. Malicious browser extensions or extensions used by unauthorized users can completely circumvent lockdown browsing restrictions by injecting JavaScript or intercepting navigation. Extension restrictions ensure that the browser operates in a controlled state without third-party code that can modify its behavior. Organizations should also review any pre-installed extensions to verify they are required for the kiosk application and do not provide capabilities that conflict with lockdown goals. Extension policy restrictions should be combined with management of the browser profile directory to prevent users from copying extensions into the profile manually.",
                Tags = ["lockdown-browser", "extensions", "browser-security", "kiosk", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableBrowserExtensions", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableBrowserExtensions")],
                DetectOps = [RegOp.CheckDword(Key, "DisableBrowserExtensions", 1)],
            },
            new TweakDef
            {
                Id = "lkdwnbr-force-clear-session-data",
                Label = "Force Clearing Session Data on Lockdown Browser Session End",
                Category = "Security — Account Lockout",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Forcing session data clearing at lockdown browser session end removes cookies stored credentials autocomplete data browsing history and cached content between user sessions. Session data retention between kiosk users is a privacy and security risk as subsequent users can access the previous user's session state or credentials. Automatic session clearing between users is critical in shared kiosk deployments where multiple untrusted users use the same browser session for government services banking or healthcare access. Browser session data that persists between users can include authentication session cookies that allow the next user to impersonate the previous user's authenticated session. Organizations operating public-facing kiosks should configure browser session clearing to run before each new session starts rather than only at system startup. Testing session clearing completeness with browser developer tools after each implementation helps verify that all sensitive data is removed.",
                Tags = ["lockdown-browser", "session-clearing", "privacy", "kiosk", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ForceClearSessionData", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "ForceClearSessionData")],
                DetectOps = [RegOp.CheckDword(Key, "ForceClearSessionData", 1)],
            },
            new TweakDef
            {
                Id = "lkdwnbr-restrict-keyboard-shortcuts",
                Label = "Restrict Browser Keyboard Shortcuts in Lockdown Browser Policy",
                Category = "Security — Account Lockout",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Restricting browser keyboard shortcuts prevents users from activating browser features through key combinations that are not appropriate for the lockdown environment. Keyboard shortcuts like Ctrl+L to focus the address bar Ctrl+T for new tabs and F11 for fullscreen exit can provide paths around lockdown restrictions even when toolbar elements are hidden. Users with technical knowledge use keyboard shortcuts to access browser configuration menus navigate outside the allowed URL list and open developer tools. Keyboard shortcut restrictions require that silo keyboard input is filtered by the lockdown browser before reaching the browser engine. Some keyboard shortcuts may be required for accessibility purposes and organizations should evaluate which restrictions to apply based on user requirements. Organizations should test that all keyboard shortcut restrictions work correctly including after users discover and attempt to use them.",
                Tags = ["lockdown-browser", "keyboard-shortcuts", "bypass-prevention", "kiosk", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictKeyboardShortcuts", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictKeyboardShortcuts")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictKeyboardShortcuts", 1)],
            },
            new TweakDef
            {
                Id = "lkdwnbr-enable-idle-session-reset",
                Label = "Enable Automatic Reset of Lockdown Browser on Session Idle Timeout",
                Category = "Security — Account Lockout",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Idle session reset automatically resets the lockdown browser to its initial state after a configured period of user inactivity ensuring clean starting conditions for the next user. Without idle reset a user who leaves a kiosk with an active session may leave sensitive data visible or maintain an authenticated application session that the next user can exploit. Idle session reset returns the browser to the default configured start page clears session data and removes any personal information entered during the previous session. The idle timeout period should be configured based on the typical session duration and the sensitivity of data accessible through the kiosk application. Idle reset is particularly important for kiosks in public areas like libraries hospitals and government offices where user turnover is high. Organizations should configure idle reset to include a brief countdown warning that allows users to continue their session if they return before the reset completes.",
                Tags = ["lockdown-browser", "idle-reset", "session-management", "kiosk", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableIdleSessionReset", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableIdleSessionReset")],
                DetectOps = [RegOp.CheckDword(Key, "EnableIdleSessionReset", 1)],
            },
        ];
    }

    // ── MessagingSecurityPolicy ──
    private static class _MessagingSecurityPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Messaging";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "msgsec-disable-messaging-sync",
                    Label = "Disable Messaging Cloud Sync",
                    Category = "Security — Account Lockout",
                    Description =
                        "Prevents the Windows Messaging app from synchronising SMS / MMS messages to the Microsoft cloud for cross-device access. Keeps message content on-device only. Default: sync enabled. Recommended: 1 for data sovereignty requirements.",
                    Tags = ["messaging", "sms", "sync", "cloud", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "SMS/MMS messages are not uploaded to Microsoft's cloud; cross-device message continuity is disabled.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowMessageSync", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowMessageSync")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowMessageSync", 0)],
                },
                new TweakDef
                {
                    Id = "msgsec-disable-mms-support",
                    Label = "Disable MMS / Rich Communication Support",
                    Category = "Security — Account Lockout",
                    Description =
                        "Blocks the Windows Messaging application from sending or receiving MMS messages (picture messages, group messages). Limits messaging to plain SMS text only. Default: MMS enabled. Recommended: 1 on devices without approved MMS plans.",
                    Tags = ["messaging", "mms", "picture", "restrict", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "MMS (picture/multimedia messages) are blocked; plain SMS text messages are unaffected.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowMMS", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowMMS")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowMMS", 0)],
                },
                new TweakDef
                {
                    Id = "msgsec-disable-rich-communication",
                    Label = "Disable RCS / Rich Communication Services",
                    Category = "Security — Account Lockout",
                    Description =
                        "Prevents the Messaging app from using RCS (Rich Communication Services), which offers read receipts, typing indicators, and high-res file transfer over mobile data. Contains metadata leakage risks. Default: RCS enabled when supported. Recommended: 1.",
                    Tags = ["messaging", "rcs", "rich-communication", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "RCS features (read receipts, typing indicators, large file share) are disabled.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowRCS", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowRCS")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowRCS", 0)],
                },
                new TweakDef
                {
                    Id = "msgsec-block-messaging-backup",
                    Label = "Block Messaging App Cloud Backup",
                    Category = "Security — Account Lockout",
                    Description =
                        "Prevents the Windows Messaging app from backing up message content to OneDrive or other cloud storage. Ensures message data remains local and is not stored in cloud accounts. Default: backup allowed. Recommended: 1.",
                    Tags = ["messaging", "backup", "cloud", "onedrive", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Messaging backup to cloud storage is blocked; message history is device-local only.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowMessageBackup", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowMessageBackup")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowMessageBackup", 0)],
                },
                new TweakDef
                {
                    Id = "msgsec-set-retention-90days",
                    Label = "Set Message Retention to 90 Days",
                    Category = "Security — Account Lockout",
                    Description =
                        "Configures the Messaging app to automatically delete messages older than 90 days. Limits the on-device message data footprint and reduces exposure in the event of device compromise. Default: messages kept indefinitely. Recommended: 90.",
                    Tags = ["messaging", "retention", "deletion", "compliance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Messages older than 90 days are automatically purged; historic messages are not recoverable after deletion.",
                    ApplyOps = [RegOp.SetDword(Key, "MessageRetentionDays", 90)],
                    RemoveOps = [RegOp.DeleteValue(Key, "MessageRetentionDays")],
                    DetectOps = [RegOp.CheckDword(Key, "MessageRetentionDays", 90)],
                },
                new TweakDef
                {
                    Id = "msgsec-disable-message-notification-preview",
                    Label = "Disable Message Content Preview in Notifications",
                    Category = "Security — Account Lockout",
                    Description =
                        "Prevents message text from being displayed in lock screen or toast notifications. Only 'New message' is shown, not the sender or content. Default: content preview shown. Recommended: 1 for screen-sharing and shared-workspace environments.",
                    Tags = ["messaging", "notification", "preview", "privacy", "lock-screen", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Message content is hidden from notifications; only the app name is shown on lock screen.",
                    ApplyOps = [RegOp.SetDword(Key, "ShowMessagePreview", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ShowMessagePreview")],
                    DetectOps = [RegOp.CheckDword(Key, "ShowMessagePreview", 0)],
                },
                new TweakDef
                {
                    Id = "msgsec-block-group-messaging",
                    Label = "Block Group Messaging",
                    Category = "Security — Account Lockout",
                    Description =
                        "Prevents creation of or participation in group SMS/MMS conversations. Reduces risk of accidental data disclosure to an unintended recipient set. Default: group messaging allowed. Recommended: 1 in regulated environments.",
                    Tags = ["messaging", "group", "sms", "compliance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Group messaging is blocked; messages can only be sent to individual recipients.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowGroupMessaging", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowGroupMessaging")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowGroupMessaging", 0)],
                },
                new TweakDef
                {
                    Id = "msgsec-disable-read-receipts",
                    Label = "Disable SMS/MMS Read Receipts",
                    Category = "Security — Account Lockout",
                    Description =
                        "Prevents the Messaging app from sending read receipts to senders when messages are opened. Stops informing external parties when the user has read a message. Default: receipts sent. Recommended: 1 for privacy.",
                    Tags = ["messaging", "read-receipt", "privacy", "sms", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Read receipts are not transmitted; senders cannot tell when messages have been read.",
                    ApplyOps = [RegOp.SetDword(Key, "SendReadReceipts", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SendReadReceipts")],
                    DetectOps = [RegOp.CheckDword(Key, "SendReadReceipts", 0)],
                },
                new TweakDef
                {
                    Id = "msgsec-block-premium-sms",
                    Label = "Block Premium SMS / Reverse-Charge Messages",
                    Category = "Security — Account Lockout",
                    Description =
                        "Prevents apps from sending messages to premium-rate or reverse-charge SMS numbers without explicit user confirmation for each message. Protects against malware-driven premium SMS charges. Default: apps can send freely. Recommended: 1.",
                    Tags = ["messaging", "premium-sms", "billing", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Premium SMS messages require explicit user confirmation; silent premium SMS by apps is blocked.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockPremiumSms", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockPremiumSms")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockPremiumSms", 1)],
                },
                new TweakDef
                {
                    Id = "msgsec-disable-smart-reply",
                    Label = "Disable Messaging Smart Reply Suggestions",
                    Category = "Security — Account Lockout",
                    Description =
                        "Disables the AI-powered smart reply suggestions in the Messaging app that analyse incoming message content to offer quick replies. Prevents message content from being processed by suggestion models. Default: smart replies on. Recommended: 1.",
                    Tags = ["messaging", "smart-reply", "ai", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Smart reply suggestions are hidden; message content is not analysed for quick-reply AI features.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableSmartReply", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableSmartReply")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableSmartReply", 1)],
                },
            ];
    }

    // ── NtlmAuthentication ──
    private static class _NtlmAuthentication
    {
        private const string Lsa = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa";
        private const string Msv10 = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\MSV1_0";
        private const string Netlogon = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "ntlma-enforce-client-session-security",
                Label = "NTLM: Enforce NTLMv2 and 128-Bit Encryption for Client Sessions",
                Category = "Security — Account Lockout",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Tags = ["ntlm", "session-security", "encryption", "hardening", "authentication"],
                Description =
                    "Sets NTLMMinClientSec=537395200 (0x20080000) in the LSA key. "
                    + "Requires NTLM client sessions to use NTLMv2 session keys and 128-bit session encryption. "
                    + "Bit flags: 0x00080000 = require NTLM2 session security, 0x20000000 = require 128-bit key. "
                    + "Complements LmCompatibilityLevel by enforcing the session security level independently.",
                ApplyOps = [RegOp.SetDword(Lsa, "NTLMMinClientSec", 537395200)],
                RemoveOps = [RegOp.DeleteValue(Lsa, "NTLMMinClientSec")],
                DetectOps = [RegOp.CheckDword(Lsa, "NTLMMinClientSec", 537395200)],
            },
            new TweakDef
            {
                Id = "ntlma-enforce-server-session-security",
                Label = "NTLM: Enforce NTLMv2 and 128-Bit Encryption for Server Sessions",
                Category = "Security — Account Lockout",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Tags = ["ntlm", "session-security", "encryption", "hardening", "server"],
                Description =
                    "Sets NTLMMinServerSec=537395200 (0x20080000) in the LSA key. "
                    + "Requires NTLM server sessions to accept only NTLMv2 session keys with 128-bit encryption. "
                    + "Prevents clients using weaker LM or NTLMv1 session security from negotiating a session "
                    + "with this server, even if the client attempts to downgrade.",
                ApplyOps = [RegOp.SetDword(Lsa, "NTLMMinServerSec", 537395200)],
                RemoveOps = [RegOp.DeleteValue(Lsa, "NTLMMinServerSec")],
                DetectOps = [RegOp.CheckDword(Lsa, "NTLMMinServerSec", 537395200)],
            },
            new TweakDef
            {
                Id = "ntlma-restrict-ntlm-in-domain",
                Label = "NTLM: Restrict NTLM In-Domain Authentication to Specific Servers",
                Category = "Security — Account Lockout",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                Tags = ["ntlm", "restriction", "domain", "whitelist"],
                Description =
                    "Sets RestrictNTLMInDomain=1 in the LSA key. "
                    + "Restricts NTLM authentication for domain users to an allowlist of specific servers. "
                    + "Values: 0=disabled, 1=audit NTLM, 2=deny all domain NTLM. "
                    + "Setting to 1 allows auditing without blocking — safe to deploy as a monitoring step "
                    + "before moving to 2 (enforce). Combined with AuditNTLMInDomain for full visibility.",
                ApplyOps = [RegOp.SetDword(Lsa, "RestrictNTLMInDomain", 1)],
                RemoveOps = [RegOp.DeleteValue(Lsa, "RestrictNTLMInDomain")],
                DetectOps = [RegOp.CheckDword(Lsa, "RestrictNTLMInDomain", 1)],
            },
            new TweakDef
            {
                Id = "ntlma-audit-logon-events-msv",
                Label = "NTLM: Enable NTLM Logon Auditing in MSV1_0",
                Category = "Security — Account Lockout",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["ntlm", "audit", "logon", "msv10", "monitoring"],
                Description =
                    "Sets NTLMAuditUserAccountLogonEvents=2 in the MSV1_0 subkey. "
                    + "Enables auditing of all NTLM authentication attempts at the MSV1_0 level "
                    + "(the local SAM authentication provider). Value 2 audits all events. "
                    + "Generates additional detail in the Security event log alongside AuditNTLMInDomain.",
                ApplyOps = [RegOp.SetDword(Msv10, "NTLMAuditUserAccountLogonEvents", 2)],
                RemoveOps = [RegOp.DeleteValue(Msv10, "NTLMAuditUserAccountLogonEvents")],
                DetectOps = [RegOp.CheckDword(Msv10, "NTLMAuditUserAccountLogonEvents", 2)],
            },
            new TweakDef
            {
                Id = "ntlma-block-null-session-fallback",
                Label = "NTLM: Block Null Session NTLM Authentication Fallback",
                Category = "Security — Account Lockout",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Tags = ["ntlm", "null-session", "anonymous", "security", "msv10"],
                Description =
                    "Sets allownullsessionfallback=0 in the MSV1_0 subkey. "
                    + "Disables the fallback that allows NTLM to authenticate as a null (anonymous) session "
                    + "when no credentials are provided. Null sessions are used by legacy tools for anonymous "
                    + "enumeration of shares, user accounts, and group memberships. Blocking this closes "
                    + "a common Windows network reconnaissance pathway.",
                ApplyOps = [RegOp.SetDword(Msv10, "allownullsessionfallback", 0)],
                RemoveOps = [RegOp.DeleteValue(Msv10, "allownullsessionfallback")],
                DetectOps = [RegOp.CheckDword(Msv10, "allownullsessionfallback", 0)],
            },
        ];
    }

    // ── NtlmAuthenticationPolicy ──
    private static class _NtlmAuthenticationPolicy
    {
        private const string NtlmWorkKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Connect\NTLMRestrictions";
        private const string NtlmClientKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LanmanWorkstation";
        private const string NtlmServerKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LanmanServer";
        private const string NtlmAuditKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkProvider";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "ntlm-restrict-outgoing-ntlm-to-servers",
                    Label = "Restrict Outgoing NTLM Authentication to Remote Servers",
                    Category = "Security — Ntlm Authentication",
                    Description =
                        "Restricts NTLM authentication for outgoing network connections from this machine. Supports deny-all or allowlist modes; prevents NTLM relay and pass-the-hash attacks via outbound credential exposure.",
                    Tags = ["ntlm", "authentication", "relay", "pass-the-hash", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote =
                        "Restricting outbound NTLM can break access to older servers or NAS devices that do not support Kerberos or NTLMv2. Audit before blocking.",
                    RegistryKeys = [NtlmClientKey],
                    ApplyOps = [RegOp.SetDword(NtlmClientKey, "RestrictSendingNTLMTraffic", 2)],
                    RemoveOps = [RegOp.DeleteValue(NtlmClientKey, "RestrictSendingNTLMTraffic")],
                    DetectOps = [RegOp.CheckDword(NtlmClientKey, "RestrictSendingNTLMTraffic", 2)],
                },
                new TweakDef
                {
                    Id = "ntlm-block-incoming-ntlm-auth",
                    Label = "Block Incoming NTLM Authentication on This Server",
                    Category = "Security — Ntlm Authentication",
                    Description =
                        "Prevents this machine from accepting NTLM authentication for inbound network connections. Forces clients to negotiate a stronger protocol (Kerberos or NegotiateExt). Effective against NTLM relay to this host.",
                    Tags = ["ntlm", "authentication", "server", "relay-protection", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 3,
                    ImpactNote =
                        "Completely eliminates NTLM relay attack surface on this host. Will break legacy clients that cannot negotiate Kerberos; test extensively.",
                    RegistryKeys = [NtlmServerKey],
                    ApplyOps = [RegOp.SetDword(NtlmServerKey, "RestrictReceivingNTLMTraffic", 2)],
                    RemoveOps = [RegOp.DeleteValue(NtlmServerKey, "RestrictReceivingNTLMTraffic")],
                    DetectOps = [RegOp.CheckDword(NtlmServerKey, "RestrictReceivingNTLMTraffic", 2)],
                },
                new TweakDef
                {
                    Id = "ntlm-audit-outgoing-ntlm-traffic",
                    Label = "Audit Outgoing NTLM Authentication Requests",
                    Category = "Security — Ntlm Authentication",
                    Description =
                        "Enables audit-only mode for outbound NTLM authentication, logging every server to which this machine sends NTLM credentials. Use audit data to build an allowlist before enforcing restrictions.",
                    Tags = ["ntlm", "authentication", "audit", "event-log", "compliance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Non-disruptive; only produces event log entries. Essential preparatory step before enforcing NTLM restrictions.",
                    RegistryKeys = [NtlmClientKey],
                    ApplyOps = [RegOp.SetDword(NtlmClientKey, "AuditReceivingNTLMTraffic", 2)],
                    RemoveOps = [RegOp.DeleteValue(NtlmClientKey, "AuditReceivingNTLMTraffic")],
                    DetectOps = [RegOp.CheckDword(NtlmClientKey, "AuditReceivingNTLMTraffic", 2)],
                },
                new TweakDef
                {
                    Id = "ntlm-disable-ntlmv1",
                    Label = "Disable NTLMv1 Authentication (Require NTLMv2 Minimum)",
                    Category = "Security — Ntlm Authentication",
                    Description =
                        "Configures the LAN Manager authentication level to refuse NTLMv1 and LM authentication. Requires NTLMv2 as the minimum level, or Kerberos where available. NTLMv1 is cryptographically weak and bruteforceable.",
                    Tags = ["ntlm", "ntlmv1", "lm-hash", "authentication-level", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "NTLMv1 and LM hashes can be cracked in minutes. Setting LmCompatibilityLevel=5 enforces NTLMv2+; legacy Windows 9x clients cannot authenticate.",
                    RegistryKeys = [NtlmClientKey],
                    ApplyOps = [RegOp.SetDword(NtlmClientKey, "LmCompatibilityLevel", 5)],
                    RemoveOps = [RegOp.DeleteValue(NtlmClientKey, "LmCompatibilityLevel")],
                    DetectOps = [RegOp.CheckDword(NtlmClientKey, "LmCompatibilityLevel", 5)],
                },
                new TweakDef
                {
                    Id = "ntlm-require-session-security",
                    Label = "Require NTLMv2 Session Security (128-bit Encryption)",
                    Category = "Security — Ntlm Authentication",
                    Description =
                        "Enforces 128-bit message confidentiality and integrity for all NTLM-based network sessions. Clients and servers that do not support 128-bit NTLMv2 session security cannot establish NTLM sessions.",
                    Tags = ["ntlm", "session-security", "encryption", "ntlmv2", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "Requires both message confidentiality and integrity (537395200 = 0x20080000 bitmask). Network monitoring tools and legacy SMB clients may fail.",
                    RegistryKeys = [NtlmClientKey],
                    ApplyOps = [RegOp.SetDword(NtlmClientKey, "MinimumSessionSecurity", 537395200)],
                    RemoveOps = [RegOp.DeleteValue(NtlmClientKey, "MinimumSessionSecurity")],
                    DetectOps = [RegOp.CheckDword(NtlmClientKey, "MinimumSessionSecurity", 537395200)],
                },
                new TweakDef
                {
                    Id = "ntlm-block-ntlm-over-http",
                    Label = "Block NTLM Authentication over HTTP (IWA Web Authentication)",
                    Category = "Security — Ntlm Authentication",
                    Description =
                        "Prevents Integrated Windows Authentication from presenting NTLM credentials to web proxies and HTTP endpoints. Restricts IWA to Kerberos-capable servers only, preventing NTLM relay via HTTP.",
                    Tags = ["ntlm", "http", "iwa", "web-auth", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote =
                        "Can break intranet web apps that rely on NTLM-based IWA. Ensure web servers support Kerberos IWA (SPN registered, constrained delegation configured) before enabling.",
                    RegistryKeys = [NtlmWorkKey],
                    ApplyOps = [RegOp.SetDword(NtlmWorkKey, "BlockNTLMOverHTTP", 1)],
                    RemoveOps = [RegOp.DeleteValue(NtlmWorkKey, "BlockNTLMOverHTTP")],
                    DetectOps = [RegOp.CheckDword(NtlmWorkKey, "BlockNTLMOverHTTP", 1)],
                },
                new TweakDef
                {
                    Id = "ntlm-require-extended-protection",
                    Label = "Require Extended Protection for NTLM Authentication",
                    Category = "Security — Ntlm Authentication",
                    Description =
                        "Enables Extended Protection for Authentication (EPA), binding NTLM tokens to the TLS channel they're sent over. Prevents cross-protocol NTLM relay even when credentials are intercepted in transit.",
                    Tags = ["ntlm", "epa", "extended-protection", "channel-binding", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "Most effective defence against NTLM relay. Requires server-side EPA support (IIS, LDAP, SMB). Old application servers may not support EPA.",
                    RegistryKeys = [NtlmServerKey],
                    ApplyOps = [RegOp.SetDword(NtlmServerKey, "ExtendedProtectionForAuthentication", 2)],
                    RemoveOps = [RegOp.DeleteValue(NtlmServerKey, "ExtendedProtectionForAuthentication")],
                    DetectOps = [RegOp.CheckDword(NtlmServerKey, "ExtendedProtectionForAuthentication", 2)],
                },
                new TweakDef
                {
                    Id = "ntlm-audit-all-domain-ntlm",
                    Label = "Enable Domain-Level NTLM Authentication Audit",
                    Category = "Security — Ntlm Authentication",
                    Description =
                        "Configures domain-level NTLM auditing to log every NTLM authentication event across the domain, including accounts, client names, and server names. Essential for NTLM-reduction baselining.",
                    Tags = ["ntlm", "domain", "audit", "event-log", "active-directory"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Generates high-volume events on Active Directory DCs during initial audit; filter before forwarding to SIEM.",
                    RegistryKeys = [NtlmAuditKey],
                    ApplyOps = [RegOp.SetDword(NtlmAuditKey, "AuditNTLMAuthenticationInDomain", 7)],
                    RemoveOps = [RegOp.DeleteValue(NtlmAuditKey, "AuditNTLMAuthenticationInDomain")],
                    DetectOps = [RegOp.CheckDword(NtlmAuditKey, "AuditNTLMAuthenticationInDomain", 7)],
                },
                new TweakDef
                {
                    Id = "ntlm-enable-server-allowlist",
                    Label = "Enable NTLM Server Allowlist (Exception List) Enforcement",
                    Category = "Security — Ntlm Authentication",
                    Description =
                        "Activates enforcement of the NTLM server exception allowlist, permitting NTLM only to servers explicitly named in the AllowlistedServers value. All other NTLM outbound traffic is blocked.",
                    Tags = ["ntlm", "allowlist", "exception", "server-list", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "Must define AllowlistedServers (Multi-SZ) before enabling enforcement; otherwise all NTLM outbound traffic is blocked.",
                    RegistryKeys = [NtlmClientKey],
                    ApplyOps = [RegOp.SetDword(NtlmClientKey, "EnableNTLMAllowList", 1)],
                    RemoveOps = [RegOp.DeleteValue(NtlmClientKey, "EnableNTLMAllowList")],
                    DetectOps = [RegOp.CheckDword(NtlmClientKey, "EnableNTLMAllowList", 1)],
                },
                new TweakDef
                {
                    Id = "ntlm-block-ntlm-to-ldap",
                    Label = "Block NTLM Authentication to LDAP (Require Kerberos for AD)",
                    Category = "Security — Ntlm Authentication",
                    Description =
                        "Prevents LDAP clients on this machine from using NTLM to authenticate to Active Directory Domain Controllers. Forces Kerberos-based LDAP authentication, eliminating LDAP relay attack vectors.",
                    Tags = ["ntlm", "ldap", "active-directory", "kerberos", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote =
                        "Can break LDAP queries from scripts or tools that hardcode NTLM. Verify all LDAP clients can use Kerberos before enforcing.",
                    RegistryKeys = [NtlmWorkKey],
                    ApplyOps = [RegOp.SetDword(NtlmWorkKey, "BlockNTLMToLDAP", 1)],
                    RemoveOps = [RegOp.DeleteValue(NtlmWorkKey, "BlockNTLMToLDAP")],
                    DetectOps = [RegOp.CheckDword(NtlmWorkKey, "BlockNTLMToLDAP", 1)],
                },
            ];
    }

    // ── NtlmAuthPolicy ──
    private static class _NtlmAuthPolicy
    {
        private const string LsaKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa";
        private const string NetlogonKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters";
        private const string MsvKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\MSV1_0";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "ntlm-require-ntlmv2-session-security-128",
                Label = "Require 128-bit NTLMv2 Session Security",
                Category = "Security — Ntlm Authentication",
                Description = "Forces NTLM session security to use 128-bit encryption and NTLMv2 message integrity.",
                Tags = ["ntlm", "session-security", "encryption", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Ensures NTLM sessions use strong encryption. Breaks connections to legacy systems without 128-bit support.",
                ApplyOps = [RegOp.SetDword(MsvKey, "NTLMMinClientSec", 537395200)],
                RemoveOps = [RegOp.DeleteValue(MsvKey, "NTLMMinClientSec")],
                DetectOps = [RegOp.CheckDword(MsvKey, "NTLMMinClientSec", 537395200)],
            },
            new TweakDef
            {
                Id = "ntlm-require-server-ntlmv2-128",
                Label = "Require 128-bit NTLMv2 Server Session Security",
                Category = "Security — Ntlm Authentication",
                Description = "Forces the NTLM server to require 128-bit session security and NTLMv2 from clients.",
                Tags = ["ntlm", "server", "session-security", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Prevents downgrade of session security on server side. Rejects NTLMv1 client connections.",
                ApplyOps = [RegOp.SetDword(MsvKey, "NTLMMinServerSec", 537395200)],
                RemoveOps = [RegOp.DeleteValue(MsvKey, "NTLMMinServerSec")],
                DetectOps = [RegOp.CheckDword(MsvKey, "NTLMMinServerSec", 537395200)],
            },
            new TweakDef
            {
                Id = "ntlm-restrict-outbound-to-domain",
                Label = "Restrict NTLM Outbound Traffic to Domain Servers Only",
                Category = "Security — Ntlm Authentication",
                Description = "Denies NTLM authentication to servers outside the local domain (blocks NTLM relay to internet).",
                Tags = ["ntlm", "outbound", "relay", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "Prevents NTLM relay to external servers. May break workgroup or cloud SMB scenarios.",
                ApplyOps = [RegOp.SetDword(LsaKey, "RestrictSendingNTLMTraffic", 2)],
                RemoveOps = [RegOp.DeleteValue(LsaKey, "RestrictSendingNTLMTraffic")],
                DetectOps = [RegOp.CheckDword(LsaKey, "RestrictSendingNTLMTraffic", 2)],
            },
            new TweakDef
            {
                Id = "ntlm-deny-inbound-ntlm",
                Label = "Deny All Inbound NTLM Authentication",
                Category = "Security — Ntlm Authentication",
                Description = "Blocks the local server from accepting NTLM authentication from any client.",
                Tags = ["ntlm", "inbound", "hardening", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 2,
                ImpactNote = "Fully blocks NTLM inbound. Only safe in Kerberos-only environments; breaks SMB file sharing for downlevel clients.",
                ApplyOps = [RegOp.SetDword(LsaKey, "RestrictReceivingNTLMTraffic", 2)],
                RemoveOps = [RegOp.DeleteValue(LsaKey, "RestrictReceivingNTLMTraffic")],
                DetectOps = [RegOp.CheckDword(LsaKey, "RestrictReceivingNTLMTraffic", 2)],
            },
            new TweakDef
            {
                Id = "ntlm-enable-audit-incoming",
                Label = "Enable NTLM Audit for Incoming Authentication",
                Category = "Security — Ntlm Authentication",
                Description = "Logs all incoming NTLM authentication attempts to the security event log for review.",
                Tags = ["ntlm", "audit", "logging", "monitoring"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Enables visibility into NTLM usage; essential before blocking NTLM to discover dependencies.",
                ApplyOps = [RegOp.SetDword(LsaKey, "AuditReceivingNTLMTraffic", 2)],
                RemoveOps = [RegOp.DeleteValue(LsaKey, "AuditReceivingNTLMTraffic")],
                DetectOps = [RegOp.CheckDword(LsaKey, "AuditReceivingNTLMTraffic", 2)],
            },
            new TweakDef
            {
                Id = "ntlm-enable-audit-outgoing",
                Label = "Enable NTLM Audit for Outgoing Authentication",
                Category = "Security — Ntlm Authentication",
                Description = "Logs all outgoing NTLM authentication attempts to identify apps still using NTLM.",
                Tags = ["ntlm", "audit", "outbound", "logging"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Use before restricting outbound NTLM to identify all callers.",
                ApplyOps = [RegOp.SetDword(LsaKey, "AuditNTLMInDomain", 7)],
                RemoveOps = [RegOp.DeleteValue(LsaKey, "AuditNTLMInDomain")],
                DetectOps = [RegOp.CheckDword(LsaKey, "AuditNTLMInDomain", 7)],
            },
        ];
    }

    // ── NtlmRestrictionPolicy ──
    private static class _NtlmRestrictionPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\MSV1_0";
        private const string LsaKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa";
        private const string PolKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "ntlmadv-enable-extended-protection",
                    Label = "Enable NTLM Extended Protection for Authentication (EPA)",
                    Category = "Security — Ntlm Authentication",
                    Description =
                        "Enables NTLM Extended Protection for Authentication (EPA/CBT) on the client, adding channel binding tokens to NTLM authentication ensuring credentials can only be used on the TLS channel over which they were captured, preventing relay attacks.",
                    Tags = ["ntlm", "epa", "channel-binding", "relay-attack", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "NTLM EPA/CBT enabled; captured NTLM credentials cannot be replayed on a different TLS channel.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableExtendedProtection", 2)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableExtendedProtection")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableExtendedProtection", 2)],
                },
                new TweakDef
                {
                    Id = "ntlmadv-disable-lm-hash-storage",
                    Label = "Disable LAN Manager (LM) Hash Storage in SAM",
                    Category = "Security — Ntlm Authentication",
                    Description =
                        "Prevents Windows from storing LAN Manager password hashes in the local SAM database, eliminating the easily cracked LM hash from credential stores that could be extracted by credential dumping tools.",
                    Tags = ["ntlm", "lm-hash", "sam", "credential-dump", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "LM hash storage disabled; SAM/LSASS no longer contains crackable LM password hashes.",
                    ApplyOps = [RegOp.SetDword(Key, "NoLMHash", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NoLMHash")],
                    DetectOps = [RegOp.CheckDword(Key, "NoLMHash", 1)],
                },
                new TweakDef
                {
                    Id = "ntlmadv-restrict-incoming-ntlm",
                    Label = "Restrict Incoming NTLM Authentication to Domain Accounts Only",
                    Category = "Security — Ntlm Authentication",
                    Description =
                        "Configures the server component to only accept NTLM authentication from accounts in the domain (rejecting local SAM account NTLM), reducing the attack surface from pass-the-hash attacks using local account credentials.",
                    Tags = ["ntlm", "incoming", "domain-only", "pass-the-hash", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Incoming NTLM restricted to domain accounts; local SAM account NTLM authentication blocked.",
                    ApplyOps = [RegOp.SetDword(Key, "RestrictReceivingNTLMTraffic", 2)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RestrictReceivingNTLMTraffic")],
                    DetectOps = [RegOp.CheckDword(Key, "RestrictReceivingNTLMTraffic", 2)],
                },
                new TweakDef
                {
                    Id = "ntlmadv-enable-outbound-audit",
                    Label = "Enable Audit Logging for Outbound NTLM Authentication",
                    Category = "Security — Ntlm Authentication",
                    Description =
                        "Enables audit logging for all outbound NTLM authentication attempts from this machine, providing visibility into which remote servers receive NTLM credentials from applications running on this endpoint.",
                    Tags = ["ntlm", "outbound-audit", "credential-exposure", "logging", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Outbound NTLM audit enabled; all remote servers receiving NTLM credentials from this machine logged.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditSendingNTLMTraffic", 2)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditSendingNTLMTraffic")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditSendingNTLMTraffic", 2)],
                },
                new TweakDef
                {
                    Id = "ntlmadv-disable-ntlm-auth-in-smb",
                    Label = "Require Kerberos Authentication for SMB (Block NTLM in SMB)",
                    Category = "Security — Ntlm Authentication",
                    Description =
                        "Configures SMB over network shares to require Kerberos authentication rather than NTLM, preventing NTLM relay attacks that capture and forward SMB authentication to remote shares or other services.",
                    Tags = ["ntlm", "smb", "kerberos", "relay-attack", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "SMB NTLM authentication blocked; Kerberos required for network share access. Relay attacks via SMB mitigated.",
                    ApplyOps = [RegOp.SetDword(PolKey, "RequireDomainAuthForSMB", 1)],
                    RemoveOps = [RegOp.DeleteValue(PolKey, "RequireDomainAuthForSMB")],
                    DetectOps = [RegOp.CheckDword(PolKey, "RequireDomainAuthForSMB", 1)],
                },
                new TweakDef
                {
                    Id = "ntlmadv-disable-ntlm-telemetry",
                    Label = "Disable NTLM Authentication Telemetry to Microsoft",
                    Category = "Security — Ntlm Authentication",
                    Description =
                        "Prevents the Windows NTLM authentication provider from sending authentication success/failure rates, cipher usage, and session fallback telemetry to Microsoft.",
                    Tags = ["ntlm", "telemetry", "privacy", "microsoft", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "NTLM telemetry to Microsoft disabled; auth stats and session data not sent to cloud.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableNTLMTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableNTLMTelemetry")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableNTLMTelemetry", 1)],
                },
            ];
    }

    // ── ProcessMitigationPolicy ──
    private static class _ProcessMitigationPolicy
    {
        private const string KernelCtl = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel";
        private const string MemMgmt = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management";
        private const string SessMgr = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager";
        private const string LsaMain = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa";
        private const string KernelAudit = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Executive";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "prctmtg-enable-sehop",
                Label = "Process Mitigation: Enable SEHOP (SEH Overwrite Protection)",
                Category = "Security — Ntlm Authentication",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [KernelCtl],
                Tags = ["sehop", "exploit-mitigation", "seh", "security", "hardening"],
                Description =
                    "Sets DisableExceptionChainValidation=0 in kernel settings. Enables SEHOP which "
                    + "validates the integrity of the Structured Exception Handler chain before allowing "
                    + "an exception handler to execute. Mitigates SEH overwrite exploits. "
                    + "Default: 0 (enabled) on Windows Server; some client SKUs default to 1.",
                ApplyOps = [RegOp.SetDword(KernelCtl, "DisableExceptionChainValidation", 0)],
                RemoveOps = [RegOp.DeleteValue(KernelCtl, "DisableExceptionChainValidation")],
                DetectOps = [RegOp.CheckDword(KernelCtl, "DisableExceptionChainValidation", 0)],
            },
            new TweakDef
            {
                Id = "prctmtg-enable-heap-termination-on-corruption",
                Label = "Process Mitigation: Enable Heap Termination on Corruption",
                Category = "Security — Ntlm Authentication",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [KernelCtl],
                Tags = ["heap", "corruption", "termination", "exploit-mitigation", "security"],
                Description =
                    "Sets HeapDeCommitFreeBlockThreshold=0 in kernel with heap checking. Ensures that "
                    + "any detected heap corruption terminates the affected process immediately. "
                    + "Sets GlobalFlag to include heap verification flags (0x20). "
                    + "Default: process may continue after heap corruption allowing further exploitation.",
                ApplyOps = [RegOp.SetDword(SessMgr, "GlobalFlag", 0x20)],
                RemoveOps = [RegOp.DeleteValue(SessMgr, "GlobalFlag")],
                DetectOps = [RegOp.CheckDword(SessMgr, "GlobalFlag", 0x20)],
            },
            new TweakDef
            {
                Id = "prctmtg-enable-mandatory-aslr",
                Label = "Process Mitigation: Enable Mandatory ASLR System-Wide",
                Category = "Security — Ntlm Authentication",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [MemMgmt],
                Tags = ["aslr", "randomisation", "exploit-mitigation", "security", "hardening"],
                Description =
                    "Sets MoveImages=1 in Memory Management. Forces ASLR randomisation for all executable "
                    + "images loaded into memory, even those not compiled with /DYNAMICBASE. "
                    + "Default: 0 (optional). Mandatory ASLR significantly raises the cost of exploitation.",
                ApplyOps = [RegOp.SetDword(MemMgmt, "MoveImages", 1)],
                RemoveOps = [RegOp.DeleteValue(MemMgmt, "MoveImages")],
                DetectOps = [RegOp.CheckDword(MemMgmt, "MoveImages", 1)],
            },
            new TweakDef
            {
                Id = "prctmtg-enable-bottom-up-aslr",
                Label = "Process Mitigation: Enable Bottom-Up ASLR (Stack / Heap Randomisation)",
                Category = "Security — Ntlm Authentication",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [MemMgmt],
                Tags = ["aslr", "bottom-up", "stack", "heap", "exploit-mitigation", "security"],
                Description =
                    "Sets EnableBottomUpRandomization=1 in Memory Management. Applies ASLR to "
                    + "heap allocations, stack and other bottom-up allocations in addition to image base. "
                    + "Default: 0. Provides entropy against stack/heap spray attacks.",
                ApplyOps = [RegOp.SetDword(MemMgmt, "EnableBottomUpRandomization", 1)],
                RemoveOps = [RegOp.DeleteValue(MemMgmt, "EnableBottomUpRandomization")],
                DetectOps = [RegOp.CheckDword(MemMgmt, "EnableBottomUpRandomization", 1)],
            },
            new TweakDef
            {
                Id = "prctmtg-enable-high-entropy-aslr",
                Label = "Process Mitigation: Enable High-Entropy ASLR (64-bit Processes)",
                Category = "Security — Ntlm Authentication",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [MemMgmt],
                Tags = ["aslr", "entropy", "64bit", "exploit-mitigation", "security"],
                Description =
                    "Sets EnableHighEntropyASLR=1 in Memory Management. Uses the full 64-bit address "
                    + "space entropy for 64-bit processes compiled with /HIGHENTROPYVA. "
                    + "Default: 0. High entropy exponentially increases brute-force difficulty.",
                ApplyOps = [RegOp.SetDword(MemMgmt, "EnableHighEntropyASLR", 1)],
                RemoveOps = [RegOp.DeleteValue(MemMgmt, "EnableHighEntropyASLR")],
                DetectOps = [RegOp.CheckDword(MemMgmt, "EnableHighEntropyASLR", 1)],
            },
            new TweakDef
            {
                Id = "prctmtg-enable-kernel-stack-protection",
                Label = "Process Mitigation: Enable Kernel Stack Cookie Protection",
                Category = "Security — Ntlm Authentication",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [KernelCtl],
                Tags = ["kernel", "stack", "cookie", "security", "mitigation", "hardening"],
                Description =
                    "Sets KernelStackCookies=1 in kernel settings to enforce stack canary values "
                    + "in kernel-mode functions. Detects kernel stack buffer overflows before return. "
                    + "Default: implementation-defined. Explicit opt-in ensures protection is active.",
                ApplyOps = [RegOp.SetDword(KernelCtl, "KernelStackCookies", 1)],
                RemoveOps = [RegOp.DeleteValue(KernelCtl, "KernelStackCookies")],
                DetectOps = [RegOp.CheckDword(KernelCtl, "KernelStackCookies", 1)],
            },
            new TweakDef
            {
                Id = "prctmtg-protect-svc-with-emet",
                Label = "Process Mitigation: Enable Kernel Patch Protection (KPP) Enforcement",
                Category = "Security — Ntlm Authentication",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [KernelCtl],
                Tags = ["kpp", "patchguard", "kernel", "protect", "security"],
                Description =
                    "Sets BpbEnabled=1 in kernel settings. Ensures Branch Prediction Buffer mitigation "
                    + "is enabled against Spectre-class speculative execution vulnerabilities. "
                    + "Default: 1 (enabled when microcode available). Explicit enforcement prevents disablement.",
                ApplyOps = [RegOp.SetDword(KernelCtl, "BpbEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(KernelCtl, "BpbEnabled")],
                DetectOps = [RegOp.CheckDword(KernelCtl, "BpbEnabled", 1)],
            },
        ];
    }

    // ── SecureChannelPolicy ──
    private static class _SecureChannelPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetLogon";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "secchan-require-secure-channel-signing",
                Label = "Require Signing on Domain Secure Channel Communication",
                Category = "Security — Ntlm Authentication",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "Domain secure channel signing ensures that NetLogon traffic between domain members and domain controllers is cryptographically signed to prevent replay and man-in-the-middle attacks. Requiring secure channel signing prevents an attacker from intercepting and replaying domain authentication traffic to authenticate as legitimate domain users. Unsigned secure channel traffic can be captured and manipulated by an attacker with network access to forge authentication responses from a domain controller. Secure channel signing is required in addition to sealing to provide both integrity and confidentiality protection for domain authentication. Organizations with older legacy clients that do not support secure channel signing must upgrade those systems before requiring signing across the domain. Microsoft requires secure channel signing in all hardened domain configurations and it is enforced in Windows Server 2022 default hardening baselines.",
                Tags = ["secure-channel", "signing", "netlogon", "authentication", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequireSignOrSeal", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireSignOrSeal")],
                DetectOps = [RegOp.CheckDword(Key, "RequireSignOrSeal", 1)],
            },
            new TweakDef
            {
                Id = "secchan-require-secure-channel-sealing",
                Label = "Require Encryption Sealing on Domain Secure Channel",
                Category = "Security — Ntlm Authentication",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "Secure channel sealing encrypts all NetLogon traffic between domain members and domain controllers protecting authentication data from network eavesdropping. Requiring secure channel sealing prevents credential harvesting from network captures by encrypting all domain authentication communication. Secure channel sealing uses the session key negotiated during the NetLogon authentication to provide symmetric encryption of subsequent traffic. The combination of signing and sealing provides both integrity and confidentiality protection for domain authentication channels. Organizations should enable both RequireSignOrSeal and SealSecureChannel to enforce maximum security on domain communication. Secure channel sealing has negligible performance impact on modern systems and should be enabled universally across all domain-joined systems.",
                Tags = ["secure-channel", "sealing", "encryption", "netlogon", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "SealSecureChannel", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "SealSecureChannel")],
                DetectOps = [RegOp.CheckDword(Key, "SealSecureChannel", 1)],
            },
            new TweakDef
            {
                Id = "secchan-require-strong-session-key",
                Label = "Require Strong Session Keys for Domain Secure Channel",
                Category = "Security — Ntlm Authentication",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Strong secure channel session keys require the use of 128-bit encryption for NetLogon channel protection replacing older 40-bit and 56-bit keys that are trivially crackable. Requiring strong session keys ensures that encrypted secure channel traffic cannot be decrypted by an attacker even with captured traffic and offline attacks. Older Windows NT systems used 40-bit session keys that can be broken in minutes on modern hardware making strong key requirements essential for current environments. Strong session key requirements are enabled by default on Windows Vista and later but should be explicitly required through policy to prevent negotiation of weaker keys. Organizations must ensure all domain-joined systems running Windows Vista or later which support 128-bit keys before enabling this requirement. The transition from weak to strong session keys is part of the NetLogon hardening changes in the Zerologon vulnerability remediation.",
                Tags = ["secure-channel", "session-keys", "encryption", "netlogon", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequireStrongKey", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireStrongKey")],
                DetectOps = [RegOp.CheckDword(Key, "RequireStrongKey", 1)],
            },
            new TweakDef
            {
                Id = "secchan-enable-full-netlogon-audit",
                Label = "Enable Full Audit Logging for NetLogon Secure Channel Events",
                Category = "Security — Ntlm Authentication",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "NetLogon audit logging captures secure channel establishment failures and anomalous authentication events for security monitoring and incident response. Enabling full NetLogon audit creates visibility into domain authentication attempts that may indicate credential attacks or infrastructure problems. NetLogon failures during secure channel establishment can indicate Zerologon-style attacks or misconfigured domain trust relationships. The NetLogon log is stored on domain controllers and should be forwarded to SIEM for centralized analysis and alerting. Domain controller NetLogon log data combined with Security event log data provides comprehensive coverage of authentication threats. NetLogon audit data is particularly valuable during incident response to trace lateral movement paths through domain authentication.",
                Tags = ["secure-channel", "audit", "netlogon", "monitoring", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "FullSecureChannelAudit", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "FullSecureChannelAudit")],
                DetectOps = [RegOp.CheckDword(Key, "FullSecureChannelAudit", 1)],
            },
            new TweakDef
            {
                Id = "secchan-disable-vulnerable-netlogon",
                Label = "Block Vulnerable NetLogon Connections from Non-Compliant Devices",
                Category = "Security — Ntlm Authentication",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                Description =
                    "The Zerologon vulnerability (CVE-2020-1472) exploits a flaw in the NetLogon secure channel allowing unauthenticated clients to set machine account passwords and gain domain administrator access. Blocking vulnerable NetLogon connections enforces the second phase of CVE-2020-1472 remediation denying connections from clients that do not use secure RPC. Microsoft released this as a phased fix where the initial patch logged vulnerable connections and the second phase blocked them outright. All Windows clients released after 2016 use secure RPC for NetLogon by default making this restriction safe for modern environments. Non-Windows devices like Linux Samba domain members may require updates to support secure RPC NetLogon before enforcement mode can be enabled. Organizations should review the NetLogon event log for any vulnerable client connections before enabling enforcement mode.",
                Tags = ["secure-channel", "zerologon", "cve-2020-1472", "netlogon", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "FullSecureChannelProtection", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "FullSecureChannelProtection")],
                DetectOps = [RegOp.CheckDword(Key, "FullSecureChannelProtection", 1)],
            },
            new TweakDef
            {
                Id = "secchan-set-machine-account-password-age",
                Label = "Set Maximum Machine Account Password Age for Secure Channel",
                Category = "Security — Ntlm Authentication",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Machine account password age controls how frequently domain-joined computers automatically rotate their computer account passwords as part of secure channel maintenance. Setting maximum machine account password age ensures regular rotation of computer credentials that are used to establish the secure channel to domain controllers. Computer account passwords are automatically changed every 30 days by default in Active Directory but this setting can enforce a shorter maximum. Environments with compliance requirements for credential rotation should set machine account password age to align with their policy requirements. Machine accounts with very old passwords may indicate stale or abandoned systems that should be reviewed for decommissioning. Disabling machine account password changes creates long-lived credentials that are a security risk if the machine is compromised.",
                Tags = ["secure-channel", "machine-account", "password-rotation", "netlogon", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "MaximumPasswordAge", 30)],
                RemoveOps = [RegOp.DeleteValue(Key, "MaximumPasswordAge")],
                DetectOps = [RegOp.CheckDword(Key, "MaximumPasswordAge", 30)],
            },
            new TweakDef
            {
                Id = "secchan-disable-machine-account-password-changes",
                Label = "Prevent Disabling of Automatic Machine Account Password Changes",
                Category = "Security — Ntlm Authentication",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Automatic machine account password rotation maintains the security of domain trust relationships by regularly changing computer account credentials. Preventing administrators from disabling machine account password changes ensures that automated credential rotation is not bypassed which would create static long-term credentials. Some organizations disable machine account password changes for problematic legacy systems but this creates long-lived credentials that weaken domain security. Machine account password change failures are often the cause of systems falling off the domain which should be addressed by fixing root causes rather than disabling rotation. DisablePasswordChange set to 1 is a common misconfiguration that should be detected and remediated during security assessments. Domain controllers should always enforce machine account password rotation as part of a healthy Active Directory environment.",
                Tags = ["secure-channel", "machine-account", "password-changes", "netlogon", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePasswordChange", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePasswordChange")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePasswordChange", 0)],
            },
            new TweakDef
            {
                Id = "secchan-restrict-domain-controller-replication",
                Label = "Restrict Unauthorized Domain Controller Replication Requests",
                Category = "Security — Ntlm Authentication",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                Description =
                    "DC Sync attacks exploit domain replication rights to steal password hashes from Active Directory without requiring code execution on a domain controller. Restricting domain controller replication ensures that only authorized domain controller accounts have rights to request replication of sensitive AD information. DCSync attacks use DRSUAPI replication calls to extract KRBTGT and other sensitive account hashes enabling further credential attacks. Replication rights should be limited exclusively to accounts with the DS-Replication-Get-Changes and DS-Replication-Get-Changes-All permissions that are domain controllers and select administrative accounts. Monitoring for DS-Replication-Get-Changes events from non-domain-controller accounts is a high-fidelity detection indicator for DCSync attacks. Organizations should audit replication permissions quarterly and remove any accounts that do not require these rights.",
                Tags = ["secure-channel", "dcsync", "replication", "netlogon", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictDCReplication", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictDCReplication")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictDCReplication", 1)],
            },
            new TweakDef
            {
                Id = "secchan-enforce-netlogon-service-hardening",
                Label = "Enforce NetLogon Service Security Hardening Settings",
                Category = "Security — Ntlm Authentication",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "NetLogon service hardening enforces additional security constraints on the NetLogon service that handles domain authentication preventing exploitation of service vulnerabilities. Enabling NetLogon service hardening sets a higher security bar for all NetLogon operations reducing the attack surface available to vulnerabilities like Zerologon. NetLogon service hardening was introduced as part of the CVE-2020-1472 remediation and Microsoft recommends it for all domain-joined systems. Service hardening configuration ensures the NetLogon service uses maximally secure communication parameters for all domain authentication operations. Organizations should monitor for NetLogon service failures after enabling hardening as service misconfiguration may prevent authentication. Testing NetLogon hardening in a non-production environment is recommended before wide deployment in complex multi-domain environments.",
                Tags = ["secure-channel", "service-hardening", "netlogon", "zero-day", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ServiceHardeningEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "ServiceHardeningEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "ServiceHardeningEnabled", 1)],
            },
            new TweakDef
            {
                Id = "secchan-set-account-lockout-on-channel-failure",
                Label = "Enable Account Lockout after Secure Channel Authentication Failures",
                Category = "Security — Ntlm Authentication",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Account lockout on secure channel authentication failure provides protection against brute-force attacks that attempt to guess machine account passwords or negotiate vulnerable secure channel parameters. Enabling lockout after repeated secure channel failures prevents automated attacks from making unlimited authentication guesses against domain accounts. Secure channel authentication failure lockout thresholds should be set high enough to avoid locking out systems with legitimate authentication problems such as time synchronization issues. The lockout threshold for machine accounts should be higher than interactive user accounts since machines do not have users who can notice and unlock their accounts. Organizations should implement account lockout monitoring to identify systems triggering lockout as this indicates either misconfiguration or active attack attempts. Automatically unlocking accounts after a short observation period balances security protection with operational availability.",
                Tags = ["secure-channel", "account-lockout", "brute-force", "netlogon", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "LockoutOnChannelFailure", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "LockoutOnChannelFailure")],
                DetectOps = [RegOp.CheckDword(Key, "LockoutOnChannelFailure", 1)],
            },
        ];
    }

    // ── SecureConnectionPolicy ──
    private static class _SecureConnectionPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SecureConnections";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "seccxn-disable-tls-10-protocol",
                Label = "Disable TLS 1.0 Protocol for All System Secure Connections",
                Category = "Security — Ntlm Authentication",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                Description =
                    "TLS 1.0 is a deprecated protocol with known vulnerabilities including POODLE and BEAST that allow attackers to decrypt TLS-protected traffic under specific conditions. Disabling TLS 1.0 at the system level enforces the use of TLS 1.2 or TLS 1.3 for all Windows Schannel-based connections preventing negotiation of the weak TLS 1.0 protocol. Organizations in regulated industries are required by PCI-DSS HIPAA and other frameworks to disable TLS 1.0 for transmission of regulated data. Compatibility impact should be assessed before disabling TLS 1.0 as some legacy applications and services may only support older TLS versions. Organizations should perform an inventory of all internal and external service dependencies to identify any that require TLS 1.0 and remediate those dependencies before disabling the protocol. Disabling TLS 1.0 should be accompanied by enabling TLS 1.3 support to ensure that the strongest available protocol is used for connections.",
                Tags = ["tls", "tls-1.0", "protocol-security", "encryption", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableTls10", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableTls10")],
                DetectOps = [RegOp.CheckDword(Key, "DisableTls10", 1)],
            },
            new TweakDef
            {
                Id = "seccxn-disable-tls-11-protocol",
                Label = "Disable TLS 1.1 Protocol for All System Secure Connections",
                Category = "Security — Ntlm Authentication",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                Description =
                    "TLS 1.1 is deprecated by RFC 8996 alongside TLS 1.0 and shares many of the same weaknesses including CBC cipher mode vulnerabilities that were addressed in TLS 1.2. Disabling TLS 1.1 at the system policy level prevents Windows Schannel from negotiating TLS 1.1 connections ensuring only TLS 1.2 and TLS 1.3 are available. Most modern applications and services have updated to support TLS 1.2 making TLS 1.1 disablement generally feasible in well-maintained environments. Organizations should scan their application portfolio for TLS 1.1 dependencies before enforcing this policy to identify applications that require updates. The IANA has assigned TLS 1.1 as historical status meaning it is no longer maintained and future cryptographic attacks may exploit known but currently impractical weaknesses. Disabling TLS 1.1 together with TLS 1.0 eliminates the most widely exploited historical protocol versions and reduces the attack surface for TLS-based attacks.",
                Tags = ["tls", "tls-1.1", "protocol-security", "deprecated-protocols", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableTls11", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableTls11")],
                DetectOps = [RegOp.CheckDword(Key, "DisableTls11", 1)],
            },
            new TweakDef
            {
                Id = "seccxn-enable-tls-13-support",
                Label = "Enable TLS 1.3 Protocol Support for Enhanced Connection Security",
                Category = "Security — Ntlm Authentication",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "TLS 1.3 provides significant security improvements over TLS 1.2 including perfect forward secrecy by default removal of weak cipher suites an improved handshake and reduced latency. Enabling TLS 1.3 at system policy level ensures that connections use the most secure available transport protocol when both endpoints support it. TLS 1.3 eliminates several classes of downgrade attacks that affected TLS 1.2 including padding oracle attacks CBC mode attacks and certain MITM techniques. The improved TLS 1.3 handshake reduces the number of round trips required to establish a connection improving performance particularly for latency-sensitive applications. Organizations should enable TLS 1.3 support and monitor for any compatibility issues with services that do not yet support TLS 1.3 while retaining TLS 1.2 as a fallback. Perfect forward secrecy in TLS 1.3 is particularly valuable for protecting sensitive communications from future decryption if encryption keys are later compromised.",
                Tags = ["tls", "tls-1.3", "forward-secrecy", "encryption", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableTls13", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableTls13")],
                DetectOps = [RegOp.CheckDword(Key, "EnableTls13", 1)],
            },
            new TweakDef
            {
                Id = "seccxn-disable-ssl-30-protocol",
                Label = "Disable SSL 3.0 Protocol to Prevent POODLE Attack Vulnerability",
                Category = "Security — Ntlm Authentication",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "SSL 3.0 is vulnerable to the POODLE attack which allows a network attacker to decrypt 1 byte of plaintext per 256 crafted requests making it possible to recover session cookies and HTTPS protected content. Disabling SSL 3.0 eliminates the POODLE vulnerability and prevents any connection from falling back to SSL 3.0 even when TLS is unavailable. SSL 3.0 has been deprecated since 2015 by RFC 7568 and no legitimate modern services should require SSL 3.0 support. The POODLE attack requires network access and an ability to inject requests but is practical against browsers and HTTPS connections to web applications. Organizations that have not explicitly disabled SSL 3.0 may still have it available in Windows Schannel as a legacy fallback option. Disabling SSL 3.0 together with TLS 1.0 and 1.1 should be standard practice and the combination ensures that only TLS 1.2 and 1.3 are available for secure connections.",
                Tags = ["ssl", "ssl-3.0", "poodle", "protocol-security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableSsl30", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableSsl30")],
                DetectOps = [RegOp.CheckDword(Key, "DisableSsl30", 1)],
            },
            new TweakDef
            {
                Id = "seccxn-restrict-cipher-suite-order",
                Label = "Restrict Cipher Suite Selection to Security-Approved Algorithms",
                Category = "Security — Ntlm Authentication",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Cipher suite ordering policy restricts TLS connections to use only approved cryptographic algorithm combinations preventing negotiation of weak ciphers that might otherwise be selected by clients or servers. Unapproved cipher suites may use weak key exchange algorithms like static RSA without forward secrecy weak symmetric encryption like RC4 or DES or broken hash algorithms like MD5 or SHA-1. NIST SP 800-52 provides cipher suite selection guidance for government systems that should be referenced when defining the approved cipher list. Organizations should review their cipher suite configuration against current NIST or other applicable guidance to identify suites that should be removed. Cipher suite restrictions apply to all Schannel consumers including IIS RDP SMB and PowerShell Remoting connections reducing the attack surface across multiple protocols simultaneously. Regular review of cipher suite policy is necessary as new vulnerabilities in existing algorithms may require removing previously approved suites.",
                Tags = ["tls", "cipher-suites", "cryptographic-agility", "encryption", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictCipherSuiteOrder", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictCipherSuiteOrder")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictCipherSuiteOrder", 1)],
            },
            new TweakDef
            {
                Id = "seccxn-enable-extended-master-secret",
                Label = "Enable Extended Master Secret Support for TLS Session Resumption",
                Category = "Security — Ntlm Authentication",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Extended Master Secret is a TLS extension defined in RFC 7627 that mitigates triple handshake attacks and synchronization attacks on TLS session resumption by binding the master secret to the full handshake transcript. Without Extended Master Secret TLS session resumption is vulnerable to triple handshake attacks that allow a MITM attacker to inject themselves into resumed sessions. Enabling Extended Master Secret support ensures that all TLS connections and session resumptions include the cryptographic binding that prevents these attack classes. The extension is widely supported by modern TLS implementations and enabling it typically does not cause compatibility issues with current services. Organizations should verify that their internal TLS services support Extended Master Secret to avoid session resumption failures when the extension is required for accepting connections. Extended Master Secret is required for TLS 1.3 connections by specification so enabling it for TLS 1.2 connections aligns behavior with TLS 1.3 requirements.",
                Tags = ["tls", "extended-master-secret", "session-resumption", "mitm-prevention", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableExtendedMasterSecret", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableExtendedMasterSecret")],
                DetectOps = [RegOp.CheckDword(Key, "EnableExtendedMasterSecret", 1)],
            },
            new TweakDef
            {
                Id = "seccxn-disable-rc4-cipher",
                Label = "Disable RC4 Stream Cipher for All TLS and Secure Channel Connections",
                Category = "Security — Ntlm Authentication",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "RC4 is a stream cipher with multiple known weaknesses including biases in its keystream that allow statistical decryption attacks with sufficient ciphertext samples. RFC 7465 prohibits the use of RC4 cipher suites in TLS and its use in active connections is considered a security vulnerability. Practical attacks against RC4 in TLS have been demonstrated requiring approximately 2 billion samples to recover session cookies with high confidence. Disabling RC4 at the system level ensures that Windows Schannel will not use RC4 in any context even when peer systems offer it as an option. RC4 was historically used as a performance optimization over AES but modern hardware AES-NI instructions make AES block ciphers faster than RC4 eliminating the performance justification for using RC4. Organizations should verify that RC4 is disabled in all cryptographic library configurations and not just the Windows Schannel context.",
                Tags = ["rc4", "cipher-security", "weak-ciphers", "tls-hardening", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableRC4Cipher", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableRC4Cipher")],
                DetectOps = [RegOp.CheckDword(Key, "DisableRC4Cipher", 1)],
            },
            new TweakDef
            {
                Id = "seccxn-require-certificate-revocation-check",
                Label = "Require Certificate Revocation Status Checks for TLS Connections",
                Category = "Security — Ntlm Authentication",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Certificate revocation checking verifies that TLS certificates have not been revoked before accepting them as valid ensuring that compromised or mis-issued certificates are not trusted. Without revocation checking a certificate that has been revoked due to private key compromise can still be used to impersonate a legitimate service. Revocation checking should use OCSP stapling for performance where the server provides current revocation status in the TLS handshake without requiring the client to contact an OCSP responder. Hard-fail revocation checking should be configured to reject connections when revocation status cannot be determined rather than allowing connections with an unknown revocation status. Organizations should ensure that their internal certificate infrastructure provides accessible OCSP and CRL endpoints so that revocation checking does not fail for internal certificates. The performance impact of revocation checking should be measured and OCSP stapling or OCSP response caching should be deployed to minimize latency impact.",
                Tags = ["tls", "certificate-revocation", "ocsp", "pki", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequireCertRevocationCheck", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireCertRevocationCheck")],
                DetectOps = [RegOp.CheckDword(Key, "RequireCertRevocationCheck", 1)],
            },
            new TweakDef
            {
                Id = "seccxn-enable-certificate-transparency-audit",
                Label = "Enable Certificate Transparency Verification for Public TLS Certificates",
                Category = "Security — Ntlm Authentication",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Certificate Transparency verification checks that TLS certificates are logged in public append-only CT logs before trusting them helping detect mis-issued certificates from compromised or rogue certificate authorities. CT logs provide public accountability for certificate issuance enabling organizations to monitor for unauthorized certificates issued for their domains. Requiring CT compliance helps detect man-in-the-middle attacks that use certificates issued by compromised CAs that might not have been publicly logged. Organizations should monitor Certificate Transparency logs for their domains to detect unauthorized certificate issuance before those certificates are used in attacks. CT verification is enforced by Chrome and other modern browsers for publicly-trusted certificates and policy can extend this requirement to all system connections. Violations of CT requirements where a certificate is trusted but not CT-logged should generate security alerts for investigation.",
                Tags = ["tls", "certificate-transparency", "pki", "man-in-the-middle", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableCertificateTransparencyAudit", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableCertificateTransparencyAudit")],
                DetectOps = [RegOp.CheckDword(Key, "EnableCertificateTransparencyAudit", 1)],
            },
            new TweakDef
            {
                Id = "seccxn-require-minimum-rsa-key-size",
                Label = "Require Minimum 2048-Bit RSA Key Size for TLS Certificate Acceptance",
                Category = "Security — Ntlm Authentication",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Minimum RSA key size enforcement prevents TLS connections from being established using certificates with RSA keys smaller than 2048 bits which are considered insufficiently secure against modern computational resources. RSA 1024-bit keys are factorable with determined effort using modern hardware and cloud computing resources and should not be trusted for security-sensitive connections. NIST recommends RSA 2048-bit as the minimum key size for long-term security and RSA 3072-bit for higher-assurance applications. Policy enforcement of key size minimums applies to all certificates in the TLS certificate chain not just the leaf certificate providing protection against weak intermediate CA certificates. Organizations should inventory their internal certificate infrastructure to identify any certificates with key sizes below 2048 bits and replace them before enforcing the minimum key size policy. The minimum key size policy should be reviewed periodically as computing capabilities increase and minimum recommendations may be raised.",
                Tags = ["tls", "rsa-key-size", "certificate-strength", "cryptographic-agility", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "MinimumRsaKeySize", 2048)],
                RemoveOps = [RegOp.DeleteValue(Key, "MinimumRsaKeySize")],
                DetectOps = [RegOp.CheckDword(Key, "MinimumRsaKeySize", 2048)],
            },
        ];
    }

    // ── SecureLaunchDrtmPolicy ──
    private static class _SecureLaunchDrtmPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard\Scenarios\SecureLaunch";
        private const string Key2 = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\TpmBootEntropy";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "sldrtm-enable-secure-launch",
                    Label = "Enable Secure Launch (DRTM) for Boot Integrity",
                    Category = "Security — Ntlm Authentication",
                    Description =
                        "Enables Secure Launch using Dynamic Root of Trust for Measurement (DRTM), which uses CPU SKINIT/SENTER instructions and TPM to establish a fresh chain of trust independent of firmware prior to OS load.",
                    Tags = ["secure-launch", "drtm", "tpm", "boot-integrity", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "DRTM-based Secure Launch active; firmware compromise isolated from OS boot chain. Requires Intel TXT or AMD SKINIT.",
                    ApplyOps = [RegOp.SetDword(Key, "Enabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "Enabled")],
                    DetectOps = [RegOp.CheckDword(Key, "Enabled", 1)],
                },
                new TweakDef
                {
                    Id = "sldrtm-require-txt-active",
                    Label = "Require Intel TXT Active for Secure Launch",
                    Category = "Security — Ntlm Authentication",
                    Description =
                        "Requires Intel Trusted Execution Technology (TXT) to be active and verified for Secure Launch to proceed, aborting boot if TXT is disabled or in error state.",
                    Tags = ["secure-launch", "intel-txt", "drtm", "tpm", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote = "Boot halted if TXT inactive; ensures DRTM chain is enforced on every boot.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireIntelTXTActive", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireIntelTXTActive")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireIntelTXTActive", 1)],
                },
                new TweakDef
                {
                    Id = "sldrtm-seal-tpm-to-secure-launch",
                    Label = "Seal TPM PCR Values to Secure Launch Measurements",
                    Category = "Security — Ntlm Authentication",
                    Description =
                        "Configures the TPM to seal key material to the PCR values produced by the DRTM Secure Launch measurement, ensuring sealed secrets are only released when the boot chain is unmodified.",
                    Tags = ["secure-launch", "tpm", "pcr", "sealed-storage", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "TPM-sealed secrets only released if boot measurements match; tampered firmware locks out DRTM-sealed data.",
                    ApplyOps = [RegOp.SetDword(Key, "SealTPMToSecureLaunch", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SealTPMToSecureLaunch")],
                    DetectOps = [RegOp.CheckDword(Key, "SealTPMToSecureLaunch", 1)],
                },
                new TweakDef
                {
                    Id = "sldrtm-enable-tpm-boot-entropy",
                    Label = "Enable TPM Boot Entropy for DRTM",
                    Category = "Security — Ntlm Authentication",
                    Description =
                        "Enables TPM-based boot entropy collection during the DRTM phase, seeding the OS CSPRNG with hardware-attested randomness from the TPM prior to key generation.",
                    Tags = ["secure-launch", "tpm", "entropy", "csprng", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "TPM boot entropy enabled; OS CSPRNG seeded with hardware RNG before cryptographic keys generated.",
                    ApplyOps = [RegOp.SetDword(Key2, "EnableBootEntropy", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "EnableBootEntropy")],
                    DetectOps = [RegOp.CheckDword(Key2, "EnableBootEntropy", 1)],
                },
                new TweakDef
                {
                    Id = "sldrtm-require-verified-acm",
                    Label = "Require Verified Authenticated Code Module for DRTM",
                    Category = "Security — Ntlm Authentication",
                    Description =
                        "Requires that the Intel TXT/AMD SKINIT Authenticated Code Module (ACM) used in DRTM is cryptographically verified before execution, preventing use of revoked or tampered ACMs.",
                    Tags = ["secure-launch", "acm", "drtm", "code-integrity", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "ACM verified before DRTM proceeds; revoked ACMs rejected, maintaining DRTM chain integrity.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireVerifiedACM", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireVerifiedACM")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireVerifiedACM", 1)],
                },
                new TweakDef
                {
                    Id = "sldrtm-block-bootkit-execution",
                    Label = "Block Bootkit Execution via DRTM Measurement",
                    Category = "Security — Ntlm Authentication",
                    Description =
                        "Enables the DRTM measurement of the MBR and VBR sectors to detect bootkits that modify the boot record, causing the boot to fail if the MBR/VBR measurements do not match the expected policy.",
                    Tags = ["secure-launch", "bootkit", "mbr", "vbr", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote = "MBR/VBR measured by DRTM; bootkit infection detected and boot halted.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockBootkitExecution", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockBootkitExecution")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockBootkitExecution", 1)],
                },
                new TweakDef
                {
                    Id = "sldrtm-enable-slat-enforcement",
                    Label = "Enforce Second Level Address Translation (SLAT) for VBS",
                    Category = "Security — Ntlm Authentication",
                    Description =
                        "Requires Second Level Address Translation (Intel EPT / AMD RVI) to be active and used by VBS before allowing the Secure Launch environment to initialise.",
                    Tags = ["secure-launch", "slat", "ept", "rvi", "vbs", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "SLAT required; without EPT/RVI the Secure Launch environment will not initialise.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireSLAT", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireSLAT")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireSLAT", 1)],
                },
                new TweakDef
                {
                    Id = "sldrtm-enable-sl-logging",
                    Label = "Enable Secure Launch Audit Logging",
                    Category = "Security — Ntlm Authentication",
                    Description =
                        "Enables event logging for all Secure Launch DRTM measurement and verification events, providing a forensic record of the boot chain measurements on each startup.",
                    Tags = ["secure-launch", "logging", "audit", "drtm", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "DRTM events logged on boot; measurements and any anomalies visible in event log.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableSecureLaunchLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableSecureLaunchLogging")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableSecureLaunchLogging", 1)],
                },
                new TweakDef
                {
                    Id = "sldrtm-block-intel-me-exploit",
                    Label = "Block Intel ME/AMT Exploit Path via TXT Lockdown",
                    Category = "Security — Ntlm Authentication",
                    Description =
                        "Configures TXT policy to lock down the Intel Management Engine (ME/AMT) memory space during the DRTM launch phase, mitigating SMM handler exploits that target ME-accessible addresses.",
                    Tags = ["secure-launch", "intel-me", "amt", "smm", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "ME/AMT memory space locked during DRTM; SMM attacks via ME mitigated.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockIntelMEExploitPath", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockIntelMEExploitPath")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockIntelMEExploitPath", 1)],
                },
                new TweakDef
                {
                    Id = "sldrtm-require-tpm-pcr17-attestation",
                    Label = "Require TPM PCR17 Attestation for Secure Launch",
                    Category = "Security — Ntlm Authentication",
                    Description =
                        "Requires TPM PCR17 to be populated by DRTM measurements and attestation-verified before Windows allows the Secure Launch environment to proceed, ensuring an unbroken hardware attestation chain.",
                    Tags = ["secure-launch", "tpm", "pcr17", "attestation", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "PCR17 attestation required; Secure Launch halts if PCR17 is not set by DRTM.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireTPMPCR17Attestation", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireTPMPCR17Attestation")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireTPMPCR17Attestation", 1)],
                },
            ];
    }
}
