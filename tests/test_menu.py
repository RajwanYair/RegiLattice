"""Tests for regilattice.menu — Menu construction and display."""

from __future__ import annotations

from regilattice.menu import Menu


class TestMenuConstruction:
    def test_tweaks_list_populated(self) -> None:
        menu = Menu()
        assert len(menu._tweaks) > 0

    def test_tweaks_are_tweakdefs(self) -> None:
        from regilattice.tweaks import TweakDef

        menu = Menu()
        for td in menu._tweaks:
            assert isinstance(td, TweakDef)

    def test_each_tweak_has_callable_apply(self) -> None:
        menu = Menu()
        for td in menu._tweaks:
            assert callable(
                td.apply_fn
            ), f"Tweak '{td.label}' apply_fn is not callable"

    def test_each_tweak_has_callable_remove(self) -> None:
        menu = Menu()
        for td in menu._tweaks:
            assert callable(
                td.remove_fn
            ), f"Tweak '{td.label}' remove_fn is not callable"
