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
        private readonly IApplicationUserManager _userManager;
        private readonly ITokenFactoryService _tokenFactoryService;
        private readonly ITokenStoreService _tokenStoreService;
        private readonly IUnitOfWork _uow;

        public AccountController(IApplicationUserManager userManager,
                          IApplicationSignInManager signInManager,
                          ITokenFactoryService tokenFactoryService,
                          ITokenStoreService tokenStoreService,
                          IUnitOfWork uow)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenFactoryService = tokenFactoryService;
            _tokenStoreService = tokenStoreService;
            _uow = uow;
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginUser)
        {
            if (loginUser == null)
            {
                return BadRequest("user is not set.");
            }

            var user = await _userManager.FindByNameAsync(loginUser.Username);

            if (user == null || !user.IsActive)
            {
                return Unauthorized();
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
      

    }

}
