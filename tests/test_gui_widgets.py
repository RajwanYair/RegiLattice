"""Tests for regilattice.gui_widgets — TweakRow logic (deferred, no tkinter window needed)."""

from __future__ import annotations

import pytest

from regilattice.tweaks import TweakDef


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
        assert row.frame.winfo_manager() == "pack"
        row.unpack_row()
        assert row.frame.winfo_manager() == ""
