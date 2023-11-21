using MySpot.Core.Services;
using MySpot.Core.ValueObjects;

namespace MySpot.Tests.Unit.Shared;

public class TestClock : IClock
{
    public Date Current() => new Date(DateTime.Parse("2023-11-21"));
}