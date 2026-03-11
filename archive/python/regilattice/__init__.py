"""RegiLattice — Windows registry tweak toolkit.

Provides a CLI, interactive menu, tkinter GUI, and Python API for
applying/removing Windows registry tweaks covering performance,
privacy, usability, and shell enhancements.
"""

from __future__ import annotations

import re
from importlib.metadata import PackageNotFoundError, version
from pathlib import Path

__all__ = ["__version__"]

try:
    __version__: str = version("regilattice")
except PackageNotFoundError:  # pragma: no cover — local/editable dev
    # Fall back to reading from pyproject.toml so the version is never "0.0.0-dev"
    _pyproject = Path(__file__).parent.parent / "pyproject.toml"
    if _pyproject.exists():
        _m = re.search(r'^version\s*=\s*"([^"]+)"', _pyproject.read_text(encoding="utf-8"), re.M)
        __version__ = _m.group(1) if _m else "0.0.0-dev"
    else:
        __version__ = "0.0.0-dev"
