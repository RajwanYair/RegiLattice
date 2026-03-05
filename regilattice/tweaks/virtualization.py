"""Virtualization tweaks — Hyper-V, WSL integration, VM performance.

Covers: Hyper-V performance settings, memory management, nested virtualization,
container isolation, and hypervisor scheduling optimizations.
"""

from __future__ import annotations

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Key paths ────────────────────────────────────────────────────────────────

_HYPERV = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization"
_HYPERV_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HyperV"
_MEMORY = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"
_VIRT_SECURITY = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows"
    r"\DeviceGuard"
)
_SANDBOX = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Sandbox"
_VBS = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard"
_APPGUARD = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\AppHVSI"


# ── Disable Hyper-V Enhanced Session Mode (Default) ──────────────────────────


def _apply_disable_enhanced_session(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Virtualization: disable Hyper-V Enhanced Session Mode by default")
    SESSION.backup([_HYPERV], "HyperVEnhancedSession")
    SESSION.set_dword(_HYPERV, "AllowEnhancedSessionMode", 0)


def _remove_disable_enhanced_session(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_HYPERV, "AllowEnhancedSessionMode", 1)


def _detect_disable_enhanced_session() -> bool:
    return SESSION.read_dword(_HYPERV, "AllowEnhancedSessionMode") == 0


# ── Optimise Hyper-V Dynamic Memory ─────────────────────────────────────────


def _apply_optimise_dynamic_memory(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Virtualization: tune dynamic memory for better VM density")
    SESSION.backup([_MEMORY], "DynamicMemory")
    SESSION.set_dword(_MEMORY, "LargeSystemCache", 0)
    SESSION.set_dword(_MEMORY, "SecondLevelDataCache", 0)


def _remove_optimise_dynamic_memory(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_MEMORY, "LargeSystemCache")
    SESSION.delete_value(_MEMORY, "SecondLevelDataCache")


def _detect_optimise_dynamic_memory() -> bool:
    return SESSION.read_dword(_MEMORY, "LargeSystemCache") == 0


# ── Disable Virtualization Based Security (VBS) ─────────────────────────────


def _apply_disable_vbs(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Virtualization: disable Virtualization Based Security (VBS)")
    SESSION.backup([_VBS], "VBS")
    SESSION.set_dword(_VBS, "EnableVirtualizationBasedSecurity", 0)


def _remove_disable_vbs(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_VBS, "EnableVirtualizationBasedSecurity", 1)


def _detect_disable_vbs() -> bool:
    return SESSION.read_dword(_VBS, "EnableVirtualizationBasedSecurity") == 0


# ── Disable Credential Guard ────────────────────────────────────────────────


def _apply_disable_credential_guard(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Virtualization: disable Credential Guard")
    SESSION.backup([_VIRT_SECURITY], "CredentialGuard")
    SESSION.set_dword(_VIRT_SECURITY, "LsaCfgFlags", 0)
    SESSION.set_dword(_VIRT_SECURITY, "EnableVirtualizationBasedSecurity", 0)


def _remove_disable_credential_guard(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_VIRT_SECURITY, "LsaCfgFlags", 1)
    SESSION.set_dword(_VIRT_SECURITY, "EnableVirtualizationBasedSecurity", 1)


def _detect_disable_credential_guard() -> bool:
    return SESSION.read_dword(_VIRT_SECURITY, "LsaCfgFlags") == 0


# ── Disable Windows Sandbox ─────────────────────────────────────────────────


def _apply_disable_sandbox(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Virtualization: disable Windows Sandbox")
    SESSION.backup([_SANDBOX], "Sandbox")
    SESSION.set_dword(_SANDBOX, "AllowSandbox", 0)


def _remove_disable_sandbox(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_SANDBOX, "AllowSandbox", 1)


def _detect_disable_sandbox() -> bool:
    return SESSION.read_dword(_SANDBOX, "AllowSandbox") == 0


# ── Optimise Hypervisor Scheduler ────────────────────────────────────────────


def _apply_hypervisor_scheduler(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Virtualization: set hypervisor scheduler to Core mode")
    _key = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization"
    SESSION.backup([_key], "HypervisorScheduler")
    SESSION.set_dword(_key, "SchedulerType", 2)  # Core scheduler


def _remove_hypervisor_scheduler(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    _key = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization"
    SESSION.set_dword(_key, "SchedulerType", 1)  # Classic scheduler


def _detect_hypervisor_scheduler() -> bool:
    _key = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization"
    return SESSION.read_dword(_key, "SchedulerType") == 2


# ── Disable HVCI (Memory Integrity) ─────────────────────────────────────────


def _apply_disable_hvci(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Virtualization: disable HVCI / Memory Integrity")
    _key = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard\Scenarios\HypervisorEnforcedCodeIntegrity"
    SESSION.backup([_key], "HVCI")
    SESSION.set_dword(_key, "Enabled", 0)


def _remove_disable_hvci(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    _key = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard\Scenarios\HypervisorEnforcedCodeIntegrity"
    SESSION.set_dword(_key, "Enabled", 1)


def _detect_disable_hvci() -> bool:
    _key = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard\Scenarios\HypervisorEnforcedCodeIntegrity"
    return SESSION.read_dword(_key, "Enabled") == 0


# ── Disable Hyper-V Auto Stop Action ─────────────────────────────────────────


def _apply_disable_autostop(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Virtualization: disable automatic VM stop action on host shutdown")
    SESSION.backup([_HYPERV_POLICY], "HyperVAutoStop")
    SESSION.set_dword(_HYPERV_POLICY, "DisableAutomaticStopAction", 1)


def _remove_disable_autostop(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_HYPERV_POLICY, "DisableAutomaticStopAction")


def _detect_disable_autostop() -> bool:
    return SESSION.read_dword(_HYPERV_POLICY, "DisableAutomaticStopAction") == 1


# ── Disable Application Guard ────────────────────────────────────────────────


def _apply_disable_appguard(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Virtualization: disable Microsoft Defender Application Guard")
    SESSION.backup([_APPGUARD], "AppGuard")
    SESSION.set_dword(_APPGUARD, "AllowAppHVSI_ProviderSet", 0)


def _remove_disable_appguard(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_APPGUARD, "AllowAppHVSI_ProviderSet")


def _detect_disable_appguard() -> bool:
    return SESSION.read_dword(_APPGUARD, "AllowAppHVSI_ProviderSet") == 0


# ── Optimize VM Worker Process Priority ──────────────────────────────────────


def _apply_optimize_worker_priority(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Virtualization: set VM worker processes to high priority")
    SESSION.backup([_HYPERV], "VMWorkerPriority")
    SESSION.set_dword(_HYPERV, "HighVmWorkerProcessPriority", 1)


def _remove_optimize_worker_priority(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_HYPERV, "HighVmWorkerProcessPriority")


def _detect_optimize_worker_priority() -> bool:
    return SESSION.read_dword(_HYPERV, "HighVmWorkerProcessPriority") == 1


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="virt-disable-enhanced-session",
        label="Disable Hyper-V Enhanced Session Default",
        category="Virtualization",
        apply_fn=_apply_disable_enhanced_session,
        remove_fn=_remove_disable_enhanced_session,
        detect_fn=_detect_disable_enhanced_session,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_HYPERV],
        description=(
            "Disables Hyper-V Enhanced Session Mode by default. "
            "Useful if clipboard/file sharing between host and VM causes issues."
        ),
        tags=["hyperv", "virtualization", "enhanced-session"],
    ),
    TweakDef(
        id="virt-optimise-dynamic-memory",
        label="Optimise Dynamic Memory for VMs",
        category="Virtualization",
        apply_fn=_apply_optimise_dynamic_memory,
        remove_fn=_remove_optimise_dynamic_memory,
        detect_fn=_detect_optimise_dynamic_memory,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_MEMORY],
        description=(
            "Tunes Windows memory manager for better VM density "
            "by disabling large system cache and second-level data cache."
        ),
        tags=["hyperv", "virtualization", "memory", "performance"],
    ),
    TweakDef(
        id="virt-disable-vbs",
        label="Disable Virtualization Based Security (VBS)",
        category="Virtualization",
        apply_fn=_apply_disable_vbs,
        remove_fn=_remove_disable_vbs,
        detect_fn=_detect_disable_vbs,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_VBS],
        description=(
            "Disables VBS which can reduce performance in games and creative apps. "
            "WARNING: Reduces security — only use on personal gaming/performance rigs."
        ),
        tags=["vbs", "virtualization", "performance", "gaming"],
    ),
    TweakDef(
        id="virt-disable-credential-guard",
        label="Disable Credential Guard",
        category="Virtualization",
        apply_fn=_apply_disable_credential_guard,
        remove_fn=_remove_disable_credential_guard,
        detect_fn=_detect_disable_credential_guard,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_VIRT_SECURITY],
        description=(
            "Disables Credential Guard (VBS-backed LSASS protection). "
            "May be needed for compatibility with third-party VPN/auth tools."
        ),
        tags=["virtualization", "security", "credential-guard"],
    ),
    TweakDef(
        id="virt-disable-sandbox",
        label="Disable Windows Sandbox",
        category="Virtualization",
        apply_fn=_apply_disable_sandbox,
        remove_fn=_remove_disable_sandbox,
        detect_fn=_detect_disable_sandbox,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_SANDBOX],
        description="Disables the Windows Sandbox feature via policy.",
        tags=["virtualization", "sandbox", "policy"],
    ),
    TweakDef(
        id="virt-hypervisor-core-scheduler",
        label="Hyper-V Core Scheduler Mode",
        category="Virtualization",
        apply_fn=_apply_hypervisor_scheduler,
        remove_fn=_remove_hypervisor_scheduler,
        detect_fn=_detect_hypervisor_scheduler,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_HYPERV],
        description=(
            "Sets the hypervisor to Core scheduler mode for better security "
            "against side-channel attacks (Spectre/MDS). Recommended for Hyper-V hosts."
        ),
        tags=["hyperv", "virtualization", "scheduler", "security"],
    ),
    TweakDef(
        id="virt-disable-hvci",
        label="Disable HVCI (Memory Integrity)",
        category="Virtualization",
        apply_fn=_apply_disable_hvci,
        remove_fn=_remove_disable_hvci,
        detect_fn=_detect_disable_hvci,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_VBS],
        description=(
            "Disables Hypervisor-enforced Code Integrity (HVCI / Memory Integrity). "
            "Can improve gaming performance by 5-10% but reduces security."
        ),
        tags=["hvci", "virtualization", "performance", "gaming"],
    ),
    TweakDef(
        id="virt-disable-autostop",
        label="Disable Hyper-V Auto Stop Action",
        category="Virtualization",
        apply_fn=_apply_disable_autostop,
        remove_fn=_remove_disable_autostop,
        detect_fn=_detect_disable_autostop,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_HYPERV_POLICY],
        description=(
            "Disables automatic VM stop action on host shutdown. VMs will be saved to disk instead of stopped. "
            "Default: Disabled. Recommended: Enabled for server VMs."
        ),
        tags=["hyperv", "virtualization", "auto-stop"],
    ),
    TweakDef(
        id="virt-disable-appguard",
        label="Disable Application Guard",
        category="Virtualization",
        apply_fn=_apply_disable_appguard,
        remove_fn=_remove_disable_appguard,
        detect_fn=_detect_disable_appguard,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_APPGUARD],
        description=(
            "Disables Microsoft Defender Application Guard (MDAG). "
            "Frees memory and CPU used by isolation containers for Edge browser. "
            "Default: Enabled. Recommended: Disabled if not needed."
        ),
        tags=["virtualization", "appguard", "security", "performance"],
    ),
    TweakDef(
        id="virt-optimize-worker-priority",
        label="Optimize VM Worker Process Priority",
        category="Virtualization",
        apply_fn=_apply_optimize_worker_priority,
        remove_fn=_remove_optimize_worker_priority,
        detect_fn=_detect_optimize_worker_priority,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_HYPERV],
        description=(
            "Sets Hyper-V VM worker processes to high priority within the Windows scheduler. "
            "Reduces VM latency. Default: Normal priority. Recommended: High priority for dedicated VM hosts."
        ),
        tags=["hyperv", "virtualization", "performance", "priority"],
    ),
]
