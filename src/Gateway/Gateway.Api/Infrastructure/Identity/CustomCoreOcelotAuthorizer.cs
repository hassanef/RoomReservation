using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Gateway.Api.Infrastructure.Identity
{
    public class CustomCoreOcelotAuthorizer : BaseCoreOcelotAuthorizer
    {
        private readonly IHttpClientFactory _httpClientFactory;
        readonly IConfiguration _configuration;
        public CustomCoreOcelotAuthorizer(IConfiguration configuration,
                                          IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }


        public override bool Authorize(HttpContext context)
        {
            var user = context.User;
            var extractedPath = ExtractPath(context.Request.Path);
            var hasAccess = PerformAuthorize(user, extractedPath.controller, extractedPath.action).GetAwaiter();

            if (hasAccess.GetResult() == false)
                throw new UnauthorizedAccessException("You do not have sufficent access");
            else
                return true;
        }

        public async Task<bool> PerformAuthorize(ClaimsPrincipal user, string controller, string action)
        {
            if (user.IsInRole("Admin"))
            {
                // Admin users have access to all of the pages.
                return true;
            }
            if ((AnonymousAccess.AccountController == controller && AnonymousAccess.LoginAction == action) ||
                (AnonymousAccess.RegisterController == controller && AnonymousAccess.RegisterAction == action))
                return true;

            if (!user.Identity.IsAuthenticated)
                throw new UnauthorizedAccessException("Unauthenticated");

            if (!string.IsNullOrEmpty(controller) || !string.IsNullOrEmpty(action))
            {
                var currentClaimValue = $"{controller}:{action}";
                var httpClient = _httpClientFactory.CreateClient();

                using (var response = await httpClient.GetAsync(new Uri(_configuration.GetSection("SecurityAppUrl").Value + $"/api/v1/Account/Authorize/{currentClaimValue}"), HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();
                    var result = await response.Content.ReadAsStringAsync();

                    if(!string.IsNullOrWhiteSpace(result))
                        return bool.Parse(result);
                }
            }
            return false;
        }

    }
}
