using MySpot.Application.Abstractions;
using MySpot.Application.Exceptions;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;

namespace MySpot.Application.Commands.Handlers;

public class ChangeReservationLicensePlateHandler
    : ICommandHandler<ChangeReservationLicensePlate>
{
    private readonly IWeeklyParkingSpotRepository _weeklyParkingSpotRepository;

    public ChangeReservationLicensePlateHandler(IWeeklyParkingSpotRepository weeklyParkingSpotRepository)
        => _weeklyParkingSpotRepository = weeklyParkingSpotRepository;

    public async Task HandleAsync(ChangeReservationLicensePlate command)
    {
        var weeklyParkingSpot = await GetWeeklyParkingSpotByReservationAsync(command.ReservationId);
        if (weeklyParkingSpot is null)
            throw new WeeklyParkingSpotNotFoundException();

        var existingReservation = weeklyParkingSpot.Reservations
            .OfType<VehicleReservation>()
            .SingleOrDefault(x => x.Id == command.ReservationId);
        
        if (existingReservation is null)
            throw new ReservationNotFoundException(command.ReservationId);
        
        existingReservation.ChangeLicensePlate(command.LicensePlate);
        await _weeklyParkingSpotRepository.UpdateAsync(weeklyParkingSpot);
    }

    private async Task<WeeklyParkingSpot> GetWeeklyParkingSpotByReservationAsync(Guid reservationId)
    {
        var weeklyParkingSpots = await _weeklyParkingSpotRepository.GetAllAsync();
        return weeklyParkingSpots.SingleOrDefault(x => x.Reservations.Any(r => r.Id == reservationId));
    }
}
