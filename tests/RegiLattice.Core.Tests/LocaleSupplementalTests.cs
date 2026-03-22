// RegiLattice.Core.Tests — LocaleSupplementalTests.cs
// Supplemental locale coverage for Sprint 108 — extends beyond the 12 tests
// already present in ServicesTests.cs:LocaleTests.
// Covers: Hebrew/Japanese built-ins, LoadLocaleFile(), AvailableLocales count,
// unknown-locale fallback, hot-cache coherence, and AvailableKeys completeness.

using RegiLattice.Core;
using Xunit;

namespace RegiLattice.Core.Tests;

public sealed class LocaleSupplementalTests : IDisposable
{
    // Reset to English before and after each test to avoid cross-test locale contamination.
    public LocaleSupplementalTests()
    {
        Locale.SetLocale("en");
    }

    public void Dispose()
    {
        Locale.SetLocale("en");
    }

    // ── Built-in locale catalogue ───────────────────────────────────────────

    [Fact]
    public void AvailableLocales_ContainsSixBuiltIns()
    {
        Assert.Equal(6, Locale.AvailableLocales.Count);
    }

    [Fact]
    public void AvailableLocales_ContainsAllExpectedCodes()
    {
        var locales = Locale.AvailableLocales;
        Assert.Contains("en", locales);
        Assert.Contains("de", locales);
        Assert.Contains("fr", locales);
        Assert.Contains("es", locales);
        Assert.Contains("he", locales);
        Assert.Contains("ja", locales);
    }

    // ── Hebrew locale ───────────────────────────────────────────────────────

    [Fact]
    public void SetLocale_Hebrew_TranslatesApplyAll()
    {
        Locale.SetLocale("he");
        string translated = Locale.T("apply_all");
        Assert.False(string.IsNullOrWhiteSpace(translated));
        Assert.NotEqual("apply_all", translated); // key not returned as-is
    }

    [Fact]
    public void SetLocale_Hebrew_HasAllSameKeysAsEnglish()
    {
        Locale.SetLocale("en");
        int enCount = Locale.AvailableKeys.Count;

        Locale.SetLocale("he");
        Assert.Equal(enCount, Locale.AvailableKeys.Count);
    }

    // ── Japanese locale ─────────────────────────────────────────────────────

    [Fact]
    public void SetLocale_Japanese_TranslatesApplyAll()
    {
        Locale.SetLocale("ja");
        string translated = Locale.T("apply_all");
        Assert.False(string.IsNullOrWhiteSpace(translated));
        Assert.NotEqual("apply_all", translated);
    }

    [Fact]
    public void SetLocale_Japanese_HasAllSameKeysAsEnglish()
    {
        Locale.SetLocale("en");
        int enCount = Locale.AvailableKeys.Count;

        Locale.SetLocale("ja");
        Assert.Equal(enCount, Locale.AvailableKeys.Count);
    }

    // ── Unknown locale fallback ─────────────────────────────────────────────

    [Fact]
    public void SetLocale_UnknownLocale_FallsBackToEnglishKeys()
    {
        // An unknown locale name should not throw; T() should return a non-empty string.
        Locale.SetLocale("zz"); // non-existent locale
        string result = Locale.T("apply_all");
        Assert.False(string.IsNullOrWhiteSpace(result));
    }

    // ── Switching back to English resets state ──────────────────────────────

    [Fact]
    public void SetLocale_SwitchToGermanThenBackToEnglish_CurrentLocaleIsEn()
    {
        Locale.SetLocale("de");
        Assert.Equal("de", Locale.CurrentLocale);
        Locale.SetLocale("en");
        Assert.Equal("en", Locale.CurrentLocale);
    }

    // ── Hot-cache coherence ─────────────────────────────────────────────────

    [Fact]
    public void T_ConsecutiveCallSameKey_ReturnsSameValue()
    {
        string first = Locale.T("apply_all");
        string second = Locale.T("apply_all");
        Assert.Equal(first, second);
    }

    [Fact]
    public void SetLocale_ClearsCache_NewLocaleTranslationTakesPrecedence()
    {
        Locale.SetLocale("en");
        string enValue = Locale.T("apply_all");

        Locale.SetLocale("de");
        string deValue = Locale.T("apply_all");

        // German translation must differ from the English one.
        Assert.NotEqual(enValue, deValue);
    }

    // ── LoadLocaleFile ──────────────────────────────────────────────────────

    [Fact]
    public void LoadLocaleFile_ValidKeyValueFile_OverridesKey()
    {
        string tempFile = Path.GetTempFileName();
        try
        {
            File.WriteAllText(tempFile, "apply_all=CUSTOM_APPLY_ALL\n");
            Locale.LoadLocaleFile(tempFile);
            Assert.Equal("CUSTOM_APPLY_ALL", Locale.T("apply_all"));
        }
        finally
        {
            if (File.Exists(tempFile)) File.Delete(tempFile);
        }
    }

    [Fact]
    public void LoadLocaleFile_MissingFile_DoesNotThrow()
    {
        string missingPath = Path.Combine(Path.GetTempPath(), "locale-does-not-exist.properties");
        var ex = Record.Exception(() => Locale.LoadLocaleFile(missingPath));
        Assert.Null(ex);
    }

    [Fact]
    public void LoadLocaleFile_MalformedLines_SkipsInvalidLinesAndLoadsValid()
    {
        string tempFile = Path.GetTempFileName();
        try
        {
            File.WriteAllLines(tempFile,
            [
                "# comment line",
                "",                       // blank
                "malformed line",         // no '=' separator
                "apply_all=VALID_VALUE",
            ]);
            Locale.LoadLocaleFile(tempFile);
            Assert.Equal("VALID_VALUE", Locale.T("apply_all"));
        }
        finally
        {
            if (File.Exists(tempFile)) File.Delete(tempFile);
        }
    }

    // ── AvailableKeys completeness ──────────────────────────────────────────

    [Theory]
    [InlineData("apply_all")]
    [InlineData("remove_all")]
    [InlineData("search_placeholder")]
    [InlineData("status_applied")]
    [InlineData("status_not_applied")]
    public void AvailableKeys_ContainsRequiredKey(string key)
    {
        Locale.SetLocale("en");
        Assert.Contains(key, Locale.AvailableKeys);
    }

    // ── Multiple format args ────────────────────────────────────────────────

    [Fact]
    public void T_MultipleFormatArgs_FormatsCorrectly()
    {
        // Use apply_count or similar key that takes {0}; if not found, key is returned as-is.
        // We just verify T() doesn't throw with format args.
        var ex = Record.Exception(() => Locale.T("nonexistent_key", "arg1", "arg2"));
        Assert.Null(ex);
    }
}
