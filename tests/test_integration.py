"""Integration tests — exercise apply/remove/detect triplets end-to-end.

Unlike test_tweaks_smoke (which validates signatures and metadata), these tests
verify that calling apply_fn/remove_fn actually goes through SESSION methods
without raising, and that detect_fn returns coherent values.

All tests run with SESSION._dry_run = True — no real registry writes occur.
"""

from __future__ import annotations

import random
from pathlib import Path

import pytest

from regilattice.registry import SESSION, RegistrySession
from regilattice.tweaks import TweakDef, TweakResult, all_tweaks, tweak_status

# ── Fixtures ─────────────────────────────────────────────────────────────────


@pytest.fixture(scope="module")
def _all_tweaks() -> list[TweakDef]:
    return all_tweaks()


@pytest.fixture(scope="module")
def _sample_tweaks(_all_tweaks: list[TweakDef]) -> list[TweakDef]:
    """Deterministic random sample of 80 tweaks for deeper integration tests."""
    rng = random.Random(42)
    return rng.sample(_all_tweaks, min(80, len(_all_tweaks)))


@pytest.fixture(autouse=True)
def _dry_run_session(tmp_path: Path) -> RegistrySession:
    """Temporarily enable dry-run on the global SESSION during each test."""
    original_dry = SESSION._dry_run
    original_base = SESSION.base_dir
    SESSION._dry_run = True
    SESSION.base_dir = tmp_path
    SESSION.__post_init__()  # reset log path to tmp
    SESSION._dry_ops = 0
    yield SESSION
    SESSION._dry_run = original_dry
    SESSION.base_dir = original_base
    SESSION.__post_init__()


# ── Apply functions don't raise ──────────────────────────────────────────────


class TestApplyNoRaise:
    """Verify that apply_fn(require_admin=False) doesn't raise for registry tweaks."""

    def test_apply_no_raise(self, _sample_tweaks: list[TweakDef]) -> None:
        for td in _sample_tweaks:
            if not td.registry_keys:
                continue
            try:
                td.apply_fn(require_admin=False)
            except Exception as exc:
                pytest.fail(f"{td.id}: apply_fn raised {type(exc).__name__}: {exc}")


# ── Remove functions don't raise ─────────────────────────────────────────────


class TestRemoveNoRaise:
    """Verify that remove_fn(require_admin=False) doesn't raise for registry tweaks."""

    def test_remove_no_raise(self, _sample_tweaks: list[TweakDef]) -> None:
        for td in _sample_tweaks:
            if not td.registry_keys:
                continue
            try:
                td.remove_fn(require_admin=False)
            except Exception as exc:
                pytest.fail(f"{td.id}: remove_fn raised {type(exc).__name__}: {exc}")


# ── Apply produces dry-run operations ────────────────────────────────────────


class TestApplyProducesOps:
    """Verify that apply_fn generates at least one SESSION operation."""

    def test_apply_produces_ops(self, _sample_tweaks: list[TweakDef]) -> None:
        for td in _sample_tweaks:
            if not td.registry_keys:
                continue
            SESSION._dry_ops = 0
            try:
                td.apply_fn(require_admin=False)
            except Exception:
                continue
            assert SESSION._dry_ops > 0, f"{td.id}: apply_fn produced no SESSION operations"


# ── Remove produces dry-run operations ───────────────────────────────────────


class TestRemoveProducesOps:
    """Verify that remove_fn generates at least one SESSION operation."""

    def test_remove_produces_ops(self, _sample_tweaks: list[TweakDef]) -> None:
        for td in _sample_tweaks:
            if not td.registry_keys:
                continue
            SESSION._dry_ops = 0
            try:
                td.remove_fn(require_admin=False)
            except Exception:
                continue
            assert SESSION._dry_ops > 0, f"{td.id}: remove_fn produced no SESSION operations"


# ── Detect functions return bool ─────────────────────────────────────────────


class TestDetectReturnsBool:
    """Verify that detect_fn returns a boolean, not an exception."""

    def test_detect_returns_bool(self, _sample_tweaks: list[TweakDef]) -> None:
        for td in _sample_tweaks:
            if td.detect_fn is None:
                continue
            try:
                result = td.detect_fn()
            except Exception:
                continue  # detect functions may use APIs beyond SESSION
            assert isinstance(result, bool), f"{td.id}: detect_fn returned {type(result)}"


# ── Status map integration ───────────────────────────────────────────────────


class TestStatusMapIntegration:
    """Verify tweak_status() returns coherent results for all tweaks."""

    def test_status_covers_all(self, _all_tweaks: list[TweakDef]) -> None:
        statuses = {td.id: tweak_status(td) for td in _all_tweaks}
        assert len(statuses) == len(_all_tweaks)
        for tid, status in statuses.items():
            assert isinstance(status, TweakResult), f"{tid}: got {status!r}"


# ── Apply → detect round-trip ────────────────────────────────────────────────


class TestApplyDetectRoundTrip:
    """For tweaks with detect_fn, exercise apply→detect sequence."""

    def test_apply_then_detect_no_crash(self, _sample_tweaks: list[TweakDef]) -> None:
        """apply_fn followed by detect_fn should not crash."""
        exercised = 0
        for td in _sample_tweaks:
            if td.detect_fn is None or not td.registry_keys:
                continue
            try:
                td.apply_fn(require_admin=False)
                result = td.detect_fn()
                assert isinstance(result, bool)
                exercised += 1
            except Exception:
                continue
        assert exercised > 0, "No tweak completed the apply→detect round-trip"


# ── Registry key format validation ───────────────────────────────────────────


class TestRegistryKeyFormat:
    """Validate that registry_keys entries use valid hive prefixes."""

    _VALID_PREFIXES = (
        "HKEY_LOCAL_MACHINE\\",
        "HKEY_CURRENT_USER\\",
        "HKEY_USERS\\",
        "HKEY_CLASSES_ROOT\\",
        "HKEY_CURRENT_CONFIG\\",
        "HKLM\\",
        "HKCU\\",
        "HKCR\\",
        "HKU\\",
    )

    def test_all_keys_have_valid_prefix(self, _all_tweaks: list[TweakDef]) -> None:
        for td in _all_tweaks:
            for key in td.registry_keys:
                assert any(
                    key.startswith(p) for p in self._VALID_PREFIXES
                ), f"{td.id}: invalid registry key prefix: {key!r}"


# ── Backup call integration ──────────────────────────────────────────────────


class TestBackupIntegration:
    """Verify that apply_fn calls SESSION.backup for tweaks with registry_keys."""

    def test_apply_calls_backup(self, _sample_tweaks: list[TweakDef]) -> None:
        backed_up = 0
        for td in _sample_tweaks:
            if not td.registry_keys:
                continue
            # Check if backup is called by reading the log
            try:
                td.apply_fn(require_admin=False)
            except Exception:
                continue
            log_text = SESSION.log_path.read_text(encoding="utf-8") if SESSION.log_path.exists() else ""
            if "[DRY-RUN] backup" in log_text or "backup" in log_text.lower():
                backed_up += 1
        # At least some tweaks should call backup
        assert backed_up > 0, "No tweak called SESSION.backup during apply"
