namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

/// <summary>
/// Sprint 653 — App privacy access via Group Policy (10 tweaks).
/// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy
/// Controls which app capabilities (camera, microphone, contacts, etc.)
/// apps may access on the machine. LetApps* = 2 forces access off for all apps.
/// </summary>
internal static class PolicyAppPrivacy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } = [];
}
