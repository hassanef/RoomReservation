using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Application.Infrastructure.TokenConfiguration
{
    public static class TokenExtension
    {
        public static IServiceCollection AddCustomTokenAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
             .AddJwtBearer(cfg =>
             {
                 cfg.SaveToken = false;
                 cfg.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidIssuer = configuration["BearerTokens:Issuer"], // site that makes the token
                         ValidateIssuer = true, // TODO: change this to avoid forwarding attacks
                         ValidAudience = configuration["BearerTokens:Audience"], // site that consumes the token
                         ValidateAudience = true, // TODO: change this to avoid forwarding attacks
                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["BearerTokens:Key"])),
                     ValidateIssuerSigningKey = true, // verify signature to avoid tampering
                         ValidateLifetime = true, // validate the expiration
                         ClockSkew = TimeSpan.Zero // tolerance for the expiration date
                     };
                 //cfg.Events = new JwtBearerEvents
                 //{
                 //    OnAuthenticationFailed = context =>
                 //    {
                 //        var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(JwtBearerEvents));
                 //        logger.LogError("Authentication failed.", context.Exception);

                 //        throw new UnauthorizedAccessException("Authentication Failed ");
                 //    },
                 //    OnTokenValidated = context =>
                 //    {
                 //            //TODO
                 //            //var tokenValidatorService = context.HttpContext.RequestServices.GetRequiredService<ITokenValidatorService>();
                 //            //return tokenValidatorService.ValidateAsync(context);
                 //            return Task.CompletedTask;
                 //    },
                 //    OnMessageReceived = context =>
                 //    {
                 //        return Task.CompletedTask;
                 //    },
                 //    OnChallenge = context =>
                 //    {
                 //        var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(JwtBearerEvents));
                 //        logger.LogError("OnChallenge error", context.Error, context.ErrorDescription);
                 //        return Task.CompletedTask;
                 //    }
                 //};
             });

            return services;
        }

    }
}
