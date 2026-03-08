# UNIVERSAL PROJECT ENHANCEMENT SPECIFICATION

**Professional Software Development Methodology**
*Transform any script or application into a production-ready, enterprise-grade solution*

**Version: 13.0.0 — Complete Enhancement Framework**
**Updated: March 2026**

---

## TABLE OF CONTENTS

1. [What Is This Specification?](#what-is-this-specification)
2. [Core Philosophy & Principles](#core-philosophy--principles)
3. [Critical Success Patterns](#critical-success-patterns)
4. [Project Structure Template](#project-structure-template)
5. [Build System & Packaging](#build-system--packaging)
6. [Linting & Formatting](#linting--formatting)
7. [Type Checking](#type-checking)
8. [Testing Strategy](#testing-strategy)
9. [IDE & Editor Configuration](#ide--editor-configuration)
10. [Continuous Integration & Delivery](#continuous-integration--delivery)
11. [Source Control & Git Practices](#source-control--git-practices)
12. [Configuration Management](#configuration-management)
13. [Implementation Methodology](#implementation-methodology)
14. [Quality Assurance Checklist](#quality-assurance-checklist)

---

## WHAT IS THIS SPECIFICATION?

This specification is a **comprehensive, universal framework** for transforming
any software project into a production-ready, enterprise-grade solution ready
for GitHub publication. The methodology consolidates proven patterns for code
organisation, naming consistency, cross-platform compatibility, and professional
presentation based on real-world project transformations.

### Who Should Use This?

- **AI Assistants** — Apply this specification systematically to any project
  enhancement request. Follow the phases and checklists to ensure comprehensive
  coverage.
- **Developers** — Use this as a roadmap to professionalise your projects with
  industry best practices, enterprise features, and production-ready quality.
- **Organisations** — Implement this framework to standardise development
  practices and achieve consistent, high-quality software across all projects.

---

## CORE PHILOSOPHY & PRINCIPLES

### Design Principles

1. **Portability First** — No hardcoded paths (`Path(__file__).parent` only).
2. **Single Entry Point** — One executable with command routing.
3. **Multiple Interfaces** — CLI + GUI (+ Web GUI if needed) with feature parity.
4. **Configuration-Driven** — External config controls all customisable aspects.
5. **Zero Duplication** — One source of truth for every setting and constant.
6. **Signal-Safe** — Graceful shutdown with cleanup on SIGTERM/SIGINT.
7. **Security-First** — User-space execution; selective privilege escalation only.
8. **Graceful Degradation** — Provide fallback mechanisms when dependencies are
   unavailable.

### Quality Standards

- **Error-First Development** — Design error handling before happy paths.
- **Testing-Integrated** — Write tests alongside implementation, not as an
  afterthought.
- **Documentation-Driven** — Maintain docs at all levels (module, class,
  function, README, CONTRIBUTING, CHANGELOG).
- **Performance-Conscious** — Optimise for efficiency without sacrificing
  maintainability.
- **User-Centric** — Progressive disclosure; multiple interface options.

---

## CRITICAL SUCCESS PATTERNS

### 1. Portability

```python
# GOOD — portable, works from any directory
PROJECT_ROOT = Path(__file__).parent.resolve()
config = PROJECT_ROOT / "config" / "default.yaml"

# BAD — hardcoded, won't work for anyone else
config = "C:\\Users\\name\\project\\config.yaml"
```

Requirements:
- Use `Path(__file__).parent` everywhere.
- Generic placeholders in docs (`<username>`, not specific names).
- Relative references in help (`./README.md`).
- Works from any directory and any user account.

### 2. Single Entry Point Consolidation

Remove duplicate scripts. Merge all functionality into one main script.
Create clear command routing:

```python
def main() -> int:
    parser = argparse.ArgumentParser(prog="my-tool")
    sub = parser.add_subparsers(dest="command")
    sub.add_parser("apply", help="Apply configuration")
    sub.add_parser("remove", help="Remove configuration")
    sub.add_parser("list", help="List available items")
    sub.add_parser("gui", help="Launch graphical interface")
    args = parser.parse_args()
    # ... dispatch to appropriate handler
```

### 3. Interface Parity

All interfaces (CLI, Desktop GUI, Web GUI) share a **common backend service
layer**. Business logic lives in shared modules; interfaces are thin wrappers.

```
Desktop GUI (Tkinter) ──┐
Web GUI (FastAPI)  ──────┼──> Shared Backend Services
CLI (argparse)     ──────┘
```

### 4. Graceful Signal Handling

```python
import signal, atexit, sys

class GracefulShutdown:
    def __init__(self):
        self.is_shutting_down = False
        signal.signal(signal.SIGTERM, self._handle)
        signal.signal(signal.SIGINT, self._handle)
        atexit.register(self._cleanup)

    def _handle(self, signum, frame):
        if self.is_shutting_down:
            sys.exit(1)  # Force exit on second signal
        self.is_shutting_down = True
        self._cleanup()
        sys.exit(0)

    def _cleanup(self):
        # Close file handles, terminate threads, remove temp files,
        # flush log buffers, release locks, save state.
        ...
```

### 5. Progress Indicators

- **CLI**: Use `rich.progress` or `tqdm` for terminal progress bars.
- **GUI**: Use callback functions (`callback(current, total, message)`).
- Keep progress logic in the shared backend, not in the interface layer.

### 6. Package Management Strategy

Preference order:
1. System package managers (APT, yum, brew, choco).
2. Language-specific (pip, npm, cargo).
3. Universal (Snap, Flatpak, Conda).
4. Manual installation (last resort).

### 7. Proxy-Aware Networking

Never hard-code proxy URLs. Check in order:
1. Environment variables (`HTTP_PROXY`, `HTTPS_PROXY`, `NO_PROXY`).
2. Configuration file.
3. Direct connection (fallback).

---

## PROJECT STRUCTURE TEMPLATE

```
project-root/
├── pyproject.toml               # Build config, tool settings, metadata
├── pyrightconfig.json           # Pyright/Pylance type-check config
├── README.md                    # Main documentation
├── CONTRIBUTING.md              # Contribution guidelines
├── CHANGELOG.md                 # Release history (Keep a Changelog format)
├── LICENSE                      # License file (MIT recommended)
├── .gitignore                   # Git ignore rules
│
├── .github/
│   ├── copilot-instructions.md  # AI assistant context (auto-loaded)
│   └── workflows/
│       ├── ci.yml               # Lint + type-check + test matrix
│       └── release.yml          # Build + publish on tag push
│
├── .vscode/
│   ├── settings.json            # Workspace editor + linter + formatter config
│   ├── launch.json              # Debug configurations
│   ├── tasks.json               # Build/test/lint task shortcuts
│   └── extensions.json          # Recommended extensions list
│
├── src/ (or package_name/)      # Source code
│   ├── __init__.py              # Package version (__version__)
│   ├── __main__.py              # python -m entry point
│   ├── cli.py                   # argparse CLI
│   ├── gui.py                   # Tkinter GUI (or other framework)
│   ├── config.py                # Configuration loading (TOML/YAML/JSON)
│   ├── registry.py              # Core domain logic
│   └── plugins/                 # Auto-discovered plugin modules
│       ├── __init__.py          # Plugin loader
│       └── *.py                 # One module per plugin category
│
├── tests/
│   ├── conftest.py              # Shared fixtures (session, function scope)
│   ├── test_<module>.py         # Unit tests per source module
│   ├── test_integration.py      # End-to-end workflow tests
│   ├── test_property.py         # Hypothesis property-based tests
│   └── test_benchmarks.py       # Performance regression tests
│
└── docs/                        # Extended documentation (optional)
    ├── QUICK_START.md
    └── ARCHITECTURE.md
```

### Root Directory Rules

Keep the root clean. Only these files belong at top level:
- `pyproject.toml`, `pyrightconfig.json`, `README.md`, `CONTRIBUTING.md`,
  `CHANGELOG.md`, `LICENSE`, `.gitignore`, and directories (`.github/`,
  `.vscode/`, `src/`, `tests/`, `docs/`).
- No `.py` scripts at root (use `__main__.py` and `pyproject.toml [project.scripts]`).

---

## BUILD SYSTEM & PACKAGING

### pyproject.toml — Single Source of Truth

`pyproject.toml` is the **one file** for build config, project metadata, and
tool settings. Avoid maintaining separate `setup.py`, `setup.cfg`, `tox.ini`,
`.pylintrc`, or `mypy.ini` files — consolidate everything here.

```toml
[build-system]
requires = ["hatchling"]
build-backend = "hatchling.build"

[project]
name = "my-tool"
version = "1.0.0"
description = "One-line project description."
readme = "README.md"
license = { text = "MIT" }
requires-python = ">=3.10"
authors = [{ name = "Author Name" }]
keywords = ["keyword1", "keyword2"]
classifiers = [
    "Development Status :: 5 - Production/Stable",
    "Programming Language :: Python :: 3",
    "Programming Language :: Python :: 3.10",
    "Programming Language :: Python :: 3.11",
    "Programming Language :: Python :: 3.12",
    "Programming Language :: Python :: 3.13",
]

[project.scripts]
my-tool = "my_tool.cli:main"

[project.optional-dependencies]
dev = ["pytest>=8,<9", "pytest-cov>=7,<8", "mypy>=1.5,<2", "ruff>=0.4,<1"]
```

### Version Management

- Store version in `__init__.py` (`__version__ = "1.0.0"`) and mirror it in
  `pyproject.toml` `[project] version`.
- Use semantic versioning: `MAJOR.MINOR.PATCH`.
- Tag releases with `v` prefix: `v1.0.0`.

---

## LINTING & FORMATTING

### Ruff — Primary Linter & Formatter

Ruff replaces flake8, isort, pyflakes, pycodestyle, and most of pylint's
style checks. Configure in `pyproject.toml`:

```toml
[tool.ruff]
target-version = "py310"
line-length = 150

[tool.ruff.lint]
select = ["E", "F", "W", "I", "UP", "B", "SIM", "RUF"]
ignore = ["ARG002"]  # Document why each ignore is needed
```

**Rule set explanation**:
- `E` — pycodestyle errors
- `F` — pyflakes
- `W` — pycodestyle warnings
- `I` — isort import sorting
- `UP` — pyupgrade (modern Python syntax)
- `B` — bugbear (common pitfalls)
- `SIM` — simplify (code simplification)
- `RUF` — ruff-specific rules (Unicode confusables, etc.)

### Pylint — Supplementary Deep Analysis

Pylint catches issues ruff doesn't: naming conventions, design complexity
(too-many-branches, too-many-arguments), missing docstrings, and module
structure problems.

Configure in `pyproject.toml` under `[tool.pylint.*]`:

```toml
[tool.pylint."messages_control"]
# Document EVERY suppression with the reason it's needed.
# W0613 / unused-argument: require_admin kwarg is part of the TweakDef API
#   contract — must be present even when unused.
# C0301 / line-too-long: handled by ruff with line-length=150.
# C0415 / import-outside-toplevel: intentional lazy imports for fast startup.
disable = [
    "W0613", "unused-argument",
    "C0301", "line-too-long",
    "C0415", "import-outside-toplevel",
]

[tool.pylint.format]
max-line-length = 150  # Must match ruff line-length

[tool.pylint.design]
max-branches = 20     # CLI main() legitimately has many branches
max-statements = 80
max-args = 10
max-locals = 25
```

### Suppression Best Practices

1. **One source of truth** — Put all pylint suppressions in `pyproject.toml`.
   Do NOT duplicate in VS Code `settings.json` `pylint.args`. Instead, use
   `"pylint.args": ["--rcfile=pyproject.toml"]` in VS Code.
2. **Document every suppression** — Add a comment explaining WHY the rule is
   disabled, not just WHAT rule it is.
3. **Prefer solving over suppressing** — Before disabling a rule, attempt to
   fix the code. Only suppress when the rule conflicts with an architectural
   decision.
4. **Review periodically** — Re-enable suppressed rules when the underlying
   reason no longer applies.

---

## TYPE CHECKING

### Dual-Checker Strategy: mypy + Pyright/Pylance

Use **mypy** for CI strictness and **Pyright/Pylance** for real-time IDE
feedback. They complement each other: mypy is more conservative; Pyright has
better type narrowing and editor integration.

#### mypy Configuration

```toml
[tool.mypy]
python_version = "3.10"
strict = true
warn_unused_configs = true
```

#### Pyright/Pylance Configuration

Create `pyrightconfig.json` at the project root. This file takes precedence
over `[tool.pyright]` in `pyproject.toml` when both exist.

```jsonc
{
  "pythonVersion": "3.10",
  "typeCheckingMode": "standard",
  "extraPaths": ["."],

  // Document every suppression with the architectural reason.
  // mypy uses `# type: ignore`; Pyright doesn't always need them.
  "reportUnnecessaryTypeIgnoreComment": false,
  // Module sources unavailable in CI stubs (e.g., winreg on Linux).
  "reportMissingModuleSource": false,
  // API-contract parameters that are intentionally unused.
  "reportUnusedParameter": false,
  // GUI widget methods return values we intentionally discard.
  "reportUnusedCallResult": false
}
```

#### Handling Dual-Checker Conflicts

The most common conflict: mypy needs `# type: ignore[attr-defined]` for
dynamic attributes (e.g., `ctypes.windll`), but Pyright flags these as
"Unused type: ignore comment" because Pyright knows about the attribute.

**Resolution**: Set `reportUnnecessaryTypeIgnoreComment: false` in
`pyrightconfig.json`. This lets mypy's `# type: ignore` comments coexist
with Pyright without spurious warnings.

**Important**: `python.analysis.diagnosticSeverityOverrides` in VS Code
`settings.json` CANNOT be set when `pyrightconfig.json` exists. Put all
Pyright settings in `pyrightconfig.json` only.

#### Suppression Review Policy

Periodically review all `report*: false` settings:
- **Keep** suppressions for platform-specific attributes, test fixtures
  accessing private members, and decorator-based patterns.
- **Remove** overly broad suppressions (`reportReturnType`,
  `reportAssignmentType`, `reportCallIssue`) and fix the real type errors
  they were hiding.

---

## TESTING STRATEGY

### Framework: pytest

```toml
[tool.pytest.ini_options]
testpaths = ["tests"]
addopts = "-v --tb=short"

[tool.coverage.run]
source = ["my_tool"]
omit = ["my_tool/__main__.py"]
```

### Test File Organisation

| File | Purpose |
| ---- | ------- |
| `conftest.py` | Shared fixtures: session-scoped (e.g., `all_items_list`) and function-scoped (e.g., `dry_session` for safe testing). |
| `test_<module>.py` | Unit tests for each source module. |
| `test_smoke.py` | Auto-parametrised tests over ALL plugins/items (signature checks, ID uniqueness, callability). |
| `test_integration.py` | End-to-end workflow tests (CLI commands, batch operations). |
| `test_property.py` | Hypothesis property-based tests for invariant checking. |
| `test_benchmarks.py` | Performance regression tests with assertions on execution time. |

### Key Testing Patterns

**Auto-parametrised smoke tests** — If your project has a plugin/tweak/module
system, auto-discover all items and parametrise tests over them. Adding a new
plugin automatically generates tests:

```python
@pytest.fixture(scope="session")
def all_items():
    return load_all_plugins()

@pytest.mark.parametrize("item", all_items(), ids=lambda t: t.id)
def test_has_valid_signature(item):
    sig = inspect.signature(item.apply_fn)
    assert "require_admin" in sig.parameters
```

**Dry-run fixtures** — For tests that touch external resources (registry,
filesystem, network), provide a fixture that sets a dry-run flag:

```python
@pytest.fixture
def dry_session(monkeypatch):
    session = MySession()
    monkeypatch.setattr(session, "_dry_run", True)
    return session
```

**Coverage target**: 90%+ line coverage. Use `pragma: no cover` sparingly and
only for genuinely untestable code (entry-point guards, platform-specific
branches that can't run in CI).

---

## IDE & EDITOR CONFIGURATION

### VS Code Workspace Settings

Store IDE configuration in `.vscode/` (tracked in Git). This ensures every
contributor has the same experience.

#### `.vscode/settings.json`

```jsonc
{
  // ── Python ─────────────────────────────────────────────────────────
  "python.defaultInterpreterPath": "<path-to-python>",
  "python.analysis.autoImportCompletions": true,
  "python.testing.pytestEnabled": true,
  "python.testing.pytestArgs": ["tests", "-v", "--tb=short"],

  // ── Ruff (linter + formatter) ──────────────────────────────────────
  "ruff.lint.args": ["--config=pyproject.toml"],
  "[python]": {
    "editor.defaultFormatter": "charliermarsh.ruff",
    "editor.formatOnSave": true,
    "editor.codeActionsOnSave": {
      "source.fixAll.ruff": "explicit",
      "source.organizeImports.ruff": "explicit"
    }
  },

  // ── Editor defaults ────────────────────────────────────────────────
  "editor.tabSize": 4,
  "editor.insertSpaces": true,
  "editor.rulers": [150],
  "editor.renderWhitespace": "boundary",
  "editor.bracketPairColorization.enabled": true,
  "editor.minimap.enabled": false,
  "editor.stickyScroll.enabled": true,

  // ── Files ──────────────────────────────────────────────────────────
  "files.trimTrailingWhitespace": true,
  "files.insertFinalNewline": true,
  "files.trimFinalNewlines": true,
  "files.eol": "\n",
  "files.exclude": {
    "**/__pycache__": true,
    "**/*.pyc": true,
    "**/.mypy_cache": true,
    "**/.pytest_cache": true,
    "**/.ruff_cache": true,
    "**/dist": true,
    "**/build": true,
    "**/*.egg-info": true
  },

  // ── Search ─────────────────────────────────────────────────────────
  "search.exclude": {
    "**/__pycache__": true,
    "**/.mypy_cache": true,
    "**/.pytest_cache": true,
    "**/dist": true,
    "**/build": true
  },

  // ── Terminal ───────────────────────────────────────────────────────
  "terminal.integrated.env.windows": {
    "PYTHONDONTWRITEBYTECODE": "1",
    "PYTHONPATH": "${workspaceFolder}",
    "PYTHONIOENCODING": "utf-8"
  },

  // ── Git ────────────────────────────────────────────────────────────
  "git.autofetch": true,
  "git.confirmSync": false,
  "git.enableSmartCommit": true,

  // ── Markdown ───────────────────────────────────────────────────────
  "markdownlint.config": {
    "MD013": false,
    "MD033": false,
    "MD041": false
  },

  // ── Pylint ─────────────────────────────────────────────────────────
  // Single source of truth: pyproject.toml. Do NOT duplicate disable list here.
  "pylint.args": ["--rcfile=pyproject.toml"],

  // ── YAML ───────────────────────────────────────────────────────────
  // GitHub Actions validation requires network access to resolve `uses:` refs.
  // Behind corporate proxies this fails with "Unable to resolve action".
  "yaml.schemas": {
    "https://json.schemastore.org/github-workflow.json": ".github/workflows/*.yml"
  }
}
```

#### `.vscode/tasks.json`

Define tasks for every common operation so developers can use `Ctrl+Shift+B`:

```jsonc
{
  "version": "2.0.0",
  "tasks": [
    {
      "label": "Lint (Ruff)",
      "type": "shell",
      "command": "python -m ruff check src/ tests/",
      "group": "build"
    },
    {
      "label": "Format (Ruff)",
      "type": "shell",
      "command": "python -m ruff format src/ tests/",
      "group": "build"
    },
    {
      "label": "Type Check (mypy)",
      "type": "shell",
      "command": "python -m mypy src/",
      "group": "build"
    },
    {
      "label": "Test (pytest)",
      "type": "shell",
      "command": "python -m pytest tests/ -v --tb=short",
      "group": { "kind": "test", "isDefault": true }
    },
    {
      "label": "Test + Coverage",
      "type": "shell",
      "command": "python -m pytest tests/ -v --tb=short --cov=src --cov-report=term-missing",
      "group": "test"
    },
    {
      "label": "Install (editable)",
      "type": "shell",
      "command": "pip install -e .[dev]",
      "group": "build"
    }
  ]
}
```

#### `.vscode/launch.json`

Debug configurations for common entry points:

```jsonc
{
  "version": "0.2.0",
  "configurations": [
    {
      "name": "CLI (list)",
      "type": "debugpy",
      "request": "launch",
      "module": "my_tool",
      "args": ["--list"],
      "console": "integratedTerminal"
    },
    {
      "name": "GUI",
      "type": "debugpy",
      "request": "launch",
      "module": "my_tool",
      "args": ["--gui"],
      "console": "integratedTerminal"
    },
    {
      "name": "pytest",
      "type": "debugpy",
      "request": "launch",
      "module": "pytest",
      "args": ["tests/", "-v", "--tb=short"],
      "console": "integratedTerminal",
      "justMyCode": false
    }
  ]
}
```

#### `.vscode/extensions.json`

Recommend extensions so contributors get the right tooling on first open:

```jsonc
{
  "recommendations": [
    "ms-python.python",        // Python language support
    "ms-python.debugpy",       // Python debugger
    "charliermarsh.ruff",      // Ruff linter + formatter
    "tamasfe.even-better-toml",// TOML syntax
    "redhat.vscode-yaml",      // YAML syntax + validation
    "esbenp.prettier-vscode",  // Markdown / JSON formatter
    "eamodio.gitlens",         // Git blame + history
    "usernamehw.errorlens",    // Inline error display
    "gruntfuggly.todo-tree"    // TODO/FIXME scanner
  ]
}
```

### AI Assistant Context: `.github/copilot-instructions.md`

Create a `copilot-instructions.md` in `.github/` that GitHub Copilot auto-loads
on every chat session. Include:

- **Quick Facts** — Language, build tool, line length, test command, Python path.
- **Architecture at a Glance** — File tree with one-line descriptions.
- **Key Patterns** — Plugin loader, function signatures, API contracts.
- **Common Pitfalls** — Duplicate IDs, Unicode confusables, path conventions.
- **Checklist for Adding Items** — Step-by-step contributor guide.

This file is the fastest path from "new contributor" to "productive contributor".

---

## CONTINUOUS INTEGRATION & DELIVERY

### GitHub Actions: CI Workflow

```yaml
name: CI
on:
  push:
    branches: [main]
  pull_request:
    branches: [main]

permissions:
  contents: read

jobs:
  lint-and-test:
    runs-on: windows-latest  # or ubuntu-latest
    strategy:
      matrix:
        python-version: ["3.10", "3.11", "3.12", "3.13"]
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-python@v5
        with:
          python-version: ${{ matrix.python-version }}
          allow-prereleases: true
      - name: Install dependencies
        run: pip install -e ".[dev]"
      - name: Lint (ruff)
        run: python -m ruff check src/ tests/
      - name: Type check (mypy)
        run: python -m mypy src/
      - name: Test (pytest + coverage)
        run: >-
          python -m pytest tests/ -x --tb=short
          --cov=src --cov-report=term-missing --cov-report=xml
      - name: Upload coverage artifact
        if: matrix.python-version == '3.12'
        uses: actions/upload-artifact@v4
        with:
          name: coverage-report
          path: coverage.xml
```

### GitHub Actions: Release Workflow

```yaml
name: Release
on:
  push:
    tags: ["v*"]

permissions:
  contents: write

jobs:
  build-and-release:
    runs-on: windows-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-python@v5
        with:
          python-version: "3.12"
      - name: Build sdist and wheel
        run: |
          pip install build
          python -m build
      - name: Create GitHub Release
        uses: softprops/action-gh-release@v2
        with:
          files: dist/*
          generate_release_notes: true
```

### CI Best Practices

- **Matrix testing** — Test against all supported Python versions.
- **Fail fast** — Use `-x` flag in pytest to stop on first failure.
- **Permissions** — Use minimal permissions (`contents: read` for CI,
  `contents: write` only for releases).
- **Coverage artifacts** — Upload coverage reports for one Python version
  to avoid redundant uploads.
- **YAML validation note** — The `uses:` directives in workflow files require
  network access to resolve. Behind corporate firewalls, the VS Code YAML
  extension may show "Unable to resolve action" warnings. This is a network
  issue, not a code problem. Configure `yaml.schemas` in `settings.json` to
  use the GitHub workflow JSON schema from the schema store.

---

## SOURCE CONTROL & GIT PRACTICES

### .gitignore — Comprehensive Template

Organise by category with section headers:

```gitignore
# ── Build & packaging ────────────────────────────────────────
dist/
build/
*.egg-info/
*.whl

# ── Compiled / binary ────────────────────────────────────────
__pycache__/
*.py[cod]
*$py.class
*.pyd
*.so

# ── Temp files ────────────────────────────────────────────────
*.tmp
*.bak
*.swp
*~

# ── IDE / editor ──────────────────────────────────────────────
# NOTE: .vscode/ is intentionally NOT ignored — it contains
# shared workspace settings that all contributors should use.
.idea/
*.code-workspace

# ── Logs ──────────────────────────────────────────────────────
*.log
logs/

# ── OS cruft ──────────────────────────────────────────────────
Thumbs.db
.DS_Store
desktop.ini

# ── Test & coverage artefacts ─────────────────────────────────
.pytest_cache/
.coverage
htmlcov/
coverage.xml

# ── Type checker caches ──────────────────────────────────────
.mypy_cache/
.pyright/

# ── Linter caches ────────────────────────────────────────────
.ruff_cache/
```

### .gitignore Key Decisions

| Item | In `.gitignore`? | Reason |
| ---- | ----------------- | ------ |
| `.vscode/` | **No** | Shared workspace settings benefit all contributors. |
| `__pycache__/` | Yes | Generated bytecode; never commit. |
| `.mypy_cache/` | Yes | Type-checker cache; machine-specific. |
| `dist/`, `build/` | Yes | Build artefacts; reproduced by `python -m build`. |
| `*.egg-info/` | Yes | Editable install metadata. |
| `.env` files | Yes | May contain secrets. |

### Verifying No Tracked Ignored Files

After updating `.gitignore`, verify no tracked files should be ignored:

```bash
git ls-files -ic --exclude-standard
```

If this outputs any files, remove them from tracking:

```bash
git rm --cached <file>
```

### Commit Messages

Use imperative mood, concise subject lines:

- `Add bluetooth-disable-handsfree tweak`
- `Fix detect_fn for explorer-show-extensions`
- `Update Privacy category descriptions`

### Branching

- `main` — Production-ready code. Protected branch.
- Feature branches — `feature/<description>` or `fix/<description>`.
- Release tags — `v1.0.0` format, triggers release workflow.

---

## CONFIGURATION MANAGEMENT

### Hierarchy (highest to lowest precedence)

1. Command-line arguments
2. Environment variables
3. User configuration file (`~/.myapp.toml`)
4. System/project configuration file
5. Hardcoded defaults

### User Configuration Pattern

```python
@dataclass
class AppConfig:
    """User preferences loaded from ~/.myapp.toml."""
    theme: str = "default"
    log_level: str = "INFO"
    auto_backup: bool = True

def load_config() -> AppConfig:
    path = Path.home() / ".myapp.toml"
    if not path.exists():
        return AppConfig()
    data = tomllib.loads(path.read_text(encoding="utf-8"))
    return AppConfig(**{k: v for k, v in data.items() if k in AppConfig.__dataclass_fields__})
```

### External Tool Configurations in pyproject.toml

All tool settings should live in `pyproject.toml` to maintain a single source
of truth:

```toml
# Build
[build-system]
requires = ["hatchling"]
build-backend = "hatchling.build"

# Ruff
[tool.ruff]
target-version = "py310"
line-length = 150

[tool.ruff.lint]
select = ["E", "F", "W", "I", "UP", "B", "SIM", "RUF"]
ignore = ["ARG002"]

# mypy
[tool.mypy]
python_version = "3.10"
strict = true

# Pyright (sync with pyrightconfig.json)
[tool.pyright]
pythonVersion = "3.10"
typeCheckingMode = "standard"

# pytest
[tool.pytest.ini_options]
testpaths = ["tests"]
addopts = "-v --tb=short"

# Coverage
[tool.coverage.run]
source = ["my_tool"]
omit = ["my_tool/__main__.py"]

# Pylint
[tool.pylint."messages_control"]
disable = ["W0613", "C0301"]

[tool.pylint.format]
max-line-length = 150

[tool.pylint.design]
max-branches = 20
```

**Exception**: `pyrightconfig.json` must be a separate file because
Pyright/Pylance prioritises it over `[tool.pyright]` in `pyproject.toml`, and
it supports JSONC comments for documenting each suppression.

---

## IMPLEMENTATION METHODOLOGY

### Phase 1: Foundation

- Project structure, `pyproject.toml`, entry point, configuration loading.
- `.gitignore`, `README.md`, `LICENSE`.
- Logging framework, error handling patterns.
- CI pipeline (GitHub Actions).

### Phase 2: Core Logic

- Domain-specific business logic in shared backend modules.
- Plugin/module auto-discovery if applicable.
- CLI interface with argparse.

### Phase 3: Interfaces

- Desktop GUI (Tkinter or other framework).
- Feature parity with CLI.
- Web GUI if needed.

### Phase 4: Testing & Quality

- Unit tests, integration tests, smoke tests.
- Property-based tests (Hypothesis).
- 90%+ coverage target.
- Linting clean (ruff + pylint), type-check clean (mypy + Pyright).

### Phase 5: Production Prep

- `CHANGELOG.md` populated.
- `CONTRIBUTING.md` with setup instructions.
- `.github/copilot-instructions.md` for AI assistant context.
- VS Code workspace fully configured.
- Final portability check (no hardcoded paths).
- Version consistency across `__init__.py`, `pyproject.toml`, `CHANGELOG.md`.

---

## QUALITY ASSURANCE CHECKLIST

### Code Quality

- [ ] Single entry point, no duplicate scripts at root.
- [ ] All paths portable (`Path(__file__).parent`, no hardcoded paths).
- [ ] Clean root directory (only config files and directories).
- [ ] Version consistent across `__init__.py`, `pyproject.toml`, `CHANGELOG.md`.
- [ ] No `# type: ignore` without a specific error code (e.g., `[attr-defined]`).
- [ ] No bare `except:` — always catch specific exceptions.
- [ ] All public functions and classes have docstrings.
- [ ] Module-level docstrings on all source files.

### Linting & Type Checking

- [ ] `ruff check` passes with zero errors.
- [ ] `mypy --strict` passes with zero errors.
- [ ] Pyright/Pylance shows zero errors in VS Code problems pane.
- [ ] All pylint suppressions documented with reasons in `pyproject.toml`.
- [ ] All pyrightconfig.json suppressions documented with JSONC comments.
- [ ] No suppression without a clear architectural justification.

### Testing

- [ ] `pytest` passes all tests.
- [ ] Coverage >= 90%.
- [ ] Smoke tests auto-parametrised over all plugins/modules.
- [ ] Integration tests cover end-to-end workflows.
- [ ] Property-based tests validate invariants.
- [ ] Performance benchmarks prevent regressions.

### IDE Configuration

- [ ] `.vscode/settings.json` configures formatter, linter, test runner.
- [ ] `.vscode/tasks.json` provides lint/format/test/install shortcuts.
- [ ] `.vscode/launch.json` provides debug configurations.
- [ ] `.vscode/extensions.json` lists recommended extensions.
- [ ] Pylint args in VS Code point to `pyproject.toml` (single source of truth).
- [ ] `pyrightconfig.json` at project root with documented suppressions.

### CI/CD

- [ ] GitHub Actions CI runs lint + type-check + test on every push/PR.
- [ ] Matrix testing covers all supported Python versions.
- [ ] Release workflow builds and publishes on tag push.
- [ ] Minimal permissions (`contents: read` for CI).

### Documentation

- [ ] `README.md` with installation, usage, and architecture overview.
- [ ] `CONTRIBUTING.md` with setup, workflow, and coding standards.
- [ ] `CHANGELOG.md` with all released versions (Keep a Changelog format).
- [ ] `LICENSE` file present.
- [ ] `.github/copilot-instructions.md` with project context for AI assistants.

### Source Control

- [ ] `.gitignore` covers all build artefacts, caches, and OS-specific files.
- [ ] `.vscode/` is tracked (shared workspace settings).
- [ ] No tracked files that should be ignored (`git ls-files -ic --exclude-standard` is empty).
- [ ] Commit messages use imperative mood.
- [ ] Release tags follow `v{MAJOR}.{MINOR}.{PATCH}` format.
- [ ] Consistent naming
- [ ] No hardcoded values

#### Functionality

- [ ] Cross-platform tested
- [ ] Config loading works
- [ ] Error handling comprehensive
- [ ] Signal handling functional
- [ ] Progress tracking working

#### Interfaces

- [ ] Desktop GUI launches
- [ ] Web GUI accessible
- [ ] CLI fully functional
- [ ] Feature parity verified
- [ ] Config UI in both GUIs

#### Portability

- [ ] No hardcoded paths in code
- [ ] Generic placeholders in docs
- [ ] Relative paths in help
- [ ] Works from any directory
- [ ] Tested on multiple platforms

#### Documentation

- [ ] README comprehensive
- [ ] QUICK-START complete
- [ ] All docs use portable paths
- [ ] Help system complete
- [ ] Examples tested

#### Security

- [ ] No hardcoded credentials/proxies
- [ ] Proxy cleanup after use
- [ ] Graceful shutdown
- [ ] Audit logging (if needed)

---

## SUCCESS METRICS & VALIDATION

| Metric | Target |
|--------|--------|
| Code | Clean, modular, documented, portable |
| Tests | 90%+ coverage, all platforms |
| Performance | Sub-second response |
| Security | Zero critical vulnerabilities |
| Portability | 100% compatible, no hardcoded paths |
| Docs | Complete, accurate, portable |

---

## ADAPTATION GUIDELINES

### For Different Project Types

| Type | Focus Areas |
|------|-------------|
| CLI Tools | Rich CLI, config files, shell completion |
| Web Apps | API design, authentication, database |
| Desktop Apps | Cross-platform GUI, local storage, auto-update |
| System Admin | Security, privilege management, audit logging |
| Data Processing | Performance, batch processing, progress tracking |
| Enterprise | Security, compliance, centralized config |

### For Different Technology Stacks

| Stack | Equivalent Components |
|-------|----------------------|
| Python | argparse, pytest, PyYAML, tqdm, rich |
| Node.js | commander, jest, js-yaml, ora |
| Java | Spring Boot, JUnit, SnakeYAML |
| Go | Cobra, testing, gopkg.in/yaml |

### Common Pitfalls to Avoid

**Don't**:
- Hardcode absolute paths
- Use specific usernames in docs
- Skip signal handlers
- Miss GUI/CLI feature parity
- Hardcode proxy configs
- Leave dev artifacts

**Do**:
- Relative paths everywhere
- Generic placeholders
- Comprehensive signals
- Verify feature parity
- Config-driven proxy
- Clean structure

---

## CONCLUSION

This Universal Project Enhancement Specification represents proven methodologies for transforming any project into a production-ready, enterprise-grade solution. Apply this specification systematically to transform any project into a professional, production-ready application with enterprise-grade quality, security, and operational excellence.

---

**Framework Status**: ✅ Production Ready
**Validation**: ✅ Multi-Project Tested
**Applicability**: ✅ Universal
**Version**: 12.0.0
**Updated**: January 2026
**License**: MIT
