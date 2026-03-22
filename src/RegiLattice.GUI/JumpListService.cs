#nullable enable

// Windows.UI.Shell.JumpList is a WinRT API that is NOT projected to desktop WinForms
// apps unless the app is MSIX-packaged with a registered AUMID.  For unpackaged WinForms
// apps the correct approach is to call ICustomDestinationList via COM, which requires
// additional COM interop scaffolding beyond the current sprint scope.
//
// This service is therefore a graceful no-op scaffold:
// — It compiles, has no runtime side-effects, and provides the hook for future wiring.
// — Full COM implementation is tracked as a future Phase-D work item.

namespace RegiLattice.GUI;

/// <summary>
/// Manages the Windows 11 / Windows 10 taskbar Jump List for RegiLattice.
/// <para>
/// For unpackaged WinForms apps the WinRT <c>Windows.UI.Shell.JumpList</c> API requires
/// an AUMID provided by an MSIX package.  This scaffold exposes the correct async contract
/// and will be filled in once the installer registers the AUMID.  Until then all calls
/// silently no-op.
/// </para>
/// </summary>
public static class JumpListService
{
    /// <summary>
    /// Asynchronously updates the taskbar Jump List.
    /// Currently a no-op until the app is installed via the MSIX / WiX package
    /// that registers a Start-menu shortcut with an AppUserModelID.
    /// </summary>
    public static Task UpdateAsync() => Task.CompletedTask;
}
