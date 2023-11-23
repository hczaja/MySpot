namespace MySpot.Application.DTO;

public class ReservationDto
{
    public Guid Id { get; set; }
    public Guid ParkingSpotId { get; set; }
    public string EmployeeName { get; set; }
    public DateTimeOffset Date { get; set; }
    public string Type { get; set; }
}
