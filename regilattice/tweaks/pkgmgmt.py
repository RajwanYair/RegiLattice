"""Package management tweaks — PowerShell, Scoop, Python/pip, winget.

These tweaks configure package managers and developer tool policies
via registry settings and shell commands.
"""

from __future__ import annotations

import subprocess

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


# ── Pip Disable Cache (save disk) ──────────────────────────────────────────


def apply_pip_no_cache(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Pip: set PIP_NO_CACHE_DIR=1 to save disk space")
    SESSION.backup([_PIP_ENV], "PipNoCache")
    SESSION.set_string(_PIP_ENV, "PIP_NO_CACHE_DIR", "1")


def remove_pip_no_cache(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_PIP_ENV, "PIP_NO_CACHE_DIR")


def detect_pip_no_cache() -> bool:
    return SESSION.read_string(_PIP_ENV, "PIP_NO_CACHE_DIR") == "1"


# ── Node.js npm Prefix (user-level installs) ─────────────────────────────

_NODE_ENV = r"HKEY_CURRENT_USER\Environment"


def apply_npm_prefer_offline(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("npm: set npm_config_prefer_offline for faster installs")
    SESSION.backup([_NODE_ENV], "NpmOffline")
    SESSION.set_string(_NODE_ENV, "npm_config_prefer_offline", "true")


def remove_npm_prefer_offline(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_NODE_ENV, "npm_config_prefer_offline")


def detect_npm_prefer_offline() -> bool:
    return SESSION.read_string(_NODE_ENV, "npm_config_prefer_offline") == "true"


# ── Pip Require Virtualenv (prevent accidental global installs) ──────────────


def apply_pip_require_venv(*, require_admin: bool = False) -> None:
    """Set PIP_REQUIRE_VIRTUALENV=1 to prevent accidental global installs."""
    assert_admin(require_admin)
    SESSION.log("Pip: require virtualenv for installs")
    SESSION.backup([_PIP_ENV], "PipRequireVenv")
    SESSION.set_string(_PIP_ENV, "PIP_REQUIRE_VIRTUALENV", "1")


def remove_pip_require_venv(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_PIP_ENV, "PIP_REQUIRE_VIRTUALENV")


def detect_pip_require_venv() -> bool:
    return SESSION.read_string(_PIP_ENV, "PIP_REQUIRE_VIRTUALENV") == "1"


# ── Pip Disable Version Check ───────────────────────────────────────────────


def apply_pip_disable_version_check(*, require_admin: bool = False) -> None:
    """Suppress the 'new pip version available' warning."""
    assert_admin(require_admin)
    SESSION.log("Pip: disable version check nag")
    SESSION.backup([_PIP_ENV], "PipVersionCheck")
    SESSION.set_string(_PIP_ENV, "PIP_DISABLE_PIP_VERSION_CHECK", "1")


def remove_pip_disable_version_check(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_PIP_ENV, "PIP_DISABLE_PIP_VERSION_CHECK")


def detect_pip_disable_version_check() -> bool:
    return SESSION.read_string(_PIP_ENV, "PIP_DISABLE_PIP_VERSION_CHECK") == "1"


# ── Pip Timeout (increase for slow networks) ────────────────────────────────


def apply_pip_timeout(*, require_admin: bool = False) -> None:
    """Set PIP_TIMEOUT=60 for corporate/slow networks."""
    assert_admin(require_admin)
    SESSION.log("Pip: set timeout to 60s")
    SESSION.backup([_PIP_ENV], "PipTimeout")
    SESSION.set_string(_PIP_ENV, "PIP_TIMEOUT", "60")


def remove_pip_timeout(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_PIP_ENV, "PIP_TIMEOUT")


def detect_pip_timeout() -> bool:
    return SESSION.read_string(_PIP_ENV, "PIP_TIMEOUT") == "60"


# ── Pip Trusted Host (corporate proxy/mirror) ───────────────────────────────


def apply_pip_trusted_host(*, require_admin: bool = False) -> None:
    """Add common trusted hosts for pip (useful behind corp proxies)."""
    assert_admin(require_admin)
    SESSION.log("Pip: set trusted hosts for corporate environments")
    SESSION.backup([_PIP_ENV], "PipTrustedHost")
    SESSION.set_string(_PIP_ENV, "PIP_TRUSTED_HOST", "pypi.org files.pythonhosted.org pypi.python.org")


def remove_pip_trusted_host(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_PIP_ENV, "PIP_TRUSTED_HOST")


def detect_pip_trusted_host() -> bool:
    val = SESSION.read_string(_PIP_ENV, "PIP_TRUSTED_HOST")
    return val is not None and "pypi.org" in val


# ── Pip Index URL (system-level, HKLM) ──────────────────────────────────────

_PIP_SYS_ENV = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment"


def apply_pip_system_index(*, require_admin: bool = True) -> None:
    """Set system-wide pip index URL to the official PyPI (HKLM)."""
    assert_admin(require_admin)
    SESSION.log("Pip (system): set index URL to official PyPI")
    SESSION.backup([_PIP_SYS_ENV], "PipSystemIndex")
    SESSION.set_string(_PIP_SYS_ENV, "PIP_INDEX_URL", "https://pypi.org/simple")


def remove_pip_system_index(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_PIP_SYS_ENV, "PIP_INDEX_URL")


def detect_pip_system_index() -> bool:
    val = SESSION.read_string(_PIP_SYS_ENV, "PIP_INDEX_URL")
    return val is not None and "pypi.org" in val


# ── Pip System No-Cache (system-level) ──────────────────────────────────────


def apply_pip_system_no_cache(*, require_admin: bool = True) -> None:
    """Set system-wide PIP_NO_CACHE_DIR=1 (HKLM)."""
    assert_admin(require_admin)
    SESSION.log("Pip (system): disable cache system-wide")
    SESSION.backup([_PIP_SYS_ENV], "PipSysNoCache")
    SESSION.set_string(_PIP_SYS_ENV, "PIP_NO_CACHE_DIR", "1")


def remove_pip_system_no_cache(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_PIP_SYS_ENV, "PIP_NO_CACHE_DIR")


def detect_pip_system_no_cache() -> bool:
    return SESSION.read_string(_PIP_SYS_ENV, "PIP_NO_CACHE_DIR") == "1"


# ── Pip System Trusted Host (system-level, HKLM) ────────────────────────────


def apply_pip_system_trusted_host(*, require_admin: bool = True) -> None:
    """Set system-wide PIP_TRUSTED_HOST for all users."""
    assert_admin(require_admin)
    SESSION.log("Pip (system): set trusted hosts for all users")
    SESSION.backup([_PIP_SYS_ENV], "PipSysTrustedHost")
    SESSION.set_string(_PIP_SYS_ENV, "PIP_TRUSTED_HOST", "pypi.org files.pythonhosted.org pypi.python.org")


def remove_pip_system_trusted_host(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_PIP_SYS_ENV, "PIP_TRUSTED_HOST")


def detect_pip_system_trusted_host() -> bool:
    val = SESSION.read_string(_PIP_SYS_ENV, "PIP_TRUSTED_HOST")
    return val is not None and "pypi.org" in val


# ── Pip System Require Virtualenv (system-level) ────────────────────────────


def apply_pip_system_require_venv(*, require_admin: bool = True) -> None:
    """System-wide PIP_REQUIRE_VIRTUALENV=1 — block global pip installs for all users."""
    assert_admin(require_admin)
    SESSION.log("Pip (system): require virtualenv for all users")
    SESSION.backup([_PIP_SYS_ENV], "PipSysRequireVenv")
    SESSION.set_string(_PIP_SYS_ENV, "PIP_REQUIRE_VIRTUALENV", "1")


def remove_pip_system_require_venv(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_PIP_SYS_ENV, "PIP_REQUIRE_VIRTUALENV")


def detect_pip_system_require_venv() -> bool:
    return SESSION.read_string(_PIP_SYS_ENV, "PIP_REQUIRE_VIRTUALENV") == "1"


# ── Disable WinGet Auto-Upgrade ─────────────────────────────────────────────


def apply_winget_disable_auto_update(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("WinGet: disable automatic package upgrades")
    SESSION.backup([_WINGET_KEY], "WingetAutoUpdate")
    SESSION.set_dword(_WINGET_KEY, "EnableAutoUpgrade", 0)


def remove_winget_disable_auto_update(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_WINGET_KEY, "EnableAutoUpgrade")


def detect_winget_disable_auto_update() -> bool:
    return SESSION.read_dword(_WINGET_KEY, "EnableAutoUpgrade") == 0


# ── Disable WinGet Microsoft Store Source ───────────────────────────────────


def apply_winget_disable_msstore(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("WinGet: disable Microsoft Store package source")
    SESSION.backup([_WINGET_KEY], "WingetMSStore")
    SESSION.set_dword(_WINGET_KEY, "EnableMSStoreSource", 0)


def remove_winget_disable_msstore(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_WINGET_KEY, "EnableMSStoreSource")


def detect_winget_disable_msstore() -> bool:
    return SESSION.read_dword(_WINGET_KEY, "EnableMSStoreSource") == 0


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
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
    TweakDef(
        id="pip-no-cache",
        label="Pip Disable Cache (Save Disk)",
        category="Package Management",
        apply_fn=apply_pip_no_cache,
        remove_fn=remove_pip_no_cache,
        detect_fn=detect_pip_no_cache,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_PIP_ENV],
        description="Sets PIP_NO_CACHE_DIR=1 to avoid storing downloaded package caches.",
        tags=["python", "pip", "disk"],
    ),
    TweakDef(
        id="npm-prefer-offline",
        label="npm Prefer Offline Cache",
        category="Package Management",
        apply_fn=apply_npm_prefer_offline,
        remove_fn=remove_npm_prefer_offline,
        detect_fn=detect_npm_prefer_offline,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_NODE_ENV],
        description="Sets npm to prefer local cache for faster installs.",
        tags=["npm", "node", "packages", "offline"],
    ),
    TweakDef(
        id="pip-require-venv",
        label="Pip Require Virtualenv",
        category="Package Management",
        apply_fn=apply_pip_require_venv,
        remove_fn=remove_pip_require_venv,
        detect_fn=detect_pip_require_venv,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_PIP_ENV],
        description=(
            "Sets PIP_REQUIRE_VIRTUALENV=1 — prevents accidental "
            "installs into system/global Python. Forces explicit "
            "virtualenv usage."
        ),
        tags=["python", "pip", "virtualenv", "safety"],
    ),
    TweakDef(
        id="pip-disable-version-check",
        label="Pip Disable Version Nag",
        category="Package Management",
        apply_fn=apply_pip_disable_version_check,
        remove_fn=remove_pip_disable_version_check,
        detect_fn=detect_pip_disable_version_check,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_PIP_ENV],
        description=(
            "Sets PIP_DISABLE_PIP_VERSION_CHECK=1 to suppress "
            "'new pip version available' warnings."
        ),
        tags=["python", "pip", "nag"],
    ),
    TweakDef(
        id="pip-timeout",
        label="Pip Timeout 60s (Slow Networks)",
        category="Package Management",
        apply_fn=apply_pip_timeout,
        remove_fn=remove_pip_timeout,
        detect_fn=detect_pip_timeout,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_PIP_ENV],
        description=(
            "Sets PIP_TIMEOUT=60 for more reliable installs on "
            "slow or corporate networks (default: 15s)."
        ),
        tags=["python", "pip", "network", "timeout"],
    ),
    TweakDef(
        id="pip-trusted-host",
        label="Pip Trusted Hosts (PyPI)",
        category="Package Management",
        apply_fn=apply_pip_trusted_host,
        remove_fn=remove_pip_trusted_host,
        detect_fn=detect_pip_trusted_host,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_PIP_ENV],
        description=(
            "Adds pypi.org and pythonhosted.org as trusted hosts "
            "so pip works behind corporate TLS-inspecting proxies."
        ),
        tags=["python", "pip", "proxy", "corporate"],
    ),
    TweakDef(
        id="pip-system-index",
        label="Pip System Index URL (HKLM)",
        category="Package Management",
        apply_fn=apply_pip_system_index,
        remove_fn=remove_pip_system_index,
        detect_fn=detect_pip_system_index,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_PIP_SYS_ENV],
        description=(
            "Sets system-wide PIP_INDEX_URL to official PyPI for "
            "all users (HKLM environment variable)."
        ),
        tags=["python", "pip", "system", "index"],
    ),
    TweakDef(
        id="pip-system-no-cache",
        label="Pip System Disable Cache (HKLM)",
        category="Package Management",
        apply_fn=apply_pip_system_no_cache,
        remove_fn=remove_pip_system_no_cache,
        detect_fn=detect_pip_system_no_cache,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_PIP_SYS_ENV],
        description=(
            "Sets system-wide PIP_NO_CACHE_DIR=1 to prevent "
            "pip cache accumulation for all users."
        ),
        tags=["python", "pip", "system", "disk"],
    ),
    TweakDef(
        id="pip-system-trusted-host",
        label="Pip System Trusted Hosts (HKLM)",
        category="Package Management",
        apply_fn=apply_pip_system_trusted_host,
        remove_fn=remove_pip_system_trusted_host,
        detect_fn=detect_pip_system_trusted_host,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_PIP_SYS_ENV],
        description=(
            "Sets system-wide PIP_TRUSTED_HOST for all users — "
            "useful for fleet-wide corporate proxy configurations."
        ),
        tags=["python", "pip", "system", "proxy", "corporate"],
    ),
    TweakDef(
        id="pip-system-require-venv",
        label="Pip System Require Virtualenv (HKLM)",
        category="Package Management",
        apply_fn=apply_pip_system_require_venv,
        remove_fn=remove_pip_system_require_venv,
        detect_fn=detect_pip_system_require_venv,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_PIP_SYS_ENV],
        description=(
            "System-wide PIP_REQUIRE_VIRTUALENV=1 — blocks any pip "
            "install outside a virtualenv for all users on the machine."
        ),
        tags=["python", "pip", "system", "virtualenv", "safety"],
    ),
    TweakDef(
        id="winget-disable-auto-update",
        label="Disable WinGet Auto-Upgrade",
        category="Package Management",
        apply_fn=apply_winget_disable_auto_update,
        remove_fn=remove_winget_disable_auto_update,
        detect_fn=detect_winget_disable_auto_update,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_WINGET_KEY],
        description=(
            "Disables WinGet automatic package upgrades via Group Policy. "
            "Packages must be upgraded manually with 'winget upgrade'. "
            "Default: Enabled. Recommended: Disabled for managed environments."
        ),
        tags=["winget", "packages", "update", "performance"],
    ),
    TweakDef(
        id="winget-disable-msstore-source",
        label="Disable WinGet MS Store Source",
        category="Package Management",
        apply_fn=apply_winget_disable_msstore,
        remove_fn=remove_winget_disable_msstore,
        detect_fn=detect_winget_disable_msstore,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_WINGET_KEY],
        description=(
            "Disables the Microsoft Store as a WinGet package source. "
            "Only the winget community repository is used. "
            "Default: Enabled. Recommended: Disabled for developer workflows."
        ),
        tags=["winget", "packages", "source", "msstore"],
    ),
]


# -- Disable Store Auto-Download -----------------------------------------------

_STORE_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore"


def _apply_pkg_disable_store_auto_download(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Pkg: disabling Store auto-download")
    SESSION.backup([_STORE_POLICY], "StoreAutoDownload")
    SESSION.set_dword(_STORE_POLICY, "AutoDownload", 2)
    SESSION.log("Pkg: Store auto-download disabled")


def _remove_pkg_disable_store_auto_download(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_STORE_POLICY], "StoreAutoDownload_Remove")
    SESSION.delete_value(_STORE_POLICY, "AutoDownload")


def _detect_pkg_disable_store_auto_download() -> bool:
    return SESSION.read_dword(_STORE_POLICY, "AutoDownload") == 2


# -- Disable Suggested Apps ----------------------------------------------------

_CDM = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows"
    r"\CurrentVersion\ContentDeliveryManager"
)


def _apply_pkg_disable_suggested_apps(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Pkg: disabling suggested apps")
    SESSION.backup([_CDM], "SuggestedApps")
    SESSION.set_dword(_CDM, "SilentInstalledAppsEnabled", 0)
    SESSION.log("Pkg: suggested apps disabled")


def _remove_pkg_disable_suggested_apps(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_CDM], "SuggestedApps_Remove")
    SESSION.set_dword(_CDM, "SilentInstalledAppsEnabled", 1)


def _detect_pkg_disable_suggested_apps() -> bool:
    return SESSION.read_dword(_CDM, "SilentInstalledAppsEnabled") == 0


TWEAKS += [
    TweakDef(
        id="pkg-disable-store-auto-download",
        label="Disable Store Auto-Download",
        category="Package Management",
        apply_fn=_apply_pkg_disable_store_auto_download,
        remove_fn=_remove_pkg_disable_store_auto_download,
        detect_fn=_detect_pkg_disable_store_auto_download,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_STORE_POLICY],
        description=(
            "Prevents Microsoft Store from auto-downloading apps. "
            "Default: Enabled. Recommended: Disabled."
        ),
        tags=["packages", "store", "auto-download"],
    ),
    TweakDef(
        id="pkg-disable-suggested-apps",
        label="Disable Suggested App Installations",
        category="Package Management",
        apply_fn=_apply_pkg_disable_suggested_apps,
        remove_fn=_remove_pkg_disable_suggested_apps,
        detect_fn=_detect_pkg_disable_suggested_apps,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CDM],
        description=(
            "Prevents Windows from silently installing suggested apps. "
            "Default: Enabled. Recommended: Disabled."
        ),
        tags=["packages", "suggested", "bloatware"],
    ),
]


# ── Disable WinGet Auto-Update ───────────────────────────────────────────────


def _apply_pkg_disable_winget_auto_update(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Pkg: disabling WinGet auto-update")
    SESSION.backup([_WINGET_KEY], "WinGetAutoUpdate")
    SESSION.set_dword(_WINGET_KEY, "EnableAutoUpdate", 0)


def _remove_pkg_disable_winget_auto_update(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_WINGET_KEY, "EnableAutoUpdate")


def _detect_pkg_disable_winget_auto_update() -> bool:
    return SESSION.read_dword(_WINGET_KEY, "EnableAutoUpdate") == 0


# ── Set Chocolatey System Proxy ───────────────────────────────────────────────

_CHOCO_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Chocolatey"


def _apply_pkg_choco_proxy(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Pkg: enabling Chocolatey system proxy usage")
    SESSION.backup([_CHOCO_POLICY], "ChocoProxy")
    SESSION.set_dword(_CHOCO_POLICY, "UseSystemProxy", 1)


def _remove_pkg_choco_proxy(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CHOCO_POLICY, "UseSystemProxy")


def _detect_pkg_choco_proxy() -> bool:
    return SESSION.read_dword(_CHOCO_POLICY, "UseSystemProxy") == 1


# ── Enable Package Source Validation ─────────────────────────────────────────


def _apply_pkg_source_validation(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Pkg: enabling package source hash validation")
    SESSION.backup([_WINGET_KEY], "PkgSourceValidation")
    SESSION.set_dword(_WINGET_KEY, "EnableHashOverride", 0)


def _remove_pkg_source_validation(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_WINGET_KEY, "EnableHashOverride", 1)


def _detect_pkg_source_validation() -> bool:
    return SESSION.read_dword(_WINGET_KEY, "EnableHashOverride") == 0


TWEAKS += [
    TweakDef(
        id="pkg-disable-winget-auto-update",
        label="Disable WinGet Auto-Update",
        category="Package Management",
        apply_fn=_apply_pkg_disable_winget_auto_update,
        remove_fn=_remove_pkg_disable_winget_auto_update,
        detect_fn=_detect_pkg_disable_winget_auto_update,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_WINGET_KEY],
        description=(
            "Disables automatic WinGet package manager self-updates via policy. "
            "Default: Enabled. Recommended: Disabled for controlled environments."
        ),
        tags=["packages", "winget", "auto-update", "policy"],
    ),
    TweakDef(
        id="pkg-choco-proxy",
        label="Set Chocolatey System Proxy",
        category="Package Management",
        apply_fn=_apply_pkg_choco_proxy,
        remove_fn=_remove_pkg_choco_proxy,
        detect_fn=_detect_pkg_choco_proxy,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_CHOCO_POLICY],
        description=(
            "Configures Chocolatey to use the system proxy for package downloads. "
            "Useful in corporate environments behind a proxy. "
            "Default: Direct. Recommended: Enabled behind proxy."
        ),
        tags=["packages", "chocolatey", "proxy", "corporate"],
    ),
    TweakDef(
        id="pkg-source-validation",
        label="Enable Package Source Validation",
        category="Package Management",
        apply_fn=_apply_pkg_source_validation,
        remove_fn=_remove_pkg_source_validation,
        detect_fn=_detect_pkg_source_validation,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_WINGET_KEY],
        description=(
            "Prevents WinGet from overriding package hash validation. "
            "Ensures integrity checks are enforced for all package sources. "
            "Default: Override allowed. Recommended: Disabled (validation enforced)."
        ),
        tags=["packages", "winget", "hash", "validation", "security"],
    ),
]
