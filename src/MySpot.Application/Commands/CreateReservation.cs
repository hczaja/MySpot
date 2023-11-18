namespace MySpot.Application.Commands;

public record CreateReservation(Guid ParkingSpotId, Guid ReservationId, string EmployeeName, string LicensePlate, DateTimeOffset Date);
