using MySpot.Core.Entities;
using MySpot.Core.ValueObjects;

namespace MySpot.Core.Policies;

public sealed class BossReservationPolicy : IReservationPolicy
{
    public bool CanBeApplied(JobTitle jobTitle)
        => jobTitle == JobTitle.Boss;

    public bool CanReserved(IEnumerable<WeeklyParkingSpot> weeklyParkingSpots, EmployeeName employeeName)
        => true;
}