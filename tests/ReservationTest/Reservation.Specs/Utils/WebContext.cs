using System.Net.Http.Json;

namespace Reservation.Specs.Utils
{
    public class WebContext : ISystemUnderTest
    {
        public HttpClient Client { get; set; }


        public async Task PostAsync<T>(string route, T entity)
        {
            Client.DefaultRequestHeaders.Add("userId", "1");
            Response = await Client.PostAsync(route, JsonContent.Create(entity));
        }

        public HttpResponseMessage Response { get; private set; }

        public async Task PutAsync<T>(string route, T entity)
        {
            Response = await Client.PutAsync(route, JsonContent.Create(entity));
        }

        public async Task GetAsync(string route)
        {
            Response = await Client.GetAsync(route);
        }
    }
}
