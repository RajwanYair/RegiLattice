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
    SESSION.set_string(_SMARTSCREEN, "ShellSmartScreenLevel", "Block")


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
        id="sec-disable-defender-cloud-samples",
        label="Disable Defender Sample Submission",
        category="Security",
        apply_fn=_apply_disable_cloud_samples,
        remove_fn=_remove_disable_cloud_samples,
        detect_fn=_detect_disable_cloud_samples,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_SPYNET],
        description=("Prevents Windows Defender from automatically uploading file samples to Microsoft for analysis."),
        tags=["defender", "privacy", "security"],
    ),
    TweakDef(
        id="sec-enable-pua-protection",
        label="Enable PUA / Adware Protection",
        category="Security",
        apply_fn=_apply_pua_protection,
        remove_fn=_remove_pua_protection,
        detect_fn=_detect_pua_protection,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_PUA],
        description=("Enables Potentially Unwanted Application (PUA) detection in Windows Defender."),
        tags=["defender", "security", "adware"],
    ),
    TweakDef(
        id="sec-harden-smartscreen",
        label="Harden SmartScreen (Warn + Block)",
        category="Security",
        apply_fn=_apply_harden_smartscreen,
        remove_fn=_remove_harden_smartscreen,
        detect_fn=_detect_harden_smartscreen,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_SMARTSCREEN],
        description=("Sets SmartScreen to warn and block unrecognized apps and downloads."),
        tags=["smartscreen", "security"],
    ),
    TweakDef(
        id="sec-disable-exploit-telemetry",
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
        id="sec-defender-scan-cpu-limit",
        label="Limit Defender Scan CPU to 25%",
        category="Security",
        apply_fn=_apply_scan_cpu_limit,
        remove_fn=_remove_scan_cpu_limit,
        detect_fn=_detect_scan_cpu_limit,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_SCAN],
        description=("Limits Windows Defender scheduled-scan CPU usage to 25% to reduce impact during scans."),
        tags=["defender", "performance", "cpu"],
    ),
    TweakDef(
        id="sec-disable-defender-notifications",
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
        id="sec-defender-dev-exclusions",
        label="Add Developer Folder Exclusions",
        category="Security",
        apply_fn=_apply_dev_exclusions,
        remove_fn=_remove_dev_exclusions,
        detect_fn=_detect_dev_exclusions,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_EXCLUSIONS],
        description=("Excludes common dev folders (source/repos, .cargo, .rustup, go, node_modules) from real-time Defender scans."),
        tags=["defender", "developer", "performance"],
    ),
    TweakDef(
        id="sec-enable-controlled-folder-access",
        label="Enable Controlled Folder Access",
        category="Security",
        apply_fn=_apply_controlled_folder_access,
        remove_fn=_remove_controlled_folder_access,
        detect_fn=_detect_controlled_folder_access,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_CFA_KEY],
        description=("Enables Controlled Folder Access (ransomware protection) which blocks unauthorized changes to protected folders."),
        tags=["defender", "ransomware", "security"],
    ),
    TweakDef(
        id="sec-enable-network-protection",
        label="Enable Network Protection",
        category="Security",
        apply_fn=_apply_network_protection,
        remove_fn=_remove_network_protection,
        detect_fn=_detect_network_protection,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_NET_PROTECT],
        description=("Enables Defender Network Protection to block connections to malicious domains and IP addresses."),
        tags=["defender", "network", "security"],
    ),
    TweakDef(
        id="sec-enable-asr-rules",
        label="Enable Attack Surface Reduction",
        category="Security",
        apply_fn=_apply_asr_rules,
        remove_fn=_remove_asr_rules,
        detect_fn=_detect_asr_rules,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_ASR],
        description=("Enables Defender ASR rules which block common attack vectors like Office macro exploits and script-based threats."),
        tags=["defender", "asr", "security", "enterprise"],
    ),
    TweakDef(
        id="sec-disable-defender-realtime",
        label="Disable Real-Time Protection",
        category="Security",
        apply_fn=_apply_disable_realtime,
        remove_fn=_remove_disable_realtime,
        detect_fn=_detect_disable_realtime,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_RT],
        description=("Disables Defender real-time, behavior, and on-access monitoring for maximum performance. USE WITH CAUTION."),
        tags=["defender", "performance", "realtime"],
    ),
    TweakDef(
        id="sec-disable-edge-smartscreen",
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


# -- Disable Credential Guard --------------------------------------------------

_DEVICE_GUARD = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard"


def _apply_sec_disable_credential_guard(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Security: disabling Credential Guard")
    SESSION.backup([_DEVICE_GUARD], "CredentialGuard")
    SESSION.set_dword(_DEVICE_GUARD, "EnableVirtualizationBasedSecurity", 0)
    SESSION.log("Security: Credential Guard disabled")


def _remove_sec_disable_credential_guard(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_DEVICE_GUARD], "CredentialGuard_Remove")
    SESSION.set_dword(_DEVICE_GUARD, "EnableVirtualizationBasedSecurity", 1)


def _detect_sec_disable_credential_guard() -> bool:
    return SESSION.read_dword(_DEVICE_GUARD, "EnableVirtualizationBasedSecurity") == 0


# -- Enable Logon Event Auditing -----------------------------------------------

_LSA = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa"


def _apply_sec_enable_audit_logon(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Security: enabling logon event auditing")
    SESSION.backup([_LSA], "AuditLogon")
    SESSION.set_dword(_LSA, "AuditLogonEvents", 3)
    SESSION.log("Security: logon event auditing enabled")


def _remove_sec_enable_audit_logon(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_LSA], "AuditLogon_Remove")
    SESSION.set_dword(_LSA, "AuditLogonEvents", 0)


def _detect_sec_enable_audit_logon() -> bool:
    return SESSION.read_dword(_LSA, "AuditLogonEvents") == 3


TWEAKS += [
    TweakDef(
        id="sec-disable-credential-guard",
        label="Disable Credential Guard",
        category="Security",
        apply_fn=_apply_sec_disable_credential_guard,
        remove_fn=_remove_sec_disable_credential_guard,
        detect_fn=_detect_sec_disable_credential_guard,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_DEVICE_GUARD],
        description=(
            "Disables Virtualization Based Security / Credential Guard. May improve performance. Default: Enabled. Recommended: Keep enabled."
        ),
        tags=["security", "credential-guard", "vbs", "performance"],
    ),
    TweakDef(
        id="sec-enable-audit-logon",
        label="Enable Logon Event Auditing",
        category="Security",
        apply_fn=_apply_sec_enable_audit_logon,
        remove_fn=_remove_sec_enable_audit_logon,
        detect_fn=_detect_sec_enable_audit_logon,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_LSA],
        description=("Enables auditing of logon success and failure events. Default: Disabled. Recommended: Enabled for security monitoring."),
        tags=["security", "audit", "logon", "monitoring"],
    ),
]


# ── Disable PUA Protection ───────────────────────────────────────────────────


def _apply_sec_disable_pua(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Security: disabling PUA/Potentially Unwanted App detection")
    SESSION.backup([_PUA], "PUAProtection")
    SESSION.set_dword(_PUA, "MpEnablePus", 0)


def _remove_sec_disable_pua(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_PUA, "MpEnablePus", 1)


def _detect_sec_disable_pua() -> bool:
    return SESSION.read_dword(_PUA, "MpEnablePus") == 0


# ── Set Defender Scan Schedule to Weekly ─────────────────────────────────────

_SCAN = rf"{_DEFENDER}\Scan"


def _apply_sec_scan_weekly(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Security: setting scan schedule to weekly (Sunday)")
    SESSION.backup([_SCAN], "ScanScheduleWeekly")
    SESSION.set_dword(_SCAN, "ScheduleDay", 1)  # 1 = Sunday


def _remove_sec_scan_weekly(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SCAN, "ScheduleDay")


def _detect_sec_scan_weekly() -> bool:
    return SESSION.read_dword(_SCAN, "ScheduleDay") == 1


# ── Disable Automatic Sample Submission ──────────────────────────────────────


def _apply_sec_disable_sample_submission(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Security: disabling automatic sample submission")
    SESSION.backup([_SPYNET], "SampleSubmission")
    SESSION.set_dword(_SPYNET, "SubmitSamplesConsent", 2)  # 2 = Never send


def _remove_sec_disable_sample_submission(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_SPYNET, "SubmitSamplesConsent", 1)  # 1 = Safe samples


def _detect_sec_disable_sample_submission() -> bool:
    return SESSION.read_dword(_SPYNET, "SubmitSamplesConsent") == 2


TWEAKS += [
    TweakDef(
        id="sec-disable-pua-protection",
        label="Disable PUA Detection",
        category="Security",
        apply_fn=_apply_sec_disable_pua,
        remove_fn=_remove_sec_disable_pua,
        detect_fn=_detect_sec_disable_pua,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_PUA],
        description=(
            "Disables Potentially Unwanted Application (PUA) detection "
            "in Windows Defender via MpEnablePus policy. "
            "Default: Enabled. Recommended: Keep enabled for safety."
        ),
        tags=["security", "defender", "pua", "detection", "policy"],
    ),
    TweakDef(
        id="sec-scan-schedule-weekly",
        label="Set Defender Scan Schedule to Weekly",
        category="Security",
        apply_fn=_apply_sec_scan_weekly,
        remove_fn=_remove_sec_scan_weekly,
        detect_fn=_detect_sec_scan_weekly,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_SCAN],
        description=(
            "Sets Windows Defender scheduled scan to run weekly on Sunday "
            "instead of daily. Reduces system overhead. "
            "Default: Daily. Recommended: Weekly for low-risk environments."
        ),
        tags=["security", "defender", "scan", "schedule", "weekly"],
    ),
    TweakDef(
        id="sec-disable-auto-sample-submission",
        label="Disable Automatic Sample Submission",
        category="Security",
        apply_fn=_apply_sec_disable_sample_submission,
        remove_fn=_remove_sec_disable_sample_submission,
        detect_fn=_detect_sec_disable_sample_submission,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_SPYNET],
        description=(
            "Disables automatic submission of file samples to Microsoft "
            "for cloud-based analysis (SubmitSamplesConsent=2). "
            "Default: Safe samples. Recommended: Disabled for privacy."
        ),
        tags=["security", "defender", "samples", "cloud", "privacy"],
    ),
]


# ══ Additional Security Tweaks ═════════════════════════════════════════

_SMB_PARAMS = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services"
    r"\LanmanServer\Parameters"
)


def _apply_sec_disable_smbv1(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Security: disable SMBv1 protocol")
    SESSION.backup([_SMB_PARAMS], "SecSMBv1")
    SESSION.set_dword(_SMB_PARAMS, "SMB1", 0)


def _remove_sec_disable_smbv1(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_SMB_PARAMS, "SMB1", 1)


def _detect_sec_disable_smbv1() -> bool:
    return SESSION.read_dword(_SMB_PARAMS, "SMB1") == 0


def _apply_sec_enable_lsa_protection(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Security: enable LSA protection (RunAsPPL)")
    SESSION.backup([_LSA], "SecLSAProtection")
    SESSION.set_dword(_LSA, "RunAsPPL", 1)


def _remove_sec_enable_lsa_protection(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_LSA, "RunAsPPL", 0)


def _detect_sec_enable_lsa_protection() -> bool:
    return SESSION.read_dword(_LSA, "RunAsPPL") == 1


TWEAKS += [
    TweakDef(
        id="sec-disable-smbv1",
        label="Disable SMBv1 Protocol",
        category="Security",
        apply_fn=_apply_sec_disable_smbv1,
        remove_fn=_remove_sec_disable_smbv1,
        detect_fn=_detect_sec_disable_smbv1,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_SMB_PARAMS],
        description=(
            "Disables the legacy SMBv1 protocol on the server side. "
            "Mitigates WannaCry and EternalBlue vulnerabilities. "
            "Default: Enabled. Recommended: Disabled."
        ),
        tags=["security", "smb", "smbv1", "protocol", "vulnerability"],
    ),
    TweakDef(
        id="sec-enable-lsa-protection",
        label="Enable LSA Protection (RunAsPPL)",
        category="Security",
        apply_fn=_apply_sec_enable_lsa_protection,
        remove_fn=_remove_sec_enable_lsa_protection,
        detect_fn=_detect_sec_enable_lsa_protection,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_LSA],
        description=(
            "Enables Local Security Authority (LSA) protection by running "
            "LSASS as a Protected Process Light (PPL). Mitigates credential theft. "
            "Default: Disabled. Recommended: Enabled."
        ),
        tags=["security", "lsa", "lsass", "ppl", "credential"],
    ),
]

# ── Additional security tweaks ────────────────────────────────────────────────

_SPECTRE = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"
_UAC_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"
_NTLM_POLICY = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa"


def _apply_sec_enable_spectre_mitigations(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_SPECTRE], "SecSpectreMitigations")
    SESSION.set_dword(_SPECTRE, "FeatureSettingsOverride", 0)
    SESSION.set_dword(_SPECTRE, "FeatureSettingsOverrideMask", 3)


def _remove_sec_enable_spectre_mitigations(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_SPECTRE], "SecSpectreMitigations_Remove")
    SESSION.delete_value(_SPECTRE, "FeatureSettingsOverride")
    SESSION.delete_value(_SPECTRE, "FeatureSettingsOverrideMask")


def _detect_sec_enable_spectre_mitigations() -> bool:
    return SESSION.read_dword(_SPECTRE, "FeatureSettingsOverride") == 0 and SESSION.read_dword(_SPECTRE, "FeatureSettingsOverrideMask") == 3


def _apply_sec_uac_always_notify(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_UAC_POLICY], "SecUACAlwaysNotify")
    SESSION.set_dword(_UAC_POLICY, "ConsentPromptBehaviorAdmin", 2)
    SESSION.set_dword(_UAC_POLICY, "PromptOnSecureDesktop", 1)


def _remove_sec_uac_always_notify(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_UAC_POLICY], "SecUACAlwaysNotify_Remove")
    SESSION.set_dword(_UAC_POLICY, "ConsentPromptBehaviorAdmin", 5)
    SESSION.set_dword(_UAC_POLICY, "PromptOnSecureDesktop", 1)


def _detect_sec_uac_always_notify() -> bool:
    return SESSION.read_dword(_UAC_POLICY, "ConsentPromptBehaviorAdmin") == 2


def _apply_sec_restrict_ntlmv1(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_NTLM_POLICY], "SecRestrictNtlmv1")
    SESSION.set_dword(_NTLM_POLICY, "LmCompatibilityLevel", 5)


def _remove_sec_restrict_ntlmv1(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_NTLM_POLICY], "SecRestrictNtlmv1_Remove")
    SESSION.set_dword(_NTLM_POLICY, "LmCompatibilityLevel", 3)


def _detect_sec_restrict_ntlmv1() -> bool:
    return SESSION.read_dword(_NTLM_POLICY, "LmCompatibilityLevel") == 5


TWEAKS += [
    TweakDef(
        id="sec-enable-spectre-mitigations",
        label="Enable Spectre/Meltdown Mitigations",
        category="Security",
        apply_fn=_apply_sec_enable_spectre_mitigations,
        remove_fn=_remove_sec_enable_spectre_mitigations,
        detect_fn=_detect_sec_enable_spectre_mitigations,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_SPECTRE],
        description=(
            "Ensures Spectre (variant 2) and Meltdown mitigations are enabled via "
            "FeatureSettingsOverride. May reduce performance on older CPUs. "
            "Default: usually enabled by Windows Update. Recommended: Enabled."
        ),
        tags=["security", "spectre", "meltdown", "cpu", "vulnerability", "mitigations"],
    ),
    TweakDef(
        id="sec-uac-always-notify",
        label="Set UAC to Always Notify (Highest Level)",
        category="Security",
        apply_fn=_apply_sec_uac_always_notify,
        remove_fn=_remove_sec_uac_always_notify,
        detect_fn=_detect_sec_uac_always_notify,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_UAC_POLICY],
        description=(
            "Sets UAC to 'Always notify' (ConsentPromptBehaviorAdmin=2) — prompts "
            "for both Windows changes and other program elevation requests. "
            "Default: notify only for app changes (5). Recommended: Always notify."
        ),
        tags=["security", "uac", "elevation", "prompt", "consent"],
    ),
    TweakDef(
        id="sec-restrict-ntlmv1",
        label="Require NTLMv2 (Block LM and NTLMv1)",
        category="Security",
        apply_fn=_apply_sec_restrict_ntlmv1,
        remove_fn=_remove_sec_restrict_ntlmv1,
        detect_fn=_detect_sec_restrict_ntlmv1,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_NTLM_POLICY],
        description=(
            "Sets LmCompatibilityLevel=5 to only use NTLMv2 and refuse LM/NTLMv1 "
            "responses. Hardens network authentication. May break legacy devices. "
            "Default: 3 (NTLMv2 only send). Recommended: 5 for hardened environments."
        ),
        tags=["security", "ntlm", "ntlmv1", "authentication", "network", "lm"],
    ),
]

# ── Extra security controls ─────────────────────────────────────────────────

_WDIGEST = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\WDigest"
_SAM_HARDEN = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa"
_SCRIPT_SCAN = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Scan"
_EXCLUSIONS = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Exclusions"
_BEHAVIOR_MON = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection"


def _apply_sec_disable_wdigest(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_WDIGEST], "WDigest")
    SESSION.set_dword(_WDIGEST, "UseLogonCredential", 0)


def _remove_sec_disable_wdigest(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_WDIGEST, "UseLogonCredential")


def _detect_sec_disable_wdigest() -> bool:
    return SESSION.read_dword(_WDIGEST, "UseLogonCredential") == 0


def _apply_sec_enable_cred_guard_policy(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_SAM_HARDEN], "CredGuardPolicy")
    SESSION.set_dword(_SAM_HARDEN, "LsaCfgFlags", 1)


def _remove_sec_enable_cred_guard_policy(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SAM_HARDEN, "LsaCfgFlags")


def _detect_sec_enable_cred_guard_policy() -> bool:
    return SESSION.read_dword(_SAM_HARDEN, "LsaCfgFlags") == 1


def _apply_sec_enable_startup_scan(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_SCRIPT_SCAN], "StartupScan")
    SESSION.set_dword(_SCRIPT_SCAN, "ScanOnlyIfIdle", 0)


def _remove_sec_enable_startup_scan(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SCRIPT_SCAN, "ScanOnlyIfIdle")


def _detect_sec_enable_startup_scan() -> bool:
    return SESSION.read_dword(_SCRIPT_SCAN, "ScanOnlyIfIdle") == 0


def _apply_sec_block_exclusion_editing(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_EXCLUSIONS], "ExclusionBlock")
    SESSION.set_dword(_EXCLUSIONS, "DisableLocalAdminMerge", 1)


def _remove_sec_block_exclusion_editing(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_EXCLUSIONS, "DisableLocalAdminMerge")


def _detect_sec_block_exclusion_editing() -> bool:
    return SESSION.read_dword(_EXCLUSIONS, "DisableLocalAdminMerge") == 1


def _apply_sec_enable_behavior_monitoring(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_BEHAVIOR_MON], "BehaviorMon")
    SESSION.set_dword(_BEHAVIOR_MON, "DisableBehaviorMonitoring", 0)


def _remove_sec_enable_behavior_monitoring(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_BEHAVIOR_MON, "DisableBehaviorMonitoring", 1)


def _detect_sec_enable_behavior_monitoring() -> bool:
    return SESSION.read_dword(_BEHAVIOR_MON, "DisableBehaviorMonitoring") == 0


TWEAKS += [
    TweakDef(
        id="sec-disable-wdigest",
        label="Disable WDigest Authentication (Credential Hardening)",
        category="Security",
        apply_fn=_apply_sec_disable_wdigest,
        remove_fn=_remove_sec_disable_wdigest,
        detect_fn=_detect_sec_disable_wdigest,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_WDIGEST],
        description=(
            "Disables WDigest authentication to prevent plain-text password storage in LSASS. "
            "Mitigates credential harvesting attacks via Mimikatz. "
            "Default: Enabled on older systems. Recommended: Disabled."
        ),
        tags=["security", "wdigest", "lsass", "credential", "mimikatz"],
    ),
    TweakDef(
        id="sec-enable-cred-guard-policy",
        label="Enable Credential Guard via Policy",
        category="Security",
        apply_fn=_apply_sec_enable_cred_guard_policy,
        remove_fn=_remove_sec_enable_cred_guard_policy,
        detect_fn=_detect_sec_enable_cred_guard_policy,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_SAM_HARDEN],
        description=(
            "Enables Credential Guard via LsaCfgFlags=1, protecting LSASS credential secrets. "
            "Requires TPM 2.0 and Secure Boot. "
            "Default: Disabled. Recommended: Enabled on modern hardware."
        ),
        tags=["security", "credential-guard", "lsa", "tpm", "secureboot"],
    ),
    TweakDef(
        id="sec-scan-not-idle-only",
        label="Allow Defender Scans When System is Busy",
        category="Security",
        apply_fn=_apply_sec_enable_startup_scan,
        remove_fn=_remove_sec_enable_startup_scan,
        detect_fn=_detect_sec_enable_startup_scan,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_SCRIPT_SCAN],
        description=(
            "Disables the ScanOnlyIfIdle requirement, allowing Defender scans "
            "to run even when the system is in use. Ensures scans complete on time. "
            "Default: Idle-only. Recommended: Always allow."
        ),
        tags=["security", "defender", "scan", "schedule", "idle"],
    ),
    TweakDef(
        id="sec-block-exclusion-local-merge",
        label="Block Local Admin from Adding Defender Exclusions",
        category="Security",
        apply_fn=_apply_sec_block_exclusion_editing,
        remove_fn=_remove_sec_block_exclusion_editing,
        detect_fn=_detect_sec_block_exclusion_editing,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_EXCLUSIONS],
        description=(
            "Prevents local admins from merging Defender exclusion lists with policy exclusions. "
            "Ensures exclusion policy cannot be bypassed locally. "
            "Default: Allowed. Recommended: Blocked in managed environments."
        ),
        tags=["security", "defender", "exclusions", "policy", "hardening"],
    ),
    TweakDef(
        id="sec-enable-behavior-monitoring",
        label="Enable Defender Behavior Monitoring",
        category="Security",
        apply_fn=_apply_sec_enable_behavior_monitoring,
        remove_fn=_remove_sec_enable_behavior_monitoring,
        detect_fn=_detect_sec_enable_behavior_monitoring,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_BEHAVIOR_MON],
        description=(
            "Enables Defender behavior monitoring, which watches processes for "
            "suspicious activity patterns beyond signature-based detection. "
            "Default: Enabled. Recommended: Enabled."
        ),
        tags=["security", "defender", "behavior", "monitoring", "heuristics"],
    ),
]
