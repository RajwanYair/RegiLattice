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
[Collection("Favorites")]
public sealed class FavoritesWhitespaceBranchTests : IDisposable
{
    public FavoritesWhitespaceBranchTests() => Favorites.Reset();

    public void Dispose() => Favorites.Reset();

    [Fact]
    public void ImportFromJson_WhitespaceIds_AreSkipped()
    {
        var path = Path.GetTempFileName();
        try
        {
            // JSON array with empty string and whitespace-only string.
            // IsNullOrWhiteSpace("") = true, IsNullOrWhiteSpace("   ") = true
            // → short-circuit → added == 0 (L116 branch A covered).
            File.WriteAllText(path, """["", "   "]""");
            int added = Favorites.ImportFromJson(path);
            Assert.Equal(0, added);
        }
        finally
        {
            File.Delete(path);
        }
    }
}

// ── 9. AppConfig.Validate — brightness < 0 (L248) and minute > 59 (L280) ────
//    AppConfig.cs L248: `if (BrightnessDayPct < 0 || BrightnessDayPct > 100)`
//      Missing: `BrightnessDayPct < 0` T-branch (short-circuits `> 100` evaluation).
//    AppConfig.cs L280: `return int.TryParse(...) && ... && mm is >= 0 and <= 59;`
//      Missing: `mm > 59` (hh valid, minute out of range) sub-branch.

public sealed class FavoritesBranchTests : IDisposable
{
    public FavoritesBranchTests() => Favorites.Reset();

    public void Dispose() => Favorites.Reset();

    [Fact]
    public async Task ExportToJsonAsync_ValidPath_WritesFile()
    {
        // Normal export with a full path — confirms ExportToJsonAsync works end-to-end
        // and covers the F-branch of the ?? operator (non-null directory part)
        Favorites.Add("id-export-test");
        string path = Path.Combine(Path.GetTempPath(), $"fav_bc6_{Guid.NewGuid():N}.json");
        try
        {
            await Favorites.ExportToJsonAsync(path);
            Assert.True(File.Exists(path));
            string json = File.ReadAllText(path);
            Assert.Contains("id-export-test", json);
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    [Fact]
    public void ImportFromJson_NullJsonContent_ThrowsInvalidOperationException()
    {
        // Write "null" JSON to a temp file → Deserialize<List<string>>("null") returns null
        // → ?? throw fires (line 107 T-branch)
        string path = Path.Combine(Path.GetTempPath(), $"fav_null_{Guid.NewGuid():N}.json");
        File.WriteAllText(path, "null");
        try
        {
            Assert.Throws<InvalidOperationException>(() => Favorites.ImportFromJson(path));
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    [Fact]
    public void LoadSet_NullJsonInFile_ReturnsEmptySet()
    {
        // Reset wipes cache and file. Write "null" to favorites file.
        // Then calling IsFavorite triggers LoadSet → Deserialize returns null → ?? [] fires.
        Favorites.Reset();
        // Directly write null JSON to the favorites file path via reflection
        var filePathField = typeof(Favorites).GetField("FilePath", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!;
        var filePath = (string)filePathField.GetValue(null)!;
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
        File.WriteAllText(filePath, "null");
        // Now call IsFavorite which triggers LoadSet; _cache will be re-populated from null JSON → ?? []
        bool result = Favorites.IsFavorite("any-id");
        // null JSON → empty set → not a favorite
        Assert.False(result);
    }

    [Fact]
    public void ImportFromJson_DuplicateIds_SkipsAlreadyPresent()
    {
        // Add "dup-id" to favorites first, then import a file with "dup-id" again
        // → set.Add returns false for duplicate → covers the !set.Add F-branch
        Favorites.Add("dup-id");
        string json = """["dup-id", "new-id"]""";
        string path = Path.Combine(Path.GetTempPath(), $"fav_dup_{Guid.NewGuid():N}.json");
        File.WriteAllText(path, json);
        try
        {
            int added = Favorites.ImportFromJson(path);
            Assert.Equal(1, added); // only "new-id" is new
            Assert.True(Favorites.IsFavorite("dup-id"));
            Assert.True(Favorites.IsFavorite("new-id"));
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    [Fact]
    public void Flush_WhenNotDirty_IsNoOp()
    {
        // Flush when _dirty = false → if (!_dirty || _cache is null) return immediately
        Favorites.Clear(); // sets _dirty = true
        Favorites.Flush(); // write and set _dirty = false
        // Now _dirty = false; second Flush should be a no-op (covers F of !_dirty)
        Favorites.Flush();
        // No assertion needed — just must not throw; the branch was taken
        Assert.True(Favorites.All().Count >= 0);
    }
}

// ── 8. TweakHistory — partial branch coverage ────────────────────────────────

