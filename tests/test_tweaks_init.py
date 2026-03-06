"""Tests for regilattice.tweaks.__init__ — plugin loader, search, status, snapshots."""
# mypy: disable-error-code="type-arg,no-untyped-def"
# pyright: reportMissingTypeArgument=false

from __future__ import annotations

import json
from pathlib import Path
from unittest.mock import MagicMock, patch

import pytest

from regilattice.tweaks import (
    TweakDef,
    TweakResult,
    _topo_sort,
    all_tweaks,
    apply_all,
    categories,
    get_tweak,
    load_snapshot,
    reload_plugins,
    remove_all,
    restore_snapshot,
    save_snapshot,
    search_tweaks,
    status_map,
    tweak_status,
    tweaks_by_category,
)

# ── TweakDef dataclass ──────────────────────────────────────────────────────


class TestTweakDef:
    """Validate the TweakDef dataclass contract."""

    def test_required_fields(self) -> None:
        td = TweakDef(
            id="test.foo",
            label="Foo",
            category="Testing",
            apply_fn=lambda: None,
            remove_fn=lambda: None,
        )
        assert td.id == "test.foo"
        assert td.label == "Foo"
        assert td.category == "Testing"

    def test_defaults(self) -> None:
        td = TweakDef(
            id="t",
            label="L",
            category="C",
            apply_fn=lambda: None,
            remove_fn=lambda: None,
        )
        assert td.detect_fn is None
        assert td.needs_admin is True
        assert td.corp_safe is False
        assert td.registry_keys == []
        assert td.description == ""
        assert td.tags == []

    def test_repr_contains_id(self) -> None:
        td = TweakDef(
            id="repr.test",
            label="R",
            category="C",
            apply_fn=lambda: None,
            remove_fn=lambda: None,
        )
        assert "repr.test" in repr(td)

    def test_custom_fields(self) -> None:
        def det() -> bool:
            return True

        td = TweakDef(
            id="c",
            label="Custom",
            category="Cat",
            apply_fn=lambda: None,
            remove_fn=lambda: None,
            detect_fn=det,
            needs_admin=False,
            corp_safe=True,
            registry_keys=["HKCU\\Test"],
            description="desc",
            tags=["tag1"],
        )
        assert td.detect_fn is det
        assert td.needs_admin is False
        assert td.corp_safe is True
        assert td.registry_keys == ["HKCU\\Test"]
        assert td.description == "desc"
        assert td.tags == ["tag1"]


# ── Plugin loader ────────────────────────────────────────────────────────────


class TestPluginLoader:
    """Test that the plugin loader discovers all TweakDef instances."""

    def test_all_tweaks_non_empty(self, all_tweaks_list: list[TweakDef]) -> None:
        assert len(all_tweaks_list) > 0

    def test_no_duplicate_ids(self, all_tweaks_list: list[TweakDef]) -> None:
        ids = [t.id for t in all_tweaks_list]
        assert len(ids) == len(set(ids)), f"Duplicate ids found: {[x for x in ids if ids.count(x) > 1]}"

    def test_all_have_required_fields(self, all_tweaks_list: list[TweakDef]) -> None:
        for td in all_tweaks_list:
            assert isinstance(td.id, str) and td.id, f"{td!r}: id must be non-empty str"
            assert isinstance(td.label, str) and td.label, f"{td!r}: label must be non-empty str"
            assert isinstance(td.category, str) and td.category, f"{td!r}: category must be non-empty str"

    def test_apply_fn_callable(self, all_tweaks_list: list[TweakDef]) -> None:
        for td in all_tweaks_list:
            assert callable(td.apply_fn), f"{td.id}: apply_fn not callable"

    def test_remove_fn_callable(self, all_tweaks_list: list[TweakDef]) -> None:
        for td in all_tweaks_list:
            assert callable(td.remove_fn), f"{td.id}: remove_fn not callable"

    def test_detect_fn_callable_or_none(self, all_tweaks_list: list[TweakDef]) -> None:
        for td in all_tweaks_list:
            assert td.detect_fn is None or callable(td.detect_fn), f"{td.id}: detect_fn must be callable or None"


# ── Categories ───────────────────────────────────────────────────────────────


class TestCategories:
    """Test category helper functions."""

    def test_returns_list_of_strings(self) -> None:
        cats = categories()
        assert isinstance(cats, list)
        assert all(isinstance(c, str) for c in cats)

    def test_all_categories_non_empty(self) -> None:
        for c in categories():
            assert c, "Category must not be empty string"

    def test_tweaks_by_category_groups_match(self) -> None:
        grouped = tweaks_by_category()
        cats = categories()
        assert set(grouped.keys()) == set(cats)

    def test_every_tweak_appears_in_group(self, all_tweaks_list: list[TweakDef]) -> None:
        grouped = tweaks_by_category()
        grouped_ids = {td.id for tds in grouped.values() for td in tds}
        all_ids = {td.id for td in all_tweaks_list}
        assert grouped_ids == all_ids

    def test_tweaks_by_category_values_non_empty(self) -> None:
        for cat, tds in tweaks_by_category().items():
            assert len(tds) > 0, f"Category {cat!r} has no tweaks"


# ── Search ───────────────────────────────────────────────────────────────────


class TestSearchTweaks:
    """Test search_tweaks() filtering."""

    def test_search_by_id(self, all_tweaks_list: list[TweakDef]) -> None:
        td = all_tweaks_list[0]
        results = search_tweaks(td.id)
        assert any(r.id == td.id for r in results)

    def test_search_by_label(self, all_tweaks_list: list[TweakDef]) -> None:
        td = all_tweaks_list[0]
        # Use a word from the label
        word = td.label.split()[0]
        results = search_tweaks(word)
        assert len(results) >= 1

    def test_search_by_category(self) -> None:
        cat = categories()[0]
        results = search_tweaks(cat)
        assert all(
            cat.lower() in r.category.lower()
            or cat.lower() in r.id.lower()
            or cat.lower() in r.label.lower()
            or cat.lower() in r.description.lower()
            or any(cat.lower() in t.lower() for t in r.tags)
            for r in results
        )

    def test_search_by_tag(self, all_tweaks_list: list[TweakDef]) -> None:
        # Find a tweak with tags
        tagged = [t for t in all_tweaks_list if t.tags]
        if not tagged:
            pytest.skip("No tweaks with tags found")
        td = tagged[0]
        tag = td.tags[0]
        results = search_tweaks(tag)
        assert any(r.id == td.id for r in results)

    def test_empty_query_returns_all(self, all_tweaks_list: list[TweakDef]) -> None:
        # Empty string is "in" every string, so everything matches
        results = search_tweaks("")
        assert len(results) == len(all_tweaks_list)

    def test_no_results(self) -> None:
        results = search_tweaks("zzz_nonexistent_xxyyzz_99999")
        assert results == []


# ── get_tweak ────────────────────────────────────────────────────────────────


class TestGetTweak:
    """Test get_tweak() lookup."""

    def test_existing_id(self, all_tweaks_list: list[TweakDef]) -> None:
        td = all_tweaks_list[0]
        found = get_tweak(td.id)
        assert found is not None
        assert found.id == td.id

    def test_nonexistent_id(self) -> None:
        assert get_tweak("nonexistent.fake.id.999") is None


# ── Tweak status ─────────────────────────────────────────────────────────────


class TestTweakStatus:
    """Test tweak_status() with mocked detect_fn."""

    @staticmethod
    def _make_tweak(detect_fn=None) -> TweakDef:
        return TweakDef(
            id="test.status",
            label="S",
            category="C",
            apply_fn=lambda: None,
            remove_fn=lambda: None,
            detect_fn=detect_fn,
        )

    def test_detect_returns_true(self) -> None:
        td = self._make_tweak(detect_fn=lambda: True)
        assert tweak_status(td) == TweakResult.APPLIED

    def test_detect_returns_false(self) -> None:
        td = self._make_tweak(detect_fn=lambda: False)
        assert tweak_status(td) == TweakResult.NOT_APPLIED

    def test_detect_raises(self) -> None:
        def bad():
            raise RuntimeError("boom")

        td = self._make_tweak(detect_fn=bad)
        assert tweak_status(td) == TweakResult.UNKNOWN

    def test_detect_is_none(self) -> None:
        td = self._make_tweak(detect_fn=None)
        assert tweak_status(td) == TweakResult.UNKNOWN


# ── status_map ───────────────────────────────────────────────────────────────


class TestStatusMap:
    """Test status_map() returns dict with all tweak ids."""

    @patch("regilattice.tweaks.tweak_status", return_value=TweakResult.UNKNOWN)
    def test_returns_dict(self, _mock: MagicMock) -> None:
        sm = status_map()
        assert isinstance(sm, dict)

    @patch("regilattice.tweaks.tweak_status", return_value=TweakResult.UNKNOWN)
    def test_all_ids_present(self, _mock: MagicMock, all_tweaks_list: list[TweakDef]) -> None:
        sm = status_map()
        for td in all_tweaks_list:
            assert td.id in sm

    @patch("regilattice.tweaks.tweak_status", return_value=TweakResult.APPLIED)
    def test_values_are_valid_statuses(self, _mock: MagicMock) -> None:
        valid = {TweakResult.APPLIED, TweakResult.NOT_APPLIED, TweakResult.UNKNOWN}
        for status in status_map().values():
            assert status in valid


# ── Snapshot ─────────────────────────────────────────────────────────────────


class TestSnapshot:
    """Test save/load snapshot round-trip."""

    @patch("regilattice.tweaks.status_map", return_value={"fake.id": TweakResult.UNKNOWN})
    def test_save_creates_file(self, _mock: MagicMock, tmp_path: Path) -> None:
        path = tmp_path / "snap.json"
        save_snapshot(path)
        assert path.exists()

    @patch("regilattice.tweaks.status_map", return_value={"fake.id": TweakResult.APPLIED})
    def test_load_reads_snapshot(self, _mock: MagicMock, tmp_path: Path) -> None:
        path = tmp_path / "snap.json"
        save_snapshot(path)
        data = load_snapshot(path)
        assert isinstance(data, dict)

    @patch("regilattice.tweaks.status_map", return_value={"a": TweakResult.APPLIED, "b": TweakResult.NOT_APPLIED})
    def test_round_trip_consistency(self, mock_sm: MagicMock, tmp_path: Path) -> None:
        path = tmp_path / "snap.json"
        save_snapshot(path)
        loaded = load_snapshot(path)
        assert loaded == {"a": "applied", "b": "not applied"}

    @patch("regilattice.tweaks.status_map", return_value={"x": TweakResult.UNKNOWN})
    def test_save_creates_parent_dirs(self, _mock: MagicMock, tmp_path: Path) -> None:
        path = tmp_path / "deep" / "nested" / "snap.json"
        save_snapshot(path)
        assert path.exists()

    @patch("regilattice.tweaks.status_map", return_value={"y": TweakResult.APPLIED})
    def test_snapshot_is_valid_json(self, _mock: MagicMock, tmp_path: Path) -> None:
        path = tmp_path / "snap.json"
        save_snapshot(path)
        data = json.loads(path.read_text(encoding="utf-8"))
        assert isinstance(data, dict)


# ── restore_snapshot ─────────────────────────────────────────────────────────


class TestRestoreSnapshot:
    """Test restore_snapshot with mocked tweaks."""

    def _make_snapshot(self, tmp_path: Path, data: dict[str, str]) -> Path:
        path = tmp_path / "restore.json"
        path.write_text(json.dumps(data), encoding="utf-8")
        return path

    @patch("regilattice.tweaks._ALL_TWEAKS", new_callable=list)
    @patch("regilattice.tweaks.tweak_status")
    def test_applies_missing_tweak(
        self,
        mock_status: MagicMock,
        mock_all: list,
        tmp_path: Path,
    ) -> None:
        apply_fn = MagicMock()
        remove_fn = MagicMock()
        td = TweakDef(
            id="t1",
            label="T",
            category="C",
            apply_fn=apply_fn,
            remove_fn=remove_fn,
            corp_safe=True,
        )
        mock_all.append(td)
        mock_status.return_value = TweakResult.NOT_APPLIED

        path = self._make_snapshot(tmp_path, {"t1": "applied"})
        result = restore_snapshot(path, force_corp=True)
        apply_fn.assert_called_once()
        assert result["t1"] == TweakResult.APPLIED

    @patch("regilattice.tweaks._ALL_TWEAKS", new_callable=list)
    @patch("regilattice.tweaks.tweak_status")
    def test_removes_applied_tweak(
        self,
        mock_status: MagicMock,
        mock_all: list,
        tmp_path: Path,
    ) -> None:
        apply_fn = MagicMock()
        remove_fn = MagicMock()
        td = TweakDef(
            id="t1",
            label="T",
            category="C",
            apply_fn=apply_fn,
            remove_fn=remove_fn,
            corp_safe=True,
        )
        mock_all.append(td)
        mock_status.return_value = TweakResult.APPLIED

        path = self._make_snapshot(tmp_path, {"t1": "not applied"})
        result = restore_snapshot(path, force_corp=True)
        remove_fn.assert_called_once()
        assert result["t1"] == TweakResult.REMOVED

    @patch("regilattice.tweaks._ALL_TWEAKS", new_callable=list)
    @patch("regilattice.tweaks.tweak_status")
    def test_unchanged_tweak(
        self,
        mock_status: MagicMock,
        mock_all: list,
        tmp_path: Path,
    ) -> None:
        td = TweakDef(
            id="t1",
            label="T",
            category="C",
            apply_fn=MagicMock(),
            remove_fn=MagicMock(),
            corp_safe=True,
        )
        mock_all.append(td)
        mock_status.return_value = TweakResult.APPLIED

        path = self._make_snapshot(tmp_path, {"t1": "applied"})
        result = restore_snapshot(path, force_corp=True)
        assert result["t1"] == TweakResult.UNCHANGED

    @patch("regilattice.tweaks._ALL_TWEAKS", new_callable=list)
    @patch("regilattice.tweaks.tweak_status")
    @patch("regilattice.corpguard.is_corporate_network", return_value=True)
    def test_corp_skip(
        self,
        mock_corp: MagicMock,
        mock_status: MagicMock,
        mock_all: list,
        tmp_path: Path,
    ) -> None:
        td = TweakDef(
            id="t1",
            label="T",
            category="C",
            apply_fn=MagicMock(),
            remove_fn=MagicMock(),
            corp_safe=False,
        )
        mock_all.append(td)
        mock_status.return_value = TweakResult.NOT_APPLIED

        path = self._make_snapshot(tmp_path, {"t1": "applied"})
        result = restore_snapshot(path, force_corp=False)
        assert result["t1"] == TweakResult.SKIPPED_CORP


# ── apply_all / remove_all ───────────────────────────────────────────────────


class TestApplyAll:
    """Test apply_all() with mocked tweaks."""

    @patch("regilattice.tweaks._ALL_TWEAKS", new_callable=list)
    def test_applies_tweaks(self, mock_all: list) -> None:
        fn1 = MagicMock()
        fn2 = MagicMock()
        mock_all.extend(
            [
                TweakDef(id="a1", label="A", category="C", apply_fn=fn1, remove_fn=MagicMock(), corp_safe=True),
                TweakDef(id="a2", label="B", category="C", apply_fn=fn2, remove_fn=MagicMock(), corp_safe=True),
            ]
        )
        result = apply_all(force_corp=True)
        fn1.assert_called_once()
        fn2.assert_called_once()
        assert result["a1"] == TweakResult.APPLIED
        assert result["a2"] == TweakResult.APPLIED

    @patch("regilattice.tweaks._ALL_TWEAKS", new_callable=list)
    def test_progress_cb_called(self, mock_all: list) -> None:
        mock_all.append(
            TweakDef(id="p1", label="P", category="C", apply_fn=MagicMock(), remove_fn=MagicMock(), corp_safe=True),
        )
        cb = MagicMock()
        apply_all(force_corp=True, progress_cb=cb)
        cb.assert_called_once_with("p1", TweakResult.APPLIED)

    @patch("regilattice.tweaks._ALL_TWEAKS", new_callable=list)
    @patch("regilattice.corpguard.is_corporate_network", return_value=True)
    def test_corp_skip(self, mock_corp: MagicMock, mock_all: list) -> None:
        fn = MagicMock()
        mock_all.append(
            TweakDef(id="cs1", label="CS", category="C", apply_fn=fn, remove_fn=MagicMock(), corp_safe=False),
        )
        result = apply_all(force_corp=False)
        fn.assert_not_called()
        assert result["cs1"] == TweakResult.SKIPPED_CORP

    @patch("regilattice.tweaks._ALL_TWEAKS", new_callable=list)
    def test_parallel_mode(self, mock_all: list) -> None:
        fn = MagicMock()
        mock_all.append(
            TweakDef(id="par1", label="Par", category="C", apply_fn=fn, remove_fn=MagicMock(), corp_safe=True),
        )
        result = apply_all(force_corp=True, parallel=True, max_workers=2)
        fn.assert_called_once()
        assert result["par1"] == TweakResult.APPLIED

    @patch("regilattice.tweaks._ALL_TWEAKS", new_callable=list)
    def test_error_handling(self, mock_all: list) -> None:
        fn = MagicMock(side_effect=RuntimeError("fail"))
        mock_all.append(
            TweakDef(id="err1", label="E", category="C", apply_fn=fn, remove_fn=MagicMock(), corp_safe=True),
        )
        result = apply_all(force_corp=True)
        assert result["err1"] == TweakResult.ERROR


class TestRemoveAll:
    """Test remove_all() with mocked tweaks."""

    @patch("regilattice.tweaks._ALL_TWEAKS", new_callable=list)
    def test_removes_tweaks(self, mock_all: list) -> None:
        fn1 = MagicMock()
        fn2 = MagicMock()
        mock_all.extend(
            [
                TweakDef(id="r1", label="R", category="C", apply_fn=MagicMock(), remove_fn=fn1, corp_safe=True),
                TweakDef(id="r2", label="R2", category="C", apply_fn=MagicMock(), remove_fn=fn2, corp_safe=True),
            ]
        )
        result = remove_all(force_corp=True)
        fn1.assert_called_once()
        fn2.assert_called_once()
        assert result["r1"] == TweakResult.REMOVED
        assert result["r2"] == TweakResult.REMOVED

    @patch("regilattice.tweaks._ALL_TWEAKS", new_callable=list)
    def test_progress_cb_called(self, mock_all: list) -> None:
        mock_all.append(
            TweakDef(id="rp1", label="RP", category="C", apply_fn=MagicMock(), remove_fn=MagicMock(), corp_safe=True),
        )
        cb = MagicMock()
        remove_all(force_corp=True, progress_cb=cb)
        cb.assert_called_once_with("rp1", TweakResult.REMOVED)

    @patch("regilattice.tweaks._ALL_TWEAKS", new_callable=list)
    @patch("regilattice.corpguard.is_corporate_network", return_value=True)
    def test_corp_skip(self, mock_corp: MagicMock, mock_all: list) -> None:
        fn = MagicMock()
        mock_all.append(
            TweakDef(id="rcs1", label="RCS", category="C", apply_fn=MagicMock(), remove_fn=fn, corp_safe=False),
        )
        result = remove_all(force_corp=False)
        fn.assert_not_called()
        assert result["rcs1"] == TweakResult.SKIPPED_CORP

    @patch("regilattice.tweaks._ALL_TWEAKS", new_callable=list)
    def test_parallel_mode(self, mock_all: list) -> None:
        fn = MagicMock()
        mock_all.append(
            TweakDef(id="rpar1", label="RPar", category="C", apply_fn=MagicMock(), remove_fn=fn, corp_safe=True),
        )
        result = remove_all(force_corp=True, parallel=True, max_workers=2)
        fn.assert_called_once()
        assert result["rpar1"] == TweakResult.REMOVED

    @patch("regilattice.tweaks._ALL_TWEAKS", new_callable=list)
    def test_error_handling(self, mock_all: list) -> None:
        fn = MagicMock(side_effect=RuntimeError("fail"))
        mock_all.append(
            TweakDef(id="rerr1", label="RE", category="C", apply_fn=MagicMock(), remove_fn=fn, corp_safe=True),
        )
        result = remove_all(force_corp=True)
        assert result["rerr1"] == TweakResult.ERROR


# ── reload_plugins ───────────────────────────────────────────────────────────


class TestReloadPlugins:
    """Ensure reload_plugins() completes without raising."""

    def test_reload_does_not_crash(self) -> None:
        reload_plugins()  # should not raise

    def test_reload_preserves_tweaks(self) -> None:
        before = len(all_tweaks())
        reload_plugins()
        after = len(all_tweaks())
        assert before == after


# ── _topo_sort ───────────────────────────────────────────────────────────────


def _td(tid: str, deps: list[str] | None = None) -> TweakDef:
    """Create a minimal TweakDef with optional depends_on."""
    return TweakDef(
        id=tid,
        label=tid.upper(),
        category="C",
        apply_fn=MagicMock(),
        remove_fn=MagicMock(),
        corp_safe=True,
        depends_on=deps or [],
    )


class TestTopoSort:
    """Tests for the dependency-aware topological sort."""

    def test_no_deps_preserves_order(self) -> None:
        tweaks = [_td("a"), _td("b"), _td("c")]
        result = _topo_sort(tweaks)
        assert [t.id for t in result] == ["a", "b", "c"]

    def test_simple_chain(self) -> None:
        tweaks = [_td("c", ["b"]), _td("b", ["a"]), _td("a")]
        result = _topo_sort(tweaks)
        ids = [t.id for t in result]
        assert ids.index("a") < ids.index("b") < ids.index("c")

    def test_diamond_dependency(self) -> None:
        tweaks = [_td("d", ["b", "c"]), _td("b", ["a"]), _td("c", ["a"]), _td("a")]
        result = _topo_sort(tweaks)
        ids = [t.id for t in result]
        assert ids.index("a") < ids.index("b")
        assert ids.index("a") < ids.index("c")
        assert ids.index("b") < ids.index("d")
        assert ids.index("c") < ids.index("d")

    def test_missing_dep_outside_batch_ignored(self) -> None:
        tweaks = [_td("x", ["missing-id"])]
        result = _topo_sort(tweaks)
        assert [t.id for t in result] == ["x"]

    def test_cycle_raises(self) -> None:
        tweaks = [_td("a", ["b"]), _td("b", ["a"])]
        with pytest.raises(ValueError, match="Cyclic dependency"):
            _topo_sort(tweaks)
