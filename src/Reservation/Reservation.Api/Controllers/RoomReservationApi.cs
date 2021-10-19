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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Reservation.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RoomReservationApi : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IRoomReservationQuery _query;

        public RoomReservationApi(IMediator mediator, IRoomReservationQuery query)
        {
            _mediator = mediator;
            _query = query;
        }
      
        // GET api/v1/<RoomReservationApi>/5
        [HttpGet("{location}")]
        public async Task<IActionResult> Get(int location)
        {
            var result = await _query.GetRoomReservations((Location) location);

            return Ok(result);
        }

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
