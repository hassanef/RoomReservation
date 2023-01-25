using System.Threading.Tasks;

namespace Reservation.Domain.IRepositories
{
    public interface IRepository<T> : ISaveRepository<T>, IDeleteRepository<T>, IFetchRepository<T> where T : class
    {
        Task<int> SaveChangesAsync();

        void SaveChanges();
    }
}
