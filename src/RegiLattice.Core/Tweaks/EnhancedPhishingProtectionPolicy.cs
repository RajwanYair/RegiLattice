namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class EnhancedPhishingProtectionPolicy
{
    private const string WtdsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WTDS";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "ephpol-enable-service",
            Label = "Enhanced Phishing Protection: Enable Service",
            Category = "Enhanced Phishing Protection Policy",
            Description =
                "Enables Windows Defender SmartScreen Enhanced Phishing Protection (WTDS). Monitors corporate passwords entered in browsers and apps for phishing indicators.",
            Tags = ["phishing", "smartscreen", "wtds", "credential", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "Enables the WTDS service; prerequisite for all Enhanced Phishing Protection tweaks.",
            RegistryKeys = [WtdsKey],
            ApplyOps = [RegOp.SetDword(WtdsKey, "ServiceEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(WtdsKey, "ServiceEnabled")],
            DetectOps = [RegOp.CheckDword(WtdsKey, "ServiceEnabled", 1)],
        },
        new TweakDef
        {
            Id = "ephpol-notify-unsafe-app",
            Label = "Enhanced Phishing Protection: Notify on Unsafe App Password Reuse",
            Category = "Enhanced Phishing Protection Policy",
            Description =
                "Warns users when they type their corporate (Azure AD/local) password into apps other than Windows sign-in. Available from Windows 11 22H2.",
            Tags = ["phishing", "smartscreen", "wtds", "credential", "password", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Warns users when corporate password is typed in non-system apps.",
            RegistryKeys = [WtdsKey],
            ApplyOps = [RegOp.SetDword(WtdsKey, "NotifyPasswordReuse", 1)],
            RemoveOps = [RegOp.DeleteValue(WtdsKey, "NotifyPasswordReuse")],
            DetectOps = [RegOp.CheckDword(WtdsKey, "NotifyPasswordReuse", 1)],
        },
        new TweakDef
        {
            Id = "ephpol-notify-unsafe-site",
            Label = "Enhanced Phishing Protection: Notify on Phishing Site Password Entry",
            Category = "Enhanced Phishing Protection Policy",
            Description =
                "Warns users when they type their corporate password onto a site that SmartScreen identifies as phishing. Triggers a warning before the password is submitted.",
            Tags = ["phishing", "smartscreen", "wtds", "credential", "browser", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "Warns users before submitting credentials to a phishing site.",
            RegistryKeys = [WtdsKey],
            ApplyOps = [RegOp.SetDword(WtdsKey, "NotifyMalicious", 1)],
            RemoveOps = [RegOp.DeleteValue(WtdsKey, "NotifyMalicious")],
            DetectOps = [RegOp.CheckDword(WtdsKey, "NotifyMalicious", 1)],
        },
        new TweakDef
        {
            Id = "ephpol-block-plaintext-passwords",
            Label = "Enhanced Phishing Protection: Block Plaintext Password Storage",
            Category = "Enhanced Phishing Protection Policy",
            Description =
                "Prevents users from storing work or school passwords in plain text files (Notepad, Word, etc.). WTDS detects password entry in low-trust document contexts.",
            Tags = ["phishing", "smartscreen", "wtds", "password", "plaintext", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Prevents corporate password entry in plaintext documents.",
            RegistryKeys = [WtdsKey],
            ApplyOps = [RegOp.SetDword(WtdsKey, "CaptureThreatWindow", 1)],
            RemoveOps = [RegOp.DeleteValue(WtdsKey, "CaptureThreatWindow")],
            DetectOps = [RegOp.CheckDword(WtdsKey, "CaptureThreatWindow", 1)],
        },
        new TweakDef
        {
            Id = "ephpol-audit-only-mode",
            Label = "Enhanced Phishing Protection: Set Audit-Only Mode",
            Category = "Enhanced Phishing Protection Policy",
            Description =
                "Puts Enhanced Phishing Protection into audit mode — events are logged but no user warnings are shown. Useful for baseline assessment before enforcing notifications.",
            Tags = ["phishing", "smartscreen", "wtds", "audit", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Logs phishing events without user warnings; for baseline assessment only.",
            RegistryKeys = [WtdsKey],
            ApplyOps = [RegOp.SetDword(WtdsKey, "AuditMode", 1)],
            RemoveOps = [RegOp.DeleteValue(WtdsKey, "AuditMode")],
            DetectOps = [RegOp.CheckDword(WtdsKey, "AuditMode", 1)],
        },
        new TweakDef
        {
            Id = "ephpol-enable-enterprise-indicators",
            Label = "Enhanced Phishing Protection: Enable Enterprise Phishing Indicators",
            Category = "Enhanced Phishing Protection Policy",
            Description =
                "Enables enterprise-specific phishing indicator checks in WTDS, allowing domain-joined and Entra ID-joined devices to use corporate threat intelligence feeds.",
            Tags = ["phishing", "smartscreen", "wtds", "enterprise", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Enables corporate threat intelligence feeds for phishing detection.",
            RegistryKeys = [WtdsKey],
            ApplyOps = [RegOp.SetDword(WtdsKey, "EnterpriseIndicatorsEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(WtdsKey, "EnterpriseIndicatorsEnabled")],
            DetectOps = [RegOp.CheckDword(WtdsKey, "EnterpriseIndicatorsEnabled", 1)],
        },
        new TweakDef
        {
            Id = "ephpol-block-credential-reuse-apps",
            Label = "Enhanced Phishing Protection: Block Credential Reuse Across Apps",
            Category = "Enhanced Phishing Protection Policy",
            Description =
                "Blocks users from reusing their Windows sign-in PIN or password in non-system apps. Reduces password spray attack surface on shared or kiosk machines.",
            Tags = ["phishing", "smartscreen", "wtds", "pin", "credential", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Blocks Windows sign-in PIN/password reuse in non-system apps.",
            RegistryKeys = [WtdsKey],
            ApplyOps = [RegOp.SetDword(WtdsKey, "BlockCredentialReuseInApps", 1)],
            RemoveOps = [RegOp.DeleteValue(WtdsKey, "BlockCredentialReuseInApps")],
            DetectOps = [RegOp.CheckDword(WtdsKey, "BlockCredentialReuseInApps", 1)],
        },
        new TweakDef
        {
            Id = "ephpol-enable-logging",
            Label = "Enhanced Phishing Protection: Enable Diagnostic Logging",
            Category = "Enhanced Phishing Protection Policy",
            Description =
                "Enables detailed WTDS diagnostic logging to the Windows Event Log under Microsoft-Windows-Security-EnhancedPhishingProtection. Useful for SOC triage.",
            Tags = ["phishing", "smartscreen", "wtds", "logging", "soc", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Enables WTDS diagnostic logging to Event Log for SOC triage.",
            RegistryKeys = [WtdsKey],
            ApplyOps = [RegOp.SetDword(WtdsKey, "EnableEventLogging", 1)],
            RemoveOps = [RegOp.DeleteValue(WtdsKey, "EnableEventLogging")],
            DetectOps = [RegOp.CheckDword(WtdsKey, "EnableEventLogging", 1)],
        },
        new TweakDef
        {
            Id = "ephpol-enforce-service",
            Label = "Enhanced Phishing Protection: Enforce Service (Non-Interactive)",
            Category = "Enhanced Phishing Protection Policy",
            Description =
                "Prevents users from disabling or bypassing Enhanced Phishing Protection via Settings. The WTDS service cannot be turned off by non-admins.",
            Tags = ["phishing", "smartscreen", "wtds", "enforce", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Prevents non-admins from disabling Enhanced Phishing Protection.",
            RegistryKeys = [WtdsKey],
            ApplyOps = [RegOp.SetDword(WtdsKey, "ServiceEnforced", 1)],
            RemoveOps = [RegOp.DeleteValue(WtdsKey, "ServiceEnforced")],
            DetectOps = [RegOp.CheckDword(WtdsKey, "ServiceEnforced", 1)],
        },
        new TweakDef
        {
            Id = "ephpol-notify-password-change",
            Label = "Enhanced Phishing Protection: Notify IT on Password Re-Entry After Change",
            Category = "Enhanced Phishing Protection Policy",
            Description =
                "Notifies the IT help desk (via telemetry event) when a user re-enters their previous password after a forced password change. Detects credential-recycling behaviour.",
            Tags = ["phishing", "smartscreen", "wtds", "password", "helpdesk", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Alerts IT when user re-enters previous password after a forced change.",
            RegistryKeys = [WtdsKey],
            ApplyOps = [RegOp.SetDword(WtdsKey, "NotifyPasswordChangeReuse", 1)],
            RemoveOps = [RegOp.DeleteValue(WtdsKey, "NotifyPasswordChangeReuse")],
            DetectOps = [RegOp.CheckDword(WtdsKey, "NotifyPasswordChangeReuse", 1)],
        },
    ];
}
