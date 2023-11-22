using Microsoft.AspNetCore.Mvc;
using MySpot.Application.Commands;
using MySpot.Application.Services;
using MySpot.Core.Entities;

namespace MySpot.Api.Controllers;

[ApiController]
[Route("Reservations")]
public class ReservationsController : ControllerBase
{
    private readonly IReservationService _service;

    public ReservationsController(IReservationService reservationsService)
    {
        _service = reservationsService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Reservation>>> Get() 
        => Ok(await _service.GetAllWeeklyAsync());

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Reservation>> Get(Guid id)
    {
        var reservation = await _service.GetAsync(id);
        if (reservation is null)
        {
            return NotFound();
        }

        return Ok(reservation);
    }

    [HttpPost("vehicle")]
    public async Task<ActionResult> Post(ReserveParkingSpotForVehicle command)
    {
        var id = await _service.ReserveForVehicleAsync(command with { ReservationId = Guid.NewGuid() });
        if (id is null)
        {
            return BadRequest();
        }
        
        return CreatedAtAction(nameof(Get), new {id}, null);
    }

    [HttpPost("cleaning")]
    public async Task<ActionResult> Post(ReserveParkingSpotForCleaning command)
    {
        await _service.ReserveForCleaningAsync(command);
        return Ok();
    }


    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Put(Guid id, ChangeReservationLicensePlate command)
    {
        if (await _service.ChangeReservationLicensePlateAsync(command with { ReservationId = id }))
        {
            return NoContent();
        }

        return NotFound();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        if (await _service.DeleteAsync(new DeleteReservation(id)))
        {
            return NoContent();
        }

        return NotFound();
    }
}
