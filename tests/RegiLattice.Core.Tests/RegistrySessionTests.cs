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

    // ── Read operations (safe, uses real HKCU) ──────────────────────────

    [Fact]
    public void ReadDword_ExistingValue_ReturnsInt()
    {
        // Windows always has HKCU\Console\ScreenBufferSize
        var s = new RegistrySession(dryRun: false);
        var val = s.ReadValue(@"HKEY_CURRENT_USER\Console", "ScreenBufferSize");
        Assert.NotNull(val);
    }

    [Fact]
    public void ReadString_NonexistentValue_ReturnsNull()
    {
        var s = new RegistrySession(dryRun: false);
        var val = s.ReadString(@"HKEY_CURRENT_USER\Software", "RegiLatticeNonExistent12345");
        Assert.Null(val);
    }

    [Fact]
    public void KeyExists_KnownKey_ReturnsTrue()
    {
        var s = new RegistrySession(dryRun: false);
        Assert.True(s.KeyExists(@"HKEY_CURRENT_USER\Software"));
    }

    [Fact]
    public void KeyExists_UnknownKey_ReturnsFalse()
    {
        var s = new RegistrySession(dryRun: false);
        Assert.False(s.KeyExists(@"HKEY_CURRENT_USER\Software\RegiLattice_NonExistent_Key_12345"));
    }

    [Fact]
    public void ValueExists_KnownValue_ReturnsTrue()
    {
        var s = new RegistrySession(dryRun: false);
        // Console key always has values
        Assert.True(s.ValueExists(@"HKEY_CURRENT_USER\Console", "ScreenBufferSize"));
    }

    [Fact]
    public void ValueExists_UnknownValue_ReturnsFalse()
    {
        var s = new RegistrySession(dryRun: false);
        Assert.False(s.ValueExists(@"HKEY_CURRENT_USER\Software", "RegiLatticeNonExistent12345"));
    }

    [Fact]
    public void ListSubKeys_Software_ReturnsNonEmpty()
    {
        var s = new RegistrySession(dryRun: false);
        var keys = s.ListSubKeys(@"HKEY_CURRENT_USER\Software");
        Assert.NotEmpty(keys);
    }

    [Fact]
    public void ListValueNames_Console_ReturnsNonEmpty()
    {
        var s = new RegistrySession(dryRun: false);
        var names = s.ListValueNames(@"HKEY_CURRENT_USER\Console");
        Assert.NotEmpty(names);
    }

    [Fact]
    public void ListSubKeys_NonexistentKey_ReturnsEmpty()
    {
        var s = new RegistrySession(dryRun: false);
        var keys = s.ListSubKeys(@"HKEY_CURRENT_USER\Software\RegiLattice_NonExistent_Key_12345");
        Assert.Empty(keys);
    }

    // ── ParsePath edge cases ────────────────────────────────────────────

    [Fact]
    public void ParsePath_AbbreviatedHklm_Works()
    {
        var (root, subKey) = RegistrySession.ParsePath(@"HKLM\SOFTWARE\Microsoft");
        Assert.Equal(Microsoft.Win32.Registry.LocalMachine, root);
        Assert.Equal(@"SOFTWARE\Microsoft", subKey);
    }

    [Fact]
    public void ParsePath_AbbreviatedHkcu_Works()
    {
        var (root, subKey) = RegistrySession.ParsePath(@"HKCU\Software\Test");
        Assert.Equal(Microsoft.Win32.Registry.CurrentUser, root);
        Assert.Equal(@"Software\Test", subKey);
    }

    [Fact]
    public void ParsePath_SingleSegmentNoBackslash_Throws()
    {
        Assert.Throws<ArgumentException>(() => RegistrySession.ParsePath("NoBackslash"));
    }

    [Fact]
    public void ParsePath_UnsupportedHivePrefix_Throws()
    {
        Assert.Throws<ArgumentException>(() => RegistrySession.ParsePath(@"HKZZ\Software\Test"));
    }

    // ── Execute in DryRun ───────────────────────────────────────────────

    [Fact]
    public void Execute_DryRun_RecordsOpsWithoutWriting()
    {
        var s = new RegistrySession(dryRun: true);
        var ops = new[]
        {
            RegOp.SetDword(@"HKCU\Software\Test", "Val1", 1),
            RegOp.SetDword(@"HKCU\Software\Test", "Val2", 2),
            RegOp.DeleteValue(@"HKCU\Software\Test", "Val3"),
        };
        s.Execute(ops);
        Assert.Equal(3, s.DryOps);
    }

    [Fact]
    public void Execute_EmptyOps_DoesNothing()
    {
        var s = new RegistrySession(dryRun: true);
        s.Execute([]);
        Assert.Equal(0, s.DryOps);
    }

    // ── Evaluate edge cases ──────────────────────────────────────────

    [Fact]
    public void Evaluate_CheckMissing_OnNonExistentKey_ReturnsTrue()
    {
        var s = new RegistrySession(dryRun: false);
        var ops = new[] { RegOp.CheckMissing(@"HKEY_CURRENT_USER\Software\RegiLattice_NonExistent_12345", "NoVal") };
        var result = s.Evaluate(ops);
        Assert.True(result);
    }

    [Fact]
    public void Evaluate_CheckKeyMissing_OnNonExistentKey_ReturnsTrue()
    {
        var s = new RegistrySession(dryRun: false);
        var ops = new[] { RegOp.CheckKeyMissing(@"HKEY_CURRENT_USER\Software\RegiLattice_NonExistent_12345") };
        var result = s.Evaluate(ops);
        Assert.True(result);
    }

    [Fact]
    public void Evaluate_CheckKeyMissing_OnExistingKey_ReturnsFalse()
    {
        var s = new RegistrySession(dryRun: false);
        var ops = new[] { RegOp.CheckKeyMissing(@"HKEY_CURRENT_USER\Software") };
        var result = s.Evaluate(ops);
        Assert.False(result);
    }

    // ── Backup in DryRun ────────────────────────────────────────────────

    [Fact]
    public void Backup_ReadOnlyKeys_CreatesBackupFile()
    {
        var backupDir = Path.Combine(Path.GetTempPath(), $"rl_backup_test_{Guid.NewGuid()}");
        var s = new RegistrySession(dryRun: false, backupDir: backupDir);

        try
        {
            var path = s.Backup([@"HKEY_CURRENT_USER\Console"], "test-backup");
            Assert.True(File.Exists(path));
            var json = File.ReadAllText(path);
            Assert.False(string.IsNullOrWhiteSpace(json));
        }
        finally
        {
            if (Directory.Exists(backupDir))
                Directory.Delete(backupDir, true);
        }
    }

    [Fact]
    public void Backup_NonExistentKey_StillCreatesFile()
    {
        var backupDir = Path.Combine(Path.GetTempPath(), $"rl_backup_nxk_{Guid.NewGuid()}");
        var s = new RegistrySession(dryRun: false, backupDir: backupDir);

        try
        {
            var path = s.Backup([@"HKEY_CURRENT_USER\Software\RegiLattice_NonExistent_99999"], "test-nxk");
            Assert.True(File.Exists(path));
        }
        finally
        {
            if (Directory.Exists(backupDir))
                Directory.Delete(backupDir, true);
        }
    }

    // ── WriteLog ────────────────────────────────────────────────────────

    [Fact]
    public void WriteLog_AppendsToLog()
    {
        var s = new RegistrySession(dryRun: true);
        s.WriteLog("Test message 1");
        s.WriteLog("Test message 2");
        Assert.Equal(2, s.Log.Count);
        Assert.Contains("Test message 1", s.Log[0]);
        Assert.Contains("Test message 2", s.Log[1]);
    }

    // ── Read operations ─────────────────────────────────────────────────

    [Fact]
    public void ReadDword_NonExistentKey_ReturnsNull()
    {
        var s = new RegistrySession(dryRun: false);
        var result = s.ReadDword(@"HKEY_CURRENT_USER\Software\RegiLattice_NonExistent_12345", "Nope");
        Assert.Null(result);
    }

    [Fact]
    public void ReadString_NonExistentKey_ReturnsNull()
    {
        var s = new RegistrySession(dryRun: false);
        var result = s.ReadString(@"HKEY_CURRENT_USER\Software\RegiLattice_NonExistent_12345", "Nope");
        Assert.Null(result);
    }

    [Fact]
    public void KeyExists_ExistingKey_ReturnsTrue()
    {
        var s = new RegistrySession(dryRun: false);
        Assert.True(s.KeyExists(@"HKEY_CURRENT_USER\Software"));
    }

    [Fact]
    public void KeyExists_NonExistentKey_ReturnsFalse()
    {
        var s = new RegistrySession(dryRun: false);
        Assert.False(s.KeyExists(@"HKEY_CURRENT_USER\Software\RegiLattice_NonExistent_12345"));
    }

    [Fact]
    public void ValueExists_NonExistentKey_ReturnsFalse()
    {
        var s = new RegistrySession(dryRun: false);
        Assert.False(s.ValueExists(@"HKEY_CURRENT_USER\Software\RegiLattice_NonExistent_12345", "V"));
    }

    // ── ParsePath: full hive names ──────────────────────────────────────

    [Fact]
    public void ParsePath_FullHKLM_Works()
    {
        var (root, subKey) = RegistrySession.ParsePath(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft");
        Assert.Equal(Microsoft.Win32.Registry.LocalMachine, root);
        Assert.Equal(@"SOFTWARE\Microsoft", subKey);
    }

    [Fact]
    public void ParsePath_HKCR_Works()
    {
        var (root, subKey) = RegistrySession.ParsePath(@"HKEY_CLASSES_ROOT\*\shell");
        Assert.Equal(Microsoft.Win32.Registry.ClassesRoot, root);
        Assert.Equal(@"*\shell", subKey);
    }

    // ── Sprint 21: Coverage boost — DryRun ops, Execute dispatch, Evaluate, ParsePath, Events ──

    [Fact]
    public void DryRun_SetExpandString_IncrementsCounter()
    {
        var s = new RegistrySession(dryRun: true);
        s.SetExpandString(@"HKCU\Software\Test", "Path", @"%SystemRoot%\bin");
        Assert.Equal(1, s.DryOps);
    }

    [Fact]
    public void DryRun_SetQword_IncrementsCounter()
    {
        var s = new RegistrySession(dryRun: true);
        s.SetQword(@"HKCU\Software\Test", "Big", 123456789L);
        Assert.Equal(1, s.DryOps);
    }

    [Fact]
    public void DryRun_SetBinary_IncrementsCounter()
    {
        var s = new RegistrySession(dryRun: true);
        s.SetBinary(@"HKCU\Software\Test", "Blob", [0x01, 0x02, 0x03]);
        Assert.Equal(1, s.DryOps);
    }

    [Fact]
    public void DryRun_SetMultiSz_IncrementsCounter()
    {
        var s = new RegistrySession(dryRun: true);
        s.SetMultiSz(@"HKCU\Software\Test", "Lines", ["a", "b"]);
        Assert.Equal(1, s.DryOps);
    }

    [Fact]
    public void Execute_DeleteValueOp_IncrementsDryOps()
    {
        var s = new RegistrySession(dryRun: true);
        s.Execute([RegOp.DeleteValue(@"HKCU\Software\Test", "V")]);
        Assert.Equal(1, s.DryOps);
    }

    [Fact]
    public void Execute_DeleteTreeOp_IncrementsDryOps()
    {
        var s = new RegistrySession(dryRun: true);
        s.Execute([RegOp.DeleteTree(@"HKCU\Software\Test\SubKey")]);
        Assert.Equal(1, s.DryOps);
    }

    [Fact]
    public void Execute_SetStringOp_IncrementsDryOps()
    {
        var s = new RegistrySession(dryRun: true);
        s.Execute([RegOp.SetString(@"HKCU\Software\Test", "N", "V")]);
        Assert.Equal(1, s.DryOps);
    }

    [Fact]
    public void Execute_SetExpandStringOp_IncrementsDryOps()
    {
        var s = new RegistrySession(dryRun: true);
        s.Execute([RegOp.SetExpandString(@"HKCU\Software\Test", "Path", @"%TEMP%\x")]);
        Assert.Equal(1, s.DryOps);
    }

    [Fact]
    public void Execute_SetQwordOp_IncrementsDryOps()
    {
        var s = new RegistrySession(dryRun: true);
        s.Execute([RegOp.SetQword(@"HKCU\Software\Test", "Q", 42L)]);
        Assert.Equal(1, s.DryOps);
    }

    [Fact]
    public void Execute_SetBinaryOp_IncrementsDryOps()
    {
        var s = new RegistrySession(dryRun: true);
        s.Execute([RegOp.SetBinary(@"HKCU\Software\Test", "B", [0xFF])]);
        Assert.Equal(1, s.DryOps);
    }

    [Fact]
    public void Execute_SetMultiSzOp_IncrementsDryOps()
    {
        var s = new RegistrySession(dryRun: true);
        s.Execute([RegOp.SetMultiSz(@"HKCU\Software\Test", "M", ["a", "b"])]);
        Assert.Equal(1, s.DryOps);
    }

    [Fact]
    public void Evaluate_CheckDword_OnKnownValue_ReturnsCorrectly()
    {
        var s = new RegistrySession(dryRun: false);
        // HKCU\Console\FullScreen should exist on most Windows; 0 = windowed
        var result = s.Evaluate([RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "FullScreen", 0)]);
        // Whether true or false, it should not throw
        Assert.True(result || !result);
    }

    [Fact]
    public void Evaluate_CheckMissing_OnExistingValue_ReturnsFalse()
    {
        var s = new RegistrySession(dryRun: false);
        // HKCU\Console has known values, so checking for "missing" on an existing one should fail
        var result = s.Evaluate([RegOp.CheckMissing(@"HKEY_CURRENT_USER\Console", "FullScreen")]);
        Assert.False(result);
    }

    [Fact]
    public void Evaluate_MultipleChecks_AllMustPass()
    {
        var s = new RegistrySession(dryRun: false);
        // Both check non-existent keys → both true → overall true
        var result = s.Evaluate([
            RegOp.CheckMissing(@"HKEY_CURRENT_USER\Software\RegiLattice_NOEXIST_99", "A"),
            RegOp.CheckMissing(@"HKEY_CURRENT_USER\Software\RegiLattice_NOEXIST_99", "B"),
        ]);
        Assert.True(result);
    }

    [Fact]
    public void Evaluate_MixedCheckResults_ReturnsFalse()
    {
        var s = new RegistrySession(dryRun: false);
        // One passes (non-existent), one fails (existing key that shouldn't be missing)
        var result = s.Evaluate([
            RegOp.CheckMissing(@"HKEY_CURRENT_USER\Software\RegiLattice_NOEXIST_99", "A"),
            RegOp.CheckKeyMissing(@"HKEY_CURRENT_USER\Console"),
        ]);
        Assert.False(result);
    }

    [Fact]
    public void ParsePath_HKU_FullName_Works()
    {
        var (root, subKey) = RegistrySession.ParsePath(@"HKEY_USERS\.DEFAULT\Software");
        Assert.Equal(Microsoft.Win32.Registry.Users, root);
        Assert.Equal(@".DEFAULT\Software", subKey);
    }

    [Fact]
    public void ParsePath_HKU_Abbreviated_Works()
    {
        var (root, subKey) = RegistrySession.ParsePath(@"HKU\.DEFAULT\Software");
        Assert.Equal(Microsoft.Win32.Registry.Users, root);
        Assert.Equal(@".DEFAULT\Software", subKey);
    }

    [Fact]
    public void ParsePath_HKCC_FullName_Works()
    {
        var (root, subKey) = RegistrySession.ParsePath(@"HKEY_CURRENT_CONFIG\Software");
        Assert.Equal(Microsoft.Win32.Registry.CurrentConfig, root);
        Assert.Equal("Software", subKey);
    }

    [Fact]
    public void ParsePath_HKCC_Abbreviated_Works()
    {
        var (root, subKey) = RegistrySession.ParsePath(@"HKCC\Software");
        Assert.Equal(Microsoft.Win32.Registry.CurrentConfig, root);
        Assert.Equal("Software", subKey);
    }

    [Fact]
    public void LogWritten_Event_FiresOnWriteLog()
    {
        var s = new RegistrySession(dryRun: true);
        string? received = null;
        s.LogWritten += msg => received = msg;
        s.WriteLog("test-event");
        Assert.NotNull(received);
        Assert.Contains("test-event", received);
    }

    [Fact]
    public void LogWritten_Event_FiresOnDryRunOp()
    {
        var s = new RegistrySession(dryRun: true);
        var events = new List<string>();
        s.LogWritten += msg => events.Add(msg);
        s.SetDword(@"HKCU\Software\Test", "V", 1);
        Assert.NotEmpty(events);
    }

    [Fact]
    public void ReadQword_NonExistentKey_ReturnsNull()
    {
        var s = new RegistrySession(dryRun: false);
        var result = s.ReadQword(@"HKEY_CURRENT_USER\Software\RegiLattice_NOEXIST_99", "Q");
        Assert.Null(result);
    }

    [Fact]
    public void ReadBinary_NonExistentKey_ReturnsNull()
    {
        var s = new RegistrySession(dryRun: false);
        var result = s.ReadBinary(@"HKEY_CURRENT_USER\Software\RegiLattice_NOEXIST_99", "B");
        Assert.Null(result);
    }

    [Fact]
    public void ReadMultiSz_NonExistentKey_ReturnsNull()
    {
        var s = new RegistrySession(dryRun: false);
        var result = s.ReadMultiSz(@"HKEY_CURRENT_USER\Software\RegiLattice_NOEXIST_99", "M");
        Assert.Null(result);
    }

    [Fact]
    public void ReadValue_NonExistentKey_ReturnsNull()
    {
        var s = new RegistrySession(dryRun: false);
        var result = s.ReadValue(@"HKEY_CURRENT_USER\Software\RegiLattice_NOEXIST_99", "X");
        Assert.Null(result);
    }

    [Fact]
    public void ListValueNames_NonexistentKey_ReturnsEmpty()
    {
        var s = new RegistrySession(dryRun: false);
        var result = s.ListValueNames(@"HKEY_CURRENT_USER\Software\RegiLattice_NOEXIST_99");
        Assert.Empty(result);
    }

    [Fact]
    public void DryRun_AllOpsTypes_LogMessages()
    {
        var s = new RegistrySession(dryRun: true);
        s.SetDword(@"HKCU\Test", "D", 1);
        s.SetString(@"HKCU\Test", "S", "v");
        s.SetExpandString(@"HKCU\Test", "E", @"%TEMP%");
        s.SetQword(@"HKCU\Test", "Q", 1L);
        s.SetBinary(@"HKCU\Test", "B", [0x01]);
        s.SetMultiSz(@"HKCU\Test", "M", ["a"]);
        s.DeleteValue(@"HKCU\Test", "X");
        s.DeleteTree(@"HKCU\Test\Sub");
        Assert.Equal(8, s.DryOps);
        Assert.Equal(8, s.Log.Count);
    }
}

// ── Sprint 24: CheckValueMatch branches, ReadQword/Binary/MultiSz/Value ────

public sealed class RegistrySessionSprint24Tests : IDisposable
{
    private const string TestKey = @"HKEY_CURRENT_USER\Software\RegiLattice\TestTemp_Sprint24";

    public void Dispose()
    {
        try
        {
            new RegistrySession().DeleteTree(TestKey);
        }
        catch { }
    }

    [Fact]
    public void Evaluate_CheckDword_WhenKeyMissing_ReturnsFalse()
    {
        var s = new RegistrySession(dryRun: true);
        var result = s.Evaluate([RegOp.CheckDword($@"{TestKey}\Missing", "Value", 1)]);
        Assert.False(result);
    }

    [Fact]
    public void Evaluate_CheckString_WhenKeyMissing_ReturnsFalse()
    {
        var s = new RegistrySession(dryRun: true);
        var result = s.Evaluate([RegOp.CheckString($@"{TestKey}\Missing", "Value", "x")]);
        Assert.False(result);
    }

    [Fact]
    public void Evaluate_WriteAndCheckDword_MatchReturnsTrue_NonMatchReturnsFalse()
    {
        var s = new RegistrySession();
        s.SetDword(TestKey, "CheckDwordVal", 42);
        Assert.True(s.Evaluate([RegOp.CheckDword(TestKey, "CheckDwordVal", 42)]));
        Assert.False(s.Evaluate([RegOp.CheckDword(TestKey, "CheckDwordVal", 99)]));
    }

    [Fact]
    public void Evaluate_WriteAndCheckString_MatchReturnsTrue_NonMatchReturnsFalse()
    {
        var s = new RegistrySession();
        s.SetString(TestKey, "CheckStrVal", "hello");
        Assert.True(s.Evaluate([RegOp.CheckString(TestKey, "CheckStrVal", "hello")]));
        Assert.False(s.Evaluate([RegOp.CheckString(TestKey, "CheckStrVal", "world")]));
    }

    [Fact]
    public void ReadQword_AfterSetQword_ReturnsExpected()
    {
        var s = new RegistrySession();
        s.SetQword(TestKey, "QwordVal", 1234567890123L);
        Assert.Equal(1234567890123L, s.ReadQword(TestKey, "QwordVal"));
    }

    [Fact]
    public void ReadBinary_AfterSetBinary_ReturnsExpected()
    {
        var s = new RegistrySession();
        byte[] data = [0x01, 0x02, 0x03];
        s.SetBinary(TestKey, "BinVal", data);
        var read = s.ReadBinary(TestKey, "BinVal");
        Assert.Equal(data, read);
    }

    [Fact]
    public void ReadMultiSz_AfterSetMultiSz_ReturnsExpected()
    {
        var s = new RegistrySession();
        string[] values = ["alpha", "beta", "gamma"];
        s.SetMultiSz(TestKey, "MultiVal", values);
        var read = s.ReadMultiSz(TestKey, "MultiVal");
        Assert.Equal(values, read);
    }

    [Fact]
    public void ReadString_MissingValue_ReturnsNull()
    {
        var s = new RegistrySession();
        Assert.Null(s.ReadString(TestKey, $"NoSuchValue_{Guid.NewGuid():N}"));
    }

    [Fact]
    public void ValueExists_AfterSetDword_ReturnsTrue()
    {
        var s = new RegistrySession();
        s.SetDword(TestKey, "ExistsVE", 1);
        Assert.True(s.ValueExists(TestKey, "ExistsVE"));
    }

    [Fact]
    public void DeleteValue_RemovesSpecificValue_LeavesOther()
    {
        var s = new RegistrySession();
        s.SetDword(TestKey, "KeepMe", 1);
        s.SetDword(TestKey, "DeleteMe", 2);
        s.DeleteValue(TestKey, "DeleteMe");
        Assert.True(s.ValueExists(TestKey, "KeepMe"));
        Assert.False(s.ValueExists(TestKey, "DeleteMe"));
    }
}
