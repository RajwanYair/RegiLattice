using RegiLattice.Core;
using RegiLattice.Core.Models;
using RegiLattice.Core.Registry;
using Xunit;

namespace RegiLattice.Core.Tests;

/// <summary>Shared fixture that calls RegisterBuiltins() once for all tests that need it.</summary>
public sealed class BuiltinsFixture
{
    public TweakEngine Engine { get; }

    public BuiltinsFixture()
    {
        Engine = new TweakEngine(new RegistrySession(dryRun: true));
        Engine.RegisterBuiltins();
    }
}

/// <summary>Shared factory methods for creating test data across all Core test files.</summary>
internal static class TestHelpers
{
    /// <summary>Create a TweakEngine in dry-run mode, optionally pre-loaded with tweaks.</summary>
    internal static TweakEngine CreateEngine(params TweakDef[] tweaks)
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        if (tweaks.Length > 0)
            engine.Register(tweaks);
        return engine;
    }

    /// <summary>Create a minimal valid TweakDef with HKCU registry ops.</summary>
    internal static TweakDef MakeTweak(string id, string category = "Test", string label = "Tweak") =>
        new()
        {
            Id = id,
            Label = label,
            Category = category,
            RegistryKeys = [$@"HKCU\Software\{id}"],
            Description = $"Description for {id}",
            Tags = ["test", category.ToLowerInvariant()],
            ApplyOps = [RegOp.SetDword($@"HKCU\Software\{id}", "Enabled", 1)],
            RemoveOps = [RegOp.DeleteValue($@"HKCU\Software\{id}", "Enabled")],
            DetectOps = [RegOp.CheckDword($@"HKCU\Software\{id}", "Enabled", 1)],
        };

    /// <summary>Create a minimal TweakDef for validator/dependency tests (supports DependsOn).</summary>
    internal static TweakDef Make(
        string id,
        string label = "Tweak",
        string category = "Test",
        IReadOnlyList<string>? dependsOn = null,
        bool hasApplyOps = true
    ) =>
        new()
        {
            Id = id,
            Label = label,
            Category = category,
            DependsOn = dependsOn ?? [],
            ApplyOps = hasApplyOps ? [RegOp.SetDword($@"HKCU\Software\{id}", "V", 1)] : [],
        };

    /// <summary>Create a lookup function from a set of TweakDefs.</summary>
    internal static Func<string, TweakDef?> LookupFrom(params TweakDef[] tweaks)
    {
        var dict = tweaks.ToDictionary(t => t.Id, StringComparer.OrdinalIgnoreCase);
        return id => dict.GetValueOrDefault(id);
    }
}
