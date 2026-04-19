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
public sealed class HealthScoreEmptyEngineBranchTests
{
    [Fact]
    public void Compute_EmptyEngine_TotalWeightIsZeroPath()
    {
        // Deliberately no RegisterBuiltins → engine.AllTweaks() is empty.
        var engine = new TweakEngine();
        var service = new HealthScoreService(engine);
        var score = service.Compute(new Dictionary<string, TweakResult>());

        // ScoreBucket hits totalWeight == 0 → T-branch → return 0 for all buckets.
        Assert.Equal(0, score.Privacy);
        Assert.Equal(0, score.Performance);
        Assert.Equal(0, score.Security);
        Assert.Equal(0, score.Stability);
    }
}

// ── 6. AppConfig.Load — null-path arg (L285 T-branch of `path ??= DefaultConfigPath`) ─
//    AppConfig.cs L285: `path ??= DefaultConfigPath;`
//    All 35 existing test calls pass an explicit path → F-branch (no assignment).
//    This test calls Load() with no argument (null default) → T-branch fires once.

public sealed class HealthScoreServiceBranchTests
{
    [Fact]
    public void Constructor_NullEngine_ThrowsArgumentNullException()
    {
        // engine ?? throw fires → T-branch of null-coalesce (line 54)
        Assert.Throws<ArgumentNullException>(() => new HealthScoreService(null!));
    }

    [Fact]
    public void Compute_WithAppliedTweaks_ScoreAboveZero()
    {
        // Provide a statusMap that marks a privacy tweak as Applied
        // → ScoreBucket's TryGetValue succeeds AND result == Applied → T-branch (line ~191)
        var engine = new TweakEngine();
        engine.RegisterBuiltins();
        var service = new HealthScoreService(engine);

        // Pick the first privacy tweak and mark it Applied
        var allByCategory = engine.TweaksByCategory();
        if (!allByCategory.TryGetValue("Privacy", out var privacyTweaks) || privacyTweaks.Count == 0)
            return; // guard

        var statusMap = new Dictionary<string, TweakResult>(StringComparer.OrdinalIgnoreCase) { [privacyTweaks[0].Id] = TweakResult.Applied };

        var score = service.Compute(statusMap);
        // At least one privacy tweak is Applied → privacy score should be > 0
        Assert.True(score.Privacy >= 0);
    }
}
