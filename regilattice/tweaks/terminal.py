"""Windows Terminal & Console tweaks.

Covers default terminal application, console V2, VT processing,
quick edit mode, legacy console settings, and terminal appearance.
"""

from __future__ import annotations

from regilattice.registry import SESSION
from regilattice.tweaks import TweakDef

# ── Key paths ────────────────────────────────────────────────────────────────

_CONSOLE = r"HKEY_CURRENT_USER\Console"
_DEFAULT_TERMINAL = (
    r"HKEY_CURRENT_USER\Console\%%Startup"
)
_CONSOLE_V2 = r"HKEY_CURRENT_USER\Console"
_VIRTUAL_TERMINAL = r"HKEY_CURRENT_USER\Console"
_CMD_KEYS = r"HKEY_CURRENT_USER\Console"
_WIN_TERMINAL_POLICY = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal"
)
_EXPLORER_ADV = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows"
    r"\CurrentVersion\Explorer\Advanced"
)
_TERM_DELEGATE = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows"
    r"\CurrentVersion\Console\TerminalSettings"
)


# ── Set Windows Terminal as Default Terminal ─────────────────────────────────


def _apply_default_wt(*, require_admin: bool = False) -> None:
    SESSION.log("Terminal: set Windows Terminal as default console host")
    SESSION.backup([_DEFAULT_TERMINAL], "DefaultTerminal")
    # {2EACA947-7F5F-4CFA-BA87-8F7FBEEFBE69} = Windows Terminal GUID
    SESSION.set_string(
        _DEFAULT_TERMINAL, "DelegationConsole",
        "{2EACA947-7F5F-4CFA-BA87-8F7FBEEFBE69}",
    )
    SESSION.set_string(
        _DEFAULT_TERMINAL, "DelegationTerminal",
        "{E12CFF52-A866-4C77-9A90-F570A7AA2C6B}",
    )


def _remove_default_wt(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_DEFAULT_TERMINAL, "DelegationConsole")
    SESSION.delete_value(_DEFAULT_TERMINAL, "DelegationTerminal")


def _detect_default_wt() -> bool:
    val = SESSION.read_string(_DEFAULT_TERMINAL, "DelegationConsole")
    return val == "{2EACA947-7F5F-4CFA-BA87-8F7FBEEFBE69}"


# ── Enable Console V2 (New Console Host) ─────────────────────────────────────


def _apply_console_v2(*, require_admin: bool = False) -> None:
    SESSION.log("Terminal: enable new Console V2 host")
    SESSION.backup([_CONSOLE_V2], "ConsoleV2")
    SESSION.set_dword(_CONSOLE_V2, "ForceV2", 1)


def _remove_console_v2(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_CONSOLE_V2, "ForceV2", 0)


def _detect_console_v2() -> bool:
    return SESSION.read_dword(_CONSOLE_V2, "ForceV2") == 1


# ── Enable Virtual Terminal Processing ───────────────────────────────────────


def _apply_vt_processing(*, require_admin: bool = False) -> None:
    SESSION.log("Terminal: enable VT100/ANSI escape sequence processing")
    SESSION.backup([_VIRTUAL_TERMINAL], "VTProcessing")
    SESSION.set_dword(_VIRTUAL_TERMINAL, "VirtualTerminalLevel", 1)


def _remove_vt_processing(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_VIRTUAL_TERMINAL, "VirtualTerminalLevel", 0)


def _detect_vt_processing() -> bool:
    return SESSION.read_dword(_VIRTUAL_TERMINAL, "VirtualTerminalLevel") == 1


# ── Disable Quick Edit Mode ─────────────────────────────────────────────────


def _apply_disable_quick_edit(*, require_admin: bool = False) -> None:
    SESSION.log("Terminal: disable Quick Edit mode (prevents accidental pause)")
    SESSION.backup([_CONSOLE], "QuickEdit")
    SESSION.set_dword(_CONSOLE, "QuickEdit", 0)


def _remove_disable_quick_edit(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_CONSOLE, "QuickEdit", 1)


def _detect_disable_quick_edit() -> bool:
    return SESSION.read_dword(_CONSOLE, "QuickEdit") == 0


# ── Enable Insert Mode by Default ────────────────────────────────────────────


def _apply_insert_mode(*, require_admin: bool = False) -> None:
    SESSION.log("Terminal: enable insert mode by default")
    SESSION.backup([_CONSOLE], "InsertMode")
    SESSION.set_dword(_CONSOLE, "InsertMode", 1)


def _remove_insert_mode(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_CONSOLE, "InsertMode", 0)


def _detect_insert_mode() -> bool:
    return SESSION.read_dword(_CONSOLE, "InsertMode") == 1


# ── Set Console Buffer Size (Lines) ─────────────────────────────────────────


def _apply_large_buffer(*, require_admin: bool = False) -> None:
    SESSION.log("Terminal: set console screen buffer to 9999 lines")
    SESSION.backup([_CONSOLE], "LargeBuffer")
    SESSION.set_dword(_CONSOLE, "ScreenBufferSize", 0x270F0078)  # 9999 lines x 120 cols


def _remove_large_buffer(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_CONSOLE, "ScreenBufferSize", 0x012C0078)  # 300 lines x 120 cols


def _detect_large_buffer() -> bool:
    val = SESSION.read_dword(_CONSOLE, "ScreenBufferSize")
    return val is not None and (val >> 16) >= 9999


# ── Enable Line Wrapping ────────────────────────────────────────────────────


def _apply_line_wrap(*, require_admin: bool = False) -> None:
    SESSION.log("Terminal: enable line wrapping on resize")
    SESSION.backup([_CONSOLE], "LineWrap")
    SESSION.set_dword(_CONSOLE, "LineWrap", 1)


def _remove_line_wrap(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_CONSOLE, "LineWrap", 0)


def _detect_line_wrap() -> bool:
    return SESSION.read_dword(_CONSOLE, "LineWrap") == 1


# ── Disable Legacy Console Mode ─────────────────────────────────────────────

_LEGACY = r"HKEY_CURRENT_USER\Console"


def _apply_disable_legacy(*, require_admin: bool = False) -> None:
    SESSION.log("Terminal: disable legacy console mode")
    SESSION.backup([_LEGACY], "LegacyConsole")
    SESSION.set_dword(_LEGACY, "UseLegacyConsole", 0)


def _remove_disable_legacy(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_LEGACY, "UseLegacyConsole", 1)


def _detect_disable_legacy() -> bool:
    return SESSION.read_dword(_LEGACY, "UseLegacyConsole") == 0


# ── Set Console Font to Cascadia Mono ────────────────────────────────────────


def _apply_console_font(*, require_admin: bool = False) -> None:
    SESSION.log("Terminal: set console font to Cascadia Mono")
    SESSION.backup([_CONSOLE], "ConsoleFont")
    SESSION.set_string(_CONSOLE, "FaceName", "Cascadia Mono")
    SESSION.set_dword(_CONSOLE, "FontSize", 0x00100000)  # 16pt
    SESSION.set_dword(_CONSOLE, "FontFamily", 54)  # TrueType


def _remove_console_font(*, require_admin: bool = False) -> None:
    SESSION.set_string(_CONSOLE, "FaceName", "Consolas")
    SESSION.set_dword(_CONSOLE, "FontSize", 0x000E0000)  # 14pt
    SESSION.set_dword(_CONSOLE, "FontFamily", 54)


def _detect_console_font() -> bool:
    return SESSION.read_string(_CONSOLE, "FaceName") == "Cascadia Mono"


# ── Enable Ctrl+C / Ctrl+V in Console ───────────────────────────────────────


def _apply_ctrl_cv(*, require_admin: bool = False) -> None:
    SESSION.log("Terminal: enable Ctrl+Shift+C/V copy-paste shortcuts")
    SESSION.backup([_CONSOLE], "CtrlCV")
    SESSION.set_dword(_CONSOLE, "CtrlKeyShortcutsDisabled", 0)
    SESSION.set_dword(_CONSOLE, "FilterOnPaste", 1)
    SESSION.set_dword(_CONSOLE, "LineSelection", 1)


def _remove_ctrl_cv(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_CONSOLE, "CtrlKeyShortcutsDisabled")
    SESSION.delete_value(_CONSOLE, "FilterOnPaste")
    SESSION.delete_value(_CONSOLE, "LineSelection")


def _detect_ctrl_cv() -> bool:
    return SESSION.read_dword(_CONSOLE, "CtrlKeyShortcutsDisabled") == 0


# ── Set Console Color Scheme (Campbell) ──────────────────────────────────────


def _apply_campbell_colors(*, require_admin: bool = False) -> None:
    SESSION.log("Terminal: set console colors to Campbell color scheme")
    SESSION.backup([_CONSOLE], "CampbellColors")
    # Campbell dark background (#0C0C0C) and foreground (#CCCCCC)
    SESSION.set_dword(_CONSOLE, "ScreenColors", 0x07)
    SESSION.set_dword(_CONSOLE, "PopupColors", 0xF5)
    SESSION.set_dword(_CONSOLE, "ColorTable00", 0x000C0C0C)  # Black
    SESSION.set_dword(_CONSOLE, "ColorTable01", 0x00DA3700)  # DarkBlue
    SESSION.set_dword(_CONSOLE, "ColorTable07", 0x00CCCCCC)  # Gray


def _remove_campbell_colors(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_CONSOLE, "ScreenColors")
    SESSION.delete_value(_CONSOLE, "PopupColors")
    SESSION.delete_value(_CONSOLE, "ColorTable00")
    SESSION.delete_value(_CONSOLE, "ColorTable01")
    SESSION.delete_value(_CONSOLE, "ColorTable07")


def _detect_campbell_colors() -> bool:
    return SESSION.read_dword(_CONSOLE, "ColorTable00") == 0x000C0C0C


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="term-default-windows-terminal",
        label="Set Windows Terminal as Default",
        category="Windows Terminal",
        apply_fn=_apply_default_wt,
        remove_fn=_remove_default_wt,
        detect_fn=_detect_default_wt,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_DEFAULT_TERMINAL],
        description=(
            "Sets Windows Terminal as the default terminal application "
            "for cmd.exe and PowerShell. Requires Windows Terminal installed. "
            "Default: Windows Console Host. Recommended: Windows Terminal."
        ),
        tags=["terminal", "windows-terminal", "default", "console"],
    ),
    TweakDef(
        id="term-enable-console-v2",
        label="Enable Console V2 Host",
        category="Windows Terminal",
        apply_fn=_apply_console_v2,
        remove_fn=_remove_console_v2,
        detect_fn=_detect_console_v2,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CONSOLE_V2],
        description=(
            "Forces the new Console V2 host with ANSI support, "
            "line wrapping, and improved rendering. "
            "Default: 1 (enabled). Recommended: 1."
        ),
        tags=["terminal", "console", "v2", "modern"],
    ),
    TweakDef(
        id="term-enable-vt-processing",
        label="Enable Virtual Terminal (ANSI) Processing",
        category="Windows Terminal",
        apply_fn=_apply_vt_processing,
        remove_fn=_remove_vt_processing,
        detect_fn=_detect_vt_processing,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_VIRTUAL_TERMINAL],
        description=(
            "Enables VT100/ANSI escape sequence processing in the console. "
            "Required for colored output in many CLI tools. "
            "Default: 0 (off). Recommended: 1 (on)."
        ),
        tags=["terminal", "vt100", "ansi", "colors", "escape"],
    ),
    TweakDef(
        id="term-disable-quick-edit",
        label="Disable Quick Edit Mode",
        category="Windows Terminal",
        apply_fn=_apply_disable_quick_edit,
        remove_fn=_remove_disable_quick_edit,
        detect_fn=_detect_disable_quick_edit,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CONSOLE],
        description=(
            "Disables Quick Edit so clicking the console window does not "
            "pause running commands. Prevents accidental hangs. "
            "Default: 1 (on). Recommended: 0 (off)."
        ),
        tags=["terminal", "quickedit", "console", "hang"],
    ),
    TweakDef(
        id="term-enable-insert-mode",
        label="Enable Insert Mode by Default",
        category="Windows Terminal",
        apply_fn=_apply_insert_mode,
        remove_fn=_remove_insert_mode,
        detect_fn=_detect_insert_mode,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CONSOLE],
        description=(
            "Sets insert mode as the default typing mode in consoles. "
            "Default: 1 (insert). Recommended: 1."
        ),
        tags=["terminal", "insert", "mode", "typing"],
    ),
    TweakDef(
        id="term-large-buffer",
        label="Set Large Screen Buffer (9999 Lines)",
        category="Windows Terminal",
        apply_fn=_apply_large_buffer,
        remove_fn=_remove_large_buffer,
        detect_fn=_detect_large_buffer,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CONSOLE],
        description=(
            "Increases the console screen buffer to 9999 lines for "
            "longer scrollback history. "
            "Default: 300 lines. Recommended: 9999."
        ),
        tags=["terminal", "buffer", "scrollback", "history"],
    ),
    TweakDef(
        id="term-enable-line-wrap",
        label="Enable Line Wrapping",
        category="Windows Terminal",
        apply_fn=_apply_line_wrap,
        remove_fn=_remove_line_wrap,
        detect_fn=_detect_line_wrap,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CONSOLE],
        description=(
            "Enables automatic line wrapping when resizing the console. "
            "Default: 1. Recommended: 1."
        ),
        tags=["terminal", "wrap", "resize", "console"],
    ),
    TweakDef(
        id="term-disable-legacy-console",
        label="Disable Legacy Console Mode",
        category="Windows Terminal",
        apply_fn=_apply_disable_legacy,
        remove_fn=_remove_disable_legacy,
        detect_fn=_detect_disable_legacy,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_LEGACY],
        description=(
            "Disables the legacy console subsystem. Required for "
            "Console V2 features like ANSI escape support. "
            "Default: 0 (modern). Recommended: 0."
        ),
        tags=["terminal", "legacy", "console", "modern"],
    ),
    TweakDef(
        id="term-console-font-cascadia",
        label="Set Console Font to Cascadia Mono",
        category="Windows Terminal",
        apply_fn=_apply_console_font,
        remove_fn=_remove_console_font,
        detect_fn=_detect_console_font,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CONSOLE],
        description=(
            "Sets the default console font to Cascadia Mono at 16pt. "
            "Bundled with Windows Terminal; supports ligatures. "
            "Default: Consolas 14pt. Recommended: Cascadia Mono 16pt."
        ),
        tags=["terminal", "font", "cascadia", "appearance"],
    ),
    TweakDef(
        id="term-enable-ctrl-cv",
        label="Enable Ctrl+Shift+C/V Copy-Paste",
        category="Windows Terminal",
        apply_fn=_apply_ctrl_cv,
        remove_fn=_remove_ctrl_cv,
        detect_fn=_detect_ctrl_cv,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CONSOLE],
        description=(
            "Enables Ctrl+Shift+C / Ctrl+Shift+V clipboard shortcuts "
            "in the classic console. Also enables filter-on-paste and "
            "line selection. Default: enabled. Recommended: enabled."
        ),
        tags=["terminal", "copy", "paste", "keyboard", "shortcuts"],
    ),
    TweakDef(
        id="term-campbell-color-scheme",
        label="Set Campbell Color Scheme",
        category="Windows Terminal",
        apply_fn=_apply_campbell_colors,
        remove_fn=_remove_campbell_colors,
        detect_fn=_detect_campbell_colors,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CONSOLE],
        description=(
            "Applies the modern Campbell color scheme to the classic "
            "console. Darker background, improved contrast. "
            "Default: legacy colors. Recommended: Campbell."
        ),
        tags=["terminal", "colors", "theme", "campbell", "appearance"],
    ),
]
