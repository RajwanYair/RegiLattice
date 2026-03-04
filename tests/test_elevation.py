"""Tests for regilattice.elevation — UAC elevation and privilege helpers."""

from __future__ import annotations

import subprocess
from unittest.mock import MagicMock, patch

import pytest

from regilattice.elevation import is_admin, request_elevation, run_elevated

# ── is_admin ─────────────────────────────────────────────────────────────────


class TestIsAdmin:
    """Test admin detection with mocked ctypes."""

    @patch("regilattice.elevation.os.name", "nt")
    @patch("regilattice.elevation.ctypes")
    def test_admin_on_windows(self, mock_ctypes: MagicMock) -> None:
        mock_ctypes.windll.shell32.IsUserAnAdmin.return_value = 1
        assert is_admin() is True

    @patch("regilattice.elevation.os.name", "nt")
    @patch("regilattice.elevation.ctypes")
    def test_not_admin_on_windows(self, mock_ctypes: MagicMock) -> None:
        mock_ctypes.windll.shell32.IsUserAnAdmin.return_value = 0
        assert is_admin() is False

    @patch("regilattice.elevation.os.name", "nt")
    @patch("regilattice.elevation.ctypes")
    def test_exception_returns_false(self, mock_ctypes: MagicMock) -> None:
        mock_ctypes.windll.shell32.IsUserAnAdmin.side_effect = Exception("fail")
        assert is_admin() is False

    @patch("regilattice.elevation.os.name", "posix")
    @patch("regilattice.elevation.os.getuid", return_value=0, create=True)
    def test_root_on_posix(self, _getuid: MagicMock) -> None:
        assert is_admin() is True

    @patch("regilattice.elevation.os.name", "posix")
    @patch("regilattice.elevation.os.getuid", return_value=1000, create=True)
    def test_non_root_on_posix(self, _getuid: MagicMock) -> None:
        assert is_admin() is False


# ── request_elevation ────────────────────────────────────────────────────────


class TestRequestElevation:
    """Test UAC re-launch logic."""

    @patch("regilattice.elevation.is_admin", return_value=True)
    def test_already_admin_returns_zero(self, _mock: MagicMock) -> None:
        assert request_elevation() == 0

    @patch("regilattice.elevation.is_admin", return_value=False)
    @patch("regilattice.elevation.os.name", "posix")
    def test_non_windows_returns_one(self, _mock: MagicMock) -> None:
        assert request_elevation() == 1

    @patch("regilattice.elevation.is_admin", return_value=False)
    @patch("regilattice.elevation.os.name", "nt")
    @patch("regilattice.elevation.ctypes")
    def test_shell_execute_success(self, mock_ctypes: MagicMock, _mock: MagicMock) -> None:
        mock_ctypes.windll.shell32.ShellExecuteW.return_value = 42  # >32 = success
        result = request_elevation(args=["--gui"])
        assert result == 0
        mock_ctypes.windll.shell32.ShellExecuteW.assert_called_once()

    @patch("regilattice.elevation.is_admin", return_value=False)
    @patch("regilattice.elevation.os.name", "nt")
    @patch("regilattice.elevation.ctypes")
    def test_shell_execute_failure(self, mock_ctypes: MagicMock, _mock: MagicMock) -> None:
        mock_ctypes.windll.shell32.ShellExecuteW.return_value = 5  # <=32 = error
        result = request_elevation(args=["--gui"])
        assert result == 5

    @patch("regilattice.elevation.is_admin", return_value=False)
    @patch("regilattice.elevation.os.name", "nt")
    @patch("regilattice.elevation.ctypes")
    def test_shell_execute_exception(self, mock_ctypes: MagicMock, _mock: MagicMock) -> None:
        mock_ctypes.windll.shell32.ShellExecuteW.side_effect = OSError("denied")
        result = request_elevation(args=["--gui"])
        assert result == 1


# ── run_elevated ─────────────────────────────────────────────────────────────


class TestRunElevated:
    """Test run_elevated with mocked subprocess."""

    @patch("regilattice.elevation.is_admin", return_value=True)
    @patch("regilattice.elevation.subprocess.run")
    def test_admin_runs_directly(self, mock_run: MagicMock, _admin: MagicMock) -> None:
        expected = subprocess.CompletedProcess(args=[], returncode=0, stdout="ok", stderr="")
        mock_run.return_value = expected
        result = run_elevated(["reg", "query", "HKCU"])
        mock_run.assert_called_once_with(
            ["reg", "query", "HKCU"],
            capture_output=True,
            text=True,
            timeout=120,
        )
        assert result.returncode == 0

    @patch("regilattice.elevation.is_admin", return_value=False)
    @patch("regilattice.elevation.subprocess.run")
    def test_non_admin_uses_powershell(self, mock_run: MagicMock, _admin: MagicMock) -> None:
        expected = subprocess.CompletedProcess(args=[], returncode=0, stdout="", stderr="")
        mock_run.return_value = expected
        result = run_elevated(["reg", "query", "HKCU"])
        # Should call powershell with Start-Process -Verb RunAs
        call_args = mock_run.call_args
        cmd_list = call_args[0][0]
        assert cmd_list[0] == "powershell"
        assert "-Verb" in call_args[0][0][-1] or "RunAs" in str(call_args)

    @patch("regilattice.elevation.is_admin", return_value=True)
    @patch("regilattice.elevation.subprocess.run")
    def test_custom_timeout(self, mock_run: MagicMock, _admin: MagicMock) -> None:
        mock_run.return_value = subprocess.CompletedProcess(args=[], returncode=0, stdout="", stderr="")
        run_elevated(["cmd", "/c", "echo"], timeout=30)
        assert mock_run.call_args.kwargs["timeout"] == 30
