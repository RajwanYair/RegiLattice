# Security Policy

This document describes the security policy for **RegiLattice** and how to
report security vulnerabilities responsibly.

---

## Supported Versions

| Version | Supported |
|---------|-----------|
| 1.0.x (latest) | ✅ Active security fixes |
| < 1.0 | ❌ No longer supported |

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
|------|-----------|
| Irreversible registry changes | `RegistrySession.backup()` called before every write |
| Privilege escalation | UAC prompt shown before any HKLM write; `assert_admin()` gate |
| Corporate policy violations | `corpguard.py` detects domain/AAD/VPN/GPO/Intune; `corp_safe` flag per tweak |
| Plugin code execution | Plugins loaded from `~/.regilattice/plugins/` only; path escapes rejected |
| Supply-chain injection | Zero runtime dependencies; stdlib only |

---

## Secure Coding Practices

All contributors are expected to follow these practices:

### No Hardcoded Credentials

Never commit passwords, API keys, tokens, or proxy credentials. Use environment
variables or the system keyring.

### No Hardcoded Paths

Use `Path(__file__).parent.resolve()` or `Path.home()` for all paths.
Never hardcode `C:\Users\...`.

### Input Validation

Validate all user-supplied tweak IDs, registry paths, and file paths at
the entry points (`cli.py`, `gui.py`). Reject inputs with `..` path
traversal sequences.

### Parameterised Commands

Never concatenate user input into shell command strings. All `subprocess`
calls use list arguments, not `shell=True`.

### Least Privilege

- Request admin (UAC) only when performing HKLM writes.
- `corp_safe=True` tweaks touch only `HKCU` — no admin required.
- Never request `SeDebugPrivilege` or `SeTcbPrivilege`.

### No Secrets in Version Control

`.env` files, `.regilattice.toml` with credentials, and all `*.key` / `*.pem`
files are listed in `.gitignore`.

---

## OWASP Compliance Checklist

| OWASP Category | Status |
|---|---|
| Broken Access Control | ✅ `assert_admin()` + `corp_guard()` gates |
| Cryptographic Failures | ✅ No encryption used; no secrets stored |
| Injection (command) | ✅ All subprocess calls use list args |
| Injection (path traversal) | ✅ Plugin paths validated; no `..` allowed |
| Insecure Design | ✅ Backup-before-write, dry-run mode |
| Security Misconfiguration | ✅ No external services; no default credentials |
| Vulnerable Components | ✅ Zero runtime deps; Dependabot enabled |
| Authentication Failures | N/A — local desktop tool, no auth |
| Software Integrity Failures | ✅ Hatchling build; pinned CI actions |
| Security Logging | ✅ `RegistrySession.log()` records all writes |
| SSRF | N/A — no outbound HTTP at runtime |

---

## Changelog

Security-relevant changes are documented in [CHANGELOG.md](CHANGELOG.md)
and tagged with `[security]`.
