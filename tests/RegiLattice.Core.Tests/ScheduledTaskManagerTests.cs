// tests/RegiLattice.Core.Tests/ScheduledTaskManagerTests.cs
// Sprint coverage — ScheduledTaskManager CSV parsing, status mapping, and record model.

using System.Reflection;
using RegiLattice.Core.Services;
using Xunit;

namespace RegiLattice.Core.Tests;

/// <summary>
/// Tests for ScheduledTaskManager: CSV parsing logic via reflection on private
/// ParseCsvOutput/ParseStatus/SplitCsvLine helpers, ScheduledTaskEntry record,
/// and EscapeTaskName helper.
/// </summary>
public sealed class ScheduledTaskManagerTests
{
    // ── Reflection helpers ───────────────────────────────────────────────────

    private static IReadOnlyList<ScheduledTaskEntry> InvokeParseCsvOutput(string csv)
    {
        var method = typeof(ScheduledTaskManager).GetMethod("ParseCsvOutput", BindingFlags.NonPublic | BindingFlags.Static)!;
        return (IReadOnlyList<ScheduledTaskEntry>)method.Invoke(null, [csv])!;
    }

    private static ScheduledTaskStatus InvokeParseStatus(string s)
    {
        var method = typeof(ScheduledTaskManager).GetMethod("ParseStatus", BindingFlags.NonPublic | BindingFlags.Static)!;
        return (ScheduledTaskStatus)method.Invoke(null, [s])!;
    }

    private static string[] InvokeSplitCsvLine(string line)
    {
        var method = typeof(ScheduledTaskManager).GetMethod("SplitCsvLine", BindingFlags.NonPublic | BindingFlags.Static)!;
        return (string[])method.Invoke(null, [line])!;
    }

    private static string InvokeEscapeTaskName(string name)
    {
        var method = typeof(ScheduledTaskManager).GetMethod("EscapeTaskName", BindingFlags.NonPublic | BindingFlags.Static)!;
        return (string)method.Invoke(null, [name])!;
    }

    // ── ScheduledTaskEntry record ─────────────────────────────────────────────

    [Fact]
    public void ScheduledTaskEntry_Constructor_SetsAllProperties()
    {
        var entry = new ScheduledTaskEntry(
            TaskName: "\\Microsoft\\Windows\\TestTask",
            TaskPath: "\\Microsoft\\Windows\\TestTask",
            Status: ScheduledTaskStatus.Ready,
            NextRunTime: "1/1/2025 12:00:00 AM",
            LastRunTime: "12/31/2024 11:00:00 PM",
            Author: "Microsoft Corporation"
        );

        Assert.Equal("\\Microsoft\\Windows\\TestTask", entry.TaskName);
        Assert.Equal(ScheduledTaskStatus.Ready, entry.Status);
        Assert.Equal("Microsoft Corporation", entry.Author);
    }

    [Fact]
    public void DisplayName_WithBackslashedPath_ReturnsLastSegment()
    {
        var entry = new ScheduledTaskEntry("\\Microsoft\\Windows\\Work Folders\\Sync", "", ScheduledTaskStatus.Ready, "", "", "");

        Assert.Equal("Sync", entry.DisplayName);
    }

    [Fact]
    public void DisplayName_WithoutBackslash_ReturnsFullName()
    {
        var entry = new ScheduledTaskEntry("SimpleTask", "", ScheduledTaskStatus.Ready, "", "", "");
        Assert.Equal("SimpleTask", entry.DisplayName);
    }

    [Fact]
    public void DisplayName_RootLevelTask_ReturnsName()
    {
        var entry = new ScheduledTaskEntry("\\TopLevelTask", "", ScheduledTaskStatus.Disabled, "", "", "");
        Assert.Equal("TopLevelTask", entry.DisplayName);
    }

    // ── ScheduledTaskStatus enum ───────────────────────────────────────────────

    [Fact]
    public void ParseStatus_Ready_ReturnsReady()
    {
        Assert.Equal(ScheduledTaskStatus.Ready, InvokeParseStatus("Ready"));
    }

    [Fact]
    public void ParseStatus_Running_ReturnsRunning()
    {
        Assert.Equal(ScheduledTaskStatus.Running, InvokeParseStatus("Running"));
    }

    [Fact]
    public void ParseStatus_Disabled_ReturnsDisabled()
    {
        Assert.Equal(ScheduledTaskStatus.Disabled, InvokeParseStatus("Disabled"));
    }

    [Fact]
    public void ParseStatus_Queued_ReturnsQueued()
    {
        Assert.Equal(ScheduledTaskStatus.Queued, InvokeParseStatus("Queued"));
    }

    [Fact]
    public void ParseStatus_Unknown_ReturnsUnknown()
    {
        Assert.Equal(ScheduledTaskStatus.Unknown, InvokeParseStatus("Something weird"));
    }

    [Fact]
    public void ParseStatus_CaseInsensitive_Ready()
    {
        Assert.Equal(ScheduledTaskStatus.Ready, InvokeParseStatus("READY"));
        Assert.Equal(ScheduledTaskStatus.Ready, InvokeParseStatus("ready"));
    }

    // ── SplitCsvLine ──────────────────────────────────────────────────────────

    [Fact]
    public void SplitCsvLine_SimpleFields_SplitsOnComma()
    {
        var fields = InvokeSplitCsvLine("Field1,Field2,Field3");
        Assert.Equal(3, fields.Length);
        Assert.Equal("Field1", fields[0]);
        Assert.Equal("Field2", fields[1]);
        Assert.Equal("Field3", fields[2]);
    }

    [Fact]
    public void SplitCsvLine_QuotedFieldWithComma_TreatedAsSingleField()
    {
        // A field with a comma inside quotes must not be split
        var fields = InvokeSplitCsvLine("\"Last, First\",Status");
        Assert.Equal(2, fields.Length);
        Assert.Equal("Last, First", fields[0]);
        Assert.Equal("Status", fields[1]);
    }

    [Fact]
    public void SplitCsvLine_QuotedFields_QuotesAreStripped()
    {
        // Quotes are consumed — not included in the field value
        var fields = InvokeSplitCsvLine("\"TaskName\",\"Ready\",\"Author\"");
        Assert.Equal(3, fields.Length);
        Assert.Equal("TaskName", fields[0]);
        Assert.Equal("Ready", fields[1]);
        Assert.Equal("Author", fields[2]);
    }

    [Fact]
    public void SplitCsvLine_EmptyField_ReturnsEmptyString()
    {
        var fields = InvokeSplitCsvLine("A,,C");
        Assert.Equal(3, fields.Length);
        Assert.Equal("", fields[1]);
    }

    // ── ParseCsvOutput ────────────────────────────────────────────────────────

    [Fact]
    public void ParseCsvOutput_EmptyInput_ReturnsEmpty()
    {
        var tasks = InvokeParseCsvOutput("");
        Assert.Empty(tasks);
    }

    [Fact]
    public void ParseCsvOutput_SingleRow_ParsedCorrectly()
    {
        // Schema: TaskName, NextRunTime, Status, LogonMode, LastRunTime, LastResult, Author
        string csv = "\"\\Microsoft\\TestTask\",\"1/1/2025 12:00:00 AM\",\"Ready\",\"Interactive\",\"12/31/2024\",\"0\",\"Microsoft\"";

        var tasks = InvokeParseCsvOutput(csv);

        Assert.Single(tasks);
        Assert.Equal("\\Microsoft\\TestTask", tasks[0].TaskName);
        Assert.Equal(ScheduledTaskStatus.Ready, tasks[0].Status);
        Assert.Equal("Microsoft", tasks[0].Author);
    }

    [Fact]
    public void ParseCsvOutput_DisabledTask_HasDisabledStatus()
    {
        string csv = "\"\\Tasks\\MyDisabledTask\",\"N/A\",\"Disabled\",\"Interactive\",\"N/A\",\"0\",\"\"";

        var tasks = InvokeParseCsvOutput(csv);

        Assert.Single(tasks);
        Assert.Equal(ScheduledTaskStatus.Disabled, tasks[0].Status);
    }

    [Fact]
    public void ParseCsvOutput_HeaderRow_SkippedGracefully()
    {
        // A row where TaskName starts with "TaskName" (case-insensitive) is filtered out
        string csv = "\"TaskName\",\"Next Run Time\",\"Status\",\"Logon Mode\",\"Last Run Time\",\"Last Result\",\"Author\"";

        var tasks = InvokeParseCsvOutput(csv);
        Assert.Empty(tasks);
    }

    [Fact]
    public void ParseCsvOutput_MultipleRows_SortedAlphabetically()
    {
        string csv = "\"\\ZTasks\\Zebra\",\"N/A\",\"Ready\",\"\",\"\",\"0\",\"\"\n" + "\"\\ATasks\\Alpha\",\"N/A\",\"Ready\",\"\",\"\",\"0\",\"\"";

        var tasks = InvokeParseCsvOutput(csv);

        Assert.Equal(2, tasks.Count);
        Assert.Equal("\\ATasks\\Alpha", tasks[0].TaskName);
        Assert.Equal("\\ZTasks\\Zebra", tasks[1].TaskName);
    }

    // ── EscapeTaskName ────────────────────────────────────────────────────────

    [Fact]
    public void EscapeTaskName_NoQuotes_ReturnsSameString()
    {
        Assert.Equal("\\Tasks\\MyTask", InvokeEscapeTaskName("\\Tasks\\MyTask"));
    }

    [Fact]
    public void EscapeTaskName_WithDoubleQuote_EscapesIt()
    {
        string escaped = InvokeEscapeTaskName("Task\"Name");
        Assert.Equal("Task\\\"Name", escaped);
    }
}
