"""Fix remaining @"_key" and @"_svc" placeholders by using RegistryKeys from same TweakDef.

These are local variable references that leaked from Python functions into C#.
The correct path is already in the RegistryKeys array of the same TweakDef block.
"""

import re
from pathlib import Path

CS_DIR = Path(r"c:\Users\ryair\OneDrive - Intel Corporation\Documents\MyScripts\RegiLattice\src\RegiLattice.Core\Tweaks")


def fix_local_placeholders(cs_file: Path) -> int:
    text = cs_file.read_text(encoding="utf-8")
    if '@"_key"' not in text and '@"_svc"' not in text:
        return 0

    # Parse each TweakDef block and extract RegistryKeys + placeholder usage
    # Strategy: find each "new TweakDef" block, extract RegistryKeys[0], replace @"_key"/@"_svc"
    
    lines = text.split("\n")
    fixes = 0
    in_tweakdef = False
    registry_key = None
    placeholder_lines: list[int] = []
    brace_depth = 0
    tweakdef_start = -1

    for i, line in enumerate(lines):
        if "new TweakDef" in line:
            in_tweakdef = True
            tweakdef_start = i
            registry_key = None
            placeholder_lines = []
            brace_depth = 0

        if in_tweakdef:
            brace_depth += line.count("{") - line.count("}")
            
            # Extract RegistryKeys path
            rk_match = re.search(r'RegistryKeys\s*=\s*\[@"([^"]+)"', line)
            if rk_match:
                registry_key = rk_match.group(1)

            # Track lines with @"_key" or @"_svc"
            if '@"_key"' in line or '@"_svc"' in line:
                placeholder_lines.append(i)

            # End of TweakDef block
            if brace_depth <= 0 and i > tweakdef_start + 2:
                # Apply fixes
                if registry_key and placeholder_lines:
                    for pl in placeholder_lines:
                        lines[pl] = lines[pl].replace('@"_key"', f'@"{registry_key}"')
                        lines[pl] = lines[pl].replace('@"_svc"', f'@"{registry_key}"')
                        fixes += 1
                in_tweakdef = False

    if fixes > 0:
        cs_file.write_text("\n".join(lines), encoding="utf-8")
    return fixes


def main():
    total = 0
    for cs_file in sorted(CS_DIR.glob("*.cs")):
        fixes = fix_local_placeholders(cs_file)
        if fixes:
            print(f"  {cs_file.name}: {fixes} local placeholder fix(es)")
            total += fixes
    print(f"\nTotal: {total} local placeholder fixes")


if __name__ == "__main__":
    main()
