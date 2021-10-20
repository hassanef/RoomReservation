using Identity.Common.GuardToolkit;
using Identity.Entities.Identity;
using Identity.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Services.Token

{
    public class JwtTokensData
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string RefreshTokenSerial { get; set; }
        public IEnumerable<Claim> Claims { get; set; }
    }

    public interface ITokenFactoryService
    {
        Task<JwtTokensData> CreateJwtTokensAsync(User user);
        string GetRefreshTokenSerial(string refreshTokenValue);
    }

    public class TokenFactoryService : ITokenFactoryService
    {
        private readonly ISecurityService _securityService;
        private readonly IOptionsSnapshot<BearerTokensOptions> _configuration;
        private readonly IApplicationRoleManager _rolesService;
        private readonly ILogger<TokenFactoryService> _logger;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenFactoryService(
            ISecurityService securityService,
            IApplicationRoleManager rolesService,
            IOptionsSnapshot<BearerTokensOptions> configuration,
            ILogger<TokenFactoryService> logger,
            IConfiguration config,
            IHttpContextAccessor httpContextAccessor)
        {
            _securityService = securityService;
            _securityService.CheckArgumentIsNull(nameof(_securityService));

            _rolesService = rolesService;
            _rolesService.CheckArgumentIsNull(nameof(rolesService));

            _configuration = configuration;
            _configuration.CheckArgumentIsNull(nameof(configuration));

            _logger = logger;
            _logger.CheckArgumentIsNull(nameof(logger));

            _httpContextAccessor = httpContextAccessor;
            _httpContextAccessor.CheckArgumentIsNull(nameof(httpContextAccessor));


            //TODO
            _config = config;
        }


        public async Task<JwtTokensData> CreateJwtTokensAsync(User user)
        {
            var (accessToken, claims) = await createAccessTokenAsync(user);
            var (refreshTokenValue, refreshTokenSerial) = createRefreshToken();
            return new JwtTokensData
            {
                AccessToken = accessToken,
                RefreshToken = refreshTokenValue,
                RefreshTokenSerial = refreshTokenSerial,
                Claims = claims
            };
        }

        private (string RefreshTokenValue, string RefreshTokenSerial) createRefreshToken()
        {
            var refreshTokenSerial = _securityService.CreateCryptographicallySecureGuid().ToString().Replace("-", "");

            var claims = new List<Claim>
            {
                // Unique Id for all Jwt tokes
                new Claim(JwtRegisteredClaimNames.Jti, _securityService.CreateCryptographicallySecureGuid().ToString(), ClaimValueTypes.String, _config["BearerTokens:Issuer"]),
                // Issuer
                new Claim(JwtRegisteredClaimNames.Iss, _config["BearerTokens:Issuer"], ClaimValueTypes.String, _config["BearerTokens:Issuer"]),
                // Issued at
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64, _config["BearerTokens:Issuer"]),
                // for invalidation
                new Claim(ClaimTypes.SerialNumber, refreshTokenSerial, ClaimValueTypes.String, _config["BearerTokens:Issuer"])
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["BearerTokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var now = DateTime.UtcNow;
            var token = new JwtSecurityToken(
                issuer: _config["BearerTokens:Issuer"],
                audience: _config["BearerTokens:Audience"],
                claims: claims,
                notBefore: now,
                expires: now.AddMinutes(double.Parse(_config["BearerTokens:RefreshTokenExpirationMinutes"])),
                signingCredentials: creds);
            var refreshTokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            return (refreshTokenValue, refreshTokenSerial);
        }

        public string GetRefreshTokenSerial(string refreshTokenValue)
        {
            if (string.IsNullOrWhiteSpace(refreshTokenValue))
            {
                return null;
            }

            ClaimsPrincipal decodedRefreshTokenPrincipal = null;
            try
            {
                decodedRefreshTokenPrincipal = new JwtSecurityTokenHandler().ValidateToken(
                    refreshTokenValue,
                    new TokenValidationParameters
                    {
                        RequireExpirationTime = true,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["BearerTokens:Key"])),
                        ValidateIssuerSigningKey = true, // verify signature to avoid tampering
                        ValidateLifetime = true, // validate the expiration
                        ClockSkew = TimeSpan.Zero // tolerance for the expiration date
                    },
                    out _
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to validate refreshTokenValue: `{refreshTokenValue}`.");
            }

            return decodedRefreshTokenPrincipal?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.SerialNumber)?.Value;
        }

        private async Task<(string AccessToken, IEnumerable<Claim> Claims)> createAccessTokenAsync(User user)
        {
            var bearer = _config["BearerTokens:key"];

            var claims = new List<Claim>
            {

                // Unique Id for all Jwt tokes
                new Claim(JwtRegisteredClaimNames.Jti, _securityService.CreateCryptographicallySecureGuid().ToString(), ClaimValueTypes.String, _config["BearerTokens:Issuer"]),
                // Issuer
                new Claim(JwtRegisteredClaimNames.Iss, _config["BearerTokens:Issuer"], ClaimValueTypes.String, _config["BearerTokens:Issuer"]),
                // Issued at
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64, _config["BearerTokens:Issuer"]),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), ClaimValueTypes.String, _config["BearerTokens:Issuer"]),
                new Claim(ClaimTypes.Name, user.UserName ??= string.Empty, ClaimValueTypes.String, _config["BearerTokens:Issuer"]),
                new Claim("displayName", user.DisplayName ??= string.Empty, ClaimValueTypes.String, _config["BearerTokens:Issuer"]),
                new Claim("userId", user.Id.ToString(), ClaimValueTypes.String, _config["BearerTokens:Issuer"]),
                new Claim("userName", user.UserName ??= string.Empty, ClaimValueTypes.String, _config["BearerTokens:Issuer"]),

            };

            // add roles
            var roles = (await _rolesService.GetRolesForUsers(new List<int> { user.Id }));

            claims.Add(new Claim(ClaimTypes.Role, roles.First(x => x.Id == 1).Name, ClaimValueTypes.String, _config["BearerTokens:Issuer"]));


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["BearerTokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var now = DateTime.UtcNow;
            var token = new JwtSecurityToken(
                issuer: _config["BearerTokens:Issuer"],
                audience: _config["BearerTokens:Audience"],
                claims: claims,
                notBefore: now,
                expires: now.AddMinutes(double.Parse(_config["BearerTokens:AccessTokenExpirationMinutes"])),
                signingCredentials: creds);
            return (new JwtSecurityTokenHandler().WriteToken(token), claims);
        }
    }
}