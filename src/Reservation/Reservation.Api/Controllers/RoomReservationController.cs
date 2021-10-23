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
      
        //// GET api/v1/<RoomReservationApi>/5
        //[HttpGet("{location}")]
        //public async Task<IActionResult> Get(int location)
        //{
        //    var result = await _query.GetRoomReservations((Location) location);

        //    return Ok(result);
        //}

        // POST api/v1/<RoomReservationApi>
        [HttpPost]
        public async Task<IActionResult> Post(RoomReservationCommand cmd)
        {
            var result = await _mediator.Send(cmd);

            if (result == true)
                return Ok();
            return BadRequest();
        }

    }
}
