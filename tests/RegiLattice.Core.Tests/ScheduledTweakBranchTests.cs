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
public sealed class ScheduledTweakServiceBranchTests : IDisposable
{
    private static readonly string SchedulesPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "RegiLattice",
        "scheduled-tweaks.json"
    );

    private readonly string? _backup;

    public ScheduledTweakServiceBranchTests()
    {
        if (File.Exists(SchedulesPath))
            _backup = File.ReadAllText(SchedulesPath);
    }

    public void Dispose()
    {
        if (_backup is not null)
            File.WriteAllText(SchedulesPath, _backup);
        else if (File.Exists(SchedulesPath))
            File.Delete(SchedulesPath);
    }

    [Fact]
    public void Load_NullJsonFile_SchedulesIsEmpty_TBranch()
    {
        // "null" → Deserialize<List<TweakSchedule>>("null") = null → ?? [] fires.
        Directory.CreateDirectory(Path.GetDirectoryName(SchedulesPath)!);
        File.WriteAllText(SchedulesPath, "null");

        var svc = new ScheduledTweakService();
        svc.Load();
        Assert.Empty(svc.Schedules);
    }

    [Fact]
    public void Load_EmptyArrayJson_SchedulesIsEmpty_FBranch()
    {
        // "[]" → Deserialize<List<TweakSchedule>>("[]") = empty List (not null) → ?? not triggered.
        Directory.CreateDirectory(Path.GetDirectoryName(SchedulesPath)!);
        File.WriteAllText(SchedulesPath, "[]");

        var svc = new ScheduledTweakService();
        svc.Load();
        Assert.Empty(svc.Schedules);
    }
}
