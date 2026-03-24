namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class SidebarGadgetsPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Sidebar";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "sidebar-turn-off-sidebar",
            Label = "Sidebar Policy: Turn Off Windows Sidebar",
            Category = "Sidebar & Gadgets Policy",
            Description = "Disables the Windows Sidebar and all desktop gadgets via Group Policy. Prevents users from running the sidebar process (Sidebar.exe) on Windows Vista/7/8 and legacy gadgets on Windows 10/11.",
            Tags = ["sidebar", "gadgets", "legacy", "disable", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "TurnOffSidebar", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "TurnOffSidebar")],
            DetectOps = [RegOp.CheckDword(Key, "TurnOffSidebar", 1)],
        },
        new TweakDef
        {
            Id = "sidebar-block-unsupported-packages",
            Label = "Sidebar Policy: Block Unsupported Gadget Packages",
            Category = "Sidebar & Gadgets Policy",
            Description = "Prevents execution of gadget packages that are not explicitly supported by the installed Windows version. Unsupported gadget packages can contain vulnerabilities or unsigned code.",
            Tags = ["sidebar", "gadgets", "packages", "block", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "TurnOffUnsupportedPackages", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "TurnOffUnsupportedPackages")],
            DetectOps = [RegOp.CheckDword(Key, "TurnOffUnsupportedPackages", 1)],
        },
        new TweakDef
        {
            Id = "sidebar-disable-user-gadgets",
            Label = "Sidebar Policy: Disable Per-User Gadget Execution",
            Category = "Sidebar & Gadgets Policy",
            Description = "Prevents individual users from installing and running desktop gadgets. Removes gadgets from the right-click desktop context menu and disables the gadget installation dialog.",
            Tags = ["sidebar", "gadgets", "user", "disable", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableUserGadgets", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableUserGadgets")],
            DetectOps = [RegOp.CheckDword(Key, "DisableUserGadgets", 1)],
        },
        new TweakDef
        {
            Id = "sidebar-disable-auto-update",
            Label = "Sidebar Policy: Disable Gadget Metadata Auto-Update",
            Category = "Sidebar & Gadgets Policy",
            Description = "Prevents Windows gadgets from automatically downloading updated metadata from the Windows Live Gallery or third-party gadget feeds. Reduces network activity and potential data exfiltration.",
            Tags = ["sidebar", "gadgets", "auto-update", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "TurnOffAutoUpdate", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "TurnOffAutoUpdate")],
            DetectOps = [RegOp.CheckDword(Key, "TurnOffAutoUpdate", 1)],
        },
        new TweakDef
        {
            Id = "sidebar-block-from-running",
            Label = "Sidebar Policy: Block Sidebar Process from Running",
            Category = "Sidebar & Gadgets Policy",
            Description = "Blocks the Windows Sidebar process (sidebar.exe) from launching. CVE-2013-0088 and other CVEs affect Windows gadgets — disabling the process is a defence-in-depth mitigation.",
            Tags = ["sidebar", "gadgets", "block", "process", "cve", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "BlockFromRunning", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockFromRunning")],
            DetectOps = [RegOp.CheckDword(Key, "BlockFromRunning", 1)],
        },
        new TweakDef
        {
            Id = "sidebar-require-signed-packages",
            Label = "Sidebar Policy: Require Signed Gadget Packages",
            Category = "Sidebar & Gadgets Policy",
            Description = "Enforces digital signature verification for all gadget packages before execution. Prevents loading of unsigned or tampered gadgets that could execute arbitrary code.",
            Tags = ["sidebar", "gadgets", "signatures", "signing", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RequireGadgetSignatures", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireGadgetSignatures")],
            DetectOps = [RegOp.CheckDword(Key, "RequireGadgetSignatures", 1)],
        },
        new TweakDef
        {
            Id = "sidebar-disable-web-gadgets",
            Label = "Sidebar Policy: Disable Internet-Connected Gadgets",
            Category = "Sidebar & Gadgets Policy",
            Description = "Prevents gadgets from connecting to the internet to fetch live content (weather, news, finance widgets). Eliminates a data exfiltration channel and mitigates web content injection risks.",
            Tags = ["sidebar", "gadgets", "web", "internet", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableOnlineGadgetContent", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableOnlineGadgetContent")],
            DetectOps = [RegOp.CheckDword(Key, "DisableOnlineGadgetContent", 1)],
        },
        new TweakDef
        {
            Id = "sidebar-disable-desktop-gadgets",
            Label = "Sidebar Policy: Disable Desktop Gadgets",
            Category = "Sidebar & Gadgets Policy",
            Description = "Removes desktop gadget functionality entirely, including the right-click 'Gadgets' menu entry on the desktop. Enforces a clean desktop policy on managed enterprise endpoints.",
            Tags = ["sidebar", "gadgets", "desktop", "disable", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableDesktopGadgets", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableDesktopGadgets")],
            DetectOps = [RegOp.CheckDword(Key, "DisableDesktopGadgets", 1)],
        },
        new TweakDef
        {
            Id = "sidebar-disable-third-party-gadgets",
            Label = "Sidebar Policy: Disable Third-Party Gadget Installation",
            Category = "Sidebar & Gadgets Policy",
            Description = "Prevents users from installing gadgets from third-party sources or URLs. Restricts gadget sources to the built-in Windows Gallery only, reducing the attack surface via malicious gadget packages.",
            Tags = ["sidebar", "gadgets", "third-party", "installation", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AllowThirdPartyGadgets", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowThirdPartyGadgets")],
            DetectOps = [RegOp.CheckDword(Key, "AllowThirdPartyGadgets", 0)],
        },
        new TweakDef
        {
            Id = "sidebar-disable-gadget-gallery",
            Label = "Sidebar Policy: Disable Gadget Gallery Access",
            Category = "Sidebar & Gadgets Policy",
            Description = "Prevents access to the Windows Gadget Gallery (the built-in gadget browser). Removes the ability to browse and install new gadgets from both the OS gallery and online sources.",
            Tags = ["sidebar", "gadgets", "gallery", "lockdown", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableGadgetGallery", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableGadgetGallery")],
            DetectOps = [RegOp.CheckDword(Key, "DisableGadgetGallery", 1)],
        },
    ];
}
