using Microsoft.AspNetCore.Http;

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
