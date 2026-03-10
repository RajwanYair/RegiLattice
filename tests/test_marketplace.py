"""Tests for regilattice.marketplace — plugin discovery & loading."""

from __future__ import annotations

import hashlib
import json
from pathlib import Path

import pytest

from regilattice.marketplace import (
    PluginMeta,
    _parse_version,
    _sha256_file,
    _verify_checksums,
    _version_ok,
    discover_plugins,
    load_plugin,
    loaded_plugins,
    unload_plugin,
)

# ── Fixtures ─────────────────────────────────────────────────────────────────


@pytest.fixture()
def _isolated_plugins(tmp_path: Path, monkeypatch: pytest.MonkeyPatch) -> Path:
    """Redirect the plugins directory to a temp folder."""
    import regilattice.marketplace as mp

    monkeypatch.setattr(mp, "_PLUGINS_DIR", tmp_path / "plugins")
    monkeypatch.setattr(mp, "_loaded", {})
    return tmp_path / "plugins"


def _make_tweak_module(directory: Path, name: str, tweak_id: str) -> None:
    """Create a minimal .py file that exports TWEAKS."""
    directory.mkdir(parents=True, exist_ok=True)
    code = (
        "from regilattice.tweaks import TweakDef\n"
        f'TWEAKS = [TweakDef(id="{tweak_id}", label="Test", category="Test", '
        "apply_fn=lambda **kw: None, remove_fn=lambda **kw: None, "
        "registry_keys=[])]\n"
    )
    (directory / f"{name}.py").write_text(code, encoding="utf-8")


# ── Version parsing ──────────────────────────────────────────────────────────


class TestVersionParsing:
    def test_simple(self) -> None:
        assert _parse_version("1.2.3") == (1, 2, 3)

    def test_two_parts(self) -> None:
        assert _parse_version("1.0") == (1, 0)

    def test_empty(self) -> None:
        assert _parse_version("") == (0,)

    def test_version_ok_current(self) -> None:
        assert _version_ok("0.0.0")

    def test_version_ok_exact(self) -> None:
        from regilattice import __version__

        assert _version_ok(__version__)


# ── Discovery ────────────────────────────────────────────────────────────────


class TestDiscoverPlugins:
    def test_empty_dir(self, _isolated_plugins: Path) -> None:
        assert discover_plugins() == []

    def test_finds_plugin_with_metadata(self, _isolated_plugins: Path) -> None:
        plugin_dir = _isolated_plugins / "my_pack"
        plugin_dir.mkdir(parents=True)
        meta = {"name": "my_pack", "version": "0.1.0", "author": "Bob"}
        (plugin_dir / "plugin.json").write_text(json.dumps(meta), encoding="utf-8")
        _make_tweak_module(plugin_dir, "tweaks", "test-mp-tweak")

        found = discover_plugins()
        assert len(found) == 1
        assert found[0].name == "my_pack"
        assert found[0].version == "0.1.0"

    def test_finds_bare_plugin(self, _isolated_plugins: Path) -> None:
        plugin_dir = _isolated_plugins / "bare_pack"
        _make_tweak_module(plugin_dir, "tweaks", "test-bare-tweak")

        found = discover_plugins()
        assert len(found) == 1
        assert found[0].name == "bare_pack"

    def test_skips_hidden_dirs(self, _isolated_plugins: Path) -> None:
        hidden = _isolated_plugins / ".hidden"
        _make_tweak_module(hidden, "tweaks", "test-hidden")
        assert discover_plugins() == []


# ── Loading ──────────────────────────────────────────────────────────────────


class TestLoadPlugin:
    def test_load_tweaks(self, _isolated_plugins: Path) -> None:
        plugin_dir = _isolated_plugins / "loader_test"
        _make_tweak_module(plugin_dir, "custom", "test-loader-tweak")
        meta = PluginMeta(name="loader_test", path=plugin_dir)

        tweaks = load_plugin(meta)
        assert len(tweaks) == 1
        assert tweaks[0].id == "test-loader-tweak"

    def test_version_too_new_raises(self, _isolated_plugins: Path) -> None:
        meta = PluginMeta(name="future", min_regilattice="99.0.0", path=_isolated_plugins)
        with pytest.raises(RuntimeError, match="requires RegiLattice"):
            load_plugin(meta)

    def test_loaded_plugins_tracks(self, _isolated_plugins: Path) -> None:
        plugin_dir = _isolated_plugins / "tracked"
        _make_tweak_module(plugin_dir, "tw", "test-tracked")
        meta = PluginMeta(name="tracked", path=plugin_dir)
        load_plugin(meta)

        all_loaded = loaded_plugins()
        assert "tracked" in all_loaded
        assert len(all_loaded["tracked"]) == 1

    def test_unload_plugin(self, _isolated_plugins: Path) -> None:
        plugin_dir = _isolated_plugins / "removable"
        _make_tweak_module(plugin_dir, "tw", "test-removable")
        meta = PluginMeta(name="removable", path=plugin_dir)
        load_plugin(meta)
        assert unload_plugin("removable") is True
        assert unload_plugin("removable") is False
        assert "removable" not in loaded_plugins()

    def test_path_outside_plugins_raises(self, _isolated_plugins: Path, tmp_path: Path) -> None:
        """Plugin path outside the plugins directory must raise RuntimeError (path traversal guard)."""
        outside_dir = tmp_path / "outside"
        outside_dir.mkdir()
        _make_tweak_module(outside_dir, "tw", "evil-traversal-tweak")
        meta = PluginMeta(name="evil", path=outside_dir)
        with pytest.raises(RuntimeError, match="outside the plugins directory"):
            load_plugin(meta)

    def test_path_inside_plugins_loads_ok(self, _isolated_plugins: Path) -> None:
        """Plugin whose path is inside the plugins directory should load normally."""
        plugin_dir = _isolated_plugins / "safe_plugin"
        _make_tweak_module(plugin_dir, "tw", "safe-inside-tweak")
        meta = PluginMeta(name="safe_plugin", path=plugin_dir)
        tweaks = load_plugin(meta)
        assert len(tweaks) == 1
        assert tweaks[0].id == "safe-inside-tweak"


# ── Checksum verification ─────────────────────────────────────────────────────


class TestChecksumVerification:
    def test_sha256_file(self, tmp_path: Path) -> None:
        f = tmp_path / "sample.py"
        f.write_bytes(b"hello world")
        expected = hashlib.sha256(b"hello world").hexdigest()
        assert _sha256_file(f) == expected

    def test_no_checksums_json_warns(self, _isolated_plugins: Path, caplog: pytest.LogCaptureFixture) -> None:
        """Plugin without checksums.json loads with a warning."""
        plugin_dir = _isolated_plugins / "no_manifest"
        _make_tweak_module(plugin_dir, "tw", "no-manifest-tweak")
        import logging

        with caplog.at_level(logging.WARNING, logger="regilattice.marketplace"):
            _verify_checksums(plugin_dir, "no_manifest")
        assert "checksums.json" in caplog.text

    def test_valid_checksums_pass(self, _isolated_plugins: Path) -> None:
        """Plugin with correct SHA-256 hashes loads without error."""
        plugin_dir = _isolated_plugins / "valid_cs"
        _make_tweak_module(plugin_dir, "tw", "valid-cs-tweak")
        py_file = plugin_dir / "tw.py"
        digest = hashlib.sha256(py_file.read_bytes()).hexdigest()
        (plugin_dir / "checksums.json").write_text(
            json.dumps({"sha256": {"tw.py": digest}}),
            encoding="utf-8",
        )
        # Must not raise
        _verify_checksums(plugin_dir, "valid_cs")

    def test_mismatched_hash_raises(self, _isolated_plugins: Path) -> None:
        """A tampered file (hash mismatch) must raise RuntimeError."""
        plugin_dir = _isolated_plugins / "tampered"
        _make_tweak_module(plugin_dir, "tw", "tampered-tweak")
        (plugin_dir / "checksums.json").write_text(
            json.dumps({"sha256": {"tw.py": "0" * 64}}),
            encoding="utf-8",
        )
        with pytest.raises(RuntimeError, match="checksum mismatch"):
            _verify_checksums(plugin_dir, "tampered")

    def test_unlisted_file_warns(self, _isolated_plugins: Path, caplog: pytest.LogCaptureFixture) -> None:
        """A .py file not listed in checksums.json triggers a warning."""
        plugin_dir = _isolated_plugins / "partial_cs"
        _make_tweak_module(plugin_dir, "tw", "partial-cs-tweak")
        # Intentionally empty sha256 dict — file not listed
        (plugin_dir / "checksums.json").write_text(
            json.dumps({"sha256": {}}),
            encoding="utf-8",
        )
        import logging

        with caplog.at_level(logging.WARNING, logger="regilattice.marketplace"):
            _verify_checksums(plugin_dir, "partial_cs")
        assert "not listed in checksums.json" in caplog.text

    def test_bad_checksums_json_raises(self, _isolated_plugins: Path) -> None:
        """Corrupt checksums.json must raise RuntimeError."""
        plugin_dir = _isolated_plugins / "corrupt_cs"
        plugin_dir.mkdir(parents=True, exist_ok=True)
        (plugin_dir / "checksums.json").write_text("not json!!!", encoding="utf-8")
        with pytest.raises(RuntimeError, match=r"failed to read checksums\.json"):
            _verify_checksums(plugin_dir, "corrupt_cs")

    def test_load_plugin_with_valid_checksums(self, _isolated_plugins: Path) -> None:
        """load_plugin integrates checksum verification end-to-end."""
        plugin_dir = _isolated_plugins / "e2e_cs"
        _make_tweak_module(plugin_dir, "tw", "e2e-cs-tweak")
        py_file = plugin_dir / "tw.py"
        digest = hashlib.sha256(py_file.read_bytes()).hexdigest()
        (plugin_dir / "checksums.json").write_text(
            json.dumps({"sha256": {"tw.py": digest}}),
            encoding="utf-8",
        )
        meta = PluginMeta(name="e2e_cs", path=plugin_dir)
        tweaks = load_plugin(meta)
        assert len(tweaks) == 1

    def test_load_plugin_with_bad_checksums_raises(self, _isolated_plugins: Path) -> None:
        """load_plugin raises RuntimeError when a checksum mismatch is detected."""
        plugin_dir = _isolated_plugins / "e2e_bad"
        _make_tweak_module(plugin_dir, "tw", "e2e-bad-tweak")
        (plugin_dir / "checksums.json").write_text(
            json.dumps({"sha256": {"tw.py": "a" * 64}}),
            encoding="utf-8",
        )
        meta = PluginMeta(name="e2e_bad", path=plugin_dir)
        with pytest.raises(RuntimeError, match="checksum mismatch"):
            load_plugin(meta)
