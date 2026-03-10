# Security — Subprocess Call Inventory

This document enumerates every `subprocess` call in the RegiLattice codebase,
the executable invoked, the source of each argument, and the security rationale.

All calls use **list arguments** (never `shell=True`) to prevent shell-injection.
No argument is constructed from unsanitised user input.

---

## Inventory

### `regilattice/elevation.py`

| Call site | Executable | Arguments from | Notes |
|-----------|-----------|----------------|-------|
| `run_elevated()` (admin path) | `command[0]` | Caller-supplied list | Validated against `_ALLOWED_ELEVATED_EXECUTABLES` allowlist; raises `ValueError` if unknown |
| `run_elevated()` (non-admin path) | `powershell` | Hardcoded + `command[0]`/args | Only wraps an already-validated command; `Start-Process -Verb RunAs` |

**Allowlisted executables** (see `_ALLOWED_ELEVATED_EXECUTABLES`):
`reg`, `dism`, `icacls`, `takeown`, `netsh`, `sc`, `schtasks`, `taskkill`,
`wusa`, `powershell`, `pwsh`, `cmd`, `wmic`, `bcdedit`, `fsutil`, `net`, `netstat`
(and their `.exe` variants).

---

### `regilattice/registry.py`

| Call site | Executable | Arguments from | Notes |
|-----------|-----------|----------------|-------|
| `restore_backup()` | `reg import` | `str(reg_file)` — a `Path` from a trusted local backup directory | Path is never constructed from user text |
| `read_value()` (fallback) | `reg query` | Hardcoded key path split by `_split_root()` | Only reached when `winreg` is unavailable |
| `delete_tree()` (fallback) | `reg delete` | `f"{root}\\{subkey}"` — output of `_split_root()` | `_split_root()` validates hive prefix |
| `delete_value()` (fallback) | `reg delete` | Registry path from `_split_root()` + value name | Path validated; value name is caller-controlled internal data |

---

### `regilattice/corpguard.py`

| Call site | Executable | Arguments from | Notes |
|-----------|-----------|----------------|-------|
| `_check_aad_join()` | `dsregcmd /status` | Hardcoded | Read-only; no user input |
| `_check_vpn_adapter()` | `powershell Get-NetAdapter` | Hardcoded | Read-only |
| `_check_gpo_registry()` | `reg query` | Hardcoded key paths | Read-only |
| `_check_sccm_intune()` | `reg query` | Hardcoded key paths | Read-only |

---

### `regilattice/deps.py`

| Call site | Executable | Arguments from | Notes |
|-----------|-----------|----------------|-------|
| `install_package()` | `sys.executable -m pip install` | Package name from caller | Caller is internal code; package names are short identifiers validated by pip |

---

### `regilattice/gui_dialogs.py`

| Call site | Executable | Arguments from | Notes |
|-----------|-----------|----------------|-------|
| `open_scoop_manager()` | `scoop` | Hardcoded subcommand + package name from GUI list | Package names come from the curated `SCOOP_PACKAGES` constant, not user text |

---

### `regilattice/hwinfo.py`

| Call site | Executable | Arguments from | Notes |
|-----------|-----------|----------------|-------|
| `_wmi_query()` | `powershell Get-CimInstance` | Hardcoded WMI class name | Read-only; class name is a string literal |
| `_detect_gpu_wmi()` | `powershell Get-CimInstance Win32_VideoController` | Hardcoded | Read-only |

---

### `regilattice/tweaks/boot.py`

| Call site | Executable | Arguments from | Notes |
|-----------|-----------|----------------|-------|
| `_apply_boot_verbose()` | `bcdedit /set` | Hardcoded flag and value | Write; executed only when `require_admin=True` confirmed |
| `_remove_boot_verbose()` | `bcdedit /deletevalue` | Hardcoded flag | Write; admin-gated |
| `_detect_boot_verbose()` | `bcdedit /enum current` | Hardcoded | Read-only |
| `_apply_boot_pae()` | `bcdedit /set` | Hardcoded | Write; admin-gated |
| `_remove_boot_pae()` | `bcdedit /deletevalue` | Hardcoded | Write; admin-gated |
| `_detect_boot_pae()` | `bcdedit /enum current` | Hardcoded | Read-only |
| `_remove_boot_driver_verifier()` | `verifier /reset` | Hardcoded | Write; admin-gated |

---

## Security Properties

| Property | Status |
|----------|--------|
| No `shell=True` | ✅ All calls use list arguments |
| No user input in commands | ✅ All argument fragments come from internal constants or validated paths |
| Allowlist for elevated execution | ✅ `run_elevated()` checks `_ALLOWED_ELEVATED_EXECUTABLES` |
| Registry path validation | ✅ `validate_registry_path()` + `_split_root()` reject invalid hive prefixes |
| No outbound network calls | ✅ No `curl`, `wget`, or HTTP clients at runtime |
| Timeout on all long-running calls | ✅ `timeout=` set on all potentially blocking calls |
| Capture-output (no terminal hijack) | ✅ `capture_output=True` on all calls |

---

## Adding New Subprocess Calls

When adding a new `subprocess.run()` call:

1. Use a **list** of arguments — never `shell=True`.
2. Set `capture_output=True, text=True` and a reasonable `timeout`.
3. Ensure the executable name is a **hardcoded literal** or validated against an allowlist.
4. If the call performs a write and requires privileges, gate it behind `assert_admin(require_admin)`.
5. Update this document.
