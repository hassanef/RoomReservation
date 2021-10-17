using System.Threading.Tasks;

namespace Reservation.Domain.IRepositories
{
    public interface IDeleteRepository<T> where T : class
    {
        bool DeleteUoW(T item);
        void Delete(T item);
        Task DeleteAsync(T item);

    }

}
