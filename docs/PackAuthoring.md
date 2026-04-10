# Pack Authoring Guide

> **RegiLattice Tweak Pack** — a JSON-only, sandboxed bundle of declarative registry tweaks.
> Packs extend RegiLattice with community content without requiring any compiled code.
> Last updated: 2026-07-01 (v6.29.0)

---

## Table of Contents

1. [Overview](#overview)
2. [Pack JSON Schema](#pack-json-schema)
3. [Tweak Schema](#tweak-schema)
4. [RegOp Schema](#regop-schema)
5. [ID Naming Convention](#id-naming-convention)
6. [Validation Rules](#validation-rules)
7. [Supported RegOp Kinds](#supported-regop-kinds)
8. [Computing the SHA-256 Hash](#computing-the-sha-256-hash)
9. [RSA Signing (Optional)](#rsa-signing-optional)
10. [Submitting to the Marketplace](#submitting-to-the-marketplace)
11. [Testing a Pack Locally](#testing-a-pack-locally)
12. [Official Pack Examples](#official-pack-examples)

---

## Overview

A RegiLattice tweak pack is a single `.rlpack.json` file. It contains:

- **Metadata** — name, version, author, description, categories, tags
- **Tweaks** — an array of declarative registry operation sets (apply / remove / detect)

Packs are loaded by `PackLoader` in `RegiLattice.Core.Plugins`, which validates structure and
registers the tweaks into the engine alongside built-ins. Packs are installed to:

```
%LOCALAPPDATA%\RegiLattice\packs\
```

Packs support a maximum of **100 tweaks** per file, with a maximum file size of **1 MB**.
All registry operations go through the `RegistrySession` wrapper, which honours DryRun mode
and creates backups before any destructive operation.

---

## Pack JSON Schema

```json
{
  "name": "my-pack",
  "displayName": "My Awesome Pack",
  "version": "1.0.0",
  "author": "Your Name or Organisation",
  "description": "A short, human-readable description of what this pack does.",
  "categories": ["Privacy", "Performance"],
  "tags": ["optional", "tags", "for", "search"],
  "minRegiLatticeVersion": "6.0.0",
  "minWindowsBuild": 19041,
  "changelog": "1.0.0 — Initial release",
  "tweaks": [ ... ]
}
```

### Required Fields

| Field         | Type     | Description                                                               |
| ------------- | -------- | ------------------------------------------------------------------------- |
| `name`        | `string` | Machine-readable slug (lowercase, hyphens only). Used as tweak ID prefix. |
| `displayName` | `string` | Human-readable name shown in the GUI and CLI.                             |
| `version`     | `string` | Semantic version string: `MAJOR.MINOR.PATCH`.                             |
| `author`      | `string` | Name or organisation of the pack author.                                  |
| `tweaks`      | `array`  | Array of tweak objects (see Tweak Schema). At least 1 required.           |

### Optional Fields

| Field                   | Type     | Default      | Description                                                          |
| ----------------------- | -------- | ------------ | -------------------------------------------------------------------- |
| `description`           | `string` | `""`         | Longer description shown in the Marketplace dialog.                  |
| `categories`            | `string[]` | `[]`       | Category names for filtering in the marketplace.                     |
| `tags`                  | `string[]` | `[]`       | Search tags.                                                         |
| `minRegiLatticeVersion` | `string` | `"3.3.0"`    | Minimum RegiLattice version required to load this pack.              |
| `minWindowsBuild`       | `integer` | `0`         | Minimum Windows 10/11 build number (e.g. `19041` for 20H1).         |
| `changelog`             | `string` | `""`         | Release notes shown in the Marketplace dialog.                       |
| `signatureUrl`          | `string` | `""`         | URL to a detached RSA-SHA256 `.rlpack.sig` file for signed packs.   |

---

## Tweak Schema

Each item in the `tweaks` array:

```json
{
  "id": "my-pack-unique-tweak-id",
  "label": "Human-Readable Label",
  "category": "Privacy",
  "description": "Optional longer description.",
  "needsAdmin": true,
  "corpSafe": false,
  "tags": ["optional", "tags"],
  "applyOps":  [ ... ],
  "removeOps": [ ... ],
  "detectOps": [ ... ]
}
```

### Tweak Fields

| Field         | Type      | Default | Required | Description                                                              |
| ------------- | --------- | ------- | -------- | ------------------------------------------------------------------------ |
| `id`          | `string`  | —       | Yes      | Global unique ID. **Must start with `{pack-name}-`** (see ID convention).|
| `label`       | `string`  | —       | Yes      | Short human label shown in the GUI.                                      |
| `category`    | `string`  | —       | Yes      | Category group (e.g. "Privacy", "Performance", "Gaming").                |
| `description` | `string`  | `""`    | No       | One-sentence description.                                                |
| `needsAdmin`  | `boolean` | `true`  | No       | Whether applying this tweak requires elevated privileges.                |
| `corpSafe`    | `boolean` | `false` | No       | Whether the tweak is safe to apply on corporate/managed machines.        |
| `tags`        | `string[]` | `[]`  | No       | Search tags for the tweak.                                               |
| `applyOps`    | `RegOp[]` | `[]`   | *See below* | Operations to apply the tweak.                                      |
| `removeOps`   | `RegOp[]` | `[]`   | *See below* | Operations to remove/revert the tweak.                              |
| `detectOps`   | `RegOp[]` | `[]`   | Yes      | Operations to detect whether the tweak is currently applied.            |

**Rules:**
- At least one of `applyOps` or `removeOps` must be non-empty.
- `detectOps` is required and must be non-empty.

---

## RegOp Schema

```json
{
  "kind": "SetDword",
  "path": "HKEY_CURRENT_USER\\SOFTWARE\\Example",
  "name": "MyValue",
  "dwordValue": 1
}
```

### RegOp Fields

| Field         | Type              | When required                              |
| ------------- | ----------------- | ------------------------------------------ |
| `kind`        | `string`          | Always — must be one of the supported kinds |
| `path`        | `string`          | Always — registry key path                 |
| `name`        | `string`          | Always (for value ops)                     |
| `dwordValue`  | `integer`         | `SetDword`, `CheckDword`                   |
| `stringValue` | `string`          | `SetString`, `SetExpandString`, `CheckString` |
| `qwordValue`  | `integer`         | `SetQword`                                 |
| `binaryValue` | `string` (Base64) | `SetBinary` — Base64-encoded bytes         |
| `multiSzValue`| `string[]`        | `SetMultiSz`                               |

---

## ID Naming Convention

Every tweak ID **must** follow this pattern:

```
{pack-name}-{descriptive-slug}
```

Where:
- `{pack-name}` is the exact value of the top-level `"name"` field
- `{descriptive-slug}` is lowercase kebab-case

**Examples:**
```
"name": "my-pack"       → tweak IDs: "my-pack-disable-widgets", "my-pack-enable-hags"
"name": "privacy-pro"   → tweak IDs: "privacy-pro-block-telemetry", "privacy-pro-clear-mru"
```

If any tweak ID does NOT start with `{pack-name}-`, the pack will be rejected by the loader.

---

## Validation Rules

The following rules are enforced by `PackLoader.ValidatePackJson` before a pack is loaded:

| Rule | Error Message |
|------|--------------|
| `name` is present | `"Pack 'name' is required"` |
| `displayName` is present | `"Pack 'displayName' is required"` |
| `version` is present | `"Pack 'version' is required"` |
| `author` is present | `"Pack 'author' is required"` |
| At least 1 tweak | `"Pack must have at least 1 tweak"` |
| Max 100 tweaks | `"Pack exceeds maximum of 100 tweaks"` |
| File size ≤ 1 MB | `"Pack file exceeds 1 MB limit"` |
| Each tweak has an `id` | `"Tweak at index N is missing 'id'"` |
| Each tweak has a `label` | `"Tweak at index N is missing 'label'"` |
| Each tweak has a `category` | `"Tweak at index N is missing 'category'"` |
| Tweak ID starts with pack-name | `"Tweak ID '...' must start with pack name prefix '...'"` |
| Each tweak has `detectOps`  | `"Tweak '...' is missing detectOps"` |
| Tweak has `applyOps` or `removeOps` | `"Tweak '...' has neither applyOps nor removeOps"` |
| Registry paths start with HKEY_ / HKCU / HKLM / HKCR | `"Invalid registry path '...'"` |

---

## Supported RegOp Kinds

The following `kind` values are supported (case-insensitive):

| Kind              | Description                                          | Required fields                    |
| ----------------- | ---------------------------------------------------- | ---------------------------------- |
| `SetDword`        | Write a REG_DWORD value                              | `path`, `name`, `dwordValue`       |
| `SetString`       | Write a REG_SZ string value                          | `path`, `name`, `stringValue`      |
| `SetExpandString` | Write a REG_EXPAND_SZ value (with env var expansion) | `path`, `name`, `stringValue`      |
| `SetQword`        | Write a REG_QWORD value                              | `path`, `name`, `qwordValue`       |
| `SetBinary`       | Write a REG_BINARY value                             | `path`, `name`, `binaryValue`      |
| `SetMultiSz`      | Write a REG_MULTI_SZ value (array of strings)        | `path`, `name`, `multiSzValue`     |
| `DeleteValue`     | Delete a registry value                              | `path`, `name`                     |
| `DeleteTree`      | Delete an entire registry key and its subkeys        | `path`                             |
| `CheckDword`      | Detect: value exists and equals expected DWORD       | `path`, `name`, `dwordValue`       |
| `CheckString`     | Detect: value exists and equals expected string      | `path`, `name`, `stringValue`      |
| `CheckMissing`    | Detect: named value does not exist                   | `path`, `name`                     |
| `CheckKeyMissing` | Detect: registry key does not exist                  | `path`                             |

### Supported Registry Hive Prefixes

| Prefix                   | Hive                       |
| ------------------------ | -------------------------- |
| `HKEY_LOCAL_MACHINE`     | Machine-wide settings      |
| `HKLM`                   | Alias for HKEY_LOCAL_MACHINE |
| `HKEY_CURRENT_USER`      | Per-user settings          |
| `HKCU`                   | Alias for HKEY_CURRENT_USER |
| `HKEY_CLASSES_ROOT`      | File associations          |
| `HKCR`                   | Alias for HKEY_CLASSES_ROOT |
| `HKEY_USERS`             | All user profiles          |
| `HKU`                    | Alias for HKEY_USERS       |

---

## Computing the SHA-256 Hash

When distributing a pack via the marketplace, the `sha256` field in the index should contain
the SHA-256 hash of the raw `.rlpack.json` file content (UTF-8, no BOM):

```powershell
# PowerShell
$hash = (Get-FileHash "my-pack.rlpack.json" -Algorithm SHA256).Hash.ToLower()
Write-Host $hash
```

```bash
# Linux / macOS
sha256sum my-pack.rlpack.json
```

The `PackSignatureVerifier` in RegiLattice verifies this hash at install time.

---

## RSA Signing (Optional)

Signed packs earn **TrustLevel.Signed** in the UI, displayed as a verified badge.
Unsigned packs show **TrustLevel.None**.

### Generating a key pair

```powershell
# Generate 2048-bit RSA key pair (requires OpenSSL)
openssl genrsa -out private.pem 2048
openssl rsa -in private.pem -pubout -out public.pem
```

### Signing a pack

```powershell
# Sign the pack content
openssl dgst -sha256 -sign private.pem -out my-pack.rlpack.sig my-pack.rlpack.json

# Base64-encode the signature for distribution
[Convert]::ToBase64String([System.IO.File]::ReadAllBytes("my-pack.rlpack.sig")) | Set-Content my-pack.rlpack.sig.b64
```

### Registering your public key

Submit a PR to the [regilattice-marketplace](https://github.com/RajwanYair/regilattice-marketplace)
repository adding your `author` name and PEM public key to `index.json`:

```json
{
  "author": "Your Name",
  "publicKeyPem": "-----BEGIN PUBLIC KEY-----\n...\n-----END PUBLIC KEY-----"
}
```

---

## Submitting to the Marketplace

1. Fork [https://github.com/RajwanYair/regilattice-marketplace](https://github.com/RajwanYair/regilattice-marketplace)
2. Add your pack file to the `packs/` directory
3. Add your pack metadata to `index.json` in the `packs` array
4. Open a Pull Request with the title `feat: add pack {your-pack-name} v1.0.0`
5. The `pack-validation.yml` CI workflow will validate your pack JSON automatically
6. A maintainer will review and merge the PR

### PR Checklist

```
☐ Pack file placed in packs/ with name {pack-name}.rlpack.json
☐ Pack JSON passes local validation: dotnet run --project src/RegiLattice.CLI -- --validate-pack path/to/pack.json
☐ All tweak IDs start with {pack-name}-
☐ All detectOps are present and correct
☐ SHA-256 hash computed and added to index.json
☐ No duplicate registry paths/values with built-in tweaks (run audit first)
☐ Pack description is accurate and grammatically correct
```

---

## Testing a Pack Locally

### Via CLI

```powershell
# Validate pack JSON for structural errors
dotnet run --project src/RegiLattice.CLI -- --validate-pack "path\to\my-pack.rlpack.json"

# Install the pack to the local data directory
dotnet run --project src/RegiLattice.CLI -- --install-pack "path\to\my-pack.rlpack.json"

# List installed packs
dotnet run --project src/RegiLattice.CLI -- --list-packs

# Show tweaks from a specific pack
dotnet run --project src/RegiLattice.CLI -- --list --search "my-pack"
```

### Via Tests

Use `PackLoader.LoadFromJson()` in a unit test:

```csharp
[Fact]
public void MyPack_LoadsValid()
{
    string json = File.ReadAllText("path/to/my-pack.rlpack.json");
    var (pack, tweaks) = PackLoader.LoadFromJson(json);
    Assert.NotNull(pack);
    Assert.Equal(5, tweaks.Count);
    Assert.All(tweaks, t => Assert.NotEmpty(t.ApplyOps.Concat(t.RemoveOps)));
    Assert.All(tweaks, t => Assert.NotEmpty(t.DetectOps));
}
```

---

## Official Pack Examples

Five official packs ship with RegiLattice in the `packs/` directory:

| File                                    | Tweaks | Description                                          |
| --------------------------------------- | ------ | ---------------------------------------------------- |
| `packs/privacy-extreme.rlpack.json`     | 5      | Advanced privacy beyond standard built-ins           |
| `packs/gaming-fps.rlpack.json`          | 5      | FPS and latency optimizations for competitive gaming |
| `packs/enterprise-soc2.rlpack.json`     | 5      | SOC 2 Type II registry hardening controls            |
| `packs/developer-full.rlpack.json`      | 5      | Developer workflow: long paths, UTF-8, PS logging   |
| `packs/accessibility-inclusive.rlpack.json` | 5  | Inclusive accessibility: Toggle Keys, Mouse Keys    |

These official packs also serve as authoritative examples of the pack JSON format.
