# Troubleshooting Guide

## Common Errors

### `AdminRequirementError: This operation requires elevated privileges`

**Cause:** The tweak modifies `HKEY_LOCAL_MACHINE` keys which require administrator access.

**Fix:**
- Right-click your terminal → **Run as administrator**, then re-run the command.
- Or use the GUI (`--gui`) which auto-elevates via UAC.
- If running from VS Code, restart VS Code as administrator.

### `CorporateNetworkError: Corporate environment detected`

**Cause:** RegiLattice detected a domain-joined, Azure AD, VPN, or managed machine.

**Fix:**
- Use `--force` to bypass the safety check (at your own risk).
- Or select only `corp_safe=True` tweaks (shown as green USER badges in GUI).
- In GUI, check the "Force" checkbox.

### `WinError 5: Access is denied`

**Cause:** The registry key is protected by ACLs or Group Policy.

**Fix:**
- Ensure you're running as administrator.
- Some keys are locked by Group Policy — contact IT or use `gpedit.msc`.
- Corporate-managed machines may block certain keys entirely.

### `WinError 6: The handle is invalid`

**Cause:** The registry operation failed due to a stale or invalid handle. This can happen in automated test environments or when the registry is being actively modified by another process.

**Fix:**
- Retry the operation.
- If in tests, ensure the `dry_session` fixture is active.

### `ValueError: Duplicate TweakDef id`

**Cause:** Two tweak modules define the same `id` value.

**Fix:**
- Search for the duplicate: `grep -r "id=\"<the-id>\"" regilattice/tweaks/`
- Rename one of the duplicates to be unique.

### `ImportError: No module named 'winreg'`

**Cause:** Running on a non-Windows platform.

**Fix:**
- RegiLattice requires Windows. Registry operations are not available on Linux/macOS.
- Dry-run mode (`--dry-run`) works on any platform for testing.

## GUI Issues

### GUI window is blank / doesn't load categories

**Cause:** Deferred loading may be blocked by a slow corporate network check.

**Fix:**
- Wait a few seconds — categories load in batches of 4.
- If it persists, check for Python errors in the terminal.

### System tray icon doesn't appear

**Cause:** `pystray` and/or `Pillow` are not installed (optional dependencies).

**Fix:**
```bash
pip install pystray Pillow
```

### Theme looks wrong / colours are off

**Cause:** Some terminal emulators don't support full ANSI colours.

**Fix:**
- Use Windows Terminal (recommended) for full colour support.
- In the GUI, try switching themes via the theme selector dropdown.

## CLI Issues

### `--snapshot-diff` shows no colours

**Cause:** ANSI colour codes are not supported in your terminal.

**Fix:**
- Use Windows Terminal, PowerShell 7+, or VS Code integrated terminal.
- Use `--html` to generate an HTML report instead.

### `--profile` doesn't apply all expected tweaks

**Cause:** Some tweaks may be skipped due to corporate guard, build version, or admin requirements.

**Fix:**
- Check the output for `skipped (corp)`, `skipped (build)`, or `skipped (admin)` entries.
- Use `--force` to bypass corporate safety.
- Run as administrator for HKLM tweaks.

## Development Issues

### Tests fail with `hypothesis.errors.FailedHealthCheck`

**Cause:** Hypothesis property-based tests may be slow on first run.

**Fix:**
- Run again — hypothesis caches examples after the first run.
- Increase the deadline: `@settings(deadline=10000)`

### `ruff` reports line-length errors

**Cause:** The project uses a 150-character line limit.

**Fix:**
- Check `.vscode/settings.json` and `pyproject.toml` both set `line-length = 150`.
- Split long strings/expressions across multiple lines.

### `mypy` strict mode failures

**Cause:** Type annotations are required in strict mode.

**Fix:**
- Add return type annotations to all functions.
- Add type annotations to all parameters.
- Use `Callable[..., None]` for tweak functions.
