"""Tests for regilattice.gui_tooltip — description parser, recommendation detector, tooltip text builder."""

from __future__ import annotations

from regilattice.gui_tooltip import build_tooltip_text, has_recommendation, parse_description_metadata
from regilattice.tweaks import TweakDef, TweakResult

# ── Helper to build a minimal TweakDef ───────────────────────────────────────


def _make_td(
    *,
    desc: str = "",
    tags: list[str] | None = None,
    registry_keys: list[str] | None = None,
    needs_admin: bool = False,
    corp_safe: bool = True,
) -> TweakDef:
    return TweakDef(
        id="test-tweak",
        label="Test Tweak",
        category="Test",
        apply_fn=lambda: None,
        remove_fn=lambda: None,
        detect_fn=None,
        needs_admin=needs_admin,
        corp_safe=corp_safe,
        registry_keys=registry_keys or [],
        description=desc,
        tags=tags or [],
    )


class TestParseDescriptionMetadata:
    def test_empty_description(self) -> None:
        main, default, rec, options = parse_description_metadata("")
        assert main == ""
        assert default == ""
        assert rec == ""
        assert options == ""

    def test_plain_text_only(self) -> None:
        main, default, rec, options = parse_description_metadata("Disables Windows telemetry.")
        assert "telemetry" in main.lower()
        assert default == ""
        assert rec == ""
        assert options == ""

    def test_default_hint(self) -> None:
        _, default, _, _ = parse_description_metadata("Some text. Default: Enabled")
        assert default == "Default: Enabled"

    def test_recommended_hint(self) -> None:
        _, _, rec, _ = parse_description_metadata("Some text. Recommended: Disable for privacy")
        assert rec == "Recommended: Disable for privacy"

    def test_options_hint(self) -> None:
        _, _, _, options = parse_description_metadata("Some text. Options: 0=Off, 1=On")
        assert "0=Off" in options

    def test_values_hint(self) -> None:
        _, _, _, options = parse_description_metadata("Some text. Values: 0-100")
        assert "0-100" in options

    def test_all_metadata(self) -> None:
        desc = "Main description. Default: On. Recommended: Off for privacy. Options: 0 or 1"
        main, default, rec, options = parse_description_metadata(desc)
        assert "Main description" in main
        assert "Default:" in default
        assert "Recommended:" in rec
        assert "Options:" in options

    def test_caching_returns_same(self) -> None:
        desc = "Cached test. Default: Yes"
        r1 = parse_description_metadata(desc)
        r2 = parse_description_metadata(desc)
        assert r1 == r2


class TestHasRecommendation:
    def test_with_recommendation(self) -> None:
        td = _make_td(desc="Some tweak. Recommended: Disable")
        assert has_recommendation(td) is True

    def test_without_recommendation(self) -> None:
        td = _make_td(desc="Some tweak without any hint.")
        assert has_recommendation(td) is False

    def test_empty_description(self) -> None:
        td = _make_td(desc="")
        assert has_recommendation(td) is False


class TestBuildTooltipText:
    def test_includes_status_applied(self) -> None:
        td = _make_td(desc="My tweak description")
        text = build_tooltip_text(td, TweakResult.APPLIED)
        assert "APPLIED" in text

    def test_includes_status_default(self) -> None:
        td = _make_td(desc="My tweak description")
        text = build_tooltip_text(td, TweakResult.NOT_APPLIED)
        assert "DEFAULT" in text

    def test_includes_status_unknown(self) -> None:
        td = _make_td(desc="My tweak description")
        text = build_tooltip_text(td, TweakResult.UNKNOWN)
        assert "UNKNOWN" in text

    def test_includes_admin_warning(self) -> None:
        td = _make_td(needs_admin=True)
        text = build_tooltip_text(td, TweakResult.UNKNOWN)
        assert "admin" in text.lower()

    def test_corp_safe_shown(self) -> None:
        td = _make_td(corp_safe=True)
        text = build_tooltip_text(td, TweakResult.UNKNOWN)
        assert "corporate" in text.lower()

    def test_not_corp_safe_shown(self) -> None:
        td = _make_td(corp_safe=False)
        text = build_tooltip_text(td, TweakResult.UNKNOWN)
        assert "corporate" in text.lower()

    def test_tags_shown(self) -> None:
        td = _make_td(tags=["privacy", "telemetry"])
        text = build_tooltip_text(td, TweakResult.UNKNOWN)
        assert "privacy" in text
        assert "telemetry" in text

    def test_registry_keys_shown(self) -> None:
        td = _make_td(registry_keys=[r"HKEY_LOCAL_MACHINE\SOFTWARE\Test"])
        text = build_tooltip_text(td, TweakResult.UNKNOWN)
        assert "Registry:" in text

    def test_multiple_registry_keys(self) -> None:
        td = _make_td(registry_keys=[r"HKEY_LOCAL_MACHINE\A", r"HKEY_LOCAL_MACHINE\B"])
        text = build_tooltip_text(td, TweakResult.UNKNOWN)
        assert "+1 more" in text

    def test_recommendation_shown(self) -> None:
        td = _make_td(desc="Tweak. Recommended: Enable")
        text = build_tooltip_text(td, TweakResult.UNKNOWN)
        assert "Recommended" in text

    def test_default_shown(self) -> None:
        td = _make_td(desc="Tweak. Default: Enabled")
        text = build_tooltip_text(td, TweakResult.UNKNOWN)
        assert "Default:" in text


# ── TooltipManager singleton tests ──────────────────────────────────────────

# Try to import Tk — skip GUI tests if unavailable
_tk_available = True
try:
    import tkinter as tk

    _test_root: tk.Tk | None = tk.Tk()
    assert _test_root is not None
    _test_root.withdraw()
except Exception:
    _tk_available = False
    _test_root = None


import pytest


@pytest.fixture()
def root() -> tk.Tk:
    if not _tk_available:
        pytest.skip("tkinter not available or headless environment")
    assert _test_root is not None
    return _test_root


@pytest.mark.skipif(not _tk_available, reason="tkinter not available")
class TestTooltipManager:
    """Tests for the shared TooltipManager singleton."""

    def setup_method(self) -> None:
        from regilattice.gui_tooltip import TooltipManager

        TooltipManager.reset()

    def teardown_method(self) -> None:
        from regilattice.gui_tooltip import TooltipManager

        TooltipManager.reset()

    def test_get_returns_none_before_init(self) -> None:
        from regilattice.gui_tooltip import TooltipManager

        assert TooltipManager.get() is None

    def test_init_creates_singleton(self, root: tk.Tk) -> None:
        from regilattice.gui_tooltip import TooltipManager

        mgr = TooltipManager.init(root)
        assert mgr is not None
        assert TooltipManager.get() is mgr

    def test_init_is_idempotent(self, root: tk.Tk) -> None:
        from regilattice.gui_tooltip import TooltipManager

        mgr1 = TooltipManager.init(root)
        mgr2 = TooltipManager.init(root)
        assert mgr1 is mgr2

    def test_reset_clears_singleton(self, root: tk.Tk) -> None:
        from regilattice.gui_tooltip import TooltipManager

        TooltipManager.init(root)
        assert TooltipManager.get() is not None
        TooltipManager.reset()
        assert TooltipManager.get() is None

    def test_panel_created_lazily_on_show(self, root: tk.Tk) -> None:
        from regilattice.gui_tooltip import TooltipManager

        mgr = TooltipManager.init(root)
        # No panel yet — _tip should be None before first show()
        assert mgr._tip is None
        mgr.show("Hello", 100, 100)
        # Panel now created
        assert mgr._tip is not None

    def test_hide_withdraws_not_destroys(self, root: tk.Tk) -> None:
        from regilattice.gui_tooltip import TooltipManager

        mgr = TooltipManager.init(root)
        mgr.show("Test", 50, 50)
        tip = mgr._tip
        mgr.hide()
        # Toplevel should still exist (just withdrawn)
        assert mgr._tip is tip
        assert mgr._visible is False

    def test_show_updates_label_text(self, root: tk.Tk) -> None:
        from regilattice.gui_tooltip import TooltipManager

        mgr = TooltipManager.init(root)
        mgr.show("First text", 0, 0)
        assert mgr._label is not None
        assert mgr._label.cget("text") == "First text"
        mgr.show("Second text", 0, 0)
        assert mgr._label.cget("text") == "Second text"

    def test_move_repositions_panel(self, root: tk.Tk) -> None:
        from regilattice.gui_tooltip import TooltipManager

        mgr = TooltipManager.init(root)
        mgr.show("Visible", 0, 0)
        # move should not raise and panel should still exist
        mgr.move(200, 300)
        assert mgr._tip is not None

    def test_hide_before_show_does_not_raise(self, root: tk.Tk) -> None:
        from regilattice.gui_tooltip import TooltipManager

        mgr = TooltipManager.init(root)
        # hide() before any show() must not raise
        mgr.hide()
        assert mgr._visible is False

    def test_tooltip_proxy_uses_manager(self, root: tk.Tk) -> None:
        """Tooltip._show routes to the singleton manager when available."""
        import tkinter as tk

        from regilattice.gui_tooltip import Tooltip, TooltipManager

        mgr = TooltipManager.init(root)
        lbl = tk.Label(root, text="w")
        lbl.pack()
        tt = Tooltip(lbl, text="proxy test")
        # Simulate <Enter> event
        event = type("E", (), {"x_root": 10, "y_root": 10})()
        tt._show(event)
        assert mgr._visible is True
        assert mgr._label is not None
        assert mgr._label.cget("text") == "proxy test"

    def test_tooltip_fallback_without_manager(self, root: tk.Tk) -> None:
        """Tooltip falls back to its own Toplevel when manager is not initialised."""
        import tkinter as tk

        from regilattice.gui_tooltip import Tooltip, TooltipManager

        # Ensure manager is cleared
        TooltipManager.reset()
        lbl = tk.Label(root, text="w2")
        lbl.pack()
        tt = Tooltip(lbl, text="fallback")
        event = type("E", (), {"x_root": 5, "y_root": 5})()
        tt._show(event)
        # Should have created its own Toplevel
        assert tt._tip is not None
        # Clean up
        tt._hide(event)
        assert tt._tip is None
