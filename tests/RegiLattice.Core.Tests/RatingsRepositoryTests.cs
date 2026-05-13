// RegiLattice.Core.Tests — RatingsRepositoryTests.cs
// Tests for IRatingsRepository / JsonRatingsRepository (B.3).

using RegiLattice.Core.Services;
using Xunit;

namespace RegiLattice.Core.Tests;

public sealed class RatingsRepositoryTests : IDisposable
{
    private readonly string _tempFile;
    private readonly JsonRatingsRepository _repo;

    public RatingsRepositoryTests()
    {
        _tempFile = Path.GetTempFileName();
        File.Delete(_tempFile); // start with no file
        _repo = new JsonRatingsRepository(_tempFile);
    }

    public void Dispose()
    {
        if (File.Exists(_tempFile))
            File.Delete(_tempFile);
    }

    // ── Interface resolution via DI ──────────────────────────────────────────

    [Fact]
    public void JsonRatingsRepository_ImplementsInterface()
    {
        Assert.IsAssignableFrom<IRatingsRepository>(_repo);
    }

    // ── Rate and GetRating ───────────────────────────────────────────────────

    [Fact]
    public void Rate_ValidStars_RecordsRating()
    {
        _repo.Rate("priv-foo", 4);
        var r = _repo.GetRating("priv-foo");
        Assert.NotNull(r);
        Assert.Equal(4, r.Stars);
    }

    [Fact]
    public void Rate_StarsOutOfRange_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _repo.Rate("priv-foo", 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => _repo.Rate("priv-foo", 6));
    }

    [Fact]
    public void Rate_WithNote_StoredCorrectly()
    {
        _repo.Rate("priv-bar", 5, "very good");
        var r = _repo.GetRating("priv-bar");
        Assert.NotNull(r);
        Assert.Equal("very good", r.Note);
    }

    [Fact]
    public void GetRating_UnknownId_ReturnsNull()
    {
        Assert.Null(_repo.GetRating("does-not-exist"));
    }

    // ── AllRatings ───────────────────────────────────────────────────────────

    [Fact]
    public void AllRatings_EmptyFile_ReturnsEmptyDict()
    {
        Assert.Empty(_repo.AllRatings());
    }

    [Fact]
    public void AllRatings_AfterMultipleRates_ContainsAllEntries()
    {
        _repo.Rate("a", 3);
        _repo.Rate("b", 5);
        _repo.Rate("c", 1);
        var all = _repo.AllRatings();
        Assert.Equal(3, all.Count);
        Assert.Contains("a", all);
        Assert.Contains("b", all);
    }

    // ── RemoveRating ─────────────────────────────────────────────────────────

    [Fact]
    public void RemoveRating_ExistingId_RemovesIt()
    {
        _repo.Rate("priv-baz", 2);
        _repo.RemoveRating("priv-baz");
        Assert.Null(_repo.GetRating("priv-baz"));
    }

    [Fact]
    public void RemoveRating_NonExistent_DoesNotThrow()
    {
        var ex = Record.Exception(() => _repo.RemoveRating("no-such-tweak"));
        Assert.Null(ex);
    }

    // ── TopRated ─────────────────────────────────────────────────────────────

    [Fact]
    public void TopRated_ReturnsSortedDescending()
    {
        _repo.Rate("low", 1);
        _repo.Rate("mid", 3);
        _repo.Rate("high", 5);
        var top = _repo.TopRated(3);
        Assert.Equal(3, top.Count);
        Assert.Equal(5, top[0].Rating.Stars);
        Assert.Equal(1, top[2].Rating.Stars);
    }

    [Fact]
    public void TopRated_LimitRespected()
    {
        for (int i = 1; i <= 5; i++)
            _repo.Rate($"t{i}", i);
        var top = _repo.TopRated(3);
        Assert.Equal(3, top.Count);
    }

    // ── AverageRating ────────────────────────────────────────────────────────

    [Fact]
    public void AverageRating_NoRatings_ReturnsNull()
    {
        Assert.Null(_repo.AverageRating());
    }

    [Fact]
    public void AverageRating_MultipleRatings_ReturnsCorrectMean()
    {
        _repo.Rate("x", 2);
        _repo.Rate("y", 4);
        var avg = _repo.AverageRating();
        Assert.NotNull(avg);
        Assert.Equal(3.0, avg.Value, precision: 1);
    }

    // ── Flush + persistence ──────────────────────────────────────────────────

    [Fact]
    public void Flush_WritesToDisk()
    {
        _repo.Rate("priv-persist", 5);
        _repo.Flush();
        Assert.True(File.Exists(_tempFile));
    }

    [Fact]
    public void Rate_OverridesExistingRating()
    {
        _repo.Rate("priv-same", 2);
        _repo.Rate("priv-same", 5, "updated");
        var r = _repo.GetRating("priv-same");
        Assert.NotNull(r);
        Assert.Equal(5, r.Stars);
        Assert.Equal("updated", r.Note);
    }
}
