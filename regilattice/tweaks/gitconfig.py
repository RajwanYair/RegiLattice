"""Git for Windows registry tweaks.

Covers: credential manager, autocrlf, long paths, default editor.
"""

from __future__ import annotations

from typing import List

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Key paths ────────────────────────────────────────────────────────────────

_GIT = r"HKEY_LOCAL_MACHINE\SOFTWARE\GitForWindows"


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


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: List[TweakDef] = [
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
]
