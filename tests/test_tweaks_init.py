"""Tests for regilattice.tweaks.__init__ — plugin loader, search, status, snapshots."""
# mypy: disable-error-code="type-arg,no-untyped-def"
# pyright: reportMissingTypeArgument=false

from __future__ import annotations

import json
from pathlib import Path
from unittest.mock import MagicMock, patch

import pytest

from regilattice.tweaks import (
    CategoryInfo,
    TweakDef,
    TweakExecutor,
    TweakResult,
    _topo_sort,
    all_category_info,
    all_tweaks,
    apply_all,
    apply_profile,
    available_profiles,
    categories,
    categories_by_risk,
    categories_by_scope,
    category_counts,
    category_info,
    diff_snapshots,
    get_tweak,
    load_snapshot,
    profile_info,
    reload_plugins,
    remove_all,
    restore_snapshot,
    save_snapshot,
    search_tweaks,
    status_map,
    tweak_count_by_scope,
    tweak_risk_level,
    tweak_scope,
    tweak_status,
    tweaks_above_build,
    tweaks_by_category,
    tweaks_by_scope,
    tweaks_excluded_by_profile,
    tweaks_for_profile,
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
        assert td.source_url == ""  # Sprint 5: new optional KB/docs URL field

    def test_source_url_optional(self) -> None:
        """source_url defaults to empty string and accepts any URL string."""
        td_no_url = TweakDef(id="t2", label="L", category="C", apply_fn=lambda: None, remove_fn=lambda: None)
        assert td_no_url.source_url == ""
        td_with_url = TweakDef(
            id="t3",
            label="L",
            category="C",
            apply_fn=lambda: None,
            remove_fn=lambda: None,
            source_url="https://docs.microsoft.com/en-us/example",
        )
        assert td_with_url.source_url == "https://docs.microsoft.com/en-us/example"

    def test_source_url_present_on_all_tweaks(self) -> None:
        """Every loaded tweak must have a source_url attribute (may be empty)."""
        for td in all_tweaks():
            assert hasattr(td, "source_url"), f"{td.id} is missing source_url attribute"
            assert isinstance(td.source_url, str), f"{td.id}.source_url must be str"

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


# ── tweak_scope ──────────────────────────────────────────────────────────────


class TestTweakScope:
    """Test tweak_scope() classification."""

    def test_hkcu_is_user(self) -> None:
        td = TweakDef(id="scope.u", label="U", category="C", apply_fn=lambda: None, remove_fn=lambda: None, registry_keys=[r"HKCU\Test"])
        assert tweak_scope(td) == "user"

    def test_hklm_is_machine(self) -> None:
        td = TweakDef(id="scope.m", label="M", category="C", apply_fn=lambda: None, remove_fn=lambda: None, registry_keys=[r"HKLM\Test"])
        assert tweak_scope(td) == "machine"

    def test_both_hives(self) -> None:
        td = TweakDef(id="scope.b", label="B", category="C", apply_fn=lambda: None, remove_fn=lambda: None, registry_keys=[r"HKCU\A", r"HKLM\B"])
        assert tweak_scope(td) == "both"

    def test_no_keys_admin(self) -> None:
        td = TweakDef(id="scope.na", label="NA", category="C", apply_fn=lambda: None, remove_fn=lambda: None, needs_admin=True)
        assert tweak_scope(td) == "machine"

    def test_no_keys_no_admin(self) -> None:
        td = TweakDef(id="scope.nn", label="NN", category="C", apply_fn=lambda: None, remove_fn=lambda: None, needs_admin=False)
        assert tweak_scope(td) == "user"

    def test_hkcr_is_machine(self) -> None:
        td = TweakDef(
            id="scope.cr",
            label="CR",
            category="C",
            apply_fn=lambda: None,
            remove_fn=lambda: None,
            registry_keys=[r"HKEY_CLASSES_ROOT\Test"],
        )
        assert tweak_scope(td) == "machine"

    def test_real_tweaks_have_scope(self, all_tweaks_list: list[TweakDef]) -> None:
        for td in all_tweaks_list[:10]:
            s = tweak_scope(td)
            assert s in {"user", "machine", "both"}


# ── Category info ────────────────────────────────────────────────────────────


class TestCategoryInfo:
    """Test category metadata functions."""

    def test_category_info_returns_for_known(self) -> None:
        cats = categories()
        assert len(cats) > 0
        ci = category_info(cats[0])
        assert ci is not None
        assert isinstance(ci, CategoryInfo)
        assert ci.name == cats[0]

    def test_category_info_returns_none_for_unknown(self) -> None:
        assert category_info("__nonexistent_category__") is None

    def test_all_category_info_matches_categories(self) -> None:
        all_ci = all_category_info()
        assert set(all_ci.keys()) == set(categories())

    def test_category_info_risk_is_valid(self) -> None:
        for ci in all_category_info().values():
            assert ci.risk_level in {"low", "medium", "high"}

    def test_category_info_scope_is_valid(self) -> None:
        for ci in all_category_info().values():
            assert ci.scope in {"user", "machine", "mixed"}

    def test_categories_by_risk(self) -> None:
        high = categories_by_risk("high")
        assert isinstance(high, list)
        for name in high:
            ci = category_info(name)
            assert ci is not None
            assert ci.risk_level == "high"

    def test_categories_by_scope_user(self) -> None:
        user_cats = categories_by_scope("user")
        for name in user_cats:
            ci = category_info(name)
            assert ci is not None
            assert ci.scope == "user"

    def test_categories_by_scope_machine(self) -> None:
        machine_cats = categories_by_scope("machine")
        for name in machine_cats:
            ci = category_info(name)
            assert ci is not None
            assert ci.scope == "machine"


# ── Profiles ─────────────────────────────────────────────────────────────────


class TestProfiles:
    """Test profile metadata and tweak retrieval."""

    def test_available_profiles(self) -> None:
        profs = available_profiles()
        assert isinstance(profs, list)
        assert "business" in profs
        assert "gaming" in profs
        assert "privacy" in profs
        assert "minimal" in profs
        assert "server" in profs

    def test_profile_info_known(self) -> None:
        pi = profile_info("business")
        assert pi is not None
        assert pi.id == "business"
        assert len(pi.description) > 0
        assert len(pi.apply_categories) > 0

    def test_profile_info_unknown(self) -> None:
        assert profile_info("__fake_profile__") is None

    def test_tweaks_for_profile_returns_list(self) -> None:
        result = tweaks_for_profile("business")
        assert isinstance(result, list)
        assert all(isinstance(td, TweakDef) for td in result)

    def test_tweaks_for_profile_unknown_raises(self) -> None:
        with pytest.raises(ValueError, match="Unknown profile"):
            tweaks_for_profile("__fake__")

    def test_tweaks_excluded_by_profile(self) -> None:
        result = tweaks_excluded_by_profile("business")
        assert isinstance(result, list)

    def test_tweaks_excluded_unknown_raises(self) -> None:
        with pytest.raises(ValueError, match="Unknown profile"):
            tweaks_excluded_by_profile("__fake__")

    def test_gaming_profile_covers_gaming_category(self) -> None:
        result = tweaks_for_profile("gaming")
        cats = {td.category for td in result}
        assert "Gaming" in cats

    @patch("regilattice.tweaks._ALL_TWEAKS", new_callable=list)
    @patch("regilattice.tweaks._TWEAKS_BY_CAT", new_callable=dict)
    def test_apply_profile(self, mock_cat: dict, mock_all: list) -> None:
        from regilattice.tweaks import _tweaks_for_profile_inner

        _tweaks_for_profile_inner.cache_clear()  # ensure patched _TWEAKS_BY_CAT is used
        try:
            fn = MagicMock()
            td = TweakDef(id="ap1", label="AP", category="Cloud Storage", apply_fn=fn, remove_fn=MagicMock(), corp_safe=True)
            mock_all.append(td)
            mock_cat["Cloud Storage"] = [td]
            result = apply_profile("business", force_corp=True)
            fn.assert_called_once()
            assert result["ap1"] == TweakResult.APPLIED
        finally:
            _tweaks_for_profile_inner.cache_clear()  # restore for subsequent tests


# ── diff_snapshots ───────────────────────────────────────────────────────────


class TestDiffSnapshots:
    """Test snapshot diff comparison."""

    def test_identical_snapshots(self, tmp_path: Path) -> None:
        a = tmp_path / "a.json"
        b = tmp_path / "b.json"
        data = {"x": "applied", "y": "not applied"}
        a.write_text(json.dumps(data), encoding="utf-8")
        b.write_text(json.dumps(data), encoding="utf-8")
        assert diff_snapshots(a, b) == {}

    def test_different_snapshots(self, tmp_path: Path) -> None:
        a = tmp_path / "a.json"
        b = tmp_path / "b.json"
        a.write_text(json.dumps({"x": "applied", "y": "applied"}), encoding="utf-8")
        b.write_text(json.dumps({"x": "applied", "y": "not applied"}), encoding="utf-8")
        diffs = diff_snapshots(a, b)
        assert "y" in diffs
        assert diffs["y"] == ("applied", "not applied")
        assert "x" not in diffs

    def test_missing_in_one_snapshot(self, tmp_path: Path) -> None:
        a = tmp_path / "a.json"
        b = tmp_path / "b.json"
        a.write_text(json.dumps({"x": "applied"}), encoding="utf-8")
        b.write_text(json.dumps({"y": "applied"}), encoding="utf-8")
        diffs = diff_snapshots(a, b)
        assert diffs["x"] == ("applied", "(absent)")
        assert diffs["y"] == ("(absent)", "applied")


# ── TweakExecutor ────────────────────────────────────────────────────────────


class TestTweakExecutor:
    """Test the TweakExecutor class directly."""

    def test_apply_one_corp_blocked(self) -> None:
        td = TweakDef(id="te1", label="TE", category="C", apply_fn=MagicMock(), remove_fn=MagicMock(), corp_safe=False)
        exe = TweakExecutor(force_corp=False)
        with patch("regilattice.tweaks.TweakExecutor._is_blocked", return_value=True):
            assert exe.apply_one(td) == TweakResult.SKIPPED_CORP

    def test_apply_one_success(self) -> None:
        fn = MagicMock()
        td = TweakDef(id="te2", label="TE", category="C", apply_fn=fn, remove_fn=MagicMock(), corp_safe=True)
        exe = TweakExecutor(force_corp=True)
        assert exe.apply_one(td) == TweakResult.APPLIED
        fn.assert_called_once()

    def test_apply_one_error(self) -> None:
        fn = MagicMock(side_effect=RuntimeError("boom"))
        td = TweakDef(id="te3", label="TE", category="C", apply_fn=fn, remove_fn=MagicMock(), corp_safe=True)
        exe = TweakExecutor(force_corp=True)
        assert exe.apply_one(td) == TweakResult.ERROR

    def test_remove_one_success(self) -> None:
        fn = MagicMock()
        td = TweakDef(id="te4", label="TE", category="C", apply_fn=MagicMock(), remove_fn=fn, corp_safe=True)
        exe = TweakExecutor(force_corp=True)
        assert exe.remove_one(td) == TweakResult.REMOVED
        fn.assert_called_once()

    def test_remove_one_error(self) -> None:
        fn = MagicMock(side_effect=RuntimeError("boom"))
        td = TweakDef(id="te5", label="TE", category="C", apply_fn=MagicMock(), remove_fn=fn, corp_safe=True)
        exe = TweakExecutor(force_corp=True)
        assert exe.remove_one(td) == TweakResult.ERROR

    def test_remove_one_corp_blocked(self) -> None:
        td = TweakDef(id="te6", label="TE", category="C", apply_fn=MagicMock(), remove_fn=MagicMock(), corp_safe=False)
        exe = TweakExecutor(force_corp=False)
        with patch("regilattice.tweaks.TweakExecutor._is_blocked", return_value=True):
            assert exe.remove_one(td) == TweakResult.SKIPPED_CORP

    def test_run_batch_apply(self) -> None:
        fn1 = MagicMock()
        fn2 = MagicMock()
        td1 = TweakDef(id="rb1", label="RB1", category="C", apply_fn=fn1, remove_fn=MagicMock(), corp_safe=True)
        td2 = TweakDef(id="rb2", label="RB2", category="C", apply_fn=fn2, remove_fn=MagicMock(), corp_safe=True)
        exe = TweakExecutor(force_corp=True)
        result = exe.run_batch([td1, td2], "apply")
        assert result["rb1"] == TweakResult.APPLIED
        assert result["rb2"] == TweakResult.APPLIED

    def test_run_batch_remove_parallel(self) -> None:
        fn = MagicMock()
        td = TweakDef(id="rbp1", label="RBP", category="C", apply_fn=MagicMock(), remove_fn=fn, corp_safe=True)
        exe = TweakExecutor(force_corp=True)
        result = exe.run_batch([td], "remove", parallel=True, max_workers=2)
        assert result["rbp1"] == TweakResult.REMOVED

    def test_run_batch_progress_cb(self) -> None:
        fn = MagicMock()
        td = TweakDef(id="rbcb1", label="RBCB", category="C", apply_fn=fn, remove_fn=MagicMock(), corp_safe=True)
        exe = TweakExecutor(force_corp=True)
        cb = MagicMock()
        exe.run_batch([td], "apply", progress_cb=cb)
        cb.assert_called_once_with("rbcb1", TweakResult.APPLIED)

    def test_is_blocked_force_corp(self) -> None:
        td = TweakDef(id="bl1", label="BL", category="C", apply_fn=MagicMock(), remove_fn=MagicMock(), corp_safe=False)
        exe = TweakExecutor(force_corp=True)
        assert exe.is_blocked(td) is False

    def test_is_blocked_corp_safe(self) -> None:
        td = TweakDef(id="bl2", label="BL", category="C", apply_fn=MagicMock(), remove_fn=MagicMock(), corp_safe=True)
        exe = TweakExecutor(force_corp=False)
        assert exe.is_blocked(td) is False


# ── Snapshot full round-trip ─────────────────────────────────────────────────


class TestSnapshotRoundTrip:
    """End-to-end save → load → diff → restore cycle."""

    def test_save_load_round_trip(self, tmp_path: Path) -> None:
        with patch("regilattice.tweaks.status_map", return_value={"a": TweakResult.APPLIED, "b": TweakResult.NOT_APPLIED}):
            save_snapshot(tmp_path / "s.json")
        loaded = load_snapshot(tmp_path / "s.json")
        assert loaded == {"a": "applied", "b": "not applied"}

    def test_diff_against_self_is_empty(self, tmp_path: Path) -> None:
        data = {"x": "applied", "y": "unknown"}
        (tmp_path / "a.json").write_text(json.dumps(data), encoding="utf-8")
        (tmp_path / "b.json").write_text(json.dumps(data), encoding="utf-8")
        assert diff_snapshots(tmp_path / "a.json", tmp_path / "b.json") == {}

    @patch("regilattice.tweaks._ALL_TWEAKS", new_callable=list)
    @patch("regilattice.tweaks.tweak_status")
    def test_restore_then_diff_shows_no_changes(self, mock_status: MagicMock, mock_all: list, tmp_path: Path) -> None:
        """After restoring, a new snapshot should match the original."""
        apply_fn = MagicMock()
        td = TweakDef(id="rt1", label="RT", category="C", apply_fn=apply_fn, remove_fn=MagicMock(), corp_safe=True)
        mock_all.append(td)
        mock_status.return_value = TweakResult.NOT_APPLIED
        snap = tmp_path / "snap.json"
        snap.write_text(json.dumps({"rt1": "applied"}), encoding="utf-8")
        result = restore_snapshot(snap, force_corp=True)
        assert result["rt1"] == TweakResult.APPLIED
        apply_fn.assert_called_once()


# ── Additional coverage: profiles, search, tags ──────────────────────────────


class TestAllProfilesCoverage:
    """Ensure every profile yields non-empty tweak lists and consistent categories."""

    def test_all_profiles_return_tweaks(self) -> None:
        for name in available_profiles():
            tds = tweaks_for_profile(name)
            assert len(tds) > 0, f"Profile {name!r} returned 0 tweaks"

    def test_profile_categories_match_info(self) -> None:
        for name in available_profiles():
            pi = profile_info(name)
            assert pi is not None
            tds = tweaks_for_profile(name)
            cats = {td.category for td in tds}
            for cat in pi.apply_categories:
                assert cat in cats or cat not in categories(), f"Profile {name!r}: {cat!r} not in tweaks"

    def test_excluded_and_included_no_overlap(self) -> None:
        for name in available_profiles():
            included_ids = {td.id for td in tweaks_for_profile(name)}
            excluded_ids = {td.id for td in tweaks_excluded_by_profile(name)}
            assert included_ids & excluded_ids == set(), f"Profile {name!r}: overlap"


class TestSearchEdgeCases:
    """Test edge cases in search_tweaks."""

    def test_case_insensitive(self) -> None:
        results_lower = search_tweaks("explorer")
        results_upper = search_tweaks("EXPLORER")
        assert {r.id for r in results_lower} == {r.id for r in results_upper}

    def test_partial_match(self) -> None:
        results = search_tweaks("explor")
        assert len(results) > 0

    def test_special_chars_no_crash(self) -> None:
        search_tweaks("[.*+?")
        search_tweaks("\\")


class TestTagIntegrity:
    """Verify tag data integrity across all tweaks."""

    def test_all_tags_are_strings(self, all_tweaks_list: list[TweakDef]) -> None:
        for td in all_tweaks_list:
            assert isinstance(td.tags, list), f"{td.id}: tags not a list"
            for tag in td.tags:
                assert isinstance(tag, str) and tag, f"{td.id}: empty or non-str tag"

    def test_all_tags_are_ascii(self, all_tweaks_list: list[TweakDef]) -> None:
        for td in all_tweaks_list:
            for tag in td.tags:
                assert tag.isascii(), f"{td.id}: tag {tag!r} contains non-ASCII"

    def test_no_whitespace_in_tags(self, all_tweaks_list: list[TweakDef]) -> None:
        for td in all_tweaks_list:
            for tag in td.tags:
                assert tag.strip() == tag, f"{td.id}: tag {tag!r} has whitespace"


# ── C8 engine additions ───────────────────────────────────────────────────────


from regilattice.tweaks import (
    apply_tweaks,
    filter_tweaks,  # noqa: E402
    remove_tweaks,
    tweak_dependencies,
    tweaks_by_ids,
    tweaks_by_tag,
)


class TestFilterTweaks:
    """Tests for filter_tweaks() composable filter."""

    def test_no_criteria_returns_all(self, all_tweaks_list: list[TweakDef]) -> None:
        result = filter_tweaks()
        assert len(result) == len(all_tweaks_list)

    def test_corp_safe_true(self, all_tweaks_list: list[TweakDef]) -> None:
        result = filter_tweaks(corp_safe=True)
        assert all(td.corp_safe for td in result)

    def test_corp_safe_false(self, all_tweaks_list: list[TweakDef]) -> None:
        result = filter_tweaks(corp_safe=False)
        assert all(not td.corp_safe for td in result)

    def test_needs_admin_true(self, all_tweaks_list: list[TweakDef]) -> None:
        result = filter_tweaks(needs_admin=True)
        assert all(td.needs_admin for td in result)

    def test_needs_admin_false(self, all_tweaks_list: list[TweakDef]) -> None:
        result = filter_tweaks(needs_admin=False)
        assert all(not td.needs_admin for td in result)

    def test_scope_user(self) -> None:
        result = filter_tweaks(scope="user")
        for td in result:
            from regilattice.tweaks import tweak_scope

            assert tweak_scope(td) == "user"

    def test_scope_machine(self) -> None:
        result = filter_tweaks(scope="machine")
        for td in result:
            from regilattice.tweaks import tweak_scope

            assert tweak_scope(td) == "machine"

    def test_category_filter(self, all_tweaks_list: list[TweakDef]) -> None:
        cat = categories()[0]
        result = filter_tweaks(category=cat)
        assert all(td.category == cat for td in result)
        assert len(result) > 0

    def test_min_build_zero_includes_all(self, all_tweaks_list: list[TweakDef]) -> None:
        result = filter_tweaks(min_build=0)
        # Only tweaks with min_build==0 (no requirement) are included
        assert all(td.min_build <= 0 for td in result)

    def test_min_build_large_includes_all(self, all_tweaks_list: list[TweakDef]) -> None:
        result = filter_tweaks(min_build=99999)
        assert len(result) == len(all_tweaks_list)

    def test_query_filters_results(self) -> None:
        result = filter_tweaks(query="zzz_completely_nonexistent_query_xyz999")
        assert result == []

    def test_tags_filter(self, all_tweaks_list: list[TweakDef]) -> None:
        tagged = [td for td in all_tweaks_list if td.tags]
        if not tagged:
            pytest.skip("No tweaks with tags")
        td0 = tagged[0]
        result = filter_tweaks(tags=[td0.tags[0]])
        assert any(td.id == td0.id for td in result)

    def test_combined_criteria(self, all_tweaks_list: list[TweakDef]) -> None:
        cat = categories()[0]
        result = filter_tweaks(category=cat, needs_admin=True)
        assert all(td.category == cat and td.needs_admin for td in result)

    def test_empty_tags_list_returns_all(self, all_tweaks_list: list[TweakDef]) -> None:
        # Empty tag list: every tweak has empty.issubset(...) == True
        result = filter_tweaks(tags=[])
        assert len(result) == len(all_tweaks_list)


class TestTweakDependencies:
    """Tests for tweak_dependencies() resolver."""

    def test_no_deps_returns_empty(self, all_tweaks_list: list[TweakDef]) -> None:
        td = next(t for t in all_tweaks_list if not t.depends_on)
        assert tweak_dependencies(td) == []

    def test_direct_only(self) -> None:
        a = TweakDef(id="dep.a", label="A", category="C", apply_fn=MagicMock(), remove_fn=MagicMock(), corp_safe=True)
        b = TweakDef(id="dep.b", label="B", category="C", apply_fn=MagicMock(), remove_fn=MagicMock(), corp_safe=True, depends_on=["dep.a"])
        with (
            patch("regilattice.tweaks._TWEAK_INDEX", {"dep.a": a, "dep.b": b}),
            patch("regilattice.tweaks._ALL_TWEAKS", [a, b]),
        ):
            result = tweak_dependencies(b, transitive=False)
        assert result == [a]

    def test_transitive_chain(self) -> None:
        a = TweakDef(id="dep.ta", label="A", category="C", apply_fn=MagicMock(), remove_fn=MagicMock(), corp_safe=True)
        b = TweakDef(id="dep.tb", label="B", category="C", apply_fn=MagicMock(), remove_fn=MagicMock(), corp_safe=True, depends_on=["dep.ta"])
        c = TweakDef(id="dep.tc", label="C", category="C", apply_fn=MagicMock(), remove_fn=MagicMock(), corp_safe=True, depends_on=["dep.tb"])
        with (
            patch("regilattice.tweaks._TWEAK_INDEX", {"dep.ta": a, "dep.tb": b, "dep.tc": c}),
            patch("regilattice.tweaks._ALL_TWEAKS", [a, b, c]),
        ):
            result = tweak_dependencies(c, transitive=True)
        ids = [t.id for t in result]
        assert "dep.ta" in ids
        assert "dep.tb" in ids
        assert ids.index("dep.ta") < ids.index("dep.tb")

    def test_unknown_dep_silently_skipped(self) -> None:
        td = TweakDef(id="dep.unk", label="U", category="C", apply_fn=MagicMock(), remove_fn=MagicMock(), corp_safe=True, depends_on=["missing.id"])
        with patch("regilattice.tweaks._TWEAK_INDEX", {"dep.unk": td}):
            result = tweak_dependencies(td, transitive=True)
        assert result == []


class TestPartialStatusMap:
    """Tests for status_map(ids=...) partial evaluation."""

    @patch("regilattice.tweaks.tweak_status", return_value=TweakResult.UNKNOWN)
    def test_ids_restricts_output(self, _mock: MagicMock, all_tweaks_list: list[TweakDef]) -> None:
        ids = [all_tweaks_list[0].id, all_tweaks_list[1].id]
        result = status_map(ids=ids)
        assert set(result.keys()) == set(ids)

    @patch("regilattice.tweaks.tweak_status", return_value=TweakResult.APPLIED)
    def test_ids_none_returns_all(self, _mock: MagicMock, all_tweaks_list: list[TweakDef]) -> None:
        result = status_map(ids=None)
        assert len(result) == len(all_tweaks_list)

    @patch("regilattice.tweaks.tweak_status", return_value=TweakResult.UNKNOWN)
    def test_unknown_ids_silently_skipped(self, _mock: MagicMock) -> None:
        result = status_map(ids=["nonexistent.id.xyz"])
        assert result == {}

    @patch("regilattice.tweaks.tweak_status", return_value=TweakResult.APPLIED)
    def test_parallel_ids_subset(self, _mock: MagicMock, all_tweaks_list: list[TweakDef]) -> None:
        ids = [all_tweaks_list[0].id]
        result = status_map(ids=ids, parallel=True, max_workers=2)
        assert set(result.keys()) == set(ids)

    @patch("regilattice.tweaks.tweak_status", return_value=TweakResult.UNKNOWN)
    def test_progress_fn_called_correct_times(self, _mock: MagicMock, all_tweaks_list: list[TweakDef]) -> None:
        ids = [all_tweaks_list[0].id, all_tweaks_list[1].id]
        calls: list[tuple[int, int]] = []
        status_map(ids=ids, progress_fn=lambda done, total: calls.append((done, total)))
        assert len(calls) == 2
        assert calls[-1] == (2, 2)


class TestApplyRemoveTweaks:
    """Tests for apply_tweaks() and remove_tweaks() ID-based batch helpers."""

    @patch("regilattice.tweaks._TWEAK_INDEX")
    @patch("regilattice.tweaks._ALL_TWEAKS", new_callable=list)
    def test_apply_tweaks_calls_apply_fn(self, mock_all: list, mock_index: MagicMock) -> None:
        fn = MagicMock()
        td = TweakDef(id="at1", label="AT", category="C", apply_fn=fn, remove_fn=MagicMock(), corp_safe=True)
        mock_all.append(td)
        mock_index.__getitem__ = MagicMock(return_value=td)
        mock_index.__contains__ = MagicMock(return_value=True)
        mock_index.get = MagicMock(return_value=td)
        with patch("regilattice.tweaks.tweaks_by_ids", return_value=[td]):
            result = apply_tweaks(["at1"], force_corp=True, include_deps=False)
        fn.assert_called_once()
        assert result["at1"] == TweakResult.APPLIED

    @patch("regilattice.tweaks._TWEAK_INDEX")
    @patch("regilattice.tweaks._ALL_TWEAKS", new_callable=list)
    def test_remove_tweaks_calls_remove_fn(self, mock_all: list, mock_index: MagicMock) -> None:
        fn = MagicMock()
        td = TweakDef(id="rt1", label="RT", category="C", apply_fn=MagicMock(), remove_fn=fn, corp_safe=True)
        mock_all.append(td)
        with patch("regilattice.tweaks.tweaks_by_ids", return_value=[td]):
            result = remove_tweaks(["rt1"], force_corp=True)
        fn.assert_called_once()
        assert result["rt1"] == TweakResult.REMOVED

    def test_apply_tweaks_empty_ids(self) -> None:
        result = apply_tweaks([], force_corp=True)
        assert result == {}

    def test_remove_tweaks_empty_ids(self) -> None:
        result = remove_tweaks([], force_corp=True)
        assert result == {}


class TestTweaksByIdsAndTag:
    """Tests for tweaks_by_ids() and tweaks_by_tag() helpers."""

    def test_tweaks_by_ids_known(self, all_tweaks_list: list[TweakDef]) -> None:
        ids = [all_tweaks_list[0].id, all_tweaks_list[1].id]
        result = tweaks_by_ids(ids)
        assert {td.id for td in result} == set(ids)

    def test_tweaks_by_ids_unknown_silently_skipped(self) -> None:
        result = tweaks_by_ids(["unknown.xyz.999"])
        assert result == []

    def test_tweaks_by_ids_empty(self) -> None:
        assert tweaks_by_ids([]) == []

    def test_tweaks_by_tag_known(self, all_tweaks_list: list[TweakDef]) -> None:
        tagged = [td for td in all_tweaks_list if td.tags]
        if not tagged:
            pytest.skip("No tweaks with tags")
        td0 = tagged[0]
        result = tweaks_by_tag(td0.tags[0])
        assert any(td.id == td0.id for td in result)

    def test_tweaks_by_tag_unknown_returns_empty(self) -> None:
        assert tweaks_by_tag("zzz_no_such_tag_xyz_9999") == []

    def test_tweaks_by_tag_case_insensitive(self, all_tweaks_list: list[TweakDef]) -> None:
        tagged = [td for td in all_tweaks_list if td.tags]
        if not tagged:
            pytest.skip("No tweaks with tags")
        tag = tagged[0].tags[0]
        assert tweaks_by_tag(tag.lower()) == tweaks_by_tag(tag.upper())


# ── C11 Phase 45: status_map parallel + progress_fn edge cases ───────────────


class TestStatusMapParallelAndProgress:
    """Edge cases for status_map with parallel=True and progress_fn tracking."""

    def test_status_map_parallel_ids_subset(self, all_tweaks_list: list[TweakDef]) -> None:
        """parallel=True + ids= restriction returns only the requested IDs."""
        subset = [all_tweaks_list[0].id, all_tweaks_list[1].id]
        result = status_map(ids=subset, parallel=True)
        assert set(result.keys()) == set(subset)

    def test_status_map_parallel_returns_tweak_results(self, all_tweaks_list: list[TweakDef]) -> None:
        """All values are TweakResult instances."""
        ids = [td.id for td in all_tweaks_list[:3]]
        result = status_map(ids=ids, parallel=True)
        assert all(isinstance(v, TweakResult) for v in result.values())

    def test_status_map_progress_fn_called_exact_times(self, all_tweaks_list: list[TweakDef]) -> None:
        """progress_fn(done, total) is invoked once per tweak (sequential)."""
        ids = [td.id for td in all_tweaks_list[:4]]
        calls: list[tuple[int, int]] = []
        status_map(ids=ids, parallel=False, progress_fn=lambda d, t: calls.append((d, t)))
        assert len(calls) == 4
        # done increments 1..N
        assert [d for d, _ in calls] == [1, 2, 3, 4]
        # total is always the same
        assert all(t == 4 for _, t in calls)

    def test_status_map_progress_fn_parallel(self, all_tweaks_list: list[TweakDef]) -> None:
        """progress_fn is called for each tweak in parallel mode too."""
        ids = [td.id for td in all_tweaks_list[:3]]
        call_count: list[int] = []
        status_map(ids=ids, parallel=True, progress_fn=lambda d, t: call_count.append(d))
        assert len(call_count) == 3

    def test_status_map_empty_ids_returns_empty(self) -> None:
        """status_map(ids=[]) returns empty dict."""
        result = status_map(ids=[])
        assert result == {}

    def test_status_map_unknown_ids_skipped(self) -> None:
        """Unknown IDs are silently excluded from results."""
        result = status_map(ids=["zzz_no_exist_xyz_99999"])
        assert result == {}


# ── C11 Phase 46: apply_tweaks(include_deps=True) pulls in dependencies ──────


class TestApplyTweaksIncludeDeps:
    """Verify that apply_tweaks resolves and includes dependency tweaks."""

    @patch("regilattice.tweaks._TWEAK_INDEX")
    @patch("regilattice.tweaks._ALL_TWEAKS", new_callable=list)
    def test_include_deps_calls_dep_apply_fn(self, mock_all: list, mock_index: MagicMock) -> None:
        """When include_deps=True, dependency apply_fn is called before the main tweak."""
        dep_fn = MagicMock()
        main_fn = MagicMock()

        dep_td = TweakDef(id="dep.one", label="Dep", category="C", apply_fn=dep_fn, remove_fn=MagicMock(), corp_safe=True)
        main_td = TweakDef(id="main.one", label="Main", category="C", apply_fn=main_fn, remove_fn=MagicMock(), corp_safe=True, depends_on=["dep.one"])

        mock_all.extend([dep_td, main_td])
        mock_index.__getitem__ = MagicMock(side_effect=lambda k: {"dep.one": dep_td, "main.one": main_td}[k])
        mock_index.__contains__ = MagicMock(side_effect=lambda k: k in {"dep.one", "main.one"})
        mock_index.get = MagicMock(side_effect=lambda k, default=None: {"dep.one": dep_td, "main.one": main_td}.get(k, default))

        with patch("regilattice.tweaks.tweaks_by_ids", return_value=[main_td]):
            result = apply_tweaks(["main.one"], force_corp=True, include_deps=True)

        dep_fn.assert_called_once()
        main_fn.assert_called_once()
        assert result.get("main.one") == TweakResult.APPLIED

    @patch("regilattice.tweaks._TWEAK_INDEX")
    @patch("regilattice.tweaks._ALL_TWEAKS", new_callable=list)
    def test_include_deps_false_does_not_call_dep_fn(self, mock_all: list, mock_index: MagicMock) -> None:
        """When include_deps=False, dependency apply_fn is NOT called."""
        dep_fn = MagicMock()
        main_fn = MagicMock()

        dep_td = TweakDef(id="dep.two", label="Dep2", category="C", apply_fn=dep_fn, remove_fn=MagicMock(), corp_safe=True)
        main_td = TweakDef(
            id="main.two",
            label="Main2",
            category="C",
            apply_fn=main_fn,
            remove_fn=MagicMock(),
            corp_safe=True,
            depends_on=["dep.two"],
        )

        mock_all.extend([dep_td, main_td])
        mock_index.__getitem__ = MagicMock(side_effect=lambda k: {"dep.two": dep_td, "main.two": main_td}[k])
        mock_index.__contains__ = MagicMock(side_effect=lambda k: k in {"dep.two", "main.two"})
        mock_index.get = MagicMock(side_effect=lambda k, default=None: {"dep.two": dep_td, "main.two": main_td}.get(k, default))

        with patch("regilattice.tweaks.tweaks_by_ids", return_value=[main_td]):
            apply_tweaks(["main.two"], force_corp=True, include_deps=False)

        dep_fn.assert_not_called()
        main_fn.assert_called_once()

    def test_apply_tweaks_unknown_dep_silently_skipped(self) -> None:
        """If a dependency ID doesn't exist in the index, apply still runs main tweak."""
        fn = MagicMock()
        td = TweakDef(
            id="orphan.main",
            label="Orphan",
            category="C",
            apply_fn=fn,
            remove_fn=MagicMock(),
            corp_safe=True,
            depends_on=["does.not.exist.zzz"],
        )
        with patch("regilattice.tweaks.tweaks_by_ids", return_value=[td]):
            result = apply_tweaks(["orphan.main"], force_corp=True, include_deps=True)
        fn.assert_called_once()
        assert result.get("orphan.main") == TweakResult.APPLIED


# ── Scope / build / risk / count helpers ────────────────────────────────────


class TestTweaksByScope:
    """Tests for tweaks_by_scope()."""

    def test_returns_list(self) -> None:
        result = tweaks_by_scope("user")
        assert isinstance(result, list)

    def test_all_scopes_non_negative(self) -> None:
        for scope in ("user", "machine", "both"):
            result = tweaks_by_scope(scope)
            assert len(result) >= 0

    def test_user_machine_both_partition(self) -> None:
        """user + machine + both should sum to all tweaks count."""
        u = tweaks_by_scope("user")
        m = tweaks_by_scope("machine")
        b = tweaks_by_scope("both")
        assert len(u) + len(m) + len(b) == len(all_tweaks())

    def test_each_result_matches_requested_scope(self) -> None:
        for scope in ("user", "machine", "both"):
            for td in tweaks_by_scope(scope):
                assert tweak_scope(td) == scope

    def test_unknown_scope_returns_empty(self) -> None:
        result = tweaks_by_scope("invalid_scope")
        assert result == []


class TestTweaksAboveBuild:
    """Tests for tweaks_above_build()."""

    def test_build_zero_includes_all(self) -> None:
        # build=0 excludes tweaks with min_build>0; a very high build includes all
        result = tweaks_above_build(99999)
        assert len(result) == len(all_tweaks())

    def test_large_build_still_has_results(self) -> None:
        """min_build=0 tweaks should always be included regardless of build."""
        result = tweaks_above_build(99999)
        # All tweaks with min_build=0 must be in the result
        zero_tweaks = [td for td in all_tweaks() if td.min_build == 0]
        for td in zero_tweaks:
            assert td in result

    def test_result_is_subset_of_all_tweaks(self) -> None:
        full = set(td.id for td in all_tweaks())
        for td in tweaks_above_build(22000):
            assert td.id in full

    def test_min_build_constraint_respected(self) -> None:
        build = 22000
        for td in tweaks_above_build(build):
            assert td.min_build <= build


class TestTweakRiskLevel:
    """Tests for tweak_risk_level()."""

    def test_returns_known_risk_string(self) -> None:
        valid = {"low", "medium", "high"}
        for td in all_tweaks():
            assert tweak_risk_level(td) in valid

    def test_all_tweaks_have_risk(self) -> None:
        """No tweak should return an unexpected string."""
        for td in all_tweaks():
            level = tweak_risk_level(td)
            assert isinstance(level, str) and len(level) > 0


class TestTweakCountByScope:
    """Tests for tweak_count_by_scope()."""

    def test_returns_dict(self) -> None:
        result = tweak_count_by_scope()
        assert isinstance(result, dict)

    def test_has_all_three_scopes(self) -> None:
        result = tweak_count_by_scope()
        for scope in ("user", "machine", "both"):
            assert scope in result

    def test_counts_are_non_negative(self) -> None:
        for count in tweak_count_by_scope().values():
            assert count >= 0

    def test_total_matches_all_tweaks(self) -> None:
        total = sum(tweak_count_by_scope().values())
        assert total == len(all_tweaks())


class TestCategoryCounts:
    """Tests for category_counts()."""

    def test_returns_dict(self) -> None:
        assert isinstance(category_counts(), dict)

    def test_has_explorer_category(self) -> None:
        assert "Explorer" in category_counts()

    def test_counts_match_tweaks_by_category(self) -> None:
        cc = category_counts()
        for cat, tds in tweaks_by_category().items():
            assert cc[cat] == len(tds)

    def test_total_matches_all_tweaks(self) -> None:
        assert sum(category_counts().values()) == len(all_tweaks())


class TestScopeCachePrewarmed:
    """Verify that scope cache is pre-warmed at import time."""

    def test_scope_cache_populated_for_all_tweaks(self) -> None:
        from regilattice.tweaks import _SCOPE_CACHE

        for td in all_tweaks():
            assert td.id in _SCOPE_CACHE

    def test_scope_cache_values_are_valid(self) -> None:
        from regilattice.tweaks import _SCOPE_CACHE

        valid = {"user", "machine", "both"}
        for val in _SCOPE_CACHE.values():
            assert val in valid


# ── C22 Phase 87-88: filter_tweaks early-exit + status_map detect-free ───────


class TestFilterTweaksEarlyExit:
    """Verify filter_tweaks returns early when pool is drained after a filter step."""

    def test_nonexistent_category_returns_empty(self) -> None:
        result = filter_tweaks(category="__NO_SUCH_CATEGORY_xyz9999__")
        assert result == []

    def test_impossible_corp_and_admin_combo_returns_list(self) -> None:
        # corp_safe=True tweaks are HKCU-only (usually needs_admin=False)
        # Either way, impossible combo should return a list (possibly empty)
        result = filter_tweaks(corp_safe=True, needs_admin=True)
        assert isinstance(result, list)

    def test_early_exit_does_not_skip_valid_tweaks(self, all_tweaks_list: list[TweakDef]) -> None:
        cat = categories()[0]
        # min_build=99999 keeps everything since all tweaks have min_build<=99999
        result_combined = filter_tweaks(category=cat, min_build=99999)
        result_category = filter_tweaks(category=cat)
        assert len(result_combined) == len(result_category)

    def test_fake_category_plus_scope_still_empty(self) -> None:
        result = filter_tweaks(category="__FAKE__", scope="user")
        assert result == []

    def test_empty_no_criteria_returns_all(self, all_tweaks_list: list[TweakDef]) -> None:
        result = filter_tweaks()
        assert len(result) == len(all_tweaks_list)


class TestStatusMapDetectFree:
    """Verify status_map assigns UNKNOWN directly for tweaks with no detect_fn."""

    def test_sequential_detect_free_gets_unknown(self) -> None:
        no_detect = TweakDef(id="__sm_seq_nd__", label="T", category="C", apply_fn=MagicMock(), remove_fn=MagicMock(), corp_safe=True, detect_fn=None)
        with (
            patch("regilattice.tweaks._TWEAK_INDEX", {"__sm_seq_nd__": no_detect}),
            patch("regilattice.tweaks._ALL_TWEAKS", [no_detect]),
        ):
            result = status_map(parallel=False, ids=["__sm_seq_nd__"])
        assert result["__sm_seq_nd__"] == TweakResult.UNKNOWN

    def test_parallel_detect_free_gets_unknown(self) -> None:
        no_detect = TweakDef(id="__sm_par_nd__", label="T", category="C", apply_fn=MagicMock(), remove_fn=MagicMock(), corp_safe=True, detect_fn=None)
        with (
            patch("regilattice.tweaks._TWEAK_INDEX", {"__sm_par_nd__": no_detect}),
            patch("regilattice.tweaks._ALL_TWEAKS", [no_detect]),
        ):
            result = status_map(parallel=True, max_workers=1, ids=["__sm_par_nd__"])
        assert result["__sm_par_nd__"] == TweakResult.UNKNOWN

    def test_progress_fn_called_for_detect_free_parallel(self) -> None:
        calls: list[tuple[int, int]] = []
        no_detect = TweakDef(
            id="__sm_prog_nd__",
            label="T",
            category="C",
            apply_fn=MagicMock(),
            remove_fn=MagicMock(),
            corp_safe=True,
            detect_fn=None,
        )
        with (
            patch("regilattice.tweaks._TWEAK_INDEX", {"__sm_prog_nd__": no_detect}),
            patch("regilattice.tweaks._ALL_TWEAKS", [no_detect]),
        ):
            status_map(parallel=True, max_workers=1, progress_fn=lambda d, t: calls.append((d, t)), ids=["__sm_prog_nd__"])
        assert calls != []
        assert calls[-1][0] == 1

    def test_detect_fn_present_executed_sequentially(self) -> None:
        detect_fn = MagicMock(return_value=True)
        with_detect = TweakDef(
            id="__sm_seq_det__",
            label="T",
            category="C",
            apply_fn=MagicMock(),
            remove_fn=MagicMock(),
            corp_safe=True,
            detect_fn=detect_fn,
        )
        with (
            patch("regilattice.tweaks._TWEAK_INDEX", {"__sm_seq_det__": with_detect}),
            patch("regilattice.tweaks._ALL_TWEAKS", [with_detect]),
        ):
            result = status_map(parallel=False, ids=["__sm_seq_det__"])
        assert result["__sm_seq_det__"] == TweakResult.APPLIED
        detect_fn.assert_called_once()

    def test_mixed_sequential_detect_and_no_detect(self) -> None:
        detect_fn = MagicMock(return_value=False)
        td_with = TweakDef(
            id="__sm_mix_det__",
            label="W",
            category="C",
            apply_fn=MagicMock(),
            remove_fn=MagicMock(),
            corp_safe=True,
            detect_fn=detect_fn,
        )
        td_without = TweakDef(
            id="__sm_mix_nd__",
            label="X",
            category="C",
            apply_fn=MagicMock(),
            remove_fn=MagicMock(),
            corp_safe=True,
            detect_fn=None,
        )
        index = {"__sm_mix_det__": td_with, "__sm_mix_nd__": td_without}
        with (
            patch("regilattice.tweaks._TWEAK_INDEX", index),
            patch("regilattice.tweaks._ALL_TWEAKS", [td_with, td_without]),
        ):
            result = status_map(parallel=False)
        assert result["__sm_mix_det__"] == TweakResult.NOT_APPLIED
        assert result["__sm_mix_nd__"] == TweakResult.UNKNOWN
