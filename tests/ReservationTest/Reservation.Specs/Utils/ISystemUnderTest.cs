using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Specs.Utils
{
    public interface ISystemUnderTest
    {
        Task PostAsync<T>(string route, T entity);
        HttpResponseMessage Response { get; }
        Task PutAsync<T>(string route, T entity);
        Task GetAsync(string route);
    }
}
