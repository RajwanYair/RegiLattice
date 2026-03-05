"""Tests for regilattice.deps — dependency management and lazy imports."""

from __future__ import annotations

import subprocess
from types import ModuleType
from unittest.mock import MagicMock, patch

import pytest

from regilattice.deps import _ensure_pip, _MissingSentinel, _pip_install, install_package, lazy_import, require

# ── _pip_install ─────────────────────────────────────────────────────────────


class TestPipInstall:
    """Test the low-level pip install helper."""

    @patch("regilattice.deps.subprocess.run")
    def test_success(self, mock_run: MagicMock) -> None:
        mock_run.return_value = subprocess.CompletedProcess(
            args=[], returncode=0, stdout="", stderr="",
        )
        assert _pip_install("some-pkg") is True

    @patch("regilattice.deps.subprocess.run")
    def test_failure(self, mock_run: MagicMock) -> None:
        mock_run.return_value = subprocess.CompletedProcess(
            args=[], returncode=1, stdout="", stderr="error",
        )
        assert _pip_install("bad-pkg") is False

    @patch("regilattice.deps.subprocess.run", side_effect=subprocess.TimeoutExpired(cmd="pip", timeout=120))
    def test_timeout(self, _mock: MagicMock) -> None:
        assert _pip_install("slow-pkg") is False

    @patch("regilattice.deps.subprocess.run", side_effect=FileNotFoundError)
    def test_file_not_found(self, _mock: MagicMock) -> None:
        assert _pip_install("no-pip") is False

    @patch("regilattice.deps.subprocess.run")
    def test_user_flag(self, mock_run: MagicMock) -> None:
        mock_run.return_value = subprocess.CompletedProcess(
            args=[], returncode=0, stdout="", stderr="",
        )
        _pip_install("pkg", user=True)
        cmd = mock_run.call_args[0][0]
        assert "--user" in cmd

    @patch("regilattice.deps.subprocess.run")
    def test_no_user_flag(self, mock_run: MagicMock) -> None:
        mock_run.return_value = subprocess.CompletedProcess(
            args=[], returncode=0, stdout="", stderr="",
        )
        _pip_install("pkg", user=False)
        cmd = mock_run.call_args[0][0]
        assert "--user" not in cmd


# ── _ensure_pip ──────────────────────────────────────────────────────────────


class TestEnsurePip:
    """Test pip bootstrap logic."""

    def test_pip_already_present(self) -> None:
        # importlib.import_module("pip") succeeds → return True
        with patch("importlib.import_module", return_value=MagicMock()) as mock_imp:
            assert _ensure_pip() is True
            mock_imp.assert_called_once_with("pip")

    def test_pip_missing_ensurepip_succeeds(self) -> None:
        mock_ensurepip = MagicMock()

        def _import_side_effect(name: str):
            if name == "pip":
                raise ImportError("no pip")
            return MagicMock()  # Should not be reached

        import sys
        with patch("importlib.import_module", side_effect=_import_side_effect), patch.dict(sys.modules, {"ensurepip": mock_ensurepip}):
            assert _ensure_pip() is True
            mock_ensurepip.bootstrap.assert_called_once()

    def test_pip_missing_ensurepip_fails(self) -> None:
        def _import_side_effect(name: str):
            if name == "pip":
                raise ImportError("no pip")
            return MagicMock()

        import sys

        # Setting module to None in sys.modules makes `import ensurepip` raise ImportError
        with patch("importlib.import_module", side_effect=_import_side_effect), patch.dict(sys.modules, {"ensurepip": None}):
            assert _ensure_pip() is False


# ── install_package ──────────────────────────────────────────────────────────


class TestInstallPackage:
    """Test cascading install strategies."""

    @patch("regilattice.deps._pip_install", return_value=True)
    def test_first_strategy_succeeds(self, mock_pip: MagicMock) -> None:
        assert install_package("pkg") is True
        # Called with user=True first
        mock_pip.assert_called_once_with("pkg", user=True)

    @patch("regilattice.deps._pip_install", side_effect=[False, True])
    def test_second_strategy_succeeds(self, mock_pip: MagicMock) -> None:
        assert install_package("pkg") is True
        assert mock_pip.call_count == 2

    @patch("regilattice.deps._ensure_pip", return_value=True)
    @patch("regilattice.deps._pip_install", side_effect=[False, False, True])
    def test_third_strategy_after_ensure_pip(self, mock_pip: MagicMock, _ep: MagicMock) -> None:
        assert install_package("pkg") is True

    @patch("regilattice.deps._ensure_pip", return_value=False)
    @patch("regilattice.deps._pip_install", return_value=False)
    def test_all_strategies_fail(self, _pip: MagicMock, _ep: MagicMock) -> None:
        assert install_package("pkg") is False


# ── lazy_import ──────────────────────────────────────────────────────────────


class TestLazyImport:
    """Test lazy module importation."""

    def test_import_existing_module(self) -> None:
        mod = lazy_import("json")
        assert isinstance(mod, ModuleType)
        assert hasattr(mod, "dumps")

    def test_import_existing_stdlib(self) -> None:
        mod = lazy_import("os")
        assert isinstance(mod, ModuleType)

    @patch("regilattice.deps.install_package", return_value=False)
    def test_missing_module_auto_install_false(self, _mock: MagicMock) -> None:
        result = lazy_import("__nonexistent_pkg_xyz__", auto_install=False)
        assert isinstance(result, _MissingSentinel)

    @patch("regilattice.deps.install_package", return_value=False)
    def test_missing_module_with_auto_install(self, mock_install: MagicMock) -> None:
        result = lazy_import("__nonexistent_pkg_xyz__")
        mock_install.assert_called_once_with("__nonexistent_pkg_xyz__")
        assert isinstance(result, _MissingSentinel)

    @patch("regilattice.deps.install_package", return_value=False)
    def test_pip_name_override(self, mock_install: MagicMock) -> None:
        lazy_import("__nonexistent__", pip_name="real-package-name")
        mock_install.assert_called_once_with("real-package-name")

    def test_sentinel_raises_on_attribute_access(self) -> None:
        result = lazy_import("__nonexistent_pkg_xyz__", auto_install=False)
        with pytest.raises(ImportError, match="__nonexistent_pkg_xyz__"):
            _ = result.some_attr


# ── require ──────────────────────────────────────────────────────────────────


class TestRequire:
    """Test require() for package availability enforcement."""

    def test_existing_package(self) -> None:
        require("json")  # should not raise

    def test_multiple_existing_packages(self) -> None:
        require("json", "os", "sys")  # should not raise

    @patch("regilattice.deps.install_package", return_value=False)
    def test_missing_package_raises(self, _mock: MagicMock) -> None:
        with pytest.raises(ImportError, match="__fake_missing_pkg__"):
            require("__fake_missing_pkg__")

    def test_missing_but_install_succeeds(self) -> None:
        # Patch install_package first (before import_module mock breaks resolution)
        with patch("regilattice.deps.install_package", return_value=True):
            def _import_side_effect(name: str):
                raise ImportError(f"No module named '{name}'")

            with patch("importlib.import_module", side_effect=_import_side_effect):
                require("__fake__")  # should not raise


# ── _MissingSentinel ─────────────────────────────────────────────────────────


class TestMissingSentinel:
    """Test the _MissingSentinel stub object."""

    def test_raises_import_error_on_attr(self) -> None:
        s = _MissingSentinel("fakepkg")
        with pytest.raises(ImportError, match="fakepkg"):
            _ = s.anything

    def test_error_message_contains_install_hint(self) -> None:
        s = _MissingSentinel("mypkg")
        with pytest.raises(ImportError, match="pip install mypkg"):
            _ = s.foo

    def test_stores_name(self) -> None:
        s = _MissingSentinel("test_name")
        assert s._name == "test_name"

    def test_different_attributes_all_raise(self) -> None:
        s = _MissingSentinel("pkg")
        for attr in ("method", "Class", "CONSTANT", "__special__"):
            # __special__ is handled by object.__getattribute__ for dunder
            # so only test non-dunder attrs
            if not attr.startswith("__"):
                with pytest.raises(ImportError):
                    getattr(s, attr)
