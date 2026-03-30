// RegiLattice.Core — Tweaks/WindowsDeploymentServicesPolicy.cs
// Windows Deployment Services (WDS) PXE boot and image deployment policy — Sprint 635.
// Category: "Windows Deployment Services Policy" | Slug: wds
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\WDS

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WindowsDeploymentServicesPolicy
{
    private const string SvcKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDS\Server";
    private const string PxeKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDS\PXE";
    private const string TransKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDS\Transport";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id           = "wds-require-admin-approval",
            Label        = "Require Admin Approval for PXE Boot Clients",
            Category     = "Windows Deployment Services Policy",
            Description  = "Requires administrator approval before unknown PXE clients can boot from WDS. Prevents unauthorised devices from imaging. Default: auto-approve.",
            Tags         = ["wds", "pxe", "security", "approval", "deployment", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 4,
            ImpactNote   = "Unknown PXE clients are held pending admin approval; known-device imaging unaffected.",
            ApplyOps     = [RegOp.SetDword(PxeKey, "RequireAdminApproval", 1)],
            RemoveOps    = [RegOp.DeleteValue(PxeKey, "RequireAdminApproval")],
            DetectOps    = [RegOp.CheckDword(PxeKey, "RequireAdminApproval", 1)],
        },
        new TweakDef
        {
            Id           = "wds-disable-unknown-pxe",
            Label        = "Block Unknown Clients from PXE Boot",
            Category     = "Windows Deployment Services Policy",
            Description  = "Prevents unknown (non-pre-staged) computers from performing PXE boot via WDS. Only pre-staged/known devices can image. Default: allow all.",
            Tags         = ["wds", "pxe", "security", "unknown-clients", "deployment", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 5,
            SafetyRating = 3,
            ImpactNote   = "Only pre-staged devices can PXE boot; new devices must be pre-staged in AD first.",
            ApplyOps     = [RegOp.SetDword(PxeKey, "AllowUnknownClients", 0)],
            RemoveOps    = [RegOp.DeleteValue(PxeKey, "AllowUnknownClients")],
            DetectOps    = [RegOp.CheckDword(PxeKey, "AllowUnknownClients", 0)],
        },
        new TweakDef
        {
            Id           = "wds-enable-pxe-prompt",
            Label        = "Enable PXE Boot Key Press Prompt",
            Category     = "Windows Deployment Services Policy",
            Description  = "Requires the user to press a key (e.g., F12) to initiate PXE boot. Prevents automatic network boot on every startup. Default: may auto-boot.",
            Tags         = ["wds", "pxe", "prompt", "boot", "deployment", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Users must press a key to PXE boot; prevents accidental reimaging.",
            ApplyOps     = [RegOp.SetDword(PxeKey, "PxePromptPolicy", 1)],
            RemoveOps    = [RegOp.DeleteValue(PxeKey, "PxePromptPolicy")],
            DetectOps    = [RegOp.CheckDword(PxeKey, "PxePromptPolicy", 1)],
        },
        new TweakDef
        {
            Id           = "wds-set-pxe-timeout",
            Label        = "Set PXE Prompt Timeout to 10 Seconds",
            Category     = "Windows Deployment Services Policy",
            Description  = "Sets the PXE boot key-press prompt timeout to 10 seconds. After timeout, the device continues to local disk boot. Default: varies by BIOS.",
            Tags         = ["wds", "pxe", "timeout", "boot", "deployment", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 2,
            SafetyRating = 5,
            ImpactNote   = "10-second window for PXE boot; device falls through to local boot on timeout.",
            ApplyOps     = [RegOp.SetDword(PxeKey, "PxePromptTimeout", 10)],
            RemoveOps    = [RegOp.DeleteValue(PxeKey, "PxePromptTimeout")],
            DetectOps    = [RegOp.CheckDword(PxeKey, "PxePromptTimeout", 10)],
        },
        new TweakDef
        {
            Id           = "wds-enable-logging",
            Label        = "Enable WDS Deployment Event Logging",
            Category     = "Windows Deployment Services Policy",
            Description  = "Enables detailed event logging for WDS deployment operations. Provides audit trail of which devices were imaged and when. Default: minimal logging.",
            Tags         = ["wds", "logging", "audit", "deployment", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Detailed WDS deployment events written to event log; slight disk overhead.",
            ApplyOps     = [RegOp.SetDword(SvcKey, "EnableLogging", 1)],
            RemoveOps    = [RegOp.DeleteValue(SvcKey, "EnableLogging")],
            DetectOps    = [RegOp.CheckDword(SvcKey, "EnableLogging", 1)],
        },
        new TweakDef
        {
            Id           = "wds-set-multicast-transfer-mode",
            Label        = "Set WDS Multicast Transfer to Auto Mode",
            Category     = "Windows Deployment Services Policy",
            Description  = "Configures multicast image transfers to auto-select between multicast and unicast based on network conditions. Default: multicast only.",
            Tags         = ["wds", "multicast", "network", "deployment", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 4,
            ImpactNote   = "WDS auto-selects best transfer mode; improves reliability on networks without multicast support.",
            ApplyOps     = [RegOp.SetDword(TransKey, "TransferMode", 1)],
            RemoveOps    = [RegOp.DeleteValue(TransKey, "TransferMode")],
            DetectOps    = [RegOp.CheckDword(TransKey, "TransferMode", 1)],
        },
        new TweakDef
        {
            Id           = "wds-set-multicast-session-threshold",
            Label        = "Set Multicast Session Client Threshold to 10",
            Category     = "Windows Deployment Services Policy",
            Description  = "Sets the minimum number of clients before a multicast session starts. Prevents starting a multicast session for only 1–2 clients. Default: 1.",
            Tags         = ["wds", "multicast", "threshold", "deployment", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 2,
            SafetyRating = 5,
            ImpactNote   = "Multicast waits for 10 clients before starting; single clients use unicast fallback.",
            ApplyOps     = [RegOp.SetDword(TransKey, "MulticastSessionThreshold", 10)],
            RemoveOps    = [RegOp.DeleteValue(TransKey, "MulticastSessionThreshold")],
            DetectOps    = [RegOp.CheckDword(TransKey, "MulticastSessionThreshold", 10)],
        },
        new TweakDef
        {
            Id           = "wds-enable-tftp-window-size",
            Label        = "Set WDS TFTP Block Size to 16384",
            Category     = "Windows Deployment Services Policy",
            Description  = "Increases the TFTP block size used by WDS PXE boot to 16384 bytes. Improves image download speed on modern networks. Default: 1456 (standard TFTP block).",
            Tags         = ["wds", "tftp", "performance", "pxe", "deployment", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 4,
            ImpactNote   = "Faster PXE image downloads; may fail on networks with low MTU or NAT.",
            ApplyOps     = [RegOp.SetDword(TransKey, "TftpBlockSize", 16384)],
            RemoveOps    = [RegOp.DeleteValue(TransKey, "TftpBlockSize")],
            DetectOps    = [RegOp.CheckDword(TransKey, "TftpBlockSize", 16384)],
        },
        new TweakDef
        {
            Id           = "wds-disable-dhcp-option-60",
            Label        = "Disable DHCP Option 60 (PXEClient Class ID)",
            Category     = "Windows Deployment Services Policy",
            Description  = "Prevents WDS from adding DHCP Option 60 (PXEClient class identifier) to DHCP responses. Use when WDS is co-located with DHCP to avoid conflicts. Default: enabled.",
            Tags         = ["wds", "dhcp", "pxe", "network", "deployment", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 3,
            ImpactNote   = "Prevents DHCP conflict when WDS and DHCP share the same server; PXE may need DHCP Option 66/67 instead.",
            ApplyOps     = [RegOp.SetDword(PxeKey, "UseDhcpPorts", 0)],
            RemoveOps    = [RegOp.DeleteValue(PxeKey, "UseDhcpPorts")],
            DetectOps    = [RegOp.CheckDword(PxeKey, "UseDhcpPorts", 0)],
        },
        new TweakDef
        {
            Id           = "wds-restrict-naming-policy",
            Label        = "Enforce WDS Computer Naming Policy",
            Category     = "Windows Deployment Services Policy",
            Description  = "Enforces a server-defined computer naming policy for imaged devices. Prevents users from choosing arbitrary computer names during imaging. Default: user-chosen.",
            Tags         = ["wds", "naming", "policy", "deployment", "standardisation"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Imaged computers get server-defined names; ensures naming convention compliance.",
            ApplyOps     = [RegOp.SetDword(SvcKey, "NamingPolicy", 1)],
            RemoveOps    = [RegOp.DeleteValue(SvcKey, "NamingPolicy")],
            DetectOps    = [RegOp.CheckDword(SvcKey, "NamingPolicy", 1)],
        },
    ];
}
