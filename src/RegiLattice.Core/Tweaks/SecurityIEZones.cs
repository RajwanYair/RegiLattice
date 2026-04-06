namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class SecurityIEZones
{
    private const string Zone4 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\Internet Settings\Zones\4";

    private const string Zone1 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\Internet Settings\Zones\1";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "iezone-rzone-block-activex",
                Label = "Block ActiveX Controls in Restricted Sites Zone",
                Category = "Security — IE Zone Policy",
                Description =
                    "Disables all ActiveX plug-ins and controls in the IE/Edge Restricted Sites zone (Zone 4). "
                    + "ActiveX in Restricted Sites is a major malware delivery vector; this should always be disabled. "
                    + "Default: may be allowed (3=Disable). Recommended: 3.",
                Tags = ["ie-zone", "activex", "restricted-sites", "browser-security", "internet-settings"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(Zone4, "1200", 3)],
                RemoveOps = [RegOp.DeleteValue(Zone4, "1200")],
                DetectOps = [RegOp.CheckDword(Zone4, "1200", 3)],
            },
            new TweakDef
            {
                Id = "iezone-rzone-block-unsafe-activex",
                Label = "Block Unsafe ActiveX Init in Restricted Sites Zone",
                Category = "Security — IE Zone Policy",
                Description =
                    "Disables the ability to initialise and script ActiveX controls not marked as safe for scripting in Zone 4. "
                    + "Such controls bypass SafeForScripting protections and are used in exploitation chains. "
                    + "Default: may be allowed or prompted. Recommended: 3 (Disable).",
                Tags = ["ie-zone", "activex", "unsafe", "restricted-sites", "cve", "exploitation"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(Zone4, "1201", 3)],
                RemoveOps = [RegOp.DeleteValue(Zone4, "1201")],
                DetectOps = [RegOp.CheckDword(Zone4, "1201", 3)],
            },
            new TweakDef
            {
                Id = "iezone-rzone-block-active-scripting",
                Label = "Block Active Scripting in Restricted Sites Zone",
                Category = "Security — IE Zone Policy",
                Description =
                    "Disables JavaScript and VBScript execution in the IE/Edge Restricted Sites zone. "
                    + "Scripting in Zone 4 is the most common drive-by malware delivery path. "
                    + "Default: may be allowed. Recommended: 3 (Disable).",
                Tags = ["ie-zone", "javascript", "active-scripting", "restricted-sites", "xss"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(Zone4, "1400", 3)],
                RemoveOps = [RegOp.DeleteValue(Zone4, "1400")],
                DetectOps = [RegOp.CheckDword(Zone4, "1400", 3)],
            },
            new TweakDef
            {
                Id = "iezone-rzone-block-clipboard-script",
                Label = "Block Script Clipboard Access in Restricted Sites Zone",
                Category = "Security — IE Zone Policy",
                Description =
                    "Prevents scripts in Zone 4 from reading or writing the system clipboard. "
                    + "Script clipboard access can be used to exfiltrate copied sensitive data from a web page. "
                    + "Default: may prompt. Recommended: 3 (Disable).",
                Tags = ["ie-zone", "clipboard", "script", "restricted-sites", "data-exfiltration"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(Zone4, "1406", 3)],
                RemoveOps = [RegOp.DeleteValue(Zone4, "1406")],
                DetectOps = [RegOp.CheckDword(Zone4, "1406", 3)],
            },
            new TweakDef
            {
                Id = "iezone-rzone-block-frame-nav",
                Label = "Block Cross-Domain Frame Navigation in Restricted Sites Zone",
                Category = "Security — IE Zone Policy",
                Description =
                    "Prevents frames and windows in Zone 4 from navigating to content in different domains. "
                    + "Cross-domain frame navigation enables clickjacking and framing-based phishing attacks. "
                    + "Default: may be allowed. Recommended: 3 (Disable).",
                Tags = ["ie-zone", "frame-navigation", "clickjacking", "restricted-sites", "cross-domain"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(Zone4, "1607", 3)],
                RemoveOps = [RegOp.DeleteValue(Zone4, "1607")],
                DetectOps = [RegOp.CheckDword(Zone4, "1607", 3)],
            },
            new TweakDef
            {
                Id = "iezone-rzone-block-subframe-cross-domain",
                Label = "Block Subframe Cross-Domain Navigation in Restricted Sites Zone",
                Category = "Security — IE Zone Policy",
                Description =
                    "Disables subframe navigation across different domains in Zone 4. "
                    + "Prevents embedded frames from redirecting to arbitrary external domain content even when the top frame is restricted. "
                    + "Default: may be allowed. Recommended: 3 (Disable).",
                Tags = ["ie-zone", "subframe", "restricted-sites", "iframe", "cross-domain"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(Zone4, "1803", 3)],
                RemoveOps = [RegOp.DeleteValue(Zone4, "1803")],
                DetectOps = [RegOp.CheckDword(Zone4, "1803", 3)],
            },
            new TweakDef
            {
                Id = "iezone-rzone-enable-popup-blocker",
                Label = "Enable Pop-up Blocker in Restricted Sites Zone",
                Category = "Security — IE Zone Policy",
                Description =
                    "Forces the Internet Explorer / Edge pop-up blocker ON for the Restricted Sites zone. "
                    + "Pop-ups from Zone 4 are a classic malvertising and phishing vector; blocking them is fundamental hygiene. "
                    + "Default: may not be enforced by policy. Recommended: 1 (Enable enforcement).",
                Tags = ["ie-zone", "popup-blocker", "restricted-sites", "phishing", "malvertising"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(Zone4, "1809", 1)],
                RemoveOps = [RegOp.DeleteValue(Zone4, "1809")],
                DetectOps = [RegOp.CheckDword(Zone4, "1809", 1)],
            },
            new TweakDef
            {
                Id = "iezone-intranet-block-activex",
                Label = "Block Unsigned ActiveX Controls in Local Intranet Zone",
                Category = "Security — IE Zone Policy",
                Description =
                    "Disables loading of unsigned ActiveX controls in the Local Intranet zone (Zone 1). "
                    + "Intranet zones are often trusted but can be compromised; unsigned ActiveX in Zone 1 enables lateral movement pivoting. "
                    + "Default: may be allowed. Recommended: 3 (Disable).",
                Tags = ["ie-zone", "activex", "intranet-zone", "unsigned", "lateral-movement"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ApplyOps = [RegOp.SetDword(Zone1, "1201", 3)],
                RemoveOps = [RegOp.DeleteValue(Zone1, "1201")],
                DetectOps = [RegOp.CheckDword(Zone1, "1201", 3)],
            },
            new TweakDef
            {
                Id = "iezone-intranet-block-clipboard-script",
                Label = "Block Script Clipboard Access in Local Intranet Zone",
                Category = "Security — IE Zone Policy",
                Description =
                    "Prevents scripts in the Local Intranet zone (Zone 1) from reading or writing the system clipboard. "
                    + "Even on intranet pages, script clipboard harvesting can be leveraged for data exfiltration. "
                    + "Default: may be enabled. Recommended: 3 (Disable).",
                Tags = ["ie-zone", "clipboard", "intranet-zone", "script", "data-exfiltration"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ApplyOps = [RegOp.SetDword(Zone1, "1406", 3)],
                RemoveOps = [RegOp.DeleteValue(Zone1, "1406")],
                DetectOps = [RegOp.CheckDword(Zone1, "1406", 3)],
            },
            new TweakDef
            {
                Id = "iezone-intranet-block-cross-domain-nav",
                Label = "Block Cross-Domain Navigation in Local Intranet Zone",
                Category = "Security — IE Zone Policy",
                Description =
                    "Prevents windows and frames in the Local Intranet zone from navigating across different domains. "
                    + "Limits the attack surface if a compromised intranet page tries to redirect users to external content. "
                    + "Default: intranet zone permits cross-domain navigation. Recommended: 3 (Disable).",
                Tags = ["ie-zone", "intranet-zone", "cross-domain", "frame-navigation", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ApplyOps = [RegOp.SetDword(Zone1, "1803", 3)],
                RemoveOps = [RegOp.DeleteValue(Zone1, "1803")],
                DetectOps = [RegOp.CheckDword(Zone1, "1803", 3)],
            },
        ];
}
