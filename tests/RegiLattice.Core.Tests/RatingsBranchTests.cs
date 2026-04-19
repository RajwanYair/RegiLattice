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
public sealed class RatingsFileExistsBranchTests : IDisposable
{
    private readonly string _filePath;

    public RatingsFileExistsBranchTests()
    {
        _filePath = (string)typeof(Ratings).GetField("FilePath", BindingFlags.NonPublic | BindingFlags.Static)!.GetValue(null)!;

        if (File.Exists(_filePath))
            File.Delete(_filePath);
    }

    public void Dispose()
    {
        if (File.Exists(_filePath))
            File.Delete(_filePath);
    }

    [Fact]
    public void AllRatings_AfterRate_FileExistsBranchIsTaken()
    {
        // Rate() creates the ratings file.
        Ratings.Rate("bc7-test-tweak", 4);

        // Subsequent AllRatings() finds file exists → F-branch of if (!File.Exists(FilePath)).
        var all = Ratings.AllRatings();
        Assert.True(all.ContainsKey("bc7-test-tweak"));
    }
}

// ── 5. HealthScoreService — empty engine → totalWeight == 0 (L191 T-branch) ──
//    HealthScoreService.cs ScoreBucket L191: `if (totalWeight == 0) return 0;`
//    Existing tests use RegisterBuiltins() so every bucket has tweaks → totalWeight > 0 → F-branch.
//    Empty engine has no tweaks → foreach does nothing → totalWeight stays 0 → T-branch.

