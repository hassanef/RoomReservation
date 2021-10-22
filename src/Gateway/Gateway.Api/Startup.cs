using Gateway.Api.Infrastructure.Identity;
using Gateway.Api.Infrastructure.Middlewares.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Collections.Generic;
using System.Linq;

namespace Gateway.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Gateway.Api", Version = "v1" });
            });

            services.AddMvc(options => options.EnableEndpointRouting = false);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // add custom CoreOcelot Config
            services.AddScoped<CustomCoreOcelotAuthorizer>();

            services.AddCustomAuthentication(Configuration);

            services.AddOcelot();
            services.AddHttpClient();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gateway.Api v1"));
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            //ignore Ocelat ReRoutes
            AppMapWhen(app);

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}


            app.UseMiddleware<AuthorizationMiddleware>();
            app.UseOcelot().Wait();
           // app.UseMvc();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        private void AppMapWhen(IApplicationBuilder app)
        {
            List<string> ignoreList = new List<string>()
            {
                "/robots.txt",
                "/favicon.ico",
                "/api/values",
                "/api/CustomValues",
                "/api/UserAccess",
                "/.well-known"
            };

            //Add the specific route to app.MapWhen To ignore Ocelat route capturing.
            app.MapWhen((context) =>
            {
                return ignoreList.Any(q => context.Request.Path.StartsWithSegments(q));
            }, (appBuilder) =>
            {
                appBuilder.UseMvc();
            });
        }
    }
}
