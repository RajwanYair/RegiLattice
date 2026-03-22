// tests/RegiLattice.Core.Tests/PowerPlanManagerTests.cs
// Sprint coverage — PowerPlanManager record types, well-known GUIDs, and ParseListOutput parsing.

using System.Reflection;
using RegiLattice.Core.Services;
using Xunit;

namespace RegiLattice.Core.Tests;

/// <summary>
/// Tests for PowerPlanManager: parsing logic via reflection on the private
/// ParseListOutput method, PowerPlanEntry record, and well-known GUID constants.
/// </summary>
public sealed class PowerPlanManagerTests
{
    // Reflection helper — accesses the private static ParseListOutput method.
    private static IReadOnlyList<PowerPlanEntry> ParseListOutput(string output)
    {
        var method = typeof(PowerPlanManager).GetMethod(
            "ParseListOutput",
            BindingFlags.NonPublic | BindingFlags.Static
        )!;
        return (IReadOnlyList<PowerPlanEntry>)method.Invoke(null, [output])!;
    }

    // ── Well-known GUIDs ──────────────────────────────────────────────────────

    [Fact]
    public void HighPerformance_GUID_MatchesKnownValue()
    {
        Assert.Equal(
            new Guid("8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c"),
            PowerPlanManager.HighPerformance
        );
    }

    [Fact]
    public void Balanced_GUID_MatchesKnownValue()
    {
        Assert.Equal(
            new Guid("381b4222-f694-41f0-9685-ff5bb260df2e"),
            PowerPlanManager.Balanced
        );
    }

    [Fact]
    public void PowerSaver_GUID_MatchesKnownValue()
    {
        Assert.Equal(
            new Guid("a1841308-3541-4fab-bc81-f71556f20b4a"),
            PowerPlanManager.PowerSaver
        );
    }

    [Fact]
    public void UltimatePerfMode_GUID_MatchesKnownValue()
    {
        Assert.Equal(
            new Guid("e9a42b02-d5df-448d-aa00-03f14749eb61"),
            PowerPlanManager.UltimatePerfMode
        );
    }

    [Fact]
    public void AllWellKnownGuids_AreDistinct()
    {
        var guids = new[]
        {
            PowerPlanManager.HighPerformance,
            PowerPlanManager.Balanced,
            PowerPlanManager.PowerSaver,
            PowerPlanManager.UltimatePerfMode,
        };
        Assert.Equal(guids.Length, guids.Distinct().Count());
    }

    // ── PowerPlanEntry record ─────────────────────────────────────────────────

    [Fact]
    public void PowerPlanEntry_Constructor_SetsAllProperties()
    {
        var guid = Guid.NewGuid();
        var entry = new PowerPlanEntry(guid, "High Performance", true);

        Assert.Equal(guid, entry.Guid);
        Assert.Equal("High Performance", entry.Name);
        Assert.True(entry.IsActive);
    }

    [Fact]
    public void PowerPlanEntry_InactiveEntry_IsActiveFalse()
    {
        var entry = new PowerPlanEntry(Guid.NewGuid(), "Balanced", false);
        Assert.False(entry.IsActive);
    }

    // ── ParseListOutput ───────────────────────────────────────────────────────

    [Fact]
    public void ParseListOutput_EmptyInput_ReturnsEmpty()
    {
        var plans = ParseListOutput("");
        Assert.Empty(plans);
    }

    [Fact]
    public void ParseListOutput_GarbageLines_ReturnsEmpty()
    {
        string garbage = "No power schemes found.\r\nSomething else entirely.";
        var plans = ParseListOutput(garbage);
        Assert.Empty(plans);
    }

    [Fact]
    public void ParseListOutput_SingleActivePlan_ParsedCorrectly()
    {
        string output =
            "Power Scheme GUID: 381b4222-f694-41f0-9685-ff5bb260df2e  (Balanced) *\r\n";

        var plans = ParseListOutput(output);

        Assert.Single(plans);
        Assert.Equal(new Guid("381b4222-f694-41f0-9685-ff5bb260df2e"), plans[0].Guid);
        Assert.Equal("Balanced", plans[0].Name);
        Assert.True(plans[0].IsActive);
    }

    [Fact]
    public void ParseListOutput_SingleInactivePlan_ParsedCorrectly()
    {
        string output =
            "Power Scheme GUID: 8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c  (High performance)\r\n";

        var plans = ParseListOutput(output);

        Assert.Single(plans);
        Assert.Equal("High performance", plans[0].Name);
        Assert.False(plans[0].IsActive);
    }

    [Fact]
    public void ParseListOutput_MultiplePlans_OnlyActiveMarked()
    {
        string output =
            "Existing Power Schemes (* Active)\r\n"
            + "-----------------------------------\r\n"
            + "Power Scheme GUID: 381b4222-f694-41f0-9685-ff5bb260df2e  (Balanced) *\r\n"
            + "Power Scheme GUID: 8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c  (High performance)\r\n"
            + "Power Scheme GUID: a1841308-3541-4fab-bc81-f71556f20b4a  (Power saver)\r\n";

        var plans = ParseListOutput(output);

        Assert.Equal(3, plans.Count);
        Assert.Single(plans, p => p.IsActive);
        Assert.Equal("Balanced", plans.Single(p => p.IsActive).Name);
        Assert.All(plans.Where(p => !p.IsActive), p => Assert.False(p.IsActive));
    }

    [Fact]
    public void ParseListOutput_PlanNameWithSpaces_TrimmedCorrectly()
    {
        string output =
            "Power Scheme GUID: e9a42b02-d5df-448d-aa00-03f14749eb61  (Ultimate Performance)\r\n";

        var plans = ParseListOutput(output);

        Assert.Single(plans);
        Assert.Equal("Ultimate Performance", plans[0].Name);
    }

    [Fact]
    public void ParseListOutput_UppercaseGuid_ParsedCorrectly()
    {
        string output =
            "Power Scheme GUID: 381B4222-F694-41F0-9685-FF5BB260DF2E  (Balanced)\r\n";

        var plans = ParseListOutput(output);

        Assert.Single(plans);
        Assert.Equal(new Guid("381b4222-f694-41f0-9685-ff5bb260df2e"), plans[0].Guid);
    }
}
