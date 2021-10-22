using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Gateway.Api.Infrastructure.Identity
{
    public abstract class BaseCoreOcelotAuthorizer
    {
        public abstract bool Authorize(HttpContext context);
        
        public (string controller, string action) ExtractPath(string path)
        {
            var area = string.Empty;
            var controller = string.Empty;
            var action = string.Empty;

            if (path.ToLower().StartsWith("api"))
                return (controller, action);

            var pathParts = path.Split('/');

            ///api/area/Ticket/GetTicketBaseInfo?sth=11
            if (Regex.IsMatch(path, @"(\/api\/)(\w+\/)(\w+\/)(\w+)"))
            {
                controller = pathParts[4];
                action = pathParts[5];
            }
          
            return (controller, action);
        }

        public static string GetSha256Hash(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

    }
}
