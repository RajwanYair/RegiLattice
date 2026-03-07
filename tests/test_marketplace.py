"""Tests for regilattice.marketplace — plugin discovery & loading."""

from __future__ import annotations

import json
from pathlib import Path

import pytest

from regilattice.marketplace import (
    PluginMeta,
    _parse_version,
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
        'registry_keys=[])]\n'
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
