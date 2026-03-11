---
mode: agent
description: "Diagnose and fix build errors, test failures, or runtime issues in RegiLattice"
tools: ["read_file", "replace_string_in_file", "grep_search", "semantic_search", "runTests", "get_errors", "run_in_terminal"]
---

# Debug & Fix — RegiLattice Skill

You are an expert at diagnosing and fixing issues in the RegiLattice C# codebase.

## Diagnostic Process

1. **Identify** the error type: build error, test failure, runtime exception, or warning
2. **Read** the affected file(s) and understand the context
3. **Search** for related code patterns that might be involved
4. **Fix** the root cause (not symptoms)
5. **Verify** with `dotnet build` and `dotnet test`

## Common Issues & Solutions

### Duplicate Tweak ID
- **Error**: `InvalidOperationException: Duplicate tweak ID: xxx`
- **Fix**: Search all `Tweaks/*.cs` for the ID, rename the duplicate

### Nullable Reference Warning
- **Warning**: `CS8602: Dereference of a possibly null reference`
- **Fix**: Add null check or use `?.` / `??` operator

### Sealed Class Missing
- **Warning**: Class not marked sealed
- **Fix**: Add `sealed` keyword unless inheritance is needed

### Registry Path Error
- **Error**: Path missing hive prefix
- **Fix**: Ensure path starts with `HKEY_LOCAL_MACHINE\` or `HKEY_CURRENT_USER\`

### WinForms Test Hang
- **Symptom**: Test times out
- **Fix**: Never create Form instances in tests — test data/logic layer only

## Build & Test Commands (PowerShell only)

```powershell
dotnet build RegiLattice.sln -c Debug
dotnet test --logger "console;verbosity=normal"
dotnet test --filter "FullyQualifiedName~TweakDef"
```

## Rules

- All terminal commands must be PowerShell — no Unix/bash
- Always use `RegistrySession { DryRun = true }` in tests
- Follow Conventional Commits for any fix commits
