#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Win32;
using RegiLattice.Core;
using RegiLattice.Core.Models;
using RegiLattice.Core.Plugins;
using RegiLattice.Core.Registry;
using RegiLattice.Core.Services;
using Xunit;

namespace RegiLattice.Core.Tests;
public sealed class ConfigExporterFormat3BranchTests : IDisposable
{
    private readonly string _tempDir;

    public ConfigExporterFormat3BranchTests()
    {
        _tempDir = Path.Combine(Path.GetTempPath(), $"RegiLattice_BC6_CFGExp_{Guid.NewGuid():N}");
        Directory.CreateDirectory(_tempDir);
    }

    public void Dispose() => Directory.Delete(_tempDir, recursive: true);

    // JSON has "tweaks" array but "name" is a number → fails TweakConfig (type mismatch),
    // falls to Format 3 (dict) → TryGetValue("tweaks") = true, ids is List → returns config.
    [Fact]
    public void Import_DictFormat_InvalidNameField_ReturnsConfigViaDictPath()
    {
        string json = """{"tweaks":["id-a","id-b"],"name":123}""";
        string file = Path.Combine(_tempDir, "cfg1.json");
        File.WriteAllText(file, json);

        var result = ConfigExporter.Import(file);
        Assert.NotNull(result);
        Assert.Contains("id-a", result.Tweaks);
        Assert.Contains("id-b", result.Tweaks);
    }

    // JSON has "tweaks": null → dict found, TryGetValue = true, ids = null → return null.
    [Fact]
    public void Import_DictFormat_TweaksNull_ReturnsNull()
    {
        string json = """{"tweaks":null,"name":123}""";
        string file = Path.Combine(_tempDir, "cfg2.json");
        File.WriteAllText(file, json);

        var result = ConfigExporter.Import(file);
        // ids is null → return null (line 108 F-branch)
        Assert.Null(result);
    }

    // JSON has no "tweaks" key → dict found, TryGetValue = false → return null.
    [Fact]
    public void Import_DictFormat_NoTweaksKey_ReturnsNull()
    {
        string json = """{"other":"val","name":123}""";
        string file = Path.Combine(_tempDir, "cfg3.json");
        File.WriteAllText(file, json);

        var result = ConfigExporter.Import(file);
        // TryGetValue("tweaks") = false → line 105 F-branch
        Assert.Null(result);
    }
}

// ── 3. TweakEngine — RegisterInstalledPacks with no packs ───────────────────

