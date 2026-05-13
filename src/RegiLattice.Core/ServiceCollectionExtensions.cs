// RegiLattice.Core — ServiceCollectionExtensions.cs
// DI registration helper for host applications (B.1 — Dependency Injection Container).
// Usage:
//   var services = new ServiceCollection()
//       .AddRegiLatticeCore()
//       .BuildServiceProvider();
//   var search = services.GetRequiredService<ITweakSearch>();

using Microsoft.Extensions.DependencyInjection;
using RegiLattice.Core.Registry;
using RegiLattice.Core.Services;

namespace RegiLattice.Core;

/// <summary>
/// Extension methods for <see cref="IServiceCollection"/> that register all
/// RegiLattice Core services as singletons.
/// <para>
/// <see cref="TweakEngine"/> is the backward-compatible façade that implements
/// all six <c>ITweakEngine</c> interfaces. All interface registrations resolve
/// to the same <see cref="TweakEngine"/> singleton.
/// </para>
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers all RegiLattice Core services into <paramref name="services"/>.
    /// <list type="bullet">
    ///   <item><see cref="TweakEngine"/> — shared singleton (all six <c>ITweakEngine</c> interfaces)</item>
    ///   <item><see cref="IEventBus"/> — backed by <see cref="InProcessEventBus"/></item>
    ///   <item><see cref="IFavoritesRepository"/> — backed by <see cref="JsonFavoritesRepository"/></item>
    ///   <item><see cref="RegistrySession"/> — default session (no dry-run)</item>
    /// </list>
    /// Override individual registrations <em>after</em> calling this method to substitute
    /// custom implementations (e.g. an in-memory repository for tests).
    /// </summary>
    /// <param name="services">The service collection to populate.</param>
    /// <returns>The same <paramref name="services"/> for fluent chaining.</returns>
    public static IServiceCollection AddRegiLatticeCore(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        // ── Core engine ─────────────────────────────────────────────────────────
        // Register TweakEngine once; all interfaces resolve the same instance.
        services.AddSingleton<TweakEngine>();
        services.AddSingleton<ITweakRegistry>(sp => sp.GetRequiredService<TweakEngine>());
        services.AddSingleton<ITweakSearch>(sp => sp.GetRequiredService<TweakEngine>());
        services.AddSingleton<ITweakExecutor>(sp => sp.GetRequiredService<TweakEngine>());
        services.AddSingleton<ITweakStatus>(sp => sp.GetRequiredService<TweakEngine>());
        services.AddSingleton<IProfileManager>(sp => sp.GetRequiredService<TweakEngine>());
        services.AddSingleton<ITweakValidator>(sp => sp.GetRequiredService<TweakEngine>());

        // ── Registry session ─────────────────────────────────────────────────────
        services.AddSingleton<RegistrySession>();

        // ── Event bus ────────────────────────────────────────────────────────────
        services.AddSingleton<IEventBus, InProcessEventBus>();

        // ── Repository layer ─────────────────────────────────────────────────────
        services.AddSingleton<IFavoritesRepository, JsonFavoritesRepository>();
        services.AddSingleton<IRatingsRepository, JsonRatingsRepository>();
        services.AddSingleton<IHistoryRepository, JsonHistoryRepository>();

        return services;
    }
}
