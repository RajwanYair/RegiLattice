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

// ── 1. Analytics — GetStats when file exists (L49 F-branch) ──────────────────
//    analytics.cs L49: `if (!File.Exists(FilePath))` — F-branch (file exists → try read)
//    Existing tests always call Reset() first (deletes file) → T-branch covered.
//    This test writes a file first so the F-branch is taken.

[Collection("Analytics")]
public sealed class AnalyticsFileExistsBranchTests : IDisposable
{
    public AnalyticsFileExistsBranchTests() => Analytics.Reset();

    public void Dispose() => Analytics.Reset();

    [Fact]
    public void GetStats_WhenFileExists_LoadsFromDisk()
    {
        var filePathField = typeof(Analytics).GetField("FilePath", BindingFlags.NonPublic | BindingFlags.Static)!;
        var fp = (string)filePathField.GetValue(null)!;

        // Write valid analytics JSON so File.Exists returns true.
        Directory.CreateDirectory(Path.GetDirectoryName(fp)!);
        File.WriteAllText(
            fp,
            """{"total_applies":7,"total_removes":2,"total_errors":0,"total_sessions":1,"most_applied":{},"most_removed":{},"error_counts":{},"last_session":0}"""
        );

        // Null the in-memory cache so GetStats() re-reads from disk (hits the F-branch).
        typeof(Analytics).GetField("_cache", BindingFlags.NonPublic | BindingFlags.Static)!.SetValue(null, null);

        var data = Analytics.GetStats();
        Assert.Equal(7, data.TotalApplies);
    }
}

// ── 2. SnapshotManager — null JSON → ?? [] (L32 T-branch) ────────────────────
//    SnapshotManager.cs L32:
//       `return JsonSerializer.Deserialize<Dictionary<string,string>>(json) ?? [];`
//    Existing tests always pass valid JSON → Deserialize returns non-null → F-branch.
//    This test writes "null" so Deserialize returns null → ?? [] fires (T-branch).

