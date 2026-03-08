"""Tests for regilattice.ratings — local tweak rating/feedback system."""

from __future__ import annotations

from collections.abc import Generator
from pathlib import Path
from unittest.mock import patch

import pytest

from regilattice import ratings


@pytest.fixture(autouse=True)
def _isolated_ratings(tmp_path: Path) -> Generator[None]:
    """Redirect ratings to a temp dir for test isolation."""
    with (
        patch.object(ratings, "_RATINGS_DIR", tmp_path),
        patch.object(ratings, "_RATINGS_FILE", tmp_path / "ratings.json"),
    ):
        yield


class TestRateTweak:
    def test_rate_and_retrieve(self) -> None:
        ratings.rate_tweak("test-tweak", 5, "Great tweak!")
        r = ratings.get_rating("test-tweak")
        assert r is not None
        assert r.stars == 5
        assert r.note == "Great tweak!"

    def test_invalid_stars_low(self) -> None:
        with pytest.raises(ValueError, match="Stars must be"):
            ratings.rate_tweak("x", 0)

    def test_invalid_stars_high(self) -> None:
        with pytest.raises(ValueError, match="Stars must be"):
            ratings.rate_tweak("x", 6)

    def test_overwrite_rating(self) -> None:
        ratings.rate_tweak("a", 3)
        ratings.rate_tweak("a", 5)
        r = ratings.get_rating("a")
        assert r is not None
        assert r.stars == 5


class TestGetRating:
    def test_missing_returns_none(self) -> None:
        assert ratings.get_rating("nonexistent") is None


class TestAllRatings:
    def test_returns_all(self) -> None:
        ratings.rate_tweak("a", 4)
        ratings.rate_tweak("b", 2)
        all_r = ratings.all_ratings()
        assert len(all_r) == 2
        assert all_r["a"].stars == 4


class TestRemoveRating:
    def test_remove_existing(self) -> None:
        ratings.rate_tweak("x", 3)
        assert ratings.remove_rating("x") is True
        assert ratings.get_rating("x") is None

    def test_remove_missing(self) -> None:
        assert ratings.remove_rating("nope") is False


class TestTopRated:
    def test_sorted_by_stars(self) -> None:
        ratings.rate_tweak("low", 1)
        ratings.rate_tweak("mid", 3)
        ratings.rate_tweak("high", 5)
        top = ratings.top_rated(2)
        assert top[0][0] == "high"
        assert top[1][0] == "mid"
        assert len(top) == 2


class TestCorruptFile:
    def test_corrupt_returns_empty(self, tmp_path: Path) -> None:
        (tmp_path / "ratings.json").write_text("{invalid", encoding="utf-8")
        assert ratings.all_ratings() == {}


# ── average_rating + rated_count tests ──────────────────────────────────────


class TestAverageRating:
    """Tests for average_rating()."""

    def test_returns_none_when_no_ratings(self) -> None:
        ratings.rate_tweak("__clear__", 3)
        ratings.remove_rating("__clear__")
        # Ensure clean slate
        for tid in list(ratings.all_ratings().keys()):
            ratings.remove_rating(tid)
        assert ratings.average_rating() is None

    def test_single_rating_equals_its_stars(self) -> None:
        for tid in list(ratings.all_ratings().keys()):
            ratings.remove_rating(tid)
        ratings.rate_tweak("solo", 4)
        assert ratings.average_rating() == 4.0
        ratings.remove_rating("solo")

    def test_average_of_multiple_ratings(self) -> None:
        for tid in list(ratings.all_ratings().keys()):
            ratings.remove_rating(tid)
        ratings.rate_tweak("a", 2)
        ratings.rate_tweak("b", 4)
        avg = ratings.average_rating()
        assert avg is not None
        assert abs(avg - 3.0) < 0.001
        ratings.remove_rating("a")
        ratings.remove_rating("b")

    def test_average_between_1_and_5(self) -> None:
        for tid in list(ratings.all_ratings().keys()):
            ratings.remove_rating(tid)
        for i, stars in enumerate([1, 3, 5], 1):
            ratings.rate_tweak(f"tw{i}", stars)
        avg = ratings.average_rating()
        assert avg is not None
        assert 1.0 <= avg <= 5.0
        for i in range(1, 4):
            ratings.remove_rating(f"tw{i}")


class TestRatedCount:
    """Tests for rated_count()."""

    def test_zero_when_empty(self) -> None:
        for tid in list(ratings.all_ratings().keys()):
            ratings.remove_rating(tid)
        assert ratings.rated_count() == 0

    def test_increments_with_each_rating(self) -> None:
        for tid in list(ratings.all_ratings().keys()):
            ratings.remove_rating(tid)
        ratings.rate_tweak("r1", 3)
        assert ratings.rated_count() == 1
        ratings.rate_tweak("r2", 5)
        assert ratings.rated_count() == 2
        ratings.remove_rating("r1")
        ratings.remove_rating("r2")

    def test_decrements_on_remove(self) -> None:
        for tid in list(ratings.all_ratings().keys()):
            ratings.remove_rating(tid)
        ratings.rate_tweak("temp", 4)
        ratings.remove_rating("temp")
        assert ratings.rated_count() == 0

    def test_overwrite_does_not_increase_count(self) -> None:
        for tid in list(ratings.all_ratings().keys()):
            ratings.remove_rating(tid)
        ratings.rate_tweak("overwrite", 3)
        ratings.rate_tweak("overwrite", 5)  # overwrite same ID
        assert ratings.rated_count() == 1
        ratings.remove_rating("overwrite")
