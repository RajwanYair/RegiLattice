// RegiLattice.Core — Tweaks/XboxCloudGamingPolicy.cs
// Xbox Cloud Gaming (xCloud) and content-restriction Group Policy controls (Sprint 602).
// Category: "Xbox Cloud Gaming Policy" | Slug: xbcloud
// Key: HKLM\SOFTWARE\Policies\Microsoft\XboxLive

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class XboxCloudGamingPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\XboxLive";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "xbcloud-disable-cloud-gaming-access",
            Label = "Xbox Cloud: Block Xbox Cloud Gaming (xCloud) Access",
            Category = "Xbox Cloud Gaming Policy",
            Description = "Sets AllowCloudGaming=0 in XboxLive machine policy. Prevents users on this device from accessing Xbox Cloud Gaming (Project xCloud). " +
                "Xbox Cloud Gaming streams game compute from Azure data-centres and requires persistent high-bandwidth internet. On corporate or managed devices, cloud gaming represents an unsanctioned cloud service, a bandwidth-exhaustion risk during business hours, and a potential data-exfiltration vector if game streaming sessions can capture screen content. " +
                "Blocking cloud gaming ensures the device is used only for sanctioned workloads.",
            Tags = ["xbox", "cloud-gaming", "xcloud", "enterprise", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Blocks xCloud streaming; prevents bandwidth consumption and unsanctioned cloud service access on managed devices.",
            ApplyOps = [RegOp.SetDword(Key, "AllowCloudGaming", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowCloudGaming")],
            DetectOps = [RegOp.CheckDword(Key, "AllowCloudGaming", 0)],
        },
        new TweakDef
        {
            Id = "xbcloud-disable-xbox-cloud-save-sync",
            Label = "Xbox Cloud: Disable Xbox Live Cloud Save Synchronisation",
            Category = "Xbox Cloud Gaming Policy",
            Description = "Sets AllowCloudSaveSync=0 in XboxLive machine policy. Prevents Xbox game save data from being synchronised to and from Xbox Live cloud storage. " +
                "Cloud save sync transfers game state, progress, and personal gaming data to Microsoft's Xbox Live servers. In regulated industries or GDPR-compliant environments, this data transfer requires legal basis and may conflict with data residency requirements. " +
                "Disabling cloud save sync keeps game data local and prevents any XboxLive-originated outbound data flow from the device.",
            Tags = ["xbox", "cloud-save", "data-sync", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Blocks Xbox save data sync to cloud; game progress stays local only.",
            ApplyOps = [RegOp.SetDword(Key, "AllowCloudSaveSync", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowCloudSaveSync")],
            DetectOps = [RegOp.CheckDword(Key, "AllowCloudSaveSync", 0)],
        },
        new TweakDef
        {
            Id = "xbcloud-block-xbox-multiplayer-social-features",
            Label = "Xbox Cloud: Block Xbox Live Multiplayer and Social Features",
            Category = "Xbox Cloud Gaming Policy",
            Description = "Sets AllowXboxLiveMultiplayer=0 in XboxLive machine policy. Disables Xbox Live multiplayer matchmaking, party chat, and social gaming features. " +
                "Xbox Live multiplayer requires open communication channels to Xbox Live services and to other players, which can be misused for social engineering, off-channel communication in corporate environments, or unmonitored voice chat. " +
                "Blocking multiplayer social features ensures the device does not participate in any Xbox Live social graph activity.",
            Tags = ["xbox", "multiplayer", "social", "enterprise", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Blocks Xbox multiplayer matchmaking and social features; games can still be played locally without Xbox Live.",
            ApplyOps = [RegOp.SetDword(Key, "AllowXboxLiveMultiplayer", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowXboxLiveMultiplayer")],
            DetectOps = [RegOp.CheckDword(Key, "AllowXboxLiveMultiplayer", 0)],
        },
        new TweakDef
        {
            Id = "xbcloud-block-xbox-in-game-purchases",
            Label = "Xbox Cloud: Block Xbox Live In-Game Purchases",
            Category = "Xbox Cloud Gaming Policy",
            Description = "Sets AllowInGamePurchases=0 in XboxLive machine policy. Prevents users from making in-game or in-app purchases through the Xbox Live storefront. " +
                "In-game purchases (microtransactions) using corporate-provisioned accounts or linked payment methods create financial exposure. Blocking this setting on managed devices prevents unauthorised financial transactions and ensures the device cannot be used as a purchase gateway for Xbox content.",
            Tags = ["xbox", "purchases", "microtransactions", "financial", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Blocks Xbox in-game purchases; eliminates financial transaction risk on managed devices.",
            ApplyOps = [RegOp.SetDword(Key, "AllowInGamePurchases", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowInGamePurchases")],
            DetectOps = [RegOp.CheckDword(Key, "AllowInGamePurchases", 0)],
        },
        new TweakDef
        {
            Id = "xbcloud-block-xbox-achievement-sharing",
            Label = "Xbox Cloud: Block Xbox Achievement and Activity Sharing",
            Category = "Xbox Cloud Gaming Policy",
            Description = "Sets AllowAchievementSharing=0 in XboxLive machine policy. Blocks Xbox achievement notifications and activity sharing from being posted to the Xbox Live social feed. " +
                "Achievement sharing publishes play activity to the public Xbox social graph, which may disclose information about the user's presence, games played, and gaming schedule. On managed devices, any outbound social activity not authorised by IT policy should be suppressed.",
            Tags = ["xbox", "achievements", "sharing", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "Suppresses achievement sharing; no social feed posts from this device.",
            ApplyOps = [RegOp.SetDword(Key, "AllowAchievementSharing", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowAchievementSharing")],
            DetectOps = [RegOp.CheckDword(Key, "AllowAchievementSharing", 0)],
        },
        new TweakDef
        {
            Id = "xbcloud-restrict-xbox-content-rating-e",
            Label = "Xbox Cloud: Restrict Xbox Content to Everyone (E) Rated Only",
            Category = "Xbox Cloud Gaming Policy",
            Description = "Sets ContentRatingMaxLevel=1 in XboxLive machine policy. Restricts the maximum content rating that can be played or purchased through Xbox to Everyone (E) — suitable for all ages. " +
                "Content rating enforcement is critical on shared devices, lab workstations, kiosk PCs, or any device where unexpected mature content could appear on-screen in a professional or educational setting. An E-rating cap prevents the Xbox service from displaying or launching Mature/M-rated, Teen/T-rated, or Adults Only content.",
            Tags = ["xbox", "content-rating", "parental-control", "kiosk", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Enforces E-rating ceiling for Xbox content; only Everyone-rated titles accessible.",
            ApplyOps = [RegOp.SetDword(Key, "ContentRatingMaxLevel", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "ContentRatingMaxLevel")],
            DetectOps = [RegOp.CheckDword(Key, "ContentRatingMaxLevel", 1)],
        },
        new TweakDef
        {
            Id = "xbcloud-disable-xbox-live-friend-requests",
            Label = "Xbox Cloud: Disable Xbox Live Friend Requests",
            Category = "Xbox Cloud Gaming Policy",
            Description = "Sets AllowFriendRequests=0 in XboxLive machine policy. Prevents users from sending or receiving Xbox Live friend requests from this device. " +
                "Friends on Xbox Live gain access to the user's gaming status, presence information, and can initiate voice/party invitations. On corporate or supervised devices, building a social gaming network using company credentials or contact details is an oversharing risk. Blocking friend requests prevents social graph expansion from managed endpoints.",
            Tags = ["xbox", "friends", "social-graph", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "Blocks friend requests; prevents Xbox social graph expansion from managed endpoints.",
            ApplyOps = [RegOp.SetDword(Key, "AllowFriendRequests", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowFriendRequests")],
            DetectOps = [RegOp.CheckDword(Key, "AllowFriendRequests", 0)],
        },
        new TweakDef
        {
            Id = "xbcloud-block-user-generated-content-access",
            Label = "Xbox Cloud: Block User-Generated Content Access",
            Category = "Xbox Cloud Gaming Policy",
            Description = "Sets AllowUserGeneratedContent=0 in XboxLive machine policy. Prevents browsing and downloading of user-generated content (UGC) from Xbox Live — including user-created game maps, mods, character skins, and downloadable content created by the community. " +
                "UGC from Xbox Live is unvetted third-party content that could contain inappropriate material, modified game executables, or content that violates organisational acceptable-use policies. Blocking UGC access reduces the attack surface for content-moderation bypass exploits in Xbox-connected games.",
            Tags = ["xbox", "ugc", "user-content", "content-filter", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Blocks Xbox UGC access; prevents community-created content downloads from unvetted sources.",
            ApplyOps = [RegOp.SetDword(Key, "AllowUserGeneratedContent", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowUserGeneratedContent")],
            DetectOps = [RegOp.CheckDword(Key, "AllowUserGeneratedContent", 0)],
        },
        new TweakDef
        {
            Id = "xbcloud-disable-xbox-live-voice-messaging",
            Label = "Xbox Cloud: Disable Xbox Live Voice and Text Messaging",
            Category = "Xbox Cloud Gaming Policy",
            Description = "Sets AllowVoiceMessaging=0 in XboxLive machine policy. Disables the Xbox Live voice message and text message features that allow users to send audio clips and text messages to other Xbox Live users. " +
                "Xbox Live messaging creates a communication channel that bypasses corporate email/instant-messaging policies and monitoring. On managed devices, any off-channel communication tool should be blocked to ensure all communications go through monitored, policy-compliant channels.",
            Tags = ["xbox", "messaging", "voice", "communication", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "Disables Xbox Live voice/text messaging; eliminates off-channel communication vector.",
            ApplyOps = [RegOp.SetDword(Key, "AllowVoiceMessaging", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowVoiceMessaging")],
            DetectOps = [RegOp.CheckDword(Key, "AllowVoiceMessaging", 0)],
        },
        new TweakDef
        {
            Id = "xbcloud-block-xbox-cross-play-with-consoles",
            Label = "Xbox Cloud: Block Xbox Cross-Play with Console Players",
            Category = "Xbox Cloud Gaming Policy",
            Description = "Sets AllowCrossPlay=0 in XboxLive machine policy. Prevents games running on this Windows PC from engaging in crossplay sessions with Xbox console players via Xbox Live. " +
                "Cross-play requires the Windows machine to be reachable by console-based connection requests through Xbox Live's relay infrastructure. Blocking cross-play reduces the device's exposure to Xbox Live's multi-platform multiplayer network and prevents unexpected inbound connection attempts from console players who may have different content settings.",
            Tags = ["xbox", "cross-play", "multiplayer", "network", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Blocks cross-play with console users; PC gaming sessions isolated from Xbox console multiplayer.",
            ApplyOps = [RegOp.SetDword(Key, "AllowCrossPlay", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowCrossPlay")],
            DetectOps = [RegOp.CheckDword(Key, "AllowCrossPlay", 0)],
        },
    ];
}
