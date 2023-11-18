using MySpot.Core.Exceptions;

namespace MySpot.Core.ValueObjects;

public sealed record ReservationId
{
    public Guid Id { get; }

    public ReservationId(Guid id)
    {
        if (id == Guid.Empty)
            throw new InvalidEntityIdException(id);

        Id = id;
    }

    public static bool operator ==(Guid guid, ReservationId parkingSpotId)
        => guid == parkingSpotId.Id;

    public static bool operator ==(ReservationId reservationId, Guid guid) => guid == reservationId;

    public static bool operator !=(Guid guid, ReservationId parkingSpotId)
        => guid == parkingSpotId.Id;

    public static bool operator !=(ReservationId reservationId, Guid guid) => guid != reservationId;

    public static implicit operator Guid(ReservationId reservationId) => reservationId.Id;
    public static implicit operator ReservationId(Guid id) => new(id);
}