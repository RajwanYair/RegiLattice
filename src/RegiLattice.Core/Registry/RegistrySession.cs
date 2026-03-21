// RegiLattice.Core — Registry/RegistrySession.cs
// Windows registry wrapper using Microsoft.Win32.Registry.
// Replaces the Python RegistrySession entirely — zero subprocess, zero P/Invoke.

using System.Text.Json;
using Microsoft.Win32;
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Registry;

/// <summary>
/// Thread-safe registry read/write/backup wrapper.
/// All operations go through <see cref="Microsoft.Win32.Registry"/> APIs.
/// </summary>
public sealed class RegistrySession
{
    private readonly bool _dryRun;
    private int _dryOps;
    private readonly object _logLock = new();
    private readonly List<string> _log = [];
    private readonly string _backupDir;

    public bool DryRun => _dryRun;
    public int DryOps => _dryOps;
    public IReadOnlyList<string> Log => _log;

    public RegistrySession(bool dryRun = false, string? backupDir = null)
    {
        _dryRun = dryRun;
        _backupDir = backupDir ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "RegiLattice", "backups");
    }

    // ── Write operations ────────────────────────────────────────────────

    public void SetDword(string path, string name, int value) => SetValue(path, name, value, RegistryValueKind.DWord);

    public void SetString(string path, string name, string value) => SetValue(path, name, value, RegistryValueKind.String);

    public void SetExpandString(string path, string name, string value) => SetValue(path, name, value, RegistryValueKind.ExpandString);

    public void SetQword(string path, string name, long value) => SetValue(path, name, value, RegistryValueKind.QWord);

    public void SetBinary(string path, string name, byte[] value) => SetValue(path, name, value, RegistryValueKind.Binary);

    public void SetMultiSz(string path, string name, string[] value) => SetValue(path, name, value, RegistryValueKind.MultiString);

    public void SetValue(string path, string name, object value, RegistryValueKind kind)
    {
        WriteLog($"SET {path}\\{name} = {value} ({kind})");
        if (_dryRun)
        {
            Interlocked.Increment(ref _dryOps);
            return;
        }

        var (root, subKey) = ParsePath(path);
        using var key = root.CreateSubKey(subKey, writable: true) ?? throw new InvalidOperationException($"Cannot open/create key: {path}");
        key.SetValue(name, value, kind);
    }

    // ── Delete operations ───────────────────────────────────────────────

    public void DeleteValue(string path, string name)
    {
        WriteLog($"DELETE {path}\\{name}");
        if (_dryRun)
        {
            Interlocked.Increment(ref _dryOps);
            return;
        }

        var (root, subKey) = ParsePath(path);
        using var key = root.OpenSubKey(subKey, writable: true);
        key?.DeleteValue(name, throwOnMissingValue: false);
    }

    public void DeleteTree(string path)
    {
        WriteLog($"DELETE_TREE {path}");
        if (_dryRun)
        {
            Interlocked.Increment(ref _dryOps);
            return;
        }

        var (root, subKey) = ParsePath(path);
        try
        {
            root.DeleteSubKeyTree(subKey, throwOnMissingSubKey: false);
        }
        catch (UnauthorizedAccessException)
        {
            WriteLog($"WARN: Cannot delete {path} (access denied)");
        }
    }

    // ── Read operations ─────────────────────────────────────────────────

    public int? ReadDword(string path, string name)
    {
        var (root, subKey) = ParsePath(path);
        using var key = root.OpenSubKey(subKey);
        return key?.GetValue(name) is int val ? val : null;
    }

    public string? ReadString(string path, string name)
    {
        var (root, subKey) = ParsePath(path);
        using var key = root.OpenSubKey(subKey);
        return key?.GetValue(name) as string;
    }

    public long? ReadQword(string path, string name)
    {
        var (root, subKey) = ParsePath(path);
        using var key = root.OpenSubKey(subKey);
        return key?.GetValue(name) is long val ? val : null;
    }

    public byte[]? ReadBinary(string path, string name)
    {
        var (root, subKey) = ParsePath(path);
        using var key = root.OpenSubKey(subKey);
        return key?.GetValue(name) as byte[];
    }

    public string[]? ReadMultiSz(string path, string name)
    {
        var (root, subKey) = ParsePath(path);
        using var key = root.OpenSubKey(subKey);
        return key?.GetValue(name) as string[];
    }

    public object? ReadValue(string path, string name)
    {
        var (root, subKey) = ParsePath(path);
        using var key = root.OpenSubKey(subKey);
        return key?.GetValue(name);
    }

    public bool KeyExists(string path)
    {
        var (root, subKey) = ParsePath(path);
        using var key = root.OpenSubKey(subKey);
        return key is not null;
    }

    public bool ValueExists(string path, string name)
    {
        var (root, subKey) = ParsePath(path);
        using var key = root.OpenSubKey(subKey);
        return key?.GetValue(name) is not null;
    }

    // ── Enumeration ─────────────────────────────────────────────────────

    public string[] ListSubKeys(string path)
    {
        var (root, subKey) = ParsePath(path);
        using var key = root.OpenSubKey(subKey);
        return key?.GetSubKeyNames() ?? [];
    }

    public string[] ListValueNames(string path)
    {
        var (root, subKey) = ParsePath(path);
        using var key = root.OpenSubKey(subKey);
        return key?.GetValueNames() ?? [];
    }

    // ── Backup / Restore ────────────────────────────────────────────────

    public string Backup(IReadOnlyList<string> keys, string label)
    {
        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var safeName = string.Join("_", label.Split(Path.GetInvalidFileNameChars()));
        var backupPath = Path.Combine(_backupDir, $"{timestamp}_{safeName}.json");

        Directory.CreateDirectory(_backupDir);

        var snapshot = new Dictionary<string, Dictionary<string, object?>>();
        foreach (var keyPath in keys)
        {
            var values = new Dictionary<string, object?>();
            var (root, subKey) = ParsePath(keyPath);
            using var key = root.OpenSubKey(subKey);
            if (key is not null)
            {
                foreach (var name in key.GetValueNames())
                    values[name] = key.GetValue(name);
            }
            snapshot[keyPath] = values;
        }

        var json = JsonSerializer.Serialize(snapshot, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(backupPath, json);
        WriteLog($"BACKUP [{label}] → {backupPath}");
        return backupPath;
    }

    // ── Execute RegOp ───────────────────────────────────────────────────

    /// <summary>Executes a list of write operations (apply/remove).</summary>
    public void Execute(IReadOnlyList<RegOp> ops)
    {
        foreach (var op in ops)
        {
            switch (op.Kind)
            {
                case RegOpKind.SetValue:
                    SetValue(op.Path, op.Name, op.Value!, op.ValueKind);
                    break;
                case RegOpKind.DeleteValue:
                    DeleteValue(op.Path, op.Name);
                    break;
                case RegOpKind.DeleteTree:
                    DeleteTree(op.Path);
                    break;
                default:
                    throw new InvalidOperationException($"Cannot execute read-only op: {op.Kind}");
            }
        }
    }

    /// <summary>Evaluates a list of detect/check operations. All must pass → true.</summary>
    public bool Evaluate(IReadOnlyList<RegOp> ops)
    {
        foreach (var op in ops)
        {
            switch (op.Kind)
            {
                case RegOpKind.CheckValue:
                    if (!CheckValueMatch(op))
                        return false;
                    break;
                case RegOpKind.CheckMissing:
                    if (ValueExists(op.Path, op.Name))
                        return false;
                    break;
                case RegOpKind.CheckKeyMissing:
                    if (KeyExists(op.Path))
                        return false;
                    break;
                default:
                    throw new InvalidOperationException($"Cannot evaluate write op: {op.Kind}");
            }
        }
        return ops.Count > 0;
    }

    private bool CheckValueMatch(RegOp op)
    {
        var (root, subKey) = ParsePath(op.Path);
        using var key = root.OpenSubKey(subKey);
        if (key is null)
            return false;
        var actual = key.GetValue(op.Name);
        if (actual is null)
            return false;

        return op.Value switch
        {
            int expected => actual is int i && i == expected,
            long expected => actual is long l && l == expected,
            string expected => actual is string s && s.Equals(expected, StringComparison.Ordinal),
            byte[] expected => actual is byte[] b && b.AsSpan().SequenceEqual(expected),
            _ => Equals(actual, op.Value),
        };
    }

    // ── Logging ─────────────────────────────────────────────────────────

    public void WriteLog(string message)
    {
        var entry = $"[{DateTime.Now:HH:mm:ss}] {message}";
        lock (_logLock)
        {
            _log.Add(entry);
        }
        LogWritten?.Invoke(entry);
    }

    public event Action<string>? LogWritten;

    // ── Path parsing ────────────────────────────────────────────────────

    public static (RegistryKey Root, string SubKey) ParsePath(string path)
    {
        // Normalize: HKLM\ → HKEY_LOCAL_MACHINE\, HKCU\ → HKEY_CURRENT_USER\
        var sep = path.IndexOf('\\');
        if (sep < 0)
            throw new ArgumentException($"Invalid registry path: {path}");

        var hive = path[..sep].ToUpperInvariant();
        var subKey = path[(sep + 1)..];

        return hive switch
        {
            "HKLM" or "HKEY_LOCAL_MACHINE" => (Microsoft.Win32.Registry.LocalMachine, subKey),
            "HKCU" or "HKEY_CURRENT_USER" => (Microsoft.Win32.Registry.CurrentUser, subKey),
            "HKCR" or "HKEY_CLASSES_ROOT" => (Microsoft.Win32.Registry.ClassesRoot, subKey),
            "HKU" or "HKEY_USERS" => (Microsoft.Win32.Registry.Users, subKey),
            "HKCC" or "HKEY_CURRENT_CONFIG" => (Microsoft.Win32.Registry.CurrentConfig, subKey),
            _ => throw new ArgumentException($"Unknown registry hive: {hive}"),
        };
    }
}
