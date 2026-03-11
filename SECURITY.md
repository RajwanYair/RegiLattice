# Security Policy

This document describes the security policy for **RegiLattice** and how to
report security vulnerabilities responsibly.

---

## Supported Versions

| Version | Supported |
|---------|-----------|
| 3.x (latest) | ✅ Active security fixes |
| 2.x | ❌ No longer supported |
| 1.x (Python) | ❌ Archived — no fixes |

---

## Reporting a Vulnerability

**Do NOT create a public GitHub issue for security vulnerabilities.**

To report a security concern:

1. Open a [GitHub Security Advisory](https://github.com/RajwanYair/RegiLattice/security/advisories/new) (preferred — private disclosure).
2. Alternatively, email the maintainer with the subject line `[SECURITY] RegiLattice - <short description>`.

Please include:

- A clear description of the vulnerability
- Steps to reproduce the issue
- The potential impact and affected versions
- Any suggested remediation or patches

You will receive an acknowledgement within **72 hours** and a full response
within **7 days**.

---

## Security Model

RegiLattice modifies the Windows registry. This carries inherent risk:

| Risk | Mitigation |
|------|------------|
| Irreversible registry changes | `RegistrySession.Backup()` creates JSON backup before every write |
| Privilege escalation | UAC prompt shown before any HKLM write; `Elevation.IsAdmin()` gate |
| Corporate policy violations | `CorporateGuard` detects domain/AAD/GPO/Intune; `CorpSafe` flag per tweak |
| Supply-chain injection | Minimal NuGet dependencies: `System.Management` (Microsoft-published) only |
| Preview mode | `DryRun` mode captures all operations without writing to the registry |

---

## Dependencies

| Package | Version | Publisher | Purpose |
|---------|---------|-----------|---------|
| System.Management | 9.0.3 | Microsoft | WMI queries (CorporateGuard, HardwareInfo) |
| Microsoft.Win32.Registry | (framework) | Microsoft | Registry access via RegistrySession |

---

## Secure Coding Practices

All contributors are expected to follow these practices:

### No Hardcoded Credentials

Never commit passwords, API keys, tokens, or proxy credentials. Use environment
variables or `%LOCALAPPDATA%`.

### No Hardcoded Paths

Use `Environment.GetFolderPath()` for all paths.
Never hardcode `C:\Users\...`.

### Input Validation

Validate all user-supplied tweak IDs, registry paths, and file paths at
the entry points (`Program.cs` in CLI and GUI). Reject inputs with `..` path
traversal sequences.

### Registry Access via RegistrySession

Never use raw `Microsoft.Win32.Registry` calls. All registry access flows
through `RegistrySession`, which enforces DryRun mode, JSON backups,
and structured logging.

### P/Invoke Minimised

Only 2 P/Invoke calls in the entire codebase:

- `GetComputerNameExW` — AD domain detection (CorporateGuard)
- `GlobalMemoryStatusEx` — RAM detection (HardwareInfo)

### Least Privilege

- Request admin (UAC) only when performing HKLM writes.
- `CorpSafe = true` tweaks touch only `HKCU` — no admin required.
- Sealed classes by default to prevent unintended inheritance.
- Never request `SeDebugPrivilege` or `SeTcbPrivilege`.

### No Secrets in Version Control

`.env` files, config files with credentials, and all `*.key` / `*.pem`
files are listed in `.gitignore`.

---

## OWASP Compliance Checklist

| OWASP Category | Status |
|---|---|
| Broken Access Control | ✅ `Elevation.IsAdmin()` + `CorporateGuard` gates |
| Cryptographic Failures | ✅ No encryption used; no secrets stored |
| Injection (command) | ✅ No `Process.Start` with user input; registry via RegistrySession |
| Injection (path traversal) | ✅ Paths validated at entry points; no `..` allowed |
| Insecure Design | ✅ Backup-before-write, DryRun mode |
| Security Misconfiguration | ✅ No external services; no default credentials |
| Vulnerable Components | ✅ Minimal NuGet deps (Microsoft-published); Dependabot enabled |
| Authentication Failures | N/A — local desktop tool, no auth |
| Software Integrity Failures | ✅ MSBuild/dotnet build; pinned CI actions |
| Security Logging | ✅ `RegistrySession.Log` records all writes |
| SSRF | N/A — no outbound HTTP at runtime |

---

## Changelog

Security-relevant changes are documented in [CHANGELOG.md](CHANGELOG.md)
and tagged with `[security]`.
