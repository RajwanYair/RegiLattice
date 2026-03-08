"""Tests for regilattice.registry — session, split_root, helpers."""

from __future__ import annotations

import os
import re
import subprocess
from pathlib import Path
from unittest.mock import MagicMock, patch

import pytest

# Always importable — the module guards non-Windows imports.
from regilattice.registry import AdminRequirementError, RegistrySession, _split_root, assert_admin, is_windows, platform_summary

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

    def test_set_dword_dry_run(self, tmp_path: Path) -> None:
        session = RegistrySession(base_dir=tmp_path, _dry_run=True)
        session.set_dword("HKLM\\Software\\Test", "val", 42)
        log = session.log_path.read_text(encoding="utf-8")
        assert "[DRY-RUN]" in log
        assert "42" in log

    def test_set_string_dry_run(self, tmp_path: Path) -> None:
        session = RegistrySession(base_dir=tmp_path, _dry_run=True)
        session.set_string("HKLM\\Software\\Test", "name", "hello")
        log = session.log_path.read_text(encoding="utf-8")
        assert "[DRY-RUN]" in log
        assert "hello" in log

    def test_restore_backup_dry_run(self, tmp_path: Path) -> None:
        session = RegistrySession(base_dir=tmp_path, _dry_run=True)
        result = session.restore_backup(tmp_path / "fake_backup")
        assert result is True
        log = session.log_path.read_text(encoding="utf-8")
        assert "[DRY-RUN]" in log


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

        root, _subkey = _split_root(r"HKLM\SOFTWARE\X")
        assert root == winreg.HKEY_LOCAL_MACHINE

    def test_bad_root_raises(self) -> None:
        with pytest.raises(ValueError, match="Unsupported"):
            _split_root(r"BOGUS\key")


# ── Backup ──────────────────────────────────────────────────────────────────


@pytest.mark.skipif(not is_windows(), reason="reg.exe unavailable")
class TestBackup:
    def test_backup_creates_directory(self, tmp_path: Path) -> None:
        session = RegistrySession(base_dir=tmp_path)
        with (
            patch.dict(os.environ, {"OneDrive": str(tmp_path / "od")}),
            patch("subprocess.run") as mock_run,
        ):
            mock_run.return_value = subprocess.CompletedProcess(args=[], returncode=0)
            path = session.backup(["HKEY_CURRENT_USER\\Environment"], "TestLabel")
        assert path.exists()
        assert "TestLabel" in path.name
        mock_run.assert_called_once()

    def test_backup_fallback_no_onedrive(self, tmp_path: Path) -> None:
        session = RegistrySession(base_dir=tmp_path)
        with (
            patch.dict(os.environ, {"ONEDRIVE": ""}, clear=False),
            patch("subprocess.run") as mock_run,
        ):
            mock_run.return_value = subprocess.CompletedProcess(args=[], returncode=0)
            path = session.backup(["HKEY_CURRENT_USER\\Test"], "TestNoOD")
        assert path.exists()


# ── Admin assertion ─────────────────────────────────────────────────────────


@pytest.mark.skipif(not is_windows(), reason="ctypes.windll unavailable")
class TestAssertAdmin:
    def test_not_required_passes(self) -> None:
        assert_admin(required=False)  # no-op

    def test_non_admin_raises(self) -> None:
        with patch("ctypes.windll.shell32.IsUserAnAdmin", return_value=0), pytest.raises(AdminRequirementError):
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


# ── RegistrySession read helpers (dry-run / non-Windows) ────────────────────


@pytest.mark.skipif(not is_windows(), reason="winreg unavailable")
class TestRegistryReads:
    """Test read helpers return None for missing keys."""

    def test_read_dword_missing(self, tmp_path: Path) -> None:
        session = RegistrySession(base_dir=tmp_path)
        result = session.read_dword(r"HKEY_CURRENT_USER\Software\__RegiLattice_Test_Missing__", "nope")
        assert result is None

    def test_read_string_missing(self, tmp_path: Path) -> None:
        session = RegistrySession(base_dir=tmp_path)
        result = session.read_string(r"HKEY_CURRENT_USER\Software\__RegiLattice_Test_Missing__", "nope")
        assert result is None

    def test_key_exists_missing(self, tmp_path: Path) -> None:
        session = RegistrySession(base_dir=tmp_path)
        assert session.key_exists(r"HKEY_CURRENT_USER\Software\__RegiLattice_Test_Missing__") is False


# ── Retry logic ──────────────────────────────────────────────────────────────


class TestRetryOnTransient:
    """Test the _retry_on_transient helper."""

    def test_success_no_retry(self) -> None:
        from regilattice.registry import _retry_on_transient

        calls: list[int] = []

        def fn() -> str:
            calls.append(1)
            return "ok"

        assert _retry_on_transient(fn) == "ok"
        assert len(calls) == 1

    def test_transient_winerror_retries(self) -> None:
        from regilattice.registry import _retry_on_transient

        calls: list[int] = []

        def fn() -> str:
            calls.append(1)
            if len(calls) == 1:
                exc = OSError("handle error")
                exc.winerror = 6  # attr set dynamically on Windows
                raise exc
            return "ok"

        with patch("regilattice.registry.time.sleep"):
            assert _retry_on_transient(fn) == "ok"
        assert len(calls) == 2

    def test_non_transient_raises_immediately(self) -> None:
        from regilattice.registry import _retry_on_transient

        calls: list[int] = []

        def fn() -> str:
            calls.append(1)
            raise OSError("permanent")

        with pytest.raises(OSError, match="permanent"):
            _retry_on_transient(fn)
        assert len(calls) == 1

    def test_transient_fails_twice_raises(self) -> None:
        from regilattice.registry import _retry_on_transient

        def fn() -> str:
            exc = OSError("access denied")
            exc.winerror = 5  # attr set dynamically on Windows
            raise exc

        with patch("regilattice.registry.time.sleep"), pytest.raises(OSError, match="access denied"):
            _retry_on_transient(fn)


# ── ReadCacheContext tests ───────────────────────────────────────────────────


class TestReadCacheContext:
    """Tests for the read_cache() context manager."""

    def test_context_enables_cache(self, tmp_path: Path) -> None:
        session = RegistrySession(base_dir=tmp_path, _dry_run=True)
        assert not session._read_cache_enabled
        with session.read_cache():
            assert session._read_cache_enabled
        assert not session._read_cache_enabled

    def test_context_clears_on_exit(self, tmp_path: Path) -> None:
        session = RegistrySession(base_dir=tmp_path, _dry_run=True)
        with session.read_cache():
            session._read_cache[("p", "n", "dword")] = 42
        assert len(session._read_cache) == 0

    def test_context_clears_on_exception(self, tmp_path: Path) -> None:
        session = RegistrySession(base_dir=tmp_path, _dry_run=True)
        with pytest.raises(ValueError, match="boom"), session.read_cache():
            session._read_cache[("p", "n", "dword")] = 1
            raise ValueError("boom")
        assert not session._read_cache_enabled
        assert len(session._read_cache) == 0

    def test_context_returns_self(self, tmp_path: Path) -> None:
        session = RegistrySession(base_dir=tmp_path, _dry_run=True)
        with session.read_cache() as ctx:
            from regilattice.registry import _ReadCacheContext

            assert isinstance(ctx, _ReadCacheContext)


# ── New C7 method tests (dry-run / non-Windows) ────────────────────────────


class TestNewReadWriteMethods:
    """Tests for read_binary, read_qword, set_binary, set_qword in dry-run mode."""

    def test_set_binary_logs_dry_run(self, tmp_path: Path) -> None:
        session = RegistrySession(base_dir=tmp_path, _dry_run=True)
        session.set_binary(r"HKLM\Software\Test", "blob", b"\xde\xad\xbe\xef")
        log = session.log_path.read_text(encoding="utf-8")
        assert "[DRY-RUN]" in log

    def test_set_qword_logs_dry_run(self, tmp_path: Path) -> None:
        session = RegistrySession(base_dir=tmp_path, _dry_run=True)
        session.set_qword(r"HKLM\Software\Test", "big", 2**40)
        log = session.log_path.read_text(encoding="utf-8")
        assert "[DRY-RUN]" in log

    def test_set_binary_and_set_qword_are_logged_with_key(self, tmp_path: Path) -> None:
        session = RegistrySession(base_dir=tmp_path, _dry_run=True)
        session.set_binary(r"HKLM\Software\BinTest", "data", b"\x00\xff")
        session.set_qword(r"HKLM\Software\QTest", "q", 123456789)
        log = session.log_path.read_text(encoding="utf-8")
        assert "BinTest" in log
        assert "QTest" in log

    @pytest.mark.skipif(not is_windows(), reason="winreg unavailable")
    def test_read_binary_missing_returns_none(self, tmp_path: Path) -> None:
        session = RegistrySession(base_dir=tmp_path)
        result = session.read_binary(r"HKEY_CURRENT_USER\Software\__RL_Missing__", "blob")
        assert result is None

    @pytest.mark.skipif(not is_windows(), reason="winreg unavailable")
    def test_read_qword_missing_returns_none(self, tmp_path: Path) -> None:
        session = RegistrySession(base_dir=tmp_path)
        result = session.read_qword(r"HKEY_CURRENT_USER\Software\__RL_Missing__", "q")
        assert result is None

    @pytest.mark.skipif(not is_windows(), reason="winreg unavailable")
    def test_list_values_missing_returns_empty(self, tmp_path: Path) -> None:
        session = RegistrySession(base_dir=tmp_path)
        result = session.list_values(r"HKEY_CURRENT_USER\Software\__RL_NoSuchKey__")
        assert result == []

    @pytest.mark.skipif(not is_windows(), reason="winreg unavailable")
    def test_list_keys_missing_returns_empty(self, tmp_path: Path) -> None:
        session = RegistrySession(base_dir=tmp_path)
        result = session.list_keys(r"HKEY_CURRENT_USER\Software\__RL_NoSuchKey__")
        assert result == []


# ── _split_root cache coherence ──────────────────────────────────────────────


@pytest.mark.skipif(not is_windows(), reason="winreg unavailable")
class TestSplitRootCache:
    """Verify _split_root is stable and consistent."""

    def test_same_result_on_repeated_calls(self) -> None:
        path = r"HKEY_LOCAL_MACHINE\SOFTWARE\TestRepeat"
        r1 = _split_root(path)
        r2 = _split_root(path)
        assert r1 == r2

    def test_case_insensitive_root_resolution(self) -> None:
        """Both casings resolve to the same root handle (subkey preserves original case)."""
        r1_root, _ = _split_root(r"HKEY_LOCAL_MACHINE\SOFTWARE\X")
        r2_root, _ = _split_root(r"hkey_local_machine\SOFTWARE\X")
        assert r1_root == r2_root

    def test_prefix_list_sorted_longest_first(self) -> None:
        from regilattice.registry import _PREFIX_LIST

        if not _PREFIX_LIST:
            pytest.skip("_PREFIX_LIST empty (non-Windows CI)")
        for i in range(len(_PREFIX_LIST) - 1):
            assert len(_PREFIX_LIST[i][0]) >= len(_PREFIX_LIST[i + 1][0])


# ── C11 Phase 43: list_values / list_keys with mocked winreg ─────────────────


@pytest.mark.skipif(not is_windows(), reason="winreg unavailable")
class TestListValuesWithMock:
    """Verify list_values() correctly maps winreg.EnumValue tuples."""

    def test_list_values_returns_name_value_type_tuples(self, tmp_path: Path) -> None:
        import winreg

        fake_key = MagicMock()
        fake_key.__enter__ = MagicMock(return_value=fake_key)
        fake_key.__exit__ = MagicMock(return_value=False)

        enum_responses: list[object] = [
            ("MyDword", 42, winreg.REG_DWORD),
            ("MyStr", "hello", winreg.REG_SZ),
            OSError("no more"),
        ]

        session = RegistrySession(base_dir=tmp_path)
        with (
            patch("regilattice.registry.winreg.OpenKey", return_value=fake_key),
            patch("regilattice.registry.winreg.EnumValue", side_effect=enum_responses),
        ):
            result = session.list_values(r"HKEY_CURRENT_USER\Software\TestListVal")

        assert len(result) == 2
        assert result[0] == ("MyDword", 42, winreg.REG_DWORD)
        assert result[1] == ("MyStr", "hello", winreg.REG_SZ)

    def test_list_values_empty_key_returns_empty(self, tmp_path: Path) -> None:
        fake_key = MagicMock()
        fake_key.__enter__ = MagicMock(return_value=fake_key)
        fake_key.__exit__ = MagicMock(return_value=False)

        session = RegistrySession(base_dir=tmp_path)
        with (
            patch("regilattice.registry.winreg.OpenKey", return_value=fake_key),
            patch("regilattice.registry.winreg.EnumValue", side_effect=[OSError("empty")]),
        ):
            result = session.list_values(r"HKEY_CURRENT_USER\Software\TestEmpty")

        assert result == []

    def test_list_values_openkey_fails_returns_empty(self, tmp_path: Path) -> None:
        session = RegistrySession(base_dir=tmp_path)
        with patch("regilattice.registry.winreg.OpenKey", side_effect=FileNotFoundError("no key")):
            result = session.list_values(r"HKEY_CURRENT_USER\Software\NoSuchKey999")
        assert result == []


@pytest.mark.skipif(not is_windows(), reason="winreg unavailable")
class TestListKeysWithMock:
    """Verify list_keys() correctly maps winreg.EnumKey results."""

    def test_list_keys_returns_subkey_names(self, tmp_path: Path) -> None:
        fake_key = MagicMock()
        fake_key.__enter__ = MagicMock(return_value=fake_key)
        fake_key.__exit__ = MagicMock(return_value=False)

        enum_responses: list[object] = ["SubA", "SubB", OSError("no more")]

        session = RegistrySession(base_dir=tmp_path)
        with (
            patch("regilattice.registry.winreg.OpenKey", return_value=fake_key),
            patch("regilattice.registry.winreg.EnumKey", side_effect=enum_responses),
        ):
            result = session.list_keys(r"HKEY_CURRENT_USER\Software\TestListKeys")

        assert result == ["SubA", "SubB"]

    def test_list_keys_empty_key_returns_empty(self, tmp_path: Path) -> None:
        fake_key = MagicMock()
        fake_key.__enter__ = MagicMock(return_value=fake_key)
        fake_key.__exit__ = MagicMock(return_value=False)

        session = RegistrySession(base_dir=tmp_path)
        with (
            patch("regilattice.registry.winreg.OpenKey", return_value=fake_key),
            patch("regilattice.registry.winreg.EnumKey", side_effect=[OSError("empty")]),
        ):
            result = session.list_keys(r"HKEY_CURRENT_USER\Software\TestEmpty2")

        assert result == []


# ── C11 Phase 44: set_binary / set_qword call winreg.SetValueEx ──────────────


@pytest.mark.skipif(not is_windows(), reason="winreg unavailable")
class TestSetBinaryQwordCallsWinreg:
    """Verify set_binary / set_qword call SetValueEx with correct type constants."""

    def test_set_binary_uses_reg_binary_type(self, tmp_path: Path) -> None:
        import winreg

        session = RegistrySession(base_dir=tmp_path)
        fake_key = MagicMock()
        fake_key.__enter__ = MagicMock(return_value=fake_key)
        fake_key.__exit__ = MagicMock(return_value=False)

        with (
            patch("regilattice.registry.winreg.CreateKey", return_value=fake_key),
            patch("regilattice.registry.winreg.SetValueEx") as mock_set,
        ):
            session.set_binary(r"HKEY_CURRENT_USER\Software\BinTest", "blob", b"\xde\xad\xbe\xef")

        mock_set.assert_called_once()
        _handle, name, _zero, reg_type, value = mock_set.call_args.args
        assert reg_type == winreg.REG_BINARY
        assert name == "blob"
        assert value == b"\xde\xad\xbe\xef"

    def test_set_qword_uses_reg_qword_type(self, tmp_path: Path) -> None:
        import winreg

        session = RegistrySession(base_dir=tmp_path)
        fake_key = MagicMock()
        fake_key.__enter__ = MagicMock(return_value=fake_key)
        fake_key.__exit__ = MagicMock(return_value=False)

        with (
            patch("regilattice.registry.winreg.CreateKey", return_value=fake_key),
            patch("regilattice.registry.winreg.SetValueEx") as mock_set,
        ):
            session.set_qword(r"HKEY_CURRENT_USER\Software\QTest", "bignum", 2**40)

        mock_set.assert_called_once()
        _handle, name, _zero, reg_type, value = mock_set.call_args.args
        assert reg_type == winreg.REG_QWORD
        assert name == "bignum"
        assert value == 2**40


# ── REG_EXPAND_SZ tests ─────────────────────────────────────────────────────


class TestExpandStringMethods:
    """Tests for set_expand_string and read_expand_string."""

    @pytest.mark.skipif(not is_windows(), reason="registry requires Windows")
    def test_set_expand_string_uses_reg_expand_sz(self, tmp_path: Path) -> None:
        import winreg

        session = RegistrySession(base_dir=tmp_path)
        fake_key = MagicMock()
        fake_key.__enter__ = MagicMock(return_value=fake_key)
        fake_key.__exit__ = MagicMock(return_value=False)
        with (
            patch("regilattice.registry.winreg.CreateKey", return_value=fake_key),
            patch("regilattice.registry.winreg.SetValueEx") as mock_set,
        ):
            session.set_expand_string(r"HKEY_CURRENT_USER\Software\ExpandTest", "TestVal", "%WINDIR%\\test")
        mock_set.assert_called_once()
        _h, _n, _z, reg_type, _v = mock_set.call_args.args
        assert reg_type == winreg.REG_EXPAND_SZ

    @pytest.mark.skipif(not is_windows(), reason="registry requires Windows")
    def test_read_expand_string_returns_str_on_match(self, tmp_path: Path) -> None:
        import winreg

        session = RegistrySession(base_dir=tmp_path)
        fake_key = MagicMock()
        fake_key.__enter__ = MagicMock(return_value=fake_key)
        fake_key.__exit__ = MagicMock(return_value=False)
        with (
            patch("regilattice.registry.winreg.OpenKey", return_value=fake_key),
            patch("regilattice.registry.winreg.QueryValueEx", return_value=("%WINDIR%\\test", winreg.REG_EXPAND_SZ)),
        ):
            result = session.read_expand_string(r"HKEY_CURRENT_USER\Software\ExpandTest", "TestVal")
        assert result == "%WINDIR%\\test"

    @pytest.mark.skipif(not is_windows(), reason="registry requires Windows")
    def test_read_expand_string_returns_none_on_type_mismatch(self, tmp_path: Path) -> None:
        import winreg

        session = RegistrySession(base_dir=tmp_path)
        fake_key = MagicMock()
        fake_key.__enter__ = MagicMock(return_value=fake_key)
        fake_key.__exit__ = MagicMock(return_value=False)
        with (
            patch("regilattice.registry.winreg.OpenKey", return_value=fake_key),
            patch("regilattice.registry.winreg.QueryValueEx", return_value=("value", winreg.REG_DWORD)),
        ):
            result = session.read_expand_string(r"HKEY_CURRENT_USER\Software\ExpandTest", "TestVal")
        assert result is None

    @pytest.mark.skipif(not is_windows(), reason="registry requires Windows")
    def test_read_expand_string_returns_none_on_missing(self, tmp_path: Path) -> None:
        session = RegistrySession(base_dir=tmp_path)
        with patch("regilattice.registry.winreg.OpenKey", side_effect=FileNotFoundError):
            result = session.read_expand_string(r"HKEY_CURRENT_USER\Software\ExpandTest", "Missing")
        assert result is None

    @pytest.mark.skipif(not is_windows(), reason="registry requires Windows")
    def test_read_expand_string_caches_result(self, tmp_path: Path) -> None:
        import winreg

        session = RegistrySession(base_dir=tmp_path)
        fake_key = MagicMock()
        fake_key.__enter__ = MagicMock(return_value=fake_key)
        fake_key.__exit__ = MagicMock(return_value=False)
        with (
            patch("regilattice.registry.winreg.OpenKey", return_value=fake_key) as mock_ok,
            patch("regilattice.registry.winreg.QueryValueEx", return_value=("%SYSTEMROOT%", winreg.REG_EXPAND_SZ)),
        ):
            with session._read_cache_lock:
                session._read_cache_enabled = True
            r1 = session.read_expand_string(r"HKEY_CURRENT_USER\Software\ExpandTest", "CachedVal")
            r2 = session.read_expand_string(r"HKEY_CURRENT_USER\Software\ExpandTest", "CachedVal")
        assert r1 == r2 == "%SYSTEMROOT%"
        # Second call should have hit the cache — OpenKey called once only
        assert mock_ok.call_count == 1


# ── REG_MULTI_SZ tests ──────────────────────────────────────────────────────


class TestMultiSzMethods:
    """Tests for set_multi_sz and read_multi_sz."""

    @pytest.mark.skipif(not is_windows(), reason="registry requires Windows")
    def test_set_multi_sz_uses_reg_multi_sz(self, tmp_path: Path) -> None:
        import winreg

        session = RegistrySession(base_dir=tmp_path)
        fake_key = MagicMock()
        fake_key.__enter__ = MagicMock(return_value=fake_key)
        fake_key.__exit__ = MagicMock(return_value=False)
        with (
            patch("regilattice.registry.winreg.CreateKey", return_value=fake_key),
            patch("regilattice.registry.winreg.SetValueEx") as mock_set,
        ):
            session.set_multi_sz(r"HKEY_CURRENT_USER\Software\MultiTest", "TestMulti", ["alpha", "beta"])
        mock_set.assert_called_once()
        _h, _n, _z, reg_type, _v = mock_set.call_args.args
        assert reg_type == winreg.REG_MULTI_SZ

    @pytest.mark.skipif(not is_windows(), reason="registry requires Windows")
    def test_read_multi_sz_returns_list_on_match(self, tmp_path: Path) -> None:
        import winreg

        session = RegistrySession(base_dir=tmp_path)
        fake_key = MagicMock()
        fake_key.__enter__ = MagicMock(return_value=fake_key)
        fake_key.__exit__ = MagicMock(return_value=False)
        with (
            patch("regilattice.registry.winreg.OpenKey", return_value=fake_key),
            patch("regilattice.registry.winreg.QueryValueEx", return_value=(["alpha", "beta"], winreg.REG_MULTI_SZ)),
        ):
            result = session.read_multi_sz(r"HKEY_CURRENT_USER\Software\MultiTest", "TestMulti")
        assert result == ["alpha", "beta"]

    @pytest.mark.skipif(not is_windows(), reason="registry requires Windows")
    def test_read_multi_sz_returns_none_on_type_mismatch(self, tmp_path: Path) -> None:
        import winreg

        session = RegistrySession(base_dir=tmp_path)
        fake_key = MagicMock()
        fake_key.__enter__ = MagicMock(return_value=fake_key)
        fake_key.__exit__ = MagicMock(return_value=False)
        with (
            patch("regilattice.registry.winreg.OpenKey", return_value=fake_key),
            patch("regilattice.registry.winreg.QueryValueEx", return_value=("not_a_list", 1)),
        ):
            result = session.read_multi_sz(r"HKEY_CURRENT_USER\Software\MultiTest", "TestMulti")
        assert result is None

    @pytest.mark.skipif(not is_windows(), reason="registry requires Windows")
    def test_read_multi_sz_returns_none_on_missing(self, tmp_path: Path) -> None:
        session = RegistrySession(base_dir=tmp_path)
        with patch("regilattice.registry.winreg.OpenKey", side_effect=FileNotFoundError):
            result = session.read_multi_sz(r"HKEY_CURRENT_USER\Software\MultiTest", "Missing")
        assert result is None

    @pytest.mark.skipif(not is_windows(), reason="registry requires Windows")
    def test_read_multi_sz_caches_result(self, tmp_path: Path) -> None:
        import winreg

        session = RegistrySession(base_dir=tmp_path)
        fake_key = MagicMock()
        fake_key.__enter__ = MagicMock(return_value=fake_key)
        fake_key.__exit__ = MagicMock(return_value=False)
        with (
            patch("regilattice.registry.winreg.OpenKey", return_value=fake_key) as mock_ok,
            patch("regilattice.registry.winreg.QueryValueEx", return_value=(["x", "y"], winreg.REG_MULTI_SZ)),
        ):
            with session._read_cache_lock:
                session._read_cache_enabled = True
            r1 = session.read_multi_sz(r"HKEY_CURRENT_USER\Software\MultiTest", "CachedMulti")
            r2 = session.read_multi_sz(r"HKEY_CURRENT_USER\Software\MultiTest", "CachedMulti")
        assert r1 == r2 == ["x", "y"]
        assert mock_ok.call_count == 1


# ── Cache invalidation extended tests ───────────────────────────────────────


class TestCacheInvalidationExtended:
    """Verify _invalidate_cache_for clears binary, qword, expand, multi_sz entries."""

    @pytest.mark.skipif(not is_windows(), reason="registry requires Windows")
    def test_invalidate_clears_binary_cache(self, tmp_path: Path) -> None:
        import winreg

        session = RegistrySession(base_dir=tmp_path)
        path = r"HKEY_CURRENT_USER\Software\InvalTest"
        name = "BinVal"
        with session._read_cache_lock:
            session._read_cache_enabled = True
            session._read_cache[(path, name, "binary")] = b"\x01\x02"
        fake_key = MagicMock()
        fake_key.__enter__ = MagicMock(return_value=fake_key)
        fake_key.__exit__ = MagicMock(return_value=False)
        with (
            patch("regilattice.registry.winreg.CreateKey", return_value=fake_key),
            patch("regilattice.registry.winreg.SetValueEx"),
        ):
            session.set_binary(path, name, b"\x03")
        assert (path, name, "binary") not in session._read_cache

    @pytest.mark.skipif(not is_windows(), reason="registry requires Windows")
    def test_invalidate_clears_qword_cache(self, tmp_path: Path) -> None:
        import winreg

        session = RegistrySession(base_dir=tmp_path)
        path = r"HKEY_CURRENT_USER\Software\InvalTest"
        name = "QwVal"
        with session._read_cache_lock:
            session._read_cache_enabled = True
            session._read_cache[(path, name, "qword")] = 12345
        fake_key = MagicMock()
        fake_key.__enter__ = MagicMock(return_value=fake_key)
        fake_key.__exit__ = MagicMock(return_value=False)
        with (
            patch("regilattice.registry.winreg.CreateKey", return_value=fake_key),
            patch("regilattice.registry.winreg.SetValueEx"),
        ):
            session.set_qword(path, name, 99999)
        assert (path, name, "qword") not in session._read_cache

    @pytest.mark.skipif(not is_windows(), reason="registry requires Windows")
    def test_invalidate_clears_expand_cache(self, tmp_path: Path) -> None:
        session = RegistrySession(base_dir=tmp_path)
        path = r"HKEY_CURRENT_USER\Software\InvalTest"
        name = "ExpandVal"
        with session._read_cache_lock:
            session._read_cache_enabled = True
            session._read_cache[(path, name, "expand")] = "%WINDIR%"
        fake_key = MagicMock()
        fake_key.__enter__ = MagicMock(return_value=fake_key)
        fake_key.__exit__ = MagicMock(return_value=False)
        with (
            patch("regilattice.registry.winreg.CreateKey", return_value=fake_key),
            patch("regilattice.registry.winreg.SetValueEx"),
        ):
            session.set_expand_string(path, name, "%SYSTEMROOT%")
        assert (path, name, "expand") not in session._read_cache

    @pytest.mark.skipif(not is_windows(), reason="registry requires Windows")
    def test_invalidate_clears_multi_sz_cache(self, tmp_path: Path) -> None:
        session = RegistrySession(base_dir=tmp_path)
        path = r"HKEY_CURRENT_USER\Software\InvalTest"
        name = "MultiVal"
        with session._read_cache_lock:
            session._read_cache_enabled = True
            session._read_cache[(path, name, "multi_sz")] = ["old"]
        fake_key = MagicMock()
        fake_key.__enter__ = MagicMock(return_value=fake_key)
        fake_key.__exit__ = MagicMock(return_value=False)
        with (
            patch("regilattice.registry.winreg.CreateKey", return_value=fake_key),
            patch("regilattice.registry.winreg.SetValueEx"),
        ):
            session.set_multi_sz(path, name, ["new1", "new2"])
        assert (path, name, "multi_sz") not in session._read_cache
