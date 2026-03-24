// RegiLattice.Core — Tweaks/WinRmRemoteShellPolicy.cs
// WinRM Remote Shell Quota & Access Group Policy — Sprint 196.
// Controls remote shell access, per-user/per-session quotas, idle timeout,
// run time limits, process/memory limits, and environment variable access.
// Category: "WinRM Shell Policy" | Slug: rshpol
// Registry path: HKLM\SOFTWARE\Policies\Microsoft\Windows\WinRM\Service\RemoteShell

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WinRmRemoteShellPolicy
{
    private const string ShellKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinRM\Service\RemoteShell";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "rshpol-disable-remote-shell",
                Label = "Disable WinRM Remote Shell Access",
                Category = "WinRM Shell Policy",
                Description =
                    "Sets AllowRemoteShellAccess=0 to disable remote shell access over WinRM entirely. Blocks winrs.exe connections and PowerShell remoting sessions initiated from remote machines, limiting interactive shell exposure.",
                Tags = ["winrm", "remote-shell", "access", "policy", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "Blocks winrs and PowerShell remoting shells; WSMAN API and Invoke-Command may also fail.",
                ApplyOps = [RegOp.SetDword(ShellKey, "AllowRemoteShellAccess", 0)],
                RemoveOps = [RegOp.DeleteValue(ShellKey, "AllowRemoteShellAccess")],
                DetectOps = [RegOp.CheckDword(ShellKey, "AllowRemoteShellAccess", 0)],
            },
            new TweakDef
            {
                Id = "rshpol-limit-shells-per-user",
                Label = "Limit WinRM Shells per User",
                Category = "WinRM Shell Policy",
                Description =
                    "Sets MaxShellsPerUser=2 in the RemoteShell policy. Caps the number of concurrent WinRM remote shells a single user can open, mitigating resource exhaustion from shell flooding attacks.",
                Tags = ["winrm", "shell", "quota", "limit", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Limits remote shells to 2 per user; legitimate automation may need a higher value.",
                ApplyOps = [RegOp.SetDword(ShellKey, "MaxShellsPerUser", 2)],
                RemoveOps = [RegOp.DeleteValue(ShellKey, "MaxShellsPerUser")],
                DetectOps = [RegOp.CheckDword(ShellKey, "MaxShellsPerUser", 2)],
            },
            new TweakDef
            {
                Id = "rshpol-limit-concurrent-users",
                Label = "Limit Concurrent WinRM Shell Users",
                Category = "WinRM Shell Policy",
                Description =
                    "Sets MaxConcurrentUsers=5 in the RemoteShell policy. Restricts the total number of users who can run simultaneous WinRM remote shells on this endpoint, bounding server load from large-scale remoting campaigns.",
                Tags = ["winrm", "concurrent", "users", "quota", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Limits to 5 concurrent remote shell users; increase for servers used by larger teams.",
                ApplyOps = [RegOp.SetDword(ShellKey, "MaxConcurrentUsers", 5)],
                RemoveOps = [RegOp.DeleteValue(ShellKey, "MaxConcurrentUsers")],
                DetectOps = [RegOp.CheckDword(ShellKey, "MaxConcurrentUsers", 5)],
            },
            new TweakDef
            {
                Id = "rshpol-set-idle-timeout",
                Label = "Set WinRM Shell Idle Timeout (1 min)",
                Category = "WinRM Shell Policy",
                Description =
                    "Sets IdleTimeoutms=60000 (1 minute) in the RemoteShell policy. Disconnects remote shell sessions that have been idle beyond the threshold, reclaiming server resources and reducing the attack window of orphaned sessions.",
                Tags = ["winrm", "timeout", "idle", "session", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Terminates shells idle for >1 min; increase to 300000ms (5 min) if admins need longer pauses.",
                ApplyOps = [RegOp.SetDword(ShellKey, "IdleTimeoutms", 60000)],
                RemoveOps = [RegOp.DeleteValue(ShellKey, "IdleTimeoutms")],
                DetectOps = [RegOp.CheckDword(ShellKey, "IdleTimeoutms", 60000)],
            },
            new TweakDef
            {
                Id = "rshpol-set-shell-run-time",
                Label = "Limit WinRM Shell Maximum Run Time",
                Category = "WinRM Shell Policy",
                Description =
                    "Sets MaxShellRunTime=900000 (15 minutes) in the RemoteShell policy. Forces termination of remote shells running longer than the threshold, preventing long-running background processes from persisting through WinRM sessions.",
                Tags = ["winrm", "runtime", "timeout", "shell", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Kills shells after 15 min; long-running admin scripts may be interrupted.",
                ApplyOps = [RegOp.SetDword(ShellKey, "MaxShellRunTime", 900000)],
                RemoveOps = [RegOp.DeleteValue(ShellKey, "MaxShellRunTime")],
                DetectOps = [RegOp.CheckDword(ShellKey, "MaxShellRunTime", 900000)],
            },
            new TweakDef
            {
                Id = "rshpol-limit-processes-per-shell",
                Label = "Limit WinRM Processes per Shell",
                Category = "WinRM Shell Policy",
                Description =
                    "Sets MaxProcessesPerShell=5 in the RemoteShell policy. Caps the number of child processes a single WinRM shell session can spawn, limiting post-exploitation process trees from remote shell access.",
                Tags = ["winrm", "processes", "quota", "limit", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Limits remote shell to 5 child processes; complex scripts spawning many processes may fail.",
                ApplyOps = [RegOp.SetDword(ShellKey, "MaxProcessesPerShell", 5)],
                RemoveOps = [RegOp.DeleteValue(ShellKey, "MaxProcessesPerShell")],
                DetectOps = [RegOp.CheckDword(ShellKey, "MaxProcessesPerShell", 5)],
            },
            new TweakDef
            {
                Id = "rshpol-limit-memory-per-shell",
                Label = "Limit WinRM Shell Memory (150 MB)",
                Category = "WinRM Shell Policy",
                Description =
                    "Sets MaxMemoryPerShellMB=150 in the RemoteShell policy. Restricts total RAM available to a single WinRM remote shell session, preventing memory exhaustion attacks from intensive remote operations.",
                Tags = ["winrm", "memory", "quota", "limit", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Shell terminated when it uses >150 MB; increase for data-intensive remote admin tasks.",
                ApplyOps = [RegOp.SetDword(ShellKey, "MaxMemoryPerShellMB", 150)],
                RemoveOps = [RegOp.DeleteValue(ShellKey, "MaxMemoryPerShellMB")],
                DetectOps = [RegOp.CheckDword(ShellKey, "MaxMemoryPerShellMB", 150)],
            },
            new TweakDef
            {
                Id = "rshpol-block-env-variables",
                Label = "Block Environment Variable Modification in WinRM Shells",
                Category = "WinRM Shell Policy",
                Description =
                    "Sets AllowEnvironmentVariables=0 in the RemoteShell policy. Prevents remote WinRM shells from setting or overriding environment variables, reducing the risk of PATH hijacking or credential injection via env variable manipulation.",
                Tags = ["winrm", "environment", "variables", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Blocks env variable changes in remote shells; scripts relying on custom env vars may fail.",
                ApplyOps = [RegOp.SetDword(ShellKey, "AllowEnvironmentVariables", 0)],
                RemoveOps = [RegOp.DeleteValue(ShellKey, "AllowEnvironmentVariables")],
                DetectOps = [RegOp.CheckDword(ShellKey, "AllowEnvironmentVariables", 0)],
            },
            new TweakDef
            {
                Id = "rshpol-block-interactive-shell",
                Label = "Block Interactive WinRM Shell Sessions",
                Category = "WinRM Shell Policy",
                Description =
                    "Sets AllowInteractiveShell=0 in the RemoteShell policy. Blocks the creation of interactive WinRM shell sessions (winrs -r:<server> cmd) while still allowing non-interactive command execution, limiting attacker-controlled shell access.",
                Tags = ["winrm", "interactive", "shell", "access", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Blocks interactive winrs shells; non-interactive Invoke-Command sessions are unaffected.",
                ApplyOps = [RegOp.SetDword(ShellKey, "AllowInteractiveShell", 0)],
                RemoveOps = [RegOp.DeleteValue(ShellKey, "AllowInteractiveShell")],
                DetectOps = [RegOp.CheckDword(ShellKey, "AllowInteractiveShell", 0)],
            },
            new TweakDef
            {
                Id = "rshpol-disable-remote-shell-inbound",
                Label = "Disable WinRM Inbound Remote Shell",
                Category = "WinRM Shell Policy",
                Description =
                    "Sets AllowRemoteShellInbound=0 in the RemoteShell policy. Prevents this machine from accepting inbound WinRM remote shell connections while still permitting outbound WinRM sessions to remote targets.",
                Tags = ["winrm", "inbound", "shell", "hardening", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Blocks this machine as a WinRM target; it can still initiate outbound WinRM connections.",
                ApplyOps = [RegOp.SetDword(ShellKey, "AllowRemoteShellInbound", 0)],
                RemoveOps = [RegOp.DeleteValue(ShellKey, "AllowRemoteShellInbound")],
                DetectOps = [RegOp.CheckDword(ShellKey, "AllowRemoteShellInbound", 0)],
            },
        ];
}
