"""Package management tweaks — PowerShell, Scoop, Python/pip, winget.

These tweaks configure package managers and developer tool policies
via registry settings and shell commands.
"""

from __future__ import annotations

import subprocess
from typing import List

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── PowerShell Execution Policy (CurrentUser) ───────────────────────────────

_PS_KEY = (
    r"HKEY_CURRENT_USER\SOFTWARE\Microsoft\PowerShell\1\ShellIds\Microsoft.PowerShell"
)


def apply_ps_unrestricted(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-PSUnrestricted")
    SESSION.backup([_PS_KEY], "PSExecutionPolicy")
    SESSION.set_string(_PS_KEY, "ExecutionPolicy", "RemoteSigned")
    SESSION.log("Completed Add-PSUnrestricted")


def remove_ps_unrestricted(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-PSUnrestricted")
    SESSION.backup([_PS_KEY], "PSExecutionPolicy_Remove")
    SESSION.set_string(_PS_KEY, "ExecutionPolicy", "Restricted")
    SESSION.log("Completed Remove-PSUnrestricted")


def detect_ps_unrestricted() -> bool:
    val = SESSION.read_string(_PS_KEY, "ExecutionPolicy")
    return val is not None and val.lower() in ("remotesigned", "unrestricted", "bypass")


# ── PowerShell Module Auto-Install (PSGallery trust) ────────────────────────


def apply_ps_gallery_trust(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-PSGalleryTrust")
    try:
        # Set PSGallery as trusted via PowerShell command
        subprocess.run(
            [
                "powershell",
                "-NoProfile",
                "-Command",
                "Set-PSRepository -Name PSGallery -InstallationPolicy Trusted",
            ],
            capture_output=True,
            text=True,
            timeout=30,
        )
    except Exception as exc:
        SESSION.log(f"PSGallery trust warning: {exc}")
    SESSION.log("Completed Add-PSGalleryTrust")


def remove_ps_gallery_trust(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-PSGalleryTrust")
    try:
        subprocess.run(
            [
                "powershell",
                "-NoProfile",
                "-Command",
                "Set-PSRepository -Name PSGallery -InstallationPolicy Untrusted",
            ],
            capture_output=True,
            text=True,
            timeout=30,
        )
    except Exception as exc:
        SESSION.log(f"PSGallery untrust warning: {exc}")
    SESSION.log("Completed Remove-PSGalleryTrust")


def detect_ps_gallery_trust() -> bool:
    try:
        r = subprocess.run(
            [
                "powershell",
                "-NoProfile",
                "-Command",
                "(Get-PSRepository -Name PSGallery).InstallationPolicy",
            ],
            capture_output=True,
            text=True,
            timeout=15,
        )
        return r.stdout.strip().lower() == "trusted"
    except Exception:
        return False


# ── Scoop Bucket Management ─────────────────────────────────────────────────

_SCOOP_ENV = r"HKEY_CURRENT_USER\Environment"


def apply_scoop_setup(*, require_admin: bool = False) -> None:
    """Ensure Scoop is installed and add common buckets."""
    assert_admin(require_admin)
    SESSION.log("Starting Add-ScoopSetup")
    # Check if scoop is available
    try:
        r = subprocess.run(
            ["scoop", "--version"], capture_output=True, text=True, timeout=15
        )
        if r.returncode != 0:
            raise FileNotFoundError
    except (FileNotFoundError, OSError):
        # Install scoop
        SESSION.log("Installing Scoop...")
        subprocess.run(
            [
                "powershell",
                "-NoProfile",
                "-Command",
                "Set-ExecutionPolicy RemoteSigned -Scope CurrentUser -Force; "
                "iwr -useb get.scoop.sh | iex",
            ],
            capture_output=True,
            text=True,
            timeout=120,
        )
    # Add common buckets
    for bucket in ("extras", "versions", "nerd-fonts"):
        subprocess.run(
            ["scoop", "bucket", "add", bucket],
            capture_output=True,
            text=True,
            timeout=30,
        )
    SESSION.log("Completed Add-ScoopSetup")


def remove_scoop_setup(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-ScoopSetup — skipping (Scoop left installed)")
    # We don't uninstall scoop; just log the action
    SESSION.log("Scoop removal skipped (manual: scoop uninstall scoop)")


def detect_scoop_setup() -> bool:
    try:
        r = subprocess.run(
            ["scoop", "--version"], capture_output=True, text=True, timeout=10
        )
        return r.returncode == 0
    except (FileNotFoundError, OSError):
        return False


# ── Winget Settings via Registry ─────────────────────────────────────────────

_WINGET_KEY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller"


def apply_enable_winget(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-EnableWinget")
    SESSION.backup([_WINGET_KEY], "Winget")
    SESSION.set_dword(_WINGET_KEY, "EnableAppInstaller", 1)
    SESSION.set_dword(_WINGET_KEY, "EnableExperimentalFeatures", 1)
    SESSION.set_dword(_WINGET_KEY, "EnableHashOverride", 1)
    SESSION.log("Completed Add-EnableWinget")


def remove_enable_winget(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-EnableWinget")
    SESSION.backup([_WINGET_KEY], "Winget_Remove")
    SESSION.delete_value(_WINGET_KEY, "EnableAppInstaller")
    SESSION.delete_value(_WINGET_KEY, "EnableExperimentalFeatures")
    SESSION.delete_value(_WINGET_KEY, "EnableHashOverride")
    SESSION.log("Completed Remove-EnableWinget")


def detect_enable_winget() -> bool:
    return SESSION.read_dword(_WINGET_KEY, "EnableAppInstaller") == 1


# ── Python pip Configuration (user-space) ────────────────────────────────────

_PIP_ENV = r"HKEY_CURRENT_USER\Environment"


def apply_pip_user_default(*, require_admin: bool = False) -> None:
    """Set PIP_USER=1 so pip defaults to --user installs."""
    assert_admin(require_admin)
    SESSION.log("Starting Add-PipUserDefault")
    SESSION.backup([_PIP_ENV], "PipUser")
    SESSION.set_string(_PIP_ENV, "PIP_USER", "1")
    SESSION.set_string(_PIP_ENV, "PIP_NO_WARN_SCRIPT_LOCATION", "0")
    SESSION.log("Completed Add-PipUserDefault")


def remove_pip_user_default(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-PipUserDefault")
    SESSION.backup([_PIP_ENV], "PipUser_Remove")
    SESSION.delete_value(_PIP_ENV, "PIP_USER")
    SESSION.delete_value(_PIP_ENV, "PIP_NO_WARN_SCRIPT_LOCATION")
    SESSION.log("Completed Remove-PipUserDefault")


def detect_pip_user_default() -> bool:
    return SESSION.read_string(_PIP_ENV, "PIP_USER") == "1"


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: List[TweakDef] = [
    TweakDef(
        id="ps-remotesigned",
        label="PowerShell RemoteSigned Policy",
        category="Package Management",
        apply_fn=apply_ps_unrestricted,
        remove_fn=remove_ps_unrestricted,
        detect_fn=detect_ps_unrestricted,
        needs_admin=False,
        corp_safe=False,
        registry_keys=[_PS_KEY],
        description=(
            "Sets PowerShell execution policy to RemoteSigned for "
            "the current user, enabling local scripts."
        ),
        tags=["powershell", "security", "scripting"],
    ),
    TweakDef(
        id="ps-gallery-trust",
        label="Trust PSGallery Repository",
        category="Package Management",
        apply_fn=apply_ps_gallery_trust,
        remove_fn=remove_ps_gallery_trust,
        detect_fn=detect_ps_gallery_trust,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[],
        description="Sets PSGallery as a trusted repository for Install-Module.",
        tags=["powershell", "packages", "gallery"],
    ),
    TweakDef(
        id="scoop-setup",
        label="Install & Configure Scoop",
        category="Package Management",
        apply_fn=apply_scoop_setup,
        remove_fn=remove_scoop_setup,
        detect_fn=detect_scoop_setup,
        needs_admin=False,
        corp_safe=False,
        registry_keys=[],
        description=(
            "Installs Scoop package manager (if missing) and adds "
            "extras, versions, and nerd-fonts buckets."
        ),
        tags=["scoop", "packages", "installer"],
    ),
    TweakDef(
        id="enable-winget",
        label="Enable Winget (App Installer)",
        category="Package Management",
        apply_fn=apply_enable_winget,
        remove_fn=remove_enable_winget,
        detect_fn=detect_enable_winget,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_WINGET_KEY],
        description=(
            "Enables winget (App Installer), experimental features, "
            "and hash override via Group Policy."
        ),
        tags=["winget", "packages", "installer"],
    ),
    TweakDef(
        id="pip-user-default",
        label="Pip Default --user Install",
        category="Package Management",
        apply_fn=apply_pip_user_default,
        remove_fn=remove_pip_user_default,
        detect_fn=detect_pip_user_default,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_PIP_ENV],
        description=(
            "Sets PIP_USER=1 environment variable so pip installs "
            "to user site-packages by default (no admin required)."
        ),
        tags=["python", "pip", "packages"],
    ),
]
