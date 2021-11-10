using MediatR;
using Microsoft.AspNetCore.Mvc;
using Reservation.Application.Commands;
using Reservation.Application.Queries.Contract;
using Reservation.Domain.AggregatesModel;
using Reservation.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Reservation.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RoomReservationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RoomReservationController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> CreateRoomReservation(CreateRoomReservationCommand cmd)
        {
            var result = await _mediator.Send(cmd);

            if (result == true)
                return Ok();
            return BadRequest();
        }
        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteRoomReservation(DeleteRoomReservationCommand cmd)
        {
            var result = await _mediator.Send(cmd);

            if (result == true)
                return Ok();
            return BadRequest();
        }
    }
}
