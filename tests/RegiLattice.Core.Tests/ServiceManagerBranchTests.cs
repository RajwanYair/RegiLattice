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
public sealed class ServiceManagerBranchTests
{
    // ServiceManager.GetAllServices() is a read-only WMI/sc.exe query — safe to call.
    [Fact]
    public void GetAllServices_ReturnsCollection()
    {
        var services = ServiceManager.GetAllServices();
        Assert.NotNull(services);
        // On any Windows machine there will be services
        Assert.NotEmpty(services);
    }

    [Fact]
    public void GetAllServices_AllEntries_HaveNonEmptyServiceName()
    {
        var services = ServiceManager.GetAllServices();
        Assert.All(services, s => Assert.False(string.IsNullOrWhiteSpace(s.ServiceName)));
    }

    [Fact]
    public void ServiceEntry_Record_CanBeConstructed()
    {
        var entry = new ServiceEntry(
            "Spooler",
            "Print Spooler",
            "Manages print jobs",
            System.ServiceProcess.ServiceControllerStatus.Running,
            System.ServiceProcess.ServiceStartMode.Automatic,
            CanStop: true,
            CanPauseAndContinue: false
        );
        Assert.Equal("Spooler", entry.ServiceName);
        Assert.Equal("Print Spooler", entry.DisplayName);
        Assert.Equal(System.ServiceProcess.ServiceControllerStatus.Running, entry.Status);
        Assert.Equal(System.ServiceProcess.ServiceStartMode.Automatic, entry.StartType);
        Assert.True(entry.CanStop);
        Assert.False(entry.CanPauseAndContinue);
    }

    [Fact]
    public async Task ExportToCsvAsync_WritesValidCsv()
    {
        string path = Path.Combine(Path.GetTempPath(), $"RL_services_{Guid.NewGuid():N}.csv");
        try
        {
            await ServiceManager.ExportToCsvAsync(path);
            Assert.True(File.Exists(path));
            string content = File.ReadAllText(path);
            // CSV should have a header row
            Assert.Contains("Name", content);
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }
}

// ── merged from BranchCoverage3Tests.cs ──────────────────────────────────

