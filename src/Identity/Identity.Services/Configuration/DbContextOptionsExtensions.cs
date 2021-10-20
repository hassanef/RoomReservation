using DNTCommon.Web.Core;
using Identity.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Services.Configuration
{
    public static class DbContextOptionsExtensions
    {
        /// <summary>
        /// Creates and seeds the database.
        /// </summary>
        public static void InitializeDb(this IServiceProvider serviceProvider)
        {
            serviceProvider.RunScopedService<IIdentityDbInitializer>(identityDbInitialize =>
            {
                identityDbInitialize.Initialize();
                identityDbInitialize.SeedData();
            });
        }
    }

}
