"""Tests for regilattice.gui_theme — theme data, switching, and API."""

from __future__ import annotations

import pytest

from regilattice import gui_theme as theme


class TestAvailableThemes:
    def test_returns_list(self) -> None:
        result = theme.available_themes()
        assert isinstance(result, list)
        assert len(result) >= 4

    def test_known_themes_present(self) -> None:
        names = theme.available_themes()
        for expected in ("Catppuccin Mocha", "Catppuccin Latte", "Nord", "Dracula"):
            assert expected in names


class TestCurrentTheme:
    def test_returns_string(self) -> None:
        assert isinstance(theme.current_theme(), str)


class TestSetTheme:
    def test_switch_and_back(self) -> None:
        original = theme.current_theme()
        try:
            theme.set_theme("Nord")
            assert theme.current_theme() == "Nord"
            assert theme.ACCENT == "#88C0D0"
            assert theme.BG == "#2E3440"
        finally:
            theme.set_theme(original)

    def test_all_themes_switchable(self) -> None:
        original = theme.current_theme()
        try:
            for name in theme.available_themes():
                theme.set_theme(name)
                assert theme.current_theme() == name
                # All colour attributes must be non-empty strings
                for attr in ("ACCENT", "BG", "BG_SURFACE", "FG", "FG_DIM", "CARD_BG",
                             "CARD_HOVER", "OK_GREEN", "WARN_YELLOW", "ERR_RED", "PURPLE",
                             "HEADER_BG", "DIM_BG", "TEAL", "GPO_ORANGE"):
                    val = getattr(theme, attr)
                    assert isinstance(val, str) and val, f"{attr} empty after set_theme({name!r})"
        finally:
            theme.set_theme(original)

    def test_unknown_theme_raises(self) -> None:
        with pytest.raises(ValueError, match="Unknown theme"):
            theme.set_theme("NonExistent_Theme_12345")

    def test_status_aliases_updated(self) -> None:
        original = theme.current_theme()
        try:
            theme.set_theme("Dracula")
            assert theme.STATUS_APPLIED == theme.OK_GREEN
            assert theme.STATUS_NOT_APPLIED == theme.FG_DIM
            assert theme.STATUS_UNKNOWN == theme.WARN_YELLOW
            assert theme.STATUS_CORP_BLOCKED == theme.ERR_RED
        finally:
            theme.set_theme(original)


class TestThemeDataIntegrity:
    """Verify all theme dicts have the same keys and valid hex colours."""

    def test_all_themes_have_same_keys(self) -> None:
        names = theme.available_themes()
        reference_keys = set(theme._THEMES[names[0]].keys())
        for name in names[1:]:
            assert set(theme._THEMES[name].keys()) == reference_keys, f"{name} has different keys"

    def test_all_values_are_hex_colours(self) -> None:
        import re
        hex_re = re.compile(r"^#[0-9A-Fa-f]{6}$")
        for name in theme.available_themes():
            for key, value in theme._THEMES[name].items():
                assert hex_re.match(value), f"{name}.{key} = {value!r} is not a valid hex colour"


class TestFonts:
    def test_font_tuples(self) -> None:
        for attr in ("FONT", "FONT_BOLD", "FONT_SM", "FONT_XS", "FONT_XS_BOLD", "FONT_TITLE", "FONT_CAT"):
            val = getattr(theme, attr)
            assert isinstance(val, tuple)
            assert len(val) >= 2
            assert isinstance(val[0], str)
            assert isinstance(val[1], int)
