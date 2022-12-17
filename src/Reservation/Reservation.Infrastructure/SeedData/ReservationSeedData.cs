using DNTCommon.Web.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Reservation.Infrastructure.Context;
using Reservation.Infrastructure.SeedData.Contract;
using System.Linq;

namespace Reservation.Infrastructure.SeedData
{
    public class ReservationSeedData : IReservationSeedData
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<ReservationSeedData> _logger;

        public ReservationSeedData(IServiceScopeFactory scopeFactory, ILogger<ReservationSeedData> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }
        public void SeedData()
        {
            _scopeFactory.RunScopedService<ReservationDbContext>(context =>
            {
                context.Database.Migrate();
            });

            _scopeFactory.RunScopedService<ReservationDbContext>(context =>
            {
                if (!context.Location.Any())
                {
                    context.Location.AddRange(ReservationDefaultData.CreateLocationsPredefine());
                    context.SaveChanges();

                    _logger.LogInformation("Create rooms predefine in seed data.");
                }
                if (!context.Resources.Any())
                {
                    context.Resources.AddRange(ReservationDefaultData.CreateResourcesPredefine());
                    context.SaveChanges();

                    _logger.LogInformation("Create resources predefine in seed data.");
                }
                if (!context.Offices.Any())
                {
                    context.Offices.AddRange(ReservationDefaultData.CreateOfficesRoomsPredefine());
                    context.SaveChanges();

                    _logger.LogInformation("Create rooms predefine in seed data.");
                }
            });
        }
      
    }
}
