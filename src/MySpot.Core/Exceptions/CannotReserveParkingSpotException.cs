using MySpot.Core.ValueObjects;

namespace MySpot.Core.Exceptions;

public class CannotReserveParkingSpotException : CustomException
{
    public ParkingSpotId ParkingSpotId { get; }

    public CannotReserveParkingSpotException(ParkingSpotId parkingSpotId) 
        : base($"Cannot reserve parking spot with id {parkingSpotId}.")
    {
            this.ParkingSpotId = parkingSpotId;
    }
}
