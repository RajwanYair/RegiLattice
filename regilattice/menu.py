"""Interactive category-based menu driven by the plugin tweak registry."""

from __future__ import annotations

import contextlib
import os
import sys

from . import __version__
from .corpguard import CorporateNetworkError, assert_not_corporate, corp_guard_status
from .registry import SESSION, AdminRequirementError, is_windows, platform_summary
from .tweaks import TweakDef, TweakResult, apply_all, categories, remove_all, tweak_status, tweaks_by_category

__all__ = ["Menu", "main"]

# ANSI colour shortcuts
_RST = "\033[0m"
_CYAN = "\033[96m"
_DIM = "\033[90m"
_RED = "\033[91m"
_GRN = "\033[92m"
_YLW = "\033[93m"
_MAG = "\033[95m"
_BLU = "\033[94m"


class Menu:
    """Interactive two-level menu: categories → tweaks within a category."""

    def __init__(self, *, force_corp: bool = False) -> None:
        self._categories = categories()
        self._by_cat = tweaks_by_category()
        self._force_corp = force_corp

    # ── helpers ───────────────────────────────────────────────────────────

    @staticmethod
    def _clear() -> None:
        os.system("cls" if os.name == "nt" else "clear")

    @staticmethod
    def _pause() -> None:
        with contextlib.suppress(EOFError, KeyboardInterrupt):
            input(f"  {_DIM}Press Enter to continue...{_RST}")

    def _header(self, subtitle: str = "") -> None:
        self._clear()
        print()
        print(f"  {_CYAN}\u2554{'═' * 46}\u2557{_RST}")
        print(f"  {_CYAN}\u2551         \u26a1 RegiLattice Launcher \u26a1            \u2551{_RST}")
        print(f"  {_CYAN}\u255a{'═' * 46}\u255d{_RST}")
        print(f"  {_DIM}Version: {__version__} | Platform: {platform_summary()}{_RST}")
        print(f"  {_DIM}Log: {SESSION.log_path}{_RST}")
        corp_info = corp_guard_status()
        if corp_info:
            print(f"  {_RED}\U0001f6d1 Corporate network: {corp_info} \u2014 tweaks blocked{_RST}")
        if subtitle:
            print(f"\n  {_BLU}{subtitle}{_RST}")
        print()

    def _run_single(self, td: TweakDef, mode: str) -> None:
        try:
            assert_not_corporate(force=self._force_corp)
        except CorporateNetworkError as exc:
            print(f"\n  \U0001f6d1 {exc}")
            return
        label = f"{'Apply' if mode == 'apply' else 'Remove'}: {td.label}"
        print(f"\n  \U0001f9e9 Running: {label} ...")
        try:
            fn = td.apply_fn if mode == "apply" else td.remove_fn
            fn()
            print(f"  \u2705 Done. Logged to {SESSION.log_path}")
        except AdminRequirementError as exc:
            print(f"  \u274c {exc}")
        except Exception as exc:
            SESSION.log(f"Error running {label}: {exc}")
            print(f"  \u274c Error: {exc}")

    # ── category list ─────────────────────────────────────────────────────

    def _show_categories(self) -> str:
        """Display categories and return user choice."""
        self._header("Select a category")
        for idx, cat in enumerate(self._categories, 1):
            count = len(self._by_cat.get(cat, []))
            print(f"  [{idx:>2}] {cat}  {_DIM}({count} tweaks){_RST}")
        print()
        print(f"  {_GRN} [A]  Apply All Tweaks{_RST}")
        print(f"  {_YLW} [R]  Remove All Tweaks{_RST}")
        print(f"  {_MAG} [G]  Launch GUI{_RST}")
        print(f"\n  {_DIM} [ 0] Exit{_RST}\n")
        try:
            return input("  Choose category (number), A/R/G, or 0: ").strip()
        except (EOFError, KeyboardInterrupt):
            return "0"

    # ── tweak list within a category ──────────────────────────────────────

    def _show_tweaks(self, cat_name: str) -> None:
        """Display tweaks in a category and handle actions."""
        tweaks = self._by_cat.get(cat_name, [])
        if not tweaks:
            print(f"  {_RED}No tweaks in '{cat_name}'{_RST}")
            return

        while True:
            self._header(f"Category: {cat_name}")
            for idx, td in enumerate(tweaks, 1):
                st = tweak_status(td)
                tag = f"{_GRN}[ON] {_RST}" if st == TweakResult.APPLIED else f"{_DIM}[OFF]{_RST}"
                if st == TweakResult.UNKNOWN:
                    tag = f"{_YLW}[???]{_RST}"
                print(f"  [{idx:>2}] {tag}  {td.label}  {_DIM}({td.id}){_RST}")
            print()
            print(f"  {_GRN} [A]  Apply All in {cat_name}{_RST}")
            print(f"  {_YLW} [R]  Remove All in {cat_name}{_RST}")
            print(f"\n  {_DIM} [ 0] Back to categories{_RST}\n")
            try:
                choice = input("  Choose tweak (number), A/R, or 0: ").strip()
            except (EOFError, KeyboardInterrupt):
                break

            if choice == "0":
                break
            elif choice.upper() == "A":
                try:
                    assert_not_corporate(force=self._force_corp)
                except CorporateNetworkError as exc:
                    print(f"\n  \U0001f6d1 {exc}")
                else:
                    for td in tweaks:
                        self._run_single(td, "apply")
                self._pause()
            elif choice.upper() == "R":
                try:
                    assert_not_corporate(force=self._force_corp)
                except CorporateNetworkError as exc:
                    print(f"\n  \U0001f6d1 {exc}")
                else:
                    for td in tweaks:
                        self._run_single(td, "remove")
                self._pause()
            else:
                try:
                    idx = int(choice) - 1
                    if 0 <= idx < len(tweaks):
                        td = tweaks[idx]
                        st = tweak_status(td)
                        mode = "remove" if st == TweakResult.APPLIED else "apply"
                        self._run_single(td, mode)
                    else:
                        print(f"  {_RED}Invalid selection.{_RST}")
                except ValueError:
                    print(f"  {_RED}Invalid selection. Try again.{_RST}")
                self._pause()

    # ── main loop ─────────────────────────────────────────────────────────

    def loop(self) -> None:
        if not is_windows():
            print(f"\u26a0\ufe0f  Intended for Windows. Detected: {platform_summary()}")
            return

        while True:
            try:
                choice = self._show_categories()
            except (EOFError, KeyboardInterrupt):
                break

            if choice == "0":
                print("\n  \U0001f44b RegiLattice session ended.")
                break
            elif choice.upper() == "A":
                try:
                    assert_not_corporate(force=self._force_corp)
                except CorporateNetworkError as exc:
                    print(f"\n  \U0001f6d1 {exc}")
                else:
                    apply_all(force_corp=self._force_corp)
                    print("  \u2705 All tweaks applied.")
                self._pause()
            elif choice.upper() == "R":
                try:
                    assert_not_corporate(force=self._force_corp)
                except CorporateNetworkError as exc:
                    print(f"\n  \U0001f6d1 {exc}")
                else:
                    remove_all(force_corp=self._force_corp)
                    print("  \u2705 All tweaks removed.")
                self._pause()
            elif choice.upper() == "G":
                from .gui import launch

                launch()
            else:
                try:
                    idx = int(choice) - 1
                    if 0 <= idx < len(self._categories):
                        self._show_tweaks(self._categories[idx])
                    else:
                        print(f"  {_RED}Invalid selection.{_RST}")
                        self._pause()
                except ValueError:
                    print(f"  {_RED}Invalid selection. Try again.{_RST}")
                    self._pause()


def main(argv: list[str] | None = None, *, force_corp: bool = False) -> int:
    """Standalone entry point for the interactive menu."""
    menu = Menu(force_corp=force_corp)
    menu.loop()
    return 0


if __name__ == "__main__":  # pragma: no cover
    sys.exit(main())
