using Identity.Common.ViewModels;
using Identity.DataLayer.Context;
using Identity.Entities.Identity;
using Identity.Services.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Services.Configuration
{
    public static class AddIdentityOptionsExtensions
    {

        public static void AddIdentityOptions(this IServiceCollection services)
        {

            services.AddIdentity<User, Role>()
                .AddUserStore<ApplicationUserStore>()
              .AddUserManager<ApplicationUserManager>()
              .AddRoleStore<ApplicationRoleStore>()
              .AddRoleManager<ApplicationRoleManager>()
              .AddSignInManager<ApplicationSignInManager>()
              .AddErrorDescriber<CustomIdentityErrorDescriber>()
              //.AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders();

        }


    }

}
