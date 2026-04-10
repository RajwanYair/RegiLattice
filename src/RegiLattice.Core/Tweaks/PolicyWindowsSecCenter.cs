namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PolicyWindowsSecCenter
{
    private const string AcctProt = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender Security Center\Account protection";
    private const string AppBrowser = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender Security Center\App and Browser protection";
    private const string DevPerf = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender Security Center\Device performance and health";
    private const string DevSec = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender Security Center\Device security";
    private const string FamilyOpts = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender Security Center\Family options";
    private const string FwNetwork =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender Security Center\Firewall and network protection";
    private const string Notif = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender Security Center\Notifications";
    private const string Systray = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender Security Center\Systray";
    private const string VtProt = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender Security Center\Virus and threat protection";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "wsc-hide-account-protection-area",
                Label = "WSC: Hide Account Protection Area",
                Category = "Security — WSC Display",
                Description =
                    "Hides the Account Protection section in Windows Security Center. "
                    + "Reduces clutter in managed or hardened environments where account settings are controlled by policy. "
                    + "The underlying protection mechanisms remain active.",
                Tags = ["wsc", "security-center", "account-protection", "ui"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Visual change only — Account Protection section hidden from Windows Security UI.",
                ApplyOps = [RegOp.SetDword(AcctProt, "HideAccountProtectionArea", 1)],
                RemoveOps = [RegOp.DeleteValue(AcctProt, "HideAccountProtectionArea")],
                DetectOps = [RegOp.CheckDword(AcctProt, "HideAccountProtectionArea", 1)],
            },
            new TweakDef
            {
                Id = "wsc-hide-app-browser-smartscreen",
                Label = "WSC: Hide App & Browser SmartScreen Block",
                Category = "Security — WSC Display",
                Description =
                    "Hides the SmartScreen prompt block inside the App & Browser Protection section. "
                    + "Useful in environments where SmartScreen is managed separately via Defender Advanced policies. "
                    + "SmartScreen protection itself is not disabled by this tweak.",
                Tags = ["wsc", "security-center", "smartscreen", "app-browser"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Hides SmartScreen block UI; actual SmartScreen protection unchanged.",
                ApplyOps = [RegOp.SetDword(AppBrowser, "HideSmartScreenBlock", 1)],
                RemoveOps = [RegOp.DeleteValue(AppBrowser, "HideSmartScreenBlock")],
                DetectOps = [RegOp.CheckDword(AppBrowser, "HideSmartScreenBlock", 1)],
            },
            new TweakDef
            {
                Id = "wsc-prevent-exploit-protection-override",
                Label = "WSC: Prevent Users Overriding Exploit Protection",
                Category = "Security — WSC Display",
                Description =
                    "Blocks users from changing Exploit Protection settings via the Windows Security UI. "
                    + "Ensures that mitigation policy set by administrators or via policy cannot be weakened at the user level. "
                    + "Recommended for hardened workstations and kiosks.",
                Tags = ["wsc", "security-center", "exploit-protection", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Locks Exploit Protection UI; users cannot weaken system mitigations.",
                ApplyOps = [RegOp.SetDword(AppBrowser, "DisallowExploitProtectionOverride", 1)],
                RemoveOps = [RegOp.DeleteValue(AppBrowser, "DisallowExploitProtectionOverride")],
                DetectOps = [RegOp.CheckDword(AppBrowser, "DisallowExploitProtectionOverride", 1)],
            },
            new TweakDef
            {
                Id = "wsc-hide-device-performance-health",
                Label = "WSC: Hide Device Performance and Health Area",
                Category = "Security — WSC Display",
                Description =
                    "Hides the Device Performance & Health area inside Windows Security Center. "
                    + "Removes the Windows Spotlight, Storage Capacity, Device Driver, and Battery Life health checks from the Security UI. "
                    + "Useful in managed environments where these checks are handled by MDM.",
                Tags = ["wsc", "security-center", "device-health", "performance"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Visual change only — Device Performance & Health section removed from Security Center.",
                ApplyOps = [RegOp.SetDword(DevPerf, "HideAllUI", 1)],
                RemoveOps = [RegOp.DeleteValue(DevPerf, "HideAllUI")],
                DetectOps = [RegOp.CheckDword(DevPerf, "HideAllUI", 1)],
            },
            new TweakDef
            {
                Id = "wsc-hide-device-security-area",
                Label = "WSC: Hide Device Security Area",
                Category = "Security — WSC Display",
                Description =
                    "Hides the Device Security section in Windows Security Center including Secure Boot, "
                    + "Core Isolation, and TPM information. Reduces noise in environments where hardware security "
                    + "status is managed centrally and end users do not need to interact with it.",
                Tags = ["wsc", "security-center", "device-security", "secure-boot", "tpm"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Visual change only — Device Security section hidden; Secure Boot and TPM are unaffected.",
                ApplyOps = [RegOp.SetDword(DevSec, "HideAllUI", 1)],
                RemoveOps = [RegOp.DeleteValue(DevSec, "HideAllUI")],
                DetectOps = [RegOp.CheckDword(DevSec, "HideAllUI", 1)],
            },
            new TweakDef
            {
                Id = "wsc-hide-family-options",
                Label = "WSC: Hide Family Options Area",
                Category = "Security — WSC Display",
                Description =
                    "Hides the Family Options section in Windows Security Center. "
                    + "Suitable for enterprise workstations where Microsoft Family Safety features are irrelevant. "
                    + "Removes the parental controls and screen time links from the Security Center UI.",
                Tags = ["wsc", "security-center", "family", "parental-controls"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Visual change only — Family Options section removed from Windows Security UI.",
                ApplyOps = [RegOp.SetDword(FamilyOpts, "UILockdown", 1)],
                RemoveOps = [RegOp.DeleteValue(FamilyOpts, "UILockdown")],
                DetectOps = [RegOp.CheckDword(FamilyOpts, "UILockdown", 1)],
            },
            new TweakDef
            {
                Id = "wsc-hide-firewall-network-area",
                Label = "WSC: Hide Firewall and Network Area",
                Category = "Security — WSC Display",
                Description =
                    "Hides the Firewall & Network Protection section in Windows Security Center. "
                    + "Intended for environments where Windows Firewall is managed by Group Policy and "
                    + "users should not be able to view or modify firewall profiles via the Security UI.",
                Tags = ["wsc", "security-center", "firewall", "network-protection"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Visual change only — Firewall section hidden; active firewall policy is unchanged.",
                ApplyOps = [RegOp.SetDword(FwNetwork, "UILockdown", 1)],
                RemoveOps = [RegOp.DeleteValue(FwNetwork, "UILockdown")],
                DetectOps = [RegOp.CheckDword(FwNetwork, "UILockdown", 1)],
            },
            new TweakDef
            {
                Id = "wsc-disable-all-notifications",
                Label = "WSC: Disable All Security Center Notifications",
                Category = "Security — WSC Display",
                Description =
                    "Disables all notifications from Windows Security Center including threat alerts, "
                    + "virus scan completion, and health check summaries. "
                    + "Values are still logged but no toasts or action center alerts appear. "
                    + "Recommended only in environments that use a separate alerting/SIEM pipeline.",
                Tags = ["wsc", "security-center", "notifications", "toast"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                ImpactNote = "Suppresses all Windows Security toast notifications; security events still logged.",
                ApplyOps = [RegOp.SetDword(Notif, "DisableNotifications", 1)],
                RemoveOps = [RegOp.DeleteValue(Notif, "DisableNotifications")],
                DetectOps = [RegOp.CheckDword(Notif, "DisableNotifications", 1)],
            },
            new TweakDef
            {
                Id = "wsc-disable-enhanced-notifications",
                Label = "WSC: Disable Enhanced Security Notifications",
                Category = "Security — WSC Display",
                Description =
                    "Disables the enhanced notification banners that Windows Security Center shows after "
                    + "a threat is blocked or a major security event occurs. "
                    + "Basic notifications remain functional. "
                    + "Reduces distraction in environments with SOC monitoring.",
                Tags = ["wsc", "security-center", "notifications", "enhanced"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Only enhanced (follow-up) security banners are suppressed; critical alerts remain.",
                ApplyOps = [RegOp.SetDword(Notif, "DisableEnhancedNotifications", 1)],
                RemoveOps = [RegOp.DeleteValue(Notif, "DisableEnhancedNotifications")],
                DetectOps = [RegOp.CheckDword(Notif, "DisableEnhancedNotifications", 1)],
            },
            new TweakDef
            {
                Id = "wsc-hide-systray-icon",
                Label = "WSC: Hide Windows Security System Tray Icon",
                Category = "Security — WSC Display",
                Description =
                    "Prevents the Windows Security icon from appearing in the system tray notification area. "
                    + "The icon is removed from the taskbar corner; Security Center itself remains fully functional. "
                    + "Useful for decluttering the notification area on managed desktops.",
                Tags = ["wsc", "security-center", "systray", "taskbar"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Removes WSC tray icon; no change to security protection levels.",
                ApplyOps = [RegOp.SetDword(Systray, "HideSystray", 1)],
                RemoveOps = [RegOp.DeleteValue(Systray, "HideSystray")],
                DetectOps = [RegOp.CheckDword(Systray, "HideSystray", 1)],
            },
        ];
}
