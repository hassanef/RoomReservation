using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Reservation.Domain.IRepositories
{
    public interface ISaveRepository<T> where T : class
    {
        void Create(T item);

        Task<T> CreateAsync(T item);

        Task<T> CreateAsyncUoW(T item);

        void Update(T item);

        Task<T> UpdateAsync(T item);

        T UpdateUoW(T item); 

    }
}
