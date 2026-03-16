using System.Text.Json;
using RegiLattice.Core;
using RegiLattice.Core.Models;
using RegiLattice.Core.Services;
using Xunit;

namespace RegiLattice.Core.Tests;

/// <summary>Tests for ConfigExporter — export, import, validate.</summary>
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
}
