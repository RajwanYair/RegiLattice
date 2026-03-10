r"""Template: How to add a new tweak to RegiLattice.

===========================================================================
 QUICK-START  (5 minutes to your first tweak)
===========================================================================

 1. Copy this file  → regilattice/tweaks/yourmodule.py
 2. Define key paths → _MY_KEY = r"HKEY_...\Path"
 3. Write triplets   → _apply_xxx(), _remove_xxx(), _detect_xxx()
 4. Register         → append TweakDef(...) to the TWEAKS list
 5. Test             → python -m pytest tests/ -x --tb=short -q

 That's it!  The plugin loader auto-discovers any .py file in this
 package that exports ``TWEAKS: list[TweakDef]``.

===========================================================================
 ARCHITECTURE
===========================================================================

 ┌──────────────┐   auto-discover   ┌─────────────────────┐
 │ tweaks/      │ ───────────────►  │ tweaks/__init__.py   │
 │  yourmod.py  │  (pkgutil scan)   │  all_tweaks()        │
 │  TWEAKS list │                   │  tweaks_by_category()│
 └──────────────┘                   └─────────┬───────────┘
                                              │
                  ┌───────────────────────────┼──────────────────┐
                  │                           │                  │
           ┌──────▼──────┐            ┌───────▼──────┐   ┌──────▼──────┐
           │   gui.py    │            │   cli.py     │   │  profiles   │
           │  (Tkinter)  │            │  (argparse)  │   │  (presets)  │
           └─────────────┘            └──────────────┘   └─────────────┘

 TweakDef fields:
   id           — Globally unique kebab-case ID  (e.g. "myapp-disable-telemetry")
   label        — Human-readable name shown in GUI
   category     — Grouping header (creates new category if none exists)
   apply_fn     — Callable that WRITES registry values
   remove_fn    — Callable that REVERTS / deletes values
   detect_fn    — Callable that returns True if tweak is active
   needs_admin  — True for HKLM keys; False for HKCU
   corp_safe    — True unless tweak might break enterprise policies
   registry_keys— List of every key path touched (for backup/display)
   description  — Tooltip text (1-2 sentences)
   tags         — Search keywords: [category, verb, noun, ...]

===========================================================================
 REGISTRY METHOD CHEAT-SHEET  (from regilattice.registry.SESSION)
===========================================================================

 SESSION.set_dword(key, name, value)    — Write a DWORD (integer)
 SESSION.set_string(key, name, value)   — Write a REG_SZ (string)
 SESSION.read_dword(key, name)          — Read DWORD (returns int | None)
 SESSION.read_string(key, name)         — Read REG_SZ (returns str | None)
 SESSION.delete_value(key, name)        — Delete a single value
 SESSION.key_exists(key)                — True if key path exists
 SESSION.backup(keys, label)            — Snapshot key(s) before change
 SESSION.log(message)                   — Write to log/console

===========================================================================
 COMMON PATTERNS
===========================================================================

 Pattern 1 — Toggle a DWORD on/off:
     apply:  SESSION.set_dword(KEY, "ValueName", 0)
     remove: SESSION.set_dword(KEY, "ValueName", 1)  # or delete_value
     detect: SESSION.read_dword(KEY, "ValueName") == 0

 Pattern 2 — Delete a value entirely:
     apply:  SESSION.delete_value(KEY, "ValueName")
     remove: SESSION.set_dword(KEY, "ValueName", <default>)
     detect: SESSION.read_dword(KEY, "ValueName") is None

 Pattern 3 — Multiple versions (like Office 14.0/15.0/16.0):
     Loop over version list; apply to each detected version.
     See office.py for a full example.

 Pattern 4 — Disable a Windows service:
     apply:  SESSION.set_dword(SVC_KEY, "Start", 4)     # 4 = Disabled
     remove: SESSION.set_dword(SVC_KEY, "Start", 3)     # 3 = Manual
     detect: SESSION.read_dword(SVC_KEY, "Start") == 4

===========================================================================
 CHECKLIST  (before submitting)
===========================================================================

 [ ] Each tweak has _apply_, _remove_, _detect_ functions (triplet).
 [ ] TweakDef ``id`` is globally unique (use ``<module>-<verb>-<noun>``).
 [ ] ``category`` matches existing category or intentionally creates new one.
 [ ] ``needs_admin=True`` if ANY key is under HKLM.
 [ ] ``corp_safe=True`` unless tweak could disrupt enterprise management.
 [ ] ``registry_keys`` lists EVERY key the tweak reads or writes.
 [ ] ``tags`` includes category keyword + 1-2 descriptive words.
 [ ] _apply_ calls SESSION.backup() BEFORE writing any values.
 [ ] _apply_ calls SESSION.log() with a clear message.
 [ ] ``require_admin`` kwarg present on apply/remove (even if unused).
 [ ] Tests pass: ``python -m pytest tests/ -x --tb=short -q``

===========================================================================
 TROUBLESHOOTING
===========================================================================

 Q: My module isn't picked up by the GUI.
 A: Ensure the file is in regilattice/tweaks/ and exports TWEAKS: list[TweakDef].

 Q: "assert_admin" raises even though I'm admin.
 A: The test suite runs with dry_run=True and require_admin=False.
    Make sure your detect_fn doesn't call assert_admin.

 Q: Two tweaks share the same id and one is silently ignored.
 A: IDs must be globally unique. Search all modules for duplicates.

 Q: My tweak doesn't appear in any profile.
 A: Profiles map by category name.  Check tweaks/__init__.py PROFILES dict.

===========================================================================
 STANDARD FILE STRUCTURE  (enforced by code review)
===========================================================================

 Every tweak module MUST follow this layout — top to bottom:

   1. Module docstring (one paragraph summarising coverage)
   2. Imports
   3. # ── Key paths ── block (ALL registry paths for the whole module)
   4. Repeated for each tweak group:
       a. # ── Tweak Label ── block header
       b. _apply_<name>(*, require_admin: bool = True) -> None
       c. _remove_<name>(*, require_admin: bool = True) -> None
       d. _detect_<name>() -> bool
       e. TWEAKS += [TweakDef(...)]     <- immediately after the triplet

 DECISION: keep TweakDef RIGHT after the triplet (not consolidated at end).
 This keeps each group self-contained: if you edit a triplet, the
 TweakDef binding is immediately visible without scrolling to the bottom.
 Adding a new tweak = scroll to the end of its section, not the whole file.

 Example:

   # ── Key paths ────────────────────────────────────────────────────────────
   _FOO_KEY = r"HKEY_LOCAL_MACHINE\..."
   _BAR_KEY = r"HKEY_CURRENT_USER\..."

   # ── Disable Foo ───────────────────────────────────────────────────────────
   def _apply_disable_foo(*, require_admin: bool = True) -> None: ...
   def _remove_disable_foo(*, require_admin: bool = True) -> None: ...
   def _detect_disable_foo() -> bool: ...

   TWEAKS += [
       TweakDef(
           id="mycat-disable-foo",
           ...
       ),
   ]

   # ── Enable Bar ────────────────────────────────────────────────────────────
   def _apply_enable_bar(*, require_admin: bool = True) -> None: ...
   ...
   TWEAKS += [TweakDef(id="mycat-enable-bar", ...)]

 Comment style:
   - Block headers:  # ── Section Title ─[fill]──  (per-section, required)
   - Inline values: SESSION.set_dword(KEY, "Name", 4)  # 4 = Disabled (allowed)
   - No per-line prose comments — keep the code self-documenting
"""

from __future__ import annotations

# Step 1 -- Import the required helpers.
from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ---------------------------------------------------------------------------
# Step 2 — Define registry key constants.
# Use raw strings (r"...") for backslashes.
# ---------------------------------------------------------------------------

_EXAMPLE_KEY = r"HKEY_CURRENT_USER\Software\MyApp\Settings"
_EXAMPLE_HKLM = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\MyApp"


# ---------------------------------------------------------------------------
# Step 3 — Write the function triplet for each tweak.
#
#   _apply_<name>   → write the registry value(s)
#   _remove_<name>  → delete or restore the default value(s)
#   _detect_<name>  → return True if the tweak is currently active
#
# The `require_admin` kwarg is part of the standard signature.
# Call ``assert_admin(require_admin)`` when the key is under HKLM.
# For HKCU keys that don't need elevation, it can be a no-op but keep
# the parameter for API consistency.
# ---------------------------------------------------------------------------


# ── Example tweak: Disable MyApp Telemetry ────────────────────────────────


def _apply_disable_telemetry(*, require_admin: bool = False) -> None:
    # No assert_admin() needed — HKCU key, user-level.
    SESSION.log("MyApp: disable telemetry")
    SESSION.backup([_EXAMPLE_KEY], "MyAppTelemetry")  # always backup first
    SESSION.set_dword(_EXAMPLE_KEY, "Telemetry", 0)  # DWORD value


def _remove_disable_telemetry(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_EXAMPLE_KEY, "Telemetry", 1)  # restore default


def _detect_disable_telemetry() -> bool:
    return SESSION.read_dword(_EXAMPLE_KEY, "Telemetry") == 0


# ── Example tweak: Set MyApp Theme (string value) ────────────────────────


def _apply_dark_theme(*, require_admin: bool = False) -> None:
    SESSION.log("MyApp: set dark theme")
    SESSION.backup([_EXAMPLE_KEY], "MyAppTheme")
    SESSION.set_string(_EXAMPLE_KEY, "Theme", "dark")  # string value


def _remove_dark_theme(*, require_admin: bool = False) -> None:
    SESSION.set_string(_EXAMPLE_KEY, "Theme", "light")


def _detect_dark_theme() -> bool:
    return SESSION.read_string(_EXAMPLE_KEY, "Theme") == "dark"


# ── Example tweak: HKLM key (needs admin) ────────────────────────────────


def _apply_disable_policy(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)  # <-- required for HKLM keys
    SESSION.log("MyApp: disable via policy")
    SESSION.backup([_EXAMPLE_HKLM], "MyAppPolicy")
    SESSION.set_dword(_EXAMPLE_HKLM, "DisableMyApp", 1)


def _remove_disable_policy(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_EXAMPLE_HKLM, "DisableMyApp")  # remove entirely


def _detect_disable_policy() -> bool:
    return SESSION.read_dword(_EXAMPLE_HKLM, "DisableMyApp") == 1


# ---------------------------------------------------------------------------
# Step 4 — Register tweaks in the TWEAKS list.
#
# Each TweakDef entry ties together:
#   - A unique ``id`` (kebab-case, globally unique across all modules)
#   - A human-readable ``label`` (shown in GUI)
#   - The ``category`` (used for grouping in GUI and profiles)
#   - The three functions from step 3
#   - Metadata: needs_admin, corp_safe, registry_keys, description, tags
# ---------------------------------------------------------------------------

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="myapp-disable-telemetry",  # must be unique across all modules
        label="Disable MyApp Telemetry",  # shown in GUI
        category="My Category",  # creates new category if none exists
        apply_fn=_apply_disable_telemetry,
        remove_fn=_remove_disable_telemetry,
        detect_fn=_detect_disable_telemetry,
        needs_admin=False,  # HKCU → no admin needed
        corp_safe=True,  # safe for corporate environments
        registry_keys=[_EXAMPLE_KEY],  # every key the tweak touches
        description=(  # shown in tooltip
            "Stops MyApp from sending usage data to vendor servers."
        ),
        tags=["myapp", "telemetry", "privacy"],  # used for search / filtering
    ),
    TweakDef(
        id="myapp-dark-theme",
        label="MyApp: Enable Dark Theme",
        category="My Category",
        apply_fn=_apply_dark_theme,
        remove_fn=_remove_dark_theme,
        detect_fn=_detect_dark_theme,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_EXAMPLE_KEY],
        description="Sets the MyApp display theme to dark mode.",
        tags=["myapp", "theme", "appearance"],
    ),
    TweakDef(
        id="myapp-disable-policy",
        label="Disable MyApp via Group Policy",
        category="My Category",
        apply_fn=_apply_disable_policy,
        remove_fn=_remove_disable_policy,
        detect_fn=_detect_disable_policy,
        needs_admin=True,  # HKLM → admin required
        corp_safe=True,  # policy-based → corp friendly
        registry_keys=[_EXAMPLE_HKLM],
        description="Disables MyApp entirely via a machine-level Group Policy key.",
        tags=["myapp", "policy", "disable"],
    ),
]
