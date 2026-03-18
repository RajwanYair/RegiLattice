#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Win32 = Microsoft.Win32;

namespace RegiLattice.Core.Services;

/// <summary>Where the startup entry lives.</summary>
public enum StartupLocation
{
    /// <summary>HKCU\Software\Microsoft\Windows\CurrentVersion\Run</summary>
    RegistryUser,

    /// <summary>HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Run</summary>
    RegistryMachine,

    /// <summary>%APPDATA%\Microsoft\Windows\Start Menu\Programs\Startup</summary>
    FolderUser,

    /// <summary>%ALLUSERSPROFILE%\Microsoft\Windows\Start Menu\Programs\StartUp</summary>
    FolderAllUsers,
}

/// <summary>Immutable snapshot of a single startup entry.</summary>
public sealed record StartupEntry(string Id, string Name, string Command, StartupLocation Location, bool IsEnabled);

/// <summary>
/// Reads and manages Windows startup entries from the registry and
/// the per-user / all-users Startup shell folders.
/// All write operations require administrator rights when touching
/// HKLM or the all-users Startup folder.
/// </summary>
public static class StartupManager
{
    private const string RegRunUser = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
    private const string RegRunMachine = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

    // Disabled entries are stored under a companion key using the same name prefixed with a timestamp.
    private const string RegRunUserDisabled = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run-Disabled";
    private const string RegRunMachineDisabled = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run-Disabled";

    // ── Public API ──────────────────────────────────────────────────────────

    /// <summary>Returns all startup entries from registry + Startup folders.</summary>
    public static IReadOnlyList<StartupEntry> GetAllEntries()
    {
        var entries = new List<StartupEntry>();

        entries.AddRange(ReadRegistryEntries(Win32.Registry.CurrentUser, RegRunUser, StartupLocation.RegistryUser, enabled: true));
        entries.AddRange(ReadRegistryEntries(Win32.Registry.CurrentUser, RegRunUserDisabled, StartupLocation.RegistryUser, enabled: false));
        entries.AddRange(ReadRegistryEntries(Win32.Registry.LocalMachine, RegRunMachine, StartupLocation.RegistryMachine, enabled: true));
        entries.AddRange(ReadRegistryEntries(Win32.Registry.LocalMachine, RegRunMachineDisabled, StartupLocation.RegistryMachine, enabled: false));
        entries.AddRange(ReadFolderEntries(GetUserStartupFolder(), StartupLocation.FolderUser));
        entries.AddRange(ReadFolderEntries(GetAllUsersStartupFolder(), StartupLocation.FolderAllUsers));

        return entries.AsReadOnly();
    }

    /// <summary>
    /// Enables or disables a startup entry.
    /// Registry entries are moved between the Run and Run-Disabled keys.
    /// Folder entries are renamed with a ".disabled" extension.
    /// </summary>
    public static void SetEnabled(StartupEntry entry, bool enable)
    {
        if (entry.IsEnabled == enable)
            return;

        if (entry.Location is StartupLocation.RegistryUser or StartupLocation.RegistryMachine)
            ToggleRegistryEntry(entry, enable);
        else
            ToggleFolderEntry(entry, enable);
    }

    /// <summary>
    /// Permanently deletes a startup entry.  Registry entries are removed from whichever
    /// key they live in; folder entries have their .lnk/.disabled file deleted.
    /// </summary>
    public static void Delete(StartupEntry entry)
    {
        if (entry.Location is StartupLocation.RegistryUser or StartupLocation.RegistryMachine)
            DeleteRegistryEntry(entry);
        else
            DeleteFolderEntry(entry);
    }

    // ── Registry helpers ────────────────────────────────────────────────────

    private static IEnumerable<StartupEntry> ReadRegistryEntries(Win32.RegistryKey hive, string subKeyPath, StartupLocation location, bool enabled)
    {
        // Cannot yield inside a try/catch — collect into list first.
        var results = new List<StartupEntry>();
        try
        {
            using Win32.RegistryKey? key = hive.OpenSubKey(subKeyPath);
            if (key is not null)
            {
                foreach (string name in key.GetValueNames())
                {
                    string command = key.GetValue(name)?.ToString() ?? string.Empty;
                    if (!string.IsNullOrWhiteSpace(command))
                        results.Add(new StartupEntry(Id: $"{location}|{name}", Name: name, Command: command, Location: location, IsEnabled: enabled));
                }
            }
        }
        catch (Exception ex) when (ex is UnauthorizedAccessException or System.Security.SecurityException)
        {
            // No read access — skip silently (happens for HKLM on non-admin sessions)
            _ = ex;
        }
        return results;
    }

    private static void ToggleRegistryEntry(StartupEntry entry, bool enable)
    {
        Win32.RegistryKey hive = entry.Location == StartupLocation.RegistryUser ? Win32.Registry.CurrentUser : Win32.Registry.LocalMachine;
        string activeKey = entry.Location == StartupLocation.RegistryUser ? RegRunUser : RegRunMachine;
        string disabledKey = entry.Location == StartupLocation.RegistryUser ? RegRunUserDisabled : RegRunMachineDisabled;

        string sourceKey = enable ? disabledKey : activeKey;
        string destKey = enable ? activeKey : disabledKey;

        using Win32.RegistryKey? src = hive.OpenSubKey(sourceKey, writable: true);
        using Win32.RegistryKey? dest = hive.CreateSubKey(destKey);

        if (src is null || dest is null)
            throw new InvalidOperationException($"Cannot open registry key for {entry.Name}.");

        object? value = src.GetValue(entry.Name);
        if (value is null)
            throw new InvalidOperationException($"Registry value '{entry.Name}' not found in source key.");

        dest.SetValue(entry.Name, value, src.GetValueKind(entry.Name));
        src.DeleteValue(entry.Name);
    }

    private static void DeleteRegistryEntry(StartupEntry entry)
    {
        Win32.RegistryKey hive = entry.Location == StartupLocation.RegistryUser ? Win32.Registry.CurrentUser : Win32.Registry.LocalMachine;
        string activeKey = entry.Location == StartupLocation.RegistryUser ? RegRunUser : RegRunMachine;
        string disabledKey = entry.Location == StartupLocation.RegistryUser ? RegRunUserDisabled : RegRunMachineDisabled;

        TryDeleteValue(hive, activeKey, entry.Name);
        TryDeleteValue(hive, disabledKey, entry.Name);
    }

    private static void TryDeleteValue(Win32.RegistryKey hive, string subKey, string name)
    {
        try
        {
            using Win32.RegistryKey? key = hive.OpenSubKey(subKey, writable: true);
            if (key is not null && key.GetValue(name) is not null)
                key.DeleteValue(name);
        }
        catch (Exception ex) when (ex is UnauthorizedAccessException or System.Security.SecurityException)
        {
            throw new UnauthorizedAccessException($"Access denied deleting '{name}' from registry.", ex);
        }
    }

    // ── Folder helpers ──────────────────────────────────────────────────────

    private static IEnumerable<StartupEntry> ReadFolderEntries(string folderPath, StartupLocation location)
    {
        if (!Directory.Exists(folderPath))
            yield break;

        foreach (string file in Directory.EnumerateFiles(folderPath))
        {
            bool isDisabled = file.EndsWith(".disabled", StringComparison.OrdinalIgnoreCase);
            string name = Path.GetFileNameWithoutExtension(isDisabled ? file[..^".disabled".Length] : file);

            yield return new StartupEntry(Id: $"{location}|{name}", Name: name, Command: file, Location: location, IsEnabled: !isDisabled);
        }
    }

    private static void ToggleFolderEntry(StartupEntry entry, bool enable)
    {
        string current = entry.Command;
        string target = enable ? current[..^".disabled".Length] : current + ".disabled";

        if (!File.Exists(current))
            throw new FileNotFoundException($"Startup file not found: {current}");

        File.Move(current, target);
    }

    private static void DeleteFolderEntry(StartupEntry entry)
    {
        if (File.Exists(entry.Command))
            File.Delete(entry.Command);
    }

    // ── Path helpers ────────────────────────────────────────────────────────

    private static string GetUserStartupFolder() =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Microsoft\Windows\Start Menu\Programs\Startup");

    private static string GetAllUsersStartupFolder() =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"Microsoft\Windows\Start Menu\Programs\StartUp");
}
