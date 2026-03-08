"""Tests for regilattice.analytics — local-only usage analytics."""

from __future__ import annotations

from collections.abc import Generator
from pathlib import Path
from unittest.mock import patch

import pytest

from regilattice import analytics


@pytest.fixture(autouse=True)
def _isolated_analytics(tmp_path: Path) -> Generator[None]:
    """Redirect analytics to a temp dir for test isolation."""
    with (
        patch.object(analytics, "_ANALYTICS_DIR", tmp_path),
        patch.object(analytics, "_ANALYTICS_FILE", tmp_path / "analytics.json"),
    ):
        yield


class TestRecordApply:
    def test_increments_total(self) -> None:
        analytics.record_apply("test-tweak")
        data = analytics.get_stats()
        assert data.total_applies == 1

    def test_tracks_per_tweak(self) -> None:
        analytics.record_apply("tweak-a")
        analytics.record_apply("tweak-a")
        analytics.record_apply("tweak-b")
        data = analytics.get_stats()
        assert data.most_applied["tweak-a"] == 2
        assert data.most_applied["tweak-b"] == 1


class TestRecordRemove:
    def test_increments_total(self) -> None:
        analytics.record_remove("test-tweak")
        data = analytics.get_stats()
        assert data.total_removes == 1

    def test_tracks_per_tweak(self) -> None:
        analytics.record_remove("tweak-x")
        data = analytics.get_stats()
        assert data.most_removed["tweak-x"] == 1


class TestRecordError:
    def test_increments_errors(self) -> None:
        analytics.record_error()
        analytics.record_error()
        data = analytics.get_stats()
        assert data.total_errors == 2


class TestRecordSession:
    def test_increments_sessions(self) -> None:
        analytics.record_session()
        data = analytics.get_stats()
        assert data.total_sessions == 1
        assert data.last_session > 0


class TestTopTweaks:
    def test_returns_sorted(self) -> None:
        analytics.record_apply("a")
        analytics.record_apply("b")
        analytics.record_apply("b")
        analytics.record_apply("c")
        analytics.record_apply("c")
        analytics.record_apply("c")
        top = analytics.top_tweaks(2)
        assert top[0] == ("c", 3)
        assert top[1] == ("b", 2)
        assert len(top) == 2


class TestReset:
    def test_clears_all(self) -> None:
        analytics.record_apply("x")
        analytics.record_session()
        analytics.reset()
        data = analytics.get_stats()
        assert data.total_applies == 0
        assert data.total_sessions == 0


class TestLoadCorrupt:
    def test_corrupt_json_returns_defaults(self, tmp_path: Path) -> None:
        (tmp_path / "analytics.json").write_text("not json{{{", encoding="utf-8")
        data = analytics.get_stats()
        assert data.total_applies == 0


# ── record_error_for + error_stats tests ────────────────────────────────────


class TestRecordErrorFor:
    """Tests for record_error_for() and error_stats()."""

    def test_record_error_for_increments_global(self) -> None:
        analytics.reset()
        analytics.record_error_for("pri-disable-telemetry")
        data = analytics.get_stats()
        assert data.total_errors == 1

    def test_record_error_for_tracks_per_tweak(self) -> None:
        analytics.reset()
        analytics.record_error_for("pri-disable-telemetry")
        analytics.record_error_for("pri-disable-telemetry")
        analytics.record_error_for("net-disable-ipv6")
        data = analytics.get_stats()
        assert data.error_counts["pri-disable-telemetry"] == 2
        assert data.error_counts["net-disable-ipv6"] == 1

    def test_error_stats_returns_dict(self) -> None:
        analytics.reset()
        result = analytics.error_stats()
        assert isinstance(result, dict)

    def test_error_stats_empty_initially(self) -> None:
        analytics.reset()
        assert analytics.error_stats() == {}

    def test_error_stats_reflects_record_error_for(self) -> None:
        analytics.reset()
        analytics.record_error_for("some-tweak")
        analytics.record_error_for("some-tweak")
        stats = analytics.error_stats()
        assert stats["some-tweak"] == 2

    def test_error_stats_returns_copy(self) -> None:
        """Mutating the returned dict should not affect stored analytics."""
        analytics.reset()
        analytics.record_error_for("a-tweak")
        s1 = analytics.error_stats()
        s1["injected"] = 99
        s2 = analytics.error_stats()
        assert "injected" not in s2

    def test_record_error_still_increments_global(self) -> None:
        """Legacy record_error() must still work alongside record_error_for()."""
        analytics.reset()
        analytics.record_error()
        analytics.record_error_for("t1")
        data = analytics.get_stats()
        assert data.total_errors == 2

    def test_error_counts_persist_across_reloads(self) -> None:
        analytics.reset()
        analytics.record_error_for("persistent-tweak")
        reloaded = analytics.get_stats()
        assert reloaded.error_counts.get("persistent-tweak") == 1
