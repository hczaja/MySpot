using System.ComponentModel.DataAnnotations;
using MySpot.Api.Commands;
using MySpot.Api.Entities;
using MySpot.Api.Services;
using MySpot.Api.ValueObjects;
using Shouldly;

namespace MySpot.Tests.Unit.Services;

public class ReservationServiceTests
{
   [Test] 
   public void given_reservation_for_not_taken_date_add_reservation_should_succeed()
   {
      var parkingSpot = _weeklyParkingSpots.First();
      var command = new CreateReservation(
         parkingSpot.Id, Guid.NewGuid(), "Jon Snow", "ABC123", DateTime.UtcNow.Date);
   
      var reservationId = _reservationService.Create(command);

      reservationId.ShouldNotBeNull();
      reservationId.Value.ShouldBe(command.ReservationId);
   }

   #region Arrange

   private readonly Clock _clock;
   private readonly List<WeeklyParkingSpot> _weeklyParkingSpots;
   private readonly ReservationsService _reservationService;

   public ReservationServiceTests()
   {
      _clock = new Clock();

      _weeklyParkingSpots = new()
      {
         new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000001"), new Week(_clock.Current()), "P1"),
         new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000002"), new Week(_clock.Current()), "P2"),
         new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000003"), new Week(_clock.Current()), "P3"),
         new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000004"), new Week(_clock.Current()), "P4"),
         new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000005"), new Week(_clock.Current()), "P5")
      };

      _reservationService = new ReservationsService(_weeklyParkingSpots);
   }

   #endregion
}