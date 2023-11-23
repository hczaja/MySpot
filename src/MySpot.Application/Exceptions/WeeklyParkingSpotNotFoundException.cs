using MySpot.Core.Exceptions;

namespace MySpot.Application.Exceptions;

public class WeeklyParkingSpotNotFoundException : CustomException
{
    public Guid Id { get; }

    public WeeklyParkingSpotNotFoundException(Guid id) 
        : base($"Weekly parking spot with ID: {id} was not found.")
    {
            this.Id = id;
    }

    public WeeklyParkingSpotNotFoundException() 
        : base($"Weekly parking spot was not found.")
    { }
}
