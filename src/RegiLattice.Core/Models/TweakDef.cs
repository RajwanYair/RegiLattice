// RegiLattice.Core — Models/TweakDef.cs
// C# tweak definition model.

namespace RegiLattice.Core.Models;

/// <summary>Scope of a tweak based on registry hive.</summary>
public enum TweakScope
{
    User,
    Machine,
    Both,
}

/// <summary>Result of an apply/remove/detect operation.</summary>
public enum TweakResult
{
    Applied,
    NotApplied,
    Unknown,
    Error,
    SkippedCorp,
    SkippedBuild,
    SkippedHw,
}

/// <summary>How a tweak performs its work.</summary>
public enum TweakKind
{
    /// <summary>Tweak modifies the Windows registry via RegOps.</summary>
    Registry,

    /// <summary>Tweak runs a PowerShell cmdlet or script block.</summary>
    PowerShell,

    /// <summary>Tweak runs a system command (bcdedit, dism, netsh, etc.).</summary>
    SystemCommand,

    /// <summary>Tweak controls a Windows service (sc, Set-Service).</summary>
    ServiceControl,

    /// <summary>Tweak manages a scheduled task (schtasks, Register-ScheduledTask).</summary>
    ScheduledTask,

    /// <summary>Tweak modifies a configuration file (e.g. JSON, INI, .wslconfig).</summary>
    FileConfig,

    /// <summary>Tweak modifies Group Policy via registry under Policies key.</summary>
    GroupPolicy,

    /// <summary>Tweak installs/configures a package manager tool.</summary>
    PackageManager,
}

/// <summary>Risk flags describing what a tweak may affect. Auto-detected from ApplyOps when not explicitly set.</summary>
[Flags]
public enum TweakRisk
{
    None = 0,
    ModifiesHKLM = 1 << 0, // Machine-wide registry change (HKLM)
    ModifiesHKCU = 1 << 1, // User-scope registry change (HKCU)
    DeletesKey = 1 << 2, // Uses DeleteTree or DeleteValue
    RequiresReboot = 1 << 3, // Change needs reboot to fully take effect
    AffectsService = 1 << 4, // Stops or disables a Windows service
    AffectsNetwork = 1 << 5, // Modifies firewall, proxy, or DNS settings
    AffectsSecurity = 1 << 6, // Weakens or hardens a security boundary
    PotentialDataLoss = 1 << 7, // Could cause data loss (e.g. deletes a cache/log tree)
}

/// <summary>Standard icon identifier for a tweak category.</summary>
public enum CategoryIcon
{
    Shield, // Security, Defender, Firewall, Encryption
    Globe, // Network, DNS, Browser (Edge, Chrome, Firefox)
    Monitor, // Display, GPU, Night Light
    Gear, // System, Performance, Boot, Services
    Lock, // Privacy, Telemetry, Lock Screen
    HardDrive, // Storage, File System, Backup, Recovery
    Cpu, // Performance, Power, Gaming
    Keyboard, // Input, Accessibility, Touch & Pen
    Speaker, // Audio, Multimedia, Speech
    Cloud, // Cloud Storage, OneDrive
    App, // MsStore, Package Management, Scoop
    Terminal, // Shell, Windows Terminal, WSL, Dev Drive
    Mail, // Communication, Office, M365
    Palette, // Fonts, Context Menu, Explorer
    Notification, // Notifications, Widgets
    Wrench, // Maintenance, Scheduled Tasks, Crash
    Phone, // Phone Link, Bluetooth, USB
    Desktop, // Taskbar, Snap, Startup, Screensaver
    Windows, // Win11, Windows Update
    Search, // Cortana, Indexing & Search
    Camera, // Remote Desktop, RealVNC, Virtualization
    Printer, // Printing
    Code, // VS Code, Java, LibreOffice, Adobe
}

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

    /// <summary>Explicit expected-result text shown when tweak is selected. Auto-generated from metadata if empty.</summary>
    public string ExpectedResult { get; init; } = "";

    // ── Declarative registry operations (majority of tweaks) ────────────
    public IReadOnlyList<RegOp> ApplyOps { get; init; } = [];
    public IReadOnlyList<RegOp> RemoveOps { get; init; } = [];
    public IReadOnlyList<RegOp> DetectOps { get; init; } = [];

    // ── Custom logic delegates (for complex tweaks like bcdedit, fsutil) ─
    public Action<bool>? ApplyAction { get; init; }
    public Action<bool>? RemoveAction { get; init; }
    public Func<bool>? DetectAction { get; init; }

    /// <summary>
    /// Optional update action for package-manager tweaks (e.g. scoop update, pip upgrade).
    /// Only meaningful for <see cref="TweakKind.PackageManager"/> tweaks.
    /// </summary>
    public Action<bool>? UpdateAction { get; init; }

    /// <summary>Override the auto-detected TweakKind (set on tweaks with ApplyAction).</summary>
    public TweakKind? KindHint { get; init; }

    /// <summary>
    /// Optional predicate that returns false when this tweak is not applicable
    /// to the current hardware (e.g. NVIDIA tweaks on a non-NVIDIA machine).
    /// Evaluated lazily and cached.
    /// </summary>
    public Func<bool>? IsApplicable { get; init; }

    /// <summary>Human-readable reason when <see cref="IsApplicable"/> returns false.</summary>
    public string ApplicabilityNote { get; init; } = "";

    /// <summary>
    /// If non-null, this tweak came from an installed Tweak Pack (plugin).
    /// Value is the pack name (e.g. "gaming-extra"). Null = built-in.
    /// </summary>
    public string? PackSource { get; init; }

    // ── Impact & safety metadata (Phase C — Intelligence Engine) ────────

    /// <summary>
    /// Estimated benefit magnitude when this tweak is applied (1 = minimal, 5 = major).
    /// Used by the Health Score dashboard and Smart Scan recommendation engine.
    /// Defaults to 3 (moderate) if not explicitly set.
    /// </summary>
    public int ImpactScore { get; init; } = 3;

    /// <summary>
    /// Safety rating indicating how risky applying this tweak is (1 = risky, 5 = very safe).
    /// Used by the recommendation engine to surface safe Quick Wins first.
    /// Defaults to 4 (safe for most users) if not explicitly set.
    /// </summary>
    public int SafetyRating { get; init; } = 4;

    /// <summary>
    /// Short human-readable description of the expected impact when this tweak is applied.
    /// Shown in the detail panel and CLI output. Empty string = omitted from display.
    /// Example: "Reduces boot time by ~2–5 s by disabling heavy startup services."
    /// </summary>
    public string ImpactNote { get; init; } = "";

    /// <summary>
    /// Explicit risk flags for this tweak. When left as <see cref="TweakRisk.None"/>,
    /// <see cref="EffectiveRiskFlags"/> auto-detects from <see cref="ApplyOps"/> and
    /// <see cref="Category"/>.
    /// </summary>
    public TweakRisk RiskFlags { get; init; } = TweakRisk.None;

    /// <summary>
    /// Effective risk flags: combines explicit <see cref="RiskFlags"/> with auto-detected
    /// flags derived from <see cref="ApplyOps"/> paths and <see cref="Category"/>.
    /// </summary>
    public TweakRisk EffectiveRiskFlags => _effectiveRiskFlags ??= ComputeEffectiveRiskFlags();
    private TweakRisk? _effectiveRiskFlags;

    private TweakRisk ComputeEffectiveRiskFlags()
    {
        TweakRisk flags = RiskFlags;

        // Auto-detect from ApplyOps registry paths
        foreach (var op in ApplyOps)
        {
            if (
                op.Path.StartsWith("HKLM", StringComparison.OrdinalIgnoreCase)
                || op.Path.StartsWith("HKEY_LOCAL_MACHINE", StringComparison.OrdinalIgnoreCase)
                || op.Path.StartsWith("HKCR", StringComparison.OrdinalIgnoreCase)
                || op.Path.StartsWith("HKEY_CLASSES_ROOT", StringComparison.OrdinalIgnoreCase)
            )
                flags |= TweakRisk.ModifiesHKLM;

            if (
                op.Path.StartsWith("HKCU", StringComparison.OrdinalIgnoreCase)
                || op.Path.StartsWith("HKEY_CURRENT_USER", StringComparison.OrdinalIgnoreCase)
            )
                flags |= TweakRisk.ModifiesHKCU;

            if (op.Kind is RegOpKind.DeleteTree or RegOpKind.DeleteValue)
                flags |= TweakRisk.DeletesKey;
        }

        // Auto-detect from Category
        switch (Category)
        {
            case "Services" or "Scheduled Tasks":
                flags |= TweakRisk.AffectsService;
                break;
            case "Network" or "Network Optimization" or "DNS & Networking Advanced" or "Firewall" or "Proxy & VPN":
                flags |= TweakRisk.AffectsNetwork;
                break;
            case "Security" or "Hardening" or "Encryption" or "User Account":
                flags |= TweakRisk.AffectsSecurity;
                break;
        }

        // Firewall affects both network and security
        if (Category == "Firewall")
            flags |= TweakRisk.AffectsNetwork | TweakRisk.AffectsSecurity;

        return flags;
    }

    /// <summary>How this tweak performs its work (auto-detected if KindHint not set, cached).</summary>
    public TweakKind Kind => _kind ??= KindHint ?? (ApplyAction is not null ? TweakKind.PowerShell : DetectKindFromOps());
    private TweakKind? _kind;

    /// <summary>Whether this tweak has any operations defined (not a stub).</summary>
    public bool HasOperations => ApplyOps.Count > 0 || ApplyAction is not null;

    /// <summary>
    /// Estimated time in milliseconds to apply or remove this tweak, derived from <see cref="Kind"/>.
    /// Used by the GUI batch-progress ETA calculator (Phase 2.4).
    /// Registry/GroupPolicy = 50 ms, FileConfig = 200 ms, ScheduledTask/PowerShell = 500 ms,
    /// SystemCommand = 1 000 ms, ServiceControl = 2 000 ms, PackageManager = 3 000 ms.
    /// </summary>
    public int EstimatedApplyTimeMs => Kind switch
    {
        TweakKind.Registry => 50,
        TweakKind.GroupPolicy => 50,
        TweakKind.FileConfig => 200,
        TweakKind.ScheduledTask => 500,
        TweakKind.PowerShell => 500,
        TweakKind.SystemCommand => 1_000,
        TweakKind.ServiceControl => 2_000,
        TweakKind.PackageManager => 3_000,
        _ => 100,
    };

    /// <summary>Cached scope, computed from RegistryKeys.</summary>
    public TweakScope Scope => _scope ??= ComputeScope();
    private TweakScope? _scope;

    private TweakScope ComputeScope()
    {
        bool hasUser = false,
            hasMachine = false;
        foreach (var k in RegistryKeys)
        {
            if (k.StartsWith("HKCU", StringComparison.OrdinalIgnoreCase) || k.StartsWith("HKEY_CURRENT_USER", StringComparison.OrdinalIgnoreCase))
                hasUser = true;
            else if (
                k.StartsWith("HKLM", StringComparison.OrdinalIgnoreCase)
                || k.StartsWith("HKEY_LOCAL_MACHINE", StringComparison.OrdinalIgnoreCase)
                || k.StartsWith("HKCR", StringComparison.OrdinalIgnoreCase)
                || k.StartsWith("HKEY_CLASSES_ROOT", StringComparison.OrdinalIgnoreCase)
            )
                hasMachine = true;
        }
        if (hasUser && hasMachine)
            return TweakScope.Both;
        if (hasUser)
            return TweakScope.User;
        if (!hasUser && !hasMachine && !NeedsAdmin)
            return TweakScope.User;
        return TweakScope.Machine;
    }

    /// <summary>Returns ExpectedResult if explicitly set, otherwise auto-generates from metadata (cached).</summary>
    public string GetExpectedResult() => _cachedExpectedResult ??= (ExpectedResult.Length > 0 ? ExpectedResult : GenerateExpectedResult());

    private string? _cachedExpectedResult;

    private string GenerateExpectedResult()
    {
        // Parse action verb from Label
        string label = Label;
        string verb = "";
        string subject = label;

        if (label.StartsWith("Disable ", StringComparison.OrdinalIgnoreCase))
        {
            verb = "disable";
            subject = label[8..];
        }
        else if (label.StartsWith("Enable ", StringComparison.OrdinalIgnoreCase))
        {
            verb = "enable";
            subject = label[7..];
        }
        else if (label.StartsWith("Block ", StringComparison.OrdinalIgnoreCase))
        {
            verb = "block";
            subject = label[6..];
        }
        else if (label.StartsWith("Remove ", StringComparison.OrdinalIgnoreCase))
        {
            verb = "remove";
            subject = label[7..];
        }
        else if (label.StartsWith("Hide ", StringComparison.OrdinalIgnoreCase))
        {
            verb = "hide";
            subject = label[5..];
        }
        else if (label.StartsWith("Show ", StringComparison.OrdinalIgnoreCase))
        {
            verb = "show";
            subject = label[5..];
        }
        else if (label.StartsWith("Set ", StringComparison.OrdinalIgnoreCase))
        {
            verb = "set";
            subject = label[4..];
        }
        else if (label.StartsWith("Optimize ", StringComparison.OrdinalIgnoreCase))
        {
            verb = "optimize";
            subject = label[9..];
        }
        else if (label.StartsWith("Reduce ", StringComparison.OrdinalIgnoreCase))
        {
            verb = "reduce";
            subject = label[7..];
        }
        else if (label.StartsWith("Increase ", StringComparison.OrdinalIgnoreCase))
        {
            verb = "increase";
            subject = label[9..];
        }
        else if (label.StartsWith("Configure ", StringComparison.OrdinalIgnoreCase))
        {
            verb = "configure";
            subject = label[10..];
        }
        else if (label.StartsWith("Force ", StringComparison.OrdinalIgnoreCase))
        {
            verb = "force";
            subject = label[6..];
        }
        else if (label.StartsWith("Prevent ", StringComparison.OrdinalIgnoreCase))
        {
            verb = "prevent";
            subject = label[8..];
        }
        else if (label.StartsWith("Restrict ", StringComparison.OrdinalIgnoreCase))
        {
            verb = "restrict";
            subject = label[9..];
        }
        else if (label.StartsWith("Limit ", StringComparison.OrdinalIgnoreCase))
        {
            verb = "limit";
            subject = label[6..];
        }

        string action = verb switch
        {
            "disable" => $"{subject} will be turned off.",
            "enable" => $"{subject} will be activated.",
            "block" => $"{subject} will be blocked.",
            "remove" => $"{subject} will be removed.",
            "hide" => $"{subject} will be hidden from view.",
            "show" => $"{subject} will become visible.",
            "set" => $"{subject} will be applied.",
            "optimize" => $"{subject} will be optimized.",
            "reduce" => $"{subject} will be reduced.",
            "increase" => $"{subject} will be increased.",
            "configure" => $"{subject} will be configured.",
            "force" => $"{subject} will be enforced.",
            "prevent" => $"{subject} will be prevented.",
            "restrict" => $"{subject} will be restricted.",
            "limit" => $"{subject} will be limited.",
            _ => $"{label} will be applied.",
        };

        string categoryNote = Category switch
        {
            "Privacy" or "Telemetry Advanced" => " Less data will be sent to Microsoft, improving your privacy.",
            "Performance" or "Memory Optimization" or "SSD Optimization" => " System responsiveness and speed may improve.",
            "Gaming" or "GPU / Graphics" => " Gaming performance and frame rates may improve.",
            "Security" or "Hardening" or "Encryption" or "Firewall" => " System security posture will be strengthened.",
            "Network" or "Network Optimization" or "DNS & Networking Advanced" => " Network behaviour or performance will change.",
            "Power" or "Power Management" => " Power consumption or sleep behaviour will change.",
            "Boot" or "Startup" => " Startup or boot time may be affected.",
            "Explorer" or "Context Menu" or "Shell" or "Taskbar" => " Windows shell appearance or behaviour will change.",
            "Notifications" => " Notification behaviour will change; fewer interruptions.",
            "Windows Update" => " Windows Update behaviour will be modified.",
            "Accessibility" => " Accessibility features will be adjusted.",
            "Bluetooth" or "USB & Peripherals" => " Peripheral device behaviour will change.",
            "Printing" => " Printing configuration will be modified.",
            _ => "",
        };

        bool needsRestart =
            NeedsAdmin
            || RegistryKeys.Any(k =>
                k.Contains("HKLM", StringComparison.OrdinalIgnoreCase) || k.Contains("HKEY_LOCAL_MACHINE", StringComparison.OrdinalIgnoreCase)
            );
        string restartNote = needsRestart ? " A restart or sign-out may be needed for changes to take full effect." : "";

        return action + categoryNote + restartNote;
    }

    public override string ToString() => $"{Id} [{Category}]";

    private TweakKind DetectKindFromOps()
    {
        foreach (var op in ApplyOps)
        {
            if (op.Path.Contains(@"\Policies\", StringComparison.OrdinalIgnoreCase))
                return TweakKind.GroupPolicy;
        }
        return TweakKind.Registry;
    }
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
    public static RegOp SetDword(string path, string name, int value) =>
        new()
        {
            Kind = RegOpKind.SetValue,
            Path = path,
            Name = name,
            Value = value,
            ValueKind = Microsoft.Win32.RegistryValueKind.DWord,
        };

    public static RegOp SetString(string path, string name, string value) =>
        new()
        {
            Kind = RegOpKind.SetValue,
            Path = path,
            Name = name,
            Value = value,
            ValueKind = Microsoft.Win32.RegistryValueKind.String,
        };

    public static RegOp SetExpandString(string path, string name, string value) =>
        new()
        {
            Kind = RegOpKind.SetValue,
            Path = path,
            Name = name,
            Value = value,
            ValueKind = Microsoft.Win32.RegistryValueKind.ExpandString,
        };

    public static RegOp SetQword(string path, string name, long value) =>
        new()
        {
            Kind = RegOpKind.SetValue,
            Path = path,
            Name = name,
            Value = value,
            ValueKind = Microsoft.Win32.RegistryValueKind.QWord,
        };

    public static RegOp SetBinary(string path, string name, byte[] value) =>
        new()
        {
            Kind = RegOpKind.SetValue,
            Path = path,
            Name = name,
            Value = value,
            ValueKind = Microsoft.Win32.RegistryValueKind.Binary,
        };

    public static RegOp SetMultiSz(string path, string name, string[] value) =>
        new()
        {
            Kind = RegOpKind.SetValue,
            Path = path,
            Name = name,
            Value = value,
            ValueKind = Microsoft.Win32.RegistryValueKind.MultiString,
        };

    public static RegOp DeleteValue(string path, string name) =>
        new()
        {
            Kind = RegOpKind.DeleteValue,
            Path = path,
            Name = name,
        };

    public static RegOp DeleteTree(string path) => new() { Kind = RegOpKind.DeleteTree, Path = path };

    public static RegOp CheckDword(string path, string name, int expected) =>
        new()
        {
            Kind = RegOpKind.CheckValue,
            Path = path,
            Name = name,
            Value = expected,
            ValueKind = Microsoft.Win32.RegistryValueKind.DWord,
        };

    public static RegOp CheckString(string path, string name, string expected) =>
        new()
        {
            Kind = RegOpKind.CheckValue,
            Path = path,
            Name = name,
            Value = expected,
            ValueKind = Microsoft.Win32.RegistryValueKind.String,
        };

    public static RegOp CheckMissing(string path, string name) =>
        new()
        {
            Kind = RegOpKind.CheckMissing,
            Path = path,
            Name = name,
        };

    public static RegOp CheckKeyMissing(string path) => new() { Kind = RegOpKind.CheckKeyMissing, Path = path };
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

/// <summary>
/// Result of a transactional batch apply or remove operation.
/// Returned by the <see cref="TweakEngine.ApplyBatch(IReadOnlyList{string},bool,bool,Action{int,int,string}?,System.Threading.CancellationToken)"/>
/// and <see cref="TweakEngine.RemoveBatch(IReadOnlyList{string},bool,bool,Action{int,int,string}?,System.Threading.CancellationToken)"/> overloads.
/// </summary>
public sealed class BatchResult
{
    public required IReadOnlyList<(string Id, TweakResult Result)> Results { get; init; }

    /// <summary>True if the batch was transactional and rolled back due to a failure.</summary>
    public bool RolledBack { get; init; }

    /// <summary>IDs of tweaks that could not be reverted during rollback (empty when <see cref="RolledBack"/> is false).</summary>
    public IReadOnlyList<string> RollbackErrors { get; init; } = [];

    /// <summary>True if the cancellation token was triggered before all tweaks were processed.</summary>
    public bool WasCancelled { get; init; }

    public int SuccessCount => Results.Count(r => r.Result is TweakResult.Applied or TweakResult.NotApplied);
    public int FailureCount => Results.Count(r => r.Result == TweakResult.Error);
    public bool HasErrors => FailureCount > 0;
}
