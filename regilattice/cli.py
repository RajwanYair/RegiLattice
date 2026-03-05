"""Command-line interface for RegiLattice.

Usage examples::

    python -m regilattice                    # interactive menu
    python -m regilattice --list             # show available actions
    python -m regilattice apply take-ownership  # apply a single tweak
    python -m regilattice remove all -y      # skip confirmation
    python -m regilattice --gui              # graphical interface
"""

from __future__ import annotations

import argparse
import sys
from collections.abc import Callable
from pathlib import Path

from . import __version__
from .corpguard import CorporateNetworkError, assert_not_corporate
from .menu import Menu
from .registry import SESSION, AdminRequirementError, is_windows, platform_summary
from .tweaks import TweakResult, all_tweaks, apply_all, apply_profile, get_tweak, remove_all, tweak_status, tweaks_for_profile


def _confirm(prompt: str) -> bool:
    try:
        return input(f"{prompt} [y/N]: ").strip().lower() in {"y", "yes"}
    except (EOFError, KeyboardInterrupt):
        return False


def _run_action(
    mode: str,
    tweak_id: str,
    *,
    assume_yes: bool,
    force: bool = False,
) -> int:
    # Corporate network safety guard
    try:
        assert_not_corporate(force=force)
    except CorporateNetworkError as exc:
        print(f"🛑 {exc}")
        return 6

    # Batch operations
    if tweak_id == "all":
        label = f"{mode.title()} all tweaks"
        if not assume_yes and not _confirm(f"Proceed with '{label}'?"):
            print("i️  Aborted by user.")
            return 1
        batch_fn = apply_all if mode == "apply" else remove_all
        results = batch_fn(force_corp=force)
        ok = sum(1 for v in results.values() if v in (TweakResult.APPLIED, TweakResult.REMOVED))
        print(f"✅ {ok}/{len(results)} tweaks processed. Log: {SESSION.log_path}")
        return 0

    # Single tweak
    td = get_tweak(tweak_id)
    if td is None:
        print(f"❌ Unknown tweak '{tweak_id}'. Use --list to see available tweaks.")
        return 2

    action_label = f"{mode} {td.label}"
    if not assume_yes and not _confirm(f"Proceed with '{action_label}'?"):
        print("i️  Aborted by user.")
        return 1

    try:
        fn: Callable[..., None] = td.apply_fn if mode == "apply" else td.remove_fn
        fn()
        print(f"✅ Completed '{action_label}'. Log: {SESSION.log_path}")
        return 0
    except AdminRequirementError as exc:
        print(f"❌ {exc}")
        return 5
    except Exception as exc:  # pragma: no cover — defensive
        SESSION.log(f"Error running {action_label}: {exc}")
        print(f"❌ Error: {exc}")
        return 3


def _build_parser() -> argparse.ArgumentParser:
    parser = argparse.ArgumentParser(
        prog="regilattice",
        description="RegiLattice — Windows registry tweak toolkit",
    )
    parser.add_argument(
        "mode",
        nargs="?",
        choices=["apply", "remove", "status"],
        help="Action mode: apply, remove, or status.",
    )
    parser.add_argument(
        "tweak",
        nargs="?",
        help="Tweak id (e.g. 'take-ownership') or 'all'. Use --list to see ids.",
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
        help="List available tweaks and exit.",
    )
    parser.add_argument(
        "--force",
        action="store_true",
        help="Bypass corporate-network safety guard (use at your own risk).",
    )
    parser.add_argument(
        "--gui",
        action="store_true",
        help="Launch the graphical (tkinter) interface.",
    )
    parser.add_argument(
        "--snapshot",
        metavar="PATH",
        help="Save current tweak state snapshot to file (JSON).",
    )
    parser.add_argument(
        "--restore",
        metavar="PATH",
        help="Restore tweaks to state saved in snapshot file.",
    )
    parser.add_argument(
        "--profile",
        choices=["business", "gaming"],
        help=(
            "Apply a machine-purpose profile. "
            "'business' disables gaming/GPU tweaks; "
            "'gaming' disables Office/Communication/OneDrive tweaks."
        ),
    )
    parser.add_argument(
        "--version",
        action="version",
        version=f"regilattice {__version__} ({platform_summary()})",
    )
    return parser


def main(argv: list[str] | None = None) -> int:
    """Entry-point for both ``python -m regilattice`` and the console_script."""
    parser = _build_parser()
    args = parser.parse_args(argv)

    if args.list:
        tweaks = all_tweaks()
        print(f"{'ID':<30} {'Category':<14} {'Status':<14} Label")
        print("-" * 80)
        for td in tweaks:
            st = tweak_status(td)
            print(f"{td.id:<30} {td.category:<14} {st:<14} {td.label}")
        return 0

    if args.snapshot:
        from .tweaks import save_snapshot

        save_snapshot(Path(args.snapshot))
        print(f"✅ Snapshot saved to {args.snapshot}")
        return 0

    if args.restore:
        from .tweaks import restore_snapshot

        results = restore_snapshot(Path(args.restore), force_corp=args.force)
        for tid, action in results.items():
            print(f"  {tid}: {action}")
        return 0

    if args.profile:
        try:
            assert_not_corporate(force=args.force)
        except CorporateNetworkError as exc:
            print(f"\U0001f6d1 {exc}")
            return 6
        targets = tweaks_for_profile(args.profile)
        label = f"Apply '{args.profile}' profile ({len(targets)} tweaks)"
        if not args.assume_yes and not _confirm(label):
            print("\u2139\ufe0f  Aborted by user.")
            return 1
        results = apply_profile(args.profile, force_corp=args.force)
        ok = sum(1 for v in results.values() if v == TweakResult.APPLIED)
        print(f"\u2705 Profile '{args.profile}': {ok}/{len(results)} tweaks applied.")
        for tid, res in results.items():
            print(f"  {tid}: {res}")
        return 0

    if args.gui:
        from .gui import launch

        launch()
        return 0

    if args.mode and args.tweak:
        if args.mode == "status":
            found = get_tweak(args.tweak)
            if found:
                print(f"{found.label}: {tweak_status(found)}")
            else:
                print(f"❌ Unknown tweak '{args.tweak}'.")
                return 2
            return 0
        return _run_action(
            args.mode, args.tweak, assume_yes=args.assume_yes, force=args.force
        )

    # Interactive menu
    if not is_windows():
        print(f"⚠️  This menu is intended for Windows. Detected: {platform_summary()}")
        return 4

    menu = Menu()
    menu.loop()
    return 0


if __name__ == "__main__":  # pragma: no cover
    sys.exit(main())
