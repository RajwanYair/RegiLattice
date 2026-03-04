"""Shared fixtures for the RegiLattice test suite."""

from __future__ import annotations

from pathlib import Path
from typing import List

import pytest

from regilattice.registry import RegistrySession
from regilattice.tweaks import TweakDef, all_tweaks


@pytest.fixture()
def dry_session(tmp_path: Path) -> RegistrySession:
    """A RegistrySession with ``_dry_run=True`` rooted in a temp directory."""
    return RegistrySession(base_dir=tmp_path, _dry_run=True)


@pytest.fixture(scope="session")
def all_tweaks_list() -> List[TweakDef]:
    """Session-scoped cached list of every registered TweakDef."""
    return all_tweaks()
