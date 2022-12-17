using Microsoft.AspNetCore.Http;
using Reservation.Domain.AggregatesModel.OfficeAggregate;
using Reservation.Domain.IRepositories;
using Reservation.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Infrastructure.Repositories
{
    public class OfficeRepository : Repository<Room>, IOfficeRepository
    {
        public OfficeRepository(ReservationDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
        }
    }
}
