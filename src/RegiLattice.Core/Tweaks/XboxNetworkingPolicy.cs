// RegiLattice.Core — Tweaks/XboxNetworkingPolicy.cs
// Xbox Live, Xbox networking, gaming services, and Xbox Identity Provider policy — Sprint 514.
// Category: "Xbox Networking Policy" | Slug: xboxnet
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\XboxLive

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class XboxNetworkingPolicy
{
    private const string Key    = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\XboxLive";
    private const string SvcKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Gaming";
    private const string GipKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameInput";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id           = "xboxnet-disable-xbox-live-access",
            Label        = "Disable Xbox Live Network Access for Win32 Apps",
            Category     = "Xbox Networking Policy",
            Description  = "Prevents Win32 (non-Store) applications from accessing Xbox Live services and the Xbox Identity Provider API, blocking non-Store games from connecting to Xbox Live authentication and social services.",
            Tags         = ["xbox", "xbox-live", "win32", "network", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Xbox Live access blocked for Win32 apps; non-Store games cannot authenticate or use Xbox services.",
            ApplyOps     = [RegOp.SetDword(Key, "BlockXboxLiveForWin32Apps", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "BlockXboxLiveForWin32Apps")],
            DetectOps    = [RegOp.CheckDword(Key, "BlockXboxLiveForWin32Apps", 1)],
        },
        new TweakDef
        {
            Id           = "xboxnet-disable-xbox-live-signin",
            Label        = "Disable Xbox Live Automatic Sign-In at Windows Logon",
            Category     = "Xbox Networking Policy",
            Description  = "Prevents the Xbox Identity Provider service from automatically signing in the user to Xbox Live when they log on to Windows, reducing outbound authentication traffic and Xbox identity disclosure.",
            Tags         = ["xbox", "xbox-live", "auto-signin", "privacy", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Xbox Live auto-sign-in at logon disabled; user not automatically authenticated to Xbox on Windows startup.",
            ApplyOps     = [RegOp.SetDword(Key, "DisableAutoSignIn", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "DisableAutoSignIn")],
            DetectOps    = [RegOp.CheckDword(Key, "DisableAutoSignIn", 1)],
        },
        new TweakDef
        {
            Id           = "xboxnet-disable-gameinput-service",
            Label        = "Disable GameInput Service Auto-Start",
            Category     = "Xbox Networking Policy",
            Description  = "Prevents the GameInput host service from starting automatically at boot, reducing background service overhead on non-gaming devices. GameInput is required only for Xbox controller input handling in games.",
            Tags         = ["gameinput", "service", "auto-start", "performance", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "GameInput service auto-start disabled; Xbox controller input may not work in some games until service is started.",
            ApplyOps     = [RegOp.SetDword(GipKey, "DisableAutoStart", 1)],
            RemoveOps    = [RegOp.DeleteValue(GipKey, "DisableAutoStart")],
            DetectOps    = [RegOp.CheckDword(GipKey, "DisableAutoStart", 1)],
        },
        new TweakDef
        {
            Id           = "xboxnet-block-xbox-identity-provider",
            Label        = "Block Xbox Identity Provider from Network Access",
            Category     = "Xbox Networking Policy",
            Description  = "Restricts the Xbox Identity Provider (XIP) from making outbound network requests, effectively preventing Xbox authentication tokens from being obtained via the network and Xbox Live connectivity.",
            Tags         = ["xbox", "identity-provider", "network-block", "privacy", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Xbox Identity Provider blocked from network; Xbox Live authentication tokens cannot be obtained.",
            ApplyOps     = [RegOp.SetDword(Key, "BlockXboxIdentityProviderNetwork", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "BlockXboxIdentityProviderNetwork")],
            DetectOps    = [RegOp.CheckDword(Key, "BlockXboxIdentityProviderNetwork", 1)],
        },
        new TweakDef
        {
            Id           = "xboxnet-disable-xbox-presence",
            Label        = "Disable Xbox Presence and Social Notifications",
            Category     = "Xbox Networking Policy",
            Description  = "Disables Xbox presence reporting and social notifications (friends online, friend activity, achievement alerts) from appearing on the Windows desktop, preventing Xbox social platform notifications from interrupting work.",
            Tags         = ["xbox", "presence", "social", "notifications", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Xbox social presence and achievement notifications disabled; Xbox friend alerts not shown on desktop.",
            ApplyOps     = [RegOp.SetDword(Key, "DisablePresenceAndSocialNotifications", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "DisablePresenceAndSocialNotifications")],
            DetectOps    = [RegOp.CheckDword(Key, "DisablePresenceAndSocialNotifications", 1)],
        },
        new TweakDef
        {
            Id           = "xboxnet-disable-xbox-tcui",
            Label        = "Disable Xbox Title-Callable UI Overlay",
            Category     = "Xbox Networking Policy",
            Description  = "Prevents Xbox Title-Callable UI (TCUI) — the Xbox social overlay triggered by games — from rendering on top of applications, eliminating the in-game Xbox social layer that shows friend lists and achievement progress.",
            Tags         = ["xbox", "tcui", "overlay", "social", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Xbox TCUI in-game social overlay disabled; Xbox friend list and achievement overlay no longer rendered.",
            ApplyOps     = [RegOp.SetDword(Key, "DisableTCUI", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "DisableTCUI")],
            DetectOps    = [RegOp.CheckDword(Key, "DisableTCUI", 1)],
        },
        new TweakDef
        {
            Id           = "xboxnet-disable-gaming-services-update",
            Label        = "Disable Automatic Gaming Services Component Updates",
            Category     = "Xbox Networking Policy",
            Description  = "Prevents the Gaming Services package (GamingServices.exe) and Xbox Gaming Overlay from automatically updating via the Microsoft Store in the background, ensuring controlled update cycles for gaming components.",
            Tags         = ["gaming-services", "auto-update", "store", "gaming", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Gaming Services auto-update disabled; Xbox Gaming Overlay not updated automatically via Store.",
            ApplyOps     = [RegOp.SetDword(SvcKey, "DisableGamingServicesAutoUpdate", 1)],
            RemoveOps    = [RegOp.DeleteValue(SvcKey, "DisableGamingServicesAutoUpdate")],
            DetectOps    = [RegOp.CheckDword(SvcKey, "DisableGamingServicesAutoUpdate", 1)],
        },
        new TweakDef
        {
            Id           = "xboxnet-disable-xbox-app-launch-on-connect",
            Label        = "Disable Xbox App Auto-Launch on Xbox Controller Connect",
            Category     = "Xbox Networking Policy",
            Description  = "Prevents the Xbox application from automatically launching when an Xbox One or Series controller is connected via USB or Bluetooth, eliminating unwanted UI interruptions when using controllers with non-Xbox applications.",
            Tags         = ["xbox-app", "controller", "auto-launch", "usb", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Xbox app auto-launch on controller connect disabled; plugging an Xbox controller does not open the Xbox app.",
            ApplyOps     = [RegOp.SetDword(Key, "DisableXboxAppAutoLaunch", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "DisableXboxAppAutoLaunch")],
            DetectOps    = [RegOp.CheckDword(Key, "DisableXboxAppAutoLaunch", 1)],
        },
        new TweakDef
        {
            Id           = "xboxnet-disable-xbox-networking-telemetry",
            Label        = "Disable Xbox Networking and Gaming Service Telemetry",
            Category     = "Xbox Networking Policy",
            Description  = "Prevents Xbox networking services, Gaming Services, and GameInput from sending usage, connectivity, and diagnostic telemetry to Microsoft.",
            Tags         = ["xbox", "gaming-services", "telemetry", "privacy", "microsoft", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Xbox networking and gaming service telemetry to Microsoft disabled; gaming usage data not sent to cloud.",
            ApplyOps     = [RegOp.SetDword(Key, "DisableXboxTelemetry", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "DisableXboxTelemetry")],
            DetectOps    = [RegOp.CheckDword(Key, "DisableXboxTelemetry", 1)],
        },
        new TweakDef
        {
            Id           = "xboxnet-log-gaming-service-events",
            Label        = "Log Gaming Services Start/Stop Events in System Log",
            Category     = "Xbox Networking Policy",
            Description  = "Enables System event log entries for Gaming Services (GamingServices.exe) start, stop, crash, and recovery events, providing audit visibility into Xbox/gaming component lifecycle on corporate endpoints.",
            Tags         = ["gaming-services", "event-log", "audit", "xbox", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 2,
            SafetyRating = 5,
            ImpactNote   = "Gaming Services start/stop events logged in System log; Xbox component lifecycle visible for auditing.",
            ApplyOps     = [RegOp.SetDword(SvcKey, "LogGamingServiceEvents", 1)],
            RemoveOps    = [RegOp.DeleteValue(SvcKey, "LogGamingServiceEvents")],
            DetectOps    = [RegOp.CheckDword(SvcKey, "LogGamingServiceEvents", 1)],
        },
    ];
}
