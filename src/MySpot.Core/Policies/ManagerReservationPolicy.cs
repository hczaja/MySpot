using MySpot.Core.Entities;
using MySpot.Core.ValueObjects;

namespace MySpot.Core.Policies;

public sealed class ManagerReservationPolicy : IReservationPolicy
{
    public bool CanBeApplied(JobTitle jobTitle)
        => jobTitle == JobTitle.Manager;

    public bool CanReserved(IEnumerable<WeeklyParkingSpot> weeklyParkingSpots, EmployeeName employeeName)
    {
        var totalEmployeeReservations = weeklyParkingSpots
            .SelectMany(w => w.Reservations)
            .OfType<VehicleReservation>()
            .Count(r => r.EmployeeName == employeeName);

        return totalEmployeeReservations <= 4;
    }
}
