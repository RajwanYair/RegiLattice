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

Which operation is slow? (e.g., `AllTweaks()` load, `StatusMap()` detection, GUI startup, `--list`)

## Observed Performance

- **Operation:** (e.g., `StatusMap()` over 1 360 tweaks)
- **Time taken:** (e.g., 12 seconds)
- **Expected time:** (e.g., ~2 seconds)
- **Memory used:** (e.g., 150 MB peak)

## Profiling Data

<details>
<summary>Profile output (optional)</summary>

```
# Use dotnet-trace or Visual Studio profiler
Paste profiler output here
```

</details>

## Benchmark Result

```powershell
# Run: dotnet test --filter "Category=Performance"
Paste benchmark output here
```

## Environment

- **OS:** Windows 11 (build ...)
- **CPU:** (e.g., Intel Core i7-12700K)
- **RAM:** (e.g., 32 GB)
- **Runtime:** .NET 10.0
- **RegiLattice:** (version 3.x.x)
- **Tweaks loaded:** (number from `--stats`)

## Additional Context

Any relevant flags used, specific category or tweak subset, etc.
