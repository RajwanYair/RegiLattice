"""Theme constants and multi-theme support for the RegiLattice GUI.

Default theme is Catppuccin Mocha (dark).  Available themes can be switched
at runtime via ``set_theme(name)`` — all module-level colour constants are
updated in-place so existing ``from gui_theme import X`` bindings see the
new values after a refresh.
"""

from __future__ import annotations

from typing import TypedDict

# ── Theme data structure ─────────────────────────────────────────────────────


class ThemeDict(TypedDict):
    accent: str
    bg: str
    bg_surface: str
    fg: str
    fg_dim: str
    card_bg: str
    card_bg_alt: str
    card_hover: str
    ok_green: str
    warn_yellow: str
    err_red: str
    purple: str
    header_bg: str
    border: str
    dim_bg: str
    teal: str
    gpo_orange: str
    status_default: str


_THEMES: dict[str, ThemeDict] = {
    "Catppuccin Mocha": {
        "accent": "#89B4FA",
        "bg": "#1E1E2E",
        "bg_surface": "#24273A",
        "fg": "#CDD6F4",
        "fg_dim": "#6C7086",
        "card_bg": "#313244",
        "card_bg_alt": "#2A2B3D",
        "card_hover": "#45475A",
        "ok_green": "#A6E3A1",
        "warn_yellow": "#F9E2AF",
        "err_red": "#F38BA8",
        "purple": "#CBA6F7",
        "header_bg": "#181825",
        "border": "#45475A",
        "dim_bg": "#585B70",
        "teal": "#94E2D5",
        "gpo_orange": "#FAB387",
        "status_default": "#89DCEB",
    },
    "Catppuccin Latte": {
        "accent": "#1E66F5",
        "bg": "#EFF1F5",
        "bg_surface": "#E6E9EF",
        "fg": "#4C4F69",
        "fg_dim": "#8C8FA1",
        "card_bg": "#DCE0E8",
        "card_bg_alt": "#D3D7DF",
        "card_hover": "#CCD0DA",
        "ok_green": "#40A02B",
        "warn_yellow": "#DF8E1D",
        "err_red": "#D20F39",
        "purple": "#8839EF",
        "header_bg": "#DCE0E8",
        "border": "#CCD0DA",
        "dim_bg": "#9CA0B0",
        "teal": "#179299",
        "gpo_orange": "#FE640B",
        "status_default": "#04A5E5",
    },
    "Nord": {
        "accent": "#88C0D0",
        "bg": "#2E3440",
        "bg_surface": "#3B4252",
        "fg": "#ECEFF4",
        "fg_dim": "#7B88A1",
        "card_bg": "#434C5E",
        "card_bg_alt": "#3D4555",
        "card_hover": "#4C566A",
        "ok_green": "#A3BE8C",
        "warn_yellow": "#EBCB8B",
        "err_red": "#BF616A",
        "purple": "#B48EAD",
        "header_bg": "#2E3440",
        "border": "#4C566A",
        "dim_bg": "#616E88",
        "teal": "#8FBCBB",
        "gpo_orange": "#D08770",
        "status_default": "#81A1C1",
    },
    "Dracula": {
        "accent": "#BD93F9",
        "bg": "#282A36",
        "bg_surface": "#2D303E",
        "fg": "#F8F8F2",
        "fg_dim": "#6272A4",
        "card_bg": "#44475A",
        "card_bg_alt": "#3E4152",
        "card_hover": "#515470",
        "ok_green": "#50FA7B",
        "warn_yellow": "#F1FA8C",
        "err_red": "#FF5555",
        "purple": "#BD93F9",
        "header_bg": "#21222C",
        "border": "#44475A",
        "dim_bg": "#6272A4",
        "teal": "#8BE9FD",
        "gpo_orange": "#FFB86C",
        "status_default": "#8BE9FD",
    },
}


# ── Current theme state ──────────────────────────────────────────────────────

_current_theme: str = "Catppuccin Mocha"


def _t() -> ThemeDict:
    return _THEMES[_current_theme]


# ── Module-level colour constants (initially Catppuccin Mocha) ───────────────

ACCENT = _t()["accent"]
BG = _t()["bg"]
BG_SURFACE = _t()["bg_surface"]
FG = _t()["fg"]
FG_DIM = _t()["fg_dim"]
CARD_BG = _t()["card_bg"]
CARD_BG_ALT = _t()["card_bg_alt"]
CARD_HOVER = _t()["card_hover"]
OK_GREEN = _t()["ok_green"]
WARN_YELLOW = _t()["warn_yellow"]
ERR_RED = _t()["err_red"]
PURPLE = _t()["purple"]
HEADER_BG = _t()["header_bg"]
DIM_BG = _t()["dim_bg"]
TEAL = _t()["teal"]
GPO_ORANGE = _t()["gpo_orange"]

STATUS_APPLIED = OK_GREEN
STATUS_NOT_APPLIED = FG_DIM
STATUS_UNKNOWN = WARN_YELLOW
STATUS_CORP_BLOCKED = ERR_RED
STATUS_DEFAULT = _t()["status_default"]

# ── Fonts ────────────────────────────────────────────────────────────────────

FONT = ("Segoe UI", 10)
FONT_BOLD = ("Segoe UI Semibold", 10)
FONT_SM = ("Segoe UI", 9)
FONT_XS = ("Segoe UI", 8)
FONT_XS_BOLD = ("Segoe UI", 8, "bold")
FONT_TITLE = ("Segoe UI Semibold", 16)
FONT_CAT = ("Segoe UI Semibold", 11)


# ── Theme API ────────────────────────────────────────────────────────────────


def available_themes() -> list[str]:
    """Return list of available theme names."""
    return list(_THEMES)


def current_theme() -> str:
    """Return the name of the active theme."""
    return _current_theme


def set_theme(name: str) -> None:
    """Switch the active theme and update all module-level constants."""
    global _current_theme
    global ACCENT, BG, BG_SURFACE, FG, FG_DIM, CARD_BG, CARD_BG_ALT, CARD_HOVER
    global OK_GREEN, WARN_YELLOW, ERR_RED, PURPLE, HEADER_BG, DIM_BG, TEAL, GPO_ORANGE
    global STATUS_APPLIED, STATUS_NOT_APPLIED, STATUS_UNKNOWN, STATUS_CORP_BLOCKED, STATUS_DEFAULT

    if name not in _THEMES:
        msg = f"Unknown theme: {name!r}. Available: {', '.join(_THEMES)}"
        raise ValueError(msg)

    _current_theme = name
    t = _THEMES[name]
    ACCENT = t["accent"]
    BG = t["bg"]
    BG_SURFACE = t["bg_surface"]
    FG = t["fg"]
    FG_DIM = t["fg_dim"]
    CARD_BG = t["card_bg"]
    CARD_BG_ALT = t["card_bg_alt"]
    CARD_HOVER = t["card_hover"]
    OK_GREEN = t["ok_green"]
    WARN_YELLOW = t["warn_yellow"]
    ERR_RED = t["err_red"]
    PURPLE = t["purple"]
    HEADER_BG = t["header_bg"]
    DIM_BG = t["dim_bg"]
    TEAL = t["teal"]
    GPO_ORANGE = t["gpo_orange"]
    STATUS_APPLIED = OK_GREEN
    STATUS_NOT_APPLIED = FG_DIM
    STATUS_UNKNOWN = WARN_YELLOW
    STATUS_CORP_BLOCKED = ERR_RED
    STATUS_DEFAULT = t["status_default"]
