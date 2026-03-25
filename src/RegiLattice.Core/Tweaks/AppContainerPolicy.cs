// RegiLattice.Core — Tweaks/AppContainerPolicy.cs
// Sprint 297: App Container Policy tweaks (10 tweaks)
// Category: "App Container Policy" | Slug: appcont
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppContainer

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class AppContainerPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppContainer";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "appcont-disable-loopback",
            Label = "Disable App Container Loopback",
            Category = "App Container Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            Description =
                "App Container loopback exemptions allow UWP apps running inside App Container sandboxes to make network connections to localhost and other loopback addresses. Disabling loopback access prevents sandboxed apps from communicating with local services or other processes via the loopback adapter. App Container isolation is designed to prevent sandboxed apps from accessing sensitive local resources and services. Loopback connections can bypass the intended network isolation of App Container by allowing communication with locally running privileged services. Restricting loopback access enforces stricter sandbox isolation for UWP applications. Applications requiring loopback access for legitimate development or proxy scenarios require explicit exemptions through approved mechanisms.",
            Tags = ["appcontainer", "sandbox", "loopback", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableLoopback", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableLoopback")],
            DetectOps = [RegOp.CheckDword(Key, "DisableLoopback", 1)],
        },
        new TweakDef
        {
            Id = "appcont-disable-capability-enumeration",
            Label = "Disable App Container Capability Enumeration",
            Category = "App Container Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "App Container capability enumeration exposes detailed information about the capabilities declared by sandboxed UWP applications. Disabling capability enumeration prevents the capability list from being queried through the shell and system APIs. Capability information can be used by malicious code to identify apps with elevated permissions or sensitive access grants. Limiting capability discovery reduces information exposure about the privilege levels of installed applications. Enterprise application assessments should use managed inventory tools rather than runtime capability enumeration. Disabling this feature has no functional impact on the operation of installed applications.",
            Tags = ["appcontainer", "capabilities", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableCapabilityEnumeration", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableCapabilityEnumeration")],
            DetectOps = [RegOp.CheckDword(Key, "DisableCapabilityEnumeration", 1)],
        },
        new TweakDef
        {
            Id = "appcont-disable-network-access",
            Label = "Restrict App Container Network Access",
            Category = "App Container Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 3,
            Description =
                "App Container network access controls whether sandboxed UWP applications can make network connections to external resources. Restricting network access prevents App Container applications from communicating with external services without explicit network capability declarations. Unauthorized network access from sandboxed applications represents a data exfiltration risk for enterprise environments. UWP applications should declare required network capabilities at packaging time and have those capabilities reviewed before deployment. Blanket restriction of network access forces application review and approval before any sandbox network connectivity is permitted. Applications with legitimate network requirements should be approved through the enterprise application governance process.",
            Tags = ["appcontainer", "network", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictNetworkAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictNetworkAccess")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictNetworkAccess", 1)],
        },
        new TweakDef
        {
            Id = "appcont-disable-local-filesystem",
            Label = "Restrict App Container Local Filesystem Access",
            Category = "App Container Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            Description =
                "App Container filesystem access controls allow sandboxed UWP applications to request access to user libraries, documents, and other local filesystem locations. Restricting local filesystem access prevents sandboxed applications from reading or writing to sensitive filesystem locations beyond their designated isolated storage. Enterprise document stores and user data should not be accessible to sandboxed applications without explicit entitlement review. File access capabilities declared in the application manifest are subject to runtime user consent prompts which can be inadvertently approved. Policy-level restriction provides a mandatory control layer that operates below the user consent mechanism. Applications requiring filesystem access should be assessed for data handling practices before deployment.",
            Tags = ["appcontainer", "filesystem", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictLocalFilesystemAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictLocalFilesystemAccess")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictLocalFilesystemAccess", 1)],
        },
        new TweakDef
        {
            Id = "appcont-disable-clipboard-access",
            Label = "Restrict App Container Clipboard Access",
            Category = "App Container Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "The clipboard is a shared resource used to transfer data between applications and often contains sensitive information copied from enterprise applications. App Container clipboard access allows sandboxed UWP applications to read from and write to the system clipboard. Restricting clipboard access prevents sandboxed applications from monitoring clipboard contents or injecting malicious clipboard data. Clipboard monitoring from sandboxed applications represents a low-privilege data exfiltration path in enterprise environments. DLP controls for clipboard data are more effective when combined with App Container clipboard restrictions. Applications requiring clipboard integration should be evaluated for data sensitivity and approved explicitly.",
            Tags = ["appcontainer", "clipboard", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictClipboardAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictClipboardAccess")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictClipboardAccess", 1)],
        },
        new TweakDef
        {
            Id = "appcont-disable-usb-access",
            Label = "Restrict App Container USB Access",
            Category = "App Container Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "App Container USB access capability allows sandboxed UWP applications to communicate with USB-connected devices including HID devices and custom USB hardware. Restricting USB access prevents sandboxed applications from reading from or writing to USB devices without explicit approval. USB interfaces have been used as covert channels for data exfiltration and as attack surfaces against device firmware. Sandboxed applications rarely have legitimate reasons to communicate with arbitrary USB devices in enterprise deployments. USB device access for UWP applications should be reviewed against specific workflow requirements and granted selectively. Restricting this capability reduces the risk of malicious applications abusing USB access for reconnaissance or data theft.",
            Tags = ["appcontainer", "usb", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictUsbAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictUsbAccess")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictUsbAccess", 1)],
        },
        new TweakDef
        {
            Id = "appcont-disable-com-access",
            Label = "Restrict App Container COM Server Access",
            Category = "App Container Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 3,
            Description =
                "COM server access from App Container allows sandboxed UWP applications to activate and communicate with out-of-process COM servers registered on the system. Restricting COM server access prevents sandboxed applications from leveraging COM to interact with higher-privileged processes and system services. COM objects exposed by system services or administrative applications may be accessible to sandboxed apps with insufficient access controls. The COM attack surface has historically been used for privilege escalation from sandboxed contexts. Restricting COM access from App Container strengthens the sandbox boundary against COM-based escape paths. Enterprise UWP applications with COM dependencies should be assessed and explicitly exempted if required.",
            Tags = ["appcontainer", "com", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictComAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictComAccess")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictComAccess", 1)],
        },
        new TweakDef
        {
            Id = "appcont-disable-telemetry",
            Label = "Disable App Container Telemetry",
            Category = "App Container Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Description =
                "App Container telemetry reports usage data about sandboxed application behavior, capability usage, and isolation events to Microsoft. This telemetry data helps improve the App Container security model and identify compatibility issues. Disabling this telemetry prevents information about installed sandboxed applications and their access patterns from being reported. Application behavior patterns represent sensitive operational information in enterprise environments. Telemetry should be evaluated under data governance policies before being permitted to transmit enterprise application usage data. App Container sandbox enforcement operates independently of this telemetry setting and is fully maintained.",
            Tags = ["appcontainer", "telemetry", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableTelemetry", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableTelemetry")],
            DetectOps = [RegOp.CheckDword(Key, "DisableTelemetry", 1)],
        },
        new TweakDef
        {
            Id = "appcont-disable-auto-launch",
            Label = "Disable App Container Automatic Launch",
            Category = "App Container Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "App Container automatic launch allows UWP applications to be launched automatically in response to system events, file associations, and protocol handlers. Disabling automatic launch prevents sandboxed applications from being invoked automatically without explicit user or administrator initiation. Automatic application launch triggered by file associations or protocol handlers can be exploited to launch attacker-controlled applications. Enterprise environment application launches should be controlled through managed deployment tools rather than automatic content-based triggers. Restricting automatic launch reduces the likelihood of sandboxed apps being invoked through social engineering attacks using malicious file types. Manual launch of approved applications continues to function normally.",
            Tags = ["appcontainer", "startup", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableAutoLaunch", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoLaunch")],
            DetectOps = [RegOp.CheckDword(Key, "DisableAutoLaunch", 1)],
        },
        new TweakDef
        {
            Id = "appcont-enforce-app-isolation",
            Label = "Enforce App Container Strict Isolation",
            Category = "App Container Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 3,
            Description =
                "App Container strict isolation enforces the strongest available sandbox boundaries, blocking all capability requests that have not been explicitly approved by policy. Enabling strict isolation prevents any relaxation of the App Container security model through API compatibility shims or legacy exception paths. Default Windows App Container settings include several compatibility adjustments that slightly weaken the sandbox for application compatibility reasons. Strict isolation removes these compatibility relaxations to enforce the maximum intended sandbox boundary. Enterprise environments prioritizing security over UWP application compatibility should enable strict isolation. Applications that fail under strict isolation require developer remediation to use the App Container model correctly.",
            Tags = ["appcontainer", "isolation", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnforceStrictIsolation", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnforceStrictIsolation")],
            DetectOps = [RegOp.CheckDword(Key, "EnforceStrictIsolation", 1)],
        },
    ];
}
