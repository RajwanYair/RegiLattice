// RegiLattice.Core — Tweaks/NetworkListPolicy.cs
// Sprint 293: Network List Policy tweaks (10 tweaks)
// Category: "Network List Policy" | Slug: netlst
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkList

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class NetworkListPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkList";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "netlst-delete-all-user-files-on-exit",
            Label = "Delete Network Profile Files on Exit",
            Category = "Network List Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Network profile files contain connection history and metadata about previously connected networks stored on the device. Deleting these files on exit prevents accumulation of network history that could reveal location patterns and network infrastructure details. This policy is particularly relevant for mobile devices that connect to diverse networks during travel. Removing connection history on exit limits the information available to an attacker who gains physical or logical access to the device. Enterprise security policies often require that connection history be purged to prevent network topology exposure. Standard network connectivity remains fully functional as new connection profiles are created as needed.",
            Tags = ["network-list", "privacy", "cleanup", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DeleteAllUserFilesOnExit", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DeleteAllUserFilesOnExit")],
            DetectOps = [RegOp.CheckDword(Key, "DeleteAllUserFilesOnExit", 1)],
        },
        new TweakDef
        {
            Id = "netlst-disable-safety-ui",
            Label = "Disable Network Safety UI",
            Category = "Network List Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "The network safety UI presents dialogs warning users about connecting to public or unsecured networks and requesting network location choices. Disabling this UI removes the location type selection dialog that appears when connecting to a new network. Enterprise networks are classified centrally through domain membership and network profile policies, making the interactive UI redundant. User-initiated network classification can result in incorrect security zone assignments that override intended enterprise policy. Removing the UI prevents users from inadvertently classifying enterprise networks as public or home networks. Network classification is managed deterministically through network location awareness policies and domain detection.",
            Tags = ["network-list", "ui", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableNetworkSafetyUI", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableNetworkSafetyUI")],
            DetectOps = [RegOp.CheckDword(Key, "DisableNetworkSafetyUI", 1)],
        },
        new TweakDef
        {
            Id = "netlst-disable-connected-standby",
            Label = "Disable Connected Standby Network Mode",
            Category = "Network List Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Connected standby maintains network connectivity management during device sleep states, allowing the network list to update and applications to receive push notifications. Disabling connected standby in the network list policy prevents background network classification updates during sleep. This reduces battery consumption on laptop devices that connect to multiple networks across different work locations. Enterprise devices with corporate VPN requirements often do not need continuous network awareness while sleeping. Disabling this feature eliminates the power drain associated with maintaining the network enumeration stack during sleep. Standard network operations resume immediately upon device wake without any impact on connectivity.",
            Tags = ["network-list", "standby", "power", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableConnectedStandbyMode", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableConnectedStandbyMode")],
            DetectOps = [RegOp.CheckDword(Key, "DisableConnectedStandbyMode", 1)],
        },
        new TweakDef
        {
            Id = "netlst-disable-connection-assistant",
            Label = "Disable Network Connection Assistant",
            Category = "Network List Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "The network connection assistant guides users through troubleshooting and configuring network connections when issues are detected. Disabling the connection assistant prevents users from using the wizard-based interface to modify network adapter configurations. Enterprise networking configurations are managed by network administrators and should not be modified by end users through interactive wizards. Allowing users to run the connection assistant can result in incorrect network configuration changes that require helpdesk intervention. Disabling this feature enforces network configuration control without removing administrators' ability to manage settings directly. Corporate network issues should be resolved through the helpdesk rather than user-initiated configuration changes.",
            Tags = ["network-list", "assistant", "configuration", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableConnectionAssistant", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableConnectionAssistant")],
            DetectOps = [RegOp.CheckDword(Key, "DisableConnectionAssistant", 1)],
        },
        new TweakDef
        {
            Id = "netlst-allow-network-icon",
            Label = "Allow Network Icon in System Tray",
            Category = "Network List Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "The network icon in the system tray provides users with quick access to network status and connection management. Allowing the network icon to appear at its default value ensures users can easily identify connectivity issues and connection state. This policy sets DisableNetworkIcon to zero, confirming the network icon remains visible in the system tray. Removing the network icon impedes user ability to identify connectivity issues, leading to increased helpdesk calls. Enterprise environments benefit from users having basic network status visibility to self-diagnose simple connectivity problems. The network icon represents a balance between user access and administrative control over network configuration.",
            Tags = ["network-list", "ui", "system-tray", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableNetworkIcon", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableNetworkIcon")],
            DetectOps = [RegOp.CheckDword(Key, "DisableNetworkIcon", 0)],
        },
        new TweakDef
        {
            Id = "netlst-disable-telemetry",
            Label = "Disable Network List Telemetry",
            Category = "Network List Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Network list telemetry transmits information about discovered networks, connection events, and network profile changes to Microsoft. This data includes network identifiers such as SSIDs and BSSID information that can reveal location information. Disabling network list telemetry prevents network topology and location data from being transmitted outside the enterprise. Regulated industries require that network infrastructure details are not disclosed through telemetry channels. The network list continues to function identically regardless of telemetry status. Administrators requiring network connection analytics should implement dedicated network monitoring solutions.",
            Tags = ["network-list", "telemetry", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableNetworkListTelemetry", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableNetworkListTelemetry")],
            DetectOps = [RegOp.CheckDword(Key, "DisableNetworkListTelemetry", 1)],
        },
        new TweakDef
        {
            Id = "netlst-disable-manual-roaming",
            Label = "Disable Manual Network Roaming",
            Category = "Network List Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 3,
            Description =
                "Manual roaming allows users to select which wireless network the device connects to when multiple networks with the same SSID are available in different locations. Disabling manual roaming prevents users from overriding the automatic network selection algorithm with manual access point selections. Enterprise wireless networks use 802.11r and RSSI-based roaming algorithms that should not be overridden by user selection. User-initiated manual AP selection can cause persistent connections to distant access points with poor signal, degrading performance. Disabling this feature ensures the enterprise wireless client uses optimal access points based on signal strength and policy. Centrally managed wireless infrastructure delivers better performance outcomes than user-directed manual selection.",
            Tags = ["network-list", "roaming", "wireless", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AllowManualRoaming", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowManualRoaming")],
            DetectOps = [RegOp.CheckDword(Key, "AllowManualRoaming", 0)],
        },
        new TweakDef
        {
            Id = "netlst-disable-social-network",
            Label = "Disable Social Network Integration",
            Category = "Network List Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Social network integration in the network list manager allows Windows to display social network status and share network information with connected social services. Disabling social network integration prevents the operating system from communicating with social network APIs through network list event callbacks. Enterprise workstations should not have social network integration active as it represents an uncontrolled data channel. Social network APIs can receive information about device connectivity state and network location through this integration. Disabling this feature eliminates a potential data exfiltration path through social network API calls. Standard network connectivity is completely unaffected by disabling social network integration.",
            Tags = ["network-list", "social-network", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableSocialNetworkIntegration", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableSocialNetworkIntegration")],
            DetectOps = [RegOp.CheckDword(Key, "DisableSocialNetworkIntegration", 1)],
        },
        new TweakDef
        {
            Id = "netlst-disable-network-mapping",
            Label = "Disable Network Mapping",
            Category = "Network List Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Network mapping enumerates devices on the local network and displays them in the Network section of Windows Explorer using LLTD and network discovery protocols. Disabling network mapping prevents the device from actively probing the network for neighboring devices. Reducing device enumeration broadcasts lowers the network's attack surface by preventing device discovery by unauthenticated network participants. Enterprise devices on segmented corporate networks should not broadcast their presence to other network segments via network mapping. The LLTD protocol used for network mapping can reveal device types and capabilities to network observers. Disabling network mapping is recommended for all production enterprise endpoints as a defense-in-depth measure.",
            Tags = ["network-list", "discovery", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableNetworkMapping", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableNetworkMapping")],
            DetectOps = [RegOp.CheckDword(Key, "DisableNetworkMapping", 1)],
        },
        new TweakDef
        {
            Id = "netlst-disable-category-change",
            Label = "Disable Network Category Change",
            Category = "Network List Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Windows allows users to change the network location category between Private, Public, and Domain profiles through the network settings UI. Disabling network category change prevents end users from modifying the assigned network location type for any connected network. Enterprise network profiles should be assigned and locked by Group Policy based on domain membership and network identity detection. User-modified network categories can override firewall profiles and compliance policies leading to unintended security exposure. Incorrect network category assignments are a common cause of unexpected firewall behavior affecting enterprise applications. Locking network categories through policy ensures consistent firewall and security profile application across all endpoints.",
            Tags = ["network-list", "firewall", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableNetworkCategoryChange", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableNetworkCategoryChange")],
            DetectOps = [RegOp.CheckDword(Key, "DisableNetworkCategoryChange", 1)],
        },
    ];
}
