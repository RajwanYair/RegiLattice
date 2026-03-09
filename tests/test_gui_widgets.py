"""Tests for regilattice.gui_widgets — TweakRow logic (deferred, no tkinter window needed)."""

from __future__ import annotations

import contextlib
from typing import TYPE_CHECKING

import pytest

from regilattice.tweaks import TweakDef

if TYPE_CHECKING:
    from regilattice.gui_widgets import CategorySection, TweakRow


def _make_td(
    *,
    tweak_id: str = "test-tweak",
    label: str = "Test Tweak",
    category: str = "Test",
    needs_admin: bool = False,
    corp_safe: bool = True,
    registry_keys: list[str] | None = None,
    description: str = "",
    tags: list[str] | None = None,
) -> TweakDef:
    return TweakDef(
        id=tweak_id,
        label=label,
        category=category,
        apply_fn=lambda: None,
        remove_fn=lambda: None,
        detect_fn=None,
        needs_admin=needs_admin,
        corp_safe=corp_safe,
        registry_keys=registry_keys or [],
        description=description,
        tags=tags or [],
    )


# Try to import Tk — skip GUI-dependent tests if unavailable
_tk_available = True
try:
    import tkinter as tk
except Exception:
    _tk_available = False


@pytest.fixture()
def root():  # type: ignore[return]
    """Create a hidden Tk root for the test; destroyed after each test."""
    if not _tk_available:
        pytest.skip("tkinter not available or headless environment")
    import tkinter as tk

    r = tk.Tk()
    r.withdraw()
    yield r
    with contextlib.suppress(Exception):
        r.destroy()


class TestTweakRowDeferred:
    """Test TweakRow creation with defer_widgets=True (no tkinter needed)."""

    def test_deferred_row_no_widgets(self, root: tk.Tk) -> None:
        from tkinter import ttk

        from regilattice.gui_widgets import TweakRow

        parent = ttk.Frame(root)
        td = _make_td()
        row = TweakRow(parent, td, corp_blocked=False, defer_widgets=True)
        assert row.td is td
        assert row.frame is None
        assert row.var.get() is False
        assert row.disabled_by_corp is False

    def test_deferred_corp_blocked(self, root: tk.Tk) -> None:
        from tkinter import ttk

        from regilattice.gui_widgets import TweakRow

        parent = ttk.Frame(root)
        td = _make_td(corp_safe=False)
        row = TweakRow(parent, td, corp_blocked=True, defer_widgets=True)
        assert row.disabled_by_corp is True

    def test_corp_safe_not_blocked(self, root: tk.Tk) -> None:
        from tkinter import ttk

        from regilattice.gui_widgets import TweakRow

        parent = ttk.Frame(root)
        td = _make_td(corp_safe=True)
        row = TweakRow(parent, td, corp_blocked=True, defer_widgets=True)
        assert row.disabled_by_corp is False


@pytest.mark.skipif(not _tk_available, reason="tkinter not available")
class TestTweakRowWidgets:
    """Test TweakRow with actual widget creation."""

    def test_build_widgets(self, root: tk.Tk) -> None:
        from tkinter import ttk

        from regilattice.gui_widgets import TweakRow

        parent = ttk.Frame(root)
        td = _make_td()
        row = TweakRow(parent, td, corp_blocked=False)
        assert row.frame is not None
        assert row.status_dot is not None
        assert row.status_text is not None
        assert row.cb is not None
        assert row.toggle_btn is not None
        assert row.tooltip is not None

    def test_mark_corp_blocked(self, root: tk.Tk) -> None:
        from tkinter import ttk

        from regilattice.gui_widgets import TweakRow

        parent = ttk.Frame(root)
        td = _make_td(corp_safe=False)
        row = TweakRow(parent, td, corp_blocked=False)
        assert row.disabled_by_corp is False
        row.mark_corp_blocked()
        assert row.disabled_by_corp is True
        assert row.var.get() is False
        assert row.status_text.cget("text") == "BLOCKED"

    def test_pack_unpack(self, root: tk.Tk) -> None:
        from tkinter import ttk

        from regilattice.gui_widgets import TweakRow

        parent = ttk.Frame(root)
        parent.pack()
        td = _make_td()
        row = TweakRow(parent, td, corp_blocked=False)
        row.pack_row()
        frame = row.frame
        assert frame is not None
        assert frame.winfo_manager() == "pack"
        row.unpack_row()
        assert frame.winfo_manager() == ""


@pytest.mark.skipif(not _tk_available, reason="tkinter not available")
class TestCategorySectionLazy:
    """Tests for CategorySection lazy widget build (Sprint 7 feature)."""

    def _make_section(self, root: tk.Tk, *, expanded: bool, n: int = 2) -> tuple[CategorySection, list[TweakRow]]:
        from tkinter import ttk

        from regilattice.gui_widgets import CategorySection, TweakRow

        parent = ttk.Frame(root)
        parent.pack()
        rows = [TweakRow(parent, _make_td(tweak_id=f"t-{i}"), corp_blocked=False, defer_widgets=True) for i in range(n)]
        section = CategorySection(parent, "Test Category", rows, expanded=expanded)
        return section, rows

    def test_collapsed_section_widgets_not_built(self, root: tk.Tk) -> None:
        section, rows = self._make_section(root, expanded=False)
        assert section._widgets_built is False
        # Row frames must not exist yet
        for row in rows:
            assert row.frame is None

    def test_expanded_section_builds_immediately(self, root: tk.Tk) -> None:
        section, rows = self._make_section(root, expanded=True)
        assert section._widgets_built is True
        for row in rows:
            assert row.frame is not None

    def test_toggle_triggers_lazy_build(self, root: tk.Tk) -> None:
        section, rows = self._make_section(root, expanded=False)
        assert section._widgets_built is False
        section.toggle()  # first expand → lazy build
        assert section._widgets_built is True
        for row in rows:
            assert row.frame is not None

    def test_toggle_again_does_not_rebuild(self, root: tk.Tk) -> None:
        """Second toggle (collapse then expand) must not rebuild widgets."""
        section, rows = self._make_section(root, expanded=False)
        section.toggle()  # expand — builds once
        first_frames = [row.frame for row in rows]
        section.toggle()  # collapse
        section.toggle()  # expand again
        for row, frm in zip(rows, first_frames, strict=False):
            # Same frame object — was not recreated
            assert row.frame is frm

    def test_on_rows_built_callback_fires(self, root: tk.Tk) -> None:

        section, _ = self._make_section(root, expanded=False)
        fired: list[CategorySection] = []
        section.set_on_rows_built(fired.append)
        section.toggle()  # triggers lazy build
        assert len(fired) == 1
        assert fired[0] is section

    def test_on_rows_built_not_fired_before_expand(self, root: tk.Tk) -> None:
        section, _ = self._make_section(root, expanded=False)
        fired: list[object] = []
        section.set_on_rows_built(fired.append)
        # No expand yet — callback must not have fired
        assert len(fired) == 0

    def test_on_rows_built_fires_once_only(self, root: tk.Tk) -> None:
        section, _ = self._make_section(root, expanded=False)
        fired: list[object] = []
        section.set_on_rows_built(fired.append)
        section.toggle()  # build + fire
        section.toggle()  # collapse
        section.toggle()  # re-expand — no rebuild, no extra fire
        assert len(fired) == 1

    def test_filter_rows_triggers_lazy_build(self, root: tk.Tk) -> None:
        """Searching through a collapsed section must trigger widget build."""
        from tkinter import ttk

        from regilattice.gui_widgets import TweakRow

        parent = ttk.Frame(root)
        parent.pack()
        # Create rows with distinctive labels for search matching
        rows = [
            TweakRow(parent, _make_td(tweak_id="search-foo", label="Foo Tweak"), corp_blocked=False, defer_widgets=True),
            TweakRow(parent, _make_td(tweak_id="search-bar", label="Bar Tweak"), corp_blocked=False, defer_widgets=True),
        ]
        from regilattice.gui_widgets import CategorySection

        section = CategorySection(parent, "Search Category", rows, expanded=False)
        assert section._widgets_built is False

        section.filter_rows("foo")
        # Build should have been triggered by the search filter
        assert section._widgets_built is True


# ── Helper ─────────────────────────────────────────────────────────────────


def _all_widgets(parent: tk.Misc) -> list[tk.Misc]:
    """Recursively collect all descendant widgets."""
    result: list[tk.Misc] = []
    for child in parent.winfo_children():
        result.append(child)
        result.extend(_all_widgets(child))
    return result


# ── TweakRow badge tests ────────────────────────────────────────────────────


@pytest.mark.skipif(not _tk_available, reason="tkinter not available")
class TestTweakRowBadges:
    """Test that optional badge labels appear based on TweakDef properties."""

    def _make_row(self, root: tk.Tk, **kwargs):  # type: ignore[no-untyped-def]
        from tkinter import ttk

        from regilattice.gui_widgets import TweakRow

        parent = ttk.Frame(root)
        parent.pack()
        td = _make_td(**kwargs)
        return TweakRow(parent, td, corp_blocked=False), parent

    def _label_texts(self, frame: tk.Misc) -> list[str]:
        return [w.cget("text") for w in _all_widgets(frame) if isinstance(w, tk.Label)]

    def test_admin_badge_shown(self, root: tk.Tk) -> None:
        row, _ = self._make_row(root, needs_admin=True)
        assert row.frame is not None
        texts = self._label_texts(row.frame)
        assert "ADMIN" in texts

    def test_admin_badge_absent(self, root: tk.Tk) -> None:
        row, _ = self._make_row(root, needs_admin=False)
        assert row.frame is not None
        texts = self._label_texts(row.frame)
        assert "ADMIN" not in texts

    def test_rec_badge_shown(self, root: tk.Tk) -> None:
        row, _ = self._make_row(root, description="Some tweak. Recommended: enable this.")
        assert row.frame is not None
        texts = self._label_texts(row.frame)
        assert any("REC" in t for t in texts)

    def test_rec_badge_absent(self, root: tk.Tk) -> None:
        row, _ = self._make_row(root, description="No recommendation here.")
        assert row.frame is not None
        texts = self._label_texts(row.frame)
        assert not any("REC" in t for t in texts)

    def test_gpo_badge_shown(self, root: tk.Tk) -> None:
        from tkinter import ttk
        from unittest.mock import patch

        from regilattice.gui_widgets import TweakRow

        parent = ttk.Frame(root)
        parent.pack()
        td = _make_td(registry_keys=[r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\test"])
        with patch("regilattice.gui_widgets.is_gpo_managed", return_value=True):
            row = TweakRow(parent, td, corp_blocked=False)
        assert row.frame is not None
        texts = self._label_texts(row.frame)
        assert "GPO" in texts

    def test_scope_user_badge(self, root: tk.Tk) -> None:
        row, _ = self._make_row(root, tweak_id="scope-user-test", registry_keys=[r"HKEY_CURRENT_USER\Software\test"])
        assert row.frame is not None
        texts = self._label_texts(row.frame)
        assert "USER" in texts

    def test_scope_machine_badge(self, root: tk.Tk) -> None:
        row, _ = self._make_row(root, tweak_id="scope-machine-test", registry_keys=[r"HKEY_LOCAL_MACHINE\SYSTEM\test"])
        assert row.frame is not None
        texts = self._label_texts(row.frame)
        assert "MACHINE" in texts


# ── TweakRow hover / apply_theme / refresh_status tests ────────────────────


@pytest.mark.skipif(not _tk_available, reason="tkinter not available")
class TestTweakRowInteraction:
    """Test hover effects, apply_theme, and refresh_status."""

    def _make_row(self, root: tk.Tk) -> TweakRow:
        from tkinter import ttk

        from regilattice.gui_widgets import TweakRow

        parent = ttk.Frame(root)
        parent.pack()
        return TweakRow(parent, _make_td(), corp_blocked=False)

    def test_on_enter_changes_bg(self, root: tk.Tk) -> None:
        from regilattice import gui_theme as theme

        row = self._make_row(root)
        assert row.frame is not None
        # Call _on_enter directly to avoid event-loop blocking
        row._on_enter(None)  # type: ignore[arg-type]
        # At least one Label should have bg == CARD_HOVER
        labels = [w for w in _all_widgets(row.frame) if isinstance(w, tk.Label)]
        assert any(w.cget("bg") == theme.CARD_HOVER for w in labels)

    def test_on_leave_restores_bg(self, root: tk.Tk) -> None:
        from regilattice import gui_theme as theme

        row = self._make_row(root)
        assert row.frame is not None
        row._on_enter(None)  # type: ignore[arg-type]
        row._on_leave(None)  # type: ignore[arg-type]
        # After leave, labels should not have CARD_HOVER bg (restored to row bg)
        labels = [w for w in _all_widgets(row.frame) if isinstance(w, tk.Label)]
        assert not any(w.cget("bg") == theme.CARD_HOVER for w in labels)

    def test_apply_theme_no_crash(self, root: tk.Tk) -> None:
        row = self._make_row(root)
        # Should not raise
        row.apply_theme()

    def test_refresh_status_applied(self, root: tk.Tk) -> None:
        from unittest.mock import patch

        from regilattice.tweaks import TweakResult

        row = self._make_row(root)
        with patch("regilattice.gui_widgets.tweak_status", return_value=TweakResult.APPLIED):
            row.refresh_status()
        assert row.status_text is not None
        assert row.status_text.cget("text") == "APPLIED"

    def test_refresh_status_default(self, root: tk.Tk) -> None:
        from unittest.mock import patch

        from regilattice.tweaks import TweakResult

        row = self._make_row(root)
        with patch("regilattice.gui_widgets.tweak_status", return_value=TweakResult.NOT_APPLIED):
            row.refresh_status()
        assert row.status_text is not None
        assert row.status_text.cget("text") == "DEFAULT"

    def test_refresh_status_unknown(self, root: tk.Tk) -> None:
        from unittest.mock import patch

        from regilattice.tweaks import TweakResult

        row = self._make_row(root)
        with patch("regilattice.gui_widgets.tweak_status", return_value=TweakResult.UNKNOWN):
            row.refresh_status()
        assert row.status_text is not None
        assert row.status_text.cget("text") == "UNKNOWN"

    def test_on_toggle_click_fires_callback(self, root: tk.Tk) -> None:
        from tkinter import ttk

        from regilattice.gui_widgets import TweakRow

        parent = ttk.Frame(root)
        fired: list[TweakRow] = []
        td = _make_td()
        row = TweakRow(parent, td, corp_blocked=False, on_toggle=fired.append)
        row.on_toggle_click()
        assert len(fired) == 1
        assert fired[0] is row

    def test_on_toggle_click_no_callback(self, root: tk.Tk) -> None:
        row = self._make_row(root)
        row.on_toggle_click()  # must not raise when no callback set


# ── CategorySection extra tests ─────────────────────────────────────────────


@pytest.mark.skipif(not _tk_available, reason="tkinter not available")
class TestCategorySectionExtra:
    """Extra CategorySection tests for coverage of callbacks, batch ops, filter_rows."""

    def _make_section(self, root: tk.Tk, *, expanded: bool = True, n: int = 2) -> tuple[CategorySection, list[TweakRow]]:
        from tkinter import ttk

        from regilattice.gui_widgets import CategorySection, TweakRow

        parent = ttk.Frame(root)
        parent.pack()
        rows = [TweakRow(parent, _make_td(tweak_id=f"ex-{i}", label=f"Tweak {i}"), corp_blocked=False, defer_widgets=True) for i in range(n)]
        section = CategorySection(parent, "Extra Category", rows, expanded=expanded)
        return section, rows

    def test_category_info_badges_rendered(self, root: tk.Tk) -> None:
        """CategoryInfo with risk/scope/profiles causes badge labels to be created."""
        from tkinter import ttk
        from unittest.mock import patch

        from regilattice.gui_widgets import CategorySection, TweakRow
        from regilattice.tweaks import CategoryInfo

        ci = CategoryInfo(name="Mocked", risk_level="high", scope="machine", profiles=["gaming", "business"])
        parent = ttk.Frame(root)
        parent.pack()
        rows = [TweakRow(parent, _make_td(tweak_id="ci-0"), corp_blocked=False, defer_widgets=True)]
        with patch("regilattice.gui_widgets.category_info", return_value=ci):
            section = CategorySection(parent, "Mocked", rows, expanded=False)

        # Header should contain risk / scope / profile badge labels
        texts = [w.cget("text") for w in _all_widgets(section.header) if isinstance(w, tk.Label)]
        assert "HIGH" in texts
        assert "MACHINE" in texts
        assert any("gaming" in t for t in texts)

    def test_on_collapse_change_fires_on_toggle(self, root: tk.Tk) -> None:
        section, _ = self._make_section(root, expanded=False)
        fired: list[CategorySection] = []
        section.set_on_collapse_change(fired.append)
        section.toggle()
        assert len(fired) == 1
        assert fired[0] is section
        section.toggle()
        assert len(fired) == 2

    def test_set_on_batch_wires_enable_all(self, root: tk.Tk) -> None:
        section, _ = self._make_section(root)
        calls: list[tuple[CategorySection, str]] = []
        section.set_on_batch(lambda s, a: calls.append((s, a)))
        section._btn_enable_all.invoke()
        assert calls == [(section, "apply")]

    def test_set_on_batch_wires_disable_all(self, root: tk.Tk) -> None:
        section, _ = self._make_section(root)
        calls: list[tuple[CategorySection, str]] = []
        section.set_on_batch(lambda s, a: calls.append((s, a)))
        section._btn_disable_all.invoke()
        assert calls == [(section, "remove")]

    def test_set_on_reorder_wires_up(self, root: tk.Tk) -> None:
        section, _ = self._make_section(root)
        calls: list[tuple[CategorySection, str]] = []
        section.set_on_reorder(lambda s, d: calls.append((s, d)))
        section._btn_up.invoke()
        assert calls == [(section, "up")]

    def test_set_on_reorder_wires_down(self, root: tk.Tk) -> None:
        section, _ = self._make_section(root)
        calls: list[tuple[CategorySection, str]] = []
        section.set_on_reorder(lambda s, d: calls.append((s, d)))
        section._btn_down.invoke()
        assert calls == [(section, "down")]

    def test_update_count_with_statuses(self, root: tk.Tk) -> None:
        section, rows = self._make_section(root)
        from regilattice.tweaks import TweakResult

        statuses = {rows[0].td.id: TweakResult.APPLIED, rows[1].td.id: TweakResult.NOT_APPLIED}
        section.update_count(statuses=statuses)
        assert "1/2" in section._count_lbl.cget("text")

    def test_update_count_without_statuses(self, root: tk.Tk) -> None:
        from unittest.mock import patch

        from regilattice.tweaks import TweakResult

        section, _ = self._make_section(root)
        with patch("regilattice.gui_widgets.tweak_status", return_value=TweakResult.NOT_APPLIED):
            section.update_count()
        assert "0/2" in section._count_lbl.cget("text")

    def test_filter_rows_hides_section_when_no_match(self, root: tk.Tk) -> None:
        section, _ = self._make_section(root)
        visible = section.filter_rows("xyznotfound")
        assert visible is False
        # header should be gone from view
        assert section.header.winfo_manager() == ""

    def test_filter_rows_empty_query_shows_all(self, root: tk.Tk) -> None:
        section, _ = self._make_section(root)
        visible = section.filter_rows("")
        assert visible is True

    def test_apply_theme_category_section(self, root: tk.Tk) -> None:
        section, _ = self._make_section(root)
        # Should not raise
        section.apply_theme()
