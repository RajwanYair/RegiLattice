# RegiLattice — Roadmap

> Living document — updated after every sprint. Moved from root to `docs/` 2026-03-09.
> Last updated: 2026-03-09 · v1.0.0 · 1 292 tweaks · 69 categories · ~17 633 tests

---

## Current State (as of 2026-03-09)

| Metric | Value |
|--------|-------|
| Tweaks | 1 292 across 69 categories |
| Tests | ~17 633 (all passing) |
| Python | 3.10 – 3.14 |
| Lint | ruff (E, F, W, I, UP, B, SIM, RUF) |
| Type check | mypy --strict |
| Coverage | ≥ 90 % (target) |
| GUI | tkinter + 4 themes (Catppuccin Mocha/Latte, Nord, Dracula) |
| Profiles | 5 (business, gaming, privacy, minimal, server) |
| Platforms | Windows (primary); Linux/WSL (partial) |
| Repo | [github.com/RajwanYair/RegiLattice](https://github.com/RajwanYair/RegiLattice) |

---

## Long-Term Vision

Make RegiLattice the **reference Windows registry tweak toolkit**:

- Zero-dependency runtime (stdlib-only at runtime)
- Production-grade packaging distributed via pip, winget, and scoop
- GUI that rivals dedicated system administration tools
- Full corporate-environment safety and detection
- Extensible plugin marketplace with sandboxing
- World-class CI/CD pipeline with cross-platform testing
- Comprehensive documentation and developer experience

---

## Sprints

### Sprint 1 — Foundation & Hygiene ✅ (2026-03-08)

| Theme | Goal | Status |
|-------|------|--------|
| Git hygiene | Fix lint, commit Sprint 8 pending changes | ✅ |
| Config cleanup | `.gitattributes`, formatter (ruff not black), remove `.flake8` | ✅ |
| CI | Consolidate duplicate workflows into `ci.yml` | ✅ |
| Docs | ROADMAP.md, move PROJECT_SPEC_PROMPT, update stale architecture stats | ✅ |

### Sprint 2 — Repo Cleanup & Publishing ✅ (2026-03-08)

| Theme | Goal | Status |
|-------|------|--------|
| GitHub registration | Create repo at RajwanYair/RegiLattice; push all history | ✅ |
| Metadata correctness | Fix `pyproject.toml` URLs + author (aeger → RajwanYair) | ✅ |
| Architecture docs | Fix stale ASCII diagram counts (1 228→1 292, 64→69) | ✅ |
| VS Code hygiene | Remove hardcoded user-specific paths from `settings.json` | ✅ |
| README | Add live CI badge linked to Actions | ✅ |
| Roadmap | Consolidate all plans into this single document | ✅ |

### Sprint 3 — Documentation & Developer Experience ✅ (2026-03-08, retroactively completed)

| # | Task | Priority |
|---|------|----------|
| 1 | Add `docs/Development.md` — local setup, Windows + WSL guide | P1 ✅ |
| 2 | Refresh `Contributing.md` with current workflow and PR checklist | P1 ✅ |
| 3 | Add `hypothesis` + `pytest-mock` to `[dev]` dependencies in `pyproject.toml` | P1 ✅ |
| 4 | Measure per-module coverage; document gaps in `docs/Coverage.md` | P1 ✅ |
| 5 | Add `__all__` to `registry.py`, `tweaks/__init__.py`, `config.py` | P2 ✅ |
| 6 | Review and update all `.github/issue_template/` files for accuracy | P2 ✅ |
| 7 | Add `Security.md` policy (responsible disclosure, supported versions) | P2 ✅ |

### Sprint 4 — Test Coverage Push (planned)

| # | Task | Priority |
|---|------|----------|
| 1 | Identify modules below 80 % coverage; add targeted tests | P1 |
| 2 | Add tests for `gui.py` (widget lifecycle, search, filter) with `unittest.mock` | P1 |
| 3 | Add tests for `menu.py` edge cases (empty list, invalid input, quit) | P1 |
| 4 | Add property-based tests for registry path splitting and normalization | P2 |
| 5 | Add mutation-style tests for tweak apply/remove idempotency | P2 |
| 6 | Add cross-platform stubs (Linux path handling for WSL) | P2 |
| 7 | Push overall coverage ≥ 95 % on all critical paths | P1 |

### Sprint 5 — Refactoring & Performance (planned)

| # | Task | Priority |
|---|------|----------|
| 1 | Dead-code audit — remove unused functions/imports | P1 |
| 2 | Refactor `tweaks/__init__.py` — split by concern into loader/profiles/engine | P2 |
| 3 | Profile startup time with `cProfile`; defer heavy imports | P2 |
| 4 | Add `typing.TYPE_CHECKING` guards for expensive platform imports | P2 |
| 5 | Add `__all__` to all public modules consistently | P2 |
| 6 | Harden `marketplace.py` — plugin signature verification | P1 |
| 7 | Audit all `subprocess` calls for command-injection safety (OWASP) | P1 |

### Sprint 6 — Production Readiness & Release (planned)

| # | Task | Priority |
|---|------|----------|
| 1 | Validate PyPI packaging (`hatch build`, `twine check`) | P1 |
| 2 | WSL-compatibility pass — ensure all cross-platform tests pass on Ubuntu | P1 |
| 3 | Verify pre-commit hooks work end-to-end (`pre-commit run --all-files`) | P1 |
| 4 | Add Codecov integration to CI workflow | P2 |
| 5 | Tag and publish v1.0.1 GitHub Release with generated notes | P1 |
| 6 | Update winget manifest (`winget/`) for v1.0.1 | P2 |
| 7 | Publish to Scoop bucket (personal) | P3 |

### Sprint 10 — Production Quality (2026-03-09, in progress)

| # | Task | Priority |
|---|------|----------|
| 1 | Fix ruff I001 import-sort in `test_tweaks_init.py` | P0 ✅ |
| 2 | Remove generic `.github/contributing.md` / `.github/security.md` (superseded by root) | P1 ✅ |
| 3 | Move `Roadmap.md` → `docs/Roadmap.md`; update all cross-references | P1 ✅ |
| 4 | Update `docs/Coverage.md` with current measured coverage numbers (2026-03-09) | P1 ✅ |
| 5 | Push `gui.py` coverage from 57 % toward ≥ 80 % | P1 |
| 6 | Push `gui_dialogs.py` coverage from 35 % toward ≥ 80 % | P1 |
| 7 | Audit all `subprocess` calls for command-injection safety (OWASP) | P1 |

---

## Prioritized Backlog

_50 concrete tasks derived from the roadmap above, ordered by priority._

### P0 — Completed ✅

- [x] Auto-fix ruff I001 import-sort issues
- [x] Commit Sprint 8 new tweaks and test changes
- [x] Fix `.gitattributes` header + add `*.py text eol=lf`
- [x] Fix `.vscode/settings.json` — use ruff, not black-formatter
- [x] Remove redundant `.flake8` config file
- [x] Add `[project.urls]` to `pyproject.toml`
- [x] Move generic spec → `.github/docs/project-spec.md`
- [x] Consolidate `ci.yml` workflow (removed duplicate `python.yml`)
- [x] Register repo at `RajwanYair/RegiLattice` and push full history
- [x] Fix `pyproject.toml` author + URLs (aeger → RajwanYair)
- [x] Fix architecture.md ASCII diagram stale counts
- [x] Remove hardcoded user paths from `.vscode/settings.json`
- [x] Add live CI badge to README

### P1 — High Value (Sprint 3–4)

- [x] Add `docs/Development.md` (setup, Windows + WSL, run, test, contribute)
- [x] Refresh `Contributing.md` with current PR/issue/commit workflow
- [x] Add `Security.md` with supported versions + disclosure policy
- [x] Add `hypothesis` + `pytest-mock` to `[project.optional-dependencies.dev]`
- [x] Measure per-module coverage; document gaps in `docs/Coverage.md`
- [ ] Push gui.py test coverage ≥ 80 %
- [ ] Add explicit `menu.py` edge-case tests
- [ ] Add `__all__` to `registry.py`, `tweaks/__init__.py`, `config.py`
- [ ] Audit all `subprocess` calls — no user-input shell injection possible
- [ ] Harden `marketplace.py` — verify plugin does not escape package dir

### P2 — Medium Value (Sprint 5–6)

- [ ] Review `.github/issue_template/` files for accuracy against v1.0.1 API
- [ ] Refactor `tweaks/__init__.py` — split loader/profiles/engine into sub-modules
- [ ] Profile `all_tweaks()` load time; document in benchmarks
- [ ] Add `typing.TYPE_CHECKING` guards for `winreg`, `ctypes.windll`
- [ ] Add `TweakDef.source_url` field for KB article references
- [ ] Add `Makefile` / `just` runner for common dev tasks (optional convenience)
- [ ] Codecov integration in CI — track coverage regressions on PRs
- [ ] Add GitHub Actions `powershell.yml` lint for `.psm1` / `.ps1`
- [ ] GUI: audit tkinter background thread interactions for race conditions
- [ ] Add CLI shell completion support (PowerShell + bash/zsh)

### P3 — Nice to Have / Long-term

- [ ] `src/` layout migration for PEP 517 packaging isolation
- [ ] Dark mode auto-detection for GUI (follow Windows system theme)
- [ ] `regilattice export --format json/ps1/reg` unified export command
- [ ] Plugin sandbox — limit filesystem/network access for third-party plugins
- [ ] Automated winget manifest PR via GitHub Actions on tag
- [ ] Add `logging` module integration to replace ad-hoc `SESSION.log`
- [ ] Localization: add German locale as proof-of-concept for i18n
- [ ] REST API layer (FastAPI) for remote management scenarios
- [ ] Web dashboard (read-only) for tweak status visualization
- [ ] Auto-generate per-category Markdown docs from `TweakDef` metadata
- [ ] `regilattice diff` — compare two snapshots side-by-side
- [ ] `regilattice doctor` — sanity-check for common misconfigurations
- [ ] Support user-defined registry tweaks via TOML (no Python required)
- [ ] Chocolatey package submission
- [ ] Benchmark suite in CI with regression detection

---

## Sprint Velocity Tracking

| Sprint | Dates | Tasks Planned | Tasks Done | Notes |
|--------|-------|--------------|-----------|-------|
| Sprint 1 | 2026-03-08 | 10 | 10 | Foundation & hygiene |
| Sprint 2 | 2026-03-08 | 6 | 6 | Repo publishing & metadata cleanup |
| Sprint 3 | 2026-03-08 | 7 | 7 | Documentation & developer experience (retroactively completed) |
| Sprint 4 | TBD | 7 | — | Test coverage push |
| Sprint 5 | TBD | 7 | — | Refactoring & performance |
| Sprint 6 | TBD | 7 | — | Production readiness & v1.0.1 release |
| Sprint 9 | 2026-03-09 | 8 | 8 | Profiling infra, MD Title Case rename, perf guide, PS profile cleanup |
| Sprint 10 | 2026-03-09 | 7 | — | Doc consolidation, coverage push, security audit |
