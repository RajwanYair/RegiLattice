"""Tests for regilattice.registry — session, split_root, helpers."""

from __future__ import annotations

import os
import re
from pathlib import Path
from unittest.mock import MagicMock, patch

import pytest

# Always importable — the module guards non-Windows imports.
from regilattice.registry import (AdminRequirementError, RegistrySession,
                                  _split_root, assert_admin, is_windows,
                                  platform_summary)

# ── RegistrySession tests ───────────────────────────────────────────────────


class TestRegistrySession:
    """Tests for the RegistrySession dataclass."""

    def test_log_creates_file(self, tmp_path: Path) -> None:
        session = RegistrySession(base_dir=tmp_path)
        session.log("hello world")
        log = session.log_path.read_text(encoding="utf-8")
        assert "hello world" in log

    def test_log_appends(self, tmp_path: Path) -> None:
        session = RegistrySession(base_dir=tmp_path)
        session.log("first")
        session.log("second")
        lines = session.log_path.read_text(encoding="utf-8").strip().splitlines()
        assert len(lines) == 2
        assert "first" in lines[0]
        assert "second" in lines[1]

    def test_log_path_is_regilattice_log(self, tmp_path: Path) -> None:
        session = RegistrySession(base_dir=tmp_path)
        assert session.log_path == tmp_path / "RegiLattice.log"

    def test_log_timestamp_format(self, tmp_path: Path) -> None:
        session = RegistrySession(base_dir=tmp_path)
        session.log("check-timestamp")
        text = session.log_path.read_text(encoding="utf-8")
        # Expect "YYYY-MM-DD HH:MM:SS : check-timestamp"
        assert re.search(r"\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2} : check-timestamp", text)


class TestDryRun:
    """Dry-run mode should log but never touch the registry."""

    def test_set_value_dry_run(self, tmp_path: Path) -> None:
        session = RegistrySession(base_dir=tmp_path, _dry_run=True)
        # Should not raise even on non-Windows or without winreg
        session.set_value("HKLM\\Software\\Test", "val", 1, 4)
        log = session.log_path.read_text(encoding="utf-8")
        assert "[DRY-RUN]" in log

    def test_delete_tree_dry_run(self, tmp_path: Path) -> None:
        session = RegistrySession(base_dir=tmp_path, _dry_run=True)
        session.delete_tree("HKLM\\Software\\Test")
        assert "[DRY-RUN]" in session.log_path.read_text(encoding="utf-8")

    def test_delete_value_dry_run(self, tmp_path: Path) -> None:
        session = RegistrySession(base_dir=tmp_path, _dry_run=True)
        session.delete_value("HKLM\\Software\\Test", "val")
        assert "[DRY-RUN]" in session.log_path.read_text(encoding="utf-8")


# ── Path splitting ──────────────────────────────────────────────────────────


@pytest.mark.skipif(not is_windows(), reason="winreg unavailable")
class TestSplitRoot:
    def test_hklm(self) -> None:
        import winreg

        root, subkey = _split_root(r"HKEY_LOCAL_MACHINE\SOFTWARE\Test")
        assert root == winreg.HKEY_LOCAL_MACHINE
        assert subkey == r"SOFTWARE\Test"

    def test_hkcu(self) -> None:
        import winreg

        root, subkey = _split_root(r"HKEY_CURRENT_USER\SOFTWARE\Classes\Foo")
        assert root == winreg.HKEY_CURRENT_USER
        assert subkey == r"SOFTWARE\Classes\Foo"

    def test_hkcr(self) -> None:
        import winreg

        root, subkey = _split_root(r"HKEY_CLASSES_ROOT\*\shell\Foo")
        assert root == winreg.HKEY_CLASSES_ROOT
        assert subkey == r"*\shell\Foo"

    def test_short_alias(self) -> None:
        import winreg

        root, subkey = _split_root(r"HKLM\SOFTWARE\X")
        assert root == winreg.HKEY_LOCAL_MACHINE

    def test_bad_root_raises(self) -> None:
        with pytest.raises(ValueError, match="Unsupported"):
            _split_root(r"BOGUS\key")


# ── Backup ──────────────────────────────────────────────────────────────────


@pytest.mark.skipif(not is_windows(), reason="reg.exe unavailable")
class TestBackup:
    def test_backup_creates_directory(self, tmp_path: Path) -> None:
        session = RegistrySession(base_dir=tmp_path)
        with patch.dict(os.environ, {"OneDrive": str(tmp_path / "od")}):
            path = session.backup(["HKEY_CURRENT_USER\\Environment"], "TestLabel")
        assert path.exists()
        assert "TestLabel" in path.name


# ── Admin assertion ─────────────────────────────────────────────────────────


@pytest.mark.skipif(not is_windows(), reason="ctypes.windll unavailable")
class TestAssertAdmin:
    def test_not_required_passes(self) -> None:
        assert_admin(required=False)  # no-op

    def test_non_admin_raises(self) -> None:
        with patch("ctypes.windll.shell32.IsUserAnAdmin", return_value=0):
            with pytest.raises(AdminRequirementError):
                assert_admin(required=True)


# ── Utility functions ───────────────────────────────────────────────────────


class TestUtilities:
    def test_is_windows(self) -> None:
        result = is_windows()
        assert isinstance(result, bool)

    def test_platform_summary(self) -> None:
        s = platform_summary()
        assert isinstance(s, str)
        assert len(s) > 0
