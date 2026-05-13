namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

[TweakModule]
internal static class SystemOptimization
{
    private const string MemMgmt = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management";
    private const string FileSystem = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem";
    private const string SessionMgr = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager";
    private const string PriorityCtrl = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl";
    private const string CrashCtrl = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl";
    private const string Lsa = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa";
    private const string Power = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power";
    private const string Kernel = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile";
    private const string WinLogon = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon";
    private const string WinErr = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting";
    private const string Explorer = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        // ── Memory Management ────────────────────────────────────────────

        // ── File System ──────────────────────────────────────────────────

        // ── Process Priority ─────────────────────────────────────────────

        // ── Multimedia / Gaming Scheduling ───────────────────────────────

        // ── Crash & Error Handling ───────────────────────────────────────

        // ── Boot & Logon ─────────────────────────────────────────────────

        new TweakDef
        {
            Id = "sysopt-auto-logon-last-user",
            Label = "Auto-Logon Last User (Skip Lock Screen)",
            Category = "System 2",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Automatically logs in the last user at boot, skipping the lock screen (not for shared PCs).",
            Tags = ["optimization", "logon", "auto", "boot"],
            RegistryKeys = [WinLogon],
            ApplyOps = [RegOp.SetDword(WinLogon, "AutoRestartShell", 1)],
            RemoveOps = [RegOp.SetDword(WinLogon, "AutoRestartShell", 0)],
            DetectOps = [RegOp.CheckDword(WinLogon, "AutoRestartShell", 1)],
        },
        // ── Security & LSA ───────────────────────────────────────────────

        // ── Power & Energy ───────────────────────────────────────────────

        // ── Visual Effects Minimal ───────────────────────────────────────

        // ── Misc Performance ─────────────────────────────────────────────

        // ── Network Buffer Tuning ────────────────────────────────────────

        // ── UI Responsiveness ────────────────────────────────────────────
    ];
}
