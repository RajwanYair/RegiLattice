"""Interactive menu mirroring ``TurboTweakMenu.ps1``."""

from __future__ import annotations

import os
import sys
from dataclasses import dataclass, field
from typing import Callable, List

from . import __version__
from .registry import SESSION, AdminRequirementError, is_windows, platform_summary
from . import tweaks


@dataclass
class _MenuItem:
    key: str
    label: str
    action: Callable[[], None]
    color: str = ""


class Menu:
    """Interactive numbered menu for applying / removing tweaks."""

    def __init__(self) -> None:
        self._items: List[_MenuItem] = [
            _MenuItem("1", "Add Take Ownership", tweaks.add_take_ownership),
            _MenuItem("2", "Remove Take Ownership", tweaks.remove_take_ownership),
            _MenuItem("3", "Add Recent Folders", tweaks.add_recent_places),
            _MenuItem("4", "Remove Recent Folders", tweaks.remove_recent_places),
            _MenuItem("5", "Enable Verbose Boot Messages", tweaks.enable_verbose_boot),
            _MenuItem("6", "Disable Verbose Boot Messages", tweaks.disable_verbose_boot),
            _MenuItem("7", "Apply Performance Tweaks", tweaks.apply_performance_tweaks),
            _MenuItem("8", "Remove Performance Tweaks", tweaks.remove_performance_tweaks),
            _MenuItem("9", "Enable Registry Backup", tweaks.enable_registry_backup),
            _MenuItem("10", "Disable Registry Backup", tweaks.disable_registry_backup),
            _MenuItem("11", "Apply All Tweaks", tweaks.apply_all, color="\033[92m"),
            _MenuItem("12", "Remove All Tweaks", tweaks.remove_all, color="\033[93m"),
            _MenuItem("13", "Create Restore Point", tweaks.create_restore_point, color="\033[95m"),
        ]
        self._lookup = {item.key: item for item in self._items}

    # -- Display --

    @staticmethod
    def _clear() -> None:
        os.system("cls" if os.name == "nt" else "clear")

    def _banner(self) -> None:
        self._clear()
        rst = "\033[0m"
        cyan = "\033[96m"
        dim = "\033[90m"
        print()
        print(f"  {cyan}╔══════════════════════════════════════════════╗{rst}")
        print(f"  {cyan}║         ⚡ TurboTweak Launcher ⚡            ║{rst}")
        print(f"  {cyan}╚══════════════════════════════════════════════╝{rst}")
        print(f"  {dim}Version: {__version__} | Platform: {platform_summary()}{rst}")
        print(f"  {dim}Log: {SESSION.log_path}{rst}")
        print()
        for item in self._items:
            pad = " " if len(item.key) < 2 else ""
            color = item.color or ""
            print(f"  {color} [{pad}{item.key}] {item.label}{rst}")
        print(f"\n  {dim} [ 0] Exit{rst}\n")

    # -- Action dispatch --

    def _run(self, choice: str) -> None:
        item = self._lookup.get(choice)
        if item is None:
            print("  ❌ Invalid selection. Try again.")
            return

        print(f"\n  🧩 Running: {item.label} ...")
        try:
            item.action()
            print(f"  ✅ Done. Logged to {SESSION.log_path}")
        except AdminRequirementError as exc:
            print(f"  ❌ {exc}")
        except Exception as exc:  # pragma: no cover — defensive
            SESSION.log(f"Error running {item.label}: {exc}")
            print(f"  ❌ Error: {exc}")

    # -- Main loop --

    def loop(self) -> None:
        if not is_windows():
            print(f"⚠️  Intended for Windows. Detected: {platform_summary()}")
            return

        while True:
            self._banner()
            try:
                choice = input("  Enter your choice: ").strip()
            except (EOFError, KeyboardInterrupt):
                choice = "0"

            if choice == "0":
                print("\n  👋 TurboTweak session ended.")
                break

            self._run(choice)
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
