"""Tests for regilattice.corpguard — corporate network detection."""
# mypy: disable-error-code="type-arg,no-untyped-def"
# pyright: reportMissingTypeArgument=false

from __future__ import annotations

import subprocess
from unittest.mock import MagicMock, patch

import pytest

from regilattice.registry import is_windows

from regilattice.corpguard import (
    CorporateNetworkError,
    _derive_policy_path,
    _has_group_policy,
    _has_management_agent,
    _has_vpn_adapter,
    _is_azure_ad_joined,
    _is_domain_joined,
    _split_hive,
    assert_not_corporate,
    corp_guard_status,
    is_corporate_network,
    is_gpo_managed,
)

# ── Helpers ──────────────────────────────────────────────────────────────────


def _completed(stdout: str = "", returncode: int = 0) -> subprocess.CompletedProcess:
    """Build a fake CompletedProcess."""
    return subprocess.CompletedProcess(args=[], returncode=returncode, stdout=stdout, stderr="")


# ── _is_domain_joined ───────────────────────────────────────────────────────


class TestIsDomainJoined:
    """Test domain join detection with mocked ctypes and subprocess."""

    @patch("regilattice.corpguard.is_windows", return_value=False)
    def test_non_windows_returns_false(self, _mock: MagicMock) -> None:
        assert _is_domain_joined() is False

    @patch("regilattice.corpguard.is_windows", return_value=True)
    @patch("regilattice.corpguard.subprocess.run")
    def test_ctypes_domain_detected(
        self,
        mock_run: MagicMock,
        _win: MagicMock,
    ) -> None:
        # The ctypes path uses advapi32.GetComputerNameExW which may not be
        # available on all Windows versions. The code has a try/except that
        # falls through to the WMI PowerShell fallback.
        # Here we verify that the WMI fallback detects a domain.
        mock_run.return_value = _completed("True\n")
        assert _is_domain_joined() is True

    @patch("regilattice.corpguard.is_windows", return_value=True)
    @patch("regilattice.corpguard.subprocess.run", return_value=_completed("True\n"))
    def test_wmi_fallback_true(
        self,
        mock_run: MagicMock,
        _win: MagicMock,
    ) -> None:
        assert _is_domain_joined() is True

    @patch("regilattice.corpguard.is_windows", return_value=True)
    @patch("regilattice.corpguard.subprocess.run", return_value=_completed("False\n"))
    def test_no_domain(
        self,
        mock_run: MagicMock,
        _win: MagicMock,
    ) -> None:
        assert _is_domain_joined() is False


# ── _has_vpn_adapter ────────────────────────────────────────────────────────


class TestHasVpnAdapter:
    """Test VPN adapter detection with mocked subprocess."""

    @patch("regilattice.corpguard.is_windows", return_value=False)
    def test_non_windows_returns_false(self, _mock: MagicMock) -> None:
        assert _has_vpn_adapter() is False

    @patch("regilattice.corpguard.is_windows", return_value=True)
    @patch(
        "regilattice.corpguard.subprocess.run",
        return_value=_completed("Intel Wi-Fi 6 AX201\nRealtek USB GbE\n"),
    )
    def test_no_vpn(self, _run: MagicMock, _win: MagicMock) -> None:
        assert _has_vpn_adapter() is False

    @patch("regilattice.corpguard.is_windows", return_value=True)
    @patch(
        "regilattice.corpguard.subprocess.run",
        return_value=_completed("Intel Wi-Fi 6 AX201\nCisco AnyConnect Virtual\n"),
    )
    def test_vpn_detected_cisco(self, _run: MagicMock, _win: MagicMock) -> None:
        assert _has_vpn_adapter() is True

    @patch("regilattice.corpguard.is_windows", return_value=True)
    @patch(
        "regilattice.corpguard.subprocess.run",
        return_value=_completed("Zscaler Network Adapter\n"),
    )
    def test_vpn_detected_zscaler(self, _run: MagicMock, _win: MagicMock) -> None:
        assert _has_vpn_adapter() is True

    @patch("regilattice.corpguard.is_windows", return_value=True)
    @patch("regilattice.corpguard.subprocess.run", side_effect=OSError("cmd not found"))
    def test_subprocess_error_returns_false(self, _run: MagicMock, _win: MagicMock) -> None:
        assert _has_vpn_adapter() is False


# ── _is_azure_ad_joined ─────────────────────────────────────────────────────


class TestIsAzureAdJoined:
    """Test Azure AD detection via mocked dsregcmd."""

    @patch("regilattice.corpguard.is_windows", return_value=False)
    def test_non_windows_returns_false(self, _mock: MagicMock) -> None:
        assert _is_azure_ad_joined() is False

    @patch("regilattice.corpguard.is_windows", return_value=True)
    @patch(
        "regilattice.corpguard.subprocess.run",
        return_value=_completed("AzureAdJoined : YES\nDomainJoined : NO\n"),
    )
    def test_azure_ad_yes(self, _run: MagicMock, _win: MagicMock) -> None:
        assert _is_azure_ad_joined() is True

    @patch("regilattice.corpguard.is_windows", return_value=True)
    @patch(
        "regilattice.corpguard.subprocess.run",
        return_value=_completed("AzureAdJoined : NO\nDomainJoined : NO\n"),
    )
    def test_azure_ad_no(self, _run: MagicMock, _win: MagicMock) -> None:
        assert _is_azure_ad_joined() is False

    @patch("regilattice.corpguard.is_windows", return_value=True)
    @patch(
        "regilattice.corpguard.subprocess.run",
        return_value=_completed("AzureAdJoined : NO\nDomainJoined : YES\n"),
    )
    def test_domain_joined_via_dsregcmd(self, _run: MagicMock, _win: MagicMock) -> None:
        assert _is_azure_ad_joined() is True

    @patch("regilattice.corpguard.is_windows", return_value=True)
    @patch("regilattice.corpguard.subprocess.run", side_effect=FileNotFoundError)
    def test_dsregcmd_missing(self, _run: MagicMock, _win: MagicMock) -> None:
        assert _is_azure_ad_joined() is False


# ── is_corporate_network ────────────────────────────────────────────────────


class TestIsCorporateNetwork:
    """Test the top-level corporate network check."""

    @patch("regilattice.corpguard._has_group_policy", return_value=False)
    @patch("regilattice.corpguard._has_management_agent", return_value=False)
    @patch("regilattice.corpguard._has_vpn_adapter", return_value=False)
    @patch("regilattice.corpguard._is_azure_ad_joined", return_value=False)
    @patch("regilattice.corpguard._is_domain_joined", return_value=False)
    def test_no_corporate_indicators(self, *_mocks: MagicMock) -> None:
        assert is_corporate_network() is False

    @patch("regilattice.corpguard._has_group_policy", return_value=False)
    @patch("regilattice.corpguard._has_management_agent", return_value=False)
    @patch("regilattice.corpguard._has_vpn_adapter", return_value=True)
    @patch("regilattice.corpguard._is_azure_ad_joined", return_value=False)
    @patch("regilattice.corpguard._is_domain_joined", return_value=False)
    def test_vpn_triggers_corp(self, *_mocks: MagicMock) -> None:
        assert is_corporate_network() is True

    @patch("regilattice.corpguard._has_group_policy", return_value=False)
    @patch("regilattice.corpguard._has_management_agent", return_value=False)
    @patch("regilattice.corpguard._has_vpn_adapter", return_value=False)
    @patch("regilattice.corpguard._is_azure_ad_joined", return_value=False)
    @patch("regilattice.corpguard._is_domain_joined", return_value=True)
    def test_domain_triggers_corp(self, *_mocks: MagicMock) -> None:
        assert is_corporate_network() is True

    @patch("regilattice.corpguard._has_group_policy", return_value=False)
    @patch("regilattice.corpguard._has_management_agent", return_value=True)
    @patch("regilattice.corpguard._has_vpn_adapter", return_value=False)
    @patch("regilattice.corpguard._is_azure_ad_joined", return_value=False)
    @patch("regilattice.corpguard._is_domain_joined", return_value=False)
    def test_management_agent_triggers_corp(self, *_mocks: MagicMock) -> None:
        assert is_corporate_network() is True

    @patch("regilattice.corpguard._has_group_policy", return_value=False)
    @patch("regilattice.corpguard._has_management_agent", return_value=False)
    @patch("regilattice.corpguard._has_vpn_adapter", return_value=False)
    @patch("regilattice.corpguard._is_azure_ad_joined", return_value=True)
    @patch("regilattice.corpguard._is_domain_joined", return_value=False)
    def test_azure_ad_triggers_corp(self, *_mocks: MagicMock) -> None:
        assert is_corporate_network() is True

    @patch("regilattice.corpguard._has_group_policy", return_value=True)
    @patch("regilattice.corpguard._has_management_agent", return_value=False)
    @patch("regilattice.corpguard._has_vpn_adapter", return_value=False)
    @patch("regilattice.corpguard._is_azure_ad_joined", return_value=False)
    @patch("regilattice.corpguard._is_domain_joined", return_value=False)
    def test_group_policy_triggers_corp(self, *_mocks: MagicMock) -> None:
        assert is_corporate_network() is True

    @patch("regilattice.corpguard._has_group_policy", return_value=False)
    @patch("regilattice.corpguard._has_management_agent", side_effect=Exception("oops"))
    @patch("regilattice.corpguard._has_vpn_adapter", return_value=False)
    @patch("regilattice.corpguard._is_azure_ad_joined", return_value=False)
    @patch("regilattice.corpguard._is_domain_joined", return_value=False)
    def test_exception_in_check_does_not_crash(self, *_mocks: MagicMock) -> None:
        # Should return False (exception is caught internally)
        assert is_corporate_network() is False


# ── CorporateNetworkError ───────────────────────────────────────────────────


class TestCorporateNetworkError:
    """Test that CorporateNetworkError is the correct type."""

    def test_is_runtime_error(self) -> None:
        assert issubclass(CorporateNetworkError, RuntimeError)

    def test_can_be_raised(self) -> None:
        with pytest.raises(CorporateNetworkError):
            raise CorporateNetworkError("test")


# ── assert_not_corporate ────────────────────────────────────────────────────


class TestAssertNotCorporate:
    """Test assert_not_corporate helper."""

    @patch("regilattice.corpguard.is_corporate_network", return_value=True)
    def test_raises_when_corp(self, _mock: MagicMock) -> None:
        with pytest.raises(CorporateNetworkError):
            assert_not_corporate()

    @patch("regilattice.corpguard.is_corporate_network", return_value=False)
    def test_no_raise_when_not_corp(self, _mock: MagicMock) -> None:
        assert_not_corporate()  # should not raise

    def test_force_bypasses_guard(self) -> None:
        # force=True should never raise regardless of network
        assert_not_corporate(force=True)


# ── corp_guard_status ────────────────────────────────────────────────────────


class TestCorpGuardStatus:
    """Test the human-readable status helper."""

    @patch("regilattice.corpguard.is_windows", return_value=False)
    def test_non_windows_returns_none(self, _mock: MagicMock) -> None:
        assert corp_guard_status() is None

    @patch("regilattice.corpguard._has_group_policy", return_value=False)
    @patch("regilattice.corpguard._has_management_agent", return_value=False)
    @patch("regilattice.corpguard._has_vpn_adapter", return_value=False)
    @patch("regilattice.corpguard._is_azure_ad_joined", return_value=False)
    @patch("regilattice.corpguard._is_domain_joined", return_value=False)
    @patch("regilattice.corpguard.is_windows", return_value=True)
    def test_returns_none_when_clean(self, *_mocks: MagicMock) -> None:
        assert corp_guard_status() is None

    @patch("regilattice.corpguard._has_group_policy", return_value=False)
    @patch("regilattice.corpguard._has_management_agent", return_value=False)
    @patch("regilattice.corpguard._has_vpn_adapter", return_value=True)
    @patch("regilattice.corpguard._is_azure_ad_joined", return_value=False)
    @patch("regilattice.corpguard._is_domain_joined", return_value=True)
    @patch("regilattice.corpguard.is_windows", return_value=True)
    def test_returns_reasons_string(self, *_mocks: MagicMock) -> None:
        status = corp_guard_status()
        assert isinstance(status, str)
        assert "domain" in status.lower()
        assert "VPN" in status


# ── _has_management_agent ────────────────────────────────────────────────────


class TestHasManagementAgent:
    """Test SCCM/Intune management agent detection."""

    @patch("regilattice.corpguard.is_windows", return_value=False)
    def test_non_windows_returns_false(self, _mock: MagicMock) -> None:
        assert _has_management_agent() is False

    @patch("regilattice.corpguard.is_windows", return_value=True)
    @patch("regilattice.corpguard.subprocess.run", return_value=_completed("5.00.9096.1000\n", 0))
    def test_sccm_detected(self, _run: MagicMock, _win: MagicMock) -> None:
        assert _has_management_agent() is True

    @patch("regilattice.corpguard.is_windows", return_value=True)
    @patch("regilattice.corpguard.subprocess.run", return_value=_completed("", 1))
    def test_no_sccm_falls_through(self, _run: MagicMock, _win: MagicMock) -> None:
        # No SCCM, and no winreg Intune key → False
        assert _has_management_agent() is False


# ── _has_group_policy ────────────────────────────────────────────────────────


class TestHasGroupPolicy:
    """Test Group Policy detection."""

    @patch("regilattice.corpguard.is_windows", return_value=False)
    def test_non_windows_returns_false(self, _mock: MagicMock) -> None:
        assert _has_group_policy() is False


# ── _split_hive / _derive_policy_path (Windows only) ────────────────────────


@pytest.mark.skipif(not is_windows(), reason="winreg unavailable")
class TestSplitHive:
    """Test _split_hive registry hive parsing."""

    def test_hklm(self) -> None:
        import winreg

        hive, sub = _split_hive(r"HKEY_LOCAL_MACHINE\SOFTWARE\Test")
        assert hive == winreg.HKEY_LOCAL_MACHINE
        assert sub == r"SOFTWARE\Test"

    def test_hklm_short(self) -> None:
        import winreg

        hive, sub = _split_hive(r"HKLM\SOFTWARE\Test")
        assert hive == winreg.HKEY_LOCAL_MACHINE
        assert sub == r"SOFTWARE\Test"

    def test_hkcu(self) -> None:
        import winreg

        hive, sub = _split_hive(r"HKEY_CURRENT_USER\Software\Test")
        assert hive == winreg.HKEY_CURRENT_USER
        assert sub == r"Software\Test"

    def test_hkcu_short(self) -> None:
        import winreg

        hive, sub = _split_hive(r"HKCU\Software\Test")
        assert hive == winreg.HKEY_CURRENT_USER
        assert sub == r"Software\Test"

    def test_unknown_hive(self) -> None:
        hive, sub = _split_hive(r"BOGUS\Test")
        assert hive is None
        assert sub == r"BOGUS\Test"


@pytest.mark.skipif(not is_windows(), reason="winreg unavailable")
class TestDerivePolicyPath:
    """Test _derive_policy_path conversion."""

    def test_hklm_software(self) -> None:
        result = _derive_policy_path(r"HKLM\SOFTWARE\Microsoft\Edge\Setting")
        assert result == r"HKLM\SOFTWARE\Policies\Microsoft\Edge\Setting"

    def test_hkcu_software(self) -> None:
        result = _derive_policy_path(r"HKCU\Software\Microsoft\Office\16.0")
        assert result == r"HKCU\Software\Policies\Microsoft\Office\16.0"

    def test_non_software_returns_none(self) -> None:
        result = _derive_policy_path(r"HKLM\SYSTEM\CurrentControlSet\Test")
        assert result is None

    def test_full_hklm_prefix(self) -> None:
        result = _derive_policy_path(r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Test")
        assert result == r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Test"


@pytest.mark.skipif(not is_windows(), reason="winreg unavailable")
class TestIsGpoManaged:
    """Test is_gpo_managed registry check."""

    def test_empty_keys(self) -> None:
        assert is_gpo_managed([]) is False

    def test_nonexistent_policy_key(self) -> None:
        assert is_gpo_managed([r"HKLM\SOFTWARE\__RegiLattice_Test_Missing__\Foo"]) is False
