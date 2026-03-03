"""Command-line interface for TurboTweak.

Usage examples::

    python -m turbotweak                    # interactive menu
    python -m turbotweak --list             # show available actions
    python -m turbotweak apply-performance  # run a single action
    python -m turbotweak apply-all -y       # skip confirmation
"""

from __future__ import annotations

import argparse
import sys
from typing import Callable, Dict

from . import __version__, tweaks
from .corpguard import CorporateNetworkError, assert_not_corporate
from .menu import Menu
from .registry import SESSION, AdminRequirementError, is_windows, platform_summary

Action = Callable[[], None]


def _actions() -> Dict[str, Action]:
    """Return an ordered mapping of action-name → callable."""
    return {
        "add-take-ownership": tweaks.add_take_ownership,
        "remove-take-ownership": tweaks.remove_take_ownership,
        "add-recent-folders": tweaks.add_recent_places,
        "remove-recent-folders": tweaks.remove_recent_places,
        "enable-verbose-boot": tweaks.enable_verbose_boot,
        "disable-verbose-boot": tweaks.disable_verbose_boot,
        "apply-performance": tweaks.apply_performance_tweaks,
        "remove-performance": tweaks.remove_performance_tweaks,
        "enable-registry-backup": tweaks.enable_registry_backup,
        "disable-registry-backup": tweaks.disable_registry_backup,
        "disable-telemetry": tweaks.disable_telemetry,
        "enable-telemetry": tweaks.enable_telemetry,
        "disable-cortana": tweaks.disable_cortana,
        "enable-cortana": tweaks.enable_cortana,
        "disable-mouse-accel": tweaks.disable_mouse_accel,
        "enable-mouse-accel": tweaks.enable_mouse_accel,
        "disable-game-dvr": tweaks.disable_game_dvr,
        "enable-game-dvr": tweaks.enable_game_dvr,
        "optimize-svchost": tweaks.optimize_svchost_split,
        "restore-svchost": tweaks.restore_svchost_split,
        "disable-last-access": tweaks.disable_last_access,
        "enable-last-access": tweaks.enable_last_access,
        "enable-long-paths": tweaks.enable_long_paths,
        "disable-long-paths": tweaks.disable_long_paths,
        "create-restore-point": tweaks.create_restore_point,
        "apply-all": tweaks.apply_all,
        "remove-all": tweaks.remove_all,
    }


def _confirm(prompt: str) -> bool:
    try:
        return input(f"{prompt} [y/N]: ").strip().lower() in {"y", "yes"}
    except (EOFError, KeyboardInterrupt):
        return False


def _run_action(name: str, *, assume_yes: bool, force: bool = False) -> int:
    actions = _actions()
    action = actions.get(name)
    if action is None:
        print(f"❌ Unknown action '{name}'. Use --list to see available options.")
        return 2

    # Corporate network safety guard
    try:
        assert_not_corporate(force=force)
    except CorporateNetworkError as exc:
        print(f"🛑 {exc}")
        return 6

    if not assume_yes and not _confirm(f"Proceed with '{name}'?"):
        print("ℹ️  Aborted by user.")
        return 1

    try:
        action()
        print(f"✅ Completed. Details logged to {SESSION.log_path}")
        return 0
    except AdminRequirementError as exc:
        print(f"❌ {exc}")
        return 5
    except Exception as exc:  # pragma: no cover — defensive
        SESSION.log(f"Error running action {name}: {exc}")
        print(f"❌ Error: {exc}")
        return 3


def _build_parser() -> argparse.ArgumentParser:
    parser = argparse.ArgumentParser(
        prog="turbotweak",
        description="TurboTweak — Windows registry tweak toolkit",
    )
    parser.add_argument(
        "action",
        nargs="?",
        help="Action to run (use --list for options). Omit to launch the interactive menu.",
    )
    parser.add_argument(
        "-y",
        "--assume-yes",
        action="store_true",
        help="Skip confirmation prompts (for scripting).",
    )
    parser.add_argument(
        "--list",
        action="store_true",
        help="List available actions and exit.",
    )
    parser.add_argument(
        "--force",
        action="store_true",
        help="Bypass corporate-network safety guard (use at your own risk).",
    )
    parser.add_argument(
        "--gui",
        action="store_true",
        help="Launch the graphical (tkinter) interface instead of the console menu.",
    )
    parser.add_argument(
        "--version",
        action="version",
        version=f"turbotweak {__version__} ({platform_summary()})",
    )
    return parser


def main(argv: list[str] | None = None) -> int:
    """Entry-point for both ``python -m turbotweak`` and the console_script."""
    parser = _build_parser()
    args = parser.parse_args(argv)

    if args.list:
        print("Available actions:")
        for key in _actions():
            print(f"  {key}")
        return 0

    if args.gui:
        from .gui import launch
        launch()
        return 0

    if args.action:
        return _run_action(args.action, assume_yes=args.assume_yes, force=args.force)

    # Interactive menu
    if not is_windows():
        print(f"⚠️  This menu is intended for Windows. Detected: {platform_summary()}")
        return 4

    menu = Menu()
    menu.loop()
    return 0


if __name__ == "__main__":  # pragma: no cover
    sys.exit(main())
