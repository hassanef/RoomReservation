using DNTCommon.Web.Core;
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
using System.Threading.Tasks;

namespace Identity.Services.Identity
{
    /// <summary>
    /// More info: http://www.dotnettips.info/post/2578
    /// </summary>
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



        #region Select

        public IList<Role> FindCurrentUserRoles()
        {
            var userId = getCurrentUserId();
            return FindUserRoles(userId);
        }
        public IList<Role> FindCurrentUserRolesWithoutTenantFilter()
        {
            var userId = getCurrentUserId();
            return FindUserRolesWithoutTenant(userId);
        }

        public IList<Role> FindUserRoles(int userId)
        {
            var userRolesQuery = from role in Roles
                                 from user in role.Users
                                 where user.UserId == userId
                                 select role;

            return userRolesQuery.OrderBy(x => x.Name).ToList();
        }
        public IList<Role> FindUserRolesWithoutTenant(int userId)
        {
            var userRolesQuery = from role in _roles
                                 from user in role.Users
                                 where user.UserId == userId
                                 select role;

            return userRolesQuery.OrderBy(x => x.Name).ToList();
        }

        public async Task<List<Role>> GetAllCustomRolesAsync(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
                return await _roles.Where(x => x.Name.Contains(name)).ToListAsync();
            return await _roles.ToListAsync();
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

        public IList<Role> GetRolesForCurrentUser()
        {
            var userId = getCurrentUserId();
            return GetRolesForUser(userId);
        }
        
        public IList<Role> GetRolesForUser(int userId)
        {
            var roles = FindUserRoles(userId);
            if (roles == null || !roles.Any())
            {
                return new List<Role>();
            }

            return roles.ToList();
        }
        public async Task<IList<Role>> GetRolesForUser(int userId, Guid applicationId)
        {
            var userRolesQuery = from role in _roles
                                 from user in role.Users
                                 where user.UserId == userId && role.ApplicationId == applicationId
                                 select role;

            return await userRolesQuery.OrderBy(x => x.Name).ToListAsync();
        }
        public async Task<IList<Role>> GetRolesForUsers(List<int> userIds)
        {
            var userRolesQuery = from role in _roles
                                 from user in role.Users
                                 where userIds.Contains(user.UserId)
                                 select role;

            return await userRolesQuery.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<ILookup<int, Role>> GetRolesForUsers(IEnumerable<int> userIds)
        {
            var roles = await _roles.Include(x => x.Users).Where(x => x.Users.Any(u => userIds.Contains(u.UserId))).ToListAsync();

            return roles.ToLookup(x => x.Users.First().UserId);
        }


        public IList<UserRole> GetUserRolesInRole(string roleName)
        {
            return Roles.Where(role => role.Name == roleName)
                             .SelectMany(role => role.Users)
                             .ToList();
        }

        public bool IsCurrentUserInRole(string roleName)
        {
            var userId = getCurrentUserId();
            return IsUserInRole(userId, roleName);
        }

        public bool IsUserInRole(int userId, string roleName)
        {
            var userRolesQuery = from role in Roles
                                 where role.Name == roleName
                                 from user in role.Users
                                 where user.UserId == userId
                                 select role;
            var userRole = userRolesQuery.FirstOrDefault();
            return userRole != null;
        }

   
        public async Task<Role> GetRoleById(int roleId)
        {
            return await Roles.Where(role => role.Id == roleId).SingleOrDefaultAsync();
        }

        public async Task<int> GetRoleCountAsync(Expression<Func<Role, bool>> where)
        {
            //Guard.ArgumentNotNull(where, nameof(where));

            return await _roles.AsQueryable().AsNoTracking().CountAsync(where);
        }
        public async Task<int> GetRoleCount(Expression<Func<Role, bool>> where)
        {
            //Guard.ArgumentNotNull(where, nameof(where));

            return await _roles.AsQueryable().AsNoTracking().CountAsync(where);
        }

        private int getCurrentUserId() => _contextAccessor.HttpContext.User.Identity.GetUserId<int>();


        #endregion

        #region CRUD

        public async Task<IdentityResult> RemoveRoles(int[] roleIds)
        {
            try
            {
                var roles = await _roles.Where(x => roleIds.Contains(x.Id)).ToListAsync();

                if (roles.Any())
                {
                    _roles.RemoveRange(roles);
                    await _uow.SaveChangesAsync();
                }
                else
                {
                    return IdentityResult.Failed(new IdentityError() { Code = "RemoveRoles", Description = "Role not exist" });

                }
            }
            catch (Exception)
            {
                return IdentityResult.Failed(new IdentityError() { Code = "RemoveRoles", Description = "Error deleting role" });
            }

            return IdentityResult.Success;
        }

        
        #endregion

    }
}
