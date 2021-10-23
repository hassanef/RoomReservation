using MediatR;
using Microsoft.AspNetCore.Mvc;
using Reservation.Application.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Reservation.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ResourceReservationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ResourceReservationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AddResourceReservation(ResourceReservationCommand cmd)
        {
            var result = await _mediator.Send(cmd);

            if (result == true)
                return Ok();
            return BadRequest();
        }

    }
}
