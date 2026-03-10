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
import signal
import sys
from collections.abc import Callable
from pathlib import Path
from typing import Any

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
    tweak_scope,
    tweak_status,
    tweaks_by_category,
    tweaks_for_profile,
)

__all__ = ["main"]


def _status_map_with_progress(label: str = "Scanning") -> dict[str, TweakResult]:
    """Run status_map(parallel=True) while printing a live counter to stderr."""
    from .tweaks import status_map

    _done: list[int] = [0]

    def _on_progress(done: int, _total: int) -> None:
        _done[0] = done
        print(f"\r{_ANSI_DIM}{label} {done}/{_total}…{_ANSI_RESET}", end="", flush=True, file=sys.stderr)

    result = status_map(parallel=True, progress_fn=_on_progress)
    print(f"\r{' ' * (len(label) + 20)}\r", end="", flush=True, file=sys.stderr)
    return result


# ── Snapshot diff helpers ────────────────────────────────────────────────────


def _confirm(prompt: str) -> bool:
    try:
        return input(f"{prompt} [y/N]: ").strip().lower() in {"y", "yes"}
    except (EOFError, KeyboardInterrupt):
        return False


_ANSI_RESET = "\033[0m"
_ANSI_GREEN = "\033[32m"
_ANSI_RED = "\033[31m"
_ANSI_YELLOW = "\033[33m"
_ANSI_CYAN = "\033[36m"
_ANSI_BOLD = "\033[1m"
_ANSI_DIM = "\033[2m"


def _state_colour(state: str) -> str:
    """Return ANSI-coloured state string."""
    if state in ("applied", "APPLIED"):
        return f"{_ANSI_GREEN}{state}{_ANSI_RESET}"
    if state in ("not applied", "NOT_APPLIED", "removed", "REMOVED"):
        return f"{_ANSI_RED}{state}{_ANSI_RESET}"
    if state == "(absent)":
        return f"{_ANSI_DIM}{state}{_ANSI_RESET}"
    return f"{_ANSI_YELLOW}{state}{_ANSI_RESET}"


def _print_diff_coloured(
    diffs: dict[str, tuple[str, str]],
    name_a: str,
    name_b: str,
) -> None:
    """Print coloured snapshot diff table to terminal."""
    header = f"{_ANSI_BOLD}{'Tweak ID':<40} {name_a:<20} {name_b:<20}{_ANSI_RESET}"
    print(header)
    print(f"{_ANSI_DIM}{'─' * 80}{_ANSI_RESET}")

    added = removed = changed = 0
    for tid, (sa, sb) in diffs.items():
        col_a = _state_colour(sa)
        col_b = _state_colour(sb)
        arrow = f"{_ANSI_CYAN}→{_ANSI_RESET}"
        print(f"  {tid:<38} {col_a:<32} {arrow} {col_b}")
        if sa == "(absent)":
            added += 1
        elif sb == "(absent)":
            removed += 1
        else:
            changed += 1

    print(f"\n{_ANSI_BOLD}{len(diffs)} tweak(s) differ{_ANSI_RESET}", end="")
    parts: list[str] = []
    if added:
        parts.append(f"{_ANSI_GREEN}+{added} added{_ANSI_RESET}")
    if removed:
        parts.append(f"{_ANSI_RED}-{removed} removed{_ANSI_RESET}")
    if changed:
        parts.append(f"{_ANSI_YELLOW}~{changed} changed{_ANSI_RESET}")
    if parts:
        print(f"  ({', '.join(parts)})")
    else:
        print()


def _write_diff_html(
    diffs: dict[str, tuple[str, str]],
    dest: Path,
    name_a: str,
    name_b: str,
) -> None:
    """Write an HTML diff report to *dest*."""
    from html import escape

    rows: list[str] = []
    for tid, (sa, sb) in diffs.items():
        cls_a = "applied" if sa in ("applied", "APPLIED") else "removed" if sa in ("not applied", "NOT_APPLIED", "removed") else "absent"
        cls_b = "applied" if sb in ("applied", "APPLIED") else "removed" if sb in ("not applied", "NOT_APPLIED", "removed") else "absent"
        rows.append(f'<tr><td class="tid">{escape(tid)}</td><td class="{cls_a}">{escape(sa)}</td><td class="{cls_b}">{escape(sb)}</td></tr>')

    html = f"""\
<!DOCTYPE html>
<html lang="en">
<head>
<meta charset="utf-8">
<title>RegiLattice Snapshot Diff</title>
<style>
  body {{ font-family: 'Segoe UI', system-ui, sans-serif; margin: 2rem; background: #1e1e2e; color: #cdd6f4; }}
  h1 {{ color: #89b4fa; font-size: 1.4rem; }}
  table {{ border-collapse: collapse; width: 100%; margin-top: 1rem; }}
  th {{ background: #313244; color: #cdd6f4; padding: 8px 12px; text-align: left; border-bottom: 2px solid #45475a; }}
  td {{ padding: 6px 12px; border-bottom: 1px solid #313244; }}
  .tid {{ font-family: 'Cascadia Code', monospace; color: #89dceb; }}
  .applied {{ color: #a6e3a1; font-weight: 600; }}
  .removed {{ color: #f38ba8; }}
  .absent {{ color: #6c7086; font-style: italic; }}
  .summary {{ margin-top: 1rem; color: #a6adc8; }}
</style>
</head>
<body>
<h1>RegiLattice &mdash; Snapshot Diff</h1>
<p class="summary">Comparing <b>{escape(name_a)}</b> vs <b>{escape(name_b)}</b> &mdash; {len(diffs)} difference(s)</p>
<table>
<tr><th>Tweak ID</th><th>{escape(name_a)}</th><th>{escape(name_b)}</th></tr>
{"".join(rows)}
</table>
</body>
</html>"""
    dest.parent.mkdir(parents=True, exist_ok=True)
    dest.write_text(html, encoding="utf-8")


def _run_action(
    mode: str,
    tweak_id: str,
    *,
    assume_yes: bool,
    force: bool = False,
) -> int:
    # Batch operations — TweakExecutor handles corp check per-tweak (respects corp_safe)
    if tweak_id == "all":
        label = f"{mode.title()} all tweaks"
        if not assume_yes and not _confirm(f"Proceed with '{label}'?"):
            print("i️  Aborted by user.")
            return 1
        batch_fn = apply_all if mode == "apply" else remove_all
        results = batch_fn(force_corp=force)
        ok = sum(1 for v in results.values() if v in (TweakResult.APPLIED, TweakResult.REMOVED))
        skipped_corp = sum(1 for v in results.values() if v == TweakResult.SKIPPED_CORP)
        if SESSION.dry_run:
            print(f"\U0001f50d Dry-run: {ok}/{len(results)} tweaks would be processed ({SESSION.dry_ops} registry ops skipped).")
        else:
            print(f"✅ {ok}/{len(results)} tweaks processed. Log: {SESSION.log_path}")
        if skipped_corp:
            print(f"   \u26a0\ufe0f  {skipped_corp} tweak(s) skipped \u2014 corporate network detected. Use --force to override.")
        return 0

    # Single tweak — look up TweakDef FIRST so corp_safe tweaks bypass the corp guard
    td = get_tweak(tweak_id)
    if td is None:
        print(f"❌ Unknown tweak '{tweak_id}'. Use --list to see available tweaks.")
        return 2

    # Corp check — corp_safe tweaks are always allowed regardless of corporate network
    if not td.corp_safe:
        try:
            assert_not_corporate(force=force)
        except CorporateNetworkError as exc:
            print(f"🛑 {exc}")
            return 6

    action_label = f"{mode} {td.label}"
    if not assume_yes and not _confirm(f"Proceed with '{action_label}'?"):
        print("i️  Aborted by user.")
        return 1

    try:
        fn: Callable[..., None] = td.apply_fn if mode == "apply" else td.remove_fn
        fn(require_admin=td.needs_admin)  # respect TweakDef.needs_admin
        if SESSION.dry_run:
            print(f"\U0001f50d Dry-run '{action_label}' \u2014 {SESSION.dry_ops} registry op(s) skipped.")
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


def _format_reg_value(val: Any, typ: int) -> str:
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
        data = bytes(val) if isinstance(val, bytes | bytearray) else b""
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


def _run_report(args: argparse.Namespace) -> int:
    """Print current enable/disable status of all tweaks grouped by category."""
    cat_filter: str | None = getattr(args, "category", None)
    only_enabled = getattr(args, "only_enabled", False)
    only_disabled = getattr(args, "only_disabled", False)
    output_fmt = getattr(args, "output", "table")

    by_cat = tweaks_by_category()
    if cat_filter:
        by_cat = {k: v for k, v in by_cat.items() if k.lower() == cat_filter.lower()}
        if not by_cat:
            print(f"❌ Unknown category '{cat_filter}'. Use --categories to list categories.")
            return 2

    smap = _status_map_with_progress("Scanning")

    if output_fmt == "json":
        import json as _j

        report: list[dict[str, object]] = []
        for cat_name, cat_tweaks in sorted(by_cat.items()):
            for td in cat_tweaks:
                st = smap.get(td.id, TweakResult.UNKNOWN)
                state = "enabled" if st == TweakResult.APPLIED else "disabled" if st == TweakResult.NOT_APPLIED else "unknown"
                if only_enabled and state != "enabled":
                    continue
                if only_disabled and state != "disabled":
                    continue
                report.append({"category": cat_name, "id": td.id, "label": td.label, "status": state})
        print(_j.dumps(report, indent=2))
        return 0

    for cat_name, cat_tweaks in sorted(by_cat.items()):
        rows: list[tuple[str, str, str, str]] = []
        for td in cat_tweaks:
            st = smap.get(td.id, TweakResult.UNKNOWN)
            if st == TweakResult.APPLIED:
                icon, label_colour = "●", _ANSI_GREEN
                state = "enabled"
            elif st == TweakResult.NOT_APPLIED:
                icon, label_colour = "○", _ANSI_DIM
                state = "disabled"
            else:
                icon, label_colour = "?", _ANSI_YELLOW
                state = "unknown"
            if only_enabled and state != "enabled":
                continue
            if only_disabled and state != "disabled":
                continue
            rows.append((icon, label_colour, td.id, td.label))

        if not rows:
            continue
        enabled_cnt = sum(1 for td in cat_tweaks if smap.get(td.id) == TweakResult.APPLIED)
        print(f"\n{_ANSI_BOLD}{cat_name}{_ANSI_RESET}  {_ANSI_DIM}({enabled_cnt}/{len(cat_tweaks)} enabled){_ANSI_RESET}")
        for icon, col, tid, lbl in rows:
            print(f"  {col}{icon}{_ANSI_RESET} {tid:<38} {lbl}")

    total_enabled = sum(1 for st in smap.values() if st == TweakResult.APPLIED)
    total = sum(len(v) for v in by_cat.values())
    print(f"\n{_ANSI_BOLD}Summary:{_ANSI_RESET} {total_enabled} enabled / {total - total_enabled} disabled / {total} total")
    return 0


def _run_doctor() -> int:
    """Comprehensive system health check — prints a report and returns exit code."""
    import platform

    checks: list[tuple[str, bool, str]] = []  # (label, passed, detail)

    # 1. Python version
    vi = sys.version_info
    py_ok = (vi.major, vi.minor) >= (3, 9)
    checks.append(("Python >= 3.9", py_ok, f"{vi.major}.{vi.minor}.{vi.micro}"))

    # 2. winreg availability
    win_ok = is_windows()
    checks.append(("Windows / winreg", win_ok, platform.system()))

    # 3. Admin status
    from .elevation import is_admin

    admin_ok = is_admin()
    checks.append(("Running as admin", admin_ok, "yes" if admin_ok else "no (some tweaks unavailable)"))

    # 4. Config file validity
    config_detail = "OK"
    try:
        from .config import load_config

        load_config(None)
        cfg_ok = True
    except Exception as exc:
        cfg_ok = False
        config_detail = str(exc)[:80]
    checks.append(("Config file", cfg_ok, config_detail))

    # 5. Tweaks load cleanly
    tweak_detail = "OK"
    try:
        from .tweaks import all_tweaks

        all_tweaks_list = all_tweaks()
        ids = [td.id for td in all_tweaks_list]
        dup_ids = {tid for tid in ids if ids.count(tid) > 1}
        tweaks_ok = len(dup_ids) == 0
        tweak_detail = f"{len(all_tweaks_list)} tweaks loaded" if tweaks_ok else f"Duplicate IDs: {', '.join(sorted(dup_ids))}"
    except Exception as exc:
        tweaks_ok = False
        tweak_detail = str(exc)[:80]
    checks.append(("Tweaks registry", tweaks_ok, tweak_detail))

    # 6. Corporate guard
    corp_detail = "not detected"
    try:
        from .corpguard import corp_guard_status, is_corporate_network

        if is_corporate_network():
            corp_detail = corp_guard_status() or "corporate network detected"
        corp_ok = True  # detecting is not a failure; just informational
    except Exception as exc:
        corp_ok = False
        corp_detail = str(exc)[:80]
    checks.append(("Corp guard", corp_ok, corp_detail))

    # 7. Session log writable
    try:
        SESSION.log_path.parent.mkdir(parents=True, exist_ok=True)
        log_ok = True
        log_detail = str(SESSION.log_path)
    except Exception as exc:
        log_ok = False
        log_detail = str(exc)[:80]
    checks.append(("Log path writable", log_ok, log_detail))

    # ── Report ────────────────────────────────────────────────────────────────
    _W = 30
    print(f"\n  {'RegiLattice Doctor':^{_W + 20}}")
    print(f"  {platform_summary()}")
    print()
    all_ok = True
    for label, passed, detail in checks:
        icon = "\u2705" if passed else "\u274c"
        all_ok = all_ok and passed
        print(f"  {icon}  {label:<{_W}}  {detail}")
    print()
    if all_ok:
        print("  All checks passed \u2014 RegiLattice is healthy.")
    else:
        print("  \u26a0\ufe0f  Some checks failed. Review the items marked with \u274c above.")
    print()
    return 0 if all_ok else 1


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
        "--menu",
        action="store_true",
        help="Launch the interactive terminal category menu.",
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
        "--html",
        metavar="PATH",
        help="With --snapshot-diff, write an HTML report to PATH instead of terminal output.",
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
        "--hwinfo",
        action="store_true",
        help="Detect and display hardware info (CPU, GPU, RAM, caps) and suggested profile.",
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
        "--report",
        action="store_true",
        help="Show current enable/disable status of all tweaks grouped by category.",
    )
    parser.add_argument(
        "--diff",
        metavar="PROFILE",
        choices=list(available_profiles()),
        help="Compare current state against a profile and show which tweaks need changes.",
    )
    parser.add_argument(
        "--output",
        choices=["table", "json"],
        default="table",
        help="Output format for --list and --search (table or json). Default: table.",
    )
    parser.add_argument(
        "--validate",
        action="store_true",
        help="Validate all TweakDef entries for consistency (duplicate IDs, missing fields, etc.).",
    )
    parser.add_argument(
        "--stats",
        action="store_true",
        help="Show comprehensive statistics: tweaks by category, scope, corp-safety, admin requirement.",
    )
    parser.add_argument(
        "--list-categories",
        action="store_true",
        dest="list_categories",
        help="Alias for --categories: list all tweak categories with counts.",
    )
    parser.add_argument(
        "--scope",
        choices=["user", "machine", "both"],
        metavar="SCOPE",
        help="Filter tweaks by registry scope: user, machine, or both (use with --list/--search).",
    )
    parser.add_argument(
        "--min-build",
        type=int,
        metavar="BUILD",
        dest="min_build",
        help="Filter tweaks compatible with this Windows build number (use with --list/--search).",
    )
    parser.add_argument(
        "--corp-safe",
        action="store_true",
        default=False,
        dest="corp_safe",
        help="Show only tweaks safe to apply on corporate machines (use with --list/--search).",
    )
    parser.add_argument(
        "--needs-admin",
        action="store_true",
        default=False,
        dest="needs_admin",
        help="Show only tweaks that require administrator rights (use with --list/--search).",
    )
    parser.add_argument(
        "--doctor",
        action="store_true",
        help="Run a comprehensive system health check: Python version, winreg, admin, config, tweaks.",
    )
    parser.add_argument(
        "--log-level",
        metavar="LEVEL",
        default="WARNING",
        choices=["DEBUG", "INFO", "WARNING", "ERROR", "CRITICAL"],
        help="Set logging verbosity (DEBUG, INFO, WARNING, ERROR, CRITICAL). Default: WARNING.",
    )
    return parser


def main(argv: list[str] | None = None) -> int:
    """Entry-point for both ``python -m regilattice`` and the console_script."""

    # Graceful shutdown on Ctrl-C or SIGTERM
    def _handle_shutdown(signum: int, frame: object) -> None:
        print("\nShutting down gracefully\u2026")
        sys.exit(128 + signum)

    signal.signal(signal.SIGINT, _handle_shutdown)
    signal.signal(signal.SIGTERM, _handle_shutdown)

    parser = _build_parser()
    args = parser.parse_args(argv)

    # Configure logging as early as possible so all subsequent code can use it
    from .logger import configure_logging

    configure_logging(args.log_level)

    # Load user config (~/.regilattice.toml or --config path)
    from .config import load_config

    cfg = load_config(Path(args.config) if args.config else None)

    # Apply config defaults (CLI flags override config)
    if cfg.force_corp and not args.force:
        args.force = True

    if args.dry_run:
        SESSION.dry_run = True
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

    if args.doctor:
        return _run_doctor()

    if args.hwinfo:
        from .hwinfo import detect_hardware, hardware_summary, suggest_profile

        print("Detecting hardware\u2026")
        hw = detect_hardware()
        print(hardware_summary(hw))
        print(f"\nSuggested profile: {suggest_profile(hw)}")
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

    if args.validate:
        tweaks = all_tweaks()
        errors: list[str] = []
        seen_ids: set[str] = set()
        for td in tweaks:
            if not td.id or not td.id.strip():
                errors.append(f"[{td.label}] empty id")
            elif td.id in seen_ids:
                errors.append(f"Duplicate id: {td.id!r}")
            seen_ids.add(td.id)
            if not td.label or not td.label.strip():
                errors.append(f"[{td.id}] empty label")
            if not td.category or not td.category.strip():
                errors.append(f"[{td.id}] empty category")
            if td.apply_fn is None:
                errors.append(f"[{td.id}] missing apply_fn")
            if td.remove_fn is None:
                errors.append(f"[{td.id}] missing remove_fn")
        if errors:
            print(f"\u274c Validation found {len(errors)} issue(s):")
            for e in errors:
                print(f"  \u2022 {e}")
            return 1
        print(f"\u2705 All {len(tweaks)} tweaks passed validation (no issues found).")
        return 0

    if args.stats:
        tweaks = all_tweaks()
        by_cat = tweaks_by_category()
        from collections import Counter

        scope_counts: Counter[str] = Counter(tweak_scope(td) for td in tweaks)
        print("── RegiLattice Stats ────────────────────")
        print(f"  Total tweaks   : {len(tweaks)}")
        print(f"  Categories     : {len(by_cat)}")
        print(f"  Profiles       : {len(available_profiles())}")
        print()
        print("  Scope breakdown:")
        for scope_name in ("user", "machine", "both"):
            print(f"    {scope_name:<10} {scope_counts[scope_name]}")
        corp_safe = sum(1 for td in tweaks if td.corp_safe)
        needs_admin = sum(1 for td in tweaks if td.needs_admin)
        has_detect = sum(1 for td in tweaks if td.detect_fn is not None)
        has_desc = sum(1 for td in tweaks if td.description.strip())
        has_deps = sum(1 for td in tweaks if td.depends_on)
        print()
        print(f"  Corp-safe      : {corp_safe}")
        print(f"  Needs admin    : {needs_admin}")
        print(f"  Has detect_fn  : {has_detect}")
        print(f"  Has description: {has_desc}")
        print(f"  Has depends_on : {has_deps}")
        print()
        print(f"{'Category':<30} Tweaks")
        print("  " + "-" * 38)
        for cat_name in sorted(by_cat):
            print(f"  {cat_name:<28} {len(by_cat[cat_name])}")
        return 0

    if args.categories or getattr(args, "list_categories", False):
        by_cat = tweaks_by_category()
        if getattr(args, "output", "table") == "json":
            import json as _j

            print(_j.dumps({cat: len(ts) for cat, ts in sorted(by_cat.items())}, indent=2))
        else:
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

    if args.report:
        return _run_report(args)

    if args.check:
        smap = _status_map_with_progress("Checking")
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
                matching = get_tweak(tid)
                label = matching.label if matching else tid
                print(f"  \u2713 {tid:<35} {label}")
        return 0

    if args.diff:
        profile_tweaks = {td.id for td in tweaks_for_profile(args.diff)}
        smap = _status_map_with_progress("Diffing")
        to_apply = sorted(tid for tid in profile_tweaks if smap.get(tid) != TweakResult.APPLIED)
        to_remove = sorted(tid for tid in smap if tid not in profile_tweaks and smap[tid] == TweakResult.APPLIED)
        if not to_apply and not to_remove:
            print(f"System matches '{args.diff}' profile \u2014 no changes needed.")
            return 0
        if to_apply:
            print(f"Tweaks to APPLY for '{args.diff}' profile ({len(to_apply)}):")
            for tid in to_apply:
                matching = get_tweak(tid)
                label = matching.label if matching else tid
                print(f"  + {tid:<35} {label}")
        if to_remove:
            print(f"\nApplied tweaks NOT in '{args.diff}' profile ({len(to_remove)}):")
            for tid in to_remove:
                matching = get_tweak(tid)
                label = matching.label if matching else tid
                print(f"  - {tid:<35} {label}")
        print(f"\nSummary: {len(to_apply)} to apply, {len(to_remove)} extra applied.")
        return 0

    if args.list:
        tweaks = all_tweaks()
        if args.category:
            tweaks = [td for td in tweaks if td.category.lower() == args.category.lower()]
            if not tweaks:
                print(f"\u274c No tweaks found in category '{args.category}'.")
                return 2
        if getattr(args, "scope", None):
            tweaks = [td for td in tweaks if tweak_scope(td) == args.scope]
        if getattr(args, "min_build", None):
            tweaks = [td for td in tweaks if td.min_build <= args.min_build]
        if getattr(args, "corp_safe", False):
            tweaks = [td for td in tweaks if td.corp_safe]
        if getattr(args, "needs_admin", False):
            tweaks = [td for td in tweaks if td.needs_admin]
        if getattr(args, "output", "table") == "json":
            import json as _j

            print(
                _j.dumps(
                    [
                        {"id": td.id, "label": td.label, "category": td.category, "needs_admin": td.needs_admin, "corp_safe": td.corp_safe}
                        for td in tweaks
                    ],
                    indent=2,
                )
            )
        else:
            print(f"{'ID':<30} {'Category':<14} {'Status':<14} Label")
            print("-" * 80)
            smap = _status_map_with_progress("Listing")
            for td in tweaks:
                st = smap.get(td.id, TweakResult.UNKNOWN)
                print(f"{td.id:<30} {td.category:<14} {st:<14} {td.label}")
        return 0

    if args.search:
        search_results = search_tweaks(args.search)
        if not search_results:
            print(f"No tweaks matching '{args.search}'.")
            return 0
        if getattr(args, "scope", None):
            search_results = [td for td in search_results if tweak_scope(td) == args.scope]
        if getattr(args, "min_build", None):
            search_results = [td for td in search_results if td.min_build <= args.min_build]
        if getattr(args, "corp_safe", False):
            search_results = [td for td in search_results if td.corp_safe]
        if getattr(args, "needs_admin", False):
            search_results = [td for td in search_results if td.needs_admin]
        if getattr(args, "output", "table") == "json":
            import json as _j

            print(_j.dumps([{"id": td.id, "label": td.label, "category": td.category, "tags": td.tags} for td in search_results], indent=2))
        else:
            print(f"{'ID':<30} {'Category':<14} {'Status':<14} Label")
            print("-" * 80)
            smap_search = _status_map_with_progress("Searching")
            for td in search_results:
                st = smap_search.get(td.id, TweakResult.UNKNOWN)
                print(f"{td.id:<30} {td.category:<14} {st:<14} {td.label}")
            print(f"\n{len(search_results)} tweak(s) found.")
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
            return 0
        if getattr(args, "html", None):
            _write_diff_html(diffs, Path(args.html), args.snapshot_diff[0], args.snapshot_diff[1])
            print(f"✅ HTML diff report written to {args.html}")
        else:
            _print_diff_coloured(diffs, args.snapshot_diff[0], args.snapshot_diff[1])
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
        if SESSION.dry_run:
            print(
                f"\U0001f50d Dry-run profile '{args.profile}': {ok}/{len(results)} tweaks would be applied ({SESSION.dry_ops} registry ops skipped)."
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
        cat_errors: list[str] = []
        for td in cat_tweaks:
            fn = td.apply_fn if args.mode == "apply" else td.remove_fn
            try:
                fn(require_admin=td.needs_admin)
            except (AdminRequirementError, OSError, RuntimeError, ValueError) as exc:
                cat_errors.append(f"{td.label}: {exc}")
        ok = len(cat_tweaks) - len(cat_errors)
        if SESSION.dry_run:
            print(
                f"\U0001f50d Dry-run: {ok}/{len(cat_tweaks)} tweaks would be processed in '{args.category}' ({SESSION.dry_ops} registry ops skipped)."
            )
        else:
            print(f"\u2705 {ok}/{len(cat_tweaks)} tweaks processed in '{args.category}'.")
        for e in cat_errors:
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
            ids: list[str] = [str(x) for x in data.get("tweaks", [])]
        elif isinstance(data, list):
            ids = [str(x) for x in data]
        else:
            print('\u274c Expected a JSON list of tweak IDs or {"tweaks": [...]}.')
            return 3
        targets = []
        for tid in ids:
            matching = get_tweak(tid)
            if matching is None:
                print(f"\u26a0\ufe0f  Skipping unknown tweak '{tid}'")
            else:
                targets.append(matching)
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
                fn(require_admin=td.needs_admin)
            except (AdminRequirementError, OSError, RuntimeError, ValueError) as exc:
                errors_list.append(f"{td.label}: {exc}")
        ok = len(targets) - len(errors_list)
        if SESSION.dry_run:
            print(f"\U0001f50d Dry-run: {ok}/{len(targets)} tweaks from JSON ({SESSION.dry_ops} registry ops skipped).")
        else:
            print(f"\u2705 {ok}/{len(targets)} tweaks processed from JSON.")
        for e in errors_list:
            print(f"  \u274c {e}")
        return 0

    if args.gui:
        from .gui import launch

        launch()
        return 0

    if args.menu:
        from .menu import Menu as _CliMenu

        _CliMenu(force_corp=args.force).loop()
        return 0

    if args.mode == "status":
        found = get_tweak(args.tweak)
        if found:
            print(f"{found.label}: {tweak_status(found)}")
        else:
            print(f"❌ Unknown tweak '{args.tweak}'.")
            return 2
        return 0
    if args.mode:
        return _run_action(args.mode, args.tweak, assume_yes=args.assume_yes, force=args.force)

    # Interactive menu
    if not is_windows():
        print(f"⚠️  This menu is intended for Windows. Detected: {platform_summary()}")
        return 4

    menu = Menu(force_corp=args.force)
    menu.loop()
    return 0


if __name__ == "__main__":  # pragma: no cover
    sys.exit(main())
