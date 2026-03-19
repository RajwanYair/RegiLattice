#nullable enable

using System.ServiceProcess;
using RegiLattice.Core.Services;
using Xunit;

namespace RegiLattice.Core.Tests;

/// <summary>Tests for the Sprint-29 ServiceManager service (read-only operations).</summary>
public sealed class ServiceManagerTests
{
    // Fetched once per test-class lifetime so the slow OS enumeration runs only once.
    private static readonly IReadOnlyList<ServiceEntry> _allServices = ServiceManager.GetAllServices();

    [Fact]
    public void GetAllServices_ReturnsNonEmptyList()
    {
        Assert.NotEmpty(_allServices);
    }

    [Fact]
    public void GetAllServices_AllEntries_HaveServiceName()
    {
        Assert.All(_allServices, s => Assert.False(string.IsNullOrEmpty(s.ServiceName)));
    }

    [Fact]
    public void GetAllServices_AllEntries_HaveDisplayName()
    {
        Assert.All(_allServices, s => Assert.False(string.IsNullOrEmpty(s.DisplayName)));
    }

    [Fact]
    public void GetAllServices_AllEntries_HaveValidStatus()
    {
        Assert.All(_allServices, s => Assert.True(Enum.IsDefined(typeof(ServiceControllerStatus), s.Status)));
    }

    [Fact]
    public void GetAllServices_AllEntries_HaveValidStartType()
    {
        Assert.All(_allServices, s => Assert.True(Enum.IsDefined(typeof(ServiceStartMode), s.StartType)));
    }

    [Fact]
    public void GetService_Spooler_ReturnsEntry()
    {
        // The Print Spooler ("Spooler") is present on all standard Windows installations.
        var entry = ServiceManager.GetService("Spooler");
        Assert.NotNull(entry);
        Assert.Equal("Spooler", entry.ServiceName);
        Assert.False(string.IsNullOrEmpty(entry.DisplayName));
    }

    [Fact]
    public void GetService_NonExistentName_ReturnsNull()
    {
        var entry = ServiceManager.GetService("RegiLattice_Nonexistent_XYZ_DoesNotExist");
        Assert.Null(entry);
    }

    [Fact]
    public void ServiceEntry_Record_CanBeConstructed()
    {
        var entry = new ServiceEntry(
            ServiceName: "TestSvc",
            DisplayName: "Test Service",
            Description: "A test",
            Status: ServiceControllerStatus.Stopped,
            StartType: ServiceStartMode.Manual,
            CanStop: false,
            CanPauseAndContinue: false
        );

        Assert.Equal("TestSvc", entry.ServiceName);
        Assert.Equal(ServiceControllerStatus.Stopped, entry.Status);
        Assert.Equal(ServiceStartMode.Manual, entry.StartType);
    }

    [Fact]
    public void ServiceEntry_EqualityByValue()
    {
        var a = new ServiceEntry("Svc", "Svc Name", "", ServiceControllerStatus.Running, ServiceStartMode.Automatic, true, false);
        var b = new ServiceEntry("Svc", "Svc Name", "", ServiceControllerStatus.Running, ServiceStartMode.Automatic, true, false);
        Assert.Equal(a, b);
    }

    [Fact]
    public void GetAllServices_CountIsGreaterThanTen()
    {
        // Any Windows installation will have well more than 10 services
        Assert.True(_allServices.Count > 10, $"Expected >10 services but got {_allServices.Count}");
    }
}
