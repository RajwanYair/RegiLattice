#nullable enable
using System.Collections.Generic;
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

// Sprint 246 — MSDTC Distributed Transactions Policy (10 tweaks)
// Keys: HKLM\SOFTWARE\Policies\Microsoft\MSDTC
//       HKLM\SOFTWARE\Policies\Microsoft\MSDTC\Security
internal static class MsdtcPolicy
{
    private const string MsDtcKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MSDTC";
    private const string MsDtcSec = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MSDTC\Security";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "msdtc-require-secure-rpc",
            Label = "Require Secure RPC for MSDTC",
            Category = "MSDTC & Distributed Transactions Policy",
            Description =
                "Sets AllowOnlySecureRpcCalls=1 in the MSDTC policy key. "
                + "Forces the Microsoft Distributed Transaction Coordinator to use only secure, authenticated RPC calls, "
                + "rejecting any client that tries to connect over unauthenticated or unencrypted RPC channels. "
                + "Default: absent. Recommended: 1 on all servers running MSDTC in production environments.",
            Tags = ["msdtc", "rpc", "security", "distributed-transactions", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "MSDTC rejects unauthenticated RPC clients; may break legacy applications using plain-text RPC.",
            ApplyOps = [RegOp.SetDword(MsDtcKey, "AllowOnlySecureRpcCalls", 1)],
            RemoveOps = [RegOp.DeleteValue(MsDtcKey, "AllowOnlySecureRpcCalls")],
            DetectOps = [RegOp.CheckDword(MsDtcKey, "AllowOnlySecureRpcCalls", 1)],
        },
        new TweakDef
        {
            Id = "msdtc-no-fallback-insecure-rpc",
            Label = "Prevent MSDTC Fallback to Unsecure RPC",
            Category = "MSDTC & Distributed Transactions Policy",
            Description =
                "Sets FallbackToUnsecureRPCIfNecessary=0 in the MSDTC policy key. "
                + "Prevents MSDTC from automatically falling back to unprotected RPC connections when a secure connection fails. "
                + "Combined with AllowOnlySecureRpcCalls, this closes the fallback loophole that would otherwise allow "
                + "unauthenticated transactions if the secure path is unavailable. "
                + "Default: absent. Recommended: 0 to prevent security downgrade.",
            Tags = ["msdtc", "rpc", "security", "fallback", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Blocks RPC security downgrade; MSDTC will fail rather than use unencrypted fallback RPC.",
            ApplyOps = [RegOp.SetDword(MsDtcKey, "FallbackToUnsecureRPCIfNecessary", 0)],
            RemoveOps = [RegOp.DeleteValue(MsDtcKey, "FallbackToUnsecureRPCIfNecessary")],
            DetectOps = [RegOp.CheckDword(MsDtcKey, "FallbackToUnsecureRPCIfNecessary", 0)],
        },
        new TweakDef
        {
            Id = "msdtc-keep-rpc-security",
            Label = "Keep MSDTC RPC Security Enabled",
            Category = "MSDTC & Distributed Transactions Policy",
            Description =
                "Sets TurnOffRpcSecurity=0 in the MSDTC policy key. "
                + "Ensures RPC security is kept active for MSDTC communications. "
                + "Setting this to 0 via policy prevents administrators from disabling RPC security for MSDTC, "
                + "hardening governance over transaction security posture. "
                + "Default: absent. Recommended: 0 to enforce security on.",
            Tags = ["msdtc", "rpc", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Locks RPC security ON for MSDTC; prevents manual disablement via Component Services UI.",
            ApplyOps = [RegOp.SetDword(MsDtcKey, "TurnOffRpcSecurity", 0)],
            RemoveOps = [RegOp.DeleteValue(MsDtcKey, "TurnOffRpcSecurity")],
            DetectOps = [RegOp.CheckDword(MsDtcKey, "TurnOffRpcSecurity", 0)],
        },
        new TweakDef
        {
            Id = "msdtc-disable-network-access",
            Label = "Disable MSDTC Network DTC Access",
            Category = "MSDTC & Distributed Transactions Policy",
            Description =
                "Sets NetworkDtcAccess=0 in the MSDTC Security policy key. "
                + "Disables all network access to the Distributed Transaction Coordinator. "
                + "When off, MSDTC only handles local transactions; all inbound and outbound network "
                + "distributed transaction requests are rejected. "
                + "Default: absent (network access determined by Component Services settings). "
                + "Recommended: 0 on machines where distributed transactions are not required.",
            Tags = ["msdtc", "network", "dtc", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            ImpactNote = "Disables all MSDTC network access; distributed transactions requiring network DTC will fail.",
            ApplyOps = [RegOp.SetDword(MsDtcSec, "NetworkDtcAccess", 0)],
            RemoveOps = [RegOp.DeleteValue(MsDtcSec, "NetworkDtcAccess")],
            DetectOps = [RegOp.CheckDword(MsDtcSec, "NetworkDtcAccess", 0)],
        },
        new TweakDef
        {
            Id = "msdtc-disable-client-dtc",
            Label = "Disable MSDTC Network Client Access",
            Category = "MSDTC & Distributed Transactions Policy",
            Description =
                "Sets NetworkDtcAccessClients=0 in the MSDTC Security policy key. "
                + "Prevents client applications on this machine from initiating or participating in "
                + "outbound distributed transactions via MSDTC over the network. "
                + "Default: absent. Recommended: 0 on workstations and servers not acting as DTC clients.",
            Tags = ["msdtc", "client", "dtc", "network", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Blocks outbound client-initiated distributed transactions via MSDTC; local transactions still work.",
            ApplyOps = [RegOp.SetDword(MsDtcSec, "NetworkDtcAccessClients", 0)],
            RemoveOps = [RegOp.DeleteValue(MsDtcSec, "NetworkDtcAccessClients")],
            DetectOps = [RegOp.CheckDword(MsDtcSec, "NetworkDtcAccessClients", 0)],
        },
        new TweakDef
        {
            Id = "msdtc-disable-inbound-transactions",
            Label = "Disable MSDTC Inbound Network Transactions",
            Category = "MSDTC & Distributed Transactions Policy",
            Description =
                "Sets NetworkDtcAccessInbound=0 in the MSDTC Security policy key. "
                + "Blocks this machine from accepting inbound distributed transaction requests from remote MSDTC coordinators. "
                + "Reduces the attack surface by preventing this machine from being enlisted as a resource manager "
                + "in transactions initiated by other systems. "
                + "Default: absent. Recommended: 0 when the server does not need to accept remote DTC enlistments.",
            Tags = ["msdtc", "inbound", "dtc", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            ImpactNote = "Rejects inbound DTC enlistment from remote machines; local resource managers unaffected.",
            ApplyOps = [RegOp.SetDword(MsDtcSec, "NetworkDtcAccessInbound", 0)],
            RemoveOps = [RegOp.DeleteValue(MsDtcSec, "NetworkDtcAccessInbound")],
            DetectOps = [RegOp.CheckDword(MsDtcSec, "NetworkDtcAccessInbound", 0)],
        },
        new TweakDef
        {
            Id = "msdtc-disable-outbound-transactions",
            Label = "Disable MSDTC Outbound Network Transactions",
            Category = "MSDTC & Distributed Transactions Policy",
            Description =
                "Sets NetworkDtcAccessOutbound=0 in the MSDTC Security policy key. "
                + "Prevents this machine from propagating distributed transactions to remote resource managers. "
                + "Stops outbound transaction enlistment requests that this MSDTC instance would send to remote systems. "
                + "Default: absent. Recommended: 0 on machines that must not initiate network-spanning transactions.",
            Tags = ["msdtc", "outbound", "dtc", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            ImpactNote = "Blocks outbound DTC enlistment to remote systems; local-only transactions are unaffected.",
            ApplyOps = [RegOp.SetDword(MsDtcSec, "NetworkDtcAccessOutbound", 0)],
            RemoveOps = [RegOp.DeleteValue(MsDtcSec, "NetworkDtcAccessOutbound")],
            DetectOps = [RegOp.CheckDword(MsDtcSec, "NetworkDtcAccessOutbound", 0)],
        },
        new TweakDef
        {
            Id = "msdtc-disable-network-transactions",
            Label = "Disable MSDTC Network Transaction Coordinator",
            Category = "MSDTC & Distributed Transactions Policy",
            Description =
                "Sets NetworkDtcAccessTransactions=0 in the MSDTC Security policy key. "
                + "Disables the network transaction coordination role of MSDTC, preventing it from acting as "
                + "a transaction manager for distributed transactions involving remote participants. "
                + "Default: absent. Recommended: 0 on workstations and servers not participating in multi-system transactions.",
            Tags = ["msdtc", "network", "transactions", "coordinator", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            ImpactNote = "Disables MSDTC as a network transaction coordinator; cross-machine transactions will fail.",
            ApplyOps = [RegOp.SetDword(MsDtcSec, "NetworkDtcAccessTransactions", 0)],
            RemoveOps = [RegOp.DeleteValue(MsDtcSec, "NetworkDtcAccessTransactions")],
            DetectOps = [RegOp.CheckDword(MsDtcSec, "NetworkDtcAccessTransactions", 0)],
        },
        new TweakDef
        {
            Id = "msdtc-disable-xa-transactions",
            Label = "Disable MSDTC XA Transactions",
            Category = "MSDTC & Distributed Transactions Policy",
            Description =
                "Sets XaTransactions=0 in the MSDTC Security policy key. "
                + "Disables support for XA (X/Open) standard distributed transactions in MSDTC. "
                + "XA transactions are used by cross-platform distributed systems including Java EE app servers and "
                + "databases like Oracle and IBM DB2. Disabling reduces the MSDTC attack surface. "
                + "Default: absent. Recommended: 0 if XA transactions are not required.",
            Tags = ["msdtc", "xa", "transactions", "cross-platform", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Disables XA transaction support; affects Java EE and cross-vendor DB distributed transactions.",
            ApplyOps = [RegOp.SetDword(MsDtcSec, "XaTransactions", 0)],
            RemoveOps = [RegOp.DeleteValue(MsDtcSec, "XaTransactions")],
            DetectOps = [RegOp.CheckDword(MsDtcSec, "XaTransactions", 0)],
        },
        new TweakDef
        {
            Id = "msdtc-disable-lu-transactions",
            Label = "Disable MSDTC LU (SNA/LU6.2) Transactions",
            Category = "MSDTC & Distributed Transactions Policy",
            Description =
                "Sets LuTransactions=0 in the MSDTC Security policy key. "
                + "Disables support for IBM SNA LU6.2 (Logical Unit) transactions in MSDTC. "
                + "LU 6.2 is a legacy protocol used in mainframe integration (Host Integration Server / BizTalk). "
                + "Disabling removes an older protocol handler, reducing the attack surface on modern systems. "
                + "Default: absent. Recommended: 0 unless IBM mainframe integration via LU6.2 is required.",
            Tags = ["msdtc", "lu", "sna", "transactions", "legacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Disables LU6.2 transaction support; no impact unless IBM mainframe HIS/BizTalk scenarios are in use.",
            ApplyOps = [RegOp.SetDword(MsDtcSec, "LuTransactions", 0)],
            RemoveOps = [RegOp.DeleteValue(MsDtcSec, "LuTransactions")],
            DetectOps = [RegOp.CheckDword(MsDtcSec, "LuTransactions", 0)],
        },
    ];
}
