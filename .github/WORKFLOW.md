# TurboTweak — Development Workflow

## Branch Strategy

| Branch | Purpose |
|---|---|
| `main` | Stable, production-ready code |
| `feat/*` | New tweaks or features |
| `fix/*` | Bug fixes |

## Commit Convention

Use [Conventional Commits](https://www.conventionalcommits.org/):

```
feat: add WSL memory-limit optimisation tweak
fix: corp guard false positive on personal VPN
docs: update README with new tweak table
test: add detect_fn unit tests for privacy tweaks
refactor: extract plugin loader into tweaks/__init__
ci: add Python ruff+pytest workflow
```

## Adding a New Registry Tweak

1. **Identify the category** — pick an existing file in `turbotweak/tweaks/`
   or create a new one.

2. **Implement three functions:**
   ```python
   def apply_my_tweak(*, require_admin: bool = True) -> None: ...
   def remove_my_tweak(*, require_admin: bool = True) -> None: ...
   def detect_my_tweak() -> bool: ...
   ```

3. **Register as TweakDef** — append to the module's `TWEAKS` list:
   ```python
   TWEAKS: list[TweakDef] = [
       TweakDef(
           id="my-tweak",
           label="My New Tweak",
           category="Performance",
           apply_fn=apply_my_tweak,
           remove_fn=remove_my_tweak,
           detect_fn=detect_my_tweak,
           needs_admin=True,
           corp_safe=False,
           registry_keys=[r"HKLM\..."],
       ),
   ]
   ```

4. **Write tests** in `tests/test_tweaks_<category>.py`.

5. **The GUI, CLI, and PS menu auto-discover the new tweak.**

## Running Tests

```bash
# Python
pip install -e ".[dev]"
pytest -v
ruff check turbotweak/ tests/
mypy turbotweak/

# PowerShell (Pester)
Invoke-Pester tests/ -Verbose
```

## Pre-commit Checklist

- [ ] `ruff check` passes
- [ ] `pytest` passes
- [ ] `mypy --strict` passes
- [ ] New tweak has `detect_fn` for status reporting
- [ ] New tweak has `corp_safe` flag set correctly
- [ ] README tweak table updated (if user-visible)
- [ ] Git commit uses conventional format

## Release Process

1. Bump version in `pyproject.toml`.
2. Update CHANGELOG / README.
3. Tag: `git tag v1.x.x && git push --tags`.
