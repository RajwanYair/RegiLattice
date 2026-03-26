# Getting Support

## Quick Links

| Resource | Link |
|----------|------|
| Documentation | [docs/](docs/) |
| Changelog | [docs/CHANGELOG.md](docs/CHANGELOG.md) |
| Troubleshooting | [docs/Troubleshooting.md](docs/Troubleshooting.md) |
| Discussions | [GitHub Discussions](https://github.com/RajwanYair/RegiLattice/discussions) |
| Security | [Report a vulnerability](https://github.com/RajwanYair/RegiLattice/security/advisories/new) |

---

## Before Opening an Issue

1. Check the [Troubleshooting Guide](docs/Troubleshooting.md) — covers the most common build errors, OneDrive cache issues, and registry access problems
2. Search [existing issues](https://github.com/RajwanYair/RegiLattice/issues) — your problem may already be tracked
3. Run the built-in diagnostics:
   ```powershell
   RegiLatticeCLI.exe --doctor      # system health check
   RegiLatticeCLI.exe --validate    # TweakDef integrity check
   ```

---

## Opening an Issue

Use one of the **[issue templates](https://github.com/RajwanYair/RegiLattice/issues/new/choose)**:

| Template | When to use |
|----------|-------------|
| Bug Report | App crashes, tweaks don't apply, incorrect behaviour |
| Build Issue | Compile errors, test failures, CI problems |
| Feature Request | New features, new dialogs/services, UI improvements |
| New Tweak | Request a specific Windows registry tweak |
| Performance Issue | Slow startup, high memory usage, UI lag |

---

## Debugging a Failed GitHub Actions Run

1. Open the failing run in the **[Actions tab](https://github.com/RajwanYair/RegiLattice/actions)**
2. Expand the ❌ failed step — the log shows the exact error
3. Trigger the **[Debug Diagnostics](https://github.com/RajwanYair/RegiLattice/actions/workflows/debug.yml)** workflow manually (`workflow_dispatch`) for a full environment dump with verbose build output and crash artifacts
4. A GitHub Issue is automatically created when CI or Release fails (see [notify-failure.yml](.github/workflows/notify-failure.yml))

---

## Security Vulnerabilities

**Do not open public issues for security bugs.**

Report them privately via [GitHub Security Advisories](https://github.com/RajwanYair/RegiLattice/security/advisories/new).

---

## Response Time

This is a single-developer project. Expected response times:

| Type | Expected response |
|------|------------------|
| Security reports | 1–2 business days |
| Broken builds / regressions | 1–3 days |
| Bug reports | 3–7 days |
| Feature requests / tweak requests | Best-effort, may take weeks |
