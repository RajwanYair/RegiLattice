"""Hover tooltip widget and tooltip-text builder for RegiLattice GUI."""

from __future__ import annotations

import functools
import tkinter as tk

from . import gui_theme as theme
from .corpguard import is_gpo_managed
from .tweaks import TweakDef, TweakResult

# ── Theme aliases ────────────────────────────────────────────────────────────

_CARD_HOVER = theme.CARD_HOVER
_FG = theme.FG
_FONT_SM = theme.FONT_SM


# ── Tooltip widget ───────────────────────────────────────────────────────────


class Tooltip:
    """Hover tooltip that follows the mouse cursor and supports text updates.

    Accepts either a text string or a callable that returns text.
    When a callable is provided, the text is computed lazily on first hover.
    """

    def __init__(self, widget: tk.Widget, text: str | None = None, *, text_fn: object = None) -> None:
        self._widget = widget
        self._text = text or ""
        self._text_fn = text_fn  # Callable[[], str] | None
        self._tip: tk.Toplevel | None = None
        widget.bind("<Enter>", self._show)
        widget.bind("<Leave>", self._hide)
        widget.bind("<Motion>", self._move)

    def update_text(self, text: str) -> None:
        self._text = text
        self._text_fn = None  # clear lazy source on explicit update

    def _resolve_text(self) -> str:
        if self._text_fn is not None and callable(self._text_fn):
            self._text = self._text_fn()
            self._text_fn = None
        return self._text

    def _show(self, event: tk.Event[tk.Misc]) -> None:
        if self._tip:
            return
        x = event.x_root + 14
        y = event.y_root + 10
        self._tip = tw = tk.Toplevel(self._widget)
        tw.wm_overrideredirect(True)
        tw.wm_geometry(f"+{x}+{y}")
        tw.wm_attributes("-topmost", True)
        lbl = tk.Label(
            tw,
            text=self._resolve_text(),
            bg=_CARD_HOVER,
            fg=_FG,
            font=_FONT_SM,
            padx=10,
            pady=6,
            wraplength=440,
            justify="left",
        )
        lbl.pack()

    def _move(self, event: tk.Event[tk.Misc]) -> None:
        if self._tip:
            self._tip.wm_geometry(f"+{event.x_root + 14}+{event.y_root + 10}")

    def _hide(self, _: tk.Event[tk.Misc]) -> None:
        if self._tip:
            self._tip.destroy()
            self._tip = None


# ── Tooltip text helpers ─────────────────────────────────────────────────────


@functools.lru_cache(maxsize=2048)
def parse_description_metadata(description: str) -> tuple[str, str, str, str]:
    """Extract Default/Recommended/Options hints from description text.

    Returns (main_text, default_hint, recommendation_hint, options_hint).
    """
    main_parts: list[str] = []
    default_hint = ""
    rec_hint = ""
    options_hint = ""
    for sentence in description.replace(". ", ".\n").splitlines():
        s = sentence.strip()
        low = s.lower()
        if low.startswith("default:"):
            default_hint = s
        elif low.startswith("recommended:"):
            rec_hint = s
        elif low.startswith("options:") or low.startswith("values:"):
            options_hint = s
        else:
            main_parts.append(s)
    return " ".join(main_parts).strip(), default_hint, rec_hint, options_hint


def has_recommendation(td: TweakDef) -> bool:
    """Return True if the tweak description contains a recommendation."""
    _, _, rec_hint, _ = parse_description_metadata(td.description or "")
    return bool(rec_hint)


def build_tooltip_text(td: TweakDef, status: str | TweakResult) -> str:
    """Build rich tooltip including description, current state, options, and recommendation."""
    parts: list[str] = []

    main_desc, default_hint, rec_hint, options_hint = parse_description_metadata(td.description or "")
    if main_desc:
        parts.append(main_desc)

    parts.append("\u2500" * 40)

    # Status line
    status_labels: dict[str, str] = {
        TweakResult.APPLIED: "\u2714 Currently: APPLIED (tweak is active)",
        TweakResult.NOT_APPLIED: "\u25cb Currently: DEFAULT (tweak is not active)",
        TweakResult.UNKNOWN: "? Currently: UNKNOWN (cannot detect state)",
    }
    parts.append(status_labels.get(status, f"Currently: {status}"))

    # Default behaviour & recommendation
    if default_hint:
        parts.append(f"i {default_hint}")
    if rec_hint:
        parts.append(f"\u2605 {rec_hint}")
    if options_hint:
        parts.append(f"\u2630 {options_hint}")

    parts.append("")

    # GPO status
    if td.registry_keys and is_gpo_managed(td.registry_keys):
        parts.append("\U0001f6e1 Group Policy: MANAGED (value may be overridden by GPO)")

    # Admin / corp info
    if td.needs_admin:
        parts.append("\u26a0 Requires administrator privileges")
    if td.corp_safe:
        parts.append("\u2713 Safe for corporate environments")
    else:
        parts.append("\u26a0 Not recommended for corporate networks")

    # Tags
    if td.tags:
        parts.append(f"Tags: {', '.join(td.tags)}")

    # Dependencies
    if td.depends_on:
        parts.append(f"Depends on: {', '.join(td.depends_on)}")

    # Registry keys hint
    if td.registry_keys:
        parts.append(f"\nRegistry: {td.registry_keys[0]}")
        if len(td.registry_keys) > 1:
            parts.append(f"  (+{len(td.registry_keys) - 1} more key(s))")

    return "\n".join(parts)
