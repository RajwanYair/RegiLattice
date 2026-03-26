// RegiLattice.Core — Tweaks/SudoWindowsPolicy.cs
// Sudo for Windows Group Policy controls — Sprint 368.
// Category: "Sudo for Windows Policy" | Slug: sudopol
// Registry path: HKLM\SOFTWARE\Policies\Microsoft\Windows\Sudo
// MinBuild: 22631 (Windows 11 23H2+)
namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class SudoWindowsPolicy
{
    private const string SudoKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Sudo";
    private const string ElevationConfigKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ElevationConfig";
    private const string UacPoliciesKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\UAC";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "sudopol-disable-sudo",
            Label = "Disable Sudo for Windows",
            Category = "Sudo for Windows Policy",
            Description = "Prevents users from using the 'sudo' command in Windows to run programs with elevated privileges from a standard terminal. Enforces traditional UAC elevation only.",
            Tags = ["sudo", "elevation", "uac", "security", "windows-11"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22631,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Prevents privilege escalation via sudo from standard terminals; users must use dedicated elevated sessions.",
            RegistryKeys = [SudoKey],
            ApplyOps  = [RegOp.SetDword(SudoKey, "Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(SudoKey, "Enabled")],
            DetectOps = [RegOp.CheckDword(SudoKey, "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "sudopol-force-new-window",
            Label = "Force Sudo to Open New Window",
            Category = "Sudo for Windows Policy",
            Description = "When sudo is permitted, forces elevated processes to launch in a new, separate console window rather than running inline in the calling shell. Increases visibility of elevated sessions.",
            Tags = ["sudo", "elevation", "new-window", "uac", "windows-11"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22631,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Elevated process runs in a clearly separate window, reducing confusion about which shell context is privileged.",
            RegistryKeys = [SudoKey],
            ApplyOps  = [RegOp.SetDword(SudoKey, "SudoMode", 0)],
            RemoveOps = [RegOp.DeleteValue(SudoKey, "SudoMode")],
            DetectOps = [RegOp.CheckDword(SudoKey, "SudoMode", 0)],
        },
        new TweakDef
        {
            Id = "sudopol-disable-inline-mode",
            Label = "Disable Sudo Inline Execution Mode",
            Category = "Sudo for Windows Policy",
            Description = "Prevents the 'inline' execution mode of sudo where the elevated process shares the calling terminal session. Inline mode can mask privilege escalation; this policy requires isolated execution.",
            Tags = ["sudo", "elevation", "inline-mode", "security", "windows-11"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22631,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Inline sudo blurs the boundary between privileged and non-privileged sessions; disabling it is recommended for corporate environments.",
            RegistryKeys = [SudoKey],
            ApplyOps  = [RegOp.SetDword(SudoKey, "AllowInlineMode", 0)],
            RemoveOps = [RegOp.DeleteValue(SudoKey, "AllowInlineMode")],
            DetectOps = [RegOp.CheckDword(SudoKey, "AllowInlineMode", 0)],
        },
        new TweakDef
        {
            Id = "sudopol-disable-input-disabled-mode",
            Label = "Disable Sudo Input-Disabled Mode",
            Category = "Sudo for Windows Policy",
            Description = "Prevents the 'input disabled' mode of sudo, which runs an elevated process with stdin closed. This mode is useful for non-interactive elevated scripts but may bypass certain security controls.",
            Tags = ["sudo", "elevation", "input-disabled", "security", "windows-11"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22631,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Prevents automated elevated scripts from running silently via sudo in environments where operator oversight is required.",
            RegistryKeys = [SudoKey],
            ApplyOps  = [RegOp.SetDword(SudoKey, "AllowInputDisabledMode", 0)],
            RemoveOps = [RegOp.DeleteValue(SudoKey, "AllowInputDisabledMode")],
            DetectOps = [RegOp.CheckDword(SudoKey, "AllowInputDisabledMode", 0)],
        },
        new TweakDef
        {
            Id = "sudopol-require-admin-group-membership",
            Label = "Restrict Sudo to Local Administrators Group",
            Category = "Sudo for Windows Policy",
            Description = "Ensures that only users who are members of the local Administrators group can use sudo for Windows, preventing standard users from attempting elevation via sudo.",
            Tags = ["sudo", "elevation", "admin-group", "access-control", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22631,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Provides an explicit access gate: even if sudo is enabled on the device, non-admin users receive a denial.",
            RegistryKeys = [SudoKey],
            ApplyOps  = [RegOp.SetDword(SudoKey, "RequireAdminGroupMembership", 1)],
            RemoveOps = [RegOp.DeleteValue(SudoKey, "RequireAdminGroupMembership")],
            DetectOps = [RegOp.CheckDword(SudoKey, "RequireAdminGroupMembership", 1)],
        },
        new TweakDef
        {
            Id = "sudopol-enable-audit-events",
            Label = "Enable Sudo Elevation Audit Events",
            Category = "Sudo for Windows Policy",
            Description = "Configures Windows to write an audit log entry whenever a process is elevated via sudo. Audit entries include the calling user, the target executable, and the elevation timestamp.",
            Tags = ["sudo", "elevation", "audit", "compliance", "event-log"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22631,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Produces an accountable record of every sudo elevation, supporting incident response and SOC monitoring.",
            RegistryKeys = [SudoKey],
            ApplyOps  = [RegOp.SetDword(SudoKey, "EnableAuditEvents", 1)],
            RemoveOps = [RegOp.DeleteValue(SudoKey, "EnableAuditEvents")],
            DetectOps = [RegOp.CheckDword(SudoKey, "EnableAuditEvents", 1)],
        },
        new TweakDef
        {
            Id = "sudopol-block-network-elevated-processes",
            Label = "Block Elevated Processes from Accessing Network",
            Category = "Sudo for Windows Policy",
            Description = "Restricts network access for processes elevated via sudo, preventing elevated shells from making outbound network connections. Limits lateral movement potential from elevated contexts.",
            Tags = ["sudo", "elevation", "network", "security", "hardening"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22631,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "An elevated process with network access can pivot to other systems; this policy reduces the blast radius of a compromised sudo-elevated session.",
            RegistryKeys = [ElevationConfigKey],
            ApplyOps  = [RegOp.SetDword(ElevationConfigKey, "BlockNetworkFromElevatedProcesses", 1)],
            RemoveOps = [RegOp.DeleteValue(ElevationConfigKey, "BlockNetworkFromElevatedProcesses")],
            DetectOps = [RegOp.CheckDword(ElevationConfigKey, "BlockNetworkFromElevatedProcesses", 1)],
        },
        new TweakDef
        {
            Id = "sudopol-enforce-credential-prompt",
            Label = "Always Prompt for Credentials on Sudo Elevation",
            Category = "Sudo for Windows Policy",
            Description = "Requires the user to enter explicit credentials (password or Windows Hello) before each sudo elevation, even within an existing authenticated session. Prevents silent re-elevation.",
            Tags = ["sudo", "elevation", "credential-prompt", "uac", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22631,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Satisfies 'explicit approval' audit requirements by ensuring the user actively authenticates for each elevated action.",
            RegistryKeys = [ElevationConfigKey],
            ApplyOps  = [RegOp.SetDword(ElevationConfigKey, "AlwaysPromptCredentialsOnElevation", 1)],
            RemoveOps = [RegOp.DeleteValue(ElevationConfigKey, "AlwaysPromptCredentialsOnElevation")],
            DetectOps = [RegOp.CheckDword(ElevationConfigKey, "AlwaysPromptCredentialsOnElevation", 1)],
        },
        new TweakDef
        {
            Id = "sudopol-log-elevated-command-line",
            Label = "Log Command-Line Arguments for Sudo-Elevated Processes",
            Category = "Sudo for Windows Policy",
            Description = "Enables command-line logging for all processes elevated through sudo, recording the full command line in the Windows event log. Aids forensic investigation of elevation abuse.",
            Tags = ["sudo", "elevation", "command-line", "audit", "forensics"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22631,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Command-line data in event logs reveals what was actually run with elevated privileges, not just that elevation occurred.",
            RegistryKeys = [ElevationConfigKey],
            ApplyOps  = [RegOp.SetDword(ElevationConfigKey, "LogElevatedCommandLine", 1)],
            RemoveOps = [RegOp.DeleteValue(ElevationConfigKey, "LogElevatedCommandLine")],
            DetectOps = [RegOp.CheckDword(ElevationConfigKey, "LogElevatedCommandLine", 1)],
        },
        new TweakDef
        {
            Id = "sudopol-block-unapproved-shells",
            Label = "Block Sudo Elevation from Unapproved Shell Hosts",
            Category = "Sudo for Windows Policy",
            Description = "Restricts sudo elevation to approved shell host executables (Windows Terminal, PowerShell 7, cmd.exe). Prevents elevation from unusual hosts such as scripting engines or third-party terminals.",
            Tags = ["sudo", "elevation", "shell", "allowlist", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22631,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Reduces attack surface by ensuring only known-good terminal applications can initiate sudo elevation requests.",
            RegistryKeys = [UacPoliciesKey],
            ApplyOps  = [RegOp.SetDword(UacPoliciesKey, "RestrictSudoToApprovedHosts", 1)],
            RemoveOps = [RegOp.DeleteValue(UacPoliciesKey, "RestrictSudoToApprovedHosts")],
            DetectOps = [RegOp.CheckDword(UacPoliciesKey, "RestrictSudoToApprovedHosts", 1)],
        },
    ];
}
