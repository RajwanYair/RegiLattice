"""Local-only telemetry-free usage analytics.

Records tweak operations, errors, and session data to a local JSON file
at ``~/.regilattice/analytics.json``.  No data is ever sent anywhere.
"""

from __future__ import annotations

import json
import time
from dataclasses import dataclass, field
from pathlib import Path

_ANALYTICS_DIR = Path.home() / ".regilattice"
_ANALYTICS_FILE = _ANALYTICS_DIR / "analytics.json"


@dataclass
class AnalyticsData:
    """In-memory representation of local analytics."""

    total_applies: int = 0
    total_removes: int = 0
    total_errors: int = 0
    total_sessions: int = 0
    most_applied: dict[str, int] = field(default_factory=dict)
    most_removed: dict[str, int] = field(default_factory=dict)
    error_counts: dict[str, int] = field(default_factory=dict)
    last_session: float = 0.0


def _load() -> AnalyticsData:
    """Load analytics from disk, returning defaults if missing/corrupt."""
    try:
        raw = json.loads(_ANALYTICS_FILE.read_text(encoding="utf-8"))
        return AnalyticsData(
            total_applies=raw.get("total_applies", 0),
            total_removes=raw.get("total_removes", 0),
            total_errors=raw.get("total_errors", 0),
            total_sessions=raw.get("total_sessions", 0),
            most_applied=raw.get("most_applied", {}),
            most_removed=raw.get("most_removed", {}),
            error_counts=raw.get("error_counts", {}),
            last_session=raw.get("last_session", 0.0),
        )
    except (FileNotFoundError, json.JSONDecodeError, KeyError):
        return AnalyticsData()


def _save(data: AnalyticsData) -> None:
    """Persist analytics to disk."""
    _ANALYTICS_DIR.mkdir(parents=True, exist_ok=True)
    payload = {
        "total_applies": data.total_applies,
        "total_removes": data.total_removes,
        "total_errors": data.total_errors,
        "total_sessions": data.total_sessions,
        "most_applied": data.most_applied,
        "most_removed": data.most_removed,
        "error_counts": data.error_counts,
        "last_session": data.last_session,
    }
    _ANALYTICS_FILE.write_text(json.dumps(payload, indent=2), encoding="utf-8")


def record_apply(tweak_id: str) -> None:
    """Record a successful tweak apply."""
    data = _load()
    data.total_applies += 1
    data.most_applied[tweak_id] = data.most_applied.get(tweak_id, 0) + 1
    _save(data)


def record_remove(tweak_id: str) -> None:
    """Record a successful tweak remove."""
    data = _load()
    data.total_removes += 1
    data.most_removed[tweak_id] = data.most_removed.get(tweak_id, 0) + 1
    _save(data)


def record_error() -> None:
    """Record a tweak error (global counter only)."""
    data = _load()
    data.total_errors += 1
    _save(data)


def record_error_for(tweak_id: str) -> None:
    """Record an error for a specific tweak ID and increment global counter."""
    data = _load()
    data.total_errors += 1
    data.error_counts[tweak_id] = data.error_counts.get(tweak_id, 0) + 1
    _save(data)


def error_stats() -> dict[str, int]:
    """Return per-tweak error counts. Keys are tweak IDs, values are counts."""
    return dict(_load().error_counts)


def record_session() -> None:
    """Record a new session start."""
    data = _load()
    data.total_sessions += 1
    data.last_session = time.time()
    _save(data)


def get_stats() -> AnalyticsData:
    """Return current analytics data."""
    return _load()


def top_tweaks(n: int = 10) -> list[tuple[str, int]]:
    """Return the top *n* most-applied tweaks."""
    data = _load()
    return sorted(data.most_applied.items(), key=lambda x: x[1], reverse=True)[:n]


def reset() -> None:
    """Clear all analytics data."""
    _save(AnalyticsData())
