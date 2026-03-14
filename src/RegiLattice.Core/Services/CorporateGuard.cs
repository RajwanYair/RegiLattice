// RegiLattice.Core — Services/CorporateGuard.cs
// Corporate environment detection using .NET APIs (registry, WMI, P/Invoke).

using System.Management;
using System.Runtime.InteropServices;
using System.Text;

namespace RegiLattice.Core;

/// <summary>Detects corporate/managed environments to block unsafe tweaks.</summary>
public static class CorporateGuard
{
    private static bool? _cached;

    /// <summary>Returns true if this machine is in a corporate/managed environment.</summary>
    public static bool IsCorporateNetwork()
    {
        if (_cached.HasValue)
            return _cached.Value;
        _cached = IsDomainJoined() || IsAzureAdJoined() || HasGroupPolicy() || HasManagementAgent();
        return _cached.Value;
    }

    public static void ClearCache() => _cached = null;

    /// <summary>Detailed status for UI display.</summary>
    public static (bool IsCorporate, string Reason) Status()
    {
        if (IsDomainJoined())
            return (true, "Active Directory domain member");
        if (IsAzureAdJoined())
            return (true, "Azure AD / Entra ID joined");
        if (HasGroupPolicy())
            return (true, "Group Policy indicators found");
        if (HasManagementAgent())
            return (true, "SCCM / Intune enrollment detected");
        return (false, "Not a managed environment");
    }

    /// <summary>Check if specific registry keys have GPO overlay.</summary>
    public static bool IsGpoManaged(IReadOnlyList<string> registryKeys)
    {
        foreach (var key in registryKeys)
        {
            if (key.Contains(@"\Policies\", StringComparison.OrdinalIgnoreCase))
                return true;
            // Check if a corresponding Policies key exists
            var policyKey = key.Replace(@"SOFTWARE\Microsoft", @"SOFTWARE\Policies\Microsoft", StringComparison.OrdinalIgnoreCase);
            if (policyKey != key && Registry.RegistrySession.ParsePath(policyKey) is var (root, sub))
            {
                using var k = root.OpenSubKey(sub);
                if (k is not null)
                    return true;
            }
        }
        return false;
    }

    // ── Private detectors ───────────────────────────────────────────────

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern bool GetComputerNameExW(int nameType, StringBuilder buffer, ref uint size);

    private static bool IsDomainJoined()
    {
        try
        {
            uint size = 256;
            var buffer = new StringBuilder((int)size);
            // NameType 2 = ComputerNameDnsDomain
            if (GetComputerNameExW(2, buffer, ref size) && buffer.Length > 0)
                return true;
        }
        catch { }

        // Fallback: check registry for domain membership
        try
        {
            using var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\Tcpip\Parameters");
            var domain = key?.GetValue("Domain") as string;
            return !string.IsNullOrWhiteSpace(domain);
        }
        catch
        {
            return false;
        }
    }

    private static bool IsAzureAdJoined()
    {
        // Check Entra ID (AAD) join status via registry
        try
        {
            using var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\CloudDomainJoin\JoinInfo");
            if (key is null)
                return false;
            return key.GetSubKeyNames().Length > 0;
        }
        catch
        {
            return false;
        }
    }

    private static bool HasGroupPolicy()
    {
        // Check for Group Policy registry indicators
        string[] gpoKeys = [@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies", @"SOFTWARE\Policies\Microsoft\Windows"];

        foreach (var path in gpoKeys)
        {
            try
            {
                using var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(path);
                if (key is not null)
                {
                    var subKeys = key.GetSubKeyNames();
                    if (subKeys.Length > 5)
                        return true; // Non-default GPO presence
                }
            }
            catch { }
        }
        return false;
    }

    private static bool HasManagementAgent()
    {
        // Check for SCCM client
        try
        {
            using var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\CCM");
            if (key is not null)
                return true;
        }
        catch { }

        // Check for Intune MDM enrollment
        try
        {
            using var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Enrollments");
            if (key is not null && key.GetSubKeyNames().Length > 0)
                return true;
        }
        catch { }

        return false;
    }
}
