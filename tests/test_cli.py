"""Tests for turbotweak.cli — argument parsing and action dispatch."""

from __future__ import annotations

from unittest.mock import patch

import pytest

from turbotweak.cli import _actions, main


class TestListActions:
    def test_list_prints_actions(self, capsys) -> None:
        rc = main(["--list"])
        assert rc == 0
        out = capsys.readouterr().out
        assert "apply-performance" in out
        assert "remove-all" in out
        assert "create-restore-point" in out

    def test_actions_are_callable(self) -> None:
        for name, fn in _actions().items():
            assert callable(fn), f"Action '{name}' is not callable"


class TestUnknownAction:
    def test_returns_2(self, capsys) -> None:
        rc = main(["nonexistent-action", "-y"])
        assert rc == 2
        assert "Unknown action" in capsys.readouterr().out


class TestVersion:
    def test_version_flag(self, capsys) -> None:
        with pytest.raises(SystemExit) as exc_info:
            main(["--version"])
        assert exc_info.value.code == 0


class TestAbortByUser:
    def test_abort_returns_1(self, capsys) -> None:
        with patch("builtins.input", return_value="n"):
            rc = main(["apply-performance"])
        assert rc == 1
        assert "Aborted" in capsys.readouterr().out
