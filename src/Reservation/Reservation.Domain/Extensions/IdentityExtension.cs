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
        public static int GetUser(this IHttpContextAccessor contextAccessor)
        {
            var userId = contextAccessor.HttpContext.User.Claims?.Where(x => x.Type == "UserId")?.Single().Value;

            if (!string.IsNullOrWhiteSpace(userId))
                return int.Parse(userId);
            return 0;
        }

    }
}
