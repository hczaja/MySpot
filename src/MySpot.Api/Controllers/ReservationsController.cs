using Microsoft.AspNetCore.Mvc;
using MySpot.Api.Commands;
using MySpot.Api.Entities;
using MySpot.Api.Services;
using MySpot.Api.ValueObjects;

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
    public ActionResult<IEnumerable<Reservation>> Get() => Ok(_service.GetAllWeekly());

    [HttpGet("{id:guid}")]
    public ActionResult<Reservation> Get(Guid id)
    {
        var reservation = _service.Get(id);
        if (reservation is null)
        {
            return NotFound();
        }

        return Ok(reservation);
    }

    [HttpPost]
    public ActionResult Post(CreateReservation command)
    {
        var id = _service.Create(command with { ReservationId = Guid.NewGuid() });
        if (id is null)
        {
            return BadRequest();
        }
        
        return CreatedAtAction(nameof(Get), new {id}, null);
    }

    [HttpPut("{id:guid}")]
    public ActionResult Put(Guid id, ChangeReservationLicensePlate command)
    {
        if (_service.Update(command with { ReservationId = id }))
        {
            return NoContent();
        }

        return NotFound();
    }

    [HttpDelete("{id:guid}")]
    public ActionResult Delete(Guid id)
    {
        if (_service.Delete(new DeleteReservation(id)))
        {
            return NoContent();
        }

        return NotFound();
    }
}
