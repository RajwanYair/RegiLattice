// RegiLattice.Core — Tweaks/SecurityCenterPolicy.cs
// Sprint 269: Windows Security Center Group Policy (10 tweaks)
// Category: "Security Center Policy" | Slug: secctr
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SecurityCenter

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class SecurityCenterPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SecurityCenter";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "secctr-disable-security-center",
            Label = "Disable Windows Security Center",
            Category = "Security Center Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            Description =
                "Sets NoSecurityCenter=1 in the SecurityCenter policy key. Disables the "
                + "Windows Security Center (WSC) service and its tray icon. WSC provides "
                + "unified visibility into antivirus, firewall, updates, and UAC status. "
                + "Disabling it removes the notification hub but does not disable the "
                + "underlying security features. Suitable for environments with third-party "
                + "endpoint management tools. Default: 0. Recommended: enterprise use only.",
            Tags = ["security-center", "wsc", "tray", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "NoSecurityCenter", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "NoSecurityCenter")],
            DetectOps = [RegOp.CheckDword(Key, "NoSecurityCenter", 1)],
        },
        new TweakDef
        {
            Id = "secctr-disable-spyware-monitoring",
            Label = "Disable Security Center Spyware Monitoring",
            Category = "Security Center Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Sets DisableAntiSpywareMonitoring=1 in the SecurityCenter policy key. "
                + "Prevents Windows Security Center from monitoring and reporting the status "
                + "of installed anti-spyware solutions. Useful when a third-party endpoint "
                + "protection platform manages spyware detection and WSC reports a false "
                + "warning about missing coverage. Default: 0. Recommended: when using a "
                + "non-WSC-integrated security suite.",
            Tags = ["security-center", "antispyware", "monitoring", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableAntiSpywareMonitoring", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableAntiSpywareMonitoring")],
            DetectOps = [RegOp.CheckDword(Key, "DisableAntiSpywareMonitoring", 1)],
        },
        new TweakDef
        {
            Id = "secctr-disable-antivirus-monitoring",
            Label = "Disable Security Center Antivirus Monitoring",
            Category = "Security Center Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Sets DisableAntiVirusMonitoring=1 in the SecurityCenter policy key. Stops "
                + "Windows Security Center from monitoring and alerting on antivirus product "
                + "status. Enterprise environments using CrowdStrike, Sentinel One, or other "
                + "third-party AV tools that do not fully integrate with WSC may see constant "
                + "warning notifications. This policy suppresses those. "
                + "Default: 0. Recommended: where third-party AV is managed separately.",
            Tags = ["security-center", "antivirus", "monitoring", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableAntiVirusMonitoring", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableAntiVirusMonitoring")],
            DetectOps = [RegOp.CheckDword(Key, "DisableAntiVirusMonitoring", 1)],
        },
        new TweakDef
        {
            Id = "secctr-disable-firewall-monitoring",
            Label = "Disable Security Center Firewall Monitoring",
            Category = "Security Center Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Sets DisableFirewallMonitoring=1 in the SecurityCenter policy key. Prevents "
                + "Windows Security Center from reporting the status of the Windows firewall or "
                + "third-party firewall products. When a hardware firewall or enterprise network "
                + "appliance provides perimeter protection, WSC firewall alerts are noise. "
                + "Default: 0. Recommended: network-locked enterprise LAN environments.",
            Tags = ["security-center", "firewall", "monitoring", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableFirewallMonitoring", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableFirewallMonitoring")],
            DetectOps = [RegOp.CheckDword(Key, "DisableFirewallMonitoring", 1)],
        },
        new TweakDef
        {
            Id = "secctr-disable-update-monitoring",
            Label = "Disable Security Center Windows Update Monitoring",
            Category = "Security Center Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Sets DisableWindowsUpdateMonitoring=1 in the SecurityCenter policy key. "
                + "Stops Security Center from surfacing Windows Update status alerts. In "
                + "managed environments where WSUS, MECM, or Intune controls the update "
                + "schedule, WSC update prompts are redundant and confusing. "
                + "Default: 0. Recommended: managed update pipelines only.",
            Tags = ["security-center", "windows-update", "monitoring", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableWindowsUpdateMonitoring", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableWindowsUpdateMonitoring")],
            DetectOps = [RegOp.CheckDword(Key, "DisableWindowsUpdateMonitoring", 1)],
        },
        new TweakDef
        {
            Id = "secctr-disable-uac-monitoring",
            Label = "Disable Security Center UAC Monitoring",
            Category = "Security Center Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Sets DisableUACMonitoring=1 in the SecurityCenter policy key. Prevents "
                + "Security Center from monitoring User Account Control state and alerting "
                + "when UAC is disabled or misconfigured. Useful when UAC policy is managed "
                + "via separate GP objects and WSC warnings are contradictory to intent. "
                + "Default: 0. Recommended: only when UAC is governed by dedicated policy.",
            Tags = ["security-center", "uac", "monitoring", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableUACMonitoring", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableUACMonitoring")],
            DetectOps = [RegOp.CheckDword(Key, "DisableUACMonitoring", 1)],
        },
        new TweakDef
        {
            Id = "secctr-disable-internet-monitoring",
            Label = "Disable Security Center Internet Security Monitoring",
            Category = "Security Center Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Sets DisableInternetSecurityMonitoring=1 in the SecurityCenter policy key. "
                + "Stops Security Center from checking and alerting on Internet Explorer / "
                + "Microsoft Edge security zone settings. In environments where browser "
                + "security is locked down via separate GP templates, duplicate WSC reports "
                + "add noise. Default: 0. Recommended: enterprise-managed browser fleets.",
            Tags = ["security-center", "internet", "browser", "monitoring", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableInternetSecurityMonitoring", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableInternetSecurityMonitoring")],
            DetectOps = [RegOp.CheckDword(Key, "DisableInternetSecurityMonitoring", 1)],
        },
        new TweakDef
        {
            Id = "secctr-hide-systray-icon",
            Label = "Hide Windows Security Center Tray Icon",
            Category = "Security Center Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Description =
                "Sets HideSystrayIcon=1 in the SecurityCenter policy key. Removes the "
                + "Windows Security tray icon from the notification area, including the "
                + "shield badge that appears on Start. The underlying Security Center "
                + "service continues to run; only the visual indicator is suppressed. "
                + "Useful for kiosk or locked-down desktop configurations. "
                + "Default: 0 (icon visible). Recommended: kiosk environments.",
            Tags = ["security-center", "systray", "tray", "icon", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "HideSystrayIcon", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "HideSystrayIcon")],
            DetectOps = [RegOp.CheckDword(Key, "HideSystrayIcon", 1)],
        },
        new TweakDef
        {
            Id = "secctr-disable-account-monitoring",
            Label = "Disable Security Center Account Protection Monitoring",
            Category = "Security Center Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Sets DisableAccountMonitoring=1 in the SecurityCenter policy key. Prevents "
                + "Security Center from surfacing Account Protection recommendations (Windows "
                + "Hello, lock screen PIN, dynamic lock). In environments using smart-card "
                + "or certificate-based authentication, these prompts encourage conflicting "
                + "credential configurations. Default: 0. Recommended: smart-card auth.",
            Tags = ["security-center", "account", "windows-hello", "monitoring", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableAccountMonitoring", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableAccountMonitoring")],
            DetectOps = [RegOp.CheckDword(Key, "DisableAccountMonitoring", 1)],
        },
        new TweakDef
        {
            Id = "secctr-disable-notifications",
            Label = "Disable Security Center Notification Toasts",
            Category = "Security Center Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Sets DisableNotifications=1 in the SecurityCenter policy key. Suppresses "
                + "all Security Center action-required and informational notification toasts. "
                + "WSC notifications for status changes (protection off, update pending) will "
                + "not appear as toast banners; the Security app itself still shows status. "
                + "Default: 0. Recommended: managed estates where alerts go to SIEM instead.",
            Tags = ["security-center", "notifications", "toasts", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableNotifications", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableNotifications")],
            DetectOps = [RegOp.CheckDword(Key, "DisableNotifications", 1)],
        },
    ];
}
