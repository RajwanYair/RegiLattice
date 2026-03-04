"""Java runtime registry tweaks.

Covers: auto-update, security prompts, web plugin.
"""

from __future__ import annotations

from typing import List

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Key paths ────────────────────────────────────────────────────────────────

_JAVA_UPDATE = r"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy"
_JAVA_UPDATE32 = r"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\JavaSoft\Java Update\Policy"
_JAVA_DEPLOY = r"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Plug-in"
_JAVA_WEB = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"


# ── Disable Java Auto-Update ────────────────────────────────────────────────


def _apply_disable_update(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Java: disable auto-update")
    SESSION.backup([_JAVA_UPDATE, _JAVA_UPDATE32], "JavaUpdate")
    SESSION.set_dword(_JAVA_UPDATE, "EnableJavaUpdate", 0)
    SESSION.set_dword(_JAVA_UPDATE, "NotifyDownload", 0)
    SESSION.set_dword(_JAVA_UPDATE32, "EnableJavaUpdate", 0)
    SESSION.set_dword(_JAVA_UPDATE32, "NotifyDownload", 0)


def _remove_disable_update(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_JAVA_UPDATE, "EnableJavaUpdate", 1)
    SESSION.set_dword(_JAVA_UPDATE32, "EnableJavaUpdate", 1)


def _detect_disable_update() -> bool:
    return SESSION.read_dword(_JAVA_UPDATE, "EnableJavaUpdate") == 0


# ── Disable Java Web Plugin ─────────────────────────────────────────────────


def _apply_disable_web(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Java: disable browser web plugin")
    SESSION.backup([_JAVA_WEB], "JavaWebPlugin")
    SESSION.set_dword(_JAVA_WEB, "deployment.webjava.enabled", 0)


def _remove_disable_web(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_JAVA_WEB, "deployment.webjava.enabled", 1)


def _detect_disable_web() -> bool:
    return SESSION.read_dword(_JAVA_WEB, "deployment.webjava.enabled") == 0


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: List[TweakDef] = [
    TweakDef(
        id="disable-java-update",
        label="Disable Java Auto-Update",
        category="Java",
        apply_fn=_apply_disable_update,
        remove_fn=_remove_disable_update,
        detect_fn=_detect_disable_update,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_JAVA_UPDATE, _JAVA_UPDATE32],
        description="Disables the Java automatic update scheduler (both 32-bit and 64-bit).",
        tags=["java", "update"],
    ),
    TweakDef(
        id="disable-java-web-plugin",
        label="Disable Java Web Plugin",
        category="Java",
        apply_fn=_apply_disable_web,
        remove_fn=_remove_disable_web,
        detect_fn=_detect_disable_web,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_JAVA_WEB],
        description="Disables the Java browser web plugin for better security.",
        tags=["java", "security", "web"],
    ),
]
