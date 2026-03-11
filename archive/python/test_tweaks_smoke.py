"""Smoke tests for ALL tweak plugin modules.

Parametrized by tweak id so failures pinpoint the exact tweak.
"""

from __future__ import annotations

import inspect

import pytest

from regilattice.tweaks import TweakDef, all_tweaks

# ── Collect all tweak ids once at module level ───────────────────────────────

_ALL = all_tweaks()
_IDS = [td.id for td in _ALL]


def _get_tweak(tweak_id: str) -> TweakDef:
    for td in _ALL:
        if td.id == tweak_id:
            return td
    raise KeyError(tweak_id)


# ── Global assertions ───────────────────────────────────────────────────────


class TestGlobalConstraints:
    """Project-wide invariants across all tweak modules."""

    def test_at_least_200_tweaks(self) -> None:
        assert len(_ALL) >= 200, f"Expected >=200 tweaks, got {len(_ALL)}"

    def test_no_duplicate_ids(self) -> None:
        seen: set[str] = set()
        dupes: list[str] = []
        for tid in _IDS:
            if tid in seen:
                dupes.append(tid)
            seen.add(tid)
        assert not dupes, f"Duplicate tweak IDs: {dupes}"


# ── Per-tweak parametrized checks ───────────────────────────────────────────


@pytest.mark.parametrize("tweak_id", _IDS)
class TestTweakContract:
    """Validate every TweakDef satisfies the expected contract."""

    def test_id_is_non_empty_string(self, tweak_id: str) -> None:
        td = _get_tweak(tweak_id)
        assert isinstance(td.id, str) and td.id != ""

    def test_label_is_non_empty(self, tweak_id: str) -> None:
        td = _get_tweak(tweak_id)
        assert isinstance(td.label, str) and td.label != ""

    def test_category_is_non_empty(self, tweak_id: str) -> None:
        td = _get_tweak(tweak_id)
        assert isinstance(td.category, str) and td.category != ""

    def test_apply_fn_is_callable(self, tweak_id: str) -> None:
        td = _get_tweak(tweak_id)
        assert callable(td.apply_fn)

    def test_remove_fn_is_callable(self, tweak_id: str) -> None:
        td = _get_tweak(tweak_id)
        assert callable(td.remove_fn)

    def test_detect_fn_is_callable_or_none(self, tweak_id: str) -> None:
        td = _get_tweak(tweak_id)
        assert td.detect_fn is None or callable(td.detect_fn)

    def test_registry_keys_is_list(self, tweak_id: str) -> None:
        td = _get_tweak(tweak_id)
        assert isinstance(td.registry_keys, list)

    def test_tags_is_list(self, tweak_id: str) -> None:
        td = _get_tweak(tweak_id)
        assert isinstance(td.tags, list)

    def test_depends_on_is_list(self, tweak_id: str) -> None:
        td = _get_tweak(tweak_id)
        assert isinstance(td.depends_on, list)

    def test_needs_admin_is_bool(self, tweak_id: str) -> None:
        td = _get_tweak(tweak_id)
        assert isinstance(td.needs_admin, bool)

    def test_corp_safe_is_bool(self, tweak_id: str) -> None:
        td = _get_tweak(tweak_id)
        assert isinstance(td.corp_safe, bool)

    def test_apply_fn_accepts_require_admin(self, tweak_id: str) -> None:
        """Ensure apply_fn accepts a keyword-only ``require_admin`` parameter."""
        td = _get_tweak(tweak_id)
        sig = inspect.signature(td.apply_fn)
        param = sig.parameters.get("require_admin")
        assert param is not None, f"{td.id} apply_fn missing require_admin kwarg"
        assert param.kind == inspect.Parameter.KEYWORD_ONLY

    def test_remove_fn_accepts_require_admin(self, tweak_id: str) -> None:
        """Ensure remove_fn accepts a keyword-only ``require_admin`` parameter."""
        td = _get_tweak(tweak_id)
        sig = inspect.signature(td.remove_fn)
        param = sig.parameters.get("require_admin")
        assert param is not None, f"{td.id} remove_fn missing require_admin kwarg"
        assert param.kind == inspect.Parameter.KEYWORD_ONLY
