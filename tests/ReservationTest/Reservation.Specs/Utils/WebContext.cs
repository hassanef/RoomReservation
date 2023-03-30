using SpecFlow.Internal.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Specs.Utils
{
    public class WebContext : ISystemUnderTest
    {
        public HttpClient Client { get; set; }
        

        public async Task PostAsync<T>(string route, T entity)
        {
            Client.DefaultRequestHeaders.Add("userId", "1");
            Response = await Client.PostAsync(route, new StringContent(entity.ToJson(), Encoding.UTF8, "application/json"));
        }

        public HttpResponseMessage Response { get; private set; }

        public async Task PutAsync<T>(string route, T entity)
        {
            Response = await Client.PutAsync(route, new StringContent(entity.ToJson(), Encoding.UTF8, "application/json"));
        }

        public async Task GetAsync(string route)
        {
            Response = await Client.GetAsync(route);
        }
    }
}
