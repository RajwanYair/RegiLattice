# Security Policy

## Supported Versions

| Version | Supported |
|---------|-----------|
| 3.7.x   | ✅ Active support |
| 3.6.x   | ⚠️ Security fixes only |
| < 3.6   | ❌ Not supported |

## Reporting a Vulnerability

**Please do NOT open a public GitHub issue for security vulnerabilities.**

Report vulnerabilities via [GitHub Private Security Advisory](https://github.com/RajwanYair/RegiLattice/security/advisories/new).

Include:
- A clear description of the vulnerability
- Steps to reproduce
- Potential impact assessment
- Suggested fix (if any)

You will receive an acknowledgement within 48 hours and a full response within 7 days.

## Security Design

RegiLattice is designed with the following security principles:

- **No hardcoded credentials** — all user data stored in `%LOCALAPPDATA%\RegiLattice\`
- **DryRun mode** — preview every change before applying (`--dry-run`)
- **Automatic registry backups** — JSON backup before any destructive operation
- **Corporate Guard** — detects domain-joined/managed environments; blocks unsafe tweaks
- **P/Invoke minimized** — only 4 system calls (documented in source)
- **No network calls from core** — registry operations are entirely local
- **SHA-256 verification** — all Tweak Pack plugins are hash-verified on install
- **Input validation** — all CLI inputs and package names validated before use
