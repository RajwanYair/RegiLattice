"""TurboTweak — Windows registry tweak toolkit.

Provides a CLI, interactive menu, tkinter GUI, and Python API for
applying/removing Windows registry tweaks covering performance,
privacy, usability, and shell enhancements.
"""

from __future__ import annotations

from importlib.metadata import PackageNotFoundError, version

__all__ = ["__version__"]

try:
    __version__: str = version("turbotweak")
except PackageNotFoundError:  # pragma: no cover — local/editable dev
    __version__ = "0.0.0-dev"
