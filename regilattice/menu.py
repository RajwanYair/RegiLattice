"""Interactive menu driven by the plugin tweak registry."""

from __future__ import annotations

import os
import sys

from . import __version__
from .corpguard import CorporateNetworkError, assert_not_corporate, corp_guard_status
from .registry import SESSION, AdminRequirementError, is_windows, platform_summary
from .tweaks import TweakDef, all_tweaks, apply_all, remove_all, tweak_status
from .tweaks.maintenance import create_restore_point


class Menu:
    """Interactive numbered menu auto-built from registered tweaks."""

    def __init__(self) -> None:
        self._tweaks = all_tweaks()

    # -- Display --

    @staticmethod
    def _clear() -> None:
        os.system("cls" if os.name == "nt" else "clear")

    def _banner(self) -> None:
        self._clear()
        rst = "\033[0m"
        cyan = "\033[96m"
        dim = "\033[90m"
        red = "\033[91m"
        grn = "\033[92m"
        ylw = "\033[93m"
        mag = "\033[95m"
        print()
        print(f"  {cyan}╔══════════════════════════════════════════════╗{rst}")
        print(f"  {cyan}║         ⚡ RegiLattice Launcher ⚡            ║{rst}")
        print(f"  {cyan}╚══════════════════════════════════════════════╝{rst}")
        print(f"  {dim}Version: {__version__} | Platform: {platform_summary()}{rst}")
        print(f"  {dim}Log: {SESSION.log_path}{rst}")

        # Corporate network warning
        corp_info = corp_guard_status()
        if corp_info:
            print(f"  {red}🛑 Corporate network: {corp_info} — tweaks blocked{rst}")

        print()
        last_cat = ""
        for idx, td in enumerate(self._tweaks, 1):
            if td.category != last_cat:
                last_cat = td.category
                print(f"  {cyan}── {td.category} ──{rst}")
            status = tweak_status(td)
            tag = f"{grn}[ON] {rst}" if status == "applied" else f"{dim}[OFF]{rst}"
            pad = " " if idx < 10 else ""
            print(f"  {pad}[{idx:>2}] {tag}  {td.label}")

        n = len(self._tweaks)
        print()
        print(f"  {grn} [A]  Apply All Tweaks{rst}")
        print(f"  {ylw} [R]  Remove All Tweaks{rst}")
        print(f"  {mag} [P]  Create Restore Point{rst}")
        print(f"\n  {dim} [ 0] Exit{rst}\n")

    # -- Action dispatch --

    def _run_single(self, td: TweakDef, mode: str) -> None:
        # Corporate network safety guard
        try:
            assert_not_corporate()
        except CorporateNetworkError as exc:
            print(f"\n  🛑 {exc}")
            return

        label = f"{'Apply' if mode == 'apply' else 'Remove'}: {td.label}"
        print(f"\n  🧩 Running: {label} ...")
        try:
            fn = td.apply_fn if mode == "apply" else td.remove_fn
            fn()
            print(f"  ✅ Done. Logged to {SESSION.log_path}")
        except AdminRequirementError as exc:
            print(f"  ❌ {exc}")
        except Exception as exc:
            SESSION.log(f"Error running {label}: {exc}")
            print(f"  ❌ Error: {exc}")

    # -- Main loop --

    def loop(self) -> None:
        if not is_windows():
            print(f"⚠️  Intended for Windows. Detected: {platform_summary()}")
            return

        while True:
            self._banner()
            try:
                choice = input("  Enter choice (number=toggle, A/R/P/0): ").strip()
            except (EOFError, KeyboardInterrupt):
                choice = "0"

            if choice == "0":
                print("\n  👋 RegiLattice session ended.")
                break
            elif choice.upper() == "A":
                try:
                    assert_not_corporate()
                except CorporateNetworkError as exc:
                    print(f"\n  🛑 {exc}")
                else:
                    apply_all()
                    print("  ✅ All tweaks applied.")
            elif choice.upper() == "R":
                try:
                    assert_not_corporate()
                except CorporateNetworkError as exc:
                    print(f"\n  🛑 {exc}")
                else:
                    remove_all()
                    print("  ✅ All tweaks removed.")
            elif choice.upper() == "P":
                try:
                    create_restore_point()
                    print("  ✅ Restore point created.")
                except Exception as exc:
                    print(f"  ❌ {exc}")
            else:
                try:
                    idx = int(choice) - 1
                    if 0 <= idx < len(self._tweaks):
                        td = self._tweaks[idx]
                        st = tweak_status(td)
                        mode = "remove" if st == "applied" else "apply"
                        self._run_single(td, mode)
                    else:
                        print("  ❌ Invalid selection.")
                except ValueError:
                    print("  ❌ Invalid selection. Try again.")

            try:
                input("  Press Enter to return to the menu...")
            except (EOFError, KeyboardInterrupt):
                break


def main(argv: list[str] | None = None) -> int:  # noqa: ARG001
    """Standalone entry point for the interactive menu."""
    menu = Menu()
    menu.loop()
    return 0


if __name__ == "__main__":  # pragma: no cover
    sys.exit(main())
