using MySpot.Core.Exceptions;

namespace MySpot.Application.Exceptions;

public class ReservationNotFoundException : CustomException
{
    public Guid Id { get; }

    public ReservationNotFoundException(Guid id) 
        : base($"Reservation with ID: {id} was not found.")
    {
            this.Id = id;
    }
}
