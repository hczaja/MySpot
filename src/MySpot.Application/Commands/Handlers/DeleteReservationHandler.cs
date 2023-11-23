using MySpot.Application.Abstractions;
using MySpot.Application.Exceptions;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.Services;

namespace MySpot.Application.Commands.Handlers;

public class DeleteReservationHandler
    : ICommandHandler<DeleteReservation>
{
    private readonly IWeeklyParkingSpotRepository _weeklyParkingSpotRepository;

    public DeleteReservationHandler(IWeeklyParkingSpotRepository weeklyParkingSpotRepository)
        => _weeklyParkingSpotRepository = weeklyParkingSpotRepository;

    public async Task HandleAsync(DeleteReservation command)
    {
        var weeklyParkingSpot = await GetWeeklyParkingSpotByReservationAsync(command.ReservationId);
        if (weeklyParkingSpot is null)
            throw new WeeklyParkingSpotNotFoundException(command.ReservationId);

        weeklyParkingSpot.RemoveReservation(command.ReservationId);
        await _weeklyParkingSpotRepository.DeleteAsync(weeklyParkingSpot);
    }

    private async Task<WeeklyParkingSpot> GetWeeklyParkingSpotByReservationAsync(Guid reservationId)
    {
        var weeklyParkingSpots = await _weeklyParkingSpotRepository.GetAllAsync();
        return weeklyParkingSpots.SingleOrDefault(x => x.Reservations.Any(r => r.Id == reservationId));
    }
}
