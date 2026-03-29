// RegiLattice.Core — Tweaks/WindowsInstallerAdvPolicy.cs
// Windows Installer (MSI) Advanced Security Policy — Sprint 628.
// Category: "Windows Installer Adv Policy" | Slug: winstadv
// Key: HKLM\SOFTWARE\Policies\Microsoft\Windows\Installer

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WindowsInstallerAdvPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Installer";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "winstadv-disable-always-install-elevated",
            Label = "Installer Adv: Disable AlwaysInstallElevated to Prevent MSI Privilege Escalation",
            Category = "Windows Installer Adv Policy",
            Description = "Sets AlwaysInstallElevated=0 in the machine-scope Windows Installer policy. The AlwaysInstallElevated policy (when set to 1 in BOTH HKLM and HKCU) allows standard users to run MSI installers with SYSTEM-level privileges. This is a critical privilege escalation vector: any malicious MSI file dropped by an attacker on a machine with both keys set to 1 will execute its custom actions with SYSTEM rights, enabling immediate privilege escalation to SYSTEM for any standard user. " +
                "AlwaysInstallElevated is one of the most well-known Windows privilege escalation misconfigurations — it is checked as a first-step by tools like PowerUp, BeRoot, and Sherlock. In penetration tests of enterprise environments, this setting is frequently found enabled on developer machines where IT set it to allow software installation without admin prompts. The fix is zero-cost: AlwaysInstallElevated=0 requires no user-visible workflow changes while eliminating a binary privilege escalation path for any attacker who can write a temporary MSI file.",
            Tags = ["winstadv", "msi", "alwaysinstallelevated", "privilege-escalation", "powerup", "system-priv"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "MSI privilege escalation via AlwaysInstallElevated disabled. Standard users cannot install in elevated context — no breakage if admin approval was already required.",
            ApplyOps = [RegOp.SetDword(Key, "AlwaysInstallElevated", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AlwaysInstallElevated")],
            DetectOps = [RegOp.CheckDword(Key, "AlwaysInstallElevated", 0)],
        },
        new TweakDef
        {
            Id = "winstadv-disable-user-control-over-install",
            Label = "Installer Adv: Remove User Control Over Windows Installer Behaviour",
            Category = "Windows Installer Adv Policy",
            Description = "Sets EnableUserControl=0 in Windows Installer policy. Prevents standard users from changing Windows Installer installation options — including installation location, install/remove component selection, and rollback behaviour — during a privileged MSI installation. Without this restriction, a carefully crafted UI sequence in a malicious MSI can be used to direct installation output to attacker-controlled paths during an elevated installer run. " +
                "The EnableUserControl=1 setting allows the Transform (MST) feature to accept user-provided transforms that modify the installer's property table. In a DLL planting scenario: an elevated MSI installer that copies DLLs to a system path writes them to a location specified as INSTALLDIR. If the user can influence INSTALLDIR (e.g., via a malicious transform), they can redirect DLL installation to a subfolder they control — and a privileged service that loads from that path will load the attacker's DLL. Disabling user control eliminates this transform injection vector.",
            Tags = ["winstadv", "msi", "user-control", "transform-injection", "dll-planting"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Standard users cannot modify Windows Installer options during elevated installations. Custom MST transforms under user control are rejected.",
            ApplyOps = [RegOp.SetDword(Key, "EnableUserControl", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableUserControl")],
            DetectOps = [RegOp.CheckDword(Key, "EnableUserControl", 0)],
        },
        new TweakDef
        {
            Id = "winstadv-disable-patch-installation",
            Label = "Installer Adv: Prevent Standard Users from Applying MSI Patches Directly",
            Category = "Windows Installer Adv Policy",
            Description = "Sets DisablePatch=1 in Windows Installer policy. Prevents standard users from applying MSP (Microsoft Patch) files directly to installed MSI applications without administrator approval. Standard user MSP application runs at the user's privilege level; however, if the base MSI was installed at elevated privilege and the patch modifies system-scope files, inconsistency between patch installation context and base package context can leave system files in a mixed state exploitable by privilege escalation. " +
                "Patch files (MSP) can replace arbitrary files within an installed product by referencing the original MSI's file table. An attacker who controls a well-crafted MSP for an installed enterprise MSI (e.g., a third-party vendor application) can use a modified patch to replace an application DLL with a malicious version. If the original MSI installed files to a system-protected path but the MSP runs at user privilege, the Windows Installer service's elevated write context (used for patching) can be manipulated to write attacker-controlled content to protected paths.",
            Tags = ["winstadv", "msi", "msp", "patch", "disable-patch", "dll-replacement"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Standard users cannot install MSP patches directly. IT-managed patching (WSUS, SCCM) unaffected — only manual MSP execution by standard users is blocked.",
            ApplyOps = [RegOp.SetDword(Key, "DisablePatch", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisablePatch")],
            DetectOps = [RegOp.CheckDword(Key, "DisablePatch", 1)],
        },
        new TweakDef
        {
            Id = "winstadv-prevent-rollback-cleanup",
            Label = "Installer Adv: Disable MSI Rollback to Prevent Temporary File Persistence",
            Category = "Windows Installer Adv Policy",
            Description = "Sets DisableRollback=1 in Windows Installer policy. Disables the Windows Installer rollback feature. Rollback creates temporary backup copies of files before overwriting them, enabling restoration if the installation fails. These rollback files (stored in %TEMP%\\MSI*) persist on disk during the installation and are deleted on success/failure. On machines with malware that monitors %TEMP% for incoming files, rollback temporary files can expose sensitive installation data. " +
                "The installer rollback mechanism creates temporary copies of existing files before overwriting them. These temporary rollback files contain the complete binary content of production files (DLLs, EXEs, config files, encryption keys bundled in MSI packages) and are world-readable during installation. A process running as a standard user at the same time as an elevated MSI installation can read the rollback files in %TEMP% and obtain sensitive data that the MSI package installs to protected locations. Disabling rollback eliminates these temporary file exposures at the cost of post-failure manual cleanup.",
            Tags = ["winstadv", "msi", "rollback", "temp-files", "sensitive-data"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            ImpactNote = "MSI rollback disabled. Failed installations require manual cleanup — MSIZMA files will not be automatically removed. Test on non-production before deploying widely.",
            ApplyOps = [RegOp.SetDword(Key, "DisableRollback", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableRollback")],
            DetectOps = [RegOp.CheckDword(Key, "DisableRollback", 1)],
        },
        new TweakDef
        {
            Id = "winstadv-disable-msi-internet-sources",
            Label = "Installer Adv: Disable MSI Package Installation from Internet Sources",
            Category = "Windows Installer Adv Policy",
            Description = "Sets DisableWebInstall=1 in Windows Installer policy. Prevents Windows Installer from downloading and installing MSI packages directly from internet URLs (http://, https://, ftp:// paths). Without this restriction, a shortcut or script can trigger an MSI download-and-install directly from an external web server. " +
                "Internet-sourced MSI installation is an attack vector in phishing campaigns: a click on a malicious email attachment or web link can trigger a Windows Installer URL handler that downloads and executes a malicious MSI from an attacker-controlled server. The MSI runs with the context of the logged-in user and can contain PowerShell/VBScript custom actions. Modern LOLBins-based attacks use MSI download-and-run as a code execution mechanism that bypasses application whitelisting. Blocking internet MSI sources forces all installations to originate from approved internal sources (SCCM, Intune, network shares). ",
            Tags = ["winstadv", "msi", "internet-install", "url-install", "phishing", "lolbins"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "MSI installation from internet URLs blocked. Enterprise deployment tools (SCCM, Intune, internal network shares) are unaffected.",
            ApplyOps = [RegOp.SetDword(Key, "DisableWebInstall", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableWebInstall")],
            DetectOps = [RegOp.CheckDword(Key, "DisableWebInstall", 1)],
        },
        new TweakDef
        {
            Id = "winstadv-enable-logging",
            Label = "Installer Adv: Enable Verbose MSI Logging for Installation Audit Trail",
            Category = "Windows Installer Adv Policy",
            Description = "Sets Logging='voicewarmupx' in Windows Installer policy (REG_SZ). Enables verbose Windows Installer logging to %TEMP%\\MSI*.log for all installations. Each character in the logging string controls a category: v=verbose, o=out-of-disk status, i=status messages, c=initial UI parameters, e=error messages, w=warning messages, a=action start, r=action-specific records, m=memory used, u=user requests, p=terminal properties, x=extra debug info. " +
                "Without installer logging, a failed or malicious MSI installation leaves no audit trail. When investigating a security incident where an attacker installed malware via MSI, the absence of installer logs makes forensic reconstruction of the installation impossible. MSI logs record every action taken during the installation — including all file copies, registry writes, custom action command lines, DLL invocations, and error codes. MSI logs are critical for SOC investigations of supply chain attacks, trojanised enterprise software, and malicious package installations.",
            Tags = ["winstadv", "msi", "logging", "audit", "forensics", "malware-investigation"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Verbose MSI logging enabled to %TEMP%\\MSI*.log. Log files accumulate over time; clean up log files periodically. No performance impact on installation itself.",
            ApplyOps = [RegOp.SetString(Key, "Logging", "voicewarmupx")],
            RemoveOps = [RegOp.DeleteValue(Key, "Logging")],
            DetectOps = [RegOp.CheckString(Key, "Logging", "voicewarmupx")],
        },
        new TweakDef
        {
            Id = "winstadv-disable-advertised-shortcuts",
            Label = "Installer Adv: Disable Advertised Shortcut Install-on-Demand to Prevent Elevation Abuse",
            Category = "Windows Installer Adv Policy",
            Description = "Sets DisableAdvertisedShortcuts=1 in Windows Installer policy. Disables the Windows Installer install-on-demand feature triggered by advertised shortcuts. Advertised shortcuts are MSI feature installation triggers — clicking an advertised shortcut to a feature that was not fully installed causes Windows Installer to complete the feature installation on demand, potentially with elevated privileges if the original product was installed elevated. " +
                "Install-on-demand via advertised shortcut is a privilege escalation vector: if an MSI product was installed with elevated privileges and an advertised shortcut triggers on-demand installation of a not-yet-installed component, the Windows Installer service performs the installation at elevated privilege on behalf of the user. An attacker who can manipulate an advertised shortcut (via shortcut write access to a shared profile directory) can point it at a malicious MSI component ID — causing the Installer service to execute attacker-controlled code at SYSTEM privilege.",
            Tags = ["winstadv", "msi", "advertised-shortcut", "install-on-demand", "privilege-escalation"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Install-on-demand via advertised shortcuts disabled. Some Office features (install-on-demand Office languages, click-to-run components) may require full pre-installation.",
            ApplyOps = [RegOp.SetDword(Key, "DisableAdvertisedShortcuts", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableAdvertisedShortcuts")],
            DetectOps = [RegOp.CheckDword(Key, "DisableAdvertisedShortcuts", 1)],
        },
        new TweakDef
        {
            Id = "winstadv-limit-system-restore-checkpoints",
            Label = "Installer Adv: Limit System Restore Checkpoint Creation During MSI Installs",
            Category = "Windows Installer Adv Policy",
            Description = "Sets LimitSystemRestoreCheckpointing=1 in Windows Installer policy. Prevents Windows Installer from creating a System Restore checkpoint for every MSI installation. By default, each MSI install creates a restore point — on machines that install many packages (e.g., during software deployment runs), this generates numerous restore points that consume significant disk space and slow down batch installations. " +
                "On a freshly provisioned endpoint receiving its full software stack (100+ MSI packages via SCCM/Intune), each installation creates an individual system restore point — resulting in 100 restore points consuming many gigabytes. Shadow storage fills up, causing earlier restore points to be deleted, rendering pre-provisioning baseline restore points inaccessible. For enterprise-managed endpoints, System Center Config Manager provides a superior rollback mechanism — MSI restore points on managed machines create overhead without providing actionable rollback capability.",
            Tags = ["winstadv", "msi", "system-restore", "checkpoint", "disk-space", "provisioning"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "System Restore checkpoints not created during MSI installations. Reduces restore point proliferation and disk usage during batch software deployment.",
            ApplyOps = [RegOp.SetDword(Key, "LimitSystemRestoreCheckpointing", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "LimitSystemRestoreCheckpointing")],
            DetectOps = [RegOp.CheckDword(Key, "LimitSystemRestoreCheckpointing", 1)],
        },
        new TweakDef
        {
            Id = "winstadv-disable-safe-mode-installs",
            Label = "Installer Adv: Block MSI Installation in Windows Safe Mode",
            Category = "Windows Installer Adv Policy",
            Description = "Sets SafeForScripting=1 in Windows Installer policy. Disables MSI installation in Windows Safe Mode. Windows Safe Mode loads a minimal driver set — security software (AV, EDR, application control) may not load in safe mode, leaving the endpoint unprotected. An attacker who can force a reboot into Safe Mode (e.g., using bcdedit, AutoRuns persistence on safeboot key, or physical access) gains an environment where Windows Installer would normally still function without security controls active. " +
                "Safe Mode abuse is a known attacker technique for bypassing endpoint security tools: ESET, Symantec, CrowdStrike, and other security agents do not load their kernel-mode components in Safe Mode. Ransomware variants (LockerGoga, SunCrypt, REvil) have been observed forcing reboots into Safe Mode before executing their payload precisely to bypass endpoint security. Blocking MSI installation in Safe Mode prevents this technique from deploying additional malware payloads during the unprotected Safe Mode boot window.",
            Tags = ["winstadv", "msi", "safe-mode", "edralert bypass", "ransomware", "safeboot"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "MSI installation blocked in Windows Safe Mode. Legitimate safe mode maintenance that requires MSI installation must be performed from normal mode.",
            ApplyOps = [RegOp.SetDword(Key, "SafeForScripting", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "SafeForScripting")],
            DetectOps = [RegOp.CheckDword(Key, "SafeForScripting", 1)],
        },
        new TweakDef
        {
            Id = "winstadv-disable-msi-in-locked-session",
            Label = "Installer Adv: Block Elevated MSI Installs When Session is Locked",
            Category = "Windows Installer Adv Policy",
            Description = "Sets DisableLockdownInstall=1 in Windows Installer policy. Prevents elevation of Windows Installer packages when the user desktop is locked. Without this restriction, a standard user can trigger an elevated MSI installation (via RunAs or Invoke-Item) for a package that has a UI sequence, then lock their desktop — the Installer continues processing and a crafted DLL extraction step can write to system locations while the desktop is locked and unmonitored. " +
                "Locked desktop MSI exploitation requires a multi-step attack: (1) trigger an elevated MSI with a crafted UI sequence, (2) lock the desktop before the custom action phase, (3) the custom action executes at SYSTEM during the locked desktop window delivering attacker payloads. This works because Windows Installer continues installation even while the session is locked (installation UI is suppressed but custom actions continue). DisableLockdownInstall=1 aborts any pending elevated installation when the desktop is locked.",
            Tags = ["winstadv", "msi", "locked-session", "custom-action", "elevation-control"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Elevated MSI installations aborted when session locked. Users installing software must keep desktop unlocked until completion.",
            ApplyOps = [RegOp.SetDword(Key, "DisableLockdownInstall", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableLockdownInstall")],
            DetectOps = [RegOp.CheckDword(Key, "DisableLockdownInstall", 1)],
        },
    ];
}
