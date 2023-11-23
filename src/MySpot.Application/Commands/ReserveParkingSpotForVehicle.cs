using MySpot.Application.Abstractions;

namespace MySpot.Application.Commands;

public record ReserveParkingSpotForVehicle(Guid ParkingSpotId, Guid ReservationId, 
    string EmployeeName, string LicensePlate, DateTimeOffset Date, int Capacity) : ICommand;