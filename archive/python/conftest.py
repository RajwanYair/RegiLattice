"""Shared fixtures for the RegiLattice test suite."""

from __future__ import annotations

from pathlib import Path

import pytest

from regilattice.registry import RegistrySession
from regilattice.tweaks import TweakDef, all_tweaks


def pytest_configure(config: pytest.Config) -> None:
    """Register custom markers to prevent PytestUnknownMarkWarning."""
    config.addinivalue_line("markers", "unit: marks test as a pure unit test (no I/O or GUI)")
    config.addinivalue_line("markers", "integration: marks test as an integration test (file/OS/network)")
    config.addinivalue_line("markers", "slow: marks test as slow (skipped by default with -m 'not slow')")
    config.addinivalue_line("markers", "windows: marks test as Windows-only")
    config.addinivalue_line("markers", "gui: marks test as requiring a Tk window (auto-timeout via pytest-timeout)")


@pytest.fixture()
def dry_session(tmp_path: Path) -> RegistrySession:
    """A RegistrySession with ``_dry_run=True`` rooted in a temp directory."""
    return RegistrySession(base_dir=tmp_path, _dry_run=True)


@pytest.fixture(scope="session")
def all_tweaks_list() -> list[TweakDef]:
    """Session-scoped cached list of every registered TweakDef."""
    return all_tweaks()
