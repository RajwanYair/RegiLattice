"""Windows Defender / Security registry tweaks.

Covers: real-time protection hints, SmartScreen, PUA protection,
exploit guard, cloud-delivered protection, and tamper protection.
"""

from __future__ import annotations

from typing import List

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Key paths ────────────────────────────────────────────────────────────────

_DEFENDER = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender"
_RT = rf"{_DEFENDER}\Real-Time Protection"
_SPYNET = rf"{_DEFENDER}\Spynet"
_THREATS = rf"{_DEFENDER}\Threats"
_PUA = rf"{_DEFENDER}\MpEngine"
_SMARTSCREEN = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows"
    r"\System"
)
_SMARTSCREEN_EDGE = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MicrosoftEdge"
    r"\PhishingFilter"
)
_EXPLOIT = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control"
    r"\Session Manager\kernel"
)


# ── Disable Defender Cloud Samples ───────────────────────────────────────────


def _apply_disable_cloud_samples(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Defender: disable automatic sample submission")
    SESSION.backup([_SPYNET], "DefenderCloud")
    SESSION.set_dword(_SPYNET, "SubmitSamplesConsent", 2)  # 2 = Never send
    SESSION.set_dword(_SPYNET, "SpynetReporting", 0)  # 0 = Basic


def _remove_disable_cloud_samples(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SPYNET, "SubmitSamplesConsent")
    SESSION.delete_value(_SPYNET, "SpynetReporting")


def _detect_disable_cloud_samples() -> bool:
    return SESSION.read_dword(_SPYNET, "SubmitSamplesConsent") == 2


# ── Enable PUA (Potentially Unwanted App) Protection ─────────────────────────


def _apply_pua_protection(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Defender: enable PUA / adware protection")
    SESSION.backup([_PUA], "DefenderPUA")
    SESSION.set_dword(_PUA, "MpEnablePus", 1)


def _remove_pua_protection(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_PUA, "MpEnablePus")


def _detect_pua_protection() -> bool:
    return SESSION.read_dword(_PUA, "MpEnablePus") == 1


# ── Harden SmartScreen ───────────────────────────────────────────────────────


def _apply_harden_smartscreen(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Security: harden SmartScreen to warn + block")
    SESSION.backup([_SMARTSCREEN], "SmartScreen")
    SESSION.set_dword(_SMARTSCREEN, "EnableSmartScreen", 2)  # 2 = Warn
    SESSION.set_string(
        _SMARTSCREEN, "ShellSmartScreenLevel", "Block"
    )


def _remove_harden_smartscreen(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_SMARTSCREEN, "EnableSmartScreen", 1)  # default
    SESSION.delete_value(_SMARTSCREEN, "ShellSmartScreenLevel")


def _detect_harden_smartscreen() -> bool:
    return SESSION.read_dword(_SMARTSCREEN, "EnableSmartScreen") == 2


# ── Disable Exploit Protection Telemetry ─────────────────────────────────────


def _apply_disable_exploit_telemetry(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Security: disable exploit-protection telemetry")
    SESSION.backup([_EXPLOIT], "ExploitTelemetry")
    SESSION.set_dword(_EXPLOIT, "MitigationAuditOptions", 0)


def _remove_disable_exploit_telemetry(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_EXPLOIT, "MitigationAuditOptions")


def _detect_disable_exploit_telemetry() -> bool:
    return SESSION.read_dword(_EXPLOIT, "MitigationAuditOptions") == 0


# ── Increase Defender Scan CPU Limit ─────────────────────────────────────────

_SCAN = rf"{_DEFENDER}\Scan"


def _apply_scan_cpu_limit(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Defender: limit scan CPU usage to 25%")
    SESSION.backup([_SCAN], "DefenderScanCPU")
    SESSION.set_dword(_SCAN, "AvgCPULoadFactor", 25)


def _remove_scan_cpu_limit(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_SCAN, "AvgCPULoadFactor", 50)  # default


def _detect_scan_cpu_limit() -> bool:
    return SESSION.read_dword(_SCAN, "AvgCPULoadFactor") == 25


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: List[TweakDef] = [
    TweakDef(
        id="disable-defender-cloud-samples",
        label="Disable Defender Sample Submission",
        category="Security",
        apply_fn=_apply_disable_cloud_samples,
        remove_fn=_remove_disable_cloud_samples,
        detect_fn=_detect_disable_cloud_samples,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_SPYNET],
        description=(
            "Prevents Windows Defender from automatically uploading "
            "file samples to Microsoft for analysis."
        ),
        tags=["defender", "privacy", "security"],
    ),
    TweakDef(
        id="enable-pua-protection",
        label="Enable PUA / Adware Protection",
        category="Security",
        apply_fn=_apply_pua_protection,
        remove_fn=_remove_pua_protection,
        detect_fn=_detect_pua_protection,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_PUA],
        description=(
            "Enables Potentially Unwanted Application (PUA) detection "
            "in Windows Defender."
        ),
        tags=["defender", "security", "adware"],
    ),
    TweakDef(
        id="harden-smartscreen",
        label="Harden SmartScreen (Warn + Block)",
        category="Security",
        apply_fn=_apply_harden_smartscreen,
        remove_fn=_remove_harden_smartscreen,
        detect_fn=_detect_harden_smartscreen,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_SMARTSCREEN],
        description=(
            "Sets SmartScreen to warn and block unrecognized apps "
            "and downloads."
        ),
        tags=["smartscreen", "security"],
    ),
    TweakDef(
        id="disable-exploit-telemetry",
        label="Disable Exploit Protection Telemetry",
        category="Security",
        apply_fn=_apply_disable_exploit_telemetry,
        remove_fn=_remove_disable_exploit_telemetry,
        detect_fn=_detect_disable_exploit_telemetry,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_EXPLOIT],
        description="Disables audit telemetry from Windows exploit mitigations.",
        tags=["security", "telemetry", "privacy"],
    ),
    TweakDef(
        id="defender-scan-cpu-limit",
        label="Limit Defender Scan CPU to 25%",
        category="Security",
        apply_fn=_apply_scan_cpu_limit,
        remove_fn=_remove_scan_cpu_limit,
        detect_fn=_detect_scan_cpu_limit,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_SCAN],
        description=(
            "Limits Windows Defender scheduled-scan CPU usage to 25% "
            "to reduce impact during scans."
        ),
        tags=["defender", "performance", "cpu"],
    ),
]
