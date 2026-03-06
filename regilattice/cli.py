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
from .tweaks import (
    TweakResult,
    all_tweaks,
    apply_all,
    apply_profile,
    available_profiles,
    get_tweak,
    remove_all,
    search_tweaks,
    tweak_status,
    tweaks_by_category,
    tweaks_for_profile,
)


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
        "--snapshot-diff",
        nargs=2,
        metavar=("FILE_A", "FILE_B"),
        help="Compare two snapshot files and show differences.",
    )
    parser.add_argument(
        "--profile",
        choices=list(available_profiles()),
        help="Apply a machine-purpose profile (business, gaming, privacy, minimal, server).",
    )
    parser.add_argument(
        "--dry-run",
        action="store_true",
        help="Preview changes without modifying the registry.",
    )
    parser.add_argument(
        "--config",
        metavar="PATH",
        help="Path to config file (default: ~/.regilattice.toml).",
    )
    parser.add_argument(
        "--version",
        action="version",
        version=f"regilattice {__version__} ({platform_summary()})",
    )
    parser.add_argument(
        "--check-deps",
        action="store_true",
        help="Verify dev dependencies (pytest, ruff, mypy) and auto-install missing ones.",
    )
    parser.add_argument(
        "--search",
        metavar="QUERY",
        help="Search tweaks by keyword (id, label, category, tags) and print matches.",
    )
    parser.add_argument(
        "--category",
        metavar="NAME",
        help="Apply or remove all tweaks in a specific category (use with apply/remove mode).",
    )
    parser.add_argument(
        "--export-json",
        metavar="PATH",
        help="Export all tweak definitions (id, label, category, status, tags) to JSON file.",
    )
    return parser


def main(argv: list[str] | None = None) -> int:
    """Entry-point for both ``python -m regilattice`` and the console_script."""
    parser = _build_parser()
    args = parser.parse_args(argv)

    # Load user config (~/.regilattice.toml or --config path)
    from .config import load_config

    cfg = load_config(Path(args.config) if args.config else None)

    # Apply config defaults (CLI flags override config)
    if cfg.force_corp and not args.force:
        args.force = True

    if args.dry_run:
        SESSION._dry_run = True
        print("\U0001f50d Dry-run mode \u2014 no registry changes will be made.")

    if args.check_deps:
        from .deps import require

        _DEV_DEPS = ["pytest", "mypy", "ruff"]
        print("Checking dev dependencies\u2026")
        for pkg in _DEV_DEPS:
            try:
                require(pkg)
                print(f"  \u2705 {pkg}")
            except ImportError:
                print(f"  \u274c {pkg} \u2014 could not install")
        return 0

    if args.list:
        tweaks = all_tweaks()
        print(f"{'ID':<30} {'Category':<14} {'Status':<14} Label")
        print("-" * 80)
        for td in tweaks:
            st = tweak_status(td)
            print(f"{td.id:<30} {td.category:<14} {st:<14} {td.label}")
        return 0

    if args.search:
        results = search_tweaks(args.search)
        if not results:
            print(f"No tweaks matching '{args.search}'.")
            return 0
        print(f"{'ID':<30} {'Category':<14} {'Status':<14} Label")
        print("-" * 80)
        for td in results:
            st = tweak_status(td)
            print(f"{td.id:<30} {td.category:<14} {st:<14} {td.label}")
        print(f"\n{len(results)} tweak(s) found.")
        return 0

    if args.export_json:
        import json

        tweaks = all_tweaks()
        data = [
            {
                "id": td.id,
                "label": td.label,
                "category": td.category,
                "status": tweak_status(td),
                "needs_admin": td.needs_admin,
                "corp_safe": td.corp_safe,
                "tags": td.tags,
                "registry_keys": td.registry_keys,
                "description": td.description,
            }
            for td in tweaks
        ]
        Path(args.export_json).write_text(json.dumps(data, indent=2), encoding="utf-8")
        print(f"\u2705 Exported {len(data)} tweaks to {args.export_json}")
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

    if args.snapshot_diff:
        from .tweaks import diff_snapshots

        diffs = diff_snapshots(Path(args.snapshot_diff[0]), Path(args.snapshot_diff[1]))
        if not diffs:
            print("No differences found.")
        else:
            print(f"{'Tweak ID':<35} {'File A':<18} {'File B':<18}")
            print("-" * 71)
            for tid, (sa, sb) in diffs.items():
                print(f"{tid:<35} {sa:<18} {sb:<18}")
            print(f"\n{len(diffs)} tweak(s) differ.")
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

    if args.category and args.mode in ("apply", "remove"):
        cat_tweaks = tweaks_by_category().get(args.category, [])
        if not cat_tweaks:
            print(f"\u274c Unknown category '{args.category}'. Use --list to see categories.")
            return 2
        try:
            assert_not_corporate(force=args.force)
        except CorporateNetworkError as exc:
            print(f"\U0001f6d1 {exc}")
            return 6
        label = f"{args.mode.title()} {len(cat_tweaks)} tweaks in '{args.category}'"
        if not args.assume_yes and not _confirm(label):
            print("\u2139\ufe0f  Aborted by user.")
            return 1
        errors: list[str] = []
        for td in cat_tweaks:
            fn = td.apply_fn if args.mode == "apply" else td.remove_fn
            try:
                fn()
            except (AdminRequirementError, OSError, RuntimeError, ValueError) as exc:
                errors.append(f"{td.label}: {exc}")
        ok = len(cat_tweaks) - len(errors)
        print(f"\u2705 {ok}/{len(cat_tweaks)} tweaks processed in '{args.category}'.")
        for e in errors:
            print(f"  \u274c {e}")
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
        return _run_action(args.mode, args.tweak, assume_yes=args.assume_yes, force=args.force)

    # Interactive menu
    if not is_windows():
        print(f"⚠️  This menu is intended for Windows. Detected: {platform_summary()}")
        return 4

    menu = Menu()
    menu.loop()
    return 0


if __name__ == "__main__":  # pragma: no cover
    sys.exit(main())
