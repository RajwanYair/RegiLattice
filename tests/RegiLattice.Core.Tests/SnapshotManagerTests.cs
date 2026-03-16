using System.Text.Json;
using RegiLattice.Core;
using RegiLattice.Core.Models;
using RegiLattice.Core.Registry;
using Xunit;

namespace RegiLattice.Core.Tests;

/// <summary>Direct tests for <see cref="SnapshotManager"/> — save, load, restore snapshot logic.</summary>
public sealed class SnapshotManagerTests
{
    private static TweakEngine CreateEngine(params TweakDef[] tweaks)
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        if (tweaks.Length > 0)
            engine.Register(tweaks);
        return engine;
    }

    private static TweakDef MakeTweak(string id) =>
        new()
        {
            Id = id,
            Label = $"Tweak {id}",
            Category = "Test",
            ApplyOps = [RegOp.SetDword($@"HKCU\Software\{id}", "V", 1)],
            RemoveOps = [RegOp.DeleteValue($@"HKCU\Software\{id}", "V")],
            DetectOps = [RegOp.CheckDword($@"HKCU\Software\{id}", "V", 1)],
        };

    // ── Save ────────────────────────────────────────────────────────────

    [Fact]
    public void Save_CreatesJsonFile()
    {
        var engine = CreateEngine(MakeTweak("snap-1"), MakeTweak("snap-2"));
        var mgr = new SnapshotManager(engine);
        var path = Path.Combine(Path.GetTempPath(), $"snap_test_{Guid.NewGuid()}.json");

        try
        {
            mgr.Save(path);
            Assert.True(File.Exists(path));

            var content = File.ReadAllText(path);
            Assert.False(string.IsNullOrWhiteSpace(content));
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    [Fact]
    public void Save_WritesValidJson()
    {
        var engine = CreateEngine(MakeTweak("snap-json-1"));
        var mgr = new SnapshotManager(engine);
        var path = Path.Combine(Path.GetTempPath(), $"snap_json_{Guid.NewGuid()}.json");

        try
        {
            mgr.Save(path);
            var json = File.ReadAllText(path);
            var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            Assert.NotNull(dict);
            Assert.Contains("snap-json-1", dict.Keys);
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    [Fact]
    public void Save_ContainsAllTweakIds()
    {
        var engine = CreateEngine(MakeTweak("snap-a"), MakeTweak("snap-b"), MakeTweak("snap-c"));
        var mgr = new SnapshotManager(engine);
        var path = Path.Combine(Path.GetTempPath(), $"snap_ids_{Guid.NewGuid()}.json");

        try
        {
            mgr.Save(path);
            var dict = SnapshotManager.Load(path);
            Assert.Equal(3, dict.Count);
            Assert.Contains("snap-a", dict.Keys);
            Assert.Contains("snap-b", dict.Keys);
            Assert.Contains("snap-c", dict.Keys);
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    [Fact]
    public void Save_StatusValuesAreLowercase()
    {
        var engine = CreateEngine(MakeTweak("snap-lower"));
        var mgr = new SnapshotManager(engine);
        var path = Path.Combine(Path.GetTempPath(), $"snap_lower_{Guid.NewGuid()}.json");

        try
        {
            mgr.Save(path);
            var dict = SnapshotManager.Load(path);
            Assert.All(dict.Values, v => Assert.Equal(v, v.ToLowerInvariant()));
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    // ── Load ────────────────────────────────────────────────────────────

    [Fact]
    public void Load_ParsesJsonCorrectly()
    {
        var path = Path.Combine(Path.GetTempPath(), $"snap_load_{Guid.NewGuid()}.json");
        var expected = new Dictionary<string, string>
        {
            ["tweak-1"] = "applied",
            ["tweak-2"] = "notapplied",
            ["tweak-3"] = "unknown",
        };

        try
        {
            File.WriteAllText(path, JsonSerializer.Serialize(expected));
            var loaded = SnapshotManager.Load(path);
            Assert.Equal(3, loaded.Count);
            Assert.Equal("applied", loaded["tweak-1"]);
            Assert.Equal("notapplied", loaded["tweak-2"]);
            Assert.Equal("unknown", loaded["tweak-3"]);
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    [Fact]
    public void Load_EmptySnapshot_ReturnsEmptyDict()
    {
        var path = Path.Combine(Path.GetTempPath(), $"snap_empty_{Guid.NewGuid()}.json");

        try
        {
            File.WriteAllText(path, "{}");
            var loaded = SnapshotManager.Load(path);
            Assert.Empty(loaded);
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    [Fact]
    public void Load_MissingFile_ThrowsIOException()
    {
        Assert.ThrowsAny<IOException>(() => SnapshotManager.Load(@"C:\nonexistent\path\snap.json"));
    }

    // ── Restore ─────────────────────────────────────────────────────────

    [Fact]
    public void Restore_AppliesTweaksMarkedApplied()
    {
        var engine = CreateEngine(MakeTweak("restore-apply"));
        var mgr = new SnapshotManager(engine);
        var path = Path.Combine(Path.GetTempPath(), $"snap_restore_{Guid.NewGuid()}.json");

        try
        {
            var snapshot = new Dictionary<string, string> { ["restore-apply"] = "applied" };
            File.WriteAllText(path, JsonSerializer.Serialize(snapshot));

            var results = mgr.Restore(path, forceCorp: true);
            Assert.Contains("restore-apply", results.Keys);
            Assert.Equal(TweakResult.Applied, results["restore-apply"]);
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    [Fact]
    public void Restore_RemovesTweaksMarkedNotApplied()
    {
        var engine = CreateEngine(MakeTweak("restore-remove"));
        var mgr = new SnapshotManager(engine);
        var path = Path.Combine(Path.GetTempPath(), $"snap_restore_rm_{Guid.NewGuid()}.json");

        try
        {
            var snapshot = new Dictionary<string, string> { ["restore-remove"] = "notapplied" };
            File.WriteAllText(path, JsonSerializer.Serialize(snapshot));

            var results = mgr.Restore(path, forceCorp: true);
            Assert.Contains("restore-remove", results.Keys);
            Assert.Equal(TweakResult.NotApplied, results["restore-remove"]);
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    [Fact]
    public void Restore_UnknownState_ReturnsUnknown()
    {
        var engine = CreateEngine(MakeTweak("restore-unk"));
        var mgr = new SnapshotManager(engine);
        var path = Path.Combine(Path.GetTempPath(), $"snap_restore_unk_{Guid.NewGuid()}.json");

        try
        {
            var snapshot = new Dictionary<string, string> { ["restore-unk"] = "error" };
            File.WriteAllText(path, JsonSerializer.Serialize(snapshot));

            var results = mgr.Restore(path);
            Assert.Contains("restore-unk", results.Keys);
            Assert.Equal(TweakResult.Unknown, results["restore-unk"]);
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    [Fact]
    public void Restore_SkipsMissingTweaks()
    {
        var engine = CreateEngine(MakeTweak("restore-existing"));
        var mgr = new SnapshotManager(engine);
        var path = Path.Combine(Path.GetTempPath(), $"snap_restore_missing_{Guid.NewGuid()}.json");

        try
        {
            var snapshot = new Dictionary<string, string> { ["restore-existing"] = "applied", ["nonexistent-tweak"] = "applied" };
            File.WriteAllText(path, JsonSerializer.Serialize(snapshot));

            var results = mgr.Restore(path);
            Assert.Single(results);
            Assert.Contains("restore-existing", results.Keys);
            Assert.DoesNotContain("nonexistent-tweak", results.Keys);
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    // ── Round-trip ──────────────────────────────────────────────────────

    [Fact]
    public void SaveThenLoad_RoundTrip_PreservesKeys()
    {
        var engine = CreateEngine(MakeTweak("rt-1"), MakeTweak("rt-2"));
        var mgr = new SnapshotManager(engine);
        var path = Path.Combine(Path.GetTempPath(), $"snap_rt_{Guid.NewGuid()}.json");

        try
        {
            mgr.Save(path);
            var loaded = SnapshotManager.Load(path);
            Assert.Equal(2, loaded.Count);
            Assert.Contains("rt-1", loaded.Keys);
            Assert.Contains("rt-2", loaded.Keys);
            Assert.All(
                loaded.Values,
                v => Assert.Contains(v, new[] { "applied", "notapplied", "unknown", "error", "skippedcorp", "skippedbuild", "skippedhw" })
            );
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }
}
