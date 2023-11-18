using MySpot.Core.Exceptions;

namespace MySpot.Core.ValueObjects;

public sealed record ParkingSpotId
{
    public Guid Id { get; }

    public ParkingSpotId(Guid id)
    {
        if (id == Guid.Empty)
            throw new InvalidEntityIdException(id);

        Id = id;
    }

    public static bool operator ==(Guid guid, ParkingSpotId parkingSpotId)
        => guid == parkingSpotId.Id;

    public static bool operator ==(ParkingSpotId parkingSpotId, Guid guid) => guid == parkingSpotId;

    public static bool operator !=(Guid guid, ParkingSpotId parkingSpotId)
        => guid == parkingSpotId.Id;

    public static bool operator !=(ParkingSpotId parkingSpotId, Guid guid) => guid != parkingSpotId;

    public static implicit operator Guid(ParkingSpotId parkingSpotId) => parkingSpotId.Id;
    public static implicit operator ParkingSpotId(Guid id) => new(id);
}
