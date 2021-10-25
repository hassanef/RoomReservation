using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Domain.Extensions
{
    public static class IdentityExtensions
    {
        public static int GetUserId(this IHttpContextAccessor contextAccessor)
        {
            contextAccessor.HttpContext.Request.Headers.TryGetValue("userId", out Microsoft.Extensions.Primitives.StringValues userId);

            if (!string.IsNullOrWhiteSpace(userId))
                return int.Parse(userId);
            return 0;
        }

    }
}
