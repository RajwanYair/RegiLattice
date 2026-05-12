namespace RegiLattice.Core;

/// <summary>
/// Marks a static class as a tweak module for opt-in discovery by
/// <see cref="TweakEngine.RegisterBuiltins"/>.
/// </summary>
/// <remarks>
/// Classes decorated with this attribute are discovered by <see cref="TweakEngine.RegisterBuiltins"/>
/// regardless of their namespace, as long as they reside in the <c>RegiLattice.Core</c> assembly
/// and expose a static <c>Tweaks</c> property returning
/// <see cref="System.Collections.Generic.IReadOnlyList{T}"/> of <see cref="Models.TweakDef"/>.
/// The attribute-based path is additive: the namespace-based discovery path still runs for
/// any module class that does not yet carry the attribute.
/// </remarks>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class TweakModuleAttribute : Attribute { }
