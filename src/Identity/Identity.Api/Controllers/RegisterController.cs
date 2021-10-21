using Identity.Common.ViewModels;
using Identity.Entities.Identity;
using Identity.Services.Contracts;
using Identity.Services.Identity.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly ILogger<RegisterController> _logger;
        private readonly IApplicationUserManager _userManager;
        private readonly IPasswordValidator<User> _passwordValidator;
        private readonly IUserValidator<User> _userValidator;
        private readonly IOptionsSnapshot<SiteSettings> _siteOptions;

        public RegisterController(
            IApplicationUserManager userManager,
            IPasswordValidator<User> passwordValidator,
            IUserValidator<User> userValidator,
            IOptionsSnapshot<SiteSettings> siteOptions,
            ILogger<RegisterController> logger)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _passwordValidator = passwordValidator ?? throw new ArgumentNullException(nameof(passwordValidator));
            _userValidator = userValidator ?? throw new ArgumentNullException(nameof(userValidator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _siteOptions = siteOptions ?? throw new ArgumentNullException(nameof(siteOptions));
        }


        [HttpPost("ValidateUsername")]
        public async Task<IActionResult> ValidateUsername(string username, string email)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest();
            }

            var result = await _userValidator.ValidateAsync(
                (UserManager<User>)_userManager, new User { UserName = username, Email = email });

            if (result.Succeeded)
            {
                return Ok(true);
            }
            return Ok(false);
        }


        [HttpPost("ValidatePassword")]
        public async Task<IActionResult> ValidatePassword(string password, string username)
        {
            var result = await _passwordValidator.ValidateAsync(
                (UserManager<User>)_userManager, new User { UserName = username }, password);

            if (result.Succeeded)
            {
                return Ok(true);
            }
            return Ok(false);
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.Username,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation(3, $"{user.UserName} created a new account with password.");

                    //if (_siteOptions.Value.EnableEmailConfirmation)
                    //{
                    //    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //    //ControllerExtensions.ShortControllerName<RegisterController>(), //todo: use everywhere .................

                    //    await _emailSender.SendEmailAsync(
                    //       email: user.Email,
                    //       subject: "please accept your account",
                    //       viewNameOrPath: "~/Areas/Identity/Views/EmailTemplates/_RegisterEmailConfirmation.cshtml",
                    //       model: new RegisterEmailConfirmationViewModel
                    //       {
                    //           User = user,
                    //           EmailConfirmationToken = code,
                    //           EmailSignature = _siteOptions.Value.Smtp.FromName,
                    //           MessageDateTime = DateTime.UtcNow.ToLongPersianDateTimeString()
                    //       });

                    //    return Ok();
                    //}
                    return Ok();
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return Ok(ModelState);
            }

            return BadRequest();
        }

    }
}
