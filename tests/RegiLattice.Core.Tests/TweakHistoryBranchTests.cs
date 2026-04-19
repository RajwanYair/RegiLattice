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
public sealed class TweakHistoryBranchTests : IDisposable
{
    public TweakHistoryBranchTests() => TweakHistory.Reset();

    public void Dispose() => TweakHistory.Reset();

    [Fact]
    public async Task ExportToJsonAsync_ValidPath_WritesFile()
    {
        // Normal export covering the F-branch of the ?? operator (non-null directory)
        TweakHistory.RecordApply("some-tweak", TweakResult.Applied);
        string path = Path.Combine(Path.GetTempPath(), $"history_bc6_{Guid.NewGuid():N}.json");
        try
        {
            await TweakHistory.ExportToJsonAsync(path);
            Assert.True(File.Exists(path));
        }
        finally
        {
            string abs = Path.GetFullPath(path);
            if (File.Exists(abs))
                File.Delete(abs);
        }
    }

    [Fact]
    public void LoadList_NullJsonFile_ReturnsEmptyList()
    {
        // Reset wipes cache and file. Write "null" to history file path.
        // Calling Recent() → LoadList → Deserialize returns null → ?? [] fires.
        TweakHistory.Reset();
        var filePathField = typeof(TweakHistory).GetField(
            "FilePath",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static
        )!;
        var filePath = (string)filePathField.GetValue(null)!;
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
        File.WriteAllText(filePath, "null");
        var recent = TweakHistory.Recent();
        Assert.Empty(recent);
    }
}

// ── 9. ConflictDetector — CheckForId matching Id2 branch ─────────────────────

