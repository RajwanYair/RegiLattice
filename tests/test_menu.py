"""Tests for regilattice.menu — Menu construction and display."""

from __future__ import annotations

from unittest.mock import MagicMock, patch

import pytest

from regilattice.menu import Menu


class TestMenuConstruction:
    def test_categories_populated(self) -> None:
        menu = Menu()
        assert len(menu._categories) > 0

    def test_by_cat_matches_categories(self) -> None:
        menu = Menu()
        for cat in menu._categories:
            assert cat in menu._by_cat
            assert len(menu._by_cat[cat]) > 0

    def test_tweaks_are_tweakdefs(self) -> None:
        from regilattice.tweaks import TweakDef

        menu = Menu()
        for cat_tweaks in menu._by_cat.values():
            for td in cat_tweaks:
                assert isinstance(td, TweakDef)

    def test_each_tweak_has_callable_apply(self) -> None:
        menu = Menu()
        for cat_tweaks in menu._by_cat.values():
            for td in cat_tweaks:
                assert callable(td.apply_fn), f"Tweak '{td.label}' apply_fn is not callable"

    def test_each_tweak_has_callable_remove(self) -> None:
        menu = Menu()
        for cat_tweaks in menu._by_cat.values():
            for td in cat_tweaks:
                assert callable(td.remove_fn), f"Tweak '{td.label}' remove_fn is not callable"


class TestMenuHeader:
    def test_header_prints(self, capsys: pytest.CaptureFixture[str]) -> None:
        menu = Menu()
        with patch.object(menu, "_clear"):
            menu._header("Test Subtitle")
        out = capsys.readouterr().out
        assert "RegiLattice" in out
        assert "Test Subtitle" in out

    def test_header_no_subtitle(self, capsys: pytest.CaptureFixture[str]) -> None:
        menu = Menu()
        with patch.object(menu, "_clear"):
            menu._header()
        out = capsys.readouterr().out
        assert "RegiLattice" in out


class TestShowCategories:
    def test_show_categories_exit(self, capsys: pytest.CaptureFixture[str]) -> None:
        menu = Menu()
        with patch.object(menu, "_clear"), patch("builtins.input", return_value="0"):
            choice = menu._show_categories()
        assert choice == "0"

    def test_show_categories_eof(self, capsys: pytest.CaptureFixture[str]) -> None:
        menu = Menu()
        with patch.object(menu, "_clear"), patch("builtins.input", side_effect=EOFError):
            choice = menu._show_categories()
        assert choice == "0"

    def test_show_categories_lists_all(self, capsys: pytest.CaptureFixture[str]) -> None:
        menu = Menu()
        with patch.object(menu, "_clear"), patch("builtins.input", return_value="0"):
            menu._show_categories()
        out = capsys.readouterr().out
        # Should contain at least some category names
        assert "Apply All" in out
        assert "Remove All" in out
        assert "GUI" in out


class TestShowTweaks:
    def test_show_tweaks_back(self, capsys: pytest.CaptureFixture[str]) -> None:
        menu = Menu()
        cat = menu._categories[0]
        with (
            patch.object(menu, "_clear"),
            patch("regilattice.menu.tweak_status", return_value=MagicMock(value="unknown")),
            patch("builtins.input", return_value="0"),
        ):
            menu._show_tweaks(cat)

    def test_show_tweaks_empty_category(self, capsys: pytest.CaptureFixture[str]) -> None:
        menu = Menu()
        with patch.object(menu, "_clear"):
            menu._show_tweaks("NonexistentCategory12345")
        assert "No tweaks" in capsys.readouterr().out

    def test_show_tweaks_toggle(self, capsys: pytest.CaptureFixture[str]) -> None:
        menu = Menu()
        cat = menu._categories[0]
        # First call returns "1" (toggle first tweak), second returns "0" (back)
        with (
            patch.object(menu, "_clear"),
            patch.object(menu, "_pause"),
            patch.object(menu, "_run_single"),
            patch("regilattice.menu.tweak_status", return_value=MagicMock(value="unknown")),
            patch("builtins.input", side_effect=["1", "0"]),
        ):
            menu._show_tweaks(cat)

    def test_show_tweaks_apply_all(self, capsys: pytest.CaptureFixture[str]) -> None:
        menu = Menu()
        cat = menu._categories[0]
        with (
            patch.object(menu, "_clear"),
            patch.object(menu, "_pause"),
            patch.object(menu, "_run_single"),
            patch("regilattice.menu.assert_not_corporate"),
            patch("regilattice.menu.tweak_status", return_value=MagicMock(value="unknown")),
            patch("builtins.input", side_effect=["A", "0"]),
        ):
            menu._show_tweaks(cat)

    def test_show_tweaks_remove_all(self, capsys: pytest.CaptureFixture[str]) -> None:
        menu = Menu()
        cat = menu._categories[0]
        with (
            patch.object(menu, "_clear"),
            patch.object(menu, "_pause"),
            patch.object(menu, "_run_single"),
            patch("regilattice.menu.assert_not_corporate"),
            patch("regilattice.menu.tweak_status", return_value=MagicMock(value="unknown")),
            patch("builtins.input", side_effect=["R", "0"]),
        ):
            menu._show_tweaks(cat)

    def test_show_tweaks_invalid_choice(self, capsys: pytest.CaptureFixture[str]) -> None:
        menu = Menu()
        cat = menu._categories[0]
        with (
            patch.object(menu, "_clear"),
            patch.object(menu, "_pause"),
            patch("regilattice.menu.tweak_status", return_value=MagicMock(value="unknown")),
            patch("builtins.input", side_effect=["xyz", "0"]),
        ):
            menu._show_tweaks(cat)

    def test_show_tweaks_out_of_range(self, capsys: pytest.CaptureFixture[str]) -> None:
        menu = Menu()
        cat = menu._categories[0]
        with (
            patch.object(menu, "_clear"),
            patch.object(menu, "_pause"),
            patch("regilattice.menu.tweak_status", return_value=MagicMock(value="unknown")),
            patch("builtins.input", side_effect=["999", "0"]),
        ):
            menu._show_tweaks(cat)

    def test_show_tweaks_corp_blocked_apply_all(self, capsys: pytest.CaptureFixture[str]) -> None:
        from regilattice.corpguard import CorporateNetworkError

        menu = Menu()
        cat = menu._categories[0]
        with (
            patch.object(menu, "_clear"),
            patch.object(menu, "_pause"),
            patch("regilattice.menu.assert_not_corporate", side_effect=CorporateNetworkError("blocked")),
            patch("regilattice.menu.tweak_status", return_value=MagicMock(value="unknown")),
            patch("builtins.input", side_effect=["A", "0"]),
        ):
            menu._show_tweaks(cat)


class TestRunSingle:
    def test_run_single_apply(self, capsys: pytest.CaptureFixture[str]) -> None:
        menu = Menu()
        td = MagicMock()
        td.label = "Test Tweak"
        td.apply_fn = MagicMock()
        with patch("regilattice.menu.assert_not_corporate"):
            menu._run_single(td, "apply")
        td.apply_fn.assert_called_once()

    def test_run_single_remove(self, capsys: pytest.CaptureFixture[str]) -> None:
        menu = Menu()
        td = MagicMock()
        td.label = "Test Tweak"
        td.remove_fn = MagicMock()
        with patch("regilattice.menu.assert_not_corporate"):
            menu._run_single(td, "remove")
        td.remove_fn.assert_called_once()

    def test_run_single_admin_error(self, capsys: pytest.CaptureFixture[str]) -> None:
        from regilattice.registry import AdminRequirementError

        menu = Menu()
        td = MagicMock()
        td.label = "Test Tweak"
        td.apply_fn.side_effect = AdminRequirementError("need admin")
        with patch("regilattice.menu.assert_not_corporate"):
            menu._run_single(td, "apply")

    def test_run_single_general_error(self, capsys: pytest.CaptureFixture[str]) -> None:
        menu = Menu()
        td = MagicMock()
        td.label = "Test Tweak"
        td.apply_fn.side_effect = RuntimeError("boom")
        with patch("regilattice.menu.assert_not_corporate"):
            menu._run_single(td, "apply")

    def test_run_single_corp_blocked(self, capsys: pytest.CaptureFixture[str]) -> None:
        from regilattice.corpguard import CorporateNetworkError

        menu = Menu()
        td = MagicMock()
        td.label = "Test Tweak"
        with patch("regilattice.menu.assert_not_corporate", side_effect=CorporateNetworkError("blocked")):
            menu._run_single(td, "apply")
        td.apply_fn.assert_not_called()


class TestMenuLoop:
    def test_loop_exit(self) -> None:
        menu = Menu()
        with (
            patch("regilattice.menu.is_windows", return_value=True),
            patch.object(menu, "_show_categories", return_value="0"),
        ):
            menu.loop()

    def test_loop_non_windows(self, capsys: pytest.CaptureFixture[str]) -> None:
        menu = Menu()
        with patch("regilattice.menu.is_windows", return_value=False):
            menu.loop()

    def test_loop_keyboard_interrupt(self) -> None:
        menu = Menu()
        with (
            patch("regilattice.menu.is_windows", return_value=True),
            patch.object(menu, "_show_categories", side_effect=KeyboardInterrupt),
        ):
            menu.loop()

    def test_loop_apply_all(self) -> None:
        menu = Menu()
        with (
            patch("regilattice.menu.is_windows", return_value=True),
            patch.object(menu, "_show_categories", side_effect=["A", "0"]),
            patch.object(menu, "_pause"),
            patch("regilattice.menu.assert_not_corporate"),
            patch("regilattice.menu.apply_all"),
        ):
            menu.loop()

    def test_loop_remove_all(self) -> None:
        menu = Menu()
        with (
            patch("regilattice.menu.is_windows", return_value=True),
            patch.object(menu, "_show_categories", side_effect=["R", "0"]),
            patch.object(menu, "_pause"),
            patch("regilattice.menu.assert_not_corporate"),
            patch("regilattice.menu.remove_all"),
        ):
            menu.loop()

    def test_loop_gui_launch(self) -> None:
        menu = Menu()
        with (
            patch("regilattice.menu.is_windows", return_value=True),
            patch.object(menu, "_show_categories", side_effect=["G", "0"]),
            patch("regilattice.gui.launch"),
        ):
            menu.loop()

    def test_loop_invalid_choice(self) -> None:
        menu = Menu()
        with (
            patch("regilattice.menu.is_windows", return_value=True),
            patch.object(menu, "_show_categories", side_effect=["xyz", "0"]),
            patch.object(menu, "_pause"),
        ):
            menu.loop()

    def test_loop_category_selection(self) -> None:
        menu = Menu()
        with (
            patch("regilattice.menu.is_windows", return_value=True),
            patch.object(menu, "_show_categories", side_effect=["1", "0"]),
            patch.object(menu, "_show_tweaks"),
        ):
            menu.loop()

    def test_loop_out_of_range(self) -> None:
        menu = Menu()
        with (
            patch("regilattice.menu.is_windows", return_value=True),
            patch.object(menu, "_show_categories", side_effect=["999", "0"]),
            patch.object(menu, "_pause"),
        ):
            menu.loop()

    def test_loop_corp_blocked_apply_all(self) -> None:
        from regilattice.corpguard import CorporateNetworkError

        menu = Menu()
        with (
            patch("regilattice.menu.is_windows", return_value=True),
            patch.object(menu, "_show_categories", side_effect=["A", "0"]),
            patch.object(menu, "_pause"),
            patch("regilattice.menu.assert_not_corporate", side_effect=CorporateNetworkError("blocked")),
        ):
            menu.loop()


class TestMenuMain:
    def test_main_function(self) -> None:
        from regilattice.menu import main as menu_main

        with (
            patch("regilattice.menu.is_windows", return_value=True),
            patch.object(Menu, "_show_categories", return_value="0"),
        ):
            rc = menu_main()
        assert rc == 0
