---
mode: agent
description: "Review an open GitHub Pull Request: summarise changes, check against workspace coding standards, identify risks, and suggest actionable fixes."
---

# PR Review

Review the active or specified pull request against RegiLattice workspace standards.

## PR Identity

PR URL or number: `${input:PR URL or number (e.g. 42 or https://github.com/RajwanYair/RegiLattice/pull/42)}`

## Review Checklist

### Changed Files

Fetch the diff and list all changed files. For each:

1. **Build quality gate**
   - [ ] 0 build errors, 0 warnings (`TreatWarningsAsErrors=true`)
   - [ ] No `#pragma warning disable`, `[SuppressMessage]`, `// NOSONAR`, `// NCA`, `// NOLINT`
   - [ ] No `TODO`/`FIXME` comments (open GitHub Issues instead)

2. **C# conventions**
   - [ ] All new classes are `sealed` unless inheritance is explicitly needed
   - [ ] Public collections exposed as `IReadOnlyList<T>`, not `List<T>`
   - [ ] `#nullable enable` — all nullable returns annotated with `?`
   - [ ] No `dynamic`, no `Thread` (use `Task.Run`)
   - [ ] No hardcoded absolute paths (use `Environment.GetFolderPath`)
   - [ ] Registry access via `RegistrySession`, not raw `Registry.SetValue`

3. **Tests**
   - [ ] New/changed public API has corresponding xUnit tests
   - [ ] No `[Fact(Skip=...)]` / `[Theory(Skip=...)]` — fix tests, don't skip
   - [ ] DryRun mode used for any registry-touching tests
   - [ ] `Assert.NotNull(result)` before accessing `ParseArgs()` return
   - [ ] Performance budget tests include a tweak-count comment
   - [ ] No `DateTime.Now` used twice for generated output comparison (strip timestamps)

4. **Tweak additions** (if applicable)
   - [ ] All new IDs are globally unique (search all `Tweaks/*.cs`)
   - [ ] IDs follow `{category_slug}-{descriptive-name}` format
   - [ ] `ImpactScore` and `SafetyRating` set explicitly (1–5, not default 3/4)
   - [ ] `ApplyOps`, `RemoveOps`, and `DetectOps` all populated
   - [ ] No duplicate `PATH\ValueName` across modules (check `ConflictDetector`)

5. **Git / workflow**
   - [ ] Commit messages follow Conventional Commits (`type(scope): description`)
   - [ ] Issue referenced in footer (`Closes #N`)
   - [ ] Version bump included if MINOR/MAJOR change (all 4 properties in `Directory.Build.props`)

6. **Security (OWASP Top 10)**
   - [ ] No hardcoded credentials
   - [ ] No `Process.Start` with unsanitized input
   - [ ] No SQL or template injection vectors

## Output Format

Provide:
1. **Summary** — one paragraph describing what the PR changes
2. **Risk rating** — Low / Medium / High and justification
3. **Issues found** — numbered list with file + line reference
4. **Suggestions** — actionable fixes for each issue
5. **Verdict** — Approve / Request Changes / Comment
