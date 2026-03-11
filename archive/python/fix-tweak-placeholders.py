"""Fix auto-generated C# tweak files by resolving Python constant placeholders.

Reads each Python tweak module to extract _CONST = r"HKEY_..." mappings,
then replaces @"_CONST" in the corresponding C# file with the real path.
Also fixes Description strings with unescaped backslashes.
"""

import os
import re
import sys
from pathlib import Path

PY_DIR = Path(r"c:\Users\ryair\OneDrive - Intel Corporation\Documents\MyScripts\RegiLattice\regilattice\tweaks")
CS_DIR = Path(r"c:\Users\ryair\OneDrive - Intel Corporation\Documents\MyScripts\RegiLattice\src\RegiLattice.Core\Tweaks")

# Python file → C# file name mapping (when names differ)
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
}


def extract_py_constants(py_file: Path) -> dict[str, str]:
    """Extract _CONST = r"HKEY_..." mappings from a Python file."""
    consts: dict[str, str] = {}
    text = py_file.read_text(encoding="utf-8")
    
    # Match patterns like: _KEY_NAME = r"HKEY_LOCAL_MACHINE\..."
    # Also handle: _KEY_NAME = r"HKEY_CURRENT_USER\..."
    for m in re.finditer(r'^(_[A-Z_][A-Z_0-9]*)\s*=\s*r?"(HKEY_[^"]+)"', text, re.MULTILINE):
        consts[m.group(1)] = m.group(2)
    
    # Also handle f-string patterns like: _ver_key = lambda ver: f"HKEY_CURRENT_USER\\Software\\Microsoft\\Office\\{ver}\\Common"
    for m in re.finditer(r'^(_[a-z_][a-z_0-9]*)\s*=\s*lambda\s*\w+\s*:\s*(?:f"(HKEY_[^"]+)"|f\'(HKEY_[^\']+)\')', text, re.MULTILINE):
        raw = m.group(2) or m.group(3)
        # Replace {ver} with 16.0 (most common Office version)
        resolved = re.sub(r'\{[^}]+\}', '16.0', raw)
        consts[m.group(1)] = resolved

    # Handle _svc = lambda name: f"..." patterns for services
    for m in re.finditer(r'^(_[a-z_]+)\s*=\s*lambda\s+(\w+)\s*:\s*(?:f"(HKEY_[^"]+)"|rf?"(HKEY_[^"]+)")', text, re.MULTILINE):
        raw = m.group(3) or m.group(4)
        # We can't resolve lambda vars generically — skip
        consts[m.group(1)] = raw  # Store template

    return consts


def py_to_cs_name(py_stem: str) -> str:
    """Convert Python module name to C# file name."""
    if py_stem in NAME_MAP:
        return NAME_MAP[py_stem]
    return py_stem.title().replace("_", "")


def fix_cs_file(cs_file: Path, consts: dict[str, str]) -> int:
    """Fix placeholder references in a C# file. Returns number of replacements."""
    text = cs_file.read_text(encoding="utf-8")
    original = text
    count = 0

    # Pattern 1: @"_CONST@"HKEY_... (corrupted verbatim strings where const leaked)
    # Example: @"_EXP@"HKEY_CURRENT_USER\Software\..." → @"HKEY_CURRENT_USER\Software\..."
    for const_name, path in consts.items():
        pattern = re.escape(f'@"{const_name}@"')
        if re.search(pattern, text):
            text = text.replace(f'@"{const_name}@"', '@"')
            count += 1

    # Pattern 2: @"_CONST" as a standalone string (placeholder key names)
    # Example: @"_TIPS_KEY" → @"HKEY_...\..."
    for const_name, path in consts.items():
        placeholder = f'@"{const_name}"'
        if placeholder in text:
            text = text.replace(placeholder, f'@"{path}"')
            count += 1

    # Pattern 3: @"_ver_key(ver", "SubKey") ... — Python function call leaked into C#
    # Example: RegOp.CheckDword(@"_ver_key(ver", "General"), "DisableBootToOfficeStart", 1)
    # Should become: RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\General", "DisableBootToOfficeStart", 1)
    if "_ver_key" in consts:
        base_path = consts["_ver_key"]
        # Match: @"_ver_key(ver", "SubKey")  — note the closing paren after SubKey
        pattern = r'@"_ver_key\([^"]*",\s*"([^"]+)"\)'
        def replace_ver_key(m: re.Match) -> str:
            sub_key = m.group(1)
            return f'@"{base_path}\\{sub_key}"'
        new_text = re.sub(pattern, replace_ver_key, text)
        if new_text != text:
            text = new_text
            count += 1

    # Pattern 4: @"_svc(name", ...) — lambda call patterns
    if "_svc" in consts:
        svc_template = consts["_svc"]
        # Match: @"_svc("ServiceName")" or similar
        pattern = r'@"_svc\("([^"]+)"\)"'
        def replace_svc(m: re.Match) -> str:
            svc_name = m.group(1)
            resolved = svc_template.replace("{name}", svc_name).replace("{" + "name" + "}", svc_name)
            return f'@"{resolved}"'
        new_text = re.sub(pattern, replace_svc, text)
        if new_text != text:
            text = new_text
            count += 1

    # Pattern 5: Fix @"_key" standalone (lowercase, usually from key = r"...")
    if "_key" in consts:
        text_new = text.replace('@"_key"', f'@"{consts["_key"]}"')
        if text_new != text:
            text = text_new
            count += 1

    # Pattern 6: Fix unescaped backslashes in Description strings
    # Find Description = "..." lines with backslashes that aren't @"..."
    def fix_description(m: re.Match) -> str:
        return f'Description = @"{m.group(1)}"'
    
    new_text = re.sub(
        r'Description = "([^"]*\\[^"]*)"',
        fix_description,
        text
    )
    if new_text != text:
        text = new_text
        count += 1

    if text != original:
        cs_file.write_text(text, encoding="utf-8")
    return count


def main():
    total_fixes = 0
    files_fixed = 0
    
    for py_file in sorted(PY_DIR.glob("*.py")):
        if py_file.name.startswith("_"):
            continue
        
        stem = py_file.stem
        cs_name = py_to_cs_name(stem) + ".cs"
        cs_file = CS_DIR / cs_name

        if not cs_file.exists():
            print(f"SKIP {py_file.name} → {cs_name} (C# file does not exist)")
            continue

        consts = extract_py_constants(py_file)
        if not consts:
            # Still try to fix Description escapes
            fixes = fix_cs_file(cs_file, {})
            if fixes:
                total_fixes += fixes
                files_fixed += 1
                print(f"  {cs_name}: {fixes} description fix(es)")
            continue

        fixes = fix_cs_file(cs_file, consts)
        if fixes > 0:
            total_fixes += fixes
            files_fixed += 1
            print(f"  {cs_name}: {fixes} fix(es) ({len(consts)} constants mapped)")
        else:
            # Check if any placeholders remain unresolved
            cs_text = cs_file.read_text(encoding="utf-8")
            remaining = re.findall(r'@"(_[A-Z_][A-Z_0-9]*)"', cs_text)
            if remaining:
                unique = set(remaining)
                print(f"  {cs_name}: UNRESOLVED placeholders: {unique}")

    print(f"\nTotal: {total_fixes} fixes across {files_fixed} files")


if __name__ == "__main__":
    main()
