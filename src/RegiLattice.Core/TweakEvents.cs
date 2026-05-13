// RegiLattice.Core — TweakEvents.cs
// Tweak operation event records published via IEventBus (B.5).

namespace RegiLattice.Core;

/// <summary>Published after a tweak is successfully applied.</summary>
public sealed record TweakApplied(string TweakId, string Label, DateTimeOffset OccurredAt);

/// <summary>Published after a tweak is successfully removed/reverted.</summary>
public sealed record TweakRemoved(string TweakId, string Label, DateTimeOffset OccurredAt);

/// <summary>Published when an apply or remove operation fails.</summary>
public sealed record TweakFailed(string TweakId, string Label, string Operation, string Reason, DateTimeOffset OccurredAt);
