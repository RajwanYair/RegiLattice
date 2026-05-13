using System.Text.Json;
using RegiLattice.Core;
using RegiLattice.Core.Models;
using RegiLattice.Core.Services;
using Xunit;

namespace RegiLattice.Core.Tests;

/// <summary>Tests for ConfigExporter — export, import, validate.</summary>
[Collection("ConfigExporter")]
public sealed class ConfigExporterTests : IDisposable
{
    private readonly string _tempDir;

    public ConfigExporterTests()
    {
        _tempDir = Path.Combine(Path.GetTempPath(), $"rl-configtest-{Guid.NewGuid()}");
        Directory.CreateDirectory(_tempDir);
    }

    public void Dispose()
    {
        try
        {
            Directory.Delete(_tempDir, true);
        }
        catch
        { /* best-effort cleanup */
        }
    }

    [Fact]
    public void Export_CreatesValidJsonFile()
    {
        var path = Path.Combine(_tempDir, "test.json");
        ConfigExporter.Export(path, ["perf-disable-animations", "priv-disable-telemetry"], "Test Config", "A test");

        Assert.True(File.Exists(path));
        var json = File.ReadAllText(path);
        var config = JsonSerializer.Deserialize<TweakConfig>(json);
        Assert.NotNull(config);
        Assert.Equal("Test Config", config!.Name);
        Assert.Equal(2, config.Tweaks.Count);
    }

    [Fact]
    public void Export_NullPath_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => ConfigExporter.Export(null!, []));
    }

    [Fact]
    public void Import_FullFormat_ReturnsConfig()
    {
        var path = Path.Combine(_tempDir, "full.json");
        var config = new TweakConfig
        {
            Name = "Full",
            Description = "Full format test",
            Tweaks = ["id-1", "id-2", "id-3"],
        };
        File.WriteAllText(path, JsonSerializer.Serialize(config));

        var imported = ConfigExporter.Import(path);
        Assert.NotNull(imported);
        Assert.Equal("Full", imported!.Name);
        Assert.Equal(3, imported.Tweaks.Count);
    }

    [Fact]
    public void Import_PlainArrayFormat_ReturnsConfig()
    {
        var path = Path.Combine(_tempDir, "array.json");
        File.WriteAllText(path, """["tweakA", "tweakB"]""");

        var imported = ConfigExporter.Import(path);
        Assert.NotNull(imported);
        Assert.Equal(2, imported!.Tweaks.Count);
        Assert.Contains("tweakA", imported.Tweaks);
    }

    [Fact]
    public void Import_TweaksObjectFormat_ReturnsConfig()
    {
        var path = Path.Combine(_tempDir, "obj.json");
        File.WriteAllText(path, """{"tweaks": ["x", "y"]}""");

        var imported = ConfigExporter.Import(path);
        Assert.NotNull(imported);
        Assert.Equal(2, imported!.Tweaks.Count);
    }

    [Fact]
    public void Import_MissingFile_ReturnsNull()
    {
        var result = ConfigExporter.Import(Path.Combine(_tempDir, "nope.json"));
        Assert.Null(result);
    }

    [Fact]
    public void Import_InvalidJson_ReturnsNull()
    {
        var path = Path.Combine(_tempDir, "bad.json");
        File.WriteAllText(path, "this is not json {{{");

        var result = ConfigExporter.Import(path);
        Assert.Null(result);
    }

    [Fact]
    public void Import_NullPath_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => ConfigExporter.Import(null!));
    }

    [Fact]
    public void Validate_SeparatesValidAndUnknownIds()
    {
        var engine = new TweakEngine();
        engine.Register([
            new TweakDef
            {
                Id = "test-known",
                Label = "Known",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "V", 1)],
            },
        ]);

        var config = new TweakConfig { Tweaks = ["test-known", "test-unknown", "also-unknown"] };
        var (valid, unknown) = ConfigExporter.Validate(config, engine);

        Assert.Single(valid);
        Assert.Equal("test-known", valid[0]);
        Assert.Equal(2, unknown.Count);
    }

    [Fact]
    public void ExportAndImport_RoundTrips()
    {
        var path = Path.Combine(_tempDir, "roundtrip.json");
        var ids = new[] { "alpha", "beta", "gamma" };

        ConfigExporter.Export(path, ids, "RoundTrip", "Test round-trip");
        var imported = ConfigExporter.Import(path);

        Assert.NotNull(imported);
        Assert.Equal("RoundTrip", imported!.Name);
        Assert.Equal(3, imported.Tweaks.Count);
        Assert.Contains("alpha", imported.Tweaks);
        Assert.Contains("gamma", imported.Tweaks);
    }

    [Fact]
    public void Export_EmptyTweakList_CreatesConfigWithEmptyArray()
    {
        var path = Path.Combine(_tempDir, "empty.json");
        ConfigExporter.Export(path, [], "Empty");
        var imported = ConfigExporter.Import(path);
        Assert.NotNull(imported);
        Assert.Empty(imported!.Tweaks);
    }

    [Fact]
    public void Export_CreatesIntermediateDirectory()
    {
        var nested = Path.Combine(_tempDir, "subdir", "deep", "config.json");
        ConfigExporter.Export(nested, ["id-x"]);
        Assert.True(File.Exists(nested));
    }

    [Fact]
    public void Export_OverwritesExistingFile()
    {
        var path = Path.Combine(_tempDir, "overwrite.json");
        ConfigExporter.Export(path, ["first"], "First");
        ConfigExporter.Export(path, ["second", "third"], "Second");
        var imported = ConfigExporter.Import(path);
        Assert.Equal("Second", imported!.Name);
        Assert.Equal(2, imported.Tweaks.Count);
    }

    [Fact]
    public void Export_PreservesDescription()
    {
        var path = Path.Combine(_tempDir, "desc.json");
        ConfigExporter.Export(path, ["a"], "N", "My description");
        var imported = ConfigExporter.Import(path);
        Assert.Equal("My description", imported!.Description);
    }

    [Fact]
    public void TweakConfig_DefaultVersion_IsNonEmpty()
    {
        var config = new TweakConfig();
        Assert.False(string.IsNullOrEmpty(config.RegiLatticeVersion));
    }

    [Fact]
    public void TweakConfig_DefaultCreated_IsValidDate()
    {
        var config = new TweakConfig();
        Assert.True(DateTimeOffset.TryParse(config.Created, out _));
    }

    [Fact]
    public void TweakConfig_Tweaks_DefaultsToEmpty()
    {
        var config = new TweakConfig();
        Assert.Empty(config.Tweaks);
    }

    [Fact]
    public void Import_EmptyJson_ReturnsNull()
    {
        var path = Path.Combine(_tempDir, "empty-content.json");
        File.WriteAllText(path, "");
        var result = ConfigExporter.Import(path);
        Assert.Null(result);
    }

    [Fact]
    public void Import_DictWithoutTweaksKey_DeserialisesAsTweakConfig()
    {
        // The full-format TweakConfig deserializer accepts any JSON object,
        // so a dict without a "tweaks" key returns a TweakConfig with empty Tweaks.
        var path = Path.Combine(_tempDir, "no-tweaks-key.json");
        File.WriteAllText(path, """{"name": "test", "other": 42}""");
        var result = ConfigExporter.Import(path);
        Assert.NotNull(result);
        Assert.Empty(result!.Tweaks);
    }

    [Fact]
    public void Import_EmptyArrayFormat_ReturnsEmptyTweaks()
    {
        var path = Path.Combine(_tempDir, "empty-array.json");
        File.WriteAllText(path, "[]");
        var imported = ConfigExporter.Import(path);
        Assert.NotNull(imported);
        Assert.Empty(imported!.Tweaks);
    }

    [Fact]
    public void Validate_EmptyConfig_ReturnsBothEmpty()
    {
        var engine = new TweakEngine();
        var config = new TweakConfig { Tweaks = [] };
        var (valid, unknown) = ConfigExporter.Validate(config, engine);
        Assert.Empty(valid);
        Assert.Empty(unknown);
    }

    [Fact]
    public void Validate_AllValidIds_ReturnsNoUnknown()
    {
        var engine = new TweakEngine();
        engine.Register([
            new TweakDef
            {
                Id = "v1",
                Label = "V1",
                Category = "T",
                ApplyOps = [RegOp.SetDword(@"HKCU\Test", "V", 1)],
            },
            new TweakDef
            {
                Id = "v2",
                Label = "V2",
                Category = "T",
                ApplyOps = [RegOp.SetDword(@"HKCU\Test", "V", 2)],
            },
        ]);
        var config = new TweakConfig { Tweaks = ["v1", "v2"] };
        var (valid, unknown) = ConfigExporter.Validate(config, engine);
        Assert.Equal(2, valid.Count);
        Assert.Empty(unknown);
    }

    [Fact]
    public void Validate_AllUnknownIds_ReturnsNoValid()
    {
        var engine = new TweakEngine();
        var config = new TweakConfig { Tweaks = ["no-such-a", "no-such-b"] };
        var (valid, unknown) = ConfigExporter.Validate(config, engine);
        Assert.Empty(valid);
        Assert.Equal(2, unknown.Count);
    }

    [Fact]
    public void ExportAndImport_PreservesAllFields()
    {
        var path = Path.Combine(_tempDir, "all-fields.json");
        ConfigExporter.Export(path, ["one", "two", "three"], "Full Config", "Full desc");
        var imported = ConfigExporter.Import(path);
        Assert.NotNull(imported);
        Assert.Equal("Full Config", imported!.Name);
        Assert.Equal("Full desc", imported.Description);
        Assert.Equal(3, imported.Tweaks.Count);
    }
}
