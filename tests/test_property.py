"""Property-based tests for TweakDef validation edge cases using Hypothesis.

Exercises:
- TweakDef field constraints (ID format, category non-empty, etc.)
- Search invariants (query subset of label/tags always matches)
- Snapshot round-trip (save → load preserves data)
- Category grouping (every tweak appears in exactly one category)
"""

from __future__ import annotations

import json
import string

import hypothesis.strategies as st
from hypothesis import HealthCheck, given, settings

from regilattice.tweaks import (
    TweakDef,
    all_tweaks,
    categories,
    get_tweak,
    search_tweaks,
    tweaks_by_category,
)

# ── Strategies ───────────────────────────────────────────────────────────────

_kebab = st.text(
    alphabet=string.ascii_lowercase + string.digits + "-",
    min_size=3,
    max_size=30,
).filter(lambda s: s[0].isalpha() and not s.endswith("-"))
_label = st.text(
    alphabet=string.ascii_letters + string.digits + " -_",
    min_size=1,
    max_size=60,
)
_category = st.text(
    alphabet=string.ascii_letters + string.digits + " &-",
    min_size=1,
    max_size=40,
)
_tag = st.text(alphabet=string.ascii_lowercase + "-", min_size=1, max_size=20)
_reg_key = st.sampled_from([
    r"HKEY_LOCAL_MACHINE\SOFTWARE\TestKey",
    r"HKEY_CURRENT_USER\SOFTWARE\TestKey",
    r"HKEY_LOCAL_MACHINE\SYSTEM\TestKey",
    r"HKEY_CURRENT_USER\Control Panel\TestKey",
])

_tweak_def = st.builds(
    TweakDef,
    id=_kebab,
    label=_label,
    category=_category,
    apply_fn=st.just(lambda **_kw: None),
    remove_fn=st.just(lambda **_kw: None),
    detect_fn=st.just(None),
    needs_admin=st.booleans(),
    corp_safe=st.booleans(),
    registry_keys=st.lists(_reg_key, max_size=4),
    description=st.text(max_size=200),
    tags=st.lists(_tag, max_size=5),
)


# ── TweakDef construction ───────────────────────────────────────────────────


class TestTweakDefConstruction:
    """Property: any TweakDef built from valid components should be a valid object."""

    @given(td=_tweak_def)
    @settings(max_examples=100, suppress_health_check=[HealthCheck.too_slow])
    def test_tweakdef_round_trip(self, td: TweakDef) -> None:
        assert td.id
        assert td.label
        assert td.category
        assert isinstance(td.needs_admin, bool)
        assert isinstance(td.corp_safe, bool)
        assert isinstance(td.registry_keys, list)
        assert isinstance(td.tags, list)


# ── Search always finds tweaks by exact label or tag ─────────────────────────


class TestSearchInvariants:
    """Property: searching by a tweak's own label or tag should return it."""

    def test_search_by_label_substring(self) -> None:
        tweaks = all_tweaks()
        for td in tweaks[:20]:  # spot-check first 20
            if len(td.label) < 3:
                continue
            substr = td.label[:3]
            results = search_tweaks(substr)
            ids = {r.id for r in results}
            assert td.id in ids, f"Search for {substr!r} didn't find {td.id}"

    def test_search_by_tag(self) -> None:
        tweaks = all_tweaks()
        for td in tweaks[:20]:
            if not td.tags:
                continue
            tag = td.tags[0]
            results = search_tweaks(tag)
            ids = {r.id for r in results}
            assert td.id in ids, f"Search for tag {tag!r} didn't find {td.id}"


# ── Every tweak has exactly one category ─────────────────────────────────────


class TestCategoryGrouping:
    """Property: every tweak appears in exactly one category."""

    def test_all_tweaks_in_exactly_one_category(self) -> None:
        grouped = tweaks_by_category()
        seen: dict[str, str] = {}
        for cat_name, cat_tweaks in grouped.items():
            for td in cat_tweaks:
                assert td.id not in seen, f"{td.id} in both {seen[td.id]!r} and {cat_name!r}"
                seen[td.id] = cat_name
        # All tweaks are accounted for
        all_ids = {td.id for td in all_tweaks()}
        assert set(seen.keys()) == all_ids

    def test_categories_non_empty(self) -> None:
        for cat in categories():
            grouped = tweaks_by_category()
            assert len(grouped.get(cat, [])) > 0, f"Category {cat!r} has no tweaks"


# ── get_tweak round-trip ─────────────────────────────────────────────────────


class TestGetTweakRoundTrip:
    """Property: get_tweak(td.id) returns the same TweakDef."""

    def test_get_tweak_identity(self) -> None:
        for td in all_tweaks():
            found = get_tweak(td.id)
            assert found is not None, f"get_tweak({td.id!r}) returned None"
            assert found.id == td.id
            assert found.label == td.label
            assert found.category == td.category


# ── Snapshot round-trip ──────────────────────────────────────────────────────


class TestSnapshotRoundTrip:
    """Property: serialized snapshot data round-trips through JSON."""

    @given(
        ids=st.lists(_kebab, min_size=1, max_size=20),
        statuses=st.lists(
            st.sampled_from(["applied", "default", "unknown"]),
            min_size=1,
            max_size=20,
        ),
    )
    @settings(max_examples=50)
    def test_snapshot_json_round_trip(self, ids: list[str], statuses: list[str]) -> None:
        """A snapshotDict[str, str] survives JSON serialization."""
        n = min(len(ids), len(statuses))
        snapshot = dict(zip(ids[:n], statuses[:n], strict=False))
        serialized = json.dumps(snapshot)
        deserialized = json.loads(serialized)
        assert deserialized == snapshot


# ── ID format validation ────────────────────────────────────────────────────


class TestIDFormat:
    """Property: all real tweak IDs match the expected kebab-case pattern."""

    def test_ids_are_kebab(self) -> None:
        import re
        pattern = re.compile(r"^[a-z][a-z0-9\-]+$")
        for td in all_tweaks():
            assert pattern.match(td.id), f"ID {td.id!r} is not kebab-case"

    def test_ids_globally_unique(self) -> None:
        ids = [td.id for td in all_tweaks()]
        assert len(ids) == len(set(ids)), "Duplicate IDs found"
