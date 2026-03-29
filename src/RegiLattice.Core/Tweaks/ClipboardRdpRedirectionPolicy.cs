// RegiLattice.Core — Tweaks/ClipboardRdpRedirectionPolicy.cs
// Clipboard and device redirection control in RDP / Terminal Services via Group Policy — Sprint 443.
// Disables clipboard, drive, printer, COM port, LPT port, smart card, audio recording,
// clipboard file transfer, USB, and PnP device redirection in remote desktop sessions.
// Category: "Clipboard RDP Redirection" | Slug: cliprdp
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class ClipboardRdpRedirectionPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "cliprdp-disable-clipboard-redirection",
                Label = "Disable Clipboard Redirection in RDP Sessions",
                Category = "Clipboard RDP Redirection",
                Description =
                    "Sets fDisableClip=1 to prevent clipboard contents from being shared between the RDP client and the remote session, blocking data exfiltration via clipboard.",
                Tags = ["rdp", "clipboard", "redirection", "hardening", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Clipboard copy-paste between local and remote session is blocked. Copy-paste within the session still works.",
                ApplyOps = [RegOp.SetDword(Key, "fDisableClip", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "fDisableClip")],
                DetectOps = [RegOp.CheckDword(Key, "fDisableClip", 1)],
            },
            new TweakDef
            {
                Id = "cliprdp-disable-drive-redirection",
                Label = "Disable Drive Redirection in RDP Sessions",
                Category = "Clipboard RDP Redirection",
                Description =
                    "Sets fDisableCdm=1 to prevent local drives from being mapped into the remote session, blocking file transfer via mapped drives.",
                Tags = ["rdp", "drive", "redirection", "hardening", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Local drives are not accessible inside RDP sessions; file sharing via drive mapping is blocked.",
                ApplyOps = [RegOp.SetDword(Key, "fDisableCdm", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "fDisableCdm")],
                DetectOps = [RegOp.CheckDword(Key, "fDisableCdm", 1)],
            },
            new TweakDef
            {
                Id = "cliprdp-disable-printer-redirection",
                Label = "Disable Printer Redirection in RDP Sessions",
                Category = "Clipboard RDP Redirection",
                Description =
                    "Sets fDisableCpm=1 to prevent local printers from being redirected into RDP sessions, blocking potentially sensitive print jobs from reaching the remote host.",
                Tags = ["rdp", "printer", "redirection", "hardening", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Local printers not available in RDP session; use remote printers instead.",
                ApplyOps = [RegOp.SetDword(Key, "fDisableCpm", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "fDisableCpm")],
                DetectOps = [RegOp.CheckDword(Key, "fDisableCpm", 1)],
            },
            new TweakDef
            {
                Id = "cliprdp-disable-com-port-redirection",
                Label = "Disable COM Port Redirection in RDP Sessions",
                Category = "Clipboard RDP Redirection",
                Description = "Sets fDisableCcm=1 to prevent local COM (serial) ports from being redirected into RDP sessions.",
                Tags = ["rdp", "com-port", "redirection", "hardening", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Local COM ports not accessible in RDP; serial device data cannot be exfiltrated.",
                ApplyOps = [RegOp.SetDword(Key, "fDisableCcm", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "fDisableCcm")],
                DetectOps = [RegOp.CheckDword(Key, "fDisableCcm", 1)],
            },
            new TweakDef
            {
                Id = "cliprdp-disable-lpt-redirection",
                Label = "Disable LPT Port Redirection in RDP Sessions",
                Category = "Clipboard RDP Redirection",
                Description = "Sets fDisableLPT=1 to prevent local parallel (LPT) ports from being redirected into the remote session.",
                Tags = ["rdp", "lpt", "redirection", "hardening", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Local LPT ports not available in RDP session.",
                ApplyOps = [RegOp.SetDword(Key, "fDisableLPT", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "fDisableLPT")],
                DetectOps = [RegOp.CheckDword(Key, "fDisableLPT", 1)],
            },
            new TweakDef
            {
                Id = "cliprdp-disable-smart-card-redirection",
                Label = "Disable Smart Card Redirection in RDP Sessions",
                Category = "Clipboard RDP Redirection",
                Description =
                    "Sets fDisableSCard=1 to prevent local smart cards from being redirected into the remote session, mitigating remote authentication using locally inserted smart cards.",
                Tags = ["rdp", "smart-card", "redirection", "hardening", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Smart cards not forwarded into remote session; use remote card readers instead.",
                ApplyOps = [RegOp.SetDword(Key, "fDisableSCard", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "fDisableSCard")],
                DetectOps = [RegOp.CheckDword(Key, "fDisableSCard", 1)],
            },
            new TweakDef
            {
                Id = "cliprdp-disable-audio-recording-redirection",
                Label = "Disable Audio Recording Redirection in RDP",
                Category = "Clipboard RDP Redirection",
                Description = "Sets fDisableAudioCapture=1 to prevent the remote session from recording audio through the local client's microphone.",
                Tags = ["rdp", "audio", "microphone", "redirection", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Remote session cannot access local microphone; audio recording in session is disabled.",
                ApplyOps = [RegOp.SetDword(Key, "fDisableAudioCapture", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "fDisableAudioCapture")],
                DetectOps = [RegOp.CheckDword(Key, "fDisableAudioCapture", 1)],
            },
            new TweakDef
            {
                Id = "cliprdp-disable-clipboard-file-copy",
                Label = "Disable Clipboard File Copy from RDP Session",
                Category = "Clipboard RDP Redirection",
                Description =
                    "Disables the ability to copy files via clipboard between the remote session and the local desktop, supplementing fDisableClip for file-drag exfiltration prevention.",
                Tags = ["rdp", "clipboard", "file-copy", "hardening", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "File drag-and-drop and clipboard file copy blocked between sessions.",
                ApplyOps = [RegOp.SetDword(Key, "fDisableClipboardFileTransfer", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "fDisableClipboardFileTransfer")],
                DetectOps = [RegOp.CheckDword(Key, "fDisableClipboardFileTransfer", 1)],
            },
            new TweakDef
            {
                Id = "cliprdp-disable-usb-redirection",
                Label = "Disable USB Device Redirection in RDP Sessions",
                Category = "Clipboard RDP Redirection",
                Description = "Disables USB device redirection so that locally connected USB devices are not forwarded into the remote session.",
                Tags = ["rdp", "usb", "redirection", "hardening", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "USB devices not available in remote session; prevents USB-based data exfiltration.",
                ApplyOps = [RegOp.SetDword(Key, "fDisableUSBRedir", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "fDisableUSBRedir")],
                DetectOps = [RegOp.CheckDword(Key, "fDisableUSBRedir", 1)],
            },
            new TweakDef
            {
                Id = "cliprdp-disable-pnp-redirection",
                Label = "Disable PnP Device Redirection in RDP Sessions",
                Category = "Clipboard RDP Redirection",
                Description =
                    "Disables Plug-and-Play device redirection so locally connected PnP devices (cameras, scanners, etc.) are not accessible from the remote session.",
                Tags = ["rdp", "pnp", "redirection", "hardening", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "PnP devices not forwarded into remote session; cameras and scanners inaccessible.",
                ApplyOps = [RegOp.SetDword(Key, "fDisablePNPRedir", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "fDisablePNPRedir")],
                DetectOps = [RegOp.CheckDword(Key, "fDisablePNPRedir", 1)],
            },
        ];
}
