using DNTCommon.Web.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Reservation.Infrastructure.Context;
using Reservation.Infrastructure.SeedData.Contract;

namespace Reservation.Infrastructure.SeedData
{
    public class ReservationSeedData:IReservationSeedData
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public ReservationSeedData(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        public void SeedData()
        {
            _scopeFactory.RunScopedService<ReservationDbContext>(context =>
            {
                context.Database.Migrate();
            });
        }
    }
}
