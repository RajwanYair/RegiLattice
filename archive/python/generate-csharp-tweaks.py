"""Auto-generate C# tweak modules from Python source files.

Reads each Python tweak module in regilattice/tweaks/, extracts the TWEAKS
list metadata, and produces equivalent C# static classes using the
declarative RegOp pattern.

Usage:
    python scripts/generate-csharp-tweaks.py
"""

from __future__ import annotations

import ast
import importlib
import os
import re
import sys
import textwrap
from pathlib import Path

# Add project root to path
ROOT = Path(__file__).resolve().parent.parent
sys.path.insert(0, str(ROOT))

TWEAKS_PY_DIR = ROOT / "regilattice" / "tweaks"
TWEAKS_CS_DIR = ROOT / "src" / "RegiLattice.Core" / "Tweaks"

# Map Python module names to C# class names
MODULE_MAP: dict[str, str] = {
    "accessibility": "Accessibility",
    "adobe": "Adobe",
    "audio": "Audio",
    "backup": "Backup",
    "bluetooth": "Bluetooth",
    "boot": "Boot",
    "chrome": "Chrome",
    "clipboard": "Clipboard",
    "cloudstorage": "CloudStorage",
    "communication": "Communication",
    "contextmenu": "ContextMenu",
    "copilot": "Copilot",
    "cortana": "Cortana",
    "crash_diagnostics": "CrashDiagnostics",
    "defender": "Defender",
    "devdrive": "DevDrive",
    "display": "Display",
    "dns_networking": "DnsNetworking",
    "edge": "Edge",
    "explorer": "Explorer",
    "filesystem": "FileSystem",
    "firefox": "Firefox",
    "fonts": "Fonts",
    "gaming": "Gaming",
    "gpu": "Gpu",
    "indexing_search": "IndexingSearch",
    "input": "Input",
    "java": "Java",
    "libreoffice": "LibreOffice",
    "lockscreen": "LockScreen",
    "maintenance": "Maintenance",
    "ms365_copilot": "Ms365Copilot",
    "msstore": "MsStore",
    "multimedia": "Multimedia",
    "network": "Network",
    "nightlight": "NightLight",
    "notifications": "Notifications",
    "office": "Office",
    "onedrive": "OneDrive",
    "performance": "Performance",
    "phonelink": "PhoneLink",
    "pkgmgmt": "PackageManagement",
    "power": "Power",
    "printing": "Printing",
    "privacy": "Privacy",
    "realvnc": "RealVnc",
    "remote_desktop": "RemoteDesktop",
    "scheduled_tasks": "ScheduledTasks",
    "scoop_tools": "ScoopTools",
    "screensaver": "Screensaver",
    "services": "Services",
    "shell": "Shell",
    "snap_multitasking": "SnapMultitasking",
    "speech": "Speech",
    "startup": "Startup",
    "storage": "Storage",
    "system": "SystemTweaks",
    "taskbar": "Taskbar",
    "telemetry_advanced": "TelemetryAdvanced",
    "terminal": "WindowsTerminal",
    "touch_pen": "TouchPen",
    "usb_peripherals": "UsbPeripherals",
    "virtualization": "Virtualization",
    "vscode": "VsCode",
    "widgets_news": "Widgets",
    "win11": "Win11",
    "windowsupdate": "WindowsUpdate",
    "wsl": "Wsl",
    "gitconfig": None,  # skip — not a registry tweak module
}


def load_python_tweaks(module_name: str) -> list[dict]:
    """Import a Python tweak module and extract TWEAKS metadata."""
    # Patch registry to avoid actual winreg calls on import
    import regilattice.registry as reg
    reg.SESSION._dry_run = True

    full_name = f"regilattice.tweaks.{module_name}"
    mod = importlib.import_module(full_name)
    raw_tweaks = getattr(mod, "TWEAKS", [])

    results = []
    for td in raw_tweaks:
        results.append({
            "id": td.id,
            "label": td.label,
            "category": td.category,
            "needs_admin": td.needs_admin,
            "corp_safe": td.corp_safe,
            "registry_keys": list(td.registry_keys),
            "description": td.description,
            "tags": list(td.tags),
            "depends_on": list(td.depends_on),
            "min_build": td.min_build,
            "side_effects": td.side_effects,
            "source_url": getattr(td, "source_url", ""),
        })
    return results


def extract_apply_remove_detect(module_name: str) -> dict[str, dict]:
    """Parse the Python source to extract apply/remove/detect function bodies.

    Returns {tweak_id: {"apply_ops": [...], "remove_ops": [...], "detect_ops": [...]}}
    """
    py_file = TWEAKS_PY_DIR / f"{module_name}.py"
    source = py_file.read_text(encoding="utf-8")

    # Parse function bodies to extract SESSION calls
    functions: dict[str, list[str]] = {}
    current_fn = None
    current_body: list[str] = []

    for line in source.splitlines():
        if line.startswith("def _"):
            if current_fn:
                functions[current_fn] = current_body
            match = re.match(r"def (_\w+)\(", line)
            if match:
                current_fn = match.group(1)
                current_body = []
        elif current_fn and line.strip() and not line.startswith("def ") and not line.startswith("class "):
            current_body.append(line)
        elif current_fn and (line.startswith("def ") or line.startswith("class ") or line.startswith("TWEAKS")):
            functions[current_fn] = current_body
            current_fn = None
            current_body = []
    if current_fn:
        functions[current_fn] = current_body

    return functions


def session_call_to_regop(line: str, key_map: dict[str, str]) -> str | None:
    """Convert a SESSION.set_xxx / SESSION.delete_xxx call to a RegOp factory call."""
    line = line.strip()

    # Resolve key variable references
    for var, val in key_map.items():
        # Use string replacement instead of regex to avoid backslash issues
        line = line.replace(f"{var},", f'@"{val}",').replace(f"{var})", f'@"{val}")')

    # SESSION.set_dword(path, name, value)
    m = re.match(r'SESSION\.set_dword\((.+?),\s*"(.+?)",\s*(\d+)\)', line)
    if m:
        path, name, val = m.group(1).strip().strip('"').strip("'"), m.group(2), m.group(3)
        path = resolve_path(path, key_map)
        return f'RegOp.SetDword(@"{path}", "{name}", {val})'

    # SESSION.set_string(path, name, value)
    m = re.match(r'SESSION\.set_string\((.+?),\s*"(.+?)",\s*"(.+?)"\)', line)
    if m:
        path, name, val = m.group(1).strip().strip('"').strip("'"), m.group(2), m.group(3)
        path = resolve_path(path, key_map)
        return f'RegOp.SetString(@"{path}", "{name}", "{val}")'

    # SESSION.delete_value(path, name)
    m = re.match(r'SESSION\.delete_value\((.+?),\s*"(.+?)"\)', line)
    if m:
        path, name = m.group(1).strip().strip('"').strip("'"), m.group(2)
        path = resolve_path(path, key_map)
        return f'RegOp.DeleteValue(@"{path}", "{name}")'

    # SESSION.delete_tree(path)
    m = re.match(r'SESSION\.delete_tree\((.+?)\)', line)
    if m:
        path = m.group(1).strip().strip('"').strip("'")
        path = resolve_path(path, key_map)
        return f'RegOp.DeleteTree(@"{path}")'

    return None


def detect_call_to_regop(line: str, key_map: dict[str, str]) -> str | None:
    """Convert a detect function return statement to a RegOp check."""
    line = line.strip()
    if not line.startswith("return"):
        return None

    # Resolve key variable references
    for var, val in key_map.items():
        line = line.replace(f"{var},", f'@"{val}",').replace(f"{var})", f'@"{val}")')

    # return SESSION.read_dword(path, name) == value
    m = re.search(r'SESSION\.read_dword\((.+?),\s*"(.+?)"\)\s*==\s*(\d+)', line)
    if m:
        path, name, val = m.group(1).strip().strip('"').strip("'"), m.group(2), m.group(3)
        path = resolve_path(path, key_map)
        return f'RegOp.CheckDword(@"{path}", "{name}", {val})'

    # return SESSION.read_string(path, name) == "value"
    m = re.search(r'SESSION\.read_string\((.+?),\s*"(.+?)"\)\s*==\s*"(.+?)"', line)
    if m:
        path, name, val = m.group(1).strip().strip('"').strip("'"), m.group(2), m.group(3)
        path = resolve_path(path, key_map)
        return f'RegOp.CheckString(@"{path}", "{name}", "{val}")'

    # return SESSION.read_dword(path, name) is None — missing value
    m = re.search(r'SESSION\.read_dword\((.+?),\s*"(.+?)"\)\s*is\s+None', line)
    if m:
        path, name = m.group(1).strip().strip('"').strip("'"), m.group(2)
        path = resolve_path(path, key_map)
        return f'RegOp.CheckMissing(@"{path}", "{name}")'

    # return not SESSION.key_exists(path)
    m = re.search(r'not\s+SESSION\.key_exists\((.+?)\)', line)
    if m:
        path = m.group(1).strip().strip('"').strip("'")
        path = resolve_path(path, key_map)
        return f'RegOp.CheckKeyMissing(@"{path}")'

    return None


def resolve_path(path_str: str, key_map: dict[str, str]) -> str:
    """Resolve a path that might be a variable reference or a string literal."""
    path_str = path_str.strip().strip('"').strip("'").strip('@"').rstrip('"')
    # Check if it's a key map variable
    if path_str in key_map:
        return key_map[path_str]
    return path_str


def extract_key_constants(module_name: str) -> dict[str, str]:
    """Extract _KEY = r"HKEY..." constants from a Python module."""
    py_file = TWEAKS_PY_DIR / f"{module_name}.py"
    source = py_file.read_text(encoding="utf-8")
    key_map = {}
    for m in re.finditer(r'^(_\w+)\s*=\s*r?"(HK[^"]+)"', source, re.MULTILINE):
        key_map[m.group(1)] = m.group(2)
    return key_map


def generate_cs_module(module_name: str, class_name: str) -> str:
    """Generate a C# tweak module from the Python source."""
    try:
        tweaks = load_python_tweaks(module_name)
    except Exception as e:
        print(f"  WARNING: Could not import {module_name}: {e}")
        return ""

    if not tweaks:
        print(f"  SKIP: {module_name} has no tweaks")
        return ""

    key_map = extract_key_constants(module_name)
    functions = extract_apply_remove_detect(module_name)

    lines = []
    lines.append(f"namespace RegiLattice.Core.Tweaks;")
    lines.append("")
    lines.append(f"using RegiLattice.Core.Models;")
    lines.append("")
    lines.append(f"internal static class {class_name}")
    lines.append("{")
    lines.append(f"    internal static IReadOnlyList<TweakDef> Tweaks {{ get; }} =")
    lines.append(f"    [")

    for i, td in enumerate(tweaks):
        lines.append(f"        new TweakDef")
        lines.append(f"        {{")
        lines.append(f'            Id = "{td["id"]}",')
        # Escape any quotes in label
        label = td["label"].replace('"', '\\"')
        lines.append(f'            Label = "{label}",')
        lines.append(f'            Category = "{td["category"]}",')
        lines.append(f'            NeedsAdmin = {str(td["needs_admin"]).lower()},')
        lines.append(f'            CorpSafe = {str(td["corp_safe"]).lower()},')

        # Description
        desc = td["description"].replace('"', '\\"').replace("\n", " ")
        lines.append(f'            Description = "{desc}",')

        # Tags
        if td["tags"]:
            tags_str = ", ".join(f'"{t}"' for t in td["tags"])
            lines.append(f'            Tags = [{tags_str}],')

        # Registry keys
        if td["registry_keys"]:
            keys_str = ", ".join(f'@"{k}"' for k in td["registry_keys"])
            lines.append(f'            RegistryKeys = [{keys_str}],')

        # DependsOn
        if td["depends_on"]:
            deps_str = ", ".join(f'"{d}"' for d in td["depends_on"])
            lines.append(f'            DependsOn = [{deps_str}],')

        # MinBuild
        if td["min_build"]:
            lines.append(f'            MinBuild = {td["min_build"]},')

        # SideEffects
        if td["side_effects"]:
            se = td["side_effects"].replace('"', '\\"')
            lines.append(f'            SideEffects = "{se}",')

        # SourceUrl
        if td.get("source_url"):
            lines.append(f'            SourceUrl = "{td["source_url"]}",')

        # Try to extract apply/remove/detect ops from function bodies
        apply_ops = extract_ops_for_tweak(td["id"], "apply", functions, key_map)
        remove_ops = extract_ops_for_tweak(td["id"], "remove", functions, key_map)
        detect_ops = extract_ops_for_tweak(td["id"], "detect", functions, key_map)

        if apply_ops:
            lines.append(f'            ApplyOps =')
            lines.append(f'            [')
            for op in apply_ops:
                lines.append(f'                {op},')
            lines.append(f'            ],')

        if remove_ops:
            lines.append(f'            RemoveOps =')
            lines.append(f'            [')
            for op in remove_ops:
                lines.append(f'                {op},')
            lines.append(f'            ],')

        if detect_ops:
            lines.append(f'            DetectOps = [{", ".join(detect_ops)}],')

        lines.append(f"        }},")

    lines.append(f"    ];")
    lines.append("}")
    lines.append("")

    return "\n".join(lines)


def extract_ops_for_tweak(tweak_id: str, op_type: str, functions: dict, key_map: dict) -> list[str]:
    """Find the matching function and extract RegOps."""
    # Build a mapping from tweak id suffix to function names
    ops = []

    # Find function that matches this tweak's operation type
    for fn_name, body_lines in functions.items():
        if op_type == "detect" and not fn_name.startswith("_detect_"):
            continue
        if op_type == "apply" and not fn_name.startswith("_apply_"):
            continue
        if op_type == "remove" and not fn_name.startswith("_remove_"):
            continue

        # Match function to tweak by ID patterns
        # Build a searchable suffix from the function name
        fn_suffix = fn_name.replace(f"_{op_type}_", "", 1).replace("_", "-")
        id_suffix = tweak_id.split("-", 1)[-1] if "-" in tweak_id else tweak_id

        # Fuzzy match since naming isn't always 1:1
        if fn_suffix in id_suffix or id_suffix in fn_suffix or fn_suffix.replace("-", "") == id_suffix.replace("-", ""):
            for line in body_lines:
                line = line.strip()
                if op_type in ("apply", "remove"):
                    regop = session_call_to_regop(line, key_map)
                    if regop:
                        ops.append(regop)
                elif op_type == "detect":
                    regop = detect_call_to_regop(line, key_map)
                    if regop:
                        ops.append(regop)
            if ops:
                return ops

    return ops


def main() -> None:
    TWEAKS_CS_DIR.mkdir(parents=True, exist_ok=True)

    total_tweaks = 0
    generated = 0
    skipped = 0

    for module_name, class_name in MODULE_MAP.items():
        if class_name is None:
            continue
        if class_name == "Privacy":
            # Already manually ported
            print(f"  SKIP (already done): {module_name} -> {class_name}")
            skipped += 1
            continue

        print(f"  Generating: {module_name} -> {class_name}.cs")
        cs_content = generate_cs_module(module_name, class_name)
        if not cs_content:
            skipped += 1
            continue

        cs_file = TWEAKS_CS_DIR / f"{class_name}.cs"
        cs_file.write_text(cs_content, encoding="utf-8")
        generated += 1

        # Count tweaks
        count = cs_content.count("new TweakDef")
        total_tweaks += count
        print(f"    -> {count} tweaks")

    print(f"\nDone: {generated} modules generated, {skipped} skipped, {total_tweaks} total tweaks")


if __name__ == "__main__":
    main()
