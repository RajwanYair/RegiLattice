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
public sealed class ConflictDetectorId2BranchTests
{
    [Fact]
    public void CheckForId_IdMatchesId2_ReturnsConflict()
    {
        // Get the list of all known conflicts. Find one where an ID appears as Id2.
        var all = ConflictDetector.AllConflicts;
        if (all.Count == 0)
            return; // no conflicts registered; skip

        // Use the Id2 from first conflict pair
        var first = all[0];
        string id2 = first.Id2;

        // Build an applied-ids list that contains Id1 (the "other" conflicting tweak)
        var appliedIds = new[] { first.Id1, id2 };

        // Call ConflictsFor with id2 → exercises the c.Id2 == id true-branch in the static loop
        var conflicts = ConflictDetector.ConflictsFor(id2, appliedIds);
        Assert.NotEmpty(conflicts);
    }
}

// ── 10. HealthScoreService — null-guard and applied-tweak Compute branch ──────

