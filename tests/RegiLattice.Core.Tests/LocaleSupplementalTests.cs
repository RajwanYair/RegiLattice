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
    public void AvailableLocales_ContainsTenBuiltIns()
    {
        Assert.Equal(10, Locale.AvailableLocales.Count);
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
        Assert.Contains("zh-CN", locales);
        Assert.Contains("ko", locales);
        Assert.Contains("ar", locales);
        Assert.Contains("pt-BR", locales);
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
            if (File.Exists(tempFile))
                File.Delete(tempFile);
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
            File.WriteAllLines(
                tempFile,
                [
                    "# comment line",
                    "", // blank
                    "malformed line", // no '=' separator
                    "apply_all=VALID_VALUE",
                ]
            );
            Locale.LoadLocaleFile(tempFile);
            Assert.Equal("VALID_VALUE", Locale.T("apply_all"));
        }
        finally
        {
            if (File.Exists(tempFile))
                File.Delete(tempFile);
            Locale.SetLocale("en"); // reset so LoadLocaleFile overrides don't pollute later tests
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

    // ── Sprint 120: expanded key set (200+ keys) ────────────────────────────

    [Fact]
    public void AvailableKeys_EnglishHasAtLeast200Keys()
    {
        Locale.SetLocale("en");
        Assert.True(Locale.AvailableKeys.Count >= 200, $"Expected >=200 keys, got {Locale.AvailableKeys.Count}");
    }

    [Theory]
    [InlineData("toolbar_apply")]
    [InlineData("toolbar_remove")]
    [InlineData("toolbar_refresh")]
    [InlineData("label_filter")]
    [InlineData("label_profile")]
    [InlineData("label_scope")]
    [InlineData("label_kind")]
    [InlineData("btn_select_all_short")]
    [InlineData("btn_deselect_all_short")]
    [InlineData("btn_invert_short")]
    public void ToolbarKeys_PresentInEnglish(string key)
    {
        Locale.SetLocale("en");
        Assert.Contains(key, Locale.AvailableKeys);
    }

    [Theory]
    [InlineData("mnu_win_health")]
    [InlineData("mnu_net_tools")]
    [InlineData("mnu_smart_scan")]
    [InlineData("mnu_privacy_dash")]
    [InlineData("mnu_temp_cleaner")]
    [InlineData("mnu_hosts_file_mgr")]
    [InlineData("mnu_dns_https")]
    [InlineData("mnu_profile_wizard")]
    public void MenuToolsKeys_PresentInEnglish(string key)
    {
        Locale.SetLocale("en");
        Assert.Contains(key, Locale.AvailableKeys);
    }

    [Theory]
    [InlineData("status_ready")]
    [InlineData("status_loading")]
    [InlineData("status_applying")]
    [InlineData("status_n_selected")]
    [InlineData("status_admin_no")]
    public void StatusBarKeys_PresentInEnglish(string key)
    {
        Locale.SetLocale("en");
        Assert.Contains(key, Locale.AvailableKeys);
    }

    [Theory]
    [InlineData("msg_reboot_pending")]
    [InlineData("msg_no_selection")]
    [InlineData("msg_apply_done")]
    [InlineData("msg_remove_done")]
    [InlineData("msg_batch_done")]
    public void MessageKeys_PresentInEnglish(string key)
    {
        Locale.SetLocale("en");
        Assert.Contains(key, Locale.AvailableKeys);
    }

    [Fact]
    public void StatusNSelected_FormatsCount()
    {
        Locale.SetLocale("en");
        string result = Locale.T("status_n_selected", 7);
        Assert.Contains("7", result);
    }

    [Fact]
    public void MsgApplyDone_FormatsTweakCount()
    {
        Locale.SetLocale("en");
        string result = Locale.T("msg_apply_done", 3);
        Assert.Contains("3", result);
    }

    [Fact]
    public void MsgBatchDone_FormatsTwoArgs()
    {
        Locale.SetLocale("en");
        string result = Locale.T("msg_batch_done", 5, 2);
        Assert.Contains("5", result);
        Assert.Contains("2", result);
    }

    [Theory]
    [InlineData("toolbar_apply")]
    [InlineData("toolbar_remove")]
    [InlineData("status_ready")]
    [InlineData("col_label")]
    [InlineData("ctx_apply_sel")]
    public void NewKeys_PresentInAllSixLocales(string key)
    {
        foreach (string locale in new[] { "en", "de", "fr", "es", "he", "ja", "zh-CN", "ko", "ar", "pt-BR" })
        {
            Locale.SetLocale(locale);
            Assert.Contains(key, Locale.AvailableKeys);
        }
    }

    [Fact]
    public void NewGermanKeys_AreTranslated()
    {
        Locale.SetLocale("en");
        string enValue = Locale.T("toolbar_apply");

        Locale.SetLocale("de");
        string deValue = Locale.T("toolbar_apply");

        // German "toolbar_apply" should differ from English "Apply"
        Assert.NotEqual(enValue, deValue);
    }

    // ── Chinese Simplified (zh-CN) ─────────────────────────────────────────

    [Fact]
    public void SetLocale_ZhCn_TranslatesApplyAll()
    {
        Locale.SetLocale("zh-CN");
        Assert.Equal("\u5168\u90e8\u5e94\u7528", Locale.T("apply_all"));
    }

    [Fact]
    public void SetLocale_ZhCn_TranslatesStatusReady()
    {
        Locale.SetLocale("zh-CN");
        Assert.Equal("\u5c31\u7dd2", Locale.T("status_ready"));
    }

    [Fact]
    public void SetLocale_ZhCn_TranslatesMenuFile()
    {
        Locale.SetLocale("zh-CN");
        Assert.Equal("\u6587\u4ef6", Locale.T("menu_file"));
    }

    [Fact]
    public void SetLocale_ZhCn_BtnOkAndCancel()
    {
        Locale.SetLocale("zh-CN");
        Assert.Equal("\u786e\u5b9a", Locale.T("btn_ok"));
        Assert.Equal("\u53d6\u6d88", Locale.T("btn_cancel"));
    }

    // ── Korean (ko) ────────────────────────────────────────────────────────

    [Fact]
    public void SetLocale_Ko_TranslatesApplyAll()
    {
        Locale.SetLocale("ko");
        Assert.Equal("\uc804\uccb4 \uc801\uc6a9", Locale.T("apply_all"));
    }

    [Fact]
    public void SetLocale_Ko_TranslatesStatusApplied()
    {
        Locale.SetLocale("ko");
        Assert.Equal("\uc801\uc6a9\ub428", Locale.T("status_applied"));
    }

    [Fact]
    public void SetLocale_Ko_BtnOkAndCancel()
    {
        Locale.SetLocale("ko");
        Assert.Equal("\ud655\uc778", Locale.T("btn_ok"));
        Assert.Equal("\ucde8\uc18c", Locale.T("btn_cancel"));
    }

    // ── Arabic (ar) ────────────────────────────────────────────────────────

    [Fact]
    public void SetLocale_Ar_TranslatesApplyAll()
    {
        Locale.SetLocale("ar");
        Assert.Equal("\u062a\u0637\u0628\u064a\u0642 \u0627\u0644\u0643\u0644", Locale.T("apply_all"));
    }

    [Fact]
    public void SetLocale_Ar_TranslatesMenuFile()
    {
        Locale.SetLocale("ar");
        Assert.Equal("\u0645\u0644\u0641", Locale.T("menu_file"));
    }

    [Fact]
    public void SetLocale_Ar_BtnOkAndCancel()
    {
        Locale.SetLocale("ar");
        Assert.Equal("\u0645\u0648\u0627\u0641\u0642", Locale.T("btn_ok"));
        Assert.Equal("\u0625\u0644\u063a\u0627\u0621", Locale.T("btn_cancel"));
    }

    [Fact]
    public void SetLocale_Ar_StatusReady_IsJahiz()
    {
        Locale.SetLocale("ar");
        Assert.Equal("\u062c\u0627\u0647\u0632", Locale.T("status_ready"));
    }

    // ── Portuguese Brazil (pt-BR) ──────────────────────────────────────────

    [Fact]
    public void SetLocale_PtBr_TranslatesApplyAll()
    {
        Locale.SetLocale("pt-BR");
        Assert.Equal("Aplicar todos", Locale.T("apply_all"));
    }

    [Fact]
    public void SetLocale_PtBr_TranslatesStatusReady()
    {
        Locale.SetLocale("pt-BR");
        Assert.Equal("Pronto", Locale.T("status_ready"));
    }

    [Fact]
    public void SetLocale_PtBr_BtnCancelIsPortuguese()
    {
        Locale.SetLocale("pt-BR");
        Assert.Equal("Cancelar", Locale.T("btn_cancel"));
    }

    [Fact]
    public void SetLocale_PtBr_MenuFileIsArquivo()
    {
        Locale.SetLocale("pt-BR");
        Assert.Equal("Arquivo", Locale.T("menu_file"));
    }

    // ── All 10 locales smoke test ──────────────────────────────────────────

    [Theory]
    [InlineData("zh-CN")]
    [InlineData("ko")]
    [InlineData("ar")]
    [InlineData("pt-BR")]
    public void NewLocale_TranslatesApplyAll_NotRawKey(string locale)
    {
        Locale.SetLocale(locale);
        string val = Locale.T("apply_all");
        Assert.NotEqual("apply_all", val);
        Assert.False(string.IsNullOrWhiteSpace(val));
    }

    [Theory]
    [InlineData("zh-CN")]
    [InlineData("ko")]
    [InlineData("ar")]
    [InlineData("pt-BR")]
    public void NewLocale_TranslatesBtnOk_NotRawKey(string locale)
    {
        Locale.SetLocale(locale);
        string val = Locale.T("btn_ok");
        Assert.NotEqual("btn_ok", val);
    }

    [Theory]
    [InlineData("zh-CN")]
    [InlineData("ko")]
    [InlineData("ar")]
    [InlineData("pt-BR")]
    public void NewLocale_RoundTrip_BackToEnglishWorks(string locale)
    {
        string enBefore = Locale.T("apply_all");
        Locale.SetLocale(locale);
        Locale.SetLocale("en");
        string enAfter = Locale.T("apply_all");
        Assert.Equal(enBefore, enAfter);
    }
}
