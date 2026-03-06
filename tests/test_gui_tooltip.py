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
