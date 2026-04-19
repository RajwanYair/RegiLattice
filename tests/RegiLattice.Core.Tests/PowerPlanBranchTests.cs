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
public sealed class PowerPlanManagerLiveTests
{
    [Fact]
    public void GetAllPlans_ReturnsAtLeastOnePlan()
    {
        // powercfg.exe is present on all Windows installations.
        var plans = PowerPlanManager.GetAllPlans();
        Assert.NotEmpty(plans);
    }

    [Fact]
    public void GetAllPlans_AllEntries_HaveNonEmptyName()
    {
        var plans = PowerPlanManager.GetAllPlans();
        Assert.All(plans, p => Assert.False(string.IsNullOrWhiteSpace(p.Name)));
    }

    [Fact]
    public void GetActivePlanGuid_ReturnsNonNullGuid()
    {
        Guid? active = PowerPlanManager.GetActivePlanGuid();
        Assert.NotNull(active);
        Assert.NotEqual(Guid.Empty, active.Value);
    }

    [Fact]
    public void GetAllPlans_ExactlyOneIsActive()
    {
        var plans = PowerPlanManager.GetAllPlans();
        int activeCount = plans.Count(p => p.IsActive);
        // There should be exactly one active plan
        Assert.Equal(1, activeCount);
    }
}
