using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;
using System.Text;

namespace Identity.Common.IdentityToolkit
{
    public static class IdentityExtensions
    {
        public const string ValidationErrorCode= "ValidationError";


        public static void AddErrorsFromResult(this ModelStateDictionary modelStat, IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                modelStat.AddModelError("", error.Description);
            }
        }

        public static string DumpErrors(this IdentityResult result, bool useHtmlNewLine = false)
        {
            var results = new StringBuilder();
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    var errorDescription = error.Description;
                    if (string.IsNullOrWhiteSpace(errorDescription))
                    {
                        continue;
                    }

                    if (!useHtmlNewLine)
                    {
                        results.AppendLine(errorDescription);
                    }
                    else
                    {
                        results.Append(errorDescription).AppendLine("<br/>");
                    }
                }
            }
            return results.ToString();
        }


        public static int GetRoleId(this IHttpContextAccessor contextAccessor)
        {
            contextAccessor.HttpContext.Request.Headers.TryGetValue("RoleId", out Microsoft.Extensions.Primitives.StringValues userId);

            if (!string.IsNullOrWhiteSpace(userId))
                return int.Parse(userId);
            return 0;
        }
    }
}