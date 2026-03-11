"""Improved fixer: resolves Python constant placeholders in C# tweak files.

Handles: multi-line parenthesized strings, rf"" interpolation, 
variable-based composition (rf"{_BASE}\Sub"), and Description escaping.
"""

import ast
import os
import re
import sys
from pathlib import Path

PY_DIR = Path(r"c:\Users\ryair\OneDrive - Intel Corporation\Documents\MyScripts\RegiLattice\regilattice\tweaks")
CS_DIR = Path(r"c:\Users\ryair\OneDrive - Intel Corporation\Documents\MyScripts\RegiLattice\src\RegiLattice.Core\Tweaks")


def extract_all_constants(py_file: Path) -> dict[str, str]:
    """Extract all _CONST = ... definitions that resolve to HKEY_ paths.
    
    Handles:
    - Single-line: _KEY = r"HKEY_..."
    - Multi-line parenthesized: _KEY = (\n    r"HKEY_..."\n    r"\\Sub"\n)
    - rf-string with var refs: _SUB = rf"{_BASE}\\SubKey"
    """
    text = py_file.read_text(encoding="utf-8")
    lines = text.split("\n")
    raw_consts: dict[str, str] = {}  # name -> raw RHS text
    resolved: dict[str, str] = {}    # name -> resolved HKEY_... path

    # Pass 1: collect all raw assignments
    i = 0
    while i < len(lines):
        line = lines[i]
        m = re.match(r"^(_[A-Za-z_][A-Za-z_0-9]*)\s*=\s*(.*)", line)
        if m:
            name = m.group(1)
            rest = m.group(2).strip()
            full = rest
            # Multi-line: accumulate until closing paren
            if rest.startswith("(") and ")" not in rest:
                i += 1
                while i < len(lines):
                    stripped = lines[i].strip()
                    full += " " + stripped
                    if ")" in stripped:
                        break
                    i += 1
            raw_consts[name] = full
        i += 1

    # Pass 2: resolve each to a string value
    for name, raw in raw_consts.items():
        # Try literal_eval first (handles r"..." and parenthesized string concat)
        try:
            val = ast.literal_eval(raw)
            if isinstance(val, str) and "HKEY_" in val:
                resolved[name] = val
                continue
        except Exception:
            pass

        # Handle rf"{_VAR}\SubKey" patterns
        # First try to extract the f-string content
        fm = re.search(r'rf?"(.*?)"', raw) or re.search(r"rf?'(.*?)'", raw)
        if fm:
            fval = fm.group(1)
            if "HKEY_" in fval or "{" in fval:
                # Resolve variable references like {_DEFENDER}
                def resolve_ref(m2: re.Match) -> str:
                    ref_name = m2.group(1)
                    return resolved.get(ref_name, raw_consts.get(ref_name, m2.group(0)))
                
                result = re.sub(r"\{(_[A-Za-z_][A-Za-z_0-9]*)\}", resolve_ref, fval)
                if "HKEY_" in result and "{" not in result:
                    resolved[name] = result
                    continue

        # Try extracting just the HKEY_ path from the raw expression
        hm = re.search(r'"(HKEY_[^"]+)"', raw) or re.search(r"'(HKEY_[^']+)'", raw)
        if hm:
            resolved[name] = hm.group(1)

    # Pass 3: re-resolve rf-string refs now that more base constants are known
    for name, raw in raw_consts.items():
        if name in resolved:
            continue
        fm = re.search(r'rf?"(.*?)"', raw) or re.search(r"rf?'(.*?)'", raw)
        if fm:
            fval = fm.group(1)
            def resolve_ref2(m2: re.Match) -> str:
                ref_name = m2.group(1)
                return resolved.get(ref_name, m2.group(0))
            result = re.sub(r"\{(_[A-Za-z_][A-Za-z_0-9]*)\}", resolve_ref2, fval)
            if "HKEY_" in result and "{" not in result:
                resolved[name] = result

    return resolved


# Python stem → C# file name mapping
NAME_MAP = {
    "crash_diagnostics": "CrashDiagnostics",
    "dns_networking": "DnsNetworking",
    "indexing_search": "IndexingSearch",
    "ms365_copilot": "Ms365Copilot",
    "scoop_tools": "ScoopTools",
    "snap_multitasking": "SnapMultitasking",
    "scheduled_tasks": "ScheduledTasks",
    "touch_pen": "TouchPen",
    "usb_peripherals": "UsbPeripherals",
    "telemetry_advanced": "TelemetryAdvanced",
    "remote_desktop": "RemoteDesktop",
    "widgets_news": "Widgets",
    "cloudstorage": "CloudStorage",
    "contextmenu": "ContextMenu",
    "lockscreen": "LockScreen",
    "nightlight": "NightLight",
    "phonelink": "PhoneLink",
    "pkgmgmt": "PackageManagement",
    "windowsupdate": "WindowsUpdate",
    "realvnc": "RealVnc",
    "filesystem": "FileSystem",
    "msstore": "MsStore",
    "system": "SystemTweaks",
    "services": "Services",
    "terminal": "WindowsTerminal",
    "windows11": "Win11",
}


def py_to_cs_name(py_stem: str) -> str:
    if py_stem in NAME_MAP:
        return NAME_MAP[py_stem]
    return py_stem.title().replace("_", "")


def fix_cs_file(cs_file: Path, consts: dict[str, str]) -> tuple[int, list[str]]:
    """Fix placeholder references. Returns (fix_count, unresolved_list)."""
    text = cs_file.read_text(encoding="utf-8")
    original = text
    count = 0

    # Fix @"_CONST@"HKEY_..." corrupted patterns (type 1)
    for const_name in consts:
        bad = f'@"{const_name}@"'
        if bad in text:
            text = text.replace(bad, '@"')
            count += 1

    # Fix @"_CONST" standalone placeholders (type 2)
    for const_name, path in consts.items():
        placeholder = f'@"{const_name}"'
        if placeholder in text:
            text = text.replace(placeholder, f'@"{path}"')
            count += 1

    # Fix @"_ver_key(ver", "SubKey") patterns (type 3 — Office)
    if "_ver_key" in consts:
        base_path = consts["_ver_key"]
        pattern = r'@"_ver_key\([^"]*",\s*"([^"]+)"\)'
        def replace_ver_key(m: re.Match) -> str:
            sub_key = m.group(1)
            return f'@"{base_path}\\{sub_key}"'
        new_text = re.sub(pattern, replace_ver_key, text)
        if new_text != text:
            text = new_text
            count += 1

    # Fix @"_svc("name")" patterns
    if "_svc" in consts:
        svc_template = consts["_svc"]
        pattern = r'@"_svc\("([^"]+)"\)"'
        def replace_svc(m: re.Match) -> str:
            svc_name = m.group(1)
            resolved = svc_template.replace("{name}", svc_name)
            return f'@"{resolved}"'
        new_text = re.sub(pattern, replace_svc, text)
        if new_text != text:
            text = new_text
            count += 1

    # Fix unescaped backslashes in Description strings (type 4)
    def fix_desc(m: re.Match) -> str:
        return f'Description = @"{m.group(1)}"'
    new_text = re.sub(r'Description = "([^"]*\\[^"]*)"', fix_desc, text)
    if new_text != text:
        text = new_text
        count += 1

    if text != original:
        cs_file.write_text(text, encoding="utf-8")

    # Find remaining unresolved
    remaining = re.findall(r'@"(_[A-Za-z_][A-Za-z_0-9]*)"', text)
    # Filter out false positives (strings that happen to start with _)
    unresolved = [r for r in set(remaining) if not r.startswith("_template")]
    
    return count, unresolved


def main():
    total_fixes = 0
    files_fixed = 0
    all_unresolved: dict[str, list[str]] = {}
    
    # Build global constant map from ALL Python files first
    # This handles cross-file references (though rare)
    global_consts: dict[str, str] = {}
    for py_file in sorted(PY_DIR.glob("*.py")):
        if py_file.name.startswith("_"):
            continue
        consts = extract_all_constants(py_file)
        for k, v in consts.items():
            global_consts[k] = v

    print(f"Extracted {len(global_consts)} constants from Python files\n")

    for py_file in sorted(PY_DIR.glob("*.py")):
        if py_file.name.startswith("_"):
            continue
        
        stem = py_file.stem
        cs_name = py_to_cs_name(stem) + ".cs"
        cs_file = CS_DIR / cs_name

        if not cs_file.exists():
            continue

        # Use file-specific constants + global fallback
        file_consts = extract_all_constants(py_file)
        combined = {**global_consts, **file_consts}  # file-specific wins

        fixes, unresolved = fix_cs_file(cs_file, combined)
        if fixes > 0:
            total_fixes += fixes
            files_fixed += 1
            print(f"  {cs_name}: {fixes} fix(es)")
        if unresolved:
            all_unresolved[cs_name] = unresolved
            print(f"  {cs_name}: UNRESOLVED: {unresolved}")

    print(f"\nTotal: {total_fixes} fixes across {files_fixed} files")
    if all_unresolved:
        print(f"\nStill unresolved in {len(all_unresolved)} files:")
        for f, u in sorted(all_unresolved.items()):
            print(f"  {f}: {u}")


if __name__ == "__main__":
    main()
