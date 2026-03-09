"""Hover tooltip widget and tooltip-text builder for RegiLattice GUI."""

from __future__ import annotations

import functools
import tkinter as tk
from collections.abc import Callable

from . import gui_theme as theme
from .corpguard import is_gpo_managed
from .tweaks import TweakDef, TweakResult

__all__ = [
    "Tooltip",
    "TooltipManager",
    "build_tooltip_text",
    "has_recommendation",
    "parse_description_metadata",
]

# ── Theme aliases ────────────────────────────────────────────────────────────

_CARD_HOVER = theme.CARD_HOVER
_FG = theme.FG
_FONT_SM = theme.FONT_SM


# ── Tooltip singleton (shared Toplevel — avoids create/destroy per hover) ────


class TooltipManager:
    """Application-wide singleton tooltip panel.

    All 1 200+ tweak rows bind to this single manager instead of each
    creating/destroying their own ``tk.Toplevel`` on every hover event.
    The panel is created once (lazily) and merely repositioned + updated
    on each ``show()`` call — eliminating the per-hover Tk widget churn.
    """

    _instance: TooltipManager | None = None

    def __init__(self, root: tk.Tk) -> None:
        self._root = root
        self._tip: tk.Toplevel | None = None
        self._label: tk.Label | None = None
        self._visible = False

    @classmethod
    def get(cls) -> TooltipManager | None:
        """Return the global instance, or None if not yet initialised."""
        return cls._instance

    @classmethod
    def init(cls, root: tk.Tk) -> TooltipManager:
        """Create (idempotent) and return the global instance."""
        if cls._instance is None:
            cls._instance = cls(root)
        return cls._instance

    @classmethod
    def reset(cls) -> None:
        """Destroy the singleton (for testing)."""
        if cls._instance is not None:
            try:
                if cls._instance._tip is not None:
                    cls._instance._tip.destroy()
            except Exception:
                pass
            cls._instance = None

    def _ensure_panel(self) -> None:
        """Lazily create the shared Toplevel panel."""
        if self._tip is not None:
            return
        self._tip = tk.Toplevel(self._root)
        self._tip.wm_overrideredirect(True)
        self._tip.wm_attributes("-topmost", True)
        self._tip.withdraw()  # hidden until first show()
        self._label = tk.Label(
            self._tip,
            text="",
            bg=theme.CARD_HOVER,
            fg=theme.FG,
            font=theme.FONT_SM,
            padx=10,
            pady=6,
            wraplength=440,
            justify="left",
        )
        self._label.pack()

    def show(self, text: str, x: int, y: int) -> None:
        """Display the tooltip panel with *text* at screen position (*x*, *y*)."""
        self._ensure_panel()
        assert self._tip is not None
        assert self._label is not None
        self._label.configure(text=text, bg=theme.CARD_HOVER, fg=theme.FG)
        self._tip.wm_geometry(f"+{x + 14}+{y + 10}")
        self._tip.deiconify()
        self._visible = True

    def move(self, x: int, y: int) -> None:
        """Reposition the visible panel."""
        if self._tip is not None and self._visible:
            self._tip.wm_geometry(f"+{x + 14}+{y + 10}")

    def hide(self) -> None:
        """Hide the panel without destroying it."""
        if self._tip is not None:
            self._tip.withdraw()
        self._visible = False


# ── Tooltip proxy ─────────────────────────────────────────────────────────────


class Tooltip:
    """Lightweight per-row tooltip proxy backed by the shared ``TooltipManager``.

    Accepts either a text string or a callable that returns text.
    When a callable is provided, the text is computed lazily on first hover.
    Falls back to creating its own ``tk.Toplevel`` when the manager is not
    initialised (e.g. in unit tests).
    """

    def __init__(self, widget: tk.Widget, text: str | None = None, *, text_fn: Callable[[], str] | None = None) -> None:
        self._widget = widget
        self._text = text or ""
        self._text_fn = text_fn
        # Fallback private Toplevel — only used when manager not available
        self._tip: tk.Toplevel | None = None
        widget.bind("<Enter>", self._show)
        widget.bind("<Leave>", self._hide)
        widget.bind("<Motion>", self._move)

    def update_text(self, text: str) -> None:
        self._text = text
        self._text_fn = None

    def _resolve_text(self) -> str:
        if self._text_fn is not None:
            self._text = self._text_fn()
            self._text_fn = None
        return self._text

    def _show(self, event: tk.Event[tk.Misc]) -> None:
        text = self._resolve_text()
        mgr = TooltipManager.get()
        if mgr is not None:
            mgr.show(text, event.x_root, event.y_root)
            return
        # Fallback: own Toplevel (test / standalone use)
        if self._tip:
            return
        x = event.x_root + 14
        y = event.y_root + 10
        self._tip = tw = tk.Toplevel(self._widget)
        tw.wm_overrideredirect(True)
        tw.wm_geometry(f"+{x}+{y}")
        tw.wm_attributes("-topmost", True)
        tk.Label(
            tw,
            text=text,
            bg=_CARD_HOVER,
            fg=_FG,
            font=_FONT_SM,
            padx=10,
            pady=6,
            wraplength=440,
            justify="left",
        ).pack()

    def _move(self, event: tk.Event[tk.Misc]) -> None:
        mgr = TooltipManager.get()
        if mgr is not None:
            mgr.move(event.x_root, event.y_root)
            return
        if self._tip:
            self._tip.wm_geometry(f"+{event.x_root + 14}+{event.y_root + 10}")

    def _hide(self, _: tk.Event[tk.Misc]) -> None:
        mgr = TooltipManager.get()
        if mgr is not None:
            mgr.hide()
            return
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

    # Side effects
    if td.side_effects:
        parts.append(f"\u26a0 Side effects: {td.side_effects}")

    # Registry keys hint
    if td.registry_keys:
        parts.append(f"\nRegistry: {td.registry_keys[0]}")
        if len(td.registry_keys) > 1:
            parts.append(f"  (+{len(td.registry_keys) - 1} more key(s))")

    return "\n".join(parts)
