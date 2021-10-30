using Microsoft.AspNetCore.Http;
using Reservation.Domain.AggregatesModel;
using Reservation.Domain.IRepositories;
using Reservation.Infrastructure.Context;

namespace Reservation.Infrastructure.Repositories
{
    public class LocationRepository : Repository<Location>, ILocationRepository
    {
        public LocationRepository(ReservationDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
        }
    }
}