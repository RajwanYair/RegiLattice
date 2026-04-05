namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PolicyWinRM
{
    public static IReadOnlyList<TweakDef> Tweaks => [.. _WinRMServicePolicy.Data, .. _WinRMClientPolicy.Data];

    // ── Sprint 670a — WinRM Service (Server Side) Hardening ───────────────────
    private static class _WinRMServicePolicy
    {
        private const string SvcKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinRM\Service";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "sec-winrm-disable-digest-auth",
                    Label = "Disable Digest Authentication on WinRM Service",
                    Category = "Security",
                    Description =
                        "Disables Digest authentication on the WinRM server. Digest auth is weak against offline dictionary and pass-the-hash attacks. Disabling forces more secure protocols (Kerberos, HTTPS + certificate). Default: Digest allowed. Recommended: disabled.",
                    Tags = ["winrm", "digest", "authentication", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ApplyOps = [RegOp.SetDword(SvcKey, "AllowDigest", 0)],
                    RemoveOps = [RegOp.DeleteValue(SvcKey, "AllowDigest")],
                    DetectOps = [RegOp.CheckDword(SvcKey, "AllowDigest", 0)],
                },
                new TweakDef
                {
                    Id = "sec-winrm-disable-auto-config",
                    Label = "Disable WinRM Service Auto-Configuration",
                    Category = "Security",
                    Description =
                        "Prevents the WinRM service from automatically configuring itself at startup. Auto-configuration can silently enable HTTP listeners and create firewall rules without explicit administrator action. Disabling requires explicit manual configuration. Default: auto-config allowed. Recommended: disabled.",
                    Tags = ["winrm", "autoconfig", "listener", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ApplyOps = [RegOp.SetDword(SvcKey, "AllowAutoConfig", 0)],
                    RemoveOps = [RegOp.DeleteValue(SvcKey, "AllowAutoConfig")],
                    DetectOps = [RegOp.CheckDword(SvcKey, "AllowAutoConfig", 0)],
                },
            ];
    }

    // ── Sprint 670b — WinRM Client Hardening ──────────────────────────────────
    private static class _WinRMClientPolicy
    {
        private const string CliKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinRM\Client";

        public static IReadOnlyList<TweakDef> Data => [];
    }
}

internal static class PolicyCredentialUI
{
    public static IReadOnlyList<TweakDef> Tweaks => [.. _CredUIPolicy.Data, .. _CredProviderPolicy.Data];

    // ── Sprint 671a — Credential UI (LogonUI) Policy ──────────────────────────
    private static class _CredUIPolicy
    {
        private const string CredUiKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CredUI";

        public static IReadOnlyList<TweakDef> Data =>
            [
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

internal static class PolicyAutoRun
{
    private const string ExplorerPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer";
    private const string ExplorerCur = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "autoplay-policy-disable-autorun",
            Label = "Disable AutoRun via Policy (All Drives)",
            Category = "Security",
            Description =
                "Sets NoDriveTypeAutoRun=0xFF under the Windows\\CurrentVersion\\Policies\\Explorer key. "
                + "Disables AutoRun for all drive types (0xFF = all bits set), including optical drives, removable drives, "
                + "network drives, and fixed drives. "
                + "Prevents malware from exploiting the autorun.inf mechanism on any media.",
            Tags = ["autorun", "autoplay", "drives", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "AutoRun disabled on all drive types; eliminates autorun.inf exploitation vector.",
            ApplyOps = [RegOp.SetDword(ExplorerCur, "NoDriveTypeAutoRun", 0xFF)],
            RemoveOps = [RegOp.DeleteValue(ExplorerCur, "NoDriveTypeAutoRun")],
            DetectOps = [RegOp.CheckDword(ExplorerCur, "NoDriveTypeAutoRun", 0xFF)],
        },
        new TweakDef
        {
            Id = "autoplay-policy-prevent-mixed-content",
            Label = "Prevent AutoPlay for Mixed-Content Drives",
            Category = "Security",
            Description =
                "Sets HonorAutorunSetting=1 under the Windows\\Explorer Group Policy key. "
                + "Instructs Windows to honour the AutoRun setting from the device itself for mixed-content discs "
                + "(CD/DVD with both data and audio tracks). "
                + "Ensures that mixed-content media does not bypass AutoRun suppression policies.",
            Tags = ["autoplay", "cd", "dvd", "mixed-content", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Mixed-content media respects per-device AutoRun flags; no automatic execution on insert.",
            ApplyOps = [RegOp.SetDword(ExplorerPol, "HonorAutorunSetting", 1)],
            RemoveOps = [RegOp.DeleteValue(ExplorerPol, "HonorAutorunSetting")],
            DetectOps = [RegOp.CheckDword(ExplorerPol, "HonorAutorunSetting", 1)],
        },
        new TweakDef
        {
            Id = "autoplay-policy-block-set-default",
            Label = "Block Users from Changing AutoPlay Default",
            Category = "Security",
            Description =
                "Sets NoAutoplayBackpropagation=1 under the Windows\\Explorer Group Policy key. "
                + "Prevents the AutoPlay dialog from remembering and persisting new user-selected defaults. "
                + "Ensures that the centrally-configured AutoPlay policy is not overridden by individual user actions.",
            Tags = ["autoplay", "policy", "enterprise", "default"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "User-selected AutoPlay defaults are not saved; policy setting always takes precedence.",
            ApplyOps = [RegOp.SetDword(ExplorerPol, "NoAutoplayBackpropagation", 1)],
            RemoveOps = [RegOp.DeleteValue(ExplorerPol, "NoAutoplayBackpropagation")],
            DetectOps = [RegOp.CheckDword(ExplorerPol, "NoAutoplayBackpropagation", 1)],
        },
        new TweakDef
        {
            Id = "autoplay-policy-disable-shell-autoplay-handlers",
            Label = "Disable Shell AutoPlay Handlers for Removable Media",
            Category = "Security",
            Description =
                "Sets DisableAutoplayForRemovableMedia=1 under the Windows\\Explorer Group Policy key. "
                + "Suppresses all shell AutoPlay handler registrations for removable media when enforced via Group Policy. "
                + "Ensures that third-party software cannot add its own AutoPlay handler entries for USB drives.",
            Tags = ["autoplay", "shell", "handlers", "removable", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Third-party AutoPlay handler registrations for removable media are blocked.",
            ApplyOps = [RegOp.SetDword(ExplorerPol, "DisableAutoplayForRemovableMedia", 1)],
            RemoveOps = [RegOp.DeleteValue(ExplorerPol, "DisableAutoplayForRemovableMedia")],
            DetectOps = [RegOp.CheckDword(ExplorerPol, "DisableAutoplayForRemovableMedia", 1)],
        },
        new TweakDef
        {
            Id = "autoplay-policy-turn-off-autoplay",
            Label = "Turn Off AutoPlay for All Media Types",
            Category = "Security",
            Description =
                "Sets DisableAutoplay=1 under the Windows\\Explorer Group Policy key — the master switch. "
                + "Completely disables the AutoPlay feature for ALL media and devices system-wide. "
                + "When this policy is applied, Windows will not process autorun.inf files or launch AutoPlay dialog handlers "
                + "regardless of device type.",
            Tags = ["autoplay", "disable", "master", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "AutoPlay completely disabled system-wide; no media or device will trigger automatic actions.",
            ApplyOps = [RegOp.SetDword(ExplorerPol, "DisableAutoplay", 1)],
            RemoveOps = [RegOp.DeleteValue(ExplorerPol, "DisableAutoplay")],
            DetectOps = [RegOp.CheckDword(ExplorerPol, "DisableAutoplay", 1)],
        },
    ];
}

internal static class PolicyLockScreen
{
    private const string PersonalKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "lockpol-policy-no-spotlight",
            Label = "Disable Windows Spotlight on Lock Screen",
            Category = "Security",
            Description =
                "Sets NoWindowsSpotlightOnLockScreen=1 under the Personalization Group Policy key. "
                + "Prevents Windows Spotlight from downloading and displaying rotating background images, "
                + "ads, and tips on the lock screen. "
                + "Eliminates background network traffic and removes Microsoft advertising from the pre-auth surface.",
            Tags = ["lockscreen", "spotlight", "policy", "privacy", "ads"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Windows Spotlight disabled on lock screen; no rotating images or advertising content.",
            ApplyOps = [RegOp.SetDword(PersonalKey, "NoWindowsSpotlightOnLockScreen", 1)],
            RemoveOps = [RegOp.DeleteValue(PersonalKey, "NoWindowsSpotlightOnLockScreen")],
            DetectOps = [RegOp.CheckDword(PersonalKey, "NoWindowsSpotlightOnLockScreen", 1)],
        },
        new TweakDef
        {
            Id = "lockpol-policy-no-spotlight-action-center",
            Label = "Disable Windows Spotlight in Action Centre",
            Category = "Security",
            Description =
                "Sets NoWindowsSpotlightOnActionCenter=1 under the Personalization Group Policy key. "
                + "Prevents Windows Spotlight suggestions and featured apps from appearing in the Action Centre panel. "
                + "Removes promotional content from the notification/action area.",
            Tags = ["lockscreen", "spotlight", "action-center", "policy", "privacy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Spotlight suggestions removed from Action Centre; notification area shows only real notifications.",
            ApplyOps = [RegOp.SetDword(PersonalKey, "NoWindowsSpotlightOnActionCenter", 1)],
            RemoveOps = [RegOp.DeleteValue(PersonalKey, "NoWindowsSpotlightOnActionCenter")],
            DetectOps = [RegOp.CheckDword(PersonalKey, "NoWindowsSpotlightOnActionCenter", 1)],
        },
        new TweakDef
        {
            Id = "lockpol-policy-no-third-party-suggestions",
            Label = "Disable Third-Party App Suggestions on Lock Screen",
            Category = "Security",
            Description =
                "Sets NoThirdPartySuggestions=1 under the Personalization Group Policy key. "
                + "Prevents Windows from displaying app suggestions from third-party publishers on the lock screen. "
                + "Eliminates advertising and unsolicited install prompts from the sign-in surface.",
            Tags = ["lockscreen", "suggestions", "ads", "third-party", "policy", "privacy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Third-party app suggestions removed from lock screen; clean pre-auth experience.",
            ApplyOps = [RegOp.SetDword(PersonalKey, "NoThirdPartySuggestions", 1)],
            RemoveOps = [RegOp.DeleteValue(PersonalKey, "NoThirdPartySuggestions")],
            DetectOps = [RegOp.CheckDword(PersonalKey, "NoThirdPartySuggestions", 1)],
        },
        new TweakDef
        {
            Id = "lockpol-policy-no-spotlight-windows-welcome",
            Label = "Disable Windows Welcome Experience Spotlight",
            Category = "Security",
            Description =
                "Sets NoWindowsSpotlightWindowsWelcomeExperience=1 under the Personalization Group Policy key. "
                + "Prevents the 'What's new in Windows' welcome experience from appearing after feature updates. "
                + "Stops an animated fullscreen takeover that introduces new features at the expense of user focus.",
            Tags = ["lockscreen", "spotlight", "welcome", "policy", "onboarding"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Post-update Windows Welcome Experience spotlight is suppressed.",
            ApplyOps = [RegOp.SetDword(PersonalKey, "NoWindowsSpotlightWindowsWelcomeExperience", 1)],
            RemoveOps = [RegOp.DeleteValue(PersonalKey, "NoWindowsSpotlightWindowsWelcomeExperience")],
            DetectOps = [RegOp.CheckDword(PersonalKey, "NoWindowsSpotlightWindowsWelcomeExperience", 1)],
        },
        new TweakDef
        {
            Id = "lockpol-policy-lock-screen-camera",
            Label = "Disable Camera Access from Lock Screen",
            Category = "Security",
            Description =
                "Sets NoCameraOnLockScreen=1 under the Personalization Group Policy key. "
                + "Prevents the Camera app from launching directly from the lock screen. "
                + "Eliminates a potential avenue for taking photos or accessing media without authenticating first.",
            Tags = ["lockscreen", "camera", "policy", "security", "access"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Camera cannot be launched from lock screen; full authentication required to access camera.",
            ApplyOps = [RegOp.SetDword(PersonalKey, "NoCameraOnLockScreen", 1)],
            RemoveOps = [RegOp.DeleteValue(PersonalKey, "NoCameraOnLockScreen")],
            DetectOps = [RegOp.CheckDword(PersonalKey, "NoCameraOnLockScreen", 1)],
        },
        new TweakDef
        {
            Id = "lockpol-policy-no-spotlight-features",
            Label = "Turn Off All Spotlight Features",
            Category = "Security",
            Description =
                "Sets ConfigureWindowsSpotlight=2 under the Personalization Group Policy key. "
                + "Value 2 applies the most restrictive Spotlight policy: disabled entirely. "
                + "This replaces all per-feature Spotlight toggles with a single master-off switch for Group Policy compliance.",
            Tags = ["lockscreen", "spotlight", "master", "policy", "privacy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "All Windows Spotlight feature categories disabled via a single master policy value.",
            ApplyOps = [RegOp.SetDword(PersonalKey, "ConfigureWindowsSpotlight", 2)],
            RemoveOps = [RegOp.DeleteValue(PersonalKey, "ConfigureWindowsSpotlight")],
            DetectOps = [RegOp.CheckDword(PersonalKey, "ConfigureWindowsSpotlight", 2)],
        },
        new TweakDef
        {
            Id = "lockpol-policy-no-spotlight-taskbar",
            Label = "Disable Spotlight Suggestions in Taskbar Search",
            Category = "Security",
            Description =
                "Sets NoWindowsSpotlightInSearch=1 under the Personalization Group Policy key. "
                + "Removes Microsoft-curated Spotlight content suggestions from appearing in the Windows Search bar on the taskbar. "
                + "Search results show only local and Bing content, not Spotlight-injected promotions.",
            Tags = ["lockscreen", "spotlight", "search", "taskbar", "policy", "privacy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Spotlight suggestions removed from taskbar Search; search shows only query results.",
            ApplyOps = [RegOp.SetDword(PersonalKey, "NoWindowsSpotlightInSearch", 1)],
            RemoveOps = [RegOp.DeleteValue(PersonalKey, "NoWindowsSpotlightInSearch")],
            DetectOps = [RegOp.CheckDword(PersonalKey, "NoWindowsSpotlightInSearch", 1)],
        },
        new TweakDef
        {
            Id = "lockpol-policy-no-spotlight-settings",
            Label = "Disable Spotlight Tips in Settings",
            Category = "Security",
            Description =
                "Sets NoWindowsSpotlightInSettings=1 under the Personalization Group Policy key. "
                + "Removes the Windows Spotlight-powered tips and feature highlights that appear throughout the Settings app. "
                + "Reduces noise from Microsoft suggestions inside Settings pages.",
            Tags = ["lockscreen", "spotlight", "settings", "tips", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Spotlight tips and suggestions removed from all Settings pages.",
            ApplyOps = [RegOp.SetDword(PersonalKey, "NoWindowsSpotlightInSettings", 1)],
            RemoveOps = [RegOp.DeleteValue(PersonalKey, "NoWindowsSpotlightInSettings")],
            DetectOps = [RegOp.CheckDword(PersonalKey, "NoWindowsSpotlightInSettings", 1)],
        },
    ];
}

internal static class PolicySmartCard
{
    private const string ScCredProv = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SmartCardCredentialProvider";
    private const string WinSystem = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "smartcard-policy-force-logoff-on-removal",
            Label = "Force Logoff on Smart Card Removal",
            Category = "Security",
            Description =
                "Sets scremoveoption=2 under the SmartCardCredentialProvider Group Policy key. "
                + "Value 2 causes a full sign-out (rather than a lock) when the smart card is removed. "
                + "More aggressive than the lock option — suitable for high-security shared terminal environments.",
            Tags = ["smartcard", "logoff", "removal", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "User is immediately logged off when card is removed; unsaved work may be lost.",
            ApplyOps = [RegOp.SetDword(ScCredProv, "scremoveoption", 2)],
            RemoveOps = [RegOp.DeleteValue(ScCredProv, "scremoveoption")],
            DetectOps = [RegOp.CheckDword(ScCredProv, "scremoveoption", 2)],
        },
        new TweakDef
        {
            Id = "smartcard-policy-allow-integrated-unblock",
            Label = "Allow Integrated Unblock Screen for Smart Card PIN",
            Category = "Security",
            Description =
                "Sets AllowIntegratedUnblock=1 under the SmartCardCredentialProvider Group Policy key. "
                + "Enables a built-in PIN unlock screen that appears on the credential provider for blocked smart cards, "
                + "removing the need for a separate help-desk intervention to reset a locked card.",
            Tags = ["smartcard", "pin", "unblock", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Users can self-service unblock a PIN-locked smart card via the Windows credential screen.",
            ApplyOps = [RegOp.SetDword(ScCredProv, "AllowIntegratedUnblock", 1)],
            RemoveOps = [RegOp.DeleteValue(ScCredProv, "AllowIntegratedUnblock")],
            DetectOps = [RegOp.CheckDword(ScCredProv, "AllowIntegratedUnblock", 1)],
        },
        new TweakDef
        {
            Id = "smartcard-policy-turn-on-virtual-card",
            Label = "Enable Virtual Smart Card Creation",
            Category = "Security",
            Description =
                "Sets AllowDomainPINLogon=1 under the SmartCardCredentialProvider Group Policy key. "
                + "Allows BitLocker Network Unlock and domain accounts to authenticate with a PIN against a virtual TPM smart card. "
                + "Enables software-based smart card equivalent for devices without physical card readers.",
            Tags = ["smartcard", "virtual", "tpm", "pin", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Virtual smart card PIN logon enabled; TPM-backed credential usable without a physical card reader.",
            ApplyOps = [RegOp.SetDword(ScCredProv, "AllowDomainPINLogon", 1)],
            RemoveOps = [RegOp.DeleteValue(ScCredProv, "AllowDomainPINLogon")],
            DetectOps = [RegOp.CheckDword(ScCredProv, "AllowDomainPINLogon", 1)],
        },
        new TweakDef
        {
            Id = "smartcard-policy-enable-certificate-propagation",
            Label = "Enable Smart Card Certificate Propagation",
            Category = "Security",
            Description =
                "Sets CertPropEnabled=1 under the Windows\\System Group Policy key. "
                + "Enables automatic propagation of smart card certificates from the card to the user's certificate store "
                + "when the card is inserted. "
                + "Required for applications that directly query the user certificate store rather than the card reader.",
            Tags = ["smartcard", "certificate", "propagation", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Smart card certificates automatically appear in the user certificate store on card insert.",
            ApplyOps = [RegOp.SetDword(WinSystem, "CertPropEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(WinSystem, "CertPropEnabled")],
            DetectOps = [RegOp.CheckDword(WinSystem, "CertPropEnabled", 1)],
        },
        new TweakDef
        {
            Id = "smartcard-policy-enable-cleanup-certificates",
            Label = "Clean Up Smart Card Certificates on Card Removal",
            Category = "Security",
            Description =
                "Sets CleanupCerts=1 under the Windows\\System Group Policy key. "
                + "Removes smart card certificates from the user store when the card is removed. "
                + "Works in conjunction with CertPropEnabled to maintain a consistent certificate state "
                + "reflecting only currently-inserted cards.",
            Tags = ["smartcard", "certificate", "cleanup", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Propagated certificates removed from user store when smart card is removed.",
            ApplyOps = [RegOp.SetDword(WinSystem, "CleanupCerts", 1)],
            RemoveOps = [RegOp.DeleteValue(WinSystem, "CleanupCerts")],
            DetectOps = [RegOp.CheckDword(WinSystem, "CleanupCerts", 1)],
        },
        new TweakDef
        {
            Id = "smartcard-policy-turn-off-root-auto-update",
            Label = "Prevent Smart Card Root Certificate Auto-Update",
            Category = "Security",
            Description =
                "Sets RootCertificateAutoUpdate=0 under the SmartCardCredentialProvider key. "
                + "Prevents Windows from automatically downloading and updating root certificates from Windows Update "
                + "for smart card trust anchors. "
                + "Appropriate in air-gapped or strictly controlled environments where certificate trust is managed manually.",
            Tags = ["smartcard", "certificate", "root", "update", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            ImpactNote = "Root certificate auto-update for smart cards disabled; manual CA trust management required.",
            ApplyOps = [RegOp.SetDword(ScCredProv, "RootCertificateAutoUpdate", 0)],
            RemoveOps = [RegOp.DeleteValue(ScCredProv, "RootCertificateAutoUpdate")],
            DetectOps = [RegOp.CheckDword(ScCredProv, "RootCertificateAutoUpdate", 0)],
        },
        new TweakDef
        {
            Id = "smartcard-policy-disable-pinpad-logon",
            Label = "Disable PIN Pad Bypass for Smart Card Logon",
            Category = "Security",
            Description =
                "Sets DisallowPINLessLogon=1 under the SmartCardCredentialProvider Group Policy key. "
                + "Requires a PIN to be entered for every smart card logon, even if the card supports and is configured for PINless logon. "
                + "Ensures that PIN knowledge (something-you-know) combined with card possession (something-you-have) is always required.",
            Tags = ["smartcard", "pin", "logon", "policy", "mfa", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "PINless logon blocked; card + PIN is always required for true two-factor authentication.",
            ApplyOps = [RegOp.SetDword(ScCredProv, "DisallowPINLessLogon", 1)],
            RemoveOps = [RegOp.DeleteValue(ScCredProv, "DisallowPINLessLogon")],
            DetectOps = [RegOp.CheckDword(ScCredProv, "DisallowPINLessLogon", 1)],
        },
    ];
}

internal static class PolicyFido
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FIDO";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "fido-disable-security-key-signin",
            Label = "Block FIDO2 Security Key Sign-In",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks FIDO2 hardware security key logon via Group Policy. Useful for environments where all authentication must go through managed identity providers.",
            Tags = ["fido", "security-key", "block", "policy", "authentication"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "FIDO2 hardware key sign-in blocked; use only when alternative MFA is mandated.",
            ApplyOps = [RegOp.SetDword(Key, "EnableFIDODeviceLogon", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableFIDODeviceLogon")],
            DetectOps = [RegOp.CheckDword(Key, "EnableFIDODeviceLogon", 0)],
        },
        new TweakDef
        {
            Id = "fido-require-user-presence",
            Label = "Require User Presence for FIDO Authentication",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enforces that FIDO2 authentication always requires a physical user-presence test (button touch). Prevents silent automated authentication without user interaction.",
            Tags = ["fido", "user-presence", "policy", "authentication", "security"],
            RegistryKeys = [Key],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "FIDO key must be physically touched; silent automated use blocked.",
            ApplyOps = [RegOp.SetDword(Key, "EnablePresenceRequired", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnablePresenceRequired")],
            DetectOps = [RegOp.CheckDword(Key, "EnablePresenceRequired", 1)],
        },
        new TweakDef
        {
            Id = "fido-disable-nfc-security-keys",
            Label = "Block NFC FIDO2 Security Keys",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Blocks NFC-based FIDO2 security keys via Group Policy. Use when only USB FIDO keys are approved for your environment.",
            Tags = ["fido", "nfc", "block", "policy", "authentication"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "NFC FIDO2 keys blocked; only USB keys permitted for logon.",
            ApplyOps = [RegOp.SetDword(Key, "EnableNFCFIDODevices", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableNFCFIDODevices")],
            DetectOps = [RegOp.CheckDword(Key, "EnableNFCFIDODevices", 0)],
        },
        new TweakDef
        {
            Id = "fido-disable-ble-security-keys",
            Label = "Block Bluetooth FIDO2 Security Keys",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks Bluetooth LE FIDO2 security keys via Group Policy. Eliminates wireless hardware key sign-in attack surface in high-security environments.",
            Tags = ["fido", "bluetooth", "ble", "block", "policy", "authentication"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "BLE FIDO2 keys blocked; only wired/NFC keys allowed.",
            ApplyOps = [RegOp.SetDword(Key, "EnableBLEFIDODevices", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableBLEFIDODevices")],
            DetectOps = [RegOp.CheckDword(Key, "EnableBLEFIDODevices", 0)],
        },
        new TweakDef
        {
            Id = "fido-require-attestation",
            Label = "Require FIDO2 Key Attestation",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Requires attestation from FIDO2 security keys during registration. Ensures only manufacturer-verified hardware can be enrolled, blocking counterfeit or unapproved keys.",
            Tags = ["fido", "attestation", "policy", "authentication", "hardware", "security"],
            RegistryKeys = [Key],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Only attested FIDO2 keys with verified manufacturing credentials can be registered.",
            ApplyOps = [RegOp.SetDword(Key, "RequireAttestation", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireAttestation")],
            DetectOps = [RegOp.CheckDword(Key, "RequireAttestation", 1)],
        },
        new TweakDef
        {
            Id = "fido-enable-enterprise-attestation",
            Label = "Enable FIDO2 Enterprise Attestation",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables enterprise attestation for FIDO2 security keys. Allows the relying party to receive full manufacturer attestation data for auditing and key inventory management.",
            Tags = ["fido", "enterprise", "attestation", "policy", "authentication", "audit"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Full attestation data available to relying party; key inventory auditable.",
            ApplyOps = [RegOp.SetDword(Key, "EnableEnterpriseAttestation", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableEnterpriseAttestation")],
            DetectOps = [RegOp.CheckDword(Key, "EnableEnterpriseAttestation", 1)],
        },
        new TweakDef
        {
            Id = "fido-disable-roaming-credentials",
            Label = "Disable FIDO2 Roaming Credentials",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents FIDO2 credentials from roaming between devices via Windows credential manager sync. Keeps authentication credentials strictly bound to the enrolled hardware key and device pair.",
            Tags = ["fido", "roaming", "credentials", "policy", "authentication", "isolation"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "FIDO2 credentials isolated to the device; no cross-device sync.",
            ApplyOps = [RegOp.SetDword(Key, "DisableRoamingCredentials", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableRoamingCredentials")],
            DetectOps = [RegOp.CheckDword(Key, "DisableRoamingCredentials", 1)],
        },
    ];
}

internal static class PolicyWindowsHello
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PassportForWork";
    private const string PinKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PassportForWork\PINComplexity";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "whi-set-pin-expiry-90-days",
            Label = "Set Windows Hello PIN Expiry to 90 Days",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Configures Windows Hello PINs to expire after 90 days, prompting users to create a new PIN. Aligns with common password-rotation compliance requirements.",
            Tags = ["windows-hello", "pin", "expiry", "rotation", "policy", "compliance"],
            RegistryKeys = [PinKey],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Hello PIN expires after 90 days; periodic rotation enforced.",
            ApplyOps = [RegOp.SetDword(PinKey, "PINExpirationPeriod", 90)],
            RemoveOps = [RegOp.DeleteValue(PinKey, "PINExpirationPeriod")],
            DetectOps = [RegOp.CheckDword(PinKey, "PINExpirationPeriod", 90)],
        },
        new TweakDef
        {
            Id = "whi-set-pin-history-5",
            Label = "Enforce Windows Hello PIN History (5 Previous)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents reuse of the last 5 Windows Hello PINs. Combined with expiry, prevents users from cycling through a small set of known PINs.",
            Tags = ["windows-hello", "pin", "history", "reuse", "policy", "compliance"],
            RegistryKeys = [PinKey],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Last 5 Hello PINs remembered; immediate re-use blocked.",
            ApplyOps = [RegOp.SetDword(PinKey, "PINHistory", 5)],
            RemoveOps = [RegOp.DeleteValue(PinKey, "PINHistory")],
            DetectOps = [RegOp.CheckDword(PinKey, "PINHistory", 5)],
        },
    ];
}

internal static class PolicyEntraId
{
    private const string JoinKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WorkplaceJoin";
    private const string MsaKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MicrosoftAccount";
    private const string CloudKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent";
    private const string SysKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "entra-block-microsoft-accounts",
            Label = "Block Microsoft Consumer Accounts",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks Microsoft consumer account (MSA) sign-in and authentication via Group Policy. Forces all account activity through managed work/school accounts only — no personal microsoft.com accounts.",
            Tags = ["entra", "microsoft-account", "msa", "block", "policy", "corporate"],
            RegistryKeys = [MsaKey],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Personal Microsoft accounts blocked; only work/school accounts allowed.",
            ApplyOps = [RegOp.SetDword(MsaKey, "DisableUserAuth", 1)],
            RemoveOps = [RegOp.DeleteValue(MsaKey, "DisableUserAuth")],
            DetectOps = [RegOp.CheckDword(MsaKey, "DisableUserAuth", 1)],
        },
        new TweakDef
        {
            Id = "entra-disable-device-enrollment-user",
            Label = "Prevent User-Initiated MDM Enrollment",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents end users from initiating MDM (Intune/Azure AD) device enrollment from the Settings app. Enrollment must be performed by IT through approved provisioning flows.",
            Tags = ["entra", "mdm", "enrollment", "intune", "policy", "corporate"],
            RegistryKeys = [SysKey],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "User-initiated MDM enrollment blocked; IT-managed provisioning only.",
            ApplyOps = [RegOp.SetDword(SysKey, "DisableMDMEnrollment", 1)],
            RemoveOps = [RegOp.DeleteValue(SysKey, "DisableMDMEnrollment")],
            DetectOps = [RegOp.CheckDword(SysKey, "DisableMDMEnrollment", 1)],
        },
        new TweakDef
        {
            Id = "entra-disable-adding-work-accounts",
            Label = "Block Adding Work or School Accounts",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks users from adding additional work or school accounts via Settings > Accounts > Access work or school. Ensures devices have exactly one managed identity, preventing shadow account issues.",
            Tags = ["entra", "work-account", "school-account", "policy", "corporate", "identity"],
            RegistryKeys = [SysKey],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Adding secondary work/school accounts blocked from user Settings.",
            ApplyOps = [RegOp.SetDword(SysKey, "AllowWorkplaceJoinViaSetup", 0)],
            RemoveOps = [RegOp.DeleteValue(SysKey, "AllowWorkplaceJoinViaSetup")],
            DetectOps = [RegOp.CheckDword(SysKey, "AllowWorkplaceJoinViaSetup", 0)],
        },
        new TweakDef
        {
            Id = "entra-disable-find-my-device",
            Label = "Disable Find My Device (Entra Policy)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Find My Device feature on Entra-joined corporate devices via Group Policy. Prevents the device location from being reported to Microsoft / the user's Microsoft account, appropriate for managed shared devices.",
            Tags = ["entra", "find-my-device", "location", "policy", "privacy", "corporate"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FindMyDevice"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Find My Device disabled; device location not reported to Microsoft.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FindMyDevice", "FindMyDeviceEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FindMyDevice", "FindMyDeviceEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FindMyDevice", "FindMyDeviceEnabled", 0)],
        },
        new TweakDef
        {
            Id = "entra-prevent-privacy-settings-prompt",
            Label = "Suppress Privacy Settings Prompt at Entra Sign-In",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents the privacy settings diagnostic prompt that appears for new users at first sign-in on Entra-joined devices. IT policies govern data settings; users should not be prompted to override them.",
            Tags = ["entra", "privacy", "oobe", "policy", "first-run", "prompt"],
            RegistryKeys = [SysKey],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Privacy settings prompt suppressed at first logon; corporate policy governs settings.",
            ApplyOps = [RegOp.SetDword(SysKey, "PreventUserPrivacySettingPrompt", 1)],
            RemoveOps = [RegOp.DeleteValue(SysKey, "PreventUserPrivacySettingPrompt")],
            DetectOps = [RegOp.CheckDword(SysKey, "PreventUserPrivacySettingPrompt", 1)],
        },
        new TweakDef
        {
            Id = "entra-block-setup-next-steps",
            Label = "Block Entra Sign-In Setup Next-Steps Prompt",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks the 'What do you want to do next?' setup prompt shown after users complete the initial Entra ID sign-in flow. Streamlines the managed first-logon experience in corporate environments.",
            Tags = ["entra", "setup", "oobe", "first-run", "policy", "corporate"],
            RegistryKeys = [SysKey],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Post-sign-in setup prompt suppressed; users go directly to desktop.",
            ApplyOps = [RegOp.SetDword(SysKey, "BlockNextSteps", 1)],
            RemoveOps = [RegOp.DeleteValue(SysKey, "BlockNextSteps")],
            DetectOps = [RegOp.CheckDword(SysKey, "BlockNextSteps", 1)],
        },
        new TweakDef
        {
            Id = "entra-block-phone-link",
            Label = "Block Phone Link App on Entra-Managed Devices",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Phone Link (Your Phone) application on Entra-joined managed devices via Group Policy. Prevents personal mobile device data — messages, notifications, photos, calls — from being accessible on corporate PCs.",
            Tags = ["entra", "phone-link", "your-phone", "policy", "data-protection", "corporate"],
            RegistryKeys = [SysKey],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Phone Link blocked; personal phone data cannot be accessed from corporate PC.",
            ApplyOps = [RegOp.SetDword(SysKey, "AllowPhoneLink", 0)],
            RemoveOps = [RegOp.DeleteValue(SysKey, "AllowPhoneLink")],
            DetectOps = [RegOp.CheckDword(SysKey, "AllowPhoneLink", 0)],
        },
    ];
}

internal static class PolicyKerberos
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\Kerberos\Parameters";
    private const string LsaKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\Kerberos\Parameters";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "kerberos-enable-claims",
            Label = "Enable Kerberos Claims Authentication",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables Kerberos claims-based access control (Dynamic Access Control). Allows Kerberos tickets to carry user and device claim attributes used for resource-based authorisation decisions.",
            Tags = ["kerberos", "claims", "dac", "dynamic-access-control", "policy", "authentication"],
            RegistryKeys = [LsaKey],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Kerberos tickets carry claims; Dynamic Access Control policies can be applied.",
            ApplyOps = [RegOp.SetDword(LsaKey, "EnableCbacAndArmor", 1)],
            RemoveOps = [RegOp.DeleteValue(LsaKey, "EnableCbacAndArmor")],
            DetectOps = [RegOp.CheckDword(LsaKey, "EnableCbacAndArmor", 1)],
        },
        new TweakDef
        {
            Id = "kerberos-enable-resource-sid-compression",
            Label = "Enable Kerberos Resource SID Compression",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables Kerberos SID compression for resource group SIDs in service tickets. Reduces Kerberos ticket size when users belong to many groups, preventing ticket-too-large (KDC_ERR_RESPONSE_TOO_BIG) failures.",
            Tags = ["kerberos", "sid", "compression", "policy", "authentication", "performance"],
            RegistryKeys = [LsaKey],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "SID compression enabled; Kerberos tickets smaller for users with many group memberships.",
            ApplyOps = [RegOp.SetDword(LsaKey, "KdcUseRequestedEtypesForTickets", 1)],
            RemoveOps = [RegOp.DeleteValue(LsaKey, "KdcUseRequestedEtypesForTickets")],
            DetectOps = [RegOp.CheckDword(LsaKey, "KdcUseRequestedEtypesForTickets", 1)],
        },
        new TweakDef
        {
            Id = "kerberos-enable-kdc-proxy",
            Label = "Enable Kerberos KDC Proxy",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables the Kerberos KDC Proxy (KKDCP) for clients that cannot reach the domain controller directly. Authentication is tunnelled over HTTPS to the proxy, supporting remote workers without VPN.",
            Tags = ["kerberos", "kdc-proxy", "kkdcp", "remote", "policy", "vpn-alternative"],
            RegistryKeys = [LsaKey],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "KDC proxy enabled; Kerberos auth works over HTTPS for remoting without full VPN.",
            ApplyOps = [RegOp.SetDword(LsaKey, "UseKdcProxy", 1)],
            RemoveOps = [RegOp.DeleteValue(LsaKey, "UseKdcProxy")],
            DetectOps = [RegOp.CheckDword(LsaKey, "UseKdcProxy", 1)],
        },
    ];
}

internal static class PolicyAppInstaller
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "appinst-disable-local-archive-install",
            Label = "Block App Installer Local Archive Installation",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks WinGet from installing applications from local archive files (ZIP, tar.gz). Prevents circumventing approved sources by extracting and installing from locally downloaded archives.",
            Tags = ["app-installer", "winget", "archive", "zip", "policy", "sideloading"],
            RegistryKeys = [Key],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Local archive installs blocked; ZIP/tar package sideloading prevented.",
            ApplyOps = [RegOp.SetDword(Key, "EnableLocalArchiveInstall", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableLocalArchiveInstall")],
            DetectOps = [RegOp.CheckDword(Key, "EnableLocalArchiveInstall", 0)],
        },
        new TweakDef
        {
            Id = "appinst-set-source-update-interval-1h",
            Label = "Set App Installer Source Update Interval to 1 Hour",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the WinGet package source auto-update interval to 1 hour. More frequent source refreshes ensure the package catalogue is always current, reducing the window where a compromised package version could be served.",
            Tags = ["app-installer", "winget", "source", "update", "interval", "policy"],
            RegistryKeys = [Key],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Source index refreshed hourly; stale package catalogue window minimised.",
            ApplyOps = [RegOp.SetDword(Key, "SourceAutoUpdateInterval", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "SourceAutoUpdateInterval")],
            DetectOps = [RegOp.CheckDword(Key, "SourceAutoUpdateInterval", 1)],
        },
        new TweakDef
        {
            Id = "appinst-enable-bypass-cert-pinning",
            Label = "Enable Certificate Pin Bypass for Microsoft Store",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Allows App Installer to bypass certificate pinning when connecting to the Microsoft Store source. Required in corporate environments that use TLS-intercepting proxies; without this, MSIX downloads may fail.",
            Tags = ["app-installer", "winget", "certificate", "pinning", "proxy", "corporate", "policy"],
            RegistryKeys = [Key],
            ImpactScore = 2,
            SafetyRating = 3,
            ImpactNote = "Certificate pinning bypassed for Store; needed behind TLS-inspection proxies.",
            ApplyOps = [RegOp.SetDword(Key, "EnableBypassCertificatePinningForMicrosoftStore", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableBypassCertificatePinningForMicrosoftStore")],
            DetectOps = [RegOp.CheckDword(Key, "EnableBypassCertificatePinningForMicrosoftStore", 1)],
        },
    ];
}

internal static class PolicyNetworkIsolation
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkIsolation";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "netiso-block-domain-enterprise-exceptions",
            Label = "Block AppContainer Domain Enterprise Exception Bypass",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents Windows Store apps running in AppContainer from adding themselves to the Enterprise authentication exception list. Stops apps from bypassing network isolation to access domain resources.",
            Tags = ["network", "isolation", "appcontainer", "security", "policy", "uwp"],
            RegistryKeys = [Key],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "AppContainer apps cannot self-add to domain exception list; network isolation enforced.",
            ApplyOps = [RegOp.SetDword(Key, "BlockDomainEnterprise", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockDomainEnterprise")],
            DetectOps = [RegOp.CheckDword(Key, "BlockDomainEnterprise", 1)],
        },
        new TweakDef
        {
            Id = "netiso-disable-appcontainer-loopback",
            Label = "Disable AppContainer Loopback Exemption",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Removes the loopback exemption for Windows Store apps (AppContainer), forcing them to go through the network stack for all traffic. By default, AppContainer apps can bypass the loopback for localhost communications; removing this closes the bypass.",
            Tags = ["network", "isolation", "appcontainer", "loopback", "security", "policy"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "AppContainer loopback bypass removed; all app traffic goes through network isolation.",
            ApplyOps = [RegOp.SetDword(Key, "DisableLoopback", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableLoopback")],
            DetectOps = [RegOp.CheckDword(Key, "DisableLoopback", 1)],
        },
        new TweakDef
        {
            Id = "netiso-require-package-authentication",
            Label = "Require Package Family Authentication for Network Isolation",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Requires that applications declare their Package Family Name (PFN) to access enterprise resources beyond the AppContainer boundary. Strengthens network isolation capability attestation.",
            Tags = ["network", "isolation", "authentication", "pfn", "security", "policy"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Apps must declare PFN for enterprise network access; undeclared apps blocked.",
            ApplyOps = [RegOp.SetDword(Key, "RequirePackageAuthentication", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequirePackageAuthentication")],
            DetectOps = [RegOp.CheckDword(Key, "RequirePackageAuthentication", 1)],
        },
        new TweakDef
        {
            Id = "netiso-disable-intranet-lookup",
            Label = "Disable Automatic Intranet Address Classification",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Stops Windows from automatically classifying internal IP ranges as 'intranet' for network isolation purposes. Prevents Store apps from accidentally gaining enterprise network access based on auto-detected IP ranges.",
            Tags = ["network", "isolation", "intranet", "classification", "security", "policy"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Auto-intranet detection disabled; only explicitly defined ranges treated as intranet.",
            ApplyOps = [RegOp.SetDword(Key, "DisableIntranetLookup", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableIntranetLookup")],
            DetectOps = [RegOp.CheckDword(Key, "DisableIntranetLookup", 1)],
        },
        new TweakDef
        {
            Id = "netiso-block-proxy-browsing",
            Label = "Block AppContainer Internet Proxy Access",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks Windows Store apps from using the system proxy for internet access. AppContainer-isolated apps are restricted from routing traffic through corporate proxies, preventing proxy-bypass attacks.",
            Tags = ["network", "isolation", "proxy", "appcontainer", "security", "policy"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Store apps cannot route through system proxy; direct proxy access from AppContainer blocked.",
            ApplyOps = [RegOp.SetDword(Key, "BlockProxyBrowsing", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockProxyBrowsing")],
            DetectOps = [RegOp.CheckDword(Key, "BlockProxyBrowsing", 1)],
        },
        new TweakDef
        {
            Id = "netiso-disable-network-capability-autodeny",
            Label = "Deny AppContainer Network Capability by Default",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the default policy for network capability declarations by AppContainer apps to deny. Apps must be explicitly whitelisted to access network resources rather than being allowed by default.",
            Tags = ["network", "isolation", "appcontainer", "capability", "deny-default", "policy"],
            RegistryKeys = [Key],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Default-deny network capability for Store apps; explicit allow-list required.",
            ApplyOps = [RegOp.SetDword(Key, "DenyNetworkByDefault", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DenyNetworkByDefault")],
            DetectOps = [RegOp.CheckDword(Key, "DenyNetworkByDefault", 1)],
        },
        new TweakDef
        {
            Id = "netiso-require-private-network-declaration",
            Label = "Require Explicit Private Network Capability Declaration",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Requires that AppContainer apps explicitly declare the 'privateNetworkClientServer' capability in their manifest to access local network resources. Prevents undeclared apps from silently accessing home/enterprise network devices.",
            Tags = ["network", "isolation", "private-network", "capability", "security", "policy"],
            RegistryKeys = [Key],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Private network access requires manifest declaration; undeclared apps blocked from LAN.",
            ApplyOps = [RegOp.SetDword(Key, "RequirePrivateNetworkDeclaration", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequirePrivateNetworkDeclaration")],
            DetectOps = [RegOp.CheckDword(Key, "RequirePrivateNetworkDeclaration", 1)],
        },
        new TweakDef
        {
            Id = "netiso-block-internet-appcontainer",
            Label = "Block AppContainer Direct Internet Access",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks AppContainer-sandboxed apps from making direct internet connections. All internet traffic from Store apps must go through a declared network capability and optionally a firewall policy.",
            Tags = ["network", "isolation", "internet", "appcontainer", "firewall", "policy"],
            RegistryKeys = [Key],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Direct internet connections from AppContainer apps blocked; capability declaration required.",
            ApplyOps = [RegOp.SetDword(Key, "BlockInternetAppContainer", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockInternetAppContainer")],
            DetectOps = [RegOp.CheckDword(Key, "BlockInternetAppContainer", 1)],
        },
        new TweakDef
        {
            Id = "netiso-disable-isolation-debug",
            Label = "Disable Network Isolation Debug Mode",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents enabling the Network Isolation debug/diagnostic mode that bypasses AppContainer restrictions for troubleshooting. Debug mode can be used to permanently relax isolation on developer machines.",
            Tags = ["network", "isolation", "debug", "security", "policy"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Debug bypass of network isolation disabled; isolation policies cannot be bypassed for diagnosis.",
            ApplyOps = [RegOp.SetDword(Key, "DisableIsolationDebug", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableIsolationDebug")],
            DetectOps = [RegOp.CheckDword(Key, "DisableIsolationDebug", 1)],
        },
        new TweakDef
        {
            Id = "netiso-enable-strict-capability-enforcement",
            Label = "Enforce Strict AppContainer Network Capability Checking",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables strict mode for AppContainer network capability enforcement. In strict mode, any capability not explicitly declared in the app manifest is denied without fallback, tightening the network isolation boundary.",
            Tags = ["network", "isolation", "strict", "capability", "appcontainer", "policy"],
            RegistryKeys = [Key],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Strict capability enforcement active; any undeclared network access denied.",
            ApplyOps = [RegOp.SetDword(Key, "StrictCapabilityEnforcement", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "StrictCapabilityEnforcement")],
            DetectOps = [RegOp.CheckDword(Key, "StrictCapabilityEnforcement", 1)],
        },
    ];
}
