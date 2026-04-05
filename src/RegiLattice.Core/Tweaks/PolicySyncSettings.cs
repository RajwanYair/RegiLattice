namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

/// <summary>
/// Sprint 656 — Windows Settings Sync policies (10 tweaks).
/// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\SettingSync
/// Disables roaming sync of various Windows settings categories via
/// Group Policy. Suitable for environments where settings should remain
/// machine-local and not roam through Microsoft accounts or Azure AD.
/// </summary>
internal static class PolicySyncSettings
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SettingSync";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } = [];
}
