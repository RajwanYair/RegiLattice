"""Logging helpers for RegiLattice.

Provides a thin wrapper around :mod:`logging` so every module uses the
same hierarchy (``regilattice.*``) with consistent formatting.

Usage::

    from regilattice.logger import get_logger
    log = get_logger(__name__)
    log.info("Something happened")
"""

from __future__ import annotations

import logging
import sys

__all__ = ["configure_logging", "get_logger"]

_ROOT_LOGGER = "regilattice"
_DEFAULT_FORMAT = "%(asctime)s [%(levelname)s] %(name)s: %(message)s"
_SIMPLE_FORMAT = "[%(levelname)s] %(message)s"

_CONFIGURED = False


def configure_logging(
    level: str | int = logging.WARNING,
    *,
    simple: bool = False,
    stream: object = None,
) -> None:
    """Configure the root ``regilattice`` logger.

    Call once early in ``main()`` with the user-supplied ``--log-level``.
    Subsequent calls update the level without adding duplicate handlers.

    Args:
        level: A :mod:`logging` level name or integer (e.g. ``"DEBUG"`` or
               ``logging.DEBUG``).
        simple: If *True*, use the compact single-line format (no timestamp).
        stream: Output stream (defaults to :data:`sys.stderr`).
    """
    global _CONFIGURED

    root = logging.getLogger(_ROOT_LOGGER)

    if isinstance(level, str):
        level = getattr(logging, level.upper(), logging.WARNING)

    root.setLevel(level)

    if _CONFIGURED:
        root.setLevel(level)
        return

    handler = logging.StreamHandler(stream or sys.stderr)
    handler.setLevel(level)
    handler.setFormatter(logging.Formatter(_SIMPLE_FORMAT if simple else _DEFAULT_FORMAT))
    root.addHandler(handler)
    _CONFIGURED = True


def get_logger(name: str) -> logging.Logger:
    """Return a child logger under the ``regilattice`` hierarchy.

    Args:
        name: Usually ``__name__`` of the calling module.

    Returns:
        A :class:`logging.Logger` instance.
    """
    if not name.startswith(_ROOT_LOGGER):
        name = f"{_ROOT_LOGGER}.{name.lstrip('.')}"
    return logging.getLogger(name)
