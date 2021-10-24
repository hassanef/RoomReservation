using Microsoft.AspNetCore.Mvc;
using Reservation.Application.Queries.Contract;
using Reservation.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Reservation.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomQuery _roomQuery;

        public RoomController(IRoomQuery roomQuery)
        {
            _roomQuery = roomQuery;
        }
     
        [HttpGet("[action]/{officeId}")]
        public async Task<IActionResult> GetRooms(int officeId)
        {
            if (officeId <= 0)
                return BadRequest();

            return Ok(await _roomQuery.GetRooms(officeId));
        }
    }
}
