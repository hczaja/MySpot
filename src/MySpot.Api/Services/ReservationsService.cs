using MySpot.Api.Models;

namespace MySpot.Api.Services;

public class ReservationsService
{
    private static int _id = 1;
    private static readonly List<Reservation> Reservations = new();
    private static readonly List<string> ParkingSpotNames = new()
    {
        "P1", "P2", "P3", "P4", "P5"
    };
    
    public Reservation Get(int id) 
        => Reservations.SingleOrDefault(r => r.Id == id);

    public IEnumerable<Reservation> GetAll() 
        => Reservations;

    public int? Create(Reservation reservation)
    {
        if (ParkingSpotNames.All(spot => spot != reservation.ParkingSpotName))
        {
            return default;
        }

        var reservationAlreadyExists = Reservations.Any(r =>
            r.ParkingSpotName == reservation.ParkingSpotName &&
            r.Date.Date == reservation.Date.Date);

        if (reservationAlreadyExists)
        {
            return default;
        }

        reservation.Id = _id;
        _id++;
        
        reservation.Date = DateTime.UtcNow.AddDays(1).Date;
        Reservations.Add(reservation);

        return reservation.Id;
    }

    public bool Update(Reservation reservation)
    {
        var existingReservation = Reservations.SingleOrDefault(r => r.Id == reservation.Id);
        if (existingReservation is null)
        {
            return false;
        }

        existingReservation.LicensePlate = reservation.LicensePlate;
        return true;
    }

    public bool Delete(int id)
    {
        var existingReservation = Reservations.SingleOrDefault(r => r.Id == id);
        if (existingReservation is null)
        {
            return false;
        }

        Reservations.Remove(existingReservation);
        return true;
    }
}