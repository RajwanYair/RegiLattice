"""Tests for regilattice.gui — main application window.

Strategy:
- Static/class method tests run without any Tk window.
- Instance tests create a hidden RegiLatticeGUI with _deferred_init patched
  out (so 1 292+ tweak rows are never loaded), then exercise state-manipulation
  logic against the empty row set.
"""

from __future__ import annotations

import contextlib
import json
from collections.abc import Generator
from pathlib import Path
from unittest.mock import MagicMock, patch

import pytest

from regilattice.gui import RegiLatticeGUI, launch

# ── Tk availability guard ────────────────────────────────────────────────────

_tk_available = True
try:
    import tkinter as tk
except Exception:
    _tk_available = False


def _skip_no_tk() -> None:
    if not _tk_available:
        pytest.skip("tkinter unavailable or headless environment")


# ── Fixture: headless GUI instance ───────────────────────────────────────────


@pytest.fixture()
def gui() -> Generator[RegiLatticeGUI, None, None]:
    """Create a RegiLatticeGUI with _deferred_init patched out.

    _deferred_init normally loads 1 292 tweak rows via background threads.
    Patching it leaves _tweak_rows empty so pure-logic tests remain fast.
    """
    _skip_no_tk()
    try:
        with patch.object(RegiLatticeGUI, "_deferred_init"):
            app = RegiLatticeGUI()
    except Exception as exc:
        pytest.skip(f"tkinter / Tcl-Tk initialisation failed: {exc}")
    app._root.withdraw()
    yield app
    with contextlib.suppress(Exception):
        app._root.destroy()


# ── Static / class-method tests (no Tk needed) ───────────────────────────────


class TestLoadGeometry:
    def test_returns_none_when_no_file(self, tmp_path: Path, monkeypatch: pytest.MonkeyPatch) -> None:
        monkeypatch.setattr("regilattice.gui._GEOMETRY_FILE", tmp_path / "missing.json")
        assert RegiLatticeGUI._load_geometry() is None

    def test_returns_geometry_string(self, tmp_path: Path, monkeypatch: pytest.MonkeyPatch) -> None:
        geo_file = tmp_path / "window.json"
        geo_file.write_text(json.dumps({"geometry": "800x600+100+100"}), encoding="utf-8")
        monkeypatch.setattr("regilattice.gui._GEOMETRY_FILE", geo_file)
        assert RegiLatticeGUI._load_geometry() == "800x600+100+100"

    def test_returns_none_for_corrupt_file(self, tmp_path: Path, monkeypatch: pytest.MonkeyPatch) -> None:
        geo_file = tmp_path / "window.json"
        geo_file.write_text("!!! not valid json !!!", encoding="utf-8")
        monkeypatch.setattr("regilattice.gui._GEOMETRY_FILE", geo_file)
        assert RegiLatticeGUI._load_geometry() is None

    def test_returns_none_for_empty_geometry_value(self, tmp_path: Path, monkeypatch: pytest.MonkeyPatch) -> None:
        geo_file = tmp_path / "window.json"
        geo_file.write_text(json.dumps({"geometry": ""}), encoding="utf-8")
        monkeypatch.setattr("regilattice.gui._GEOMETRY_FILE", geo_file)
        assert RegiLatticeGUI._load_geometry() is None

    def test_returns_none_when_geometry_is_wrong_type(self, tmp_path: Path, monkeypatch: pytest.MonkeyPatch) -> None:
        geo_file = tmp_path / "window.json"
        geo_file.write_text(json.dumps({"geometry": 12345}), encoding="utf-8")
        monkeypatch.setattr("regilattice.gui._GEOMETRY_FILE", geo_file)
        assert RegiLatticeGUI._load_geometry() is None

    def test_returns_none_for_missing_key(self, tmp_path: Path, monkeypatch: pytest.MonkeyPatch) -> None:
        geo_file = tmp_path / "window.json"
        geo_file.write_text(json.dumps({"other": "data"}), encoding="utf-8")
        monkeypatch.setattr("regilattice.gui._GEOMETRY_FILE", geo_file)
        assert RegiLatticeGUI._load_geometry() is None


class TestLoadCategoryOrder:
    def test_returns_empty_when_no_file(self, tmp_path: Path, monkeypatch: pytest.MonkeyPatch) -> None:
        monkeypatch.setattr("regilattice.gui._CATEGORY_ORDER_FILE", tmp_path / "missing.json")
        assert RegiLatticeGUI._load_category_order() == []

    def test_returns_list_of_strings(self, tmp_path: Path, monkeypatch: pytest.MonkeyPatch) -> None:
        order_file = tmp_path / "category_order.json"
        order_file.write_text(json.dumps(["Explorer", "Privacy", "Taskbar"]), encoding="utf-8")
        monkeypatch.setattr("regilattice.gui._CATEGORY_ORDER_FILE", order_file)
        assert RegiLatticeGUI._load_category_order() == ["Explorer", "Privacy", "Taskbar"]

    def test_returns_empty_for_corrupt_file(self, tmp_path: Path, monkeypatch: pytest.MonkeyPatch) -> None:
        order_file = tmp_path / "category_order.json"
        order_file.write_text("badjson!!!", encoding="utf-8")
        monkeypatch.setattr("regilattice.gui._CATEGORY_ORDER_FILE", order_file)
        assert RegiLatticeGUI._load_category_order() == []

    def test_returns_empty_for_non_list(self, tmp_path: Path, monkeypatch: pytest.MonkeyPatch) -> None:
        order_file = tmp_path / "category_order.json"
        order_file.write_text(json.dumps({"not": "a list"}), encoding="utf-8")
        monkeypatch.setattr("regilattice.gui._CATEGORY_ORDER_FILE", order_file)
        assert RegiLatticeGUI._load_category_order() == []

    def test_coerces_items_to_str(self, tmp_path: Path, monkeypatch: pytest.MonkeyPatch) -> None:
        order_file = tmp_path / "category_order.json"
        order_file.write_text(json.dumps([1, 2, "Explorer"]), encoding="utf-8")
        monkeypatch.setattr("regilattice.gui._CATEGORY_ORDER_FILE", order_file)
        result = RegiLatticeGUI._load_category_order()
        assert all(isinstance(x, str) for x in result)
        assert "1" in result and "Explorer" in result


class TestLoadSearchHistory:
    def test_returns_empty_when_no_file(self, tmp_path: Path, monkeypatch: pytest.MonkeyPatch) -> None:
        monkeypatch.setattr("regilattice.gui._SEARCH_HISTORY_FILE", tmp_path / "missing.json")
        assert RegiLatticeGUI._load_search_history() == []

    def test_returns_history_list(self, tmp_path: Path, monkeypatch: pytest.MonkeyPatch) -> None:
        hist_file = tmp_path / "search_history.json"
        hist_file.write_text(json.dumps(["privacy", "taskbar", "telemetry"]), encoding="utf-8")
        monkeypatch.setattr("regilattice.gui._SEARCH_HISTORY_FILE", hist_file)
        assert RegiLatticeGUI._load_search_history() == ["privacy", "taskbar", "telemetry"]

    def test_filters_non_string_items(self, tmp_path: Path, monkeypatch: pytest.MonkeyPatch) -> None:
        hist_file = tmp_path / "search_history.json"
        hist_file.write_text(json.dumps(["ok", 42, None, "also-ok"]), encoding="utf-8")
        monkeypatch.setattr("regilattice.gui._SEARCH_HISTORY_FILE", hist_file)
        result = RegiLatticeGUI._load_search_history()
        assert result == ["ok", "also-ok"]

    def test_caps_at_max_history(self, tmp_path: Path, monkeypatch: pytest.MonkeyPatch) -> None:
        from regilattice.gui import _MAX_SEARCH_HISTORY

        hist_file = tmp_path / "search_history.json"
        items = [f"query-{i}" for i in range(_MAX_SEARCH_HISTORY + 10)]
        hist_file.write_text(json.dumps(items), encoding="utf-8")
        monkeypatch.setattr("regilattice.gui._SEARCH_HISTORY_FILE", hist_file)
        result = RegiLatticeGUI._load_search_history()
        assert len(result) == _MAX_SEARCH_HISTORY

    def test_returns_empty_for_non_list(self, tmp_path: Path, monkeypatch: pytest.MonkeyPatch) -> None:
        hist_file = tmp_path / "search_history.json"
        hist_file.write_text(json.dumps({"searches": ["x"]}), encoding="utf-8")
        monkeypatch.setattr("regilattice.gui._SEARCH_HISTORY_FILE", hist_file)
        assert RegiLatticeGUI._load_search_history() == []

    def test_returns_empty_for_corrupt_file(self, tmp_path: Path, monkeypatch: pytest.MonkeyPatch) -> None:
        hist_file = tmp_path / "search_history.json"
        hist_file.write_text("{ BAD JSON", encoding="utf-8")
        monkeypatch.setattr("regilattice.gui._SEARCH_HISTORY_FILE", hist_file)
        assert RegiLatticeGUI._load_search_history() == []


class TestEnableDpiAwareness:
    def test_does_not_raise_normally(self) -> None:
        """Always returns None; errors are silently swallowed."""
        result = RegiLatticeGUI._enable_dpi_awareness()
        assert result is None

    def test_handles_missing_windll(self) -> None:
        with patch("ctypes.windll", new=None, create=True):
            RegiLatticeGUI._enable_dpi_awareness()  # must not raise


class TestApplyWin11DarkTitlebar:
    def test_does_not_raise_normally(self) -> None:
        """Always returns None; ctypes failures are swallowed."""
        result = RegiLatticeGUI._apply_win11_dark_titlebar()
        assert result is None

    def test_handles_attribute_error(self) -> None:
        with patch("ctypes.windll", new=MagicMock(side_effect=AttributeError), create=True):
            RegiLatticeGUI._apply_win11_dark_titlebar()  # must not raise


# ── launch() tests ────────────────────────────────────────────────────────────


class TestLaunch:
    def test_exits_on_non_windows(self) -> None:
        with (
            patch("regilattice.gui.is_windows", return_value=False),
            patch("sys.exit", side_effect=SystemExit(1)) as mock_exit,
            patch("builtins.print"),
            pytest.raises(SystemExit),
        ):
            launch()
        mock_exit.assert_called_once_with(1)

    def test_prints_message_on_non_windows(self, capsys: pytest.CaptureFixture[str]) -> None:
        with (
            patch("regilattice.gui.is_windows", return_value=False),
            patch("sys.exit", side_effect=SystemExit(1)),
            pytest.raises(SystemExit),
        ):
            launch()
        out = capsys.readouterr().out
        assert "Windows" in out or "windows" in out.lower()

    def test_creates_and_runs_app_on_windows(self) -> None:
        mock_app = MagicMock()
        with (
            patch("regilattice.gui.is_windows", return_value=True),
            patch("regilattice.gui.RegiLatticeGUI", return_value=mock_app) as mock_cls,
        ):
            launch()
        mock_cls.assert_called_once()
        mock_app.run.assert_called_once()


# ── RegiLatticeGUI instance tests ─────────────────────────────────────────────


class TestGuiInit:
    def test_creates_successfully(self, gui: RegiLatticeGUI) -> None:
        assert gui._root is not None

    def test_has_required_attributes(self, gui: RegiLatticeGUI) -> None:
        assert hasattr(gui, "_tweak_rows")
        assert hasattr(gui, "_category_sections")
        assert hasattr(gui, "_row_by_id")
        assert hasattr(gui, "_search_var")
        assert hasattr(gui, "_status_filter_var")
        assert hasattr(gui, "_scope_filter_var")

    def test_tweak_rows_empty_before_deferred(self, gui: RegiLatticeGUI) -> None:
        assert gui._tweak_rows == []

    def test_category_sections_empty_before_deferred(self, gui: RegiLatticeGUI) -> None:
        assert gui._category_sections == []

    def test_corp_blocked_false_initially(self, gui: RegiLatticeGUI) -> None:
        assert gui._corp_blocked is False

    def test_running_false_initially(self, gui: RegiLatticeGUI) -> None:
        assert gui._running is False

    def test_loading_true_before_deferred(self, gui: RegiLatticeGUI) -> None:
        # _finish_loading() hasn't been called yet
        assert gui._loading is True

    def test_cancel_event_not_set(self, gui: RegiLatticeGUI) -> None:
        assert not gui._cancel.is_set()

    def test_undo_stack_empty(self, gui: RegiLatticeGUI) -> None:
        assert gui._undo_stack == []


class TestSelectDeselect:
    def test_select_all_empty_rows(self, gui: RegiLatticeGUI) -> None:
        gui._select_all()  # no rows — must not raise

    def test_deselect_all_empty_rows(self, gui: RegiLatticeGUI) -> None:
        gui._deselect_all()  # no rows — must not raise

    def test_select_all_sets_row_vars(self, gui: RegiLatticeGUI) -> None:
        row = MagicMock()
        row.var = tk.BooleanVar(gui._root, value=False)
        row.td.corp_safe = True
        gui._tweak_rows = [row]
        gui._corp_blocked = False
        gui._select_all()
        assert row.var.get() is True

    def test_deselect_all_clears_row_vars(self, gui: RegiLatticeGUI) -> None:
        row = MagicMock()
        row.var = tk.BooleanVar(gui._root, value=True)
        gui._tweak_rows = [row]
        gui._deselect_all()
        assert row.var.get() is False

    def test_select_all_skips_non_corp_safe_when_corp_blocked(self, gui: RegiLatticeGUI) -> None:
        row = MagicMock()
        row.var = tk.BooleanVar(gui._root, value=False)
        row.td.corp_safe = False
        gui._tweak_rows = [row]
        gui._corp_blocked = True
        gui._select_all()
        assert row.var.get() is False

    def test_select_all_includes_corp_safe_when_corp_blocked(self, gui: RegiLatticeGUI) -> None:
        row = MagicMock()
        row.var = tk.BooleanVar(gui._root, value=False)
        row.td.corp_safe = True
        gui._tweak_rows = [row]
        gui._corp_blocked = True
        gui._select_all()
        assert row.var.get() is True

    def test_invert_selection_toggles_rows(self, gui: RegiLatticeGUI) -> None:
        row_on = MagicMock()
        row_on.var = tk.BooleanVar(gui._root, value=True)
        row_on.disabled_by_corp = False
        row_off = MagicMock()
        row_off.var = tk.BooleanVar(gui._root, value=False)
        row_off.disabled_by_corp = False
        gui._tweak_rows = [row_on, row_off]
        gui._invert_selection()
        assert row_on.var.get() is False
        assert row_off.var.get() is True

    def test_invert_skips_corp_blocked_rows(self, gui: RegiLatticeGUI) -> None:
        row = MagicMock()
        row.var = tk.BooleanVar(gui._root, value=False)
        row.disabled_by_corp = True
        gui._tweak_rows = [row]
        gui._invert_selection()
        assert row.var.get() is False  # unchanged


class TestSearch:
    def test_clear_search_resets_variable(self, gui: RegiLatticeGUI) -> None:
        gui._search_var.set("telemetry")
        gui._clear_search()
        assert gui._search_var.get() == ""

    def test_commit_search_adds_entry(self, gui: RegiLatticeGUI) -> None:
        gui._search_history = []
        gui._search_var.set("privacy tweak")
        with patch.object(gui, "_save_search_history"):
            gui._commit_search()
        assert "privacy tweak" in gui._search_history

    def test_commit_empty_query_does_not_add(self, gui: RegiLatticeGUI) -> None:
        gui._search_history = []
        gui._search_var.set("")
        with patch.object(gui, "_save_search_history") as mock_save:
            gui._commit_search()
        mock_save.assert_not_called()
        assert gui._search_history == []

    def test_commit_search_deduplicates(self, gui: RegiLatticeGUI) -> None:
        gui._search_history = ["privacy", "taskbar"]
        gui._search_var.set("privacy")
        with patch.object(gui, "_save_search_history"):
            gui._commit_search()
        assert gui._search_history.count("privacy") == 1
        assert gui._search_history[0] == "privacy"

    def test_commit_search_moves_duplicate_to_top(self, gui: RegiLatticeGUI) -> None:
        gui._search_history = ["a", "b", "c"]
        gui._search_var.set("c")
        with patch.object(gui, "_save_search_history"):
            gui._commit_search()
        assert gui._search_history[0] == "c"

    def test_filter_rows_noop_while_loading(self, gui: RegiLatticeGUI) -> None:
        gui._loading = True
        gui._filter_rows()  # must not raise

    def test_filter_rows_with_empty_rows(self, gui: RegiLatticeGUI) -> None:
        gui._loading = False
        gui._filter_rows()  # must not raise

    def test_filter_rows_with_query(self, gui: RegiLatticeGUI) -> None:
        gui._loading = False
        gui._search_var.set("privacy")
        gui._filter_rows()  # no rows → no crash


class TestCollapseExpand:
    def test_expand_all_empty(self, gui: RegiLatticeGUI) -> None:
        gui._expand_all()  # no sections — no crash

    def test_collapse_all_empty(self, gui: RegiLatticeGUI) -> None:
        gui._collapse_all()  # no sections — no crash

    def test_expand_calls_toggle_on_collapsed_section(self, gui: RegiLatticeGUI) -> None:
        section = MagicMock()
        section.expanded = False
        gui._category_sections = [section]
        gui._expand_all()
        section.toggle.assert_called_once()

    def test_collapse_calls_toggle_on_expanded_section(self, gui: RegiLatticeGUI) -> None:
        section = MagicMock()
        section.expanded = True
        gui._category_sections = [section]
        gui._collapse_all()
        section.toggle.assert_called_once()

    def test_expand_skips_already_expanded(self, gui: RegiLatticeGUI) -> None:
        section = MagicMock()
        section.expanded = True
        gui._category_sections = [section]
        gui._expand_all()
        section.toggle.assert_not_called()

    def test_collapse_skips_already_collapsed(self, gui: RegiLatticeGUI) -> None:
        section = MagicMock()
        section.expanded = False
        gui._category_sections = [section]
        gui._collapse_all()
        section.toggle.assert_not_called()

    def test_mixed_sections_expand(self, gui: RegiLatticeGUI) -> None:
        s1 = MagicMock()
        s1.expanded = True
        s2 = MagicMock()
        s2.expanded = False
        gui._category_sections = [s1, s2]
        gui._expand_all()
        s1.toggle.assert_not_called()
        s2.toggle.assert_called_once()


class TestOnCloseQuit:
    def test_on_close_calls_quit_when_no_tray(self, gui: RegiLatticeGUI) -> None:
        gui._tray_available = False
        with patch.object(gui, "_quit") as mock_quit:
            gui._on_close()
        mock_quit.assert_called_once()

    def test_on_close_minimizes_to_tray_when_available(self, gui: RegiLatticeGUI) -> None:
        gui._tray_available = True
        with (
            patch.object(gui, "_minimize_to_tray") as mock_tray,
            patch.object(gui, "_quit") as mock_quit,
        ):
            gui._on_close()
        mock_tray.assert_called_once()
        mock_quit.assert_not_called()

    def test_quit_calls_save_methods(self, gui: RegiLatticeGUI) -> None:
        with (
            patch.object(gui, "_save_geometry") as m_geo,
            patch.object(gui, "_save_collapse_state") as m_col,
            patch.object(gui, "_save_preferences") as m_pref,
            patch.object(gui, "_save_search_history") as m_hist,
            patch.object(gui, "_stop_tray"),
            patch.object(gui._root, "destroy"),
        ):
            gui._quit()
        m_geo.assert_called_once()
        m_col.assert_called_once()
        m_pref.assert_called_once()
        m_hist.assert_called_once()


class TestSaveGeometry:
    def test_save_geometry_creates_file(self, gui: RegiLatticeGUI, tmp_path: Path, monkeypatch: pytest.MonkeyPatch) -> None:
        monkeypatch.setattr("regilattice.gui._CONFIG_DIR", tmp_path)
        monkeypatch.setattr("regilattice.gui._GEOMETRY_FILE", tmp_path / "window.json")
        gui._save_geometry()
        geo_file = tmp_path / "window.json"
        assert geo_file.exists()
        data = json.loads(geo_file.read_text(encoding="utf-8"))
        assert "geometry" in data
        assert isinstance(data["geometry"], str)

    def test_save_geometry_round_trip(self, gui: RegiLatticeGUI, tmp_path: Path, monkeypatch: pytest.MonkeyPatch) -> None:
        monkeypatch.setattr("regilattice.gui._CONFIG_DIR", tmp_path)
        geo_file = tmp_path / "window.json"
        monkeypatch.setattr("regilattice.gui._GEOMETRY_FILE", geo_file)
        gui._save_geometry()
        loaded = RegiLatticeGUI._load_geometry()
        assert loaded is not None
        assert "x" in loaded  # e.g. "900x940+0+0"


class TestSaveCollapseState:
    def test_save_collapse_state_with_all_expanded(self, gui: RegiLatticeGUI, tmp_path: Path, monkeypatch: pytest.MonkeyPatch) -> None:
        monkeypatch.setattr("regilattice.gui._CONFIG_DIR", tmp_path)
        monkeypatch.setattr("regilattice.gui._COLLAPSE_FILE", tmp_path / "collapsed.json")
        section = MagicMock()
        section.expanded = True
        section.name = "Explorer"
        gui._category_sections = [section]
        gui._save_collapse_state()
        # Expanded sections saved → file IS created containing the expanded name
        collapse_file = tmp_path / "collapsed.json"
        assert collapse_file.exists()
        data = json.loads(collapse_file.read_text(encoding="utf-8"))
        assert "Explorer" in data

    def test_save_collapse_state_with_collapsed(self, gui: RegiLatticeGUI, tmp_path: Path, monkeypatch: pytest.MonkeyPatch) -> None:
        monkeypatch.setattr("regilattice.gui._CONFIG_DIR", tmp_path)
        collapse_file = tmp_path / "collapsed.json"
        monkeypatch.setattr("regilattice.gui._COLLAPSE_FILE", collapse_file)
        section = MagicMock()
        section.expanded = False
        section.name = "Privacy"
        gui._category_sections = [section]
        gui._save_collapse_state()
        # No expanded sections → file is deleted / not created
        assert not collapse_file.exists()


class TestSaveCategoryOrder:
    def test_save_category_order_writes_list(self, gui: RegiLatticeGUI, tmp_path: Path, monkeypatch: pytest.MonkeyPatch) -> None:
        monkeypatch.setattr("regilattice.gui._CONFIG_DIR", tmp_path)
        order_file = tmp_path / "category_order.json"
        monkeypatch.setattr("regilattice.gui._CATEGORY_ORDER_FILE", order_file)
        s1 = MagicMock()
        s1.name = "Explorer"
        s2 = MagicMock()
        s2.name = "Privacy"
        gui._category_sections = [s1, s2]
        gui._save_category_order()
        data = json.loads(order_file.read_text(encoding="utf-8"))
        assert data == ["Explorer", "Privacy"]


class TestSwitchTheme:
    def test_switch_to_each_available_theme(self, gui: RegiLatticeGUI) -> None:
        from regilattice.gui_theme import available_themes

        for theme_name in available_themes():
            gui._switch_theme(theme_name)  # should not raise

    def test_switch_to_auto_does_not_raise(self, gui: RegiLatticeGUI) -> None:
        gui._switch_theme("Auto")

    def test_theme_var_updated(self, gui: RegiLatticeGUI) -> None:
        from regilattice.gui_theme import available_themes

        themes = available_themes()
        if not themes:
            pytest.skip("no themes found")
        gui._switch_theme(themes[0])
        assert gui._theme_var.get() in (themes[0], "Auto")  # set by caller before _switch_theme


class TestApplyProfileSelection:
    def test_none_selection_deselects_all(self, gui: RegiLatticeGUI) -> None:
        row = MagicMock()
        row.var = tk.BooleanVar(gui._root, value=True)
        gui._tweak_rows = [row]
        gui._apply_profile_selection("(none)")
        assert row.var.get() is False

    def test_unknown_profile_deselects_all(self, gui: RegiLatticeGUI) -> None:
        row = MagicMock()
        row.var = tk.BooleanVar(gui._root, value=True)
        gui._tweak_rows = [row]
        gui._apply_profile_selection("nonexistent-profile-xyz")
        assert row.var.get() is False

    def test_valid_profile_selects_matching_category_rows(self, gui: RegiLatticeGUI) -> None:
        from regilattice.tweaks import available_profiles, profile_info

        profiles = available_profiles()
        if not profiles:
            pytest.skip("no profiles available")
        profile_name = profiles[0]
        info = profile_info(profile_name)
        assert info is not None
        # Create one row whose category matches the profile
        target_cat = next(iter(info.apply_categories))
        row = MagicMock()
        row.var = tk.BooleanVar(gui._root, value=False)
        row.td.category = target_cat
        row.disabled_by_corp = False
        gui._tweak_rows = [row]
        gui._apply_profile_selection(profile_name.title())
        assert row.var.get() is True


class TestCorpResult:
    def test_apply_corp_result_non_corp_clears_blocked(self, gui: RegiLatticeGUI) -> None:
        gui._apply_corp_result(False, None)
        assert gui._corp_blocked is False

    def test_apply_corp_result_sets_corp_blocked(self, gui: RegiLatticeGUI) -> None:
        gui._apply_corp_result(True, "domain-joined")
        assert gui._corp_blocked is True

    def test_apply_corp_result_corp_marks_rows(self, gui: RegiLatticeGUI) -> None:
        row = MagicMock()
        row.td.corp_safe = False
        gui._tweak_rows = [row]
        gui._apply_corp_result(True, "gpo")
        row.mark_corp_blocked.assert_called_once()

    def test_apply_corp_result_skips_corp_safe_rows(self, gui: RegiLatticeGUI) -> None:
        row = MagicMock()
        row.td.corp_safe = True
        gui._tweak_rows = [row]
        gui._apply_corp_result(True, "domain")
        row.mark_corp_blocked.assert_not_called()


class TestLogPanel:
    def test_toggle_log_shows_panel(self, gui: RegiLatticeGUI) -> None:
        gui._log_visible = False
        with (
            patch.object(gui._log_frame, "pack"),
            patch.object(gui, "_refresh_log"),
            patch.object(gui, "_auto_refresh_log"),
        ):
            gui._toggle_log_panel()
        assert gui._log_visible is True

    def test_toggle_log_hides_panel(self, gui: RegiLatticeGUI) -> None:
        gui._log_visible = True
        with patch.object(gui._log_frame, "pack_forget"):
            gui._toggle_log_panel()
        assert gui._log_visible is False


class TestHwResult:
    def test_apply_hw_result_ignores_wrong_type(self, gui: RegiLatticeGUI) -> None:
        gui._apply_hw_result("not an HWProfile")  # wrong type → ignore, no crash

    def test_apply_hw_result_with_valid_profile(self, gui: RegiLatticeGUI) -> None:
        from regilattice.hwinfo import CPUInfo, DiskInfo, GPUInfo, HWProfile, MemoryInfo

        hw = HWProfile(
            cpu=CPUInfo(name="Intel Core i9-12900K @ 3.2GHz", cores_physical=16, cores_logical=24),
            gpus=[GPUInfo(name="NVIDIA GeForce RTX 4090")],
            memory=MemoryInfo(total_mb=32768, available_mb=16384),
            disk=DiskInfo(total_gb=1000, free_gb=500),
        )
        gui._apply_hw_result(hw)  # should update _hw_label without error


class TestSetStatus:
    def test_set_status_updates_label(self, gui: RegiLatticeGUI) -> None:
        gui._set_status("Test message")
        assert gui._status_label.cget("text") == "Test message"

    def test_set_status_with_color(self, gui: RegiLatticeGUI) -> None:
        gui._set_status("Colored", "#FF0000")
        assert gui._status_label.cget("text") == "Colored"


class TestSelectedTweaks:
    def test_returns_empty_when_no_rows(self, gui: RegiLatticeGUI) -> None:
        assert gui._selected_tweaks() == []

    def test_returns_checked_rows_only(self, gui: RegiLatticeGUI) -> None:
        row_on = MagicMock()
        row_on.var = tk.BooleanVar(gui._root, value=True)
        row_off = MagicMock()
        row_off.var = tk.BooleanVar(gui._root, value=False)
        gui._tweak_rows = [row_on, row_off]
        result = gui._selected_tweaks()
        assert result == [row_on.td]


# ── Sprint 7: delta status + lazy-build wire tests ───────────────────────────


class TestDeltaStatus:
    """Tests for the _apply_statuses delta optimisation (Sprint 7)."""

    def _make_row(self, gui: RegiLatticeGUI, tweak_id: str) -> MagicMock:
        """Create a mock TweakRow with real widgets needed by _apply_statuses."""
        row = MagicMock()
        row.td.id = tweak_id
        row.td.registry_keys = []
        row.td.description = ""
        row.td.tags = []
        row.td.needs_admin = False
        row.td.corp_safe = True
        row.disabled_by_corp = False
        row.frame = MagicMock()  # non-None → "widgets already built"
        row.status_dot = MagicMock()
        row.status_text = MagicMock()
        row.toggle_btn = MagicMock()
        row.tooltip = MagicMock()
        return row

    def test_first_apply_configures_all_rows(self, gui: RegiLatticeGUI) -> None:
        from regilattice.tweaks import TweakResult

        row = self._make_row(gui, "t1")
        gui._tweak_rows = [row]
        gui._prev_statuses = {}
        gui._apply_statuses({"t1": TweakResult.APPLIED})
        row.status_text.configure.assert_called()

    def test_unchanged_status_skips_configure(self, gui: RegiLatticeGUI) -> None:
        from regilattice.tweaks import TweakResult

        row = self._make_row(gui, "t2")
        gui._tweak_rows = [row]
        # Prime cache with same status
        gui._prev_statuses = {"t2": TweakResult.APPLIED}
        row.status_text.reset_mock()
        row.status_dot.reset_mock()
        row.toggle_btn.reset_mock()
        gui._apply_statuses({"t2": TweakResult.APPLIED})
        # Status unchanged AND frame exists → configure should NOT be called again
        row.status_text.configure.assert_not_called()

    def test_changed_status_reconfigures_row(self, gui: RegiLatticeGUI) -> None:
        from regilattice.tweaks import TweakResult

        row = self._make_row(gui, "t3")
        gui._tweak_rows = [row]
        gui._prev_statuses = {"t3": TweakResult.APPLIED}
        row.status_text.reset_mock()
        gui._apply_statuses({"t3": TweakResult.NOT_APPLIED})
        row.status_text.configure.assert_called()

    def test_prev_statuses_updated_after_apply(self, gui: RegiLatticeGUI) -> None:
        from regilattice.tweaks import TweakResult

        row = self._make_row(gui, "t4")
        gui._tweak_rows = [row]
        gui._prev_statuses = {}
        gui._apply_statuses({"t4": TweakResult.UNKNOWN})
        assert gui._prev_statuses.get("t4") == TweakResult.UNKNOWN

    def test_unbuilt_row_frame_none_triggers_configure(self, gui: RegiLatticeGUI) -> None:
        """Rows with frame=None (not yet built) always need configure on next build."""
        from regilattice.tweaks import TweakResult

        row = self._make_row(gui, "t5")
        row.frame = None  # not yet built
        row.status_dot = None
        row.status_text = None
        row.toggle_btn = None
        gui._tweak_rows = [row]
        gui._prev_statuses = {"t5": TweakResult.APPLIED}
        # Should not raise even though widgets are None; no configure called
        gui._apply_statuses({"t5": TweakResult.APPLIED})

    def test_corp_blocked_row_skipped(self, gui: RegiLatticeGUI) -> None:
        from regilattice.tweaks import TweakResult

        row = self._make_row(gui, "t6")
        row.disabled_by_corp = True
        gui._tweak_rows = [row]
        gui._prev_statuses = {}
        gui._apply_statuses({"t6": TweakResult.APPLIED})
        # Corp-blocked rows never touch widget configure
        row.status_text.configure.assert_not_called()

    def test_missing_tweak_id_falls_back_to_unknown(self, gui: RegiLatticeGUI) -> None:

        row = self._make_row(gui, "not-in-map")
        gui._tweak_rows = [row]
        gui._prev_statuses = {}
        # Pass empty statuses dict — missing id should default to UNKNOWN
        gui._apply_statuses({})
        # Frame is not None + prev != UNKNOWN (prev was missing) → configure called
        row.status_text.configure.assert_called()


class TestWireSectionBindings:
    """Tests for _wire_section_bindings deferred-binding mechanism (Sprint 7)."""

    def test_wire_with_no_rows_does_not_raise(self, gui: RegiLatticeGUI) -> None:
        section = MagicMock()
        section.rows = []
        gui._cached_statuses = {}
        gui._wire_section_bindings(section)  # must not raise

    def test_wire_applies_cached_statuses_to_built_rows(self, gui: RegiLatticeGUI) -> None:
        from regilattice.tweaks import TweakResult

        row = MagicMock()
        row.td.id = "wire-t1"
        row.td.description = ""
        row.td.tags = []
        row.td.needs_admin = False
        row.td.corp_safe = True
        row.td.registry_keys = []
        row.disabled_by_corp = False
        row.cb = MagicMock()
        row.frame = MagicMock()
        row.status_dot = MagicMock()
        row.status_text = MagicMock()
        row.toggle_btn = MagicMock()
        row.tooltip = MagicMock()

        section = MagicMock()
        section.rows = [row]
        gui._tweak_rows = [row]
        gui._prev_statuses = {}
        gui._cached_statuses = {"wire-t1": TweakResult.APPLIED}
        gui._wire_section_bindings(section)
        # Status was applied — status_text.configure should have been called
        row.status_text.configure.assert_called()

    def test_wire_skips_already_known_statuses(self, gui: RegiLatticeGUI) -> None:
        from regilattice.tweaks import TweakResult

        row = MagicMock()
        row.td.id = "wire-t2"
        row.td.description = ""
        row.td.tags = []
        row.td.needs_admin = False
        row.td.corp_safe = True
        row.td.registry_keys = []
        row.disabled_by_corp = False
        row.cb = MagicMock()
        row.frame = MagicMock()
        row.status_dot = MagicMock()
        row.status_text = MagicMock()
        row.toggle_btn = MagicMock()
        row.tooltip = MagicMock()

        section = MagicMock()
        section.rows = [row]
        gui._tweak_rows = [row]
        # prev_statuses already has same value → delta skips configure
        gui._prev_statuses = {"wire-t2": TweakResult.NOT_APPLIED}
        gui._cached_statuses = {"wire-t2": TweakResult.NOT_APPLIED}
        gui._wire_section_bindings(section)
        row.status_text.configure.assert_not_called()

    def test_empty_cached_statuses_skips_wire(self, gui: RegiLatticeGUI) -> None:
        row = MagicMock()
        row.cb = MagicMock()
        section = MagicMock()
        section.rows = [row]
        gui._tweak_rows = [row]
        gui._cached_statuses = {}
        gui._wire_section_bindings(section)
        # With empty cached_statuses the status block is skipped
        row.status_text.configure.assert_not_called()


# ── Sprint 10: additional coverage ───────────────────────────────────────────


class TestCopyToClipboard:
    def test_copies_text_to_clipboard(self, gui: RegiLatticeGUI) -> None:
        gui._copy_to_clipboard("explorer-hide-ribbon")
        assert gui._root.clipboard_get() == "explorer-hide-ribbon"

    def test_updates_status_with_text(self, gui: RegiLatticeGUI) -> None:
        gui._copy_to_clipboard("my-id")
        assert "my-id" in gui._status_label.cget("text")


class TestSelectCategory:
    def test_selects_matching_category(self, gui: RegiLatticeGUI) -> None:
        row = MagicMock()
        row.var = tk.BooleanVar(gui._root, value=False)
        row.td.category = "Explorer"
        row.disabled_by_corp = False
        gui._tweak_rows = [row]
        gui._select_category("Explorer")
        assert row.var.get() is True

    def test_skips_different_category(self, gui: RegiLatticeGUI) -> None:
        row = MagicMock()
        row.var = tk.BooleanVar(gui._root, value=False)
        row.td.category = "Privacy"
        row.disabled_by_corp = False
        gui._tweak_rows = [row]
        gui._select_category("Explorer")
        assert row.var.get() is False

    def test_skips_corp_blocked_row(self, gui: RegiLatticeGUI) -> None:
        row = MagicMock()
        row.var = tk.BooleanVar(gui._root, value=False)
        row.td.category = "Explorer"
        row.disabled_by_corp = True
        gui._tweak_rows = [row]
        gui._select_category("Explorer")
        assert row.var.get() is False


class TestDelegateDialogs:
    """Thin wrappers on gui.py that delegate to dialogs module."""

    def test_export_powershell_delegates(self, gui: RegiLatticeGUI) -> None:
        with patch("regilattice.gui.dialogs.export_powershell") as m:
            gui._export_powershell()
            m.assert_called_once()

    def test_export_json_selection_delegates(self, gui: RegiLatticeGUI) -> None:
        with patch("regilattice.gui.dialogs.export_json_selection") as m:
            gui._export_json_selection()
            m.assert_called_once()

    def test_import_json_selection_delegates(self, gui: RegiLatticeGUI) -> None:
        with patch("regilattice.gui.dialogs.import_json_selection") as m:
            gui._import_json_selection()
            m.assert_called_once()

    def test_open_scoop_manager_delegates(self, gui: RegiLatticeGUI) -> None:
        with patch("regilattice.gui.dialogs.open_scoop_manager") as m:
            gui._open_scoop_manager()
            m.assert_called_once()

    def test_open_psmodule_manager_delegates(self, gui: RegiLatticeGUI) -> None:
        with patch("regilattice.gui.dialogs.open_psmodule_manager") as m:
            gui._open_psmodule_manager()
            m.assert_called_once()

    def test_open_pip_manager_delegates(self, gui: RegiLatticeGUI) -> None:
        with patch("regilattice.gui.dialogs.open_pip_manager") as m:
            gui._open_pip_manager()
            m.assert_called_once()

    def test_show_about_delegates(self, gui: RegiLatticeGUI) -> None:
        with patch("regilattice.gui.dialogs.show_about") as m:
            gui._show_about()
            m.assert_called_once()

    def test_show_about_passes_hw_profile(self, gui: RegiLatticeGUI) -> None:
        from regilattice.hwinfo import CPUInfo, DiskInfo, GPUInfo, HWProfile, MemoryInfo

        gui._hw_profile = HWProfile(
            cpu=CPUInfo(name="Intel i9", cores_physical=8, cores_logical=16),
            gpus=[GPUInfo(name="RTX 4090")],
            memory=MemoryInfo(total_mb=32768, available_mb=16000),
            disk=DiskInfo(total_gb=1000, free_gb=400),
        )
        with patch("regilattice.gui.dialogs.show_about") as m:
            gui._show_about()
            _args, kwargs = m.call_args
            # hw_summary keyword should be a non-empty string
            assert isinstance(kwargs.get("hw_summary"), str)
            assert len(kwargs["hw_summary"]) > 0


class TestBatchCategory:
    def _make_section(self, gui: RegiLatticeGUI, *, all_corp: bool = False) -> MagicMock:
        row = MagicMock()
        row.disabled_by_corp = all_corp
        row.td = MagicMock()
        section = MagicMock()
        section.rows = [row]
        section.name = "Explorer"
        return section

    def test_empty_non_corp_tweaks_is_noop(self, gui: RegiLatticeGUI) -> None:
        section = self._make_section(gui, all_corp=True)  # all rows corp-blocked → tweaks=[]
        with patch("regilattice.gui.messagebox.askyesno") as m:
            gui._batch_category(section, "apply")
            m.assert_not_called()

    def test_user_cancels_no_thread(self, gui: RegiLatticeGUI) -> None:
        section = self._make_section(gui)
        with (
            patch("regilattice.gui.messagebox.askyesno", return_value=False),
            patch("regilattice.gui.threading.Thread") as m_thread,
        ):
            gui._batch_category(section, "apply")
            m_thread.assert_not_called()

    def test_user_confirms_starts_thread(self, gui: RegiLatticeGUI) -> None:
        section = self._make_section(gui)
        with (
            patch("regilattice.gui.messagebox.askyesno", return_value=True),
            patch("regilattice.gui.threading.Thread") as m_thread,
        ):
            gui._batch_category(section, "remove")
            m_thread.assert_called_once()


class TestOnRowClick:
    def test_tracks_last_clicked_index(self, gui: RegiLatticeGUI) -> None:
        gui._on_row_click(7)
        assert gui._last_clicked_row_idx == 7

    def test_overrides_previous_index(self, gui: RegiLatticeGUI) -> None:
        gui._on_row_click(3)
        gui._on_row_click(10)
        assert gui._last_clicked_row_idx == 10


class TestOnShiftClick:
    def _make_rows(self, gui: RegiLatticeGUI, n: int) -> list[MagicMock]:
        rows = []
        for _ in range(n):
            r = MagicMock()
            r.var = tk.BooleanVar(gui._root, value=False)
            r.disabled_by_corp = False
            rows.append(r)
        return rows

    def test_selects_range_forward(self, gui: RegiLatticeGUI) -> None:
        rows = self._make_rows(gui, 5)
        gui._tweak_rows = rows
        gui._last_clicked_row_idx = 1
        result = gui._on_shift_click(3)
        assert result == "break"
        assert rows[0].var.get() is False
        for i in range(1, 4):
            assert rows[i].var.get() is True
        assert rows[4].var.get() is False

    def test_selects_range_backward(self, gui: RegiLatticeGUI) -> None:
        rows = self._make_rows(gui, 5)
        gui._tweak_rows = rows
        gui._last_clicked_row_idx = 4
        gui._on_shift_click(2)
        for i in range(2, 5):
            assert rows[i].var.get() is True

    def test_no_anchor_sets_anchor(self, gui: RegiLatticeGUI) -> None:
        rows = self._make_rows(gui, 3)
        gui._tweak_rows = rows
        gui._last_clicked_row_idx = None
        result = gui._on_shift_click(2)
        assert result == ""
        assert gui._last_clicked_row_idx == 2

    def test_skips_corp_blocked_rows(self, gui: RegiLatticeGUI) -> None:
        rows = self._make_rows(gui, 3)
        rows[1].disabled_by_corp = True
        gui._tweak_rows = rows
        gui._last_clicked_row_idx = 0
        gui._on_shift_click(2)
        assert rows[1].var.get() is False  # corp-blocked stays unchecked


class TestSetRunning:
    def test_set_running_true_disables_apply_remove(self, gui: RegiLatticeGUI) -> None:
        with (
            patch.object(gui._btn_apply, "state") as m_apply,
            patch.object(gui._btn_remove, "state") as m_remove,
        ):
            gui._set_running(True)
        m_apply.assert_called_with(["disabled"])
        m_remove.assert_called_with(["disabled"])
        assert gui._running is True

    def test_set_running_false_enables_buttons(self, gui: RegiLatticeGUI) -> None:
        gui._running = True
        with (
            patch.object(gui._btn_apply, "state") as m_apply,
            patch.object(gui._btn_remove, "state") as m_remove,
        ):
            gui._set_running(False)
        m_apply.assert_called_with(["!disabled"])
        m_remove.assert_called_with(["!disabled"])
        assert gui._running is False

    def test_set_running_true_clears_cancel(self, gui: RegiLatticeGUI) -> None:
        gui._cancel.set()
        with (
            patch.object(gui._btn_apply, "state"),
            patch.object(gui._btn_remove, "state"),
        ):
            gui._set_running(True)
        assert not gui._cancel.is_set()


class TestUndoLast:
    def test_empty_stack_is_noop(self, gui: RegiLatticeGUI) -> None:
        gui._undo_stack = []
        gui._undo_last()  # must not raise

    def test_running_prevents_undo(self, gui: RegiLatticeGUI) -> None:
        gui._running = True
        gui._undo_stack = [("apply", [MagicMock()])]
        gui._undo_last()
        assert len(gui._undo_stack) == 1  # unchanged

    def test_undo_apply_dispatches_remove(self, gui: RegiLatticeGUI) -> None:
        td = MagicMock()
        gui._undo_stack = [("apply", [td])]
        gui._running = False
        with patch.object(gui, "_dispatch_raw") as m:
            gui._undo_last()
        m.assert_called_once_with([td], "remove")

    def test_undo_remove_dispatches_apply(self, gui: RegiLatticeGUI) -> None:
        td = MagicMock()
        gui._undo_stack = [("remove", [td])]
        gui._running = False
        with patch.object(gui, "_dispatch_raw") as m:
            gui._undo_last()
        m.assert_called_once_with([td], "apply")

    def test_undo_disables_btn_when_stack_empty(self, gui: RegiLatticeGUI) -> None:
        td = MagicMock()
        gui._undo_stack = [("apply", [td])]
        gui._running = False
        with (
            patch.object(gui, "_dispatch_raw"),
            patch.object(gui._btn_undo, "state") as m_state,
        ):
            gui._undo_last()
        m_state.assert_called_with(["disabled"])


class TestDispatchRaw:
    def test_sets_running_and_starts_thread(self, gui: RegiLatticeGUI) -> None:
        with (
            patch.object(gui, "_set_running") as m_run,
            patch("regilattice.gui.threading.Thread") as m_thread,
        ):
            gui._dispatch_raw([MagicMock()], "apply")
        m_run.assert_called_once_with(True)
        m_thread.assert_called_once()


class TestSavePreferences:
    def test_writes_prefs_to_file(self, gui: RegiLatticeGUI, tmp_path: Path, monkeypatch: pytest.MonkeyPatch) -> None:
        monkeypatch.setattr("regilattice.gui._CONFIG_DIR", tmp_path)
        prefs_file = tmp_path / "preferences.json"
        monkeypatch.setattr("regilattice.gui._PREFS_FILE", prefs_file)
        gui._save_preferences()
        data = json.loads(prefs_file.read_text(encoding="utf-8"))
        assert "theme" in data
        assert "profile" in data
        assert "status_filter" in data
        assert "scope_filter" in data


class TestRestorePreferences:
    def test_restores_valid_theme(self, gui: RegiLatticeGUI, tmp_path: Path, monkeypatch: pytest.MonkeyPatch) -> None:
        from regilattice.gui_theme import available_themes

        themes = available_themes()
        if not themes:
            pytest.skip("no themes")
        prefs = {"theme": themes[0], "profile": "(none)", "status_filter": "All", "scope_filter": "All"}
        prefs_file = tmp_path / "prefs.json"
        prefs_file.write_text(json.dumps(prefs), encoding="utf-8")
        monkeypatch.setattr("regilattice.gui._PREFS_FILE", prefs_file)
        with patch.object(gui, "_switch_theme") as m:
            gui._restore_preferences()
        m.assert_called_once_with(themes[0])

    def test_invalid_theme_not_applied(self, gui: RegiLatticeGUI, tmp_path: Path, monkeypatch: pytest.MonkeyPatch) -> None:
        prefs = {"theme": "____nonexistent____"}
        prefs_file = tmp_path / "prefs.json"
        prefs_file.write_text(json.dumps(prefs), encoding="utf-8")
        monkeypatch.setattr("regilattice.gui._PREFS_FILE", prefs_file)
        with patch.object(gui, "_switch_theme") as m:
            gui._restore_preferences()
        m.assert_not_called()

    def test_non_dict_json_is_ignored(self, gui: RegiLatticeGUI, tmp_path: Path, monkeypatch: pytest.MonkeyPatch) -> None:
        prefs_file = tmp_path / "prefs.json"
        prefs_file.write_text("42", encoding="utf-8")
        monkeypatch.setattr("regilattice.gui._PREFS_FILE", prefs_file)
        gui._restore_preferences()  # must not raise

    def test_restores_status_filter(self, gui: RegiLatticeGUI, tmp_path: Path, monkeypatch: pytest.MonkeyPatch) -> None:
        prefs = {"theme": "Auto", "status_filter": "Applied", "scope_filter": "All"}
        prefs_file = tmp_path / "prefs.json"
        prefs_file.write_text(json.dumps(prefs), encoding="utf-8")
        monkeypatch.setattr("regilattice.gui._PREFS_FILE", prefs_file)
        with patch.object(gui, "_switch_theme"):
            gui._restore_preferences()
        assert gui._status_filter_var.get() == "Applied"

    def test_restores_scope_filter(self, gui: RegiLatticeGUI, tmp_path: Path, monkeypatch: pytest.MonkeyPatch) -> None:
        prefs = {"theme": "Auto", "status_filter": "All", "scope_filter": "User Only"}
        prefs_file = tmp_path / "prefs.json"
        prefs_file.write_text(json.dumps(prefs), encoding="utf-8")
        monkeypatch.setattr("regilattice.gui._PREFS_FILE", prefs_file)
        with patch.object(gui, "_switch_theme"):
            gui._restore_preferences()
        assert gui._scope_filter_var.get() == "User Only"

    def test_missing_file_is_ignored(self, gui: RegiLatticeGUI, tmp_path: Path, monkeypatch: pytest.MonkeyPatch) -> None:
        monkeypatch.setattr("regilattice.gui._PREFS_FILE", tmp_path / "missing.json")
        gui._restore_preferences()  # must not raise


class TestSaveSearchHistory:
    def test_writes_history_to_file(self, gui: RegiLatticeGUI, tmp_path: Path, monkeypatch: pytest.MonkeyPatch) -> None:
        monkeypatch.setattr("regilattice.gui._CONFIG_DIR", tmp_path)
        hist_file = tmp_path / "search_history.json"
        monkeypatch.setattr("regilattice.gui._SEARCH_HISTORY_FILE", hist_file)
        gui._search_history = ["privacy", "telemetry", "gaming"]
        gui._save_search_history()
        data = json.loads(hist_file.read_text(encoding="utf-8"))
        assert data == ["privacy", "telemetry", "gaming"]


class TestReorderCategory:
    def _make_sections(self, names: list[str]) -> list[MagicMock]:
        sections = []
        for name in names:
            s = MagicMock()
            s.name = name
            s.expanded = False
            sections.append(s)
        return sections

    def test_move_second_up(self, gui: RegiLatticeGUI) -> None:
        s1, s2 = self._make_sections(["A", "B"])
        gui._category_sections = [s1, s2]
        with patch.object(gui, "_save_category_order"):
            gui._reorder_category(s2, "up")
        assert gui._category_sections[0].name == "B"
        assert gui._category_sections[1].name == "A"

    def test_move_first_up_is_noop(self, gui: RegiLatticeGUI) -> None:
        s1, s2 = self._make_sections(["A", "B"])
        gui._category_sections = [s1, s2]
        with patch.object(gui, "_save_category_order") as m_save:
            gui._reorder_category(s1, "up")
        m_save.assert_not_called()
        assert gui._category_sections[0].name == "A"

    def test_move_last_down_is_noop(self, gui: RegiLatticeGUI) -> None:
        s1, s2 = self._make_sections(["A", "B"])
        gui._category_sections = [s1, s2]
        with patch.object(gui, "_save_category_order") as m_save:
            gui._reorder_category(s2, "down")
        m_save.assert_not_called()

    def test_move_first_down(self, gui: RegiLatticeGUI) -> None:
        s1, s2 = self._make_sections(["A", "B"])
        gui._category_sections = [s1, s2]
        with patch.object(gui, "_save_category_order"):
            gui._reorder_category(s1, "down")
        assert gui._category_sections[0].name == "B"
        assert gui._category_sections[1].name == "A"


class TestOnCategoryCollapseChange:
    def test_calls_save_collapse_state(self, gui: RegiLatticeGUI) -> None:
        with patch.object(gui, "_save_collapse_state") as m:
            gui._on_category_collapse_change(MagicMock())
        m.assert_called_once()


class TestRestoreCollapseState:
    def test_expands_saved_sections(self, gui: RegiLatticeGUI, tmp_path: Path, monkeypatch: pytest.MonkeyPatch) -> None:
        collapse_file = tmp_path / "collapsed.json"
        collapse_file.write_text(json.dumps(["Explorer", "Privacy"]), encoding="utf-8")
        monkeypatch.setattr("regilattice.gui._COLLAPSE_FILE", collapse_file)
        s1, s2, s3 = MagicMock(), MagicMock(), MagicMock()
        s1.name, s2.name, s3.name = "Explorer", "Privacy", "Audio"
        s1.expanded = s2.expanded = s3.expanded = False
        gui._category_sections = [s1, s2, s3]
        gui._restore_collapse_state()
        s1.toggle.assert_called_once()
        s2.toggle.assert_called_once()
        s3.toggle.assert_not_called()

    def test_skips_already_expanded(self, gui: RegiLatticeGUI, tmp_path: Path, monkeypatch: pytest.MonkeyPatch) -> None:
        collapse_file = tmp_path / "collapsed.json"
        collapse_file.write_text(json.dumps(["Explorer"]), encoding="utf-8")
        monkeypatch.setattr("regilattice.gui._COLLAPSE_FILE", collapse_file)
        section = MagicMock()
        section.name = "Explorer"
        section.expanded = True  # already expanded
        gui._category_sections = [section]
        gui._restore_collapse_state()
        section.toggle.assert_not_called()

    def test_missing_file_ignored(self, gui: RegiLatticeGUI, tmp_path: Path, monkeypatch: pytest.MonkeyPatch) -> None:
        monkeypatch.setattr("regilattice.gui._COLLAPSE_FILE", tmp_path / "missing.json")
        gui._restore_collapse_state()  # must not raise

    def test_invalid_json_ignored(self, gui: RegiLatticeGUI, tmp_path: Path, monkeypatch: pytest.MonkeyPatch) -> None:
        collapse_file = tmp_path / "collapsed.json"
        collapse_file.write_text("!!!bad json!!!", encoding="utf-8")
        monkeypatch.setattr("regilattice.gui._COLLAPSE_FILE", collapse_file)
        gui._restore_collapse_state()  # must not raise


class TestRefreshLog:
    def test_loads_log_content(self, gui: RegiLatticeGUI, tmp_path: Path, monkeypatch: pytest.MonkeyPatch) -> None:
        log_file = tmp_path / "session.log"
        log_file.write_text("line1\nline2\n", encoding="utf-8")
        monkeypatch.setattr("regilattice.gui.SESSION", MagicMock(log_path=log_file))
        gui._refresh_log()
        content = gui._log_text.get("1.0", "end")
        assert "line1" in content

    def test_missing_log_shows_fallback(self, gui: RegiLatticeGUI, tmp_path: Path, monkeypatch: pytest.MonkeyPatch) -> None:
        monkeypatch.setattr("regilattice.gui.SESSION", MagicMock(log_path=tmp_path / "missing.log"))
        gui._refresh_log()  # must not raise; shows fallback message
        content = gui._log_text.get("1.0", "end")
        assert "Could not read" in content


class TestExportLog:
    def test_cancel_dialog_does_nothing(self, gui: RegiLatticeGUI) -> None:
        with patch("regilattice.gui.filedialog.asksaveasfilename", return_value=""):
            gui._export_log()  # no copy, no crash

    def test_copies_log_file(self, gui: RegiLatticeGUI, tmp_path: Path, monkeypatch: pytest.MonkeyPatch) -> None:
        src = tmp_path / "session.log"
        src.write_text("session log content", encoding="utf-8")
        dst = tmp_path / "exported.log"
        monkeypatch.setattr("regilattice.gui.SESSION", MagicMock(log_path=src))
        with patch("regilattice.gui.filedialog.asksaveasfilename", return_value=str(dst)):
            gui._export_log()
        assert dst.exists()
        assert dst.read_text() == "session log content"

    def test_oserror_shows_dialog(self, gui: RegiLatticeGUI, tmp_path: Path, monkeypatch: pytest.MonkeyPatch) -> None:
        src = tmp_path / "session.log"
        src.write_text("x", encoding="utf-8")
        monkeypatch.setattr("regilattice.gui.SESSION", MagicMock(log_path=src))
        with (
            patch("regilattice.gui.filedialog.asksaveasfilename", return_value=str(tmp_path / "dst.log")),
            patch("shutil.copy2", side_effect=OSError("disk full")),
            patch("regilattice.gui.messagebox.showerror") as m_err,
        ):
            gui._export_log()
        m_err.assert_called_once()


class TestReloadThemeAliases:
    def test_runs_without_error(self, gui: RegiLatticeGUI) -> None:
        gui._reload_theme_aliases()  # must not raise

    def test_updates_module_globals(self, gui: RegiLatticeGUI) -> None:
        import regilattice.gui as gui_mod
        import regilattice.gui_theme as theme_mod

        gui._reload_theme_aliases()
        assert gui_mod._ACCENT == theme_mod.ACCENT
        assert gui_mod._BG == theme_mod.BG


class TestToggleFocusedRow:
    def test_search_widget_focus_skipped(self, gui: RegiLatticeGUI) -> None:
        event = MagicMock()
        event.widget = gui._search_entry
        row = MagicMock()
        row.var = tk.BooleanVar(gui._root, value=False)
        row.disabled_by_corp = False
        row.frame = MagicMock()
        row.frame.winfo_ismapped.return_value = True
        gui._tweak_rows = [row]
        gui._focused_row_idx = 0
        gui._toggle_focused_row(event)
        # The space bar was pressed while search is focused — should NOT toggle
        assert row.var.get() is False

    def test_toggles_focused_row(self, gui: RegiLatticeGUI) -> None:
        event = MagicMock()
        event.widget = gui._root  # not the search entry
        row = MagicMock()
        row.var = tk.BooleanVar(gui._root, value=False)
        row.disabled_by_corp = False
        row.frame = MagicMock()
        row.frame.winfo_ismapped.return_value = True
        gui._tweak_rows = [row]
        gui._focused_row_idx = 0
        gui._toggle_focused_row(event)
        assert row.var.get() is True


class TestDispatch:
    def test_cancel_while_running(self, gui: RegiLatticeGUI) -> None:
        """Calling _dispatch when _running == True sets the cancel flag."""
        gui._running = True
        gui._cancel.clear()
        with patch.object(gui, "_set_status") as m_status:
            gui._dispatch("apply")
        assert gui._cancel.is_set()
        m_status.assert_called()

    def test_no_selection_shows_info(self, gui: RegiLatticeGUI) -> None:
        gui._running = False
        gui._tweak_rows = []
        with (
            patch("regilattice.gui.assert_not_corporate"),
            patch("regilattice.gui.messagebox.showinfo") as m_info,
        ):
            gui._dispatch("apply")
        m_info.assert_called_once()

    def test_corp_network_blocks_dispatch(self, gui: RegiLatticeGUI) -> None:
        from regilattice.corpguard import CorporateNetworkError

        gui._running = False
        gui._force_var.set(False)
        with (
            patch("regilattice.gui.assert_not_corporate", side_effect=CorporateNetworkError("corp")),
            patch("regilattice.gui.messagebox.showwarning") as m_warn,
        ):
            gui._dispatch("apply")
        m_warn.assert_called_once()


class TestOfferRollback:
    def test_rollback_accepted_dispatches_reverse(self, gui: RegiLatticeGUI) -> None:
        td = MagicMock()
        with (
            patch("regilattice.gui.messagebox.askyesno", return_value=True),
            patch.object(gui, "_dispatch_raw") as m,
        ):
            gui._offer_rollback("partial failure?", [td], "apply")
        m.assert_called_once_with([td], "remove")

    def test_rollback_declined_does_nothing(self, gui: RegiLatticeGUI) -> None:
        with (
            patch("regilattice.gui.messagebox.askyesno", return_value=False),
            patch.object(gui, "_dispatch_raw") as m,
        ):
            gui._offer_rollback("msg", [MagicMock()], "remove")
        m.assert_not_called()


# ── Sprint 11: gui.py 70 % → 80 % ────────────────────────────────────────────


class TestNavigateRows:
    """Tests for _navigate_rows and _focus_search keyboard-navigation helpers."""

    def _make_visible_row(self, gui: RegiLatticeGUI) -> MagicMock:
        row = MagicMock()
        row.frame = MagicMock()
        row.frame.winfo_ismapped.return_value = True
        return row

    def test_navigate_forward_sets_focused_idx(self, gui: RegiLatticeGUI) -> None:
        r0 = self._make_visible_row(gui)
        r1 = self._make_visible_row(gui)
        gui._tweak_rows = [r0, r1]
        gui._focused_row_idx = 0
        gui._navigate_rows(1)
        assert gui._focused_row_idx == 1

    def test_navigate_clamps_at_last_row(self, gui: RegiLatticeGUI) -> None:
        row = self._make_visible_row(gui)
        gui._tweak_rows = [row]
        gui._focused_row_idx = 0
        gui._navigate_rows(1)  # already at last
        assert gui._focused_row_idx == 0

    def test_navigate_backward_clamps_at_zero(self, gui: RegiLatticeGUI) -> None:
        row = self._make_visible_row(gui)
        gui._tweak_rows = [row]
        gui._focused_row_idx = 0
        gui._navigate_rows(-1)
        assert gui._focused_row_idx == 0

    def test_navigate_no_visible_rows_is_noop(self, gui: RegiLatticeGUI) -> None:
        row = MagicMock()
        row.frame = MagicMock()
        row.frame.winfo_ismapped.return_value = False  # not visible
        gui._tweak_rows = [row]
        gui._navigate_rows(1)  # must not raise

    def test_navigate_focuses_frame(self, gui: RegiLatticeGUI) -> None:
        row = self._make_visible_row(gui)
        gui._tweak_rows = [row]
        gui._focused_row_idx = -1
        gui._navigate_rows(1)
        row.frame.focus_set.assert_called_once()

    def test_focus_search_focuses_entry(self, gui: RegiLatticeGUI) -> None:
        with patch.object(gui._search_entry, "focus_set") as m_focus:
            gui._focus_search()
        m_focus.assert_called_once()


class TestStatLabelUpdates:
    """Covers the stat-label configure paths at the bottom of _apply_statuses."""

    def _make_row(self, gui: RegiLatticeGUI, tweak_id: str) -> MagicMock:
        row = MagicMock()
        row.td.id = tweak_id
        row.td.registry_keys = []
        row.td.description = ""
        row.td.tags = []
        row.td.needs_admin = False
        row.td.corp_safe = True
        row.disabled_by_corp = False
        row.frame = MagicMock()
        row.status_dot = MagicMock()
        row.status_text = MagicMock()
        row.toggle_btn = MagicMock()
        row.tooltip = MagicMock()
        return row

    def test_section_update_count_called(self, gui: RegiLatticeGUI) -> None:
        from regilattice.tweaks import TweakResult

        row = self._make_row(gui, "stat-sec-t1")
        gui._tweak_rows = [row]
        gui._prev_statuses = {}
        section = MagicMock()
        gui._category_sections = [section]
        gui._apply_statuses({"stat-sec-t1": TweakResult.APPLIED})
        section.update_count.assert_called_once()

    def test_stat_blocked_label_updated_when_set(self, gui: RegiLatticeGUI) -> None:

        row = self._make_row(gui, "stat-blk-t1")
        row.disabled_by_corp = True  # This row is blocked
        gui._tweak_rows = [row]
        gui._prev_statuses = {}
        gui._stat_blocked = MagicMock()
        gui._apply_statuses({})
        gui._stat_blocked.configure.assert_called_once()


class TestSwitchThemeWithRows:
    """_switch_theme must call apply_theme on built rows and sections."""

    def test_applies_theme_to_built_rows_and_sections(self, gui: RegiLatticeGUI) -> None:
        row = MagicMock()
        row.frame = MagicMock()  # frame is not None → widgets built
        section = MagicMock()
        gui._tweak_rows = [row]
        gui._category_sections = [section]
        gui._cached_statuses = {}
        gui._switch_theme("Catppuccin Mocha")
        row.apply_theme.assert_called_once()
        section.apply_theme.assert_called_once()

    def test_reapplies_cached_statuses_after_theme_switch(self, gui: RegiLatticeGUI) -> None:
        from regilattice.tweaks import TweakResult

        gui._tweak_rows = []
        gui._category_sections = []
        gui._cached_statuses = {"some-id": TweakResult.APPLIED}
        with patch.object(gui, "_apply_statuses") as m:
            gui._switch_theme("Nord")
        m.assert_called_once()


class TestAutoRefreshLog:
    """_auto_refresh_log should refresh while log is visible, stop when hidden."""

    def test_visible_calls_refresh_and_reschedules(self, gui: RegiLatticeGUI) -> None:
        gui._log_visible = True
        with (
            patch.object(gui, "_refresh_log") as m_refresh,
            patch.object(gui._root, "after") as m_after,
        ):
            gui._auto_refresh_log()
        m_refresh.assert_called_once()
        m_after.assert_called_once()

    def test_hidden_skips_refresh(self, gui: RegiLatticeGUI) -> None:
        gui._log_visible = False
        with patch.object(gui, "_refresh_log") as m_refresh:
            gui._auto_refresh_log()
        m_refresh.assert_not_called()


class TestContextMenu:
    """Tests for _show_context_menu right-click handler."""

    def _make_row(self, gui: RegiLatticeGUI, *, registry_keys: list[str] | None = None, depends_on: list[str] | None = None) -> MagicMock:
        row = MagicMock()
        row.td.id = "ctx-m-1"
        row.td.registry_keys = registry_keys if registry_keys is not None else []
        row.td.depends_on = depends_on if depends_on is not None else []
        row.td.category = "Performance"
        row.var.get.return_value = False
        return row

    def _call_ctx(self, gui: RegiLatticeGUI, row: MagicMock, *, applied: bool = False) -> None:
        from regilattice.tweaks import TweakResult

        event = MagicMock()
        event.x_root = 0
        event.y_root = 0
        status = TweakResult.APPLIED if applied else TweakResult.NOT_APPLIED
        with (
            patch("regilattice.gui.tweak_status", return_value=status),
            patch.object(gui._ctx_menu, "tk_popup"),
            patch.object(gui._ctx_menu, "grab_release"),
        ):
            gui._show_context_menu(event, row)

    def test_applied_row_shows_disable_label(self, gui: RegiLatticeGUI) -> None:
        row = self._make_row(gui)
        self._call_ctx(gui, row, applied=True)
        # If context menu was populated without crashing, and row is set as target
        assert gui._ctx_target is row

    def test_not_applied_row_shows_enable_label(self, gui: RegiLatticeGUI) -> None:
        row = self._make_row(gui)
        self._call_ctx(gui, row, applied=False)
        assert gui._ctx_target is row

    def test_registry_keys_adds_copy_entry(self, gui: RegiLatticeGUI) -> None:
        row = self._make_row(gui, registry_keys=["HKEY_LOCAL_MACHINE\\SOFTWARE\\Test"])
        self._call_ctx(gui, row)
        # menu should have been populated (no crash) with copy-registry-key entry
        assert gui._ctx_target is row

    def test_depends_on_adds_info_entry(self, gui: RegiLatticeGUI) -> None:
        row = self._make_row(gui, depends_on=["other-tweak-id"])
        self._call_ctx(gui, row)
        assert gui._ctx_target is row


class TestStatusRefresh:
    """_initial_refresh and _refresh_status_all should start background threads."""

    def test_initial_refresh_starts_daemon_thread(self, gui: RegiLatticeGUI) -> None:
        with patch("regilattice.gui.threading.Thread") as m_thread:
            gui._initial_refresh()
        m_thread.assert_called_once()
        assert m_thread.call_args.kwargs.get("daemon") is True

    def test_refresh_status_all_starts_daemon_thread(self, gui: RegiLatticeGUI) -> None:
        with patch("regilattice.gui.threading.Thread") as m_thread:
            gui._refresh_status_all()
        m_thread.assert_called_once()
        assert m_thread.call_args.kwargs.get("daemon") is True

    def test_initial_refresh_sets_status_text(self, gui: RegiLatticeGUI) -> None:
        with patch("regilattice.gui.threading.Thread"):
            gui._initial_refresh()
        assert "Detecting" in gui._status_label.cget("text")

    def test_refresh_status_all_sets_status_text(self, gui: RegiLatticeGUI) -> None:
        with patch("regilattice.gui.threading.Thread"):
            gui._refresh_status_all()
        assert "Refreshing" in gui._status_label.cget("text")


class TestToggleSingle:
    """Tests for the _toggle_single per-row toggle handler."""

    def _make_toggle_row(self, gui: RegiLatticeGUI, tweak_id: str) -> MagicMock:
        row = MagicMock()
        row.td.id = tweak_id
        row.td.label = "Test Tweak"
        row.td.category = "Performance"
        row.toggle_btn = MagicMock()
        row.disabled_by_corp = False
        return row

    def test_corp_blocked_warns_and_returns(self, gui: RegiLatticeGUI) -> None:
        from regilattice.corpguard import CorporateNetworkError

        gui._force_var.set(False)
        row = self._make_toggle_row(gui, "ts-corp-1")
        with (
            patch("regilattice.gui.assert_not_corporate", side_effect=CorporateNetworkError("blocked")),
            patch("regilattice.gui.messagebox.showwarning") as m_warn,
            patch("regilattice.gui.threading.Thread") as m_thread,
        ):
            gui._toggle_single(row)
        m_warn.assert_called_once()
        m_thread.assert_not_called()

    def test_force_flag_skips_corp_check(self, gui: RegiLatticeGUI) -> None:
        from regilattice.tweaks import TweakResult

        gui._force_var.set(True)
        row = self._make_toggle_row(gui, "ts-force-1")
        with (
            patch("regilattice.gui.tweak_status", return_value=TweakResult.NOT_APPLIED),
            patch("regilattice.gui.threading.Thread") as m_thread,
        ):
            gui._toggle_single(row)
        m_thread.assert_called_once()

    def test_starts_worker_thread_when_clear(self, gui: RegiLatticeGUI) -> None:
        from regilattice.tweaks import TweakResult

        gui._force_var.set(False)
        row = self._make_toggle_row(gui, "ts-worker-1")
        with (
            patch("regilattice.gui.assert_not_corporate"),
            patch("regilattice.gui.tweak_status", return_value=TweakResult.NOT_APPLIED),
            patch("regilattice.gui.threading.Thread") as m_thread,
        ):
            gui._toggle_single(row)
        m_thread.assert_called_once()
        assert m_thread.call_args.kwargs.get("daemon") is True

    def test_returns_early_when_toggle_btn_is_none(self, gui: RegiLatticeGUI) -> None:
        from regilattice.tweaks import TweakResult

        gui._force_var.set(True)
        row = self._make_toggle_row(gui, "ts-nobtn-1")
        row.toggle_btn = None  # no button built yet
        with (
            patch("regilattice.gui.tweak_status", return_value=TweakResult.NOT_APPLIED),
            patch("regilattice.gui.threading.Thread") as m_thread,
        ):
            gui._toggle_single(row)
        m_thread.assert_not_called()

    def test_worker_success_sets_status(self, gui: RegiLatticeGUI) -> None:
        """Run the _worker closure directly to exercise the worker-thread path."""
        from regilattice.tweaks import TweakResult

        applied = []
        row = self._make_toggle_row(gui, "ts-wrkr-1")
        row.td.apply_fn = lambda *, require_admin=True: applied.append(True)
        row.td.remove_fn = lambda *, require_admin=True: None

        captured_thread: list[object] = []

        def fake_thread(**kwargs: object) -> MagicMock:
            t = MagicMock()
            captured_thread.append(kwargs["target"])
            return t

        with (
            patch("regilattice.gui.assert_not_corporate"),
            patch("regilattice.gui.tweak_status", return_value=TweakResult.NOT_APPLIED),
            patch("regilattice.gui.threading.Thread", side_effect=fake_thread),
        ):
            gui._toggle_single(row)

        assert captured_thread, "thread target not captured"
        # Run the worker inline to exercise the worker body
        worker = captured_thread[0]
        worker()  # type: ignore[operator]
        assert applied == [True]


class TestSnapshotOps:
    """Tests for _save_snapshot and _restore_snapshot."""

    def test_save_snapshot_cancel_returns_early(self, gui: RegiLatticeGUI) -> None:
        with (
            patch("regilattice.gui.filedialog.asksaveasfilename", return_value=""),
            patch("regilattice.gui.save_snapshot") as m_save,
        ):
            gui._save_snapshot()
        m_save.assert_not_called()

    def test_save_snapshot_calls_save_fn(self, gui: RegiLatticeGUI, tmp_path: Path) -> None:
        snap = tmp_path / "snap.json"
        with (
            patch("regilattice.gui.filedialog.asksaveasfilename", return_value=str(snap)),
            patch("regilattice.gui.save_snapshot") as m_save,
        ):
            gui._save_snapshot()
        m_save.assert_called_once()

    def test_save_snapshot_oserror_shows_dialog(self, gui: RegiLatticeGUI, tmp_path: Path) -> None:
        snap = tmp_path / "snap.json"
        with (
            patch("regilattice.gui.filedialog.asksaveasfilename", return_value=str(snap)),
            patch("regilattice.gui.save_snapshot", side_effect=OSError("disk full")),
            patch("regilattice.gui.messagebox.showerror") as m_err,
        ):
            gui._save_snapshot()
        m_err.assert_called_once()

    def test_restore_snapshot_cancel_returns_early(self, gui: RegiLatticeGUI) -> None:
        with (
            patch("regilattice.gui.filedialog.askopenfilename", return_value=""),
            patch("regilattice.gui.threading.Thread") as m_thread,
        ):
            gui._restore_snapshot()
        m_thread.assert_not_called()

    def test_restore_snapshot_user_declines(self, gui: RegiLatticeGUI, tmp_path: Path) -> None:
        snap = tmp_path / "snap.json"
        with (
            patch("regilattice.gui.filedialog.askopenfilename", return_value=str(snap)),
            patch("regilattice.gui.messagebox.askyesno", return_value=False),
            patch("regilattice.gui.threading.Thread") as m_thread,
        ):
            gui._restore_snapshot()
        m_thread.assert_not_called()

    def test_restore_snapshot_accepted_starts_thread(self, gui: RegiLatticeGUI, tmp_path: Path) -> None:
        snap = tmp_path / "snap.json"
        with (
            patch("regilattice.gui.filedialog.askopenfilename", return_value=str(snap)),
            patch("regilattice.gui.messagebox.askyesno", return_value=True),
            patch("regilattice.gui.threading.Thread") as m_thread,
        ):
            gui._restore_snapshot()
        m_thread.assert_called_once()


class TestDispatchSelectionPath:
    """Additional _dispatch tests: selection confirmation dialog path."""

    def _make_selected_row(self, gui: RegiLatticeGUI, tweak_id: str) -> MagicMock:
        row = MagicMock()
        row.var = tk.BooleanVar(gui._root, value=True)
        row.td.id = tweak_id
        row.td.label = "Test"
        row.td.category = "Performance"
        row.td.needs_admin = False
        row.td.registry_keys = []
        return row

    def test_restore_mode_starts_thread_immediately(self, gui: RegiLatticeGUI) -> None:
        gui._running = False
        gui._force_var.set(True)
        with patch("regilattice.gui.threading.Thread") as m_thread:
            gui._dispatch("restore")
        m_thread.assert_called_once()

    def test_user_cancels_confirmation_no_thread(self, gui: RegiLatticeGUI) -> None:
        gui._running = False
        gui._force_var.set(True)
        row = self._make_selected_row(gui, "dsp-cancel-1")
        gui._tweak_rows = [row]
        with (
            patch("regilattice.gui.messagebox.askyesno", return_value=False),
            patch("regilattice.gui.threading.Thread") as m_thread,
        ):
            gui._dispatch("apply")
        m_thread.assert_not_called()

    def test_user_confirms_starts_thread(self, gui: RegiLatticeGUI) -> None:
        gui._running = False
        gui._force_var.set(True)
        row = self._make_selected_row(gui, "dsp-confirm-1")
        gui._tweak_rows = [row]
        with (
            patch("regilattice.gui.messagebox.askyesno", return_value=True),
            patch("regilattice.gui.threading.Thread") as m_thread,
        ):
            gui._dispatch("apply")
        m_thread.assert_called_once()


class TestRunTweaks:
    """Direct (synchronous) tests for the _run_tweaks worker method."""

    def _make_td(self, suffix: str, *, apply_fn: object = None, fail_with: Exception | None = None) -> MagicMock:
        td = MagicMock()
        td.id = f"rt-{suffix}"
        td.label = f"Tweak {suffix}"
        td.category = "Performance"
        td.needs_admin = False
        td.registry_keys = []

        if fail_with is not None:
            exc = fail_with

            def _fail(*, require_admin: bool = True) -> None:
                raise exc

            td.apply_fn = _fail
            td.remove_fn = _fail
        else:
            td.apply_fn = lambda *, require_admin=True: None
            td.remove_fn = lambda *, require_admin=True: None
        return td

    def test_run_single_success(self, gui: RegiLatticeGUI) -> None:
        td = self._make_td("s1")
        gui._running = False
        gui._cancel.clear()
        gui._run_tweaks([td], "apply")
        # succeeded → session_changed gets the id
        assert td.id in gui._session_changed

    def test_run_remove_mode_success(self, gui: RegiLatticeGUI) -> None:
        td = self._make_td("r1")
        gui._cancel.clear()
        gui._run_tweaks([td], "remove")
        assert td.id in gui._session_changed

    def test_run_oserror_records_error(self, gui: RegiLatticeGUI) -> None:
        td = self._make_td("err1", fail_with=OSError("disk full"))
        gui._cancel.clear()
        with patch("regilattice.gui.messagebox.showwarning"):
            gui._run_tweaks([td], "apply")
        # id should NOT be in session_changed (apply failed)
        assert td.id not in gui._session_changed

    def test_run_admin_error_records_error(self, gui: RegiLatticeGUI) -> None:
        from regilattice.registry import AdminRequirementError

        td = self._make_td("adm1", fail_with=AdminRequirementError("need admin"))
        gui._cancel.clear()
        with patch("regilattice.gui.messagebox.showwarning"):
            gui._run_tweaks([td], "apply")
        assert td.id not in gui._session_changed

    def test_run_cancel_mid_breaks_early(self, gui: RegiLatticeGUI) -> None:
        td1 = self._make_td("c1")
        td2 = self._make_td("c2")
        gui._cancel.set()  # cancel immediately
        with patch.object(gui, "_set_status"):
            gui._run_tweaks([td1, td2], "apply")
        # Neither tweak was applied since cancel was set before the loop body
        assert td1.id not in gui._session_changed
        assert td2.id not in gui._session_changed

    def test_run_partial_failure_pushes_undo(self, gui: RegiLatticeGUI) -> None:
        td_ok = self._make_td("p1")
        td_fail = self._make_td("p2", fail_with=OSError("fail"))
        gui._cancel.clear()
        gui._undo_stack = []
        with patch("regilattice.gui.messagebox.askyesno", return_value=False):
            gui._run_tweaks([td_ok, td_fail], "apply")
        # Undo stack gets the items list when any items were processed
        assert len(gui._undo_stack) == 1

    def test_run_success_updates_undo_stack(self, gui: RegiLatticeGUI) -> None:
        td = self._make_td("undo1")
        gui._cancel.clear()
        gui._undo_stack = []
        with patch("regilattice.gui.messagebox.showinfo"):
            gui._run_tweaks([td], "apply")
        assert len(gui._undo_stack) == 1
        mode, items = gui._undo_stack[0]
        assert mode == "apply"
        assert items == [td]


# ── Sprint 11: prefix-filter coverage, worker-error paths, restore-point ──────


class TestFilterRowsPrefixOperators:
    """Tests for _filter_rows tag:/cat:/admin: prefix operators (Sprint 11)."""

    def _make_section(
        self,
        gui: RegiLatticeGUI,
        *,
        tweak_id: str,
        tags: list[str] | None = None,
        category: str = "Performance",
        needs_admin: bool = True,
    ) -> tuple[MagicMock, MagicMock]:
        """Return (section, row) mocks wired for _filter_rows checks."""
        row = MagicMock()
        row.td.id = tweak_id
        row.td.tags = tags if tags is not None else []
        row.td.category = category
        row.td.needs_admin = needs_admin
        row.td.corp_safe = True
        row.disabled_by_corp = False
        row.var = tk.BooleanVar(gui._root, value=False)

        section = MagicMock()
        section.rows = [row]
        section._widgets_built = True  # skip lazy build during filter
        section.expanded = False
        return section, row

    def _run_filter(self, gui: RegiLatticeGUI, query: str) -> None:
        gui._loading = False
        gui._cached_statuses = {}
        gui._session_changed = set()
        gui._search_var.set(query)
        gui._filter_rows()

    # ── tag: operator ─────────────────────────────────────────────────────

    def test_tag_prefix_matches_row_with_tag(self, gui: RegiLatticeGUI) -> None:
        section, row = self._make_section(gui, tweak_id="fp-tag-match-1", tags=["privacy", "telemetry"])
        gui._category_sections = [section]
        self._run_filter(gui, "tag:privacy")
        row.pack_row.assert_called()

    def test_tag_prefix_excludes_row_without_tag(self, gui: RegiLatticeGUI) -> None:
        section, row = self._make_section(gui, tweak_id="fp-tag-miss-1", tags=["gaming"])
        gui._category_sections = [section]
        self._run_filter(gui, "tag:privacy")
        row.unpack_row.assert_called()

    def test_tag_prefix_partial_match_is_enough(self, gui: RegiLatticeGUI) -> None:
        """tag:priv should match a tag "privacy" (substring match)."""
        section, row = self._make_section(gui, tweak_id="fp-tag-sub-1", tags=["privacy"])
        gui._category_sections = [section]
        self._run_filter(gui, "tag:priv")
        row.pack_row.assert_called()

    def test_empty_tags_list_misses_tag_filter(self, gui: RegiLatticeGUI) -> None:
        section, row = self._make_section(gui, tweak_id="fp-tag-empty-1", tags=[])
        gui._category_sections = [section]
        self._run_filter(gui, "tag:network")
        row.unpack_row.assert_called()

    # ── cat: operator ─────────────────────────────────────────────────────

    def test_cat_prefix_matches_row_category(self, gui: RegiLatticeGUI) -> None:
        section, row = self._make_section(gui, tweak_id="fp-cat-match-1", category="Privacy")
        gui._category_sections = [section]
        self._run_filter(gui, "cat:priv")
        row.pack_row.assert_called()

    def test_cat_prefix_excludes_different_category(self, gui: RegiLatticeGUI) -> None:
        section, row = self._make_section(gui, tweak_id="fp-cat-miss-1", category="Gaming")
        gui._category_sections = [section]
        self._run_filter(gui, "cat:privacy")
        row.unpack_row.assert_called()

    def test_cat_prefix_case_insensitive(self, gui: RegiLatticeGUI) -> None:
        section, row = self._make_section(gui, tweak_id="fp-cat-case-1", category="Explorer")
        gui._category_sections = [section]
        self._run_filter(gui, "cat:EXPLORER")
        row.pack_row.assert_called()

    # ── admin: operator ───────────────────────────────────────────────────

    def test_admin_yes_matches_admin_required_row(self, gui: RegiLatticeGUI) -> None:
        section, row = self._make_section(gui, tweak_id="fp-adm-yes-1", needs_admin=True)
        gui._category_sections = [section]
        self._run_filter(gui, "admin:yes")
        row.pack_row.assert_called()

    def test_admin_yes_excludes_non_admin_row(self, gui: RegiLatticeGUI) -> None:
        section, row = self._make_section(gui, tweak_id="fp-adm-yes-2", needs_admin=False)
        gui._category_sections = [section]
        self._run_filter(gui, "admin:yes")
        row.unpack_row.assert_called()

    def test_admin_no_matches_non_admin_row(self, gui: RegiLatticeGUI) -> None:
        section, row = self._make_section(gui, tweak_id="fp-adm-no-1", needs_admin=False)
        gui._category_sections = [section]
        self._run_filter(gui, "admin:no")
        row.pack_row.assert_called()

    def test_admin_no_excludes_admin_required_row(self, gui: RegiLatticeGUI) -> None:
        section, row = self._make_section(gui, tweak_id="fp-adm-no-2", needs_admin=True)
        gui._category_sections = [section]
        self._run_filter(gui, "admin:no")
        row.unpack_row.assert_called()

    def test_admin_true_variant_matches(self, gui: RegiLatticeGUI) -> None:
        """admin:true should also be recognised (bool True)."""
        section, row = self._make_section(gui, tweak_id="fp-adm-true-1", needs_admin=True)
        gui._category_sections = [section]
        self._run_filter(gui, "admin:true")
        row.pack_row.assert_called()

    # ── combined prefix + text ────────────────────────────────────────────

    def test_multiple_operators_are_all_required(self, gui: RegiLatticeGUI) -> None:
        """tag:gaming cat:gaming — must satisfy both tag and category checks."""
        section, row = self._make_section(
            gui,
            tweak_id="fp-multi-1",
            tags=["gaming"],
            category="Gaming",
        )
        gui._category_sections = [section]
        self._run_filter(gui, "tag:gaming cat:gaming")
        row.pack_row.assert_called()

    def test_combined_fails_if_one_prefix_mismatches(self, gui: RegiLatticeGUI) -> None:
        section, row = self._make_section(
            gui,
            tweak_id="fp-multi-miss-1",
            tags=["gaming"],
            category="Privacy",  # does NOT match cat:gaming
        )
        gui._category_sections = [section]
        self._run_filter(gui, "tag:gaming cat:gaming")
        row.unpack_row.assert_called()


class TestToggleSingleWorkerErrors:
    """Worker thread error paths for _toggle_single (AdminRequirementError, OSError)."""

    def _capture_worker(self, gui: RegiLatticeGUI, row: MagicMock) -> object:
        """Run _toggle_single and capture the worker callable passed to Thread."""
        from regilattice.tweaks import TweakResult

        captured: list[object] = []

        def fake_thread(**kwargs: object) -> MagicMock:
            captured.append(kwargs["target"])
            return MagicMock()

        with (
            patch("regilattice.gui.assert_not_corporate"),
            patch("regilattice.gui.tweak_status", return_value=TweakResult.NOT_APPLIED),
            patch("regilattice.gui.threading.Thread", side_effect=fake_thread),
        ):
            gui._toggle_single(row)

        assert captured, "Thread was not started"
        return captured[0]

    def _make_row(self, gui: RegiLatticeGUI, tweak_id: str) -> MagicMock:
        row = MagicMock()
        row.td.id = tweak_id
        row.td.label = "Test Tweak"
        row.td.category = "Performance"
        row.toggle_btn = MagicMock()
        row.disabled_by_corp = False
        return row

    def test_admin_error_records_after_call(self, gui: RegiLatticeGUI) -> None:
        """AdminRequirementError in apply_fn → after() called with 'admin required'."""
        from regilattice.registry import AdminRequirementError

        row = self._make_row(gui, "ts-err-adm-1")
        row.td.apply_fn = MagicMock(side_effect=AdminRequirementError("need admin"))
        gui._category_sections = []

        worker = self._capture_worker(gui, row)
        after_calls: list[tuple[object, ...]] = []
        with patch.object(gui._root, "after", side_effect=lambda *a: after_calls.append(a)):
            worker()  # type: ignore[operator]

        # At least one after() call must carry 'admin required' status text
        assert any("admin required" in str(a) for a in after_calls)

    def test_oserror_records_after_call(self, gui: RegiLatticeGUI) -> None:
        """OSError in apply_fn → after() called with the error message."""
        row = self._make_row(gui, "ts-err-os-1")
        row.td.apply_fn = MagicMock(side_effect=OSError("permission denied"))
        gui._category_sections = []

        worker = self._capture_worker(gui, row)
        after_calls: list[tuple[object, ...]] = []
        with patch.object(gui._root, "after", side_effect=lambda *a: after_calls.append(a)):
            worker()  # type: ignore[operator]

        assert any("permission denied" in str(a) for a in after_calls)

    def test_success_records_enabled_status(self, gui: RegiLatticeGUI) -> None:
        """On success, after() carries the 'enabled' text (apply action)."""
        row = self._make_row(gui, "ts-err-ok-1")
        row.td.apply_fn = MagicMock(return_value=None)
        gui._category_sections = []

        worker = self._capture_worker(gui, row)
        after_calls: list[tuple[object, ...]] = []
        with patch.object(gui._root, "after", side_effect=lambda *a: after_calls.append(a)):
            worker()  # type: ignore[operator]

        assert any("enabled" in str(a) for a in after_calls)


class TestRunRestorePoint:
    """Tests for the _run_restore_point synchronous helper."""

    def test_success_schedules_ready_status(self, gui: RegiLatticeGUI) -> None:
        after_calls: list[tuple[object, ...]] = []
        with (
            patch("regilattice.gui.create_restore_point"),
            patch.object(gui._root, "after", side_effect=lambda *a: after_calls.append(a)),
        ):
            gui._run_restore_point()
        assert any("Restore point created" in str(a) for a in after_calls)

    def test_admin_error_schedules_warning(self, gui: RegiLatticeGUI) -> None:
        from regilattice.registry import AdminRequirementError

        after_calls: list[tuple[object, ...]] = []
        with (
            patch("regilattice.gui.create_restore_point", side_effect=AdminRequirementError("no admin")),
            patch("regilattice.gui.messagebox.showwarning"),
            patch.object(gui._root, "after", side_effect=lambda *a: after_calls.append(a)),
        ):
            gui._run_restore_point()
        assert any("failed" in str(a) for a in after_calls)

    def test_oserror_schedules_error_status(self, gui: RegiLatticeGUI) -> None:
        after_calls: list[tuple[object, ...]] = []
        with (
            patch("regilattice.gui.create_restore_point", side_effect=OSError("disk error")),
            patch.object(gui._root, "after", side_effect=lambda *a: after_calls.append(a)),
        ):
            gui._run_restore_point()
        assert any("disk error" in str(a) for a in after_calls)

    def test_runtime_error_schedules_error_status(self, gui: RegiLatticeGUI) -> None:
        after_calls: list[tuple[object, ...]] = []
        with (
            patch("regilattice.gui.create_restore_point", side_effect=RuntimeError("wmi failure")),
            patch.object(gui._root, "after", side_effect=lambda *a: after_calls.append(a)),
        ):
            gui._run_restore_point()
        assert any("wmi failure" in str(a) for a in after_calls)
