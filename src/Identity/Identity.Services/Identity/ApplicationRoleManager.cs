using DNTCommon.Web.Core;
using Identity.Common.IdentityToolkit;
using Identity.DataLayer.Context;
using Identity.Entities.Identity;
using Identity.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Identity.Services.Identity
{
    public class ApplicationRoleManager :
        RoleManager<Role>,
        IApplicationRoleManager
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Role> _roles;


        public ApplicationRoleManager(
            IApplicationRoleStore store,
            IEnumerable<IRoleValidator<Role>> roleValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            ILogger<ApplicationRoleManager> logger,
            IHttpContextAccessor contextAccessor,
            IUnitOfWork uow) :
            base((RoleStore<Role, ApplicationDbContext, int, UserRole, RoleClaim>)store, roleValidators, keyNormalizer, errors, logger)
        {
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _roles = _uow.Set<Role>();
        }


        public IList<Role> FindUserRoles(int userId)
        {
            var userRolesQuery = from role in Roles
                                 from user in role.Users
                                 where user.UserId == userId
                                 select role;

            return userRolesQuery.OrderBy(x => x.Name).ToList();
        }

        public async Task<IdentityResult> CreateRole(Role role)
        {
            if (role != null)
            {
                await _roles.AddAsync(role);
                await _uow.SaveChangesAsync();

                return IdentityResult.Success;
            }

            return IdentityResult.Failed(new IdentityError
            {
                Code = "RoleIsNull",
                Description = "The information is incorrect."
            });
        }
        public async Task<Role> GetRoleByName(string name)
        {

            if (!string.IsNullOrWhiteSpace(name))
                return await _roles.Where(x => x.Name.ToLower().Trim().Contains(name.ToLower().Trim())).FirstOrDefaultAsync();
            return null;
        }
        
        public async Task<IList<Role>> GetRolesForUsers(List<int> userIds)
        {
            var userRolesQuery = from role in _roles
                                 from user in role.Users
                                 where userIds.Contains(user.UserId)
                                 select role;

            return await userRolesQuery.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<bool> Authorize(string currentClaim)
        {
            var roleId = _contextAccessor.GetRoleId();

            var result = await _roles.Include(x => x.Claims)
                                     .AnyAsync(x => x.Id == roleId && x.Claims.Any(c => c.ClaimValue == currentClaim));

            return result;
        }

    }
}
