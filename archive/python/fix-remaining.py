"""Fix remaining Explorer.cs and Office.cs corrupted patterns."""

import re
from pathlib import Path

CS_DIR = Path(r"c:\Users\ryair\OneDrive - Intel Corporation\Documents\MyScripts\RegiLattice\src\RegiLattice.Core\Tweaks")

# Fix Explorer.cs - remove @"_EXP@" prefix from verbatim strings
ef = CS_DIR / "Explorer.cs"
t = ef.read_text("utf-8")
old_count = t.count('@"_EXP@"HKEY_')
t2 = t.replace('@"_EXP@"HKEY_', '@"HKEY_')
if t2 != t:
    ef.write_text(t2, "utf-8")
    print(f"Explorer.cs: fixed {old_count} corrupted verbatim strings")
else:
    print("Explorer.cs: no change")

# Fix Office.cs - replace @"_ver_key(ver", "SubKey") patterns
of = CS_DIR / "Office.cs"
t = of.read_text("utf-8")
old_count = len(re.findall(r'_ver_key', t))

def fix_ver(m: re.Match) -> str:
    sub = m.group(1)
    return f'@"HKEY_CURRENT_USER\\Software\\Microsoft\\Office\\16.0\\Common\\{sub}"'

t2 = re.sub(r'@"_ver_key\(ver",\s*"([^"]+)"\)', fix_ver, t)
if t2 != t:
    of.write_text(t2, "utf-8")
    print(f"Office.cs: fixed {old_count} _ver_key patterns")
else:
    print("Office.cs: no change")
