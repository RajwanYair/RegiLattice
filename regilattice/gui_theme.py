"""Catppuccin Mocha / Windows 11 dark theme constants for the GUI."""

from __future__ import annotations

# ── Colour palette — Catppuccin Mocha ────────────────────────────────────────

ACCENT = "#89B4FA"  # Blue
BG = "#1E1E2E"  # Base
BG_SURFACE = "#24273A"  # Surface0
FG = "#CDD6F4"  # Text
FG_DIM = "#6C7086"  # Overlay0
CARD_BG = "#313244"  # Surface1
CARD_HOVER = "#45475A"  # Surface2
OK_GREEN = "#A6E3A1"  # Green
WARN_YELLOW = "#F9E2AF"  # Yellow
ERR_RED = "#F38BA8"  # Red
PURPLE = "#CBA6F7"  # Mauve
HEADER_BG = "#181825"  # Crust
BORDER = "#45475A"  # Surface2
DIM_BG = "#585B70"  # Overlay2
TEAL = "#94E2D5"  # Teal
GPO_ORANGE = "#FAB387"  # Peach — Group Policy managed indicator

# ── Status indicator colours ─────────────────────────────────────────────────

STATUS_APPLIED = OK_GREEN
STATUS_NOT_APPLIED = FG_DIM
STATUS_UNKNOWN = WARN_YELLOW
STATUS_CORP_BLOCKED = ERR_RED
STATUS_DEFAULT = "#89DCEB"  # Sky — tweak not in registry (Windows default)

# ── Fonts ────────────────────────────────────────────────────────────────────

FONT = ("Segoe UI", 10)
FONT_BOLD = ("Segoe UI Semibold", 10)
FONT_SM = ("Segoe UI", 9)
FONT_XS = ("Segoe UI", 8)
FONT_XS_BOLD = ("Segoe UI", 8, "bold")
FONT_TITLE = ("Segoe UI Semibold", 16)
FONT_CAT = ("Segoe UI Semibold", 11)
