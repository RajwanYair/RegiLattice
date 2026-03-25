#nullable enable
using System.Collections.Generic;
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

// Sprint 252 — ActiveX Installer Service Policy (10 tweaks)
// Keys: HKLM\SOFTWARE\Policies\Microsoft\Windows\AxInstaller
internal static class ActiveXInstallerServicePolicy
{
    private const string AxKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AxInstaller";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "axinst-disable-activex-install",
            Label = "Disable ActiveX Installer Service",
            Category = "ActiveX Installer Service Policy",
            Description =
                "Sets DoNotRunAxInstaller=1 in the AxInstaller policy key. "
                + "Prevents the ActiveX Installer Service from installing or updating ActiveX controls on this machine. "
                + "Recommended on all modern systems where legacy ActiveX content is not required, "
                + "reducing the attack surface from malicious or out-of-date ActiveX controls. "
                + "Default: absent (service runs). Recommended: 1 for all non-IE-enterprise deployments.",
            Tags = ["activex", "installer", "legacy", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "ActiveX Installer Service disabled; ActiveX controls cannot be installed or updated machine-wide.",
            ApplyOps = [RegOp.SetDword(AxKey, "DoNotRunAxInstaller", 1)],
            RemoveOps = [RegOp.DeleteValue(AxKey, "DoNotRunAxInstaller")],
            DetectOps = [RegOp.CheckDword(AxKey, "DoNotRunAxInstaller", 1)],
        },
        new TweakDef
        {
            Id = "axinst-require-admin-approval",
            Label = "Require Admin Approval for ActiveX Install",
            Category = "ActiveX Installer Service Policy",
            Description =
                "Sets RequireApproval=1 in the AxInstaller policy key. "
                + "Forces the ActiveX Installer Service to require administrator approval "
                + "before installing any ActiveX control, even for controls from trusted zones. "
                + "Prevents silent ActiveX installation by non-admin users in enterprise environments. "
                + "Default: absent (controls install silently from trusted zones). Recommended: 1.",
            Tags = ["activex", "approval", "admin", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "ActiveX installs require explicit admin approval even from trusted sites.",
            ApplyOps = [RegOp.SetDword(AxKey, "RequireApproval", 1)],
            RemoveOps = [RegOp.DeleteValue(AxKey, "RequireApproval")],
            DetectOps = [RegOp.CheckDword(AxKey, "RequireApproval", 1)],
        },
        new TweakDef
        {
            Id = "axinst-disable-trusted-zone-only",
            Label = "Block ActiveX Install from Untrusted Zones",
            Category = "ActiveX Installer Service Policy",
            Description =
                "Sets DisableActiveXInstallFromUntrustedZones=1 in the AxInstaller policy key. "
                + "Prevents the ActiveX Installer Service from processing install requests "
                + "for controls that originate from untrusted or restricted security zones. "
                + "Controls from the Internet zone and restricted sites are blocked. "
                + "Default: absent (all zones allowed). Recommended: 1.",
            Tags = ["activex", "zones", "untrusted", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "ActiveX installs from untrusted or Internet zones blocked by the installer service.",
            ApplyOps = [RegOp.SetDword(AxKey, "DisableActiveXInstallFromUntrustedZones", 1)],
            RemoveOps = [RegOp.DeleteValue(AxKey, "DisableActiveXInstallFromUntrustedZones")],
            DetectOps = [RegOp.CheckDword(AxKey, "DisableActiveXInstallFromUntrustedZones", 1)],
        },
        new TweakDef
        {
            Id = "axinst-log-successful-installs",
            Label = "Enable ActiveX Install Success Logging",
            Category = "ActiveX Installer Service Policy",
            Description =
                "Sets LoggingEnabled=1 in the AxInstaller policy key. "
                + "Instructs the ActiveX Installer Service to write a log entry to the Windows Event Log "
                + "for every successfully installed ActiveX control, including source URL and CLSID. "
                + "Supports compliance auditing of legacy ActiveX deployments. "
                + "Default: absent (no success logging). Recommended: 1 in audit-aware environments.",
            Tags = ["activex", "logging", "audit", "event-log", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "ActiveX successful install events logged to the Application event log.",
            ApplyOps = [RegOp.SetDword(AxKey, "LoggingEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(AxKey, "LoggingEnabled")],
            DetectOps = [RegOp.CheckDword(AxKey, "LoggingEnabled", 1)],
        },
        new TweakDef
        {
            Id = "axinst-log-failed-installs",
            Label = "Enable ActiveX Install Failure Logging",
            Category = "ActiveX Installer Service Policy",
            Description =
                "Sets ErrorLoggingEnabled=1 in the AxInstaller policy key. "
                + "Instructs the ActiveX Installer Service to write error events to the Windows Event Log "
                + "for failed ActiveX control installations, including the error code and control CLSID. "
                + "Default: absent (errors silently discarded). Recommended: 1 to track blocked or failing installs.",
            Tags = ["activex", "logging", "errors", "event-log", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "ActiveX install failure and error events logged to the Application event log.",
            ApplyOps = [RegOp.SetDword(AxKey, "ErrorLoggingEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(AxKey, "ErrorLoggingEnabled")],
            DetectOps = [RegOp.CheckDword(AxKey, "ErrorLoggingEnabled", 1)],
        },
        new TweakDef
        {
            Id = "axinst-disable-activex-update",
            Label = "Disable Automatic ActiveX Control Updates",
            Category = "ActiveX Installer Service Policy",
            Description =
                "Sets DisableAxUpdate=1 in the AxInstaller policy key. "
                + "Prevents the ActiveX Installer Service from automatically updating existing ActiveX controls "
                + "to newer versions when a web page requests an update. Helps maintain a known-good control state "
                + "in locked-down enterprise environments. "
                + "Default: absent (updates allowed). Recommended: 1 when ActiveX control versions are change-managed.",
            Tags = ["activex", "update", "version-lock", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "ActiveX controls not automatically updated; only initial installs processed by the service.",
            ApplyOps = [RegOp.SetDword(AxKey, "DisableAxUpdate", 1)],
            RemoveOps = [RegOp.DeleteValue(AxKey, "DisableAxUpdate")],
            DetectOps = [RegOp.CheckDword(AxKey, "DisableAxUpdate", 1)],
        },
        new TweakDef
        {
            Id = "axinst-block-per-user-install",
            Label = "Block Per-User ActiveX Control Installation",
            Category = "ActiveX Installer Service Policy",
            Description =
                "Sets BlockPerUserInstall=1 in the AxInstaller policy key. "
                + "Prevents the ActiveX Installer Service from installing controls in per-user profile locations, "
                + "forcing all ActiveX installations to the machine-wide registry or Program Files. "
                + "Prevents users from silently deploying controls into their own profile. "
                + "Default: absent (per-user installs allowed). Recommended: 1 in enterprise environments.",
            Tags = ["activex", "per-user", "profile", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Per-user ActiveX control installations blocked; only machine-wide installs by admins are permitted.",
            ApplyOps = [RegOp.SetDword(AxKey, "BlockPerUserInstall", 1)],
            RemoveOps = [RegOp.DeleteValue(AxKey, "BlockPerUserInstall")],
            DetectOps = [RegOp.CheckDword(AxKey, "BlockPerUserInstall", 1)],
        },
        new TweakDef
        {
            Id = "axinst-disable-silent-install",
            Label = "Disable Silent ActiveX Control Installation",
            Category = "ActiveX Installer Service Policy",
            Description =
                "Sets DisableSilentInstall=1 in the AxInstaller policy key. "
                + "Prevents the ActiveX Installer Service from installing controls without displaying "
                + "a visible installation prompt or User Account Control elevation dialog. "
                + "Ensures all ActiveX activity is visible to the user or admin. "
                + "Default: absent (silent install possible). Recommended: 1 for user awareness.",
            Tags = ["activex", "silent", "uac", "prompt", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "ActiveX controls always install with a visible prompt; silent background installs blocked.",
            ApplyOps = [RegOp.SetDword(AxKey, "DisableSilentInstall", 1)],
            RemoveOps = [RegOp.DeleteValue(AxKey, "DisableSilentInstall")],
            DetectOps = [RegOp.CheckDword(AxKey, "DisableSilentInstall", 1)],
        },
        new TweakDef
        {
            Id = "axinst-restrict-download-cache",
            Label = "Restrict ActiveX Download Cache Size",
            Category = "ActiveX Installer Service Policy",
            Description =
                "Sets MaxCachedDownloadSize=0 in the AxInstaller policy key. "
                + "Limits the ActiveX Installer Service download cache to zero, preventing caching of "
                + "downloaded ActiveX control installer packages. Each install always re-downloads the package. "
                + "Prevents malicious packages from being cached and re-used across sessions. "
                + "Default: absent (default cache size). Recommended: 0 on high-security machines.",
            Tags = ["activex", "cache", "download", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "ActiveX installer download cache disabled; no packages cached on disk.",
            ApplyOps = [RegOp.SetDword(AxKey, "MaxCachedDownloadSize", 0)],
            RemoveOps = [RegOp.DeleteValue(AxKey, "MaxCachedDownloadSize")],
            DetectOps = [RegOp.CheckDword(AxKey, "MaxCachedDownloadSize", 0)],
        },
        new TweakDef
        {
            Id = "axinst-block-ocx-download",
            Label = "Block ActiveX OCX Download from Internet",
            Category = "ActiveX Installer Service Policy",
            Description =
                "Sets BlockOcxDownload=1 in the AxInstaller policy key. "
                + "Prevents the ActiveX Installer Service from downloading .ocx files (OLE Control eXtensions) "
                + "from any internet-based source, including trusted sites. "
                + "Forces all ActiveX control files to be sourced from local paths or intranet shares. "
                + "Default: absent (internet downloads allowed). Recommended: 1 in air-gapped or enterprise environments.",
            Tags = ["activex", "ocx", "download", "internet", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "ActiveX .ocx file downloads from the internet completely blocked by the installer service.",
            ApplyOps = [RegOp.SetDword(AxKey, "BlockOcxDownload", 1)],
            RemoveOps = [RegOp.DeleteValue(AxKey, "BlockOcxDownload")],
            DetectOps = [RegOp.CheckDword(AxKey, "BlockOcxDownload", 1)],
        },
    ];
}
