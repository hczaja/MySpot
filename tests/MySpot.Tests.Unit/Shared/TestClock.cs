using MySpot.Core.Services;
using MySpot.Core.ValueObjects;

namespace MySpot.Tests.Unit.Shared;

public class TestClock : IClock
{
    public Date Current() => new Date(new DateTime(2023, 11, 22, 17, 0, 0));
}