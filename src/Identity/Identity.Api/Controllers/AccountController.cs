using Identity.Common.ViewModels;
using Identity.DataLayer.Context;
using Identity.Services.Contracts;
using Identity.Services.Token;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;


namespace Identity.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IApplicationSignInManager _signInManager;
        private readonly IApplicationRoleManager _roleManager;
        private readonly IApplicationUserManager _userManager;
        private readonly ITokenFactoryService _tokenFactoryService;
        private readonly ITokenStoreService _tokenStoreService;
        private readonly IUnitOfWork _uow;


        public AccountController(IApplicationUserManager userManager,
                          IApplicationSignInManager signInManager,
                          IApplicationRoleManager roleManager,
                          ITokenFactoryService tokenFactoryService,
                          ITokenStoreService tokenStoreService,
                          IUnitOfWork uow)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _tokenFactoryService = tokenFactoryService;
            _tokenStoreService = tokenStoreService;
            _uow = uow;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginViewModel loginUser)
        {
            if (loginUser == null)
            {
                return BadRequest("user is not set.");
            }

            var user = await _userManager.FindByNameAsync(loginUser.Username);

            if (user == null || !user.IsActive)
            {
                return NotFound();
            }

            var result1 = await _signInManager.PasswordSignInAsync(
              loginUser.Username,
              loginUser.Password,
              false,
              lockoutOnFailure: true);

            if(result1.Succeeded == false)
            {
                return BadRequest("error in sign in!");
            }

            var result = await _tokenFactoryService.CreateJwtTokensAsync(user);
            await _tokenStoreService.AddUserTokenAsync(user, result.RefreshTokenSerial, result.AccessToken, null);
            await _uow.SaveChangesAsync();

            return Ok(new { access_token = result.AccessToken, refresh_token = result.RefreshToken });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RefreshToken(string refreshTokenValue)
        {
            if (string.IsNullOrWhiteSpace(refreshTokenValue))
            {
                return BadRequest("refreshToken is not set.");
            }

            var token = await _tokenStoreService.FindTokenAsync(refreshTokenValue);
            if (token == null)
            {
                return Unauthorized();
            }

            var result = await _tokenFactoryService.CreateJwtTokensAsync(token.User);
            await _tokenStoreService.AddUserTokenAsync(token.User, result.RefreshTokenSerial, result.AccessToken, _tokenFactoryService.GetRefreshTokenSerial(refreshTokenValue));
            await _uow.SaveChangesAsync();

            return Ok(new { access_token = result.AccessToken, refresh_token = result.RefreshToken });
        }

        [HttpGet("[action]/{currentClaimValue}")]
        public async Task<bool> Authorize(string currentClaimValue)
        {
            return await _roleManager.Authorize(currentClaimValue);
        }
    }
}
