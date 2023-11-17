using MySpot.Api.Commands;
using MySpot.Api.DTO;
using MySpot.Api.Entities;
using MySpot.Api.ValueObjects;

namespace MySpot.Api.Services;

public class ReservationsService
{
    private readonly Clock _clock = new();
    private readonly List<WeeklyParkingSpot> _weeklyParkingSpots;

    public ReservationsService(List<WeeklyParkingSpot> weeklyParkingSpots)
    {
        _weeklyParkingSpots = weeklyParkingSpots;
    }
    
    public ReservationDto Get(Guid id) => GetAllWeekly().SingleOrDefault(r => r.Id == id);

    public IEnumerable<ReservationDto> GetAllWeekly() => 
        _weeklyParkingSpots.SelectMany(x => x.Reservations)
            .Select(r => new ReservationDto()
            {
                Id = r.Id,
                ParkingSpotId = r.ParkingSpotId,
                EmployeeName = r.EmployeeName,
                Date = r.Date
            });

    public Guid? Create(CreateReservation command)
    {
        var weeklyParkingSpot = _weeklyParkingSpots.SingleOrDefault(x => x.Id == command.ParkingSpotId);
        if (weeklyParkingSpot is null)
        {
            return default;
        }
 
        var reservation = new Reservation(command.ReservationId, 
            command.ParkingSpotId, command.EmployeeName,
            command.LicensePlate, command.Date);

        weeklyParkingSpot.AddReservation(reservation, _clock.Current());

        return reservation.Id;
    }

    public bool Update(ChangeReservationLicensePlate command)
    {
        var weeklyParkingSpot = GetWeeklyParkingSpotByReservation(command.ReservationId);
        if (weeklyParkingSpot is null)
        {
            return false;
        }

        var existingReservation = weeklyParkingSpot.Reservations.SingleOrDefault(x => x.Id == command.ReservationId);
        if (existingReservation is null)
        {
            return false;
        }
        
        if (existingReservation.Date <= _clock.Current())
        {
            return false;
        }

        existingReservation.ChangeLicensePlate(command.LicensePlate);
        return true;
    }

    public bool Delete(DeleteReservation command)
    {
        var weeklyParkingSpot = GetWeeklyParkingSpotByReservation(command.ReservationId);
        if (weeklyParkingSpot is null)
        {
            return false;
        }

        var existingReservation = weeklyParkingSpot.Reservations.SingleOrDefault(r => r.Id == command.ReservationId);
        if (existingReservation is null)
        {
            return false;
        }

        weeklyParkingSpot.RemoveReservation(command.ReservationId);
        return true;
    }

    private WeeklyParkingSpot GetWeeklyParkingSpotByReservation(Guid reservationId)
        => _weeklyParkingSpots.SingleOrDefault(x => x.Reservations.Any(r => r.Id == reservationId));
}