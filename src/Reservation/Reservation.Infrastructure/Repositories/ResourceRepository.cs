using Microsoft.AspNetCore.Http;
using Reservation.Domain.AggregatesModel;
using Reservation.Domain.IRepositories;
using Reservation.Infrastructure.Context;

namespace Reservation.Infrastructure.Repositories
{
    public class ResourceRepository : Repository<Resource>, IResourceRepository
    {
        public ResourceRepository(ReservationDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
        }
    }
}