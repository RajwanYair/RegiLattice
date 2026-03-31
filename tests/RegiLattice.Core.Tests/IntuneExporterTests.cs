using System.Text.Json;
using Microsoft.Win32;
using RegiLattice.Core;
using RegiLattice.Core.Models;
using Xunit;

namespace RegiLattice.Core.Tests;

/// <summary>Tests for <see cref="IntuneOmaUriExporter"/> — OMA-URI build + export.</summary>
public sealed class IntuneExporterTests : IDisposable
{
    private readonly string _tempDir;

    public IntuneExporterTests()
    {
        _tempDir = Path.Combine(Path.GetTempPath(), $"rl-intunetest-{Guid.NewGuid()}");
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

    /// <summary>Wraps a single TweakDef in a collection so it can pass to Register(IEnumerable).</summary>
    private static void Reg(TweakEngine e, TweakDef td) => e.Register([td]);

    // ── Build — null / empty guard ───────────────────────────────────────

    [Fact]
    public void Build_NullEngine_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => IntuneOmaUriExporter.Build(null!));
    }

    [Fact]
    public void Build_EmptyEngine_ReturnsDocumentWithNoSettings()
    {
        var engine = new TweakEngine();
        var doc = IntuneOmaUriExporter.Build(engine);

        Assert.Equal(0, doc.MappedCount);
        Assert.Equal(0, doc.UnmappedCount);
        Assert.Empty(doc.Settings);
    }

    // ── DWORD → Integer mapping ──────────────────────────────────────────

    [Fact]
    public void Build_WithDwordHklmTweak_MapsToIntegerDataType()
    {
        var engine = new TweakEngine();
        Reg(
            engine,
            new TweakDef
            {
                Id = "test-dword-hklm",
                Label = "Test DWORD HKLM",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Test\Values", "TestValue", 1)],
                RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Test\Values", "TestValue")],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Test\Values", "TestValue", 1)],
            }
        );

        var doc = IntuneOmaUriExporter.Build(engine);

        Assert.Equal(1, doc.MappedCount);
        Assert.Equal("Integer", doc.Settings[0].DataType);
        Assert.Equal("1", doc.Settings[0].Value);
        Assert.Equal("test-dword-hklm", doc.Settings[0].TweakId);
    }

    // ── String → String mapping ──────────────────────────────────────────

    [Fact]
    public void Build_WithStringHklmTweak_MapsToStringDataType()
    {
        var engine = new TweakEngine();
        Reg(
            engine,
            new TweakDef
            {
                Id = "test-string-hklm",
                Label = "Test REG_SZ HKLM",
                Category = "Test",
                ApplyOps = [RegOp.SetString(@"HKLM\SOFTWARE\Test\Values", "DnsPolicy", "SecureOnly")],
                RemoveOps = [RegOp.DeleteValue(@"HKLM\SOFTWARE\Test\Values", "DnsPolicy")],
                DetectOps = [RegOp.CheckString(@"HKLM\SOFTWARE\Test\Values", "DnsPolicy", "SecureOnly")],
            }
        );

        var doc = IntuneOmaUriExporter.Build(engine);

        Assert.Equal(1, doc.MappedCount);
        Assert.Equal("String", doc.Settings[0].DataType);
        Assert.Equal("SecureOnly", doc.Settings[0].Value);
    }

    // ── ExpandString → String mapping ────────────────────────────────────

    [Fact]
    public void Build_WithExpandStringTweak_MapsToStringDataType()
    {
        var engine = new TweakEngine();
        Reg(
            engine,
            new TweakDef
            {
                Id = "test-expand-hklm",
                Label = "Test REG_EXPAND_SZ HKLM",
                Category = "Test",
                ApplyOps = [RegOp.SetExpandString(@"HKLM\SOFTWARE\Test\Values", "DataPath", @"%SystemRoot%\Logs")],
                RemoveOps = [RegOp.DeleteValue(@"HKLM\SOFTWARE\Test\Values", "DataPath")],
            }
        );

        var doc = IntuneOmaUriExporter.Build(engine);

        Assert.Equal(1, doc.MappedCount);
        Assert.Equal("String", doc.Settings[0].DataType);
        Assert.Equal(@"%SystemRoot%\Logs", doc.Settings[0].Value);
    }

    // ── QWord → Integer mapping ──────────────────────────────────────────

    [Fact]
    public void Build_WithQwordTweak_MapsToIntegerDataType()
    {
        var engine = new TweakEngine();
        Reg(
            engine,
            new TweakDef
            {
                Id = "test-qword-hklm",
                Label = "Test QWORD HKLM",
                Category = "Test",
                ApplyOps = [RegOp.SetQword(@"HKLM\SOFTWARE\Test\Values", "LargeCounter", 9999999999L)],
                RemoveOps = [RegOp.DeleteValue(@"HKLM\SOFTWARE\Test\Values", "LargeCounter")],
            }
        );

        var doc = IntuneOmaUriExporter.Build(engine);

        Assert.Equal(1, doc.MappedCount);
        Assert.Equal("Integer", doc.Settings[0].DataType);
        Assert.Equal("9999999999", doc.Settings[0].Value);
    }

    // ── OMA-URI path normalization ───────────────────────────────────────

    [Fact]
    public void Build_HklmPath_BuildsCorrectOmaUri()
    {
        var engine = new TweakEngine();
        Reg(
            engine,
            new TweakDef
            {
                Id = "test-oma-uri-path-hklm",
                Label = "Test OMA-URI path",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "AllowTelemetry", 0)],
                RemoveOps = [RegOp.DeleteValue(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "AllowTelemetry")],
            }
        );

        var doc = IntuneOmaUriExporter.Build(engine);

        Assert.Equal(1, doc.MappedCount);
        Assert.Equal("./Device/Vendor/MSFT/Registry/HKLM/SOFTWARE/Policies/Microsoft/Windows/DataCollection/AllowTelemetry", doc.Settings[0].OmaUri);
    }

    [Fact]
    public void Build_FullHkeyLocalMachinePath_NormalizedToHklm()
    {
        var engine = new TweakEngine();
        Reg(
            engine,
            new TweakDef
            {
                Id = "test-full-hklm-path",
                Label = "Test full HKEY_LOCAL_MACHINE path",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Test\Sub\Key", "MyValue", 1)],
                RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Test\Sub\Key", "MyValue")],
            }
        );

        var doc = IntuneOmaUriExporter.Build(engine);

        Assert.Equal(1, doc.MappedCount);
        Assert.Equal("./Device/Vendor/MSFT/Registry/HKLM/SOFTWARE/Test/Sub/Key/MyValue", doc.Settings[0].OmaUri);
    }

    // ── Non-registry tweaks go to unmapped ────────────────────────────────

    [Fact]
    public void Build_NonRegistryTweak_GoesToUnmappedList()
    {
        var engine = new TweakEngine();
        Reg(
            engine,
            new TweakDef
            {
                Id = "test-ps-tweak",
                Label = "Test PowerShell Tweak",
                Category = "PowerShell Tweaks",
                KindHint = TweakKind.PowerShell,
                ApplyAction = _ => { },
            }
        );

        var doc = IntuneOmaUriExporter.Build(engine);

        Assert.Equal(0, doc.MappedCount);
        Assert.Equal(1, doc.UnmappedCount);
        Assert.NotNull(doc.UnmappedTweaks);
        Assert.Equal("test-ps-tweak", doc.UnmappedTweaks![0].TweakId);
    }

    // ── machineOnly=true excludes HKCU ────────────────────────────────────

    [Fact]
    public void Build_HkcuTweak_ExcludedWhenMachineOnlyIsTrue()
    {
        var engine = new TweakEngine();
        Reg(
            engine,
            new TweakDef
            {
                Id = "test-hkcu-filtered",
                Label = "Test HKCU (machine-only filter)",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\Test\Values", "CuValue", 0)],
                RemoveOps = [RegOp.DeleteValue(@"HKCU\Software\Test\Values", "CuValue")],
            }
        );

        var doc = IntuneOmaUriExporter.Build(engine, machineOnly: true);

        // HKCU tweak has no HKLM ops → goes to unmapped
        Assert.Equal(0, doc.MappedCount);
        Assert.Equal(1, doc.UnmappedCount);
    }

    [Fact]
    public void Build_HkcuTweak_IncludedWhenMachineOnlyIsFalse()
    {
        var engine = new TweakEngine();
        Reg(
            engine,
            new TweakDef
            {
                Id = "test-hkcu-included",
                Label = "Test HKCU (all-scopes)",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\Test\Values", "CuValue", 1)],
                RemoveOps = [RegOp.DeleteValue(@"HKCU\Software\Test\Values", "CuValue")],
            }
        );

        var doc = IntuneOmaUriExporter.Build(engine, machineOnly: false);

        Assert.Equal(1, doc.MappedCount);
        Assert.StartsWith("./User/Vendor/MSFT/Registry/HKCU/", doc.Settings[0].OmaUri);
    }

    // ── Delete / Check ops are NOT mapped ────────────────────────────────

    [Fact]
    public void Build_TweakWithOnlyDeleteApplyOps_GoesToUnmapped()
    {
        // A tweak whose ApplyOps is a single DeleteValue is unusual but valid.
        // DeleteValue is not a SetValue → cannot be mapped to OMA-URI.
        var engine = new TweakEngine();
        Reg(
            engine,
            new TweakDef
            {
                Id = "test-delete-only",
                Label = "Test delete-only apply",
                Category = "Test",
                ApplyOps = [RegOp.DeleteValue(@"HKLM\SOFTWARE\Test\Values", "OldValue")],
                RemoveOps = [],
            }
        );

        var doc = IntuneOmaUriExporter.Build(engine);

        // No SetValue ops → goes to unmapped
        Assert.Equal(0, doc.MappedCount);
        Assert.Equal(1, doc.UnmappedCount);
    }

    // ── Export writes valid JSON file ─────────────────────────────────────

    [Fact]
    public void Export_WritesValidUtf8JsonFile()
    {
        var engine = new TweakEngine();
        Reg(
            engine,
            new TweakDef
            {
                Id = "test-export-to-file",
                Label = "Test Export",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKLM\SOFTWARE\Test\Export", "Flag", 1)],
                RemoveOps = [RegOp.DeleteValue(@"HKLM\SOFTWARE\Test\Export", "Flag")],
            }
        );

        string outPath = Path.Combine(_tempDir, "intune-export.json");
        IntuneOmaUriExporter.Export(engine, outPath);

        Assert.True(File.Exists(outPath));
        string json = File.ReadAllText(outPath, System.Text.Encoding.UTF8);
        var doc = JsonSerializer.Deserialize<IntuneExportDocument>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        Assert.NotNull(doc);
        Assert.Single(doc!.Settings);
    }

    [Fact]
    public void Export_NullEngine_ThrowsArgumentNullException()
    {
        string outPath = Path.Combine(_tempDir, "should-not-create.json");
        Assert.Throws<ArgumentNullException>(() => IntuneOmaUriExporter.Export(null!, outPath));
    }

    [Fact]
    public void Export_NullOrWhitespacePath_ThrowsArgumentException()
    {
        var engine = new TweakEngine();
        Assert.Throws<ArgumentException>(() => IntuneOmaUriExporter.Export(engine, "  "));
    }

    // ── Document metadata ─────────────────────────────────────────────────

    [Fact]
    public void IntuneExportDocument_MappedAndUnmappedCounts_AreConsistent()
    {
        var engine = new TweakEngine();
        Reg(
            engine,
            new TweakDef
            {
                Id = "test-mapped-1",
                Label = "Mapped",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKLM\SOFTWARE\Test", "A", 0)],
                RemoveOps = [RegOp.DeleteValue(@"HKLM\SOFTWARE\Test", "A")],
            }
        );
        Reg(
            engine,
            new TweakDef
            {
                Id = "test-unmapped-1",
                Label = "Unmapped",
                Category = "PowerShell Tweaks",
                KindHint = TweakKind.PowerShell,
                ApplyAction = _ => { },
            }
        );

        var doc = IntuneOmaUriExporter.Build(engine);

        Assert.Equal(1, doc.MappedCount);
        Assert.Equal(1, doc.UnmappedCount);
    }

    // ── Multiple ApplyOps in one tweak ────────────────────────────────────

    [Fact]
    public void Build_MultipleApplyOps_ProducesOneSettingPerMappableOp()
    {
        var engine = new TweakEngine();
        Reg(
            engine,
            new TweakDef
            {
                Id = "test-multi-ops",
                Label = "Multi Ops",
                Category = "Test",
                ApplyOps =
                [
                    RegOp.SetDword(@"HKLM\SOFTWARE\Test", "Val1", 1),
                    RegOp.SetDword(@"HKLM\SOFTWARE\Test", "Val2", 2),
                    RegOp.SetString(@"HKLM\SOFTWARE\Test", "StrVal", "hello"),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(@"HKLM\SOFTWARE\Test", "Val1"),
                    RegOp.DeleteValue(@"HKLM\SOFTWARE\Test", "Val2"),
                    RegOp.DeleteValue(@"HKLM\SOFTWARE\Test", "StrVal"),
                ],
            }
        );

        var doc = IntuneOmaUriExporter.Build(engine);

        Assert.Equal(3, doc.MappedCount);
        Assert.Equal(0, doc.UnmappedCount);
        Assert.All(doc.Settings, s => Assert.Equal("test-multi-ops", s.TweakId));
    }

    // ── Builtin integration smoke ─────────────────────────────────────────

    [Fact]
    public void Build_WithBuiltins_ReturnsManyMappedSettings()
    {
        var engine = new TweakEngine();
        engine.RegisterBuiltins();

        var doc = IntuneOmaUriExporter.Build(engine, machineOnly: true);

        // Thousands of HKLM Registry tweaks exist — expect many mapped settings
        Assert.True(doc.MappedCount > 100, $"Expected >100 mapped, got {doc.MappedCount}");
    }
}
