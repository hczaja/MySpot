namespace MySpot.Core.Exceptions;

public class InvalidReservationDayException : CustomException
{
    public DateTime Date { get; }

    public InvalidReservationDayException(DateTime date) 
        : base($"Reservation date: {date:d} is invalid.")
    {
        Date = date;
    }
}