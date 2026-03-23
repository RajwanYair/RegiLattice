#nullable enable

namespace RegiLattice.Core.Tests;

using System;
using Microsoft.Win32;
using RegiLattice.Core;
using RegiLattice.Core.Models;
using RegiLattice.Core.Registry;
using Xunit;

/// <summary>
/// T6.3 — Virtual registry integration tests.
///
/// Uses a GUID-scoped HKCU test key so the Apply → Detect → Remove cycle
/// runs against the real Windows registry without administrator rights and
/// passes unchanged on GitHub Actions CI (<c>windows-latest</c>).
///
/// Design choice vs. RegLoadKey/RegUnLoadKey:
///   RegLoadKey requires SeRestorePrivilege (admin). Loading an isolated .hive
///   file is therefore not feasible on a standard CI runner. The HKCU-subkey
///   approach provides the same isolation guarantee for CI purposes:
///   - Each test instance owns a unique GUID key.
///   - The key is created on demand and deleted in <see cref="Dispose"/>.
///   - No test can observe another test's key.
/// </summary>
public sealed class VirtualRegistryTests : IDisposable
{
    // Root namespace for all integration-test keys.
    private const string TestRootRelative = @"Software\RegiLattice\IntegrationTests";

    // Full HKCU path for THIS test instance's isolated hive, e.g.
    // HKEY_CURRENT_USER\Software\RegiLattice\IntegrationTests\<guid>
    private readonly string _hive;
    private readonly string _hiveGuid;
    private readonly RegistrySession _session;

    public VirtualRegistryTests()
    {
        _hiveGuid = Guid.NewGuid().ToString("N");
        _hive = $@"HKEY_CURRENT_USER\{TestRootRelative}\{_hiveGuid}";
        _session = new RegistrySession(dryRun: false);
    }

    /// <summary>Deletes the GUID-scoped isolation key tree after each test.</summary>
    public void Dispose()
    {
        try
        {
            using RegistryKey? parent = Registry.CurrentUser.OpenSubKey(TestRootRelative, writable: true);
            parent?.DeleteSubKeyTree(_hiveGuid, throwOnMissingSubKey: false);
        }
        catch (Exception)
        {
            // Best-effort cleanup — isolation key may already be absent.
        }
    }

    // Helper: full HKCU path with optional sub-key appended.
    private string P(string subKey = "") => string.IsNullOrEmpty(subKey) ? _hive : $@"{_hive}\{subKey}";

    // ── Basic read/write cycles ───────────────────────────────────────────────

    [Fact]
    public void SetDword_RealRegistry_ValuePersists()
    {
        _session.SetDword(P(), "IntVal", 12345);

        Assert.Equal(12345, _session.ReadDword(P(), "IntVal"));
    }

    [Fact]
    public void SetString_RealRegistry_ValuePersists()
    {
        _session.SetString(P(), "StrVal", "hello-world");

        Assert.Equal("hello-world", _session.ReadString(P(), "StrVal"));
    }

    [Fact]
    public void SetQword_RealRegistry_ValuePersists()
    {
        _session.SetQword(P(), "BigNum", 9_876_543_210L);

        Assert.Equal(9_876_543_210L, _session.ReadQword(P(), "BigNum"));
    }

    [Fact]
    public void SetBinary_RealRegistry_ValuePersists()
    {
        byte[] data = [0xDE, 0xAD, 0xBE, 0xEF];
        _session.SetBinary(P(), "BinVal", data);

        Assert.Equal(data, _session.ReadBinary(P(), "BinVal"));
    }

    [Fact]
    public void SetMultiSz_RealRegistry_ValuePersists()
    {
        string[] lines = ["alpha", "beta", "gamma"];
        _session.SetMultiSz(P(), "MultiVal", lines);

        Assert.Equal(lines, _session.ReadMultiSz(P(), "MultiVal"));
    }

    // ── Delete operations ─────────────────────────────────────────────────────

    [Fact]
    public void DeleteValue_RealRegistry_ValueGone()
    {
        _session.SetDword(P(), "Temp", 1);
        Assert.True(_session.ValueExists(P(), "Temp"), "Value should exist after SetDword.");

        _session.DeleteValue(P(), "Temp");

        Assert.False(_session.ValueExists(P(), "Temp"), "Value should be absent after DeleteValue.");
    }

    [Fact]
    public void DeleteTree_RealRegistry_SubKeyGone()
    {
        _session.SetDword(P("Sub"), "V", 1);
        Assert.True(_session.KeyExists(P("Sub")), "Sub-key should exist after SetDword.");

        _session.Execute([RegOp.DeleteTree(P("Sub"))]);

        Assert.False(_session.KeyExists(P("Sub")), "Sub-key should be absent after DeleteTree.");
    }

    // ── Evaluate (detection) ──────────────────────────────────────────────────

    [Fact]
    public void Evaluate_CheckDword_TrueWhenValueMatches()
    {
        _session.SetDword(P(), "Flag", 1);
        IReadOnlyList<RegOp> ops = [RegOp.CheckDword(P(), "Flag", 1)];

        Assert.True(_session.Evaluate(ops));
    }

    [Fact]
    public void Evaluate_CheckDword_FalseWhenValueDiffers()
    {
        _session.SetDword(P(), "Flag", 0);
        IReadOnlyList<RegOp> ops = [RegOp.CheckDword(P(), "Flag", 1)];

        Assert.False(_session.Evaluate(ops));
    }

    [Fact]
    public void Evaluate_CheckMissing_TrueBeforeSet()
    {
        IReadOnlyList<RegOp> ops = [RegOp.CheckMissing(P(), "Ghost")];

        Assert.True(_session.Evaluate(ops), "CheckMissing should return true when value is absent.");
    }

    [Fact]
    public void Evaluate_CheckMissing_FalseAfterSet()
    {
        _session.SetDword(P(), "Ghost", 99);
        IReadOnlyList<RegOp> ops = [RegOp.CheckMissing(P(), "Ghost")];

        Assert.False(_session.Evaluate(ops), "CheckMissing should return false when value is present.");
    }

    [Fact]
    public void Evaluate_CheckKeyMissing_TrueForNonExistentSubKey()
    {
        IReadOnlyList<RegOp> ops = [RegOp.CheckKeyMissing(P("NoSuchKey"))];

        Assert.True(_session.Evaluate(ops));
    }

    // ── Full Apply → Detect → Remove round-trip via Execute ──────────────────

    [Fact]
    public void Execute_SetDword_CheckDword_Delete_RoundTrip()
    {
        IReadOnlyList<RegOp> applyOps = [RegOp.SetDword(P(), "Cycle", 7)];
        IReadOnlyList<RegOp> detectOps = [RegOp.CheckDword(P(), "Cycle", 7)];
        IReadOnlyList<RegOp> removeOps = [RegOp.DeleteValue(P(), "Cycle")];

        // 1. Apply
        _session.Execute(applyOps);
        Assert.True(_session.Evaluate(detectOps), "Value should be detected after Apply.");

        // 2. Remove
        _session.Execute(removeOps);
        Assert.False(_session.Evaluate(detectOps), "Value should not be detected after Remove.");
    }

    // ── Full Apply → Detect → Remove via TweakEngine (real cycle) ────────────

    [Fact]
    public void TweakEngine_Apply_Detect_Remove_RealRegistryCycle()
    {
        // Full round-trip through TweakEngine using a real (non-DryRun) session.
        var engine = new TweakEngine(new RegistrySession(dryRun: false));
        string valueName = "EngineTest";

        var tweak = new TweakDef
        {
            Id = "it-virtual-reg-engine-cycle",
            Label = "IT Virtual Registry Engine Cycle",
            Category = "Test",
            NeedsAdmin = false,
            CorpSafe = true,  // must be true so CorporateGuard doesn't block in corporate CI
            Tags = ["integration", "virtual-registry"],
            ApplyOps = [RegOp.SetDword(P(), valueName, 1)],
            RemoveOps = [RegOp.DeleteValue(P(), valueName)],
            DetectOps = [RegOp.CheckDword(P(), valueName, 1)],
        };
        engine.Register([tweak]);

        // Pre-condition: not applied
        Assert.Equal(TweakResult.NotApplied, engine.DetectStatus(tweak));

        // Apply
        engine.Apply(tweak);
        Assert.Equal(TweakResult.Applied, engine.DetectStatus(tweak));

        // Remove
        engine.Remove(tweak);
        Assert.Equal(TweakResult.NotApplied, engine.DetectStatus(tweak));
    }

    // ── DryRun isolation guard ────────────────────────────────────────────────

    [Fact]
    public void DryRun_Session_DoesNotWriteToRealRegistry()
    {
        var drySession = new RegistrySession(dryRun: true);

        // DryRun write — must NOT appear in the real registry.
        drySession.SetDword(P(), "ShouldNotExist", 42);

        Assert.NotEqual(0, drySession.DryOps);
        Assert.False(_session.ValueExists(P(), "ShouldNotExist"), "DryRun write must not appear in real registry.");
    }
}
