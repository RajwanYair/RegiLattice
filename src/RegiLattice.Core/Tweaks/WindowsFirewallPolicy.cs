#nullable enable
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

internal static class WindowsFirewallPolicy
{
    private const string FwBase = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall";
    private const string DomainKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile";
    private const string PrivateKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PrivateProfile";
    private const string PublicKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PublicProfile";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "fwpol-enable-domain-profile",
            Label = "Enable Firewall on Domain Profile",
            Category = "Windows Firewall Policy",
            Description = "Ensures Windows Defender Firewall is active on Domain network profile connections.",
            Tags = ["firewall", "domain", "profile", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "Firewall enforced on domain-joined networks; prevents admin from disabling it without policy change.",
            ApplyOps = [RegOp.SetDword(DomainKey, "EnableFirewall", 1)],
            RemoveOps = [RegOp.DeleteValue(DomainKey, "EnableFirewall")],
            DetectOps = [RegOp.CheckDword(DomainKey, "EnableFirewall", 1)],
        },
        new TweakDef
        {
            Id = "fwpol-enable-private-profile",
            Label = "Enable Firewall on Private Profile",
            Category = "Windows Firewall Policy",
            Description = "Ensures Windows Defender Firewall is active on Private (trusted home/work) network profile connections.",
            Tags = ["firewall", "private", "profile", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "Firewall enforced on private networks; required for defence-in-depth on non-domain devices.",
            ApplyOps = [RegOp.SetDword(PrivateKey, "EnableFirewall", 1)],
            RemoveOps = [RegOp.DeleteValue(PrivateKey, "EnableFirewall")],
            DetectOps = [RegOp.CheckDword(PrivateKey, "EnableFirewall", 1)],
        },
        new TweakDef
        {
            Id = "fwpol-enable-public-profile",
            Label = "Enable Firewall on Public Profile",
            Category = "Windows Firewall Policy",
            Description = "Ensures Windows Defender Firewall is active on Public (untrusted) network profile connections.",
            Tags = ["firewall", "public", "profile", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "Firewall enforced on public networks; highest risk profile; critical for laptops on hotel/café Wi-Fi.",
            ApplyOps = [RegOp.SetDword(PublicKey, "EnableFirewall", 1)],
            RemoveOps = [RegOp.DeleteValue(PublicKey, "EnableFirewall")],
            DetectOps = [RegOp.CheckDword(PublicKey, "EnableFirewall", 1)],
        },
        new TweakDef
        {
            Id = "fwpol-block-inbound-domain",
            Label = "Block All Inbound Connections on Domain Profile",
            Category = "Windows Firewall Policy",
            Description = "Sets the default inbound action to block all inbound connections on the domain network profile.",
            Tags = ["firewall", "domain", "inbound", "block", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 3,
            ImpactNote = "Blocks all unsolicited inbound traffic on domain networks; explicit allow rules required for managed services.",
            ApplyOps = [RegOp.SetDword(DomainKey, "DefaultInboundAction", 1)],
            RemoveOps = [RegOp.DeleteValue(DomainKey, "DefaultInboundAction")],
            DetectOps = [RegOp.CheckDword(DomainKey, "DefaultInboundAction", 1)],
        },
        new TweakDef
        {
            Id = "fwpol-block-inbound-public",
            Label = "Block All Inbound Connections on Public Profile",
            Category = "Windows Firewall Policy",
            Description = "Sets the default action to block all inbound connections on the Public network profile.",
            Tags = ["firewall", "public", "inbound", "block", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "Blocks all inbound on public networks; most secure setting for untrusted/mobile scenarios.",
            ApplyOps = [RegOp.SetDword(PublicKey, "DefaultInboundAction", 1)],
            RemoveOps = [RegOp.DeleteValue(PublicKey, "DefaultInboundAction")],
            DetectOps = [RegOp.CheckDword(PublicKey, "DefaultInboundAction", 1)],
        },
        new TweakDef
        {
            Id = "fwpol-no-local-rules-domain",
            Label = "Prevent Local Firewall Rules on Domain Profile",
            Category = "Windows Firewall Policy",
            Description = "Disallows local administrators from creating firewall allow-rules that bypass Group Policy domain profile rules.",
            Tags = ["firewall", "domain", "local-rules", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Only GP-delivered firewall rules apply on the Domain profile; local exceptions are ignored.",
            ApplyOps = [RegOp.SetDword(DomainKey, "AllowLocalPolicyMerge", 0)],
            RemoveOps = [RegOp.DeleteValue(DomainKey, "AllowLocalPolicyMerge")],
            DetectOps = [RegOp.CheckDword(DomainKey, "AllowLocalPolicyMerge", 0)],
        },
        new TweakDef
        {
            Id = "fwpol-no-local-rules-public",
            Label = "Prevent Local Firewall Rules on Public Profile",
            Category = "Windows Firewall Policy",
            Description = "Disallows local firewall rule creation on the Public profile, enforcing only policy-delivered rules.",
            Tags = ["firewall", "public", "local-rules", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "GP rules are authoritative on public networks; local admin cannot weaken public profile protection.",
            ApplyOps = [RegOp.SetDword(PublicKey, "AllowLocalPolicyMerge", 0)],
            RemoveOps = [RegOp.DeleteValue(PublicKey, "AllowLocalPolicyMerge")],
            DetectOps = [RegOp.CheckDword(PublicKey, "AllowLocalPolicyMerge", 0)],
        },
        new TweakDef
        {
            Id = "fwpol-unicast-response-domain",
            Label = "Disable Unicast Response to Multicast on Domain Profile",
            Category = "Windows Firewall Policy",
            Description = "Prevents the firewall from allowing unicast responses to multicast/broadcast packets on Domain networks.",
            Tags = ["firewall", "domain", "multicast", "unicast", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Reduces information disclosure via multicast; may affect some network discovery features.",
            ApplyOps = [RegOp.SetDword(DomainKey, "DisableUnicastResponsesToMulticastBroadcast", 1)],
            RemoveOps = [RegOp.DeleteValue(DomainKey, "DisableUnicastResponsesToMulticastBroadcast")],
            DetectOps = [RegOp.CheckDword(DomainKey, "DisableUnicastResponsesToMulticastBroadcast", 1)],
        },
        new TweakDef
        {
            Id = "fwpol-unicast-response-public",
            Label = "Disable Unicast Response to Multicast on Public Profile",
            Category = "Windows Firewall Policy",
            Description = "Prevents the firewall from sending unicast responses to multicast/broadcast probes on Public networks.",
            Tags = ["firewall", "public", "multicast", "unicast", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Helps hide the device on public Wi-Fi; reduces exposure to broadcast-based network enumeration.",
            ApplyOps = [RegOp.SetDword(PublicKey, "DisableUnicastResponsesToMulticastBroadcast", 1)],
            RemoveOps = [RegOp.DeleteValue(PublicKey, "DisableUnicastResponsesToMulticastBroadcast")],
            DetectOps = [RegOp.CheckDword(PublicKey, "DisableUnicastResponsesToMulticastBroadcast", 1)],
        },
        new TweakDef
        {
            Id = "fwpol-disable-notifications-public",
            Label = "Disable Firewall Notifications on Public Profile",
            Category = "Windows Firewall Policy",
            Description = "Suppresses Windows Defender Firewall blocked-connection notifications when on a Public network profile.",
            Tags = ["firewall", "public", "notifications", "policy", "ui"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Blocks pop-up notifications for blocked connections; users won't see firewall alert dialogs.",
            ApplyOps = [RegOp.SetDword(PublicKey, "DisableNotifications", 1)],
            RemoveOps = [RegOp.DeleteValue(PublicKey, "DisableNotifications")],
            DetectOps = [RegOp.CheckDword(PublicKey, "DisableNotifications", 1)],
        },
    ];
}
