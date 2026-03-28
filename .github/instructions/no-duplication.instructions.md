---
applyTo: "**/*.cs,**/Tweaks/**,**/tests/**"
---

# No-Duplication Rules — RegiLattice

> Prevention is cheaper than remediation.
> Apply these rules when adding tweaks, reviewing code, or before every commit.
>
> **Full 4-layer rules, PowerShell audit commands, and step-by-step workflow:**
> see `.github/skills/no-duplication/SKILL.md`

## The 4 Duplication Layers (summary)

| Layer | Severity | Auto-detected by |
|---|---|---|
| 1 — Duplicate ID | **HARD BLOCK** | `TweakEngine.Register()` + `RegisterBuiltins_AllIdsUnique` test |
| 2 — Duplicate registry op (same `PATH\ValueName`) | **Warning** | `TweakValidator.DetectDuplicateRegistryOps()` + builtins threshold test |
| 3 — Duplicate label (same human name in 2+ modules) | **Smell** | Label scan + `RegisterBuiltins_NoCrossModuleLabelAndPathCollision` test |
| 4 — Conceptual duplicate (same outcome, different code path) | **Debt** | Manual review + `Audit-Duplications.ps1` |

---

## Pre-Commit Duplication Checklist

Before committing any change to `src/RegiLattice.Core/Tweaks/`:

```
☐ Searched for the new tweak ID in all Tweaks/*.cs files — no match found
☐ Searched for the registry PATH\ValueName in all Tweaks/*.cs files — no match found
☐ Checked that the Label is unique OR that any collision is intentional (same label, different mechanism)
☐ Ran: dotnet build (0 errors — RegisterBuiltins would throw on duplicate IDs)
☐ Ran: dotnet test tests/RegiLattice.Core.Tests — all duplication guard tests green
```

---

## Running the Full Audit

For a comprehensive duplication report across all layers:

```powershell
. .\scripts\Audit-Duplications.ps1
```

This script checks:
1. Duplicate tweak IDs (hard error)
2. Duplicate registry operations (Path\ValueName written by 2+ tweaks)
3. Duplicate labels (same human-readable name in multiple modules)
4. Duplicate instruction/skill files in `.github/`

---

## Fixing Duplicates — Decision Tree

```
Found a duplicate registry op?
├── Same category, same label → REMOVE the newer one (keep the original)
├── Same category, different label → MERGE ApplyOps into one tweak
├── Different categories, same label → Check if both are needed by different profiles
│   ├── Both needed → add "// INTENTIONAL DUPLICATE: needed for {reason}" comment
│   └── Only one needed → REMOVE the one with the wrong category
└── Different categories, different labels → audit semantics; usually remove one
```

---

## What NOT to Do

- Don't add `// duplicate of X` comments and leave the duplicate — remove it
- Don't rename an ID to avoid a collision if the tweak is functionally the same — remove it
- Don't split a multi-registry-path tweak into two separate tweaks — use multiple `ApplyOps`
- Don't commit without running the pre-commit checklist above
