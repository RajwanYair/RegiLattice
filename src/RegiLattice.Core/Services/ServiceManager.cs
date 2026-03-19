#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace RegiLattice.Core.Services;

/// <summary>Simplified snapshot of a Windows service.</summary>
public sealed record ServiceEntry(
    string ServiceName,
    string DisplayName,
    string Description,
    ServiceControllerStatus Status,
    ServiceStartMode StartType,
    bool CanStop,
    bool CanPauseAndContinue
);

/// <summary>
/// Enumerates and controls Windows services via <see cref="ServiceController"/>.
/// Start / Stop operations are performed asynchronously with a configurable timeout.
/// Changing the start type requires admin rights and uses the SC.exe CLI.
/// </summary>
public static class ServiceManager
{
    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);

    // ── Query ────────────────────────────────────────────────────────────────

    /// <summary>Returns all installed services with their current status.</summary>
    public static IReadOnlyList<ServiceEntry> GetAllServices()
    {
        ServiceController[] controllers = ServiceController.GetServices();
        var result = new List<ServiceEntry>(controllers.Length);

        foreach (ServiceController sc in controllers)
        {
            try
            {
                result.Add(ToEntry(sc));
            }
            catch (Exception ex) when (ex is InvalidOperationException or System.ComponentModel.Win32Exception)
            {
                // Service may have been removed between enumeration and query
            }
            finally
            {
                sc.Dispose();
            }
        }

        return result.OrderBy(s => s.DisplayName, StringComparer.OrdinalIgnoreCase).ToList().AsReadOnly();
    }

    /// <summary>Returns a refreshed snapshot for a single service.</summary>
    public static ServiceEntry? GetService(string serviceName)
    {
        try
        {
            using var sc = new ServiceController(serviceName);
            sc.Refresh();
            return ToEntry(sc);
        }
        catch (Exception ex) when (ex is InvalidOperationException or System.ComponentModel.Win32Exception)
        {
            return null;
        }
    }

    // ── Control ──────────────────────────────────────────────────────────────

    /// <summary>
    /// Starts the named service. Does nothing if already running.
    /// </summary>
    public static async Task StartAsync(string serviceName, CancellationToken ct = default)
    {
        using var sc = new ServiceController(serviceName);
        if (sc.Status == ServiceControllerStatus.Running)
            return;

        sc.Start();
        await WaitForStatusAsync(sc, ServiceControllerStatus.Running, ct);
    }

    /// <summary>
    /// Stops the named service. Does nothing if already stopped.
    /// </summary>
    public static async Task StopAsync(string serviceName, CancellationToken ct = default)
    {
        using var sc = new ServiceController(serviceName);
        if (sc.Status is ServiceControllerStatus.Stopped or ServiceControllerStatus.StopPending)
            return;

        if (!sc.CanStop)
            throw new InvalidOperationException($"Service '{serviceName}' cannot be stopped.");

        sc.Stop();
        await WaitForStatusAsync(sc, ServiceControllerStatus.Stopped, ct);
    }

    /// <summary>
    /// Changes the startup type of a service via <c>sc.exe config</c>.
    /// Requires administrator rights.
    /// </summary>
    public static async Task SetStartTypeAsync(string serviceName, ServiceStartMode mode, bool dryRun = false, CancellationToken ct = default)
    {
        string startParam = mode switch
        {
            ServiceStartMode.Automatic => "auto",
            ServiceStartMode.Manual => "demand",
            ServiceStartMode.Disabled => "disabled",
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null),
        };

        if (dryRun)
            return;

        var (exitCode, _, stderr) = await ShellRunner.RunAsync("sc.exe", ["config", serviceName, "start=", startParam], ct);

        if (exitCode != 0)
            throw new InvalidOperationException($"sc.exe config failed for '{serviceName}' (exit {exitCode}): {stderr}");
    }

    // ── Sprint 47 enhancements ────────────────────────────────────────────────

    /// <summary>
    /// Returns the names of all services that depend on <paramref name="serviceName"/>
    /// (i.e. services that would stop if the specified service is stopped).
    /// </summary>
    public static IReadOnlyList<string> GetDependentServices(string serviceName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(serviceName);
        try
        {
            using var sc = new ServiceController(serviceName);
            return sc.DependentServices.Select(d => d.ServiceName).OrderBy(n => n, StringComparer.OrdinalIgnoreCase).ToList().AsReadOnly();
        }
        catch (Exception ex) when (ex is InvalidOperationException or System.ComponentModel.Win32Exception)
        {
            return [];
        }
    }

    /// <summary>
    /// Exports the current service list to a CSV file at <paramref name="filePath"/>.
    /// Columns: ServiceName, DisplayName, Status, StartType, CanStop.
    /// </summary>
    public static async Task ExportToCsvAsync(string filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
        var services = GetAllServices();
        var lines = new List<string>(services.Count + 1) { "ServiceName,DisplayName,Status,StartType,CanStop" };
        foreach (var s in services)
        {
            static string safe(string v) => v.Contains(',') || v.Contains('"') ? $"\"{v.Replace("\"", "\"\"")}\"" : v;
            lines.Add($"{safe(s.ServiceName)},{safe(s.DisplayName)},{s.Status},{s.StartType},{s.CanStop}");
        }
        Directory.CreateDirectory(Path.GetDirectoryName(filePath) ?? ".");
        await File.WriteAllLinesAsync(filePath, lines).ConfigureAwait(false);
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    private static async Task WaitForStatusAsync(ServiceController sc, ServiceControllerStatus desired, CancellationToken ct)
    {
        var deadline = DateTime.UtcNow + DefaultTimeout;
        while (sc.Status != desired)
        {
            ct.ThrowIfCancellationRequested();
            if (DateTime.UtcNow >= deadline)
                throw new System.TimeoutException(
                    $"Service '{sc.ServiceName}' did not reach status '{desired}' within {DefaultTimeout.TotalSeconds}s."
                );

            await Task.Delay(500, ct);
            sc.Refresh();
        }
    }

    private static ServiceEntry ToEntry(ServiceController sc)
    {
        string description = GetDescription(sc.ServiceName);
        return new ServiceEntry(
            ServiceName: sc.ServiceName,
            DisplayName: sc.DisplayName,
            Description: description,
            Status: sc.Status,
            StartType: sc.StartType,
            CanStop: sc.CanStop,
            CanPauseAndContinue: sc.CanPauseAndContinue
        );
    }

    private static string GetDescription(string serviceName)
    {
        try
        {
            using var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey($@"SYSTEM\CurrentControlSet\Services\{serviceName}");
            return key?.GetValue("Description")?.ToString() ?? string.Empty;
        }
        catch
        {
            return string.Empty;
        }
    }
}
