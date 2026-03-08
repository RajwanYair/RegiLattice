"""Tests for regilattice.locale — i18n string table."""

from __future__ import annotations

import json
from collections.abc import Generator
from pathlib import Path

import pytest

from regilattice.locale import available_keys, current_locale, load_locale_file, set_locale, t


@pytest.fixture(autouse=True)
def _reset_locale() -> Generator[None]:
    """Reset locale to English before each test."""
    set_locale("en")
    yield
    set_locale("en")


class TestTranslate:
    def test_known_key(self) -> None:
        assert t("apply_all") == "Apply All"

    def test_unknown_key_returns_key(self) -> None:
        assert t("zzz_nonexistent") == "zzz_nonexistent"

    def test_format_kwargs(self) -> None:
        assert t("tweaks_count", n=5) == "5 tweaks"

    def test_confirm_apply_format(self) -> None:
        assert t("confirm_apply", n=3) == "Apply 3 tweak(s)?"


class TestSetLocale:
    def test_override_string(self) -> None:
        set_locale("de", {"apply_all": "Alle anwenden"})
        assert t("apply_all") == "Alle anwenden"
        assert current_locale() == "de"

    def test_fallback_to_english(self) -> None:
        set_locale("fr", {"apply_all": "Appliquer tout"})
        # Keys not overridden fall back to English
        assert t("remove_all") == "Remove All"

    def test_reset_to_english(self) -> None:
        set_locale("de", {"apply_all": "Alle anwenden"})
        set_locale("en")
        assert t("apply_all") == "Apply All"


class TestLoadLocaleFile:
    def test_load_json(self, tmp_path: Path) -> None:
        locale_data = {"locale": "es", "strings": {"apply_all": "Aplicar todo", "tweaks_count": "{n} ajustes"}}
        f = tmp_path / "es.json"
        f.write_text(json.dumps(locale_data), encoding="utf-8")
        load_locale_file(f)
        assert current_locale() == "es"
        assert t("apply_all") == "Aplicar todo"
        assert t("tweaks_count", n=10) == "10 ajustes"

    def test_load_empty_strings(self, tmp_path: Path) -> None:
        f = tmp_path / "empty.json"
        f.write_text(json.dumps({"locale": "xx", "strings": {}}), encoding="utf-8")
        load_locale_file(f)
        assert current_locale() == "xx"
        assert t("apply_all") == "Apply All"  # falls back


class TestAvailableKeys:
    def test_returns_sorted_list(self) -> None:
        keys = available_keys()
        assert isinstance(keys, list)
        assert keys == sorted(keys)
        assert "apply_all" in keys
        assert "tweaks_count" in keys

    def test_all_keys_have_english_value(self) -> None:
        for key in available_keys():
            val = t(key)
            assert val != key, f"Key {key!r} has no English translation"
