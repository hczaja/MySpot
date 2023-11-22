using System.ComponentModel.DataAnnotations;
using MySpot.Application.Commands;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Application.Services;
using MySpot.Core.ValueObjects;
using MySpot.Tests.Unit.Shared;
using Shouldly;
using MySpot.Infrastructure.DAL.Repositories;
using MySpot.Core.Services;
using MySpot.Core.Policies;

namespace MySpot.Tests.Unit.Services;

public class ReservationServiceTests
{
   [Test] 
   public async Task given_reservation_for_not_taken_date_add_reservation_should_succeed()
   {
      var parkingSpots = await _weeklyParkingSpotRepository.GetAllAsync();
      var parkingSpot = parkingSpots.First();

      var command = new ReserveParkingSpotForVehicle(
         parkingSpot.Id, Guid.NewGuid(), "Jon Snow", "ABC123", DateTime.Parse("2023-11-23"), 2);
   
      var reservationId = await _reservationService.ReserveForVehicleAsync(command);

      reservationId.ShouldNotBeNull();
      reservationId.Value.ShouldBe(command.ReservationId);
   }

   #region Arrange

   private readonly IClock _clock = new TestClock();
   private readonly IWeeklyParkingSpotRepository _weeklyParkingSpotRepository;
   private readonly IReservationService _reservationService;
   private readonly IEnumerable<IReservationPolicy> _polices;
   private readonly IParkingReservationService _parkingReservationService;

   public ReservationServiceTests()
   {
      _clock = new TestClock();
      _weeklyParkingSpotRepository = new InMemoryWeeklyParkingSpotRepository(_clock);
      _polices = new List<IReservationPolicy>() { new RegularEmployeeReservationPolicy(_clock) };
      _parkingReservationService = new ParkingReservationService(_polices, _clock);
      _reservationService = new ReservationsService(_clock, _weeklyParkingSpotRepository, _parkingReservationService);
   }

   #endregion
}