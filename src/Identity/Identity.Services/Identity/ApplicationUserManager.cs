using DNTCommon.Web.Core;
using DNTPersianUtils.Core;
using GuardNet;
using Identity.DataLayer.Context;
using Identity.Entities.Identity;
using Identity.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Identity.Services.Identity
{
    public class ApplicationUserManager :
		UserManager<User>,
		IApplicationUserManager
	{
		private readonly IHttpContextAccessor _contextAccessor;
		private readonly IUnitOfWork _uow;
		private readonly IUsedPasswordsService _usedPasswordsService;
		private readonly IdentityErrorDescriber _errors;
		private readonly ILookupNormalizer _keyNormalizer;
		private readonly ILogger<ApplicationUserManager> _logger;
		private readonly IOptions<IdentityOptions> _optionsAccessor;
		private readonly IPasswordHasher<User> _passwordHasher;
		private readonly IEnumerable<IPasswordValidator<User>> _passwordValidators;
		private readonly IServiceProvider _services;
		private readonly DbSet<User> _users;

		private readonly IApplicationUserStore _userStore;
		private readonly IEnumerable<IUserValidator<User>> _userValidators;
		private User _currentUserInScope;

		public ApplicationUserManager(
			IApplicationUserStore store,
			IOptions<IdentityOptions> optionsAccessor,
			IPasswordHasher<User> passwordHasher,
			IEnumerable<IUserValidator<User>> userValidators,
			IEnumerable<IPasswordValidator<User>> passwordValidators,
			ILookupNormalizer keyNormalizer,
			IdentityErrorDescriber errors,
			IServiceProvider services,
			ILogger<ApplicationUserManager> logger,
			IHttpContextAccessor contextAccessor,
			IUnitOfWork uow,
			IUsedPasswordsService usedPasswordsService)
			: base(
				(UserStore<User, Role, ApplicationDbContext, int, UserClaim, UserRole, UserLogin, UserToken, RoleClaim>)store,
				  optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
		{
			_userStore = store ?? throw new ArgumentNullException(nameof(_userStore));
			_optionsAccessor = optionsAccessor ?? throw new ArgumentNullException(nameof(_optionsAccessor));
			_passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(_passwordHasher));
			_userValidators = userValidators ?? throw new ArgumentNullException(nameof(_userValidators));
			_passwordValidators = passwordValidators ?? throw new ArgumentNullException(nameof(_passwordValidators));
			_keyNormalizer = keyNormalizer ?? throw new ArgumentNullException(nameof(_keyNormalizer));
			_errors = errors ?? throw new ArgumentNullException(nameof(_errors));
			_services = services ?? throw new ArgumentNullException(nameof(_services));
			_logger = logger ?? throw new ArgumentNullException(nameof(_logger));
			_contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(_contextAccessor));
			_uow = uow ?? throw new ArgumentNullException(nameof(_uow));
			_usedPasswordsService = usedPasswordsService ?? throw new ArgumentNullException(nameof(_usedPasswordsService));
			_users = uow.Set<User>();
		}

		#region BaseClass

		string IApplicationUserManager.CreateTwoFactorRecoveryCode()
		{
			return base.CreateTwoFactorRecoveryCode();
		}

		Task<PasswordVerificationResult> IApplicationUserManager.VerifyPasswordAsync(IUserPasswordStore<User> store, User user, string password)
		{
			return base.VerifyPasswordAsync(store, user, password);
		}

		public override async Task<IdentityResult> CreateAsync(User user)
		{
			var result = await base.CreateAsync(user);
			if (result.Succeeded)
			{
				await _usedPasswordsService.AddToUsedPasswordsListAsync(user);
			}
			return result;
		}

		public override async Task<IdentityResult> CreateAsync(User user, string password)
		{
			var result = await base.CreateAsync(user, password);
			if (result.Succeeded)
			{
				await _usedPasswordsService.AddToUsedPasswordsListAsync(user);
			}
			return result;
		}
		public override async Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword)
		{
			var result = await base.ChangePasswordAsync(user, currentPassword, newPassword);
			if (result.Succeeded)
			{
				await _usedPasswordsService.AddToUsedPasswordsListAsync(user);
			}
			return result;
		}

		public override async Task<IdentityResult> ResetPasswordAsync(User user, string token, string newPassword)
		{
			var result = await base.ResetPasswordAsync(user, token, newPassword);
			if (result.Succeeded)
			{
				await _usedPasswordsService.AddToUsedPasswordsListAsync(user);
			}
			return result;
		}

		#endregion

		#region Select

		public User FindById(int userId)
		{
			return _users.Find(userId);
		}

		public Task<User> FindByIdIncludeUserRolesAsync(int userId)
		{
			return _users.Include(x => x.Roles).FirstOrDefaultAsync(x => x.Id == userId);
		}

		public async Task<List<User>> GetAllUsersAsync()
		{
			return await Users.ToListAsync();
		}
		public User GetCurrentUser()
		{
			if (_currentUserInScope != null)
			{
				return _currentUserInScope;
			}

			var currentUserId = GetCurrentUserId();
			if (string.IsNullOrWhiteSpace(currentUserId))
			{
				return null;
			}

			var userId = int.Parse(currentUserId);
			return _currentUserInScope = FindById(userId);
		}

		public async Task<User> GetCurrentUserAsync()
		{
			return _currentUserInScope ??
				(_currentUserInScope = await GetUserAsync(_contextAccessor.HttpContext.User));
		}

		public string GetCurrentUserId()
		{
			return _contextAccessor.HttpContext.User.Identity.GetUserId();
		}

		public int? CurrentUserId
		{
			get
			{
				var userId = _contextAccessor.HttpContext.User.Identity.GetUserId();
				if (string.IsNullOrEmpty(userId))
				{
					return null;
				}

				return !int.TryParse(userId, out int result) ? (int?)null : result;
			}
		}

		IPasswordHasher<User> IApplicationUserManager.PasswordHasher { get => base.PasswordHasher; set => base.PasswordHasher = value; }

		IList<IUserValidator<User>> IApplicationUserManager.UserValidators => base.UserValidators;

		IList<IPasswordValidator<User>> IApplicationUserManager.PasswordValidators => base.PasswordValidators;

		IQueryable<User> IApplicationUserManager.Users => base.Users;

		public string GetCurrentUserName()
		{
			return _contextAccessor.HttpContext.User.Identity.GetUserName();
		}

		public async Task<bool> HasPasswordAsync(int userId)
		{
			var user = await FindByIdAsync(userId.ToString());
			return user?.PasswordHash != null;
		}

		public async Task<bool> HasPhoneNumberAsync(int userId)
		{
			var user = await FindByIdAsync(userId.ToString());
			return user?.PhoneNumber != null;
		}

		public async Task<byte[]> GetEmailImageAsync(int? userId)
		{
			if (userId == null)
				return "?".TextToImage(new TextToImageOptions());

			var user = await FindByIdAsync(userId.Value.ToString());
			if (user == null)
				return "?".TextToImage(new TextToImageOptions());

			if (!user.IsEmailPublic)
				return "?".TextToImage(new TextToImageOptions());

			return user.Email.TextToImage(new TextToImageOptions());
		}


		public async Task<string> GetSerialNumberAsync(int userId)
		{
			var user = FindById(userId);
			return user.SerialNumber;
		}


		public async Task<bool> HasUserAsync(Expression<Func<User, bool>> where)
		{
			Guard.NotNull(where, nameof(where));

			return await _users.AsQueryable().AsNoTracking().AnyAsync(where);
		}
		public async Task<int> GetUserCountAsync(Expression<Func<User, bool>> where)
		{
			Guard.NotNull(where, nameof(where));

			return await _users.AsQueryable().AsNoTracking().CountAsync(where);
		}

		#endregion

		#region CRUD

	
		public async Task<IdentityResult> UpdateUserAndSecurityStampAsync(int userId, Action<User> action)
		{
			var user = await FindByIdIncludeUserRolesAsync(userId);
			if (user == null)
			{
				return IdentityResult.Failed(new IdentityError
				{
					Code = "UserNotFound",
					Description = "کاربر مورد نظر یافت نشد."
				});
			}

			action(user);

			var result = await UpdateAsync(user);
			if (!result.Succeeded)
			{
				return result;
			}
			return await UpdateSecurityStampAsync(user);
		}

		public async Task<IdentityResult> AddOrUpdateUserRolesAsync(int userId, IList<int> selectedRoleIds, Action<User> action = null)
		{
			var user = await FindByIdIncludeUserRolesAsync(userId);
			if (user == null)
			{
				return IdentityResult.Failed(new IdentityError
				{
					Code = "UserNotFound",
					Description = "کاربر مورد نظر یافت نشد."
				});
			}

			var currentUserRoleIds = user.Roles.Select(x => x.RoleId).ToList();

			if (selectedRoleIds == null)
			{
				selectedRoleIds = new List<int>();
			}

			var newRolesToAdd = selectedRoleIds.Except(currentUserRoleIds).ToList();
			foreach (var roleId in newRolesToAdd)
			{
				user.Roles.Add(new UserRole { RoleId = roleId, UserId = user.Id });
			}

			var removedRoles = currentUserRoleIds.Except(selectedRoleIds).ToList();
			foreach (var roleId in removedRoles)
			{
				var userRole = user.Roles.SingleOrDefault(ur => ur.RoleId == roleId);
				if (userRole != null)
				{
					user.Roles.Remove(userRole);
				}
			}

			action?.Invoke(user);

			var result = await UpdateAsync(user);
			if (!result.Succeeded)
			{
				return result;
			}
			return await UpdateSecurityStampAsync(user);
		}

		Task<IdentityResult> IApplicationUserManager.UpdatePasswordHash(User user, string newPassword, bool validatePassword)
		{
			return base.UpdatePasswordHash(user, newPassword, validatePassword);
		}

		public async Task UpdateUserLastActivityDateAsync(int userId)
		{
			var user = FindById(userId);
			if (user.LastLoggedIn != null)
			{
				var updateLastActivityDate = TimeSpan.FromMinutes(2);
				var currentUtc = DateTimeOffset.UtcNow;
				var timeElapsed = currentUtc.Subtract(user.LastLoggedIn.Value);
				if (timeElapsed < updateLastActivityDate)
				{
					return;
				}
			}
			user.LastLoggedIn = DateTimeOffset.UtcNow;
			await _uow.SaveChangesAsync();
		}


		public async Task<IdentityResult> RemoveUserRolesAsync(int userId, IList<int> removeRoleIds)
		{
			var user = await FindByIdIncludeUserRolesAsync(userId);
			if (user == null)
			{
				return IdentityResult.Failed(new IdentityError
				{
					Code = "UserNotFound",
					Description = "کاربر مورد نظر یافت نشد."
				});
			}

			foreach (var roleId in removeRoleIds)
			{
				var userRole = user.Roles.SingleOrDefault(ur => ur.RoleId == roleId);
				if (userRole != null)
				{
					user.Roles.Remove(userRole);
				}
			}


			var result = await UpdateAsync(user);
			if (!result.Succeeded)
			{
				return result;
			}
			return await UpdateSecurityStampAsync(user);
		}

		public async Task<IdentityResult> AddUsersRolesAsync(IList<int> userIds, IList<int> selectedRoleIds)
		{

			foreach (var userId in userIds)
			{
				var user = await FindByIdIncludeUserRolesAsync(userId);
				if (user == null)
				{
					//return IdentityResult.Failed(new IdentityError
					//{
					//    Code = "UserNotFound",
					//    Description = "کاربر مورد نظر یافت نشد."
					//});
					continue;
				}

				var currentUserRoleIds = user.Roles.Select(x => x.RoleId).ToList();

				if (selectedRoleIds == null)
				{
					selectedRoleIds = new List<int>();
				}

				var newRolesToAdd = selectedRoleIds.Except(currentUserRoleIds).ToList();
				foreach (var roleId in newRolesToAdd)
				{
					user.Roles.Add(new UserRole { RoleId = roleId, UserId = user.Id });
				}

				_users.UpdateRange(user);
			}

			var result = await _uow.SaveChangesAsync();


			if (result <= 0)
			{
				return IdentityResult.Failed(new IdentityError
				{
					Code = "Error",
					Description = "خطا در ثبت دیتا"
				});
			}

			return IdentityResult.Success;
		}


		#endregion
	}
}