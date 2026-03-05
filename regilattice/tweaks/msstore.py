"""Microsoft Store tweaks -- auto-install, updates, suggestions, content delivery."""

from __future__ import annotations

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# -- Key paths ----------------------------------------------------------------

_STORE_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore"
_CDM = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows"
    r"\CurrentVersion\ContentDeliveryManager"
)
_CLOUD_CONTENT = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent"
_SIUF = r"HKEY_CURRENT_USER\Software\Microsoft\Siuf\Rules"


# -- 1. Disable Microsoft Store ------------------------------------------------


def _apply_disable_store(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Microsoft Store: disable via policy")
    SESSION.backup([_STORE_POLICY], "DisableStore")
    SESSION.set_dword(_STORE_POLICY, "RemoveWindowsStore", 1)


def _remove_disable_store(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_STORE_POLICY, "RemoveWindowsStore")


def _detect_disable_store() -> bool:
    return SESSION.read_dword(_STORE_POLICY, "RemoveWindowsStore") == 1


# -- 2. Disable auto-install of suggested apps --------------------------------


def _apply_disable_auto_install(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Microsoft Store: disable auto-install of suggested apps")
    SESSION.backup([_CDM], "AutoInstall")
    SESSION.set_dword(_CDM, "SilentInstalledAppsEnabled", 0)


def _remove_disable_auto_install(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CDM, "SilentInstalledAppsEnabled")


def _detect_disable_auto_install() -> bool:
    return SESSION.read_dword(_CDM, "SilentInstalledAppsEnabled") == 0


# -- 3. Disable Store app auto-updates ----------------------------------------


def _apply_disable_auto_update(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Microsoft Store: disable app auto-updates")
    SESSION.backup([_STORE_POLICY], "AutoUpdate")
    SESSION.set_dword(_STORE_POLICY, "AutoDownload", 2)


def _remove_disable_auto_update(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_STORE_POLICY, "AutoDownload")


def _detect_disable_auto_update() -> bool:
    return SESSION.read_dword(_STORE_POLICY, "AutoDownload") == 2


# -- 4. Disable Windows tips about Store --------------------------------------


def _apply_disable_tips(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Microsoft Store: disable Windows tips about Store")
    SESSION.backup([_CDM], "StoreTips")
    SESSION.set_dword(_CDM, "SoftLandingEnabled", 0)


def _remove_disable_tips(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CDM, "SoftLandingEnabled")


def _detect_disable_tips() -> bool:
    return SESSION.read_dword(_CDM, "SoftLandingEnabled") == 0


# -- 5. Disable preinstalled apps ---------------------------------------------


def _apply_disable_preinstalled(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Microsoft Store: disable preinstalled apps")
    SESSION.backup([_CDM], "PreInstalledApps")
    SESSION.set_dword(_CDM, "PreInstalledAppsEnabled", 0)


def _remove_disable_preinstalled(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CDM, "PreInstalledAppsEnabled")


def _detect_disable_preinstalled() -> bool:
    return SESSION.read_dword(_CDM, "PreInstalledAppsEnabled") == 0


# -- 6. Disable consumer features / app suggestions ---------------------------


def _apply_disable_consumer_features(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Microsoft Store: disable consumer features and app suggestions")
    SESSION.backup([_CLOUD_CONTENT], "ConsumerFeatures")
    SESSION.set_dword(_CLOUD_CONTENT, "DisableWindowsConsumerFeatures", 1)


def _remove_disable_consumer_features(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CLOUD_CONTENT, "DisableWindowsConsumerFeatures")


def _detect_disable_consumer_features() -> bool:
    return SESSION.read_dword(_CLOUD_CONTENT, "DisableWindowsConsumerFeatures") == 1


# -- 7. Disable feedback notifications ----------------------------------------


def _apply_disable_feedback(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Microsoft Store: disable feedback notifications")
    SESSION.backup([_SIUF], "Feedback")
    SESSION.set_dword(_SIUF, "NumberOfSIUFInPeriod", 0)


def _remove_disable_feedback(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SIUF, "NumberOfSIUFInPeriod")


def _detect_disable_feedback() -> bool:
    return SESSION.read_dword(_SIUF, "NumberOfSIUFInPeriod") == 0


# -- 8. Disable Windows Spotlight ---------------------------------------------


def _apply_disable_spotlight(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Microsoft Store: disable Windows Spotlight")
    SESSION.backup([_CDM], "Spotlight")
    SESSION.set_dword(_CDM, "RotatingLockScreenEnabled", 0)
    SESSION.set_dword(_CDM, "RotatingLockScreenOverlayEnabled", 0)


def _remove_disable_spotlight(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CDM, "RotatingLockScreenEnabled")
    SESSION.delete_value(_CDM, "RotatingLockScreenOverlayEnabled")


def _detect_disable_spotlight() -> bool:
    return (
        SESSION.read_dword(_CDM, "RotatingLockScreenEnabled") == 0
        and SESSION.read_dword(_CDM, "RotatingLockScreenOverlayEnabled") == 0
    )


# -- 9. Disable app suggestions in Start --------------------------------------


def _apply_disable_app_suggestions_start(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Microsoft Store: disable app suggestions in Start menu")
    SESSION.backup([_CDM], "AppSuggestionsStart")
    SESSION.set_dword(_CDM, "SystemPaneSuggestionsEnabled", 0)


def _remove_disable_app_suggestions_start(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CDM, "SystemPaneSuggestionsEnabled")


def _detect_disable_app_suggestions_start() -> bool:
    return SESSION.read_dword(_CDM, "SystemPaneSuggestionsEnabled") == 0


# -- 10. Disable content delivery entirely ------------------------------------


def _apply_disable_content_delivery(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Microsoft Store: disable content delivery entirely")
    SESSION.backup([_CDM], "ContentDelivery")
    SESSION.set_dword(_CDM, "ContentDeliveryAllowed", 0)


def _remove_disable_content_delivery(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CDM, "ContentDeliveryAllowed")


def _detect_disable_content_delivery() -> bool:
    return SESSION.read_dword(_CDM, "ContentDeliveryAllowed") == 0


# -- Exports -------------------------------------------------------------------

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="msstore-disable-store",
        label="Disable Microsoft Store",
        category="Microsoft Store",
        apply_fn=_apply_disable_store,
        remove_fn=_remove_disable_store,
        detect_fn=_detect_disable_store,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_STORE_POLICY],
        description="Disables Microsoft Store via group policy. Default: enabled. Recommended: disabled.",
        tags=["store", "microsoft", "policy", "bloat"],
    ),
    TweakDef(
        id="msstore-disable-auto-install",
        label="Disable Auto-Install of Suggested Apps",
        category="Microsoft Store",
        apply_fn=_apply_disable_auto_install,
        remove_fn=_remove_disable_auto_install,
        detect_fn=_detect_disable_auto_install,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CDM],
        description="Disables automatic installation of suggested apps. Default: enabled. Recommended: disabled.",
        tags=["store", "auto-install", "suggestions", "bloat"],
    ),
    TweakDef(
        id="msstore-disable-auto-update",
        label="Disable Store App Auto-Updates",
        category="Microsoft Store",
        apply_fn=_apply_disable_auto_update,
        remove_fn=_remove_disable_auto_update,
        detect_fn=_detect_disable_auto_update,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_STORE_POLICY],
        description="Disables automatic updates of Store apps. Default: enabled. Recommended: disabled.",
        tags=["store", "updates", "auto-update", "policy"],
    ),
    TweakDef(
        id="msstore-disable-tips",
        label="Disable Windows Tips About Store",
        category="Microsoft Store",
        apply_fn=_apply_disable_tips,
        remove_fn=_remove_disable_tips,
        detect_fn=_detect_disable_tips,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CDM],
        description="Disables Windows tips and suggestions about the Store. Default: enabled. Recommended: disabled.",
        tags=["store", "tips", "suggestions", "ux"],
    ),
    TweakDef(
        id="msstore-disable-preinstalled",
        label="Disable Preinstalled Apps",
        category="Microsoft Store",
        apply_fn=_apply_disable_preinstalled,
        remove_fn=_remove_disable_preinstalled,
        detect_fn=_detect_disable_preinstalled,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CDM],
        description="Disables preinstalled apps from being installed on new accounts. Default: enabled. Recommended: disabled.",
        tags=["store", "preinstalled", "bloat", "cleanup"],
    ),
    TweakDef(
        id="msstore-disable-consumer-features",
        label="Disable Consumer Features / App Suggestions",
        category="Microsoft Store",
        apply_fn=_apply_disable_consumer_features,
        remove_fn=_remove_disable_consumer_features,
        detect_fn=_detect_disable_consumer_features,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_CLOUD_CONTENT],
        description="Disables Windows consumer features and app suggestions. Default: enabled. Recommended: disabled.",
        tags=["store", "consumer", "suggestions", "policy"],
    ),
    TweakDef(
        id="msstore-disable-feedback",
        label="Disable Feedback Notifications",
        category="Microsoft Store",
        apply_fn=_apply_disable_feedback,
        remove_fn=_remove_disable_feedback,
        detect_fn=_detect_disable_feedback,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SIUF],
        description="Disables feedback notification prompts. Default: enabled. Recommended: disabled.",
        tags=["store", "feedback", "notifications", "privacy"],
    ),
    TweakDef(
        id="msstore-disable-spotlight",
        label="Disable Windows Spotlight",
        category="Microsoft Store",
        apply_fn=_apply_disable_spotlight,
        remove_fn=_remove_disable_spotlight,
        detect_fn=_detect_disable_spotlight,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CDM],
        description="Disables Windows Spotlight on the lock screen. Default: enabled. Recommended: disabled.",
        tags=["store", "spotlight", "lockscreen", "ux"],
    ),
    TweakDef(
        id="msstore-disable-app-suggestions-start",
        label="Disable App Suggestions in Start",
        category="Microsoft Store",
        apply_fn=_apply_disable_app_suggestions_start,
        remove_fn=_remove_disable_app_suggestions_start,
        detect_fn=_detect_disable_app_suggestions_start,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CDM],
        description="Disables app suggestions in the Start menu. Default: enabled. Recommended: disabled.",
        tags=["store", "start", "suggestions", "ux"],
    ),
    TweakDef(
        id="msstore-disable-content-delivery",
        label="Disable Content Delivery",
        category="Microsoft Store",
        apply_fn=_apply_disable_content_delivery,
        remove_fn=_remove_disable_content_delivery,
        detect_fn=_detect_disable_content_delivery,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CDM],
        description="Disables content delivery entirely. Default: enabled. Recommended: disabled.",
        tags=["store", "content", "delivery", "privacy"],
    ),
]


# -- Disable Remote Push-to-Install -----------------------------------------------


def _apply_disable_push_install(*, require_admin: bool = True) -> None:
    SESSION.log("Microsoft Store: disable remote push-to-install")
    SESSION.backup([_CDM], "PushInstall")
    SESSION.set_dword(_CDM, "SilentInstalledAppsEnabled", 0)


def _remove_disable_push_install(*, require_admin: bool = True) -> None:
    SESSION.delete_value(_CDM, "SilentInstalledAppsEnabled")


def _detect_disable_push_install() -> bool:
    return SESSION.read_dword(_CDM, "SilentInstalledAppsEnabled") == 0


# -- Disable Windows Consumer Experiences (Policy) --------------------------------


def _apply_disable_consumer_experiences(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Microsoft Store: disable Windows consumer experiences via policy")
    SESSION.backup([_CLOUD_CONTENT], "ConsumerExperiences")
    SESSION.set_dword(_CLOUD_CONTENT, "DisableWindowsConsumerFeatures", 1)
    SESSION.set_dword(_CLOUD_CONTENT, "DisableConsumerAccountStateContent", 1)


def _remove_disable_consumer_experiences(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CLOUD_CONTENT, "DisableWindowsConsumerFeatures")
    SESSION.delete_value(_CLOUD_CONTENT, "DisableConsumerAccountStateContent")


def _detect_disable_consumer_experiences() -> bool:
    return (
        SESSION.read_dword(_CLOUD_CONTENT, "DisableWindowsConsumerFeatures") == 1
        and SESSION.read_dword(_CLOUD_CONTENT, "DisableConsumerAccountStateContent") == 1
    )


TWEAKS += [
    TweakDef(
        id="msstore-disable-push-install",
        label="Disable Remote Push-to-Install",
        category="Microsoft Store",
        apply_fn=_apply_disable_push_install,
        remove_fn=_remove_disable_push_install,
        detect_fn=_detect_disable_push_install,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CDM],
        description=(
            "Disables remote push-to-install from Microsoft Store. "
            "Prevents apps from being silently installed via the web store. "
            "Default: enabled. Recommended: disabled."
        ),
        tags=["store", "push", "install", "silent"],
    ),
    TweakDef(
        id="msstore-disable-consumer-experiences",
        label="Disable Windows Consumer Experiences (Policy)",
        category="Microsoft Store",
        apply_fn=_apply_disable_consumer_experiences,
        remove_fn=_remove_disable_consumer_experiences,
        detect_fn=_detect_disable_consumer_experiences,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_CLOUD_CONTENT],
        description=(
            "Disables Windows consumer experiences via Group Policy. "
            "Prevents bloatware, suggested apps, and consumer account content. "
            "Default: enabled. Recommended: disabled."
        ),
        tags=["store", "consumer", "bloatware", "policy", "experiences"],
    ),
]
