namespace MySpot.Core.Exceptions;

public class ParkingSpotAlreadyReservedException : CustomException
{
    public string Name { get; }
    public DateTime Date { get; }

    public ParkingSpotAlreadyReservedException(string name, DateTime date) 
        : base($"Parking spot: {name} is already reserved at: {date:d}.")
    {
        Name = name;
        Date = date;
    }
}

