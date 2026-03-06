"""Tests for regilattice.menu — Menu construction and display."""

from __future__ import annotations

from regilattice.menu import Menu


class TestMenuConstruction:
    def test_categories_populated(self) -> None:
        menu = Menu()
        assert len(menu._categories) > 0

    def test_by_cat_matches_categories(self) -> None:
        menu = Menu()
        for cat in menu._categories:
            assert cat in menu._by_cat
            assert len(menu._by_cat[cat]) > 0

    def test_tweaks_are_tweakdefs(self) -> None:
        from regilattice.tweaks import TweakDef

        menu = Menu()
        for cat_tweaks in menu._by_cat.values():
            for td in cat_tweaks:
                assert isinstance(td, TweakDef)

    def test_each_tweak_has_callable_apply(self) -> None:
        menu = Menu()
        for cat_tweaks in menu._by_cat.values():
            for td in cat_tweaks:
                assert callable(td.apply_fn), f"Tweak '{td.label}' apply_fn is not callable"

    def test_each_tweak_has_callable_remove(self) -> None:
        menu = Menu()
        for cat_tweaks in menu._by_cat.values():
            for td in cat_tweaks:
                assert callable(td.remove_fn), f"Tweak '{td.label}' remove_fn is not callable"
