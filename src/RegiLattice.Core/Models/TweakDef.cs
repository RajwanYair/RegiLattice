// RegiLattice.Core — Models/TweakDef.cs
// Native C# tweak definition — replaces Python TweakDef dataclass.

namespace RegiLattice.Core.Models;

/// <summary>Scope of a tweak based on registry hive.</summary>
public enum TweakScope { User, Machine, Both }

/// <summary>Result of an apply/remove/detect operation.</summary>
public enum TweakResult { Applied, NotApplied, Unknown, Error, SkippedCorp, SkippedBuild }

/// <summary>
/// Immutable definition of a single registry tweak.
/// For simple registry tweaks, use <see cref="ApplyOps"/>/<see cref="RemoveOps"/>/<see cref="DetectOps"/>.
/// For complex tweaks requiring custom logic, use <see cref="ApplyAction"/>/<see cref="RemoveAction"/>/<see cref="DetectAction"/>.
/// </summary>
public sealed class TweakDef
{
    public required string Id { get; init; }
    public required string Label { get; init; }
    public required string Category { get; init; }
    public bool NeedsAdmin { get; init; } = true;
    public bool CorpSafe { get; init; }
    public IReadOnlyList<string> RegistryKeys { get; init; } = [];
    public string Description { get; init; } = "";
    public IReadOnlyList<string> Tags { get; init; } = [];
    public IReadOnlyList<string> DependsOn { get; init; } = [];
    public int MinBuild { get; init; }
    public string SideEffects { get; init; } = "";
    public string SourceUrl { get; init; } = "";

    // ── Declarative registry operations (majority of tweaks) ────────────
    public IReadOnlyList<RegOp> ApplyOps { get; init; } = [];
    public IReadOnlyList<RegOp> RemoveOps { get; init; } = [];
    public IReadOnlyList<RegOp> DetectOps { get; init; } = [];

    // ── Custom logic delegates (for complex tweaks like bcdedit, fsutil) ─
    public Action<bool>? ApplyAction { get; init; }
    public Action<bool>? RemoveAction { get; init; }
    public Func<bool>? DetectAction { get; init; }

    /// <summary>Cached scope, computed from RegistryKeys.</summary>
    public TweakScope Scope => _scope ??= ComputeScope();
    private TweakScope? _scope;

    private TweakScope ComputeScope()
    {
        bool hasUser = false, hasMachine = false;
        foreach (var k in RegistryKeys)
        {
            if (k.StartsWith("HKCU", StringComparison.OrdinalIgnoreCase) ||
                k.StartsWith("HKEY_CURRENT_USER", StringComparison.OrdinalIgnoreCase))
                hasUser = true;
            else if (k.StartsWith("HKLM", StringComparison.OrdinalIgnoreCase) ||
                     k.StartsWith("HKEY_LOCAL_MACHINE", StringComparison.OrdinalIgnoreCase) ||
                     k.StartsWith("HKCR", StringComparison.OrdinalIgnoreCase) ||
                     k.StartsWith("HKEY_CLASSES_ROOT", StringComparison.OrdinalIgnoreCase))
                hasMachine = true;
        }
        if (hasUser && hasMachine) return TweakScope.Both;
        if (hasUser) return TweakScope.User;
        if (!hasUser && !hasMachine && !NeedsAdmin) return TweakScope.User;
        return TweakScope.Machine;
    }

    public override string ToString() => $"{Id} [{Category}]";
}

/// <summary>Single registry operation.</summary>
public sealed class RegOp
{
    public required RegOpKind Kind { get; init; }
    public required string Path { get; init; }
    public string Name { get; init; } = "";
    public object? Value { get; init; }
    public Microsoft.Win32.RegistryValueKind ValueKind { get; init; } = Microsoft.Win32.RegistryValueKind.DWord;

    // ── Factory methods ─────────────────────────────────────────────────
    public static RegOp SetDword(string path, string name, int value) => new()
        { Kind = RegOpKind.SetValue, Path = path, Name = name, Value = value, ValueKind = Microsoft.Win32.RegistryValueKind.DWord };

    public static RegOp SetString(string path, string name, string value) => new()
        { Kind = RegOpKind.SetValue, Path = path, Name = name, Value = value, ValueKind = Microsoft.Win32.RegistryValueKind.String };

    public static RegOp SetExpandString(string path, string name, string value) => new()
        { Kind = RegOpKind.SetValue, Path = path, Name = name, Value = value, ValueKind = Microsoft.Win32.RegistryValueKind.ExpandString };

    public static RegOp SetQword(string path, string name, long value) => new()
        { Kind = RegOpKind.SetValue, Path = path, Name = name, Value = value, ValueKind = Microsoft.Win32.RegistryValueKind.QWord };

    public static RegOp SetBinary(string path, string name, byte[] value) => new()
        { Kind = RegOpKind.SetValue, Path = path, Name = name, Value = value, ValueKind = Microsoft.Win32.RegistryValueKind.Binary };

    public static RegOp SetMultiSz(string path, string name, string[] value) => new()
        { Kind = RegOpKind.SetValue, Path = path, Name = name, Value = value, ValueKind = Microsoft.Win32.RegistryValueKind.MultiString };

    public static RegOp DeleteValue(string path, string name) => new()
        { Kind = RegOpKind.DeleteValue, Path = path, Name = name };

    public static RegOp DeleteTree(string path) => new()
        { Kind = RegOpKind.DeleteTree, Path = path };

    public static RegOp CheckDword(string path, string name, int expected) => new()
        { Kind = RegOpKind.CheckValue, Path = path, Name = name, Value = expected, ValueKind = Microsoft.Win32.RegistryValueKind.DWord };

    public static RegOp CheckString(string path, string name, string expected) => new()
        { Kind = RegOpKind.CheckValue, Path = path, Name = name, Value = expected, ValueKind = Microsoft.Win32.RegistryValueKind.String };

    public static RegOp CheckMissing(string path, string name) => new()
        { Kind = RegOpKind.CheckMissing, Path = path, Name = name };

    public static RegOp CheckKeyMissing(string path) => new()
        { Kind = RegOpKind.CheckKeyMissing, Path = path };
}

public enum RegOpKind
{
    SetValue,
    DeleteValue,
    DeleteTree,
    CheckValue,
    CheckMissing,
    CheckKeyMissing,
}
