using DNTCommon.Web.Core;
using Reservation.Infrastructure.SeedData.Contract;
using System;

namespace Reservation.Infrastructure.Context
{
    public static class DbContextOptionsExtensions
    {
        /// <summary>
        /// Create and seed the database.
        /// </summary>
        public static void InitializeDb(this IServiceProvider serviceProvider)
        {
            serviceProvider.RunScopedService<IReservationSeedData>(dbInitialize =>
            {
                dbInitialize.SeedData();
            });
        }
    }

}
