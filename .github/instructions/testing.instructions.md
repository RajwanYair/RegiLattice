---
applyTo: "**/tests/**,**/conftest.py,**/*_test.py,**/test_*.py"
---

# Testing Instructions

## Framework & Tools

- **Primary**: pytest 8.0+
- **Coverage**: pytest-cov, targeting 90%+
- **Timeouts**: pytest-timeout (default 30 s, configured in `pyproject.toml`)
- **Property-based**: hypothesis for complex logic
- **Mocking**: pytest-mock (prefer over unittest.mock directly)
- **Async**: pytest-asyncio for async code
- **Profiling**: pyinstrument or snakeviz+cProfile for slow test root-cause

## Running Tests — VS Code / GitHub Copilot

**Always use the VS Code `runTests` tool** (or the Testing panel) for running tests
in VS Code and GitHub Copilot agent sessions. This uses the configured Python
interpreter and pytest arguments from `.vscode/settings.json` and `pyproject.toml`.

```
# Correct in GitHub Copilot:  use the runTests tool
# Correct in terminal:
python -m pytest tests/                          # all tests
python -m pytest tests/test_gui.py -x            # single file, stop on first fail
python -m pytest tests/ --timeout=30            # explicit timeout
```

**Never** run `python -m pytest` inside a Copilot agent when the `runTests` tool is
available — terminal output is truncated at 16 KB and coverage floods the buffer.

## Test Structure

```
tests/
├── conftest.py           # Shared fixtures, markers
├── unit/                 # Pure Python, no I/O
│   ├── test_core.py
│   └── test_utils.py
├── integration/          # File/network/OS interaction
│   └── test_integration.py
└── fixtures/             # Test data files
```

## Naming Conventions

```python
# Functions: test_<what>_<when>_<expected>
def test_process_file_with_valid_input_returns_result():
    ...

def test_process_file_with_missing_file_raises_file_not_found_error():
    ...
```

## Fixtures Pattern

```python
import pytest
from pathlib import Path

@pytest.fixture
def temp_dir(tmp_path: Path) -> Path:
    """Provide a temporary directory cleaned up after the test."""
    return tmp_path

@pytest.fixture
def sample_config(temp_dir: Path) -> Path:
    """Create a minimal config file for testing."""
    config = temp_dir / "config.yaml"
    config.write_text("app:\n  debug: true\n")
    return config
```

## Markers — Always Mark Your Tests

```python
@pytest.mark.unit
def test_pure_logic():
    ...

@pytest.mark.integration
def test_with_filesystem(tmp_path):
    ...

@pytest.mark.slow
def test_large_dataset():
    ...

@pytest.mark.windows
def test_registry_operation():
    ...

@pytest.mark.network
def test_api_call():
    ...
```

## Coverage Rules

- Minimum: 80% overall, 90% for core modules
- Exclude: `__main__.py`, `**/gui*.py` display code, `TYPE_CHECKING` blocks
- Run: `pytest --cov=src --cov-report=term-missing --cov-report=html`

## Hypothesis — Property-Based Testing

```python
from hypothesis import given, settings, strategies as st

@given(st.text(min_size=1), st.integers(min_value=0))
@settings(max_examples=100)
def test_process_handles_any_valid_input(text: str, count: int) -> None:
    result = process(text, count)
    assert result is not None
```

## What NOT to Do in Tests

- Don't test implementation details — test behaviour
- Don't use `time.sleep()` — use mocks or events
- Don't leave temporary files — use `tmp_path` fixture
- Don't catch exceptions to silence test failures
- Don't use `assert` on mutable defaults
- **Don't run the actual GUI in tests** — always `withdraw()` windows and patch
  `_deferred_init` to block background tweak loading. If a test must interact
  with real Tkinter widgets, add `@pytest.mark.timeout(10)` and call
  `root.after(100, root.destroy)` to ensure the event loop exits.
- **Don't run tests via `python -m pytest` in Copilot agents** — use `runTests` tool.
