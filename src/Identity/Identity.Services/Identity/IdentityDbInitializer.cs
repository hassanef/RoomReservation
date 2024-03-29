﻿using DNTCommon.Web.Core;
using Identity.Common.GuardToolkit;
using Identity.Common.IdentityToolkit;
using Identity.Common.ViewModels;
using Identity.DataLayer.Context;
using Identity.Entities.Identity;
using Identity.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Services.Identity
{

    public class IdentityDbInitializer : IIdentityDbInitializer
    {
        private readonly IOptionsSnapshot<SiteSettings> _adminUserSeedOptions;
        private readonly IApplicationUserManager _applicationUserManager;
        private readonly ILogger<IdentityDbInitializer> _logger;
        private readonly IApplicationRoleManager _roleManager;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUnitOfWork _uow;


        public IdentityDbInitializer(
            IApplicationUserManager applicationUserManager,
            IServiceScopeFactory scopeFactory,
            IApplicationRoleManager roleManager,
            IOptionsSnapshot<SiteSettings> adminUserSeedOptions,
            ILogger<IdentityDbInitializer> logger,
            IHttpContextAccessor contextAccessor,
            IUnitOfWork uow
            )
        {
            _applicationUserManager = applicationUserManager;
            _applicationUserManager.CheckArgumentIsNull(nameof(_applicationUserManager));

            _scopeFactory = scopeFactory;
            _scopeFactory.CheckArgumentIsNull(nameof(_scopeFactory));

            _roleManager = roleManager;
            _roleManager.CheckArgumentIsNull(nameof(_roleManager));

            _adminUserSeedOptions = adminUserSeedOptions;
            _adminUserSeedOptions.CheckArgumentIsNull(nameof(_adminUserSeedOptions));

            _logger = logger;
            _logger.CheckArgumentIsNull(nameof(_logger));

            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));

        }

        /// <summary>
        /// Applies any pending migrations for the context to the database.
        /// Will create the database if it does not already exist.
        /// </summary>
        public void Initialize()
        {
            _scopeFactory.RunScopedService<ApplicationDbContext>(context =>
                {
                    if (_adminUserSeedOptions.Value.ActiveDatabase == ActiveDatabase.InMemoryDatabase)
                    {
                        context.Database.EnsureCreated();
                    }
                    else
                    {
                        context.Database.Migrate();
                    }
                });
        }

        /// <summary>
        /// Adds some default values to the IdentityDb
        /// </summary>
        public void SeedData()
        {
            _scopeFactory.RunScopedService<IIdentityDbInitializer>(identityDbSeedData =>
            {
                var result = identityDbSeedData.SeedDatabaseWithAdminUserAsync().Result;
                if (result == IdentityResult.Failed())
                {
                    throw new InvalidOperationException(result.DumpErrors());
                }
            });

        }

        public async Task<IdentityResult> SeedDatabaseWithAdminUserAsync()
        {
            var adminUserSeed = _adminUserSeedOptions.Value.AdminUserSeed;
            adminUserSeed.CheckArgumentIsNull(nameof(adminUserSeed));

            var name = adminUserSeed.Username;
            var password = adminUserSeed.Password;
            var email = adminUserSeed.Email;
            var roleName = adminUserSeed.RoleName;
            

            var thisMethodName = nameof(SeedDatabaseWithAdminUserAsync);

            //Create the `User` Role and Claims if it does not exist
            var userRole = await _roleManager.GetRoleByName(ConstantRoles.User);
            if (userRole == null)
            {
                userRole = new Role(ConstantRoles.User);

                var userRoleResult = await _roleManager.CreateRole(userRole);
                if (userRoleResult == IdentityResult.Failed())
                {
                    _logger.LogError($"{thisMethodName}: userRole CreateAsync failed. {userRoleResult.DumpErrors()}");
                    return IdentityResult.Failed();
                }
                else
                {
                    var claimRoomReservation = new RoleClaim() { RoleId = userRole.Id, ClaimType = ConstantPolicies.DynamicPermission, ClaimValue = "RoomReservation:CreateRoomReservation" };
                    var claimCreateResourceReservation = new RoleClaim() { RoleId = userRole.Id, ClaimType = ConstantPolicies.DynamicPermission, ClaimValue = "ResourceReservation:CreateResourceReservation" };
                    var claimGetRooms = new RoleClaim() { RoleId = userRole.Id, ClaimType = ConstantPolicies.DynamicPermission, ClaimValue = "Room:GetRooms" };

                    userRole.Claims = new List<RoleClaim>();
                    userRole.Claims.Add(claimRoomReservation);
                    userRole.Claims.Add(claimCreateResourceReservation);
                    userRole.Claims.Add(claimGetRooms);

                    var roleClaimResult = await _roleManager.UpdateAsync(userRole);
                    if (roleClaimResult == IdentityResult.Failed())
                    {
                        _logger.LogError($"{thisMethodName}: roleClaim UpdateAsync failed. {userRoleResult.DumpErrors()}");
                        return IdentityResult.Failed();
                    }
                }
            }
            else
            {
                _logger.LogInformation($"{thisMethodName}: userRole already exists.");
            }

            var adminUser = await _applicationUserManager.FindByNameAsync(name);
            if (adminUser != null)
            {
                _logger.LogInformation($"{thisMethodName}: adminUser already exists.");
                return IdentityResult.Success;
            }

            //Create the `Admin` Role if it does not exist
            var adminRole = await _roleManager.GetRoleByName(roleName);
            if (adminRole == null)
            {
                adminRole = new Role(roleName);

                var adminRoleResult = await _roleManager.CreateRole(adminRole);
                if (adminRoleResult == IdentityResult.Failed())
                {
                    _logger.LogError($"{thisMethodName}: adminRole CreateAsync failed. {adminRoleResult.DumpErrors()}");
                    return IdentityResult.Failed();
                }
            }
            else
            {
                _logger.LogInformation($"{thisMethodName}: adminRole already exists.");
            }

         

            adminUser = new User
            {
                FirstName = "Admin",
                LastName = "Admin",
                UserName = name,
                Email = email,
                EmailConfirmed = true,
                IsEmailPublic = true,
                LockoutEnabled = true
            };
            var adminUserResult = await _applicationUserManager.CreateAsync(adminUser, password);
            if (adminUserResult == IdentityResult.Failed())
            {
                _logger.LogError($"{thisMethodName}: adminUser CreateAsync failed. {adminUserResult.DumpErrors()}");
                return IdentityResult.Failed();
            }

            var setLockoutResult = await _applicationUserManager.SetLockoutEnabledAsync(adminUser, enabled: false);
            if (setLockoutResult == IdentityResult.Failed())
            {
                _logger.LogError($"{thisMethodName}: adminUser SetLockoutEnabledAsync failed. {setLockoutResult.DumpErrors()}");
                return IdentityResult.Failed();
            }

            var addToRoleResult = await _applicationUserManager.AddUsersRolesAsync(adminUser.Id , adminRole.Id);
            if (addToRoleResult == IdentityResult.Failed())
            {
                _logger.LogError($"{thisMethodName}: adminUser AddToRoleAsync failed. {addToRoleResult.DumpErrors()}");
                return IdentityResult.Failed();
            }

            return IdentityResult.Success;
        }
   
    }
}