"""Tests for regilattice.menu — Menu construction and display."""

from __future__ import annotations

from regilattice.menu import Menu


class TestMenuConstruction:
    def test_all_items_present(self) -> None:
        menu = Menu()
        assert len(menu._items) == 13

    def test_lookup_keys(self) -> None:
        menu = Menu()
        for item in menu._items:
            assert item.key in menu._lookup

    def test_invalid_choice_prints_error(self, capsys) -> None:
        menu = Menu()
        menu._run("999")
        out = capsys.readouterr().out
        assert "Invalid" in out

    def test_each_item_has_callable_action(self) -> None:
        menu = Menu()
        for item in menu._items:
            assert callable(
                item.action
            ), f"Menu item '{item.label}' action is not callable"
