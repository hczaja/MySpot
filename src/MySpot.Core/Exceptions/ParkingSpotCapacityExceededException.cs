using MySpot.Core.ValueObjects;

namespace MySpot.Core.Exceptions;

public sealed class ParkingSpotCapacityExceededException : CustomException
{
    public ParkingSpotCapacityExceededException(ParkingSpotId id) 
        : base($"Parking spot with id: {id} exceeds its reservation capacity.")
    {
        Id = id;
    }

    public ParkingSpotId Id { get; }
}
