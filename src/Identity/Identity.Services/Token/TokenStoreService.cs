using DNTCommon.Web.Core;
using Identity.Common.GuardToolkit;
using Identity.Common.ViewModels;
using Identity.DataLayer.Context;
using Identity.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Services.Token
{
    public interface ITokenStoreService
    {
        Task AddUserTokenAsync(CustomUserToken userToken);
        Task<CustomUserToken> AddUserTokenAsync(User user, string refreshTokenSerial, string accessToken, string refreshTokenSourceSerial);
        Task<bool> IsValidTokenAsync(string accessToken, int userId);
        Task DeleteExpiredTokensAsync();
        Task<CustomUserToken> FindTokenAsync(string refreshTokenValue);
        Task DeleteTokenAsync(string refreshTokenValue);
        Task<bool> DeleteTokenAsync(int id, string refreshTokenValue);
        Task DeleteTokensWithSameRefreshTokenSourceAsync(string refreshTokenIdHashSource);
        Task InvalidateUserTokensAsync(int userId);
        Task RevokeUserBearerTokensAsync(string userIdValue, string refreshTokenValue);
        Task<List<CustomUserTokenViewModel>> GetUserTokens(int userId);
    }

    public class TokenStoreService : ITokenStoreService
    {
        private readonly ISecurityService _securityService;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<CustomUserToken> _tokens;
        private readonly IOptionsSnapshot<BearerTokensOptions> _configuration;
        private readonly ITokenFactoryService _tokenFactoryService;
        private readonly IHttpContextAccessor _contextAccessor;

        public TokenStoreService(
        IUnitOfWork uow,
        ISecurityService securityService,
        IOptionsSnapshot<BearerTokensOptions> configuration,
        ITokenFactoryService tokenFactoryService,
        IHttpContextAccessor contextAccessor)
        {
            _uow = uow;
            _uow.CheckArgumentIsNull(nameof(_uow));

            _securityService = securityService;
            _securityService.CheckArgumentIsNull(nameof(_securityService));

            _tokens = _uow.Set<CustomUserToken>();

            _configuration = configuration;
            _configuration.CheckArgumentIsNull(nameof(configuration));


            _tokenFactoryService = tokenFactoryService;
            _tokenFactoryService.CheckArgumentIsNull(nameof(tokenFactoryService));

            _contextAccessor = contextAccessor;
            _contextAccessor.CheckArgumentIsNull(nameof(contextAccessor));
        }

        public async Task AddUserTokenAsync(CustomUserToken userToken)
        {
            if (!_configuration.Value.AllowMultipleLoginsFromTheSameUser)
            {
                await InvalidateUserTokensAsync(userToken.UserId);
            }

            await DeleteTokensWithSameRefreshTokenSourceAsync(userToken.RefreshTokenIdHashSource);
            await _tokens.AddAsync(userToken);
        }

        public async Task<CustomUserToken> AddUserTokenAsync(User user, string refreshTokenSerial, string accessToken, string refreshTokenSourceSerial)
        {
            var now = DateTimeOffset.UtcNow;
            var token = new CustomUserToken();

            try
            {
             token = new CustomUserToken
                        {

                            UserId = user.Id,
                            // Refresh token handles should be treated as secrets and should be stored hashed
                            RefreshTokenIdHash = _securityService.GetSha256Hash(refreshTokenSerial),
                            RefreshTokenIdHashSource = string.IsNullOrWhiteSpace(refreshTokenSourceSerial) ?
                                                       null : _securityService.GetSha256Hash(refreshTokenSourceSerial),
                            AccessTokenHash = _securityService.GetSha256Hash(accessToken),
                            RefreshTokenExpiresDateTime = now.AddMinutes(_configuration.Value.RefreshTokenExpirationMinutes),
                            AccessTokenExpiresDateTime = now.AddMinutes(_configuration.Value.AccessTokenExpirationMinutes)
                        };
            }
            catch (Exception)
            {

                throw;
            }
            
            await AddUserTokenAsync(token);

            return token;
        }

        public async Task DeleteExpiredTokensAsync()
        {
            var now = DateTimeOffset.UtcNow;
            await _tokens.Where(x => x.RefreshTokenExpiresDateTime < now)
                         .ForEachAsync(userToken => 
                         {
                             _tokens.Remove(userToken);
                         });

            var userIds = await _tokens.Where(x => x.RefreshTokenExpiresDateTime < now).Select(x => x.UserId).Distinct().ToListAsync();
      
        }

        public async Task DeleteTokenAsync(string refreshTokenValue)
        {
            var token = await FindTokenAsync(refreshTokenValue);
            if (token != null)
            {
                _tokens.Remove(token);
            }
        }
        public async Task<bool> DeleteTokenAsync(int id, string refreshTokenValue)
        {
            var userId = _contextAccessor.HttpContext.User.GetUserId();
            var currentToken = await FindTokenAsync(refreshTokenValue);

            if (currentToken == null)
                throw new Exception("refresh token is not valid!");

            if (currentToken.Id == id)
                throw new Exception("can not remove current token");

            var token = await _tokens.Where(x => x.Id == id && x.UserId == userId).SingleOrDefaultAsync();

            if (token != null)
            {
                _tokens.Remove(token);
                await _uow.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task DeleteTokensWithSameRefreshTokenSourceAsync(string refreshTokenIdHashSource)
        {
            if (string.IsNullOrWhiteSpace(refreshTokenIdHashSource))
            {
                return;
            }
            await _tokens.Where(t => t.RefreshTokenIdHashSource == refreshTokenIdHashSource)
                         .ForEachAsync(userToken =>
                         {
                             _tokens.Remove(userToken);
                            // _cacheProvider.ClearCache(tokenRedisKey + userToken.Id);
                         });

        }

        public async Task RevokeUserBearerTokensAsync(string userIdValue, string refreshTokenValue)
        {
            if (!string.IsNullOrWhiteSpace(userIdValue) && int.TryParse(userIdValue, out int userId))
            {
                if (_configuration.Value.AllowSignoutAllUserActiveClients)
                {
                    await InvalidateUserTokensAsync(userId);
                }
            }

            if (!string.IsNullOrWhiteSpace(refreshTokenValue))
            {
                var refreshTokenSerial = _tokenFactoryService.GetRefreshTokenSerial(refreshTokenValue);
                if (!string.IsNullOrWhiteSpace(refreshTokenSerial))
                {
                    var refreshTokenIdHashSource = _securityService.GetSha256Hash(refreshTokenSerial);
                    await DeleteTokensWithSameRefreshTokenSourceAsync(refreshTokenIdHashSource);
                }
            }

            await DeleteExpiredTokensAsync();
        }

        public Task<CustomUserToken> FindTokenAsync(string refreshTokenValue)
        {
            if (string.IsNullOrWhiteSpace(refreshTokenValue))
            {
                return null;
            }

            var refreshTokenSerial = _tokenFactoryService.GetRefreshTokenSerial(refreshTokenValue);
            if (string.IsNullOrWhiteSpace(refreshTokenSerial))
            {
                return null;
            }

            var refreshTokenIdHash = _securityService.GetSha256Hash(refreshTokenSerial);
            return _tokens.Include(x => x.User).FirstOrDefaultAsync(x => x.RefreshTokenIdHash == refreshTokenIdHash);
        }

        public async Task InvalidateUserTokensAsync(int userId)
        {
            await _tokens.Where(x => x.UserId == userId)
                         .ForEachAsync(userToken =>
                         {
                             _tokens.Remove(userToken);
                         });

        }

        public async Task<bool> IsValidTokenAsync(string accessToken, int userId)
        {
            var accessTokenHash = _securityService.GetSha256Hash(accessToken);
            var userToken = await _tokens.FirstOrDefaultAsync(
                x => x.AccessTokenHash == accessTokenHash && x.UserId == userId);
            return userToken?.AccessTokenExpiresDateTime >= DateTimeOffset.UtcNow;
        }

        public async Task<List<CustomUserTokenViewModel>> GetUserTokens(int userId)
        {
            return await _tokens.Include(x => x.User)
                                .Where(x => x.UserId == userId)
                                .Select(x => new CustomUserTokenViewModel() 
                                {
                                    AccessTokenExpiresDateTime = x.AccessTokenExpiresDateTime,
                                    AccessTokenHash = x.AccessTokenHash,
                                    Id = x.Id,
                                    IsActiveUser = x.User.IsActive,
                                    RefreshTokenExpiresDateTime = x.RefreshTokenExpiresDateTime,
                                    RefreshTokenIdHash = x.RefreshTokenIdHash,
                                    RefreshTokenIdHashSource = x.RefreshTokenIdHashSource,
                                    UserId = x.UserId
                                }).ToListAsync();
        }

    }
}