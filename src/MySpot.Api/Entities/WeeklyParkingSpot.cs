using MySpot.Api.Exceptions;
using MySpot.Api.ValueObjects;

namespace MySpot.Api.Entities;

public class WeeklyParkingSpot
{
    private readonly HashSet<Reservation> _reservations = new();
    
    public ParkingSpotId Id { get; }
    public Week Week { get; }
    public string Name { get; }
    public IEnumerable<Reservation> Reservations => _reservations;

    public WeeklyParkingSpot(Guid id, Week week, string name)
    {
        Id = id;
        Week = week;
        Name = name;
    }

    public void AddReservation(Reservation reservation, Date now)
    {
        bool isInvalidDate = reservation.Date < Week.From ||
                             reservation.Date > Week.To ||
                             reservation.Date < now;

        if (isInvalidDate)
            throw new InvalidReservationDayException(reservation.Date.Value.Date);

        bool reservationAlreadyExists = Reservations.Any(r => 
            r.Date == reservation.Date);

        if (reservationAlreadyExists)
            throw new ParkingSpotAlreadyReservedException(Name, reservation.Date.Value.Date);


        _reservations.Add(reservation);
    }

    internal void RemoveReservation(ReservationId reservationId)
        => _reservations.RemoveWhere(r => r.Id == reservationId);
}
