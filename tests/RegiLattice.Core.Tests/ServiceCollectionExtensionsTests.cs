// RegiLattice.Core.Tests — ServiceCollectionExtensionsTests.cs
// Tests for the AddRegiLatticeCore() DI registration helper (B.1).

using Microsoft.Extensions.DependencyInjection;
using RegiLattice.Core;
using RegiLattice.Core.Registry;
using RegiLattice.Core.Services;
using Xunit;

namespace RegiLattice.Core.Tests;

public sealed class ServiceCollectionExtensionsTests
{
    // ── Arrange shared provider ───────────────────────────────────────────────

    private static ServiceProvider BuildProvider() =>
        new ServiceCollection()
            .AddRegiLatticeCore()
            .BuildServiceProvider();

    // ── Null guard ────────────────────────────────────────────────────────────

    [Fact]
    public void AddRegiLatticeCore_NullServices_Throws()
    {
        IServiceCollection? services = null;
        Assert.Throws<ArgumentNullException>(() => services!.AddRegiLatticeCore());
    }

    // ── Interface resolution ─────────────────────────────────────────────────

    [Fact]
    public void AddRegiLatticeCore_ResolvesITweakRegistry()
    {
        using var sp = BuildProvider();
        Assert.NotNull(sp.GetRequiredService<ITweakRegistry>());
    }

    [Fact]
    public void AddRegiLatticeCore_ResolvesITweakSearch()
    {
        using var sp = BuildProvider();
        Assert.NotNull(sp.GetRequiredService<ITweakSearch>());
    }

    [Fact]
    public void AddRegiLatticeCore_ResolvesITweakExecutor()
    {
        using var sp = BuildProvider();
        Assert.NotNull(sp.GetRequiredService<ITweakExecutor>());
    }

    [Fact]
    public void AddRegiLatticeCore_ResolvesITweakStatus()
    {
        using var sp = BuildProvider();
        Assert.NotNull(sp.GetRequiredService<ITweakStatus>());
    }

    [Fact]
    public void AddRegiLatticeCore_ResolvesIProfileManager()
    {
        using var sp = BuildProvider();
        Assert.NotNull(sp.GetRequiredService<IProfileManager>());
    }

    [Fact]
    public void AddRegiLatticeCore_ResolvesITweakValidator()
    {
        using var sp = BuildProvider();
        Assert.NotNull(sp.GetRequiredService<ITweakValidator>());
    }

    [Fact]
    public void AddRegiLatticeCore_ResolvesIEventBus()
    {
        using var sp = BuildProvider();
        Assert.NotNull(sp.GetRequiredService<IEventBus>());
    }

    [Fact]
    public void AddRegiLatticeCore_ResolvesIFavoritesRepository()
    {
        using var sp = BuildProvider();
        Assert.NotNull(sp.GetRequiredService<IFavoritesRepository>());
    }

    [Fact]
    public void AddRegiLatticeCore_ResolvesRegistrySession()
    {
        using var sp = BuildProvider();
        Assert.NotNull(sp.GetRequiredService<RegistrySession>());
    }

    // ── Singleton identity: all ITweakEngine interfaces share one TweakEngine ─

    [Fact]
    public void AddRegiLatticeCore_AllInterfacesResolveSameTweakEngineInstance()
    {
        using var sp = BuildProvider();
        var engine = sp.GetRequiredService<TweakEngine>();
        Assert.Same(engine, (TweakEngine)sp.GetRequiredService<ITweakRegistry>());
        Assert.Same(engine, (TweakEngine)sp.GetRequiredService<ITweakSearch>());
        Assert.Same(engine, (TweakEngine)sp.GetRequiredService<ITweakExecutor>());
        Assert.Same(engine, (TweakEngine)sp.GetRequiredService<ITweakStatus>());
        Assert.Same(engine, (TweakEngine)sp.GetRequiredService<IProfileManager>());
        Assert.Same(engine, (TweakEngine)sp.GetRequiredService<ITweakValidator>());
    }

    // ── IEventBus is InProcessEventBus ────────────────────────────────────────

    [Fact]
    public void AddRegiLatticeCore_IEventBusIsInProcessEventBus()
    {
        using var sp = BuildProvider();
        Assert.IsType<InProcessEventBus>(sp.GetRequiredService<IEventBus>());
    }

    // ── IFavoritesRepository is JsonFavoritesRepository ───────────────────────

    [Fact]
    public void AddRegiLatticeCore_IFavoritesRepositoryIsJsonFavoritesRepository()
    {
        using var sp = BuildProvider();
        Assert.IsType<JsonFavoritesRepository>(sp.GetRequiredService<IFavoritesRepository>());
    }

    // ── Fluent return ─────────────────────────────────────────────────────────

    [Fact]
    public void AddRegiLatticeCore_ReturnsSameServiceCollection()
    {
        var original = new ServiceCollection();
        var returned = original.AddRegiLatticeCore();
        Assert.Same(original, returned);
    }
}
