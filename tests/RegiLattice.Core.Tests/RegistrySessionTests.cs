using RegiLattice.Core.Models;
using RegiLattice.Core.Registry;
using Xunit;

namespace RegiLattice.Core.Tests;

/// <summary>Tests for RegistrySession in DryRun mode, ParsePath, Execute, and Evaluate.</summary>
public sealed class RegistrySessionTests
{
    // ── DryRun basic ops ────────────────────────────────────────────────

    [Fact]
    public void DryRun_SetDword_IncrementsCounter()
    {
        var s = new RegistrySession(dryRun: true);
        s.SetDword(@"HKCU\Software\Test", "Val", 1);
        Assert.Equal(1, s.DryOps);
    }

    [Fact]
    public void DryRun_SetString_Logs()
    {
        var s = new RegistrySession(dryRun: true);
        s.SetString(@"HKCU\Software\Test", "Name", "Value");
        Assert.Single(s.Log);
        Assert.Contains("SET", s.Log[0]);
    }

    [Fact]
    public void DryRun_DeleteValue_IncrementsCounter()
    {
        var s = new RegistrySession(dryRun: true);
        s.DeleteValue(@"HKCU\Software\Test", "Name");
        Assert.Equal(1, s.DryOps);
        Assert.Contains("DELETE", s.Log[0]);
    }

    [Fact]
    public void DryRun_DeleteTree_IncrementsCounter()
    {
        var s = new RegistrySession(dryRun: true);
        s.DeleteTree(@"HKCU\Software\Test");
        Assert.Equal(1, s.DryOps);
        Assert.Contains("DELETE_TREE", s.Log[0]);
    }

    [Fact]
    public void DryRun_MultipleOps_Accumulate()
    {
        var s = new RegistrySession(dryRun: true);
        s.SetDword(@"HKCU\Software\A", "V", 1);
        s.SetString(@"HKCU\Software\B", "V", "x");
        s.SetQword(@"HKCU\Software\C", "V", 100L);
        s.SetBinary(@"HKCU\Software\D", "V", [0x01, 0x02]);
        s.SetMultiSz(@"HKCU\Software\E", "V", ["a", "b"]);
        s.SetExpandString(@"HKCU\Software\F", "V", "%TEMP%");
        s.DeleteValue(@"HKCU\Software\G", "V");
        Assert.Equal(7, s.DryOps);
        Assert.Equal(7, s.Log.Count);
    }

    // ── ParsePath ───────────────────────────────────────────────────────

    [Theory]
    [InlineData(@"HKCU\Software\Test", "Software\\Test")]
    [InlineData(@"HKEY_CURRENT_USER\Software\Test", "Software\\Test")]
    [InlineData(@"HKLM\SOFTWARE\Microsoft", "SOFTWARE\\Microsoft")]
    [InlineData(@"HKEY_LOCAL_MACHINE\SYSTEM\Test", "SYSTEM\\Test")]
    [InlineData(@"HKCR\.txt", ".txt")]
    [InlineData(@"HKEY_CLASSES_ROOT\.txt", ".txt")]
    [InlineData(@"HKU\.DEFAULT", ".DEFAULT")]
    [InlineData(@"HKEY_USERS\.DEFAULT", ".DEFAULT")]
    [InlineData(@"HKCC\System\Config", "System\\Config")]
    [InlineData(@"HKEY_CURRENT_CONFIG\System\Config", "System\\Config")]
    public void ParsePath_AllHives(string path, string expectedSubKey)
    {
        var (_, subKey) = RegistrySession.ParsePath(path);
        Assert.Equal(expectedSubKey, subKey);
    }

    [Fact]
    public void ParsePath_NoBackslash_Throws()
    {
        Assert.Throws<ArgumentException>(() => RegistrySession.ParsePath("HKCU"));
    }

    [Fact]
    public void ParsePath_UnknownHive_Throws()
    {
        Assert.Throws<ArgumentException>(() => RegistrySession.ParsePath(@"HKFAKE\Test"));
    }

    // ── Execute / Evaluate ──────────────────────────────────────────────

    [Fact]
    public void Execute_SetDwordOp_CountsDryRun()
    {
        var s = new RegistrySession(dryRun: true);
        s.Execute([RegOp.SetDword(@"HKCU\Software\Test", "V", 1)]);
        Assert.Equal(1, s.DryOps);
    }

    [Fact]
    public void Execute_MultipleOps_CountsAll()
    {
        var s = new RegistrySession(dryRun: true);
        s.Execute([
            RegOp.SetDword(@"HKCU\Software\Test", "V1", 1),
            RegOp.SetString(@"HKCU\Software\Test", "V2", "hello"),
            RegOp.DeleteValue(@"HKCU\Software\Test", "V3"),
        ]);
        Assert.Equal(3, s.DryOps);
    }

    [Fact]
    public void Execute_CheckOp_Throws()
    {
        var s = new RegistrySession(dryRun: true);
        Assert.Throws<InvalidOperationException>(() => s.Execute([RegOp.CheckDword(@"HKCU\Software\Test", "V", 1)]));
    }

    [Fact]
    public void Evaluate_EmptyOps_ReturnsFalse()
    {
        var s = new RegistrySession(dryRun: true);
        Assert.False(s.Evaluate([]));
    }

    [Fact]
    public void Evaluate_SetOp_Throws()
    {
        var s = new RegistrySession(dryRun: true);
        Assert.Throws<InvalidOperationException>(() => s.Evaluate([RegOp.SetDword(@"HKCU\Software\Test", "V", 1)]));
    }

    // ── Log ─────────────────────────────────────────────────────────────

    [Fact]
    public void WriteLog_AppendsEntry()
    {
        var s = new RegistrySession(dryRun: true);
        s.WriteLog("custom message");
        Assert.Single(s.Log);
        Assert.Contains("custom message", s.Log[0]);
    }

    [Fact]
    public void WriteLog_ContainsTimestamp()
    {
        var s = new RegistrySession(dryRun: true);
        s.WriteLog("test");
        Assert.Matches(@"\[\d{2}:\d{2}:\d{2}\]", s.Log[0]);
    }

    // ── Properties ──────────────────────────────────────────────────────

    [Fact]
    public void DryRun_PropertyReflectsConstructor()
    {
        Assert.True(new RegistrySession(dryRun: true).DryRun);
        Assert.False(new RegistrySession(dryRun: false).DryRun);
    }
}
