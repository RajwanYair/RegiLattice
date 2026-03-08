---
name: Performance Issue
about: Report a performance problem (slow startup, slow detection, high memory, etc.)
title: "[Perf] "
labels: performance
assignees: ''
---

## Summary

Brief description of the performance issue.

## Component

Which operation is slow? (e.g., `all_tweaks()` load, `status_map()` detection, GUI startup, `--list`)

## Observed Performance

- **Operation:** (e.g., `status_map()` over 1 292 tweaks)
- **Time taken:** (e.g., 12 seconds)
- **Expected time:** (e.g., ~2 seconds)
- **Memory used:** (e.g., 150 MB peak)

## Profiling Data

<details>
<summary>Profile output (optional)</summary>

```
# Run: python -m cProfile -s cumtime -m regilattice --list
Paste cProfile / line_profiler output here
```

</details>

## Benchmark Result

```powershell
# Run: python -m pytest tests/test_benchmarks.py -v
Paste benchmark output here
```

## Environment

- **OS:** Windows 11 (build ...)
- **CPU:** (e.g., Intel Core i7-12700K)
- **RAM:** (e.g., 32 GB)
- **Python:** (`python -m regilattice --version` / `python --version`)
- **RegiLattice:** (version from `--version`)
- **Tweaks loaded:** (number from `--stats`)

## Additional Context

Any relevant flags used, specific category or tweak subset, etc.
