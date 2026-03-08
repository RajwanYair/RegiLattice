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

    _probe_root = tk.Tk()
    _probe_root.withdraw()
    _probe_root.destroy()
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
    with patch.object(RegiLatticeGUI, "_deferred_init"):
        app = RegiLatticeGUI()
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
            patch("sys.exit") as mock_exit,
            patch("builtins.print"),
        ):
            launch()
        mock_exit.assert_called_once_with(1)

    def test_prints_message_on_non_windows(self, capsys: pytest.CaptureFixture[str]) -> None:
        with (
            patch("regilattice.gui.is_windows", return_value=False),
            patch("sys.exit"),
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
        # No collapsed sections → file removed or not created
        assert not (tmp_path / "collapsed.json").exists()

    def test_save_collapse_state_with_collapsed(self, gui: RegiLatticeGUI, tmp_path: Path, monkeypatch: pytest.MonkeyPatch) -> None:
        monkeypatch.setattr("regilattice.gui._CONFIG_DIR", tmp_path)
        collapse_file = tmp_path / "collapsed.json"
        monkeypatch.setattr("regilattice.gui._COLLAPSE_FILE", collapse_file)
        section = MagicMock()
        section.expanded = False
        section.name = "Privacy"
        gui._category_sections = [section]
        gui._save_collapse_state()
        assert collapse_file.exists()
        data = json.loads(collapse_file.read_text(encoding="utf-8"))
        assert "Privacy" in data


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
