"""Tests for regilattice.config — TOML configuration loading."""

from __future__ import annotations

from pathlib import Path
from unittest.mock import patch

from regilattice.config import AppConfig, load_config


class TestAppConfigDefaults:
    def test_defaults(self) -> None:
        cfg = AppConfig()
        assert cfg.force_corp is False
        assert cfg.max_workers == 8
        assert cfg.backup_dir == ""
        assert cfg.auto_backup is True


class TestLoadConfig:
    def test_missing_file_returns_defaults(self, tmp_path: Path) -> None:
        cfg = load_config(tmp_path / "nonexistent.toml")
        assert cfg.force_corp is False
        assert cfg.max_workers == 8

    def test_empty_file(self, tmp_path: Path) -> None:
        f = tmp_path / "empty.toml"
        f.write_bytes(b"")
        cfg = load_config(f)
        assert cfg.force_corp is False

    def test_general_section(self, tmp_path: Path) -> None:
        f = tmp_path / "test.toml"
        f.write_bytes(b"[general]\nforce_corp = true\nmax_workers = 16\n")
        cfg = load_config(f)
        assert cfg.force_corp is True
        assert cfg.max_workers == 16

    def test_backups_section(self, tmp_path: Path) -> None:
        f = tmp_path / "test.toml"
        f.write_bytes(b'[backups]\ndirectory = "/tmp/backups"\nauto_backup = false\n')
        cfg = load_config(f)
        assert cfg.backup_dir == "/tmp/backups"
        assert cfg.auto_backup is False

    def test_full_config(self, tmp_path: Path) -> None:
        f = tmp_path / "test.toml"
        f.write_bytes(
            b"[general]\nforce_corp = true\nmax_workers = 4\n"
            b'[backups]\ndirectory = "C:/Backup"\nauto_backup = true\n'
        )
        cfg = load_config(f)
        assert cfg.force_corp is True
        assert cfg.max_workers == 4
        assert cfg.backup_dir == "C:/Backup"
        assert cfg.auto_backup is True

    def test_invalid_types_ignored(self, tmp_path: Path) -> None:
        f = tmp_path / "test.toml"
        # force_corp should be bool, max_workers should be int
        f.write_bytes(b'[general]\nforce_corp = "yes"\nmax_workers = "many"\n')
        cfg = load_config(f)
        assert cfg.force_corp is False  # default, since "yes" is not bool
        assert cfg.max_workers == 8  # default, since "many" is not int

    def test_no_toml_parser(self, tmp_path: Path) -> None:
        f = tmp_path / "test.toml"
        f.write_bytes(b"[general]\nforce_corp = true\n")
        with patch("regilattice.config._tomllib", None):
            cfg = load_config(f)
        assert cfg.force_corp is False  # defaults when no parser available

    def test_default_path_used_when_none(self) -> None:
        # When path is None, uses _DEFAULT_PATH (which likely doesn't exist)
        cfg = load_config(None)
        assert isinstance(cfg, AppConfig)

    def test_extra_keys_ignored(self, tmp_path: Path) -> None:
        f = tmp_path / "test.toml"
        f.write_bytes(b"[general]\nforce_corp = true\nunknown_key = 42\n[other]\nfoo = 1\n")
        cfg = load_config(f)
        assert cfg.force_corp is True
