"""Scoop package manager tools — list, install, remove individual tools.

Provides tweaks for managing popular developer and power-user tools
via the Scoop package manager. Each tool can be installed or removed
individually through the GUI/CLI.
"""

from __future__ import annotations

import re
import subprocess

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# Words that appear in scoop list headers — skip any line whose first token matches.
_SCOOP_HEADER_WORDS: frozenset[str] = frozenset({"name", "version", "source", "updated", "info", "apps"})
# Valid scoop package name: letters, digits, hyphen, dot, underscore (no leading dash).
_SCOOP_NAME_RE: re.Pattern[str] = re.compile(r"^[A-Za-z0-9][A-Za-z0-9._\-]*$")

# ── Helpers ──────────────────────────────────────────────────────────────────────────────────


def _scoop_installed() -> bool:
    """Return True if scoop CLI is available."""
    try:
        r = subprocess.run(["scoop", "--version"], capture_output=True, text=True, timeout=10, check=False)
        return r.returncode == 0
    except (FileNotFoundError, OSError):
        return False


def _is_scoop_app_installed(app_name: str) -> bool:
    """Check if a specific scoop app is installed."""
    try:
        r = subprocess.run(["scoop", "list"], capture_output=True, text=True, timeout=15, check=False)
        return app_name.lower() in r.stdout.lower()
    except (FileNotFoundError, OSError):
        return False


def _install_scoop_app(app_name: str, bucket: str = "") -> None:
    """Install a scoop app (with optional bucket)."""
    SESSION.log(f"Scoop: installing {app_name}")
    if not _scoop_installed():
        raise RuntimeError("Scoop is not installed. Apply 'Install & Configure Scoop' first.")
    if bucket:
        subprocess.run(["scoop", "bucket", "add", bucket], capture_output=True, text=True, timeout=30, check=False)
    subprocess.run(["scoop", "install", app_name], capture_output=True, text=True, timeout=120, check=False)
    SESSION.log(f"Scoop: installed {app_name}")


def _remove_scoop_app(app_name: str) -> None:
    """Remove a scoop app."""
    SESSION.log(f"Scoop: removing {app_name}")
    subprocess.run(["scoop", "uninstall", app_name], capture_output=True, text=True, timeout=60, check=False)
    SESSION.log(f"Scoop: removed {app_name}")


def list_installed_scoop_apps() -> list[str]:
    """Return list of currently installed scoop app names.

    Handles both legacy (``Installed apps: / appname version``) and modern
    (table with ``Name  Version  Source  Updated  Info`` header) scoop output.
    """
    try:
        r = subprocess.run(["scoop", "list"], capture_output=True, text=True, timeout=15, check=False)
        if r.returncode != 0:
            return []
        apps: list[str] = []
        for line in r.stdout.strip().splitlines():
            parts = line.strip().split()
            if not parts:
                continue
            name = parts[0].rstrip(":")
            # Skip separator lines (----...) and known header/keyword tokens.
            if name.startswith("-") or name.lower() in _SCOOP_HEADER_WORDS:
                continue
            # Accept any valid package name: letters, digits, hyphen, dot, underscore.
            if _SCOOP_NAME_RE.match(name):
                apps.append(name)
        return sorted(apps)
    except (FileNotFoundError, OSError):
        return []


def search_scoop_apps(query: str) -> list[str]:
    """Search for available scoop apps matching query."""
    try:
        r = subprocess.run(
            ["scoop", "search", query],
            capture_output=True,
            text=True,
            timeout=30,
            check=False,
        )
        if r.returncode != 0:
            return []
        results: list[str] = []
        for line in r.stdout.strip().splitlines():
            parts = line.strip().split()
            if parts and not parts[0].startswith("-") and not parts[0].startswith("Results"):
                results.append(parts[0].strip())
        return results
    except (FileNotFoundError, OSError):
        return []


def _make_scoop_tweak(
    app_id: str,
    app_name: str,
    label: str,
    description: str,
    bucket: str = "",
    tags: list[str] | None = None,
) -> TweakDef:
    """Factory for scoop-app TweakDef instances."""

    def _apply(*, require_admin: bool = False) -> None:
        _install_scoop_app(app_name, bucket=bucket)

    def _remove(*, require_admin: bool = False) -> None:
        _remove_scoop_app(app_name)

    def _detect() -> bool:
        return _is_scoop_app_installed(app_name)

    return TweakDef(
        id=f"scoop-{app_id}",
        label=label,
        category="Scoop Tools",
        apply_fn=_apply,
        remove_fn=_remove,
        detect_fn=_detect,
        needs_admin=False,
        corp_safe=False,
        registry_keys=[],
        description=description,
        tags=["scoop", "tools"] + (tags or []),
    )


# ── Tool Definitions ─────────────────────────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    _make_scoop_tweak(
        "7zip",
        "7zip",
        "Scoop: 7-Zip",
        "7-Zip file archiver — fast compression/decompression "
        "supporting 7z, ZIP, RAR, TAR, GZIP, and more. "
        "Default: Not installed. Recommended: Install.",
        tags=["archiver", "compression"],
    ),
    _make_scoop_tweak(
        "git",
        "git",
        "Scoop: Git",
        "Git distributed version control system. "
        "Required by many scoop buckets and developer workflows. "
        "Default: Not installed. Recommended: Install.",
        tags=["git", "vcs", "developer"],
    ),
    _make_scoop_tweak(
        "curl",
        "curl",
        "Scoop: cURL",
        "cURL command-line tool for transferring data with URLs. "
        "Supports HTTP, HTTPS, FTP, and many other protocols. "
        "Default: Not installed. Recommended: Install.",
        tags=["networking", "download"],
    ),
    _make_scoop_tweak(
        "wget",
        "wget",
        "Scoop: wget",
        "GNU Wget — non-interactive network downloader. "
        "Supports HTTP, HTTPS, and FTP with recursive download. "
        "Default: Not installed. Recommended: Install.",
        tags=["networking", "download"],
    ),
    _make_scoop_tweak(
        "fzf",
        "fzf",
        "Scoop: fzf (fuzzy finder)",
        "fzf is a general-purpose command-line fuzzy finder. "
        "Blazing fast, works with any list — files, history, processes. "
        "Default: Not installed. Recommended: Install.",
        tags=["search", "productivity"],
    ),
    _make_scoop_tweak(
        "ripgrep",
        "ripgrep",
        "Scoop: ripgrep (rg)",
        "ripgrep (rg) — lightning-fast regex search tool. "
        "Recursively searches directories, respects .gitignore. "
        "Default: Not installed. Recommended: Install.",
        tags=["search", "developer"],
    ),
    _make_scoop_tweak(
        "fd",
        "fd",
        "Scoop: fd (find alternative)",
        "fd is a fast, user-friendly alternative to 'find'. "
        "Colorized output, regex support, respects .gitignore. "
        "Default: Not installed. Recommended: Install.",
        tags=["search", "files", "developer"],
    ),
    _make_scoop_tweak(
        "bat",
        "bat",
        "Scoop: bat (cat with syntax)",
        "bat — a cat clone with syntax highlighting, git integration, "
        "and automatic paging. Drop-in replacement for cat. "
        "Default: Not installed. Recommended: Install.",
        tags=["terminal", "developer"],
    ),
    _make_scoop_tweak(
        "jq",
        "jq",
        "Scoop: jq (JSON processor)",
        "jq — lightweight command-line JSON processor. "
        "Slice, filter, map, and transform structured data. "
        "Default: Not installed. Recommended: Install.",
        tags=["json", "developer"],
    ),
    _make_scoop_tweak(
        "neovim",
        "neovim",
        "Scoop: Neovim",
        "Neovim — hyperextensible Vim-based text editor. Async plugins, built-in LSP, Lua scripting. Default: Not installed.",
        tags=["editor", "vim", "developer"],
    ),
    _make_scoop_tweak(
        "starship",
        "starship",
        "Scoop: Starship Prompt",
        "Starship — blazing-fast, cross-shell prompt in Rust. "
        "Shows git status, language versions, battery, and more. "
        "Default: Not installed. Recommended: Install.",
        tags=["terminal", "prompt", "shell"],
    ),
    _make_scoop_tweak(
        "delta",
        "delta",
        "Scoop: delta (git diff)",
        "delta — syntax-highlighting pager for git, diff, and grep. Modern, beautiful git diffs. Default: Not installed. Recommended: Install.",
        tags=["git", "diff", "developer"],
    ),
    _make_scoop_tweak(
        "everything",
        "everything",
        "Scoop: Everything Search",
        "Everything — instant file search for Windows by name. "
        "Indexes NTFS drives in seconds — near-zero memory usage. "
        "Default: Not installed. Recommended: Install.",
        bucket="extras",
        tags=["search", "files", "productivity"],
    ),
    _make_scoop_tweak(
        "gsudo",
        "gsudo",
        "Scoop: gsudo (sudo for Windows)",
        "gsudo — a sudo equivalent for Windows. Run elevated commands from your current console. Default: Not installed. Recommended: Install.",
        tags=["admin", "terminal", "elevation"],
    ),
    _make_scoop_tweak(
        "python",
        "python",
        "Scoop: Python",
        "Python interpreter managed via Scoop. Easy version switching with scoop reset. Default: Not installed.",
        tags=["python", "developer", "language"],
    ),
    _make_scoop_tweak(
        "nodejs",
        "nodejs",
        "Scoop: Node.js",
        "Node.js JavaScript runtime managed via Scoop. Includes npm package manager. Default: Not installed.",
        tags=["nodejs", "javascript", "developer"],
    ),
    _make_scoop_tweak(
        "btop",
        "btop",
        "Scoop: btop++ (Resource Monitor)",
        "btop++ \u2014 modern resource monitor with battery, CPU, memory, disk, network stats. "
        "Beautiful TUI. Default: Not installed. Recommended: Install.",
        tags=["monitor", "resource", "terminal"],
    ),
    _make_scoop_tweak(
        "lazygit",
        "lazygit",
        "Scoop: lazygit (Git TUI)",
        "lazygit \u2014 simple terminal UI for git commands. Stage, commit, rebase interactively. Default: Not installed. Recommended: Install.",
        tags=["git", "tui", "developer"],
    ),
    _make_scoop_tweak(
        "duf",
        "duf",
        "Scoop: duf (Disk Usage)",
        "duf \u2014 disk usage/free utility with colorful output. Modern alternative to df. Default: Not installed.",
        tags=["disk", "utility", "terminal"],
    ),
    _make_scoop_tweak(
        "tldr",
        "tldr",
        "Scoop: tldr (Simplified Man Pages)",
        "tldr \u2014 simplified, community-driven man pages. Quick command reference. Default: Not installed. Recommended: Install.",
        tags=["documentation", "terminal", "developer"],
    ),
]

TWEAKS += [
    _make_scoop_tweak(
        "dust",
        "dust",
        "Scoop: dust (Disk Usage TUI)",
        "dust -- a more intuitive version of du written in Rust. "
        "Visualizes disk usage with a bar chart in the terminal. "
        "Default: Not installed. Recommended: Install.",
        tags=["disk", "utility", "terminal", "rust"],
    ),
    _make_scoop_tweak(
        "hyperfine",
        "hyperfine",
        "Scoop: hyperfine (Benchmarking)",
        "hyperfine -- command-line benchmarking tool. "
        "Statistical analysis, warmup runs, export to CSV/JSON. "
        "Default: Not installed. Recommended: Install.",
        tags=["benchmark", "performance", "developer"],
    ),
]


# -- Scoop Registry-Based Environment Settings ---------------------------------

_ENV_CU = r"HKEY_CURRENT_USER\Environment"
_ENV_LM = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control"
    r"\Session Manager\Environment"
)


def _apply_scoop_global_path(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Scoop: setting global install path to C:\\ScoopGlobal")
    SESSION.backup([_ENV_LM], "ScoopGlobalPath")
    SESSION.set_string(_ENV_LM, "SCOOP_GLOBAL", r"C:\ScoopGlobal")


def _remove_scoop_global_path(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_ENV_LM, "SCOOP_GLOBAL")


def _detect_scoop_global_path() -> bool:
    return SESSION.read_string(_ENV_LM, "SCOOP_GLOBAL") == r"C:\ScoopGlobal"


def _apply_scoop_disable_autoupdate(*, require_admin: bool = True) -> None:
    SESSION.log("Scoop: disabling auto-update on install")
    SESSION.backup([_ENV_CU], "ScoopNoAutoUpdate")
    SESSION.set_string(_ENV_CU, "SCOOP_NO_AUTO_UPDATE", "1")


def _remove_scoop_disable_autoupdate(*, require_admin: bool = True) -> None:
    SESSION.delete_value(_ENV_CU, "SCOOP_NO_AUTO_UPDATE")


def _detect_scoop_disable_autoupdate() -> bool:
    return SESSION.read_string(_ENV_CU, "SCOOP_NO_AUTO_UPDATE") == "1"


def _apply_scoop_parallel_downloads(*, require_admin: bool = True) -> None:
    SESSION.log("Scoop: enabling parallel downloads via aria2")
    SESSION.backup([_ENV_CU], "ScoopParallelDL")
    SESSION.set_string(_ENV_CU, "SCOOP_ARIA2_ENABLED", "true")


def _remove_scoop_parallel_downloads(*, require_admin: bool = True) -> None:
    SESSION.delete_value(_ENV_CU, "SCOOP_ARIA2_ENABLED")


def _detect_scoop_parallel_downloads() -> bool:
    return SESSION.read_string(_ENV_CU, "SCOOP_ARIA2_ENABLED") == "true"


TWEAKS += [
    TweakDef(
        id="scoop-set-global-path",
        label="Set Scoop Global Install Path",
        category="Scoop Tools",
        apply_fn=_apply_scoop_global_path,
        remove_fn=_remove_scoop_global_path,
        detect_fn=_detect_scoop_global_path,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_ENV_LM],
        description=(
            "Sets the SCOOP_GLOBAL environment variable to C:\\ScoopGlobal "
            "for system-wide Scoop installations. "
            "Default: not set. Recommended: set for multi-user machines."
        ),
        tags=["scoop", "global", "path", "environment"],
    ),
    TweakDef(
        id="scoop-disable-autoupdate",
        label="Disable Scoop Auto-Update on Install",
        category="Scoop Tools",
        apply_fn=_apply_scoop_disable_autoupdate,
        remove_fn=_remove_scoop_disable_autoupdate,
        detect_fn=_detect_scoop_disable_autoupdate,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ENV_CU],
        description=(
            "Sets SCOOP_NO_AUTO_UPDATE=1 to prevent Scoop from "
            "auto-updating itself before every app install. "
            "Default: auto-update. Recommended: disabled for speed."
        ),
        tags=["scoop", "autoupdate", "speed", "environment"],
    ),
    TweakDef(
        id="scoop-parallel-downloads",
        label="Enable Scoop Parallel Downloads (aria2)",
        category="Scoop Tools",
        apply_fn=_apply_scoop_parallel_downloads,
        remove_fn=_remove_scoop_parallel_downloads,
        detect_fn=_detect_scoop_parallel_downloads,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ENV_CU],
        description=(
            "Sets SCOOP_ARIA2_ENABLED=true to enable parallel downloads "
            "via aria2 for faster Scoop package installs. "
            "Default: disabled. Recommended: enabled."
        ),
        tags=["scoop", "parallel", "downloads", "aria2", "speed"],
    ),
]
