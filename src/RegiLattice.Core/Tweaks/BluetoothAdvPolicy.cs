#nullable enable
using System.Collections.Generic;
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

// Bluetooth Advertising Policy — controls whether Bluetooth adapters can broadcast
// advertising packets (BLE advertising), discoverability, and related privacy settings.
// HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Bluetooth
// HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters
// HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthA2dp\Parameters
internal static class BluetoothAdvPolicy
{
    private const string BtPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Bluetooth";
    private const string BthPort = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters";
    private const string BtHub = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\BluetoothDeviceEnumerator";
    private const string BtPrivacy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\BlueTooth";
    private const string BtPhoneBook = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\BlueTooth\PhoneBook";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "btadv-disable-bluetooth-advertising",
            Label = "BT Advertising: Disable Bluetooth Advertising (BLE Beacon)",
            Category = "Bluetooth Advertising Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [BtPolicy],
            Tags = ["bluetooth", "advertising", "ble", "beacon", "privacy", "security"],
            Description =
                "Sets DisableAdvertising=1 in Bluetooth policy. Stops the Bluetooth adapter from "
                + "broadcasting advertising packets (BLE beacon). Prevents passive tracking and "
                + "reduces RF attack surface. Default: advertising enabled.",
            ApplyOps = [RegOp.SetDword(BtPolicy, "DisableAdvertising", 1)],
            RemoveOps = [RegOp.DeleteValue(BtPolicy, "DisableAdvertising")],
            DetectOps = [RegOp.CheckDword(BtPolicy, "DisableAdvertising", 1)],
        },
        new TweakDef
        {
            Id = "btadv-disable-promiscuous-mode",
            Label = "BT Advertising: Disable Bluetooth Promiscuous Mode",
            Category = "Bluetooth Advertising Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [BthPort],
            Tags = ["bluetooth", "promiscuous", "sniffing", "security", "hardening"],
            Description =
                "Sets PromiscuousMode=0 in BTHPORT Parameters. Prevents the Bluetooth adapter from "
                + "entering promiscuous receive mode which would capture all BT packets in range. "
                + "Default: 0 (already off). Explicit enforcement ensures the value is never changed.",
            ApplyOps = [RegOp.SetDword(BthPort, "PromiscuousMode", 0)],
            RemoveOps = [RegOp.DeleteValue(BthPort, "PromiscuousMode")],
            DetectOps = [RegOp.CheckDword(BthPort, "PromiscuousMode", 0)],
        },
        new TweakDef
        {
            Id = "btadv-disable-bluetooth-pairing-notification",
            Label = "BT Advertising: Disable Bluetooth Auto-Pairing Notifications",
            Category = "Bluetooth Advertising Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [BtPolicy],
            Tags = ["bluetooth", "pairing", "notification", "lockdown", "security"],
            Description =
                "Sets DisablePairingNotifications=1 in Bluetooth policy. Suppresses automatic "
                + "pairing prompts when Bluetooth devices are discovered nearby. "
                + "Default: notifications enabled. Prevents social engineering via proximity.",
            ApplyOps = [RegOp.SetDword(BtPolicy, "DisablePairingNotifications", 1)],
            RemoveOps = [RegOp.DeleteValue(BtPolicy, "DisablePairingNotifications")],
            DetectOps = [RegOp.CheckDword(BtPolicy, "DisablePairingNotifications", 1)],
        },
        new TweakDef
        {
            Id = "btadv-set-connectable-timeout-short",
            Label = "BT Advertising: Limit Bluetooth Discoverable/Connectable Timeout",
            Category = "Bluetooth Advertising Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [BthPort],
            Tags = ["bluetooth", "discoverable", "timeout", "privacy", "security"],
            Description =
                "Sets ConnectableTimeout=30 in BTHPORT Parameters. Limits how long the adapter "
                + "remains in connectable mode after being made visible. "
                + "Default: 180 seconds. Shorter window reduces passive attack exposure.",
            ApplyOps = [RegOp.SetDword(BthPort, "ConnectableTimeout", 30)],
            RemoveOps = [RegOp.DeleteValue(BthPort, "ConnectableTimeout")],
            DetectOps = [RegOp.CheckDword(BthPort, "ConnectableTimeout", 30)],
        },
        new TweakDef
        {
            Id = "btadv-disable-bluetooth-file-transfer",
            Label = "BT Advertising: Disable Bluetooth OBEX File Transfer",
            Category = "Bluetooth Advertising Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [BtPolicy],
            Tags = ["bluetooth", "obex", "file-transfer", "security", "lockdown"],
            Description =
                "Sets DisableFileTransfer=1 in Bluetooth policy. Blocks OBEX-based file exchange "
                + "over Bluetooth (Push and FTP profiles). Prevents data exfiltration via wireless. "
                + "Default: file transfer enabled. Recommended in high-security environments.",
            ApplyOps = [RegOp.SetDword(BtPolicy, "DisableFileTransfer", 1)],
            RemoveOps = [RegOp.DeleteValue(BtPolicy, "DisableFileTransfer")],
            DetectOps = [RegOp.CheckDword(BtPolicy, "DisableFileTransfer", 1)],
        },
        new TweakDef
        {
            Id = "btadv-disable-bluetooth-phonebook-access",
            Label = "BT Advertising: Disable Bluetooth Phone Book Access",
            Category = "Bluetooth Advertising Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [BtPolicy],
            Tags = ["bluetooth", "phonebook", "pbap", "privacy", "security"],
            Description =
                "Sets DisablePhoneBookAccess=1 in Bluetooth policy. Blocks the Phone Book Access "
                + "Profile (PBAP). Prevents paired devices from reading local contacts. "
                + "Default: PBAP enabled. Disabling protects contact data from BT-paired devices.",
            ApplyOps = [RegOp.SetDword(BtPolicy, "DisablePhoneBookAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(BtPolicy, "DisablePhoneBookAccess")],
            DetectOps = [RegOp.CheckDword(BtPolicy, "DisablePhoneBookAccess", 1)],
        },
        new TweakDef
        {
            Id = "btadv-require-bt-encryption",
            Label = "BT Advertising: Require Encryption on Bluetooth Connections",
            Category = "Bluetooth Advertising Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [BthPort],
            Tags = ["bluetooth", "encryption", "security", "hardening"],
            Description =
                "Sets EncryptionEnabled=1 in BTHPORT Parameters. Enforces that all Bluetooth "
                + "connections use link-layer encryption. Unencrypted pairing attempts are rejected. "
                + "Default: optional. Explicit enforcement prevents plaintext BT sessions.",
            ApplyOps = [RegOp.SetDword(BthPort, "EncryptionEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(BthPort, "EncryptionEnabled")],
            DetectOps = [RegOp.CheckDword(BthPort, "EncryptionEnabled", 1)],
        },
        new TweakDef
        {
            Id = "btadv-disable-remote-audio-playback",
            Label = "BT Advertising: Disable Remote Audio Playback over Bluetooth",
            Category = "Bluetooth Advertising Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [BtPolicy],
            Tags = ["bluetooth", "audio", "a2dp", "remote", "security", "lockdown"],
            Description =
                "Sets DisableRemoteAudioPlayback=1 in Bluetooth policy. Prevents audio streaming "
                + "to remote Bluetooth devices (A2DP sink). Disables Bluetooth speakers as data channel. "
                + "Default: audio playback allowed. Recommended in air-gapped/classified environments.",
            ApplyOps = [RegOp.SetDword(BtPolicy, "DisableRemoteAudioPlayback", 1)],
            RemoveOps = [RegOp.DeleteValue(BtPolicy, "DisableRemoteAudioPlayback")],
            DetectOps = [RegOp.CheckDword(BtPolicy, "DisableRemoteAudioPlayback", 1)],
        },
        new TweakDef
        {
            Id = "btadv-disable-bt-discoverable-state",
            Label = "BT Advertising: Force Bluetooth Always Non-Discoverable",
            Category = "Bluetooth Advertising Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [BtPolicy],
            Tags = ["bluetooth", "discoverable", "visibility", "privacy", "security"],
            Description =
                "Sets ForceNonDiscoverable=1 in Bluetooth policy. Keeps the Bluetooth adapter in "
                + "non-discoverable state at all times. Prevents detection from BT scanning tools. "
                + "Default: users can toggle discoverability. Policy lock prevents exposure.",
            ApplyOps = [RegOp.SetDword(BtPolicy, "ForceNonDiscoverable", 1)],
            RemoveOps = [RegOp.DeleteValue(BtPolicy, "ForceNonDiscoverable")],
            DetectOps = [RegOp.CheckDword(BtPolicy, "ForceNonDiscoverable", 1)],
        },
        new TweakDef
        {
            Id = "btadv-disable-bluetooth-shared-experiences",
            Label = "BT Advertising: Disable Bluetooth Shared Experiences",
            Category = "Bluetooth Advertising Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [BtPolicy],
            Tags = ["bluetooth", "shared-experiences", "nearby-share", "privacy", "security"],
            Description =
                "Sets DisableSharedExperiences=1 in Bluetooth policy. Blocks the Bluetooth-based "
                + "'Shared Experiences' feature which is used for Nearby Share file transfers. "
                + "Default: enabled. Disabling removes an additional passive data transfer vector.",
            ApplyOps = [RegOp.SetDword(BtPolicy, "DisableSharedExperiences", 1)],
            RemoveOps = [RegOp.DeleteValue(BtPolicy, "DisableSharedExperiences")],
            DetectOps = [RegOp.CheckDword(BtPolicy, "DisableSharedExperiences", 1)],
        },
    ];
}
