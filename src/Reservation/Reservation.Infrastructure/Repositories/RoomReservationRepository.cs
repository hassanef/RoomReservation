using Microsoft.AspNetCore.Http;
using Reservation.Domain.AggregatesModel;
using Reservation.Domain.IRepositories;
using Reservation.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Infrastructure.Repositories
{
    public class RoomReservationRepository : Repository<RoomReservation>, IRoomReservationRepository
    {
        public RoomReservationRepository(ReservationDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
        }
    }
}