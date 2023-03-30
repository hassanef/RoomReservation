using Identity.DataLayer.Context;
using Identity.Entities.Identity;
using Identity.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Security.Claims;

namespace Identity.Services.Identity
{
    public class ApplicationRoleStore :
        RoleStore<Role, ApplicationDbContext, int, UserRole, RoleClaim>,
        IApplicationRoleStore
    {
        private readonly IUnitOfWork _uow;
        private readonly IdentityErrorDescriber _describer;

        public ApplicationRoleStore(
            IUnitOfWork uow,
            IdentityErrorDescriber describer)
            : base((ApplicationDbContext)uow, describer)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
            _describer = describer ?? throw new ArgumentNullException(nameof(_describer));
        }

        protected override RoleClaim CreateRoleClaim(Role role, Claim claim)
        {
            return new RoleClaim
            {
                RoleId = role.Id,
                ClaimType = claim.Type,
                ClaimValue = claim.Value
            };
        }

    }
}