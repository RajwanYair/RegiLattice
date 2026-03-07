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
        if SESSION._dry_run:
            print(f"\U0001f50d Dry-run: {ok}/{len(results)} tweaks would be processed ({SESSION._dry_ops} registry ops skipped).")
        else:
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
        if SESSION._dry_run:
            print(f"\U0001f50d Dry-run '{action_label}' — {SESSION._dry_ops} registry op(s) skipped.")
            if td.registry_keys:
                print("  Registry keys that would be touched:")
                for rk in td.registry_keys:
                    print(f"    \u2022 {rk}")
        else:
            print(f"✅ Completed '{action_label}'. Log: {SESSION.log_path}")
        return 0
    except AdminRequirementError as exc:
        print(f"❌ {exc}")
        print("   Hint: run as Administrator — right-click terminal → 'Run as administrator',")
        print("   or use: python -m regilattice --gui  (the GUI can auto-elevate via UAC).")
        return 5
    except Exception as exc:  # pragma: no cover — defensive
        SESSION.log(f"Error running {action_label}: {exc}")
        print(f"❌ Error: {exc}")
        return 3


def _format_reg_value(val: object, typ: int) -> str:
    """Format a single registry value for .reg file output."""
    import winreg as _wr

    _NUL2 = b"\x00\x00"

    if typ == _wr.REG_DWORD:
        return f"dword:{int(val) & 0xFFFFFFFF:08x}"
    if typ == _wr.REG_SZ:
        escaped = str(val).replace("\\", "\\\\").replace('"', '\\"')
        return f'"{escaped}"'
    if typ == _wr.REG_EXPAND_SZ:
        raw = str(val).encode("utf-16-le") + _NUL2
        hex_str = ",".join(f"{b:02x}" for b in raw)
        return f"hex(2):{hex_str}"
    if typ == _wr.REG_BINARY:
        data = bytes(val) if isinstance(val, (bytes, bytearray)) else b""
        hex_str = ",".join(f"{b:02x}" for b in data)
        return f"hex:{hex_str}"
    if typ == _wr.REG_MULTI_SZ:
        parts = list(val) if isinstance(val, list) else [str(val)]
        combined = "\0".join(parts) + "\0"
        raw = combined.encode("utf-16-le") + _NUL2
        hex_str = ",".join(f"{b:02x}" for b in raw)
        return f"hex(7):{hex_str}"
    if typ == _wr.REG_QWORD:
        v = int(val) & 0xFFFFFFFFFFFFFFFF
        raw = v.to_bytes(8, "little")
        hex_str = ",".join(f"{b:02x}" for b in raw)
        return f"hex(b):{hex_str}"
    return f"hex({typ:x}):"


def _export_reg(dest: Path) -> int:
    """Export current registry state for all tweak keys to a .reg file."""
    if not is_windows():
        print("❌ --export-reg requires Windows.")
        return 4

    import winreg as _wr

    tweaks = all_tweaks()
    seen_keys: set[str] = set()
    lines: list[str] = ["Windows Registry Editor Version 5.00", ""]

    for td in tweaks:
        key_lines: list[str] = []
        for key_path in td.registry_keys:
            if key_path in seen_keys:
                continue
            seen_keys.add(key_path)
            try:
                root, subkey = _split_root_for_reg(key_path)
                with _wr.OpenKey(root, subkey, 0, _wr.KEY_READ) as handle:
                    key_lines.append(f"[{key_path}]")
                    idx = 0
                    while True:
                        try:
                            name, val, typ = _wr.EnumValue(handle, idx)
                            formatted = _format_reg_value(val, typ)
                            display_name = f'"{name}"' if name else "@"
                            key_lines.append(f"{display_name}={formatted}")
                            idx += 1
                        except OSError:
                            break
                    key_lines.append("")
            except (FileNotFoundError, OSError, ValueError):
                continue

        if key_lines:
            lines.append(f"; Tweak: {td.label} ({td.id})")
            lines.extend(key_lines)

    dest.write_text("\n".join(lines), encoding="utf-16-le")
    key_count = sum(1 for ln in lines if ln.startswith("["))
    print(f"✅ Exported {key_count} registry keys from {len(tweaks)} tweaks to {dest}")
    return 0


def _split_root_for_reg(path: str) -> tuple[int, str]:
    """Split a registry path for _export_reg (same logic as registry._split_root)."""
    import winreg as _wr

    roots: dict[str, int] = {
        "HKEY_CLASSES_ROOT": _wr.HKEY_CLASSES_ROOT,
        "HKCR": _wr.HKEY_CLASSES_ROOT,
        "HKEY_CURRENT_USER": _wr.HKEY_CURRENT_USER,
        "HKCU": _wr.HKEY_CURRENT_USER,
        "HKEY_LOCAL_MACHINE": _wr.HKEY_LOCAL_MACHINE,
        "HKLM": _wr.HKEY_LOCAL_MACHINE,
    }
    for prefix, root in roots.items():
        if path.upper().startswith(prefix.upper() + "\\"):
            return root, path[len(prefix) + 1 :]
    raise ValueError(f"Unsupported registry path: {path}")


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
    parser.add_argument(
        "--import-json",
        metavar="PATH",
        help="Import tweak IDs from JSON file and apply/remove them (use with apply/remove mode).",
    )
    parser.add_argument(
        "--list-profiles",
        action="store_true",
        help="List available profiles with descriptions and tweak counts.",
    )
    parser.add_argument(
        "--categories",
        action="store_true",
        help="List all tweak categories with tweak counts.",
    )
    parser.add_argument(
        "--tags",
        action="store_true",
        help="List all unique tags with usage counts.",
    )
    parser.add_argument(
        "--export-reg",
        metavar="PATH",
        help="Export current registry state for all tweak keys to a .reg file.",
    )
    parser.add_argument(
        "--check",
        action="store_true",
        help="Non-destructive audit: show which tweaks are applied, default, or unknown.",
    )
    parser.add_argument(
        "--diff",
        metavar="PROFILE",
        choices=list(available_profiles()),
        help="Compare current state against a profile and show which tweaks need changes.",
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

    if args.list_profiles:
        from .tweaks import profile_info

        print(f"{'Profile':<12} {'Tweaks':<8} Description")
        print("-" * 60)
        for name in available_profiles():
            info = profile_info(name)
            count = len(tweaks_for_profile(name)) if info else 0
            desc = info.description if info else ""
            print(f"{name:<12} {count:<8} {desc}")
        return 0

    if args.categories:
        by_cat = tweaks_by_category()
        print(f"{'Category':<25} Tweaks")
        print("-" * 35)
        for cat_name in sorted(by_cat):
            print(f"{cat_name:<25} {len(by_cat[cat_name])}")
        print(f"\n{len(by_cat)} categories, {sum(len(v) for v in by_cat.values())} tweaks total.")
        return 0

    if args.tags:
        from collections import Counter

        counts: Counter[str] = Counter()
        for td in all_tweaks():
            for tag in td.tags:
                counts[tag] += 1
        print(f"{'Tag':<25} Tweaks")
        print("-" * 35)
        for tag, cnt in sorted(counts.items()):
            print(f"{tag:<25} {cnt}")
        print(f"\n{len(counts)} unique tags across {sum(counts.values())} tag usages.")
        return 0

    if args.export_reg:
        return _export_reg(Path(args.export_reg))

    if args.check:
        from .tweaks import status_map

        smap = status_map()
        applied = [tid for tid, st in smap.items() if st == TweakResult.APPLIED]
        default = [tid for tid, st in smap.items() if st == TweakResult.NOT_APPLIED]
        unknown = [tid for tid, st in smap.items() if st == TweakResult.UNKNOWN]
        print(f"{'Status':<14} {'Count':<8}")
        print("-" * 22)
        print(f"{'Applied':<14} {len(applied):<8}")
        print(f"{'Default':<14} {len(default):<8}")
        print(f"{'Unknown':<14} {len(unknown):<8}")
        print(f"{'Total':<14} {len(smap):<8}")
        if applied:
            print(f"\nApplied tweaks ({len(applied)}):")
            for tid in sorted(applied):
                td = get_tweak(tid)
                label = td.label if td else tid
                print(f"  \u2713 {tid:<35} {label}")
        return 0

    if args.diff:
        from .tweaks import status_map

        profile_tweaks = {td.id for td in tweaks_for_profile(args.diff)}
        smap = status_map()
        to_apply = sorted(tid for tid in profile_tweaks if smap.get(tid) != TweakResult.APPLIED)
        to_remove = sorted(tid for tid in smap if tid not in profile_tweaks and smap[tid] == TweakResult.APPLIED)
        if not to_apply and not to_remove:
            print(f"System matches '{args.diff}' profile \u2014 no changes needed.")
            return 0
        if to_apply:
            print(f"Tweaks to APPLY for '{args.diff}' profile ({len(to_apply)}):")
            for tid in to_apply:
                td = get_tweak(tid)
                label = td.label if td else tid
                print(f"  + {tid:<35} {label}")
        if to_remove:
            print(f"\nApplied tweaks NOT in '{args.diff}' profile ({len(to_remove)}):")
            for tid in to_remove:
                td = get_tweak(tid)
                label = td.label if td else tid
                print(f"  - {tid:<35} {label}")
        print(f"\nSummary: {len(to_apply)} to apply, {len(to_remove)} extra applied.")
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
        if SESSION._dry_run:
            print(
                f"\U0001f50d Dry-run profile '{args.profile}': {ok}/{len(results)} tweaks would be applied"
                f" ({SESSION._dry_ops} registry ops skipped)."
            )
        else:
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
        if SESSION._dry_run:
            print(
                f"\U0001f50d Dry-run: {ok}/{len(cat_tweaks)} tweaks would be processed in '{args.category}'"
                f" ({SESSION._dry_ops} registry ops skipped)."
            )
        else:
            print(f"\u2705 {ok}/{len(cat_tweaks)} tweaks processed in '{args.category}'.")
        for e in errors:
            print(f"  \u274c {e}")
        return 0

    if args.import_json and args.mode in ("apply", "remove"):
        import json as _json

        try:
            assert_not_corporate(force=args.force)
        except CorporateNetworkError as exc:
            print(f"\U0001f6d1 {exc}")
            return 6
        try:
            data = _json.loads(Path(args.import_json).read_text(encoding="utf-8"))
        except (OSError, _json.JSONDecodeError) as exc:
            print(f"\u274c Failed to read JSON: {exc}")
            return 3
        if isinstance(data, dict):
            ids = list(data.get("tweaks", []))
        elif isinstance(data, list):
            ids = list(data)
        else:
            print("\u274c Expected a JSON list of tweak IDs or {\"tweaks\": [...]}.")
            return 3
        targets = []
        for tid in ids:
            td = get_tweak(tid)
            if td is None:
                print(f"\u26a0\ufe0f  Skipping unknown tweak '{tid}'")
            else:
                targets.append(td)
        if not targets:
            print("No valid tweaks found in JSON.")
            return 2
        label = f"{args.mode.title()} {len(targets)} tweaks from {Path(args.import_json).name}"
        if not args.assume_yes and not _confirm(label):
            print("\u2139\ufe0f  Aborted by user.")
            return 1
        errors_list: list[str] = []
        for td in targets:
            fn = td.apply_fn if args.mode == "apply" else td.remove_fn
            try:
                fn()
            except (AdminRequirementError, OSError, RuntimeError, ValueError) as exc:
                errors_list.append(f"{td.label}: {exc}")
        ok = len(targets) - len(errors_list)
        if SESSION._dry_run:
            print(f"\U0001f50d Dry-run: {ok}/{len(targets)} tweaks from JSON ({SESSION._dry_ops} registry ops skipped).")
        else:
            print(f"\u2705 {ok}/{len(targets)} tweaks processed from JSON.")
        for e in errors_list:
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
