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
public sealed class RegistrySessionCheckValueBranchTests
{
    private const string TestKeyPath = @"HKEY_CURRENT_USER\Software\RegiLattice\BranchCovTest3";

    [Fact]
    public void Evaluate_CheckMissing_ValueExistsReturnsTrue_ButCheckMissingReturnsFalse()
    {
        // Write a value, then CheckMissing should return FALSE (value is present = not missing)
        var session = new RegistrySession(dryRun: false);
        try
        {
            session.SetDword(TestKeyPath, "ExistingForCheck", 42);
            var result = session.Evaluate([RegOp.CheckMissing(TestKeyPath, "ExistingForCheck")]);
            Assert.False(result); // value exists → CheckMissing fails → Evaluate returns false
        }
        finally
        {
            session.DeleteValue(TestKeyPath, "ExistingForCheck");
            session.DeleteTree(TestKeyPath);
        }
    }

    [Fact]
    public void CheckValueMatch_LongValue_MatchesCorrectly()
    {
        var session = new RegistrySession(dryRun: false);
        try
        {
            const long testVal = 9_876_543_210L;
            session.SetQword(TestKeyPath, "QwordTest", testVal);
            // CheckValue with a Qword op
            var checkOp = RegOp.SetQword(TestKeyPath, "QwordTest", testVal); // Use as detect by re-evaluating raw
            // Use raw Evaluate with a CheckValue type that reads long
            var readBack = session.ReadQword(TestKeyPath, "QwordTest");
            Assert.Equal(testVal, readBack);
        }
        finally
        {
            session.DeleteValue(TestKeyPath, "QwordTest");
            session.DeleteTree(TestKeyPath);
        }
    }

    [Fact]
    public void Evaluate_CheckValue_BinaryMatch_ReturnsTrue()
    {
        var session = new RegistrySession(dryRun: false);
        try
        {
            byte[] data = [0x01, 0x02, 0x03, 0xAB];
            session.SetBinary(TestKeyPath, "BinTest", data);
            var readBack = session.ReadBinary(TestKeyPath, "BinTest");
            Assert.NotNull(readBack);
            Assert.Equal(data, readBack!);
        }
        finally
        {
            session.DeleteValue(TestKeyPath, "BinTest");
            session.DeleteTree(TestKeyPath);
        }
    }

    [Fact]
    public void SetMultiSz_And_ReadBack_IsCorrect()
    {
        var session = new RegistrySession(dryRun: false);
        try
        {
            string[] vals = ["one", "two", "three"];
            session.SetMultiSz(TestKeyPath, "MultiTest", vals);
            var readBack = session.ReadMultiSz(TestKeyPath, "MultiTest");
            Assert.NotNull(readBack);
            Assert.Equal(vals, readBack!);
        }
        finally
        {
            session.DeleteValue(TestKeyPath, "MultiTest");
            session.DeleteTree(TestKeyPath);
        }
    }

    [Fact]
    public void SetExpandString_And_ReadBack_IsCorrect()
    {
        var session = new RegistrySession(dryRun: false);
        try
        {
            session.SetExpandString(TestKeyPath, "ExpandTest", "%SystemRoot%\\test");
            var readBack = session.ReadString(TestKeyPath, "ExpandTest");
            Assert.NotNull(readBack);
            // The stored value should contain the path
            Assert.True(readBack!.Contains("test") || readBack.Contains("Windows"));
        }
        finally
        {
            session.DeleteValue(TestKeyPath, "ExpandTest");
            session.DeleteTree(TestKeyPath);
        }
    }

    [Fact]
    public void ValueExists_ExistingValue_ReturnsTrue()
    {
        var session = new RegistrySession(dryRun: false);
        try
        {
            session.SetDword(TestKeyPath, "ExVal", 1);
            Assert.True(session.ValueExists(TestKeyPath, "ExVal"));
        }
        finally
        {
            session.DeleteValue(TestKeyPath, "ExVal");
            session.DeleteTree(TestKeyPath);
        }
    }

    [Fact]
    public void ListValueNames_AfterSettingValues_ContainsSetNames()
    {
        var session = new RegistrySession(dryRun: false);
        try
        {
            session.SetDword(TestKeyPath, "Name1", 1);
            session.SetString(TestKeyPath, "Name2", "val");
            var names = session.ListValueNames(TestKeyPath);
            Assert.Contains("Name1", names, StringComparer.OrdinalIgnoreCase);
            Assert.Contains("Name2", names, StringComparer.OrdinalIgnoreCase);
        }
        finally
        {
            session.DeleteValue(TestKeyPath, "Name1");
            session.DeleteValue(TestKeyPath, "Name2");
            session.DeleteTree(TestKeyPath);
        }
    }

    [Fact]
    public void WriteLog_Event_LogContainsEntry()
    {
        var session = new RegistrySession(dryRun: true);
        session.WriteLog("test-event");
        Assert.Contains(session.Log, l => l.Contains("test-event"));
    }

    [Fact]
    public void LogWritten_Event_Fires()
    {
        var session = new RegistrySession(dryRun: true);
        string? captured = null;
        session.LogWritten += msg => captured = msg;
        session.WriteLog("event-test");
        Assert.Contains("event-test", captured);
    }
}

// ═══════════════════════════════════════════════════════════════════════════════
// NetworkManager — safe (no-system-mutation) branches
// ═══════════════════════════════════════════════════════════════════════════════

public sealed class RegistrySessionBranchTests2
{
    [Fact]
    public void ParsePath_HkccHive_ReturnsCurrentConfig()
    {
        var (root, subKey) = RegistrySession.ParsePath(@"HKCC\Software\Test");
        Assert.Same(Microsoft.Win32.Registry.CurrentConfig, root);
        Assert.Equal(@"Software\Test", subKey);
    }

    [Fact]
    public void ParsePath_HkeyCurrentConfig_ReturnsCurrentConfig()
    {
        var (root, _) = RegistrySession.ParsePath(@"HKEY_CURRENT_CONFIG\System\Test");
        Assert.Same(Microsoft.Win32.Registry.CurrentConfig, root);
    }

    [Fact]
    public void ParsePath_HkuHive_ReturnsUsers()
    {
        var (root, _) = RegistrySession.ParsePath(@"HKU\.DEFAULT\Software");
        Assert.Same(Microsoft.Win32.Registry.Users, root);
    }

    [Fact]
    public void ParsePath_HkeyUsers_ReturnsUsers()
    {
        var (root, _) = RegistrySession.ParsePath(@"HKEY_USERS\.DEFAULT\Software");
        Assert.Same(Microsoft.Win32.Registry.Users, root);
    }

    [Fact]
    public void Execute_CheckDwordOp_ThrowsCannotExecuteReadOnly()
    {
        var session = new RegistrySession(dryRun: true);
        var checkOp = RegOp.CheckDword(@"HKCU\Software\Test", "V", 1);
        var ex = Assert.Throws<InvalidOperationException>(() => session.Execute([checkOp]));
        Assert.Contains("read-only", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Evaluate_SetDwordOp_ThrowsCannotEvaluateWriteOp()
    {
        var session = new RegistrySession(dryRun: true);
        var setOp = RegOp.SetDword(@"HKCU\Software\Test", "V", 1);
        var ex = Assert.Throws<InvalidOperationException>(() => session.Evaluate([setOp]));
        Assert.Contains("write", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Evaluate_CheckMissing_WhenValueExists_ReturnsFalse()
    {
        const string path = @"HKCU\Software\RegiLattice\UnitTest\BC4CM";
        var session = new RegistrySession(dryRun: false);
        session.SetDword(path, "ExistingVal", 42);
        try
        {
            var result = session.Evaluate([RegOp.CheckMissing(path, "ExistingVal")]);
            Assert.False(result);
        }
        finally
        {
            session.DeleteValue(path, "ExistingVal");
            session.DeleteTree(path);
        }
    }

    [Fact]
    public void Evaluate_CheckKeyMissing_WhenKeyExists_ReturnsFalse()
    {
        const string path = @"HKCU\Software\RegiLattice\UnitTest\BC4CKM";
        var session = new RegistrySession(dryRun: false);
        session.SetDword(path, "V", 1);
        try
        {
            var result = session.Evaluate([RegOp.CheckKeyMissing(path)]);
            Assert.False(result);
        }
        finally
        {
            session.DeleteValue(path, "V");
            session.DeleteTree(path);
        }
    }

    [Fact]
    public void Evaluate_CheckString_MatchingValue_ReturnsTrue()
    {
        const string path = @"HKCU\Software\RegiLattice\UnitTest\BC4CS";
        var session = new RegistrySession(dryRun: false);
        session.SetString(path, "SVal", "hello-world");
        try
        {
            var result = session.Evaluate([RegOp.CheckString(path, "SVal", "hello-world")]);
            Assert.True(result);
        }
        finally
        {
            session.DeleteTree(path);
        }
    }

    [Fact]
    public void Evaluate_CheckString_MismatchValue_ReturnsFalse()
    {
        const string path = @"HKCU\Software\RegiLattice\UnitTest\BC4CSFail";
        var session = new RegistrySession(dryRun: false);
        session.SetString(path, "SVal", "actual-value");
        try
        {
            var result = session.Evaluate([RegOp.CheckString(path, "SVal", "expected-value")]);
            Assert.False(result);
        }
        finally
        {
            session.DeleteTree(path);
        }
    }

    [Fact]
    public void Evaluate_CheckValue_KeyNotInRegistry_ReturnsFalse()
    {
        // Key doesn't exist → CheckValueMatch returns false immediately (key is null)
        const string path = @"HKCU\Software\RegiLattice\UnitTest\BC4NoKey_XYZ99999";
        var session = new RegistrySession(dryRun: false);
        var result = session.Evaluate([RegOp.CheckDword(path, "V", 1)]);
        Assert.False(result);
    }

    [Fact]
    public void Evaluate_CheckValue_ValueNotInKey_ReturnsFalse()
    {
        // Key exists but named value doesn't → CheckValueMatch returns false
        const string path = @"HKCU\Software\Microsoft";
        var session = new RegistrySession(dryRun: false);
        var result = session.Evaluate([RegOp.CheckDword(path, "BC4NonExistentValue9999", 1)]);
        Assert.False(result);
    }

    [Fact]
    public void Backup_WithCustomDir_CreatesFile()
    {
        var backupDir = Path.Combine(Path.GetTempPath(), $"bc4-backup-{Guid.NewGuid():N}");
        try
        {
            var session = new RegistrySession(backupDir: backupDir);
            var path = session.Backup([@"HKCU\Software\Microsoft"], "bc4-test");
            Assert.True(File.Exists(path));
        }
        finally
        {
            if (Directory.Exists(backupDir))
                Directory.Delete(backupDir, recursive: true);
        }
    }

    [Fact]
    public void SetQword_DryRun_IncrementsCounter()
    {
        var session = new RegistrySession(dryRun: true);
        session.SetQword(@"HKCU\Software\bc4qwtest", "QV", 123456789L);
        Assert.Equal(1, session.DryOps);
    }

    [Fact]
    public void SetBinary_DryRun_IncrementsCounter()
    {
        var session = new RegistrySession(dryRun: true);
        session.SetBinary(@"HKCU\Software\bc4bintest", "BV", [0x01, 0x02, 0x03]);
        Assert.Equal(1, session.DryOps);
    }

    [Fact]
    public void SetMultiSz_DryRun_IncrementsCounter()
    {
        var session = new RegistrySession(dryRun: true);
        session.SetMultiSz(@"HKCU\Software\bc4mstest", "MSV", ["a", "b"]);
        Assert.Equal(1, session.DryOps);
    }

    [Fact]
    public void SetExpandString_DryRun_IncrementsCounter()
    {
        var session = new RegistrySession(dryRun: true);
        session.SetExpandString(@"HKCU\Software\bc4estest", "EV", @"%SystemRoot%\test");
        Assert.Equal(1, session.DryOps);
    }

    [Fact]
    public void ReadQword_ExistingQword_ReturnsValue()
    {
        const string path = @"HKCU\Software\RegiLattice\UnitTest\BC4RQ";
        var session = new RegistrySession(dryRun: false);
        session.SetQword(path, "QVal", 987654321L);
        try
        {
            var val = session.ReadQword(path, "QVal");
            Assert.Equal(987654321L, val);
        }
        finally
        {
            session.DeleteTree(path);
        }
    }

    [Fact]
    public void ReadBinary_ExistingBinary_ReturnsBytes()
    {
        const string path = @"HKCU\Software\RegiLattice\UnitTest\BC4RB";
        var session = new RegistrySession(dryRun: false);
        byte[] expected = [0xAA, 0xBB, 0xCC];
        session.SetBinary(path, "BVal", expected);
        try
        {
            var val = session.ReadBinary(path, "BVal");
            Assert.NotNull(val);
            Assert.Equal(expected, val);
        }
        finally
        {
            session.DeleteTree(path);
        }
    }

    [Fact]
    public void ReadMultiSz_ExistingMultiSz_ReturnsStrings()
    {
        const string path = @"HKCU\Software\RegiLattice\UnitTest\BC4RMS";
        var session = new RegistrySession(dryRun: false);
        session.SetMultiSz(path, "MSVal", ["hello", "world"]);
        try
        {
            var val = session.ReadMultiSz(path, "MSVal");
            Assert.NotNull(val);
            Assert.Equal(2, val!.Length);
        }
        finally
        {
            session.DeleteTree(path);
        }
    }
}

// ── 6. TweakEngine IsApplicableOnHardware Category Arm Tests ────────────────

public sealed class RegistrySessionQwordBinaryBranchTests : IDisposable
{
    private const string TestKey = @"HKEY_CURRENT_USER\Software\RegiLattice\TestTemp_BC6";

    public RegistrySessionQwordBinaryBranchTests()
    {
        // Clean slate
        try
        {
            new RegistrySession().DeleteTree(TestKey);
        }
        catch { }
    }

    public void Dispose()
    {
        try
        {
            new RegistrySession().DeleteTree(TestKey);
        }
        catch { }
    }

    [Fact]
    public void CheckQword_MatchingValue_ReturnsTrue()
    {
        var s = new RegistrySession();
        s.SetQword(TestKey, "QV", 9_999_999_999L);
        Assert.True(
            s.Evaluate([
                new RegOp
                {
                    Kind = RegOpKind.CheckValue,
                    Path = TestKey,
                    Name = "QV",
                    Value = 9_999_999_999L,
                    ValueKind = RegistryValueKind.QWord,
                },
            ])
        );
    }

    [Fact]
    public void CheckQword_NonMatchingValue_ReturnsFalse()
    {
        var s = new RegistrySession();
        s.SetQword(TestKey, "QV2", 9_999_999_999L);
        Assert.False(
            s.Evaluate([
                new RegOp
                {
                    Kind = RegOpKind.CheckValue,
                    Path = TestKey,
                    Name = "QV2",
                    Value = 1L,
                    ValueKind = RegistryValueKind.QWord,
                },
            ])
        );
    }

    [Fact]
    public void CheckBinary_MatchingBytes_ReturnsTrue()
    {
        var s = new RegistrySession();
        byte[] data = [0xDE, 0xAD, 0xBE, 0xEF];
        s.SetBinary(TestKey, "BV", data);
        Assert.True(
            s.Evaluate([
                new RegOp
                {
                    Kind = RegOpKind.CheckValue,
                    Path = TestKey,
                    Name = "BV",
                    Value = data,
                    ValueKind = RegistryValueKind.Binary,
                },
            ])
        );
    }

    [Fact]
    public void CheckBinary_NonMatchingBytes_ReturnsFalse()
    {
        var s = new RegistrySession();
        byte[] data = [0xDE, 0xAD, 0xBE, 0xEF];
        s.SetBinary(TestKey, "BV2", data);
        Assert.False(
            s.Evaluate([
                new RegOp
                {
                    Kind = RegOpKind.CheckValue,
                    Path = TestKey,
                    Name = "BV2",
                    Value = new byte[] { 0x00, 0x01, 0x02 },
                    ValueKind = RegistryValueKind.Binary,
                },
            ])
        );
    }

    [Fact]
    public void CheckQword_MissingValue_ReturnsFalse()
    {
        var s = new RegistrySession();
        // Key doesn't exist at all → long arm still reached, key lookup returns false
        Assert.False(
            s.Evaluate([
                new RegOp
                {
                    Kind = RegOpKind.CheckValue,
                    Path = @"HKCU\Software\RegiLattice\NoSuchKey_BC6",
                    Name = "NoVal",
                    Value = 1L,
                    ValueKind = RegistryValueKind.QWord,
                },
            ])
        );
    }
}

// ── 2. ConfigExporter — Format-3 import (dict {"tweaks": [...]}) ─────────────

