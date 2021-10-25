using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Api.Infrastructure.Identity
{
    public static class IdentityExtension
    {
        public static StringValues GetUserId(this HttpContext context)
        {
            var userId = context.User.Claims?.Where(x => x.Type == "userId")?.Single().Value;

            return userId;
        }
        public static string GetRoleId(this HttpContext context)
        {
            var userId = context.User.Claims?.Where(x => x.Type == "roleId")?.Single().Value;

            return userId;
        }
    }
}
