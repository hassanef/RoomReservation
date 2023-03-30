using Microsoft.AspNetCore.Http;
using Reservation.Domain.IRepositories;
using Reservation.Infrastructure.Context;

namespace Reservation.Infrastructure.Repositories
{
    public class RoomReservationRepository : Repository<Domain.AggregatesModel.RoomReservation>, IRoomReservationRepository
    {
        public RoomReservationRepository(ReservationDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
        }
    }
}