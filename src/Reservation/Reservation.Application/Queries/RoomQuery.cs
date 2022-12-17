using Microsoft.EntityFrameworkCore;
using Reservation.Application.Queries.Contract;
using Reservation.Domain.IRepositories;
using Reservation.Domain.ReadModels;
using Reservation.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Application.Queries
{
    public class RoomQuery:IRoomQuery
    {
        private readonly ReservationDbContextReadOnly _contextReadOnly;

        public RoomQuery(ReservationDbContextReadOnly contextReadOnly)
        {
            _contextReadOnly = contextReadOnly;
        }
        public async Task<List<RoomDto>> GetRooms(int officeId)
        {
            return await _contextReadOnly.Rooms
                                         .Where(x => x.OfficeId == officeId)
                                         .Include(x => x.RoomReservations)
                                         .ThenInclude(x => x.ResourceReservations)
                                         .ToListAsync();
        }
    }
}
