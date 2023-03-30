using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Reservation.Infrastructure.Context;
using System;
using System.Net.Http;
using Xunit;

namespace Reservation.Api.IntegrationTests.ApiTests
{

    [CollectionDefinition("Integration")]
    public sealed class IntegrationFixture : IDisposable, ICollectionFixture<IntegrationFixture>
    {
        private readonly TestServer _server;

        public HttpClient Client { get; }

        public IntegrationFixture()
        {
            _server = new TestServer(new WebHostBuilder()
                    .ConfigureAppConfiguration((context, builder) =>
                    {
                        builder.AddJsonFile("appsettings.json");
                    }).UseStartup<Startup>());


            Client = _server.CreateClient();
        }

        public void Dispose()
        {
            Client.Dispose();
            _server.Dispose();
        }
    }
}
