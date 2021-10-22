using Gateway.Api.Infrastructure.Identity;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Gateway.Api.Infrastructure.Middlewares.Authorization
{

    public class AuthorizationMiddleware
    {
        private RequestDelegate nextMiddleware;
        public AuthorizationMiddleware(RequestDelegate next)
        {
            this.nextMiddleware = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context == null) return;

            CustomCoreOcelotAuthorizer customCoreOcelotAuthorizer = (CustomCoreOcelotAuthorizer)context.RequestServices.GetService(typeof(CustomCoreOcelotAuthorizer));

            if(customCoreOcelotAuthorizer != null)
                customCoreOcelotAuthorizer.Authorize(context);

            await this.nextMiddleware.Invoke(context);
        }
    }
}
