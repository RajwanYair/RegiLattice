---
mode: agent
description: "Perform a thorough code review of the selected file or changes"
---

# Code Review

Review the selected code against workspace standards.

## Target

File: `${file}`
Selection: `${selection}`

## Review Checklist

### Security (OWASP Top 10)

- [ ] No hardcoded credentials, tokens, or API keys
- [ ] No `Process.Start` with unsanitized user input (command injection)
- [ ] No SQL/template injection vectors
- [ ] Input validated at system boundaries
- [ ] Sensitive data not logged in plaintext
- [ ] No SSRF vectors (external URLs not constructed from user input)
- [ ] Registry access via `RegistrySession` only — no raw `Registry.SetValue`

### C# Quality

- [ ] `sealed` on all classes unless inheritance is explicit
- [ ] `#nullable enable` — no nullable warnings
- [ ] No bare `catch` clauses — use specific exception types
- [ ] `IReadOnlyList<T>` for all public collection properties
- [ ] `init` or `private set` on properties — no mutable public fields
- [ ] C# 13 collection expressions `[]` instead of `new List<T>()`
- [ ] No `Console.WriteLine` in library code

### Architecture

- [ ] Single responsibility — methods do one thing
- [ ] Immutable models (`sealed record` or `sealed class` with `required init`)
- [ ] Enums used for constants (not magic strings/numbers)
- [ ] Configuration loaded from JSON/env, not hardcoded
- [ ] P/Invoke minimized — only 2 allowed calls in codebase

### Testing

- [ ] New code has corresponding xUnit tests
- [ ] Error paths tested (not just happy path)
- [ ] `RegistrySession.DryRun = true` in all registry tests

## Output Format

Provide feedback as:
1. **Critical issues** (blockers) — security, data loss, crashes
2. **Standard issues** (should fix) — quality, maintainability
3. **Suggestions** (optional) — style, performance, clarity
