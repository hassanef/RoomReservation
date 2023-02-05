using BoDi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reservation.Api;
using Reservation.Infrastructure.Context;
using Reservation.Specs.Utils;
using TechTalk.SpecFlow;

namespace Reservation.Specs.Hooks
{
    [Binding]
    public class TestServerHook
    {

        private readonly WebContext _context;
        private static HttpClient _client;

        public TestServerHook(WebContext context, IObjectContainer objectContainer)
        {
            _context = context;
            objectContainer.RegisterInstanceAs<ISystemUnderTest>(context);
        }
        [BeforeTestRun]
        public static async void BeforeTestRun()
        {

            var _server = new TestServer(new WebHostBuilder()
                 .ConfigureAppConfiguration((context, builder) =>
                 {
                     builder.AddJsonFile("appsettings.json");
                 })
                 .ConfigureServices(services =>
                 {
                     services.AddEntityFrameworkSqlServer()
                             .AddDbContext<ReservationDbContext>(options =>
                             //TODO
                             options.UseSqlServer("data source=127.0.0.1,5434;initial catalog=ReservationDBTest;user=sa;password=Your_password123;"));

                 }).UseStartup<Startup>());

            //TODO
            _server.Host.Services.InitializeDb();
            var dbContext = _server.Host.Services.GetService<ReservationDbContext>();
            if (dbContext != null)
                await dbContext.Database.ExecuteSqlRawAsync("Delete [ReservationDBTest].[dbo].[RoomReservations]");

            _client = _server.Host.GetTestClient();
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            _context.Client = _client;
        }

        [AfterTestRun]
        public static void DeleteData()
        {
            //something to remove the data
        }

    }
}
