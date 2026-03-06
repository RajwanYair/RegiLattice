"""Tests for regilattice.cli — argument parsing and action dispatch."""

from __future__ import annotations

from unittest.mock import patch

import pytest

from regilattice.cli import main
from regilattice.tweaks import TweakResult


class TestListFlag:
    def test_list_prints_tweaks(self, capsys) -> None:
        with patch("regilattice.cli.tweak_status", return_value=TweakResult.UNKNOWN):
            rc = main(["--list"])
        assert rc == 0
        out = capsys.readouterr().out
        assert "explorer-show-file-extensions" in out or "telem-disable" in out

    def test_list_shows_header(self, capsys) -> None:
        with patch("regilattice.cli.tweak_status", return_value=TweakResult.UNKNOWN):
            rc = main(["--list"])
        assert rc == 0
        out = capsys.readouterr().out
        assert "ID" in out
        assert "Category" in out


class TestVersion:
    def test_version_flag(self, capsys) -> None:
        with pytest.raises(SystemExit) as exc_info:
            main(["--version"])
        assert exc_info.value.code == 0


class TestAbortByUser:
    def test_abort_returns_1(self, capsys) -> None:
        with patch("regilattice.cli.assert_not_corporate"), patch("builtins.input", return_value="n"):
            rc = main(["apply", "explorer-show-file-extensions"])
        assert rc == 1
        assert "Aborted" in capsys.readouterr().out


class TestSnapshotFlag:
    def test_snapshot_creates_file(self, tmp_path, capsys) -> None:
        path = tmp_path / "snap.json"
        with patch("regilattice.tweaks.save_snapshot") as mock_save:

            def _save(p):
                import json

                p.parent.mkdir(parents=True, exist_ok=True)
                p.write_text(json.dumps({"test": "applied"}))

            mock_save.side_effect = _save
            rc = main(["--snapshot", str(path)])
        assert rc == 0
        assert path.exists()
