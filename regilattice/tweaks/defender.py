"""Windows Defender / Security registry tweaks.

Covers: real-time protection hints, SmartScreen, PUA protection,
exploit guard, cloud-delivered protection, and tamper protection.
"""

from __future__ import annotations

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
_NOTIFICATIONS = rf"{_DEFENDER}\Reporting"
_EXCLUSIONS = rf"{_DEFENDER}\Exclusions\Paths"
_NIS = rf"{_DEFENDER}\NIS"


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


# ── Disable Defender Notifications ─────────────────────────────────────────


def _apply_disable_defender_notify(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Defender: suppress non-critical notifications")
    SESSION.backup([_NOTIFICATIONS], "DefenderNotify")
    SESSION.set_dword(_NOTIFICATIONS, "DisableEnhancedNotifications", 1)
    SESSION.set_dword(_DEFENDER, "DisableRealtimeMonitoring_Toast", 1)


def _remove_disable_defender_notify(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_NOTIFICATIONS, "DisableEnhancedNotifications")
    SESSION.delete_value(_DEFENDER, "DisableRealtimeMonitoring_Toast")


def _detect_disable_defender_notify() -> bool:
    return SESSION.read_dword(_NOTIFICATIONS, "DisableEnhancedNotifications") == 1


# ── Add Developer Folder Exclusions ────────────────────────────────────────

_DEV_PATHS = (
    r"C:\Users\*\source\repos",
    r"C:\Users\*\.cargo",
    r"C:\Users\*\.rustup",
    r"C:\Users\*\go",
    r"C:\Users\*\node_modules",
)


def _apply_dev_exclusions(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Defender: add developer folder scan exclusions")
    SESSION.backup([_EXCLUSIONS], "DefenderDevExcl")
    for p in _DEV_PATHS:
        SESSION.set_dword(_EXCLUSIONS, p, 0)


def _remove_dev_exclusions(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    for p in _DEV_PATHS:
        SESSION.delete_value(_EXCLUSIONS, p)


def _detect_dev_exclusions() -> bool:
    return SESSION.read_dword(_EXCLUSIONS, _DEV_PATHS[0]) == 0


# ── Enable Controlled Folder Access ───────────────────────────────────────

_CFA_KEY = rf"{_DEFENDER}\Windows Defender Exploit Guard\Controlled Folder Access"


def _apply_controlled_folder_access(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Defender: enable Controlled Folder Access (ransomware shield)")
    SESSION.backup([_CFA_KEY], "ControlledFolderAccess")
    SESSION.set_dword(_CFA_KEY, "EnableControlledFolderAccess", 1)


def _remove_controlled_folder_access(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_CFA_KEY, "EnableControlledFolderAccess", 0)


def _detect_controlled_folder_access() -> bool:
    return SESSION.read_dword(_CFA_KEY, "EnableControlledFolderAccess") == 1


# ── Enable Network Protection ───────────────────────────────────────────────

_NET_PROTECT = rf"{_DEFENDER}\Windows Defender Exploit Guard\Network Protection"


def _apply_network_protection(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Defender: enable Network Protection")
    SESSION.backup([_NET_PROTECT], "NetworkProtection")
    SESSION.set_dword(_NET_PROTECT, "EnableNetworkProtection", 1)


def _remove_network_protection(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_NET_PROTECT, "EnableNetworkProtection", 0)


def _detect_network_protection() -> bool:
    return SESSION.read_dword(_NET_PROTECT, "EnableNetworkProtection") == 1


# ── Enable Attack Surface Reduction Rules ─────────────────────────────────────

_ASR = rf"{_DEFENDER}\Windows Defender Exploit Guard\ASR"


def _apply_asr_rules(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Defender: enable Attack Surface Reduction rules")
    SESSION.backup([_ASR], "ASR")
    SESSION.set_dword(_ASR, "ExploitGuard_ASR_Rules", 1)


def _remove_asr_rules(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_ASR, "ExploitGuard_ASR_Rules", 0)


def _detect_asr_rules() -> bool:
    return SESSION.read_dword(_ASR, "ExploitGuard_ASR_Rules") == 1


# ── Disable Defender for Performance ──────────────────────────────────────────


def _apply_disable_realtime(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Defender: disable real-time protection (performance mode)")
    SESSION.backup([_RT], "RealtimeProtection")
    SESSION.set_dword(_RT, "DisableRealtimeMonitoring", 1)
    SESSION.set_dword(_RT, "DisableBehaviorMonitoring", 1)
    SESSION.set_dword(_RT, "DisableOnAccessProtection", 1)
    SESSION.set_dword(_RT, "DisableScanOnRealtimeEnable", 1)


def _remove_disable_realtime(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_RT, "DisableRealtimeMonitoring")
    SESSION.delete_value(_RT, "DisableBehaviorMonitoring")
    SESSION.delete_value(_RT, "DisableOnAccessProtection")
    SESSION.delete_value(_RT, "DisableScanOnRealtimeEnable")


def _detect_disable_realtime() -> bool:
    return SESSION.read_dword(_RT, "DisableRealtimeMonitoring") == 1


# ── Disable Defender SmartScreen for Edge ─────────────────────────────────────


def _apply_disable_edge_smartscreen(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Defender: disable SmartScreen for Edge")
    SESSION.backup([_SMARTSCREEN_EDGE], "EdgeSmartScreen")
    SESSION.set_dword(_SMARTSCREEN_EDGE, "EnabledV9", 0)


def _remove_disable_edge_smartscreen(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_SMARTSCREEN_EDGE, "EnabledV9", 1)


def _detect_disable_edge_smartscreen() -> bool:
    return SESSION.read_dword(_SMARTSCREEN_EDGE, "EnabledV9") == 0


# ── Reduce Defender CPU Usage ────────────────────────────────────────────────


def _apply_defender_cpu_limit(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Defender: reduce scan CPU usage to 25%")
    SESSION.backup([_SCAN], "DefenderCPUReduce")
    SESSION.set_dword(_SCAN, "AvgCPULoadFactor", 25)


def _remove_defender_cpu_limit(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_SCAN, "AvgCPULoadFactor", 50)


def _detect_defender_cpu_limit() -> bool:
    return SESSION.read_dword(_SCAN, "AvgCPULoadFactor") == 25


# ── Disable Defender Network Inspection ──────────────────────────────────────


def _apply_disable_nis(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Defender: disable Network Inspection System (NIS)")
    SESSION.backup([_NIS], "DefenderNIS")
    SESSION.set_dword(_NIS, "DisableProtocolRecognition", 1)


def _remove_disable_nis(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_NIS, "DisableProtocolRecognition")


def _detect_disable_nis() -> bool:
    return SESSION.read_dword(_NIS, "DisableProtocolRecognition") == 1


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
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
    TweakDef(
        id="disable-defender-notifications",
        label="Disable Defender Notifications",
        category="Security",
        apply_fn=_apply_disable_defender_notify,
        remove_fn=_remove_disable_defender_notify,
        detect_fn=_detect_disable_defender_notify,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_NOTIFICATIONS],
        description="Suppresses non-critical Defender notification toasts.",
        tags=["defender", "notifications", "ux"],
    ),
    TweakDef(
        id="defender-dev-exclusions",
        label="Add Developer Folder Exclusions",
        category="Security",
        apply_fn=_apply_dev_exclusions,
        remove_fn=_remove_dev_exclusions,
        detect_fn=_detect_dev_exclusions,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_EXCLUSIONS],
        description=(
            "Excludes common dev folders (source/repos, .cargo, .rustup, "
            "go, node_modules) from real-time Defender scans."
        ),
        tags=["defender", "developer", "performance"],
    ),
    TweakDef(
        id="enable-controlled-folder-access",
        label="Enable Controlled Folder Access",
        category="Security",
        apply_fn=_apply_controlled_folder_access,
        remove_fn=_remove_controlled_folder_access,
        detect_fn=_detect_controlled_folder_access,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_CFA_KEY],
        description=(
            "Enables Controlled Folder Access (ransomware protection) "
            "which blocks unauthorized changes to protected folders."
        ),
        tags=["defender", "ransomware", "security"],
    ),
    TweakDef(
        id="enable-network-protection",
        label="Enable Network Protection",
        category="Security",
        apply_fn=_apply_network_protection,
        remove_fn=_remove_network_protection,
        detect_fn=_detect_network_protection,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_NET_PROTECT],
        description=(
            "Enables Defender Network Protection to block connections "
            "to malicious domains and IP addresses."
        ),
        tags=["defender", "network", "security"],
    ),
    TweakDef(
        id="enable-asr-rules",
        label="Enable Attack Surface Reduction",
        category="Security",
        apply_fn=_apply_asr_rules,
        remove_fn=_remove_asr_rules,
        detect_fn=_detect_asr_rules,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_ASR],
        description=(
            "Enables Defender ASR rules which block common attack vectors "
            "like Office macro exploits and script-based threats."
        ),
        tags=["defender", "asr", "security", "enterprise"],
    ),
    TweakDef(
        id="disable-defender-realtime",
        label="Disable Real-Time Protection",
        category="Security",
        apply_fn=_apply_disable_realtime,
        remove_fn=_remove_disable_realtime,
        detect_fn=_detect_disable_realtime,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_RT],
        description=(
            "Disables Defender real-time, behavior, and on-access monitoring "
            "for maximum performance. USE WITH CAUTION."
        ),
        tags=["defender", "performance", "realtime"],
    ),
    TweakDef(
        id="disable-edge-smartscreen",
        label="Disable SmartScreen for Edge",
        category="Security",
        apply_fn=_apply_disable_edge_smartscreen,
        remove_fn=_remove_disable_edge_smartscreen,
        detect_fn=_detect_disable_edge_smartscreen,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_SMARTSCREEN_EDGE],
        description="Disables SmartScreen phishing filter specifically for Microsoft Edge.",
        tags=["defender", "smartscreen", "edge"],
    ),
    TweakDef(
        id="sec-defender-cpu-limit",
        label="Reduce Defender CPU Usage",
        category="Security",
        apply_fn=_apply_defender_cpu_limit,
        remove_fn=_remove_defender_cpu_limit,
        detect_fn=_detect_defender_cpu_limit,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_SCAN],
        description=(
            "Limits Windows Defender scan CPU usage to 25%. Prevents Defender from slowing "
            "down the system during scans. Options: 5-100. Default: 50. Recommended: 25."
        ),
        tags=["security", "defender", "cpu", "performance"],
    ),
    TweakDef(
        id="sec-defender-disable-nis",
        label="Disable Defender Network Inspection",
        category="Security",
        apply_fn=_apply_disable_nis,
        remove_fn=_remove_disable_nis,
        detect_fn=_detect_disable_nis,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_NIS],
        description=(
            "Disables Defender Network Inspection System (NIS) protocol analysis. Reduces "
            "CPU overhead from deep packet inspection. Default: Enabled. Recommended: "
            "Disabled if using third-party firewall."
        ),
        tags=["security", "defender", "network", "performance"],
    ),
]
