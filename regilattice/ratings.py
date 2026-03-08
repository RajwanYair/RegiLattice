"""Local tweak rating and feedback system.

Stores user ratings (1-5 stars) and notes per tweak in a local JSON file
at ``~/.regilattice/ratings.json``.  No data is ever sent anywhere.
"""

from __future__ import annotations

import json
from dataclasses import dataclass
from pathlib import Path
from typing import Any

_RATINGS_DIR = Path.home() / ".regilattice"
_RATINGS_FILE = _RATINGS_DIR / "ratings.json"


@dataclass
class TweakRating:
    """A single tweak rating entry."""

    stars: int  # 1-5
    note: str = ""


def _load_all() -> dict[str, Any]:
    """Load all ratings from disk."""
    try:
        raw: object = json.loads(_RATINGS_FILE.read_text(encoding="utf-8"))
        if isinstance(raw, dict):
            return dict(raw)
        return {}
    except (FileNotFoundError, json.JSONDecodeError):
        return {}


def _save_all(data: dict[str, Any]) -> None:
    """Persist all ratings to disk."""
    _RATINGS_DIR.mkdir(parents=True, exist_ok=True)
    _RATINGS_FILE.write_text(json.dumps(data, indent=2), encoding="utf-8")


def rate_tweak(tweak_id: str, stars: int, note: str = "") -> None:
    """Rate a tweak (1-5 stars) with an optional note."""
    if not 1 <= stars <= 5:
        msg = f"Stars must be 1-5, got {stars}"
        raise ValueError(msg)
    data = _load_all()
    data[tweak_id] = {"stars": stars, "note": note}
    _save_all(data)


def _to_rating(entry: Any) -> TweakRating:
    """Convert a raw dict entry to a TweakRating."""
    if isinstance(entry, dict):
        return TweakRating(stars=int(entry.get("stars", 3)), note=str(entry.get("note", "")))
    return TweakRating(stars=3)


def get_rating(tweak_id: str) -> TweakRating | None:
    """Get the rating for a tweak, or None if not rated."""
    data = _load_all()
    entry = data.get(tweak_id)
    if entry is None:
        return None
    return _to_rating(entry)


def all_ratings() -> dict[str, TweakRating]:
    """Return all ratings."""
    data = _load_all()
    return {tid: _to_rating(entry) for tid, entry in data.items() if isinstance(entry, dict)}


def remove_rating(tweak_id: str) -> bool:
    """Remove a rating. Returns True if it existed."""
    data = _load_all()
    if tweak_id not in data:
        return False
    del data[tweak_id]
    _save_all(data)
    return True


def top_rated(n: int = 10) -> list[tuple[str, TweakRating]]:
    """Return the top *n* highest-rated tweaks."""
    ratings = all_ratings()
    return sorted(ratings.items(), key=lambda x: x[1].stars, reverse=True)[:n]


def average_rating() -> float | None:
    """Return the mean star value across all rated tweaks, or None if no ratings."""
    ratings = all_ratings()
    if not ratings:
        return None
    return sum(r.stars for r in ratings.values()) / len(ratings)


def rated_count() -> int:
    """Return the number of tweaks that have been rated."""
    return len(_load_all())
