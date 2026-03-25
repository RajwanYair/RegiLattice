#nullable enable
using System.Collections.Generic;
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

// Sprint 259 — Cloud Desktop Policy (10 tweaks)
// Keys: HKLM\SOFTWARE\Policies\Microsoft\CloudDesktop
//       HKLM\SOFTWARE\Policies\Microsoft\CloudPC  (Windows 365 Cloud PC policy key)
// Controls Windows 365 Cloud PC / Azure Virtual Desktop presence and provisioning.
// Slug: "clouddesk-"
internal static class CloudDesktopPolicy
{
    private const string CdKey  = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\CloudDesktop";
    private const string CpcKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\CloudPC";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "clouddesk-disable-cloud-pc-entry-points",
            Label = "Disable Cloud PC Entry Points in Windows UI",
            Category = "Cloud Desktop Policy",
            Description = "Sets DisableCloudPCEntryPoints=1 in the CloudDesktop policy key. "
                + "Removes the Windows 365 Cloud PC link, button, and notification from the "
                + "Windows Start menu, Settings, and taskbar. Prevents users from seeing or clicking "
                + "entry points that would prompt them to sign up for or access a Windows 365 subscription. "
                + "Appropriate for organizations that do not use Windows 365. "
                + "Default: absent (entry points shown). Recommended: 1 on non-W365 endpoints.",
            Tags = ["cloud-desktop", "windows-365", "cloud-pc", "ui", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Windows 365 Cloud PC entry points removed from Windows UI.",
            ApplyOps  = [RegOp.SetDword(CdKey, "DisableCloudPCEntryPoints", 1)],
            RemoveOps = [RegOp.DeleteValue(CdKey, "DisableCloudPCEntryPoints")],
            DetectOps = [RegOp.CheckDword(CdKey, "DisableCloudPCEntryPoints", 1)],
        },
        new TweakDef
        {
            Id = "clouddesk-disable-provisioning",
            Label = "Disable Cloud PC Provisioning",
            Category = "Cloud Desktop Policy",
            Description = "Sets EnableProvisioning=0 in the CloudDesktop policy key. "
                + "Prevents the Windows 365 agent from auto-provisioning a Cloud PC session on this device. "
                + "Useful on physical endpoints that should never auto-redirect to a cloud desktop, "
                + "ensuring users always work on the local machine's resources. "
                + "Default: absent (provisioning allowed). Recommended: 0 on standard physical desktops.",
            Tags = ["cloud-desktop", "windows-365", "provisioning", "cloud-pc", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Cloud PC auto-provisioning disabled; users must connect manually if needed.",
            ApplyOps  = [RegOp.SetDword(CdKey, "EnableProvisioning", 0)],
            RemoveOps = [RegOp.DeleteValue(CdKey, "EnableProvisioning")],
            DetectOps = [RegOp.CheckDword(CdKey, "EnableProvisioning", 0)],
        },
        new TweakDef
        {
            Id = "clouddesk-disable-virtual-desktop-agent",
            Label = "Disable Cloud PC Agent Auto-Start",
            Category = "Cloud Desktop Policy",
            Description = "Sets DisableCloudPCAgent=1 in the CloudDesktop policy key. "
                + "Prevents the Windows 365/Cloud PC management agent from auto-starting at user login. "
                + "The agent monitors session state and applies Cloud PC policies; disabling it prevents "
                + "Windows 365 session management from running on machines that should not connect to "
                + "any cloud desktop infrastructure. "
                + "Default: absent (agent starts automatically). Recommended: 1 on non-W365 machines.",
            Tags = ["cloud-desktop", "windows-365", "agent", "startup", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Windows 365 Cloud PC management agent blocked from auto-starting at login.",
            ApplyOps  = [RegOp.SetDword(CdKey, "DisableCloudPCAgent", 1)],
            RemoveOps = [RegOp.DeleteValue(CdKey, "DisableCloudPCAgent")],
            DetectOps = [RegOp.CheckDword(CdKey, "DisableCloudPCAgent", 1)],
        },
        new TweakDef
        {
            Id = "clouddesk-disable-cloudpc-connection-uac",
            Label = "Disable Cloud PC UAC Elevation Prompts",
            Category = "Cloud Desktop Policy",
            Description = "Sets NoAdminUACForCloudPC=1 in the CloudDesktop policy key. "
                + "Prevents the Cloud PC connection process from triggering UAC elevation dialogs "
                + "on the local machine. When a Cloud PC session needs elevated rights, the request "
                + "is handled within the remote cloud session — not on the local endpoint. "
                + "Reduces login friction on kiosk machines where Cloud PC is the primary desktop. "
                + "Default: absent. Recommended: 1 on dedicated Cloud PC access endpoints.",
            Tags = ["cloud-desktop", "uac", "elevation", "cloud-pc", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "Cloud PC connection process does not prompt UAC elevation on the local machine.",
            ApplyOps  = [RegOp.SetDword(CdKey, "NoAdminUACForCloudPC", 1)],
            RemoveOps = [RegOp.DeleteValue(CdKey, "NoAdminUACForCloudPC")],
            DetectOps = [RegOp.CheckDword(CdKey, "NoAdminUACForCloudPC", 1)],
        },
        new TweakDef
        {
            Id = "clouddesk-disable-single-sign-on",
            Label = "Disable Single Sign-On to Cloud PC",
            Category = "Cloud Desktop Policy",
            Description = "Sets DisableSSO=1 in the CloudPC policy key. "
                + "Prevents automatic single sign-on (SSO) to the Windows 365 Cloud PC using the "
                + "local Windows account credentials. When SSO is enabled, a logged-in user is "
                + "automatically authenticated to the Cloud PC session without re-entering credentials. "
                + "Disabling SSO requires users to explicitly authenticate each Cloud PC session, "
                + "providing an additional security checkpoint. "
                + "Default: absent (SSO enabled). Recommended: 1 for high-security access control.",
            Tags = ["cloud-desktop", "sso", "authentication", "windows-365", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Cloud PC SSO disabled; users explicitly authenticate each Cloud PC session.",
            ApplyOps  = [RegOp.SetDword(CpcKey, "DisableSSO", 1)],
            RemoveOps = [RegOp.DeleteValue(CpcKey, "DisableSSO")],
            DetectOps = [RegOp.CheckDword(CpcKey, "DisableSSO", 1)],
        },
        new TweakDef
        {
            Id = "clouddesk-enable-cloud-pc-telemetry-opt-out",
            Label = "Opt Out of Cloud PC Telemetry",
            Category = "Cloud Desktop Policy",
            Description = "Sets DisableTelemetry=1 in the CloudPC policy key. "
                + "Prevents the Cloud PC client from sending diagnostics, usage telemetry, and "
                + "session-quality metrics to Microsoft's Windows 365 service. "
                + "Applicable in privacy-sensitive environments or air-gapped networks where "
                + "outbound telemetry must be minimised. "
                + "Default: absent (telemetry enabled). Recommended: 1 in high-privacy environments.",
            Tags = ["cloud-desktop", "telemetry", "privacy", "windows-365", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Cloud PC client telemetry to Microsoft suppressed.",
            ApplyOps  = [RegOp.SetDword(CpcKey, "DisableTelemetry", 1)],
            RemoveOps = [RegOp.DeleteValue(CpcKey, "DisableTelemetry")],
            DetectOps = [RegOp.CheckDword(CpcKey, "DisableTelemetry", 1)],
        },
        new TweakDef
        {
            Id = "clouddesk-restrict-cloud-pc-regions",
            Label = "Restrict Cloud PC Provisioning to Closest Region Only",
            Category = "Cloud Desktop Policy",
            Description = "Sets RegionSelectionPolicy=1 in the CloudPC policy key. "
                + "Forces the Windows 365 provisioning system to select only the closest Azure region "
                + "when allocating a new Cloud PC, instead of allowing cross-region or scheduled-region "
                + "provisioning. Ensures low latency for users and keeps data residency within the "
                + "organisation's primary Azure geography. "
                + "Default: absent (any region). Recommended: 1 for data residency compliance.",
            Tags = ["cloud-desktop", "region", "data-residency", "windows-365", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Cloud PC provisioned in closest Azure region only; data stays in primary geography.",
            ApplyOps  = [RegOp.SetDword(CpcKey, "RegionSelectionPolicy", 1)],
            RemoveOps = [RegOp.DeleteValue(CpcKey, "RegionSelectionPolicy")],
            DetectOps = [RegOp.CheckDword(CpcKey, "RegionSelectionPolicy", 1)],
        },
        new TweakDef
        {
            Id = "clouddesk-disable-cloud-pc-share-clipboard",
            Label = "Disable Clipboard Sharing Between Cloud PC and Local",
            Category = "Cloud Desktop Policy",
            Description = "Sets DisableServerClipboard=1 in the CloudPC policy key. "
                + "Prevents clipboard content from being shared between the local endpoint and the "
                + "Windows 365 Cloud PC session. Clipboard sync can be a vector for data exfiltration "
                + "(copy from cloud session, paste to local — or vice versa). "
                + "Disabling this enforces a hard data-boundary between local and cloud environments. "
                + "Default: absent (clipboard sharing enabled). Recommended: 1 for DLP compliance.",
            Tags = ["cloud-desktop", "clipboard", "data-leakage", "windows-365", "dlp", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Clipboard not shared between Cloud PC and local endpoint session.",
            ApplyOps  = [RegOp.SetDword(CpcKey, "DisableServerClipboard", 1)],
            RemoveOps = [RegOp.DeleteValue(CpcKey, "DisableServerClipboard")],
            DetectOps = [RegOp.CheckDword(CpcKey, "DisableServerClipboard", 1)],
        },
        new TweakDef
        {
            Id = "clouddesk-disable-cloud-pc-redirect-printers",
            Label = "Disable Printer Redirection for Cloud PC Sessions",
            Category = "Cloud Desktop Policy",
            Description = "Sets DisablePrinterRedirection=1 in the CloudPC policy key. "
                + "Prevents local printers attached to the endpoint from being presented inside the "
                + "Windows 365 Cloud PC session. Printer redirection streams print jobs from the cloud "
                + "session to a local network printer, but can expose printer model/driver information "
                + "across the cloud boundary. Disabling this restricts Cloud PC sessions to cloud-side "
                + "printing only. Default: absent (redirection enabled).",
            Tags = ["cloud-desktop", "printer", "redirection", "windows-365", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Local printers not redirected into Cloud PC sessions.",
            ApplyOps  = [RegOp.SetDword(CpcKey, "DisablePrinterRedirection", 1)],
            RemoveOps = [RegOp.DeleteValue(CpcKey, "DisablePrinterRedirection")],
            DetectOps = [RegOp.CheckDword(CpcKey, "DisablePrinterRedirection", 1)],
        },
        new TweakDef
        {
            Id = "clouddesk-set-max-session-idle-timeout",
            Label = "Set Cloud PC Session Idle Disconnect Timeout",
            Category = "Cloud Desktop Policy",
            Description = "Sets IdleSessionTimeout=30 in the CloudPC policy key. "
                + "Sets the maximum idle time (in minutes) before a Windows 365 Cloud PC session is "
                + "automatically disconnected. Idle Cloud PC sessions continue to consume Azure compute "
                + "and network resources. Auto-disconnect after 30 minutes of inactivity reduces costs "
                + "and ensures unattended sessions do not remain accessible for extended periods. "
                + "Default: absent (no idle timeout). Recommended: 15-60 depending on TCO requirements.",
            Tags = ["cloud-desktop", "session", "idle", "timeout", "windows-365", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Cloud PC sessions disconnected after 30 minutes of inactivity; saves Azure compute cost.",
            ApplyOps  = [RegOp.SetDword(CpcKey, "IdleSessionTimeout", 30)],
            RemoveOps = [RegOp.DeleteValue(CpcKey, "IdleSessionTimeout")],
            DetectOps = [RegOp.CheckDword(CpcKey, "IdleSessionTimeout", 30)],
        },
    ];
}
