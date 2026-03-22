// tests/RegiLattice.Core.Tests/ScheduledTweakServiceTests.cs
// Sprint coverage — ScheduledTweakService CRUD and scheduling logic.

using RegiLattice.Core.Services;
using Xunit;

namespace RegiLattice.Core.Tests;

/// <summary>Tests for ScheduledTweakService: in-memory CRUD, GetDueTimerSchedules, TweakSchedule model.</summary>
public sealed class ScheduledTweakServiceTests
{
    // ── Initial state ────────────────────────────────────────────────────────

    [Fact]
    public void Schedules_FreshInstance_IsEmpty()
    {
        var svc = new ScheduledTweakService();
        Assert.Empty(svc.Schedules);
    }

    [Fact]
    public void Load_FileAbsent_DoesNotThrow()
    {
        // Verifies no exception when no file exists (graceful no-op)
        var svc = new ScheduledTweakService();
        var ex = Record.Exception(() => svc.Load());
        Assert.Null(ex);
    }

    // ── AddSchedule ──────────────────────────────────────────────────────────

    [Fact]
    public void AddSchedule_Single_IncreasesCountToOne()
    {
        var svc = new ScheduledTweakService();
        svc.AddSchedule(new TweakSchedule { TweakId = "perf-test", Trigger = ScheduleTrigger.OnBoot });
        Assert.Single(svc.Schedules);
    }

    [Fact]
    public void AddSchedule_DuplicateTweakId_ReplacesExisting()
    {
        var svc = new ScheduledTweakService();
        svc.AddSchedule(new TweakSchedule { TweakId = "perf-test", Trigger = ScheduleTrigger.OnBoot });
        svc.AddSchedule(new TweakSchedule { TweakId = "perf-test", Trigger = ScheduleTrigger.OnLogin });

        Assert.Single(svc.Schedules);
        Assert.Equal(ScheduleTrigger.OnLogin, svc.Schedules[0].Trigger);
    }

    [Fact]
    public void AddSchedule_CaseInsensitiveDuplicate_Replaces()
    {
        var svc = new ScheduledTweakService();
        svc.AddSchedule(new TweakSchedule { TweakId = "Perf-Test", Trigger = ScheduleTrigger.OnBoot });
        svc.AddSchedule(new TweakSchedule { TweakId = "perf-test", Trigger = ScheduleTrigger.Timer });

        Assert.Single(svc.Schedules);
    }

    [Fact]
    public void AddSchedule_MultipleDistinctIds_AllAdded()
    {
        var svc = new ScheduledTweakService();
        svc.AddSchedule(new TweakSchedule { TweakId = "tweak-a", Trigger = ScheduleTrigger.OnBoot });
        svc.AddSchedule(new TweakSchedule { TweakId = "tweak-b", Trigger = ScheduleTrigger.OnLogin });
        svc.AddSchedule(new TweakSchedule { TweakId = "tweak-c", Trigger = ScheduleTrigger.Timer });

        Assert.Equal(3, svc.Schedules.Count);
    }

    [Fact]
    public void AddSchedule_NullArgument_ThrowsArgumentNullException()
    {
        var svc = new ScheduledTweakService();
        Assert.Throws<ArgumentNullException>(() => svc.AddSchedule(null!));
    }

    // ── RemoveSchedule ────────────────────────────────────────────────────────

    [Fact]
    public void RemoveSchedule_ExistingId_ReturnsTrueAndRemoves()
    {
        var svc = new ScheduledTweakService();
        svc.AddSchedule(new TweakSchedule { TweakId = "perf-test", Trigger = ScheduleTrigger.OnBoot });

        bool result = svc.RemoveSchedule("perf-test");

        Assert.True(result);
        Assert.Empty(svc.Schedules);
    }

    [Fact]
    public void RemoveSchedule_NonExistentId_ReturnsFalse()
    {
        var svc = new ScheduledTweakService();
        Assert.False(svc.RemoveSchedule("ghost-tweak"));
    }

    [Fact]
    public void RemoveSchedule_CaseInsensitive_Removes()
    {
        var svc = new ScheduledTweakService();
        svc.AddSchedule(new TweakSchedule { TweakId = "Perf-Test", Trigger = ScheduleTrigger.OnBoot });

        bool result = svc.RemoveSchedule("perf-test");
        Assert.True(result);
    }

    // ── GetSchedule ───────────────────────────────────────────────────────────

    [Fact]
    public void GetSchedule_ExistingId_ReturnsCorrectSchedule()
    {
        var svc = new ScheduledTweakService();
        svc.AddSchedule(
            new TweakSchedule
            {
                TweakId = "perf-test",
                Trigger = ScheduleTrigger.OnLogin,
                IntervalMinutes = 90,
            }
        );

        var found = svc.GetSchedule("perf-test");

        Assert.NotNull(found);
        Assert.Equal("perf-test", found.TweakId);
        Assert.Equal(ScheduleTrigger.OnLogin, found.Trigger);
        Assert.Equal(90, found.IntervalMinutes);
    }

    [Fact]
    public void GetSchedule_NonExistentId_ReturnsNull()
    {
        var svc = new ScheduledTweakService();
        Assert.Null(svc.GetSchedule("nothing-here"));
    }

    [Fact]
    public void GetSchedule_CaseInsensitive_Finds()
    {
        var svc = new ScheduledTweakService();
        svc.AddSchedule(new TweakSchedule { TweakId = "Perf-Test", Trigger = ScheduleTrigger.OnBoot });

        Assert.NotNull(svc.GetSchedule("perf-test"));
        Assert.NotNull(svc.GetSchedule("PERF-TEST"));
    }

    // ── RecordLastRun ─────────────────────────────────────────────────────────

    [Fact]
    public void RecordLastRun_ExistingId_UpdatesTimestamp()
    {
        var svc = new ScheduledTweakService();
        svc.AddSchedule(
            new TweakSchedule
            {
                TweakId = "perf-test",
                Trigger = ScheduleTrigger.Timer,
                IntervalMinutes = 60,
            }
        );

        var runTime = new DateTime(2025, 1, 15, 10, 30, 0, DateTimeKind.Utc);
        svc.RecordLastRun("perf-test", runTime);

        Assert.Equal(runTime, svc.GetSchedule("perf-test")!.LastRun);
    }

    [Fact]
    public void RecordLastRun_NonExistentId_DoesNotThrow()
    {
        var svc = new ScheduledTweakService();
        var ex = Record.Exception(() => svc.RecordLastRun("ghost-tweak", DateTime.UtcNow));
        Assert.Null(ex);
    }

    // ── SetEnabled ────────────────────────────────────────────────────────────

    [Fact]
    public void SetEnabled_False_DisablesSchedule()
    {
        var svc = new ScheduledTweakService();
        svc.AddSchedule(new TweakSchedule { TweakId = "perf-test", Trigger = ScheduleTrigger.OnBoot });

        svc.SetEnabled("perf-test", false);

        Assert.False(svc.GetSchedule("perf-test")!.Enabled);
    }

    [Fact]
    public void SetEnabled_True_EnablesDisabledSchedule()
    {
        var svc = new ScheduledTweakService();
        svc.AddSchedule(
            new TweakSchedule
            {
                TweakId = "perf-test",
                Trigger = ScheduleTrigger.OnBoot,
                Enabled = false,
            }
        );

        svc.SetEnabled("perf-test", true);

        Assert.True(svc.GetSchedule("perf-test")!.Enabled);
    }

    // ── GetDueTimerSchedules ──────────────────────────────────────────────────

    [Fact]
    public void GetDueTimerSchedules_OverdueSchedule_ReturnedAsDue()
    {
        var svc = new ScheduledTweakService();
        // 30-min interval, last ran 60 mins ago → overdue
        svc.AddSchedule(
            new TweakSchedule
            {
                TweakId = "perf-test",
                Trigger = ScheduleTrigger.Timer,
                IntervalMinutes = 30,
                LastRun = DateTime.UtcNow.AddMinutes(-60),
                Enabled = true,
            }
        );

        Assert.Single(svc.GetDueTimerSchedules());
    }

    [Fact]
    public void GetDueTimerSchedules_NotYetDue_NotReturned()
    {
        var svc = new ScheduledTweakService();
        // 120-min interval, last ran 10 mins ago → not due yet
        svc.AddSchedule(
            new TweakSchedule
            {
                TweakId = "perf-test",
                Trigger = ScheduleTrigger.Timer,
                IntervalMinutes = 120,
                LastRun = DateTime.UtcNow.AddMinutes(-10),
                Enabled = true,
            }
        );

        Assert.Empty(svc.GetDueTimerSchedules());
    }

    [Fact]
    public void GetDueTimerSchedules_DisabledSchedule_NotReturned()
    {
        var svc = new ScheduledTweakService();
        // Overdue but disabled
        svc.AddSchedule(
            new TweakSchedule
            {
                TweakId = "perf-test",
                Trigger = ScheduleTrigger.Timer,
                IntervalMinutes = 30,
                LastRun = DateTime.UtcNow.AddMinutes(-60),
                Enabled = false,
            }
        );

        Assert.Empty(svc.GetDueTimerSchedules());
    }

    [Fact]
    public void GetDueTimerSchedules_NeverRun_IsDue()
    {
        var svc = new ScheduledTweakService();
        // LastRun = null with any positive interval → treated as due
        svc.AddSchedule(
            new TweakSchedule
            {
                TweakId = "perf-test",
                Trigger = ScheduleTrigger.Timer,
                IntervalMinutes = 60,
                LastRun = null,
                Enabled = true,
            }
        );

        Assert.Single(svc.GetDueTimerSchedules());
    }

    [Fact]
    public void GetDueTimerSchedules_OnBootTrigger_NotIncluded()
    {
        var svc = new ScheduledTweakService();
        // OnBoot trigger is not Timer → excluded from timer due check
        svc.AddSchedule(
            new TweakSchedule
            {
                TweakId = "perf-test",
                Trigger = ScheduleTrigger.OnBoot,
                IntervalMinutes = 0,
                LastRun = null,
                Enabled = true,
            }
        );

        Assert.Empty(svc.GetDueTimerSchedules());
    }

    [Fact]
    public void GetDueTimerSchedules_ZeroInterval_NotDue()
    {
        var svc = new ScheduledTweakService();
        // IntervalMinutes = 0 means it cannot be a due timer
        svc.AddSchedule(
            new TweakSchedule
            {
                TweakId = "perf-test",
                Trigger = ScheduleTrigger.Timer,
                IntervalMinutes = 0,
                LastRun = null,
                Enabled = true,
            }
        );

        Assert.Empty(svc.GetDueTimerSchedules());
    }

    // ── TweakSchedule record defaults ─────────────────────────────────────────

    [Fact]
    public void TweakSchedule_DefaultEnabled_IsTrue()
    {
        var s = new TweakSchedule { TweakId = "test", Trigger = ScheduleTrigger.OnBoot };
        Assert.True(s.Enabled);
    }

    [Fact]
    public void TweakSchedule_DefaultCreatedAt_IsRecentUtc()
    {
        var before = DateTime.UtcNow.AddSeconds(-5);
        var s = new TweakSchedule { TweakId = "test", Trigger = ScheduleTrigger.OnBoot };
        var after = DateTime.UtcNow.AddSeconds(5);

        Assert.InRange(s.CreatedAt, before, after);
    }

    [Fact]
    public void TweakSchedule_DefaultLastRun_IsNull()
    {
        var s = new TweakSchedule { TweakId = "test", Trigger = ScheduleTrigger.Timer };
        Assert.Null(s.LastRun);
    }

    [Fact]
    public void TweakSchedule_AllTriggerValues_AreDistinct()
    {
        var values = Enum.GetValues<ScheduleTrigger>();
        Assert.Equal(3, values.Length);
        Assert.Contains(ScheduleTrigger.OnBoot, values);
        Assert.Contains(ScheduleTrigger.OnLogin, values);
        Assert.Contains(ScheduleTrigger.Timer, values);
    }
}
