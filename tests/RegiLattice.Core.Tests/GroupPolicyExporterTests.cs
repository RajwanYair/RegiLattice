// tests/RegiLattice.Core.Tests/GroupPolicyExporterTests.cs
// Sprint coverage — GroupPolicyExporter ADMX/ADML file generation.

using System.Xml.Linq;
using RegiLattice.Core;
using RegiLattice.Core.Models;
using RegiLattice.Core.Registry;
using Xunit;

namespace RegiLattice.Core.Tests;

/// <summary>Tests for GroupPolicyExporter: file creation, XML validity, content filtering.</summary>
public sealed class GroupPolicyExporterTests : IDisposable
{
    private readonly string _tempDir;

    public GroupPolicyExporterTests()
    {
        _tempDir = Path.Combine(Path.GetTempPath(), $"rl-gpo-{Guid.NewGuid():N}");
        Directory.CreateDirectory(_tempDir);
    }

    public void Dispose()
    {
        try
        {
            Directory.Delete(_tempDir, recursive: true);
        }
        catch
        { /* best-effort cleanup */
        }
    }

    private static TweakEngine BuildEngineWithGpTweaks()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([
            new TweakDef
            {
                Id = "test-gpo-disable-telemetry",
                Label = "Disable Telemetry (GP)",
                Category = "Privacy",
                Description = "Turns off telemetry via Group Policy.",
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Test", "AllowTelemetry", 0)],
                RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Test", "AllowTelemetry")],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Test", "AllowTelemetry", 0)],
            },
            new TweakDef
            {
                Id = "test-gpo-disable-cortana",
                Label = "Disable Cortana (GP)",
                Category = "Cortana & Search",
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsSearch", "AllowCortana", 0)],
                RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsSearch", "AllowCortana")],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsSearch", "AllowCortana", 0)],
            },
        ]);
        return engine;
    }

    // ── File creation ────────────────────────────────────────────────────────

    [Fact]
    public void Export_CreatesAdmxFile()
    {
        var engine = BuildEngineWithGpTweaks();
        var admxPath = Path.Combine(_tempDir, "RegiLattice.admx");

        GroupPolicyExporter.Export(engine, admxPath);

        Assert.True(File.Exists(admxPath));
    }

    [Fact]
    public void Export_CreatesAdmlFileInEnUsSubdirectory()
    {
        var engine = BuildEngineWithGpTweaks();
        var admxPath = Path.Combine(_tempDir, "RegiLattice.admx");

        GroupPolicyExporter.Export(engine, admxPath);

        Assert.True(File.Exists(Path.Combine(_tempDir, "en-US", "RegiLattice.adml")));
    }

    // ── XML validity ─────────────────────────────────────────────────────────

    [Fact]
    public void Export_AdmxFile_ContainsValidXml()
    {
        var engine = BuildEngineWithGpTweaks();
        var admxPath = Path.Combine(_tempDir, "RegiLattice.admx");
        GroupPolicyExporter.Export(engine, admxPath);

        // Read as string to avoid encoding="utf-16" vs UTF-8 file conflict
        string content = File.ReadAllText(admxPath).TrimStart('\uFEFF');
        var doc = XDocument.Parse(content); // throws if invalid XML
        Assert.NotNull(doc.Root);
    }

    [Fact]
    public void Export_AdmlFile_ContainsValidXml()
    {
        var engine = BuildEngineWithGpTweaks();
        var admxPath = Path.Combine(_tempDir, "RegiLattice.admx");
        GroupPolicyExporter.Export(engine, admxPath);

        var admlPath = Path.Combine(_tempDir, "en-US", "RegiLattice.adml");
        string content = File.ReadAllText(admlPath).TrimStart('\uFEFF');
        var doc = XDocument.Parse(content);
        Assert.NotNull(doc.Root);
    }

    // ── Content ───────────────────────────────────────────────────────────────

    [Fact]
    public void Export_AdmxFile_ContainsSchemaVersion()
    {
        var engine = BuildEngineWithGpTweaks();
        var admxPath = Path.Combine(_tempDir, "RegiLattice.admx");
        GroupPolicyExporter.Export(engine, admxPath);

        string content = File.ReadAllText(admxPath);
        Assert.Contains("1.192", content);
    }

    [Fact]
    public void Export_AdmxFile_ContainsSanitizedTweakIds()
    {
        var engine = BuildEngineWithGpTweaks();
        var admxPath = Path.Combine(_tempDir, "RegiLattice.admx");
        GroupPolicyExporter.Export(engine, admxPath);

        // Sanitize replaces '-' with '_'
        string content = File.ReadAllText(admxPath);
        Assert.Contains("test_gpo_disable_telemetry", content);
        Assert.Contains("test_gpo_disable_cortana", content);
    }

    [Fact]
    public void Export_AdmxFile_ContainsTweakLabels()
    {
        var engine = BuildEngineWithGpTweaks();
        var admxPath = Path.Combine(_tempDir, "RegiLattice.admx");
        GroupPolicyExporter.Export(engine, admxPath);

        // Labels appear in the .adml string table
        string admlContent = File.ReadAllText(Path.Combine(_tempDir, "en-US", "RegiLattice.adml"));
        Assert.Contains("Disable Telemetry (GP)", admlContent);
        Assert.Contains("Disable Cortana (GP)", admlContent);
    }

    // ── Empty engine ──────────────────────────────────────────────────────────

    [Fact]
    public void Export_EmptyEngine_CreatesValidFiles()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        var admxPath = Path.Combine(_tempDir, "Empty.admx");

        GroupPolicyExporter.Export(engine, admxPath);

        Assert.True(File.Exists(admxPath));
        string content = File.ReadAllText(admxPath).TrimStart('\uFEFF');
        var doc = XDocument.Parse(content); // must be valid even with no tweaks
        Assert.NotNull(doc.Root);
    }

    // ── Kind filtering ────────────────────────────────────────────────────────

    [Fact]
    public void Export_FiltersOutRegistryKindTweaks()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        // Registry-kind tweak — path has no \Policies\ segment
        engine.Register([TestHelpers.MakeTweak("test-reg-tweak", "Performance")]);
        // GroupPolicy-kind tweak — path contains \Policies\
        engine.Register([
            new TweakDef
            {
                Id = "test-gpo-only",
                Label = "GPO Only Tweak",
                Category = "Security",
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Test", "SecureVal", 1)],
            },
        ]);

        var admxPath = Path.Combine(_tempDir, "Filtered.admx");
        GroupPolicyExporter.Export(engine, admxPath);
        string content = File.ReadAllText(admxPath);

        Assert.Contains("test_gpo_only", content);
        Assert.DoesNotContain("test_reg_tweak", content);
    }

    // ── Argument validation ───────────────────────────────────────────────────

    [Fact]
    public void Export_NullEngine_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => GroupPolicyExporter.Export(null!, Path.Combine(_tempDir, "test.admx")));
    }

    [Fact]
    public void Export_EmptyPath_ThrowsArgumentException()
    {
        var engine = BuildEngineWithGpTweaks();
        Assert.Throws<ArgumentException>(() => GroupPolicyExporter.Export(engine, ""));
    }

    [Fact]
    public void Export_WhitespacePath_ThrowsArgumentException()
    {
        var engine = BuildEngineWithGpTweaks();
        Assert.Throws<ArgumentException>(() => GroupPolicyExporter.Export(engine, "   "));
    }
}
