using MySpot.Core.Entities;
using MySpot.Core.Policies;
using MySpot.Core.ValueObjects;

namespace MySpot.Tests.Unit.Shared;

public class TestNoReservationPolicy : IReservationPolicy
{
    public bool CanBeApplied(JobTitle jobTitle) => true;
    public bool CanReserved(IEnumerable<WeeklyParkingSpot> weeklyParkingSpots, EmployeeName employeeName) => true;
}
