using Microsoft.EntityFrameworkCore;
using Reservation.Application.Queries.Contract;
using Reservation.Domain.AggregatesModel;
using Reservation.Domain.ReadModels;
using Reservation.Infrastructure.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reservation.Application.Queries
{
    public class RoomReservationQuery: IRoomReservationQuery
    {
        private readonly ReservationDbContextReadOnly _contextReadOnly;

        public RoomReservationQuery(ReservationDbContextReadOnly contextReadOnly)
        {
            _contextReadOnly = contextReadOnly;
        }

        public async Task<List<RoomReservationReadModel>> GetRoomReservations(Location location)
        {
            return await _contextReadOnly.RoomReservations.Include(x => x.ResourceReservations)
                                                          .Include(x => x.Room)
                                                          .ThenInclude(x => x.Office)
                                                          .Where(x => x.Room.Office.Location == location)
                                                          .ToListAsync();
        }
    }
}
