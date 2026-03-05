---
name: New Tweak Proposal
about: Propose a new registry tweak to add
title: "[Tweak] "
labels: tweak, enhancement
assignees: ''
---

## Tweak Details

- **ID (kebab-case):** `example-disable-feature`
- **Label:** Disable Feature
- **Category:** (existing category or new one)
- **Needs admin:** Yes / No
- **Corp safe:** Yes / No

## Registry Keys

```
HKEY_LOCAL_MACHINE\SOFTWARE\Policies\...
  ValueName (REG_DWORD) = 1
```

## What It Does

Description of what the tweak changes and why a user would want it.

## Apply Logic

```python
SESSION.set_dword(r"HKLM\SOFTWARE\Policies\...", "ValueName", 1)
```

## Remove Logic (revert to default)

```python
SESSION.delete_value(r"HKLM\SOFTWARE\Policies\...", "ValueName")
```

## Detect Logic

```python
SESSION.read_dword(r"HKLM\SOFTWARE\Policies\...", "ValueName") == 1
```

## References

- Microsoft docs link
- Related Group Policy setting
- Windows version requirements (10/11, specific builds)

## Checklist

- [ ] I've verified the registry path exists on a fresh Windows install
- [ ] I've tested apply + remove manually via `reg add` / `reg delete`
- [ ] The tweak is reversible (remove restores the default state)
- [ ] The ID doesn't conflict with existing tweaks (`python -m regilattice --list`)
