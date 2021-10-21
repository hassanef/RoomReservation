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
        
        public RegisterController(
            IApplicationUserManager userManager,
            ILogger<RegisterController> logger)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
