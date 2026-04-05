namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

/// <summary>
/// Sprint 654 — Windows CloudContent / User Experience policies (10 tweaks).
/// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\CloudContent
/// These Group Policy keys disable Microsoft promotional content,
/// Spotlight features, lock screen suggestions, and tailored experiences
/// that are delivered via cloud services.
/// </summary>
/// <summary>
/// Sprint 655 — Windows Event Log sizing and access policies (10 tweaks).
/// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\EventLog\{Application|Security|System|Setup}
/// These Group Policy keys configure maximum event log sizes and guest access
/// restrictions for the four primary Windows event log channels.
/// </summary>
internal static class PolicyEventLogAudit
{
    private const string AppLog = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\Application";
    private const string SecLog = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\Security";
    private const string SysLog = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\System";
    private const string SetupLog = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\Setup";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } = [];
}
