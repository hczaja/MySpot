using MySpot.Application.Abstractions;
using MySpot.Core.Repositories;
using MySpot.Core.Services;
using MySpot.Core.ValueObjects;

namespace MySpot.Application.Commands.Handlers;

public class ReserveParkingSpotForCleaningHandler
    : ICommandHandler<ReserveParkingSpotForCleaning>
{
    private readonly IWeeklyParkingSpotRepository _weeklyParkingSpotRepository;
    private readonly IParkingReservationService _parkingReservationService;

    public ReserveParkingSpotForCleaningHandler(
        IWeeklyParkingSpotRepository weeklyParkingSpotRepository,
        IParkingReservationService parkingReservationService)
    {
        _weeklyParkingSpotRepository = weeklyParkingSpotRepository;
        _parkingReservationService = parkingReservationService;
    }

    public async Task HandleAsync(ReserveParkingSpotForCleaning command)
    {
        var week = new Week(command.Date);
        
        var weeklyParkingSpots = await _weeklyParkingSpotRepository.GetAllByWeekAsync(week);

        _parkingReservationService.ReserveParkingForCleaning(
            weeklyParkingSpots, command.Date);

        // var tasks = weeklyParkingSpots.Select(parkingSpot => _repository.UpdateAsync(parkingSpot));
        // await Task.WhenAll(tasks);

        foreach (var parkingSpot in weeklyParkingSpots)
        {
            await _weeklyParkingSpotRepository.UpdateAsync(parkingSpot);
        }
    }
}
