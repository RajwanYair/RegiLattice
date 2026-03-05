"""Git for Windows registry tweaks.

Covers: credential manager, autocrlf, long paths, default editor.
"""

from __future__ import annotations

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Key paths ────────────────────────────────────────────────────────────────

_GIT = r"HKEY_LOCAL_MACHINE\SOFTWARE\GitForWindows"
_GIT_USER = r"HKEY_CURRENT_USER\Software\GitForWindows"


# ── Set Git Credential Manager to Windows ────────────────────────────────────


def _apply_credential_manager(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Git: configure credential.helper=manager")
    SESSION.backup([_GIT], "GitCredMgr")
    SESSION.set_string(_GIT, "CredentialHelper", "manager")


def _remove_credential_manager(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_GIT, "CredentialHelper")


def _detect_credential_manager() -> bool:
    return SESSION.read_string(_GIT, "CredentialHelper") == "manager"


# ── Enable Git Long Paths Support ────────────────────────────────────────────


def _apply_git_longpaths(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Git: enable core.longpaths")
    SESSION.backup([_GIT], "GitLongPaths")
    SESSION.set_dword(_GIT, "LongPaths", 1)


def _remove_git_longpaths(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_GIT, "LongPaths")


def _detect_git_longpaths() -> bool:
    return SESSION.read_dword(_GIT, "LongPaths") == 1


# ── Set Git Default Branch to main ───────────────────────────────────────────


def _apply_default_branch(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Git: set default branch name to 'main'")
    SESSION.backup([_GIT], "GitDefaultBranch")
    SESSION.set_string(_GIT, "DefaultBranch", "main")


def _remove_default_branch(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_GIT, "DefaultBranch")


def _detect_default_branch() -> bool:
    return SESSION.read_string(_GIT, "DefaultBranch") == "main"


# ── Set Git autocrlf to input ─────────────────────────────────────────────


def _apply_autocrlf(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Git: set core.autocrlf=input (LF in repo, native checkout)")
    SESSION.backup([_GIT], "GitAutoCrlf")
    SESSION.set_string(_GIT, "AutoCRLF", "input")


def _remove_autocrlf(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_string(_GIT, "AutoCRLF", "true")  # Windows default


def _detect_autocrlf() -> bool:
    return SESSION.read_string(_GIT, "AutoCRLF") == "input"


# ── Set Git Default Editor ───────────────────────────────────────────────


def _apply_editor_code(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Git: set core.editor to VS Code (--wait)")
    SESSION.backup([_GIT], "GitEditor")
    SESSION.set_string(_GIT, "Editor", "code --wait")


def _remove_editor_code(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_GIT, "Editor")


def _detect_editor_code() -> bool:
    v = SESSION.read_string(_GIT, "Editor")
    return v is not None and "code" in v.lower()


# ── Enable Git Built-in FS Monitor ───────────────────────────────────────────


def _apply_fsmonitor() -> None:
    SESSION.log("Git: enable built-in FS monitor")
    SESSION.backup([_GIT_USER], "GitFSMonitor")
    SESSION.set_string(_GIT_USER, "FSMonitor", "true")


def _remove_fsmonitor() -> None:
    SESSION.delete_value(_GIT_USER, "FSMonitor")


def _detect_fsmonitor() -> bool:
    return SESSION.read_string(_GIT_USER, "FSMonitor") == "true"


# ── Enable Git manyFiles Feature ────────────────────────────────────────────


def _apply_manyfiles() -> None:
    SESSION.log("Git: enable manyFiles feature flag")
    SESSION.backup([_GIT_USER], "GitManyFiles")
    SESSION.set_string(_GIT_USER, "ManyFiles", "true")


def _remove_manyfiles() -> None:
    SESSION.delete_value(_GIT_USER, "ManyFiles")


def _detect_manyfiles() -> bool:
    return SESSION.read_string(_GIT_USER, "ManyFiles") == "true"


# ── Enable Git Parallel Checkout ─────────────────────────────────────────────


def _apply_parallel_checkout() -> None:
    SESSION.log("Git: enable parallel checkout")
    SESSION.backup([_GIT_USER], "GitParallelCheckout")
    SESSION.set_string(_GIT_USER, "ParallelCheckout", "true")


def _remove_parallel_checkout() -> None:
    SESSION.delete_value(_GIT_USER, "ParallelCheckout")


def _detect_parallel_checkout() -> bool:
    return SESSION.read_string(_GIT_USER, "ParallelCheckout") == "true"


# ── Set Git Auto-GC Threshold to 512 ────────────────────────────────────────


def _apply_gc_auto() -> None:
    SESSION.log("Git: set gc.auto threshold to 512")
    SESSION.backup([_GIT_USER], "GitGCAutoThreshold")
    SESSION.set_string(_GIT_USER, "GCAutoThreshold", "512")


def _remove_gc_auto() -> None:
    SESSION.delete_value(_GIT_USER, "GCAutoThreshold")


def _detect_gc_auto() -> bool:
    return SESSION.read_string(_GIT_USER, "GCAutoThreshold") == "512"


# ── Increase Git Delta Cache Limit ──────────────────────────────────────────


def _apply_delta_cache() -> None:
    SESSION.log("Git: increase pack delta cache size to 4096")
    SESSION.backup([_GIT_USER], "GitDeltaCacheSize")
    SESSION.set_string(_GIT_USER, "DeltaCacheSize", "4096")


def _remove_delta_cache() -> None:
    SESSION.delete_value(_GIT_USER, "DeltaCacheSize")


def _detect_delta_cache() -> bool:
    return SESSION.read_string(_GIT_USER, "DeltaCacheSize") == "4096"


# ── Enable Git Commit GPG Signing ───────────────────────────────────────────


def _apply_commit_gpgsign() -> None:
    SESSION.log("Git: enable commit.gpgsign")
    SESSION.backup([_GIT_USER], "GitGPGSign")
    SESSION.set_string(_GIT_USER, "CommitGPGSign", "true")


def _remove_commit_gpgsign() -> None:
    SESSION.delete_value(_GIT_USER, "CommitGPGSign")


def _detect_commit_gpgsign() -> bool:
    return SESSION.read_string(_GIT_USER, "CommitGPGSign") == "true"


# ── Enable Git Fetch Prune ──────────────────────────────────────────────────


def _apply_fetch_prune() -> None:
    SESSION.log("Git: enable fetch.prune (auto-prune stale remote branches)")
    SESSION.backup([_GIT_USER], "GitFetchPrune")
    SESSION.set_string(_GIT_USER, "FetchPrune", "true")


def _remove_fetch_prune() -> None:
    SESSION.delete_value(_GIT_USER, "FetchPrune")


def _detect_fetch_prune() -> bool:
    return SESSION.read_string(_GIT_USER, "FetchPrune") == "true"


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="git-credential-manager",
        label="Git: Use Credential Manager",
        category="Developer Tools",
        apply_fn=_apply_credential_manager,
        remove_fn=_remove_credential_manager,
        detect_fn=_detect_credential_manager,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_GIT],
        description="Configures Git to use the Windows Credential Manager for HTTPS.",
        tags=["git", "credentials", "developer"],
    ),
    TweakDef(
        id="git-long-paths",
        label="Git: Enable Long Paths",
        category="Developer Tools",
        apply_fn=_apply_git_longpaths,
        remove_fn=_remove_git_longpaths,
        detect_fn=_detect_git_longpaths,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_GIT],
        description="Enables core.longpaths in Git to work with paths > 260 characters.",
        tags=["git", "longpaths", "developer"],
    ),
    TweakDef(
        id="git-default-branch-main",
        label="Git: Default Branch → main",
        category="Developer Tools",
        apply_fn=_apply_default_branch,
        remove_fn=_remove_default_branch,
        detect_fn=_detect_default_branch,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_GIT],
        description="Sets the default branch name to 'main' for new Git repositories.",
        tags=["git", "branch", "developer"],
    ),
    TweakDef(
        id="git-autocrlf-input",
        label="Git: autocrlf=input (LF in Repo)",
        category="Developer Tools",
        apply_fn=_apply_autocrlf,
        remove_fn=_remove_autocrlf,
        detect_fn=_detect_autocrlf,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_GIT],
        description="Sets core.autocrlf to 'input' so files are stored with LF in the repo.",
        tags=["git", "line-endings", "developer"],
    ),
    TweakDef(
        id="git-editor-vscode",
        label="Git: Default Editor → VS Code",
        category="Developer Tools",
        apply_fn=_apply_editor_code,
        remove_fn=_remove_editor_code,
        detect_fn=_detect_editor_code,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_GIT],
        description="Sets the default Git editor to VS Code with --wait.",
        tags=["git", "editor", "vscode", "developer"],
    ),
    TweakDef(
        id="git-fsmonitor",
        label="Git: Enable Built-in FS Monitor",
        category="Developer Tools",
        apply_fn=_apply_fsmonitor,
        remove_fn=_remove_fsmonitor,
        detect_fn=_detect_fsmonitor,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_GIT_USER],
        description="Enables Git's built-in file system monitor for faster status commands in large repos.",
        tags=["git", "fsmonitor", "performance", "developer"],
    ),
    TweakDef(
        id="git-manyfiles",
        label="Git: Enable manyFiles Feature",
        category="Developer Tools",
        apply_fn=_apply_manyfiles,
        remove_fn=_remove_manyfiles,
        detect_fn=_detect_manyfiles,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_GIT_USER],
        description="Enables Git manyFiles feature flag optimizing index-heavy operations.",
        tags=["git", "manyfiles", "performance", "developer"],
    ),
    TweakDef(
        id="git-parallel-checkout",
        label="Git: Enable Parallel Checkout",
        category="Developer Tools",
        apply_fn=_apply_parallel_checkout,
        remove_fn=_remove_parallel_checkout,
        detect_fn=_detect_parallel_checkout,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_GIT_USER],
        description="Enables parallel file checkout for faster git clone and checkout operations.",
        tags=["git", "checkout", "performance", "developer"],
    ),
    TweakDef(
        id="git-gc-auto",
        label="Git: Set Auto-GC Threshold to 512",
        category="Developer Tools",
        apply_fn=_apply_gc_auto,
        remove_fn=_remove_gc_auto,
        detect_fn=_detect_gc_auto,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_GIT_USER],
        description="Raises the threshold for automatic garbage collection from 256 to 512 loose objects.",
        tags=["git", "gc", "performance", "developer"],
    ),
    TweakDef(
        id="git-delta-cache",
        label="Git: Increase Delta Cache Limit",
        category="Developer Tools",
        apply_fn=_apply_delta_cache,
        remove_fn=_remove_delta_cache,
        detect_fn=_detect_delta_cache,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_GIT_USER],
        description="Increases the pack delta cache size to 4 GB for faster pack operations on large repos.",
        tags=["git", "delta-cache", "performance", "developer"],
    ),
    TweakDef(
        id="git-commit-gpgsign",
        label="Git: Enable Commit GPG Signing",
        category="Developer Tools",
        apply_fn=_apply_commit_gpgsign,
        remove_fn=_remove_commit_gpgsign,
        detect_fn=_detect_commit_gpgsign,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_GIT_USER],
        description="Enables GPG signing for all Git commits by default (commit.gpgsign=true).",
        tags=["git", "gpg", "signing", "security", "developer"],
    ),
    TweakDef(
        id="git-fetch-prune",
        label="Git: Enable Fetch Prune",
        category="Developer Tools",
        apply_fn=_apply_fetch_prune,
        remove_fn=_remove_fetch_prune,
        detect_fn=_detect_fetch_prune,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_GIT_USER],
        description=(
            "Automatically prunes stale remote-tracking branches on fetch "
            "(fetch.prune=true). Keeps branch lists clean."
        ),
        tags=["git", "fetch", "prune", "cleanup", "developer"],
    ),
]

# -- Key paths (system-wide developer helpers) --------------------------------

_WIN_SEARCH_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"
_FILESYSTEM_CTL = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"


# -- Prevent indexing on dev workloads ----------------------------------------


def _apply_disable_search_indexing_dev(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Dev: prevent Windows Search from indexing dev folders (set large low-disk threshold)")
    SESSION.backup([_WIN_SEARCH_POLICY], "DevSearchIndex")
    SESSION.set_dword(_WIN_SEARCH_POLICY, "PreventIndexingLowDiskSpaceMB", 999999)


def _remove_disable_search_indexing_dev(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_WIN_SEARCH_POLICY], "DevSearchIndex_Remove")
    SESSION.delete_value(_WIN_SEARCH_POLICY, "PreventIndexingLowDiskSpaceMB")


def _detect_disable_search_indexing_dev() -> bool:
    return SESSION.read_dword(_WIN_SEARCH_POLICY, "PreventIndexingLowDiskSpaceMB") == 999999


# -- Enable Win32 Long Path Support -------------------------------------------


def _apply_enable_long_paths(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Dev: enable Win32 long path support")
    SESSION.backup([_FILESYSTEM_CTL], "LongPathsEnabled")
    SESSION.set_dword(_FILESYSTEM_CTL, "LongPathsEnabled", 1)


def _remove_enable_long_paths(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_FILESYSTEM_CTL], "LongPathsEnabled_Remove")
    SESSION.set_dword(_FILESYSTEM_CTL, "LongPathsEnabled", 0)


def _detect_enable_long_paths() -> bool:
    return SESSION.read_dword(_FILESYSTEM_CTL, "LongPathsEnabled") == 1


TWEAKS += [
    TweakDef(
        id="dev-disable-windows-search-indexing-dev-folders",
        label="Prevent Windows Search Indexing on Dev Folders",
        category="Developer Tools",
        apply_fn=_apply_disable_search_indexing_dev,
        remove_fn=_remove_disable_search_indexing_dev,
        detect_fn=_detect_disable_search_indexing_dev,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_WIN_SEARCH_POLICY],
        description=(
            "Sets PreventIndexingLowDiskSpaceMB to 999999 to effectively disable "
            "Windows Search indexing on developer workloads. Reduces I/O contention "
            "during builds. Default: not set. Recommended: Apply on dev machines."
        ),
        tags=["developer", "search", "indexing", "performance", "build"],
    ),
    TweakDef(
        id="dev-enable-long-paths",
        label="Enable Win32 Long Path Support",
        category="Developer Tools",
        apply_fn=_apply_enable_long_paths,
        remove_fn=_remove_enable_long_paths,
        detect_fn=_detect_enable_long_paths,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_FILESYSTEM_CTL],
        description=(
            "Enables Win32 long path support (paths > 260 chars). Required for deep "
            "node_modules, Rust target dirs, and deeply nested repos. "
            "Default: 0 (disabled). Recommended: 1 (enabled)."
        ),
        tags=["developer", "longpath", "filesystem", "nodejs", "rust"],
    ),
]
