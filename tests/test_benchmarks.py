"""Performance benchmarks for RegiLattice core operations.

Run with::

    python -m pytest tests/test_benchmarks.py -v --tb=short

These tests assert that critical operations complete within reasonable time
bounds.  They do NOT use pytest-benchmark (no extra dependency); instead they
use simple ``time.perf_counter`` checks.
"""

from __future__ import annotations

import time
from collections.abc import Callable
from typing import Any

import pytest

from regilattice.tweaks import (all_tweaks, categories, search_tweaks,
                                status_map, tweaks_by_category)

# ── Constants ────────────────────────────────────────────────────────────────

_MAX_LOAD_S = 2.0  # plugin loading + all_tweaks()
_MAX_STATUS_S = 60.0  # status_map() — calls detect_fn on every tweak (generous for loaded machines)
_MAX_SEARCH_S = 0.5  # search_tweaks() full scan
_MAX_CATEGORIES_S = 0.2  # categories() enumeration


# ── Helpers ──────────────────────────────────────────────────────────────────


def _timed(fn: Callable[..., Any], *args: Any, **kwargs: Any) -> tuple[Any, float]:
    """Return (result, elapsed_seconds)."""
    t0 = time.perf_counter()
    result = fn(*args, **kwargs)
    return result, time.perf_counter() - t0


# ── Benchmarks ───────────────────────────────────────────────────────────────


class TestPluginLoading:
    def test_all_tweaks_loads_fast(self) -> None:
        tweaks, elapsed = _timed(all_tweaks)
        assert len(tweaks) > 1000
        assert elapsed < _MAX_LOAD_S, f"all_tweaks() took {elapsed:.2f}s (limit {_MAX_LOAD_S}s)"

    def test_categories_fast(self) -> None:
        cats, elapsed = _timed(categories)
        assert len(cats) > 50
        assert elapsed < _MAX_CATEGORIES_S, f"categories() took {elapsed:.2f}s (limit {_MAX_CATEGORIES_S}s)"

    def test_tweaks_by_category_fast(self) -> None:
        result, elapsed = _timed(tweaks_by_category)
        assert len(result) > 0
        assert elapsed < _MAX_CATEGORIES_S, f"tweaks_by_category() took {elapsed:.2f}s"


class TestSearch:
    def test_search_by_keyword_fast(self) -> None:
        result, elapsed = _timed(search_tweaks, "privacy")
        assert len(result) > 0
        assert elapsed < _MAX_SEARCH_S, f"search_tweaks() took {elapsed:.2f}s (limit {_MAX_SEARCH_S}s)"

    def test_search_empty_term_fast(self) -> None:
        _result, elapsed = _timed(search_tweaks, "")
        assert elapsed < _MAX_SEARCH_S


class TestStatusMap:
    def test_status_map_completes(self) -> None:
        # Pass max_workers explicitly to avoid PowerShell hwinfo probe overhead
        smap, elapsed = _timed(status_map, max_workers=8)
        assert len(smap) > 1000
        assert elapsed < _MAX_STATUS_S, f"status_map() took {elapsed:.2f}s (limit {_MAX_STATUS_S}s)"


class TestRepeatedCalls:
    @pytest.mark.parametrize("n", [10, 50])
    def test_all_tweaks_repeated(self, n: int) -> None:
        """Repeated all_tweaks() calls should be near-instant (cached)."""
        # Warm up
        all_tweaks()
        t0 = time.perf_counter()
        for _ in range(n):
            all_tweaks()
        elapsed = time.perf_counter() - t0
        assert elapsed < 1.0, f"{n} repeated all_tweaks() calls took {elapsed:.2f}s"

    @pytest.mark.parametrize("n", [10, 50])
    def test_categories_repeated(self, n: int) -> None:
        categories()
        t0 = time.perf_counter()
        for _ in range(n):
            categories()
        elapsed = time.perf_counter() - t0
        assert elapsed < 1.0, f"{n} repeated categories() calls took {elapsed:.2f}s"
