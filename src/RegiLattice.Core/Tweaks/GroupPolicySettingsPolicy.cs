// RegiLattice.Core — Tweaks/GroupPolicySettingsPolicy.cs
// Group Policy infrastructure hardening GPO controls — Sprint 217.
// Hardens Group Policy processing, refresh interval, and logging.
// Category: "Group Policy Settings" | Slug: gppol
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GroupPolicy

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class GroupPolicySettingsPolicy
{
    private const string Key =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GroupPolicy";

    // Well-known Group Policy Object GUID for User Configuration CSE
    private const string UserCseKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GroupPolicy\{35378EAC-683F-11D2-A89A-00C04FBBCFA2}";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "gppol-disable-slow-link-detection",
                Label = "Disable Slow-Link GP Processing Skip",
                Category = "Group Policy Settings",
                Description =
                    "By default, some Group Policy CSEs (such as Software Installation) are skipped when a slow network link is detected. Disabling this exception ensures all policies are fully applied even over slow connections. Default: slow-link skip enabled. Recommended: 1 on all managed endpoints.",
                Tags = ["group-policy", "slow-link", "processing", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Group Policy fully applies on all network connections including slow links; no CSEs are skipped.",
                ApplyOps =
                [
                    RegOp.SetDword(
                        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GroupPolicy\{35378EAC-683F-11D2-A89A-00C04FBBCFA2}",
                        "NoSlowLink",
                        0
                    ),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(
                        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GroupPolicy\{35378EAC-683F-11D2-A89A-00C04FBBCFA2}",
                        "NoSlowLink"
                    ),
                ],
                DetectOps =
                [
                    RegOp.CheckDword(
                        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GroupPolicy\{35378EAC-683F-11D2-A89A-00C04FBBCFA2}",
                        "NoSlowLink",
                        0
                    ),
                ],
            },
            new TweakDef
            {
                Id = "gppol-force-reprocess-changed",
                Label = "Force Reprocessing of Changed GP Objects",
                Category = "Group Policy Settings",
                Description =
                    "Instructs the Group Policy client to reapply all policy settings at each background refresh cycle, even if no GPO has changed since the last refresh. Ensures policy drift (caused by local changes) is corrected at the next refresh. Default: only changed GPOs are reprocessed. Recommended: 1.",
                Tags = ["group-policy", "refresh", "reprocess", "compliance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "All policy settings are re-applied at every background refresh cycle; local configuration drift is corrected automatically.",
                ApplyOps =
                [
                    RegOp.SetDword(
                        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GroupPolicy\{35378EAC-683F-11D2-A89A-00C04FBBCFA2}",
                        "NoBackgroundPolicy",
                        0
                    ),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(
                        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GroupPolicy\{35378EAC-683F-11D2-A89A-00C04FBBCFA2}",
                        "NoBackgroundPolicy"
                    ),
                ],
                DetectOps =
                [
                    RegOp.CheckDword(
                        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GroupPolicy\{35378EAC-683F-11D2-A89A-00C04FBBCFA2}",
                        "NoBackgroundPolicy",
                        0
                    ),
                ],
            },
            new TweakDef
            {
                Id = "gppol-set-refresh-interval-30min",
                Label = "Set GP Background Refresh Interval to 30 Minutes",
                Category = "Group Policy Settings",
                Description =
                    "Reduces the background Group Policy refresh interval from the default 90 minutes (+30-minute random offset) to 30 minutes. Faster refresh means policy changes reach devices sooner and local configuration drift is corrected more quickly. Default: 90 minutes. Recommended: 30 for dynamic policy environments.",
                Tags = ["group-policy", "refresh-interval", "compliance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Group Policy background refresh fires every 30 minutes; policy changes propagate to devices within half an hour.",
                ApplyOps = [RegOp.SetDword(Key, "GroupPolicyRefreshTime", 30)],
                RemoveOps = [RegOp.DeleteValue(Key, "GroupPolicyRefreshTime")],
                DetectOps = [RegOp.CheckDword(Key, "GroupPolicyRefreshTime", 30)],
            },
            new TweakDef
            {
                Id = "gppol-set-refresh-offset-0",
                Label = "Set GP Refresh Random Offset to 0",
                Category = "Group Policy Settings",
                Description =
                    "Removes the random time offset added to each policy refresh interval. The default offset spreads refresh load across the interval; setting it to 0 makes refreshes predictable and easier to correlate with compliance scan windows. Default: 30-minute random offset. Recommended: 0 in controlled networks.",
                Tags = ["group-policy", "refresh-interval", "offset", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "GP refresh fires at exactly the configured interval with no random delay; refresh times are deterministic.",
                ApplyOps = [RegOp.SetDword(Key, "GroupPolicyRefreshTimeDC", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "GroupPolicyRefreshTimeDC")],
                DetectOps = [RegOp.CheckDword(Key, "GroupPolicyRefreshTimeDC", 0)],
            },
            new TweakDef
            {
                Id = "gppol-enable-verbose-logging",
                Label = "Enable Verbose GP Processing Logging",
                Category = "Group Policy Settings",
                Description =
                    "Writes detailed diagnostic information about each Group Policy processing cycle to the Group Policy Operational event log. Enables troubleshooting of GPO application failures and security audit of policy application. Default: limited operational logging. Recommended: 1 on managed endpoints.",
                Tags = ["group-policy", "logging", "audit", "diagnostics", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Detailed GP processing events written to Microsoft-Windows-GroupPolicy/Operational log.",
                ApplyOps = [RegOp.SetDword(Key, "EnableVerboseLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableVerboseLogging")],
                DetectOps = [RegOp.CheckDword(Key, "EnableVerboseLogging", 1)],
            },
            new TweakDef
            {
                Id = "gppol-disable-user-gpo-override",
                Label = "Prevent Users from Overriding Group Policy Settings",
                Category = "Group Policy Settings",
                Description =
                    "Blocks users from modifying registry keys that are managed by Group Policy, even when those keys are under HKCU. Without this, a technically savvy user could temporarily override a GPO setting by writing directly to HKCU. Default: HKCU writes allowed. Recommended: 1 on high-security desktops.",
                Tags = ["group-policy", "user-override", "security", "hkcu", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Users cannot bypass HKCU-targeted GP settings by writing conflicting registry values.",
                ApplyOps = [RegOp.SetDword(Key, "DisableUserGPOOverride", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableUserGPOOverride")],
                DetectOps = [RegOp.CheckDword(Key, "DisableUserGPOOverride", 1)],
            },
            new TweakDef
            {
                Id = "gppol-apply-during-logon",
                Label = "Apply Group Policy Synchronously at Logon",
                Category = "Group Policy Settings",
                Description =
                    "Forces Group Policy to be applied synchronously at logon — the desktop does not appear until all policies have been processed. Prevents users from interacting with the desktop before security policies (such as drive mappings, logon scripts, and folder redirection) are applied. Default: async logon on workstations. Recommended: 1.",
                Tags = ["group-policy", "logon", "synchronous", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Logon is slightly slower but all policies are guaranteed applied before the desktop appears.",
                ApplyOps = [RegOp.SetDword(Key, "SynchronousMachineGroupPolicy", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "SynchronousMachineGroupPolicy")],
                DetectOps = [RegOp.CheckDword(Key, "SynchronousMachineGroupPolicy", 1)],
            },
            new TweakDef
            {
                Id = "gppol-log-rsop-data",
                Label = "Enable RSoP (Resultant Set of Policy) Logging",
                Category = "Group Policy Settings",
                Description =
                    "Enables collection and logging of Resultant Set of Policy data, which records exactly which policies are applied to each user and computer. Required for the 'Logging' mode of Group Policy Modeling and for compliance audits that verify policy coverage. Default: RSoP logging enabled but may be disabled by some hardening guides. Recommended: 1.",
                Tags = ["group-policy", "rsop", "logging", "audit", "compliance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "RSoP data is collected; IT can run Group Policy Results wizard to verify policy application.",
                ApplyOps = [RegOp.SetDword(Key, "DisableLocalGPOs", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableLocalGPOs")],
                DetectOps = [RegOp.CheckDword(Key, "DisableLocalGPOs", 0)],
            },
            new TweakDef
            {
                Id = "gppol-block-local-gpo",
                Label = "Disable Local Group Policy Objects for Domain Members",
                Category = "Group Policy Settings",
                Description =
                    "Prevents Local GPOs (lgpo.exe modifications, Local Security Policy) from being applied on domain-joined machines. When domain GPOs manage all settings, local GPOs can introduce conflicts or be used to circumvent domain policy. Default: local GPOs applied. Recommended: 1 on all domain-joined machines.",
                Tags = ["group-policy", "local-gpo", "domain", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Local Group Policy Objects are ignored; only domain-delivered GPOs apply.",
                ApplyOps = [RegOp.SetDword(Key, "DisableLocalGPO", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableLocalGPO")],
                DetectOps = [RegOp.CheckDword(Key, "DisableLocalGPO", 1)],
            },
            new TweakDef
            {
                Id = "gppol-require-secure-channel",
                Label = "Require Secure Channel for GP Download",
                Category = "Group Policy Settings",
                Description =
                    "Forces Windows to use a signed and encrypted secure channel (Kerberos) when downloading GPOs from the domain controller. Prevents man-in-the-middle attacks that inject malicious policy settings during transport. Default: secure channel used but not strictly enforced for all GPOs. Recommended: 1.",
                Tags = ["group-policy", "secure-channel", "kerberos", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "GP downloads are authenticated and encrypted; rogue policy injection during GPO download is blocked.",
                ApplyOps = [RegOp.SetDword(Key, "RequireSecureChannel", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireSecureChannel")],
                DetectOps = [RegOp.CheckDword(Key, "RequireSecureChannel", 1)],
            },
        ];
}
