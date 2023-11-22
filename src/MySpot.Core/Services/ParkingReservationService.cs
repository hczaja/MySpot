using System.Reflection;
using System.Runtime.CompilerServices;
using MySpot.Core.Entities;
using MySpot.Core.Exceptions;
using MySpot.Core.Policies;
using MySpot.Core.ValueObjects;

[assembly: InternalsVisibleTo("MySpot.Tests.Unit")]
namespace MySpot.Core.Services;

internal sealed class ParkingReservationService : IParkingReservationService
{
    private readonly IEnumerable<IReservationPolicy> _policies;
    private readonly IClock _clock;

    public ParkingReservationService(IEnumerable<IReservationPolicy> policies, IClock clock)
    {
        _policies = policies;
        _clock = clock;
    }

    public void ReserveParkingForCleaning(IEnumerable<WeeklyParkingSpot> allParkingSpots, Date date)
    {
        foreach (var parkingSpot in allParkingSpots)
        {
            var reservationsForSameDate = parkingSpot.Reservations.Where(r => r.Date == date);
            parkingSpot.RemoveReservations(reservationsForSameDate);

            var cleaningReservation = new CleaningReservation(ReservationId.Create(), parkingSpot.Id, date);
            parkingSpot.AddReservation(cleaningReservation, _clock.Current());
        }
    }

    public void ReserveSpotForVehicle(IEnumerable<WeeklyParkingSpot> allParkingSpots, 
        JobTitle jobTitle, WeeklyParkingSpot parkingSpotToReserve, VehicleReservation reservation)
    {
        var parkingSpotId = parkingSpotToReserve.Id;
        var policy = _policies.SingleOrDefault(p => p.CanBeApplied(jobTitle));

        if (policy is null)
            throw new NoReservationPolicyFoundException(jobTitle);

        if (!policy.CanReserved(allParkingSpots, reservation.EmployeeName))
            throw new CannotReserveParkingSpotException(parkingSpotId);

        parkingSpotToReserve.AddReservation(reservation, _clock.Current());
    }
}