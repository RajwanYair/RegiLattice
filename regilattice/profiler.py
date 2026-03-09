"""Performance profiling utilities for RegiLattice.

Usage — profile the whole application startup::

    python -m cProfile -o /tmp/rl.prof -m regilattice --gui
    snakeviz /tmp/rl.prof

Usage — wrap a specific callback::

    from regilattice.profiler import profile_fn, timed

    @timed("apply_all")
    def _on_apply_all_clicked(self) -> None:
        ...

    # Or use cProfile directly for a single call:
    result = profile_fn(expensive_fn, args, kwargs,
                        output="expensive.prof", top_n=15)

Usage — time a block::

    from regilattice.profiler import Stopwatch

    with Stopwatch("status_refresh") as sw:
        statuses = status_map(parallel=True)
    #  prints: "[profiler] status_refresh  342.1 ms"
"""

from __future__ import annotations

import cProfile
import functools
import io
import logging
import pstats
import time
from collections.abc import Callable
from pathlib import Path
from typing import Any, TypeVar

__all__ = [
    "SLOW_THRESHOLD_MS",
    "Stopwatch",
    "profile_fn",
    "timed",
]

_LOG = logging.getLogger(__name__)

# Any GUI action slower than this threshold is logged at WARNING level.
SLOW_THRESHOLD_MS: float = 50.0

_F = TypeVar("_F", bound=Callable[..., Any])


# ── cProfile wrapper ─────────────────────────────────────────────────────────


def profile_fn(
    fn: Callable[..., Any],
    args: tuple[Any, ...] = (),
    kwargs: dict[str, Any] | None = None,
    *,
    output: str | Path | None = None,
    top_n: int = 20,
    sort_by: str = "cumulative",
) -> Any:
    """Run *fn* under cProfile and print/save the stats.

    Parameters
    ----------
    fn:
        Callable to profile.
    args, kwargs:
        Arguments forwarded to *fn*.
    output:
        If given, write a binary ``.prof`` file at this path (readable by
        ``snakeviz`` or ``pstats.Stats``).
    top_n:
        Number of top functions to print.
    sort_by:
        pstats sort key — ``"cumulative"`` (total wall time including callees)
        or ``"tottime"`` (time exclusively in the function).

    Returns
    -------
    Any
        The return value of *fn*.

    Example
    -------
    >>> from regilattice.profiler import profile_fn
    >>> result = profile_fn(my_slow_fn, (arg1,), output="out.prof", top_n=15)
    """
    if kwargs is None:
        kwargs = {}

    pr = cProfile.Profile()
    pr.enable()
    try:
        result = fn(*args, **kwargs)
    finally:
        pr.disable()

    if output:
        pr.dump_stats(str(output))
        _LOG.info("[profiler] stats written to %s  (run: snakeviz %s)", output, output)

    buf = io.StringIO()
    ps = pstats.Stats(pr, stream=buf).sort_stats(sort_by)
    ps.print_stats(top_n)
    _LOG.info("[profiler] top-%d functions by %s:\n%s", top_n, sort_by, buf.getvalue())
    return result


# ── Decorator: @timed ────────────────────────────────────────────────────────


def timed(label: str | None = None, threshold_ms: float = SLOW_THRESHOLD_MS) -> Callable[[_F], _F]:
    """Decorator that measures the wall-clock time of a function call.

    Logs at DEBUG normally; escalates to WARNING if execution exceeds
    *threshold_ms*.  Labels the entry with *label* (or the function name).

    Example
    -------
    >>> @timed("on_apply_all")
    ... def _on_apply_all_clicked(self) -> None:
    ...     ...
    """

    def decorator(fn: _F) -> _F:
        _label = label or fn.__qualname__

        @functools.wraps(fn)
        def wrapper(*args: Any, **kwargs: Any) -> Any:
            t0 = time.perf_counter()
            try:
                return fn(*args, **kwargs)
            finally:
                elapsed_ms = (time.perf_counter() - t0) * 1000
                msg = f"[profiler] {_label:<35s} {elapsed_ms:7.1f} ms"
                if elapsed_ms >= threshold_ms:
                    _LOG.warning(msg)
                else:
                    _LOG.debug(msg)

        return wrapper  # type: ignore[return-value]

    return decorator


# ── Context manager: Stopwatch ───────────────────────────────────────────────


class Stopwatch:
    """Context manager that times a code block and logs the result.

    Example
    -------
    >>> with Stopwatch("status_refresh") as sw:
    ...     statuses = heavy_operation()
    >>> print(f"Took {sw.elapsed_ms:.1f} ms")
    """

    def __init__(self, label: str = "block", threshold_ms: float = SLOW_THRESHOLD_MS) -> None:
        self.label = label
        self.threshold_ms = threshold_ms
        self.elapsed_ms: float = 0.0
        self._t0: float = 0.0

    def __enter__(self) -> Stopwatch:
        self._t0 = time.perf_counter()
        return self

    def __exit__(self, *_: object) -> None:
        self.elapsed_ms = (time.perf_counter() - self._t0) * 1000
        msg = f"[profiler] {self.label:<35s} {self.elapsed_ms:7.1f} ms"
        if self.elapsed_ms >= self.threshold_ms:
            _LOG.warning(msg)
        else:
            _LOG.debug(msg)
