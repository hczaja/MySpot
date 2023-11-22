namespace MySpot.Application.Commands;

public record ReserveParkingSpotForVehicle(Guid ParkingSpotId, Guid ReservationId, 
    string EmployeeName, string LicensePlate, DateTimeOffset Date);