"""Tests for regilattice.gui_widgets — TweakRow logic (deferred, no tkinter window needed)."""

from __future__ import annotations

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

    # Attempt to create a hidden root — may fail in headless CI
    _test_root = tk.Tk()
    _test_root.withdraw()
except Exception:
    _tk_available = False
    _test_root = None  # type: ignore[assignment]


@pytest.fixture()
def root() -> tk.Tk:
    if not _tk_available:
        pytest.skip("tkinter not available or headless environment")
    assert _test_root is not None
    return _test_root


class TestTweakRowDeferred:
    """Test TweakRow creation with defer_widgets=True (no tkinter needed)."""

    def test_deferred_row_no_widgets(self) -> None:
        from tkinter import ttk

        from regilattice.gui_widgets import TweakRow

        if not _tk_available:
            pytest.skip("tkinter not available")
        parent = ttk.Frame(_test_root)
        td = _make_td()
        row = TweakRow(parent, td, corp_blocked=False, defer_widgets=True)
        assert row.td is td
        assert row.frame is None
        assert row.var.get() is False
        assert row.disabled_by_corp is False

    def test_deferred_corp_blocked(self) -> None:
        from tkinter import ttk

        from regilattice.gui_widgets import TweakRow

        if not _tk_available:
            pytest.skip("tkinter not available")
        parent = ttk.Frame(_test_root)
        td = _make_td(corp_safe=False)
        row = TweakRow(parent, td, corp_blocked=True, defer_widgets=True)
        assert row.disabled_by_corp is True

    def test_corp_safe_not_blocked(self) -> None:
        from tkinter import ttk

        from regilattice.gui_widgets import TweakRow

        if not _tk_available:
            pytest.skip("tkinter not available")
        parent = ttk.Frame(_test_root)
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
        for row, frm in zip(rows, first_frames):
            # Same frame object — was not recreated
            assert row.frame is frm

    def test_on_rows_built_callback_fires(self, root: tk.Tk) -> None:
        from regilattice.gui_widgets import CategorySection

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
