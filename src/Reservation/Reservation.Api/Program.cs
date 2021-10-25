using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Reservation.Infrastructure.Context;

namespace Reservation.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            host.Services.InitializeDb();
            host.Run();
        }

        public static IWebHostBuilder CreateHostBuilder(string[] args) =>
                 WebHost.CreateDefaultBuilder(args)
              .UseStartup<Startup>();
    }
}
